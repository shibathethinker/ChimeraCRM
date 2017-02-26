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

namespace OnLine.Pages.Popups.Contacts
{
    public partial class AllDealsWithContact : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                fillIncomingInvGrid(null);
                fillOutoingInvGrid(null);
            }
        }

        protected void fillIncomingInvGrid(ArrayList invList)
        {
            String[] contactEntId = Request.QueryString.GetValues("contactId");

            if(invList==null || invList.Count==0)
            invList = BackEndObjects.Invoice.getAllInvoicesbyEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

            DataTable dt = new DataTable();
            dt.Columns.Add("rfqId");
            dt.Columns.Add("InvId");
            dt.Columns.Add("InvNo");
            dt.Columns.Add("totalAmnt");
            dt.Columns.Add("InvDate");
            dt.Columns.Add("pmntStat");
            dt.Columns.Add("totalPending");

            DateUtility dU = new DateUtility();

            int rowCount = 0;
            for (int i = 0; i < invList.Count; i++)
            {
                BackEndObjects.Invoice invObj = (BackEndObjects.Invoice)invList[i];

                if (invObj.getRespEntityId().Equals(contactEntId[0]))
                {
                    float totalPendingAmnt = 0;
                    float totalClearedAmnt = 0;                    

                    Dictionary<String, Payment> pmntDict = BackEndObjects.Payment.getPaymentDetailsforInvoiceDB(invObj.getInvoiceId());

                    foreach (KeyValuePair<String, Payment> kvp in pmntDict)
                    {
                        BackEndObjects.Payment pmntObj = kvp.Value;

                        totalClearedAmnt += pmntObj.getClearingStat().Equals(BackEndObjects.Payment.PAYMENT_CLEARING_STAT_CLEAR) ?
                            pmntObj.getAmount() : 0;
                    }

                    totalPendingAmnt = invObj.getTotalAmount() - totalClearedAmnt;

                    dt.Rows.Add();
                    dt.Rows[rowCount]["rfqId"] = invObj.getRFQId();
                    dt.Rows[rowCount]["InvId"] = invObj.getInvoiceId();
                    dt.Rows[rowCount]["InvNo"] = invObj.getInvoiceNo() != null && !invObj.getInvoiceNo().Equals("") ? invObj.getInvoiceNo() : invObj.getInvoiceId();
                    dt.Rows[rowCount]["totalAmnt"] = invObj.getTotalAmount();
                    dt.Rows[rowCount]["InvDate"] =dU.getConvertedDate(invObj.getInvoiceDate().Substring(0,invObj.getInvoiceDate().IndexOf(" ")));
                    dt.Rows[rowCount]["pmntStat"] = invObj.getPaymentStatus();
                    dt.Rows[rowCount]["totalPending"] = totalPendingAmnt;

                    rowCount++;
                }
            }

            GridView_Incoming_Invoices.Visible = true;
            GridView_Incoming_Invoices.DataSource = dt;
            GridView_Incoming_Invoices.DataBind();
            GridView_Incoming_Invoices.SelectedIndex = -1;
            Session[SessionFactory.ALL_CONTACT_ALL_DEAL_INCOMING_INV_GRID] = dt;
            Session[SessionFactory.ALL_CONTACT_ALL_DEAL_INCOMING_INV_ARRAYLIST] = invList;
        }

        protected void fillOutoingInvGrid(ArrayList invList)
        {
            String[] contactEntId = Request.QueryString.GetValues("contactId");

            if(invList==null || invList.Count==0)
            invList = BackEndObjects.Invoice.getAllInvoicesbyRespEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            
            DataTable dt = new DataTable();
            dt.Columns.Add("rfqId");
            dt.Columns.Add("InvId");
            dt.Columns.Add("InvNo");
            dt.Columns.Add("totalAmnt");
            dt.Columns.Add("InvDate");
            dt.Columns.Add("pmntStat");
            dt.Columns.Add("totalPending");

            DateUtility dU = new DateUtility();

            int counter = 0;
            for (int i = 0; i < invList.Count; i++)
            {                
                BackEndObjects.Invoice invObj = (BackEndObjects.Invoice)invList[i];

                //Filter out invoices whicha re meant for this contact only
                BackEndObjects.RFQDetails rfqObj=BackEndObjects.RFQDetails.getRFQDetailsbyIdDB(invObj.getRFQId());
                if (rfqObj != null && rfqObj.getEntityId() != null && rfqObj.getEntityId().Equals(contactEntId[0]))
                {
                    float totalPendingAmnt = 0;
                    float totalClearedAmnt = 0;

                    Dictionary<String, Payment> pmntDict = BackEndObjects.Payment.getPaymentDetailsforInvoiceDB(invObj.getInvoiceId());

                    foreach (KeyValuePair<String, Payment> kvp in pmntDict)
                    {
                        BackEndObjects.Payment pmntObj = kvp.Value;

                        totalClearedAmnt += pmntObj.getClearingStat().Equals(BackEndObjects.Payment.PAYMENT_CLEARING_STAT_CLEAR) ?
                            pmntObj.getAmount() : 0;
                    }

                    totalPendingAmnt = invObj.getTotalAmount() - totalClearedAmnt;

                    dt.Rows.Add();
                    dt.Rows[counter]["rfqId"] = invObj.getRFQId();
                    dt.Rows[counter]["InvId"] = invObj.getInvoiceId();
                    dt.Rows[counter]["InvNo"] = invObj.getInvoiceNo() != null && !invObj.getInvoiceNo().Equals("") ? invObj.getInvoiceNo() : invObj.getInvoiceId();
                    dt.Rows[counter]["totalAmnt"] = invObj.getTotalAmount();
                    dt.Rows[counter]["InvDate"] =dU.getConvertedDate(invObj.getInvoiceDate().Substring(0,invObj.getInvoiceDate().IndexOf(" ")));
                    dt.Rows[counter]["pmntStat"] = invObj.getPaymentStatus();
                    dt.Rows[counter]["totalPending"] = totalPendingAmnt;

                    counter++;
                }
            }

            GridView_Outgoing_Invoices.Visible = true;
            GridView_Outgoing_Invoices.DataSource = dt;
            GridView_Outgoing_Invoices.DataBind();
            GridView_Outgoing_Invoices.SelectedIndex = -1;
            Session[SessionFactory.ALL_CONTACT_ALL_DEAL_OUTGOING_INV_GRID] = dt;
            Session[SessionFactory.ALL_CONTACT_ALL_DEAL_OUTGOING_INV_ARRAYLIST] = invList;
        }

        protected void Button_Filter_Incom_Invoice_Click(object sender, EventArgs e)
        {
            String invNo = TextBox_Inv_No.Text;
            String fromDate = TextBox_From_Date.Text;
            String toDate = TextBox_To_Date.Text;
            ActionLibrary.PurchaseActions._dispInvoiceDetails dInv = new ActionLibrary.PurchaseActions._dispInvoiceDetails();

            Dictionary<String, String> filterParams = new Dictionary<string, string>();

            if (invNo != null && !invNo.Equals(""))
                filterParams.Add(dInv.FILTER_BY_INVOICE_NO, invNo);
            if (fromDate != null && !fromDate.Equals(""))
                filterParams.Add(dInv.FILTER_BY_FROM_DATE, fromDate);
            if (toDate != null && !toDate.Equals(""))
                filterParams.Add(dInv.FILTER_BY_TO_DATE, toDate);

            if (filterParams.Count > 0)
                fillIncomingInvGrid(dInv.getAllInvDetailsFiltered(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), (ArrayList)Session[SessionFactory.ALL_CONTACT_ALL_DEAL_INCOMING_INV_ARRAYLIST], filterParams));
            else
                fillIncomingInvGrid(null);
        }

        protected void GridView_Incoming_Invoices_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_Incoming_Invoices.PageIndex = e.NewPageIndex;
            GridView_Incoming_Invoices.DataSource = (DataTable)Session[SessionFactory.ALL_CONTACT_ALL_DEAL_INCOMING_INV_GRID];
            GridView_Incoming_Invoices.DataBind();
        }

        protected void GridView_Outgoing_Invoices_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_Outgoing_Invoices.PageIndex = e.NewPageIndex;
            GridView_Outgoing_Invoices.DataSource = (DataTable)Session[SessionFactory.ALL_CONTACT_ALL_DEAL_OUTGOING_INV_GRID];
            GridView_Outgoing_Invoices.DataBind();
        }

        protected void LinkButton_Invoice_Id_No_Incoming_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_Incoming_Invoices.SelectedIndex)
                GridView_Incoming_Invoices.SelectRow(row.RowIndex);

            ((RadioButton)GridView_Incoming_Invoices.SelectedRow.Cells[0].FindControl("deals_radio")).Checked = true;

            String forwardString = "/Pages/Popups/Sale/Inv_Details.aspx";

            String invId=((Label)GridView_Incoming_Invoices.SelectedRow.Cells[0].FindControl("Label_Inv_Id_Hidden")).Text;
                String rfqId=((Label)GridView_Incoming_Invoices.SelectedRow.Cells[0].FindControl("Label_rfq_Id_Hidden")).Text;
            String poId=BackEndObjects.PurchaseOrder.getPurchaseOrderforRFQIdDB(rfqId).getPo_id();

            forwardString += "?rfId=" + rfqId;
            forwardString += "&context=" + "clientInvoiceGrid";
            forwardString += "&poId=" + poId;
            forwardString += "&invId=" + invId;

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispInvforContactIncoming", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=1000,Height=900');", true);
        }

        protected void LinkButton_Invoice_Id_No_Outgoing_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_Outgoing_Invoices.SelectedIndex)
                GridView_Outgoing_Invoices.SelectRow(row.RowIndex);

            ((RadioButton)GridView_Outgoing_Invoices.SelectedRow.Cells[0].FindControl("deals_radio_outg")).Checked = true;

            String forwardString = "/Pages/Popups/Sale/Inv_Details.aspx";

            String invId = ((Label)GridView_Outgoing_Invoices.SelectedRow.Cells[0].FindControl("Label_Inv_Id_Hidden")).Text;
            String rfqId = ((Label)GridView_Outgoing_Invoices.SelectedRow.Cells[0].FindControl("Label_rfq_Id_Hidden")).Text;
            String poId = BackEndObjects.PurchaseOrder.getPurchaseOrderforRFQIdDB(rfqId).getPo_id();

            forwardString += "?rfId=" + rfqId;
            forwardString += "&context=" + "vendInvoiceGrid";
            forwardString += "&poId=" + poId;
            forwardString += "&invId=" + invId;

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispInvforContactOutgoing", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=1000,Height=900');", true);
        }

        protected void LinkButton_Pmnt_Det_Incoming_Click(object sender, EventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_Incoming_Invoices.SelectedIndex)
                GridView_Incoming_Invoices.SelectRow(row.RowIndex);

            ((RadioButton)GridView_Incoming_Invoices.SelectedRow.Cells[0].FindControl("deals_radio")).Checked = true;

            String forwardString = "/Pages/Popups/Purchase/Inv_Payment_Details.aspx";
            forwardString += "?rfId=" + ((Label)GridView_Incoming_Invoices.SelectedRow.Cells[0].FindControl("Label_rfq_Id_Hidden")).Text;
            forwardString += "&context=" + "client";
            forwardString += "&invId=" + ((Label)GridView_Incoming_Invoices.SelectedRow.Cells[0].FindControl("Label_Inv_Id_Hidden")).Text;
            forwardString += "&invNo=" + ((LinkButton)GridView_Incoming_Invoices.SelectedRow.Cells[0].FindControl("LinkButton_Invoice_Id_No_Incoming")).Text;

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispInvPmntContactDealsIncoming", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=700');", true);

        }

        protected void LinkButton_Pmnt_Det_Outgoing_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_Outgoing_Invoices.SelectedIndex)
                GridView_Outgoing_Invoices.SelectRow(row.RowIndex);

            ((RadioButton)GridView_Outgoing_Invoices.SelectedRow.Cells[0].FindControl("deals_radio_outg")).Checked = true;


            String forwardString = "/Pages/Popups/Purchase/Inv_Payment_Details.aspx";
            forwardString += "?rfId=" + ((Label)GridView_Outgoing_Invoices.SelectedRow.Cells[0].FindControl("Label_rfq_Id_Hidden")).Text;
            forwardString += "&context=" + "vendor";
            forwardString += "&invId=" + ((Label)GridView_Outgoing_Invoices.SelectedRow.Cells[0].FindControl("Label_Inv_Id_Hidden")).Text;
            forwardString += "&invNo=" + ((LinkButton)GridView_Outgoing_Invoices.SelectedRow.Cells[0].FindControl("LinkButton_Invoice_Id_No_Outgoing")).Text;

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispInvPmntContactDealsOutgoing", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=700');", true);
        }

        protected void Button_Filter_Outgoing_Invoice_Click(object sender, EventArgs e)
        {
            String invNo = TextBox_Inv_No_Outgoing.Text;
            String fromDate = TextBox_From_Date_Outgoing.Text;
            String toDate = TextBox_To_Date_Outgoing.Text;
            ActionLibrary.PurchaseActions._dispInvoiceDetails dInv = new ActionLibrary.PurchaseActions._dispInvoiceDetails();

            Dictionary<String, String> filterParams = new Dictionary<string, string>();

            if (invNo != null && !invNo.Equals(""))
                filterParams.Add(dInv.FILTER_BY_INVOICE_NO, invNo);
            if (fromDate != null && !fromDate.Equals(""))
                filterParams.Add(dInv.FILTER_BY_FROM_DATE, fromDate);
            if (toDate != null && !toDate.Equals(""))
                filterParams.Add(dInv.FILTER_BY_TO_DATE, toDate);

            if (filterParams.Count > 0)
                fillOutoingInvGrid(dInv.getAllInvDetailsFiltered(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(),
                    (ArrayList)Session[SessionFactory.ALL_CONTACT_ALL_DEAL_OUTGOING_INV_ARRAYLIST], filterParams));
            else
                fillOutoingInvGrid(null);
        }

        protected void GridView_Incoming_Invoices_RadioSelect(object sender, EventArgs e)
        {
            RadioButton linkb = (RadioButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1)
                GridView_Incoming_Invoices.SelectRow(row.RowIndex);
        }

        protected void GridView_Incoming_Invoices_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((RadioButton)GridView_Incoming_Invoices.SelectedRow.Cells[0].FindControl("deals_radio")).Checked = true;
        }

        protected void GridView_Outgoing_Invoices_RadioSelect(object sender, EventArgs e)
        {
            RadioButton linkb = (RadioButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1)
                GridView_Outgoing_Invoices.SelectRow(row.RowIndex);
        }

        protected void GridView_Outgoing_Invoices_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((RadioButton)GridView_Outgoing_Invoices.SelectedRow.Cells[0].FindControl("deals_radio_outg")).Checked = true;
        }
    }
}