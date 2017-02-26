<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TagRFQtoReq.aspx.cs" Inherits="OnLine.Pages.Popups.Purchase.TagRFQtoReq" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link type="text/css" rel="stylesheet" href="~/css/bootstrap.css" /> 
    <style type="text/css">
        .style1
        {
            font-size: small;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center" style="font-family: Andalus">
    
        <span class="style1">Requirement List: </span>
        <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Req_List" runat="server" 
            style="font-family: Andalus; font-size: small">
        </asp:DropDownList>
        <br />
        <br />
        <asp:Button ID="Button1" runat="server" onclick="Button1_Click" class="btn btn-sm btn-success"
            style="font-family: Andalus; font-size: small" Text="Tag!" />
&nbsp;<asp:Label ID="Label_Update_Stat" runat="server" style="font-size: small" 
            Visible="False"></asp:Label>
    
    </div>
    </form>
</body>
</html>
