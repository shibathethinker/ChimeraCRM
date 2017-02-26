using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BackEndObjects;
using System.Collections;
using ActionLibrary;
using System.Data;


namespace OnLine.Pages
{
    public partial class Workflow_Tree : System.Web.UI.Page
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
            String contextId = null;
            String contextName = null;
            String approverContext = null; 

            if (Request.QueryString.GetValues("contextId") != null)
                contextId = Request.QueryString.GetValues("contextId")[0];
            if (Request.QueryString.GetValues("contextName") != null)
                contextName = Request.QueryString.GetValues("contextName")[0];
            if (Request.QueryString.GetValues("approvalContext") != null)
                approverContext = Request.QueryString.GetValues("approvalContext")[0];

            if (approverContext!=null && approverContext.Equals("N", StringComparison.InvariantCultureIgnoreCase))
            {
                TextBox_Comment.Visible = false;
                Button_Approve.Visible = false;
                Button_Reject.Visible = false;
                Label_Comments.Visible = false;
            }

            ArrayList actionList = BackEndObjects.Workflow_Action.getWorkflowActionHistoryForContextIdandEntId(contextId, contextName, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

            DataTable dt = new DataTable();
            dt.Columns.Add("action");
            dt.Columns.Add("usrid");
            dt.Columns.Add("comment");
            dt.Columns.Add("timestamp");

            for (int i = 0; i < actionList.Count; i++)
            {
                dt.Rows.Add();
                Workflow_Action actionObj=(Workflow_Action)actionList[i];

                dt.Rows[i]["action"] = actionObj.getActionTaken();
                dt.Rows[i]["usrid"] = actionObj.getUserId();
                dt.Rows[i]["comment"] = actionObj.getComment();
                dt.Rows[i]["timestamp"] = actionObj.getActionDateTime();
            }

            GridView1.DataSource = dt;
            GridView1.DataBind();
            GridView1.Visible = true;
        }

