Imports System.Data
Imports System.Data.SqlClient
Partial Class cadfornecedor : Inherits clsLayoutBase
#Region "Variáveis e Objetos"
    Private strScript As String
    Private blnRet As Boolean
    Private objScript As clsScript
    Protected WithEvents btnIncluir As System.Web.UI.WebControls.Button
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
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PaginaProtegida(Session("blnUsuarioAdmin"))
        If Not IsPostBack Then
            dtgRegistros.CurrentPageIndex = 0
        End If
        Call PopulaGrid()
    End Sub
    'Robson 19/05/2011
    'Incluido parametro para fazer paginação
    Private Sub PopulaGrid(Optional ByVal strSort As String = "", Optional ByVal paginacao As Integer = 0)
		Dim objPagina As New sta.clsFornecedor
        Dim objDataSet As Data.DataSet
        Dim objConexao As New clsDB

        'Tenta abrir conexão e trata erro se houver.
        If Not objConexao.AbreConexao() Then
            strScript += "alert('" & objScript.limpaStringMensagem(objConexao.MensagemErroDB) & "');"
            objScript.eventoObjetoGenerico(Me, "body", "onload", strScript)
            Exit Sub
        End If

        'Atribui a conexão ativa para a classe do formulário
        objPagina.objConexao = objConexao

        blnRet = objPagina.ConsultaFornecedorLista(objDataSet, strErro, strSort)

        If Not blnRet Then
            strScript += "alert('" & objScript.limpaStringMensagem(strErro) & "');"
            objScript.eventoObjetoGenerico(Me, "body", "onload", strScript)
            Exit Sub
        End If

        Try
            dtgRegistros.CurrentPageIndex = paginacao
            dtgRegistros.DataSource = objDataSet
            dtgRegistros.DataBind()

        Catch objErro As System.Exception
            strErro = objErro.Message.ToString().Trim()
            strScript += "alert('" & objScript.limpaStringMensagem(strErro) & "');"
            objScript.eventoObjetoGenerico(Me, "body", "onload", strScript)

        Finally
            objConexao.FechaConexao()
            dtgRegistros.Dispose()

        End Try
    End Sub
    Private Sub dtgRegistros_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgRegistros.ItemDataBound
        If e.Item.ItemType = Web.UI.WebControls.ListItemType.Header Or _
            e.Item.ItemType = Web.UI.WebControls.ListItemType.Footer Then Exit Sub

        Dim id As String = dtgRegistros.DataKeys(e.Item.ItemIndex)
        e.Item.Attributes.Add("onclick", "javascript:abrirJanela('popfornecedor.aspx?codFornecedor=" & id & "', 360, 335, false);")
    End Sub
    'Robson 19/05/2011
    'Corrigido para fazer a paginação
    Private Sub dtgRegistros_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgRegistros.PageIndexChanged
        Call PopulaGrid("", e.NewPageIndex)
    End Sub
    Private Sub dtgRegistros_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dtgRegistros.SortCommand
        Call PopulaGrid(e.SortExpression)
    End Sub
End Class