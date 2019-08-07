<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ad.aspx.vb" Inherits="sta.ad" %>

<script language="javascript" type="text/javascript">
	function retornaUsuario(strLogin)
	{
		var strCampo = new String();
		strCampo = document.location.search;
		strCampo = strCampo.substr(1, (strCampo.length - 1));
		
		if(!window.opener.document.getElementById(strCampo))
		{
			alert('Erro:\n\nNão foi encontrado o campo de controle do usuário pesquisado.');
			return;
		}
		var strValor = new String();
		window.opener.document.getElementById(strCampo).value += ";" + strLogin;
		self.close();
	}
</script>

	<table width="100%">
		<tr>
			<td class="tdCabecalho" colspan="2">Pesquisa de usuário</td>
		</tr>
	</table>

	<form id="frmSTA" method="post" runat="server">
	
		Nome: <asp:TextBox Runat="server" ID="txtPesquisa" Width="100px" MaxLength="30" />
		<asp:Button Runat="server" ID="btnPesquisa" Text="Pesquisar" />
		
		<br />
	
		<asp:DataGrid ID="dtgUsu" Runat="server" AutoGenerateColumns="False"
			AllowPaging="True" AllowCustomPaging="False" AllowSorting="True"
			PagerStyle-Mode="NumericPages" PagerStyle-HorizontalAlign="Center"
			CssClass="dataGrid" DataKeyField="samAccountName" PageSize="20">
				<ItemStyle CssClass="tdLinhaRegistro" />
				<HeaderStyle ForeColor="#FFFFFF" CssClass="tdCabecalho" />
				<PagerStyle CssClass="tdPaginacao" />
				<Columns>
					<asp:EditCommandColumn ButtonType="PushButton" EditText="Selecionar"></asp:EditCommandColumn>
					<asp:BoundColumn DataField="cn" SortExpression="cn" HeaderText="Nome" />
					<asp:BoundColumn DataField="samAccountName" SortExpression="samAccountName" HeaderText="Conta" />
				</Columns>
		</asp:DataGrid>
		
		<asp:Label ID="lblErro" Runat="server" />
			
	</form>
