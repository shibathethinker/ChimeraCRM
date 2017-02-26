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
    public partial class AllRFQ_Quotes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                fillRespQuoteGrid();
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
            dt.Columns.Add("ShortListed");

            int counter = 0;

            Dictionary<String,RFQResponse> respDict=RFQResponse.getAllRFQResponseforRFQIdDB(Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SELECTED_RFQ_ID].ToString());
            DateUtility dU = new DateUtility();

            foreach (KeyValuePair<String, RFQResponse> kvp in respDict)
            {
                RFQResponse respObj = (RFQResponse)kvp.Value;
                dt.Rows.Add();
                dt.Rows[counter]["RespCompId"] = respObj.getRespEntityId();
                dt.Rows[counter]["VendName"] = MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(respObj.getRespEntityId()).getEntityName();
                dt.Rows[counter]["DateVal"] = dU.getConvertedDate(respObj.getRespDate());
                                
              BackEndObjects.RFQShortlisted shortObj = BackEndObjects.RFQShortlisted.
                getRFQShortlistedbyRespEntandRFQId(respObj.getRespEntityId(),
                Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SELECTED_RFQ_ID].ToString());

              if (shortObj != null && shortObj.getRFQId() != null && !shortObj.getRFQId().Equals(""))
                  dt.Rows[counter]["ShortListed"] = "Y";
              else
                  dt.Rows[counter]["ShortListed"] = "N";

                counter++;
            }

            GridView_RFQ_Resp_Quotes.DataSource = dt;
            GridView_RFQ_Resp_Quotes.DataBind();
            GridView_RFQ_Resp_Quotes.Columns[1].Visible = false;

            if (dt == null || dt.Rows.Count == 0)
                Label_Empty_Quote.Visible = true;
            else
                Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_ALL_QUOTES_GRID] = dt;
        }

        protected void GridView_RFQ_Resp_Quotes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void GridView_RFQ_Resp_Quotes_SelectedIndexChanged(object sender, EventArgs e)
        {
            String respEntityId = ((Label)GridView_RFQ_Resp_Quotes.SelectedRow.Cells[0].FindControl("Label_Hidden")).Text;

            ((RadioButton)GridView_RFQ_Resp_Quotes.SelectedRow.Cells[0].FindControl("rfq_radio")).Checked = true;

            Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SHOW_QUOTE_SELECTED_RESP_ENTITY_ID] = respEntityId;


        }

        protected void ShowComm(object sender, CommandEventArgs e)
        {
            GridView grdViewquotes = (GridView)((LinkButton)sender).Parent.Parent.Parent.Parent;
            Int32 SelectedRowIndex = Convert.ToInt32(e.CommandArgument) % grdViewquotes.PageSize;
            String sourceEnt = ((Label)((GridView)grdViewquotes).Rows[SelectedRowIndex].Cells[0].FindControl("Label_Hidden")).Text;
            String destEnt = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();

            String forwardString = "/Pages/DispComm.aspx";
            forwardString+="?contextId="+Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SELECTED_RFQ_ID].ToString()+
                "&source="+sourceEnt+
                "&destination="+destEnt;

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

            String forwardString = "/Pages/DispEntityDetails.aspx";
            forwardString += "?entityId=" + sourceEnt;

            Server.Transfer(forwardString);
        }

        protected void GridView_RFQ_Resp_Quotes_RadioSelect(object sender, EventArgs e)
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
            forwardString += "?dataItemIndex=" + GridView_RFQ_Resp_Quotes.SelectedRow.DataItemIndex;
            Response.Redirect(forwardString);
            /*ScriptManager.RegisterStartupScript(this, typeof(string), "DispRespQuoteInd",
               "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=1000,height=400,left=100,right=500');", true);*/
        }

        protected void LinkButton_All_Shortlisted_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_RFQ_Resp_Quotes.SelectedIndex)
                GridView_RFQ_Resp_Quotes.SelectRow(row.RowIndex);

            ((RadioButton)GridView_RFQ_Resp_Quotes.SelectedRow.Cells[0].FindControl("rfq_radio")).Checked = true;
        }

        protected void Button_Rfq_Refresh_Hidden_Click_Index_Unchanged_Event(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_ALL_QUOTES_GRID];
            //dt.DefaultView.Sort = "Submit Date Ticks" + " " + "DESC";         
            GridView_RFQ_Resp_Quotes.DataSource = dt;
            GridView_RFQ_Resp_Quotes.DataBind();
            ((RadioButton)GridView_RFQ_Resp_Quotes.SelectedRow.Cells[0].FindControl("rfq_radio")).Checked = true;
            //GridView_RFQ.SelectedIndex = -1;
        }
    }
}