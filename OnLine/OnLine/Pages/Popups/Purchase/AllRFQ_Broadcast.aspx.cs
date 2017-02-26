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
using DBConn;

namespace OnLine.Pages.Popups.Purchase
{
    public partial class AllRFQ_Broadcast : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                checkAndFillGrid();
                fillListDropDown();
            }
        }

        protected void checkAndFillGrid()
        {
            Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];
            ActionLibrary.PurchaseActions._dispRFQDetails dspRFQ = new ActionLibrary.PurchaseActions._dispRFQDetails();

            Dictionary<String,String> BroadCastListDict=dspRFQ.getRFQBroadCastList(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), 
                User.Identity.Name,
                Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SELECTED_RFQ_ID].ToString());

            bool approvalContext = false;
            if (Request.QueryString.GetValues("approvalContext") != null)
                approvalContext = Request.QueryString.GetValues("approvalContext")[0].Equals("Y") ? true : false;

            if (approvalContext)
            {
                Button_Submit.Enabled = false;
                Label_Invalid_Broad_List.Visible = true;
                Label_Invalid_Broad_List.Text = "To Edit this broadcast list, please check purchase-> RFQ screen";
                Label_Invalid_Broad_List.ForeColor = System.Drawing.Color.Red;
            }
            else if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_RFQ] &&
            !accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                Button_Submit.Enabled = false;
            //Fill the gridview
            if (BroadCastListDict.Count > 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("name");
                int counter = 0;

                foreach (KeyValuePair<String, String> kvp in BroadCastListDict)
                {
                    dt.Rows.Add();
                    dt.Rows[counter]["name"] = kvp.Value.ToString();
                    counter++;
                }
                GridView_Broadcast_List.DataSource = dt;
                GridView_Broadcast_List.DataBind();
                GridView_Broadcast_List.Visible = true;


                if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_RFQ] &&
                !accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                {
                    GridView_Broadcast_List.Columns[0].Visible = false;                   
                }

                Label_Deletion_Not_Allowed.Visible = false;
                Label_Invalid_Broad_List.Visible = false;
                Label_No_Broadcast_List.Visible = false;
            }
            else
            {
                GridView_Broadcast_List.Visible = false;
                Label_No_Broadcast_List.Visible = true;
                
            }
        }

        protected void fillListDropDown()
        {
            if (DropDownList_BroadCast_List != null && DropDownList_BroadCast_List.Items.Count > 0)
                DropDownList_BroadCast_List.Items.Clear();

            ListItem lt1 = new ListItem();
            lt1.Text = BackEndObjects.RFQBroadcastList.RFQ_BROADCAST_TO_ALL;
            lt1.Value = BackEndObjects.RFQBroadcastList.RFQ_BROADCAST_TO_ALL;

            ListItem lt2 = new ListItem();
            lt2.Text = BackEndObjects.RFQBroadcastList.RFQ_BROADCAST_TO_ALL_INTERESTED;
            lt2.Value = BackEndObjects.RFQBroadcastList.RFQ_BROADCAST_TO_ALL_INTERESTED;
            
            DropDownList_BroadCast_List.Items.Add(lt1);
            DropDownList_BroadCast_List.Items.Add(lt2);

            ArrayList contactObjList = BackEndObjects.Contacts.getAllContactsbyEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            for (int i = 0; i < contactObjList.Count; i++)
            {
                BackEndObjects.Contacts contactObj = (BackEndObjects.Contacts)contactObjList[i];
                if (contactObj.getFromSite().Equals("Y",StringComparison.InvariantCultureIgnoreCase))
                {
                    ListItem lt = new ListItem();
                    lt.Text = contactObj.getContactName();
                    lt.Value = contactObj.getContactEntityId();
                    DropDownList_BroadCast_List.Items.Add(lt);
                }
            }

            
            DropDownList_BroadCast_List.SelectedIndex = -1;
        }
                

        protected void GridView_Broadcast_List_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow gVR = GridView_Broadcast_List.Rows[e.RowIndex];
            String contactNameToDelete = ((LinkButton)gVR.Cells[1].FindControl("LinkButton_Name")).Text;
            String contactToDelete="";

            ActionLibrary.PurchaseActions._dispRFQDetails dspRFQ = new ActionLibrary.PurchaseActions._dispRFQDetails();
            Dictionary<String, String> BroadCastListDict = dspRFQ.getRFQBroadCastList(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(),
                User.Identity.Name,
                Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SELECTED_RFQ_ID].ToString());

            foreach(KeyValuePair<String,String> kvp in BroadCastListDict)
                        contactToDelete = (contactNameToDelete.Equals(kvp.Value.ToString()) ? kvp.Key.ToString() : contactToDelete);
            

            String rfId=Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SELECTED_RFQ_ID].ToString();

            //Deletion only allowed if the RFQ is not approved already
            //Because if the RFQ is approved then chances are the 'to be deleted' entity has already got the RFQ as a lead and started 
            //communicating with this entity.
            //Deleteing at this point will cause confusion
            if (!BackEndObjects.RFQDetails.getRFQDetailsbyIdDB(rfId).getApprovalStat().
                Equals(BackEndObjects.RFQDetails.RFQ_APPROVAL_STAT_APPROVED))
            {
                Dictionary<String, String> whereCls = new Dictionary<string, string>();
                whereCls.Add(BackEndObjects.RFQBroadcastList.RFQ_BROADCAST_LIST_COL_RFQ_ID, rfId);
                whereCls.Add(BackEndObjects.RFQBroadcastList.RFQ_BROADCAST_LIST_COL_BROADCAST_TO, contactToDelete);

                BackEndObjects.RFQBroadcastList.updateRFQBroadcastListDB(new Dictionary<string, string>(), whereCls, Connections.OPERATION_DELETE);
                checkAndFillGrid();
            }
            else
                Label_Deletion_Not_Allowed.Visible = true;
        }

        protected void Button_Submit_Click(object sender, EventArgs e)
        {
            String broadCastToId = DropDownList_BroadCast_List.SelectedValue;
            Boolean invalidBroadCastVal = false;

            switch (broadCastToId)
            {
                case RFQBroadcastList.RFQ_BROADCAST_TO_ALL:
                    RFQBroadcastList existingList=RFQBroadcastList.
                        getRFQBroadcastListbyIdDB(Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SELECTED_RFQ_ID].ToString());
                    ArrayList allBrdList=existingList.getBroadcastList();

                    for (int i = 0; i < allBrdList.Count; i++)
                    {
                        if (allBrdList[i].ToString().Equals(RFQBroadcastList.RFQ_BROADCAST_TO_ALL_INTERESTED))
                        {
                            invalidBroadCastVal = true;
                            break;
                        }
                    }

                    break;


                case RFQBroadcastList.RFQ_BROADCAST_TO_ALL_INTERESTED:
                                       existingList=RFQBroadcastList.
                        getRFQBroadcastListbyIdDB(Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SELECTED_RFQ_ID].ToString());
                    allBrdList=existingList.getBroadcastList();

                    for (int i = 0; i < allBrdList.Count; i++)
                    {
                        if (allBrdList[i].ToString().Equals(RFQBroadcastList.RFQ_BROADCAST_TO_ALL))
                        {
                            invalidBroadCastVal = true;
                            break;
                        }
                    }

                    break;
            }

            if (!invalidBroadCastVal)
            {
                if (broadCastToId != null && !broadCastToId.Equals(""))
                {
                    ArrayList entIdList = new ArrayList();
                    entIdList.Add(broadCastToId);
                    BackEndObjects.RFQBroadcastList brdListObj = new BackEndObjects.RFQBroadcastList();
                    brdListObj.setRFQId(Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SELECTED_RFQ_ID].ToString());
                    brdListObj.setBroadcastList(entIdList);

                    BackEndObjects.RFQBroadcastList.insertRFQBroadcastListDB(brdListObj);
                    checkAndFillGrid();
                }
            }
            else
                Label_Invalid_Broad_List.Visible = true;
        }

        protected void GridView_Broadcast_List_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_Broadcast_List.PageIndex = e.NewPageIndex;
            checkAndFillGrid();
        }

        protected void gatherContactData(object sender, CommandEventArgs e)
        {

            GridViewRow gVR = GridView_Broadcast_List.Rows[Int32.Parse(e.CommandArgument.ToString())];
            String contact = ((LinkButton)gVR.Cells[1].FindControl("LinkButton_Name")).Text;
            String contactId = "";

            ActionLibrary.PurchaseActions._dispRFQDetails dspRFQ = new ActionLibrary.PurchaseActions._dispRFQDetails();
            Dictionary<String, String> BroadCastListDict = dspRFQ.getRFQBroadCastList(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(),
                User.Identity.Name,
                Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SELECTED_RFQ_ID].ToString());

            foreach (KeyValuePair<String, String> kvp in BroadCastListDict)
                contactId = (contact.Equals(kvp.Value.ToString()) ? kvp.Key.ToString() : contactId);

            //String rfqEntity = RFQDetails.getRFQDetailsbyIdDB(Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SELECTED_RFQ_ID].ToString()).getEntityId();
            Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_BROADCASTLIST_SELECTED_CONTACT] = ActionLibrary.customerDetails.
                getContactDetails(contactId, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

            //Server.Transfer("AllRFQ_Broadcast_customer_Details.aspx", true);
            Response.Redirect("AllRFQ_Broadcast_customer_Details.aspx");
        }

        protected void LinkButton_Create_Contact_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/createContact.aspx";
            forwardString += "?parentContext=" + "broadcast";
            //Server.Transfer("createContact.aspx",true);

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispContactBroadcast", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=900');", true);

        }

        protected void Button_Refresh_Click(object sender, EventArgs e)
        {
            fillListDropDown();
        }

    }
}