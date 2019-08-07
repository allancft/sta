<%@ Import Namespace="System.Configuration"%>
<%@ Page Language="vb" AutoEventWireup="false" Codebehind="trocasenha.aspx.vb" Inherits="sta.trocasenha" %>
<?xml version="1.0" encoding="iso-8859-1"?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" dir="ltr">

<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<meta http-equiv="Content-Style-Type" content="text/css" />
<link rel="stylesheet" href="css/style.css" type="text/css" />
<script language="javascript" src="js/funcoes.js"></script>
<title><%=ConfigurationSettings.AppSettings.Get("nomesistema")%></title>
</head>

<body onload="javascript:focoCampoPadrao();">

    <form id="frmSTA" method="post" runat="server">
    
		<br /><br /><br />
		
		<table align="center" width="50%" border="0">
			<tr>
				<td colspan="2">
					<p class="textoNormal" align="justify">
						Digite sua senha atual e confirme a troca digitando a mesma senha nos dois 
						campos seguintes. Caso você não se lembre da sua senha de acesso, solicite
						o re-envio, <a href="senhaperdida.aspx" style="color:blue">clique aqui</a>.
					</p>				
				</td>
			</tr>
			<tr>
				<td class="textoNormal" width="35%" align="right">Email: </td>
				<td width="75%">
					<asp:TextBox Runat="server" Width="200" ID="txtEmail" />
				</td>
			</tr>
			<tr>
				<td class="textoNormal" width="35%" align="right">Senha atual: </td>
				<td width="75%">
					<asp:TextBox Runat="server" Width="200" ID="txtSenha" TextMode="Password" />
				</td>
			</tr>
			<tr>
				<td class="textoNormal" width="35%" align="right">Nova senha: </td>
				<td width="75%">
					<asp:TextBox Runat="server" Width="200" ID="txtNovaSenha" TextMode="Password" />
				</td>
			</tr>
			<tr>
				<td class="textoNormal" width="35%" align="right">Repita a nova senha: </td>
				<td width="75%">
					<asp:TextBox Runat="server" Width="200" ID="txtNovaSenhaR" TextMode="Password" />
				</td>
			</tr>
			<tr>
				<td colspan="2" align="center">
					<asp:Button Runat="server" onmouseover="javascript:this.className='botaoOut';" onmouseout="javascript:this.className='botaoGeral';" ID="btnSubmit" Text="Envie a senha" />
				</td>
			</tr>
			<tr>
				<td colspan="2" align="center" valign="middle" height="50">
					<asp:Label Runat="server" ID="lblErro" CssClass="lblErro" Visible="False" />
				</td>
			</tr>
		</table>

    </form>

</body>
</html>