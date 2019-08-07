<%@ Page Language="vb" AutoEventWireup="false" Codebehind="popfornecedor.aspx.vb" Inherits="sta.popfornecedor" %>
<form runat="server" id="frmSTA" method="post">
	<table class="tabelaConteudo" align="center" width="100%">
		<tr>
			<td class="tdCabecalho" colspan="2">Inclus�o de Usu�rio</td>
		</tr>
		<tr>
			<td colspan="2"><asp:Label Runat="Server" ID="lblErro" Visible="False" CssClass="lblErro" /></td>
		</tr>
		<tr>
			<td class="textoNormal" valign="top">
				C�digo na Itamb�
			</td>
			<td>
				<asp:Textbox onblur="javascript:controleDiretorio(this);" runat="server" mask="000000" id="txtCodigo" maxlength="6" />
			</td>
		</tr>
		<tr>
			<td class="textoNormal" valign="top">
				Nome
			</td>
			<td>
				<asp:Textbox runat="server" id="TxtNome" null="false" maxlength="50" />
			</td>
		</tr>
		<tr>
			<td class="textoNormal" valign="top">
				Diret�rio
			</td>
			<td>
				<asp:Textbox runat="server" id="txtDiretorio" null="false" maxlength="100" />
			</td>
		</tr>
		<tr>
			<td class="textoNormal" valign="top">
				CNPJ/CPF
			</td>
			<td>
				<asp:Textbox runat="server" id="txtCNPJCPF" null="false" maxlength="20" />
			</td>
		</tr>
		<tr>
			<td class="textoNormal" valign="top">
				E-Mail
			</td>
			<td>
				<asp:Textbox runat="server" id="TxtEmail" null="false" maxlength="50" />
			</td>
		</tr>
		<tr>
			<td class="textoNormal" valign="top">
				Telefone
			</td>
			<td>
				<asp:Textbox mask="00000000000000" null="false" runat="server" id="TxtTelefone" maxlength="15" />
			</td>
		</tr>
		<tr>
			<td class="textoNormal" valign="top">
				Respons�vel
			</td>
			<td>
				<asp:Textbox runat="server" null="false" id="txtResponsavel" maxlength="50" />
			</td>
		</tr>
		<tr>
			<td class="textoNormal" valign="top">
				Senha
			</td>
			<td>
				<asp:TextBox Runat="server" ID="txtPassword" TextMode="Password" maxlength="50" />
			</td>
		</tr>
		<tr>
			<td class="textoNormal" valign="top">
				E-Mail do respons�vel<br>
				na Itamb�
			</td>
			<td>
				<asp:TextBox Runat="server" ID="txtMailResponsavel" maxlength="100" />
			</td>
		</tr>
		<tr>
			<td class="textoNormal" valign="top">
				Extens�es permitidas
			</td>
			<td>
				<asp:TextBox Runat="server" ID="txtExtensoes" Textmode="Multiline" />
			</td>
		</tr>
		<tr>
			<td class="textoNormal" valign="top">
				Manter janela em inclus�o
			</td>
			<td class="textoNormal" valign="top">
				<asp:CheckBox Runat="server" id="chkMantem" CssClass="checkboxGeral" />
			</td>
		</tr>
	</table>
	
	<input type="hidden" id="codFornecedor" runat="server" name="codFornecedor">
	<input type="hidden" id="hidExclusao" runat="server" name="hidExclusao">
	<input type="hidden" id="hidDiretorio" runat="server" name="hidDiretorio">
	
	<table border="0" align="center">
		<tr>
			<td>
				<asp:Button ID="btnSubmit" Runat="server" CssClass="botaoGeral" onmouseover="javascript:this.className='botaoOut';"
					onmouseout="javascript:this.className='botaoGeral';" text="Incluir" Width="100" />
			</td>
			<td>
				<input type="button" id="btnExcluir" name="btnExcluir" Runat="server" class="botaoGeral"
					onmouseover="javascript:this.className='botaoOut';" onmouseout="javascript:this.className='botaoGeral';"
					value="Excluir" style="DISPLAY:none">
			</td>
			<td>
				<input type="button" class="botaoGeral" onmouseover="javascript:this.className='botaoOut';"
					onmouseout="javascript:this.className='botaoGeral';" value="Fechar janela" onclick="javascript:self.close();"
					style="WIDTH:85px">
			</td>
		</tr>
	</table>
</form>

<script language="javascript">
	function controleDiretorio(obj)
	{
		var objAux = document.getElementById('txtDiretorio');

		if (obj.value != '')
		{
			objAux.readOnly = true;
			objAux.value = obj.value;
		}
		else
		{
			objAux.readOnly = false;
			objAux = "";
		}
	}
</script>

