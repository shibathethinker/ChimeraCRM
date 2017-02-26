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
    public partial class ReportingStructure : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //loadTreeView();
                loadUserList();
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

                DropDownList_Users.Items.Add(lt1);
                //DropDownList_Users_Add_Members.Items.Add(lt1);
            }

            ListItem emptyItem = new ListItem();
            emptyItem.Text = "";
            emptyItem.Value = "";

            DropDownList_Users.Items.Add(emptyItem);
            DropDownList_Users.SelectedValue = "";

            //DropDownList_Users_Add_Members.Items.Add(emptyItem);
            //DropDownList_Users_Add_Members.SelectedValue = "";
        }

        protected void createHierarchy(Dictionary<String, TreeNode> parentNodesDict, TreeNode childNode,userDetails currentUserObject)
        {
            if (parentNodesDict.ContainsKey(currentUserObject.getReportsTo()))
                parentNodesDict[currentUserObject.getReportsTo()].ChildNodes.Add(childNode);
            else
            {
                TreeNode child = new TreeNode();
                LinkedList<TreeNode> n = new LinkedList<TreeNode>();
               
            }

        }

        /*protected void loadTreeView()
        {
            
            Dictionary<String, userDetails> allUserDetails = BackEndObjects.MainBusinessEntity.getUserDetailsforMainEntitybyIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            Cache.Remove("allUserDetailsReportingStructure");
            Cache["allUserDetailsReportingStructure"] = allUserDetails;

            Dictionary<String, TreeNode> parentNodesDict = new Dictionary<string, TreeNode>();

            TreeNode emptyParent = new TreeNode();
            emptyParent.Text = "N/A";
            emptyParent.Value = "N/A";
            parentNodesDict.Add("N/A",emptyParent);

            foreach (KeyValuePair<String, userDetails> kvp in allUserDetails)
            {
                userDetails userObj = kvp.Value;
                if (userObj.getReportsTo() == null || userObj.getReportsTo().Equals("")) 
                {
                    TreeNode parentNode = new TreeNode();
                    parentNode.Text = userObj.getUserId();
                    parentNode.Value = userObj.getUserId();

                    //emptyParent.ChildNodes.Add(parentNode);
                    parentNodesDict["N/A"].ChildNodes.Add(parentNode);
                    //parentNodesDict.Add(userObj.getUserId(), parentNode);
                }
                else
                {
                    if(!parentNodesDict.ContainsKey(userObj.getReportsTo()))
                    {
                                            TreeNode parentNode = new TreeNode();
                                            parentNode.Text = userObj.getReportsTo();
                                            parentNode.Value = userObj.getReportsTo();

                    parentNodesDict.Add(userObj.getReportsTo(), parentNode);
                    }

                    TreeNode childNode = new TreeNode();
                    childNode.Text = userObj.getUserId();
                    childNode.Value = userObj.getUserId();
                    parentNodesDict[userObj.getReportsTo()].ChildNodes.Add(childNode);
                }
            }

            foreach (KeyValuePair<String, TreeNode> kvp in parentNodesDict)
                   //TreeView1.Nodes.Add(kvp.Value);
            
            
        }*/

        protected void DropDownList_Users_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Populate the second dropdown list based on
            //The second list users should not be reporting to the selected value
            //The second list will not have the same selected item
            Dictionary<String, userDetails> allUserDetails = (Dictionary<String, userDetails>)Cache["allUserDetailsReportingStructure"];

            foreach (KeyValuePair<String, userDetails> kvp in allUserDetails)
            {
                if (!DropDownList_Users.SelectedValue.Equals(kvp.Key) &&
                    (kvp.Value.getReportsTo() == null || !kvp.Value.getReportsTo().Equals(DropDownList_Users.SelectedValue)))
                {
                    ListItem lt = new ListItem();
                    lt.Text = kvp.Key;
                    lt.Value = kvp.Key;
                    DropDownList_Reporting_To.Items.Add(lt);
                }
            }

            ListItem ltEmpty = new ListItem();
            ltEmpty.Text = "";
            ltEmpty.Value = "";
            DropDownList_Reporting_To.Items.Add(ltEmpty);
            DropDownList_Reporting_To.SelectedValue = "";
        }

        protected void Button_Add_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            Dictionary<String, String> targetVals = new Dictionary<string, string>();

            whereCls.Add(BackEndObjects.userDetails.USER_DETAILS_COL_BUSINESS_ID, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            whereCls.Add(BackEndObjects.userDetails.USER_DETAILS_COL_USERID, DropDownList_Users.SelectedValue);

            targetVals.Add(BackEndObjects.userDetails.USER_DETAILS_COL_REPORTS_TO, DropDownList_Reporting_To.SelectedValue);

            try
            {
                BackEndObjects.userDetails.updateUserDetailsDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
                //loadTreeView();
            }
            catch (Exception ex)
            {
            }
        }


    }
}