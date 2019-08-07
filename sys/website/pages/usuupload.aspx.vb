Imports System.IO
Imports System.Data
Imports System.Web.Security
Imports System.Configuration
Partial Class usuupload : Inherits clsLayoutBase
#Region "Variáveis"
    Private strDirRaiz As String = ConfigurationSettings.AppSettings("diretorioArquivos")
    Private strScript As String
    Private strSQL As String
    Private intAcao As Integer
    Private intTipoAcesso As Integer
#End Region
#Region "Classes"
    Private objPagina As clsEnvio
    Private objConexao As clsDB
    Private objScript As clsScript
    Private objEmail As clsEmail
#End Region
#Region "Objetos Asp.Net"
    Protected lblConfirmacao As System.Web.UI.WebControls.Label
    Private Enum EnumEnvio
        Interno = 1
        Externo = 2
        Temporario = 3
    End Enum
    Private Property TipoEnvio() As EnumEnvio
        Get
            Return intAcao
        End Get
        Set(ByVal Value As EnumEnvio)
            intAcao = Value
        End Set
    End Property
#End Region
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
    Private Sub incluiEnvioBaan()
        Try
            Dim strCodEnvio As String
            Dim strDir As String = Session("strDir")

            objPagina = New clsEnvio
            objConexao = New clsDB

            If Not (objConexao.AbreConexao) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            objPagina.objConexao = objConexao

            If Not (objPagina.IncluiTransmissaoExterna(strCodEnvio, _
                                                        Session("codSeq"), _
                                                        Session("tipoAcesso"), _
                                                        0, _
                                                        StrErro)) Then
                Throw New System.Exception(strErro)
            End If

            If Not (objConexao.FechaConexao) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

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

            objLinha = objTabela.NewRow()
            objLinha("Login") = ""
            objLinha("Nome") = ""
            objLinha("diretorio") = "externo\" & strDir & "\baan\" & strCodEnvio
            objLinha("codigo") = strCodEnvio
            objTabela.Rows.Add(objLinha)

            objDataSet.Tables.Add(objTabela)

            Session("objDataSetDestinos") = objDataSet
            objDataSet = Nothing

        Catch ex As System.Exception
            strErro = ex.Message.ToString()
            With lblErro
                .Text = "Ocorreu um erro: " & strErro
                .Visible = True
            End With

        End Try
    End Sub
    Private Sub incluiEnvioChave()
        Try
            Dim objTabela As DataTable
            Dim objLinha As DataRow
            Dim strCodigo As String
            Dim objDataset As DataSet

            objPagina = New clsEnvio
            objConexao = New clsDB

            If Not (objConexao.AbreConexao) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            objPagina.objConexao = objConexao

            If Not (objPagina.IncluirTransmissaoInterna(strCodigo, _
                                                "Chave Temporaria", _
                                                Session("Remetente"), _
                                                strErro)) Then
                Throw New System.Exception(strErro)
            End If

            Dim objAD As clsADSI
            Dim objDataSetAux As DataSet
            objAD = New clsADSI

            objTabela = New DataTable
            objDataset = New DataSet

            With objTabela
                .TableName = "destinos"
                .Columns.Add("Login")
                .Columns.Add("Nome")
                .Columns.Add("diretorio")
                .Columns.Add("codigo")
            End With

            Dim strRemetente As String = Session("Remetente")
            strRemetente.Substring(0, InStr(strRemetente, "@"))

            objLinha = objTabela.NewRow()
            objLinha("login") = strRemetente
            objLinha("Nome") = strRemetente
            objLinha("diretorio") = "interno\" & Session("Remetente") & "\recebidas\" & strCodigo
            objLinha("codigo") = strCodigo.Trim

            objTabela.Rows.Add(objLinha)

            objDataset.Tables.Add(objTabela)

            Session("objDataSetDestinos") = objDataset

            With dtgDestinos
                .DataSource = Session("objDataSetDestinos")
                .DataBind()
            End With
            btnConfirmar.Enabled = True

        Catch ex As System.Exception
            lblErro.Text = ex.Message.Trim

        Finally
            If Not IsNothing(objConexao) Then
                objConexao.FechaConexao()
                objConexao = Nothing
            End If
        End Try
    End Sub
    Private Sub btnInterno_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnInterno.Click
        montaTelaBotao(EnumEnvio.Interno)
    End Sub
    Private Sub btnFornecedores_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnFornecedores.Click
        montaTelaBotao(EnumEnvio.Externo)
    End Sub
    Private Sub btnTemporaria_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnTemporaria.Click
        montaTelaBotao(EnumEnvio.Temporario)
    End Sub
    Private Sub montaTelaBotao(ByVal tipoEnvio As EnumEnvio)
        'tipoEnvio = EnumEnvio.Externo

        Select Case tipoEnvio
            Case EnumEnvio.Interno
                htitulo.InnerText = "Envio de Arquivos - Interno"
                Session("TipoEnvio") = 1
            Case EnumEnvio.Externo
                htitulo.InnerText = "Envio de Arquivos - Usuários"
                Session("TipoEnvio") = 2
            Case EnumEnvio.Temporario
                htitulo.InnerText = "Envio de Arquivos - Chave Temporária"
                Session("TipoEnvio") = 3
                btnConfirmar.Enabled = (intTipoAcesso = 1)
                dtgDestinos.Visible = (intTipoAcesso = 1)
        End Select
        btnMail.Attributes.Add("onclick", "javascript:abrirJanela('popdestinos.aspx?tpenvio=" & Session("TipoEnvio").ToString.Trim & "', 420, 380, false);")
        btnArquiv.Attributes.Add("onclick", "javascript:abrirJanela('popupload.aspx?tpenvio=" & Session("TipoEnvio").ToString.Trim & "', 350, 300, false);")
        If intTipoAcesso = 1 Then btnMail.Visible = True
        trComandosConfirmacao.Visible = True
        trComandosMailArquivo.Visible = True
    End Sub
    Private Function validaFormulario() As Boolean
        Try
            If dtgDestinos.Items.Count = 0 Then
                strErro += "Selecione os destinatários.\n"
            End If
            If Session("TipoEnvio") < 3 And dtgArquivos.Items.Count = 0 Then
                strErro += "Selecione os arquivos que deseja enviar.\n"
            End If
            If (strErro <> "") Then
                Throw New System.Exception(strErro)
            End If

        Catch ex As Exception
            strErro = ex.Message.Trim
            Me.RegisterStartupScript("scriptErro", "<script language=""javascript"">alert('" & strErro & "');history.back();</script>")

        End Try

        Return (strErro = "")
    End Function
    Private Sub btnConfirmar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnConfirmar.Click
        If Not validaFormulario() Then Exit Sub
        Try
            Dim objEmail As clsEmail

            objPagina = New clsEnvio
            objConexao = New clsDB

            'Faz o Loop do DataGrid de Destinos pegando os arquivos
            'enviados que estão na coluna oculta no formato: <usuario>\<cod_transacao>
            'Em seguida, faz Loop do DataGrid de Arquivos enviados para concatenar
            'o nome dos arquivos com o diretório do usuário de destino
            Dim intContAuxDest, intContAuxArq As Integer
            For intContAuxDest = 0 To (dtgDestinos.Items.Count - 1)
                objEmail = New clsEmail
                Dim strArquivoAux As String
                Dim strEnviados As String
                Dim strVirus As String

                For intContAuxArq = 0 To (dtgArquivos.Items.Count - 1)
                    If intContAuxDest = 0 Then 'So concatena arquivos enviados e verifica virus no primeiro laco do loop
                        strArquivoAux = dtgDestinos.Items(intContAuxDest).Cells(3).Text.Trim & _
                                        "\" & _
                                        dtgArquivos.Items(intContAuxArq).Cells(1).Text.Trim()
                        strEnviados += dtgArquivos.Items(intContAuxArq).Cells(1).Text.Trim & vbCrLf
                        'Se o arquivo não existe mais no servidor, provavelmente foi
                        'excluído pelo AntiVirus
                        If Not (File.Exists(strDirRaiz & strArquivoAux)) Then
                            strVirus += dtgArquivos.Items(intContAuxArq).Cells(1).Text.Trim & vbCrLf
                        End If
                    End If

                    Select Case intTipoAcesso
                        Case 1
                            Select Case CInt(Session("TipoEnvio"))
                                Case 1
                                    incluiRegIntInt(dtgArquivos.Items(intContAuxArq).Cells(1).Text.Trim, _
                                                    dtgDestinos.Items(intContAuxDest).Cells(4).Text, _
                                                    strErro)
                                Case 2
                                    incluiRegIntExt(dtgDestinos.Items(intContAuxDest).Cells(4).Text, _
                                                    dtgArquivos.Items(intContAuxArq).Cells(1).Text.Trim, _
                                                    dtgDestinos.Items(intContAuxDest).Cells(1).Text.Trim, _
                                                    strErro)
                            End Select

                        Case 2
                            incluiRegArquivo(Session("codSeq"), _
                                            dtgDestinos.Items(intContAuxDest).Cells(4).Text, _
                                            dtgArquivos.Items(intContAuxArq).Cells(1).Text.Trim, _
                                            strerro)
                        Case 3
                            incluiRegArquivo(dtgDestinos.Items(intContAuxDest).Cells(4).Text, _
                                            dtgArquivos.Items(intContAuxArq).Cells(1).Text.Trim, _
                                            strerro)
                    End Select
                    If strErro <> "" Then
                        Throw New System.Exception(strErro)
                    End If
                Next

                If dtgDestinos.Items(intContAuxDest).Cells(1).Text.Trim <> "&nbsp;" Then

                    If Not (objEmail.EnviaEmail(IIf(dtgDestinos.Items(intContAuxDest).Cells(1).Text.IndexOf("@") = -1, _
                                                    dtgDestinos.Items(intContAuxDest).Cells(1).Text & "@itambe.com.br", _
                                                    dtgDestinos.Items(intContAuxDest).Cells(1).Text), _
                                                IIf(intTipoAcesso < 3, Session("usuario"), "sta@itambe.com.br"), _
                                                dtgDestinos.Items(intContAuxDest).Cells(4).Text.Trim, _
                                                strEnviados, _
                                                Session("tipoEnvio"), _
                                                Session("tipoAcesso"), _
                                                strErro, _
                                                strVirus, _
                                                txtMensagem.Text)) Then
                        Throw New System.Exception(strErro)
                    End If
                End If
            Next

        Catch ex As System.Exception
            strErro = ex.Message.ToString()
            With lblErro
                .Text = "Ocorreu um erro: " & strErro
                .Visible = True
            End With

        Finally
            If strErro = "" Then envioOK()

        End Try
    End Sub
    'Grava envio de Arquivos de Fornecedor Externo para o BAAN
    Private Overloads Function incluiRegArquivo(ByVal intSeqFornecedor As Integer, _
                                                ByVal intSeqRegistro As Integer, _
                                                ByVal strNomeArquivo As String, _
                                                ByRef strErro As String) As Boolean
        Dim intSeqArquivo As Integer
        Dim objReader As SqlClient.SqlDataReader
        objConexao = New clsDB

        Try
            If Not (objConexao.AbreConexao) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            objPagina.objConexao = objConexao

            strSQL = "SELECT ISNULL(MAX(ANUM_SEQU_REGISTRO), 0) + 1 AS CODIGO" & _
                    " FROM TARQUIVO WHERE ANUM_SEQU_REGISTRO = " & intSeqRegistro & _
                    " AND ANUM_SEQU_FORNECEDOR = " & intSeqFornecedor
            If Not (objConexao.ExecuteReader(strSQL, objReader)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            objReader.Read()

            intSeqArquivo = Convert.ToInt32(objReader("CODIGO"))

            objReader.Close()

            strSQL = "INSERT INTO TARQUIVO (ANUM_SEQU_FORNECEDOR, ANUM_SEQU_REGISTRO, " & _
                    " ANUM_SEQU_ARQUIVO, ANOM_ARQUIVO) VALUES (" & _
                    intSeqFornecedor & ", " & _
                    intSeqRegistro & ", " & _
                    intSeqArquivo & ", " & _
                    "'" & strNomeArquivo.Trim & "')"
            If Not (objConexao.ExecutaSQL(strSQL)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            If Not (objConexao.FechaConexao) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

        Catch ex As Exception
            strErro = ex.Message.ToString()

        Finally
            If Not (IsNothing(objReader)) Then objReader = Nothing
            If Not (IsNothing(objConexao)) Then objConexao = Nothing

        End Try
        Return (strErro = "")
    End Function
    'Grava envio de Arquivos de Chave Temporária para Usuário Interno
    Private Overloads Function incluiRegArquivo(ByVal strChaveRegistro As String, _
                                                ByVal strNomeArquivo As String, _
                                                ByRef strErro As String) As Boolean
        Dim intSeqArquivo As Integer
        Dim objReader As SqlClient.SqlDataReader
        objConexao = New clsDB

        Try
            If Not (objConexao.AbreConexao) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            objPagina.objConexao = objConexao

            strSQL = "SELECT ISNULL(MAX(ANUM_SEQU_ARQUIVO_INTERNO), 0) + 1 AS CODIGO" & _
                    " FROM TARQUIVO_INTERNO"
            If Not (objConexao.ExecuteReader(strSQL, objReader)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            objReader.Read()

            intSeqArquivo = Convert.ToInt32(objReader("CODIGO"))

            objReader.Close()

            strSQL = "INSERT INTO TARQUIVO_INTERNO (ANUM_SEQU_ARQUIVO_INTERNO," & _
                    " ANUM_SEQU_REGISTRO_INTERNO, ANOM_ARQUIVO) VALUES (" & _
                    intSeqArquivo & ", " & _
                    "'" & strChaveRegistro.Trim & "', " & _
                    "'" & strNomeArquivo.Trim & "')"
            If Not (objConexao.ExecutaSQL(strSQL)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            If Not (objConexao.FechaConexao) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

        Catch ex As Exception
            strErro = ex.Message.ToString()

        Finally
            If Not (IsNothing(objReader)) Then objReader = Nothing
            If Not (IsNothing(objConexao)) Then objConexao = Nothing

        End Try
        Return (strErro = "")
    End Function
    'Envios de usuários Internos para outro usuário Interno
    Private Overloads Function incluiRegIntInt(ByVal strNomeArquivo As String, _
                                                ByVal intSeqRegistro As Integer, _
                                                ByRef strErro As String) As Boolean
        Dim intSeqArquivo As Integer
        Dim objReader As SqlClient.SqlDataReader
        objConexao = New clsDB

        Try
            If Not (objConexao.AbreConexao) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            objPagina.objConexao = objConexao

            strSQL = "SELECT ISNULL(MAX(ANUM_SEQU_ARQUIVO_INTERNO), 0) + 1 AS CODIGO" & _
                    " FROM TARQUIVO_INTERNO"
            If Not (objConexao.ExecuteReader(strSQL, objReader)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            objReader.Read()

            intSeqArquivo = Convert.ToInt32(objReader("CODIGO"))

            objReader.Close()

            strSQL = "INSERT INTO TARQUIVO_INTERNO (ANUM_SEQU_ARQUIVO_INTERNO, " & _
                    "ANUM_SEQU_REGISTRO_INTERNO, ANOM_ARQUIVO) VALUES(" & _
                    intSeqRegistro & ", " & _
                    intSeqArquivo & ", '" & _
                    strNomeArquivo.Trim & "')"
            If Not (objConexao.ExecutaSQL(strSQL)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            If Not (objConexao.FechaConexao) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

        Catch ex As Exception
            strErro = ex.Message.ToString() & vbCrLf & ex.StackTrace
            With lblErro
                .Text = "Ocorreu um erro: " & strErro
                .Visible = True
            End With

        Finally
            objConexao.FechaConexao()

        End Try
        Return (strErro = "")
    End Function
    'Envios de usuários Internos para usuário Externo
    Private Overloads Function incluiRegIntExt(ByVal intSeqRegistro As Integer, _
                                                ByVal strNomeArquivo As String, _
                                                ByVal strEmailFornecedor As String, _
                                                ByRef strErro As String) As Boolean
        Dim intSeqArquivo As Integer
        Dim intSeqFornecedor As Integer
        Dim objReader As SqlClient.SqlDataReader
        objConexao = New clsDB

        Try
            If Not (objConexao.AbreConexao) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            objPagina.objConexao = objConexao

            'busca o codigo sequencial do fornecedor a partir do email
            strSQL = "SELECT ANUM_SEQU_FORNECEDOR" & _
                    " FROM TFORNECEDOR WHERE ADES_EMAIL_FORNECEDOR = '" & strEmailFornecedor.Trim & "'"
            If Not (objConexao.ExecuteReader(strSQL, objReader)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            objReader.Read()

            If objReader.HasRows Then
                intSeqFornecedor = Convert.ToInt32(objReader("ANUM_SEQU_FORNECEDOR"))
            Else
                Throw New System.Exception("Sequencial do Fornecedor não encontrado")
            End If

            objReader.Close()

            strSQL = "SELECT ISNULL(MAX(ANUM_SEQU_REGISTRO), 0) + 1 AS CODIGO" & _
                    " FROM TARQUIVO WHERE ANUM_SEQU_REGISTRO = " & intSeqRegistro & _
                    " AND ANUM_SEQU_FORNECEDOR = " & intSeqFornecedor
            If Not (objConexao.ExecuteReader(strSQL, objReader)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            objReader.Read()

            intSeqArquivo = Convert.ToInt32(objReader("CODIGO"))

            objReader.Close()

            strSQL = "INSERT INTO TARQUIVO (ANUM_SEQU_FORNECEDOR, ANUM_SEQU_REGISTRO, " & _
                    " ANUM_SEQU_ARQUIVO, ANOM_ARQUIVO) VALUES (" & _
                    intSeqFornecedor & ", " & _
                    intSeqRegistro & ", " & _
                    intSeqArquivo & ", " & _
                    "'" & strNomeArquivo.Trim & "')"
            If Not (objConexao.ExecutaSQL(strSQL)) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            If Not (objConexao.FechaConexao) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

        Catch ex As Exception
            strErro = ex.Message.ToString() & vbCrLf & ex.StackTrace
            With lblErro
                .Text = "Ocorreu um erro: " & strErro
                .Visible = True
            End With

        Finally
            objConexao.FechaConexao()

        End Try
        Return (strErro = "")
    End Function
    Private Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCancelar.Click
        Dim intCont As Integer
        If Not IsNothing(Session("objDataSetArquivos")) Then
            Dim objDataSetCopiaDest As DataSet
            objDataSetCopiaDest = New DataSet
            objDataSetCopiaDest = Session("objDataSetDestinos").Copy

            Try
                For intCont = 0 To (objDataSetCopiaDest.Tables(0).Rows.Count - 1)
                    IO.Directory.Delete(ConfigurationSettings.AppSettings("diretorioArquivos") & _
                                        objDataSetCopiaDest.Tables(0).Rows(intCont).Item("diretorio").ToString.Trim, True)
                Next
            Catch : End Try

            objDataSetCopiaDest = Nothing
        End If
        Session("objDataSetDestinos") = Nothing
        Session("objDataSetArquivos") = Nothing
        Response.Redirect("usuupload.aspx", True)
    End Sub
    Private Sub envioOK()
        Session("objDataSetDestinos") = Nothing
        Session("objDataSetArquivos") = Nothing
        Me.RegisterStartupScript("enviook", "<script language=""javascript"">alert('Arquivo(s) enviado(s) com sucesso.');self.location.href='usuupload.aspx';</script>")
    End Sub

    'Robson 19/05/2011
    'Corrigido método para remover o item, indiferente do arquivo. 
    Private Sub dtgDestinos_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgDestinos.DeleteCommand
        Dim objDataSetCopiaDest As DataSet
        objDataSetCopiaDest = New DataSet
        objDataSetCopiaDest = Session("objDataSetDestinos").Copy

        If Not IsNothing(Session("objDataSetArquivos")) Then
            IO.Directory.Delete(ConfigurationSettings.AppSettings("diretorioArquivos") & _
                                objDataSetCopiaDest.Tables(0).Rows(e.Item.ItemIndex).Item("diretorio").ToString.Trim, True)
        End If

        objDataSetCopiaDest.Tables(0).Rows(e.Item.ItemIndex).Delete()

        With dtgDestinos
            .DataSource = objDataSetCopiaDest
            .Visible = (IIf(TipoEnvio = 2, False, True))
            .DataBind()
        End With

        Session("objDataSetDestinos") = objDataSetCopiaDest
        objDataSetCopiaDest = Nothing
    End Sub
    Private Sub dtgArquivos_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgArquivos.DeleteCommand

        Dim intControleDest As Integer

        If Not IsNothing(Session("objDataSetArquivos")) Then
            Dim objDataSetCopia As DataSet
            objDataSetCopia = New DataSet
            objDataSetCopia = Session("objDataSetArquivos").Copy

            Dim objDataSetCopiaDest As DataSet
            objDataSetCopiaDest = New DataSet
            objDataSetCopiaDest = Session("objDataSetDestinos").Copy

            For intControleDest = 0 To (objDataSetCopiaDest.Tables(0).Rows.Count - 1)
                IO.File.Delete(ConfigurationSettings.AppSettings("diretorioArquivos") & _
                                objDataSetCopiaDest.Tables(0).Rows(intControleDest).Item("diretorio").ToString.Trim & _
                                "\" & objDataSetCopia.Tables(0).Rows(e.Item.ItemIndex).Item("arquivo").ToString.Trim)
            Next

            objDataSetCopia.Tables(0).Rows(e.Item.ItemIndex).Delete()

            With dtgArquivos
                .DataSource = objDataSetCopia
                .Visible = True
                .DataBind()
            End With

            Session("objDataSetArquivos") = objDataSetCopia
            objDataSetCopia = Nothing

        End If
    End Sub
    Private Sub btnArquiv_ServerClick(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnArquiv.ServerClick
        Select Case intTipoAcesso
            Case 2 : incluiEnvioBaan()
            Case 3 : incluiEnvioChave()
        End Select
    End Sub
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        intTipoAcesso = CInt(Session("tipoAcesso"))

        'Robson 19-05-2010
        'IMPORTANTE: em caso de gerar versão para acesso EXTERNO (SERVIDOR EUROPA - Internet) atribuir "2" no intTipoAcesso
        'Caso de gerar versão para acesso INTERNO (INTRANET, Acesso exclusivo de dentro da Itambé) atribuir "1" no intTipoAcesso Abaixo
        'intTipoAcesso = 2
        'Ou melhor, trabalhe com o if (If strIpServidor.IndexOf(strIpInicialServidorLan) = 0 Then) na página login.aspx.vb, pois ele
        'vai mudar não apenas o intTipoAcesso, mas todos os locais necessários.
        btnMail.Visible = (intTipoAcesso = 1)

        If IsPostBack Then
            If Not IsNothing(Session("objDataSetDestinos")) Then
                With dtgDestinos
                    .DataSource = CType(Session("objDataSetDestinos"), DataSet)
                    .Visible = (intTipoAcesso = 1)
                    .DataBind()
                End With
                If Session("TipoEnvio") = 3 Then
                    DivMensagem.Visible = True
                End If
            End If
            If Not IsNothing(Session("objDataSetArquivos")) Then
                With dtgArquivos
                    .DataSource = CType(Session("objDataSetArquivos"), DataSet)
                    .DataBind()
                End With
                btnConfirmar.Enabled = True
                If Session("TipoEnvio") = 3 Then
                    DivMensagem.Visible = True
                End If
            End If
            Exit Sub
        Else
            DivMensagem.Visible = False
        End If

        linhaTipoEnvio.Visible = (intTipoAcesso = 3)

        Select Case intTipoAcesso
            Case 1 : montaTelaBotao(EnumEnvio.Interno)
            Case 2 : montaTelaBotao(EnumEnvio.Externo)
            Case 3 : montaTelaBotao(EnumEnvio.Temporario)
        End Select
    End Sub
End Class
