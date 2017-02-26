using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using ActionLibrary;
using BackEndObjects;
using System.Data;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Web.UI.HtmlControls;



namespace OnLine.Pages
{
    public partial class DashBoard : System.Web.UI.Page
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            Page.Theme = Session[SessionFactory.LOGGED_IN_USER_THEME].ToString();
        }

        protected void loadFiledNameCollections()
        {
            Dictionary<String, String> fieldNameCollections = new Dictionary<String, String>();
                        
            ActionLibrary.LeadRecord leadObj=new LeadRecord();

            String LEAD_ACTIVE_STAT = "LEAD_ACTIVE_STAT";
            String LEAD_APPROVAL_STAT = "LEAD_APPROVAL_STAT";
            String LEAD_CREATED_USR = "LEAD_CREATED_USR";
            String LEAD_CREATE_MODE = "LEAD_CREATE_MODE";
            String LEAD_DUE_DATE = "LEAD_DUE_DATE";
            String LEAD_ENTITY_NAME = "LEAD_ENTITY_NAME";
            String LEAD_RFQ_ID = "LEAD_RFQ_ID";
            String LEAD_RFQ_NAME = "LEAD_RFQ_NAME";
            String LEAD_SUBMIT_DATE = "LEAD_SUBMIT_DATE";
            String LEAD_TERMS_COND = "LEAD_TERMS_COND";

            fieldNameCollections.Add(LEAD_ACTIVE_STAT, "Active Stat");
            fieldNameCollections.Add(LEAD_APPROVAL_STAT, "Approval Stat");
            fieldNameCollections.Add(LEAD_CREATED_USR, "Create By");
            fieldNameCollections.Add(LEAD_CREATE_MODE, "Create Mode");
            fieldNameCollections.Add(LEAD_DUE_DATE, "Due Date");
            fieldNameCollections.Add(LEAD_ENTITY_NAME, "Entity Name");
            fieldNameCollections.Add(LEAD_RFQ_ID, "RFQ id");
            fieldNameCollections.Add(LEAD_RFQ_NAME, "Lead Name");
            fieldNameCollections.Add(LEAD_SUBMIT_DATE, "Submit Date");
            fieldNameCollections.Add(LEAD_TERMS_COND, "Terms");


    }   

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING] == null)
                Response.Redirect("Login.aspx");
            else
            {
                if (!Page.IsPostBack)
                {
                    //((Menu)Master.FindControl("Menu1")).Items[5].Selected = true;
                    ((HtmlGenericControl)(Master.FindControl("Dashboard"))).Attributes.Add("class", "active");

                    String entId = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();
                    Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];
                    Dictionary<String, Currency> allCurrList = (Dictionary<String, Currency>)Session[SessionFactory.CURRENCY_LIST];
                    String defaultCurr = Session[SessionFactory.MAIN_BUSINESS_ENTITY_DEFAULT_CURRENCY] != null ?
                       Session[SessionFactory.MAIN_BUSINESS_ENTITY_DEFAULT_CURRENCY].ToString() : "";
                    Dictionary<String, String> allExistingContacts = (Dictionary<String, String>)Session[SessionFactory.EXISTING_CONTACT_DICTIONARY];
                    String theme = Session[SessionFactory.LOGGED_IN_USER_THEME].ToString();

                    Session.RemoveAll();
                    Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING] = entId;
                    Session[SessionFactory.LOGGED_IN_USER_ID_STRING] = User.Identity.Name;
                    Session[SessionFactory.ACCESSLIST_FOR_USER] = accessList;
                    Session[SessionFactory.CURRENCY_LIST] = allCurrList;
                    Session[SessionFactory.MAIN_BUSINESS_ENTITY_DEFAULT_CURRENCY] = defaultCurr;
                    Session[SessionFactory.EXISTING_CONTACT_DICTIONARY] = allExistingContacts;
                    Session[SessionFactory.LOGGED_IN_USER_THEME] = theme;

                    populateLogo();

                    if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS] ||
                        accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_DASHBOARD_SCREEN_VIEW])
                    {
                        //Now check accesses to all other dashboard components
                        if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                            CheckAccessToActions();

                        if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS] ||
    accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_LEAD_REPORT])
                            generateLeadCharts();
                       
                        
                        loadDropDownLists();
                    }
                    else
                    {
                        Label_Dashboard_Screen_Access.Visible = true;
                        Label_Dashboard_Screen_Access.Text = "You don't have access to view this page";
                    }
                    //generatePotentialCharts(generateLeadCharts().getPotList());
                    /*generateSalesTransactionCharts(null, null);*/
                    /*generatePurchaseTransactionCharts(null, null);*/
                    /*generateIncomingDefectectsCharts(null, null);
                    generateOutgoingDefectsCharts(null, null);*/
                    
                }
            }
        }

        protected void CheckAccessToActions()
        {
            Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

            if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_POTENTIAL_REPORT])
            {
                Panel_Pot_Dashboard.Visible = false;
                Label_Potn_Dashboard_Access.Visible = true;
                Label_Potn_Dashboard_Access.Text = "You don't have access to view Potential dashboards";
            }
            else if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_DOWNLOAD_POTENTIAL_REPORT])
            {
                Button_Potn_Conv_Val_Download.Enabled=false;
                Button_Potn_Conv_No_Download.Enabled=false;
                Button_Potn_By_Cat_Download.Enabled = false;

                Label_Potn_Dashboard_Access.Visible = true;
                Label_Potn_Dashboard_Access.Text = "You don't have access to download Potential reports";
            }

            if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_LEAD_REPORT])
            {
                Panel_Lead_Dashboard.Visible = false;
                Label_Lead_Dashboard_Access.Visible = true;
                Label_Lead_Dashboard_Access.Text = "You don't have access to view Lead dashboards";
            }
            else if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_DOWNLOAD_LEAD_REPORT])
            {
                Button_Lead_Conv_Val_Download.Enabled = false;
                Button_Lead_Conv_No_Download.Enabled = false;

                Label_Lead_Dashboard_Access.Visible = true;
                Label_Lead_Dashboard_Access.Text = "You don't have access to download Lead reports";
            }

            if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_SALES_TRANSAC_REPORT])
            {
                Panel_Tran_Sales_Dashboard.Visible = false;
                Label_Tran_Sales_Dashboard_Access.Visible = true;
                Label_Tran_Sales_Dashboard_Access.Text = "You don't have access to view Sales Transaction dashboards";
            }
            else if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_DOWNLOAD_SALES_TRANSAC_REPORT])
            {
                Button_Prod_Wise_Sales_Qnty_Download.Enabled = false;
                Button_Prod_Wise_Sales_Amount.Enabled = false;
                Button_Pending_Clear_Contact_Download.Enabled = false;

                Label_Tran_Sales_Dashboard_Access.Visible = true;
                Label_Tran_Sales_Dashboard_Access.Text = "You don't have access to download Sales Transaction reports";
            }

            if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_PURCHASE_TRANSAC_REPORT])
            {
                Panel_Tran_Purchase_Dashboard.Visible = false;
                Label_Tran_Purchase_Dashboard_Access.Visible = true;
                Label_Tran_Purchase_Dashboard_Access.Text = "You don't have access to view Purchase Transaction dashboards";
            }
            else if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_DOWNLOAD_PURCHASE_TRANSAC_REPORT])
            {
                Button_Prod_Wise_Purchase_Qnty_Download.Enabled = false;
                Button_Prod_Wise_Purchase_Amount.Enabled = false;
                Button_Pending_Clear_Contact_Purchase_Download.Enabled = false;

                Label_Tran_Purchase_Dashboard_Access.Visible = true;
                Label_Tran_Purchase_Dashboard_Access.Text = "You don't have access to download Purchase Transaction reports";
            }

            if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_INCOMING_DEFECT_REPORT])
            {
                Panel_Incoming_Defects.Visible = false;
                Label_Incm_Def_Dashboard_Access.Visible = true;
                Label_Incm_Def_Dashboard_Access.Text = "You don't have access to view Incoming Defects dashboards";
            }
            else if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_DOWNLOAD_INCOMING_DEFECT_REPORT])
            {
                Button_Report_Date_Incoming_Defect_Arrival_Closure.Enabled=false;
                Button_Report_Date_Incoming_Defect_Avg_Closure.Enabled=false;
                Button_Incoming_Defect_By_Account_Download.Enabled=false;
                Button_Incoming_Defect_No_By_Account_Download.Enabled = false;

                Label_Incm_Def_Dashboard_Access.Visible = true;
                Label_Incm_Def_Dashboard_Access.Text = "You don't have access to download Incoming Defects reports";
            }

            if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_OUTGOING_DEFECT_REPORT])
            {
                Panel_Outgoing_Defects.Visible = false;
                Label_Outg_Def_Dashboard_Access.Visible = true;
                Label_Outg_Def_Dashboard_Access.Text = "You don't have access to view Outgoing Defects dashboards";
            }
            else if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_DOWNLOAD_OUTGOING_DEFECT_REPORT])
            {
                Button_Report_Outgoing_Defect_Arrival_Closure.Enabled=false;
                Button_Report_Outgoing_Defect_Avg_Closure.Enabled=false;
                Button_Outgoing_Defect_By_Account_Download.Enabled=false;
                Button_Outgoing_Defect_No_By_Account_Download.Enabled = false;

                Label_Outg_Def_Dashboard_Access.Visible = true;
                Label_Outg_Def_Dashboard_Access.Text = "You don't have access to download Outgoing Defects reports";
            }
        }

        protected void loadDropDownLists()
        {
            ArrayList contactObjList = Contacts.getAllContactsbyEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            for (int i = 0; i < contactObjList.Count; i++)
            {
                ListItem lt = new ListItem();
                String contactShortName = ((Contacts)contactObjList[i]).getContactShortName();
                String contactName = ((Contacts)contactObjList[i]).getContactName();
                String contactEntId = ((Contacts)contactObjList[i]).getContactEntityId();

                lt.Text = (contactShortName == null || contactShortName.Equals("") ? contactName : contactShortName);
                lt.Value = contactEntId;
                DropDownList_Outg_Defect_Avg_Closure_Vendor.Items.Add(lt);
                DropDownList_Outg_SR_Avg_Closure_Vendor.Items.Add(lt);
                
            }


            ListItem lt1 = new ListItem();
            lt1.Text = "weekly";
            lt1.Value = "weekly";

            ListItem lt2 = new ListItem();
            lt2.Text = "monthly";
            lt2.Value = "monthly";

            DropDownList_Incm_Defect_Arrival_Closure_Freq.Items.Add(lt1);
            DropDownList_Incm_Defect_Arrival_Closure_Freq.Items.Add(lt2);

            DropDownList_Incm_SR_Arrival_Closure_Freq.Items.Add(lt1);
            DropDownList_Incm_SR_Arrival_Closure_Freq.Items.Add(lt2);

            DropDownList_Incm_Defect_Avg_Closure_Freq.Items.Add(lt1);
            DropDownList_Incm_Defect_Avg_Closure_Freq.Items.Add(lt2);

            DropDownList_Incm_SR_Avg_Closure_Freq.Items.Add(lt1);
            DropDownList_Incm_SR_Avg_Closure_Freq.Items.Add(lt2);

            DropDownList_Outgoing_Defect_Arrival_Closure_Freq.Items.Add(lt1);
            DropDownList_Outgoing_Defect_Arrival_Closure_Freq.Items.Add(lt2);

            DropDownList_Outg_SR_Arrival_Closure_Freq.Items.Add(lt1);
            DropDownList_Outg_SR_Arrival_Closure_Freq.Items.Add(lt2);

            DropDownList_Outg_Defect_Avg_Closure_Freq.Items.Add(lt1);
            DropDownList_Outg_Defect_Avg_Closure_Freq.Items.Add(lt2);

            DropDownList_Outg_SR_Avg_Closure_Freq.Items.Add(lt1);
            DropDownList_Outg_SR_Avg_Closure_Freq.Items.Add(lt2);

            DropDownList_Incm_Defect_Arrival_Closure_Freq.SelectedValue = "weekly";
            DropDownList_Incm_Defect_Avg_Closure_Freq.SelectedValue = "weekly";
            DropDownList_Incm_SR_Arrival_Closure_Freq.SelectedValue = "weekly";
            DropDownList_Incm_SR_Avg_Closure_Freq.SelectedValue = "weekly";
            DropDownList_Outgoing_Defect_Arrival_Closure_Freq.SelectedValue = "weekly";
            DropDownList_Outg_Defect_Avg_Closure_Freq.SelectedValue = "weekly";
            DropDownList_Outg_SR_Arrival_Closure_Freq.SelectedValue = "weekly";
            DropDownList_Outg_SR_Avg_Closure_Freq.SelectedValue = "weekly";

            Dictionary<String, userDetails> allUserDetails = BackEndObjects.MainBusinessEntity.
                getUserDetailsforMainEntitybyIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

            foreach (KeyValuePair<String, userDetails> kvp in allUserDetails)
            {
                ListItem ltAgent = new ListItem();
                ltAgent.Text = kvp.Value.getName();
                ltAgent.Value = kvp.Value.getName();

                DropDownList_Incm_Defect_Avg_Closure_Service_Agnt.Items.Add(ltAgent);
                DropDownList_Incm_SR_Avg_Closure_Service_Agnt.Items.Add(ltAgent);
            }

            ListItem ltAgentEmpty = new ListItem();
            ltAgentEmpty.Text = "All";
            ltAgentEmpty.Value = "All";
            DropDownList_Incm_Defect_Avg_Closure_Service_Agnt.Items.Add(ltAgentEmpty);
            DropDownList_Incm_Defect_Avg_Closure_Service_Agnt.SelectedValue = "All";

            DropDownList_Incm_SR_Avg_Closure_Service_Agnt.Items.Add(ltAgentEmpty);
            DropDownList_Incm_SR_Avg_Closure_Service_Agnt.SelectedValue = "All";

            DropDownList_Outg_Defect_Avg_Closure_Vendor.Items.Add(ltAgentEmpty);
            DropDownList_Outg_Defect_Avg_Closure_Vendor.SelectedValue = "All";

            DropDownList_Outg_SR_Avg_Closure_Vendor.Items.Add(ltAgentEmpty);
            DropDownList_Outg_SR_Avg_Closure_Vendor.SelectedValue = "All";

            ListItem ltDefectType1 = new ListItem();
            ltDefectType1.Text = "Open";
            ltDefectType1.Value = "Open";

            ListItem ltDefectType2 = new ListItem();
            ltDefectType2.Text = "Resolved";
            ltDefectType2.Value = "Resolved";

            ListItem ltDefectType3 = new ListItem();
            ltDefectType3.Text = "All";
            ltDefectType3.Value="All";

            DropDownList_Incoming_Defect_No_By_Account_Defect_Type.Items.Add(ltDefectType1);
            DropDownList_Incoming_Defect_No_By_Account_Defect_Type.Items.Add(ltDefectType2);
            DropDownList_Incoming_Defect_No_By_Account_Defect_Type.Items.Add(ltDefectType3);

            DropDownList_Incoming_Defect_No_By_Account_Defect_Type.SelectedValue = "All";

            DropDownList_Incoming_SR_No_By_Account_SR_Type.Items.Add(ltDefectType1);
            DropDownList_Incoming_SR_No_By_Account_SR_Type.Items.Add(ltDefectType2);
            DropDownList_Incoming_SR_No_By_Account_SR_Type.Items.Add(ltDefectType3);

            DropDownList_Incoming_SR_No_By_Account_SR_Type.SelectedValue = "All";
        }

        protected void populateLogo()
        {
            ArrayList imgListObjs = BackEndObjects.Image.getImagebyTypeandEntId(BackEndObjects.Image.PICTURE_IMG_TYPE_LOGO, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            if (imgListObjs.Count > 0)
            {
                //Only consider the first image object for logo
                BackEndObjects.Image imgObj = (BackEndObjects.Image)imgListObjs[0];
                ((System.Web.UI.WebControls.Image)Master.FindControl("Image_Logo")).ImageUrl = imageToURL(imgObj.getImgPath());
                ((System.Web.UI.WebControls.Image)Master.FindControl("Image_Logo")).Visible = true;

            }
        }
        public String generateImagePath(String folderName)
        {
            String imgStoreRoot = "~/Images/SessionImages";
            imgStoreRoot = Server.MapPath(imgStoreRoot);

            String returnPath = "";
            try
            {
                if (!Directory.Exists(imgStoreRoot + "\\" + folderName.ToString()))
                    Directory.CreateDirectory(imgStoreRoot + "\\" + folderName.ToString());
                returnPath = imgStoreRoot + "\\" + folderName.ToString();
            }
            catch (Exception ex)
            {
                returnPath = "";

            }
            return returnPath;
        }
        public String imageToURL(string sPath)
        {
            /*System.Net.WebClient client = new System.Net.WebClient();
            byte[] imageData = client.DownloadData(sPath);
            System.IO.MemoryStream stream = new System.IO.MemoryStream(imageData);
            System.Drawing.Image img = System.Drawing.Image.FromStream(stream);*/

            String[] imageNameParts = null;
            if (sPath != null && !sPath.Equals(""))
                imageNameParts = sPath.Split(new String[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);

            String finalImageUrl = "~/Images/SessionImages/" + Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString() + "/" + imageNameParts[imageNameParts.Length - 1];

            if (!File.Exists(Server.MapPath(finalImageUrl)))
            {
                try
                {
                    System.Drawing.Image img = System.Drawing.Image.FromFile(sPath);
                    System.Drawing.Bitmap newBitMap = new System.Drawing.Bitmap(img);

                    generateImagePath(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

                    newBitMap.Save(Server.MapPath(finalImageUrl), System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                catch (Exception ex)
                {
                    finalImageUrl = "";
                }
            }

            return finalImageUrl;
        }

        protected class LeadandPotential
        {
            private ArrayList leadList;
            private Dictionary<String, String> potDict;
            /// <summary>
            /// This variable was added later for requirement of potential dashboard containing the 'PotentialRecords' objects.
            /// </summary>
            private ArrayList potList;

            public ArrayList getPotList()
            {
                return this.potList;
            }
            public void setPotList(ArrayList potList)
            {
                this.potList = potList;
            }
            public ArrayList getLeadList()
            {
                return this.leadList;
            }
            public void setLeadList(ArrayList ldLst)
            {
                leadList = ldLst;
            }
            public Dictionary<String,String> getPotDict()
            {
                return this.potDict;
            }

            public void setPotDict(Dictionary<String, String> ptDict)
            {
                potDict = ptDict;
            }
        }
        /// <summary>
        /// Based on a given date range this method updates the Lead objects in the Arraylist and all potentials generated during that time period
        /// </summary>
        /// <param name="leadList"></param>
        /// <param name="potIDict"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        protected LeadandPotential generateLeadListandPotDict(ArrayList leadList, Dictionary<String, String> potIDict,String fromDate,String toDate)
        {
            ActionLibrary.SalesActions._dispLeads dspLeadObj = new SalesActions._dispLeads();

            Dictionary<String, String> filterParamsLead = new Dictionary<String, String>();
            filterParamsLead.Add(dspLeadObj.FILTER_BY_SUBMIT_DATE_FROM, fromDate);
            filterParamsLead.Add(dspLeadObj.FILTER_BY_SUBMIT_DATE_TO, toDate);

            leadList = dspLeadObj.
                getAllLeadDetailsFilteredIncludingConvertedtoPotential(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), User.Identity.Name, filterParamsLead);

            ActionLibrary.SalesActions._dispPotentials dspPotObj = new SalesActions._dispPotentials();

            Dictionary<String, String> filterParamsPot = new Dictionary<String, String>();
            filterParamsPot.Add(dspPotObj.FILTER_BY_CREATE_DATE_FROM, fromDate);
            filterParamsPot.Add(dspPotObj.FILTER_BY_CREATE_DATE_TO, toDate);

            ArrayList potList = dspPotObj.
                getAllPotentialsFiltered(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), User.Identity.Name, filterParamsPot);

            Dictionary<String, String> potDict = new Dictionary<string, string>();
            for (int j = 0; j < potList.Count; j++)
                potDict.Add(((PotentialRecords)potList[j]).getRFQId(), ((PotentialRecords)potList[j]).getRFQId());

            LeadandPotential leadPotObj = new LeadandPotential();
            leadPotObj.setLeadList(leadList);
            leadPotObj.setPotDict(potDict);
            leadPotObj.setPotList(potList);

            return leadPotObj;
        }

        protected void UpdatePanel11_load(object sender, EventArgs e)
        {
           //generateLeadCharts();

            Dictionary<String,String>  leadConvValLatestDict = (Dictionary<String, String>)Session[SessionFactory.ALL_DASHBOARD_LEAD_BY_VAL_SUCCESS_FAILURE];

            if (leadConvValLatestDict != null && leadConvValLatestDict.Count > 0)
            {
                Chart_Lead_Conv_By_Val.Titles.Clear();
                Chart_Lead_Conv_By_Val.Series[0].Points.Clear();

                Series leadConvbyValSeries = Chart_Lead_Conv_By_Val.Series[0];

                leadConvbyValSeries.IsVisibleInLegend = true;

                leadConvbyValSeries.Points.Add(float.Parse(leadConvValLatestDict["success"]));
                leadConvbyValSeries.Points[0].LegendText = "Success";
                leadConvbyValSeries.Points.Add(float.Parse(leadConvValLatestDict["failure"]));
                leadConvbyValSeries.Points[1].LegendText = "Failure";


                Chart_Lead_Conv_By_Val.Titles.Add("Lead Conversion % by Amount");
                Chart_Lead_Conv_By_Val.Titles[0].Font = new System.Drawing.Font("Book Antiqua", 9);
                Chart_Lead_Conv_By_Val.Series[0].Label = "#PERCENT{P0}(#VALY{0;0;#})";

                /*String finalImageUrl = "~/Images/SessionImages/" + Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString() + "/" + "Chart_Lead_Conv_By_Val.png";
                 if (!File.Exists(Server.MapPath(finalImageUrl)))
                     generateImagePath(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                 
                Chart_Lead_Conv_By_Val.SaveImage(Server.MapPath(finalImageUrl), ChartImageFormat.Png);*/
                 //Chart_Lead_Conv_By_Val.SaveImage(Server.MapPath(finalImageUrl));
                 //ImageLead.ImageUrl = finalImageUrl;
                 //ImageLead.Visible = true;
            }

            Dictionary<String, String> leadConvNoLatestDict = (Dictionary<String, String>)Session[SessionFactory.ALL_DASHBOARD_LEAD_BY_NUMBER_SUCCESS_FAILURE];

            if (leadConvNoLatestDict != null && leadConvNoLatestDict.Count > 0)
            {
                Chart_Lead_Conv_By_Number.Titles.Clear();
                Chart_Lead_Conv_By_Number.Series[0].Points.Clear();

                Series leadConvbyNumberSeries = Chart_Lead_Conv_By_Number.Series[0];

                leadConvbyNumberSeries.IsVisibleInLegend = true;

                leadConvbyNumberSeries.Points.Add(float.Parse(leadConvNoLatestDict["success"]));
                leadConvbyNumberSeries.Points[0].LegendText = "Success";
                leadConvbyNumberSeries.Points.Add(float.Parse(leadConvNoLatestDict["failure"]));
                leadConvbyNumberSeries.Points[1].LegendText = "Failure";

                Chart_Lead_Conv_By_Number.Titles.Add("Lead Conversion % by Numbers");
                Chart_Lead_Conv_By_Number.Titles[0].Font = new System.Drawing.Font("Book Antiqua", 9);
                Chart_Lead_Conv_By_Number.Series[0].Label = "#PERCENT{P0}(#VALY{0;0;#})";

            }
        }

        protected void UpdatePanelPotential_load(object sender, EventArgs e)
        {
            if (CollapsiblePanelExtender_Pot.ClientState!=null && CollapsiblePanelExtender_Pot.ClientState.Equals("false"))
           {
                //Check the last saved values for each of the charts

                //Potential by Stage Chart                
                String potStgFromDate = (TextBox_From_Date_Potn_Stage.Text.Equals("") ? DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd") : TextBox_From_Date_Potn_Stage.Text);
                String potStgToDate = (TextBox_To_Date_Potn_Stage.Text.Equals("") ? DateTime.Now.ToString("yyyy-MM-dd") : TextBox_To_Date_Potn_Stage.Text);
                generatePotnValbyStage(potStgFromDate, potStgToDate, null);

                //Potential By Category chart
                String potCatFromDate=(TextBox_From_Date_Potn_by_Cat.Text.Equals("")?DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd"):TextBox_From_Date_Potn_by_Cat.Text);
                String potCatToDate = (TextBox_To_Date_Potn_By_Cat.Text.Equals("") ? DateTime.Now.ToString("yyyy-MM-dd") : TextBox_To_Date_Potn_By_Cat.Text);
                generatePotnbyCategory(potCatFromDate, potCatToDate, null);

                //Potential By Success-Failure Values Chart
                String potValFromDate = (TextBox_From_Date_Potn_Val.Text.Equals("") ? DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd") : TextBox_From_Date_Potn_Val.Text);
                String potValToDate=(TextBox_To_Date_Potn_Val.Text.Equals("")? DateTime.Now.ToString("yyyy-MM-dd") :TextBox_To_Date_Potn_Val.Text);
                generatePotnConvbyVal(potValFromDate, potValToDate, null);

                //Potential By Success-Failure No Chart
                String potNoFromDate = (TextBox_From_Date_Potn_No.Text.Equals("") ? DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd") : TextBox_From_Date_Potn_No.Text);
                String potNoToDate = (TextBox_To_Date_Potn_No.Text.Equals("") ? DateTime.Now.ToString("yyyy-MM-dd") : TextBox_To_Date_Potn_No.Text);
                generatePotnConvbyNo(potNoFromDate, potNoToDate, null);
                //generatePotentialCharts(null);
            }

        }

        protected void UpdatePanelTranSales_load(object sender, EventArgs e)
        {
            if (CollapsiblePanelExtender_Tran_Sales.ClientState != null && CollapsiblePanelExtender_Tran_Sales.ClientState.Equals("false"))
            {
                String tranSalesPendingClearByAccntFromDate=(TextBox_From_Date_Pending_Clear_Contact.Text.Equals("")?DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd"):TextBox_From_Date_Pending_Clear_Contact.Text);
                String tranSalesPendingClearByAccntToDate = (TextBox_To_Date_Pending_Clear_Contact.Text.Equals("") ? DateTime.Now.ToString("yyyy-MM-dd") : TextBox_To_Date_Pending_Clear_Contact.Text);
                //This dictionary will have the customer id as the key and the name as the value
                Dictionary<String, String> custDict = new Dictionary<String, String>();
                if (ListBox_Contacts_Pending_Clear_Amnt_Sales.GetSelectedIndices().Length != 0)
                {
                    int[] indexList = ListBox_Contacts_Pending_Clear_Amnt_Sales.GetSelectedIndices();

                    for (int i = 0; i < indexList.Length; i++)
                        custDict.Add(ListBox_Contacts_Pending_Clear_Amnt_Sales.Items[indexList[i]].Value, ListBox_Contacts_Pending_Clear_Amnt_Sales.Items[indexList[i]].Text);

                }

                ArrayList invList = generatePendingClearPaymentsForSalesByAccounts(tranSalesPendingClearByAccntFromDate, tranSalesPendingClearByAccntToDate, null, custDict);

                String tranSalesTotalBusinessForSalesFromDate=(TextBox_From_Date_Chart_Sales_Total_Business_Contact.Text.Equals("")?DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd"):TextBox_From_Date_Chart_Sales_Total_Business_Contact.Text);
                String tranSalesTotalBusinessForSalesToDate = (TextBox_To_Date_Chart_Sales_Total_Business_Contact.Text.Equals("") ? DateTime.Now.ToString("yyyy-MM-dd") : TextBox_To_Date_Chart_Sales_Total_Business_Contact.Text);
                Dictionary<String, String> custDict1 = new Dictionary<String, String>();

                if (ListBox_Contacts_Total_Business_Chart.GetSelectedIndices().Length != 0)
                {
                    int[] indexList = ListBox_Contacts_Total_Business_Chart.GetSelectedIndices();

                    for (int i = 0; i < indexList.Length; i++)
                        custDict1.Add(ListBox_Contacts_Total_Business_Chart.Items[indexList[i]].Value, ListBox_Contacts_Total_Business_Chart.Items[indexList[i]].Text);

                }

                generateTotalBusinessForSalesByAccounts(tranSalesTotalBusinessForSalesFromDate, tranSalesTotalBusinessForSalesToDate, invList, custDict1);

                String fromDateProdWiseSalesQnty=(TextBox_From_Date_Prod_Wise_Sales_Qnty.Text.Equals("")?DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd"):TextBox_From_Date_Prod_Wise_Sales_Qnty.Text);
                String toDateProdWiseSalesQnty = (TextBox_To_Date_Prod_Wise_Sales_Qnty.Text.Equals("") ? DateTime.Now.ToString("yyyy-MM-dd") : TextBox_To_Date_Prod_Wise_Sales_Qnty.Text);

                generateProductWiseSalesQnty(fromDateProdWiseSalesQnty, toDateProdWiseSalesQnty, invList);

                String fromDateProdWiseSalesAmnt = (TextBox_From_Date_Prod_Wise_Sales_Amount.Text.Equals("") ? DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd") : TextBox_From_Date_Prod_Wise_Sales_Amount.Text);
                String toDateProdWiseSalesAmnt = (TextBox_To_Date_Prod_Wise_Sales_Amount.Text.Equals("") ? DateTime.Now.ToString("yyyy-MM-dd") : TextBox_To_Date_Prod_Wise_Sales_Amount.Text);

                generateProductWiseSalesAmnt(fromDateProdWiseSalesAmnt, toDateProdWiseSalesAmnt, invList);
            }
        }

        /*protected void generateSalesTransactionCharts(String fromDate, String toDate)
        {
            ActionLibrary.PurchaseActions._dispInvoiceDetails dspInv = new PurchaseActions._dispInvoiceDetails();
            Dictionary<String, String> filterParam = new Dictionary<String, String>();
            filterParam.Add(dspInv.FILTER_BY_FROM_DATE, fromDate);
            filterParam.Add(dspInv.FILTER_BY_TO_DATE, toDate);

            ArrayList invList = Invoice.getAllInvoicesbyRespEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

            //ArrayList invListFiltered=dspInv.
            //getAllInvDetailsFiltered(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), invList, filterParam);


            generateTotalBusinessForSalesByAccounts(fromDate, toDate, generatePendingClearPaymentsForSalesByAccounts
                (fromDate, toDate, invList, new Dictionary<String, String>()), new Dictionary<String, String>());
            generateProductWiseSalesQnty(fromDate, toDate, invList);
            //generateProductWiseSales(fromDate, toDate, invList, "amount");
        }*/

        protected void UpdatePanelTranPurchase_load(object sender, EventArgs e)
        {
            if (CollapsiblePanelExtender_Tran_Purchase.ClientState != null && CollapsiblePanelExtender_Tran_Purchase.ClientState.Equals("false"))
            {
                String tranpurchasePendingClearByAccntFromDate = (TextBox_From_Date_Pending_Clear_Contact.Text.Equals("") ? DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd") : TextBox_From_Date_Pending_Clear_Contact.Text);
                String tranpurchasePendingClearByAccntToDate = (TextBox_To_Date_Pending_Clear_Contact.Text.Equals("") ? DateTime.Now.ToString("yyyy-MM-dd") : TextBox_To_Date_Pending_Clear_Contact.Text);
                //This dictionary will have the customer id as the key and the name as the value
                Dictionary<String, String> custDict = new Dictionary<String, String>();
                if (ListBox_Contacts_Pending_Clear_Amnt_Purchase.GetSelectedIndices().Length != 0)
                {
                    int[] indexList = ListBox_Contacts_Pending_Clear_Amnt_Purchase.GetSelectedIndices();

                    for (int i = 0; i < indexList.Length; i++)
                        custDict.Add(ListBox_Contacts_Pending_Clear_Amnt_Purchase.Items[indexList[i]].Value, ListBox_Contacts_Pending_Clear_Amnt_Purchase.Items[indexList[i]].Text);

                }

                ArrayList invList = generatePendingClearPaymentsForPurchaseByAccounts(tranpurchasePendingClearByAccntFromDate, tranpurchasePendingClearByAccntToDate, null, custDict);

                String tranpurchaseTotalBusinessForpurchaseFromDate = (TextBox_From_Date_Chart_Purchase_Total_Business_Contact.Text.Equals("") ? DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd") : TextBox_From_Date_Chart_Purchase_Total_Business_Contact.Text);
                String tranpurchaseTotalBusinessForpurchaseToDate = (TextBox_To_Date_Chart_Purchase_Total_Business_Contact.Text.Equals("") ? DateTime.Now.ToString("yyyy-MM-dd") : TextBox_To_Date_Chart_Purchase_Total_Business_Contact.Text);
                Dictionary<String, String> custDict1 = new Dictionary<String, String>();

                if (ListBox_Contacts_Total_Business_Chart.GetSelectedIndices().Length != 0)
                {
                    int[] indexList = ListBox_Contacts_Total_Business_Chart.GetSelectedIndices();

                    for (int i = 0; i < indexList.Length; i++)
                        custDict1.Add(ListBox_Contacts_Total_Business_Chart.Items[indexList[i]].Value, ListBox_Contacts_Total_Business_Chart.Items[indexList[i]].Text);

                }

                generateTotalBusinessForPurchaseByAccounts(tranpurchaseTotalBusinessForpurchaseFromDate, tranpurchaseTotalBusinessForpurchaseToDate, invList, custDict1);

                String fromDateProdWisepurchaseQnty = (TextBox_From_Date_Prod_Wise_Purchase_Qnty.Text.Equals("") ? DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd") : TextBox_From_Date_Prod_Wise_Purchase_Qnty.Text);
                String toDateProdWisepurchaseQnty = (TextBox_To_Date_Prod_Wise_Purchase_Qnty.Text.Equals("") ? DateTime.Now.ToString("yyyy-MM-dd") : TextBox_To_Date_Prod_Wise_Purchase_Qnty.Text);

                generateProductWisePurchaseQnty(fromDateProdWisepurchaseQnty, toDateProdWisepurchaseQnty, invList);

                String fromDateProdWisepurchaseAmnt = (TextBox_From_Date_Prod_Wise_Purchase_Amount.Text.Equals("") ? DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd") : TextBox_From_Date_Prod_Wise_Purchase_Amount.Text);
                String toDateProdWisepurchaseAmnt = (TextBox_To_Date_Prod_Wise_Purchase_Amount.Text.Equals("") ? DateTime.Now.ToString("yyyy-MM-dd") : TextBox_To_Date_Prod_Wise_Purchase_Amount.Text);

                generateProductWisePurchaseAmnt(fromDateProdWisepurchaseAmnt, toDateProdWisepurchaseAmnt, invList);


            }
        }

        protected void UpdatePanelIncmDefects_load(object sender, EventArgs e)
        {
           if (CollapsiblePanelExtender_Inc_Defect.ClientState != null && CollapsiblePanelExtender_Inc_Defect.ClientState.Equals("false"))
            {
               //Incoming amount
                String incmAmntFromDate = (Textbox_From_Date_Incoming_Defect_By_Account.Text.Equals("") ? 
                    DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd") : Textbox_From_Date_Incoming_Defect_By_Account.Text);
                String incmAmntToDate = (Textbox_To_Date_Incoming_Defect_By_Account.Text.Equals("") ?
                     DateTime.Now.ToString("yyyy-MM-dd") : Textbox_To_Date_Incoming_Defect_By_Account.Text);

                               Dictionary<String, String> custDict = new Dictionary<String, String>();
                if (ListBox_Incoming_Defect_By_Account.GetSelectedIndices().Length != 0)
                {
                    int[] indexList = ListBox_Incoming_Defect_By_Account.GetSelectedIndices();

                    for (int i = 0; i < indexList.Length; i++)
                        custDict.Add(ListBox_Incoming_Defect_By_Account.Items[indexList[i]].Value, ListBox_Incoming_Defect_By_Account.Items[indexList[i]].Text);

                }
               
                generateDefectValByAccount(incmAmntFromDate, incmAmntToDate, null, custDict);


               //Incming No
                String incmNoFromDate = (Textbox_From_Date_Incoming_Defect_No_By_Account.Text.Equals("") ?
      DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd") : Textbox_From_Date_Incoming_Defect_No_By_Account.Text);
                String incmNoToDate = (Textbox_To_Date_Incoming_Defect_No_By_Account.Text.Equals("") ?
DateTime.Now.ToString("yyyy-MM-dd") : Textbox_To_Date_Incoming_Defect_No_By_Account.Text);

                Dictionary<String, String> custDict2 = new Dictionary<String, String>();
                if (Listbox_Incoming_Defect_No_By_Account.GetSelectedIndices().Length != 0)
                {
                    int[] indexList = Listbox_Incoming_Defect_No_By_Account.GetSelectedIndices();

                    for (int i = 0; i < indexList.Length; i++)
                        custDict2.Add(Listbox_Incoming_Defect_No_By_Account.Items[indexList[i]].Value, Listbox_Incoming_Defect_No_By_Account.Items[indexList[i]].Text);

                }

                generateDefectNoByAccount(incmNoFromDate, incmNoToDate, null, custDict2,DropDownList_Incoming_Defect_No_By_Account_Defect_Type.SelectedValue);

               String fromDate=(TextBox_From_Date_Incoming_Defect_Arrvl_Closure.Text.Equals("")?
                   DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd") : TextBox_From_Date_Incoming_Defect_Arrvl_Closure.Text);
               String toDate = (TextBox_To_Date_Incoming_Defect_Arrvl_Closure.Text.Equals("") ?
                   DateTime.Now.ToString("yyyy-MM-dd") : TextBox_To_Date_Incoming_Defect_Arrvl_Closure.Text);

                generateDefectArrivalandClosure(fromDate, toDate, null,null,DropDownList_Incm_Defect_Arrival_Closure_Freq.SelectedValue);

               String fromDateAvgClosingTime=(TextBox_From_Date_Incoming_Defect_Avg_Closure.Text.Equals("")?
                   DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd") : TextBox_From_Date_Incoming_Defect_Avg_Closure.Text);

               String toDateAvgClosingTime = (TextBox_To_Date_Incoming_Defect_Avg_Closure.Text.Equals("") ?
                   DateTime.Now.ToString("yyyy-MM-dd") : TextBox_To_Date_Incoming_Defect_Avg_Closure.Text);

               generateAvgDefectClosureTime(fromDateAvgClosingTime, toDateAvgClosingTime, null,DropDownList_Incm_Defect_Avg_Closure_Service_Agnt.SelectedValue,DropDownList_Incm_Defect_Avg_Closure_Freq.SelectedValue);

            }
        }

        protected void UpdatePanelOutDefects_load(object sender, EventArgs e)
        {
            if (CollapsiblePanelExtender_Out_Defect.ClientState != null && CollapsiblePanelExtender_Out_Defect.ClientState.Equals("false"))
            {
                //Outgoing Amount
                String outgAmntFromDate = (Textbox_From_Date_Outgoing_Defect_By_Account.Text.Equals("") ?
     DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd") : Textbox_From_Date_Outgoing_Defect_By_Account.Text);
                String outgAmntToDate = (Textbox_To_Date_Outgoing_Defect_By_Account.Text.Equals("") ?
                     DateTime.Now.ToString("yyyy-MM-dd") : Textbox_To_Date_Outgoing_Defect_By_Account.Text);

                Dictionary<String, String> custDict = new Dictionary<String, String>();
                if (ListBox_Outgoing_Defect_By_Account.GetSelectedIndices().Length != 0)
                {
                    int[] indexList = ListBox_Outgoing_Defect_By_Account.GetSelectedIndices();

                    for (int i = 0; i < indexList.Length; i++)
                        custDict.Add(ListBox_Outgoing_Defect_By_Account.Items[indexList[i]].Value, ListBox_Outgoing_Defect_By_Account.Items[indexList[i]].Text);

                }

                generateOutgoingDefectValByAccount(outgAmntFromDate, outgAmntToDate, null, custDict);

                //Outgoing No
                String outgNoFromDate = (Textbox_From_Date_Outgoing_Defect_No_By_Account.Text.Equals("") ?
     DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd") : Textbox_From_Date_Outgoing_Defect_No_By_Account.Text);
                String outgNoToDate = (Textbox_To_Date_Outgoing_Defect_No_By_Account.Text.Equals("") ?
                     DateTime.Now.ToString("yyyy-MM-dd") : Textbox_To_Date_Outgoing_Defect_No_By_Account.Text);

                Dictionary<String, String> custDict2 = new Dictionary<String, String>();
                if (Listbox_Outgoing_Defect_No_By_Account.GetSelectedIndices().Length != 0)
                {
                    int[] indexList = Listbox_Outgoing_Defect_No_By_Account.GetSelectedIndices();

                    for (int i = 0; i < indexList.Length; i++)
                        custDict2.Add(Listbox_Outgoing_Defect_No_By_Account.Items[indexList[i]].Value, Listbox_Outgoing_Defect_No_By_Account.Items[indexList[i]].Text);

                }

                generateOutgoingDefectNoByAccount(outgNoFromDate, outgNoToDate, null, custDict2);

                String fromDate = (TextBox_From_Date_Outgoing_Defect_Arrvl_Closure.Text.Equals("") ?
    DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd") : TextBox_From_Date_Outgoing_Defect_Arrvl_Closure.Text);
                String toDate = (TextBox_To_Date_Outgoing_Defect_Arrvl_Closure.Text.Equals("") ?
                    DateTime.Now.ToString("yyyy-MM-dd") : TextBox_To_Date_Outgoing_Defect_Arrvl_Closure.Text);

                generateDefectArrivalandClosureOutgoing(fromDate, toDate, null, null, DropDownList_Outgoing_Defect_Arrival_Closure_Freq.SelectedValue);

                String fromDateAvgClosingTime = (TextBox_From_Date_Outgoing_Defect_Avg_Closure.Text.Equals("") ?
    DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd") : TextBox_From_Date_Outgoing_Defect_Avg_Closure.Text);

                String toDateAvgClosingTime = (TextBox_To_Date_Outgoing_Defect_Avg_Closure.Text.Equals("") ?
                    DateTime.Now.ToString("yyyy-MM-dd") : TextBox_To_Date_Outgoing_Defect_Avg_Closure.Text);

                generateAvgDefectClosureTimeOutgoing(fromDateAvgClosingTime, toDateAvgClosingTime, null, DropDownList_Outg_Defect_Avg_Closure_Vendor.SelectedValue, DropDownList_Outg_Defect_Avg_Closure_Freq.SelectedValue);

            }
        }

        protected void UpdatePanelIncmSRs_load(object sender, EventArgs e)
        {
            if (CollapsiblePanelExtender_Inc_SR.ClientState != null && CollapsiblePanelExtender_Inc_SR.ClientState.Equals("false"))
            {
                //Incoming amount
                String incmAmntFromDate = (Textbox_From_Date_Incoming_SR_By_Account.Text.Equals("") ?
                    DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd") : Textbox_From_Date_Incoming_SR_By_Account.Text);
                String incmAmntToDate = (Textbox_To_Date_Incoming_SR_By_Account.Text.Equals("") ?
                     DateTime.Now.ToString("yyyy-MM-dd") : Textbox_To_Date_Incoming_SR_By_Account.Text);

                Dictionary<String, String> custDict = new Dictionary<String, String>();
                if (ListBox_Incoming_SR_By_Account.GetSelectedIndices().Length != 0)
                {
                    int[] indexList = ListBox_Incoming_SR_By_Account.GetSelectedIndices();

                    for (int i = 0; i < indexList.Length; i++)
                        custDict.Add(ListBox_Incoming_SR_By_Account.Items[indexList[i]].Value, ListBox_Incoming_SR_By_Account.Items[indexList[i]].Text);

                }

                generateSRValByAccount(incmAmntFromDate, incmAmntToDate, null, custDict);


                //Incming No
                String incmNoFromDate = (Textbox_From_Date_Incoming_SR_No_By_Account.Text.Equals("") ?
      DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd") : Textbox_From_Date_Incoming_SR_No_By_Account.Text);
                String incmNoToDate = (Textbox_To_Date_Incoming_SR_No_By_Account.Text.Equals("") ?
DateTime.Now.ToString("yyyy-MM-dd") : Textbox_To_Date_Incoming_SR_No_By_Account.Text);

                Dictionary<String, String> custDict2 = new Dictionary<String, String>();
                if (Listbox_Incoming_SR_No_By_Account.GetSelectedIndices().Length != 0)
                {
                    int[] indexList = Listbox_Incoming_SR_No_By_Account.GetSelectedIndices();

                    for (int i = 0; i < indexList.Length; i++)
                        custDict2.Add(Listbox_Incoming_SR_No_By_Account.Items[indexList[i]].Value, Listbox_Incoming_SR_No_By_Account.Items[indexList[i]].Text);

                }

                generateSRNoByAccount(incmNoFromDate, incmNoToDate, null, custDict2, DropDownList_Incoming_SR_No_By_Account_SR_Type.SelectedValue);

                String fromDate = (TextBox_From_Date_Incoming_SR_Arrvl_Closure.Text.Equals("") ?
                    DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd") : TextBox_From_Date_Incoming_SR_Arrvl_Closure.Text);
                String toDate = (TextBox_To_Date_Incoming_SR_Arrvl_Closure.Text.Equals("") ?
                    DateTime.Now.ToString("yyyy-MM-dd") : TextBox_To_Date_Incoming_SR_Arrvl_Closure.Text);

                generateSRArrivalandClosure(fromDate, toDate, null, null, DropDownList_Incm_SR_Arrival_Closure_Freq.SelectedValue);

                String fromDateAvgClosingTime = (TextBox_From_Date_Incoming_SR_Avg_Closure.Text.Equals("") ?
                    DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd") : TextBox_From_Date_Incoming_SR_Avg_Closure.Text);

                String toDateAvgClosingTime = (TextBox_To_Date_Incoming_SR_Avg_Closure.Text.Equals("") ?
                    DateTime.Now.ToString("yyyy-MM-dd") : TextBox_To_Date_Incoming_SR_Avg_Closure.Text);

                generateAvgSRClosureTime(fromDateAvgClosingTime, toDateAvgClosingTime, null, DropDownList_Incm_SR_Avg_Closure_Service_Agnt.SelectedValue, DropDownList_Incm_SR_Avg_Closure_Freq.SelectedValue);

            }
        }

        protected void UpdatePanelOutgSRs_load(object sender, EventArgs e)
        {
            if (CollapsiblePanelExtender_Out_SR.ClientState != null && CollapsiblePanelExtender_Out_SR.ClientState.Equals("false"))
            {
                //Outgoing Amount
                String outgAmntFromDate = (Textbox_From_Date_Outgoing_SR_By_Account.Text.Equals("") ?
     DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd") : Textbox_From_Date_Outgoing_SR_By_Account.Text);
                String outgAmntToDate = (Textbox_To_Date_Outgoing_SR_By_Account.Text.Equals("") ?
                     DateTime.Now.ToString("yyyy-MM-dd") : Textbox_To_Date_Outgoing_SR_By_Account.Text);

                Dictionary<String, String> custDict = new Dictionary<String, String>();
                if (ListBox_Outgoing_SR_By_Account.GetSelectedIndices().Length != 0)
                {
                    int[] indexList = ListBox_Outgoing_SR_By_Account.GetSelectedIndices();

                    for (int i = 0; i < indexList.Length; i++)
                        custDict.Add(ListBox_Outgoing_SR_By_Account.Items[indexList[i]].Value, ListBox_Outgoing_SR_By_Account.Items[indexList[i]].Text);

                }

                generateOutgoingSRValByAccount(outgAmntFromDate, outgAmntToDate, null, custDict);

                //Outgoing No
                String outgNoFromDate = (Textbox_From_Date_Outgoing_SR_No_By_Account.Text.Equals("") ?
     DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd") : Textbox_From_Date_Outgoing_SR_No_By_Account.Text);
                String outgNoToDate = (Textbox_To_Date_Outgoing_SR_No_By_Account.Text.Equals("") ?
                     DateTime.Now.ToString("yyyy-MM-dd") : Textbox_To_Date_Outgoing_SR_No_By_Account.Text);

                Dictionary<String, String> custDict2 = new Dictionary<String, String>();
                if (Listbox_Outgoing_SR_No_By_Account.GetSelectedIndices().Length != 0)
                {
                    int[] indexList = Listbox_Outgoing_SR_No_By_Account.GetSelectedIndices();

                    for (int i = 0; i < indexList.Length; i++)
                        custDict2.Add(Listbox_Outgoing_SR_No_By_Account.Items[indexList[i]].Value, Listbox_Outgoing_SR_No_By_Account.Items[indexList[i]].Text);

                }

                generateOutgoingSRNoByAccount(outgNoFromDate, outgNoToDate, null, custDict2);

                String fromDate = (TextBox_From_Date_Outgoing_SR_Arrvl_Closure.Text.Equals("") ?
    DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd") : TextBox_From_Date_Outgoing_SR_Arrvl_Closure.Text);
                String toDate = (TextBox_To_Date_Outgoing_SR_Arrvl_Closure.Text.Equals("") ?
                    DateTime.Now.ToString("yyyy-MM-dd") : TextBox_To_Date_Outgoing_SR_Arrvl_Closure.Text);

                generateSRArrivalandClosureOutgoing(fromDate, toDate, null, null, DropDownList_Outg_SR_Arrival_Closure_Freq.SelectedValue);

                String fromDateAvgClosingTime = (TextBox_From_Date_Outgoing_SR_Avg_Closure.Text.Equals("") ?
    DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd") : TextBox_From_Date_Outgoing_SR_Avg_Closure.Text);

                String toDateAvgClosingTime = (TextBox_To_Date_Outgoing_SR_Avg_Closure.Text.Equals("") ?
                    DateTime.Now.ToString("yyyy-MM-dd") : TextBox_To_Date_Outgoing_SR_Avg_Closure.Text);

                generateAvgSRClosureTimeOutgoing(fromDateAvgClosingTime, toDateAvgClosingTime, null, DropDownList_Outg_SR_Avg_Closure_Vendor.SelectedValue, DropDownList_Outg_SR_Avg_Closure_Freq.SelectedValue);

            }
        }

       /* protected void generateOutgoingDefectsCharts(String fromDate, String toDate)
        {
            if (fromDate == null || fromDate.Equals(""))
                fromDate = DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd");
            if (toDate == null || toDate.Equals(""))
                toDate = DateTime.Now.ToString("yyyy-MM-dd");

            generateOutgoingDefectValByAccount(fromDate, toDate, null, null);
            generateOutgoingDefectNoByAccount(fromDate, toDate, null, null);
        }*/

        /// <summary>
        /// For incoming defects this method creates the chart showing defects arrival and closure by severity over a given time span
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="defectList"></param>
        /// <param name="userList"></param>
        /// <param name="frequency"></param>
        protected void generateDefectArrivalandClosureOutgoing(String fromDate, String toDate, ArrayList defectList, Dictionary<String, String> userList, String frequency)
        {
            if (fromDate == null || fromDate.Equals(""))
                fromDate = DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd");
            if (toDate == null || toDate.Equals(""))
                toDate = DateTime.Now.ToString("yyyy-MM-dd");

            bool userFilterCheck = (userList != null && userList.Count > 0) ? true : false;
            bool generateChart = false;

            Dictionary<DateTime, float> totalDefectArrivalHigh = new Dictionary<DateTime, float>();
            Dictionary<DateTime, float> totalDefectArrivalLow = new Dictionary<DateTime, float>();
            Dictionary<DateTime, float> totalDefectArrivalMed = new Dictionary<DateTime, float>();
            Dictionary<DateTime, float> defectClosureHigh = new Dictionary<DateTime, float>();
            Dictionary<DateTime, float> defectClosureLow = new Dictionary<DateTime, float>();
            Dictionary<DateTime, float> defectClosureMed = new Dictionary<DateTime, float>();
            Dictionary<DateTime, String> dateRangeDict = new Dictionary<DateTime, string>();

            Dictionary<String, Dictionary<DateTime, String>> lastGeneratedDateRangeDict = (Dictionary<String, Dictionary<DateTime, String>>)Session[SessionFactory.ALL_DASHBOARD_OUTG_DEFECT_ARRVLCLOS_DATE_RANGE];
            String lastRequestedFreq = (Session[SessionFactory.ALL_DASHBOARD_OUTG_DEFECT_ARRVLCLOS_LAST_FREQ] != null ? Session[SessionFactory.ALL_DASHBOARD_OUTG_DEFECT_ARRVLCLOS_LAST_FREQ].ToString() : "");

            if (lastGeneratedDateRangeDict != null && lastGeneratedDateRangeDict.Count > 0 && lastGeneratedDateRangeDict.ContainsKey(fromDate + "-" + toDate) && frequency.Equals(lastRequestedFreq))
            {
                Dictionary<String, Dictionary<DateTime, float>> allGeneratedDicts = (Dictionary<String, Dictionary<DateTime, float>>)
                    Session[SessionFactory.ALL_DASHBOARD_OUTG_DEFECT_ARRVLCLOS_GENERATED_DICTS];

                totalDefectArrivalHigh = allGeneratedDicts["ArrivalHigh"];
                totalDefectArrivalLow = allGeneratedDicts["ArrivalLow"];
                totalDefectArrivalMed = allGeneratedDicts["ArrivalMed"];
                defectClosureHigh = allGeneratedDicts["ClosureHigh"];
                defectClosureLow = allGeneratedDicts["ClosureLow"];
                defectClosureMed = allGeneratedDicts["ClosureMed"];

                dateRangeDict = lastGeneratedDateRangeDict[fromDate + "-" + toDate];
            }
            else
            {
                generateChart = true;
                if (defectList == null || defectList.Count == 0)
                {
                    Dictionary<String, String> filterParams = new Dictionary<String, String>();
                    ActionLibrary.DefectActions._dispDefectDetails dspDefect = new DefectActions._dispDefectDetails();
                    if (fromDate != null && !fromDate.Equals(""))
                        filterParams.Add(dspDefect.FILTER_BY_SUBMIT_DATE_FROM, fromDate);
                    if (toDate != null && !toDate.Equals(""))
                        filterParams.Add(dspDefect.FILTER_BY_SUBMIT_DATE_TO, toDate);

                    defectList = dspDefect.getAllDefectsFilteredORDERBYCreateDate("outgoing", Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), filterParams);
                    if (defectList != null && defectList.Count > 0)
                    {
                        String lowerDate = ((DefectDetails)defectList[0]).getDateCreated().Substring(0, ((DefectDetails)defectList[0]).getDateCreated().IndexOf(" "));
                        String higherDate = ((DefectDetails)defectList[defectList.Count - 1]).getDateCreated().Substring(0, ((DefectDetails)defectList[defectList.Count - 1]).getDateCreated().IndexOf(" "));

                        dateRangeDict = getDateRange(lowerDate, higherDate, frequency);
                        List<DateTime> dataRangeList = dateRangeDict.Keys.ToList();
                        dataRangeList.Sort();
                        //Make sure all the dictionaries have the same key set
                        foreach (KeyValuePair<DateTime, String> kvp in dateRangeDict)
                        {
                            if (!totalDefectArrivalHigh.ContainsKey(kvp.Key))
                                totalDefectArrivalHigh.Add(kvp.Key, 0);

                            if (!totalDefectArrivalLow.ContainsKey(kvp.Key))
                                totalDefectArrivalLow.Add(kvp.Key, 0);

                            if (!totalDefectArrivalMed.ContainsKey(kvp.Key))
                                totalDefectArrivalMed.Add(kvp.Key, 0);

                            if (!defectClosureHigh.ContainsKey(kvp.Key))
                                defectClosureHigh.Add(kvp.Key, 0);

                            if (!defectClosureLow.ContainsKey(kvp.Key))
                                defectClosureLow.Add(kvp.Key, 0);

                            if (!defectClosureMed.ContainsKey(kvp.Key))
                                defectClosureMed.Add(kvp.Key, 0);
                        }

                        for (int i = 0; i < defectList.Count; i++)
                        {
                            DefectDetails defObj = (DefectDetails)defectList[i];
                            /*String closeDate = defObj.getCloseDate() != null && !defObj.getCloseDate().Equals("") && !(defObj.getCloseDate().IndexOf("1/1/1900")>=0)?
                                defObj.getCloseDate().Replace(" ", "  ").Substring(0, 9) : "";
                            String createDate = defObj.getDateCreated().Replace(" ", "  ").Substring(0, 9);*/
                            String closeDate = defObj.getCloseDate() != null && !defObj.getCloseDate().Equals("") && !(defObj.getCloseDate().IndexOf("1/1/1900") >= 0) ?
                                (defObj.getCloseDate().IndexOf(" ")>=0? defObj.getCloseDate().Substring(0,defObj.getCloseDate().IndexOf(" ")):defObj.getCloseDate()): "";
                            String createDate = defObj.getDateCreated().IndexOf(" ") >= 0 ? defObj.getDateCreated().Substring(0, defObj.getDateCreated().IndexOf(" ")) : defObj.getDateCreated();

                            int index = dataRangeList.BinarySearch(Convert.ToDateTime(createDate));
                            if (index >= 0)
                            {
                                //Exact Match Found
                                if (defObj.getSeverity().Equals("High", StringComparison.InvariantCultureIgnoreCase))
                                    totalDefectArrivalHigh[dataRangeList[index]] += 1;
                                if (defObj.getSeverity().Equals("Medium", StringComparison.InvariantCultureIgnoreCase))
                                    totalDefectArrivalMed[dataRangeList[index]] += 1;
                                if (defObj.getSeverity().Equals("Low", StringComparison.InvariantCultureIgnoreCase))
                                    totalDefectArrivalLow[dataRangeList[index]] += 1;
                            }
                            else
                            {
                                //Nearest Greater Value Found
                                index = ~index;
                                if (defObj.getSeverity().Equals("High", StringComparison.InvariantCultureIgnoreCase))
                                    totalDefectArrivalHigh[dataRangeList[index - 1]] += 1;
                                if (defObj.getSeverity().Equals("Medium", StringComparison.InvariantCultureIgnoreCase))
                                    totalDefectArrivalMed[dataRangeList[index - 1]] += 1;
                                if (defObj.getSeverity().Equals("Low", StringComparison.InvariantCultureIgnoreCase))
                                    totalDefectArrivalLow[dataRangeList[index - 1]] += 1;
                            }

                            if (closeDate != null && !closeDate.Equals(""))
                            {
                                index = dataRangeList.BinarySearch(Convert.ToDateTime(closeDate));
                                if (index >= 0)
                                {
                                    //Exact Match Found
                                    if (defObj.getSeverity().Equals("High", StringComparison.InvariantCultureIgnoreCase))
                                        defectClosureHigh[dataRangeList[index]] += 1;
                                    if (defObj.getSeverity().Equals("Medium", StringComparison.InvariantCultureIgnoreCase))
                                        defectClosureMed[dataRangeList[index]] += 1;
                                    if (defObj.getSeverity().Equals("Low", StringComparison.InvariantCultureIgnoreCase))
                                        defectClosureLow[dataRangeList[index]] += 1;
                                }
                                else
                                {
                                    //Nearest Greater Value Found
                                    index = ~index;
                                    if (defObj.getSeverity().Equals("High", StringComparison.InvariantCultureIgnoreCase))
                                        defectClosureHigh[dataRangeList[index - 1]] += 1;
                                    if (defObj.getSeverity().Equals("Medium", StringComparison.InvariantCultureIgnoreCase))
                                        defectClosureMed[dataRangeList[index - 1]] += 1;
                                    if (defObj.getSeverity().Equals("Low", StringComparison.InvariantCultureIgnoreCase))
                                        defectClosureLow[dataRangeList[index - 1]] += 1;
                                }
                            }
                        }
                    }

                }

                lastGeneratedDateRangeDict = new Dictionary<string, Dictionary<DateTime, string>>(); lastGeneratedDateRangeDict.Add(fromDate + "-" + toDate, dateRangeDict);
                Dictionary<String, Dictionary<DateTime, float>> allGeneratedDicts = new Dictionary<string, Dictionary<DateTime, float>>();
                allGeneratedDicts.Add("ArrivalHigh", totalDefectArrivalHigh);
                allGeneratedDicts.Add("ArrivalLow", totalDefectArrivalLow);
                allGeneratedDicts.Add("ArrivalMed", totalDefectArrivalMed);
                allGeneratedDicts.Add("ClosureHigh", defectClosureHigh);
                allGeneratedDicts.Add("ClosureLow", defectClosureLow);
                allGeneratedDicts.Add("ClosureMed", defectClosureMed);

                Session[SessionFactory.ALL_DASHBOARD_OUTG_DEFECT_ARRVLCLOS_GENERATED_DICTS] = allGeneratedDicts;
                Session[SessionFactory.ALL_DASHBOARD_OUTG_DEFECT_ARRVLCLOS_DATE_RANGE] = lastGeneratedDateRangeDict;
                Session[SessionFactory.ALL_DASHBOARD_OUTG_DEFECT_ARRVLCLOS_LAST_FREQ] = frequency;

            }

            float[] totalDefectArrivalHighArray = totalDefectArrivalHigh.Values.ToArray();
            float[] totalDefectArrivalLowArray = totalDefectArrivalLow.Values.ToArray();
            float[] totalDefectArrivalMedArray = totalDefectArrivalMed.Values.ToArray();
            float[] defectClosureHighArray = defectClosureHigh.Values.ToArray();
            float[] defectClosureLowArray = defectClosureLow.Values.ToArray();
            float[] defectClosureMedArray = defectClosureMed.Values.ToArray();
            String[] dateRangeDictArray = dateRangeDict.Values.ToArray();

            if (generateChart)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Date Range");
                dt.Columns.Add("No of High Sev Created");
                dt.Columns.Add("No of High Sev Closed");
                dt.Columns.Add("No of Medium Sev Created");
                dt.Columns.Add("No of Medium Sev Closed");
                dt.Columns.Add("No of Low Sev Created");
                dt.Columns.Add("No of Low Sev Closed");

                for (int i = 0; i < dateRangeDictArray.Length; i++)
                {
                    dt.Rows.Add();
                    dt.Rows[i]["Date Range"] = dateRangeDictArray[i];
                    dt.Rows[i]["No of High Sev Created"] = totalDefectArrivalHighArray[i];
                    dt.Rows[i]["No of High Sev Closed"] = defectClosureHighArray[i];
                    dt.Rows[i]["No of Medium Sev Created"] = totalDefectArrivalMedArray[i];
                    dt.Rows[i]["No of Medium Sev Closed"] = defectClosureMedArray[i];
                    dt.Rows[i]["No of Low Sev Created"] = totalDefectArrivalLowArray[i];
                    dt.Rows[i]["No of Low Sev Closed"] = defectClosureLowArray[i];

                }

                Dictionary<String, DataTable> reportDict = (Dictionary<String, DataTable>)Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES];
                if (reportDict == null)
                    reportDict = new Dictionary<string, DataTable>();

                if (!reportDict.ContainsKey("outgoing_Defect_Arrival_Closure"))
                    reportDict.Add("outgoing_Defect_Arrival_Closure", dt);
                else
                    reportDict["outgoing_Defect_Arrival_Closure"] = dt;

                Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES] = reportDict;
            }
            
            //Chart_Defect_Arrival_Closure.Titles.Clear();
            //Chart_Defect_Arrival_Closure.Titles.Add("Defects Arrival and Closure By Severity for Customers during the period");
            Chart_Out_Defect_Arrval_Closure.Titles[0].Font = new System.Drawing.Font("Book Antiqua", 10);
            Chart_Out_Defect_Arrval_Closure.Series["Series_Defects_High"].Label = "#VALY{0;0;#}";
            Chart_Out_Defect_Arrval_Closure.Series["Series_Defects_Medium"].Label = "#VALY{0;0;#}";
            Chart_Out_Defect_Arrval_Closure.Series["Series_Defects_Low"].Label = "#VALY{0;0;#}";
            Chart_Out_Defect_Arrval_Closure.Series["Series_Closure_High"].Label = "#VALY{0;0;#}";
            Chart_Out_Defect_Arrval_Closure.Series["Series_Closure_Medium"].Label = "#VALY{0;0;#}";
            Chart_Out_Defect_Arrval_Closure.Series["Series_Closure_Low"].Label = "#VALY{0;0;#}";

            Chart_Out_Defect_Arrval_Closure.Series["Series_Defects_High"].Points.DataBindXY(dateRangeDictArray, totalDefectArrivalHighArray);
            Chart_Out_Defect_Arrval_Closure.Series["Series_Defects_Medium"].Points.DataBindXY(dateRangeDictArray, totalDefectArrivalMedArray);
            Chart_Out_Defect_Arrval_Closure.Series["Series_Defects_Low"].Points.DataBindXY(dateRangeDictArray, totalDefectArrivalLowArray);
            Chart_Out_Defect_Arrval_Closure.Series["Series_Closure_High"].Points.DataBindXY(dateRangeDictArray, defectClosureHighArray);
            Chart_Out_Defect_Arrval_Closure.Series["Series_Closure_Medium"].Points.DataBindXY(dateRangeDictArray, defectClosureMedArray);
            Chart_Out_Defect_Arrval_Closure.Series["Series_Closure_Low"].Points.DataBindXY(dateRangeDictArray, defectClosureLowArray);

            //Remove empty values

            //String[] seriesArray = { "Series_Defects_High", "Series_Defects_Medium", "Series_Defects_Low", "Series_Closure_High", "Series_Closure_Medium", "Series_Closure_Low" };

            //removeZeroValues(Chart_Out_Defect_Arrval_Closure, seriesArray);
        }
             

        protected void generateAvgDefectClosureTimeOutgoing(String fromDate, String toDate, ArrayList defectList, String selectedVendor, String frequency)
        {
            if (fromDate == null || fromDate.Equals(""))
                fromDate = DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd");
            if (toDate == null || toDate.Equals(""))
                toDate = DateTime.Now.ToString("yyyy-MM-dd");

            bool vendFilterCheck = (selectedVendor != null && !selectedVendor.Equals("All")) ? true : false;
            bool generateChart = false;

            Dictionary<DateTime, double> defectClosureAvgTotal = new Dictionary<DateTime, double>();
            Dictionary<DateTime, double> defectClosureAvgHigh = new Dictionary<DateTime, double>();
            Dictionary<DateTime, double> defectClosureAvgLow = new Dictionary<DateTime, double>();
            Dictionary<DateTime, double> defectClosureAvgMed = new Dictionary<DateTime, double>();
            Dictionary<DateTime, String> dateRangeDict = new Dictionary<DateTime, string>();

            Dictionary<String, Dictionary<DateTime, String>> lastGeneratedDateRangeDict = (Dictionary<String, Dictionary<DateTime, String>>)
                Session[SessionFactory.ALL_DASHBOARD_OUTG_DEFECT_AVERAGECLOSTIME_DATE_RANGE];

            String lastRequestedFreq = (Session[SessionFactory.ALL_DASHBOARD_OUTG_DEFECT_AVERAGECLOSTIME_LAST_FREQ] != null ?
                Session[SessionFactory.ALL_DASHBOARD_OUTG_DEFECT_AVERAGECLOSTIME_LAST_FREQ].ToString() : "");

            String lastSelectedAgent = (Session[SessionFactory.ALL_DASHBOARD_OUTG_DEFECT_AVERAGECLOSTIME_LAST_SELECTED_AGENT] != null ?
                Session[SessionFactory.ALL_DASHBOARD_OUTG_DEFECT_AVERAGECLOSTIME_LAST_SELECTED_AGENT].ToString() : "");

            if (lastGeneratedDateRangeDict != null && lastGeneratedDateRangeDict.Count > 0 && lastGeneratedDateRangeDict.ContainsKey(fromDate + "-" + toDate) && frequency.Equals(lastRequestedFreq) && lastSelectedAgent.Equals(selectedVendor))
            {
                Dictionary<String, Dictionary<DateTime, double>> allGeneratedDicts = (Dictionary<String, Dictionary<DateTime, double>>)
                    Session[SessionFactory.ALL_DASHBOARD_OUTG_DEFECT_AVERAGECLOSTIME_GENERATED_DICTS];

                defectClosureAvgTotal = allGeneratedDicts["ClosureTotal"];
                defectClosureAvgHigh = allGeneratedDicts["ClosureHigh"];
                defectClosureAvgLow = allGeneratedDicts["ClosureLow"];
                defectClosureAvgMed = allGeneratedDicts["ClosureMed"];

                dateRangeDict = lastGeneratedDateRangeDict[fromDate + "-" + toDate];
            }
            else
            {
                int rowCount = 0; 
                generateChart = true;
                
                if (defectList == null || defectList.Count == 0)
                {
                    Dictionary<String, String> filterParams = new Dictionary<String, String>();
                    ActionLibrary.DefectActions._dispDefectDetails dspDefect = new DefectActions._dispDefectDetails();
                    if (fromDate != null && !fromDate.Equals(""))
                        filterParams.Add(dspDefect.FILTER_BY_SUBMIT_DATE_FROM, fromDate);
                    if (toDate != null && !toDate.Equals(""))
                        filterParams.Add(dspDefect.FILTER_BY_SUBMIT_DATE_TO, toDate);

                    defectList = dspDefect.getAllDefectsFilteredORDERBYCreateDate("outgoing", Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), filterParams);

                    Dictionary<DateTime, ArrayList> defectClosureAvgHighTemp = new Dictionary<DateTime, ArrayList>();
                    Dictionary<DateTime, ArrayList> defectClosureAvgLowTemp = new Dictionary<DateTime, ArrayList>();
                    Dictionary<DateTime, ArrayList> defectClosureAvgMedTemp = new Dictionary<DateTime, ArrayList>();
                    Dictionary<DateTime, ArrayList> defectClosureAvgTotalTemp = new Dictionary<DateTime, ArrayList>();

                    if (defectList != null && defectList.Count > 0)
                    {
                        String lowerDate = ((DefectDetails)defectList[0]).getDateCreated().Substring(0, ((DefectDetails)defectList[0]).getDateCreated().IndexOf(" "));
                        String higherDate = ((DefectDetails)defectList[defectList.Count - 1]).getDateCreated().Substring(0, ((DefectDetails)defectList[defectList.Count - 1]).getDateCreated().IndexOf(" "));

                        dateRangeDict = getDateRange(lowerDate, higherDate, frequency);
                        List<DateTime> dataRangeList = dateRangeDict.Keys.ToList();
                        dataRangeList.Sort();
                        //Make sure all the dictionaries have the same key set
                        foreach (KeyValuePair<DateTime, String> kvp in dateRangeDict)
                        {
                            if (!defectClosureAvgTotal.ContainsKey(kvp.Key))
                            { defectClosureAvgTotal.Add(kvp.Key, 0); defectClosureAvgTotalTemp.Add(kvp.Key, new ArrayList()); }

                            if (!defectClosureAvgHigh.ContainsKey(kvp.Key))
                            { defectClosureAvgHigh.Add(kvp.Key, 0); defectClosureAvgHighTemp.Add(kvp.Key, new ArrayList()); }

                            if (!defectClosureAvgLow.ContainsKey(kvp.Key))
                            { defectClosureAvgLow.Add(kvp.Key, 0); defectClosureAvgLowTemp.Add(kvp.Key, new ArrayList()); }

                            if (!defectClosureAvgMed.ContainsKey(kvp.Key))
                            { defectClosureAvgMed.Add(kvp.Key, 0); defectClosureAvgMedTemp.Add(kvp.Key, new ArrayList()); }
                        }

                        int index = 0;
                        bool considerRecord = true;

                        for (int i = 0; i < defectList.Count; i++)
                        {
                            DefectDetails defObj = (DefectDetails)defectList[i];
                            considerRecord = true;
                            if (vendFilterCheck)
                            {
                                if (selectedVendor.Equals(defObj.getSupplierId()))
                                    considerRecord = true;
                                else
                                    considerRecord = false;
                            }

                            if (considerRecord)
                            {
                                /*String closeDate = defObj.getCloseDate() != null && !defObj.getCloseDate().Equals("") && !(defObj.getCloseDate().IndexOf("1/1/1900") >= 0) ?
                                    defObj.getCloseDate().Replace(" ", "  ").Substring(0, 9) : "";
                                String createDate = defObj.getDateCreated().Replace(" ", "  ").Substring(0, 9);*/
                                String closeDate = defObj.getCloseDate() != null && !defObj.getCloseDate().Equals("") && !(defObj.getCloseDate().IndexOf("1/1/1900") >= 0) ?
    (defObj.getCloseDate().IndexOf(" ") >= 0 ? defObj.getCloseDate().Substring(0, defObj.getCloseDate().IndexOf(" ")) : defObj.getCloseDate()) : "";
                                String createDate = defObj.getDateCreated().IndexOf(" ") >= 0 ? defObj.getDateCreated().Substring(0, defObj.getDateCreated().IndexOf(" ")) : defObj.getDateCreated();


                                if (closeDate != null && !closeDate.Equals(""))
                                {
                                    index = dataRangeList.BinarySearch(Convert.ToDateTime(createDate));
                                    double timeTakenInHours = Convert.ToDateTime(defObj.getCloseDate()).Subtract(Convert.ToDateTime(defObj.getDateCreated())).TotalHours;                                    
                                    timeTakenInHours = Math.Round(timeTakenInHours, 2);

                                    if (index >= 0)
                                    {
                                        //Exact Match Found
                                        if (defObj.getSeverity().Equals("High", StringComparison.InvariantCultureIgnoreCase))
                                            defectClosureAvgHighTemp[dataRangeList[index]].Add(timeTakenInHours);
                                        if (defObj.getSeverity().Equals("Medium", StringComparison.InvariantCultureIgnoreCase))
                                            defectClosureAvgMedTemp[dataRangeList[index]].Add(timeTakenInHours);
                                        if (defObj.getSeverity().Equals("Low", StringComparison.InvariantCultureIgnoreCase))
                                            defectClosureAvgLowTemp[dataRangeList[index]].Add(timeTakenInHours);

                                        defectClosureAvgTotalTemp[dataRangeList[index]].Add(timeTakenInHours);
                                    }
                                    else
                                    {
                                        //Nearest Greater Value Found
                                        index = ~index;
                                        if (defObj.getSeverity().Equals("High", StringComparison.InvariantCultureIgnoreCase))
                                            defectClosureAvgHighTemp[dataRangeList[index - 1]].Add(timeTakenInHours);
                                        if (defObj.getSeverity().Equals("Medium", StringComparison.InvariantCultureIgnoreCase))
                                            defectClosureAvgMedTemp[dataRangeList[index - 1]].Add(timeTakenInHours);
                                        if (defObj.getSeverity().Equals("Low", StringComparison.InvariantCultureIgnoreCase))
                                            defectClosureAvgLowTemp[dataRangeList[index - 1]].Add(timeTakenInHours);

                                        defectClosureAvgTotalTemp[dataRangeList[index - 1]].Add(timeTakenInHours);
                                    }
                                }

                            }
                        }
                    }

                    foreach (KeyValuePair<DateTime, ArrayList> kvp in defectClosureAvgTotalTemp)
                    {
                        ArrayList totalAvgList = kvp.Value;
                        ArrayList medAvgList = defectClosureAvgMedTemp[kvp.Key];
                        ArrayList lowAvgList = defectClosureAvgLowTemp[kvp.Key];
                        ArrayList highAvgList = defectClosureAvgHighTemp[kvp.Key];

                        double tempSum = 0;
                        for (int i = 0; i < totalAvgList.Count; i++)
                            tempSum += (double)totalAvgList[i];

                        if (tempSum > 0)
                            defectClosureAvgTotal[kvp.Key] = tempSum / (totalAvgList.Count);

                        tempSum = 0;
                        for (int i = 0; i < highAvgList.Count; i++)
                            tempSum += (double)highAvgList[i];

                        if (tempSum > 0)
                            defectClosureAvgHigh[kvp.Key] = tempSum / (highAvgList.Count);

                        tempSum = 0;
                        for (int i = 0; i < lowAvgList.Count; i++)
                            tempSum += (double)lowAvgList[i];

                        if (tempSum > 0)
                            defectClosureAvgLow[kvp.Key] = tempSum / (lowAvgList.Count);

                        tempSum = 0;
                        for (int i = 0; i < medAvgList.Count; i++)
                            tempSum += (double)medAvgList[i];

                        if (tempSum > 0)
                            defectClosureAvgMed[kvp.Key] = tempSum / (medAvgList.Count);
                    }
                }

                lastGeneratedDateRangeDict = new Dictionary<string, Dictionary<DateTime, string>>(); lastGeneratedDateRangeDict.Add(fromDate + "-" + toDate, dateRangeDict);
                Dictionary<String, Dictionary<DateTime, double>> allGeneratedDicts = new Dictionary<string, Dictionary<DateTime, double>>();


                allGeneratedDicts.Add("ClosureTotal", defectClosureAvgTotal);
                allGeneratedDicts.Add("ClosureHigh", defectClosureAvgHigh);
                allGeneratedDicts.Add("ClosureLow", defectClosureAvgLow);
                allGeneratedDicts.Add("ClosureMed", defectClosureAvgMed);

                Session[SessionFactory.ALL_DASHBOARD_OUTG_DEFECT_AVERAGECLOSTIME_GENERATED_DICTS] = allGeneratedDicts;
                Session[SessionFactory.ALL_DASHBOARD_OUTG_DEFECT_AVERAGECLOSTIME_DATE_RANGE] = lastGeneratedDateRangeDict;
                Session[SessionFactory.ALL_DASHBOARD_OUTG_DEFECT_AVERAGECLOSTIME_LAST_FREQ] = frequency;
                Session[SessionFactory.ALL_DASHBOARD_OUTG_DEFECT_AVERAGECLOSTIME_LAST_SELECTED_AGENT] = selectedVendor;

            }

            double[] defectClosureTotalArray = defectClosureAvgTotal.Values.ToArray();
            double[] defectClosureHighArray = defectClosureAvgHigh.Values.ToArray();
            double[] defectClosureLowArray = defectClosureAvgLow.Values.ToArray();
            double[] defectClosureMedArray = defectClosureAvgMed.Values.ToArray();
            String[] dateRangeDictArray = dateRangeDict.Values.ToArray();

            if (generateChart)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Date Range");
                dt.Columns.Add("Vendor");
                dt.Columns.Add("High Sev Average (Hours)");
                dt.Columns.Add("Medium Sev Average (Hours)");
                dt.Columns.Add("Low Sev Average (Hours)");
                dt.Columns.Add("Combined Average (Hours)");

                for (int i = 0; i < dateRangeDict.Count; i++)
                {
                    dt.Rows.Add();
                    dt.Rows[i]["Date Range"] = dateRangeDictArray[i];
                    dt.Rows[i]["Vendor"] = DropDownList_Outg_Defect_Avg_Closure_Vendor.SelectedItem.Text;
                    dt.Rows[i]["High Sev Average (Hours)"] = defectClosureHighArray[i];
                    dt.Rows[i]["Medium Sev Average (Hours)"] = defectClosureMedArray[i];
                    dt.Rows[i]["Low Sev Average (Hours)"] = defectClosureLowArray[i];
                    dt.Rows[i]["Combined Average (Hours)"] = defectClosureTotalArray[i];
                }

                Dictionary<String, DataTable> reportDict = (Dictionary<String, DataTable>)Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES];
                if (reportDict == null)
                    reportDict = new Dictionary<string, DataTable>();

                if (!reportDict.ContainsKey("outgoing_Defect_Avg_Closure"))
                    reportDict.Add("outgoing_Defect_Avg_Closure", dt);
                else
                    reportDict["outgoing_Defect_Avg_Closure"] = dt;

                Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES] = reportDict;

            }

            //Chart_Defect_Arrival_Closure.Titles.Clear();
            //Chart_Defect_Arrival_Closure.Titles.Add("Defects Arrival and Closure By Severity for Customers during the period");
            Chart_Outg_Defect_Closure_Avg_Time.Titles[0].Font = new System.Drawing.Font("Book Antiqua", 10);
            Chart_Outg_Defect_Closure_Avg_Time.Series["Series_Defects_Closure_Total"].Label = "#VALY{0;0;#}";
            Chart_Outg_Defect_Closure_Avg_Time.Series["Series_Closure_High"].Label = "#VALY{0;0;#}";
            Chart_Outg_Defect_Closure_Avg_Time.Series["Series_Closure_Medium"].Label = "#VALY{0;0;#}";
            Chart_Outg_Defect_Closure_Avg_Time.Series["Series_Closure_Low"].Label = "#VALY{0;0;#}";

            Chart_Outg_Defect_Closure_Avg_Time.Series["Series_Defects_Closure_Total"].Points.DataBindXY(dateRangeDictArray, defectClosureTotalArray);
            Chart_Outg_Defect_Closure_Avg_Time.Series["Series_Closure_High"].Points.DataBindXY(dateRangeDictArray, defectClosureHighArray);
            Chart_Outg_Defect_Closure_Avg_Time.Series["Series_Closure_Medium"].Points.DataBindXY(dateRangeDictArray, defectClosureMedArray);
            Chart_Outg_Defect_Closure_Avg_Time.Series["Series_Closure_Low"].Points.DataBindXY(dateRangeDictArray, defectClosureLowArray);

        }

        protected void generateOutgoingDefectValByAccount(String fromDate, String toDate, Dictionary<String, DefectDetails> defectDict, Dictionary<String, String> vendList)
        {
            if (fromDate == null || fromDate.Equals(""))
                fromDate = DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd");
            if (toDate == null || toDate.Equals(""))
                toDate = DateTime.Now.ToString("yyyy-MM-dd");

            bool vendFilterCheck = (vendList != null && vendList.Count > 0) ? true : false;
            bool regenerateValues = false;

            Dictionary<String, String> contactDict = new Dictionary<String, String>();
            Dictionary<String, float> totalDefectAmount = new Dictionary<string, float>();

            Dictionary<String, Dictionary<String, String>> lastGeneratedContactDict = (Dictionary<String, Dictionary<String, String>>)
    Session[SessionFactory.ALL_DASHBOARD_OUTG_DEFECT_DEFECTVALBYACCNT_CONTACT_DICT];

            if (lastGeneratedContactDict != null && lastGeneratedContactDict.Count > 0 && lastGeneratedContactDict.ContainsKey(fromDate + "," + toDate))
            {
                totalDefectAmount = ((Dictionary<String, Dictionary<String, float>>)Session[SessionFactory.ALL_DASHBOARD_OUTG_DEFECT_DEFECTVALBYACCNT_DEFECT_AMNT])[fromDate + "," + toDate];

                if (vendFilterCheck)
                {
                    Dictionary<String, float> totalDefectAmountTemp = new Dictionary<String, float>();

                    foreach (KeyValuePair<String, String> kvp in vendList)
                    {
                        if (totalDefectAmount.ContainsKey(kvp.Key))
                            totalDefectAmountTemp.Add(kvp.Key, totalDefectAmount[kvp.Key]);
                        else
                            regenerateValues = true;
                    }
                    totalDefectAmount = totalDefectAmountTemp;
                }
            }
            if (lastGeneratedContactDict != null && lastGeneratedContactDict.Count > 0 && lastGeneratedContactDict.ContainsKey(fromDate + "," + toDate) & !regenerateValues)
            {
                contactDict = lastGeneratedContactDict[fromDate + "," + toDate];
            }
            else
            {
                totalDefectAmount = new Dictionary<string, float>();

                int rowCount = 0;
                DataTable dt = new DataTable();
                dt.Columns.Add("Vendor Name");
                dt.Columns.Add("Defect#");
                dt.Columns.Add("Description");
                dt.Columns.Add("Defect Date");
                dt.Columns.Add("Defect Amount");
                dt.Columns.Add("Defect Status");
                dt.Columns.Add("Defect Resolution Status");

                if (defectDict == null || defectDict.Count == 0)
                {
                    Dictionary<String, String> filterParams = new Dictionary<String, String>();
                    ActionLibrary.DefectActions._dispDefectDetails dspDefect = new DefectActions._dispDefectDetails();
                    if (fromDate != null && !fromDate.Equals(""))
                        filterParams.Add(dspDefect.FILTER_BY_SUBMIT_DATE_FROM, fromDate);
                    if (toDate != null && !toDate.Equals(""))
                        filterParams.Add(dspDefect.FILTER_BY_SUBMIT_DATE_TO, toDate);

                    defectDict = dspDefect.getAllDefectsFiltered("outgoing", Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), filterParams);
                }

                foreach (KeyValuePair<String, DefectDetails> kvp in defectDict)
                {
                    DefectDetails defectObj = kvp.Value;

                    String vendEntId = defectObj.getSupplierId();
                    String vendName = "";
                    if (defectObj.getCreationMode().Equals(BackEndObjects.DefectDetails.DEFECT_CREATION_MODE_MANUAL))
                    {
                        vendName = Contacts.getContactDetailsforContactEntityDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), vendEntId).getContactName();
                        if (vendName == null || vendName.Equals(""))
                            vendName = MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(vendEntId).getEntityName();
                    }
                    else
                        vendName = MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(vendEntId).getEntityName();

                    bool considerVendEnt = vendFilterCheck ? vendList.ContainsKey(vendEntId) : true;

                    if (considerVendEnt)
                    {
                        if (!contactDict.ContainsKey(vendEntId))
                        {
                            Contacts contactObj = Contacts.
                                getContactDetailsforContactEntityDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), vendEntId);
                            if (contactObj == null || contactObj.getContactName() == null || contactObj.getContactName().Equals(""))
                                contactDict.Add(vendEntId, MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(vendEntId).getEntityName());
                            else
                                contactDict.Add(vendEntId, contactObj.getContactName());
                        }

                        if (!totalDefectAmount.ContainsKey(vendEntId))
                            totalDefectAmount.Add(vendEntId, defectObj.getTotalAmount());
                        else
                            totalDefectAmount[vendEntId] += defectObj.getTotalAmount();

                        dt.Rows.Add();

                        dt.Rows[rowCount]["Vendor Name"] = contactDict[vendEntId];
                        dt.Rows[rowCount]["Defect#"] = defectObj.getDefectId();
                        dt.Rows[rowCount]["Description"] = defectObj.getDescription();
                        dt.Rows[rowCount]["Defect Date"] = defectObj.getDateCreated();
                        dt.Rows[rowCount]["Defect Amount"] = defectObj.getTotalAmount();
                        dt.Rows[rowCount]["Defect Status"] = defectObj.getDefectStat();
                        dt.Rows[rowCount]["Defect Resolution Status"] = defectObj.getResolStat();

                        rowCount++;
                    }
                }

                Dictionary<String, DataTable> reportDict = (Dictionary<String, DataTable>)Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES];

                if (reportDict == null)
                    reportDict = new Dictionary<string, DataTable>();

                if (!reportDict.ContainsKey("outgoing_Defect_By_Accnt"))
                    reportDict.Add("outgoing_Defect_By_Accnt", dt);
                else
                    reportDict["outgoing_Defect_By_Accnt"] = dt;

                Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES] = reportDict;

                lastGeneratedContactDict = new Dictionary<string, Dictionary<string, string>>(); lastGeneratedContactDict.Add(fromDate + "," + toDate, contactDict);
                Dictionary<String, Dictionary<String, float>> lastGeneratedAmntDict = new Dictionary<string, Dictionary<string, float>>(); lastGeneratedAmntDict.Add(fromDate + "," + toDate, totalDefectAmount);

                Session[SessionFactory.ALL_DASHBOARD_OUTG_DEFECT_DEFECTVALBYACCNT_CONTACT_DICT] = lastGeneratedContactDict;
                Session[SessionFactory.ALL_DASHBOARD_OUTG_DEFECT_DEFECTVALBYACCNT_DEFECT_AMNT] = lastGeneratedAmntDict;


            }
            //Now populate the List box with customer details
            foreach (KeyValuePair<String, String> kvp in contactDict)
            {
                ListItem ltExists = ListBox_Outgoing_Defect_By_Account.Items.FindByValue(kvp.Key);
                if (ltExists == null || ltExists.Text.Equals(""))
                {
                    ltExists = new ListItem();
                    ltExists.Text = kvp.Value;
                    ltExists.Value = kvp.Key;

                    ListBox_Outgoing_Defect_By_Account.Items.Add(ltExists);
                }
            }

            String[] vendorArray = new String[contactDict.Count];
            float[] totalDefectAmountArray = new float[contactDict.Count];
            int counter = 0;
            foreach (KeyValuePair<String, float> kvp in totalDefectAmount)
            {
                vendorArray[counter] = contactDict[kvp.Key];
                totalDefectAmountArray[counter] = totalDefectAmount[kvp.Key];
                counter++;
            }

            Chart_Outgoing_Defect_By_Account.Titles.Clear();
            Chart_Outgoing_Defect_By_Account.Titles.Add("Total Invoice Value of Defects for Vendors during this period (defect submit date)");
            Chart_Outgoing_Defect_By_Account.Titles[0].Font = new System.Drawing.Font("Book Antiqua", 10);
            Chart_Outgoing_Defect_By_Account.Series["TotalDefects"].Label = "#VALY{0;0;#}";
            Chart_Outgoing_Defect_By_Account.Series["TotalDefects"].Points.DataBindXY(vendorArray, totalDefectAmountArray);

        }
        
        protected void generateOutgoingDefectNoByAccount(String fromDate, String toDate, Dictionary<String, DefectDetails> defectDict, Dictionary<String, String> vendList)
        {
            if (fromDate == null || fromDate.Equals(""))
                fromDate = DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd");
            if (toDate == null || toDate.Equals(""))
                toDate = DateTime.Now.ToString("yyyy-MM-dd");

            bool vendFilterCheck = (vendList != null && vendList.Count > 0) ? true : false;
            bool regenerateValues = false;

            Dictionary<String, String> contactDict = new Dictionary<String, String>();
            Dictionary<String, float> totalDefectAmount = new Dictionary<string, float>();
            Dictionary<String, float> highDefect = new Dictionary<string, float>();
            Dictionary<String, float> mediumDefect = new Dictionary<string, float>();
            Dictionary<String, float> lowDefect = new Dictionary<string, float>();

            Dictionary<String, Dictionary<String, String>> lastGeneratedContactDict = ((Dictionary<String, Dictionary<String, String>>)
    Session[SessionFactory.ALL_DASHBOARD_OUTG_DEFECT_DEFECTNOBYACCNT_CONTACT_DICT]);

            if (lastGeneratedContactDict != null && lastGeneratedContactDict.Count > 0 && lastGeneratedContactDict.ContainsKey(fromDate + "," + toDate))
            {
                totalDefectAmount = ((Dictionary<String, Dictionary<String, float>>)Session[SessionFactory.ALL_DASHBOARD_OUTG_DEFECT_DEFECTNOBYACCNT_TOTAL_DEFECT])[fromDate + "," + toDate];

                highDefect = ((Dictionary<String, Dictionary<String, float>>)Session[SessionFactory.ALL_DASHBOARD_OUTG_DEFECT_DEFECTNOBYACCNT_HIGH_DEFECT])[fromDate + "," + toDate];
                mediumDefect = ((Dictionary<String, Dictionary<String, float>>)Session[SessionFactory.ALL_DASHBOARD_OUTG_DEFECT_DEFECTNOBYACCNT_MEDM_DEFECT])[fromDate + "," + toDate];
                lowDefect = ((Dictionary<String, Dictionary<String, float>>)Session[SessionFactory.ALL_DASHBOARD_OUTG_DEFECT_DEFECTNOBYACCNT_LOW_DEFECT])[fromDate + "," + toDate];

                if (vendFilterCheck)
                {
                    Dictionary<String, float> totalDefectNoTemp = new Dictionary<String, float>();
                    Dictionary<String, float> highDefectTemp = new Dictionary<String, float>();
                    Dictionary<String, float> lowDefectTemp = new Dictionary<String, float>();
                    Dictionary<String, float> mediumDefectTemp = new Dictionary<String, float>();

                    foreach (KeyValuePair<String, String> kvp in vendList)
                    {
                        if (totalDefectAmount.ContainsKey(kvp.Key))
                            totalDefectNoTemp.Add(kvp.Key, totalDefectAmount[kvp.Key]);
                        else
                            regenerateValues = true;

                        if (highDefect.ContainsKey(kvp.Key))
                            highDefectTemp.Add(kvp.Key, highDefect[kvp.Key]);
                        else
                            regenerateValues = true;

                        if (lowDefect.ContainsKey(kvp.Key))
                            lowDefectTemp.Add(kvp.Key, lowDefect[kvp.Key]);
                        else
                            regenerateValues = true;

                        if (mediumDefect.ContainsKey(kvp.Key))
                            mediumDefectTemp.Add(kvp.Key, mediumDefect[kvp.Key]);
                        else
                            regenerateValues = true;
                    }

                    highDefect = highDefectTemp;
                    lowDefect = lowDefectTemp;
                    mediumDefect = mediumDefectTemp;
                    totalDefectAmount = totalDefectNoTemp;
                }

            }
            if (lastGeneratedContactDict != null && lastGeneratedContactDict.Count > 0 && lastGeneratedContactDict.ContainsKey(fromDate + "," + toDate) && !regenerateValues)
            {
                contactDict = lastGeneratedContactDict[fromDate + "," + toDate];
            }

            else
            {
                highDefect = new Dictionary<string, float>();
                lowDefect = new Dictionary<string, float>();
                mediumDefect = new Dictionary<string, float>();
                totalDefectAmount = new Dictionary<string, float>();

                int rowCount = 0;
                DataTable dt = new DataTable();
                dt.Columns.Add("Vendor Name");
                dt.Columns.Add("Severity");
                dt.Columns.Add("Defect#");
                dt.Columns.Add("Description");
                dt.Columns.Add("Defect Date");
                dt.Columns.Add("Close Date");
                dt.Columns.Add("Defect Amount");
                dt.Columns.Add("Defect Status");
                dt.Columns.Add("Defect Resolution Status");

                if (defectDict == null || defectDict.Count == 0)
                {
                    Dictionary<String, String> filterParams = new Dictionary<String, String>();
                    ActionLibrary.DefectActions._dispDefectDetails dspDefect = new DefectActions._dispDefectDetails();
                    if (fromDate != null && !fromDate.Equals(""))
                        filterParams.Add(dspDefect.FILTER_BY_SUBMIT_DATE_FROM, fromDate);
                    if (toDate != null && !toDate.Equals(""))
                        filterParams.Add(dspDefect.FILTER_BY_SUBMIT_DATE_TO, toDate);

                    defectDict = dspDefect.getAllDefectsFiltered("outgoing", Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), filterParams);
                }

                foreach (KeyValuePair<String, DefectDetails> kvp in defectDict)
                {
                    DefectDetails defectObj = kvp.Value;

                    String vendEntId = defectObj.getSupplierId();
                    String vendName = "";
                    if (defectObj.getCreationMode().Equals(BackEndObjects.DefectDetails.DEFECT_CREATION_MODE_MANUAL))
                    {
                        vendName = Contacts.getContactDetailsforContactEntityDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), vendEntId).getContactName();
                        if (vendName == null || vendName.Equals(""))
                            vendName = MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(vendEntId).getEntityName();
                    }
                    else
                        vendName = MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(vendEntId).getEntityName();

                    bool considerVendEnt = vendFilterCheck ? vendList.ContainsKey(vendEntId) : true;

                    if (considerVendEnt)
                    {
                        if (!contactDict.ContainsKey(vendEntId))
                        {
                            Contacts contactObj = Contacts.
                                getContactDetailsforContactEntityDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), vendEntId);
                            if (contactObj == null || contactObj.getContactName() == null || contactObj.getContactName().Equals(""))
                                contactDict.Add(vendEntId, MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(vendEntId).getEntityName());
                            else
                                contactDict.Add(vendEntId, contactObj.getContactName());

                            if (!highDefect.ContainsKey(vendEntId))
                                highDefect.Add(vendEntId, 0);

                            if (!lowDefect.ContainsKey(vendEntId))
                                lowDefect.Add(vendEntId, 0);

                            if (!mediumDefect.ContainsKey(vendEntId))
                                mediumDefect.Add(vendEntId, 0);
                        }

                        if (!totalDefectAmount.ContainsKey(vendEntId))
                            totalDefectAmount.Add(vendEntId, 1);
                        else
                            totalDefectAmount[vendEntId] += 1;

                        if (defectObj.getSeverity().Equals("High", StringComparison.InvariantCultureIgnoreCase))
                            highDefect[vendEntId] += 1;
                        if (defectObj.getSeverity().Equals("Low", StringComparison.InvariantCultureIgnoreCase))
                            lowDefect[vendEntId] += 1;
                        if (defectObj.getSeverity().Equals("Medium", StringComparison.InvariantCultureIgnoreCase))
                            mediumDefect[vendEntId] += 1;

                        dt.Rows.Add();

                        dt.Rows[rowCount]["Vendor Name"] = contactDict[vendEntId];
                        dt.Rows[rowCount]["Severity"] = defectObj.getSeverity();
                        dt.Rows[rowCount]["Defect#"] = defectObj.getDefectId();
                        dt.Rows[rowCount]["Description"] = defectObj.getDescription();
                        dt.Rows[rowCount]["Defect Date"] = defectObj.getDateCreated();
                        dt.Rows[rowCount]["Close Date"] = defectObj.getCloseDate();
                        dt.Rows[rowCount]["Defect Amount"] = defectObj.getTotalAmount();
                        dt.Rows[rowCount]["Defect Status"] = defectObj.getDefectStat();
                        dt.Rows[rowCount]["Defect Resolution Status"] = defectObj.getResolStat();

                        rowCount++;

                    }
                }

                Dictionary<String, DataTable> reportDict = (Dictionary<String, DataTable>)Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES];

                if (reportDict == null)
                    reportDict = new Dictionary<string, DataTable>();

                if (!reportDict.ContainsKey("outgoing_Defect_No_By_Accnt"))
                    reportDict.Add("outgoing_Defect_No_By_Accnt", dt);
                else
                    reportDict["outgoing_Defect_No_By_Accnt"] = dt;

                Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES] = reportDict;

                lastGeneratedContactDict = new Dictionary<string, Dictionary<string, string>>(); lastGeneratedContactDict.Add(fromDate + "," + toDate, contactDict);
                Dictionary<String, Dictionary<String, float>> lastGeneratedTotalDefectDict = new Dictionary<string, Dictionary<string, float>>(); lastGeneratedTotalDefectDict.Add(fromDate + "," + toDate, totalDefectAmount);
                Dictionary<String, Dictionary<String, float>> lastGeneratedHighDefectDict = new Dictionary<string, Dictionary<string, float>>(); lastGeneratedHighDefectDict.Add(fromDate + "," + toDate, highDefect);
                Dictionary<String, Dictionary<String, float>> lastGeneratedLowDefectDict = new Dictionary<string, Dictionary<string, float>>(); lastGeneratedLowDefectDict.Add(fromDate + "," + toDate, lowDefect);
                Dictionary<String, Dictionary<String, float>> lastGeneratedMedmDefectDict = new Dictionary<string, Dictionary<string, float>>(); lastGeneratedMedmDefectDict.Add(fromDate + "," + toDate, mediumDefect);

                Session[SessionFactory.ALL_DASHBOARD_OUTG_DEFECT_DEFECTNOBYACCNT_CONTACT_DICT] = lastGeneratedContactDict;
                Session[SessionFactory.ALL_DASHBOARD_OUTG_DEFECT_DEFECTNOBYACCNT_HIGH_DEFECT] = lastGeneratedHighDefectDict;
                Session[SessionFactory.ALL_DASHBOARD_OUTG_DEFECT_DEFECTNOBYACCNT_MEDM_DEFECT] = lastGeneratedMedmDefectDict;
                Session[SessionFactory.ALL_DASHBOARD_OUTG_DEFECT_DEFECTNOBYACCNT_LOW_DEFECT] = lastGeneratedLowDefectDict;
                Session[SessionFactory.ALL_DASHBOARD_OUTG_DEFECT_DEFECTNOBYACCNT_TOTAL_DEFECT] = lastGeneratedTotalDefectDict;

            }
            //Now populate the List box with customer details
            foreach (KeyValuePair<String, String> kvp in contactDict)
            {
                ListItem ltExists = Listbox_Outgoing_Defect_No_By_Account.Items.FindByValue(kvp.Key);
                if (ltExists == null || ltExists.Text.Equals(""))
                {
                    ltExists = new ListItem();
                    ltExists.Text = kvp.Value;
                    ltExists.Value = kvp.Key;

                    Listbox_Outgoing_Defect_No_By_Account.Items.Add(ltExists);
                }
            }

            String[] vendorArray = new String[contactDict.Count];
            float[] totalDefectAmountArray = new float[contactDict.Count];
            float[] totalHighDefectNoArray = new float[contactDict.Count];
            float[] totalLowDefectNoArray = new float[contactDict.Count];
            float[] totalMediumDefectNoArray = new float[contactDict.Count];

            int counter = 0;
            foreach (KeyValuePair<String, float> kvp in totalDefectAmount)
            {
                vendorArray[counter] = contactDict[kvp.Key];
                totalDefectAmountArray[counter] = totalDefectAmount[kvp.Key];
                totalHighDefectNoArray[counter] = highDefect[kvp.Key];
                totalLowDefectNoArray[counter] = lowDefect[kvp.Key];
                totalMediumDefectNoArray[counter] = mediumDefect[kvp.Key];

                counter++;
            }

            Chart_Outgoing_Defect_No_By_Account.Titles.Clear();
            Chart_Outgoing_Defect_No_By_Account.Titles.Add("Total Number of Defects for Vendors during this period (defect submit date)");
            Chart_Outgoing_Defect_No_By_Account.Titles[0].Font = new System.Drawing.Font("Book Antiqua", 10);
            Chart_Outgoing_Defect_No_By_Account.Series["HighDefects"].Label = "#VALY{0;0;#}";
            Chart_Outgoing_Defect_No_By_Account.Series["LowDefects"].Label = "#VALY{0;0;#}";
            Chart_Outgoing_Defect_No_By_Account.Series["MediumDefects"].Label = "#VALY{0;0;#}";

            Chart_Outgoing_Defect_No_By_Account.Series["HighDefects"].Points.DataBindXY(vendorArray, totalHighDefectNoArray);
            Chart_Outgoing_Defect_No_By_Account.Series["LowDefects"].Points.DataBindXY(vendorArray, totalLowDefectNoArray);
            Chart_Outgoing_Defect_No_By_Account.Series["MediumDefects"].Points.DataBindXY(vendorArray, totalMediumDefectNoArray);
        }

        /*protected void generateIncomingDefectectsCharts(String fromDate, String toDate)
        {
            if (fromDate == null || fromDate.Equals(""))
                fromDate = DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd");
            if (toDate == null || toDate.Equals(""))
                toDate = DateTime.Now.ToString("yyyy-MM-dd");

            generateDefectValByAccount(fromDate, toDate, null, null);
            generateDefectNoByAccount(fromDate, toDate, null, null);
        }*/

        protected void generateOutgoingSRValByAccount(String fromDate, String toDate, Dictionary<String, DefectDetails> SRDict, Dictionary<String, String> vendList)
        {
            if (fromDate == null || fromDate.Equals(""))
                fromDate = DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd");
            if (toDate == null || toDate.Equals(""))
                toDate = DateTime.Now.ToString("yyyy-MM-dd");

            bool vendFilterCheck = (vendList != null && vendList.Count > 0) ? true : false;
            bool regenerateValues = false;

            Dictionary<String, String> contactDict = new Dictionary<String, String>();
            Dictionary<String, float> totalSRAmount = new Dictionary<string, float>();

            Dictionary<String, Dictionary<String, String>> lastGeneratedContactDict = (Dictionary<String, Dictionary<String, String>>)
    Session[SessionFactory.ALL_DASHBOARD_OUTG_SR_SRVALBYACCNT_CONTACT_DICT];

            if (lastGeneratedContactDict != null && lastGeneratedContactDict.Count > 0 && lastGeneratedContactDict.ContainsKey(fromDate + "," + toDate))
            {
                totalSRAmount = ((Dictionary<String, Dictionary<String, float>>)Session[SessionFactory.ALL_DASHBOARD_OUTG_SR_SRVALBYACCNT_SR_AMNT])[fromDate + "," + toDate];

                if (vendFilterCheck)
                {
                    Dictionary<String, float> totalSRAmountTemp = new Dictionary<String, float>();

                    foreach (KeyValuePair<String, String> kvp in vendList)
                    {
                        if (totalSRAmount.ContainsKey(kvp.Key))
                            totalSRAmountTemp.Add(kvp.Key, totalSRAmount[kvp.Key]);
                        else
                            regenerateValues = true;
                    }
                    totalSRAmount = totalSRAmountTemp;
                }
            }
            if (lastGeneratedContactDict != null && lastGeneratedContactDict.Count > 0 && lastGeneratedContactDict.ContainsKey(fromDate + "," + toDate) & !regenerateValues)
            {
                contactDict = lastGeneratedContactDict[fromDate + "," + toDate];
            }
            else
            {
                totalSRAmount = new Dictionary<string, float>();

                int rowCount = 0;
                DataTable dt = new DataTable();
                dt.Columns.Add("Vendor Name");
                dt.Columns.Add("SR#");
                dt.Columns.Add("Description");
                dt.Columns.Add("SR Date");
                dt.Columns.Add("SR Amount");
                dt.Columns.Add("SR Status");
                dt.Columns.Add("SR Resolution Status");

                if (SRDict == null || SRDict.Count == 0)
                {
                    Dictionary<String, String> filterParams = new Dictionary<String, String>();
                    ActionLibrary.DefectActions._dispDefectDetails dspSR = new DefectActions._dispDefectDetails();
                    if (fromDate != null && !fromDate.Equals(""))
                        filterParams.Add(dspSR.FILTER_BY_SUBMIT_DATE_FROM, fromDate);
                    if (toDate != null && !toDate.Equals(""))
                        filterParams.Add(dspSR.FILTER_BY_SUBMIT_DATE_TO, toDate);

                    SRDict = dspSR.getAllSRsFiltered("outgoing", Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), filterParams);
                }

                foreach (KeyValuePair<String, DefectDetails> kvp in SRDict)
                {
                    DefectDetails SRObj = kvp.Value;

                    String vendEntId = SRObj.getSupplierId();
                    String vendName = "";
                    if (SRObj.getCreationMode().Equals(BackEndObjects.DefectDetails.DEFECT_CREATION_MODE_MANUAL))
                    {
                        vendName = Contacts.getContactDetailsforContactEntityDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), vendEntId).getContactName();
                        if (vendName == null || vendName.Equals(""))
                            vendName = MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(vendEntId).getEntityName();
                    }
                    else
                        vendName = MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(vendEntId).getEntityName();

                    bool considerVendEnt = vendFilterCheck ? vendList.ContainsKey(vendEntId) : true;

                    if (considerVendEnt)
                    {
                        if (!contactDict.ContainsKey(vendEntId))
                        {
                            Contacts contactObj = Contacts.
                                getContactDetailsforContactEntityDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), vendEntId);
                            if (contactObj == null || contactObj.getContactName() == null || contactObj.getContactName().Equals(""))
                                contactDict.Add(vendEntId, MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(vendEntId).getEntityName());
                            else
                                contactDict.Add(vendEntId, contactObj.getContactName());
                        }

                        if (!totalSRAmount.ContainsKey(vendEntId))
                            totalSRAmount.Add(vendEntId, SRObj.getTotalAmount());
                        else
                            totalSRAmount[vendEntId] += SRObj.getTotalAmount();

                        dt.Rows.Add();

                        dt.Rows[rowCount]["Vendor Name"] = contactDict[vendEntId];
                        dt.Rows[rowCount]["SR#"] = SRObj.getDefectId();
                        dt.Rows[rowCount]["Description"] = SRObj.getDescription();
                        dt.Rows[rowCount]["SR Date"] = SRObj.getDateCreated();
                        dt.Rows[rowCount]["SR Amount"] = SRObj.getTotalAmount();
                        dt.Rows[rowCount]["SR Status"] = SRObj.getDefectStat();
                        dt.Rows[rowCount]["SR Resolution Status"] = SRObj.getResolStat();

                        rowCount++;
                    }
                }

                Dictionary<String, DataTable> reportDict = (Dictionary<String, DataTable>)Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES];

                if (reportDict == null)
                    reportDict = new Dictionary<string, DataTable>();

                if (!reportDict.ContainsKey("outgoing_SR_By_Accnt"))
                    reportDict.Add("outgoing_SR_By_Accnt", dt);
                else
                    reportDict["outgoing_SR_By_Accnt"] = dt;

                Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES] = reportDict;

                lastGeneratedContactDict = new Dictionary<string, Dictionary<string, string>>(); lastGeneratedContactDict.Add(fromDate + "," + toDate, contactDict);
                Dictionary<String, Dictionary<String, float>> lastGeneratedAmntDict = new Dictionary<string, Dictionary<string, float>>(); lastGeneratedAmntDict.Add(fromDate + "," + toDate, totalSRAmount);

                Session[SessionFactory.ALL_DASHBOARD_OUTG_SR_SRVALBYACCNT_CONTACT_DICT] = lastGeneratedContactDict;
                Session[SessionFactory.ALL_DASHBOARD_OUTG_SR_SRVALBYACCNT_SR_AMNT] = lastGeneratedAmntDict;


            }
            //Now populate the List box with customer details
            foreach (KeyValuePair<String, String> kvp in contactDict)
            {
                ListItem ltExists = ListBox_Outgoing_SR_By_Account.Items.FindByValue(kvp.Key);
                if (ltExists == null || ltExists.Text.Equals(""))
                {
                    ltExists = new ListItem();
                    ltExists.Text = kvp.Value;
                    ltExists.Value = kvp.Key;

                    ListBox_Outgoing_SR_By_Account.Items.Add(ltExists);
                }
            }

            String[] vendorArray = new String[contactDict.Count];
            float[] totalSRAmountArray = new float[contactDict.Count];
            int counter = 0;
            foreach (KeyValuePair<String, float> kvp in totalSRAmount)
            {
                vendorArray[counter] = contactDict[kvp.Key];
                totalSRAmountArray[counter] = totalSRAmount[kvp.Key];
                counter++;
            }

            Chart_Outgoing_SR_By_Account.Titles.Clear();
            Chart_Outgoing_SR_By_Account.Titles.Add("Total Invoice Value of SRs for Vendors during this period (SR submit date)");
            Chart_Outgoing_SR_By_Account.Titles[0].Font = new System.Drawing.Font("Book Antiqua", 10);
            Chart_Outgoing_SR_By_Account.Series["TotalSRs"].Label = "#VALY{0;0;#}";
            Chart_Outgoing_SR_By_Account.Series["TotalSRs"].Points.DataBindXY(vendorArray, totalSRAmountArray);

        }

        protected void generateOutgoingSRNoByAccount(String fromDate, String toDate, Dictionary<String, DefectDetails> SRDict, Dictionary<String, String> vendList)
        {
            if (fromDate == null || fromDate.Equals(""))
                fromDate = DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd");
            if (toDate == null || toDate.Equals(""))
                toDate = DateTime.Now.ToString("yyyy-MM-dd");

            bool vendFilterCheck = (vendList != null && vendList.Count > 0) ? true : false;
            bool regenerateValues = false;

            Dictionary<String, String> contactDict = new Dictionary<String, String>();
            Dictionary<String, float> totalSRAmount = new Dictionary<string, float>();
            Dictionary<String, float> highSR = new Dictionary<string, float>();
            Dictionary<String, float> mediumSR = new Dictionary<string, float>();
            Dictionary<String, float> lowSR = new Dictionary<string, float>();

            Dictionary<String, Dictionary<String, String>> lastGeneratedContactDict = ((Dictionary<String, Dictionary<String, String>>)
    Session[SessionFactory.ALL_DASHBOARD_OUTG_SR_SRNOBYACCNT_CONTACT_DICT]);

            if (lastGeneratedContactDict != null && lastGeneratedContactDict.Count > 0 && lastGeneratedContactDict.ContainsKey(fromDate + "," + toDate))
            {
                totalSRAmount = ((Dictionary<String, Dictionary<String, float>>)Session[SessionFactory.ALL_DASHBOARD_OUTG_SR_SRNOBYACCNT_TOTAL_SR])[fromDate + "," + toDate];

                highSR = ((Dictionary<String, Dictionary<String, float>>)Session[SessionFactory.ALL_DASHBOARD_OUTG_SR_SRNOBYACCNT_HIGH_SR])[fromDate + "," + toDate];
                mediumSR = ((Dictionary<String, Dictionary<String, float>>)Session[SessionFactory.ALL_DASHBOARD_OUTG_SR_SRNOBYACCNT_MEDM_SR])[fromDate + "," + toDate];
                lowSR = ((Dictionary<String, Dictionary<String, float>>)Session[SessionFactory.ALL_DASHBOARD_OUTG_SR_SRNOBYACCNT_LOW_SR])[fromDate + "," + toDate];

                if (vendFilterCheck)
                {
                    Dictionary<String, float> totalSRNoTemp = new Dictionary<String, float>();
                    Dictionary<String, float> highSRTemp = new Dictionary<String, float>();
                    Dictionary<String, float> lowSRTemp = new Dictionary<String, float>();
                    Dictionary<String, float> mediumSRTemp = new Dictionary<String, float>();

                    foreach (KeyValuePair<String, String> kvp in vendList)
                    {
                        if (totalSRAmount.ContainsKey(kvp.Key))
                            totalSRNoTemp.Add(kvp.Key, totalSRAmount[kvp.Key]);
                        else
                            regenerateValues = true;

                        if (highSR.ContainsKey(kvp.Key))
                            highSRTemp.Add(kvp.Key, highSR[kvp.Key]);
                        else
                            regenerateValues = true;

                        if (lowSR.ContainsKey(kvp.Key))
                            lowSRTemp.Add(kvp.Key, lowSR[kvp.Key]);
                        else
                            regenerateValues = true;

                        if (mediumSR.ContainsKey(kvp.Key))
                            mediumSRTemp.Add(kvp.Key, mediumSR[kvp.Key]);
                        else
                            regenerateValues = true;
                    }

                    highSR = highSRTemp;
                    lowSR = lowSRTemp;
                    mediumSR = mediumSRTemp;
                    totalSRAmount = totalSRNoTemp;
                }

            }
            if (lastGeneratedContactDict != null && lastGeneratedContactDict.Count > 0 && lastGeneratedContactDict.ContainsKey(fromDate + "," + toDate) && !regenerateValues)
            {
                contactDict = lastGeneratedContactDict[fromDate + "," + toDate];
            }

            else
            {
                highSR = new Dictionary<string, float>();
                lowSR = new Dictionary<string, float>();
                mediumSR = new Dictionary<string, float>();
                totalSRAmount = new Dictionary<string, float>();

                int rowCount = 0;
                DataTable dt = new DataTable();
                dt.Columns.Add("Vendor Name");
                dt.Columns.Add("Severity");
                dt.Columns.Add("SR#");
                dt.Columns.Add("Description");
                dt.Columns.Add("SR Date");
                dt.Columns.Add("Close Date");
                dt.Columns.Add("SR Amount");
                dt.Columns.Add("SR Status");
                dt.Columns.Add("SR Resolution Status");

                if (SRDict == null || SRDict.Count == 0)
                {
                    Dictionary<String, String> filterParams = new Dictionary<String, String>();
                    ActionLibrary.DefectActions._dispDefectDetails dspSR = new DefectActions._dispDefectDetails();
                    if (fromDate != null && !fromDate.Equals(""))
                        filterParams.Add(dspSR.FILTER_BY_SUBMIT_DATE_FROM, fromDate);
                    if (toDate != null && !toDate.Equals(""))
                        filterParams.Add(dspSR.FILTER_BY_SUBMIT_DATE_TO, toDate);

                    SRDict = dspSR.getAllSRsFiltered("outgoing", Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), filterParams);
                }

                foreach (KeyValuePair<String, DefectDetails> kvp in SRDict)
                {
                    DefectDetails SRObj = kvp.Value;

                    String vendEntId = SRObj.getSupplierId();
                    String vendName = "";
                    if (SRObj.getCreationMode().Equals(BackEndObjects.DefectDetails.DEFECT_CREATION_MODE_MANUAL))
                    {
                        vendName = Contacts.getContactDetailsforContactEntityDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), vendEntId).getContactName();
                        if (vendName == null || vendName.Equals(""))
                            vendName = MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(vendEntId).getEntityName();
                    }
                    else
                        vendName = MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(vendEntId).getEntityName();

                    bool considerVendEnt = vendFilterCheck ? vendList.ContainsKey(vendEntId) : true;

                    if (considerVendEnt)
                    {
                        if (!contactDict.ContainsKey(vendEntId))
                        {
                            Contacts contactObj = Contacts.
                                getContactDetailsforContactEntityDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), vendEntId);
                            if (contactObj == null || contactObj.getContactName() == null || contactObj.getContactName().Equals(""))
                                contactDict.Add(vendEntId, MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(vendEntId).getEntityName());
                            else
                                contactDict.Add(vendEntId, contactObj.getContactName());

                            if (!highSR.ContainsKey(vendEntId))
                                highSR.Add(vendEntId, 0);

                            if (!lowSR.ContainsKey(vendEntId))
                                lowSR.Add(vendEntId, 0);

                            if (!mediumSR.ContainsKey(vendEntId))
                                mediumSR.Add(vendEntId, 0);
                        }

                        if (!totalSRAmount.ContainsKey(vendEntId))
                            totalSRAmount.Add(vendEntId, 1);
                        else
                            totalSRAmount[vendEntId] += 1;

                        if (SRObj.getSeverity().Equals("High", StringComparison.InvariantCultureIgnoreCase))
                            highSR[vendEntId] += 1;
                        if (SRObj.getSeverity().Equals("Low", StringComparison.InvariantCultureIgnoreCase))
                            lowSR[vendEntId] += 1;
                        if (SRObj.getSeverity().Equals("Medium", StringComparison.InvariantCultureIgnoreCase))
                            mediumSR[vendEntId] += 1;

                        dt.Rows.Add();

                        dt.Rows[rowCount]["Vendor Name"] = contactDict[vendEntId];
                        dt.Rows[rowCount]["Severity"] = SRObj.getSeverity();
                        dt.Rows[rowCount]["SR#"] = SRObj.getDefectId();
                        dt.Rows[rowCount]["Description"] = SRObj.getDescription();
                        dt.Rows[rowCount]["SR Date"] = SRObj.getDateCreated();
                        dt.Rows[rowCount]["Close Date"] = SRObj.getCloseDate();
                        dt.Rows[rowCount]["SR Amount"] = SRObj.getTotalAmount();
                        dt.Rows[rowCount]["SR Status"] = SRObj.getDefectStat();
                        dt.Rows[rowCount]["SR Resolution Status"] = SRObj.getResolStat();

                        rowCount++;

                    }
                }

                Dictionary<String, DataTable> reportDict = (Dictionary<String, DataTable>)Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES];

                if (reportDict == null)
                    reportDict = new Dictionary<string, DataTable>();

                if (!reportDict.ContainsKey("outgoing_SR_No_By_Accnt"))
                    reportDict.Add("outgoing_SR_No_By_Accnt", dt);
                else
                    reportDict["outgoing_SR_No_By_Accnt"] = dt;

                Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES] = reportDict;

                lastGeneratedContactDict = new Dictionary<string, Dictionary<string, string>>(); lastGeneratedContactDict.Add(fromDate + "," + toDate, contactDict);
                Dictionary<String, Dictionary<String, float>> lastGeneratedTotalSRDict = new Dictionary<string, Dictionary<string, float>>(); lastGeneratedTotalSRDict.Add(fromDate + "," + toDate, totalSRAmount);
                Dictionary<String, Dictionary<String, float>> lastGeneratedHighSRDict = new Dictionary<string, Dictionary<string, float>>(); lastGeneratedHighSRDict.Add(fromDate + "," + toDate, highSR);
                Dictionary<String, Dictionary<String, float>> lastGeneratedLowSRDict = new Dictionary<string, Dictionary<string, float>>(); lastGeneratedLowSRDict.Add(fromDate + "," + toDate, lowSR);
                Dictionary<String, Dictionary<String, float>> lastGeneratedMedmSRDict = new Dictionary<string, Dictionary<string, float>>(); lastGeneratedMedmSRDict.Add(fromDate + "," + toDate, mediumSR);

                Session[SessionFactory.ALL_DASHBOARD_OUTG_SR_SRNOBYACCNT_CONTACT_DICT] = lastGeneratedContactDict;
                Session[SessionFactory.ALL_DASHBOARD_OUTG_SR_SRNOBYACCNT_HIGH_SR] = lastGeneratedHighSRDict;
                Session[SessionFactory.ALL_DASHBOARD_OUTG_SR_SRNOBYACCNT_MEDM_SR] = lastGeneratedMedmSRDict;
                Session[SessionFactory.ALL_DASHBOARD_OUTG_SR_SRNOBYACCNT_LOW_SR] = lastGeneratedLowSRDict;
                Session[SessionFactory.ALL_DASHBOARD_OUTG_SR_SRNOBYACCNT_TOTAL_SR] = lastGeneratedTotalSRDict;

            }
            //Now populate the List box with customer details
            foreach (KeyValuePair<String, String> kvp in contactDict)
            {
                ListItem ltExists = Listbox_Outgoing_SR_No_By_Account.Items.FindByValue(kvp.Key);
                if (ltExists == null || ltExists.Text.Equals(""))
                {
                    ltExists = new ListItem();
                    ltExists.Text = kvp.Value;
                    ltExists.Value = kvp.Key;

                    Listbox_Outgoing_SR_No_By_Account.Items.Add(ltExists);
                }
            }

            String[] vendorArray = new String[contactDict.Count];
            float[] totalSRAmountArray = new float[contactDict.Count];
            float[] totalHighSRNoArray = new float[contactDict.Count];
            float[] totalLowSRNoArray = new float[contactDict.Count];
            float[] totalMediumSRNoArray = new float[contactDict.Count];

            int counter = 0;
            foreach (KeyValuePair<String, float> kvp in totalSRAmount)
            {
                vendorArray[counter] = contactDict[kvp.Key];
                totalSRAmountArray[counter] = totalSRAmount[kvp.Key];
                totalHighSRNoArray[counter] = highSR[kvp.Key];
                totalLowSRNoArray[counter] = lowSR[kvp.Key];
                totalMediumSRNoArray[counter] = mediumSR[kvp.Key];

                counter++;
            }

            Chart_Outgoing_SR_No_By_Account.Titles.Clear();
            Chart_Outgoing_SR_No_By_Account.Titles.Add("Total Number of SRs for Vendors during this period (SR submit date)");
            Chart_Outgoing_SR_No_By_Account.Titles[0].Font = new System.Drawing.Font("Book Antiqua", 10);
            Chart_Outgoing_SR_No_By_Account.Series["HighSRs"].Label = "#VALY{0;0;#}";
            Chart_Outgoing_SR_No_By_Account.Series["LowSRs"].Label = "#VALY{0;0;#}";
            Chart_Outgoing_SR_No_By_Account.Series["MediumSRs"].Label = "#VALY{0;0;#}";

            Chart_Outgoing_SR_No_By_Account.Series["HighSRs"].Points.DataBindXY(vendorArray, totalHighSRNoArray);
            Chart_Outgoing_SR_No_By_Account.Series["LowSRs"].Points.DataBindXY(vendorArray, totalLowSRNoArray);
            Chart_Outgoing_SR_No_By_Account.Series["MediumSRs"].Points.DataBindXY(vendorArray, totalMediumSRNoArray);
        }

        protected void generateSRArrivalandClosureOutgoing(String fromDate, String toDate, ArrayList SRList, Dictionary<String, String> userList, String frequency)
        {
            if (fromDate == null || fromDate.Equals(""))
                fromDate = DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd");
            if (toDate == null || toDate.Equals(""))
                toDate = DateTime.Now.ToString("yyyy-MM-dd");

            bool userFilterCheck = (userList != null && userList.Count > 0) ? true : false;
            bool generateChart = false;

            Dictionary<DateTime, float> totalSRArrivalHigh = new Dictionary<DateTime, float>();
            Dictionary<DateTime, float> totalSRArrivalLow = new Dictionary<DateTime, float>();
            Dictionary<DateTime, float> totalSRArrivalMed = new Dictionary<DateTime, float>();
            Dictionary<DateTime, float> SRClosureHigh = new Dictionary<DateTime, float>();
            Dictionary<DateTime, float> SRClosureLow = new Dictionary<DateTime, float>();
            Dictionary<DateTime, float> SRClosureMed = new Dictionary<DateTime, float>();
            Dictionary<DateTime, String> dateRangeDict = new Dictionary<DateTime, string>();

            Dictionary<String, Dictionary<DateTime, String>> lastGeneratedDateRangeDict = (Dictionary<String, Dictionary<DateTime, String>>)Session[SessionFactory.ALL_DASHBOARD_OUTG_SR_ARRVLCLOS_DATE_RANGE];
            String lastRequestedFreq = (Session[SessionFactory.ALL_DASHBOARD_OUTG_SR_ARRVLCLOS_LAST_FREQ] != null ? Session[SessionFactory.ALL_DASHBOARD_OUTG_SR_ARRVLCLOS_LAST_FREQ].ToString() : "");

            if (lastGeneratedDateRangeDict != null && lastGeneratedDateRangeDict.Count > 0 && lastGeneratedDateRangeDict.ContainsKey(fromDate + "-" + toDate) && frequency.Equals(lastRequestedFreq))
            {
                Dictionary<String, Dictionary<DateTime, float>> allGeneratedDicts = (Dictionary<String, Dictionary<DateTime, float>>)
                    Session[SessionFactory.ALL_DASHBOARD_OUTG_SR_ARRVLCLOS_GENERATED_DICTS];

                totalSRArrivalHigh = allGeneratedDicts["ArrivalHigh"];
                totalSRArrivalLow = allGeneratedDicts["ArrivalLow"];
                totalSRArrivalMed = allGeneratedDicts["ArrivalMed"];
                SRClosureHigh = allGeneratedDicts["ClosureHigh"];
                SRClosureLow = allGeneratedDicts["ClosureLow"];
                SRClosureMed = allGeneratedDicts["ClosureMed"];

                dateRangeDict = lastGeneratedDateRangeDict[fromDate + "-" + toDate];
            }
            else
            {
                generateChart = true;
                if (SRList == null || SRList.Count == 0)
                {
                    Dictionary<String, String> filterParams = new Dictionary<String, String>();
                    ActionLibrary.DefectActions._dispDefectDetails dspSR = new DefectActions._dispDefectDetails();
                    if (fromDate != null && !fromDate.Equals(""))
                        filterParams.Add(dspSR.FILTER_BY_SUBMIT_DATE_FROM, fromDate);
                    if (toDate != null && !toDate.Equals(""))
                        filterParams.Add(dspSR.FILTER_BY_SUBMIT_DATE_TO, toDate);

                    SRList = dspSR.getAllSRsFilteredORDERBYCreateDate("outgoing", Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), filterParams);
                    if (SRList != null && SRList.Count > 0)
                    {
                        String lowerDate = ((DefectDetails)SRList[0]).getDateCreated().Substring(0, ((DefectDetails)SRList[0]).getDateCreated().IndexOf(" "));
                        String higherDate = ((DefectDetails)SRList[SRList.Count - 1]).getDateCreated().Substring(0, ((DefectDetails)SRList[SRList.Count - 1]).getDateCreated().IndexOf(" "));

                        dateRangeDict = getDateRange(lowerDate, higherDate, frequency);
                        List<DateTime> dataRangeList = dateRangeDict.Keys.ToList();
                        dataRangeList.Sort();
                        //Make sure all the dictionaries have the same key set
                        foreach (KeyValuePair<DateTime, String> kvp in dateRangeDict)
                        {
                            if (!totalSRArrivalHigh.ContainsKey(kvp.Key))
                                totalSRArrivalHigh.Add(kvp.Key, 0);

                            if (!totalSRArrivalLow.ContainsKey(kvp.Key))
                                totalSRArrivalLow.Add(kvp.Key, 0);

                            if (!totalSRArrivalMed.ContainsKey(kvp.Key))
                                totalSRArrivalMed.Add(kvp.Key, 0);

                            if (!SRClosureHigh.ContainsKey(kvp.Key))
                                SRClosureHigh.Add(kvp.Key, 0);

                            if (!SRClosureLow.ContainsKey(kvp.Key))
                                SRClosureLow.Add(kvp.Key, 0);

                            if (!SRClosureMed.ContainsKey(kvp.Key))
                                SRClosureMed.Add(kvp.Key, 0);
                        }

                        for (int i = 0; i < SRList.Count; i++)
                        {
                            DefectDetails defObj = (DefectDetails)SRList[i];
                            /*String closeDate = defObj.getCloseDate() != null && !defObj.getCloseDate().Equals("") && !(defObj.getCloseDate().IndexOf("1/1/1900")>=0)?
                                defObj.getCloseDate().Replace(" ", "  ").Substring(0, 9) : "";
                            String createDate = defObj.getDateCreated().Replace(" ", "  ").Substring(0, 9);*/
                            String closeDate = defObj.getCloseDate() != null && !defObj.getCloseDate().Equals("") && !(defObj.getCloseDate().IndexOf("1/1/1900") >= 0) ?
                                (defObj.getCloseDate().IndexOf(" ") >= 0 ? defObj.getCloseDate().Substring(0, defObj.getCloseDate().IndexOf(" ")) : defObj.getCloseDate()) : "";
                            String createDate = defObj.getDateCreated().IndexOf(" ") >= 0 ? defObj.getDateCreated().Substring(0, defObj.getDateCreated().IndexOf(" ")) : defObj.getDateCreated();

                            int index = dataRangeList.BinarySearch(Convert.ToDateTime(createDate));
                            if (index >= 0)
                            {
                                //Exact Match Found
                                if (defObj.getSeverity().Equals("High", StringComparison.InvariantCultureIgnoreCase))
                                    totalSRArrivalHigh[dataRangeList[index]] += 1;
                                if (defObj.getSeverity().Equals("Medium", StringComparison.InvariantCultureIgnoreCase))
                                    totalSRArrivalMed[dataRangeList[index]] += 1;
                                if (defObj.getSeverity().Equals("Low", StringComparison.InvariantCultureIgnoreCase))
                                    totalSRArrivalLow[dataRangeList[index]] += 1;
                            }
                            else
                            {
                                //Nearest Greater Value Found
                                index = ~index;
                                if (defObj.getSeverity().Equals("High", StringComparison.InvariantCultureIgnoreCase))
                                    totalSRArrivalHigh[dataRangeList[index - 1]] += 1;
                                if (defObj.getSeverity().Equals("Medium", StringComparison.InvariantCultureIgnoreCase))
                                    totalSRArrivalMed[dataRangeList[index - 1]] += 1;
                                if (defObj.getSeverity().Equals("Low", StringComparison.InvariantCultureIgnoreCase))
                                    totalSRArrivalLow[dataRangeList[index - 1]] += 1;
                            }

                            if (closeDate != null && !closeDate.Equals(""))
                            {
                                index = dataRangeList.BinarySearch(Convert.ToDateTime(closeDate));
                                if (index >= 0)
                                {
                                    //Exact Match Found
                                    if (defObj.getSeverity().Equals("High", StringComparison.InvariantCultureIgnoreCase))
                                        SRClosureHigh[dataRangeList[index]] += 1;
                                    if (defObj.getSeverity().Equals("Medium", StringComparison.InvariantCultureIgnoreCase))
                                        SRClosureMed[dataRangeList[index]] += 1;
                                    if (defObj.getSeverity().Equals("Low", StringComparison.InvariantCultureIgnoreCase))
                                        SRClosureLow[dataRangeList[index]] += 1;
                                }
                                else
                                {
                                    //Nearest Greater Value Found
                                    index = ~index;
                                    if (defObj.getSeverity().Equals("High", StringComparison.InvariantCultureIgnoreCase))
                                        SRClosureHigh[dataRangeList[index - 1]] += 1;
                                    if (defObj.getSeverity().Equals("Medium", StringComparison.InvariantCultureIgnoreCase))
                                        SRClosureMed[dataRangeList[index - 1]] += 1;
                                    if (defObj.getSeverity().Equals("Low", StringComparison.InvariantCultureIgnoreCase))
                                        SRClosureLow[dataRangeList[index - 1]] += 1;
                                }
                            }
                        }
                    }

                }

                lastGeneratedDateRangeDict = new Dictionary<string, Dictionary<DateTime, string>>(); lastGeneratedDateRangeDict.Add(fromDate + "-" + toDate, dateRangeDict);
                Dictionary<String, Dictionary<DateTime, float>> allGeneratedDicts = new Dictionary<string, Dictionary<DateTime, float>>();
                allGeneratedDicts.Add("ArrivalHigh", totalSRArrivalHigh);
                allGeneratedDicts.Add("ArrivalLow", totalSRArrivalLow);
                allGeneratedDicts.Add("ArrivalMed", totalSRArrivalMed);
                allGeneratedDicts.Add("ClosureHigh", SRClosureHigh);
                allGeneratedDicts.Add("ClosureLow", SRClosureLow);
                allGeneratedDicts.Add("ClosureMed", SRClosureMed);

                Session[SessionFactory.ALL_DASHBOARD_OUTG_SR_ARRVLCLOS_GENERATED_DICTS] = allGeneratedDicts;
                Session[SessionFactory.ALL_DASHBOARD_OUTG_SR_ARRVLCLOS_DATE_RANGE] = lastGeneratedDateRangeDict;
                Session[SessionFactory.ALL_DASHBOARD_OUTG_SR_ARRVLCLOS_LAST_FREQ] = frequency;

            }

            float[] totalSRArrivalHighArray = totalSRArrivalHigh.Values.ToArray();
            float[] totalSRArrivalLowArray = totalSRArrivalLow.Values.ToArray();
            float[] totalSRArrivalMedArray = totalSRArrivalMed.Values.ToArray();
            float[] SRClosureHighArray = SRClosureHigh.Values.ToArray();
            float[] SRClosureLowArray = SRClosureLow.Values.ToArray();
            float[] SRClosureMedArray = SRClosureMed.Values.ToArray();
            String[] dateRangeDictArray = dateRangeDict.Values.ToArray();

            if (generateChart)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Date Range");
                dt.Columns.Add("No of High Sev Created");
                dt.Columns.Add("No of High Sev Closed");
                dt.Columns.Add("No of Medium Sev Created");
                dt.Columns.Add("No of Medium Sev Closed");
                dt.Columns.Add("No of Low Sev Created");
                dt.Columns.Add("No of Low Sev Closed");

                for (int i = 0; i < dateRangeDictArray.Length; i++)
                {
                    dt.Rows.Add();
                    dt.Rows[i]["Date Range"] = dateRangeDictArray[i];
                    dt.Rows[i]["No of High Sev Created"] = totalSRArrivalHighArray[i];
                    dt.Rows[i]["No of High Sev Closed"] = SRClosureHighArray[i];
                    dt.Rows[i]["No of Medium Sev Created"] = totalSRArrivalMedArray[i];
                    dt.Rows[i]["No of Medium Sev Closed"] = SRClosureMedArray[i];
                    dt.Rows[i]["No of Low Sev Created"] = totalSRArrivalLowArray[i];
                    dt.Rows[i]["No of Low Sev Closed"] = SRClosureLowArray[i];

                }

                Dictionary<String, DataTable> reportDict = (Dictionary<String, DataTable>)Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES];
                if (reportDict == null)
                    reportDict = new Dictionary<string, DataTable>();

                if (!reportDict.ContainsKey("outgoing_SR_Arrival_Closure"))
                    reportDict.Add("outgoing_SR_Arrival_Closure", dt);
                else
                    reportDict["outgoing_SR_Arrival_Closure"] = dt;

                Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES] = reportDict;
            }

            //Chart_SR_Arrival_Closure.Titles.Clear();
            //Chart_SR_Arrival_Closure.Titles.Add("SRs Arrival and Closure By Severity for Customers during the period");
            Chart_Out_SR_Arrval_Closure.Titles[0].Font = new System.Drawing.Font("Book Antiqua", 10);
            Chart_Out_SR_Arrval_Closure.Series["Series_SRs_High"].Label = "#VALY{0;0;#}";
            Chart_Out_SR_Arrval_Closure.Series["Series_SRs_Medium"].Label = "#VALY{0;0;#}";
            Chart_Out_SR_Arrval_Closure.Series["Series_SRs_Low"].Label = "#VALY{0;0;#}";
            Chart_Out_SR_Arrval_Closure.Series["Series_Closure_High"].Label = "#VALY{0;0;#}";
            Chart_Out_SR_Arrval_Closure.Series["Series_Closure_Medium"].Label = "#VALY{0;0;#}";
            Chart_Out_SR_Arrval_Closure.Series["Series_Closure_Low"].Label = "#VALY{0;0;#}";

            Chart_Out_SR_Arrval_Closure.Series["Series_SRs_High"].Points.DataBindXY(dateRangeDictArray, totalSRArrivalHighArray);
            Chart_Out_SR_Arrval_Closure.Series["Series_SRs_Medium"].Points.DataBindXY(dateRangeDictArray, totalSRArrivalMedArray);
            Chart_Out_SR_Arrval_Closure.Series["Series_SRs_Low"].Points.DataBindXY(dateRangeDictArray, totalSRArrivalLowArray);
            Chart_Out_SR_Arrval_Closure.Series["Series_Closure_High"].Points.DataBindXY(dateRangeDictArray, SRClosureHighArray);
            Chart_Out_SR_Arrval_Closure.Series["Series_Closure_Medium"].Points.DataBindXY(dateRangeDictArray, SRClosureMedArray);
            Chart_Out_SR_Arrval_Closure.Series["Series_Closure_Low"].Points.DataBindXY(dateRangeDictArray, SRClosureLowArray);

            //Remove empty values

            //String[] seriesArray = { "Series_SRs_High", "Series_SRs_Medium", "Series_SRs_Low", "Series_Closure_High", "Series_Closure_Medium", "Series_Closure_Low" };

            //removeZeroValues(Chart_Out_SR_Arrval_Closure, seriesArray);
        }

        protected void generateAvgSRClosureTimeOutgoing(String fromDate, String toDate, ArrayList SRList, String selectedVendor, String frequency)
        {
            if (fromDate == null || fromDate.Equals(""))
                fromDate = DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd");
            if (toDate == null || toDate.Equals(""))
                toDate = DateTime.Now.ToString("yyyy-MM-dd");

            bool vendFilterCheck = (selectedVendor != null && !selectedVendor.Equals("All")) ? true : false;
            bool generateChart = false;

            Dictionary<DateTime, double> SRClosureAvgTotal = new Dictionary<DateTime, double>();
            Dictionary<DateTime, double> SRClosureAvgHigh = new Dictionary<DateTime, double>();
            Dictionary<DateTime, double> SRClosureAvgLow = new Dictionary<DateTime, double>();
            Dictionary<DateTime, double> SRClosureAvgMed = new Dictionary<DateTime, double>();
            Dictionary<DateTime, String> dateRangeDict = new Dictionary<DateTime, string>();

            Dictionary<String, Dictionary<DateTime, String>> lastGeneratedDateRangeDict = (Dictionary<String, Dictionary<DateTime, String>>)
                Session[SessionFactory.ALL_DASHBOARD_OUTG_SR_AVERAGECLOSTIME_DATE_RANGE];

            String lastRequestedFreq = (Session[SessionFactory.ALL_DASHBOARD_OUTG_SR_AVERAGECLOSTIME_LAST_FREQ] != null ?
                Session[SessionFactory.ALL_DASHBOARD_OUTG_SR_AVERAGECLOSTIME_LAST_FREQ].ToString() : "");

            String lastSelectedAgent = (Session[SessionFactory.ALL_DASHBOARD_OUTG_SR_AVERAGECLOSTIME_LAST_SELECTED_AGENT] != null ?
                Session[SessionFactory.ALL_DASHBOARD_OUTG_SR_AVERAGECLOSTIME_LAST_SELECTED_AGENT].ToString() : "");

            if (lastGeneratedDateRangeDict != null && lastGeneratedDateRangeDict.Count > 0 && lastGeneratedDateRangeDict.ContainsKey(fromDate + "-" + toDate) && frequency.Equals(lastRequestedFreq) && lastSelectedAgent.Equals(selectedVendor))
            {
                Dictionary<String, Dictionary<DateTime, double>> allGeneratedDicts = (Dictionary<String, Dictionary<DateTime, double>>)
                    Session[SessionFactory.ALL_DASHBOARD_OUTG_SR_AVERAGECLOSTIME_GENERATED_DICTS];

                SRClosureAvgTotal = allGeneratedDicts["ClosureTotal"];
                SRClosureAvgHigh = allGeneratedDicts["ClosureHigh"];
                SRClosureAvgLow = allGeneratedDicts["ClosureLow"];
                SRClosureAvgMed = allGeneratedDicts["ClosureMed"];

                dateRangeDict = lastGeneratedDateRangeDict[fromDate + "-" + toDate];
            }
            else
            {
                int rowCount = 0;
                generateChart = true;

                if (SRList == null || SRList.Count == 0)
                {
                    Dictionary<String, String> filterParams = new Dictionary<String, String>();
                    ActionLibrary.DefectActions._dispDefectDetails dspSR = new DefectActions._dispDefectDetails();
                    if (fromDate != null && !fromDate.Equals(""))
                        filterParams.Add(dspSR.FILTER_BY_SUBMIT_DATE_FROM, fromDate);
                    if (toDate != null && !toDate.Equals(""))
                        filterParams.Add(dspSR.FILTER_BY_SUBMIT_DATE_TO, toDate);

                    SRList = dspSR.getAllSRsFilteredORDERBYCreateDate("outgoing", Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), filterParams);

                    Dictionary<DateTime, ArrayList> SRClosureAvgHighTemp = new Dictionary<DateTime, ArrayList>();
                    Dictionary<DateTime, ArrayList> SRClosureAvgLowTemp = new Dictionary<DateTime, ArrayList>();
                    Dictionary<DateTime, ArrayList> SRClosureAvgMedTemp = new Dictionary<DateTime, ArrayList>();
                    Dictionary<DateTime, ArrayList> SRClosureAvgTotalTemp = new Dictionary<DateTime, ArrayList>();

                    if (SRList != null && SRList.Count > 0)
                    {
                        String lowerDate = ((DefectDetails)SRList[0]).getDateCreated().Substring(0, ((DefectDetails)SRList[0]).getDateCreated().IndexOf(" "));
                        String higherDate = ((DefectDetails)SRList[SRList.Count - 1]).getDateCreated().Substring(0, ((DefectDetails)SRList[SRList.Count - 1]).getDateCreated().IndexOf(" "));

                        dateRangeDict = getDateRange(lowerDate, higherDate, frequency);
                        List<DateTime> dataRangeList = dateRangeDict.Keys.ToList();
                        dataRangeList.Sort();
                        //Make sure all the dictionaries have the same key set
                        foreach (KeyValuePair<DateTime, String> kvp in dateRangeDict)
                        {
                            if (!SRClosureAvgTotal.ContainsKey(kvp.Key))
                            { SRClosureAvgTotal.Add(kvp.Key, 0); SRClosureAvgTotalTemp.Add(kvp.Key, new ArrayList()); }

                            if (!SRClosureAvgHigh.ContainsKey(kvp.Key))
                            { SRClosureAvgHigh.Add(kvp.Key, 0); SRClosureAvgHighTemp.Add(kvp.Key, new ArrayList()); }

                            if (!SRClosureAvgLow.ContainsKey(kvp.Key))
                            { SRClosureAvgLow.Add(kvp.Key, 0); SRClosureAvgLowTemp.Add(kvp.Key, new ArrayList()); }

                            if (!SRClosureAvgMed.ContainsKey(kvp.Key))
                            { SRClosureAvgMed.Add(kvp.Key, 0); SRClosureAvgMedTemp.Add(kvp.Key, new ArrayList()); }
                        }

                        int index = 0;
                        bool considerRecord = true;

                        for (int i = 0; i < SRList.Count; i++)
                        {
                            DefectDetails defObj = (DefectDetails)SRList[i];
                            considerRecord = true;
                            if (vendFilterCheck)
                            {
                                if (selectedVendor.Equals(defObj.getSupplierId()))
                                    considerRecord = true;
                                else
                                    considerRecord = false;
                            }

                            if (considerRecord)
                            {
                                /*String closeDate = defObj.getCloseDate() != null && !defObj.getCloseDate().Equals("") && !(defObj.getCloseDate().IndexOf("1/1/1900") >= 0) ?
                                    defObj.getCloseDate().Replace(" ", "  ").Substring(0, 9) : "";
                                String createDate = defObj.getDateCreated().Replace(" ", "  ").Substring(0, 9);*/
                                String closeDate = defObj.getCloseDate() != null && !defObj.getCloseDate().Equals("") && !(defObj.getCloseDate().IndexOf("1/1/1900") >= 0) ?
    (defObj.getCloseDate().IndexOf(" ") >= 0 ? defObj.getCloseDate().Substring(0, defObj.getCloseDate().IndexOf(" ")) : defObj.getCloseDate()) : "";
                                String createDate = defObj.getDateCreated().IndexOf(" ") >= 0 ? defObj.getDateCreated().Substring(0, defObj.getDateCreated().IndexOf(" ")) : defObj.getDateCreated();


                                if (closeDate != null && !closeDate.Equals(""))
                                {
                                    index = dataRangeList.BinarySearch(Convert.ToDateTime(createDate));
                                    double timeTakenInHours = Convert.ToDateTime(defObj.getCloseDate()).Subtract(Convert.ToDateTime(defObj.getDateCreated())).TotalHours;
                                    timeTakenInHours = Math.Round(timeTakenInHours, 2);

                                    if (index >= 0)
                                    {
                                        //Exact Match Found
                                        if (defObj.getSeverity().Equals("High", StringComparison.InvariantCultureIgnoreCase))
                                            SRClosureAvgHighTemp[dataRangeList[index]].Add(timeTakenInHours);
                                        if (defObj.getSeverity().Equals("Medium", StringComparison.InvariantCultureIgnoreCase))
                                            SRClosureAvgMedTemp[dataRangeList[index]].Add(timeTakenInHours);
                                        if (defObj.getSeverity().Equals("Low", StringComparison.InvariantCultureIgnoreCase))
                                            SRClosureAvgLowTemp[dataRangeList[index]].Add(timeTakenInHours);

                                        SRClosureAvgTotalTemp[dataRangeList[index]].Add(timeTakenInHours);
                                    }
                                    else
                                    {
                                        //Nearest Greater Value Found
                                        index = ~index;
                                        if (defObj.getSeverity().Equals("High", StringComparison.InvariantCultureIgnoreCase))
                                            SRClosureAvgHighTemp[dataRangeList[index - 1]].Add(timeTakenInHours);
                                        if (defObj.getSeverity().Equals("Medium", StringComparison.InvariantCultureIgnoreCase))
                                            SRClosureAvgMedTemp[dataRangeList[index - 1]].Add(timeTakenInHours);
                                        if (defObj.getSeverity().Equals("Low", StringComparison.InvariantCultureIgnoreCase))
                                            SRClosureAvgLowTemp[dataRangeList[index - 1]].Add(timeTakenInHours);

                                        SRClosureAvgTotalTemp[dataRangeList[index - 1]].Add(timeTakenInHours);
                                    }
                                }

                            }
                        }
                    }

                    foreach (KeyValuePair<DateTime, ArrayList> kvp in SRClosureAvgTotalTemp)
                    {
                        ArrayList totalAvgList = kvp.Value;
                        ArrayList medAvgList = SRClosureAvgMedTemp[kvp.Key];
                        ArrayList lowAvgList = SRClosureAvgLowTemp[kvp.Key];
                        ArrayList highAvgList = SRClosureAvgHighTemp[kvp.Key];

                        double tempSum = 0;
                        for (int i = 0; i < totalAvgList.Count; i++)
                            tempSum += (double)totalAvgList[i];

                        if (tempSum > 0)
                            SRClosureAvgTotal[kvp.Key] = tempSum / (totalAvgList.Count);

                        tempSum = 0;
                        for (int i = 0; i < highAvgList.Count; i++)
                            tempSum += (double)highAvgList[i];

                        if (tempSum > 0)
                            SRClosureAvgHigh[kvp.Key] = tempSum / (highAvgList.Count);

                        tempSum = 0;
                        for (int i = 0; i < lowAvgList.Count; i++)
                            tempSum += (double)lowAvgList[i];

                        if (tempSum > 0)
                            SRClosureAvgLow[kvp.Key] = tempSum / (lowAvgList.Count);

                        tempSum = 0;
                        for (int i = 0; i < medAvgList.Count; i++)
                            tempSum += (double)medAvgList[i];

                        if (tempSum > 0)
                            SRClosureAvgMed[kvp.Key] = tempSum / (medAvgList.Count);
                    }
                }

                lastGeneratedDateRangeDict = new Dictionary<string, Dictionary<DateTime, string>>(); lastGeneratedDateRangeDict.Add(fromDate + "-" + toDate, dateRangeDict);
                Dictionary<String, Dictionary<DateTime, double>> allGeneratedDicts = new Dictionary<string, Dictionary<DateTime, double>>();


                allGeneratedDicts.Add("ClosureTotal", SRClosureAvgTotal);
                allGeneratedDicts.Add("ClosureHigh", SRClosureAvgHigh);
                allGeneratedDicts.Add("ClosureLow", SRClosureAvgLow);
                allGeneratedDicts.Add("ClosureMed", SRClosureAvgMed);

                Session[SessionFactory.ALL_DASHBOARD_OUTG_SR_AVERAGECLOSTIME_GENERATED_DICTS] = allGeneratedDicts;
                Session[SessionFactory.ALL_DASHBOARD_OUTG_SR_AVERAGECLOSTIME_DATE_RANGE] = lastGeneratedDateRangeDict;
                Session[SessionFactory.ALL_DASHBOARD_OUTG_SR_AVERAGECLOSTIME_LAST_FREQ] = frequency;
                Session[SessionFactory.ALL_DASHBOARD_OUTG_SR_AVERAGECLOSTIME_LAST_SELECTED_AGENT] = selectedVendor;

            }

            double[] SRClosureTotalArray = SRClosureAvgTotal.Values.ToArray();
            double[] SRClosureHighArray = SRClosureAvgHigh.Values.ToArray();
            double[] SRClosureLowArray = SRClosureAvgLow.Values.ToArray();
            double[] SRClosureMedArray = SRClosureAvgMed.Values.ToArray();
            String[] dateRangeDictArray = dateRangeDict.Values.ToArray();

            if (generateChart)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Date Range");
                dt.Columns.Add("Vendor");
                dt.Columns.Add("High Sev Average (Hours)");
                dt.Columns.Add("Medium Sev Average (Hours)");
                dt.Columns.Add("Low Sev Average (Hours)");
                dt.Columns.Add("Combined Average (Hours)");

                for (int i = 0; i < dateRangeDict.Count; i++)
                {
                    dt.Rows.Add();
                    dt.Rows[i]["Date Range"] = dateRangeDictArray[i];
                    dt.Rows[i]["Vendor"] = DropDownList_Outg_SR_Avg_Closure_Vendor.SelectedItem.Text;
                    dt.Rows[i]["High Sev Average (Hours)"] = SRClosureHighArray[i];
                    dt.Rows[i]["Medium Sev Average (Hours)"] = SRClosureMedArray[i];
                    dt.Rows[i]["Low Sev Average (Hours)"] = SRClosureLowArray[i];
                    dt.Rows[i]["Combined Average (Hours)"] = SRClosureTotalArray[i];
                }

                Dictionary<String, DataTable> reportDict = (Dictionary<String, DataTable>)Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES];
                if (reportDict == null)
                    reportDict = new Dictionary<string, DataTable>();

                if (!reportDict.ContainsKey("outgoing_SR_Avg_Closure"))
                    reportDict.Add("outgoing_SR_Avg_Closure", dt);
                else
                    reportDict["outgoing_SR_Avg_Closure"] = dt;

                Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES] = reportDict;

            }

            //Chart_SR_Arrival_Closure.Titles.Clear();
            //Chart_SR_Arrival_Closure.Titles.Add("SRs Arrival and Closure By Severity for Customers during the period");
            Chart_Outg_SR_Closure_Avg_Time.Titles[0].Font = new System.Drawing.Font("Book Antiqua", 10);
            Chart_Outg_SR_Closure_Avg_Time.Series["Series_SRs_Closure_Total"].Label = "#VALY{0;0;#}";
            Chart_Outg_SR_Closure_Avg_Time.Series["Series_Closure_High"].Label = "#VALY{0;0;#}";
            Chart_Outg_SR_Closure_Avg_Time.Series["Series_Closure_Medium"].Label = "#VALY{0;0;#}";
            Chart_Outg_SR_Closure_Avg_Time.Series["Series_Closure_Low"].Label = "#VALY{0;0;#}";

            Chart_Outg_SR_Closure_Avg_Time.Series["Series_SRs_Closure_Total"].Points.DataBindXY(dateRangeDictArray, SRClosureTotalArray);
            Chart_Outg_SR_Closure_Avg_Time.Series["Series_Closure_High"].Points.DataBindXY(dateRangeDictArray, SRClosureHighArray);
            Chart_Outg_SR_Closure_Avg_Time.Series["Series_Closure_Medium"].Points.DataBindXY(dateRangeDictArray, SRClosureMedArray);
            Chart_Outg_SR_Closure_Avg_Time.Series["Series_Closure_Low"].Points.DataBindXY(dateRangeDictArray, SRClosureLowArray);

        }

        protected void generateSRValByAccount(String fromDate, String toDate, Dictionary<String, DefectDetails> SRDict, Dictionary<String, String> custList)
        {
            if (fromDate == null || fromDate.Equals(""))
                fromDate = DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd");
            if (toDate == null || toDate.Equals(""))
                toDate = DateTime.Now.ToString("yyyy-MM-dd");

            bool custFilterCheck = (custList != null && custList.Count > 0) ? true : false;
            bool regenerateValues = false;


            Dictionary<String, String> contactDict = new Dictionary<String, String>();
            Dictionary<String, float> totalSRAmount = new Dictionary<string, float>();

            Dictionary<String, Dictionary<String, String>> lastGeneratedContactDict = (Dictionary<String, Dictionary<String, String>>)
                Session[SessionFactory.ALL_DASHBOARD_INCM_SR_SRVALBYACCNT_CONTACT_DICT];

            if (lastGeneratedContactDict != null && lastGeneratedContactDict.Count > 0 && lastGeneratedContactDict.ContainsKey(fromDate + "," + toDate))
            {
                totalSRAmount = ((Dictionary<String, Dictionary<String, float>>)Session[SessionFactory.ALL_DASHBOARD_INCM_SR_SRVALBYACCNT_SR_AMNT])[fromDate + "," + toDate];

                if (custFilterCheck)
                {
                    Dictionary<String, float> totalSRAmountTemp = new Dictionary<String, float>();

                    foreach (KeyValuePair<String, String> kvp in custList)
                    {
                        if (totalSRAmount.ContainsKey(kvp.Key))
                            totalSRAmountTemp.Add(kvp.Key, totalSRAmount[kvp.Key]);
                        else
                            regenerateValues = true;
                    }
                    totalSRAmount = totalSRAmountTemp;
                }
            }
            if (lastGeneratedContactDict != null && lastGeneratedContactDict.Count > 0 && lastGeneratedContactDict.ContainsKey(fromDate + "," + toDate) & !regenerateValues)
            {
                contactDict = lastGeneratedContactDict[fromDate + "," + toDate];
            }
            else
            {
                totalSRAmount = new Dictionary<string, float>();

                int rowCount = 0;
                DataTable dt = new DataTable();
                dt.Columns.Add("Customer Name");
                dt.Columns.Add("SR#");
                dt.Columns.Add("Description");
                dt.Columns.Add("SR Date");
                dt.Columns.Add("SR Amount");
                dt.Columns.Add("SR Status");
                dt.Columns.Add("SR Resolution Status");

                if (SRDict == null || SRDict.Count == 0)
                {
                    Dictionary<String, String> filterParams = new Dictionary<String, String>();
                    ActionLibrary.DefectActions._dispDefectDetails dspSR = new DefectActions._dispDefectDetails();
                    if (fromDate != null && !fromDate.Equals(""))
                        filterParams.Add(dspSR.FILTER_BY_SUBMIT_DATE_FROM, fromDate);
                    if (toDate != null && !toDate.Equals(""))
                        filterParams.Add(dspSR.FILTER_BY_SUBMIT_DATE_TO, toDate);
                    SRDict = dspSR.getAllSRsFiltered("incoming", Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), filterParams);
                }

                foreach (KeyValuePair<String, DefectDetails> kvp in SRDict)
                {
                    DefectDetails SRObj = kvp.Value;

                    String customerEntId = SRObj.getCustomerId();
                    String custName = "";
                    if (SRObj.getCreationMode().Equals(BackEndObjects.DefectDetails.DEFECT_CREATION_MODE_MANUAL))
                    {
                        custName = Contacts.getContactDetailsforContactEntityDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), customerEntId).getContactName();
                        if (custName == null || custName.Equals(""))
                            custName = MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(customerEntId).getEntityName();
                    }
                    else
                        custName = MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(customerEntId).getEntityName();

                    bool considerCustomerEnt = custFilterCheck ? custList.ContainsKey(customerEntId) : true;

                    if (considerCustomerEnt)
                    {
                        if (!contactDict.ContainsKey(customerEntId))
                        {
                            Contacts contactObj = Contacts.
                                getContactDetailsforContactEntityDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), customerEntId);
                            if (contactObj == null || contactObj.getContactName() == null || contactObj.getContactName().Equals(""))
                                contactDict.Add(customerEntId, MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(customerEntId).getEntityName());
                            else
                                contactDict.Add(customerEntId, contactObj.getContactName());
                        }

                        if (!totalSRAmount.ContainsKey(customerEntId))
                            totalSRAmount.Add(customerEntId, SRObj.getTotalAmount());
                        else
                            totalSRAmount[customerEntId] += SRObj.getTotalAmount();

                        dt.Rows.Add();

                        dt.Rows[rowCount]["Customer Name"] = contactDict[customerEntId];
                        dt.Rows[rowCount]["SR#"] = SRObj.getDefectId();
                        dt.Rows[rowCount]["Description"] = SRObj.getDescription();
                        dt.Rows[rowCount]["SR Date"] = SRObj.getDateCreated();
                        dt.Rows[rowCount]["SR Amount"] = SRObj.getTotalAmount();
                        dt.Rows[rowCount]["SR Status"] = SRObj.getDefectStat();
                        dt.Rows[rowCount]["SR Resolution Status"] = SRObj.getResolStat();

                        rowCount++;
                    }
                }

                Dictionary<String, DataTable> reportDict = (Dictionary<String, DataTable>)Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES];

                if (reportDict == null)
                    reportDict = new Dictionary<string, DataTable>();

                if (!reportDict.ContainsKey("incoming_SR_By_Accnt"))
                    reportDict.Add("incoming_SR_By_Accnt", dt);
                else
                    reportDict["incoming_SR_By_Accnt"] = dt;

                Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES] = reportDict;

                lastGeneratedContactDict = new Dictionary<string, Dictionary<string, string>>(); lastGeneratedContactDict.Add(fromDate + "," + toDate, contactDict);
                Dictionary<String, Dictionary<String, float>> lastGeneratedAmntDict = new Dictionary<string, Dictionary<string, float>>(); lastGeneratedAmntDict.Add(fromDate + "," + toDate, totalSRAmount);

                Session[SessionFactory.ALL_DASHBOARD_INCM_SR_SRVALBYACCNT_CONTACT_DICT] = lastGeneratedContactDict;
                Session[SessionFactory.ALL_DASHBOARD_INCM_SR_SRVALBYACCNT_SR_AMNT] = lastGeneratedAmntDict;

            }

            //Now populate the List box with customer details
            foreach (KeyValuePair<String, String> kvp in contactDict)
            {
                ListItem ltExists = ListBox_Incoming_SR_By_Account.Items.FindByValue(kvp.Key);
                if (ltExists == null || ltExists.Text.Equals(""))
                {
                    ltExists = new ListItem();
                    ltExists.Text = kvp.Value;
                    ltExists.Value = kvp.Key;

                    ListBox_Incoming_SR_By_Account.Items.Add(ltExists);
                }
            }

            String[] customerArray = new String[contactDict.Count];
            float[] totalSRAmountArray = new float[contactDict.Count];
            int counter = 0;
            foreach (KeyValuePair<String, float> kvp in totalSRAmount)
            {
                customerArray[counter] = contactDict[kvp.Key];
                totalSRAmountArray[counter] = totalSRAmount[kvp.Key];
                counter++;
            }

            Chart_Incoming_SR_By_Account.Titles.Clear();
            Chart_Incoming_SR_By_Account.Titles.Add("Total Invoice Value of SRs for Customers during this period (SR submit date)");
            Chart_Incoming_SR_By_Account.Titles[0].Font = new System.Drawing.Font("Book Antiqua", 10);
            Chart_Incoming_SR_By_Account.Series["TotalSRs"].Label = "#VALY{0;0;#}";
            Chart_Incoming_SR_By_Account.Series["TotalSRs"].Points.DataBindXY(customerArray, totalSRAmountArray);

        }

        protected void generateSRNoByAccount(String fromDate, String toDate, Dictionary<String, DefectDetails> SRDict, Dictionary<String, String> custList, String SRType)
        {
            if (fromDate == null || fromDate.Equals(""))
                fromDate = DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd");
            if (toDate == null || toDate.Equals(""))
                toDate = DateTime.Now.ToString("yyyy-MM-dd");

            bool custFilterCheck = (custList != null && custList.Count > 0) ? true : false;
            bool regenerateValues = false;

            Dictionary<String, String> contactDict = new Dictionary<String, String>();
            Dictionary<String, float> totalSRNo = new Dictionary<string, float>();
            Dictionary<String, float> highSR = new Dictionary<string, float>();
            Dictionary<String, float> mediumSR = new Dictionary<string, float>();
            Dictionary<String, float> lowSR = new Dictionary<string, float>();

            Dictionary<String, Dictionary<String, String>> lastGeneratedContactDict = ((Dictionary<String, Dictionary<String, String>>)
                Session[SessionFactory.ALL_DASHBOARD_INCM_SR_SRNOBYACCNT_CONTACT_DICT]);

            if (lastGeneratedContactDict != null && lastGeneratedContactDict.Count > 0 && lastGeneratedContactDict.ContainsKey(fromDate + "," + toDate))
            {
                totalSRNo = ((Dictionary<String, Dictionary<String, float>>)Session[SessionFactory.ALL_DASHBOARD_INCM_SR_SRNOBYACCNT_TOTAL_SR])[fromDate + "," + toDate];

                highSR = ((Dictionary<String, Dictionary<String, float>>)Session[SessionFactory.ALL_DASHBOARD_INCM_SR_SRNOBYACCNT_HIGH_SR])[fromDate + "," + toDate];
                mediumSR = ((Dictionary<String, Dictionary<String, float>>)Session[SessionFactory.ALL_DASHBOARD_INCM_SR_SRNOBYACCNT_MEDM_SR])[fromDate + "," + toDate];
                lowSR = ((Dictionary<String, Dictionary<String, float>>)Session[SessionFactory.ALL_DASHBOARD_INCM_SR_SRNOBYACCNT_LOW_SR])[fromDate + "," + toDate];

                if (custFilterCheck)
                {
                    Dictionary<String, float> totalSRNoTemp = new Dictionary<String, float>();
                    Dictionary<String, float> highSRTemp = new Dictionary<String, float>();
                    Dictionary<String, float> lowSRTemp = new Dictionary<String, float>();
                    Dictionary<String, float> mediumSRTemp = new Dictionary<String, float>();

                    foreach (KeyValuePair<String, String> kvp in custList)
                    {
                        if (totalSRNo.ContainsKey(kvp.Key))
                            totalSRNoTemp.Add(kvp.Key, totalSRNo[kvp.Key]);
                        else
                            regenerateValues = true;

                        if (highSR.ContainsKey(kvp.Key))
                            highSRTemp.Add(kvp.Key, highSR[kvp.Key]);
                        else
                            regenerateValues = true;

                        if (lowSR.ContainsKey(kvp.Key))
                            lowSRTemp.Add(kvp.Key, lowSR[kvp.Key]);
                        else
                            regenerateValues = true;

                        if (mediumSR.ContainsKey(kvp.Key))
                            mediumSRTemp.Add(kvp.Key, mediumSR[kvp.Key]);
                        else
                            regenerateValues = true;
                    }

                    highSR = highSRTemp;
                    lowSR = lowSRTemp;
                    mediumSR = mediumSRTemp;
                    totalSRNo = totalSRNoTemp;
                }

                String lastSelectedSRType = Session[SessionFactory.ALL_DASHBOARD_INCM_SR_SRNOBYACCNT_LAST_SR_TYPE] != null ?
                    Session[SessionFactory.ALL_DASHBOARD_INCM_SR_SRNOBYACCNT_LAST_SR_TYPE].ToString() : "";

                if (!lastSelectedSRType.Equals(SRType))
                    regenerateValues = true;

            }

            if (lastGeneratedContactDict != null && lastGeneratedContactDict.Count > 0 && lastGeneratedContactDict.ContainsKey(fromDate + "," + toDate) && !regenerateValues)
            {
                contactDict = lastGeneratedContactDict[fromDate + "," + toDate];
            }
            else
            {
                highSR = new Dictionary<string, float>();
                lowSR = new Dictionary<string, float>();
                mediumSR = new Dictionary<string, float>();
                totalSRNo = new Dictionary<string, float>();


                int rowCount = 0;
                DataTable dt = new DataTable();
                dt.Columns.Add("Customer Name");
                dt.Columns.Add("Severity");
                dt.Columns.Add("SR#");
                dt.Columns.Add("Description");
                dt.Columns.Add("SR Date");
                dt.Columns.Add("Close Date");
                dt.Columns.Add("SR Amount");
                dt.Columns.Add("SR Status");
                dt.Columns.Add("SR Resolution Status");
                dt.Columns.Add("Assigned To");

                if (SRDict == null || SRDict.Count == 0)
                {
                    Dictionary<String, String> filterParams = new Dictionary<String, String>();
                    ActionLibrary.DefectActions._dispDefectDetails dspSR = new DefectActions._dispDefectDetails();
                    if (fromDate != null && !fromDate.Equals(""))
                        filterParams.Add(dspSR.FILTER_BY_SUBMIT_DATE_FROM, fromDate);
                    if (toDate != null && !toDate.Equals(""))
                        filterParams.Add(dspSR.FILTER_BY_SUBMIT_DATE_TO, toDate);

                    SRDict = dspSR.getAllSRsFiltered("incoming", Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), filterParams);
                }

                bool considerRecordBySRType = true;

                foreach (KeyValuePair<String, DefectDetails> kvp in SRDict)
                {
                    DefectDetails SRObj = kvp.Value;

                    considerRecordBySRType = true;

                    if (!"All".Equals(SRType))
                    {
                        if ("Open".Equals(SRType) &&
                            SRObj.getResolStat().Equals(BackEndObjects.DefectResolStatCodes.DEFECT_RESOL_STAT_CODE_RESOLVED))
                            considerRecordBySRType = false;
                        else if ("Resolved".Equals(SRType) &&
                            !SRObj.getResolStat().Equals(BackEndObjects.DefectResolStatCodes.DEFECT_RESOL_STAT_CODE_RESOLVED))
                            considerRecordBySRType = false;
                    }

                    if (considerRecordBySRType)
                    {
                        String customerEntId = SRObj.getCustomerId();
                        String custName = "";
                        if (SRObj.getCreationMode().Equals(BackEndObjects.DefectDetails.DEFECT_CREATION_MODE_MANUAL))
                        {
                            custName = Contacts.getContactDetailsforContactEntityDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), customerEntId).getContactName();
                            if (custName == null || custName.Equals(""))
                                custName = MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(customerEntId).getEntityName();
                        }
                        else
                            custName = MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(customerEntId).getEntityName();

                        bool considerCustomerEnt = custFilterCheck ? custList.ContainsKey(customerEntId) : true;

                        if (considerCustomerEnt)
                        {
                            if (!contactDict.ContainsKey(customerEntId))
                            {
                                Contacts contactObj = Contacts.
                                    getContactDetailsforContactEntityDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), customerEntId);
                                if (contactObj == null || contactObj.getContactName() == null || contactObj.getContactName().Equals(""))
                                    contactDict.Add(customerEntId, MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(customerEntId).getEntityName());
                                else
                                    contactDict.Add(customerEntId, contactObj.getContactName());

                                if (!highSR.ContainsKey(customerEntId))
                                    highSR.Add(customerEntId, 0);

                                if (!lowSR.ContainsKey(customerEntId))
                                    lowSR.Add(customerEntId, 0);

                                if (!mediumSR.ContainsKey(customerEntId))
                                    mediumSR.Add(customerEntId, 0);
                            }

                            if (!totalSRNo.ContainsKey(customerEntId))
                                totalSRNo.Add(customerEntId, 1);
                            else
                                totalSRNo[customerEntId] += 1;

                            if (SRObj.getSeverity().Equals("High", StringComparison.InvariantCultureIgnoreCase))
                                highSR[customerEntId] += 1;
                            if (SRObj.getSeverity().Equals("Low", StringComparison.InvariantCultureIgnoreCase))
                                lowSR[customerEntId] += 1;
                            if (SRObj.getSeverity().Equals("Medium", StringComparison.InvariantCultureIgnoreCase))
                                mediumSR[customerEntId] += 1;

                            dt.Rows.Add();

                            dt.Rows[rowCount]["Customer Name"] = contactDict[customerEntId];
                            dt.Rows[rowCount]["Severity"] = SRObj.getSeverity();
                            dt.Rows[rowCount]["SR#"] = SRObj.getDefectId();
                            dt.Rows[rowCount]["Description"] = SRObj.getDescription();
                            dt.Rows[rowCount]["SR Date"] = SRObj.getDateCreated();
                            dt.Rows[rowCount]["Close Date"] = SRObj.getCloseDate();
                            dt.Rows[rowCount]["SR Amount"] = SRObj.getTotalAmount();
                            dt.Rows[rowCount]["SR Status"] = SRObj.getDefectStat();
                            dt.Rows[rowCount]["SR Resolution Status"] = SRObj.getResolStat();
                            dt.Rows[rowCount]["Assigned To"] = SRObj.getAssignedToUser();

                            rowCount++;
                        }
                    }
                }

                Dictionary<String, DataTable> reportDict = (Dictionary<String, DataTable>)Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES];

                if (reportDict == null)
                    reportDict = new Dictionary<string, DataTable>();

                if (!reportDict.ContainsKey("incoming_SR_No_By_Accnt"))
                    reportDict.Add("incoming_SR_No_By_Accnt", dt);
                else
                    reportDict["incoming_SR_No_By_Accnt"] = dt;

                Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES] = reportDict;
                lastGeneratedContactDict = new Dictionary<string, Dictionary<string, string>>(); lastGeneratedContactDict.Add(fromDate + "," + toDate, contactDict);
                Dictionary<String, Dictionary<String, float>> lastGeneratedTotalSRDict = new Dictionary<string, Dictionary<string, float>>(); lastGeneratedTotalSRDict.Add(fromDate + "," + toDate, totalSRNo);
                Dictionary<String, Dictionary<String, float>> lastGeneratedHighSRDict = new Dictionary<string, Dictionary<string, float>>(); lastGeneratedHighSRDict.Add(fromDate + "," + toDate, highSR);
                Dictionary<String, Dictionary<String, float>> lastGeneratedLowSRDict = new Dictionary<string, Dictionary<string, float>>(); lastGeneratedLowSRDict.Add(fromDate + "," + toDate, lowSR);
                Dictionary<String, Dictionary<String, float>> lastGeneratedMedmSRDict = new Dictionary<string, Dictionary<string, float>>(); lastGeneratedMedmSRDict.Add(fromDate + "," + toDate, mediumSR);

                Session[SessionFactory.ALL_DASHBOARD_INCM_SR_SRNOBYACCNT_CONTACT_DICT] = lastGeneratedContactDict;
                Session[SessionFactory.ALL_DASHBOARD_INCM_SR_SRNOBYACCNT_HIGH_SR] = lastGeneratedHighSRDict;
                Session[SessionFactory.ALL_DASHBOARD_INCM_SR_SRNOBYACCNT_MEDM_SR] = lastGeneratedMedmSRDict;
                Session[SessionFactory.ALL_DASHBOARD_INCM_SR_SRNOBYACCNT_LOW_SR] = lastGeneratedLowSRDict;
                Session[SessionFactory.ALL_DASHBOARD_INCM_SR_SRNOBYACCNT_TOTAL_SR] = lastGeneratedTotalSRDict;
                Session[SessionFactory.ALL_DASHBOARD_INCM_SR_SRNOBYACCNT_LAST_SR_TYPE] = SRType;
            }
            //Now populate the List box with customer details
            foreach (KeyValuePair<String, String> kvp in contactDict)
            {
                ListItem ltExists = Listbox_Incoming_SR_No_By_Account.Items.FindByValue(kvp.Key);
                if (ltExists == null || ltExists.Text.Equals(""))
                {
                    ltExists = new ListItem();
                    ltExists.Text = kvp.Value;
                    ltExists.Value = kvp.Key;

                    Listbox_Incoming_SR_No_By_Account.Items.Add(ltExists);
                }
            }

            String[] customerArray = new String[contactDict.Count];
            float[] totalSRNoArray = new float[contactDict.Count];
            float[] totalHighSRNoArray = new float[contactDict.Count];
            float[] totalLowSRNoArray = new float[contactDict.Count];
            float[] totalMediumSRNoArray = new float[contactDict.Count];

            int counter = 0;
            foreach (KeyValuePair<String, float> kvp in totalSRNo)
            {
                customerArray[counter] = contactDict[kvp.Key];

                totalSRNoArray[counter] = totalSRNo[kvp.Key];
                totalHighSRNoArray[counter] = highSR[kvp.Key];
                totalLowSRNoArray[counter] = lowSR[kvp.Key];
                totalMediumSRNoArray[counter] = mediumSR[kvp.Key];

                counter++;
            }

            Chart_Incoming_SR_No_By_Account.Titles.Clear();
            Chart_Incoming_SR_No_By_Account.Titles.Add("Total Number of SRs for Customers during this period (SR submit date)");
            Chart_Incoming_SR_No_By_Account.Titles[0].Font = new System.Drawing.Font("Book Antiqua", 10);
            Chart_Incoming_SR_No_By_Account.Series["HighSRs"].Label = "#VALY{0;0;#}";
            Chart_Incoming_SR_No_By_Account.Series["LowSRs"].Label = "#VALY{0;0;#}";
            Chart_Incoming_SR_No_By_Account.Series["MediumSRs"].Label = "#VALY{0;0;#}";

            Chart_Incoming_SR_No_By_Account.Series["HighSRs"].Points.DataBindXY(customerArray, totalHighSRNoArray);
            Chart_Incoming_SR_No_By_Account.Series["LowSRs"].Points.DataBindXY(customerArray, totalLowSRNoArray);
            Chart_Incoming_SR_No_By_Account.Series["MediumSRs"].Points.DataBindXY(customerArray, totalMediumSRNoArray);
        }

        protected void generateSRArrivalandClosure(String fromDate, String toDate, ArrayList SRList, Dictionary<String, String> userList, String frequency)
        {
            if (fromDate == null || fromDate.Equals(""))
                fromDate = DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd");
            if (toDate == null || toDate.Equals(""))
                toDate = DateTime.Now.ToString("yyyy-MM-dd");

            bool userFilterCheck = (userList != null && userList.Count > 0) ? true : false;
            bool generateChart = false;

            Dictionary<DateTime, float> totalSRArrivalHigh = new Dictionary<DateTime, float>();
            Dictionary<DateTime, float> totalSRArrivalLow = new Dictionary<DateTime, float>();
            Dictionary<DateTime, float> totalSRArrivalMed = new Dictionary<DateTime, float>();
            Dictionary<DateTime, float> SRClosureHigh = new Dictionary<DateTime, float>();
            Dictionary<DateTime, float> SRClosureLow = new Dictionary<DateTime, float>();
            Dictionary<DateTime, float> SRClosureMed = new Dictionary<DateTime, float>();
            Dictionary<DateTime, String> dateRangeDict = new Dictionary<DateTime, string>();

            Dictionary<String, Dictionary<DateTime, String>> lastGeneratedDateRangeDict = (Dictionary<String, Dictionary<DateTime, String>>)Session[SessionFactory.ALL_DASHBOARD_INCM_SR_ARRVLCLOS_DATE_RANGE];
            String lastRequestedFreq = (Session[SessionFactory.ALL_DASHBOARD_INCM_SR_ARRVLCLOS_LAST_FREQ] != null ? Session[SessionFactory.ALL_DASHBOARD_INCM_SR_ARRVLCLOS_LAST_FREQ].ToString() : "");

            if (lastGeneratedDateRangeDict != null && lastGeneratedDateRangeDict.Count > 0 && lastGeneratedDateRangeDict.ContainsKey(fromDate + "-" + toDate) && frequency.Equals(lastRequestedFreq))
            {
                Dictionary<String, Dictionary<DateTime, float>> allGeneratedDicts = (Dictionary<String, Dictionary<DateTime, float>>)
                    Session[SessionFactory.ALL_DASHBOARD_INCM_SR_ARRVLCLOS_GENERATED_DICTS];

                totalSRArrivalHigh = allGeneratedDicts["ArrivalHigh"];
                totalSRArrivalLow = allGeneratedDicts["ArrivalLow"];
                totalSRArrivalMed = allGeneratedDicts["ArrivalMed"];
                SRClosureHigh = allGeneratedDicts["ClosureHigh"];
                SRClosureLow = allGeneratedDicts["ClosureLow"];
                SRClosureMed = allGeneratedDicts["ClosureMed"];

                dateRangeDict = lastGeneratedDateRangeDict[fromDate + "-" + toDate];
            }
            else
            {
                generateChart = true;

                if (SRList == null || SRList.Count == 0)
                {
                    Dictionary<String, String> filterParams = new Dictionary<String, String>();
                    ActionLibrary.DefectActions._dispDefectDetails dspSR = new DefectActions._dispDefectDetails();
                    if (fromDate != null && !fromDate.Equals(""))
                        filterParams.Add(dspSR.FILTER_BY_SUBMIT_DATE_FROM, fromDate);
                    if (toDate != null && !toDate.Equals(""))
                        filterParams.Add(dspSR.FILTER_BY_SUBMIT_DATE_TO, toDate);

                    SRList = dspSR.getAllSRsFilteredORDERBYCreateDate("incoming", Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), filterParams);
                    if (SRList != null && SRList.Count > 0)
                    {
                        String lowerDate = ((DefectDetails)SRList[0]).getDateCreated().Substring(0, ((DefectDetails)SRList[0]).getDateCreated().IndexOf(" "));
                        String higherDate = ((DefectDetails)SRList[SRList.Count - 1]).getDateCreated().Substring(0, ((DefectDetails)SRList[SRList.Count - 1]).getDateCreated().IndexOf(" "));

                        dateRangeDict = getDateRange(lowerDate, higherDate, frequency);
                        List<DateTime> dataRangeList = dateRangeDict.Keys.ToList();
                        dataRangeList.Sort();
                        //Make sure all the dictionaries have the same key set
                        foreach (KeyValuePair<DateTime, String> kvp in dateRangeDict)
                        {
                            if (!totalSRArrivalHigh.ContainsKey(kvp.Key))
                                totalSRArrivalHigh.Add(kvp.Key, 0);

                            if (!totalSRArrivalLow.ContainsKey(kvp.Key))
                                totalSRArrivalLow.Add(kvp.Key, 0);

                            if (!totalSRArrivalMed.ContainsKey(kvp.Key))
                                totalSRArrivalMed.Add(kvp.Key, 0);

                            if (!SRClosureHigh.ContainsKey(kvp.Key))
                                SRClosureHigh.Add(kvp.Key, 0);

                            if (!SRClosureLow.ContainsKey(kvp.Key))
                                SRClosureLow.Add(kvp.Key, 0);

                            if (!SRClosureMed.ContainsKey(kvp.Key))
                                SRClosureMed.Add(kvp.Key, 0);
                        }

                        for (int i = 0; i < SRList.Count; i++)
                        {
                            DefectDetails defObj = (DefectDetails)SRList[i];
                            /*String closeDate = defObj.getCloseDate() != null && !defObj.getCloseDate().Equals("")  && !(defObj.getCloseDate().IndexOf("1/1/1900")>=0)?
                                defObj.getCloseDate().Replace(" ", "  ").Substring(0, 9) : "";
                            String createDate = defObj.getDateCreated().Replace(" ", "  ").Substring(0, 9);*/
                            String closeDate = defObj.getCloseDate() != null && !defObj.getCloseDate().Equals("") && !(defObj.getCloseDate().IndexOf("1/1/1900") >= 0) ?
    (defObj.getCloseDate().IndexOf(" ") >= 0 ? defObj.getCloseDate().Substring(0, defObj.getCloseDate().IndexOf(" ")) : defObj.getCloseDate()) : "";
                            String createDate = defObj.getDateCreated().IndexOf(" ") >= 0 ? defObj.getDateCreated().Substring(0, defObj.getDateCreated().IndexOf(" ")) : defObj.getDateCreated();

                            int index = dataRangeList.BinarySearch(Convert.ToDateTime(createDate));
                            if (index >= 0)
                            {
                                //Exact Match Found
                                if (defObj.getSeverity().Equals("High", StringComparison.InvariantCultureIgnoreCase))
                                    totalSRArrivalHigh[dataRangeList[index]] += 1;
                                if (defObj.getSeverity().Equals("Medium", StringComparison.InvariantCultureIgnoreCase))
                                    totalSRArrivalMed[dataRangeList[index]] += 1;
                                if (defObj.getSeverity().Equals("Low", StringComparison.InvariantCultureIgnoreCase))
                                    totalSRArrivalLow[dataRangeList[index]] += 1;
                            }
                            else
                            {
                                //Nearest Greater Value Found
                                index = ~index;
                                if (defObj.getSeverity().Equals("High", StringComparison.InvariantCultureIgnoreCase))
                                    totalSRArrivalHigh[dataRangeList[index - 1]] += 1;
                                if (defObj.getSeverity().Equals("Medium", StringComparison.InvariantCultureIgnoreCase))
                                    totalSRArrivalMed[dataRangeList[index - 1]] += 1;
                                if (defObj.getSeverity().Equals("Low", StringComparison.InvariantCultureIgnoreCase))
                                    totalSRArrivalLow[dataRangeList[index - 1]] += 1;
                            }

                            if (closeDate != null && !closeDate.Equals(""))
                            {
                                index = dataRangeList.BinarySearch(Convert.ToDateTime(closeDate));
                                if (index >= 0)
                                {
                                    //Exact Match Found
                                    if (defObj.getSeverity().Equals("High", StringComparison.InvariantCultureIgnoreCase))
                                        SRClosureHigh[dataRangeList[index]] += 1;
                                    if (defObj.getSeverity().Equals("Medium", StringComparison.InvariantCultureIgnoreCase))
                                        SRClosureMed[dataRangeList[index]] += 1;
                                    if (defObj.getSeverity().Equals("Low", StringComparison.InvariantCultureIgnoreCase))
                                        SRClosureLow[dataRangeList[index]] += 1;
                                }
                                else
                                {
                                    //Nearest Greater Value Found
                                    index = ~index;
                                    if (defObj.getSeverity().Equals("High", StringComparison.InvariantCultureIgnoreCase))
                                        SRClosureHigh[dataRangeList[index - 1]] += 1;
                                    if (defObj.getSeverity().Equals("Medium", StringComparison.InvariantCultureIgnoreCase))
                                        SRClosureMed[dataRangeList[index - 1]] += 1;
                                    if (defObj.getSeverity().Equals("Low", StringComparison.InvariantCultureIgnoreCase))
                                        SRClosureLow[dataRangeList[index - 1]] += 1;
                                }
                            }
                        }
                    }

                }

                lastGeneratedDateRangeDict = new Dictionary<string, Dictionary<DateTime, string>>(); lastGeneratedDateRangeDict.Add(fromDate + "-" + toDate, dateRangeDict);
                Dictionary<String, Dictionary<DateTime, float>> allGeneratedDicts = new Dictionary<string, Dictionary<DateTime, float>>();
                allGeneratedDicts.Add("ArrivalHigh", totalSRArrivalHigh);
                allGeneratedDicts.Add("ArrivalLow", totalSRArrivalLow);
                allGeneratedDicts.Add("ArrivalMed", totalSRArrivalMed);
                allGeneratedDicts.Add("ClosureHigh", SRClosureHigh);
                allGeneratedDicts.Add("ClosureLow", SRClosureLow);
                allGeneratedDicts.Add("ClosureMed", SRClosureMed);

                Session[SessionFactory.ALL_DASHBOARD_INCM_SR_ARRVLCLOS_GENERATED_DICTS] = allGeneratedDicts;
                Session[SessionFactory.ALL_DASHBOARD_INCM_SR_ARRVLCLOS_DATE_RANGE] = lastGeneratedDateRangeDict;
                Session[SessionFactory.ALL_DASHBOARD_INCM_SR_ARRVLCLOS_LAST_FREQ] = frequency;

            }

            float[] totalSRArrivalHighArray = totalSRArrivalHigh.Values.ToArray();
            float[] totalSRArrivalLowArray = totalSRArrivalLow.Values.ToArray();
            float[] totalSRArrivalMedArray = totalSRArrivalMed.Values.ToArray();
            float[] SRClosureHighArray = SRClosureHigh.Values.ToArray();
            float[] SRClosureLowArray = SRClosureLow.Values.ToArray();
            float[] SRClosureMedArray = SRClosureMed.Values.ToArray();
            String[] dateRangeDictArray = dateRangeDict.Values.ToArray();

            if (generateChart)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Date Range");
                dt.Columns.Add("No of High Sev Created");
                dt.Columns.Add("No of High Sev Closed");
                dt.Columns.Add("No of Medium Sev Created");
                dt.Columns.Add("No of Medium Sev Closed");
                dt.Columns.Add("No of Low Sev Created");
                dt.Columns.Add("No of Low Sev Closed");

                for (int i = 0; i < dateRangeDictArray.Length; i++)
                {
                    dt.Rows.Add();
                    dt.Rows[i]["Date Range"] = dateRangeDictArray[i];
                    dt.Rows[i]["No of High Sev Created"] = totalSRArrivalHighArray[i];
                    dt.Rows[i]["No of High Sev Closed"] = SRClosureHighArray[i];
                    dt.Rows[i]["No of Medium Sev Created"] = totalSRArrivalMedArray[i];
                    dt.Rows[i]["No of Medium Sev Closed"] = SRClosureMedArray[i];
                    dt.Rows[i]["No of Low Sev Created"] = totalSRArrivalLowArray[i];
                    dt.Rows[i]["No of Low Sev Closed"] = SRClosureLowArray[i];

                }

                Dictionary<String, DataTable> reportDict = (Dictionary<String, DataTable>)Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES];
                if (reportDict == null)
                    reportDict = new Dictionary<string, DataTable>();

                if (!reportDict.ContainsKey("incoming_SR_Arrival_Closure"))
                    reportDict.Add("incoming_SR_Arrival_Closure", dt);
                else
                    reportDict["incoming_SR_Arrival_Closure"] = dt;

                Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES] = reportDict;
            }

            //Chart_SR_Arrival_Closure.Titles.Clear();
            //Chart_SR_Arrival_Closure.Titles.Add("SRs Arrival and Closure By Severity for Customers during the period");
            Chart_SR_Arrival_Closure.Titles[0].Font = new System.Drawing.Font("Book Antiqua", 10);
            Chart_SR_Arrival_Closure.Series["Series_SRs_High"].Label = "#VALY{0;0;#}";
            Chart_SR_Arrival_Closure.Series["Series_SRs_Medium"].Label = "#VALY{0;0;#}";
            Chart_SR_Arrival_Closure.Series["Series_SRs_Low"].Label = "#VALY{0;0;#}";
            Chart_SR_Arrival_Closure.Series["Series_Closure_High"].Label = "#VALY{0;0;#}";
            Chart_SR_Arrival_Closure.Series["Series_Closure_Medium"].Label = "#VALY{0;0;#}";
            Chart_SR_Arrival_Closure.Series["Series_Closure_Low"].Label = "#VALY{0;0;#}";

            Chart_SR_Arrival_Closure.Series["Series_SRs_High"].Points.DataBindXY(dateRangeDictArray, totalSRArrivalHighArray);
            Chart_SR_Arrival_Closure.Series["Series_SRs_Medium"].Points.DataBindXY(dateRangeDictArray, totalSRArrivalMedArray);
            Chart_SR_Arrival_Closure.Series["Series_SRs_Low"].Points.DataBindXY(dateRangeDictArray, totalSRArrivalLowArray);
            Chart_SR_Arrival_Closure.Series["Series_Closure_High"].Points.DataBindXY(dateRangeDictArray, SRClosureHighArray);
            Chart_SR_Arrival_Closure.Series["Series_Closure_Medium"].Points.DataBindXY(dateRangeDictArray, SRClosureMedArray);
            Chart_SR_Arrival_Closure.Series["Series_Closure_Low"].Points.DataBindXY(dateRangeDictArray, SRClosureLowArray);

        }

        protected void generateAvgSRClosureTime(String fromDate, String toDate, ArrayList SRList, String selectedAgent, String frequency)
        {
            if (fromDate == null || fromDate.Equals(""))
                fromDate = DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd");
            if (toDate == null || toDate.Equals(""))
                toDate = DateTime.Now.ToString("yyyy-MM-dd");

            bool agentFilterCheck = (selectedAgent != null && !selectedAgent.Equals("All")) ? true : false;
            bool generateChart = false;

            Dictionary<DateTime, double> SRClosureAvgTotal = new Dictionary<DateTime, double>();
            Dictionary<DateTime, double> SRClosureAvgHigh = new Dictionary<DateTime, double>();
            Dictionary<DateTime, double> SRClosureAvgLow = new Dictionary<DateTime, double>();
            Dictionary<DateTime, double> SRClosureAvgMed = new Dictionary<DateTime, double>();
            Dictionary<DateTime, String> dateRangeDict = new Dictionary<DateTime, string>();

            Dictionary<String, Dictionary<DateTime, String>> lastGeneratedDateRangeDict = (Dictionary<String, Dictionary<DateTime, String>>)
                Session[SessionFactory.ALL_DASHBOARD_INCM_SR_AVERAGECLOSTIME_DATE_RANGE];

            String lastRequestedFreq = (Session[SessionFactory.ALL_DASHBOARD_INCM_SR_AVERAGECLOSTIME_LAST_FREQ] != null ?
                Session[SessionFactory.ALL_DASHBOARD_INCM_SR_AVERAGECLOSTIME_LAST_FREQ].ToString() : "");

            String lastSelectedAgent = (Session[SessionFactory.ALL_DASHBOARD_INCM_SR_AVERAGECLOSTIME_LAST_SELECTED_AGENT] != null ?
                Session[SessionFactory.ALL_DASHBOARD_INCM_SR_AVERAGECLOSTIME_LAST_SELECTED_AGENT].ToString() : "");

            if (lastGeneratedDateRangeDict != null && lastGeneratedDateRangeDict.Count > 0 && lastGeneratedDateRangeDict.ContainsKey(fromDate + "-" + toDate) && frequency.Equals(lastRequestedFreq) && lastSelectedAgent.Equals(selectedAgent))
            {
                Dictionary<String, Dictionary<DateTime, double>> allGeneratedDicts = (Dictionary<String, Dictionary<DateTime, double>>)
                    Session[SessionFactory.ALL_DASHBOARD_INCM_SR_AVERAGECLOSTIME_GENERATED_DICTS];

                SRClosureAvgTotal = allGeneratedDicts["ClosureTotal"];
                SRClosureAvgHigh = allGeneratedDicts["ClosureHigh"];
                SRClosureAvgLow = allGeneratedDicts["ClosureLow"];
                SRClosureAvgMed = allGeneratedDicts["ClosureMed"];

                dateRangeDict = lastGeneratedDateRangeDict[fromDate + "-" + toDate];
            }
            else
            {
                generateChart = true;

                if (SRList == null || SRList.Count == 0)
                {
                    Dictionary<String, String> filterParams = new Dictionary<String, String>();
                    ActionLibrary.DefectActions._dispDefectDetails dspSR = new DefectActions._dispDefectDetails();
                    if (fromDate != null && !fromDate.Equals(""))
                        filterParams.Add(dspSR.FILTER_BY_SUBMIT_DATE_FROM, fromDate);
                    if (toDate != null && !toDate.Equals(""))
                        filterParams.Add(dspSR.FILTER_BY_SUBMIT_DATE_TO, toDate);

                    SRList = dspSR.getAllSRsFilteredORDERBYCreateDate("incoming", Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), filterParams);

                    Dictionary<DateTime, ArrayList> SRClosureAvgHighTemp = new Dictionary<DateTime, ArrayList>();
                    Dictionary<DateTime, ArrayList> SRClosureAvgLowTemp = new Dictionary<DateTime, ArrayList>();
                    Dictionary<DateTime, ArrayList> SRClosureAvgMedTemp = new Dictionary<DateTime, ArrayList>();
                    Dictionary<DateTime, ArrayList> SRClosureAvgTotalTemp = new Dictionary<DateTime, ArrayList>();

                    if (SRList != null && SRList.Count > 0)
                    {
                        String lowerDate = ((DefectDetails)SRList[0]).getDateCreated().Substring(0, ((DefectDetails)SRList[0]).getDateCreated().IndexOf(" "));
                        String higherDate = ((DefectDetails)SRList[SRList.Count - 1]).getDateCreated().Substring(0, ((DefectDetails)SRList[SRList.Count - 1]).getDateCreated().IndexOf(" "));

                        dateRangeDict = getDateRange(lowerDate, higherDate, frequency);
                        List<DateTime> dataRangeList = dateRangeDict.Keys.ToList();
                        dataRangeList.Sort();
                        //Make sure all the dictionaries have the same key set
                        foreach (KeyValuePair<DateTime, String> kvp in dateRangeDict)
                        {
                            if (!SRClosureAvgTotal.ContainsKey(kvp.Key))
                            { SRClosureAvgTotal.Add(kvp.Key, 0); SRClosureAvgTotalTemp.Add(kvp.Key, new ArrayList()); }

                            if (!SRClosureAvgHigh.ContainsKey(kvp.Key))
                            { SRClosureAvgHigh.Add(kvp.Key, 0); SRClosureAvgHighTemp.Add(kvp.Key, new ArrayList()); }

                            if (!SRClosureAvgLow.ContainsKey(kvp.Key))
                            { SRClosureAvgLow.Add(kvp.Key, 0); SRClosureAvgLowTemp.Add(kvp.Key, new ArrayList()); }

                            if (!SRClosureAvgMed.ContainsKey(kvp.Key))
                            { SRClosureAvgMed.Add(kvp.Key, 0); SRClosureAvgMedTemp.Add(kvp.Key, new ArrayList()); }
                        }

                        int index = 0;
                        bool considerRecord = true;

                        for (int i = 0; i < SRList.Count; i++)
                        {
                            DefectDetails defObj = (DefectDetails)SRList[i];
                            considerRecord = true;
                            if (agentFilterCheck)
                            {
                                if (selectedAgent.Equals(defObj.getAssignedToUser()))
                                    considerRecord = true;
                                else
                                    considerRecord = false;
                            }

                            if (considerRecord)
                            {
                                /*String closeDate = defObj.getCloseDate() != null && !defObj.getCloseDate().Equals("") && !(defObj.getCloseDate().IndexOf("1/1/1900") >= 0) ?
                                    defObj.getCloseDate().Replace(" ", "  ").Substring(0, 9) : "";
                                String createDate = defObj.getDateCreated().Replace(" ", "  ").Substring(0, 9);*/
                                String closeDate = defObj.getCloseDate() != null && !defObj.getCloseDate().Equals("") && !(defObj.getCloseDate().IndexOf("1/1/1900") >= 0) ?
    (defObj.getCloseDate().IndexOf(" ") >= 0 ? defObj.getCloseDate().Substring(0, defObj.getCloseDate().IndexOf(" ")) : defObj.getCloseDate()) : "";
                                String createDate = defObj.getDateCreated().IndexOf(" ") >= 0 ? defObj.getDateCreated().Substring(0, defObj.getDateCreated().IndexOf(" ")) : defObj.getDateCreated();

                                if (closeDate != null && !closeDate.Equals(""))
                                {
                                    index = dataRangeList.BinarySearch(Convert.ToDateTime(createDate));
                                    double timeTakenInHours = Convert.ToDateTime(defObj.getCloseDate()).Subtract(Convert.ToDateTime(defObj.getDateCreated())).TotalHours;
                                    timeTakenInHours = Math.Round(timeTakenInHours, 2);

                                    if (index >= 0)
                                    {
                                        //Exact Match Found
                                        if (defObj.getSeverity().Equals("High", StringComparison.InvariantCultureIgnoreCase))
                                            SRClosureAvgHighTemp[dataRangeList[index]].Add(timeTakenInHours);
                                        if (defObj.getSeverity().Equals("Medium", StringComparison.InvariantCultureIgnoreCase))
                                            SRClosureAvgMedTemp[dataRangeList[index]].Add(timeTakenInHours);
                                        if (defObj.getSeverity().Equals("Low", StringComparison.InvariantCultureIgnoreCase))
                                            SRClosureAvgLowTemp[dataRangeList[index]].Add(timeTakenInHours);

                                        SRClosureAvgTotalTemp[dataRangeList[index]].Add(timeTakenInHours);
                                    }
                                    else
                                    {
                                        //Nearest Greater Value Found
                                        index = ~index;
                                        if (defObj.getSeverity().Equals("High", StringComparison.InvariantCultureIgnoreCase))
                                            SRClosureAvgHighTemp[dataRangeList[index - 1]].Add(timeTakenInHours);
                                        if (defObj.getSeverity().Equals("Medium", StringComparison.InvariantCultureIgnoreCase))
                                            SRClosureAvgMedTemp[dataRangeList[index - 1]].Add(timeTakenInHours);
                                        if (defObj.getSeverity().Equals("Low", StringComparison.InvariantCultureIgnoreCase))
                                            SRClosureAvgLowTemp[dataRangeList[index - 1]].Add(timeTakenInHours);

                                        SRClosureAvgTotalTemp[dataRangeList[index - 1]].Add(timeTakenInHours);
                                    }
                                }

                            }
                        }
                    }

                    foreach (KeyValuePair<DateTime, ArrayList> kvp in SRClosureAvgTotalTemp)
                    {
                        ArrayList totalAvgList = kvp.Value;
                        ArrayList medAvgList = SRClosureAvgMedTemp[kvp.Key];
                        ArrayList lowAvgList = SRClosureAvgLowTemp[kvp.Key];
                        ArrayList highAvgList = SRClosureAvgHighTemp[kvp.Key];

                        double tempSum = 0;
                        for (int i = 0; i < totalAvgList.Count; i++)
                            tempSum += (double)totalAvgList[i];

                        if (tempSum > 0)
                            SRClosureAvgTotal[kvp.Key] = tempSum / (totalAvgList.Count);

                        tempSum = 0;
                        for (int i = 0; i < highAvgList.Count; i++)
                            tempSum += (double)highAvgList[i];

                        if (tempSum > 0)
                            SRClosureAvgHigh[kvp.Key] = tempSum / (highAvgList.Count);

                        tempSum = 0;
                        for (int i = 0; i < lowAvgList.Count; i++)
                            tempSum += (double)lowAvgList[i];

                        if (tempSum > 0)
                            SRClosureAvgLow[kvp.Key] = tempSum / (lowAvgList.Count);

                        tempSum = 0;
                        for (int i = 0; i < medAvgList.Count; i++)
                            tempSum += (double)medAvgList[i];

                        if (tempSum > 0)
                            SRClosureAvgMed[kvp.Key] = tempSum / (medAvgList.Count);
                    }
                }

                lastGeneratedDateRangeDict = new Dictionary<string, Dictionary<DateTime, string>>(); lastGeneratedDateRangeDict.Add(fromDate + "-" + toDate, dateRangeDict);
                Dictionary<String, Dictionary<DateTime, double>> allGeneratedDicts = new Dictionary<string, Dictionary<DateTime, double>>();


                allGeneratedDicts.Add("ClosureTotal", SRClosureAvgTotal);
                allGeneratedDicts.Add("ClosureHigh", SRClosureAvgHigh);
                allGeneratedDicts.Add("ClosureLow", SRClosureAvgLow);
                allGeneratedDicts.Add("ClosureMed", SRClosureAvgMed);

                Session[SessionFactory.ALL_DASHBOARD_INCM_SR_AVERAGECLOSTIME_GENERATED_DICTS] = allGeneratedDicts;
                Session[SessionFactory.ALL_DASHBOARD_INCM_SR_AVERAGECLOSTIME_DATE_RANGE] = lastGeneratedDateRangeDict;
                Session[SessionFactory.ALL_DASHBOARD_INCM_SR_AVERAGECLOSTIME_LAST_FREQ] = frequency;
                Session[SessionFactory.ALL_DASHBOARD_INCM_SR_AVERAGECLOSTIME_LAST_SELECTED_AGENT] = selectedAgent;

            }

            double[] SRClosureTotalArray = SRClosureAvgTotal.Values.ToArray();
            double[] SRClosureHighArray = SRClosureAvgHigh.Values.ToArray();
            double[] SRClosureLowArray = SRClosureAvgLow.Values.ToArray();
            double[] SRClosureMedArray = SRClosureAvgMed.Values.ToArray();
            String[] dateRangeDictArray = dateRangeDict.Values.ToArray();

            if (generateChart)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Date Range");
                dt.Columns.Add("Service Agent");
                dt.Columns.Add("High Sev Average (Hours)");
                dt.Columns.Add("Medium Sev Average (Hours)");
                dt.Columns.Add("Low Sev Average (Hours)");
                dt.Columns.Add("Combined Average (Hours)");

                for (int i = 0; i < dateRangeDict.Count; i++)
                {
                    dt.Rows.Add();
                    dt.Rows[i]["Date Range"] = dateRangeDictArray[i];
                    dt.Rows[i]["Service Agent"] = selectedAgent;
                    dt.Rows[i]["High Sev Average (Hours)"] = SRClosureHighArray[i];
                    dt.Rows[i]["Medium Sev Average (Hours)"] = SRClosureMedArray[i];
                    dt.Rows[i]["Low Sev Average (Hours)"] = SRClosureLowArray[i];
                    dt.Rows[i]["Combined Average (Hours)"] = SRClosureTotalArray[i];
                }

                Dictionary<String, DataTable> reportDict = (Dictionary<String, DataTable>)Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES];
                if (reportDict == null)
                    reportDict = new Dictionary<string, DataTable>();

                if (!reportDict.ContainsKey("incoming_SR_Avg_Closure"))
                    reportDict.Add("incoming_SR_Avg_Closure", dt);
                else
                    reportDict["incoming_SR_Avg_Closure"] = dt;

                Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES] = reportDict;

            }


            //Chart_SR_Arrival_Closure.Titles.Clear();
            //Chart_SR_Arrival_Closure.Titles.Add("SRs Arrival and Closure By Severity for Customers during the period");
            Chart_Incm_SR_Closure_Avg_Time.Titles[0].Font = new System.Drawing.Font("Book Antiqua", 10);
            Chart_Incm_SR_Closure_Avg_Time.Series["Series_SRs_Closure_Total"].Label = "#VALY{0;0;#}";
            Chart_Incm_SR_Closure_Avg_Time.Series["Series_Closure_High"].Label = "#VALY{0;0;#}";
            Chart_Incm_SR_Closure_Avg_Time.Series["Series_Closure_Medium"].Label = "#VALY{0;0;#}";
            Chart_Incm_SR_Closure_Avg_Time.Series["Series_Closure_Low"].Label = "#VALY{0;0;#}";

            Chart_Incm_SR_Closure_Avg_Time.Series["Series_SRs_Closure_Total"].Points.DataBindXY(dateRangeDictArray, SRClosureTotalArray);
            Chart_Incm_SR_Closure_Avg_Time.Series["Series_Closure_High"].Points.DataBindXY(dateRangeDictArray, SRClosureHighArray);
            Chart_Incm_SR_Closure_Avg_Time.Series["Series_Closure_Medium"].Points.DataBindXY(dateRangeDictArray, SRClosureMedArray);
            Chart_Incm_SR_Closure_Avg_Time.Series["Series_Closure_Low"].Points.DataBindXY(dateRangeDictArray, SRClosureLowArray);

        }




        /// <summary>
        /// This method generates incoming defects by amount for different accounts.
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        protected void generateDefectValByAccount(String fromDate, String toDate, Dictionary<String, DefectDetails> defectDict, Dictionary<String, String> custList)
        {
            if (fromDate == null || fromDate.Equals(""))
                fromDate = DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd");
            if (toDate == null || toDate.Equals(""))
                toDate = DateTime.Now.ToString("yyyy-MM-dd");

            bool custFilterCheck = (custList != null && custList.Count > 0) ? true : false;
            bool regenerateValues = false;


            Dictionary<String, String> contactDict = new Dictionary<String, String>();
            Dictionary<String, float> totalDefectAmount = new Dictionary<string, float>();

            Dictionary<String, Dictionary<String, String>> lastGeneratedContactDict = (Dictionary<String,Dictionary<String,String>>)
                Session[SessionFactory.ALL_DASHBOARD_INCM_DEFECT_DEFECTVALBYACCNT_CONTACT_DICT];

            if (lastGeneratedContactDict != null && lastGeneratedContactDict.Count > 0 && lastGeneratedContactDict.ContainsKey(fromDate + "," + toDate))
            {
                totalDefectAmount = ((Dictionary<String, Dictionary<String, float>>)Session[SessionFactory.ALL_DASHBOARD_INCM_DEFECT_DEFECTVALBYACCNT_DEFECT_AMNT])[fromDate + "," + toDate];

                if (custFilterCheck)
                {
                    Dictionary<String, float> totalDefectAmountTemp = new Dictionary<String, float>();
                    
                    foreach (KeyValuePair<String, String> kvp in custList)
                    {
                        if (totalDefectAmount.ContainsKey(kvp.Key))
                            totalDefectAmountTemp.Add(kvp.Key, totalDefectAmount[kvp.Key]);
                        else
                            regenerateValues = true;
                    }
                    totalDefectAmount = totalDefectAmountTemp;
                }
            }
            if (lastGeneratedContactDict != null && lastGeneratedContactDict.Count > 0 && lastGeneratedContactDict.ContainsKey(fromDate + "," + toDate) & !regenerateValues)
            {
                contactDict = lastGeneratedContactDict[fromDate + "," + toDate];
            }
            else
            {
                totalDefectAmount = new Dictionary<string, float>();

                int rowCount = 0;
                DataTable dt = new DataTable();
                dt.Columns.Add("Customer Name");
                dt.Columns.Add("Defect#");
                dt.Columns.Add("Description");
                dt.Columns.Add("Defect Date");
                dt.Columns.Add("Defect Amount");
                dt.Columns.Add("Defect Status");
                dt.Columns.Add("Defect Resolution Status");
                
                if (defectDict == null || defectDict.Count == 0)
                {
                    Dictionary<String, String> filterParams = new Dictionary<String, String>();
                    ActionLibrary.DefectActions._dispDefectDetails dspDefect = new DefectActions._dispDefectDetails();
                    if (fromDate != null && !fromDate.Equals(""))
                        filterParams.Add(dspDefect.FILTER_BY_SUBMIT_DATE_FROM, fromDate);
                    if (toDate != null && !toDate.Equals(""))
                        filterParams.Add(dspDefect.FILTER_BY_SUBMIT_DATE_TO, toDate);
                    defectDict = dspDefect.getAllDefectsFiltered("incoming", Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), filterParams);
                }

                foreach (KeyValuePair<String, DefectDetails> kvp in defectDict)
                {
                    DefectDetails defectObj = kvp.Value;

                    String customerEntId = defectObj.getCustomerId();
                    String custName = "";
                    if (defectObj.getCreationMode().Equals(BackEndObjects.DefectDetails.DEFECT_CREATION_MODE_MANUAL))
                    {
                        custName = Contacts.getContactDetailsforContactEntityDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), customerEntId).getContactName();
                        if (custName == null || custName.Equals(""))
                            custName = MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(customerEntId).getEntityName();
                    }
                    else
                        custName = MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(customerEntId).getEntityName();

                    bool considerCustomerEnt = custFilterCheck ? custList.ContainsKey(customerEntId) : true;

                    if (considerCustomerEnt)
                    {
                        if (!contactDict.ContainsKey(customerEntId))
                        {
                            Contacts contactObj = Contacts.
                                getContactDetailsforContactEntityDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), customerEntId);
                            if (contactObj == null || contactObj.getContactName() == null || contactObj.getContactName().Equals(""))
                                contactDict.Add(customerEntId, MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(customerEntId).getEntityName());
                            else
                                contactDict.Add(customerEntId, contactObj.getContactName());
                        }

                        if (!totalDefectAmount.ContainsKey(customerEntId))
                            totalDefectAmount.Add(customerEntId, defectObj.getTotalAmount());
                        else
                            totalDefectAmount[customerEntId] += defectObj.getTotalAmount();

                        dt.Rows.Add();

                        dt.Rows[rowCount]["Customer Name"] = contactDict[customerEntId];
                        dt.Rows[rowCount]["Defect#"] = defectObj.getDefectId();
                        dt.Rows[rowCount]["Description"] = defectObj.getDescription();
                        dt.Rows[rowCount]["Defect Date"] = defectObj.getDateCreated();
                        dt.Rows[rowCount]["Defect Amount"] = defectObj.getTotalAmount();
                        dt.Rows[rowCount]["Defect Status"] = defectObj.getDefectStat();
                        dt.Rows[rowCount]["Defect Resolution Status"] = defectObj.getResolStat();

                        rowCount++;
                    }
                }

                Dictionary<String, DataTable> reportDict = (Dictionary<String, DataTable>)Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES];

                if (reportDict == null)
                    reportDict = new Dictionary<string, DataTable>();

                if (!reportDict.ContainsKey("incoming_Defect_By_Accnt"))
                    reportDict.Add("incoming_Defect_By_Accnt", dt);
                else
                    reportDict["incoming_Defect_By_Accnt"] = dt;

                Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES] = reportDict;
                
                lastGeneratedContactDict=new Dictionary<string,Dictionary<string,string>>();lastGeneratedContactDict.Add(fromDate+","+toDate,contactDict);
                Dictionary<String,Dictionary<String,float>> lastGeneratedAmntDict=new Dictionary<string,Dictionary<string,float>>(); lastGeneratedAmntDict.Add(fromDate+","+toDate,totalDefectAmount);
               
                Session[SessionFactory.ALL_DASHBOARD_INCM_DEFECT_DEFECTVALBYACCNT_CONTACT_DICT] = lastGeneratedContactDict;
                Session[SessionFactory.ALL_DASHBOARD_INCM_DEFECT_DEFECTVALBYACCNT_DEFECT_AMNT] = lastGeneratedAmntDict;

            }

            //Now populate the List box with customer details
            foreach (KeyValuePair<String, String> kvp in contactDict)
            {
                ListItem ltExists = ListBox_Incoming_Defect_By_Account.Items.FindByValue(kvp.Key);
                if (ltExists == null || ltExists.Text.Equals(""))
                {
                    ltExists = new ListItem();
                    ltExists.Text = kvp.Value;
                    ltExists.Value = kvp.Key;

                    ListBox_Incoming_Defect_By_Account.Items.Add(ltExists);
                }
            }

            String[] customerArray = new String[contactDict.Count];
            float[] totalDefectAmountArray = new float[contactDict.Count];
            int counter = 0;
            foreach (KeyValuePair<String, float> kvp in totalDefectAmount)
            {
                customerArray[counter] = contactDict[kvp.Key];
                totalDefectAmountArray[counter] = totalDefectAmount[kvp.Key];
                counter++;
            }

            Chart_Incoming_Defect_By_Account.Titles.Clear();
            Chart_Incoming_Defect_By_Account.Titles.Add("Total Invoice Value of Defects for Customers during this period (defect submit date)");
            Chart_Incoming_Defect_By_Account.Titles[0].Font = new System.Drawing.Font("Book Antiqua", 10);
            Chart_Incoming_Defect_By_Account.Series["TotalDefects"].Label = "#VALY{0;0;#}";
            Chart_Incoming_Defect_By_Account.Series["TotalDefects"].Points.DataBindXY(customerArray, totalDefectAmountArray);

        }
        /// <summary>
        /// For incoming defects this method creates the chart showing defects arrival and closure by severity over a given time span
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="defectList"></param>
        /// <param name="userList"></param>
        /// <param name="frequency"></param>
        protected void generateDefectArrivalandClosure(String fromDate, String toDate, ArrayList defectList, Dictionary<String, String> userList,String frequency)
        {
            if (fromDate == null || fromDate.Equals(""))
                fromDate = DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd");
            if (toDate == null || toDate.Equals(""))
                toDate = DateTime.Now.ToString("yyyy-MM-dd");

            bool userFilterCheck = (userList != null && userList.Count > 0) ? true : false;
            bool generateChart = false;

            Dictionary<DateTime, float> totalDefectArrivalHigh = new Dictionary<DateTime, float>();
            Dictionary<DateTime, float> totalDefectArrivalLow = new Dictionary<DateTime, float>();
            Dictionary<DateTime, float> totalDefectArrivalMed = new Dictionary<DateTime, float>();
            Dictionary<DateTime, float> defectClosureHigh = new Dictionary<DateTime, float>();
            Dictionary<DateTime, float> defectClosureLow = new Dictionary<DateTime, float>();
            Dictionary<DateTime, float> defectClosureMed = new Dictionary<DateTime, float>();
            Dictionary<DateTime, String> dateRangeDict = new Dictionary<DateTime, string>();

            Dictionary<String, Dictionary<DateTime, String>> lastGeneratedDateRangeDict = (Dictionary<String, Dictionary<DateTime, String>>)Session[SessionFactory.ALL_DASHBOARD_INCM_DEFECT_ARRVLCLOS_DATE_RANGE];
            String lastRequestedFreq = (Session[SessionFactory.ALL_DASHBOARD_INCM_DEFECT_ARRVLCLOS_LAST_FREQ] != null ? Session[SessionFactory.ALL_DASHBOARD_INCM_DEFECT_ARRVLCLOS_LAST_FREQ].ToString() : "");

            if (lastGeneratedDateRangeDict != null && lastGeneratedDateRangeDict.Count > 0 && lastGeneratedDateRangeDict.ContainsKey(fromDate + "-" + toDate) && frequency.Equals(lastRequestedFreq))
            {
                Dictionary<String, Dictionary<DateTime, float>> allGeneratedDicts = (Dictionary<String, Dictionary<DateTime, float>>)
                    Session[SessionFactory.ALL_DASHBOARD_INCM_DEFECT_ARRVLCLOS_GENERATED_DICTS];

                totalDefectArrivalHigh = allGeneratedDicts["ArrivalHigh"];
                totalDefectArrivalLow = allGeneratedDicts["ArrivalLow"];
                totalDefectArrivalMed = allGeneratedDicts["ArrivalMed"];
                defectClosureHigh = allGeneratedDicts["ClosureHigh"];
                defectClosureLow = allGeneratedDicts["ClosureLow"];
                defectClosureMed = allGeneratedDicts["ClosureMed"];

                dateRangeDict = lastGeneratedDateRangeDict[fromDate + "-" + toDate];
            }
            else
            {
                generateChart = true;

                if (defectList == null || defectList.Count == 0)
                {
                    Dictionary<String, String> filterParams = new Dictionary<String, String>();
                    ActionLibrary.DefectActions._dispDefectDetails dspDefect = new DefectActions._dispDefectDetails();
                    if (fromDate != null && !fromDate.Equals(""))
                        filterParams.Add(dspDefect.FILTER_BY_SUBMIT_DATE_FROM, fromDate);
                    if (toDate != null && !toDate.Equals(""))
                        filterParams.Add(dspDefect.FILTER_BY_SUBMIT_DATE_TO, toDate);

                    defectList = dspDefect.getAllDefectsFilteredORDERBYCreateDate("incoming", Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), filterParams);
                    if (defectList != null && defectList.Count > 0)
                    {
                        String lowerDate = ((DefectDetails)defectList[0]).getDateCreated().Substring(0, ((DefectDetails)defectList[0]).getDateCreated().IndexOf(" "));
                        String higherDate = ((DefectDetails)defectList[defectList.Count - 1]).getDateCreated().Substring(0, ((DefectDetails)defectList[defectList.Count - 1]).getDateCreated().IndexOf(" "));

                        dateRangeDict = getDateRange(lowerDate, higherDate, frequency);
                        List<DateTime> dataRangeList = dateRangeDict.Keys.ToList();
                        dataRangeList.Sort();
                        //Make sure all the dictionaries have the same key set
                        foreach (KeyValuePair<DateTime, String> kvp in dateRangeDict)
                        {
                            if (!totalDefectArrivalHigh.ContainsKey(kvp.Key))
                                totalDefectArrivalHigh.Add(kvp.Key, 0);

                            if (!totalDefectArrivalLow.ContainsKey(kvp.Key))
                                totalDefectArrivalLow.Add(kvp.Key, 0);

                            if (!totalDefectArrivalMed.ContainsKey(kvp.Key))
                                totalDefectArrivalMed.Add(kvp.Key, 0);

                            if (!defectClosureHigh.ContainsKey(kvp.Key))
                                defectClosureHigh.Add(kvp.Key, 0);

                            if (!defectClosureLow.ContainsKey(kvp.Key))
                                defectClosureLow.Add(kvp.Key, 0);

                            if (!defectClosureMed.ContainsKey(kvp.Key))
                                defectClosureMed.Add(kvp.Key, 0);
                        }

                        for (int i = 0; i < defectList.Count; i++)
                        {
                            DefectDetails defObj = (DefectDetails)defectList[i];
                            /*String closeDate = defObj.getCloseDate() != null && !defObj.getCloseDate().Equals("")  && !(defObj.getCloseDate().IndexOf("1/1/1900")>=0)?
                                defObj.getCloseDate().Replace(" ", "  ").Substring(0, 9) : "";
                            String createDate = defObj.getDateCreated().Replace(" ", "  ").Substring(0, 9);*/
                            String closeDate = defObj.getCloseDate() != null && !defObj.getCloseDate().Equals("") && !(defObj.getCloseDate().IndexOf("1/1/1900") >= 0) ?
    (defObj.getCloseDate().IndexOf(" ") >= 0 ? defObj.getCloseDate().Substring(0, defObj.getCloseDate().IndexOf(" ")) : defObj.getCloseDate()) : "";
                            String createDate = defObj.getDateCreated().IndexOf(" ") >= 0 ? defObj.getDateCreated().Substring(0, defObj.getDateCreated().IndexOf(" ")) : defObj.getDateCreated();

                            int index = dataRangeList.BinarySearch(Convert.ToDateTime(createDate));
                            if (index >= 0)
                            {
                                //Exact Match Found
                                if (defObj.getSeverity().Equals("High", StringComparison.InvariantCultureIgnoreCase))
                                    totalDefectArrivalHigh[dataRangeList[index]] += 1;
                                if (defObj.getSeverity().Equals("Medium", StringComparison.InvariantCultureIgnoreCase))
                                    totalDefectArrivalMed[dataRangeList[index]] += 1;
                                if (defObj.getSeverity().Equals("Low", StringComparison.InvariantCultureIgnoreCase))
                                    totalDefectArrivalLow[dataRangeList[index]] += 1;
                            }
                            else
                            {
                                //Nearest Greater Value Found
                                index = ~index;
                                if (defObj.getSeverity().Equals("High", StringComparison.InvariantCultureIgnoreCase))
                                    totalDefectArrivalHigh[dataRangeList[index - 1]] += 1;
                                if (defObj.getSeverity().Equals("Medium", StringComparison.InvariantCultureIgnoreCase))
                                    totalDefectArrivalMed[dataRangeList[index - 1]] += 1;
                                if (defObj.getSeverity().Equals("Low", StringComparison.InvariantCultureIgnoreCase))
                                    totalDefectArrivalLow[dataRangeList[index - 1]] += 1;
                            }

                            if (closeDate != null && !closeDate.Equals(""))
                            {
                                index = dataRangeList.BinarySearch(Convert.ToDateTime(closeDate));
                                if (index >= 0)
                                {
                                    //Exact Match Found
                                    if (defObj.getSeverity().Equals("High", StringComparison.InvariantCultureIgnoreCase))
                                        defectClosureHigh[dataRangeList[index]] += 1;
                                    if (defObj.getSeverity().Equals("Medium", StringComparison.InvariantCultureIgnoreCase))
                                        defectClosureMed[dataRangeList[index]] += 1;
                                    if (defObj.getSeverity().Equals("Low", StringComparison.InvariantCultureIgnoreCase))
                                        defectClosureLow[dataRangeList[index]] += 1;
                                }
                                else
                                {
                                    //Nearest Greater Value Found
                                    index = ~index;
                                    if (defObj.getSeverity().Equals("High", StringComparison.InvariantCultureIgnoreCase))
                                        defectClosureHigh[dataRangeList[index - 1]] += 1;
                                    if (defObj.getSeverity().Equals("Medium", StringComparison.InvariantCultureIgnoreCase))
                                        defectClosureMed[dataRangeList[index - 1]] += 1;
                                    if (defObj.getSeverity().Equals("Low", StringComparison.InvariantCultureIgnoreCase))
                                        defectClosureLow[dataRangeList[index - 1]] += 1;
                                }
                            }
                        }
                    }

                }

                lastGeneratedDateRangeDict = new Dictionary<string, Dictionary<DateTime, string>>(); lastGeneratedDateRangeDict.Add(fromDate + "-" + toDate, dateRangeDict);
                Dictionary<String, Dictionary<DateTime, float>> allGeneratedDicts = new Dictionary<string, Dictionary<DateTime, float>>();
                allGeneratedDicts.Add("ArrivalHigh", totalDefectArrivalHigh);
                allGeneratedDicts.Add("ArrivalLow", totalDefectArrivalLow);
                allGeneratedDicts.Add("ArrivalMed", totalDefectArrivalMed);
                allGeneratedDicts.Add("ClosureHigh", defectClosureHigh);
                allGeneratedDicts.Add("ClosureLow", defectClosureLow);
                allGeneratedDicts.Add("ClosureMed", defectClosureMed);

                Session[SessionFactory.ALL_DASHBOARD_INCM_DEFECT_ARRVLCLOS_GENERATED_DICTS] = allGeneratedDicts;
                Session[SessionFactory.ALL_DASHBOARD_INCM_DEFECT_ARRVLCLOS_DATE_RANGE]=lastGeneratedDateRangeDict;
                Session[SessionFactory.ALL_DASHBOARD_INCM_DEFECT_ARRVLCLOS_LAST_FREQ] = frequency;

            }
           
            float[] totalDefectArrivalHighArray = totalDefectArrivalHigh.Values.ToArray();
            float[] totalDefectArrivalLowArray = totalDefectArrivalLow.Values.ToArray();
            float[] totalDefectArrivalMedArray = totalDefectArrivalMed.Values.ToArray();
            float[] defectClosureHighArray = defectClosureHigh.Values.ToArray();
            float[] defectClosureLowArray = defectClosureLow.Values.ToArray();
            float[] defectClosureMedArray = defectClosureMed.Values.ToArray();
            String[] dateRangeDictArray = dateRangeDict.Values.ToArray();

            if (generateChart)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Date Range");
                dt.Columns.Add("No of High Sev Created");
                dt.Columns.Add("No of High Sev Closed");
                dt.Columns.Add("No of Medium Sev Created");
                dt.Columns.Add("No of Medium Sev Closed");
                dt.Columns.Add("No of Low Sev Created");
                dt.Columns.Add("No of Low Sev Closed");

                for (int i = 0; i < dateRangeDictArray.Length; i++)
                {
                    dt.Rows.Add();
                    dt.Rows[i]["Date Range"] = dateRangeDictArray[i];
                    dt.Rows[i]["No of High Sev Created"] = totalDefectArrivalHighArray[i];
                    dt.Rows[i]["No of High Sev Closed"] = defectClosureHighArray[i];
                    dt.Rows[i]["No of Medium Sev Created"] = totalDefectArrivalMedArray[i];
                    dt.Rows[i]["No of Medium Sev Closed"] = defectClosureMedArray[i];
                    dt.Rows[i]["No of Low Sev Created"] = totalDefectArrivalLowArray[i];
                    dt.Rows[i]["No of Low Sev Closed"] = defectClosureLowArray[i];

                }

                Dictionary<String, DataTable> reportDict = (Dictionary<String, DataTable>)Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES];
                if (reportDict == null)
                    reportDict = new Dictionary<string, DataTable>();

                if (!reportDict.ContainsKey("incoming_Defect_Arrival_Closure"))
                    reportDict.Add("incoming_Defect_Arrival_Closure", dt);
                else
                    reportDict["incoming_Defect_Arrival_Closure"] = dt;

                Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES] = reportDict;
            }

            //Chart_Defect_Arrival_Closure.Titles.Clear();
            //Chart_Defect_Arrival_Closure.Titles.Add("Defects Arrival and Closure By Severity for Customers during the period");
            Chart_Defect_Arrival_Closure.Titles[0].Font = new System.Drawing.Font("Book Antiqua", 10);
            Chart_Defect_Arrival_Closure.Series["Series_Defects_High"].Label = "#VALY{0;0;#}";
            Chart_Defect_Arrival_Closure.Series["Series_Defects_Medium"].Label = "#VALY{0;0;#}";
            Chart_Defect_Arrival_Closure.Series["Series_Defects_Low"].Label = "#VALY{0;0;#}";
            Chart_Defect_Arrival_Closure.Series["Series_Closure_High"].Label = "#VALY{0;0;#}";
            Chart_Defect_Arrival_Closure.Series["Series_Closure_Medium"].Label = "#VALY{0;0;#}";
            Chart_Defect_Arrival_Closure.Series["Series_Closure_Low"].Label = "#VALY{0;0;#}";

            Chart_Defect_Arrival_Closure.Series["Series_Defects_High"].Points.DataBindXY(dateRangeDictArray, totalDefectArrivalHighArray);
            Chart_Defect_Arrival_Closure.Series["Series_Defects_Medium"].Points.DataBindXY(dateRangeDictArray, totalDefectArrivalMedArray);
            Chart_Defect_Arrival_Closure.Series["Series_Defects_Low"].Points.DataBindXY(dateRangeDictArray, totalDefectArrivalLowArray);
            Chart_Defect_Arrival_Closure.Series["Series_Closure_High"].Points.DataBindXY(dateRangeDictArray, defectClosureHighArray);
            Chart_Defect_Arrival_Closure.Series["Series_Closure_Medium"].Points.DataBindXY(dateRangeDictArray, defectClosureMedArray);
            Chart_Defect_Arrival_Closure.Series["Series_Closure_Low"].Points.DataBindXY(dateRangeDictArray, defectClosureLowArray);

        }

        protected void generateAvgDefectClosureTime(String fromDate, String toDate, ArrayList defectList, String selectedAgent, String frequency)
        {
            if (fromDate == null || fromDate.Equals(""))
                fromDate = DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd");
            if (toDate == null || toDate.Equals(""))
                toDate = DateTime.Now.ToString("yyyy-MM-dd");

            bool agentFilterCheck = (selectedAgent != null && !selectedAgent.Equals("All")) ? true : false;
            bool generateChart = false;

            Dictionary<DateTime, double> defectClosureAvgTotal = new Dictionary<DateTime, double>();
            Dictionary<DateTime, double> defectClosureAvgHigh = new Dictionary<DateTime, double>();
            Dictionary<DateTime, double> defectClosureAvgLow = new Dictionary<DateTime, double>();
            Dictionary<DateTime, double> defectClosureAvgMed = new Dictionary<DateTime, double>();
            Dictionary<DateTime, String> dateRangeDict = new Dictionary<DateTime, string>();

            Dictionary<String, Dictionary<DateTime, String>> lastGeneratedDateRangeDict = (Dictionary<String, Dictionary<DateTime, String>>)
                Session[SessionFactory.ALL_DASHBOARD_INCM_DEFECT_AVERAGECLOSTIME_DATE_RANGE];

            String lastRequestedFreq = (Session[SessionFactory.ALL_DASHBOARD_INCM_DEFECT_AVERAGECLOSTIME_LAST_FREQ] != null ?
                Session[SessionFactory.ALL_DASHBOARD_INCM_DEFECT_AVERAGECLOSTIME_LAST_FREQ].ToString() : "");

            String lastSelectedAgent=(Session[SessionFactory.ALL_DASHBOARD_INCM_DEFECT_AVERAGECLOSTIME_LAST_SELECTED_AGENT]!=null ?
                Session[SessionFactory.ALL_DASHBOARD_INCM_DEFECT_AVERAGECLOSTIME_LAST_SELECTED_AGENT].ToString():"");

            if (lastGeneratedDateRangeDict != null && lastGeneratedDateRangeDict.Count > 0 && lastGeneratedDateRangeDict.ContainsKey(fromDate + "-" + toDate) && frequency.Equals(lastRequestedFreq) && lastSelectedAgent.Equals(selectedAgent))
            {
                Dictionary<String, Dictionary<DateTime, double>> allGeneratedDicts = (Dictionary<String, Dictionary<DateTime, double>>)
                    Session[SessionFactory.ALL_DASHBOARD_INCM_DEFECT_AVERAGECLOSTIME_GENERATED_DICTS];

                defectClosureAvgTotal = allGeneratedDicts["ClosureTotal"];
                defectClosureAvgHigh = allGeneratedDicts["ClosureHigh"];
                defectClosureAvgLow = allGeneratedDicts["ClosureLow"];
                defectClosureAvgMed = allGeneratedDicts["ClosureMed"];

                dateRangeDict = lastGeneratedDateRangeDict[fromDate + "-" + toDate];
            }
            else
            {
                generateChart = true;

                if (defectList == null || defectList.Count == 0)
                {
                    Dictionary<String, String> filterParams = new Dictionary<String, String>();
                    ActionLibrary.DefectActions._dispDefectDetails dspDefect = new DefectActions._dispDefectDetails();
                    if (fromDate != null && !fromDate.Equals(""))
                        filterParams.Add(dspDefect.FILTER_BY_SUBMIT_DATE_FROM, fromDate);
                    if (toDate != null && !toDate.Equals(""))
                        filterParams.Add(dspDefect.FILTER_BY_SUBMIT_DATE_TO, toDate);

                    defectList = dspDefect.getAllDefectsFilteredORDERBYCreateDate("incoming", Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), filterParams);
                    
                    Dictionary<DateTime, ArrayList> defectClosureAvgHighTemp = new Dictionary<DateTime, ArrayList>();
                    Dictionary<DateTime, ArrayList> defectClosureAvgLowTemp = new Dictionary<DateTime, ArrayList>();
                    Dictionary<DateTime, ArrayList> defectClosureAvgMedTemp = new Dictionary<DateTime, ArrayList>();
                    Dictionary<DateTime, ArrayList> defectClosureAvgTotalTemp = new Dictionary<DateTime, ArrayList>();

                    if (defectList != null && defectList.Count > 0)
                    {
                        String lowerDate = ((DefectDetails)defectList[0]).getDateCreated().Substring(0, ((DefectDetails)defectList[0]).getDateCreated().IndexOf(" "));
                        String higherDate = ((DefectDetails)defectList[defectList.Count - 1]).getDateCreated().Substring(0, ((DefectDetails)defectList[defectList.Count - 1]).getDateCreated().IndexOf(" "));

                        dateRangeDict = getDateRange(lowerDate, higherDate, frequency);
                        List<DateTime> dataRangeList = dateRangeDict.Keys.ToList();
                        dataRangeList.Sort();
                        //Make sure all the dictionaries have the same key set
                        foreach (KeyValuePair<DateTime, String> kvp in dateRangeDict)
                        {
                            if (!defectClosureAvgTotal.ContainsKey(kvp.Key))
                            {defectClosureAvgTotal.Add(kvp.Key, 0);defectClosureAvgTotalTemp.Add(kvp.Key,new ArrayList());}

                            if (!defectClosureAvgHigh.ContainsKey(kvp.Key))
                            {defectClosureAvgHigh.Add(kvp.Key, 0);defectClosureAvgHighTemp.Add(kvp.Key,new ArrayList());}

                            if (!defectClosureAvgLow.ContainsKey(kvp.Key))
                            {defectClosureAvgLow.Add(kvp.Key, 0);defectClosureAvgLowTemp.Add(kvp.Key,new ArrayList());}

                            if (!defectClosureAvgMed.ContainsKey(kvp.Key))
                            {defectClosureAvgMed.Add(kvp.Key, 0);defectClosureAvgMedTemp.Add(kvp.Key,new ArrayList());}
                        }

                        int index = 0;
                        bool considerRecord=true;

                        for (int i = 0; i < defectList.Count; i++)
                        {
                            DefectDetails defObj = (DefectDetails)defectList[i];
                            considerRecord = true;
                            if (agentFilterCheck)
                            {
                                if (selectedAgent.Equals(defObj.getAssignedToUser()))
                                    considerRecord = true;
                                else
                                    considerRecord = false;
                            }

                            if (considerRecord)
                            {
                                /*String closeDate = defObj.getCloseDate() != null && !defObj.getCloseDate().Equals("") && !(defObj.getCloseDate().IndexOf("1/1/1900") >= 0) ?
                                    defObj.getCloseDate().Replace(" ", "  ").Substring(0, 9) : "";
                                String createDate = defObj.getDateCreated().Replace(" ", "  ").Substring(0, 9);*/
                                String closeDate = defObj.getCloseDate() != null && !defObj.getCloseDate().Equals("") && !(defObj.getCloseDate().IndexOf("1/1/1900") >= 0) ?
    (defObj.getCloseDate().IndexOf(" ") >= 0 ? defObj.getCloseDate().Substring(0, defObj.getCloseDate().IndexOf(" ")) : defObj.getCloseDate()) : "";
                                String createDate = defObj.getDateCreated().IndexOf(" ") >= 0 ? defObj.getDateCreated().Substring(0, defObj.getDateCreated().IndexOf(" ")) : defObj.getDateCreated();

                                if (closeDate != null && !closeDate.Equals(""))
                                {
                                    index = dataRangeList.BinarySearch(Convert.ToDateTime(createDate));
                                    double timeTakenInHours = Convert.ToDateTime(defObj.getCloseDate()).Subtract(Convert.ToDateTime(defObj.getDateCreated())).TotalHours;
                                    timeTakenInHours = Math.Round(timeTakenInHours, 2);

                                    if (index >= 0)
                                    {
                                        //Exact Match Found
                                        if (defObj.getSeverity().Equals("High", StringComparison.InvariantCultureIgnoreCase))
                                            defectClosureAvgHighTemp[dataRangeList[index]].Add(timeTakenInHours);
                                        if (defObj.getSeverity().Equals("Medium", StringComparison.InvariantCultureIgnoreCase))
                                            defectClosureAvgMedTemp[dataRangeList[index]].Add(timeTakenInHours);
                                        if (defObj.getSeverity().Equals("Low", StringComparison.InvariantCultureIgnoreCase))
                                            defectClosureAvgLowTemp[dataRangeList[index]].Add(timeTakenInHours);

                                        defectClosureAvgTotalTemp[dataRangeList[index]].Add(timeTakenInHours);
                                    }
                                    else
                                    {
                                        //Nearest Greater Value Found
                                        index = ~index;
                                        if (defObj.getSeverity().Equals("High", StringComparison.InvariantCultureIgnoreCase))
                                            defectClosureAvgHighTemp[dataRangeList[index - 1]].Add(timeTakenInHours);
                                        if (defObj.getSeverity().Equals("Medium", StringComparison.InvariantCultureIgnoreCase))
                                            defectClosureAvgMedTemp[dataRangeList[index - 1]].Add(timeTakenInHours);
                                        if (defObj.getSeverity().Equals("Low", StringComparison.InvariantCultureIgnoreCase))
                                            defectClosureAvgLowTemp[dataRangeList[index - 1]].Add(timeTakenInHours);

                                        defectClosureAvgTotalTemp[dataRangeList[index - 1]].Add(timeTakenInHours);
                                    }
                                }

                            }
                        }
                    }

                    foreach (KeyValuePair<DateTime, ArrayList> kvp in defectClosureAvgTotalTemp)
                    {
                        ArrayList totalAvgList = kvp.Value;
                        ArrayList medAvgList = defectClosureAvgMedTemp[kvp.Key];
                        ArrayList lowAvgList = defectClosureAvgLowTemp[kvp.Key];
                        ArrayList highAvgList = defectClosureAvgHighTemp[kvp.Key];

                        double tempSum=0;
                        for (int i = 0; i < totalAvgList.Count; i++)
                            tempSum += (double)totalAvgList[i];

                        if(tempSum>0)
                        defectClosureAvgTotal[kvp.Key]=tempSum / (totalAvgList.Count);
                        
                        tempSum=0;
                        for (int i = 0; i < highAvgList.Count; i++)
                            tempSum += (double)highAvgList[i];

                        if (tempSum > 0)
                            defectClosureAvgHigh[kvp.Key] = tempSum / (highAvgList.Count);

                        tempSum = 0;
                        for (int i = 0; i < lowAvgList.Count; i++)
                            tempSum += (double)lowAvgList[i];

                        if (tempSum > 0)
                            defectClosureAvgLow[kvp.Key] = tempSum / (lowAvgList.Count);

                        tempSum = 0;
                        for (int i = 0; i < medAvgList.Count; i++)
                            tempSum += (double)medAvgList[i];

                        if (tempSum > 0)
                            defectClosureAvgMed[kvp.Key] = tempSum / (medAvgList.Count);
                    }
                }

                lastGeneratedDateRangeDict = new Dictionary<string, Dictionary<DateTime, string>>(); lastGeneratedDateRangeDict.Add(fromDate + "-" + toDate, dateRangeDict);
                Dictionary<String, Dictionary<DateTime, double>> allGeneratedDicts = new Dictionary<string, Dictionary<DateTime, double>>();
                              

                allGeneratedDicts.Add("ClosureTotal", defectClosureAvgTotal);
                allGeneratedDicts.Add("ClosureHigh", defectClosureAvgHigh);
                allGeneratedDicts.Add("ClosureLow", defectClosureAvgLow);
                allGeneratedDicts.Add("ClosureMed", defectClosureAvgMed);

                Session[SessionFactory.ALL_DASHBOARD_INCM_DEFECT_AVERAGECLOSTIME_GENERATED_DICTS] = allGeneratedDicts;
                Session[SessionFactory.ALL_DASHBOARD_INCM_DEFECT_AVERAGECLOSTIME_DATE_RANGE] = lastGeneratedDateRangeDict;
                Session[SessionFactory.ALL_DASHBOARD_INCM_DEFECT_AVERAGECLOSTIME_LAST_FREQ] = frequency;
                Session[SessionFactory.ALL_DASHBOARD_INCM_DEFECT_AVERAGECLOSTIME_LAST_SELECTED_AGENT] = selectedAgent;

            }

            double[] defectClosureTotalArray = defectClosureAvgTotal.Values.ToArray();
            double[] defectClosureHighArray = defectClosureAvgHigh.Values.ToArray();
            double[] defectClosureLowArray = defectClosureAvgLow.Values.ToArray();
            double[] defectClosureMedArray = defectClosureAvgMed.Values.ToArray();
            String[] dateRangeDictArray = dateRangeDict.Values.ToArray();

            if (generateChart)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Date Range");
                dt.Columns.Add("Service Agent");
                dt.Columns.Add("High Sev Average (Hours)");
                dt.Columns.Add("Medium Sev Average (Hours)");
                dt.Columns.Add("Low Sev Average (Hours)");
                dt.Columns.Add("Combined Average (Hours)");

                for (int i = 0; i < dateRangeDict.Count; i++)
                {
                    dt.Rows.Add();
                    dt.Rows[i]["Date Range"] = dateRangeDictArray[i];
                    dt.Rows[i]["Service Agent"] = selectedAgent;
                    dt.Rows[i]["High Sev Average (Hours)"] = defectClosureHighArray[i];
                    dt.Rows[i]["Medium Sev Average (Hours)"] = defectClosureMedArray[i];
                    dt.Rows[i]["Low Sev Average (Hours)"] = defectClosureLowArray[i];
                    dt.Rows[i]["Combined Average (Hours)"] = defectClosureTotalArray[i];
                }

                Dictionary<String, DataTable> reportDict = (Dictionary<String, DataTable>)Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES];
                if (reportDict == null)
                    reportDict = new Dictionary<string, DataTable>();

                if (!reportDict.ContainsKey("incoming_Defect_Avg_Closure"))
                    reportDict.Add("incoming_Defect_Avg_Closure", dt);
                else
                    reportDict["incoming_Defect_Avg_Closure"] = dt;

                Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES] = reportDict;

            }


            //Chart_Defect_Arrival_Closure.Titles.Clear();
            //Chart_Defect_Arrival_Closure.Titles.Add("Defects Arrival and Closure By Severity for Customers during the period");
            Chart_Incm_Defect_Closure_Avg_Time.Titles[0].Font = new System.Drawing.Font("Book Antiqua", 10);
            Chart_Incm_Defect_Closure_Avg_Time.Series["Series_Defects_Closure_Total"].Label = "#VALY{0;0;#}";
            Chart_Incm_Defect_Closure_Avg_Time.Series["Series_Closure_High"].Label = "#VALY{0;0;#}";
            Chart_Incm_Defect_Closure_Avg_Time.Series["Series_Closure_Medium"].Label = "#VALY{0;0;#}";
            Chart_Incm_Defect_Closure_Avg_Time.Series["Series_Closure_Low"].Label = "#VALY{0;0;#}";

            Chart_Incm_Defect_Closure_Avg_Time.Series["Series_Defects_Closure_Total"].Points.DataBindXY(dateRangeDictArray, defectClosureTotalArray);
            Chart_Incm_Defect_Closure_Avg_Time.Series["Series_Closure_High"].Points.DataBindXY(dateRangeDictArray, defectClosureHighArray);
            Chart_Incm_Defect_Closure_Avg_Time.Series["Series_Closure_Medium"].Points.DataBindXY(dateRangeDictArray, defectClosureMedArray);
            Chart_Incm_Defect_Closure_Avg_Time.Series["Series_Closure_Low"].Points.DataBindXY(dateRangeDictArray, defectClosureLowArray);

        }

        protected Dictionary<DateTime, String> getDateRange(String lowerDate,String higherDate, String frequency)
        {
            DateTime startDate, endDate;
            startDate = Convert.ToDateTime(lowerDate);
            endDate = Convert.ToDateTime(higherDate);

            Dictionary<DateTime, String> returnDict = new Dictionary<DateTime, String>();

            while (frequency.Equals("weekly")?(startDate.AddDays(7)<=endDate):(startDate.AddMonths(1)<=endDate))
            {                
                if (frequency.Equals("weekly"))
                {                    
                    returnDict.Add(startDate, startDate.ToShortDateString() + "-" + startDate.AddDays(7).ToShortDateString());
                    startDate = startDate.AddDays(8);
                }
                if (frequency.Equals("monthly"))
                {
                    returnDict.Add(startDate, startDate.ToShortDateString() + "-" + startDate.AddMonths(1).ToShortDateString());
                    startDate = startDate.AddMonths(1).AddDays(1);
                }
            }

            returnDict.Add(startDate, startDate.ToShortDateString()+"-"+endDate.ToShortDateString());

            return returnDict;
        }

        protected void generateDefectNoByAccount(String fromDate, String toDate, Dictionary<String, DefectDetails> defectDict, Dictionary<String, String> custList,String defectType)
        {
            if (fromDate == null || fromDate.Equals(""))
                fromDate = DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd");
            if (toDate == null || toDate.Equals(""))
                toDate = DateTime.Now.ToString("yyyy-MM-dd");

            bool custFilterCheck = (custList != null && custList.Count > 0) ? true : false;
            bool regenerateValues = false;

            Dictionary<String, String> contactDict = new Dictionary<String, String>();
            Dictionary<String, float> totalDefectNo = new Dictionary<string, float>();
            Dictionary<String, float> highDefect = new Dictionary<string, float>();
            Dictionary<String, float> mediumDefect = new Dictionary<string, float>();
            Dictionary<String, float> lowDefect = new Dictionary<string, float>();

            Dictionary<String, Dictionary<String, String>> lastGeneratedContactDict = ((Dictionary<String, Dictionary<String, String>>)
                Session[SessionFactory.ALL_DASHBOARD_INCM_DEFECT_DEFECTNOBYACCNT_CONTACT_DICT]);

            if (lastGeneratedContactDict != null && lastGeneratedContactDict.Count > 0 && lastGeneratedContactDict.ContainsKey(fromDate + "," + toDate))
            {
                totalDefectNo=((Dictionary<String,Dictionary<String,float>>)Session[SessionFactory.ALL_DASHBOARD_INCM_DEFECT_DEFECTNOBYACCNT_TOTAL_DEFECT])[fromDate+","+toDate];

            highDefect=((Dictionary<String,Dictionary<String,float>>)Session[SessionFactory.ALL_DASHBOARD_INCM_DEFECT_DEFECTNOBYACCNT_HIGH_DEFECT])[fromDate+","+toDate];
        mediumDefect=((Dictionary<String,Dictionary<String,float>>)Session[SessionFactory.ALL_DASHBOARD_INCM_DEFECT_DEFECTNOBYACCNT_MEDM_DEFECT])[fromDate+","+toDate];
        lowDefect=((Dictionary<String,Dictionary<String,float>>)Session[SessionFactory.ALL_DASHBOARD_INCM_DEFECT_DEFECTNOBYACCNT_LOW_DEFECT])[fromDate+","+toDate];

            if(custFilterCheck)
            {
                Dictionary<String, float> totalDefectNoTemp = new Dictionary<String, float>();
                Dictionary<String, float> highDefectTemp = new Dictionary<String, float>();
                Dictionary<String, float> lowDefectTemp = new Dictionary<String, float>();
                Dictionary<String, float> mediumDefectTemp = new Dictionary<String, float>();

                     foreach (KeyValuePair<String, String> kvp in custList)
                    {
                        if (totalDefectNo.ContainsKey(kvp.Key))
                            totalDefectNoTemp.Add(kvp.Key, totalDefectNo[kvp.Key]);
                        else
                            regenerateValues = true;

                        if (highDefect.ContainsKey(kvp.Key))
                            highDefectTemp.Add(kvp.Key, highDefect[kvp.Key]);
                        else
                            regenerateValues = true;

                        if (lowDefect.ContainsKey(kvp.Key))
                            lowDefectTemp.Add(kvp.Key, lowDefect[kvp.Key]);
                        else
                            regenerateValues = true;

                        if (mediumDefect.ContainsKey(kvp.Key))
                            mediumDefectTemp.Add(kvp.Key, mediumDefect[kvp.Key]);
                        else
                            regenerateValues = true;
                    }

                highDefect=highDefectTemp;
                lowDefect=lowDefectTemp;
                mediumDefect=mediumDefectTemp;
                totalDefectNo=totalDefectNoTemp;
            }

            String lastSelectedDefectType = Session[SessionFactory.ALL_DASHBOARD_INCM_DEFECT_DEFECTNOBYACCNT_LAST_DEFECT_TYPE] != null? 
                Session[SessionFactory.ALL_DASHBOARD_INCM_DEFECT_DEFECTNOBYACCNT_LAST_DEFECT_TYPE].ToString() : "";

            if (!lastSelectedDefectType.Equals(defectType))
                regenerateValues = true;

            }

            if (lastGeneratedContactDict != null && lastGeneratedContactDict.Count > 0 && lastGeneratedContactDict.ContainsKey(fromDate + "," + toDate) && !regenerateValues)
            {
                contactDict = lastGeneratedContactDict[fromDate + "," + toDate];
            }
            else
            {
                highDefect=new Dictionary<string, float>();
                lowDefect = new Dictionary<string, float>();
                mediumDefect = new Dictionary<string, float>();
                totalDefectNo = new Dictionary<string, float>();


                int rowCount = 0;
                DataTable dt = new DataTable();
                dt.Columns.Add("Customer Name");
                dt.Columns.Add("Severity");
                dt.Columns.Add("Defect#");
                dt.Columns.Add("Description");
                dt.Columns.Add("Defect Date");
                dt.Columns.Add("Close Date");
                dt.Columns.Add("Defect Amount");
                dt.Columns.Add("Defect Status");
                dt.Columns.Add("Defect Resolution Status");
                dt.Columns.Add("Assigned To");

                if (defectDict == null || defectDict.Count == 0)
                {
                    Dictionary<String, String> filterParams = new Dictionary<String, String>();
                    ActionLibrary.DefectActions._dispDefectDetails dspDefect = new DefectActions._dispDefectDetails();
                    if (fromDate != null && !fromDate.Equals(""))
                        filterParams.Add(dspDefect.FILTER_BY_SUBMIT_DATE_FROM, fromDate);
                    if (toDate != null && !toDate.Equals(""))
                        filterParams.Add(dspDefect.FILTER_BY_SUBMIT_DATE_TO, toDate);

                    defectDict = dspDefect.getAllDefectsFiltered("incoming", Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), filterParams);
                }

                bool considerRecordByDefectType = true;

                foreach (KeyValuePair<String, DefectDetails> kvp in defectDict)
                {
                    DefectDetails defectObj = kvp.Value;

                    considerRecordByDefectType = true;

                    if (!"All".Equals(defectType))
                    {
                        if ("Open".Equals(defectType) &&
                            defectObj.getResolStat().Equals(BackEndObjects.DefectResolStatCodes.DEFECT_RESOL_STAT_CODE_RESOLVED))
                            considerRecordByDefectType = false;
                        else if ("Resolved".Equals(defectType) &&
                            !defectObj.getResolStat().Equals(BackEndObjects.DefectResolStatCodes.DEFECT_RESOL_STAT_CODE_RESOLVED))
                            considerRecordByDefectType = false;
                    }

                    if (considerRecordByDefectType)
                    {
                        String customerEntId = defectObj.getCustomerId();
                        String custName = "";
                        if (defectObj.getCreationMode().Equals(BackEndObjects.DefectDetails.DEFECT_CREATION_MODE_MANUAL))
                        {
                            custName = Contacts.getContactDetailsforContactEntityDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), customerEntId).getContactName();
                            if (custName == null || custName.Equals(""))
                                custName = MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(customerEntId).getEntityName();
                        }
                        else
                            custName = MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(customerEntId).getEntityName();

                        bool considerCustomerEnt = custFilterCheck ? custList.ContainsKey(customerEntId) : true;

                        if (considerCustomerEnt)
                        {
                            if (!contactDict.ContainsKey(customerEntId))
                            {
                                Contacts contactObj = Contacts.
                                    getContactDetailsforContactEntityDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), customerEntId);
                                if (contactObj == null || contactObj.getContactName() == null || contactObj.getContactName().Equals(""))
                                    contactDict.Add(customerEntId, MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(customerEntId).getEntityName());
                                else
                                    contactDict.Add(customerEntId, contactObj.getContactName());

                                if (!highDefect.ContainsKey(customerEntId))
                                    highDefect.Add(customerEntId, 0);

                                if (!lowDefect.ContainsKey(customerEntId))
                                    lowDefect.Add(customerEntId, 0);

                                if (!mediumDefect.ContainsKey(customerEntId))
                                    mediumDefect.Add(customerEntId, 0);
                            }

                            if (!totalDefectNo.ContainsKey(customerEntId))
                                totalDefectNo.Add(customerEntId, 1);
                            else
                                totalDefectNo[customerEntId] += 1;

                            if (defectObj.getSeverity().Equals("High", StringComparison.InvariantCultureIgnoreCase))
                                highDefect[customerEntId] += 1;
                            if (defectObj.getSeverity().Equals("Low", StringComparison.InvariantCultureIgnoreCase))
                                lowDefect[customerEntId] += 1;
                            if (defectObj.getSeverity().Equals("Medium", StringComparison.InvariantCultureIgnoreCase))
                                mediumDefect[customerEntId] += 1;

                            dt.Rows.Add();

                            dt.Rows[rowCount]["Customer Name"] = contactDict[customerEntId];
                            dt.Rows[rowCount]["Severity"] = defectObj.getSeverity();
                            dt.Rows[rowCount]["Defect#"] = defectObj.getDefectId();
                            dt.Rows[rowCount]["Description"] = defectObj.getDescription();
                            dt.Rows[rowCount]["Defect Date"] = defectObj.getDateCreated();
                            dt.Rows[rowCount]["Close Date"] = defectObj.getCloseDate();
                            dt.Rows[rowCount]["Defect Amount"] = defectObj.getTotalAmount();
                            dt.Rows[rowCount]["Defect Status"] = defectObj.getDefectStat();
                            dt.Rows[rowCount]["Defect Resolution Status"] = defectObj.getResolStat();
                            dt.Rows[rowCount]["Assigned To"] = defectObj.getAssignedToUser();

                            rowCount++;
                        }
                    }
                    }

                Dictionary<String, DataTable> reportDict = (Dictionary<String, DataTable>)Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES];

                if (reportDict == null)
                    reportDict = new Dictionary<string, DataTable>();

                if (!reportDict.ContainsKey("incoming_Defect_No_By_Accnt"))
                    reportDict.Add("incoming_Defect_No_By_Accnt", dt);
                else
                    reportDict["incoming_Defect_No_By_Accnt"] = dt;

                Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES] = reportDict;
                lastGeneratedContactDict = new Dictionary<string, Dictionary<string, string>>(); lastGeneratedContactDict.Add(fromDate + "," + toDate, contactDict);
                Dictionary<String, Dictionary<String, float>> lastGeneratedTotalDefectDict = new Dictionary<string, Dictionary<string, float>>(); lastGeneratedTotalDefectDict.Add(fromDate + "," + toDate, totalDefectNo);
                Dictionary<String, Dictionary<String, float>> lastGeneratedHighDefectDict = new Dictionary<string, Dictionary<string, float>>(); lastGeneratedHighDefectDict.Add(fromDate + "," + toDate, highDefect);
                Dictionary<String, Dictionary<String, float>> lastGeneratedLowDefectDict = new Dictionary<string, Dictionary<string, float>>(); lastGeneratedLowDefectDict.Add(fromDate + "," + toDate,lowDefect);
                Dictionary<String, Dictionary<String, float>> lastGeneratedMedmDefectDict = new Dictionary<string, Dictionary<string, float>>(); lastGeneratedMedmDefectDict.Add(fromDate + "," + toDate, mediumDefect);

                Session[SessionFactory.ALL_DASHBOARD_INCM_DEFECT_DEFECTNOBYACCNT_CONTACT_DICT]=lastGeneratedContactDict;
                Session[SessionFactory.ALL_DASHBOARD_INCM_DEFECT_DEFECTNOBYACCNT_HIGH_DEFECT] = lastGeneratedHighDefectDict;
                Session[SessionFactory.ALL_DASHBOARD_INCM_DEFECT_DEFECTNOBYACCNT_MEDM_DEFECT] = lastGeneratedMedmDefectDict;
                Session[SessionFactory.ALL_DASHBOARD_INCM_DEFECT_DEFECTNOBYACCNT_LOW_DEFECT] = lastGeneratedLowDefectDict;
                Session[SessionFactory.ALL_DASHBOARD_INCM_DEFECT_DEFECTNOBYACCNT_TOTAL_DEFECT] = lastGeneratedTotalDefectDict;
                Session[SessionFactory.ALL_DASHBOARD_INCM_DEFECT_DEFECTNOBYACCNT_LAST_DEFECT_TYPE] = defectType;
            }
            //Now populate the List box with customer details
            foreach (KeyValuePair<String, String> kvp in contactDict)
            {
                ListItem ltExists = Listbox_Incoming_Defect_No_By_Account.Items.FindByValue(kvp.Key);
                if (ltExists == null || ltExists.Text.Equals(""))
                {
                    ltExists = new ListItem();
                    ltExists.Text = kvp.Value;
                    ltExists.Value = kvp.Key;

                    Listbox_Incoming_Defect_No_By_Account.Items.Add(ltExists);
                }
            }

            String[] customerArray = new String[contactDict.Count];
            float[] totalDefectNoArray = new float[contactDict.Count];
            float[] totalHighDefectNoArray = new float[contactDict.Count];
            float[] totalLowDefectNoArray = new float[contactDict.Count];
            float[] totalMediumDefectNoArray = new float[contactDict.Count];

            int counter = 0;
            foreach (KeyValuePair<String, float> kvp in totalDefectNo)
            {
                customerArray[counter] = contactDict[kvp.Key];

                totalDefectNoArray[counter] = totalDefectNo[kvp.Key];
                totalHighDefectNoArray[counter] = highDefect[kvp.Key];
                totalLowDefectNoArray[counter] = lowDefect[kvp.Key];
                totalMediumDefectNoArray[counter] = mediumDefect[kvp.Key];

                counter++;
            }

            Chart_Incoming_Defect_No_By_Account.Titles.Clear();
            Chart_Incoming_Defect_No_By_Account.Titles.Add("Total Number of Defects for Customers during this period (defect submit date)");
            Chart_Incoming_Defect_No_By_Account.Titles[0].Font = new System.Drawing.Font("Book Antiqua", 10);
            Chart_Incoming_Defect_No_By_Account.Series["HighDefects"].Label = "#VALY{0;0;#}";
            Chart_Incoming_Defect_No_By_Account.Series["LowDefects"].Label = "#VALY{0;0;#}";
            Chart_Incoming_Defect_No_By_Account.Series["MediumDefects"].Label = "#VALY{0;0;#}";

            Chart_Incoming_Defect_No_By_Account.Series["HighDefects"].Points.DataBindXY(customerArray, totalHighDefectNoArray);
            Chart_Incoming_Defect_No_By_Account.Series["LowDefects"].Points.DataBindXY(customerArray, totalLowDefectNoArray);
            Chart_Incoming_Defect_No_By_Account.Series["MediumDefects"].Points.DataBindXY(customerArray, totalMediumDefectNoArray);
        }

        protected LeadandPotential generateLeadCharts()
        {
           String  fromDate = (TextBox_From_Date_Lead_Val.Text.Equals("")?DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd"):TextBox_From_Date_Lead_Val.Text);
          String  toDate = (TextBox_To_Date_Lead_Val.Text.Equals("")?DateTime.Now.ToString("yyyy-MM-dd"):TextBox_To_Date_Lead_Val.Text);

          Dictionary<String, LeadandPotential> lastGeneratedLeadPotDict = (Dictionary<String, LeadandPotential>)Session[SessionFactory.ALL_DASHBOARD_LEAD_AND_POTENTIAL_DICTIONARY];


            ArrayList leadList=new ArrayList();
            Dictionary<String,String> potDict=new Dictionary<String,String>();
            
            LeadandPotential leadPotObj=new LeadandPotential();
            if (lastGeneratedLeadPotDict != null && lastGeneratedLeadPotDict.Count > 0 && lastGeneratedLeadPotDict.ContainsKey(fromDate + "," + toDate))
                leadPotObj = lastGeneratedLeadPotDict[fromDate + "," + toDate];
            else
            {
                leadPotObj = generateLeadListandPotDict(leadList, potDict, fromDate, toDate);
                lastGeneratedLeadPotDict = new Dictionary<String, LeadandPotential>();
                lastGeneratedLeadPotDict.Add(fromDate + "," + toDate, leadPotObj);
                Session[SessionFactory.ALL_DASHBOARD_LEAD_AND_POTENTIAL_DICTIONARY]=lastGeneratedLeadPotDict;
            }

            leadList = leadPotObj.getLeadList();
            potDict = leadPotObj.getPotDict();

            generateLeadConvbyVal(fromDate, toDate, leadList, potDict);
            generateLeadConvbyNumber(fromDate, toDate, leadList, potDict);

            return leadPotObj;
        }

        /*protected void generatePotentialCharts(ArrayList potList)
        {
            String fromDate = DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd");
            String toDate = DateTime.Now.ToString("yyyy-MM-dd");

            generatePotnConvbyVal(fromDate,toDate,potList);
            generatePotnConvbyNo(fromDate, toDate, potList);
            generatePotnValbyStage(fromDate, toDate, potList);
            generatePotnbyCategory(fromDate, toDate, potList);
        }*/

        protected void generatePotnValbyStage(String fromDate, String toDate, ArrayList potList)
        {
            if (fromDate == null || fromDate.Equals(""))
                fromDate = DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd");
            if (toDate == null || toDate.Equals(""))
                toDate = DateTime.Now.ToString("yyyy-MM-dd");

            Dictionary<String, float> potStages = new Dictionary<string, float>();
            //The key of the last generated value is the from date+to date
            Dictionary<String, Dictionary<String, float>> lastGeneratedPotStagesVal = (Dictionary<String, Dictionary<String, float>>)Session[SessionFactory.ALL_DASHBOARD_POTN_BY_STAGES];
            if (lastGeneratedPotStagesVal != null && lastGeneratedPotStagesVal.Count > 0 && lastGeneratedPotStagesVal.ContainsKey(fromDate + "," + toDate))
                potStages = lastGeneratedPotStagesVal[fromDate + "," + toDate];
            else
            {
                potStages.Add(PotentialStatus.POTENTIAL_STAT_PRELIM, 0);
                potStages.Add(PotentialStatus.POTENTIAL_STAT_MEDIUM, 0);
                potStages.Add(PotentialStatus.POTENTIAL_STAT_ADVNCD, 0);

                if (potList == null || potList.Count == 0)
                {
                    ActionLibrary.SalesActions._dispPotentials dspPotObj = new SalesActions._dispPotentials();

                    Dictionary<String, String> filterParamsPot = new Dictionary<String, String>();
                    filterParamsPot.Add(dspPotObj.FILTER_BY_CREATE_DATE_FROM, fromDate);
                    filterParamsPot.Add(dspPotObj.FILTER_BY_CREATE_DATE_TO, toDate);

                    potList = dspPotObj.
                       getAllPotentialsFiltered(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), User.Identity.Name, filterParamsPot);
                }

                for (int i = 0; i < potList.Count; i++)
                {
                    PotentialRecords potRec = (PotentialRecords)potList[i];
                    if (potStages.ContainsKey(potRec.getPotStat()))
                        potStages[potRec.getPotStat()] = potStages[potRec.getPotStat()] + potRec.getPotenAmnt();
                }

                lastGeneratedPotStagesVal = new Dictionary<string, Dictionary<string, float>>();
                lastGeneratedPotStagesVal.Add(fromDate + "," + toDate, potStages);
                Session[SessionFactory.ALL_DASHBOARD_POTN_BY_STAGES] = lastGeneratedPotStagesVal;
            }
            

            DataTable dt = new DataTable();
            dt.Columns.Add("Stage");
            dt.Columns.Add("Stage Total");

            Chart_Pot_By_Stage.Titles.Clear();
            Chart_Pot_By_Stage.Series[0].Points.Clear();
            Series potnValByStage = Chart_Pot_By_Stage.Series[0];
            potnValByStage.IsVisibleInLegend = true;

            int counter = 0;
            foreach (KeyValuePair<String, float> kvp in potStages)
            {
                dt.Rows.Add();

                dt.Rows[counter]["Stage"] = kvp.Key.ToString();
                dt.Rows[counter]["Stage Total"] = kvp.Value.ToString();
                counter++;
            }

            Chart_Pot_By_Stage.Titles.Add("Potential value in different stages");
            Chart_Pot_By_Stage.Titles[0].Font = new System.Drawing.Font("Book Antiqua", 9);
            Chart_Pot_By_Stage.Series[0].Label = "#VALY{0;0;#}";
            Chart_Pot_By_Stage.DataSource = dt;
            Chart_Pot_By_Stage.Series[0].XValueMember = "Stage";
            Chart_Pot_By_Stage.Series[0].YValueMembers = "Stage Total";
            Chart_Pot_By_Stage.ChartAreas["ChartArea1"].AxisX.Interval = 0;
            Chart_Pot_By_Stage.DataBind();
            
        }

        protected void generatePotnbyCategory(String fromDate, String toDate, ArrayList potList)
        {
            if (fromDate == null || fromDate.Equals(""))
                fromDate = DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd");
            if (toDate == null || toDate.Equals(""))
                toDate = DateTime.Now.ToString("yyyy-MM-dd");

            Dictionary<String,Dictionary<String,float>> lastGeneratedPotCat=(Dictionary<String,Dictionary<String,float>>) Session[SessionFactory.ALL_DASHBOARD_POTN_BY_CATEGORY];
            Dictionary<String, Dictionary<String, String>> lastGeneratedPotCatNames = (Dictionary<String, Dictionary<String, String>>)Session[SessionFactory.ALL_DASHBOARD_POTN_BY_CATEGORY_CATG_NAMES];
            Dictionary<String, float> potCat=new Dictionary<string,float>();
            Dictionary<String, String> prodCatIdName = new Dictionary<String, String>();

            if (lastGeneratedPotCat != null && lastGeneratedPotCat.Count > 0 && lastGeneratedPotCat.ContainsKey(fromDate + "," + toDate))
            {
                potCat = lastGeneratedPotCat[fromDate + "," + toDate];
                prodCatIdName = lastGeneratedPotCatNames[fromDate + "," + toDate];
            }
            else
            {
                int rowCount = 0;
                DataTable dt = new DataTable();
                dt.Columns.Add("Product Category");
                dt.Columns.Add("Product Name");
                dt.Columns.Add("Potential Name");
                dt.Columns.Add("Product's Value in Potential");
                dt.Columns.Add("Total Potential Amount");
                dt.Columns.Add("Date Created");
                dt.Columns.Add("Due Date");
                dt.Columns.Add("Customer Name");
                dt.Columns.Add("Potential Stage");
                dt.Columns.Add("Potential Status");
                dt.Columns.Add("Creation Mode");

                if (potList == null || potList.Count == 0)
                {
                    ActionLibrary.SalesActions._dispPotentials dspPotObj = new SalesActions._dispPotentials();

                    Dictionary<String, String> filterParamsPot = new Dictionary<String, String>();
                    filterParamsPot.Add(dspPotObj.FILTER_BY_CREATE_DATE_FROM, fromDate);
                    filterParamsPot.Add(dspPotObj.FILTER_BY_CREATE_DATE_TO, toDate);

                    potList = dspPotObj.
                       getAllPotentialsFiltered(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), User.Identity.Name, filterParamsPot);
                }

                for (int i = 0; i < potList.Count; i++)
                {
                    PotentialRecords potRec = (PotentialRecords)potList[i];

                    //Consider only if the potential is not already closed
                    if (!potRec.getPotStat().Equals(RFQShortlisted.POTENTIAL_ACTIVE_STAT_CLOSED_LOST) &&
                        !potRec.getPotStat().Equals(RFQShortlisted.POTENTIAL_ACTIVE_STAT_CLOSED_WON))
                    {
                        Dictionary<String, RFQResponseQuotes> rfqRespDict = RFQResponseQuotes.getAllResponseQuotesforRFQandResponseEntityDB(potRec.getRFQId(),
                            Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

                        foreach (KeyValuePair<String, RFQResponseQuotes> kvp in rfqRespDict)
                        {
                            RFQProdServQnty prodSrvQntyObj = RFQProdServQnty.getRFQProductServiceQuantityforRFIdandCatIdDB(potRec.getRFQId(), kvp.Key.ToString());
                            float fromQnty = prodSrvQntyObj.getFromQnty();
                            float toQnty = prodSrvQntyObj.getToQnty();
                            float respQuoteAmount = (fromQnty != 0 && toQnty != 0) ? (((fromQnty + toQnty) / 2) * float.Parse(kvp.Value.getQuote())) :
                                (fromQnty != 0 ? fromQnty * float.Parse(kvp.Value.getQuote()) : toQnty * float.Parse(kvp.Value.getQuote()));

                            if (!potCat.ContainsKey(kvp.Key.ToString()))
                                potCat.Add(kvp.Key.ToString(), respQuoteAmount);
                            else
                                potCat[kvp.Key.ToString()] = potCat[kvp.Key.ToString()] + respQuoteAmount;

                            dt.Rows.Add();
                            String catName = "";
                            if (!prodCatIdName.ContainsKey(kvp.Key))
                            {
                                catName = ProductCategory.getProductCategorybyIdwoFeaturesDB(kvp.Key).getProductCategoryName();
                                prodCatIdName.Add(kvp.Key, catName);
                            }
                            else
                                catName = prodCatIdName[kvp.Key];

                            dt.Rows[rowCount]["Product Category"] = catName;
                            dt.Rows[rowCount]["Product Name"] = kvp.Value.getProductName();
                            dt.Rows[rowCount]["Product's Value in Potential"] = respQuoteAmount;
                            dt.Rows[rowCount]["Total Potential Amount"] = potRec.getPotenAmnt();
                            dt.Rows[rowCount]["Potential Name"] = potRec.getRFQName();
                            dt.Rows[rowCount]["Date Created"] = potRec.getCreatedDate();
                            dt.Rows[rowCount]["Due Date"] = potRec.getDueDate();
                            dt.Rows[rowCount]["Customer Name"] = potRec.getEntityName();
                            dt.Rows[rowCount]["Potential Stage"] = potRec.getPotStat();
                            dt.Rows[rowCount]["Potential Status"] = potRec.getPotActStat();
                            dt.Rows[rowCount]["Creation Mode"] = potRec.getCreateMode();

                            rowCount++;
                        }
                    }
                }

                Dictionary<String, DataTable> reportDict = (Dictionary<String, DataTable>)
Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES];

                if (reportDict == null)
                    reportDict = new Dictionary<string, DataTable>();

                if (!reportDict.ContainsKey("potn_by_Cat"))
                    reportDict.Add("potn_by_Cat", dt);
                else
                    reportDict["potn_by_Cat"] = dt;

                Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES] = reportDict;

                lastGeneratedPotCat = new Dictionary<String, Dictionary<String, float>>();
                lastGeneratedPotCatNames = new Dictionary<String, Dictionary<String, String>>();

                lastGeneratedPotCat.Add(fromDate + "," + toDate, potCat);
                lastGeneratedPotCatNames.Add(fromDate + "," + toDate, prodCatIdName);
                Session[SessionFactory.ALL_DASHBOARD_POTN_BY_CATEGORY] = lastGeneratedPotCat;
                Session[SessionFactory.ALL_DASHBOARD_POTN_BY_CATEGORY_CATG_NAMES] = lastGeneratedPotCatNames;
            }

            Chart_Potn_By_Product.Titles.Clear();
            Chart_Potn_By_Product.Series[0].Points.Clear();

            Series potByProdSeries = Chart_Potn_By_Product.Series[0];

            potByProdSeries.IsVisibleInLegend = true;

            int j = 0;
            foreach (KeyValuePair<String, float> kvp in potCat)
            {
                potByProdSeries.Points.Add(kvp.Value);
                potByProdSeries.Points[j].LegendText = prodCatIdName[kvp.Key];
                j++;
            }
            Chart_Potn_By_Product.Titles.Add("All Product's % in total potential pipeline(by Value)");
            Chart_Potn_By_Product.Titles[0].Font = new System.Drawing.Font("Book Antiqua", 9);
            Chart_Potn_By_Product.Series[0].Label = "#PERCENT{P0}(#VALY{0;0;#})";

        }

        protected void generatePotnConvbyVal(String fromDate, String toDate, ArrayList potList)
        {
            if (fromDate == null || fromDate.Equals(""))
                fromDate = DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd");
            if (toDate == null || toDate.Equals(""))
                toDate = DateTime.Now.ToString("yyyy-MM-dd");
                        
            Dictionary<String, String> potnConvValLatestDict = new Dictionary<String, String>();
            float successVal = 0;
            float failureVal = 0;

            Dictionary<String, Dictionary<String, String>> lastGeneratedPotnConvVal = (Dictionary<String,Dictionary<String,String>>)Session[SessionFactory.ALL_DASHBOARD_POTN_BY_VAL_SUCCESS_FAILURE];
           
            if (lastGeneratedPotnConvVal != null && lastGeneratedPotnConvVal.Count > 0 && lastGeneratedPotnConvVal.ContainsKey(fromDate + "," + toDate))
            {
                potnConvValLatestDict = lastGeneratedPotnConvVal[fromDate + "," + toDate];
                successVal = float.Parse(potnConvValLatestDict["success"]);
                failureVal = float.Parse(potnConvValLatestDict["failure"]);
            }

            else
            {
                if (potList == null || potList.Count == 0)
                {
                    ActionLibrary.SalesActions._dispPotentials dspPotObj = new SalesActions._dispPotentials();

                    Dictionary<String, String> filterParamsPot = new Dictionary<String, String>();
                    filterParamsPot.Add(dspPotObj.FILTER_BY_CREATE_DATE_FROM, fromDate);
                    filterParamsPot.Add(dspPotObj.FILTER_BY_CREATE_DATE_TO, toDate);

                    potList = dspPotObj.
                       getAllPotentialsFiltered(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), User.Identity.Name, filterParamsPot);

                }

                int rowCount = 0;
                DataTable dt = new DataTable();
                dt.Columns.Add("Potential Name");
                dt.Columns.Add("Date Created");
                dt.Columns.Add("Due Date");
                dt.Columns.Add("Customer Name");
                dt.Columns.Add("Potential Stage");
                dt.Columns.Add("Potential Status");
                dt.Columns.Add("Amount");
                dt.Columns.Add("Creation Mode");

                for (int i = 0; i < potList.Count; i++)
                {
                    PotentialRecords potRec = (PotentialRecords)potList[i];

                    dt.Rows.Add();

                    dt.Rows[rowCount]["Potential Name"] = potRec.getRFQName();
                    dt.Rows[rowCount]["Date Created"] = potRec.getCreatedDate();
                    dt.Rows[rowCount]["Due Date"] = potRec.getDueDate();
                    dt.Rows[rowCount]["Customer Name"] = potRec.getEntityName();
                    dt.Rows[rowCount]["Potential Stage"] = potRec.getPotStat();
                    dt.Rows[rowCount]["Potential Status"] = potRec.getPotActStat();
                    dt.Rows[rowCount]["Amount"] = potRec.getPotenAmnt();
                    dt.Rows[rowCount]["Creation Mode"] = potRec.getCreateMode();

                    rowCount++;

                    if (potRec.getPotStat().Equals(BackEndObjects.RFQShortlisted.POTENTIAL_ACTIVE_STAT_CLOSED_WON))
                    {
                        successVal += potRec.getPotenAmnt();
                    }
                    else
                    {
                        failureVal += potRec.getPotenAmnt();
                    }
                }

                Dictionary<String, DataTable> reportDict = (Dictionary<String, DataTable>)
    Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES];

                if (reportDict == null)
                    reportDict = new Dictionary<string, DataTable>();

                if (!reportDict.ContainsKey("potn_by_Val"))
                    reportDict.Add("potn_by_Val", dt);
                else
                    reportDict["potn_by_Val"] = dt;

                Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES] = reportDict;
                
                potnConvValLatestDict.Add("success", successVal.ToString());
                potnConvValLatestDict.Add("failure", failureVal.ToString());
                lastGeneratedPotnConvVal = new Dictionary<String, Dictionary<String, String>>();
                lastGeneratedPotnConvVal.Add(fromDate + "," + toDate,potnConvValLatestDict);

                Session[SessionFactory.ALL_DASHBOARD_POTN_BY_VAL_SUCCESS_FAILURE] = lastGeneratedPotnConvVal;
            }

            Chart_Potn_Conv_By_Val.Titles.Clear();
            Chart_Potn_Conv_By_Val.Series[0].Points.Clear();

            Series potnConvbyValSeries = Chart_Potn_Conv_By_Val.Series[0];

            potnConvbyValSeries.IsVisibleInLegend = true;

            potnConvbyValSeries.Points.Add(successVal);
            potnConvbyValSeries.Points[0].LegendText = "Success";
            potnConvbyValSeries.Points.Add(failureVal);
            potnConvbyValSeries.Points[1].LegendText = "Failure";


            Chart_Potn_Conv_By_Val.Titles.Add("Potential Conversion % by Amount");
            Chart_Potn_Conv_By_Val.Titles[0].Font = new System.Drawing.Font("Book Antiqua", 9);
            Chart_Potn_Conv_By_Val.Series[0].Label = "#PERCENT{P0}(#VALY{0;0;#})";
                        
            //Session[SessionFactory.ALL_DASHBOARD_POTN_BY_VAL_SUCCESS_FAILURE] = potnConvValLatestDict;

        }

        protected void generatePotnConvbyNo(String fromDate, String toDate, ArrayList potList)
        {
            if (fromDate == null || fromDate.Equals(""))
                fromDate = DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd");
            if (toDate == null || toDate.Equals(""))
                toDate = DateTime.Now.ToString("yyyy-MM-dd");

            int successCount = 0;
            int failureCount = 0;
            Dictionary<String, String> potnConvNoLatestDict = new Dictionary<String, String>();

            Dictionary<String, Dictionary<String, String>> lastGeneratedPotnConvDict = (Dictionary<String, Dictionary<String, String>>)
                Session[SessionFactory.ALL_DASHBOARD_POTN_BY_NO_SUCCESS_FAILURE];

            if (lastGeneratedPotnConvDict != null && lastGeneratedPotnConvDict.Count > 0 && lastGeneratedPotnConvDict.ContainsKey(fromDate + "," + toDate))
            {
                potnConvNoLatestDict = lastGeneratedPotnConvDict[fromDate + "," + toDate];
                successCount = Int32.Parse(potnConvNoLatestDict["success"]);
                failureCount = Int32.Parse(potnConvNoLatestDict["failure"]);
            }

            else
            {
                if (potList == null || potList.Count == 0)
                {
                    ActionLibrary.SalesActions._dispPotentials dspPotObj = new SalesActions._dispPotentials();

                    Dictionary<String, String> filterParamsPot = new Dictionary<String, String>();
                    filterParamsPot.Add(dspPotObj.FILTER_BY_CREATE_DATE_FROM, fromDate);
                    filterParamsPot.Add(dspPotObj.FILTER_BY_CREATE_DATE_TO, toDate);

                    potList = dspPotObj.
                       getAllPotentialsFiltered(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), User.Identity.Name, filterParamsPot);

                }

                int rowCount = 0;
                DataTable dt = new DataTable();
                dt.Columns.Add("Potential Name");
                dt.Columns.Add("Date Created");
                dt.Columns.Add("Due Date");
                dt.Columns.Add("Customer Name");
                dt.Columns.Add("Potential Stage");
                dt.Columns.Add("Potential Status");
                //dt.Columns.Add("Amount");
                dt.Columns.Add("Creation Mode");

                for (int i = 0; i < potList.Count; i++)
                {
                    PotentialRecords potRec = (PotentialRecords)potList[i];

                    dt.Rows.Add();

                    dt.Rows[rowCount]["Potential Name"] = potRec.getRFQName();
                    dt.Rows[rowCount]["Date Created"] = potRec.getCreatedDate();
                    dt.Rows[rowCount]["Due Date"] = potRec.getDueDate();
                    dt.Rows[rowCount]["Customer Name"] = potRec.getEntityName();
                    dt.Rows[rowCount]["Potential Stage"] = potRec.getPotStat();
                    dt.Rows[rowCount]["Potential Status"] = potRec.getPotActStat();
                    dt.Rows[rowCount]["Creation Mode"] = potRec.getCreateMode();
                    rowCount++;

                    if (potRec.getPotStat().Equals(BackEndObjects.RFQShortlisted.POTENTIAL_ACTIVE_STAT_CLOSED_WON))
                        successCount += 1;
                    else
                        failureCount += 1;
                }

                Dictionary<String, DataTable> reportDict = (Dictionary<String, DataTable>)
Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES];

                if (reportDict == null)
                    reportDict = new Dictionary<string, DataTable>();

                if (!reportDict.ContainsKey("potn_by_No"))
                    reportDict.Add("potn_by_No", dt);
                else
                    reportDict["potn_by_No"] = dt;

                Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES] = reportDict;

                potnConvNoLatestDict.Add("success", successCount.ToString());
                potnConvNoLatestDict.Add("failure", failureCount.ToString());
                lastGeneratedPotnConvDict = new Dictionary<String, Dictionary<String, String>>();
                lastGeneratedPotnConvDict.Add(fromDate + "," + toDate, potnConvNoLatestDict);

                Session[SessionFactory.ALL_DASHBOARD_POTN_BY_NO_SUCCESS_FAILURE] = lastGeneratedPotnConvDict;

            }
            Chart_Potn_Conv_By_No.Titles.Clear();
            Chart_Potn_Conv_By_No.Series[0].Points.Clear();

            Series potnConvbyNoSeries = Chart_Potn_Conv_By_No.Series[0];

            potnConvbyNoSeries.IsVisibleInLegend = true;

            potnConvbyNoSeries.Points.Add(successCount);
            potnConvbyNoSeries.Points[0].LegendText = "Success";
            potnConvbyNoSeries.Points.Add(failureCount);
            potnConvbyNoSeries.Points[1].LegendText = "Failure";


            Chart_Potn_Conv_By_No.Titles.Add("Potential Conversion % by Number");
            Chart_Potn_Conv_By_No.Titles[0].Font = new System.Drawing.Font("Book Antiqua", 9);
            Chart_Potn_Conv_By_No.Series[0].Label = "#PERCENT{P0}(#VALY{0;0;#})";

            //Session[SessionFactory.ALL_DASHBOARD_POTN_BY_NO_SUCCESS_FAILURE] = potnConvNoLatestDict;


        }

        protected void Button_Potn_Conv_Val_Show_Click(object sender, EventArgs e)
        {
            generatePotnConvbyVal(TextBox_From_Date_Potn_Val.Text, TextBox_To_Date_Potn_Val.Text, new ArrayList());

            //re-create the other potential chart from last saved value - its getting refreshed otherwise
           /* Dictionary<String, String> successFailDict = (Dictionary<String, String>)
                Session[SessionFactory.ALL_DASHBOARD_POTN_BY_NO_SUCCESS_FAILURE];

            int successCount = int.Parse(successFailDict["success"]);
            int failureCount = int.Parse(successFailDict["failure"]);

            Chart_Potn_Conv_By_No.Titles.Clear();
            Chart_Potn_Conv_By_No.Series[0].Points.Clear();

            Series potnConvbyNumberSeries = Chart_Potn_Conv_By_No.Series[0];

            potnConvbyNumberSeries.IsVisibleInLegend = true;

            potnConvbyNumberSeries.Points.Add(successCount);
            potnConvbyNumberSeries.Points[0].LegendText = "Success";
            potnConvbyNumberSeries.Points.Add(failureCount);
            potnConvbyNumberSeries.Points[1].LegendText = "Failure";

            Chart_Potn_Conv_By_No.Titles.Add("Potential Conversion % by Numbers");
            Chart_Potn_Conv_By_No.Series[0].Label = "#PERCENT{P0}(#VALY{0;0;#})";*/

            Button_Potn_Conv_Val_Show.Focus();
        }

        protected void Button_Potn_Conv_No_Show_Click(object sender, EventArgs e)
        {
            generatePotnConvbyNo(TextBox_From_Date_Potn_No.Text, TextBox_To_Date_Potn_No.Text, new ArrayList());

            //re-create the other potential chart from last saved value - its getting refreshed otherwise
/*            Dictionary<String, String> successFailDict = (Dictionary<String, String>)
                Session[SessionFactory.ALL_DASHBOARD_POTN_BY_VAL_SUCCESS_FAILURE];

            float successVal = float.Parse(successFailDict["success"]);
            float failureVal = float.Parse(successFailDict["failure"]);

            Chart_Potn_Conv_By_Val.Titles.Clear();
            Chart_Potn_Conv_By_Val.Series[0].Points.Clear();

            Series potnConvbyValSeries = Chart_Potn_Conv_By_Val.Series[0];

            potnConvbyValSeries.IsVisibleInLegend = true;

            potnConvbyValSeries.Points.Add(successVal);
            potnConvbyValSeries.Points[0].LegendText = "Success";
            potnConvbyValSeries.Points.Add(failureVal);
            potnConvbyValSeries.Points[1].LegendText = "Failure";

            Chart_Potn_Conv_By_Val.Titles.Add("Potential Conversion % by Value");
            Chart_Potn_Conv_By_Val.Series[0].Label = "#PERCENT{P0}(#VALY{0;0;#})";*/

            Button_Potn_Conv_No_Show.Focus();
        }

        protected void Button_Potn_Stage_Click(object sender, EventArgs e)
        {
            generatePotnValbyStage(TextBox_From_Date_Potn_Stage.Text, TextBox_To_Date_Potn_Stage.Text, new ArrayList());

            Button_Potn_Stage.Focus();
        }

        protected void Button_Potn_By_Cat_Click(object sender, EventArgs e)
        {
            generatePotnbyCategory(TextBox_From_Date_Potn_by_Cat.Text, TextBox_To_Date_Potn_By_Cat.Text, new ArrayList());

            Button_Potn_By_Cat.Focus();
        }

        protected void generateTotalBusinessForSalesByAccounts(String fromDate, String toDate, ArrayList invList, Dictionary<String, String> custList)
        {
            if (fromDate == null || fromDate.Equals(""))
                fromDate = DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd");
            if (toDate == null || toDate.Equals(""))
                toDate = DateTime.Now.ToString("yyyy-MM-dd");

            bool custFilterCheck = (custList != null && custList.Count > 0) ? true : false;
            Boolean regenerateValues = false;

            Dictionary<String, String> contactDict = new Dictionary<String, String>();
            Dictionary<String, float> totalBusiness = new Dictionary<string, float>();
            Dictionary<String, float> totalPending = new Dictionary<string, float>();
            Dictionary<String, float> businessDuringPeriod = new Dictionary<string, float>();

            Dictionary<String, Dictionary<String, String>> lastGeneratedContactDict = (Dictionary<String, Dictionary<String, String>>)
                Session[SessionFactory.ALL_DASHBOARD_TRAN_SALES_TOTAL_BUSINESS_BY_ACCNT_CONTACTS];

            if (lastGeneratedContactDict != null && lastGeneratedContactDict.Count > 0 && lastGeneratedContactDict.ContainsKey(fromDate + "," + toDate))
            {
                totalBusiness = ((Dictionary<String, Dictionary<String, float>>)Session[SessionFactory.ALL_DASHBOARD_TRAN_SALES_TOTAL_BUSINESS_BY_ACCNT_TOTAL_BUSINESS])[fromDate + "," + toDate];
                totalPending = ((Dictionary<String, Dictionary<String, float>>)Session[SessionFactory.ALL_DASHBOARD_TRAN_SALES_TOTAL_BUSINESS_BY_ACCNT_TOTAL_PENDING])[fromDate + "," + toDate];
                businessDuringPeriod = ((Dictionary<String, Dictionary<String, float>>)Session[SessionFactory.ALL_DASHBOARD_TRAN_SALES_TOTAL_BUSINESS_BY_ACCNT_TOTAL_BUSINESS_DURING_PERIOD])[fromDate + "," + toDate];

                if (custFilterCheck)
                {
                    Dictionary<String, float> totalBusinessTemp = new Dictionary<string, float>();
                    Dictionary<String, float> totalPendingTemp = new Dictionary<string, float>();
                    Dictionary<String, float> businessDuringPeriodTemp = new Dictionary<string, float>();

                    foreach (KeyValuePair<String, String> kvp in custList)
                    {
                        if (totalBusiness.ContainsKey(kvp.Key))
                            totalBusinessTemp.Add(kvp.Key, totalBusiness[kvp.Key]);
                        else
                           regenerateValues = true;
                        if (totalPending.ContainsKey(kvp.Key))
                            totalPendingTemp.Add(kvp.Key, totalPending[kvp.Key]);
                        else
                           regenerateValues = true;
                    if (businessDuringPeriod.ContainsKey(kvp.Key))
                            businessDuringPeriodTemp.Add(kvp.Key, businessDuringPeriod[kvp.Key]);
                        else
                        regenerateValues = true;
                    }

                    totalBusiness = totalBusinessTemp;
                    totalPending = totalPendingTemp;
                    businessDuringPeriod = businessDuringPeriodTemp;
                }
            }

            if (lastGeneratedContactDict != null && lastGeneratedContactDict.Count > 0 && lastGeneratedContactDict.ContainsKey(fromDate + "," + toDate) && !regenerateValues)
            {
                contactDict = lastGeneratedContactDict[fromDate + "," + toDate];
                     
            }

            else
            {
                totalBusiness = new Dictionary<string, float>();
                totalPending = new Dictionary<string, float>();
                businessDuringPeriod = new Dictionary<string, float>();

                if (invList == null || invList.Count == 0)
                    invList = Invoice.getAllInvoicesbyRespEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

                for (int i = 0; i < invList.Count; i++)
                {
                    Invoice invObj = (Invoice)invList[i];
                    String customerEntId = RFQDetails.getRFQDetailsbyIdDB(invObj.getRFQId()).getEntityId();
                    bool considerCustomerEnt = custFilterCheck ? custList.ContainsKey(customerEntId) : true;

                    if (considerCustomerEnt)
                    {
                        if (!contactDict.ContainsKey(customerEntId))
                        {
                            Contacts contactObj = Contacts.
                                getContactDetailsforContactEntityDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), customerEntId);
                            if (contactObj == null || contactObj.getContactName() == null || contactObj.getContactName().Equals(""))
                                contactDict.Add(customerEntId, MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(customerEntId).getEntityName());
                            else
                                contactDict.Add(customerEntId, contactObj.getContactName());
                        }

                        Dictionary<String, Payment> paymentDict = Payment.getPaymentDetailsforInvoiceDB(invObj.getInvoiceId());

                        if (!totalBusiness.ContainsKey(customerEntId))
                            totalBusiness.Add(customerEntId, invObj.getTotalAmount());
                        else
                            totalBusiness[customerEntId] = totalBusiness[customerEntId] + invObj.getTotalAmount();

                        if (!totalPending.ContainsKey(customerEntId))
                            totalPending.Add(customerEntId, totalBusiness[customerEntId]);
                        if (!businessDuringPeriod.ContainsKey(customerEntId))
                            businessDuringPeriod.Add(customerEntId, 0);
                        //Total business during the period is the total value of the invoices during the period
                        if (compareDateTime(fromDate, toDate, invObj.getInvoiceDate().Replace(" ", "  ").Substring(0, 10)))
                            businessDuringPeriod[customerEntId] = businessDuringPeriod[customerEntId] + invObj.getTotalAmount();

                        foreach (KeyValuePair<String, Payment> kvp in paymentDict)
                        {
                            Payment pmntObj = kvp.Value;
                            String pmntDate = pmntObj.getPmntDate().Replace(" ", "  ").Substring(0, 10);

                            if (pmntObj.getClearingStat().Equals(Payment.PAYMENT_CLEARING_STAT_CLEAR))
                                totalPending[customerEntId] -= pmntObj.getAmount();
                        }
                    }
                }
                                

                lastGeneratedContactDict = new Dictionary<string, Dictionary<string, string>>(); lastGeneratedContactDict.Add(fromDate + "," + toDate, contactDict);
                Dictionary<String, Dictionary<String, float>> lastGeneratedTotalBusinessDict = new Dictionary<string, Dictionary<string, float>>(); lastGeneratedTotalBusinessDict.Add(fromDate + "," + toDate, totalBusiness);
                Dictionary<String, Dictionary<String, float>> lastGeneratedTotalPendingDict = new Dictionary<string, Dictionary<string, float>>(); lastGeneratedTotalPendingDict.Add(fromDate + "," + toDate, totalPending);
                Dictionary<String, Dictionary<String, float>> lastGeneratedBusinessDuringDict = new Dictionary<string, Dictionary<string, float>>(); lastGeneratedBusinessDuringDict.Add(fromDate + "," + toDate, businessDuringPeriod);

                Session[SessionFactory.ALL_DASHBOARD_TRAN_SALES_TOTAL_BUSINESS_BY_ACCNT_CONTACTS] = lastGeneratedContactDict;
                Session[SessionFactory.ALL_DASHBOARD_TRAN_SALES_TOTAL_BUSINESS_BY_ACCNT_TOTAL_BUSINESS] = lastGeneratedTotalBusinessDict;
                Session[SessionFactory.ALL_DASHBOARD_TRAN_SALES_TOTAL_BUSINESS_BY_ACCNT_TOTAL_PENDING] = lastGeneratedTotalPendingDict;
                Session[SessionFactory.ALL_DASHBOARD_TRAN_SALES_TOTAL_BUSINESS_BY_ACCNT_TOTAL_BUSINESS_DURING_PERIOD] = lastGeneratedBusinessDuringDict;
            }
            //Now populate the List box with customer details
            foreach (KeyValuePair<String, String> kvp in contactDict)
            {
                ListItem ltExists = ListBox_Contacts_Total_Business_Chart.Items.FindByValue(kvp.Key);
                if (ltExists == null || ltExists.Text.Equals(""))
                {
                    ltExists = new ListItem();
                    ltExists.Text = kvp.Value;
                    ltExists.Value = kvp.Key;

                    ListBox_Contacts_Total_Business_Chart.Items.Add(ltExists);
                }
            }

            String[] customerArray = new String[contactDict.Count];
            float[] totalBusinessArray = new float[contactDict.Count];
            float[] totalPendingArray = new float[contactDict.Count];
            float[] businessDuringPeriodArray = new float[contactDict.Count];

            int counter = 0;
            foreach (KeyValuePair<String, float> kvp in totalBusiness)
            {
                customerArray[counter] = contactDict[kvp.Key];
                totalBusinessArray[counter] = totalBusiness[kvp.Key];
                totalPendingArray[counter] = totalPending[kvp.Key];
                businessDuringPeriodArray[counter] = businessDuringPeriod[kvp.Key];
                counter++;
            }

            Chart_Sales_Total_Business_Contact.Titles.Clear();
            Chart_Sales_Total_Business_Contact.Titles.Add("Business during this period (by Invoice date),Total Business (till Date) and Total Pending Amount (till Date) for Customers");
            Chart_Sales_Total_Business_Contact.Titles[0].Font = new System.Drawing.Font("Book Antiqua", 10);
            Chart_Sales_Total_Business_Contact.Series["TotalBusiness"].Label = "#VALY{0;0;#}";
            Chart_Sales_Total_Business_Contact.Series["TotalBusiness"].Points.DataBindXY(customerArray, totalBusinessArray);
            Chart_Sales_Total_Business_Contact.Series["BusinessDuringTimeSpan"].Label = "#VALY{0;0;#}";
            Chart_Sales_Total_Business_Contact.Series["BusinessDuringTimeSpan"].Points.DataBindXY(customerArray, businessDuringPeriodArray);
            Chart_Sales_Total_Business_Contact.Series["TotalPendingAmount"].Label = "#VALY{0;0;#}";
            Chart_Sales_Total_Business_Contact.Series["TotalPendingAmount"].Points.DataBindXY(customerArray, totalPendingArray);

        }
        /// <summary>
        /// For the given time range this function will generate thechart showing  non-cleared and cleared amounts per account
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        protected ArrayList generatePendingClearPaymentsForSalesByAccounts(String fromDate, String toDate, ArrayList invList, Dictionary<String, String> custList)
        {
            if (fromDate == null || fromDate.Equals(""))
                fromDate = DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd");
            if (toDate == null || toDate.Equals(""))
                toDate = DateTime.Now.ToString("yyyy-MM-dd");

            bool custFilterCheck = (custList != null && custList.Count > 0) ? true : false;
            bool regenerateValues = false;
            
            Dictionary<String, String> contactDict = new Dictionary<String, String>();
            Dictionary<String, float> totalCleared = new Dictionary<string, float>();
            Dictionary<String, float> totalNotCleared = new Dictionary<string, float>();

            Dictionary<String, Dictionary<String, String>> lastGeneratedContactDict = (Dictionary<String,Dictionary<String,String>>)
                Session[SessionFactory.ALL_DASHBOARD_TRAN_SALES_PENDINGCLEARPMNT_CONTACT];

            if (lastGeneratedContactDict != null && lastGeneratedContactDict.Count > 0 && lastGeneratedContactDict.ContainsKey(fromDate + "," + toDate))
            {
                totalCleared = ((Dictionary<String, Dictionary<String, float>>)Session[SessionFactory.ALL_DASHBOARD_TRAN_SALES_PENDINGCLEARPMNT_TOTAL_CLEARED])[fromDate + "," + toDate];
                totalNotCleared = ((Dictionary<String, Dictionary<String, float>>)Session[SessionFactory.ALL_DASHBOARD_TRAN_SALES_PENDINGCLEARPMNT_TOTAL_NOT_CLEARED])[fromDate + "," + toDate];

                if (custFilterCheck)
                {
                    Dictionary<String, float> totalClearedTemp = new Dictionary<string, float>();
                    Dictionary<String, float> totalNotClearedTemp = new Dictionary<string, float>();

                    foreach (KeyValuePair<String, String> kvp in custList)
                    {
                        if (totalCleared.ContainsKey(kvp.Key))
                            totalClearedTemp.Add(kvp.Key, totalCleared[kvp.Key]);
                        else
                            regenerateValues = true;
                        if (totalNotCleared.ContainsKey(kvp.Key))
                            totalNotClearedTemp.Add(kvp.Key, totalNotCleared[kvp.Key]);
                        else
                            regenerateValues = true;
                    }

                    //No need to put these updated dictionaries in the session now. The session variable will always contain a superset with other contact names
                    totalCleared = totalClearedTemp;
                    totalNotCleared = totalNotClearedTemp;
                }
            }

            if (lastGeneratedContactDict != null && lastGeneratedContactDict.Count > 0 && lastGeneratedContactDict.ContainsKey(fromDate + "," + toDate) && !regenerateValues)
            {
                contactDict = lastGeneratedContactDict[fromDate + "," + toDate];
                           
            }
            else
            {
                totalCleared = new Dictionary<string, float>();
                totalNotCleared = new Dictionary<string, float>();

                int rowCount = 0;
                DataTable dt = new DataTable();
                dt.Columns.Add("Contact Name");
                dt.Columns.Add("Invoice#");
                dt.Columns.Add("RFQ#");
                dt.Columns.Add("Invoice Date");
                dt.Columns.Add("Payment Dates [for cleared]");
                dt.Columns.Add("Total Invoice Amount");
                dt.Columns.Add("Cleared Amount");
                dt.Columns.Add("Non-Cleared Amount");
                dt.Columns.Add("Total Pending (Including Non-Cleared)");
                //Dictionary<String, Dictionary<String, float>> contactDifferentPaymentModes = new Dictionary<string, Dictionary<string, float>>();

                if (invList == null || invList.Count == 0)
                    invList = Invoice.getAllInvoicesbyRespEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

                for (int i = 0; i < invList.Count; i++)
                {
                    Invoice invObj = (Invoice)invList[i];

                    String customerEntId = RFQDetails.getRFQDetailsbyIdDB(invObj.getRFQId()).getEntityId();
                    bool considerCustomerEnt = custFilterCheck ? custList.ContainsKey(customerEntId) : true;

                    if (considerCustomerEnt)
                    {
                        if (!contactDict.ContainsKey(customerEntId))
                        {
                            Contacts contactObj = Contacts.
                                getContactDetailsforContactEntityDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), customerEntId);
                            if (contactObj == null || contactObj.getContactName() == null || contactObj.getContactName().Equals(""))
                                contactDict.Add(customerEntId, MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(customerEntId).getEntityName());
                            else
                                contactDict.Add(customerEntId, contactObj.getContactName());
                        }

                        dt.Rows.Add();
                        dt.Rows[rowCount]["Contact Name"] = contactDict[customerEntId];
                        dt.Rows[rowCount]["Invoice#"] = (invObj.getInvoiceNo() != null && invObj.getInvoiceNo().Equals("") ? invObj.getInvoiceNo() : invObj.getInvoiceId());
                        dt.Rows[rowCount]["RFQ#"] = invObj.getRFQId();
                        dt.Rows[rowCount]["Invoice Date"] = invObj.getInvoiceDate();
                        dt.Rows[rowCount]["Total Invoice Amount"] = invObj.getTotalAmount();

                        float totalClearedForInvoice = 0;
                        float totalNonClearedForInvoice = 0;
                        String pmntDates = "";

                        Dictionary<String, Payment> paymentDict = Payment.getPaymentDetailsforInvoiceDB(invObj.getInvoiceId());

                        foreach (KeyValuePair<String, Payment> kvp in paymentDict)
                        {
                            Payment pmntObj = kvp.Value;
                            String pmntDate = pmntObj.getPmntDate().Replace(" ", "  ").Substring(0, 10);
                            if (compareDateTime(fromDate, toDate, pmntDate))
                            {
                                if (pmntObj.getClearingStat().Equals(Payment.PAYMENT_CLEARING_STAT_CLEAR))
                                {
                                    if (totalCleared.ContainsKey(customerEntId))
                                        totalCleared[customerEntId] = totalCleared[customerEntId] + pmntObj.getAmount();
                                    else
                                        totalCleared.Add(customerEntId, pmntObj.getAmount());

                                    totalClearedForInvoice += pmntObj.getAmount();
                                    
                                    pmntDates = pmntDates.Equals("") ? pmntDate : pmntDates + "," + pmntDate;
                                    //Make sure both the dictionaries have same number of elements
                                    if (!totalNotCleared.ContainsKey(customerEntId))
                                        totalNotCleared.Add(customerEntId, 0);
                                }
                                else
                                {
                                    if (totalNotCleared.ContainsKey(customerEntId))
                                        totalNotCleared[customerEntId] = totalNotCleared[customerEntId] + pmntObj.getAmount();
                                    else
                                        totalNotCleared.Add(customerEntId, pmntObj.getAmount());

                                    totalNonClearedForInvoice += pmntObj.getAmount();
                                    if (!totalCleared.ContainsKey(customerEntId))
                                        totalCleared.Add(customerEntId, 0);
                                }
                            }
                        }

                        dt.Rows[rowCount]["Payment Dates [for cleared]"] = pmntDates;
                        dt.Rows[rowCount]["Cleared Amount"] = totalClearedForInvoice;
                        dt.Rows[rowCount]["Non-Cleared Amount"] = totalNonClearedForInvoice;
                        dt.Rows[rowCount]["Total Pending (Including Non-Cleared)"] = invObj.getTotalAmount() - totalClearedForInvoice;
                        rowCount++;
                    }
                }

                Dictionary<String, DataTable> reportDict = (Dictionary<String, DataTable>)
Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES];

                if (reportDict == null)
                    reportDict = new Dictionary<string, DataTable>();

                if (!reportDict.ContainsKey("pending_clear_contact_sales"))
                    reportDict.Add("pending_clear_contact_sales", dt);
                else
                    reportDict["pending_clear_contact_sales"] = dt;

                Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES] = reportDict;
                lastGeneratedContactDict = new Dictionary<string, Dictionary<string, string>>();lastGeneratedContactDict.Add(fromDate+","+toDate,contactDict);
                Dictionary<String, Dictionary<String, float>> lastGeneratedtotalClearedDict = new Dictionary<string, Dictionary<string, float>>(); lastGeneratedtotalClearedDict.Add(fromDate + "," + toDate, totalCleared);
                Dictionary<String, Dictionary<String, float>> lastGeneratedtotalNotClearedDict = new Dictionary<string, Dictionary<string, float>>(); lastGeneratedtotalNotClearedDict.Add(fromDate + "," + toDate, totalNotCleared);

                Session[SessionFactory.ALL_DASHBOARD_TRAN_SALES_PENDINGCLEARPMNT_CONTACT] = lastGeneratedContactDict;
                Session[SessionFactory.ALL_DASHBOARD_TRAN_SALES_PENDINGCLEARPMNT_TOTAL_CLEARED] = lastGeneratedtotalClearedDict;
                Session[SessionFactory.ALL_DASHBOARD_TRAN_SALES_PENDINGCLEARPMNT_TOTAL_NOT_CLEARED] = lastGeneratedtotalNotClearedDict;

            }
            //Now populate the List box with customer details
            foreach (KeyValuePair<String, String> kvp in contactDict)
            {
                ListItem ltExists = ListBox_Contacts_Pending_Clear_Amnt_Sales.Items.FindByValue(kvp.Key);
                if (ltExists == null || ltExists.Text.Equals(""))
                {
                    ltExists = new ListItem();
                    ltExists.Text = kvp.Value;
                    ltExists.Value = kvp.Key;

                    ListBox_Contacts_Pending_Clear_Amnt_Sales.Items.Add(ltExists);
                }
            }

            float[] dtCleared = new float[contactDict.Count];
            float[] dtNCleared = new float[contactDict.Count];
            String[] contactArray = new String[contactDict.Count];

            int counter = 0;
            foreach (KeyValuePair<String, float> kvp in totalCleared)
            {
                contactArray[counter] = contactDict[kvp.Key];
                dtCleared[counter] = kvp.Value;
                dtNCleared[counter] = totalNotCleared[kvp.Key];
                counter++;
            }
            Chart_Sales_Pending_Clear_By_Account.Titles.Clear();
            Chart_Sales_Pending_Clear_By_Account.Titles.Add("Cleared and Not-cleared amount from different customers during the period (By Payment Date)");
            Chart_Sales_Pending_Clear_By_Account.Titles[0].Font = new System.Drawing.Font("Book Antiqua", 10);
            Chart_Sales_Pending_Clear_By_Account.Series["clearedSeries"].Label = "#VALY{0;0;#}";
            Chart_Sales_Pending_Clear_By_Account.Series["clearedSeries"].Points.DataBindXY(contactArray, dtCleared);
            Chart_Sales_Pending_Clear_By_Account.Series["NotClearedSeries"].Label = "#VALY{0;0;#}";
            Chart_Sales_Pending_Clear_By_Account.Series["NotClearedSeries"].Points.DataBindXY(contactArray, dtNCleared);


            return invList;
        }
        /// <summary>
        /// The mode will determine which of the two graphs to generate - 
        /// a.both
        /// b.amount
        /// c. qunty
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="invList"></param>
        /// <param name="mode"></param>
        protected void generateProductWiseSalesQnty(String fromDate, String toDate, ArrayList invList)
        {
            if (fromDate == null || fromDate.Equals(""))
                fromDate = DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd");
            if (toDate == null || toDate.Equals(""))
                toDate = DateTime.Now.ToString("yyyy-MM-dd");
            

            Dictionary<String, String> prodDict = new Dictionary<string, string>();
            Dictionary<String, float> prodSalesQnty = new Dictionary<string, float>();

            Dictionary<String, Dictionary<String, String>> lastGeneratedProdDict = (Dictionary<String,Dictionary<String,String>>)
                Session[SessionFactory.ALL_DASHBOARD_TRAN_SALES_PRODUCTWISESALES_QNTY_PROD_DICT];

            if (lastGeneratedProdDict != null && lastGeneratedProdDict.Count > 0 && lastGeneratedProdDict.ContainsKey(fromDate + "," + toDate))
            {
                prodDict = ((Dictionary<String, Dictionary<String, String>>)Session[SessionFactory.ALL_DASHBOARD_TRAN_SALES_PRODUCTWISESALES_QNTY_PROD_DICT])[fromDate + "," + toDate];
                prodSalesQnty = ((Dictionary<String, Dictionary<String, float>>)Session[SessionFactory.ALL_DASHBOARD_TRAN_SALES_PRODUCTWISESALES_QNTY_SALES_QNTY])[fromDate + "," + toDate];
            }
            else
            {

                int rowCount = 0;
                DataTable dt = new DataTable();
                dt.Columns.Add("Product Category");
                dt.Columns.Add("Product Name");
                dt.Columns.Add("RFQ#");
                dt.Columns.Add("Invoice#");
                dt.Columns.Add("Invoice Date");
                dt.Columns.Add("Product Amount");

                if (invList == null || invList.Count == 0)
                    invList = Invoice.getAllInvoicesbyRespEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

                for (int i = 0; i < invList.Count; i++)
                {
                    Invoice invObj = (Invoice)invList[i];
                    String invDate = invObj.getInvoiceDate();
                    if (compareDateTime(fromDate, toDate, invDate.Replace(" ", "  ").Substring(0, 10)))
                    {
                        PurchaseOrder poOBJ = PurchaseOrder.getPurchaseOrderforRFQIdDB(invObj.getRFQId());
                        ArrayList poQuoteList = PurchaseOrderQuote.getPurcahseOrderQuoteListbyPOIdDB(poOBJ.getPo_id());

                        for (int j = 0; j < poQuoteList.Count; j++)
                        {
                            PurchaseOrderQuote poQuoteObj = (PurchaseOrderQuote)poQuoteList[j];
                            dt.Rows.Add();

                            if (!prodDict.ContainsKey(poQuoteObj.getProd_srv_category()))
                                prodDict.Add(poQuoteObj.getProd_srv_category(),
                                    ProductCategory.getProductCategorybyIdwoFeaturesDB(poQuoteObj.getProd_srv_category()).getProductCategoryName());

                            float salesAmount = poQuoteObj.getQuote() * poQuoteObj.getUnits();

                            dt.Rows[rowCount]["Product Category"] = prodDict[poQuoteObj.getProd_srv_category()];
                            dt.Rows[rowCount]["Product Name"] = poQuoteObj.getProduct_name();
                            dt.Rows[rowCount]["RFQ#"] = invObj.getRFQId();
                            dt.Rows[rowCount]["Invoice#"] = (invObj.getInvoiceNo() != null && !invObj.getInvoiceNo().Equals("") ? invObj.getInvoiceNo() : invObj.getInvoiceId());
                            dt.Rows[rowCount]["Invoice Date"] = invObj.getInvoiceDate();
                            dt.Rows[rowCount]["Product Amount"] = salesAmount;

                            rowCount++;

                            if (!prodSalesQnty.ContainsKey(poQuoteObj.getProd_srv_category()))
                                prodSalesQnty.Add(poQuoteObj.getProd_srv_category(), poQuoteObj.getUnits());
                            else
                                prodSalesQnty[poQuoteObj.getProd_srv_category()] += poQuoteObj.getUnits();
                        }
                    }
                }

                Dictionary<String, DataTable> reportDict = (Dictionary<String, DataTable>)
    Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES];

                if (reportDict == null)
                    reportDict = new Dictionary<string, DataTable>();

                if (!reportDict.ContainsKey("prod_wise_Sale"))
                    reportDict.Add("prod_wise_Sale", dt);
                else
                    reportDict["prod_wise_Sale"] = dt;

                lastGeneratedProdDict = new Dictionary<string, Dictionary<string, string>>(); lastGeneratedProdDict.Add(fromDate + "," + toDate, prodDict);
                Dictionary<String, Dictionary<String, float>> lastGeneratedProdSalesQnty = new Dictionary<string, Dictionary<string, float>>(); lastGeneratedProdSalesQnty.Add(fromDate + "," + toDate, prodSalesQnty);

                Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES] = reportDict;
                Session[SessionFactory.ALL_DASHBOARD_TRAN_SALES_PRODUCTWISESALES_QNTY_SALES_QNTY] = lastGeneratedProdSalesQnty;
                Session[SessionFactory.ALL_DASHBOARD_TRAN_SALES_PRODUCTWISESALES_QNTY_PROD_DICT] = lastGeneratedProdDict;

            }

            Chart_Transaction_Sales_Prod_Wise_Qnty.Titles.Clear();
            Chart_Transaction_Sales_Prod_Wise_Qnty.Series[0].Points.Clear();
            Chart_Transaction_Sales_Prod_Wise_Qnty.Titles.Add("Product Wise Sales by Quantity");
            Chart_Transaction_Sales_Prod_Wise_Qnty.Titles[0].Font = new System.Drawing.Font("Book Antiqua", 9);
            Chart_Transaction_Sales_Prod_Wise_Qnty.Series[0].Label = "#PERCENT{P0}(#VALY{0;0;#})";
      
            Series transaction_Sales_Prod_Wise_Qnty = Chart_Transaction_Sales_Prod_Wise_Qnty.Series[0];
            transaction_Sales_Prod_Wise_Qnty.IsVisibleInLegend = true;

            int counter = 0;
            foreach (KeyValuePair<String, float> kvp in prodSalesQnty)
            {
                    transaction_Sales_Prod_Wise_Qnty.Points.Add(prodSalesQnty[kvp.Key]);
                    transaction_Sales_Prod_Wise_Qnty.Points[counter].LegendText = prodDict[kvp.Key];
                counter++;
            }
                  
        }

        protected void generateProductWiseSalesAmnt(String fromDate, String toDate, ArrayList invList)
        {
            if (fromDate == null || fromDate.Equals(""))
                fromDate = DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd");
            if (toDate == null || toDate.Equals(""))
                toDate = DateTime.Now.ToString("yyyy-MM-dd");


            Dictionary<String, String> prodDict = new Dictionary<string, string>();
            Dictionary<String, float> prodSalesAmount = new Dictionary<string, float>();

            Dictionary<String, Dictionary<String, String>> lastGeneratedProdDict =(Dictionary<String,Dictionary<String,String>>) 
                Session[SessionFactory.ALL_DASHBOARD_TRAN_SALES_PRODUCTWISESALES_AMNT_PROD_DICT];

            if (lastGeneratedProdDict != null && lastGeneratedProdDict.Count > 0 && lastGeneratedProdDict.ContainsKey(fromDate + "," + toDate))
            {
                prodDict = lastGeneratedProdDict[fromDate + "," + toDate];
                prodSalesAmount = ((Dictionary<String, Dictionary<String, float>>)Session[SessionFactory.ALL_DASHBOARD_TRAN_SALES_PRODUCTWISESALES_AMNT_SALES_AMNT])[fromDate + "," + toDate];
            }
            else
            {

                if (invList == null || invList.Count == 0)
                    invList = Invoice.getAllInvoicesbyRespEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

                for (int i = 0; i < invList.Count; i++)
                {
                    Invoice invObj = (Invoice)invList[i];
                    String invDate = invObj.getInvoiceDate();
                    if (compareDateTime(fromDate, toDate, invDate.Replace(" ", "  ").Substring(0, 10)))
                    {
                        PurchaseOrder poOBJ = PurchaseOrder.getPurchaseOrderforRFQIdDB(invObj.getRFQId());
                        ArrayList poQuoteList = PurchaseOrderQuote.getPurcahseOrderQuoteListbyPOIdDB(poOBJ.getPo_id());

                        for (int j = 0; j < poQuoteList.Count; j++)
                        {
                            PurchaseOrderQuote poQuoteObj = (PurchaseOrderQuote)poQuoteList[j];

                            if (!prodDict.ContainsKey(poQuoteObj.getProd_srv_category()))
                                prodDict.Add(poQuoteObj.getProd_srv_category(),
                                    ProductCategory.getProductCategorybyIdwoFeaturesDB(poQuoteObj.getProd_srv_category()).getProductCategoryName());

                            float salesAmount = poQuoteObj.getQuote() * poQuoteObj.getUnits();

                            if (!prodSalesAmount.ContainsKey(poQuoteObj.getProd_srv_category()))
                                prodSalesAmount.Add(poQuoteObj.getProd_srv_category(), salesAmount);
                            else
                                prodSalesAmount[poQuoteObj.getProd_srv_category()] += salesAmount;

                        }
                    }
                }

                lastGeneratedProdDict = new Dictionary<string, Dictionary<string, string>>(); lastGeneratedProdDict.Add(fromDate + "," + toDate, prodDict);
                Dictionary<String, Dictionary<String, float>> lastGeneratedProdSalesAmntDict = new Dictionary<string, Dictionary<string, float>>(); lastGeneratedProdSalesAmntDict.Add(fromDate + "," + toDate, prodSalesAmount);

                Session[SessionFactory.ALL_DASHBOARD_TRAN_SALES_PRODUCTWISESALES_AMNT_PROD_DICT] = lastGeneratedProdDict;
                Session[SessionFactory.ALL_DASHBOARD_TRAN_SALES_PRODUCTWISESALES_AMNT_SALES_AMNT] = lastGeneratedProdSalesAmntDict;
            }

                Chart_Transaction_Sales_Prod_Wise_Amount.Titles.Clear();
                Chart_Transaction_Sales_Prod_Wise_Amount.Series[0].Points.Clear();

            Series transaction_Sales_Prod_Wise_Amount = Chart_Transaction_Sales_Prod_Wise_Amount.Series[0];
            transaction_Sales_Prod_Wise_Amount.IsVisibleInLegend = true;

            int counter = 0;
            foreach (KeyValuePair<String, float> kvp in prodSalesAmount)
            {
                    transaction_Sales_Prod_Wise_Amount.Points.Add(kvp.Value);
                    transaction_Sales_Prod_Wise_Amount.Points[counter].LegendText = prodDict[kvp.Key];
               counter++;
            }

                Chart_Transaction_Sales_Prod_Wise_Amount.Titles.Add("Product Wise Sales by Amount");
                Chart_Transaction_Sales_Prod_Wise_Amount.Titles[0].Font = new System.Drawing.Font("Book Antiqua", 9);
                Chart_Transaction_Sales_Prod_Wise_Amount.Series[0].Label = "#PERCENT{P0}(#VALY{0;0;#})";

        }

        protected Boolean compareDateTime(String fromDate,String toDate,String targetDate)
        {
                            DateTime dueDateVal;
                            DateTime.TryParseExact(targetDate, "%M/%d/yyyy", null, DateTimeStyles.AllowWhiteSpaces, out dueDateVal);

                            DateTime dueDateFromVal;
                            DateTime.TryParseExact(fromDate, "yyyy-MM-dd", null, DateTimeStyles.None, out dueDateFromVal);
                            DateTime dueDateToVal;
                            DateTime.TryParseExact(toDate, "yyyy-MM-dd", null, DateTimeStyles.None, out dueDateToVal);


                            if (DateTime.Compare(dueDateVal, dueDateFromVal) >= 0 && DateTime.Compare(dueDateVal, dueDateToVal) <= 0)
                                return true;
                            else
                                return false;

        }

        /*protected void generatePurchaseTransactionCharts(String fromDate, String toDate)
        {
            ActionLibrary.PurchaseActions._dispInvoiceDetails dspInv = new PurchaseActions._dispInvoiceDetails();
            Dictionary<String, String> filterParam = new Dictionary<String, String>();
            filterParam.Add(dspInv.FILTER_BY_FROM_DATE, fromDate);
            filterParam.Add(dspInv.FILTER_BY_TO_DATE, toDate);

            ArrayList invList = Invoice.getAllInvoicesbyEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

            //ArrayList invListFiltered=dspInv.
            //getAllInvDetailsFiltered(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), invList, filterParam);


            generateTotalBusinessForPurchaseByAccounts(fromDate, toDate, generatePendingClearPaymentsForPurchaseByAccounts
                (fromDate, toDate, invList, new Dictionary<String, String>()), new Dictionary<String, String>());
            generateProductWisePurchase(fromDate, toDate, invList, "both");
        }*/

        protected void generateProductWisePurchaseQnty(String fromDate, String toDate, ArrayList invList)
        {
            if (fromDate == null || fromDate.Equals(""))
                fromDate = DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd");
            if (toDate == null || toDate.Equals(""))
                toDate = DateTime.Now.ToString("yyyy-MM-dd");

            Dictionary<String, String> prodDict = new Dictionary<string, string>();
            Dictionary<String, float> prodPurchaseQnty = new Dictionary<string, float>();

            Dictionary<String, Dictionary<String, String>> lastGeneratedProdDict = (Dictionary<String, Dictionary<String, String>>)
    Session[SessionFactory.ALL_DASHBOARD_TRAN_PURCHASE_PRODUCTWISEPURCHASE_QNTY_PROD_DICT];

            if (lastGeneratedProdDict != null && lastGeneratedProdDict.Count > 0 && lastGeneratedProdDict.ContainsKey(fromDate + "," + toDate))
            {
                prodDict = ((Dictionary<String, Dictionary<String, String>>)Session[SessionFactory.ALL_DASHBOARD_TRAN_PURCHASE_PRODUCTWISEPURCHASE_QNTY_PROD_DICT])[fromDate + "," + toDate];
                prodPurchaseQnty = ((Dictionary<String, Dictionary<String, float>>)Session[SessionFactory.ALL_DASHBOARD_TRAN_PURCHASE_PRODUCTWISEPURCHASE_QNTY_PURCHASE_QNTY])[fromDate + "," + toDate];
            }
            else
            {
                int rowCount = 0;
                DataTable dt = new DataTable();
                dt.Columns.Add("Product Category");
                dt.Columns.Add("Product Name");
                dt.Columns.Add("RFQ#");
                dt.Columns.Add("Invoice#");
                dt.Columns.Add("Invoice Date");
                dt.Columns.Add("Product Amount");

                if (invList == null || invList.Count == 0)
                    invList = Invoice.getAllInvoicesbyEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

                for (int i = 0; i < invList.Count; i++)
                {
                    Invoice invObj = (Invoice)invList[i];
                    String invDate = invObj.getInvoiceDate();
                    if (compareDateTime(fromDate, toDate, invDate.Replace(" ", "  ").Substring(0, 10)))
                    {
                        PurchaseOrder poOBJ = PurchaseOrder.getPurchaseOrderforRFQIdDB(invObj.getRFQId());
                        ArrayList poQuoteList = PurchaseOrderQuote.getPurcahseOrderQuoteListbyPOIdDB(poOBJ.getPo_id());

                        for (int j = 0; j < poQuoteList.Count; j++)
                        {
                            PurchaseOrderQuote poQuoteObj = (PurchaseOrderQuote)poQuoteList[j];

                            if (!prodDict.ContainsKey(poQuoteObj.getProd_srv_category()))
                                prodDict.Add(poQuoteObj.getProd_srv_category(),
                                    ProductCategory.getProductCategorybyIdwoFeaturesDB(poQuoteObj.getProd_srv_category()).getProductCategoryName());

                            float purchaseAmount = poQuoteObj.getQuote() * poQuoteObj.getUnits();

                            dt.Rows.Add();

                            dt.Rows[rowCount]["Product Category"] = prodDict[poQuoteObj.getProd_srv_category()];
                            dt.Rows[rowCount]["Product Name"] = poQuoteObj.getProduct_name();
                            dt.Rows[rowCount]["RFQ#"] = invObj.getRFQId();
                            dt.Rows[rowCount]["Invoice#"] = (invObj.getInvoiceNo() != null && !invObj.getInvoiceNo().Equals("") ? invObj.getInvoiceNo() : invObj.getInvoiceId());
                            dt.Rows[rowCount]["Invoice Date"] = invObj.getInvoiceDate();
                            dt.Rows[rowCount]["Product Amount"] = purchaseAmount;

                            rowCount++;

                            if (!prodPurchaseQnty.ContainsKey(poQuoteObj.getProd_srv_category()))
                                prodPurchaseQnty.Add(poQuoteObj.getProd_srv_category(), poQuoteObj.getUnits());
                            else
                                prodPurchaseQnty[poQuoteObj.getProd_srv_category()] += poQuoteObj.getUnits();
                        }
                    }
                }

                Dictionary<String, DataTable> reportDict = (Dictionary<String, DataTable>)
Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES];

                if (reportDict == null)
                    reportDict = new Dictionary<string, DataTable>();

                if (!reportDict.ContainsKey("prod_wise_Purchase"))
                    reportDict.Add("prod_wise_Purchase", dt);
                else
                    reportDict["prod_wise_Purchase"] = dt;

                Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES] = reportDict;
                lastGeneratedProdDict = new Dictionary<string, Dictionary<string, string>>(); lastGeneratedProdDict.Add(fromDate + "," + toDate, prodDict);
                Dictionary<String, Dictionary<String, float>> lastGeneratedProdSalesQnty = new Dictionary<string, Dictionary<string, float>>(); lastGeneratedProdSalesQnty.Add(fromDate + "," + toDate, prodPurchaseQnty);

                Session[SessionFactory.ALL_DASHBOARD_TRAN_PURCHASE_PRODUCTWISEPURCHASE_QNTY_PURCHASE_QNTY] = lastGeneratedProdSalesQnty;
                Session[SessionFactory.ALL_DASHBOARD_TRAN_PURCHASE_PRODUCTWISEPURCHASE_QNTY_PROD_DICT] = lastGeneratedProdDict;


            }

                Chart_Transaction_Purchase_Prod_Wise_Qnty.Titles.Clear();
                Chart_Transaction_Purchase_Prod_Wise_Qnty.Series[0].Points.Clear();
            Series transaction_Purchase_Prod_Wise_Qnty = Chart_Transaction_Purchase_Prod_Wise_Qnty.Series[0];
            transaction_Purchase_Prod_Wise_Qnty.IsVisibleInLegend = true;


            int counter = 0;
            foreach (KeyValuePair<String, float> kvp in prodPurchaseQnty)
            {
                    transaction_Purchase_Prod_Wise_Qnty.Points.Add(prodPurchaseQnty[kvp.Key]);
                    transaction_Purchase_Prod_Wise_Qnty.Points[counter].LegendText = prodDict[kvp.Key];
                counter++;
            }

                Chart_Transaction_Purchase_Prod_Wise_Qnty.Titles.Add("Product Wise Purchase by Quantity");
                Chart_Transaction_Purchase_Prod_Wise_Qnty.Titles[0].Font = new System.Drawing.Font("Book Antiqua", 9);
                Chart_Transaction_Purchase_Prod_Wise_Qnty.Series[0].Label = "#PERCENT{P0}(#VALY{0;0;#})";


        }

        protected void generateProductWisePurchaseAmnt(String fromDate, String toDate, ArrayList invList)
        {
            if (fromDate == null || fromDate.Equals(""))
                fromDate = DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd");
            if (toDate == null || toDate.Equals(""))
                toDate = DateTime.Now.ToString("yyyy-MM-dd");


            int rowCount = 0;
            DataTable dt = new DataTable();
            dt.Columns.Add("Product Category");
            dt.Columns.Add("Product Name");
            dt.Columns.Add("RFQ#");
            dt.Columns.Add("Invoice#");
            dt.Columns.Add("Invoice Date");
            dt.Columns.Add("Product Amount");

            Dictionary<String, String> prodDict = new Dictionary<string, string>();
            Dictionary<String, float> prodPurchaseAmount = new Dictionary<string, float>();

            Dictionary<String, Dictionary<String, String>> lastGeneratedProdDict = (Dictionary<String, Dictionary<String, String>>)
    Session[SessionFactory.ALL_DASHBOARD_TRAN_PURCHASE_PRODUCTWISEPURCHASE_AMNT_PROD_DICT];

            if (lastGeneratedProdDict != null && lastGeneratedProdDict.Count > 0 && lastGeneratedProdDict.ContainsKey(fromDate + "," + toDate))
            {
                prodDict = lastGeneratedProdDict[fromDate + "," + toDate];
                prodPurchaseAmount = ((Dictionary<String, Dictionary<String, float>>)Session[SessionFactory.ALL_DASHBOARD_TRAN_PURCHASE_PRODUCTWISEPURCHASE_AMNT_PURCHASE_AMNT])[fromDate + "," + toDate];
            }
            else
            {
                if (invList == null || invList.Count == 0)
                    invList = Invoice.getAllInvoicesbyEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

                for (int i = 0; i < invList.Count; i++)
                {
                    Invoice invObj = (Invoice)invList[i];
                    String invDate = invObj.getInvoiceDate();
                    if (compareDateTime(fromDate, toDate, invDate.Replace(" ", "  ").Substring(0, 10)))
                    {
                        PurchaseOrder poOBJ = PurchaseOrder.getPurchaseOrderforRFQIdDB(invObj.getRFQId());
                        ArrayList poQuoteList = PurchaseOrderQuote.getPurcahseOrderQuoteListbyPOIdDB(poOBJ.getPo_id());

                        for (int j = 0; j < poQuoteList.Count; j++)
                        {
                            PurchaseOrderQuote poQuoteObj = (PurchaseOrderQuote)poQuoteList[j];

                            if (!prodDict.ContainsKey(poQuoteObj.getProd_srv_category()))
                                prodDict.Add(poQuoteObj.getProd_srv_category(),
                                    ProductCategory.getProductCategorybyIdwoFeaturesDB(poQuoteObj.getProd_srv_category()).getProductCategoryName());

                            float purchaseAmount = poQuoteObj.getQuote() * poQuoteObj.getUnits();

                            dt.Rows.Add();

                            dt.Rows[rowCount]["Product Category"] = prodDict[poQuoteObj.getProd_srv_category()];
                            dt.Rows[rowCount]["Product Name"] = poQuoteObj.getProduct_name();
                            dt.Rows[rowCount]["RFQ#"] = invObj.getRFQId();
                            dt.Rows[rowCount]["Invoice#"] = (invObj.getInvoiceNo() != null && !invObj.getInvoiceNo().Equals("") ? invObj.getInvoiceNo() : invObj.getInvoiceId());
                            dt.Rows[rowCount]["Invoice Date"] = invObj.getInvoiceDate();
                            dt.Rows[rowCount]["Product Amount"] = purchaseAmount;

                            rowCount++;

                            if (!prodPurchaseAmount.ContainsKey(poQuoteObj.getProd_srv_category()))
                                prodPurchaseAmount.Add(poQuoteObj.getProd_srv_category(), purchaseAmount);
                            else
                                prodPurchaseAmount[poQuoteObj.getProd_srv_category()] += purchaseAmount;

                        }
                    }
                }

                lastGeneratedProdDict = new Dictionary<string, Dictionary<string, string>>(); lastGeneratedProdDict.Add(fromDate + "," + toDate, prodDict);
                Dictionary<String, Dictionary<String, float>> lastGeneratedProdSalesAmntDict = new Dictionary<string, Dictionary<string, float>>(); lastGeneratedProdSalesAmntDict.Add(fromDate + "," + toDate, prodPurchaseAmount);

                Session[SessionFactory.ALL_DASHBOARD_TRAN_PURCHASE_PRODUCTWISEPURCHASE_AMNT_PROD_DICT] = lastGeneratedProdDict;
                Session[SessionFactory.ALL_DASHBOARD_TRAN_PURCHASE_PRODUCTWISEPURCHASE_AMNT_PURCHASE_AMNT] = lastGeneratedProdSalesAmntDict;

            }
                Chart_Transaction_Purchase_Prod_Wise_Amount.Titles.Clear();
                Chart_Transaction_Purchase_Prod_Wise_Amount.Series[0].Points.Clear();

            Series transaction_Purchase_Prod_Wise_Amount = Chart_Transaction_Purchase_Prod_Wise_Amount.Series[0];
            transaction_Purchase_Prod_Wise_Amount.IsVisibleInLegend = true;

            int counter = 0;
            foreach (KeyValuePair<String, float> kvp in prodPurchaseAmount)
            {
                    transaction_Purchase_Prod_Wise_Amount.Points.Add(kvp.Value);
                    transaction_Purchase_Prod_Wise_Amount.Points[counter].LegendText = prodDict[kvp.Key];
                counter++;
            }

                Chart_Transaction_Purchase_Prod_Wise_Amount.Titles.Add("Product Wise Purchase by Amount");
                Chart_Transaction_Purchase_Prod_Wise_Amount.Titles[0].Font = new System.Drawing.Font("Book Antiqua", 9);
                Chart_Transaction_Purchase_Prod_Wise_Amount.Series[0].Label = "#PERCENT{P0}(#VALY{0;0;#})";

        }

        protected void generateTotalBusinessForPurchaseByAccounts(String fromDate, String toDate, ArrayList invList, Dictionary<String, String> vendList)
        {
            if (fromDate == null || fromDate.Equals(""))
                fromDate = DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd");
            if (toDate == null || toDate.Equals(""))
                toDate = DateTime.Now.ToString("yyyy-MM-dd");

            bool vendorFilterCheck = (vendList != null && vendList.Count > 0) ? true : false;
            Boolean regenerateValues = false;

            Dictionary<String, String> contactDict = new Dictionary<String, String>();
            Dictionary<String, float> totalBusiness = new Dictionary<string, float>();
            Dictionary<String, float> totalPending = new Dictionary<string, float>();
            Dictionary<String, float> businessDuringPeriod = new Dictionary<string, float>();

            Dictionary<String, Dictionary<String, String>> lastGeneratedContactDict = (Dictionary<String, Dictionary<String, String>>)
    Session[SessionFactory.ALL_DASHBOARD_TRAN_PURCHASE_TOTAL_BUSINESS_BY_ACCNT_CONTACTS];

            if (lastGeneratedContactDict != null && lastGeneratedContactDict.Count > 0 && lastGeneratedContactDict.ContainsKey(fromDate + "," + toDate))
            {
                totalBusiness = ((Dictionary<String, Dictionary<String, float>>)Session[SessionFactory.ALL_DASHBOARD_TRAN_PURCHASE_TOTAL_BUSINESS_BY_ACCNT_TOTAL_BUSINESS])[fromDate + "," + toDate];
                totalPending = ((Dictionary<String, Dictionary<String, float>>)Session[SessionFactory.ALL_DASHBOARD_TRAN_PURCHASE_TOTAL_BUSINESS_BY_ACCNT_TOTAL_PENDING])[fromDate + "," + toDate];
                businessDuringPeriod = ((Dictionary<String, Dictionary<String, float>>)Session[SessionFactory.ALL_DASHBOARD_TRAN_PURCHASE_TOTAL_BUSINESS_BY_ACCNT_TOTAL_BUSINESS_DURING_PERIOD])[fromDate + "," + toDate];

                if (vendorFilterCheck)
                {
                    Dictionary<String, float> totalBusinessTemp = new Dictionary<string, float>();
                    Dictionary<String, float> totalPendingTemp = new Dictionary<string, float>();
                    Dictionary<String, float> businessDuringPeriodTemp = new Dictionary<string, float>();

                    foreach (KeyValuePair<String, String> kvp in vendList)
                    {
                        if (totalBusiness.ContainsKey(kvp.Key))
                            totalBusinessTemp.Add(kvp.Key, totalBusiness[kvp.Key]);
                        else
                            regenerateValues = true;
                        if (totalPending.ContainsKey(kvp.Key))
                            totalPendingTemp.Add(kvp.Key, totalPending[kvp.Key]);
                        else
                            regenerateValues = true;
                        if (businessDuringPeriod.ContainsKey(kvp.Key))
                            businessDuringPeriodTemp.Add(kvp.Key, businessDuringPeriod[kvp.Key]);
                        else
                            regenerateValues = true;
                    }

                    totalBusiness = totalBusinessTemp;
                    totalPending = totalPendingTemp;
                    businessDuringPeriod = businessDuringPeriodTemp;
                }
            }
            if (lastGeneratedContactDict != null && lastGeneratedContactDict.Count > 0 && lastGeneratedContactDict.ContainsKey(fromDate + "," + toDate) && !regenerateValues)
            {
                contactDict = lastGeneratedContactDict[fromDate + "," + toDate];
            }
            else
            {
                totalBusiness = new Dictionary<string, float>();
                totalPending = new Dictionary<string, float>();
                businessDuringPeriod = new Dictionary<string, float>();

                if (invList == null || invList.Count == 0)
                    invList = Invoice.getAllInvoicesbyEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

                for (int i = 0; i < invList.Count; i++)
                {
                    Invoice invObj = (Invoice)invList[i];
                    String vendEntId = invObj.getRespEntityId();
                    bool considerVendorEnt = vendorFilterCheck ? vendList.ContainsKey(vendEntId) : true;

                    if (considerVendorEnt)
                    {
                        if (!contactDict.ContainsKey(vendEntId))
                        {
                            Contacts contactObj = Contacts.
                                getContactDetailsforContactEntityDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), vendEntId);
                            if (contactObj == null || contactObj.getContactName() == null || contactObj.getContactName().Equals(""))
                                contactDict.Add(vendEntId, MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(vendEntId).getEntityName());
                            else
                                contactDict.Add(vendEntId, contactObj.getContactName());
                        }

                        Dictionary<String, Payment> paymentDict = Payment.getPaymentDetailsforInvoiceDB(invObj.getInvoiceId());

                        if (!totalBusiness.ContainsKey(vendEntId))
                            totalBusiness.Add(vendEntId, invObj.getTotalAmount());
                        else
                            totalBusiness[vendEntId] = totalBusiness[vendEntId] + invObj.getTotalAmount();

                        if (!totalPending.ContainsKey(vendEntId))
                            totalPending.Add(vendEntId, totalBusiness[vendEntId]);
                        else
                            totalPending[vendEntId] += totalBusiness[vendEntId];

                        if (!businessDuringPeriod.ContainsKey(vendEntId))
                            businessDuringPeriod.Add(vendEntId, 0);
                        //Total business during the period is the total value of the invoices during the period
                        if (compareDateTime(fromDate, toDate, invObj.getInvoiceDate().Replace(" ", "  ").Substring(0, 10)))
                            businessDuringPeriod[vendEntId] = businessDuringPeriod[vendEntId] + invObj.getTotalAmount();

                        foreach (KeyValuePair<String, Payment> kvp in paymentDict)
                        {
                            Payment pmntObj = kvp.Value;
                            String pmntDate = pmntObj.getPmntDate().Replace(" ", "  ").Substring(0, 10);

                            if (pmntObj.getClearingStat().Equals(Payment.PAYMENT_CLEARING_STAT_CLEAR))
                                totalPending[vendEntId] -= pmntObj.getAmount();
                        }
                    }
                }

                lastGeneratedContactDict = new Dictionary<string, Dictionary<string, string>>(); lastGeneratedContactDict.Add(fromDate + "," + toDate, contactDict);
                Dictionary<String, Dictionary<String, float>> lastGeneratedTotalBusinessDict = new Dictionary<string, Dictionary<string, float>>(); lastGeneratedTotalBusinessDict.Add(fromDate + "," + toDate, totalBusiness);
                Dictionary<String, Dictionary<String, float>> lastGeneratedTotalPendingDict = new Dictionary<string, Dictionary<string, float>>(); lastGeneratedTotalPendingDict.Add(fromDate + "," + toDate, totalPending);
                Dictionary<String, Dictionary<String, float>> lastGeneratedBusinessDuringDict = new Dictionary<string, Dictionary<string, float>>(); lastGeneratedBusinessDuringDict.Add(fromDate + "," + toDate, businessDuringPeriod);

                Session[SessionFactory.ALL_DASHBOARD_TRAN_PURCHASE_TOTAL_BUSINESS_BY_ACCNT_CONTACTS] = lastGeneratedContactDict;
                Session[SessionFactory.ALL_DASHBOARD_TRAN_PURCHASE_TOTAL_BUSINESS_BY_ACCNT_TOTAL_BUSINESS] = lastGeneratedTotalBusinessDict;
                Session[SessionFactory.ALL_DASHBOARD_TRAN_PURCHASE_TOTAL_BUSINESS_BY_ACCNT_TOTAL_PENDING] = lastGeneratedTotalPendingDict;
                Session[SessionFactory.ALL_DASHBOARD_TRAN_PURCHASE_TOTAL_BUSINESS_BY_ACCNT_TOTAL_BUSINESS_DURING_PERIOD] = lastGeneratedBusinessDuringDict;
            }
            //Now populate the List box with vendor details
            foreach (KeyValuePair<String, String> kvp in contactDict)
            {
                ListItem ltExists = ListBox_Contacts_Total_Business_Chart_Purchase.Items.FindByValue(kvp.Key);
                if (ltExists == null || ltExists.Text.Equals(""))
                {
                    ltExists = new ListItem();
                    ltExists.Text = kvp.Value;
                    ltExists.Value = kvp.Key;

                    ListBox_Contacts_Total_Business_Chart_Purchase.Items.Add(ltExists);
                }
            }

            String[] vendorArray = new String[contactDict.Count];
            float[] totalBusinessArray = new float[contactDict.Count];
            float[] totalPendingArray = new float[contactDict.Count];
            float[] businessDuringPeriodArray = new float[contactDict.Count];

            int counter = 0;
            foreach (KeyValuePair<String, float> kvp in totalBusiness)
            {
                vendorArray[counter] = contactDict[kvp.Key];
                totalBusinessArray[counter] = totalBusiness[kvp.Key];
                totalPendingArray[counter] = totalPending[kvp.Key];
                businessDuringPeriodArray[counter] = businessDuringPeriod[kvp.Key];
                counter++;
            }

            Chart_Purchase_Total_Business_Contact.Titles.Clear();
            Chart_Purchase_Total_Business_Contact.Titles.Add("Business during this period (by Invoice date),Total Business (till Date) and Total Pending Amount (till Date) for Vendors");
            Chart_Purchase_Total_Business_Contact.Titles[0].Font = new System.Drawing.Font("Book Antiqua", 10);
            Chart_Purchase_Total_Business_Contact.Series["TotalBusiness"].Label = "#VALY{0;0;#}";
            Chart_Purchase_Total_Business_Contact.Series["TotalBusiness"].Points.DataBindXY(vendorArray, totalBusinessArray);
            Chart_Purchase_Total_Business_Contact.Series["BusinessDuringTimeSpan"].Label = "#VALY{0;0;#}";
            Chart_Purchase_Total_Business_Contact.Series["BusinessDuringTimeSpan"].Points.DataBindXY(vendorArray, businessDuringPeriodArray);
            Chart_Purchase_Total_Business_Contact.Series["TotalPendingAmount"].Label = "#VALY{0;0;#}";
            Chart_Purchase_Total_Business_Contact.Series["TotalPendingAmount"].Points.DataBindXY(vendorArray, totalPendingArray);

        }

        protected ArrayList generatePendingClearPaymentsForPurchaseByAccounts(String fromDate, String toDate, ArrayList invList, Dictionary<String, String> vendList)
        {
            if (fromDate == null || fromDate.Equals(""))
                fromDate = DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd");
            if (toDate == null || toDate.Equals(""))
                toDate = DateTime.Now.ToString("yyyy-MM-dd");

            bool custFilterCheck = (vendList != null && vendList.Count > 0) ? true : false;
            bool regenerateValues = false;

            Dictionary<String, String> contactDict = new Dictionary<String, String>();
            Dictionary<String, float> totalCleared = new Dictionary<string, float>();
            Dictionary<String, float> totalNotCleared = new Dictionary<string, float>();

            Dictionary<String, Dictionary<String, String>> lastGeneratedContactDict = (Dictionary<String, Dictionary<String, String>>)
    Session[SessionFactory.ALL_DASHBOARD_TRAN_PURCHASE_PENDINGCLEARPMNT_CONTACT];

            if (lastGeneratedContactDict != null && lastGeneratedContactDict.Count > 0 && lastGeneratedContactDict.ContainsKey(fromDate + "," + toDate))
            {
                totalCleared = ((Dictionary<String, Dictionary<String, float>>)Session[SessionFactory.ALL_DASHBOARD_TRAN_PURCHASE_PENDINGCLEARPMNT_TOTAL_CLEARED])[fromDate + "," + toDate];
                totalNotCleared = ((Dictionary<String, Dictionary<String, float>>)Session[SessionFactory.ALL_DASHBOARD_TRAN_PURCHASE_PENDINGCLEARPMNT_TOTAL_NOT_CLEARED])[fromDate + "," + toDate];

                if (custFilterCheck)
                {
                    Dictionary<String, float> totalClearedTemp = new Dictionary<string, float>();
                    Dictionary<String, float> totalNotClearedTemp = new Dictionary<string, float>();

                    foreach (KeyValuePair<String, String> kvp in vendList)
                    {
                        if (totalCleared.ContainsKey(kvp.Key))
                            totalClearedTemp.Add(kvp.Key, totalCleared[kvp.Key]);
                        else
                            regenerateValues = true;
                        if (totalNotCleared.ContainsKey(kvp.Key))
                            totalNotClearedTemp.Add(kvp.Key, totalNotCleared[kvp.Key]);
                        else
                            regenerateValues = true;
                    }

                    //No need to put these updated dictionaries in the session now. The session variable will always contain a superset with other contact names
                    totalCleared = totalClearedTemp;
                    totalNotCleared = totalNotClearedTemp;
                }
            }

            if (lastGeneratedContactDict != null && lastGeneratedContactDict.Count > 0 && lastGeneratedContactDict.ContainsKey(fromDate + "," + toDate) && !regenerateValues)
            {
                contactDict = lastGeneratedContactDict[fromDate + "," + toDate];
            }
            else
            {
                totalCleared = new Dictionary<string, float>();
                totalNotCleared = new Dictionary<string, float>();

                if (invList == null || invList.Count == 0)
                    invList = Invoice.getAllInvoicesbyEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

                int rowCount = 0;
                DataTable dt = new DataTable();
                dt.Columns.Add("Contact Name");
                dt.Columns.Add("Invoice#");
                dt.Columns.Add("RFQ#");
                dt.Columns.Add("Invoice Date");
                dt.Columns.Add("Payment Dates [for cleared]");
                dt.Columns.Add("Total Invoice Amount");
                dt.Columns.Add("Cleared Amount");
                dt.Columns.Add("Non-Cleared Amount");
                dt.Columns.Add("Total Pending (Including Non-Cleared)");

                for (int i = 0; i < invList.Count; i++)
                {
                    Invoice invObj = (Invoice)invList[i];

                    String vendEntId = invObj.getRespEntityId();
                    bool considerVendorEnt = custFilterCheck ? vendList.ContainsKey(vendEntId) : true;

                    if (considerVendorEnt)
                    {
                        if (!contactDict.ContainsKey(vendEntId))
                        {
                            Contacts contactObj = Contacts.
                                getContactDetailsforContactEntityDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), vendEntId);
                            if (contactObj == null || contactObj.getContactName() == null || contactObj.getContactName().Equals(""))
                                contactDict.Add(vendEntId, MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(vendEntId).getEntityName());
                            else
                                contactDict.Add(vendEntId, contactObj.getContactName());
                        }

                        dt.Rows.Add();
                        dt.Rows[rowCount]["Contact Name"] = contactDict[vendEntId];
                        dt.Rows[rowCount]["Invoice#"] = (invObj.getInvoiceNo() != null && invObj.getInvoiceNo().Equals("") ? invObj.getInvoiceNo() : invObj.getInvoiceId());
                        dt.Rows[rowCount]["RFQ#"] = invObj.getRFQId();
                        dt.Rows[rowCount]["Invoice Date"] = invObj.getInvoiceDate();
                        dt.Rows[rowCount]["Total Invoice Amount"] = invObj.getTotalAmount();

                        float totalClearedForInvoice = 0;
                        float totalNonClearedForInvoice = 0;
                        String pmntDates = "";

                        Dictionary<String, Payment> paymentDict = Payment.getPaymentDetailsforInvoiceDB(invObj.getInvoiceId());

                        foreach (KeyValuePair<String, Payment> kvp in paymentDict)
                        {
                            Payment pmntObj = kvp.Value;
                            String pmntDate = pmntObj.getPmntDate().Replace(" ", "  ").Substring(0, 10);

                            if (compareDateTime(fromDate, toDate, pmntDate))
                            {
                                if (pmntObj.getClearingStat().Equals(Payment.PAYMENT_CLEARING_STAT_CLEAR))
                                {
                                    if (totalCleared.ContainsKey(vendEntId))
                                        totalCleared[vendEntId] = totalCleared[vendEntId] + pmntObj.getAmount();
                                    else
                                        totalCleared.Add(vendEntId, pmntObj.getAmount());

                                    totalClearedForInvoice += pmntObj.getAmount();
                                    pmntDates = pmntDates.Equals("") ? pmntDate : pmntDates + "," + pmntDate;
                                    //Make sure both the dictionaries have same number of elements
                                    if (!totalNotCleared.ContainsKey(vendEntId))
                                        totalNotCleared.Add(vendEntId, 0);
                                }
                                else
                                {
                                    if (totalNotCleared.ContainsKey(vendEntId))
                                        totalNotCleared[vendEntId] = totalNotCleared[vendEntId] + pmntObj.getAmount();
                                    else
                                        totalNotCleared.Add(vendEntId, pmntObj.getAmount());

                                    totalNonClearedForInvoice += pmntObj.getAmount();
                                    if (!totalCleared.ContainsKey(vendEntId))
                                        totalCleared.Add(vendEntId, 0);
                                }
                            }
                        }

                        dt.Rows[rowCount]["Payment Dates [for cleared]"] = pmntDates;
                        dt.Rows[rowCount]["Cleared Amount"] = totalClearedForInvoice;
                        dt.Rows[rowCount]["Non-Cleared Amount"] = totalNonClearedForInvoice;
                        dt.Rows[rowCount]["Total Pending (Including Non-Cleared)"] = invObj.getTotalAmount() - totalClearedForInvoice;
                        rowCount++;

                    }
                }

                Dictionary<String, DataTable> reportDict = (Dictionary<String, DataTable>)
Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES];

                if (reportDict == null)
                    reportDict = new Dictionary<string, DataTable>();

                if (!reportDict.ContainsKey("pending_clear_contact_purchase"))
                    reportDict.Add("pending_clear_contact_purchase", dt);
                else
                    reportDict["pending_clear_contact_purchase"] = dt;

                Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES] = reportDict;

                lastGeneratedContactDict = new Dictionary<string, Dictionary<string, string>>(); lastGeneratedContactDict.Add(fromDate + "," + toDate, contactDict);
                Dictionary<String, Dictionary<String, float>> lastGeneratedtotalClearedDict = new Dictionary<string, Dictionary<string, float>>(); lastGeneratedtotalClearedDict.Add(fromDate + "," + toDate, totalCleared);
                Dictionary<String, Dictionary<String, float>> lastGeneratedtotalNotClearedDict = new Dictionary<string, Dictionary<string, float>>(); lastGeneratedtotalNotClearedDict.Add(fromDate + "," + toDate, totalNotCleared);

                Session[SessionFactory.ALL_DASHBOARD_TRAN_PURCHASE_PENDINGCLEARPMNT_CONTACT] = lastGeneratedContactDict;
                Session[SessionFactory.ALL_DASHBOARD_TRAN_PURCHASE_PENDINGCLEARPMNT_TOTAL_CLEARED] = lastGeneratedtotalClearedDict;
                Session[SessionFactory.ALL_DASHBOARD_TRAN_PURCHASE_PENDINGCLEARPMNT_TOTAL_NOT_CLEARED] = lastGeneratedtotalNotClearedDict;

            }
            //Now populate the List box with vendor details
            foreach (KeyValuePair<String, String> kvp in contactDict)
            {
                ListItem ltExists = ListBox_Contacts_Pending_Clear_Amnt_Purchase.Items.FindByValue(kvp.Key);
                if (ltExists == null || ltExists.Text.Equals(""))
                {
                    ltExists = new ListItem();
                    ltExists.Text = kvp.Value;
                    ltExists.Value = kvp.Key;

                    ListBox_Contacts_Pending_Clear_Amnt_Purchase.Items.Add(ltExists);
                }
            }

            float[] dtCleared = new float[contactDict.Count];
            float[] dtNCleared = new float[contactDict.Count];
            String[] contactArray = new String[contactDict.Count];

            int counter = 0;
            foreach (KeyValuePair<String, float> kvp in totalCleared)
            {
                contactArray[counter] = contactDict[kvp.Key];
                dtCleared[counter] = kvp.Value;
                dtNCleared[counter] = totalNotCleared[kvp.Key];
                counter++;
            }
            Chart_Purchase_Pending_Clear_By_Account.Titles.Clear();
            Chart_Purchase_Pending_Clear_By_Account.Titles.Add("Cleared and Not-cleared amount for different vendors during the period (By Payment Date)");
            Chart_Purchase_Pending_Clear_By_Account.Titles[0].Font = new System.Drawing.Font("Book Antiqua", 10);
            Chart_Purchase_Pending_Clear_By_Account.Series["clearedSeries"].Label = "#VALY{0;0;#}";
            Chart_Purchase_Pending_Clear_By_Account.Series["clearedSeries"].Points.DataBindXY(contactArray, dtCleared);
            Chart_Purchase_Pending_Clear_By_Account.Series["NotClearedSeries"].Label = "#VALY{0;0;#}";
            Chart_Purchase_Pending_Clear_By_Account.Series["NotClearedSeries"].Points.DataBindXY(contactArray, dtNCleared);

            return invList;
        }

        protected void generateLeadConvbyVal(String fromDate,String toDate,ArrayList leadList,Dictionary<String,String> potDict)
        {
            if(fromDate==null ||fromDate.Equals(""))
            fromDate = DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd");
            if(toDate==null ||toDate.Equals(""))
            toDate = DateTime.Now.ToString("yyyy-MM-dd");

            Dictionary<String, String> leadConvValLatestDict = new Dictionary<String, String>();
            float successVal = 0;
            float failureVal = 0;
            DataTable dt = new DataTable();

            Dictionary<String,LeadandPotential> lastGeneratedLeadPotn=(Dictionary<String,LeadandPotential>)
            Session[SessionFactory.ALL_DASHBOARD_LEAD_AND_POTENTIAL_DICTIONARY];

            if (lastGeneratedLeadPotn != null && lastGeneratedLeadPotn.Count > 0 && lastGeneratedLeadPotn.ContainsKey(fromDate + "," + toDate))
            {
                leadConvValLatestDict = (Dictionary<String, String>)Session[SessionFactory.ALL_DASHBOARD_LEAD_BY_VAL_SUCCESS_FAILURE];

                if (leadConvValLatestDict != null && leadConvValLatestDict.Count > 0)
                {
                    successVal = float.Parse(leadConvValLatestDict["success"]);
                    failureVal = float.Parse(leadConvValLatestDict["failure"]);
                }
            }

            Label_Message_Lead_Val.Visible = false;

            //If new calculation required
            if(leadConvValLatestDict==null || leadConvValLatestDict.Count==0)
            {

            if (leadList == null || leadList.Count == 0 || potDict == null || potDict.Count == 0)
            {
                LeadandPotential leadPotObj = generateLeadListandPotDict(leadList, potDict, fromDate, toDate);
                leadList = leadPotObj.getLeadList();
                potDict = leadPotObj.getPotDict();
                lastGeneratedLeadPotn = new Dictionary<string, LeadandPotential>();
                lastGeneratedLeadPotn.Add(fromDate + "," + toDate, leadPotObj);
                Session[SessionFactory.ALL_DASHBOARD_LEAD_AND_POTENTIAL_DICTIONARY] = lastGeneratedLeadPotn;
            }
            
            ActionLibrary.SalesActions._dispLeads dspLeadObj = new SalesActions._dispLeads();
            
            bool anyEmptyBid=false;

            int reportRowCount = 0;
            dt.Columns.Add("Lead Name");
            dt.Columns.Add("Account Name");
            dt.Columns.Add("Success?");
            dt.Columns.Add("Amount");
            dt.Columns.Add("Submit Date");
            dt.Columns.Add("Due Date");
            dt.Columns.Add("Create Mode");
            dt.Columns.Add("Terms and Conditions");

            for (int i = 0; i < leadList.Count; i++)
            {
                String rfqId = ((LeadRecord)leadList[i]).getRFQId();
                LeadRecord leadObj=((LeadRecord)leadList[i]);
                
                if (potDict.ContainsKey(rfqId))
                {
                    float tempVal = dspLeadObj.
                        getLeadQuoteandSpecDetails(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), rfqId).getTotalAmnt();
                    if (tempVal == 0)
                        anyEmptyBid = true;

                        dt.Rows.Add();
                        dt.Rows[reportRowCount]["Lead name"] = leadObj.getRFQName();
                        dt.Rows[reportRowCount]["Account Name"] = leadObj.getEntityName();
                        dt.Rows[reportRowCount]["Success?"] = "Y";
                        dt.Rows[reportRowCount]["Amount"]=tempVal;
                            dt.Rows[reportRowCount]["Submit Date"]=leadObj.getSubmitDate();
                                dt.Rows[reportRowCount]["Due Date"]=leadObj.getDueDate();
                                    dt.Rows[reportRowCount]["Terms and Conditions"]=leadObj.getTermsandConds();
                                    dt.Rows[reportRowCount]["Create Mode"] = leadObj.getCreateMode();

                        reportRowCount++;
                    successVal += tempVal;
                }
                else
                {
                    float tempVal = dspLeadObj.
                        getLeadQuoteandSpecDetails(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), rfqId).getTotalAmnt();
                    if (tempVal == 0)
                        anyEmptyBid = true;
                    
                        dt.Rows.Add();
                        dt.Rows[reportRowCount]["Lead name"] = leadObj.getRFQName();
                        dt.Rows[reportRowCount]["Account Name"] = leadObj.getEntityName();
                        dt.Rows[reportRowCount]["Success?"] = "N";
                        dt.Rows[reportRowCount]["Amount"] = tempVal;
                        dt.Rows[reportRowCount]["Submit Date"] = leadObj.getSubmitDate();
                        dt.Rows[reportRowCount]["Due Date"] = leadObj.getDueDate();
                        dt.Rows[reportRowCount]["Terms and Conditions"] = leadObj.getTermsandConds();
                        dt.Rows[reportRowCount]["Create Mode"] = leadObj.getCreateMode();

                        reportRowCount++;
                    failureVal += tempVal;
                }
                //leadObj.getSubmitDate().
            }

            if (anyEmptyBid)
            {
                Label_Message_Lead_Val.Visible = true;
                Label_Message_Lead_Val.Text = "Lead Entries Found without Quotes...the above graph only consider entries with quotes";
                Label_Message_Lead_Val.ForeColor = System.Drawing.Color.Red;
            }

            leadConvValLatestDict = new Dictionary<String, String>();
            leadConvValLatestDict.Add("success", successVal.ToString());
            leadConvValLatestDict.Add("failure", failureVal.ToString());

            Dictionary<String, DataTable> reportDict = (Dictionary<String, DataTable>)Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES];

            if (reportDict == null)
                reportDict = new Dictionary<string, DataTable>();

            if (!reportDict.ContainsKey("lead_by_Amnt"))
                reportDict.Add("lead_by_Amnt", dt);
            else
                reportDict["lead_by_Amnt"] = dt;

            Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES] = reportDict;

            }

            Chart_Lead_Conv_By_Val.Titles.Clear();
            Chart_Lead_Conv_By_Val.Series[0].Points.Clear();

            Series leadConvbyValSeries = Chart_Lead_Conv_By_Val.Series[0];
            
            leadConvbyValSeries.IsVisibleInLegend = true;

            leadConvbyValSeries.Points.Add(successVal);
            leadConvbyValSeries.Points[0].LegendText = "Success";
            leadConvbyValSeries.Points.Add(failureVal);
            leadConvbyValSeries.Points[1].LegendText = "Failure";

            
            Chart_Lead_Conv_By_Val.Titles.Add("Lead Conversion % by Amount");
            Chart_Lead_Conv_By_Val.Titles[0].Font = new System.Drawing.Font("Book Antiqua", 9);
            Chart_Lead_Conv_By_Val.Series[0].Label = "#PERCENT{P0}(#VALY{0;0;#})";
            

            Session[SessionFactory.ALL_DASHBOARD_LEAD_BY_VAL_SUCCESS_FAILURE] = leadConvValLatestDict;


        }

        protected void generateLeadConvbyNumber(String fromDate, String toDate,ArrayList leadList, Dictionary<String, String> potDict)
        {
            if (fromDate == null || fromDate.Equals(""))
                fromDate = DateTime.Now.AddMonths(-12).ToString("yyyy-MM-dd");
            if (toDate == null || toDate.Equals(""))
                toDate = DateTime.Now.ToString("yyyy-MM-dd");

            Dictionary<String, String> leadConvNoLatestDict = new Dictionary<String, String>();
            int successCount = 0;
            int failureCount = 0;
            int reportRowCount = 0;
            DataTable dt = new DataTable();
            dt.Columns.Add("Lead Name");
            dt.Columns.Add("Account Name");
            dt.Columns.Add("Success?");
            dt.Columns.Add("Submit Date");
            dt.Columns.Add("Due Date");
            dt.Columns.Add("Create Mode");
            dt.Columns.Add("Terms and Conditions");
            Dictionary<String, LeadandPotential> lastGeneratedLeadPotn = (Dictionary<String, LeadandPotential>)
Session[SessionFactory.ALL_DASHBOARD_LEAD_AND_POTENTIAL_DICTIONARY];

            if (lastGeneratedLeadPotn != null && lastGeneratedLeadPotn.Count > 0 && lastGeneratedLeadPotn.ContainsKey(fromDate + "," + toDate))
            {
                leadConvNoLatestDict = (Dictionary<String, String>)Session[SessionFactory.ALL_DASHBOARD_LEAD_BY_NUMBER_SUCCESS_FAILURE];
                if (leadConvNoLatestDict != null && leadConvNoLatestDict.Count > 0)
                {
                    successCount = Int32.Parse(leadConvNoLatestDict["success"]);
                    failureCount = Int32.Parse(leadConvNoLatestDict["failure"]);
                }
            }

            //New Calculation required
            if (leadConvNoLatestDict == null || leadConvNoLatestDict.Count == 0)
            {

                if (leadList == null || leadList.Count == 0 || potDict == null || potDict.Count == 0)
                {
                    LeadandPotential leadPotObj = generateLeadListandPotDict(leadList, potDict, fromDate, toDate);
                    leadList = leadPotObj.getLeadList();
                    potDict = leadPotObj.getPotDict();
                    lastGeneratedLeadPotn = new Dictionary<string, LeadandPotential>();
                    lastGeneratedLeadPotn.Add(fromDate + "," + toDate, leadPotObj);
                    Session[SessionFactory.ALL_DASHBOARD_LEAD_AND_POTENTIAL_DICTIONARY] = lastGeneratedLeadPotn;
                }
                ActionLibrary.SalesActions._dispLeads dspLeadObj = new SalesActions._dispLeads();

                for (int i = 0; i < leadList.Count; i++)
                {
                    String rfqId = ((LeadRecord)leadList[i]).getRFQId();
                    LeadRecord leadObj = ((LeadRecord)leadList[i]);

                    dt.Rows.Add();
                    dt.Rows[reportRowCount]["Lead name"] = leadObj.getRFQName();
                    dt.Rows[reportRowCount]["Account Name"] = leadObj.getEntityName();
                    dt.Rows[reportRowCount]["Submit Date"] = leadObj.getSubmitDate();
                    dt.Rows[reportRowCount]["Due Date"] = leadObj.getDueDate();
                    dt.Rows[reportRowCount]["Terms and Conditions"] = leadObj.getTermsandConds();
                    dt.Rows[reportRowCount]["Create Mode"] = leadObj.getCreateMode();

                    if (potDict.ContainsKey(rfqId))
                    {
                        dt.Rows[reportRowCount]["Success?"] = "Y";
                        successCount++;
                    }
                    else
                    {
                        failureCount++;
                        dt.Rows[reportRowCount]["Success?"] = "N";
                    }
                    reportRowCount++;
                }

                leadConvNoLatestDict = new Dictionary<String, String>();
                leadConvNoLatestDict.Add("success", successCount.ToString());
                leadConvNoLatestDict.Add("failure", failureCount.ToString());

                Dictionary<String, DataTable> reportDict = (Dictionary<String, DataTable>)Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES];

                if (reportDict == null)
                    reportDict = new Dictionary<string, DataTable>();

                if (!reportDict.ContainsKey("lead_by_Number"))
                    reportDict.Add("lead_by_Number", dt);
                else
                    reportDict["lead_by_Number"] = dt;

                Session[SessionFactory.ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES] = reportDict;
            }

            Chart_Lead_Conv_By_Number.Titles.Clear();
            Chart_Lead_Conv_By_Number.Series[0].Points.Clear();

            Series leadConvbyNumberSeries = Chart_Lead_Conv_By_Number.Series[0];

            leadConvbyNumberSeries.IsVisibleInLegend = true;

            leadConvbyNumberSeries.Points.Add(successCount);
            leadConvbyNumberSeries.Points[0].LegendText = "Success";
            leadConvbyNumberSeries.Points.Add(failureCount);
            leadConvbyNumberSeries.Points[1].LegendText = "Failure";

            Chart_Lead_Conv_By_Number.Titles.Add("Lead Conversion % by Numbers");
            Chart_Lead_Conv_By_Number.Titles[0].Font = new System.Drawing.Font("Book Antiqua", 9);
            Chart_Lead_Conv_By_Number.Series[0].Label = "#PERCENT{P0}(#VALY{0;0;#})";
            

            Session[SessionFactory.ALL_DASHBOARD_LEAD_BY_NUMBER_SUCCESS_FAILURE] = leadConvNoLatestDict;

        }
        

        protected void Button_Lead_Conv_Val_Show_Click(object sender, EventArgs e)
        {            
            generateLeadConvbyVal(TextBox_From_Date_Lead_Val.Text, TextBox_To_Date_Lead_Val.Text, new ArrayList(), new Dictionary<String, String>());
            
            //re-create the other lead chart from last saved value - its getting refreshed otherwise
            /*Dictionary<String, String> successFailDict = (Dictionary<String,String>)
                Session[SessionFactory.ALL_DASHBOARD_LEAD_BY_NUMBER_SUCCESS_FAILURE];

            int successCount =int.Parse(successFailDict["success"]);
            int failureCount =int.Parse(successFailDict["failure"]);

            Chart_Lead_Conv_By_Number.Titles.Clear();
            Chart_Lead_Conv_By_Number.Series[0].Points.Clear();

            Series leadConvbyNumberSeries = Chart_Lead_Conv_By_Number.Series[0];

            leadConvbyNumberSeries.IsVisibleInLegend = true;

            leadConvbyNumberSeries.Points.Add(successCount);
            leadConvbyNumberSeries.Points[0].LegendText = "Success";
            leadConvbyNumberSeries.Points.Add(failureCount);
            leadConvbyNumberSeries.Points[1].LegendText = "Failure";

            Chart_Lead_Conv_By_Number.Titles.Add("Lead Conversion % by Numbers");
            Chart_Lead_Conv_By_Number.Series[0].Label = "#PERCENT{P0}(#VALY{0;0;#})";

            Button_Lead_Conv_Val_Show.Focus();*/
        }


        protected void Button_Lead_Conv_Val_Show0_Click(object sender, EventArgs e)
        {
            generateLeadConvbyNumber(TextBox_From_Date_Lead_Amnt.Text, TextBox_To_Date_Lead_Amnt.Text, new ArrayList(), new Dictionary<String, String>());

            //re-create the other lead chart from last saved value - its getting refreshed otherwise
      /*      Dictionary<String, String> successFailDict = (Dictionary<String,String>)
                Session[SessionFactory.ALL_DASHBOARD_LEAD_BY_VAL_SUCCESS_FAILURE];

            float successVal =float.Parse(successFailDict["success"]);
            float failureVal = float.Parse(successFailDict["failure"]);

            Chart_Lead_Conv_By_Val.Titles.Clear();
            Chart_Lead_Conv_By_Val.Series[0].Points.Clear();

            Series leadConvbyValSeries = Chart_Lead_Conv_By_Val.Series[0];

            leadConvbyValSeries.IsVisibleInLegend = true;

            leadConvbyValSeries.Points.Add(successVal);
            leadConvbyValSeries.Points[0].LegendText = "Success";
            leadConvbyValSeries.Points.Add(failureVal);
            leadConvbyValSeries.Points[1].LegendText = "Failure";
            
            Chart_Lead_Conv_By_Val.Titles.Add("Lead Conversion % by Value");
            Chart_Lead_Conv_By_Val.Series[0].Label = "#PERCENT{P0}(#VALY{0;0;#})";

            Button_Lead_Conv_Val_Show0.Focus();*/
        }

        protected void Button_Pending_Clear_Contact_Click(object sender, EventArgs e)
        {
            String fromDate = TextBox_From_Date_Pending_Clear_Contact.Text;
            String toDate = TextBox_To_Date_Pending_Clear_Contact.Text;

            //This dictionary will have the customer id as the key and the name as the value
            Dictionary<String, String> custDict = new Dictionary<String, String>();

            if (ListBox_Contacts_Pending_Clear_Amnt_Sales.GetSelectedIndices().Length != 0)
            {
                int[] indexList=ListBox_Contacts_Pending_Clear_Amnt_Sales.GetSelectedIndices();

                for (int i = 0; i < indexList.Length; i++)
                    custDict.Add(ListBox_Contacts_Pending_Clear_Amnt_Sales.Items[indexList[i]].Value, ListBox_Contacts_Pending_Clear_Amnt_Sales.Items[indexList[i]].Text);
                
            }

            generatePendingClearPaymentsForSalesByAccounts(fromDate, toDate, new ArrayList(), custDict);

            Button_Pending_Clear_Contact.Focus();
        }

        protected void Button_Chart_Sales_Total_Business_Contact_Click(object sender, EventArgs e)
        {
            String fromDate = TextBox_From_Date_Chart_Sales_Total_Business_Contact.Text;
            String toDate = TextBox_To_Date_Chart_Sales_Total_Business_Contact.Text;

            //This dictionary will have the customer id as the key and the name as the value
            Dictionary<String, String> custDict = new Dictionary<String, String>();

            if (ListBox_Contacts_Total_Business_Chart.GetSelectedIndices().Length != 0)
            {
                int[] indexList = ListBox_Contacts_Total_Business_Chart.GetSelectedIndices();

                for (int i = 0; i < indexList.Length; i++)
                    custDict.Add(ListBox_Contacts_Total_Business_Chart.Items[indexList[i]].Value, ListBox_Contacts_Total_Business_Chart.Items[indexList[i]].Text);

            }

            generateTotalBusinessForSalesByAccounts(fromDate,toDate,new ArrayList(),custDict);

            Button_Chart_Sales_Total_Business_Contact.Focus();
        }

        protected void Button_Prod_Wise_Sales_Qnty_Click(object sender, EventArgs e)
        {
            String fromDate = TextBox_From_Date_Prod_Wise_Sales_Qnty.Text;
            String toDate = TextBox_To_Date_Prod_Wise_Sales_Qnty.Text;
            generateProductWiseSalesQnty(fromDate, toDate, new ArrayList());

            Button_Prod_Wise_Sales_Qnty.Focus();
        }

        protected void Button_Prod_Wise_Sales_Amount_Click(object sender, EventArgs e)
        {
            String fromDate = TextBox_From_Date_Prod_Wise_Sales_Amount.Text;
            String toDate = TextBox_To_Date_Prod_Wise_Sales_Amount.Text;
            generateProductWiseSalesAmnt(fromDate, toDate, new ArrayList());

            Button_Prod_Wise_Sales_Amount.Focus();
        }

        protected void Button_Pending_Clear_Contact_Purchase_Click(object sender, EventArgs e)
        {
            String fromDate = TextBox_From_Date_Pending_Clear_Contact_Purchase.Text;
            String toDate = TextBox_To_Date_Pending_Clear_Contact_Purchase.Text;

            //This dictionary will have the vendor id as the key and the name as the value
            Dictionary<String, String> vendDict = new Dictionary<String, String>();

            if (ListBox_Contacts_Pending_Clear_Amnt_Purchase.GetSelectedIndices().Length != 0)
            {
                int[] indexList = ListBox_Contacts_Pending_Clear_Amnt_Purchase.GetSelectedIndices();

                for (int i = 0; i < indexList.Length; i++)
                    vendDict.Add(ListBox_Contacts_Pending_Clear_Amnt_Purchase.Items[indexList[i]].Value,
                        ListBox_Contacts_Pending_Clear_Amnt_Purchase.Items[indexList[i]].Text);

            }

            generatePendingClearPaymentsForPurchaseByAccounts(fromDate, toDate, new ArrayList(), vendDict);

            Button_Pending_Clear_Contact_Purchase.Focus();
        }


        protected void Button_Prod_Wise_Purchase_Amount_Click(object sender, EventArgs e)
        {
            String fromDate = TextBox_From_Date_Prod_Wise_Purchase_Amount.Text;
            String toDate = TextBox_To_Date_Prod_Wise_Purchase_Amount.Text;
            generateProductWisePurchaseAmnt(fromDate, toDate, new ArrayList());

            Button_Prod_Wise_Purchase_Amount.Focus();
        }

        protected void Button_Prod_Wise_Purchase_Qnty_Click(object sender, EventArgs e)
        {
            String fromDate = TextBox_From_Date_Prod_Wise_Purchase_Qnty.Text;
            String toDate = TextBox_To_Date_Prod_Wise_Purchase_Qnty.Text;
            generateProductWisePurchaseQnty(fromDate, toDate, new ArrayList());

            Button_Prod_Wise_Purchase_Qnty.Focus();
        }

        protected void Button_Chart_Purchase_Total_Business_Contact_Click(object sender, EventArgs e)
        {
            String fromDate = TextBox_From_Date_Chart_Purchase_Total_Business_Contact.Text;
            String toDate = TextBox_To_Date_Chart_Purchase_Total_Business_Contact.Text;

            //This dictionary will have the customer id as the key and the name as the value
            Dictionary<String, String> vendDict = new Dictionary<String, String>();

            if (ListBox_Contacts_Total_Business_Chart_Purchase.GetSelectedIndices().Length != 0)
            {
                int[] indexList = ListBox_Contacts_Total_Business_Chart_Purchase.GetSelectedIndices();

                for (int i = 0; i < indexList.Length; i++)
                    vendDict.Add(ListBox_Contacts_Total_Business_Chart_Purchase.Items[indexList[i]].Value,
                        ListBox_Contacts_Total_Business_Chart_Purchase.Items[indexList[i]].Text);

            }

            generateTotalBusinessForPurchaseByAccounts(fromDate, toDate, new ArrayList(), vendDict);

            Button_Chart_Purchase_Total_Business_Contact.Focus();
        }

        protected void Button_Incoming_Defect_By_Account_Click(object sender, EventArgs e)
        {
            String fromDate = Textbox_From_Date_Incoming_Defect_By_Account.Text;
            String toDate = Textbox_To_Date_Incoming_Defect_By_Account.Text;

            //This dictionary will have the customer id as the key and the name as the value
            Dictionary<String, String> custDict = new Dictionary<String, String>();

            if (ListBox_Incoming_Defect_By_Account.GetSelectedIndices().Length != 0)
            {
                int[] indexList = ListBox_Incoming_Defect_By_Account.GetSelectedIndices();

                for (int i = 0; i < indexList.Length; i++)
                    custDict.Add(ListBox_Incoming_Defect_By_Account.Items[indexList[i]].Value,
                        ListBox_Incoming_Defect_By_Account.Items[indexList[i]].Text);

            }

            generateDefectValByAccount(fromDate, toDate, null, custDict);

            Button_Incoming_Defect_By_Account.Focus();
        }

        protected void Button_Incoming_Defect_No_By_Account_Click(object sender, EventArgs e)
        {
            String fromDate = Textbox_From_Date_Incoming_Defect_No_By_Account.Text;
            String toDate = Textbox_To_Date_Incoming_Defect_No_By_Account.Text;

            //This dictionary will have the customer id as the key and the name as the value
            Dictionary<String, String> custDict = new Dictionary<String, String>();

            if (Listbox_Incoming_Defect_No_By_Account.GetSelectedIndices().Length != 0)
            {
                int[] indexList = Listbox_Incoming_Defect_No_By_Account.GetSelectedIndices();

                for (int i = 0; i < indexList.Length; i++)
                    custDict.Add(Listbox_Incoming_Defect_No_By_Account.Items[indexList[i]].Value,
                        Listbox_Incoming_Defect_No_By_Account.Items[indexList[i]].Text);

            }

            generateDefectNoByAccount(fromDate, toDate, null, custDict,DropDownList_Incoming_Defect_No_By_Account_Defect_Type.SelectedValue);

            Button_Incoming_Defect_No_By_Account.Focus();
        }

        protected void Button_Outgoing_Defect_By_Account_Click(object sender, EventArgs e)
        {
            String fromDate = Textbox_From_Date_Outgoing_Defect_By_Account.Text;
            String toDate = Textbox_To_Date_Outgoing_Defect_By_Account.Text;

            //This dictionary will have the customer id as the key and the name as the value
            Dictionary<String, String> custDict = new Dictionary<String, String>();

            if (ListBox_Outgoing_Defect_By_Account.GetSelectedIndices().Length != 0)
            {
                int[] indexList = ListBox_Outgoing_Defect_By_Account.GetSelectedIndices();

                for (int i = 0; i < indexList.Length; i++)
                    custDict.Add(ListBox_Outgoing_Defect_By_Account.Items[indexList[i]].Value,
                        ListBox_Outgoing_Defect_By_Account.Items[indexList[i]].Text);

            }

            generateOutgoingDefectValByAccount(fromDate, toDate, null, custDict);

            Button_Outgoing_Defect_By_Account.Focus();
        }

        protected void Button_Outgoing_Defect_No_By_Account_Click(object sender, EventArgs e)
        {

            String fromDate = Textbox_From_Date_Outgoing_Defect_No_By_Account.Text;
            String toDate = Textbox_To_Date_Outgoing_Defect_No_By_Account.Text;

            //This dictionary will have the customer id as the key and the name as the value
            Dictionary<String, String> custDict = new Dictionary<String, String>();

            if (Listbox_Outgoing_Defect_No_By_Account.GetSelectedIndices().Length != 0)
            {
                int[] indexList = Listbox_Outgoing_Defect_No_By_Account.GetSelectedIndices();

                for (int i = 0; i < indexList.Length; i++)
                    custDict.Add(Listbox_Outgoing_Defect_No_By_Account.Items[indexList[i]].Value,
                        Listbox_Outgoing_Defect_No_By_Account.Items[indexList[i]].Text);

            }

            generateOutgoingDefectNoByAccount(fromDate, toDate, null, custDict);

            Button_Outgoing_Defect_No_By_Account.Focus();
        }

        protected void Button_Lead_Conv_Val_Download_Click(object sender, EventArgs e)
        {
           // generateLeadConvbyVal(TextBox_From_Date_Lead_Val.Text, TextBox_To_Date_Lead_Val.Text, new ArrayList(), new Dictionary<String, String>(), true);

            String forwardString = "/Pages/Popups/Dashboard/ReportGen.aspx";

            forwardString += "?sessionDict=" + "lead_by_Amnt";
            forwardString += "&fileName=" + "lead_By_Amnt";
            forwardString += "&heading=" + "All Success and Failures By Amount";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispLeadByAmntReport",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=400');", true);
        }

        protected void Button_Lead_Conv_No_Download_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/Popups/Dashboard/ReportGen.aspx";

            forwardString += "?sessionDict=" + "lead_by_Number";
            forwardString += "&fileName=" + "lead_By_Number";
            forwardString += "&heading=" + "All Success and Failures By Number";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispLeadByNoReport",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=400');", true);
        }

        protected void Button_Potn_Conv_Val_Download_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/Popups/Dashboard/ReportGen.aspx";

            forwardString += "?sessionDict=" + "potn_by_Val";
            forwardString += "&fileName=" + "potn_by_Val";
            forwardString += "&heading=" + "All Success and Failures For Potential By Amount";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispPotnByValReport",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=400');", true);

        }

        protected void Button_Potn_Conv_No_Download_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/Popups/Dashboard/ReportGen.aspx";

            forwardString += "?sessionDict=" + "potn_by_No";
            forwardString += "&fileName=" + "potn_by_No";
            forwardString += "&heading=" + "All Success and Failures For Potential";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispPotnByNoReport",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=400');", true);
        }

        protected void Button_Potn_By_Cat_Download_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/Popups/Dashboard/ReportGen.aspx";

            forwardString += "?sessionDict=" + "potn_by_Cat";
            forwardString += "&fileName=" + "potn_by_Cat";
            forwardString += "&heading=" + "All Product Share in Different Potentials";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispPotnByCatReport",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=400');", true);
        }

        protected void Button_Prod_Wise_Sales_Qnty_Download_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/Popups/Dashboard/ReportGen.aspx";

            forwardString += "?sessionDict=" + "prod_wise_Sale";
            forwardString += "&fileName=" + "prod_wise_Sale";
            forwardString += "&heading=" + "All Product Share in Different Sales Transactions";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispProdWiseSalesReport",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=400');", true);
        }

        protected void Button_Pending_Clear_Contact_Download_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/Popups/Dashboard/ReportGen.aspx";

            forwardString += "?sessionDict=" + "pending_clear_contact_sales";
            forwardString += "&fileName=" + "pending_clear_contact_sales";
            forwardString += "&heading=" + "Cleared and Not-cleared amount from different customers during the period";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispPendingClearContactSalesReport",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=400');", true);
        }

        protected void Button_Prod_Wise_Purchase_Qnty_Download_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/Popups/Dashboard/ReportGen.aspx";

            forwardString += "?sessionDict=" + "prod_wise_Purchase";
            forwardString += "&fileName=" + "prod_wise_Purchase";
            forwardString += "&heading=" + "All Product Share in Different Purchase Transactions";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispProdWisePurchaseReport",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=400');", true);
        }

        protected void Button_Pending_Clear_Contact_Purchase_Download_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/Popups/Dashboard/ReportGen.aspx";

            forwardString += "?sessionDict=" + "pending_clear_contact_purchase";
            forwardString += "&fileName=" + "pending_clear_contact_purchase";
            forwardString += "&heading=" + "Cleared and Not-cleared amount to different vendors during the period";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispPendingClearContactPurchaseReport",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=400');", true);
        }

        protected void Button_Incoming_Defect_By_Account_Download_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/Popups/Dashboard/ReportGen.aspx";

            forwardString += "?sessionDict=" + "incoming_Defect_By_Accnt";
            forwardString += "&fileName=" + "incoming_Defect_By_Accnt";
            forwardString += "&heading=" + "Defects Raised By Customers during this period (defect submit date)";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispIncmDefectByAccntReport",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=400');", true);
        }

        protected void Button_Incoming_Defect_No_By_Account_Download_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/Popups/Dashboard/ReportGen.aspx";

            forwardString += "?sessionDict=" + "incoming_Defect_No_By_Accnt";
            forwardString += "&fileName=" + "incoming_Defect_No_By_Accnt";
            forwardString += "&heading=" + "Defects Raised By Customers during this period By Severity(defect submit date)";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispIncmDefectNoByAccntReport",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=400');", true);
        }

        protected void Button_Outgoing_Defect_By_Account_Download_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/Popups/Dashboard/ReportGen.aspx";

            forwardString += "?sessionDict=" + "outgoing_Defect_By_Accnt";
            forwardString += "&fileName=" + "outgoing_Defect_By_Accnt";
            forwardString += "&heading=" + "Total Invoice Value of Defects for Vendors during this period (defect submit date)";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispOutDefectByAccntReport",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=400');", true);
        }

        protected void Button_Outgoing_Defect_No_By_Account_Download_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/Popups/Dashboard/ReportGen.aspx";

            forwardString += "?sessionDict=" + "outgoing_Defect_No_By_Accnt";
            forwardString += "&fileName=" + "outgoing_Defect_No_By_Accnt";
            forwardString += "&heading=" + "Total Number of Defects for Vendors during this period By Severity(defect submit date)";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispOutDefectNoByAccntReport",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=400');", true);
        }

        protected void Button_Filter_Date_Incoming_Defect_Arrvl_Closure_Click(object sender, EventArgs e)
        {
            generateDefectArrivalandClosure(TextBox_From_Date_Incoming_Defect_Arrvl_Closure.Text, TextBox_To_Date_Incoming_Defect_Arrvl_Closure.Text, null, null, DropDownList_Incm_Defect_Arrival_Closure_Freq.SelectedValue);
        }

        protected void Button_Filter_Date_Incoming_Defect_Avg_Closure_Click(object sender, EventArgs e)
        {
            generateAvgDefectClosureTime(TextBox_From_Date_Incoming_Defect_Avg_Closure.Text, TextBox_To_Date_Incoming_Defect_Avg_Closure.Text, null, DropDownList_Incm_Defect_Avg_Closure_Service_Agnt.SelectedValue, DropDownList_Incm_Defect_Avg_Closure_Freq.SelectedValue);
        }

        protected void Button_Filter_Date_Outgoing_Defect_Arrvl_Closure_Click(object sender, EventArgs e)
        {
            generateDefectArrivalandClosureOutgoing(TextBox_From_Date_Outgoing_Defect_Arrvl_Closure.Text, TextBox_To_Date_Outgoing_Defect_Arrvl_Closure.Text, null, null, DropDownList_Outgoing_Defect_Arrival_Closure_Freq.SelectedValue);
        }

        protected void Button_Filter_Date_Outgoing_Defect_Avg_Closure_Click(object sender, EventArgs e)
        {
            generateAvgDefectClosureTimeOutgoing(TextBox_From_Date_Outgoing_Defect_Avg_Closure.Text, TextBox_To_Date_Outgoing_Defect_Avg_Closure.Text, null, DropDownList_Outg_Defect_Avg_Closure_Vendor.SelectedValue, DropDownList_Outg_Defect_Avg_Closure_Freq.SelectedValue);
        }

        protected void Button_Report_Date_Incoming_Defect_Avg_Closure_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/Popups/Dashboard/ReportGen.aspx";

            forwardString += "?sessionDict=" + "incoming_Defect_Avg_Closure";
            forwardString += "&fileName=" + "incoming_Defect_Avg_Closure";
            forwardString += "&heading=" + "Defects Closure Average Time (Hours)";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispIncmDefectAvgClsReport",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=400');", true);
        }

        protected void Button_Report_Outgoing_Defect_Avg_Closure_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/Popups/Dashboard/ReportGen.aspx";

            forwardString += "?sessionDict=" + "outgoing_Defect_Avg_Closure";
            forwardString += "&fileName=" + "outgoing_Defect_Avg_Closure";
            forwardString += "&heading=" + "Defects Closure Average Time (Hours) By Vendors";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispOutgDefectAvgClsReport",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=400');", true);
        }

        protected void Button_Report_Outgoing_Defect_Arrival_Closure_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/Popups/Dashboard/ReportGen.aspx";

            forwardString += "?sessionDict=" + "outgoing_Defect_Arrival_Closure";
            forwardString += "&fileName=" + "outgoing_Defect_Arrival_Closure";
            forwardString += "&heading=" + "Defects Creation and Closure By Vendors";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispOutgDefectArrvlClsReport",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=400');", true);
        }

        protected void Button_Report_Date_Incoming_Defect_Arrival_Closure_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/Popups/Dashboard/ReportGen.aspx";

            forwardString += "?sessionDict=" + "incoming_Defect_Arrival_Closure";
            forwardString += "&fileName=" + "incoming_Defect_Arrival_Closure";
            forwardString += "&heading=" + "Defects Creation and Closure";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispIncmDefectArrvlClsReport",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=400');", true);
        }

        protected void Button_Filter_Date_Incoming_SR_Arrvl_Closure_Click(object sender, EventArgs e)
        {
            generateSRArrivalandClosure(TextBox_From_Date_Incoming_SR_Arrvl_Closure.Text, TextBox_To_Date_Incoming_SR_Arrvl_Closure.Text, null, null,

            DropDownList_Incm_SR_Arrival_Closure_Freq.SelectedValue);
        }

        protected void Button_Report_Date_Incoming_SR_Arrival_Closure_Click(object sender, EventArgs e)
        {

            String forwardString = "/Pages/Popups/Dashboard/ReportGen.aspx";

            forwardString += "?sessionDict=" + "incoming_SR_Arrival_Closure";
            forwardString += "&fileName=" + "incoming_SR_Arrival_Closure";
            forwardString += "&heading=" + "SRs Creation and Closure";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispIncmSRArrvlClsReport",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=400');", true);
        }

        protected void Button_Filter_Date_Incoming_SR_Avg_Closure_Click(object sender, EventArgs e)
        {
            generateAvgSRClosureTime(TextBox_From_Date_Incoming_SR_Avg_Closure.Text, TextBox_To_Date_Incoming_SR_Avg_Closure.Text, null,

            DropDownList_Incm_SR_Avg_Closure_Service_Agnt.SelectedValue, DropDownList_Incm_SR_Avg_Closure_Freq.SelectedValue);
        }

        protected void Button_Report_Date_Incoming_SR_Avg_Closure_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/Popups/Dashboard/ReportGen.aspx";

            forwardString += "?sessionDict=" + "incoming_SR_Avg_Closure";
            forwardString += "&fileName=" + "incoming_SR_Avg_Closure";
            forwardString += "&heading=" + "SRs Closure Average Time (Hours)";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispIncmSRAvgClsReport",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=400');", true);
        }

        protected void Button_Incoming_SR_By_Account_Click(object sender, EventArgs e)
        {
            String fromDate = Textbox_From_Date_Incoming_SR_By_Account.Text;
            String toDate = Textbox_To_Date_Incoming_SR_By_Account.Text;

            //This dictionary will have the customer id as the key and the name as the value
            Dictionary<String, String> custDict = new Dictionary<String, String>();

            if (ListBox_Incoming_SR_By_Account.GetSelectedIndices().Length != 0)
            {
                int[] indexList = ListBox_Incoming_SR_By_Account.GetSelectedIndices();

                for (int i = 0; i < indexList.Length; i++)
                    custDict.Add(ListBox_Incoming_SR_By_Account.Items[indexList[i]].Value,
                        ListBox_Incoming_SR_By_Account.Items[indexList[i]].Text);

            }

            generateSRValByAccount(fromDate, toDate, null, custDict);

            Button_Incoming_SR_By_Account.Focus();
        }

        protected void Button_Incoming_SR_By_Account_Download_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/Popups/Dashboard/ReportGen.aspx";

            forwardString += "?sessionDict=" + "incoming_SR_By_Accnt";
            forwardString += "&fileName=" + "incoming_SR_By_Accnt";
            forwardString += "&heading=" + "SRs Raised By Customers during this period (SR submit date)";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispIncmSRByAccntReport",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=400');", true);
        }

        protected void Button_Incoming_SR_No_By_Account_Click(object sender, EventArgs e)
        {
            String fromDate = Textbox_From_Date_Incoming_SR_No_By_Account.Text;
            String toDate = Textbox_To_Date_Incoming_SR_No_By_Account.Text;

            //This dictionary will have the customer id as the key and the name as the value
            Dictionary<String, String> custDict = new Dictionary<String, String>();

            if (Listbox_Incoming_SR_No_By_Account.GetSelectedIndices().Length != 0)
            {
                int[] indexList = Listbox_Incoming_SR_No_By_Account.GetSelectedIndices();

                for (int i = 0; i < indexList.Length; i++)
                    custDict.Add(Listbox_Incoming_SR_No_By_Account.Items[indexList[i]].Value,
                        Listbox_Incoming_SR_No_By_Account.Items[indexList[i]].Text);

            }

            generateSRNoByAccount(fromDate, toDate, null, custDict, DropDownList_Incoming_SR_No_By_Account_SR_Type.SelectedValue);

            Button_Incoming_SR_No_By_Account.Focus();
        }

        protected void Button_Incoming_SR_No_By_Account_Download_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/Popups/Dashboard/ReportGen.aspx";

            forwardString += "?sessionDict=" + "incoming_SR_No_By_Accnt";
            forwardString += "&fileName=" + "incoming_SR_No_By_Accnt";
            forwardString += "&heading=" + "SRs Raised By Customers during this period By Severity(SR submit date)";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispIncmSRNoByAccntReport",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=400');", true);
        }

        protected void Button_Filter_Date_Outgoing_SR_Arrvl_Closure_Click(object sender, EventArgs e)
        {
            generateSRArrivalandClosureOutgoing(TextBox_From_Date_Outgoing_SR_Arrvl_Closure.Text, TextBox_To_Date_Outgoing_SR_Arrvl_Closure.Text, null, null, DropDownList_Outg_SR_Arrival_Closure_Freq.SelectedValue);

        }

        protected void Button_Report_Date_Outgoing_SR_Arrival_Closure_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/Popups/Dashboard/ReportGen.aspx";

            forwardString += "?sessionDict=" + "outgoing_SR_Arrival_Closure";
            forwardString += "&fileName=" + "outgoing_SR_Arrival_Closure";
            forwardString += "&heading=" + "SRs Creation and Closure By Vendors";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispOutgSRArrvlClsReport",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=400');", true);

        }

        protected void Button_Filter_Date_Outgoing_SR_Avg_Closure_Click(object sender, EventArgs e)
        {
            generateAvgSRClosureTimeOutgoing(TextBox_From_Date_Outgoing_SR_Avg_Closure.Text, TextBox_To_Date_Outgoing_SR_Avg_Closure.Text, null, DropDownList_Outg_SR_Avg_Closure_Vendor.SelectedValue, DropDownList_Outg_SR_Avg_Closure_Freq.SelectedValue);
        }

        protected void Button_Report_Date_Outgoing_SR_Avg_Closure_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/Popups/Dashboard/ReportGen.aspx";

            forwardString += "?sessionDict=" + "outgoing_SR_Avg_Closure";
            forwardString += "&fileName=" + "outgoing_SR_Avg_Closure";
            forwardString += "&heading=" + "SRs Closure Average Time (Hours) By Vendors";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispOutgSRAvgClsReport",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=400');", true);

        }

        protected void Button_Outgoing_SR_By_Account_Click(object sender, EventArgs e)
        {
            String fromDate = Textbox_From_Date_Outgoing_SR_By_Account.Text;
            String toDate = Textbox_To_Date_Outgoing_SR_By_Account.Text;

            //This dictionary will have the customer id as the key and the name as the value
            Dictionary<String, String> custDict = new Dictionary<String, String>();

            if (ListBox_Outgoing_SR_By_Account.GetSelectedIndices().Length != 0)
            {
                int[] indexList = ListBox_Outgoing_SR_By_Account.GetSelectedIndices();

                for (int i = 0; i < indexList.Length; i++)
                    custDict.Add(ListBox_Outgoing_SR_By_Account.Items[indexList[i]].Value,
                        ListBox_Outgoing_SR_By_Account.Items[indexList[i]].Text);

            }

            generateOutgoingSRValByAccount(fromDate, toDate, null, custDict);

            Button_Outgoing_SR_By_Account.Focus();

        }

        protected void Button_Outgoing_SR_By_Account_Download_Click(object sender, EventArgs e)
        {

            String forwardString = "/Pages/Popups/Dashboard/ReportGen.aspx";

            forwardString += "?sessionDict=" + "outgoing_SR_By_Accnt";
            forwardString += "&fileName=" + "outgoing_SR_By_Accnt";
            forwardString += "&heading=" + "Total Invoice Value of SRs for Vendors during this period (SR submit date)";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispOutSRByAccntReport",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=400');", true);
        }

        protected void Button_Outgoing_SR_No_By_Account_Click(object sender, EventArgs e)
        {

            String fromDate = Textbox_From_Date_Outgoing_SR_No_By_Account.Text;
            String toDate = Textbox_To_Date_Outgoing_SR_No_By_Account.Text;

            //This dictionary will have the customer id as the key and the name as the value
            Dictionary<String, String> custDict = new Dictionary<String, String>();

            if (Listbox_Outgoing_SR_No_By_Account.GetSelectedIndices().Length != 0)
            {
                int[] indexList = Listbox_Outgoing_SR_No_By_Account.GetSelectedIndices();

                for (int i = 0; i < indexList.Length; i++)
                    custDict.Add(Listbox_Outgoing_SR_No_By_Account.Items[indexList[i]].Value,
                        Listbox_Outgoing_SR_No_By_Account.Items[indexList[i]].Text);

            }

            generateOutgoingSRNoByAccount(fromDate, toDate, null, custDict);

            Button_Outgoing_SR_No_By_Account.Focus();
        }

        protected void Button_Outgoing_SR_No_By_Account_Download_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/Popups/Dashboard/ReportGen.aspx";

            forwardString += "?sessionDict=" + "outgoing_SR_No_By_Accnt";
            forwardString += "&fileName=" + "outgoing_SR_No_By_Accnt";
            forwardString += "&heading=" + "Total Number of SRs for Vendors during this period By Severity(SR submit date)";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispOutSRNoByAccntReport",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=400');", true);

        }




    }
}