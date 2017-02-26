<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Notification_Sales.aspx.cs" Inherits="OnLine.Pages.Popups.Sale.Notification_Sales" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">    
        <asp:Label ID="Label1" runat="server" ForeColor="Red" 
            style="font-family: Andalus; font-size: medium"></asp:Label>
        <br />
        <br />
        <asp:Button ID="Button1" runat="server" 
            style="font-family: Andalus; font-size: small" Text="OK!" OnClientClick='window.close();' />
    </div>
    </form>
</body>
</html>
