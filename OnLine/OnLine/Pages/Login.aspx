<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="OnLine.Pages.Login" %>

<%@ Register assembly="EO.Web" namespace="EO.Web" tagprefix="eo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="description" content="">
    <meta name="author" content="">
    <title></title>
    <link type="text/css" rel="stylesheet" href="~/css/bootstrap.min.css" /> 
    <link type="text/css" rel="stylesheet" href="~/css/signin.css" /> 
</head>
<body style="font-family: Andalus">
<div class="container">
    <form id="form1" runat="server" class="form-signin">
    <h2 class="form-signin-heading">Please sign in</h2>        
                                        <asp:TextBox class="form-control" ID="UserName" runat="server" style="text-align: left" placeholder="User Id"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                                            ControlToValidate="UserName" ErrorMessage="User Name is required." 
                                            ToolTip="User Name is required." ValidationGroup="Login2" 
                                            Display="Dynamic" ForeColor="Red">*</asp:RequiredFieldValidator>
                                        <asp:TextBox class="form-control" ID="Password" runat="server" TextMode="Password"  placeholder="Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                                            ControlToValidate="Password" ErrorMessage="Password is required." 
                                            ToolTip="Password is required." 
        ValidationGroup="Login2" Display="Dynamic" ForeColor="Red">*</asp:RequiredFieldValidator>
                                    
                                        <asp:CheckBox ID="RememberMe" runat="server" Text="Remember me next time." />
                                        &nbsp;<asp:Button ID="LoginButton" runat="server" CommandName="Login" class="btn btn-lg btn-primary btn-block" 
                                            onclick="LoginButton_Click" Text="Log In" 
                                            ValidationGroup="Login2" />
                                        <asp:Literal ID="FailureText" runat="server" EnableViewState="False" 
                                            Text="Invalid Credentials Please Try Again" Visible="False"></asp:Literal>
                                      <asp:Label ID="Label1" runat="server" Text="Not registered yet? Register " 
                                            style="text-align: left"></asp:Label>
                                        <asp:HyperLink ID="HyperLink1" runat="server" 
                                            NavigateUrl="~/Account/Register.aspx">Here</asp:HyperLink>
   

    </form>
    </div>
</body>
</html>
