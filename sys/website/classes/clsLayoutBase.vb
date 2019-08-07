Imports System.Web
Public Class clsLayoutBase : Inherits System.Web.UI.Page
#Region "Variáveis"
    Protected StrErro As String
#End Region
    Public Sub PaginaProtegida(ByVal blnAdmin As Boolean)
        If Not blnAdmin Then
            Response.Redirect("htmacessonegado.htm", True)
        End If
    End Sub
    Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
        'Trata o TimeOut de sessao
        If HttpContext.Current.Session.IsNewSession Then
            Response.Redirect("../default.aspx")
        End If

        'Adiciona o Webcontrol de Cabecalho
        'Inicia a Classe
        'Adiciona o Webcontrol de Rodape
        Me.Controls.AddAt(0, LoadControl("common/webcontrols/wbcCabecalho.ascx"))
        MyBase.OnInit(e)
        Me.Controls.Add(LoadControl("common/webcontrols/wbcRodape.ascx"))
    End Sub
End Class
Public Class LayoutBase : Inherits System.Web.UI.UserControl
#Region "Variáveis"
    Protected BlnProtegida As Boolean
#End Region
#Region "Propriedades"
    Public WriteOnly Property PropProtegida() As Boolean
        Set(ByVal Value As Boolean)
            BlnProtegida = Value
        End Set
    End Property
#End Region
    Public Shadows ReadOnly Property Page() As clsLayoutBase
        Get
            Return CType(MyBase.Page, clsLayoutBase)
        End Get
    End Property
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then 'Limpa DataSet de Arquivos Enviados e Destinatários
            'Session("objDataSetDestinos") = Nothing
            Session("objDataSetArquivos") = Nothing
        End If
    End Sub
End Class