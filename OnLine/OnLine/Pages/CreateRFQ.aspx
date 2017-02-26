<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateRFQ.aspx.cs" Inherits="OnLine.Pages.CreateRFQ" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link href="../css/bootswatch_flaty.css" rel="stylesheet" type="text/css" />
    <link href="../css/bootstrap-theme.min.css" rel="stylesheet" type="text/css" />
    <link href="../css/Custom.css?version=5" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-2.2.3.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui.min.js" type="text/javascript"></script>
    <script src="../js/bootstrap.min.js" type="text/javascript"></script>
    
    <title></title>
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
            window.opener.document.getElementById('ContentPlaceHolderBody_Button_Rfq_Refresh_Hidden').click();
        }
        </script>
</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
    <div align="center">
    
        <br />
        <br />

            <div class="panel panel-primary" Width="95%">
        <div class="panel-heading">
            <h3 class="style3">Create New RFQ</h3>
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
            <div >
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
                            <asp:Button ID="Buttin_Show_Spec_List1" runat="server" class="btn btn-sm btn-success"
                                onclick="Buttin_Show_Spec_List_Click" style="font-family: Andalus" 
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
                <div align="center"><asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
                    AutoGenerateColumns="False" CellPadding="4" 
                                                                        CssClass="table table-striped table-bordered table-hover tableShadow" 
                    GridLines="None" onpageindexchanging="GridView1_PageIndexChanging" 
                    onrowdatabound="GridView1_RowDataBound" onrowdeleting="GridView1_RowDeleting" 
                    onrowediting="GridView1_RowEditing" onrowupdating="GridView1_RowUpdating" 
                    PageSize="3" Visible="False" 
                        onselectedindexchanged="GridView1_SelectedIndexChanged" 
                        onselectedindexchanging="GridView1_SelectedIndexChanging" Width="90%">                    
                    <Columns>
                        <asp:CommandField SelectText="Add To RFQ" ShowSelectButton="True" />
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
                </asp:GridView></div>
                
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
            From:
            <asp:TextBox class="form-control" ID="TextBox_Prod_Qnty_From" runat="server"  
                Width="20%" MaxLength="14"></asp:TextBox>
            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator_Qnty_From" 
                runat="server" ControlToValidate="TextBox_Prod_Qnty_From" Display="Dynamic" 
                ErrorMessage="Required" ForeColor="Red" ValidationGroup="Rfq_Validate"></asp:RequiredFieldValidator>
            &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator1" 
                runat="server" ControlToValidate="TextBox_Prod_Qnty_From" Display="Dynamic" 
                ErrorMessage="Please enter valid number (maximum 11 digits before decimal and 2 after decimal)" 
                ForeColor="Red" ValidationExpression="^(?:[1-9]\d{0,10})?(?:\.\d{0,2})?$" 
                ValidationGroup="Rfq_Validate"></asp:RegularExpressionValidator>
            To:&nbsp;<asp:TextBox class="form-control" ID="TextBoxrod_Qnty_To" runat="server"  
                Width="20%" MaxLength="14"></asp:TextBox>
            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator_Qnty_To" runat="server" 
                ControlToValidate="TextBox_Prod_Qnty_From" Display="Dynamic" ErrorMessage="Required" 
                ForeColor="Red" ValidationGroup="Rfq_Validate"></asp:RequiredFieldValidator>
            &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator2" 
                runat="server" ControlToValidate="TextBoxrod_Qnty_To" Display="Dynamic" 
                ErrorMessage="Please enter valid number (maximum 11 digits before decimal and 2 after decimal)" 
                ForeColor="Red" ValidationExpression="^(?:[1-9]\d{0,10})?(?:\.\d{0,2})?$" 
                ValidationGroup="Rfq_Validate"></asp:RegularExpressionValidator>
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
            From:
            <asp:TextBox class="form-control" ID="TextBox_Price_Range_From" runat="server"  
                Width="20%" MaxLength="14"></asp:TextBox>
            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator_Price_From" 
                runat="server" ControlToValidate="TextBox_Price_Range_From" Display="Dynamic" 
                ErrorMessage="Required" ForeColor="Red" ValidationGroup="Rfq_Validate"></asp:RequiredFieldValidator>
            &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator3" 
                runat="server" ControlToValidate="TextBox_Price_Range_From" Display="Dynamic" 
                ErrorMessage="Please enter valid number (maximum 11 digits before decimal and 2 after decimal)" 
                ForeColor="Red" ValidationExpression="^(?:[1-9]\d{0,10})?(?:\.\d{0,2})?$" 
                ValidationGroup="Rfq_Validate"></asp:RegularExpressionValidator>
            To:&nbsp;<asp:TextBox class="form-control" ID="TextBox_Price_Range_To" runat="server"  
                Width="20%" MaxLength="14"></asp:TextBox>
            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator_Price_To" 
                runat="server" ControlToValidate="TextBox_Price_Range_To" Display="Dynamic" 
                ErrorMessage="Required" ForeColor="Red" ValidationGroup="Rfq_Validate"></asp:RequiredFieldValidator>
            &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator4" 
                runat="server" ControlToValidate="TextBox_Price_Range_To" Display="Dynamic" 
                ErrorMessage="Please enter valid number (maximum 11 digits before decimal and 2 after decimal)" 
                ForeColor="Red" ValidationExpression="^(?:[1-9]\d{0,10})?(?:\.\d{0,2})?$" 
                ValidationGroup="Rfq_Validate"></asp:RegularExpressionValidator>
            Currency:
            <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Curr" runat="server" 
                 style="font-family: Andalus">
            </asp:DropDownList>
            &nbsp;
        </asp:Panel>
                    </div>
            </div>
        <br />
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                                    <div class="panel-info" Width="95%">
        <div class="panel-heading">
            <h3 class="panel-title">Location</h3>
            </div>
            <div class="panel-body">   
                <asp:Panel ID="Panel_Location" runat="server"
                    style="font-family: Andalus" >
                    Country:
                    <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Country" runat="server" AutoPostBack="True" 
                        style="font-family: Andalus" 
                        onselectedindexchanged="DropDownList_Country_SelectedIndexChanged">
                    </asp:DropDownList>
                    &nbsp;State/Province:
                    <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_State" runat="server" AutoPostBack="True" 
                         style="font-family: Andalus" 
                        onselectedindexchanged="DropDownList_State_SelectedIndexChanged">
                    </asp:DropDownList>
                    &nbsp;City:
                    <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_City" runat="server" AutoPostBack="True" 
                          style="font-family: Andalus" onselectedindexchanged="DropDownList_City_SelectedIndexChanged">                          
                    </asp:DropDownList>
                    &nbsp;Locality:
                    <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Locality" runat="server" 
                        style="font-family: Andalus">
                    </asp:DropDownList>
                    &nbsp;Street Name:
                    <asp:TextBox class="form-control" ID="TextBox_Street_Name" runat="server"  
                        Width="20%"></asp:TextBox>
                </asp:Panel>
                 </div>
                </div>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <br />
          <div class="panel-info" Width="95%">
        <div class="panel-heading">
            <h3 class="panel-title">Docs and Terms &amp; Cond</h3>
            </div>
            <div class="panel-body">   
        <asp:Panel ID="Panel_Price_Range0" runat="server" 
            style="font-family: Andalus" >
            Upload Documnet:
            <asp:FileUpload ID="FileUpload1" runat="server"  
                ViewStateMode="Enabled" />
            <br />
            <br />
            Terms and Conditions:
            <asp:TextBox class="form-control" ID="TextBox_TnC" runat="server" Height="108px" MaxLength="500" 
                TextMode="MultiLine" Width="50%"></asp:TextBox>
                <br /><br />
                        <asp:Label ID="Label6" runat="server" Text="Within Date:"></asp:Label>
        &nbsp;<asp:TextBox class="form-control" ID="TextBox_Within_Date" runat="server"  
            Width="20%"></asp:TextBox>
        <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_Within_Date_CalendarExtender" 
            runat="server" TargetControlID="TextBox_Within_Date" PopupButtonID="ImageButton1"   Format="yyyy-MM-dd">
        </ajaxtoolkit:CalendarExtender>
        &nbsp;<asp:ImageButton ID="ImageButton1" runat="server" Height="23px" 
            ImageUrl="~/Images/Calendar.png" Width="40px" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; RFQ Name:
        <asp:TextBox class="form-control" ID="TextBox_Reqr_Name" runat="server"  
            Width="50%" MaxLength="200"></asp:TextBox>
        &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator_RFQ_Name" 
            runat="server" ControlToValidate="TextBox_Reqr_Name" Display="Dynamic" 
            ErrorMessage="Required" ForeColor="Red" ValidationGroup="Rfq_Validate"></asp:RequiredFieldValidator>
        </asp:Panel>
                         </div>
                </div>
        <br />
        <br />

                <div align="center">
                    <asp:Button ID="Button_Submit_Req" runat="server" class="btn btn-sm btn-success"
                        onclick="Button_Submit_Req_Click" style="font-family: Andalus" Text="Submit!" 
                        ValidationGroup="Rfq_Validate" />
                        
                    &nbsp;&nbsp;<asp:Button ID="Button_Submit_Extra_Prd_Srv" runat="server" class="btn btn-sm btn-success"
                        onclick="Button_Submit_Extra_Prd_Srv_Click" style="font-family: Andalus" 
                        Text="Add More Products/Services To This RFQ!" 
                        ToolTip="You can add multiple product/service details in the same RFQ. Once complete you can submit the final RFQ." 
                        ValidationGroup="Rfq_Validate" />
                    <br />
                    <br />
                    <asp:Button ID="Button_Submit_Next" runat="server" class="btn btn-sm btn-success"
                        onclick="Button_Submit_Req_Next" style="font-family: Andalus" 
                        Text="Submit and Next!" ValidationGroup="Rfq_Validate" />
                    &nbsp;<br /> 
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                    <asp:Label ID="Label_Status" runat="server" Text="Label" Visible="False"></asp:Label>
                     </ContentTemplate>
                </asp:UpdatePanel>                    
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
