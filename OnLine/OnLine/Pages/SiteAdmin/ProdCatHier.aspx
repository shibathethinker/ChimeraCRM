<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProdCatHier.aspx.cs" Inherits="OnLine.Pages.SiteAdmin.ProdCatHier" %>

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
    
        <br />
        <br />
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    <asp:Panel ID="Panel2" runat="server" BorderColor="#0066CC" BorderStyle="Ridge" 
        BorderWidth="2px" GroupingText="Product Category and Hierarchy" 
        style="font-family: Andalus">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Parent 
        Categories:
        <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList1" runat="server" 
            onselectedindexchanged="DropDownList1_SelectedIndexChanged" 
            style="font-family: Andalus">
        </asp:DropDownList>
        &nbsp;
        <asp:Button ID="Button_Show_Sub_Cat" runat="server" 
            onclick="Button1_Click" style="font-family: Andalus" 
            Text="Show All Sub-Categories!" />
        <br />
&nbsp;<br />
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
        <div align="center">
        <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" 
            GridLines="None" AllowPaging="True" PageSize="3" Visible="False" 
                onpageindexchanging="GridView1_PageIndexChanging" 
                onrowediting="GridView1_RowEditing" onrowupdating="GridView1_RowUpdating" 
                AutoGenerateColumns="False" onrowdatabound="GridView1_RowDataBound" 
                onrowdeleting="GridView1_RowDeleting">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:CommandField ShowDeleteButton="True" />
                <asp:CommandField ShowCancelButton="False" ShowEditButton="True" />
                <asp:TemplateField HeaderText="Hidden">
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Eval("Hidden_Cat_Id") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ProdName">
                    <ItemTemplate>
                        <asp:Label ID="Label4" runat="server" Text='<%# Eval("Category Name") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Visible">
                    <EditItemTemplate>
                        <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownListVisible" runat="server" 
                            style="font-family: Andalus" 
                            onselectedindexchanged="DropDownListVisible_SelectedIndexChanged">
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%#Eval("Visible") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EditRowStyle BackColor="#999999" />
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#E9E7E2" />
            <SortedAscendingHeaderStyle BackColor="#506C8C" />
            <SortedDescendingCellStyle BackColor="#FFFDF8" />
            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
        </asp:GridView>
            <br />
            <asp:Button ID="Button_Change_Parent_Cat" runat="server" 
                onclick="Button1_Click" style="font-family: Andalus" Text="Change Parent Category!" 
                ValidationGroup="Panel2" Visible="False" />
            &nbsp;<asp:Label ID="Label1" runat="server" Text="Label" Visible="False"></asp:Label>
        </div>
        </ContentTemplate>
        </asp:UpdatePanel>
        
        
        <br />
        <br />
        <div align="center">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
             <asp:Panel ID="Panel4" runat="server" BorderColor="#0066CC" BorderStyle="Ridge" 
            BorderWidth="2px" GroupingText="Add Category" 
            style="font-family: Andalus">
            Name:
            <asp:TextBox class="form-control" ID="TextBox1" runat="server"  Width="169px"></asp:TextBox>
                 &nbsp;Parent Category:
            <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList2" runat="server" 
                onselectedindexchanged="DropDownList1_SelectedIndexChanged" 
                style="font-family: Andalus">
            </asp:DropDownList>
                 &nbsp;Add Features to this Category:
                 <asp:ListBox ID="ListBoxFeat" runat="server"  Height="51px" 
                     SelectionMode="Multiple"></asp:ListBox>
                 &nbsp;<asp:Button ID="Button_Add_Category" runat="server" 
                onclick="Button_Add_Category_Click" style="font-family: Andalus" Text="Add!" 
                />
        </asp:Panel>
           
            </ContentTemplate>
            </asp:UpdatePanel>
                    
        </div>
        
        <asp:Label ID="Label5" runat="server" Text="Label" Visible="False"></asp:Label>
        
        <br />

        <br />
    </asp:Panel>
    
    </div>
    </form>
</body>
</html>
