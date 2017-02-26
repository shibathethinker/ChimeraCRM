<%@ Page Title="" Language="C#" Theme="ThemeBlue" MasterPageFile="~/Pages/Site1.Master" AutoEventWireup="true" CodeBehind="Sales.aspx.cs" Inherits="OnLine.Pages.Sales" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style6
        {
            width: 90%;        
            font-size="small";    
           /* border: 1px solid #c0c0c0;*/
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

    function currentDateTimeVale()
    {
        var currentTime = new Date();
        var month = currentTime.getMonth() + 1;
        var day = currentTime.getDate();
        var year = currentTime.getFullYear();
        var finalVal=month + "/" + day + "/" + year;
        return finalVal;
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

    window.onload = function () {
        var table, tbody, i, rowLen, row, j, colLen, cell;

        table = document.getElementById("GridView_Lead");
        tbody = table.tBodies[0];

        for (i = 0, rowLen = tbody.rows.length; i < rowLen; i++) {
            row = tbody.rows[i];
            for (j = 0, colLen = row.cells.length; j < colLen; j++) {
                cell = row.cells[j]; 

                if (j == 8) {
                    // Followup date
                    console.log("--Found gender: " + cell.innerHTML);
                    var dval = new Date(Date.parse(cell.innerHTML));

                    if (Date.parse(dval) > currentDateTimeVale)
                        cell.bgColor = "yellow"

                } else if (j == 12) {
                    // Active status
                    /*console.log("--Found age: " + cell.innerHTML);*/
                }
            }
        }
    };
</script>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
                        <asp:Label ID="Label_Sales_Screen_Access" runat="server" ForeColor="Red" 
                            style="font-size: small; font-family: Andalus;" 
        Visible="False"></asp:Label>
    
    <asp:UpdatePanel ID="UpdatePanel_Lead_Collp" runat="server">
    <ContentTemplate>

                                                                    <table width="100%">
                                                                  <tr>
                                                                <td background="../Images/menu_bg.gif">
                                                                <asp:LinkButton ID="LinkButton_Lead_Collp" runat="server" ForeColor="White" 
                                                                        ToolTip="Click to expand or collapse">All Leads</asp:LinkButton>
                                                                        <div style="float: right; vertical-align: middle;">
                                                                    <asp:Image ID="Lead_Collapse_Img" runat="server"/>
                                                                    </div>
                                                                    </td>
                                                            </tr>
                                                            </table> 
    <asp:Panel  ID="Panel_Lead_Collp" runat="server">
    <br />
                <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title">All Leads</h3>
            </div>
            <div class="panel-body">  
        <asp:Panel ID="Panel_All_Leads" runat="server" 
            style="font-family: Andalus">
            <div align="center">
                                  <div class="panel panel-success">
            <div class="panel-heading">
              <h3 class="panel-title">Filter By</h3>
            </div>
            <div class="panel-body">
                        <table class="style6">
                            <tr>
                                <td align="center">
                                    Lead#:
                                    <asp:TextBox class="form-control" ID="TextBox_RFQ_No_Lead" runat="server" 
                                        style="font-family: Andalus" Width="20%"></asp:TextBox>
                                    &nbsp;Category:
                                    <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Category_Lead" runat="server" 
                                        style="font-family: Andalus">
                                    </asp:DropDownList>
                                    &nbsp; Active Status:
                                    <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Lead_Active_Stat" runat="server" 
                                        style="font-family: Andalus">
                                    </asp:DropDownList>
                                    &nbsp;Customer:
                                    <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Contact_Lead" runat="server" 
                                        style="font-family: Andalus">
                                    </asp:DropDownList>
                                    <br /><br />From Date (Due) :
                                    <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_From_Date_Lead" runat="server" 
                                        ValidationGroup="LeadDueDate" Width="20%"></asp:TextBox>
                                    <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_From_Date_Lead_CalendarExtender" 
                                        runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton3" 
                                        TargetControlID="TextBox_From_Date_Lead">
                                    </ajaxtoolkit:CalendarExtender>
                                    &nbsp;&nbsp; To Date (Due) :&nbsp;
                                    <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_To_Date_Lead" runat="server" 
                                        ValidationGroup="LeadDueDate" Width="20%"></asp:TextBox>
                                    <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_To_Date_Lead_CalendarExtender" 
                                        runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton4" 
                                        TargetControlID="TextBox_To_Date_Lead">
                                    </ajaxtoolkit:CalendarExtender>
                                    &nbsp;Assigned To:&nbsp;<asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Assigned_To_Lead" 
                                        runat="server" style="font-family: Andalus">
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;<asp:Button ID="Button_Filter_All_RFQ" runat="server" class="btn btn-sm btn-success"
                                        onclick="Button_Filter_All_Lead_Click" style="font-family: Andalus" 
                                        Text="Filter" ValidationGroup="RFQDueDate" />
                                    &nbsp;<asp:CompareValidator ID="CompareValidator2" runat="server" 
                                        ControlToCompare="TextBox_From_Date_Lead" 
                                        ControlToValidate="TextBox_To_Date_Lead" Display="Dynamic" 
                                        ErrorMessage="to date can not be earlier than from date" ForeColor="Red" 
                                        Operator="GreaterThan" Type="Date" ValidationGroup="LeadDueDate"></asp:CompareValidator>
                                </td>
                            </tr>
                        </table>
                                    </div>
                                    </div>      
                        <br />
                <div align="center">
                
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:Image ID="Image_Unanswered_Lead" runat="server" Height="16px" 
                            ImageUrl="~/Images/attention.jpg" Visible="False" />
                        <asp:Label ID="Label_Unanswered_Leads" runat="server" Text="Unanswered Leads" 
                            Visible="False"></asp:Label>
                    <asp:GridView ID="GridView_Lead" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CellPadding="4" CssClass="table table-striped table-bordered table-hover tableShadow"  GridLines="None"
                        OnPageIndexChanging="GridView_Lead_PageIndexChanging" PageSize="5" Visible="False"
                        OnSelectedIndexChanged="GridView_Lead_SelectedIndexChanged" Style="font-size: small"
                        Height="30%" Width="95%" OnRowCancelingEdit="GridView_Lead_RowCancelingEdit"
                        OnRowDataBound="GridView_Lead_RowDataBound" OnRowEditing="GridView_Lead_RowEditing"
                        OnRowUpdating="GridView_Lead_RowUpdating" OnSorting="GridView_Lead_Sorting" AllowSorting="True">                 
                      
                        <Columns>
                          <%--   <asp:CommandField ShowSelectButton="True" /> --%>
                            
                            <asp:TemplateField HeaderText=""   >
                                <ItemTemplate    >
                                              <asp:RadioButton GroupName= "rain" runat="server" AutoPostBack="true" OnClick="javascript:Selrdbtn(this.id)"  
                                                  OnCheckedChanged="GridView_Lead_RadioSelect" ID="lead_radio"></asp:RadioButton>                                         
                                              
                                </ItemTemplate>
                                </asp:TemplateField>
                            <asp:CommandField ShowEditButton="True" />
                            <asp:TemplateField HeaderText="Lead#">
                                <ItemTemplate>
                                    <asp:Label ID="Label_RFQId" runat="server" Text='<%# Eval("RFQNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Lead Name">
                                <EditItemTemplate>
                                    <asp:TextBox class="form-control" ID="TextBox_RFQName" runat="server" style="font-family: Andalus" 
                                        Text='<%# Eval("RFQName") %>'></asp:TextBox>
                                    <asp:Label ID="Label_RFQName_Edit" runat="server" Text='<%# Eval("RFQName") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label_RFQName" runat="server" Text='<%# Eval("RFQName") %>'></asp:Label>
                                    <asp:Image ID="Image1" runat="server" Height="16px" 
                                        ImageUrl="~/Images/attention.jpg" Visible='<%# Eval("Lead_Alert_Required").ToString().Equals("true") %>'
                                        Width="18px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Customer Details">
                                <EditItemTemplate>
                                    <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Contact" runat="server" Visible="False">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label_Contact" runat="server" Text='<%# Eval("CustId") %>' 
                                        Visible="False"></asp:Label>
                                    <asp:LinkButton ID="LinkButton_Customer_Edit" runat="server" 
                                        onclientclick="window.open('Popups/Sale/AllLead_Customer.aspx','DispCust','status=1,location=yes,width=1000,height=400,left=100,right=500',true);" 
                                        Text='<%# Eval("CustName") %>' Visible="False"></asp:LinkButton>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton_Customer" runat="server" 
                                        Text='<%# Eval("CustName") %>' 
                                        oncommand="LinkButton_Customer_Gridview_Lead_Command" OnClientClick="javascript:unSelrdbtn()"></asp:LinkButton>
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
                            <asp:TemplateField HeaderText="Specifications &amp; Reply">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" 
                                        oncommand="GridView_Lead_Spec_Command" 
                                        OnClientClick="javascript:unSelrdbtn()">Show!</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Submit Date" SortExpression="Submit Date Ticks">
                                <ItemTemplate>
                                    <asp:Label ID="Label_Submit_Date" runat="server" 
                                        Text='<%# Eval("Submit Date") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle ForeColor="black" Font-Underline="True" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Due Date" SortExpression="Due Date Ticks">
                                <EditItemTemplate>
                                    <asp:TextBox class="form-control" ID="TextBox_DueDate" runat="server"  
                                        style="font-family: Andalus" Text='<%# Eval("Due Date") %>' Width="169px"></asp:TextBox>
                                    <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_DueDate_CalendarExtender" 
                                        runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton_DueDate_Edit" 
                                        TargetControlID="TextBox_DueDate">
                                    </ajaxtoolkit:CalendarExtender>
                                    <asp:Label ID="Label_Due_Date_Edit" runat="server" 
                                        Text='<%# Eval("Due Date") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label_Due_Date" runat="server" Text='<%# Eval("Due Date") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle ForeColor="black" Font-Underline="True" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Followup Date" SortExpression="Next Date Ticks">
                                <ItemTemplate>
                                    <asp:Label ID="Label_Fwp_Date" runat="server" Text='<%# Eval("Next Date") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox class="form-control" ID="TextBox_FwpDate" runat="server"  
                                        style="font-family: Andalus" Text='<%# Eval("Next Date") %>' Width="169px"></asp:TextBox>
                                    <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_FwpDate_CalendarExtender" 
                                        runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton_FwpDate_Edit" 
                                        TargetControlID="TextBox_FwpDate">
                                    </ajaxtoolkit:CalendarExtender>
                                    <asp:Label ID="Label_Fwp_Date_Edit" runat="server" 
                                        Text='<%# Eval("Next Date") %>' Visible="False"></asp:Label>
                                </EditItemTemplate>
                                <HeaderStyle ForeColor="black" Font-Underline="True" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Assigned To" >
                                <EditItemTemplate>
                                    <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Lead_Assgn_To_Edit" runat="server" 
                                        style="font-family: Andalus">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label_Assgn_To_Edit" runat="server" 
                                        Text='<%# Eval("Assgn To") %>' Visible="False"></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate  >
                                    <asp:LinkButton ID="LinkButton_Assgn_To" runat="server" 
                                        Text='<%# Eval("Assgn To") %>' oncommand="LinkButton_Assgn_To_Command" 
                                        OnClientClick="javascript:unSelrdbtn()" ></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Location">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton_Location" runat="server" 
                                        oncommand="GridView_Lead_LinkButton_Location_Command" 
                                        OnClientClick="javascript:unSelrdbtn()">Show!</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="T&amp;C">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton_TnC" runat="server" 
                                        oncommand="GridView_Lead_TC_Command" 
                                        OnClientClick="javascript:unSelrdbtn()">Show!</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Active Status">
                                <ItemTemplate>
                                    <asp:Label ID="Label_Stat" runat="server" Text='<%# Eval("ActiveStat") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Lead_Active" runat="server" 
                                        style="font-family: Andalus">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label_Act_Stat_Edit" runat="server" 
                                        Text='<%# Eval("ActiveStat") %>' Visible="False"></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Creation Mode">
                                <ItemTemplate>
                                    <asp:Label ID="Label_Mode" runat="server" Text='<%# Eval("Mode") %>'></asp:Label>
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
                        <asp:Label ID="Label_Lead_Grid_Access" runat="server" ForeColor="Red" 
                            style="font-size: small" Visible="False"></asp:Label>
                        <br />
                                       <asp:Button ID="Button_Lead_Refresh_Hidden" runat="server" style="display:none" 
                            ForeColor="#336600" onclick="Button_Lead_Refresh_Hidden_Click" 
                            Text="Hidden" />
                        <asp:Button ID="Button_Lead_Refresh" runat="server" ForeColor="#336600" 
                            onclick="Button_Lead_Refresh_Click" 
                            style="font-family: Andalus; font-size: x-small" Text="Refresh Data!" />
                        &nbsp;<asp:Button ID="Button_Create_Req0" runat="server" 
                            onclientclick="window.open('createLead.aspx','CreateLead','width=1000,height=1000,left=100,right=500,scrollbars=1',true);" class="btn btn-sm btn-success"
                            style="font-family: Andalus" Text="Create New Lead!" />
                        &nbsp;<asp:Button ID="Button_Create_Clone_Lead" runat="server" Enabled="False" 
                            onclick="Button_Create_Clone_Lead_Click" style="font-family: Andalus" class="btn btn-sm btn-success"
                            Text="Clone This!" />
                        &nbsp;<asp:Button ID="Button_Convert_Lead" runat="server" Enabled="False" 
                            oncommand="Button_Convert_Lead_Command" style="font-family: Andalus" class="btn btn-sm btn-success"
                            Text="Convert to Potential!" 
                            ToolTip="Only Leads created by your organization can be converted by you!" />
                        &nbsp;<asp:Button ID="Button_Lead_Doc" runat="server" Enabled="False" 
                            style="font-family: Andalus" Text="Documents" class="btn btn-sm btn-success"
                            onclick="Button_Lead_Doc_Click" />
                        &nbsp;<asp:Button ID="Button_Notes_Lead" runat="server" Enabled="False" 
                            onclick="Button_Notes_Lead_Click" style="font-family: Andalus" Text="Notes!" class="btn btn-sm btn-success" />
                        &nbsp;<asp:Button ID="Button_Audit_Lead" runat="server" Enabled="False" 
                            onclick="Button_Audit_Lead_Click" style="font-family: Andalus" Text="Logs!" class="btn btn-sm btn-success" />
                        &nbsp;<asp:Label ID="Label_Lead_Conv_Stat" runat="server" style="font-size: small" 
                            Visible="False"></asp:Label>
                    </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                    &nbsp;&nbsp;</div>
                
            </div>
        </asp:Panel>   
                </div>
        </div> 
        </asp:Panel>
                            <ajaxtoolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender_Lead" CollapsedText="Show Details..." ExpandedText="Hide Details" runat="server" Collapsed="true" AutoCollapse="false" TargetControlID="Panel_Lead_Collp"
            ImageControlId="Lead_Collapse_Img"  CollapsedImage="~/Images/ArrowTip_Down.png" ExpandedImage="~/Images/ArrowTip_Parallel.png"   ExpandControlID="LinkButton_Lead_Collp"
             CollapseControlID="LinkButton_Lead_Collp"/>     
    </ContentTemplate>
    </asp:UpdatePanel>
        <br />
    <asp:UpdatePanel ID="UpdatePanel_Potn_Collp" runat="server">
    <ContentTemplate>

                                                                            <table width="100%">
                                                                  <tr>
                                                                <td background="../Images/menu_bg.gif">
                                                                <asp:LinkButton ID="LinkButton_Potn_Collp" runat="server" ForeColor="White" 
                                                                        ToolTip="Click to expand or collapse">All Potentials</asp:LinkButton>
                                                                        <div style="float: right; vertical-align: middle;">
                                                                    <asp:Image ID="Image_Potn_Collp" runat="server"/>
                                                                    </div>
                                                                    </td>
                                                            </tr>
                                                            </table> 
                <asp:Panel  ID="Panel_Potn_Collp" runat="server">
                <br />
                <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title">All Potentials</h3>
            </div>
            <div class="panel-body"> 
        <asp:Panel ID="Panel_All_Potential" runat="server" 
            style="font-family: Andalus" >    
            <div align="center">
                         <div class="panel panel-success">
            <div class="panel-heading">
              <h3 class="panel-title">Filter By</h3>
            </div>
            <div class="panel-body">
                        <table class="style6">
                            <tr>
                                <td align="center">
                                    Pot#:
                                    <asp:TextBox class="form-control" ID="TextBox_RFQ_No_Potn" runat="server" 
                                        style="font-family: Andalus" Width="20%"></asp:TextBox>
                                    &nbsp;Category:
                                    <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Category_Pot" runat="server" 
                                        style="font-family: Andalus">
                                    </asp:DropDownList>
                                    &nbsp; Potential Stage:
                                    <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Pot_Stage_Stat" runat="server" 
                                        style="font-family: Andalus">
                                    </asp:DropDownList>
                                    &nbsp; Actvie Status:
                                    <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Pot_Active_Stat" runat="server" 
                                        style="font-family: Andalus">
                                    </asp:DropDownList>
                                    &nbsp;Customer:
                                    <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Contact_Potn" runat="server" 
                                        style="font-family: Andalus">
                                    </asp:DropDownList>
                                    &nbsp;<br />
                                    <br />
                                    From Date (Due) :
                                    <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_From_Date_Due" runat="server" 
                                        ValidationGroup="PotDate" Width="20%"></asp:TextBox>
                                    <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_From_Date_Lead_CalendarExtender1" 
                                        runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton5" 
                                        TargetControlID="TextBox_From_Date_Due">
                                    </ajaxtoolkit:CalendarExtender>
                                    &nbsp;To Date (Due) :<asp:TextBox class="form-control datepicker-textbox" ID="TextBox_To_Date_Due" 
                                        runat="server"  ValidationGroup="PotDate" Width="20%"></asp:TextBox>
                                    <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_To_Date_Lead_CalendarExtender2" 
                                        runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton6" 
                                        TargetControlID="TextBox_To_Date_Due">
                                    </ajaxtoolkit:CalendarExtender>
                                    <br /><br />
                                    From Date (Create) :
                                    <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_From_Date_Create" runat="server" 
                                        ValidationGroup="PotDate" Width="20%"></asp:TextBox>
                                    <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_From_Date_Lead_CalendarExtender3" 
                                        runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton7" 
                                        TargetControlID="TextBox_From_Date_Create">
                                    </ajaxtoolkit:CalendarExtender>
                                    &nbsp;To Date (Create) :
                                    <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_To_Date_Create" runat="server"  
                                        ValidationGroup="PotDate" Width="20%"></asp:TextBox>
                                    <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_To_Date_Lead_CalendarExtender4" 
                                        runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton8" 
                                        TargetControlID="TextBox_To_Date_Create">
                                    </ajaxtoolkit:CalendarExtender>
                                    &nbsp;Assigned To:&nbsp;<asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Assigned_To_Potn" 
                                        runat="server" style="font-family: Andalus">
                                    </asp:DropDownList>
                                    &nbsp;<asp:Button ID="Button_Filter_All_Pot" runat="server" 
                                        onclick="Button_Filter_All_Pot_Click" style="font-family: Andalus" class="btn btn-sm btn-success"
                                        Text="Filter" ValidationGroup="RFQDueDate" />
                                    &nbsp;<asp:CompareValidator ID="CompareValidator3" runat="server" 
                                        ControlToCompare="TextBox_From_Date_Due" 
                                        ControlToValidate="TextBox_To_Date_Due" Display="Dynamic" 
                                        ErrorMessage="to date (due) can not be earlier than from date (due)" 
                                        ForeColor="Red" Operator="GreaterThan" Type="Date" ValidationGroup="PotDate"></asp:CompareValidator>
                                    &nbsp;
                                    <asp:CompareValidator ID="CompareValidator4" runat="server" 
                                        ControlToCompare="TextBox_From_Date_Create" 
                                        ControlToValidate="TextBox_To_Date_Create" Display="Dynamic" 
                                        ErrorMessage="to date (create) can not be earlier than from date (create)" 
                                        ForeColor="Red" Operator="GreaterThan" Type="Date" ValidationGroup="PotDate"></asp:CompareValidator>
                                </td>
                            </tr>
                        </table>
                                          </div>
                                </div> 
                        </div>              
                <br />
                <div align="center">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                    <asp:Label ID="Label_Pot_Edit_Tooltip" runat="server" ForeColor="Red" 
                            style="font-size: small" Visible="False"></asp:Label>
                        <br />
                        <asp:Image ID="Image_Unanswered_Potn" runat="server" Height="16px" 
                            ImageUrl="~/Images/attention.jpg" Visible="False" />
                        <asp:Label ID="Label_Unanswered_Potn" runat="server" Visible="False"></asp:Label>
                    <asp:GridView ID="GridView_Potential" 
                        runat="server" 
                        AllowPaging="True" 
                    AutoGenerateColumns="False" 
                    CellPadding="4"                     
                    GridLines="None" 
                                        CssClass="table table-striped table-bordered table-hover tableShadow" 
                    onpageindexchanging="GridView_Potential_PageIndexChanging" 
                    PageSize="5" Visible="False" 
                        onselectedindexchanged="GridView_Potential_SelectedIndexChanged" 
                        style="font-size: small" Height="30%" Width="95%" 
                        Font-Size="Medium" onrowcancelingedit="GridView_Potential_RowCancelingEdit" 
                            onrowdatabound="GridView_Potential_RowDataBound" 
                            onrowediting="GridView_Potential_RowEditing" 
                            onrowupdating="GridView_Potential_RowUpdating" AllowSorting="True" 
                            onsorting="GridView_Potential_Sorting">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:RadioButton ID="potn_radio" runat="server" AutoPostBack="true" 
                                    GroupName="rain" OnCheckedChanged="GridView_Potn_RadioSelect" 
                                    OnClick="javascript:Selrdbtn(this.id)" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowEditButton="True" />
                        <asp:TemplateField HeaderText="Hidden_Pot_Id">
                            <ItemTemplate>
                                <asp:Label ID="Label_Hidden_Pot_Id" runat="server" Text='<%# Eval("PotId") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Potential#">
                            <ItemTemplate>
                                <asp:Label ID="Label_RFQId" runat="server" 
                                    Text='<%# Eval("RFQNo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Potential Name">
                            <EditItemTemplate>
                                <asp:TextBox class="form-control" ID="TextBox_RFQName_Edit" runat="server" Enabled="False" 
                                    Text='<%# Eval("RFQName") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label_RFQName" runat="server" 
                                    Text='<%# Eval("RFQName") %>'></asp:Label>
                                <asp:Image ID="Image1" runat="server" Height="16px" 
                                    ImageUrl="~/Images/attention.jpg" 
                                    Visible='<%# Eval("Potn_Alert_Required").ToString().Equals("true") %>' 
                                    Width="18px" />
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
                        <asp:TemplateField HeaderText="Specifications- Quotes">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton_Spec" runat="server" 
                                    
                                    onclientclick="javascript:unSelrdbtn()" 
                                    oncommand="LinkButton_Spec_Command">Show!</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date Created" SortExpression="DateCreatedTicks">
                            <ItemTemplate>
                                <asp:Label ID="Label_Date_Created" runat="server" 
                                    Text='<%# Eval("DateCreated") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle ForeColor="black" Font-Underline="True" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Due Date" SortExpression="Due Date Ticks">
                            <EditItemTemplate>
                                <asp:TextBox class="form-control" ID="TextBox_DueDate" runat="server"  
                                    style="font-family: Andalus" Text='<%# Eval("Due Date") %>' Width="169px"></asp:TextBox>
                                <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_DueDate_CalendarExtender" 
                                    runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton_DueDate_Edit" 
                                    TargetControlID="TextBox_DueDate">
                                </ajaxtoolkit:CalendarExtender>
                                <asp:Label ID="Label_Due_Date_Edit" runat="server" 
                                    Text='<%# Eval("Due Date") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label_Due_Date" runat="server" Text='<%# Eval("Due Date") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle ForeColor="black" Font-Underline="True" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Followup Date" SortExpression="Next Date Ticks">
                            <EditItemTemplate>
                                <asp:TextBox class="form-control" ID="TextBox_FwpDate" runat="server"  
                                    style="font-family: Andalus" Text='<%# Eval("Next Date") %>' Width="169px"></asp:TextBox>
                                <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_FwpDate_CalendarExtender" 
                                    runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton_FwpDate_Edit" 
                                    TargetControlID="TextBox_FwpDate">
                                </ajaxtoolkit:CalendarExtender>
                                <asp:Label ID="Label_Fwp_Date_Edit" runat="server" 
                                    Text='<%# Eval("Next Date") %>' Visible="False"></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label_Fwp_Date" runat="server" Text='<%# Eval("Next Date") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle ForeColor="black" Font-Underline="True" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Assigned To">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton_Assgn_To" runat="server" 
                                    Text='<%# Eval("Assgn To") %>' oncommand="LinkButton_Assgn_To_Command1" 
                                    onclientclick="javascript:unSelrdbtn()"></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Potn_Assgn_To_Edit" runat="server" 
                                    style="font-family: Andalus">
                                </asp:DropDownList>
                                <asp:Label ID="Label_Assgn_To_Edit" runat="server" 
                                    Text='<%# Eval("Assgn To") %>' Visible="False"></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Customer Details">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton_Cust_Pot" runat="server" 
                                    onclientclick="javascript:unSelrdbtn()" 
                                    Text='<%# Eval("CustName") %>' oncommand="LinkButton_Cust_Pot_Command"></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Contact" runat="server" Visible="False">
                                </asp:DropDownList>
                                <asp:Label ID="Label_Contact" runat="server" Text='<%# Eval("CustId") %>' 
                                    Visible="False"></asp:Label>
                                <asp:LinkButton ID="LinkButton_Customer_Edit" runat="server" 
                                    onclientclick="window.open('Popups/Sale/AllLead_Customer.aspx','DispCust','status=1,location=yes,width=1000,height=400,left=100,right=500',true);" 
                                    Text='<%# Eval("CustName") %>' Visible="False"></asp:LinkButton>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Location">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton_Location_Pot" runat="server" 
                                    
                                    
                                    
                                    
                                    onclientclick="javascript:unSelrdbtn()" 
                                    oncommand="LinkButton_Location_Pot_Command">Show!</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Potential Amount">
                            <ItemTemplate>
                                <asp:Label ID="Label_Pot_Amnt" runat="server" Text='<%# Eval("PotAmnt") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Request For Deal Sent?">
                            <ItemTemplate>
                                <asp:Label ID="Label_Deal_Request" runat="server" 
                                    Text='<%# Eval("DealRequest") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Potential Stage">
                            <EditItemTemplate>
                                <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Pot_Stage" runat="server" 
                                    style="font-family: Andalus">
                                </asp:DropDownList>
                                &nbsp;
                                <asp:Label ID="Label_Pot_Stage_Edit" runat="server" 
                                    Text='<%# Eval("PotStage") %>' Visible="False"></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label_Pot_Stage" runat="server" Text='<%# Eval("PotStage") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Active Status">
                            <EditItemTemplate>
                                <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Pot_Active" runat="server" 
                                    style="font-family: Andalus">
                                </asp:DropDownList>
                                &nbsp;<asp:Label ID="Label_Act_Stat_Edit" runat="server" 
                                    Text='<%# Eval("ActiveStat") %>' Visible="False"></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label_Act_Stat" runat="server" Text='<%# Eval("ActiveStat") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Sales Order">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton_COM" runat="server" 
                                    oncommand="LinkButton_COM_Command" 
                                    CommandArgument ="<%#((GridViewRow)Container).RowIndex%>" 
                                    Text='<%# Eval("PO_Id") %>' onclientclick="javascript:unSelrdbtn()"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Creation Mode">
                            <ItemTemplate>
                                <asp:Label ID="Label_Mode" runat="server" Text='<%# Eval("Mode") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="T&amp;C">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton_TnC" runat="server" 
                                    oncommand="LinkButton_TnC_Command" onclientclick="javascript:unSelrdbtn()">Show!</asp:LinkButton>
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

                        <asp:Label ID="Label_Potential_Grid_Access" runat="server" ForeColor="Red" 
                            style="font-size: small" Visible="False"></asp:Label>

                        &nbsp;<asp:Label ID="Label_Finalize_Stat" runat="server" Visible="False"></asp:Label>
                        <br />
                        <asp:Button ID="Button_Potn_Refresh_Hidden" runat="server" ForeColor="#336600" style="display:none" 
                            onclick="Button_Potn_Refresh_Hidden_Click" Text="Hidden" />
                                                    <asp:Button ID="Button_Potn_Refresh_Hidden_Index_Unchanged" runat="server" ForeColor="#336600" style="display:none" 
                            onclick="Button_Potn_Refresh_Hidden_Index_Unchanged_Click" Text="Hidden" />
                        <asp:Button ID="Button_Pot_Refresh" runat="server" ForeColor="#336600" 
                            onclick="Button_Pot_Refresh_Click" 
                            style="font-family: Andalus; font-size: x-small" Text="Refresh Data!" />
                        &nbsp;<asp:Button ID="Button_Create_Pot" runat="server" class="btn btn-sm btn-success"
                            onclientclick="window.open('createPotential.aspx','CreatePotential','width=1000,height=1000,left=100,right=500,scrollbars=1',true);" 
                            style="font-family: Andalus" Text="Create New Potential!" />
                        &nbsp;<asp:Button ID="Button_Sales_Order" runat="server" Enabled="False" class="btn btn-sm btn-success"
                            onclick="Button_Sales_Order_Click" style="font-family: Andalus" 
                            Text="Sales Order" />
                        &nbsp;<asp:Button ID="Button_Create_Clone_Potn" runat="server" Enabled="False" class="btn btn-sm btn-success"
                            onclick="Button_Create_Clone_Potn_Click" style="font-family: Andalus" 
                            Text="Clone This!" />
                        &nbsp;<asp:Button ID="Button_Finalz_Deal" runat="server" Enabled="False" class="btn btn-sm btn-success"
                            onclick="Button_Finalz_Deal_Click" style="font-family: Andalus" 
                            Text="Finalize Deal!" 
                            ToolTip="Send finalization request to the customer for this deal! Only once the customer accepts this request he can send the PO.This button is enabled only if this deal is not yet closed and the potential entry is not manually created by your organization!" />
                        &nbsp;<asp:Button ID="Button_Inv_Pmnt" runat="server" Enabled="False" class="btn btn-sm btn-success"
                            onclick="Button_Inv_Pmnt_Click" style="font-family: Andalus" 
                            Text="Invoice and Payments!" 
                            ToolTip="Click this button to view/update already created invoice for this Potential! If no invoice is created already create new. Invoice can also be created from the Invoice section below! For 'Auto' created potential this is enabled for you only if you have WON the contract." />
                        &nbsp;<asp:Button ID="Button_Potn_Doc" runat="server" Enabled="False" class="btn btn-sm btn-success"
                            style="font-family: Andalus" Text="Documents" 
                            onclick="Button_Potn_Doc_Click" />
                        &nbsp;<asp:Button ID="Button_Notes_Potn" runat="server" Enabled="False" class="btn btn-sm btn-success"
                            onclick="Button_Notes_Potn_Click" style="font-family: Andalus" Text="Notes!" />
                        &nbsp;<asp:Button ID="Button_Audit_Potn" runat="server" Enabled="False" class="btn btn-sm btn-success"
                            onclick="Button_Audit_Potn_Click" style="font-family: Andalus" Text="Logs!" />
                        <br />
                        <br />
                                    </ContentTemplate>
                    </asp:UpdatePanel>
            </div>
        </asp:Panel>    
                       </div>
        </div>
                </asp:Panel>
                                            <ajaxtoolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender_Potn" CollapsedText="Show Details..." ExpandedText="Hide Details" runat="server" Collapsed="true" AutoCollapse="false" TargetControlID="Panel_Potn_Collp"
            ImageControlId="Image_Potn_Collp"  CollapsedImage="~/Images/ArrowTip_Down.png" ExpandedImage="~/Images/ArrowTip_Parallel.png"   ExpandControlID="LinkButton_Potn_Collp"
             CollapseControlID="LinkButton_Potn_Collp"/>     
                 </ContentTemplate>
    </asp:UpdatePanel>
        <br />
    <asp:UpdatePanel ID="UpdatePanel_Inv_Collp" runat="server">
    <ContentTemplate>
                                            <table width="100%">
                                                                  <tr>
                                                                <td background="../Images/menu_bg.gif">
                                                                <asp:LinkButton ID="LinkButton_Inv_Collp" runat="server" ForeColor="White" 
                                                                        ToolTip="Click to expand or collapse">All Invoices Sent By Us</asp:LinkButton>
                                                                        <div style="float: right; vertical-align: middle;">
                                                                    <asp:Image ID="Image_Inv_Collp" runat="server"/>
                                                                    </div>
                                                                    </td>
                                                            </tr>
                                                            </table> 
                <asp:Panel  ID="Panel_Inv_Collp" runat="server">
                <br />
                <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title">All Invoices Sent By Us </h3> </div>
              <div class="panel-body">  
            <asp:Panel ID="Panel_All_Invoice" runat="server" 
            style="font-family: Andalus">
            <div align="center">
                                  <div class="panel panel-success">
            <div class="panel-heading">
              <h3 class="panel-title">Filter By</h3>
            </div>
            <div class="panel-body">
                        <table class="style6">
                            <tr>
                                <td>
                                    RFQ#:
                                    <asp:TextBox class="form-control" ID="TextBox_RFQ_No" runat="server" style="font-family: Andalus" 
                                        Width="20%"></asp:TextBox>
                                    &nbsp;Invoice No:
                                    <asp:TextBox class="form-control" ID="TextBox_Inv_No" runat="server" style="font-family: Andalus" 
                                        Width="20%"></asp:TextBox>
                                    &nbsp;<asp:LinkButton ID="LinkButton_Tran_No" runat="server" 
                                        ToolTip="Any invoice associated with this transaction no will be displayed. The filter will look for exact match of the transaction no">Transaction No:</asp:LinkButton>
                                    <asp:TextBox class="form-control" ID="TextBox_Tran_No" runat="server" style="font-family: Andalus" 
                                        Width="20%"></asp:TextBox>
                                    <br /><br />Category:
                                    <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Prod_Cat" runat="server" 
                                        style="font-family: Andalus">
                                    </asp:DropDownList>
                                    &nbsp;Delivery Status:
                                    <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Deliv_Stat" runat="server" 
                                        style="font-family: Andalus">
                                    </asp:DropDownList>
                                    &nbsp;Payment Status:
                                    <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Pmnt_Stat" runat="server" 
                                        style="font-family: Andalus">
                                    </asp:DropDownList>
                                    &nbsp;Customer:&nbsp;<asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Contact_Inv" runat="server" 
                                        style="font-family: Andalus">
                                    </asp:DropDownList>
                                    <br />
                                    <br />
                                    Invoice Date (From) :
                                    <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_From_Date_Invoice" runat="server" 
                                        ValidationGroup="DueDate" Width="20%"></asp:TextBox>
                                    <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_From_Date_CalendarExtender_Inv_From_Date" 
                                        runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton_Inv_From_Date" 
                                        TargetControlID="TextBox_From_Date_Invoice">
                                    </ajaxtoolkit:CalendarExtender>
                                    &nbsp;&nbsp; Invoice Date (To) :&nbsp;
                                    <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_To_Date_Inv" runat="server" 
                                        ValidationGroup="DueDate" Width="20%"></asp:TextBox>
                                    <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_To_Date_CalendarExtender_Inv_To_Date" 
                                        runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton_To_Date_Inv" 
                                        TargetControlID="TextBox_To_Date_Inv">
                                    </ajaxtoolkit:CalendarExtender>
                                    &nbsp;&nbsp;<asp:Button ID="Button_Filter_All_Inv" runat="server" 
                                        onclick="Button_Filter_All_Inv_Click" 
                                        style="font-family: Andalus; " Text="Filter" class="btn btn-sm btn-success"
                                        ValidationGroup="DueDate" />
                                    &nbsp;<asp:CompareValidator ID="CompareValidator5" runat="server" 
                                        ControlToCompare="TextBox_From_Date_Invoice" 
                                        ControlToValidate="TextBox_To_Date_Inv" Display="Dynamic" 
                                        ErrorMessage="to date can not be earlier than from date" ForeColor="Red" 
                                        Operator="GreaterThan" Type="Date" ValidationGroup="DueDate"></asp:CompareValidator>
                                </td>
                            </tr>
                        </table>
                            </div>
                                </div> 
                <br />
                <div align="center">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="GridView_Invoice" 
                            runat="server" AllowPaging="True" 
                                AutoGenerateColumns="False" 
                                CellPadding="4" 
                                GridLines="None" 
                                CssClass="table table-striped table-bordered table-hover tableShadow" 
                                Height="30%" 
                                onpageindexchanging="GridView_Invoice_PageIndexChanging" 
                                onrowdatabound="GridView_Invoice_RowDataBound" PageSize="5" 
                                style="font-size: small" Visible="False" Width="90%" 
                                onselectedindexchanged="GridView_Invoice_SelectedIndexChanged" 
                                onrowediting="GridView_Invoice_RowEditing" 
                                onrowcancelingedit="GridView_Invoice_RowCancelingEdit" 
                                onrowupdating="GridView_Invoice_RowUpdating" AllowSorting="True" 
                                onsorting="GridView_Invoice_Sorting">
                                
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:RadioButton ID="inv_radio" runat="server" AutoPostBack="true" 
                                                GroupName="rain" OnCheckedChanged="GridView_Inv_RadioSelect" 
                                                OnClick="javascript:Selrdbtn(this.id)" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:CommandField ShowEditButton="True" />
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
                                            <br />
                                            <asp:Label ID="Label_Related_PO" runat="server" 
                                                Text='<%# Eval("Related_PO") %>' Visible="False"></asp:Label>
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
                                    <asp:TemplateField ConvertEmptyStringToNull="False" HeaderText="Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="Label_Amount" runat="server" Text='<%# Eval("Amount") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice Details">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton_Show_Inv_Invoice_Grid" runat="server" 
                                                oncommand="LinkButton_Show_Inv_Invoice_Grid_Command" 
                                                onclientclick="javascript:unSelrdbtn()">Show!</asp:LinkButton>
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
                                                oncommand="LinkButton_Pmnt_Det_Inv_Command" 
                                                onclientclick="javascript:unSelrdbtn()">Show!</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Defects">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton_Show_Defect1" runat="server" 
                                                oncommand="LinkButton_Show_Defect1_Command" 
                                                onclientclick="javascript:unSelrdbtn()">Show!</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Approval Status">
                                        <EditItemTemplate>
                                            <asp:Label ID="Label_Apprvl_Stat" runat="server" Text='<%# Eval("approval") %>'></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton_Apprvl_Stat" runat="server" 
                                                Text='<%# Eval("approval") %>' oncommand="LinkButton_Apprvl_Stat_Command" 
                                                onclientclick="javascript:unSelrdbtn()"></asp:LinkButton>
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
                            <asp:Label ID="Label_Invoice_Grid_Access" runat="server" ForeColor="Red" 
                                style="font-size: small" Visible="False"></asp:Label>
                            <br />
                            <asp:Button ID="Button_Inv_Refresh_Hidden" runat="server" ForeColor="#336600" style="display:none" 
                                oncommand="Button_Inv_Refresh_Hidden_Command" Text="Hidden" />
                            <asp:Button ID="Button_Inv_Refresh" runat="server" ForeColor="#336600" 
                                onclick="Button_Inv_Refresh_Click" 
                                style="font-family: Andalus; font-size: x-small" Text="Refresh Data!" />
                            &nbsp;<asp:Button ID="Button_Create_Invoice_Manual" runat="server" class="btn btn-sm btn-success"
                                onclientclick="window.open('Popups/Sale/createInvoiceSelectRFQ.aspx','CreateInvMan','width=1000,height=1000,left=100,right=500,scrollbars=1,resizable=1',true);" 
                                style="font-family: Andalus" Text="Create New Invoice!" />
                            &nbsp;<asp:Button ID="Button_Workflow_Tree_Inv" runat="server" Enabled="False" class="btn btn-sm btn-success"
                                onclick="Button_Workflow_Tree_Inv_Click" style="font-family: Andalus" 
                                Text="Approval History" />
                            &nbsp;<asp:Button ID="Button_Notes_Inv" runat="server" Enabled="False" class="btn btn-sm btn-success"
                                onclick="Button_Notes_Inv_Click" style="font-family: Andalus" Text="Notes!" />
                            &nbsp;<asp:Button ID="Button_Audit_Inv" runat="server" Enabled="False" class="btn btn-sm btn-success"
                                onclick="Button_Audit_Inv_Click" style="font-family: Andalus" Text="Logs!" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                    &nbsp;</div>
                
            </div>
        </asp:Panel>
                             </div>
                      </div>
                </asp:Panel>
                                                            <ajaxtoolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender_Inv" CollapsedText="Show Details..." ExpandedText="Hide Details" runat="server" Collapsed="true" AutoCollapse="false" TargetControlID="Panel_Inv_Collp"
            ImageControlId="Image_Inv_Collp"  CollapsedImage="~/Images/ArrowTip_Down.png" ExpandedImage="~/Images/ArrowTip_Parallel.png"   ExpandControlID="LinkButton_Inv_Collp"
             CollapseControlID="LinkButton_Inv_Collp"/>     
    </ContentTemplate>
    </asp:UpdatePanel>  
                      <br />
    <asp:UpdatePanel ID="UpdatePanel_SO_Collp" runat="server">
    <ContentTemplate>

                                                                  <table width="100%">
                                                                  <tr>
                                                                <td background="../Images/menu_bg.gif">
                                                                <asp:LinkButton ID="LinkButton_SO_Collp" runat="server" ForeColor="White" 
                                                                        ToolTip="Click to expand or collapse">All Sales Orders</asp:LinkButton>
                                                                        <div style="float: right; vertical-align: middle;">
                                                                    <asp:Image ID="Image_SO_Collp" runat="server"/>
                                                                    </div>
                                                                    </td>
                                                            </tr>
                                                            </table> 
                <asp:Panel  ID="Panel_SO_Collp" runat="server">
                <br />
                <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title">All Sales Orders </h3> </div>
              <div class="panel-body">  
            <asp:Panel ID="Panel1" runat="server" 
            style="font-family: Andalus">
            <div align="center">
                         <div class="panel panel-success">
            <div class="panel-heading">
              <h3 class="panel-title">Filter By</h3>
            </div>
            <div class="panel-body">
                        <table class="style6">
                            <tr>
                                <td align="center">
                                    RFQ#:
                                    <asp:TextBox class="form-control" ID="TextBox_rfq_no_po" runat="server" style="font-family: Andalus" 
                                        Width="20%"></asp:TextBox>
                                    &nbsp;SO No:
                                    <asp:TextBox class="form-control" ID="TextBox_po" runat="server" style="font-family: Andalus" 
                                        Width="20%"></asp:TextBox>
                                    &nbsp;Customer:&nbsp;<asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Client_SO" runat="server" 
                                        style="font-family: Andalus">
                                    </asp:DropDownList>
                                    <br />
                                    <br />
                                    SO Date (From) :
                                    <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_From_Date_PO" runat="server" 
                                        ValidationGroup="DueDatePO" Width="20%"></asp:TextBox>
                                    <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_From_Date_CalendarExtender_PO_From_Date" 
                                        runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton_PO_From_Date" 
                                        TargetControlID="TextBox_From_Date_PO">
                                    </ajaxtoolkit:CalendarExtender>
                                    &nbsp;&nbsp; SO Date (To) :&nbsp;
                                    <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_To_Date_PO" runat="server" 
                                        ValidationGroup="DueDatePO" Width="20%"></asp:TextBox>
                                    <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_To_Date_CalendarExtender_PO_To_Date" 
                                        runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton_To_Date_PO" 
                                        TargetControlID="TextBox_To_Date_PO">
                                    </ajaxtoolkit:CalendarExtender>
                                    &nbsp;&nbsp;<asp:Button ID="Button_Filter_SO" runat="server" class="btn btn-sm btn-success"
                                        onclick="Button_Filter_SO_Click" style="font-family: Andalus" 
                                        Text="Filter" ValidationGroup="DueDate" />
                                    &nbsp;<asp:CompareValidator ID="CompareValidator1" runat="server" 
                                        ControlToCompare="TextBox_From_Date_PO" 
                                        ControlToValidate="TextBox_To_Date_PO" Display="Dynamic" 
                                        ErrorMessage="to date can not be earlier than from date" ForeColor="Red" 
                                        Operator="GreaterThan" Type="Date" ValidationGroup="DueDatePO"></asp:CompareValidator>
                                </td>
                            </tr>
                        </table>
                         </div>
                                </div> 
                <br />
                <div align="center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="GridView_SO" 
                            runat="server" AllowPaging="True" 
                                AutoGenerateColumns="False" 
                                CellPadding="4" 
                                CssClass="table table-striped table-bordered table-hover tableShadow"                                            
                                GridLines="None" 
                                Height="30%" 
                                PageSize="5" 
                                style="font-size: small" Visible="False" Width="70%" 
                                onselectedindexchanged="GridView_SO_SelectedIndexChanged" 
                                AllowSorting="True" onpageindexchanging="GridView_SO_PageIndexChanging" 
                                onrowdatabound="GridView_SO_RowDataBound" 
                                onrowcancelingedit="GridView_SO_RowCancelingEdit" 
                                onrowediting="GridView_SO_RowEditing" onrowupdating="GridView_SO_RowUpdating" 
                                onsorting="GridView_SO_Sorting">                                
                                <AlternatingRowStyle CssClass="alt" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:RadioButton ID="so_radio" runat="server" AutoPostBack="true" 
                                                GroupName="rain" OnCheckedChanged="GridView_SO_RadioSelect" 
                                                OnClick="javascript:Selrdbtn(this.id)" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:CommandField ShowEditButton="True" />
                                    <asp:TemplateField HeaderText="RFQ#">
                                        <ItemTemplate>
                                            <asp:Label ID="Label_RFQId1" runat="server" Text='<%# Eval("RFQNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PO#">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton_PO_Purchase_Order" runat="server" 
                                                CommandArgument="<%# Container.DataItemIndex %>" 
                                                onclientclick="javascript:unSelrdbtn()" 
                                                oncommand="LinkButton_SO_Purchase_Order_Command" 
                                                Text='<%# Eval("SO_No") %>'></asp:LinkButton>
                                            <br />
                                            <br />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SO Date" ShowHeader="False" 
                                        SortExpression="SO_Date_Ticks">
                                        <ItemTemplate>
                                            <asp:Label ID="Label_PO_Date" runat="server" Text='<%# Eval("SO_Date") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle Font-Underline="True" ForeColor="black" />
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
                                    <asp:TemplateField ConvertEmptyStringToNull="False" HeaderText="Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="Label_Amount" runat="server" Text='<%# Eval("Amount") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CreationMode" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="Label_Create_Mode" runat="server" 
                                                Text='<%# Eval("CreateMode") %>'></asp:Label>
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
                            <asp:Label ID="Label_PO_Grid_Access" runat="server" ForeColor="Red" 
                                style="font-size: small" Visible="False"></asp:Label>
                            <br />
                                <asp:Button ID="Button_PO_Refresh_Hidden" runat="server" style="display:none"
                            ForeColor="#336600" onclick="Button_SO_Refresh_Hidden_Click" onclientclick="javascript:unSelrdbtn()" 
                            Text="Hidden" />
                            <asp:Button ID="Button_PO_Refresh" runat="server" ForeColor="#336600" 
                                onclick="Button_SO_Refresh_Click" 
                                style="font-family: Andalus; font-size: x-small" Text="Refresh Data!" />
                            &nbsp;<asp:Button ID="Button_Create_PO" runat="server" 
                                class="btn btn-sm btn-success" 
                                style="font-family: Andalus" Text="Create New PO!" 
                                onclick="Button_Create_PO_Click" />
                            &nbsp;<asp:Button ID="Button_Notes_PO" runat="server" Enabled="False" class="btn btn-sm btn-success"
                                onclick="Button_Notes_SO_Click" style="font-family: Andalus" 
                                Text="Notes!" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                    </div>
                
            </div>
        </asp:Panel>
                      </div>
                      </div>
                </asp:Panel>
                                                                                  <ajaxtoolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender_SO_Collp" CollapsedText="Show Details..." ExpandedText="Hide Details" runat="server" Collapsed="true" AutoCollapse="false" TargetControlID="Panel_SO_Collp"
            ImageControlId="Image_SO_Collp"  CollapsedImage="~/Images/ArrowTip_Down.png" ExpandedImage="~/Images/ArrowTip_Parallel.png"   ExpandControlID="LinkButton_SO_Collp"
             CollapseControlID="LinkButton_SO_Collp"/>     
                 </ContentTemplate>
    </asp:UpdatePanel>
                          <div align="center" id="hover" 
        style="position:fixed; right:10px; top:250px; z-index:20; height:auto;">
        <asp:Button ID="Button_Clear_Filter_All" runat="server" 
                                                                        class="btn btn-sm btn-danger"
                                                                        style="font-family: Andalus" Text="Clear Filter" 
                                                                        onclientclick="ClearAllControls();return false;" />
        </div>
        <br />
</asp:Content>
