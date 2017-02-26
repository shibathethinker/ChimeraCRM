<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DispEntityDetails.aspx.cs" Inherits="OnLine.Pages.DispEntityDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">






legend {color:red}
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <div align="center">
    
            <br />
                        <asp:Panel ID="Panel_Customer" runat="server" 
            style="font-family: Andalus; font-size: small" Font-Size="Medium">
                            <br />
                            <asp:Label ID="Label_1" runat="server" Text="Name:" Font-Size="Medium"></asp:Label>
                            &nbsp;<asp:Label ID="Label_Cust_Name" runat="server" Font-Size="Medium" 
                                Text="N/A"></asp:Label>
                            &nbsp;&nbsp;
                            <asp:Label ID="Label_2" runat="server" Text="Email Id:" Font-Size="Medium"></asp:Label>
                            &nbsp;<asp:Label ID="Label_Email" runat="server" Font-Size="Medium" Text="N/A"></asp:Label>
                            &nbsp;&nbsp;
                            <asp:Label ID="Label_3" runat="server" Text="Contact No:" Font-Size="Medium"></asp:Label>
                            &nbsp;<asp:Label ID="Label_Contact_No" runat="server" Font-Size="Medium" 
                                Text="N/A"></asp:Label>
                            &nbsp;&nbsp;
                            <asp:Label ID="Label_9" runat="server" Font-Size="Medium" Text="Main Business:"></asp:Label>
                            &nbsp;<asp:Label ID="Label_Main_Business" runat="server" Font-Size="Medium" 
                                Text="N/A"></asp:Label>
                            &nbsp;&nbsp;&nbsp;<asp:Label ID="Label_4" runat="server" Font-Size="Medium" 
                                Text="From Website?:"></asp:Label>
                            &nbsp;<asp:Label ID="Label_From_Site" runat="server" Font-Size="Medium" Text="N/A"></asp:Label>
                            <br />
                            <br />
                            <asp:Label ID="Label_5" runat="server" Font-Size="Medium" Text="Locality:"></asp:Label>
                            &nbsp;<asp:Label ID="Label_Locality" runat="server" Font-Size="Medium" Text="N/A"></asp:Label>
                            &nbsp;
                            <asp:Label ID="Label_6" runat="server" Font-Size="Medium" Text="City:"></asp:Label>
                            <asp:Label ID="Label_City" runat="server" Font-Size="Medium" Text="N/A"></asp:Label>
                            &nbsp;
                            <asp:Label ID="Label_7" runat="server" Font-Size="Medium" Text="State:"></asp:Label>
                            <asp:Label ID="Label_State" runat="server" Font-Size="Medium" Text="N/A"></asp:Label>
                            &nbsp;
                            <asp:Label ID="Label_8" runat="server" Font-Size="Medium" Text="Country:"></asp:Label>
                            <asp:Label ID="Label_Country" runat="server" Font-Size="Medium" Text="N/A"></asp:Label>
                        </asp:Panel>
    
    </div>
    
    </div>
    </form>
</body>
</html>
