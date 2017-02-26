<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocFormatManagement.aspx.cs" Inherits="OnLine.Pages.Popups.AdminPref.DocFormatManagement" %>

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
              <h3 class="panel-title">Purchase Order Section Details</h3>
            </div>
            <div class="panel-body">  
                <asp:Panel ID="Panel_PO" runat="server" 
            style="font-family: Andalus">
                    Format Name:
                    <asp:TextBox class="form-control" ID="TextBox_PO_format_name" runat="server" Height="26px" 
                        Width="20%" style="font-family: Andalus" ValidationGroup="PO_validate"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ControlToValidate="TextBox_PO_format_name" ErrorMessage="*" ForeColor="Red" 
                        SetFocusOnError="True" ValidationGroup="PO_validate"></asp:RequiredFieldValidator>
                    <br />
                    <br />
                    Terms and Conditions:
                    <asp:TextBox class="form-control" ID="TextBox_PO_TnC" runat="server"  
                        Width="50%" Height="50%" TextMode="MultiLine"></asp:TextBox>
&nbsp;
                    <asp:Label ID="Label_TNC_flag_PO" runat="server" Visible="False">Y</asp:Label>
                    <br />
                    <br />
                    <asp:Button ID="Button_Update_PO" runat="server" onclick="Button_Update_PO_Click" class="btn btn-sm btn-success"
                        style="font-family: Andalus" Text="Update!" 
                        ValidationGroup="PO_validate" />
                    &nbsp;<asp:Label ID="Label_PO_update_stat" runat="server"></asp:Label>
                    <br />
                    <br />
                    <br /><br />
                    </asp:Panel>
                    </div>
                    </div>
        </ContentTemplate>
        </asp:UpdatePanel>                
                    <br />
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
                        <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title">Invoice Section Details</h3>
            </div>
            <div class="panel-body">  
                        <asp:Panel ID="Panel_Invoice" runat="server" 
            style="font-family: Andalus">
                            <br />
                            Format Name:
                            <asp:TextBox class="form-control" ID="TextBox_INV_format_name" runat="server" Height="26px" 
                                Width="20%" style="font-family: Andalus" ValidationGroup="INV_validate"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                ControlToValidate="TextBox_INV_format_name" ErrorMessage="*" ForeColor="Red" 
                                SetFocusOnError="True" ValidationGroup="INV_validate"></asp:RequiredFieldValidator>
                            <br />
                            <br />
                    Terms and Conditions:
                    <asp:TextBox class="form-control" ID="TextBox_INV_TnC" runat="server"  
                        Width="50%" Height="50%" TextMode="MultiLine"></asp:TextBox>
&nbsp;
                            <asp:Label ID="Label_TNC_flag_Inv" runat="server" Text="Y" Visible="False"></asp:Label>
                            <br />
                            <br />
            <div class="panel-info" Width="90%">
            <div class="panel-heading">
            <h3 class="panel-title">Tax Components</h3>
            </div>
            <div class="panel-body">   
                            <asp:Panel ID="Panel_Inv_Tax" runat="server"
                                style="font-family: Andalus; font-size: small" >
                                <asp:Label ID="Label1" runat="server" Text="Tax Component name:"></asp:Label>
                                &nbsp;<asp:TextBox class="form-control" ID="TextBox_Inv_Tax_Comp_Name" runat="server"  
                                    Width="20%"></asp:TextBox>
                                &nbsp;<asp:Label ID="Label2" runat="server" Text="Tax Component value (% of total):"></asp:Label>
                                &nbsp;<asp:TextBox class="form-control" ID="TextBox_Inv_Tax_Comp_Value" runat="server"  
                                    Width="20%"></asp:TextBox>
                                <br />
                                <br />
