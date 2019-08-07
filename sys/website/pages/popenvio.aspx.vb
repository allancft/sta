Imports System.Data
Imports System.Configuration
Public Class popenvio
    Inherits System.Web.UI.Page
#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents lblConfirmacao As System.Web.UI.WebControls.Label

    'NOTE: The following placeholder declaration is required by the Web Form Designer.
    'Do not delete or move it.
    Private designerPlaceholderDeclaration As System.Object

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region
#Region "Variáveis e Objetos e Enumeradores"
    Private strCaminhoServidor, strScript, strSQL, strErro As String
    Private objPagina As clsEnvio
    Private objConexao As clsDB
    Private objScript As clsScript
    Public Shared intAcao As Integer
    Private strExtensoes As String

    Protected WithEvents btnEnvio As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents txtarquivo As System.Web.UI.HtmlControls.HtmlInputFile
    Protected WithEvents lblErro As System.Web.UI.WebControls.Label
    Public Enum EnumTipoEnvio
        Interno = 1
        Externo = 2
        Temporario = 3
    End Enum
    Public Shared Property TipoEnvio() As EnumTipoEnvio
        Get
            Return intAcao
        End Get
        Set(ByVal Value As EnumTipoEnvio)
            intAcao = Value
        End Set
    End Property
