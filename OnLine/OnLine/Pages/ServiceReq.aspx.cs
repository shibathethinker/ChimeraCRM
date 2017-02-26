using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using ActionLibrary;
using BackEndObjects;
using System.Collections;
using System.Web.Services;
using System.Web.Script.Services;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Web.UI.HtmlControls;

namespace OnLine.Pages
{
    public partial class ServiceReq : System.Web.UI.Page
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
                    ((HtmlGenericControl)(Master.FindControl("ServiceReq"))).Attributes.Add("class", "active");
                    //((Menu)Master.FindControl("Menu1")).Items[4].Selected = true;
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
                    CheckAccessToActions();
                    if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                    { //Full Access - no need to do any restriction
                        loadDefectStatAndResolStat();
                        loadIncomingDefectList(null);
                        loadOutgoingDefectList(null);
                        LoadAssignedToList();
                    }
                    else if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_SR_SCREEN_VIEW])
                    {
                        loadDefectStatAndResolStat();
                        loadIncomingDefectList(null);
                        loadOutgoingDefectList(null);
                        LoadAssignedToList();
                    }
                    else
                    {
                        Label_Defect_Screen_Access.Visible = true;
                        Label_Defect_Screen_Access.Text = "You don't have access to view this page";
                    }
                }
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

        protected void LoadAssignedToList()
        {
            ListItem me = new ListItem();
            me.Text = "Me";
            me.Value = User.Identity.Name;

            ListItem unassgnd = new ListItem();
            unassgnd.Text = "Unassigned";
            unassgnd.Value = "";
            DropDownList_Assigned_To.Items.Add(me);
            DropDownList_Assigned_To.Items.Add(unassgnd);

            Dictionary<String, userDetails> allUserDetails = (Dictionary<String, userDetails>)Cache[Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()];
            if (allUserDetails == null)
            {
                allUserDetails = BackEndObjects.MainBusinessEntity.getUserDetailsforMainEntitybyIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                Cache.Insert(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), allUserDetails);
            }
            //Dictionary<String, userDetails> allUserDetails = BackEndObjects.MainBusinessEntity.getUserDetailsforMainEntitybyIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

            foreach (KeyValuePair<String, userDetails> kvp in allUserDetails)
            {
                BackEndObjects.userDetails userDetObj = kvp.Value;

                if (!userDetObj.getUserId().Equals(User.Identity.Name))
                {
                    ListItem lt1 = new ListItem();
                    lt1.Text = userDetObj.getUserId();
                    lt1.Value = userDetObj.getUserId();

                    DropDownList_Assigned_To.Items.Add(lt1);
                }
            }
            ListItem emptyUser = new ListItem();
            emptyUser.Text = "_";
            emptyUser.Value = "_";

            DropDownList_Assigned_To.Items.Add(emptyUser);
            DropDownList_Assigned_To.SelectedValue = "_";
        }

        /// <summary>
        /// This method will check access to different buttons and enable/disable based on access
        /// </summary>
        protected void CheckAccessToActions()
        {
            Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

            if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
            {
                Button_Create_Defect_Incm.Enabled = true;
                Button_Create_Defect_Outgoing.Enabled = true;
            }
            else
            {
                if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_CREATE_INCOMING_SR])
                    Button_Create_Defect_Incm.Enabled = false;
                if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_CREATE_OUTGOING_SR])
                    Button_Create_Defect_Outgoing.Enabled = false;
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

        protected void loadDefectStatAndResolStat()
        {
            //First load the SLA list
            ArrayList slaList = (ArrayList)Session[SessionFactory.ALL_SR_SLA_LIST];
            if (slaList == null || slaList.Count == 0)
            {
                slaList = DefectSLA.getDefectSLADetailsbyentIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), DefectSLA.DEFECT_TYPE_SERVICE_REQUEST);
                Session[SessionFactory.ALL_SR_SLA_LIST] = slaList;
            }

            ArrayList defectStatList = BackEndObjects.DefectStatCodes.getAllDefectStatCodesDB();
            ArrayList defectResolStatList = BackEndObjects.DefectResolStatCodes.getAllDefectResolStatDB();

            for (int i = 0; i < defectStatList.Count; i++)
            {
                ListItem lt = new ListItem();
                lt.Text = ((BackEndObjects.DefectStatCodes)defectStatList[i]).getDefectStat();
                lt.Value = ((BackEndObjects.DefectStatCodes)defectStatList[i]).getDefectStat();

                DropDownList_Incm_Defect_Stat.Items.Add(lt);
                DropDownList_Outgoing_Defect_Stat.Items.Add(lt);
            }

            for (int i = 0; i < defectResolStatList.Count; i++)
            {
                ListItem lt = new ListItem();
                lt.Text = ((BackEndObjects.DefectResolStatCodes)defectResolStatList[i]).getResolStat();
                lt.Value = ((BackEndObjects.DefectResolStatCodes)defectResolStatList[i]).getResolStat();

                DropDownList_Incm_Defect_Resol_Stat.Items.Add(lt);
                DropDownList_Outgoing_Defect_Resol_Stat.Items.Add(lt);
            }

            ListItem ltHigh = new ListItem();
            ltHigh.Text = "High";
            ltHigh.Value = "High";

            ListItem ltMed = new ListItem();
            ltMed.Text = "Medium";
            ltMed.Text = "Medium";

            ListItem ltLow = new ListItem();
            ltLow.Text = "Low";
            ltLow.Value = "Low";

            ListItem ltNone = new ListItem();
            ltNone.Text = "_";
            ltNone.Value = "_";

            DropDownList_Incm_Defect_Sev.Items.Add(ltHigh);
            DropDownList_Incm_Defect_Sev.Items.Add(ltMed);
            DropDownList_Incm_Defect_Sev.Items.Add(ltLow);
            DropDownList_Incm_Defect_Sev.Items.Add(ltNone);

            DropDownList_Outg_Defect_Sev.Items.Add(ltHigh);
            DropDownList_Outg_Defect_Sev.Items.Add(ltMed);
            DropDownList_Outg_Defect_Sev.Items.Add(ltLow);
            DropDownList_Outg_Defect_Sev.Items.Add(ltNone);


            ListItem ltEmpty = new ListItem();
            ltEmpty.Text = "_";
            ltEmpty.Value = "_";

            DropDownList_Incm_Defect_Stat.Items.Add(ltEmpty);
            DropDownList_Outgoing_Defect_Stat.Items.Add(ltEmpty);
            DropDownList_Incm_Defect_Resol_Stat.Items.Add(ltEmpty);
            DropDownList_Outgoing_Defect_Resol_Stat.Items.Add(ltEmpty);

            DropDownList_Incm_Defect_Stat.SelectedValue = "_";
            DropDownList_Outgoing_Defect_Stat.SelectedValue = "_";
            DropDownList_Incm_Defect_Resol_Stat.SelectedValue = "_";
            DropDownList_Outgoing_Defect_Resol_Stat.SelectedValue = "_";
            DropDownList_Incm_Defect_Sev.SelectedValue = "_";
            DropDownList_Outg_Defect_Sev.SelectedValue = "_";
        }

        protected void loadDefectSev(DropDownList DropDownList_Defect_Sev, String selectedVal)
        {
            ListItem lt1 = new ListItem();
            lt1.Text = "High";
            lt1.Value = "High";

            ListItem lt2 = new ListItem();
            lt2.Text = "Medium";
            lt2.Text = "Medium";

            ListItem lt3 = new ListItem();
            lt3.Text = "Low";
            lt3.Value = "Low";

            DropDownList_Defect_Sev.Items.Add(lt1);
            DropDownList_Defect_Sev.Items.Add(lt2);
            DropDownList_Defect_Sev.Items.Add(lt3);

            if (selectedVal != null && !selectedVal.Equals(""))
                DropDownList_Defect_Sev.SelectedValue = selectedVal;
            else
                DropDownList_Defect_Sev.SelectedValue = "Low";
        }

        protected void loadContacts(DropDownList DropDownList_Contacts, String selectedVal)
        {
            //loadContactList();
            //This view state implementation of storing and retriving the contact list caused the gridview to freeze
            //(ArrayList)ViewState["contactObjectListDefect"];
            // ArrayList contactObjList = Contacts.getAllContactsbyEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            /*for (int i = 0; i < contactObjList.Count; i++)
            {
                ListItem lt = new ListItem();
                String contactShortName = ((Contacts)contactObjList[i]).getContactShortName();
                String contactName = ((Contacts)contactObjList[i]).getContactName();
                String contactEntId = ((Contacts)contactObjList[i]).getContactEntityId();

               //lt.Text = (contactShortName == null || contactShortName.Equals("") ? contactName : contactShortName);
                lt.Text = contactName;
                lt.Value = contactEntId;
                DropDownList_Contacts.Items.Add(lt);
            }*/

            Dictionary<String, String> existingContactDict = (Dictionary<String, String>)Session[SessionFactory.EXISTING_CONTACT_DICTIONARY];
            if (existingContactDict != null)
                foreach (KeyValuePair<String, String> kvp in existingContactDict)
                {
                    String contactName = kvp.Key;
                    String contactEntId = kvp.Value;

                    ListItem ltItem = new ListItem();
                    ltItem.Text = contactName;
                    ltItem.Value = contactEntId;
                    DropDownList_Contacts.Items.Add(ltItem);

                }
            if (selectedVal != null && !selectedVal.Equals(""))
                DropDownList_Contacts.SelectedValue = selectedVal;
        }

        protected void loadIncomingDefectListFiltered()
        {
            Dictionary<String, String> filterParams = new Dictionary<String, String>();
            ActionLibrary.DefectActions._dispDefectDetails dspDefect = new DefectActions._dispDefectDetails();

            if (!TextBox_Defect_No_Incm_Defect.Text.Equals(""))
                filterParams.Add(dspDefect.FILTER_BY_DEFECT_NO, TextBox_Defect_No_Incm_Defect.Text);
            if (!TextBox_Inv_No_Incm_Defect.Text.Equals(""))
                filterParams.Add(dspDefect.FILTER_BY_INVOICE_NO, TextBox_Inv_No_Incm_Defect.Text);
            if (!TextBox_RFQ_No_Incm_Defect.Text.Equals(""))
                filterParams.Add(dspDefect.FILTER_BY_RFQ_NO, TextBox_RFQ_No_Incm_Defect.Text);
            if (!DropDownList_Incm_Defect_Resol_Stat.SelectedValue.Equals("_"))
                filterParams.Add(dspDefect.FILTER_BY_DEFECT_RESOL_STAT, DropDownList_Incm_Defect_Resol_Stat.SelectedValue);
            if (!DropDownList_Incm_Defect_Stat.SelectedValue.Equals("_"))
                filterParams.Add(dspDefect.FILTER_BY_DEFECT_STAT, DropDownList_Incm_Defect_Stat.SelectedValue);
            if (!TextBox_From_Date_Incm_Defect_Raised.Text.Equals(""))
                filterParams.Add(dspDefect.FILTER_BY_SUBMIT_DATE_FROM, TextBox_From_Date_Incm_Defect_Raised.Text);
            if (!TextBox_To_Date_Incm_Defect_Raised.Text.Equals(""))
                filterParams.Add(dspDefect.FILTER_BY_SUBMIT_DATE_TO, TextBox_To_Date_Incm_Defect_Raised.Text);
            if (!DropDownList_Assigned_To.SelectedValue.Equals("_"))
                filterParams.Add(dspDefect.FILTER_BY_ASSIGNED_TO, DropDownList_Assigned_To.SelectedValue);
            if (!DropDownList_Incm_Defect_Sev.SelectedValue.Equals("_"))
                filterParams.Add(dspDefect.FILTER_BY_DEFECT_SEV, DropDownList_Incm_Defect_Sev.SelectedValue);

            loadIncomingDefectList(dspDefect.getAllDefectsFiltered("incoming", Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), filterParams));

            Button_Filter_Incoming_Defects.Focus();
        }

        protected void loadIncomingDefectList(Dictionary<String, DefectDetails> defectDictPassed)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DefectId");
            dt.Columns.Add("RFQId");
            dt.Columns.Add("InvNo");
            dt.Columns.Add("descr");
            dt.Columns.Add("descr_short");
            dt.Columns.Add("Submit Date");
            dt.Columns.Add("Submit Date Ticks");
            dt.Columns.Add("Close Date");
            dt.Columns.Add("Close Date Ticks");
            dt.Columns.Add("Amount");
            dt.Columns.Add("CustName");
            dt.Columns.Add("entId");
            dt.Columns.Add("Defect_Stat");
            dt.Columns.Add("Defect_Stat_Reason");
            dt.Columns.Add("Assigned_To");
            dt.Columns.Add("Severity");
            dt.Columns.Add("Defect_Resol_Stat");
            //dt.Columns.Add("docName");
            dt.Columns.Add("docNameHidden");
            dt.Columns.Add("curr");

            Dictionary<String, DefectDetails> defectDict = new Dictionary<string, DefectDetails>();

            if (defectDictPassed == null)
                defectDict = BackEndObjects.DefectDetails.getAllSRDetailsforSupplierIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            else
                defectDict = defectDictPassed;

            int counter = 0;
            Dictionary<String, Currency> allCurrList = (Dictionary<String, Currency>)Session[SessionFactory.CURRENCY_LIST];
            DateUtility dU = new DateUtility();

            foreach (KeyValuePair<String, DefectDetails> kvp in defectDict)
            {
                dt.Rows.Add();

                BackEndObjects.DefectDetails defObj = kvp.Value;

                String custId = defObj.getCustomerId();
                String custName = "";
                if (defObj.getCreationMode().Equals(BackEndObjects.DefectDetails.DEFECT_CREATION_MODE_MANUAL))
                {
                    custName = BackEndObjects.Contacts.getContactDetailsforContactEntityDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), custId).getContactName();
                    if (custName == null || custName.Equals(""))
                        custName = MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(custId).getEntityName();
                }
                else
                    custName = MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(custId).getEntityName();


                dt.Rows[counter]["DefectId"] = defObj.getDefectId();
                dt.Rows[counter]["RFQId"] = defObj.getRFQId();
                dt.Rows[counter]["InvNo"] = defObj.getInvoiceId();
                dt.Rows[counter]["descr"] = defObj.getDescription();
                dt.Rows[counter]["descr_short"] = !defObj.getDescription().Equals("") ? defObj.getDescription().
                    Substring(0, (defObj.getDescription().Length > 20 ? 20 : defObj.getDescription().Length - 1)) : "";
                dt.Rows[counter]["Submit Date"] = dU.getConvertedDate(defObj.getDateCreated());
                dt.Rows[counter]["Submit Date Ticks"] = Convert.ToDateTime(defObj.getDateCreated()).Ticks;
                dt.Rows[counter]["Close Date"] = dU.getConvertedDate((defObj.getCloseDate() != null && !defObj.getCloseDate().Equals("") && !(defObj.getCloseDate().IndexOf("1/1/1900") >= 0) ? defObj.getCloseDate() : ""));
                dt.Rows[counter]["Close Date Ticks"] = (defObj.getCloseDate() != null && !defObj.getCloseDate().Equals("") ? Convert.ToDateTime(defObj.getCloseDate()).Ticks : 0);
                dt.Rows[counter]["Amount"] = defObj.getTotalAmount();
                dt.Rows[counter]["CustName"] = custName;
                dt.Rows[counter]["entId"] = custId;
                dt.Rows[counter]["Defect_Stat"] = defObj.getDefectStat();
                dt.Rows[counter]["Defect_Stat_Reason"] = defObj.getStatReason();
                dt.Rows[counter]["Assigned_To"] = defObj.getAssignedToUser();
                dt.Rows[counter]["Severity"] = defObj.getSeverity();
                dt.Rows[counter]["Defect_Resol_Stat"] = defObj.getResolStat();
                dt.Rows[counter]["curr"] = defObj.getCurrency() != null &&
                    allCurrList.ContainsKey(defObj.getCurrency()) ?
                               allCurrList[defObj.getCurrency()].getCurrencyName() : "";
                String docName = "";
                if (defObj.getDocPath() != null)
                {
                    String[] docPathList = defObj.getDocPath().Split(new String[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                    if (docPathList.Length > 0)
                        docName = docPathList[docPathList.Length - 1];
                }
                //dt.Rows[counter]["docName"] = (defObj.getDocPath() == null || defObj.getDocPath().Equals("") ? "N/A" : "Show");
                dt.Rows[counter]["docNameHidden"] = (defObj.getDocPath() == null || defObj.getDocPath().Equals("") ? "N/A" : docName);

                counter++;
            }
            dt.DefaultView.Sort = "Submit Date Ticks" + " " + "DESC";
            disableOnPageChange("incm");
            GridView_Incoming_SR.SelectedIndex = -1;
            GridView_Incoming_SR.Visible = true;
            GridView_Incoming_SR.DataSource = dt.DefaultView.ToTable();
            GridView_Incoming_SR.DataBind();

            Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];
            if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_EDIT_INCOMING_SR] &&
    !accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                GridView_Incoming_SR.Columns[1].Visible = false;

            Session[SessionFactory.ALL_SR_ALL_INCOMING_SR_GRID] = dt.DefaultView.ToTable();

        }

        protected void loadOutgoingDefectListFiltered()
        {
            Dictionary<String, String> filterParams = new Dictionary<String, String>();
            ActionLibrary.DefectActions._dispDefectDetails dspDefect = new DefectActions._dispDefectDetails();

            if (!TextBox_Defect_No_Outgoing_Defect.Text.Equals(""))
                filterParams.Add(dspDefect.FILTER_BY_DEFECT_NO, TextBox_Defect_No_Outgoing_Defect.Text);
            if (!TextBox_Inv_No_Outgoing_Defect.Text.Equals(""))
                filterParams.Add(dspDefect.FILTER_BY_INVOICE_NO, TextBox_Inv_No_Outgoing_Defect.Text);
            if (!TextBox_RFQ_No_Outgoing_Defect.Text.Equals(""))
                filterParams.Add(dspDefect.FILTER_BY_RFQ_NO, TextBox_RFQ_No_Outgoing_Defect.Text);
            if (!DropDownList_Outgoing_Defect_Resol_Stat.SelectedValue.Equals("_"))
                filterParams.Add(dspDefect.FILTER_BY_DEFECT_RESOL_STAT, DropDownList_Outgoing_Defect_Resol_Stat.SelectedValue);
            if (!DropDownList_Outgoing_Defect_Stat.SelectedValue.Equals("_"))
                filterParams.Add(dspDefect.FILTER_BY_DEFECT_STAT, DropDownList_Outgoing_Defect_Stat.SelectedValue);
            if (!TextBox_From_Date_Outgoing_Defect_Raised.Text.Equals(""))
                filterParams.Add(dspDefect.FILTER_BY_SUBMIT_DATE_FROM, TextBox_From_Date_Outgoing_Defect_Raised.Text);
            if (!TextBox_To_Date_Outgoing_Defect_Raised.Text.Equals(""))
                filterParams.Add(dspDefect.FILTER_BY_SUBMIT_DATE_TO, TextBox_To_Date_Outgoing_Defect_Raised.Text);
            if (!DropDownList_Outg_Defect_Sev.SelectedValue.Equals("_"))
                filterParams.Add(dspDefect.FILTER_BY_DEFECT_SEV, DropDownList_Outg_Defect_Sev.SelectedValue);

            loadOutgoingDefectList(dspDefect.getAllSRsFiltered("outgoing", Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), filterParams));

            Button_Filter_Outgoing_Defects.Focus();
        }

        protected void loadOutgoingDefectList(Dictionary<String, DefectDetails> defectDictPassed)
        {
            DataTable dt = new DataTable();
            Dictionary<String, Currency> allCurrList = (Dictionary<String, Currency>)Session[SessionFactory.CURRENCY_LIST];

            dt.Columns.Add("DefectId");
            dt.Columns.Add("RFQId");
            dt.Columns.Add("InvNo");
            dt.Columns.Add("descr");
            dt.Columns.Add("descr_short");
            dt.Columns.Add("Submit Date");
            dt.Columns.Add("Submit Date Ticks");
            dt.Columns.Add("Close Date");
            dt.Columns.Add("Close Date Ticks");
            dt.Columns.Add("Amount");
            dt.Columns.Add("SuplName");
            dt.Columns.Add("entId");
            dt.Columns.Add("Defect_Stat");
            dt.Columns.Add("Defect_Stat_Reason");
            //dt.Columns.Add("Assigned_To");
            dt.Columns.Add("Severity");
            dt.Columns.Add("Defect_Resol_Stat");
            //dt.Columns.Add("docName");
            dt.Columns.Add("docNameHidden");
            dt.Columns.Add("curr");

            Dictionary<String, DefectDetails> defectDict = new Dictionary<string, DefectDetails>();

            if (defectDictPassed == null)
                defectDict = BackEndObjects.DefectDetails.getAllSRDetailsforCustomerIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            else
                defectDict = defectDictPassed;

            int counter = 0;

            DateUtility dU = new DateUtility();

            foreach (KeyValuePair<String, DefectDetails> kvp in defectDict)
            {
                dt.Rows.Add();

                BackEndObjects.DefectDetails defObj = kvp.Value;

                String suplId = defObj.getSupplierId();
                String suplName = "";
                if (defObj.getCreationMode().Equals(BackEndObjects.DefectDetails.DEFECT_CREATION_MODE_MANUAL))
                {
                    suplName = BackEndObjects.Contacts.getContactDetailsforContactEntityDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), suplId).getContactName();
                    if (suplName == null || suplName.Equals(""))
                        suplName = MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(suplId).getEntityName();
                }
                else
                    suplName = MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(suplId).getEntityName();

                dt.Rows[counter]["DefectId"] = defObj.getDefectId();
                dt.Rows[counter]["RFQId"] = defObj.getRFQId();
                dt.Rows[counter]["InvNo"] = defObj.getInvoiceId();
                dt.Rows[counter]["descr"] = defObj.getDescription();
                dt.Rows[counter]["descr_short"] = !defObj.getDescription().Equals("") ? defObj.getDescription().
    Substring(0, (defObj.getDescription().Length > 20 ? 20 : defObj.getDescription().Length - 1)) : "";
                dt.Rows[counter]["Submit Date"] = dU.getConvertedDate(defObj.getDateCreated());
                dt.Rows[counter]["Submit Date Ticks"] = Convert.ToDateTime(defObj.getDateCreated()).Ticks;
                dt.Rows[counter]["Close Date"] = dU.getConvertedDate((defObj.getCloseDate() != null && !defObj.getCloseDate().Equals("") && !(defObj.getCloseDate().IndexOf("1/1/1900") >= 0) ? defObj.getCloseDate() : ""));
                dt.Rows[counter]["Close Date Ticks"] = (defObj.getCloseDate() != null && !defObj.getCloseDate().Equals("") ? Convert.ToDateTime(defObj.getCloseDate()).Ticks : 0);
                dt.Rows[counter]["Amount"] = defObj.getTotalAmount();
                dt.Rows[counter]["SuplName"] = suplName;
                dt.Rows[counter]["entId"] = suplId;
                dt.Rows[counter]["Defect_Stat"] = defObj.getDefectStat();
                dt.Rows[counter]["Defect_Stat_Reason"] = defObj.getStatReason();
                //dt.Rows[counter]["Assigned_To"] = defObj.getAssignedToUser();
                dt.Rows[counter]["Severity"] = defObj.getSeverity();
                dt.Rows[counter]["Defect_Resol_Stat"] = defObj.getResolStat();
                dt.Rows[counter]["curr"] = defObj.getCurrency() != null &&
                                        allCurrList.ContainsKey(defObj.getCurrency()) ?
                                                   allCurrList[defObj.getCurrency()].getCurrencyName() : "";

                String docName = "";
                if (defObj.getDocPath() != null)
                {
                    String[] docPathList = defObj.getDocPath().Split(new String[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                    if (docPathList.Length > 0)
                        docName = docPathList[docPathList.Length - 1];
                }
                //dt.Rows[counter]["docName"] = (defObj.getDocPath() == null || defObj.getDocPath().Equals("") ? "N/A" : "Show");
                dt.Rows[counter]["docNameHidden"] = (defObj.getDocPath() == null || defObj.getDocPath().Equals("") ? "N/A" : docName);


                counter++;
            }
            dt.DefaultView.Sort = "Submit Date Ticks" + " " + "DESC";

            disableOnPageChange("outg");
            GridView_Outgoing_SR.SelectedIndex = -1;
            GridView_Outgoing_SR.Visible = true;
            GridView_Outgoing_SR.DataSource = dt;
            GridView_Outgoing_SR.DataBind();

            Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

            if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_EDIT_OUTGOING_SR] &&
                !accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                GridView_Outgoing_SR.Columns[1].Visible = false;

            Session[SessionFactory.ALL_SR_ALL_OUTGOING_SR_GRID] = dt.DefaultView.ToTable();
        }

        protected void Button_Create_Defect_Incm_Click(object sender, EventArgs e)
        {
            String forwardString = "CreateSR.aspx";
            forwardString += "?context=" + "incoming";

            ScriptManager.RegisterStartupScript(this, typeof(string), "createIncmSRManual", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=900,Height=900');", true);

        }

        protected void Button_Create_Defect_Outgoing_Click(object sender, EventArgs e)
        {
            String forwardString = "CreateSR.aspx";
            forwardString += "?context=" + "outgoing";

            ScriptManager.RegisterStartupScript(this, typeof(string), "createIncmSRManual", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=900,Height=900');", true);
        }

        protected void GridView_Incoming_SR_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_Incoming_SR.PageIndex = e.NewPageIndex;
            disableOnPageChange("incm");
            DataTable dt = (DataTable)Session[SessionFactory.ALL_SR_ALL_INCOMING_SR_GRID];
            if (dt != null)
            {
                GridView_Incoming_SR.DataSource = dt;
                GridView_Incoming_SR.DataBind();
                GridView_Incoming_SR.SelectedIndex = -1;
            }
        }

        protected void GridView_Outgoing_SR_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_Outgoing_SR.PageIndex = e.NewPageIndex;
            disableOnPageChange("outg");
            DataTable dt = (DataTable)Session[SessionFactory.ALL_SR_ALL_OUTGOING_SR_GRID];
            if (dt != null)
            {
                GridView_Outgoing_SR.DataSource = dt;
                GridView_Outgoing_SR.DataBind();
                GridView_Outgoing_SR.SelectedIndex = -1;
            }
        }

        protected void Button_Filter_Outgoing_Defects_Click(object sender, EventArgs e)
        {
            loadOutgoingDefectListFiltered();
        }

        protected void Button_Filter_Incoming_Defects_Click(object sender, EventArgs e)
        {
            loadIncomingDefectListFiltered();
        }

        protected void GridView_Incoming_SR_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                GridViewRow gVR = e.Row;

                ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Incm_Defect_Stat_Edit")).Visible = true;
                ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Incm_Defect_Resol_Stat_Edit")).Visible = true;

                ArrayList defectStatList = BackEndObjects.DefectStatCodes.getAllDefectStatCodesDB();
                ArrayList defectResolStatList = BackEndObjects.DefectResolStatCodes.getAllDefectResolStatDB();
                //BackEndObjects.MainBusinessEntity.getUserDetailsforMainEntitybyIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                Dictionary<String, userDetails> allUserDetails = (Dictionary<String, userDetails>)Cache[Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()];
                if (allUserDetails == null)
                {
                    allUserDetails = BackEndObjects.MainBusinessEntity.getUserDetailsforMainEntitybyIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                    Cache.Insert(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), allUserDetails);
                }

                DropDownList DropDownList_Incm_Defect_Stat = (DropDownList)gVR.Cells[0].FindControl("DropDownList_Incm_Defect_Stat_Edit");
                DropDownList DropDownList_Incm_Defect_Resol_Stat = (DropDownList)gVR.Cells[0].FindControl("DropDownList_Incm_Defect_Resol_Stat_Edit");
                DropDownList DropDownList_Incm_Defect_Assgn_To = (DropDownList)gVR.Cells[0].FindControl("DropDownList_Incm_Defect_Assgn_To_Edit");

                loadDefectSev((DropDownList)gVR.Cells[0].FindControl("DropDownList_Incm_Defect_Sev_Edit"),
                    ((Label)gVR.Cells[0].FindControl("Label_Sev_Edit")).Text);

                loadContacts((DropDownList)gVR.Cells[0].FindControl("DropDownList_Incm_Defect_Cust_Det_Edit"),
                    ((Label)gVR.Cells[0].FindControl("Label_EntId_Edit")).Text);

                for (int i = 0; i < defectStatList.Count; i++)
                {
                    ListItem lt = new ListItem();
                    lt.Text = ((BackEndObjects.DefectStatCodes)defectStatList[i]).getDefectStat();
                    lt.Value = ((BackEndObjects.DefectStatCodes)defectStatList[i]).getDefectStat();

                    DropDownList_Incm_Defect_Stat.Items.Add(lt);
                }

                for (int i = 0; i < defectResolStatList.Count; i++)
                {
                    ListItem lt = new ListItem();
                    lt.Text = ((BackEndObjects.DefectResolStatCodes)defectResolStatList[i]).getResolStat();
                    lt.Value = ((BackEndObjects.DefectResolStatCodes)defectResolStatList[i]).getResolStat();

                    DropDownList_Incm_Defect_Resol_Stat.Items.Add(lt);
                }

                foreach (KeyValuePair<String, userDetails> kvp in allUserDetails)
                {
                    BackEndObjects.userDetails userDetObj = kvp.Value;

                    ListItem lt = new ListItem();
                    lt.Text = userDetObj.getUserId();
                    lt.Value = userDetObj.getUserId();

                    DropDownList_Incm_Defect_Assgn_To.Items.Add(lt);
                }

                DropDownList_Incm_Defect_Stat.SelectedValue = ((Label)gVR.Cells[0].FindControl("Label_Defect_Stat_Edit")).Text;
                DropDownList_Incm_Defect_Resol_Stat.SelectedValue = ((Label)gVR.Cells[0].FindControl("Label_Defect_Resol_Stat_Edit")).Text;

                if (!((Label)gVR.Cells[0].FindControl("Label_Assgn_To_Edit")).Text.Equals(""))
                    DropDownList_Incm_Defect_Assgn_To.SelectedValue = ((Label)gVR.Cells[0].FindControl("Label_Assgn_To_Edit")).Text;


                String defectId = ((Label)gVR.Cells[0].FindControl("Label_Defect_Id")).Text;
                BackEndObjects.DefectDetails defObj = BackEndObjects.DefectDetails.getSRDetailsbyidDB(defectId);

                if (defObj.getCreationMode().Equals(BackEndObjects.DefectDetails.DEFECT_CREATION_MODE_MANUAL))
                {
                    //If this defect was manually created by this entity id then allow editing the following fields
                    if (defObj.getCreatedByComp().Equals(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()))
                    {
                        ((TextBox)gVR.Cells[0].FindControl("TextBox_RFQ_No_Edit")).Enabled = true;
                        ((TextBox)gVR.Cells[0].FindControl("TextBox_Invoice_No_Edit")).Enabled = true;
                        ((TextBox)gVR.Cells[0].FindControl("TextBox_Descr_Edit")).Enabled = true;
                        ((TextBox)gVR.Cells[0].FindControl("Textbox_Edit_Amount")).Enabled = true;
                        ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Incm_Defect_Sev_Edit")).Visible = true;
                        ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Incm_Defect_Stat_Edit")).Visible = true;
                        ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Incm_Defect_Resol_Stat_Edit")).Visible = true;

                        loadCurrency(((DropDownList)gVR.FindControl("DropDownList_Curr")), ((Label)gVR.FindControl("Label_Curr_Edit")).Text);
                    }
                    else
                    {
                        ((TextBox)gVR.Cells[0].FindControl("Textbox_Edit_Amount")).Enabled = false;
                        ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Incm_Defect_Sev_Edit")).Visible = false;

                        ((Label)gVR.Cells[0].FindControl("Label_Customer_Name_Edit_Incoming")).Visible = true;
                        ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Incm_Defect_Cust_Det_Edit")).Visible = false;

                        ((Label)gVR.Cells[0].FindControl("Label_Sev_Edit")).Visible = true;
                        ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Incm_Defect_Stat_Edit")).Visible = true;
                        ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Incm_Defect_Resol_Stat_Edit")).Visible = true;

                        ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Curr")).Visible = false;
                        ((Label)gVR.Cells[0].FindControl("Label_Curr_Edit")).Visible = true;

                    }
                }
                else
                {
                    if (defObj.getCreatedByComp().Equals(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()))
                    {
                        ((TextBox)gVR.Cells[0].FindControl("TextBox_Descr_Edit")).Enabled = true;
                        ((TextBox)gVR.Cells[0].FindControl("Textbox_Edit_Amount")).Enabled = true;
                        ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Incm_Defect_Sev_Edit")).Visible = true;
                        ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Incm_Defect_Stat_Edit")).Visible = true;
                        ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Incm_Defect_Resol_Stat_Edit")).Visible = true;
                    }
                    else
                    {
                        ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Incm_Defect_Sev_Edit")).Visible = false;
                        ((Label)gVR.Cells[0].FindControl("Label_Sev_Edit")).Visible = true;
                        ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Incm_Defect_Stat_Edit")).Visible = true;
                        ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Incm_Defect_Resol_Stat_Edit")).Visible = true;
                    }
                }
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
                    ArrayList slaList = (ArrayList)Session[SessionFactory.ALL_SR_SLA_LIST];
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

        protected void GridView_Incoming_SR_RowEditing(object sender, GridViewEditEventArgs e)
        {
            /* Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

             if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_EDIT_INCOMING_DEFECT]||
                 accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
             {*/
            Label_Incoming_Defect_Grid_Access.Visible = false;
            GridView_Incoming_SR.EditIndex = e.NewEditIndex;
            GridView_Incoming_SR.DataSource = (DataTable)Session[SessionFactory.ALL_SR_ALL_INCOMING_SR_GRID];
            GridView_Incoming_SR.DataBind();
            Label_Incm_msg.Visible = true;
            Label_Incm_msg.Text = "You can edit all the fields only if the SR is created by your organization";
            Label_Incm_msg.ForeColor = System.Drawing.Color.Green;
            /*  }
              else
              {
                  GridView_Incoming_SR.EditIndex = -1;
                  GridView_Incoming_SR.DataSource = (DataTable)Session[SessionFactory.ALL_DEFECT_ALL_INCOMING_DEFECT_GRID];
                  GridView_Incoming_SR.DataBind();

                  Label_Incoming_Defect_Grid_Access.Visible = true;
                  Label_Incoming_Defect_Grid_Access.Text = "You dont have edit access to Incoming defect records";
              }*/
        }

        protected void GridView_Incoming_SR_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView_Incoming_SR.EditIndex = -1;
            GridView_Incoming_SR.DataSource = (DataTable)Session[SessionFactory.ALL_SR_ALL_INCOMING_SR_GRID];
            GridView_Incoming_SR.DataBind();
        }

        protected void GridView_Incoming_SR_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            String defectId = ((Label)GridView_Incoming_SR.Rows[e.RowIndex].Cells[0].FindControl("Label_Defect_Id")).Text;
            BackEndObjects.DefectDetails defObj = BackEndObjects.DefectDetails.getSRDetailsbyidDB(defectId);

            Dictionary<String, String> whereCls = new Dictionary<String, String>();
            whereCls.Add(BackEndObjects.DefectDetails.DEFECT_COL_DEFECT_ID, defectId);

            Dictionary<String, String> targetVals = new Dictionary<String, String>();

            int index = GridView_Incoming_SR.Rows[e.RowIndex].DataItemIndex;

            DataTable dt = (DataTable)Session[SessionFactory.ALL_SR_ALL_INCOMING_SR_GRID];
            Dictionary<String, Currency> allCurrList = (Dictionary<String, Currency>)Session[SessionFactory.CURRENCY_LIST];

            String assgnTo = "";
            String rfqNo = "";
            String invNo = "";
            String descr = "";
            String defectStat = "";
            String defectStatReason = "";
            String defectResolStat = "";
            String sev = "";
            String amount = "";
            String defectResolStatOld = "";
            String closeDateTime = "";
            String customerEntId = "";
            String curr = "";

            if (defObj.getCreationMode().Equals(BackEndObjects.DefectDetails.DEFECT_CREATION_MODE_MANUAL))
            {
                //If this defect was manually created by this entity id then allow editing the following fields
                if (defObj.getCreatedByComp().Equals(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()))
                {
                    //DropDownList_Incm_Defect_Assgn_To_Edit((TextBox)gVR.Cells[0].FindControl("")).Enabled = true;
                    rfqNo = ((TextBox)GridView_Incoming_SR.Rows[e.RowIndex].Cells[0].FindControl("TextBox_RFQ_No_Edit")).Text;
                    invNo = ((TextBox)GridView_Incoming_SR.Rows[e.RowIndex].Cells[0].FindControl("TextBox_Invoice_No_Edit")).Text;
                    descr = ((TextBox)GridView_Incoming_SR.Rows[e.RowIndex].Cells[0].FindControl("TextBox_Descr_Edit")).Text;
                    amount = ((TextBox)GridView_Incoming_SR.Rows[e.RowIndex].Cells[0].FindControl("Textbox_Edit_Amount")).Text;
                    sev = ((DropDownList)GridView_Incoming_SR.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Incm_Defect_Sev_Edit")).SelectedValue;
                    defectStat = ((DropDownList)GridView_Incoming_SR.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Incm_Defect_Stat_Edit")).SelectedValue;
                    defectStatReason = ((TextBox)GridView_Incoming_SR.Rows[e.RowIndex].Cells[0].FindControl("TextBox_Defect_Stat_Reason_Edit")).Text;
                    defectResolStat = ((DropDownList)GridView_Incoming_SR.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Incm_Defect_Resol_Stat_Edit")).SelectedValue;
                    defectResolStatOld = ((Label)GridView_Incoming_SR.Rows[e.RowIndex].Cells[0].FindControl("Label_Defect_Resol_Stat_Edit")).Text;
                    assgnTo = ((DropDownList)GridView_Incoming_SR.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Incm_Defect_Assgn_To_Edit")).SelectedValue;
                    customerEntId = ((DropDownList)GridView_Incoming_SR.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Incm_Defect_Cust_Det_Edit")).SelectedValue;

                    dt.Rows[index]["RFQId"] = rfqNo;
                    dt.Rows[index]["InvNo"] = invNo;
                    dt.Rows[index]["descr"] = descr;
                    dt.Rows[index]["amount"] = (!amount.Equals("") ? amount : "0".ToString());
                    dt.Rows[index]["Severity"] = sev;
                    dt.Rows[index]["Defect_Stat"] = defectStat;
                    dt.Rows[index]["Defect_Stat_Reason"] = defectStatReason;
                    dt.Rows[index]["Defect_Resol_Stat"] = defectResolStat;
                    dt.Rows[index]["Assigned_To"] = assgnTo;
                    dt.Rows[index]["CustName"] = ((DropDownList)GridView_Incoming_SR.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Incm_Defect_Cust_Det_Edit")).SelectedItem.Text;
                    dt.Rows[index]["curr"] = ((DropDownList)GridView_Incoming_SR.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Curr")).SelectedItem.Text;

                    if (defectResolStat.Equals(BackEndObjects.DefectResolStatCodes.DEFECT_RESOL_STAT_CODE_RESOLVED) && !defectResolStatOld.Equals(defectResolStat))
                    {
                        closeDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        dt.Rows[index]["Close Date"] = closeDateTime;
                        //Non blocking task
                        Task emailTask = Task.Factory.StartNew(() => trySendEmail(defObj.getDefectId(), defObj.getCustomerId()));
                    }
                    else if (!defectResolStat.Equals(BackEndObjects.DefectResolStatCodes.DEFECT_RESOL_STAT_CODE_RESOLVED))
                    {
                        //If the status is changed back to Non-resolved from resolved then remove the closeDateTime
                        closeDateTime = "";
                        dt.Rows[index]["Close Date"] = closeDateTime;
                    }


                    targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_RFQ_ID, rfqNo);
                    targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_INVOICE_ID, invNo);
                    targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_DESCR, descr);
                    targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_TOTAL_AMNT, (!amount.Equals("") ? amount : "0".ToString()));
                    targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_SEVERITY, sev);
                    targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_DEFECT_STAT, defectStat);
                    targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_STAT_REASON, defectStatReason);
                    targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_RESOL_STAT, defectResolStat);
                    targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_ASSGND_TO, assgnTo);
                    targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_CUSTOMER_ID, customerEntId);
                    //if (!closeDateTime.Equals(""))
                    targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_CLOSE_DATE, closeDateTime);
                    targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_CURRENCY,
                        ((DropDownList)GridView_Incoming_SR.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Curr")).SelectedValue);
                    //else
                    //targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_CLOSE_DATE, null);

                }
                else
                {
                    defectStat = ((DropDownList)GridView_Incoming_SR.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Incm_Defect_Stat_Edit")).SelectedValue;
                    defectStatReason = ((TextBox)GridView_Incoming_SR.Rows[e.RowIndex].Cells[0].FindControl("TextBox_Defect_Stat_Reason_Edit")).Text;
                    defectResolStat = ((DropDownList)GridView_Incoming_SR.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Incm_Defect_Resol_Stat_Edit")).SelectedValue;
                    assgnTo = ((DropDownList)GridView_Incoming_SR.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Incm_Defect_Assgn_To_Edit")).SelectedValue;
                    defectResolStatOld = ((Label)GridView_Incoming_SR.Rows[e.RowIndex].Cells[0].FindControl("Label_Defect_Resol_Stat_Edit")).Text;

                    dt.Rows[index]["Defect_Stat"] = defectStat;
                    dt.Rows[index]["Defect_Stat_Reason"] = defectStatReason;
                    dt.Rows[index]["Defect_Resol_Stat"] = defectResolStat;
                    dt.Rows[index]["Assigned_To"] = assgnTo;
                    if (defectResolStat.Equals(BackEndObjects.DefectResolStatCodes.DEFECT_RESOL_STAT_CODE_RESOLVED) && !defectResolStatOld.Equals(defectResolStat))
                    {
                        closeDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        dt.Rows[index]["Close Date"] = closeDateTime;
                        Task emailTask = Task.Factory.StartNew(() => trySendEmail(defObj.getDefectId(), defObj.getCustomerId()));
                    }
                    else if (!defectResolStat.Equals(BackEndObjects.DefectResolStatCodes.DEFECT_RESOL_STAT_CODE_RESOLVED))
                    {
                        //If the status is changed back to Non-resolved from resolved then remove the closeDateTime
                        closeDateTime = "";
                        dt.Rows[index]["Close Date"] = closeDateTime;
                    }

                    targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_DEFECT_STAT, defectStat);
                    targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_STAT_REASON, defectStatReason);
                    targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_RESOL_STAT, defectResolStat);
                    targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_ASSGND_TO, assgnTo);
                    //if (!closeDateTime.Equals(""))
                    targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_CLOSE_DATE, closeDateTime);
                }
            }
            else
            {
                //If this defect was auto created by this entity id then allow editing the following fields
                //Severity can only be edited by the entity who logged the defect
                if (defObj.getCreatedByComp().Equals(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()))
                {

                    descr = ((TextBox)GridView_Incoming_SR.Rows[e.RowIndex].Cells[0].FindControl("TextBox_Descr_Edit")).Text;
                    amount = ((TextBox)GridView_Incoming_SR.Rows[e.RowIndex].Cells[0].FindControl("Textbox_Edit_Amount")).Text;
                    sev = ((DropDownList)GridView_Incoming_SR.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Incm_Defect_Sev_Edit")).SelectedValue;
                    defectStat = ((DropDownList)GridView_Incoming_SR.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Incm_Defect_Stat_Edit")).SelectedValue;
                    defectStatReason = ((TextBox)GridView_Incoming_SR.Rows[e.RowIndex].Cells[0].FindControl("TextBox_Defect_Stat_Reason_Edit")).Text;
                    defectResolStat = ((DropDownList)GridView_Incoming_SR.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Incm_Defect_Resol_Stat_Edit")).SelectedValue;
                    assgnTo = ((DropDownList)GridView_Incoming_SR.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Incm_Defect_Assgn_To_Edit")).SelectedValue;
                    defectResolStatOld = ((Label)GridView_Incoming_SR.Rows[e.RowIndex].Cells[0].FindControl("Label_Defect_Resol_Stat_Edit")).Text;

                    dt.Rows[index]["descr"] = descr;
                    dt.Rows[index]["Amount"] = (!amount.Equals("") ? amount : "0".ToString()); ;
                    dt.Rows[index]["Severity"] = sev;
                    dt.Rows[index]["Defect_Stat"] = defectStat;
                    dt.Rows[index]["Defect_Stat_Reason"] = defectStatReason;
                    dt.Rows[index]["Defect_Resol_Stat"] = defectResolStat;
                    dt.Rows[index]["Assigned_To"] = assgnTo;
                    if (defectResolStat.Equals(BackEndObjects.DefectResolStatCodes.DEFECT_RESOL_STAT_CODE_RESOLVED) && !defectResolStatOld.Equals(defectResolStat))
                    {
                        closeDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        dt.Rows[index]["Close Date"] = closeDateTime;
                        Task emailTask = Task.Factory.StartNew(() => trySendEmail(defObj.getDefectId(), defObj.getCustomerId()));
                    }
                    else if (!defectResolStat.Equals(BackEndObjects.DefectResolStatCodes.DEFECT_RESOL_STAT_CODE_RESOLVED))
                    {
                        //If the status is changed back to Non-resolved from resolved then remove the closeDateTime
                        closeDateTime = "";
                        dt.Rows[index]["Close Date"] = closeDateTime;
                    }

                    targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_DESCR, descr);
                    targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_TOTAL_AMNT, (!amount.Equals("") ? amount : "0".ToString()));
                    targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_SEVERITY, sev);
                    targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_DEFECT_STAT, defectStat);
                    targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_STAT_REASON, defectStatReason);
                    targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_RESOL_STAT, defectResolStat);
                    targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_ASSGND_TO, assgnTo);
                    //if (!closeDateTime.Equals(""))
                    targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_CLOSE_DATE, closeDateTime);

                }
                else
                {
                    defectStat = ((DropDownList)GridView_Incoming_SR.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Incm_Defect_Stat_Edit")).SelectedValue;
                    defectStatReason = ((TextBox)GridView_Incoming_SR.Rows[e.RowIndex].Cells[0].FindControl("TextBox_Defect_Stat_Reason_Edit")).Text;
                    defectResolStat = ((DropDownList)GridView_Incoming_SR.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Incm_Defect_Resol_Stat_Edit")).SelectedValue;
                    assgnTo = ((DropDownList)GridView_Incoming_SR.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Incm_Defect_Assgn_To_Edit")).SelectedValue;
                    defectResolStatOld = ((Label)GridView_Incoming_SR.Rows[e.RowIndex].Cells[0].FindControl("Label_Defect_Resol_Stat_Edit")).Text;

                    dt.Rows[index]["Defect_Stat"] = defectStat;
                    dt.Rows[index]["Defect_Stat_Reason"] = defectStatReason;
                    dt.Rows[index]["Defect_Resol_Stat"] = defectResolStat;
                    dt.Rows[index]["Assigned_To"] = assgnTo;
                    if (defectResolStat.Equals(BackEndObjects.DefectResolStatCodes.DEFECT_RESOL_STAT_CODE_RESOLVED) && !defectResolStatOld.Equals(defectResolStat))
                    {
                        closeDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        dt.Rows[index]["Close Date"] = closeDateTime;

                        //Try to send email update
                        Task emailTask = Task.Factory.StartNew(() => trySendEmail(defObj.getDefectId(), defObj.getCustomerId()));
                    }
                    else if (!defectResolStat.Equals(BackEndObjects.DefectResolStatCodes.DEFECT_RESOL_STAT_CODE_RESOLVED))
                    {
                        //If the status is changed back to Non-resolved from resolved then remove the closeDateTime
                        closeDateTime = "";
                        dt.Rows[index]["Close Date"] = closeDateTime;
                    }

                    targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_DEFECT_STAT, defectStat);
                    targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_STAT_REASON, defectStatReason);
                    targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_RESOL_STAT, defectResolStat);
                    targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_ASSGND_TO, assgnTo);
                    //if (!closeDateTime.Equals(""))
                    targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_CLOSE_DATE, closeDateTime);

                }
            }

            try
            {
                BackEndObjects.DefectDetails.updateDefectDetails(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
                GridView_Incoming_SR.EditIndex = -1;
                GridView_Incoming_SR.DataSource = dt;
                GridView_Incoming_SR.DataBind();

            }
            catch (Exception ex)
            {

            }
        }

        protected void loadContactList()
        {
            Dictionary<String, String> contactEmailList = (Dictionary<String, String>)ViewState["ReolveSRContactEmailList"];

            if (contactEmailList == null || contactEmailList.Count == 0)
            {
                contactEmailList = new Dictionary<string, string>();
                ArrayList contactObjList = BackEndObjects.Contacts.getAllContactsbyEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                //ViewState["contactObjectListDefect"] = contactObjList;

                for (int i = 0; i < contactObjList.Count; i++)
                {
                    ListItem lt = new ListItem();
                    String contactEntId = ((BackEndObjects.Contacts)contactObjList[i]).getContactEntityId();

                    if (!contactEmailList.ContainsKey(contactEntId))
                        contactEmailList.Add(contactEntId, ((BackEndObjects.Contacts)contactObjList[i]).getEmailId());
                }
                ViewState["ReolveSRContactEmailList"] = contactEmailList;
            }

        }
        protected void trySendEmail(String defId, String entId)
        {
            loadContactList();
            Dictionary<String, String> contactEmailList = new Dictionary<string, string>();

            MainBusinessEntity mBEObj = MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            contactEmailList = (Dictionary<String, String>)ViewState["ReolveSRContactEmailList"];
            if (mBEObj.getSupportEmail() != null && !mBEObj.getSupportEmail().Equals("") && contactEmailList != null)
            {
                try
                {
                    ActionLibrary.Emails.sendEmail(
                        mBEObj.getSupportEmail(), mBEObj.getSupportEmailPass(),
                        contactEmailList[entId],
                        "SR Resolved with SR id#" + defId,
                        mBEObj.getResolvedSRBody());
                }
                catch (Exception ex)
                {
                }
            }

        }

        protected void GridView_Outgoing_SR_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView_Outgoing_SR.EditIndex = -1;
            GridView_Outgoing_SR.DataSource = (DataTable)Session[SessionFactory.ALL_SR_ALL_OUTGOING_SR_GRID];
            GridView_Outgoing_SR.DataBind();
        }

        protected void GridView_Outgoing_SR_RowEditing(object sender, GridViewEditEventArgs e)
        {
            /*Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

            if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_EDIT_OUTGOING_DEFECT]||
                accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
            {*/
            Label_Outgoing_Defect_Grid_Access.Visible = false;
            GridView_Outgoing_SR.EditIndex = e.NewEditIndex;
            GridView_Outgoing_SR.DataSource = (DataTable)Session[SessionFactory.ALL_SR_ALL_OUTGOING_SR_GRID];
            GridView_Outgoing_SR.DataBind();
            Label_Outg_msg.Visible = true;
            Label_Outg_msg.Text = "You can edit all the fields only if the SR is created by your organization";
            Label_Outg_msg.ForeColor = System.Drawing.Color.Green;
            /*}
            else
            {
                Label_Outgoing_Defect_Grid_Access.Visible = true;
                Label_Outgoing_Defect_Grid_Access.Text = "You dont have edit access to Outgoing defect records";

                GridView_Outgoing_SR.EditIndex = -1;
                GridView_Outgoing_SR.DataSource = (DataTable)Session[SessionFactory.ALL_DEFECT_ALL_OUTGOING_DEFECT_GRID];
                GridView_Outgoing_SR.DataBind();
            }*/
        }

        protected void GridView_Outgoing_SR_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            String defectId = ((Label)GridView_Outgoing_SR.Rows[e.RowIndex].Cells[0].FindControl("Label_Defect_Id")).Text;
            BackEndObjects.DefectDetails defObj = BackEndObjects.DefectDetails.getSRDetailsbyidDB(defectId);

            Dictionary<String, String> whereCls = new Dictionary<String, String>();
            whereCls.Add(BackEndObjects.DefectDetails.DEFECT_COL_DEFECT_ID, defectId);

            Dictionary<String, String> targetVals = new Dictionary<String, String>();

            int index = GridView_Outgoing_SR.Rows[e.RowIndex].DataItemIndex;

            DataTable dt = (DataTable)Session[SessionFactory.ALL_SR_ALL_OUTGOING_SR_GRID];

            //String assgnTo = "";
            String rfqNo = "";
            String invNo = "";
            String descr = "";
            String defectStat = "";
            String defectStatReason = "";
            String defectResolStatOld = "";
            String defectResolStat = "";
            String sev = "";
            String amount = "";
            String closeDateTime = "";
            String suplEntId = "";

            if (defObj.getCreationMode().Equals(BackEndObjects.DefectDetails.DEFECT_CREATION_MODE_MANUAL))
            {
                //If this defect was manually created by this entity id then allow editing the following fields
                // if (defObj.getCreatedByComp().Equals(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()))
                //{
                //DropDownList_Incm_Defect_Assgn_To_EdittargetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_DESCR, descr);
                rfqNo = ((TextBox)GridView_Outgoing_SR.Rows[e.RowIndex].Cells[0].FindControl("TextBox_RFQ_No_Edit")).Text;
                invNo = ((TextBox)GridView_Outgoing_SR.Rows[e.RowIndex].Cells[0].FindControl("TextBox_Invoice_No_Edit")).Text;
                descr = ((TextBox)GridView_Outgoing_SR.Rows[e.RowIndex].Cells[0].FindControl("TextBox_Descr_Edit")).Text;
                amount = ((TextBox)GridView_Outgoing_SR.Rows[e.RowIndex].Cells[0].FindControl("Textbox_Edit_Amount")).Text;
                sev = ((DropDownList)GridView_Outgoing_SR.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Outgoing_Defect_Sev")).SelectedValue;
                defectStat = ((DropDownList)GridView_Outgoing_SR.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Outgoing_Defect_Stat_Edit")).SelectedValue;
                defectStatReason = ((TextBox)GridView_Outgoing_SR.Rows[e.RowIndex].Cells[0].FindControl("TextBox_Defect_Stat_Reason_Edit")).Text;
                defectResolStat = ((DropDownList)GridView_Outgoing_SR.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Outgoing_Defect_Resol_Stat_Edit")).SelectedValue;
                defectResolStatOld = ((Label)GridView_Outgoing_SR.Rows[e.RowIndex].Cells[0].FindControl("Label_Defect_Resol_Stat_Edit")).Text;
                suplEntId = ((DropDownList)GridView_Outgoing_SR.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Outgoing_Defect_Supplier_Edit")).SelectedValue;
                //assgnTo = ((DropDownList)GridView_Outgoing_SR.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Outgoing_Defect_Assgn_To_Edit")).SelectedValue;


                dt.Rows[index]["RFQId"] = rfqNo;
                dt.Rows[index]["InvNo"] = invNo;
                dt.Rows[index]["descr"] = descr;
                dt.Rows[index]["Amount"] = (!amount.Equals("") ? amount : "0".ToString());
                dt.Rows[index]["Severity"] = sev;
                dt.Rows[index]["Defect_Stat"] = defectStat;
                dt.Rows[index]["Defect_Stat_Reason"] = defectStatReason;
                dt.Rows[index]["Defect_Resol_Stat"] = defectResolStat;
                dt.Rows[index]["SuplName"] = ((DropDownList)GridView_Outgoing_SR.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Outgoing_Defect_Supplier_Edit")).SelectedItem.Text; ;
                dt.Rows[index]["curr"] = ((DropDownList)GridView_Outgoing_SR.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Curr")).SelectedItem.Text;

                //dt.Rows[index]["Assigned_To"] = assgnTo;
                if (defectResolStat.Equals(BackEndObjects.DefectResolStatCodes.DEFECT_RESOL_STAT_CODE_RESOLVED) && !defectResolStatOld.Equals(defectResolStat))
                {
                    closeDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    dt.Rows[index]["Close Date"] = closeDateTime;
                }
                else if (!defectResolStat.Equals(BackEndObjects.DefectResolStatCodes.DEFECT_RESOL_STAT_CODE_RESOLVED))
                {
                    //If the status is changed back to Non-resolved from resolved then remove the closeDateTime
                    closeDateTime = "";
                    dt.Rows[index]["Close Date"] = closeDateTime;
                }

                targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_RFQ_ID, rfqNo);
                targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_INVOICE_ID, invNo);
                targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_DESCR, descr);
                targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_TOTAL_AMNT, (!amount.Equals("") ? amount : "0".ToString()));
                targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_SEVERITY, sev);
                targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_DEFECT_STAT, defectStat);
                targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_STAT_REASON, defectStatReason);
                targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_RESOL_STAT, defectResolStat);
                targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_SUPPLIER_ID, suplEntId);
                //targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_ASSGND_TO, assgnTo);
                //if(!closeDateTime.Equals(""))
                targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_CLOSE_DATE, closeDateTime);
                targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_CURRENCY,
                    ((DropDownList)GridView_Outgoing_SR.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Curr")).SelectedValue);

                //  }
            }
            else
            {
                //If this defect was auto created by this entity id then allow editing the following fields
                //Severity can only be edited by the entity who logged the defect
                //if (defObj.getCreatedByComp().Equals(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()))
                //{

                descr = ((TextBox)GridView_Outgoing_SR.Rows[e.RowIndex].Cells[0].FindControl("TextBox_Descr_Edit")).Text;
                amount = ((TextBox)GridView_Outgoing_SR.Rows[e.RowIndex].Cells[0].FindControl("Textbox_Edit_Amount")).Text;
                sev = ((DropDownList)GridView_Outgoing_SR.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Outgoing_Defect_Sev")).SelectedValue;
                defectStat = ((DropDownList)GridView_Outgoing_SR.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Outgoing_Defect_Stat_Edit")).SelectedValue;
                defectStatReason = ((TextBox)GridView_Outgoing_SR.Rows[e.RowIndex].Cells[0].FindControl("TextBox_Defect_Stat_Reason_Edit")).Text;
                defectResolStat = ((DropDownList)GridView_Outgoing_SR.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Outgoing_Defect_Resol_Stat_Edit")).SelectedValue;
                defectResolStatOld = ((Label)GridView_Outgoing_SR.Rows[e.RowIndex].Cells[0].FindControl("Label_Defect_Resol_Stat_Edit")).Text;
                //assgnTo = ((DropDownList)GridView_Outgoing_SR.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Outgoing_Defect_Assgn_To_Edit")).SelectedValue;

                dt.Rows[index]["descr"] = descr;
                dt.Rows[index]["Amount"] = (!amount.Equals("") ? amount : "0".ToString());
                dt.Rows[index]["Severity"] = sev;
                dt.Rows[index]["Defect_Stat"] = defectStat;
                dt.Rows[index]["Defect_Stat_Reason"] = defectStatReason;
                dt.Rows[index]["Defect_Resol_Stat"] = defectResolStat;
                //dt.Rows[index]["Assigned_To"] = assgnTo;
                if (defectResolStat.Equals(BackEndObjects.DefectResolStatCodes.DEFECT_RESOL_STAT_CODE_RESOLVED) && !defectResolStatOld.Equals(defectResolStat))
                {
                    closeDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    dt.Rows[index]["Close Date"] = closeDateTime;
                }
                else if (!defectResolStat.Equals(BackEndObjects.DefectResolStatCodes.DEFECT_RESOL_STAT_CODE_RESOLVED))
                {
                    //If the status is changed back to Non-resolved from resolved then remove the closeDateTime
                    closeDateTime = "";
                    dt.Rows[index]["Close Date"] = closeDateTime;
                }

                targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_DESCR, descr);
                targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_TOTAL_AMNT, (!amount.Equals("") ? amount : "0".ToString()));
                targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_SEVERITY, sev);
                targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_DEFECT_STAT, defectStat);
                targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_STAT_REASON, defectStatReason);
                targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_RESOL_STAT, defectResolStat);
                //targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_ASSGND_TO, assgnTo);
                //if (!closeDateTime.Equals(""))
                targetVals.Add(BackEndObjects.DefectDetails.DEFECT_COL_CLOSE_DATE, closeDateTime);

                // }

            }

            try
            {
                BackEndObjects.DefectDetails.updateDefectDetails(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
                GridView_Outgoing_SR.EditIndex = -1;
                GridView_Outgoing_SR.DataSource = dt;
                GridView_Outgoing_SR.DataBind();

            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView_Outgoing_SR_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                GridViewRow gVR = e.Row;

                ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Outgoing_Defect_Stat_Edit")).Visible = true;
                ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Outgoing_Defect_Resol_Stat_Edit")).Visible = true;

                ArrayList defectStatList = BackEndObjects.DefectStatCodes.getAllDefectStatCodesDB();
                ArrayList defectResolStatList = BackEndObjects.DefectResolStatCodes.getAllDefectResolStatDB();

                //Dictionary<String, userDetails> allUserDetails = BackEndObjects.MainBusinessEntity.getUserDetailsforMainEntitybyIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

                DropDownList DropDownList_Outgoing_Defect_Stat = (DropDownList)gVR.Cells[0].FindControl("DropDownList_Outgoing_Defect_Stat_Edit");
                DropDownList DropDownList_Outgoing_Defect_Resol_Stat = (DropDownList)gVR.Cells[0].FindControl("DropDownList_Outgoing_Defect_Resol_Stat_Edit");
                DropDownList DropDownList_Outgoing_Defect_Assgn_To = (DropDownList)gVR.Cells[0].FindControl("DropDownList_Outgoing_Defect_Assgn_To_Edit");

                loadDefectSev((DropDownList)gVR.Cells[0].FindControl("DropDownList_Outgoing_Defect_Sev"),
                    ((Label)gVR.Cells[0].FindControl("Label_Sev_Edit")).Text);

                loadContacts((DropDownList)gVR.Cells[0].FindControl("DropDownList_Outgoing_Defect_Supplier_Edit"),
                    ((Label)gVR.Cells[0].FindControl("Label_EntId_Edit")).Text);

                for (int i = 0; i < defectStatList.Count; i++)
                {
                    ListItem lt = new ListItem();
                    lt.Text = ((BackEndObjects.DefectStatCodes)defectStatList[i]).getDefectStat();
                    lt.Value = ((BackEndObjects.DefectStatCodes)defectStatList[i]).getDefectStat();

                    DropDownList_Outgoing_Defect_Stat.Items.Add(lt);
                }

                for (int i = 0; i < defectResolStatList.Count; i++)
                {
                    ListItem lt = new ListItem();
                    lt.Text = ((BackEndObjects.DefectResolStatCodes)defectResolStatList[i]).getResolStat();
                    lt.Value = ((BackEndObjects.DefectResolStatCodes)defectResolStatList[i]).getResolStat();

                    DropDownList_Outgoing_Defect_Resol_Stat.Items.Add(lt);
                }

                /* foreach (KeyValuePair<String, userDetails> kvp in allUserDetails)
                 {
                     BackEndObjects.userDetails userDetObj = kvp.Value;

                     ListItem lt = new ListItem();
                     lt.Text = userDetObj.getName();
                     lt.Value = userDetObj.getName();

                     DropDownList_Outgoing_Defect_Assgn_To.Items.Add(lt);
                 }*/

                DropDownList_Outgoing_Defect_Stat.SelectedValue = ((Label)gVR.Cells[0].FindControl("Label_Defect_Stat_Edit_Outgoing")).Text;
                DropDownList_Outgoing_Defect_Resol_Stat.SelectedValue = ((Label)gVR.Cells[0].FindControl("Label_Defect_Resol_Stat_Edit")).Text;

                // if (!((Label)gVR.Cells[0].FindControl("Label_Assgn_To_Edit")).Text.Equals(""))
                //DropDownList_Outgoing_Defect_Assgn_To.SelectedValue = ((Label)gVR.Cells[0].FindControl("Label_Assgn_To_Edit")).Text;


                String defectId = ((Label)gVR.Cells[0].FindControl("Label_Defect_Id")).Text;
                BackEndObjects.DefectDetails defObj = BackEndObjects.DefectDetails.getSRDetailsbyidDB(defectId);

                if (defObj.getCreationMode().Equals(BackEndObjects.DefectDetails.DEFECT_CREATION_MODE_MANUAL))
                {
                    //If this defect was manually created by this entity id then allow editing the following fields
                    //if (defObj.getCreatedByComp().Equals(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()))
                    //{

                    ((TextBox)gVR.Cells[0].FindControl("TextBox_RFQ_No_Edit")).Enabled = true;
                    ((TextBox)gVR.Cells[0].FindControl("TextBox_Invoice_No_Edit")).Enabled = true;
                    ((TextBox)gVR.Cells[0].FindControl("TextBox_Descr_Edit")).Enabled = true;
                    ((TextBox)gVR.Cells[0].FindControl("Textbox_Edit_Amount")).Enabled = true;
                    ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Outgoing_Defect_Sev")).Visible = true;
                    ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Outgoing_Defect_Stat_Edit")).Visible = true;
                    ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Outgoing_Defect_Resol_Stat_Edit")).Visible = true;

                    loadCurrency(((DropDownList)gVR.FindControl("DropDownList_Curr")), ((Label)gVR.FindControl("Label_Curr_Edit")).Text);

                    //}
                }
                else
                {
                    //if (defObj.getCreatedByComp().Equals(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()))
                    //{
                    ((TextBox)gVR.Cells[0].FindControl("TextBox_Descr_Edit")).Enabled = true;
                    ((TextBox)gVR.Cells[0].FindControl("Textbox_Edit_Amount")).Enabled = true;
                    ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Outgoing_Defect_Sev")).Visible = true;
                    ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Outgoing_Defect_Stat_Edit")).Visible = true;
                    ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Outgoing_Defect_Resol_Stat_Edit")).Visible = true;

                    loadCurrency(((DropDownList)gVR.FindControl("DropDownList_Curr")), ((Label)gVR.FindControl("Label_Curr_Edit")).Text);
                    //}
                }
            }
        }


        protected void Button_Inc_Defect_Refresh_Click(object sender, EventArgs e)
        {
            loadIncomingDefectList(null);
            GridView_Incoming_SR.SelectedIndex = -1;
            disableOnPageChange("incm");
            Button_Inc_Defect_Refresh.Focus();
        }

        protected void Button_Out_Defect_Refresh_Click(object sender, EventArgs e)
        {
            loadOutgoingDefectList(null);
            GridView_Outgoing_SR.SelectedIndex = -1;
            disableOnPageChange("outg");
            Button_Out_Defect_Refresh.Focus();
        }

        protected void Button_Audit_Incm_Defect_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/DispAudit.aspx";
            forwardString += "?contextId1=" + ((Label)GridView_Incoming_SR.SelectedRow.Cells[0].FindControl("Label_Defect_Id")).Text;
            forwardString += "&contextId2=" + "";
            forwardString += "&contextId3=" + "";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispIncmSRAudit", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=700');", true);

        }

        protected void Button_Audit_Outg_Defect_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/DispAudit.aspx";
            forwardString += "?contextId1=" + ((Label)GridView_Outgoing_SR.SelectedRow.Cells[0].FindControl("Label_Defect_Id")).Text;
            forwardString += "&contextId2=" + "";
            forwardString += "&contextId3=" + "";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispOutgSRAudit", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=700');", true);

        }

        protected void enableOnSelect(String senderName)
        {
            switch (senderName)
            {
                case "incm": Button_Audit_Incm_Defect.Enabled = true;
                    Button_Incm_Doc.Enabled = true;
                    Button_Notes_Incm_Def.Enabled = true;
                    break;
                case "outg": Button_Audit_Outg_Defect.Enabled = true;
                    Button_Outg_Doc.Enabled = true;
                    Button_Notes_Outg_Def.Enabled = true;
                    break;
            }
        }

        protected void disableOnPageChange(String senderName)
        {
            switch (senderName)
            {
                case "incm": Button_Audit_Incm_Defect.Enabled = false;
                    Button_Incm_Doc.Enabled = false;
                    Button_Notes_Incm_Def.Enabled = false;
                    break;
                case "outg": Button_Audit_Outg_Defect.Enabled = false;
                    Button_Outg_Doc.Enabled = false;
                    Button_Notes_Outg_Def.Enabled = false;
                    break;
            }
        }

        protected void GridView_Incoming_SR_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session[SessionFactory.ALL_SR_INCM_SELECTED_CUSTOMER_OBJ] =
