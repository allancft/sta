Imports System.Data
Imports System.Data.SqlClient
Partial Class popfornecedor : Inherits clsLayoutPopup
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
#Region "Enumeradores"
    Private Enum enumAcao
        Excluir = 1
        Incluir = 2
        Editar = 3
    End Enum
#End Region
#Region "Propriedades"
    Private Property TipoAcao() As enumAcao
        Get
            Return strAcao
        End Get
        Set(ByVal Value As enumAcao)
            strAcao = Value
        End Set
    End Property
#End Region
#Region "Variáveis"
    Private strAcao As String
    Private objPagina As New clsFornecedor
    Private objDB As New clsDB
    Private objScript As New clsScript
    Private blnRet As Boolean
    Private intCodSeqFornecedor As Integer
#End Region
#Region "Objetos Asp.Net"
#End Region
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        PaginaProtegida(Session("blnUsuarioAdmin"))
        If Request.QueryString.Count > 0 Then 'Controla se é exclusao, edicao e seta o valor da variavel local com codigo do fornecedor
            TipoAcao = IIf(hidExclusao.Value <> "", enumAcao.Excluir, enumAcao.Editar)
            intCodSeqFornecedor = Convert.ToInt32(Request.QueryString("codFornecedor"))
        Else
            TipoAcao = enumAcao.Incluir
            'Adiciona obrigatoriedade de preenchimento
            txtPassword.Attributes.Add("null", "false")
        End If

        If TipoAcao = enumAcao.Editar Then 'Se for edicao formata texto do botão submit e exibe botão de excluir

            btnSubmit.Text = "Editar"
            chkMantem.Visible = False

            If Not IsPostBack Then 'Caso não tenha enviado formulario, carrega a tela com valores já cadastrados
                If Not (PopulaCampos(StrErro)) Then
                    With lblErro
                        .Visible = True
                        .Text = StrErro
                    End With
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

                Dim objCriptografia As aim.objCriptografia
                objCriptografia = New aim.objCriptografia

                objDataReader.Read()
                txtCodigo.Text = objDataReader("ACOD_FORNECEDOR_BAAN").ToString().Trim()
                txtCNPJCPF.Text = objDataReader("ANUM_CNPJ_CPF").ToString().Trim()
                txtEmail.Text = objDataReader("ADES_EMAIL_FORNECEDOR").ToString().Trim()
                txtNome.Text = objDataReader("ANOM_FORNECEDOR").ToString().Trim()
                txtResponsavel.Text = objDataReader("ANOM_RESPONSAVEL_FORNECEDOR").ToString().Trim()
                txtTelefone.Text = objDataReader("ANUM_TEL_FORNECEDOR").ToString().Trim()
                txtMailResponsavel.Text = objDataReader("ADES_EMAIL_RESPONSAVEL").ToString().Trim()
                txtExtensoes.Text = objDataReader("AFTP_EXT_ARQU").ToString().Trim()
                txtDiretorio.Text = objDataReader("ADES_DIRETORIO_FORNECEDOR").ToString().Trim()
                hidDiretorio.Value = IIf(objDataReader("ADES_DIRETORIO_FORNECEDOR").ToString().Trim() <> "", objDataReader("ADES_DIRETORIO_FORNECEDOR").ToString().Trim(), objDataReader("ACOD_FORNECEDOR_BAAN").ToString().Trim())
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
        Dim strMsg As String
        Try
            If Not (objDB.AbreConexao) Then
                Throw New System.Exception(objDB.MensagemErroDB)
            End If

            objPagina.objConexao = objDB

            If TipoAcao = enumAcao.Editar Then 'editando existente

                blnRet = objPagina.Edita(intCodSeqFornecedor, _
                                        txtCodigo.Text, _
                                        TxtNome.Text, _
                                        txtCNPJCPF.Text, _
                                        TxtEmail.Text, _
                                        TxtTelefone.Text, _
                                        txtResponsavel.Text, _
                                        txtMailResponsavel.Text, _
                                        StrErro, _
                                        txtPassword.Text, _
                                        txtExtensoes.Text, _
                                        txtDiretorio.Text, _
                                        hidDiretorio.Value)
                strMsg = "Fornecedor editado com sucesso"
            Else
                blnRet = objPagina.Inclui(TxtNome.Text, _
                                                txtCNPJCPF.Text, _
                                                TxtEmail.Text, _
                                                TxtTelefone.Text, _
                                                txtResponsavel.Text, _
                                                txtPassword.Text, _
                                                txtMailResponsavel.Text, _
                                                txtExtensoes.Text, _
                                                txtDiretorio.Text, _
                                                StrErro, _
                                                txtCodigo.Text)
                strMsg = "Fornecedor incluído com sucesso"
            End If

            If Not blnRet Then
                Throw New System.Exception(StrErro)
            End If

            If Not (objDB.FechaConexao) Then
                Throw New System.Exception(objDB.MensagemErroDB)
            End If

        Catch ex As System.Exception
            StrErro = ex.Message.Trim
            With lblErro
                .Visible = True
                .Text = StrErro
            End With

        End Try

        If StrErro = "" Then
            Me.RegisterStartupScript("reload", "<script language=""javascript"">alert('" & strMsg & "');window.opener.location.reload();self.close();</script>")
        End If

        Return (StrErro = "")
    End Function
    Private Function ExcluiRegistro(ByRef strErro As String) As Boolean
        Try
            If Not (objDB.AbreConexao) Then
                Throw New System.Exception(objDB.MensagemErroDB)
            End If

            objPagina.objConexao = objDB

            If Not (objPagina.Exclui(intCodSeqFornecedor, _
                                        strErro)) Then
                Throw New System.Exception(strErro)
            End If

            If Not (objDB.FechaConexao) Then
                Throw New System.Exception(objDB.MensagemErroDB)
            End If

            TipoAcao = enumAcao.Excluir

        Catch ex As System.Exception
            strErro = ex.Message.Trim
            With lblErro
                .Visible = True
                .Text = strErro
            End With

        End Try

        If strErro = "" Then
            Me.RegisterStartupScript("reload", "<script language=""javascript"">alert('Fornecedor excluído com sucesso');window.opener.location.reload();self.close();</script>")
        End If

        Return (strErro = "")
    End Function
    Private Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        If Not (SalvaRegistro()) Then
            StrErro = "alert('" & objScript.limpaStringMensagem(StrErro) & "')"
            objScript.eventoObjetoGenerico(Me, "body", "onload", StrErro)
            Exit Sub
        Else
            objScript.eventoObjetoGenerico(Me, "body", "onload", "javascript:alert('Ação concluída com sucesso.');window.opener.location.href='cadastrofornecedor.aspx';")
            If Not (chkMantem.Checked) Then 'fecha janela se check não estiver marcado
                objScript.eventoObjetoGenerico(Me, "body", "onload", "self.close();")
            Else
                txtMailResponsavel.Text = ""
                txtCNPJCPF.Text = ""
                txtCodigo.Text = ""
                txtEmail.Text = ""
                txtNome.Text = ""
                txtResponsavel.Text = ""
                txtTelefone.Text = ""
            End If
        End If
    End Sub
    Private Sub btnExcluir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcluir.ServerClick
        If Not (ExcluiRegistro(StrErro)) Then
            StrErro = "alert('" & objScript.limpaStringMensagem(StrErro) & "')"
            objScript.eventoObjetoGenerico(Me, "body", "onload", StrErro)
            Exit Sub
        Else
            objScript.eventoObjetoGenerico(Me, "body", "onload", "javascript:alert('Ação concluída com sucesso.');window.opener.document.forms[0].submit();self.close();")
        End If
    End Sub
End Class