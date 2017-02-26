<%@ Page Title="" Language="C#" Theme="ThemeBlue" MasterPageFile="~/Pages/Site1.Master" AutoEventWireup="true" CodeBehind="UserPref.aspx.cs" Inherits="OnLine.Pages.UserPref" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">

legend {color:red}
        .style7
        {
            font-size: small;
        }
        .style9
        {
            margin-top: 0;
            margin-bottom: 0;
            font-size: 16px;
            color: inherit;
            font-family: Andalus;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHeader" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderBody" runat="server">
    <div align="center">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
              <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="style9">Change Password</h3>
            </div>
            <div class="panel-body">  
    <asp:Panel ID="Panel_Change_Pass" runat="server" 
            style="font-family: Andalus">
            Type New Password:
            <asp:TextBox class="form-control" ID="TextBox_Pass1" runat="server"  
                Width="20%" TextMode="Password" ValidationGroup="changePass"></asp:TextBox>
            &nbsp;<br /> <br /> Re-type New Password:&nbsp;<asp:TextBox class="form-control" ID="TextBox_Pass2" runat="server"  
                Width="20%" TextMode="Password" ValidationGroup="changePass"></asp:TextBox>
            &nbsp;<br />
            <asp:CompareValidator ID="CompareValidator1" runat="server" 
                ControlToCompare="TextBox_Pass1" ControlToValidate="TextBox_Pass2" 
                Display="Dynamic" ErrorMessage="Password Mismatch" ForeColor="#FF3300"></asp:CompareValidator>
            <br />
            <asp:Button ID="Button_Change_Password" runat="server" class="btn btn-sm btn-success"
                onclick="Button_Submit_Req_Click" style="font-family: Andalus" Text="Submit!" 
                ValidationGroup="changePass" />
            &nbsp;<br />
            <asp:Label ID="Label_Pass_Change_Stat" runat="server" Visible="False"></asp:Label>
        </asp:Panel>
        </div>
        </div>
    </ContentTemplate>
    </asp:UpdatePanel>
        
</div>
           <br />
            <div align="center">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>     
                              <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="style9">Personal Details</h3>
            </div>
            <div class="panel-body">  
                <asp:Panel ID="Panel_User_Det" runat="server"  
            style="font-family: Andalus">
                    Name:
            <asp:TextBox class="form-control" ID="TextBox_Name" runat="server"  
                Width="20%"></asp:TextBox>
                    &nbsp; Email Id:
                    <asp:TextBox class="form-control" ID="TextBox_Email" runat="server"  Width="20%"></asp:TextBox>
                    &nbsp;
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                        ControlToValidate="TextBox_Email" Display="Dynamic" 
                        ErrorMessage="Invalid email id" ForeColor="Red" 
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                        ValidationGroup="changePersonal"></asp:RegularExpressionValidator>
                    <br />
                    <br />
                    Contact No:&nbsp;
                    <asp:TextBox class="form-control" ID="TextBox_Contact" runat="server"  
                        Width="20%" MaxLength="14"></asp:TextBox>
                    &nbsp;
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                        ControlToValidate="TextBox_Contact" Display="Dynamic" 
                        ErrorMessage="Please enter valid contact number" ForeColor="Red" 
                        ValidationExpression="^\d{10,14}$" ValidationGroup="changePersonal"></asp:RegularExpressionValidator>
                    &nbsp;<br />&nbsp;&nbsp;<br />
            <asp:Button ID="Button_Change_Details" runat="server" class="btn btn-sm btn-success"
                onclick="Button_Change_Details_Click" style="font-family: Andalus" Text="Submit!" 
                ValidationGroup="changePersonal" />
                    &nbsp;<br />
            <asp:Label ID="Label_Det_Change_Stat" runat="server" Visible="False"></asp:Label>
        </asp:Panel>
        </div>
        </div>
                    <br />
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                    <asp:Panel ID="Panel_User_Theme" runat="server" BorderColor="#0066CC" Visible=false
                        BorderStyle="Ridge" BorderWidth="2px" GroupingText="Theme Preference" 
                        style="font-family: Andalus">
                        <asp:Label ID="Label_Curren_Theme" runat="server" Font-Size="Medium" 
                            Text="Current Theme:"></asp:Label>
&nbsp;<asp:Label ID="Label_Curren_Theme_Name" runat="server" Font-Size="Medium"></asp:Label>
                        <br />
                        &nbsp;&nbsp;<br />
                        <asp:Label ID="Label_Select_Theme" runat="server" Font-Size="Medium" 
                            Text="Select Theme:"></asp:Label>
                        &nbsp;<asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Theme" runat="server" 
                            style="font-family: Andalus">
                        </asp:DropDownList>
                        <br />
                        <br />
                        <asp:Button ID="Button_Change_Theme" runat="server" 
                            onclick="Button_Change_Theme_Click" style="font-family: Andalus" 
                            Text="Submit!" />
                        &nbsp;<br />
                        <asp:Label ID="Label_Theme_Change_Stat" runat="server" Visible="False"></asp:Label>
                    </asp:Panel>
                                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                    <asp:Panel ID="Panel_Chain_Det" runat="server" Visible=false
                        style="font-family: Andalus">
                        Name:
                        <asp:Label ID="Label_Chain_Name" runat="server" Font-Size="Medium" Text="N/A"></asp:Label>
                        &nbsp; Email Id:
                        <asp:Label ID="Label_Chain_Email" runat="server" Font-Size="Medium" Text="N/A"></asp:Label>
                        &nbsp;
                        <br />
                        Contact No:&nbsp;
                        <asp:Label ID="Label_Chain_Contact" runat="server" Font-Size="Medium" 
                            Text="N/A"></asp:Label>
                        &nbsp; Registration No:
                        <asp:Label ID="Label_Chain_Regstr" runat="server" Font-Size="Medium" Text="N/A"></asp:Label>
                        &nbsp;<br />
                        <br />
                        Address Line1:
                        <asp:Label ID="Label_Addr1" runat="server" Font-Size="Medium" Text="N/A"></asp:Label>
                        <br />
                        <asp:Label ID="Label_1" runat="server" Font-Size="Medium" Text="Country:"></asp:Label>
                        <asp:Label ID="Label_Country" runat="server" Font-Size="Medium" Text="N/A"></asp:Label>
                        <asp:Label ID="Label_2" runat="server" Font-Size="Medium" Text="State:"></asp:Label>
                        <asp:Label ID="Label_State" runat="server" Font-Size="Medium" Text="N/A"></asp:Label>
                        <asp:Label ID="Label_3" runat="server" Font-Size="Medium" Text="City:"></asp:Label>
                        <asp:Label ID="Label_City" runat="server" Font-Size="Medium" Text="N/A"></asp:Label>
                        <asp:Label ID="Label_4" runat="server" Font-Size="Medium" Text="Locality:"></asp:Label>
                        <asp:Label ID="Label_Locality" runat="server" Font-Size="Medium" Text="N/A"></asp:Label>
                        <br />
                        &nbsp;&nbsp;<br />&nbsp;&nbsp;
                        <br />
                        &nbsp;</asp:Panel>
                </ContentTemplate>
                </asp:UpdatePanel>
            </div>

    </asp:Content>
