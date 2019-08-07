<%@ Page EnableViewState="false" Language="vb" AutoEventWireup="false" Codebehind="cadastrofornecedormodal.aspx.vb" Inherits="sta.cadastrofornecedormodal" %>
<?xml version="1.0" encoding="iso-8859-1"?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" dir="ltr">

<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<meta http-equiv="Content-Style-Type" content="text/css" />
<link rel="stylesheet" href="common/style.css" type="text/css" />
<script language="javascript" src="common/funcoes.js"></script>
<title><%=Application("nameApp")%></title>
</head>

<body runat="server" id="body">

<form runat="server" id="frmSTA" method="post" onsubmit="return validaForm();">

	<table class="tabelaConteudo" align="center" width="100%">
		<tr>
			<td class="tdCabecalho" colspan="2">Inclusão de Fornecedor</td>
		</tr>
		<tr>
			<td class="textoNormal" valign="top">
				Código na Itambé
			</td>
			<td>
				<asp:Textbox runat="server" onkeyup="javascript:campoNumerico(this);" id="obrTxtCodigo" maxlength="6" />
			</td>
		</tr>
		<tr>
			<td class="textoNormal" valign="top">
				Nome
			</td>
			<td>
				<asp:Textbox runat="server" id="obrTxtNome" maxlength="50" />
			</td>
		</tr>
		<tr>
			<td class="textoNormal" valign="top">
				CNPJ/CPF
			</td>
			<td>
				<asp:Textbox runat="server" id="obrCNPJCPF" maxlength="20" />
			</td>
		</tr>
		<tr>
			<td class="textoNormal" valign="top">
				EMail
			</td>
			<td>
				<asp:Textbox runat="server" id="obrTxtEmail" maxlength="50" />
			</td>
		</tr>
		<tr>
			<td class="textoNormal" valign="top">
				Telefone
			</td>
			<td>
				<asp:Textbox onkeyup="javascript:campoNumerico(this);" runat="server" id="obrTxtTelefone" maxlength="15" />
			</td>
		</tr>
		<tr>
			<td class="textoNormal" valign="top">
				Responsável
			</td>
			<td>
				<asp:Textbox runat="server" id="obrTxtResponsavel" maxlength="50" />
			</td>
		</tr>
		<tr>
			<td class="textoNormal" valign="top">
				Senha
			</td>
			<td>
				<asp:TextBox Runat="server" ID="txtPassword" TextMode="Password" maxlength="50" />
			</td>
		</tr>
		<tr>
			<td class="textoNormal" valign="top">
				E-Mail do responsável<br />na Itambé
			</td>
			<td>
				<asp:TextBox Runat="server" ID="txtMailResponsavel" maxlength="100" />
			</td>
		</tr>
		<tr>
			<td class="textoNormal" valign="top" colspan="2">
				<asp:CheckBox Runat="server" Text="Manter janela em inclusão" id="chkMantem" CssClass="checkboxGeral" />
			</td>
		</tr>
	</table>

	<input type="hidden" id="codFornecedor" runat="server" />
	<input type="hidden" id="hidExclusao" runat="server" />

	<table border="0" align="center">
		<tr>
			<td>
				<asp:Button ID="btnSubmit" Runat="server" CssClass="botaoGeral" onmouseover="javascript:this.className='botaoOut';" onmouseout="javascript:this.className='botaoGeral';" text="Incluir" Width="100"/>
			</td>
			<td>
				<input type="button" id="btnExcluir" name="btnExcluir" Runat="server" class="botaoGeral" onmouseover="javascript:this.className='botaoOut';" onmouseout="javascript:this.className='botaoGeral';" value="Excluir" style="display:none" />
			</td>
			<td>
				<input type="button" class="botaoGeral" onmouseover="javascript:this.className='botaoOut';" onmouseout="javascript:this.className='botaoGeral';" value="Fechar janela" onclick="javascript:self.close();" style="width:85px" >
			</td>
		</tr>
	</table>
	
</form>

</body>
</HTML>