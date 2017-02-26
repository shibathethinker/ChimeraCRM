<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllLead_NDA.aspx.cs" Inherits="OnLine.Pages.Popups.Sale.AllLead_NDA" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            border-style: solid;
            border-width: 1px;
        }
        .style2
        {
            width: 209px;
            font-family: Andalus;
        }
        .style3
        {
            font-family: Andalus;
        }
        .style4
        {
            font-family: Andalus;
            color: #FFFFCC;
        }
        .style5
        {
            width: 209px;
            font-family: Andalus;
            color: #FFFFCC;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
    
        <br />
        <table class="style1">
            <tr>
                <td id="orgCopyHeader" runat="server" bgcolor="990000" class="style4"> Original Documents Copy By Customer</td>
                <td id="CopyByYouHeder" runat="server" bgcolor="990000" class="style4"> Documents Uploaded by You</td>
                <td id="UploadHeder" runat="server" bgcolor="990000" class="style5">Update Document and Upload</td>
            </tr>
            <tr>
                <td id="orgCopyLink" runat="server" bgcolor="FFFBD6" class="style3">
                    <asp:LinkButton ID="LinkButton_Org_NDA_Path" oncommand="Link_Show_NDA" 
                        CommandArgument="org" runat="server">Show!</asp:LinkButton>
                </td>
                <td bgcolor="FFFBD6" class="style3">
                    <asp:LinkButton ID="LinkButton_New_NDA" oncommand="Link_Show_NDA" CommandArgument="new" runat="server">Show!</asp:LinkButton>
                </td>
                <td  bgcolor="FFFBD6" class="style2">
                    <asp:FileUpload ID="FileUpload1" runat="server" Font-Names="Andalus" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="FileUpload1" Display="Dynamic" ErrorMessage="Please select a file" 
                ForeColor="Red" SetFocusOnError="True" ValidationGroup="Doc_Validate"></asp:RequiredFieldValidator>
                </td>
            </tr>
        </table>
        <br />
        <asp:Button ID="Button_Submit" runat="server" Text="Submit!" 
            onclick="Button_Submit_Click" style="font-family: Andalus" 
            ValidationGroup="Doc_Validate" />
    &nbsp;<asp:Label ID="Label_Upload_Stat" runat="server" style="font-family: Andalus" 
            Visible="False"></asp:Label>
    </div>
    </form>
</body>
</html>
