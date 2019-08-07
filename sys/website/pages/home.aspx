<%@ Page EnableViewState="false" Language="vb" AutoEventWireup="false" Codebehind="home.aspx.vb" Inherits="sta.home" %>

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

<table background="images/layout02.gif" width="100%" border="0" cellpadding="0" cellspacing="0">
	<tr>
		<td colspan="5">
			<img src="images/layout01.gif" onclick="javascript:top.location.href='home.aspx';" style="cursor:hand" width=663 height=41 alt=""></td>
	</tr>
	<tr>
		<td>
			<table border="0" cellpadding="0" cellspacing="0">
				<tr>
					<td>
						<a href="logoff.aspx" alt="Sair do STA"><img src="images/layout03.gif" width="229" height="23" alt="Sair do sistema" border="0"></a></td>
					<td>
						<img id="imgTransf" runat="server" style="cursor:hand" onmouseover="javascript:document.getElementById('menuTransfConsultas').className='menuTransfVisivel';" onmouseout="javascript:document.getElementById('menuTransfConsultas').className='menuTransfOculto';" onclick="javascript:top.location.href='home.aspx';" src="images/layout07.gif" width=113 height=23 alt="Iniciar nova transferência"></td>
					<td>
						<img id="imgFornec" runat="server" style="cursor:hand" onclick="javascript:navegacao('cadastrofornecedor.aspx', 'Cadastro de Fornecedores');" src="images/layout05.gif" width="120" height="23" alt="Cadastro de Fornecedores"></td>
					<td>
						<img id="imgConsul" runat="server" style="cursor:hand" onmouseover="javascript:document.getElementById('menuConsultas').className='menuVisivel';" onmouseout="javascript:document.getElementById('menuConsultas').className='menuOculto';" src="images/layout06.gif" width="98" height="23" alt="Consultas de transações"></td>
					<td>
						<img id="imgConfig" runat="server" style="cursor:hand" onclick="javascript:navegacao('configuracao.aspx', 'Configuração');" src="images/layout04.gif" width="103" height="23" alt="Configuração do Sistema"></td>
				</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td>
			<img src="images/layout08.gif" width="229" height="10" alt=""></td>
	</tr>
</table>

<div id="divTitulo" name="divTitulo" class="textoNormal"></div>

<div id="menuConsultas" class="menuOculto" onmouseover="javascript:document.getElementById('menuConsultas').className='menuVisivel';" onmouseout="javascript:document.getElementById('menuConsultas').className='menuOculto';">
	<table class="tabelaMenu" onmouseover="javascript:document.getElementById('menuConsultas').className='menuVisivel';" onmouseout="javascript:document.getElementById('menuConsultas').className='menuOculto';">
		<tr class="trLinhaMenu">
			<td onclick="javascript:navegacao('consultasinternas.aspx', 'Consultas - Transações Internas')">Transações Internas</td>
		</tr>
		<tr class="trLinhaMenu">
			<td onclick="javascript:navegacao('consultasexternas.aspx', 'Consultas - Transações Externas')">Transações Externas</td>
		</tr>
	</table>
</div>

<div id="menuTransfConsultas" class="menuTransfOculto" onmouseover="javascript:document.getElementById('menuTransfConsultas').className='menuTransfVisivel';" onmouseout="javascript:document.getElementById('menuTransfConsultas').className='menuTransfOculto';">
	<table class="tabelaMenu" onmouseover="javascript:document.getElementById('menuTransfConsultas').className='menuTransfVisivel';" onmouseout="javascript:document.getElementById('menuTransfConsultas').className='menuTransfOculto';">
		<tr class="trLinhaMenu">
			<td onclick="javascript:navegacao('envio.aspx', 'Envio de arquivos')">Envio de Arquivos</td>
		</tr>
		<tr class="trLinhaMenu">
			<td onclick="javascript:navegacao('copia.aspx', 'Cópia de arquivos')">Cópia de Arquivos</td>
		</tr>
	</table>
</div>

<iframe width="100%" border="0" frameborder="no" scrolling="auto" name="iframeNavegacao" id="iframeNavegacao" src="envio.aspx"></iframe>

</body>
</html>