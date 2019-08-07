VERSION 5.00
Object = "{E7BC34A0-BA86-11CF-84B1-CBC2DA68BF6C}#1.0#0"; "ntsvc.ocx"
Begin VB.Form frmMain 
   ClientHeight    =   615
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   1560
   LinkTopic       =   "Form1"
   ScaleHeight     =   615
   ScaleWidth      =   1560
   StartUpPosition =   3  'Windows Default
   Begin NTService.NTService NTService 
      Left            =   600
      Top             =   120
      _Version        =   65536
      _ExtentX        =   741
      _ExtentY        =   741
      _StockProps     =   0
      ServiceName     =   "Simple"
      StartMode       =   3
   End
   Begin VB.Timer Temporizador 
      Enabled         =   0   'False
      Left            =   120
      Top             =   120
   End
End
Attribute VB_Name = "frmMain"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private strConexao, strSQL, strDiretorioBase, strPasta As String
Private objFSO As FileSystemObject

Private Enum enumRotina
    Iniciar = 1
    Parar = 2
End Enum
Private Sub Form_Load()
    'Call IniciaExclusao
    IniciaServico
End Sub
Private Sub IniciaServico()

    On Error GoTo Trata
    
    NTService.DisplayName = App.ProductName
    NTService.ServiceName = NTService.DisplayName

    Select Case LCase(Command)
        
        Case "-instalar"
            If NTService.Install Then
                MsgBox "Serviço instalado com sucesso.", vbInformation + vbOKOnly, App.Title
                Call GravarEvento("Serviço " & App.Title & " instalado com sucesso.", svcEventSuccess)
            Else
                MsgBox "Erro ao executar tarefa requisitada:" & vbCrLf & vbCrLf & _
                        Err.Description
            End If
            
            End
            
        Case "-remover"
            If NTService.Uninstall Then
                MsgBox "Serviço removido com sucesso.", vbInformation + vbOKOnly, App.Title
                Call GravarEvento("Serviço " & App.Title & " removido com sucesso.", svcEventSuccess)
            Else
                MsgBox "Erro ao executar tarefa requisitada:" & vbCrLf & vbCrLf & _
                        Err.Description
            End If
            
            End
        
        Case "-debug"
            NTService.Debug = True
            NTService.Interactive = True
            
    End Select
        
    If Trim(LCase(Command)) <> "" Then
        MsgBox "Comando inválido", vbInformation + vbOKOnly, App.Title
        End
    End If
    
    With NTService
        .Interactive = False
        .ControlsAccepted = svcCtrlStartStop
        .StartMode = svcStartAutomatic
        .StartService
    End With
    
    Exit Sub
    
Trata:

    Call GravarEvento("Método IniciaServico - " & CStr(Err.Number) & " - " & CStr(Err.Description), svcEventError)

