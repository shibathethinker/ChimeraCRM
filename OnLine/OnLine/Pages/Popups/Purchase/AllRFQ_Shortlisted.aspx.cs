using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using BackEndObjects;
using ActionLibrary;
using System.Data;

namespace OnLine.Pages.Popups.Purchase
{
    public partial class AllRFQ_Shortlisted : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                fillRespQuoteGrid();
                Button_Finalz_Deal.Enabled = false;
            }
        }

        protected void fillRespQuoteGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("RespCompId");
            dt.Columns.Add("VendName");
            dt.Columns.Add("quote");
            dt.Columns.Add("NDA");
            dt.Columns.Add("DateVal");
            dt.Columns.Add("comm");
            dt.Columns.Add("TotalAmnt");
            dt.Columns.Add("DealReq");

            ArrayList  shortListedList = RFQShortlisted.
                getAllShortListedEntriesbyRFQId(Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SELECTED_RFQ_ID].ToString());
            DateUtility dU = new DateUtility();

            for (int i = 0; i < shortListedList.Count; i++)
            {
                RFQShortlisted shortObj = (RFQShortlisted)shortListedList[i];
                dt.Rows.Add();
                dt.Rows[i]["RespCompId"] = shortObj.getRespEntityId();
                dt.Rows[i]["VendName"] = MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(shortObj.getRespEntityId()).getEntityName();
                dt.Rows[i]["DateVal"] =dU.getConvertedDate(shortObj.getCreatedDate());
                dt.Rows[i]["TotalAmnt"] = shortObj.getPotenAmnt();
                dt.Rows[i]["DealReq"] = shortObj.getFinlSupFlag();
            } 


            GridView_RFQ_Resp_Quotes.DataSource = dt;
            GridView_RFQ_Resp_Quotes.DataBind();
            GridView_RFQ_Resp_Quotes.Columns[1].Visible = false;

            //Find out the selected entity if any
            RFQShortlisted selectedPotObj=RFQShortlisted.
                getRFQShortlistedEntryforFinalizedVendor(Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SELECTED_RFQ_ID].ToString());
            Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SHORLISTED_FINALIZED_POTEN] = selectedPotObj.getPotentialId();

            foreach (GridViewRow gVR in GridView_RFQ_Resp_Quotes.Rows)
            {
                if (((Label)gVR.Cells[0].FindControl("Label_Hidden")).Text.Equals(selectedPotObj.getRespEntityId()))
                    ((System.Web.UI.WebControls.Image)gVR.Cells[0].FindControl("Image_Selected")).Visible = true;
            }
        }



        protected void GridView_RFQ_Resp_Quotes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void GridView_RFQ_Resp_Quotes_SelectedIndexChanged(object sender, EventArgs e)
        {
            String respEntityId = ((Label)GridView_RFQ_Resp_Quotes.SelectedRow.Cells[0].FindControl("Label_Hidden")).Text;
            Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SHOW_QUOTE_SELECTED_RESP_ENTITY_ID] = respEntityId;
            
            bool hasAccess = true;
            Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];
            if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_RFQ] &&
