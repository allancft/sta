Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Public Class configuracao
    Inherits System.Web.UI.Page
#Region "Variaveis e Objetos"

    Private strErro, strScript As String
    Private blnRet As Boolean
    Private objScript As clsScript
    Private objCriptografia As aimCriptografia.clsCriptografia

    Protected WithEvents obrTxtDiasExclusao As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtExtensoesInt As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtExtensoesExt As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtUsuariosAdmin As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSenhaSQL As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtSenhaAD As System.Web.UI.WebControls.TextBox
    Protected WithEvents lblSenhaSQL As System.Web.UI.WebControls.Label
    Protected WithEvents lblSenhaAD As System.Web.UI.WebControls.Label
    Protected WithEvents btnSubmit As System.Web.UI.WebControls.Button
    Protected WithEvents btnCripto As System.Web.UI.WebControls.Button
    Protected WithEvents body As System.Web.UI.HtmlControls.HtmlGenericControl
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
        Session("objDataSetDestinos") = Nothing
        Session("objDataSetArquivos") = Nothing

        If IsPostBack Then Exit Sub
        Call PopulaCampos()
    End Sub
    Private Sub PopulaCampos()
        Dim objConexao As clsDB
        objConexao = New clsDB
        Try
            Dim objPagina As New sta.clsConfiguracao
            Dim objReader As SqlClient.SqlDataReader

            'Tenta abrir conexão e trata erro se houver.
            If Not objConexao.AbreConexao() Then
                strScript += "alert('" & objScript.limpaStringMensagem(objConexao.MensagemErroDB) & "');"
                objScript.eventoObjetoGenerico(Me, "body", "onload", strScript)
                Exit Sub
            End If

            'Atribui a conexão ativa para a classe do formulário
            objPagina.objConexao = objConexao

            blnRet = objPagina.ConfiguracaoConsultar(objReader, strErro)

            If Not blnRet Then
                strScript += "alert('" & objScript.limpaStringMensagem(strErro) & "');"
                objScript.eventoObjetoGenerico(Me, "body", "onload", strScript)
                Exit Sub
            End If

            objReader.Read()

            If objReader.HasRows Then
                obrTxtDiasExclusao.Text = objReader("ADAT_EXCLUSAO").ToString().Trim()
                txtExtensoesInt.Text = objReader("AFTP_EXT_ARQU_USER_INTERNO").ToString().Trim()
                txtExtensoesExt.Text = objReader("AFTP_EXT_ARQU_USER_FORNECEDOR").ToString().Trim()
                txtUsuariosAdmin.Text = objReader("ANOM_USER_ADMIN").ToString().Trim()
            End If

        Catch ex As System.Exception
            strScript += "alert('" & objScript.limpaStringMensagem(ex.Message.Trim()) & "');"
            objScript.eventoObjetoGenerico(Me, "body", "onload", strScript)

        Finally
            objConexao.FechaConexao()

        End Try

    End Sub
    Private Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Dim objConexao As clsDB
        objConexao = New clsDB

        Try
            'Salva a configuração no Banco de Dados.
            Dim objPagina As New sta.clsConfiguracao
            Dim objReader As SqlClient.SqlDataReader

            'Tenta abrir conexão e trata erro se houver.
            If Not objConexao.AbreConexao() Then
                strScript += "alert('" & objScript.limpaStringMensagem(objConexao.MensagemErroDB) & "');"
                objScript.eventoObjetoGenerico(Me, "body", "onload", strScript)
                Exit Sub
            End If

            objPagina.objConexao = objConexao
            blnRet = objPagina.ConfiguracaoSalvar(obrTxtDiasExclusao.Text, _
                                        txtExtensoesInt.Text, _
                                        txtExtensoesExt.Text, _
                                        txtUsuariosAdmin.Text, _
                                        strErro)
            If Not blnRet Then
                Throw New System.Exception(strErro)
            End If

        Catch ex As Exception
            strScript += "alert('" & objScript.limpaStringMensagem(ex.Message.Trim()) & "');"
            objScript.eventoObjetoGenerico(Me, "body", "onload", strScript)

        Finally
            objConexao.FechaConexao()
            PopulaCampos()

            If strErro = "" Then
                strScript += "alert('Parâmetros de configuração armazenados com sucesso.');"
                objScript.eventoObjetoGenerico(Me, "body", "onload", strScript)
            End If
        End Try
    End Sub
    Private Sub btnCripto_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCripto.Click
        Try
            objCriptografia = New aimCriptografia.clsCriptografia
            lblSenhaAD.Text = objCriptografia.Criptografar(txtSenhaAD.Text)
            lblSenhaSQL.Text = objCriptografia.Criptografar(txtSenhaSQL.Text)
        Catch ex As Exception
            strScript += "alert('" & objScript.limpaStringMensagem(ex.Message.Trim()) & "');"
            objScript.eventoObjetoGenerico(Me, "body", "onload", strScript)
        Finally
            PopulaCampos()
        End Try
    End Sub
End Class
