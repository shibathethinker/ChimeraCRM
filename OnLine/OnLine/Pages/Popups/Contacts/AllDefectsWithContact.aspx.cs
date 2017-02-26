using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using BackEndObjects;
using ActionLibrary;

namespace OnLine.Pages.Popups.Contacts
{
    public partial class AllDefectsWithContact : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                fillIncomingDefectGrid(null);
                fillOutgoingDefectGrid(null);
            }
        }

        protected void fillIncomingDefectGrid(Dictionary<String,DefectDetails> defectDictPassed)
        {
            String[] contactEntId = Request.QueryString.GetValues("contactId");

            DataTable dt = new DataTable();
            dt.Columns.Add("DefectId");
            dt.Columns.Add("RFQId");
            dt.Columns.Add("InvNo");
            dt.Columns.Add("descr");
            dt.Columns.Add("Submit Date");
            dt.Columns.Add("Amount");
            dt.Columns.Add("Defect_Stat");
            dt.Columns.Add("Defect_Stat_Reason");
            dt.Columns.Add("Assigned_To");
            dt.Columns.Add("Severity");
            dt.Columns.Add("Defect_Resol_Stat");

            Dictionary<String, DefectDetails> defectDict = new Dictionary<string, DefectDetails>();

            if (defectDictPassed == null)
                defectDict = BackEndObjects.DefectDetails.getAllDefectDetailsforSupplierIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            else
                defectDict = defectDictPassed;

            int counter = 0;

            DateUtility dU = new DateUtility();

            foreach (KeyValuePair<String, DefectDetails> kvp in defectDict)
            {

                BackEndObjects.DefectDetails defObj = kvp.Value;

                String custId = defObj.getCustomerId();

                if (custId != null && custId.Equals(contactEntId[0]))
                {
                    dt.Rows.Add();

                    dt.Rows[counter]["DefectId"] = defObj.getDefectId();
                    dt.Rows[counter]["RFQId"] = defObj.getRFQId();
                    dt.Rows[counter]["InvNo"] = defObj.getInvoiceId();
                    dt.Rows[counter]["descr"] = defObj.getDescription();
                    dt.Rows[counter]["Submit Date"] = dU.getConvertedDate(defObj.getDateCreated());
                    dt.Rows[counter]["Amount"] = defObj.getTotalAmount();
                    dt.Rows[counter]["Defect_Stat"] = defObj.getDefectStat();
                    dt.Rows[counter]["Defect_Stat_Reason"] = defObj.getStatReason();
                    dt.Rows[counter]["Assigned_To"] = defObj.getAssignedToUser();
                    dt.Rows[counter]["Severity"] = defObj.getSeverity();
                    dt.Rows[counter]["Defect_Resol_Stat"] = defObj.getResolStat();


                    counter++;
                }
            }

            GridView_Incoming_Defects.Visible = true;
            GridView_Incoming_Defects.DataSource = dt;
            GridView_Incoming_Defects.DataBind();

            Session[SessionFactory.ALL_CONTACT_ALL_DEFECT_INCOMING_DEF_GRID] = dt;
            Session[SessionFactory.ALL_CONTACT_ALL_DEFECT_INCOMING_DEF_DICTIONARY] = defectDict;
        }

        protected void fillOutgoingDefectGrid(Dictionary<String, DefectDetails> defectDictPassed)
        {
            String[] contactEntId = Request.QueryString.GetValues("contactId");

            DataTable dt = new DataTable();
            dt.Columns.Add("DefectId");
            dt.Columns.Add("RFQId");
            dt.Columns.Add("InvNo");
            dt.Columns.Add("descr");
            dt.Columns.Add("Submit Date");
            dt.Columns.Add("Amount");
            dt.Columns.Add("Defect_Stat");
            dt.Columns.Add("Defect_Stat_Reason");
            //dt.Columns.Add("Assigned_To");
            dt.Columns.Add("Severity");
            dt.Columns.Add("Defect_Resol_Stat");

            Dictionary<String, DefectDetails> defectDict = new Dictionary<string, DefectDetails>();

            if (defectDictPassed == null)
                defectDict = BackEndObjects.DefectDetails.getAllDefectDetailsforCustomerIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            else
                defectDict = defectDictPassed;

            int counter = 0;

            DateUtility dU = new DateUtility();

            foreach (KeyValuePair<String, DefectDetails> kvp in defectDict)
            {
                BackEndObjects.DefectDetails defObj = kvp.Value;

                String suplId = defObj.getSupplierId();

                if (suplId != null && suplId.Equals(contactEntId[0]))
                {
                    dt.Rows.Add();
                    dt.Rows[counter]["DefectId"] = defObj.getDefectId();
                    dt.Rows[counter]["RFQId"] = defObj.getRFQId();
                    dt.Rows[counter]["InvNo"] = defObj.getInvoiceId();
                    dt.Rows[counter]["descr"] = defObj.getDescription();
                    dt.Rows[counter]["Submit Date"] = dU.getConvertedDate(defObj.getDateCreated());
                    dt.Rows[counter]["Amount"] = defObj.getTotalAmount();
                    dt.Rows[counter]["Defect_Stat"] = defObj.getDefectStat();
                    dt.Rows[counter]["Defect_Stat_Reason"] = defObj.getStatReason();
                    //dt.Rows[counter]["Assigned_To"] = defObj.getAssignedToUser();
                    dt.Rows[counter]["Severity"] = defObj.getSeverity();
                    dt.Rows[counter]["Defect_Resol_Stat"] = defObj.getResolStat();


                    counter++;
                }
            }

            GridView_Outgoing_Defects.Visible = true;
            GridView_Outgoing_Defects.DataSource = dt;
            GridView_Outgoing_Defects.DataBind();

            Session[SessionFactory.ALL_CONTACT_ALL_DEFECT_OUTGOING_DEF_GRID] = dt;
            Session[SessionFactory.ALL_CONTACT_ALL_DEFECT_OUTGOING_DEF_DICTIONARY] = defectDictPassed;
        }

        protected void Button_Filter_Incom_Defect_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> filterParams = new Dictionary<String, String>();
            ActionLibrary.DefectActions._dispDefectDetails dspDefect = new DefectActions._dispDefectDetails();

            if (!TextBox_Def_No.Text.Equals(""))
                filterParams.Add(dspDefect.FILTER_BY_DEFECT_NO, TextBox_Def_No.Text);
            if (!TextBox_Inv_No_Incoming_Defect.Text.Equals(""))
                filterParams.Add(dspDefect.FILTER_BY_INVOICE_NO, TextBox_Inv_No_Incoming_Defect.Text);
            if (!TextBox_RFQ_No_Incoming_Defect.Text.Equals(""))
                filterParams.Add(dspDefect.FILTER_BY_RFQ_NO, TextBox_RFQ_No_Incoming_Defect.Text);
            if (!TextBox_From_Date.Text.Equals(""))
                filterParams.Add(dspDefect.FILTER_BY_SUBMIT_DATE_FROM, TextBox_From_Date.Text);
            if (!TextBox_To_Date.Text.Equals(""))
                filterParams.Add(dspDefect.FILTER_BY_SUBMIT_DATE_TO, TextBox_To_Date.Text);

            fillIncomingDefectGrid(dspDefect.getAllDefectsFiltered("incoming",
                    Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), filterParams));

            Button_Filter_Incom_Defect.Focus();
        }

        protected void GridView_Incoming_Defects_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_Incoming_Defects.PageIndex = e.NewPageIndex;
            GridView_Incoming_Defects.DataSource = (DataTable)Session[SessionFactory.ALL_CONTACT_ALL_DEFECT_INCOMING_DEF_GRID];
            GridView_Incoming_Defects.DataBind();
        }

        protected void GridView_Outgoing_Defects_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_Outgoing_Defects.PageIndex = e.NewPageIndex;
            GridView_Outgoing_Defects.DataSource = (DataTable)Session[SessionFactory.ALL_CONTACT_ALL_DEFECT_OUTGOING_DEF_GRID];
            GridView_Outgoing_Defects.DataBind();
        }

        protected void Button_Filter_Outgoing_Defect_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> filterParams = new Dictionary<String, String>();
            ActionLibrary.DefectActions._dispDefectDetails dspDefect = new DefectActions._dispDefectDetails();

            if (!TextBox_Def_No_Outgoing.Text.Equals(""))
                filterParams.Add(dspDefect.FILTER_BY_DEFECT_NO, TextBox_Def_No_Outgoing.Text);
            if (!TextBox_Inv_No_Outgoing_Defect.Text.Equals(""))
                filterParams.Add(dspDefect.FILTER_BY_INVOICE_NO, TextBox_Inv_No_Outgoing_Defect.Text);
            if (!TextBox_RFQ_No_Outgoing_Defect.Text.Equals(""))
                filterParams.Add(dspDefect.FILTER_BY_RFQ_NO, TextBox_RFQ_No_Outgoing_Defect.Text);
            if (!TextBox_From_Date_Outgoing.Text.Equals(""))
                filterParams.Add(dspDefect.FILTER_BY_SUBMIT_DATE_FROM, TextBox_From_Date_Outgoing.Text);
            if (!TextBox_To_Date_Outgoing.Text.Equals(""))
                filterParams.Add(dspDefect.FILTER_BY_SUBMIT_DATE_TO, TextBox_To_Date_Outgoing.Text);
            
                fillOutgoingDefectGrid(dspDefect.getAllDefectsFiltered("outgoing", Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), filterParams));

            Button_Filter_Outgoing_Defect.Focus();

        }

        protected void LinkButton_Assgn_To_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_Incoming_Defects.SelectedIndex)
                GridView_Incoming_Defects.SelectRow(row.RowIndex);

            ((RadioButton)GridView_Incoming_Defects.SelectedRow.Cells[0].FindControl("incm_radio")).Checked = true;

            String forwardString = "/Pages/DispUserDetails.aspx";
            //window.open('Popups/Sale/AllPotn_NDA.aspx','DispNDAPot','status=1,toolbar=1,width=500,height=200,left=500,right=500',true);
            String userId = ((LinkButton)GridView_Incoming_Defects.SelectedRow.Cells[0].FindControl("LinkButton_Assgn_To")).Text;

            forwardString += "?userId=" + userId;

            ScriptManager.RegisterStartupScript(this, typeof(string), "userDetIncmDefContact",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=400,height=400,left=500,right=500');", true);

        }

        protected void GridView_Defect_RadioSelect(object sender, EventArgs e)
        {
            RadioButton linkb = (RadioButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1)
                GridView_Incoming_Defects.SelectRow(row.RowIndex);
        }

        protected void GridView_Incoming_Defects_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((RadioButton)GridView_Incoming_Defects.SelectedRow.Cells[0].FindControl("incm_radio")).Checked = true;            
        }

        protected void LinkButton_All_Comm_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_Incoming_Defects.SelectedIndex)
                GridView_Incoming_Defects.SelectRow(row.RowIndex);

            ((RadioButton)GridView_Incoming_Defects.SelectedRow.Cells[0].FindControl("incm_radio")).Checked = true;

            String sourceEnt = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();

            String forwardString = "/Pages/DispComm.aspx";
            forwardString += "?contextId=" + ((Label)GridView_Incoming_Defects.SelectedRow.Cells[0].FindControl("Label_Defect_Id")).Text +
                "&source=" + sourceEnt;

            ScriptManager.RegisterStartupScript(this, typeof(string), "CommIncmDef",
    "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=800,height=800,left=500,right=500');", true);
        }

        protected void GridView_Out_Defect_RadioSelect(object sender, EventArgs e)
        {
            RadioButton linkb = (RadioButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1)
                GridView_Outgoing_Defects.SelectRow(row.RowIndex);
        }

        protected void GridView_Outgoing_Defects_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((RadioButton)GridView_Outgoing_Defects.SelectedRow.Cells[0].FindControl("outg_radio")).Checked = true;
        }

        protected void LinkButton_All_Comm_Outg_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_Outgoing_Defects.SelectedIndex)
                GridView_Outgoing_Defects.SelectRow(row.RowIndex);

            ((RadioButton)GridView_Outgoing_Defects.SelectedRow.Cells[0].FindControl("outg_radio")).Checked = true;

            String sourceEnt = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();

            String forwardString = "/Pages/DispComm.aspx";
            forwardString += "?contextId=" + ((Label)GridView_Outgoing_Defects.SelectedRow.Cells[0].FindControl("Label_Defect_Id")).Text +
                "&source=" + sourceEnt;

            ScriptManager.RegisterStartupScript(this, typeof(string), "CommOutgDef",
   "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=800,height=800,left=500,right=500');", true);
        }

    }
}