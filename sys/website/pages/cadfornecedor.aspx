<%@ Page Language="vb" AutoEventWireup="false" Codebehind="cadfornecedor.aspx.vb" Inherits="sta.cadfornecedor" %>
<h3>Cadastro de Usuários</h3>

<form id="frmSTA" runat="server">

	<asp:DataGrid ID="dtgRegistros" Runat="server" AutoGenerateColumns="False"
		AllowPaging="True" EnableViewState="True" AllowCustomPaging="False" AllowSorting="True"
		PagerStyle-Mode="NumericPages" PagerStyle-HorizontalAlign="Center"
		CssClass="dataGrid" DataKeyField="ANUM_SEQU_FORNECEDOR" PageSize="15">
			<ItemStyle CssClass="tdLinhaRegistroHand" />
			<AlternatingItemStyle CssClass="tdLinhaRegistroAltHand" />
			<HeaderStyle ForeColor="#FFFFFF" CssClass="tdCabecalho" />
			<PagerStyle CssClass="tdPaginacao" />
			<Columns>
				<asp:BoundColumn DataField="ANOM_FORNECEDOR" SortExpression="ANOM_FORNECEDOR" HeaderText="Nome" />
				<asp:BoundColumn DataField="ANUM_CNPJ_CPF" SortExpression="ANUM_CNPJ_CPF" HeaderText="CNPJ/CPF" />
				<asp:BoundColumn DataField="ANUM_TEL_FORNECEDOR" SortExpression="ANUM_TEL_FORNECEDOR" HeaderText="Telefone" />
				<asp:BoundColumn DataField="ANOM_RESPONSAVEL_FORNECEDOR" SortExpression="ANOM_RESPONSAVEL_FORNECEDOR" HeaderText="Responsável" />
				<asp:BoundColumn DataField="ADES_EMAIL_FORNECEDOR" SortExpression="ADES_EMAIL_FORNECEDOR" HeaderText="E-Mail" />
			</Columns>
	</asp:DataGrid>

	<p align="center">
		<input type="button" value="Incluir Usuário" class="botaoGeral" name="btnIncluir"
				onmouseover="javascript:this.className='botaoOut';"
				onmouseout="javascript:this.className='botaoGeral';"
				onclick="javascript:abrirJanela('popfornecedor.aspx', 360, 345, false);" />
<input type="button" value="ALTERAR Usuário" class="botaoGeral" name="btnAlterar" onclick="javascript:window.open('http://www.intranet.itambe.com.br/sta_atualiza_fornecedor');">
	</p>

</form>