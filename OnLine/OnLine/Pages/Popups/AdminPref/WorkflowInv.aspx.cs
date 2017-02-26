using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BackEndObjects;
using ActionLibrary;

namespace OnLine.Pages.Popups.AdminPref
{
    public partial class WorkflowInv : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int Level = BackEndObjects.MainBusinessEntity.
                    getMainBusinessEntitybyIdwithLessDetailsDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()).getInvApprovalLevel();
                TextBox_Apprv_Level.Text = Level.ToString();
                ((Menu)Master.FindControl("Menu_WorkFlow_Master")).Items[2].Selected = true;
            }
        }

        protected void Button_Submit_Inv_Level_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            Dictionary<String, String> targetVals = new Dictionary<string, string>();
            String newLevel = TextBox_Apprv_Level.Text.Trim().Equals("") ? "0" : TextBox_Apprv_Level.Text.Trim();

            whereCls.Add(BackEndObjects.MainBusinessEntity.MAIN_BUSINESS_COL_BUSINESS_ID, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            targetVals.Add(BackEndObjects.MainBusinessEntity.MAIN_BUSINESS_COL_INV_APPROVAL_LEVEL, newLevel);

            try
            {
                BackEndObjects.MainBusinessEntity.updateMainBusinessEntityWOimg_prd_user_subDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
                Label_Stat.Visible = true;
                Label_Stat.Text = "Approval level updated successfully.Already approved Invoices will not be impacted";
                Label_Stat.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                Label_Stat.Visible = true;
                Label_Stat.Text = "Approval level updated failed";
                Label_Stat.ForeColor = System.Drawing.Color.Red;
            }

        }
    }
}