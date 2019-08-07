<%@ Page Language="vb" AutoEventWireup="false" Codebehind="confparametros.aspx.vb" Inherits="sta.confparametros" %>
<h3>Parametriza��o (Servi�o de exclus�o)</h3>

<br />

<form runat="server" onsubmit="return validaForm()" ID="frmSTA">

	<asp:Label Runat="Server" ID="lblErro" Visible="False" />

	<table class="tabelaConteudo" align="center">
		<tr>
			<td class="textoNormal" valign="top">
				Dias para exclus�o autom�tica dos arquivos
			</td>
			<td>
				<asp:textbox runat="server" id="obrTxtDiasExclusao" onkeyup="javascript:campoNumerico(this);" maxlength="3" />
			</td>
		</tr>
	</table>

	<p align="center">
		<asp:Button ID="btnSubmit" Runat="server"
			CssClass="botaoGeral" onmouseover="javascript:this.className='botaoOut';"
			onmouseout="javascript:this.className='botaoGeral';"
			Text="Salvar" />
	</p>
	
<h3>Criptografia de par�metros (web.config)</h3>

<br />
	
	<table class="tabelaConteudo" align="center">
		<tr>
			<td class="textoNormal" valign="top">
				Valor do par�metro
			</td>
			<td class="textoNormal" >
				<asp:textbox runat="server" TextMode="Password" id="txtParam" maxlength="14" />
				<br />Criptografada:
				<asp:Label CssClass="textoNormal" ID="lblCriptografia" Runat="server" />
			</td>
		</tr>
	</table>

	<p align="center">
		<asp:Button ID="btnCriptografia" Runat="server"
			CssClass="botaoGeral" onmouseover="javascript:this.className='botaoOut';"
			onmouseout="javascript:this.className='botaoGeral';"
			Text="Criptografar" />
	</p>

</form>