!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                hasAccess = false;

            //Enable the finalize button only if some vendor if not already selected
            if((Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SHORLISTED_FINALIZED_POTEN]==null||
                Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SHORLISTED_FINALIZED_POTEN].ToString()==null || 
                Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SHORLISTED_FINALIZED_POTEN].ToString().Equals(""))
                && hasAccess)
            Button_Finalz_Deal.Enabled = true;
        }

        protected void ShowComm(object sender, CommandEventArgs e)
        {
            GridView grdViewquotes = (GridView)((LinkButton)sender).Parent.Parent.Parent.Parent;
            Int32 SelectedRowIndex = Convert.ToInt32(e.CommandArgument) % grdViewquotes.PageSize;
            String sourceEnt = ((Label)((GridView)grdViewquotes).Rows[SelectedRowIndex].Cells[0].FindControl("Label_Hidden")).Text;
            String destEnt = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();

            String forwardString = "/Pages/DispComm.aspx";
            forwardString += "?contextId=" + Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SELECTED_RFQ_ID].ToString() +
                "&source=" + sourceEnt +
                "&destination=" + destEnt;

            Server.Transfer(forwardString);

            /*SortedDictionary<DateTime, BackEndObjects.Communications> allComm = BackEndObjects.Communications.
                getAllCommunicationsforContextIdBetweenSourceAndDestDB(Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SELECTED_RFQ_ID].ToString(),
                sourceEnt,
                destEnt);*/

        }

        protected void show_Vendor(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_RFQ_Resp_Quotes.SelectedIndex)
                GridView_RFQ_Resp_Quotes.SelectRow(row.RowIndex);

            ((RadioButton)GridView_RFQ_Resp_Quotes.SelectedRow.Cells[0].FindControl("rfq_radio")).Checked = true;

            GridView grdViewquotes = (GridView)((LinkButton)sender).Parent.Parent.Parent.Parent;
            Int32 SelectedRowIndex = Convert.ToInt32(e.CommandArgument) % grdViewquotes.PageSize;
            String sourceEnt = ((Label)((GridView)grdViewquotes).Rows[SelectedRowIndex].Cells[0].FindControl("Label_Hidden")).Text;
            String destEnt = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();

            String forwardString = "/Pages/DispEntityDetails.aspx";
            forwardString += "?entityId=" + sourceEnt;

            Server.Transfer(forwardString);
        }

        protected void Button_Finalz_Deal_Click(object sender, EventArgs e)
        {
            String respEntityId = Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SHOW_QUOTE_SELECTED_RESP_ENTITY_ID].ToString();
            String rfqId = Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SELECTED_RFQ_ID].ToString();

            RFQShortlisted rfqShortObj = RFQShortlisted.getRFQShortlistedbyRespEntandRFQId(respEntityId, rfqId);

            //Allow only if the supplier finalized the deal
            if (rfqShortObj.getFinlSupFlag().Equals("Y"))
            {
                Dictionary<String, String> whereCls = new Dictionary<string, string>();
                whereCls.Add(BackEndObjects.RFQShortlisted.RFQ_SHORTLIST_COL_RFQ_ID, rfqId);
                whereCls.Add(BackEndObjects.RFQShortlisted.RFQ_SHORTLIST_COL_RESP_ENT_ID, respEntityId);

                Dictionary<String, String> targetVals = new Dictionary<string, string>();
                targetVals.Add(BackEndObjects.RFQShortlisted.RFQ_SHORTLIST_COL_FINL_BY_CUST, "Y");

                try
                {
                    BackEndObjects.RFQShortlisted.updateRFQShortListedEntryDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
                    Label_Finalize_Stat.Visible = true;
                    Label_Finalize_Stat.ForeColor = System.Drawing.Color.Green;
                    Label_Finalize_Stat.Text = "Deal Finalization Request Sent";

                    String forwardString = "/Pages/Popups/Purchase/FinalizeDeal.aspx";
                    forwardString += "?respCompId=" + respEntityId + "&rfqId=" + rfqId+"&context="+"client";
                    
                    Button_Finalz_Deal.Enabled = false;
                    Response.Redirect(forwardString);
                    //Server.Transfer(forwardString);

                }
                catch (Exception ex)
                {
                    Label_Finalize_Stat.Visible = true;
                    Label_Finalize_Stat.ForeColor = System.Drawing.Color.Red;
                    Label_Finalize_Stat.Text = "Deal Finalization Request Failed";
                }
            }
            else
            {
                Label_Finalize_Stat.Visible = true;
                Label_Finalize_Stat.ForeColor = System.Drawing.Color.Red;
                Label_Finalize_Stat.Text = "Vendor needs to finalize the deal with final quotes";
            }
        }

        protected void GridView_RFQ_Shortlisted_RadioSelect(object sender, EventArgs e)
        {
            RadioButton linkb = (RadioButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1)
                GridView_RFQ_Resp_Quotes.SelectRow(row.RowIndex);
        }

        protected void LinkButton_All_Quotes_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_RFQ_Resp_Quotes.SelectedIndex)
                GridView_RFQ_Resp_Quotes.SelectRow(row.RowIndex);

            ((RadioButton)GridView_RFQ_Resp_Quotes.SelectedRow.Cells[0].FindControl("rfq_radio")).Checked = true;

            String forwardString = "AllRFQ_Quotes_Indv_Resp_Det.aspx";
            ScriptManager.RegisterStartupScript(this, typeof(string), "DispRespQuoteInd",
               "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=1000,height=400,left=100,right=500');", true);
        }
    }
}