#Region "Sobre este Aplicativo"
'***************************************************************************
'* Itamb� - CCPR / Dez 2004.
'* -------------------------
'* Servico de C�pia de Arquivos atrav�s de FTP
'*
'* Objetivo: Realizar c�pia de arquivos de transacoes realizadas
'*           atraves do Sistema de Transferencia de Arquivos (STA)
'*           levando-os do ambiente Windows (Web/IIS) para o ambiente
'*           UNIX (Baan)
'*
'* Particularidades: O usu�rio que executa o servico deve ter acesso
'*                   de escrita (write) no drive ou share de rede configurado
'*                   como raiz das exclusoes.
'*
'*                   A instala��o se d� atraves da ferramenta InstallUtil.exe
'*                   do ".Net Framework" da Microsoft. Por padronizacao, este
'*                   utilitario encontra-se no diretorio:
'*                   <windir>\Microsoft.Net\Framework\<versao do framework>
'* -------------------------------------------------------------------------
'* Autor: Raphael Jorge dos Santos
'* Email: rsantos@aim.com.br
'* Empresa: Aim Inform�tica Ltda
'* Telefone: 031 3281-4341
'***************************************************************************
#End Region

Imports System.IO
Imports System.Data
Imports System.Web.Mail
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.ServiceProcess
Imports EnterpriseDT.Net.Ftp
Public Class svcCopia
  Inherits System.ServiceProcess.ServiceBase

#Region " Component Designer generated code "

  Public Sub New()
    MyBase.New()

    ' This call is required by the Component Designer.
    InitializeComponent()

    ' Add any initialization after the InitializeComponent() call

  End Sub

  'UserService overrides dispose to clean up the component list.
  Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
    If disposing Then
      If Not (components Is Nothing) Then
        components.Dispose()
      End If
    End If
    MyBase.Dispose(disposing)
  End Sub

  ' The main entry point for the process
  <MTAThread()> _
  Shared Sub Main()
    Dim ServicesToRun() As System.ServiceProcess.ServiceBase

    ' More than one NT Service may run within the same process. To add
    ' another service to this process, change the following line to
    ' create a second service object. For example,
    '
    '   ServicesToRun = New System.ServiceProcess.ServiceBase () {New Service1, New MySecondUserService}
    '
    ServicesToRun = New System.ServiceProcess.ServiceBase() {New svcCopia}

    System.ServiceProcess.ServiceBase.Run(ServicesToRun)
  End Sub

  'Required by the Component Designer
  Private components As System.ComponentModel.IContainer

  ' NOTE: The following procedure is required by the Component Designer
  ' It can be modified using the Component Designer.  
  ' Do not modify it using the code editor.

  <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
    Me.objTimer = New System.Timers.Timer
    CType(Me.objTimer, System.ComponentModel.ISupportInitialize).BeginInit()
    '
    'objTimer
    '
    Me.objTimer.Enabled = True
    '
    'svcCopia
    '
    Me.ServiceName = "STA - C�pia"
    CType(Me.objTimer, System.ComponentModel.ISupportInitialize).EndInit()

  End Sub

#End Region

#Region " Vari�veis do Aplicativo "
  Private strNomeApp As String = "STA - C�pia"
  Private strSQL As String
  Private strDepto As String
  Private strCompanhia As String
  Private strAplicativo As String
  Private strArquivo As String
  Private strCodTransp As String
  Private strCodTransf As String
  Private arrDirTransf As Array 'Array com toda a estrutura de diret�rios de uma transferencia
#End Region

#Region " Vari�veis de Configura��o "
  Private strDirRaizIIS As String
  Private strArquivoLog As String
  Private strFTPUsuario As String
  Private strFTPSenha As String
  Private strFTPServidor As String
  Private strServidorCorreio As String
  Private strSQLBanco As String
  Private strSQLServidor As String
  Private strSQLUsuario As String
  Private strSQLSenha As String
  Private strURLSite As String
#End Region

#Region " Objetos "
  Private objFTP As FTPClient = New FTPClient
  Private objDB As clsDB = New clsDB
  Private WithEvents objTimer As System.Timers.Timer
