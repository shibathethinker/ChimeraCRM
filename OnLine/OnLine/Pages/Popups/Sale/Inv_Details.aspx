<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Inv_Details.aspx.cs" Inherits="OnLine.Pages.Popups.Sale.Inv_Details" %>

<%@ Register assembly="EO.Web" namespace="EO.Web" tagprefix="eo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link type="text/css" rel="stylesheet" href="~/Styles/GridViewStyle.css" />
        <link type="text/css" rel="stylesheet" href="~/css/bootstrap.css" /> 
<link type="text/css" rel="stylesheet" href="~/css/normalize.css" />
    <title></title>
    <style type="text/css">
        .style1
        {
            font-family: Andalus;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
<div style="height: 1440px; font-family: Andalus;" align="center">
    
        <asp:Label ID="Label1" runat="server" Text="Invoice" 
            style="font-family: Andalus; font-size: large;"></asp:Label>
    
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    
        <eo:ASPXToPDF ID="ASPXToPDF1" runat="server">
        </eo:ASPXToPDF>
    
        <br />
        <br />
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
        <td align="left">
        </td>
        <td align="right">
            Invoice#
                    <asp:Label ID="Label_INV_No" runat="server" 
            style="font-family: Andalus; text-align: right"></asp:Label>               
            <br />
            Date:
                               <asp:TextBox class="form-control" ID="TextBox_Inv_Date" runat="server"  
                                Width="169px" ValidationGroup="LeadDueDate"></asp:TextBox>
                            <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_Inv_Date_CalendarExtender" 
                                runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton3" 
                                TargetControlID="TextBox_Inv_Date">
                            </ajaxtoolkit:CalendarExtender>               
        </td>
        </tr>
        <tr>
        <td align="left">
                            <asp:Image ID="Image_Logo_Vendor" runat="server"  ImageAlign="Middle" 
                        Visible="False" Height="89px" Width="93px" />
                            <br />
                    <asp:Label ID="Label_Vendor_Name" runat="server" 
            style="font-family: Andalus; text-align: right"></asp:Label>               
                            <br />
            <asp:Label ID="Label_Vendor_Addr" runat="server" style="font-family: Andalus"></asp:Label>
                        <br />
        </td>
        <td align="right">
                            <asp:Image ID="Image_Logo_Client" runat="server"  ImageAlign="Middle" 
                        Visible="False" Height="89px" Width="93px" />
                            &nbsp;
                    <br />
                    <asp:Label ID="Label_To" runat="server" 
            style="font-family: Andalus; text-align: right; font-weight: 700;">To</asp:Label>               
                    <br />
                    <asp:Label ID="Label_Client_Name" runat="server" 
            style="font-family: Andalus; text-align: right"></asp:Label>               
            <br />
        <asp:Label ID="Label_Client_Addr" runat="server" style="font-family: Andalus"></asp:Label>
            <br />
        </td>
        </tr>
        </table>
        <br />

    
        <br />
        <asp:Label ID="Label_Ship_Via" runat="server" style="font-family: Andalus">Ship Via:</asp:Label>
                        &nbsp;<asp:TextBox class="form-control" ID="TextBox_Ship_Via" runat="server" style="font-family: Andalus" 
            Width="248px"></asp:TextBox>
        <br />
        <br />
        <asp:Label ID="Label_Policy_No" runat="server" style="font-family: Andalus">Policy:</asp:Label>
                        &nbsp;<asp:TextBox class="form-control" ID="TextBox_Policy_No" runat="server" style="font-family: Andalus" 
            Width="248px"></asp:TextBox>
        <br />
        <br />
                    <br />
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                    <asp:GridView ID="GridView1" 
                        runat="server" 
                    AutoGenerateColumns="False" CellPadding="4" 
                    GridLines="None" 
                                    CssClass="mGrid"
            PagerStyle-CssClass="pgr"
            AlternatingRowStyle-CssClass="alt"
            SelectedRowSyle-CssClass="sel"
                    onrowdatabound="GridView1_RowDataBound" 
                    PageSize="5" Visible="False" 
                        style="font-size: small" Height="50%" Width="90%" 
                        Font-Size="Medium">
                    <AlternatingRowStyle BackColor="#F1FBFF" />
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
                                        <asp:TextBox class="form-control" ID="TextBox_Qnty" runat="server" AutoPostBack="True" style="font-family: Andalus" 
                                            Text='<%# Eval("Qnty") %>' BackColor="Transparent" BorderStyle="None" 
                                            ReadOnly="True"></asp:TextBox>
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
                                        <asp:TextBox class="form-control" ID="TextBox_Unit_Price" runat="server" AutoPostBack="True" style="font-family: Andalus" 
                                            Text='<%# Eval("Unit_Price") %>' MaxLength="14" BackColor="Transparent" 
                                            BorderStyle="None" ReadOnly="True"></asp:TextBox>
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
                        <PagerStyle CssClass="pgr" />
                    <SelectedRowStyle BackColor="#B9E5FB" Font-Bold="True" ForeColor="#00488F" />
                </asp:GridView>
                    </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                    <td align="left" class="style1">
                                                                    <asp:Label ID="Label_Comments" runat="server" 
            style="font-family: Andalus; text-align: right; font-weight: 700;">Comments:</asp:Label>               
                        <br />
                        <asp:TextBox class="form-control" ID="TextBox_TnC" runat="server"
                            TextMode="MultiLine" Width="353px" Height="190px"></asp:TextBox>
                        <br />
                        <br />
                    </td>

                    <td align="right" class="style1">


                                            <asp:Label ID="Label_Sub_Total" runat="server" 
            style="font-family: Andalus; text-align: right; font-weight: 700;">Sub-Total</asp:Label>               
            &nbsp;<asp:Label ID="Label_Sub_Total_Amount_Value" runat="server"></asp:Label>
                                            <br />



                        <asp:Label ID="Label_No_Tax_Comp_Warning" runat="server" ForeColor="Red" 
                            style="font-size: small" Visible="False"></asp:Label>
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                                                                    <asp:Label ID="Label_Sub_Total0" runat="server" 
            style="font-family: Andalus; text-align: right; font-weight: 700;">Taxable-Amount</asp:Label>               
            &nbsp; <asp:TextBox class="form-control" ID="TextBox_Taxable_Amount" runat="server" style="font-family: Andalus" 
            Width="169px" AutoPostBack="True" ontextchanged="TextBox_Taxable_Amount_TextChanged" MaxLength="14"></asp:TextBox>
                                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator_Taxable_Amnt" 
                                                                        runat="server" ControlToValidate="TextBox_Taxable_Amount" Display="Dynamic" 
                                                                        ErrorMessage="Please enter valid number (maximum 11 digits before decimal and 2 after decimal)" 
                                                                        ForeColor="Red" ValidationExpression="^(?:[1-9]\d{0,10})?(?:\.\d{0,2})?$" 
                                                                        ValidationGroup="Lead_Validate"></asp:RegularExpressionValidator>
        &nbsp;<br />
                        <br />
                                <asp:GridView ID="GridView_Inv_Tax_Comp" runat="server" 
                                    AutoGenerateColumns="False" CellPadding="4" CssClass="table table-striped table-bordered table-hover tableShadow"  
                                    GridLines="None" PageSize="3" 
                                        Visible="False" 
                                        onrowdeleting="GridView_Inv_Tax_Comp_RowDeleting" 
                            style="font-size: small">
                                    <Columns>
                                        <asp:CommandField ShowDeleteButton="True" />
                                        <asp:TemplateField HeaderText="Hidden">
                                            <ItemTemplate>
                                                <asp:Label ID="Label_Hidden" runat="server" Text='<%# Eval("Hidden") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Components    ">
                                            <ItemTemplate>
                                                <asp:Label ID="Label_Name" runat="server" Text='<%# Eval("Comp_Name") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Value (%)">
                                            <ItemTemplate>
                                                <asp:TextBox class="form-control" ID="TextBox_Value" runat="server" style="font-family: Andalus" 
                                                    Text='<%# Eval("Comp_Value") %>' AutoPostBack="True" 
                                                    ontextchanged="TextBox_Value_TextChanged" MaxLength="6"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator_Tax_Percentage" 
                                                    runat="server" ControlToValidate="TextBox_Value" Display="Dynamic" 
                                                    ErrorMessage="Please enter valid percentage" ForeColor="Red" 
                                                    ValidationExpression="^(?:[0-9]\d{0,2})?(?:\.\d{0,2})?$" 
                                                    ValidationGroup="Lead_Validate"></asp:RegularExpressionValidator>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
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
                                                                    <asp:Label ID="Label_Inv_Tax_Comp_Changed" runat="server" 
                                                                        style="font-size: small" Visible="False"></asp:Label>
                                                                    <br />
                                                                    <asp:Button ID="Button_Show_All_Tax_Comp_List" runat="server" class="btn btn-sm btn-success"
                                                                        onclick="Button_Show_All_Tax_Comp_List_Click" style="font-family: Andalus" 
                                                                        Text="Add More Tax Components!" />
                        <br />
                    <asp:Label ID="Label_Amount" runat="server" 
            style="font-family: Andalus; text-align: right; font-weight: 700;">Total</asp:Label>               
&nbsp;<asp:Label ID="Label_Curr" runat="server" style="font-family: Andalus; text-align: right; font-weight: 700;"></asp:Label>
&nbsp;
                        <asp:Label ID="Label_Total_Amount_Value" runat="server"></asp:Label>
                                                </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    </tr>
                    </table>
    
                    <asp:Button ID="Button_Create_Inv" runat="server" class="btn btn-sm btn-success"
                        onclick="Button_Create_Inv_Click" 
                        style="font-family: Andalus" Text="Submit!" />
                    &nbsp;<asp:Button ID="Button_Update_Inv" runat="server" class="btn btn-sm btn-success"
                        onclick="Button_Update_Inv_Click" 
                        style="font-family: Andalus" Text="Update!" />
                    <asp:Button ID="Button_Render" runat="server" class="btn btn-sm btn-success"
            onclick="Button_Render_Click" style="font-family: Andalus" Text="Render" />
                    <br />
                        <asp:Label ID="Label_INV_Creation_Stat" runat="server" 
            Visible="False"></asp:Label>
    
    </div>
    <div align="center" id="hover" 
        style="position:fixed; right:300px; top:100px; z-index:20; width:298px; height:auto;">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
        <asp:Panel ID="Panel_Select_Tax_Comp" runat="server" 
        style="font-family: Andalus" Visible="False">
                <div class="panel panel-danger">
        <div class="panel-heading">
        <h3 class="panel-title">Existing Tax Components for Invoice</h3>
        </div>
        <div class="panel-body">
            <div align="center">

                <asp:GridView ID="GridView_Inv_Tax_Complete_List" runat="server" AllowPaging="True" 
                    AutoGenerateColumns="False" CellPadding="4" CssClass="table table-striped table-bordered table-hover tableShadow" 
                    GridLines="None" 
                        onpageindexchanging="GridView_Inv_Tax_Complete_List_PageIndexChanging" PageSize="3" 
                    style="font-size: x-small" 
                    
                    onselectedindexchanged="GridView_Inv_Tax_Complete_List_SelectedIndexChanged" 
                    Visible="False">
                    <Columns>
                        <asp:CommandField SelectText="Add to the Invoice" ShowSelectButton="True" />
                        <asp:TemplateField HeaderText="Hidden">
                            <ItemTemplate>
                                <asp:Label ID="Label_Hidden" runat="server" Text='<%# Eval("Hidden") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Component Name">
                            <ItemTemplate>
                                <asp:Label ID="Label_Name" runat="server" Text='<%# Eval("Comp_Name") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Value">
                            <ItemTemplate>
                                <asp:Label ID="Label_Value" runat="server" Text='<%# Eval("Comp_Value") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox class="form-control" ID="TextBox_Value_Edit" runat="server" 
                                    style="font-family: Andalus" Text='<%# Eval("Comp_Value") %>'></asp:TextBox>
                            </EditItemTemplate>
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
                                
                <asp:Label ID="Label_Tax_Comp_Addn_Stat" runat="server" 
                    style="font-size: small" Visible="False"></asp:Label>
                                
                <br />
                <asp:Button ID="Button_Hide_Complete_Tax_Comp_List" runat="server" Text="Hide!" class="btn btn-sm btn-success"
                style="text-align: center; font-family: Andalus;" 
                    onclick="Button_Hide_Complete_Tax_Comp_List_Click" /></div>
                    </div>
                    </div>
            </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
