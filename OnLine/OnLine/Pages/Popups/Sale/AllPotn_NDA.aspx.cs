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

namespace OnLine.Pages.Popups.Sale
{
    public partial class AllPotn_NDA : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String createMode = Request.QueryString.GetValues("createMode")[0];
            //For Manually created potential dont show the "original document uploaded by client" link
            if (createMode.Equals(BackEndObjects.RFQShortlisted.POTENTIAL_CREATION_MODE_MANUAL))
            { orgCopyHeader.Visible = false; orgCopyLink.Visible = false; }

            Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];
            if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS] ||
                accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_POTENTIAL])
                FileUpload1.Enabled = true;
            else
            {
                FileUpload1.Enabled = false;
                Button_Submit.Enabled = false;
                Label_Upload_Stat.Text = "You dont have edit access to Potential records";
                Label_Upload_Stat.Visible = true;                
                Label_Upload_Stat.ForeColor = System.Drawing.Color.Red;
            }

            RFQDetails rfqObj = RFQDetails.getRFQDetailsbyIdDB(Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_SELECTED_RFQ_ID].ToString());
            String docName = "";
            if (rfqObj.getNDADocPath() != null && !rfqObj.getNDADocPath().Equals(""))
            {
                String[] docPathList = rfqObj.getNDADocPath().
                    Split(new String[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                if (docPathList.Length > 0)
                    docName = docPathList[docPathList.Length - 1];

                LinkButton_Org_NDA_Path.Text = docName;
            }
            else
                LinkButton_Org_NDA_Path.Text = "N/A";


            RFQResponse respObj = RFQResponse.getRFQResponseforRFQIdandResponseEntityIdDB(Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_SELECTED_RFQ_ID].ToString(), Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            if (respObj.getNdaPath() != null && !respObj.getNdaPath().Equals(""))
            {
                String[] docPathList = respObj.getNdaPath().
Split(new String[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                if (docPathList.Length > 0)
                    docName = docPathList[docPathList.Length - 1];

                LinkButton_New_NDA.Text = docName;
            }
            else
                LinkButton_New_NDA.Text = "N/A";

        }


        protected void Link_Show_NDA(object sender, CommandEventArgs e)
        {
            if (e.CommandArgument.Equals("org"))
            {
                ActionLibrary.ImageContextFactory icObj = new ImageContextFactory();

                icObj.setParentContextName(ActionLibrary.ImageContextFactory.PARENT_CONTEXT_RFQ);
                icObj.setParentContextValue(Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_SELECTED_RFQ_ID].ToString());
                icObj.setDestinationContextName(ActionLibrary.ImageContextFactory.DESTINATION_CONTEXT_NDA_FOR_PARENT_RFQ);

                Session[SessionFactory.DISP_IMAGE_CONTEXT_FACTORY_OBJ] = icObj;
                Response.Redirect("/Pages/DispImage.aspx", true);
            }
            else if (e.CommandArgument.Equals("new"))
            {
                ActionLibrary.ImageContextFactory icObj = new ImageContextFactory();

                icObj.setParentContextName(ActionLibrary.ImageContextFactory.PARENT_CONTEXT_RFQ_RESPONSE);
                icObj.setParentContextValue(Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_SELECTED_RFQ_ID].ToString());
                icObj.setDestinationContextName(ActionLibrary.ImageContextFactory.DESTINATION_CONTEXT_NDA_FOR_PARENT_RFQ_RESPONSE);

                Dictionary<String, String> childContextDict = new Dictionary<string, string>();
                childContextDict.Add(ActionLibrary.ImageContextFactory.CHILD_CONTEXT_RFQ_RESPONSE_RESPONSE_ENTITY_ID, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                icObj.setChildContextObjects(childContextDict);

                Session[SessionFactory.DISP_IMAGE_CONTEXT_FACTORY_OBJ] = icObj;
                // Server.Transfer("/Pages/DispImage.aspx", true);
                Response.Redirect("/Pages/DispImage.aspx");
            }

        }
        /// <summary>
        /// Upload the NDA copy for this response
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_Submit_Click(object sender, EventArgs e)
        {
            RFQResponse respObj = RFQResponse.getRFQResponseforRFQIdandResponseEntityIdDB(Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_SELECTED_RFQ_ID].ToString(),
                Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            try
            {
                if (respObj != null && respObj.getRFQId() != null && !respObj.getRFQId().Equals(""))
                {
                    respObj.setFileStream(FileUpload1);
                    RFQResponse.updateorInsertRFQResponseNDADB(respObj);
                }
                else
                {
                    respObj.setRespEntityId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                    respObj.setRFQId(Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_SELECTED_RFQ_ID].ToString());
                    respObj.setRespDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    //Load the document for the lead response 
                    if (FileUpload1 != null && FileUpload1.HasFile)
                    {
                        respObj.setFileStream(FileUpload1);
                        respObj.setNDADocPathInFileStore(respObj.getRespEntityId());
                    }

                }
                Label_Upload_Stat.Visible = true;
                Label_Upload_Stat.Text = "upload successfull";
                Label_Upload_Stat.ForeColor = System.Drawing.Color.Green;

                String docName = "";
                if (respObj.getNdaPath() != null && !respObj.getNdaPath().Equals(""))
                {
                    String[] docPathList = respObj.getNdaPath().
    Split(new String[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                    if (docPathList.Length > 0)
                        docName = docPathList[docPathList.Length - 1];

                    LinkButton_New_NDA.Text = docName;
                }
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