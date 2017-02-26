<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllLead_Location.aspx.cs" Inherits="OnLine.Pages.Popups.Sale.AllLead_Location" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title></title>
        <link type="text/css" rel="stylesheet" href="~/css/bootstrap.css" /> 
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
    <div>
    
        <div align="center">
    
            <br />
    
    </div>
    
    </div>
        <div align="center">
    
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
    
        <br />
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                                 <div class="panel panel-primary" Width="95%">
        <div class="panel-heading">
            <h3 class="style3">Location Details</h3>
            </div>
            <div class="panel-body">     
                        <asp:Panel ID="Panel_Location" runat="server"                     
            style="font-family: Andalus" Font-Size="Medium" 
                            >
                            <br />
                            <asp:Label ID="Label_1" runat="server" Text="Country:" Font-Size="Medium"></asp:Label>
                            &nbsp;<asp:Label ID="Label_Country" runat="server" Font-Size="Medium" Text="N/A"></asp:Label>
                            &nbsp;&nbsp;
                            <asp:Label ID="Label_2" runat="server" Text="State:" Font-Size="Medium"></asp:Label>
                            &nbsp;<asp:Label ID="Label_State" runat="server" Font-Size="Medium" Text="N/A"></asp:Label>
                            &nbsp;&nbsp;
                            <asp:Label ID="Label_3" runat="server" Text="City:" Font-Size="Medium"></asp:Label>
                            &nbsp;<asp:Label ID="Label_City" runat="server" Font-Size="Medium" Text="N/A"></asp:Label>
                            &nbsp;&nbsp;
                            <asp:Label ID="Label_4" runat="server" Text="Locality:" Font-Size="Medium"></asp:Label>
                            &nbsp;<asp:Label ID="Label_Locality" runat="server" Font-Size="Medium" Text="N/A"></asp:Label>
                        </asp:Panel>
                        </div>
                        </div>
            </ContentTemplate>
            </asp:UpdatePanel>
            
    </div>
   
        <div align="center">    
        <br />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                                             <div class="panel panel-primary" Width="95%">
        <div class="panel-heading">
            <h3 class="style3">Update Location Details</h3>
            </div>
            <div class="panel-body">     
                        <asp:Panel ID="Panel_Location_Edit" runat="server" 
                               style="font-family: Andalus" Font-Size="Medium" >
                            <br />
                            Country:
                            <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Country" runat="server" AutoPostBack="True" 
                                onselectedindexchanged="DropDownList_Country_SelectedIndexChanged" 
                                style="font-family: Andalus">
                            </asp:DropDownList>
                            &nbsp;State/Province:&nbsp;<asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_State" runat="server" 
                                AutoPostBack="True" 
                                onselectedindexchanged="DropDownList_State_SelectedIndexChanged" 
                                style="font-family: Andalus">
                            </asp:DropDownList>
                            &nbsp;City:
                            <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_City" runat="server" AutoPostBack="True" 
                                onselectedindexchanged="DropDownList_City_SelectedIndexChanged" 
                                style="font-family: Andalus">
                            </asp:DropDownList>
                            &nbsp;Locality:&nbsp;<asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Locality" runat="server" 
                                style="font-family: Andalus">
                            </asp:DropDownList>
                            &nbsp;Street Name:
                            <asp:TextBox class="form-control" ID="TextBox_Street_Name" runat="server"  
                                Width="20%"></asp:TextBox>
                            <br />
                            <br />
                            <asp:Button ID="Button_Update" runat="server" Text="Update" class="btn btn-sm btn-success"
                                onclick="Button_Update_Click" />
                            &nbsp;<asp:Label ID="Label_Status" runat="server" Visible="False"></asp:Label>
                            <br />
                        </asp:Panel>
                        </div>
                        </div>
            </ContentTemplate>
            </asp:UpdatePanel>
            
        <br />
    
    </div>

    </form>
</body>
</html>
