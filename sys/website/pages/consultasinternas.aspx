<%@ Page Language="vb" AutoEventWireup="false" Codebehind="consultasinternas.aspx.vb" Inherits="sta.consultasinternas" %>
<h3>Consulta de Transações Internas</h3>

<form runat="server" method="post">

	<input runat="server" type="hidden" id="codFornecedor"/>

	<table border="0" cellpadding="0" cellspacing="2">
		<tr>
			<td colspan="2" class="textoNormal">Usuário:</td>
		</tr>
		<tr>
			<td class="textoNormal" colspan="2">
				<asp:TextBox Runat="Server" ID="txtNome" size="20" />
				<input type="button" value="..." title="Clique para consultar usuários no Active Directory" onclick="javascript:abrirJanelaAD('txtNome', '400', '350');" />
			</td>
		</tr>
		<tr>
			<td class="textoNormal">Data Inicial:</td>
			<td class="textoNormal">Data Final:</td>
		</tr>
		<tr>
			<td class="textoNormal">
				<asp:TextBox Runat="Server" ReadOnly="True" ID="txtDataInicial" size="10" MaxLength="10" />
				<asp:Button Runat="server" id="btnCalInicial" text="..." />
			</td>
			<td>
				<asp:TextBox Runat="Server" ReadOnly="True" ID="txtDataFinal" size="10" MaxLength="10" />
				<asp:Button Runat="server" id="btnCalFinal" Text="..." />
			</td>
			<td align="right">
				<asp:Button Runat="server" ID="btnConsultar" text="Consultar" onmouseover="javascript:this.className='botaoOut';" onmouseout="javascript:this.className='botaoGeral';" />
			</td>
		</tr>
	</table>

	<asp:DataGrid ID="dtgRegistros" Runat="server" AutoGenerateColumns="False"
		AllowPaging="True" AllowCustomPaging="False" AllowSorting="True"
		PagerStyle-Mode="NumericPages" PagerStyle-HorizontalAlign="Center"
		CssClass="dataGrid" DataKeyField="ANUM_SEQU_REGISTRO_INTERNO" PageSize="15">
			<ItemStyle CssClass="tdLinhaRegistroHand" />
			<AlternatingItemStyle CssClass="tdLinhaRegistroAltHand" />
			<HeaderStyle ForeColor="#FFFFFF" CssClass="tdCabecalho" />
			<PagerStyle CssClass="tdPaginacao" />
			<Columns>
				<asp:BoundColumn DataField="ANUM_SEQU_REGISTRO_INTERNO" SortExpression="ANUM_SEQU_REGISTRO_INTERNO" HeaderText="Codigo" />
				<asp:BoundColumn DataField="ADES_DESTINATARIO" SortExpression="ADES_DESTINATARIO" HeaderText="Nome" />
				<asp:BoundColumn DataField="DATA" SortExpression="DATA" HeaderText="Data" />
			</Columns>
	</asp:DataGrid>
	
	<asp:Label ID="lblErro" CssClass="lblErro" Runat="server" />
		
</form>
