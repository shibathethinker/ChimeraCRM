<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Popups/AdminPref/WorkFlow.Master" AutoEventWireup="true" CodeBehind="WorkflowInv.aspx.cs" Inherits="OnLine.Pages.Popups.AdminPref.WorkflowInv" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderBody" runat="server">
                                    <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title">Define/Modify Levels of Approvals required before Invoice is floated</h3>
            </div>
            <div class="panel-body">  
    <asp:Panel ID="Panel_Inv_Workflow" runat="server" 
            style="font-family: Andalus">
        <asp:LinkButton ID="LinkButton1" runat="server" 
            ToolTip="All Newly Created Invoice will go through this many levels of approval or till it reaches the highest level of approval (whichever comes earlier)">Levels Required</asp:LinkButton>
        :
        <asp:TextBox class="form-control" ID="TextBox_Apprv_Level" runat="server"  
                Width="10%"></asp:TextBox>
            &nbsp;<asp:Button ID="Button_Submit_Inv_Level" runat="server" class="btn btn-sm btn-success"
            onclick="Button_Submit_Inv_Level_Click" style="font-family: Andalus" 
            Text="Submit!" ValidationGroup="Rfq_Validate" />
        <asp:Label ID="Label_Stat" runat="server" Visible="False"></asp:Label>
    </asp:Panel>
    </div>
    </div>
</asp:Content>
