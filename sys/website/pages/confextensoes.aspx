<%@ Page Language="vb" AutoEventWireup="false" Codebehind="confextensoes.aspx.vb" Inherits="sta.confextensoes" %>
<h3>Configura��o de Extens�es</h3>

<br >

<form runat="server" onsubmit="return validaForm()" ID="frmSTA">

	<asp:Label Runat="Server" ID="lblErro" Visible="False" />

	<table class="tabelaConteudo" align="center">
		<tr>
			<td class="textoNormal" valign="top">
				Extens�es de arquivo permitidas<br />para usu�rios <b>externos</b>.
			</td>
			<td class="textoNormal">
				<asp:TextBox Runat="server" TextMode="MultiLine" ID="txtExtensoesExt" rows="3" Columns="60" onkeypress="return tamanhoMaximo(this, 100);" />
				<br >
				Obs: Extens�es devem ser delimitadas por ";"
			</td>
		</tr>
		<tr>
			<td class="textoNormal" valign="top">
				Extens�es de arquivo permitidas<br />para usu�rios <b>internos</b>.
			</td>
			<td class="textoNormal">
				<asp:TextBox Runat="server" TextMode="MultiLine" ID="txtExtensoesInt" rows="3" Columns="60" onkeypress="return tamanhoMaximo(this, 100);" />
				<br >
				Obs: Extens�es devem ser delimitadas por ";"
			</td>
		</tr>
	</table>

	<p align="center">
		<asp:Button ID="btnSubmit" Runat="server"
			CssClass="botaoGeral" onmouseover="javascript:this.className='botaoOut';"
			onmouseout="javascript:this.className='botaoGeral';"
			Text="Salvar" />
	</p>
	
</form>