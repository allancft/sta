Public Class home
    Inherits System.Web.UI.Page
    Private strErro As String
#Region "Variaveis e Objetos"

    Protected imgConfig As System.Web.UI.HtmlControls.HtmlImage
    Protected imgFornec As System.Web.UI.HtmlControls.HtmlImage
    Protected imgConsul As System.Web.UI.HtmlControls.HtmlImage

    Private objConfiguracao As clsConfiguracao
    Private objScript As clsScript
    Private objDB As clsDB
    Private blnRet, blnJaConfigurado As Boolean
    Protected WithEvents imgTransf As System.Web.UI.HtmlControls.HtmlImage
    Private strScript As String = "javascript:"

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
        Try
            objConfiguracao = New clsConfiguracao
            objDB = New clsDB

            If Not (objDB.AbreConexao()) Then
                Throw New System.Exception(objDB.MensagemErroDB.Trim())
            End If

            objConfiguracao.objConexao = objDB

            blnRet = objConfiguracao.verificaConfiguracao(blnJaConfigurado, strErro)

            If Not blnRet Then
                strErro = objScript.limpaStringMensagem(strErro)
                strScript += "alert('" & strErro & "');"
                objScript.eventoObjetoGenerico(Me, "body", "onload", strScript)
                Exit Sub
            End If

            If Not blnJaConfigurado Then
                strScript += "alert('Não foi encontrada a configuração inicial do sistema. Você será encaminhado para ajustar estes parâmetros.');"
                strScript += "navegacao('configuracao.aspx', 'Configuração Geral');"
            End If

            strScript += "tamanhoIframe();"

            objScript.eventoObjetoGenerico(Me, "body", "onload", strScript)
            objScript.eventoObjetoGenerico(Me, "body", "onresize", "javascript:tamanhoIframe();")

            objScript = Nothing

            If Not (Session("blnUsuarioAdmin")) Then
                imgConfig.Visible = False
                imgConsul.Visible = False
                imgFornec.Visible = False
            End If

        Catch objErro As System.Exception

        End Try
    End Sub

End Class
