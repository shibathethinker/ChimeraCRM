<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Workflow_Tree.aspx.cs" Inherits="OnLine.Pages.Workflow_Tree" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
          <link type="text/css" rel="stylesheet" href="~/css/bootstrap.css" /> 
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
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <script language="javascript">

        function RefreshParentRFQ() {
            window.opener.document.getElementById('ContentPlaceHolderBody_Button_Rfq_Refresh').click();
            window.clos();
        }
        function RefreshParentInv() {
            window.opener.document.getElementById('ContentPlaceHolderBody_Button_Inv_Refresh').click();
            window.clos();
        }
    </script>
    <div align="center">
    <br />
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
                    <asp:GridView ID="GridView1" 
                        runat="server" 
                    AutoGenerateColumns="False" CellPadding="4" 
                    GridLines="None" 
                     CssClass="table table-striped table-bordered table-hover tableShadow" 
                    PageSize="5" 
                        style="font-size: small" Width="50%" >
                    <Columns>
                        <asp:TemplateField HeaderText="User Id">
                            <ItemTemplate>
                                <asp:Label ID="Label_user" runat="server" style="font-family: Andalus" 
                                    Text='<%# Eval("usrid") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action Taken">
                            <ItemTemplate>
                                <asp:Label ID="Label_action" runat="server" Text='<%# Eval("action") %>' 
                                    style="font-family: Andalus"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Comment">
                            <ItemTemplate>
                                <asp:Label ID="Label_Comment" runat="server" style="font-family: Andalus" 
                                    Text='<%# Eval("comment") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Timestamp">
                            <ItemTemplate>
                                <asp:Label ID="Label_Timestamp" runat="server" style="font-family: Andalus" 
                                    Text='<%# Eval("timestamp") %>'></asp:Label>
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
    </div>
    
    </div>
    <br />
    <div align="center">
                <div class="panel-info" Width="90%">
            <div class="panel-heading">
            <h3 class="panel-title"></h3>
            </div>
            <div class="panel-body">   
        <asp:Panel ID="Panel_Approval" runat="server" 
            style="font-family: Andalus">
            <br />
            <asp:Label ID="Label_Comments" runat="server" Text="Comments:"></asp:Label>
            &nbsp;<asp:TextBox class="form-control" ID="TextBox_Comment" runat="server" Height="100px" MaxLength="100" 
                TextMode="MultiLine" Width="50%" ValidationGroup="Approval_Validate"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="TextBox_Comment" Display="Dynamic" 
                ErrorMessage="* Enter Remarks" ForeColor="Red" SetFocusOnError="True" 
                ValidationGroup="Approval_Validate"></asp:RequiredFieldValidator>
            <br />
            <br />
            <asp:Button ID="Button_Approve" runat="server" onclick="Button_Approve_Click" 
                style="font-family: Andalus" Text="Approve" class="btn btn-sm btn-success"
                ValidationGroup="Approval_Validate" />
            &nbsp;<asp:Button ID="Button_Reject" runat="server" onclick="Button_Reject_Click" 
                style="font-family: Andalus" Text="Reject" class="btn btn-sm btn-success"
                ValidationGroup="Approval_Validate" />
        </asp:Panel>
        </div>
        </div>
        </div>
    </form>
</body>
</html>