#End Region

  Protected Overrides Sub OnStart(ByVal args() As String)
    'Aguarda 10 segundos para iniciar a execucao apos iniciar o Servico
    objTimer.Interval = 10 * 1000
    objTimer.Enabled = True
  End Sub

  Protected Overrides Sub OnStop()
    objTimer.Enabled = False
  End Sub

  Private Sub objTimer_Elapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles objTimer.Elapsed
    objTimer.Stop()
    Call iniciaCopia()
    objTimer.Start()
  End Sub

  Private Sub iniciaCopia()
    Try
      'Seta o timer para rodar o aplicativo a cada 15 minutos
      objTimer.Interval = 900 * 1000

      'L� parametros de configuracao fazendo o tratamento
      If Not (leConfiguracao()) Then Exit Sub

      'Verifica se o diret�rio ra�z com os sub-diretorios de
      'transacoes Externas, Temporarias e Internas existe
      If Not Directory.Exists(strDirRaizIIS) Then
        Throw New System.Exception("O diret�rio " & strDirRaizIIS & " n�o existe")
      End If

      'Exclui o log a cada vez que o servico e executado
      If File.Exists(strArquivoLog) Then File.Delete(strArquivoLog)

      'Configura��es da conex�o FTP
      'Modo passivo com a propriedade ConnectMode()
      'Nome ou endere�o do servidor Remoto com a propriedade RemoteHost()
      objFTP.ConnectMode = FTPConnectMode.PASV
      objFTP.RemoteHost = strFTPServidor

      'Tenta abrir conex�o com o servidor FTP.
      'Em caso de sucesso, grava no EventViewer uma informa��o
      'alertando que a conex�o (chegada ao FTP) configurado foi
      'realizada com sucesso.
      objFTP.Connect()
      logEventViewer("Conectado ao servidor " & strFTPServidor, EventLogEntryType.Information)

      'Aberta a conex�o FTP, � realizada a valida��o do usu�rio a senha.
      objFTP.User(strFTPUsuario) : objFTP.Password(strFTPSenha)

      'Caso a valida��o tenha ocorrido com sucesso, grava novo EventViewer
      'notificando o sucesso da opera��o.
      logEventViewer("Usu�rio e senha validados ok!", EventLogEntryType.Information)

      'Seta o FTP para usar o tipo ASCII nas trasfer�ncias de arquivos
      objFTP.TransferType = FTPTransferType.BINARY

      'Abre conexao com o SQL Server
      If Not objDB.AbreConexao() Then
        Throw New System.Exception(objDB.MensagemErroDB)
      End If

    Catch ex As Exception
      logEventViewer(ex.Message.Trim, EventLogEntryType.Error)
      Exit Sub

    End Try

    'Executa Transferencias entre ambieten Web (IIS) e Baan (Unix)
    Call executaCopiaWebBaan()

    'Executa Transferencias entre ambieten Baan (Unix) e Web (IIS)
    Call executaCopiaBaanWeb()

    Try
      'Fecha conexao FTP
      objFTP.Quit()

      'Fecha conex�o com SQL Server
      If Not objDB.FechaConexao() Then
        Throw New System.Exception(objDB.MensagemErroDB)
      End If

    Catch ex As Exception
      logEventViewer(ex.Message.Trim, EventLogEntryType.Error)

    End Try

    logEventViewer("Transfer�ncias conclu�das", EventLogEntryType.Information)
  End Sub

  Private Sub executaCopiaWebBaan()
    Try
      logEventViewer("Iniciadas transfer�ncias entre o Web (IIS) e Baan (Unix)", EventLogEntryType.Information)

      'Busca todos os c�digos de Fornecedores no diret�rio configurado
      'no servidor Web. A partir desse diret�rio ra�z, o aplicativo busca
      'subdiret�rios seguindo a padroniza��o:
      '\<cod transportador>\baan\<codigo da transferencia>
      Dim arrCodFornecedor() As String = Directory.GetDirectories(strDirRaizIIS)
      Dim strCodFornecedor As String
      For Each strCodFornecedor In arrCodFornecedor

        'Diret�rios com c�digos de Transferencias dentro
        ' do diretorio "\baan" de um determinado Fornecedor
        Dim arrCodTransferencia() As String = Directory.GetDirectories(strCodFornecedor & "\baan")
        Dim strCodTransferencia As String
        For Each strCodTransferencia In arrCodTransferencia

          'Arquivos contidos em um diret�rio representando
          'um codigo de transferencia.
          Dim arrArquivos() As String = Directory.GetFiles(strCodTransferencia)
          For Each strArquivo In arrArquivos

            'Limpa string para saber o nome do arquivo a ser transferido,
            'a partir do nome sabe-se a qual companhia e a qual aplicativo
            'o arquivo em quest�o pertence. A padronizacao do nome �:
            '<cod companhia><nome aplicativo>. Ex.: 012cfrete.txt
            strArquivo = strArquivo.Substring(InStrRev(strArquivo, "\"))

            'Buscando Companhia e Aplicativo no nome do arquivo
            strCompanhia = "C" & strArquivo.Substring(0, 3)
            strAplicativo = Mid(strArquivo, 4)
            strAplicativo = strAplicativo.Substring(0, InStrRev(strAplicativo, ".") - 1)

            'strDepto     = Retorna o diret�rio raiz (depto) no Unix do aplicativo
            'strCodTransp = Codigo do Fornecedor atual
            'strCodTransf = Codigo da Transferencia no dir \baan
            strDepto = retornaRaiz(strAplicativo)
            strCodTransp = strCodFornecedor.Substring(InStrRev(strCodFornecedor, "\"))
            strCodTransf = strCodTransferencia.Substring(InStrRev(strCodTransferencia, "\"))

            'Faz a transferencia FTP
            executaCopiaFTP(strDepto, _
                            strCompanhia, _
                            strAplicativo, _
                            strArquivo, _
                            strCodTransp, _
                            strCodTransf)
          Next
          'Remove o Diret�rio com C�digo de Transmissao
          Directory.Delete(strCodTransferencia, True)
        Next
      Next

    Catch ex As Exception
      logEventViewer(ex.Message.Trim, EventLogEntryType.Error)

    End Try
  End Sub

  Private Sub executaCopiaBaanWeb()
    Dim objReader As SqlDataReader
    Dim arrDepto As String()
    Dim intControle As Integer

    'Busca todos diret�rios raiz cadastrados fazendo um array
    'posteriomente, faz-se um loop em todos eles
    Try
      logEventViewer("Iniciadas transfer�ncias entre o Baan (Unix) e Web (IIS)", EventLogEntryType.Information)

      strSQL = "SELECT DISTINCT ANOM_DIR_RAIZ FROM TAPLICATIVO"
      If Not objDB.ExecuteReader(strSQL, objReader) Then
        Throw New System.Exception(objDB.MensagemErroDB)
      End If

      While objReader.Read()
        ReDim Preserve arrDepto(intControle)
        arrDepto(intControle) = objReader("ANOM_DIR_RAIZ").ToString.Trim
        intControle += 1
      End While

    Catch ex As Exception
      logEventViewer(ex.Message.Trim, EventLogEntryType.Error)
      Exit Sub

    Finally
      If Not objReader.IsClosed Then objReader.Close()
      objReader = Nothing
      intControle = 0

    End Try

    Try
      'Loop em todos Departamentos
      For intControle = 0 To UBound(arrDepto)

        objFTP.ChDir("/")

        'Procurando companhias existentes para um Departamento
        Dim intDirCompanhia As Integer
        Dim arrDirCompanhia As Array = objFTP.Dir(arrDepto(intControle), False)
        While intDirCompanhia < arrDirCompanhia.Length

          'Procurando todos aplicativos existentes no diret�rio \sta
          'de determinada companhia
          Dim intDirAplic As Integer
          Dim arrDirAplic As Array = objFTP.Dir(arrDirCompanhia(intDirCompanhia) & "/sta", False)
          While intDirAplic < arrDirAplic.Length

            'Busca os arquivos enviados para o fornecedor, buscando
            'os arquivos existentes no diret�rio no formato:
            '<departamento>\<companhia>\sta\<aplicativo>\<enviadas>\<cod fornecedor>
            Dim intDirTransp As Integer

            'O diret�rio /enviadas pode n�o existir, por isso aqui tenta-se cri�-lo sem tratar erro
            'caso ele j� exista, passa direto.
            Try
              objFTP.MkDir(arrDirAplic(intDirAplic) & "/enviadas")
            Catch : End Try

            Dim arrDirTransp As Array = objFTP.Dir(arrDirAplic(intDirAplic) & "/enviadas", False)
            While intDirTransp < arrDirTransp.Length

              Dim strDirTransp As String = arrDirTransp(intDirTransp).ToString.Substring(InStrRev(arrDirTransp(intDirTransp), "/"))

              Dim intDirArq As Integer
              Dim arrDirArq As Array = objFTP.Dir(arrDirTransp(intDirTransp), False)
              While intDirArq < arrDirArq.Length

                'Busca o c�digo a ser usar para a nova transacao
                If Not (objDB.ExecuteReader("SP_NUMERACAO", objReader)) Then
                  Throw New System.Exception(objDB.MensagemErroDB)
                End If

                objReader.Read()
                Dim intContAux As Integer = objReader("CODIGO")

                objReader.Close()

                'Caso seja o primeiro la�o do loop.
                'Inclui o registro de transmissao e envia correio avisando do recebimento
                If intDirArq = 0 Then

                  'Busca o email do Fornecedor
                  strSQL = "SELECT ADES_EMAIL_FORNECEDOR,ANUM_SEQU_FORNECEDOR FROM TFORNECEDOR" & _
                          " WHERE ACOD_FORNECEDOR_BAAN = '" & strDirTransp & "'"
                  If Not (objDB.ExecuteReader(strSQL, objReader)) Then
                    Throw New System.Exception(objDB.MensagemErroDB)
                  End If

                  objReader.Read()
                  Dim strNumeroFornecedor As String
                  Dim strEmailFornecedor As String
                  strNumeroFornecedor = objReader("ANUM_SEQU_FORNECEDOR").ToString()
                  strEmailFornecedor = objReader("ADES_EMAIL_FORNECEDOR").ToString.Trim
                  objReader.Close()

                  'By Rodrigo:07/07 - O Sequencial nao estava sendo recuperado o que ocasinava erro de pai sem filho
                  'incluiRegTransmissao(strDirTransp, intContAux)
                  incluiRegTransmissao(strNumeroFornecedor, intContAux)

                  enviaCorreio(strEmailFornecedor, _
                              intContAux)


                End If

                'Nome do arquivo, sem o caminho de diret�rios
                strArquivo = arrDirArq(intDirArq)
                strArquivo = strArquivo.Substring(InStrRev(strArquivo, "/"))

                'Nome do aplicativo, sem caminho de diret�rios
                strAplicativo = arrDirAplic(intDirAplic).ToString.Trim
                strAplicativo = strAplicativo.Substring(InStrRev(strAplicativo, "/"))

                'Cria o diret�rio que vai receber o arquivo no ambiente web
                Directory.CreateDirectory(strDirRaizIIS & "\" & strDirTransp & "\recebidas\" & intContAux)

                'TODO: Rodrigo 13/03/2006 ... Debug....
                Debug.WriteLine("Transferindo:" & arrDirCompanhia(intDirCompanhia).ToString.Replace(arrDepto(intControle).ToString.Trim & "/", "").Trim)
                Debug.Write(" Aplicativo:" & strAplicativo.ToString.Trim)
                Debug.Write(" Arquivo:" & strArquivo.ToString.Trim)
                Debug.Write(" DirTransp:" & strDirTransp.ToString.Trim)
                Debug.Write(" intContAux:" & CStr(intContAux))

                executaCopiaFTP(arrDepto(intControle).ToString.Trim, _
                                arrDirCompanhia(intDirCompanhia).ToString.Replace(arrDepto(intControle).ToString.Trim & "/", "").Trim, _
                                strAplicativo.ToString.Trim, _
                                strArquivo.ToString.Trim, _
                                strDirTransp.ToString.Trim, _
                                intContAux)

                intDirArq += 1
              End While 'While intDirArq < arrDirArq.Length

              intDirArq = 0

              intDirTransp += 1
            End While 'While intDirTransp < arrDirTransp.Length

            intDirTransp = 0

            intDirAplic += 1
          End While 'While intDirAplic < arrDirAplic.Length

          intDirAplic = 0

          intDirCompanhia += 1
        End While 'While intDirCompanhia < arrDirCompanhia.Length

        intDirCompanhia = 0

      Next 'For intControle = 0 To UBound(arrDepto)

    Catch ex As Exception
      logEventViewer(ex.ToString.Trim, EventLogEntryType.Error)

    End Try
  End Sub

  'Realiza c�pias entre o ambiente Web (IIS) e Baan (Unix)
  'A partir do diret�rio raiz, tenta criar se n�o houver a estrutura:
  '<departamento>/<companhia>/sta/<aplicativo>/recebidas/
  '<departamento>/<companhia>/sta/<aplicativo>/bkp/recebidas
  'Ent�o copia o arquivo no c�digo do Fornecedor no diret�rio recebidas
  'concatenando com o c�digo de transferencia. Em seguida, copia o mesmo
  'arquivo para o diret�rio de backup, concatenando data e hora do envio FTP.
  Private Overloads Sub executaCopiaFTP(ByVal strDepartamento As String, _
                                      ByVal strCompanhia As String, _
                                      ByVal strAplicativo As String, _
                                      ByVal strArquivo As String, _
                                      ByVal strCodTransp As String, _
                                      ByVal strCodTransf As String)
    On Error GoTo trata

    Dim strPathArquivoLocal As String = strDirRaizIIS & "/" & strCodTransp & "/baan/" & strCodTransf & "/" & strArquivo
    objFTP.ChDir("/")
    objFTP.ChDir(strDepartamento)
    objFTP.MkDir(strCompanhia)
    objFTP.ChDir(strCompanhia)
    objFTP.MkDir("sta")
    objFTP.ChDir("sta")
    objFTP.MkDir(strAplicativo)
    objFTP.ChDir(strAplicativo)

    objFTP.MkDir("recebidas")
    objFTP.MkDir("bkp")

    objFTP.ChDir("recebidas")
    objFTP.MkDir(strCodTransp)
    objFTP.ChDir(strCodTransp)
    objFTP.Put(strPathArquivoLocal, strCodTransf & "-" & strArquivo)

    objFTP.ChDir("..")
    objFTP.ChDir("..")

    objFTP.ChDir("bkp")
    objFTP.MkDir("recebidas")
    objFTP.ChDir("recebidas")
    objFTP.MkDir(strCodTransp)
    objFTP.ChDir(strCodTransp)
    objFTP.Put(strPathArquivoLocal, strArquivo & "-" & Format(DateTime.Now, "dd-mm-yyyy_hh-mm"))

    objFTP.ChDir("/")

    On Error GoTo 0

    Exit Sub
trata:
    logArquivo(Err.Description)
    Resume Next
  End Sub

  'Realiza c�pias entre o ambiente Baan (Unix) e Web (IIS)
  'A partir do diret�rio raiz, tenta acessar a estrutura, criando se necess�rio:
  '<departamento>/<companhia>/sta/<aplicativo>/enviadas/
  '<departamento>/<companhia>/sta/<aplicativo>/bkp/enviadas
  'Ent�o copia os arquivos contidos no diret�rio /enviadas
  'para o diret�rio /recebidas/<cod transferencia> no ambiemte web
  'logo em seguida copia o mesmo arquivo para o diret�rio de backup,
  'concatenando data e hora do envio FTP.
  Private Overloads Sub executaCopiaFTP(ByVal strDepartamento As String, _
                                        ByVal strCompanhia As String, _
                                        ByVal strAplicativo As String, _
                                        ByVal strArquivo As String, _
                                        ByVal strDirTrans As String, _
                                        ByVal intCodTransf As Integer)
    On Error GoTo trata

    objFTP.ChDir("/")
    objFTP.ChDir(strDepartamento)
    objFTP.ChDir(strCompanhia)
    objFTP.ChDir("sta")
    objFTP.ChDir(strAplicativo)

    objFTP.MkDir("bkp")
    objFTP.MkDir("enviadas")

    objFTP.ChDir("bkp")
    objFTP.MkDir("enviadas")
    objFTP.ChDir("enviadas")
    objFTP.MkDir(strDirTrans)
    objFTP.ChDir("..")
    objFTP.ChDir("..")

    objFTP.ChDir("enviadas")
    objFTP.MkDir(strDirTrans)
    objFTP.ChDir(strDirTrans)

    objFTP.Get(strDirRaizIIS & "\" & strDirTrans & "\recebidas\" & intCodTransf & "\" & strArquivo, strArquivo)
    objFTP.Rename(strArquivo, "../../bkp/enviadas/" & strDirTrans & "/" & strArquivo & "-" & Format(DateTime.Now, "dd-mm-yyyy_hh-mm"))

    objFTP.ChDir("/")

    On Error GoTo 0


    incluiArquivoTrasmissao(strDirTrans, intCodTransf, strArquivo)

    Exit Sub
trata:
    logArquivo(Err.Description)
    Resume Next
  End Sub

  'Inclui um registro de transacao
  Private Sub incluiRegTransmissao(ByVal strCodTransp As String, ByVal intContador As Integer)
    Try
      strSQL = "INSERT INTO TREGISTRO_TRANSMISSAO (ANUM_SEQU_FORNECEDOR, " & _
              "ANUM_SEQU_REGISTRO, ATIP_PROTOCOLO, ABOL_VALIDACAO_PROTOCOLO, " & _
              "ADAT_PROTOCOLO, ABOL_EXCLUSAO) VALUES('" & _
              strCodTransp.Trim & "', " & _
              intContador & _
              ", 1, 0, getdate(), 0)"

      If Not (objDB.ExecutaSQL(strSQL)) Then
        Throw New System.Exception(objDB.MensagemErroDB)
      End If

    Catch ex As Exception
      logEventViewer(ex.Message.Trim & strSQL, EventLogEntryType.Error)

    End Try
  End Sub

  'Associa arquivos enviados a uma transacao
  Private Sub incluiArquivoTrasmissao(ByVal strCodTransp As String, _
                                      ByVal intContador As Integer, _
                                      ByVal strNomeArquivo As String)
    Dim objReader As SqlDataReader
    Dim strNumFornecedor As String

    Try
      strSQL = "SELECT ISNULL(MAX(ANUM_SEQU_ARQUIVO), 0) + 1 AS CONTADOR" & _
              " FROM TARQUIVO"
      If Not (objDB.ExecuteReader(strSQL, objReader)) Then
        Throw New System.Exception(objDB.MensagemErroDB)
      End If

      objReader.Read()
      Dim intContAux As Integer = objReader("CONTADOR")

      objReader.Close()

      strSQL = "SELECT ANUM_SEQU_FORNECEDOR FROM TFORNECEDOR" & _
                          " WHERE ACOD_FORNECEDOR_BAAN = '" & strCodTransp & "'"
      If Not (objDB.ExecuteReader(strSQL, objReader)) Then
        Throw New System.Exception(objDB.MensagemErroDB)
      End If

      objReader.Read()
      strNumFornecedor = objReader("ANUM_SEQU_FORNECEDOR").ToString()
      objReader.Close()
      ' By Rodrigo 07/07: strCodTransp.Trim por strNumFornecedor 
      strSQL = "INSERT INTO TARQUIVO (ANUM_SEQU_FORNECEDOR, " & _
              "ANUM_SEQU_REGISTRO, ANUM_SEQU_ARQUIVO, " & _
              "ANOM_ARQUIVO) VALUES('" & _
              strNumFornecedor.Trim & "', " & _
              intContador & ", " & _
              intContAux & _
              ", '" & strNomeArquivo & "')"

      If Not (objDB.ExecutaSQL(strSQL)) Then
        Throw New System.Exception(objDB.MensagemErroDB)
      End If

      objReader = Nothing

    Catch ex As Exception
      logEventViewer(ex.Message.Trim & strSQL, EventLogEntryType.Error)

    End Try
  End Sub

  Private Function retornaRaiz(ByVal strAplicativo As String) As String
    Dim objReader As SqlDataReader
    Dim strDirAux As String
    Try
      strSQL = "SELECT ANOM_DIR_RAIZ FROM TAPLICATIVO WHERE ANOM_APLIC = '" & strAplicativo.Trim & "'"
      If Not objDB.ExecuteReader(strSQL, objReader) Then
        Throw New System.Exception(objDB.MensagemErroDB)
      End If

      If objReader.HasRows Then
        objReader.Read()
        strDirAux = objReader("ANOM_DIR_RAIZ").ToString.Trim
      End If

    Catch ex As Exception
      logEventViewer(ex.Message.Trim, EventLogEntryType.Error)

    Finally
      objReader.Close()
      If Not IsNothing(objReader) Then objReader = Nothing

    End Try
    Return strDirAux
  End Function

  Private Sub enviaCorreio(ByVal strDestino As String, ByVal intCodigo As Integer)
    Try
      Dim objMail As System.Web.Mail.SmtpMail
      Dim objMailMsg As New System.Web.Mail.MailMessage

      Dim strTextoMsg As String
      strTextoMsg = "Voc� est� recebendo este e-mail, em notifica��o"
      strTextoMsg += " ao recebimento de arquivos para c�pia atrav�s do"
      strTextoMsg += " Sistema de Transfer�ncia de Arquivos da Itamb�." & vbCrLf & vbCrLf
      strTextoMsg += "O c�digo dessa transfer�ncia �: " & intCodigo & vbCrLf & vbCrLf
      strTextoMsg += "Clique no link abaixo para ter acesso:" & vbCrLf & vbCrLf
      strTextoMsg += strURLSite & vbCrLf & vbCrLf

      'Rodape
      strTextoMsg += "As informa��es contidas neste ""e-mail"", e nos "
      strTextoMsg += "arquivos anexos, s�o para o uso exclusivo do destinat�rio "
      strTextoMsg += "aqui indicado, n�o se autorizando o acesso por qualquer outra "
      strTextoMsg += "pessoa. Caso n�o seja o destinat�rio correto, esteja "
      strTextoMsg += "notificado pelo presente, que qualquer revis�o, leitura, c�pia e/ou "
      strTextoMsg += "divulga��o do conte�do deste ""e-mail"" s�o estritamente proibidas e n�o "
      strTextoMsg += "autorizadas. Por favor, apague o conte�do do ""e-mail"" e notifique o "
      strTextoMsg += "remetente e o setor respons�vel na Itamb� (suporte@itambe.com.br) "
      strTextoMsg += "imediatamente." & vbCrLf & vbCrLf
      strTextoMsg += "Grato pela coopera��o." & vbCrLf & vbCrLf
      strTextoMsg += "Cooperativa Central dos Produtores Rurais de Minas Gerais Ltda - Itamb�" & vbCrLf
      strTextoMsg += "Esta mensagem foi verificada pelo sistema de antiv�rus e acredita-se "
      strTextoMsg += "estar livre de perigo."

      With objMailMsg
        .Priority = MailPriority.Normal
        .From = "sta@itambe.com.br"
        .To = strDestino.Trim
        .Subject = "Itamb� - Transfer�ncia de Arquivos"
        .Body = strTextoMsg
      End With

      objMail.SmtpServer = strServidorCorreio
      'TODO:By Rodrigo. N�o enviar emails nos testes
      objMail.Send(objMailMsg)

    Catch ex As System.Exception
      logEventViewer(strDestino.Trim & " " & Trim(ex.ToString), EventLogEntryType.Error)

    End Try
  End Sub

  Private Function leConfiguracao() As Boolean
    Dim blnRetorno As Boolean = True
    Try
      strArquivoLog = ConfigurationSettings.AppSettings.Get("DirArquivoLog").Trim
      strDirRaizIIS = ConfigurationSettings.AppSettings.Get("DirRaizIIS").Trim
      strServidorCorreio = ConfigurationSettings.AppSettings.Get("ServidorCorreio").Trim
      strFTPUsuario = ConfigurationSettings.AppSettings.Get("FTPUsuario").Trim
      strFTPSenha = ConfigurationSettings.AppSettings.Get("FTPSenha").Trim
      strFTPServidor = ConfigurationSettings.AppSettings.Get("FTPServidor").Trim
      strSQLBanco = ConfigurationSettings.AppSettings.Get("SQLBanco").Trim
      strSQLServidor = ConfigurationSettings.AppSettings.Get("SQLServidor").Trim
      strSQLUsuario = ConfigurationSettings.AppSettings.Get("SQLUsuario").Trim
      strSQLSenha = ConfigurationSettings.AppSettings.Get("SQLSenha").Trim
      strURLSite = ConfigurationSettings.AppSettings.Get("URLSite").Trim

      If strArquivoLog = "" Then
        Throw New System.Exception("DirArquivoLog")
      ElseIf strDirRaizIIS = "" Then
        Throw New System.Exception("DirRaizIIS")
      ElseIf strServidorCorreio = "" Then
        Throw New System.Exception("ServidorCorreio")
      ElseIf strFTPUsuario = "" Then
        Throw New System.Exception("FTPUsuario")
      ElseIf strFTPSenha = "" Then
        Throw New System.Exception("FTPSenha")
      ElseIf strFTPServidor = "" Then
        Throw New System.Exception("FTPServidor")
      ElseIf strSQLBanco = "" Then
        Throw New System.Exception("SQLBanco")
      ElseIf strSQLServidor = "" Then
        Throw New System.Exception("SQLServidor")
      ElseIf strSQLUsuario = "" Then
        Throw New System.Exception("SQLUsuario")
      ElseIf strSQLSenha = "" Then
        Throw New System.Exception("SQLSenha")
      ElseIf strURLSite = "" Then
        Throw New System.Exception("URLSite")
      End If

      'verifica se existe o caminho do arquivo de log
      If Not (Directory.Exists(strArquivoLog)) Then
        Throw New System.Exception("O diret�rio do arquivo de log configurado n�o existe")
      End If

      'Concatena o caminho da configura��o com o nome do arquivo que � fixo
      strArquivoLog += "\copia.log"

    Catch ex As Exception
      logEventViewer("N�o foi encontrado o par�metro de configura��o " & _
                      ex.Message.Trim, EventLogEntryType.Error)
      blnRetorno = False

    End Try
    Return blnRetorno
  End Function

  Private Sub logEventViewer(ByVal strMsg As String, ByVal tipoMsg As EventLogEntryType)
    Dim objLog As EventLog = New EventLog

    If Not objLog.SourceExists(strNomeApp) Then
      objLog.CreateEventSource(strNomeApp, "Application")
    End If

    objLog.Source = strNomeApp
    objLog.WriteEntry(strMsg, tipoMsg)
  End Sub

  Private Sub logArquivo(ByVal strLinha As String)
    Try
      Dim objFS As FileStream = New FileStream(strArquivoLog, FileMode.OpenOrCreate, FileAccess.Write)
      Dim objEscritor As StreamWriter = New StreamWriter(objFS)

      objEscritor.BaseStream.Seek(0, SeekOrigin.End)
      objEscritor.WriteLine(strLinha)
      objEscritor.Close()

      objEscritor = Nothing
      objFS = Nothing

    Catch ex As Exception
      logEventViewer(ex.Message.Trim, EventLogEntryType.Error)

    End Try
  End Sub

End Class