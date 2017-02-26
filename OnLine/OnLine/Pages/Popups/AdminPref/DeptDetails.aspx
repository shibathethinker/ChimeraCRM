<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeptDetails.aspx.cs" Inherits="OnLine.Pages.Popups.AdminPref.DeptDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
<link type="text/css" rel="stylesheet" href="~/Styles/GridViewStyle.css" />
<link type="text/css" rel="stylesheet" href="~/Styles/Panel_Backgroud.css" />

    <style type="text/css">

        .style2
        {
            text-align: center;
            margin-left: 40px;
        }
        .style1
        {
            font-family: Andalus;
        }
        </style>

</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
    
    <asp:Panel ID="Panel_Dept" runat="server" GroupingText="Existing Department Details" 
            style="font-family: Andalus">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            &nbsp;&nbsp;&nbsp;<br />
                Existing Department Details<br />
            <br />
            Dept Name (Containing):
            <asp:TextBox class="form-control" ID="TextBox_DeptName_Search" runat="server"  
                Width="213px"></asp:TextBox>
            &nbsp;<asp:Button ID="Button_Dept_Filter" runat="server" 
                style="font-family: Andalus" Text="Search" 
                onclick="Button_Dept_Filter_Click" />
            &nbsp;<br />
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="GridView_Dept" runat="server" 
                        AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False" CellPadding="4" 
                        CssClass="mGrid" EditRowStyle-CssClass="sel" GridLines="None" 
                        onrowcancelingedit="GridView_Dept_RowCancelingEdit" 
                        onrowdeleting="GridView_Dept_RowDeleting" 
                        onrowediting="GridView_Dept_RowEditing" 
                        onrowupdating="GridView_Dept_RowUpdating" PagerStyle-CssClass="pgr" 
                        PageSize="3" SelectedRowStyle-CssClass="sel" Visible="False" width="50%" 
                        onrowdatabound="GridView_Dept_RowDataBound">
                        <AlternatingRowStyle CssClass="alt" />
                        <Columns>
                            <asp:CommandField ShowEditButton="True" />
                            <asp:TemplateField HeaderText="Hidden" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="Label_Hidden" runat="server" Text='<%# Eval("dept_id") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Dept Name">
                                <EditItemTemplate>
                                    <asp:TextBox class="form-control" ID="TextBox_DeptName_Edit" runat="server"  
                                        Text='<%# Eval("name") %>' Width="213px"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label_Name" runat="server" Text='<%# Eval("name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Description">
                                <EditItemTemplate>
                                    <asp:TextBox class="form-control" ID="TextBox_Descr_Edit" runat="server"  
                                        Height="75px" MaxLength="200" Text='<%# Eval("desc") %>' TextMode="MultiLine" 
                                        Width="338px"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label_Desc" runat="server" Text='<%# Eval("desc") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Dept Head">
                                <EditItemTemplate>
                                    <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Dept_Head_Edit" runat="server" 
                                        style="font-family: Andalus">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label_Head_Edit" runat="server" Text='<%# Eval("head") %>' 
                                        Visible="False"></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label_Head" runat="server" 
                                        Text='<%# Eval("head") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Dept Members">
                                <ItemTemplate>
                                    <asp:LinkButton ID="Show_Members" runat="server" CommandArgument="<%# Container.DataItemIndex %>" 
                                        oncommand="Show_Members_Command">Show</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle CssClass="sel" />
                        <PagerStyle CssClass="pgr" />
                        <SelectedRowStyle CssClass="sel" />
                    </asp:GridView>
                    <asp:TreeView ID="TreeView1" runat="server">
                    </asp:TreeView>
                </ContentTemplate>
            </asp:UpdatePanel>
    </asp:Panel>
    
        <br />
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
        <asp:Panel ID="Panel_Create_Dept" runat="server" GroupingText="Create Department" 
            style="font-family: Andalus" CssClass="Panel3_Gradient">
            <div class="style2">
                <br />
                &nbsp;Department Name:&nbsp;<asp:TextBox class="form-control" ID="TextBox_Dept_Name" runat="server"  
                    Width="275px" ValidationGroup="Dept"></asp:TextBox>
                <ajaxtoolkit:AutoCompleteExtender ID="TextBox_Dept_Name_AutoCompleteExtender"  ServiceMethod="GetCompletionListRFQ" 
                MinimumPrefixLength="1" onclientitemselected="OnRFQSelected"
                    runat="server" TargetControlID="TextBox_Dept_Name" UseContextKey="True">
                </ajaxtoolkit:AutoCompleteExtender>
                &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    ControlToValidate="TextBox_Dept_Name" Display="Dynamic" 
                    ErrorMessage="* Mandatory" ForeColor="Red" SetFocusOnError="True" 
                    ValidationGroup="Dept"></asp:RequiredFieldValidator>
                <br />
                <br />
                &nbsp;Dept Head:
                <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Dept_Head" runat="server" 
                    style="font-family: Andalus">
                </asp:DropDownList>
                &nbsp;<br />&nbsp;<br />&nbsp;<asp:Label ID="Label_Dept_Descr" runat="server" 
                    Text="Department Description:" Visible="False"></asp:Label>
                &nbsp;<asp:TextBox class="form-control" ID="TextBox_Desc" runat="server"  
                    Width="544px" Height="93px" TextMode="MultiLine" 
                    ValidationGroup="CreateDefect" MaxLength="200"></asp:TextBox>
                <br />
                <br />
                <asp:Button ID="Button_Dept_Add" runat="server" onclick="Button_Dept_Add_Click" 
                    style="font-family: Andalus" Text="Add" ValidationGroup="Dept" />
                <asp:Label ID="Label_Dept_Add_Stat" runat="server" Text="Label" Visible="False"></asp:Label>
                <br />
                &nbsp;<br />
                
            </div>
        </asp:Panel>
        </ContentTemplate>
        </asp:UpdatePanel>   
    </div>
    <div align="center" id="hover" 
        style="position:fixed; right:200px; top:50px; z-index:20; width:40%; height:auto;">
            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <ContentTemplate>
        <asp:Panel ID="Panel_Add_Dept_Members" runat="server" GroupingText="Product Service Details" 
            style="font-family: Andalus; font-size: small; margin-left: 0px; margin-top: 0px;" 
                 Visible="False" CssClass="Panel3_Gradient_Blackish">
                        <asp:Panel ID="Panel3" runat="server">
                            User List:
                            <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Users_Add_Members" runat="server" AutoPostBack="True" 
                                style="font-family: Andalus">
                            </asp:DropDownList>
                            &nbsp;&nbsp; &nbsp;&nbsp;
                            <asp:Button ID="Button_Add_To_Dept" runat="server" 
                                onclick="Button_Add_To_Dept_Click" style="font-family: Andalus" 
                                Text="Add To This Department!" ValidationGroup="Panel2" />
                        </asp:Panel>
                        <asp:Label ID="Label_Add_To_Dept_Status" runat="server" Visible="False"></asp:Label>
                        &nbsp;&nbsp;<br />&nbsp;<asp:GridView ID="GridView_Dept_Members" 
                        runat="server" AllowPaging="True" width="90%" CssClass="mGrid"
                        AlternatingRowStyle-CssClass="alt"  
                        PagerStyle-CssClass="pgr" 
                    AutoGenerateColumns="False" CellPadding="4" 
                    GridLines="None" onpageindexchanging="GridView_Dept_Members_PageIndexChanging" 
                            Visible="False" onrowdeleting="GridView_Dept_Members_RowDeleting">                    
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
                </asp:GridView>
                <br />
                        &nbsp;<asp:hiddenfield id="hdnValueDeptId" 
                            runat="server" />
                        <asp:Button ID="Button_Hide" runat="server" onclick="Button_Hide_Click" 
                            style="font-family: Andalus" Text="Hide" />


                        &nbsp;<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />&nbsp;&nbsp;<br />&nbsp;</asp:Panel>                   

            </ContentTemplate>
            </asp:UpdatePanel>

        </div>
    </form>
</body>
</html>
