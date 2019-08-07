Imports System.Data
Imports System.Data.SqlClient
Partial Class popdestinos
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
#Region "Objetos"
#End Region
#Region "Classes"
    Private objScript As clsScript
    Private objPesquisa As clsADSI
    Private objPagina As clsEnvio
    Private objConexao As clsDB
#End Region
#Region "Variáveis"
    Private intAcao As Integer
#End Region
#Region "Enumeradores"
    Public Enum EnumTipoEnvio
        Interno = 1
        Externo = 2
        Temporario = 3
    End Enum
#End Region
#Region "Propriedades"
    Private Property TipoEnvio() As EnumTipoEnvio
        Get
            Return intAcao
        End Get
        Set(ByVal Value As EnumTipoEnvio)
            intAcao = Value
        End Set
    End Property
#End Region
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.TipoEnvio = CType(Request.QueryString.Get("tpEnvio"), Integer)
        If Not IsPostBack Then
            Me.Titulo = "Selecionar Destino"
            dtgDestinos.CurrentPageIndex = 0
            If TipoEnvio = EnumTipoEnvio.Temporario Then
                lblTitulo.Text = "E-Mail: "
                btnPesquisa.Text = "Incluir"
            End If
        End If
    End Sub
    'Robson 19/05/2011 
    'Adicionado no método o parametro de paginação
    Private Sub AdicionaDestino(Optional ByVal strDestino As String = "", Optional ByVal paginacao As Integer = 0)

        Dim objDataSet As DataSet

        Select Case TipoEnvio
            Case EnumTipoEnvio.Interno
                Try
                    Dim objPesquisa As New clsADSI

                    objDataSet = objPesquisa.pesquisaUsuario(strDestino, _
                                                    StrErro)

                    If objDataSet Is Nothing Then
                        Throw New System.Exception(StrErro)
                    End If

                    If objDataSet.Tables(0).Rows.Count = 0 Then
                        Throw New System.Exception("Usuário não encontrado")
                    End If

                    With dtgDestinos
                        .CurrentPageIndex = paginacao
                        .DataSource = objDataSet
                        .DataBind()
                    End With

                Catch objErro As Exception
                    Me.RegisterStartupScript("", "<script language=""javascript"">alert('" & objerro.Message.Trim() & "');history.back();</script>")

                Finally
                    objScript = Nothing

                End Try

                objPesquisa = Nothing

            Case EnumTipoEnvio.Externo

                Dim objPesquisa As clsFornecedor
                objpesquisa = New clsFornecedor
                objConexao = New clsDB

                Try
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
                        objScript = New clsScript
                        objScript.eventoObjetoGenerico(Me, "body", "onload", _
                                            "alert('Fornecedor não encontrado.');history.back();")
                        objConexao.FechaConexao()
                        objConexao = Nothing
                        objScript = Nothing
                        Exit Select
                    End If

                    With dtgDestinos
                        .CurrentPageIndex = paginacao
                        .DataSource = objDataSet
                        .DataBind()
                    End With

                Catch objErro As Exception
                    objScript = New clsScript
                    objScript.eventoObjetoGenerico(Me, "body", "onload", _
                        "alert('" & objScript.limpaStringMensagem(objerro.Message.Trim()) & "');history.back();")
                    objScript = Nothing

                End Try

                objConexao.FechaConexao()
                objConexao = Nothing
                objPesquisa = Nothing

            Case EnumTipoEnvio.Temporario

                'Valida o Email antes de incluir novo destino
                Dim objUtil As New clsUtil
                If Not (objUtil.ValidaEmail(strDestino)) Then
                    objScript = New clsScript
                    objScript.eventoObjetoGenerico(Me, "body", "onload", _
                                        "alert('Endereço de email inválido');")
                    Exit Sub
                End If
                Call PopulaGridDestinos(strDestino)
        End Select

    End Sub
    Private Sub btnPesquisa_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPesquisa.Click
        AdicionaDestino(txtDestino.Text, 0)
    End Sub
    'Robson 19/05/2011 
    'Para a páginação foi chamado o método AdicionarDestino, passando a nova página. 
    Private Sub dtgDestinos_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dtgDestinos.PageIndexChanged
        Call AdicionaDestino("", e.NewPageIndex)
    End Sub
    Private Sub PopulaGridDestinos(ByVal strLogin As String, _
                                    Optional ByVal strNome As String = "", _
                                    Optional ByVal strDir As String = "", _
                                    Optional ByVal intCodSeq As Integer = 0)
        Try
            Dim objDataSet As DataSet
            Dim objTabela As DataTable
            Dim objLinha As DataRow

            objTabela = New DataTable
            objDataSet = New DataSet

            With objTabela
                .TableName = "destinos"
                .Columns.Add("Login")
                .Columns.Add("Nome")
                .Columns.Add("diretorio")
                .Columns.Add("codigo")
            End With

            If Not IsNothing(Session("objDataSetDestinos")) Then
                Dim objDataSetCopia As DataSet
                objDataSetCopia = New DataSet
                objDataSetCopia = Session("objDataSetDestinos").Copy

                Dim intControle As Integer
                For intControle = 0 To (objDataSetCopia.Tables(0).Rows.Count - 1)
                    objLinha = objTabela.NewRow()
                    objLinha("Login") = objDataSetCopia.Tables(0).Rows(intControle).Item("Login")
                    objLinha("Nome") = objDataSetCopia.Tables(0).Rows(intControle).Item("Nome")
                    objLinha("diretorio") = objDataSetCopia.Tables(0).Rows(intControle).Item("diretorio")
                    objLinha("codigo") = objDataSetCopia.Tables(0).Rows(intControle).Item("codigo")
                    objTabela.Rows.Add(objLinha)
                Next
            End If

            objPagina = New clsEnvio
            objConexao = New clsDB

            Dim strCodEnvio As String

            If Not (objConexao.AbreConexao) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            objPagina.objConexao = objConexao

            Dim blnRet As Boolean
            Dim intTipoAcesso As Integer
            intTipoAcesso = CType(Session("tipoAcesso"), Integer)

            Select Case TipoEnvio
                Case EnumTipoEnvio.Interno, intTipoAcesso = 1
                    blnRet = (objPagina.IncluirTransmissaoInterna(strCodEnvio, _
                                                                    Session("usuario"), _
                                                                    strLogin, _
                                                                    StrErro))
                Case EnumTipoEnvio.Externo
                    blnRet = (objPagina.IncluiTransmissaoExterna(strCodEnvio, _
                                                                strDir, _
                                                                intTipoAcesso, _
                                                                intCodSeq, _
                                                                StrErro))
                Case EnumTipoEnvio.Temporario
                    blnRet = (objPagina.IncluirTransmissaoTemporaria(StrErro, _
                                                                    strCodEnvio, _
                                                                    Session("usuario")))
            End Select

            If Not blnRet Then
                Throw New System.Exception(StrErro)
            End If

            objLinha = objTabela.NewRow()
            objLinha("Login") = strLogin

            If TipoEnvio = EnumTipoEnvio.Externo Then 'salva o arquivo no diretorio correspondente no baan

            End If

            objLinha("Nome") = strNome
            objLinha("diretorio") = (IIf(Me.TipoEnvio = EnumTipoEnvio.Temporario, "temporario\" & strCodEnvio, _
                                    IIf(Me.TipoEnvio = EnumTipoEnvio.Interno, "interno\", "externo\") & _
                                    IIf(Me.TipoEnvio = EnumTipoEnvio.Externo, strDir, strLogin) & "\recebidas\" & strCodEnvio))
            objLinha("codigo") = strCodEnvio
            objTabela.Rows.Add(objLinha)

            objDataSet.Tables.Add(objTabela)

            Session("objDataSetDestinos") = objDataSet

            objDataSet = Nothing

        Catch ex As Exception
            StrErro = ex.Message.Trim()
            lblErro.Text = StrErro
            lblErro.Visible = True

        Finally
            If StrErro = "" Then
                Me.RegisterStartupScript("reload", "<script language=""javascript"">window.opener.document.getElementById('frmSTA').submit();</script>")
            End If
            objConexao.FechaConexao()

        End Try
    End Sub
    Private Sub dtgDestinos_EditCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgDestinos.EditCommand
        PopulaGridDestinos(e.Item.Cells(1).Text.Trim(), e.Item.Cells(2).Text.Trim(), e.Item.Cells(3).Text.Trim(), e.Item.Cells(4).Text.Trim())
    End Sub
End Class