<%@ Page Title="" Language="C#" Theme="ThemeBlue" MasterPageFile="~/Pages/Site1.Master" AutoEventWireup="true" CodeBehind="AdminPref.aspx.cs" Inherits="OnLine.Pages.AdminPref" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHeader" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderBody" runat="server">
    <p>
                        <asp:Label ID="Label_Admin_Screen_Access" runat="server" ForeColor="Red" 
                            style="font-size: small; font-family: Andalus;" 
        Visible="False"></asp:Label>
        <div align="center">
            <ul>
                <li>
                    <asp:LinkButton ID="LinkButton_MBE" runat="server" 
            style="font-family: Andalus; font-size: medium" 
                        onclientclick="window.open('Popups/AdminPref/mainEntityManagement.aspx','AdminMBE','resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=1000,height=400,left=100,right=500',true);">Manage Main Business Entity Details</asp:LinkButton>
                </li>
                <li>
                    <asp:LinkButton ID="LinkButton_Sec_Grp" runat="server" 
            style="font-family: Andalus; font-size: medium" 
                        
                        onclientclick="window.open('Popups/AdminPref/AccessMgmt.aspx','SecurityMgmt','resizable=1,resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=1000,height=800,left=100,right=500');">Access and Security Groups Management</asp:LinkButton>
                </li>
                <li>
                    <asp:LinkButton ID="LinkButton_Users" runat="server" 
            style="font-family: Andalus; font-size: medium" 
                        
                        onclientclick="window.open('Popups/AdminPref/userMgmt.aspx','userMgmt','resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=1000,height=800,left=100,right=500,resizable=1',true);">Users and Hierarchy Management</asp:LinkButton>
                </li>
                <li>
                    <asp:LinkButton ID="LinkButton_Workflow" runat="server" 
            style="font-family: Andalus; font-size: medium" 
                        
                        onclientclick="window.open('Popups/AdminPref/WorkflowDefect.aspx','WorkFlowMgmt','resizable=1,resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=1000,height=800,left=100,right=500');" 
                        ToolTip="Define Rules for Defects,RFQ and Invoice">Workflow Management</asp:LinkButton>
                </li>
                <li>
                    <asp:LinkButton ID="LinkButton_Docformat" runat="server" 
            style="font-family: Andalus; font-size: medium" 
                        onclientclick="window.open('Popups/AdminPref/DocFormatManagement.aspx','AdminDocFormat','resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=1000,height=400,left=100,right=500',true);">Document Format Management</asp:LinkButton>
                </li>
            </ul>
            <p>
                &nbsp;</p>
    </div>
        <br />
    </p>
</asp:Content>
