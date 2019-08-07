Imports System.Web
Imports System.Web.SessionState
Imports System.Globalization
Imports System.Threading
Imports System.Web.Security

Public Class [Global]
    Inherits System.Web.HttpApplication

#Region " Component Designer Generated Code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Component Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Component Designer
    'It can be modified using the Component Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container()
    End Sub

#End Region
    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        Application("nameApp") = "Itambé - STA : Sistema de Transferência de Arquivos"
    End Sub
    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)

    End Sub
    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        'Definindo o Regional Settings da aplicação para o
        'Padrão Brasileiro
        Dim objRegional As CultureInfo = New CultureInfo("pt-BR")
        System.Threading.Thread.CurrentThread.CurrentCulture = objRegional
        objRegional = Nothing
    End Sub
    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub
    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when an error occurs
    End Sub
    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)

    End Sub
    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub

End Class