<%@ Page EnableViewState="False" Language="vb" AutoEventWireup="false" Codebehind="popaplicativo.aspx.vb" Inherits="sta.popaplicativo" %>

	<table width="100%">
		<tr>
			<td class="tdCabecalho" colspan="2">Cadastro de Aplicativo</td>
		</tr>
	</table>
    
    <form id="frmSTA" method="post" runat="server">

	<asp:Label ID="lblErro" Runat="server" CssClass="lblErro" Visible="False" />

		<table>
			<tr>
				<td>Diretório raíz:</td>
				<td>
					<asp:Textbox EnableViewState="False" Runat="Server" ID="txtDirRaiz" Maxlength="20" null="false" />
				</td>
			</tr>
			<tr>
				<td>Nome:</td>
				<td>
					<asp:Textbox EnableViewState="False" Runat="Server" ID="txtDescricao" Maxlength="8" null="false" />
				</td>
			</tr>
		</table>

		<p align="center">
			<asp:Button CSSClass="botaoGeral" Text="Incluir" Runat="Server"
				ID="btnSubmit" onmouseover="javascript:this.className='botaoOut';"
				onmouseout="javascript:this.className='botaoGeral';"
			/>
			<input class="botaoGeral" type="button" value="Fechar"
				onclick="javascript:self.close();"
				onmouseover="javascript:this.className='botaoOut';"
				onmouseout="javascript:this.className='botaoGeral';"
			/>
		</p>

    </form>