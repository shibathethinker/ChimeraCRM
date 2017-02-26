using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActionLibrary;
using BackEndObjects;
using System.Data;
using System.Collections;

namespace OnLine.Pages.Popups.AdminPref
{
    public partial class AccessMgmt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                fillPurchaseGrid();
                fillSalesGrid();
                fillProductGrid();
                fillDefectGrid();
                fillSRGrid();
                fillContactGrid();
                fillDashboardGrid();
                fillAdminGrid();
                fillAllGroupList();
            }
        }

        protected void fillAllGroupList()
        {
            String entId = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();
            Dictionary<String,ArrayList> accessList=BackEndObjects.EntityAccessListRecord.getCompleteAccessListbyEntId(entId);
            Dictionary<String, String> existingGroupNamesList = new Dictionary<string, string>();

            DataTable dt = new DataTable();
            dt.Columns.Add("Group Name");
            dt.Columns.Add("2");

            int counter = 0;

            foreach (KeyValuePair<String, ArrayList> kvp in accessList)
            {
                dt.Rows.Add();
                dt.Rows[counter]["Group Name"] = kvp.Key;
                existingGroupNamesList.Add(kvp.Key, kvp.Key);
                counter++;
            }

            GridView_Existing_List.DataSource = dt;
            GridView_Existing_List.DataBind();

            Session[SessionFactory.ADMIN_PREF_ACCESS_MGMT_EXISTING_GROUP_DATAGRID] = dt;
            Session[SessionFactory.ADMIN_PREF_ACCESS_MGMT_EXISTING_GROUP_LIST] = existingGroupNamesList;
        }

        protected void fillPurchaseGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("1");
            dt.Columns.Add("2");
            dt.Columns.Add("3");
            dt.Columns.Add("4");
            dt.Columns.Add("5");
            dt.Columns.Add("6");
            dt.Columns.Add("7");
            dt.Columns.Add("8");
            dt.Columns.Add("9");
            dt.Columns.Add("10");

            dt.Rows.Add();
            
            GridView_Purchase.DataSource = dt;
            GridView_Purchase.DataBind();

        }

        protected void fillSalesGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("1");
            dt.Columns.Add("2");
            dt.Columns.Add("3");
            dt.Columns.Add("4");
            dt.Columns.Add("5");
            dt.Columns.Add("6");
            dt.Columns.Add("7");
            dt.Columns.Add("8");

            dt.Rows.Add();

            Gridview_Sales.DataSource = dt;
            Gridview_Sales.DataBind();
            
        }

        protected void fillContactGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("1");
            dt.Columns.Add("2");
            dt.Columns.Add("3");
    
            dt.Rows.Add();

            GridView_Contacts.DataSource = dt;
            GridView_Contacts.DataBind();
        }
        
        protected void fillProductGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("1");
            dt.Columns.Add("2");
            dt.Columns.Add("3");

            dt.Rows.Add();

            GridView_Product.DataSource = dt;
            GridView_Product.DataBind();
        }

        protected void fillDefectGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("1");
            dt.Columns.Add("2");
            dt.Columns.Add("3");
            dt.Columns.Add("4");
            dt.Columns.Add("5");

            dt.Rows.Add();

            GridView_Defect.DataSource = dt;
            GridView_Defect.DataBind();
        }

        protected void fillSRGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("1");
            dt.Columns.Add("2");
            dt.Columns.Add("3");
            dt.Columns.Add("4");
            dt.Columns.Add("5");

            dt.Rows.Add();

            GridView_SR.DataSource = dt;
            GridView_SR.DataBind();
        }
        
        protected void fillDashboardGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("1");
            dt.Columns.Add("2");
            dt.Columns.Add("3");
            dt.Columns.Add("4");
            dt.Columns.Add("5");
            dt.Columns.Add("6");
            dt.Columns.Add("7");
            dt.Columns.Add("8");
            dt.Columns.Add("9");
            dt.Columns.Add("10");
            dt.Columns.Add("11");
            dt.Columns.Add("12");
            dt.Columns.Add("13");
            dt.Columns.Add("14");
            dt.Columns.Add("15");

            dt.Rows.Add();

            GridView_Dashboard.DataSource = dt;
            GridView_Dashboard.DataBind();
        }
        
        protected void fillAdminGrid()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("2");
            dt.Columns.Add("3");
            dt.Columns.Add("4");
            dt.Columns.Add("5");
            dt.Columns.Add("6");
            dt.Columns.Add("7");
            dt.Columns.Add("8");

            dt.Rows.Add();

            GridView_Admin.DataSource = dt;
            GridView_Admin.DataBind();

        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void Button_Filter_Click(object sender, EventArgs e)
        {
            if (TextBox_Group_Name.Equals(""))
            {
                fillAllGroupList();
            }
            else
            {
                String entId = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();
                Dictionary<String, ArrayList> accessList = BackEndObjects.EntityAccessListRecord.getCompleteAccessListbyEntId(entId);

                DataTable dt = new DataTable();
                dt.Columns.Add("Group Name");
                dt.Columns.Add("2");

                int counter = 0;

                foreach (KeyValuePair<String, ArrayList> kvp in accessList)
                {
                    if (kvp.Key.IndexOf(TextBox_Group_Name.Text.Trim()) >= 0)
                    {
                        dt.Rows.Add();
                        dt.Rows[counter]["Group Name"] = kvp.Key;
                        counter++;
                    }
                }

                GridView_Existing_List.DataSource = dt;
                GridView_Existing_List.DataBind();

                Session[SessionFactory.ADMIN_PREF_ACCESS_MGMT_EXISTING_GROUP_DATAGRID] = dt;
            }
        }

        protected void Button_Submit_Click(object sender, EventArgs e)
        {

            Dictionary<String, String> existingGroupNamesList = (Dictionary<String, String>)Session[SessionFactory.ADMIN_PREF_ACCESS_MGMT_EXISTING_GROUP_LIST];
            if (existingGroupNamesList != null && existingGroupNamesList.ContainsKey(TextBox_Group_Name_New.Text.Trim()))
            {
                Label_Stat.Visible = true;
                Label_Stat.Text = "Group name already existis";
                Label_Stat.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                Label_Stat.Visible = false;
                ArrayList EARecordList = new ArrayList();

                //Owner Access
                String[] ownerAccessNames = { BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS };
                createSecurityContext(ownerAccessNames, null, EARecordList);


                if (!CheckBox_Owner_Access.Checked)
                {
                    //Purchase screen security
                    String[] purchaseAccessNames ={BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_PURCHASE_SCREEN_VIEW,
                                             BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_REQUIREMENT,
                                             BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_CREATE_REQUIREMENT,
                                             BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_REQUIREMENT,
                                             BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_CONVERT_REQUIREMENT,
                                             BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_RFQ,
                                             BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_CREATE_RFQ,
                                             BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_RFQ,
                                             BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_INVOICE_PURCHASE,
                                            BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_PO_PURCHASE,
                                          BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_PO_PURCHASE,
                                          BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_CREATE_PO_PURCHASE };

                    createSecurityContext(purchaseAccessNames, GridView_Purchase, EARecordList);

                    //Sales screen security

                    String[] salesAccessNames ={BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_SALES_SCREEN_VIEW,
                                          BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_LEAD,
                                          BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_CREATE_LEAD,
                                          BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_LEAD,
                                          BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_CONVERT_LEAD,
                                          BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_POTENTIAL,
                                          BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_CREATE_POTENTIAL,
                                          BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_POTENTIAL,
                                       BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_INVOICE_SALES,
                                       BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_CREATE_INVOICE_SALES,
                                       BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_INVOICE_SALES,
                                        BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_SO_SALES,
                                          BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_SO_SALES,
                                          BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_CREATE_SO_SALES};

                    createSecurityContext(salesAccessNames, Gridview_Sales, EARecordList);


                    //Products screen security

                    String[] prodAccessNames ={BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_PRODUCTS_SCREEN_VIEW,
                                         BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_CREATE_PRODUCT,
                                         BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_PRODUCT};


                    createSecurityContext(prodAccessNames, GridView_Product, EARecordList);


                    //Defects screen security

                    String[] defectAccessNames ={BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_DEFECTS_SCREEN_VIEW,
                                           BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_CREATE_INCOMING_DEFECT,
                                           BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_EDIT_INCOMING_DEFECT,
                                           BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_CREATE_OUTGOING_DEFECT,
                                           BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_EDIT_OUTGOING_DEFECT};

                    createSecurityContext(defectAccessNames, GridView_Defect, EARecordList);

                    //SR screen security
                    String[] srAccessNames ={BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_SR_SCREEN_VIEW,
                                           BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_CREATE_INCOMING_SR,
                                           BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_EDIT_INCOMING_SR,
                                           BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_CREATE_OUTGOING_SR,
                                           BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_EDIT_OUTGOING_SR};

                    createSecurityContext(srAccessNames, GridView_SR, EARecordList);

                    //Contact Screen Security

                    String[] contactAccessNames ={BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_CONTACTS_SCREEN_VIEW,
                                           BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_CREATE_CONTACT,
                                           BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_EDIT_CONTACT};

                    createSecurityContext(contactAccessNames, GridView_Contacts, EARecordList);


                    //Dashboard screen security

                    String[] dashBoardAccessNames ={BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_DASHBOARD_SCREEN_VIEW,
                                              BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_LEAD_REPORT,
                                              BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_DOWNLOAD_LEAD_REPORT,
                                              BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_POTENTIAL_REPORT,
                                              BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_DOWNLOAD_POTENTIAL_REPORT,
                                              BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_SALES_TRANSAC_REPORT,
                                              BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_DOWNLOAD_SALES_TRANSAC_REPORT,
                                              BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_PURCHASE_TRANSAC_REPORT,
                                              BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_DOWNLOAD_PURCHASE_TRANSAC_REPORT,
                                              BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_INCOMING_DEFECT_REPORT,
                                              BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_DOWNLOAD_INCOMING_DEFECT_REPORT,
                                              BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_OUTGOING_DEFECT_REPORT,
                                              BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_DOWNLOAD_OUTGOING_DEFECT_REPORT,
                                              BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_CUSTOM_REPORT,
                                              BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_DOWNLOAD_CUSTOM_REPORT};


                    createSecurityContext(dashBoardAccessNames, GridView_Dashboard, EARecordList);

                    //Admin screen security

                    String[] adminAccessNames ={BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_ADMIN_PREF_PAGE_VIEW,
                                          BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_ADMIN_PREF_ENT_MGMT_WRITE_ACCESS,
                                          //BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_ADMIN_PREF_CHAIN_MGMT_WRITE_ACCESS,
                                          BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_ADMIN_PREF_SEC_MGMT_WRITE_ACCESS,
                                          BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_ADMIN_PREF_USR_MGMT_WRITE_ACCESS,
                                          BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_ADMIN_PREF_WORKFLOW_MGMT_WRITE_ACCESS};

                    createSecurityContext(adminAccessNames, GridView_Admin, EARecordList);
                }

                try
                {
                    if (EARecordList == null || EARecordList.Count == 0)
                    {
                        Label_Stat.Visible = true;
                        Label_Stat.ForeColor = System.Drawing.Color.Red;
                        Label_Stat.Text = "Select at least one access level";
                    }
                    else
                    {
                        BackEndObjects.EntityAccessListRecord.insertEntityAccessListRecordObjectsDB(EARecordList);
                        Label_Stat.Visible = true;
                        Label_Stat.ForeColor = System.Drawing.Color.Green;
                        Label_Stat.Text = "Group Created Successfully";
                        fillAllGroupList();
                    }
                }
                catch (Exception ex)
                {
                    Label_Stat.Visible = true;
                    Label_Stat.ForeColor = System.Drawing.Color.Red;
                    Label_Stat.Text = "Group Creation Failed";
                }
            }                  
                 }

        protected ArrayList createSecurityContext(String[] contextList, GridView grvObj, ArrayList EARecordList)
        {
            //ArrayList EARecordList = new ArrayList();
            //GridViewRow gVR = grvObj.Rows[0];
            String entId = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();

            for (int i = 1; i <= contextList.Length; i++)
            {
                if (grvObj != null && ((CheckBox)grvObj.Rows[0].FindControl("CheckBox" + i)).Checked)
                {
                    BackEndObjects.EntityAccessListRecord EARecordObj = new EntityAccessListRecord();
                    EARecordObj.setEntId(entId);
                    EARecordObj.setAccessContext(contextList[i - 1]);
                    EARecordObj.setAdGrouporId(TextBox_Group_Name_New.Text);

                    EARecordList.Add(EARecordObj);
                }
                else
                {
                    if (CheckBox_Owner_Access.Checked)
                    {
                        BackEndObjects.EntityAccessListRecord EARecordObj = new EntityAccessListRecord();
                        EARecordObj.setEntId(entId);
                        EARecordObj.setAccessContext(contextList[i - 1]);
                        EARecordObj.setAdGrouporId(TextBox_Group_Name_New.Text);

                        EARecordList.Add(EARecordObj);
                    }
                }
            }

            return EARecordList;
        }


        protected void LinkButton_Show_Purchase_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView_Existing_List.SelectedIndex)
                GridView_Existing_List.SelectRow(row.RowIndex);

            String forwardString = "/Pages/Popups/AdminPref/ExistingGroupAccess.aspx";
            String groupName = ((Label)GridView_Existing_List.Rows[Convert.ToInt32(e.CommandArgument)].Cells[0].FindControl("Label_Name")).Text;

            forwardString += "?groupName=" + groupName;

            Response.Redirect(forwardString);
            //Server.Transfer(forwardString);
        }

        protected void LinkButton_Show_Users_Command(object sender, CommandEventArgs e)
        {

        }

        protected void CheckBox_Owner_Access_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox_Owner_Access.Checked)
            {
                GridView_Admin.Enabled = false;
                GridView_Contacts.Enabled = false;
                GridView_Dashboard.Enabled = false;
                GridView_Defect.Enabled = false;
                GridView_SR.Enabled = false;
                GridView_Product.Enabled = false;
                GridView_Purchase.Enabled = false;
                Gridview_Sales.Enabled = false;
            }
            else
            {
             
                GridView_Admin.Enabled = true;
                GridView_Contacts.Enabled = true;
                GridView_Dashboard.Enabled = true;
                GridView_Defect.Enabled = true;
                GridView_SR.Enabled = true;
                GridView_Product.Enabled = true;
                GridView_Purchase.Enabled = true;
                Gridview_Sales.Enabled = true;
            
            }
        }

        protected void GridView_Existing_List_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_Existing_List.PageIndex = e.NewPageIndex;
            GridView_Existing_List.DataSource = (DataTable)Session[SessionFactory.ADMIN_PREF_ACCESS_MGMT_EXISTING_GROUP_DATAGRID];
            GridView_Existing_List.DataBind();
            GridView_Existing_List.SelectedIndex = -1;
        }

    }
}