<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllRFQ_Broadcast.aspx.cs" Inherits="OnLine.Pages.Popups.Purchase.AllRFQ_Broadcast" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">


legend {color:red}
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
    
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    
                            <asp:Label ID="Label_Brdcast_msg" runat="server" 
                                
            Text="You can broadcast this RFQ to other orgnizations in this site. Select the accrount detail from the dropdown and broadcast" 
            ForeColor="Green" 
            style="font-family: Andalus; font-size: small; font-weight: 700"></asp:Label>
        <br />
        <br />
                            <asp:Label ID="Label_Brdcast_msg0" runat="server" 
                                
            Text="ALL: Means to every other organization in this site" ForeColor="Green" 
            style="font-family: Andalus; font-size: small; font-weight: 700"></asp:Label>
        <br />
                            <asp:Label ID="Label_Brdcast_msg1" runat="server" 
                                
            Text="ALL INTERESTED: Only to those organizations who sells product which is required for this RFQ" 
            ForeColor="Green" 
            style="font-family: Andalus; font-size: small; font-weight: 700"></asp:Label>
    
        <br />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
                        <asp:Panel ID="Panel_Broadcast" runat="server" 
                    GroupingText="Broadcast Details" 
            style="font-family: Andalus; font-size: small">
                            <asp:Label ID="Label_crnt_brdcat" runat="server" 
                                Text="Currently broadcasted to"></asp:Label>
&nbsp;<asp:Label ID="Label_No_Broadcast_List" runat="server" ForeColor="Red" Text="Broadcast list is empty" 
                                Visible="False"></asp:Label>
                            &nbsp;<asp:Label ID="Label_Deletion_Not_Allowed" runat="server" ForeColor="Red" 
                                Text="RFQ is already approved.. no deletion allowed" Visible="False"></asp:Label>
                            <br />
                            &nbsp;<asp:GridView ID="GridView_Broadcast_List" runat="server" AllowPaging="True" 
                                AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                                GridLines="None" PageSize="5" Visible="False" 
                                onrowdeleting="GridView_Broadcast_List_RowDeleting" 
                                onpageindexchanging="GridView_Broadcast_List_PageIndexChanging">
                                <AlternatingRowStyle BackColor="White" />
                                <Columns>
                                    <asp:CommandField ShowDeleteButton="True" />
                                    <asp:TemplateField HeaderText="Broadcasted To">
                                        <EditItemTemplate>
                                            <asp:TextBox class="form-control" ID="TextBox1" runat="server"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton_Name" OnCommand="gatherContactData" CommandArgument
                                     ="<%#Container.DataItemIndex %>" runat="server" Text='<%# Eval("name") %>'></asp:LinkButton>
                                            <br />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                                <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                                <SortedAscendingCellStyle BackColor="#FDF5AC" />
                                <SortedAscendingHeaderStyle BackColor="#4D0000" />
                                <SortedDescendingCellStyle BackColor="#FCF6C0" />
                                <SortedDescendingHeaderStyle BackColor="#820000" />
                                <SortedAscendingCellStyle BackColor="#FDF5AC" />
                                <SortedAscendingHeaderStyle BackColor="#4D0000" />
                                <SortedDescendingCellStyle BackColor="#FCF6C0" />
                                <SortedDescendingHeaderStyle BackColor="#820000" />
                            </asp:GridView>
                            <br />
                            <asp:Label ID="Label1" runat="server" Text=" Add to broadcast list:"></asp:Label>
                            &nbsp;<asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_BroadCast_List" runat="server" 
                                style="font-family: Andalus">
                            </asp:DropDownList>
                            &nbsp;<asp:Button ID="Button_Submit" runat="server" onclick="Button_Submit_Click" 
                                style="font-family: Andalus" Text="Add!" />
                            &nbsp;<asp:Button ID="Button_Refresh" runat="server" ForeColor="#336600" 
                                onclick="Button_Refresh_Click" style="font-family: Andalus; font-size: x-small" 
                                Text="Refresh Data!" />
                            &nbsp;or
                            <asp:LinkButton ID="LinkButton_Create_Contact" runat="server" 
                                onclick="LinkButton_Create_Contact_Click">Create New Contact and Add to this List!</asp:LinkButton>
                            &nbsp;<br />&nbsp;&nbsp;<asp:Label ID="Label_Invalid_Broad_List" runat="server" ForeColor="Red" 
                                Text="Only ALL or ALL_INTERESTED is allowed at a time" Visible="False"></asp:Label>
                            <br />
                            <br />
                            <asp:Label ID="Label_Approve_To_BroadCast" runat="server" ForeColor="Green" 
                                Text="* RFQ need to be approved for broadcasting rules to be active"></asp:Label>
                            <br />
                            &nbsp;</asp:Panel>
        </ContentTemplate>
        </asp:UpdatePanel>
        
    
    </div>
    </form>
</body>
</html>