ActionLibrary.customerDetails.getContactDetails(((Label)GridView_Incoming_SR.SelectedRow.Cells[0].FindControl("Label_EntId")).Text,
Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            ((RadioButton)GridView_Incoming_SR.SelectedRow.Cells[0].FindControl("incm_radio")).Checked = true;
            enableOnSelect("incm");
        }

        protected void GridView_Outgoing_SR_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session[SessionFactory.ALL_SR_OUTG_SELECTED_SUPPLIER_OBJ] =
ActionLibrary.customerDetails.getContactDetails(((Label)GridView_Outgoing_SR.SelectedRow.Cells[0].FindControl("Label_EntId")).Text,
Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            ((RadioButton)GridView_Outgoing_SR.SelectedRow.Cells[0].FindControl("outg_radio")).Checked = true;
            enableOnSelect("outg");
        }

        protected void Button_Incm_Doc_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/Popups/Defects/AllDocs_Defect.aspx";
            String docName = ((Label)GridView_Incoming_SR.SelectedRow.Cells[0].FindControl("Label_Doc_Name_Hidden")).Text;
            String defId = ((Label)GridView_Incoming_SR.SelectedRow.Cells[0].FindControl("Label_Defect_Id")).Text;

            forwardString += "?contextId=" + "incoming";
            forwardString += "&contextId1=" + defId;
            forwardString += "&contextId2=" + docName;
            forwardString += "&dataItemIndex=" + GridView_Incoming_SR.SelectedRow.DataItemIndex;


            ScriptManager.RegisterStartupScript(this, typeof(string), "IncmSRDoc",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=900,Height=400');", true);

        }

        protected void Button_Outg_Doc_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/Popups/Defects/AllDocs_Defect.aspx";
            String docName = ((Label)GridView_Outgoing_SR.SelectedRow.Cells[0].FindControl("Label_Doc_Name_Hidden")).Text;
            String defId = ((Label)GridView_Outgoing_SR.SelectedRow.Cells[0].FindControl("Label_Defect_Id")).Text;

            forwardString += "?contextId=" + "outgoing";
            forwardString += "&contextId1=" + defId;
            forwardString += "&contextId2=" + docName;
            forwardString += "&dataItemIndex=" + GridView_Outgoing_SR.SelectedRow.DataItemIndex;

            ScriptManager.RegisterStartupScript(this, typeof(string), "OutgSRDoc",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=900,Height=400');", true);

        }

        protected void LinkButton_Customer_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_Incoming_SR.SelectedIndex)
                GridView_Incoming_SR.SelectRow(row.RowIndex);

            ((RadioButton)GridView_Incoming_SR.SelectedRow.Cells[0].FindControl("incm_radio")).Checked = true;

            String forwardString = "/Pages/Popups/Defects/AllDefectsContacts.aspx";
            forwardString += "?context=" + "incomingSR";

            ScriptManager.RegisterStartupScript(this, typeof(string), "IncmSRContact",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=400');", true);
        }

        protected void LinkButton_Supplier_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_Outgoing_SR.SelectedIndex)
                GridView_Outgoing_SR.SelectRow(row.RowIndex);

            ((RadioButton)GridView_Outgoing_SR.SelectedRow.Cells[0].FindControl("outg_radio")).Checked = true;

            String forwardString = "/Pages/Popups/Defects/AllDefectsContacts.aspx";
            forwardString += "?context=" + "outgoingSR";

            ScriptManager.RegisterStartupScript(this, typeof(string), "OutgSRContact",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=400');", true);
        }

        protected void Button_Notes_Outg_Def_Click(object sender, EventArgs e)
        {
            String forwardString = "NoteDetails.aspx";

            String rfqId = ((Label)GridView_Outgoing_SR.SelectedRow.Cells[0].FindControl("Label_Defect_Id")).Text;

            forwardString += "?contextId=" + rfqId;

            ScriptManager.RegisterStartupScript(this, typeof(string), "OutgSRNotes",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=800,height=800,left=500,right=500');", true);
        }

        protected void Button_Notes_Incm_Def_Click(object sender, EventArgs e)
        {
            String forwardString = "NoteDetails.aspx";

            String rfqId = ((Label)GridView_Incoming_SR.SelectedRow.Cells[0].FindControl("Label_Defect_Id")).Text;

            forwardString += "?contextId=" + rfqId;

            ScriptManager.RegisterStartupScript(this, typeof(string), "IncmSRNotes",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=800,height=800,left=500,right=500');", true);

        }

        protected void bindSortedData(GridView grd, DataTable dt)
        {
            grd.DataSource = dt;
            grd.DataBind();
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

        private SortDirection GridViewSortDirectionOutg
        {
            get
            {
                if (ViewState["sortDirectionOutg"] == null)
                    ViewState["sortDirectionOutg"] = SortDirection.Descending;
                return (SortDirection)ViewState["sortDirectionOutg"];
            }
            set { ViewState["sortDirectionOutg"] = value; }
        }

        protected void GridView_Incoming_SR_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            ViewState["SortExpressionIncm"] = sortExpression;
            DataTable dt = (DataTable)Session[SessionFactory.ALL_SR_ALL_INCOMING_SR_GRID];

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
            Session[SessionFactory.ALL_SR_ALL_INCOMING_SR_GRID] = sortedTable;

            disableOnPageChange("incm");
            GridView_Incoming_SR.SelectedIndex = -1;
            bindSortedData(GridView_Incoming_SR, sortedTable);
        }

        protected void GridView_Outgoing_SR_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            ViewState["SortExpressionOutg"] = sortExpression;
            DataTable dt = (DataTable)Session[SessionFactory.ALL_SR_ALL_OUTGOING_SR_GRID];

            if (GridViewSortDirectionOutg == SortDirection.Ascending)
            {
                GridViewSortDirectionOutg = SortDirection.Descending;
                dt.DefaultView.Sort = sortExpression + " " + "DESC";
            }
            else
            {
                GridViewSortDirectionOutg = SortDirection.Ascending;
                dt.DefaultView.Sort = sortExpression + " " + "ASC";
            }

            DataTable sortedTable = dt.DefaultView.ToTable();
            Session[SessionFactory.ALL_SR_ALL_OUTGOING_SR_GRID] = sortedTable;
            GridView_Outgoing_SR.SelectedIndex = -1;
            disableOnPageChange("outg");
            bindSortedData(GridView_Outgoing_SR, sortedTable);

        }

        protected void LinkButton_Assgn_To_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_Incoming_SR.SelectedIndex)
                GridView_Incoming_SR.SelectRow(row.RowIndex);

            ((RadioButton)GridView_Incoming_SR.SelectedRow.Cells[0].FindControl("incm_radio")).Checked = true;

            String forwardString = "DispUserDetails.aspx";
            //window.open('Popups/Sale/AllPotn_NDA.aspx','DispNDAPot','status=1,toolbar=1,width=500,height=200,left=500,right=500',true);
            String userId = ((LinkButton)GridView_Incoming_SR.SelectedRow.Cells[0].FindControl("LinkButton_Assgn_To")).Text;

            forwardString += "?userId=" + userId;

            ScriptManager.RegisterStartupScript(this, typeof(string), "userDetIncmSR",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=400,height=400,left=500,right=500');", true);

        }

        protected void Button_Outg_Refresh_Hidden_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session[SessionFactory.ALL_SR_ALL_OUTGOING_SR_GRID];
            GridView_Outgoing_SR.DataSource = dt;
            GridView_Outgoing_SR.DataBind();
            GridView_Outgoing_SR.SelectedIndex = -1;
            disableOnPageChange("outg");

        }

        protected void Button_Incm_Refresh_Hidden_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session[SessionFactory.ALL_SR_ALL_INCOMING_SR_GRID];
            GridView_Incoming_SR.DataSource = dt;
            GridView_Incoming_SR.DataBind();
            GridView_Incoming_SR.SelectedIndex = -1;
            disableOnPageChange("incm");

        }

        protected void GridView_Defect_RadioSelect(object sender, EventArgs e)
        {
            RadioButton linkb = (RadioButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1)
                GridView_Incoming_SR.SelectRow(row.RowIndex);
        }


        protected void GridView_Out_Defect_RadioSelect(object sender, EventArgs e)
        {
            RadioButton linkb = (RadioButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1)
                GridView_Outgoing_SR.SelectRow(row.RowIndex);
        }

        protected void LinkButton_All_Comm_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_Incoming_SR.SelectedIndex)
                GridView_Incoming_SR.SelectRow(row.RowIndex);

            ((RadioButton)GridView_Incoming_SR.SelectedRow.Cells[0].FindControl("incm_radio")).Checked = true;

            String sourceEnt = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();

            String forwardString = "/Pages/DispComm.aspx";
            forwardString += "?contextId=" + ((Label)GridView_Incoming_SR.SelectedRow.Cells[0].FindControl("Label_Defect_Id")).Text +
                "&source=" + sourceEnt;

            ScriptManager.RegisterStartupScript(this, typeof(string), "CommIncmSR",
    "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=800,height=800,left=500,right=500');", true);

        }

        protected void LinkButton_All_Comm_Outg_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_Outgoing_SR.SelectedIndex)
                GridView_Outgoing_SR.SelectRow(row.RowIndex);

            ((RadioButton)GridView_Outgoing_SR.SelectedRow.Cells[0].FindControl("outg_radio")).Checked = true;

            String sourceEnt = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();

            String forwardString = "/Pages/DispComm.aspx";
            forwardString += "?contextId=" + ((Label)GridView_Outgoing_SR.SelectedRow.Cells[0].FindControl("Label_Defect_Id")).Text +
                "&source=" + sourceEnt;

            ScriptManager.RegisterStartupScript(this, typeof(string), "CommOutgSR",
   "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=800,height=800,left=500,right=500');", true);
        }

        protected void Link_Descr_Short_Click(object sender, EventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_Incoming_SR.SelectedIndex)
                GridView_Incoming_SR.SelectRow(row.RowIndex);

            ((RadioButton)GridView_Incoming_SR.SelectedRow.Cells[0].FindControl("incm_radio")).Checked = true;

            Panel_Description.Visible = true;
            Label_Description.Text = ((LinkButton)GridView_Incoming_SR.SelectedRow.Cells[0].FindControl("Link_Descr")).Text;
        }

        protected void Button_Hide_Click(object sender, EventArgs e)
        {
            Panel_Description.Visible = false;
        }

        protected void Link_Descr_Short_Click_Outg(object sender, EventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_Outgoing_SR.SelectedIndex)
                GridView_Outgoing_SR.SelectRow(row.RowIndex);

            ((RadioButton)GridView_Outgoing_SR.SelectedRow.Cells[0].FindControl("outg_radio")).Checked = true;

            Panel_Description.Visible = true;
            Label_Description.Text = ((LinkButton)GridView_Outgoing_SR.SelectedRow.Cells[0].FindControl("Link_Descr")).Text;
        }

    }
}