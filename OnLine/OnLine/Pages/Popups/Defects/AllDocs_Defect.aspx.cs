using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BackEndObjects;
using ActionLibrary;
using System.Data;

namespace OnLine.Pages.Popups.Defects
{
    public partial class AllDocs_Defect : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                                Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];
                                if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                                    FileUpload1.Enabled = true;
                                else
                                {
                                    String context = Request.QueryString.GetValues("contextId")[0];
                                    FileUpload1.Enabled = (context.Equals("incoming") ? accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_EDIT_INCOMING_DEFECT] :
                                        accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_EDIT_OUTGOING_DEFECT]);
                                }
                                if (!FileUpload1.Enabled)
                                {
                                    Button_Submit.Enabled = false;
                                    Label_Upload_Stat.Visible = true;
                                    Label_Upload_Stat.Text = "You don't have edit access";
                                    Label_Upload_Stat.ForeColor = System.Drawing.Color.Red;
                                }

                //contextId1 is the defect id
                                String docName = Request.QueryString.GetValues("contextId2")[0];
                                LinkButton_Existing_Doc.Text = docName;

            }
        }

        protected void loadDocumentForDefect(String defectId)
        {
         //   BackEndObjects.DefectDetails.getDefectDetailsbyidDB(defectId)
        }

        protected void LinkButton_Existing_Doc_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandArgument.Equals("new"))
            {
                ActionLibrary.ImageContextFactory icObj = new ImageContextFactory();

                icObj.setParentContextName(ActionLibrary.ImageContextFactory.PARENET_CONTEXT_DEFECT);
                icObj.setParentContextValue(Request.QueryString.GetValues("contextId1")[0]);
                icObj.setDestinationContextName(ActionLibrary.ImageContextFactory.DESTINATION_CONTEXT_DOC_FOR_PARENT_DEFECT);

                Session[SessionFactory.DISP_IMAGE_CONTEXT_FACTORY_OBJ] = icObj;
                Response.Redirect("/Pages/DispImage.aspx", true);
            }
        }

        protected void Button_Submit_Click(object sender, EventArgs e)
        {
            if (FileUpload1 != null && FileUpload1.HasFile)
            {
                BackEndObjects.DefectDetails defObj = BackEndObjects.DefectDetails.getDefectDetailsbyidDB(Request.QueryString.GetValues("contextId1")[0]);
                defObj.setFileStream(FileUpload1);
                try
                {
                    BackEndObjects.DefectDetails.updateorInsertDefectDocDB(defObj);
                    Label_Upload_Stat.Visible = true;
                    Label_Upload_Stat.Text = "Document Uploaded Successfully";
                    Label_Upload_Stat.ForeColor = System.Drawing.Color.Green;

                    String docName = "";
                    if (defObj.getDocPath() != null)
                    {
                        String[] docPathList = defObj.getDocPath().
                            Split(new String[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                        if (docPathList.Length > 0)
                            docName = docPathList[docPathList.Length - 1];
                    }

                    LinkButton_Existing_Doc.Text = docName;

                    String context = Request.QueryString.GetValues("contextId")[0];
                    String dataItemIndex = Request.QueryString.GetValues("dataItemIndex")[0];
                    


                    if (context.Equals("incoming"))
                    {
                        DataTable dt = (DataTable)Session[SessionFactory.ALL_DEFECT_ALL_INCOMING_DEFECT_GRID];
                        dt.Rows[Int32.Parse(dataItemIndex)]["docNameHidden"] = docName;
                       dt.DefaultView.Sort = "Submit Date Ticks" + " " + "DESC";
                        Session[SessionFactory.ALL_DEFECT_ALL_INCOMING_DEFECT_GRID] = dt.DefaultView.ToTable();

                        ScriptManager.RegisterStartupScript(this, typeof(string), "RefreshDefectGridIncm", "RefreshParentIncm();", true);
                    }
                    else
                    {
                        DataTable dt = (DataTable)Session[SessionFactory.ALL_DEFECT_ALL_OUTGOING_DEFECT_GRID];
                        dt.Rows[Int32.Parse(dataItemIndex)]["docNameHidden"] = docName;
                        dt.DefaultView.Sort = "Submit Date Ticks" + " " + "DESC";
                        Session[SessionFactory.ALL_DEFECT_ALL_OUTGOING_DEFECT_GRID] = dt.DefaultView.ToTable();

                        ScriptManager.RegisterStartupScript(this, typeof(string), "RefreshDefectGridIncm", "RefreshParentIncm();", true);
                    }


                }
                catch (Exception ex)
                {
                    Label_Upload_Stat.Visible = true;
                    Label_Upload_Stat.Text = "Document Upload Failed";
                    Label_Upload_Stat.ForeColor = System.Drawing.Color.Red;
                }

            }
        }
    }
}