<%@ Page Title="" Language="C#" Theme="ThemeBlue" MasterPageFile="~/Pages/Site1.Master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="OnLine.Pages.Products" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
        .style6
        {
            width: 90%;        
            font-size="small";    
           /* border: 1px solid #c0c0c0;*/
        }
        .style7
        {
            font-size: small;
        }
        .style8
        {
            width: 100%;
            border-collapse: collapse;
            border-bottom-color: "#0066CC";
            font-size: small;
            border-left-width: thin;
            border-right-width: thin;
            border-top-width: thin;
            border-bottom-width: thin;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHeader" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderBody" runat="server">
<script language="javascript">

    function ClearAllControls() {
        for (i = 0; i < document.forms[0].length; i++) {
            doc = document.forms[0].elements[i];
            switch (doc.type) {
                case "text":
                    doc.value = "";
                    break;

                case "checkbox":
                    doc.checked = false;
                    break;

                case "radio":
                    doc.checked = false;
                    break;

                case "select-one":
                    doc.options[doc.selectedIndex].selected = false;
                    doc.value = "_";
                    break;

                case "select-multiple":
                    while (doc.selectedIndex != -1) {
                        indx = doc.selectedIndex;
                        doc.options[indx].selected = false;
                    }
                    doc.selected = false;
                    break;
                default:
                    break;
            }
        }
    }

    function Selrdbtn(id) {
        var rdBtn = document.getElementById(id);
        var List = document.getElementsByTagName("input");
        for (i = 0; i < List.length; i++) {
            if (List[i].type == "radio" && List[i].id != rdBtn.id) {
                List[i].checked = false;
            }
        }
    }

    function unSelrdbtn() {
        var List = document.getElementsByTagName("input");
        for (i = 0; i < List.length; i++) {
            if (List[i].type == "radio") {
                List[i].checked = false;
            }
        }
    }
</script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
                        <asp:Label ID="Label_Product_Screen_Access" runat="server" ForeColor="Red" 
                            style="font-size: small; font-family: Andalus;" 
        Visible="False"></asp:Label>
                  <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title">All Products</h3>
            </div>
            <div class="panel-body">  
    <asp:Panel ID="Panel_All_Products" runat="server" 
            style="font-family: Andalus">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
            <div align="center">

                                      <div class="panel panel-success">
            <div class="panel-heading">
              <h3 class="panel-title">Filter By</h3>
            </div>
            <div class="panel-body">
                            <table class="style6">
                                <tr>
                                    <td align="center">
                                        Category:
                                        <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Category" runat="server" AutoPostBack="True" 
                                            onselectedindexchanged="DropDownList_Category_SelectedIndexChanged" 
                                            style="font-family: Andalus">
                                        </asp:DropDownList>
                                        &nbsp; Child Category:
                                        <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Child_Category" runat="server" 
                                            onselectedindexchanged="DropDownList_Child_Category_SelectedIndexChanged" 
                                            style="font-family: Andalus">
                                        </asp:DropDownList>
                                        &nbsp; <asp:Button ID="Button_Filter_All_Prod" runat="server" 
                                            onclick="Button_Filter_All_Prod_Click" style="font-family: Andalus"  class="btn btn-sm btn-success"
                                            Text="Filter" ValidationGroup="DueDate" />
                                    </td>
                                </tr>
                            </table>
               </div>
            </div>               
              
            </div>
                            </ContentTemplate>
                </asp:UpdatePanel>
                <br />
            <div align="center">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="GridView1" 
                        runat="server" AllowPaging="True" 
                    AutoGenerateColumns="False" CellPadding="4" 
                    GridLines="None" 
                                        CssClass="table table-striped table-bordered table-hover tableShadow" 
                                         Visible="False" 
                        onselectedindexchanged="GridView1_SelectedIndexChanged" 
                        style="font-size: small" Height="30%" Width="90%" 
                        Font-Size="Medium" onpageindexchanged="GridView1_PageIndexChanged" 
                            onpageindexchanging="GridView1_PageIndexChanging" 
                            onrowediting="GridView1_RowEditing" onrowupdating="GridView1_RowUpdating" 
                            onrowcancelingedit="GridView1_RowCancelingEdit" 
                            onrowdatabound="GridView1_RowDataBound">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:RadioButton ID="prod_radio" runat="server" AutoPostBack="true" 
                                            GroupName="rain" OnCheckedChanged="GridView_Prod_RadioSelect" 
                                            OnClick="javascript:Selrdbtn(this.id)" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField ShowEditButton="True" />
                                <asp:TemplateField HeaderText="Hidden">
                                    <ItemTemplate>
                                        <asp:Label ID="Label_Hidden" runat="server" Text='<%# Eval("ProdCatId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Product Name">
                                    <EditItemTemplate>
                                        <asp:TextBox class="form-control" ID="TextBox_Prod_Name_Edit" runat="server" 
                                            style="font-family: Andalus" Text='<%# Eval("ProdName") %>'></asp:TextBox>
                                        <asp:Label ID="Label_Name_Hidden" runat="server" Text='<%# Eval("ProdName") %>' 
                                            Visible="False"></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label_Name" runat="server" Text='<%# Eval("ProdName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Specifications">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton_Show_Spec" runat="server" 
                                            onclientclick="javascript:unSelrdbtn()" 
                                            oncommand="LinkButton_Show_Spec_Command">Show!</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Inventory Stock">
                                    <EditItemTemplate>
                                        <asp:TextBox class="form-control" ID="TextBox_Invt_Stock_Edit" runat="server" 
                                            style="font-family: Andalus" Text='<%# Eval("Stock") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label_Invt_Stock" runat="server" Text='<%# Eval("Stock") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Measurement Unit">
                                    <ItemTemplate>
                                        <asp:Label ID="Label_Msrmnt" runat="server" Text='<%# Eval("msrmnt") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Currency">
                                    <EditItemTemplate>
                                        <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Curr" runat="server" 
                                            style="font-family: Andalus">
                                        </asp:DropDownList>
                                        <asp:Label ID="Label_Curr_Edit" runat="server" Text='<%# Eval("curr") %>' 
                                            Visible="False"></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label_Curr" runat="server" Text='<%# Eval("curr") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Unit Source Price">
                                    <EditItemTemplate>
                                        <asp:TextBox class="form-control" ID="TextBox_Unit_Src_Prc_Edit" runat="server" 
                                            style="font-family: Andalus" Text='<%# Eval("srcPrice") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label_Unit_Src_Prc" runat="server" 
                                            Text='<%# Eval("srcPrice") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Unit List Price">
                                    <EditItemTemplate>
                                        <asp:TextBox class="form-control" ID="TextBox_Unit_Lst_Price_Edit" runat="server" 
                                            style="font-family: Andalus" Text='<%# Eval("listPrice") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label_Unit_Lst_Price" runat="server" 
                                            Text='<%# Eval("listPrice") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sales Orders">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton_Show_SO" runat="server" 
                                            oncommand="LinkButton_Show_SO_Command" 
                                            onclientclick="javascript:unSelrdbtn()">Show!</asp:LinkButton>
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
                        <asp:Label ID="Label_Product_Grid_Access" runat="server" ForeColor="Red" 
                            style="font-size: small" Visible="False"></asp:Label>
                        <br />
                                                                                       <asp:Button ID="Button_Prod_Refresh_Hidden" runat="server" style="display:none" 
                            ForeColor="#336600" onclick="Button_Prod_Refresh_Hidden_Click" 
                            Text="Hidden" />
                        <asp:Button ID="Button_Prod_Refresh" runat="server" ForeColor="#336600" 
                            onclick="Button_Prod_Refresh_Click" 
                            style="font-family: Andalus; font-size: x-small" Text="Refresh Data!" />
                        &nbsp;<asp:Button ID="Button_Create_Product" runat="server"  class="btn btn-sm btn-success"
                            onclientclick="window.open('createProduct.aspx','CreateProd','width=1000,height=1000,left=100,right=500,scrollbars=1',true);" 
                            style="font-family: Andalus" Text="Create New Product!" />
                        &nbsp;<asp:Button ID="Button_Notes_Prod" runat="server" Enabled="False"  class="btn btn-sm btn-success"
                            onclick="Button_Notes_Prod_Click" style="font-family: Andalus" Text="Notes!" />
                        &nbsp;<asp:Button ID="Button_Audit_Prod" runat="server" Enabled="False"  class="btn btn-sm btn-success"
                            onclick="Button_Audit_Prod_Click" style="font-family: Andalus" Text="Logs!" />
                    </ContentTemplate>
                </asp:UpdatePanel>
               </div>
    </asp:Panel>
            </div>
        </div>
            <div align="center" id="hover" 
        style="position:fixed; right:10px; top:250px; z-index:20; height:auto;">
        <asp:Button ID="Button_Clear_Filter_All" runat="server" 
                                                                        class="btn btn-sm btn-danger"
                                                                        style="font-family: Andalus" Text="Clear Filter" 
                                                                        onclientclick="ClearAllControls();return false;" />
        </div>
        <br />
</asp:Content>
