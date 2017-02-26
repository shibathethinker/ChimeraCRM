using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BackEndObjects;
using ActionLibrary;
using System.Data;

namespace OnLine.Pages.Popups.Purchase
{
    public partial class AllRFQ_NDA : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

                LinkButton_New_NDA.Text = Request.QueryString.GetValues("contextId2")[0];

                if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS] ||
                    accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_RFQ])
                    FileUpload1.Enabled = true;
                else
                {
                    FileUpload1.Enabled = false;
                    Button_Submit.Enabled = false;
                    Label_Upload_Stat.Text = "You dont have edit access to RFQ records";
                    Label_Upload_Stat.Visible = true;
                    Label_Upload_Stat.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        protected void Link_Show_NDA(object sender, CommandEventArgs e)
        {
            if (e.CommandArgument.Equals("new"))
            {
                ActionLibrary.ImageContextFactory icObj = new ImageContextFactory();

                icObj.setParentContextName(ActionLibrary.ImageContextFactory.PARENT_CONTEXT_RFQ);
                icObj.setParentContextValue(Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SELECTED_RFQ_ID].ToString());
                icObj.setDestinationContextName(ActionLibrary.ImageContextFactory.DESTINATION_CONTEXT_NDA_FOR_PARENT_RFQ);

                Session[SessionFactory.DISP_IMAGE_CONTEXT_FACTORY_OBJ] = icObj;
                Response.Redirect("/Pages/DispImage.aspx", true);
            }
           
        }
        /// <summary>
        /// Upload the NDA copy for this response
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_Submit_Click(object sender, EventArgs e)
        {
            //RFQResponse respObj = RFQResponse.
                //getRFQResponseforRFQIdandResponseEntityIdDB(Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SELECTED_RFQ_ID].ToString(),

                RFQDetails rfqObj=RFQDetails.getRFQDetailsbyIdDB(Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SELECTED_RFQ_ID].ToString());
                
            try
            {
                if (rfqObj != null && rfqObj.getRFQId() != null && !rfqObj.getRFQId().Equals(""))
                {
                    rfqObj.setFileStream(FileUpload1);
                    RFQDetails.updateorInsertRFQNDADB(rfqObj);                    
                }
       
                Label_Upload_Stat.Visible = true;
                Label_Upload_Stat.Text = "upload successfull";
                Label_Upload_Stat.ForeColor = System.Drawing.Color.Green;

                String docName = "";
                if (rfqObj.getNDADocPath() != null)
                {
                    String[] docPathList = rfqObj.getNDADocPath().
                        Split(new String[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                    if (docPathList.Length > 0)
                        docName = docPathList[docPathList.Length - 1];
                }

                LinkButton_New_NDA.Text = docName;

                DataTable dt = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_GRID_DATA];
                int dataItemIndex = Int32.Parse(Request.QueryString.GetValues("dataItemIndex")[0]);
                dt.Rows[dataItemIndex]["Hidden_Doc_Name"] = docName;
                dt.DefaultView.Sort = "Submit Date Ticks" + " " + "DESC";
                Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_GRID_DATA] = dt.DefaultView.ToTable();
                ScriptManager.RegisterStartupScript(this, typeof(string), "RefreshRefqGrid", "RefreshParent();", true);
            }
            catch (Exception ex)
            {
                Label_Upload_Stat.Visible = true;
                Label_Upload_Stat.Text = "upload failed";
                Label_Upload_Stat.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}