using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BackEndObjects;
using ActionLibrary;
using System.Collections;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Web.UI.HtmlControls;

namespace OnLine.Pages
{
    public partial class Sales : System.Web.UI.Page
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
                    ((HtmlGenericControl)(Master.FindControl("Sales"))).Attributes.Add("class", "active");

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

                   // ((Menu)Master.FindControl("Menu1")).Items[2].Selected = true;

                    populateLogo();
                    CheckAccessToActions();

                    if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS]||
                        accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_SALES_SCREEN_VIEW]
                        )
                    { //Full Access - no need to do any restriction
                        LoadProductCatAndContacts();
                        LoadAssignedToList();
                        LoadReqrActiveStat();
                        loadPotStageAndStats();
                        populateInvFilter();

                        Task leadTask = Task.Factory.StartNew(fillLeadGrid);
                        Task potnTask = Task.Factory.StartNew(fillPotnGrid);                        
                        Task invTask = Task.Factory.StartNew(() => fillInvoiceGrid(BackEndObjects.Invoice.getAllInvoicesbyRespEntId(entId)));
                        Task soTask = Task.Factory.StartNew(loadSOGrid);
                        //loadSOGrid();
                        try
                        {
                            Task.WaitAll(leadTask, potnTask, invTask);
                        }
                        catch (AggregateException ex)
                        {
                        }

                        //fillLeadGrid();
                        //fillPotnGrid();
                        
                        
                    }                    
                    else
                    {
                        Label_Sales_Screen_Access.Visible = true;
                        Label_Sales_Screen_Access.Text = "You don't have access to view this page";
                    }
                    
                }
            }
        }

        protected void LoadAssignedToList()
        {
            ListItem me = new ListItem();
            me.Text = "Me";
            me.Value = User.Identity.Name;

            ListItem unassgnd = new ListItem();
            unassgnd.Text = "Unassigned";
            unassgnd.Value = "";
            DropDownList_Assigned_To_Lead.Items.Add(me);
            DropDownList_Assigned_To_Lead.Items.Add(unassgnd);
            DropDownList_Assigned_To_Potn.Items.Add(me);
            DropDownList_Assigned_To_Potn.Items.Add(unassgnd);
            //BackEndObjects.MainBusinessEntity.getUserDetailsforMainEntitybyIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            Dictionary<String, userDetails> allUserDetails = (Dictionary<String,userDetails>)Cache[Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()];
            if (allUserDetails == null)
            {
                allUserDetails = BackEndObjects.MainBusinessEntity.getUserDetailsforMainEntitybyIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                Cache.Insert(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), allUserDetails);
            }

            foreach (KeyValuePair<String, userDetails> kvp in allUserDetails)
            {
                BackEndObjects.userDetails userDetObj = kvp.Value;

                if (!userDetObj.getUserId().Equals(User.Identity.Name))
                {
                    ListItem lt1 = new ListItem();
                    lt1.Text = userDetObj.getUserId();
                    lt1.Value = userDetObj.getUserId();

                    DropDownList_Assigned_To_Lead.Items.Add(lt1);
                    DropDownList_Assigned_To_Potn.Items.Add(lt1);
                }
            }
            ListItem emptyUser = new ListItem();
            emptyUser.Text = "_";
            emptyUser.Value = "_";

            DropDownList_Assigned_To_Lead.Items.Add(emptyUser);
            DropDownList_Assigned_To_Potn.Items.Add(emptyUser);
            DropDownList_Assigned_To_Lead.SelectedValue = "_";
            DropDownList_Assigned_To_Potn.SelectedValue = "_";
        }
        /// <summary>
        /// This method will check access to different buttons and enable/disable based on access
        /// </summary>
        protected void CheckAccessToActions()
        {
            Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

            if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
            {
                Button_Create_Req0.Enabled = true;
                //Button_Convert_Lead.Enabled = true;
                Button_Create_Pot.Enabled = true;
                Button_Create_Invoice_Manual.Enabled = true;
                Button_Create_PO.Enabled = true;
            }
            else
            {
                if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_CREATE_LEAD])
                    Button_Create_Req0.Enabled = false;
                if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_CONVERT_LEAD])
                    Button_Convert_Lead.Enabled = false;
                if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_CREATE_POTENTIAL])
                    Button_Create_Pot.Enabled = false;
                if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_CREATE_INVOICE_SALES])
                    Button_Create_Invoice_Manual.Enabled = false;
                if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_CREATE_SO_SALES])
                    Button_Create_PO.Enabled = false;
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

        protected void LoadProductCatAndContacts()
        {
            Dictionary<String, ProductCategory> prodDict = BackEndObjects.ProductCategory.getAllParentCategory();
            ListItem firstItem = new ListItem();
            firstItem.Text = "_";
            firstItem.Value = "_";
            foreach (KeyValuePair<String, ProductCategory> kvp in prodDict)
            {
                ListItem ltProd = new ListItem();
                ltProd.Text = ((BackEndObjects.ProductCategory)kvp.Value).getProductCategoryName();
                ltProd.Value = kvp.Key.ToString();
                DropDownList_Category_Lead.Items.Add(ltProd);
                DropDownList_Category_Pot.Items.Add(ltProd);
                DropDownList_Prod_Cat.Items.Add(ltProd);
            }
            DropDownList_Prod_Cat.Items.Add(firstItem);
            DropDownList_Category_Lead.Items.Add(firstItem);
            DropDownList_Category_Pot.Items.Add(firstItem);
            DropDownList_Contact_Lead.Items.Add(firstItem);
            DropDownList_Contact_Potn.Items.Add(firstItem);
            DropDownList_Contact_Inv.Items.Add(firstItem);
            DropDownList_Client_SO.Items.Add(firstItem);

            //ArrayList contactList = BackEndObjects.Contacts.getAllContactsbyEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            Dictionary<String, String> existingContactDict = (Dictionary<String, String>)Session[SessionFactory.EXISTING_CONTACT_DICTIONARY];
            if(existingContactDict!=null)
            foreach (KeyValuePair<String, String> kvp in existingContactDict)
            {
                String contactName = kvp.Key;
                String contactEntId = kvp.Value;

                ListItem ltItem = new ListItem();
                ltItem.Text = contactName;
                ltItem.Value = contactEntId;
                DropDownList_Contact_Lead.Items.Add(ltItem);
                DropDownList_Contact_Potn.Items.Add(ltItem);
                DropDownList_Contact_Inv.Items.Add(ltItem);
                DropDownList_Client_SO.Items.Add(ltItem);
            }


            DropDownList_Contact_Potn.SelectedValue = "_";
            DropDownList_Contact_Lead.SelectedValue = "_";
            DropDownList_Category_Lead.SelectedValue = "_";
            DropDownList_Category_Pot.SelectedValue = "_";
            DropDownList_Prod_Cat.SelectedValue = "_";
            DropDownList_Contact_Inv.SelectedValue = "_";
            DropDownList_Client_SO.SelectedValue = "_";
        }

        protected void populateInvFilter()
        {
            ListItem lt = new ListItem();
            lt.Text = "_";
            lt.Value = "_";

            DropDownList_Deliv_Stat.Items.Add(lt);
            DropDownList_Pmnt_Stat.Items.Add(lt);

            ListItem ltD1 = new ListItem();
            ltD1.Text = BackEndObjects.DeliveryStat.DELIV_STAT_DELIVERED;
            ltD1.Value = BackEndObjects.DeliveryStat.DELIV_STAT_DELIVERED;

            ListItem ltD2 = new ListItem();
            ltD2.Text = BackEndObjects.DeliveryStat.DELIV_STAT_UNDELIVERED;
            ltD2.Value = BackEndObjects.DeliveryStat.DELIV_STAT_UNDELIVERED;

            DropDownList_Deliv_Stat.Items.Add(ltD1);
            DropDownList_Deliv_Stat.Items.Add(ltD2);

            ListItem ltP1 = new ListItem();
            ltP1.Text = BackEndObjects.PaymentStat.PATMENT_STAT_COMPLETE;
            ltP1.Value = BackEndObjects.PaymentStat.PATMENT_STAT_COMPLETE;

            ListItem ltP2 = new ListItem();
            ltP2.Text = BackEndObjects.PaymentStat.PAYMENT_STAT_INCOMPLETE;
            ltP2.Text = BackEndObjects.PaymentStat.PAYMENT_STAT_INCOMPLETE;

            DropDownList_Pmnt_Stat.Items.Add(ltP1);
            DropDownList_Pmnt_Stat.Items.Add(ltP2);


            DropDownList_Pmnt_Stat.SelectedValue = "_";
            DropDownList_Deliv_Stat.SelectedValue = "_";

        }

        protected void fillInvGridwithFilter()
        {
                                                Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

                                                if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_INVOICE_SALES]||
                                                    accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                                                {

                                                    String invNo = TextBox_Inv_No.Text;
                                                    String prodCatId = DropDownList_Prod_Cat.SelectedValue;
                                                    String delivStat = DropDownList_Deliv_Stat.SelectedValue;
                                                    String pmntStat = DropDownList_Pmnt_Stat.SelectedValue;
                                                    String fromDate = TextBox_From_Date_Invoice.Text;
                                                    String toDate = TextBox_To_Date_Inv.Text;
                                                    String rfqNo = TextBox_RFQ_No.Text;
                                                    String tranNo = TextBox_Tran_No.Text;
                                                    String customer = DropDownList_Contact_Inv.SelectedValue;

                                                    ActionLibrary.PurchaseActions._dispInvoiceDetails dInv = new ActionLibrary.PurchaseActions._dispInvoiceDetails();

                                                    Dictionary<String, String> filterParams = new Dictionary<string, string>();

                                                    if (rfqNo != null && !rfqNo.Equals(""))
                                                        filterParams.Add(dInv.FILTER_BY_RFQ_NO, rfqNo);
                                                    if (invNo != null && !invNo.Equals(""))
                                                        filterParams.Add(dInv.FILTER_BY_INVOICE_NO, invNo);
                                                    if (prodCatId != null && !prodCatId.Equals("_"))
                                                        filterParams.Add(dInv.FILTER_BY_PRODUCT_CAT, prodCatId);
                                                    if (delivStat != null && !delivStat.Equals("") && !delivStat.Equals("_"))
                                                        filterParams.Add(dInv.FILTER_BY_DELIV_STAT, delivStat);
                                                    if (pmntStat != null && !pmntStat.Equals("") && !pmntStat.Equals("_"))
                                                        filterParams.Add(dInv.FILTER_BY_PMNT_STAT, pmntStat);
                                                    if (fromDate != null && !fromDate.Equals(""))
                                                        filterParams.Add(dInv.FILTER_BY_FROM_DATE, fromDate);
                                                    if (toDate != null && !toDate.Equals(""))
                                                        filterParams.Add(dInv.FILTER_BY_TO_DATE, toDate);
                                                    if(tranNo!=null && !tranNo.Equals(""))
                                                        filterParams.Add(dInv.FILTER_BY_TRAN_NO, tranNo);
                                                    if (customer != null && !customer.Equals("_"))
                                                        filterParams.Add(dInv.FILTER_BY_CUSTOMER, customer);
                                                    //ArrayList rfqList = BackEndObjects.RFQDetails.getAllRFQbyEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

                                                    ArrayList invList = BackEndObjects.Invoice.getAllInvoicesbyRespEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                                                    /*for (int i = 0; i < rfqList.Count; i++)
                                                    {
                                                        Invoice invObj = BackEndObjects.Invoice.getInvoicebyRfIdDB(((RFQDetails)rfqList[i]).getRFQId());
                                                        invList.Add(invObj);
                                                    }*/

                                                    fillInvoiceGrid(dInv.getAllInvDetailsFiltered(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), invList, filterParams));
                                                }
        }

        protected void fillInvoiceGrid(ArrayList invL)
        {
                                    Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];
                                    Dictionary<String, Currency> allCurrList = (Dictionary<String, Currency>)Session[SessionFactory.CURRENCY_LIST];

                                    if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_INVOICE_SALES]||
                                        accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                                    {
                                        Label_Invoice_Grid_Access.Visible = false;

                                        Dictionary<String, String> existInvNoDict = (Dictionary<String,String>)Session[SessionFactory.ALL_SALE_ALL_INV_EXISTING_INV_NO];
                                        DateUtility dU = new DateUtility();

                                        if (existInvNoDict == null)
                                            existInvNoDict = new Dictionary<String, String>();

                                        DataTable dt = new DataTable();
                                        dt.Columns.Add("RFQNo");
                                        dt.Columns.Add("Inv_No");
                                        dt.Columns.Add("Inv_Id");
                                        dt.Columns.Add("Inv_Date");
                                        dt.Columns.Add("Inv_Date_Ticks");
                                        dt.Columns.Add("Deliv_Stat");
                                        dt.Columns.Add("Pmnt_Stat");
                                            dt.Columns.Add("Amount");
                                            dt.Columns.Add("approval");
                                            dt.Columns.Add("curr");
                                            dt.Columns.Add("Related_PO");

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
                                            dt.Rows[i]["Inv_Date_Ticks"] = !dt.Rows[i]["Inv_Date"].Equals("")?Convert.ToDateTime(((BackEndObjects.Invoice)invL[i]).getInvoiceDate()).Ticks:0;
                                            dt.Rows[i]["Deliv_Stat"] = ((BackEndObjects.Invoice)invL[i]).getDeliveryStatus();
                                            dt.Rows[i]["Pmnt_Stat"] = ((BackEndObjects.Invoice)invL[i]).getPaymentStatus();
                                            dt.Rows[i]["Amount"] = ((BackEndObjects.Invoice)invL[i]).getTotalAmount();
                                            dt.Rows[i]["approval"] = (((BackEndObjects.Invoice)invL[i]).getApprovalStat() == null || ((BackEndObjects.Invoice)invL[i]).getApprovalStat().Equals("") ?
                                                "N/A" : ((BackEndObjects.Invoice)invL[i]).getApprovalStat());
                                            dt.Rows[i]["curr"] = ((BackEndObjects.Invoice)invL[i]).getCurrency() != null &&
                        allCurrList.ContainsKey(((BackEndObjects.Invoice)invL[i]).getCurrency()) ?
                                   allCurrList[((BackEndObjects.Invoice)invL[i]).getCurrency()].getCurrencyName() : "";
                                            dt.Rows[i]["Related_PO"] = ((BackEndObjects.Invoice)invL[i]).getRelatedPO();

                                            if(!existInvNoDict.ContainsKey(((BackEndObjects.Invoice)invL[i]).getInvoiceNo()))
                                                existInvNoDict.Add(((BackEndObjects.Invoice)invL[i]).getInvoiceNo(),((BackEndObjects.Invoice)invL[i]).getInvoiceId());
                                        }

                                        dt.DefaultView.Sort = "Inv_Date_Ticks" + " " + "DESC";
                                        disableOnPageChange("inv");
                                        GridView_Invoice.SelectedIndex = -1;
                                        GridView_Invoice.DataSource = dt;
                                        GridView_Invoice.DataBind();
                                        GridView_Invoice.Visible = true;

                                        if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_INVOICE_SALES] &&
    !accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                                            GridView_Invoice.Columns[1].Visible = false;

                                        Session[SessionFactory.ALL_SALE_ALL_INV_GRID_DATA] = dt.DefaultView.ToTable();
                                        Session[SessionFactory.ALL_SALE_ALL_INV_EXISTING_INV_NO]=existInvNoDict;
                                    }
                                    else
                                    {
                                        Label_Invoice_Grid_Access.Visible = true;
                                        Label_Invoice_Grid_Access.Text = "You don't have access to view this section";
                                    }
        }

        protected void LoadReqrActiveStat()
        {
           /* ListItem lt = new ListItem();
            lt.Text = BackEndObjects.Requirement.REQ_ACTIVE_STAT_ACTIVE;
            lt.Value = BackEndObjects.Requirement.REQ_ACTIVE_STAT_ACTIVE;

            ListItem lt1 = new ListItem();
            lt1.Text = BackEndObjects.Requirement.REQ_ACTIVE_STAT_INACTIVE;
            lt1.Value = BackEndObjects.Requirement.REQ_ACTIVE_STAT_INACTIVE;*/

            ListItem ltRFQ1 = new ListItem();
            ltRFQ1.Text = BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_ACTIVE;
            ltRFQ1.Value = BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_ACTIVE;

            ListItem ltRFQ2 = new ListItem();
            ltRFQ2.Text = BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_NONE;
            ltRFQ2.Value = BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_NONE;

            ListItem ltRFQ3 = new ListItem();
            ltRFQ3.Text = BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_NOT_OPEN;
            ltRFQ3.Value = BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_NOT_OPEN;

            //DropDownList_Status.Items.Add(lt);
            DropDownList_Lead_Active_Stat.Items.Add(ltRFQ1);
            //DropDownList_Status.Items.Add(lt1);
            DropDownList_Lead_Active_Stat.Items.Add(ltRFQ2);
            DropDownList_Lead_Active_Stat.Items.Add(ltRFQ3);
            //DropDownList_Status.SelectedIndex = -1;
            DropDownList_Lead_Active_Stat.SelectedValue = BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_NONE;

        }

        protected DataTable getLeadGridDataTable()
        {
            String entityId = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();
            Dictionary<String, Currency> allCurrList = (Dictionary<String, Currency>)Session[SessionFactory.CURRENCY_LIST];

            ActionLibrary.SalesActions._dispLeads dspLead = new ActionLibrary.SalesActions._dispLeads();

            ArrayList leadList = dspLead.getAllLeadDetailsWOProdQntyAndSpec(entityId, User.Identity.Name);

            DataTable dt = new DataTable();
            dt.Columns.Add("RFQNo");
            dt.Columns.Add("RFQName");//Lead_Responded
            dt.Columns.Add("Lead_Alert_Required");
            dt.Columns.Add("CustName");
            dt.Columns.Add("CustId");
            dt.Columns.Add("curr");
            dt.Columns.Add("Spec");
            dt.Columns.Add("Submit Date");
            dt.Columns.Add("Submit Date Ticks");
            dt.Columns.Add("Due Date");
            dt.Columns.Add("Due Date Ticks");
            dt.Columns.Add("Next Date");
            dt.Columns.Add("Next Date Ticks");
            dt.Columns.Add("Assgn To");
            dt.Columns.Add("location");
            dt.Columns.Add("TC");
            dt.Columns.Add("NDA");
            dt.Columns.Add("ActiveStat");
            dt.Columns.Add("Audit");
            dt.Columns.Add("Mode");

            Dictionary<String, bool> rfqRespDict = BackEndObjects.RFQResponseQuotes.getAllRFQWithNonEmptyResponseQuotesforResponseEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

            ArrayList rfqList = new ArrayList();
            for (int i = 0; i < leadList.Count; i++)
            {
                if(!(((LeadRecord)leadList[i]).getRFQId().IndexOf("dummy")>=0))
                rfqList.Add(((LeadRecord)leadList[i]).getRFQId());
            }
            Dictionary<String, RFQResponse> rfqResp = RFQResponse.
                getRFQResponseDictWOQuotesForRFQIdListandResponseEntityIdDB(rfqList, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            
            int unAnsCount = 0;
            DateUtility dU = new DateUtility();
            
            int rowCount = 0;
            for (int i = 0; i < leadList.Count; i++)
            {
                if (!(((LeadRecord)leadList[i]).getRFQId().IndexOf("dummy") >= 0))
                {
                    dt.Rows.Add();
                    dt.Rows[rowCount]["RFQNo"] = ((ActionLibrary.LeadRecord)leadList[i]).getRFQId();
                    dt.Rows[rowCount]["RFQName"] = ((ActionLibrary.LeadRecord)leadList[i]).getRFQName();
                    dt.Rows[rowCount]["Lead_Alert_Required"] = (rfqRespDict != null &&
                        rfqRespDict.ContainsKey(((ActionLibrary.LeadRecord)leadList[i]).getRFQId()) ? "false" : "true");
                    unAnsCount = (dt.Rows[rowCount]["Lead_Alert_Required"].Equals("true") ? unAnsCount + 1 : unAnsCount);

                    dt.Rows[rowCount]["CustName"] = ((ActionLibrary.LeadRecord)leadList[i]).getEntityName();
                    dt.Rows[rowCount]["CustId"] = ((ActionLibrary.LeadRecord)leadList[i]).getEntityId();
                    dt.Rows[rowCount]["curr"] = ((ActionLibrary.LeadRecord)leadList[i]).getCurrency() != null &&
                            allCurrList.ContainsKey(((ActionLibrary.LeadRecord)leadList[i]).getCurrency()) ?
                                       allCurrList[((ActionLibrary.LeadRecord)leadList[i]).getCurrency()].getCurrencyName() : "";
                    //dt.Rows[i]["Spec"] = ((ActionLibrary.LeadRecord)leadList[i]).getRF
                    dt.Rows[rowCount]["Submit Date"] = dU.getConvertedDate(((ActionLibrary.LeadRecord)leadList[i]).getSubmitDate());
                    dt.Rows[rowCount]["Submit Date Ticks"] = Convert.ToDateTime(((ActionLibrary.LeadRecord)leadList[i]).getSubmitDate()).Ticks;
                    dt.Rows[rowCount]["Due Date"] = dU.getConvertedDate(((ActionLibrary.LeadRecord)leadList[i]).getDueDate().Substring(0, ((ActionLibrary.LeadRecord)leadList[i]).getDueDate().IndexOf(" ")));
                    dt.Rows[rowCount]["Due Date Ticks"] = !dt.Rows[rowCount]["Due Date"].Equals("") ? Convert.ToDateTime(((ActionLibrary.LeadRecord)leadList[i]).getDueDate()).Ticks : 0;

                    //RFQResponse respObj = RFQResponse.getRFQResponseWOQuotesForRFQIdandResponseEntityIdDB(((ActionLibrary.LeadRecord)leadList[i]).getRFQId(),
                    //Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                    RFQResponse respObj = rfqResp.ContainsKey(((ActionLibrary.LeadRecord)leadList[i]).getRFQId()) ?
                        rfqResp[((ActionLibrary.LeadRecord)leadList[i]).getRFQId()] : null;

                    String nextDate = (respObj != null && respObj.getNextFollowupDate() != null && !respObj.getNextFollowupDate().Equals("") ? respObj.getNextFollowupDate().Substring(0, respObj.getNextFollowupDate().IndexOf(" ")) : "");
                    dt.Rows[rowCount]["Next Date"] = dU.getConvertedDate(nextDate);
                    dt.Rows[rowCount]["Next Date Ticks"] = (!nextDate.Equals("") ? Convert.ToDateTime(nextDate).Ticks : 0);

                    dt.Rows[rowCount]["Assgn To"] = (respObj != null && respObj.getAssignedTo() != null ? respObj.getAssignedTo() : "");
                    dt.Rows[rowCount]["ActiveStat"] = ((ActionLibrary.LeadRecord)leadList[i]).getActiveStat();
                    //dt.Rows[i]["location"] = ((ActionLibrary.LeadRecord)leadList[i]).getRFQId();
                    //dt.Rows[i]["TC"] = ((ActionLibrary.LeadRecord)leadList[i]).getRFQId();
                    //dt.Rows[i]["NDA"] = ((ActionLibrary.LeadRecord)leadList[i]).getRFQId();
                    //dt.Rows[i]["Audit"] = ((ActionLibrary.LeadRecord)leadList[i]).getRFQId();
                    dt.Rows[rowCount]["Mode"] = ((ActionLibrary.LeadRecord)leadList[i]).getCreateMode();

                    rowCount++;
                }
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
        /// <summary>
        /// Potential status is indicative of the potential stage.
        /// Whereas the potential status is indicative of the potential active status.
        /// </summary>
        protected void loadPotStageAndStats()
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

            //RFQ Active status names will be shown as the potential active status as well
            ListItem ltAct1 = new ListItem();
            ltAct1.Text = BackEndObjects.RFQActiveStat.RFQ_ACTIVE_STAT_ACTIVE;
            ltAct1.Value = BackEndObjects.RFQActiveStat.RFQ_ACTIVE_STAT_ACTIVE;

/*            ListItem ltAct2 = new ListItem();
            ltAct2.Text = BackEndObjects.RFQShortlisted.POTENTIAL_ACTIVE_STAT_ACTIVE;
            ltAct2.Value = BackEndObjects.RFQShortlisted.POTENTIAL_ACTIVE_STAT_ACTIVE;*/


            ListItem ltAct3 = new ListItem();
            ltAct3.Text = BackEndObjects.RFQShortlisted.POTENTIAL_ACTIVE_STAT_NOT_OPEN;
            ltAct3.Value = BackEndObjects.RFQShortlisted.POTENTIAL_ACTIVE_STAT_NOT_OPEN;

            ListItem ltAct4 = new ListItem();
            ltAct4.Text = BackEndObjects.RFQShortlisted.POTENTIAL_ACTIVE_STAT_CLOSED_LOST;
            ltAct4.Value = BackEndObjects.RFQShortlisted.POTENTIAL_ACTIVE_STAT_CLOSED_LOST;


            ListItem ltAct5 = new ListItem();
            ltAct5.Text = BackEndObjects.RFQShortlisted.POTENTIAL_ACTIVE_STAT_CLOSED_WON;
            ltAct5.Value = BackEndObjects.RFQShortlisted.POTENTIAL_ACTIVE_STAT_CLOSED_WON;


            /*ListItem ltAct6 = new ListItem();
            ltAct6.Text = BackEndObjects.RFQActiveStat.RFQ_ACTIVE_STAT_NOT_OPEN;
            ltAct6.Value = BackEndObjects.RFQActiveStat.RFQ_ACTIVE_STAT_NOT_OPEN;*/

            ListItem ltAct7 = new ListItem();
            ltAct7.Text = "_";
            ltAct7.Value = "_";

            DropDownList_Pot_Active_Stat.Items.Add(ltAct1);
            //DropDownList_Pot_Active_Stat.Items.Add(ltAct2);
            DropDownList_Pot_Active_Stat.Items.Add(ltAct3);
            DropDownList_Pot_Active_Stat.Items.Add(ltAct4);
            DropDownList_Pot_Active_Stat.Items.Add(ltAct5);
            //DropDownList_Pot_Active_Stat.Items.Add(ltAct6);
            DropDownList_Pot_Active_Stat.Items.Add(ltAct7);

            DropDownList_Pot_Active_Stat.SelectedValue = "_";
        }

        protected DataTable getLeadGridDataTable(ArrayList leadList)
        {
            String entityId = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();
            Dictionary<String, Currency> allCurrList = (Dictionary<String, Currency>)Session[SessionFactory.CURRENCY_LIST];

            ActionLibrary.SalesActions._dispLeads dspLead = new ActionLibrary.SalesActions._dispLeads();

            //ArrayList leadList = dspLead.getAllLeadDetails(entityId, User.Identity.Name);

            DataTable dt = new DataTable();
            dt.Columns.Add("RFQNo");
            dt.Columns.Add("RFQName");
            dt.Columns.Add("Lead_Alert_Required");
            dt.Columns.Add("CustName");
            dt.Columns.Add("CustId");
            dt.Columns.Add("curr");
            dt.Columns.Add("Spec");
            dt.Columns.Add("Submit Date");
            dt.Columns.Add("Submit Date Ticks");
            dt.Columns.Add("Due Date");
            dt.Columns.Add("Due Date Ticks");
            dt.Columns.Add("Next Date");
            dt.Columns.Add("Next Date Ticks");
            dt.Columns.Add("Assgn To");
            dt.Columns.Add("location");
            dt.Columns.Add("TC");
            dt.Columns.Add("NDA");
            dt.Columns.Add("ActiveStat");
            dt.Columns.Add("Audit");
            dt.Columns.Add("Mode");

            Dictionary<String, bool> rfqRespDict = BackEndObjects.RFQResponseQuotes.getAllRFQWithNonEmptyResponseQuotesforResponseEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            DateUtility dU = new DateUtility();
            int unAnsCount = 0;

            ArrayList rfqList = new ArrayList();
            for (int i = 0; i < leadList.Count; i++)
            {
                if (!(((LeadRecord)leadList[i]).getRFQId().IndexOf("dummy") >= 0))
                rfqList.Add(((LeadRecord)leadList[i]).getRFQId());
            }
            Dictionary<String, RFQResponse> rfqResp = (rfqList!=null&& rfqList.Count>0)?RFQResponse.
                getRFQResponseDictWOQuotesForRFQIdListandResponseEntityIdDB(rfqList, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()):new Dictionary<String, RFQResponse>();

            int rowCount = 0;
            for (int i = 0; i < leadList.Count; i++)
            {
                if (!(((LeadRecord)leadList[i]).getRFQId().IndexOf("dummy") >= 0))
                {
                    dt.Rows.Add();
                    dt.Rows[rowCount]["RFQNo"] = ((ActionLibrary.LeadRecord)leadList[i]).getRFQId();
                    dt.Rows[rowCount]["RFQName"] = ((ActionLibrary.LeadRecord)leadList[i]).getRFQName();
                    dt.Rows[rowCount]["Lead_Alert_Required"] = (rfqRespDict != null &&
        rfqRespDict.ContainsKey(((ActionLibrary.LeadRecord)leadList[i]).getRFQId()) ? "false" : "true");
                    unAnsCount = (dt.Rows[rowCount]["Lead_Alert_Required"].Equals("true") ? unAnsCount + 1 : unAnsCount);

                    dt.Rows[rowCount]["CustName"] = ((ActionLibrary.LeadRecord)leadList[i]).getEntityName();
                    dt.Rows[rowCount]["CustId"] = ((ActionLibrary.LeadRecord)leadList[i]).getEntityId();
                    dt.Rows[rowCount]["curr"] = ((ActionLibrary.LeadRecord)leadList[i]).getCurrency() != null &&
                            allCurrList.ContainsKey(((ActionLibrary.LeadRecord)leadList[i]).getCurrency()) ?
                                       allCurrList[((ActionLibrary.LeadRecord)leadList[i]).getCurrency()].getCurrencyName() : "";
                    //dt.Rows[rowCount]["Spec"] = ((ActionLibrary.LeadRecord)leadList[i]).getRF
                    dt.Rows[rowCount]["Submit Date"] = dU.getConvertedDate(((ActionLibrary.LeadRecord)leadList[i]).getSubmitDate());
                    dt.Rows[rowCount]["Submit Date Ticks"] = Convert.ToDateTime(((ActionLibrary.LeadRecord)leadList[i]).getSubmitDate()).Ticks;
                    dt.Rows[rowCount]["Due Date"] = dU.getConvertedDate(((ActionLibrary.LeadRecord)leadList[i]).getDueDate().Substring(0, ((ActionLibrary.LeadRecord)leadList[i]).getDueDate().IndexOf(" ")));
                    dt.Rows[rowCount]["Due Date Ticks"] = !dt.Rows[rowCount]["Due Date"].Equals("") ? Convert.ToDateTime(((ActionLibrary.LeadRecord)leadList[i]).getDueDate()).Ticks : 0;

                    //RFQResponse respObj = RFQResponse.getRFQResponseWOQuotesForRFQIdandResponseEntityIdDB(((ActionLibrary.LeadRecord)leadList[i]).getRFQId(),
                    //Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                    RFQResponse respObj = rfqResp.ContainsKey(((ActionLibrary.LeadRecord)leadList[i]).getRFQId()) ?
        rfqResp[((ActionLibrary.LeadRecord)leadList[i]).getRFQId()] : null;

                    String nextDate = (respObj != null && respObj.getNextFollowupDate() != null && !respObj.getNextFollowupDate().Equals("") ? respObj.getNextFollowupDate().Substring(0, respObj.getNextFollowupDate().IndexOf(" ")) : "");
                    dt.Rows[rowCount]["Next Date"] = dU.getConvertedDate(nextDate);
                    dt.Rows[rowCount]["Next Date Ticks"] = (nextDate != null && !nextDate.Equals("") ? Convert.ToDateTime(nextDate).Ticks : 0);
                    dt.Rows[rowCount]["Assgn To"] = (respObj != null && respObj.getAssignedTo() != null ? respObj.getAssignedTo() : "");


                    dt.Rows[rowCount]["ActiveStat"] = ((ActionLibrary.LeadRecord)leadList[i]).getActiveStat();
                    //dt.Rows[rowCount]["location"] = ((ActionLibrary.LeadRecord)leadList[i]).getRFQId();
                    //dt.Rows[rowCount]["TC"] = ((ActionLibrary.LeadRecord)leadList[i]).getRFQId();
                    //dt.Rows[rowCount]["NDA"] = ((ActionLibrary.LeadRecord)leadList[i]).getRFQId();
                    //dt.Rows[rowCount]["Audit"] = ((ActionLibrary.LeadRecord)leadList[i]).getRFQId();
                    dt.Rows[rowCount]["Mode"] = ((ActionLibrary.LeadRecord)leadList[i]).getCreateMode();
                    rowCount++;
                }
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

        protected void fillLeadGrid()
        {
                                                Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

                                                if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_LEAD]||
                                                    accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                                                {
                                                    DataTable dt = getLeadGridDataTable();
                                                    dt.DefaultView.Sort = "Submit Date Ticks"+" "+"DESC";
                                                    Session[SessionFactory.ALL_SALE_ALL_LEAD_GRID_DATA] = dt.DefaultView.ToTable();

                                                   GridView_Lead.DataSource = dt;
                                                    GridView_Lead.DataBind();
                                                    GridView_Lead.Visible = true;

                                                    if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_LEAD] &&
                                                               !accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                                                        GridView_Lead.Columns[1].Visible = false;
                                                    //GridView_Lead.Sort("Submit Date", SortDirection.Descending);
                                                }
                                                else
                                                {
                                                    Label_Lead_Grid_Access.Visible = true;
                                                    Label_Lead_Grid_Access.Text = "You don't have access to view this section";
                                                }
        }

        protected DataTable getPotnGridDataTable()
        {
            String RespEntityId = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();
            Dictionary<String, Currency> allCurrList = (Dictionary<String, Currency>)Session[SessionFactory.CURRENCY_LIST];

            ActionLibrary.SalesActions._dispPotentials dP = new SalesActions._dispPotentials();

            ArrayList potList=dP.getAllPotentials(RespEntityId, User.Identity.Name);

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
            dt.Columns.Add("Assgn To");
            dt.Columns.Add("CustName");
            dt.Columns.Add("CustId");
            dt.Columns.Add("PotAmnt");
            dt.Columns.Add("DealRequest");
            dt.Columns.Add("PotStage");
            dt.Columns.Add("ActiveStat");
            dt.Columns.Add("PO_Id");
            dt.Columns.Add("Mode");
            dt.Columns.Add("curr");

            Dictionary<String, bool> rfqRespDict = BackEndObjects.RFQResponseQuotes.getAllRFQWithNonEmptyResponseQuotesforResponseEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

            ArrayList rfqList = new ArrayList();
            for (int i = 0; i < potList.Count; i++)
            {
                //PotentialRecords potRec = (PotentialRecords)potList[i];
                if (!(((PotentialRecords)potList[i]).getRFQId().IndexOf("dummy") >= 0))
                rfqList.Add(((PotentialRecords)potList[i]).getRFQId());
            }
            Dictionary<String, RFQResponse> rfqResp = RFQResponse.
    getRFQResponseDictWOQuotesForRFQIdListandResponseEntityIdDB(rfqList, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

            Dictionary<String, String> poDict = PurchaseOrder.getPurchaseOrdersforRFQIdListDB(rfqList);

            int unAnsCount = 0;
            DateUtility dU = new DateUtility();

            int rowCount = 0;
            for (int i = 0; i < potList.Count;i++ )
            {
                PotentialRecords potRec = (PotentialRecords)potList[i];

                if (!(potRec.getRFQId().IndexOf("dummy") >= 0))
                {
                    dt.Rows.Add();

                    dt.Rows[rowCount]["PotId"] = potRec.getPotentialId();
                    dt.Rows[rowCount]["RFQNo"] = potRec.getRFQId();
                    dt.Rows[rowCount]["RFQName"] = potRec.getRFQName();
                    dt.Rows[rowCount]["Potn_Alert_Required"] = (potRec.getPotenAmnt() == 0 ? "true" : "false");
                    //(rfqRespDict != null &&rfqRespDict.ContainsKey(potRec.getRFQId()) ? "false" : "true");
                    unAnsCount = (dt.Rows[rowCount]["Potn_Alert_Required"].Equals("true") ? unAnsCount + 1 : unAnsCount);

                    dt.Rows[rowCount]["DateCreated"] = dU.getConvertedDate(potRec.getCreatedDate());
                    dt.Rows[rowCount]["DateCreatedTicks"] = Convert.ToDateTime(potRec.getCreatedDate()).Ticks;
                    dt.Rows[rowCount]["Due Date"] = dU.getConvertedDate(potRec.getDueDate().Substring(0, potRec.getDueDate().IndexOf(" ")));
                    dt.Rows[rowCount]["Due Date Ticks"] = !dt.Rows[rowCount]["Due Date"].Equals("") ? Convert.ToDateTime(potRec.getDueDate()).Ticks : 0;
                    dt.Rows[rowCount]["CustName"] = potRec.getEntityName();
                    dt.Rows[rowCount]["CustId"] = potRec.getEntityId();
                    dt.Rows[rowCount]["PotAmnt"] = potRec.getPotenAmnt();
                    dt.Rows[rowCount]["DealRequest"] = (potRec.getCreateMode().Equals(BackEndObjects.RFQShortlisted.POTENTIAL_CREATION_MODE_MANUAL) ? "" :
                        potRec.getFinlSupFlag());
                    dt.Rows[rowCount]["PotStage"] = potRec.getPotStat();
                    dt.Rows[rowCount]["ActiveStat"] = potRec.getPotActStat();

                    /*RFQResponse respObj = RFQResponse.
                        getRFQResponseWOQuotesForRFQIdandResponseEntityIdDB(potRec.getRFQId(),
        Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());*/

                    RFQResponse respObj = rfqResp != null && rfqResp.ContainsKey(potRec.getRFQId()) ?
                        rfqResp[potRec.getRFQId()] : null;

                    String nextDate = (respObj != null && respObj.getNextFollowupDate() != null && !respObj.getNextFollowupDate().Equals("") ? respObj.getNextFollowupDate().Substring(0, respObj.getNextFollowupDate().IndexOf(" ")) : "");
                    dt.Rows[rowCount]["Next Date"] = dU.getConvertedDate(nextDate);
                    dt.Rows[rowCount]["Next Date Ticks"] = (nextDate != null && !nextDate.Equals("") ? Convert.ToDateTime(nextDate).Ticks : 0);

                    dt.Rows[rowCount]["Assgn To"] = (respObj != null && respObj.getAssignedTo() != null ? respObj.getAssignedTo() : "");

                    String poId = potRec.getPotStat().Equals(RFQShortlisted.POTENTIAL_ACTIVE_STAT_CLOSED_WON) ?
                        (poDict.ContainsKey(potRec.getRFQId()) ? poDict[potRec.getRFQId()] : "") : "";
                    //PurchaseOrder.getPurchaseOrderforRFQIdDB(potRec.getRFQId()).getPo_id() : "";
                    dt.Rows[rowCount]["PO_Id"] = (poId == null || poId.Equals("") ? "" : poId);

                    dt.Rows[rowCount]["Mode"] = potRec.getCreateMode();
                    dt.Rows[rowCount]["curr"] = potRec.getCurrency() != null && allCurrList.ContainsKey(potRec.getCurrency()) ?
                        allCurrList[potRec.getCurrency()].getCurrencyName() : "";
                    rowCount++;
                }
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
            //Session[SessionFactory.ALL_SALE_ALL_POTN_RFQ_LIST] = rfqList;
            return dt;
        }

        protected DataTable getPotnGridDataTable(ArrayList potList)
        {
            String RespEntityId = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();
            Dictionary<String, Currency> allCurrList = (Dictionary<String, Currency>)Session[SessionFactory.CURRENCY_LIST];
            ActionLibrary.SalesActions._dispPotentials dP = new SalesActions._dispPotentials();

            //ArrayList potList = dP.getAllPotentials(RespEntityId, User.Identity.Name);

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
            dt.Columns.Add("Assgn To");
            dt.Columns.Add("CustName");
            dt.Columns.Add("CustId");
            dt.Columns.Add("PotAmnt");
            dt.Columns.Add("DealRequest");
            dt.Columns.Add("PotStage");
            dt.Columns.Add("ActiveStat");
            dt.Columns.Add("PO_Id");
            dt.Columns.Add("Mode");
            dt.Columns.Add("curr");

            Dictionary<String, bool> rfqRespDict = BackEndObjects.RFQResponseQuotes.getAllRFQWithNonEmptyResponseQuotesforResponseEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

            ArrayList rfqList = new ArrayList();
            for (int i = 0; i < potList.Count; i++)
            {
                //PotentialRecords potRec = (PotentialRecords)potList[i];
                if(!(((PotentialRecords)potList[i]).getRFQId().IndexOf("dummy")>=0))
                rfqList.Add(((PotentialRecords)potList[i]).getRFQId());
            }
            Dictionary<String, String> poDict = (rfqList != null && rfqList .Count>0)?
                PurchaseOrder.getPurchaseOrdersforRFQIdListDB(rfqList): new Dictionary<String, String>();
            Dictionary<String, RFQResponse> rfqResp = (rfqList != null && rfqList.Count > 0) ?
                RFQResponse.
    getRFQResponseDictWOQuotesForRFQIdListandResponseEntityIdDB
    (rfqList, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()):new Dictionary<String, RFQResponse>();


            DateUtility dU = new DateUtility();
            int unAnsCount = 0;
            int rowCount = 0;

            for (int i = 0; i < potList.Count; i++)
            {
                PotentialRecords potRec = (PotentialRecords)potList[i];

                if (!(potRec.getRFQId().IndexOf("dummy") >= 0))
                {
                    dt.Rows.Add();

                    dt.Rows[rowCount]["PotId"] = potRec.getPotentialId();
                    dt.Rows[rowCount]["RFQNo"] = potRec.getRFQId();
                    dt.Rows[rowCount]["RFQName"] = potRec.getRFQName();
                    dt.Rows[rowCount]["Potn_Alert_Required"] = (rfqRespDict != null &&
    rfqRespDict.ContainsKey(potRec.getRFQId()) ? "false" : "true");
                    unAnsCount = (dt.Rows[rowCount]["Potn_Alert_Required"].Equals("true") ? unAnsCount + 1 : unAnsCount);

                    dt.Rows[rowCount]["DateCreated"] = dU.getConvertedDate(potRec.getCreatedDate());
                    dt.Rows[rowCount]["DateCreatedTicks"] = Convert.ToDateTime(potRec.getCreatedDate()).Ticks;
                    dt.Rows[rowCount]["Due Date"] = dU.getConvertedDate(potRec.getDueDate().Substring(0, potRec.getDueDate().IndexOf(" ")));
                    dt.Rows[rowCount]["Due Date Ticks"] = Convert.ToDateTime((potRec.getDueDate() != null && !potRec.getDueDate().Equals("") ? potRec.getDueDate() : "0")).Ticks;

                    /*RFQResponse respObj = RFQResponse.
        getRFQResponseWOQuotesForRFQIdandResponseEntityIdDB(potRec.getRFQId(),
    Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());*/
                    RFQResponse respObj = rfqResp != null && rfqResp.ContainsKey(potRec.getRFQId()) ?
        rfqResp[potRec.getRFQId()] : null;

                    String nextDate = (respObj != null && respObj.getNextFollowupDate() != null && !respObj.getNextFollowupDate().Equals("") ? respObj.getNextFollowupDate().Substring(0, respObj.getNextFollowupDate().IndexOf(" ")) : "");
                    dt.Rows[rowCount]["Next Date"] = dU.getConvertedDate(nextDate);
                    dt.Rows[rowCount]["Next Date Ticks"] = (nextDate != null && !nextDate.Equals("") ? Convert.ToDateTime(nextDate).Ticks : 0);

                    dt.Rows[rowCount]["Assgn To"] = (respObj != null && respObj.getAssignedTo() != null ? respObj.getAssignedTo() : "");

                    dt.Rows[rowCount]["CustName"] = potRec.getEntityName();
                    dt.Rows[rowCount]["CustId"] = potRec.getEntityId();
                    dt.Rows[rowCount]["PotAmnt"] = potRec.getPotenAmnt();
                    dt.Rows[rowCount]["DealRequest"] = (potRec.getCreateMode().Equals(BackEndObjects.RFQShortlisted.POTENTIAL_CREATION_MODE_MANUAL) ? "" :
                        potRec.getFinlSupFlag());
                    dt.Rows[rowCount]["PotStage"] = potRec.getPotStat();
                    dt.Rows[rowCount]["ActiveStat"] = potRec.getPotActStat();
                    dt.Rows[rowCount]["PO_Id"] = potRec.getPotStat().Equals(RFQShortlisted.POTENTIAL_ACTIVE_STAT_CLOSED_WON) ?
                        (poDict.ContainsKey(potRec.getRFQId()) ? poDict[potRec.getRFQId()] : "") : "";
                    //PurchaseOrder.getPurchaseOrderforRFQIdDB(potRec.getRFQId()).getPo_id() : "";
                    dt.Rows[rowCount]["Mode"] = potRec.getCreateMode();
                    dt.Rows[rowCount]["curr"] = potRec.getCurrency() != null && allCurrList.ContainsKey(potRec.getCurrency()) ?
                 allCurrList[potRec.getCurrency()].getCurrencyName() : "";

                    rowCount++;
                }
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

        protected void fillPotnGrid()
        {
                                                            Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

                                                            if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_POTENTIAL]||
                                                                accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                                                            {

                                                                DataTable dt = getPotnGridDataTable();
                                                                dt.DefaultView.Sort = "DateCreatedTicks" + " " + "DESC";
                                                                Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_GRID_DATA] = dt.DefaultView.ToTable();

                                                                GridView_Potential.DataSource = dt;
                                                                GridView_Potential.DataBind();
                                                                GridView_Potential.Visible = true;
                                                                GridView_Potential.Columns[2].Visible = false;

                                                                if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_POTENTIAL] &&
                                                                !accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                                                                    GridView_Potential.Columns[1].Visible = false;

                                                            }
                                                            else
                                                            {
                                                                Label_Potential_Grid_Access.Visible = true;
                                                                Label_Potential_Grid_Access.Text = "You don't have access to view this section";
                                                            }
        }

        protected void fillPotnGrid(ArrayList potList)
        {
                                                            Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

                                                            if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_POTENTIAL]||
                                                                accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                                                            {

                                                                DataTable dt = getPotnGridDataTable(potList);
                                                                dt.DefaultView.Sort = "DateCreatedTicks" + " " + "DESC";
                                                                Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_GRID_DATA] = dt.DefaultView.ToTable();
                                                                disableOnPageChange("potn");
                                                                GridView_Potential.SelectedIndex = -1;
                                                                GridView_Potential.DataSource = dt;
                                                                GridView_Potential.DataBind();
                                                                GridView_Potential.Visible = true;
                                                                GridView_Potential.Columns[2].Visible = false;

                                                                if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_POTENTIAL] &&
!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                                                                    GridView_Potential.Columns[1].Visible = false;

                                                            }
        }

        protected void fillLeadGrid(ArrayList leadList)
        {
                                                Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

                                                if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_LEAD]||
                                                    accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                                                {

                                                    DataTable dt = getLeadGridDataTable(leadList);
                                                    dt.DefaultView.Sort = "Due Date Ticks" + " " + "DESC";
                                                    Session[SessionFactory.ALL_SALE_ALL_LEAD_GRID_DATA] = dt.DefaultView.ToTable();
                                                    disableOnPageChange("lead");
                                                    GridView_Lead.SelectedIndex = -1;
                                                    GridView_Lead.DataSource = dt;
                                                    GridView_Lead.DataBind();
                                                    GridView_Lead.Visible = true;

                                                    if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_LEAD] &&
           !accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                                                        GridView_Lead.Columns[1].Visible = false;

                                                }
        }

        protected void Button_Filter_All_Lead_Click(object sender, EventArgs e)
        {
                                                            Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

                                                            if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_LEAD]||
                                                                 accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                                                            {
                                                                String prodCatId = DropDownList_Category_Lead.SelectedValue;
                                                                String activeStat = DropDownList_Lead_Active_Stat.SelectedValue;
                                                                String fromDate = TextBox_From_Date_Lead.Text;
                                                                String toDate = TextBox_To_Date_Lead.Text;
                                                                String custId=DropDownList_Contact_Lead.SelectedValue;
                                                                String rfqNo = TextBox_RFQ_No_Lead.Text;
                                                                String assgnTo = DropDownList_Assigned_To_Lead.SelectedValue;

                                                                ActionLibrary.SalesActions._dispLeads dLead = new ActionLibrary.SalesActions._dispLeads();

                                                                Dictionary<String, String> filterParams = new Dictionary<string, string>();
                                                                if (!rfqNo.Trim().Equals(""))
                                                                    filterParams.Add(dLead.FILTER_BY_RFQ_NO, rfqNo);
                                                                if (prodCatId != null && !prodCatId.Equals("") && !prodCatId.Equals("_"))
                                                                    filterParams.Add(dLead.FILTER_BY_PROD_CAT, prodCatId);
                                                                if (activeStat != null && !activeStat.Equals("") && !activeStat.Equals("_"))
                                                                    filterParams.Add(dLead.FILTER_BY_ACTIVE_STATUS, activeStat);
                                                                if (fromDate != null && !fromDate.Equals(""))
                                                                    filterParams.Add(dLead.FILTER_BY_DUE_DATE_FROM, fromDate);
                                                                if (toDate != null && !toDate.Equals(""))
                                                                    filterParams.Add(dLead.FILTER_BY_DUE_DATE_TO, toDate);
                                                                if (custId != null && !custId.Equals("_"))
                                                                    filterParams.Add(dLead.FILTER_BY_CUST_ID, DropDownList_Contact_Lead.SelectedValue);
                                                                if (assgnTo != null && !assgnTo.Equals("_"))
                                                                    filterParams.Add(dLead.FILTER_BY_ASSGND_TO, assgnTo);

                                                                fillLeadGrid(dLead.getAllLeadDetailsFiltered(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), User.Identity.Name, filterParams));
                                                                Button_Create_Req0.Focus();
                                                            }
        }

        protected void GridView_Lead_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_Lead.PageIndex = e.NewPageIndex;
            GridView_Lead.DataSource = (DataTable)Session[SessionFactory.ALL_SALE_ALL_LEAD_GRID_DATA];
            GridView_Lead.DataBind();
            GridView_Lead.SelectedIndex = -1;

            disableOnPageChange("lead");
        }

        protected void GridView_Lead_SelectedIndexChanged(object sender, EventArgs e)
        {            
            enableOnSelect("lead","");
            String selectedLeadId = ((Label)GridView_Lead.SelectedRow.Cells[2].FindControl("Label_RFQId")).Text;
            String creationMode = ((Label)GridView_Lead.SelectedRow.Cells[0].FindControl("Label_Mode")).Text;
            ((RadioButton)GridView_Lead.SelectedRow.Cells[0].FindControl("lead_radio")).Checked = true;
            
            if (BackEndObjects.RFQDetails.CREATION_MODE_MANUAL.Equals(creationMode))
            {
                Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];
                if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS]||
                    accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_CONVERT_LEAD])
                Button_Convert_Lead.Enabled = true;

                if (Button_Create_Req0.Enabled)//Replicate the access rule applied on the create lead button
                {
                    Button_Create_Clone_Lead.Enabled = true;
                    Label_Lead_Grid_Access.Visible = false;
                }
                else
                {
                    Label_Lead_Grid_Access.Visible = true;
                    Label_Lead_Grid_Access.Text = "You do not have access to create/clone Lead";
                }
            }
            else
            {
                Button_Convert_Lead.Enabled = false;
                Button_Create_Clone_Lead.Enabled = false;
                Label_Lead_Grid_Access.Visible = true;
                Label_Lead_Grid_Access.Text = "Can not clone Lead which is not created by your organization";
            }
            
            BackEndObjects.RFQDetails rfqObj = BackEndObjects.RFQDetails.getRFQDetailsbyIdDB(selectedLeadId);

            Session[SessionFactory.ALL_SALE_ALL_LEAD_SELECTED_LEAD_ID] = rfqObj.getRFQId();
            Session[SessionFactory.ALL_SALE_ALL_LEAD_SELECTED_CUSTOMER_OBJ] = ActionLibrary.customerDetails.getContactDetails(rfqObj.getEntityId(), Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            Session[SessionFactory.ALL_SALE_ALL_LEAD_LOCATION] = rfqObj.getLocalityId();
            Session[SessionFactory.ALL_SALE_ALL_LEAD_LD_SPEC] = RFQProductServiceDetails.getAllProductServiceDetailsbyRFQIdDB(selectedLeadId);
            Session[SessionFactory.ALL_SALE_ALL_LEAD_LD_PROD_SRV] = RFQProdServQnty.getRFQProductServiceQuantityforRFIdDB(selectedLeadId);
            
        }

        protected void Button_Filter_All_Pot_Click(object sender, EventArgs e)
        {
                                                                        Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

                                                                        if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_POTENTIAL]||
                                                                             accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                                                                        {
                                                                            String prodCatId = !DropDownList_Category_Pot.SelectedValue.Equals("_") ? DropDownList_Category_Pot.SelectedValue : "";
                                                                            String activeStat = !DropDownList_Pot_Active_Stat.SelectedValue.Equals("_") ? DropDownList_Pot_Active_Stat.SelectedValue : "";
                                                                            String potStage = !DropDownList_Pot_Stage_Stat.SelectedValue.Equals("_") ? DropDownList_Pot_Stage_Stat.SelectedValue : "";
                                                                            String fromDateDue = TextBox_From_Date_Due.Text;
                                                                            String toDateDue = TextBox_To_Date_Due.Text;
                                                                            String fromDateCreate = TextBox_From_Date_Create.Text;
                                                                            String toDateCreate = TextBox_To_Date_Create.Text;
                                                                            String custId = DropDownList_Contact_Potn.SelectedValue;
                                                                            String rfqNo = TextBox_RFQ_No_Potn.Text;
                                                                            String assgnTo = DropDownList_Assigned_To_Potn.SelectedValue;

                                                                            Label_Pot_Edit_Tooltip.Visible = false;

                                                                            ActionLibrary.SalesActions._dispPotentials dPot = new ActionLibrary.SalesActions._dispPotentials();

                                                                            Dictionary<String, String> filterParams = new Dictionary<string, string>();
                                                                            if(!rfqNo.Trim().Equals(""))
                                                                                filterParams.Add(dPot.FILTER_BY_RFQ_NO, rfqNo);
                                                                            if (prodCatId != null && !prodCatId.Equals(""))
                                                                                filterParams.Add(dPot.FILTER_BY_PROD_CAT, prodCatId);
                                                                            if (activeStat != null && !activeStat.Equals("") && !activeStat.Equals("_"))
                                                                                filterParams.Add(dPot.FILTER_BY_ACTIVE_STATUS, activeStat);
                                                                            if (potStage != null && !potStage.Equals(""))
                                                                                filterParams.Add(dPot.FILTER_BY_STAGE, potStage);
                                                                            if (fromDateDue != null && !fromDateDue.Equals(""))
                                                                                filterParams.Add(dPot.FILTER_BY_DUE_DATE_FROM, fromDateDue);
                                                                            if (toDateDue != null && !toDateDue.Equals(""))
                                                                                filterParams.Add(dPot.FILTER_BY_DUE_DATE_TO, toDateDue);
                                                                            if (fromDateCreate != null && !fromDateCreate.Equals(""))
                                                                                filterParams.Add(dPot.FILTER_BY_CREATE_DATE_FROM, fromDateCreate);
                                                                            if (toDateCreate != null && !toDateCreate.Equals(""))
                                                                                filterParams.Add(dPot.FILTER_BY_CREATE_DATE_TO, toDateCreate);
                                                                            if (custId != null && !custId.Equals("_"))
                                                                                filterParams.Add(dPot.FILTER_BY_CUST_ID, DropDownList_Contact_Potn.SelectedValue);
                                                                            if (assgnTo != null && !assgnTo.Equals("_"))
                                                                                filterParams.Add(dPot.FILTER_BY_ASSGND_TO, DropDownList_Assigned_To_Potn.SelectedValue);

                                                                            fillPotnGrid(dPot.getAllPotentialsFiltered(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), User.Identity.Name, filterParams));
                                                                            Button_Create_Pot.Focus();
                                                                        }
        }

        protected void GridView_Potential_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_Potential.PageIndex = e.NewPageIndex;
            GridView_Potential.SelectedIndex = -1;
            GridView_Potential.DataSource = (DataTable)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_GRID_DATA];
            GridView_Potential.DataBind();
            GridView_Potential.Columns[2].Visible = false;
            Label_Pot_Edit_Tooltip.Visible = false;

            disableOnPageChange("potn");
        }

        protected void GridView_Potential_SelectedIndexChanged(object sender, EventArgs e)
        {
            String selectedRFQId = ((Label)GridView_Potential.SelectedRow.Cells[3].FindControl("Label_RFQId")).Text;
            Label_Pot_Edit_Tooltip.Visible = false;

            
            //Disable the finalize deal button depending on the potential stage
            //and the creation mode
            String potStg = ((Label)GridView_Potential.SelectedRow.Cells[0].FindControl("Label_Pot_Stage")).Text;
            String createMode = ((Label)GridView_Potential.SelectedRow.Cells[0].FindControl("Label_Mode")).Text;
            String finalizeSent = ((Label)GridView_Potential.SelectedRow.Cells[0].FindControl("Label_Deal_Request")).Text;
            ((RadioButton)GridView_Potential.SelectedRow.Cells[0].FindControl("potn_radio")).Checked = true;
            enableOnSelect("potn",createMode);

            if (createMode.Equals(BackEndObjects.RFQShortlisted.POTENTIAL_CREATION_MODE_MANUAL))
                Button_Finalz_Deal.Enabled = false;
            else if (!potStg.Equals(BackEndObjects.RFQShortlisted.POTENTIAL_ACTIVE_STAT_CLOSED_LOST) &&
                !potStg.Equals(BackEndObjects.RFQShortlisted.POTENTIAL_ACTIVE_STAT_CLOSED_WON) &&
                createMode.Equals(BackEndObjects.RFQShortlisted.POTENTIAL_CREATION_MODE_AUTO) && 
                finalizeSent.Equals("N",StringComparison.InvariantCultureIgnoreCase))
                Button_Finalz_Deal.Enabled = true;
            else
                Button_Finalz_Deal.Enabled = false;

           
            String finalizedVendorId=BackEndObjects.RFQShortlisted.getRFQShortlistedEntryforFinalizedVendor(selectedRFQId).getRespEntityId();

            //If this entity has own the potential and also the potential is auto created
            //then enable the Invoice/Payment button            
            if (createMode.Equals(BackEndObjects.RFQShortlisted.POTENTIAL_CREATION_MODE_AUTO) &&
                Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString().Equals(finalizedVendorId))
            {
                Button_Inv_Pmnt.Enabled = true;
                Button_Create_Clone_Potn.Enabled = false;
                Label_Potential_Grid_Access.Visible = true;
                Label_Potential_Grid_Access.Text = "Can not clone potential which is not created by your organization";
            }
            //Or if the potential is manually created then enable this button all time
            else if (createMode.Equals(BackEndObjects.RFQShortlisted.POTENTIAL_CREATION_MODE_MANUAL))
            {
                Button_Inv_Pmnt.Enabled = true;

                if (Button_Create_Pot.Enabled)//Follow the access rule as applied on the create potential button
                {
                    Button_Create_Clone_Potn.Enabled = true;
                    Label_Potential_Grid_Access.Visible = false;
                }
                else
                {
                    Label_Potential_Grid_Access.Visible = true;
                    Label_Potential_Grid_Access.Text = "You do not have access to create/clone potential";
                }
            }
            else
            {
                Button_Inv_Pmnt.Enabled = false;
                Button_Create_Clone_Potn.Enabled = false;
                Label_Potential_Grid_Access.Visible = true;
                Label_Potential_Grid_Access.Text = "Can not clone potential which are not created by your organization";
            }

            BackEndObjects.RFQDetails rfqObj = BackEndObjects.RFQDetails.getRFQDetailsbyIdDB(selectedRFQId);

            Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_SELECTED_RFQ_ID] = rfqObj.getRFQId();
            Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_SELECTED_CUSTOMER_OBJ] = ActionLibrary.customerDetails.getContactDetails(rfqObj.getEntityId(), Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_LOCATION] = rfqObj.getLocalityId();
            Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_POTN_SPEC] = RFQProductServiceDetails.getAllProductServiceDetailsbyRFQIdDB(selectedRFQId);
            Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_POTN_PROD_SRV] = RFQProdServQnty.getRFQProductServiceQuantityforRFIdDB(selectedRFQId);
        }

        protected void GridView_Potential_RowEditing(object sender, GridViewEditEventArgs e)
        {
                                /*Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

                                if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_POTENTIAL]||
                                    accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                                {*/
                                    Label_Potential_Grid_Access.Visible = false;
                                    GridView_Potential.EditIndex = e.NewEditIndex;
                                    GridView_Potential.DataSource = (DataTable)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_GRID_DATA];
                                    GridView_Potential.DataBind();
                               /* }
                                else
                                {
                                    Label_Potential_Grid_Access.Visible = true;
                                    Label_Potential_Grid_Access.Text = "You dont have edit access to Potential records";
                                }*/
        }

        protected void GridView_Potential_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DataTable dt = (DataTable)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_GRID_DATA];
            GridViewRow gVR = GridView_Potential.Rows[e.RowIndex];

            int index = GridView_Potential.Rows[e.RowIndex].DataItemIndex;

            String potStage=((DropDownList)gVR.FindControl("DropDownList_Pot_Stage")).SelectedItem.Text;
            dt.Rows[index]["PotStage"] = potStage;
            
            
            String potId = ((Label)gVR.Cells[0].FindControl("Label_Hidden_Pot_Id")).Text;
            BackEndObjects.RFQShortlisted shortObj=BackEndObjects.RFQShortlisted.getAllRFQShortListedbyPotentialIdDB(potId);

            String potActiveStat="";
            String selectedContact = "";
            String rfqName = "";
            String dueDate = "";
            String assgnTo = "";
            String nextDate = "";
            String curr = "";

            if (shortObj.getCreateMode().Equals(BackEndObjects.RFQShortlisted.POTENTIAL_CREATION_MODE_MANUAL))
            {
                potActiveStat=((DropDownList)gVR.FindControl("DropDownList_Pot_Active")).SelectedItem.Text;
                dt.Rows[index]["ActiveStat"] = potActiveStat;
                dt.Rows[index]["CustId"] = ((DropDownList)gVR.FindControl("DropDownList_Contact")).SelectedItem.Value;
                dt.Rows[index]["CustName"] = ((DropDownList)gVR.FindControl("DropDownList_Contact")).SelectedItem.Text;
                dt.Rows[index]["RFQName"] = ((TextBox)gVR.Cells[0].FindControl("TextBox_RFQName_Edit")).Text;
                curr = ((DropDownList)gVR.FindControl("DropDownList_Curr")).SelectedItem.Text;
                dt.Rows[index]["curr"] = curr;

                dueDate = ((TextBox)gVR.FindControl("TextBox_DueDate")).Text;
                dt.Rows[index]["Due Date"] = dueDate.Replace("00", "").Replace(":", "");

                selectedContact = ((DropDownList)gVR.FindControl("DropDownList_Contact")).SelectedItem.Value;
                rfqName = ((TextBox)gVR.Cells[0].FindControl("TextBox_RFQName_Edit")).Text;
            }

            dt.Rows[index]["Assgn To"] = ((DropDownList)gVR.FindControl("DropDownList_Potn_Assgn_To_Edit")).SelectedItem.Value;
            assgnTo = dt.Rows[index]["Assgn To"].ToString();
            dt.Rows[index]["Next Date"] = ((TextBox)gVR.FindControl("TextBox_FwpDate")).Text;
            nextDate = dt.Rows[index]["Next Date"].ToString(); 


            GridView_Potential.EditIndex = -1;
            GridView_Potential.DataSource = dt;
            GridView_Potential.DataBind();

            Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_GRID_DATA] = dt;

            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            whereCls.Add(BackEndObjects.RFQShortlisted.RFQ_SHORTLIST_COL_POTENTIAL_ID, potId);

            Dictionary<String, String> targetVals = new Dictionary<string, string>();
            targetVals.Add(BackEndObjects.RFQShortlisted.RFQ_SHORTLIST_COL_POTENTIAL_STAT, potStage);
            if(!potActiveStat.Equals(""))
            targetVals.Add(BackEndObjects.RFQShortlisted.RFQ_SHORTLIST_COL_POTENTIAL_ACTIVE_STAT, potActiveStat);


            try
            {
                RFQResponse respObj = RFQResponse.getRFQResponseWOQuotesForRFQIdandResponseEntityIdDB(shortObj.getRFQId(), Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                if (respObj != null && respObj.getRespEntityId() != null && !respObj.getRespEntityId().Equals("")) //Response object exists
                {
                    Dictionary<String, String> whereClsResp = new Dictionary<string, string>();
                    whereClsResp.Add(RFQResponse.RFQ_RES_COL_RFQ_ID, shortObj.getRFQId());
                    whereClsResp.Add(RFQResponse.RFQ_RES_COL_RESP_ENT_ID, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

                    Dictionary<String, String> targetValsResp = new Dictionary<string, string>();
                    targetValsResp.Add(RFQResponse.RFQ_RES_COL_NXT_FWUP_DATE, nextDate);
                    targetValsResp.Add(RFQResponse.RFQ_RES_COL_ASSGN_TO, assgnTo);

                    RFQResponse.updateRFQResponseDB(targetValsResp, whereClsResp, DBConn.Connections.OPERATION_UPDATE);
                }
                else
                {
                    respObj = new RFQResponse();
                    respObj.setAssignedTo(assgnTo);
                    respObj.setNextFollowupDate(nextDate);
                    respObj.setRespEntityId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                    respObj.setRFQId(shortObj.getRFQId());
                    respObj.setRespDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    RFQResponse.insertRFQResponseDB(respObj);
                }

                BackEndObjects.RFQShortlisted.updateRFQShortListedEntryDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
                if (!selectedContact.Equals("") || !rfqName.Equals(""))
                {
                    whereCls.Clear(); targetVals.Clear();
                    
                    whereCls.Add(BackEndObjects.RFQDetails.RFQ_COL_RFQ_ID, ((Label)gVR.Cells[0].FindControl("Label_RFQId")).Text);
                    if(!selectedContact.Equals(""))
                    targetVals.Add(BackEndObjects.RFQDetails.RFQ_COL_ENTITY_ID, selectedContact);
                    if(!rfqName.Equals(""))
                        targetVals.Add(BackEndObjects.RFQDetails.RFQ_COL_RFQ_NAME, rfqName);
                    if (!dueDate.Equals(""))
                        targetVals.Add(BackEndObjects.RFQDetails.RFQ_COL_DUE_DATE, dueDate);
                    if(!curr.Equals(""))
                        targetVals.Add(BackEndObjects.RFQDetails.RFQ_COL_CURRENCY, ((DropDownList)gVR.FindControl("DropDownList_Curr")).SelectedValue);

                    BackEndObjects.RFQDetails.updateRFQDetailsDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void GridView_Potential_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                GridViewRow gVR = e.Row;

                //DropDownList_Pot_Stage
                //DropDownList_Pot_Active
                ListItem lt = new ListItem();
                lt.Text = BackEndObjects.PotentialStatus.POTENTIAL_STAT_PRELIM;
                lt.Value = BackEndObjects.PotentialStatus.POTENTIAL_STAT_PRELIM;
                ((DropDownList)gVR.FindControl("DropDownList_Pot_Stage")).Items.Add(lt);

                lt = new ListItem();
                lt.Text = BackEndObjects.PotentialStatus.POTENTIAL_STAT_MEDIUM;
                lt.Value = BackEndObjects.PotentialStatus.POTENTIAL_STAT_MEDIUM;
                ((DropDownList)gVR.FindControl("DropDownList_Pot_Stage")).Items.Add(lt);

                lt = new ListItem();
                lt.Text = BackEndObjects.PotentialStatus.POTENTIAL_STAT_ADVNCD;
                lt.Value = BackEndObjects.PotentialStatus.POTENTIAL_STAT_ADVNCD;
                ((DropDownList)gVR.FindControl("DropDownList_Pot_Stage")).Items.Add(lt);
                
                
                //Only if the Potential creation mode is manual then the user can view this drop down and edit it.
                String potId = ((Label)gVR.Cells[0].FindControl("Label_Hidden_Pot_Id")).Text;
                //BackEndObjects.MainBusinessEntity.getUserDetailsforMainEntitybyIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                Dictionary<String, userDetails> allUserDetails = (Dictionary<String, userDetails>)Cache[Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()];
                if (allUserDetails == null)
                {
                    allUserDetails = BackEndObjects.MainBusinessEntity.getUserDetailsforMainEntitybyIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                    Cache.Insert(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), allUserDetails);
                }

                DropDownList DropDownList_Assgn_To = (DropDownList)gVR.Cells[0].FindControl("DropDownList_Potn_Assgn_To_Edit");
                foreach (KeyValuePair<String, userDetails> kvp in allUserDetails)
                {
                    BackEndObjects.userDetails userDetObj = kvp.Value;

                    ListItem lt1 = new ListItem();
                    lt1.Text = userDetObj.getUserId();
                    lt1.Value = userDetObj.getUserId();

                    DropDownList_Assgn_To.Items.Add(lt1);
                }

                ListItem emptyUser = new ListItem();
                emptyUser.Text = "";
                emptyUser.Value = "";
                DropDownList_Assgn_To.Items.Add(emptyUser);

                    DropDownList_Assgn_To.SelectedValue = ((Label)gVR.Cells[0].FindControl("Label_Assgn_To_Edit")).Text;


                if (BackEndObjects.RFQShortlisted.getAllRFQShortListedbyPotentialIdDB(potId).getCreateMode().
                    Equals(BackEndObjects.RFQShortlisted.POTENTIAL_CREATION_MODE_MANUAL))
                {

                    /*ArrayList contactObjList = Contacts.
getAllContactsbyEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());*/

                    DropDownList DropDownList_Contacts = ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Contact"));
                    ((TextBox)gVR.Cells[0].FindControl("TextBox_RFQName_Edit")).Enabled = true;

                    DropDownList_Contacts.Visible = true;
                    String selectedContactVal = ((Label)gVR.Cells[0].FindControl("Label_Contact")).Text;

                    Dictionary<String, String> existingContactDict = (Dictionary<String, String>)Session[SessionFactory.EXISTING_CONTACT_DICTIONARY];
                    if(existingContactDict!=null)
                    foreach (KeyValuePair<String, String> kvp in existingContactDict)
                    {
                        String contactName = kvp.Key;
                        String contactEntId = kvp.Value;

                        ListItem ltItem = new ListItem();
                        ltItem.Text = contactName;
                        ltItem.Value = contactEntId;
                        DropDownList_Contacts.Items.Add(ltItem);

                    }

                    DropDownList_Contacts.SelectedValue = selectedContactVal;


                    lt = new ListItem();
                    lt.Text = BackEndObjects.RFQShortlisted.POTENTIAL_ACTIVE_STAT_ACTIVE;
                    lt.Value = BackEndObjects.RFQShortlisted.POTENTIAL_ACTIVE_STAT_ACTIVE;
                    ((DropDownList)gVR.FindControl("DropDownList_Pot_Active")).Items.Add(lt);

                    lt = new ListItem();
                    lt.Text = BackEndObjects.RFQShortlisted.POTENTIAL_ACTIVE_STAT_NOT_OPEN;
                    lt.Value = BackEndObjects.RFQShortlisted.POTENTIAL_ACTIVE_STAT_NOT_OPEN;
                    ((DropDownList)gVR.FindControl("DropDownList_Pot_Active")).Items.Add(lt);

                    lt = new ListItem();
                    lt.Text = BackEndObjects.RFQShortlisted.POTENTIAL_ACTIVE_STAT_CLOSED_LOST;
                    lt.Value = BackEndObjects.RFQShortlisted.POTENTIAL_ACTIVE_STAT_CLOSED_LOST;
                    ((DropDownList)gVR.FindControl("DropDownList_Pot_Active")).Items.Add(lt);

                    lt = new ListItem();
                    lt.Text = BackEndObjects.RFQShortlisted.POTENTIAL_ACTIVE_STAT_CLOSED_WON;
                    lt.Value = BackEndObjects.RFQShortlisted.POTENTIAL_ACTIVE_STAT_CLOSED_WON;
                    ((DropDownList)gVR.FindControl("DropDownList_Pot_Active")).Items.Add(lt);
                    
                    ((DropDownList)gVR.FindControl("DropDownList_Pot_Active")).SelectedIndex = -1;
                    //Now add the option for Closed!Won and Closed!Lost for potential stage for manually created potential
                    lt = new ListItem();
                    lt.Text = BackEndObjects.RFQShortlisted.POTENTIAL_ACTIVE_STAT_CLOSED_WON;
                    lt.Value = BackEndObjects.RFQShortlisted.POTENTIAL_ACTIVE_STAT_CLOSED_WON;
                    ((DropDownList)gVR.FindControl("DropDownList_Pot_Stage")).Items.Add(lt);

                    lt = new ListItem();
                    lt.Text = BackEndObjects.RFQShortlisted.POTENTIAL_ACTIVE_STAT_CLOSED_LOST;
                    lt.Value = BackEndObjects.RFQShortlisted.POTENTIAL_ACTIVE_STAT_CLOSED_LOST;
                    ((DropDownList)gVR.FindControl("DropDownList_Pot_Stage")).Items.Add(lt);

                    //((DropDownList)gVR.FindControl("DropDownList_Pot_Stage")).SelectedIndex = -1;
                    ((DropDownList)gVR.FindControl("DropDownList_Pot_Stage")).SelectedValue = ((Label)gVR.FindControl("Label_Pot_Stage_Edit")).Text;
                    ((Label)gVR.FindControl("Label_Due_Date_Edit")).Visible = false;

                    loadCurrency(((DropDownList)gVR.FindControl("DropDownList_Curr")), ((Label)gVR.FindControl("Label_Curr_Edit")).Text);
                }
                else
                {
                    ((DropDownList)gVR.FindControl("DropDownList_Pot_Active")).Visible = false;
                    ((TextBox)gVR.FindControl("TextBox_DueDate")).Visible = false;
                    ((Label)gVR.FindControl("Label_Act_Stat_Edit")).Visible = true;
                    ((LinkButton)gVR.FindControl("LinkButton_Customer_Edit")).Visible = true;
                    ((DropDownList)gVR.FindControl("DropDownList_Curr")).Visible = false;
                    ((Label)gVR.FindControl("Label_Curr_Edit")).Visible = true;

                    BackEndObjects.RFQShortlisted potObj = RFQShortlisted.getRFQShortlistedEntryforFinalizedVendor(((Label)gVR.FindControl("Label_RFQId")).Text);

                    if (potObj.getPotentialId() != null && !potObj.getPotentialId().Equals(""))
                        {
                            //Deal Closed - no editing allowed for the potential stage for a Auto - created potential
                            ((DropDownList)gVR.FindControl("DropDownList_Pot_Stage")).Visible = false;
                            ((Label)gVR.FindControl("Label_Pot_Stage_Edit")).Visible = true;
                            
                        ((TextBox)gVR.FindControl("TextBox_FwpDate")).Visible = false;
                            ((Label)gVR.FindControl("Label_Fwp_Date_Edit")).Visible = true;

                            ((DropDownList)gVR.FindControl("DropDownList_Potn_Assgn_To_Edit")).Visible = false;
                            ((Label)gVR.FindControl("Label_Assgn_To_Edit")).Visible = true;

                            Label_Pot_Edit_Tooltip.Visible = true;
                            Label_Pot_Edit_Tooltip.Text = "For an Auto-created potential, potential details can not be modified after the deal is closed";

                        }
                    
                    
                }
            }
        }

        protected void GridView_Potential_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView_Potential.EditIndex = -1;
            GridView_Potential.DataSource = (DataTable)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_GRID_DATA];
            GridView_Potential.DataBind();

        }

        protected void GridView_Lead_RowEditing(object sender, GridViewEditEventArgs e)
        {
                                                            /*Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

                                                            if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_LEAD]||
                                                                accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                                                            {*/
                                                                Label_Lead_Grid_Access.Visible = false;
                                                                GridView_Lead.EditIndex = e.NewEditIndex;
                                                                GridView_Lead.DataSource = (DataTable)Session[SessionFactory.ALL_SALE_ALL_LEAD_GRID_DATA];
                                                                GridView_Lead.DataBind();
                                                          /*  }
                                                            else
                                                            {
                                                                Label_Lead_Grid_Access.Visible = true;
                                                                Label_Lead_Grid_Access.Text = "You dont have edit access to Lead records";
                                                            }*/
        }

        protected void GridView_Lead_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DataTable dt = (DataTable)Session[SessionFactory.ALL_SALE_ALL_LEAD_GRID_DATA];
            GridViewRow gVR = GridView_Lead.Rows[e.RowIndex];

            String activeStat = "";
            String dueDate = "";
            String rfqName = "";
            String selectedContact = "";
            String rfqId=((Label)gVR.Cells[0].FindControl("Label_RFQId")).Text;
            String assgnTo = "";
            String nextDate = "";
            String curr = "";

            RFQDetails rfqObj = BackEndObjects.RFQDetails.getRFQDetailsbyIdDB(rfqId);

            int index = GridView_Lead.Rows[e.RowIndex].DataItemIndex;

            if (rfqObj.getCreateMode().Equals(BackEndObjects.RFQDetails.CREATION_MODE_MANUAL))
            {
                dt.Rows[index]["CustId"] = ((DropDownList)gVR.FindControl("DropDownList_Contact")).SelectedItem.Value;
                dt.Rows[index]["CustName"] = ((DropDownList)gVR.FindControl("DropDownList_Contact")).SelectedItem.Text;
                selectedContact = ((DropDownList)gVR.FindControl("DropDownList_Contact")).SelectedItem.Value;

                dt.Rows[index]["ActiveStat"] = ((DropDownList)gVR.FindControl("DropDownList_Lead_Active")).SelectedItem.Text;
                activeStat = ((DropDownList)gVR.FindControl("DropDownList_Lead_Active")).SelectedItem.Text;

                dueDate = ((TextBox)gVR.FindControl("TextBox_DueDate")).Text;
                dt.Rows[index]["Due Date"] = dueDate.Replace("00", "").Replace(":", "");

                rfqName = ((TextBox)gVR.FindControl("TextBox_RFQName")).Text;
                dt.Rows[index]["RFQName"] = rfqName;

                curr = ((DropDownList)gVR.FindControl("DropDownList_Curr")).SelectedItem.Text;
                dt.Rows[index]["curr"] = curr;
            }

            dt.Rows[index]["Assgn To"] = ((DropDownList)gVR.FindControl("DropDownList_Lead_Assgn_To_Edit")).SelectedItem.Value;
            assgnTo = dt.Rows[index]["Assgn To"].ToString();
            dt.Rows[index]["Next Date"] = ((TextBox)gVR.FindControl("TextBox_FwpDate")).Text;
            nextDate = dt.Rows[index]["Next Date"].ToString();

            GridView_Lead.EditIndex = -1;
            GridView_Lead.DataSource = dt;
            GridView_Lead.DataBind();
            
            Session[SessionFactory.ALL_SALE_ALL_LEAD_GRID_DATA] = dt;

            try
            {
                RFQResponse respObj = RFQResponse.getRFQResponseWOQuotesForRFQIdandResponseEntityIdDB(rfqId, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                if (respObj != null && respObj.getRespEntityId() != null && !respObj.getRespEntityId().Equals("")) //Response object exists
                {
                    Dictionary<String, String> whereCls = new Dictionary<string, string>();
                    whereCls.Add(RFQResponse.RFQ_RES_COL_RFQ_ID, rfqId);
                    whereCls.Add(RFQResponse.RFQ_RES_COL_RESP_ENT_ID, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

                    Dictionary<String, String> targetVals = new Dictionary<string, string>();
                    targetVals.Add(RFQResponse.RFQ_RES_COL_NXT_FWUP_DATE, nextDate);
                    targetVals.Add(RFQResponse.RFQ_RES_COL_ASSGN_TO, assgnTo);

                    RFQResponse.updateRFQResponseDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
                }
                else
                {
                    respObj = new RFQResponse();
                    respObj.setAssignedTo(assgnTo);
                    respObj.setNextFollowupDate(nextDate);
                    respObj.setRespEntityId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                    respObj.setRFQId(rfqId);
                    respObj.setRespDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    RFQResponse.insertRFQResponseDB(respObj);
                }
                
                if (rfqObj.getCreateMode().Equals(BackEndObjects.RFQDetails.CREATION_MODE_MANUAL))
                {
                    Dictionary<String, String> whereCls = new Dictionary<string, string>();
                    whereCls.Add(BackEndObjects.RFQDetails.RFQ_COL_RFQ_ID, rfqId);

                    Dictionary<String, String> targetVals = new Dictionary<string, string>();
                    targetVals.Add(BackEndObjects.RFQDetails.RFQ_COL_RFQ_NAME, rfqName);
                    targetVals.Add(BackEndObjects.RFQDetails.RFQ_COL_DUE_DATE, dueDate);
                    targetVals.Add(BackEndObjects.RFQDetails.RFQ_COL_ACTIVE_STAT, activeStat);
                    targetVals.Add(BackEndObjects.RFQDetails.RFQ_COL_ENTITY_ID, selectedContact);
                    if (!curr.Equals(""))
                        targetVals.Add(BackEndObjects.RFQDetails.RFQ_COL_CURRENCY, ((DropDownList)gVR.FindControl("DropDownList_Curr")).SelectedValue);
                    
                    BackEndObjects.RFQDetails.updateRFQDetailsDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void loadCurrency(DropDownList DropDownList_Curr, String selectedVal)
        {
            Dictionary<String, Currency> allCurrList = (Dictionary<String, Currency>)Session[SessionFactory.CURRENCY_LIST];
            String defaultCurr = Session[SessionFactory.MAIN_BUSINESS_ENTITY_DEFAULT_CURRENCY].ToString();

            foreach (KeyValuePair<String, Currency> kvp in allCurrList)
            {
                ListItem lt = new ListItem();
                lt.Text = kvp.Value.getCurrencyName();
                lt.Value = kvp.Key;

                DropDownList_Curr.Items.Add(lt);

                if (selectedVal != null && !selectedVal.Equals("") && selectedVal.Equals(lt.Text)) //Currency name sent instead of id 
                    DropDownList_Curr.SelectedValue = lt.Value;
            }

            if (selectedVal == null || selectedVal.Equals(""))
                DropDownList_Curr.SelectedValue = defaultCurr;
        }

        protected void GridView_Lead_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                GridViewRow gVR = e.Row;

                ListItem lt = new ListItem();
                
                String rfqId=((Label)gVR.Cells[0].FindControl("Label_RFQId")).Text;

                RFQDetails rfqObj = BackEndObjects.RFQDetails.getRFQDetailsbyIdDB(rfqId);

                //BackEndObjects.MainBusinessEntity.getUserDetailsforMainEntitybyIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                Dictionary<String, userDetails> allUserDetails = (Dictionary<String, userDetails>)Cache[Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()];
                if (allUserDetails == null)
                {
                   allUserDetails = BackEndObjects.MainBusinessEntity.getUserDetailsforMainEntitybyIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                   Cache.Insert(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), allUserDetails);
                }

                DropDownList DropDownList_Assgn_To = (DropDownList)gVR.Cells[0].FindControl("DropDownList_Lead_Assgn_To_Edit");
                
                foreach (KeyValuePair<String, userDetails> kvp in allUserDetails)
                {
                    BackEndObjects.userDetails userDetObj = kvp.Value;

                    ListItem lt1 = new ListItem();
                    lt1.Text = userDetObj.getUserId();
                    lt1.Value = userDetObj.getUserId();

                    DropDownList_Assgn_To.Items.Add(lt1);
                }
                ListItem emptyUser = new ListItem();
                emptyUser.Text = "";
                emptyUser.Value = "";
                DropDownList_Assgn_To.Items.Add(emptyUser);
                                
                    DropDownList_Assgn_To.SelectedValue = ((Label)gVR.Cells[0].FindControl("Label_Assgn_To_Edit")).Text;



                if (rfqObj.getCreateMode().Equals(BackEndObjects.RFQDetails.CREATION_MODE_MANUAL))
                {
                    
                    /*ArrayList contactObjList = Contacts.
                        getAllContactsbyEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());*/
                    Dictionary<String, String> existingContactDict = (Dictionary<String, String>)Session[SessionFactory.EXISTING_CONTACT_DICTIONARY];

                    DropDownList DropDownList_Contacts=((DropDownList)gVR.Cells[0].FindControl("DropDownList_Contact"));
                    DropDownList_Contacts.Visible = true;
                    String selectedContactVal = ((Label)gVR.Cells[0].FindControl("Label_Contact")).Text;
                    if(existingContactDict!=null)
                    foreach (KeyValuePair<String, String> kvp in existingContactDict)
                    {
                        String contactName = kvp.Key;
                        String contactEntId = kvp.Value;

                        ListItem ltItem = new ListItem();
                        ltItem.Text = contactName;
                        ltItem.Value = contactEntId;
                        DropDownList_Contacts.Items.Add(ltItem);

                    }
                    DropDownList_Contacts.SelectedValue = selectedContactVal;


                    lt = new ListItem();
                    lt.Text = BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_ACTIVE;
                    lt.Value = BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_ACTIVE;
                    ((DropDownList)gVR.FindControl("DropDownList_Lead_Active")).Items.Add(lt);

                    lt = new ListItem();
                    lt.Text = BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_NONE;
                    lt.Value = BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_NONE;
                    ((DropDownList)gVR.FindControl("DropDownList_Lead_Active")).Items.Add(lt);

                    lt = new ListItem();
                    lt.Text = BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_NOT_OPEN;
                    lt.Value = BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_NOT_OPEN;
                    ((DropDownList)gVR.FindControl("DropDownList_Lead_Active")).Items.Add(lt);

                    ((DropDownList)gVR.FindControl("DropDownList_Lead_Active")).SelectedIndex = -1;

                    ((Label)gVR.FindControl("Label_RFQName_Edit")).Visible = false;
                    ((Label)gVR.FindControl("Label_Due_Date_Edit")).Visible = false;

                    loadCurrency(((DropDownList)gVR.FindControl("DropDownList_Curr")), ((Label)gVR.FindControl("Label_Curr_Edit")).Text);
                }
                else
                {
                    ((DropDownList)gVR.FindControl("DropDownList_Lead_Active")).Visible = false;
                    ((Label)gVR.FindControl("Label_Act_Stat_Edit")).Visible = true;
                    ((TextBox)gVR.FindControl("TextBox_DueDate")).Visible = false;
                    ((TextBox)gVR.FindControl("TextBox_RFQName")).Visible = false;
                    ((LinkButton)gVR.FindControl("LinkButton_Customer_Edit")).Visible = true;
                    ((DropDownList)gVR.FindControl("DropDownList_Curr")).Visible = false;
                    ((Label)gVR.FindControl("Label_Curr_Edit")).Visible = true;
                    //((Label)gVR.FindControl("Label_Act_Stat_Edit")).Text = ((Label)gVR.FindControl("Label_Pot_Stage")).Text;
                }
            }
        }

        protected void GridView_Lead_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView_Lead.EditIndex = -1;
            GridView_Lead.DataSource = Session[SessionFactory.ALL_SALE_ALL_LEAD_GRID_DATA];
            GridView_Lead.DataBind();            
            
        }

        protected void Button_Finalz_Deal_Click(object sender, EventArgs e)
        {
            String rfqId = Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_SELECTED_RFQ_ID].ToString();
            String respEntId = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();

                            Dictionary<String, String> whereCls = new Dictionary<string, string>();
                whereCls.Add(BackEndObjects.RFQShortlisted.RFQ_SHORTLIST_COL_RFQ_ID, rfqId);
                whereCls.Add(BackEndObjects.RFQShortlisted.RFQ_SHORTLIST_COL_RESP_ENT_ID, respEntId);

                Dictionary<String, String> targetVals = new Dictionary<string, string>();
                targetVals.Add(BackEndObjects.RFQShortlisted.RFQ_SHORTLIST_COL_FINL_BY_SUPL, "Y");

                try
                {
                    BackEndObjects.RFQShortlisted.updateRFQShortListedEntryDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
                    Label_Finalize_Stat.Visible = true;
                    Label_Finalize_Stat.ForeColor = System.Drawing.Color.Green;
                    Label_Finalize_Stat.Text = "Deal Finalization Request Sent";

                    Button_Finalz_Deal.Enabled = false;

                    DataTable dt = (DataTable)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_GRID_DATA];
                    dt.Rows[GridView_Potential.SelectedRow.DataItemIndex]["DealRequest"] = "Y";
                    dt.DefaultView.Sort = "DateCreatedTicks" + " " + "DESC";

                    GridView_Potential.DataSource = dt;
                    GridView_Potential.DataBind();
                    Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_GRID_DATA] = dt.DefaultView.ToTable();


                }
                catch (Exception ex)
                {
                    Label_Finalize_Stat.Visible = true;
                    Label_Finalize_Stat.ForeColor = System.Drawing.Color.Red;
                    Label_Finalize_Stat.Text = "Deal Finalization Request Failed";
                }
            }

        protected void LinkButton_COM_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_Potential.SelectedIndex)
                GridView_Potential.SelectRow(row.RowIndex);

            ((RadioButton)GridView_Potential.SelectedRow.Cells[0].FindControl("potn_radio")).Checked = true;

            String forwardString = "/Pages/Popups/Purchase/PO_Details.aspx";
            forwardString += "?rfId=" + ((Label)GridView_Potential.Rows[Convert.ToInt32(e.CommandArgument)].Cells[0].FindControl("Label_RFQId")).Text;
            forwardString += "&context=" + "vendor";
            forwardString += "&poId=" + ((LinkButton)GridView_Potential.Rows[Convert.ToInt32(e.CommandArgument)].Cells[0].FindControl("LinkButton_COM")).Text;
            forwardString += "&createMode=" + ((Label)GridView_Potential.Rows[Convert.ToInt32(e.CommandArgument)].Cells[0].FindControl("Label_Mode")).Text;

            if (((Label)GridView_Potential.SelectedRow.Cells[0].FindControl("Label_Mode")).Text.
Equals(BackEndObjects.RFQShortlisted.POTENTIAL_CREATION_MODE_MANUAL) && GridView_Potential.Columns[1].Visible)
                forwardString += "&allowEdit=" + "true";
            //Response.Write("<script type='text/javascript'>alert('" + "Hi" + "');</script>");
            //ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "alert('hi');", true);
            ScriptManager.RegisterStartupScript(this, typeof(string), "DispPOSales", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=900');", true);
        }

        protected void Button_Inv_Pmnt_Click(object sender, EventArgs e)
        {
            String potStage = ((Label)GridView_Potential.SelectedRow.Cells[0].FindControl("Label_Pot_Stage")).Text;
            //
            if (!potStage.Equals(BackEndObjects.RFQShortlisted.POTENTIAL_ACTIVE_STAT_CLOSED_WON))
            {
                String forwardString = "/Pages/Popups/Sale/Notification_Sales.aspx";
                forwardString += "?msg=" + "To create or view or update invoice details you must first win the deal";
                ScriptManager.RegisterStartupScript(this, typeof(string), "SalesNotif", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=900');", true);
            }
            else
            {
                String forwardString = "/Pages/Popups/Sale/Inv_Details.aspx";
                forwardString += "?rfId=" + ((Label)GridView_Potential.SelectedRow.Cells[0].FindControl("Label_RFQId")).Text;
                forwardString += "&context=" + "vendor";
                //forwardString += "&poId=" + ((LinkButton)GridView_Potential.SelectedRow.Cells[0].FindControl("LinkButton_COM")).Text;

                //BackEndObjects.Invoice invObj = BackEndObjects.Invoice.getInvoicebyRfIdDB(((Label)GridView_Potential.SelectedRow.Cells[0].FindControl("Label_RFQId")).Text);

                Dictionary<String, Invoice> invDict = BackEndObjects.Invoice.getAllInvoicesbyRfIdDB(((Label)GridView_Potential.SelectedRow.Cells[0].FindControl("Label_RFQId")).Text);

                if (!(invDict.Count > 1))
                {
                    forwardString += "&invId=";
                    String invId = "",relatedPO="";
                    foreach (KeyValuePair<String, Invoice> kvp in invDict)
                    {
                        invId= kvp.Value.getInvoiceId();
                        relatedPO=kvp.Value.getRelatedPO();
                    }

                    forwardString += invId;

                    if (!relatedPO.Equals(""))
                        forwardString += "&poId=" + relatedPO;
                    else
                        forwardString += "&poId=" + ((LinkButton)GridView_Potential.SelectedRow.Cells[0].FindControl("LinkButton_COM")).Text;
                }
                else
                {
                    forwardString = "/Pages/Popups/Sale/MultipleInvoiceForRFQ.aspx";
                    forwardString += "?rfId=" + ((Label)GridView_Potential.SelectedRow.Cells[0].FindControl("Label_RFQId")).Text;
                    Dictionary<String, Dictionary<String, Invoice>> invDictSession = (Dictionary<String,Dictionary<String,Invoice>>)Session[SessionFactory.ALL_SALE_ALL_POTN_MUTLTIPLE_INV_DATA_FOR_RFQ];
                    if (invDictSession == null)
                        invDictSession = new Dictionary<string, Dictionary<string, Invoice>>();
                    
                    if (!invDictSession.ContainsKey(((Label)GridView_Potential.SelectedRow.Cells[0].FindControl("Label_RFQId")).Text))
                        invDictSession.Add(((Label)GridView_Potential.SelectedRow.Cells[0].FindControl("Label_RFQId")).Text, invDict);
                    else
                    {
                        invDictSession.Remove(((Label)GridView_Potential.SelectedRow.Cells[0].FindControl("Label_RFQId")).Text);
                        invDictSession.Add(((Label)GridView_Potential.SelectedRow.Cells[0].FindControl("Label_RFQId")).Text, invDict);
                    }

                    Session[SessionFactory.ALL_SALE_ALL_POTN_MUTLTIPLE_INV_DATA_FOR_RFQ] = invDictSession;
                }
                //Response.Write("<script type='text/javascript'>alert('" + "Hi" + "');</script>");
                //ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "alert('hi');", true);
                ScriptManager.RegisterStartupScript(this, typeof(string), "DispInv", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=900,Height=900');", true);
            }
        }

        protected void Button_Filter_All_Inv_Click(object sender, EventArgs e)
        {
            fillInvGridwithFilter();
            Button_Create_Invoice_Manual.Focus();
        }

        protected void GridView_Invoice_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                GridViewRow gVR = e.Row;

                ListItem ltD1 = new ListItem();
                ltD1.Text = BackEndObjects.DeliveryStat.DELIV_STAT_DELIVERED;
                ltD1.Value = BackEndObjects.DeliveryStat.DELIV_STAT_DELIVERED;

                ListItem ltD2 = new ListItem();
                ltD2.Text = BackEndObjects.DeliveryStat.DELIV_STAT_UNDELIVERED;
                ltD2.Value = BackEndObjects.DeliveryStat.DELIV_STAT_UNDELIVERED;

                ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Deliv_Stat_Edit")).Items.Add(ltD1);
                ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Deliv_Stat_Edit")).Items.Add(ltD2);

                ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Deliv_Stat_Edit")).SelectedValue = ((Label)gVR.Cells[0].
                    FindControl("Label_Deliv_Stat_Edit")).Text;

                ListItem ltP1 = new ListItem();
                ltP1.Text = BackEndObjects.PaymentStat.PATMENT_STAT_COMPLETE;
                ltP1.Value = BackEndObjects.PaymentStat.PATMENT_STAT_COMPLETE;

                ListItem ltP2 = new ListItem();
                ltP2.Text = BackEndObjects.PaymentStat.PAYMENT_STAT_INCOMPLETE;
                ltP2.Text = BackEndObjects.PaymentStat.PAYMENT_STAT_INCOMPLETE;

                ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Pmnt_Edit")).Items.Add(ltP1);
                ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Pmnt_Edit")).Items.Add(ltP2);

                ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Pmnt_Edit")).SelectedValue = ((Label)gVR.Cells[0].
                    FindControl("Label_Pmnt_Stat_Edit")).Text;

                loadCurrency(((DropDownList)gVR.FindControl("DropDownList_Curr")), ((Label)gVR.FindControl("Label_Curr_Edit")).Text);

            }
            else if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Normal) == DataControlRowState.Normal)
            {
                GridViewRow gVR = e.Row;
                if (((Label)gVR.FindControl("Label_RFQId1")).Text.IndexOf("dummy") >= 0)
                    ((Label)gVR.FindControl("Label_RFQId1")).Visible = false;

                String uId = User.Identity.Name;
                String approverId = ((LinkButton)gVR.Cells[0].FindControl("LinkButton_Apprvl_Stat")).Text;
                Boolean allowedToEdit = uId.Equals(approverId, StringComparison.InvariantCultureIgnoreCase) ? true :
                    approverId.Equals(BackEndObjects.Invoice.INVOICE_APPROVAL_STAT_APPROVED) ? true : false;

                if (!allowedToEdit)  //No edit access because RFQ is sent for approval
                    gVR.Cells[1].Enabled = false;
                

                if (approverId.Equals(BackEndObjects.Invoice.INVOICE_APPROVAL_STAT_APPROVED))
                {
                    ((LinkButton)gVR.FindControl("LinkButton_Apprvl_Stat")).Enabled = false;
                    //((Label)gVR.FindControl("Label_Approved")).Visible = true;
                }
                else
                {
                    //Not allowed to enter the payment details if the invoice is not approved
                    ((LinkButton)gVR.FindControl("LinkButton_Pmnt_Det_Inv")).Enabled = false;
                }
            }
        }

        protected void GridView_Invoice_RowEditing(object sender, GridViewEditEventArgs e)
        {                       
            /*Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

        if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_INVOICE_SALES]||
            accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
        {*/
            Label_Invoice_Grid_Access.Visible = false;
            GridView_Invoice.EditIndex = e.NewEditIndex;
            GridView_Invoice.DataSource = (DataTable)Session[SessionFactory.ALL_SALE_ALL_INV_GRID_DATA];
            GridView_Invoice.DataBind();
        /*}
        else
        {
            Label_Invoice_Grid_Access.Visible = true;
            Label_Invoice_Grid_Access.Text = "You dont have edit access to invoice records";
        }*/
        }

        protected void GridView_Invoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            String selectedRfqId = ((Label)GridView_Invoice.SelectedRow.Cells[2].FindControl("Label_RFQId1")).Text;
            Session[SessionFactory.ALL_SALE_ALL_INV_SELECTED_RFQ_SPEC] = BackEndObjects.RFQProductServiceDetails.
                getAllProductServiceDetailsbyRFQIdDB(selectedRfqId);
            enableOnSelect("inv","");
        }

        protected void GridView_Invoice_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_Invoice.SelectedIndex = -1;
            GridView_Invoice.PageIndex = e.NewPageIndex;
            GridView_Invoice.DataSource = (DataTable)Session[SessionFactory.ALL_SALE_ALL_INV_GRID_DATA];
            GridView_Invoice.DataBind();
            disableOnPageChange("inv");
        }

        protected void LinkButton_Pmnt_Det_Inv_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_Invoice.SelectedIndex)
                GridView_Invoice.SelectRow(row.RowIndex);
            ((RadioButton)GridView_Invoice.SelectedRow.Cells[0].FindControl("inv_radio")).Checked = true;

            String forwardString = "/Pages/Popups/Purchase/Inv_Payment_Details.aspx";
            forwardString += "?rfId=" + ((Label)GridView_Invoice.SelectedRow.Cells[0].FindControl("Label_RFQId1")).Text;
            forwardString += "&context=" + "vendor";
            forwardString += "&invId=" + ((Label)GridView_Invoice.SelectedRow.Cells[0].FindControl("Label_Invoice_Id_Val")).Text;
            forwardString += "&invNo=" + ((Label)GridView_Invoice.SelectedRow.Cells[0].FindControl("Label_Invoice_No1")).Text;
            
            //Response.Write("<script type='text/javascript'>alert('" + "Hi" + "');</script>");
            //ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "alert('hi');", true);
            ScriptManager.RegisterStartupScript(this, typeof(string), "DispInvPmntVendor", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=900,Height=700');", true);
        }

        protected void LinkButton_Show_Inv_Invoice_Grid_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_Invoice.SelectedIndex)
                GridView_Invoice.SelectRow(row.RowIndex);
            ((RadioButton)GridView_Invoice.SelectedRow.Cells[0].FindControl("inv_radio")).Checked = true;

            String forwardString = "/Pages/Popups/Sale/Inv_Details.aspx";
            forwardString += "?rfId=" + ((Label)GridView_Invoice.SelectedRow.Cells[0].FindControl("Label_RFQId1")).Text;
            forwardString += "&context=" + "vendInvoiceGrid";

            String relatedPO = ((Label)GridView_Invoice.SelectedRow.Cells[0].FindControl("Label_Related_PO")).Text;
            if (!relatedPO.Equals(""))
                forwardString += "&poId=" + relatedPO;
            else
            forwardString += "&poId=" + BackEndObjects.PurchaseOrder.
                getPurchaseOrderforRFQIdDB(((Label)GridView_Invoice.SelectedRow.Cells[0].FindControl("Label_RFQId1")).Text).getPo_id();

            forwardString += "&invId=" + ((Label)GridView_Invoice.SelectedRow.Cells[0].FindControl("Label_Invoice_Id_Val")).Text;

            //Response.Write("<script type='text/javascript'>alert('" + "Hi" + "');</script>");
            //ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "alert('hi');", true);
            ScriptManager.RegisterStartupScript(this, typeof(string), "DispInvClientInvGrid", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=900');", true);
        }

        protected void GridView_Invoice_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            String invId = ((Label)GridView_Invoice.Rows[e.RowIndex].Cells[0].FindControl("Label_Invoice_Id_Val")).Text;
            String delivStat=((DropDownList)GridView_Invoice.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Deliv_Stat_Edit")).SelectedValue;
            String pmntStat=((DropDownList)GridView_Invoice.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Pmnt_Edit")).SelectedValue;
            String currId = ((DropDownList)GridView_Invoice.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Curr")).SelectedValue;

            Dictionary<String, String> whereCls = new Dictionary<String, String>();
               whereCls.Add(BackEndObjects.Invoice.INVOICE_COL_INVOICE_ID, invId);

            Dictionary<String, String> targetVals = new Dictionary<String, String>();
            targetVals.Add(BackEndObjects.Invoice.INVOICE_COL_DELIVERY_STAT, delivStat);
            targetVals.Add(BackEndObjects.Invoice.INVOICE_COL_PAYMENT_STAT, pmntStat);
            targetVals.Add(BackEndObjects.Invoice.INVOICE_COL_CURRENCY, currId);

            BackEndObjects.Invoice.updateInvoiceDetailsDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
            
            DataTable dt=(DataTable)Session[SessionFactory.ALL_SALE_ALL_INV_GRID_DATA];
            int index = GridView_Invoice.Rows[e.RowIndex].DataItemIndex;
            dt.Rows[index]["Deliv_Stat"] = delivStat;
            dt.Rows[index]["Pmnt_Stat"] = pmntStat;
            dt.Rows[index]["curr"] = ((DropDownList)GridView_Invoice.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Curr")).SelectedItem.Text;
            Session[SessionFactory.ALL_SALE_ALL_INV_GRID_DATA] = dt;

            GridView_Invoice.EditIndex = -1;
                GridView_Invoice.DataSource = dt;
                GridView_Invoice.DataBind();
                
        }

        protected void GridView_Invoice_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView_Invoice.EditIndex = -1;
            GridView_Invoice.DataSource=(DataTable)Session[SessionFactory.ALL_SALE_ALL_INV_GRID_DATA];
            GridView_Invoice.DataBind();
        }

        
        protected void Button_Convert_Lead_Command(object sender, CommandEventArgs e)
        {            
            String rfqId = ((Label)GridView_Lead.SelectedRow.Cells[2].FindControl("Label_RFQId")).Text;
            BackEndObjects.RFQDetails rfqObj = BackEndObjects.RFQDetails.getRFQDetailsbyIdDB(rfqId);
            
            ArrayList rfqProdQntyList = BackEndObjects.RFQProdServQnty.getRFQProductServiceQuantityforRFIdDB(rfqId);

            Dictionary<String,RFQResponseQuotes> leadRespDict= BackEndObjects.RFQResponseQuotes.
                getAllResponseQuotesforRFQandResponseEntityDB(rfqId, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
           ArrayList leadRespList=new ArrayList();

           foreach (KeyValuePair<String, RFQResponseQuotes> kvp in leadRespDict)
               leadRespList.Add(kvp.Value);

           BackEndObjects.RFQShortlisted potObj = new RFQShortlisted();
           potObj.setRFQId(rfqId);
           potObj.setRespEntityId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
           potObj.setPotentialId(new Id().getNewId(BackEndObjects.Id.ID_TYPE_POTENTIAL_ID_STRING));
           potObj.setFinlSupFlag("N");
           potObj.setFinlCustFlag("N");
           potObj.setCreateMode(RFQDetails.CREATION_MODE_MANUAL);
           potObj.setPotStat(BackEndObjects.PotentialStatus.POTENTIAL_STAT_PRELIM);
           potObj.setPotActStat(rfqObj.getActiveStat());
            potObj.setPotenAmnt(calculatePotAmnt(leadRespList,rfqProdQntyList));
            potObj.setCreatedDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            try
            {
                RFQShortlisted.insertRFQShorListedEntryDB(potObj);
                Label_Lead_Conv_Stat.Visible = true;
                Label_Lead_Conv_Stat.Text = "Lead Converted to Potential Successfully";
                Label_Lead_Conv_Stat.ForeColor = System.Drawing.Color.Green;
                fillLeadGrid();
                fillPotnGrid();
            }
            catch (Exception ex)
            {
                Label_Lead_Conv_Stat.Visible = true;
                Label_Lead_Conv_Stat.Text = "Lead Conversion to Potential Failed";
                Label_Lead_Conv_Stat.ForeColor = System.Drawing.Color.Red;
            }
        }

        /// <summary>
        /// Provided the reponse quote list and the product qunatity list this method will calculate the average potential amount
        /// </summary>
        /// <param name="respQuoteList"></param>
        /// <param name="prodQntyList"></param>
        /// <returns></returns>
        protected float calculatePotAmnt(ArrayList respQuoteList, ArrayList prodQntyList)
        {
            float totalAmnt = 0;
            Dictionary<String, BackEndObjects.RFQProdServQnty> respQuoteDict = new Dictionary<string, RFQProdServQnty>();

            for (int j = 0; j < prodQntyList.Count; j++)
            {
                respQuoteDict.Add(((RFQProdServQnty)prodQntyList[j]).getProdCatId(), (RFQProdServQnty)prodQntyList[j]);
            }

            for (int i = 0; i < respQuoteList.Count; i++)
            {
                BackEndObjects.RFQResponseQuotes respQuoteObj = (RFQResponseQuotes)respQuoteList[i];
                float fromQnty = (respQuoteDict[respQuoteObj.getPrdCatId()]).getFromQnty();
                float toQnty = (respQuoteDict[respQuoteObj.getPrdCatId()]).getToQnty();
                float quoteAmnt = float.Parse(respQuoteObj.getQuote());

                //Now calculate the average
                totalAmnt += (fromQnty * quoteAmnt + toQnty * quoteAmnt) / 2;
            }

            return totalAmnt;
        }

        protected void Button_Pot_Refresh_Click(object sender, EventArgs e)
        {
            fillPotnGrid();
            GridView_Potential.SelectedIndex = -1;
            disableOnPageChange("potn");
            Button_Pot_Refresh.Focus();
        }

        protected void Button_Lead_Refresh_Click(object sender, EventArgs e)
        {
            fillLeadGrid();
            GridView_Lead.SelectedIndex = -1;
            disableOnPageChange("lead");
            Button_Lead_Refresh.Focus();
        }

        protected void Button_Inv_Refresh_Click(object sender, EventArgs e)
        {
            fillInvoiceGrid(BackEndObjects.Invoice.getAllInvoicesbyRespEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()));
            GridView_Invoice.SelectedIndex = -1;
            disableOnPageChange("inv");
            Button_Inv_Refresh.Focus();
        }

        protected void LinkButton_Show_Defect1_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_Invoice.SelectedIndex)
                GridView_Invoice.SelectRow(row.RowIndex);
            ((RadioButton)GridView_Invoice.SelectedRow.Cells[0].FindControl("inv_radio")).Checked = true;

            String forwardString = "/Pages/Popups/Purchase/AllDefectsForInvoice.aspx";
            String invId = BackEndObjects.Invoice.
                getInvoicebyNoDB(((Label)GridView_Invoice.SelectedRow.Cells[0].FindControl("Label_Invoice_No1")).Text).getInvoiceId();

            forwardString += "?contextId1=" + invId;
            forwardString += "&contextId2=" + "vendor";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispDefectForInvoiceIncoming", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=900,Height=700');", true);

        }

        protected void LinkButton_TnC_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_Potential.SelectedIndex)
                GridView_Potential.SelectRow(row.RowIndex);

            ((RadioButton)GridView_Potential.SelectedRow.Cells[0].FindControl("potn_radio")).Checked = true;

            String forwardString = "/Pages/Popups/Sale/AllPotn_TnC.aspx";
            String createMode = ((Label)GridView_Potential.SelectedRow.Cells[0].FindControl("Label_Mode")).Text;

            forwardString += "?createMode=" + createMode;
           
            ScriptManager.RegisterStartupScript(this, typeof(string), "DispPotnTnC", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=600,Height=400');", true);

        }

        protected void Button_Create_Clone_Lead_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/createClone.aspx";
            String rfqId = ((Label)GridView_Lead.SelectedRow.Cells[0].FindControl("Label_RFQId")).Text;
            String contextString = ((Label)GridView_Lead.SelectedRow.Cells[0].FindControl("Label_RFQName")).Text;

            forwardString += "?contextId1=" + rfqId;
            forwardString += "&context=" + "lead";
            forwardString += "&contextString=" + contextString;

            ScriptManager.RegisterStartupScript(this, typeof(string), "CloneLead",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=400');", true);
        }

        protected void Button_Create_Clone_Potn_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/createClone.aspx";
            String rfqId = ((Label)GridView_Potential.SelectedRow.Cells[0].FindControl("Label_RFQId")).Text;
            String contextString = ((Label)GridView_Potential.SelectedRow.Cells[0].FindControl("Label_RFQName")).Text;

            forwardString += "?contextId1=" + rfqId;
            forwardString += "&context=" + "Potn";
            forwardString += "&contextString=" + contextString;

            ScriptManager.RegisterStartupScript(this, typeof(string), "ClonePotn",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=400');", true);
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
            string sortExpression = e.SortExpression;
            ViewState["SortExpressionLead"] = sortExpression;
            DataTable dt = (DataTable)Session[SessionFactory.ALL_SALE_ALL_LEAD_GRID_DATA];

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

            DataTable sortedTable=dt.DefaultView.ToTable();
            Session[SessionFactory.ALL_SALE_ALL_LEAD_GRID_DATA] = sortedTable;
            disableOnPageChange("lead");
            GridView_Lead.SelectedIndex = -1;
            bindSortedData(GridView_Lead, sortedTable);
        }

        protected void bindSortedData(GridView grd, DataTable dt)
        {
            grd.DataSource = dt;
            grd.DataBind();
        }

        protected void LinkButton_NDA_Command(object sender, CommandEventArgs e)
        {

        }

        protected void Button_Audit_Lead_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/DispAudit.aspx";
            forwardString += "?contextId1=" + ((Label)GridView_Lead.SelectedRow.Cells[0].FindControl("Label_RFQId")).Text;
            forwardString += "&contextId2=" + "";
            forwardString += "&contextId3=" + "";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispAuditLead", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=700');", true);

        }

        protected void enableOnSelect(String senderName,String createMode)
        {
            switch (senderName)
            {
                case "lead": Button_Audit_Lead.Enabled = true;
                    Button_Lead_Doc.Enabled = true;
                    Button_Notes_Lead.Enabled = true;
                    break;
                case "potn": Button_Audit_Potn.Enabled = true;
                    Button_Potn_Doc.Enabled = true;
                    Button_Notes_Potn.Enabled = true;
                    //Finalize deal message invisible for both page change and index change
                    Label_Finalize_Stat.Visible = false;
                    if (createMode.Equals(BackEndObjects.RFQShortlisted.POTENTIAL_CREATION_MODE_MANUAL))
                        Button_Sales_Order.Enabled=true;
                    break;
                case "inv": Button_Audit_Inv.Enabled = true;
                    Button_Notes_Inv.Enabled = true;
                    Button_Workflow_Tree_Inv.Enabled = true;
                    break;
            }
        }

        protected void disableOnPageChange(String senderName)
        {
            switch (senderName)
            {
                case "lead": Button_Audit_Lead.Enabled = false;
                    Button_Lead_Doc.Enabled = false;
                    Button_Notes_Lead.Enabled = false;
                                Button_Create_Clone_Lead.Enabled = false;
            Button_Convert_Lead.Enabled = false;
            Label_Lead_Grid_Access.Visible = false;
                    break;
                case "potn": Button_Audit_Potn.Enabled = false;
                    Button_Potn_Doc.Enabled = false;
                    Button_Sales_Order.Enabled = false;
                    Button_Notes_Potn.Enabled = false;
                    Label_Finalize_Stat.Visible = false;
                                Button_Create_Clone_Potn.Enabled = false;
            Label_Potential_Grid_Access.Visible = false;
            Button_Inv_Pmnt.Enabled = false;
                    break;
                case "inv": Button_Audit_Inv.Enabled = false;
                    Button_Notes_Inv.Enabled = false;
                    Button_Workflow_Tree_Inv.Enabled = false;
                    break;


                case "so": Button_Notes_PO.Enabled = false;
                    break;
            }
        }

        protected void Button_Audit_Potn_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/DispAudit.aspx";
            forwardString += "?contextId1=" + ((Label)GridView_Potential.SelectedRow.Cells[0].FindControl("Label_Hidden_Pot_Id")).Text;
            forwardString += "&contextId2=" + "";
            forwardString += "&contextId3=" + "";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispAuditPotential", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=700');", true);

        }

        protected void Button_Audit_Inv_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/DispAudit.aspx";

            forwardString += "?contextId1=" + ((Label)GridView_Invoice.SelectedRow.Cells[0].FindControl("Label_Invoice_Id_Val")).Text;
            forwardString += "&contextId2=" + "";
            forwardString += "&contextId3=" + "";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispAuditSalesInv", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=700');", true);

        }

        protected void Button_Sales_Order_Click(object sender, EventArgs e)
        {            
            String rfqId = Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_SELECTED_RFQ_ID].ToString();
            String entId = BackEndObjects.RFQDetails.getRFQDetailsbyIdDB(rfqId).getEntityId();
            String potStg = ((Label)GridView_Potential.SelectedRow.Cells[0].FindControl("Label_Pot_Stage")).Text;         

            //Allow only if the supplier finalized the deal
            if (potStg.Equals(BackEndObjects.RFQShortlisted.POTENTIAL_ACTIVE_STAT_CLOSED_WON))
            {
                //If the PO already does not exist then go to create screen
                if (((LinkButton)GridView_Potential.SelectedRow.Cells[0].FindControl("LinkButton_COM")).Text.Trim().Equals(""))
                {
                    String forwardString = "/Pages/Popups/Purchase/FinalizeDeal.aspx";
                    forwardString += "?EntId=" + entId + "&rfqId=" + rfqId + "&context=" + "vendor";
                    forwardString += "&dataItemIndex=" + GridView_Potential.SelectedRow.DataItemIndex;

                    Button_Finalz_Deal.Enabled = false;
                    ScriptManager.RegisterStartupScript(this, typeof(string), "DispSO", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=700');", true);
                }
                else
                {
                    String forwardString = "/Pages/Popups/Purchase/PO_Details.aspx";
                    forwardString += "?rfId=" + ((Label)GridView_Potential.SelectedRow.Cells[0].FindControl("Label_RFQId")).Text;
                    forwardString += "&context=" + "vendor";
                    forwardString += "&poId=" + ((LinkButton)GridView_Potential.SelectedRow.Cells[0].FindControl("LinkButton_COM")).Text;

                   forwardString += "&createMode=" + ((Label)GridView_Potential.SelectedRow.Cells[0].FindControl("Label_Mode")).Text;

                   if (((Label)GridView_Potential.SelectedRow.Cells[0].FindControl("Label_Mode")).Text.
   Equals(BackEndObjects.RFQShortlisted.POTENTIAL_CREATION_MODE_MANUAL) && GridView_Potential.Columns[1].Visible)
                       forwardString += "&allowEdit=" + "true";

                    //Response.Write("<script type='text/javascript'>alert('" + "Hi" + "');</script>");
                    //ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "alert('hi');", true);
                    ScriptManager.RegisterStartupScript(this, typeof(string), "DispPOSales", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=900');", true);

                }
          }
            else
            {
                Label_Finalize_Stat.Visible = true;
                Label_Finalize_Stat.ForeColor = System.Drawing.Color.Red;
                Label_Finalize_Stat.Text = "To Create Sales Order Potential Stage Should be Closed!Won";
            }
        }

        protected void Button_Notes_Inv_Click(object sender, EventArgs e)
        {
            String forwardString = "NoteDetails.aspx";

            String rfqId = ((Label)GridView_Invoice.SelectedRow.Cells[0].FindControl("Label_Invoice_Id_Val")).Text;

            forwardString += "?contextId=" + rfqId;

            ScriptManager.RegisterStartupScript(this, typeof(string), "NotesInv",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=800,height=800,left=500,right=500');", true);

        }

        protected void Button_Notes_Potn_Click(object sender, EventArgs e)
        {
            String forwardString = "NoteDetails.aspx";

            String rfqId = ((Label)GridView_Potential.SelectedRow.Cells[0].FindControl("Label_Hidden_Pot_Id")).Text;

            forwardString += "?contextId=" + rfqId;

            ScriptManager.RegisterStartupScript(this, typeof(string), "NotesPotn",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=800,height=800,left=500,right=500');", true);
        }

        protected void Button_Notes_Lead_Click(object sender, EventArgs e)
        {
            String forwardString = "NoteDetails.aspx";

            String rfqId = ((Label)GridView_Lead.SelectedRow.Cells[0].FindControl("Label_RFQId")).Text;

            forwardString += "?contextId=" + rfqId;

            ScriptManager.RegisterStartupScript(this, typeof(string), "LeadNotes",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=800,height=800,left=500,right=500');", true);
        }

        protected void Button_Potn_Doc_Click(object sender, EventArgs e)
        {
            String forwardString = "Popups/Sale/AllPotn_NDA.aspx";
            //window.open('Popups/Sale/AllPotn_NDA.aspx','DispNDAPot','status=1,toolbar=1,width=500,height=200,left=500,right=500',true);
            String createMode = ((Label)GridView_Potential.SelectedRow.Cells[0].FindControl("Label_Mode")).Text;

            forwardString += "?createMode=" + createMode;
            
            ScriptManager.RegisterStartupScript(this, typeof(string), "potnDoc",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=600,height=400,left=500,right=500');", true);
        }

        protected void Button_Lead_Doc_Click(object sender, EventArgs e)
        {
            String forwardString = "Popups/Sale/AllLead_NDA.aspx";
            //window.open('Popups/Sale/AllPotn_NDA.aspx','DispNDAPot','status=1,toolbar=1,width=500,height=200,left=500,right=500',true);
            String createMode = ((Label)GridView_Lead.SelectedRow.Cells[0].FindControl("Label_Mode")).Text;

            forwardString += "?createMode=" + createMode;

            ScriptManager.RegisterStartupScript(this, typeof(string), "LeadDoc",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=600,height=400,left=500,right=500');", true);
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
            DataTable dt = (DataTable)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_GRID_DATA];

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
            Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_GRID_DATA] = sortedTable;
            disableOnPageChange("potn");
            GridView_Potential.SelectedIndex = -1;
            bindSortedData(GridView_Potential, sortedTable);
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
            DataTable dt = (DataTable)Session[SessionFactory.ALL_SALE_ALL_INV_GRID_DATA];

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
            Session[SessionFactory.ALL_SALE_ALL_INV_GRID_DATA] = sortedTable;
            disableOnPageChange("inv");
            GridView_Invoice.SelectedIndex = -1;
            bindSortedData(GridView_Invoice, sortedTable);
        }

        protected void LinkButton_Assgn_To_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_Lead.SelectedIndex)
                 GridView_Lead.SelectRow(row.RowIndex);
            
            ((RadioButton)GridView_Lead.SelectedRow.Cells[0].FindControl("lead_radio")).Checked = true;

            String forwardString = "DispUserDetails.aspx";
            //window.open('Popups/Sale/AllPotn_NDA.aspx','DispNDAPot','status=1,toolbar=1,width=500,height=200,left=500,right=500',true);
            String userId = ((LinkButton)GridView_Lead.SelectedRow.Cells[0].FindControl("LinkButton_Assgn_To")).Text;

            forwardString += "?userId=" + userId;            
            ScriptManager.RegisterStartupScript(this, typeof(string), "userDetLead",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=400,height=400,left=500,right=500');", true);


        }

        //For Potential assigned to user details
        protected void LinkButton_Assgn_To_Command1(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_Potential.SelectedIndex)
                GridView_Potential.SelectRow(row.RowIndex);
            ((RadioButton)GridView_Potential.SelectedRow.Cells[0].FindControl("potn_radio")).Checked = true;

            String forwardString = "DispUserDetails.aspx";
            //window.open('Popups/Sale/AllPotn_NDA.aspx','DispNDAPot','status=1,toolbar=1,width=500,height=200,left=500,right=500',true);
            String userId = ((LinkButton)GridView_Potential.SelectedRow.Cells[0].FindControl("LinkButton_Assgn_To")).Text;

            forwardString += "?userId=" + userId;

            ScriptManager.RegisterStartupScript(this, typeof(string), "userDetPotn",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=400,height=400,left=500,right=500');", true);

        }

        protected void Button_Lead_Refresh_Hidden_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session[SessionFactory.ALL_SALE_ALL_LEAD_GRID_DATA];
            GridView_Lead.DataSource = dt;
            GridView_Lead.DataBind();
            GridView_Lead.SelectedIndex = -1;
            disableOnPageChange("lead");
        }

        protected void Button_Potn_Refresh_Hidden_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_GRID_DATA];
            GridView_Potential.DataSource = dt;
            GridView_Potential.DataBind();
            GridView_Potential.SelectedIndex = -1;
            disableOnPageChange("potn");
        }

        protected void Button_Potn_Refresh_Hidden_Index_Unchanged_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_GRID_DATA];
            GridView_Potential.DataSource = dt;
            GridView_Potential.DataBind();
            //GridView_Potential.SelectedIndex = -1;
            //disableOnPageChange("potn");
        }

        protected void Button_Inv_Refresh_Hidden_Command(object sender, CommandEventArgs e)
        {
            DataTable dt = (DataTable)Session[SessionFactory.ALL_SALE_ALL_INV_GRID_DATA];
            GridView_Invoice.DataSource = dt;
            GridView_Invoice.DataBind();
            GridView_Invoice.SelectedIndex = -1;
            disableOnPageChange("inv");
        }

        protected void LinkButton_Apprvl_Stat_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_Invoice.SelectedIndex)
                GridView_Invoice.SelectRow(row.RowIndex);
            ((RadioButton)GridView_Invoice.SelectedRow.Cells[0].FindControl("inv_radio")).Checked = true;

            String forwardString = "DispUserDetails.aspx";
            //window.open('Popups/Sale/AllPotn_NDA.aspx','DispNDAPot','status=1,toolbar=1,width=500,height=200,left=500,right=500',true);
            String userId = ((LinkButton)GridView_Invoice.SelectedRow.Cells[0].FindControl("LinkButton_Apprvl_Stat")).Text;

            forwardString += "?userId=" + userId;

            ScriptManager.RegisterStartupScript(this, typeof(string), "userDetInv",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=400,height=400,left=500,right=500');", true);

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

        protected void LinkButton_Customer_Gridview_Lead_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_Lead.SelectedIndex)
                GridView_Lead.SelectRow(row.RowIndex);
            ((RadioButton)GridView_Lead.SelectedRow.Cells[0].FindControl("lead_radio")).Checked = true;

            String forwardString = "Popups/Sale/AllLead_Customer.aspx";
            ScriptManager.RegisterStartupScript(this, typeof(string), "DispCust",
               "window.open('" + forwardString + "',null,'status=1,location=yes,width=1000,height=400,left=100,right=500');", true);
        }

        protected void GridView_Lead_Spec_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_Lead.SelectedIndex)
                GridView_Lead.SelectRow(row.RowIndex);

            ((RadioButton)GridView_Lead.SelectedRow.Cells[0].FindControl("lead_radio")).Checked = true;

            String forwardString = "Popups/Sale/AllLead_Specification.aspx";
            ScriptManager.RegisterStartupScript(this, typeof(string), "DispSpecLead",
               "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=1000,height=600,left=100,right=500');", true);
        }


        protected void GridView_Lead_LinkButton_Location_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_Lead.SelectedIndex)
                GridView_Lead.SelectRow(row.RowIndex);

            ((RadioButton)GridView_Lead.SelectedRow.Cells[0].FindControl("lead_radio")).Checked = true;

            String forwardString = "Popups/Sale/AllLead_Location.aspx";
            ScriptManager.RegisterStartupScript(this, typeof(string), "LeadLocation",
               "window.open('" + forwardString + "',null,'status=1,width=800,height=400,left=500,right=500');", true);
        }

        protected void GridView_Lead_TC_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_Lead.SelectedIndex)
                GridView_Lead.SelectRow(row.RowIndex);

            ((RadioButton)GridView_Lead.SelectedRow.Cells[0].FindControl("lead_radio")).Checked = true;

            String forwardString = "Popups/Sale/AllLead_TnC.aspx";
            ScriptManager.RegisterStartupScript(this, typeof(string), "DispTnC",
               "window.open('" + forwardString + "',null,'status=1,width=600,height=400,left=500,right=500');", true);
        }

        protected void GridView_Inv_RadioSelect(object sender, EventArgs e)
        {
            RadioButton linkb = (RadioButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1)
                GridView_Invoice.SelectRow(row.RowIndex);
        }

        protected void LinkButton_Spec_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_Potential.SelectedIndex)
                GridView_Potential.SelectRow(row.RowIndex);

            ((RadioButton)GridView_Potential.SelectedRow.Cells[0].FindControl("potn_radio")).Checked = true;
            String forwardString = "Popups/Sale/AllPot_Specification.aspx";
            ScriptManager.RegisterStartupScript(this, typeof(string), "DispSpecPotn",
               "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=1000,height=600,left=100,right=500');", true);

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

        protected void LinkButton_Location_Pot_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_Potential.SelectedIndex)
                GridView_Potential.SelectRow(row.RowIndex);

            ((RadioButton)GridView_Potential.SelectedRow.Cells[0].FindControl("potn_radio")).Checked = true;
            String forwardString = "Popups/Sale/AllPotential_Location.aspx";
            ScriptManager.RegisterStartupScript(this, typeof(string), "PotnLocation",
               "window.open('" + forwardString + "',null,'status=1,width=800,height=400,left=500,right=500');", true);
        }

        protected void Button_Workflow_Tree_Inv_Click(object sender, EventArgs e)
        {
            String forwardString = "Workflow_Tree.aspx";

            String invId = ((Label)GridView_Invoice.SelectedRow.Cells[0].FindControl("Label_Invoice_Id_Val")).Text;

            forwardString += "?contextId=" + invId;
            forwardString += "&contextName=" + BackEndObjects.Workflow_Action.WORKFLOW_ACTION_CONTEXT_NAME_INV;
            forwardString += "&approvalContext=N";
            ScriptManager.RegisterStartupScript(this, typeof(string), "workflow_tree_inv",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=500,height=400,left=500,right=500');", true);
        }

        protected void GridView_SO_RadioSelect(object sender, EventArgs e)
        {
            RadioButton linkb = (RadioButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1)
                GridView_SO.SelectRow(row.RowIndex);
        }

        protected void loadSOGrid()
        {            
            Dictionary<String,RFQShortlisted> rfqListSO  = BackEndObjects.RFQShortlisted.getAllRFQShortlistedEntriesforFinalizedVendor(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                //(ArrayList)Session[SessionFactory.ALL_SALE_ALL_POTN_RFQ_LIST];
                //BackEndObjects.RFQDetails.getAllRFQIncludingDummybyEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), true);
            if (rfqListSO.Count > 0)
                fillSOGrid(rfqListSO);
        }

        protected void fillSOGrid(Dictionary<String, RFQShortlisted> rfqDict)
        {
            Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];
            Dictionary<String, Currency> allCurrList = (Dictionary<String, Currency>)Session[SessionFactory.CURRENCY_LIST];

            if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_SO_SALES] ||
                accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
            {
                Label_PO_Grid_Access.Visible = false;

                DataTable dt = new DataTable();
                dt.Columns.Add("RFQNo");
                dt.Columns.Add("curr");
                dt.Columns.Add("SO_No");
                dt.Columns.Add("SO_Date");
                dt.Columns.Add("Amount");//
                dt.Columns.Add("SO_Date_Ticks");
                dt.Columns.Add("CreateMode");

                ArrayList rfqList = new ArrayList();
                ArrayList poList = new ArrayList();
                DateUtility dU = new DateUtility();

                foreach (KeyValuePair<String, RFQShortlisted> kvp in rfqDict)
                    rfqList.Add(kvp.Key);

               /* for (int i = 0; i < rfqDict.Count; i++)
                {
                    rfqList.Add(((BackEndObjects.RFQShortlisted)rfqDict[i]).getRFQId());
                }*/
                Dictionary<String, String> poDict = rfqList != null && rfqList.Count > 0 ?
                    PurchaseOrder.getPurchaseOrdersforRFQIdListDB(rfqList) : new Dictionary<String, String>();

                String[] poArray = poDict.Values.ToArray<String>();
                for (int i = 0; i < poArray.Length; i++)
                {
                    poList.Add(poArray[i]);
                }
                Dictionary<String, PurchaseOrder> poObjDict = PurchaseOrder.getPurchaseOrdersforPOIdListDB(poList);

                int rowCount = 0;
                foreach (KeyValuePair<String, PurchaseOrder> kvp in poObjDict)
                {
                    dt.Rows.Add();
                    PurchaseOrder poObj = kvp.Value;
                    dt.Rows[rowCount]["RFQNo"] = poObj.getRfq_id();
                    dt.Rows[rowCount]["curr"] = poObj.getCurrency() != null &&
    allCurrList.ContainsKey(poObj.getCurrency()) ?
               allCurrList[poObj.getCurrency()].getCurrencyName() : "";
                    dt.Rows[rowCount]["SO_Date"] = dU.getConvertedDate(poObj.getDate_created());
                    dt.Rows[rowCount]["So_No"] = poObj.getPo_id();
                    dt.Rows[rowCount]["Amount"] = poObj.getAmount();
                    dt.Rows[rowCount]["SO_Date_Ticks"] = Convert.ToDateTime(poObj.getDate_created()).Ticks;
                    dt.Rows[rowCount]["CreateMode"] = rfqDict[poObj.getRfq_id()].getCreateMode();

                    rowCount++;
                }

                dt.DefaultView.Sort = "SO_Date_Ticks" + " " + "DESC";
                disableOnPageChange("po");
                GridView_SO.SelectedIndex = -1;
                GridView_SO.DataSource = dt;
                GridView_SO.DataBind();
                GridView_SO.Visible = true;
                Session[SessionFactory.ALL_SALE_ALL_SO_GRID_DATA] = dt.DefaultView.ToTable();

                if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_SO_SALES] &&
    !accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                    GridView_SO.Columns[1].Visible = false; //Hide edit option if no edit access

            }
            else
            {
                Label_PO_Grid_Access.Visible = true;
                Label_PO_Grid_Access.Text = "You don't have access to view this Section";
            }
        }

        protected void Button_Filter_SO_Click(object sender, EventArgs e)
        {
            fillSOGridwithFilter();
            if (GridView_SO.Rows.Count > 0)
                GridView_SO.Focus();
            else
                GridView_SO.Focus();
        }

        protected void fillSOGridwithFilter()
        {
            Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

            if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_SO_SALES] ||
                accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
            {
                String poId = TextBox_po.Text;
                String client = DropDownList_Client_SO.SelectedValue;
                String fromDate = TextBox_From_Date_PO.Text;
                String toDate = TextBox_To_Date_PO.Text;
                String rfqNo = TextBox_rfq_no_po.Text;

                ActionLibrary.PurchaseActions._dispPODetails dPo = new ActionLibrary.PurchaseActions._dispPODetails();

                Dictionary<String, String> filterParams = new Dictionary<string, string>();
                if (!rfqNo.Trim().Equals(""))
                    filterParams.Add(dPo.FILTER_BY_RFQ_NO, rfqNo);
                if (!poId.Trim().Equals(""))
                    filterParams.Add(dPo.FILTER_BY_PO_NO, poId);
                if (!client.Trim().Equals("_"))
                    filterParams.Add(dPo.FILTER_BY_CLIENT, client);
                if (fromDate != null && !fromDate.Equals(""))
                    filterParams.Add(dPo.FILTER_BY_SUBMIT_DATE_FROM, fromDate);
                if (toDate != null && !toDate.Equals(""))
                    filterParams.Add(dPo.FILTER_BY_SUBMIT_DATE_TO, toDate);

                ArrayList poFilteredList = dPo.getAllSODetailsFiltered(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), User.Identity.Name, filterParams);
                DataTable dt = new DataTable();
                dt.Columns.Add("RFQNo");
                dt.Columns.Add("curr");
                dt.Columns.Add("SO_No");
                dt.Columns.Add("SO_Date");
                dt.Columns.Add("Amount");//
                dt.Columns.Add("SO_Date_Ticks");
                dt.Columns.Add("CreateMode");

                if (poFilteredList != null && poFilteredList.Count > 0)
                {
                    int rowCount = 0;
                    DateUtility dU = new DateUtility();

                    Dictionary<String, Currency> allCurrList = (Dictionary<String, Currency>)Session[SessionFactory.CURRENCY_LIST];

                    for (int i = 0; i < poFilteredList.Count; i++)
                    {
                        PurchaseOrder poObj = (PurchaseOrder)poFilteredList[i];

                        dt.Rows.Add();

                        dt.Rows[rowCount]["RFQNo"] = poObj.getRfq_id().Substring(0,poObj.getRfq_id().IndexOf(";"));
                        dt.Rows[rowCount]["curr"] = poObj.getCurrency() != null &&
                    allCurrList.ContainsKey(poObj.getCurrency()) ?
                     allCurrList[poObj.getCurrency()].getCurrencyName() : "";
                        dt.Rows[rowCount]["SO_Date"] = dU.getConvertedDate(poObj.getDate_created());
                        dt.Rows[rowCount]["So_No"] = poObj.getPo_id();
                        dt.Rows[rowCount]["Amount"] = poObj.getAmount();
                        dt.Rows[rowCount]["SO_Date_Ticks"] = Convert.ToDateTime(poObj.getDate_created()).Ticks;
                        dt.Rows[rowCount]["CreateMode"] = poObj.getRfq_id().Substring(poObj.getRfq_id().IndexOf(";")+1);
                        rowCount++;
                    }
                }

                dt.DefaultView.Sort = "SO_Date_Ticks" + " " + "DESC";
                disableOnPageChange("po");
                GridView_SO.SelectedIndex = -1;
                GridView_SO.DataSource = dt;
                GridView_SO.DataBind();
                GridView_SO.Visible = true;
                Session[SessionFactory.ALL_SALE_ALL_SO_GRID_DATA] = dt.DefaultView.ToTable();

            }
        }

        protected void enableOnSelect(String senderName)
        {
            switch (senderName)
            {
                case "so": Button_Notes_PO.Enabled = true;
                    break;
            }
        }

        protected void GridView_SO_SelectedIndexChanged(object sender, EventArgs e)
        {
            String selectedRfqId = ((Label)GridView_SO.SelectedRow.Cells[2].FindControl("Label_RFQId1")).Text;
            // Session[SessionFactory.ALL_PURCHASE_ALL_INV_SELECTED_RFQ_SPEC] = BackEndObjects.RFQProductServiceDetails.
            //getAllProductServiceDetailsbyRFQIdDB(selectedRfqId);
            ((RadioButton)GridView_SO.SelectedRow.Cells[0].FindControl("so_radio")).Checked = true;
            enableOnSelect("so");
        }

        protected void LinkButton_SO_Purchase_Order_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_SO.SelectedIndex)
                GridView_SO.SelectRow(row.RowIndex);

            ((RadioButton)GridView_SO.SelectedRow.Cells[0].FindControl("so_radio")).Checked = true;

            String rfqId = ((Label)GridView_SO.SelectedRow.Cells[0].FindControl("Label_RFQId1")).Text;

            String forwardString = "/Pages/Popups/Purchase/PO_Details.aspx";
            forwardString += "?rfId=" + rfqId;
            forwardString += "&context=" + "vendor";//createMode

            forwardString += "&poId=" + ((LinkButton)GridView_SO.SelectedRow.Cells[0].FindControl("LinkButton_PO_Purchase_Order")).Text;
            forwardString += "&createMode=" + ((Label)GridView_SO.SelectedRow.Cells[0].FindControl("Label_Create_Mode")).Text;

            /*Invoice invObj = rfqId.IndexOf("dummy") >= 0 ? null : BackEndObjects.Invoice.getInvoicebyRfIdDB(rfqId);
            if ((invObj == null || invObj.getInvoiceId() == null || invObj.getInvoiceId().Equals("")) && GridView_SO.Columns[1].Visible) //Deal finalized for this RFQ - not edit allowed for PO
            {
                forwardString += "&allowEdit=" + "true";
                forwardString += "&refreshParent=" + "true";
                forwardString += "&dataItemIndex=" + row.DataItemIndex;
            }*/
            if (((Label)GridView_SO.SelectedRow.Cells[0].FindControl("Label_Create_Mode")).Text.Equals(RFQShortlisted.POTENTIAL_CREATION_MODE_MANUAL)
                && GridView_SO.Columns[1].Visible) //Allow edit of the SO only if the RFQ is manually created and there is edit access to the grid
            {//For auto created RFQs PO issued by the client is same as the final SO and can not be modified by the vendor
                forwardString += "&allowEdit=" + "true";
                forwardString += "&refreshParent=" + "true";
                forwardString += "&dataItemIndex=" + row.DataItemIndex;
            }

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispSO", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=900');", true);


        }

        protected void Button_SO_Refresh_Hidden_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session[SessionFactory.ALL_SALE_ALL_SO_GRID_DATA];
            //dt.DefaultView.Sort = "Submit Date Ticks" + " " + "DESC";         
            GridView_SO.DataSource = dt;
            GridView_SO.DataBind();
            GridView_SO.SelectedIndex = -1;
            //GridView1.PageIndex = GridView1.PageCount - 1;
            //GridView1.PageIndex = 0;
        }

        protected void Button_SO_Refresh_Click(object sender, EventArgs e)
        {
            Dictionary<String, RFQShortlisted> rfqListPO =
 BackEndObjects.RFQShortlisted.getAllRFQShortlistedEntriesforFinalizedVendor(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            if (rfqListPO.Count > 0)
                fillSOGrid(rfqListPO);
            
            GridView_SO.SelectedIndex = -1;
            disableOnPageChange("so");
            Button_PO_Refresh.Focus();
        }

        protected void Button_Notes_SO_Click(object sender, EventArgs e)
        {
            String forwardString = "NoteDetails.aspx";

            String rfqId = ((Label)GridView_SO.SelectedRow.Cells[0].FindControl("Label_RFQId1")).Text;

            forwardString += "?contextId=" + rfqId;

            ScriptManager.RegisterStartupScript(this, typeof(string), "SONotes",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=800,height=800,left=500,right=500');", true);
        }

        protected void GridView_SO_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                GridViewRow gVR = e.Row;
                loadCurrency(((DropDownList)gVR.FindControl("DropDownList_Curr")), ((Label)gVR.FindControl("Label_Curr_Edit")).Text);
            }
            else if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Normal) == DataControlRowState.Normal)
            {
                GridViewRow gVR = e.Row;
                if (((Label)gVR.FindControl("Label_RFQId1")).Text.IndexOf("dummy") >= 0)
                    ((Label)gVR.FindControl("Label_RFQId1")).Visible = false;

                //No edit allowed for auto created RFQs
                if (((Label)gVR.FindControl("Label_Create_Mode")).Text.Equals(RFQShortlisted.POTENTIAL_CREATION_MODE_AUTO))
                    gVR.Cells[1].Enabled = false;
            }
        }

        protected void GridView_SO_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_SO.PageIndex = e.NewPageIndex;
            GridView_SO.DataSource = (DataTable)Session[SessionFactory.ALL_SALE_ALL_SO_GRID_DATA];
            GridView_SO.DataBind();
            GridView_SO.SelectedIndex = -1;
        }

        protected void GridView_SO_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView_SO.EditIndex = e.NewEditIndex;
            GridView_SO.DataSource = (DataTable)Session[SessionFactory.ALL_SALE_ALL_SO_GRID_DATA];
            GridView_SO.DataBind();
        }

        protected void GridView_SO_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView_SO.EditIndex = -1;
            GridView_SO.DataSource = (DataTable)Session[SessionFactory.ALL_SALE_ALL_SO_GRID_DATA];
            GridView_SO.DataBind();
        }

        protected void GridView_SO_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DataTable dt = (DataTable)Session[SessionFactory.ALL_SALE_ALL_SO_GRID_DATA];
            GridViewRow gVR = GridView_SO.Rows[e.RowIndex];

            int index = GridView_SO.Rows[e.RowIndex].DataItemIndex;

            String curr = "";
            curr = ((DropDownList)gVR.FindControl("DropDownList_Curr")).SelectedItem.Text;
            dt.Rows[index]["curr"] = curr;

            GridView_SO.EditIndex = -1;
            GridView_SO.DataSource = dt;
            GridView_SO.DataBind();

            Session[SessionFactory.ALL_SALE_ALL_SO_GRID_DATA] = dt;

            try
            {
                String poId = ((LinkButton)gVR.Cells[0].FindControl("LinkButton_PO_Purchase_Order")).Text;
                Dictionary<String, String> whereCls = new Dictionary<string, string>();
                Dictionary<String, String> targetVals = new Dictionary<string, string>();

                whereCls.Add(BackEndObjects.PurchaseOrder.PURCHASE_ORDER_COL_PO_ID, poId);

                targetVals.Add(BackEndObjects.PurchaseOrder.PURCHASE_ORDER_COL_CURRENCY, ((DropDownList)gVR.FindControl("DropDownList_Curr")).SelectedValue);

                BackEndObjects.PurchaseOrder.updatePurchaseOrderDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);

            }
            catch (Exception ex)
            {
            }
        }

        private SortDirection GridViewSortDirectionSO
        {
            get
            {
                if (ViewState["sortDirectionSO"] == null)
                    ViewState["sortDirectionSO"] = SortDirection.Descending;
                return (SortDirection)ViewState["sortDirectionSO"];
            }
            set { ViewState["sortDirectionSO"] = value; }
        }

        protected void GridView_SO_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            ViewState["SortExpressionSO"] = sortExpression;
            DataTable dt = (DataTable)Session[SessionFactory.ALL_SALE_ALL_SO_GRID_DATA];

            if (GridViewSortDirectionSO == SortDirection.Ascending)
            {
                GridViewSortDirectionSO = SortDirection.Descending;
                dt.DefaultView.Sort = sortExpression + " " + "DESC";
            }
            else
            {
                GridViewSortDirectionSO = SortDirection.Ascending;
                dt.DefaultView.Sort = sortExpression + " " + "ASC";
            }

            DataTable sortedTable = dt.DefaultView.ToTable();
            Session[SessionFactory.ALL_SALE_ALL_SO_GRID_DATA] = sortedTable;
            disableOnPageChange("so");
            GridView_SO.SelectedIndex = -1;
            bindSortedData(GridView_SO, sortedTable);
        }

        protected void Button_Create_PO_Click(object sender, EventArgs e)
        {
            String forwardString = "CreatePO.aspx";
            forwardString += "?context=" + "vendor";
            ScriptManager.RegisterStartupScript(this, typeof(string), "createSO", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=1000,Height=1000,left=100,right=500');", true);

        }

        protected void Button_PO_Refresh_Hidden_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session[SessionFactory.ALL_SALE_ALL_SO_GRID_DATA];
            //dt.DefaultView.Sort = "Submit Date Ticks" + " " + "DESC";         
            GridView_SO.DataSource = dt;
            GridView_SO.DataBind();
            GridView_SO.SelectedIndex = -1;
            //GridView1.PageIndex = GridView1.PageCount - 1;
            //GridView1.PageIndex = 0;
        }




    }
}