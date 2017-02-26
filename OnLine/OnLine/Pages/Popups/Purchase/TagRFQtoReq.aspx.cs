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
    public partial class TagRFQtoReq : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
             if (!Page.IsPostBack)
            {
                populateReqList();
            }
        }

        protected void populateReqList()
        {
            ArrayList reqList=BackEndObjects.Requirement.getAllRequirementsbyEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

            for (int i = 0; i < reqList.Count; i++)
            {
                ListItem lt = new ListItem();
                BackEndObjects.Requirement reqObj = (BackEndObjects.Requirement)reqList[i];
                lt.Text = reqObj.getReqName();
                lt.Value = reqObj.getReqId();

                DropDownList_Req_List.Items.Add(lt);
            }

            ListItem ltFirst = new ListItem();
            ltFirst.Text = "_";
            ltFirst.Value = "_";

            DropDownList_Req_List.Items.Add(ltFirst);
            DropDownList_Req_List.SelectedValue = "_";
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            String rfqId = Request.QueryString.GetValues("contextId1")[0];
            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            Dictionary<String, String> targetVals = new Dictionary<string, string>();
           
            whereCls.Add(BackEndObjects.RFQDetails.RFQ_COL_RFQ_ID, rfqId);
            targetVals.Add(BackEndObjects.RFQDetails.RFQ_COL_RELATED_REQ, DropDownList_Req_List.SelectedValue);

            try
            {
                BackEndObjects.RFQDetails.updateRFQDetailsDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
                Label_Update_Stat.Visible = true;
                Label_Update_Stat.Text = "RFQ tagged to requirement successfully.Now You can track the progress of the requirement from requirement section in Purchase screen";
                Label_Update_Stat.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                Label_Update_Stat.Visible = true;
                Label_Update_Stat.Text = "Tagging Failed";
                Label_Update_Stat.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}