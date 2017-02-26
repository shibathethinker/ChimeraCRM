<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllContacts_ProdSrv.aspx.cs" Inherits="OnLine.Pages.Popups.Contacts.AllContacts_ProdSrv" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 <link type="text/css" rel="stylesheet" href="~/css/bootstrap.css" /> 
    <title></title>
        <style type="text/css">

legend {color:red}
        .style2
        {
            text-align: center;
            margin-left: 40px;
        }
        .style1
        {
            font-family: Andalus;
        }
        .style3
       {
           margin-top: 0;
           margin-bottom: 0;
           font-size: 16px;
           color: inherit;
           font-family: Andalus;
       }
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
    
    <div align="center">
    
    <div align="center">
    
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    
        <br />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <div class="panel panel-primary" Width="95%">
        <div class="panel-heading">
            <h3 class="style3">Product Service Details</h3>
            </div>
            <div class="panel-body">  
                        <asp:Panel ID="Panel_Prd_Srv" runat="server"                    
            style="font-family: Andalus; font-size: small" Font-Size="Medium" >
                            <br />
                            <asp:Label ID="Label_1" runat="server" 
                                Text="Main Product/Services of this Contact:" Font-Size="Medium"></asp:Label>
                            &nbsp; <asp:Label ID="Label_Main_Prd_Srv" runat="server" Font-Size="Medium" 
                                Text="N/A"></asp:Label>
                           </asp:Panel>
                           </div>
                           </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div align="center">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
                <div class="panel panel-primary" Width="95%">
        <div class="panel-heading">
            <h3 class="style3">Edit Product or Service Details</h3>
            </div>
            <div class="panel-body">  
                        <asp:Panel ID="Panel_Prod" runat="server"                             
                            style="font-family: Andalus" >
                            Main product/service range:&nbsp;&nbsp;
                            <asp:ListBox ID="ListBoxProdServc" runat="server"  
                                Height="51px" SelectionMode="Multiple"></asp:ListBox>
                            &nbsp;
                            <asp:Button ID="Buttin_Update" runat="server" onclick="Buttin_Update_Click" class="btn btn-sm btn-success"
                                style="font-family: Andalus" Text="Update" />
                            &nbsp;<asp:Label ID="Label_Status" runat="server" Visible="False"></asp:Label>
        </asp:Panel>
        </div>
        </div>
                </ContentTemplate>
        </asp:UpdatePanel>

    </div>
        <br />
    
    </div>
    
    </div>
    </form>
</body>
</html>
