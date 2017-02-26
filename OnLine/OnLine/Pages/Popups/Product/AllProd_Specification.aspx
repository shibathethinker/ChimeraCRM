<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllProd_Specification.aspx.cs" Inherits="OnLine.Pages.Popups.Product.AllProd_Specification" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<link type="text/css" rel="stylesheet" href="~/css/bootstrap.css" /> 
    <title></title>
        <style type="text/css">
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
</head>
<body>
    <form id="form1" runat="server">
    <div align="center">
    <br />
        <asp:Label ID="Label_Status" runat="server" 
                        Text="Label" Visible="False"></asp:Label>
        <asp:GridView ID="GridView1" runat="server" Width="95%" 
                                    AutoGenerateColumns="False" CellPadding="4" 
                                                        CssClass="table table-striped table-bordered table-hover tableShadow" 
                                    GridLines="None" onpageindexchanging="GridView1_PageIndexChanging" 
                                    PageSize="5" 
            style="font-family: Andalus; font-size: small;" 
            onrowcancelingedit="GridView1_RowCancelingEdit" 
            onrowdatabound="GridView1_RowDataBound" onrowediting="GridView1_RowEditing" 
            onrowupdating="GridView1_RowUpdating" 
            onrowdeleting="GridView1_RowDeleting">
               <Columns>
            <asp:CommandField ShowEditButton="True" />
                   <asp:CommandField ShowDeleteButton="True" />
            <asp:TemplateField HeaderText="Hidden">
                <ItemTemplate>
                    <asp:Label ID="Label_Hidden_Feat" runat="server" Text='<%# Eval("FeatId") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Feature">
                <ItemTemplate>
                    <asp:Label ID="Label_Feature" runat="server" Text='<%# Eval("FeatName") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Spec Text">
                <ItemTemplate>
                    <asp:Label ID="Label_SpecText" runat="server" Text='<%# Eval("SpecText") %>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox class="form-control" ID="TextBox_SpecText_Edit" runat="server" 
                        Text='<%# Eval("SpecText") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="From Spec">
                <EditItemTemplate>
                    <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_From_Spec_Edit" runat="server" 
                        style="font-family: Andalus; font-size: medium">
                    </asp:DropDownList>
                    <br />
                    <asp:Label ID="Label_From_Spec" runat="server" Text='<%# Eval("FromSpec") %>' 
                        Visible="False"></asp:Label>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label_From_Spec" runat="server" Text='<%# Eval("FromSpec") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="To Spec">
                <EditItemTemplate>
                    <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_To_Spec_Edit" runat="server" 
                        style="font-family: Andalus; font-size: medium">
                    </asp:DropDownList>
                    <br />
                    <asp:Label ID="Label_ToSpec" runat="server" Text='<%# Eval("ToSpec") %>' 
                        Visible="False"></asp:Label>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label_ToSpec" runat="server" Text='<%# Eval("ToSpec") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Image">
                <ItemTemplate>
                    <asp:LinkButton ID="LinkButton_Show" runat="server" 
                                                    oncommand="Link_Feat_Img_Show_Command"  CommandArgument
                                     ="<%# Container.DataItemIndex %>" 
                        Text='<%# Eval("imgName") %>'></asp:LinkButton>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:FileUpload ID="FileUpload_Image" runat="server" 
                        style="font-family: Andalus" />
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
    </div>
    <div align="center">
    <div class="panel panel-primary" Width="95%">
        <div class="panel-heading">
            <h3 class="style3">Add More Features</h3>
            </div>
            <div class="panel-body">   
        <asp:Panel ID="Panel_Prod_Service_Det" runat="server" 
            style="font-family: Andalus" >
            <div class="style2">
                        <asp:Panel ID="Panel3" runat="server">
                            &nbsp;&nbsp;
                            <asp:Button ID="Buttin_Show_Spec_List" runat="server" class="btn btn-sm btn-success"
                                onclick="Buttin_Show_Spec_List_Click" style="font-family: Andalus" 
                                Text="Show Features and Specifications!" ValidationGroup="Panel2" />
                        </asp:Panel>
                <br />
                <asp:Label ID="Label_Extra_Spec" runat="server" 
                    Text="Specification Not Listed Here? Then Enter..." Visible="False"></asp:Label>
                &nbsp;<asp:TextBox class="form-control" ID="TextBox_Spec" runat="server"  
                    Width="50%" Visible="False" ontextchanged="TextBox_Spec_TextChanged" 
                            AutoPostBack="True" TextMode="MultiLine"></asp:TextBox>
                <br />
                <br />
                <asp:Label ID="Label_Extra_Spec_upload" runat="server" 
                    Text="and upload picture if required" Visible="False"></asp:Label>
                &nbsp;<asp:FileUpload ID="FileUpload_Extra_Spec" runat="server"  
                    ViewStateMode="Enabled" Visible="False" />
                <br />
                        <div align="center">
                            <asp:Label ID="Label_Feat_Exists" runat="server" Text="Label" Visible="False"></asp:Label>
                        </div>
                <br />
                <div align="center">
                    <asp:GridView ID="GridView2" runat="server" AllowPaging="True" Width="50%"
                    AutoGenerateColumns="False" CellPadding="4" 
                                                    CssClass="table table-striped table-bordered table-hover tableShadow" 
                    GridLines="None" onpageindexchanging="GridView2_PageIndexChanging" 
                    onrowdatabound="GridView2_RowDataBound" 
                    PageSize="3" Visible="False" 
                        onselectedindexchanged="GridView2_SelectedIndexChanged">

                    <Columns>
                        <asp:CommandField SelectText="Add To Product" ShowSelectButton="True" />
                        <asp:TemplateField HeaderText="Hidden">
                            <ItemTemplate>
                                <asp:Label ID="Label_Hidden" runat="server" 
                                    Text='<%# Eval("Hidden_Feat_Id") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Feature">
                            <ItemTemplate>
                                <asp:Label ID="Label_Feature" runat="server" Text='<%# Eval("Feature") %>'></asp:Label>
                                <asp:Image ID="Image_Selected" runat="server" Height="16px" 
                                    ImageUrl="~/Images/tick_green.jpg" Visible="False" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="From">
                            <EditItemTemplate>
                                <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Gridview1_From" runat="server" 
                                    style="font-family: Andalus">
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Gridview1_From" runat="server" 
                                    style="font-family: Andalus">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="To">
                            <EditItemTemplate>
                                <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Gridview1_To" runat="server" 
                                    style="font-family: Andalus">
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Gridview1_To" runat="server" 
                                    style="font-family: Andalus">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Upload Picture">
                            <ItemTemplate>
                                <asp:FileUpload ID="FileUpload_Spec" runat="server" Font-Names="Andalus" />
                                <br />
                                <asp:Label ID="Label_File_Name" runat="server" Visible="False"></asp:Label>
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
                    <asp:Button ID="Button_Add_To_Prod" runat="server" class="btn btn-sm btn-success"
                        onclick="Button_Add_To_Prod_Click" style="font-family: Andalus" Text="Add" 
                        ValidationGroup="Req_Validate" Enabled="False" />
                </div>
                
            </div>
        </asp:Panel>
        </div>
        </div>
       <asp:Label ID="Label_Selected_List" runat="server" Text="Label" Visible="False"></asp:Label>
       </div>

    </form>
</body>
</html>
