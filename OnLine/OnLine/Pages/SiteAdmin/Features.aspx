<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Features.aspx.cs" Inherits="OnLine.Pages.SiteAdmin.Features" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
legend {color:red}
        .style1
        {
            font-family: Andalus;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <br />
        <br />
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    <asp:Panel ID="Panel2" runat="server" BorderColor="#0066CC" BorderStyle="Ridge" 
        BorderWidth="2px" GroupingText="Features and Specifications" 
        style="font-family: Andalus">
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Available 
        Features:
        <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList1" runat="server" 
            onselectedindexchanged="DropDownList1_SelectedIndexChanged" 
            style="font-family: Andalus">
        </asp:DropDownList>
        &nbsp;
        <asp:Button ID="Button_Show_Feat" runat="server" 
            onclick="Button_Show_Feat_Click" style="font-family: Andalus" 
            Text="Show Specification List!" />
        <br />
&nbsp;<br />
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
        <div align="center">
        <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" 
            GridLines="None" AllowPaging="True" PageSize="3" Visible="False" 
                onpageindexchanging="GridView1_PageIndexChanging" 
                onrowdeleted="GridView1_RowDeleted" onrowdeleting="GridView1_RowDeleting" 
                onrowediting="GridView1_RowEditing" onrowupdated="GridView1_RowUpdated" 
                onrowupdating="GridView1_RowUpdating">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                <asp:CommandField ShowDeleteButton="True" />
                <asp:CommandField ShowCancelButton="False" ShowEditButton="True" />
            </Columns>
            <EditRowStyle BackColor="#999999" />
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#E9E7E2" />
            <SortedAscendingHeaderStyle BackColor="#506C8C" />
            <SortedDescendingCellStyle BackColor="#FFFDF8" />
            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
        </asp:GridView>
            <br />
            <asp:Button ID="Button_Feat_Spec" runat="server" 
                onclick="Button1_Click" style="font-family: Andalus" Text="Submit!" 
                ValidationGroup="Panel2" Visible="False" />
            &nbsp;<asp:Label ID="Label3" runat="server" Text="Label" Visible="False"></asp:Label>
        </div>
        </ContentTemplate>        
        </asp:UpdatePanel>
        
        
        <br />
        <br />
        <div align="center" >
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <asp:Panel ID="Panel3" runat="server" BorderColor="#0066CC" BorderStyle="Ridge" 
                    BorderWidth="2px" GroupingText="Add New Feature" 
                    style="font-family: Andalus">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Name:
                    <asp:TextBox class="form-control" ID="TextBox8" runat="server"  Width="169px"></asp:TextBox>
                    &nbsp;Weightage:
                    <asp:TextBox class="form-control" ID="TextBox9" runat="server"  Width="169px"></asp:TextBox>
                    &nbsp;
                    <asp:Button ID="Button_Add_Feat" runat="server" 
                        onclick="Button_Add_Feat_Click" style="font-family: Andalus" Text="Add!" />
                    &nbsp;<asp:Label ID="Label1" runat="server" Text="Label" Visible="False"></asp:Label>
                </asp:Panel>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
        </div>
        <div align="center">
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
             <asp:Panel ID="Panel4" runat="server" BorderColor="#0066CC" BorderStyle="Ridge" 
            BorderWidth="2px" GroupingText="Add New Specification" 
            style="font-family: Andalus">
            Name:
            <asp:TextBox class="form-control" ID="TextBox10" runat="server"  Width="169px"></asp:TextBox>
            &nbsp;Dim1:
            <asp:TextBox class="form-control" ID="TextBox11" runat="server"  Width="169px"></asp:TextBox>
            &nbsp; Dim2:
            <asp:TextBox class="form-control" ID="TextBox12" runat="server"  Width="169px"></asp:TextBox>
            &nbsp; Dim3:
            <asp:TextBox class="form-control" ID="TextBox13" runat="server"  Width="169px"></asp:TextBox>
            <br />
             Add to Feature:
            <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList2" runat="server" 
                onselectedindexchanged="DropDownList1_SelectedIndexChanged" 
                style="font-family: Andalus">
            </asp:DropDownList>
            &nbsp;<asp:Button ID="Button_Add_Spec" runat="server" 
                onclick="Button_Add_Spec_Click" style="font-family: Andalus" Text="Add!" 
                />
                 &nbsp;<asp:Label ID="Label2" runat="server" Text="Label" Visible="False"></asp:Label>
        </asp:Panel>
           
            </ContentTemplate>
            </asp:UpdatePanel>
                    
        </div>
        
        <br />

        <br />
    </asp:Panel>
    
    </div>
    </form>
</body>
</html>
