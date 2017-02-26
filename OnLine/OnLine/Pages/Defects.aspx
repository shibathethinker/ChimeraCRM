<%@ Page Title="" Language="C#" Theme="ThemeBlue" MasterPageFile="~/Pages/Site1.Master" AutoEventWireup="true" CodeBehind="Defects.aspx.cs" Inherits="OnLine.Pages.Defects" %>
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

    var objAgent = navigator.userAgent;
    var objbrowserName = navigator.appName;
    var objOffsetName, objOffsetVersion;
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

    function myFunc(obj) {
        if ((objOffsetVersion = objAgent.indexOf("Chrome")) != -1) {
            objbrowserName = "Chrome";
            alert(obj.text);
            } else if ((objOffsetVersion = objAgent.indexOf("MSIE")) != -1) {
                objbrowserName = "Microsoft Internet Explorer";
                alert(obj.value);
            } else if ((objOffsetVersion = objAgent.indexOf("Firefox")) != -1) {
                objbrowserName = "Firefox";
                alert(obj.text);
            }
        }

</script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
                        <asp:Label ID="Label_Defect_Screen_Access" runat="server" ForeColor="Red" 
                            style="font-size: small; font-family: Andalus;" 
        Visible="False"></asp:Label>
        <asp:UpdatePanel ID="UpdatePanel_Incm_Collp" runat="server">
        <ContentTemplate>


                                                                            <table width="100%">
                                                                  <tr>
                                                                <td background="../Images/Collapse_bg.png">
                                                                <asp:LinkButton ID="LinkButton_Incm_Collp" runat="server" ForeColor="White" 
                                                                        ToolTip="Click to expand or collapse">Defects Sent To Us</asp:LinkButton>
                                                                        <div style="float: right; vertical-align: middle;">
                                                                    <asp:Image ID="Incm_Collapse_Img" runat="server"/>
                                                                    </div>
                                                                    </td>
                                                            </tr>
                                                            </table> 
    <asp:Panel  ID="Panel_Incm_Collp" runat="server">
    <br />
                  <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title">Defects Sent To Us</h3>
            </div>
            <div class="panel-body">  
    <asp:Panel ID="Panel_All_Incoming_Defects" runat="server" 
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
                        Defect No:
                        <asp:TextBox class="form-control" ID="TextBox_Defect_No_Incm_Defect" runat="server" 
                            style="font-family: Andalus" Width="20%"></asp:TextBox>
                        &nbsp;RFQ No:
                        <asp:TextBox class="form-control" ID="TextBox_RFQ_No_Incm_Defect" runat="server" 
                            style="font-family: Andalus" Width="20%"></asp:TextBox>
                        &nbsp;Invoice No:
                        <asp:TextBox class="form-control" ID="TextBox_Inv_No_Incm_Defect" runat="server" 
                            style="font-family: Andalus" Width="20%"></asp:TextBox>
                        <br /><br />Defect Severity:<asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Incm_Defect_Sev" 
                            runat="server" style="font-family: Andalus">
                        </asp:DropDownList>
                        &nbsp;Defect Status:
                        <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Incm_Defect_Stat" runat="server" 
                            style="font-family: Andalus">
                        </asp:DropDownList>
                        &nbsp; Defect Resolution Stat:
                        <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Incm_Defect_Resol_Stat" runat="server" 
                            style="font-family: Andalus">
                        </asp:DropDownList>
                        <br /><br /> From Date (Defect Raised) :
                        <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_From_Date_Incm_Defect_Raised" runat="server" 
                             ValidationGroup="IncomingDefectFilter" Width="20%"></asp:TextBox>
                        <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_From_Date_Incm_Defect_Raised_CalendarExtender" 
                            runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton3" 
                            TargetControlID="TextBox_From_Date_Incm_Defect_Raised">
                        </ajaxtoolkit:CalendarExtender>
                        &nbsp;&nbsp; To Date (Defect Raised) :&nbsp;
                        <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_To_Date_Incm_Defect_Raised" runat="server" 
                             ValidationGroup="IncomingDefectFilter" Width="20%"></asp:TextBox>
                        <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_To_Date_Incm_Defect_Raised_CalendarExtender" 
                            runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton4" 
                            TargetControlID="TextBox_To_Date_Incm_Defect_Raised">
                        </ajaxtoolkit:CalendarExtender>
                        &nbsp;&nbsp;&nbsp;Assigned To:&nbsp;<asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Assigned_To" 
                            runat="server" style="font-family: Andalus">
                        </asp:DropDownList>
                        &nbsp;<asp:Button ID="Button_Filter_Incoming_Defects" runat="server" class="btn btn-sm btn-success"
                            onclick="Button_Filter_Incoming_Defects_Click" style="font-family: Andalus" 
                            Text="Filter" ValidationGroup="IncomingDefectFilter" />
                        &nbsp;<asp:CompareValidator ID="CompareValidator2" runat="server" 
                            ControlToCompare="TextBox_From_Date_Incm_Defect_Raised" 
                            ControlToValidate="TextBox_To_Date_Incm_Defect_Raised" Display="Dynamic" 
                            ErrorMessage="to date can not be earlier than from date" ForeColor="Red" 
                            Operator="GreaterThan" Type="Date" ValidationGroup="IncomingDefectFilter"></asp:CompareValidator>
                    </td>
                </tr>
            </table>      
                        </div>
            </div>         
            <br />
            <div align="center">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                    <asp:Label ID="Label_Incm_msg" runat="server" Visible="False"></asp:Label>
                        <asp:GridView ID="GridView_Incoming_Defects" 
                        runat="server" AllowPaging="True" 
                    AutoGenerateColumns="False" CellPadding="4" 
                    GridLines="None" 
                                        CssClass="table table-striped table-bordered table-hover tableShadow" 
                       PageSize="5" Visible="False" 
                        style="font-size: small" Height="30%" Width="90%"                         
                            onpageindexchanging="GridView_Incoming_Defects_PageIndexChanging" 
                            onrowcancelingedit="GridView_Incoming_Defects_RowCancelingEdit" 
                            onrowdatabound="GridView_Incoming_Defects_RowDataBound" 
                            onrowediting="GridView_Incoming_Defects_RowEditing" 
                            onrowupdating="GridView_Incoming_Defects_RowUpdating" 
                            onselectedindexchanged="GridView_Incoming_Defects_SelectedIndexChanged" 
                            AllowSorting="True" onsorting="GridView_Incoming_Defects_Sorting">                          
                            
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:RadioButton ID="incm_radio" runat="server" AutoPostBack="true" 
                                            GroupName="rain" OnCheckedChanged="GridView_Defect_RadioSelect" 
                                            OnClick="javascript:Selrdbtn(this.id)" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField ShowEditButton="True" />
                                <asp:TemplateField HeaderText="Defect#">
                                    <ItemTemplate>
                                        <asp:Label ID="Label_Defect_Id" runat="server" Text='<%# Eval("DefectId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="RFQ#">
                                    <EditItemTemplate>
                                        <asp:TextBox class="form-control" ID="TextBox_RFQ_No_Edit" runat="server" Enabled="False" 
                                            Height="28px" style="font-family: Andalus" Text='<%# Eval("RFQId") %>' 
                                            Width="175px"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label_RFQNo" runat="server" Text='<%# Eval("RFQId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Invoice#">
                                    <EditItemTemplate>
                                        <asp:TextBox class="form-control" ID="TextBox_Invoice_No_Edit" runat="server" Enabled="False" 
                                            Height="28px" style="font-family: Andalus" Text='<%# Eval("InvNo") %>' 
                                            Width="175px"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label_InvoiceNo" runat="server" Text='<%# Eval("InvNo") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="Link_Descr" runat="server" 
                                        onmousehover="myFunc(this);"                                            
                                            Text='<%# Eval("descr") %>' value='<%# Eval("descr") %>' Visible="False"></asp:LinkButton>
                                        <asp:LinkButton ID="Link_Descr_Short" runat="server" 
                                            onmousehover="myFunc(this);" Text='<%# Eval("descr_short") %>' 
                                            value='<%# Eval("descr") %>' onclick="Link_Descr_Short_Click"></asp:LinkButton>
                                    </ItemTemplate>
                                      <EditItemTemplate>
                                        <asp:TextBox class="form-control" ID="TextBox_Descr_Edit" runat="server" Enabled="False" 
                                            Height="67px" style="font-family: Andalus" Text='<%# Eval("descr") %>' 
                                            TextMode="MultiLine" Width="329px"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Submit Date" SortExpression="Submit Date Ticks">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton_Location" runat="server" 
                                            onclientclick="window.open('Popups/Sale/AllLead_Location.aspx','LeadLocation','status=1,width=500,height=200,left=500,right=500',true);">Show!</asp:LinkButton>
                                        <asp:Label ID="Label_Submit_Date" runat="server" 
                                            Text='<%# Eval("Submit Date") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label_Submit_Date" runat="server" 
                                            Text='<%# Eval("Submit Date") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle ForeColor="black" Font-Underline="True" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Close Date" SortExpression="Close Date Ticks">
                                    <ItemTemplate>
                                        <asp:Label ID="Label_Close_Date" runat="server" 
                                            Text='<%# Eval("Close Date") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle ForeColor="black" Font-Underline="True" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer Details">
                                    <EditItemTemplate>
                                        <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Incm_Defect_Cust_Det_Edit" runat="server" 
                                            style="font-family: Andalus">
                                        </asp:DropDownList>
                                        <asp:Label ID="Label_Customer_Name_Edit_Incoming" runat="server" 
                                            Text='<%# Eval("CustName") %>' Visible="False"></asp:Label>
                                        <asp:Label ID="Label_EntId_Edit" runat="server" Text='<%# Eval("entId") %>' 
                                            Visible="False"></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton_Customer" runat="server" 
                                            Text='<%# Eval("CustName") %>' oncommand="LinkButton_Customer_Command" 
                                            onclientclick="javascript:unSelrdbtn()"></asp:LinkButton>
                                        <br />
                                        <asp:Label ID="Label_EntId" runat="server" Text='<%# Eval("entId") %>' 
                                            Visible="False"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Defect Status">
                                    <EditItemTemplate>
                                        &nbsp;
                                        <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Incm_Defect_Stat_Edit" runat="server" 
                                            style="font-family: Andalus">
                                        </asp:DropDownList>
                                        <asp:Label ID="Label_Defect_Stat_Edit" runat="server" 
                                            Text='<%# Eval("Defect_Stat") %>' Visible="False"></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label_Defect_Stat" runat="server" 
                                            Text='<%# Eval("Defect_Stat") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status Reason">
                                    <EditItemTemplate>
                                        <asp:TextBox class="form-control" ID="TextBox_Defect_Stat_Reason_Edit" runat="server" Height="67px" 
                                            Text='<%# Eval("Defect_Stat_Reason") %>' TextMode="MultiLine" Width="329px"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label_Defect_Stat_Reason" runat="server" 
                                            Text='<%# Eval("Defect_Stat_Reason") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Assigned To">
                                    <EditItemTemplate>
                                        <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Incm_Defect_Assgn_To_Edit" runat="server" 
                                            style="font-family: Andalus">
                                        </asp:DropDownList>
                                        &nbsp;<asp:Label ID="Label_Assgn_To_Edit" runat="server" 
                                            Text='<%# Eval("Assigned_To") %>' Visible="False"></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton_Assgn_To" runat="server" 
                                            Text='<%# Eval("Assigned_To") %>' oncommand="LinkButton_Assgn_To_Command" 
                                            onclientclick="javascript:unSelrdbtn()"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Severity">
                                    <EditItemTemplate>
                                        <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Incm_Defect_Sev_Edit" runat="server" 
                                            style="font-family: Andalus">
                                        </asp:DropDownList>
                                        <asp:Label ID="Label_Sev_Edit" runat="server" Text='<%# Eval("Severity") %>' 
                                            Visible="False"></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label_Sev" runat="server" Text='<%# Eval("Severity") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Defect Resolution Status">
                                    <ItemTemplate>
                                        <asp:Label ID="Label_Defect_Resol_Stat" runat="server" 
                                            Text='<%# Eval("Defect_Resol_Stat") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Incm_Defect_Resol_Stat_Edit" runat="server" 
                                            style="font-family: Andalus">
                                        </asp:DropDownList>
                                        &nbsp;<asp:Label ID="Label_Defect_Resol_Stat_Edit" runat="server" 
                                            Text='<%# Eval("Defect_Resol_Stat") %>' Visible="False"></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="All Communications">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="LinkButton_All_Comm" runat="server" 
                                            oncommand="LinkButton_All_Comm_Command" 
                                            onclientclick="javascript:unSelrdbtn()">Show!</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="Label_Amount" runat="server" Text='<%# Eval("Amount") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox class="form-control" ID="Textbox_Edit_Amount" runat="server" 
                                            Text='<%# Eval("Amount") %>'></asp:TextBox>
                                    </EditItemTemplate>
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
                                <asp:TemplateField HeaderText="Documents" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="Label_Doc_Name_Hidden" runat="server" 
                                            Text='<%# Eval("docNameHidden") %>' Visible="False"></asp:Label>
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
                        <asp:Label ID="Label_Incoming_Defect_Grid_Access" runat="server" 
                            ForeColor="Red" style="font-size: small" Visible="False"></asp:Label>
                        <br />
                         <asp:Button ID="Button_Incm_Refresh_Hidden" runat="server" style="display:none"  
                            ForeColor="#336600" onclick="Button_Incm_Refresh_Hidden_Click" 
                            Text="Hidden" />
                        <asp:Button ID="Button_Inc_Defect_Refresh" runat="server" ForeColor="#336600" 
                            onclick="Button_Inc_Defect_Refresh_Click" 
                            style="font-family: Andalus; font-size: x-small" Text="Refresh Data!" />
                        &nbsp;<asp:Button ID="Button_Create_Defect_Incm" runat="server" class="btn btn-sm btn-success"
                            onclick="Button_Create_Defect_Incm_Click" style="font-family: Andalus" 
                            Text="Enter New Defect Details!" />
                        &nbsp;<asp:Button ID="Button_Incm_Doc" runat="server" Enabled="False" class="btn btn-sm btn-success"
                            onclick="Button_Incm_Doc_Click" style="font-family: Andalus" Text="Documents" />
                        &nbsp;<asp:Button ID="Button_Notes_Incm_Def" runat="server" Enabled="False" class="btn btn-sm btn-success"
                            onclick="Button_Notes_Incm_Def_Click" style="font-family: Andalus" 
                            Text="Notes!" />
                        &nbsp;<asp:Button ID="Button_Audit_Incm_Defect" runat="server" Enabled="False" class="btn btn-sm btn-success"
                            onclick="Button_Audit_Incm_Defect_Click" style="font-family: Andalus" 
                            Text="Logs!" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                </div>
        </div>
    </asp:Panel>
            </div>
        </div>
    </asp:Panel>
                                <ajaxtoolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender_Incm" CollapsedText="Show Details..." ExpandedText="Hide Details" runat="server" Collapsed="true" AutoCollapse="false" TargetControlID="Panel_Incm_Collp"
            ImageControlId="Incm_Collapse_Img"  CollapsedImage="~/Images/ArrowTip_Down.png" ExpandedImage="~/Images/ArrowTip_Parallel.png"   ExpandControlID="LinkButton_Incm_Collp"
             CollapseControlID="LinkButton_Incm_Collp"/>     
                     </ContentTemplate>
        </asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="UpdatePanel_Outg_Collp" runat="server">
    <ContentTemplate>

                                                                    <table width="100%">
                                                                  <tr>
                                                                <td background="../Images/menu_bg.gif">
                                                                <asp:LinkButton ID="LinkButton_Outg_Collp" runat="server" ForeColor="White" 
                                                                        ToolTip="Click to expand or collapse">Defects Sent By Us</asp:LinkButton>
                                                                        <div style="float: right; vertical-align: middle;">
                                                                    <asp:Image ID="Outg_Collapse_Img" runat="server"/>
                                                                    </div>
                                                                    </td>
                                                            </tr>
                                                            </table> 
    <asp:Panel  ID="Panel_Outg_Collp" runat="server">
    <br />
               <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title">Defects Sent By Us</h3>
            </div>
            <div class="panel-body">  
    <asp:Panel ID="Panel_All_Outgoing_Defects" runat="server" 
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
                                Defect No:
                                <asp:TextBox class="form-control" ID="TextBox_Defect_No_Outgoing_Defect" runat="server" 
                                    style="font-family: Andalus" Width="20%"></asp:TextBox>
                                &nbsp;RFQ No:
                                <asp:TextBox class="form-control" ID="TextBox_RFQ_No_Outgoing_Defect" runat="server" 
                                    style="font-family: Andalus" Width="20%"></asp:TextBox>
                                &nbsp;Invoice No:
                                <asp:TextBox class="form-control" ID="TextBox_Inv_No_Outgoing_Defect" runat="server" 
                                    style="font-family: Andalus" Width="20%"></asp:TextBox>
                                <br /><br />Defect Severity:<asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Outg_Defect_Sev" 
                                    runat="server" style="font-family: Andalus">
                                </asp:DropDownList>
                                &nbsp;Defect Status:
                                <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Outgoing_Defect_Stat" runat="server" 
                                    style="font-family: Andalus">
                                </asp:DropDownList>
                                &nbsp; Defect Resolution Stat:
                                <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Outgoing_Defect_Resol_Stat" runat="server" 
                                    style="font-family: Andalus">
                                </asp:DropDownList>                                                        
                                   <br />  <br /> From Date (Defect Raised) :
                                <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_From_Date_Outgoing_Defect_Raised" runat="server" 
                                     ValidationGroup="OutgoingDefectFilter" Width="20%"></asp:TextBox>
                                <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_From_Date_Outgoing_Defect_Raised_CalendarExtender" 
                                    runat="server" Format="yyyy-MM-dd" 
                                    PopupButtonID="ImageButton_From_Date_Out_Defect" 
                                    TargetControlID="TextBox_From_Date_Outgoing_Defect_Raised">
                                </ajaxtoolkit:CalendarExtender>
 
                                &nbsp;&nbsp; To Date (Defect Raised) :&nbsp;
                                <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_To_Date_Outgoing_Defect_Raised" runat="server" 
                                     ValidationGroup="OutgoingDefectFilter" Width="20%"></asp:TextBox>
                                <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_To_Date_Outgoing_Defect_Raised_CalendarExtender" 
                                    runat="server" Format="yyyy-MM-dd" 
                                    PopupButtonID="ImageButton_To_Date_Out_Defect" 
                                    TargetControlID="TextBox_To_Date_Outgoing_Defect_Raised">
                                </ajaxtoolkit:CalendarExtender>
                                &nbsp;&nbsp;&nbsp;<asp:Button ID="Button_Filter_Outgoing_Defects" runat="server" class="btn btn-sm btn-success"
                                    onclick="Button_Filter_Outgoing_Defects_Click" style="font-family: Andalus" 
                                    Text="Filter" ValidationGroup="OutgoingDefectFilter" />
                                &nbsp;<asp:CompareValidator ID="CompareValidator4" runat="server" 
                                    ControlToCompare="TextBox_From_Date_Outgoing_Defect_Raised" 
                                    ControlToValidate="TextBox_To_Date_Outgoing_Defect_Raised" Display="Dynamic" 
                                    ErrorMessage="to date can not be earlier than from date" ForeColor="Red" 
                                    Operator="GreaterThan" Type="Date" ValidationGroup="OutgoingDefectFilter"></asp:CompareValidator>
                                &nbsp;</td>
                        </tr>
                    </table>
               </div>
            </div>         
            </div> 
            <br />
            <div align="center">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                    <asp:Label ID="Label_Outg_msg" runat="server" Visible="False"></asp:Label>
                        <asp:GridView ID="GridView_Outgoing_Defects" 
                        runat="server" AllowPaging="True" 
                    AutoGenerateColumns="False" CellPadding="4" 
                    GridLines="None" 
                                        CssClass="table table-striped table-bordered table-hover tableShadow" 
 
                    PageSize="5" Visible="False" 
                        style="font-size: small" Height="30%" Width="90%"                         
                            onpageindexchanging="GridView_Outgoing_Defects_PageIndexChanging" 
                            onrowcancelingedit="GridView_Outgoing_Defects_RowCancelingEdit" 
                            onrowdatabound="GridView_Outgoing_Defects_RowDataBound" 
                            onrowediting="GridView_Outgoing_Defects_RowEditing" 
                            onrowupdating="GridView_Outgoing_Defects_RowUpdating" 
                            onselectedindexchanged="GridView_Outgoing_Defects_SelectedIndexChanged" 
                            AllowSorting="True" onsorting="GridView_Outgoing_Defects_Sorting">
                                                       <Columns>
                                                           <asp:TemplateField>
                                                               <ItemTemplate>
                                                                   <asp:RadioButton ID="outg_radio" runat="server" AutoPostBack="true" 
                                                                       GroupName="rain" OnCheckedChanged="GridView_Out_Defect_RadioSelect" 
                                                                       OnClick="javascript:Selrdbtn(this.id)" />
                                                               </ItemTemplate>
                                                           </asp:TemplateField>
                                                           <asp:CommandField ShowEditButton="True" />
                                                           <asp:TemplateField HeaderText="Defect#">
                                                               <ItemTemplate>
                                                                   <asp:Label ID="Label_Defect_Id" runat="server" Text='<%# Eval("DefectId") %>'></asp:Label>
                                                               </ItemTemplate>
                                                           </asp:TemplateField>
                                                           <asp:TemplateField HeaderText="RFQ#">
                                                               <EditItemTemplate>
                                                                   <asp:TextBox class="form-control" ID="TextBox_RFQ_No_Edit" runat="server" Enabled="False" 
                                                                       Height="28px" style="font-family: Andalus" Text='<%# Eval("RFQId") %>' 
                                                                       Width="175px"></asp:TextBox>
                                                               </EditItemTemplate>
                                                               <ItemTemplate>
                                                                   <asp:Label ID="Label_RFQNo_Outgoing" runat="server" Text='<%# Eval("RFQId") %>'></asp:Label>
                                                               </ItemTemplate>
                                                           </asp:TemplateField>
                                                           <asp:TemplateField HeaderText="Invoice#">
                                                               <EditItemTemplate>
                                                                   <asp:TextBox class="form-control" ID="TextBox_Invoice_No_Edit" runat="server" Height="28px" 
                                                                       Text='<%# Eval("InvNo") %>' Width="175px"></asp:TextBox>
                                                               </EditItemTemplate>
                                                               <ItemTemplate>
                                                                   <asp:Label ID="Label_InvoiceNo_Outgoing" runat="server" 
                                                                       Text='<%# Eval("InvNo") %>'></asp:Label>
                                                               </ItemTemplate>
                                                           </asp:TemplateField>
                                                           <asp:TemplateField HeaderText="Description">
                                                               <ItemTemplate>
                                                                   <asp:LinkButton ID="Link_Descr" runat="server" onmousehover="myFunc(this);" 
                                                                       Text='<%# Eval("descr") %>' value='<%# Eval("descr") %>' Visible="False"></asp:LinkButton>
                                                                   <asp:LinkButton ID="Link_Descr_Short" runat="server" 
                                                                       onmousehover="myFunc(this);" Text='<%# Eval("descr_short") %>' 
                                                                       value='<%# Eval("descr") %>' onclick="Link_Descr_Short_Click_Outg"></asp:LinkButton>
                                                               </ItemTemplate>
                                                               <EditItemTemplate>
                                                                   <asp:TextBox class="form-control" ID="TextBox_Descr_Edit" runat="server" Height="77px" 
                                                                       Text='<%# Eval("descr") %>' TextMode="MultiLine" Width="415px"></asp:TextBox>
                                                               </EditItemTemplate>
                                                           </asp:TemplateField>
                                                           <asp:TemplateField HeaderText="Submit Date" SortExpression="Submit Date Ticks">
                                                               <ItemTemplate>
                                                                   <asp:LinkButton ID="LinkButton_Location0" runat="server" 
                                                                       onclientclick="window.open('Popups/Sale/AllLead_Location.aspx','LeadLocation','status=1,width=500,height=200,left=500,right=500',true);">Show!</asp:LinkButton>
                                                                   <asp:Label ID="Label_Submit_Date_Outgoing" runat="server" 
                                                                       Text='<%# Eval("Submit Date") %>'></asp:Label>
                                                               </ItemTemplate>
                                                               <ItemTemplate>
                                                                   <asp:Label ID="Label_Submit_Date1" runat="server" 
                                                                       Text='<%# Eval("Submit Date") %>'></asp:Label>
                                                               </ItemTemplate>
                                                               <HeaderStyle ForeColor="black" Font-Underline="True" />
                                                           </asp:TemplateField>
                                                           <asp:TemplateField HeaderText="Close Date" SortExpression="Close Date Ticks">
                                                               <ItemTemplate>
                                                                   <asp:Label ID="Label_Close_Date" runat="server" 
                                                                       Text='<%# Eval("Close Date") %>'></asp:Label>
                                                               </ItemTemplate>
                                                               <HeaderStyle ForeColor="black" Font-Underline="True" />
                                                           </asp:TemplateField>
                                                           <asp:TemplateField HeaderText="Supplier Details">
                                                               <EditItemTemplate>
                                                                   <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Outgoing_Defect_Supplier_Edit" 
                                                                       runat="server" style="font-family: Andalus">
                                                                   </asp:DropDownList>
                                                                   <asp:Label ID="Label_Supplier_Name_Outgoing_Edit" runat="server" 
                                                                       Text='<%# Eval("SuplName") %>' Visible="False"></asp:Label>
                                                                   <asp:Label ID="Label_EntId_Edit" runat="server" Text='<%# Eval("entId") %>' 
                                                                       Visible="False"></asp:Label>
                                                               </EditItemTemplate>
                                                               <ItemTemplate>
                                                                   <asp:LinkButton ID="LinkButton_Supplier" runat="server" 
                                                                       Text='<%# Eval("SuplName") %>' oncommand="LinkButton_Supplier_Command" 
                                                                       onclientclick="javascript:unSelrdbtn()"></asp:LinkButton>
                                                                   <br />
                                                                   <asp:Label ID="Label_EntId" runat="server" Text='<%# Eval("entId") %>' 
                                                                       Visible="False"></asp:Label>
                                                               </ItemTemplate>
                                                           </asp:TemplateField>
                                                           <asp:TemplateField HeaderText="Defect Status">
                                                               <EditItemTemplate>
                                                                   &nbsp;
                                                                   <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Outgoing_Defect_Stat_Edit" runat="server" 
                                                                       style="font-family: Andalus">
                                                                   </asp:DropDownList>
                                                                   <asp:Label ID="Label_Defect_Stat_Edit_Outgoing" runat="server" 
                                                                       Text='<%# Eval("Defect_Stat") %>' Visible="False"></asp:Label>
                                                               </EditItemTemplate>
                                                               <ItemTemplate>
                                                                   <asp:Label ID="Label_Defect_Stat_Outgoing" runat="server" 
                                                                       Text='<%# Eval("Defect_Stat") %>'></asp:Label>
                                                               </ItemTemplate>
                                                           </asp:TemplateField>
                                                           <asp:TemplateField HeaderText="Status Reason">
                                                               <EditItemTemplate>
                                                                   <asp:TextBox class="form-control" ID="TextBox_Defect_Stat_Reason_Edit" runat="server" Height="67px" 
                                                                       Text='<%# Eval("Defect_Stat_Reason") %>' TextMode="MultiLine" Width="329px"></asp:TextBox>
                                                               </EditItemTemplate>
                                                               <ItemTemplate>
                                                                   <asp:Label ID="Label_Defect_Stat_Reason" runat="server" 
                                                                       Text='<%# Eval("Defect_Stat_Reason") %>'></asp:Label>
                                                               </ItemTemplate>
                                                           </asp:TemplateField>
                                                           <asp:TemplateField HeaderText="Severity">
                                                               <EditItemTemplate>
                                                                   <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Outgoing_Defect_Sev" runat="server" 
                                                                       style="font-family: Andalus">
                                                                   </asp:DropDownList>
                                                                   <asp:Label ID="Label_Sev_Edit" runat="server" Text='<%# Eval("Severity") %>' 
                                                                       Visible="False"></asp:Label>
                                                               </EditItemTemplate>
                                                               <ItemTemplate>
                                                                   <asp:Label ID="Label_Sev" runat="server" Text='<%# Eval("Severity") %>' 
                                                                       Visible="True"></asp:Label>
                                                               </ItemTemplate>
                                                           </asp:TemplateField>
                                                           <asp:TemplateField HeaderText="Defect Resolution Status">
                                                               <ItemTemplate>
                                                                   <asp:Label ID="Label_Defect_Resol_Stat" runat="server" 
                                                                       Text='<%# Eval("Defect_Resol_Stat") %>'></asp:Label>
                                                               </ItemTemplate>
                                                               <EditItemTemplate>
                                                                   <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Outgoing_Defect_Resol_Stat_Edit" 
                                                                       runat="server" style="font-family: Andalus">
                                                                   </asp:DropDownList>
                                                                   &nbsp;<asp:Label ID="Label_Defect_Resol_Stat_Edit" runat="server" 
                                                                       Text='<%# Eval("Defect_Resol_Stat") %>' Visible="False"></asp:Label>
                                                               </EditItemTemplate>
                                                           </asp:TemplateField>
                                                           <asp:TemplateField HeaderText="All Communications">
                                                               <ItemTemplate>
                                                                   <asp:LinkButton ID="LinkButton_All_Comm0" runat="server"                                                                        
                                                                                                           
                                                                       oncommand="LinkButton_All_Comm_Outg_Command" 
                                                                       onclientclick="javascript:unSelrdbtn()">Show!</asp:LinkButton>
                                                               </ItemTemplate>
                                                           </asp:TemplateField>
                                                           <asp:TemplateField HeaderText="Amount">
                                                               <ItemTemplate>
                                                                   <asp:Label ID="Label_Amount" runat="server" Text='<%# Eval("Amount") %>'></asp:Label>
                                                               </ItemTemplate>
                                                               <EditItemTemplate>
                                                                   <asp:TextBox class="form-control" ID="Textbox_Edit_Amount" runat="server" 
                                                                       Text='<%# Eval("Amount") %>'></asp:TextBox>
                                                               </EditItemTemplate>
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
                                                           <asp:TemplateField HeaderText="Documents" Visible="False">
                                                               <ItemTemplate>
                                                                   <asp:Label ID="Label_Doc_Name_Hidden" runat="server" 
                                                                       Text='<%# Eval("docNameHidden") %>' Visible="False"></asp:Label>
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
                        <asp:Label ID="Label_Outgoing_Defect_Grid_Access" runat="server" 
                            ForeColor="Red" style="font-size: small" Visible="False"></asp:Label>
                        <br />
                        <asp:Button ID="Button_Outg_Refresh_Hidden" runat="server" ForeColor="#336600" style="display:none"  
                            onclick="Button_Outg_Refresh_Hidden_Click" Text="Hidden" />
                        <asp:Button ID="Button_Out_Defect_Refresh" runat="server" ForeColor="#336600" 
                            onclick="Button_Out_Defect_Refresh_Click" 
                            style="font-family: Andalus; font-size: x-small" Text="Refresh Data!" />
                        &nbsp;<asp:Button ID="Button_Create_Defect_Outgoing" runat="server" 
                            onclick="Button_Create_Defect_Outgoing_Click" style="font-family: Andalus" class="btn btn-sm btn-success"
                            Text="Enter New Defect Details!" />
                        &nbsp;<asp:Button ID="Button_Outg_Doc" runat="server" Enabled="False" class="btn btn-sm btn-success"
                            onclick="Button_Outg_Doc_Click" style="font-family: Andalus" Text="Documents" />
                        &nbsp;<asp:Button ID="Button_Notes_Outg_Def" runat="server" Enabled="False" class="btn btn-sm btn-success"
                            onclick="Button_Notes_Outg_Def_Click" style="font-family: Andalus" 
                            Text="Notes!" />
                        &nbsp;<asp:Button ID="Button_Audit_Outg_Defect" runat="server" Enabled="False" class="btn btn-sm btn-success"
                            onclick="Button_Audit_Outg_Defect_Click" style="font-family: Andalus" 
                            Text="Logs!" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                </div>
                </asp:Panel>    
    </div>
    </div>
    </asp:Panel>
                                    <ajaxtoolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender_Outg_Collp" CollapsedText="Show Details..." ExpandedText="Hide Details" runat="server" Collapsed="true" AutoCollapse="false" TargetControlID="Panel_Outg_Collp"
            ImageControlId="Outg_Collapse_Img"  CollapsedImage="~/Images/ArrowTip_Down.png" ExpandedImage="~/Images/ArrowTip_Parallel.png"   ExpandControlID="LinkButton_Outg_Collp"
             CollapseControlID="LinkButton_Outg_Collp"/>  
        </ContentTemplate>
    </asp:UpdatePanel>
    <div align="center" id="hover" 
        style="position:fixed; top:100px; z-index:30; width:60%; height:auto;">
            <asp:UpdatePanel ID="Description" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel_Description" runat="server" Visible="false">
                            <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title"></h3>
            </div>
            <div align="center" class="panel-body" style="background-color:#F3F3F3">
                <asp:Label ID="Label_Description" runat="server" Text="Label"></asp:Label>
                <br />
                <br />
                <asp:Button ID="Button_Hide" runat="server" Text="OK" 
                    class="btn btn-sm btn-success" onclick="Button_Hide_Click"/>
            </div>
            </div>
                </asp:Panel>
            </ContentTemplate>
            </asp:UpdatePanel>
            </div>
     <div align="center" id="Div1" 
        style="position:fixed; right:10px; top:250px; z-index:20; height:auto;">
        <asp:Button ID="Button_Clear_Filter_All" runat="server" 
                                                                        class="btn btn-sm btn-danger"
                                                                        style="font-family: Andalus" Text="Clear Filter" 
                                                                        onclientclick="ClearAllControls();return false;" />
        </div>
        <br />
</asp:Content>
