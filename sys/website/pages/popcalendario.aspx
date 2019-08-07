<%@ Page Language="vb" AutoEventWireup="false" Codebehind="popcalendario.aspx.vb" Inherits="sta.popcalendario" %>

    <form id="frmPMQ" method="post" runat="server">

		<asp:Calendar ID="calData" Runat="server" CssClass="dataGrid"
			DayHeaderStyle-CssClass="tdCabecalho"
			DayNameFormat="FirstLetter"
			DayStyle-CssClass="textoNormal"
			FirstDayOfWeek="Sunday"
			NextPrevFormat="CustomText"
			NextMonthText="Próximo"
			PrevMonthText="Anterior"
			SelectedDayStyle-CssClass="textoNormal"
			TitleFormat="MonthYear"
			ShowGridLines="True"
		/>
		
		<p align="center">
			<input type="button" value="Fechar janela" onmouseover="javascript:this.className='botaoOut';" onmouseout="javascript:this.className='botaoGeral';" onclick="javascript:self.close();" class="btn" />
		</p>

    </form>