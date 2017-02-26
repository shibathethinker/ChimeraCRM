<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllRFQ_Quotes.aspx.cs" Inherits="OnLine.Pages.Popups.Purchase.AllRFQ_Quotes" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <link type="text/css" rel="stylesheet" href="~/css/bootstrap.css" /> 
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center" style="font-family: Andalus">
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
        <asp:Label ID="Label_Empty_Quote" runat="server" Text="No Quote received." 
            ForeColor="Red" Visible="False"></asp:Label>
                    <asp:GridView ID="GridView_RFQ_Resp_Quotes" Width="95%" 
                        runat="server" AllowPaging="True" 
                    AutoGenerateColumns="False" CellPadding="4" 
                    GridLines="None" 
                                        CssClass="table table-striped table-bordered table-hover tableShadow" 
                    onpageindexchanging="GridView_RFQ_Resp_Quotes_PageIndexChanging" 
                    PageSize="3" 
                        onselectedindexchanged="GridView_RFQ_Resp_Quotes_SelectedIndexChanged" 
                        style="font-size: small">                    
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:RadioButton ID="rfq_radio" runat="server" AutoPostBack="true" 
                                    GroupName="rain" OnCheckedChanged="GridView_RFQ_Resp_Quotes_RadioSelect" 
                                    OnClick="javascript:Selrdbtn(this.id)" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Hidden">
                            <ItemTemplate>
                                <asp:Label ID="Label_Hidden" runat="server" Text='<%# Eval("RespCompId") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox class="form-control" ID="TextBox2" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Vendor">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton_Vendor" runat="server" 
                                    Text='<%# Eval("VendName") %>' oncommand="show_Vendor" CommandArgument
                                     ="<%#Container.DataItemIndex %>" onclientclick="javascript:unSelrdbtn()"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Quote Details">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton_All_Quotes" runat="server" 
                                    
                                    onclientclick="javascript:unSelrdbtn()" 
                                    oncommand="LinkButton_All_Quotes_Command">Show!</asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox class="form-control" ID="TextBox8" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="NDA">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton_All_Shortlisted" runat="server" 
                                    onclientclick="javascript:unSelrdbtn()" 
                                    oncommand="LinkButton_All_Shortlisted_Command">Show!</asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox class="form-control" ID="TextBox9" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date">
                            <ItemTemplate>
                                <asp:Label ID="Label_Date" runat="server" Text='<%# Eval("DateVal") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox class="form-control" ID="TextBox10" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="All Communications">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton_All_Audit" OnCommand="ShowComm"  CommandArgument
                                     ="<%#Container.DataItemIndex %>" runat="server">Show!</asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox class="form-control" ID="TextBox3" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Shortlisted?">
                            <ItemTemplate>
                                <asp:Label ID="Label_Short" runat="server" Text='<%# Eval("ShortListed") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox class="form-control" ID="TextBox4" runat="server"></asp:TextBox>
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
                                                                        <asp:Button ID="Button_Rfq_Refresh_Hidden_Index_Unchanged" runat="server" style="display:none"
                            ForeColor="#336600" onclick="Button_Rfq_Refresh_Hidden_Click_Index_Unchanged_Event" onclientclick="javascript:unSelrdbtn()" 
                            Text="Hidden" />
    </div>
    </form>
</body>
</html>
