Imports System.Data
Imports System.Data.SqlClient
Public Class cadastrofornecedormodal
    Inherits System.Web.UI.Page
#Region "Variaveis, Objetos, Propriedades e Enumeradores"
    Private Enum enumAcao
        Excluir = 1
        Incluir = 2
        Editar = 3
    End Enum
    Private Property TipoAcao() As enumAcao
        Get
            Return strAcao
        End Get
        Set(ByVal Value As enumAcao)
            strAcao = Value
        End Set
    End Property
    Private strAcao, strErro As String
    Private objPagina As New clsFornecedor
    Private objDB As New clsDB
    Private objScript As New clsScript
    Private blnRet As Boolean
    Private intCodSeqFornecedor As Integer

    Protected WithEvents obrTxtCodigo As System.Web.UI.WebControls.TextBox
    Protected WithEvents obrTxtNome As System.Web.UI.WebControls.TextBox
    Protected WithEvents obrCNPJCPF As System.Web.UI.WebControls.TextBox
    Protected WithEvents obrTxtEmail As System.Web.UI.WebControls.TextBox
    Protected WithEvents obrTxtTelefone As System.Web.UI.WebControls.TextBox
    Protected WithEvents obrTxtResponsavel As System.Web.UI.WebControls.TextBox
    Protected txtMailResponsavel As System.Web.UI.WebControls.TextBox
    Protected WithEvents txtPassword As System.Web.UI.WebControls.TextBox
    Protected WithEvents btnSubmit As System.Web.UI.WebControls.Button
    Protected WithEvents btnExcluir As System.Web.UI.HtmlControls.HtmlInputButton
    Protected WithEvents chkMantem As System.Web.UI.WebControls.CheckBox
    Protected WithEvents codFornecedor As System.Web.UI.HtmlControls.HtmlInputHidden
    Protected WithEvents hidExclusao As System.Web.UI.HtmlControls.HtmlInputHidden
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

        If Request.QueryString.Count > 0 Then 'Controla se é exclusao, edicao e seta o valor da variavel local com codigo do fornecedor
            TipoAcao = IIf(hidExclusao.Value <> "", enumAcao.Excluir, enumAcao.Editar)
            intCodSeqFornecedor = Convert.ToInt32(Request.QueryString("codFornecedor"))
        Else
            TipoAcao = enumAcao.Incluir
        End If

        If TipoAcao = enumAcao.Editar Then 'Se for edicao formata texto do botão submit e exibe botão de excluir
            btnSubmit.Text = "Editar"
            chkMantem.Visible = False

            If Not IsPostBack Then 'Caso não tenha enviado formulario, carrega a tela com valores já cadastrados
                If Not (PopulaCampos(strErro)) Then
                    strErro = "alert('" & objScript.limpaStringMensagem(strErro) & "')"
                    objScript.eventoObjetoGenerico(Me, "body", "onload", strErro)
                    Exit Sub
                End If
            End If
        End If

    End Sub
    Private Function PopulaCampos(ByRef strErro As String) As Boolean
        Try
            If Not (objDB.AbreConexao) Then
                Throw New System.Exception(objDB.MensagemErroDB)
            End If

            Dim objDataReader As SqlClient.SqlDataReader
            objPagina.objConexao = objDB
            If Not (objPagina.ConsultaFornecedor(objDataReader, _
                                                strErro, _
                                                intCodSeqFornecedor)) Then
                Throw New System.Exception(strErro)
            End If

            'Retornou registro
            If objDataReader.HasRows Then

                Dim objCriptografia As aimCriptografia.clsCriptografia
                objCriptografia = New aimCriptografia.clsCriptografia

                objDataReader.Read()
                obrTxtCodigo.Text = objDataReader("ACOD_FORNECEDOR_BAAN").ToString().Trim()
                obrCNPJCPF.Text = objDataReader("ANUM_CNPJ_CPF").ToString().Trim()
                obrTxtEmail.Text = objDataReader("ADES_EMAIL_FORNECEDOR").ToString().Trim()
                obrTxtNome.Text = objDataReader("ANOM_FORNECEDOR").ToString().Trim()
                obrTxtResponsavel.Text = objDataReader("ANOM_RESPONSAVEL_FORNECEDOR").ToString().Trim()
                obrTxtTelefone.Text = objDataReader("ANUM_TEL_FORNECEDOR").ToString().Trim()
                txtMailResponsavel.Text = objDataReader("ADES_EMAIL_RESPONSAVEL").ToString().Trim()
            End If

            'Adiciona evento onclick no botão de exclusao
            With btnExcluir
                .Attributes.Add("onclick", "javascript:if(!excluirRegistro()){return;}document.forms[0].hidExclusao.value='1';")
                .Style.Clear()
                .Style.Add("width", "100px")
                .Style.Add("display", "block")
            End With

        Catch objErro As System.Exception
            strErro = objScript.limpaStringMensagem(objErro.Message.ToString().Trim())

        Finally
            objDB.FechaConexao()

        End Try

        PopulaCampos = (strErro = "")
    End Function
    Private Function SalvaRegistro() As Boolean
        Try
            If Not (objDB.AbreConexao) Then
                Throw New System.Exception(objDB.MensagemErroDB)
            End If

            objPagina.objConexao = objDB

            If TipoAcao = enumAcao.Editar Then 'editando existente

                blnRet = objPagina.EditaFornecedor(intCodSeqFornecedor, _
                                                obrTxtCodigo.Text, _
                                                obrTxtNome.Text, _
                                                obrCNPJCPF.Text, _
                                                obrTxtEmail.Text, _
                                                obrTxtTelefone.Text, _
                                                obrTxtResponsavel.Text, _
                                                txtMailResponsavel.Text, _
                                                strErro, _
                                                txtPassword.Text)
            Else
                blnRet = objPagina.IncluiFornecedor(obrTxtCodigo.Text, _
                                                obrTxtNome.Text, _
                                                obrCNPJCPF.Text, _
                                                obrTxtEmail.Text, _
                                                obrTxtTelefone.Text, _
                                                obrTxtResponsavel.Text, _
                                                txtPassword.Text, _
                                                txtMailResponsavel.Text, _
                                                strErro)
            End If

            If Not blnRet Then
                Throw New System.Exception(strErro)
            End If

        Catch objErro As System.Exception
            strErro = objScript.limpaStringMensagem(objErro.Message.ToString().Trim())

        Finally
            objDB.FechaConexao()

        End Try

        SalvaRegistro = (strErro = "")
    End Function
    Private Function ExcluiRegistro(ByRef strErro As String) As Boolean
        Try
            If Not (objDB.AbreConexao) Then
                Throw New System.Exception(objDB.MensagemErroDB)
            End If

            objPagina.objConexao = objDB

            If Not (objPagina.ExcluiFornecedor(intCodSeqFornecedor, _
                                        strErro)) Then
                Throw New System.Exception(strErro)
            End If

            TipoAcao = enumAcao.Excluir

        Catch objErro As System.Exception
            strErro = objScript.limpaStringMensagem(strErro)

        End Try

        ExcluiRegistro = (strErro = "")
    End Function
    Private Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        If Not (SalvaRegistro()) Then
            strErro = "alert('" & objScript.limpaStringMensagem(strErro) & "')"
            objScript.eventoObjetoGenerico(Me, "body", "onload", strErro)
            Exit Sub
        Else
            objScript.eventoObjetoGenerico(Me, "body", "onload", "javascript:alert('Ação concluída com sucesso.');window.opener.location.href='cadastrofornecedor.aspx';")
            If Not (chkMantem.Checked) Then 'fecha janela se check não estiver marcado
                objScript.eventoObjetoGenerico(Me, "body", "onload", "self.close();")
            End If
        End If
    End Sub
    Private Sub btnExcluir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcluir.ServerClick
        If Not (ExcluiRegistro(strErro)) Then
            strErro = "alert('" & objScript.limpaStringMensagem(strErro) & "')"
            objScript.eventoObjetoGenerico(Me, "body", "onload", strErro)
            Exit Sub
        Else
            objScript.eventoObjetoGenerico(Me, "body", "onload", "javascript:alert('Ação concluída com sucesso.');window.opener.document.forms[0].submit();self.close();")
        End If
    End Sub
End Class
