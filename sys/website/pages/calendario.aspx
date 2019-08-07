<%@ Page Language="vb" AutoEventWireup="false" Codebehind="calendario.aspx.vb" Inherits="sta.calendario" %>
<?xml version="1.0" encoding="iso-8859-1"?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" dir="ltr">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<meta http-equiv="Content-Style-Type" content="text/css" />
<link rel="stylesheet" href="common/style.css" type="text/css" />
<script language="javascript" src="common/funcoes.js"></script>
<title><%=Application("nameApp")%></title>
</head>
<body>
	<center>
		<form runat="server" ID="frmSTA">
			<asp:calendar CssClass="tabelaConteudo" ID="calendario" runat="server">
				<TodayDayStyle CssClass="tdLinhaRegistroAlt"></TodayDayStyle>
				<NextPrevStyle Font-Size="8pt" Font-Bold="True" ForeColor="#333333" VerticalAlign="Bottom"></NextPrevStyle>
				<DayHeaderStyle Font-Size="8pt" Font-Bold="True"></DayHeaderStyle>
				<SelectedDayStyle CssClass="tdLinhaRegistro"></SelectedDayStyle>
				<TitleStyle Font-Size="12pt" Font-Bold="True" BorderWidth="4px" ForeColor="#333399" BorderColor="Black" BackColor="White"></TitleStyle>
				<OtherMonthDayStyle ForeColor="#999999"></OtherMonthDayStyle>
			</asp:calendar>
			<input type="hidden" id="hidControle" runat="server">
		</form>
	</center>
</body>
</html>