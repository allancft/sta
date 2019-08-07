Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.IO

Partial Class popupload : Inherits clsLayoutPopup
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
#Region "Objetos Asp.Net"
#End Region
#Region "Variáveis"
    Private strCaminhoServidor, strScript, strSQL As String
    Private objPagina As clsEnvio
    Private objConexao As clsDB
    Private objScript As clsScript
    Private intAcao As Integer
    Private strExtensoes As String
    Private intTipoEnvio As Integer
    Private intContador As Integer
    Private strTipoPasta As String
#End Region
#Region "Classes"
    Dim objUpload As aim.objUpload
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
        Me.Titulo = "Envio de Arquivo"
    End Sub
	Protected Sub UploadArquivo(ByVal Sender As Object, ByVal e As EventArgs)

		Dim objDataSetAux As DataSet
		Dim objDataReader As SqlDataReader
		Dim blnVerifica As Boolean = True
        Dim unidadeMapear As String = ConfigurationSettings.AppSettings("unidadeMapeamento")

        Try

            Dim arq As clsArquivo = New clsArquivo

            'Robson 19/05/2011
            'Mapeia unidade de rede para conseguir criar arquivo
            arq.mapearUnidade(unidadeMapear)

            objConexao = New clsDB
            objUpload = New aim.objUpload
            objUpload.PropDiretorioRaiz = ConfigurationSettings.AppSettings("diretorioArquivos")

            Dim strArquivo As String
            strArquivo = txtarquivo.PostedFile.FileName

            If strArquivo = "" Then
                Throw New System.Exception("Selecione um arquivo a ser enviado")
            End If

            strArquivo = strArquivo.Substring((InStrRev(strArquivo, "\")))

            If Not (objConexao.AbreConexao()) Then
                Throw New System.Exception(objConexao.MensagemErroDB)
            End If

            'Usuario Logado nao tem permissoes de extensoes proprias
            If IsNothing(Session("strExtensoes")) Then
                strSQL = "SELECT " & IIf(Session("tipoAcesso") = 1, _
                  "AFTP_EXT_ARQU_USER_INTERNO", _
                  "AFTP_EXT_ARQU_USER_FORNECEDOR") & _
                  " FROM TCONFIGURACAO"
                If Not (objConexao.ExecuteReader(strSQL, objDataReader)) Then
                    Throw New System.Exception(objConexao.MensagemErroDB)
                End If

                objDataReader.Read()

                If objDataReader.HasRows Then
                    objUpload.PropStrExtPermitidas = objDataReader(0).ToString.Trim
                End If

                objDataReader.Close()
            Else
                objUpload.PropStrExtPermitidas = Session("strExtensoes")
            End If

            If Session("tipoAcesso") = 2 Then    'Valida Nome e Companhia dos Arquivos enviados.

                If strArquivo.Length < 11 Then
                    Throw New System.Exception("O formato do arquivo enviado está incorreto.")
                End If

                Dim strArquivoAux As String

                'Valida Companhias (Ex.: 012)
                strSQL = "SELECT ANUM_COMP FROM TCOMPANHIA"
                If Not (objConexao.ExecuteReader(strSQL, objDataReader)) Then
                    Throw New System.Exception(objConexao.MensagemErroDB())
				End If

				Dim posicaoStr As Integer

				'** Faz o teste de quantos digitos tem no início do nome do arquivo
				If Integer.TryParse(strArquivo.Substring(0, 5), posicaoStr) Then
					posicaoStr = 5
				ElseIf Integer.TryParse(strArquivo.Substring(0, 4), posicaoStr) Then
					posicaoStr = 4
				ElseIf Integer.TryParse(strArquivo.Substring(0, 3), posicaoStr) Then
					posicaoStr = 3
				End If

				strArquivoAux = strArquivo.Substring(0, posicaoStr)

				While objDataReader.Read
					If objDataReader("ANUM_COMP").ToString.Substring(1) = strArquivoAux Then
						blnVerifica = True
						Exit While
					End If
				End While

				objDataReader.Close()

				If Not blnVerifica Then
					Throw New System.Exception("A companhia de destino """ & strArquivoAux & """ informada não existe")
				End If

				blnVerifica = False

				'Valida Aplicativos (Ex.: CRFRETE)
				strSQL = "SELECT ANOM_APLIC FROM TAPLICATIVO"
				If Not (objConexao.ExecuteReader(strSQL, objDataReader)) Then
					Throw New System.Exception(objConexao.MensagemErroDB())
				End If

				strArquivoAux = strArquivo.Substring(posicaoStr)
				strArquivoAux = strArquivoAux.Replace(Mid(strArquivoAux, (InStrRev(strArquivoAux, "."))), "")

				While objDataReader.Read
					If objDataReader("ANOM_APLIC").ToString.Trim = strArquivoAux Then
						blnVerifica = True
						Exit While
					End If
				End While

				objDataReader.Close()

				If Not blnVerifica Then
					Throw New System.Exception("O aplicativo de destino """ & strArquivoAux & """ não existe")
				End If
			End If

				If Not (objConexao.FechaConexao()) Then
					Throw New System.Exception(objConexao.MensagemErroDB)
				End If

				objDataSetAux = New DataSet
				objDataSetAux = CType(Session("objDataSetDestinos"), DataSet).Copy

				For intContador = 0 To (objDataSetAux.Tables(0).Rows.Count - 1)
					objUpload.PropDiretorioArquivo = objDataSetAux.Tables(0).Rows(intContador).Item(2).ToString.Trim

					If Not (objUpload.UploadArquivo(txtarquivo, StrErro)) Then
						Throw New System.Exception(StrErro)
					End If
				Next

				PopulaGridArquivos(strArquivo)

				With lblErro
					.Visible = True
					.CssClass = ""
					.Text = "Arquivo carregado para o envio, para concluir, feche esta janela e clique em seguida no botão ""Confirmar Envio""."
				End With

        Catch ex As Exception
			StrErro = ex.Message.Trim
			If Not blnVerifica Then
				StrErro += vbCrLf & vbCrLf & _
				  Server.HtmlEncode("O Formato do arquivo é <código companhia><nome aplicativo>")
			End If
			With lblErro
				.Visible = True
				.Text = StrErro
			End With

		Finally
			If Not IsNothing(objConexao) Then objConexao = Nothing
			If Not IsNothing(objDataReader) Then objDataReader = Nothing
			If Not IsNothing(objDataSetAux) Then objDataReader = Nothing

			If StrErro = "" Then
				Me.RegisterStartupScript("reload", "<script language=""javascript"">window.opener.document.getElementById('frmSTA').submit();</script>")
			End If
		End Try
	End Sub
	Private Sub PopulaGridArquivos(ByVal strArquivo As String)
		Dim objDataSet As DataSet
		Try
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

		Catch ex As Exception
			With lblErro
				.Visible = True
				.CssClass = "labelErro"
				.Text = ex.Message.Trim
			End With

		Finally
			If Not IsNothing(objDataSet) Then objDataSet = Nothing

		End Try
	End Sub
End Class
