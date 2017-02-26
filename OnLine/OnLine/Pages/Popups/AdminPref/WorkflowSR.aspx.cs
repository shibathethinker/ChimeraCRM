using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BackEndObjects;
using System.Data;
using ActionLibrary;
using System.Collections;

namespace OnLine.Pages.Popups.AdminPref
{
    public partial class WorkflowSR : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ((Menu)Master.FindControl("Menu_WorkFlow_Master")).Items[3].Selected = true;

                loadSLAandAlertDetails();
                loadDefectSev();
            }
        }

        protected  void loadDefectSev()
        {
            ListItem lt0 = new ListItem();
            lt0.Text = "";
            lt0.Value = "";

            ListItem lt1 = new ListItem();
            lt1.Text = "High";
            lt1.Value = "High";

            ListItem lt2 = new ListItem();
            lt2.Text = "Medium";
            lt2.Text = "Medium";

            ListItem lt3 = new ListItem();
            lt3.Text = "Low";
            lt3.Value = "Low";

            DropDownList_Sev.Items.Add(lt0);
            DropDownList_Sev.Items.Add(lt1);
            DropDownList_Sev.Items.Add(lt2);
            DropDownList_Sev.Items.Add(lt3);

            DropDownList_Sev.SelectedValue = "";
        }
        protected void loadSLAandAlertDetails()
        {
            MainBusinessEntity mBEObj=MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            TextBox_Alert_Email.Text = mBEObj.getSupportEmail();
            TextBox_New_Defect_Email.Text = mBEObj.getNewSREmailBody();
            TextBox_Resolved_Defect_Email.Text = mBEObj.getResolvedSRBody();
            
            ArrayList slaList= DefectSLA.getDefectSLADetailsbyentIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), DefectSLA.DEFECT_TYPE_SERVICE_REQUEST);
            if (slaList != null && slaList.Count > 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Hidden_Type");
                dt.Columns.Add("sev");
                dt.Columns.Add("sla");
                dt.Columns.Add("alertBefore");

                for (int i = 0; i < slaList.Count; i++)
                {
                    DefectSLA slaObj=(DefectSLA)slaList[i];
                    dt.Rows.Add();
                    dt.Rows[i]["Hidden_Type"] = slaObj.getContext_Type();
                    dt.Rows[i]["sev"] = slaObj.getSeverity();
                    dt.Rows[i]["sla"] = slaObj.getSLA();
                    dt.Rows[i]["alertBefore"] = slaObj.getAlert_Before();

                }

                GridView1.DataSource = dt;
                GridView1.DataBind();
                GridView1.Visible = true;
                ViewState["WorkflowDefectSLA" + Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()] = dt;
            }
        }

        protected void Button_Add_Email_Click(object sender, EventArgs e)
        {
            Dictionary<String,String> whereCls=new Dictionary<String,String>();
            Dictionary<String,String> targetVals=new Dictionary<string,string>();

            whereCls.Add(MainBusinessEntity.MAIN_BUSINESS_COL_BUSINESS_ID, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

            targetVals.Add(MainBusinessEntity.MAIN_BUSINESS_COL_SUPPORT_EMAIL, TextBox_Alert_Email.Text);
            targetVals.Add(MainBusinessEntity.MAIN_BUSINESS_COL_SUPPORT_PASS, TextBox_Alert_Pass.Text);

            try
            {
                MainBusinessEntity.updateMainBusinessEntityWOimg_prd_user_subDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
                Label_Email_Stat.Text = "Support email details updated successfully. This email id will be used to send auto alerts to clients";
                Label_Email_Stat.ForeColor = System.Drawing.Color.Green;
                Label_Email_Stat.Visible = true;
            }
            catch (Exception ex)
            {
                Label_Email_Stat.Text = "Update Failed";
                Label_Email_Stat.ForeColor = System.Drawing.Color.Red;
                Label_Email_Stat.Visible = true;
            }
        }

        protected void Button_Add_SLA_Rule_Click(object sender, EventArgs e)
        {
            if (DropDownList_Sev.SelectedValue.Equals(""))
            {
                Label_SLA_Exists.Visible = true;
                Label_SLA_Exists.Text = "Select one severity from the drop down";
            }
            else
            {
                Label_SLA_Exists.Visible = false;
                DataTable dt = (DataTable)ViewState["WorkflowDefectSLA" + Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()];
                Dictionary<String, String> existingSevList = null;
                if (dt == null)
                {
                    dt = new DataTable();
                    dt.Columns.Add("Hidden_Type");
                    dt.Columns.Add("sev");
                    dt.Columns.Add("sla");
                    dt.Columns.Add("alertBefore");
                }
                else
                {
                    existingSevList = new Dictionary<string, string>();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (!existingSevList.ContainsKey(dt.Rows[i]["sev"].ToString()))
                            existingSevList.Add(dt.Rows[i]["sev"].ToString(), "");
                    }
                }
                int count = dt.Rows.Count;

                if (existingSevList != null && existingSevList.ContainsKey(DropDownList_Sev.SelectedValue))
                {
                    Label_SLA_Exists.Visible = true;
                    Label_SLA_Exists.Text = "SLA exists for this severity. You can delete from the below list and re-enter";
                    Label_SLA_Exists.Focus();
                }
                else
                {
                    Label_SLA_Exists.Visible = false;
                    dt.Rows.Add();
                    dt.Rows[count]["Hidden_Type"] = DefectSLA.DEFECT_TYPE_SERVICE_REQUEST;
                    dt.Rows[count]["sev"] = DropDownList_Sev.SelectedValue;
                    dt.Rows[count]["sla"] = TextBox_SLA_Hr.Text;
                    dt.Rows[count]["alertBefore"] = TextBox_SLA_Alert.Text;
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    GridView1.Visible = true;
                    ViewState["WorkflowDefectSLA" + Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()] = dt;

                    DefectSLA slaObj = new DefectSLA();
                    slaObj.setAlert_Before(TextBox_SLA_Alert.Text);
                    slaObj.setContext_Type(DefectSLA.DEFECT_TYPE_SERVICE_REQUEST);
                    slaObj.setEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                    slaObj.setSeverity(DropDownList_Sev.SelectedValue);
                    slaObj.setSLA(TextBox_SLA_Hr.Text);
                    DefectSLA.insertDefectSLADetails(slaObj);

                    Button_Add_SLA_Rule.Focus();
                }
            }
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            String sev = ((Label)GridView1.Rows[e.RowIndex].Cells[0].FindControl("Label_Sev")).Text;
            String type = ((Label)GridView1.Rows[e.RowIndex].Cells[0].FindControl("Label_Hidden")).Text;

            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            Dictionary<String, String> targetVals = new Dictionary<string, string>();

            whereCls.Add(DefectSLA.DEFECT_SLA_COL_ENT_ID, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            whereCls.Add(DefectSLA.DEFECT_SLA_COL_SEV, sev);
            whereCls.Add(DefectSLA.DEFECT_SLA_COL_CONTXT_TYPE, type);

            DefectSLA.updateDefectDetails(targetVals, whereCls, DBConn.Connections.OPERATION_DELETE);

            DataTable dt = (DataTable)ViewState["WorkflowDefectSLA" + Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()];
            int index = GridView1.Rows[e.RowIndex].DataItemIndex;
            dt.Rows[index].Delete();

            GridView1.DataSource = dt;
            GridView1.DataBind();
            ViewState["WorkflowDefectSLA" + Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()] = dt;
        }

        protected void Button_New_Email_Body_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            Dictionary<String, String> targetVals = new Dictionary<string, string>();
            whereCls.Add(MainBusinessEntity.MAIN_BUSINESS_COL_BUSINESS_ID, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            targetVals.Add(MainBusinessEntity.MAIN_BUSINESS_COL_SR_NEW_BODY, TextBox_New_Defect_Email.Text.Trim());

            try
            {
             MainBusinessEntity.updateMainBusinessEntityWOimg_prd_user_subDB(targetVals,whereCls,DBConn.Connections.OPERATION_UPDATE);
                Label_Email_Stat.Visible=true;
                Label_Email_Stat.Text="Email details updated successfully";
                Label_Email_Stat.ForeColor=System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                Label_Email_Stat.Visible = true;
                Label_Email_Stat.Text = "Email details update failed";
                Label_Email_Stat.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void Button_Resolved_Email_Body_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            Dictionary<String, String> targetVals = new Dictionary<string, string>();
            whereCls.Add(MainBusinessEntity.MAIN_BUSINESS_COL_BUSINESS_ID, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            targetVals.Add(MainBusinessEntity.MAIN_BUSINESS_COL_SR_RESOLVED_BODY, TextBox_Resolved_Defect_Email.Text.Trim());

            try
            {
                MainBusinessEntity.updateMainBusinessEntityWOimg_prd_user_subDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
                Label_Email_Stat.Visible = true;
                Label_Email_Stat.Text = "Email details updated successfully";
                Label_Email_Stat.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                Label_Email_Stat.Visible = true;
                Label_Email_Stat.Text = "Email details update failed";
                Label_Email_Stat.ForeColor = System.Drawing.Color.Red;
            }
        }
        }
    
}