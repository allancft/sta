Imports System
Imports System.Security.Principal
Imports System.DirectoryServices
Imports System.Web
Imports System.Collections
Public Class usuariosinternos
    Inherits System.Web.UI.Page

    Protected WithEvents cboUsuarios As System.Web.UI.WebControls.DropDownList
    Protected WithEvents lblErro As System.Web.UI.WebControls.Label

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

        Dim Dominio As String = "LDAP://DOURADO"
        'Dim Senha As String = ""
        'Dim Usuario As String = "rsantos"
        Dim At As AuthenticationTypes = AuthenticationTypes.Secure

        'Dim DirEnt As New DirectoryEntry(Dominio, Usuario, Senha, At)
        Dim DirEnt As New DirectoryEntry(Dominio)

        'Try
        'MessageBox.Show(DirEnt.Username & ", bem vindo ao domínio: " & DirEnt.Name)
        'Catch ex As Exception
        'MessageBox.Show("Erro de autenticação")
        'End Try

        Dim search As New DirectorySearcher(DirEnt)
        search.Filter = "objectClass=User"

        Dim coll As SearchResultCollection

        Try
            coll = search.FindAll()

            Dim x As SearchResult

            For Each x In coll

                Dim propColl As ResultPropertyCollection
                propColl = x.Properties

                Dim myResultPropValueColl As ResultPropertyValueCollection
                myResultPropValueColl = propColl.Item("name")
                cboUsuarios.Items.Add(CStr(myResultPropValueColl.Item(0)))

            Next

        Catch ex As Exception
            lblerro.Text= ex.Message.ToString().Trim()
        End Try


    End Sub

End Class
