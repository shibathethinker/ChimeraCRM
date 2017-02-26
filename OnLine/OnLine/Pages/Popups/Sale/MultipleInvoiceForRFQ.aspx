<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MultipleInvoiceForRFQ.aspx.cs" Inherits="OnLine.Pages.Popups.Sale.MultipleInvoiceForRFQ" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <link type="text/css" rel="stylesheet" href="~/css/bootstrap.css" /> 
    <style type="text/css">
        .style1
        {
            font-family: Andalus;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
    
        <br />
        <br />
        <span class="style1">This Deal has multiple invoices attached</span><asp:GridView ID="GridView_Invoice" 
                            runat="server" 
                                AutoGenerateColumns="False" 
                                CellPadding="4" 
                                GridLines="None" 
                               CssClass="table table-striped table-bordered table-hover tableShadow" 
                                Height="30%" PageSize="5" 
                                style="font-size: small; font-family: Andalus;" 
            Visible="False" Width="90%" 
                                onsorting="GridView_Invoice_Sorting">
  
                                <Columns>
                                    <asp:TemplateField HeaderText="RFQ#">
                                        <ItemTemplate>
                                            <asp:Label ID="Label_RFQId1" runat="server" Text='<%# Eval("RFQNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice#">
                                        <ItemTemplate>
                                            <asp:Label ID="Label_Invoice_No1" runat="server" Text='<%# Eval("Inv_No") %>'></asp:Label>
                                            <br />
                                            <asp:Label ID="Label_Invoice_Id_Val" runat="server" 
                                                Text='<%# Eval("Inv_Id") %>' Visible="False"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ConvertEmptyStringToNull="False" HeaderText="Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="Label_Amount" runat="server" Text='<%# Eval("Amount") %>'></asp:Label>
                                            <br />
                                            <asp:Label ID="Label_Related_PO" runat="server" 
                                                Text='<%# Eval("Related_PO") %>' Visible="False"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice Details">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton_Show_Inv_Invoice_Grid" runat="server" 
                                                oncommand="LinkButton_Show_Inv_Invoice_Grid_Command" CommandArgument ="<%#((GridViewRow)Container).RowIndex%>">Show!</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice Date" SortExpression="Inv_Date_Ticks">
                                        <ItemTemplate>
                                            <asp:Label ID="Label_Inv_Date1" runat="server" Text='<%# Eval("Inv_Date") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle ForeColor="black" Font-Underline="True" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delivery Status">
                                        <ItemTemplate>
                                            <asp:Label ID="Label_Deliv_Stat1" runat="server" 
                                                Text='<%# Eval("Deliv_Stat") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Deliv_Stat_Edit" runat="server" 
                                                style="font-family: Andalus">
                                            </asp:DropDownList>
                                            &nbsp;<asp:Label ID="Label_Deliv_Stat_Edit" runat="server" 
                                                Text='<%# Eval("Deliv_Stat") %>' Visible="False"></asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Payment Status">
                                        <ItemTemplate>
                                            <asp:Label ID="Label_Pmnt_Stat1" runat="server" Text='<%# Eval("Pmnt_Stat") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Pmnt_Edit" runat="server" 
                                                style="font-family: Andalus">
                                            </asp:DropDownList>
                                            &nbsp;<asp:Label ID="Label_Pmnt_Stat_Edit" runat="server" 
                                                Text='<%# Eval("Pmnt_Stat") %>' Visible="False"></asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Payment Details">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton_Pmnt_Det_Inv" runat="server" 
                                                oncommand="LinkButton_Pmnt_Det_Inv_Command" CommandArgument ="<%#((GridViewRow)Container).RowIndex%>">Show!</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Defects">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton_Show_Defect1" runat="server" 
                                                oncommand="LinkButton_Show_Defect1_Command" CommandArgument ="<%#((GridViewRow)Container).RowIndex%>">Show!</asp:LinkButton>
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
