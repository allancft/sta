Imports System.Data.SqlClient
Partial Class confextensoes
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
#Region "Objetos"
    Private objDB As clsDB
    Private objConfiguracao As clsConfiguracao
#End Region
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PaginaProtegida(Session("blnUsuarioAdmin"))
        If Not IsPostBack Then PopulaCampos()
    End Sub
    Private Sub PopulaCampos()

        Dim objReader As SqlDataReader

        Try
            objDB = New clsDB
            objConfiguracao = New clsConfiguracao

            If Not (objDB.AbreConexao()) Then
                Throw New System.Exception(objDB.MensagemErroDB)
            End If

            'Atribui a conexão ativa para a classe do formulário
            objConfiguracao.objConexao = objDB

            If Not (objConfiguracao.ConfiguracaoConsultar(objReader, strErro)) Then
                Throw New System.Exception(objDB.MensagemErroDB)
            End If

            objReader.Read()

            If objReader.HasRows Then
                txtExtensoesInt.Text = objReader("AFTP_EXT_ARQU_USER_INTERNO").ToString().Trim()
                txtExtensoesExt.Text = objReader("AFTP_EXT_ARQU_USER_FORNECEDOR").ToString().Trim()
            End If

            objReader.Close()

            If Not (objDB.FechaConexao()) Then
                Throw New System.Exception(objDB.MensagemErroDB)
            End If

        Catch ex As Exception
            With lblErro
                .Visible = True
                .Text = ex.Message.Trim
            End With

        Finally
            If Not IsNothing(objDB) Then objDB = Nothing
            If Not IsNothing(objReader) Then objReader = Nothing
            If Not IsNothing(objConfiguracao) Then objConfiguracao = Nothing

        End Try
    End Sub
    Private Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Try
            objDB = New clsDB
            objConfiguracao = New clsConfiguracao

            If Not (objDB.AbreConexao()) Then
                Throw New System.Exception(objDB.MensagemErroDB)
            End If

            objConfiguracao.objConexao = objDB

            If Not (objConfiguracao.Salvar(txtExtensoesInt.Text.Trim, _
                                        txtExtensoesExt.Text.Trim, _
                                        Me.StrErro)) Then
                Throw New System.Exception(Me.StrErro)
            End If

            If Not (objDB.FechaConexao()) Then
                Throw New System.Exception(objDB.MensagemErroDB)
            End If

        Catch ex As Exception
            With lblErro
                .Visible = True
                .Text = ex.Message.Trim
            End With

        Finally
            If Not IsNothing(objDB) Then objDB = Nothing
            If Not IsNothing(objConfiguracao) Then objConfiguracao = Nothing

        End Try
    End Sub
End Class
