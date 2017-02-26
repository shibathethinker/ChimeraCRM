using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BackEndObjects;
using ActionLibrary;
using System.Data;
using System.Collections;
using System.IO;
using System.Web.UI.HtmlControls;

namespace OnLine.Pages
{
    public partial class HomePage : System.Web.UI.Page
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            Page.Theme = Session[SessionFactory.LOGGED_IN_USER_THEME].ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING] == null)
                    Response.Redirect("Login.aspx");
                else
                {
                    ((HtmlGenericControl)(Master.FindControl("Home"))).Attributes.Add("class", "active");

                    String entId = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();
                    Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];
                    Dictionary<String, Currency> allCurrList = (Dictionary<String, Currency>)Session[SessionFactory.CURRENCY_LIST];
                    String defaultCurr =Session[SessionFactory.MAIN_BUSINESS_ENTITY_DEFAULT_CURRENCY]!=null?
                        Session[SessionFactory.MAIN_BUSINESS_ENTITY_DEFAULT_CURRENCY].ToString():"";
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
                    Dictionary<String, userDetails> allUserDetails = BackEndObjects.MainBusinessEntity.getUserDetailsforMainEntitybyIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                    Cache.Insert(entId, allUserDetails);
                    loadGrids();
                }
            }
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

        protected void loadGrids()
        {
            loadContacts();
            fillRFQGrid(BackEndObjects.RFQDetails.getAllRFQbyApproverIdDB(User.Identity.Name,Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()));
            fillInvoiceGrid(BackEndObjects.Invoice.getAllInvoicesbyApproverIdAndRespEntIdDB(User.Identity.Name, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()));
            fillLeadGrid(null);
            loadPotStages();
            fillPotnGrid(null);
            loadDefectSevAndSLA(DropDownList_Incm_Defect_Sev, "_");
            loadIncomingDefectList(null);            
        }

        protected void loadPotStages()
        {
            ListItem lt1 = new ListItem();
            lt1.Text = BackEndObjects.PotentialStatus.POTENTIAL_STAT_PRELIM;
            lt1.Value = BackEndObjects.PotentialStatus.POTENTIAL_STAT_PRELIM;

            ListItem lt2 = new ListItem();
            lt2.Text = BackEndObjects.PotentialStatus.POTENTIAL_STAT_MEDIUM;
            lt2.Value = BackEndObjects.PotentialStatus.POTENTIAL_STAT_MEDIUM;

            ListItem lt3 = new ListItem();
            lt3.Text = BackEndObjects.PotentialStatus.POTENTIAL_STAT_ADVNCD;
            lt3.Value = BackEndObjects.PotentialStatus.POTENTIAL_STAT_ADVNCD;

            ListItem lt4 = new ListItem();
            lt4.Text = BackEndObjects.RFQShortlisted.POTENTIAL_ACTIVE_STAT_CLOSED_WON;
            lt4.Value = BackEndObjects.RFQShortlisted.POTENTIAL_ACTIVE_STAT_CLOSED_WON;

            ListItem lt5 = new ListItem();
            lt5.Text = BackEndObjects.RFQShortlisted.POTENTIAL_ACTIVE_STAT_CLOSED_LOST;
            lt5.Value = BackEndObjects.RFQShortlisted.POTENTIAL_ACTIVE_STAT_CLOSED_LOST;

            DropDownList_Pot_Stage_Stat.Items.Add(lt1);
            DropDownList_Pot_Stage_Stat.Items.Add(lt2);
            DropDownList_Pot_Stage_Stat.Items.Add(lt3);
            DropDownList_Pot_Stage_Stat.Items.Add(lt4);
            DropDownList_Pot_Stage_Stat.Items.Add(lt5);

            ListItem listItem1 = new ListItem();
            listItem1.Text = "_";
            listItem1.Value = "_";
            DropDownList_Pot_Stage_Stat.Items.Add(listItem1);
            DropDownList_Pot_Stage_Stat.SelectedValue = "_";

        }

        protected void loadDefectSevAndSLA(DropDownList DropDownList_Defect_Sev, String selectedVal)
        {
            //First load the SLA list
            ArrayList slaList = (ArrayList)Session[SessionFactory.ALL_DEFECT_SLA_LIST];
            if (slaList == null || slaList.Count == 0)
            {
                slaList = DefectSLA.getDefectSLADetailsbyentIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), DefectSLA.DEFECT_TYPE_DEFECT);
                Session[SessionFactory.ALL_DEFECT_SLA_LIST] = slaList;
            }

            ListItem ltEmpty = new ListItem();
            ltEmpty.Text = "_";
            ltEmpty.Value = "_";

            ListItem lt1 = new ListItem();
            lt1.Text = "High";
            lt1.Value = "High";

            ListItem lt2 = new ListItem();
            lt2.Text = "Medium";
            lt2.Text = "Medium";

            ListItem lt3 = new ListItem();
            lt3.Text = "Low";
            lt3.Value = "Low";

            DropDownList_Defect_Sev.Items.Add(ltEmpty);
            DropDownList_Defect_Sev.Items.Add(lt1);
            DropDownList_Defect_Sev.Items.Add(lt2);
            DropDownList_Defect_Sev.Items.Add(lt3);

            //if (selectedVal != null && !selectedVal.Equals(""))
                DropDownList_Defect_Sev.SelectedValue = selectedVal;
            //else
                //DropDownList_Defect_Sev.SelectedValue = "Low";
        }

        protected void loadIncomingDefectList(Dictionary<String, DefectDetails> defectDictPassed)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DefectId");
            dt.Columns.Add("RFQId");
            dt.Columns.Add("InvNo");
            dt.Columns.Add("descr");
            dt.Columns.Add("Submit Date");
            dt.Columns.Add("Submit Date Ticks");
            dt.Columns.Add("Close Date");
            dt.Columns.Add("Close Date Ticks");
            dt.Columns.Add("Amount");
            dt.Columns.Add("CustName");
            dt.Columns.Add("entId");
            dt.Columns.Add("Defect_Stat");
            dt.Columns.Add("Defect_Stat_Reason");
            dt.Columns.Add("Severity");
            dt.Columns.Add("Defect_Resol_Stat");
            //dt.Columns.Add("docName");

            Dictionary<String, DefectDetails> defectDict = new Dictionary<string, DefectDetails>();

            if (defectDictPassed == null)
                defectDict = BackEndObjects.DefectDetails.
                    getAllOpenDefectDetailsforSupplierIdAndAssignedToUserDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(),User.Identity.Name);
            else
                defectDict = defectDictPassed;

            int counter = 0;
            DateUtility dU = new DateUtility();

            foreach (KeyValuePair<String, DefectDetails> kvp in defectDict)
            {
                dt.Rows.Add();

                BackEndObjects.DefectDetails defObj = kvp.Value;

                String custId = defObj.getCustomerId();
                String custName = "";
                if (defObj.getCreationMode().Equals(BackEndObjects.DefectDetails.DEFECT_CREATION_MODE_MANUAL))
                {
                    custName = Contacts.getContactDetailsforContactEntityDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), custId).getContactName();
                    if (custName == null || custName.Equals(""))
                        custName = MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(custId).getEntityName();
                }
                else
                    custName = MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(custId).getEntityName();


                dt.Rows[counter]["DefectId"] = defObj.getDefectId();
                dt.Rows[counter]["RFQId"] = defObj.getRFQId();
                dt.Rows[counter]["InvNo"] = defObj.getInvoiceId();
                dt.Rows[counter]["descr"] = defObj.getDescription();
                dt.Rows[counter]["Submit Date"] = dU.getConvertedDate(defObj.getDateCreated());
                dt.Rows[counter]["Submit Date Ticks"] = Convert.ToDateTime(defObj.getDateCreated()).Ticks;
                dt.Rows[counter]["Close Date"] = dU.getConvertedDate(defObj.getCloseDate() != null && !defObj.getCloseDate().Equals("") && !(defObj.getCloseDate().IndexOf("1/1/1900") >= 0) ? defObj.getCloseDate() : "");
                dt.Rows[counter]["Close Date Ticks"] = (defObj.getCloseDate() != null && !defObj.getCloseDate().Equals("") ? Convert.ToDateTime(defObj.getCloseDate()).Ticks : 0);
                dt.Rows[counter]["Amount"] = defObj.getTotalAmount();
                dt.Rows[counter]["CustName"] = custName;
                dt.Rows[counter]["entId"] = custId;
                dt.Rows[counter]["Defect_Stat"] = defObj.getDefectStat();
                dt.Rows[counter]["Defect_Stat_Reason"] = defObj.getStatReason();
                dt.Rows[counter]["Severity"] = defObj.getSeverity();
                dt.Rows[counter]["Defect_Resol_Stat"] = defObj.getResolStat();

                counter++;
            }
            dt.DefaultView.Sort = "Submit Date Ticks" + " " + "DESC";
            GridView_Incoming_Defects.Visible = true;
            GridView_Incoming_Defects.DataSource = dt.DefaultView.ToTable();
            GridView_Incoming_Defects.DataBind();
            GridView_Incoming_Defects.SelectedIndex = -1;
            disableOnPageChange("defect");
            Session[SessionFactory.HOME_PAGE_ASSGND_OPEN_DEFECTS_DATAGRID] = dt.DefaultView.ToTable();

        }

        protected void loadContacts()
        {
            ArrayList contactList = BackEndObjects.Contacts.
                getAllContactsbyEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

            ListItem emptyItem = new ListItem();
            emptyItem.Text = "_";
            emptyItem.Value = "_";

            DropDownList_Contact_Lead.Items.Add(emptyItem);
            DropDownList_Contact_Potn.Items.Add(emptyItem);
            DropDownList_Contact_Inv.Items.Add(emptyItem);

            for (int i = 0; i < contactList.Count; i++)
            {
                ListItem lt = new ListItem();
                lt.Text = ((BackEndObjects.Contacts)contactList[i]).getContactName();
                lt.Value = ((BackEndObjects.Contacts)contactList[i]).getContactEntityId();

                DropDownList_Contact_Lead.Items.Add(lt);
                DropDownList_Contact_Potn.Items.Add(lt);
                DropDownList_Contact_Inv.Items.Add(lt);
            }

            DropDownList_Contact_Lead.SelectedValue = "_";
            DropDownList_Contact_Potn.SelectedValue = "_";
            DropDownList_Contact_Inv.SelectedValue = "_";

        }

        protected void fillRFQGrid(ArrayList rfqL)
        {
                DataTable dt = new DataTable();
                dt.Columns.Add("RFQNo");
                dt.Columns.Add("RFQName");
                dt.Columns.Add("Specifications");
                dt.Columns.Add("Submit Date");
                dt.Columns.Add("Submit Date Ticks");
                dt.Columns.Add("Due Date");
                dt.Columns.Add("Due Date Ticks");
                dt.Columns.Add("Location");
                dt.Columns.Add("ApprovalStat");
                dt.Columns.Add("BroadcastTo");
                dt.Columns.Add("AllAuditRecords");
                dt.Columns.Add("ActiveStatus");
                
            DateUtility dU = new DateUtility();

                for (int i = 0; i < rfqL.Count; i++)
                {
                    dt.Rows.Add();
                    String rfId = ((BackEndObjects.RFQDetails)rfqL[i]).getRFQId();

                    dt.Rows[i]["RFQNo"] = rfId;
                    dt.Rows[i]["RFQName"] = ((BackEndObjects.RFQDetails)rfqL[i]).getRFQName();
                    dt.Rows[i]["Submit Date"] = dU.getConvertedDate(((BackEndObjects.RFQDetails)rfqL[i]).getSubmitDate());
                    dt.Rows[i]["Submit Date Ticks"] = Convert.ToDateTime(((BackEndObjects.RFQDetails)rfqL[i]).getSubmitDate()).Ticks;
                    dt.Rows[i]["Due Date"] = dU.getConvertedDate(((BackEndObjects.RFQDetails)rfqL[i]).getDueDate().Substring(0, ((BackEndObjects.RFQDetails)rfqL[i]).getDueDate().IndexOf(" ")));
                    dt.Rows[i]["Due Date Ticks"] = !dt.Rows[i]["Due Date"].Equals("") ? Convert.ToDateTime(dt.Rows[i]["Due Date"]).Ticks : 0;
                    dt.Rows[i]["ApprovalStat"] = ((BackEndObjects.RFQDetails)rfqL[i]).getApprovalStat();

                    String activeStat = ((BackEndObjects.RFQDetails)rfqL[i]).getActiveStat();

                    dt.Rows[i]["ActiveStatus"] = activeStat;

                }
                dt.DefaultView.Sort = "Submit Date Ticks" + " " + "DESC";
                GridView_RFQ.DataSource = dt;
                GridView_RFQ.DataBind();
                GridView_RFQ.Visible = true;
                GridView_RFQ.SelectedIndex = -1;
                Session[SessionFactory.HOME_PAGE_RFQ_DATAGRID] = dt.DefaultView.ToTable();
            
        }

        protected void fillInvoiceGrid(ArrayList invL)
        {

                DataTable dt = new DataTable();
                dt.Columns.Add("RFQNo");
                dt.Columns.Add("Inv_No");
                dt.Columns.Add("Inv_Id");
                dt.Columns.Add("Inv_Date");
                dt.Columns.Add("Inv_Date_Ticks");
                dt.Columns.Add("Deliv_Stat");
                dt.Columns.Add("Amount");
                dt.Columns.Add("Related_PO");

                DateUtility dU = new DateUtility();

                for (int i = 0; i < invL.Count; i++)
                {
                    dt.Rows.Add();

                    dt.Rows[i]["RFQNo"] = ((BackEndObjects.Invoice)invL[i]).getRFQId();
                    //In the purchase screen user will always see auto-created invoice
                    //For all auto-created invoice invoice id and number will be same
                    //So displaying invoice id and invoice number here has the same result
                    dt.Rows[i]["Inv_No"] = ((BackEndObjects.Invoice)invL[i]).getInvoiceNo();
                    dt.Rows[i]["Inv_Id"] = ((BackEndObjects.Invoice)invL[i]).getInvoiceId();
                    dt.Rows[i]["Inv_Date"] = dU.getConvertedDate(((BackEndObjects.Invoice)invL[i]).getInvoiceDate().Substring(0, ((BackEndObjects.Invoice)invL[i]).getInvoiceDate().IndexOf(" ")));
                    dt.Rows[i]["Inv_Date_Ticks"] = !dt.Rows[i]["Inv_Date"].Equals("") ? Convert.ToDateTime(((BackEndObjects.Invoice)invL[i]).getInvoiceDate()).Ticks : 0;
                    dt.Rows[i]["Deliv_Stat"] = ((BackEndObjects.Invoice)invL[i]).getDeliveryStatus();
                    dt.Rows[i]["Amount"] = ((BackEndObjects.Invoice)invL[i]).getTotalAmount();
                    dt.Rows[i]["Related_PO"] = ((BackEndObjects.Invoice)invL[i]).getRelatedPO();
                }

                dt.DefaultView.Sort = "Inv_Date_Ticks" + " " + "DESC";
                GridView_Invoice.DataSource = dt;
                GridView_Invoice.DataBind();
                GridView_Invoice.Visible = true;
                GridView_Invoice.SelectedIndex = -1;
                Session[SessionFactory.HOME_PAGE_INV_DATAGRID] = dt.DefaultView.ToTable();

        }

        protected void fillLeadGrid(ArrayList leadList)
        {
                DataTable dt = getLeadGridDataTable(leadList);
                dt.DefaultView.Sort = "Submit Date Ticks" + " " + "DESC";
                Session[SessionFactory.HOME_PAGE_ASSGND_LEAD_DATAGRID] = dt.DefaultView.ToTable();

                GridView_Lead.DataSource = dt;
                GridView_Lead.DataBind();
                GridView_Lead.Visible = true;
                GridView_Lead.SelectedIndex = -1;
                disableOnPageChange("lead");
                //GridView_Lead.Sort("Submit Date", SortDirection.Descending);
        }

        protected DataTable getLeadGridDataTable(ArrayList leadList)
        {
            String entityId = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();

            if (leadList == null)
            {
                ActionLibrary.SalesActions._dispLeads dspLead = new ActionLibrary.SalesActions._dispLeads();

                leadList = dspLead.getAllActiveAndAssignedToLeadDetailsWOProdQntyAndSpec(entityId, User.Identity.Name);
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("RFQNo");
            dt.Columns.Add("RFQName");//Lead_Responded
            dt.Columns.Add("Lead_Alert_Required");
            dt.Columns.Add("CustName");
            dt.Columns.Add("CustId");
            dt.Columns.Add("Submit Date");
            dt.Columns.Add("Submit Date Ticks");
            dt.Columns.Add("Due Date");
            dt.Columns.Add("Due Date Ticks");
            dt.Columns.Add("Next Date");
            dt.Columns.Add("Next Date Ticks");
            dt.Columns.Add("Audit");
            dt.Columns.Add("Mode");

            Dictionary<String, bool> rfqRespDict = BackEndObjects.RFQResponseQuotes.getAllRFQWithNonEmptyResponseQuotesforResponseEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            int unAnsCount = 0;

            DateUtility dU = new DateUtility();

            for (int i = 0; i < leadList.Count; i++)
            {
                dt.Rows.Add();
                dt.Rows[i]["RFQNo"] = ((ActionLibrary.LeadRecord)leadList[i]).getRFQId();
                dt.Rows[i]["RFQName"] = ((ActionLibrary.LeadRecord)leadList[i]).getRFQName();
                dt.Rows[i]["Lead_Alert_Required"] = (rfqRespDict != null &&
                    rfqRespDict.ContainsKey(((ActionLibrary.LeadRecord)leadList[i]).getRFQId()) ? "false" : "true");
                unAnsCount = (dt.Rows[i]["Lead_Alert_Required"].Equals("true") ? unAnsCount + 1 : unAnsCount);

                dt.Rows[i]["CustName"] = ((ActionLibrary.LeadRecord)leadList[i]).getEntityName();
                dt.Rows[i]["CustId"] = ((ActionLibrary.LeadRecord)leadList[i]).getEntityId();
                //dt.Rows[i]["Spec"] = ((ActionLibrary.LeadRecord)leadList[i]).getRF
                dt.Rows[i]["Submit Date"] = dU.getConvertedDate(((ActionLibrary.LeadRecord)leadList[i]).getSubmitDate());
                dt.Rows[i]["Submit Date Ticks"] = Convert.ToDateTime(((ActionLibrary.LeadRecord)leadList[i]).getSubmitDate()).Ticks;
                dt.Rows[i]["Due Date"] = dU.getConvertedDate(((ActionLibrary.LeadRecord)leadList[i]).getDueDate().Substring(0, ((ActionLibrary.LeadRecord)leadList[i]).getDueDate().IndexOf(" ")));
                dt.Rows[i]["Due Date Ticks"] =  !dt.Rows[i]["Due Date"].Equals("")?Convert.ToDateTime(((ActionLibrary.LeadRecord)leadList[i]).getDueDate()).Ticks:0;

                RFQResponse respObj = RFQResponse.getRFQResponseWOQuotesForRFQIdandResponseEntityIdDB(((ActionLibrary.LeadRecord)leadList[i]).getRFQId(),
                    Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

                String nextDate = (respObj != null && respObj.getNextFollowupDate() != null && !respObj.getNextFollowupDate().Equals("") ? respObj.getNextFollowupDate().Substring(0,respObj.getNextFollowupDate().IndexOf(" ")) : "");
                dt.Rows[i]["Next Date"] = dU.getConvertedDate(nextDate);
                dt.Rows[i]["Next Date Ticks"] = (!nextDate.Equals("") ? Convert.ToDateTime(nextDate).Ticks : 0);
                dt.Rows[i]["Mode"] = ((ActionLibrary.LeadRecord)leadList[i]).getCreateMode();
            }

            if (unAnsCount > 0)
            {
                Image_Unanswered_Lead.Visible = true;
                Label_Unanswered_Leads.Visible = true;
                Label_Unanswered_Leads.Text = "Denotes Unanswered Leads (Total:" + unAnsCount + ")";
            }
            else
            {
                Image_Unanswered_Lead.Visible = false;
                Label_Unanswered_Leads.Visible = false;
            }

            return dt;
        }

        protected void fillPotnGrid(ArrayList potnList)
        {
            DataTable dt = getPotnGridDataTable(potnList);
                dt.DefaultView.Sort = "DateCreatedTicks" + " " + "DESC";
                Session[SessionFactory.HOME_PAGE_ASSGND_POTN_DATAGRID] = dt.DefaultView.ToTable();

                GridView_Potential.DataSource = dt;
                GridView_Potential.DataBind();
                GridView_Potential.Visible = true;
                GridView_Potential.SelectedIndex = -1;
                GridView_Potential.Columns[1].Visible = false;
                disableOnPageChange("potn");
        }

        protected DataTable getPotnGridDataTable(ArrayList potnList)
        {
            String RespEntityId = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();
            if (potnList == null )
            {
                ActionLibrary.SalesActions._dispPotentials dP = new SalesActions._dispPotentials();
                potnList = dP.getAllActiveAndAssignedToPotentials(RespEntityId, User.Identity.Name);
            }

            DataTable dt = new DataTable();

            dt.Columns.Add("PotId");
            dt.Columns.Add("RFQNo");
            dt.Columns.Add("RFQName");
            dt.Columns.Add("Potn_Alert_Required");
            dt.Columns.Add("DateCreated");
            dt.Columns.Add("DateCreatedTicks");
            dt.Columns.Add("Due Date");
            dt.Columns.Add("Due Date Ticks");
            dt.Columns.Add("Next Date");
            dt.Columns.Add("Next Date Ticks");
            dt.Columns.Add("CustName");
            dt.Columns.Add("CustId");
            dt.Columns.Add("PotAmnt");
            dt.Columns.Add("DealRequest");
            dt.Columns.Add("PotStage");
            dt.Columns.Add("Mode");

            Dictionary<String, bool> rfqRespDict = BackEndObjects.RFQResponseQuotes.getAllRFQWithNonEmptyResponseQuotesforResponseEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            int unAnsCount = 0;
            DateUtility dU = new DateUtility();

            for (int i = 0; i < potnList.Count; i++)
            {
                PotentialRecords potRec = (PotentialRecords)potnList[i];
                dt.Rows.Add();

                dt.Rows[i]["PotId"] = potRec.getPotentialId();
                dt.Rows[i]["RFQNo"] = potRec.getRFQId();
                dt.Rows[i]["RFQName"] = potRec.getRFQName();
                dt.Rows[i]["Potn_Alert_Required"] = (potRec.getPotenAmnt() == 0 ? "true" : "false");
                //(rfqRespDict != null &&rfqRespDict.ContainsKey(potRec.getRFQId()) ? "false" : "true");
                unAnsCount = (dt.Rows[i]["Potn_Alert_Required"].Equals("true") ? unAnsCount + 1 : unAnsCount);

                dt.Rows[i]["DateCreated"] = dU.getConvertedDate(potRec.getCreatedDate());
                dt.Rows[i]["DateCreatedTicks"] = Convert.ToDateTime(potRec.getCreatedDate()).Ticks;
                dt.Rows[i]["Due Date"] = dU.getConvertedDate(potRec.getDueDate().Substring(0,potRec.getDueDate().IndexOf(" ")));
                dt.Rows[i]["Due Date Ticks"] = ! dt.Rows[i]["Due Date"].Equals("")?Convert.ToDateTime(potRec.getDueDate()).Ticks:0;
                dt.Rows[i]["CustName"] = potRec.getEntityName();
                dt.Rows[i]["CustId"] = potRec.getEntityId();
                dt.Rows[i]["PotAmnt"] = potRec.getPotenAmnt();
                dt.Rows[i]["DealRequest"] = (potRec.getCreateMode().Equals(BackEndObjects.RFQShortlisted.POTENTIAL_CREATION_MODE_MANUAL) ? "" :
                    potRec.getFinlSupFlag());
                dt.Rows[i]["PotStage"] = potRec.getPotStat();

                RFQResponse respObj = RFQResponse.
                    getRFQResponseWOQuotesForRFQIdandResponseEntityIdDB(potRec.getRFQId(),
    Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                String nextDate = (respObj != null && respObj.getNextFollowupDate() != null && !respObj.getNextFollowupDate().Equals("") ? respObj.getNextFollowupDate().Substring(0,respObj.getNextFollowupDate().IndexOf(" ")) : "");
                dt.Rows[i]["Next Date"] = dU.getConvertedDate(nextDate);
                dt.Rows[i]["Next Date Ticks"] = (nextDate != null && !nextDate.Equals("") ? Convert.ToDateTime(nextDate).Ticks : 0);
               dt.Rows[i]["Mode"] = potRec.getCreateMode();
            }

            if (unAnsCount > 0)
            {
                Image_Unanswered_Potn.Visible = true;
                Label_Unanswered_Potn.Visible = true;
                Label_Unanswered_Potn.Text = "Denotes Unanswered Potentials (Total:" + unAnsCount + ")";
            }
            else
            {
                Image_Unanswered_Potn.Visible = false;
                Label_Unanswered_Potn.Visible = false;
            }
            return dt;
        }

        protected void GridView_RFQ_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_RFQ.PageIndex = e.NewPageIndex;
            GridView_RFQ.DataSource = (DataTable)Session[SessionFactory.HOME_PAGE_RFQ_DATAGRID];
            GridView_RFQ.DataBind();
            disableOnPageChange("rfq");
        }

        protected void GridView_RFQ_SelectedIndexChanged(object sender, EventArgs e)
        {
            enableOnSelect("rfq", "");
            ((RadioButton)GridView_RFQ.SelectedRow.Cells[0].FindControl("rfq_radio")).Checked = true;
            String selectedRfqId = ((Label)GridView_RFQ.SelectedRow.Cells[2].FindControl("Label_RFQId")).Text;
            BackEndObjects.RFQDetails rfqObj = BackEndObjects.RFQDetails.getRFQDetailsbyIdDB(selectedRfqId);


            Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SELECTED_RFQ_ID] = rfqObj.getRFQId();
            Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_LOCATION] = rfqObj.getLocalityId();
            Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_RFQ_SPEC] = BackEndObjects.RFQProductServiceDetails.getAllProductServiceDetailsbyRFQIdDB(rfqObj.getRFQId());
            Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_PROD_QNTY] = BackEndObjects.RFQProdServQnty.getRFQProductServiceQuantityforRFIdDB(rfqObj.getRFQId());

        }

        protected void Button_Filter_All_RFQ_Click(object sender, EventArgs e)
        {
            String rfqNo = TextBox_RFQ_No_RFQ.Text;

            ActionLibrary.PurchaseActions._dispRFQDetails dRfq = new ActionLibrary.PurchaseActions._dispRFQDetails();

            Dictionary<String, String> filterParams = new Dictionary<string, string>();
            if (!rfqNo.Trim().Equals(""))
                filterParams.Add(dRfq.FILTER_BY_RFQ_NO, rfqNo);

            fillRFQGrid(dRfq.
                getAllRFQDetailsFilteredForEntityandApproverId
                (Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), User.Identity.Name, filterParams));

        }

        protected void Button_Rfq_Refresh_Click(object sender, EventArgs e)
        {
            fillRFQGrid(BackEndObjects.RFQDetails.
                getAllRFQbyApproverIdDB(User.Identity.Name, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()));
            disableOnPageChange("rfq");
            Button_Rfq_Refresh.Focus();
        }

        protected void Button_Notes_RFQ_Click(object sender, EventArgs e)
        {
            String forwardString = "NoteDetails.aspx";

            String rfqId = ((Label)GridView_RFQ.SelectedRow.Cells[0].FindControl("Label_RFQId")).Text;

            forwardString += "?contextId=" + rfqId;

            ScriptManager.RegisterStartupScript(this, typeof(string), "rfqNotes",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=800,height=800,left=500,right=500');", true);

        }

        protected void Button_Audit_RFQ_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/DispAudit.aspx";
            forwardString += "?contextId1=" + ((Label)GridView_RFQ.SelectedRow.Cells[0].FindControl("Label_RFQId")).Text;
            forwardString += "&contextId2=" + "";
            forwardString += "&contextId3=" + "";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispAuditRFQ", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=700');", true);

        }

        protected void Button_Filter_All_Inv_Click(object sender, EventArgs e)
        {
            String invNo = TextBox_Inv_No.Text;
            String rfqNo = TextBox_RFQ_No.Text;
            String customer = DropDownList_Contact_Inv.SelectedValue;

            ActionLibrary.PurchaseActions._dispInvoiceDetails dInv = new ActionLibrary.PurchaseActions._dispInvoiceDetails();

            Dictionary<String, String> filterParams = new Dictionary<string, string>();

            if (rfqNo != null && !rfqNo.Equals(""))
                filterParams.Add(dInv.FILTER_BY_RFQ_NO, rfqNo);
            if (invNo != null && !invNo.Equals(""))
                filterParams.Add(dInv.FILTER_BY_INVOICE_NO, invNo);
            if (customer != null && !customer.Equals("_"))
                filterParams.Add(dInv.FILTER_BY_CUSTOMER, customer);

            ArrayList invList = BackEndObjects.Invoice.
                getAllInvoicesbyApproverIdAndRespEntIdDB(User.Identity.Name,Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

            fillInvoiceGrid(dInv.getAllInvDetailsFiltered(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), invList, filterParams));

        }

        protected void GridView_Invoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            String selectedRfqId = ((Label)GridView_Invoice.SelectedRow.Cells[2].FindControl("Label_RFQId")).Text;
            Session[SessionFactory.ALL_SALE_ALL_INV_SELECTED_RFQ_SPEC] = BackEndObjects.RFQProductServiceDetails.
                getAllProductServiceDetailsbyRFQIdDB(selectedRfqId);
            enableOnSelect("inv", "");

        }

        protected void GridView_Invoice_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_Invoice.PageIndex = e.NewPageIndex;
            GridView_Invoice.DataSource = (DataTable)Session[SessionFactory.HOME_PAGE_INV_DATAGRID];
            GridView_Invoice.DataBind();
            disableOnPageChange("inv");
        }

        private SortDirection GridViewSortDirectionInv
        {
            get
            {
                if (ViewState["sortDirectionInv"] == null)
                    ViewState["sortDirectionInv"] = SortDirection.Descending;
                return (SortDirection)ViewState["sortDirectionInv"];
            }
            set { ViewState["sortDirectionInv"] = value; }
        }

        protected void GridView_Invoice_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            ViewState["SortExpressionInv"] = sortExpression;
            DataTable dt = (DataTable)Session[SessionFactory.HOME_PAGE_INV_DATAGRID];

            if (GridViewSortDirectionInv == SortDirection.Ascending)
            {
                GridViewSortDirectionInv = SortDirection.Descending;
                dt.DefaultView.Sort = sortExpression + " " + "DESC";
            }
            else
            {
                GridViewSortDirectionInv = SortDirection.Ascending;
                dt.DefaultView.Sort = sortExpression + " " + "ASC";
            }

            DataTable sortedTable = dt.DefaultView.ToTable();
            Session[SessionFactory.HOME_PAGE_INV_DATAGRID] = sortedTable;
            disableOnPageChange("inv");
            GridView_Invoice.SelectedIndex = -1;
            bindSortedData(GridView_Invoice, sortedTable);
        }

        protected void Button_Notes_Inv_Click(object sender, EventArgs e)
        {
            String forwardString = "NoteDetails.aspx";

            String rfqId = ((Label)GridView_Invoice.SelectedRow.Cells[0].FindControl("Label_Invoice_Id_Val")).Text;

            forwardString += "?contextId=" + rfqId;

            ScriptManager.RegisterStartupScript(this, typeof(string), "NotesInv",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=800,height=800,left=500,right=500');", true);
        }

        protected void Button_Audit_Inv_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/DispAudit.aspx";

            forwardString += "?contextId1=" + ((Label)GridView_Invoice.SelectedRow.Cells[0].FindControl("Label_Invoice_Id_Val")).Text;
            forwardString += "&contextId2=" + "";
            forwardString += "&contextId3=" + "";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispAuditSalesInv", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=700');", true);

        }

        protected void LinkButton_Show_Inv_Invoice_Grid_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_Invoice.SelectedIndex)
                GridView_Invoice.SelectRow(row.RowIndex);
            ((RadioButton)GridView_Invoice.SelectedRow.Cells[0].FindControl("inv_radio")).Checked = true;

            String forwardString = "/Pages/Popups/Sale/Inv_Details.aspx";
            forwardString += "?rfId=" + ((Label)GridView_Invoice.SelectedRow.Cells[0].FindControl("Label_RFQId")).Text;
            forwardString += "&context=" + "vendInvoiceGrid";

            String relatedPO = ((Label)GridView_Invoice.SelectedRow.Cells[0].FindControl("Label_Related_PO")).Text;
            if (!relatedPO.Equals(""))
                forwardString += "&poId=" + relatedPO;
            else
            forwardString += "&poId=" + BackEndObjects.PurchaseOrder.
                getPurchaseOrderforRFQIdDB(((Label)GridView_Invoice.SelectedRow.Cells[0].FindControl("Label_RFQId")).Text).getPo_id();

            forwardString += "&invId=" + ((Label)GridView_Invoice.SelectedRow.Cells[0].FindControl("Label_Invoice_Id_Val")).Text;
            forwardString += "&approvalContext=Y";
            //Response.Write("<script type='text/javascript'>alert('" + "Hi" + "');</script>");
            //ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "alert('hi');", true);
            ScriptManager.RegisterStartupScript(this, typeof(string), "DispInvClientInvGrid", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=900');", true);
        }

        protected void Button_Inv_Refresh_Click(object sender, EventArgs e)
        {
            fillInvoiceGrid(BackEndObjects.Invoice.getAllInvoicesbyApproverIdAndRespEntIdDB(User.Identity.Name, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()));
            disableOnPageChange("inv");
            Button_Inv_Refresh.Focus();
        }

        protected void Button_Filter_All_Lead_Click(object sender, EventArgs e)
        {
            String custId = DropDownList_Contact_Lead.SelectedValue;
            String rfqNo = TextBox_RFQ_No_Lead.Text;


            ActionLibrary.SalesActions._dispLeads dLead = new ActionLibrary.SalesActions._dispLeads();

            Dictionary<String, String> filterParams = new Dictionary<string, string>();
            if (!rfqNo.Trim().Equals(""))
                filterParams.Add(dLead.FILTER_BY_RFQ_NO, rfqNo);
            if (custId != null && !custId.Equals("_"))
                filterParams.Add(dLead.FILTER_BY_CUST_ID, DropDownList_Contact_Lead.SelectedValue);

            
            fillLeadGrid(dLead.                
                getAllActiveAndAssignedToLeadDetailsFilteredWOProdQntyAndSpec
                (Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), User.Identity.Name, filterParams));

        }

        protected void GridView_Lead_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_Lead.PageIndex = e.NewPageIndex;
            GridView_Lead.DataSource = (DataTable)Session[SessionFactory.HOME_PAGE_ASSGND_LEAD_DATAGRID];
            GridView_Lead.DataBind();
            disableOnPageChange("lead");
        }

        protected void GridView_Lead_SelectedIndexChanged(object sender, EventArgs e)
        {
            enableOnSelect("lead", "");
            ((RadioButton)GridView_Lead.SelectedRow.Cells[0].FindControl("lead_radio")).Checked = true;
            String selectedLeadId = ((Label)GridView_Lead.SelectedRow.Cells[2].FindControl("Label_RFQId")).Text;

            BackEndObjects.RFQDetails rfqObj = BackEndObjects.RFQDetails.getRFQDetailsbyIdDB(selectedLeadId);

            Session[SessionFactory.ALL_SALE_ALL_LEAD_SELECTED_LEAD_ID] = rfqObj.getRFQId();
            Session[SessionFactory.ALL_SALE_ALL_LEAD_SELECTED_CUSTOMER_OBJ] = ActionLibrary.customerDetails.getContactDetails(rfqObj.getEntityId(), Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

        }

        protected void Button_Notes_Lead_Click(object sender, EventArgs e)
        {
            String forwardString = "NoteDetails.aspx";

            String rfqId = ((Label)GridView_Lead.SelectedRow.Cells[0].FindControl("Label_RFQId")).Text;

            forwardString += "?contextId=" + rfqId;

            ScriptManager.RegisterStartupScript(this, typeof(string), "LeadNotes",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=800,height=800,left=500,right=500');", true);
        }

        protected void Button_Audit_Lead_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/DispAudit.aspx";
            forwardString += "?contextId1=" + ((Label)GridView_Lead.SelectedRow.Cells[0].FindControl("Label_RFQId")).Text;
            forwardString += "&contextId2=" + "";
            forwardString += "&contextId3=" + "";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispAuditLead", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=700');", true);

        }

        protected void Button_Lead_Refresh_Click(object sender, EventArgs e)
        {
            fillLeadGrid(null);
            disableOnPageChange("lead");
            Button_Lead_Refresh.Focus();
        }

        protected void GridView_Potential_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_Potential.PageIndex = e.NewPageIndex;
            GridView_Potential.DataSource = (DataTable)Session[SessionFactory.HOME_PAGE_ASSGND_POTN_DATAGRID];
            GridView_Potential.DataBind();
            disableOnPageChange("potn");
        }

        protected void GridView_Potential_SelectedIndexChanged(object sender, EventArgs e)
        {
            enableOnSelect("potn", "");
            ((RadioButton)GridView_Potential.SelectedRow.Cells[0].FindControl("potn_radio")).Checked = true;

            String selectedRFQId = ((Label)GridView_Potential.SelectedRow.Cells[3].FindControl("Label_RFQId3")).Text;

            BackEndObjects.RFQDetails rfqObj = BackEndObjects.RFQDetails.getRFQDetailsbyIdDB(selectedRFQId);

            Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_SELECTED_RFQ_ID] = rfqObj.getRFQId();
            Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_SELECTED_CUSTOMER_OBJ] = ActionLibrary.customerDetails.getContactDetails(rfqObj.getEntityId(), Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

        }


        protected void Button_Filter_All_Pot_Click(object sender, EventArgs e)
        {
            String potStage = !DropDownList_Pot_Stage_Stat.SelectedValue.Equals("_") ? DropDownList_Pot_Stage_Stat.SelectedValue : "";
            String custId = DropDownList_Contact_Potn.SelectedValue;
            String rfqNo = TextBox_RFQ_No_Potn.Text;


            ActionLibrary.SalesActions._dispPotentials dPot = new ActionLibrary.SalesActions._dispPotentials();

            Dictionary<String, String> filterParams = new Dictionary<string, string>();
            if (!rfqNo.Trim().Equals(""))
                filterParams.Add(dPot.FILTER_BY_RFQ_NO, rfqNo);
            if (potStage != null && !potStage.Equals(""))
                filterParams.Add(dPot.FILTER_BY_STAGE, potStage);
            if (custId != null && !custId.Equals("_"))
                filterParams.Add(dPot.FILTER_BY_CUST_ID, DropDownList_Contact_Potn.SelectedValue);

            fillPotnGrid(dPot.getAllActiveAndAssignedPotentialsFiltered                
                (Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), User.Identity.Name, filterParams));

        }

        protected void Button_Notes_Potn_Click(object sender, EventArgs e)
        {
            String forwardString = "NoteDetails.aspx";

            String rfqId = ((Label)GridView_Potential.SelectedRow.Cells[0].FindControl("Label_Hidden_Pot_Id")).Text;

            forwardString += "?contextId=" + rfqId;

            ScriptManager.RegisterStartupScript(this, typeof(string), "NotesPotn",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=800,height=800,left=500,right=500');", true);

        }

        protected void Button_Audit_Potn_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/DispAudit.aspx";
            forwardString += "?contextId1=" + ((Label)GridView_Potential.SelectedRow.Cells[0].FindControl("Label_Hidden_Pot_Id")).Text;
            forwardString += "&contextId2=" + "";
            forwardString += "&contextId3=" + "";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispAuditPotential", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=700');", true);

        }

        protected void Button_Pot_Refresh_Click(object sender, EventArgs e)
        {
            fillPotnGrid(null);
            disableOnPageChange("potn");
            Button_Pot_Refresh.Focus();
        }

        protected void Button_Filter_Incoming_Defects_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> filterParams = new Dictionary<String, String>();
            ActionLibrary.DefectActions._dispDefectDetails dspDefect = new DefectActions._dispDefectDetails();

            if (!TextBox_Defect_No_Incm_Defect.Text.Equals(""))
                filterParams.Add(dspDefect.FILTER_BY_DEFECT_NO, TextBox_Defect_No_Incm_Defect.Text);
            if (!DropDownList_Incm_Defect_Sev.SelectedValue.Equals("_"))
                filterParams.Add(dspDefect.FILTER_BY_DEFECT_SEV, DropDownList_Incm_Defect_Sev.SelectedValue);

            loadIncomingDefectList(dspDefect.getAllOpenDefectFilteredForSupplierAndAssignedToUser
                ("incoming", Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(),User.Identity.Name, filterParams));

        }

        protected void Button_Inc_Defect_Refresh_Click(object sender, EventArgs e)
        {
            loadIncomingDefectList(null);
            disableOnPageChange("defect");
            Button_Inc_Defect_Refresh.Focus();
        }

        protected void Button_Notes_Incm_Def_Click(object sender, EventArgs e)
        {
            String forwardString = "NoteDetails.aspx";

            String rfqId = ((Label)GridView_Incoming_Defects.SelectedRow.Cells[0].FindControl("Label_Defect_Id")).Text;

            forwardString += "?contextId=" + rfqId;

            ScriptManager.RegisterStartupScript(this, typeof(string), "IncmDefNotes",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=800,height=800,left=500,right=500');", true);

        }

        protected void Button_Audit_Incm_Defect_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/DispAudit.aspx";
            forwardString += "?contextId1=" + ((Label)GridView_Incoming_Defects.SelectedRow.Cells[0].FindControl("Label_Defect_Id")).Text;
            forwardString += "&contextId2=" + "";
            forwardString += "&contextId3=" + "";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispIncmDefectAudit", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=700');", true);

        }

        protected void GridView_Incoming_Defects_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_Incoming_Defects.PageIndex = e.NewPageIndex;
            GridView_Incoming_Defects.DataSource = (DataTable)Session[SessionFactory.HOME_PAGE_ASSGND_OPEN_DEFECTS_DATAGRID];
            GridView_Incoming_Defects.DataBind();
            disableOnPageChange("defect");
        }

        protected void GridView_Incoming_Defects_SelectedIndexChanged(object sender, EventArgs e)
        {
            enableOnSelect("defect", "");
            ((RadioButton)GridView_Incoming_Defects.SelectedRow.Cells[0].FindControl("def_radio")).Checked = true;
            Session[SessionFactory.ALL_DEFECT_INCM_SELECTED_CUSTOMER_OBJ] =
ActionLibrary.customerDetails.getContactDetails(((Label)GridView_Incoming_Defects.SelectedRow.Cells[0].FindControl("Label_EntId")).Text,
Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
        }


        protected void LinkButton_Customer_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_Incoming_Defects.SelectedIndex)
                GridView_Incoming_Defects.SelectRow(row.RowIndex);

            ((RadioButton)GridView_Incoming_Defects.SelectedRow.Cells[0].FindControl("def_radio")).Checked = true;

            String forwardString = "/Pages/Popups/Defects/AllDefectsContacts.aspx";
            forwardString += "?context=" + "incoming";

            ScriptManager.RegisterStartupScript(this, typeof(string), "IncmDefContact",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=400');", true);

        }

        protected void GridView_Incoming_Defects_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
            }
            else if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Normal) == DataControlRowState.Normal)
            {
                GridViewRow gVR = e.Row;
                //Check the SLA and color it
                String submitDate = ((Label)gVR.Cells[0].FindControl("Label_Submit_Date")).Text;
                String defectResolStat = ((Label)gVR.Cells[0].FindControl("Label_Defect_Resol_Stat")).Text;
                String sev = ((Label)gVR.Cells[0].FindControl("Label_Sev")).Text;
                if (defectResolStat.Equals(BackEndObjects.DefectResolStatCodes.DEFECT_RESOL_STAT_CODE_OPEN) ||
                    defectResolStat.Equals(BackEndObjects.DefectResolStatCodes.DEFECT_RESOL_STAT_CODE_WORKING_ON))
                {
                    //Now check the SLA and change color accordingly
                    ArrayList slaList = (ArrayList)Session[SessionFactory.ALL_DEFECT_SLA_LIST];
                    if (slaList != null && slaList.Count != 0)
                    {
                        for (int i = 0; i < slaList.Count; i++)
                        {
                            DefectSLA slaObj = (DefectSLA)slaList[i];
                            if (slaObj.getSeverity().Equals(sev))
                            {
                                if (Convert.ToDateTime(submitDate).AddHours(Double.Parse(slaObj.getSLA())) < DateTime.Today)
                                {
                                    ((Label)gVR.Cells[0].FindControl("Label_Defect_Id")).Font.Bold = true;
                                    ((Label)gVR.Cells[0].FindControl("Label_Defect_Id")).ForeColor = System.Drawing.Color.Red;
                                }
                                else if (Convert.ToDateTime(submitDate).AddHours(Double.Parse(slaObj.getAlert_Before())) < DateTime.Today)
                                {
                                    ((Label)gVR.Cells[0].FindControl("Label_Defect_Id")).Font.Bold = true;
                                    ((Label)gVR.Cells[0].FindControl("Label_Defect_Id")).ForeColor = System.Drawing.Color.YellowGreen;
                                }
                                break;
                            }
                        }
                    }

                }
            }
        }

        protected void enableOnSelect(String senderName, String createMode)
        {
            switch (senderName)
            {
                case "lead": Button_Audit_Lead.Enabled = true;
                    Button_Notes_Lead.Enabled = true;
                    break;
                case "potn": Button_Audit_Potn.Enabled = true;
                    Button_Notes_Potn.Enabled = true;
                    break;
                case "inv": Button_Audit_Inv.Enabled = true;
                    Button_Notes_Inv.Enabled = true;
                    Button_Workflow_Tree_Inv.Enabled = true;
                    break;
                case "rfq": Button_Audit_RFQ.Enabled = true;
                    Button_Notes_RFQ.Enabled = true;
                    Button_Workflow_Tree.Enabled = true;
                    break;
                case "defect": Button_Audit_Incm_Defect.Enabled = true;
                    Button_Notes_Incm_Def.Enabled = true;
                    break;
            }
        }

        protected void disableOnPageChange(String senderName)
        {
            switch (senderName)
            {
                case "lead": Button_Audit_Lead.Enabled = false;
                    Button_Notes_Lead.Enabled = false;
                    break;
                case "potn": Button_Audit_Potn.Enabled = false;
                    Button_Notes_Potn.Enabled = false;
                    break;
                case "inv": Button_Audit_Inv.Enabled = false;
                    Button_Notes_Inv.Enabled = false;
                    Button_Workflow_Tree_Inv.Enabled = false;
                    break;
                case "rfq": Button_Audit_RFQ.Enabled = false;
                    Button_Notes_RFQ.Enabled = false;
                    Button_Workflow_Tree.Enabled = false;
                    break;
                case "defect": Button_Audit_Incm_Defect.Enabled = false;
                    Button_Notes_Incm_Def.Enabled = false;
                    break;
            }
        }

        protected void GridView_Lead_RadioSelect(object sender, EventArgs e)
        {
            RadioButton linkb = (RadioButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1)
                GridView_Lead.SelectRow(row.RowIndex);
        }

        protected void GridView_Potn_RadioSelect(object sender, EventArgs e)
        {
            RadioButton linkb = (RadioButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1)
                GridView_Potential.SelectRow(row.RowIndex);
        }

        protected void GridView_Def_RadioSelect(object sender, EventArgs e)
        {
            RadioButton linkb = (RadioButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1)
                GridView_Incoming_Defects.SelectRow(row.RowIndex);
        }

        protected void LinkButton_Customer_Command_Lead(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_Lead.SelectedIndex)
                GridView_Lead.SelectRow(row.RowIndex);

            ((RadioButton)GridView_Lead.SelectedRow.Cells[0].FindControl("lead_radio")).Checked = true;

            String forwardString = "Popups/Sale/AllLead_Customer.aspx";
            ScriptManager.RegisterStartupScript(this, typeof(string), "DispCustLead",
               "window.open('" + forwardString + "',null,'status=1,location=yes,width=1000,height=400,left=100,right=500');", true);
        }

        protected void LinkButton_Cust_Pot_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_Potential.SelectedIndex)
                GridView_Potential.SelectRow(row.RowIndex);

            ((RadioButton)GridView_Potential.SelectedRow.Cells[0].FindControl("potn_radio")).Checked = true;

            String forwardString = "Popups/Sale/AllPotential_Customer.aspx";
            ScriptManager.RegisterStartupScript(this, typeof(string), "DispCustPotn",
               "window.open('" + forwardString + "',null,'status=1,location=yes,width=1000,height=400,left=100,right=500');", true);
        }

        private SortDirection GridViewSortDirectionLead
        {
            get
            {
                if (ViewState["sortDirectionLead"] == null)
                    ViewState["sortDirectionLead"] = SortDirection.Descending;
                return (SortDirection)ViewState["sortDirectionLead"];
            }
            set { ViewState["sortDirectionLead"] = value; }
        }

        protected void GridView_Lead_Sorting(object sender, GridViewSortEventArgs e)
        {       
            disableOnPageChange("lead");

            string sortExpression = e.SortExpression;
            ViewState["SortExpressionLead"] = sortExpression;
            DataTable dt = (DataTable)Session[SessionFactory.HOME_PAGE_ASSGND_LEAD_DATAGRID];

            if (GridViewSortDirectionLead == SortDirection.Ascending)
            {
                GridViewSortDirectionLead = SortDirection.Descending;
                dt.DefaultView.Sort = sortExpression + " " + "DESC";
            }
            else
            {
                GridViewSortDirectionLead = SortDirection.Ascending;
                dt.DefaultView.Sort = sortExpression + " " + "ASC";
            }

            DataTable sortedTable = dt.DefaultView.ToTable();
            Session[SessionFactory.HOME_PAGE_ASSGND_LEAD_DATAGRID] = sortedTable;
            disableOnPageChange("lead");
            GridView_Lead.SelectedIndex = -1;
            bindSortedData(GridView_Lead, sortedTable);
        }

        protected void bindSortedData(GridView grd, DataTable dt)
        {
            grd.DataSource = dt;
            grd.DataBind();
        }

        private SortDirection GridViewSortDirectionPotn
        {
            get
            {
                if (ViewState["sortDirectionPotn"] == null)
                    ViewState["sortDirectionPotn"] = SortDirection.Descending;
                return (SortDirection)ViewState["sortDirectionPotn"];
            }
            set { ViewState["sortDirectionPotn"] = value; }
        }

        protected void GridView_Potential_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            ViewState["SortExpressionPotn"] = sortExpression;
            DataTable dt = (DataTable)Session[SessionFactory.HOME_PAGE_ASSGND_POTN_DATAGRID];

            if (GridViewSortDirectionPotn == SortDirection.Ascending)
            {
                GridViewSortDirectionPotn = SortDirection.Descending;
                dt.DefaultView.Sort = sortExpression + " " + "DESC";
            }
            else
            {
                GridViewSortDirectionPotn = SortDirection.Ascending;
                dt.DefaultView.Sort = sortExpression + " " + "ASC";
            }

            DataTable sortedTable = dt.DefaultView.ToTable();
            Session[SessionFactory.HOME_PAGE_ASSGND_POTN_DATAGRID] = sortedTable;
            disableOnPageChange("potn");
            GridView_Potential.SelectedIndex = -1;
            bindSortedData(GridView_Potential, sortedTable);
        }

        private SortDirection GridViewSortDirectionIncm
        {
            get
            {
                if (ViewState["sortDirectionIncm"] == null)
                    ViewState["sortDirectionIncm"] = SortDirection.Descending;
                return (SortDirection)ViewState["sortDirectionIncm"];
            }
            set { ViewState["sortDirectionIncm"] = value; }
        }

        protected void GridView_Incoming_Defects_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            ViewState["SortExpressionIncm"] = sortExpression;
            DataTable dt = (DataTable)Session[SessionFactory.HOME_PAGE_ASSGND_OPEN_DEFECTS_DATAGRID];

            if (GridViewSortDirectionIncm == SortDirection.Ascending)
            {
                GridViewSortDirectionIncm = SortDirection.Descending;
                dt.DefaultView.Sort = sortExpression + " " + "DESC";
            }
            else
            {
                GridViewSortDirectionIncm = SortDirection.Ascending;
                dt.DefaultView.Sort = sortExpression + " " + "ASC";
            }

            DataTable sortedTable = dt.DefaultView.ToTable();
            Session[SessionFactory.HOME_PAGE_ASSGND_OPEN_DEFECTS_DATAGRID] = sortedTable;

            disableOnPageChange("defect");
            GridView_Incoming_Defects.SelectedIndex = -1;
            bindSortedData(GridView_Incoming_Defects, sortedTable);
        }

        protected void GridView_RFQ_RadioSelect(object sender, EventArgs e)
        {
            RadioButton linkb = (RadioButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1)
                GridView_RFQ.SelectRow(row.RowIndex);
        }

        protected void LinkButton_Show_Spec_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_RFQ.SelectedIndex)
                GridView_RFQ.SelectRow(row.RowIndex);

            ((RadioButton)GridView_RFQ.SelectedRow.Cells[0].FindControl("rfq_radio")).Checked = true;

            String forwardString = "Popups/Purchase/AllRFQ_Specification.aspx";
            forwardString += "?approvalContext=Y";
            ScriptManager.RegisterStartupScript(this, typeof(string), "DispSpecRFQ",
               "window.open('" + forwardString + "',null,'status=1,location=yes,width=1000,height=400,left=100,right=500');", true);

        }

        protected void LinkButton_Location_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_RFQ.SelectedIndex)
                GridView_RFQ.SelectRow(row.RowIndex);

            ((RadioButton)GridView_RFQ.SelectedRow.Cells[0].FindControl("rfq_radio")).Checked = true;

            String forwardString = "Popups/Purchase/AllRFQ_Location.aspx";
            forwardString += "?approvalContext=Y";
            ScriptManager.RegisterStartupScript(this, typeof(string), "DispLocation",
               "window.open('" + forwardString + "',null,'status=1,width=700,height=400,left=500,right=500');", true);
        }

        protected void LinkButton_Broadcast_To_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_RFQ.SelectedIndex)
                GridView_RFQ.SelectRow(row.RowIndex);

            ((RadioButton)GridView_RFQ.SelectedRow.Cells[0].FindControl("rfq_radio")).Checked = true;

            String forwardString = "Popups/Purchase/AllRFQ_Broadcast.aspx";
            forwardString += "?approvalContext=Y";
            ScriptManager.RegisterStartupScript(this, typeof(string), "DispBroadcast",
               "window.open('" + forwardString + "',null,'status=1,location=yes,width=1000,height=400,left=100,right=500');", true);
        }

        protected void LinkButton1_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_RFQ.SelectedIndex)
                GridView_RFQ.SelectRow(row.RowIndex);

            ((RadioButton)GridView_RFQ.SelectedRow.Cells[0].FindControl("rfq_radio")).Checked = true;

            String forwardString = "Popups/Purchase/AllRFQ_TnC.aspx";
            forwardString += "?approvalContext=Y";
            ScriptManager.RegisterStartupScript(this, typeof(string), "DispTnCRFQ",
               "window.open('" + forwardString + "',null,'status=1,width=600,height=400,left=500,right=500');", true);
        }

        private SortDirection GridViewSortDirectionRfq
        {
            get
            {
                if (ViewState["sortDirectionRfq"] == null)
                    ViewState["sortDirectionRfq"] = SortDirection.Descending;
                return (SortDirection)ViewState["sortDirectionRfq"];
            }
            set { ViewState["sortDirectionRfq"] = value; }
        }
        
        protected void GridView_RFQ_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            ViewState["SortExpressionRfq"] = sortExpression;
            DataTable dt = (DataTable)Session[SessionFactory.HOME_PAGE_RFQ_DATAGRID];

            if (GridViewSortDirectionRfq == SortDirection.Ascending)
            {
                GridViewSortDirectionRfq = SortDirection.Descending;
                dt.DefaultView.Sort = sortExpression + " " + "DESC";
            }
            else
            {
                GridViewSortDirectionRfq = SortDirection.Ascending;
                dt.DefaultView.Sort = sortExpression + " " + "ASC";
            }

            DataTable sortedTable = dt.DefaultView.ToTable();
            Session[SessionFactory.HOME_PAGE_RFQ_DATAGRID] = sortedTable;
            disableOnPageChange("rfq");
            GridView_RFQ.SelectedIndex = -1;
            bindSortedData(GridView_RFQ, sortedTable);
        }

        protected void GridView_Inv_RadioSelect(object sender, EventArgs e)
        {
            RadioButton linkb = (RadioButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1)
                GridView_Invoice.SelectRow(row.RowIndex);
        }

        protected void Button_Workflow_Tree_Click(object sender, EventArgs e)
        {
            String forwardString = "Workflow_Tree.aspx";

            String rfqId = ((Label)GridView_RFQ.SelectedRow.Cells[0].FindControl("Label_RFQId")).Text;

            forwardString += "?contextId=" + rfqId;
            forwardString += "&contextName=" + BackEndObjects.Workflow_Action.WORKFLOW_ACTION_CONTEXT_NAME_RFQ;

            ScriptManager.RegisterStartupScript(this, typeof(string), "workflow_tree_rfq",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=600,height=500,left=500,right=500');", true);
        }

        protected void Button_Workflow_Tree_Inv_Click(object sender, EventArgs e)
        {
            String forwardString = "Workflow_Tree.aspx";

            String invId = ((Label)GridView_Invoice.SelectedRow.Cells[0].FindControl("Label_Invoice_Id_Val")).Text;

            forwardString += "?contextId=" + invId;
            forwardString += "&contextName=" + BackEndObjects.Workflow_Action.WORKFLOW_ACTION_CONTEXT_NAME_INV;

            ScriptManager.RegisterStartupScript(this, typeof(string), "workflow_tree_inv",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=600,height=500,left=500,right=500');", true);
        }

    }
}