<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="mainEntityManagement.aspx.cs" Inherits="OnLine.Pages.Popups.AdminPref.mainEntityManagement" %>

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
    <br />
    <div align="center">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title">Business Entity Details</h3>
            </div>
            <div class="panel-body">  
                <asp:Panel ID="Panel_Entity_Det" runat="server" 
            style="font-family: Andalus">
                    Organization Name:
            <asp:TextBox class="form-control" ID="TextBox_Name" runat="server"  
                Width="20%"></asp:TextBox>
                    &nbsp; Email Id:
                    <asp:TextBox class="form-control" ID="TextBox_Email" runat="server"  Width="20%"></asp:TextBox>
                    &nbsp;
                    <br />
                    Contact No:&nbsp;
                    <asp:TextBox class="form-control" ID="TextBox_Contact" runat="server"  
                        Width="20%"></asp:TextBox>
                    &nbsp; Web site:
                    <asp:TextBox class="form-control" ID="TextBox_Site" runat="server"  
                        Width="20%"></asp:TextBox>
                    &nbsp;<br /> Owner Name:
                    <asp:TextBox class="form-control" ID="TextBox_Owner_Name" runat="server"  
                        Width="20%"></asp:TextBox>
                    &nbsp; Description:
                    <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Business_Desc" runat="server" 
                        CssClass="style2">
                    </asp:DropDownList>
                    <br />
                    <br />
                    Main product/service range:&nbsp;&nbsp;
                    <asp:ListBox ID="ListBoxProdServc" runat="server"  
                        Height="51px" SelectionMode="Multiple" Width="15%"></asp:ListBox>
                    &nbsp;<br />
                    &nbsp;&nbsp;<br />
            <asp:Button ID="Button_Change_Details" runat="server" class="btn btn-sm btn-success"
                onclick="Button_Change_Details_Click" style="font-family: Andalus" Text="Submit!" 
                ValidationGroup="changePass" />
                    &nbsp;<br />
            <asp:Label ID="Label_Det_Change_Stat" runat="server" Visible="False"></asp:Label>
        </asp:Panel>
        </div>
        </div>
                <br />
        </ContentTemplate>
              </asp:UpdatePanel>  
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>                 
                                      <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title">Address and Currency Details</h3>
            </div>
            <div class="panel-body">       
                <asp:Panel ID="Panel_Chain_Det" runat="server"  
                    style="font-family: Andalus">
                    <br />
                    Address Line1:
                    <asp:Label ID="Label_Addr1" runat="server" Font-Size="Medium" Text="N/A"></asp:Label>
                    &nbsp;<asp:Label ID="Label_1" runat="server" Font-Size="Medium" Text="Country:"></asp:Label>
                    <asp:Label ID="Label_Country" runat="server" Font-Size="Medium" Text="N/A"></asp:Label>
                    <asp:Label ID="Label_2" runat="server" Font-Size="Medium" Text="State:"></asp:Label>
                    <asp:Label ID="Label_State" runat="server" Font-Size="Medium" Text="N/A"></asp:Label>
                    <br />
                    <asp:Label ID="Label_3" runat="server" Font-Size="Medium" Text="City:"></asp:Label>
                    <asp:Label ID="Label_City" runat="server" Font-Size="Medium" Text="N/A"></asp:Label>
                    <asp:Label ID="Label_4" runat="server" Font-Size="Medium" Text="Locality:"></asp:Label>
                    <asp:Label ID="Label_Locality" runat="server" Font-Size="Medium" Text="N/A"></asp:Label>
                    &nbsp;<asp:Label ID="Label_5" runat="server" Font-Size="Medium" 
                        Text="Base Currency:"></asp:Label>
                    &nbsp;<asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Base_Curr" runat="server" 
                        style="font-family: Andalus">
                    </asp:DropDownList>
                    <br />
                    <br />
                            <div class="panel-info" Width="95%">
        <div class="panel-heading">
            <h3 class="panel-title">Enter new address</h3>
            </div>
            <div class="panel-body">    
                    <asp:Panel ID="Panel_New_Addr" runat="server" 
                        style="font-family: Andalus">
                        Country:
                        <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Country" runat="server" AutoPostBack="True" 
                            onselectedindexchanged="DropDownList_Country_SelectedIndexChanged" 
                            style="font-family: Andalus">
                        </asp:DropDownList>
                        &nbsp;State/Province:
                        <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_State" runat="server" AutoPostBack="True" 
                            onselectedindexchanged="DropDownList_State_SelectedIndexChanged" 
                            style="font-family: Andalus">
                        </asp:DropDownList>
                        &nbsp;City:
                        <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_City" runat="server" AutoPostBack="True" 
                            onselectedindexchanged="DropDownList_City_SelectedIndexChanged" 
                            style="font-family: Andalus">
                        </asp:DropDownList>
                        &nbsp;Locality:
                        <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Locality" runat="server" 
                            style="font-family: Andalus">
                        </asp:DropDownList>
                        &nbsp;Street Name:
                        <asp:TextBox class="form-control" ID="TextBox_Street_Name" runat="server"  
                            Width="20%"></asp:TextBox>
                        <br />
                        <br />
                        <asp:Button ID="Button_Change_Addr" runat="server" class="btn btn-sm btn-success"
                            onclick="Button_Change_Addr_Click" style="font-family: Andalus" 
                            Text="Update" />
                        &nbsp;<br />
                        <asp:Label ID="Label_Addr_Change_Stat" runat="server" Visible="False"></asp:Label>
                    </asp:Panel>
                    </div>
                    </div>
                    </asp:Panel>
                    </div>
                    </div>
        </ContentTemplate>
        </asp:UpdatePanel>
                         <br />
            <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title">Business Logo</h3>
            </div>
            <div class="panel-body">       
                              <asp:Panel ID="Panel_Logo" runat="server" 
            style="font-family: Andalus">
                                    Existing Logo:
                                    <asp:Image ID="Image_Logo" runat="server" Height="123px" Visible="False" 
                                        Width="209px" />
                                <br />
                                    <br />
                                    Upload New Logo:<asp:FileUpload ID="FileUpload1" runat="server" 
                                         Width="20%" />
                                    &nbsp;&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator4" 
                                        runat="server" ControlToValidate="FileUpload1" Display="Dynamic" 
                                        ErrorMessage="Please select a file" ForeColor="Red" SetFocusOnError="True" 
                                        ValidationGroup="changeLogo"></asp:RequiredFieldValidator>
                                    <br />&nbsp;<br /><asp:Button ID="Button_Insert_Image" runat="server" class="btn btn-sm btn-success"
                onclick="Button_Insert_Image_Click" style="font-family: Andalus" Text="Submit!" 
                ValidationGroup="changeLogo" />
                                    &nbsp;<br />
            <asp:Label ID="Label_Upload_Logo" runat="server" Visible="False"></asp:Label>
        </asp:Panel>
        </div>
        </div>
                <br />
                         <br />
                        <br />

    </div>
    <p>
        &nbsp;</p>
    </form>
</body>
</html>
