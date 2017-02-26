<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllLead_TnC.aspx.cs" Inherits="OnLine.Pages.Popups.Sale.AllLead_TnC" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
            <link type="text/css" rel="stylesheet" href="~/css/bootstrap.css" /> 
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
    
        <asp:TextBox class="form-control" ID="TextBox_TnC" runat="server" Enabled="False" Height="185px" TextMode="MultiLine"
            MaxLength="500" style="font-family: Andalus; font-size: small;" 
            Width="80%">No terms and conditions specified</asp:TextBox>
        <br />
        <br />
        <asp:Button ID="Button_TnC" runat="server" Enabled="False" class="btn btn-sm btn-success"
            onclick="Button_TnC_Click" style="font-family: Andalus; font-size: small" 
            Text="Update" />
&nbsp;<asp:Label ID="Label_TnC_Stat" runat="server" style="font-family: Andalus; font-size: small;" 
            Visible="False"></asp:Label>
    
        <br />
    
    </div>
    </form>
</body>
</html>
