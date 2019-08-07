<%@ Page Language="vb" AutoEventWireup="false" Codebehind="envio.aspx.vb" Inherits="sta.envio" %>
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

	<h3>Envio de arquivos</h3>
	
		<form id="frmSTA" method="post" runat="server">

			<table align="center" runat="server" id="tabelaTipoEnvio">
				<tr id="linhaAvisoArquivos" runat="server">
					<td class="tabelaNovosArquivos" colspan="3" onclick="javascript:self.location.href='copia.aspx';">
						<img src="images/exclaim.gif" alt="Novos arquivos para você" border="0">
						Você tem arquivos para fazer download! Clique aqui.
					</td>
				</tr>
				<tr>
					<td>
						<asp:ImageButton BackColor="#f8f8f8" BorderStyle="None" AlternateText="Envio Interno (Itambé)" Runat="server" ID="btnInterno" ImageUrl="images/enviointerno.gif" />
					</td>
					<td>
						<asp:ImageButton BackColor="#f8f8f8" BorderStyle="None" AlternateText="Envio Externo (Fornecedores)" ID="btnFornecedores" Runat="server" ImageUrl="images/envioexterno.gif" />
					</td>
					<td>
						<asp:ImageButton BackColor="#f8f8f8" BorderStyle="None" AlternateText="Chave Temporária Externa" ID="btnTemporaria" Runat="server" ImageUrl="images/chave.gif" />
					</td>
				</tr>
			</table>
			
			<table width="90%" align="center">
				<tr>
					<td width="50%" valign="top">
						<asp:DataGrid ID="dtgDestinos" Runat="server" CssClass="dataGrid"
						 AutoGenerateColumns="False" DataKeyField="Login" OnDeleteCommand="RemoveDestino">
								<ItemStyle CssClass="tdLinhaRegistro" />
								<AlternatingItemStyle CssClass="tdLinhaRegistroAlt" />
								<HeaderStyle ForeColor="#FFFFFF" CssClass="tdCabecalho" />
								<PagerStyle CssClass="tdPaginacao" />
								<Columns>
									<asp:ButtonColumn Text="Remover" CommandName="Remover" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderImageUrl="images/lixeira.gif" />
									<asp:BoundColumn DataField="Login" HeaderText="E-Mail" Visible="False" />
									<asp:BoundColumn DataField="Nome" HeaderText="Nome" />
									<asp:BoundColumn DataField="diretorio" Visible="False" />
									<asp:BoundColumn DataField="codigo" Visible="False" />
								</Columns>
						</asp:DataGrid>
					</td>
					<td width="50%" valign="top">
						<asp:DataGrid ID="dtgArquivos" Runat="server" AutoGenerateColumns="False"
							AllowPaging="False" CssClass="dataGrid" DataKeyField="arquivo" OnDeleteCommand="RemoveArquivo">
								<ItemStyle CssClass="tdLinhaRegistro" />
								<AlternatingItemStyle CssClass="tdLinhaRegistroAlt" />
								<HeaderStyle ForeColor="#FFFFFF" CssClass="tdCabecalho" />
								<Columns>
									<asp:ButtonColumn Text="Remover" CommandName="Remover" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderImageUrl="images/lixeira.gif" />
									<asp:BoundColumn DataField="arquivo" SortExpression="arquivo" HeaderText="Arquivos" />
								</Columns>
						</asp:DataGrid>
					</td>
				</tr>
			</table>

			<table align="center">
				<tr id="trComandosMailArquivo" runat="server">
					<td>
						<input runat="server" id="btnMail" src="images/addMail.gif" style="BORDER-TOP-STYLE:none;BORDER-RIGHT-STYLE:none;BORDER-LEFT-STYLE:none;BACKGROUND-COLOR:#f8f8f8;BORDER-BOTTOM-STYLE:none" type="image" onclick="javascript:abrirJanela('popdestinos.aspx', 350, 300, false);" alt="Adicionar destinos" >
					</td>
					<td>
						<input src="images/addArquiv.gif" style="BORDER-TOP-STYLE:none;BORDER-RIGHT-STYLE:none;BORDER-LEFT-STYLE:none;BACKGROUND-COLOR:#f8f8f8;BORDER-BOTTOM-STYLE:none" type="image" onclick="javascript:abrirJanela('popenvio.aspx', 350, 300, false);" value="Adicionar arquivos" >
					</td>
				</tr>
				<tr id="trComandosConfirmacao" runat="server">
					<td>
						<asp:ImageButton BackColor="#f8f8f8" BorderStyle="None" AlternateText="Cancelar Envio" ID="btnCancelar" Runat="server" ImageUrl="images/cancelar.gif" />
					</td>
					<td>
						<asp:ImageButton BackColor="#f8f8f8" BorderStyle="None" AlternateText="Confirmar Envio" ID="btnConfirmar" Enabled="False" Runat="server" ImageUrl="images/confirmar.gif" />
					</td>
				</tr>
			</table>
			
			<asp:Label ID="lblErro" CssClass="lblErro" Runat="server" />
			
		</form>

</body>
</HTML>
