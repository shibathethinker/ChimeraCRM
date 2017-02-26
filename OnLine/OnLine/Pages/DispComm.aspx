<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DispComm.aspx.cs" Inherits="OnLine.Pages.DispComm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link type="text/css" rel="stylesheet" href="~/Styles/Panel_Backgroud.css" />
<link type="text/css" rel="stylesheet" href="~/Styles/GridViewStyle.css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
    
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <br />
        <asp:Panel ID="Panel_New_Msg" runat="server" GroupingText="Upload New Messages Related to this topic"
            style="font-family: Andalus" Height="50%" CssClass="Panel3_Gradient">
            Note:
            <asp:TextBox class="form-control" ID="TextBox_Note" runat="server"  
                Width="70%" Height="80%" TextMode="MultiLine"></asp:TextBox>
            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="TextBox_Note" Display="Dynamic" ErrorMessage="* required" 
                ForeColor="Red" ValidationGroup="Note_Validate"></asp:RequiredFieldValidator>
            <br />
            <br />
            Email From:&nbsp;<asp:TextBox class="form-control" ID="TextBox_Email_From" runat="server" Width="282px"></asp:TextBox>
            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                ControlToValidate="TextBox_Email_From" Display="Dynamic" 
                ErrorMessage="* required" ForeColor="Red" ValidationGroup="Note_Validate"></asp:RequiredFieldValidator>
            &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator1" 
                runat="server" ControlToValidate="TextBox_Email_From" Display="Dynamic" 
                ErrorMessage="Invalid email id" ForeColor="Red" 
                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                ValidationGroup="Note_Validate"></asp:RegularExpressionValidator>
            Date:
            <asp:TextBox class="form-control" ID="TextBox_Email_Date" runat="server" Width="137px"></asp:TextBox>
                                        <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_Email_Date_CalendarExtender" 
                                runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton_Email_Date" 
                                TargetControlID="TextBox_Email_Date"></ajaxtoolkit:CalendarExtender>               
            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                ControlToValidate="TextBox_Email_Date" Display="Dynamic" 
                ErrorMessage="* required" ForeColor="Red" ValidationGroup="Note_Validate"></asp:RequiredFieldValidator>
            &nbsp;<br />
            <br />
            Time (Hour:Min:Sec):
            <asp:TextBox class="form-control" ID="TextBox_Email_Time_HH" runat="server" Width="26px"></asp:TextBox>
            <asp:RangeValidator ID="RangeValidator1" runat="server" 
                ControlToValidate="TextBox_Email_Time_HH" Display="Dynamic" 
                ErrorMessage="Range 0-23" ForeColor="Red" MaximumValue="23" MinimumValue="0" 
                SetFocusOnError="True" ValidationGroup="Note_Validate"></asp:RangeValidator>
            <asp:TextBox class="form-control" ID="TextBox_Email_Time_MM" runat="server" Width="26px"></asp:TextBox>
            <asp:RangeValidator ID="RangeValidator2" runat="server" 
                ControlToValidate="TextBox_Email_Time_MM" Display="Dynamic" 
                ErrorMessage="Range 0-59" ForeColor="Red" MaximumValue="59" MinimumValue="0" 
                SetFocusOnError="True" ValidationGroup="Note_Validate"></asp:RangeValidator>
            <asp:TextBox class="form-control" ID="TextBox_Email_Time_SS" runat="server" Width="26px"></asp:TextBox>
            <asp:RangeValidator ID="RangeValidator3" runat="server" 
                ControlToValidate="TextBox_Email_Time_SS" Display="Dynamic" 
                ErrorMessage="Range 0-59" ForeColor="Red" MaximumValue="59" MinimumValue="0" 
                SetFocusOnError="True" ValidationGroup="Note_Validate"></asp:RangeValidator>
            &nbsp;&nbsp; Attach Message file from your computer:&nbsp;<asp:FileUpload 
                ID="FileUpload_Attachment" runat="server"  Height="29px" 
                ViewStateMode="Enabled" Width="30%" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                ControlToValidate="FileUpload_Attachment" Display="Dynamic" ErrorMessage="Please select a file" 
                ForeColor="Red" SetFocusOnError="True" ValidationGroup="Note_Validate"></asp:RequiredFieldValidator>
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
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <asp:GridView ID="GridView_Notes" 
                        runat="server" 
                    AutoGenerateColumns="False" CellPadding="4" ForeColor="Black" 
                    GridLines="Vertical" 
                    PageSize="3" 
                        style="font-size: small; font-family: Andalus;" Height="200px" Width="759px" 
                        Font-Size="Medium" Visible="False" BackColor="White" BorderColor="#DEDFDE" 
                        BorderStyle="None" BorderWidth="1px" CssClass="mGrid" 
            onrowdeleting="GridView_Notes_RowDeleting" AllowSorting="True" 
            onsorting="GridView_Notes_Sorting">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:CommandField ShowDeleteButton="True" />
                        <asp:TemplateField HeaderText="HiddenCommId">
                            <ItemTemplate>
                                <asp:Label ID="Label_Hidden_Comm" runat="server" 
                                    Text='<%# Eval("HiddenComm") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <asp:TextBox class="form-control" ID="TextBox_Comment" runat="server" Height="72px" ReadOnly="True" 
                                    style="font-family: Andalus; font-size: small;" Text='<%# Eval("Comments") %>' 
                                    TextMode="MultiLine" Width="353px" Enabled="False"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox class="form-control" ID="TextBox10" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Email Message">
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
                        <asp:TemplateField HeaderText="Date Time" SortExpression="RespDateTimeTicks">
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
