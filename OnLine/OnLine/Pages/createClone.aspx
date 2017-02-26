<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="createClone.aspx.cs" Inherits="OnLine.Pages.createClone" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style8
        {
            border-collapse: collapse;
            border-bottom-color: "#0066CC";
            font-size: small;
            border-left-width: thin;
            border-right-width: thin;
            border-top-width: thin;
            border-bottom-width: thin;
        }
        * {
    padding: 0; 
    margin: 0;
    font-size: small;
    font-family: Andalus;
}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
    
        Name:
                                                                    <asp:TextBox class="form-control" ID="TextBox_Name" runat="server" Width="516px" BorderStyle="Inset"></asp:TextBox>                                                                 
                                                                    &nbsp;<asp:Button 
            ID="Button_Clone" runat="server" 
                            onclick="Button_Clone_Click" 
                            style="font-family: Andalus" Text="Submit!" />
                        <br />
        <asp:Label ID="Label_Clone_Stat" runat="server" Visible="False"></asp:Label>
        <br />
    
    </div>
    </form>
</body>
</html>
