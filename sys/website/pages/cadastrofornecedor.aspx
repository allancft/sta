<%@ Page Language="vb" AutoEventWireup="false" Codebehind="cadastrofornecedor.aspx.vb" Inherits="sta.cadastrofornecedor" %>
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

	<h3>Cadastro de Fornecedores</h3>
	
	<form id="frmSTA" runat="server">

		<asp:DataGrid ID="dtgRegistros" Runat="server" AutoGenerateColumns="False"
			AllowPaging="True" AllowCustomPaging="False" AllowSorting="True"
			PagerStyle-Mode="NumericPages" PagerStyle-HorizontalAlign="Center"
			CssClass="dataGrid" DataKeyField="ANUM_SEQU_FORNECEDOR" PageSize="15">
				<ItemStyle CssClass="tdLinhaRegistroHand" />
				<AlternatingItemStyle CssClass="tdLinhaRegistroAltHand" />
				<HeaderStyle ForeColor="#FFFFFF" CssClass="tdCabecalho" />
				<PagerStyle CssClass="tdPaginacao" />
				<Columns>
					<asp:BoundColumn DataField="ACOD_FORNECEDOR_BAAN" SortExpression="ACOD_FORNECEDOR_BAAN" HeaderText="Código" />
					<asp:BoundColumn DataField="ANOM_FORNECEDOR" SortExpression="ANOM_FORNECEDOR" HeaderText="Nome" />
					<asp:BoundColumn DataField="ANUM_CNPJ_CPF" SortExpression="ANUM_CNPJ_CPF" HeaderText="CNPJ/CPF" />
					<asp:BoundColumn DataField="ANUM_TEL_FORNECEDOR" SortExpression="ANUM_TEL_FORNECEDOR" HeaderText="Telefone" />
					<asp:BoundColumn DataField="ANOM_RESPONSAVEL_FORNECEDOR" SortExpression="ANOM_RESPONSAVEL_FORNECEDOR" HeaderText="Responsável" />
					<asp:BoundColumn DataField="ADES_EMAIL_FORNECEDOR" SortExpression="ADES_EMAIL_FORNECEDOR" HeaderText="E-Mail" />
				</Columns>
		</asp:DataGrid>

	<p align="center">
		<input type="button" value="Incluir Fornecedor" class="botaoGeral" name="btnIncluir"
				onmouseover="javascript:this.className='botaoOut';"
				onmouseout="javascript:this.className='botaoGeral';"
				onclick="javascript:abrirJanela('cadastrofornecedormodal.aspx', 315, 290, false);" />
	</p>

	</form>
	
</body>
</html>