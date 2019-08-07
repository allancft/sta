Imports System.IO
Imports System.Data
Imports Microsoft.Win32
Imports System.Configuration
Public Class arquivos
    Inherits System.Web.UI.Page
#Region "Objetos e Variveis"
    Protected WithEvents dtgArquivos As System.Web.UI.WebControls.DataGrid
    Protected WithEvents cboTransacoes As System.Web.UI.WebControls.DropDownList
    Protected lblErro As System.Web.UI.WebControls.Label

    Private strdir As String
    Private dirInfo As DirectoryInfo
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
        Try
            Session("objDataSetDestinos") = Nothing
            Session("objDataSetArquivos") = Nothing

            Dim strTipoPasta As String
            Dim tipoAcesso As Integer
            tipoAcesso = CType(Session("tipoAcesso"), Integer)
            strTipoPasta = IIf(tipoAcesso = 1, "internas", IIf(tipoAcesso = 2, "externas", "temporarias"))

            strdir = ConfigurationSettings.AppSettings("diretorioArquivos")
            strdir += strTipoPasta & "\"
            strdir += Session("strDir")
            If CType(Session("TipoAcesso"), Integer) <> 3 Then strdir += "\recebidas"
            dirInfo = New DirectoryInfo(strdir)

            If IsPostBack Then Exit Sub

            'pega todos os diretórios de transmissao do fornecedor
            Dim objDiretorios As DirectoryInfo()
            objDiretorios = dirInfo.GetDirectories()

            If CType(Session("tipoAcesso"), Integer) = 3 Then
                cboTransacoes.Visible = False
                dirInfo = New DirectoryInfo(strdir)
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

            If dtgArquivos.Items.Count > 0 Then
                lblErro.Text = "Não há arquivos para download"
            End If

        Catch ex As System.Exception When IsNothing(Session("strDir"))
            lblErro.Text = "Sua sessão expirou. Feche seu navegador e acesse sistema novamente."

        Catch ex As System.Exception
            lblErro.Text = ex.Message.Trim()

        End Try
    End Sub
    Private Sub EnviaArquivo(ByVal strArquivo As String)
        Try
            Dim File As FileInfo
            File = New FileInfo(strdir & strArquivo)

            If Not (File.Exists()) Then
                Throw New System.Exception("Arquivo não encontrado")
            End If

            Response.Clear()
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + File.Name)
            Response.ContentType = TipoArquivo(File.Extension)
            Response.WriteFile(File.FullName)

        Catch ex As System.Exception When IsNothing(Session("strDir"))
            lblErro.Text = "Sua sessão expirou. Feche seu navegador e acesse sistema novamente."

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

            dirInfo = New DirectoryInfo(strdir & cboTransacoes.SelectedValue)
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
End Class
