using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BackEndObjects;
using ActionLibrary;
using System.Data;
using System.Collections;

namespace OnLine.Pages.Popups.Product
{
    public partial class AllProd_SO : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                loadSODetails();
        }

        protected void loadSODetails()
        {
            String prodName = Request.QueryString.GetValues("prodName")[0];

            Dictionary<String, PurchaseOrderQuote> poDict = PurchaseOrder.getPurchaseOrdersForProdNameAndRespEntDB(prodName, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            Dictionary<String, float> rfqDeliveredQnty = new Dictionary<string, float>();

            ArrayList rfqList = new ArrayList();
            foreach (KeyValuePair<String, PurchaseOrderQuote> kvp in poDict)
                            rfqList.Add(kvp.Key.Split(new String[] { "-" }, StringSplitOptions.RemoveEmptyEntries)[1]);

            if (rfqList.Count > 0)
                rfqDeliveredQnty = PurchaseOrder.getTotalDeliveredQntyByRFQIdAndProductName(rfqList, prodName);

            DataTable dt = new DataTable();
            dt.Columns.Add("So_No");
            dt.Columns.Add("RFQNo");
            dt.Columns.Add("delivered");
            dt.Columns.Add("quote");
            dt.Columns.Add("units");
            int counter=0;
            float unitsToDeliver=0 ,unitsDelivered= 0;

            foreach (KeyValuePair<String, PurchaseOrderQuote> kvp in poDict)
            {
                PurchaseOrderQuote quoteObj=kvp.Value;
                dt.Rows.Add();
                dt.Rows[counter]["So_No"] = quoteObj.getPo_id();
                dt.Rows[counter]["RFQNo"] =kvp.Key.Split(new String[]{"-"},StringSplitOptions.RemoveEmptyEntries)[1];
                dt.Rows[counter]["delivered"] = rfqDeliveredQnty.ContainsKey(dt.Rows[counter]["RFQNo"].ToString()) ? 
                    rfqDeliveredQnty[dt.Rows[counter]["RFQNo"].ToString()] : 0;

                unitsDelivered+=float.Parse(dt.Rows[counter]["delivered"].ToString());

                dt.Rows[counter]["quote"] = quoteObj.getQuote();
                dt.Rows[counter]["units"] = quoteObj.getUnits();
                
                //if(dt.Rows[counter]["delivered"].Equals("N"))
                    unitsToDeliver += quoteObj.getUnits();

                counter++;
            }

            if (dt.Rows.Count > 0)
            {
                GridView1.Visible = true;
                GridView1.DataSource = dt;
                GridView1.DataBind();
                Label_Total_Text.Text = "Total units of this product ordered:";
                Label_Total_No.Text = unitsToDeliver.ToString();
                Label_Total_Delivered_Text.Text = "Total units delivered as per invoice records (only considering invoices where 'Delivery Status' is set to 'Delivered':";
                Label_Total_Delivered_No.Text=unitsDelivered.ToString();
                
                Label_Total_No.Visible = true;
                Label_Total_Text.Visible = true;
                Label_Total_Delivered_Text.Visible = true;
                Label_Total_Delivered_No.Visible = true;

                Label_Total_No.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                Label_Empty_Grid.Visible = true;
                Label_Empty_Grid.Text = "No Sales Order Received for this product";
                Label_Empty_Grid.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void LinkButton_Show_SO_Command(object sender, CommandEventArgs e)
        {
            String forwardString = "/Pages/Popups/Purchase/PO_Details.aspx";
            forwardString += "?rfId=" + ((Label)GridView1.Rows[Convert.ToInt32(e.CommandArgument)].Cells[0].FindControl("Label_RFQNo")).Text;
            forwardString += "&context=" + "vendor";            
            forwardString += "&subContext=" + "SOFromProduct";
            forwardString += "&poId=" + ((LinkButton)GridView1.Rows[Convert.ToInt32(e.CommandArgument)].Cells[0].FindControl("LinkButton_Show_SO")).Text;
            //Make the SO non-editable
            forwardString += "&createMode=" + BackEndObjects.RFQShortlisted.POTENTIAL_CREATION_MODE_AUTO;
            Server.Transfer(forwardString);
            //ScriptManager.RegisterStartupScript(this, typeof(string), "DispPOSales", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=900');", true);

        }
    }
}