Partial Class popaplicativo
    Inherits clsLayoutPopup
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
#Region "Variáveis"

#End Region
#Region "Classes"
    Dim objAplicativo As clsAplicativo
    Dim objDB As clsDB
#End Region
#Region "Objetos Asp.Net"
#End Region
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If Not IsPostBack Then
            PaginaProtegida(Session("blnUsuarioAdmin"))
            Me.Titulo = "Aplicativos"
        End If
    End Sub
    Private Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Try
            objAplicativo = New clsAplicativo
            objDB = New clsDB

            If Not (objDB.AbreConexao()) Then
                Throw New System.Exception(objDB.MensagemErroDB)
            End If

            objAplicativo.objConexao = objDB

            If Not (objAplicativo.Incluir(txtDirRaiz.Text.Trim, _
                                        txtDescricao.Text.Trim, _
                                        StrErro)) Then
                Throw New System.Exception(StrErro)
            End If

            If Not (objDB.FechaConexao()) Then
                Throw New System.Exception(objDB.MensagemErroDB)
            End If

        Catch ex As Exception
            StrErro = ex.Message.Trim
            With lblErro
                .Visible = True
                .Text = StrErro
            End With

        Finally
            If StrErro = "" Then
                Me.RegisterStartupScript("confirma", "<script language=""javascript"">window.opener.navigate('confaplicativos.aspx');</script>")
            End If

        End Try
    End Sub
End Class