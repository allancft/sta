Public Class calendario
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents calendario As System.Web.UI.WebControls.Calendar
    Protected WithEvents hidControle As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents frmSTA As System.Web.UI.HtmlControls.HtmlForm

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
        hidControle.Value = Request.QueryString("textbox").ToString()
    End Sub
    Private Sub thedate_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles calendario.SelectionChanged
        Dim strScript As String
        strScript = "<script language=""javascript"">" & vbCrLf
        strScript += "window.opener.document.forms[0]." + hidControle.Value + ".value = '"
        strScript += calendario.SelectedDate.ToString("dd/MM/yyyy") & "';" & vbCrLf
        strScript += "self.close();" & vbCrLf
        strScript += "</script>" & vbCrLf
        RegisterClientScriptBlock("", strScript)
    End Sub
End Class