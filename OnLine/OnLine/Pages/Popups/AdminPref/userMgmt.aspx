<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="userMgmt.aspx.cs" Inherits="OnLine.Pages.Popups.AdminPref.userMgmt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
          <link type="text/css" rel="stylesheet" href="~/css/bootstrap.css" /> 
    <title></title>
    <style type="text/css">
legend {color:red}
        .style1
        {
            font-family: Andalus;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
            <br />
    <div align="center">
                        <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title">User Details</h3>
            </div>
            <div class="panel-body">  
                <asp:Panel ID="Panel_Basic_User_Data" runat="server" 
            style="font-family: Andalus">
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                    Filter By User Id (containing):
                    <asp:TextBox class="form-control" ID="TextBox_User_Id" runat="server"  
                        Width="20%"></asp:TextBox>
&nbsp;
                    <asp:Button ID="Button_Filter" runat="server" onclick="Button_Filter_Click" class="btn btn-sm btn-success"
                        style="font-family: Andalus" Text="Search!" />
                    <br />
                    <br />
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>                    
                                        <asp:GridView ID="GridView_User_List" runat="server" AllowPaging="True" 
                                       CssClass="table table-striped table-bordered table-hover tableShadow" 
                        AutoGenerateColumns="False" CellPadding="4" Font-Size="Small" 
                        GridLines="None" width="50%"
                        onpageindexchanging="GridView_User_List_PageIndexChanging" 
                        onselectedindexchanged="GridView_User_List_SelectedIndexChanged" PageSize="3" 
                                                                    onrowcancelingedit="GridView_User_List_RowCancelingEdit" 
                                            onrowediting="GridView_User_List_RowEditing" 
                                            onrowupdating="GridView_User_List_RowUpdating" 
                                            onrowdatabound="GridView_User_List_RowDataBound">
                                                               <Columns>
                                                    <asp:CommandField ShowSelectButton="True" />
                                                    <asp:CommandField ShowEditButton="True" />
                                                    <asp:TemplateField HeaderText="Name">
                                                        <EditItemTemplate>
                                                            <asp:TextBox class="form-control" ID="TextBox_Name_Edit" runat="server" 
                                                                style="font-family: Andalus; font-size: small" Text='<%# Eval("UserName") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label_Name" runat="server" Text='<%# Eval("UserName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="User Id">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label_User_Id" runat="server" Text='<%# Eval("UserId") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Password">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label_Password" runat="server" Text='<%# Eval("Password") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox class="form-control" ID="TextBox_Password_Edit" runat="server" 
                                                                style="font-family: Andalus; font-size: small" Text='<%# Eval("Password") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Email Id">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label_Email_Id" runat="server" Text='<%# Eval("EmailId") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox class="form-control" ID="TextBox_Email_Id_Edit" runat="server" 
                                                                style="font-family: Andalus; font-size: small" Text='<%# Eval("EmailId") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Contact No">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label_Contact_No" runat="server" Text='<%# Eval("ContactNo") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox class="form-control" ID="TextBox_Contact_No_Edit" runat="server" 
                                                                style="font-family: Andalus; font-size: small" Text='<%# Eval("ContactNo") %>'></asp:TextBox>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="AccessDet" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label_Access" runat="server" Text='<%# Eval("AccessDet") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Reports To">
                                                        <EditItemTemplate>
                                                            <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Reports_To" runat="server">
                                                            </asp:DropDownList>
                                                            <asp:Label ID="Label_Reports_To_Edit" runat="server" 
                                                                Text='<%# Eval("reportsTo") %>' Visible="False"></asp:Label>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label_Reports_To" runat="server" Text='<%# Eval("reportsTo") %>'></asp:Label>
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
                                        <br />
                                        <asp:Button ID="Button_Add_New_Usr" runat="server" 
                                            class="btn btn-sm btn-success" onclick="Button_Add_New_Usr_Click" 
                                            style="font-family: Andalus" Text="Add New User" />
                                        &nbsp;<asp:Button ID="Button_Show_Access" runat="server" 
                                            class="btn btn-sm btn-success" Enabled="False" 
                                            onclick="Button_Show_Access_Click" style="font-family: Andalus" 
                                            Text="Show Access Levels!" />
                                        &nbsp;<asp:Button ID="Button_Show_Hierarchy" runat="server" class="btn btn-sm btn-success"
                                            onclick="Button_Show_Hierarchy_Click" style="font-family: Andalus" 
                                            Text="Show Complete Graph" />
                                        &nbsp;&nbsp;<asp:Button ID="Button_All_Direct_Reporting_User" runat="server" class="btn btn-sm btn-success"
                                            Enabled="False" onclick="Button_All_Direct_Reporting_User_Click" 
                                            style="font-family: Andalus" Text="All Users Directly Reporting To This User" />
                                        &nbsp;<asp:Button ID="Button_All_Reporting_User" runat="server" Enabled="False" class="btn btn-sm btn-success"
                                            style="font-family: Andalus" Text="All Users Reporting To This User" 
                                            onclick="Button_All_Reporting_User_Click" />
                    </ContentTemplate>
                    </asp:UpdatePanel>
                    &nbsp;&nbsp;<br /> &nbsp;<br />
        </asp:Panel>
        </div>
        </div>
    
    </div>
    <div align="center">
    
        <br />
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>                
        <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title"></h3>
            </div>
            <div class="panel-body">  
                    <asp:Panel ID="Panel_Basic_User_Data0" runat="server" 
            style="font-family: Andalus">
                    <asp:Label ID="Label_Access_For_User" runat="server" ForeColor="Red" 
                        style="font-size:medium" Visible="False"></asp:Label>
                    <br />
                    <br />
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>                    
                                        <asp:GridView ID="GridView_User_Access" runat="server" AllowPaging="True" CssClass="table table-striped table-bordered table-hover tableShadow" 
                        AutoGenerateColumns="False" CellPadding="4" Font-Size="Medium" 
                        ForeColor="#333333" GridLines="None"  
                        style="font-size: small" Width="50%" 
                                            onpageindexchanging="GridView_User_Access_PageIndexChanging" 
                                            onrowdeleting="GridView_User_Access_RowDeleting">
                               <Columns>
                            <asp:CommandField ShowDeleteButton="True" />
                            <asp:TemplateField HeaderText="Name">
                                <EditItemTemplate>
                                    <asp:TextBox class="form-control" ID="TextBox_Name_Edit0" runat="server" 
                                        style="font-family: Andalus; font-size: small" 
                                        Text='<%# Eval("UserName") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label_Access_Name" runat="server" 
                                        Text='<%# Eval("AccessName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Access Details">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton_Show_Access" runat="server" 
                                        CommandArgument="<%#((GridViewRow)Container).RowIndex %>" 
                                        oncommand="LinkButton_Show_Access_Command">Show!</asp:LinkButton>
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
                                        <br />
                                        <asp:Button ID="Button_Add_Access" runat="server" class="btn btn-sm btn-success"
                                            onclick="Button_Add_Access_Click" style="font-family: Andalus" 
                                            Text="Add this user to another group/s!" Enabled="False" />
                                        <br />
                    </ContentTemplate>
                    </asp:UpdatePanel>
                    &nbsp;&nbsp;<asp:Label ID="Label_All_Groups" runat="server" ForeColor="Red" 
                            style="font-size: small" Visible="False"></asp:Label>
                    <asp:GridView ID="GridView_All_AccessGroups" runat="server" AllowPaging="True" 
                        AutoGenerateColumns="False" CellPadding="4" Font-Size="Medium" 
                        ForeColor="#333333" GridLines="None"  CssClass="table table-striped table-bordered table-hover tableShadow" 
                        onselectedindexchanged="GridView_All_AccessGroups_SelectedIndexChanged" PageSize="5" 
                        style="font-size: small" Width="50%">
                        <Columns>
                            <asp:CommandField ShowSelectButton="True" SelectText="Add To This Group!" />
                            <asp:TemplateField HeaderText="Name">
                                <ItemTemplate>
                                    <asp:Label ID="Label_Group_Name" runat="server" 
                                        Text='<%# Eval("Group Name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Access Details">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton_Show_Access_For_Group" runat="server" 
                                        CommandArgument="<%#((GridViewRow)Container).RowIndex %>" 
                                        oncommand="LinkButton_Show_Access_For_Group_Command">Show!</asp:LinkButton>
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
                        <br />
                    <br /> <br />
        </asp:Panel>
        </div>
        </div>
        </ContentTemplate>
        </asp:UpdatePanel>    
        </div>
        <div align="center" id="hover" 
        style="position:fixed; right:200px; top:50px; z-index:20; width:40%; height:auto;">
            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <ContentTemplate>
        <asp:Panel ID="Panel_Reporting_Users" runat="server" 
            style="font-family: Andalus; font-size: small; margin-left: 0px; margin-top: 0px;" 
                 Visible="False" >
                        <asp:Label ID="Label_Reporting_Users" runat="server" Visible="False"></asp:Label>
                        &nbsp;&nbsp;<br />&nbsp;<asp:GridView ID="GridView_Reporting_Users" 
                        runat="server" AllowPaging="True" width="90%" CssClass="table table-striped table-bordered table-hover tableShadow" 
                    AutoGenerateColumns="False" CellPadding="4" 
                    GridLines="None" onpageindexchanging="GridView_Reporting_Users_PageIndexChanging" 
                            Visible="False" onrowdeleting="GridView_Reporting_Users_RowDeleting">                    
                            
                    <Columns>
                        <asp:CommandField ShowDeleteButton="True" />
                        <asp:TemplateField HeaderText="userId">
                            <ItemTemplate>
                                <asp:Label ID="Label_UserId" runat="server" 
                                    Text='<%# Eval("userId") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <asp:Label ID="Label_Name" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
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
                        <asp:TreeView ID="TreeView1" runat="server" ImageSet="Contacts" NodeIndent="10" 
                            Visible="False">
                            <HoverNodeStyle Font-Underline="False" />
                            <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" 
                                HorizontalPadding="5px" NodeSpacing="0px" VerticalPadding="0px" />
                            <ParentNodeStyle Font-Bold="True" ForeColor="#5555DD" />
                            <SelectedNodeStyle Font-Underline="True" HorizontalPadding="0px" 
                                VerticalPadding="0px" />
                        </asp:TreeView>
                <br />
                        &nbsp;<asp:Button ID="Button_Hide" runat="server" onclick="Button_Hide_Click" 
                            style="font-family: Andalus" Text="Hide" />


                        <br /><br /></asp:Panel>                   

            </ContentTemplate>
            </asp:UpdatePanel>

        </div>
        <div align="center" id="hover" 
        style="position:fixed; top:50px; z-index:30; width:90%; height:auto;">
                    <asp:UpdatePanel ID="UpdatePanelNewUsr" runat="server">
            <ContentTemplate>

        <asp:Panel ID="Panel_Create_New_Usr" runat="server" Visible="false" style="font-family: Andalus">
                                <div class="panel-info" Width="90%">
            <div class="panel-heading">
            <h3 class="panel-title">New User Account</h3>
            </div>
            <div class="panel-body" style="background-color:#F3F3F3">   
                                    <br />
                                    User Id:
                                    <asp:TextBox class="form-control" ID="TextBox1" runat="server"  Width="20%"></asp:TextBox>
                                    &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                                        ControlToValidate="TextBox1" Display="Dynamic" ErrorMessage="*" ForeColor="Red" 
                                        ValidationGroup="Panel1"></asp:RequiredFieldValidator>
                                    &nbsp; Passsword:
                                    <asp:TextBox class="form-control" ID="TextBox2" runat="server"  TextMode="Password" 
                                        Width="20%"></asp:TextBox>
                                    &nbsp;
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                                        ControlToValidate="TextBox2" Display="Dynamic" ErrorMessage="*" ForeColor="Red" 
                                        ValidationGroup="Panel1"></asp:RequiredFieldValidator>
                                    <br />
                                    <br />
                                    Confirm Password:
                                    <asp:TextBox class="form-control" ID="TextBox3" runat="server"  TextMode="Password" 
                                        Width="20%"></asp:TextBox>
                                    &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" 
                                        ControlToValidate="TextBox3" Display="Dynamic" ErrorMessage="*" ForeColor="Red" 
                                        ValidationGroup="Panel1"></asp:RequiredFieldValidator>
                                    <asp:CompareValidator ID="CompareValidator1" runat="server" 
                                        ControlToCompare="TextBox2" ControlToValidate="TextBox3" Display="Dynamic" 
                                        ErrorMessage="CompareValidator" ForeColor="Red">Password mismatch</asp:CompareValidator>
                                    User Name:
                                    <asp:TextBox class="form-control" ID="TextBox_User_Name_NewAccount" runat="server"  
                                        Width="20%"></asp:TextBox>
                                    &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" 
                                        ControlToValidate="TextBox_User_Name_NewAccount" Display="Dynamic" 
                                        ErrorMessage="*" ForeColor="Red" ValidationGroup="Panel1"></asp:RequiredFieldValidator>
                                    <br />           
                                    <br />                           
                                        <asp:Button ID="Create_Chain_User" runat="server" 
class="btn btn-sm btn-success"
                                            onclick="Create_Chain_User_Click" 
                                            style="text-align: center; font-family: Andalus" Text="Submit" 
                                            ValidationGroup="Panel1" />
                                        &nbsp;<asp:Button ID="Hide_Chain_User" runat="server" 
                                        class="btn btn-sm btn-success" onclick="Hide_Chain_User_Click" 
                                        style="text-align: center; font-family: Andalus" Text="Hide" />
                                        <br />
                                        <asp:Label ID="Label2" runat="server" Text="Label" Visible="False"></asp:Label>
                                </div>
                                </div>
                                </asp:Panel>

        </ContentTemplate>
        </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
