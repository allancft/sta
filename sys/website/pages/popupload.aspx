<%@ Page Language="vb" AutoEventWireup="false" Codebehind="popupload.aspx.vb" Inherits="sta.popupload" %>

	<table width="100%">
		<tr>
			<td class="tdCabecalho" colspan="2">Envio de Arquivo</td>
		</tr>
	</table>

    <form id="frmSTA" method="post" runat="server">

		<table align="center">
			<tr>
				<td>Arquivo:</td>
				<td>
					<input id="txtarquivo" type="file" size="30" name="txtarquivo" runat="server"> 
				</td>
			</tr>
			<tr>
				<td align="center" colspan="2">
					<input class="botaoGeral" id="btnEnvio"
						type="submit" value="Enviar Arquivo"
						name="btnEnvio" runat="server"
						onserverclick="UploadArquivo"
						onclick="javascript:document.getElementById('lblErro').innerText='Aguarde...';"
						onmouseover="javascript:this.className='botaoOut';"
						onmouseout="javascript:this.className='botaoGeral';"
					> 
				</td>
			</tr>
		</table>
		
		<p align="center">
			<asp:Label ID="lblErro" Runat="server" CssClass="lblErro" />
		</p>

    </form>
