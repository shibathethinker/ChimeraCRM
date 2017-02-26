using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BackEndObjects;
using ActionLibrary;

namespace OnLine.Pages
{
    public partial class NoteDetails : System.Web.UI.Page
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
            String[] contextId = Request.QueryString.GetValues("contextId");

            Dictionary <String, BackEndObjects.Communications> allComm = Communications.
                getAllCommunicationsforContextIdAndContextTypeAndSourceIdDB(
                contextId[0], 
                Communications.COMMUNICATIONS_CONTEXT_TYPE_NOTE, 
                Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                
                

            DataTable dt = new DataTable();
   
            dt.Columns.Add("HiddenComm");
            dt.Columns.Add("RespDateTime");
            dt.Columns.Add("FromUsr");
            dt.Columns.Add("docName");
            dt.Columns.Add("Comments");

   
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
                dt.Rows[counter]["FromUsr"] = commObj.getFromUserId();
                dt.Rows[counter]["docName"] = (docName == null || docName.Equals("") ? "N/A" :
                    docName.Remove(0, (docName.IndexOf("_") + 1 + docName.Substring(docName.IndexOf("_") + 1).IndexOf("_") + 1)));
                dt.Rows[counter]["Comments"] = commObj.getText();

                counter++;
            }

            if (dt.Rows.Count > 0)
            {
                dt.DefaultView.Sort = "RespDateTime" + " " + "DESC";
                DataTable sortedTable=dt.DefaultView.ToTable();
                
                GridView_Notes.DataSource = sortedTable;
                GridView_Notes.DataBind();
                GridView_Notes.Visible = true;
                GridView_Notes.Columns[0].Visible = false;
                
                Dictionary<String, DataTable> notesData = new Dictionary<string, DataTable>();
                notesData.Add(contextId[0], sortedTable);

                Session[SessionFactory.ALL_NOTES_DATAGRID] = notesData;
            }
        }

        protected void Button_Submit_Note_Click(object sender, EventArgs e)
        {
            String[] contextId = Request.QueryString.GetValues("contextId");
            
            BackEndObjects.Communications commObj = new BackEndObjects.Communications();
            commObj.setCommId(new Id().getNewId(Id.ID_TYPE_COMM_STRING));
            commObj.setContextId(contextId[0]);
            commObj.setDateCreated(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            commObj.setFromEntityId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            commObj.setFromUserId(User.Identity.Name);
            commObj.setText(TextBox_Note.Text);
            commObj.setToEntityId("");
            commObj.setToUserId("");
            commObj.setContextType(Communications.COMMUNICATIONS_CONTEXT_TYPE_NOTE);

            if (FileUpload_Attachment != null && FileUpload_Attachment.HasFile)
            {
                if (FileUpload_Attachment.PostedFile.ContentLength == 0)
                {
                    Label_Insert_Stat.Text = "Empty file will not be uploaded";
                    Label_Insert_Stat.Visible = true;
                    Label_Insert_Stat.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    Label_Insert_Stat.Visible = false;
                    commObj.setFileStream(FileUpload_Attachment);
                    commObj.setDocPathInFileStore(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                }
            }
            

            try
            {
                BackEndObjects.Communications.insertCommunicationDB(commObj);
                Label_Insert_Stat.Visible = true;
                Label_Insert_Stat.Text = "Note Saved Successfully";
                Label_Insert_Stat.ForeColor = System.Drawing.Color.Green;

                Dictionary<String,DataTable> notesData = (Dictionary<String,DataTable>)Session[SessionFactory.ALL_NOTES_DATAGRID];

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
                    dt=new DataTable();
                    dt.Columns.Add("HiddenComm");
                    dt.Columns.Add("RespDateTime");
                    dt.Columns.Add("FromUsr");
                    dt.Columns.Add("docName");
                    dt.Columns.Add("Comments");
                }

                int counter=0;

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
                DataTable sorteddt=dt.DefaultView.ToTable();
                
                GridView_Notes.DataSource = sorteddt;
                GridView_Notes.DataBind();
                GridView_Notes.Visible = true;
                GridView_Notes.Columns[0].Visible = false;

                if (notesData == null)
                    notesData = new Dictionary<string, DataTable>();

                if (notesData.ContainsKey(contextId[0]))
                    notesData[contextId[0]] = sorteddt;
                else
                    notesData.Add(contextId[0], sorteddt);

                Session[SessionFactory.ALL_NOTES_DATAGRID] = notesData;
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
    }
}