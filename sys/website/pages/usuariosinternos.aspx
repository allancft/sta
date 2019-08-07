<%@ Page Language="vb" AutoEventWireup="false" Codebehind="usuariosinternos.aspx.vb" Inherits="sta.usuariosinternos" %>
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

<body runat="server" id="body">

    <form id="Form1" method="post" runat="server">
    
    <asp:DropDownList ID="cboUsuarios" Runat="server" />
    
    <br /><br />
    
    <asp:Label ID="lblErro" Runat="server" />

    </form>

</body>
</html>
