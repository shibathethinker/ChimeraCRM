<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportingStructure.aspx.cs" Inherits="OnLine.Pages.Popups.AdminPref.ReportingStructure" %>

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
                <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title">Existing Reporting Structure</h3>
            </div>
            <div class="panel-body">  
    <asp:Panel ID="Panel_Dept" runat="server"  
            style="font-family: Andalus">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            &nbsp;&nbsp;&nbsp;<br />
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="GridView_Dept" runat="server" 
                            AlternatingRowStyle-CssClass="alt" AutoGenerateColumns="False" CellPadding="4" 
                            CssClass="table table-striped table-bordered table-hover tableShadow"  GridLines="None" 
                            onrowcancelingedit="GridView_Dept_RowCancelingEdit" 
                            onrowdatabound="GridView_Dept_RowDataBound" 
                            onrowdeleting="GridView_Dept_RowDeleting" 
                            onrowediting="GridView_Dept_RowEditing" 
                            onrowupdating="GridView_Dept_RowUpdating" PagerStyle-CssClass="pgr" 
                            PageSize="3" SelectedRowStyle-CssClass="sel" Visible="False" width="50%">
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
                                        <asp:Label ID="Label_Head" runat="server" Text='<%# Eval("head") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dept Members">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="Show_Members" runat="server" 
                                            CommandArgument="<%# Container.DataItemIndex %>" 
                                            oncommand="Show_Members_Command">Show</asp:LinkButton>
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
                    </ContentTemplate>
            </asp:UpdatePanel>
    </asp:Panel>
    </div>
    </div>
    
        <br />
    <div align="center">
                    <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title">Define Reporting Structure</h3>
            </div>
            <div class="panel-body">  
    <asp:Panel ID="Panel_Define_Reporting" runat="server"
            style="font-family: Andalus">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    Select User :
                    <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Users" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="DropDownList_Users_SelectedIndexChanged" 
                        style="font-family: Andalus">
                    </asp:DropDownList>
                    &nbsp;Reporting To:
                    <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Reporting_To" runat="server" 
                        AutoPostBack="True" style="font-family: Andalus">
                    </asp:DropDownList>
                    &nbsp;<asp:Button ID="Button_Add" runat="server" onclick="Button_Add_Click" class="btn btn-sm btn-success"
                        style="font-family: Andalus" Text="Add" />
                    <br />
                </ContentTemplate>
            </asp:UpdatePanel>
    </asp:Panel>
    </div>
    </div>
    
    </div>
    
    </div>
    </form>
</body>
</html>
