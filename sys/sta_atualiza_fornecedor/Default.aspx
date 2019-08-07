<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>STA - Sistema de transmissão de arquivos</title>
    <link href="css/cognosDoc_basic.css" rel="stylesheet" type="text/css" />

    <script language="JavaScript" src="js/ajax_funcionario.js" type="text/javascript"></script>

    <script language="JavaScript" src="js/ajax.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div id="Main">
            <div id="Topo">
                <div id="loader">
                    Carregando....
                </div>
                <div id="Logomarca">
                    <img src="http://loki/sta_atualiza_fornecedor/imagens/Itambe_logo.jpg" />
                </div>
                <div id='NaveUser'>
                    <h1>
                        STA</h1>
                    <h2>
                        Atualização de Usuários</h2>
                </div>
            </div>
            <div id="ContainerFull">
                <h2>
                    Atualizar Usuário</h2>
                <fieldset class="FieldSetBorder">
                    <legend class="Legenda">Editor de Informações</legend>
                    <div class="Box">
                        <div id='divNotificacoes'>
                        </div>
                        <span class='FonteVermelho TextoNegrito'>Buscar:</span>
                        <br />
                        <input id="txtPesquisa" class="TextoCadastros" runat="server" style="width: 511px" type="text" autocomplete="off" onkeyup="BuscaLetra(txtPesquisa.value);" />&nbsp;
                        <div id="BoxAutoCompletar" class="TextoCadastros" onmouseleave="HideDiv(this.id)">
                      </div>
                        <br />
                        
                        <div id="Resultado" runat = "server">
                         <asp:Label ID="lbl_mensagem" runat="server" CssClass="label" Font-Bold="True" ForeColor="GradientActiveCaption"></asp:Label><br />
                       </div>
                        <div id="Painel_informacoes" runat="server">
                            <div id="Container">
                                <div class="myBox">
                                    <div class="topo-dir">
                                        <div class="baixo-esq">
                                            <div class="baixo-dir">
                                                <div class="boxAmarelo">
                                                    <div class="BoxTexto">
                                                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                            <tr>
                                                                <td style="width: 192px; height: 18px">
                                                                    <strong><span>Nome</span></strong>
                                                                </td>
                                                                <td style="width: 430px; height: 18px">
                                                                    <input id="txt_Nome" type="text" style="width: 458px" maxlength="100" runat="server" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 192px; height: 18px">
                                                                    <strong><span>CPF/CNPJ</span></strong>
                                                                </td>
                                                                <td style="width: 430px; height: 18px">
                                                                    <input id="txt_cpf" runat="server" type="text" style="width: 173px" maxlength="13" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 192px; height: 28px;">
                                                                    <strong><span>Telefone</span></strong> &nbsp; &nbsp;</td>
                                                                <td style="width: 430px; height: 28px;">
                                                                    <input id="txt_tel" type="text" style="width: 174px" maxlength="9" runat="server" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 192px; height: 28px;">
                                                                    <strong><span>Email</span></strong> &nbsp;&nbsp;</td>
                                                                <td style="width: 430px; height: 28px;">
                                                                    <input id="txt_emailFornecedor" type="text" style="width: 458px" maxlength="100" runat="server" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 192px; height: 28px">
                                                                    <strong><span>Responsável</span></strong>&nbsp;</td>
                                                                <td style="width: 430px; height: 28px">
                                                                    <input id="txt_Responsavel" type="text" style="width: 458px" maxlength="100" runat="server" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 192px; height: 28px">
                                                                    <span>Código do usuário</span></td>
                                                                <td style="width: 430px; height: 28px"><input id="txt_cod_fornecedor" type="text" style="width: 181px" maxlength="6" runat="server" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 192px; height: 28px">
                                                                <span>Diretório do usuário</span></td>
                                                                <td style="width: 430px; height: 28px"><input id="txt_Diretorio_fornecedor" type="text" style="width: 181px" maxlength="100" runat="server" /></td>
                                                            </tr>
                                                           
                                                            <tr>
                                                                <td style="width: 192px; height: 28px">
                                                                <span>Tipo de extensão de arquivo</span></td>
                                                                <td style="width: 430px; height: 28px"><input id="txt_extensao" type="text" style="width: 458px" maxlength="50" runat="server" /></td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 192px; height: 28px;">
                                                                    <strong><span>Email do Responsável</span></strong>&nbsp;
                                                                </td>
                                                                <td style="width: 430px; height: 28px;">
                                                                    <input id="txt_emailResponsavel" type="text" style="width: 458px" maxlength="50" runat="server" /></td>
                                                            </tr>
                                                        </table>
                                                        
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        
                         <br />
                         <div id ="hideDIv" runat="server">
                        <input id="btnGravar" type="submit" value="Gravar Alterações" runat="server" onserverclick="btnGravar_ServerClick" />&nbsp;
                        <input id="btn_delete" type="submit" value="Excluir" runat = "server" onserverclick="btn_delete_ServerClick" />
                        <input id="Button2" type="button" value="Limpar" onclick='LimparWebForm();' runat="server" />
                        <input id="HiddenID" type="hidden" runat='server' />
                        </div>
                    </div>
                </fieldset>
            </div>
            
        </div>
    </form>
</body>
</html>