#End Region
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If TipoEnvio = EnumTipoEnvio.Temporario Or Session("TipoAcesso") = 3 Then Exit Sub
        If sta.envio.dtgDestinos.Items.Count = 0 Then
            objScript = New clsScript
            objScript.eventoObjetoGenerico(Me, "body", "onload", "alert('Selecione os destinos antes de incluir arquivos.');self.close();")
        End If
    End Sub
    Private Function ValidaFormulario() As Boolean

        ValidaFormulario = True

        Try
            If txtarquivo.Value = "" Then
                Throw New System.Exception("alert('Selecione um arquivo para ser enviado.');")
            End If

            ExtensoesPermitidas()

            Dim strArquivoAux As String
            strArquivoAux = txtarquivo.Value.Trim().Substring(txtarquivo.Value.Length - 3)
            Dim arrExt As Array
            arrExt = strExtensoes.Split(";")

            For intAcao = 0 To arrExt.GetUpperBound(0)
                If strArquivoAux.IndexOf(arrExt(intAcao)) > -1 Then
                    Exit Function
                End If
            Next

            Throw New System.Exception("alert('O arquivo selecionado não está entre os tipos de arquivo permitidos para envio.');")

        Catch ex As System.Exception
            strScript = ex.Message.Trim()
            objScript = New clsScript
            objScript.eventoObjetoGenerico(Me, "body", "onload", strScript)

        Finally
            ValidaFormulario = (strScript = "")

        End Try
    End Function
    Private Function ExtensoesPermitidas() As Boolean
        Try
            objConexao = New clsDB
            If Not objConexao.AbreConexao() Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            Dim objReader As SqlClient.SqlDataReader
            Dim strCampoAux As String
            strCampoAux = IIf(CType(Session("tipoAcesso") = 1, Integer), "AFTP_EXT_ARQU_USER_INTERNO", "AFTP_EXT_ARQU_USER_FORNECEDOR")

            strSQL = "SELECT "
            strSQL += strcampoAux
            strSQL += " FROM TCONFIGURACAO"

            If Not (objConexao.ExecuteReader(strSQL, objReader)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            objReader.Read()

            strExtensoes = objReader(0)

            objReader.Close()

        Catch ex As Exception
            strErro = ex.Message.Trim()
            lblErro.Text = strErro

        Finally
            If Not IsNothing(objConexao) Then objConexao.FechaConexao()

        End Try

    End Function
    Protected Sub UploadArquivo(ByVal Sender As Object, ByVal e As EventArgs)

        If Not ValidaFormulario() Then Exit Sub

        Try
            'Encontra o caminho padrão para upload (/arqtransacoes_fornecedores)
            strCaminhoServidor = ConfigurationSettings.AppSettings("diretorioArquivos")

            Dim strTipoPasta As String
            Dim intTipoEnvio As Integer
            intTipoEnvio = CType(Session("TipoEnvio"), Integer)
            strTipoPasta = IIf(intTipoEnvio = 2, "externas", IIf(intTipoEnvio = 1, "internas", "temporarias"))
            strTipoPasta += "\"

            'Nome do arquivo com caminho completo no cliente
            'ex: c:\diretorio\arquivo.txt
            Dim strArqCaminho As String
            strArqCaminho = txtarquivo.PostedFile.FileName

            'Encontra apenas o nome do arquivo enviado, sem caminho de diretório
            Dim strArq, strArqCript As String

            strArq = System.IO.Path.GetFileName(strArqCaminho)
            strArq = strArq.Replace(" ", "_")

            Dim intContador As Integer

            For intContador = 0 To (envio.dtgDestinos.Items.Count - 1)
                Dim strPasta As String
                If intTipoEnvio = 1 Then
                    strPasta = strCaminhoServidor & _
                                strTipoPasta & _
                                envio.dtgDestinos.Items(intContador).Cells(1).Text.Trim() & "\recebidas\" & _
                                envio.dtgDestinos.Items(intContador).Cells(4).Text.Trim()
                ElseIf intTipoEnvio = 2 Then
                    strPasta = strCaminhoServidor & _
                                strTipoPasta & _
                                envio.dtgDestinos.Items(intContador).Cells(3).Text.Trim() & "\recebidas\" & _
                                envio.dtgDestinos.Items(intContador).Cells(4).Text.Trim()
                ElseIf intTipoEnvio = 3 Then
                    strPasta = strCaminhoServidor & _
                                strTipoPasta & _
                                envio.dtgDestinos.Items(intContador).Cells(4).Text.Trim()
                End If
                IO.Directory.CreateDirectory(strPasta)
                Session("strPasta") += strPasta & "??"
                txtarquivo.PostedFile.SaveAs(strPasta & "\" & strArq)
            Next

            'adiciona arquivo enviado na tabela com lista de arquivos
            PopulaGridArquivos(strArq)

        Catch objErro As System.Exception
            With lblErro
                .Text = "Ocorreu um erro: " & objErro.Message.ToString() & vbCrLf & vbCrLf & objErro.StackTrace
                .Visible = True
            End With

        Finally
            If Not IsNothing(objConexao) Then objConexao.FechaConexao()

        End Try

    End Sub
    Private Sub PopulaGridArquivos(ByVal strArquivo As String)

        Dim objDataSet As DataSet
        Dim objTabela As DataTable
        Dim objLinha As DataRow

        objTabela = New DataTable
        objDataSet = New DataSet

        objTabela.TableName = "arquivos"
        objTabela.Columns.Add("arquivo")

        If Not IsNothing(Session("objDataSetArquivos")) Then
            Dim objDataSetCopia As DataSet
            objDataSetCopia = New DataSet
            objDataSetCopia = Session("objDataSetArquivos").Copy

            Dim intControle As Integer
            For intControle = 0 To (objDataSetCopia.Tables(0).Rows.Count - 1)
                objLinha = objTabela.NewRow()
                objLinha("arquivo") = objDataSetCopia.Tables(0).Rows(intControle).Item("arquivo")
                objTabela.Rows.Add(objLinha)
            Next
        End If

        objLinha = objTabela.NewRow()
        objLinha("arquivo") = strArquivo
        objTabela.Rows.Add(objLinha)

        objDataSet.Tables.Add(objTabela)

        Session("objDataSetArquivos") = objDataSet

        objDataSet = Nothing

        objScript = New clsScript
        objScript.eventoObjetoGenerico(Me, "body", "onload", "window.opener.document.forms[0].submit();self.close();")
    End Sub
End Class
