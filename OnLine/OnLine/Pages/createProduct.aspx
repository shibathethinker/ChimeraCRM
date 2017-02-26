<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="createProduct.aspx.cs" Inherits="OnLine.Pages.createProduct" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">  
    <title></title>
     <link href="../css/bootswatch_flaty.css" rel="stylesheet" type="text/css" />
    <link href="../css/bootstrap-theme.min.css" rel="stylesheet" type="text/css" />
    <link href="../css/Custom.css?version=5" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-2.2.3.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    
    <style type="text/css">

legend {color:red}
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
        <script language="javascript">
            function RefreshParent() {
                window.opener.document.getElementById('ContentPlaceHolderBody_Button_Prod_Refresh_Hidden').click();
            }
        </script> 
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
    
                <div class="panel panel-primary" Width="95%">
        <div class="panel-heading">
            <h3 class="style3">Create New Product/Service</h3>
            </div>
            <div class="panel-body">   
    <asp:Panel ID="Panel2" runat="server" 
        style="font-family: Andalus">
        <br />
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

            <div class="panel-info" Width="95%">
        <div class="panel-heading">
            <h3 class="panel-title">Product Service Details</h3>
            </div>
            <div class="panel-body">    
        <asp:Panel ID="Panel_Prod_Service_Det" runat="server"  
            style="font-family: Andalus" >
            <div class="style2">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
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
                            <asp:Button ID="Buttin_Show_Spec_List" runat="server" 
                                onclick="Buttin_Show_Spec_List_Click" style="font-family: Andalus" class="btn btn-sm btn-success"
                                Text="Show Features and Specifications!" ValidationGroup="Panel2" />
                        </asp:Panel>
                <br />
                <asp:Label ID="Label_Extra_Spec" runat="server" 
                    Text="Specification Not Listed Here? Then Enter..." Visible="False"></asp:Label>
                &nbsp;<asp:TextBox class="form-control" ID="TextBox_Spec" runat="server"  
                    Width="50%" Visible="False" MaxLength="50" TextMode="MultiLine"></asp:TextBox>
                &nbsp;
                <br />
                <br />
                <asp:Label ID="Label_Extra_Spec_upload" runat="server" 
                    Text="and upload picture if required" Visible="False"></asp:Label>
                &nbsp;<asp:FileUpload ID="FileUpload2" runat="server"  
                    ViewStateMode="Enabled" Visible="False" />
                <br />
                <br />

                    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
                    AutoGenerateColumns="False" CellPadding="4" 
                                                                        CssClass="table table-striped table-bordered table-hover tableShadow" 
                    GridLines="None" onpageindexchanging="GridView1_PageIndexChanging" 
                    PageSize="3" Visible="False" 
                        onselectedindexchanged="GridView1_SelectedIndexChanged" 
                        onrowdatabound="GridView1_RowDataBound" Width="90%">                    
                        <Columns>
                            <asp:CommandField SelectText="Add To Product" ShowSelectButton="True" />
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
                                <ItemTemplate>
                                    <asp:DropDownList class="form-control feature-select-dropdown" ID="DropDownList_Gridview1_From" runat="server" 
                                        style="font-family: Andalus">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="To">
                                <ItemTemplate>
                                    <asp:DropDownList class="form-control feature-select-dropdown" ID="DropDownList_Gridview1_To" runat="server" 
                                        style="font-family: Andalus">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Upload Picture">
                                <ItemTemplate>
                                    <asp:FileUpload ID="FileUpload_Spec" runat="server" Font-Names="Andalus" />
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
        </asp:Panel>
        </div>
        </div>
       
        <br />
                    <div class="panel-info" Width="95%">
        <div class="panel-heading">
            <h3 class="panel-title">Product/Service Quantity</h3>
            </div>
            <div class="panel-body">    
        <asp:Panel ID="Panel_Prod_Srv_Qnty" runat="server" 
            style="font-family: Andalus" >
            Current Stock:
            <asp:TextBox class="form-control" ID="TextBox_Stock" runat="server"  
                Width="20%" MaxLength="14"></asp:TextBox>
            &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator5" 
                runat="server" ControlToValidate="TextBox_Stock" Display="Dynamic" 
                ErrorMessage="Please enter valid number (maximum 11 digits before decimal and 2 after decimal)" 
                ForeColor="Red" ValidationExpression="^(?:[1-9]\d{0,10})?(?:\.\d{0,2})?$" 
                ValidationGroup="createProd"></asp:RegularExpressionValidator>
            Unit Of Measurement:
            <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Unit_Of_Msrmnt" runat="server" 
                style="font-family: Andalus">
            </asp:DropDownList>
        </asp:Panel>
        </div>
        </div>
        <br />
                            <div class="panel-info" Width="95%">
        <div class="panel-heading">
            <h3 class="panel-title">Price Range</h3>
            </div>
            <div class="panel-body">    
        <asp:Panel ID="Panel_Price_Range" runat="server"
            style="font-family: Andalus" >
            Sourcing Price:
            <asp:TextBox class="form-control" ID="TextBox_Src_Price" runat="server"  
                Width="20%" MaxLength="14"></asp:TextBox>
            &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator3" 
                runat="server" ControlToValidate="TextBox_Src_Price" Display="Dynamic" 
                ErrorMessage="Please enter valid number (maximum 11 digits before decimal and 2 after decimal)" 
                ForeColor="Red" ValidationExpression="^(?:[1-9]\d{0,10})?(?:\.\d{0,2})?$" 
                ValidationGroup="createProd"></asp:RegularExpressionValidator>
            Listing Price:&nbsp;<asp:TextBox class="form-control" ID="TextBox_List_Price" runat="server"  
                Width="20%" MaxLength="14"></asp:TextBox>
            &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator4" 
                runat="server" ControlToValidate="TextBox_List_Price" Display="Dynamic" 
                ErrorMessage="Please enter valid number (maximum 11 digits before decimal and 2 after decimal)" 
                ForeColor="Red" ValidationExpression="^(?:[1-9]\d{0,10})?(?:\.\d{0,2})?$" 
                ValidationGroup="createProd"></asp:RegularExpressionValidator>
            Currency:
            <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Curr" runat="server" 
                 style="font-family: Andalus">
            </asp:DropDownList>
            &nbsp;</asp:Panel>
            </div>
            </div>
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Product/Service Name:
        <asp:TextBox class="form-control" ID="TextBox_Prod_Name" runat="server"  
            Width="50%" ValidationGroup="createProd" MaxLength="200"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
            ControlToValidate="TextBox_Prod_Name" Display="Dynamic" ErrorMessage="*" 
            ForeColor="#FF3300" ValidationGroup="createProd"></asp:RequiredFieldValidator>
        <br />
        <br />
        <br />

                <div align="center">
                    <asp:Button ID="Button_Submit_Prod" runat="server" class="btn btn-sm btn-success"
                        onclick="Button_Submit_Prod_Click" style="font-family: Andalus" Text="Submit!" 
                        ValidationGroup="createProd" />
                        
                    &nbsp;&nbsp;&nbsp;<br />
                    <br />
                    <asp:Button ID="Button_Submit_Next" runat="server" class="btn btn-sm btn-success"
                        onclick="Button_Submit_Next_Click" style="font-family: Andalus" 
                        Text="Submit and Next!" ValidationGroup="createProd" />
                    &nbsp;<br /> <asp:Label ID="Label_Status" runat="server" Text="Label" Visible="False"></asp:Label>
                    <br />
                    <br />
        </div>
        <br />
    </asp:Panel>
    </div>
    </div>
    
    </div>
    <p>
       <asp:Label ID="Label_Selected_List" runat="server" Text="Label" Visible="False"></asp:Label>
       </p>
    </form>
</body>
</html>
