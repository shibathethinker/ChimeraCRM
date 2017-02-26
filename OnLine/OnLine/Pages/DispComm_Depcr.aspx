<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DispComm_Depcr.aspx.cs" Inherits="OnLine.Pages.DispComm"%>

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
        .style2
        {
            font-family: Andalus;
            font-size: small;
        }
        </style>
</head>
<body style="font-family: Andalus">
    <form id="form1" runat="server" enctype="multipart/form-data">
    <div align="center">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div align="left">
        <asp:LinkButton ID="LinkButtonChat" runat="server">Chat Details</asp:LinkButton>
        </div>
        <asp:Panel ID="PanelChat" runat="server" Font-Names="Andalus" Font-Size="Small" 
            GroupingText="New chat and history">
            <ajaxtoolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" AutoCollapse="false" TargetControlID="PanelChat" ImageControlID="Image1"
                        ExpandedImage="~/Images/ArrowTip_Down.png" CollapsedImage="~/Image/ArrowTip_Parallel.png" ExpandControlID="LinkButtonChat"
             CollapseControlID="LinkButtonChat"/>           

        
    <div style ="height:331px; overflow:auto;width:787px">
            <br />

        <br />
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
                    <asp:GridView ID="GridView_RFQ_Resp_Quotes_Comm" 
                        runat="server" 
                    AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                    GridLines="None" onpageindexchanging="GridView_RFQ_Resp_Quotes_Comm_PageIndexChanging" 
                    PageSize="3" 
                        onselectedindexchanged="GridView_RFQ_Resp_Quotes_Comm_SelectedIndexChanged" 
                        style="font-size: xx-small" Height="200px" Width="759px" 
                        Font-Size="Medium" Visible="False">
                    <AlternatingRowStyle BackColor="#FF9933" />
                    <Columns>
                        <asp:TemplateField HeaderText="HiddenCommId">
                            <ItemTemplate>
                                <asp:Label ID="Label_Hidden_Comm" runat="server" 
                                    Text='<%# Eval("HiddenComm") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date Time">
                            <ItemTemplate>
                                <asp:Label ID="Label_Date" runat="server" Text='<%# Eval("RespDateTime") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox class="form-control" ID="TextBox3" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="From User">
                            <ItemTemplate>
                                <asp:Label ID="Label_From_Usr" runat="server" Text='<%# Eval("FromUsr") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox class="form-control" ID="TextBox8" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="From Company">
                            <ItemTemplate>
                                <asp:Label ID="Label_From_Comp" runat="server" Text='<%# Eval("FromComp") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox class="form-control" ID="TextBox9" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Comments">
                            <ItemTemplate>
                                <asp:TextBox class="form-control" ID="TextBox_Comment" runat="server" Height="72px" ReadOnly="True" 
                                    style="font-family: Andalus" Text='<%# Eval("Comments") %>' 
                                    TextMode="MultiLine" Width="353px"></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox class="form-control" ID="TextBox10" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" 
                            Font-Size="Small" />
                    <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                    <RowStyle BackColor="#FFFBD6" ForeColor="#333333" Font-Size="Small" />
                    <SelectedRowStyle BackColor="Aqua" Font-Bold="True" ForeColor="Navy" />
                    <SortedAscendingCellStyle BackColor="#FDF5AC" />
                    <SortedAscendingHeaderStyle BackColor="#4D0000" />
                    <SortedDescendingCellStyle BackColor="#FCF6C0" />
                    <SortedDescendingHeaderStyle BackColor="#820000" />
                </asp:GridView>
        </ContentTemplate>
        </asp:UpdatePanel>
        
    </div>
    
    
    <br />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <asp:Panel ID="Panel_Prod_Srv_Qnty" runat="server" BorderColor="#0066CC" 
            BorderStyle="Ridge" BorderWidth="2px" GroupingText="Enter New Comment" 
            style="font-family: Andalus">
            Comment:
            <asp:TextBox class="form-control" ID="TextBox_Comments" runat="server"  
                Width="524px" Height="109px" TextMode="MultiLine"></asp:TextBox>
            &nbsp;<br />
            <br />
            <asp:Button ID="Button_Submit_Comment" runat="server" 
                style="font-family: Andalus" Text="Submit!" 
                onclick="Button_Submit_Comment_Click" />
            &nbsp;<asp:Label ID="Label_Insert_Stat" runat="server" Visible="False"></asp:Label>
            <br />
        </asp:Panel>
        </ContentTemplate>
        </asp:UpdatePanel>
        
    </asp:Panel>
    </div>
    <div align="left" style="height: 551px">
            <asp:LinkButton ID="LinkButtonEmail" runat="server" style="font-size: medium">Email Details</asp:LinkButton>
            <asp:Panel ID="PanelEmail" runat="server" Font-Names="Andalus" Font-Size="Small" 
            GroupingText="" Height="515px">
            <ajaxtoolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" AutoCollapse="false" TargetControlID="PanelEmail" ImageControlID="Image2"
                        ExpandedImage="~/Images/ArrowTip_Down.png" CollapsedImage="~/Image/ArrowTip_Parallel.png" ExpandControlID="LinkButtonEmail"
             CollapseControlID="LinkButtonEmail"/>           
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                        <td align="center">
                            <asp:Button ID="ButtonCompose" runat="server" Text="Compose New Email" 
                                style="font-family: Andalus; font-size: small" 
                                onclick="ButtonCompose_Click" />
                            &nbsp;<asp:Button ID="ButtonRefreshEmail" runat="server" 
                                onclick="ButtonRefreshEmail_Click" 
                                style="font-family: Andalus; font-size: small" Text="Refresh" />
                        </td>
                        <td align="center">
                            
                            <asp:Button ID="ButtonReply" runat="server" Text="Reply" CssClass="style2" />
                            &nbsp;<asp:Button ID="ButtonReplyAll" runat="server" CssClass="style2" 
                                Text="ReplyAll" />
                            &nbsp;<asp:Button ID="ButtonForward" runat="server" Text="Forward" 
                                CssClass="style2" />
                        </td>
                        </tr>
                            <tr>
                                <td align="left">
                                    <asp:GridView ID="GridView_Email" runat="server" AllowPaging="True" 
                                        AutoGenerateColumns="False" CellPadding="4" Font-Size="Medium" 
                                        ForeColor="#333333" GridLines="None" Height="200px" 
                                        onpageindexchanging="GridView_Lead_PageIndexChanging" 
                                        style="font-size: xx-small" Visible="False" Width="467px" 
                                        onselectedindexchanged="GridView_Email_SelectedIndexChanged" PageSize="5">
                                        <AlternatingRowStyle BackColor="#F1FBFF" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Hidden#">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label_Hidden" runat="server" Text='<%# Eval("Hidden") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sender">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label_Sender" runat="server" Text='<%# Eval("Sender") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label_Date" runat="server" Text='<%# Eval("RecvDate") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Subject">
                                                <ItemTemplate>
                                                            <asp:LinkButton ID="LinkButton_Subject" runat="server" 
                                                                CommandArgument="<%# Container.DataItemIndex %>" 
                                                                oncommand="LinkButton_Subject_Command1" Text='<%# Eval("Subject") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="#B3DBF8" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#B3DBF8" Font-Bold="True" Font-Size="Small" 
                                            ForeColor="Black" />
                                        <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#E1F4FD" Font-Size="Small" ForeColor="#0366C7" />
                                        <SelectedRowStyle BackColor="#B9E5FB" Font-Bold="True" ForeColor="#00488F" />
                                        <SortedAscendingCellStyle BackColor="#FDF5AC" />
                                        <SortedAscendingHeaderStyle BackColor="#4D0000" />
                                        <SortedDescendingCellStyle BackColor="#FCF6C0" />
                                        <SortedDescendingHeaderStyle BackColor="#820000" />
                                    </asp:GridView>
                                    <br />
                                    <div align="center">
                                    <asp:Button ID="ButtonNextPage" runat="server" onclick="ButtonNextPage_Click" 
                                        style="font-family: Andalus; font-size: small" Text="Next Page&gt;&gt;" />
                                        </div>
                                </td>
                                <td align="left" ID="MessagBodyHTML" runat="server">
                                    <asp:TextBox class="form-control" ID="TextBox_MsgBody" runat="server" BackColor="White" 
                                        BorderColor="#003366" BorderStyle="Solid" BorderWidth="2px" Enabled="False" 
                                        Height="500px" TextMode="MultiLine" Width="471px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td align="left">
                                    <asp:Label ID="LabelAttachment" runat="server" Text="Attachment/s"></asp:Label>
                                    &nbsp;<asp:Label ID="LabelAttachList" runat="server"></asp:Label>
                                    &nbsp;<asp:Button ID="ButtonDownload" runat="server" CssClass="style2" 
                                        onclick="ButtonDownload_Click" Text="Download!" />
                                    <asp:Label ID="LabelSelectedGridItem" runat="server" Visible="False"></asp:Label>
                                </td>
                            </tr>
                        </table>            
    <br />
                <asp:Label ID="MessageStatLabel" runat="server" Text="Label" Visible="False"></asp:Label>
    </asp:Panel>
    </div>
        <div align="center" id="hover"         
        style="position:fixed; right:235px; top:50px; z-index:20; width:62%; height:auto; ">
            <asp:Panel ID="PanelCompEmail" runat="server" GroupingText="New Email" 
                Height="584px" style="font-family: Andalus; font-size: small" 
                Width="717px" Visible="false" BackColor="#E1F4FD" BorderColor="White" 
                BorderStyle="Solid" BorderWidth="2px">
            <asp:Label ID="LabelTo" runat="server" style="font-size: small" Text="To:"></asp:Label>
