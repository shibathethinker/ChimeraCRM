<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Popups/AdminPref/WorkFlow.Master" AutoEventWireup="true" CodeBehind="WorkflowSR.aspx.cs" Inherits="OnLine.Pages.Popups.AdminPref.WorkflowSR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
legend {color:red}
        .style1
        {
            font-family: Andalus;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderBody" runat="server">
    <div align="center">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
                                    <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title">Define SLA rules</h3>
            </div>
            <div class="panel-body">  
    <asp:Panel ID="Panel_SLA" runat="server"
            style="font-family: Andalus">
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
        <ContentTemplate>
            <asp:Panel ID="Panel4" runat="server" >
                Send update on SR Logging and Closure to customer from this Email Id:
                <asp:TextBox class="form-control" ID="TextBox_Alert_Email" runat="server"  
                    Width="20%"></asp:TextBox>
                &nbsp;<br /> Password:&nbsp;<asp:TextBox class="form-control" ID="TextBox_Alert_Pass" runat="server" 
                     TextMode="Password" Width="20%"></asp:TextBox>
&nbsp;
                <asp:Button ID="Button_Add_Email" runat="server" style="font-family: Andalus" class="btn btn-sm btn-success"
                    Text="Add" ValidationGroup="Panel2" onclick="Button_Add_Email_Click" />
                <asp:Label ID="Label1" runat="server" Text=" " Visible="False"></asp:Label>
                <br />
                <br />
                Email body to be sent in messages when <strong>New SR is logged</strong>:
                <asp:TextBox class="form-control" ID="TextBox_New_Defect_Email" runat="server"  
                    MaxLength="300" TextMode="MultiLine" Height="50%" Width="50%" ></asp:TextBox>
&nbsp;<asp:Button ID="Button_New_Email_Body" runat="server" class="btn btn-sm btn-success"
                    onclick="Button_New_Email_Body_Click" style="font-family: Andalus" Text="Add" 
                    ValidationGroup="Panel2" />
                <br />
                <br />
                Email body to be sent in messages when <strong>SR is Closed</strong>:
                <asp:TextBox class="form-control" ID="TextBox_Resolved_Defect_Email" runat="server" 
                     MaxLength="300" TextMode="MultiLine" 
                    Height="50%" Width="50%" ></asp:TextBox>
                &nbsp;<asp:Button ID="Button_Resolved_Email_Body" runat="server" class="btn btn-sm btn-success"
                    onclick="Button_Resolved_Email_Body_Click" style="font-family: Andalus" 
                    Text="Add" ValidationGroup="Panel2" />
                <br />
                <br />
                <asp:Label ID="Label_Email_Stat" runat="server" Visible="False"></asp:Label>
            </asp:Panel>
                    </ContentTemplate>
        </asp:UpdatePanel>
            <br />
            <asp:Panel ID="Panel3" runat="server" >
                            Severity:
                            <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Sev" runat="server" 
                                style="font-family: Andalus">
                </asp:DropDownList>
                            &nbsp;SLA (in hours):
                            <asp:TextBox class="form-control" ID="TextBox_SLA_Hr" runat="server"  Width="5%" 
                                ValidationGroup="sla"></asp:TextBox>
                            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                ControlToValidate="TextBox_SLA_Hr" Display="Dynamic" ErrorMessage="*" 
                                ForeColor="#FF3300" ValidationGroup="sla"></asp:RequiredFieldValidator>
                            &nbsp;Start Alert
                            <asp:TextBox class="form-control" ID="TextBox_SLA_Alert" runat="server"  
                                Width="5%"></asp:TextBox>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" 
                                ControlToCompare="TextBox_SLA_Hr" ControlToValidate="TextBox_SLA_Alert" 
                                Display="Dynamic" ErrorMessage="Alert Hours Must be less than SLA hours" 
                                ForeColor="Red" Operator="LessThan" Type="Double" ValidationGroup="sla"></asp:CompareValidator>
                            &nbsp;hrs onwards &nbsp;
                            <asp:Button ID="Button_Add_SLA_Rule" runat="server" style="font-family: Andalus" class="btn btn-sm btn-success"
                                Text="Add Rule" ValidationGroup="sla" 
                                onclick="Button_Add_SLA_Rule_Click" />
                            <asp:Label ID="Label_SLA_Exists" runat="server" ForeColor="Red" Visible="False"></asp:Label>
            </asp:Panel>
            <br />
           <br />
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                <asp:GridView ID="GridView1" runat="server" 
                    AutoGenerateColumns="False" CellPadding="4" 
                    width="50%"
                                                                        CssClass="table table-striped table-bordered table-hover tableShadow" 
                    GridLines="None" 
                    PageSize="3" Visible="False" onrowdeleting="GridView1_RowDeleting">
                    <Columns>
                        <asp:CommandField ShowDeleteButton="True" />
                        <asp:TemplateField HeaderText="Hidden" Visible="False">
                            <EditItemTemplate>
                                <asp:TextBox class="form-control" ID="TextBox1" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label_Hidden" runat="server" Text='<%# Eval("Hidden_Type") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Severity">
                            <ItemTemplate>
                                <asp:Label ID="Label_Sev" runat="server" Text='<%# Eval("sev") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SLA">
                            <ItemTemplate>
                                <asp:Label ID="Label_SLA" runat="server" Text='<%# Eval("sla") %>' 
                                    ForeColor="Red" ToolTip="Will Show in Red color after SLA breach"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Alert After">
                            <ItemTemplate>
                                <asp:Label ID="Label_Alert_Before" runat="server" 
                                    Text='<%# Eval("alertBefore") %>' ForeColor="YellowGreen" 
                                    ToolTip="Will Show in YellowGreen color after this many hours"></asp:Label>
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
    </asp:Panel>
    </div>
    </div>
</div>
</asp:Content>
