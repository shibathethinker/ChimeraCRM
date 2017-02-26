<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NoteDetails.aspx.cs" Inherits="OnLine.Pages.NoteDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link type="text/css" rel="stylesheet" href="~/Styles/Panel_Backgroud.css" />
<link type="text/css" rel="stylesheet" href="~/Styles/GridViewStyle.css" />
    <title></title>
    <style type="text/css">
        .style1
        {
            font-family: Andalus;
            font-size: small;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:Panel ID="Panel_New_Note" runat="server" GroupingText="Enter New Comment"
            style="font-family: Andalus" Height="50%" CssClass="Panel3_Gradient">
            Note:
            <asp:TextBox class="form-control" ID="TextBox_Note" runat="server"  
                Width="70%" Height="80%" TextMode="MultiLine"></asp:TextBox>
            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="TextBox_Note" Display="Dynamic" ErrorMessage="* required" 
                ForeColor="Red" ValidationGroup="Note_Validate"></asp:RequiredFieldValidator>
            <br />
            <br />
            Attachment:&nbsp;<asp:FileUpload ID="FileUpload_Attachment" runat="server" 
                 Height="29px" ViewStateMode="Enabled" 
                Width="30%" />
            <br />
            <br />
            <asp:Button ID="Button_Submit_Note" runat="server" 
                onclick="Button_Submit_Note_Click" 
                style="font-family: Andalus; font-size: small;" Text="Submit!" 
                ValidationGroup="Note_Validate" />
            &nbsp;<asp:Label ID="Label_Insert_Stat" runat="server" Visible="False" 
                style="font-size: small"></asp:Label>
            <br />
        </asp:Panel>
            <br />
            <br />
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
                    <asp:GridView ID="GridView_Notes" 
                        runat="server" 
                    AutoGenerateColumns="False" CellPadding="4" ForeColor="Black" 
                    GridLines="Vertical" 
                    PageSize="3" 
                        style="font-size: small; font-family: Andalus;" Height="200px" Width="759px" 
                        Font-Size="Medium" Visible="False" BackColor="White" BorderColor="#DEDFDE" 
                        BorderStyle="None" BorderWidth="1px" CssClass="mGrid">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:TemplateField HeaderText="HiddenCommId">
                            <ItemTemplate>
                                <asp:Label ID="Label_Hidden_Comm" runat="server" 
                                    Text='<%# Eval("HiddenComm") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Comments">
                            <ItemTemplate>
                                <asp:TextBox class="form-control" ID="TextBox_Comment" runat="server" Height="72px" ReadOnly="True" 
                                    style="font-family: Andalus; font-size: small;" Text='<%# Eval("Comments") %>' 
                                    TextMode="MultiLine" Width="353px" Enabled="False"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox class="form-control" ID="TextBox10" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Attachment">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton_Doc" runat="server" CommandArgument="<%# Container.DataItemIndex %>" 
                                    Text='<%# Eval("docName") %>' oncommand="LinkButton_Doc_Command"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="User">
                            <ItemTemplate>
                                <asp:Label ID="Label_From_Usr" runat="server" Text='<%# Eval("FromUsr") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date Time">
                            <ItemTemplate>
                                <asp:Label ID="Label_Date" runat="server" Text='<%# Eval("RespDateTime") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="#CCCC99" />
                    <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" 
                            Font-Size="Small" />
                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                    <RowStyle BackColor="#F7F7DE" Font-Size="Small" />
                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#FBFBF2" />
                    <SortedAscendingHeaderStyle BackColor="#848384" />
                    <SortedDescendingCellStyle BackColor="#EAEAD3" />
                    <SortedDescendingHeaderStyle BackColor="#575357" />
                </asp:GridView>
        </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
