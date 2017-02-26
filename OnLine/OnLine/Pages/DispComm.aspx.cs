using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BackEndObjects;
using ActionLibrary;
using System.Data;


namespace OnLine.Pages
{
    public partial class DispComm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                fillGrid();
        }

        protected void fillGrid()
        {
            String[] contextId = Request.QueryString.GetValues("contextId");

            Dictionary<String, BackEndObjects.Communications> allComm = Communications.
                getAllCommunicationsforContextIdAndContextTypeAndSourceIdDB(
                contextId[0],
                Communications.COMMUNICATIONS_CONTEXT_TYPE_MSG,
                Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());



            DataTable dt = new DataTable();

            dt.Columns.Add("HiddenComm");
            dt.Columns.Add("RespDateTime");
            dt.Columns.Add("RespDateTimeTicks");
            dt.Columns.Add("FromUsr");
            dt.Columns.Add("docName");
            dt.Columns.Add("Comments");
            // dt.Rows[i]["Submit Date Ticks"] = 

            int counter = 0;

            foreach (KeyValuePair<String, BackEndObjects.Communications> kvp in allComm)
            {
                BackEndObjects.Communications commObj = kvp.Value;
                dt.Rows.Add();

                String docName = "";
                if (commObj.getDocPath() != null && !commObj.getDocPath().Equals(""))
                {
                    String[] docPathList = commObj.getDocPath().
                        Split(new String[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                    if (docPathList.Length > 0)
                        docName = docPathList[docPathList.Length - 1];
                }

                dt.Rows[counter]["HiddenComm"] = commObj.getCommId();
                dt.Rows[counter]["RespDateTime"] = commObj.getDateCreated();
                dt.Rows[counter]["RespDateTimeTicks"] = Convert.ToDateTime(commObj.getDateCreated()).Ticks;
                dt.Rows[counter]["FromUsr"] = commObj.getFromUserId();
                dt.Rows[counter]["docName"] = (docName == null || docName.Equals("") ? "N/A" : 
                    docName.Remove(0, (docName.IndexOf("_") + 1+docName.Substring(docName.IndexOf("_") + 1).IndexOf("_")+1)));
                dt.Rows[counter]["Comments"] = commObj.getText();

                counter++;
            }

            if (dt.Rows.Count > 0)
            {
                dt.DefaultView.Sort = "RespDateTimeTicks" + " " + "DESC";
                DataTable sortedTable = dt.DefaultView.ToTable();

                GridView_Notes.DataSource = sortedTable;
                GridView_Notes.DataBind();
                GridView_Notes.Visible = true;
                GridView_Notes.Columns[1].Visible = false;

                Dictionary<String, DataTable> notesData = new Dictionary<string, DataTable>();
                notesData.Add(contextId[0], sortedTable);

                Session[SessionFactory.ALL_MSGS_DATAGRID] = notesData;
            }
        }

        protected void Button_Submit_Note_Click(object sender, EventArgs e)
        {
            String[] contextId = Request.QueryString.GetValues("contextId");

            BackEndObjects.Communications commObj = new BackEndObjects.Communications();
            commObj.setCommId(new Id().getNewId(Id.ID_TYPE_COMM_STRING));
            commObj.setContextId(contextId[0]);

            String dateCreated = (TextBox_Email_Date.Text + " " + (!TextBox_Email_Time_HH.Text.Equals("")?TextBox_Email_Time_HH.Text:"00") + ":"
                + (!TextBox_Email_Time_MM.Text.Equals("")?TextBox_Email_Time_MM.Text :"00")+ ":"
                + (!TextBox_Email_Time_SS.Text.Equals("")?TextBox_Email_Time_SS.Text:"00")).Trim();
            commObj.setDateCreated(dateCreated);
            commObj.setFromEntityId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            commObj.setFromUserId(TextBox_Email_From.Text.Trim());
            commObj.setText(TextBox_Note.Text);
            commObj.setToEntityId("");
            commObj.setToUserId("");
            commObj.setContextType(Communications.COMMUNICATIONS_CONTEXT_TYPE_MSG);
            
            if (FileUpload_Attachment != null && FileUpload_Attachment.HasFile)
            {
                commObj.setFileStream(FileUpload_Attachment);
                commObj.setDocPathInFileStore(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            }

            try
            {
                BackEndObjects.Communications.insertCommunicationDB(commObj);
                Label_Insert_Stat.Visible = true;
                Label_Insert_Stat.Text = "Note Saved Successfully";
                Label_Insert_Stat.ForeColor = System.Drawing.Color.Green;

                Dictionary<String, DataTable> notesData = (Dictionary<String, DataTable>)Session[SessionFactory.ALL_MSGS_DATAGRID];

                DataTable dt = null;

                if (notesData != null)
                {
                    if (notesData.ContainsKey(contextId[0]))
                        dt = notesData[contextId[0]];
                    else
                    {
                        dt = new DataTable();
                        dt.Columns.Add("HiddenComm");
                        dt.Columns.Add("RespDateTime");
                        dt.Columns.Add("FromUsr");
                        dt.Columns.Add("docName");
                        dt.Columns.Add("Comments");
                    }
                }
                else
                {
                    dt = new DataTable();
                    dt.Columns.Add("HiddenComm");
                    dt.Columns.Add("RespDateTime");
                    dt.Columns.Add("FromUsr");
                    dt.Columns.Add("docName");
                    dt.Columns.Add("Comments");
                }

                int counter = 0;

                if (dt == null)
                    dt = new DataTable();
                else
                    counter = dt.Rows.Count;

                dt.Rows.Add();
                String docName = "";

                if (commObj.getDocPath() != null && !commObj.getDocPath().Equals(""))
                {
                    String[] docPathList = commObj.getDocPath().
                        Split(new String[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                    if (docPathList.Length > 0)
                        docName = docPathList[docPathList.Length - 1];
                }

                dt.Rows[counter]["HiddenComm"] = commObj.getCommId();
                dt.Rows[counter]["RespDateTime"] = commObj.getDateCreated();
                dt.Rows[counter]["FromUsr"] = commObj.getFromUserId();
                dt.Rows[counter]["docName"] = (docName == null || docName.Equals("") ? "N/A" :
                    docName.Remove(0, (docName.IndexOf("_") + 1 + docName.Substring(docName.IndexOf("_") + 1).IndexOf("_") + 1)));
                dt.Rows[counter]["Comments"] = commObj.getText();

                dt.DefaultView.Sort = "RespDateTime" + " " + "DESC";
                DataTable sorteddt = dt.DefaultView.ToTable();

                GridView_Notes.DataSource = sorteddt;
                GridView_Notes.DataBind();
                GridView_Notes.Visible = true;
                GridView_Notes.Columns[1].Visible = false;

                if (notesData == null)
                    notesData = new Dictionary<string, DataTable>();

                if (notesData.ContainsKey(contextId[0]))
                    notesData[contextId[0]] = sorteddt;
                else
                    notesData.Add(contextId[0], sorteddt);

                Session[SessionFactory.ALL_MSGS_DATAGRID] = notesData;
            }
            catch (Exception ex)
            {
                Label_Insert_Stat.Visible = true;
                Label_Insert_Stat.Text = "Note Could not be saved";
                Label_Insert_Stat.ForeColor = System.Drawing.Color.Red;
            }

        }

        protected void LinkButton_Doc_Command(object sender, CommandEventArgs e)
        {
            ActionLibrary.ImageContextFactory icObj = new ImageContextFactory();
            GridViewRow gVR = GridView_Notes.Rows[int.Parse(e.CommandArgument.ToString())];

            icObj.setParentContextName(ActionLibrary.ImageContextFactory.PARENT_CONTEXT_NOTES);
            icObj.setParentContextValue(((Label)gVR.Cells[0].FindControl("Label_Hidden_Comm")).Text);
            icObj.setDestinationContextName(ActionLibrary.ImageContextFactory.DESTINATION_CONTEXT_DOC_FOR_PARENT_NOTE);

            Session[SessionFactory.DISP_IMAGE_CONTEXT_FACTORY_OBJ] = icObj;
            Response.Redirect("DispImage.aspx");
            //Server.Transfer("DispImage.aspx", true);

        }

        protected void GridView_Notes_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            String commId = ((Label)GridView_Notes.Rows[e.RowIndex].Cells[0].FindControl("Label_Hidden_Comm")).Text;

            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            Dictionary<String, String> targetVals = new Dictionary<string, string>();

            whereCls.Add(BackEndObjects.Communications.COMMUNICATIONS_COL_COMM_ID, commId);
             BackEndObjects.Communications.updateCommunicationDB(targetVals, whereCls, DBConn.Connections.OPERATION_DELETE);

             DataTable dt = ((Dictionary<String,DataTable>)Session[SessionFactory.ALL_MSGS_DATAGRID])[Request.QueryString.GetValues("contextId")[0]];
            int index = GridView_Notes.Rows[e.RowIndex].DataItemIndex;
            dt.Rows[index].Delete();

            
            GridView_Notes.DataSource = dt;
            GridView_Notes.DataBind();
        }

        protected void GridView_Notes_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            ViewState["SortExpressionNotes"] = sortExpression;

            String[] contextId = Request.QueryString.GetValues("contextId");
            Dictionary<String, DataTable> notesData =(Dictionary<String,DataTable>) Session[SessionFactory.ALL_MSGS_DATAGRID];
            DataTable dt = notesData[contextId[0]];
   

            if (GridViewSortDirectionNotes == SortDirection.Ascending)
            {
                GridViewSortDirectionNotes = SortDirection.Descending;
                dt.DefaultView.Sort = sortExpression + " " + "DESC";
            }
            else
            {
                GridViewSortDirectionNotes = SortDirection.Ascending;
                dt.DefaultView.Sort = sortExpression + " " + "ASC";
            }

            DataTable sortedTable = dt.DefaultView.ToTable();
            GridView_Notes.SelectedIndex = -1;
            bindSortedData(GridView_Notes, sortedTable);
        }

        protected void bindSortedData(GridView grd, DataTable dt)
        {
            grd.DataSource = dt;
            grd.DataBind();
        }

        private SortDirection GridViewSortDirectionNotes
        {
            get
            {
                if (ViewState["sortDirectionNotes"] == null)
                    ViewState["sortDirectionNotes"] = SortDirection.Descending;
                return (SortDirection)ViewState["sortDirectionNotes"];
            }
            set { ViewState["sortDirectionNotes"] = value; }
        }
    }
}