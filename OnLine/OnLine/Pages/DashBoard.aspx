<%@ Page Title="" Language="C#" Theme="ThemeBlue" MasterPageFile="~/Pages/Site1.Master" AutoEventWireup="true" CodeBehind="DashBoard.aspx.cs" Inherits="OnLine.Pages.DashBoard" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>    
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">

    $(function () {
        $('#demo > a:first').fancyzoom({ Speed: 400, showoverlay: false });
        $('#demo > a:last').fancyzoom({ Speed: 400, showoverlay: false });
        $('#nooverlay').fancyzoom({ Speed: 400, showoverlay: false });
        $('img.fancyzoom').fancyzoom();
    });
</script>

    <style type="text/css">
    legend {color:#00488F}

        .style9
        {
            width: 100%;
            border-collapse: collapse;
            border-bottom-color: "#0066CC";
            font-weight: bold;
            border-left-width: thin;
            border-right-width: thin;
            border-top-width: thin;
            border-bottom-width: thin;
        }
        .style10
        {
            font-weight: normal;
        }
        .style11
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
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderBody" runat="server">


    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
                        <asp:Label ID="Label_Dashboard_Screen_Access" 
        runat="server" ForeColor="Red" 
                            style="font-size: small; font-family: Andalus;" 
        Visible="False"></asp:Label>
             <asp:Label ID="Label_Lead_Dashboard_Access" runat="server" ForeColor="Red" 
                 style="font-size: small" Visible="False"></asp:Label>
                 <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                 <ContentTemplate>
                                                          <table width="100%">
                                                          <tr>
                                                                <td background="../Images/menu_bg.gif" >
                                                                <asp:LinkButton ID="LinkButton_Lead" runat="server" 
                                                                        ToolTip="Click to expand or collapse">Lead Dashboard</asp:LinkButton>      
                                                                    <div style="float: right; vertical-align: middle;">
                                                                    <asp:Image ID="Pend_Appr_Collapse_Img" runat="server"/>
                                                                    </div>                                                          
                                                                    </td>
                                                            </tr>
                                                            </table>
    <asp:Panel ID="Panel_Lead_Dashboard" runat="server" BorderColor="#0066CC" 
            BorderStyle="Ridge" BorderWidth="2px" 
            style="font-family: Andalus">  
            <asp:UpdateProgress ID="UpdateProgress1" 
                     AssociatedUpdatePanelID="UpdatePanel11" runat="server" DisplayAfter="1">
                 <ProgressTemplate>
                 <div style="position:fixed;top:40px;left:35%;z-index:100;width:50%;height:50%">
                 <image style="z-index:105" src="../Images/loading.gif"></image>                 
                 </div>
                 </ProgressTemplate>
                 </asp:UpdateProgress>
             <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Conditional" OnLoad="UpdatePanel11_load">
             <ContentTemplate>
             <table id="LeadTable" border="0" cellpadding="0" cellspacing="0" width="90%" >
                     <tr>
                         <td align="center" width="45%">    
                         <br />         
                                      <asp:Chart ID="Chart_Lead_Conv_By_Val" runat="server" BorderlineColor="Tan" 
                                         BorderlineDashStyle="Solid" Height="400px" ViewStateContent="All" Width="400px" 
                                         ViewStateMode="Enabled" >
                                         <Series>
                                             <asp:Series ChartType="Pie" Font="Baskerville Old Face, 9pt" Legend="Legend1" 
                                                 Name="Series1" ChartArea="ChartArea1">
                                             </asp:Series>
                                         </Series>
                                         <ChartAreas>
                                             <asp:ChartArea Name="ChartArea1">
                                                 <Area3DStyle Enable3D="True" />
                                             </asp:ChartArea>
                                         </ChartAreas>
                                         <Legends>
                                             <asp:Legend Font="Baskerville Old Face, 9pt" IsTextAutoFit="False" 
                                                 Name="Legend1">
                                             </asp:Legend>
                                             <asp:Legend Font="Baskerville Old Face, 9pt" IsTextAutoFit="False" 
                                                 Name="Legend2">
                                             </asp:Legend>
                                         </Legends>
                                         <Titles>
                                             <asp:Title Font="Book Antiqua, 9.75pt" Name="Lead Conversion % by Lead Amount">
                                             </asp:Title>
                                         </Titles>
                                     </asp:Chart>                                
                                      
                             <br />
                             <span class="style10">
                                     Filter By Submit Date (default range is last 12 months):<br />
                             <span class="style6">From Date:</span></span>
                             <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_From_Date_Lead_Val" runat="server" 
                                 style="font-family: Andalus" ValidationGroup="LeadValDateRange" 
                                 ></asp:TextBox>
                             <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_From_Date_Lead_Val_CalendarExtender" 
                                 runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton3" 
                                 TargetControlID="TextBox_From_Date_Lead_Val">
                             </ajaxtoolkit:CalendarExtender>
                             <span class="style10">
                             <span class="style6">To Date:</span></span><asp:TextBox 
                                 ID="TextBox_To_Date_Lead_Val" runat="server" class="form-control datepicker-textbox" 
                                 style="font-family: Andalus" ValidationGroup="LeadValDateRange" ></asp:TextBox>
                             <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_To_Date_Lead_Val_CalendarExtender" 
                                 runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton4" 
                                 TargetControlID="TextBox_To_Date_Lead_Val">
                             </ajaxtoolkit:CalendarExtender>
                             &nbsp;<asp:Button ID="Button_Lead_Conv_Val_Show" runat="server" class="btn btn-sm btn-success"
                                 onclick="Button_Lead_Conv_Val_Show_Click" 
                                 style="font-family: Andalus; font-size: small;" Text="Show!" 
                                 ValidationGroup="LeadValDateRange" />
                             <span class="style10">
                                     &nbsp;<asp:Button ID="Button_Lead_Conv_Val_Download" runat="server" class="btn btn-sm btn-success"
                                         onclick="Button_Lead_Conv_Val_Download_Click" 
                                         style="font-family: Andalus; font-size: small;" Text="Report" 
                                         ValidationGroup="LeadValDateRange" />
                                     <asp:CompareValidator ID="CompareValidator3" runat="server" 
                                         ControlToCompare="TextBox_From_Date_Lead_Val" 
                                         ControlToValidate="TextBox_To_Date_Lead_Val" Display="Dynamic" 
                                         ErrorMessage="To Date earlier than from date" ForeColor="Red" 
                                         Operator="GreaterThan" ValidationGroup="LeadValDateRange"></asp:CompareValidator>
                                     &nbsp;</span><asp:Label ID="Label_Message_Lead_Val" runat="server" Text="Label" 
                                         Visible="False"></asp:Label>
                         </td>
                         <td align="center" width="45%">
                                     <asp:Chart ID="Chart_Lead_Conv_By_Number" runat="server" BorderlineColor="Tan" 
                                         BorderlineDashStyle="Solid" Height="400px" ViewStateContent="All" 
                                         ViewStateMode="Enabled" Width="400px">
                                         <Series>
                                             <asp:Series ChartType="Pie" Font="Baskerville Old Face, 9pt" Legend="Legend1" 
                                                 Name="Series1">
                                             </asp:Series>
                                         </Series>
                                         <ChartAreas>
                                             <asp:ChartArea Name="ChartArea1">
                                                 <Area3DStyle Enable3D="True" />
                                             </asp:ChartArea>
                                         </ChartAreas>
                                         <Legends>
                                             <asp:Legend Font="Baskerville Old Face, 9pt" IsTextAutoFit="False" 
                                                 Name="Legend1">
                                             </asp:Legend>
                                             <asp:Legend Font="Baskerville Old Face, 9pt" IsTextAutoFit="False" 
                                                 Name="Legend2">
                                             </asp:Legend>
                                         </Legends>
                                         <Titles>
                                             <asp:Title Font="Book Antiqua, 9.75pt" Name="Title1">
                                             </asp:Title>
                                         </Titles>
                                     </asp:Chart>
                             <span class="style9">
                             <br />
                             </span><span class="style11"><span class="style10">Filter By Submit Date (default range 
                                     is last 12 months):</span>&nbsp;
                                     <br />
                                     From Date:</span>
                             <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_From_Date_Lead_Amnt" runat="server" 
                                  style="font-family: Andalus" ValidationGroup="LeadAmntDateRange" 
                                 ></asp:TextBox>
                             <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_From_Date_Lead_Amnt_CalendarExtender" 
                                 runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton5" 
                                 TargetControlID="TextBox_From_Date_Lead_Amnt">
                             </ajaxtoolkit:CalendarExtender>
                             <span class="style11">To </span><span class="style11">Date:</span><asp:TextBox 
                                 ID="TextBox_To_Date_Lead_Amnt" runat="server" class="form-control datepicker-textbox"
                                 style="font-family: Andalus" ValidationGroup="LeadAmntDateRange" 
                                         ReadOnly="True"></asp:TextBox>
                             <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_To_Date_Lead_Amnt_CalendarExtender" 
                                 runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton6" 
                                 TargetControlID="TextBox_To_Date_Lead_Amnt">
                             </ajaxtoolkit:CalendarExtender>
                             &nbsp;<asp:Button ID="Button_Lead_Conv_Val_Show0" runat="server"  class="btn btn-sm btn-success"
                                 onclick="Button_Lead_Conv_Val_Show0_Click" style="font-family: Andalus; " 
                                 Text="Show!" ValidationGroup="LeadAmntDateRange" />
                                     &nbsp;<span class="style10"><asp:Button ID="Button_Lead_Conv_No_Download" class="btn btn-sm btn-success"
                                         runat="server" onclick="Button_Lead_Conv_No_Download_Click" 
                                         style="font-family: Andalus; font-size: small;" Text="Report" 
                                         ValidationGroup="LeadValDateRange" />
                                     </span>
                             <asp:CompareValidator ID="CompareValidator2" runat="server" 
                                 ControlToCompare="TextBox_From_Date_Lead_Amnt" 
                                 ControlToValidate="TextBox_To_Date_Lead_Amnt" 
                                 Display="Dynamic" ErrorMessage="To Date earlier than from date" ForeColor="Red" 
                                 Operator="GreaterThan" ValidationGroup="LeadAmntDateRange"></asp:CompareValidator>
                         </td>
                     </tr>
            </table>        
             </ContentTemplate>
             </asp:UpdatePanel>    
            <br />        
    </asp:Panel>
    <ajaxtoolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender_Lead" runat="server" Collapsed="false" AutoCollapse="false" TargetControlID="Panel_Lead_Dashboard"
                        ImageControlId="Pend_Appr_Collapse_Img"  CollapsedImage="~/Images/ArrowTip_Down.png" ExpandedImage="~/Images/ArrowTip_Parallel.png"  ExpandControlID="LinkButton_Lead"
             CollapseControlID="LinkButton_Lead"/>
         </ContentTemplate>
                 </asp:UpdatePanel>

                        <asp:Label ID="Label_Potn_Dashboard_Access" runat="server" ForeColor="Red" 
                            style="font-size: small" Visible="False"></asp:Label>
    
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
    <ContentTemplate>

    <asp:UpdateProgress ID="UpdateProgressPotential" AssociatedUpdatePanelID="UpdatePanelPotential" DisplayAfter="1"  runat="server">
                 <ProgressTemplate>
                 <div style="position:fixed;top:40px;left:35%;z-index:100;width:50%;height:50%">
                 <image style="z-index:105" src="../Images/loading.gif"></image>                 
                 </div>
                 </ProgressTemplate>
    </asp:UpdateProgress>
    <br />
                                                            <table width="100%">
                                                          <tr>
                                                                <td  background="../Images/menu_bg.gif">
                                                                <asp:LinkButton ID="LinkButton_Pot" runat="server" 
                                                                        ToolTip="Click to expand or collapse">Potential Dashboard</asp:LinkButton>
                                                                    <div style="float: right; vertical-align: middle;">
                                                                    <asp:Image ID="Pend_Appr_Collapse_Img1" runat="server"/>
                                                                    </div>   
                                                                    </td>
                                                            </tr>
                                                            </table>
                          
    <asp:Panel ID="Panel_Pot_Dashboard" runat="server" BorderColor="#0066CC" 
            BorderStyle="Ridge" BorderWidth="2px" 
            style="font-family: Andalus" CssClass="cpBody"> 
        <asp:UpdatePanel ID="UpdatePanelPotential" runat="server" UpdateMode="Conditional" OnLoad="UpdatePanelPotential_load">
        <ContentTemplate>
             <table id="potTable" border="0" cellpadding="0" cellspacing="0" width="90%">
                     <tr>
                         <td align="center" width="45%">
                         <br />
                                     <asp:Chart ID="Chart_Potn_Conv_By_Val" runat="server" BorderlineColor="Tan" 
                                         BorderlineDashStyle="Solid" Height="400px" ViewStateContent="All" 
                                         ViewStateMode="Enabled" Width="400px">
                                         <Series>
                                             <asp:Series ChartType="Pie" Font="Baskerville Old Face, 9pt" Legend="Legend1" 
                                                 Name="Series1">
                                             </asp:Series>
                                         </Series>
                                         <ChartAreas>
                                             <asp:ChartArea Name="ChartArea1">
                                                 <Area3DStyle Enable3D="True" />
                                             </asp:ChartArea>
                                         </ChartAreas>
                                         <Legends>
                                             <asp:Legend Font="Baskerville Old Face, 9pt" IsTextAutoFit="False" 
                                                 Name="Legend1">
                                             </asp:Legend>
                                             <asp:Legend Font="Baskerville Old Face, 9pt" IsTextAutoFit="False" 
                                                 Name="Legend2">
                                             </asp:Legend>
                                         </Legends>
                                         <Titles>
                                             <asp:Title Font="Book Antiqua, 9.75pt" Name="Lead Conversion % by Lead Amount">
                                             </asp:Title>
                                         </Titles>
                                     </asp:Chart>
                             <br />
                             <span class="style10">
                             <br />
                             <span class="style6">Filter By Create Date (default range is last 12 months):
                                     <br />
                                     From Date:</span></span>
                             <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_From_Date_Potn_Val" runat="server" 
                                 style="font-family: Andalus" ValidationGroup="PotnValDateRange" 
                                 ></asp:TextBox>
                             <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_From_Date_Potn_Val_CalendarExtender" 
                                 runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton7" 
                                 TargetControlID="TextBox_From_Date_Potn_Val">
                             </ajaxtoolkit:CalendarExtender>
                             <span class="style10">
                             <span class="style6">To Date:</span></span><asp:TextBox 
                                 ID="TextBox_To_Date_Potn_Val" runat="server" class="form-control datepicker-textbox" 
                                 style="font-family: Andalus" ValidationGroup="PotnValDateRange" 
                                 ></asp:TextBox>
                             <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_To_Date_Potn_Val_CalendarExtender" 
                                 runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton8" 
                                 TargetControlID="TextBox_To_Date_Potn_Val">
                             </ajaxtoolkit:CalendarExtender>
                             &nbsp;<asp:Button ID="Button_Potn_Conv_Val_Show" runat="server" class="btn btn-sm btn-success"
                                 onclick="Button_Potn_Conv_Val_Show_Click" 
                                 style="font-family: Andalus; font-size: small;" Text="Show!" 
                                 ValidationGroup="PotnValDateRange" />
                             <span class="style10">
                             &nbsp;<asp:Button ID="Button_Potn_Conv_Val_Download" runat="server" class="btn btn-sm btn-success"
                                 onclick="Button_Potn_Conv_Val_Download_Click" 
                                 style="font-family: Andalus; font-size: small;" Text="Report" 
                                 ValidationGroup="PotnValDateRange" />
                             <asp:CompareValidator ID="CompareValidator4" runat="server" 
                                 ControlToCompare="TextBox_From_Date_Potn_Val" 
                                 ControlToValidate="TextBox_To_Date_Potn_Val" Display="Dynamic" 
                                 ErrorMessage="To Date earlier than from date" ForeColor="Red" 
                                 Operator="GreaterThan" ValidationGroup="PotnValDateRange"></asp:CompareValidator>
                             </span>
                         </td>
                         <td align="center" width="45%">
                         <br />
                                     <asp:Chart ID="Chart_Potn_Conv_By_No" runat="server" BorderlineColor="Tan" 
                                         BorderlineDashStyle="Solid" Height="400px" ViewStateContent="All" 
                                         ViewStateMode="Enabled" Width="400px">
                                         <Series>
                                             <asp:Series ChartType="Pie" Font="Baskerville Old Face, 9pt" Legend="Legend1" 
                                                 Name="Series1">
                                             </asp:Series>
                                         </Series>
                                         <ChartAreas>
                                             <asp:ChartArea Name="ChartArea1">
                                                 <Area3DStyle Enable3D="True" />
                                             </asp:ChartArea>
                                         </ChartAreas>
                                         <Legends>
                                             <asp:Legend Font="Baskerville Old Face, 9pt" IsTextAutoFit="False" 
                                                 Name="Legend1">
                                             </asp:Legend>
                                             <asp:Legend Font="Baskerville Old Face, 9pt" IsTextAutoFit="False" 
                                                 Name="Legend2">
                                             </asp:Legend>
                                         </Legends>
                                         <Titles>
                                             <asp:Title Font="Book Antiqua, 9.75pt" Name="Title1">
                                             </asp:Title>
                                         </Titles>
                                     </asp:Chart>
                             <span class="style9">
                             <br />
                             <br />
                             </span><span class="style11"><span class="style6"><span class="style10">Filter By Create 
                                     Date (default range is last 12 months):<br /> </span></span>&nbsp;From Date:</span>
                             <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_From_Date_Potn_No" runat="server" 
                                 style="font-family: Andalus" ValidationGroup="PotnNoDateRange" 
                                 ></asp:TextBox>
                             <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_From_Date_Potn_No_CalendarExtender" 
                                 runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton9" 
                                 TargetControlID="TextBox_From_Date_Potn_No">
                             </ajaxtoolkit:CalendarExtender>
                             <span class="style11">To </span><span class="style11">Date:</span><asp:TextBox 
                                 ID="TextBox_To_Date_Potn_No" runat="server"  class="form-control datepicker-textbox" 
                                 style="font-family: Andalus" ValidationGroup="PotnNoDateRange" 
                                 ></asp:TextBox>
                             <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_To_Date_Potn_No_CalendarExtender" 
                                 runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton10" 
                                 TargetControlID="TextBox_To_Date_Potn_No">
                             </ajaxtoolkit:CalendarExtender>
                             &nbsp;<asp:Button ID="Button_Potn_Conv_No_Show" runat="server" class="btn btn-sm btn-success"
                                 onclick="Button_Potn_Conv_No_Show_Click" style="font-family: Andalus; " 
                                 Text="Show!" ValidationGroup="PotnNoDateRange" />
                             &nbsp;<span class="style10"><asp:Button ID="Button_Potn_Conv_No_Download" class="btn btn-sm btn-success"
                                 runat="server" onclick="Button_Potn_Conv_No_Download_Click" 
                                 style="font-family: Andalus; font-size: small;" Text="Report" 
                                 ValidationGroup="PotnValDateRange" />
                             </span>
                             <asp:CompareValidator ID="CompareValidator5" runat="server" 
                                 ControlToCompare="TextBox_From_Date_Potn_No" 
                                 ControlToValidate="TextBox_To_Date_Potn_No" 
                                 Display="Dynamic" ErrorMessage="To Date earlier than from date" ForeColor="Red" 
                                 Operator="GreaterThan" ValidationGroup="PotnNoDateRange"></asp:CompareValidator>
                         </td>
                     </tr>
                     <tr>
                     <td align="center" width="45%">
                         <br />
                         <asp:Chart ID="Chart_Pot_By_Stage" runat="server" BorderlineColor="Tan" Height="400px" 
                             Width="400px" BorderlineDashStyle="Solid">
                             <Series>
                                 <asp:Series Name="Series1" BackGradientStyle="TopBottom" 
                                     CustomProperties="DrawingStyle=Cylinder, LabelStyle=Center" 
                                     Font="Book Antiqua, 8.25pt">
                                 </asp:Series>
                             </Series>
                             <ChartAreas>
                                 <asp:ChartArea Name="ChartArea1" BackColor="LightSteelBlue" 
                                     BackGradientStyle="Center" BorderColor="Gainsboro">
                                     <AxisY TitleFont="Baskerville Old Face, 9.75pt" IsLabelAutoFit="False">
                                         <LabelStyle Font="Book Antiqua, 9.75pt" />
                                     </AxisY>
                                     <AxisX TitleFont="Baskerville Old Face, 9.75pt" IsLabelAutoFit="False">
                                         <LabelStyle Font="Book Antiqua, 9.75pt" />
                                     </AxisX>
                                     <Area3DStyle Enable3D="True" Inclination="0" PointDepth="30" 
                                         PointGapDepth="30" />
                                 </asp:ChartArea>
                             </ChartAreas>
                             <Titles>
                                 <asp:Title Font="Book Antiqua, 8.25pt" Name="Title1">
                                 </asp:Title>
                             </Titles>
                         </asp:Chart>
                         <br />
                         <br />
                         <span class="style10"><span class="style6">Filter By Create Date (default range 
                         is last 12 months):<br /> From Date:</span></span>
                         <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_From_Date_Potn_Stage" runat="server" 
                              style="font-family: Andalus" ValidationGroup="PotnStageGroup" 
                             ></asp:TextBox>
                         <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_From_Date_Potn_Stage_CalendarExtender" 
                             runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton11" 
                             TargetControlID="TextBox_From_Date_Potn_Stage">
                         </ajaxtoolkit:CalendarExtender>
                         <span class="style10">
                         <span class="style6">To Date:</span></span><asp:TextBox 
                             ID="TextBox_To_Date_Potn_Stage" runat="server" class="form-control datepicker-textbox"
                             style="font-family: Andalus" ValidationGroup="PotnStageGroup" 
                             ></asp:TextBox>
                         <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_To_Date_Potn_Stage_CalendarExtender" 
                             runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton12" 
                             TargetControlID="TextBox_To_Date_Potn_Stage">
                         </ajaxtoolkit:CalendarExtender>
                         &nbsp;<asp:Button ID="Button_Potn_Stage" runat="server" class="btn btn-sm btn-success"
                             onclick="Button_Potn_Stage_Click" 
                             style="font-family: Andalus; font-size: small;" Text="Show!" 
                             ValidationGroup="PotnStageGroup" />
                         <span class="style10">
                         <asp:CompareValidator ID="CompareValidator6" runat="server" 
                             ControlToCompare="TextBox_From_Date_Potn_Stage" 
                             ControlToValidate="TextBox_To_Date_Potn_Stage" Display="Dynamic" 
                             ErrorMessage="To Date earlier than from date" ForeColor="Red" 
                             Operator="GreaterThan" ValidationGroup="PotnStageGroup"></asp:CompareValidator>
                         </span>
                     </td>
                     <td align="center" width="45%">
                     <br />
                         <asp:Chart ID="Chart_Potn_By_Product" runat="server" BorderlineColor="Tan" 
                             BorderlineDashStyle="Solid" Height="400px" ViewStateContent="All" 
                             ViewStateMode="Enabled" Width="400px">
                             <Series>
                                 <asp:Series ChartType="Pie" Font="Baskerville Old Face, 9pt" Legend="Legend1" 
                                     Name="Series1">
                                 </asp:Series>
                             </Series>
                             <ChartAreas>
                                 <asp:ChartArea Name="ChartArea1">
                                     <Area3DStyle Enable3D="True" />
                                 </asp:ChartArea>
                             </ChartAreas>
                             <Legends>
                                 <asp:Legend Font="Baskerville Old Face, 9pt" IsTextAutoFit="False" 
                                     Name="Legend1">
                                 </asp:Legend>
                                 <asp:Legend Font="Baskerville Old Face, 9pt" IsTextAutoFit="False" 
                                     Name="Legend2">
                                 </asp:Legend>
                             </Legends>
                             <Titles>
                                 <asp:Title Font="Book Antiqua, 9.75pt" Name="Title1">
                                 </asp:Title>
                             </Titles>
                         </asp:Chart>
                         <br />
                         <br />
                         <span class="style10"><span class="style6">Filter By Create Date (default range 
                         is last 12 months):<br /> From Date:</span></span>
                         <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_From_Date_Potn_by_Cat" runat="server" 
                              style="font-family: Andalus" 
                             ValidationGroup="PotnByCatGroup" ></asp:TextBox>
                         <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_From_Date_Potn_by_Cat_CalendarExtender" 
                             runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton13" 
                             TargetControlID="TextBox_From_Date_Potn_by_Cat">
                         </ajaxtoolkit:CalendarExtender>
                         <span class="style10">
                         <span class="style6">To Date:</span></span><asp:TextBox 
                             ID="TextBox_To_Date_Potn_By_Cat" runat="server" class="form-control datepicker-textbox"
                             style="font-family: Andalus" ValidationGroup="PotnByCatGroup" 
                             ></asp:TextBox>
                         <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_To_Date_Potn_By_Cat_CalendarExtender" 
                             runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton14" 
                             TargetControlID="TextBox_To_Date_Potn_By_Cat">
                         </ajaxtoolkit:CalendarExtender>
                         &nbsp;<asp:Button ID="Button_Potn_By_Cat" runat="server" class="btn btn-sm btn-success"
                             onclick="Button_Potn_By_Cat_Click" 
                             style="font-family: Andalus; font-size: small;" Text="Show!" 
                             ValidationGroup="PotnByCatGroup" />
                         <span class="style10">
                         &nbsp;<asp:Button ID="Button_Potn_By_Cat_Download" runat="server" class="btn btn-sm btn-success"
                             onclick="Button_Potn_By_Cat_Download_Click" 
                             style="font-family: Andalus; font-size: small;" Text="Report" 
                             ValidationGroup="PotnValDateRange" />
                         <asp:CompareValidator ID="CompareValidator7" runat="server" 
                             ControlToCompare="TextBox_From_Date_Potn_Stage" 
                             ControlToValidate="TextBox_To_Date_Potn_Stage" Display="Dynamic" 
                             ErrorMessage="To Date earlier than from date" ForeColor="Red" 
                             Operator="GreaterThan" ValidationGroup="PotnByCatGroup"></asp:CompareValidator>
                         </span>
                     </td>
                     </tr>
            </table>        
                    </ContentTemplate>
        </asp:UpdatePanel>       
            <br />        
    </asp:Panel>
    <ajaxtoolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender_Pot" runat="server" Collapsed="true" AutoCollapse="false" TargetControlID="Panel_Pot_Dashboard" 
                        ImageControlId="Pend_Appr_Collapse_Img1"  CollapsedImage="~/Images/ArrowTip_Down.png" ExpandedImage="~/Images/ArrowTip_Parallel.png" ExpandControlID="LinkButton_Pot" 
             CollapseControlID="LinkButton_Pot"/>

                 </ContentTemplate>
    </asp:UpdatePanel>
                        <asp:Label ID="Label_Tran_Sales_Dashboard_Access" 
        runat="server" ForeColor="Red" 
                            style="font-size: small" Visible="False"></asp:Label>
    
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
    <ContentTemplate>
    <br />
                                                            <table width="100%">
                                                          <tr>
                                                                <td background="../Images/menu_bg.gif" >
                                                                <asp:LinkButton ID="LinkButton_Tran_Sales" runat="server" 
                                                                        ToolTip="Click to expand or collapse">Transaction Dashboard-Sales</asp:LinkButton>
                                                                    <div style="float: right; vertical-align: middle;">
                                                                    <asp:Image ID="Pend_Appr_Collapse_Img2" runat="server"/>
                                                                    </div>   
                                                                    </td>
                                                            </tr>
                                                            </table>
                 <asp:UpdateProgress ID="UpdateProgress_Tran_Sales" 
                     AssociatedUpdatePanelID="UpdatePanelTranSales" runat="server" DisplayAfter="1">
                 <ProgressTemplate>
                 <div style="position:fixed;top:40px;left:35%;z-index:100;width:50%;height:50%">
                 <image style="z-index:105" src="../Images/loading.gif"></image>                 
                 </div>
                 </ProgressTemplate>
                 </asp:UpdateProgress>
    <asp:Panel ID="Panel_Tran_Sales_Dashboard" runat="server" BorderColor="#0066CC" 
            BorderStyle="Ridge" BorderWidth="2px" 
            style="font-family: Andalus" CssClass="cpBody">        
                                         <asp:UpdatePanel ID="UpdatePanelTranSales" runat="server" UpdateMode="Conditional" OnLoad="UpdatePanelTranSales_load">
                                 <ContentTemplate>
             <table id="potTable0" border="0" cellpadding="0" cellspacing="0" width="90%">
                     <tr>
                         <td align="center" width="45%" >
                                     <asp:Chart ID="Chart_Transaction_Sales_Prod_Wise_Qnty" runat="server" 
                                         BorderlineColor="Tan" BorderlineDashStyle="Solid" Height="400px" 
                                         ViewStateContent="All" ViewStateMode="Enabled" Width="400px">
                                         <Series>
                                             <asp:Series ChartType="Pie" Font="Baskerville Old Face, 9pt" Legend="Legend1" 
                                                 Name="Series1">
                                             </asp:Series>
                                         </Series>
                                         <ChartAreas>
                                             <asp:ChartArea Name="ChartArea1">
                                                 <Area3DStyle Enable3D="True" />
                                             </asp:ChartArea>
                                         </ChartAreas>
                                         <Legends>
                                             <asp:Legend Font="Baskerville Old Face, 9pt" IsTextAutoFit="False" 
                                                 Name="Legend1">
                                             </asp:Legend>
                                             <asp:Legend Font="Baskerville Old Face, 9pt" IsTextAutoFit="False" 
                                                 Name="Legend2">
                                             </asp:Legend>
                                         </Legends>
                                         <Titles>
                                             <asp:Title Font="Book Antiqua, 9.75pt" Name="Title1">
                                             </asp:Title>
                                         </Titles>
                                     </asp:Chart>
                             <br />
                             <span class="style10">
                             <span class="style6">Filter By Invoice Date (default range is last 12 months):<br /> 
                                     From Date:</span>
                             <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_From_Date_Prod_Wise_Sales_Qnty" runat="server" 
                                  style="font-family: Andalus" 
                                 ValidationGroup="Prod_Wise_Sales_Qnty_Group" ></asp:TextBox>
                             <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_From_Date_Prod_Wise_Sales_Qnty_CalendarExtender" 
                                 runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton21" 
                                 TargetControlID="TextBox_From_Date_Prod_Wise_Sales_Qnty">
                             </ajaxtoolkit:CalendarExtender>
                             <span class="style6">To Date:</span><asp:TextBox 
                                 ID="TextBox_To_Date_Prod_Wise_Sales_Qnty" runat="server" class="form-control datepicker-textbox"
                                  style="font-family: Andalus" ValidationGroup="Prod_Wise_Sales_Qnty_Group" 
                                 ></asp:TextBox>
                             <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_To_Date_Prod_Wise_Sales_Qnty_CalendarExtender" 
                                 runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton22" 
                                 TargetControlID="TextBox_To_Date_Prod_Wise_Sales_Qnty">
                             </ajaxtoolkit:CalendarExtender>
                             &nbsp;<asp:Button ID="Button_Prod_Wise_Sales_Qnty" runat="server" class="btn btn-sm btn-success"
                                 style="font-family: Andalus; font-size: small;" Text="Show!" 
                                 ValidationGroup="Prod_Wise_Sales_Qnty_Group" 
                                 onclick="Button_Prod_Wise_Sales_Qnty_Click" />
                             &nbsp;<asp:Button ID="Button_Prod_Wise_Sales_Qnty_Download" runat="server" class="btn btn-sm btn-success"
                                 onclick="Button_Prod_Wise_Sales_Qnty_Download_Click" 
                                 style="font-family: Andalus; font-size: small;" Text="Report" 
                                 ValidationGroup="Prod_Wise_Sales_Qnty_Group" />
                             <asp:CompareValidator ID="CompareValidator11" runat="server" 
                                 ControlToCompare="TextBox_From_Date_Prod_Wise_Sales_Qnty" 
                                 ControlToValidate="TextBox_To_Date_Prod_Wise_Sales_Qnty" Display="Dynamic" 
                                 ErrorMessage="To Date earlier than from date" ForeColor="Red" 
                                 Operator="GreaterThan" ValidationGroup="Prod_Wise_Sales_Qnty_Group"></asp:CompareValidator>
                             <br />
                             </span>
                         </td>
                         <td align="center" width="45%">
                         <br />
                                     <asp:Chart ID="Chart_Transaction_Sales_Prod_Wise_Amount" runat="server" 
                                         BorderlineColor="Tan" BorderlineDashStyle="Solid" Height="400px" 
                                         ViewStateContent="All" ViewStateMode="Enabled" Width="400px" 
                                         style="text-align: right">
                                         <Series>
                                             <asp:Series ChartType="Pie" Font="Baskerville Old Face, 9pt" Legend="Legend1" 
                                                 Name="Series1">
                                             </asp:Series>
                                         </Series>
                                         <ChartAreas>
                                             <asp:ChartArea Name="ChartArea1">
                                                 <Area3DStyle Enable3D="True" />
                                             </asp:ChartArea>
                                         </ChartAreas>
                                         <Legends>
                                             <asp:Legend Font="Baskerville Old Face, 9pt" IsTextAutoFit="False" 
                                                 Name="Legend1">
                                             </asp:Legend>
                                             <asp:Legend Font="Baskerville Old Face, 9pt" IsTextAutoFit="False" 
                                                 Name="Legend2">
                                             </asp:Legend>
                                         </Legends>
                                         <Titles>
                                             <asp:Title Font="Book Antiqua, 9.75pt" Name="Title1">
                                             </asp:Title>
                                         </Titles>
                                     </asp:Chart>
                              <span class="style9">
                             <br />
                             <br />
                             </span><span class="style11"><span class="style10"><span class="style6">
                                     Filter 
                                     By Invoice Date (default range is last 12 months):<br /> </span></span>From 
                                     Date:</span>
                             <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_From_Date_Prod_Wise_Sales_Amount" runat="server" 
                                  style="font-family: Andalus" ValidationGroup="Prod_Wise_Sales_Amount_Group" 
                                 ></asp:TextBox>
                             <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_From_Date_Prod_Wise_Sales_Amount_CalendarExtender" 
                                 runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton17" 
                                 TargetControlID="TextBox_From_Date_Prod_Wise_Sales_Amount">
                             </ajaxtoolkit:CalendarExtender>
                             <span class="style11">To </span><span class="style11">Date:</span><asp:TextBox 
                                 ID="TextBox_To_Date_Prod_Wise_Sales_Amount" runat="server" 
                                class="form-control datepicker-textbox"
                                 style="font-family: Andalus" ValidationGroup="Prod_Wise_Sales_Amount_Group" 
                                 ></asp:TextBox>
                             <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_To_Date_Prod_Wise_Sales_Amount_CalendarExtender" 
                                 runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton18" 
                                 TargetControlID="TextBox_To_Date_Prod_Wise_Sales_Amount">
                             </ajaxtoolkit:CalendarExtender>
                             &nbsp;<asp:Button ID="Button_Prod_Wise_Sales_Amount" runat="server" class="btn btn-sm btn-success"
                                  style="font-family: Andalus; " 
                                 Text="Show!" ValidationGroup="Prod_Wise_Sales_Amount_Group" 
                                 onclick="Button_Prod_Wise_Sales_Amount_Click" />
                             <asp:CompareValidator ID="CompareValidator9" runat="server" 
                                 ControlToCompare="TextBox_From_Date_Prod_Wise_Sales_Amount" 
                                 ControlToValidate="TextBox_To_Date_Prod_Wise_Sales_Amount" 
                                 Display="Dynamic" ErrorMessage="To Date earlier than from date" ForeColor="Red" 
                                 Operator="GreaterThan" ValidationGroup="PotnNoDateRange"></asp:CompareValidator>
                         </td>
                     </tr>
                     <tr>
                     <td align="center" width="45%">             
                     <br />            
                         <asp:Chart ID="Chart_Sales_Pending_Clear_By_Account" runat="server" 
                             BorderlineColor="Tan" Height="400px" 
                             Width="400px" BorderlineDashStyle="Solid" Palette="EarthTones">
                             <Series>
                                 <asp:Series Name="clearedSeries" BackGradientStyle="TopBottom" 
                                     Font="Book Antiqua, 9pt" IsValueShownAsLabel="True" Legend="Cleared" 
                                     LegendText="Cleared" 
                                     
                                     CustomProperties="DrawingStyle=Cylinder, DrawSideBySide=True, LabelStyle=Center" 
                                     ChartType="StackedColumn">
                                 </asp:Series>
                                 <asp:Series ChartArea="ChartArea1" Font="Book Antiqua, 9pt" 
                                     IsValueShownAsLabel="True" Legend="Not Cleared" LegendText="Not Cleared" 
                                     Name="NotClearedSeries" 
                                     
                                     CustomProperties="DrawingStyle=Cylinder, DrawSideBySide=True, LabelStyle=Center" 
                                     ChartType="StackedColumn">
                                 </asp:Series>
                             </Series>
                             <ChartAreas>
                                 <asp:ChartArea Name="ChartArea1" BackColor="LightSteelBlue" 
                                     BackGradientStyle="Center" BorderColor="Gainsboro">
                                     <AxisY TitleFont="Baskerville Old Face, 9.75pt" IsLabelAutoFit="False">
                                         <LabelStyle Font="Book Antiqua, 9.75pt" />
                                     </AxisY>
                                     <AxisX TitleFont="Baskerville Old Face, 9.75pt" IsLabelAutoFit="False">
                                         <LabelStyle Font="Book Antiqua, 9.75pt" />
                                     </AxisX>
                                     <Area3DStyle Enable3D="True" Inclination="10" PointDepth="50" 
                                         PointGapDepth="50" />
                                 </asp:ChartArea>
                             </ChartAreas>
                             <Legends>
                                 <asp:Legend Alignment="Far" Docking="Bottom" LegendStyle="Column" Name="Cleared" 
                                     TitleFont="Book Antiqua, 9.75pt" Font="Book Antiqua, 9pt" 
                                     IsTextAutoFit="False" BackColor="White" 
                                     DockedToChartArea="ChartArea1" IsDockedInsideChartArea="False" 
                                     ItemColumnSpacing="5">
                                 </asp:Legend>
                                 <asp:Legend Alignment="Far" Name="Not Cleared" 
                                     TitleFont="Book Antiqua, 9.75pt" Font="Book Antiqua, 9pt" 
                                     IsTextAutoFit="False" BackColor="White" IsDockedInsideChartArea="False" 
                                     ItemColumnSpacing="5" LegendStyle="Column">
                                 </asp:Legend>
                             </Legends>
                             <Titles>
                                 <asp:Title Font="Book Antiqua, 8.25pt" Name="Title1">
                                 </asp:Title>
                             </Titles>
                         </asp:Chart>
                         <br />
                         <br />
                         <span class="style10"><span class="style6">Filter By Payment Date (default range 
                         is last 12 months) and Customer:<br /> From Date:</span></span>
                         <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_From_Date_Pending_Clear_Contact" runat="server" style="font-family: Andalus" ValidationGroup="Pending_Clear_Contact_Group" 
                             ></asp:TextBox>
                         <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_From_Date_Pending_Clear_Contact_CalendarExtender" 
                             runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton19" 
                             TargetControlID="TextBox_From_Date_Pending_Clear_Contact">
                         </ajaxtoolkit:CalendarExtender>
                         <span class="style10">
                         <span class="style6">To Date:</span></span><asp:TextBox 
                             ID="TextBox_To_Date_Pending_Clear_Contact" runat="server" 
                             class="form-control datepicker-textbox" 
                             style="font-family: Andalus" ValidationGroup="Pending_Clear_Contact_Group" 
                             ></asp:TextBox>
                         <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_To_Date_Pending_Clear_Contact_CalendarExtender" 
                             runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton20" 
                             TargetControlID="TextBox_To_Date_Pending_Clear_Contact">
                         </ajaxtoolkit:CalendarExtender>
                         &nbsp;<span class="style10"><span class="style6">Select customer : </span>
                         <asp:ListBox ID="ListBox_Contacts_Pending_Clear_Amnt_Sales" runat="server" 
                             Height="57px" SelectionMode="Multiple" 
                             style="font-family: Andalus; font-size: small" Width="180px"></asp:ListBox>
                         </span>
                         <asp:Button ID="Button_Pending_Clear_Contact" runat="server" class="btn btn-sm btn-success"
                             onclick="Button_Pending_Clear_Contact_Click" 
                             style="font-family: Andalus; font-size: small;" Text="Show!" 
                             ValidationGroup="Pending_Clear_Contact_Group" />
                         <span class="style10">
                         &nbsp;<asp:Button ID="Button_Pending_Clear_Contact_Download" runat="server" class="btn btn-sm btn-success"
                             onclick="Button_Pending_Clear_Contact_Download_Click" 
                             style="font-family: Andalus; font-size: small;" Text="Report" 
                             ValidationGroup="Pending_Clear_Contact_Group" />
                         <asp:CompareValidator ID="CompareValidator10" runat="server" 
                             ControlToCompare="TextBox_From_Date_Pending_Clear_Contact" 
                             ControlToValidate="TextBox_To_Date_Pending_Clear_Contact" Display="Dynamic" 
                             ErrorMessage="To Date earlier than from date" ForeColor="Red" 
                             Operator="GreaterThan" ValidationGroup="Pending_Clear_Contact_Group"></asp:CompareValidator>
                         </span>
                     </td>
                     <td align="center" width="45%">
                     <br />
                         <asp:Chart ID="Chart_Sales_Total_Business_Contact" runat="server" 
                             BorderlineColor="Tan" BorderlineDashStyle="Solid" Height="400px" 
                             Width="400px" Palette="EarthTones">
                             <Series>
                                 <asp:Series CustomProperties="DrawingStyle=Cylinder, DrawSideBySide=True, LabelStyle=Center" 
                                     Font="Book Antiqua, 9pt" Legend="Total Business" LegendText="Total Business" 
                                     Name="TotalBusiness" ChartType="StackedColumn">
                                 </asp:Series>
                                 <asp:Series BackGradientStyle="TopBottom" ChartArea="ChartArea1" 
                                     CustomProperties="DrawingStyle=Cylinder, DrawSideBySide=True, LabelStyle=Center" 
                                     Font="Book Antiqua, 9pt" IsValueShownAsLabel="True" 
                                     Legend="Business During Period" LegendText="Business During Period" 
                                     Name="BusinessDuringTimeSpan" ChartType="StackedColumn">
                                 </asp:Series>
                                 <asp:Series ChartArea="ChartArea1" 
                                     CustomProperties="DrawingStyle=Cylinder, DrawSideBySide=True, LabelStyle=Center" 
                                     Font="Book Antiqua, 9pt" IsValueShownAsLabel="True" Legend="Pending Amount" 
                                     LegendText="Total Pending" Name="TotalPendingAmount" 
                                     ChartType="StackedColumn">
                                 </asp:Series>
                             </Series>
                             <ChartAreas>
                                 <asp:ChartArea BackColor="LightSteelBlue" BackGradientStyle="Center" 
                                     BorderColor="Gainsboro" Name="ChartArea1">
                                     <AxisY IsLabelAutoFit="False" TitleFont="Baskerville Old Face, 9.75pt">
                                         <LabelStyle Font="Book Antiqua, 9.75pt" />
                                     </AxisY>
                                     <AxisX IsLabelAutoFit="False" TitleFont="Baskerville Old Face, 9.75pt">
                                         <LabelStyle Font="Book Antiqua, 9.75pt" />
                                     </AxisX>
                                     <Area3DStyle Enable3D="True" PointDepth="30" PointGapDepth="30" />
                                 </asp:ChartArea>
                             </ChartAreas>
                             <Legends>
                                 <asp:Legend Alignment="Far" Docking="Bottom" Font="Book Antiqua, 9pt" 
                                     IsTextAutoFit="False" ItemColumnSpacing="10" LegendStyle="Row" 
                                     Name="Total Business" TitleFont="Book Antiqua, 9.75pt" 
                                     BackColor="White" DockedToChartArea="ChartArea1">
                                 </asp:Legend>
                                 <asp:Legend Alignment="Far" Docking="Bottom" Font="Book Antiqua, 9pt" 
                                     IsTextAutoFit="False" ItemColumnSpacing="10" LegendStyle="Row" 
                                     Name="Business During Period" TitleFont="Book Antiqua, 9.75pt" 
                                     BackColor="White" BackImageTransparentColor="Transparent" 
                                     DockedToChartArea="ChartArea1">
                                 </asp:Legend>
                                 <asp:Legend Alignment="Far" Docking="Bottom" Font="Book Antiqua, 9pt" 
                                     IsTextAutoFit="False" ItemColumnSpacing="10" LegendStyle="Row" 
                                     Name="Pending Amount" TitleAlignment="Near" BackColor="White" 
                                     BackImageTransparentColor="Transparent" BorderColor="Transparent" 
                                     DockedToChartArea="ChartArea1">
                                 </asp:Legend>
                             </Legends>
                             <Titles>
                                 <asp:Title Font="Book Antiqua, 8.25pt" Name="Title1">
                                 </asp:Title>
                             </Titles>
                         </asp:Chart>
                         <br />
                         <span class="style10"><span class="style6">
                         <br />
                         Filter By Invoice Date (default range is last 12 months) and Customer:<br /> 
                         From Date:</span></span>
                         <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_From_Date_Chart_Sales_Total_Business_Contact" 
                             runat="server"  style="font-family: Andalus" 
                             ValidationGroup="PotnValDateRange" ></asp:TextBox>
                         <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_From_Date_Chart_Sales_Total_Business_Contact_CalendarExtender" 
                             runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton15" 
                             TargetControlID="TextBox_From_Date_Chart_Sales_Total_Business_Contact">
                         </ajaxtoolkit:CalendarExtender>
                         <span class="style10">
                         <span class="style6">To Date:</span></span><asp:TextBox 
                             ID="TextBox_To_Date_Chart_Sales_Total_Business_Contact" runat="server" 
                             class="form-control datepicker-textbox" style="font-family: Andalus" 
                             ValidationGroup="PotnValDateRange" ></asp:TextBox>
                         <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_To_Date_Chart_Sales_Total_Business_Contact_CalendarExtender" 
                             runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton16" 
                             TargetControlID="TextBox_To_Date_Chart_Sales_Total_Business_Contact">
                         </ajaxtoolkit:CalendarExtender>
                         &nbsp;<span class="style10"><span class="style6">Select customer : </span>
                         <asp:ListBox ID="ListBox_Contacts_Total_Business_Chart" runat="server" 
                             Height="57px" SelectionMode="Multiple" 
                             style="font-family: Andalus; font-size: small" Width="180px"></asp:ListBox>
                         &nbsp;</span><asp:Button ID="Button_Chart_Sales_Total_Business_Contact" class="btn btn-sm btn-success"
                             runat="server" onclick="Button_Chart_Sales_Total_Business_Contact_Click" 
                             style="font-family: Andalus; font-size: small;" Text="Show!" 
                             ValidationGroup="PotnValDateRange" />
                         <span class="style10">
                         <asp:CompareValidator ID="CompareValidator8" runat="server" 
                             ControlToCompare="TextBox_From_Date_Chart_Sales_Total_Business_Contact" 
                             ControlToValidate="TextBox_To_Date_Chart_Sales_Total_Business_Contact" 
                             Display="Dynamic" ErrorMessage="To Date earlier than from date" ForeColor="Red" 
                             Operator="GreaterThan" ValidationGroup="PotnValDateRange"></asp:CompareValidator>
                         </span>
                         <br />
                     </td>
                     </tr>
            </table>        
                                             </ContentTemplate>
                             </asp:UpdatePanel>
            <br />        
    </asp:Panel>
     <ajaxtoolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender_Tran_Sales" runat="server" Collapsed="true" AutoCollapse="false" TargetControlID="Panel_Tran_Sales_Dashboard"
                        ImageControlId="Pend_Appr_Collapse_Img2"  CollapsedImage="~/Images/ArrowTip_Down.png" ExpandedImage="~/Images/ArrowTip_Parallel.png"  ExpandControlID="LinkButton_Tran_Sales"
             CollapseControlID="LinkButton_Tran_Sales"/>
                 </ContentTemplate>
    </asp:UpdatePanel>

                        <asp:Label ID="Label_Tran_Purchase_Dashboard_Access" 
        runat="server" ForeColor="Red" 
                            style="font-size: small" Visible="False"></asp:Label>
    <br />
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
    <ContentTemplate>
                                                                <table width="100%">
                                                          <tr>
                                                                <td background="../Images/menu_bg.gif" >
                                                                <asp:LinkButton ID="LinkButton_Tran_Purchase" runat="server" 
                                                                        ToolTip="Click to expand or collapse">Transaction Dashboard-Purchase</asp:LinkButton>
                                                                    <div style="float: right; vertical-align: middle;">
                                                                    <asp:Image ID="Pend_Appr_Collapse_Img3" runat="server"/>
                                                                    </div>
                                                                    </td>
                                                            </tr>
                                                            </table>
                    <asp:UpdateProgress ID="UpdateProgress_Tran_Purchase" 
                     AssociatedUpdatePanelID="UpdatePanelTranPurchase" runat="server" DisplayAfter="1">
                 <ProgressTemplate>
                 <div style="position:fixed;top:40px;left:35%;z-index:100;width:50%;height:50%">
                 <image style="z-index:105" src="../Images/loading.gif"></image>                 
                 </div>
                 </ProgressTemplate>
                 </asp:UpdateProgress>
    <asp:Panel ID="Panel_Tran_Purchase_Dashboard" runat="server" BorderColor="#0066CC" 
            BorderStyle="Ridge" BorderWidth="2px" 
            style="font-family: Andalus">        
                                         <asp:UpdatePanel ID="UpdatePanelTranPurchase" runat="server" UpdateMode="Conditional" OnLoad="UpdatePanelTranPurchase_load">
                                 <ContentTemplate>
             <table id="potTable1" border="0" cellpadding="0" cellspacing="0" width="90%">
                     <tr>
                         <td align="center" width="45%">
                                     <asp:Chart ID="Chart_Transaction_Purchase_Prod_Wise_Qnty" runat="server" 
                                         BorderlineColor="Tan" BorderlineDashStyle="Solid" Height="400px" 
                                         ViewStateContent="All" ViewStateMode="Enabled" Width="400px">
                                         <Series>
                                             <asp:Series ChartType="Pie" Font="Baskerville Old Face, 9pt" Legend="Legend1" 
                                                 Name="Series1">
                                             </asp:Series>
                                         </Series>
                                         <ChartAreas>
                                             <asp:ChartArea Name="ChartArea1">
                                                 <Area3DStyle Enable3D="True" />
                                             </asp:ChartArea>
                                         </ChartAreas>
                                         <Legends>
                                             <asp:Legend Font="Baskerville Old Face, 9pt" IsTextAutoFit="False" 
                                                 Name="Legend1">
                                             </asp:Legend>
                                             <asp:Legend Font="Baskerville Old Face, 9pt" IsTextAutoFit="False" 
                                                 Name="Legend2">
                                             </asp:Legend>
                                         </Legends>
                                         <Titles>
                                             <asp:Title Font="Book Antiqua, 9.75pt" Name="Title1">
                                             </asp:Title>
                                         </Titles>
                                     </asp:Chart>
                             <br />
                             <span class="style10">
                             <span class="style6">Filter By Invoice Date (default range is last 12 months):<br /> 
                                     From Date:</span>
                             <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_From_Date_Prod_Wise_Purchase_Qnty" runat="server" 
                                  style="font-family: Andalus" 
                                 ValidationGroup="Prod_Wise_Purchase_Qnty_Group" ></asp:TextBox>
                             <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_From_Date_Prod_Wise_Purchase_Qnty_CalendarExtender" 
                                 runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton23" 
                                 TargetControlID="TextBox_From_Date_Prod_Wise_Purchase_Qnty">
                             </ajaxtoolkit:CalendarExtender>
                             <span class="style6">To Date:</span><asp:TextBox 
                                 ID="TextBox_To_Date_Prod_Wise_Purchase_Qnty" runat="server" class="form-control datepicker-textbox"
                                 style="font-family: Andalus" ValidationGroup="Prod_Wise_Purchase_Qnty_Group" 
                                 ></asp:TextBox>
                             <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_To_Date_Prod_Wise_Purchase_Qnty_CalendarExtender" 
                                 runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton24" 
                                 TargetControlID="TextBox_To_Date_Prod_Wise_Purchase_Qnty">
                             </ajaxtoolkit:CalendarExtender>
                             &nbsp;<asp:Button ID="Button_Prod_Wise_Purchase_Qnty" runat="server" class="btn btn-sm btn-success"
                                 style="font-family: Andalus; font-size: small;" Text="Show!" 
                                 ValidationGroup="Prod_Wise_Purchase_Qnty_Group" 
                                 onclick="Button_Prod_Wise_Purchase_Qnty_Click" />
                             &nbsp;<asp:Button ID="Button_Prod_Wise_Purchase_Qnty_Download" runat="server" class="btn btn-sm btn-success"
                                 onclick="Button_Prod_Wise_Purchase_Qnty_Download_Click" 
                                 style="font-family: Andalus; font-size: small;" Text="Report" 
                                 ValidationGroup="Prod_Wise_Purchase_Qnty_Group" />
                             <asp:CompareValidator ID="CompareValidator12" runat="server" 
                                 ControlToCompare="TextBox_From_Date_Prod_Wise_Purchase_Qnty" 
                                 ControlToValidate="TextBox_To_Date_Prod_Wise_Purchase_Qnty" Display="Dynamic" 
                                 ErrorMessage="To Date earlier than from date" ForeColor="Red" 
                                 Operator="GreaterThan" ValidationGroup="Prod_Wise_Purchase_Qnty_Group"></asp:CompareValidator>
                             <br />
                             </span>
                         </td>
                         <td align="center" width="45%">
                         <br />
                                     <asp:Chart ID="Chart_Transaction_Purchase_Prod_Wise_Amount" runat="server" BorderlineColor="Tan" 
                                         BorderlineDashStyle="Solid" Height="400px" ViewStateContent="All" 
                                         ViewStateMode="Enabled" Width="400px">
                                         <Series>
                                             <asp:Series ChartType="Pie" Font="Baskerville Old Face, 9pt" Legend="Legend1" 
                                                 Name="Series1">
                                             </asp:Series>
                                         </Series>
                                         <ChartAreas>
                                             <asp:ChartArea Name="ChartArea1">
                                                 <Area3DStyle Enable3D="True" />
                                             </asp:ChartArea>
                                         </ChartAreas>
                                         <Legends>
                                             <asp:Legend Font="Baskerville Old Face, 9pt" IsTextAutoFit="False" 
                                                 Name="Legend1">
                                             </asp:Legend>
                                             <asp:Legend Font="Baskerville Old Face, 9pt" IsTextAutoFit="False" 
                                                 Name="Legend2">
                                             </asp:Legend>
                                         </Legends>
                                         <Titles>
                                             <asp:Title Font="Book Antiqua, 9.75pt" Name="Title1">
                                             </asp:Title>
                                         </Titles>
                                     </asp:Chart>
                             <span class="style9">
                             <br />
                             <br />
                             </span><span class="style11"><span class="style10"><span class="style6">Filter By 
                                     Invoice Date (default range is last 12 months):<br /> </span></span>From Date:</span>
                             <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_From_Date_Prod_Wise_Purchase_Amount" runat="server" 
                                 style="font-family: Andalus" ValidationGroup="Prod_Wise_Purchase_Amount_Group" 
                                 ></asp:TextBox>
                             <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_From_Date_Prod_Wise_Purchase_Amount_CalendarExtender" 
                                 runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton25" 
                                 TargetControlID="TextBox_From_Date_Prod_Wise_Purchase_Amount">
                             </ajaxtoolkit:CalendarExtender>
                             <span class="style11">To </span><span class="style11">Date:</span><asp:TextBox 
                                 ID="TextBox_To_Date_Prod_Wise_Purchase_Amount" runat="server" 
                                  class="form-control datepicker-textbox"
                                 style="font-family: Andalus" ValidationGroup="Prod_Wise_Purchase_Amount_Group" 
                                 ></asp:TextBox>
                             <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_To_Date_Prod_Wise_Purchase_Amount_CalendarExtender" 
                                 runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton26" 
                                 TargetControlID="TextBox_To_Date_Prod_Wise_Purchase_Amount">
                             </ajaxtoolkit:CalendarExtender>
                             &nbsp;<asp:Button ID="Button_Prod_Wise_Purchase_Amount" runat="server" class="btn btn-sm btn-success"
                                  style="font-family: Andalus; " 
                                 Text="Show!" ValidationGroup="Prod_Wise_Purchase_Amount_Group" 
                                 onclick="Button_Prod_Wise_Purchase_Amount_Click" />
                             <asp:CompareValidator ID="CompareValidator13" runat="server" 
                                 ControlToCompare="TextBox_From_Date_Prod_Wise_Purchase_Amount" 
                                 ControlToValidate="TextBox_To_Date_Prod_Wise_Purchase_Amount" 
                                 Display="Dynamic" ErrorMessage="To Date earlier than from date" ForeColor="Red" 
                                 Operator="GreaterThan" ValidationGroup="Prod_Wise_Purchase_Amount_Group"></asp:CompareValidator>
                         </td>
                     </tr>
                     <tr>
                     <td align="center" width="45%">
                         <br />
                         <asp:Chart ID="Chart_Purchase_Pending_Clear_By_Account" runat="server" 
                             BorderlineColor="Tan" Height="400px" 
                             Width="400px" BorderlineDashStyle="Solid" Palette="EarthTones">
                             <Series>
                                 <asp:Series Name="clearedSeries" BackGradientStyle="TopBottom" 
                                     Font="Book Antiqua, 9pt" IsValueShownAsLabel="True" Legend="Cleared" 
                                     LegendText="Cleared" 
                                     
                                     CustomProperties="DrawingStyle=Cylinder, DrawSideBySide=True, LabelStyle=Center" 
                                     ChartType="StackedColumn">
                                 </asp:Series>
                                 <asp:Series ChartArea="ChartArea1" Font="Book Antiqua, 9pt" 
                                     IsValueShownAsLabel="True" Legend="Not Cleared" LegendText="Not Cleared" 
                                     Name="NotClearedSeries" 
                                     
                                     CustomProperties="DrawingStyle=Cylinder, DrawSideBySide=True, LabelStyle=Center" 
                                     ChartType="StackedColumn">
                                 </asp:Series>
                             </Series>
                             <ChartAreas>
                                 <asp:ChartArea Name="ChartArea1" BackColor="LightSteelBlue" 
                                     BackGradientStyle="Center" BorderColor="Gainsboro">
                                     <AxisY TitleFont="Baskerville Old Face, 9.75pt" IsLabelAutoFit="False">
                                         <LabelStyle Font="Book Antiqua, 9.75pt" />
                                     </AxisY>
                                     <AxisX TitleFont="Baskerville Old Face, 9.75pt" IsLabelAutoFit="False">
                                         <LabelStyle Font="Book Antiqua, 9.75pt" />
                                     </AxisX>
                                     <Area3DStyle Enable3D="True" Inclination="10" PointDepth="50" 
                                         PointGapDepth="50" />
                                 </asp:ChartArea>
                             </ChartAreas>
                             <Legends>
                                 <asp:Legend Alignment="Far" Docking="Bottom" LegendStyle="Row" Name="Cleared" 
                                     TitleFont="Book Antiqua, 9.75pt" Font="Book Antiqua, 9pt" 
                                     IsTextAutoFit="False" BackColor="White" 
                                     DockedToChartArea="ChartArea1">
                                 </asp:Legend>
                                 <asp:Legend Alignment="Far" Docking="Bottom" Name="Not Cleared" 
                                     TitleFont="Book Antiqua, 9.75pt" Font="Book Antiqua, 9pt" 
                                     IsTextAutoFit="False" BackColor="White" 
                                     DockedToChartArea="ChartArea1">
                                 </asp:Legend>
                             </Legends>
                             <Titles>
                                 <asp:Title Font="Book Antiqua, 8.25pt" Name="Title1">
                                 </asp:Title>
                             </Titles>
                         </asp:Chart>
                         <br />
                         <br />                         
                         <span class="style10"><span class="style6">Filter By Payment Date (default range 
                         is last 12 months) and Vendor:<br /> From Date:</span></span>
                         <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_From_Date_Pending_Clear_Contact_Purchase" 
                             runat="server" 
                              style="font-family: Andalus" ValidationGroup="Pending_Clear_Contact_Group_Purchase" 
                             ></asp:TextBox>
                         <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_From_Date_Pending_Clear_Contact_Purchase_CalendarExtender" 
                             runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton27" 
                             TargetControlID="TextBox_From_Date_Pending_Clear_Contact_Purchase">
                         </ajaxtoolkit:CalendarExtender>
                         <span class="style10">
                         <span class="style6">To Date:</span></span><asp:TextBox 
                             ID="TextBox_To_Date_Pending_Clear_Contact_Purchase" runat="server" 
                             class="form-control datepicker-textbox"
                             style="font-family: Andalus" ValidationGroup="Pending_Clear_Contact_Group_Purchase" 
                             ></asp:TextBox>
                         <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_To_Date_Pending_Clear_Contact_Purchase_CalendarExtender" 
                             runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton28" 
                             TargetControlID="TextBox_To_Date_Pending_Clear_Contact_Purchase">
                         </ajaxtoolkit:CalendarExtender>
                         &nbsp;<span class="style10"><span class="style6">Select Vendor : </span>
                         <asp:ListBox ID="ListBox_Contacts_Pending_Clear_Amnt_Purchase" runat="server" 
                             Height="57px" SelectionMode="Multiple" 
                             style="font-family: Andalus; font-size: small" Width="180px"></asp:ListBox>
                         </span>
                         <asp:Button ID="Button_Pending_Clear_Contact_Purchase" runat="server" class="btn btn-sm btn-success"
                             style="font-family: Andalus; font-size: small;" Text="Show!" 
                             ValidationGroup="Pending_Clear_Contact_Group_Purchase" 
                             onclick="Button_Pending_Clear_Contact_Purchase_Click" />
                         <span class="style10">
                         &nbsp;<asp:Button ID="Button_Pending_Clear_Contact_Purchase_Download" runat="server" class="btn btn-sm btn-success"
                             onclick="Button_Pending_Clear_Contact_Purchase_Download_Click" 
                             style="font-family: Andalus; font-size: small;" Text="Report" 
                             ValidationGroup="Pending_Clear_Contact_Group_Purchase" />
                         </span>
                     </td>
                     <td align="center" width="45%">
                     <br />
                         <asp:Chart ID="Chart_Purchase_Total_Business_Contact" runat="server" 
                             BorderlineColor="Tan" BorderlineDashStyle="Solid" Height="400px" 
                             Width="400px" Palette="EarthTones">
                             <Series>
                                 <asp:Series CustomProperties="DrawingStyle=Cylinder, DrawSideBySide=True, LabelStyle=Center" 
                                     Font="Book Antiqua, 9pt" Legend="Total Business" LegendText="Total Business" 
                                     Name="TotalBusiness" ChartType="StackedColumn">
                                 </asp:Series>
                                 <asp:Series BackGradientStyle="TopBottom" ChartArea="ChartArea1" 
                                     CustomProperties="DrawingStyle=Cylinder, DrawSideBySide=True, LabelStyle=Center" 
                                     Font="Book Antiqua, 9pt" IsValueShownAsLabel="True" 
                                     Legend="Business During Period" LegendText="Business During Period" 
                                     Name="BusinessDuringTimeSpan" ChartType="StackedColumn">
                                 </asp:Series>
                                 <asp:Series ChartArea="ChartArea1" 
                                     CustomProperties="DrawingStyle=Cylinder, DrawSideBySide=True, LabelStyle=Center" 
                                     Font="Book Antiqua, 9pt" IsValueShownAsLabel="True" Legend="Pending Amount" 
                                     LegendText="Total Pending" Name="TotalPendingAmount" 
                                     ChartType="StackedColumn">
                                 </asp:Series>
                             </Series>
                             <ChartAreas>
                                 <asp:ChartArea BackColor="LightSteelBlue" BackGradientStyle="Center" 
                                     BorderColor="Gainsboro" Name="ChartArea1">
                                     <AxisY IsLabelAutoFit="False" TitleFont="Baskerville Old Face, 9.75pt">
                                         <LabelStyle Font="Book Antiqua, 9.75pt" />
                                     </AxisY>
                                     <AxisX IsLabelAutoFit="False" TitleFont="Baskerville Old Face, 9.75pt">
                                         <LabelStyle Font="Book Antiqua, 9.75pt" />
                                     </AxisX>
                                     <Area3DStyle Enable3D="True" PointDepth="30" PointGapDepth="30" />
                                 </asp:ChartArea>
                             </ChartAreas>
                             <Legends>
                                 <asp:Legend Alignment="Far" Docking="Bottom" Font="Book Antiqua, 9pt" 
                                     IsTextAutoFit="False" ItemColumnSpacing="10" LegendStyle="Row" 
                                     Name="Total Business" TitleFont="Book Antiqua, 9.75pt" 
                                     BackColor="White" DockedToChartArea="ChartArea1">
                                 </asp:Legend>
                                 <asp:Legend Alignment="Far" Docking="Bottom" Font="Book Antiqua, 9pt" 
                                     IsTextAutoFit="False" ItemColumnSpacing="10" LegendStyle="Row" 
                                     Name="Business During Period" TitleFont="Book Antiqua, 9.75pt" 
                                     BackColor="White" DockedToChartArea="ChartArea1">
                                 </asp:Legend>
                                 <asp:Legend Alignment="Far" Docking="Bottom" Font="Book Antiqua, 9pt" 
                                     IsTextAutoFit="False" ItemColumnSpacing="10" LegendStyle="Row" 
                                     Name="Pending Amount" TitleAlignment="Near" BackColor="White" 
                                     DockedToChartArea="ChartArea1">
                                 </asp:Legend>
                             </Legends>
                             <Titles>
                                 <asp:Title Font="Book Antiqua, 8.25pt" Name="Title1">
                                 </asp:Title>
                             </Titles>
                         </asp:Chart>
                         <br />
                         <span class="style10"><span class="style6">
                         <br />
                         Filter By Invoice Date (default range is last 12 months) and Vendor:<br /> From 
                         Date:</span></span>
                         <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_From_Date_Chart_Purchase_Total_Business_Contact" 
                             runat="server"   style="font-family: Andalus" 
                             ValidationGroup="PotnValDateRange" ></asp:TextBox>
                         <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_From_Date_Chart_Purchase_Total_Business_Contact_CalendarExtender" 
                             runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton29" 
                             TargetControlID="TextBox_From_Date_Chart_Purchase_Total_Business_Contact">
                         </ajaxtoolkit:CalendarExtender>
                         <span class="style10">
                         <span class="style6">To Date:</span></span><asp:TextBox 
                             ID="TextBox_To_Date_Chart_Purchase_Total_Business_Contact" runat="server" 
                              style="font-family: Andalus" class="form-control datepicker-textbox" 
                             ValidationGroup="PotnValDateRange" ></asp:TextBox>
                         <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_To_Date_Chart_Purchase_Total_Business_Contact_CalendarExtender" 
                             runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton30" 
                             TargetControlID="TextBox_To_Date_Chart_Purchase_Total_Business_Contact">
                         </ajaxtoolkit:CalendarExtender>
                         &nbsp;<span class="style10"><span class="style6">Select customer : </span>
                         <asp:ListBox ID="ListBox_Contacts_Total_Business_Chart_Purchase" runat="server" 
                             Height="57px" SelectionMode="Multiple" 
                             style="font-family: Andalus; font-size: small" Width="180px"></asp:ListBox>
                         &nbsp;</span><asp:Button ID="Button_Chart_Purchase_Total_Business_Contact" class="btn btn-sm btn-success"
                             runat="server" 
                             style="font-family: Andalus; font-size: small;" Text="Show!" 
                             ValidationGroup="PotnValDateRange" 
                             onclick="Button_Chart_Purchase_Total_Business_Contact_Click" />
                         <span class="style10">
                         <asp:CompareValidator ID="CompareValidator15" runat="server" 
                             ControlToCompare="TextBox_From_Date_Chart_Purchase_Total_Business_Contact" 
                             ControlToValidate="TextBox_To_Date_Chart_Purchase_Total_Business_Contact" 
                             Display="Dynamic" ErrorMessage="To Date earlier than from date" ForeColor="Red" 
                             Operator="GreaterThan" ValidationGroup="PotnValDateRange"></asp:CompareValidator>
                         </span>
                         <br />
                     </td>
                     </tr>
            </table>        
                                             </ContentTemplate>
                             </asp:UpdatePanel>
            <br />        
    </asp:Panel>
    <ajaxtoolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender_Tran_Purchase" runat="server" Collapsed="true" AutoCollapse="false" TargetControlID="Panel_Tran_Purchase_Dashboard"
                        ImageControlId="Pend_Appr_Collapse_Img3"  CollapsedImage="~/Images/ArrowTip_Down.png" ExpandedImage="~/Images/ArrowTip_Parallel.png"  ExpandControlID="LinkButton_Tran_Purchase"
             CollapseControlID="LinkButton_Tran_Purchase"/>
                              </ContentTemplate>
    </asp:UpdatePanel>

                        <asp:Label ID="Label_Incm_Def_Dashboard_Access" 
        runat="server" ForeColor="Red" 
                            style="font-size: small" Visible="False"></asp:Label>
    <br />
    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
    <ContentTemplate>

                                                                <table width="100%">
                                                          <tr>
                                                                <td background="../Images/menu_bg.gif" >
                                                                <asp:LinkButton ID="LinkButton_Inc_Defect" runat="server" 
                                                                        ToolTip="Click to expand or collapse">Incoming Defects Dashboard</asp:LinkButton>
                                                                    <div style="float: right; vertical-align: middle;">
                                                                    <asp:Image ID="Pend_Appr_Collapse_Img4" runat="server"/>
                                                                    </div>
                                                                    </td>
                                                            </tr>
                                                            </table>
                 <asp:UpdateProgress ID="UpdateProgress_Incm_Def" 
                     AssociatedUpdatePanelID="UpdatePanelIncmDefects" runat="server" DisplayAfter="1">
                 <ProgressTemplate>
                 <div style="position:fixed;top:40px;left:35%;z-index:100;width:50%;height:50%">
                 <image style="z-index:105" src="../Images/loading.gif"></image>                 
                 </div>
                 </ProgressTemplate>
                 </asp:UpdateProgress>
    <asp:Panel ID="Panel_Incoming_Defects" runat="server" BorderColor="#0066CC" 
            BorderStyle="Ridge" BorderWidth="2px"
            style="font-family: Andalus">     
                                <asp:UpdatePanel ID="UpdatePanelIncmDefects" runat="server" UpdateMode="Conditional" OnLoad="UpdatePanelIncmDefects_load">
                                 <ContentTemplate>   
             <table id="potTable2" border="0" cellpadding="0" cellspacing="0" width="90%">
                     <tr>
                         <td align="left" style="font-size: small; font-weight: 700" width="45%">
                                     <asp:Chart ID="Chart_Defect_Arrival_Closure" runat="server" 
                                         BorderlineColor="Tan" BorderlineDashStyle="Solid" Height="412px" 
                                         ViewStateContent="All" ViewStateMode="Enabled" Width="1126px" 
                                         Palette="EarthTones">
                                         <Series>
                                             <asp:Series ChartType="StackedColumn" Font="Book Antiqua, 8.25pt" Legend="Legend1" 
                                                 Name="Series_Defects_High" ChartArea="ChartArea_New_Defects" 
                                                 CustomProperties="DrawingStyle=Cylinder" LegendText="High Sev Created">
                                             </asp:Series>
                                             <asp:Series ChartArea="ChartArea_New_Defects" ChartType="StackedColumn" 
                                                 CustomProperties="DrawingStyle=Cylinder" Legend="Legend1" 
                                                 Name="Series_Defects_Low" Font="Book Antiqua, 8.25pt" 
                                                 LegendText="Low Sev Created">
                                             </asp:Series>
                                             <asp:Series ChartArea="ChartArea_New_Defects" ChartType="StackedColumn" 
                                                 CustomProperties="DrawingStyle=Cylinder" Legend="Legend1" 
                                                 Name="Series_Defects_Medium" Font="Book Antiqua, 8.25pt" 
                                                 LegendText="Medium Sev Created">
                                             </asp:Series>
                                             <asp:Series ChartArea="ChartArea_Closure" ChartType="StackedColumn" 
                                                 CustomProperties="DrawingStyle=Cylinder, DrawSideBySide=True" 
                                                 Font="Book Antiqua, 8.25pt" Legend="Legend1" Name="Series_Closure_High" 
                                                 LegendText="High Sev Closed">
                                             </asp:Series>
                                             <asp:Series ChartArea="ChartArea_Closure" ChartType="StackedColumn" 
                                                 CustomProperties="DrawingStyle=Cylinder" Legend="Legend1" 
                                                 Name="Series_Closure_Medium" Font="Book Antiqua, 8.25pt" 
                                                 LegendText="Medium Sev Closed">
                                             </asp:Series>
                                             <asp:Series ChartArea="ChartArea_Closure" ChartType="StackedColumn" 
                                                 CustomProperties="DrawingStyle=Cylinder" Legend="Legend1" 
                                                 Name="Series_Closure_Low" Font="Book Antiqua, 8.25pt" 
                                                 LegendText="Low Sev Closed">
                                             </asp:Series>
                                         </Series>
                                         <ChartAreas>
                                             <asp:ChartArea Name="ChartArea_Closure">
                                                 <AxisY IsLabelAutoFit="False">
                                                     <LabelStyle Font="Book Antiqua, 8.25pt" />
                                                     <MajorGrid LineDashStyle="Dot" />
                                                 </AxisY>
                                                 <AxisX IsLabelAutoFit="False">
                                                     <LabelStyle Font="Book Antiqua, 8.25pt" />
                                                 </AxisX>
                                             </asp:ChartArea>
                                             <asp:ChartArea Name="ChartArea_New_Defects">
                                                 <AxisY IsLabelAutoFit="False">
                                                     <LabelStyle Font="Book Antiqua, 8.25pt" />
                                                     <MajorGrid LineDashStyle="Dot" />
                                                 </AxisY>
                                                 <AxisX IsLabelAutoFit="False">
                                                     <LabelStyle Font="Book Antiqua, 8.25pt" />
                                                 </AxisX>
                                             </asp:ChartArea>
                                         </ChartAreas>
                                         <Legends>
                                             <asp:Legend Font="Baskerville Old Face, 9pt" IsTextAutoFit="False" 
                                                 Name="Legend1" TitleFont="Book Antiqua, 8.25pt, style=Bold">
                                             </asp:Legend>
                                         </Legends>
                                         <Titles>
                                             <asp:Title Font="Book Antiqua, 9pt, style=Bold" Name="Defect Arrival" 
                                                 DockedToChartArea="ChartArea_New_Defects" Text="Defect Arrival By Severity">
                                             </asp:Title>
                                             <asp:Title DockedToChartArea="ChartArea_Closure" 
                                                 Font="Book Antiqua, 9pt, style=Bold" Name="Defect Closure" 
                                                 Text="Defect Closure By Severity">
                                             </asp:Title>
                                         </Titles>
                                     </asp:Chart>
                             <br />
                             <span class="style10">
                             <span class="style6">Filter by Defect Submit Date and Frequency (Default date range is 6 
                                     months):<br /> From Date:</span>
                             <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_From_Date_Incoming_Defect_Arrvl_Closure" runat="server" 
                                  style="font-family: Andalus" 
                                 ValidationGroup="Incoming_Defect_Arrvl_Closure" ></asp:TextBox>
                             <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_From_Date_Incoming_Defect_Arrvl_Closure_CalendarExtender" 
                                 runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton31" 
                                 TargetControlID="TextBox_From_Date_Incoming_Defect_Arrvl_Closure">
                             </ajaxtoolkit:CalendarExtender>
                             <span class="style6">To Date:</span><asp:TextBox 
                                 ID="TextBox_To_Date_Incoming_Defect_Arrvl_Closure" class="form-control datepicker-textbox" runat="server"  style="font-family: Andalus" ValidationGroup="Incoming_Defect_Arrvl_Closure" 
                                 ></asp:TextBox>
                             <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_To_Date_Incoming_Defect_Arrvl_Closure_CalendarExtender" 
                                 runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton32" 
                                 TargetControlID="TextBox_To_Date_Incoming_Defect_Arrvl_Closure">
                             </ajaxtoolkit:CalendarExtender>
                                     &nbsp; Frequency:&nbsp;<asp:DropDownList 
                                         ID="DropDownList_Incm_Defect_Arrival_Closure_Freq" runat="server">
                                     </asp:DropDownList>
                             &nbsp;<asp:Button ID="Button_Filter_Date_Incoming_Defect_Arrvl_Closure" runat="server" class="btn btn-sm btn-success"
                                         onclick="Button_Filter_Date_Incoming_Defect_Arrvl_Closure_Click" 
                                         style="font-family: Andalus; font-size: small;" Text="Show!" 
                                         ValidationGroup="Incoming_Defect_Arrvl_Closure" />
                                     &nbsp;<asp:Button ID="Button_Report_Date_Incoming_Defect_Arrival_Closure" class="btn btn-sm btn-success"
                                         runat="server" 
                                         onclick="Button_Report_Date_Incoming_Defect_Arrival_Closure_Click" 
                                         style="font-family: Andalus; font-size: small;" Text="Report!" 
                                         ValidationGroup="Incoming_Defect_Arrvl_Closure" />
                                     &nbsp;<asp:CompareValidator ID="CompareValidator16" runat="server" 
                                 ControlToCompare="TextBox_From_Date_Incoming_Defect_Arrvl_Closure" 
                                 ControlToValidate="TextBox_To_Date_Incoming_Defect_Arrvl_Closure" Display="Dynamic" 
                                 ErrorMessage="To Date earlier than from date" ForeColor="Red" 
                                 Operator="GreaterThan" ValidationGroup="Incoming_Defect_Arrvl_Closure"></asp:CompareValidator>
                             <br />
                             </span>
                         </td>
                     </tr>
                 </table>
                 <table id="IncmDefectClsTime" border="0" cellpadding="0" cellspacing="0" width="90%">
                 <tr>
                 <td>
                     <asp:Chart ID="Chart_Incm_Defect_Closure_Avg_Time" runat="server" 
                         BorderlineColor="Tan" BorderlineDashStyle="Solid" Height="347px" 
                         ViewStateContent="All" ViewStateMode="Enabled" Width="1126px">
                         <Series>
                             <asp:Series ChartArea="ChartArea_Defect_Closure" ChartType="Line" 
                                 CustomProperties="DrawingStyle=Cylinder" Font="Book Antiqua, 8.25pt" 
                                 Legend="Legend1" LegendText="All Sev Avg Closure Time" 
                                 Name="Series_Defects_Closure_Total" BorderWidth="3" Color="Red">
                             </asp:Series>
                             <asp:Series ChartArea="ChartArea_Defect_Closure" ChartType="Line" 
                                 CustomProperties="DrawingStyle=Cylinder, DrawSideBySide=True" 
                                 Font="Book Antiqua, 8.25pt" Legend="Legend1" 
                                 LegendText="High Sev Avg Closure Time" Name="Series_Closure_High" 
                                 BorderDashStyle="Dot" BorderWidth="2" LabelToolTip="High Severity">
                             </asp:Series>
                             <asp:Series ChartArea="ChartArea_Defect_Closure" ChartType="Line" 
                                 CustomProperties="DrawingStyle=Cylinder" Font="Book Antiqua, 8.25pt" 
                                 Legend="Legend1" LegendText="Medium Sev Avg Closure Time" 
                                 Name="Series_Closure_Medium" BorderDashStyle="Dash" BorderWidth="2" 
                                 LabelToolTip="Medium Severity">
                             </asp:Series>
                             <asp:Series ChartArea="ChartArea_Defect_Closure" ChartType="Line" 
                                 CustomProperties="DrawingStyle=Cylinder" Font="Book Antiqua, 8.25pt" 
                                 Legend="Legend1" LegendText="Low Sev Avg Closure Time" 
                                 Name="Series_Closure_Low" BorderWidth="2" LabelToolTip="Low Severity">
                             </asp:Series>
                         </Series>
                         <ChartAreas>
                             <asp:ChartArea Name="ChartArea_Defect_Closure">
                                 <AxisY IsLabelAutoFit="False">
                                     <LabelStyle Font="Book Antiqua, 8.25pt" />
                                     <MajorGrid LineDashStyle="Dot" />
                                 </AxisY>
                                 <AxisX IsLabelAutoFit="False">
                                     <LabelStyle Font="Book Antiqua, 8.25pt" />
                                 </AxisX>
                             </asp:ChartArea>
                         </ChartAreas>
                         <Legends>
                             <asp:Legend Font="Baskerville Old Face, 9pt" IsTextAutoFit="False" 
                                 Name="Legend1" TitleFont="Book Antiqua, 8.25pt, style=Bold">
                             </asp:Legend>
                         </Legends>
                         <Titles>
                             <asp:Title DockedToChartArea="ChartArea_Defect_Closure" 
                                 Font="Book Antiqua, 9pt, style=Bold" Name="Defect Closure Time" 
                                 Text="Defect Closure Average Time (Hours)">
                             </asp:Title>
                         </Titles>
                     </asp:Chart>
                     <br />
                     <span class="style10"><span class="style6">Filter by Defect Submit 
                     Date,Frequency and Service Agent (Default date range is 6 months):<br /> From 
                     Date:</span>
                     <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_From_Date_Incoming_Defect_Avg_Closure" runat="server" 
                         style="font-family: Andalus" 
                         ValidationGroup="Incoming_Defect_Avg_Closure" ></asp:TextBox>
                     <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_From_Date_Incoming_Defect_Avg_Closure_CalendarExtender" 
                         runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton47" 
                         TargetControlID="TextBox_From_Date_Incoming_Defect_Avg_Closure">
                     </ajaxtoolkit:CalendarExtender>
                     <span class="style6">To Date:</span><asp:TextBox 
                         ID="TextBox_To_Date_Incoming_Defect_Avg_Closure" runat="server" 
                         class="form-control datepicker-textbox" style="font-family: Andalus" 
                         ValidationGroup="Incoming_Defect_Avg_Closure" ></asp:TextBox>
                     <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_To_Date_Incoming_Defect_Avg_Closure_CalendarExtender" 
                         runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton48" 
                         TargetControlID="TextBox_To_Date_Incoming_Defect_Avg_Closure">
                     </ajaxtoolkit:CalendarExtender>
                     &nbsp; Frequency:&nbsp;<asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Incm_Defect_Avg_Closure_Freq" 
                         runat="server">
                     </asp:DropDownList>
                     &nbsp;Select Service Agent:&nbsp;<asp:DropDownList 
                         ID="DropDownList_Incm_Defect_Avg_Closure_Service_Agnt" runat="server">
                     </asp:DropDownList>
                     &nbsp;<asp:Button ID="Button_Filter_Date_Incoming_Defect_Avg_Closure" runat="server" class="btn btn-sm btn-success"
                         style="font-family: Andalus; font-size: small;" Text="Show!" 
                         ValidationGroup="Incoming_Defect_Avg_Closure" 
                         onclick="Button_Filter_Date_Incoming_Defect_Avg_Closure_Click" />
                     &nbsp;<asp:Button ID="Button_Report_Date_Incoming_Defect_Avg_Closure" runat="server" class="btn btn-sm btn-success"
                         onclick="Button_Report_Date_Incoming_Defect_Avg_Closure_Click" 
                         style="font-family: Andalus; font-size: small;" Text="Report!" 
                         ValidationGroup="Incoming_Defect_Avg_Closure" />
                     <asp:CompareValidator ID="CompareValidator24" runat="server" 
                         ControlToCompare="TextBox_From_Date_Incoming_Defect_Avg_Closure" 
                         ControlToValidate="TextBox_To_Date_Incoming_Defect_Avg_Closure" 
                         Display="Dynamic" ErrorMessage="To Date earlier than from date" ForeColor="Red" 
                         Operator="GreaterThan" ValidationGroup="Incoming_Defect_Avg_Closure"></asp:CompareValidator>
                     <br />
                     </span>
                 </td>
                 </tr>
                 </table>
                 <table id="IncmDefect" border="0" cellpadding="0" cellspacing="0" width="90%">
                     <caption>
                         &nbsp;<tr>
                             <td align="center" width="45%">                                 
                                 <asp:Chart ID="Chart_Incoming_Defect_By_Account" runat="server" 
                                     BorderlineColor="Tan" BorderlineDashStyle="Solid" Width="400px" 
                                     Palette="EarthTones">
                                     <Series>
                                         <asp:Series BackGradientStyle="TopBottom" 
                                             CustomProperties="DrawingStyle=Cylinder, DrawSideBySide=True, LabelStyle=Center" 
                                             Font="Book Antiqua, 9pt" IsValueShownAsLabel="True" LegendText="Cleared" 
                                             Name="TotalDefects">
                                         </asp:Series>
                                     </Series>
                                     <ChartAreas>
                                         <asp:ChartArea BackColor="LightSteelBlue" BackGradientStyle="Center" 
                                             BorderColor="Gainsboro" Name="ChartArea1">
                                             <AxisY IsLabelAutoFit="False" TitleFont="Baskerville Old Face, 9.75pt">
                                                 <LabelStyle Font="Book Antiqua, 9.75pt" />
                                             </AxisY>
                                             <AxisX IsLabelAutoFit="False" TitleFont="Baskerville Old Face, 9.75pt">
                                                 <LabelStyle Font="Book Antiqua, 9.75pt" />
                                             </AxisX>
                                             <Area3DStyle Enable3D="True" Inclination="10" PointDepth="50" 
                                                 PointGapDepth="50" />
                                         </asp:ChartArea>
                                     </ChartAreas>
                                     <Titles>
                                         <asp:Title Font="Book Antiqua, 8.25pt" Name="Title1">
                                         </asp:Title>
                                     </Titles>
                                 </asp:Chart>
                                 <br />
                                 <br />
                                 <span class="style10"><span class="style6">Filter by Defect Submit Date and 
                                 Customer (Default date range is 12 months):<br /> From Date:</span></span>
                                 <asp:TextBox class="form-control datepicker-textbox" ID="Textbox_From_Date_Incoming_Defect_By_Account" runat="server" 
                                      style="font-family: Andalus" 
                                     ValidationGroup="Chart_Incoming_Defect_By_Account_Group" ></asp:TextBox>
                                 <ajaxtoolkit:CalendarExtender Enabled="false" ID="Textbox_From_Date_Incoming_Defect_By_Account_CalendarExtender" 
                                     runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton35" 
                                     TargetControlID="Textbox_From_Date_Incoming_Defect_By_Account">
                                 </ajaxtoolkit:CalendarExtender>
                                 <span class="style10">
                                 <asp:ImageButton ID="ImageButton35" runat="server" Height="19px" 
                                     ImageUrl="~/Images/Calendar.png" />
                                 <span class="style6">To Date:</span></span><asp:TextBox 
                                     ID="Textbox_To_Date_Incoming_Defect_By_Account" runat="server" 
                                      style="font-family: Andalus" class="form-control datepicker-textbox"
                                     ValidationGroup="Chart_Incoming_Defect_By_Account_Group" ></asp:TextBox>
                                 <ajaxtoolkit:CalendarExtender Enabled="false" ID="Textbox_To_Date_Incoming_Defect_By_Account_CalendarExtender" 
                                     runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton36" 
                                     TargetControlID="Textbox_To_Date_Incoming_Defect_By_Account">
                                 </ajaxtoolkit:CalendarExtender>
                                 <span class="style10">
                                 <asp:ImageButton ID="ImageButton36" runat="server" Height="19px" 
                                     ImageUrl="~/Images/Calendar.png" />
                                 </span>&nbsp;<span class="style10"><span class="style6">Select customer : </span>
                                 <asp:ListBox ID="ListBox_Incoming_Defect_By_Account" runat="server" 
                                     Height="57px" SelectionMode="Multiple" 
                                     style="font-family: Andalus; font-size: small" Width="180px"></asp:ListBox>
                                 </span>
                                 <asp:Button ID="Button_Incoming_Defect_By_Account" runat="server" class="btn btn-sm btn-success"
                                     onclick="Button_Incoming_Defect_By_Account_Click" 
                                     style="font-family: Andalus; font-size: small;" Text="Show!" 
                                     ValidationGroup="Chart_Incoming_Defect_By_Account_Group" />
                                 <span class="style10">&nbsp;<asp:Button 
                                     ID="Button_Incoming_Defect_By_Account_Download" runat="server" class="btn btn-sm btn-success"
                                     onclick="Button_Incoming_Defect_By_Account_Download_Click" 
                                     style="font-family: Andalus; font-size: small;" Text="Report" 
                                     ValidationGroup="Chart_Incoming_Defect_By_Account_Group" />
                                 <asp:CompareValidator ID="CompareValidator18" runat="server" 
                                     ControlToCompare="Textbox_From_Date_Incoming_Defect_By_Account" 
                                     ControlToValidate="Textbox_To_Date_Incoming_Defect_By_Account" 
                                     Display="Dynamic" ErrorMessage="To Date earlier than from date" ForeColor="Red" 
                                     Operator="GreaterThan" ValidationGroup="Chart_Incoming_Defect_By_Account_Group"></asp:CompareValidator>
                                 </span>
                             </td>
                             <td align="center" width="45%">
                             <br />
                                 <asp:Chart ID="Chart_Incoming_Defect_No_By_Account" runat="server" 
                                     BorderlineColor="Tan" BorderlineDashStyle="Solid" Width="400px" 
                                     Palette="EarthTones">
                                     <Series>
                                         <asp:Series BackGradientStyle="TopBottom" ChartType="StackedColumn" 
                                             CustomProperties="DrawingStyle=Cylinder, DrawSideBySide=True, LabelStyle=Center" 
                                             Font="Book Antiqua, 9pt" IsValueShownAsLabel="True" Legend="Legend_High" 
                                             LegendText="High Sev" Name="HighDefects">
                                         </asp:Series>
                                         <asp:Series ChartArea="ChartArea1" ChartType="StackedColumn" 
                                             CustomProperties="DrawingStyle=Cylinder" Legend="Legend_Low" 
                                             LegendText="Low Sev" Name="LowDefects">
                                         </asp:Series>
                                         <asp:Series ChartArea="ChartArea1" ChartType="StackedColumn" 
                                             CustomProperties="DrawingStyle=Cylinder" Legend="Legend_Medium" 
                                             LegendText="Medium Sev" Name="MediumDefects">
                                         </asp:Series>
                                     </Series>
                                     <ChartAreas>
                                         <asp:ChartArea BackColor="LightSteelBlue" BackGradientStyle="Center" 
                                             BorderColor="Gainsboro" Name="ChartArea1">
                                             <AxisY IsLabelAutoFit="False" TitleFont="Baskerville Old Face, 9.75pt">
                                                 <LabelStyle Font="Book Antiqua, 9.75pt" />
                                             </AxisY>
                                             <AxisX IsLabelAutoFit="False" TitleFont="Baskerville Old Face, 9.75pt">
                                                 <LabelStyle Font="Book Antiqua, 9.75pt" />
                                             </AxisX>
                                             <Area3DStyle Enable3D="True" Inclination="10" PointDepth="50" 
                                                 PointGapDepth="50" />
                                         </asp:ChartArea>
                                     </ChartAreas>
                                     <Legends>
                                         <asp:Legend Alignment="Far" BackColor="White" 
                                             DockedToChartArea="ChartArea1" Docking="Bottom" Font="Book Antiqua, 8.25pt" 
                                             IsTextAutoFit="False" ItemColumnSpacing="10" LegendStyle="Row" 
                                             Name="Legend_High" TitleFont="Book Antiqua, 8pt, style=Bold">
                                         </asp:Legend>
                                         <asp:Legend Alignment="Far" BackColor="White" 
                                             DockedToChartArea="ChartArea1" Docking="Bottom" Font="Book Antiqua, 8.25pt" 
                                             IsTextAutoFit="False" ItemColumnSpacing="10" LegendStyle="Row" 
                                             Name="Legend_Low" TitleFont="Book Antiqua, 8pt, style=Bold">
                                         </asp:Legend>
                                         <asp:Legend Alignment="Far" BackColor="White" 
                                             DockedToChartArea="ChartArea1" Docking="Bottom" Font="Book Antiqua, 8.25pt" 
                                             IsTextAutoFit="False" ItemColumnSpacing="10" LegendStyle="Row" 
                                             Name="Legend_Medium" TitleFont="Book Antiqua, 8pt, style=Bold">
                                         </asp:Legend>
                                     </Legends>
                                     <Titles>
                                         <asp:Title Font="Book Antiqua, 8.25pt" Name="Title1">
                                         </asp:Title>
                                     </Titles>
                                 </asp:Chart>
                                 <br />
                                 <br />
                                 <span class="style10"><span class="style6">Filter by Defect Submit Date,Type and 
                                 Customer(Default date range is 12 months):<br /> From Date:</span></span>
                                 <asp:TextBox class="form-control datepicker-textbox" ID="Textbox_From_Date_Incoming_Defect_No_By_Account" 
                                     runat="server"  style="font-family: Andalus" 
                                     ValidationGroup="Incoming_Defect_No_By_Account" ></asp:TextBox>
                                 <ajaxtoolkit:CalendarExtender Enabled="false" ID="Textbox_From_Date_Incoming_Defect_No_By_Account_CalendarExtender" 
                                     runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton37" 
                                     TargetControlID="Textbox_From_Date_Incoming_Defect_No_By_Account">
                                 </ajaxtoolkit:CalendarExtender>
                                 <span class="style10">
                                 <asp:ImageButton ID="ImageButton37" runat="server" Height="19px" 
                                     ImageUrl="~/Images/Calendar.png" />
                                 <span class="style6">To Date:</span></span><asp:TextBox 
                                     ID="Textbox_To_Date_Incoming_Defect_No_By_Account" runat="server" 
                                     class="form-control datepicker-textbox" style="font-family: Andalus" 
                                     ValidationGroup="Incoming_Defect_No_By_Account" ></asp:TextBox>
                                 <ajaxtoolkit:CalendarExtender Enabled="false" ID="Textbox_To_Date_Incoming_Defect_No_By_Account_CalendarExtender" 
                                     runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton38" 
                                     TargetControlID="Textbox_To_Date_Incoming_Defect_No_By_Account">
                                 </ajaxtoolkit:CalendarExtender>
                                 <span class="style10">
                                 <asp:ImageButton ID="ImageButton38" runat="server" Height="19px" 
                                     ImageUrl="~/Images/Calendar.png" />
                                 </span>&nbsp;<span class="style10"><span class="style6">Defect Types:
                                 <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Incoming_Defect_No_By_Account_Defect_Type" 
                                     runat="server">
                                 </asp:DropDownList>
                                 &nbsp;Select customer : </span>
                                 <asp:ListBox ID="Listbox_Incoming_Defect_No_By_Account" runat="server" 
                                     Height="57px" SelectionMode="Multiple" 
                                     style="font-family: Andalus; font-size: small" Width="180px"></asp:ListBox>
                                 &nbsp;</span><asp:Button ID="Button_Incoming_Defect_No_By_Account" runat="server" class="btn btn-sm btn-success"
                                     onclick="Button_Incoming_Defect_No_By_Account_Click" 
                                     style="font-family: Andalus; font-size: small;" Text="Show!" 
                                     ValidationGroup="Incoming_Defect_No_By_Account" />
                                 <span class="style10">&nbsp;<asp:Button 
                                     ID="Button_Incoming_Defect_No_By_Account_Download" runat="server" class="btn btn-sm btn-success"
                                     onclick="Button_Incoming_Defect_No_By_Account_Download_Click" 
                                     style="font-family: Andalus; font-size: small;" Text="Report" 
                                     ValidationGroup="Incoming_Defect_No_By_Account" />
                                 <asp:CompareValidator ID="CompareValidator19" runat="server" 
                                     ControlToCompare="Textbox_From_Date_Incoming_Defect_No_By_Account" 
                                     ControlToValidate="Textbox_To_Date_Incoming_Defect_No_By_Account" 
                                     Display="Dynamic" ErrorMessage="To Date earlier than from date" ForeColor="Red" 
                                     Operator="GreaterThan" ValidationGroup="Incoming_Defect_No_By_Account"></asp:CompareValidator>
                                 </span>
                                 <br />
                             </td>
                         </tr>
                     </caption>
            </table>   
                                             </ContentTemplate>
                             </asp:UpdatePanel>     
            <br />        
    </asp:Panel>
     <ajaxtoolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender_Inc_Defect" runat="server" Collapsed="true" AutoCollapse="false" TargetControlID="Panel_Incoming_Defects"
                        ImageControlId="Pend_Appr_Collapse_Img4"  CollapsedImage="~/Images/ArrowTip_Down.png" ExpandedImage="~/Images/ArrowTip_Parallel.png"  ExpandControlID="LinkButton_Inc_Defect"
             CollapseControlID="LinkButton_Inc_Defect"/>
                 </ContentTemplate>
    </asp:UpdatePanel>
                        <asp:Label ID="Label_Outg_Def_Dashboard_Access" 
        runat="server" ForeColor="Red" 
                            style="font-size: small" Visible="False"></asp:Label>
    <br />
    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
    <ContentTemplate>

                                                            <table width="100%">
                                                          <tr>
                                                                <td background="../Images/menu_bg.gif" >
                                                                <asp:LinkButton ID="LinkButton_Out_Defect" runat="server" 
                                                                        ToolTip="Click to expand or collapse">Outgoing Defects Dashboard</asp:LinkButton>
                                                                    <div style="float: right; vertical-align: middle;">
                                                                    <asp:Image ID="Pend_Appr_Collapse_Img5" runat="server"/>
                                                                    </div>
                                                                    </td>
                                                            </tr>
                                                            </table>
                   <asp:UpdateProgress ID="UpdateProgress_Outg_Def" 
                     AssociatedUpdatePanelID="UpdatePanelOutDefects" runat="server" DisplayAfter="1">
                 <ProgressTemplate>
                 <div style="position:fixed;top:40px;left:35%;z-index:100;width:50%;height:50%">
                 <image style="z-index:105" src="../Images/loading.gif"></image>                 
                 </div>
                 </ProgressTemplate>
                 </asp:UpdateProgress>
    <asp:Panel ID="Panel_Outgoing_Defects" runat="server" BorderColor="#0066CC" 
            BorderStyle="Ridge" BorderWidth="2px" 
            style="font-family: Andalus">        
                                         <asp:UpdatePanel ID="UpdatePanelOutDefects" runat="server" UpdateMode="Conditional" OnLoad="UpdatePanelOutDefects_load">
                                 <ContentTemplate>
                                 <table id="outDefectArrvalClosure" border="0" cellpadding="0" cellspacing="0" width="90%">
                                 <tr>
                                 <td align="left" style="font-size: small; font-weight: 700" width="45%">
                                     <asp:Chart ID="Chart_Out_Defect_Arrval_Closure" runat="server" 
                                         BorderlineColor="Tan" BorderlineDashStyle="Solid" Height="412px" 
                                         ViewStateContent="All" ViewStateMode="Enabled" Width="1126px" 
                                         Palette="EarthTones">
                                         <Series>
                                             <asp:Series ChartType="StackedColumn" Font="Book Antiqua, 8.25pt" Legend="Legend1" 
                                                 Name="Series_Defects_High" ChartArea="ChartArea_New_Defects" 
                                                 CustomProperties="DrawingStyle=Cylinder" LegendText="High Sev Created">
                                             </asp:Series>
                                             <asp:Series ChartArea="ChartArea_New_Defects" ChartType="StackedColumn" 
                                                 CustomProperties="DrawingStyle=Cylinder" Legend="Legend1" 
                                                 Name="Series_Defects_Low" Font="Book Antiqua, 8.25pt" 
                                                 LegendText="Low Sev Created">
                                             </asp:Series>
                                             <asp:Series ChartArea="ChartArea_New_Defects" ChartType="StackedColumn" 
                                                 CustomProperties="DrawingStyle=Cylinder" Legend="Legend1" 
                                                 Name="Series_Defects_Medium" Font="Book Antiqua, 8.25pt" 
                                                 LegendText="Medium Sev Created">
                                             </asp:Series>
                                             <asp:Series ChartArea="ChartArea_Closure" ChartType="StackedColumn" 
                                                 CustomProperties="DrawingStyle=Cylinder, DrawSideBySide=True" 
                                                 Font="Book Antiqua, 8.25pt" Legend="Legend1" Name="Series_Closure_High" 
                                                 LegendText="High Sev Closed">
                                             </asp:Series>
                                             <asp:Series ChartArea="ChartArea_Closure" ChartType="StackedColumn" 
                                                 CustomProperties="DrawingStyle=Cylinder" Legend="Legend1" 
                                                 Name="Series_Closure_Medium" Font="Book Antiqua, 8.25pt" 
                                                 LegendText="Medium Sev Closed">
                                             </asp:Series>
                                             <asp:Series ChartArea="ChartArea_Closure" ChartType="StackedColumn" 
                                                 CustomProperties="DrawingStyle=Cylinder" Legend="Legend1" 
                                                 Name="Series_Closure_Low" Font="Book Antiqua, 8.25pt" 
                                                 LegendText="Low Sev Closed">
                                             </asp:Series>
                                         </Series>
                                         <ChartAreas>
                                             <asp:ChartArea Name="ChartArea_Closure">
                                                 <AxisY IsLabelAutoFit="False">
                                                     <LabelStyle Font="Book Antiqua, 8.25pt" />
                                                     <MajorGrid LineDashStyle="Dot" />
                                                 </AxisY>
                                                 <AxisX IsLabelAutoFit="False">
                                                     <LabelStyle Font="Book Antiqua, 8.25pt" />
                                                 </AxisX>
                                             </asp:ChartArea>
                                             <asp:ChartArea Name="ChartArea_New_Defects">
                                                 <AxisY IsLabelAutoFit="False">
                                                     <LabelStyle Font="Book Antiqua, 8.25pt" />
                                                     <MajorGrid LineDashStyle="Dot" />
                                                 </AxisY>
                                                 <AxisX IsLabelAutoFit="False">
                                                     <LabelStyle Font="Book Antiqua, 8.25pt" />
                                                 </AxisX>
                                             </asp:ChartArea>
                                         </ChartAreas>
                                         <Legends>
                                             <asp:Legend Font="Baskerville Old Face, 9pt" IsTextAutoFit="False" 
                                                 Name="Legend1" TitleFont="Book Antiqua, 8.25pt, style=Bold">
                                             </asp:Legend>
                                         </Legends>
                                         <Titles>
                                             <asp:Title Font="Book Antiqua, 9pt, style=Bold" Name="Defect Arrival" 
                                                 DockedToChartArea="ChartArea_New_Defects" Text="Defect Raised By Severity">
                                             </asp:Title>
                                             <asp:Title DockedToChartArea="ChartArea_Closure" 
                                                 Font="Book Antiqua, 9pt, style=Bold" Name="Defect Closure" 
                                                 Text="Defect Closure By Severity By Vendors ">
                                             </asp:Title>
                                         </Titles>
                                     </asp:Chart>
                             <br />
                                     <span class="style10"><span class="style6">Filter by Defect Submit Date and 
                                     Frequency (Default date range is 6 months):<br /> From Date:</span>
                                     <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_From_Date_Outgoing_Defect_Arrvl_Closure" 
                                         runat="server"  style="font-family: Andalus" 
                                         ValidationGroup="Outgoing_Defect_Arrvl_Closure" ></asp:TextBox>
                                     <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_From_Date_Outgoing_Defect_Arrvl_Closure_CalendarExtender" 
                                         runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton49" 
                                         TargetControlID="TextBox_From_Date_Outgoing_Defect_Arrvl_Closure">
                                     </ajaxtoolkit:CalendarExtender>
                                     <span class="style6">To Date:</span><asp:TextBox 
                                         ID="TextBox_To_Date_Outgoing_Defect_Arrvl_Closure" runat="server" 
                                         class="form-control datepicker-textbox" style="font-family: Andalus" 
                                         ValidationGroup="Outgoing_Defect_Arrvl_Closure" ></asp:TextBox>
                                     <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_To_Date_Outgoing_Defect_Arrvl_Closure_CalendarExtender" 
                                         runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton50" 
                                         TargetControlID="TextBox_To_Date_Outgoing_Defect_Arrvl_Closure">
                                     </ajaxtoolkit:CalendarExtender>
                                     &nbsp; Frequency:&nbsp;<asp:DropDownList 
                                         ID="DropDownList_Outgoing_Defect_Arrival_Closure_Freq" runat="server">
                                     </asp:DropDownList>
                                     &nbsp;<asp:Button ID="Button_Filter_Date_Outgoing_Defect_Arrvl_Closure" class="btn btn-sm btn-success"
                                         runat="server" style="font-family: Andalus; font-size: small;" Text="Show!" 
                                         ValidationGroup="Outgoing_Defect_Arrvl_Closure" 
                                         onclick="Button_Filter_Date_Outgoing_Defect_Arrvl_Closure_Click" />
                                     &nbsp;<asp:Button ID="Button_Report_Outgoing_Defect_Arrival_Closure" class="btn btn-sm btn-success"
                                         runat="server" onclick="Button_Report_Outgoing_Defect_Arrival_Closure_Click" 
                                         style="font-family: Andalus; font-size: small;" Text="Report!" 
                                         ValidationGroup="Outgoing_Defect_Arrvl_Closure" />
                                     &nbsp;<asp:CompareValidator ID="CompareValidator25" runat="server" 
                                         ControlToCompare="TextBox_From_Date_Outgoing_Defect_Arrvl_Closure" 
                                         ControlToValidate="TextBox_To_Date_Outgoing_Defect_Arrvl_Closure" 
                                         Display="Dynamic" ErrorMessage="To Date earlier than from date" ForeColor="Red" 
                                         Operator="GreaterThan" ValidationGroup="Outgoing_Defect_Arrvl_Closure"></asp:CompareValidator>
                                     <br />
                                     </span>
                         </td>
                                 </tr>                                 
                                 </table>
                                 <table id="outDefectAvgClosure" border="0" cellpadding="0" cellspacing="0" width="90%">
                                 <tr>
                                 <td>
                                 
                                     <asp:Chart ID="Chart_Outg_Defect_Closure_Avg_Time" runat="server" 
                                         BorderlineColor="Tan" BorderlineDashStyle="Solid" Height="347px" 
                                         ViewStateContent="All" ViewStateMode="Enabled" Width="1126px">
                                         <Series>
                                             <asp:Series BorderWidth="3" ChartArea="ChartArea_Defect_Closure" 
                                                 ChartType="Line" Color="Red" CustomProperties="DrawingStyle=Cylinder" 
                                                 Font="Book Antiqua, 8.25pt" Legend="Legend1" 
                                                 LegendText="All Sev Avg Closure Time" Name="Series_Defects_Closure_Total">
                                             </asp:Series>
                                             <asp:Series BorderDashStyle="Dot" BorderWidth="2" 
                                                 ChartArea="ChartArea_Defect_Closure" ChartType="Line" 
                                                 CustomProperties="DrawingStyle=Cylinder, DrawSideBySide=True" 
                                                 Font="Book Antiqua, 8.25pt" LabelToolTip="High Severity" Legend="Legend1" 
                                                 LegendText="High Sev Avg Closure Time" Name="Series_Closure_High">
                                             </asp:Series>
                                             <asp:Series BorderDashStyle="Dash" BorderWidth="2" 
                                                 ChartArea="ChartArea_Defect_Closure" ChartType="Line" 
                                                 CustomProperties="DrawingStyle=Cylinder" Font="Book Antiqua, 8.25pt" 
                                                 LabelToolTip="Medium Severity" Legend="Legend1" 
                                                 LegendText="Medium Sev Avg Closure Time" Name="Series_Closure_Medium">
                                             </asp:Series>
                                             <asp:Series BorderWidth="2" ChartArea="ChartArea_Defect_Closure" 
                                                 ChartType="Line" CustomProperties="DrawingStyle=Cylinder" 
                                                 Font="Book Antiqua, 8.25pt" LabelToolTip="Low Severity" Legend="Legend1" 
                                                 LegendText="Low Sev Avg Closure Time" Name="Series_Closure_Low">
                                             </asp:Series>
                                         </Series>
                                         <ChartAreas>
                                             <asp:ChartArea Name="ChartArea_Defect_Closure">
                                                 <AxisY IsLabelAutoFit="False">
                                                     <LabelStyle Font="Book Antiqua, 8.25pt" />
                                                     <MajorGrid LineDashStyle="Dot" />
                                                 </AxisY>
                                                 <AxisX IsLabelAutoFit="False">
                                                     <LabelStyle Font="Book Antiqua, 8.25pt" />
                                                 </AxisX>
                                             </asp:ChartArea>
                                         </ChartAreas>
                                         <Legends>
                                             <asp:Legend Font="Baskerville Old Face, 9pt" IsTextAutoFit="False" 
                                                 Name="Legend1" TitleFont="Book Antiqua, 8.25pt, style=Bold">
                                             </asp:Legend>
                                         </Legends>
                                         <Titles>
                                             <asp:Title DockedToChartArea="ChartArea_Defect_Closure" 
                                                 Font="Book Antiqua, 9pt, style=Bold" Name="Defect Closure Time" 
                                                 Text="Defect Closure Average Time (Hours)">
                                             </asp:Title>
                                         </Titles>
                                     </asp:Chart>
                                     <br />
                                     <span class="style10"><span class="style6">Filter by Defect Submit 
                                     Date,Frequency and Vendor (Default date range is 6 months):<br /> From Date:</span>
                                     <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_From_Date_Outgoing_Defect_Avg_Closure" runat="server" 
                                          style="font-family: Andalus" 
                                         ValidationGroup="Outgoing_Defect_Avg_Closure" ></asp:TextBox>
                                     <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_From_Date_Outgoing_Defect_Avg_Closure_CalendarExtender" 
                                         runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton51" 
                                         TargetControlID="TextBox_From_Date_Outgoing_Defect_Avg_Closure">
                                     </ajaxtoolkit:CalendarExtender>
                                     <span class="style6">To Date:</span><asp:TextBox 
                                         ID="TextBox_To_Date_Outgoing_Defect_Avg_Closure" runat="server" 
                                          style="font-family: Andalus" class="form-control datepicker-textbox"
                                         ValidationGroup="Outgoing_Defect_Avg_Closure" ></asp:TextBox>
                                     <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_To_Date_Outgoing_Defect_Avg_Closure_CalendarExtender" 
                                         runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton52" 
                                         TargetControlID="TextBox_To_Date_Outgoing_Defect_Avg_Closure">
                                     </ajaxtoolkit:CalendarExtender>
                                     &nbsp; Frequency:&nbsp;<asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Outg_Defect_Avg_Closure_Freq" 
                                         runat="server">
                                     </asp:DropDownList>
                                     &nbsp;Select Vendor:&nbsp;<asp:DropDownList 
                                         ID="DropDownList_Outg_Defect_Avg_Closure_Vendor" runat="server">
                                     </asp:DropDownList>
                                     &nbsp;<asp:Button ID="Button_Filter_Date_Outgoing_Defect_Avg_Closure" runat="server" class="btn btn-sm btn-success"
                                         style="font-family: Andalus; font-size: small;" Text="Show!" 
                                         ValidationGroup="Outgoing_Defect_Avg_Closure" 
                                         onclick="Button_Filter_Date_Outgoing_Defect_Avg_Closure_Click" />
                                     &nbsp;<asp:Button ID="Button_Report_Outgoing_Defect_Avg_Closure" runat="server" class="btn btn-sm btn-success"
                                         onclick="Button_Report_Outgoing_Defect_Avg_Closure_Click" 
                                         style="font-family: Andalus; font-size: small;" Text="Report!" 
                                         ValidationGroup="Outgoing_Defect_Avg_Closure" />
                                     &nbsp;<asp:CompareValidator ID="CompareValidator26" runat="server" 
                                         ControlToCompare="TextBox_From_Date_Outgoing_Defect_Avg_Closure" 
                                         ControlToValidate="TextBox_To_Date_Outgoing_Defect_Avg_Closure" 
                                         Display="Dynamic" ErrorMessage="To Date earlier than from date" ForeColor="Red" 
                                         Operator="GreaterThan" ValidationGroup="Outgoing_Defect_Avg_Closure"></asp:CompareValidator>
                                     <br />
                                     </span>
                                 
                                 </td>
                                 </tr>                                 
                                 </table>
             <table id="potTable3" border="0" cellpadding="0" cellspacing="0" width="90%">
                     <tr>
                     <td align="center" width="45%">
                         <br />
                         <asp:Chart ID="Chart_Outgoing_Defect_By_Account" runat="server" 
                             BorderlineColor="Tan" 
                             Width="400px" BorderlineDashStyle="Solid" Palette="EarthTones">
                             <Series>
                                 <asp:Series Name="TotalDefects" BackGradientStyle="TopBottom" 
                                     Font="Book Antiqua, 9pt" IsValueShownAsLabel="True" 
                                     LegendText="Cleared" 
                                     
                                     
                                     CustomProperties="DrawingStyle=Cylinder, DrawSideBySide=True, LabelStyle=Center">
                                 </asp:Series>
                             </Series>
                             <ChartAreas>
                                 <asp:ChartArea Name="ChartArea1" BackColor="LightSteelBlue" 
                                     BackGradientStyle="Center" BorderColor="Gainsboro">
                                     <AxisY TitleFont="Baskerville Old Face, 9.75pt" IsLabelAutoFit="False">
                                         <LabelStyle Font="Book Antiqua, 9.75pt" />
                                     </AxisY>
                                     <AxisX TitleFont="Baskerville Old Face, 9.75pt" IsLabelAutoFit="False">
                                         <LabelStyle Font="Book Antiqua, 9.75pt" />
                                     </AxisX>
                                     <Area3DStyle Enable3D="True" Inclination="10" PointDepth="50" 
                                         PointGapDepth="50" />
                                 </asp:ChartArea>
                             </ChartAreas>
                             <Titles>
                                 <asp:Title Font="Book Antiqua, 8.25pt" Name="Title1">
                                 </asp:Title>
                             </Titles>
                         </asp:Chart>
                         <br />
                         <br />
                         <span class="style10"><span class="style6">Filter by Defect Submit Date and 
                         Vendor (Default date range is 12 months):<br /> From Date:</span></span>
                         <asp:TextBox class="form-control datepicker-textbox" ID="Textbox_From_Date_Outgoing_Defect_By_Account" 
                             runat="server"  style="font-family: Andalus" ValidationGroup="Chart_Outgoing_Defect_By_Account_Group" 
                             ></asp:TextBox>
                         <ajaxtoolkit:CalendarExtender Enabled="false" ID="Textbox_From_Date_Outgoing_Defect_By_Account_CalendarExtender" 
                             runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton43" 
                             TargetControlID="Textbox_From_Date_Outgoing_Defect_By_Account">
                         </ajaxtoolkit:CalendarExtender>
                         <span class="style10">
                         <span class="style6">To Date:</span></span><asp:TextBox 
                             ID="Textbox_To_Date_Outgoing_Defect_By_Account" runat="server" 
                              Height="22px" 
                             style="font-family: Andalus" ValidationGroup="Chart_Outgoing_Defect_By_Account_Group" 
                             Width="101px"></asp:TextBox>
                         <ajaxtoolkit:CalendarExtender Enabled="false" ID="Textbox_To_Date_Outgoing_Defect_By_Account_CalendarExtender" 
                             runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton44" 
                             TargetControlID="Textbox_To_Date_Outgoing_Defect_By_Account">
                         </ajaxtoolkit:CalendarExtender>
                         &nbsp;<span class="style10"><span class="style6">Select Vendor : </span>
                         <asp:ListBox ID="ListBox_Outgoing_Defect_By_Account" runat="server" 
                             Height="57px" SelectionMode="Multiple" 
                             style="font-family: Andalus; font-size: small" Width="180px"></asp:ListBox>
                         </span>
                         <asp:Button ID="Button_Outgoing_Defect_By_Account" runat="server" class="btn btn-sm btn-success"
                             style="font-family: Andalus; font-size: small;" Text="Show!" 
                             ValidationGroup="Chart_Outgoing_Defect_By_Account_Group" 
                             onclick="Button_Outgoing_Defect_By_Account_Click" />
                         <span class="style10">
                         &nbsp;<asp:Button ID="Button_Outgoing_Defect_By_Account_Download" runat="server" class="btn btn-sm btn-success"
                             onclick="Button_Outgoing_Defect_By_Account_Download_Click" 
                             style="font-family: Andalus; font-size: small;" Text="Report" 
                             ValidationGroup="Chart_Outgoing_Defect_By_Account_Group" />
                         <asp:CompareValidator ID="CompareValidator22" runat="server" 
                             ControlToCompare="Textbox_From_Date_Outgoing_Defect_By_Account" 
                             ControlToValidate="Textbox_To_Date_Outgoing_Defect_By_Account" Display="Dynamic" 
                             ErrorMessage="To Date earlier than from date" ForeColor="Red" 
                             Operator="GreaterThan" 
                             ValidationGroup="Chart_Outgoing_Defect_By_Account_Group"></asp:CompareValidator>
                         </span>
                     </td>
                     <td align="center" width="45%">
                                 <asp:Chart ID="Chart_Outgoing_Defect_No_By_Account" runat="server" 
                                     BorderlineColor="Tan" BorderlineDashStyle="Solid" Width="400px" 
                                     Palette="EarthTones">
                                     <Series>
                                         <asp:Series BackGradientStyle="TopBottom" ChartType="StackedColumn" 
                                             CustomProperties="DrawingStyle=Cylinder, DrawSideBySide=True, LabelStyle=Center" 
                                             Font="Book Antiqua, 9pt" IsValueShownAsLabel="True" Legend="Legend_High" 
                                             LegendText="High Sev" Name="HighDefects">
                                         </asp:Series>
                                         <asp:Series ChartArea="ChartArea1" ChartType="StackedColumn" 
                                             CustomProperties="DrawingStyle=Cylinder" Legend="Legend_Low" 
                                             LegendText="Low Sev" Name="LowDefects">
                                         </asp:Series>
                                         <asp:Series ChartArea="ChartArea1" ChartType="StackedColumn" 
                                             CustomProperties="DrawingStyle=Cylinder" Legend="Legend_Medium" 
                                             LegendText="Medium Sev" Name="MediumDefects">
                                         </asp:Series>
                                     </Series>
                                     <ChartAreas>
                                         <asp:ChartArea BackColor="LightSteelBlue" BackGradientStyle="Center" 
                                             BorderColor="Gainsboro" Name="ChartArea1">
                                             <AxisY IsLabelAutoFit="False" TitleFont="Baskerville Old Face, 9.75pt">
                                                 <LabelStyle Font="Book Antiqua, 9.75pt" />
                                             </AxisY>
                                             <AxisX IsLabelAutoFit="False" TitleFont="Baskerville Old Face, 9.75pt">
                                                 <LabelStyle Font="Book Antiqua, 9.75pt" />
                                             </AxisX>
                                             <Area3DStyle Enable3D="True" Inclination="10" PointDepth="50" 
                                                 PointGapDepth="50" />
                                         </asp:ChartArea>
                                     </ChartAreas>
                                     <Legends>
                                         <asp:Legend Alignment="Far" BackColor="White" 
                                             DockedToChartArea="ChartArea1" Docking="Bottom" Font="Book Antiqua, 8.25pt" 
                                             IsTextAutoFit="False" ItemColumnSpacing="10" LegendStyle="Row" 
                                             Name="Legend_High" TitleFont="Book Antiqua, 8pt, style=Bold">
                                         </asp:Legend>
                                         <asp:Legend Alignment="Far" BackColor="White" 
                                             DockedToChartArea="ChartArea1" Docking="Bottom" Font="Book Antiqua, 8.25pt" 
                                             IsTextAutoFit="False" ItemColumnSpacing="10" LegendStyle="Row" 
                                             Name="Legend_Low" TitleFont="Book Antiqua, 8pt, style=Bold">
                                         </asp:Legend>
                                         <asp:Legend Alignment="Far" BackColor="White" 
                                             DockedToChartArea="ChartArea1" Docking="Bottom" Font="Book Antiqua, 8.25pt" 
                                             IsTextAutoFit="False" ItemColumnSpacing="10" LegendStyle="Row" 
                                             Name="Legend_Medium" TitleFont="Book Antiqua, 8pt, style=Bold">
                                         </asp:Legend>
                                     </Legends>
                                     <Titles>
                                         <asp:Title Font="Book Antiqua, 8.25pt" Name="Title1">
                                         </asp:Title>
                                     </Titles>
                                 </asp:Chart>
                         <br />
                         <span class="style10"><span class="style6">
                                 <br />
                                 Filter by Defect Submit Date,Type and Vendor(Default date range is 12 months):<br /> 
                                 From Date:</span></span>
                         <asp:TextBox class="form-control datepicker-textbox" ID="Textbox_From_Date_Outgoing_Defect_No_By_Account" 
                             runat="server"  style="font-family: Andalus" 
                             ValidationGroup="Outgoing_Defect_No_By_Account"  ReadOnly="True"></asp:TextBox>
                         <ajaxtoolkit:CalendarExtender Enabled="false" ID="Textbox_From_Date_Outgoing_Defect_No_By_Account_CalendarExtender" 
                             runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton45" 
                             TargetControlID="Textbox_From_Date_Outgoing_Defect_No_By_Account">
                         </ajaxtoolkit:CalendarExtender>
                         <span class="style10">
                         <span class="style6">To Date:</span></span><asp:TextBox 
                             ID="Textbox_To_Date_Outgoing_Defect_No_By_Account" runat="server" 
                              style="font-family: Andalus" class="form-control datepicker-textbox"
                             ValidationGroup="Outgoing_Defect_No_By_Account"  ReadOnly="True"></asp:TextBox>
                         <ajaxtoolkit:CalendarExtender Enabled="false" ID="Textbox_To_Date_Outgoing_Defect_No_By_Account_CalendarExtender" 
                             runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton46" 
                             TargetControlID="Textbox_To_Date_Outgoing_Defect_No_By_Account">
                         </ajaxtoolkit:CalendarExtender>
                                 &nbsp;<span class="style10"><span class="style6">Select Vendor : </span>
                         <asp:ListBox ID="Listbox_Outgoing_Defect_No_By_Account" runat="server" 
                             Height="57px" SelectionMode="Multiple" 
                             style="font-family: Andalus; font-size: small" Width="180px"></asp:ListBox>
                         &nbsp;</span><asp:Button ID="Button_Outgoing_Defect_No_By_Account" class="btn btn-sm btn-success"
                             runat="server" 
                             style="font-family: Andalus; font-size: small;" Text="Show!" 
                             ValidationGroup="Outgoing_Defect_No_By_Account" 
                             onclick="Button_Outgoing_Defect_No_By_Account_Click" />
                         <span class="style10">
                         &nbsp;
                         <asp:Button ID="Button_Outgoing_Defect_No_By_Account_Download" runat="server" class="btn btn-sm btn-success"
                             onclick="Button_Outgoing_Defect_No_By_Account_Download_Click" 
                             style="font-family: Andalus; font-size: small;" Text="Report" 
                             ValidationGroup="Outgoing_Defect_No_By_Account" />
                         <asp:CompareValidator ID="CompareValidator23" runat="server" 
                             ControlToCompare="Textbox_From_Date_Outgoing_Defect_No_By_Account" 
                             ControlToValidate="Textbox_To_Date_Outgoing_Defect_No_By_Account" 
                             Display="Dynamic" ErrorMessage="To Date earlier than from date" ForeColor="Red" 
                             Operator="GreaterThan" ValidationGroup="Outgoing_Defect_No_By_Account"></asp:CompareValidator>
                         </span>
                         <br />
                     </td>
                     </tr>
            </table>        
                                             </ContentTemplate>
                             </asp:UpdatePanel>
            <br />        
    </asp:Panel>
       <ajaxtoolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender_Out_Defect" runat="server" Collapsed="true" AutoCollapse="false" TargetControlID="Panel_Outgoing_Defects"
                        ImageControlId="Pend_Appr_Collapse_Img5"  CollapsedImage="~/Images/ArrowTip_Down.png" ExpandedImage="~/Images/ArrowTip_Parallel.png"  ExpandControlID="LinkButton_Out_Defect"
             CollapseControlID="LinkButton_Out_Defect"/>
                 </ContentTemplate>
    </asp:UpdatePanel>

                            <asp:Label ID="Label_Incm_SR_Dashboard_Access" 
        runat="server" ForeColor="Red" 
                            style="font-size: small" Visible="False"></asp:Label>
    <br />
    <asp:UpdatePanel ID="UpdatePanel7" runat="server">
    <ContentTemplate>

                                                                <table width="100%">
                                                          <tr>
                                                                <td background="../Images/menu_bg.gif" >
                                                                <asp:LinkButton ID="LinkButton_Inc_SR" runat="server" 
                                                                        ToolTip="Click to expand or collapse">Incoming SRs Dashboard</asp:LinkButton>
                                                                    <div style="float: right; vertical-align: middle;">
                                                                    <asp:Image ID="Incm_SR_Collapse_Img4" runat="server"/>
                                                                    </div>
                                                                    </td>
                                                            </tr>
                                                            </table>
                 <asp:UpdateProgress ID="UpdateProgress_Incm_SR" 
                     AssociatedUpdatePanelID="UpdatePanelIncmSRs" runat="server" DisplayAfter="1">
                 <ProgressTemplate>
                 <div style="position:fixed;top:40px;left:35%;z-index:100;width:50%;height:50%">
                 <image style="z-index:105" src="../Images/loading.gif"></image>                 
                 </div>
                 </ProgressTemplate>
                 </asp:UpdateProgress>
    <asp:Panel ID="Panel_Incoming_SRs" runat="server" BorderColor="#0066CC" 
            BorderStyle="Ridge" BorderWidth="2px"
            style="font-family: Andalus">     
                                <asp:UpdatePanel ID="UpdatePanelIncmSRs" runat="server" UpdateMode="Conditional" OnLoad="UpdatePanelIncmSRs_load">
                                 <ContentTemplate>   
             <table id="Table1" border="0" cellpadding="0" cellspacing="0" width="90%">
                     <tr>
                         <td align="left" style="font-size: small; font-weight: 700" width="45%">
                                     <asp:Chart ID="Chart_SR_Arrival_Closure" runat="server" 
                                         BorderlineColor="Tan" BorderlineDashStyle="Solid" Height="412px" 
                                         ViewStateContent="All" ViewStateMode="Enabled" Width="1126px" 
                                         Palette="EarthTones">
                                         <Series>
                                             <asp:Series ChartType="StackedColumn" Font="Book Antiqua, 8.25pt" Legend="Legend1" 
                                                 Name="Series_SRs_High" ChartArea="ChartArea_New_SRs" 
                                                 CustomProperties="DrawingStyle=Cylinder" LegendText="High Sev Created">
                                             </asp:Series>
                                             <asp:Series ChartArea="ChartArea_New_SRs" ChartType="StackedColumn" 
                                                 CustomProperties="DrawingStyle=Cylinder" Legend="Legend1" 
                                                 Name="Series_SRs_Low" Font="Book Antiqua, 8.25pt" 
                                                 LegendText="Low Sev Created">
                                             </asp:Series>
                                             <asp:Series ChartArea="ChartArea_New_SRs" ChartType="StackedColumn" 
                                                 CustomProperties="DrawingStyle=Cylinder" Legend="Legend1" 
                                                 Name="Series_SRs_Medium" Font="Book Antiqua, 8.25pt" 
                                                 LegendText="Medium Sev Created">
                                             </asp:Series>
                                             <asp:Series ChartArea="ChartArea_Closure" ChartType="StackedColumn" 
                                                 CustomProperties="DrawingStyle=Cylinder, DrawSideBySide=True" 
                                                 Font="Book Antiqua, 8.25pt" Legend="Legend1" Name="Series_Closure_High" 
                                                 LegendText="High Sev Closed">
                                             </asp:Series>
                                             <asp:Series ChartArea="ChartArea_Closure" ChartType="StackedColumn" 
                                                 CustomProperties="DrawingStyle=Cylinder" Legend="Legend1" 
                                                 Name="Series_Closure_Medium" Font="Book Antiqua, 8.25pt" 
                                                 LegendText="Medium Sev Closed">
                                             </asp:Series>
                                             <asp:Series ChartArea="ChartArea_Closure" ChartType="StackedColumn" 
                                                 CustomProperties="DrawingStyle=Cylinder" Legend="Legend1" 
                                                 Name="Series_Closure_Low" Font="Book Antiqua, 8.25pt" 
                                                 LegendText="Low Sev Closed">
                                             </asp:Series>
                                         </Series>
                                         <ChartAreas>
                                             <asp:ChartArea Name="ChartArea_Closure">
                                                 <AxisY IsLabelAutoFit="False">
                                                     <LabelStyle Font="Book Antiqua, 8.25pt" />
                                                     <MajorGrid LineDashStyle="Dot" />
                                                 </AxisY>
                                                 <AxisX IsLabelAutoFit="False">
                                                     <LabelStyle Font="Book Antiqua, 8.25pt" />
                                                 </AxisX>
                                             </asp:ChartArea>
                                             <asp:ChartArea Name="ChartArea_New_SRs">
                                                 <AxisY IsLabelAutoFit="False">
                                                     <LabelStyle Font="Book Antiqua, 8.25pt" />
                                                     <MajorGrid LineDashStyle="Dot" />
                                                 </AxisY>
                                                 <AxisX IsLabelAutoFit="False">
                                                     <LabelStyle Font="Book Antiqua, 8.25pt" />
                                                 </AxisX>
                                             </asp:ChartArea>
                                         </ChartAreas>
                                         <Legends>
                                             <asp:Legend Font="Baskerville Old Face, 9pt" IsTextAutoFit="False" 
                                                 Name="Legend1" TitleFont="Book Antiqua, 8.25pt, style=Bold">
                                             </asp:Legend>
                                         </Legends>
                                         <Titles>
                                             <asp:Title Font="Book Antiqua, 9pt, style=Bold" Name="SR Arrival" 
                                                 DockedToChartArea="ChartArea_New_SRs" Text="SR Arrival By Severity">
                                             </asp:Title>
                                             <asp:Title DockedToChartArea="ChartArea_Closure" 
                                                 Font="Book Antiqua, 9pt, style=Bold" Name="SR Closure" 
                                                 Text="SR Closure By Severity">
                                             </asp:Title>
                                         </Titles>
                                     </asp:Chart>
                             <br />
                             <span class="style10">
                             <span class="style6">Filter by SR Submit Date and Frequency (SRault date range is 6 
                                     months):<br /> From Date:</span>
                             <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_From_Date_Incoming_SR_Arrvl_Closure" runat="server" 
                                  style="font-family: Andalus" 
                                 ValidationGroup="Incoming_SR_Arrvl_Closure" ></asp:TextBox>
                             <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_From_Date_Incoming_SR_Arrvl_Closure_CalendarExtender" 
                                 runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton_Incoming_SR_Arrvl_Closure" 
                                 TargetControlID="TextBox_From_Date_Incoming_SR_Arrvl_Closure">
                             </ajaxtoolkit:CalendarExtender>
                             <span class="style6">To Date:</span><asp:TextBox 
                                 ID="TextBox_To_Date_Incoming_SR_Arrvl_Closure" runat="server" class="form-control datepicker-textbox"
                                  style="font-family: Andalus" ValidationGroup="Incoming_SR_Arrvl_Closure" 
                                 ></asp:TextBox>
                             <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_To_Date_Incoming_SR_Arrvl_Closure_CalendarExtender" 
                                 runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton_To_Date_Incoming_SR_Arrvl_Closure" 
                                 TargetControlID="TextBox_To_Date_Incoming_SR_Arrvl_Closure">
                             </ajaxtoolkit:CalendarExtender>
                                     &nbsp; Frequency:&nbsp;<asp:DropDownList 
                                         ID="DropDownList_Incm_SR_Arrival_Closure_Freq" runat="server">
                                     </asp:DropDownList>
                             &nbsp;<asp:Button ID="Button_Filter_Date_Incoming_SR_Arrvl_Closure" runat="server" class="btn btn-sm btn-success"
                                         onclick="Button_Filter_Date_Incoming_SR_Arrvl_Closure_Click" 
                                         style="font-family: Andalus; font-size: small;" Text="Show!" 
                                         ValidationGroup="Incoming_SR_Arrvl_Closure" />
                                     &nbsp;<asp:Button ID="Button_Report_Date_Incoming_SR_Arrival_Closure" class="btn btn-sm btn-success"
                                         runat="server" 
                                         onclick="Button_Report_Date_Incoming_SR_Arrival_Closure_Click" 
                                         style="font-family: Andalus; font-size: small;" Text="Report!" 
                                         ValidationGroup="Incoming_SR_Arrvl_Closure" />
                                     &nbsp;<asp:CompareValidator ID="CompareValidator1" runat="server" 
                                 ControlToCompare="TextBox_From_Date_Incoming_SR_Arrvl_Closure" 
                                 ControlToValidate="TextBox_To_Date_Incoming_SR_Arrvl_Closure" Display="Dynamic" 
                                 ErrorMessage="To Date earlier than from date" ForeColor="Red" 
                                 Operator="GreaterThan" ValidationGroup="Incoming_SR_Arrvl_Closure"></asp:CompareValidator>
                             <br />
                             </span>
                         </td>
                     </tr>
                 </table>
                 <table id="IncmSRClsTime" border="0" cellpadding="0" cellspacing="0" width="90%">
                 <tr>
                 <td>
                     <asp:Chart ID="Chart_Incm_SR_Closure_Avg_Time" runat="server" 
                         BorderlineColor="Tan" BorderlineDashStyle="Solid" Height="347px" 
                         ViewStateContent="All" ViewStateMode="Enabled" Width="1126px">
                         <Series>
                             <asp:Series ChartArea="ChartArea_SR_Closure" ChartType="Line" 
                                 CustomProperties="DrawingStyle=Cylinder" Font="Book Antiqua, 8.25pt" 
                                 Legend="Legend1" LegendText="All Sev Avg Closure Time" 
                                 Name="Series_SRs_Closure_Total" BorderWidth="3" Color="Red">
                             </asp:Series>
                             <asp:Series ChartArea="ChartArea_SR_Closure" ChartType="Line" 
                                 CustomProperties="DrawingStyle=Cylinder, DrawSideBySide=True" 
                                 Font="Book Antiqua, 8.25pt" Legend="Legend1" 
                                 LegendText="High Sev Avg Closure Time" Name="Series_Closure_High" 
                                 BorderDashStyle="Dot" BorderWidth="2" LabelToolTip="High Severity">
                             </asp:Series>
                             <asp:Series ChartArea="ChartArea_SR_Closure" ChartType="Line" 
                                 CustomProperties="DrawingStyle=Cylinder" Font="Book Antiqua, 8.25pt" 
                                 Legend="Legend1" LegendText="Medium Sev Avg Closure Time" 
                                 Name="Series_Closure_Medium" BorderDashStyle="Dash" BorderWidth="2" 
                                 LabelToolTip="Medium Severity">
                             </asp:Series>
                             <asp:Series ChartArea="ChartArea_SR_Closure" ChartType="Line" 
                                 CustomProperties="DrawingStyle=Cylinder" Font="Book Antiqua, 8.25pt" 
                                 Legend="Legend1" LegendText="Low Sev Avg Closure Time" 
                                 Name="Series_Closure_Low" BorderWidth="2" LabelToolTip="Low Severity">
                             </asp:Series>
                         </Series>
                         <ChartAreas>
                             <asp:ChartArea Name="ChartArea_SR_Closure">
                                 <AxisY IsLabelAutoFit="False">
                                     <LabelStyle Font="Book Antiqua, 8.25pt" />
                                     <MajorGrid LineDashStyle="Dot" />
                                 </AxisY>
                                 <AxisX IsLabelAutoFit="False">
                                     <LabelStyle Font="Book Antiqua, 8.25pt" />
                                 </AxisX>
                             </asp:ChartArea>
                         </ChartAreas>
                         <Legends>
                             <asp:Legend Font="Baskerville Old Face, 9pt" IsTextAutoFit="False" 
                                 Name="Legend1" TitleFont="Book Antiqua, 8.25pt, style=Bold">
                             </asp:Legend>
                         </Legends>
                         <Titles>
                             <asp:Title DockedToChartArea="ChartArea_SR_Closure" 
                                 Font="Book Antiqua, 9pt, style=Bold" Name="SR Closure Time" 
                                 Text="SR Closure Average Time (Hours)">
                             </asp:Title>
                         </Titles>
                     </asp:Chart>
                     <br />
                     <span class="style10"><span class="style6">Filter by SR Submit 
                     Date,Frequency and Service Agent (SRault date range is 6 months):<br /> From 
                     Date:</span>
                     <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_From_Date_Incoming_SR_Avg_Closure" runat="server" 
                          style="font-family: Andalus" 
                         ValidationGroup="Incoming_SR_Avg_Closure" ></asp:TextBox>
                     <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_From_Date_Incoming_SR_Avg_Closure_CalendarExtender" 
                         runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton_From_Date_Incoming_SR_Avg_Closure" 
                         TargetControlID="TextBox_From_Date_Incoming_SR_Avg_Closure">
                     </ajaxtoolkit:CalendarExtender>
                     <span class="style6">To Date:</span><asp:TextBox 
                         ID="TextBox_To_Date_Incoming_SR_Avg_Closure" runat="server" 
                          style="font-family: Andalus" class="form-control datepicker-textbox" 
                         ValidationGroup="Incoming_SR_Avg_Closure" ></asp:TextBox>
                     <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_To_Date_Incoming_SR_Avg_Closure_CalendarExtender" 
                         runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton_To_Date_Incoming_SR_Avg_Closure" 
                         TargetControlID="TextBox_To_Date_Incoming_SR_Avg_Closure">
                     </ajaxtoolkit:CalendarExtender>
                     &nbsp; Frequency:&nbsp;<asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Incm_SR_Avg_Closure_Freq" 
                         runat="server">
                     </asp:DropDownList>
                     &nbsp;Select Service Agent:&nbsp;<asp:DropDownList 
                         ID="DropDownList_Incm_SR_Avg_Closure_Service_Agnt" runat="server">
                     </asp:DropDownList>
                     &nbsp;<asp:Button ID="Button_Filter_Date_Incoming_SR_Avg_Closure" runat="server" class="btn btn-sm btn-success"
                         style="font-family: Andalus; font-size: small;" Text="Show!" 
                         ValidationGroup="Incoming_SR_Avg_Closure" 
                         onclick="Button_Filter_Date_Incoming_SR_Avg_Closure_Click" />
                     &nbsp;<asp:Button ID="Button_Report_Date_Incoming_SR_Avg_Closure" runat="server" class="btn btn-sm btn-success"
                         onclick="Button_Report_Date_Incoming_SR_Avg_Closure_Click" 
                         style="font-family: Andalus; font-size: small;" Text="Report!" 
                         ValidationGroup="Incoming_SR_Avg_Closure" />
                     <asp:CompareValidator ID="CompareValidator14" runat="server" 
                         ControlToCompare="TextBox_From_Date_Incoming_SR_Avg_Closure" 
                         ControlToValidate="TextBox_To_Date_Incoming_SR_Avg_Closure" 
                         Display="Dynamic" ErrorMessage="To Date earlier than from date" ForeColor="Red" 
                         Operator="GreaterThan" ValidationGroup="Incoming_SR_Avg_Closure"></asp:CompareValidator>
                     <br />
                     </span>
                 </td>
                 </tr>
                 </table>
                 <table id="IncmSR" border="0" cellpadding="0" cellspacing="0" width="90%">
                     <caption>
                         &nbsp;<tr>
                             <td align="center" width="45%">                                 
                                 <asp:Chart ID="Chart_Incoming_SR_By_Account" runat="server" 
                                     BorderlineColor="Tan" BorderlineDashStyle="Solid" Width="400px" 
                                     Palette="EarthTones">
                                     <Series>
                                         <asp:Series BackGradientStyle="TopBottom" 
                                             CustomProperties="DrawingStyle=Cylinder, DrawSideBySide=True, LabelStyle=Center" 
                                             Font="Book Antiqua, 9pt" IsValueShownAsLabel="True" LegendText="Cleared" 
                                             Name="TotalSRs">
                                         </asp:Series>
                                     </Series>
                                     <ChartAreas>
                                         <asp:ChartArea BackColor="LightSteelBlue" BackGradientStyle="Center" 
                                             BorderColor="Gainsboro" Name="ChartArea1">
                                             <AxisY IsLabelAutoFit="False" TitleFont="Baskerville Old Face, 9.75pt">
                                                 <LabelStyle Font="Book Antiqua, 9.75pt" />
                                             </AxisY>
                                             <AxisX IsLabelAutoFit="False" TitleFont="Baskerville Old Face, 9.75pt">
                                                 <LabelStyle Font="Book Antiqua, 9.75pt" />
                                             </AxisX>
                                             <Area3DStyle Enable3D="True" Inclination="10" PointDepth="50" 
                                                 PointGapDepth="50" />
                                         </asp:ChartArea>
                                     </ChartAreas>
                                     <Titles>
                                         <asp:Title Font="Book Antiqua, 8.25pt" Name="Title1">
                                         </asp:Title>
                                     </Titles>
                                 </asp:Chart>
                                 <br />
                                 <br />
                                 <span class="style10"><span class="style6">Filter by SR Submit Date and 
                                 Customer (SRault date range is 12 months):<br /> From Date:</span></span>
                                 <asp:TextBox class="form-control datepicker-textbox" ID="Textbox_From_Date_Incoming_SR_By_Account" runat="server" 
                                      style="font-family: Andalus" 
                                     ValidationGroup="Chart_Incoming_SR_By_Account_Group" ></asp:TextBox>
                                 <ajaxtoolkit:CalendarExtender Enabled="false" ID="Textbox_From_Date_Incoming_SR_By_Account_CalendarExtender" 
                                     runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton_From_Date_Incoming_SR_By_Account" 
                                     TargetControlID="Textbox_From_Date_Incoming_SR_By_Account">
                                 </ajaxtoolkit:CalendarExtender>
                                 <span class="style10">
                                 <span class="style6">To Date:</span></span><asp:TextBox 
                                     ID="Textbox_To_Date_Incoming_SR_By_Account" runat="server" 
                                      style="font-family: Andalus" class="form-control datepicker-textbox" 
                                     ValidationGroup="Chart_Incoming_SR_By_Account_Group" ></asp:TextBox>
                                 <ajaxtoolkit:CalendarExtender Enabled="false" ID="Textbox_To_Date_Incoming_SR_By_Account_CalendarExtender" 
                                     runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton_To_Date_Incoming_SR_By_Account" 
                                     TargetControlID="Textbox_To_Date_Incoming_SR_By_Account">
                                 </ajaxtoolkit:CalendarExtender>
                                 &nbsp;<span class="style10"><span class="style6">Select customer : </span>
                                 <asp:ListBox ID="ListBox_Incoming_SR_By_Account" runat="server" 
                                     Height="57px" SelectionMode="Multiple" 
                                     style="font-family: Andalus; font-size: small" Width="180px"></asp:ListBox>
                                 </span>
                                 <asp:Button ID="Button_Incoming_SR_By_Account" runat="server" class="btn btn-sm btn-success"
                                     onclick="Button_Incoming_SR_By_Account_Click" 
                                     style="font-family: Andalus; font-size: small;" Text="Show!" 
                                     ValidationGroup="Chart_Incoming_SR_By_Account_Group" />
                                 <span class="style10">&nbsp;<asp:Button 
                                     ID="Button_Incoming_SR_By_Account_Download" runat="server" class="btn btn-sm btn-success"
                                     onclick="Button_Incoming_SR_By_Account_Download_Click" 
                                     style="font-family: Andalus; font-size: small;" Text="Report" 
                                     ValidationGroup="Chart_Incoming_SR_By_Account_Group" />
                                 <asp:CompareValidator ID="CompareValidator17" runat="server" 
                                     ControlToCompare="Textbox_From_Date_Incoming_SR_By_Account" 
                                     ControlToValidate="Textbox_To_Date_Incoming_SR_By_Account" 
                                     Display="Dynamic" ErrorMessage="To Date earlier than from date" ForeColor="Red" 
                                     Operator="GreaterThan" ValidationGroup="Chart_Incoming_SR_By_Account_Group"></asp:CompareValidator>
                                 </span>
                             </td>
                             <td align="center" width="45%">
                             <br />
                                 <asp:Chart ID="Chart_Incoming_SR_No_By_Account" runat="server" 
                                     BorderlineColor="Tan" BorderlineDashStyle="Solid" Width="400px" 
                                     Palette="EarthTones">
                                     <Series>
                                         <asp:Series BackGradientStyle="TopBottom" ChartType="StackedColumn" 
                                             CustomProperties="DrawingStyle=Cylinder, DrawSideBySide=True, LabelStyle=Center" 
                                             Font="Book Antiqua, 9pt" IsValueShownAsLabel="True" Legend="Legend_High" 
                                             LegendText="High Sev" Name="HighSRs">
                                         </asp:Series>
                                         <asp:Series ChartArea="ChartArea1" ChartType="StackedColumn" 
                                             CustomProperties="DrawingStyle=Cylinder" Legend="Legend_Low" 
                                             LegendText="Low Sev" Name="LowSRs">
                                         </asp:Series>
                                         <asp:Series ChartArea="ChartArea1" ChartType="StackedColumn" 
                                             CustomProperties="DrawingStyle=Cylinder" Legend="Legend_Medium" 
                                             LegendText="Medium Sev" Name="MediumSRs">
                                         </asp:Series>
                                     </Series>
                                     <ChartAreas>
                                         <asp:ChartArea BackColor="LightSteelBlue" BackGradientStyle="Center" 
                                             BorderColor="Gainsboro" Name="ChartArea1">
                                             <AxisY IsLabelAutoFit="False" TitleFont="Baskerville Old Face, 9.75pt">
                                                 <LabelStyle Font="Book Antiqua, 9.75pt" />
                                             </AxisY>
                                             <AxisX IsLabelAutoFit="False" TitleFont="Baskerville Old Face, 9.75pt">
                                                 <LabelStyle Font="Book Antiqua, 9.75pt" />
                                             </AxisX>
                                             <Area3DStyle Enable3D="True" Inclination="10" PointDepth="50" 
                                                 PointGapDepth="50" />
                                         </asp:ChartArea>
                                     </ChartAreas>
                                     <Legends>
                                         <asp:Legend Alignment="Far" BackColor="White" 
                                             DockedToChartArea="ChartArea1" Docking="Bottom" Font="Book Antiqua, 8.25pt" 
                                             IsTextAutoFit="False" ItemColumnSpacing="10" LegendStyle="Row" 
                                             Name="Legend_High" TitleFont="Book Antiqua, 8pt, style=Bold">
                                         </asp:Legend>
                                         <asp:Legend Alignment="Far" BackColor="White" 
                                             DockedToChartArea="ChartArea1" Docking="Bottom" Font="Book Antiqua, 8.25pt" 
                                             IsTextAutoFit="False" ItemColumnSpacing="10" LegendStyle="Row" 
                                             Name="Legend_Low" TitleFont="Book Antiqua, 8pt, style=Bold">
                                         </asp:Legend>
                                         <asp:Legend Alignment="Far" BackColor="White" 
                                             DockedToChartArea="ChartArea1" Docking="Bottom" Font="Book Antiqua, 8.25pt" 
                                             IsTextAutoFit="False" ItemColumnSpacing="10" LegendStyle="Row" 
                                             Name="Legend_Medium" TitleFont="Book Antiqua, 8pt, style=Bold">
                                         </asp:Legend>
                                     </Legends>
                                     <Titles>
                                         <asp:Title Font="Book Antiqua, 8.25pt" Name="Title1">
                                         </asp:Title>
                                     </Titles>
                                 </asp:Chart>
                                 <br />
                                 <br />
                                 <span class="style10"><span class="style6">Filter by SR Submit Date,Type and 
                                 Customer(SRault date range is 12 months):<br /> From Date:</span></span>
                                 <asp:TextBox class="form-control datepicker-textbox" ID="Textbox_From_Date_Incoming_SR_No_By_Account" 
                                     runat="server"  style="font-family: Andalus" 
                                     ValidationGroup="Incoming_SR_No_By_Account" ></asp:TextBox>
                                 <ajaxtoolkit:CalendarExtender Enabled="false" ID="Textbox_From_Date_Incoming_SR_No_By_Account_CalendarExtender" 
                                     runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton_From_Date_Incoming_SR_No_By_Account" 
                                     TargetControlID="Textbox_From_Date_Incoming_SR_No_By_Account">
                                 </ajaxtoolkit:CalendarExtender>
                                 <span class="style10">
                                 <span class="style6">To Date:</span></span><asp:TextBox 
                                     ID="Textbox_To_Date_Incoming_SR_No_By_Account" runat="server" 
                                     class="form-control datepicker-textbox" style="font-family: Andalus" 
                                     ValidationGroup="Incoming_SR_No_By_Account" ></asp:TextBox>
                                 <ajaxtoolkit:CalendarExtender Enabled="false" ID="Textbox_To_Date_Incoming_SR_No_By_Account_CalendarExtender" 
                                     runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton_To_Date_Incoming_SR_No_By_Account" 
                                     TargetControlID="Textbox_To_Date_Incoming_SR_No_By_Account">
                                 </ajaxtoolkit:CalendarExtender>
                                 &nbsp;<span class="style10"><span class="style6">SR Types:
                                 <asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Incoming_SR_No_By_Account_SR_Type" 
                                     runat="server">
                                 </asp:DropDownList>
                                 &nbsp;Select customer : </span>
                                 <asp:ListBox ID="Listbox_Incoming_SR_No_By_Account" runat="server" 
                                     Height="57px" SelectionMode="Multiple" 
                                     style="font-family: Andalus; font-size: small" Width="180px"></asp:ListBox>
                                 &nbsp;</span><asp:Button ID="Button_Incoming_SR_No_By_Account" runat="server" class="btn btn-sm btn-success"
                                     onclick="Button_Incoming_SR_No_By_Account_Click" 
                                     style="font-family: Andalus; font-size: small;" Text="Show!" 
                                     ValidationGroup="Incoming_SR_No_By_Account" />
                                 <span class="style10">&nbsp;<asp:Button 
                                     ID="Button_Incoming_SR_No_By_Account_Download" runat="server" class="btn btn-sm btn-success"
                                     onclick="Button_Incoming_SR_No_By_Account_Download_Click" 
                                     style="font-family: Andalus; font-size: small;" Text="Report" 
                                     ValidationGroup="Incoming_SR_No_By_Account" />
                                 <asp:CompareValidator ID="CompareValidator20" runat="server" 
                                     ControlToCompare="Textbox_From_Date_Incoming_SR_No_By_Account" 
                                     ControlToValidate="Textbox_To_Date_Incoming_SR_No_By_Account" 
                                     Display="Dynamic" ErrorMessage="To Date earlier than from date" ForeColor="Red" 
                                     Operator="GreaterThan" ValidationGroup="Incoming_SR_No_By_Account"></asp:CompareValidator>
                                 </span>
                                 <br />
                             </td>
                         </tr>
                     </caption>
            </table>   
                                             </ContentTemplate>
                             </asp:UpdatePanel>     
            <br />        
    </asp:Panel>
     <ajaxtoolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender_Inc_SR" runat="server" Collapsed="true" AutoCollapse="false" TargetControlID="Panel_Incoming_SRs"
                        ImageControlId="Incm_SR_Collapse_Img4"  CollapsedImage="~/Images/ArrowTip_Down.png" ExpandedImage="~/Images/ArrowTip_Parallel.png"  ExpandControlID="LinkButton_Inc_SR"
             CollapseControlID="LinkButton_Inc_SR"/>
                 </ContentTemplate>
    </asp:UpdatePanel>
    <br />
        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
    <ContentTemplate>

                                                                <table width="100%">
                                                          <tr>
                                                                <td background="../Images/menu_bg.gif" >
                                                                <asp:LinkButton ID="LinkButton_Outg_SR" runat="server" 
                                                                        ToolTip="Click to expand or collapse">Outgoing SRs Dashboard</asp:LinkButton>
                                                                    <div style="float: right; vertical-align: middle;">
                                                                    <asp:Image ID="Outg_SR_Collapse_Img4" runat="server"/>
                                                                    </div>
                                                                    </td>
                                                            </tr>
                                                            </table>
                 <asp:UpdateProgress ID="UpdateProgress_Outg_SR" 
                     AssociatedUpdatePanelID="UpdatePanelOutgSRs" runat="server" DisplayAfter="1">
                 <ProgressTemplate>
                 <div style="position:fixed;top:40px;left:35%;z-index:100;width:50%;height:50%">
                 <image style="z-index:105" src="../Images/loading.gif"></image>                 
                 </div>
                 </ProgressTemplate>
                 </asp:UpdateProgress>
    <asp:Panel ID="Panel_Outgoing_SRs" runat="server" BorderColor="#0066CC" 
            BorderStyle="Ridge" BorderWidth="2px"
            style="font-family: Andalus">     
                                <asp:UpdatePanel ID="UpdatePanelOutgSRs" runat="server" UpdateMode="Conditional" OnLoad="UpdatePanelOutgSRs_load">
                                 <ContentTemplate>   
             <table id="Table2" border="0" cellpadding="0" cellspacing="0" width="90%">
                     <tr>
                         <td align="left" style="font-size: small; font-weight: 700" width="45%">
                                     <asp:Chart ID="Chart_Out_SR_Arrval_Closure" runat="server" 
                                         BorderlineColor="Tan" BorderlineDashStyle="Solid" Height="412px" 
                                         ViewStateContent="All" ViewStateMode="Enabled" Width="1126px" 
                                         Palette="EarthTones">
                                         <Series>
                                             <asp:Series ChartType="StackedColumn" Font="Book Antiqua, 8.25pt" Legend="Legend1" 
                                                 Name="Series_SRs_High" ChartArea="ChartArea_New_SRs" 
                                                 CustomProperties="DrawingStyle=Cylinder" LegendText="High Sev Created">
                                             </asp:Series>
                                             <asp:Series ChartArea="ChartArea_New_SRs" ChartType="StackedColumn" 
                                                 CustomProperties="DrawingStyle=Cylinder" Legend="Legend1" 
                                                 Name="Series_SRs_Low" Font="Book Antiqua, 8.25pt" 
                                                 LegendText="Low Sev Created">
                                             </asp:Series>
                                             <asp:Series ChartArea="ChartArea_New_SRs" ChartType="StackedColumn" 
                                                 CustomProperties="DrawingStyle=Cylinder" Legend="Legend1" 
                                                 Name="Series_SRs_Medium" Font="Book Antiqua, 8.25pt" 
                                                 LegendText="Medium Sev Created">
                                             </asp:Series>
                                             <asp:Series ChartArea="ChartArea_Closure" ChartType="StackedColumn" 
                                                 CustomProperties="DrawingStyle=Cylinder, DrawSideBySide=True" 
                                                 Font="Book Antiqua, 8.25pt" Legend="Legend1" Name="Series_Closure_High" 
                                                 LegendText="High Sev Closed">
                                             </asp:Series>
                                             <asp:Series ChartArea="ChartArea_Closure" ChartType="StackedColumn" 
                                                 CustomProperties="DrawingStyle=Cylinder" Legend="Legend1" 
                                                 Name="Series_Closure_Medium" Font="Book Antiqua, 8.25pt" 
                                                 LegendText="Medium Sev Closed">
                                             </asp:Series>
                                             <asp:Series ChartArea="ChartArea_Closure" ChartType="StackedColumn" 
                                                 CustomProperties="DrawingStyle=Cylinder" Legend="Legend1" 
                                                 Name="Series_Closure_Low" Font="Book Antiqua, 8.25pt" 
                                                 LegendText="Low Sev Closed">
                                             </asp:Series>
                                         </Series>
                                         <ChartAreas>
                                             <asp:ChartArea Name="ChartArea_Closure">
                                                 <AxisY IsLabelAutoFit="False">
                                                     <LabelStyle Font="Book Antiqua, 8.25pt" />
                                                     <MajorGrid LineDashStyle="Dot" />
                                                 </AxisY>
                                                 <AxisX IsLabelAutoFit="False">
                                                     <LabelStyle Font="Book Antiqua, 8.25pt" />
                                                 </AxisX>
                                             </asp:ChartArea>
                                             <asp:ChartArea Name="ChartArea_New_SRs">
                                                 <AxisY IsLabelAutoFit="False">
                                                     <LabelStyle Font="Book Antiqua, 8.25pt" />
                                                     <MajorGrid LineDashStyle="Dot" />
                                                 </AxisY>
                                                 <AxisX IsLabelAutoFit="False">
                                                     <LabelStyle Font="Book Antiqua, 8.25pt" />
                                                 </AxisX>
                                             </asp:ChartArea>
                                         </ChartAreas>
                                         <Legends>
                                             <asp:Legend Font="Baskerville Old Face, 9pt" IsTextAutoFit="False" 
                                                 Name="Legend1" TitleFont="Book Antiqua, 8.25pt, style=Bold">
                                             </asp:Legend>
                                         </Legends>
                                         <Titles>
                                             <asp:Title Font="Book Antiqua, 9pt, style=Bold" Name="SR Arrival" 
                                                 DockedToChartArea="ChartArea_New_SRs" Text="SRs Raised By Severity">
                                             </asp:Title>
                                             <asp:Title DockedToChartArea="ChartArea_Closure" 
                                                 Font="Book Antiqua, 9pt, style=Bold" Name="SR Closure" 
                                                 Text="SRs Closure By Severity By Vendor">
                                             </asp:Title>
                                         </Titles>
                                     </asp:Chart>
                             <br />
                             <span class="style10">
                             <span class="style6">Filter by SR Submit Date and Frequency (SRault date range is 6 
                                     months):<br /> From Date:</span>
                             <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_From_Date_Outgoing_SR_Arrvl_Closure" runat="server" 
                                  style="font-family: Andalus" 
                                 ValidationGroup="Outgoing_SR_Arrvl_Closure" ></asp:TextBox>
                             <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_From_Date_Outgoing_SR_Arrvl_Closure_CalendarExtender" 
                                 runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton_Outgoing_SR_Arrvl_Closure" 
                                 TargetControlID="TextBox_From_Date_Outgoing_SR_Arrvl_Closure">
                             </ajaxtoolkit:CalendarExtender>
                             <span class="style6">To Date:</span><asp:TextBox 
                                 ID="TextBox_To_Date_Outgoing_SR_Arrvl_Closure" runat="server" class="form-control datepicker-textbox"
                                  style="font-family: Andalus" ValidationGroup="Outgoing_SR_Arrvl_Closure" 
                                 ></asp:TextBox>
                             <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_To_Date_Outgoing_SR_Arrvl_Closure_CalendarExtender" 
                                 runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton_To_Date_Outgoing_SR_Arrvl_Closure" 
                                 TargetControlID="TextBox_To_Date_Outgoing_SR_Arrvl_Closure">
                             </ajaxtoolkit:CalendarExtender>
                                     &nbsp; Frequency:&nbsp;<asp:DropDownList 
                                         ID="DropDownList_Outg_SR_Arrival_Closure_Freq" runat="server">
                                     </asp:DropDownList>
                             &nbsp;<asp:Button ID="Button_Filter_Date_Outgoing_SR_Arrvl_Closure" runat="server" class="btn btn-sm btn-success"
                                         onclick="Button_Filter_Date_Outgoing_SR_Arrvl_Closure_Click" 
                                         style="font-family: Andalus; font-size: small;" Text="Show!" 
                                         ValidationGroup="Outgoing_SR_Arrvl_Closure" />
                                     &nbsp;<asp:Button ID="Button_Report_Date_Outgoing_SR_Arrival_Closure" class="btn btn-sm btn-success"
                                         runat="server" 
                                         onclick="Button_Report_Date_Outgoing_SR_Arrival_Closure_Click" 
                                         style="font-family: Andalus; font-size: small;" Text="Report!" 
                                         ValidationGroup="Outgoing_SR_Arrvl_Closure" />
                                     &nbsp;<asp:CompareValidator ID="CompareValidator21" runat="server" 
                                 ControlToCompare="TextBox_From_Date_Outgoing_SR_Arrvl_Closure" 
                                 ControlToValidate="TextBox_To_Date_Outgoing_SR_Arrvl_Closure" Display="Dynamic" 
                                 ErrorMessage="To Date earlier than from date" ForeColor="Red" 
                                 Operator="GreaterThan" ValidationGroup="Outgoing_SR_Arrvl_Closure"></asp:CompareValidator>
                             <br />
                             </span>
                         </td>
                     </tr>
                 </table>
                 <table id="OutgSRClsTime" border="0" cellpadding="0" cellspacing="0" width="90%">
                 <tr>
                 <td>
                     <asp:Chart ID="Chart_Outg_SR_Closure_Avg_Time" runat="server" 
                         BorderlineColor="Tan" BorderlineDashStyle="Solid" Height="347px" 
                         ViewStateContent="All" ViewStateMode="Enabled" Width="1126px">
                         <Series>
                             <asp:Series ChartArea="ChartArea_SR_Closure" ChartType="Line" 
                                 CustomProperties="DrawingStyle=Cylinder" Font="Book Antiqua, 8.25pt" 
                                 Legend="Legend1" LegendText="All Sev Avg Closure Time" 
                                 Name="Series_SRs_Closure_Total" BorderWidth="3" Color="Red">
                             </asp:Series>
                             <asp:Series ChartArea="ChartArea_SR_Closure" ChartType="Line" 
                                 CustomProperties="DrawingStyle=Cylinder, DrawSideBySide=True" 
                                 Font="Book Antiqua, 8.25pt" Legend="Legend1" 
                                 LegendText="High Sev Avg Closure Time" Name="Series_Closure_High" 
                                 BorderDashStyle="Dot" BorderWidth="2" LabelToolTip="High Severity">
                             </asp:Series>
                             <asp:Series ChartArea="ChartArea_SR_Closure" ChartType="Line" 
                                 CustomProperties="DrawingStyle=Cylinder" Font="Book Antiqua, 8.25pt" 
                                 Legend="Legend1" LegendText="Medium Sev Avg Closure Time" 
                                 Name="Series_Closure_Medium" BorderDashStyle="Dash" BorderWidth="2" 
                                 LabelToolTip="Medium Severity">
                             </asp:Series>
                             <asp:Series ChartArea="ChartArea_SR_Closure" ChartType="Line" 
                                 CustomProperties="DrawingStyle=Cylinder" Font="Book Antiqua, 8.25pt" 
                                 Legend="Legend1" LegendText="Low Sev Avg Closure Time" 
                                 Name="Series_Closure_Low" BorderWidth="2" LabelToolTip="Low Severity">
                             </asp:Series>
                         </Series>
                         <ChartAreas>
                             <asp:ChartArea Name="ChartArea_SR_Closure">
                                 <AxisY IsLabelAutoFit="False">
                                     <LabelStyle Font="Book Antiqua, 8.25pt" />
                                     <MajorGrid LineDashStyle="Dot" />
                                 </AxisY>
                                 <AxisX IsLabelAutoFit="False">
                                     <LabelStyle Font="Book Antiqua, 8.25pt" />
                                 </AxisX>
                             </asp:ChartArea>
                         </ChartAreas>
                         <Legends>
                             <asp:Legend Font="Baskerville Old Face, 9pt" IsTextAutoFit="False" 
                                 Name="Legend1" TitleFont="Book Antiqua, 8.25pt, style=Bold">
                             </asp:Legend>
                         </Legends>
                         <Titles>
                             <asp:Title DockedToChartArea="ChartArea_SR_Closure" 
                                 Font="Book Antiqua, 9pt, style=Bold" Name="SR Closure Time" 
                                 Text="SR Closure Average Time (Hours)">
                             </asp:Title>
                         </Titles>
                     </asp:Chart>
                     <br />
                     <span class="style10"><span class="style6">Filter by SR Submit 
                     Date,Frequency and Service Agent (SRault date range is 6 months):<br /> From 
                     Date:</span>
                     <asp:TextBox class="form-control datepicker-textbox" ID="TextBox_From_Date_Outgoing_SR_Avg_Closure" runat="server" 
                          style="font-family: Andalus" 
                         ValidationGroup="Outgoing_SR_Avg_Closure" ></asp:TextBox>
                     <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_From_Date_Outgoing_SR_Avg_Closure_CalendarExtender" 
                         runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton_From_Date_Outgoing_SR_Avg_Closure" 
                         TargetControlID="TextBox_From_Date_Outgoing_SR_Avg_Closure">
                     </ajaxtoolkit:CalendarExtender>
                     <span class="style6">To Date:</span><asp:TextBox 
                         ID="TextBox_To_Date_Outgoing_SR_Avg_Closure" runat="server" 
                         class="form-control datepicker-textbox" style="font-family: Andalus" 
                         ValidationGroup="Outgoing_SR_Avg_Closure" ></asp:TextBox>
                     <ajaxtoolkit:CalendarExtender Enabled="false" ID="TextBox_To_Date_Outgoing_SR_Avg_Closure_CalendarExtender" 
                         runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton_To_Date_Outgoing_SR_Avg_Closure" 
                         TargetControlID="TextBox_To_Date_Outgoing_SR_Avg_Closure">
                     </ajaxtoolkit:CalendarExtender>
                     &nbsp; Frequency:&nbsp;<asp:DropDownList class="form-control form-control-dropdown" ID="DropDownList_Outg_SR_Avg_Closure_Freq" 
                         runat="server">
                     </asp:DropDownList>
                     &nbsp;Select Vendor:&nbsp;<asp:DropDownList 
                         ID="DropDownList_Outg_SR_Avg_Closure_Vendor" runat="server">
                     </asp:DropDownList>
                     &nbsp;<asp:Button ID="Button_Filter_Date_Outgoing_SR_Avg_Closure" runat="server" class="btn btn-sm btn-success"
                         style="font-family: Andalus; font-size: small;" Text="Show!" 
                         ValidationGroup="Outgoing_SR_Avg_Closure" 
                         onclick="Button_Filter_Date_Outgoing_SR_Avg_Closure_Click" />
                     &nbsp;<asp:Button ID="Button_Report_Date_Outgoing_SR_Avg_Closure" runat="server" class="btn btn-sm btn-success"
                         onclick="Button_Report_Date_Outgoing_SR_Avg_Closure_Click" 
                         style="font-family: Andalus; font-size: small;" Text="Report!" 
                         ValidationGroup="Outgoing_SR_Avg_Closure" />
                     <asp:CompareValidator ID="CompareValidator27" runat="server" 
                         ControlToCompare="TextBox_From_Date_Outgoing_SR_Avg_Closure" 
                         ControlToValidate="TextBox_To_Date_Outgoing_SR_Avg_Closure" 
                         Display="Dynamic" ErrorMessage="To Date earlier than from date" ForeColor="Red" 
                         Operator="GreaterThan" ValidationGroup="Outgoing_SR_Avg_Closure"></asp:CompareValidator>
                     <br />
                     </span>
                 </td>
                 </tr>
                 </table>
                 <table id="OutgSR" border="0" cellpadding="0" cellspacing="0" width="90%">
                     <caption>
                         &nbsp;<tr>
                             <td align="center" width="45%">                                 
                                 <asp:Chart ID="Chart_Outgoing_SR_By_Account" runat="server" 
                                     BorderlineColor="Tan" BorderlineDashStyle="Solid" Width="400px" 
                                     Palette="EarthTones">
                                     <Series>
                                         <asp:Series BackGradientStyle="TopBottom" 
                                             CustomProperties="DrawingStyle=Cylinder, DrawSideBySide=True, LabelStyle=Center" 
                                             Font="Book Antiqua, 9pt" IsValueShownAsLabel="True" LegendText="Cleared" 
                                             Name="TotalSRs">
                                         </asp:Series>
                                     </Series>
                                     <ChartAreas>
                                         <asp:ChartArea BackColor="LightSteelBlue" BackGradientStyle="Center" 
                                             BorderColor="Gainsboro" Name="ChartArea1">
                                             <AxisY IsLabelAutoFit="False" TitleFont="Baskerville Old Face, 9.75pt">
                                                 <LabelStyle Font="Book Antiqua, 9.75pt" />
                                             </AxisY>
                                             <AxisX IsLabelAutoFit="False" TitleFont="Baskerville Old Face, 9.75pt">
                                                 <LabelStyle Font="Book Antiqua, 9.75pt" />
                                             </AxisX>
                                             <Area3DStyle Enable3D="True" Inclination="10" PointDepth="50" 
                                                 PointGapDepth="50" />
                                         </asp:ChartArea>
                                     </ChartAreas>
                                     <Titles>
                                         <asp:Title Font="Book Antiqua, 8.25pt" Name="Title1">
                                         </asp:Title>
                                     </Titles>
                                 </asp:Chart>
                                 <br />
                                 <br />
                                 <span class="style10"><span class="style6">Filter by SR Submit Date and Vendor 
                                 (SRault date range is 12 months):<br /> From Date:</span></span>
                                 <asp:TextBox class="form-control datepicker-textbox" ID="Textbox_From_Date_Outgoing_SR_By_Account" runat="server" 
                                      style="font-family: Andalus" 
                                     ValidationGroup="Chart_Outgoing_SR_By_Account_Group" ></asp:TextBox>
                                 <ajaxtoolkit:CalendarExtender Enabled="false" ID="Textbox_From_Date_Outgoing_SR_By_Account_CalendarExtender" 
                                     runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton_From_Date_Outgoing_SR_By_Account" 
                                     TargetControlID="Textbox_From_Date_Outgoing_SR_By_Account">
                                 </ajaxtoolkit:CalendarExtender>
                                 <span class="style10">
                                 <span class="style6">To Date:</span></span><asp:TextBox 
                                     ID="Textbox_To_Date_Outgoing_SR_By_Account" runat="server" 
                                      style="font-family: Andalus" class="form-control datepicker-textbox" 
                                     ValidationGroup="Chart_Outgoing_SR_By_Account_Group" ></asp:TextBox>
                                 <ajaxtoolkit:CalendarExtender Enabled="false" ID="Textbox_To_Date_Outgoing_SR_By_Account_CalendarExtender" 
                                     runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton_To_Date_Outgoing_SR_By_Account" 
                                     TargetControlID="Textbox_To_Date_Outgoing_SR_By_Account">
                                 </ajaxtoolkit:CalendarExtender>
                                 &nbsp;<span class="style10"><span class="style6">Select Vendor : </span>
                                 <asp:ListBox ID="ListBox_Outgoing_SR_By_Account" runat="server" 
                                     Height="57px" SelectionMode="Multiple" 
                                     style="font-family: Andalus; font-size: small" Width="180px"></asp:ListBox>
                                 </span>
                                 <asp:Button ID="Button_Outgoing_SR_By_Account" runat="server" class="btn btn-sm btn-success"
                                     onclick="Button_Outgoing_SR_By_Account_Click" 
                                     style="font-family: Andalus; font-size: small;" Text="Show!" 
                                     ValidationGroup="Chart_Outgoing_SR_By_Account_Group" />
                                 <span class="style10">&nbsp;<asp:Button 
                                     ID="Button_Outgoing_SR_By_Account_Download" runat="server" class="btn btn-sm btn-success"
                                     onclick="Button_Outgoing_SR_By_Account_Download_Click" 
                                     style="font-family: Andalus; font-size: small;" Text="Report" 
                                     ValidationGroup="Chart_Outgoing_SR_By_Account_Group" />
                                 <asp:CompareValidator ID="CompareValidator28" runat="server" 
                                     ControlToCompare="Textbox_From_Date_Outgoing_SR_By_Account" 
                                     ControlToValidate="Textbox_To_Date_Outgoing_SR_By_Account" 
                                     Display="Dynamic" ErrorMessage="To Date earlier than from date" ForeColor="Red" 
                                     Operator="GreaterThan" ValidationGroup="Chart_Outgoing_SR_By_Account_Group"></asp:CompareValidator>
                                 </span>
                             </td>
                             <td align="center" width="45%">
                             <br />
                                 <asp:Chart ID="Chart_Outgoing_SR_No_By_Account" runat="server" 
                                     BorderlineColor="Tan" BorderlineDashStyle="Solid" Width="400px" 
                                     Palette="EarthTones">
                                     <Series>
                                         <asp:Series BackGradientStyle="TopBottom" ChartType="StackedColumn" 
                                             CustomProperties="DrawingStyle=Cylinder, DrawSideBySide=True, LabelStyle=Center" 
                                             Font="Book Antiqua, 9pt" IsValueShownAsLabel="True" Legend="Legend_High" 
                                             LegendText="High Sev" Name="HighSRs">
                                         </asp:Series>
                                         <asp:Series ChartArea="ChartArea1" ChartType="StackedColumn" 
                                             CustomProperties="DrawingStyle=Cylinder" Legend="Legend_Low" 
                                             LegendText="Low Sev" Name="LowSRs">
                                         </asp:Series>
                                         <asp:Series ChartArea="ChartArea1" ChartType="StackedColumn" 
                                             CustomProperties="DrawingStyle=Cylinder" Legend="Legend_Medium" 
                                             LegendText="Medium Sev" Name="MediumSRs">
                                         </asp:Series>
                                     </Series>
                                     <ChartAreas>
                                         <asp:ChartArea BackColor="LightSteelBlue" BackGradientStyle="Center" 
                                             BorderColor="Gainsboro" Name="ChartArea1">
                                             <AxisY IsLabelAutoFit="False" TitleFont="Baskerville Old Face, 9.75pt">
                                                 <LabelStyle Font="Book Antiqua, 9.75pt" />
                                             </AxisY>
                                             <AxisX IsLabelAutoFit="False" TitleFont="Baskerville Old Face, 9.75pt">
                                                 <LabelStyle Font="Book Antiqua, 9.75pt" />
                                             </AxisX>
                                             <Area3DStyle Enable3D="True" Inclination="10" PointDepth="50" 
                                                 PointGapDepth="50" />
                                         </asp:ChartArea>
                                     </ChartAreas>
                                     <Legends>
                                         <asp:Legend Alignment="Far" BackColor="White" 
                                             DockedToChartArea="ChartArea1" Docking="Bottom" Font="Book Antiqua, 8.25pt" 
                                             IsTextAutoFit="False" ItemColumnSpacing="10" LegendStyle="Row" 
                                             Name="Legend_High" TitleFont="Book Antiqua, 8pt, style=Bold">
                                         </asp:Legend>
                                         <asp:Legend Alignment="Far" BackColor="White" 
                                             DockedToChartArea="ChartArea1" Docking="Bottom" Font="Book Antiqua, 8.25pt" 
                                             IsTextAutoFit="False" ItemColumnSpacing="10" LegendStyle="Row" 
                                             Name="Legend_Low" TitleFont="Book Antiqua, 8pt, style=Bold">
                                         </asp:Legend>
                                         <asp:Legend Alignment="Far" BackColor="White" 
                                             DockedToChartArea="ChartArea1" Docking="Bottom" Font="Book Antiqua, 8.25pt" 
                                             IsTextAutoFit="False" ItemColumnSpacing="10" LegendStyle="Row" 
                                             Name="Legend_Medium" TitleFont="Book Antiqua, 8pt, style=Bold">
                                         </asp:Legend>
                                     </Legends>
                                     <Titles>
                                         <asp:Title Font="Book Antiqua, 8.25pt" Name="Title1">
                                         </asp:Title>
                                     </Titles>
                                 </asp:Chart>
                                 <br />
                                 <br />
                                 <span class="style10"><span class="style6">Filter by SR Submit Date,Type and 
                                 Vendor(SRault date range is 12 months):<br /> From Date:</span></span>
                                 <asp:TextBox class="form-control datepicker-textbox" ID="Textbox_From_Date_Outgoing_SR_No_By_Account" 
                                     runat="server"  style="font-family: Andalus" 
                                     ValidationGroup="Outgoing_SR_No_By_Account" ></asp:TextBox>
                                 <ajaxtoolkit:CalendarExtender Enabled="false" ID="Textbox_From_Date_Outgoing_SR_No_By_Account_CalendarExtender" 
                                     runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton_From_Date_Outgoing_SR_No_By_Account" 
                                     TargetControlID="Textbox_From_Date_Outgoing_SR_No_By_Account">
                                 </ajaxtoolkit:CalendarExtender>
                                 <span class="style10">
                                 <span class="style6">To Date:</span></span><asp:TextBox 
                                     ID="Textbox_To_Date_Outgoing_SR_No_By_Account" runat="server" 
                                     class="form-control datepicker-textbox" style="font-family: Andalus" 
                                     ValidationGroup="Outgoing_SR_No_By_Account" ></asp:TextBox>
                                 <ajaxtoolkit:CalendarExtender Enabled="false" ID="Textbox_To_Date_Outgoing_SR_No_By_Account_CalendarExtender" 
                                     runat="server" Format="yyyy-MM-dd" PopupButtonID="ImageButton_To_Date_Outgoing_SR_No_By_Account" 
                                     TargetControlID="Textbox_To_Date_Outgoing_SR_No_By_Account">
                                 </ajaxtoolkit:CalendarExtender>
                                 &nbsp;<span class="style10"><span class="style6">&nbsp;Select Vendor: </span>
                                 <asp:ListBox ID="Listbox_Outgoing_SR_No_By_Account" runat="server" 
                                     Height="57px" SelectionMode="Multiple" 
                                     style="font-family: Andalus; font-size: small" Width="180px"></asp:ListBox>
                                 &nbsp;</span><asp:Button ID="Button_Outgoing_SR_No_By_Account" runat="server" class="btn btn-sm btn-success"
                                     onclick="Button_Outgoing_SR_No_By_Account_Click" 
                                     style="font-family: Andalus; font-size: small;" Text="Show!" 
                                     ValidationGroup="Outgoing_SR_No_By_Account" />
                                 <span class="style10">&nbsp;<asp:Button 
                                     ID="Button_Outgoing_SR_No_By_Account_Download" runat="server" class="btn btn-sm btn-success"
                                     onclick="Button_Outgoing_SR_No_By_Account_Download_Click" 
                                     style="font-family: Andalus; font-size: small;" Text="Report" 
                                     ValidationGroup="Outgoing_SR_No_By_Account" />
                                 <asp:CompareValidator ID="CompareValidator29" runat="server" 
                                     ControlToCompare="Textbox_From_Date_Outgoing_SR_No_By_Account" 
                                     ControlToValidate="Textbox_To_Date_Outgoing_SR_No_By_Account" 
                                     Display="Dynamic" ErrorMessage="To Date earlier than from date" ForeColor="Red" 
                                     Operator="GreaterThan" ValidationGroup="Outgoing_SR_No_By_Account"></asp:CompareValidator>
                                 </span>
                                 <br />
                             </td>
                         </tr>
                     </caption>
            </table>   
                                             </ContentTemplate>
                             </asp:UpdatePanel>     
            <br />        
    </asp:Panel>
     <ajaxtoolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender_Out_SR" runat="server" Collapsed="true" AutoCollapse="false" TargetControlID="Panel_Outgoing_SRs"
                        ImageControlId="Outg_SR_Collapse_Img4"  CollapsedImage="~/Images/ArrowTip_Down.png" ExpandedImage="~/Images/ArrowTip_Parallel.png"  ExpandControlID="LinkButton_Outg_SR"
             CollapseControlID="LinkButton_Outg_SR"/>
                 </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