End Sub
Private Sub IniciaExclusao()
    
    On Error GoTo Trata
    
    Call GravarEvento("Exclusões iniciadas", svcEventSuccess)
    Call CarregaConfiguracao
    
    Dim objConexao, objRs As Object
    
    Set objConexao = CreateObject("ADODB.Connection")
    objConexao.Open (strConexao)
    
    Set objFSO = New FileSystemObject
    
    'exclusoes externas
    strSQL = "SELECT DISTINCT 'externas\' + LTRIM(RTRIM(CAST(C.ACOD_FORNECEDOR_BAAN AS CHAR))) + '\recebidas\' + LTRIM(RTRIM(CAST(A.ANUM_SEQU_REGISTRO AS CHAR))) AS PASTA, " & _
            " A.ANUM_SEQU_REGISTRO, A.ANUM_SEQU_FORNECEDOR" & _
            " FROM TREGISTRO_TRANSMISSAO A, TARQUIVO B, TFORNECEDOR C" & _
            " WHERE B.ANUM_SEQU_REGISTRO = A.ANUM_SEQU_REGISTRO" & _
            " AND C.ANUM_SEQU_FORNECEDOR = B.ANUM_SEQU_FORNECEDOR" & _
            " AND A.ABOL_EXCLUSAO = 0" & _
            " AND DATEDIFF(DAY, A.ADAT_PROTOCOLO, GETDATE()) > (" & _
            " SELECT ADAT_EXCLUSAO FROM TCONFIGURACAO)"
    Set objRs = objConexao.execute(strSQL)
    
    If Not objRs.EOF Then
    
        Do While Not objRs.EOF
        
            strPasta = Trim(objRs!PASTA)
        
            Call objFSO.DeleteFolder(strDiretorioBase & "\" & strPasta, True)
            
            strSQL = "UPDATE TREGISTRO_TRANSMISSAO SET ABOL_EXCLUSAO = 1" & _
                    " WHERE ANUM_SEQU_REGISTRO = " & CInt(objRs!ANUM_SEQU_REGISTRO) & _
                    " AND ANUM_SEQU_FORNECEDOR = " & CInt(objRs!ANUM_SEQU_FORNECEDOR)
            objConexao.execute (strSQL)
            
        objRs.MoveNext
        Loop
    
    End If
    
    'exclusoes internas
    strSQL = "SELECT ANUM_SEQU_REGISTRO_INTERNO, 'internas\' + ADES_EMAIL_USER + '\recebidas\' + LTRIM(RTRIM(CAST(ANUM_SEQU_REGISTRO_INTERNO AS CHAR))) AS PASTA" & _
            " FROM TREGISTRO_TRANS_INTERNO" & _
            " WHERE DATEDIFF(DAY, ADAT_ENVIO, GETDATE()) > (" & _
            " SELECT ADAT_EXCLUSAO FROM TCONFIGURACAO)"
    Set objRs = objConexao.execute(strSQL)
    
    If Not objRs.EOF Then
    
        Do While Not objRs.EOF
            
            strPasta = Trim(objRs!PASTA)
            Call objFSO.DeleteFolder(strDiretorioBase & strPasta, True)
            
        objRs.MoveNext
        Loop
    
    End If
    
    'exclusoes de chaves temporarias
    Dim objFolders
    Dim objFolder As Folder
    Set objFolders = objFSO.GetFolder(strDiretorioBase & "temporarias")
    
    For Each objFolder In objFolders.SubFolders
    
        strPasta = Trim(objFolder.Name)
        
        strSQL = "SELECT ANOM_CHAVE_TEMPORARIA" & _
                " FROM TCHAVE_TEMPORARIA" & _
                " WHERE ANOM_CHAVE_TEMPORARIA = '" & strPasta & "'"
        Set objRs = objConexao.execute(strSQL)
        
        If objRs.EOF Then 'chave já foi acessada (nao existe no banco) entao exclui-se o diretorio
            objFSO.DeleteFolder (strDiretorioBase & "\temporarias\" & strPasta)
        End If

    Next
    
    Set objRs = Nothing
    Set objFSO = Nothing
    
    objConexao.Close
    Set objConexao = Nothing
    
    Call GravarEvento("Exclusões finalizadas", svcEventSuccess)
    
    Exit Sub

Trata:
    
    If Err.Number = 76 Then
        Call GravarLogErro("A pasta " & strPasta & " não encontrada para exclusão")
        Resume Next
    Else
        Call GravarEvento("Método IniciaExclusao - " & Err.Number & " - " & Trim(Err.Description) & " - " & strPasta, svcEventError)
    End If

End Sub
Private Sub GravarLogErro(ByVal strErro As String)
    
    Dim objFSOAux As FileSystemObject
    Set objFSOAux = New FileSystemObject
    Dim arqAux
    Set arqAux = objFSOAux.OpenTextFile(App.Path & "\logExclusao.log", ForAppending, True)
    
    Call arqAux.WriteLine(Format(Now, "dd/mm/yyyy hh:nn") & " - " & strErro)
    
    arqAux.Close
    Set objFSOAux = Nothing

End Sub
Private Sub CarregaConfiguracao()
    
    Dim arquivoTexto, linha
    Dim intLinha As Integer
    Dim strSQLUsuario As String
    Dim strSQLSenha As String
    Dim strSQLServidor As String
    Dim strSQLBanco As String
    
    arquivoTexto = FreeFile()
    
    Open App.Path & "\configuracao.ini" For Input As arquivoTexto
    
    'Agora vamos ver se o usuário logado está no arquivo texto
    Do While Not EOF(arquivoTexto)
        Line Input #arquivoTexto, linha
        
        If intLinha = 7 Then
            strSQLUsuario = Trim(linha)
        ElseIf intLinha = 8 Then
            strSQLSenha = Trim(linha)
        ElseIf intLinha = 9 Then
            strSQLServidor = Trim(linha)
        ElseIf intLinha = 10 Then
            strSQLBanco = Trim(linha)
        ElseIf intLinha = 11 Then
            strDiretorioBase = Trim(linha)
        End If
        
        intLinha = intLinha + 1
    Loop

    strConexao = "Provider=SQLOLEDB.1;User Id=" & strSQLUsuario & _
                ";Pwd=" & strSQLSenha & _
                ";Data Source=" & strSQLServidor & _
                ";Initial Catalog=" & strSQLBanco

    Close #arquivoTexto

End Sub
Private Sub NTService_Start(Success As Boolean)

    Success = True
    
    Call IniciaExclusao
    
    Exit Sub
    
Trata:

    Call GravarEvento("Método NTService_Start - " & CStr(Err.Number) & " - " & CStr(Err.Description), svcEventError)

End Sub
Private Sub RotinasTemporizador(ByVal intCodRotina As enumRotina)

    'Habilita ou Desabilita o Temporizador

    Select Case CInt(intCodRotina)
        Case 1
            Temporizador.Enabled = True
            Temporizador.Interval = 60000
        
        Case 2
            Temporizador.Enabled = False
        
    End Select

End Sub
Private Sub GravarEvento(ByVal strMensagem As String, _
                        ByVal tipLog As SvcEventType)
    
    Call NTService.LogEvent(tipLog, svcMessageError, strMensagem)
    
End Sub
Private Sub Timer_Timer()

    Call RotinasTemporizador(Parar)
    Call IniciaExclusao
    Call RotinasTemporizador(Iniciar)

End Sub
