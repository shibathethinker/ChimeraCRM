using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BackEndObjects;
using ActionLibrary;
using System.Collections;

namespace OnLine.Pages
{
    public partial class DispImage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Page.IsPostBack)
                //Response.ContentType = "application/pdf";
            //Response.Clear();
            string filePath = "G:\\FileStoreRoot\\cmp_158\\ra_137\\img_90_Cleartrip Flight E-Ticket.pdf";
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Type", "application/pdf");
            Response.WriteFile(filePath);
            Response.End();
        }

        public DataTable poPulateImage()
        {

            ActionLibrary.ImageContextFactory icObj = (ActionLibrary.ImageContextFactory)Session[SessionFactory.DISP_IMAGE_CONTEXT_FACTORY_OBJ];
            DataTable dt=new DataTable();
            dt.Columns.Add("img");
            String serverPath = "";

            switch(icObj.getParentContextName())
            {
                case ActionLibrary.ImageContextFactory.PARENT_CONTEXT_REQUIREMENT:
                    int counter = 0;
                    if (icObj.getDestinationContextName().Equals
                        (ActionLibrary.ImageContextFactory.DESTINATION_CONTEXT_FEAT_FOR_PARENT_REQUIREMENT))
                    {
                        String prodCatId = icObj.getChildContextObjects()[ActionLibrary.ImageContextFactory.CHILD_CONTEXT_PRODCAT_ID],
featId = icObj.getChildContextObjects()[ActionLibrary.ImageContextFactory.CHILD_CONTEXT_FEAT_ID];

                        ArrayList reqSpecList = BackEndObjects.Requirement_Spec.getRequirementSpecsforReqbyIdDB(icObj.getParentContextValue());

                        
                        for (int i = 0; i < reqSpecList.Count; i++)
                        {
                            BackEndObjects.Requirement_Spec reqrObj = (Requirement_Spec)reqSpecList[i];
                            if (reqrObj.getProdCatId().Equals(prodCatId) && reqrObj.getFeatId().Equals(featId))
                            {
                                serverPath = reqrObj.getImgPath();
                                dt.Rows.Add();
                                dt.Rows[counter]["img"] = serverPath;
                                counter++;
                            }
                        }
                    }
                    break;

                case ActionLibrary.ImageContextFactory.PARENT_CONTEXT_RFQ: 

                    if(icObj.getDestinationContextName().Equals(ActionLibrary.ImageContextFactory.DESTINATION_CONTEXT_FEAT_FOR_PARENT_RFQ))
                    {
                        String prodCatId = icObj.getChildContextObjects()[ActionLibrary.ImageContextFactory.CHILD_CONTEXT_PRODCAT_ID],
featId = icObj.getChildContextObjects()[ActionLibrary.ImageContextFactory.CHILD_CONTEXT_FEAT_ID];

                                        ArrayList rfqSpecList=BackEndObjects.RFQProductServiceDetails.getAllProductServiceDetailsbyRFQIdDB(icObj.getParentContextValue());
                    
                    counter=0;
                    for (int i = 0; i < rfqSpecList.Count; i++)
                    {
                        BackEndObjects.RFQProductServiceDetails rfqObj = (RFQProductServiceDetails)rfqSpecList[i];
                        if (rfqObj.getPrdCatId().Equals(prodCatId) && rfqObj.getFeatId().Equals(featId))
                        {
                            serverPath = rfqObj.getImgPath();
                            dt.Rows.Add();
                            dt.Rows[counter]["img"] = serverPath;
                            counter++;
                        }
                    }
                     
                    }
                    if (icObj.getDestinationContextName().Equals(ActionLibrary.ImageContextFactory.DESTINATION_CONTEXT_NDA_FOR_PARENT_RFQ))
                    {
                        serverPath = BackEndObjects.RFQDetails.getRFQDetailsbyIdDB(icObj.getParentContextValue()).getNDADocPath();
                        dt.Rows.Add();
                        dt.Rows[0]["img"] = serverPath;
                    }
                    break;

                case ActionLibrary.ImageContextFactory.PARENT_CONTEXT_RFQ_RESPONSE:

                    if (icObj.getDestinationContextName().Equals(ActionLibrary.ImageContextFactory.DESTINATION_CONTEXT_NDA_FOR_PARENT_RFQ_RESPONSE))
                    {
                        serverPath = BackEndObjects.RFQResponse.
                            getRFQResponseforRFQIdandResponseEntityIdDB
                            (icObj.getParentContextValue(),
                            (icObj.getChildContextObjects()[ActionLibrary.ImageContextFactory.CHILD_CONTEXT_RFQ_RESPONSE_RESPONSE_ENTITY_ID]).ToString()).getNdaPath();

                        dt.Rows.Add();
                        dt.Rows[0]["img"] = serverPath;
                        
                    }

                    break;
            }
            
            return dt;
        }
    }
}