<%@ Page Language="vb" AutoEventWireup="false" Codebehind="popconsarquivosint.aspx.vb" Inherits="sta.popconsarquivos" %>
<?xml version="1.0" encoding="iso-8859-1"?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" dir="ltr">

<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<meta http-equiv="Content-Style-Type" content="text/css" />
		<link rel="stylesheet" href="../css/style.css" type="text/css" />
		<script language="javascript" src="../js/funcoes.js"></script>
<title></title>
</head>

<body id="body" runat="server">

    <form id="frmSTA" method="post" runat="server">
    
    <asp:Label Runat="server" CssClass="textoNormal" ID="lblCodigo" />

			<asp:DataGrid ID="dtgArquivos" Runat="server" CssClass="dataGrid" AutoGenerateColumns="False"
				AllowPaging="False" PagerStyle-Mode="NumericPages" PagerStyle-HorizontalAlign="Center">
				<ItemStyle CssClass="tdLinhaRegistro" />
				<AlternatingItemStyle CssClass="tdLinhaRegistroAlt" />
				<HeaderStyle ForeColor="#FFFFFF" CssClass="tdCabecalho" />
				<PagerStyle CssClass="tdPaginacao" />
				<Columns>
					<asp:BoundColumn DataField="ANOM_ARQUIVO" HeaderText="Arquivo" />
				</Columns>
			</asp:DataGrid>
			
	<asp:Label Runat="server" ID="lblErro" CssClass="lblErro" />

    </form>
    
    <p align="center">
		<input type="button" onclick="javascript:self.close()" value="Fechar Janela" onmouseover="javascript:this.className='botaoOut';" onmouseout="javascript:this.className='botaoGeral';" id="btnFechar" />
    </p>

</body>
</html>