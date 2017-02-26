<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FinalizeDeal.aspx.cs" Inherits="OnLine.Pages.Popups.Purchase.FinalizeDeal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link type="text/css" rel="stylesheet" href="~/Styles/GridViewStyle.css" />
        <link type="text/css" rel="stylesheet" href="~/css/bootstrap.css" /> 
        <script language="javascript">
            function RefreshParent() {
                window.opener.document.getElementById('ContentPlaceHolderBody_Button_Potn_Refresh_Hidden_Index_Unchanged').click();
            }
        </script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="height: 1211px; font-family: Andalus;" align="center">
    
        <asp:Label ID="Label1" runat="server" Text="Purchase Order" 
            style="font-family: Andalus; font-size: large;"></asp:Label>
    
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    
        <br />
        <br />
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
        <td align="left">
                            <asp:Image ID="Image_Logo" runat="server"  ImageAlign="Middle" 
                        Visible="False" Height="89px" Width="93px" />
                            <br />
                    <asp:Label ID="Label_Client_Name" runat="server" 
            style="font-family: Andalus; text-align: right; font-size: small;"></asp:Label>               
                            <br />
        <asp:Label ID="Label_Client_Addr" runat="server" style="font-family: Andalus; font-size: small;"></asp:Label>
                        <br />
        </td>
        <td align="right">
                            <asp:Image ID="Image_Logo1" runat="server"  ImageAlign="Middle" 
                        Visible="False" Height="89px" Width="93px" />
                    <asp:Label ID="Label_To" runat="server" 
            style="font-family: Andalus; text-align: right; font-weight: 700; font-size: small;"></asp:Label>               
                    <br />
                    <asp:Label ID="Label_Vendor_Name" runat="server" 
            style="font-family: Andalus; text-align: right; font-size: small;"></asp:Label>               
            <br />
            <asp:Label ID="Label_Vendor_Addr" runat="server" 
                        style="font-family: Andalus; font-size: small;"></asp:Label>
            <br />
        </td>
        </tr>
        </table>
        <br />

    
        <br />
        <asp:Label ID="Label_Ship_Via" runat="server" 
            style="font-family: Andalus; font-size: small;">Ship Via:</asp:Label>
                        &nbsp;<asp:TextBox class="form-control" ID="TextBox_Ship_Via" runat="server" style="font-family: Andalus; font-size: small;" 
            Width="248px"></asp:TextBox>
        <br />
        <br />
                    <asp:Label ID="Label_GridView_Status" runat="server" 
            style="font-size: small" Visible="False"></asp:Label>
                    <br />
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                    <asp:GridView ID="GridView1" 
                        runat="server" 
                    AutoGenerateColumns="False" CellPadding="4" 
                                                         CssClass="mGrid"
            PagerStyle-CssClass="pgr"
            AlternatingRowStyle-CssClass="alt"
            SelectedRowSyle-CssClass="sel"
                    GridLines="None" onpageindexchanging="GridView1_PageIndexChanging" 
                    onrowdatabound="GridView1_RowDataBound" onrowdeleting="GridView1_RowDeleting" 
                    onrowediting="GridView1_RowEditing" onrowupdating="GridView1_RowUpdating" 
                    PageSize="3" Visible="False" 
                        onselectedindexchanged="GridView1_SelectedIndexChanged" 
                        onselectedindexchanging="GridView1_SelectedIndexChanging" 
                        style="font-size: small" Height="50%" Width="90%" 
                        Font-Size="Medium" onrowcancelingedit="GridView1_RowCancelingEdit">
                                <Columns>
                        <asp:TemplateField HeaderText="Hidden">
                            <ItemTemplate>
                                <asp:Label ID="Label_Hidden" runat="server" 
                                    Text='<%# Eval("Hidden_Cat_Id") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Item#">
                            <ItemTemplate>
                                <asp:Label ID="Label_Serial" runat="server" 
                                    Text='<%# Eval("Serial") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Quantity">
                            <EditItemTemplate>
                                <asp:Label ID="Label_Qnty" runat="server" Text='<%# Eval("Qnty") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:UpdatePanel ID="UpdatePanel_GridView1_Qnty" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox class="form-control" ID="TextBox_Qnty" runat="server" AutoPostBack="True" 
                                            ontextchanged="TextBox_Qnty_TextChanged" style="font-family: Andalus" 
                                            Text='<%# Eval("Qnty") %>'></asp:TextBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Product Details">
                            <ItemTemplate>
                                <asp:Label ID="Label_Product_Name" runat="server" 
                                    Text='<%# Eval("Prod_Name") %>'></asp:Label>
                                <br />
                                <asp:Label ID="Label_Category_Name" runat="server" 
                                    Text='<%# Eval("Cat_Name") %>'></asp:Label>
                                <br />
                                <br />
                                <asp:GridView ID="GridView1_Inner" runat="server" AutoGenerateColumns="False" 
                                    BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px" 
                                    CellPadding="4" ForeColor="Black" GridLines="Horizontal" PageSize="1">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Hidden">
                                            <ItemTemplate>
                                                <asp:Label ID="Label_Hidden" runat="server" Text='<%# Eval("Hidden") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox class="form-control" ID="TextBox7" runat="server"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Feature">
                                            <ItemTemplate>
                                                <asp:Label ID="Label_Feature" runat="server" Text='<%# Eval("FeatName") %>'></asp:Label>
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
                                                <asp:Label ID="Label_From_Spec" runat="server" Text='<%# Eval("FromSpec") %>'></asp:Label>
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
                                    </Columns>
                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                    <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                                    <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                    <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                    <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                    <SortedDescendingHeaderStyle BackColor="#242121" />
                                </asp:GridView>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Unit Price">
                            <EditItemTemplate>
                                <asp:Label ID="Label_Unit_Price" runat="server" 
                                    Text='<%# Eval("Unit_Price") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:UpdatePanel ID="UpdatePanel_GridView1_UnitPrice" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox class="form-control" ID="TextBox_Unit_Price" runat="server" AutoPostBack="True" 
                                            ontextchanged="TextBox_Unit_Price_TextChanged" style="font-family: Andalus" 
                                            Text='<%# Eval("Unit_Price") %>'></asp:TextBox>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount">
                            <ItemTemplate>
                                <asp:Label ID="Label_Amount" runat="server" Text='<%# Eval("Cat_Total") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                    </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                    <td align="left">
                                            <asp:Label ID="Label_TnC" runat="server" 
            style="font-family: Andalus; text-align: right; font-weight: 700; font-size: small;">Terms &amp; Conditions</asp:Label>               
                        <br />
                        <asp:TextBox class="form-control" ID="TextBox_TnC" runat="server" Height="172px" 
                            TextMode="MultiLine" Width="410px"></asp:TextBox>
                    </td>

                    <td align="right">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                                            <asp:Label ID="Label_Sub_Total" runat="server" 
            style="font-family: Andalus; text-align: right; font-weight: 700; font-size: small;">Sub-Total</asp:Label>               
            &nbsp;<asp:Label ID="Label_Sub_Total_Amount_Value" runat="server" style="font-size: small"></asp:Label>
