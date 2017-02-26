<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaggedRFQs.aspx.cs" Inherits="OnLine.Pages.Popups.Purchase.TaggedRFQs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link type="text/css" rel="stylesheet" href="~/css/bootstrap.css" /> 
    <title></title>

</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
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
        <br />
        <asp:Label ID="Label_No_Records" runat="server" ForeColor="Red" 
            Text="No Records Available" Visible="False"></asp:Label>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                    <asp:GridView ID="GridView_RFQ" 
                        runat="server" 
                    AutoGenerateColumns="False" 
                    CellPadding="4" 
                    GridLines="None" 
                                                    CssClass="table table-striped table-bordered table-hover tableShadow" 
                    PageSize="3" 
                    Visible="False" 
                        onselectedindexchanged="GridView_RFQ_SelectedIndexChanged" 
                        style="font-size: small; font-family: Andalus;" Width="95%" 
                            onrowdeleting="GridView_RFQ_RowDeleting">
            
                          <Columns>
                              <asp:TemplateField>
                                  <ItemTemplate>
                                      <asp:RadioButton ID="rfq_radio" runat="server" AutoPostBack="true" 
                                          GroupName="rain" OnCheckedChanged="GridView_RFQ_RadioSelect" 
                                          OnClick="javascript:Selrdbtn(this.id)" />
                                  </ItemTemplate>
                              </asp:TemplateField>
                              <asp:CommandField ShowDeleteButton="True" />
                              <asp:TemplateField HeaderText="RFQ#">
                                  <ItemTemplate>
                                      <asp:Label ID="Label_RFQId" runat="server" Text='<%# Eval("RFQNo") %>'></asp:Label>
                                  </ItemTemplate>
                              </asp:TemplateField>
                              <asp:TemplateField HeaderText="RFQ Name">
                                  <ItemTemplate>
                                      <asp:Label ID="Label_RFQName" runat="server" Text='<%# Eval("RFQName") %>'></asp:Label>
                                  </ItemTemplate>
                              </asp:TemplateField>
                              <asp:TemplateField HeaderText="Submit Date">
                                  <ItemTemplate>
                                      <asp:Label ID="Label_Submit_Date" runat="server" 
                                          Text='<%# Eval("Submit Date") %>'></asp:Label>
                                  </ItemTemplate>
                              </asp:TemplateField>
                              <asp:TemplateField HeaderText="Due Date">
                                  <ItemTemplate>
                                      <asp:Label ID="Label_Due_Date" runat="server" Text='<%# Eval("Due Date") %>'></asp:Label>
                                  </ItemTemplate>
                              </asp:TemplateField>
                              <asp:TemplateField HeaderText="Approval Status">
                                  <ItemTemplate>
                                      <asp:Label ID="Label_Approval_Stat" runat="server" 
                                          Text='<%# Eval("ApprovalStat") %>'></asp:Label>
                                  </ItemTemplate>
                              </asp:TemplateField>
                              <asp:TemplateField HeaderText="Purchase Order">
                                  <ItemTemplate>
                                      <asp:LinkButton ID="LinkButton_PO" runat="server" 
                                          CommandArgument="<%#Container.DataItemIndex %>" 
                                          oncommand="LinkButton_PO_Command" Text='<%# Eval("PO_No") %>' 
                                          onclientclick="javascript:unSelrdbtn()"></asp:LinkButton>
                                  </ItemTemplate>
                              </asp:TemplateField>
                              <asp:TemplateField HeaderText="Active Status">
                                  <EditItemTemplate>
                                      &nbsp;
                                  </EditItemTemplate>
                                  <ItemTemplate>
                                      <asp:Label ID="Label_Active_Stat" runat="server" 
                                          Text='<%# Eval("ActiveStatus") %>'></asp:Label>
                                  </ItemTemplate>
                              </asp:TemplateField>
                              <asp:TemplateField HeaderText="Invoice">
                                  <ItemTemplate>
                                      <asp:LinkButton ID="LinkButton_Show_Inv" runat="server" 
                                          CommandArgument="<%#Container.DataItemIndex %>" 
                                          oncommand="LinkButton_Show_Inv_Command" Text='<%# Eval("Inv_No") %>' 
                                          onclientclick="javascript:unSelrdbtn()"></asp:LinkButton>
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
                        &nbsp;&nbsp;&nbsp;
                                        </ContentTemplate>
                    </asp:UpdatePanel>
                    
    </div>
    </form>
</body>
</html>