&nbsp;<asp:TextBox class="form-control" ID="TextBoxTo" runat="server" 
                style="font-family: Andalus; font-size: small" Width="511px"></asp:TextBox>
            <br />
            <br />
            <asp:Label ID="LabelCc" runat="server" style="font-size: small" Text="Cc:"></asp:Label>
&nbsp;<asp:TextBox class="form-control" ID="TextBoxCc" runat="server" 
                style="font-family: Andalus; font-size: small" Width="511px"></asp:TextBox>
                            <br />
                <br />
                <asp:Label ID="LabelSubject" runat="server" style="font-size: small" 
                    Text="Subject:"></asp:Label>
                &nbsp;<asp:TextBox class="form-control" ID="TextBoxSubject" runat="server" 
                    style="font-family: Andalus; font-size: small" Width="580px"></asp:TextBox>
                            <br />
                <br />
                <asp:FileUpload ID="FileUpload1" runat="server" Font-Names="Andalus" />
                &nbsp;<asp:FileUpload ID="FileUpload2" runat="server" Font-Names="Andalus" 
                    Visible="False" />
                &nbsp;<asp:FileUpload ID="FileUpload3" runat="server" Font-Names="Andalus" 
                    Visible="False" />
                <br />
                <asp:Button ID="ButtonAddMore" runat="server" onclick="ButtonAddMore_Click" 
                    style="font-family: Andalus; font-size: small" Text="Add More!" />
                &nbsp;<br />&nbsp;<br /><asp:TextBox class="form-control" ID="TextBoxCompEmailBody" runat="server" 
                    Height="238px" ontextchanged="TextBoxCompEmailBody_TextChanged" 
                    style="margin-left: 0px; margin-top: 4px;" TextMode="MultiLine" Width="688px"></asp:TextBox>
                <br />
                <br />
                <asp:Button ID="ButtonSend" runat="server" onclick="ButtonSend_Click" 
                    style="font-family: Andalus; font-size: small" Text="Send!" />
                &nbsp;<asp:Button ID="ButtonDone" runat="server" onclick="ButtonDone_Click" 
                    style="font-family: Andalus; font-size: small" Text="Done!" />
                           &nbsp;<asp:Label ID="LabelSending" runat="server" Text="Sending ......" 
                    Visible="False"></asp:Label>
                           </asp:Panel>
        </div>
        <div align="center" id="DivAttachment"         
        style="position:fixed; right:234px; top:150px; z-index:20; width:62%; height:auto; ">
                        <asp:Panel ID="PanelDownload" runat="server" Font-Names="Andalus" Font-Size="Small" 
            GroupingText="" Height="250px" Visible="false">
                                          <asp:GridView ID="GridView_Attachment" runat="server" 
                                        AutoGenerateColumns="False" CellPadding="4" Font-Size="Medium" 
                                        ForeColor="#333333" GridLines="None" Height="200px" 
                                        style="font-size: xx-small" Visible="False" Width="284px" 
                                        onselectedindexchanged="GridView_Attachment_SelectedIndexChanged">
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:CommandField SelectText="Download" ShowSelectButton="True" />
                                            <asp:TemplateField HeaderText="Attachment Name">
                                                <ItemTemplate>
                                                            <asp:Label ID="Label_Attachment" runat="server" 
                                                                Text='<%# Eval("attachment") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                              <EditRowStyle BackColor="#2461BF" />
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" Font-Size="Small" 
                                            ForeColor="White" />
                                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#EFF3FB" Font-Size="Small" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                    </asp:GridView>
                            <asp:Button ID="ButtonHideAttachment" runat="server" Text="Done!" 
                                              style="font-family: Andalus" onclick="ButtonHideAttachment_Click" />
                                    </asp:Panel>
        </div>
    </form>
</body>
</html>
