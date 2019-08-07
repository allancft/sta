<%@ Import Namespace="System.Configuration"%>
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="default.aspx.vb" Inherits="sta._default1" %>
<?xml version="1.0" encoding="iso-8859-1"?>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" dir="ltr">

<head>
<meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
<meta http-equiv="Content-Style-Type" content="text/css" />
		<link rel="stylesheet" href="../css/style.css" type="text/css" />
		<script language="javascript" src="../js/funcoes.js"></script>
<title><%=ConfigurationSettings.AppSettings.Get("nomesistema")%></title>
</head>

	<frameset rows="0,*" framespacing="0" frameborder="no" border="0">
		<frame src="htmvazio.htm" name="topframe" scrolling="no" noresize>
		<frame src="usuupload.aspx" name="frameNavegacao">
	</frameset>

</body>
</html>
