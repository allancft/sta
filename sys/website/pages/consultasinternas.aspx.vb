Imports System.Data
Imports System.Data.SqlClient
Partial Class consultasinternas
    Inherits clsLayoutBase
#Region "Objetos e Variáveis"

    Private objBanco As clsDB
    Private objScript As clsScript
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
        Session("objDataSetDestinos") = Nothing
        Session("objDataSetArquivos") = Nothing

        ExibeCalendario(btnCalInicial, txtDataInicial)
        ExibeCalendario(btnCalFinal, txtDataFinal)
    End Sub
    Private Sub btnConsultar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConsultar.Click

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

            If Not (objPagina.ConsultaInterna(objDataSet, _
                                            strErro, _
                                            strSort, _
                                            txtNome.Text.Replace(";", ""), _
                                            txtDataInicial.Text, _
                                            txtDataFinal.Text)) Then
                Throw New System.Exception(strErro)
            End If

            dtgRegistros.DataSource = objDataSet
            dtgRegistros.DataBind()

        Catch objErro As Exception
            objScript = New clsScript
            objScript.eventoObjetoGenerico(Me, "body", "onload", "alert('" & _
                                            objScript.limpaStringMensagem(objErro.Message.Trim()) & _
                                            "');")
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
    Private Sub AbrePopup(ByVal opener As System.Web.UI.WebControls.WebControl, ByVal PagePath As String, ByVal windowName As String, ByVal width As Integer, ByVal height As Integer)
        Dim strScript, strAtributos As String

        strAtributos = "width=" & width & "px," & _
                        "height=" & height & "px," & _
                        "left='+((screen.width -" & width & ") / 2)+'," & _
                        "top='+ (screen.height - " & height & ") / 2+'"


        strScript = "window.open('" & PagePath & "','" & windowName & "','" & strAtributos & "');return false;"
        opener.Attributes.Add("onClick", strScript)
    End Sub
    Private Sub ExibeCalendario(ByVal opener As System.Web.UI.WebControls.WebControl, ByVal dateControl As System.Web.UI.WebControls.WebControl)
        AbrePopup(opener, "popcalendario.aspx?textbox=" & dateControl.ClientID, "calendar", 300, 225)
    End Sub
    Private Sub dtgRegistros_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dtgRegistros.ItemDataBound
        If e.Item.ItemType = Web.UI.WebControls.ListItemType.Header Or e.Item.ItemType = Web.UI.WebControls.ListItemType.Footer Then
            Exit Sub
        End If
        e.Item.Attributes.Add("onclick", "abrirJanela('popconsarquivosint.aspx?codigo=" & e.Item.Cells.Item(0).Text.Trim & "', 380, 400, false);")
    End Sub
End Class