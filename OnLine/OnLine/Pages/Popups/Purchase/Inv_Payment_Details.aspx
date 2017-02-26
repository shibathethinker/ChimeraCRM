<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Inv_Payment_Details.aspx.cs" Inherits="OnLine.Pages.Popups.Purchase.Inv_Payment_Details" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link type="text/css" rel="stylesheet" href="~/css/bootstrap.css" /> 
    <title></title>
    <style type="text/css">
legend {color:#00488F}
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
    <div align="center" style="height: 800px">
    
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
        <div class="panel panel-primary" Width="95%">
        <div class="panel-heading">
            <h3 class="style3">Payment Details</h3>
            </div>
            <div class="panel-body">   
            <asp:Panel ID="Panel_All_Pmnt" runat="server"
            style="font-family: Andalus">
                            <asp:Label ID="Label1" runat="server" style="font-size: small" 
                                Text="Payment Details For Invoice#"></asp:Label>
                            &nbsp;<asp:Label ID="Label_Inv_No" runat="server" style="font-size: small"></asp:Label>
                            &nbsp;<asp:Label ID="Label2" runat="server" style="font-size: small" Text="and RFQ#"></asp:Label>
                            &nbsp;<asp:Label ID="Label_RFQ_No" runat="server" style="font-size: small"></asp:Label>
                <br />
                <div align="center">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="GridView_Invoice_Pmnt" runat="server" AllowPaging="True" 
                                AutoGenerateColumns="False" CellPadding="4" 
                                 GridLines="None" 
                                                      CssClass="table table-striped table-bordered table-hover tableShadow" 
                                 Height="40%" PageSize="5" 
                                style="font-size: small" Visible="False" Width="95%" 
                                onpageindexchanging="GridView_Invoice_Pmnt_PageIndexChanging" 
                                onrowcancelingedit="GridView_Invoice_Pmnt_RowCancelingEdit" 
                                onrowdatabound="GridView_Invoice_Pmnt_RowDataBound" 
                                onrowediting="GridView_Invoice_Pmnt_RowEditing" 
                                onrowupdating="GridView_Invoice_Pmnt_RowUpdating" 
                                onrowdeleting="GridView_Invoice_Pmnt_RowDeleting">
       
                                <Columns>
                                    <asp:CommandField ShowEditButton="True" />
                                    <asp:CommandField ShowDeleteButton="True" />
                                    <asp:TemplateField HeaderText="Hidden_Pmnt_Id">
                                        <ItemTemplate>
                                            <asp:Label ID="Label_Pmnt_Id" runat="server" Text='<%# Eval("Pmnt_Id") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Transaction No#">
                                        <EditItemTemplate>
                                            <asp:TextBox class="form-control" ID="TextBox_Tran_No_Edit" runat="server" 
                                                style="font-family: Andalus; font-size: small;" Text='<%# Eval("Tran_No") %>'></asp:TextBox>
                                            &nbsp;
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label_Tran_No" runat="server" Text='<%# Eval("Tran_No") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Payment Type">
                                        <EditItemTemplate>
                                            <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Pmnt_Type_Edit" runat="server" 
                                                style="font-family: Andalus; font-size: small;">
                                            </asp:DropDownList>
                                            &nbsp;<asp:Label ID="Label_Pmnt_Type_Edit" runat="server" 
                                                Text='<%# Eval("Pmnt_Type") %>' Visible="False"></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label_Pmnt_Type" runat="server" Text='<%# Eval("Pmnt_Type") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount">
                                        <EditItemTemplate>
                                            <asp:TextBox class="form-control" ID="TextBox_Amnt_Edit" runat="server" 
                                                style="font-family: Andalus; font-size: small;" Text='<%# Eval("Amount") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label_Amnt" runat="server" Text='<%# Eval("Amount") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Clearing Stat">
                                        <EditItemTemplate>
                                            <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Clearing_Stat_Edit" runat="server" 
                                                style="font-family: Andalus; font-size: small;">
                                            </asp:DropDownList>
                                            &nbsp;<asp:Label ID="Label_Clearing_Stat_Edit" runat="server" 
                                                Text='<%# Eval("Clearing_Stat") %>' Visible="False"></asp:Label>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="Label_Clearing_Stat" runat="server" 
                                                Text='<%# Eval("Clearing_Stat") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Clearing Status Note">
                                        <ItemTemplate>
                                            <asp:Label ID="Label_Clearing_Stat_Note" runat="server" 
                                                Text='<%# Eval("Clearing_Stat_Note") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            &nbsp;
                                            <asp:TextBox class="form-control" ID="TextBox_Clearing_Stat_Note_Edit" runat="server" Height="73px" 
                                                Text='<%# Eval("Clearing_Stat_Note") %>' TextMode="MultiLine" Width="236px"></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Date">
                                        <ItemTemplate>
                                            <asp:Label ID="Label_Pmnt_Date" runat="server" Text='<%# Eval("Pmnt_Date") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox class="form-control" ID="TextBox_Pmnt_Date" runat="server"  
                                                Text='<%# Eval("Pmnt_Date") %>' ValidationGroup="PotDate" Width="169px"></asp:TextBox>
                                            <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_Pmnt_Date_CalendarExtender1" 
                                                runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton5" 
                                                TargetControlID="TextBox_Pmnt_Date">
                                            </ajaxtoolkit:CalendarExtender>
                                            <asp:ImageButton ID="ImageButton5" runat="server" Height="19px" 
                                                ImageUrl="~/Images/Calendar.png" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ConvertEmptyStringToNull="False" HeaderText="Updated By">
                                        <ItemTemplate>
                                            <asp:Label ID="Label_Updated_By" runat="server" Text='<%# Eval("updatedBy") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="All Audit Records">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton_All_Audit_Rec" runat="server" 
                                                oncommand="LinkButton_All_Audit_Rec_Command"
                                                CommandArgument="<%# Container.DataItemIndex %>"                                           
                                                >Show!</asp:LinkButton>
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
                            <asp:Label ID="Label11" runat="server" style="font-size: small" 
                                Text="Total Payment Made:"></asp:Label>
                            &nbsp;<asp:Label ID="Label_Total_Pmnt_Made" runat="server" style="font-size: small"></asp:Label>
                            &nbsp;<asp:Label ID="Label12" runat="server" ForeColor="Black" 
                                style="font-size: small" Text="Total Cleared Amount:"></asp:Label>
                            &nbsp;<asp:Label ID="Label_Total_Cleared" runat="server" style="font-size: small"></asp:Label>
                            &nbsp;<asp:Label ID="Label13" runat="server" ForeColor="Black" 
                                style="font-size: small" Text="Total UnCleared Amount:"></asp:Label>
                            &nbsp;<asp:Label ID="Label_Total_Uncleared" runat="server" style="font-size: small"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <br />
                </asp:Panel>
            </div>
     </div>
                        <asp:Label ID="Label_Access" runat="server" style="font-size: medium; font-family: Andalus;" 
                            Visible="False" ForeColor="Red"></asp:Label>
     
            <br />
     
        <br />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="panel panel-primary" Width="95%">
        <div class="panel-heading">
            <h3 class="style3">Add New Payment Details</h3>
            </div>
            <div class="panel-body">   
                    <asp:Panel ID="Panel1" runat="server" 
            style="font-family: Andalus">
                <br />
                           <asp:Label ID="Label5" runat="server" style="font-size: small" 
                                Text="Transaction No#"></asp:Label>
                            &nbsp;<asp:TextBox class="form-control" ID="TextBox_Tran_No" runat="server" Width="20%" 
                            style="font-family: Andalus"></asp:TextBox>
                            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator2" 
                            runat="server" ControlToValidate="TextBox_Tran_No" Display="Dynamic" 
                            ErrorMessage="*" ForeColor="Red" ValidationGroup="NewPmnt"></asp:RequiredFieldValidator>
                        &nbsp;&nbsp;<asp:Label ID="Label6" runat="server" style="font-size: small" 
                                Text="Transaction Amount"></asp:Label>
                            &nbsp;<asp:TextBox class="form-control" ID="TextBox_Tran_Amnt" runat="server" Width="20%" 
                            style="font-family: Andalus"></asp:TextBox>
                    &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ControlToValidate="TextBox_Tran_Amnt" Display="Dynamic" ErrorMessage="*" 
                            ForeColor="Red" ValidationGroup="NewPmnt"></asp:RequiredFieldValidator>
                        &nbsp;<asp:RegularExpressionValidator 
                            ID="RegularExpressionValidator_Taxable_Amnt" runat="server" 
                            ControlToValidate="TextBox_Tran_Amnt" Display="Dynamic" 
                            ErrorMessage="Please enter valid number (maximum 11 digits before decimal and 2 after decimal)" 
                            ForeColor="Red" ValidationExpression="^(?:[1-9]\d{0,10})?(?:\.\d{0,2})?$" 
                            ValidationGroup="NewPmnt"></asp:RegularExpressionValidator>
                        <br />
                            <br />
                            <asp:Label ID="Label10" runat="server" style="font-size: small" 
                            Text="Clearing Status:" 
                            ToolTip="Defines whether or not this payment is cleared"></asp:Label>
                        &nbsp;<asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Clearing_Stat" runat="server" 
                            style="font-family: Andalus">
                        </asp:DropDownList>
&nbsp;<asp:Label ID="Label7" runat="server" style="font-size: small" Text="Payment Type:"></asp:Label>
                        &nbsp;<asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Pmnt_Type" runat="server" 
                            style="font-family: Andalus">
                        </asp:DropDownList>
                        &nbsp;<asp:Label ID="Label9" runat="server" style="font-size: small" Text="Payment Date:"></asp:Label>
&nbsp;<asp:TextBox class="form-control" ID="TextBox_Pmnt_Date" runat="server"  ValidationGroup="PotDate" 
                            Width="20%"></asp:TextBox>
                        <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_Pmnt_Date_CalendarExtender1" 
                            runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton5" 
                            TargetControlID="TextBox_Pmnt_Date">
                        </ajaxtoolkit:CalendarExtender>
                        <asp:ImageButton ID="ImageButton5" runat="server" Height="19px" 
                            ImageUrl="~/Images/Calendar.png" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                            ControlToValidate="TextBox_Pmnt_Date" Display="Dynamic" ErrorMessage="*" 
                            ForeColor="Red" ValidationGroup="NewPmnt"></asp:RequiredFieldValidator>
                        <br />
                        <br />
                        <asp:Label ID="Label8" runat="server" style="font-size: small" 
                            Text="Clearing Status Note:"></asp:Label>
                        &nbsp;<asp:TextBox class="form-control" ID="TextBox_Clearing_Stat_Note" runat="server" Height="145px" 
                            TextMode="MultiLine" Width="50%"></asp:TextBox>
                            <br />
                        <br />
                        <asp:Button ID="Button_Create_Pmnt" runat="server" style="font-family: Andalus" class="btn btn-sm btn-success"
                            Text="Submit!" onclick="Button_Create_Pmnt_Click" 
                            ValidationGroup="NewPmnt" />
                        <br />
                        <asp:Label ID="Label_Pmnt_Create_Stat" runat="server" style="font-size: small" 
                            Visible="False"></asp:Label>
                        <br /><br />
        </asp:Panel>     
        </div>
        </div>
        </ContentTemplate>
        </asp:UpdatePanel>
  
    </div>
    </form>
</body>
</html>
