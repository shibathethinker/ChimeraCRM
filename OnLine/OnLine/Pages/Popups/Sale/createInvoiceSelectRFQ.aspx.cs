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


namespace OnLine.Pages.Popups.Sale
{
    public partial class createInvoiceSelectRFQ : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                populateRFQList();
            }
        }

        protected void populateRFQList()
        {
            String RespEntityId = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();
            ActionLibrary.SalesActions._dispPotentials dP = new SalesActions._dispPotentials();

            ArrayList potList = dP.getAllPotentials(RespEntityId, User.Identity.Name);

            for (int i = 0; i < potList.Count; i++)
            {
                PotentialRecords potRec = (PotentialRecords)potList[i];                

                if(potRec.getPotStat().Equals(BackEndObjects.RFQShortlisted.POTENTIAL_ACTIVE_STAT_CLOSED_WON))
                {
                ListItem ltRFQ = new ListItem();
                ltRFQ.Text = potRec.getRFQName();
                ltRFQ.Value=potRec.getRFQId();                    

                BackEndObjects.Invoice invObj = BackEndObjects.Invoice.getInvoicebyRfIdDB(potRec.getRFQId());
                if (invObj != null && invObj.getInvoiceId() != null && !invObj.getInvoiceId().Equals(""))
                    ltRFQ.Attributes.Add("style", "background-color:Green;");

                DropDownList1.Items.Add(ltRFQ);
                }
            }

        }

        protected void Button_With_RFQ_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/createInvoice.aspx";
            forwardString += "?rfId=" + DropDownList1.SelectedValue;
            ScriptManager.RegisterStartupScript(this, typeof(string), "CreateInv", 
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=900');", true);
        }

        protected void Button_Without_RFQ_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/createInvoice.aspx";
            forwardString += "?rfId=" + "";
            ScriptManager.RegisterStartupScript(this, typeof(string), "CreateInv",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=900');", true);

        }
    }
}