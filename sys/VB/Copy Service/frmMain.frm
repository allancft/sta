VERSION 5.00
Object = "{48E59290-9880-11CF-9754-00AA00C00908}#1.0#0"; "MSINET.OCX"
Object = "{E7BC34A0-BA86-11CF-84B1-CBC2DA68BF6C}#1.0#0"; "ntsvc.ocx"
Begin VB.Form frmMain 
   ClientHeight    =   1260
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   4035
   ScaleHeight     =   1260
   ScaleWidth      =   4035
   StartUpPosition =   3  'Windows Default
   Begin VB.Timer objTemporizador 
      Enabled         =   0   'False
      Left            =   1080
      Top             =   0
   End
   Begin InetCtlsObjects.Inet objNet 
      Left            =   0
      Top             =   0
      _ExtentX        =   1005
      _ExtentY        =   1005
      _Version        =   393216
      AccessType      =   1
      Protocol        =   2
      RemotePort      =   21
      URL             =   "ftp://"
      RequestTimeout  =   10
   End
   Begin NTService.NTService objServicoNT 
      Left            =   600
      Top             =   0
      _Version        =   65536
      _ExtentX        =   741
      _ExtentY        =   741
      _StockProps     =   0
      DisplayName     =   "STA - Serviço de Cópia de Arquivos"
      ServiceName     =   "STA - Serviço de Cópia de Arquivos"
      StartMode       =   2
   End
   Begin VB.Label lblDebug 
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   13.5
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   495
      Left            =   0
      TabIndex        =   0
      Top             =   600
      Width           =   3975
   End
End
Attribute VB_Name = "frmMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private objFS As FileSystemObject
Private strMsgAux As String
Private strSQL As String
Private objConexao As Object

Private Enum enumRotina
    Iniciar = 1
    Parar = 2
End Enum
Private Sub Form_Load()
    'Call CarregaConfiguracao
    'Call Transferencias
    Call IniciaServico
End Sub
Private Function AbreConexao() As Boolean

    On Error GoTo Trata
    
    Set objConexao = CreateObject("ADODB.Connection")
    objConexao.Open StrConexao
    objConexao.CursorLocation = 3 'adUseClient

    AbreConexao = True
    
    Exit Function

Trata:
    AbreConexao = False
    Call GravarEvento(CStr(Err.Number) & " - " & CStr(Err.Description), svcEventError)
    
End Function
Private Sub FechaConexao()

    On Error GoTo Trata
    
    If objConexao.State = 1 Then
        objConexao.Close
    End If
    
    Exit Sub

Trata:
    Call GravarEvento(CStr(Err.Number) & " - " & CStr(Err.Description), svcEventError)

End Sub
Private Sub IniciaServico()
    
    On Error GoTo Trata
    
    objServicoNT.DisplayName = App.ProductName
    objServicoNT.ServiceName = objServicoNT.DisplayName

    Select Case LCase(Command)
        
        Case "-instalar"
            If objServicoNT.Install Then
                MsgBox "Serviço instalado com sucesso.", vbInformation + vbOKOnly, App.Title
                Call GravarEvento("Serviço " & App.Title & " instalado com sucesso.", svcEventSuccess)
            Else
                MsgBox "Erro ao executar tarefa requisitada:" & vbCrLf & vbCrLf & _
                        Err.Description
            End If
            
            End
            
        Case "-remover"
            If objServicoNT.Uninstall Then
                MsgBox "Serviço removido com sucesso.", vbInformation + vbOKOnly, App.Title
                Call GravarEvento("Serviço " & App.Title & " removido com sucesso.", svcEventSuccess)
            Else
                MsgBox "Erro ao executar tarefa requisitada:" & vbCrLf & vbCrLf & _
                        Err.Description
            End If
            
            End
        
        Case "-debug"
            objServicoNT.Debug = True
            objServicoNT.Interactive = True
            
    End Select
        
    If Trim(LCase(Command)) <> "" Then
        MsgBox "Comando inválido", vbInformation + vbOKOnly, App.Title
        End
    End If
    
    With objServicoNT
        .Interactive = False
        .ControlsAccepted = svcCtrlStartStop
        .StartMode = svcStartAutomatic
        .StartService
    End With
    
    Exit Sub
    
