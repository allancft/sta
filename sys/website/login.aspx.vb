Imports System.Data
Imports System.Web.Security
Imports fSystem.Configuration
Imports System.Data.SqlClient
Partial Class login
    Inherits System.Web.UI.Page
#Region "Variáveis, Enumeradores e Objetos"

    'variaveis
    Private strErro, strSQL, strChave As String
    'Private strCaminho = ConfigurationSettings.AppSettings("diretorioArquivos")
    Protected strNome = ConfigurationSettings.AppSettings("nomeSistema")
    Private intTipoAcesso As Integer

    'objetos
    Private Property TipoAcesso() As enumTipoAcesso
        Get
            Return intTipoAcesso
        End Get
        Set(ByVal Value As enumTipoAcesso)
            intTipoAcesso = Value
        End Set
    End Property
    Private Enum enumTipoAcesso
        Interno = 1
        Externo = 2
    End Enum
#End Region
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ControleAcesso() 'controla tipo de acesso
        If Not IsPostBack Then Exit Sub
        ValidaAcesso()
    End Sub
    Private Sub ControleAcesso()
        Dim strIpInicialServidorDmz, strIpInicialServidorLan, strIpServidor As String
        strIpInicialServidorDmz = ConfigurationSettings.AppSettings("IpInicialServidorDmz")
        strIpInicialServidorLan = ConfigurationSettings.AppSettings("IpInicialServidorLan")
        strIpServidor = Request.ServerVariables("LOCAL_ADDR")
        'strIpServidor = "10.9.2.10"
        'se o ip que esta acessando o aplicativo consta nos ips internos
        'das localidades ou escritorio central.
        '** Verifica se o início do IP é de um servidor LAN ou da DMZ.
        If strIpServidor.IndexOf(strIpInicialServidorLan) = 0 Then ' Or strIpServidor.IndexOf(strIpInicialServidorDmz) = 0 Then
            Me.TipoAcesso = enumTipoAcesso.Interno
			tabelaLogin.Attributes.Add("background", "images/login_interno.gif")
            trSenha.Visible = False
        Else
            Me.TipoAcesso = enumTipoAcesso.Externo
			tabelaLogin.Attributes.Add("background", "images/Backlogin.jpg")
            trSenha.Visible = True
        End If
        strChave = txtEmail.Text
    End Sub
    Private Sub ValidaAcesso()

        Dim objReader As SqlDataReader
        Dim objCriptografia As aim.objCriptografia
        Dim objBanco As clsDB

        Try
            If (txtEmail.Text.Trim = "") Or (txtSenha.Text.Trim = "") Then
                Throw New System.Exception("Digite um usuário e senha válidos")
            End If

            objCriptografia = New aim.objCriptografia
            objBanco = New clsDB

            Select Case TipoAcesso
                Case enumTipoAcesso.Interno
                    Try
                        Dim objADSI As clsADSI
                        objADSI = New clsADSI

                        If Not (objADSI.ValidaUsuario(txtEmail.Text.Trim(), txtSenha.Text.Trim(), strErro)) Then
                            Throw New System.Exception("Usuário ou senha incorretos.")
                        End If

                        If Not objBanco.AbreConexao() Then
                            Throw New System.Exception(objBanco.MensagemErroDB)
                        End If

                        strSQL = "SELECT ANOM_USER_ADMIN FROM TCONFIGURACAO"
                        If Not (objBanco.ExecuteReader(strSQL, objReader)) Then
                            Throw New System.Exception(objBanco.MensagemErroDB)
                        End If

                        objReader.Read()

                        Dim arrUsuarios As Array
                        Dim intIndice As Integer

                        arrUsuarios = objReader("ANOM_USER_ADMIN").ToString().Split(";")

                        Session("blnUsuarioAdmin") = False

                        For intIndice = 0 To arrUsuarios.GetUpperBound(0)
                            If strChave = arrUsuarios(intIndice).ToString().Trim() Then
                                Session("blnUsuarioAdmin") = True
                            End If
                        Next

                        Session("strDir") = strChave.Trim()
                        Session("usuario") = strChave.Trim & "@" & _
                                            ConfigurationSettings.AppSettings("dominioCorreio")
                        Session("tipoAcesso") = 1 'interno

                        Call FormsAuthentication.RedirectFromLoginPage(strChave, False)

                    Catch objErro As System.Exception
                        strErro = objErro.Message.Trim()

                    Finally
                        If Not IsNothing(objReader) Then objReader.Close()
                        If Not IsNothing(objBanco) Then objBanco.FechaConexao()

                    End Try

                Case enumTipoAcesso.Externo
                    Try
                        Dim strSenha As String = Request.Form("txtSenha").Trim()
                        strSenha = objCriptografia.Criptografar(strSenha)

                        If Not objBanco.AbreConexao() Then
                            Throw New System.Exception(objBanco.MensagemErroDB)
                        End If

                        strSQL = "SELECT A.ACOD_FORNECEDOR_BAAN, A.ADES_SENHA_FORNECEDOR, " & _
                                " A.ANOM_FORNECEDOR, A.ANUM_SEQU_FORNECEDOR, A.ADES_EMAIL_FORNECEDOR, " & _
                                " A.AFTP_EXT_ARQU, ADES_EMAIL_FORNECEDOR," & _
                                " ADES_DIRETORIO_FORNECEDOR = CASE" & _
                                " RTRIM(LTRIM(ISNULL(ACOD_FORNECEDOR_BAAN, '')))" & _
                                " WHEN '' THEN A.ADES_DIRETORIO_FORNECEDOR" & _
                                " ELSE A.ACOD_FORNECEDOR_BAAN" & _
                                " END, " & _
                                " B.ANUM_SEQU_LOG_ACESSO" & _
                                " FROM TFORNECEDOR A, TLOG_ACESSO_FORNECEDOR B" & _
                                " WHERE ADES_EMAIL_FORNECEDOR = '" & strChave & "'" & _
                                " AND A.ANUM_SEQU_FORNECEDOR *= B.ANUM_SEQU_FORNECEDOR" & _
                                " ORDER BY B.ANUM_SEQU_LOG_ACESSO DESC"

                        If Not (objBanco.ExecuteReader(strSQL, objReader)) Then
                            Throw New System.Exception(objBanco.MensagemErroDB())
                        End If

                        If Not objReader.HasRows Then 'Nao Retornou registro
                            Throw New System.Exception("Usuário não encontrado. Tente novamente")
                        End If

                        objReader.Read()

                        'Verifica se a senha digitada confere com a senha do banco de dados.
                        If strSenha <> objReader("ADES_SENHA_FORNECEDOR").ToString().Trim() Then
                            Throw New System.Exception("Senha incorreta. Tente novamente.")
                        End If

                        'Se tudo está OK. Valida acesso e seta sessoes necessárias.
                        Session("blnUsuarioAdmin") = False
                        Session("strExtensoes") = objReader("AFTP_EXT_ARQU").ToString().Trim()
                        Session("usuario") = objReader("ACOD_FORNECEDOR_BAAN").ToString().Trim()
                        Session("strDir") = IIf(objReader("ADES_DIRETORIO_FORNECEDOR") <> "", objReader("ADES_DIRETORIO_FORNECEDOR").ToString.Trim, objReader("ACOD_FORNECEDOR_BAAN").ToString().Trim())
                        Session("codSeq") = objReader("ANUM_SEQU_FORNECEDOR")
                        Session("tipoAcesso") = 2 'externo

                        Dim intCodFornecedor, intSeqAcesso As Integer
                        intCodFornecedor = CType(objReader("ANUM_SEQU_FORNECEDOR"), Integer)
                        If Not IsDBNull(objReader("ANUM_SEQU_LOG_ACESSO")) Then
                            intSeqAcesso = CType(objReader("ANUM_SEQU_LOG_ACESSO"), Integer)
                            intSeqAcesso += 1
                        Else
                            intSeqAcesso = 1
                        End If

                        objReader.Close()
                        objReader = Nothing

                        strSQL = "INSERT INTO TLOG_ACESSO_FORNECEDOR VALUES("
                        strSQL += intSeqAcesso & ", "
                        strSQL += intCodFornecedor & ", "
                        strSQL += "GetDate())"
                        If Not (objBanco.ExecutaSQL(strSQL)) Then
                            Throw New System.Exception(objBanco.MensagemErroDB)
                        End If

                        Call FormsAuthentication.RedirectFromLoginPage( _
                            strChave, False)

                    Catch objErro As Exception
                        strErro = objErro.Message.Trim()

                    Finally
                        If Not IsNothing(objReader) Then objReader.Close()
                        If Not IsNothing(objBanco) Then objBanco.FechaConexao()

                    End Try
            End Select

        Catch ex As System.Exception
            strErro = ex.Message.Trim

        End Try

        If strErro <> "" Then
            lblErro.Text = strErro
        End If

    End Sub

End Class
