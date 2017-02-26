<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="createContact.aspx.cs" Inherits="OnLine.Pages.createContact" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
      <link href="../css/bootswatch_flaty.css" rel="stylesheet" type="text/css" />
    <link href="../css/bootstrap-theme.min.css" rel="stylesheet" type="text/css" />
    <link href="../css/Custom.css?version=5" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-2.2.3.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    
    <script language="javascript">
        function RefreshParent() {
            window.opener.document.getElementById('Button_Refresh').click();            
           /* window.opener.localtion.reload(true); */
        }
        /* This function is called when contact is created from contact page and the contact page grid needs to be refreshed */
        function RefreshParentPostCreation() {
            window.opener.document.getElementById('ContentPlaceHolderBody_Button_Contact_Refresh_Hidden').click();
        }
    </script>
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
    
        <br />
                    <div class="panel panel-primary" Width="95%">
        <div class="panel-heading">
            <h3 class="style3">Create New or Choose From the site</h3>
            </div>
            <div class="panel-body">    
        <asp:Panel ID="Panel_Contact" runat="server" 
            style="font-family: Andalus">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <br />
            <br />
            <asp:Label ID="Label_Contact_Search_Site" runat="server" BackColor="#009999" 
                ForeColor="White" style="font-weight: 700" 
                
                Text="If the account is from this site.. then enter the company name (starting with):"></asp:Label>
            &nbsp;<asp:TextBox class="form-control" ID="TextBox_Search_Contact" runat="server"  
                Width="20%"></asp:TextBox>
            &nbsp;<br />
            <br />
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
            <asp:Button ID="Buttin_Show_Spec_List" runat="server" class="btn btn-sm btn-success"
                onclick="Buttin_Show_Spec_List_Click" style="font-family: Andalus" 
                Text="Show Contact!" />
            &nbsp;<asp:Label ID="Label_Status_Search" runat="server" Text="Label" 
                Visible="False"></asp:Label>
            <br />
            <br />
                    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" CellPadding="4" 
                        GridLines="None" 
                                    CssClass="table table-striped table-bordered table-hover tableShadow" 
                        onselectedindexchanged="GridView1_SelectedIndexChanged" PageSize="5" 
                        Visible="False" onpageindexchanging="GridView1_PageIndexChanging" 
                        AutoGenerateColumns="False" onrowcreated="GridView1_RowCreated" 
                        onrowdatabound="GridView1_RowDataBound">
                        <Columns>
                            <asp:CommandField SelectText="Select!" ShowSelectButton="True" />
                            <asp:TemplateField HeaderText="Name">
                                <ItemTemplate>
                                    <asp:Label ID="Label_Name" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Country">
                                <ItemTemplate>
                                    <asp:Label ID="Label_Country" runat="server" Text='<%# Eval("Country") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="State">
                                <ItemTemplate>
                                    <asp:Label ID="Label_State" runat="server" Text='<%# Eval("State") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="City">
                                <ItemTemplate>
                                    <asp:Label ID="Label_City" runat="server" Text='<%# Eval("City") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Locality">
                                <ItemTemplate>
                                    <asp:Label ID="Label_Locality" runat="server" Text='<%# Eval("Locality") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Street Name">
                                <ItemTemplate>
                                    <asp:Label ID="Label_Street_Name" runat="server" 
                                        Text='<%# Eval("Street Name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Contact No">
                                <ItemTemplate>
                                    <asp:Label ID="Label_Contact_No" runat="server" Text='<%# Eval("Mob") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email Id">
                                <ItemTemplate>
                                    <asp:Label ID="Label_Email_Id" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Existing Contact?">
                                <ItemTemplate>
                                    <asp:Label ID="Label_Contact_Exists" runat="server" 
                                        Text='<%# Eval("Contact Exists?") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="localId">
                                <ItemTemplate>
                                    <asp:Label ID="Label_Local_Id" runat="server" Text='<%# Eval("localId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ContactEntId">
                                <ItemTemplate>
                                    <asp:Label ID="Label_Contact_Ent_Id" runat="server" 
                                        Text='<%# Eval("ContactEntId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                                                <RowStyle CssClass="cursor-pointer" />
                        <AlternatingRowStyle CssClass="active" />
                        <SelectedRowStyle CssClass="danger" />
                        <HeaderStyle CssClass="success" />
                        <PagerStyle CssClass="pagination-lg" />
                        <FooterStyle CssClass="success"/>
                        <EditRowStyle CssClass="info" />
                    </asp:GridView>
            <asp:Button ID="Button_Unselect" runat="server" onclick="Button_Unselect_Click" class="btn btn-sm btn-success"
                style="font-family: Andalus" Text="UnSelect Above Selection" Visible="False" />
                                </ContentTemplate>
            </asp:UpdatePanel>
            <br />
            <br />
            <asp:Label ID="Label_Extra_Spec_upload" runat="server" style="font-weight: 700" 
                Text="or create a new Account...." BackColor="#009999" ForeColor="White"></asp:Label>
            <br />
            <br />
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>

                        <div class="panel-info" Width="95%">
        <div class="panel-heading">
            <h3 class="panel-title">Create  Account Manually</h3>
            </div>
            <div class="panel-body">    

            <asp:Panel ID="Panel_Create_Contact" runat="server"  
                style="font-family: Andalus" Width="95%">
                <asp:Label ID="Label_Disable" runat="server" ForeColor="White" 
                    Text="* Selecting account from site disables this option" Visible="False" 
                    BackColor="#009999"></asp:Label>
                <br />

                                        <div class="panel-info" Width="95%">
        <div class="panel-heading">
            <h3 class="panel-title">Name and contact details</h3>
            </div>
            <div class="panel-body">   
                <asp:Panel ID="Panel_Prod_Srv_Qnty0" runat="server" GroupingText="" 
                    style="font-family: Andalus">
                    Contact Name:
                    <asp:TextBox class="form-control" ID="TextBox_Contact_Name" runat="server"  
                        Width="50%" MaxLength="50"></asp:TextBox>
                    &nbsp;Contact Number:&nbsp;<asp:TextBox class="form-control" ID="TextBox_Contact_No" runat="server" 
                         Width="20%" MaxLength="14"></asp:TextBox>
                    &nbsp;
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                        ControlToValidate="TextBox_Contact_No" Display="Dynamic" 
                        ErrorMessage="Please enter a valid contact no" ForeColor="Red" 
                        ValidationExpression="^\d{10,14}$"></asp:RegularExpressionValidator>
                    <br />
                    Email Id:
                    <asp:TextBox class="form-control" ID="TextBox_EmailId" runat="server"  
                        Width="20%" MaxLength="50"></asp:TextBox>
                    &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator1" 
                        runat="server" ControlToValidate="TextBox_EmailId" Display="Dynamic" 
                        ErrorMessage="Invalid email id" ForeColor="Red" 
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                </asp:Panel>
                </div>
                </div>
                <br />
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
            <div class="panel-info" Width="95%">
        <div class="panel-heading">
            <h3 class="panel-title">Location</h3>
            </div>
            <div class="panel-body">   
                        <asp:Panel ID="Panel_Location" runat="server" GroupingText="" 
                            style="font-family: Andalus">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Country:
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
                            &nbsp;&nbsp;&nbsp; Street Name:
                            <asp:TextBox class="form-control" ID="TextBox_Street_Name" runat="server"  
                                Width="20%"></asp:TextBox>
                        </asp:Panel>
                        </div>
                        </div>
                        <br />
                                    <div class="panel-info" Width="95%">
        <div class="panel-heading">
            <h3 class="panel-title">Your product and service details</h3>
            </div>
            <div class="panel-body">  
                        <asp:Panel ID="Panel_Prod" runat="server" 
                            GroupingText="" 
                            style="font-family: Andalus" >
                            Main product/service range:&nbsp;&nbsp;
                            <asp:ListBox ID="ListBoxProdServc" runat="server"  
                                Height="51px" SelectionMode="Multiple"></asp:ListBox>
                            &nbsp;</asp:Panel>
                            </div>
                            </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            </div>
            </div>

                        </ContentTemplate>
            </asp:UpdatePanel>
            <br />
            <asp:Label ID="Label_Short_Name" runat="server" 
                Text="Before Submit, do you want to give a short name for the Account?" 
                ForeColor="Black"></asp:Label>
            &nbsp;<asp:TextBox class="form-control" ID="TextBox_Contact_ShortName" runat="server"  
                Width="50%" MaxLength="50"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="Buttin_Submit" runat="server" class="btn btn-sm btn-success"
                Text="Submit!" onclick="Buttin_Submit_Click" 
                style="font-family: Andalus" />
            &nbsp;
            <asp:Label ID="Label_Status" runat="server" Text="Label" Visible="False"></asp:Label>
            <br />
        </asp:Panel>
        </div>
        </div>
    
    </div>
    </form>
</body>
</html>
