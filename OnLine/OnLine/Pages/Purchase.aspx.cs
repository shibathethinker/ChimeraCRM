using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActionLibrary;
using BackEndObjects;
using System.IO;
using System.Web.UI.HtmlControls;

namespace OnLine.Pages
{
    public partial class Purchase : System.Web.UI.Page
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
                    
                    //((Menu)Master.FindControl("Menu1")).Items[1].Selected = true;
                    ((HtmlGenericControl)(Master.FindControl("Purchase"))).Attributes.Add("class", "active");

                    String entId = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();
                    Dictionary<String, bool> accessList = (Dictionary<String,bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];
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
                    //Dictionary<String, userDetails> allUserDetails = BackEndObjects.MainBusinessEntity.getUserDetailsforMainEntitybyIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                    //Cache.Insert(entId, allUserDetails);

                    populateLogo();
                    CheckAccessToActions();
                    if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                    { //Full Access - no need to do any restriction
                        loadContacts();
                        LoadProductCat();
                        LoadReqrActiveStat();
                        fillGridsOnPageLoad();
                        populateInvFilter();
                    }
                    else if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_PURCHASE_SCREEN_VIEW])
                    {
                        loadContacts();
                        LoadProductCat();
                        LoadReqrActiveStat();
                        fillGridsOnPageLoad();
                        populateInvFilter();                        
                    }
                    else
                    {
                        Label_Purchase_Screen_Access.Visible = true;
                        Label_Purchase_Screen_Access.Text = "You don't have access to view this page";
                    }
                }
            }
        }

        protected void loadCurrency(DropDownList DropDownList_Curr,String selectedVal)
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

        protected void enableOnSelect(String senderName)
        {
            switch (senderName)
            {
                case "rfq": Button_Audit_RFQ.Enabled = true;
                    Button_RFQ_Doc.Enabled = true;
                    Button_Notes_RFQ.Enabled = true;
                    Button_Workflow_Tree.Enabled = true;                    
                    break;
                case "req": Button_Audit_Req.Enabled = true;
                    Button_Notes.Enabled = true;
                    break;

                case "inv": Button_Notes_Inv.Enabled = true;
                    break;

                case "po": Button_Notes_PO.Enabled = true;
                    break;
            }
        }

        protected void loadContacts()
        {
            ListItem firstItem = new ListItem();
            firstItem.Text = "_";
            firstItem.Value = "_";
            DropDownList_Contact_Inv.Items.Add(firstItem);
            DropDownList_Vendor_po.Items.Add(firstItem);

            ArrayList contactList = BackEndObjects.Contacts.
                getAllContactsbyEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

            for (int i = 0; i < contactList.Count; i++)
            {
                ListItem lt = new ListItem();
                lt.Text = ((BackEndObjects.Contacts)contactList[i]).getContactName();
                lt.Value = ((BackEndObjects.Contacts)contactList[i]).getContactEntityId();

                DropDownList_Contact_Inv.Items.Add(lt);
                DropDownList_Vendor_po.Items.Add(lt);
            }

            DropDownList_Contact_Inv.SelectedValue = "_";
            DropDownList_Vendor_po.SelectedValue = "_";
        }

        protected void disableOnPageChange(String senderName)
        {
            switch (senderName)
            {
                case "rfq": Button_Audit_RFQ.Enabled = false;
                    Button_RFQ_Doc.Enabled = false;
                    Button_Notes_RFQ.Enabled = false;
                                Button_Create_Clone_RFQ.Enabled = false;
            Button_Tag_To_Req.Enabled = false;
            Label_RFQ_Grid_Access.Visible = false;
            Button_Workflow_Tree.Enabled = false;
                    break;
                case "req": Button_Audit_Req.Enabled = false;
                    Button_Notes.Enabled = false;
                                Button_Create_Clone.Enabled = false;
            Button_Convert_Req_To_RFQ.Enabled = false;
            Label_Reqr_Grid_Access.Visible = false;
                    break;

                case "inv": Button_Notes_Inv.Enabled = false;break;

                case "po": Button_Notes_PO.Enabled = false;
                    break;
            }
        }
        /// <summary>
        /// This method will check access to different buttons and enable/disable based on access
        /// </summary>
        protected void CheckAccessToActions()
        {
                        Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];
                        if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                        {
                            Button_Create_Req.Enabled = true;
                            //Button_Convert_Req_To_RFQ.Enabled = true;
                            Button_Create_Req0.Enabled = true;
                            Button_Create_PO.Enabled = true;
                        }
                        else
                        {
                            if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_CREATE_REQUIREMENT])
                                Button_Create_Req.Enabled = false;
                            if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_CONVERT_REQUIREMENT])
                                Button_Convert_Req_To_RFQ.Enabled = false;
                            if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_CREATE_RFQ])
                                Button_Create_Req0.Enabled = false;
                            if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_CREATE_PO_PURCHASE])
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
                ((System.Web.UI.WebControls.Image)Master.FindControl("Image_Logo")).ImageUrl =imageToURL(imgObj.getImgPath());
                ((System.Web.UI.WebControls.Image)Master.FindControl("Image_Logo")).Visible = true;
                
            }
        }

        public String generateImagePath(String folderName)
        {
            String imgStoreRoot = "~/Images/SessionImages";
            imgStoreRoot=Server.MapPath(imgStoreRoot);

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
            
            if(!File.Exists(Server.MapPath(finalImageUrl)))
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

        /// <summary>
        /// This method will be called on page load
        /// </summary>
        protected void fillGridsOnPageLoad()
        {
            //Populate the all Requirement grid

            ArrayList reqList=BackEndObjects.Requirement.getAllRequirementsbyEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            //ArrayList rfqList = BackEndObjects.RFQDetails.getAllRFQbyEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(),false);

            ArrayList invList = BackEndObjects.Invoice.getAllInvoicesbyEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            /*for (int i = 0; i < rfqList.Count; i++)
            {
                Invoice invObj = BackEndObjects.Invoice.getInvoicebyRfIdDB(((RFQDetails)rfqList[i]).getRFQId());
                if(invObj!=null && invObj.getInvoiceId()!=null && !invObj.getInvoiceId().Equals(""))
                invList.Add(invObj);
            }*/
            //Now get the RFQ list with only for those RFQs which are created by this entity id
            ArrayList rfqList = BackEndObjects.RFQDetails.getAllRFQbyEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), true);

            ArrayList rfqListPO = BackEndObjects.RFQDetails.getAllRFQIncludingDummybyEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), true);
            
            if(reqList.Count>0)
            fillReqrGrid(reqList);

            if(rfqList.Count>0)
            fillRFQGrid(rfqList);

            if(invList.Count>0)
            fillInvoiceGrid(invList);

            if (rfqListPO.Count > 0)
                fillPOGrid(rfqListPO);
        }
        protected void fillRFQGrid(ArrayList rfqL)
        {
            Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];
            Dictionary<String, Currency> allCurrList = (Dictionary<String, Currency>)Session[SessionFactory.CURRENCY_LIST];

            if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_RFQ]||
                accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
            {
                Label_RFQ_Grid_Access.Visible = false;

                DataTable dt = new DataTable();
                dt.Columns.Add("RFQNo");
                dt.Columns.Add("RFQName");
                dt.Columns.Add("curr");
                dt.Columns.Add("Specifications");
                dt.Columns.Add("Submit Date");
                dt.Columns.Add("Submit Date Ticks");
                dt.Columns.Add("Due Date");
                dt.Columns.Add("Due Date Ticks");
                dt.Columns.Add("Location");
                dt.Columns.Add("ApprovalStat");
                dt.Columns.Add("StatReason");
                dt.Columns.Add("AllQuotes");
                dt.Columns.Add("AllShortlisted");
                dt.Columns.Add("PO_No");
                dt.Columns.Add("BroadcastTo");
                dt.Columns.Add("AllAuditRecords");
                dt.Columns.Add("ActiveStatus");
                dt.Columns.Add("Inv_No");
                //dt.Columns.Add("Doc");
                dt.Columns.Add("Hidden_Doc_Name");

                DateUtility dU = new DateUtility();

                ArrayList rfqList = new ArrayList();
                for (int i = 0; i < rfqL.Count; i++)
                {
                   if(!(((BackEndObjects.RFQDetails)rfqL[i]).getRFQId().IndexOf("dummy")>=0))
                    rfqList.Add(((BackEndObjects.RFQDetails)rfqL[i]).getRFQId());
                }
                Dictionary<String, String> poDict =rfqList!=null && rfqList.Count>0?
                    PurchaseOrder.getPurchaseOrdersforRFQIdListDB(rfqList): new  Dictionary<String, String>();
                
                int rowCount = 0;
                for (int i = 0; i < rfqL.Count; i++)
                {
                    if (!(((BackEndObjects.RFQDetails)rfqL[i]).getRFQId().IndexOf("dummy") >= 0))
                    {
                        dt.Rows.Add();
                        String rfId = ((BackEndObjects.RFQDetails)rfqL[i]).getRFQId();
                        //String poId = BackEndObjects.PurchaseOrder.getPurchaseOrderforRFQIdDB(rfId).getPo_id();
                        String poId = poDict.ContainsKey(rfId) ? poDict[rfId] : "";

                        String docName = "";
                        if (((BackEndObjects.RFQDetails)rfqL[i]).getNDADocPath() != null)
                        {
                            String[] docPathList = ((BackEndObjects.RFQDetails)rfqL[i]).getNDADocPath().
                                Split(new String[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                            if (docPathList.Length > 0)
                                docName = docPathList[docPathList.Length - 1];
                        }

                        dt.Rows[rowCount]["RFQNo"] = rfId;
                        dt.Rows[rowCount]["RFQName"] = ((BackEndObjects.RFQDetails)rfqL[i]).getRFQName();
                        dt.Rows[rowCount]["curr"] = ((BackEndObjects.RFQDetails)rfqL[i]).getCurrency() != null &&
                            allCurrList.ContainsKey(((BackEndObjects.RFQDetails)rfqL[i]).getCurrency()) ?
                                       allCurrList[((BackEndObjects.RFQDetails)rfqL[i]).getCurrency()].getCurrencyName() : "";
                        dt.Rows[rowCount]["Submit Date"] = dU.getConvertedDate(((BackEndObjects.RFQDetails)rfqL[i]).getSubmitDate());
                        dt.Rows[rowCount]["Submit Date Ticks"] = Convert.ToDateTime(((BackEndObjects.RFQDetails)rfqL[i]).getSubmitDate()).Ticks;

                        dt.Rows[rowCount]["Due Date"] = dU.getConvertedDate(((BackEndObjects.RFQDetails)rfqL[i]).getDueDate().Substring(0, ((BackEndObjects.RFQDetails)rfqL[i]).getDueDate().IndexOf(" ")));
                        dt.Rows[rowCount]["Due Date Ticks"] = !dt.Rows[rowCount]["Due Date"].Equals("") ? Convert.ToDateTime(dt.Rows[rowCount]["Due Date"]).Ticks : 0;
                        dt.Rows[rowCount]["ApprovalStat"] = ((BackEndObjects.RFQDetails)rfqL[i]).getApprovalStat();
                        dt.Rows[rowCount]["Po_No"] = (poId == null || poId.Equals("") ? "N/A" : poId);

                        //dt.Rows[rowCount]["Doc"] = (docName == null || docName.Equals("") ? "N/A" : "Show");
                        dt.Rows[rowCount]["Hidden_Doc_Name"] = (docName == null || docName.Equals("") ? "" : docName);
                        String activeStat = ((BackEndObjects.RFQDetails)rfqL[i]).getActiveStat();

                        dt.Rows[rowCount]["ActiveStatus"] = activeStat;

                        if (activeStat.Equals(BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_DEAL_CLOSED))
                        {
                            Invoice invObj = BackEndObjects.Invoice.getInvoicebyRfIdDB(rfId);
                            if (invObj != null && invObj.getInvoiceId() != null && !invObj.getInvoiceId().Equals(""))
                                dt.Rows[rowCount]["Inv_No"] = "Show";
                        }
                        else
                            dt.Rows[rowCount]["Inv_No"] = "N/A";

                        rowCount++;
                    }
                }
                dt.DefaultView.Sort = "Submit Date Ticks" + " " + "DESC";
                disableOnPageChange("rfq");                
                GridView_RFQ.SelectedIndex = -1;
                GridView_RFQ.DataSource = dt;
                GridView_RFQ.DataBind();
                GridView_RFQ.Visible = true;
                Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_GRID_DATA] = dt.DefaultView.ToTable();

                if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_RFQ] &&
    !accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                    GridView_RFQ.Columns[1].Visible = false; //Hide edit option if no edit access


                foreach (GridViewRow gVR in GridView_RFQ.Rows)
                {
                    if (((LinkButton)gVR.Cells[0].FindControl("LinkButton_Show_Inv")).Text.Equals("N/A"))
                        ((LinkButton)gVR.Cells[0].FindControl("LinkButton_Show_Inv")).Enabled = false;
                    if (((LinkButton)gVR.Cells[0].FindControl("LinkButton_PO")).Text.Equals("N/A"))
                        ((LinkButton)gVR.Cells[0].FindControl("LinkButton_PO")).Enabled = false;
                    //if (((LinkButton)gVR.Cells[0].FindControl("LinkButton_Doc")).Text.Equals("N/A"))
                        //((LinkButton)gVR.Cells[0].FindControl("LinkButton_Doc")).Enabled = false;
                }
            }
            else
            {
                Label_RFQ_Grid_Access.Visible = true;
                Label_RFQ_Grid_Access.Text = "You don't have access to view this Section";
            }
        }
        protected void fillReqrGrid(ArrayList aL)
        {
                        Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];
                        Dictionary<String, Currency> allCurrList = (Dictionary<String, Currency>)Session[SessionFactory.CURRENCY_LIST];

                        if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_REQUIREMENT]||
                            accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                        {
                            Label_Reqr_Grid_Access.Visible = false;

                            DataTable dt = new DataTable();
                            dt.Columns.Add("Hidden");
                            dt.Columns.Add("Requirement Name");
                            dt.Columns.Add("Specifications");
                            dt.Columns.Add("curr");
                            dt.Columns.Add("Submit Date");
                            dt.Columns.Add("Submit Date Ticks");
                            dt.Columns.Add("Due Date");
                            dt.Columns.Add("Due Date Ticks");
                            dt.Columns.Add("Location");
                            dt.Columns.Add("Min Quote");
                            dt.Columns.Add("Tagged RFQs");
                            dt.Columns.Add("Created By");
                            dt.Columns.Add("All Audit Records");
                            dt.Columns.Add("Active?");

                            DateUtility du = new DateUtility();

                            for (int i = 0; i < aL.Count; i++)
                            {
                                dt.Rows.Add();
                                dt.Rows[i]["Hidden"] = ((BackEndObjects.Requirement)aL[i]).getReqId();
                                dt.Rows[i]["Requirement Name"] = ((BackEndObjects.Requirement)aL[i]).getReqName();
                                dt.Rows[i]["curr"] = ((BackEndObjects.Requirement)aL[i]).getCurrency()!=null && 
                                    allCurrList.ContainsKey(((BackEndObjects.Requirement)aL[i]).getCurrency()) ? 
                                    allCurrList[((BackEndObjects.Requirement)aL[i]).getCurrency()].getCurrencyName() : "";
                                dt.Rows[i]["Submit Date"] = du.getConvertedDate(((BackEndObjects.Requirement)aL[i]).getSubmitDate());
                                dt.Rows[i]["Submit Date Ticks"] = Convert.ToDateTime(((BackEndObjects.Requirement)aL[i]).getSubmitDate()).Ticks;
                                dt.Rows[i]["Due Date"] =du.getConvertedDate(((BackEndObjects.Requirement)aL[i]).getDueDate().Substring(0,((BackEndObjects.Requirement)aL[i]).getDueDate().IndexOf(" ")));
                                dt.Rows[i]["Due Date Ticks"] = !dt.Rows[i]["Due Date"].Equals("")?Convert.ToDateTime(dt.Rows[i]["Due Date"]).Ticks:0;
                                dt.Rows[i]["Created By"] = ((BackEndObjects.Requirement)aL[i]).getCratedUsr();
                                dt.Rows[i]["Active?"] = ((BackEndObjects.Requirement)aL[i]).getActiveStat();

                            }
                            dt.DefaultView.Sort = "Submit Date Ticks" + " " + "DESC";
                            disableOnPageChange("req");
                            GridView1.SelectedIndex = -1;
                            GridView1.DataSource = dt;
                            GridView1.DataBind();
                            GridView1.Columns[2].Visible = false;
                            GridView1.Visible = true;

                            if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_REQUIREMENT] &&
    !accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                                GridView1.Columns[1].Visible = false;

                            Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_GRID_DATA] = dt.DefaultView.ToTable();
                        }
                        else
                        {
                            Label_Reqr_Grid_Access.Visible = true;
                            Label_Reqr_Grid_Access.Text = "You don't have access to view this section";
                        }
        }
        protected void fillInvoiceGrid(ArrayList invL)
        {
                                    Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];
                                    Dictionary<String, Currency> allCurrList = (Dictionary<String, Currency>)Session[SessionFactory.CURRENCY_LIST];

                                    if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_INVOICE_PURCHASE]||
                                        accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                                    {
                                        DataTable dt = new DataTable();
                                        dt.Columns.Add("RFQNo");
                                        dt.Columns.Add("Inv_No");
                                        dt.Columns.Add("Inv_Id");
                                        dt.Columns.Add("Inv_Date");
                                        dt.Columns.Add("Inv_Date_Ticks");
                                        dt.Columns.Add("Deliv_Stat");
                                        dt.Columns.Add("Pmnt_Stat");
                                        dt.Columns.Add("Amount");
                                        dt.Columns.Add("curr");
                                        dt.Columns.Add("Related_PO");

                                        DateUtility du = new DateUtility();

                                        for (int i = 0; i < invL.Count; i++)
                                        {
                                            dt.Rows.Add();

                                            dt.Rows[i]["RFQNo"] = ((BackEndObjects.Invoice)invL[i]).getRFQId();

                                            String invNo = ((BackEndObjects.Invoice)invL[i]).getInvoiceNo();
                                            String invId = ((BackEndObjects.Invoice)invL[i]).getInvoiceId();
                                            dt.Rows[i]["Inv_No"] = invId.Equals(invNo) ? invId : invNo;
                                            dt.Rows[i]["Inv_Id"] = invId;
                                            dt.Rows[i]["Inv_Date"] = du.getConvertedDate(((BackEndObjects.Invoice)invL[i]).getInvoiceDate().Substring(0, ((BackEndObjects.Invoice)invL[i]).getInvoiceDate().IndexOf(" ")));
                                            dt.Rows[i]["Inv_Date_Ticks"] = !dt.Rows[i]["Inv_Date"].Equals("")?Convert.ToDateTime(((BackEndObjects.Invoice)invL[i]).getInvoiceDate()).Ticks:0;
                                            dt.Rows[i]["Deliv_Stat"] = ((BackEndObjects.Invoice)invL[i]).getDeliveryStatus();
                                            dt.Rows[i]["Pmnt_Stat"] = ((BackEndObjects.Invoice)invL[i]).getPaymentStatus();
                                            dt.Rows[i]["Amount"] = ((BackEndObjects.Invoice)invL[i]).getTotalAmount();
                                            dt.Rows[i]["curr"] = ((BackEndObjects.Invoice)invL[i]).getCurrency() != null &&
                        allCurrList.ContainsKey(((BackEndObjects.Invoice)invL[i]).getCurrency()) ?
                                   allCurrList[((BackEndObjects.Invoice)invL[i]).getCurrency()].getCurrencyName() : "";
                                            dt.Rows[i]["Related_PO"] = ((BackEndObjects.Invoice)invL[i]).getRelatedPO();
                                        }
                                        dt.DefaultView.Sort = "Inv_Date_Ticks" + " " + "DESC";
                                        disableOnPageChange("inv");
                                        GridView_Invoice.SelectedIndex = -1;
                                        GridView_Invoice.DataSource = dt;
                                        GridView_Invoice.DataBind();
                                        GridView_Invoice.Visible = true;

                                        if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_INVOICE_PURCHASE] &&
    !accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                                            GridView_Invoice.Columns[1].Visible = false;

                                        Session[SessionFactory.ALL_PURCHASE_ALL_INV_GRID_DATA] = dt.DefaultView.ToTable();
                                    }
                                    else
                                    {
                                        Label_Inv_Grid_Access.Visible = true;
                                        Label_Inv_Grid_Access.Text = "You don't have access to view this section";
                                    }
        }
        protected void fillPOGrid(ArrayList rfqL)
        {
            Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];
            Dictionary<String, Currency> allCurrList = (Dictionary<String, Currency>)Session[SessionFactory.CURRENCY_LIST];

            if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_PO_PURCHASE] ||
                accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
            {
                Label_PO_Grid_Access.Visible = false;

                DataTable dt = new DataTable();
                dt.Columns.Add("RFQNo");
                dt.Columns.Add("curr");
                dt.Columns.Add("PO_No");
                dt.Columns.Add("PO_Date");
                dt.Columns.Add("Amount");//
                dt.Columns.Add("PO_Date_Ticks");

                ArrayList rfqList = new ArrayList();
                ArrayList poList = new ArrayList();
                DateUtility dU = new DateUtility();

                for (int i = 0; i < rfqL.Count; i++)
                {
                    rfqList.Add(rfqL[i]);
                }
                Dictionary<String, String> poDict = rfqList != null && rfqList.Count > 0 ?
                    PurchaseOrder.getPurchaseOrdersforRFQIdListDB(rfqList) : new Dictionary<String, String>();

                String[] poArray=poDict.Values.ToArray<String>();
                for (int i = 0; i < poArray.Length; i++)
                {
                    poList.Add(poArray[i]);
                }
                Dictionary<String, PurchaseOrder> poObjDict = PurchaseOrder.getPurchaseOrdersforPOIdListDB(poList);

                int rowCount = 0;
                foreach (KeyValuePair<String, PurchaseOrder> kvp in poObjDict)
                {
                    dt.Rows.Add();
                    PurchaseOrder poObj=kvp.Value;
                    dt.Rows[rowCount]["RFQNo"] =poObj.getRfq_id();
                    dt.Rows[rowCount]["curr"] = poObj.getCurrency() != null &&
    allCurrList.ContainsKey(poObj.getCurrency()) ?
               allCurrList[poObj.getCurrency()].getCurrencyName() : "";
                    dt.Rows[rowCount]["PO_Date"] = dU.getConvertedDate(poObj.getDate_created());
                    dt.Rows[rowCount]["Po_No"] = poObj.getPo_id();
                    dt.Rows[rowCount]["Amount"] = poObj.getAmount();
                    dt.Rows[rowCount]["PO_Date_Ticks"] = Convert.ToDateTime(poObj.getDate_created()).Ticks;

                    rowCount++;
                }

                dt.DefaultView.Sort = "PO_Date_Ticks" + " " + "DESC";
                disableOnPageChange("po");
                GridView_PO.SelectedIndex = -1;
                GridView_PO.DataSource = dt;
                GridView_PO.DataBind();
                GridView_PO.Visible = true;
                Session[SessionFactory.ALL_PURCHASE_ALL_PO_GRID_DATA] = dt.DefaultView.ToTable();

                if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_PO_PURCHASE] &&
    !accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                    GridView_PO.Columns[1].Visible = false; //Hide edit option if no edit access

            }
            else
            {
                Label_PO_Grid_Access.Visible = true;
                Label_PO_Grid_Access.Text = "You don't have access to view this Section";
            }
        }
    
        /// <summary>
        /// Detects the filter conditions and fills the requirement list grid accordingly
        /// </summary>
        protected void fillReqrGridwithFilter()
        {
                        Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

                        if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_REQUIREMENT]||
                            accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                        {
                            String prodCatId = DropDownList_Category.SelectedValue;
                            String activeStat = DropDownList_Status.SelectedValue;
                            String fromDate = TextBox_From_Date.Text;
                            String toDate = TextBox_To_Date.Text;
                            String reqName = TextBox_Req_Name.Text;

                            ActionLibrary.PurchaseActions._dispRequirements dReq = new ActionLibrary.PurchaseActions._dispRequirements();

                            Dictionary<String, String> filterParams = new Dictionary<string, string>();
                            if (!reqName.Trim().Equals(""))
                                filterParams.Add(dReq.FILTER_BY_REQUIREMENT_NAME, reqName.Trim());
                            if (prodCatId != null && !prodCatId.Equals("") && !prodCatId.Equals("_"))
                                filterParams.Add(dReq.FILTER_BY_PROD_CAT, prodCatId);
                            if (activeStat != null && !activeStat.Equals("") && !activeStat.Equals("_"))
                                filterParams.Add(dReq.FILTER_BY_STATUS, activeStat);
                            if (fromDate != null && !fromDate.Equals(""))
                                filterParams.Add(dReq.FILTER_BY_DUE_DATE_FROM, fromDate);
                            if (toDate != null && !toDate.Equals(""))
                                filterParams.Add(dReq.FILTER_BY_DUE_DATE_TO, toDate);

                            fillReqrGrid(dReq.getAllRequirementsFiltered(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), User.Identity.Name, filterParams));
                        }
        }

        protected void fillRFQGridwithFilter()
        {
            Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

            if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_RFQ]||
                accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
            {
                String prodCatId = DropDownList_Category_RFQ.SelectedValue;
                String activeStat = DropDownList_RFQ_Active_Stat.SelectedValue;
                String fromDate = TextBox_From_Date_RFQ.Text;
                String toDate = TextBox_To_Date_RFQ.Text;
                String rfqNo = TextBox_RFQ_No_RFQ.Text;

                ActionLibrary.PurchaseActions._dispRFQDetails dRfq = new ActionLibrary.PurchaseActions._dispRFQDetails();

                Dictionary<String, String> filterParams = new Dictionary<string, string>();
                if (!rfqNo.Trim().Equals(""))
                    filterParams.Add(dRfq.FILTER_BY_RFQ_NO, rfqNo);
                if (prodCatId != null && !prodCatId.Equals("") && !prodCatId.Equals("_"))
                    filterParams.Add(dRfq.FILTER_BY_PROD_CAT, prodCatId);
                if (activeStat != null && !activeStat.Equals("") && !activeStat.Equals("_"))
                    filterParams.Add(dRfq.FILTER_BY_ACTIVE_STATUS, activeStat);
                if (fromDate != null && !fromDate.Equals(""))
                    filterParams.Add(dRfq.FILTER_BY_DUE_DATE_FROM, fromDate);
                if (toDate != null && !toDate.Equals(""))
                    filterParams.Add(dRfq.FILTER_BY_DUE_DATE_TO, toDate);

                fillRFQGrid(dRfq.getAllRFQDetailsFiltered(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), User.Identity.Name, filterParams));
            }
        }

        protected void fillPOGridwithFilter()
        {
            Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

            if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_PO_PURCHASE] ||
                accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
            {
                String poId = TextBox_po.Text;
                String vendor = DropDownList_Vendor_po.SelectedValue;
                String fromDate = TextBox_From_Date_PO.Text;
                String toDate = TextBox_To_Date_PO.Text;
                String rfqNo = TextBox_rfq_no_po.Text;

                ActionLibrary.PurchaseActions._dispPODetails dPo = new ActionLibrary.PurchaseActions._dispPODetails();

                Dictionary<String, String> filterParams = new Dictionary<string, string>();
                if (!rfqNo.Trim().Equals(""))
                    filterParams.Add(dPo.FILTER_BY_RFQ_NO, rfqNo);
                if (!poId.Trim().Equals(""))
                    filterParams.Add(dPo.FILTER_BY_PO_NO, poId);
                if (!vendor.Trim().Equals("_"))
                    filterParams.Add(dPo.FILTER_BY_VENDOR, vendor);
                if (fromDate != null && !fromDate.Equals(""))
                    filterParams.Add(dPo.FILTER_BY_SUBMIT_DATE_FROM, fromDate);
                if (toDate != null && !toDate.Equals(""))
                    filterParams.Add(dPo.FILTER_BY_SUBMIT_DATE_TO, toDate);

                ArrayList poFilteredList=dPo.getAllPODetailsFiltered(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), User.Identity.Name, filterParams);
                DataTable dt = new DataTable();
                dt.Columns.Add("RFQNo");
                dt.Columns.Add("curr");
                dt.Columns.Add("PO_No");
                dt.Columns.Add("PO_Date");
                dt.Columns.Add("Amount");//
                dt.Columns.Add("PO_Date_Ticks");

                if (poFilteredList != null && poFilteredList.Count > 0)
                {
                    int rowCount = 0;
                    DateUtility dU = new DateUtility();

                    Dictionary<String, Currency> allCurrList = (Dictionary<String, Currency>)Session[SessionFactory.CURRENCY_LIST];

                    for (int i = 0; i < poFilteredList.Count; i++)
                    {
                        PurchaseOrder poObj = (PurchaseOrder)poFilteredList[i];

                        dt.Rows.Add();

                        dt.Rows[rowCount]["RFQNo"] = poObj.getRfq_id();
                        dt.Rows[rowCount]["curr"] = poObj.getCurrency() != null &&
                    allCurrList.ContainsKey(poObj.getCurrency()) ?
                     allCurrList[poObj.getCurrency()].getCurrencyName() : "";
                        dt.Rows[rowCount]["PO_Date"] = dU.getConvertedDate(poObj.getDate_created());
                        dt.Rows[rowCount]["Po_No"] = poObj.getPo_id();
                        dt.Rows[rowCount]["Amount"] = poObj.getAmount();
                        dt.Rows[rowCount]["PO_Date_Ticks"] = Convert.ToDateTime(poObj.getDate_created()).Ticks;

                        rowCount++;
                    }
                }

                dt.DefaultView.Sort = "PO_Date_Ticks" + " " + "DESC";
                disableOnPageChange("po");
                GridView_PO.SelectedIndex = -1;
                GridView_PO.DataSource = dt;
                GridView_PO.DataBind();
                GridView_PO.Visible = true;
                Session[SessionFactory.ALL_PURCHASE_ALL_PO_GRID_DATA] = dt.DefaultView.ToTable();

                    
                

            }
        }

        protected void fillInvGridwithFilter()
        {
                        Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

                        if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_INVOICE_PURCHASE]||
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
                            String supplier = DropDownList_Contact_Inv.SelectedValue;

                            ActionLibrary.PurchaseActions._dispInvoiceDetails dInv = new ActionLibrary.PurchaseActions._dispInvoiceDetails();

                            Dictionary<String, String> filterParams = new Dictionary<string, string>();

                            if (rfqNo != null && !rfqNo.Equals(""))
                                filterParams.Add(dInv.FILTER_BY_RFQ_NO, rfqNo);
                            if (invNo != null && !invNo.Equals(""))
                                filterParams.Add(dInv.FILTER_BY_INVOICE_NO, invNo);
                            if (prodCatId != null && !prodCatId.Equals("") && !prodCatId.Equals("_"))
                                filterParams.Add(dInv.FILTER_BY_PRODUCT_CAT, prodCatId);
                            if (delivStat != null && !delivStat.Equals("") && !delivStat.Equals("_"))
                                filterParams.Add(dInv.FILTER_BY_DELIV_STAT, delivStat);
                            if (pmntStat != null && !pmntStat.Equals("") && !pmntStat.Equals("_"))
                                filterParams.Add(dInv.FILTER_BY_PMNT_STAT, pmntStat);
                            if (fromDate != null && !fromDate.Equals(""))
                                filterParams.Add(dInv.FILTER_BY_FROM_DATE, fromDate);
                            if (toDate != null && !toDate.Equals(""))
                                filterParams.Add(dInv.FILTER_BY_TO_DATE, toDate);
                            if (tranNo != null && !tranNo.Equals(""))
                                filterParams.Add(dInv.FILTER_BY_TRAN_NO, tranNo);
                            if (supplier != null && !supplier.Equals("_"))
                                filterParams.Add(dInv.FILTER_BY_SUPPLIER, supplier);
                            /* ArrayList rfqList = BackEndObjects.RFQDetails.
                                 getAllRFQbyEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), false);

                              ArrayList invList =new ArrayList();
                             for (int i = 0; i < rfqList.Count; i++)
                             {
                                 Invoice invObj = BackEndObjects.Invoice.getInvoicebyRfIdDB(((RFQDetails)rfqList[i]).getRFQId());
                                 invList.Add(invObj);
                             }*/

                            ArrayList invList = BackEndObjects.Invoice.getAllInvoicesbyEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                            fillInvoiceGrid(dInv.getAllInvDetailsFiltered(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), invList, filterParams));
                        }   
        }
        protected void LoadProductCat()
        {
            Dictionary<String,ProductCategory> prodDict=BackEndObjects.ProductCategory.getAllParentCategory();
            ListItem firstItem = new ListItem();
            firstItem.Text = "_";
            firstItem.Value = "_";
            foreach (KeyValuePair<String, ProductCategory> kvp in prodDict)
            {
                ListItem ltProd = new ListItem();
                ltProd.Text = ((BackEndObjects.ProductCategory)kvp.Value).getProductCategoryName();
                ltProd.Value = kvp.Key.ToString();
                DropDownList_Category.Items.Add(ltProd);
                DropDownList_Category_RFQ.Items.Add(ltProd);
                DropDownList_Prod_Cat.Items.Add(ltProd);
            }

            DropDownList_Category.Items.Add(firstItem);
            DropDownList_Category_RFQ.Items.Add(firstItem);
            DropDownList_Prod_Cat.Items.Add(firstItem);

            DropDownList_Category.SelectedValue = "_";
            DropDownList_Category_RFQ.SelectedValue = "_";
            DropDownList_Prod_Cat.SelectedValue = "_";
        }

        protected void LoadReqrActiveStat()
        {
            ListItem lt = new ListItem();
            lt.Text = BackEndObjects.Requirement.REQ_ACTIVE_STAT_ACTIVE;
            lt.Value = BackEndObjects.Requirement.REQ_ACTIVE_STAT_ACTIVE;

            ListItem lt1 = new ListItem();
            lt1.Text = BackEndObjects.Requirement.REQ_ACTIVE_STAT_INACTIVE;
            lt1.Value = BackEndObjects.Requirement.REQ_ACTIVE_STAT_INACTIVE;

            ListItem lt2 = new ListItem();
            lt2.Text = "_";
            lt2.Value = "_";

            ListItem ltRFQ1 = new ListItem();
            ltRFQ1.Text = BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_ACTIVE;
            ltRFQ1.Value = BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_ACTIVE;

            ListItem ltRFQ2 = new ListItem();
            ltRFQ2.Text = BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_NONE;
            ltRFQ2.Value = BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_NONE;

            ListItem ltRFQ3 = new ListItem();
            ltRFQ3.Text = BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_NOT_OPEN;
            ltRFQ3.Value = BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_NOT_OPEN;

            ListItem ltRFQ4 = new ListItem();
            ltRFQ4.Text = BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_DEAL_CLOSED;
            ltRFQ4.Value = BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_DEAL_CLOSED;


            DropDownList_Status.Items.Add(lt);
            DropDownList_RFQ_Active_Stat.Items.Add(ltRFQ1);
            DropDownList_Status.Items.Add(lt1);
            DropDownList_Status.Items.Add(lt2);

            DropDownList_RFQ_Active_Stat.Items.Add(ltRFQ2);
            DropDownList_RFQ_Active_Stat.Items.Add(ltRFQ3);
            DropDownList_RFQ_Active_Stat.Items.Add(ltRFQ4);

            DropDownList_Status.SelectedValue = "_";
            DropDownList_RFQ_Active_Stat.SelectedValue = BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_NONE;
            
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
                                    /*Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

                                    if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_REQUIREMENT]&&
                                        !accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                                    {
                                        GridView1.EditIndex = -1;
                                        GridView1.DataSource = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_GRID_DATA];                                      
                                        GridView1.DataBind();
                                        Label_Reqr_Grid_Access.Visible = true;
                                        Label_Reqr_Grid_Access.Text = "You dont have edit access to requirement records";
                                    }
                                    else
                                    {*/
                                        GridView1.EditIndex = e.NewEditIndex;
                                        GridView1.DataSource = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_GRID_DATA];
                                        GridView1.DataBind();
                                        
                                   // }
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DataTable dt = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_GRID_DATA];
            GridViewRow gVR = GridView1.Rows[e.RowIndex];
            //dt.DefaultView.Sort = "Submit Date" + " " + "DESC";
            
            int index = GridView1.Rows[e.RowIndex].DataItemIndex;

            String reqrName=((TextBox)gVR.FindControl("TextBox_Reqr_Name_Edit")).Text;
            dt.Rows[index]["Requirement Name"] = reqrName;
            String dueDate=((TextBox)gVR.FindControl("TextBox_DueDate")).Text;
            dt.Rows[index]["Due Date"] = dueDate.Replace("00", "").Replace(":", "");
            String activeStat=((DropDownList)gVR.FindControl("DropDownList_Reqr_Active")).SelectedItem.Text;
            dt.Rows[index]["Active?"] = activeStat;
            String curr = ((DropDownList)gVR.FindControl("DropDownList_Curr")).SelectedItem.Text;
            dt.Rows[index]["curr"] = curr;
            //dt.DefaultView.Sort = "Submit Date" + " " + "DESC";
            GridView1.EditIndex = -1;
            GridView1.DataSource = dt;
            GridView1.DataBind();

            Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_GRID_DATA] = dt;

            try
            {
                String reqrId = ((Label)gVR.FindControl("Label_Hidden")).Text;

                Dictionary<String, String> whereCls = new Dictionary<string, string>();
                whereCls.Add(BackEndObjects.Requirement.REQ_COL_REQ_ID, reqrId);

                Dictionary<String, String> targetVals = new Dictionary<string, string>();
                targetVals.Add(BackEndObjects.Requirement.REQ_COL_REQ_NAME, reqrName);
                targetVals.Add(BackEndObjects.Requirement.REQ_COL_DUE_DATE, dueDate);
                targetVals.Add(BackEndObjects.Requirement.REQ_COL_ACTIVE_STAT, activeStat);
                targetVals.Add(BackEndObjects.Requirement.REQ_COL_CURRENCY, ((DropDownList)gVR.FindControl("DropDownList_Curr")).SelectedValue);

                BackEndObjects.Requirement.updateRequirementDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
            }
            catch (Exception ex)
            {
            }
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                GridViewRow gVR = e.Row;

                //DropDownList_Pot_Stage
                //DropDownList_Pot_Active
                ListItem lt = new ListItem();
                lt.Text = BackEndObjects.Requirement.REQ_ACTIVE_STAT_ACTIVE;
                lt.Value = BackEndObjects.Requirement.REQ_ACTIVE_STAT_ACTIVE;
                ((DropDownList)gVR.FindControl("DropDownList_Reqr_Active")).Items.Add(lt);

                lt = new ListItem();
                lt.Text = BackEndObjects.Requirement.REQ_ACTIVE_STAT_INACTIVE;
                lt.Value = BackEndObjects.Requirement.REQ_ACTIVE_STAT_INACTIVE;
                ((DropDownList)gVR.FindControl("DropDownList_Reqr_Active")).Items.Add(lt);

                String existingVal = ((Label)gVR.FindControl("Label_Active_Edit")).Text;


                ((DropDownList)gVR.FindControl("DropDownList_Reqr_Active")).SelectedIndex = (existingVal.Equals(BackEndObjects.Requirement.REQ_ACTIVE_STAT_ACTIVE) ? 0 : 1);

                loadCurrency(((DropDownList)gVR.FindControl("DropDownList_Curr")), ((Label)gVR.FindControl("Label_Curr_Edit")).Text);
            }

            /*else  if (e.Row.RowType == DataControlRowType.DataRow)
    {
        // Hiding the Select Button Cells showing for each Data Row. 
       // e.Row.Cells[0].Style.Add(HtmlTextWriterStyle.Display, "none");

        // Attaching one onclick event for the entire row, so that it will
        // fire SelectedIndexChanged, while we click anywhere on the row.
        e.Row.Attributes["onclick"] =  ClientScript.GetPostBackClientHyperlink(GridView1, "Select$" + e.Row.RowIndex);
    }*/


        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataSource = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_GRID_DATA];
            GridView1.DataBind();
            GridView1.Columns[2].Visible = false;
            GridView1.Visible = true;
            GridView1.SelectedIndex = -1;
            //fillGrid();
            disableOnPageChange("req");
        }

        protected void DropDownList_Category_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void DropDownList_Status_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void Button_Filter_All_Reqr_Click(object sender, EventArgs e)
        {
            fillReqrGridwithFilter();
            Button_Create_Req.Focus();
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            String selectedReqrId = ((Label)GridView1.SelectedRow.Cells[2].FindControl("Label_Hidden")).Text;
            ((RadioButton)GridView1.SelectedRow.Cells[0].FindControl("reqr_radio")).Checked = true;

            Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];
            if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS] ||
                accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_CONVERT_REQUIREMENT])
                Button_Convert_Req_To_RFQ.Enabled = true;

            if (Button_Create_Req.Enabled)//Replicate the access rule applied on the create requirement button
            {
                Button_Create_Clone.Enabled = true;
                Label_Reqr_Grid_Access.Visible = false;
            }
            else
            {
                Label_Reqr_Grid_Access.Visible = true;
                Label_Reqr_Grid_Access.Text = "You do not have access to create/clone Requirement";
            }

            enableOnSelect("req");
            BackEndObjects.Requirement reqrObj = BackEndObjects.Requirement.getRequirementbyIdDB(selectedReqrId);

            Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_SELECTED_REQR_ID] = reqrObj.getReqId();
            Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_LOCATION] = reqrObj.getLocalId();
            Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_REQR_SPEC] = reqrObj.getReqSpecs();
            Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_REQR_PROD_SRV] = reqrObj.getReqProdSrvQnty();

        }
        
        protected void GridView1_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {

        }

        protected void Button_Create_Req_Click(object sender, EventArgs e)
        {
            String forwardString = "CreateReq.aspx";
            //forwardString += "?parentContext=" + "lead";
            //Server.Transfer("createContact.aspx",true);

            ScriptManager.RegisterStartupScript(this, typeof(string), "createReq", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=1000,Height=1000,left=100,right=500');", true);

        }

        protected void Button_Filter_All_RFQ_Click(object sender, EventArgs e)
        {
            fillRFQGridwithFilter();
            Button_Create_Req0.Focus();
        }

        protected void GridView_RFQ_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_RFQ.PageIndex = e.NewPageIndex;
            GridView_RFQ.DataSource = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_GRID_DATA];
            GridView_RFQ.DataBind();
            //GridView_RFQ.Columns[2].Visible = false;
            GridView_RFQ.Visible = true;
            GridView_RFQ.SelectedIndex = -1;

            disableOnPageChange("rfq");

            foreach (GridViewRow gVR in GridView_RFQ.Rows)
            {
                if (((LinkButton)gVR.Cells[0].FindControl("LinkButton_Show_Inv")).Text.Equals("N/A"))
                    ((LinkButton)gVR.Cells[0].FindControl("LinkButton_Show_Inv")).Enabled = false;
                if (((LinkButton)gVR.Cells[0].FindControl("LinkButton_PO")).Text.Equals("N/A"))
                    ((LinkButton)gVR.Cells[0].FindControl("LinkButton_PO")).Enabled = false;
                //if (((LinkButton)gVR.Cells[0].FindControl("LinkButton_Doc")).Text.Equals("N/A"))
                    //((LinkButton)gVR.Cells[0].FindControl("LinkButton_Doc")).Enabled = false;
            }
        }

        protected void GridView_RFQ_SelectedIndexChanged(object sender, EventArgs e)
        {
            String selectedRfqId = ((Label)GridView_RFQ.SelectedRow.Cells[2].FindControl("Label_RFQId")).Text;
            BackEndObjects.RFQDetails rfqObj = BackEndObjects.RFQDetails.getRFQDetailsbyIdDB(selectedRfqId);

            ((RadioButton)GridView_RFQ.SelectedRow.Cells[0].FindControl("rfq_radio")).Checked = true;

            Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];
            //Enable the button if the RFQ is not already tagged
            if ((rfqObj.getReqId() == null || rfqObj.getReqId().Equals("")) &&
            (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS] ||
                accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_RFQ]))
                Button_Tag_To_Req.Enabled = true;            
            else
                Button_Tag_To_Req.Enabled = false;

            if (Button_Create_Req0.Enabled) //Replicate the access rule applied on the create RFQ button
            {
                Button_Create_Clone_RFQ.Enabled = true;
                Label_RFQ_Grid_Access.Visible = false;
            }
            else
            {
                Label_RFQ_Grid_Access.Visible = true;
                Label_RFQ_Grid_Access.Text = "You do not have access to create/clone RFQ";
            }

            enableOnSelect("rfq");
            
            Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SELECTED_RFQ_ID] = rfqObj.getRFQId();
            Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_LOCATION] = rfqObj.getLocalityId();
            Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_RFQ_SPEC] = BackEndObjects.RFQProductServiceDetails.getAllProductServiceDetailsbyRFQIdDB(rfqObj.getRFQId());
            Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_PROD_QNTY] = BackEndObjects.RFQProdServQnty.getRFQProductServiceQuantityforRFIdDB(rfqObj.getRFQId());
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GridView1.DataSource = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_GRID_DATA];
            GridView1.DataBind();
        }

        protected void GridView_RFQ_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView_RFQ.EditIndex = -1;
            GridView_RFQ.DataSource = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_GRID_DATA];
            GridView_RFQ.DataBind();
        }

        protected void GridView_RFQ_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DataTable dt = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_GRID_DATA];
            GridViewRow gVR = GridView_RFQ.Rows[e.RowIndex];

            int index = GridView_RFQ.Rows[e.RowIndex].DataItemIndex;

            String rfqName = ((TextBox)gVR.FindControl("TextBox_RFQName_Edit")).Text;
            dt.Rows[index]["RFQName"] = rfqName;
            String dueDate = ((TextBox)gVR.FindControl("TextBox_DueDate")).Text;
            dt.Rows[index]["Due Date"] = dueDate.Replace("00", "").Replace(":", "");

            String activeStat = "",curr="";
            if (!((Label)gVR.FindControl("Label_Active_Edit")).Text.Equals(BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_DEAL_CLOSED))
            activeStat=((DropDownList)gVR.FindControl("DropDownList_Rfq_Active")).SelectedItem.Text;
            dt.Rows[index]["ActiveStatus"] = activeStat;

            if (!((Label)gVR.FindControl("Label_Active_Edit")).Text.Equals(BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_DEAL_CLOSED))
            {
                curr = ((DropDownList)gVR.FindControl("DropDownList_Curr")).SelectedItem.Text;
                dt.Rows[index]["curr"] = curr;
            }

            GridView_RFQ.EditIndex = -1;
            GridView_RFQ.DataSource = dt;
            GridView_RFQ.DataBind();

            Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_GRID_DATA] = dt;

            try
            {
                String rfId = ((Label)gVR.FindControl("Label_RFQId")).Text;

                Dictionary<String, String> whereCls = new Dictionary<string, string>();
                whereCls.Add(BackEndObjects.RFQDetails.RFQ_COL_RFQ_ID, rfId);

                Dictionary<String, String> targetVals = new Dictionary<string, string>();
                targetVals.Add(BackEndObjects.RFQDetails.RFQ_COL_RFQ_NAME , rfqName);
                targetVals.Add(BackEndObjects.RFQDetails.RFQ_COL_DUE_DATE, dueDate);
                if(!activeStat.Equals(""))
                targetVals.Add(BackEndObjects.RFQDetails.RFQ_COL_ACTIVE_STAT, activeStat);
                if(!curr.Equals(""))
                    targetVals.Add(BackEndObjects.RFQDetails.RFQ_COL_CURRENCY, ((DropDownList)gVR.FindControl("DropDownList_Curr")).SelectedValue);

                BackEndObjects.RFQDetails.updateRFQDetailsDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
            }
            catch (Exception ex)
            {
            }
        }

        protected void GridView_RFQ_RowEditing(object sender, GridViewEditEventArgs e)
        {
            /*Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];
            Label_RFQ_Grid_Access.Visible = false;*/

            /*String uId = User.Identity.Name;
            String approverId = ((LinkButton)GridView_RFQ.Rows[e.NewEditIndex].Cells[0].FindControl("LinkButton_Approval_Stat")).Text;
            Boolean allowedToEdit = uId.Equals(approverId,StringComparison.InvariantCultureIgnoreCase) ? true :
                approverId.Equals(RFQDetails.RFQ_APPROVAL_STAT_APPROVED) ? true : false;*/

           /* if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_RFQ] &&
                !accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
            {
                //GridView_RFQ.EditIndex = -1;
                GridView_RFQ.DataSource = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_GRID_DATA];
                GridView_RFQ.DataBind();
                Label_RFQ_Grid_Access.Visible = true;
                Label_RFQ_Grid_Access.Text = "You dont have edit access to RFQ records";
            }
            else 
            {*/
                GridView_RFQ.EditIndex = e.NewEditIndex;
                GridView_RFQ.DataSource = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_GRID_DATA];
                GridView_RFQ.DataBind();
          //  }
        /*    else
            {
                GridView_RFQ.EditIndex = -1;
                GridView_RFQ.DataSource = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_GRID_DATA];
                GridView_RFQ.DataBind();
                Label_RFQ_Grid_Access.Visible = true;
                Label_RFQ_Grid_Access.Text = "Can not edit the RFQ as it is sent for approval";
            }*/
        }

        protected void GridView_RFQ_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                GridViewRow gVR = e.Row;
                String existingVal = ((Label)gVR.FindControl("Label_Active_Edit")).Text;

                if (!existingVal.Equals(BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_DEAL_CLOSED))
                {
                    //DropDownList_Pot_Stage
                    //DropDownList_Pot_Active
                    ListItem lt = new ListItem();
                    lt.Text = BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_ACTIVE;
                    lt.Value = BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_ACTIVE;
                    ((DropDownList)gVR.FindControl("DropDownList_Rfq_Active")).Items.Add(lt);

                    lt = new ListItem();
                    lt.Text = BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_NONE;
                    lt.Value = BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_NONE;
                    ((DropDownList)gVR.FindControl("DropDownList_Rfq_Active")).Items.Add(lt);

                    lt = new ListItem();
                    lt.Text = BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_NOT_OPEN;
                    lt.Value = BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_NOT_OPEN;
                    ((DropDownList)gVR.FindControl("DropDownList_Rfq_Active")).Items.Add(lt);



                    ((DropDownList)gVR.FindControl("DropDownList_Rfq_Active")).SelectedIndex = (existingVal.Equals(BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_ACTIVE) ? 0
                        : existingVal.Equals(BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_NONE) ? 1 : 2);

                    loadCurrency(((DropDownList)gVR.FindControl("DropDownList_Curr")), ((Label)gVR.FindControl("Label_Curr_Edit")).Text);
                }

                else
                {
                    ((DropDownList)gVR.FindControl("DropDownList_Rfq_Active")).Visible = false;
                    ((DropDownList)gVR.FindControl("DropDownList_Curr")).Visible = false;

                    ((Label)gVR.FindControl("Label_Active_Edit")).Visible = true;
                    ((Label)gVR.FindControl("Label_Curr_Edit")).Visible = true;
                }
            }
            else if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Normal) == DataControlRowState.Normal)
            {
                GridViewRow gVR = e.Row;

                Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];
                           if ((!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_RFQ] &&
!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS]))
                               gVR.Cells[1].Enabled = false;

                Label_RFQ_Grid_Access.Visible = false;

                String uId = User.Identity.Name;
                String approverId = ((LinkButton)gVR.Cells[0].FindControl("LinkButton_Approval_Stat")).Text;
                Boolean allowedToEdit = uId.Equals(approverId, StringComparison.InvariantCultureIgnoreCase) ? true :
                    approverId.Equals(RFQDetails.RFQ_APPROVAL_STAT_APPROVED) ? true : false;

                if (!allowedToEdit)  //No edit access because RFQ is sent for approval
                    gVR.Cells[1].Enabled = false;

                if (approverId.Equals(BackEndObjects.RFQDetails.RFQ_APPROVAL_STAT_APPROVED) || approverId.Equals(BackEndObjects.RFQDetails.RFQ_APPROVAL_STAT_REJECTED))
                {
                    ((LinkButton)gVR.FindControl("LinkButton_Approval_Stat")).Enabled = false;
                    //((Label)gVR.FindControl("Label_Approved")).Visible = true;
                }

            }
        }

        protected void LinkButton_PO_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_RFQ.SelectedIndex)
                GridView_RFQ.SelectRow(row.RowIndex);

            ((RadioButton)GridView_RFQ.SelectedRow.Cells[0].FindControl("rfq_radio")).Checked = true;

            String forwardString = "/Pages/Popups/Purchase/PO_Details.aspx";
                //forwardString += "?rfId=" + Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SELECTED_RFQ_ID].ToString();
                forwardString += "?rfId=" + ((Label)GridView_RFQ.SelectedRow.Cells[0].FindControl("Label_RFQId")).Text;
                forwardString += "&context=" + "client";
                //forwardString += "&poId=" + ((LinkButton)GridView_RFQ.Rows[Convert.ToInt32(e.CommandArgument)].Cells[0].FindControl("LinkButton_PO")).Text;
                forwardString += "&poId=" + ((LinkButton)GridView_RFQ.SelectedRow.Cells[0].FindControl("LinkButton_PO")).Text;

                //
                if (!((LinkButton)GridView_RFQ.SelectedRow.Cells[0].FindControl("LinkButton_Show_Inv")).
                    Text.Equals("show", StringComparison.InvariantCultureIgnoreCase) && GridView_RFQ.Columns[1].Visible)
                    forwardString += "&allowEdit=" + "true";
                //Response.Write("<script type='text/javascript'>alert('" + "Hi" + "');</script>");
                //ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "alert('hi');", true);
                ScriptManager.RegisterStartupScript(this, typeof(string), "DispPO", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=900');", true);

        }


        protected void LinkButton_Show_Inv_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_RFQ.SelectedIndex)
                GridView_RFQ.SelectRow(row.RowIndex);

            ((RadioButton)GridView_RFQ.SelectedRow.Cells[0].FindControl("rfq_radio")).Checked = true;

            String forwardString = "/Pages/Popups/Sale/Inv_Details.aspx";
            forwardString += "?rfId=" + ((Label)GridView_RFQ.SelectedRow.Cells[0].FindControl("Label_RFQId")).Text;


            //BackEndObjects.Invoice invObj = BackEndObjects.Invoice.getInvoicebyRfIdDB(((Label)GridView_RFQ.SelectedRow.Cells[0].FindControl("Label_RFQId")).Text);
            Dictionary<String, Invoice> invDict = BackEndObjects.Invoice.
                getAllInvoicesbyRfIdDB(((Label)GridView_RFQ.SelectedRow.Cells[0].FindControl("Label_RFQId")).Text);
            //forwardString += "&invId=" + invObj.getInvoiceId();
            if (!(invDict.Count > 1))
            {
                forwardString += "&invId=";
                String invId = "";
                foreach (KeyValuePair<String, Invoice> kvp in invDict)
                    invId = kvp.Value.getInvoiceId();

                forwardString += invId;
                forwardString += "&context=" + "clientInvoiceGrid";
                forwardString += "&poId=" + ((LinkButton)GridView_RFQ.SelectedRow.Cells[0].FindControl("LinkButton_PO")).Text;
            }
            else
            {
                forwardString = "/Pages/Popups/Sale/MultipleInvoiceForRFQ.aspx";
                forwardString += "?rfId=" + ((Label)GridView_RFQ.SelectedRow.Cells[0].FindControl("Label_RFQId")).Text;
                forwardString += "&context=" + "clientInvoiceGrid";
                forwardString += "&poId=" + ((LinkButton)GridView_RFQ.SelectedRow.Cells[0].FindControl("LinkButton_PO")).Text;

                Dictionary<String, Dictionary<String, Invoice>> invDictSession = (Dictionary<String, Dictionary<String, Invoice>>)Session[SessionFactory.ALL_SALE_ALL_POTN_MUTLTIPLE_INV_DATA_FOR_RFQ];
                if (invDictSession == null)
                    invDictSession = new Dictionary<string, Dictionary<string, Invoice>>();

                if (!invDictSession.ContainsKey(((Label)GridView_RFQ.SelectedRow.Cells[0].FindControl("Label_RFQId")).Text))
                    invDictSession.Add(((Label)GridView_RFQ.SelectedRow.Cells[0].FindControl("Label_RFQId")).Text, invDict);
                else
                {
                    invDictSession.Remove(((Label)GridView_RFQ.SelectedRow.Cells[0].FindControl("Label_RFQId")).Text);
                    invDictSession.Add(((Label)GridView_RFQ.SelectedRow.Cells[0].FindControl("Label_RFQId")).Text, invDict);
                }

                Session[SessionFactory.ALL_SALE_ALL_POTN_MUTLTIPLE_INV_DATA_FOR_RFQ] = invDictSession;
            }
            //Response.Write("<script type='text/javascript'>alert('" + "Hi" + "');</script>");
            //ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "alert('hi');", true);
            //ScriptManager.RegisterStartupScript(this, typeof(string), "DispInvClient", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=900');", true);
            ScriptManager.RegisterStartupScript(this, typeof(string), "DispInv", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=900,Height=900');", true);
        }

        protected void GridView_Invoice_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_Invoice.SelectedIndex = -1;
            GridView_Invoice.PageIndex = e.NewPageIndex;
            GridView_Invoice.DataSource = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_INV_GRID_DATA];
            GridView_Invoice.DataBind();

            disableOnPageChange("inv");
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
            forwardString += "&context=" + "clientInvoiceGrid";

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

        protected void GridView_Invoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            String selectedRfqId = ((Label)GridView_Invoice.SelectedRow.Cells[2].FindControl("Label_RFQId1")).Text;
            Session[SessionFactory.ALL_PURCHASE_ALL_INV_SELECTED_RFQ_SPEC] = BackEndObjects.RFQProductServiceDetails.
                getAllProductServiceDetailsbyRFQIdDB(selectedRfqId);
            ((RadioButton)GridView_Invoice.SelectedRow.Cells[0].FindControl("inv_radio")).Checked = true;
            enableOnSelect("inv");
        }

        protected void Button_Filter_All_Inv_Click(object sender, EventArgs e)
        {
            fillInvGridwithFilter();
            if (GridView_Invoice.Rows.Count > 0)
                GridView_Invoice.Focus();
            else
                Button_Filter_All_Inv.Focus();
        }

        protected void GridView_Invoice_RowEditing(object sender, GridViewEditEventArgs e)
        {
                        //Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

                        /*if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_INVOICE_PURCHASE] &&
                            !accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                        {
                            Label_Inv_Grid_Access.Visible = true;
                            Label_Inv_Grid_Access.Text = "You dont have edit access to Invoice records";
                            GridView_Invoice.EditIndex = -1;
                            GridView_Invoice.DataSource = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_INV_GRID_DATA];
                            GridView_Invoice.DataBind();
                        }*/
                        //else
                        //{
                            GridView_Invoice.EditIndex = e.NewEditIndex;
                            GridView_Invoice.DataSource = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_INV_GRID_DATA];
                            GridView_Invoice.DataBind();
                        //}
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

            }
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
            forwardString += "&context=" + "client";
            forwardString += "&invId=" + ((Label)GridView_Invoice.SelectedRow.Cells[0].FindControl("Label_Invoice_Id1")).Text;
            //In the purchase screen the user will always see auto created invoice - so the invoice number and id will be same
            forwardString += "&invNo=" + ((Label)GridView_Invoice.SelectedRow.Cells[0].FindControl("Label_Invoice_Id1")).Text;

            //Response.Write("<script type='text/javascript'>alert('" + "Hi" + "');</script>");
            //ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "alert('hi');", true);
            ScriptManager.RegisterStartupScript(this, typeof(string), "DispInvPmntClient", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=900,Height=700');", true);
        }


        /// <summary>
        /// Purchase Invoice grid audit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LinkButton_All_Audit1_Command(object sender, CommandEventArgs e)
        {
            String forwardString = "/Pages/DispAudit.aspx";
            String invId = BackEndObjects.Invoice.
                getInvoicebyNoDB(((Label)GridView_Invoice.SelectedRow.Cells[0].FindControl("Label_Invoice_Id1")).Text).getInvoiceId();
            
            forwardString += "?contextId1=" +invId;
            forwardString += "&contextId2=" + "";
            forwardString += "&contextId3=" + "";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispAuditInv", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=700');", true);
        }

        protected void Button_Convert_Req_To_RFQ_Click(object sender, EventArgs e)
        {
            BackEndObjects.RFQDetails rfqObj = new BackEndObjects.RFQDetails();
            ActionLibrary.PurchaseActions._createRFQ cR = new ActionLibrary.PurchaseActions._createRFQ();

            String selectedReqId = ((Label)GridView1.SelectedRow.Cells[0].FindControl("Label_Hidden")).Text;
            BackEndObjects.Requirement reqObj = BackEndObjects.Requirement.getRequirementbyIdDB(selectedReqId);

            rfqObj.setRFQId(new BackEndObjects.Id().getNewId(BackEndObjects.Id.ID_TYPE_RFQ_STRING));
            rfqObj.setRFQName(reqObj.getReqName());
            rfqObj.setReqId(selectedReqId);
            rfqObj.setLocalityId(reqObj.getLocalId());
            rfqObj.setCreateMode(RFQDetails.CREATION_MODE_AUTO);
            rfqObj.setEntityId(reqObj.getEntityId());
            rfqObj.setCreatedEntity(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            rfqObj.setCreatedUsr(User.Identity.Name);
            rfqObj.setActiveStat(reqObj.getActiveStat().Equals(BackEndObjects.Requirement.REQ_ACTIVE_STAT_ACTIVE)?
                RFQDetails.RFQ_ACTIVE_STAT_ACTIVE:
                reqObj.getActiveStat().Equals(BackEndObjects.Requirement.REQ_ACTIVE_STAT_INACTIVE)?RFQDetails.RFQ_ACTIVE_STAT_NOT_OPEN:"");
            rfqObj.setApprovalStat(BackEndObjects.RFQDetails.RFQ_APPROVAL_STAT_APPROVED);
            rfqObj.setDueDate(reqObj.getDueDate());
            rfqObj.setSubmitDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            rfqObj.setTermsandConds("");
            rfqObj.setCurrency(reqObj.getCurrency());

            ArrayList reqrSpecList = reqObj.getReqSpecs();
            ArrayList rfqSpecList=new ArrayList();
            for(int i=0;i<reqrSpecList.Count;i++)
            {
                BackEndObjects.Requirement_Spec reqrSpecObj = (BackEndObjects.Requirement_Spec)reqrSpecList[i];
                
                BackEndObjects.RFQProductServiceDetails rfqSpecObj = new RFQProductServiceDetails();
                
                rfqSpecObj.setCreatedDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                rfqSpecObj.setCreatedUsr(User.Identity.Name);
                rfqSpecObj.setFeatId(reqrSpecObj.getFeatId());
                rfqSpecObj.setFromSpecId(reqrSpecObj.getFromSpecId());
                rfqSpecObj.setToSpecId(reqrSpecObj.getToSpecId());
                rfqSpecObj.setSpecText(reqrSpecObj.getSpecText());
                rfqSpecObj.setRFQId(rfqObj.getRFQId());
                rfqSpecObj.setPrdCatId(reqrSpecObj.getProdCatId());

                rfqSpecList.Add(rfqSpecObj);
            }

            rfqObj.setRFQProdServList(rfqSpecList);

            ArrayList reqrQntyList = reqObj.getReqProdSrvQnty();
            ArrayList rfqQntyList = new ArrayList();
            for (int i = 0; i < reqrQntyList.Count; i++)
            {
                BackEndObjects.RFQProdServQnty rfqQntyObj = new RFQProdServQnty();
                BackEndObjects.RequirementProdServQnty reqrQntyObj =(BackEndObjects.RequirementProdServQnty) reqrQntyList[i];
                
                rfqQntyObj.setFromPrice(reqrQntyObj.getFromPrice());
                rfqQntyObj.setToPrice(reqrQntyObj.getToPrice());
                rfqQntyObj.setFromQnty(reqrQntyObj.getFromQnty());
                rfqQntyObj.setToQnty(reqrQntyObj.getToQnty());
                rfqQntyObj.setMsrmntUnit(reqrQntyObj.getMsrmntUnit());
                rfqQntyObj.setProdCatId(reqrQntyObj.getProdCatId());
                rfqQntyObj.setRFQId(rfqObj.getRFQId());

                rfqQntyList.Add(rfqQntyObj);
            }

            rfqObj.setRFQProdServQntyList(rfqQntyList);

            try
            {
                //Set the approval details for the RFQ
                int rfqLevel = BackEndObjects.MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()).getRfqApprovalLevel();
                BackEndObjects.Workflow_Action actionObj = null;
                if (rfqLevel > 0)
                {
                    String reportingToUser = BackEndObjects.userDetails.
                        getUserDetailsbyIdDB(User.Identity.Name, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()).getReportsTo();
                    rfqObj.setApprovalStat(reportingToUser);

                    actionObj = new Workflow_Action();
                    actionObj.setEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                    actionObj.setUserId(User.Identity.Name);
                    actionObj.setActionTaken(BackEndObjects.Workflow_Action.WORKFLOW_ACTION_ACTION_TAKEN_SUBMITTED);
                    actionObj.setActionDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    actionObj.setContextId(rfqObj.getRFQId());
                    actionObj.setContextName(BackEndObjects.Workflow_Action.WORKFLOW_ACTION_CONTEXT_NAME_RFQ);
                    actionObj.setComment("");
                }
                else
                    rfqObj.setApprovalStat(RFQDetails.RFQ_APPROVAL_STAT_APPROVED);
                rfqObj.setApprovalLevel("0".ToString());

                BackEndObjects.RFQDetails.insertRFQDetailsDB(rfqObj);
                if (actionObj != null)
                    BackEndObjects.Workflow_Action.insertWorkflowActionObject(actionObj);

                ArrayList rfqList = BackEndObjects.RFQDetails.getAllRFQbyEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), false);
                if(rfqList!=null && rfqList.Count>0)
                fillRFQGrid(rfqList);

                Label_Reqr_Conversion_Stat.Text = "Requirement Converted to RFQ successfully";
                Label_Reqr_Conversion_Stat.Visible = true;
                Label_Reqr_Conversion_Stat.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                Label_Reqr_Conversion_Stat.Text = "Requirement Conversion to RFQ Failed";
                Label_Reqr_Conversion_Stat.Visible = true;
                Label_Reqr_Conversion_Stat.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void Button_Req_Refresh_Click(object sender, EventArgs e)
        {
            ArrayList reqList=BackEndObjects.Requirement.getAllRequirementsbyEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            if(reqList!=null && reqList.Count>0)
            fillReqrGrid(reqList);

            GridView1.SelectedIndex = -1;
            disableOnPageChange("req");
            Button_Req_Refresh.Focus();
        }

        protected void Button_Rfq_Refresh_Click(object sender, EventArgs e)
        {
            ArrayList rfqList = BackEndObjects.RFQDetails.getAllRFQbyEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), true);
            if (rfqList != null && rfqList.Count > 0)
                fillRFQGrid(rfqList);

            GridView_RFQ.SelectedIndex = -1;
            disableOnPageChange("rfq");
            Button_Rfq_Refresh.Focus();
        }

        protected void Button_Inv_Refresh_Click(object sender, EventArgs e)
        {
            ArrayList rfqList = BackEndObjects.RFQDetails.getAllRFQbyEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), false);

            ArrayList invList = new ArrayList();
            for (int i = 0; i < rfqList.Count; i++)
            {
                Invoice invObj = BackEndObjects.Invoice.getInvoicebyRfIdDB(((RFQDetails)rfqList[i]).getRFQId());
                if (invObj != null && invObj.getInvoiceId() != null && !invObj.getInvoiceId().Equals(""))
                    invList.Add(invObj);
            }

            if (invList.Count > 0)
                fillInvoiceGrid(invList);

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
                getInvoicebyNoDB(((Label)GridView_Invoice.SelectedRow.Cells[0].FindControl("Label_Invoice_Id1")).Text).getInvoiceId();

            forwardString += "?contextId1=" + invId;
            forwardString += "&contextId2=" + "client";
            
            ScriptManager.RegisterStartupScript(this, typeof(string), "DispDefectForInvoiceOutgoing", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=900,Height=700');", true);
        }

        protected void Button_Tag_To_Req_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/Popups/Purchase/TagRFQtoReq.aspx";
            String rfqId = Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SELECTED_RFQ_ID].ToString();

            forwardString += "?contextId1=" + rfqId;
            
            ScriptManager.RegisterStartupScript(this, typeof(string), "DispDefectForInvoiceOutgoing", 
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=600,Height=300');", true);

        }

        protected void LinkButton_Tagged_RFQ_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView1.SelectedIndex)
                GridView1.SelectRow(row.RowIndex);

            ((RadioButton)GridView1.SelectedRow.Cells[0].FindControl("reqr_radio")).Checked = true;

            String forwardString = "/Pages/Popups/Purchase/TaggedRFQs.aspx";
            String reqId = ((Label)GridView1.SelectedRow.Cells[0].FindControl("Label_Hidden")).Text;
            
            forwardString += "?contextId1=" + reqId;
            
            ScriptManager.RegisterStartupScript(this, typeof(string), "TaggedRFQForReq",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=900,Height=400');", true);

        }

        protected void Button_Create_Clone_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/createClone.aspx";
            String rfqId = ((Label)GridView1.SelectedRow.Cells[0].FindControl("Label_Hidden")).Text;
            String contextString = ((Label)GridView1.SelectedRow.Cells[0].FindControl("Label_Feature")).Text;

            forwardString += "?contextId1=" + rfqId;
            forwardString += "&context=" + "requirement";
            forwardString += "&contextString=" + contextString;

            ScriptManager.RegisterStartupScript(this, typeof(string), "CloneReq",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=400');", true);
        }

        protected void Button_Create_Clone_RFQ_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/createClone.aspx";
            String rfqId = ((Label)GridView_RFQ.SelectedRow.Cells[0].FindControl("Label_RFQId")).Text;
            String contextString = ((Label)GridView_RFQ.SelectedRow.Cells[0].FindControl("Label_RFQName")).Text;

            forwardString += "?contextId1=" + rfqId;
            forwardString += "&context=" + "rfq";
            forwardString += "&contextString=" + contextString;

            ScriptManager.RegisterStartupScript(this, typeof(string), "CloneRfq",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=400');", true);
        }


        protected void Button_Audit_RFQ_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/DispAudit.aspx";
            forwardString += "?contextId1=" + ((Label)GridView_RFQ.SelectedRow.Cells[0].FindControl("Label_RFQId")).Text;
            forwardString += "&contextId2=" + "";
            forwardString += "&contextId3=" + "";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispAuditRFQ", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=700');", true);

        }

        protected void Button_Audit_Req_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/DispAudit.aspx";
            forwardString += "?contextId1=" + ((Label)GridView1.SelectedRow.Cells[0].FindControl("Label_Hidden")).Text;
            forwardString += "&contextId2=" + "";
            forwardString += "&contextId3=" + "";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispAuditReq", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=700');", true);
        }

        protected void Button_RFQ_Doc_Click(object sender, EventArgs e)
        {
            String forwardString = "Popups/Purchase/AllRFQ_NDA.aspx";

            String rfqId = ((Label)GridView_RFQ.SelectedRow.Cells[0].FindControl("Label_RFQId")).Text;
            String docName = ((Label)GridView_RFQ.SelectedRow.Cells[0].FindControl("Label_Hidden_Doc_Name")).Text;
            if (docName == null || docName.Equals(""))
                docName = "N/A";

            forwardString += "?contextId1=" + rfqId;
            forwardString += "&contextId2=" + docName;
            forwardString += "&dataItemIndex=" + GridView_RFQ.SelectedRow.DataItemIndex;

            ScriptManager.RegisterStartupScript(this, typeof(string), "rfqDoc",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=600,height=400,left=500,right=500');", true);
        }

        protected void Button_Notes_Click(object sender, EventArgs e)
        {
            String forwardString = "NoteDetails.aspx";

            String rfqId = ((Label)GridView1.SelectedRow.Cells[0].FindControl("Label_Hidden")).Text;

            forwardString += "?contextId=" + rfqId;

            ScriptManager.RegisterStartupScript(this, typeof(string), "reqNotes",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=800,height=800,left=500,right=500');", true);
        }

        protected void Button_Notes_RFQ_Click(object sender, EventArgs e)
        {
            String forwardString = "NoteDetails.aspx";

            String rfqId = ((Label)GridView_RFQ.SelectedRow.Cells[0].FindControl("Label_RFQId")).Text;

            forwardString += "?contextId=" + rfqId;

            ScriptManager.RegisterStartupScript(this, typeof(string), "rfqNotes",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=800,height=800,left=500,right=500');", true);
        }


        protected void Button_Notes_Inv_Click(object sender, EventArgs e)
        {
            String forwardString = "NoteDetails.aspx";

            String rfqId = ((Label)GridView_Invoice.SelectedRow.Cells[0].FindControl("Label_Invoice_Id1")).Text;

            forwardString += "?contextId=" + rfqId;

            ScriptManager.RegisterStartupScript(this, typeof(string), "rfqNotes",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=800,height=800,left=500,right=500');", true);

        }

        private SortDirection GridViewSortDirectionReq
        {
            get
            {
                if (ViewState["sortDirectionReq"] == null)
                    ViewState["sortDirectionReq"] = SortDirection.Descending;
                return (SortDirection)ViewState["sortDirectionReq"];
            }
            set { ViewState["sortDirectionReq"] = value; }
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

        private SortDirection GridViewSortDirectionPO
        {
            get
            {
                if (ViewState["sortDirectionPO"] == null)
                    ViewState["sortDirectionPO"] = SortDirection.Descending;
                return (SortDirection)ViewState["sortDirectionPO"];
            }
            set { ViewState["sortDirectionPO"] = value; }
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            ViewState["SortExpressionReq"] = sortExpression;
            DataTable dt = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_GRID_DATA];

            if (GridViewSortDirectionReq == SortDirection.Ascending)
            {
                GridViewSortDirectionReq = SortDirection.Descending;
                dt.DefaultView.Sort = sortExpression + " " + "DESC";
            }
            else
            {
                GridViewSortDirectionReq = SortDirection.Ascending;
                dt.DefaultView.Sort = sortExpression + " " + "ASC";
            }

            DataTable sortedTable = dt.DefaultView.ToTable();
            Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_GRID_DATA] = sortedTable;

            disableOnPageChange("req");
            GridView1.SelectedIndex = -1;
            bindSortedData(GridView1, sortedTable);

        }

        protected void bindSortedData(GridView grd, DataTable dt)
        {
            grd.DataSource = dt;
            grd.DataBind();
        }

        protected void GridView_RFQ_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            ViewState["SortExpressionRfq"] = sortExpression;
            DataTable dt = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_GRID_DATA];

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
            Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_GRID_DATA] = sortedTable;
            disableOnPageChange("rfq");
            GridView_RFQ.SelectedIndex = -1;
            bindSortedData(GridView_RFQ, sortedTable);
        }

        protected void GridView_Invoice_Sorting(object sender, GridViewSortEventArgs e)
        {

            string sortExpression = e.SortExpression;
            ViewState["SortExpressionInv"] = sortExpression;
            DataTable dt = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_INV_GRID_DATA];

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
            Session[SessionFactory.ALL_PURCHASE_ALL_INV_GRID_DATA] = sortedTable;
            disableOnPageChange("inv");
            GridView_Invoice.SelectedIndex = -1;
            bindSortedData(GridView_Invoice, sortedTable);

        }

        protected void Button_PO_Refresh_Hidden_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_PO_GRID_DATA];
            //dt.DefaultView.Sort = "Submit Date Ticks" + " " + "DESC";         
            GridView_PO.DataSource = dt;
            GridView_PO.DataBind();
            GridView_PO.SelectedIndex = -1;
            //GridView1.PageIndex = GridView1.PageCount - 1;
            //GridView1.PageIndex = 0;
        }


        protected void Button_Req_Refresh_Hidden_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_GRID_DATA];
            //dt.DefaultView.Sort = "Submit Date Ticks" + " " + "DESC";         
            GridView1.DataSource = dt;
            GridView1.DataBind();
            GridView1.SelectedIndex = -1;
            //GridView1.PageIndex = GridView1.PageCount - 1;
            //GridView1.PageIndex = 0;
        }

        protected void Button_Rfq_Refresh_Hidden_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_GRID_DATA];
            //dt.DefaultView.Sort = "Submit Date Ticks" + " " + "DESC";         
            GridView_RFQ.DataSource = dt;
            GridView_RFQ.DataBind();
            GridView_RFQ.SelectedIndex = -1;
        }

        protected void Button_Rfq_Refresh_Hidden_Click_Index_Unchanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_GRID_DATA];
            //dt.DefaultView.Sort = "Submit Date Ticks" + " " + "DESC";         
            GridView_RFQ.DataSource = dt;
            GridView_RFQ.DataBind();
            ((RadioButton)GridView_RFQ.SelectedRow.Cells[0].FindControl("rfq_radio")).Checked = true;
            //GridView_RFQ.SelectedIndex = -1;
        }

        protected void LinkButton_Approval_Stat_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_RFQ.SelectedIndex)
                GridView_RFQ.SelectRow(row.RowIndex);

            ((RadioButton)GridView_RFQ.SelectedRow.Cells[0].FindControl("rfq_radio")).Checked = true;

            String forwardString = "DispUserDetails.aspx";
            //window.open('Popups/Sale/AllPotn_NDA.aspx','DispNDAPot','status=1,toolbar=1,width=500,height=200,left=500,right=500',true);
            String userId = ((LinkButton)GridView_RFQ.SelectedRow.Cells[0].FindControl("LinkButton_Approval_Stat")).Text;

            forwardString += "?userId=" + userId;

            ScriptManager.RegisterStartupScript(this, typeof(string), "userDetRFQ",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=400,height=400,left=500,right=500');", true);

        }

        protected void GridView1_RadioSelect(object sender, EventArgs e)
        {
            RadioButton linkb = (RadioButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1)
                GridView1.SelectRow(row.RowIndex);
        }

        protected void GridView_RFQ_RadioSelect(object sender, EventArgs e)
        {
            RadioButton linkb = (RadioButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1)
                GridView_RFQ.SelectRow(row.RowIndex);
        }

        protected void GridView_Inv_RadioSelect(object sender, EventArgs e)
        {
            RadioButton linkb = (RadioButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1)
                GridView_Invoice.SelectRow(row.RowIndex);
        }

        protected void LinkButton_Show_Spec_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView1.SelectedIndex)
                GridView1.SelectRow(row.RowIndex);

            ((RadioButton)GridView1.SelectedRow.Cells[0].FindControl("reqr_radio")).Checked = true;

            String forwardString = "Popups/Purchase/AllRequirement_Specification.aspx";
            ScriptManager.RegisterStartupScript(this, typeof(string), "DispSpec",
               "window.open('" + forwardString + "',null,'status=1,location=yes,scrollbars=yes,width=1000,height=600,left=100,right=500');", true);
        }

        protected void LinkButton_Location_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView1.SelectedIndex)
                GridView1.SelectRow(row.RowIndex);

            ((RadioButton)GridView1.SelectedRow.Cells[0].FindControl("reqr_radio")).Checked = true;

            String forwardString = "Popups/Purchase/AllRequirement_Location.aspx";
            ScriptManager.RegisterStartupScript(this, typeof(string), "DispLocation",
               "window.open('" + forwardString + "',null,'status=1,width=800,height=400,left=500,right=500');", true);
        }

        protected void LinkButton_Show_Spec_Command1(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_RFQ.SelectedIndex)
                GridView_RFQ.SelectRow(row.RowIndex);

            ((RadioButton)GridView_RFQ.SelectedRow.Cells[0].FindControl("rfq_radio")).Checked = true;
            String approvalStat = ((LinkButton)GridView_RFQ.SelectedRow.Cells[0].FindControl("LinkButton_Approval_Stat")).Text;

            String forwardString = "Popups/Purchase/AllRFQ_Specification.aspx";
            forwardString += "?approvalStat=" + approvalStat;
            ScriptManager.RegisterStartupScript(this, typeof(string), "DispSpecRFQ",
               "window.open('" + forwardString + "',null,'status=1,location=yes,width=1000,scrollbars=yes,height=600,left=100,right=500');", true);
        }

        protected void LinkButton_Location_Command1(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_RFQ.SelectedIndex)
                GridView_RFQ.SelectRow(row.RowIndex);

            ((RadioButton)GridView_RFQ.SelectedRow.Cells[0].FindControl("rfq_radio")).Checked = true;

            String forwardString = "Popups/Purchase/AllRFQ_Location.aspx";
            ScriptManager.RegisterStartupScript(this, typeof(string), "DispLocation",
               "window.open('" + forwardString + "',null,'status=1,width=700,height=400,left=500,right=500');", true);
        }

        protected void LinkButton_All_Quotes_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_RFQ.SelectedIndex)
                GridView_RFQ.SelectRow(row.RowIndex);

            ((RadioButton)GridView_RFQ.SelectedRow.Cells[0].FindControl("rfq_radio")).Checked = true;

            String forwardString = "Popups/Purchase/AllRFQ_Quotes.aspx";
            ScriptManager.RegisterStartupScript(this, typeof(string), "DispRFQQuotes",
               "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=800,height=400,left=500,right=500');", true);
        }

        protected void LinkButton_All_Shortlisted_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_RFQ.SelectedIndex)
                GridView_RFQ.SelectRow(row.RowIndex);

            ((RadioButton)GridView_RFQ.SelectedRow.Cells[0].FindControl("rfq_radio")).Checked = true;

            String forwardString = "Popups/Purchase/AllRFQ_Shortlisted.aspx";
            ScriptManager.RegisterStartupScript(this, typeof(string), "DispRFQShort",
               "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=800,height=400,left=500,right=500');", true);
        }

        protected void LinkButton_Broadcast_To_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_RFQ.SelectedIndex)
                GridView_RFQ.SelectRow(row.RowIndex);

            ((RadioButton)GridView_RFQ.SelectedRow.Cells[0].FindControl("rfq_radio")).Checked = true;

            String forwardString = "Popups/Purchase/AllRFQ_Broadcast.aspx";
            ScriptManager.RegisterStartupScript(this, typeof(string), "DispBroadcast",
               "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=1000,height=400,left=100,right=500');", true);
        }

        protected void LinkButton1_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_RFQ.SelectedIndex)
                GridView_RFQ.SelectRow(row.RowIndex);

            ((RadioButton)GridView_RFQ.SelectedRow.Cells[0].FindControl("rfq_radio")).Checked = true;

            String forwardString = "Popups/Purchase/AllRFQ_TnC.aspx";
            ScriptManager.RegisterStartupScript(this, typeof(string), "DispTnCRFQ",
               "window.open('" + forwardString + "',null,'status=1,width=600,height=400,left=500,right=500');", true);
        }

        protected void Button_Workflow_Tree_Click(object sender, EventArgs e)
        {
            String forwardString = "Workflow_Tree.aspx";

            String rfqId = ((Label)GridView_RFQ.SelectedRow.Cells[0].FindControl("Label_RFQId")).Text;

            forwardString += "?contextId=" + rfqId;
            forwardString += "&contextName=" + BackEndObjects.Workflow_Action.WORKFLOW_ACTION_CONTEXT_NAME_RFQ;
            forwardString += "&approvalContext=N";

            ScriptManager.RegisterStartupScript(this, typeof(string), "workflow_tree_rfq",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=500,height=400,left=500,right=500');", true);
        }

        protected void GridView_PO_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_PO.SelectedIndex = -1;
            GridView_PO.PageIndex = e.NewPageIndex;
            GridView_PO.DataSource = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_PO_GRID_DATA];
            GridView_PO.DataBind();
        }

        protected void GridView_PO_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView_PO.EditIndex = e.NewEditIndex;
            GridView_PO.DataSource = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_PO_GRID_DATA];
            GridView_PO.DataBind();
        }

        protected void GridView_PO_SelectedIndexChanged(object sender, EventArgs e)
        {
            String selectedRfqId = ((Label)GridView_PO.SelectedRow.Cells[2].FindControl("Label_RFQId1")).Text;
           // Session[SessionFactory.ALL_PURCHASE_ALL_INV_SELECTED_RFQ_SPEC] = BackEndObjects.RFQProductServiceDetails.
                //getAllProductServiceDetailsbyRFQIdDB(selectedRfqId);
            ((RadioButton)GridView_PO.SelectedRow.Cells[0].FindControl("po_radio")).Checked = true;
            enableOnSelect("po");
        }

        protected void GridView_PO_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            ViewState["SortExpressionPO"] = sortExpression;
            DataTable dt = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_PO_GRID_DATA];

            if (GridViewSortDirectionPO == SortDirection.Ascending)
            {
                GridViewSortDirectionPO = SortDirection.Descending;
                dt.DefaultView.Sort = sortExpression + " " + "DESC";
            }
            else
            {
                GridViewSortDirectionPO = SortDirection.Ascending;
                dt.DefaultView.Sort = sortExpression + " " + "ASC";
            }

            DataTable sortedTable = dt.DefaultView.ToTable();
            Session[SessionFactory.ALL_PURCHASE_ALL_INV_GRID_DATA] = sortedTable;
            disableOnPageChange("po");
            GridView_PO.SelectedIndex = -1;
            bindSortedData(GridView_PO, sortedTable);

        }

        protected void GridView_PO_RowDataBound(object sender, GridViewRowEventArgs e)
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
            }
        }

        protected void LinkButton_PO_Purchase_Order_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_PO.SelectedIndex)
                GridView_PO.SelectRow(row.RowIndex);

            ((RadioButton)GridView_PO.SelectedRow.Cells[0].FindControl("po_radio")).Checked = true;

            String rfqId = ((Label)GridView_PO.SelectedRow.Cells[0].FindControl("Label_RFQId1")).Text;

            String forwardString = "/Pages/Popups/Purchase/PO_Details.aspx";
            forwardString += "?rfId=" + rfqId;
            forwardString += "&context=" + "client";

            forwardString += "&poId=" + ((LinkButton)GridView_PO.SelectedRow.Cells[0].FindControl("LinkButton_PO_Purchase_Order")).Text;

            Invoice invObj = rfqId.IndexOf("dummy")>=0?null:BackEndObjects.Invoice.getInvoicebyRfIdDB(rfqId);
            if ((invObj == null || invObj.getInvoiceId() == null || invObj.getInvoiceId().Equals("")) && GridView_PO.Columns[1].Visible) //Deal finalized for this RFQ - not edit allowed for PO
            {
                forwardString += "&allowEdit=" + "true";
                forwardString += "&refreshParent=" + "true";
                forwardString += "&dataItemIndex=" + row.DataItemIndex;
            }

                
            ScriptManager.RegisterStartupScript(this, typeof(string), "DispPO", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=900');", true);



        }

        protected void Button_Filter_PO_Click(object sender, EventArgs e)
        {
            fillPOGridwithFilter();
            if (GridView_PO.Rows.Count > 0)
                GridView_PO.Focus();
            else
                Button_Filter_PO.Focus();
        }

        protected void GridView_PO_RadioSelect(object sender, EventArgs e)
        {
            RadioButton linkb = (RadioButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1)
                GridView_PO.SelectRow(row.RowIndex);
        }

        protected void Button_Notes_PO_Click(object sender, EventArgs e)
        {
            String forwardString = "NoteDetails.aspx";

            String rfqId = ((Label)GridView_PO.SelectedRow.Cells[0].FindControl("Label_RFQId1")).Text;

            forwardString += "?contextId=" + rfqId;

            ScriptManager.RegisterStartupScript(this, typeof(string), "PONotes",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=800,height=800,left=500,right=500');", true);
        }

        protected void GridView_PO_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DataTable dt = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_PO_GRID_DATA];
            GridViewRow gVR = GridView_PO.Rows[e.RowIndex];

            int index = GridView_PO.Rows[e.RowIndex].DataItemIndex;
            
            String curr = "";
            curr = ((DropDownList)gVR.FindControl("DropDownList_Curr")).SelectedItem.Text;
            dt.Rows[index]["curr"] = curr;

            GridView_PO.EditIndex = -1;
            GridView_PO.DataSource = dt;
            GridView_PO.DataBind();

            Session[SessionFactory.ALL_PURCHASE_ALL_PO_GRID_DATA] = dt;

            try
            {
                String poId =((LinkButton)gVR.Cells[0].FindControl("LinkButton_PO_Purchase_Order")).Text;
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

        protected void Button_PO_Refresh_Click(object sender, EventArgs e)
        {
            ArrayList rfqListPO =
 BackEndObjects.RFQDetails.getAllRFQIncludingDummybyEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), true);
            if (rfqListPO.Count > 0)
                fillPOGrid(rfqListPO);

            GridView_PO.SelectedIndex = -1;
            disableOnPageChange("po");
            Button_PO_Refresh.Focus();
        }

        protected void Button_Create_PO_Click(object sender, EventArgs e)
        {
            String forwardString = "CreatePO.aspx";
            ScriptManager.RegisterStartupScript(this, typeof(string), "createPO", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=1000,Height=1000,left=100,right=500');", true);

        }

        protected void GridView_PO_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView_PO.EditIndex = -1;
            GridView_PO.DataSource = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_PO_GRID_DATA];
            GridView_PO.DataBind();
        }



    }
}