&nbsp;
                        <br />
                    <asp:Label ID="Label_Sub_Total0" runat="server" 
            style="font-family: Andalus; text-align: right; font-weight: 700; font-size: small;">Tax %</asp:Label>               
            &nbsp;
                        <asp:TextBox class="form-control" ID="TextBox_tax" runat="server" style="font-family: Andalus" AutoPostBack="True"  
                                                ontextchanged="TextBox_tax_TextChanged"></asp:TextBox>
                        <br />
                    <asp:Label ID="Label_Amount" runat="server" 
            style="font-family: Andalus; text-align: right; font-weight: 700; font-size: small;">Total</asp:Label>               
            &nbsp;
                        <asp:Label ID="Label_Total_Amount_Value" runat="server" style="font-size: small"></asp:Label>
                                            &nbsp;<asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Curr" runat="server" 
                                                style="font-family: Andalus">
                                            </asp:DropDownList>
                        </ContentTemplate>
                        </asp:UpdatePanel>

                    </td>
                    </tr>
                    <tr>
                    <td align="left">
                        <br />
                        <asp:Label ID="Label_po_ack_by" runat="server" Text="P.O. Acknowledged By:" 
                            style="font-size: small"></asp:Label>
                        <br />
                        <br />
                        <asp:Label ID="Label_Vendor_Ack_Name" runat="server" Text="NAME:" 
                            style="font-size: small"></asp:Label>
                        <br />
                        <br />
                        <asp:Label ID="Label_Vendor_Ack_Name0" runat="server" Text="DATE:" 
                            style="font-size: small"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:Label ID="Label_Buyer_Name" runat="server" Text="BUYER:" 
                            style="font-size: small"></asp:Label>
                        <br />
                        <br />
                        <asp:Label ID="Label_Buyer_Date" runat="server" Text="DATE:" 
                            style="font-size: small"></asp:Label>
                        <br />
                        <br />
                        <asp:Label ID="Label_PO_No" runat="server" ForeColor="Red" 
                            style="font-size: xx-small"></asp:Label>
                        <br />
                    </td>
                    </tr>
                    </table>
    
                    <asp:Button ID="Button_Create_PO" runat="server" class="btn btn-sm btn-success"
                        onclick="Button_Create_Req_Click" 
                        style="font-family: Andalus" Text="Submit!" />
                    <br />
                        <asp:Label ID="Label_PO_Creation_Stat" runat="server" 
            Visible="False" style="font-size: small"></asp:Label>
    
    </div>
    </form>
</body>
</html>
