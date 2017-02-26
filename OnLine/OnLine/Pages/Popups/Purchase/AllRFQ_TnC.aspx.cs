using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using BackEndObjects;
using ActionLibrary;


namespace OnLine.Pages.Popups.Purchase
{
    public partial class AllRFQ_TnC : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                String selectedRFQ = Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SELECTED_RFQ_ID].ToString();
                BackEndObjects.RFQDetails rfqObj = BackEndObjects.RFQDetails.getRFQDetailsbyIdDB(selectedRFQ);

                bool approvalContext = false;
                if (Request.QueryString.GetValues("approvalContext") != null)
                    approvalContext = Request.QueryString.GetValues("approvalContext")[0].Equals("Y") ? true : false;

                Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];
                if (approvalContext)
                {
                    Button_TnC.Enabled = false; TextBox_TnC.Enabled = false;
                    Label_TnC_Stat.Visible = true;
                    Label_TnC_Stat.Text = "To Edit this T & C, please check the details from Purchase->RFQ screen";
                    Label_TnC_Stat.ForeColor = System.Drawing.Color.Red;
                }
                else if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS] ||
                    accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_RFQ])
                {
                    Button_TnC.Enabled = true; TextBox_TnC.Enabled = true;
                }
                else
                {
                    Label_TnC_Stat.Visible = true;
                    Label_TnC_Stat.Text = "You dont have edit access to Lead records";
                    Label_TnC_Stat.ForeColor = System.Drawing.Color.Red;
                }

                String tnCText = rfqObj.getTermsandConds();

                TextBox_TnC.Text = (!tnCText.Equals("") ? tnCText : TextBox_TnC.Text);
            }
        }

        protected void Button_TnC_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            Dictionary<String, String> tagetVals = new Dictionary<string, string>();

            whereCls.Add(BackEndObjects.RFQDetails.RFQ_COL_RFQ_ID, Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SELECTED_RFQ_ID].ToString());
            tagetVals.Add(BackEndObjects.RFQDetails.RFQ_COL_T_AND_C, TextBox_TnC.Text);

            try
            {
                BackEndObjects.RFQDetails.updateRFQDetailsDB(tagetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
                Label_TnC_Stat.Visible = true;
                Label_TnC_Stat.Text = "T&C details updated successfully";
                Label_TnC_Stat.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                Label_TnC_Stat.Visible = true;
                Label_TnC_Stat.Text = "T&C details update Failed";
                Label_TnC_Stat.ForeColor = System.Drawing.Color.Red;
            }
        }



    }
}