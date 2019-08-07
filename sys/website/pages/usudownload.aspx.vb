Imports System.IO
Imports System.Data
Imports Microsoft.Win32
Imports System.Configuration
Partial Class usudownload
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
#Region "Variáveis"
    Private strDir, strSQL As String
    Private dirInfo As DirectoryInfo
    Private objDB As clsDB
#End Region
    Private Sub EnviaArquivo(ByVal strArquivo As String)
        Try
            Dim objArquivo As FileInfo
            objArquivo = New FileInfo(strDir & strArquivo)

            If Not (objArquivo.Exists()) Then
                Throw New System.Exception("Arquivo não encontrado")
            End If

            If Session("tipoAcesso") = 2 Then 'usuario externo

                objDB = New clsDB
                If Not (objDB.AbreConexao) Then
                    Throw New System.Exception(objDB.MensagemErroDB)
                End If

                strSQL = "UPDATE TREGISTRO_TRANSMISSAO SET ABOL_VALIDACAO_PROTOCOLO = 1" & _
                        " WHERE ANUM_SEQU_REGISTRO = " & cboTransacoes.SelectedItem.Value.Replace("\", "")
                If Not (objDB.ExecutaSQL(strSQL)) Then
                    Throw New System.Exception(objDB.MensagemErroDB)
                End If

                objDB.FechaConexao()

            End If

            Response.Clear()
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + objArquivo.Name)
            Response.ContentType = TipoArquivo(objArquivo.Extension)
            Response.WriteFile(objArquivo.FullName)
            Response.End()
        Catch ex As System.Exception
            lblErro.Text = ex.Message.Trim()

        End Try
    End Sub
    Private Function TipoArquivo(ByVal strExtensao As String) As String
        Dim chaveRegistro As RegistryKey
        chaveRegistro = Registry.ClassesRoot.OpenSubKey(strExtensao)

        Try
            Return chaveRegistro.GetValue("Content Type", "application/octet-stream").ToString()

        Catch objErro As Exception
            Return "application/octet-stream"

        End Try
    End Function
    Private Sub cboTransacoes_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboTransacoes.SelectedIndexChanged
        Try
            If cboTransacoes.SelectedValue = "" Then Exit Sub

            dirInfo = New DirectoryInfo(strDir & cboTransacoes.SelectedValue)
            dtgArquivos.DataSource = dirInfo.GetFiles()
            dtgArquivos.DataBind()

        Catch ex As Exception
            lblErro.Text = ex.Message.Trim()

        End Try
    End Sub
    Private Sub dtgArquivos_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dtgArquivos.ItemCommand
        Dim strArquivoAux As String
        strArquivoAux = (cboTransacoes.SelectedValue.Trim() & "\" & e.Item.Cells(1).Text.Trim())
        EnviaArquivo(strArquivoAux)
    End Sub

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim strTipoPasta As String
            Dim tipoAcesso As Integer
            tipoAcesso = CType(Session("tipoAcesso"), Integer)
            strTipoPasta = IIf(tipoAcesso = 1, "interno", IIf(tipoAcesso = 2, "externo", "temporario"))

            strDir = ConfigurationSettings.AppSettings("diretorioArquivos")
            strDir += strTipoPasta & "\"
            strDir += Session("strDir")
            If CType(Session("TipoAcesso"), Integer) <> 3 Then strDir += "\recebidas"

            dirInfo = New DirectoryInfo(strDir)

            If IsPostBack Then Exit Sub

            'pega todos os diretórios de transmissao do fornecedor
            Dim objDiretorios As DirectoryInfo()
            objDiretorios = dirInfo.GetDirectories()

            If CType(Session("tipoAcesso"), Integer) = 3 Then
                cboTransacoes.Visible = False
                dirInfo = New DirectoryInfo(strDir)
                dtgArquivos.DataSource = dirInfo.GetFiles()
                dtgArquivos.DataBind()
            Else
                'faz loop dos diretorios inserindo opcoes no combo
                Dim objDiretorio As DirectoryInfo
                For Each objDiretorio In objDiretorios
                    Dim objOpcao As Web.UI.WebControls.ListItem
                    objOpcao = New Web.UI.WebControls.ListItem
                    objOpcao.Value = "\" & objDiretorio.Name
                    objOpcao.Text = "Código " & objDiretorio.Name
                    cboTransacoes.Items.Add(objOpcao)
                Next
            End If

            If dtgArquivos.Items.Count = 0 And tipoAcesso = 3 Then
                lblErro.Text = "Não existem arquivos disponíveis para download"
            End If

        Catch ex As IO.DirectoryNotFoundException
            lblErro.Text = "Não existem arquivos disponíveis para download"

        Catch ex As System.Exception
            lblErro.Text = ex.Message.Trim()

        End Try
    End Sub
End Class