Trata:
    Call GravarEvento(CStr(Err.Number) & " - " & CStr(Err.Description), svcEventError)
    End

End Sub
Private Sub GravarEvento(ByVal strMensagem As String, _
                        ByVal tipLog As SvcEventType)
    
    Call objServicoNT.LogEvent(tipLog, svcMessageError, strMensagem)
    
End Sub
Private Sub RotinasTemporizador(ByVal intCodRotina As enumRotina)

    Select Case CInt(intCodRotina)
        Case 1
            With objTemporizador
                .Enabled = True
                .Interval = 60000
            End With
        
        Case 2
            objTemporizador.Enabled = False
        
    End Select

End Sub
Private Sub Transferencias()
        
    On Error GoTo Trata
    
    Dim objFolders, objFoldersCodigo
    Dim objFolder, objFolderCodigo As Folder
    Dim objArquivo As File
    Dim strComandoAux As String
    Dim arrPastas As Variant
    Dim arrPastasRetorno As Variant
    ReDim arrPastasRetorno(0)
    Dim intIndice As Integer
   
    Set objFS = New FileSystemObject
    
    'Trata se existe o diretório no IIS de onde são copiados os arquivos
    'arquivos do IIS para o BANN e do BAAN para a o IIS.
    If Not (objFS.FolderExists(StrDiretorioIIS)) Then
        Call GravarEvento("Diretório do IIS " & StrDiretorioIIS & " não existe", svcEventError)
        End
    End If
    
    Call AbreFTP
    Call AbreConexao
    
    '### Inicia copia das Transportadoras do IIS para o BAAN
    Set objFolders = objFS.GetFolder(StrDiretorioIIS)
    For Each objFolder In objFolders.SubFolders
    
        arrPastasRetorno(UBound(arrPastasRetorno)) = objFolder.Name
        ReDim Preserve arrPastasRetorno(UBound(arrPastasRetorno) + 1)
    
        Set objFoldersCodigo = objFS.GetFolder(objFolder & "\recebidas")
        For Each objFolderCodigo In objFoldersCodigo.SubFolders
        
            Call ComandoFTP("cd " & StrDiretorioBase)
            
            'Cria o diretório
            strComandoAux = Replace(objFolderCodigo.Path, _
                            objFolderCodigo.Drive.Path, "")
            strComandoAux = Right(strComandoAux, (Len(strComandoAux) - 1))
            
            arrPastas = Split(strComandoAux, "\")
            
            Call ComandoFTP("mkdir " & arrPastas(1))
            Call ComandoFTP("cd " & arrPastas(1))
            Call ComandoFTP("mkdir " & arrPastas(2))
            Call ComandoFTP("cd " & arrPastas(2))
            
            For Each objArquivo In objFolderCodigo.Files 'Envia o Arquivo
                If objArquivo.Name <> "" Then
                    Call ComandoFTP("put " & objArquivo.Path & _
                                Space(1) & _
                                objArquivo.Name)
                End If
            Next
        Next
    Next
    
    '### Inicia copia das Transportadores do BAAN para o IIS
    Dim data As Variant
    Dim arrArquivos As Variant
    Dim strArquivo As String
    Dim intIndiceArquivos As Integer
    For intIndice = 0 To (UBound(arrPastasRetorno) - 1)
        Call ComandoFTP("cd " & StrDiretorioBase) 'Volta para o diretório raíz
        Call ComandoFTP("cd " & arrPastasRetorno(intIndice))
        Call ComandoFTP("mkdir enviadas")
        Call ComandoFTP("mkdir backup")
        Call ComandoFTP("cd enviadas")
        Call ComandoFTP("ls")
        data = objNet.GetChunk(1024, icString)
        arrArquivos = Split(data, vbCrLf)
        
        Dim blnTemArquivo As Boolean
        Dim objRs
        strSQL = "SELECT CASE" & _
            " WHEN (MAX(A.ANUM_SEQU_REGISTRO) > MAX(B.ANUM_SEQU_REGISTRO_INTERNO))" & _
            " THEN ISNULL(MAX(A.ANUM_SEQU_REGISTRO), 0) + 1" & _
            " ELSE ISNULL(MAX(B.ANUM_SEQU_REGISTRO_INTERNO), 0) + 1" & _
            " END AS CONTADOR" & _
            " FROM TREGISTRO_TRANSMISSAO A, TREGISTRO_TRANS_INTERNO B"

        Set objRs = objConexao.Execute(strSQL)
        
        Dim intCodigo As Integer
        intCodigo = CInt(0 & objRs("CONTADOR"))
        
        Set objRs = Nothing
        
        For intIndiceArquivos = 0 To (UBound(arrArquivos) - 1)
            strArquivo = Trim(arrArquivos(intIndiceArquivos))
            
            If strArquivo <> "" Then
                blnTemArquivo = True
                objFS.CreateFolder (StrDiretorioIIS & arrPastasRetorno(intIndice) & _
                            "\recebidas")
                
                objFS.CreateFolder (StrDiretorioIIS & arrPastasRetorno(intIndice) & _
                            "\recebidas" & _
                            "\" & intCodigo)
                'copia para iis
                Call ComandoFTP("get " & arrArquivos(intIndiceArquivos) & Space(1) & _
                                StrDiretorioIIS & arrPastasRetorno(intIndice) & _
                                "\recebidas" & _
                                "\" & intCodigo & "\" & _
                                arrArquivos(intIndiceArquivos))
                'faz backup
                Call ComandoFTP("cd ..")
                Call ComandoFTP("cd backup")
                Call ComandoFTP("put " & StrDiretorioIIS & arrPastasRetorno(intIndice) & _
                               "\recebidas\" & _
                               intCodigo & "\" & _
                                arrArquivos(intIndiceArquivos) & Space(1) & _
                                Format(Now, "dd-mm-yyyy-hh-mm-ss") & "_" & arrArquivos(intIndiceArquivos))
                Call ComandoFTP("cd ..")
                Call ComandoFTP("cd enviadas")
                Call ComandoFTP("delete " & arrArquivos(intIndiceArquivos))
            End If
        Next
        If blnTemArquivo Then
            strSQL = "SELECT ANUM_SEQU_FORNECEDOR, ADES_EMAIL_FORNECEDOR " & _
                    "FROM TFORNECEDOR " & _
                    "WHERE ACOD_FORNECEDOR_BAAN = '" & arrPastasRetorno(intIndice) & "'"
            Set objRs = objConexao.Execute(strSQL)
            
            strSQL = "INSERT INTO TREGISTRO_TRANSMISSAO (ANUM_SEQU_FORNECEDOR, " & _
                    "ANUM_SEQU_REGISTRO, ATIP_PROTOCOLO, ABOL_VALIDACAO_PROTOCOLO, " & _
                    "ADAT_PROTOCOLO, ABOL_EXCLUSAO) VALUES(" & _
                    Trim(objRs("ANUM_SEQU_FORNECEDOR")) & ", " & _
                    intCodigo & _
                    ", 1, 0, getdate(), 0)"
            objConexao.Execute (strSQL)
            
            blnTemArquivo = False
            
            'Call EnviaEmail(intCodigo, Trim(objRs("ADES_EMAIL_FORNECEDOR")))
            
            Set objRs = Nothing
        End If
    Next
    
    Call FechaFTP
    Call FechaConexao
    
    Exit Sub

Trata:
    Call GravarEvento(CStr(Err.Number) & " - " & Err.Description, svcEventError)
    Resume Next

End Sub
Private Sub AbreFTP()

    On Error GoTo Trata
    
    With objNet
        .RemoteHost = Trim(StrFTPServidor)
        .UserName = Trim(StrFTPUsuario)
        .Password = Trim(StrFTPSenha)
    End With
    
    strMsgAux = "Transferências iniciadas"
    Call GravarEvento(strMsgAux, svcEventSuccess)
    
    Exit Sub
    
Trata:
    Call GravarEvento(CStr(Err.Number) & " - " & Err.Description, svcEventError)
    
End Sub
Private Sub FechaFTP()
    
    On Error GoTo Trata

    Call objNet.Execute(StrURL, "bye")
    
    strMsgAux = "Transferências concluídas"
    Call GravarEvento(strMsgAux, svcEventSuccess)
    
    Exit Sub
    
Trata:
    Call GravarEvento(CStr(Err.Number) & " - " & Err.Description, svcEventError)

End Sub
Private Sub ComandoFTP(ByVal strComando As String)
    Call AguardaFTP
    Call objNet.Execute(, strComando)
    Call AguardaFTP
End Sub
Private Sub AguardaFTP()

  Dim blnAguarda As Boolean
  
  On Error GoTo Trata
 
  blnAguarda = True
  
  Do Until Not blnAguarda
        DoEvents
        blnAguarda = objNet.StillExecuting
  Loop
  
  Exit Sub
 
Trata:
  Err.Clear

End Sub
Private Sub objNet_StateChanged(ByVal State As Integer)
    'Tratamento de erro e debug das conexões FTP.
    'Só é executado se o Serviço NT estiver com as opções
    'de Interativo com o Desktop e Debug setadas para true.

    If objServicoNT.Interactive And objServicoNT.Debug Then
    
        On Error Resume Next
        
        Select Case State
            Case icNone
            Case icResolvingHost:      Me.lblDebug.Caption = "Procurando servidor"
            Case icHostResolved:       Me.lblDebug.Caption = "Servidor encontrado"
            Case icConnecting:         Me.lblDebug.Caption = "Conectando..."
            Case icConnected:          Me.lblDebug.Caption = "Conectado"
            Case icResponseReceived:   Me.lblDebug.Caption = "Transferindo arquivo (s)"
            Case icDisconnecting:      Me.lblDebug.Caption = "Desconectando..."
            Case icDisconnected:       Me.lblDebug.Caption = "Desconectado"
            Case icError:              MsgBox "Erro: " & objNet.ResponseCode & " " & objNet.ResponseInfo
            Case icResponseCompleted:  Me.lblDebug.Caption = "Tarefa finalizada."
        End Select
        
        Me.lblDebug.Visible = True
        Me.lblDebug.Refresh
        
        Err.Clear
    
    End If
End Sub
Private Sub EnviaEmail(ByVal intCodigoTransf As Integer, _
                        ByVal strEmail As String)

    On Error GoTo Trata

    Dim objMail
    Set objMail = CreateObject("CDONTS.NewMail")
    
    Dim strTextoMensagem As String
    strTextoMensagem = "Você está recebendo este e-mail, em notificação"
    strTextoMensagem = strTextoMensagem & " ao recebimento de arquivos para cópia através do"
    strTextoMensagem = strTextoMensagem & " Sistema de Transferência de Arquivos da Itambé." & vbCrLf & vbCrLf
    strTextoMensagem = strTextoMensagem & "O código da transferência é " & intCodigoTransf & vbCrLf & vbCrLf
    strTextoMensagem = strTextoMensagem & "Clique no link abaixo para ter acesso:" & vbCrLf & vbCrLf
    strTextoMensagem = strTextoMensagem & StrURLSite

    With objMail
        .From = "sta@itambe.com.br"
        .To = strEmail
        .Body = strTextoMensagem
        .Priority = 1
        .Format = 1
        .send
    End With
    
    Set objMail = Nothing

Trata:
    Call GravarEvento(CStr(Err.Number) & " - " & Err.Description, svcEventError)

End Sub
Private Sub objServicoNT_Start(Success As Boolean)
    
    Success = True
    
    Call RotinasTemporizador(Parar)
    Call CarregaConfiguracao
    Call Transferencias
    Call RotinasTemporizador(Iniciar)
    
End Sub
Private Sub objTemporizador_Timer()

    Call RotinasTemporizador(Parar)
    Call Transferencias
    Call RotinasTemporizador(Iniciar)

End Sub
