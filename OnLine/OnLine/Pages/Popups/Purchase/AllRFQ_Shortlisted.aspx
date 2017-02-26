<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllRFQ_Shortlisted.aspx.cs" Inherits="OnLine.Pages.Popups.Purchase.AllRFQ_Shortlisted" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <link type="text/css" rel="stylesheet" href="~/css/bootstrap.css" /> 
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
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
    <div align="center">
    <br />
                    <asp:GridView ID="GridView_RFQ_Resp_Quotes" Width="95%" 
                        runat="server" AllowPaging="True" 
                    AutoGenerateColumns="False" CellPadding="4" 
                    GridLines="None"
                                                            CssClass="table table-striped table-bordered table-hover tableShadow" 
                     onpageindexchanging="GridView_RFQ_Resp_Quotes_PageIndexChanging" 
                    PageSize="3" 
                        onselectedindexchanged="GridView_RFQ_Resp_Quotes_SelectedIndexChanged" 
                        style="font-size: small; font-family: Andalus;" >                    

                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:RadioButton ID="rfq_radio" runat="server" AutoPostBack="true" 
                                    GroupName="rain" OnCheckedChanged="GridView_RFQ_Shortlisted_RadioSelect" 
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
                                &nbsp;<asp:Image ID="Image_Selected" runat="server" Height="16px" 
                                    ImageUrl="~/Images/tick_green.jpg" Visible="False" />
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
                                <asp:LinkButton ID="LinkButton_All_Shortlisted" runat="server">Show!</asp:LinkButton>
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
                        <asp:TemplateField HeaderText="Total Amount">
                            <ItemTemplate>
                                <asp:Label ID="Label_Total" runat="server" Text='<%# Eval("TotalAmnt") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox class="form-control" ID="TextBox5" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Request For Deal From Vendor?">
                            <ItemTemplate>
                                <asp:Label ID="Label_Deal_Request" runat="server" Text='<%# Eval("DealReq") %>'></asp:Label>
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
    
        <br />
        <asp:Button ID="Button_Finalz_Deal" runat="server" class="btn btn-sm btn-success"
                        style="font-family: Andalus" Text="Finalize Deal!" 
                        onclick="Button_Finalz_Deal_Click" />
    
    &nbsp;<br />
                    <br />
                    <asp:Label ID="Label_Finalize_Stat" runat="server" style="font-family: Andalus" 
                        Visible="False"></asp:Label>
    
    </div>
    </form>
</body>
</html>
