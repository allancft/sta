Imports System.Configuration
Imports System.DirectoryServices
Imports System.Data
Partial Class ad : Inherits clsLayoutPopup
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
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PaginaProtegida(Session("blnUsuarioAdmin"))
        If Not IsPostBack Then dtgUsu.CurrentPageIndex = 0
    End Sub
    Private Sub PopulaGrid()

        Dim objDataSet As New DataSet
        Dim objADSI As New clsADSI
        Dim strErro As String

        Try
            objDataSet = objADSI.pesquisaUsuario(txtPesquisa.Text, _
                                                strErro)
            dtgUsu.DataSource = objDataSet
            dtgUsu.DataBind()

        Catch objErro As Exception
            lblErro.Text = strErro

        Finally
            dtgUsu.Dispose()

        End Try

    End Sub
    Private Sub dtgUsu_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgUsu.PageIndexChanged

        dtgUsu.CurrentPageIndex = e.NewPageIndex
        Call PopulaGrid()

    End Sub
    Private Sub btnPesquisa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPesquisa.Click

        dtgUsu.CurrentPageIndex = 0
        Call PopulaGrid()

    End Sub
    'Private Sub dtgUsu_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgUsu.ItemDataBound
    Private Sub dtgUsu_EditCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgUsu.EditCommand
        Dim strUsuario As String = dtgUsu.DataKeys(e.Item.ItemIndex)
        Me.RegisterStartupScript("onload", "<script language=""javascript"">retornaUsuario('" & strUsuario & "');</script>")
        '    e.Item.Attributes.Add("onclick", "javascript:retornaUsuario('" & strUsuario & "');")
    End Sub
End Class
