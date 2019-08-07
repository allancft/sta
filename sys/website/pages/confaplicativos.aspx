<%@ Page Language="vb" AutoEventWireup="false" Codebehind="confaplicativos.aspx.vb" Inherits="sta.confaplicativos" %>
<h3>Cadastro de Aplicativos</h3>
<form id="frmSTA" runat="server">
	<asp:Label Runat="Server" ID="lblErro" Visible="False" />
	<asp:DataGrid ID="dtgRegistros" Runat="server" AutoGenerateColumns="False" AllowPaging="True"
		AllowCustomPaging="False" AllowSorting="False" PagerStyle-Mode="NumericPages" PagerStyle-HorizontalAlign="Center"
		CssClass="dataGrid" DataKeyField="ANUM_SEQU_APLIC" PageSize="15">
		<ItemStyle CssClass="tdLinhaRegistroHand" />
		<AlternatingItemStyle CssClass="tdLinhaRegistroAltHand" />
		<HeaderStyle ForeColor="#FFFFFF" CssClass="tdCabecalho" />
		<PagerStyle CssClass="tdPaginacao" />
		<Columns>
			<asp:EditCommandColumn EditText="Editar" CancelText="Cancelar" UpdateText="Confirmar" />
			<asp:TemplateColumn>
				<ItemTemplate>
					<a id="linkDelete" href='<%#DataBinder.Eval(Container.DataItem, "ANUM_SEQU_APLIC")%>'
								onclick="return confirm('Confirma a exclusão do registro ?')"
								onserverclick="Excluir"
								runat="server">Excluir </a>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn ReadOnly="True" DataField="ANUM_SEQU_APLIC" HeaderText="Código" />
			<asp:TemplateColumn HeaderText="Dir">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "ANOM_DIR_RAIZ") %>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox runat="server" id="txtDir" MaxLength="20" Columns="75" EnableViewState="True" null="false" Text='<%# Container.DataItem("ANOM_DIR_RAIZ").ToString.Trim %>' />
				</EditItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Nome">
				<ItemTemplate>
					<%# DataBinder.Eval(Container.DataItem, "ANOM_APLIC") %>
				</ItemTemplate>
				<EditItemTemplate>
					<asp:TextBox runat="server" id="txtNome" MaxLength="8" Columns="75" EnableViewState="True" null="false" Text='<%# Container.DataItem("ANOM_APLIC").ToString.Trim %>' />
				</EditItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:DataGrid>
	<p align="center">
		<input type="button" class="botaoGeral" value="Incluir Aplicativo" onclick="javascript:abrirJanela('popaplicativo.aspx', 300, 180);"
			onmouseover="javascript:this.className='botaoOut';" onmouseout="javascript:this.className='botaoGeral';">
	</p>
</form>
