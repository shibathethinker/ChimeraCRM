<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllRequirement_Specification_Show_Feat.aspx.cs" Inherits="OnLine.Pages.Popups.Purchase.AllRequirement_Specification_Show_Feat" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <p>
        &nbsp;</p>
    <form id="form1" runat="server">
    <div align="center">
    <asp:GridView ID="GridView_Spec" runat="server" AllowPaging="True" 
            AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
            GridLines="None" style="font-family: Andalus">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:CommandField ShowSelectButton="True" />
                <asp:TemplateField HeaderText="Hidden">
                    <ItemTemplate>
                        <asp:Label ID="Label_Hidden" runat="server" Text='<%# Eval("Hidden") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox class="form-control" ID="TextBox1" runat="server"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Feature Name">
                    <ItemTemplate>
                        <asp:Label ID="Label_Hidden0" runat="server" Text='<%# Eval("FeatName") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox class="form-control" ID="TextBox2" runat="server"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Spec Text">
                    <ItemTemplate>
                        <asp:Label ID="Label_SpecText" runat="server" Text='<%# Eval("SpecText") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox class="form-control" ID="TextBox3" runat="server"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="From Spec">
                    <ItemTemplate>
                        <asp:Label ID="Label_FromSpec" runat="server" Text='<%# Eval("FromSpec") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox class="form-control" ID="TextBox4" runat="server"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="To Spec">
                    <ItemTemplate>
                        <asp:Label ID="Label_ToSpec" runat="server" Text='<%# Eval("ToSpec") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox class="form-control" ID="TextBox5" runat="server"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Image">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton_Show_Img" runat="server">Show!</asp:LinkButton>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox class="form-control" ID="TextBox6" runat="server"></asp:TextBox>
                    </EditItemTemplate>
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
        </asp:GridView>
    </div>
    </form>
</body>
</html>
