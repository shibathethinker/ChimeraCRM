<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="OnLine.Pages.Register" %>

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
    <script language="javascript">
        function validateRegistration() {
            var isValid = false;
            isValid = Page_ClientValidate('Panel2');
            if (isValid)
                isValid = Page_ClientValidate('Panel1');

            return isValid;
        }
    
    </script>

</head>
<body>
    <form id="form1" method="post" enctype="multipart/form-data" runat="server" target="_self">
    <div align="center">
                &nbsp;<asp:HyperLink ID="HyperLink2" runat="server" 
                                            NavigateUrl="~/Pages/Login.aspx" 
                    style="font-family: Andalus; font-size: medium">Return to Login</asp:HyperLink>
                                    <br />
                <asp:Label ID="Label1" runat="server" ForeColor="Red" 
                style="font-family: Andalus; font-size: small; text-align: center" 
                Text="All * marked fields are mandatory"></asp:Label>
    </div>
    <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
                                <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title">Register the easiest way</h3>
            </div>
            <div class="panel-body" align="center">  
                    <asp:Panel ID="Panel1"   runat="server"
            style="font-family: Andalus">
            <br />            
            <asp:TextBox class="form-control" ID="TextBox1" runat="server" Width="20%"  MaxLength="200" placeholder="User Name"></asp:TextBox>
              &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
                  ControlToValidate="TextBox1" Display="Dynamic" ErrorMessage="*" ForeColor="Red" 
                  ValidationGroup="Panel1"></asp:RequiredFieldValidator>
              &nbsp;
            <asp:TextBox class="form-control" ID="TextBox4" runat="server" Width="20%"  MaxLength="100" placeholder="Email Id"></asp:TextBox>
            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" 
                  ControlToValidate="TextBox4" Display="Dynamic" ErrorMessage="*" ForeColor="Red" 
                  ValidationGroup="Panel1"></asp:RequiredFieldValidator>
              <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                  ControlToValidate="TextBox4" Display="Dynamic" ErrorMessage="Invalid email id" 
                  ForeColor="Red" 
                  ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                  ValidationGroup="Panel1"></asp:RegularExpressionValidator>
              <br /> 
                        <br />                        
            <asp:TextBox class="form-control" ID="TextBox2" TextMode="Password" runat="server" Width="20%"  placeholder="Passsword"
                  ></asp:TextBox>
              &nbsp;
              <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" 
                  ControlToValidate="TextBox2" Display="Dynamic" ErrorMessage="*" ForeColor="Red" 
                  ValidationGroup="Panel1"></asp:RequiredFieldValidator>
              &nbsp;
            <asp:TextBox class="form-control" ID="TextBox3" TextMode="Password" runat="server" Width="20%"  placeholder="Confirm Password"
                  ></asp:TextBox>
              <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" 
                  ControlToValidate="TextBox3" ErrorMessage="*" ForeColor="Red" 
                  ValidationGroup="Panel1"></asp:RequiredFieldValidator>
            &nbsp;<asp:CompareValidator ID="CompareValidator1" runat="server" 
                  ControlToCompare="TextBox2" ControlToValidate="TextBox3" Display="Dynamic" 
                  ErrorMessage="CompareValidator" ForeColor="Red">Password mismatch</asp:CompareValidator>
            <br />
            <br />
            Name of Your Organization (Or the name by which you want to be known in this 
            site):&nbsp;
            <asp:TextBox class="form-control" ID="TextBox5" runat="server" Width="368px"  MaxLength="50" ></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" 
                  ControlToValidate="TextBox5" ErrorMessage="*" ForeColor="Red" 
                  ValidationGroup="Panel1"></asp:RequiredFieldValidator>
                <br />
                <div align="center">
                    <br />
                    <asp:Label ID="Label_UserId_Exists" runat="server" ForeColor="Red" Text="Label" 
                        Visible="False"></asp:Label>
                    <asp:LinkButton ID="LinkButton1" runat="server" onclick="LinkButton1_Click" 
                        ValidationGroup="Panel1">Want to register my business!</asp:LinkButton>
                    &nbsp; 
                    <asp:Button ID="Button_Register_Short" runat="server" Text="Submit!" 
