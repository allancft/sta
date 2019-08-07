<%@ Page EnableViewState="false" Language="vb" AutoEventWireup="false" Codebehind="configuracao.aspx.vb" Inherits="sta.configuracao" %>
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

<body id="body" runat="server">

<h3>Configuração Geral</h3>

<br >

<form runat="server" name="frmItambe" method="post" onsubmit="return validaForm()">

	<table class="tabelaConteudo" align="center">
		<tr>
			<td height="60" colspan="2" align="center" class="textoNormal">
				Parâmetros de configuração do aplicativo.
			</td>
		</tr>
		<tr>
			<td class="textoNormal" valign="top">
				Dias para exclusão automática dos arquivos
			</td>
			<td>
				<asp:textbox runat="server" id="obrTxtDiasExclusao" onkeyup="javascript:campoNumerico(this);" maxlength="3" />
			</td>
		</tr>
		<tr>
			<td class="textoNormal" valign="top">
				Extensões de arquivo permitidas<br />para usuários <b>externos</b>.
			</td>
			<td class="textoNormal">
				<asp:TextBox Runat="server" TextMode="MultiLine" ID="txtExtensoesExt" rows="3" Columns="30" onkeypress="return tamanhoMaximo(this, 100);" />
				<br >
				Obs: Extensões devem ser delimitadas por ";"
			</td>
		</tr>
		<tr>
			<td class="textoNormal" valign="top">
				Extensões de arquivo permitidas<br />para usuários <b>internos</b>.
			</td>
			<td class="textoNormal">
				<asp:TextBox Runat="server" TextMode="MultiLine" ID="txtExtensoesInt" rows="3" Columns="30" onkeypress="return tamanhoMaximo(this, 100);" />
				<br >
				Obs: Extensões devem ser delimitadas por ";"
			</td>
		</tr>
		<tr>
			<td class="textoNormal" valign="top">
				Usuários administradores do sistema.
			</td>
			<td class="textoNormal">
				<asp:TextBox Runat="server" TextMode="MultiLine" ID="txtUsuariosAdmin" rows="3" Columns="30" onkeypress="return tamanhoMaximo(this, 100);" />
				<input type="button" value="..." title="Clique para consultar usuários no Active Directory" onclick="javascript:abrirJanelaAD('txtUsuariosAdmin', '400', '350');" />
				<br >
				Obs: Usuários devem ser delimitadas por ";"
			</td>
		</tr>
		<tr>
			<td colspan="2" align="center">
				<asp:Button ID="btnSubmit" Runat="server" CssClass="botaoGeral" onmouseover="javascript:this.className='botaoOut';" onmouseout="javascript:this.className='botaoGeral';" text="Salvar Configuração" />
			</td>
		</tr>
		<tr>
			<td height="60" colspan="2" align="center" class="textoNormal">
				Parâmetros de configuração "Web.Config"
			</td>
		</tr>
		<tr>
			<td class="textoNormal" valign="top">
				Senha do SQL Server
			</td>
			<td class="textoNormal" >
				<asp:textbox runat="server" TextMode="Password" id="txtSenhaSQL" maxlength="14" />
				<br />Criptografada:
				<asp:Label CssClass="textoNormal" ID="lblSenhaSQL" Runat="server" />
			</td>
		</tr>
		<tr>
			<td class="textoNormal" valign="top">
				Senha do usuário de leitura<br />do Active Directory
			</td>
			<td class="textoNormal" >
				<asp:textbox runat="server" TextMode="Password" id="txtSenhaAD" maxlength="14" />
				<br />Criptografada:
				<asp:Label CssClass="textoNormal" ID="lblSenhaAD" Runat="server" />
			</td>
		</tr>
	</table>

	<p align="center">

		<asp:Button ID="btnCripto" Runat="server" CssClass="botaoGeral" onmouseover="javascript:this.className='botaoOut';" onmouseout="javascript:this.className='botaoGeral';" text="Criptografar" />

	</p>
	
</form>

</body>
</HTML>