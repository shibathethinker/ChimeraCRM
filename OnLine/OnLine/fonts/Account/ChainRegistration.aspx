<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChainRegistration.aspx.cs" Inherits="OnLine.Pages.ChainRegistration" %>

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
    <div>
    
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <asp:Panel ID="Panel2" runat="server" BorderColor="#0066CC" BorderStyle="Ridge" 
        BorderWidth="2px" GroupingText="Enter details of all your chains" 
        style="font-family: Andalus" Height="328px">
        &nbsp;Chain name:
        <asp:TextBox ID="TextBox6" runat="server" Width="265px" CssClass="style1"></asp:TextBox>
            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                ControlToValidate="TextBox6" Display="Dynamic" ErrorMessage="*" ForeColor="Red" 
                ValidationGroup="Panel2"></asp:RequiredFieldValidator>
            &nbsp;Registration number:
        <asp:TextBox ID="TextBox7" runat="server" Width="268px" CssClass="style1"></asp:TextBox>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Contact Name:
        <asp:TextBox ID="TextBox10" runat="server" CssClass="style1" Width="191px"></asp:TextBox>
            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                ControlToValidate="TextBox10" Display="Dynamic" ErrorMessage="*" 
                ForeColor="Red" ValidationGroup="Panel2"></asp:RequiredFieldValidator>
            &nbsp;Contact No:
        <asp:TextBox ID="TextBox11" runat="server" CssClass="style1" Width="191px"></asp:TextBox>
            &nbsp;&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                ControlToValidate="TextBox11" Display="Dynamic" ErrorMessage="*" 
                ForeColor="Red" ValidationGroup="Panel2"></asp:RequiredFieldValidator>
            &nbsp;Email Id:
        <asp:TextBox ID="TextBox12" runat="server" CssClass="style1" Width="191px"></asp:TextBox>
        &nbsp;<br />
        If this chain has a separate web site:
        <asp:TextBox ID="TextBox13" runat="server" CssClass="style1" Width="191px"></asp:TextBox>
            &nbsp;Base currency?
            <asp:DropDownList ID="DropDownListBaseCurr" runat="server" 
                style="font-family: Andalus">
            </asp:DropDownList>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel3" runat="server" BorderColor="#0066CC" BorderStyle="Ridge" 
                    BorderWidth="2px" GroupingText="Enter chain address" 
                    style="font-family: Andalus">
                    Country:
                    <asp:DropDownList ID="DropDownList2" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="DropDownList2_SelectedIndexChanged" 
                        ontextchanged="DropDownList2_TextChanged" style="font-family: Andalus">
                    </asp:DropDownList>
                    &nbsp;State/Province:
                    <asp:DropDownList ID="DropDownList3" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="DropDownList3_SelectedIndexChanged" 
                        style="font-family: Andalus">
                    </asp:DropDownList>
                    &nbsp;City:
                    <asp:DropDownList ID="DropDownList4" runat="server" AutoPostBack="True" 
                        onselectedindexchanged="DropDownList4_SelectedIndexChanged" 
                        style="font-family: Andalus">
                    </asp:DropDownList>
                    &nbsp;Locality:
                    <asp:DropDownList ID="DropDownList5" runat="server" 
                        style="font-family: Andalus">
                    </asp:DropDownList>
                    &nbsp;Street Name:
                    <asp:TextBox ID="TextBox8" runat="server" CssClass="style1" Width="169px"></asp:TextBox>
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
                    <br />
            <br />
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
            <asp:Panel ID="Panel1" runat="server" BorderColor="#0066CC" BorderStyle="Ridge" 
                BorderWidth="2px" CssClass="legend" 
                GroupingText="Create user accounts for your chains" Height="269px" 
                style="font-family: Andalus">
                <br />
                User Name:
                <asp:TextBox ID="TextBox1" runat="server" CssClass="style1" Width="227px"></asp:TextBox>
                &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                    ControlToValidate="TextBox1" Display="Dynamic" ErrorMessage="*" ForeColor="Red" 
                    ValidationGroup="Panel1"></asp:RequiredFieldValidator>
                &nbsp; Passsword:
                <asp:TextBox ID="TextBox2" TextMode="Password" runat="server" CssClass="style1" Width="219px"></asp:TextBox>
                &nbsp;
                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                    ControlToValidate="TextBox2" Display="Dynamic" ErrorMessage="*" ForeColor="Red" 
                    ValidationGroup="Panel1"></asp:RequiredFieldValidator>
                <br />
                Confirm Password:
                <asp:TextBox ID="TextBox3" TextMode="Password" runat="server" CssClass="style1" Width="258px"></asp:TextBox>
                &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" 
                    ControlToValidate="TextBox3" Display="Dynamic" ErrorMessage="*" ForeColor="Red" 
                    ValidationGroup="Panel1"></asp:RequiredFieldValidator>
                &nbsp;
                <asp:CompareValidator ID="CompareValidator1" runat="server" 
                    ControlToCompare="TextBox2" ControlToValidate="TextBox3" Display="Dynamic" 
                    ErrorMessage="CompareValidator" ForeColor="Red">Password mismatch</asp:CompareValidator>
                <br />
                Name of the chain this user account will belong to (Or you can do it later/leave 
                it blank):&nbsp;<asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>
                <asp:DropDownList ID="DropDownList1" runat="server" 
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
    </div>
    </form>
</body>
</html>
