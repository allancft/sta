<%@ Page Language="vb" AutoEventWireup="false" Codebehind="copia.aspx.vb" Inherits="sta.arquivos" %>
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

	<h3>Cópia de Arquivos</h3>

    <form id="frmSTA" method="post" runat="server">

		<asp:DataGrid runat="server" id="dtgArquivos" AutoGenerateColumns="False"
			CssClass="dataGrid">
			<ItemStyle CssClass="tdLinhaRegistro" />
			<HeaderStyle ForeColor="#FFFFFF" CssClass="tdCabecalho" />
			<PagerStyle CssClass="tdPaginacao" />
			<Columns>
				<asp:ButtonColumn ItemStyle-Width="100px" ButtonType="PushButton" Text="Copiar arquivo" Visible="True" />
				<asp:BoundColumn DataField="Name" HeaderText="Nome" />
				<asp:BoundColumn DataField="Length" HeaderText="Tamanho" DataFormatString="{0:#,### bytes}" />
			</Columns>
		</asp:DataGrid>
		
		<p align="right">
			<asp:DropDownList Runat="server" ID="cboTransacoes" AutoPostBack="True">
				<asp:ListItem></asp:ListItem>
			</asp:DropDownList>
		</p>

    </form>
    
    <asp:Label ID="lblErro" Runat="server" CssClass="lblErro" />

</body>
</html>