class="btn btn-sm btn-success"
                    style="text-align: center; font-family: Andalus" 
                        onclick="Button_Register_Short_Click" ValidationGroup="Panel1" /></div>
                </asp:Panel>    
                </div>
                </div>
        </ContentTemplate>
        </asp:UpdatePanel>
       </div>
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>

    <asp:Panel ID="Panel2" runat="server" 
        style="font-family: Andalus" Visible="False" >
                                    <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title">Want to grow business? Then Enter... (You can enter the following details later as well)</h3>
            </div>
            <div class="panel-body" align="center">  
        <asp:Panel ID="Panel2_Inner" runat="server" >
        &nbsp;Business owner name:
        <asp:TextBox class="form-control" ID="TextBox6" runat="server" Width="20%"  
            MaxLength="50" ></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
            Display="Dynamic" ErrorMessage="*" ForeColor="Red" 
            ValidationGroup="Panel2" ControlToValidate="TextBox6"></asp:RequiredFieldValidator>
        &nbsp;Website:
        <asp:TextBox class="form-control" ID="TextBox7" runat="server" Width="20%"  ></asp:TextBox>
        &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
            Display="Dynamic" ErrorMessage="*" ForeColor="Red" 
            ValidationGroup="Panel2" ControlToValidate="TextBox7"></asp:RequiredFieldValidator>
        &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator3" 
            runat="server" ControlToValidate="TextBox7" Display="Dynamic" 
            ErrorMessage="Invalid Site Name (e.g. www.abc.com)" ForeColor="Red" 
            ValidationExpression="([\w-]+\.)+[\w-]+(/[\w- ./?%&amp;=]*)?" 
            ValidationGroup="Panel2"></asp:RegularExpressionValidator>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <br />
            <br />
            Contact No:
        <asp:TextBox class="form-control" ID="TextBox10" runat="server" Width="20%" MaxLength="14" ></asp:TextBox>
        &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
            Display="Dynamic" ErrorMessage="*" ForeColor="Red" 
            ControlToValidate="TextBox10" ValidationGroup="Panel2"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
            ControlToValidate="TextBox10" Display="Dynamic" 
            ErrorMessage="Please enter valid contact number" ForeColor="Red" 
            ValidationExpression="^\d{10,14}$" ValidationGroup="Panel2"></asp:RegularExpressionValidator>
        You are representing:
        <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList1" runat="server" 
            onselectedindexchanged="DropDownList1_SelectedIndexChanged" 
            style="font-family: Andalus">
            <asp:ListItem Value="C">Chain Of Shops</asp:ListItem>
            <asp:ListItem Value="S">Single Shop</asp:ListItem>
            <asp:ListItem Value="I">Individual</asp:ListItem>
        </asp:DropDownList>
        &nbsp;
        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
            Display="Dynamic" ErrorMessage="*" ForeColor="Red" 
            ControlToValidate="DropDownList1" ValidationGroup="Panel2"></asp:RequiredFieldValidator>
        <br />
            <br />
        What best describe you?
        <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownListDescr" runat="server" 
            style="font-family: Andalus">
        </asp:DropDownList>
        &nbsp;What is your base currency?
        <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownListBaseCurr" runat="server" 
            style="font-family: Andalus">
        </asp:DropDownList>
        &nbsp;
        <asp:RequiredFieldValidator ID="RequiredFieldValidator_Curr" runat="server" 
            ControlToValidate="DropDownListBaseCurr" Display="Dynamic" ErrorMessage="*" 
            ForeColor="Red" ValidationGroup="Panel2"></asp:RequiredFieldValidator>
        <br />
        <br />
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                        <div class="panel-info" Width="90%">
            <div class="panel-heading">
            <h3 class="panel-title">Enter your primary address</h3>
            </div>
            <div class="panel-body">   
                <asp:Panel ID="Panel3" runat="server" 
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
                    <asp:TextBox class="form-control" ID="TextBox8" runat="server" Width="20%"  ></asp:TextBox>
                </asp:Panel>
                </div>
                </div>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
                                <div class="panel-info" Width="90%">
            <div class="panel-heading">
            <h3 class="panel-title">Your product and service details</h3>
            </div>
            <div class="panel-body">   
        <asp:Panel ID="Panel4" runat="server" 
            style="font-family: Andalus">
            Main product/service range:&nbsp;&nbsp;
            <asp:ListBox ID="ListBoxProdServc" runat="server" Width="30%" 
                Height="67px" SelectionMode="Multiple"  ></asp:ListBox>
            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                Display="Dynamic" ErrorMessage="*" ForeColor="Red" 
                ControlToValidate="ListBoxProdServc" ValidationGroup="Panel2"></asp:RequiredFieldValidator>
            &nbsp;</asp:Panel>
            </div>
            </div>
        <br />

                <div align="center">
                    <asp:Button ID="Button_Register_Business" runat="server" 
class="btn btn-sm btn-success"
                        onclick="Button1_Click" style="font-family: Andalus" Text="Submit!" 
                        ValidationGroup="Panel2" onclientclick="return validateRegistration()" />
                        
                    &nbsp;
                    <asp:Label ID="Label_Status" runat="server" Text="Label" Visible="False"></asp:Label>
                    <br />
                    <asp:HyperLink ID="HyperLink1" runat="server" Font-Underline="True" 
                        NavigateUrl="~/Account/ChainRegistration.aspx" Target="_blank" 
                        Visible="False">I also want to let others know about my business chains</asp:HyperLink>
                    <br />
        </div>
        <br />
        </asp:Panel>
            </div>
    </div>
    </asp:Panel>

    </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="Button_Register_Business"/>
                        </Triggers>
    </asp:UpdatePanel>
        </form>
    <p>
</p>
</body>
</html>
