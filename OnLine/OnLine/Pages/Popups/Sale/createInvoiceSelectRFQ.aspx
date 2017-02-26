<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="createInvoiceSelectRFQ.aspx.cs" Inherits="OnLine.Pages.Popups.Sale.createInvoiceSelectRFQ" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <link type="text/css" rel="stylesheet" href="~/css/bootstrap.css" /> 
    <style type="text/css">
        .style1
        {
            color: #33CC33;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center" style="font-family: Andalus; font-size: small">
    
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    
        <br />
        If you select any existing RFQ, the product/service details will be copied from 
        the RFQ.<br />
        <br />
        Select RFQ (showing only those which you have WON):&nbsp;
        <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList1" runat="server" 
            style="font-family: Andalus; font-size: small">
        </asp:DropDownList>
        <br />
&nbsp;(<span class="style1">GREEN</span> items are those for which one invoice has 
        already been created. You can choose to create more invoice)<br />
        <br />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <asp:Button ID="Button_With_RFQ" runat="server" 
class="btn btn-sm btn-success"
                            onclick="Button_With_RFQ_Click" style="font-family: Andalus" 
                            Text="Proceed With the Selected RFQ!" />
            

        &nbsp;<asp:Button ID="Button_Without_RFQ" runat="server" 
class="btn btn-sm btn-success"
                            onclick="Button_Without_RFQ_Click" style="font-family: Andalus" 
                            Text="Proceed without any RFQ!" />
            
                    </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
