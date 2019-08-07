<%@ Page Language="vb" AutoEventWireup="false" Codebehind="popenvio.aspx.vb" Inherits="sta.popenvio" %>
<?xml version="1.0" encoding="iso-8859-1"?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" dir="ltr">

<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<meta http-equiv="Content-Style-Type" content="text/css" />
<link rel="stylesheet" href="common/style.css" type="text/css" />
<title><%=Application("nameApp")%></title>
</head>

<body id="body" runat="server">

	<h3>Envio de arquivos</h3>

    <form id="frmSTA" method="post" runat="server">

		<table align="center">
			<tr>
				<td>Arquivo:</td>
				<td>
					<input id="txtarquivo" type="file" size="30" name="txtarquivo" runat="server"> 
				</td>
			</tr>
			<tr>
				<td align="center" colspan="2">
					<input class="botao" id="btnEnvio" type="submit" value="Enviar Arquivo" name="btnEnvio" runat="server" onserverclick="UploadArquivo"> 
				</td>
			</tr>
		</table>
		
		<p align="center">
			<asp:Label ID="lblErro" Runat="server" Visible="False" CssClass="lblErro" />
		</p>

    </form>

</body>
</html>
