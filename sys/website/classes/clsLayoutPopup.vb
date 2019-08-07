Imports System.Configuration
Public Class clsLayoutPopup : Inherits System.Web.UI.Page
    Private _strTituloPagina As String
    Private _strTela As String
    Public StrErro As String
    Protected BlnProtegida As Boolean
#Region "Propriedades"
    Public Property Titulo() As String
        Set(ByVal Value As String)
            _strTituloPagina = ConfigurationSettings.AppSettings("nomeSistema").Trim & " - " & Value
        End Set
        Get
            Return _strTituloPagina
        End Get
    End Property
#End Region
    Public Sub PaginaProtegida(ByVal blnAdmin As Boolean)
        If Not blnAdmin Then
            Response.Redirect("htmacessonegado.htm", True)
        End If
    End Sub
    Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
        Me.Controls.AddAt(0, LoadControl("common/webcontrols/wbcCabecalhoPopup.ascx"))
        MyBase.OnInit(e)
        Me.Controls.Add(LoadControl("common/webcontrols/wbcRodapePopup.ascx"))
    End Sub
End Class
Public Class LayoutPopup : Inherits System.Web.UI.UserControl
    Public Shadows ReadOnly Property Page() As clsLayoutPopup
        Get
            Return CType(MyBase.Page, clsLayoutPopup)
        End Get
    End Property
End Class