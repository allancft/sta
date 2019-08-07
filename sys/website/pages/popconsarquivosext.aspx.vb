Imports System.Data.SqlClient
Imports System.Data
Partial Class popconsarquivosext
    Inherits clsLayoutPopup

    'Objetos Asp.Net e Variaveis

    'Objetos (classes)
    Protected objDB As clsDB
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
        If Request.QueryString.Count < 1 Then
            Response.Write("Navegacao incorreta")
            Exit Sub
        End If

        Try
            objDB = New clsDB

            If Not (objDB.AbreConexao()) Then
                Throw New System.Exception(objDB.MensagemErroDB)
            End If

            Dim intCodigo As Integer
            intCodigo = Convert.ToInt32(Request.QueryString("codigo"))

            Dim objDataSet As DataSet
            objDataSet = New DataSet

            Dim objConsulta As clsConsulta
            objConsulta = New clsConsulta
            objConsulta.objConexao = objDB

            If Not (objConsulta.ConsultaArquivosExternos(intCodigo, _
                                                        objDataSet, _
                                                        StrErro)) Then
                Throw New System.Exception(StrErro)
            End If

            With dtgArquivos
                .DataSource = objDataSet
                .DataBind()
            End With

            objDB.FechaConexao()

            lblCodigo.Text = "Arquivos da transmissão código: " & intCodigo

        Catch ex As System.Exception
            With lblErro
                .Visible = True
                .Text = ex.Message.Trim
            End With

        End Try
    End Sub
End Class
