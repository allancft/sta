<%@ Page Language="vb" AutoEventWireup="false" Codebehind="confcompanhias.aspx.vb" Inherits="sta.confcompanhias" %>
<h3>Cadastro de Companhias</h3>

    <form id="frmSTA" runat="server">
    
		<asp:Label Runat="Server" ID="lblErro" Visible="False" />

		<asp:DataGrid ID="dtgRegistros" Runat="server" AutoGenerateColumns="False"
			AllowPaging="True" AllowCustomPaging="False" AllowSorting="False"
			PagerStyle-Mode="NumericPages" PagerStyle-HorizontalAlign="Center"
			CssClass="dataGrid" DataKeyField="ANUM_COMP" PageSize="15">
				<ItemStyle CssClass="tdLinhaRegistroHand" />
				<AlternatingItemStyle CssClass="tdLinhaRegistroAltHand" />
				<HeaderStyle ForeColor="#FFFFFF" CssClass="tdCabecalho" />
				<PagerStyle CssClass="tdPaginacao" />
				<Columns>
					<asp:EditCommandColumn EditText="Editar" CancelText="Cancelar" UpdateText="Confirmar" />
					<asp:TemplateColumn>
						<ItemTemplate>
							<a id="linkDelete" href='<%#DataBinder.Eval(Container.DataItem, "ANUM_SEQU_COMP")%>'
								onclick="return confirm('Confirma a exclusão do registro ?')"
								onserverclick="Excluir"
								runat="server">Excluir
							</a>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn ReadOnly="True" DataField="ANUM_SEQU_COMP" HeaderText="Código" />
					<asp:TemplateColumn HeaderText="Nome">
						<ItemTemplate>
						<%# DataBinder.Eval(Container.DataItem, "ANUM_COMP") %>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox runat="server" id="txtNome"
								MaxLength="4" Columns="75" EnableViewState="True"
								Text='<%# Container.DataItem("ANUM_COMP") %>'
							 />
						</EditItemTemplate>
					</asp:TemplateColumn>
				</Columns>
		</asp:DataGrid>
		
		<p align="center">
			<input type="button"
				class="botaoGeral"
				value="Incluir Companhia"
				onclick="javascript:abrirJanela('popcompanhia.aspx', 300, 180);"
				onmouseover="javascript:this.className='botaoOut';"
				onmouseout="javascript:this.className='botaoGeral';"
				 />
		</p>

    </form>