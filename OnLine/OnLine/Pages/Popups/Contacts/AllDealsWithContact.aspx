<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllDealsWithContact.aspx.cs" Inherits="OnLine.Pages.Popups.Contacts.AllDealsWithContact" %>

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
    <form id="form1" runat="server">
    <div align="center">
    
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    
        <br />
                  <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title">All Invoices Received</h3>
            </div>
            <div class="panel-body">  
        <asp:Panel ID="Panel_Incoming_Invoice" runat="server" 
            style="font-family: Andalus" >
            <div align="center">
                        <div align="center">
                        <br />
                                              <div class="panel panel-success">
            <div class="panel-heading">
              <h3 class="panel-title">Filter By</h3>
            </div>
            <div class="panel-body">
                        <asp:Panel ID="Panel_Filter_Requirement" runat="server" 
                    style="font-family: Andalus; font-size: small">
                            Invoice No:
                            <asp:TextBox class="form-control" ID="TextBox_Inv_No" runat="server" style="font-family: Andalus" 
                                Width="20%"></asp:TextBox>
                            &nbsp;&nbsp; &nbsp;From Date (Due) :
                            <asp:TextBox class="form-control" ID="TextBox_From_Date" runat="server"  
                                Width="20%" ValidationGroup="DueDate"></asp:TextBox>
                            <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_From_Date_CalendarExtender" 
                                runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton1" 
                                TargetControlID="TextBox_From_Date">
                            </ajaxtoolkit:CalendarExtender>
                            <asp:ImageButton ID="ImageButton1" runat="server" Height="19px" 
                                ImageUrl="~/Images/Calendar.png" />
                            &nbsp;&nbsp; To Date (Due) :&nbsp;
                            <asp:TextBox class="form-control" ID="TextBox_To_Date" runat="server"  
                                Width="20%" ValidationGroup="DueDate"></asp:TextBox>
                            <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_To_Date_CalendarExtender" 
                                runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton2" 
                                TargetControlID="TextBox_To_Date">
                            </ajaxtoolkit:CalendarExtender>
                            <asp:ImageButton ID="ImageButton2" runat="server" Height="19px" 
                                ImageUrl="~/Images/Calendar.png" />
                            <br /><br /><asp:Button 
                                ID="Button_Filter_Incom_Invoice" runat="server" class="btn btn-sm btn-success"
                                onclick="Button_Filter_Incom_Invoice_Click" style="font-family: Andalus" 
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
                    <asp:GridView ID="GridView_Incoming_Invoices" 
                        runat="server" AllowPaging="True" 
                    AutoGenerateColumns="False" CellPadding="4" 
                    GridLines="None" 
                                       CssClass="table table-striped table-bordered table-hover tableShadow" 
                    PageSize="3" Visible="False" 
                        style="font-size: small" Height="40%" Width="90%" 
                            onpageindexchanging="GridView_Incoming_Invoices_PageIndexChanging" 
                            onselectedindexchanged="GridView_Incoming_Invoices_SelectedIndexChanged">

                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:RadioButton ID="deals_radio" runat="server" AutoPostBack="true" 
                                        GroupName="rain" OnCheckedChanged="GridView_Incoming_Invoices_RadioSelect" 
                                        OnClick="javascript:Selrdbtn(this.id)" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Invoice No">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton_Invoice_Id_No_Incoming" runat="server" 
                                        oncommand="LinkButton_Invoice_Id_No_Incoming_Command" 
                                        Text='<%# Eval("InvNo") %>' onclientclick="javascript:unSelrdbtn()"></asp:LinkButton>
                                    <br />
                                    <asp:Label ID="Label_Inv_Id_Hidden" runat="server" Text='<%# Eval("InvId") %>' 
                                        Visible="False"></asp:Label>
                                    <asp:Label ID="Label_rfq_Id_Hidden" runat="server" Text='<%# Eval("rfqId") %>' 
                                        Visible="False"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Date">
                                <ItemTemplate>
                                    <asp:Label ID="Label_Date" runat="server" Text='<%# Eval("InvDate") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Payment Details">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton_Pmnt_Det_Incoming" runat="server" 
                                        onclick="LinkButton_Pmnt_Det_Incoming_Click" 
                                        onclientclick="javascript:unSelrdbtn()">Show!</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Payment Stat">
                                <ItemTemplate>
                                    <asp:Label ID="Label_Pmnt_Stat" runat="server" Text='<%# Eval("pmntStat") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Post Tax Amount">
                                <ItemTemplate>
                                    <asp:Label ID="Label_Total_Amnt" runat="server" Text='<%# Eval("totalAmnt") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Pending Amount">
                                <ItemTemplate>
                                    <asp:Label ID="Label_Total_Pending" runat="server" 
                                        Text='<%# Eval("totalPending") %>'></asp:Label>
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
    <p>
       </p>
                 <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title">All Invoices Sent</h3>
            </div>
            <div class="panel-body">  
        <asp:Panel ID="Panel_Outgoing_Invoice" runat="server" 
            style="font-family: Andalus" >
            <div align="center">
            <br />
                        <div align="center">
                                                                      <div class="panel panel-success">
            <div class="panel-heading">
              <h3 class="panel-title">Filter By</h3>
            </div>
            <div class="panel-body">
                        <asp:Panel ID="Panel_Filter_Requirement0" runat="server" 
                    style="font-family: Andalus; font-size: small">
                            Invoice No:
                            <asp:TextBox class="form-control" ID="TextBox_Inv_No_Outgoing" runat="server" style="font-family: Andalus" 
                                Width="20%"></asp:TextBox>
                            &nbsp;&nbsp; &nbsp;From Date (Due) :
                            <asp:TextBox class="form-control" ID="TextBox_From_Date_Outgoing" runat="server"  
                                Width="20%" ValidationGroup="DueDate"></asp:TextBox>
                            <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_From_Date_Outgoing_CalendarExtender" 
                                runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton3" 
                                TargetControlID="TextBox_From_Date_Outgoing">
                            </ajaxtoolkit:CalendarExtender>
                            <asp:ImageButton ID="ImageButton3" runat="server" Height="19px" 
                                ImageUrl="~/Images/Calendar.png" />
                            &nbsp;&nbsp; To Date (Due) :&nbsp;
                            <asp:TextBox class="form-control" ID="TextBox_To_Date_Outgoing" runat="server"  
                                Width="20%" ValidationGroup="DueDate"></asp:TextBox>
                            <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_To_Date_Outgoing_CalendarExtender" 
                                runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton4" 
                                TargetControlID="TextBox_To_Date_Outgoing">
                            </ajaxtoolkit:CalendarExtender>
                            <asp:ImageButton ID="ImageButton4" runat="server" Height="19px" 
                                ImageUrl="~/Images/Calendar.png" />
                            <br /><br /><asp:Button ID="Button_Filter_Outgoing_Invoice" class="btn btn-sm btn-success"
                                runat="server" style="font-family: Andalus" 
                                Text="Filter" ValidationGroup="DueDate" 
                                onclick="Button_Filter_Outgoing_Invoice_Click" />
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
                    <asp:GridView ID="GridView_Outgoing_Invoices" 
                        runat="server" AllowPaging="True" 
                    AutoGenerateColumns="False" CellPadding="4" 
                    GridLines="None" 
                  CssClass="table table-striped table-bordered table-hover tableShadow" 
                    PageSize="3" Visible="False" 
                        style="font-size: small" Height="40%" Width="90%" 
                            onpageindexchanging="GridView_Outgoing_Invoices_PageIndexChanging" 
                            onselectedindexchanged="GridView_Outgoing_Invoices_SelectedIndexChanged">
       
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:RadioButton ID="deals_radio_outg" runat="server" AutoPostBack="true" 
                                                        GroupName="rain" OnCheckedChanged="GridView_Outgoing_Invoices_RadioSelect" 
                                                        OnClick="javascript:Selrdbtn(this.id)" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Invoice No">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LinkButton_Invoice_Id_No_Outgoing" runat="server" 
                                                        oncommand="LinkButton_Invoice_Id_No_Outgoing_Command" 
                                                        Text='<%# Eval("InvNo") %>' onclientclick="javascript:unSelrdbtn()"></asp:LinkButton>
                                                    <br />
                                                    <asp:Label ID="Label_Inv_Id_Hidden" runat="server" Text='<%# Eval("InvId") %>' 
                                                        Visible="False"></asp:Label>
                                                    <asp:Label ID="Label_rfq_Id_Hidden" runat="server" Text='<%# Eval("rfqId") %>' 
                                                        Visible="False"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label_Date1" runat="server" Text='<%# Eval("InvDate") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Payment Details">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LinkButton_Pmnt_Det_Outgoing" runat="server" 
                                                        oncommand="LinkButton_Pmnt_Det_Outgoing_Command" 
                                                        onclientclick="javascript:unSelrdbtn()">Show!</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Payment Stat">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label_Pmnt_Stat0" runat="server" Text='<%# Eval("pmntStat") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total Post Tax Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label_Total_Amnt1" runat="server" 
                                                        Text='<%# Eval("totalAmnt") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total Pending Amount">
                                                <ItemTemplate>
                                                    <asp:Label ID="Label_Total_Pending0" runat="server" 
                                                        Text='<%# Eval("totalPending") %>'></asp:Label>
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
                    </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                    <br />
                        </div>
                
            </div>
        </asp:Panel>
       </div>
       </div>
    </form>
</body>
</html>
