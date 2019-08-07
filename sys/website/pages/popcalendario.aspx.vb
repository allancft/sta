Imports System.Web
Partial Class popcalendario : Inherits clsLayoutPopup
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
#Region "Variáveis"
    Private strNomeCampo As String
    Private strScript As String
#End Region
    Private Sub calData_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calData.SelectionChanged
        strScript = "<script language=""javascript"">" & vbCrLf
        strScript += "window.opener.document.forms[0]." & strNomeCampo.Trim & ".value='" & Format(calData.SelectedDate, "dd/MM/yyyy") & "';" & vbCrLf
        strScript += "self.close();"
        strScript += "</script>"
        Me.RegisterStartupScript("", strScript)
    End Sub
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        strNomeCampo = HttpContext.Current.Request.QueryString("textbox").Trim
    End Sub
End Class
