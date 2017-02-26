<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExistingGroupAccess.aspx.cs" Inherits="OnLine.Pages.Popups.AdminPref.ExistingGroupAccess" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
            <link type="text/css" rel="stylesheet" href="~/css/bootstrap.css" /> 
    <style type="text/css">

legend {color:red}
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <br />
    <div align="center">
                              <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title">Create New Security Group</h3>
            </div>
            <div class="panel-body">
        <asp:Panel ID="Panel_New_LDAP" runat="server" 
            style="font-family: Andalus; font-weight: 700;">
                    <asp:Button ID="Button_Submit" runat="server" class="btn btn-sm btn-success"
                        onclick="Button_Submit_Click" style="font-family: Andalus" 
                        Text="Update Group!" />
                    <br />
                    <asp:Label ID="Label_Stat" runat="server" Visible="False" 
                        style="font-size: small"></asp:Label>
                    <br />
                    <asp:CheckBox ID="CheckBox_Owner_Access" runat="server" ForeColor="#009933" 
                        oncheckedchanged="CheckBox_Owner_Access_CheckedChanged" 
                        style="font-size: small; font-weight: 700" 
                        Text="Owner Access (Complete Access to All Features)" />
                    <br />
                    OR<br />
        
            <div class="panel-info" Width="95%">
            <div class="panel-heading">
            <h3 class="panel-title">Purchase Screen Access</h3>
            </div>
            <div class="panel-body">    
                    <asp:Panel ID="Panel_Purchase" runat="server" 
                        BorderStyle="None"
                        style="font-family: Andalus" Width="95%">
                        <asp:GridView ID="GridView_Purchase" runat="server" AutoGenerateColumns="False" 
                            CellPadding="4" Font-Size="Medium" ForeColor="#333333" GridLines="Vertical"
                            PageSize="1" ShowHeaderWhenEmpty="True" 
                            style="font-size: xx-small" >
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:TemplateField HeaderText="Purchase Screen View">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox1" runat="server" />
                                    </ItemTemplate>
                                    <HeaderStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="View Requirement">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox2" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Create Requirement">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox3" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit Requirement">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox4" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Convert Requirement">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox5" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="View RFQ">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox6" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Create RFQ">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox7" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit RFQ">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox8" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="View Invoice">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox9" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="View PO">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox10" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit PO">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox11" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Create PO">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox12" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BackColor="#999999" />
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="Small" 
                                ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#F7F6F3" Font-Size="Small" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                        </asp:GridView>
                        <br />
                    </asp:Panel>
                    </div>
                    </div>
                    <br />
                <div class="panel-info" Width="95%">
             <div class="panel-heading">
            <h3 class="panel-title">Sales Screen Access</h3>
            </div>
            <div class="panel-body">    
                    <asp:Panel ID="Panel_Sales" runat="server" 
                        BorderStyle="None" style="font-family: Andalus" Width="95%">
                        <br />
                        <asp:GridView ID="Gridview_Sales" runat="server" AutoGenerateColumns="False" 
                            CellPadding="4" Font-Size="Medium" ForeColor="#333333" GridLines="Vertical" 
                             PageSize="1" ShowHeaderWhenEmpty="True" 
                            style="font-size: xx-small">
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:TemplateField HeaderText="Sales Screen View">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox1" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="View Lead">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox2" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Create Lead">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox3" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit Lead">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox4" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Convert Lead">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox5" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="View Potential">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox6" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Create Potential">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox7" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit Potential">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox8" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="View Invoice">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox9" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Create Invoice">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox10" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit Invoice">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox11" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="View SO">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox12" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit SO">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox13" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Create SO">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox14" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BackColor="#999999" />
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="Small" 
                                ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#F7F6F3" Font-Size="Small" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                        </asp:GridView>
                        <br />
                    </asp:Panel>
                    </div>
                    </div>
                    <br />
            <div class="panel-info" Width="95%">
            <div class="panel-heading">
            <h3 class="panel-title">Products Screen Access</h3>
            </div>
            <div class="panel-body">    
                    <asp:Panel ID="Panel_Products" runat="server" 
                        BorderStyle="None" 
                        style="font-family: Andalus" Width="95%">
                        <br />
                        <asp:GridView ID="GridView_Product" runat="server" AutoGenerateColumns="False" 
                            CellPadding="4" Font-Size="Medium" ForeColor="#333333" GridLines="Vertical" 
                            PageSize="1" ShowHeaderWhenEmpty="True" 
                            style="font-size: xx-small" >
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:TemplateField HeaderText="Products Screen View">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox1" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Create Product">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox2" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit Product">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox3" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BackColor="#999999" />
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="Small" 
                                ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#F7F6F3" Font-Size="Small" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                        </asp:GridView>
                    </asp:Panel>
                    </div>
                    </div>
                    <br />
            <div class="panel-info" Width="95%">
            <div class="panel-heading">
            <h3 class="panel-title">Defects Screen Access</h3>
            </div>
            <div class="panel-body">    
                    <asp:Panel ID="Panel_Defects" runat="server" 
                        BorderStyle="None" style="font-family: Andalus" Width="95%">
                        <br />
                        <asp:GridView ID="GridView_Defect" runat="server" AutoGenerateColumns="False" 
                            CellPadding="4" Font-Size="Medium" ForeColor="#333333" GridLines="Vertical" 
                             PageSize="1" ShowHeaderWhenEmpty="True" 
                            style="font-size: xx-small" >
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:TemplateField HeaderText="Defects Screen View">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox1" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Create Incoming Defect">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox2" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit Incoming Defect">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox3" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Create Outgoing Defect">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox4" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit Outgoing Defect">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox5" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BackColor="#999999" />
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="Small" 
                                ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#F7F6F3" Font-Size="Small" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                        </asp:GridView>
                        &nbsp;<br />
                    </asp:Panel>
                    </div>
                    </div>
                    <br />
                    <div class="panel-info" Width="95%">
            <div class="panel-heading">
            <h3 class="panel-title">Service Request Screen Access</h3>
            </div>
            <div class="panel-body">    
                    <asp:Panel ID="Panel1" runat="server" 
                        BorderStyle="None" style="font-family: Andalus" Width="95%">
                        <br />
                        <asp:GridView ID="GridView_SR" runat="server" AutoGenerateColumns="False" 
                            CellPadding="4" Font-Size="Medium" ForeColor="#333333" GridLines="Vertical" 
                             PageSize="1" ShowHeaderWhenEmpty="True" 
                            style="font-size: xx-small" >
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:TemplateField HeaderText="SR Screen View">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox1" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Create Incoming SR">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox2" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit Incoming SR">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox3" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Create Outgoing SR">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox4" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit Outgoing SR">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox5" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BackColor="#999999" />
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="Small" 
                                ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#F7F6F3" Font-Size="Small" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                        </asp:GridView>
                        &nbsp;<br />
                    </asp:Panel>
                    </div>
                    </div>
                    <br />
            <div class="panel-info" Width="95%">
            <div class="panel-heading">
            <h3 class="panel-title">Contacts Screen Access</h3>
            </div>
            <div class="panel-body">    
                    <asp:Panel ID="Panel_Contacts" runat="server" 
                        style="font-family: Andalus" Width="95%">
                        <asp:GridView ID="GridView_Contacts" runat="server" AutoGenerateColumns="False" 
                            CellPadding="4" Font-Size="Medium" ForeColor="#333333" GridLines="Vertical" 
                             PageSize="1" ShowHeaderWhenEmpty="True" 
                            style="font-size: xx-small" >
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:TemplateField HeaderText="Contacts Screen View">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox1" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Create Contacts">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox2" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit Contacts">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox3" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BackColor="#999999" />
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="Small" 
                                ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#F7F6F3" Font-Size="Small" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                        </asp:GridView>
                        &nbsp;<br />
                    </asp:Panel>
                    </div>
                    </div>
                    <br />
                            <div class="panel-info" Width="95%">
        <div class="panel-heading">
            <h3 class="panel-title">Dashboard Screen Access</h3>
            </div>
            <div class="panel-body"> 
                    <asp:Panel ID="Panel_Dashboard" runat="server" 
                        BorderStyle="None" style="font-family: Andalus">
                        <br />
                        <asp:GridView ID="GridView_Dashboard" runat="server" 
                            AutoGenerateColumns="False" CellPadding="4" Font-Size="Medium" 
                            ForeColor="#333333" GridLines="Vertical"  PageSize="1" 
                            ShowHeaderWhenEmpty="True" style="font-size: xx-small" >
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:TemplateField HeaderText="Dashboard Page View">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox1" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="View Lead Report">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox2" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Download Lead Report">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox3" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="View Potential Report">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox4" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Download Potential Report">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox5" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="View Sales Transac Report">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox6" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Download Sales Transac Report">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox7" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="View Purchase Transac Report">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox8" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Download Purchase Transac Report">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox9" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="View Incoming Defects Report">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox10" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Download Incoming Defects Report">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox11" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="View Outgoing Defects Report">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox12" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Download Outgoing Defects Report">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox13" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="View Custom Report">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox14" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Download Custom Report">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox15" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BackColor="#999999" />
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="Small" 
                                ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#F7F6F3" Font-Size="Small" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                        </asp:GridView>
                        &nbsp;<br />
                    </asp:Panel>
                    </div>
                    </div>
                    <br />
                                                <div class="panel-info" Width="95%">
        <div class="panel-heading">
            <h3 class="panel-title">Admin Screen Access</h3>
            </div>
            <div class="panel-body"> 
                    <asp:Panel ID="Panel_Admin" runat="server" 
                        BorderStyle="None" style="font-family: Andalus" Width="95%">
                        <br />
                        <asp:GridView ID="GridView_Admin" runat="server" AutoGenerateColumns="False" 
                            CellPadding="4" Font-Size="Medium" ForeColor="#333333" GridLines="Vertical" 
                             PageSize="1" ShowHeaderWhenEmpty="True" 
                            style="font-size: xx-small" >
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:TemplateField HeaderText="Admin Pref Page View">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox1" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Admin Pref - Entity Mgmt Write Access">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox2" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Admin Pref - Security Mgmt Write Access">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox3" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Admin Pref - User Mgmt Write Access">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox4" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Admin Pref - Workflow Mgmt Write Access">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox5" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BackColor="#999999" />
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" Font-Size="Small" 
                                ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#F7F6F3" Font-Size="Small" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                        </asp:GridView>
                    </asp:Panel>
                    </div>
                    </div>
                    <br />
                </asp:Panel>
                </div>
                </div>
    
    </div>
    </form>
</body>
</html>
