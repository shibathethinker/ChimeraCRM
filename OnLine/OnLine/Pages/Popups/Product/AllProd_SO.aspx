<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllProd_SO.aspx.cs" Inherits="OnLine.Pages.Popups.Product.AllProd_SO" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link type="text/css" rel="stylesheet" href="~/css/bootstrap.css" /> 
    <title></title>
        <style type="text/css">
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
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
    
        <br />
        <asp:Label ID="Label_Empty_Grid" runat="server" Visible="False"></asp:Label>
                        <br />
        <asp:Label ID="Label_Total_Text" runat="server" Visible="False"></asp:Label>
        &nbsp;<asp:Label ID="Label_Total_No" runat="server" Visible="False"></asp:Label>
&nbsp;&nbsp;
        <br />
                <asp:Label ID="Label_Total_Delivered_Text" runat="server" Visible="False"></asp:Label>
        &nbsp;<asp:Label ID="Label_Total_Delivered_No" runat="server" Visible="False"></asp:Label>
        <br />
   <asp:GridView ID="GridView1" 
                        runat="server" 
                    AutoGenerateColumns="False" CellPadding="4" 
                    GridLines="None" 
                                        CssClass="table table-striped table-bordered table-hover tableShadow" 
                    PageSize="3" Visible="False" 
                        style="font-size: small" Height="30%" Width="90%" 
                        Font-Size="Medium">

                            <Columns>
                                <asp:TemplateField HeaderText="SO_No">
                                    <ItemTemplate>
                                        <asp:Label ID="Label_RFQ_No" runat="server" Text='<%# Eval("RFQNo") %>'></asp:Label>
                                        <asp:LinkButton ID="LinkButton_Show_SO" runat="server" 
                                            oncommand="LinkButton_Show_SO_Command" Text='<%# Eval("So_No") %>'></asp:LinkButton>
                                    
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox class="form-control" ID="TextBox_Prod_Name_Edit" runat="server" 
                                            style="font-family: Andalus" Text='<%# Eval("ProdName") %>'></asp:TextBox>
                                        <asp:Label ID="Label_Name_Hidden" runat="server" Text='<%# Eval("ProdName") %>' 
                                            Visible="False"></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton_Show_SO" runat="server" CommandArgument ="<%#((GridViewRow)Container).RowIndex%>"
                                            oncommand="LinkButton_Show_SO_Command" Text='<%# Eval("So_No") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="RFQ_No">
                                    <ItemTemplate>
                                        <asp:Label ID="Label_RFQNo" runat="server" Text='<%# Eval("RFQNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="No of Units Delivered">
                                    <ItemTemplate>
                                        <asp:Label ID="Label_Delivered" runat="server" Text='<%# Eval("delivered") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Quote">
                                    <ItemTemplate>
                                        <asp:Label ID="Label_Units" runat="server" 
                                            Text='<%# Eval("units") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label_Quote" runat="server" Text='<%# Eval("quote") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Units Ordered">
                                    <ItemTemplate>
                                        <asp:Label ID="Label_Units" runat="server" Text='<%# Eval("units") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                                                    <RowStyle CssClass="cursor-pointer" />
                        <AlternatingRowStyle CssClass="active" />
                        <SelectedRowStyle CssClass="danger" />
                        <HeaderStyle CssClass="success" />
                        <PagerStyle CssClass="pagination-lg" />
                        <FooterStyle CssClass="success"/>
                        <EditRowStyle CssClass="info" />
                        </asp:GridView>
    
    </div>
    </form>
</body>
</html>
