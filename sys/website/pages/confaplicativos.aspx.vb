Partial Class confaplicativos
    Inherits clsLayoutBase
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
#Region "Classes"
    Dim objAplicativo As clsAplicativo
    Dim objConexao As clsDB
#End Region
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PaginaProtegida(Session("blnUsuarioAdmin"))
        If Not IsPostBack Then
            dtgRegistros.CurrentPageIndex = 0
            Call PopulaGrid()
        End If
    End Sub
    Private Sub PopulaGrid()
        Dim objDataSet As Data.DataSet
        Try
            objDataSet = New Data.DataSet
            objAplicativo = New clsAplicativo
            objConexao = New clsDB

            If Not (objConexao.AbreConexao()) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            objAplicativo.objConexao = objConexao

            If Not (objAplicativo.Consultar(objDataSet, _
                                            Me.StrErro)) Then
                Throw New System.Exception(Me.StrErro)
            End If

            With dtgRegistros
                .DataSource = objDataSet
                .DataBind()
            End With

            If Not (objConexao.FechaConexao()) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

        Catch ex As System.Exception
            With lblErro
                .Visible = True
                .Text = ex.Message.Trim
            End With

        Finally
            If Not IsNothing(objConexao) Then objConexao = Nothing
            If Not IsNothing(objDataSet) Then objDataSet = Nothing
            If Not IsNothing(objAplicativo) Then objAplicativo = Nothing

        End Try
    End Sub
    Private Sub dtgRegistros_EditCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgRegistros.EditCommand
        dtgRegistros.EditItemIndex = e.Item.ItemIndex()
        PopulaGrid()
    End Sub
    Private Sub dtgRegistros_CancelCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgRegistros.CancelCommand
        dtgRegistros.EditItemIndex = -1
        PopulaGrid()
    End Sub
    Private Sub dtgRegistros_UpdateCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgRegistros.UpdateCommand
        Try

            objAplicativo = New clsAplicativo
            objConexao = New clsDB

            If Not (objConexao.AbreConexao()) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            objAplicativo.objConexao = objConexao

            If Not (objAplicativo.Editar(CType(e.Item.FindControl("txtDir"), Web.UI.WebControls.TextBox).Text, _
                                        CType(e.Item.FindControl("txtNome"), Web.UI.WebControls.TextBox).Text, _
                                        CType(e.Item.Cells(2).Text, Integer), _
                                        strErro)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            If Not (objConexao.FechaConexao()) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

        Catch ex As System.Exception
            With lblErro
                .Visible = True
                .Text = ex.Message.Trim
            End With

        Finally
            If Not IsNothing(objConexao) Then objConexao = Nothing
            If Not IsNothing(objAplicativo) Then objAplicativo = Nothing

        End Try

        dtgRegistros.EditItemIndex = -1
        PopulaGrid()
    End Sub
    Protected Sub Excluir(ByVal source As Object, ByVal e As System.EventArgs)
        Try
            objAplicativo = New clsAplicativo
            objConexao = New clsDB

            If Not (objConexao.AbreConexao()) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            objAplicativo.objConexao = objConexao

            Dim intCod As Integer = CInt(source.HRef.Trim)

            If Not (objAplicativo.Excluir(intCod, _
                                        strErro)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            If Not (objConexao.FechaConexao()) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

        Catch ex As System.Exception
            With lblErro
                .Visible = True
                .Text = ex.Message.Trim
            End With

        Finally
            If Not IsNothing(objConexao) Then objConexao = Nothing
            If Not IsNothing(objAplicativo) Then objAplicativo = Nothing

        End Try

        dtgRegistros.EditItemIndex = -1
        PopulaGrid()
    End Sub
End Class