&nbsp;<br />
                                <br />
                                <asp:Label ID="Label_Inv_Tax_Comp_Ext_List" runat="server" 
                                    Text="Already Defined Tax Components:"></asp:Label>
                                <br />
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                <ContentTemplate>
                                <asp:GridView ID="GridView_Inv_Tax_Comp" runat="server" AllowPaging="True" Width="60%"
                                    AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" CssClass="table table-striped table-bordered table-hover tableShadow" 
                                    GridLines="None" onpageindexchanging="GridView_Inv_Tax_Comp_PageIndexChanging" 
                                    onrowdatabound="GridView_Inv_Tax_Comp_RowDataBound" PageSize="3" 
                                        Visible="False" onrowcancelingedit="GridView_Inv_Tax_Comp_RowCancelingEdit" 
                                        onrowdeleting="GridView_Inv_Tax_Comp_RowDeleting" 
                                        onrowediting="GridView_Inv_Tax_Comp_RowEditing" 
                                        onrowupdating="GridView_Inv_Tax_Comp_RowUpdating">

                                    <Columns>
                                        <asp:CommandField ShowDeleteButton="True" />
                                        <asp:CommandField ShowEditButton="True" />
                                        <asp:TemplateField HeaderText="Hidden">
                                            <ItemTemplate>
                                                <asp:Label ID="Label_Hidden" runat="server" Text='<%# Eval("Hidden") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Component Name">
                                            <ItemTemplate>
                                                <asp:Label ID="Label_Name" runat="server" Text='<%# Eval("Comp_Name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Value">
                                            <ItemTemplate>
                                                <asp:Label ID="Label_Value" runat="server" Text='<%# Eval("Comp_Value") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox class="form-control" ID="TextBox_Value_Edit" runat="server" 
                                                    style="font-family: Andalus" Text='<%# Eval("Comp_Value") %>'></asp:TextBox>
                                            </EditItemTemplate>
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
                                
                                <br />
                            </asp:Panel>
                            </div>
                            </div>
                    <asp:Button ID="Button_Inv_Update" runat="server" 
class="btn btn-sm btn-success"
                        style="font-family: Andalus" Text="Update!" onclick="Button_Inv_Update_Click" 
                                ValidationGroup="INV_validate" />
                    &nbsp;<asp:Label ID="Label_Inv_Update_stat" runat="server"></asp:Label>
                    <br />
                            <br /><br />
                            </asp:Panel>
                            </div>
                            </div>
        </ContentTemplate>
        </asp:UpdatePanel>
                    <br />
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
                                      <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title">Sales Order Details</h3>
            </div>
            <div class="panel-body">
         <asp:Panel ID="Panel_Sales_Order" runat="server"  
            style="font-family: Andalus">
                    <br />
                    Format Name:
                    <asp:TextBox class="form-control" ID="TextBox_SO_format_name" runat="server" Height="26px" 
                        Width="20%" style="font-family: Andalus" ValidationGroup="SO_Validate"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                        ControlToValidate="TextBox_SO_format_name" ErrorMessage="*" ForeColor="Red" 
                        SetFocusOnError="True" ValidationGroup="SO_Validate"></asp:RequiredFieldValidator>
                    <br />
                    <br />
                    Terms and Conditions:
                    <asp:TextBox class="form-control" ID="TextBox_SO_TnC" runat="server"  
                        Width="50%" Height="50%" TextMode="MultiLine"></asp:TextBox>
&nbsp;
                    <asp:Label ID="Label_TNC_flag_SO" runat="server" Visible="False">Y</asp:Label>
                    <br />
                    <br />
                    <asp:Button ID="Button_SO_Update" runat="server" onclick="Button_SO_Update_Click" 
class="btn btn-sm btn-success"
                        style="font-family: Andalus" Text="Update!" 
                        ValidationGroup="SO_Validate" />
                    &nbsp;<asp:Label ID="Label_SO_update_stat" runat="server"></asp:Label>
                    <br />
                    <br />
                    &nbsp;&nbsp;<br />&nbsp; <br />
                    &nbsp;</asp:Panel>
                    </div>
                    </div>
        </ContentTemplate>
        </asp:UpdatePanel>               
    <br />   
    </div>
    </form>
</body>
</html>
