using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BackEndObjects;
using ActionLibrary;
using System.Data;

namespace OnLine.Pages.Popups.Sale
{
    public partial class MultipleInvoiceForRFQ : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                fillGrid();
            }
        }

        protected void fillGrid()
        {
            String rfqId = Request.QueryString.GetValues("rfId")[0];
            Dictionary<String,Dictionary<String,Invoice>> invDict=(Dictionary<String,Dictionary<String,Invoice>>) Session[SessionFactory.ALL_SALE_ALL_POTN_MUTLTIPLE_INV_DATA_FOR_RFQ];
            Dictionary<String, Invoice> invDictForRFQ = invDict[rfqId.Trim()];

            DataTable dt = new DataTable();
            dt.Columns.Add("RFQNo");
            dt.Columns.Add("Inv_No");
            dt.Columns.Add("Inv_Id");
            dt.Columns.Add("Inv_Date");
            dt.Columns.Add("Inv_Date_Ticks");
            dt.Columns.Add("Deliv_Stat");
            dt.Columns.Add("Pmnt_Stat");
            dt.Columns.Add("Amount");
            dt.Columns.Add("Related_PO");

            int i = 0;

            foreach (KeyValuePair<String, Invoice> kvp in invDictForRFQ)
            {
                Invoice invObj = kvp.Value;

                dt.Rows.Add();
                
                dt.Rows[i]["RFQNo"] = invObj.getRFQId();
                dt.Rows[i]["Inv_No"] = invObj.getInvoiceNo();
                dt.Rows[i]["Inv_Id"] = invObj.getInvoiceId();
                dt.Rows[i]["Inv_Date"] = invObj.getInvoiceDate().Replace("00", "").Replace(":", "");
                dt.Rows[i]["Inv_Date_Ticks"] = Convert.ToDateTime(invObj.getInvoiceDate()).Ticks;
                dt.Rows[i]["Deliv_Stat"] = invObj.getDeliveryStatus();
                dt.Rows[i]["Pmnt_Stat"] = invObj.getPaymentStatus();
                dt.Rows[i]["Amount"] = invObj.getTotalAmount();
                dt.Rows[i]["Related_PO"] = invObj.getRelatedPO();

                i++;
            }

            dt.DefaultView.Sort = "Inv_Date_Ticks" + " " + "DESC";
            
            GridView_Invoice.DataSource = dt;
            GridView_Invoice.DataBind();
            GridView_Invoice.Visible = true;
        }

        protected void GridView_Invoice_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void LinkButton_Show_Inv_Invoice_Grid_Command(object sender, CommandEventArgs e)
        {
            //forwardString += "?rfId=" + ((Label)GridView_Potential.Rows[Convert.ToInt32(e.CommandArgument)].Cells[0].FindControl("Label_RFQId")).Text;

            String forwardString = "/Pages/Popups/Sale/Inv_Details.aspx";
            forwardString += "?rfId=" + ((Label)GridView_Invoice.Rows[Convert.ToInt32(e.CommandArgument)].Cells[0].FindControl("Label_RFQId1")).Text;
            String context = Request.QueryString.GetValues("context")!=null?Request.QueryString.GetValues("context")[0]:"";
            if (context.Equals("clientInvoiceGrid"))
                forwardString += "&context=" + context;
            else
            forwardString += "&context=" + "vendInvoiceGrid";

            String relatedPO = ((Label)GridView_Invoice.Rows[Convert.ToInt32(e.CommandArgument)].Cells[0].FindControl("Label_Related_PO")).Text;
            if (!relatedPO.Equals(""))
                forwardString += "&poId=" + relatedPO;
            else
            forwardString += "&poId=" + BackEndObjects.PurchaseOrder.
                getPurchaseOrderforRFQIdDB(((Label)GridView_Invoice.Rows[Convert.ToInt32(e.CommandArgument)].Cells[0].FindControl("Label_RFQId1")).Text).getPo_id();

            forwardString += "&invId=" + ((Label)GridView_Invoice.Rows[Convert.ToInt32(e.CommandArgument)].Cells[0].FindControl("Label_Invoice_Id_Val")).Text;

            //Response.Write("<script type='text/javascript'>alert('" + "Hi" + "');</script>");
            //ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "alert('hi');", true);
            //ScriptManager.RegisterStartupScript(this, typeof(string), "DispInvClientInvGrid", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=900');", true);
            Response.Redirect(forwardString);
        }

        protected void LinkButton_Pmnt_Det_Inv_Command(object sender, CommandEventArgs e)
        {
            String forwardString = "/Pages/Popups/Purchase/Inv_Payment_Details.aspx";
            forwardString += "?rfId=" + ((Label)GridView_Invoice.Rows[Convert.ToInt32(e.CommandArgument)].Cells[0].FindControl("Label_RFQId1")).Text;
            forwardString += "&context=" + "vendor";
            forwardString += "&invId=" + ((Label)GridView_Invoice.Rows[Convert.ToInt32(e.CommandArgument)].Cells[0].FindControl("Label_Invoice_Id_Val")).Text;
            forwardString += "&invNo=" + ((Label)GridView_Invoice.Rows[Convert.ToInt32(e.CommandArgument)].Cells[0].FindControl("Label_Invoice_No1")).Text;

            //Response.Write("<script type='text/javascript'>alert('" + "Hi" + "');</script>");
            //ScriptManager.RegisterStartupScript(this, typeof(string), "Error", "alert('hi');", true);
            //ScriptManager.RegisterStartupScript(this, typeof(string), "DispInvPmntVendor", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=700');", true);
            Response.Redirect(forwardString);
        }

        protected void LinkButton_Show_Defect1_Command(object sender, CommandEventArgs e)
        {
            String forwardString = "/Pages/Popups/Purchase/AllDefectsForInvoice.aspx";
            String invId = BackEndObjects.Invoice.
                getInvoicebyNoDB(((Label)GridView_Invoice.Rows[Convert.ToInt32(e.CommandArgument)].Cells[0].FindControl("Label_Invoice_No1")).Text).getInvoiceId();

            forwardString += "?contextId1=" + invId;
            String context = Request.QueryString.GetValues("context")!=null?Request.QueryString.GetValues("context")[0]:"";
            if (context.Equals("clientInvoiceGrid"))
                forwardString += "&contextId2=" + context;
            else
            forwardString += "&contextId2=" + "vendor";

            //ScriptManager.RegisterStartupScript(this, typeof(string), "DispDefectForInvoiceIncoming", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=700');", true);
            Response.Redirect(forwardString);
        }

    }
}