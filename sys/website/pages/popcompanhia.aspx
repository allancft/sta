<%@ Page EnableViewState="False" Language="vb" AutoEventWireup="false" Codebehind="popcompanhia.aspx.vb" Inherits="sta.popcompanhia" %>
	<table width="100%">
		<tr>
			<td class="tdCabecalho" colspan="2">Cadastro de Companhia</td>
		</tr>
	</table>

    <form id="frmSTA" method="post" runat="server">

	<asp:Label ID="lblErro" Runat="server" CssClass="lblErro" Visible="False" />

		<table>
			<tr>
				<td>
					Código:
					<asp:Textbox EnableViewState="False" Runat="Server" ID="txtNome" Maxlength="4" null="false" />
				</td>
			</tr>
			<tr>
				<td>
					Nome:
					<asp:Textbox EnableViewState="False" Runat="Server" ID="txtDescricao" Maxlength="20" null="false" />
				</td>
			</tr>
		</table>

		<p align="center">
			<asp:Button CSSClass="botaoGeral" Text="Incluir" Runat="Server"
				ID="btnSubmit"
				onmouseover="javascript:this.className='botaoOut';"
				onmouseout="javascript:this.className='botaoGeral';"
			/>
			<input class="botaoGeral" type="button" value="Fechar"
				onclick="javascript:self.close();"
				onmouseover="javascript:this.className='botaoOut';"
				onmouseout="javascript:this.className='botaoGeral';"
			>
		</p>

    </form>
