<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChainMgmt.aspx.cs" Inherits="OnLine.Pages.Popups.AdminPref.ChainMgmt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
    <div align="center">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <asp:Panel ID="Panel_Chain_Det" runat="server" BorderColor="#0066CC" 
            BorderStyle="Ridge" BorderWidth="2px" GroupingText="Create Chain Details" 
            style="font-family: Andalus">
                    &nbsp;&nbsp;<br />
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                        CellPadding="4" ForeColor="#333333" GridLines="None" 
                        style="font-size: medium" AllowPaging="True" PageSize="5" 
                        onrowediting="GridView1_RowEditing" onrowupdating="GridView1_RowUpdating">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:CommandField ShowSelectButton="True" />
                            <asp:CommandField DeleteText="" ShowCancelButton="False" 
                                ShowEditButton="True" />
                            <asp:TemplateField HeaderText="Chain Name">
                                <ItemTemplate>
                                    <asp:Label ID="Label_Chain_Name" runat="server" Text='<%# Eval("ChainName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox class="form-control" ID="TextBox_ChainName" runat="server" 
                                        Text='<%# Eval("ChainName") %>' style="font-family: Andalus"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Address">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton_Addr" runat="server" style="font-size: medium">Show!</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Contact No">
                                <ItemTemplate>
                                    <asp:Label ID="Label_ContactNo" runat="server" Text='<%# Eval("ContactNo") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox class="form-control" ID="TextBox_ContactNo" runat="server" 
                                        Text='<%# Eval("ContactNo") %>' style="font-family: Andalus"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Contact Name">
                                <ItemTemplate>
                                    <asp:Label ID="Label_ContactName" runat="server" 
                                        Text='<%# Eval("ContactName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox class="form-control" ID="TextBox_ContactName" runat="server" 
                                        Text='<%# Eval("ContactName") %>' style="font-family: Andalus"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email Id">
                                <ItemTemplate>
                                    <asp:Label ID="Label_EmailId" runat="server" Text='<%# Eval("EmailId") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox class="form-control" ID="TextBox_EmailId" runat="server" Text='<%# Eval("EmailId") %>' 
                                        style="font-family: Andalus"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Website">
                                <ItemTemplate>
                                    <asp:Label ID="Label_Website" runat="server" Text='<%# Eval("WebSite") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox class="form-control" ID="TextBox_WebSite" runat="server" Text='<%# Eval("WebSite") %>' 
                                        style="font-family: Andalus"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Registration No">
                                <ItemTemplate>
                                    <asp:Label ID="Label_RegstrNo" runat="server" Text='<%# Eval("RegstrNo") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox class="form-control" ID="TextBox_RegstrNo" runat="server" 
                                        Text='<%# Eval("RegstrNo") %>' style="font-family: Andalus"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Base Currency">
                                <ItemTemplate>
                                    <asp:Label ID="Label_Regstr" runat="server" Text='<%# Eval("RegstrNo") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label_BaseCurr" runat="server" Text='<%# Eval("BaseCurr") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_BaseCurr" runat="server" 
                                        style="font-family: Andalus">
                                    </asp:DropDownList>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Hidden">
                                <ItemTemplate>
                                    <asp:Label ID="Label_Hidden" runat="server" Text='<%# Eval("ChainId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#990000" Font-Bold="True" Font-Size="Medium" 
                            ForeColor="White" />
                        <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                        <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                        <SortedAscendingCellStyle BackColor="#FDF5AC" />
                        <SortedAscendingHeaderStyle BackColor="#4D0000" />
                        <SortedDescendingCellStyle BackColor="#FCF6C0" />
                        <SortedDescendingHeaderStyle BackColor="#820000" />
                    </asp:GridView>
                    &nbsp;<br />
                    <asp:Label ID="Label_Update_Stat" runat="server" Visible="False"></asp:Label>
                    <br />
        </asp:Panel>
                </ContentTemplate>
        </asp:UpdatePanel>     
            <br />
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
                    <asp:Panel ID="Panel_Insert_Chain" runat="server" BorderColor="#0066CC" 
                BorderStyle="Ridge" BorderWidth="2px" GroupingText="Manage Chain Details" 
                style="font-family: Andalus">
                &nbsp;&nbsp;<asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="Panel2" runat="server" BorderColor="#0066CC" BorderStyle="Ridge" 
                                    BorderWidth="2px" GroupingText="Enter details of all your chains" 
                                    Height="328px" style="font-family: Andalus">
                                    &nbsp;Chain name:
                                    <asp:TextBox class="form-control" ID="TextBox6" runat="server"  Width="265px"></asp:TextBox>
                                    &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                        ControlToValidate="TextBox6" Display="Dynamic" ErrorMessage="*" ForeColor="Red" 
                                        ValidationGroup="Panel2"></asp:RequiredFieldValidator>
                                    &nbsp;Registration number:
                                    <asp:TextBox class="form-control" ID="TextBox7" runat="server"  Width="268px"></asp:TextBox>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Contact Name:
                                    <asp:TextBox class="form-control" ID="TextBox10" runat="server"  Width="191px"></asp:TextBox>
                                    &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                        ControlToValidate="TextBox10" Display="Dynamic" ErrorMessage="*" 
                                        ForeColor="Red" ValidationGroup="Panel2"></asp:RequiredFieldValidator>
                                    &nbsp;Contact No:
                                    <asp:TextBox class="form-control" ID="TextBox11" runat="server"  Width="191px"></asp:TextBox>
                                    &nbsp;&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                                        ControlToValidate="TextBox11" Display="Dynamic" ErrorMessage="*" 
                                        ForeColor="Red" ValidationGroup="Panel2"></asp:RequiredFieldValidator>
                                    &nbsp;Email Id:
                                    <asp:TextBox class="form-control" ID="TextBox12" runat="server"  Width="191px"></asp:TextBox>
                                    &nbsp;<br /> If this chain has a separate web site:
                                    <asp:TextBox class="form-control" ID="TextBox13" runat="server"  Width="191px"></asp:TextBox>
                                    &nbsp;Base currency?
                                    <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownListBaseCurr" runat="server" 
                                        style="font-family: Andalus">
                                    </asp:DropDownList>
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:Panel ID="Panel3" runat="server" BorderColor="#0066CC" BorderStyle="Ridge" 
                                                BorderWidth="2px" GroupingText="Enter chain address" 
                                                style="font-family: Andalus">
                                                Country:
                                                <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList2" runat="server" AutoPostBack="True" 
                                                    onselectedindexchanged="DropDownList2_SelectedIndexChanged" 
                                                    ontextchanged="DropDownList2_TextChanged" style="font-family: Andalus">
                                                </asp:DropDownList>
                                                &nbsp;State/Province:
                                                <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList3" runat="server" AutoPostBack="True" 
                                                    onselectedindexchanged="DropDownList3_SelectedIndexChanged" 
                                                    style="font-family: Andalus">
                                                </asp:DropDownList>
                                                &nbsp;City:
                                                <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList4" runat="server" AutoPostBack="True" 
                                                    onselectedindexchanged="DropDownList4_SelectedIndexChanged" 
                                                    style="font-family: Andalus">
                                                </asp:DropDownList>
                                                &nbsp;Locality:
                                                <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList5" runat="server" 
                                                    style="font-family: Andalus">
                                                </asp:DropDownList>
                                                &nbsp;Street Name:
                                                <asp:TextBox class="form-control" ID="TextBox8" runat="server"  Width="169px"></asp:TextBox>
                                            </asp:Panel>
                                            <br />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <br />
                                    <div align="center">
                                        <asp:Button ID="Button_Register_Business" runat="server" 
                                            onclick="Button1_Click" style="font-family: Andalus" Text="Done!" 
                                            ValidationGroup="Panel2" />
                                        &nbsp;
                                        <asp:Button ID="Button_next" runat="server" onclick="ButtonNext_Click" 
                                            style="font-family: Andalus" Text="Submit and Enter Next!" 
                                            ValidationGroup="Panel2" />
                                        <br />
                                        <asp:Label ID="Label1" runat="server" Text="Label" Visible="False"></asp:Label>
                                        <br />
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <br /> &nbsp;<br />
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
                                <asp:Panel ID="Panel1" runat="server" BorderColor="#0066CC" BorderStyle="Ridge" 
                                    BorderWidth="2px" CssClass="legend" 
                                    GroupingText="Create user accounts for your chains" Height="269px" 
                                    style="font-family: Andalus">
                                    <br />
                                    User Id:
                                    <asp:TextBox class="form-control" ID="TextBox1" runat="server"  Width="227px"></asp:TextBox>
                                    &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                                        ControlToValidate="TextBox1" Display="Dynamic" ErrorMessage="*" ForeColor="Red" 
                                        ValidationGroup="Panel1"></asp:RequiredFieldValidator>
                                    &nbsp; Passsword:
                                    <asp:TextBox class="form-control" ID="TextBox2" runat="server"  TextMode="Password" 
                                        Width="219px"></asp:TextBox>
                                    &nbsp;
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                                        ControlToValidate="TextBox2" Display="Dynamic" ErrorMessage="*" ForeColor="Red" 
                                        ValidationGroup="Panel1"></asp:RequiredFieldValidator>
                                    <br />
                                    Confirm Password:
                                    <asp:TextBox class="form-control" ID="TextBox3" runat="server"  TextMode="Password" 
                                        Width="258px"></asp:TextBox>
                                    &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" 
                                        ControlToValidate="TextBox3" Display="Dynamic" ErrorMessage="*" ForeColor="Red" 
                                        ValidationGroup="Panel1"></asp:RequiredFieldValidator>
                                    &nbsp;
                                    <asp:CompareValidator ID="CompareValidator1" runat="server" 
                                        ControlToCompare="TextBox2" ControlToValidate="TextBox3" Display="Dynamic" 
                                        ErrorMessage="CompareValidator" ForeColor="Red">Password mismatch</asp:CompareValidator>
                                    <br />
                                    User Name:
                                    <asp:TextBox class="form-control" ID="TextBox_User_Name_NewAccount" runat="server"  
                                        Width="227px"></asp:TextBox>
                                    &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" 
                                        ControlToValidate="TextBox_User_Name_NewAccount" Display="Dynamic" 
                                        ErrorMessage="*" ForeColor="Red" ValidationGroup="Panel1"></asp:RequiredFieldValidator>
                                    <br />
                                    Name of the chain this user account will belong to (Or you can do it later/leave 
                                    it blank):&nbsp;<asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList1" runat="server" 
                                                onselectedindexchanged="DropDownList1_SelectedIndexChanged" 
                                                style="font-family: Andalus">
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <br />
                                    <div align="center">
                                        &nbsp;
                                        <asp:Button ID="Create_Chain_User" runat="server" 
                                            onclick="Create_Chain_User_Click" 
                                            style="text-align: center; font-family: Andalus" Text="Submit!" 
                                            ValidationGroup="Panel1" />
                                        <br />
                                        <asp:Label ID="Label2" runat="server" Text="Label" Visible="False"></asp:Label>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                <br />
            </asp:Panel>    
        </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
