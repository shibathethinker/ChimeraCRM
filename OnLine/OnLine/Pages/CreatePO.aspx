<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreatePO.aspx.cs" Inherits="OnLine.Pages.CreatePO" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link href="../css/bootswatch_flaty.css" rel="stylesheet" type="text/css" />
    <link href="../css/bootstrap-theme.min.css" rel="stylesheet" type="text/css" />
    <link href="../css/Custom.css?version=5" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-2.2.3.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    
<link type="text/css" rel="stylesheet" href="~/Styles/GridViewStyle.css" />
    <title></title>
        <style type="text/css">
legend {color:red}
        .style1
        {
            font-family: Andalus;
        }
        </style>
        <script language="javascript">
            function RefreshParent() {
                window.opener.document.getElementById('ContentPlaceHolderBody_Button_PO_Refresh_Hidden').click();
            }
            
            function OnProdSelected(source, eventArgs) {

                var hdnValueID = "<%= hdnValueProd.ClientID %>";

                document.getElementById(hdnValueID).value = eventArgs.get_value();
                __doPostBack(hdnValueID, "");
            } 
        </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="height: 1323px; font-family: Andalus;" align="center">
    
        <asp:Label ID="Label1" runat="server" Text="Purchase Order#" 
            style="font-family: Andalus; font-size: large;"></asp:Label>   
        <asp:Label ID="Label_PO_No" runat="server" ForeColor="Red" 
                            style="font-size: large"></asp:Label>
        
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    <br />
                  
    
        <br />
        <br />
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
        <td align="left">
                            <asp:Image ID="Image_Logo" runat="server"  ImageAlign="Middle" 
                        Visible="False" Height="89px" Width="93px" />
                            <br />
                    <asp:Label ID="Label_Client_Name" runat="server" 
            style="font-family: Andalus; text-align: right"></asp:Label>               
                            <br />
        <asp:Label ID="Label_Client_Addr" runat="server" style="font-family: Andalus"></asp:Label>
                        <br />
        </td>
        <td align="right">
            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                    <asp:Label ID="Label_To" runat="server" 
            style="font-family: Andalus; text-align: right; font-weight: 700;">To</asp:Label>               
                    <br />
                    Select Account:
            <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Contacts" runat="server" AutoPostBack="True" 
                onselectedindexchanged="DropDownList_Contacts_SelectedIndexChanged" 
                style="font-family: Andalus">
            </asp:DropDownList>
                    <asp:Label ID="Label_Contact_Required" runat="server" ForeColor="Red" 
                        Visible="False">Mandatory</asp:Label>
                    <br />
                    <asp:Label ID="Label_Vendor_Name" runat="server" 
            style="font-family: Andalus; text-align: right"></asp:Label>               
            <br />
            <asp:Label ID="Label_Vendor_Addr" runat="server" style="font-family: Andalus"></asp:Label>
            <br />
                        </ContentTemplate>
            </asp:UpdatePanel>
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
                        <asp:Button ID="Button_Add_Prod_Srv" runat="server" 
            Text="Add Product Details" class="btn btn-sm btn-success"
                            style="font-family: Andalus; font-size: small" 
                            onclick="Button_Add_Prod_Srv_Click" />
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
                    PageSize="3" Visible="False" 
                        style="font-size: small" Height="50%" Width="90%" 
                            onrowdatabound="GridView1_RowDataBound" 
                            onrowediting="GridView1_RowEditing" onrowupdating="GridView1_RowUpdating" 
                            onrowcancelingedit="GridView1_RowCancelingEdit" 
                            onrowdeleting="GridView1_RowDeleting" >
                        <AlternatingRowStyle CssClass="alt" />
                    <Columns>
                        <asp:CommandField ShowEditButton="True" Visible="False" />
                        <asp:CommandField ShowDeleteButton="True" />
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
                                <asp:UpdatePanel ID="UpdatePanel_GridView1_Qnty_Edit" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox class="form-control" ID="TextBox_Qnty_Edit" runat="server" AutoPostBack="True" 
                                            ontextchanged="TextBox_Qnty_TextChanged_Edit" style="font-family: Andalus" 
                                            Text='<%# Eval("Qnty") %>'></asp:TextBox>
                                        <asp:Label ID="Label_Qnty_Format_Edit" runat="server" ForeColor="Red" 
                                            Text="Invalid data format" Visible="False"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:UpdatePanel ID="UpdatePanel_GridView1_Qnty" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox class="form-control" ID="TextBox_Qnty" runat="server" AutoPostBack="True" 
                                            ontextchanged="TextBox_Qnty_TextChanged" style="font-family: Andalus" 
                                            Text='<%# Eval("Qnty") %>' Enabled="False"></asp:TextBox>
                                        <asp:Label ID="Label_Qnty_Format" runat="server" ForeColor="Red" 
                                            Text="Invalid data format" Visible="False"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Product Details">
                            <EditItemTemplate>
                                <asp:Label ID="Label_Product_Name_Edit" runat="server" 
                                    Text='<%# Eval("Prod_Name") %>' Visible="False"></asp:Label>
                                <asp:DropDownList class="form-control feature-select-dropdown" ID="DropDownList_Edit_ProdName" runat="server" 
                                    AutoPostBack="True" 
                                    onselectedindexchanged="DropDownList_Edit_ProdName_SelectedIndexChanged" 
                                    style="font-family: Andalus; font-size: small">
                                </asp:DropDownList>
                            </EditItemTemplate>
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
                                                <asp:Label ID="Label_Hidden0" runat="server" Text='<%# Eval("Hidden") %>'></asp:Label>
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
                                <asp:UpdatePanel ID="UpdatePanel_GridView1_UnitPrice_Edit" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox class="form-control" ID="TextBox_Unit_Price_Edit" runat="server" AutoPostBack="True" 
                                            ontextchanged="TextBox_Unit_Price_TextChanged_Edit" 
                                            style="font-family: Andalus" Text='<%# Eval("Unit_Price") %>'></asp:TextBox>
                                        <asp:Label ID="Label_Unit_Format_Edit" runat="server" ForeColor="Red" 
                                            Text="Invalid data format" Visible="False"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:UpdatePanel ID="UpdatePanel_GridView1_UnitPrice" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox class="form-control" ID="TextBox_Unit_Price" runat="server" AutoPostBack="True" 
                                            ontextchanged="TextBox_Unit_Price_TextChanged" style="font-family: Andalus" 
                                            Text='<%# Eval("Unit_Price") %>' Enabled="False"></asp:TextBox>
                                        <asp:Label ID="Label_Unit_Format" runat="server" ForeColor="Red" 
                                            Text="Invalid data format" Visible="False"></asp:Label>
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
            style="font-family: Andalus; text-align: right; font-weight: 700;">Sub-Total</asp:Label>               
            &nbsp;<asp:Label ID="Label_Sub_Total_Amount_Value" runat="server"></asp:Label>
