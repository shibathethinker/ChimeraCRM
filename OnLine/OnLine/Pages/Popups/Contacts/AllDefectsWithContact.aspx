<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllDefectsWithContact.aspx.cs" Inherits="OnLine.Pages.Popups.Contacts.AllDefectsWithContact" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 <link type="text/css" rel="stylesheet" href="~/css/bootstrap.css" /> 
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
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
    <script language="javascript">
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
                      <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title">All Defects Recived From This Contact</h3>
            </div>
            <div class="panel-body">  
        <asp:Panel ID="Panel_Incoming_Defects" runat="server" 
            style="font-family: Andalus" Height="45%">
            <div align="center">
                        <div align="center">
            
            <div class="panel panel-success">
            <div class="panel-heading">
              <h3 class="panel-title">Filter By</h3>
            </div>
            <div class="panel-body">
                        <asp:Panel ID="Panel_Filter_Requirement" runat="server" 
                    style="font-family: Andalus; font-size: small">
                            Defect No:
                            <asp:TextBox class="form-control" ID="TextBox_Def_No" runat="server" style="font-family: Andalus" 
                                Width="20%"></asp:TextBox>
                            &nbsp;&nbsp; &nbsp;RFQ No:
                            <asp:TextBox class="form-control" ID="TextBox_RFQ_No_Incoming_Defect" runat="server" 
                                style="font-family: Andalus" Width="20%"></asp:TextBox>
                            &nbsp;Invoice No:
                            <asp:TextBox class="form-control" ID="TextBox_Inv_No_Incoming_Defect" runat="server" 
                                style="font-family: Andalus" Width="20%"></asp:TextBox>
                            <br /><br /> From Date (Raised) :
                            <asp:TextBox class="form-control" ID="TextBox_From_Date" runat="server"  
                                Width="20%" ValidationGroup="DueDate"></asp:TextBox>
                            <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_From_Date_CalendarExtender" 
                                runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton1" 
                                TargetControlID="TextBox_From_Date">
                            </ajaxtoolkit:CalendarExtender>
                            <asp:ImageButton ID="ImageButton1" runat="server" Height="19px" 
                                ImageUrl="~/Images/Calendar.png" />
                            &nbsp;&nbsp; To Date (Raised) :&nbsp;
                            <asp:TextBox class="form-control" ID="TextBox_To_Date" runat="server"  
                                Width="20%" ValidationGroup="DueDate"></asp:TextBox>
                            <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_To_Date_CalendarExtender" 
                                runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton2" 
                                TargetControlID="TextBox_To_Date">
                            </ajaxtoolkit:CalendarExtender>
                            <asp:ImageButton ID="ImageButton2" runat="server" Height="19px" 
                                ImageUrl="~/Images/Calendar.png" />
                            <br /><br /><asp:Button 
                                ID="Button_Filter_Incom_Defect" runat="server" class="btn btn-sm btn-success"
                                onclick="Button_Filter_Incom_Defect_Click" style="font-family: Andalus" 
                                Text="Filter" ValidationGroup="DueDate" />
                            &nbsp;<asp:CompareValidator ID="CompareValidator1" runat="server" 
                                ControlToCompare="TextBox_From_Date" ControlToValidate="TextBox_To_Date" 
                                Display="Dynamic" ErrorMessage="to date can not be earlier than from date" 
                                ForeColor="Red" Operator="GreaterThan" Type="Date" 
                                ValidationGroup="DueDate"></asp:CompareValidator>
                        </asp:Panel>
                        </div>
                        </div>
                        </div>
                <br />
                <div align="center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                    <asp:GridView ID="GridView_Incoming_Defects" 
                        runat="server" AllowPaging="True" 
                    AutoGenerateColumns="False" CellPadding="4" 
                    GridLines="None" 
                      CssClass="table table-striped table-bordered table-hover tableShadow" 
                    PageSize="3" Visible="False" 
                        style="font-size: small" Height="30%" Width="90%" 
                            onpageindexchanging="GridView_Incoming_Defects_PageIndexChanging" 
                            onselectedindexchanged="GridView_Incoming_Defects_SelectedIndexChanged">                   

                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:RadioButton ID="incm_radio" runat="server" AutoPostBack="true" 
                                        GroupName="rain" OnCheckedChanged="GridView_Defect_RadioSelect" 
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
                                        Width="20%"></asp:TextBox>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label_RFQNo" runat="server" Text='<%# Eval("RFQId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Invoice#">
                                <EditItemTemplate>
                                    <asp:TextBox class="form-control" ID="TextBox_Invoice_No_Edit" runat="server" Enabled="False" 
                                        Height="28px" style="font-family: Andalus" Text='<%# Eval("InvNo") %>' 
                                        Width="20%"></asp:TextBox>
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
                                    &nbsp;<asp:Label ID="Label_Assgn_To_Edit" runat="server" Text='<%# Eval("Assigned_To") %>' 
                                        Visible="False"></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton_Assgn_To" runat="server" 
                                        onclientclick="javascript:unSelrdbtn()" oncommand="LinkButton_Assgn_To_Command" 
                                        Text='<%# Eval("Assigned_To") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Severity">
                                <EditItemTemplate>
                                    <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Incm_Defect_Sev_Edit" runat="server" 
                                        style="font-family: Andalus">
                                    </asp:DropDownList>
                                    <asp:Label ID="Label_Sev_Edit" runat="server" 
                                        Text='<%# Eval("Severity") %>' Visible="False"></asp:Label>
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
                                        onclientclick="javascript:unSelrdbtn()" oncommand="LinkButton_All_Comm_Command">Show!</asp:LinkButton>
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
                                                <RowStyle CssClass="cursor-pointer" />
                        <AlternatingRowStyle CssClass="active" />
                        <SelectedRowStyle CssClass="danger" />
                        <HeaderStyle CssClass="success" />
                        <PagerStyle CssClass="pagination-lg" />
                        <FooterStyle CssClass="success"/>
                        <EditRowStyle CssClass="info" />
                </asp:GridView>
                    </ContentTemplate>
                    </asp:UpdatePanel>
                    &nbsp;&nbsp;<br />
                    <br />
                        </div>
                
            </div>
        </asp:Panel>
        </div>
        </div>
       
        <br />
                              <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title">All Defects Sent To This Contact</h3>
            </div>
            <div class="panel-body">  
        <asp:Panel ID="Panel_Outgoing_Defects" runat="server" 
            style="font-family: Andalus" Height="45%">
            <div align="center">
                        <div align="center">
                                    <div class="panel panel-success">
            <div class="panel-heading">
              <h3 class="panel-title">Filter By</h3>
            </div>
                     <div class="panel-body">
                        <asp:Panel ID="Panel_Filter_Requirement0" runat="server" 
                    style="font-family: Andalus; font-size: small">
                            Defect No:
                            <asp:TextBox class="form-control" ID="TextBox_Def_No_Outgoing" runat="server" style="font-family: Andalus" 
                                Width="20%"></asp:TextBox>
                            &nbsp;&nbsp; RFQ No:
                            <asp:TextBox class="form-control" ID="TextBox_RFQ_No_Outgoing_Defect" runat="server" 
                                style="font-family: Andalus" Width="20%"></asp:TextBox>
                            &nbsp;Invoice No:
                            <asp:TextBox class="form-control" ID="TextBox_Inv_No_Outgoing_Defect" runat="server" 
                                style="font-family: Andalus" Width="20%"></asp:TextBox>
                            <br /><br />From Date (Raised) :
                            <asp:TextBox class="form-control" ID="TextBox_From_Date_Outgoing" runat="server"  
                                Width="20%" ValidationGroup="DueDate"></asp:TextBox>
                            <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_From_Date_Outgoing_Defect_CalendarExtender" 
                                runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton3" 
                                TargetControlID="TextBox_From_Date_Outgoing">
                            </ajaxtoolkit:CalendarExtender>
                            <asp:ImageButton ID="ImageButton3" runat="server" Height="19px" 
                                ImageUrl="~/Images/Calendar.png" />
                            &nbsp;&nbsp; To Date (Raised) :&nbsp;
                            <asp:TextBox class="form-control" ID="TextBox_To_Date_Outgoing" runat="server"  
                                Width="20%" ValidationGroup="DueDate"></asp:TextBox>
                            <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_To_Date_Outgoing_CalendarExtender" 
                                runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton4" 
                                TargetControlID="TextBox_To_Date_Outgoing">
                            </ajaxtoolkit:CalendarExtender>
                            <asp:ImageButton ID="ImageButton4" runat="server" Height="19px" 
                                ImageUrl="~/Images/Calendar.png" />
                            &nbsp;&nbsp;<br />&nbsp;<br /><asp:Button ID="Button_Filter_Outgoing_Defect" class="btn btn-sm btn-success"
                                runat="server" onclick="Button_Filter_Outgoing_Defect_Click" 
                                style="font-family: Andalus" Text="Filter" ValidationGroup="DueDate" />
                            &nbsp;<asp:CompareValidator ID="CompareValidator2" runat="server" 
                                ControlToCompare="TextBox_From_Date_Outgoing" ControlToValidate="TextBox_To_Date_Outgoing" 
                                Display="Dynamic" ErrorMessage="to date can not be earlier than from date" 
                                ForeColor="Red" Operator="GreaterThan" Type="Date" 
                                ValidationGroup="DueDate"></asp:CompareValidator>
                        </asp:Panel>
                        </div>
                        </div>
                        </div>
                <br />
                <div align="center">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="GridView_Outgoing_Defects" runat="server" AllowPaging="True" 
                                AutoGenerateColumns="False" CellPadding="4" 
                                GridLines="None" 
                                 CssClass="table table-striped table-bordered table-hover tableShadow" 
                                Height="40%" 
                                onpageindexchanging="GridView_Outgoing_Defects_PageIndexChanging" PageSize="3" 
                                style="font-size: small" Visible="False" Width="90%" 
                                onselectedindexchanged="GridView_Outgoing_Defects_SelectedIndexChanged">                                

                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:RadioButton ID="outg_radio" runat="server" AutoPostBack="true" 
                                                GroupName="rain" OnCheckedChanged="GridView_Out_Defect_RadioSelect" 
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
                                            <asp:TextBox class="form-control" ID="TextBox_RFQ_No_Edit0" runat="server" Height="28px" 
                                                Text='<%# Eval("RFQId") %>' Width="20%" Enabled="False" 
                                                style="font-family: Andalus"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label_RFQNo_Outgoing" runat="server" 
                                                Text='<%# Eval("RFQId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice#">
                                        <EditItemTemplate>
                                            <asp:TextBox class="form-control" ID="TextBox_Invoice_No_Edit0" runat="server" Height="28px" 
                                                Text='<%# Eval("InvNo") %>' Width="20%"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label_InvoiceNo_Outgoing" runat="server" 
                                                Text='<%# Eval("InvNo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description">
                                        <ItemTemplate>
                                            <asp:Label ID="Label_Descr" runat="server" 
                                                Text='<%# Eval("descr") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox class="form-control" ID="TextBox_Descr_Edit0" runat="server" Height="77px" 
                                                Text='<%# Eval("descr") %>' TextMode="MultiLine" Width="415px"></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Submit Date">
                                        <ItemTemplate>
                                            <asp:Label ID="Label_Submit_Date_Outgoing" runat="server" 
                                                Text='<%# Eval("Submit Date") %>'></asp:Label>
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
                                            <asp:TextBox class="form-control" ID="TextBox_Defect_Stat_Reason_Edit0" runat="server" Height="67px" 
                                                Text='<%# Eval("Defect_Stat_Reason") %>' TextMode="MultiLine" Width="329px"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label_Defect_Stat_Reason0" runat="server" 
                                                Text='<%# Eval("Defect_Stat_Reason") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Severity">
                                        <EditItemTemplate>
                                            <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Outgoing_Defect_Sev" 
                                                runat="server" style="font-family: Andalus">
                                            </asp:DropDownList>
                                            <asp:Label ID="Label_Sev_Edit0" runat="server" 
                                                Text='<%# Eval("Severity") %>' Visible="False"></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label_Sev0" runat="server" 
                                                Text='<%# Eval("Severity") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Defect Resolution Status">
                                        <EditItemTemplate>
                                            <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Outgoing_Defect_Resol_Stat_Edit" 
                                                runat="server" style="font-family: Andalus">
                                            </asp:DropDownList>
                                            &nbsp;<asp:Label ID="Label_Defect_Resol_Stat_Edit0" runat="server" 
                                                Text='<%# Eval("Defect_Resol_Stat") %>' Visible="False"></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label_Defect_Resol_Stat0" runat="server" 
                                                Text='<%# Eval("Defect_Resol_Stat") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="All Communications">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton_All_Comm0" runat="server" 
                                                onclientclick="javascript:unSelrdbtn()" 
                                                oncommand="LinkButton_All_Comm_Outg_Command">Show!</asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="Label_Amount0" runat="server" Text='<%# Eval("Amount") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox class="form-control" ID="Textbox_Edit_Amount0" runat="server" 
                                                Text='<%# Eval("Amount") %>'></asp:TextBox>
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
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                    <br />
                        </div>
                
            </div>
        </asp:Panel>
        </div>
        </div>
       
    </div>
    </form>
</body>
</html>
