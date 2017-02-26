<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateSR.aspx.cs" Inherits="OnLine.Pages.CreateSR" %>

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
            function RefreshParentIncm() {
                window.opener.document.getElementById('ContentPlaceHolderBody_Button_Incm_Refresh_Hidden').click();
            }
            function RefreshParentOutg() {
                window.opener.document.getElementById('ContentPlaceHolderBody_Button_Outg_Refresh_Hidden').click();
            }

            function OnRFQSelected(source, eventArgs) {

                var hdnValueID = "<%= hdnValueRFQ.ClientID %>";

                document.getElementById(hdnValueID).value = eventArgs.get_value();
                __doPostBack(hdnValueID, "");
            } 
        </script>
</head>
<body>
    <form id="form1" runat="server">
 <div align="center">
    <br />
            <div class="panel panel-primary" Width="95%">
        <div class="panel-heading">
            <h3 class="style3">Enter New Service Request</h3>
            </div>
            <div class="panel-body">    
    <asp:Panel ID="Panel2" runat="server" 
        style="font-family: Andalus">
        <br />
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
                <div class="panel-info" Width="95%">
        <div class="panel-heading">
            <h3 class="panel-title">Service Request Details</h3>
            </div>
            <div class="panel-body">    
        <asp:Panel ID="Panel_Defect_Det" runat="server" 
            style="font-family: Andalus" >
            <div class="style2">
            <asp:hiddenfield id="hdnValueRFQ" onvaluechanged="hdnValue_ValueChangedRFQ" runat="server" />
                <br />
                &nbsp;<asp:LinkButton ID="LinkButton_RFQ" runat="server" onclick="LinkButton1_Click" 
                    ToolTip="Type in the name of the RFQ and all matching names will be displayed">RFQ#</asp:LinkButton>
                :&nbsp;<asp:TextBox class="form-control" ID="TextBox_Rfq_No" runat="server"  
                    Width="20%"></asp:TextBox>
                <ajaxtoolkit:AutoCompleteExtender ID="TextBox_Rfq_No_AutoCompleteExtender"  ServiceMethod="GetCompletionListRFQ" 
                MinimumPrefixLength="1" onclientitemselected="OnRFQSelected"
                    runat="server" TargetControlID="TextBox_Rfq_No" UseContextKey="True">
                </ajaxtoolkit:AutoCompleteExtender>
                &nbsp;Invoice No:&nbsp;<asp:TextBox class="form-control" ID="TextBox_Invoice_No" runat="server" 
                     Width="20%" AutoPostBack="True" 
                    ontextchanged="TextBox_Invoice_No_TextChanged"></asp:TextBox>
                <ajaxtoolkit:AutoCompleteExtender ID="TextBox_Invoice_No_AutoCompleteExtender"  ServiceMethod="GetCompletionListInvoice" MinimumPrefixLength="1" 
                    runat="server" TargetControlID="TextBox_Invoice_No">
                </ajaxtoolkit:AutoCompleteExtender>
                &nbsp;Severity:
                <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Defect_Sev" runat="server" 
                    style="font-family: Andalus">
                </asp:DropDownList>
                &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                    ControlToValidate="DropDownList_Defect_Sev" Display="Dynamic" ErrorMessage="Required" 
                    ForeColor="Red" ValidationGroup="CreateDefect"></asp:RequiredFieldValidator>
                <br />
                <br />
                SR Amount:
                <asp:TextBox class="form-control" ID="TextBox_Defect_Amount" runat="server"  
                    Width="20%"></asp:TextBox>
                &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator3" 
                    runat="server" ControlToValidate="TextBox_Defect_Amount" Display="Dynamic" 
                    ErrorMessage="Please enter valid number (maximum 11 digits before decimal and 2 after decimal)" 
                    ForeColor="Red" ValidationExpression="^(?:[1-9]\d{0,10})?(?:\.\d{0,2})?$" 
                    ValidationGroup="CreateDefect"></asp:RegularExpressionValidator>
                Currency:
                <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Curr" runat="server" 
                    style="font-family: Andalus">
                </asp:DropDownList>
                <br />
                <br />
                &nbsp;<asp:Label ID="Label_Defect_Descr" runat="server" 
                    Text="Enter Description"></asp:Label>
                &nbsp;<asp:TextBox class="form-control" ID="TextBox_Desc" runat="server"  
                    Width="50%" Height="93px" TextMode="MultiLine" 
                    ValidationGroup="CreateDefect" MaxLength="200"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    ControlToValidate="TextBox_Desc" Display="Dynamic" ErrorMessage="Required" 
                    ForeColor="Red" SetFocusOnError="True" ValidationGroup="CreateDefect"></asp:RequiredFieldValidator>
                <br />
                <br />
                &nbsp;Upload Documnet:
                <asp:FileUpload ID="FileUpload1" runat="server"  
                    ViewStateMode="Enabled" />
                <br />
                <br />
                
            </div>
        </asp:Panel>
        </div>
        </div>
        </ContentTemplate>
        </asp:UpdatePanel>      
        <br />
        
        <div class="panel-info" Width="95%">
        <div class="panel-heading">
            <h3 class="panel-title">Status Details</h3>
            </div>
            <div class="panel-body">   
        <asp:Panel ID="Panel_Defect_Stat" runat="server"  
            style="font-family: Andalus" >
            Acknowlegement Status:
            <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Defect_Stat" runat="server" 
                style="font-family: Andalus">
            </asp:DropDownList>
            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                ControlToValidate="DropDownList_Defect_Stat" Display="Dynamic" ErrorMessage="Required" 
                ForeColor="Red" ValidationGroup="CreateDefect"></asp:RequiredFieldValidator>
            &nbsp;SR Status:
            <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Defect_Resol_Stat" runat="server" 
                style="font-family: Andalus">
            </asp:DropDownList>
            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                ControlToValidate="DropDownList_Defect_Resol_Stat" Display="Dynamic" 
                ErrorMessage="Required" ForeColor="Red" ValidationGroup="CreateDefect"></asp:RequiredFieldValidator>
            <br />
            <br />
            SR Status Justification:&nbsp;<asp:TextBox class="form-control" ID="TextBox_Resol_Stat_Reason" 
                runat="server" Height="108px" MaxLength="200" TextMode="MultiLine" 
                Width="50%"></asp:TextBox>
            &nbsp;</asp:Panel>
            </div>
            </div>
        <br />
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
                <div class="panel-info" Width="95%">
        <div class="panel-heading">
            <h3 class="panel-title">Account Details</h3>
            </div>
            <div class="panel-body">   
        <asp:Panel ID="Panel_Contacts" runat="server" 
            style="font-family: Andalus" >
            Select Account From Your Organization&#39;s Account List:
            <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Contacts" runat="server" 
                style="font-family: Andalus">
            </asp:DropDownList>
            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="DropDownList_Contacts" Display="Dynamic" ErrorMessage="Required" 
                ForeColor="Red" ValidationGroup="CreateDefect"></asp:RequiredFieldValidator>
            &nbsp;&nbsp;<asp:LinkButton ID="LinkButton1" runat="server" 
                 onclick="LinkButton1_Click" OnClientClick = "SetTarget();">Create New Account and Add to this SR!</asp:LinkButton>
            &nbsp;&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator2" 
                runat="server" ControlToValidate="DropDownList_Contacts" Display="Dynamic" 
                ForeColor="Red" ValidationGroup="CreateLeadManual">Must select an account for manully created SR</asp:RequiredFieldValidator>
        </asp:Panel>
        </div>
        </div>
                    </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <br />

                <div align="center">
                        
                    <asp:Button ID="Button_Submit_Defect" runat="server" 
                        style="font-family: Andalus" class="btn btn-sm btn-success"
                        Text="Submit" ValidationGroup="CreateDefect" 
                        onclick="Button_Submit_Defect_Click" Height="41px" />
                    <br />
                    <asp:Label ID="Label_Status" runat="server" Text="Label" Visible="False"></asp:Label>
                    <br />
                    <br />
        </div>
        <br />
    </asp:Panel>
    </div>
    </div>
    
    </div>
    </form>
</body>
</html>
