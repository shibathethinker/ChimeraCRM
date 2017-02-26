<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllRFQ_Quotes_Indv_Resp_Det.aspx.cs" Inherits="OnLine.Pages.Popups.Purchase.AllRFQ_Quotes_Indv_Resp_Det" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <link type="text/css" rel="stylesheet" href="~/css/bootstrap.css" /> 
    <title></title>
    <script language="javascript">

        function RefreshParent() {
            window.document.getElementById('ContentPlaceHolderBody_Button_Rfq_Quotes_Refresh_Hidden_Click_Index_Unchanged').click();
        }
    
    </script>
    <style type="text/css">

        .style1
        {
            font-family: Andalus;
        }
        .style2
        {
            text-align: right;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="style2">
    
    <div align="left">
                    <asp:Label ID="Label1" runat="server" style="font-family: Andalus" 
                        Text="Response For RFQ: "></asp:Label>    
    &nbsp;<asp:Label ID="Label_RFQ_Name" runat="server" ForeColor="#FF3300" 
                        style="font-family: Andalus"></asp:Label>
&nbsp;<asp:Label ID="Label3" runat="server" style="font-family: Andalus" 
                        Text="From Vendor:"></asp:Label>    
    &nbsp;<asp:Label ID="Label_Vendor" runat="server" ForeColor="#FF3300" 
                        style="font-family: Andalus"></asp:Label>
    </div>
                    <br />
                        
                    <asp:GridView ID="GridView1" 
                        runat="server" AllowPaging="True" 
                    AutoGenerateColumns="False" CellPadding="4" 
                    GridLines="None" 
                                                            CssClass="table table-striped table-bordered table-hover tableShadow" 
                    PageSize="5" 
                        style="font-size: small; font-family: Andalus;" 
             Width="95%"                         
            onpageindexchanging="GridView1_PageIndexChanging">         
                       
                    <Columns>
                        <asp:TemplateField HeaderText="Hidden">
                            <ItemTemplate>
                                <asp:Label ID="Label_Hidden" runat="server" Text='<%# Eval("Hidden") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Product Category">
                            <ItemTemplate>
                                <asp:Label ID="Label_Product" runat="server" 
                                    Text='<%# Eval("CategoryName") %>' style="font-family: Andalus"></asp:Label>
                                <br />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Feature">
                            <ItemTemplate>
                                <asp:GridView ID="GridView1_Inner" runat="server" AllowPaging="True" 
                                    AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" 
                                    BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" 
                                    GridLines="Horizontal" onpageindexchanging="GridView1_Inner_PageIndexChanging" 
                                    PageSize="1">
                                    <Columns>
                                        <asp:CommandField ShowSelectButton="True" />
                                        <asp:TemplateField HeaderText="Hidden">
                                            <ItemTemplate>
                                                <asp:Label ID="Label_Hidden" runat="server" Text='<%# Eval("Hidden") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox class="form-control" ID="TextBox7" runat="server"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Feature">
                                            <ItemTemplate>
                                                <asp:Label ID="Label_Feature" runat="server" Text='<%# Eval("FeatName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox class="form-control" ID="TextBox2" runat="server"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Spec Text">
                                            <ItemTemplate>
                                                <asp:Label ID="Label_SpecText" runat="server" Text='<%# Eval("SpecText") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox class="form-control" ID="TextBox3" runat="server"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="From Spec">
                                            <ItemTemplate>
                                                <asp:Label ID="Label_From_Spec" runat="server" Text='<%# Eval("FromSpec") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox class="form-control" ID="TextBox4" runat="server"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="To Spec">
                                            <ItemTemplate>
                                                <asp:Label ID="Label_ToSpec" runat="server" Text='<%# Eval("ToSpec") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox class="form-control" ID="TextBox5" runat="server"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Image">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton_Show" runat="server" 
                                                    oncommand="Link_Feat_Img_Show_Command"  CommandArgument
                                     ="<%#Container.DataItemIndex %>">Show!</asp:LinkButton>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox class="form-control" ID="TextBox6" runat="server"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                    <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                                    <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                    <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                    <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                    <SortedDescendingHeaderStyle BackColor="#242121" />
                                </asp:GridView>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="From Quantity">
                            <ItemTemplate>
                                <asp:Label ID="Label_From_Qnty" runat="server" style="font-family: Andalus" 
                                    Text='<%# Eval("FromQnty") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="To Quantity">
                            <ItemTemplate>
                                <asp:Label ID="Label_To_Qnty" runat="server" Text='<%# Eval("ToQnty") %>' 
                                    style="font-family: Andalus"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="From Price">
                            <ItemTemplate>
                                <asp:Label ID="Label_From_Price" runat="server" style="font-family: Andalus" 
                                    Text='<%# Eval("FromPrice") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="To Price">
                            <ItemTemplate>
                                <asp:Label ID="Label_To_Price" runat="server" 
                                    Text='<%# Eval("ToPrice") %>' style="font-family: Andalus"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Measurement Unit">
                            <ItemTemplate>
                                <asp:Label ID="Label_Msrmnt" runat="server" style="font-family: Andalus" 
                                    Text='<%# Eval("msrmntUnit") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Quote">
                            <EditItemTemplate>
                                <asp:TextBox class="form-control" ID="TextBox_Quote" runat="server" style="font-family: Andalus" 
                                    Text='<%# Eval("Quote") %>'></asp:TextBox>
                                &nbsp;<br />
                                <asp:Label ID="Label_Quote_Prev" runat="server" Text='<%# Eval("Quote") %>' 
                                    Visible="False"></asp:Label>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label_Quote" runat="server" Text='<%# Eval("Quote") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total">
                            <ItemTemplate>
                                <asp:Label ID="Label_Total" runat="server" Text='<%# Eval("Total") %>'></asp:Label>
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
                    <asp:Label ID="Label_Total_Text" runat="server" Text="Total AMount:" 
                            ></asp:Label>
                    <asp:Label ID="Label_Total" runat="server" ></asp:Label>
    
        <br />
        <br /> 
    </div>
    <div align="center">
            <asp:Button ID="Button_ShortList" runat="server" style="font-family: Andalus" class="btn btn-sm btn-success"
            Text="Shortlist For This RFQ!" Enabled="False" 
                onclick="Button_ShortList_Click" 
                ToolTip="Once shortlisted this entry will show up in our RFQ's shortlisted entries and also the vendor will see this in his Potential List" />
    &nbsp;<asp:Label ID="Label_ShortList_Stat" runat="server" style="font-family: Andalus" 
                Visible="False"></asp:Label>
    </div>
    </form>
</body>
</html>