&nbsp;
                        <br />
                    <asp:Label ID="Label_Sub_Total0" runat="server" 
            style="font-family: Andalus; text-align: right; font-weight: 700;">Tax %</asp:Label>               
            &nbsp;
                        <asp:TextBox class="form-control" ID="TextBox_tax" runat="server" style="font-family: Andalus" AutoPostBack="True"  
                                                ontextchanged="TextBox_tax_TextChanged"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                                ControlToValidate="TextBox_tax" Display="Dynamic" 
                                                ErrorMessage="Please enter valid percentage" ForeColor="Red" 
                                                ValidationGroup="CreatePO"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator_Tax_Percentage" 
                                                runat="server" ControlToValidate="TextBox_tax" Display="Dynamic" 
                                                ErrorMessage="Please enter valid percentage" ForeColor="Red" 
                                                ValidationExpression="^(?:[0-9]\d{0,2})?(?:\.\d{0,2})?$"></asp:RegularExpressionValidator>
                        <br />
                    <asp:Label ID="Label_Amount0" runat="server" 
            style="font-family: Andalus; text-align: right; font-weight: 700;">Total</asp:Label>               
            &nbsp;
                        <asp:Label ID="Label_Total_Amount_Value" runat="server"></asp:Label>
                                            <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Curr" runat="server" 
                                                style="font-family: Andalus">
                                            </asp:DropDownList>
                        </ContentTemplate>
                        </asp:UpdatePanel>

                    </td>
                    </tr>
                    <tr>
                    <td align="left">
                        <br />
                        <asp:Label ID="Label_po_ack_by" runat="server" Text="P.O. Acknowledged By:"></asp:Label>
                        <br />
                        <br />
                        <asp:Label ID="Label_Vendor_Ack_Name" runat="server" Text="NAME:"></asp:Label>
                        <br />
                        <br />
                        <asp:Label ID="Label_Vendor_Ack_Name0" runat="server" Text="DATE:"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:Label ID="Label_Buyer_Name" runat="server" Text="BUYER:"></asp:Label>
                        <br />
                        <br />
                        <asp:Label ID="Label_Buyer_Date" runat="server" Text="DATE:"></asp:Label>
                        <br />
                        <br />
                        <br />
                    </td>
                    </tr>
                    </table>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
                    <asp:Button ID="Button_Create_PO" runat="server" class="btn btn-sm btn-success" 
                        style="font-family: Andalus" Text="Submit" 
                        onclick="Button_Create_PO_Click" ValidationGroup="CreatePO" />
        <br />
                    &nbsp;<asp:Label ID="Label_Contact_Locality_Id_Hidden" runat="server" 
                        Visible="False"></asp:Label>
                    <br />
                        <asp:Label ID="Label_PO_Creation_Stat" runat="server" 
            Visible="False"></asp:Label>
    
        <asp:Label ID="Label_Deal_Status" runat="server" Visible="False"></asp:Label>
    
        <br />
                        <asp:Label ID="Label_Product_Uniq_Name_List" runat="server" 
            Visible="False"></asp:Label>
         </ContentTemplate>
        </asp:UpdatePanel>
                    
    </div>
    <div align="center" id="hover"
        
        style="position:fixed;top:50px ; z-index:20; Width:70%; height:auto; left: 0px;">
            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <ContentTemplate>
        <asp:Panel ID="Panel_Prod_Service_Det" runat="server" 
            style="font-family: Andalus; font-size: small; margin-left: 0px; margin-top: 0px;" 
               Visible="False">
                                           <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title">Product Service Details</h3>
            </div>
            <div align="center" class="panel-body" style="background-color:#F3F3F3">
                        <asp:Panel ID="Panel3" runat="server">
                            * Mention the product category or select existing product below as defined in 
                            your product catalog*
                            <br />
                            Level1:
                            <asp:DropDownList class="form-control feature-select-dropdown" ID="DropDownList_Level1" runat="server" AutoPostBack="True" 
                                onselectedindexchanged="DropDownList_Level1_SelectedIndexChanged" 
                                style="font-family: Andalus">
                            </asp:DropDownList>
                            &nbsp; Level2:
                            <asp:DropDownList class="form-control feature-select-dropdown" ID="DropDownList_Level2" runat="server" AutoPostBack="True" 
                                onselectedindexchanged="DropDownList_Level2_SelectedIndexChanged" 
                                style="font-family: Andalus">
                            </asp:DropDownList>
                            &nbsp;Level3:
                            <asp:DropDownList class="form-control feature-select-dropdown" ID="DropDownList_Level3" runat="server" AutoPostBack="True" 
                                onselectedindexchanged="DropDownList_Level3_SelectedIndexChanged" 
                                style="font-family: Andalus">
                            </asp:DropDownList>
                            <br />
                            <br />
                            <asp:Button ID="Buttin_Show_Spec_List1" runat="server" class="btn btn-sm btn-success"
                                onclick="Buttin_Show_Spec_List1_Click" style="font-family: Andalus" 
                                Text="Show Features and Specifications!" ValidationGroup="Panel2" />
                        </asp:Panel>
                <br />
                <asp:Label ID="Label_Extra_Spec" runat="server" 
                    Text="Specification Not Listed Here? Then Enter..." Visible="False"></asp:Label>
                &nbsp;<asp:TextBox class="form-control" ID="TextBox_Spec" runat="server"  
                    Width="169px" Visible="False"></asp:TextBox>
                &nbsp;
                <br />
                <br />
                        &nbsp;&nbsp;<br />&nbsp;<asp:GridView ID="GridView_Prod_Srv" 
                       Width="70%"
                        runat="server" AllowPaging="True" 
                    AutoGenerateColumns="False" 
                                CssClass="mGrid"
            PagerStyle-CssClass="pgr"
            AlternatingRowStyle-CssClass="alt"       
            SelectedRowStyle-CssClass="sel"
            EditRowStyle-CssClass="sel"
                    CellPadding="4" ForeColor="#333333" 
                    GridLines="None" onpageindexchanging="GridView_Prod_Srv_PageIndexChanging" 
                    PageSize="3" Visible="False" 
                        onselectedindexchanged="GridView_Prod_Srv_SelectedIndexChanged" 
                            onrowdatabound="GridView_Prod_Srv_RowDataBound">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:CommandField ShowSelectButton="True" SelectText="Add This Specification" />
                        <asp:TemplateField HeaderText="Hidden">
                            <ItemTemplate>
                                <asp:Label ID="Label_Hidden_FeatId" runat="server" 
                                    Text='<%# Eval("Hidden_Feat_Id") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Feature">
                            <ItemTemplate>
                                <asp:Label ID="Label_Feature0" runat="server" Text='<%# Eval("Feature") %>'></asp:Label>
                                <asp:Image ID="Image_Selected" runat="server" Height="16px" 
                                    ImageUrl="~/Images/tick_green.jpg" Visible="False" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="From">
                            <EditItemTemplate>
                                <asp:DropDownList class="form-control feature-select-dropdown" ID="DropDownList_Gridview1_From" runat="server" 
                                     style="font-family: Andalus">
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:DropDownList class="form-control feature-select-dropdown" ID="DropDownList_Gridview1_From" runat="server" 
                                    style="font-family: Andalus">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="To">
                            <EditItemTemplate>
                                <asp:DropDownList class="form-control feature-select-dropdown" ID="DropDownList_Gridview1_To" runat="server" 
                                        style="font-family: Andalus">
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:DropDownList class="form-control feature-select-dropdown" ID="DropDownList_Gridview1_To" runat="server" 
                                    style="font-family: Andalus">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                            <EditRowStyle CssClass="sel" />
                            <PagerStyle CssClass="pgr" />
                            <SelectedRowStyle CssClass="sel" />
                </asp:GridView>
                <br />
                <asp:Label ID="Label_Prod_Name" runat="server" 
                    Text="Product/Service Name (Provide Unique Name):"></asp:Label>
                        &nbsp;<asp:TextBox class="form-control" ID="TextBox_Prod_Name" runat="server"  
                Width="169px"></asp:TextBox>
                                            <ajaxtoolkit:AutoCompleteExtender ID="TextBox_Prod_Name_AutoCompleteExtender"  ServiceMethod="GetCompletionListProd" 
                MinimumPrefixLength="1" onclientitemselected="OnProdSelected"
                    runat="server" TargetControlID="TextBox_Prod_Name" UseContextKey="True">
                </ajaxtoolkit:AutoCompleteExtender>
                            <asp:hiddenfield id="hdnValueProd" onvaluechanged="hdnValue_ValueChangedProd" runat="server" />

                        <asp:RequiredFieldValidator ID="RequiredFieldValidator_ProdName" runat="server" 
                            ControlToValidate="TextBox_Prod_Name" ErrorMessage="RequiredFieldValidator" 
                            ForeColor="#FF3300" ValidationGroup="prodName" Display="Dynamic">Name of the product/service to be supplied by your vendor</asp:RequiredFieldValidator>
            

                        &nbsp;<asp:Label ID="Label_Invalid_Prod_Cat" runat="server" ForeColor="Red" 
                            Text="Product Category Not Set. Please define the product from Product screen or select the correct category from the above dropdown list" 
                            Visible="False"></asp:Label>
            

                        <br />
            

                        &nbsp;<asp:Label ID="Label_Prod_Qnty" runat="server" Text="Quantity:"></asp:Label>
                        &nbsp;<asp:TextBox class="form-control" ID="TextBox_Prod_Qnty" runat="server"  
                Width="169px"></asp:TextBox>
            

                        &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator_ProdName0" 
                            runat="server" ControlToValidate="TextBox_Prod_Qnty" 
                            ErrorMessage="RequiredFieldValidator" ForeColor="#FF3300" 
                            ValidationGroup="prodName">*</asp:RequiredFieldValidator>
                        &nbsp;<asp:Label ID="Label_Prod_Unit_Price" runat="server" Text="Unit Price:"></asp:Label>
                        &nbsp;<asp:TextBox class="form-control" ID="TextBox_Prod_Unit_Price" runat="server"  
                            Width="169px"></asp:TextBox>
            

                        &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator_ProdName1" 
                            runat="server" ControlToValidate="TextBox_Prod_Unit_Price" 
                            ErrorMessage="RequiredFieldValidator" ForeColor="#FF3300" 
                            ValidationGroup="prodName">*</asp:RequiredFieldValidator>
            

                        <br />
                        <br />
                        <asp:Button ID="Button_Add_Prod_Srv_Det" runat="server" class="btn btn-sm btn-success"
                            style="font-family: Andalus" 
                            Text="Add!" onclick="Button_Add_Prod_Srv_Det_Click" 
                            ValidationGroup="prodName" />
            

                        &nbsp;<asp:Button ID="Button_Add_Prod_Srv_Det_Hide" runat="server" class="btn btn-sm btn-success"
                            onclick="Button_Add_Prod_Srv_Det_Hide_Click" style="font-family: Andalus" 
                            Text="Hide!" />
   <asp:Label ID="Label_Selected_List" runat="server" Text="Label" Visible="False"></asp:Label>
   </div>
   </div>
        </asp:Panel>                   

            </ContentTemplate>
            </asp:UpdatePanel>

        </div>
    </form>
</body>
</html>
