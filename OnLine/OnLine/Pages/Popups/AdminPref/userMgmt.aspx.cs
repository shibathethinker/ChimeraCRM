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
using System.Security.Cryptography;


namespace OnLine.Pages.Popups.AdminPref
{
    public partial class userMgmt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                fillBasicUserDetailGrid(null);
            }
        }

        protected void fillBasicUserDetailGrid(String userFilterText)
        {
            Dictionary<String,userDetails> userDetDict=BackEndObjects.MainBusinessEntity.
                getUserDetailsforMainEntitybyIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

            DataTable dt = new DataTable();
            dt.Columns.Add("UserName");
            dt.Columns.Add("UserId");
            dt.Columns.Add("Password");
            dt.Columns.Add("EmailId");
            dt.Columns.Add("ContactNo");
            dt.Columns.Add("AccessDet");
            dt.Columns.Add("reportsTo");

            int counter = 0;

            foreach (KeyValuePair<String, userDetails> kvp in userDetDict)
            {
                
                BackEndObjects.userDetails userObj = kvp.Value;

                bool considerRecord=(userFilterText!=null && !userFilterText.Equals(""))? (userObj.getUserId().IndexOf(userFilterText.Trim(),StringComparison.InvariantCultureIgnoreCase) >= 0?true:false):true;
                
                if (considerRecord)
                {
                    dt.Rows.Add();

                    dt.Rows[counter]["UserName"] = userObj.getName();
                    dt.Rows[counter]["UserId"] = userObj.getUserId();
                    dt.Rows[counter]["Password"] = "";
                    dt.Rows[counter]["EmailId"] = userObj.getEmailId();
                    dt.Rows[counter]["ContactNo"] = userObj.getContactNo();
                    dt.Rows[counter]["AccessDet"] = userObj.getPrivilege();
                    dt.Rows[counter]["reportsTo"] = userObj.getReportsTo();
                    counter++;
                }
            }

            GridView_User_List.DataSource = dt;
            GridView_User_List.DataBind();
            GridView_User_List.SelectedIndex = -1;
            Session[SessionFactory.ADMIN_PREF_USER_MGMT_BASIC_USER_DET_GRID] = dt;
        }


        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void GridView_User_List_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView_User_List.EditIndex = -1;
            GridView_User_List.DataSource = (DataTable)Session[SessionFactory.ADMIN_PREF_USER_MGMT_BASIC_USER_DET_GRID];
            GridView_User_List.DataBind();
        }

        protected void GridView_User_List_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView_User_List.EditIndex = e.NewEditIndex;
            GridView_User_List.DataSource = (DataTable)Session[SessionFactory.ADMIN_PREF_USER_MGMT_BASIC_USER_DET_GRID];
            GridView_User_List.DataBind();
        }

        protected void GridView_User_List_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow gVR = GridView_User_List.Rows[e.RowIndex];
            int index = GridView_User_List.Rows[e.RowIndex].DataItemIndex;

            String userId=((Label)gVR.Cells[0].FindControl("Label_User_Id")).Text;

            String userName = ((TextBox)gVR.Cells[0].FindControl("TextBox_Name_Edit")).Text;

            BackEndObjects.userDetails userObj = BackEndObjects.userDetails.getUserDetailsbyIdDB(userId);
            //Combine salt and generate the password
            byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(((TextBox)gVR.Cells[0].FindControl("TextBox_Password_Edit")).Text + userObj.getSalt());
            HashAlgorithm hashConverter = new SHA256Managed();
            byte[] hashedByteStream = hashConverter.ComputeHash(plainTextBytes);
            String encryptedAndConvertedPassword = Convert.ToBase64String(hashedByteStream);

            String passWord = encryptedAndConvertedPassword;
            String emailId = ((TextBox)gVR.Cells[0].FindControl("TextBox_Email_Id_Edit")).Text;
            String contactNo = ((TextBox)gVR.Cells[0].FindControl("TextBox_Contact_No_Edit")).Text;
            String reportsTo = ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Reports_To")).SelectedValue;

            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            Dictionary<String, String> targetCls = new Dictionary<string, string>();

            whereCls.Add(BackEndObjects.userDetails.USER_DETAILS_COL_BUSINESS_ID, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            whereCls.Add(BackEndObjects.userDetails.USER_DETAILS_COL_USERID, userId);

            targetCls.Add(BackEndObjects.userDetails.USER_DETAILS_COL_PASSWORD, passWord);
            targetCls.Add(BackEndObjects.userDetails.USER_DETAILS_COL_NAME, userName);
            targetCls.Add(BackEndObjects.userDetails.USER_DETAILS_COL_EMAIL_ID, emailId);
            targetCls.Add(BackEndObjects.userDetails.USER_DETAILS_COL_CONTACT_NO, contactNo);
            targetCls.Add(BackEndObjects.userDetails.USER_DETAILS_COL_REPORTS_TO, reportsTo);

            try
            {
                BackEndObjects.userDetails.updateUserDetailsDB(targetCls, whereCls, DBConn.Connections.OPERATION_UPDATE);
                DataTable dt =(DataTable) Session[SessionFactory.ADMIN_PREF_USER_MGMT_BASIC_USER_DET_GRID];

                dt.Rows[index]["UserName"] = userName;
                dt.Rows[index]["Password"] = ((TextBox)gVR.Cells[0].FindControl("TextBox_Password_Edit")).Text;
                dt.Rows[index]["EmailId"] = emailId;
                dt.Rows[index]["ContactNo"] = contactNo;
                dt.Rows[index]["reportsTo"] = reportsTo;

                GridView_User_List.EditIndex = -1;
                GridView_User_List.DataSource = dt;
                GridView_User_List.DataBind();                
                Session[SessionFactory.ADMIN_PREF_USER_MGMT_BASIC_USER_DET_GRID] = dt;
            }
            catch (Exception ex)
            {
            }
        }

        protected void Button_Filter_Click(object sender, EventArgs e)
        {
            fillBasicUserDetailGrid(TextBox_User_Id.Text.Trim());
        }

        protected void LinkButton_Show_Access_Command(object sender, CommandEventArgs e)
        {
            String forwardString = "/Pages/Popups/AdminPref/ExistingGroupAccess.aspx";
            String groupName = ((Label)GridView_User_Access.Rows[Convert.ToInt32(e.CommandArgument)].Cells[0].FindControl("Label_Access_Name")).Text;

            forwardString += "?groupName=" + groupName;

            ScriptManager.
    RegisterStartupScript(this, typeof(string), "showGroupDet", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=700');", true);

        }

        protected void GridView_User_Access_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_User_Access.PageIndex = e.NewPageIndex;

             String accessString = ((Label)GridView_User_List.SelectedRow.Cells[0].FindControl("Label_Access")).Text;
             if (accessString != null && accessString.Length > 0)
             {
                 String[] accessList = accessString.Split(new char[] { ',' });

                 fillUserAccessDetailGrid(accessList);
             }

        }



        protected void fillUserAccessDetailGrid(String[] accessList)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("AccessName");

            for (int i = 0; i < accessList.Length; i++)
            {
                dt.Rows.Add();
                dt.Rows[i]["AccessName"] = accessList[i];
            }

            GridView_User_Access.Visible = true;
            GridView_User_Access.DataSource = dt;
            GridView_User_Access.DataBind();
        }

        protected void Button_Show_Access_Click(object sender, EventArgs e)
        {            
            String accessString = BackEndObjects.userDetails.
   getUserDetailsbyIdDB(((Label)GridView_User_List.SelectedRow.Cells[0].FindControl("Label_User_Id")).Text,
   Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()).getPrivilege();

            if (accessString != null && accessString.Length > 0)
            {
                Label_Access_For_User.Text = "Showing Access Details for the selected user";
                Label_Access_For_User.Visible = true;

                String[] accessList = accessString.Split(new char[] { ',' });

                fillUserAccessDetailGrid(accessList);
            }
            else
                GridView_User_Access.Visible = false;
        }

        protected void GridView_User_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            Label_Access_For_User.Visible = false;
            Button_Add_Access.Enabled = true;
            Button_Show_Access.Enabled = true;
            Button_All_Reporting_User.Enabled = true;
            Button_All_Direct_Reporting_User.Enabled = true;
        }

        protected void GridView_User_List_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Button_All_Reporting_User.Enabled = false;
            Button_All_Direct_Reporting_User.Enabled = false;
            Button_Show_Access.Enabled = false;

            GridView_User_List.PageIndex = e.NewPageIndex;
            GridView_User_List.SelectedIndex = -1;
            GridView_User_List.DataSource = (DataTable)Session[SessionFactory.ADMIN_PREF_USER_MGMT_BASIC_USER_DET_GRID];
            GridView_User_List.DataBind();
        }

        protected void Button_Add_Access_Click(object sender, EventArgs e)
        {
            fillAllGroupList();
        }

        protected void fillAllGroupList()
        {
            String entId = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();
            Dictionary<String, ArrayList> accessList = BackEndObjects.EntityAccessListRecord.getCompleteAccessListbyEntId(entId);

            DataTable dt = new DataTable();
            dt.Columns.Add("Group Name");
            dt.Columns.Add("2");

            int counter = 0;

            foreach (KeyValuePair<String, ArrayList> kvp in accessList)
            {
                dt.Rows.Add();
                dt.Rows[counter]["Group Name"] = kvp.Key;
                counter++;
            }

            GridView_All_AccessGroups.DataSource = dt;
            GridView_All_AccessGroups.DataBind();
            Label_All_Groups.Visible = true;
            Label_All_Groups.Text = "All Available Groups... To create more groups use the link 'Access and Security Groups Management' in admin pref ";
        }

        protected void LinkButton_Show_Access_For_Group_Command(object sender, CommandEventArgs e)
        {

            String forwardString = "/Pages/Popups/AdminPref/ExistingGroupAccess.aspx";
            String groupName = ((Label)GridView_All_AccessGroups.Rows[Convert.ToInt32(e.CommandArgument)].
                Cells[0].FindControl("Label_Group_Name")).Text;

            forwardString += "?groupName=" + groupName;

            ScriptManager.
                RegisterStartupScript(this, typeof(string), "showGroupDet", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=700');", true);
        }

        //Update the access level of the selected user with the new group
        protected void GridView_All_AccessGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            String accessString = BackEndObjects.userDetails.
                getUserDetailsbyIdDB(((Label)GridView_User_List.SelectedRow.Cells[0].FindControl("Label_User_Id")).Text, 
                Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()).getPrivilege();

            String newGroup = ((Label)GridView_All_AccessGroups.Rows[GridView_All_AccessGroups.SelectedIndex].
                Cells[0].FindControl("Label_Group_Name")).Text;
            bool groupAlreadyAdded = false;

                if (newGroup != null & !newGroup.Equals("")  )
                {
                    if (accessString != null && accessString.Length > 0)
                    {
                        if (accessString.IndexOf(newGroup) < 0)
                            accessString += "," + newGroup;
                        else//The group is already added to the users security list
                            groupAlreadyAdded = true;
                    }
                    else
                        accessString = newGroup;
                }
                if (accessString != null && accessString.Length > 0 && !groupAlreadyAdded)
                {
                    String[] accessList = accessString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    fillUserAccessDetailGrid(accessList);

                    accessString = "";
                    for (int i = 0; i < accessList.Length; i++)
                        accessString += accessList[i] + ",";

                    accessString = accessString.Remove(accessString.Length - 1);

                    Dictionary<String, String> whereCls = new Dictionary<string, string>();
                    Dictionary<String, String> targetCls = new Dictionary<string, string>();


                    whereCls.Add(BackEndObjects.userDetails.USER_DETAILS_COL_BUSINESS_ID, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                    whereCls.Add(BackEndObjects.userDetails.USER_DETAILS_COL_USERID, ((Label)GridView_User_List.SelectedRow.Cells[0].FindControl("Label_User_Id")).Text);

                    targetCls.Add(BackEndObjects.userDetails.USER_DETAILS_COL_PRIVILEGE, accessString);

                    try
                    {
                        BackEndObjects.userDetails.updateUserDetailsDB(targetCls, whereCls, DBConn.Connections.OPERATION_UPDATE);
                    }
                    catch (Exception ex)
                    {
                    }
                }
        }

        protected void GridView_User_Access_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            String accessString = BackEndObjects.userDetails.
    getUserDetailsbyIdDB(((Label)GridView_User_List.SelectedRow.Cells[0].FindControl("Label_User_Id")).Text,
    Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()).getPrivilege();

            String newGroup = ((Label)GridView_User_Access.Rows[e.RowIndex].
    Cells[0].FindControl("Label_Access_Name")).Text;

            accessString=accessString.Remove(accessString.IndexOf(newGroup), newGroup.Length);
            if (accessString != null)
            {
                String[] accessList = accessString.Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries);
                fillUserAccessDetailGrid(accessList);

                accessString = "";
                for (int i = 0; i < accessList.Length; i++)
                   accessString += accessList[i]+",";

                if(accessString.Length>0)
                accessString=accessString.Remove(accessString.Length - 1);

                Dictionary<String, String> whereCls = new Dictionary<string, string>();
                Dictionary<String, String> targetCls = new Dictionary<string, string>();


                whereCls.Add(BackEndObjects.userDetails.USER_DETAILS_COL_BUSINESS_ID, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                whereCls.Add(BackEndObjects.userDetails.USER_DETAILS_COL_USERID, ((Label)GridView_User_List.SelectedRow.Cells[0].FindControl("Label_User_Id")).Text);

                targetCls.Add(BackEndObjects.userDetails.USER_DETAILS_COL_PRIVILEGE, accessString);

                try
                {
                    BackEndObjects.userDetails.updateUserDetailsDB(targetCls, whereCls, DBConn.Connections.OPERATION_UPDATE);
                }
                catch (Exception ex)
                {
                }
            }
        }

        protected void GridView_User_List_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                GridViewRow gVR = e.Row;

                //Label_User_Id
                //BackEndObjects.MainBusinessEntity.getUserDetailsforMainEntitybyIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                Dictionary<String, userDetails> allUserDetails = (Dictionary<String, userDetails>)Cache[Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()];
                if (allUserDetails == null)
                {
                    allUserDetails = BackEndObjects.MainBusinessEntity.getUserDetailsforMainEntitybyIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                    Cache.Insert(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), allUserDetails);
                }

                Dictionary<String, userDetails> allReportingUsers = (Dictionary<String, userDetails>)BackEndObjects.
                    userDetails.
                    getAllReportingUserDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), 
                    ((Label)gVR.Cells[0].FindControl("Label_User_Id")).Text);


                DropDownList DropDown_Reports_List = (DropDownList)gVR.Cells[0].FindControl("DropDownList_Reports_To");


                foreach (KeyValuePair<String, userDetails> kvp in allUserDetails)
                {
                    if (!kvp.Key.Equals(((Label)gVR.Cells[0].FindControl("Label_User_Id")).Text) &&
                        !allReportingUsers.ContainsKey(kvp.Key))
                    {
                        ListItem lt = new ListItem();
                        lt.Text = kvp.Key;
                        lt.Value = kvp.Key;

                        DropDown_Reports_List.Items.Add(lt);
                    }                    
                }

                ListItem empty = new ListItem();
                empty.Text = "";
                empty.Value = "";
                DropDown_Reports_List.Items.Add(empty);

                if (!((Label)gVR.Cells[0].FindControl("Label_Reports_To_Edit")).Text.Equals(""))
                    DropDown_Reports_List.SelectedValue = ((Label)gVR.Cells[0].FindControl("Label_Reports_To_Edit")).Text;


            }
        }

        protected void Button_Hide_Click(object sender, EventArgs e)
        {
            Panel_Reporting_Users.Visible = false;
        }

        protected void Button_All_Reporting_User_Click(object sender, EventArgs e)
        {
            Dictionary<String, userDetails> allReportingUsers = (Dictionary<String, userDetails>)BackEndObjects.
    userDetails.
    getAllReportingUserDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(),
    ((Label)GridView_User_List.SelectedRow.Cells[0].FindControl("Label_User_Id")).Text);

            if (allReportingUsers != null && allReportingUsers.Count > 0)
            {
                Label_Reporting_Users.Visible = false;
                DataTable dt = new DataTable();
                dt.Columns.Add("userId");
                dt.Columns.Add("Name");

                int i = 0;
                foreach (KeyValuePair<String, userDetails> kvp in allReportingUsers)
                {
                    dt.Rows.Add();
                    dt.Rows[i]["userId"] = kvp.Key;
                    dt.Rows[i]["Name"] = kvp.Value.getName();
                    i++;
                }
                Cache.Remove("userMgmtAllReportingUsers");
                Cache["userMgmtAllReportingUsers"] = dt;
                GridView_Reporting_Users.Columns[0].Visible = false;
                GridView_Reporting_Users.DataSource = dt;
                GridView_Reporting_Users.DataBind();
                GridView_Reporting_Users.Visible = true;
            }
            else
            {                
                GridView_Reporting_Users.Visible = false;
                Label_Reporting_Users.Visible = true;
                Label_Reporting_Users.Text = "No user reporting to this user";
                Label_Reporting_Users.ForeColor = System.Drawing.Color.Red;
            }
            Panel_Reporting_Users.Visible = true;
        }

        protected void GridView_Reporting_Users_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_Reporting_Users.PageIndex = e.NewPageIndex;
            GridView_Reporting_Users.DataSource=(DataTable)Cache["userMgmtAllReportingUsers"];
            GridView_Reporting_Users.DataBind();
        }

        protected void Button_All_Direct_Reporting_User_Click(object sender, EventArgs e)
        {
            Dictionary<String, userDetails> allReportingUsers = (Dictionary<String, userDetails>)BackEndObjects.
userDetails.getAllDirectReportingUserDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(),
((Label)GridView_User_List.SelectedRow.Cells[0].FindControl("Label_User_Id")).Text);

            if (allReportingUsers != null && allReportingUsers.Count > 0)
            {
                Label_Reporting_Users.Visible = false;
                DataTable dt = new DataTable();
                dt.Columns.Add("userId");
                dt.Columns.Add("Name");

                int i = 0;
                foreach (KeyValuePair<String, userDetails> kvp in allReportingUsers)
                {
                    dt.Rows.Add();
                    dt.Rows[i]["userId"] = kvp.Key;
                    dt.Rows[i]["Name"] = kvp.Value.getName();
                    i++;
                }
                Cache.Remove("userMgmtAllDirectReportingUsers");
                Cache["userMgmtAllDirectReportingUsers"] = dt;
                GridView_Reporting_Users.Columns[0].Visible = true;
                GridView_Reporting_Users.DataSource = dt;
                GridView_Reporting_Users.DataBind();
                GridView_Reporting_Users.Visible = true;
            }
            else
            {                
                GridView_Reporting_Users.Visible = false;
                Label_Reporting_Users.Visible = true;
                Label_Reporting_Users.Text = "No user reporting to this user";
                Label_Reporting_Users.ForeColor = System.Drawing.Color.Red;
            }
            Panel_Reporting_Users.Visible = true;
        }

        protected void GridView_Reporting_Users_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow gVR = GridView_Reporting_Users.Rows[e.RowIndex];
            String userId = ((Label)gVR.Cells[0].FindControl("Label_UserId")).Text;

            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            Dictionary<String, String> targetVals = new Dictionary<string, string>();

            whereCls.Add(BackEndObjects.userDetails.USER_DETAILS_COL_USERID, userId);
            whereCls.Add(BackEndObjects.userDetails.USER_DETAILS_COL_BUSINESS_ID, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            targetVals.Add(BackEndObjects.userDetails.USER_DETAILS_COL_REPORTS_TO, "");
            BackEndObjects.userDetails.updateUserDetailsDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);

            DataTable dt = (DataTable)Cache["userMgmtAllDirectReportingUsers"];
            int index = GridView_Reporting_Users.Rows[e.RowIndex].DataItemIndex;
            dt.Rows[index].Delete();

            DataTable allUsers = (DataTable)Session[SessionFactory.ADMIN_PREF_USER_MGMT_BASIC_USER_DET_GRID];
            for (int i = 0; i < allUsers.Rows.Count; i++)
            {
                if (allUsers.Rows[i]["UserId"].ToString().Equals(userId))
                {
                    allUsers.Rows[i]["reportsTo"] = ""; break;
                }
            }

            Cache["userMgmtAllDirectReportingUsers"] = dt;
            GridView_Reporting_Users.DataSource = dt;
            GridView_Reporting_Users.DataBind();

            GridView_User_List.DataSource = allUsers;
            GridView_User_List.DataBind();
            Session[SessionFactory.ADMIN_PREF_USER_MGMT_BASIC_USER_DET_GRID] = allUsers;
        }

        protected void Button_Show_Hierarchy_Click(object sender, EventArgs e)
        {
            Dictionary<String, userDetails> allUserDetails = (Dictionary<String, userDetails>)
                Cache[Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()];
            Dictionary<String, TreeNode> nodeDict = new Dictionary<string, TreeNode>();

            //nodeDict and allUserDetails have the same keyset = the userIds

            foreach (KeyValuePair<String, userDetails> kvp in allUserDetails)
            {
                TreeNode thisNode = new TreeNode();
                thisNode.Text = kvp.Key + "(" + kvp.Value.getName() + ")";
                thisNode.Value = kvp.Key + "(" + kvp.Value.getName() + ")";
                nodeDict.Add(kvp.Key, thisNode);
            }

            foreach (KeyValuePair<String, userDetails> kvp in allUserDetails)
            {
                if (kvp.Value.getReportsTo() != null && !kvp.Value.getReportsTo().Equals(""))
                {
                    TreeNode parentOfThisNode = nodeDict[kvp.Value.getReportsTo()];
                    parentOfThisNode.ChildNodes.Add(nodeDict[kvp.Key]);
                    nodeDict[kvp.Value.getReportsTo()] = parentOfThisNode;
                }
            }
            TreeNode n1 = new TreeNode();
            TreeView1.Nodes.Clear();
            //TreeNodeCollection treeNodeColl = new TreeNodeCollection();
           List<TreeNode> treeNodeColl = new List<TreeNode>();
            
            
            foreach (KeyValuePair<String, TreeNode> kvp in nodeDict)
                treeNodeColl.Add(kvp.Value);

            IEnumerator itr = treeNodeColl.GetEnumerator();
            //while (itr.MoveNext())
                //TreeView1.Nodes.Add((TreeNode)itr.Current);

            TreeView1.DataSource = itr;
            
            //TreeView1.Nodes.add
            if (TreeView1.Nodes.Count > 0)
            {
                Panel_Reporting_Users.Visible = true;
                TreeView1.Visible = true;
                GridView_Reporting_Users.Visible = false;
            }
        }

        protected void Button_Add_New_Usr_Click(object sender, EventArgs e)
        {
            Panel_Create_New_Usr.Visible = true;
        }

        protected void Create_Chain_User_Click(object sender, EventArgs e)
        {
            userDetails udTest = BackEndObjects.userDetails.getUserDetailsbyIdDB(TextBox1.Text);
            if (udTest.getUserId() == null || udTest.getUserId().Equals("")) //New user id
            {
                userDetails uD = new userDetails();
                uD.setMainEntityId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

                Random ranGen = new Random();
                int saltInt = ranGen.Next(1, 16);
                byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes((TextBox2.Text.Equals("") ? TextBox2.Attributes["password"] : TextBox2.Text)
                    + saltInt);
                HashAlgorithm hashConverter = new SHA256Managed();
                byte[] hashedByteStream = hashConverter.ComputeHash(plainTextBytes);
                String encryptedAndConvertedPassword = Convert.ToBase64String(hashedByteStream);

                //uD.setSubEntityId(DropDownList1.SelectedValue);
                uD.setUserId(TextBox1.Text);
                uD.setPassword(encryptedAndConvertedPassword);
                uD.setSalt(saltInt.ToString());
                uD.setName(TextBox_User_Name_NewAccount.Text);

                Dictionary<String, userDetails> userList = MainBusinessEntity.getUserDetailsforMainEntitybyIdDB(uD.getMainEntityId());
                if (userList.ContainsKey(uD.getUserId()))
                {
                    Label2.Visible = true;
                    Label2.ForeColor = System.Drawing.Color.Red;
                    Label2.Text = "This user account is already created for your organization";
                }
                else
                {
                    ArrayList uDChains = new ArrayList();
                    uDChains.Add(uD);
                    ActionLibrary.RegistrationActions regstr = new RegistrationActions();
                    try
                    {
                        regstr.completeRegr(uDChains);
                        Label2.Visible = true;
                        Label2.ForeColor = System.Drawing.Color.Green;
                        Label2.Text = "Account created successfully. User can login and enter contact details and other details from user preference page.";

                        DataTable dt = (DataTable)Session[SessionFactory.ADMIN_PREF_USER_MGMT_BASIC_USER_DET_GRID];
                        int count = dt.Rows.Count;
                        dt.Rows.Add();
                        dt.Rows[count]["UserName"] = uD.getName();
                        dt.Rows[count]["UserId"] = uD.getUserId();

                        GridView_User_List.DataSource = dt;
                        GridView_User_List.DataBind();
                        Session[SessionFactory.ADMIN_PREF_USER_MGMT_BASIC_USER_DET_GRID] = dt;
                    }
                    catch (Exception ex)
                    {
                        Label2.Visible = true;
                        Label2.ForeColor = System.Drawing.Color.Red;
                        Label2.Text = "Account creation failed";
                    }
                }
            }
            else
            {
                Label2.Visible = true;
                Label2.ForeColor = System.Drawing.Color.Red;
                Label2.Text = "User Id is not available..please choose another one";
            }
        }

        protected void Hide_Chain_User_Click(object sender, EventArgs e)
        {
            Panel_Create_New_Usr.Visible = false;
        }
        
    }
}