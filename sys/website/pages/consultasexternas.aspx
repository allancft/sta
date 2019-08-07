<%@ Page Language="vb" AutoEventWireup="false" Codebehind="consultasexternas.aspx.vb" Inherits="sta.consultasexternas" %>
<h3>Consulta de Transações Externas</h3>

<form runat="server" name="frmSTA" method="post" ID="frmSTA">

	<asp:Label id="lblErro" CssClass="lblErro" RunAt="Server" />

	<table border="0" cellpadding="0" cellspacing="2">
		<tr runat="server" id="trAdminTit" Visible="False">
			<td class="textoNormal">Fornecedor:</td>
		</tr>
		<tr runat="server" id="trAdminCampos" Visible="False">
			<td class="textoNormal">
				<asp:TextBox Runat="Server" ID="txtNome" size="20" MaxLength="100" />
				<input type="button" value="Pesquisar" onclick="javascript:abrirJanela('popfornconsulta.aspx?campo=txtNome', 350, 300, false);"" />
			</td>
		</tr>
		<tr>
			<td class="textoNormal">Tipo:</td>
			<td class="textoNormal">Baixados</td>
			<td class="textoNormal" colspan="2">Excluídos</td>
		</tr>
		<tr>
			<td>
				<asp:DropDownList ID="cboTipo" Runat="server">
					<asp:ListItem Value="-1" Selected="True">Todos</asp:ListItem>
					<asp:ListItem Value="0">Envio</asp:ListItem>
					<asp:ListItem Value="1">Recebimento</asp:ListItem>
				</asp:DropDownList>
			</td>
			<td valign="top"><asp:CheckBox runat="server" CssClass="checkboxGeral" id="chkValidados" /></td>
			<td valign="top"><asp:CheckBox runat="server" CssClass="checkboxGeral" id="chkExcluidos" /></td>
			<td align="right" colspan="6">
				<asp:Button Runat="server" ID="btnConsultar" text="Consultar" onmouseover="javascript:this.className='botaoOut';" onmouseout="javascript:this.className='botaoGeral';" />
			</td>
		</tr>
	</table>

	<asp:DataGrid ID="dtgRegistros" Runat="server" AutoGenerateColumns="False"
		AllowPaging="True" AllowCustomPaging="False" AllowSorting="True"
		PagerStyle-Mode="NumericPages" PagerStyle-HorizontalAlign="Center"
		CssClass="dataGrid" DataKeyField="ANUM_SEQU_FORNECEDOR" PageSize="15">
			<ItemStyle CssClass="tdLinhaRegistroHand" />
			<AlternatingItemStyle CssClass="tdLinhaRegistroAltHand" />
			<HeaderStyle ForeColor="#FFFFFF" CssClass="tdCabecalho" />
			<PagerStyle CssClass="tdPaginacao" />
			<Columns>
				<asp:BoundColumn DataField="ANUM_SEQU_REGISTRO" SortExpression="ANUM_SEQU_REGISTRO" HeaderText="Código" />
				<asp:BoundColumn DataField="ANOM_FORNECEDOR" SortExpression="ANOM_FORNECEDOR" HeaderText="Nome" />
				<asp:BoundColumn DataField="TIPO" SortExpression="TIPO" HeaderText="Tipo" />
				<asp:BoundColumn DataField="VALIDACAO" SortExpression="VALIDACAO" HeaderText="Já Baixado" />
				<asp:BoundColumn DataField="EXCLUSAO" SortExpression="EXCLUSAO" HeaderText="Já excluído" />
				<asp:BoundColumn DataField="DATA" SortExpression="DATA" HeaderText="Data" />
			</Columns>
	</asp:DataGrid>
		
</form>
