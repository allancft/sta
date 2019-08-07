Imports System.Data.SqlClient
Partial Class trocasenha : Inherits System.Web.UI.Page
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
#Region "Objetos Asp.Net"
#End Region
#Region "Objetos"
    Dim objCripto As aim.objCriptografia
    Dim objBanco As clsDB
#End Region
#Region "Variáveis"
    Private strSQL As String
#End Region
    Private Sub validaTroca()
        Dim objReader As SqlDataReader
        Try
            objCripto = New aim.objCriptografia
            objBanco = New clsDB

            Dim strSenhaBanco As String
            Dim strSenhaAtual As String = txtSenha.Text.Trim
            Dim strSenhaNova As String = txtNovaSenha.Text.Trim
            Dim strSenhaNovaR As String = txtNovaSenhaR.Text.Trim

            'Verifica se as senhas digitadas (nova e nova conferencia) são identicas
            If strSenhaNova <> strSenhaNovaR Then
                Throw New System.Exception("A nova senha é diferente da senha digitada no campo de conferência. Tente novamente.")
            End If

            If Not objBanco.AbreConexao() Then
                Throw New System.Exception(objBanco.MensagemErroDB)
            End If

            'Valida Senha atual
            strSQL = "SELECT ADES_SENHA_FORNECEDOR" & _
                    " FROM TFORNECEDOR " & _
                    " WHERE ADES_EMAIL_FORNECEDOR = '" & txtEmail.Text.Trim & "'"
            If Not (objBanco.ExecuteReader(strSQL, objReader)) Then
                Throw New System.Exception(objBanco.MensagemErroDB)
            End If

            If objReader.HasRows Then
                objReader.Read()

                strSenhaBanco = objReader("ADES_SENHA_FORNECEDOR").ToString.Trim
                strSenhaBanco = objCripto.Descriptografar(strSenhaBanco)

                If strSenhaBanco <> strSenhaAtual Then
                    Throw New System.Exception("A senha atual digitada está incorreta. Tente novamente.")
                End If

                objReader.Close()
            Else
                Throw New System.Exception("Não foi encontrado o usuário para o email fornecido.")
            End If

            'Troca a senha
            strSQL = "UPDATE TFORNECEDOR" & _
                    " SET ADES_SENHA_FORNECEDOR = '" & objCripto.Criptografar(txtNovaSenha.Text.Trim) & "'" & _
                    " WHERE ADES_EMAIL_FORNECEDOR = '" & txtEmail.Text.Trim & "'"
            If Not (objBanco.ExecutaSQL(strSQL)) Then
                Throw New System.Exception(objBanco.MensagemErroDB)
            End If

            'Confirma a troca
            Dim strScript As String
            strScript = "<script language=""javascript"">"
            strScript += "alert('A senha de acesso foi alterada com sucesso');"
            strScript += "self.location.href='default.aspx';"
            strScript += "</script>"
            Me.RegisterStartupScript("", strScript)

        Catch ex As Exception
            With lblErro
                .Visible = True
                .Text = ex.Message.Trim
            End With

        Finally
            If Not IsNothing(objBanco) Then
                objBanco.FechaConexao()
                objBanco = Nothing
            End If
            objCripto = Nothing

        End Try
    End Sub
    Private Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        validaTroca()
    End Sub
End Class
