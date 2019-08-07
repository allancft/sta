Imports System.IO
Imports System.Data
Imports System.Web.Security
Imports System.Configuration
Imports System.Data.SqlClient
Partial Class chave
    Inherits System.Web.UI.Page
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
#Region "Variavies e objetos"
    'Variveis
    Private strErro, strSQL As String
    'Private strDir As String = ConfigurationSettings.AppSettings("diretorioArquivos")

    'objetos
    Dim objCriptografia As aim.objCriptografia
    Dim objBanco As clsDB
    Dim objScript As clsScript

#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Request.QueryString.Count = 0 Then
            Response.Redirect("default.aspx", True)
            Exit Sub
        End If
        ValidaChave(Request.QueryString.Get("chave").Trim())
    End Sub
    Private Sub ValidaChave(ByVal strChave As String)
        Try
            objCriptografia = New aim.objCriptografia
            objBanco = New clsDB

            Dim objReader As SqlDataReader

            If Not objBanco.AbreConexao() Then
                Throw New System.Exception(objBanco.MensagemErroDB)
            End If

            strSQL = "SELECT ANOM_CHAVE_TEMPORARIA, ADES_REMETENTE" & _
                " FROM TCHAVE_TEMPORARIA" & _
                " WHERE ANOM_CHAVE_TEMPORARIA = '" & strChave & "'"

            If Not (objBanco.ExecuteReader(strSQL, objReader)) Then
                Throw New System.Exception(objBanco.MensagemErroDB())
            End If

            objReader.Read()

            If objReader.HasRows Then 'Retornou registro

                Session("blnUsuarioAdmin") = False
                Session("strDir") = strChave
                Session("usuario") = strChave
                Dim strRemetenteTemp As String = objReader("ADES_REMETENTE").ToString.Trim()
                strRemetenteTemp = strRemetenteTemp.Substring(0, InStrRev(strRemetenteTemp, "@") - 1)
                Session("remetente") = strRemetenteTemp
                Session("tipoAcesso") = 3 'temporario

                objReader.Close()

                'exclui chave temporaria
                strSQL = "DELETE TCHAVE_TEMPORARIA" & _
                    " WHERE ANOM_CHAVE_TEMPORARIA = '" & strChave & "'"
                If Not (objBanco.ExecutaSQL(strSQL)) Then
                    Throw New System.Exception(objBanco.MensagemErroDB())
                End If

            Else
                Throw New System.Exception("Chave temporária de acesso não encontrada.")

            End If

        Catch objErro As System.Exception
            strErro = objErro.Message.Trim()
            objScript = New clsScript
            strErro = objScript.limpaStringMensagem(strErro)
            objScript.eventoObjetoGenerico(Me, "body", "onload", "alert('" & strErro & "');")

        Finally
            If Not IsNothing(objBanco) Then
                objBanco.FechaConexao()
            End If

        End Try

        If strErro = "" Then
            FormsAuthentication.RedirectFromLoginPage(strChave, False)
			Response.Redirect("pages/")
        End If
    End Sub
End Class