        protected void Button_Approve_Click(object sender, EventArgs e)
        {
            String contextName = null;
            String contextId=null;
            int level = 0,currentLevel=0;
            Workflow_Action actionObj = null;
            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            Dictionary<String, String> targetVals = new Dictionary<string, string>();

            if (Request.QueryString.GetValues("contextName") != null)
                contextName = Request.QueryString.GetValues("contextName")[0];

                        if (Request.QueryString.GetValues("contextId") != null)
                contextId = Request.QueryString.GetValues("contextId")[0];

            if (contextName.Equals(BackEndObjects.Workflow_Action.WORKFLOW_ACTION_CONTEXT_NAME_RFQ))
                level = BackEndObjects.MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()).
                    getRfqApprovalLevel();
            else if (contextName.Equals(BackEndObjects.Workflow_Action.WORKFLOW_ACTION_CONTEXT_NAME_INV))
                level = BackEndObjects.MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()).
                    getInvApprovalLevel();

                String reportingToUser = BackEndObjects.userDetails.
                    getUserDetailsbyIdDB(User.Identity.Name, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()).getReportsTo();

                if (contextName.Equals(BackEndObjects.Workflow_Action.WORKFLOW_ACTION_CONTEXT_NAME_RFQ))
                {
                    //Get the current level of the context
                    currentLevel=Int32.Parse(BackEndObjects.RFQDetails.getRFQDetailsbyIdDB(contextId).getApprovalLevel());

                    if (currentLevel + 1 >= level) //All levels reached
                    {                        
                        whereCls.Add(BackEndObjects.RFQDetails.RFQ_COL_RFQ_ID, contextId);                        
                        targetVals.Add(BackEndObjects.RFQDetails.RFQ_COL_APPROVAL_STAT, 
                            BackEndObjects.RFQDetails.RFQ_APPROVAL_STAT_APPROVED);
                        targetVals.Add(BackEndObjects.RFQDetails.RFQ_COL_APPROVAL_LEVEL,
                            (currentLevel+1).ToString());
                    }
                    else
                    {
                        whereCls.Add(BackEndObjects.RFQDetails.RFQ_COL_RFQ_ID, contextId);
                        targetVals.Add(BackEndObjects.RFQDetails.RFQ_COL_APPROVAL_STAT,
                            reportingToUser);
                        targetVals.Add(BackEndObjects.RFQDetails.RFQ_COL_APPROVAL_LEVEL,
                            (currentLevel + 1).ToString());
                    }

                        actionObj = new Workflow_Action();
                        actionObj.setContextId(contextId);
                        actionObj.setContextName(contextName);
                        actionObj.setUserId(User.Identity.Name);
                        actionObj.setEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                        actionObj.setActionTaken(BackEndObjects.Workflow_Action.WORKFLOW_ACTION_ACTION_TAKEN_RFQ_APPROVED);
                        actionObj.setComment(TextBox_Comment.Text.Trim());
                        actionObj.setActionDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        
                        BackEndObjects.RFQDetails.updateRFQDetailsDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
                        BackEndObjects.Workflow_Action.insertWorkflowActionObject(actionObj);

                        ScriptManager.RegisterStartupScript(this, typeof(string), "RefreshRfqGrid", "RefreshParentRFQ();", true);
                }
                else if (contextName.Equals(BackEndObjects.Workflow_Action.WORKFLOW_ACTION_CONTEXT_NAME_INV))
                {
                                        //Get the current level of the context
                    currentLevel=Int32.Parse(BackEndObjects.Invoice.getInvoicebyIdDB(contextId).getApprovalLevel());

                    if (currentLevel + 1 >= level) //All levels reached
                    {                        
                        whereCls.Add(BackEndObjects.Invoice.INVOICE_COL_INVOICE_ID, contextId);
                        targetVals.Add(BackEndObjects.Invoice.INVOICE_COL_APPROVAL_STAT, 
                            BackEndObjects.Invoice.INVOICE_APPROVAL_STAT_APPROVED);
                        targetVals.Add(BackEndObjects.Invoice.INVOICE_COL_APPROVAL_LEVEL,
                            (currentLevel + 1).ToString());
                    }
                    else
                    {
                        whereCls.Add(BackEndObjects.Invoice.INVOICE_COL_INVOICE_ID, contextId);
                        targetVals.Add(BackEndObjects.Invoice.INVOICE_COL_APPROVAL_STAT,
                            reportingToUser);
                        targetVals.Add(BackEndObjects.Invoice.INVOICE_COL_APPROVAL_LEVEL,
                            (currentLevel + 1).ToString());

                    }

                        actionObj = new Workflow_Action();
                        actionObj.setContextId(contextId);
                        actionObj.setContextName(contextName);
                        actionObj.setUserId(User.Identity.Name);
                        actionObj.setEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                        actionObj.setActionTaken(BackEndObjects.Workflow_Action.WORKFLOW_ACTION_ACTION_TAKEN_INV_APPROVED);
                        actionObj.setComment(TextBox_Comment.Text.Trim());
                        actionObj.setActionDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                        BackEndObjects.Invoice.updateInvoiceDetailsDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
                        BackEndObjects.Workflow_Action.insertWorkflowActionObject(actionObj);

                        ScriptManager.RegisterStartupScript(this, typeof(string), "RefreshInvGrid", "RefreshParentInv();", true);
                }


                Button_Approve.Enabled = false;
                Button_Reject.Enabled = false;
        }

        protected void Button_Reject_Click(object sender, EventArgs e)
        {
            String contextName = null;
            String contextId = null;
            int level = 0, currentLevel = 0;
            Workflow_Action actionObj = null;
            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            Dictionary<String, String> targetVals = new Dictionary<string, string>();

            if (Request.QueryString.GetValues("contextName") != null)
                contextName = Request.QueryString.GetValues("contextName")[0];

            if (Request.QueryString.GetValues("contextId") != null)
                contextId = Request.QueryString.GetValues("contextId")[0];

            if (contextName.Equals(BackEndObjects.Workflow_Action.WORKFLOW_ACTION_CONTEXT_NAME_RFQ))
                level = BackEndObjects.MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()).
                    getRfqApprovalLevel();
            else if (contextName.Equals(BackEndObjects.Workflow_Action.WORKFLOW_ACTION_CONTEXT_NAME_INV))
                level = BackEndObjects.MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()).
                    getInvApprovalLevel();

            String reportingToUser = BackEndObjects.userDetails.
                getUserDetailsbyIdDB(User.Identity.Name, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()).getReportsTo();

            if (contextName.Equals(BackEndObjects.Workflow_Action.WORKFLOW_ACTION_CONTEXT_NAME_RFQ))
            {

                    whereCls.Add(BackEndObjects.RFQDetails.RFQ_COL_RFQ_ID, contextId);
                    targetVals.Add(BackEndObjects.RFQDetails.RFQ_COL_APPROVAL_STAT,
                        BackEndObjects.Workflow_Action.WORKFLOW_ACTION_ACTION_TAKEN_REJECTED);
                    targetVals.Add(BackEndObjects.RFQDetails.RFQ_COL_APPROVAL_LEVEL,
                        (currentLevel + 1).ToString());

                actionObj = new Workflow_Action();
                actionObj.setContextId(contextId);
                actionObj.setContextName(contextName);
                actionObj.setUserId(User.Identity.Name);
                actionObj.setEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                actionObj.setActionTaken(BackEndObjects.Workflow_Action.WORKFLOW_ACTION_ACTION_TAKEN_REJECTED);
                actionObj.setComment(TextBox_Comment.Text.Trim());
                actionObj.setActionDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                BackEndObjects.RFQDetails.updateRFQDetailsDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
                BackEndObjects.Workflow_Action.insertWorkflowActionObject(actionObj);

                ScriptManager.RegisterStartupScript(this, typeof(string), "RefreshRfqGrid", "RefreshParentRFQ();", true);
            }
            else if (contextName.Equals(BackEndObjects.Workflow_Action.WORKFLOW_ACTION_CONTEXT_NAME_INV))
            {
                    whereCls.Add(BackEndObjects.Invoice.INVOICE_COL_INVOICE_ID, contextId);
                    targetVals.Add(BackEndObjects.Invoice.INVOICE_COL_APPROVAL_STAT,
                        BackEndObjects.Workflow_Action.WORKFLOW_ACTION_ACTION_TAKEN_REJECTED);
                    targetVals.Add(BackEndObjects.Invoice.INVOICE_COL_APPROVAL_LEVEL,
                        (currentLevel + 1).ToString());

                actionObj = new Workflow_Action();
                actionObj.setContextId(contextId);
                actionObj.setContextName(contextName);
                actionObj.setUserId(User.Identity.Name);
                actionObj.setEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                actionObj.setActionTaken(BackEndObjects.Workflow_Action.WORKFLOW_ACTION_ACTION_TAKEN_REJECTED);
                actionObj.setComment(TextBox_Comment.Text.Trim());
                actionObj.setActionDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                BackEndObjects.Invoice.updateInvoiceDetailsDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
                BackEndObjects.Workflow_Action.insertWorkflowActionObject(actionObj);

                ScriptManager.RegisterStartupScript(this, typeof(string), "RefreshInvGrid", "RefreshParentInv();", true);
            }

            Button_Approve.Enabled = false;
            Button_Reject.Enabled = false;
        }
    }
}