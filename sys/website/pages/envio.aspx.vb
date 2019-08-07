Imports System.IO
Imports System.Configuration
Imports System.Web.Security
Imports System.Data
Public Class envio
    Inherits System.Web.UI.Page
#Region "Variáveis e Objetos e Enumeradores"

    'Variaveis
    Private strScript, strSQL, strErro As String
    Private intAcao As Integer

    'Classes
    Private objPagina As clsEnvio
    Private objConexao As clsDB
    Private objScript As clsScript
    Private objEmail As clsEmail

    'Objetos
    Protected WithEvents trComandosMailArquivo As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents trComandosConfirmacao As System.Web.UI.HtmlControls.HtmlTableRow
    Protected WithEvents lblConfirmacao As System.Web.UI.WebControls.Label
    Protected WithEvents lblErro As System.Web.UI.WebControls.Label
    Protected WithEvents btnRemoveDestino As System.Web.UI.WebControls.ImageButton
    Protected WithEvents btnFornecedores As System.Web.UI.WebControls.ImageButton
    Protected WithEvents btnTemporaria As System.Web.UI.WebControls.ImageButton
    Protected WithEvents btnConfirmar As System.Web.UI.WebControls.ImageButton
    Protected WithEvents btnCancelar As System.Web.UI.WebControls.ImageButton
    Protected WithEvents btnInterno As System.Web.UI.WebControls.ImageButton
    Protected btnMail As System.Web.UI.HtmlControls.HtmlInputImage
    Protected tabelaTipoEnvio As System.Web.UI.HtmlControls.HtmlTable
    Protected linhaAvisoArquivos As System.Web.UI.HtmlControls.HtmlTableRow
    Public Shared WithEvents dtgDestinos As System.Web.UI.WebControls.DataGrid
    Public Shared WithEvents dtgArquivos As System.Web.UI.WebControls.DataGrid
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
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim strDir As String
        Dim strTipoPasta As String
        Dim tipoAcesso As Integer
        Dim dirInfo As DirectoryInfo
        tipoAcesso = CType(Session("tipoAcesso"), Integer)
        strTipoPasta = IIf(tipoAcesso = 1, "internas", IIf(tipoAcesso = 2, "externas", "temporarias"))

        strDir = ConfigurationSettings.AppSettings("diretorioArquivos")
        strDir += strTipoPasta & "\"
        strDir += Session("strDir")
        If CType(Session("TipoAcesso"), Integer) <> 3 Then strDir += "\recebidas"
        dirInfo = New DirectoryInfo(strDir)

        If dirInfo.Exists Then
            If DateDiff(DateInterval.Second, dirInfo.LastWriteTime, Now()) > 0 Then
                linhaAvisoArquivos.Visible = True
            Else
                linhaAvisoArquivos.Visible = False
            End If
        Else
            linhaAvisoArquivos.Visible = False
        End If

        TipoEnvio = CType(Session("tipoAcesso"), Integer)
        If TipoEnvio = 1 Then 'interno (logon da rede)
            If Not IsPostBack Then
                Session("TipoEnvio") = 1
                tabelaTipoEnvio.Visible = True
                trComandosConfirmacao.Visible = False
                trComandosMailArquivo.Visible = False
            End If
        ElseIf TipoEnvio = 2 Then 'externo (fornecedores)
            Me.TipoEnvio = EnumEnvio.Externo
            popenvio.TipoEnvio = popenvio.EnumTipoEnvio.Externo
            If Not IsPostBack Then
                AdicionaDestinoExterno()
                Session("TipoEnvio") = 2
                btnMail.Visible = False
                tabelaTipoEnvio.Visible = False
                trComandosConfirmacao.Visible = True
                trComandosMailArquivo.Visible = True
            End If
        ElseIf TipoEnvio = 3 Then 'chave temporaria
            Me.TipoEnvio = EnumEnvio.Interno
            popenvio.TipoEnvio = popenvio.EnumTipoEnvio.Interno
            If Not IsPostBack Then
                AdicionaDestinoChave()
                Session("TipoEnvio") = 3
                btnMail.Visible = False
                tabelaTipoEnvio.Visible = False
                trComandosConfirmacao.Visible = True
                trComandosMailArquivo.Visible = True
            End If
        End If

        If Not IsNothing(Session("objDataSetDestinos")) Then
            With dtgDestinos
                .DataSource = Session("objDataSetDestinos")
                .Visible = (IIf(TipoEnvio = 2, False, True))
                .DataBind()
                btnConfirmar.Enabled = True
            End With
        End If

        If Not IsNothing(Session("objDataSetArquivos")) Then
            With dtgArquivos
                .DataSource = Session("objDataSetArquivos")
                .Visible = True
                .DataBind()
            End With
        End If

        If Request.QueryString.Count > 0 Then
            objScript = New clsScript
            objScript.eventoObjetoGenerico(Me, "body", "onload", "alert('Arquivo(s) enviado(s) com sucesso.');self.location.href='envio.aspx';")
        End If
    End Sub
    Private Sub AdicionaDestinoChave()
        Try
            If IsPostBack Then Exit Sub
            Dim objTabela As DataTable
            Dim objLinha As DataRow
            Dim intEnvio As Integer
            Dim objDataset As DataSet

            objPagina = New clsEnvio
            objConexao = New clsDB

            If Not (objConexao.AbreConexao) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            objPagina.objConexao = objConexao

            If Not (objPagina.IncluirTransmissaoInterna(intEnvio, _
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

            objLinha = objTabela.NewRow()
            objLinha("login") = Session("Remetente")
            objLinha("Nome") = Session("Remetente") & "@itambe.com.br"
            objLinha("diretorio") = Session("strdir")
            objLinha("codigo") = intEnvio

            objTabela.Rows.Add(objLinha)

            objDataset.Tables.Add(objTabela)

            Session("objDataSetDestinos") = objDataset

        Catch ex As System.Exception
            lblErro.Text = ex.Message.Trim

        Finally
            If Not IsNothing(objConexao) Then
                objConexao.FechaConexao()
                objConexao = Nothing
            End If

        End Try
    End Sub
    Private Sub AdicionaDestinoExterno()
        Try
            If IsPostBack Then Exit Sub
            Dim objTabela As DataTable
            Dim objLinha As DataRow
            Dim objDataset As DataSet
            Dim intEnvio As Integer

            objPagina = New clsEnvio
            objConexao = New clsDB

            If Not (objConexao.AbreConexao) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            objPagina.objConexao = objConexao

            If Not (objPagina.IncluiTransmissaoExterna(intEnvio, _
                                                Session("usuario"), _
                                                1, _
                                                strErro)) Then
                Throw New System.Exception(strErro)
            End If

            objTabela = New DataTable
            objDataset = New DataSet

            With objTabela
                .TableName = "destinos"
                .Columns.Add("Login")
                .Columns.Add("Nome")
                .Columns.Add("diretorio")
                .Columns.Add("codigo")
            End With

            objLinha = objTabela.NewRow()
            objLinha("login") = Session("email")
            objLinha("Nome") = Session("email")
            objLinha("diretorio") = Session("strdir")
            objLinha("codigo") = intEnvio

            objTabela.Rows.Add(objLinha)

            objDataset.Tables.Add(objTabela)

            Session("objDataSetDestinos") = objDataset

            With dtgDestinos
                .DataSource = Session("objDataSet")
                .DataBind()
                .Visible = False
            End With

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
        sta.popdestinos.TipoEnvio = popdestinos.EnumTipoEnvio.Interno
        sta.popenvio.TipoEnvio = popenvio.EnumTipoEnvio.Interno
        Me.TipoEnvio = EnumEnvio.Interno
        trComandosConfirmacao.Visible = True
        trComandosMailArquivo.Visible = True
        Session("TipoEnvio") = Convert.ToInt32(EnumEnvio.Interno)
        If Not IsNothing(Session("objDataSetDestinos")) Then
            Session("objDataSetDestinos").Clear()
        End If
    End Sub
    Private Sub btnFornecedores_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnFornecedores.Click
        sta.popdestinos.TipoEnvio = popdestinos.EnumTipoEnvio.Externo
        sta.popenvio.TipoEnvio = popenvio.EnumTipoEnvio.Externo
        Me.TipoEnvio = EnumEnvio.Externo
        trComandosConfirmacao.Visible = True
        trComandosMailArquivo.Visible = True
        Session("TipoEnvio") = Convert.ToInt32(EnumEnvio.Externo)
        If Not IsNothing(Session("objDataSetDestinos")) Then
            Session("objDataSetDestinos").Clear()
        End If
    End Sub
    Private Sub btnTemporaria_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnTemporaria.Click
        sta.popdestinos.TipoEnvio = popdestinos.EnumTipoEnvio.Temporario
        sta.popenvio.TipoEnvio = popenvio.EnumTipoEnvio.Temporario
        Me.TipoEnvio = EnumEnvio.Temporario
        trComandosConfirmacao.Visible = True
        trComandosMailArquivo.Visible = True
        Session("TipoEnvio") = Convert.ToInt32(EnumEnvio.Temporario)
        If Not IsNothing(Session("objDataSetDestinos")) Then
            Session("objDataSetDestinos").Clear()
        End If
    End Sub
    Private Sub ValidaFormulario()
        Try
            objScript = New clsScript

            strErro = ""

            If dtgDestinos.Items.Count = 0 Then
                strErro += "Selecione os destinatários.\n"
            End If
            If TipoEnvio <> EnumEnvio.Temporario And dtgArquivos.Items.Count = 0 Then
                strErro += "Selecione os arquivos que deseja enviar.\n"
            End If

            If (strErro <> "") Then
                Throw New System.Exception("alert('" & strErro & "';history.back();")
            End If

        Catch ex As Exception
            objScript.eventoObjetoGenerico(Me, "body", "onload", strErro)

        Finally
            objScript = Nothing

        End Try

    End Sub
    Private Sub btnConfirmar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnConfirmar.Click

        Try

            '##################################################################
            'INCLUIR ROTINA PARA ENVIO DE CHAVE VAZIA (SEM ENVIO DE ARQUIVOS) #
            '##################################################################

            'Call ValidaFormulario()

            objPagina = New clsEnvio
            objConexao = New clsDB

            If Not (objConexao.AbreConexao()) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            objPagina.objConexao = objConexao

            Dim intContador, intCodTransmissao As Integer

            Dim strCaminhoServidor As String
            strCaminhoServidor = ConfigurationSettings.AppSettings("diretorioArquivos")

            Dim strTipoPasta As String
            strTipoPasta = IIf(TipoEnvio = EnumEnvio.Externo, "externas", IIf(TipoEnvio = EnumEnvio.Interno, "internas", "temporarias")) & "\"

            Dim intContadorAux As Integer
            Dim strArquivos, strDestinos, strDestinoCod As String

            'Inclui Registro de Envio de Arquivo para o Registro de Transmissao
            If envio.dtgArquivos.Items.Count > 0 Then
                For intContador = 0 To (envio.dtgArquivos.Items.Count - 1)
                    For intContadorAux = 0 To (envio.dtgDestinos.Items.Count - 1)
                        Dim strArq As String
                        strArq = dtgArquivos.Items(intContador).Cells(0).Text.Trim
                        strArquivos += strArq & vbCrLf 'lista de arquivos para e-mail

                        Dim strCodEnvio As String
                        strCodEnvio = dtgDestinos.Items(intContadorAux).Cells(4).Text.Trim()
                        strDestinos += dtgDestinos.Items(intContadorAux).Cells(1).Text.Trim() & ";"
                        strDestinoCod = dtgDestinos.Items(intContadorAux).Cells(3).Text.Trim()
                        If CType(Session("TipoEnvio"), EnumEnvio) = EnumEnvio.Temporario Then
                            If strCodEnvio <> "" Then
                                If Not (EnvioChave(strArq, strCodEnvio, strDestinos, strErro)) Then
                                    Throw New System.Exception(strErro)
                                End If
                            End If
                        Else
                            If Not (IncluiRegArquivo(strArq, strCodEnvio, strDestinoCod, strErro)) Then
                                Throw New System.Exception(strErro)
                            End If
                        End If
                    Next
                Next
            Else
                If CType(Session("TipoEnvio"), EnumEnvio) = EnumEnvio.Temporario Then
                    If Not (EnvioChaveVazia(strErro)) Then
                        Throw New System.Exception(strErro)
                    End If
                End If
            End If

            objEmail = New clsEmail

            Dim strRemetente As String

            If CType(Session("TipoEnvio"), EnumEnvio) = EnumEnvio.Interno Then
                strRemetente = Session("usuario") & "@" & ConfigurationSettings.AppSettings("dominioCorreio")
            End If

            If CType(Session("TipoEnvio"), EnumEnvio) <> EnumEnvio.Temporario Then
                If Not (objEmail.EnviaEmail(CType(Session("TipoEnvio"), clsEmail.enumTipo), _
                                            strErro, _
                                            strDestinos, _
                                            strArquivos, _
                                            Session("codTransmissao"), _
                                            strRemetente)) Then
                    Throw New System.Exception(strErro)
                End If
            End If
        Catch When IsNothing(Session("TipoEnvio"))
            strErro = "Sua sessão expirou. Feche seu browser e acesse o sistema novamente."
            objScript = New clsScript
            objScript.eventoObjetoGenerico(Me, "body", "onload", _
                        "alert('" & strErro & "');history.back();")
            Exit Try

        Catch objErro As System.Exception
            strErro = objScript.limpaStringMensagem(objErro.Message.Trim())
            objScript = New clsScript
            objScript.eventoObjetoGenerico(Me, "body", "onload", _
                        "alert('" & strErro & "');history.back();")
            Exit Try

        Finally
            If strErro = "" Then EnvioOK()

        End Try

    End Sub
    Private Function IncluiRegArquivo(ByVal strArq As String, _
                                ByVal strCodEnvio As String, _
                                ByVal strDestinoCod As String, _
                                ByRef strErro As String) As Boolean
        Try
            Dim TipoAcesso As EnumEnvio
            TipoAcesso = CType(Session("tipoAcesso"), EnumEnvio)

            If TipoAcesso = EnumEnvio.Externo Then
                If TipoEnvio = EnumEnvio.Interno Then 'interno enviando para fornecedor
                    If Not (objConexao.AbreConexao) Then
                        Throw New System.Exception(objConexao.MensagemErroDB)
                    End If

                    Dim objReader As SqlClient.SqlDataReader

                    strSQL = "SELECT ANUM_SEQU_FORNECEDOR WHERE ACOD_FORNECEDOR_BAAN = '" & strDestinoCod.Trim() & "'"

                    If Not (objConexao.ExecuteReader(strSQL)) Then
                        Throw New System.Exception(objConexao.MensagemErroDB)
                    End If

                    objReader.Read()

                    If Not (objPagina.IncluiRegArquivoExterno(CType(objReader("ANUM_SEQU_FORNECEDOR"), Integer), _
                                                strCodEnvio, _
                                                strArq, _
                                                strErro)) Then
                        Throw New System.Exception(strErro)
                    End If

                    objReader.Close()
                    objConexao.FechaConexao()

                Else 'interno enviando arquivos para baan
                    If Not (objPagina.IncluiRegArquivoExterno(Session("usuario"), _
                                                strCodEnvio, _
                                                strArq, _
                                                strErro)) Then
                        Throw New System.Exception(strErro)
                    End If
                End If
            ElseIf TipoAcesso = EnumEnvio.Interno Then
                If CType(Session("TipoEnvio"), EnumEnvio) <> EnumEnvio.Temporario Then
                    If Not (objPagina.IncluiRegArquivoInterno(strCodEnvio, _
                                                strArq, _
                                                strErro)) Then
                        Throw New System.Exception(strErro)
                    End If
                End If
            End If
        Catch ex As Exception
            strErro = ex.Message.ToString()
            With lblErro
                .Text = "Ocorreu um erro: " & strErro
                .Visible = True
            End With

        End Try

        Return (strErro = "")

    End Function
    Private Function EnvioChaveVazia(ByRef strErro As String) As Boolean
        Try

            objEmail = New clsEmail
            If Not objEmail.EnviaEmail(5, strErro) Then
                Throw New System.Exception(strErro)
            End If

        Catch When IsNothing(Session("Usuario"))
            strErro = "Sua sessão expirou. Feche seu navegador e acesse novamente."

        Catch objErro As System.Exception
            strErro = objErro.Message.ToString().Trim()
        Finally
            EnvioChaveVazia = (strErro = "")

        End Try
    End Function
    Private Function EnvioChave(ByVal strArq As String, _
                            ByVal strCodEnvio As String, _
                            ByVal strDestinos As String, _
                            ByRef strErro As String) As Boolean
        Try

            objEmail = New clsEmail
            If Not objEmail.EnviaEmail(3, _
                                        strErro, _
                                        strDestinos, _
                                        strArq, _
                                        strCodEnvio) Then
                Throw New System.Exception(strErro)
            End If

        Catch When IsNothing(Session("Usuario"))
            strErro = "Sua sessão expirou. Feche seu navegador e acesse novamente."

        Catch objErro As System.Exception
            strErro = objErro.Message.ToString().Trim()
        Finally
            EnvioChave = (strErro = "")

        End Try
    End Function
    Private Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnCancelar.Click

        Try
            If IsNothing(Session("strPasta")) Then Exit Try

            Dim arrPastasEnviadas As Array
            arrPastasEnviadas = Session("strPasta").ToString.Split("??")
            Dim intControle As Integer

            For intControle = 0 To arrPastasEnviadas.GetUpperBound(0)

                Dim strPasta = arrPastasEnviadas(intControle).ToString.Trim()

                If strPasta <> "" Then

                    Dim dirInfo As DirectoryInfo
                    dirInfo = New DirectoryInfo(strPasta)

                    Dim objArquivos As FileInfo()
                    objArquivos = dirInfo.GetFiles()

                    Dim objArquivo As FileInfo

                    For Each objArquivo In objArquivos
                        objArquivo.Delete()
                    Next

                    IO.Directory.Delete(strPasta)

                End If
            Next

            Session("strPasta") = Nothing

        Catch When IsNothing(Session("Usuario"))
            lblErro.Text = "Sua sessão expirou. Feche seu navegador e acesse novamente."

        Catch ex As System.Exception
            lblErro.Text = ex.Message
        End Try

        Session("objDataSetDestinos") = Nothing
        Session("objDataSetArquivos") = Nothing
        Session("codTransmissao") = Nothing
        Response.Redirect("envio.aspx", True)

    End Sub
    Public Sub EnvioOK()
        Session("objDataSetDestinos") = Nothing
        Session("objDataSetArquivos") = Nothing
        Session("codTransmissao") = Nothing
        Response.Redirect("envio.aspx?envio=1", True)
    End Sub
    Sub RemoveDestino(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)
        Dim objDataSetCopia As DataSet
        objDataSetCopia = New DataSet
        objDataSetCopia = Session("objDataSetDestinos").Copy

        Session("objDataSetDestinos") = Nothing

        objDataSetCopia.Tables(0).Rows(e.Item.ItemIndex).Delete()

        Session("objDataSetDestinos") = objDataSetCopia
    End Sub
    Sub RemoveArquivo(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs)

        Dim objDataSetCopia As DataSet
        objDataSetCopia = New DataSet
        objDataSetCopia = Session("objDataSetArquivos").Copy

        Session("objDataSetArquivos") = Nothing

        objDataSetCopia.Tables(0).Rows(e.Item.ItemIndex).Delete()

        Session("objDataSetArquivos") = objDataSetCopia
    End Sub
End Class
