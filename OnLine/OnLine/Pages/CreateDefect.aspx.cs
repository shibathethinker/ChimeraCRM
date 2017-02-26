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

namespace OnLine.Pages
{
    public partial class CreateDefect : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                loadDefectStatAndResolStat();
                loadContacts();
                loadDefectSev();
                loadCurrency();

                ArrayList rfqList = null; 
                ArrayList invList=null;

                String context = Request.QueryString.GetValues("context")[0];

                if (context.Equals("incoming"))//Invoice list created by this entity id
                {
                    rfqList = RFQDetails.getAllLeadsIncludingConvertedtoPotentialforEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                    invList = BackEndObjects.Invoice.getAllInvoicesbyRespEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                }
                else//Invoice list sent to this entity id
                {
                    rfqList = BackEndObjects.RFQDetails.getAllRFQbyEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), true);
                    invList = BackEndObjects.Invoice.getAllInvoicesbyEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                }

                if(rfqList!=null)
                {
                String[] rfqNameArray=new String[rfqList.Count];
                for (int i = 0; i < rfqList.Count; i++)
                    rfqNameArray[i] = ((RFQDetails)rfqList[i]).getRFQName()+"("+((RFQDetails)rfqList[i]).getRFQId()+")";

                Session[SessionFactory.ALL_DEFECT_CREATE_DEFECT_RFQ_AUTOCOMPLETE_LIST] = rfqNameArray;
                }
                if (invList != null)
                {
                    String[] invNameArray = new String[invList.Count];
                    Dictionary<String, String> invAmount = new Dictionary<String, String>();

                    for (int i = 0; i < invList.Count; i++)
                    {
                        invNameArray[i] = ((Invoice)invList[i]).getInvoiceId();
                        invAmount.Add(invNameArray[i], ((Invoice)invList[i]).getTotalAmount().ToString());
                    }
                    
                    Session[SessionFactory.ALL_DEFECT_CREATE_DEFECT_INV_AUTOCOMPLETE_LIST] = invNameArray;
                    Session[SessionFactory.ALL_DEFECT_CREATE_DEFECT_INV_AMNT_DICT] = invAmount;
                }

                if (context.Equals("incoming"))
                    Panel_Contacts.GroupingText = "Enter Customer Details";
                else
                    Panel_Contacts.GroupingText = "Enter Vendor Details";
                
            }
        }

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static String[] GetCompletionListRFQ(string prefixText,int count)
        {
            String[] rfqNameArray=((String[])HttpContext.Current.Session[SessionFactory.ALL_DEFECT_CREATE_DEFECT_RFQ_AUTOCOMPLETE_LIST]);
            
            ArrayList temp = new ArrayList();
            foreach (String elm in rfqNameArray)
            {                
                if (elm.StartsWith(prefixText.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    temp.Add(elm);
            }

            return (String[])temp.ToArray(typeof(String));
        }

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static String[] GetCompletionListInvoice(string prefixText, int count)
        {
            String[] invIdArray = ((String[])HttpContext.Current.Session[SessionFactory.ALL_DEFECT_CREATE_DEFECT_INV_AUTOCOMPLETE_LIST]);

            ArrayList temp = new ArrayList();
            foreach (String elm in invIdArray)
            {
                if (elm.StartsWith(prefixText.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    temp.Add(elm);
            }
            
            return (String[])temp.ToArray(typeof(String));
        }

        protected void loadCurrency()
        {
            Dictionary<String, Currency> allCurrList = (Dictionary<String, Currency>)Session[SessionFactory.CURRENCY_LIST];
            String defaultCurr = Session[SessionFactory.MAIN_BUSINESS_ENTITY_DEFAULT_CURRENCY].ToString();

            foreach (KeyValuePair<String, Currency> kvp in allCurrList)
            {
                ListItem lt = new ListItem();
                lt.Text = kvp.Value.getCurrencyName();
                lt.Value = kvp.Key;

                DropDownList_Curr.Items.Add(lt);
                if (defaultCurr.Equals(lt.Value.Trim()))
                    DropDownList_Curr.SelectedValue = lt.Value;
            }
        }

        protected void hdnValue_ValueChangedRFQ(object sender, EventArgs e)
        {
            string selectedVal = ((HiddenField)sender).Value;

            String[] rfqArray = (String[])Session[SessionFactory.ALL_DEFECT_CREATE_DEFECT_RFQ_AUTOCOMPLETE_LIST];

            for (int i = 0; i < rfqArray.Length; i++)
                if (rfqArray[i].Equals(selectedVal))
                {
                    TextBox_Rfq_No.Text = rfqArray[i].Substring(rfqArray[i].IndexOf("(")+1, (rfqArray[i].Length -2- rfqArray[i].IndexOf("(")));
                    break;
                }
            
        }

        protected void loadDefectStatAndResolStat()
        {
            ArrayList defectStatList=BackEndObjects.DefectStatCodes.getAllDefectStatCodesDB();
            ArrayList defectResolStatList = BackEndObjects.DefectResolStatCodes.getAllDefectResolStatDB();

            for (int i = 0; i < defectStatList.Count; i++)
            {
                ListItem lt = new ListItem();
                lt.Text = ((BackEndObjects.DefectStatCodes)defectStatList[i]).getDefectStat();
                lt.Value = ((BackEndObjects.DefectStatCodes)defectStatList[i]).getDefectStat();
                
                DropDownList_Defect_Stat.Items.Add(lt);
            }

            for (int i = 0; i < defectResolStatList.Count; i++)
            {
                ListItem lt = new ListItem();
                lt.Text = ((BackEndObjects.DefectResolStatCodes)defectResolStatList[i]).getResolStat();
                lt.Value = ((BackEndObjects.DefectResolStatCodes)defectResolStatList[i]).getResolStat();

                DropDownList_Defect_Resol_Stat.Items.Add(lt);
            }

            DropDownList_Defect_Stat.SelectedValue = "Accept";
            DropDownList_Defect_Resol_Stat.SelectedValue = "Open";
        }
        /// <summary>
        /// Defect severity list is not stored in any DB table as of now.
        /// The drop down list is hard coded here.
        /// </summary>
        protected void loadDefectSev()
        {
            ListItem lt0 = new ListItem();
            lt0.Text = "";
            lt0.Value = "";

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

            DropDownList_Defect_Sev.SelectedValue = "";
        }

        protected void loadContacts()
        {
            ArrayList contactObjList = Contacts.getAllContactsbyEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            
            Dictionary<String, String> contactEmailList = new Dictionary<string, string>();
            for (int i = 0; i < contactObjList.Count; i++)
            {
                ListItem lt = new ListItem();
                String contactShortName = ((Contacts)contactObjList[i]).getContactShortName();
                String contactName = ((Contacts)contactObjList[i]).getContactName();
                String contactEntId = ((Contacts)contactObjList[i]).getContactEntityId();
                
                if(!contactEmailList.ContainsKey(contactEntId))
                contactEmailList.Add(contactEntId, ((Contacts)contactObjList[i]).getEmailId());

                //lt.Text = (contactShortName == null || contactShortName.Equals("") ? contactName : contactShortName);
                lt.Text = contactName;
                lt.Value = contactEntId;
                DropDownList_Contacts.Items.Add(lt);
            }

            ViewState["createDefectContactEmailList"] = contactEmailList;
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            String forwardString = "createContact.aspx";
            forwardString += "?parentContext=" + "defect";
            //Server.Transfer("createContact.aspx",true);

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispContactLead", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=900');", true);

            //Server.Transfer("createContact.aspx", true);
        }

        protected void Button_Submit_Defect_Click(object sender, EventArgs e)
        {
            BackEndObjects.DefectDetails defObj = new DefectDetails();
            String context = Request.QueryString.GetValues("context")[0];

            if (context.Equals("incoming"))
            {
                defObj.setCustomerId(DropDownList_Contacts.SelectedValue);
                defObj.setSupplierId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            }
            else
            {
                defObj.setCustomerId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                defObj.setSupplierId(DropDownList_Contacts.SelectedValue);
            }

            defObj.setDateCreated(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            defObj.setDefectId(new Id().getNewId(BackEndObjects.Id.ID_TYPE_DEFECT_STRING));
            defObj.setDefectStat(DropDownList_Defect_Stat.SelectedValue);
            defObj.setStatReason(TextBox_Resol_Stat_Reason.Text);
            defObj.setDescription(TextBox_Desc.Text);
            //This is the invoice no
            defObj.setInvoiceId(TextBox_Invoice_No.Text);
            defObj.setResolStat(DropDownList_Defect_Resol_Stat.SelectedValue);
            if (defObj.getResolStat().Equals(DefectResolStatCodes.DEFECT_RESOL_STAT_CODE_RESOLVED))
                defObj.setCloseDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            defObj.setRFQId(TextBox_Rfq_No.Text);
            defObj.setSeverity(DropDownList_Defect_Sev.SelectedValue);
            defObj.setCreationMode(BackEndObjects.DefectDetails.DEFECT_CREATION_MODE_MANUAL);
            defObj.setCreatedByComp(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            defObj.setCreatedByUser(User.Identity.Name);
            defObj.setTotalAmount((!TextBox_Defect_Amount.Text.Equals("")?float.Parse(TextBox_Defect_Amount.Text):0));
            defObj.setCurrency(DropDownList_Curr.SelectedValue);
            defObj.setDefectorsr("defect");

            if (FileUpload1 != null && FileUpload1.HasFile)
            {
                defObj.setFileStream(FileUpload1); 
                defObj.setDocPathInFileStore(defObj.getSupplierId());                
            }

            try
            {
                BackEndObjects.DefectDetails.insertDefectDetails(defObj);
                Label_Status.Visible = true;
                Label_Status.ForeColor = System.Drawing.Color.Green;
                Label_Status.Text="Defect Entered Successfully";        
                
     
                DataTable dt=new DataTable();

                if (context.Equals("incoming"))
                    dt = (DataTable)Session[SessionFactory.ALL_DEFECT_ALL_INCOMING_DEFECT_GRID];
                else
                    dt = (DataTable)Session[SessionFactory.ALL_DEFECT_ALL_OUTGOING_DEFECT_GRID];

                dt.Rows.Add();
                int counter = dt.Rows.Count - 1;
                Dictionary<String, Currency> allCurrList = (Dictionary<String, Currency>)Session[SessionFactory.CURRENCY_LIST];

                dt.Rows[counter]["DefectId"] = defObj.getDefectId();
                dt.Rows[counter]["RFQId"] = defObj.getRFQId();
                dt.Rows[counter]["InvNo"] = defObj.getInvoiceId();
                dt.Rows[counter]["descr"] = defObj.getDescription();
                dt.Rows[counter]["Submit Date"] = defObj.getDateCreated();
                dt.Rows[counter]["Submit Date Ticks"] = Convert.ToDateTime(defObj.getDateCreated()).Ticks;
                dt.Rows[counter]["Close Date"] = (defObj.getCloseDate() != null && !defObj.getCloseDate().Equals("") ? defObj.getCloseDate() : "");
                dt.Rows[counter]["Close Date Ticks"] = (defObj.getCloseDate() != null && !defObj.getCloseDate().Equals("") ? Convert.ToDateTime(defObj.getCloseDate()).Ticks : 0);
                dt.Rows[counter]["Amount"] = defObj.getTotalAmount();
                dt.Rows[counter]["Defect_Stat"] = defObj.getDefectStat();
                dt.Rows[counter]["Defect_Stat_Reason"] = defObj.getStatReason();                
                dt.Rows[counter]["Severity"] = defObj.getSeverity();
                dt.Rows[counter]["Defect_Resol_Stat"] = defObj.getResolStat();
                dt.Rows[counter]["curr"] = allCurrList.ContainsKey(defObj.getCurrency()) ?
                                        allCurrList[defObj.getCurrency()].getCurrencyName() : "";

                String docName = "";
                if (defObj.getDocPath() != null)
                {
                    String[] docPathList = defObj.getDocPath().Split(new String[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                    if (docPathList.Length > 0)
                        docName = docPathList[docPathList.Length - 1];
                }
                dt.Rows[counter]["docNameHidden"] = (defObj.getDocPath() == null || defObj.getDocPath().Equals("") ? "N/A" : docName);

                if(context.Equals("incoming"))
                {
                    dt.Rows[counter]["Assigned_To"] = defObj.getAssignedToUser();
                   dt.Rows[counter]["CustName"] = DropDownList_Contacts.SelectedItem.Text;
                   dt.Rows[counter]["entId"] = DropDownList_Contacts.SelectedValue;

                   dt.DefaultView.Sort = "Submit Date Ticks" + " " + "DESC";
                   Session[SessionFactory.ALL_DEFECT_ALL_INCOMING_DEFECT_GRID] = dt.DefaultView.ToTable();

                    //Try to send email alert
                    //Making a non-blocking call
                   Task emailTask = Task.Factory.StartNew(() => trySendEmail(defObj.getDefectId(), defObj.getCustomerId()));

                   ScriptManager.RegisterStartupScript(this, typeof(string), "RefreshDefectGridIncm", "RefreshParentIncm();", true);
                }
                else
                {
                    dt.Rows[counter]["SuplName"] = DropDownList_Contacts.SelectedItem.Text;
                    dt.Rows[counter]["entId"] = DropDownList_Contacts.SelectedValue;
                    dt.DefaultView.Sort = "Submit Date Ticks" + " " + "DESC";
                    Session[SessionFactory.ALL_DEFECT_ALL_OUTGOING_DEFECT_GRID] = dt.DefaultView.ToTable();
                    ScriptManager.RegisterStartupScript(this, typeof(string), "RefreshDefectGridOutg", "RefreshParentOutg();", true);
                }

                
            }
            catch (Exception ex)
            {
                Label_Status.Visible = true;
                Label_Status.ForeColor = System.Drawing.Color.Red;
                Label_Status.Text = "Defect Creation Failed";
            }
        }

        protected void trySendEmail(String defId,String entId)
        {
            
            MainBusinessEntity mBEObj = MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            Dictionary<String, String> contactEmailList = (Dictionary<String, String>)ViewState["createDefectContactEmailList"];
            if (mBEObj.getSupportEmail() != null && !mBEObj.getSupportEmail().Equals("") && contactEmailList != null)
            {
                try
                {
                    ActionLibrary.Emails.sendEmail(
                        mBEObj.getSupportEmail(), mBEObj.getSupportEmailPass(),
                        contactEmailList[entId],
                        "Defect Logged with defect id#" + defId,
                        mBEObj.getNewSupportEmailBody());
                }
                catch (Exception ex)
                {
                }
            }

        }

        protected void Button_Refresh_Click(object sender, EventArgs e)
        {
            loadContacts();
        }

        protected void TextBox_Invoice_No_TextChanged(object sender, EventArgs e)
        {
            Dictionary<String, String> invAmountDict = (Dictionary<String, String>)Session[SessionFactory.ALL_DEFECT_CREATE_DEFECT_INV_AMNT_DICT];
            if (invAmountDict.ContainsKey(TextBox_Invoice_No.Text))
                TextBox_Defect_Amount.Text = invAmountDict[TextBox_Invoice_No.Text];
        }
    }
}