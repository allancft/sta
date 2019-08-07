<%@ Page Language="vb" AutoEventWireup="false" Codebehind="confadmin.aspx.vb" Inherits="sta.confadmin" %>
<h3>Usuários Administradores</h3>

<br />

<form runat="server" onsubmit="return validaForm()" ID="frmSTA">

	<asp:Label Runat="Server" ID="lblErro" Visible="False" />

	<table class="tabelaConteudo" align="center">
		<tr>
			<td class="textoNormal">
				<asp:TextBox Runat="server" TextMode="MultiLine" ID="txtUsuariosAdmin" rows="5" Columns="100" onkeypress="return tamanhoMaximo(this, 100);" />
				<input type="button" value="..." title="Clique para consultar usuários no Active Directory" onclick="javascript:abrirJanelaAD('txtUsuariosAdmin', '400', '350');" />
				<br >
				Obs: Usuários devem ser delimitadas por ";"
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
