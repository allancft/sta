<%@ Control language="vb" AutoEventWireup="false" Codebehind="wbcCabecalho.ascx.vb" Inherits="sta.wbcCabecalho" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML 
dir=ltr>
  <HEAD>
		<title></title>
<?xml version="1.0" encoding="iso-8859-1" ?>
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<meta http-equiv="Content-Style-Type" content="text/css">
		<link rel="stylesheet" href="../css/style.css" type="text/css" />
		<script language="javascript" src="../js/funcoes.js"></script>
</HEAD>
	<body onload="javascript:focoCampoPadrao();">
		<table background="images/layout02.gif" width="100%" border="0" cellpadding="0" cellspacing="0">
			<tr>
				<td colspan="5">
					<img src="images/layout01.gif" onclick="javascript:top.location.href='default.aspx';"
						style="CURSOR:hand" width="663" height="41" alt=""></td>
			</tr>
			<tr>
				<td>
					<table border="0" cellpadding="0" cellspacing="0">
						<tr>
							<td>
								<a href="logoff.aspx" alt="Sair do STA"><img src="images/layout03.gif" width="229" height="23" alt="Sair do sistema" border="0"></a></td>
							<td>
								<img id="imgTransf" runat="server" style="CURSOR:hand" onmouseover="javascript:document.getElementById('menuTransfConsultas').className='menuTransfVisivel';"
									onmouseout="javascript:document.getElementById('menuTransfConsultas').className='menuTransfOculto';"
									src="../../images/layout07.gif" width="113" height="23" alt="Iniciar nova transferência"
									border="0"></td>
							<td id="tdInt" runat="server">
								<%If Session("blnUsuarioAdmin") Or Session("tipoAcesso") = 1 Then%>
								<img id="imgConsul" runat="server" style="CURSOR:hand" onmouseover="javascript:document.getElementById('menuConsultas').className='menuVisivel';"
									onmouseout="javascript:document.getElementById('menuConsultas').className='menuOculto';"
									src="../../images/layout06.gif" width="98" height="23" alt="Consultas de transações"><%ElseIf Session("tipoAcesso") <> 3 Then%><a href="consultasexternas.aspx"><img id="Img1" runat="server" style="CURSOR:hand" src="../../images/layout06.gif" width="98"
										height="23" alt="Consultas de transações" border="0"></a><%End If%></td>
							<td>
								<%If Session("blnUsuarioAdmin") Then%>
								<img id="imgFornec" runat="server" style="CURSOR:hand" onclick="javascript:navegacao('cadfornecedor.aspx', 'Cadastro de Usuários');"
									src="../../images/usuarios.gif" width="120" height="23" alt="Cadastro de Usuários"><%End If%></td>
							<td>
								<%If Session("blnUsuarioAdmin") Then%>
								<img id="imgConfig" runat="server" style="CURSOR:hand" onmouseover="javascript:document.getElementById('menuConfig').className='menuConfigVisivel';"
									onmouseout="javascript:document.getElementById('menuConfig').className='menuConfigOculto';"
									src="../../images/layout04.gif" width="103" height="23" alt="Configuração do Sistema"><%End If%></td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td>
					<img src="images/layout08.gif" width="229" height="10" alt=""></td>
			</tr>
		</table>
		<div id="divTitulo" name="divTitulo" class="textoNormal"><b>Usuário:
				<%=Page.User.Identity.Name%>
			</b>
		</div>
		<div id="menuConsultas" class="menuOculto" onmouseover="javascript:document.getElementById('menuConsultas').className='menuVisivel';"
			onmouseout="javascript:document.getElementById('menuConsultas').className='menuOculto';">
			<table class="tabelaMenu" onmouseover="javascript:document.getElementById('menuConsultas').className='menuVisivel';"
				onmouseout="javascript:document.getElementById('menuConsultas').className='menuOculto';">
				<tr class="trLinhaMenu">
					<td><a href="javascript:navegacao('consultasinternas.aspx', 'Consultas - Transações Internas')" style="text-decoration: none;">Transações 
						Internas</a></td>
				</tr>
				<tr class="trLinhaMenu">
					<td><a href="javascript:navegacao('consultasexternas.aspx', 'Consultas - Transações Externas')" style="text-decoration: none;">Transações 
						Externas</a></td>
				</tr>
			</table>
		</div>
		<div id="menuTransfConsultas" class="menuTransfOculto" onmouseover="javascript:document.getElementById('menuTransfConsultas').className='menuTransfVisivel';"
			onmouseout="javascript:document.getElementById('menuTransfConsultas').className='menuTransfOculto';">
			<table class="tabelaMenu" onmouseover="javascript:document.getElementById('menuTransfConsultas').className='menuTransfVisivel';"
				onmouseout="javascript:document.getElementById('menuTransfConsultas').className='menuTransfOculto';">
				<tr class="trLinhaMenu">
					<td><a href="javascript:navegacao('usuupload.aspx', 'Envio de arquivos');" style="text-decoration: none;">Envio de 
						Arquivos</a></td>
				</tr>
				<tr class="trLinhaMenu">
					<td><a href="javascript: navegacao('usudownload.aspx', 'Cópia de arquivos');" style="text-decoration: none;">Recebimento&nbsp;de 
						Arquivos</a></td>
				</tr>
			</table>
		</div>
		<%If Session("blnUsuarioAdmin") Then%>
		<div id="menuConfig" class="menuConfigOculto" onmouseover="javascript:document.getElementById('menuConfig').className='menuConfigVisivel';"
			onmouseout="javascript:document.getElementById('menuConfig').className='menuConfigOculto';">
			<table class="tabelaMenu" onmouseover="javascript:document.getElementById('menuConfig').className='menuConfigVisivel';"
				onmouseout="javascript:document.getElementById('menuConfig').className='menuConfigOculto';">
				<tr class="trLinhaMenu">
					<td><a href="javascript:navegacao('confaplicativos.aspx', 'Aplicativos')" style="text-decoration: none;">Aplicativos</a></td>
				</tr>
				<tr class="trLinhaMenu">
					<td><a href="javascript:navegacao('confcompanhias.aspx', 'Companhias')" style="text-decoration: none;">Companhias</a></td>
				</tr>
				<tr class="trLinhaMenu">
					<td><a href="javascript:navegacao('confextensoes.aspx', 'Extensões de Arquivo')" style="text-decoration: none;">Extensões 
						de Arquivo</a></td>
				</tr>
				<tr class="trLinhaMenu">
					<td><a href="javascript:navegacao('confparametros.aspx', 'Parametrização')" style="text-decoration: none;">Parametrização</a></td>
				</tr>
				<tr class="trLinhaMenu">
					<td><a href="javascript:navegacao('confadmin.aspx', 'Usuários Administradores')" style="text-decoration: none;">Usuários 
						Administradores</a></td>
				</tr>
			</table>
		</div>
		<%End If%>
	</body>
</HTML>
