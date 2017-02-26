<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllLead_Specification.aspx.cs" Inherits="OnLine.Pages.Popups.Sale.AllLead_Specification" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title></title>
        <link type="text/css" rel="stylesheet" href="~/css/bootstrap.css" /> 
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
    <div>
    
    <div align="center">

        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    
    <script language="javascript">
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
    
                    <asp:Label ID="Label_Create_Mode" runat="server" 
                        style="font-family: Andalus; font-size: xx-small" Visible="False"></asp:Label>
            <br />
        <br />
        <asp:Label ID="Label_Status" runat="server" Visible="False"></asp:Label>

                    <asp:GridView ID="GridView1" Width="95%" 
                        runat="server" AllowPaging="True" 
                    AutoGenerateColumns="False" CellPadding="4" 
                    GridLines="None" 
                    CssClass="table table-striped table-bordered table-hover tableShadow" 
                    PageSize="5" Visible="False" 
                        style="font-size: small; font-family: Andalus;"            
                        
            onpageindexchanging="GridView1_PageIndexChanging" 
                        onselectedindexchanged="GridView1_SelectedIndexChanged" 
                        onrowediting="GridView1_RowEditing" onrowupdating="GridView1_RowUpdating" 
                        onrowdatabound="GridView1_RowDataBound" 
                        onrowcancelingedit="GridView1_RowCancelingEdit" 
                        onrowdeleting="GridView1_RowDeleting">
                    <Columns>
                        <asp:CommandField DeleteText="" ShowEditButton="True" />
                        <asp:CommandField ShowDeleteButton="True" />
                        <asp:TemplateField HeaderText="Hidden">
                            <ItemTemplate>
                                <asp:Label ID="Label_Hidden" runat="server" Text='<%# Eval("Hidden") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Product Category">
                            <ItemTemplate>
                                <asp:Label ID="Label_Product" runat="server" 
                                    Text='<%# Eval("CategoryName") %>' style="font-family: Andalus"></asp:Label>
                                <br />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Feature">
                            <ItemTemplate>
                                <asp:GridView ID="GridView1_Inner" runat="server" AllowPaging="True" 
                                    AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" 
                                    BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" 
                                    GridLines="Horizontal" onpageindexchanging="GridView1_Inner_PageIndexChanging" 
                                    PageSize="1" onrowcancelingedit="GridView1_Inner_RowCancelingEdit" 
                                    onrowdatabound="GridView1_Inner_RowDataBound" 
                                    onrowediting="GridView1_Inner_RowEditing" 
                                    onrowupdating="GridView1_Inner_RowUpdating">
                                    <Columns>
                                        <asp:CommandField ShowEditButton="True" />
                                        <asp:TemplateField HeaderText="Hidden">
                                            <ItemTemplate>
                                                <asp:Label ID="Label_Hidden" runat="server" Text='<%# Eval("Hidden") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Feature">
                                            <ItemTemplate>
                                                <asp:Label ID="Label_Feature" runat="server" Text='<%# Eval("FeatName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="Label_Feature_Edit" runat="server" 
                                                    Text='<%# Eval("FeatName") %>'></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Spec Text">
                                            <ItemTemplate>
                                                <asp:Label ID="Label_SpecText" runat="server" Text='<%# Eval("SpecText") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox class="form-control" ID="TextBox_SpecText_Edit" runat="server" 
                                                    Text='<%# Eval("SpecText") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="From Spec">
                                            <ItemTemplate>
                                                <asp:Label ID="Label_From_Spec" runat="server" Text='<%# Eval("FromSpec") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_From_Spec_Edit" runat="server" 
                                                    style="font-family: Andalus; font-size: small">
                                                </asp:DropDownList>
                                                <br />
                                                <asp:Label ID="Label_From_Spec" runat="server" Text='<%# Eval("FromSpec") %>' 
                                                    Visible="False"></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="To Spec">
                                            <ItemTemplate>
                                                <asp:Label ID="Label_ToSpec" runat="server" Text='<%# Eval("ToSpec") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_To_Spec_Edit" runat="server" 
                                                    style="font-family: Andalus; font-size: small">
                                                </asp:DropDownList>
                                                <br />
                                                <asp:Label ID="Label_ToSpec" runat="server" Text='<%# Eval("ToSpec") %>' 
                                                    Visible="False"></asp:Label>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Image">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton_Show" runat="server" 
                                                    oncommand="Link_Feat_Img_Show_Command"  CommandArgument
                                     ="<%# Container.DataItemIndex %>" Text='<%# Eval("imgName") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:FileUpload ID="FileUpload_Image" runat="server" 
                                                    style="font-family: Andalus; font-size: x-small" />
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
                        <asp:TemplateField HeaderText="From Quantity">
                            <EditItemTemplate>
                                <asp:TextBox class="form-control" ID="TextBox_From_Qnty_Edit" runat="server" Enabled="False" 
                                    style="font-family: Andalus; font-size: small" Text='<%# Eval("FromQnty") %>'></asp:TextBox>
                                <br />
                                <asp:Label ID="Label_From_Qnty_Prev" runat="server" 
                                    style="font-family: Andalus" Text='<%# Eval("FromQnty") %>' Visible="False"></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label_From_Qnty" runat="server" style="font-family: Andalus" 
                                    Text='<%# Eval("FromQnty") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="To Quantity">
                            <EditItemTemplate>
                                <asp:TextBox class="form-control" ID="TextBox_To_Qnty_Edit" runat="server" Enabled="False" 
                                    style="font-family: Andalus; font-size: small" Text='<%# Eval("ToQnty") %>'></asp:TextBox>
                                <br />
                                <asp:Label ID="Label_To_Qnty_Prev" runat="server" style="font-family: Andalus" 
                                    Text='<%# Eval("ToQnty") %>' Visible="False"></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label_To_Qnty" runat="server" Text='<%# Eval("ToQnty") %>' 
                                    style="font-family: Andalus"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="From Price">
                            <EditItemTemplate>
                                <asp:TextBox class="form-control" ID="TextBox_From_Price_Edit" runat="server" Enabled="False" 
                                    style="font-family: Andalus; font-size: small" Text='<%# Eval("FromPrice") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label_From_Price" runat="server" style="font-family: Andalus" 
                                    Text='<%# Eval("FromPrice") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="To Price">
                            <EditItemTemplate>
                                <asp:TextBox class="form-control" ID="TextBox_To_Price_Edit" runat="server" Enabled="False" 
                                    style="font-family: Andalus; font-size: small" Text='<%# Eval("ToPrice") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label_To_Price" runat="server" 
                                    Text='<%# Eval("ToPrice") %>' style="font-family: Andalus"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Measurement Unit">
                            <ItemTemplate>
                                <asp:Label ID="Label_Msrmnt" runat="server" style="font-family: Andalus" 
                                    Text='<%# Eval("msrmntUnit") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Product Name">
                            <EditItemTemplate>
                                <asp:Label ID="Label_Prod_Name_Edit" runat="server" 
                                    Text='<%# Eval("prodName") %>' Visible="False"></asp:Label>
                                <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Prod_Name_List" runat="server" 
                                    AutoPostBack="True" 
                                    onselectedindexchanged="DropDownList_Prod_Name_List_SelectedIndexChanged" 
                                    style="font-family: Andalus; font-size: small">
                                </asp:DropDownList>                                        
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label_Prod_Name" runat="server" Text='<%# Eval("prodName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Quote">
                            <ItemTemplate>
                                <asp:Label ID="Label_Quote" runat="server" Text='<%# Eval("Quote") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox class="form-control" ID="TextBox_Quote" runat="server" style="font-family: Andalus" 
                                    Text='<%# Eval("Quote") %>'></asp:TextBox>
                                &nbsp;<br />
                                <asp:Label ID="Label_Quote_Prev" runat="server" Text='<%# Eval("Quote") %>' 
                                    Visible="False"></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total">
                            <ItemTemplate>
                                <asp:Label ID="Label_Total" runat="server" Text='<%# Eval("Total") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="All Audit Records">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton_Show_Audit" runat="server" 
                                    oncommand="LinkButton_Show_Audit_Command">Show!</asp:LinkButton>
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
                <div align="right">
                    <asp:Label ID="Label_Total_Text" runat="server" Text="Total AMount:" 
                            ></asp:Label>
                    &nbsp;<asp:Label ID="Label_Total" runat="server" ></asp:Label>
                    </div> 

                    <br />                    
                    <div>
                        <asp:LinkButton ID="LinkButton_All_Comm" runat="server" 
                            onclick="LinkButton_All_Comm_Click" style="font-family: Andalus">Show All Communications!</asp:LinkButton>
                    </div>       
    </div>
    
    </div>
    <p>
    <div align="center">
                                     <div class="panel panel-primary" Width="95%">
        <div class="panel-heading">
            <h3 class="style3">Add Product/Service Details</h3>
            </div>
            <div class="panel-body">     
    <asp:Panel ID="Panel2" runat="server" 
        style="font-family: Andalus">
        <br />
                <div class="panel-info" Width="95%">
        <div class="panel-heading">
            <h3 class="panel-title">Product/Service Details</h3>
            </div>
            <div class="panel-body">    
        <asp:Panel ID="Panel_Prod_Service_Det" runat="server" 
            style="font-family: Andalus" >
            <div class="style2">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                                                                 </ContentTemplate>
                </asp:UpdatePanel>
                        <asp:Panel ID="Panel3" runat="server">
                            Level1:
                            <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Level1" runat="server" AutoPostBack="True" 
                                onselectedindexchanged="DropDownList_Level1_SelectedIndexChanged" 
                                style="font-family: Andalus">
                            </asp:DropDownList>
                            &nbsp; Level2:
                            <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Level2" runat="server" AutoPostBack="True" 
                                onselectedindexchanged="DropDownList_Level2_SelectedIndexChanged" 
                                style="font-family: Andalus">
                            </asp:DropDownList>
                            &nbsp;Level3:
                            <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Level3" runat="server" AutoPostBack="True" 
                                onselectedindexchanged="DropDownList_Level3_SelectedIndexChanged" 
                                style="font-family: Andalus">
                            </asp:DropDownList>
                            &nbsp;&nbsp;
                            <asp:Button ID="Buttin_Show_Spec_List" runat="server" class="btn btn-sm btn-success"
                                onclick="Buttin_Show_Spec_List_Click" style="font-family: Andalus" 
                                Text="Show Features and Specifications!" ValidationGroup="Panel2" />
                        </asp:Panel>
                <br />
                <asp:Label ID="Label_Extra_Spec" runat="server" 
                    Text="Specification Not Listed Here? Then Enter..." Visible="False"></asp:Label>
                &nbsp;<asp:TextBox class="form-control" ID="TextBox_Spec" runat="server"  
                    Width="50%" Visible="False" TextMode="MultiLine"></asp:TextBox>
                <br />
                <br />
                <asp:Label ID="Label_Extra_Spec_upload" runat="server" 
                    Text="and upload picture if required" Visible="False"></asp:Label>
                &nbsp;<asp:FileUpload ID="FileUpload_Extra_Spec" runat="server"  
                    ViewStateMode="Enabled" Visible="False" />
                <br />
                <asp:Label ID="Label_Feat_Exists" runat="server" Text="Label" Visible="False"></asp:Label>
                <br />
                <div align="center">
                    <asp:GridView ID="GridView2" runat="server" AllowPaging="True" Width="50%"
                    AutoGenerateColumns="False" CellPadding="4" 
                                                    CssClass="table table-striped table-bordered table-hover tableShadow" 
                    GridLines="None" onpageindexchanging="GridView2_PageIndexChanging" 
                    onrowdatabound="GridView2_RowDataBound" onrowupdating="GridView2_RowUpdating" 
                    PageSize="3" Visible="False" 
                        onselectedindexchanged="GridView2_SelectedIndexChanged">

                    <Columns>
                        <asp:CommandField SelectText="Add To Lead" ShowSelectButton="True" />
                        <asp:TemplateField HeaderText="Hidden">
                            <ItemTemplate>
                                <asp:Label ID="Label_Hidden" runat="server" 
                                    Text='<%# Eval("Hidden_Feat_Id") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Feature">
                            <ItemTemplate>
                                <asp:Label ID="Label_Feature" runat="server" Text='<%# Eval("Feature") %>'></asp:Label>
                                <asp:Image ID="Image_Selected" runat="server" Height="16px" 
                                    ImageUrl="~/Images/tick_green.jpg" Visible="False" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="From">
                            <EditItemTemplate>
                                <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Gridview1_From" runat="server" 
                                    style="font-family: Andalus">
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Gridview1_From" runat="server" 
                                    style="font-family: Andalus">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="To">
                            <EditItemTemplate>
                                <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Gridview1_To" runat="server" 
                                    style="font-family: Andalus">
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Gridview1_To" runat="server" 
                                    style="font-family: Andalus">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Upload Picture">
                            <ItemTemplate>
                                <asp:FileUpload ID="FileUpload_Spec" runat="server" Font-Names="Andalus" />
                                <br />
                                <asp:Label ID="Label_File_Name" runat="server" Visible="False"></asp:Label>
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
                </asp:GridView></div>
                
            </div>
        </asp:Panel>
        </div>
        </div>
       
        <br />
                        <div class="panel-info" Width="95%">
        <div class="panel-heading">
            <h3 class="panel-title">Product/Service Quantity Requested By Customer</h3>
            </div>
            <div class="panel-body">    
        <asp:Panel ID="Panel_Prod_Srv_Qnty" runat="server"    style="font-family: Andalus" >
            From:
            <asp:TextBox class="form-control" ID="TextBox_Prod_Qnty_From" runat="server"  
                Width="20%"></asp:TextBox>
            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator_Qnty_From" 
                runat="server" ControlToValidate="TextBox_Prod_Qnty_From" Display="Dynamic" 
                ErrorMessage="*" ForeColor="Red" ValidationGroup="Req_Validate"></asp:RequiredFieldValidator>
            &nbsp;To:&nbsp;<asp:TextBox class="form-control" ID="TextBoxrod_Qnty_To" runat="server"  
                Width="20%"></asp:TextBox>
            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator_Qnty_To" runat="server" 
                ControlToValidate="TextBoxrod_Qnty_To" Display="Dynamic" ErrorMessage="*" 
                ForeColor="Red" ValidationGroup="Req_Validate"></asp:RequiredFieldValidator>
            &nbsp;Unit Of Measurement:
            <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Unit_Of_Msrmnt" runat="server" 
                style="font-family: Andalus">
            </asp:DropDownList>
        </asp:Panel>
        </div>
        </div>
        <br />
                                <div class="panel-info" Width="95%">
        <div class="panel-heading">
            <h3 class="panel-title">Price Range Requested By Customer</h3>
            </div>
            <div class="panel-body">    
        <asp:Panel ID="Panel_Price_Range" runat="server"             style="font-family: Andalus" >
            From:
            <asp:TextBox class="form-control" ID="TextBox_Price_Range_From" runat="server"  
                Width="20%"></asp:TextBox>
            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator_Price_From" 
                runat="server" ControlToValidate="TextBox_Price_Range_From" Display="Dynamic" 
                ErrorMessage="*" ForeColor="Red" ValidationGroup="Req_Validate"></asp:RequiredFieldValidator>
            &nbsp;To:&nbsp;<asp:TextBox class="form-control" ID="TextBox_Price_Range_To" runat="server"  
                Width="20%"></asp:TextBox>
            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator_Price_To" 
                runat="server" ControlToValidate="TextBox_Price_Range_To" Display="Dynamic" 
                ErrorMessage="*" ForeColor="Red" ValidationGroup="Req_Validate"></asp:RequiredFieldValidator>
            &nbsp;&nbsp;</asp:Panel>
            </div>
            </div>
        <br />
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
                                        <div class="panel-info" Width="95%">
        <div class="panel-heading">
            <h3 class="panel-title">Your Quote</h3>
            </div>
            <div class="panel-body">    
        <asp:Panel ID="Panel_Quote" runat="server"  
             style="font-family: Andalus">
            Product Name:
            <asp:TextBox class="form-control" ID="TextBox_Prod_Name" runat="server"  
                Height="28px" Width="50%"></asp:TextBox>
            &nbsp;or select from list:
            <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Prod_List" runat="server" 
                AutoPostBack="True" 
                onselectedindexchanged="DropDownList_Prod_List_SelectedIndexChanged" 
                style="font-family: Andalus">
            </asp:DropDownList>
            <br /><br />Quote Amount:
            <asp:TextBox class="form-control" ID="TextBox_Quote_Amnt" runat="server"  
                Width="20%"></asp:TextBox>
            &nbsp;<br />
            <br />
        </asp:Panel>
        </div>
        </div>
                </ContentTemplate>
        </asp:UpdatePanel>
        <br />

                <div align="center">
                    <asp:Button ID="Button_Add_To_Req" runat="server" class="btn btn-sm btn-success"
                        onclick="Button_Add_To_RFQ_Click" style="font-family: Andalus" Text="Add" 
                        ValidationGroup="Req_Validate" />
                        
                    &nbsp;&nbsp;&nbsp;<br />
                    &nbsp;<asp:Label ID="Label_Status0" runat="server" Text="Label" Visible="False"></asp:Label>
                    <br />
                    <br />
        </div>
        <br />
    </asp:Panel>
    </div>
    </div>
     <asp:Label ID="Label_Selected_List" runat="server" Text="Label" Visible="False"></asp:Label>
    </div>    
    </p>
    </form>
    </body>
</html>
