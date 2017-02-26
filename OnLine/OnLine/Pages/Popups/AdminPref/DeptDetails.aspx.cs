using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BackEndObjects;
using ActionLibrary;
using System.Data;

namespace OnLine.Pages.Popups.AdminPref
{
    public partial class DeptDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                fillDeptGrid();
                loadUserList();
            }
        }

        protected void fillDeptGrid()
        {
            Dictionary<String, BackEndObjects.DeptDetails> deptDict = BackEndObjects.DeptDetails.
                getAllDeptDetailsForEntIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            
            DataTable dt = new DataTable();
            dt.Columns.Add("dept_id");
            dt.Columns.Add("name");
            dt.Columns.Add("desc");
            dt.Columns.Add("head");

            int count = 0;
            foreach (KeyValuePair<String, BackEndObjects.DeptDetails> kvp in deptDict)
            {
                dt.Rows.Add();
                dt.Rows[count]["dept_id"] = kvp.Value.getDeptId();
                dt.Rows[count]["name"] = kvp.Value.getDeptName();
                dt.Rows[count]["desc"] = kvp.Value.getDeptDescription();
                dt.Rows[count]["head"] = kvp.Value.getDeptHeadUsrId();

                count++;
            }

            if (count > 0)
            {
                GridView_Dept.DataSource = dt;
                GridView_Dept.DataBind();
                GridView_Dept.Visible = true;
                Session[SessionFactory.ADMIN_PREF_DEPT_MGMT_DEPT_GRID] = dt;
            }
        }

        protected void loadUserList()
        {
            Dictionary<String, userDetails> allUserDetails = BackEndObjects.MainBusinessEntity.getUserDetailsforMainEntitybyIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());


            foreach (KeyValuePair<String, userDetails> kvp in allUserDetails)
            {
                BackEndObjects.userDetails userDetObj = kvp.Value;

                ListItem lt1 = new ListItem();
                lt1.Text = userDetObj.getUserId();
                lt1.Value = userDetObj.getUserId();

                DropDownList_Dept_Head.Items.Add(lt1);
                DropDownList_Users_Add_Members.Items.Add(lt1);
            }

            ListItem emptyItem = new ListItem();
            emptyItem.Text = "";
            emptyItem.Value = "";

            DropDownList_Dept_Head.Items.Add(emptyItem);
            DropDownList_Dept_Head.SelectedValue = "";

            DropDownList_Users_Add_Members.Items.Add(emptyItem);
            DropDownList_Users_Add_Members.SelectedValue = "";
        }

        protected void GridView_Dept_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView_Dept.EditIndex = e.NewEditIndex;
            GridView_Dept.DataSource = (DataTable)Session[SessionFactory.ADMIN_PREF_DEPT_MGMT_DEPT_GRID];
            GridView_Dept.DataBind();
        }

        protected void GridView_Dept_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            
        }

        protected void GridView_Dept_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            String deptName = ((TextBox)GridView_Dept.Rows[e.RowIndex].Cells[0].FindControl("TextBox_DeptName_Edit")).Text;
            String descr = ((TextBox)GridView_Dept.Rows[e.RowIndex].Cells[0].FindControl("TextBox_Descr_Edit")).Text;
            String deptHead = ((DropDownList)GridView_Dept.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Dept_Head_Edit")).SelectedValue;
            String deptId = ((Label)GridView_Dept.Rows[e.RowIndex].Cells[0].FindControl("Label_Hidden")).Text;

            int index = GridView_Dept.Rows[e.RowIndex].DataItemIndex;
            DataTable dt = (DataTable)Session[SessionFactory.ADMIN_PREF_DEPT_MGMT_DEPT_GRID];

                dt.Rows[index]["name"] = deptName;
                dt.Rows[index]["desc"] = descr;
                dt.Rows[index]["head"] = deptHead;

                Session[SessionFactory.ADMIN_PREF_DEPT_MGMT_DEPT_GRID] = dt;

                try
                {
                    Dictionary<String, String> whereCls = new Dictionary<string, string>();
                    Dictionary<String, String> targetVal = new Dictionary<string, string>();

                    whereCls.Add(BackEndObjects.DeptDetails.DEPT_DETAILS_COL_ENT_ID, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                    whereCls.Add(BackEndObjects.DeptDetails.DEPT_DETAILS_COL_DEPT_ID, deptId);

                    targetVal.Add(BackEndObjects.DeptDetails.DEPT_DETAILS_COL_DEPT_DESC, descr);
                    targetVal.Add(BackEndObjects.DeptDetails.DEPT_DETAILS_COL_DEPT_HEAD_USR_ID, deptHead);
                    targetVal.Add(BackEndObjects.DeptDetails.DEPT_DETAILS_COL_DEPT_NAME, deptName);

                    BackEndObjects.DeptDetails.updateDeptDetailsDB(targetVal, whereCls, DBConn.Connections.OPERATION_UPDATE);

                    GridView_Dept.EditIndex = -1;
                    GridView_Dept.DataSource = dt;
                    GridView_Dept.DataBind();
                    
                }
                catch (Exception ex)
                {
                }
            
        }

        protected void GridView_Dept_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView_Dept.EditIndex = -1;
            GridView_Dept.DataSource = (DataTable)Session[SessionFactory.ADMIN_PREF_DEPT_MGMT_DEPT_GRID];
            GridView_Dept.DataBind();
        }

        protected void Button_Dept_Add_Click(object sender, EventArgs e)
        {
            BackEndObjects.DeptDetails deptObj = new BackEndObjects.DeptDetails();
            deptObj.setDeptId(new Id().getNewId(Id.ID_TYPE_DEPT_ID_STRING));
            deptObj.setDeptName(TextBox_Dept_Name.Text);
            deptObj.setEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            deptObj.setDeptHeadUsrId(DropDownList_Dept_Head.SelectedValue);
            deptObj.setDeptDescription(TextBox_Desc.Text);

            try
            {
                BackEndObjects.DeptDetails.insertDeptDetailsDB(deptObj);

                Dictionary<String, String> whereCls = new Dictionary<string, string>();
                Dictionary<String, String> targetVals = new Dictionary<string, string>();
                whereCls.Add(BackEndObjects.userDetails.USER_DETAILS_COL_BUSINESS_ID, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                whereCls.Add(BackEndObjects.userDetails.USER_DETAILS_COL_USERID, DropDownList_Dept_Head.SelectedValue);
                targetVals.Add(BackEndObjects.userDetails.USER_DETAILS_COL_DEPT_ID, deptObj.getDeptId());
                BackEndObjects.userDetails.updateUserDetailsDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);

                Label_Dept_Add_Stat.Visible = true;
                Label_Dept_Add_Stat.Text = "Department Details Created Successfully. You can now add members to this department";
                Label_Dept_Add_Stat.ForeColor = System.Drawing.Color.Green;
                fillDeptGrid();
            }
            catch (Exception  ex)
            {
                Label_Dept_Add_Stat.Visible = true;
                Label_Dept_Add_Stat.Text = "Department Details Creation Failed.";
                Label_Dept_Add_Stat.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void GridView_Dept_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                GridViewRow gVR = e.Row;

                //BackEndObjects.MainBusinessEntity.getUserDetailsforMainEntitybyIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                Dictionary<String, userDetails> allUserDetails = (Dictionary<String, userDetails>)Cache[Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()];
                if (allUserDetails == null)
                {
                    allUserDetails = BackEndObjects.MainBusinessEntity.getUserDetailsforMainEntitybyIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                    Cache.Insert(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), allUserDetails);
                }

                DropDownList DropDown_Users_List = (DropDownList)gVR.Cells[0].FindControl("DropDownList_Dept_Head_Edit");


                foreach (KeyValuePair<String, userDetails> kvp in allUserDetails)
                {
                    BackEndObjects.userDetails userDetObj = kvp.Value;

                    ListItem lt = new ListItem();
                    lt.Text = userDetObj.getUserId();
                    lt.Value = userDetObj.getUserId();

                    DropDown_Users_List.Items.Add(lt);
                }

                if (!((Label)gVR.Cells[0].FindControl("Label_Head_Edit")).Text.Equals(""))
                DropDown_Users_List.SelectedValue = ((Label)gVR.Cells[0].FindControl("Label_Head_Edit")).Text;


            }
        }

        protected void Button_Dept_Filter_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session[SessionFactory.ADMIN_PREF_DEPT_MGMT_DEPT_GRID];

            if (!TextBox_DeptName_Search.Text.Trim().Equals(""))
            {
                DataTable dtTemp = new DataTable();

                dtTemp.Columns.Add("dept_id");
                dtTemp.Columns.Add("name");
                dtTemp.Columns.Add("desc");
                dtTemp.Columns.Add("head");
                int count = 0;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["name"].ToString().IndexOf(TextBox_DeptName_Search.Text.Trim(), StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        dtTemp.Rows.Add();
                        dtTemp.Rows[count]["dept_id"] = dt.Rows[i]["dept_id"];
                        dtTemp.Rows[count]["name"] = dt.Rows[i]["name"];
                        dtTemp.Rows[count]["desc"] = dt.Rows[i]["desc"];
                        dtTemp.Rows[count]["head"] = dt.Rows[i]["head"];

                        count++;
                    }
                }

                GridView_Dept.DataSource = dtTemp;
                GridView_Dept.DataBind();
            }
            else
            {
                GridView_Dept.DataSource = dt;
                GridView_Dept.DataBind();
            }
        }

        protected void Button_Hide_Click(object sender, EventArgs e)
        {
            Panel_Add_Dept_Members.Visible = false;
        }

        protected void loadMembersGrid(Dictionary<String, userDetails> userDict)
        {
            if (userDict == null || userDict.Count > 0)
            {
                userDict = BackEndObjects.DeptDetails.
                    getAllUsersForDeptIdAndEntIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(),hdnValueDeptId.Value);
            }
                DataTable dt = new DataTable();
                dt.Columns.Add("userId");
                dt.Columns.Add("Name");
                Dictionary<String, String> existingUserForDept = new Dictionary<string, string>();

                int counter = 0;
                foreach (KeyValuePair<String, userDetails> kvp in userDict)
                {
                    existingUserForDept.Add(kvp.Key.ToString(), kvp.Key.ToString());
                    dt.Rows.Add();
                    dt.Rows[counter]["userId"] = kvp.Key.Trim();
                    dt.Rows[counter]["Name"] = kvp.Value.getName();
                    counter++;
                }
                GridView_Dept_Members.Visible = true;
                GridView_Dept_Members.SelectedIndex = -1;
                GridView_Dept_Members.DataSource = dt;
                GridView_Dept_Members.DataBind();

                Cache.Remove("DeptMembersList");
                Cache.Remove("DeptMembersDataTable");
                Cache["DeptMembersList"] = existingUserForDept;
                Cache["DeptMembersDataTable"] = dt;
        }


        protected void Show_Members_Command(object sender, CommandEventArgs e)
        {
            Panel_Add_Dept_Members.Visible = true;
            Label_Add_To_Dept_Status.Visible = false;
            DropDownList_Users_Add_Members.SelectedValue = "";

            String deptId = ((Label)GridView_Dept.Rows[int.Parse(e.CommandArgument.ToString())].Cells[0].FindControl("Label_Hidden")).Text;
            hdnValueDeptId.Value = deptId;

            Dictionary<String,userDetails> userDict=BackEndObjects.DeptDetails.getAllUsersForDeptIdAndEntIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), deptId);
            if (userDict != null && userDict.Count > 0)
                loadMembersGrid(userDict);
            else
            {
                try
                {
                    Cache.Remove("DeptMembersList");
                    Cache.Remove("DeptMembersDataTable");
                    GridView_Dept_Members.Visible = false;
                }
                catch (Exception ex)
                {
                }
            }
            //String deptId=
        }

        protected void GridView_Dept_Members_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_Dept_Members.PageIndex = e.NewPageIndex;
            DataTable dt=(DataTable) Cache["DeptMembersDataTable"];
            GridView_Dept_Members.DataSource = dt;
            GridView_Dept_Members.DataBind();
            //Dictionary<String, userDetails> userDict = BackEndObjects.DeptDetails.
                //getAllUsersForDeptIdAndEntIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), hdnValueDeptId.Value);
            //if (userDict != null && userDict.Count > 0)
                //loadMembersGrid(userDict);           

        }

        protected void GridView_Dept_Members_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow gVR = GridView_Dept_Members.Rows[e.RowIndex];
            String userId = ((Label)gVR.Cells[0].FindControl("Label_UserId")).Text;

            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            Dictionary<String, String> targetVals = new Dictionary<string, string>();

            whereCls.Add(BackEndObjects.userDetails.USER_DETAILS_COL_USERID, userId);
            whereCls.Add(BackEndObjects.userDetails.USER_DETAILS_COL_BUSINESS_ID, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            targetVals.Add(BackEndObjects.userDetails.USER_DETAILS_COL_DEPT_ID, "");
            BackEndObjects.userDetails.updateUserDetailsDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);

            DataTable dt=(DataTable) Cache["DeptMembersDataTable"];
            int index = GridView_Dept_Members.Rows[e.RowIndex].DataItemIndex;
            dt.Rows[index].Delete();

            GridView_Dept_Members.DataSource = dt;
            GridView_Dept_Members.DataBind();
        }

        protected void Button_Add_To_Dept_Click(object sender, EventArgs e)
        {
            Dictionary<String,String> existingIdListForThisDept=(Dictionary<String,String>)Cache["DeptMembersList"];
            existingIdListForThisDept = (existingIdListForThisDept == null ? new Dictionary<String, String>() : existingIdListForThisDept);
            
            BackEndObjects.userDetails userObj = userDetails.getUserDetailsbyIdDB(DropDownList_Users_Add_Members.SelectedValue, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            if (userObj.getDeptId() != null && !userObj.getDeptId().Equals(""))
            {
                String deptName = BackEndObjects.DeptDetails.
                    getDeptDetailsForEntIdAndDeptIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), userObj.getDeptId()).getDeptName();

                Label_Add_To_Dept_Status.Visible = true;
                Label_Add_To_Dept_Status.Text = "The user already part of " + deptName;
                Label_Add_To_Dept_Status.ForeColor = System.Drawing.Color.Red;
            }
            else if (existingIdListForThisDept.ContainsKey(DropDownList_Users_Add_Members.SelectedValue))
            {
                Label_Add_To_Dept_Status.Visible = true;
                Label_Add_To_Dept_Status.Text = "User already in the list below";
                Label_Add_To_Dept_Status.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                Dictionary<String, String> whereCls = new Dictionary<string, string>();
                Dictionary<String, String> targetVals = new Dictionary<string, string>();
                whereCls.Add(BackEndObjects.userDetails.USER_DETAILS_COL_BUSINESS_ID, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                whereCls.Add(BackEndObjects.userDetails.USER_DETAILS_COL_USERID, userObj.getUserId());

                targetVals.Add(BackEndObjects.userDetails.USER_DETAILS_COL_DEPT_ID, hdnValueDeptId.Value);

                BackEndObjects.userDetails.updateUserDetailsDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
                loadMembersGrid(null);
                Label_Add_To_Dept_Status.Visible = false;
            }
            

        }
    }
}