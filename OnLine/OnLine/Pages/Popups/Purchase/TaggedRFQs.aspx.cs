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
    public partial class TaggedRFQs : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                fillTaggedRFQsList();
        }

        protected void fillTaggedRFQsList()
        {
            String reqId = Request.QueryString.GetValues("contextId1")[0];
            ArrayList rfqL = BackEndObjects.RFQDetails.getAllRFQbyRequirementIdDB(reqId);

            DataTable dt = new DataTable();
            dt.Columns.Add("RFQNo");
            dt.Columns.Add("RFQName");
             dt.Columns.Add("Submit Date");
            dt.Columns.Add("Due Date");
            dt.Columns.Add("ApprovalStat");
            dt.Columns.Add("PO_No");
            dt.Columns.Add("ActiveStatus");
            dt.Columns.Add("Inv_No");

            DateUtility dU = new DateUtility();

            for (int i = 0; i < rfqL.Count; i++)
            {
                dt.Rows.Add();
                String rfId = ((BackEndObjects.RFQDetails)rfqL[i]).getRFQId();
                String poId = BackEndObjects.PurchaseOrder.getPurchaseOrderforRFQIdDB(rfId).getPo_id();
                dt.Rows[i]["RFQNo"] = rfId;
                dt.Rows[i]["RFQName"] = ((BackEndObjects.RFQDetails)rfqL[i]).getRFQName();
                dt.Rows[i]["Submit Date"] =dU.getConvertedDate(((BackEndObjects.RFQDetails)rfqL[i]).getSubmitDate());
                dt.Rows[i]["Due Date"] = dU.getConvertedDate(((BackEndObjects.RFQDetails)rfqL[i]).getDueDate());
                dt.Rows[i]["ApprovalStat"] = ((BackEndObjects.RFQDetails)rfqL[i]).getApprovalStat();
                dt.Rows[i]["Po_No"] = (poId == null || poId.Equals("") ? "N/A" : poId);

                String activeStat = ((BackEndObjects.RFQDetails)rfqL[i]).getActiveStat();

                dt.Rows[i]["ActiveStatus"] = activeStat;

                if (activeStat.Equals(BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_DEAL_CLOSED))
                {
                    Invoice invObj = BackEndObjects.Invoice.getInvoicebyRfIdDB(rfId);
                    if (invObj != null && invObj.getInvoiceId() != null && !invObj.getInvoiceId().Equals(""))
                        dt.Rows[i]["Inv_No"] ="Show";
                }
                else
                    dt.Rows[i]["Inv_No"] = "N/A";
            }

            GridView_RFQ.DataSource = dt;
            GridView_RFQ.DataBind();
            GridView_RFQ.Visible = true;

            Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

            if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_REQUIREMENT] &&
!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                GridView_RFQ.Columns[1].Visible = false;

            Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_TAGGED_RFQ_GRID] = dt;

            foreach (GridViewRow gVR in GridView_RFQ.Rows)
            {
                if (((LinkButton)gVR.Cells[0].FindControl("LinkButton_Show_Inv")).Text.Equals("N/A"))
                    ((LinkButton)gVR.Cells[0].FindControl("LinkButton_Show_Inv")).Enabled = false;
                if (((LinkButton)gVR.Cells[0].FindControl("LinkButton_PO")).Text.Equals("N/A"))
                    ((LinkButton)gVR.Cells[0].FindControl("LinkButton_PO")).Enabled = false;
            }

            if (dt == null || dt.Rows.Count == 0)
                Label_No_Records.Visible = true;
        }

        protected void GridView_RFQ_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((RadioButton)GridView_RFQ.SelectedRow.Cells[0].FindControl("rfq_radio")).Checked = true;
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
            //forwardString += "&context=" + "client";
            //forwardString += "&poId=" + ((LinkButton)GridView_RFQ.SelectedRow.Cells[0].FindControl("LinkButton_PO")).Text;

            //BackEndObjects.Invoice invObj = BackEndObjects.Invoice.getInvoicebyRfIdDB(((Label)GridView_RFQ.SelectedRow.Cells[0].FindControl("Label_RFQId")).Text);
            //forwardString += "&invId=" + invObj.getInvoiceId();

            //Response.Write("<script type='text/javascript'>alert('" + "Hi" + "');</script>");
            //ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "alert('hi');", true);
            //ScriptManager.RegisterStartupScript(this, typeof(string), "DispInvClient", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=900');", true);
            ScriptManager.RegisterStartupScript(this, typeof(string), "DispInv", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=900,Height=900');", true);
        }

        protected void LinkButton_PO_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_RFQ.SelectedIndex)
                GridView_RFQ.SelectRow(row.RowIndex);

            ((RadioButton)GridView_RFQ.SelectedRow.Cells[0].FindControl("rfq_radio")).Checked = true;

            String forwardString = "/Pages/Popups/Purchase/PO_Details.aspx";
            forwardString += "?rfId=" + ((Label)GridView_RFQ.SelectedRow.Cells[0].FindControl("Label_RFQId")).Text;
            forwardString += "&context=" + "clientTaggedRFQ";
            forwardString += "&poId=" + ((LinkButton)GridView_RFQ.SelectedRow.Cells[0].FindControl("LinkButton_PO")).Text;
            ScriptManager.RegisterStartupScript(this, typeof(string), "DispPOTaggedRFQ", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=900');", true);

        }

        protected void GridView_RFQ_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            String rfqId = ((Label)GridView_RFQ.Rows[e.RowIndex].Cells[0].FindControl("Label_RFQId")).Text;

            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            Dictionary<String, String> targetVals = new Dictionary<string, string>();

            whereCls.Add(BackEndObjects.RFQDetails.RFQ_COL_RFQ_ID, rfqId);
            targetVals.Add(BackEndObjects.RFQDetails.RFQ_COL_RELATED_REQ, "");
            BackEndObjects.RFQDetails.updateRFQDetailsDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);

            DataTable dt = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_TAGGED_RFQ_GRID];
            int index = GridView_RFQ.Rows[e.RowIndex].DataItemIndex;
            dt.Rows[index].Delete();

            GridView_RFQ.DataSource = dt;
            GridView_RFQ.DataBind();

        }

        protected void GridView_RFQ_RadioSelect(object sender, EventArgs e)
        {
            RadioButton linkb = (RadioButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1)
                GridView_RFQ.SelectRow(row.RowIndex);
        }
    }
}