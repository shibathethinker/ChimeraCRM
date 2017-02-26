<%@ Page Title="" Language="C#" Theme="ThemeBlue" MasterPageFile="~/Pages/Site1.Master" AutoEventWireup="true" CodeBehind="AllContacts.aspx.cs" Inherits="OnLine.Pages.AllContacts" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
        .style6
        {
            width: 90%;        
            font-size="small";    
           /* border: 1px solid #c0c0c0;*/
        }
        .style7
        {
            font-size: small;
        }
        .style8
        {
            width: 100%;
            border-collapse: collapse;
            border-bottom-color: "#0066CC";
            font-size: small;
            border-left-width: thin;
            border-right-width: thin;
            border-top-width: thin;
            border-bottom-width: thin;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderHeader" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderBody" runat="server">
<script language="javascript">

    function ClearAllControls() {
        for (i = 0; i < document.forms[0].length; i++) {
            doc = document.forms[0].elements[i];
            switch (doc.type) {
                case "text":
                    doc.value = "";
                    break;

                case "checkbox":
                    doc.checked = false;
                    break;

                case "radio":
                    doc.checked = false;
                    break;

                case "select-one":
                    doc.options[doc.selectedIndex].selected = false;
                    doc.value = "_";
                    break;

                case "select-multiple":
                    while (doc.selectedIndex != -1) {
                        indx = doc.selectedIndex;
                        doc.options[indx].selected = false;
                    }
                    doc.selected = false;
                    break;
                default:
                    break;
            }
        }
    }

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

            <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
                        <asp:Label ID="Label_Contact_Screen_Access" runat="server" ForeColor="Red" 
                            style="font-size: small; font-family: Andalus;" 
        Visible="False"></asp:Label>
                  <div class="panel panel-primary" Width="95%">
            <div class="panel-heading">
              <h3 class="panel-title">All Accounts</h3>
            </div>
            <div class="panel-body">  
        <asp:Panel ID="Panel_All_Contact" runat="server" 
            style="font-family: Andalus">
            <div align="center">  
                                  <div class="panel panel-success">
            <div class="panel-heading">
              <h3 class="panel-title">Filter By</h3>
            </div>
            <div class="panel-body">
                        <table class="style6">
                            <tr>
                                <td align="center">
                                    Name :
                                    <asp:TextBox class="form-control" ID="TextBox_Contact_Name" runat="server" 
                                        style="font-family: Andalus" Width="20%"></asp:TextBox>
                                    &nbsp; Short Name:&nbsp;<asp:TextBox class="form-control" ID="TextBox_Contact_ShortName" runat="server" 
                                        style="font-family: Andalus" Width="20%"></asp:TextBox>
                                    &nbsp; Main Product/Services:&nbsp;<asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Prod" 
                                        runat="server" style="font-family: Andalus">
                                    </asp:DropDownList>
                                    &nbsp;<asp:Button ID="Button_Filter_Contacts" runat="server" class="btn btn-sm btn-success"
                                        onclick="Button_Filter_Contacts_Click" style="font-family: Andalus" 
                                        Text="Filter" ValidationGroup="DueDate" />
                                </td>
                            </tr>
                        </table>
                       </div>
            </div>    
                <br />
                <div align="center">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Label ID="Label_Contact_Note" runat="server" style="font-size: small" 
                            Text="Label" Visible="False"></asp:Label>
                        <br />
                    <asp:GridView ID="GridView1" 
                        runat="server" 
                        AllowPaging="True" 
                    AutoGenerateColumns="False" 
                    CellPadding="4" 
                    GridLines="None" 
                                        CssClass="table table-striped table-bordered table-hover tableShadow" 
            PagerStyle-CssClass="pgr"
            AlternatingRowStyle-CssClass="alt"
            SelectedRowStyle-CssClass="sel"
            EditRowStyle-CssClass="sel"
                    onpageindexchanging="GridView1_PageIndexChanging" 
                    onrowdatabound="GridView1_RowDataBound" 
                    onrowdeleting="GridView1_RowDeleting" 
                    onrowediting="GridView1_RowEditing" 
                    onrowupdating="GridView1_RowUpdating" Visible="False" 
                        onselectedindexchanged="GridView1_SelectedIndexChanged" 
                        onselectedindexchanging="GridView1_SelectedIndexChanging" 
                        style="font-size: small" Height="30%" Width="90%" 
                        onrowcancelingedit="GridView1_RowCancelingEdit">                    

                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:RadioButton ID="contact_radio" runat="server" AutoPostBack="true" 
                                        GroupName="rain" OnCheckedChanged="GridView_Contact_RadioSelect" 
                                        OnClick="javascript:Selrdbtn(this.id)" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowEditButton="True" />
                            <asp:TemplateField HeaderText="Hidden">
                                <ItemTemplate>
                                    <asp:Label ID="Label_Hidden" runat="server" Text='<%# Eval("ContactEntId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Account Name">
                                <EditItemTemplate>
                                    <asp:TextBox class="form-control" ID="TextBox_Contact_Name" runat="server" 
                                        style="font-family: Andalus" Text='<%# Eval("ContactName") %>'></asp:TextBox>
                                    &nbsp;<asp:Label ID="Label_Contact_Name_Edit" runat="server" 
                                        Text='<%# Eval("ContactName") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label_Contact_Name" runat="server" 
                                        Text='<%# Eval("ContactName") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Account Short Name">
                                <ItemTemplate>
                                    <asp:Label ID="Label_Contact_Short_Name" runat="server" 
                                        Text='<%# Eval("ShortName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox class="form-control" ID="TextBox_Contact_Short_Name" runat="server" 
                                        style="font-family: Andalus" Text='<%# Eval("ShortName") %>'></asp:TextBox>
                                    &nbsp;<asp:Label ID="Label_Contact_Short_Name_Edit" runat="server" 
                                        Text='<%# Eval("ShortName") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ph No#">
                                <ItemTemplate>
                                    <asp:Label ID="Label_Ph_No" runat="server" Text='<%# Eval("PhNo") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox class="form-control" ID="TextBox_Ph_No" runat="server" style="font-family: Andalus" 
                                        Text='<%# Eval("PhNo") %>'></asp:TextBox>
                                    &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator2" 
                                        runat="server" ControlToValidate="TextBox_Ph_No" Display="Dynamic" 
                                        ErrorMessage="Please enter a valid contact no" ForeColor="Red" 
                                        ValidationExpression="^\d{10,14}$"></asp:RegularExpressionValidator>
                                    <asp:Label ID="Label_Ph_No_Edit" runat="server" Text='<%# Eval("PhNo") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email">
                                <EditItemTemplate>
                                    <asp:TextBox class="form-control" ID="TextBox_Email_Id" runat="server"  
                                        style="font-family: Andalus" Text='<%# Eval("EmailId") %>' Width="169px"></asp:TextBox>
                                    &nbsp;<asp:RegularExpressionValidator ID="RegularExpressionValidator1" 
                                        runat="server" ControlToValidate="TextBox_Email_Id" Display="Dynamic" 
                                        ErrorMessage="Invalid email id" ForeColor="Red" 
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                    <asp:Label ID="Label_Email_Id_Edit" runat="server" 
                                        Text='<%# Eval("EmailId") %>'></asp:Label>
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Label_Email" runat="server" Text='<%# Eval("EmailId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="From Site?">
                                <ItemTemplate>
                                    <asp:Label ID="Label_From_Site" runat="server" Text='<%# Eval("FromSite") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Location">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton_Location" runat="server" 
                                        onclientclick="javascript:unSelrdbtn()" 
                                        oncommand="LinkButton_Location_Command">Show!</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Main Prod/Services">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton_Prd" runat="server" 
                                        onclientclick="javascript:unSelrdbtn()" oncommand="LinkButton_Prd_Command">Show!</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="All Deals With this Account">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton_Deal" runat="server" 
                                        oncommand="LinkButton_Deal_Command" 
                                        onclientclick="javascript:unSelrdbtn()">Show!</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="All Defects">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton_Defect" runat="server" 
                                        oncommand="LinkButton_Defect_Command" 
                                        onclientclick="javascript:unSelrdbtn()">Show!</asp:LinkButton>
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
                        <asp:Label ID="Label_Contact_Grid_Access" runat="server" ForeColor="Red" 
                            style="font-size: small" Visible="False"></asp:Label>
                        <br />
                                                                                                                                       <asp:Button ID="Button_Contact_Refresh_Hidden" runat="server" style="display:none"  
                            ForeColor="#336600" onclick="Button_Contact_Refresh_Hidden_Click" 
                            Text="Hidden" />
                        <asp:Button ID="Button_Req_Refresh" runat="server" ForeColor="#336600" 
                            onclick="Button_Refresh_Click" style="font-family: Andalus; font-size: x-small" 
                            Text="Refresh Data!" />
                        &nbsp;<asp:Button ID="Button_Create_Req" runat="server" class="btn btn-sm btn-success"
                            onclientclick="window.open('/Pages/createContact.aspx','createContact','status=1,scrollbars=1,location=yes,width=1000,height=600,left=200,right=500',true);" 
                            style="font-family: Andalus" Text="Create New Account!" />
                        &nbsp;<asp:Button ID="Button_Notes_Contact" runat="server" Enabled="False" class="btn btn-sm btn-success"
                            onclick="Button_Notes_Contact_Click" style="font-family: Andalus" 
                            Text="Notes!" />
                        &nbsp;<asp:Button ID="Button_Audit_Contact" runat="server" Enabled="False" class="btn btn-sm btn-success"
                            onclick="Button_Audit_Contact_Click" style="font-family: Andalus" 
                            Text="Logs!" />
                    </ContentTemplate>
                    </asp:UpdatePanel>                    
                    <br />
                    </div>              
            </div>
        </asp:Panel>
               </div>
        </div>
        <ajaxtoolkit:RoundedCornersExtender ID="Panel_All_Contact_RoundedCornersExtender" 
        runat="server" TargetControlID="Panel_All_Contact" Radius="20" Corners="All">
    </ajaxtoolkit:RoundedCornersExtender>
        <div align="center" id="hover" 
        style="position:fixed; right:10px; top:250px; z-index:20; height:auto;">
        <asp:Button ID="Button_Clear_Filter_All" runat="server" 
                                                                        class="btn btn-sm btn-danger"
                                                                        style="font-family: Andalus" Text="Clear Filter" 
                                                                        onclientclick="ClearAllControls();return false;" />
        </div>
        <br />
</asp:Content>
