<%@ Page Title="" Language="C#" Theme="ThemeBlue" MasterPageFile="~/Pages/Site1.Master" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="OnLine.Pages.HomePage" %>
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
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderBody" runat="server">
    <p>
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
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>

                                                            <table width="100%">
                                                                  <tr>
                                                                <td background="../Images/menu_bg.gif">
                                                                <asp:LinkButton ID="LinkButton_Pend_Appr" runat="server" ForeColor="White" 
                                                                        ToolTip="Click to expand or collapse">Items Pending My Approval</asp:LinkButton>
                                                                        <div style="float: right; vertical-align: middle;">
                                                                    <asp:Image ID="Pend_Appr_Collapse_Img" runat="server"/>
                                                                    </div>
                                                                    </td>
                                                            </tr>
                                                            </table>            
                                                                                                          
         <asp:Panel ID="Panel_Pend_Appr" runat="server"> 
         <br />
            <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title">RFQs</h3>
            </div>
            <div class="panel-body">                                                          
        <asp:Panel ID="Panel_All_RFQ" runat="server" 
            style="font-family: Andalus">
           
                        <div align="center">
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
                                               <asp:TextBox  class="form-control" ID="TextBox_RFQ_No_RFQ" runat="server" 
                                                   style="font-family: Andalus" Width="20%"></asp:TextBox>
                                               &nbsp;&nbsp;&nbsp;<asp:Button ID="Button_Filter_All_RFQ" runat="server" class="btn btn-sm btn-success"
                                                   onclick="Button_Filter_All_RFQ_Click" style="font-family: Andalus" 
                                                   Text="Filter" ValidationGroup="RFQDueDate" />
                                               </td>
                                       </tr>
                                   </table>
           </div>
            </div>       
                        </div>
                <br />
                <div align="center">
                    <asp:UpdatePanel ID="UpdatePanel_RFQ" runat="server">
                    <ContentTemplate>
                    <asp:GridView ID="GridView_RFQ" 
                        runat="server" 
                        AllowPaging="True" 
                    AutoGenerateColumns="False" 
                    CellPadding="4" 
                    GridLines="None" 
                                                    CssClass="table table-striped table-bordered table-hover tableShadow"          
                    onpageindexchanging="GridView_RFQ_PageIndexChanging" 
                    PageSize="5" 
                    Visible="False" 
                        onselectedindexchanged="GridView_RFQ_SelectedIndexChanged" 
                        style="font-size: small" Height="30%" Width="95%" AllowSorting="True" 
                            onsorting="GridView_RFQ_Sorting">

                          <Columns>
                              <asp:TemplateField>
                                  <ItemTemplate>
                                      <asp:RadioButton ID="rfq_radio" runat="server" AutoPostBack="true" 
                                          GroupName="rain" OnCheckedChanged="GridView_RFQ_RadioSelect" 
                                          OnClick="javascript:Selrdbtn(this.id)" />
                                  </ItemTemplate>
                              </asp:TemplateField>
                              <asp:TemplateField HeaderText="RFQ#">
                                  <ItemTemplate>
                                      <asp:Label ID="Label_RFQId" runat="server" Text='<%# Eval("RFQNo") %>'></asp:Label>
                                  </ItemTemplate>
                              </asp:TemplateField>
                              <asp:TemplateField HeaderText="RFQ Name">
                                  <EditItemTemplate>
                                      <asp:TextBox  class="form-control" ID="TextBox_RFQName_Edit" runat="server" 
                                          style="font-family: Andalus" Text='<%# Eval("RFQName") %>'></asp:TextBox>
                                  </EditItemTemplate>
                                  <ItemTemplate>
                                      <asp:Label ID="Label_RFQName" runat="server" Text='<%# Eval("RFQName") %>'></asp:Label>
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
                                  </ItemTemplate>
                                  <HeaderStyle ForeColor="black" Font-Underline="True" />
                              </asp:TemplateField>
                              <asp:TemplateField HeaderText="Due Date" SortExpression="Due Date Ticks">
                                  <EditItemTemplate>
                                      <asp:TextBox  class="form-control" ID="TextBox_DueDate" runat="server"  
                                          style="font-family: Andalus" Text='<%# Eval("Due Date") %>' Width="169px"></asp:TextBox>
                                      <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_DueDate_CalendarExtender" 
                                          runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton_DueDate_Edit" 
                                          TargetControlID="TextBox_DueDate"></ajaxtoolkit:CalendarExtender>
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
                              <asp:TemplateField HeaderText="T&amp;C">
                                  <ItemTemplate>
                                      <asp:LinkButton ID="LinkButton1" runat="server" 
                                          onclientclick="javascript:unSelrdbtn()" oncommand="LinkButton1_Command">Show!</asp:LinkButton>
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
                        <asp:Button ID="Button_Rfq_Refresh" runat="server" ForeColor="#336600" 
                            onclick="Button_Rfq_Refresh_Click" 
                            style="font-family: Andalus; font-size: x-small" Text="Refresh Data!" />
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="Button_Workflow_Tree" 
                            runat="server" Enabled="False" 
                            onclick="Button_Workflow_Tree_Click" style="font-family: Andalus" class="btn btn-sm btn-success"
                            Text="Approval History" />
                        &nbsp;<asp:Button ID="Button_Notes_RFQ" runat="server" Enabled="False" class="btn btn-sm btn-success"
                            onclick="Button_Notes_RFQ_Click" style="font-family: Andalus" Text="Notes!" />
                        &nbsp;<asp:Button ID="Button_Audit_RFQ" runat="server" Enabled="False" class="btn btn-sm btn-success"
                            onclick="Button_Audit_RFQ_Click" style="font-family: Andalus" Text="Logs!" />
                        &nbsp;
                                        </ContentTemplate>
                    </asp:UpdatePanel>
                    </div>
                
            </div>
        </asp:Panel>       
                </div>
        </div>
                                                               
        <br />

            <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title">Invoices</h3>
            </div>
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
                                    <asp:TextBox  class="form-control" ID="TextBox_RFQ_No" runat="server" style="font-family: Andalus" 
                                        Width="20%"></asp:TextBox>
                                    &nbsp;Invoice No:
                                    <asp:TextBox  class="form-control" ID="TextBox_Inv_No" runat="server" style="font-family: Andalus" 
                                        Width="20%"></asp:TextBox>
                                    &nbsp;Customer:&nbsp;<asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Contact_Inv" runat="server" 
                                        style="font-family: Andalus">
                                    </asp:DropDownList>
                                    &nbsp;<asp:Button ID="Button_Filter_All_Inv" runat="server" 
                                        onclick="Button_Filter_All_Inv_Click" 
                                        style="font-family: Andalus;" Text="Filter" class="btn btn-sm btn-success"
                                        ValidationGroup="DueDate" />
                                    </td>
                            </tr>
                        </table>
            </div>
            </div> 
                <br />

                    <asp:UpdatePanel ID="UpdatePanel_Inv" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="GridView_Invoice" 
                            runat="server" AllowPaging="True" 
                                AutoGenerateColumns="False" 
                                CellPadding="4" 
                                GridLines="None" 
                                CssClass="table table-striped table-bordered table-hover tableShadow" 
                                Height="30%" 
                                onpageindexchanging="GridView_Invoice_PageIndexChanging" PageSize="5" 
                                style="font-size: small" Visible="False" Width="90%" 
                                onselectedindexchanged="GridView_Invoice_SelectedIndexChanged" AllowSorting="True" 
                                onsorting="GridView_Invoice_Sorting">
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
                                            <asp:Label ID="Label_RFQId" runat="server" Text='<%# Eval("RFQNo") %>'></asp:Label>
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
                                </Columns>
                                                        <RowStyle CssClass="cursor-pointer" />
                        <AlternatingRowStyle CssClass="active" />
                        <SelectedRowStyle CssClass="danger" />
                        <HeaderStyle CssClass="success" />
                        <PagerStyle CssClass="pagination-lg" />
                        <FooterStyle CssClass="success"/>
                        <EditRowStyle CssClass="info" />
                            </asp:GridView>
                            <br />
                            <asp:Button ID="Button_Inv_Refresh" runat="server" ForeColor="#336600" 
                                onclick="Button_Inv_Refresh_Click" 
                                style="font-family: Andalus; font-size: x-small" Text="Refresh Data!" />
                            &nbsp;&nbsp;<asp:Button ID="Button_Workflow_Tree_Inv" runat="server" class="btn btn-sm btn-success"
                                Enabled="False" onclick="Button_Workflow_Tree_Inv_Click" 
                                style="font-family: Andalus" Text="Approval History" />
                            &nbsp;<asp:Button ID="Button_Notes_Inv" runat="server" Enabled="False" class="btn btn-sm btn-success"
                                onclick="Button_Notes_Inv_Click" style="font-family: Andalus" Text="Notes!" />
                            &nbsp;<asp:Button ID="Button_Audit_Inv" runat="server" Enabled="False" class="btn btn-sm btn-success"
                                onclick="Button_Audit_Inv_Click" style="font-family: Andalus" Text="Logs!" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
             
            </div>
        </asp:Panel>
        </div>
        </div>

        </asp:Panel>
            <ajaxtoolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender_Pend_Appr" CollapsedText="Show Details..." ExpandedText="Hide Details" runat="server" Collapsed="true" AutoCollapse="false" TargetControlID="Panel_Pend_Appr"
            ImageControlId="Pend_Appr_Collapse_Img"  CollapsedImage="~/Images/ArrowTip_Down.png" ExpandedImage="~/Images/ArrowTip_Parallel.png"   ExpandControlID="LinkButton_Pend_Appr"
             CollapseControlID="LinkButton_Pend_Appr"/>       
                     </ContentTemplate>
        </asp:UpdatePanel>
        <br/>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
                                                                    <table width="100%">
                                                                  <tr>
                                                                <td background="../Images/menu_bg.gif" >
                                                                                                                                <asp:LinkButton ID="LinkButton_Lead_Appr" runat="server" ForeColor="White" 
                                                                        ToolTip="Click to expand or collapse">Active Leads Assigned to Me</asp:LinkButton>
                                                                        <div style="float: right; vertical-align: middle;">
                                                                        <asp:Image ID="Lead_Img" runat="server"/>
                                                                        </div>
                                                                    </td>
                                                            </tr>
                                                            </table>

        <asp:Panel ID="Panel_All_Leads" runat="server" 
            style="font-family: Andalus">
            <br />
            <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title">Leads</h3>
            </div>
            <div class="panel-body">     
            <br />
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
                                    <asp:TextBox  class="form-control" ID="TextBox_RFQ_No_Lead" runat="server" 
                                        style="font-family: Andalus" Width="20%"></asp:TextBox>
                                    &nbsp;Customer:
                                    <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Contact_Lead" runat="server" 
                                        style="font-family: Andalus">
                                    </asp:DropDownList>
                                    &nbsp;<asp:Button ID="Button_Filter_All_RFQ0" runat="server" class="btn btn-sm btn-success"
                                        onclick="Button_Filter_All_Lead_Click" style="font-family: Andalus" 
                                        Text="Filter" ValidationGroup="RFQDueDate" />
                                    &nbsp;</td>
                            </tr>
                        </table>
                        </div>
                        </div>
                        <br />
                <div align="center">
                    <asp:UpdatePanel ID="UpdatePanel_Lead" runat="server">
                    <ContentTemplate>
                        <asp:Image ID="Image_Unanswered_Lead" runat="server" Height="16px" 
                            ImageUrl="~/Images/attention.jpg" Visible="False" />
                        <asp:Label ID="Label_Unanswered_Leads" runat="server" Text="Unanswered Leads" 
                            Visible="False"></asp:Label>
                    <asp:GridView ID="GridView_Lead" 
                        runat="server" 
                        AllowPaging="True" 
                    AutoGenerateColumns="False" 
                    CellPadding="4" 
                    CssClass="table table-striped table-bordered table-hover tableShadow" 
                    GridLines="None" 
                    onpageindexchanging="GridView_Lead_PageIndexChanging" 
                    PageSize="5" Visible="False" 
                        onselectedindexchanged="GridView_Lead_SelectedIndexChanged" 
                        style="font-size: small" Height="30%" Width="90%" AllowSorting="True" 
                            onsorting="GridView_Lead_Sorting">                    
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:RadioButton ID="lead_radio" runat="server" AutoPostBack="true" 
                                        GroupName="rain" OnCheckedChanged="GridView_Lead_RadioSelect" 
                                        OnClick="javascript:Selrdbtn(this.id)" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Lead#">
                                <ItemTemplate>
                                    <asp:Label ID="Label_RFQId" runat="server" Text='<%# Eval("RFQNo") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Lead Name">
                                <EditItemTemplate>
                                    <asp:TextBox  class="form-control" ID="TextBox_RFQName" runat="server" style="font-family: Andalus" 
                                        Text='<%# Eval("RFQName") %>'></asp:TextBox>
                                    <asp:Label ID="Label_RFQName_Edit" runat="server" Text='<%# Eval("RFQName") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label_RFQName0" runat="server" Text='<%# Eval("RFQName") %>'></asp:Label>
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
                                        onclientclick="javascript:unSelrdbtn()" 
                                        Text='<%# Eval("CustName") %>' 
                                        oncommand="LinkButton_Customer_Command_Lead"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Submit Date" SortExpression="Submit Date Ticks">
                                <ItemTemplate>
                                    <asp:Label ID="Label_Submit_Date0" runat="server" 
                                        Text='<%# Eval("Submit Date") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle ForeColor="black" Font-Underline="True" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Due Date" SortExpression="Due Date Ticks">
                                <EditItemTemplate>
                                    <asp:TextBox  class="form-control" ID="TextBox_DueDate0" runat="server"  
                                        style="font-family: Andalus" Text='<%# Eval("Due Date") %>' Width="169px"></asp:TextBox>
                                    <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_DueDate_CalendarExtender0" 
                                        runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton_DueDate_Edit" 
                                        TargetControlID="TextBox_DueDate"></ajaxtoolkit:CalendarExtender>
                                    <asp:Label ID="Label_Due_Date_Edit" runat="server" 
                                        Text='<%# Eval("Due Date") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label_Due_Date0" runat="server" Text='<%# Eval("Due Date") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle ForeColor="black" Font-Underline="True" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Followup Date" SortExpression="Next Date Ticks">
                                <ItemTemplate>
                                    <asp:Label ID="Label_Fwp_Date" runat="server" Text='<%# Eval("Next Date") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox  class="form-control" ID="TextBox_FwpDate" runat="server"  
                                        style="font-family: Andalus" Text='<%# Eval("Next Date") %>' Width="169px"></asp:TextBox>
                                    <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_FwpDate_CalendarExtender" 
                                        runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton_FwpDate_Edit" 
                                        TargetControlID="TextBox_FwpDate"></ajaxtoolkit:CalendarExtender>
                                    <asp:Label ID="Label_Fwp_Date_Edit" runat="server" 
                                        Text='<%# Eval("Next Date") %>' Visible="False"></asp:Label>
                                </EditItemTemplate>
                                <HeaderStyle ForeColor="black" Font-Underline="True" />
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
                        <br />
                        <asp:Button ID="Button_Lead_Refresh" runat="server" ForeColor="#336600" 
                            onclick="Button_Lead_Refresh_Click" 
                            style="font-family: Andalus; font-size: x-small" Text="Refresh Data!" />
                        &nbsp;&nbsp;&nbsp;<asp:Button ID="Button_Notes_Lead" runat="server" Enabled="False" class="btn btn-sm btn-success"
                            onclick="Button_Notes_Lead_Click" style="font-family: Andalus" Text="Notes!" />
                        &nbsp;<asp:Button ID="Button_Audit_Lead" runat="server" Enabled="False" class="btn btn-sm btn-success"
                            onclick="Button_Audit_Lead_Click" style="font-family: Andalus" Text="Logs!" />
                        &nbsp;
                    </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                    </div>                
            </div>
        </div>
        </div>
        </asp:Panel>    

                    <ajaxtoolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender_Lead" runat="server" Collapsed="true" AutoCollapse="false" TargetControlID="Panel_All_Leads"
                        ImageControlId="Lead_Img"  CollapsedImage="~/Images/ArrowTip_Down.png" ExpandedImage="~/Images/ArrowTip_Parallel.png"  ExpandControlID="LinkButton_Lead_Appr"
             CollapseControlID="LinkButton_Lead_Appr"/>
                     </ContentTemplate>
        </asp:UpdatePanel>
             <br />
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
        <ContentTemplate>

                                                                                 <table width="100%">
                                                                  <tr>
                                                                <td background="../Images/menu_bg.gif" >
                                                                  <asp:LinkButton ID="LinkButton_Potn" runat="server" ForeColor="White" 
                                                                        ToolTip="Click to expand or collapse">Active Potentials Assigned to Me</asp:LinkButton>
                                                                        <div style="float: right; vertical-align: middle;">
                                                                        <asp:Image ID="Potn_Image" runat="server" ImageAlign="AbsMiddle" />
                                                                        </div>
                                                                    </td>
                                                            </tr>
                                                            </table>
        <asp:Panel ID="Panel_All_Potential" runat="server" 
            style="font-family: Andalus" >    
            <br />
            <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title">Potentials</h3>
            </div>
            <div class="panel-body">     
            <br />
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
                                    <asp:TextBox  class="form-control" ID="TextBox_RFQ_No_Potn" runat="server" 
                                        style="font-family: Andalus" Width="20%"></asp:TextBox>
                                    &nbsp; Potential Stage:
                                    <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Pot_Stage_Stat" runat="server" 
                                        style="font-family: Andalus">
                                    </asp:DropDownList>
                                    &nbsp; Customer:
                                    <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Contact_Potn" runat="server" 
                                        style="font-family: Andalus">
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;<asp:Button ID="Button_Filter_All_Pot" runat="server" class="btn btn-sm btn-success"
                                        onclick="Button_Filter_All_Pot_Click" style="font-family: Andalus" 
                                        Text="Filter" ValidationGroup="RFQDueDate" />
                                    &nbsp;&nbsp;&nbsp;</td>
                            </tr>
                        </table>
                        </div>
                        </div>                    
                <br />
         <div align="center">
                    <asp:UpdatePanel ID="UpdatePanel_Potn" runat="server">
                    <ContentTemplate>
                        <br />
                        <asp:Image ID="Image_Unanswered_Potn" runat="server" Height="16px" 
                            ImageUrl="~/Images/attention.jpg" Visible="False" />
                        <asp:Label ID="Label_Unanswered_Potn" runat="server" Visible="False"></asp:Label>
                    <asp:GridView ID="GridView_Potential" 
                        runat="server" 
                        AllowPaging="True" 
                    AutoGenerateColumns="False" 
                    CellPadding="4" 
                    ForeColor="#333333" 
                    GridLines="None" 
                                        CssClass="table table-striped table-bordered table-hover tableShadow" 
                    onpageindexchanging="GridView_Potential_PageIndexChanging" 
                    PageSize="5" Visible="False" 
                        onselectedindexchanged="GridView_Potential_SelectedIndexChanged" 
                        style="font-size: small" Height="30%" Width="100%" 
                        Font-Size="Medium" AllowSorting="True" 
                            onsorting="GridView_Potential_Sorting">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:RadioButton ID="potn_radio" runat="server" AutoPostBack="true" 
                                    GroupName="rain" OnCheckedChanged="GridView_Potn_RadioSelect" 
                                    OnClick="javascript:Selrdbtn(this.id)" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Hidden_Pot_Id">
                            <ItemTemplate>
                                <asp:Label ID="Label_Hidden_Pot_Id" runat="server" Text='<%# Eval("PotId") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Potential#">
                            <ItemTemplate>
                                <asp:Label ID="Label_RFQId3" runat="server" 
                                    Text='<%# Eval("RFQNo") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Potential Name">
                            <EditItemTemplate>
                                <asp:TextBox  class="form-control" ID="TextBox_RFQName_Edit0" runat="server" Enabled="False" 
                                    Text='<%# Eval("RFQName") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label_RFQName1" runat="server" 
                                    Text='<%# Eval("RFQName") %>'></asp:Label>
                                <asp:Image ID="Image2" runat="server" Height="16px" 
                                    ImageUrl="~/Images/attention.jpg" 
                                    Visible='<%# Eval("Potn_Alert_Required").ToString().Equals("true") %>' 
                                    Width="18px" />
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
                                <asp:TextBox  class="form-control" ID="TextBox_DueDate1" runat="server"  
                                    style="font-family: Andalus" Text='<%# Eval("Due Date") %>' Width="169px"></asp:TextBox>
                                <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_DueDate_CalendarExtender1" 
                                    runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton_DueDate_Edit" 
                                    TargetControlID="TextBox_DueDate"></ajaxtoolkit:CalendarExtender>
                                <asp:Label ID="Label_Due_Date_Edit0" runat="server" 
                                    Text='<%# Eval("Due Date") %>'></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label_Due_Date1" runat="server" Text='<%# Eval("Due Date") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle ForeColor="black" Font-Underline="True" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Followup Date" SortExpression="Next Date Ticks">
                            <EditItemTemplate>
                                <asp:TextBox  class="form-control" ID="TextBox_FwpDate0" runat="server"  
                                    style="font-family: Andalus" Text='<%# Eval("Next Date") %>' Width="169px"></asp:TextBox>
                                <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_FwpDate_CalendarExtender0" 
                                    runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton_FwpDate_Edit" 
                                    TargetControlID="TextBox_FwpDate"></ajaxtoolkit:CalendarExtender>
                                <asp:Label ID="Label_Fwp_Date_Edit0" runat="server" 
                                    Text='<%# Eval("Next Date") %>' Visible="False"></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label_Fwp_Date0" runat="server" Text='<%# Eval("Next Date") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle ForeColor="black" Font-Underline="True" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Customer Details">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton_Cust_Pot" runat="server" 
                                    onclientclick="javascript:unSelrdbtn()" 
                                    Text='<%# Eval("CustName") %>' oncommand="LinkButton_Cust_Pot_Command"></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Contact0" runat="server" Visible="False">
                                </asp:DropDownList>
                                <asp:Label ID="Label_Contact0" runat="server" Text='<%# Eval("CustId") %>' 
                                    Visible="False"></asp:Label>
                                <asp:LinkButton ID="LinkButton_Customer_Edit0" runat="server" 
                                    onclientclick="window.open('Popups/Sale/AllLead_Customer.aspx','DispCust','status=1,location=yes,width=1000,height=400,left=100,right=500',true);" 
                                    Text='<%# Eval("CustName") %>' Visible="False"></asp:LinkButton>
                            </EditItemTemplate>
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
                        <asp:TemplateField HeaderText="Creation Mode">
                            <ItemTemplate>
                                <asp:Label ID="Label_Mode0" runat="server" Text='<%# Eval("Mode") %>'></asp:Label>
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
                        <br />
                        <asp:Button ID="Button_Pot_Refresh" runat="server" ForeColor="#336600" 
                            onclick="Button_Pot_Refresh_Click" 
                            style="font-family: Andalus; font-size: x-small" Text="Refresh Data!" />
                        &nbsp;&nbsp;<asp:Button ID="Button_Notes_Potn" runat="server" Enabled="False" class="btn btn-sm btn-success"
                            onclick="Button_Notes_Potn_Click" style="font-family: Andalus" Text="Notes!" />
                        &nbsp;<asp:Button ID="Button_Audit_Potn" runat="server" Enabled="False" class="btn btn-sm btn-success"
                            onclick="Button_Audit_Potn_Click" style="font-family: Andalus" Text="Logs!" />
                        <br />
                        <br />
                                    </ContentTemplate>
                    </asp:UpdatePanel>
                      <br />
                    </div>                
            </div>
        </div>
        </div>
        </asp:Panel>    
                            <ajaxtoolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender_Potn" runat="server" Collapsed="true" AutoCollapse="false" TargetControlID="Panel_All_Potential"
                        ImageControlId="Potn_Image"  CollapsedImage="~/Images/ArrowTip_Down.png" ExpandedImage="~/Images/ArrowTip_Parallel.png"  ExpandControlID="LinkButton_Potn"
             CollapseControlID="LinkButton_Potn"/>
                     </ContentTemplate>
        </asp:UpdatePanel>
             <br />
        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
        <ContentTemplate>
                                                                    <table width="100%">
                                                                  <tr>
                                                                <td background="../Images/menu_bg.gif" >
                                                                                                                                <asp:LinkButton ID="LinkButton_Defect" runat="server" ForeColor="White" 
                                                                        ToolTip="Click to expand or collapse">Unresolved Defects Assigned to Me</asp:LinkButton>
                                                                        <div style="float: right; vertical-align: middle;">
                                                                        <asp:Image ID="Defect_Img" runat="server" ImageAlign="AbsMiddle" />
                                                                        </div>
                                                                    </td>
                                                            </tr>
                                                            </table>
    <asp:Panel ID="Panel_All_Incoming_Defects" runat="server" 
            style="font-family: Andalus">
            <br />
                 <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title">Defects</h3>
            </div>
            <div class="panel-body">     
            <br />
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
                        <asp:TextBox  class="form-control" ID="TextBox_Defect_No_Incm_Defect" runat="server" 
                            style="font-family: Andalus" Width="20%"></asp:TextBox>
                        &nbsp;Defect Severity:<asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Incm_Defect_Sev" 
                            runat="server" style="font-family: Andalus">
                        </asp:DropDownList>
                        &nbsp;&nbsp;<asp:Button ID="Button_Filter_Incoming_Defects" runat="server" class="btn btn-sm btn-success"
                            onclick="Button_Filter_Incoming_Defects_Click" style="font-family: Andalus" 
                            Text="Filter" ValidationGroup="IncomingDefectFilter" />
                        </td>
                </tr>
            </table>
                                    </div>
                        </div>                    
                <br />
         <div align="center">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="GridView_Incoming_Defects" 
                        runat="server" AllowPaging="True" 
                    AutoGenerateColumns="False" CellPadding="4" 
                    GridLines="None" 
                                        CssClass="table table-striped table-bordered table-hover tableShadow"  
                    PageSize="5" Visible="False" 
                        style="font-size: small" Height="30%" Width="100%"                         
                            onpageindexchanging="GridView_Incoming_Defects_PageIndexChanging" 
                            onselectedindexchanged="GridView_Incoming_Defects_SelectedIndexChanged" 
                            AllowSorting="True" onsorting="GridView_Incoming_Defects_Sorting" 
                            onrowdatabound="GridView_Incoming_Defects_RowDataBound">                            
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:RadioButton ID="def_radio" runat="server" AutoPostBack="true" 
                                            GroupName="rain" OnCheckedChanged="GridView_Def_RadioSelect" 
                                            OnClick="javascript:Selrdbtn(this.id)" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Defect#">
                                    <ItemTemplate>
                                        <asp:Label ID="Label_Defect_Id" runat="server" Text='<%# Eval("DefectId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="RFQ#">
                                    <EditItemTemplate>
                                        <asp:TextBox  class="form-control" ID="TextBox_RFQ_No_Edit" runat="server" Enabled="False" 
                                            Height="28px" style="font-family: Andalus" Text='<%# Eval("RFQId") %>' 
                                            Width="175px"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label_RFQNo" runat="server" Text='<%# Eval("RFQId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Invoice#">
                                    <EditItemTemplate>
                                        <asp:TextBox  class="form-control" ID="TextBox_Invoice_No_Edit" runat="server" Enabled="False" 
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
                                            Text='<%# Eval("descr") %>' value='<%# Eval("descr") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                      <EditItemTemplate>
                                        <asp:TextBox  class="form-control" ID="TextBox_Descr_Edit" runat="server" Enabled="False" 
                                            Height="67px" style="font-family: Andalus" Text='<%# Eval("descr") %>' 
                                            TextMode="MultiLine" Width="329px"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Submit Date" SortExpression="Submit Date Ticks">
                                    <ItemTemplate>
                                        <asp:Label ID="Label_Submit_Date" runat="server" 
                                            Text='<%# Eval("Submit Date") %>'></asp:Label>
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
                                        <asp:LinkButton ID="LinkButton_Customer0" runat="server" 
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
                                        <asp:TextBox  class="form-control" ID="TextBox_Defect_Stat_Reason_Edit" runat="server" Height="67px" 
                                            Text='<%# Eval("Defect_Stat_Reason") %>' TextMode="MultiLine" Width="329px"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label_Defect_Stat_Reason" runat="server" 
                                            Text='<%# Eval("Defect_Stat_Reason") %>'></asp:Label>
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
                                    <EditItemTemplate>
                                        <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Incm_Defect_Resol_Stat_Edit" runat="server" 
                                            style="font-family: Andalus">
                                        </asp:DropDownList>
                                        &nbsp;<asp:Label ID="Label_Defect_Resol_Stat_Edit" runat="server" 
                                            Text='<%# Eval("Defect_Resol_Stat") %>' Visible="False"></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label_Defect_Resol_Stat" runat="server" 
                                            Text='<%# Eval("Defect_Resol_Stat") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount">
                                    <EditItemTemplate>
                                        <asp:TextBox  class="form-control" ID="Textbox_Edit_Amount" runat="server" 
                                            Text='<%# Eval("Amount") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label_Amount0" runat="server" Text='<%# Eval("Amount") %>'></asp:Label>
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
                        <br />
                        <asp:Button ID="Button_Inc_Defect_Refresh" runat="server" ForeColor="#336600" 
                            onclick="Button_Inc_Defect_Refresh_Click" 
                            style="font-family: Andalus; font-size: x-small" Text="Refresh Data!" />
                        &nbsp;&nbsp;&nbsp;<asp:Button ID="Button_Notes_Incm_Def" runat="server" Enabled="False" class="btn btn-sm btn-success"
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
        </div>
        </div>    
    </asp:Panel>
                                <ajaxtoolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender_Defect" runat="server" Collapsed="true" AutoCollapse="false" TargetControlID="Panel_All_Incoming_Defects"
                        ImageControlId="Defect_Img"  CollapsedImage="~/Images/ArrowTip_Down.png" ExpandedImage="~/Images/ArrowTip_Parallel.png"  ExpandControlID="LinkButton_Defect"
             CollapseControlID="LinkButton_Defect"/>  
                     </ContentTemplate>
        </asp:UpdatePanel>                                                          
        </asp:Content>
