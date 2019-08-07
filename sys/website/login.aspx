<%@ Page Language="vb" AutoEventWireup="false" Codebehind="login.aspx.vb" Inherits="sta.login" %>
<%@ Import Namespace="System.Configuration"%>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML dir="ltr">
	<HEAD>
		<title>
			<%=ConfigurationSettings.AppSettings.Get("nomesistema")%>
		</title>
<?xml version="1.0" encoding="iso-8859-1" ?>
		<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1">
		<meta http-equiv="Content-Style-Type" content="text/css">
		<link rel="stylesheet" href="css/style.css" type="text/css" />
		<script language="javascript" src="js/funcoes.js"></script>
	</HEAD>
	<body onload="javascript:focoCampoPadrao();">
		<form id="frmSTA" method="post" runat="server">
			<table width="100%" height="100%" border="0">
				<tr>
					<td align="center">
						<table border="0">
							<tr>
								<td align="center">
									<p class="textoNormal"><b><%=strNome%></b></p>
									<table border="0" runat="server" id="tabelaLogin" width="352" height="185">
										<tr>
											<td width="116" height="70">&nbsp;</td>
											<td>&nbsp;</td>
										</tr>
										<tr>
											<td>&nbsp;</td>
											<td>
												<asp:TextBox cssClass="txtLogin" width="120" ID="txtEmail" Runat="server" />
											</td>
										</tr>
										<tr>
											<td></td>
											<td>
												<asp:TextBox cssClass="txtLogin" width="120" ID="txtSenha" TextMode="Password" Runat="server" />
											</td>
										</tr>
										<tr>
											<td height="45" colspan="2" align="center">
												<asp:Button ID="btnLogon" cssClass="btnLogin" background="images/btn_conectar.gif" Text="Conectar"
													Runat="server" />
											</td>
										</tr>
									</table>
								</td>
							</tr>
							<tr runat="server" id="trSenha">
								<td align="center">
									<table class="tabelaSenha">
										<tr>
											<td>
												<a href="senhaperdida.aspx" target="_self">Esqueceu a senha de acesso?</a>
											</td>
										</tr>
										<tr>
											<td>
												<a href="trocasenha.aspx" target="_self">Mudar a senha de acesso</a>
											</td>
										</tr>
									</table>
								</td>
							</tr>
							<tr>
								<td align="center">
									<asp:Label ID="lblErro" Runat="server" CssClass="lblErro" />
								</td>
							</tr>
						</table>
					</td>
				</tr>
			</table>
		</form>
	</body>
</HTML>
