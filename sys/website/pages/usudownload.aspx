<%@ Page Language="vb" AutoEventWireup="false" Codebehind="usudownload.aspx.vb" Inherits="sta.usudownload" %>
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