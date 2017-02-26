<%@ Page Title="" Language="C#" Theme="ThemeBlue" MasterPageFile="~/Pages/Site1.Master" AutoEventWireup="true" CodeBehind="Purchase.aspx.cs" Inherits="OnLine.Pages.Purchase" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <style type="text/css">
        .style6
        {
            width: 90%;        
            /*font-size="small";    */
           /* border: 1px solid #c0c0c0;*/
        }
        .style7
        {
            /*font-size: small;*/
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
    <script language="javascript">
    </script>
                        <asp:Label ID="Label_Purchase_Screen_Access" runat="server" ForeColor="Red" 
                            style="font-size: small; font-family: Andalus;" 
        Visible="False"></asp:Label>
    <asp:UpdatePanel ID="UpdatePanel_Reqr_Collp" runat="server">
        <ContentTemplate>

                                                                    <table width="100%">
                                                                  <tr>
                                                                <td background="../Images/menu_bg.gif">
                                                                <asp:LinkButton ID="LinkButton_Req_Collp" runat="server" ForeColor="White" 
                                                                        ToolTip="Click to expand or collapse">All Requirement Details</asp:LinkButton>
                                                                        <div style="float: right; vertical-align: middle;">
                                                                    <asp:Image ID="Req_Collapse_Img" runat="server"/>
                                                                    </div>
                                                                    </td>
                                                            </tr>
                                                            </table>      
        <asp:Panel  ID="Panel_Reqr_Collp" runat="server">
        <br />
          <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title">All Requirements</h3>
            </div>
            <div class="panel-body">  
        <asp:Panel ID="Panel_All_Requirements" runat="server" Width="95%" 
            style="font-family: Andalus" >
            
            <div align="center">
                      <div class="panel panel-success">
            <div class="panel-heading">
              <h3 class="panel-title panel-filter">Filter By</h3>
            </div>
            <div class="panel-body">
              <table class="style6">
                                                            <tr>
                                                                <td class="style7" align="center">
                                                                    <span class="style7">Requirement Name:
                                                                    <asp:TextBox class="form-control" ID="TextBox_Req_Name" runat="server" style="font-family: Andalus"
                                                                        ></asp:TextBox>
                                                                    &nbsp;Category: </span>
                                                                    <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Category" runat="server" 
                                                                        onselectedindexchanged="DropDownList_Category_SelectedIndexChanged" 
                                                                        style="font-family: Andalus">
                                                                    </asp:DropDownList>
                                                                    <span class="style7">&nbsp; Status: </span>
                                                                    <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Status" runat="server" 
                                                                        onselectedindexchanged="DropDownList_Status_SelectedIndexChanged" 
                                                                        style="font-family: Andalus">
                                                                    </asp:DropDownList>
                                                                    <br />   <br />                                                                
                                                                    <span class="style7">&nbsp;From Date (Due) : </span>
                                                                    <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_From_Date" runat="server"  
                                                                        ValidationGroup="DueDate" Width="20%"></asp:TextBox>
                                                                    
                                                                    <span class="style7">&nbsp;&nbsp; To Date (Due) :&nbsp; </span>
                                                                    <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_To_Date" runat="server"  
                                                                        ValidationGroup="DueDate" Width="20%"></asp:TextBox>

                                                                    <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_To_Date0_CalendarExtender" 
                                                                        runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton2" 
                                                                        TargetControlID="TextBox_To_Date">
                                                                    </ajaxtoolkit:CalendarExtender>
                                                                    &nbsp;<asp:Button ID="Button_Filter_All_Reqr" runat="server" 
                                                                        class="btn btn-sm btn-success" onclick="Button_Filter_All_Reqr_Click" 
                                                                        style="font-family: Andalus" Text="Filter" ValidationGroup="DueDate" />                                                                   
                                                                    &nbsp;<asp:CompareValidator ID="CompareValidator4" runat="server" 
                                                                        ControlToCompare="TextBox_From_Date" ControlToValidate="TextBox_To_Date" 
                                                                        Display="Dynamic" 
                                                                        ErrorMessage="to date can not be earlier than from date" ForeColor="Red" 
                                                                        Operator="GreaterThan" Type="Date" ValidationGroup="DueDate"></asp:CompareValidator>
                                                                     <br />    
                                                                </td>
                                                            </tr>
                                                        </table>
            </div>
            </div>             
                                                          

                <br />
                <div align="center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                    <asp:GridView ID="GridView1" 
                        runat="server" 
                        AllowPaging="True" 
                    AutoGenerateColumns="False" 
                    CellPadding="4" 
                    GridLines="None"                     
                    CssClass="table table-striped table-bordered table-hover tableShadow" 

                    onpageindexchanging="GridView1_PageIndexChanging" 
                    onrowdatabound="GridView1_RowDataBound" 
                    onrowdeleting="GridView1_RowDeleting" 
                    onrowediting="GridView1_RowEditing" 
                    onrowupdating="GridView1_RowUpdating" 
                    PageSize="5" Visible="False" 
                        onselectedindexchanged="GridView1_SelectedIndexChanged" 
                        onselectedindexchanging="GridView1_SelectedIndexChanging" 
                        style="font-size: small" Height="30%" Width="90%" 
                        onrowcancelingedit="GridView1_RowCancelingEdit" AllowSorting="True" 
                            onsorting="GridView1_Sorting">       
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:RadioButton ID="reqr_radio" runat="server" AutoPostBack="true" 
                                        GroupName="rain" OnCheckedChanged="GridView1_RadioSelect" 
                                        OnClick="javascript:Selrdbtn(this.id)" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowEditButton="True" />
                            <asp:TemplateField HeaderText="Hidden">
                                <ItemTemplate>
                                    <asp:Label ID="Label_Hidden" runat="server" Text='<%# Eval("Hidden") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Requirement Name">
                                <EditItemTemplate>
                                    <asp:TextBox class="form-control" ID="TextBox_Reqr_Name_Edit" runat="server" 
                                        style="font-family: Andalus" Text='<%# Eval("Requirement Name") %>'></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label_Feature" runat="server" 
                                        Text='<%# Eval("Requirement Name") %>'></asp:Label>
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
                            <asp:TemplateField HeaderText="Specifications">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton_Show_Spec" runat="server" 
                                        onclientclick="javascript:unSelrdbtn()" 
                                        oncommand="LinkButton_Show_Spec_Command">Show!</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Submit Date" SortExpression="Submit Date Ticks">
                                <ItemTemplate>
                                    <asp:Label ID="Label_Submit_Date" runat="server" 
                                        Text='<%# Eval("Submit Date") %>'></asp:Label>
                                    <br />
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
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label_Due_Date" runat="server" Text='<%# Eval("Due Date") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle ForeColor="black" Font-Underline="True" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Location">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton_Location" runat="server" 
                                        onclientclick="javascript:unSelrdbtn()" 
                                        oncommand="LinkButton_Location_Command">Show!</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Min Quote">
                                <ItemTemplate>
                                    <asp:Label ID="Label_Min_Quote" runat="server" Text='<%# Eval("Min Quote") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tagged RFQs">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton_Tagged_RFQ" runat="server" 
                                        oncommand="LinkButton_Tagged_RFQ_Command" 
                                        onclientclick="javascript:unSelrdbtn()">Show!</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Created By">
                                <ItemTemplate>
                                    <asp:Label ID="Label_Created_By" runat="server" 
                                        Text='<%# Eval("Created By") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Active?">
                                <EditItemTemplate>
                                    <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Reqr_Active" runat="server" 
                                        style="font-family: Andalus">
                                    </asp:DropDownList>
                                    &nbsp;<asp:Label ID="Label_Active_Edit" runat="server" Text='<%# Eval("Active?") %>' 
                                        Visible="False"></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label_Active" runat="server" Text='<%# Eval("Active?") %>'></asp:Label>
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
                        <asp:Label ID="Label_Reqr_Grid_Access" runat="server" ForeColor="Red" 
                            style="font-size: small" Visible="False"></asp:Label>
                        <br />                        
                                    &nbsp;<asp:Button ID="Button_Req_Refresh_Hidden" runat="server" style="display:none" 
                            ForeColor="#336600" onclick="Button_Req_Refresh_Hidden_Click" Text="Hidden" />
                        <asp:Button ID="Button_Req_Refresh" runat="server" ForeColor="#336600" 
                            onclick="Button_Req_Refresh_Click" 
                            style="font-family: Andalus; font-size: x-small" Text="Refresh Data!" />
                        &nbsp;<asp:Button ID="Button_Create_Req" runat="server" class="btn btn-sm btn-success"
                            onclick="Button_Create_Req_Click" 
                            style="font-family: Andalus" Text="Create New Requirement!" />
                        &nbsp;<asp:Button ID="Button_Create_Clone" runat="server" Enabled="False"  class="btn btn-sm btn-success" 
                            onclick="Button_Create_Clone_Click" style="font-family: Andalus" 
                            Text="Clone This!" />
                        &nbsp;<asp:Button ID="Button_Convert_Req_To_RFQ" runat="server" class="btn btn-sm btn-success" 
                            onclick="Button_Convert_Req_To_RFQ_Click" style="font-family: Andalus" 
                            Text="Convert to RFQ!" Enabled="False" />
                        &nbsp;<asp:Button ID="Button_Notes" runat="server" Enabled="False" class="btn btn-sm btn-success" 
                            onclick="Button_Notes_Click" style="font-family: Andalus" Text="Notes!" />
                        &nbsp;<asp:Button ID="Button_Audit_Req" runat="server" Enabled="False" class="btn btn-sm btn-success" 
                            onclick="Button_Audit_Req_Click" style="font-family: Andalus" Text="Logs!" />
                        &nbsp;<asp:Label ID="Label_Reqr_Conversion_Stat" runat="server" 
                            style="font-size: small" Visible="False"></asp:Label>
                    </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                    &nbsp&nbsp;</div>
                
            </div>
        </asp:Panel>
        </div>
        </div>
        </asp:Panel>
                    <ajaxtoolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender_Pend_Appr" CollapsedText="Show Details..." ExpandedText="Hide Details" runat="server" Collapsed="true" AutoCollapse="false" TargetControlID="Panel_Reqr_Collp"
            ImageControlId="Req_Collapse_Img"  CollapsedImage="~/Images/ArrowTip_Down.png" ExpandedImage="~/Images/ArrowTip_Parallel.png"   ExpandControlID="LinkButton_Req_Collp"
             CollapseControlID="LinkButton_Req_Collp"/>     
                     </ContentTemplate>
        </asp:UpdatePanel>       
        <br />
           
    <asp:UpdatePanel ID="UpdatePanel_RFQ_Collp" runat="server">
    <ContentTemplate>
                                                <table width="100%">
                                                                  <tr>
                                                                <td background="../Images/menu_bg.gif">
                                                                <asp:LinkButton ID="LinkButton_Rfq_Collapse" runat="server" ForeColor="White" 
                                                                        ToolTip="Click to expand or collapse">All RFQ Details</asp:LinkButton>
                                                                        <div style="float: right; vertical-align: middle;">
                                                                    <asp:Image ID="Rfq_Collapse_Img" runat="server"/>
                                                                    </div>
                                                                    </td>
                                                            </tr>
                                                            </table>      
            <asp:Panel  ID="Panel_RFQ_Collp" runat="server">
            <br />
           <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title">All RFQs</h3>
            </div>
            <div class="panel-body">  
        <asp:Panel ID="Panel_All_RFQ" runat="server" Width="95%"
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
                                               <asp:TextBox class="form-control" ID="TextBox_RFQ_No_RFQ" runat="server" 
                                                   style="font-family: Andalus" Width="20%"></asp:TextBox>
                                               &nbsp;Category:
                                               <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Category_RFQ" runat="server" 
                                                   onselectedindexchanged="DropDownList_Category_SelectedIndexChanged" 
                                                   style="font-family: Andalus">
                                               </asp:DropDownList>
                                               &nbsp; Status:
                                               <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_RFQ_Active_Stat" runat="server" 
                                                   onselectedindexchanged="DropDownList_Status_SelectedIndexChanged" 
                                                   style="font-family: Andalus">
                                               </asp:DropDownList>
                                               <br /><br />
                                               From Date (Due) :
                                               <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_From_Date_RFQ" runat="server" 
                                                   ValidationGroup="RFQDueDate" Width="20%"></asp:TextBox>
                                               <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_From_Date_RFQ_CalendarExtender" 
                                                   runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton3" 
                                                   TargetControlID="TextBox_From_Date_RFQ">
                                               </ajaxtoolkit:CalendarExtender>
                                               &nbsp;&nbsp; To Date (Due) :&nbsp;
                                               <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_To_Date_RFQ" runat="server" 
                                                   ValidationGroup="RFQDueDate" Width="20%"></asp:TextBox>
                                               <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_To_Date_RFQ_CalendarExtender" 
                                                   runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton4" 
                                                   TargetControlID="TextBox_To_Date_RFQ">
                                               </ajaxtoolkit:CalendarExtender>
                                               &nbsp;&nbsp;<asp:Button ID="Button_Filter_All_RFQ" runat="server" 
                                                   class="btn btn-sm btn-success" onclick="Button_Filter_All_RFQ_Click" 
                                                   style="font-family: Andalus" Text="Filter" ValidationGroup="RFQDueDate" />
                                               &nbsp;<asp:CompareValidator ID="CompareValidator2" runat="server" 
                                                   ControlToCompare="TextBox_From_Date_RFQ" 
                                                   ControlToValidate="TextBox_To_Date_RFQ" Display="Dynamic" 
                                                   ErrorMessage="to date can not be earlier than from date" ForeColor="Red" 
                                                   Operator="GreaterThan" Type="Date" ValidationGroup="RFQDueDate"></asp:CompareValidator>
                                                <br />
                                           </td>
                                       </tr>
                                   </table>
                                    </div>
                                </div> 
                        </div>
                <br />
                <div align="center">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                    <asp:GridView ID="GridView_RFQ" 
                        runat="server" 
                        AllowPaging="True" 
                    AutoGenerateColumns="False" 
                    CellPadding="4" 
                    GridLines="None" 
                                                    CssClass="table table-striped table-bordered table-hover tableShadow" 
         
                    onpageindexchanging="GridView_RFQ_PageIndexChanging" 
                    onrowdatabound="GridView_RFQ_RowDataBound" 
                    PageSize="5" 
                    Visible="False" 
                        onselectedindexchanged="GridView_RFQ_SelectedIndexChanged" 
                        style="font-size: small" Height="30%" Width="95%" 
                             onrowcancelingedit="GridView_RFQ_RowCancelingEdit" 
                            onrowediting="GridView_RFQ_RowEditing" 
                            onrowupdating="GridView_RFQ_RowUpdating" AllowSorting="True" 
                            onsorting="GridView_RFQ_Sorting">
                          <AlternatingRowStyle CssClass="alt" />
                          <Columns>
                              <asp:TemplateField>
                                  <ItemTemplate>
                                      <asp:RadioButton ID="rfq_radio" runat="server" AutoPostBack="true" 
                                          GroupName="rain" OnCheckedChanged="GridView_RFQ_RadioSelect" 
                                          OnClick="javascript:Selrdbtn(this.id)" />
                                  </ItemTemplate>
                              </asp:TemplateField>
                              <asp:CommandField ShowEditButton="True" />
                              <asp:TemplateField HeaderText="RFQ#">
                                  <ItemTemplate>
                                      <asp:Label ID="Label_RFQId" runat="server" Text='<%# Eval("RFQNo") %>'></asp:Label>
                                  </ItemTemplate>
                              </asp:TemplateField>
                              <asp:TemplateField HeaderText="RFQ Name">
                                  <EditItemTemplate>
                                      <asp:TextBox class="form-control" ID="TextBox_RFQName_Edit" runat="server" 
                                          style="font-family: Andalus" Text='<%# Eval("RFQName") %>'></asp:TextBox>
                                  </EditItemTemplate>
                                  <ItemTemplate>
                                      <asp:Label ID="Label_RFQName" runat="server" Text='<%# Eval("RFQName") %>'></asp:Label>
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
                              <asp:TemplateField HeaderText="Specifications">
                                  <ItemTemplate>
                                      <asp:LinkButton ID="LinkButton_Show_Spec" runat="server" 
                                          onclientclick="javascript:unSelrdbtn()" 
                                          oncommand="LinkButton_Show_Spec_Command1">Show!</asp:LinkButton>
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
                                  </EditItemTemplate>
                                  <ItemTemplate>
                                      <asp:Label ID="Label_Due_Date" runat="server" Text='<%# Eval("Due Date") %>'></asp:Label>
                                  </ItemTemplate>
                                  <HeaderStyle ForeColor="black" Font-Underline="True" />
                              </asp:TemplateField>
                              <asp:TemplateField HeaderText="Location">
                                  <ItemTemplate>
                                      <asp:LinkButton ID="LinkButton_Location" runat="server" 
                                          
                                          onclientclick="javascript:unSelrdbtn()" 
                                          oncommand="LinkButton_Location_Command1">Show!</asp:LinkButton>
                                  </ItemTemplate>
                              </asp:TemplateField>
                              <asp:TemplateField HeaderText="Approval Status">
                                  <ItemTemplate>
                                      <asp:LinkButton ID="LinkButton_Approval_Stat" runat="server" 
                                          oncommand="LinkButton_Approval_Stat_Command" 
                                          Text='<%# Eval("ApprovalStat") %>' onclientclick="javascript:unSelrdbtn()"></asp:LinkButton>
                                  </ItemTemplate>
                              </asp:TemplateField>
                              <asp:TemplateField HeaderText="All Quotes">
                                  <ItemTemplate>
                                      <asp:LinkButton ID="LinkButton_All_Quotes" runat="server" 
                                          onclientclick="javascript:unSelrdbtn()" 
                                          oncommand="LinkButton_All_Quotes_Command">Show!</asp:LinkButton>
                                  </ItemTemplate>
                              </asp:TemplateField>
                              <asp:TemplateField HeaderText="All Shortlisted">
                                  <ItemTemplate>
                                      <asp:LinkButton ID="LinkButton_All_Shortlisted" runat="server" 
                                          onclientclick="javascript:unSelrdbtn()" 
                                          oncommand="LinkButton_All_Shortlisted_Command">Show!</asp:LinkButton>
                                  </ItemTemplate>
                              </asp:TemplateField>
                              <asp:TemplateField HeaderText="Purchase Order">
                                  <ItemTemplate>
                                      <asp:LinkButton ID="LinkButton_PO" runat="server" 
                                          CommandArgument="<%# Container.DataItemIndex %>"                                           
                                          oncommand="LinkButton_PO_Command" Text='<%# Eval("PO_No") %>' 
                                          onclientclick="javascript:unSelrdbtn()" ></asp:LinkButton>
                                  </ItemTemplate>
                              </asp:TemplateField>
                              <asp:TemplateField HeaderText="Broadcast To">
                                  <ItemTemplate>
                                      <asp:LinkButton ID="LinkButton_Broadcast_To" runat="server" 
                                          onclientclick="javascript:unSelrdbtn()" 
                                          oncommand="LinkButton_Broadcast_To_Command">Show!</asp:LinkButton>
                                  </ItemTemplate>
                              </asp:TemplateField>
                              <asp:TemplateField HeaderText="Active Status">
                                  <ItemTemplate>
                                      <asp:Label ID="Label_Active_Stat" runat="server" 
                                          Text='<%# Eval("ActiveStatus") %>'></asp:Label>
                                  </ItemTemplate>
                                  <EditItemTemplate>
                                      <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Rfq_Active" runat="server" 
                                          style="font-family: Andalus">
                                      </asp:DropDownList>
                                      &nbsp;<asp:Label ID="Label_Active_Edit" runat="server" 
                                          Text='<%# Eval("ActiveStatus") %>' Visible="False"></asp:Label>
                                  </EditItemTemplate>
                              </asp:TemplateField>
                              <asp:TemplateField HeaderText="Invoice">
                                  <ItemTemplate>
                                      <asp:LinkButton ID="LinkButton_Show_Inv" runat="server" 
                                          CommandArgument="<%#Container.DataItemIndex %>" 
                                          oncommand="LinkButton_Show_Inv_Command" Text='<%# Eval("Inv_No") %>' 
                                          onclientclick="javascript:unSelrdbtn()"></asp:LinkButton>
                                  </ItemTemplate>
                              </asp:TemplateField>
                              <asp:TemplateField HeaderText="T&amp;C">
                                  <ItemTemplate>
                                      <asp:LinkButton ID="LinkButton1" runat="server" 
                                          onclientclick="javascript:unSelrdbtn()" oncommand="LinkButton1_Command">Show!</asp:LinkButton>
                                  </ItemTemplate>
                              </asp:TemplateField>
                              <asp:TemplateField HeaderText="All Documents" Visible="False">
                                  <ItemTemplate>
                                      <asp:Label ID="Label_Hidden_Doc_Name" runat="server" 
                                          Text='<%# Eval("Hidden_Doc_Name") %>' Visible="False"></asp:Label>
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
                                        <asp:Label ID="Label_RFQ_Grid_Access" runat="server" 
                            ForeColor="Red" style="font-size: small" Visible="False"></asp:Label>
                                        <br />
                                        <asp:Button ID="Button_Rfq_Refresh_Hidden" runat="server" style="display:none"
                            ForeColor="#336600" onclick="Button_Rfq_Refresh_Hidden_Click" 
                            Text="Hidden" />
                                                                    <asp:Button ID="Button_Rfq_Refresh_Hidden_Index_Unchanged" runat="server" style="display:none"
                            ForeColor="#336600" onclick="Button_Rfq_Refresh_Hidden_Click_Index_Unchanged" onclientclick="javascript:unSelrdbtn()" 
                            Text="Hidden" />
                        <asp:Button ID="Button_Rfq_Refresh" runat="server" ForeColor="#336600" 
                            onclick="Button_Rfq_Refresh_Click" 
                            style="font-family: Andalus; font-size: x-small" Text="Refresh Data!" />
                        &nbsp;<asp:Button ID="Button_Create_Req0" runat="server" class="btn btn-sm btn-success" 
                            onclientclick="window.open('CreateRFQ.aspx','CreateRfq','width=1000,height=1000,left=100,right=500,scrollbars=1',true);" 
                            style="font-family: Andalus" Text="Create New RFQ!" />
                                        &nbsp;<asp:Button ID="Button_Create_Clone_RFQ" runat="server" class="btn btn-sm btn-success" 
                            Enabled="False" onclick="Button_Create_Clone_RFQ_Click" 
                            style="font-family: Andalus" Text="Clone This!" />
                        &nbsp;<asp:Button ID="Button_Tag_To_Req" runat="server" class="btn btn-sm btn-success"
                            Enabled="False" onclick="Button_Tag_To_Req_Click" style="font-family: Andalus" 
                            Text="Tag to Requirement!" 
                            ToolTip="Tagging will help you see the progress of a requirement from requirement section." />
                        &nbsp;<asp:Button ID="Button_Workflow_Tree" runat="server" Enabled="False" class="btn btn-sm btn-success"
                            onclick="Button_Workflow_Tree_Click" style="font-family: Andalus" 
                            Text="Approval History" />
                        &nbsp;<asp:Button ID="Button_RFQ_Doc" runat="server" Enabled="False" class="btn btn-sm btn-success"
                            onclick="Button_RFQ_Doc_Click" style="font-family: Andalus" 
                            Text="Documents" />
                        &nbsp;<asp:Button ID="Button_Notes_RFQ" runat="server" Enabled="False" class="btn btn-sm btn-success"
                            onclick="Button_Notes_RFQ_Click" style="font-family: Andalus" Text="Notes!" />
                        &nbsp;<asp:Button ID="Button_Audit_RFQ" runat="server" Enabled="False" class="btn btn-sm btn-success"
                            onclick="Button_Audit_RFQ_Click" style="font-family: Andalus" Text="Logs!" />
                        &nbsp;<asp:Label ID="Label_RFQ_Tag" runat="server" ForeColor="Green" 
                            style="font-size: small" Visible="False"></asp:Label>
                                        </ContentTemplate>
                    </asp:UpdatePanel>
                    
                    <br />
                    &nbsp;</div>
        </asp:Panel>
               </div>
        </div>
        </asp:Panel>
                            <ajaxtoolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender_RFQ" CollapsedText="Show Details..." ExpandedText="Hide Details" runat="server" Collapsed="true" AutoCollapse="false" TargetControlID="Panel_RFQ_Collp"
            ImageControlId="Rfq_Collapse_Img"  CollapsedImage="~/Images/ArrowTip_Down.png" ExpandedImage="~/Images/ArrowTip_Parallel.png"   ExpandControlID="LinkButton_Rfq_Collapse"
             CollapseControlID="LinkButton_Rfq_Collapse"/>    
     </ContentTemplate>
    </asp:UpdatePanel>     
        <br />
    <asp:UpdatePanel ID="UpdatePanel_Inv_Collp" runat="server">
    <ContentTemplate>
                                                        <table width="100%">
                                                                  <tr>
                                                                <td background="../Images/menu_bg.gif">
                                                                <asp:LinkButton ID="LinkButton_Inv_Collp" runat="server" ForeColor="White" 
                                                                        ToolTip="Click to expand or collapse">All Invoices Sent to Us</asp:LinkButton>
                                                                        <div style="float: right; vertical-align: middle;">
                                                                    <asp:Image ID="Inv_Collapse_Img" runat="server"/>
                                                                    </div>
                                                                    </td>
                                                            </tr>
                                                            </table>   
            <asp:Panel  ID="Panel_Inv_Collp" runat="server">
            <br />   
            <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title">All Invoices Sent to Us </h3> </div>
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
                                <td align="center">
                                    RFQ#:
                                    <asp:TextBox class="form-control" ID="TextBox_RFQ_No" runat="server" style="font-family: Andalus" 
                                        Width="20%"></asp:TextBox>
                                    &nbsp;Invoice No:
                                    <asp:TextBox class="form-control" ID="TextBox_Inv_No" runat="server" style="font-family: Andalus" 
                                        Width="20%"></asp:TextBox>
                                    &nbsp;<asp:LinkButton ID="LinkButton_Tran_No" runat="server" 
                                        ToolTip="Any invoice associated with this transaction no will be displayed. The filter will look for exact match of the transaction no">Transaction No:</asp:LinkButton>
                                    &nbsp;<asp:TextBox class="form-control" ID="TextBox_Tran_No" runat="server" style="font-family: Andalus" 
                                        Width="20%"></asp:TextBox>
                                        <br />
                                        <br />
                                    Category:
                                    <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Prod_Cat" runat="server" 
                                        onselectedindexchanged="DropDownList_Category_SelectedIndexChanged" 
                                        style="font-family: Andalus">
                                    </asp:DropDownList>
                                    &nbsp; Delivery Status:
                                    <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Deliv_Stat" runat="server" 
                                        onselectedindexchanged="DropDownList_Status_SelectedIndexChanged" 
                                        style="font-family: Andalus">
                                    </asp:DropDownList>
                                    &nbsp;Payment Status:
                                    <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Pmnt_Stat" runat="server" 
                                        onselectedindexchanged="DropDownList_Status_SelectedIndexChanged" 
                                        style="font-family: Andalus">
                                    </asp:DropDownList>
                                    &nbsp;Supplier:
                                    <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Contact_Inv" runat="server" 
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
                                    &nbsp;&nbsp;<asp:Button ID="Button_Filter_All_Inv" runat="server" class="btn btn-sm btn-success"
                                        onclick="Button_Filter_All_Inv_Click" style="font-family: Andalus" 
                                        Text="Filter" ValidationGroup="DueDate" />
                                    &nbsp;<asp:CompareValidator ID="CompareValidator3" runat="server" 
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
                                CssClass="table table-striped table-bordered table-hover tableShadow"                                            
                                GridLines="None" 
                                Height="30%" 
                                onpageindexchanging="GridView_Invoice_PageIndexChanging" 
                                onrowdatabound="GridView_Invoice_RowDataBound" 
                                PageSize="5" 
                                style="font-size: small" Visible="False" Width="90%" 
                                onselectedindexchanged="GridView_Invoice_SelectedIndexChanged" 
                                onrowediting="GridView_Invoice_RowEditing" AllowSorting="True" 
                                onsorting="GridView_Invoice_Sorting">                                
                                <AlternatingRowStyle CssClass="alt" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:RadioButton ID="inv_radio" runat="server" AutoPostBack="true" 
                                                GroupName="rain" OnCheckedChanged="GridView_Inv_RadioSelect" 
                                                OnClick="javascript:Selrdbtn(this.id)" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="RFQ#">
                                        <ItemTemplate>
                                            <asp:Label ID="Label_RFQId1" runat="server" Text='<%# Eval("RFQNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice#">
                                        <ItemTemplate>
                                            <asp:Label ID="Label_Invoice_Id1" runat="server" Text='<%# Eval("Inv_No") %>'></asp:Label>
                                            <br />
                                            <asp:Label ID="Label_Related_PO" runat="server" 
                                                Text='<%# Eval("Related_PO") %>' Visible="False"></asp:Label>
                                            <br />
                                            <asp:Label ID="Label_Invoice_Id_Val" runat="server" 
                                                Text='<%# Eval("Inv_Id") %>' Visible="False"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Currency">
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
                                                onselectedindexchanged="DropDownList_Category_SelectedIndexChanged" 
                                                style="font-family: Andalus">
                                            </asp:DropDownList>
                                            &nbsp;<asp:Label ID="Label_Deliv_Stat_Edit" runat="server" 
                                                Text='<%# Eval("Deliv_Stat") %>'></asp:Label>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Payment Status">
                                        <ItemTemplate>
                                            <asp:Label ID="Label_Pmnt_Stat1" runat="server" Text='<%# Eval("Pmnt_Stat") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Pmnt_Edit" runat="server" 
                                                onselectedindexchanged="DropDownList_Category_SelectedIndexChanged" 
                                                style="font-family: Andalus">
                                            </asp:DropDownList>
                                            &nbsp;<asp:Label ID="Label_Pmnt_Stat_Edit" runat="server" 
                                                Text='<%# Eval("Pmnt_Stat") %>'></asp:Label>
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
                                </Columns>
                                                        <RowStyle CssClass="cursor-pointer" />
                        <AlternatingRowStyle CssClass="active" />
                        <SelectedRowStyle CssClass="danger" />
                        <HeaderStyle CssClass="success" />
                        <PagerStyle CssClass="pagination-lg" />
                        <FooterStyle CssClass="success"/>
                        <EditRowStyle CssClass="info" />
                            </asp:GridView>
                            <asp:Label ID="Label_Inv_Grid_Access" runat="server" ForeColor="Red" 
                                style="font-size: small" Visible="False"></asp:Label>
                            <br />
                            <asp:Button ID="Button_Inv_Refresh" runat="server" ForeColor="#336600" 
                                onclick="Button_Inv_Refresh_Click" 
                                style="font-family: Andalus; font-size: x-small" Text="Refresh Data!" />
                            &nbsp;<asp:Button ID="Button_Notes_Inv" runat="server" Enabled="False" class="btn btn-sm btn-success"
                                onclick="Button_Notes_Inv_Click" style="font-family: Andalus" Text="Notes!" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                    </div>
                
            </div>
        </asp:Panel>
                      </div>
                      </div>
             </asp:Panel>
                                         <ajaxtoolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender_Inv" CollapsedText="Show Details..." ExpandedText="Hide Details" runat="server" Collapsed="true" AutoCollapse="false" TargetControlID="Panel_Inv_Collp"
            ImageControlId="Inv_Collapse_Img"  CollapsedImage="~/Images/ArrowTip_Down.png" ExpandedImage="~/Images/ArrowTip_Parallel.png"   ExpandControlID="LinkButton_Inv_Collp"
             CollapseControlID="LinkButton_Inv_Collp"/>    
                 </ContentTemplate>
    </asp:UpdatePanel>
<br />
    <asp:UpdatePanel ID="UpdatePanel_Po_Collp" runat="server">
    <ContentTemplate>

                                                        <table width="100%">
                                                                  <tr>
                                                                <td background="../Images/menu_bg.gif">
                                                                <asp:LinkButton ID="LinkButton_Po_Collp" runat="server" ForeColor="White" 
                                                                        ToolTip="Click to expand or collapse">All Purchase Orders</asp:LinkButton>
                                                                        <div style="float: right; vertical-align: middle;">
                                                                    <asp:Image ID="Po_Collapse_Img" runat="server"/>
                                                                    </div>
                                                                    </td>
                                                            </tr>
                                                            </table>   
            <asp:Panel  ID="Panel_PO_Collp" runat="server">
            <br />
             <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title">All Purchase Orders </h3> </div>
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
                                    &nbsp;PO No:
                                    <asp:TextBox class="form-control" ID="TextBox_po" runat="server" style="font-family: Andalus" 
                                        Width="20%"></asp:TextBox>
                                    &nbsp;Supplier:&nbsp;<asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Vendor_po" runat="server" 
                                        style="font-family: Andalus">
                                    </asp:DropDownList>
                                    <br />
                                    <br />
                                    PO Date (From) :
                                    <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_From_Date_PO" runat="server" 
                                        ValidationGroup="DueDatePO" Width="20%"></asp:TextBox>
                                    <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_From_Date_CalendarExtender_PO_From_Date" 
                                        runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton_PO_From_Date" 
                                        TargetControlID="TextBox_From_Date_PO">
                                    </ajaxtoolkit:CalendarExtender>
                                    &nbsp;&nbsp; PO Date (To) :&nbsp;
                                    <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_To_Date_PO" runat="server" 
                                        ValidationGroup="DueDatePO" Width="20%"></asp:TextBox>
                                    <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_To_Date_CalendarExtender_PO_To_Date" 
                                        runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton_To_Date_PO" 
                                        TargetControlID="TextBox_To_Date_PO">
                                    </ajaxtoolkit:CalendarExtender>
                                    &nbsp;&nbsp;<asp:Button ID="Button_Filter_PO" runat="server" class="btn btn-sm btn-success"
                                        onclick="Button_Filter_PO_Click" style="font-family: Andalus" 
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
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="GridView_PO" 
                            runat="server" AllowPaging="True" 
                                AutoGenerateColumns="False" 
                                CellPadding="4" 
                                CssClass="table table-striped table-bordered table-hover tableShadow"                                            
                                GridLines="None" 
                                Height="30%" 
                                onpageindexchanging="GridView_PO_PageIndexChanging" 
                                onrowdatabound="GridView_PO_RowDataBound" 
                                PageSize="5" 
                                style="font-size: small" Visible="False" Width="70%" 
                                onselectedindexchanged="GridView_PO_SelectedIndexChanged" 
                                onrowediting="GridView_PO_RowEditing" AllowSorting="True" 
                                onsorting="GridView_PO_Sorting" onrowupdating="GridView_PO_RowUpdating" 
                                onrowcancelingedit="GridView_PO_RowCancelingEdit">                                
                                <AlternatingRowStyle CssClass="alt" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:RadioButton ID="po_radio" runat="server" AutoPostBack="true" 
                                                GroupName="rain" OnCheckedChanged="GridView_PO_RadioSelect" 
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
                                                oncommand="LinkButton_PO_Purchase_Order_Command" Text='<%# Eval("PO_No") %>'></asp:LinkButton>
                                            <br />
                                            <br />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="PO Date" ShowHeader="False" 
                                        SortExpression="PO_Date_Ticks">
                                        <ItemTemplate>
                                            <asp:Label ID="Label_PO_Date" runat="server" Text='<%# Eval("PO_Date") %>'></asp:Label>
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
                            ForeColor="#336600" onclick="Button_PO_Refresh_Hidden_Click" onclientclick="javascript:unSelrdbtn()" 
                            Text="Hidden" />
                            <asp:Button ID="Button_PO_Refresh" runat="server" ForeColor="#336600" 
                                onclick="Button_PO_Refresh_Click" 
                                style="font-family: Andalus; font-size: x-small" Text="Refresh Data!" />
                            &nbsp;<asp:Button ID="Button_Create_PO" runat="server" 
                                class="btn btn-sm btn-success" onclick="Button_Create_PO_Click" 
                                style="font-family: Andalus" Text="Create New PO!" />
                            &nbsp;<asp:Button ID="Button_Notes_PO" runat="server" Enabled="False" class="btn btn-sm btn-success"
                                onclick="Button_Notes_PO_Click" style="font-family: Andalus" 
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
                                                      <ajaxtoolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender_PO" CollapsedText="Show Details..." ExpandedText="Hide Details" runat="server" Collapsed="true" AutoCollapse="false" TargetControlID="Panel_PO_Collp"
            ImageControlId="Po_Collapse_Img"  CollapsedImage="~/Images/ArrowTip_Down.png" ExpandedImage="~/Images/ArrowTip_Parallel.png"   ExpandControlID="LinkButton_Po_Collp"
             CollapseControlID="LinkButton_Po_Collp"/>    
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
