<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllDefectsForInvoice.aspx.cs" Inherits="OnLine.Pages.Popups.Purchase.AllDefectsForInvoice" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        * {
    padding: 0; 
    margin: 0;
            font-size: small;
        }

.mGrid { width: 100%; background-color: #fff; margin: 5px 0 10px 0; border: solid 1px #525252; border-collapse:collapse; }
    .mGrid th { padding: 4px 2px; color: #fff; background: #7795BD url('/Images/menu_bg.gif') repeat-x top; border-left: solid 1px #525252; font-size: 0.9em; }
    .mGrid td { padding: 2px; border: solid 1px #c1c1c1; color: Black; }
   .mGrid .alt { background: #fcfcfc url('/Images/grd_alt.png') repeat-x top; }
.mGrid .pgr {background: #7795BD url('/Images/menu_bg.gif') repeat-x top; }
    .mGrid .pgr td { border-width: 0; padding: 0 6px; border-left: solid 1px #666; font-weight: bold; color: #fff; line-height: 12px; }   
    .mGrid .pgr table { margin: 5px 0; }
    .mGrid .pgr a { color: #666; text-decoration: none; }
    </style>
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
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
    
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    
        <br />
    
        <asp:Panel ID="Panel_Defects" runat="server" BorderColor="#0066CC" 
            BorderStyle="Ridge" BorderWidth="2px" GroupingText="All Defects For This Invoice" 
            style="font-family: Andalus">
            <div align="center">
                        <div align="center">
                            <asp:Label ID="Label_Empty_Grid" runat="server" ForeColor="Red" 
                                Text="No defect created for this invoice. Defect can be created from defect page." 
                                Visible="False"></asp:Label>
                        </div>
                <br />
                <div align="center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                    <asp:GridView ID="GridView_Defects" 
                        runat="server" 
                    AutoGenerateColumns="False" CellPadding="4" 
                    GridLines="None" 
                      CssClass="mGrid"
            PagerStyle-CssClass="pgr"
            AlternatingRowStyle-CssClass="alt"
            SelectedRowSyle-CssClass="sel"
                    PageSize="3" Visible="False" 
                        style="font-size: small" Width="90%">                   
                        <AlternatingRowStyle CssClass="alt" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:RadioButton ID="outg_radio" runat="server" AutoPostBack="true" 
                                        GroupName="rain" OnCheckedChanged="GridView_Inv_RadioSelect" 
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
                                        onclientclick="&quot;alertDescrIncoming(this)&quot;" 
                                        Text='<%# Eval("descr") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox class="form-control" ID="TextBox_Descr_Edit" runat="server" Enabled="False" 
                                        Height="67px" style="font-family: Andalus" Text='<%# Eval("descr") %>' 
                                        TextMode="MultiLine" Width="329px"></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Submit Date">
                                <ItemTemplate>
                                    <asp:Label ID="Label_Submit_Date" runat="server" 
                                        Text='<%# Eval("Submit Date") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Close Date">
                                <ItemTemplate>
                                    <asp:Label ID="Label_Close_Date" runat="server" 
                                        Text='<%# Eval("Close Date") %>'></asp:Label>
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
                        </Columns>
                        <PagerStyle CssClass="pgr" />
                    <SelectedRowStyle BackColor="#B9E5FB" Font-Bold="True" ForeColor="#00488F" />
                </asp:GridView>
                    </ContentTemplate>
                    </asp:UpdatePanel>
                    &nbsp;&nbsp;<br />
                    <br />
                        </div>
                
            </div>
        </asp:Panel>
       
    </div>
    </form>
</body>
</html>
