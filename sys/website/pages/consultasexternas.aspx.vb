Imports System.Data
Imports System.Data.SqlClient
Partial Class consultasexternas
    Inherits clsLayoutBase
#Region "Objetos e Variáveis"

    Private objBanco As clsDB
    Private objScript As clsScript
    Private intTipoAcesso As Integer

    Private objPagina As clsConsulta
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
        Try
            intTipoAcesso = Session("tipoAcesso")

            If intTipoAcesso = 1 Then
                trAdminTit.Visible = True
                trAdminCampos.Visible = True
                If Not IsNothing(Session("objDataSetDestinos")) Then Session("objDataSetDestinos") = Nothing
                If Not IsNothing(Session("objDataSetArquivos")) Then Session("objDataSetArquivos") = Nothing

                dtgRegistros.CurrentPageIndex = 0
                Call PopulaGrid()
            End If

        Catch When IsNothing(Session("tipoAcesso"))
            With lblErro
                .Visible = True
                .Text = "Sua sessão expirou, conecte-se novamente ao sistema"
            End With
            Response.End()

        Catch ex As Exception
            With lblErro
                .Visible = True
                .Text = ex.Message.Trim
            End With

        End Try
    End Sub
    Private Sub btnConsultar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConsultar.Click

        Session("objDataSetDestinos") = Nothing
        Session("objDataSetArquivos") = Nothing

        dtgRegistros.CurrentPageIndex = 0
        Call PopulaGrid()

    End Sub
    Private Sub dtgRegistros_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgRegistros.PageIndexChanged

        dtgRegistros.CurrentPageIndex = e.NewPageIndex
        Call PopulaGrid()

    End Sub
    Private Sub PopulaGrid(Optional ByVal strSort As String = "")

        objBanco = New clsDB
        objPagina = New clsConsulta

        Dim objDataSet As Data.DataSet

        Try
            If Not (objBanco.AbreConexao(Application("strCn"))) Then
                Throw New System.Exception(objBanco.MensagemErroDB)
            End If

            objPagina.objConexao = objBanco

            Dim blnRet As Boolean

            If intTipoAcesso = 1 Then 'usuario interno com acesso aos filtros

                blnRet = (objPagina.ConsultaExterna(objDataSet, _
                                                strErro, _
                                                strSort, _
                                                txtNome.Text, _
                                                cboTipo.SelectedValue, _
                                                IIf(chkValidados.Checked, Convert.ToInt32(chkValidados.Checked), -1), _
                                                IIf(chkExcluidos.Checked, Convert.ToInt32(chkExcluidos.Checked), -1)))

            ElseIf intTipoAcesso = 2 Then 'fornecedor consultando apenas suas transmissoes

                blnRet = (objPagina.ConsultaExterna(objDataSet, _
                                                strErro, _
                                                strSort, _
                                                , _
                                                cboTipo.SelectedValue.Trim, _
                                                IIf(chkValidados.Checked, Convert.ToInt32(chkValidados.Checked), -1), _
                                                IIf(chkExcluidos.Checked, Convert.ToInt32(chkExcluidos.Checked), -1), _
                                                Session("codSeq")))
            End If

            If Not blnRet Then
                Throw New System.Exception(strErro)
            End If

            dtgRegistros.DataSource = objDataSet
            dtgRegistros.DataBind()

        Catch objErro As Exception
            lblErro.Text = objErro.Message.Trim()

        Finally
            objBanco.FechaConexao()

            objDataSet = Nothing
            objScript = Nothing
            objBanco = Nothing
            objPagina = Nothing

        End Try

    End Sub
    Private Sub dtgRegistros_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dtgRegistros.SortCommand
        Call PopulaGrid(e.SortExpression)
    End Sub
    Private Sub dtgRegistros_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgRegistros.ItemDataBound
        If e.Item.ItemType = Web.UI.WebControls.ListItemType.Header Or e.Item.ItemType = Web.UI.WebControls.ListItemType.Footer Then
            Exit Sub
        End If
        e.Item.Attributes.Add("onclick", "abrirJanela('popconsarquivosext.aspx?codigo=" & e.Item.Cells.Item(0).Text.Trim & "', 380, 400, false);")
    End Sub
End Class