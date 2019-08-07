Imports System.Data
Imports System.Data.SqlClient
Partial Class popfornconsulta : Inherits clsLayoutPopup
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
#Region "Objetos"
#End Region
#Region "Classes"
    Private objPesquisa As clsFornecedor
    Private objConexao As clsDB
#End Region
#Region "Variaveis"
    Private strNomeCampo As String
#End Region
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strNomeCampo = Request.QueryString.Get("campo").Trim
    End Sub
    Private Sub btnPesquisa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPesquisa.Click
        Dim objDataSet As DataSet
        Dim strDestino = txtDestino.Text.Trim
        Try
            objPesquisa = New clsFornecedor
            objConexao = New clsDB
            If Not (objConexao.AbreConexao) Then
                Throw New System.Exception(StrErro)
            End If
            objPesquisa.objConexao = objConexao
            If Not (objPesquisa.ConsultaDestinoFornecedor(StrErro, _
                                            objDataSet, _
                                            strDestino)) Then
                Throw New System.Exception(StrErro)
            End If

            If objDataSet.Tables(0).Rows.Count = 0 Then
                Me.RegisterStartupScript("alerta", "<script language=""javascript"">alert('Fornecedor não encontrado.');history.back();</script>")
                objConexao.FechaConexao()
                objConexao = Nothing
                Exit Try
            End If

            With dtgDestinos
                .DataSource = objDataSet
                .DataBind()
            End With

        Catch ex As System.Exception
            lblErro.Visible = True
            lblErro.Text = ex.Message.Trim

        End Try
    End Sub
    Private Sub dtgDestinos_EditCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgDestinos.EditCommand
        Dim strScript As String
        strScript = "<script language=""javascript"">" & vbCrLf
        strScript += "window.opener.document.forms[0]." & strNomeCampo.Trim & ".value='" & e.Item.Cells(1).Text.Trim & "';" & vbCrLf
        strScript += "self.close();"
        strScript += "</script>"
        Me.RegisterStartupScript("", strScript)
    End Sub
End Class
