<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DispImage_Deprecated.aspx.cs" Inherits="OnLine.Pages.DispImage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:ListView ID="ListView1" runat="server" DataSourceID="ObjectDataSource1">
        <EmptyDataTemplate>
            <table id="Table1" runat="server" 
                style="background-color: #FFFFFF;border-collapse: collapse;border-color: #999999;border-style:none;border-width:1px;">
                <tr>
                    <td>
                        No data was returned.</td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <ItemTemplate>
            <td id="Td2" runat="server" style="background-color: #E0FFFF;color: #333333;">
                <asp:Image ID="Image1" runat="server" ImageUrl='<%#Eval("img")%>' Width="250px" Height="250px" />
                <br />
            </td>
        </ItemTemplate>
        </asp:ListView>
    
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
            SelectMethod="poPulateImage" TypeName="OnLine.Pages.DispImage">
        </asp:ObjectDataSource>
    
    </div>
    </form>
</body>
</html>
