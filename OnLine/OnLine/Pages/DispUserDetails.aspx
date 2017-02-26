<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DispUserDetails.aspx.cs" Inherits="OnLine.Pages.DispUserDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link type="text/css" rel="stylesheet" href="~/Styles/Panel_Backgroud.css" />
    <title></title>
    <style type="text/css">
* {
    padding: 0; 
    font-size: small;
    font-family: Andalus;
    margin-left: 0;
    margin-right: 0;
    margin-bottom: 0;
}

legend {color:red}
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
    
        <asp:Panel ID="Panel_User_Details" runat="server" GroupingText="User Details" 
            style="font-family: Andalus" CssClass="Panel3_Gradient">
            Name:
            <asp:Label ID="Label_Name" runat="server" Text="N/A"></asp:Label>
            &nbsp;&nbsp;<br /> Email Id:&nbsp;<asp:Label ID="Label_Email" runat="server">N/A</asp:Label>
            &nbsp;&nbsp;<br /> Contact#:
            <asp:Label ID="Label_Contact" runat="server">N/A</asp:Label>
        </asp:Panel>
    
    </div>
    </form>
</body>
</html>
