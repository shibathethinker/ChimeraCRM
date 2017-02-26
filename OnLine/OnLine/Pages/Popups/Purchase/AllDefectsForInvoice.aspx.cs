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

namespace OnLine.Pages.Popups.Purchase
{
    public partial class AllDefectsForInvoice : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                fillDefectGrid(null);
        }

        protected void fillDefectGrid(Dictionary<String, DefectDetails> defectDictPassed)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DefectId");
            dt.Columns.Add("RFQId");
            dt.Columns.Add("InvNo");
            dt.Columns.Add("descr");
            dt.Columns.Add("Submit Date");
            dt.Columns.Add("Close Date");
            dt.Columns.Add("Amount");
            dt.Columns.Add("Defect_Stat");
            dt.Columns.Add("Defect_Stat_Reason");
            dt.Columns.Add("Assigned_To");
            dt.Columns.Add("Severity");
            dt.Columns.Add("Defect_Resol_Stat");

            Dictionary<String, DefectDetails> defectDict = new Dictionary<string, DefectDetails>();

            if (defectDictPassed == null)
                defectDict = BackEndObjects.DefectDetails.getAllDefectDetailsforInvoiceIdDB(Request.QueryString.GetValues("contextId1")[0]);
            else
                defectDict = defectDictPassed;

            int counter = 0;
            DateUtility dU = new DateUtility();

            foreach (KeyValuePair<String, DefectDetails> kvp in defectDict)
            {

                BackEndObjects.DefectDetails defObj = kvp.Value;

                          dt.Rows.Add();

                    dt.Rows[counter]["DefectId"] = defObj.getDefectId();
                    dt.Rows[counter]["RFQId"] = defObj.getRFQId();
                    dt.Rows[counter]["InvNo"] = defObj.getInvoiceId();
                    dt.Rows[counter]["descr"] = defObj.getDescription();
                    dt.Rows[counter]["Submit Date"] = dU.getConvertedDate(defObj.getDateCreated());
                    dt.Rows[counter]["Close Date"] = dU.getConvertedDate(defObj.getCloseDate() != null && !defObj.getCloseDate().Equals("") ? defObj.getCloseDate() : "");
                    dt.Rows[counter]["Amount"] = defObj.getTotalAmount();
                    dt.Rows[counter]["Defect_Stat"] = defObj.getDefectStat();
                    dt.Rows[counter]["Defect_Stat_Reason"] = defObj.getStatReason();
                    dt.Rows[counter]["Assigned_To"] = defObj.getAssignedToUser();
                    dt.Rows[counter]["Severity"] = defObj.getSeverity();
                    dt.Rows[counter]["Defect_Resol_Stat"] = defObj.getResolStat();
                
                    counter++;
      
            }

            if (defectDict.Count > 0)
            {
                GridView_Defects.Visible = true;
                GridView_Defects.DataSource = dt;
                GridView_Defects.DataBind();
            }
            else
                Label_Empty_Grid.Visible = true;
            //Dont show assigned to for outgoing defects
            String context = Request.QueryString.GetValues("contextId2")[0];
            if (context != null && !context.Equals("vendor"))
                GridView_Defects.Columns[9].Visible = false;

        }

        protected void LinkButton_Assgn_To_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_Defects.SelectedIndex)
                GridView_Defects.SelectRow(row.RowIndex);

            ((RadioButton)GridView_Defects.SelectedRow.Cells[0].FindControl("outg_radio")).Checked = true;

            String forwardString = "/Pages/DispUserDetails.aspx";
            //window.open('Popups/Sale/AllPotn_NDA.aspx','DispNDAPot','status=1,toolbar=1,width=500,height=200,left=500,right=500',true);
            String userId = ((LinkButton)GridView_Defects.SelectedRow.Cells[0].FindControl("LinkButton_Assgn_To")).Text;

            forwardString += "?userId=" + userId;

            ScriptManager.RegisterStartupScript(this, typeof(string), "allDefInv",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=400,height=400,left=500,right=500');", true);

        }

        protected void GridView_Inv_RadioSelect(object sender, EventArgs e)
        {
            RadioButton linkb = (RadioButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1)
                GridView_Defects.SelectRow(row.RowIndex);
        }

        protected void LinkButton_All_Comm_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_Defects.SelectedIndex)
                GridView_Defects.SelectRow(row.RowIndex);

            ((RadioButton)GridView_Defects.SelectedRow.Cells[0].FindControl("outg_radio")).Checked = true;

            String sourceEnt = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();

            String forwardString = "/Pages/DispComm.aspx";
            forwardString += "?contextId=" + ((Label)GridView_Defects.SelectedRow.Cells[0].FindControl("Label_Defect_Id")).Text +
                "&source=" + sourceEnt;

            ScriptManager.RegisterStartupScript(this, typeof(string), "CommOutgDef",
   "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=800,height=800,left=500,right=500');", true);

        }

    }
}