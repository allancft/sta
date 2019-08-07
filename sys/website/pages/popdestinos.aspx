<%@ Page Language="vb" AutoEventWireup="false" Codebehind="popdestinos.aspx.vb" Inherits="sta.popdestinos" %>
	<table width="100%">
		<tr>
			<td class="tdCabecalho" colspan="2">Adicionar Destino</td>
		</tr>
	</table>

<form id="frmSTA" method="post" runat="server">

	<asp:Label ID="lblErro" CssClass="lblErro" Runat="server" Visible="False" />

	<asp:Label ID="lblTitulo" CssClass="textoNormal" Runat="server">Pesquisa: </asp:Label><asp:TextBox ID="txtDestino" Runat="server" />

	<asp:Button ID="btnPesquisa" Runat="server" Text="Pesquisar" />

	<asp:DataGrid ID="dtgDestinos" Runat="server" CssClass="dataGrid" AutoGenerateColumns="False"
		AllowPaging="True" PagerStyle-Mode="NumericPages" PagerStyle-HorizontalAlign="Center">
		<ItemStyle CssClass="tdLinhaRegistro" />
		<AlternatingItemStyle CssClass="tdLinhaRegistroAlt" />
		<HeaderStyle ForeColor="#FFFFFF" CssClass="tdCabecalho" />
		<PagerStyle CssClass="tdPaginacao" />
		<Columns>
			<asp:EditCommandColumn ButtonType="PushButton" EditText="Selecionar"></asp:EditCommandColumn>
			<asp:BoundColumn DataField="samAccountName" HeaderText="Login" />
			<asp:BoundColumn DataField="cn" HeaderText="Nome" />
			<asp:BoundColumn DataField="diretorio" Visible="False" />
			<asp:BoundColumn DataField="codSeq" Visible="False" />
		</Columns>
	</asp:DataGrid>

</form>
