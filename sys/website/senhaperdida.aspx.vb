Imports System.Data.SqlClient
Partial Class senhaperdida
    Inherits System.Web.UI.Page
#Region "Objetos e Variaveis"
    'objetos
    'variveis
    Dim objDB As clsDB
    Dim objMail As clsEmail
    Dim objScript As clsScript
    Dim objCripto As aim.objCriptografia
    Dim objUtil As clsUtil
    Private strSQL, strScript, strErro As String
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
    Private Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Try
            Dim strEmail As String
            stremail = txtEmail.Text.Trim()

            objUtil = New clsUtil

            If Not (objUtil.ValidaEmail(strEmail)) Then
                Throw New System.Exception("O Email """ & strEmail & """ não é válido")
            End If

            objDB = New clsDB

            If Not (objDB.AbreConexao) Then
                Throw New System.Exception(objDB.MensagemErroDB)
            End If

            'verifica se existe o email informado
            'se existir, envia a senha para este mesmo email
            Dim objDataReader As SqlDataReader

            strSQL = "SELECT ADES_EMAIL_FORNECEDOR, ADES_SENHA_FORNECEDOR"
            strSQL += " FROM TFORNECEDOR"
            strSQL += " WHERE ADES_EMAIL_FORNECEDOR = '" & strEmail & "'"
            If Not (objDB.ExecuteReader(strSQL, objDataReader)) Then
                Throw New System.Exception(objDB.MensagemErroDB.Trim())
            End If

            If objDataReader.HasRows Then
                objDataReader.Read()
                objMail = New clsEmail
                If Not (objMail.EnviaSenhaAcesso(strEmail, objDataReader("ADES_SENHA_FORNECEDOR").ToString.Trim(), strErro)) Then
                    Throw New System.Exception(strErro)
                End If
                Me.RegisterStartupScript("aviso", "<script language=""javascript"">alert('A senha de acesso foi enviada para o email \""" & strEmail & "\""');top.location.href='default.aspx';</script>")
            Else
                Me.RegisterStartupScript("aviso", "<script language=""javascript"">alert('Não foi encontrada conta de acesso para o email \""" & strEmail & "\""');</script>")
            End If

        Catch ex As Exception
            strErro = ex.Message.Replace("'", "")
            Me.RegisterStartupScript("erro", "<script language=""javascript"">javascript:alert('Erro:\n\n" & strErro & "');</script>")

        End Try
    End Sub
End Class
