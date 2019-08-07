<%@ Page Language="vb" AutoEventWireup="false" Codebehind="usuupload.aspx.vb" Inherits="sta.usuupload" %>

<h3 id="htitulo" runat="server">
	Envio de Arquivos</h3>
<form id="frmSTA" method="post" runat="server">
	<table id="tabelaTipoEnvio" align="center" runat="server">
		<tr id="linhaTipoEnvio" runat="server">
			<td>
				<asp:imagebutton id="btnInterno" imageurl="images/enviointerno.gif" runat="server"
					alternatetext="Envio Interno (Itambé)" borderstyle="None" backcolor="#f8f8f8"></asp:imagebutton>
			</td>
			<td>
				<asp:imagebutton id="btnFornecedores" imageurl="images/envioexterno.gif" runat="server"
					alternatetext="Envio Externo (Fornecedores)" borderstyle="None" backcolor="#f8f8f8"></asp:imagebutton>
			</td>
			<td>
				<asp:imagebutton id="btnTemporaria" imageurl="images/chave.gif" runat="server" alternatetext="Chave Temporária Externa"
					borderstyle="None" backcolor="#f8f8f8"></asp:imagebutton>
			</td>
		</tr>
	</table>
	<table width="90%" align="center">
		<tr>
			<td valign="top" width="50%">
				<asp:datagrid id="dtgDestinos" runat="server" datakeyfield="Login" autogeneratecolumns="False"
					cssclass="dataGrid">
					<ItemStyle CssClass="tdLinhaRegistro" />
					<AlternatingItemStyle CssClass="tdLinhaRegistroAlt" />
					<HeaderStyle ForeColor="#FFFFFF" CssClass="tdCabecalho" />
					<PagerStyle CssClass="tdPaginacao" />
					<Columns>
						<asp:ButtonColumn Text="Remover" CommandName="Delete" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
							HeaderImageUrl="images/lixeira.gif" />
						<asp:BoundColumn DataField="Login" HeaderText="E-Mail" />
						<asp:BoundColumn DataField="Nome" HeaderText="Nome" />
						<asp:BoundColumn DataField="diretorio" Visible="False" />
						<asp:BoundColumn DataField="codigo" Visible="False" />
					</Columns>
				</asp:datagrid>
			</td>
			<td valign="top" width="50%">
				<asp:datagrid id="dtgArquivos" runat="server" datakeyfield="arquivo" autogeneratecolumns="False"
					cssclass="dataGrid">
					<ItemStyle CssClass="tdLinhaRegistro" />
					<AlternatingItemStyle CssClass="tdLinhaRegistroAlt" />
					<HeaderStyle ForeColor="#FFFFFF" CssClass="tdCabecalho" />
					<PagerStyle CssClass="tdPaginacao" />
					<Columns>
						<asp:ButtonColumn Text="Remover" CommandName="Delete" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
							HeaderImageUrl="images/lixeira.gif" />
						<asp:BoundColumn DataField="arquivo" SortExpression="arquivo" HeaderText="Arquivos" />
					</Columns>
				</asp:datagrid>
			</td>
		</tr>
	</table>
	<div id="DivMensagem" align="center" runat="server">
		<br>
		<font face="Arial" size="1">*Se você deseja enviar uma mensagem no corpo do email, basta
			escrevê-la no campo abaixo:</font>
		<asp:textbox id="txtMensagem" runat="server" height="80px" width="448px" textmode="MultiLine"></asp:textbox>
	</div>
	<!-- Botões de envio -->
	<table width="100%">
		<tr>
			<td align="center">
				<table align="center">
					<tr id="trComandosMailArquivo" runat="server" visible="False">
						<td>
							<input id="btnMail" style="border-bottom-style: none; border-right-style: none; background-color: #f8f8f8;
								border-top-style: none; border-left-style: none" type="image" alt="Adicionar destinos"
								src="images/addMail.gif" name="btnMail" runat="server" autopostback="False">
						</td>
						<td>
							<input id="btnArquiv" style="border-bottom-style: none; border-right-style: none;
								background-color: #f8f8f8; border-top-style: none; border-left-style: none" type="image"
								src="images/addArquiv.gif" value="Adicionar arquivos" name="btnArquiv" runat="server">
						</td>
					</tr>
					<tr id="trComandosConfirmacao" runat="server" visible="False">
						<td>
							<asp:imagebutton id="btnCancelar" imageurl="images/cancelar.gif" runat="server" alternatetext="Cancelar Envio"
								borderstyle="None" backcolor="#f8f8f8"></asp:imagebutton>
						</td>
						<td>
							<asp:imagebutton id="btnConfirmar" imageurl="images/confirmar.gif" runat="server"
								alternatetext="Confirmar Envio" borderstyle="None" backcolor="#f8f8f8" enabled="False"></asp:imagebutton>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
	<asp:label id="lblErro" runat="server" cssclass="lblErro"></asp:label>
</form>
