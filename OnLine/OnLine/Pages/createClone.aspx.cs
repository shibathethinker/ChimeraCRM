using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BackEndObjects;
using ActionLibrary;
using System.Collections;
using System.Data;

namespace OnLine.Pages
{
    public partial class createClone : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                TextBox_Name.Text = Request.QueryString.GetValues("contextString")[0];
            }

        }
        /// <summary>
        /// if create potential is true then it will create the potential as well
        /// </summary>
        /// <param name="contextId"></param>
        /// <param name="newrfqId"></param>
        /// <returns></returns>
        protected void createLead(String contextId,String newrfqId,bool createPotn)
        {
            BackEndObjects.RFQDetails rfqObjForLead = BackEndObjects.RFQDetails.getRFQDetailsbyIdDB(contextId);
            BackEndObjects.RFQResponse rfqRespObj = BackEndObjects.RFQResponse.
                getRFQResponseforRFQIdandResponseEntityIdDB(rfqObjForLead.getRFQId(),
                Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

            BackEndObjects.RFQShortlisted potObj = new RFQShortlisted();
            if(createPotn)
            potObj = RFQShortlisted.
                getRFQShortlistedbyRespEntandRFQId(rfqRespObj.getRespEntityId(), rfqRespObj.getRFQId());

            Dictionary<String, RFQResponseQuotes> leadRespDict = BackEndObjects.RFQResponseQuotes.getAllResponseQuotesforRFQandResponseEntityDB(rfqObjForLead.getRFQId(),
                                                        Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

            ArrayList rfqSpecListForLead = BackEndObjects.RFQProductServiceDetails.getAllProductServiceDetailsbyRFQIdDB(rfqObjForLead.getRFQId());
            ArrayList rfqQntyListForLead = BackEndObjects.RFQProdServQnty.getRFQProductServiceQuantityforRFIdDB(rfqObjForLead.getRFQId());

            rfqObjForLead.setRFQName(TextBox_Name.Text);
            rfqObjForLead.setSubmitDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            rfqObjForLead.setRFQId(newrfqId);
            rfqObjForLead.setReqId("");//Remove tagged requirements
            rfqObjForLead.setNDADocPath("");

            for (int i = 0; i < rfqSpecListForLead.Count; i++)
            {
                BackEndObjects.RFQProductServiceDetails rfqSpecObj = (BackEndObjects.RFQProductServiceDetails)rfqSpecListForLead[i];
                rfqSpecObj.setRFQId(rfqObjForLead.getRFQId());
                rfqSpecObj.setImgPath("");
            }
            rfqObjForLead.setRFQProdServList(rfqSpecListForLead);
            for (int i = 0; i < rfqQntyListForLead.Count; i++)
            {
                BackEndObjects.RFQProdServQnty rfqQntyObj = (BackEndObjects.RFQProdServQnty)rfqQntyListForLead[i];
                rfqQntyObj.setRFQId(rfqObjForLead.getRFQId());
            }
            rfqObjForLead.setRFQProdServQntyList(rfqQntyListForLead);

            rfqRespObj.setRespDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            rfqRespObj.setNdaPath("");
            rfqRespObj.setRFQId(rfqObjForLead.getRFQId());

            ArrayList rfqRespQuoteList = new ArrayList();
            foreach (KeyValuePair<String, RFQResponseQuotes> kvp in leadRespDict)
            {
                BackEndObjects.RFQResponseQuotes respQuoteObj = kvp.Value;
                respQuoteObj.setRFQId(rfqObjForLead.getRFQId());
                rfqRespQuoteList.Add(respQuoteObj);
            }
            rfqRespObj.setResponseQuoteList(rfqRespQuoteList);
            
            if (createPotn)
            {
                potObj.setRFQId(rfqObjForLead.getRFQId());
                potObj.setPotentialId(new Id().getNewId(BackEndObjects.Id.ID_TYPE_POTENTIAL_ID_STRING));
                potObj.setCreatedDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                potObj.setConfMatPath("");                
            }

            try
            {
                ActionLibrary.SalesActions._createLeads cL = new ActionLibrary.SalesActions._createLeads();

                LeadRecord leadObj = new LeadRecord();
                leadObj.setRFQProdServList(rfqObjForLead.getRFQProdServList());
                leadObj.setRFQId(rfqObjForLead.getRFQId());
                leadObj.setActiveStat(rfqObjForLead.getActiveStat());
                leadObj.setApprovalStat(rfqObjForLead.getApprovalStat());
                leadObj.setCreatedEntity(rfqObjForLead.getCreatedEntity());
                leadObj.setCreatedUsr(rfqObjForLead.getCreatedUsr());
                leadObj.setCreateMode(rfqObjForLead.getCreateMode());
                leadObj.setDueDate(rfqObjForLead.getDueDate());
                leadObj.setEntityId(rfqObjForLead.getEntityId());

                leadObj.setLocalityId(rfqObjForLead.getLocalityId());

                leadObj.setReqId(rfqObjForLead.getReqId());
                leadObj.setRFQName(rfqObjForLead.getRFQName());
                leadObj.setRFQProdServQntyList(rfqObjForLead.getRFQProdServQntyList());
                leadObj.setSubmitDate(rfqObjForLead.getSubmitDate());
                leadObj.setTermsandConds(rfqObjForLead.getTermsandConds());
                leadObj.setCurrency(rfqObjForLead.getCurrency());

                leadObj.setLeadResp(rfqRespObj);

                cL.createNewLead(leadObj, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), User.Identity.Name);
                if (createPotn)
                    RFQShortlisted.insertRFQShorListedEntryDB(potObj);

                Label_Clone_Stat.Visible = true;
                Label_Clone_Stat.ForeColor = System.Drawing.Color.Green;
                Label_Clone_Stat.Text = "Cloning Successful. But no existing image or document will be copied.";
            }
            catch (Exception ex)
            {
                Label_Clone_Stat.Visible = true;
                Label_Clone_Stat.ForeColor = System.Drawing.Color.Red;
                Label_Clone_Stat.Text = "Cloning Failed";
            }

        }

        /// <summary>
        /// Provided the reponse quote list and the product qunatity list this method will calculate the average potential amount
        /// </summary>
        /// <param name="respQuoteList"></param>
        /// <param name="prodQntyList"></param>
        /// <returns></returns>
        protected float calculatePotAmnt(ArrayList respQuoteList, ArrayList prodQntyList)
        {
            float totalAmnt = 0;
            Dictionary<String, BackEndObjects.RFQProdServQnty> respQuoteDict = new Dictionary<string, RFQProdServQnty>();

            for (int j = 0; j < prodQntyList.Count; j++)
            {
                respQuoteDict.Add(((RFQProdServQnty)prodQntyList[j]).getProdCatId(), (RFQProdServQnty)prodQntyList[j]);
            }

            for (int i = 0; i < respQuoteList.Count; i++)
            {
                BackEndObjects.RFQResponseQuotes respQuoteObj = (RFQResponseQuotes)respQuoteList[i];
                float fromQnty = (respQuoteDict[respQuoteObj.getPrdCatId()]).getFromQnty();
                float toQnty = (respQuoteDict[respQuoteObj.getPrdCatId()]).getToQnty();
                float quoteAmnt = float.Parse(respQuoteObj.getQuote());

                //Now calculate the average
                totalAmnt += (fromQnty * quoteAmnt + toQnty * quoteAmnt) / 2;
            }

            return totalAmnt;
        }

        protected void Button_Clone_Click(object sender, EventArgs e)
        {
            String contextName = Request.QueryString.GetValues("context")[0];
            String contextId=Request.QueryString.GetValues("contextId1")[0];

            switch(contextName)
            {
                case "requirement":
                    BackEndObjects.Requirement reqObj=BackEndObjects.Requirement.getRequirementbyIdDB(contextId);
                    reqObj.setReqName(TextBox_Name.Text);                    
                    reqObj.setReqId(new Id().getNewId(BackEndObjects.Id.ID_TYPE_REQR_STRING));
                    reqObj.setSubmitDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    
                    ArrayList reqSpecLists = reqObj.getReqSpecs();
                    for (int i = 0; i < reqSpecLists.Count; i++)
                    {
                        BackEndObjects.Requirement_Spec reqSpecObj = (BackEndObjects.Requirement_Spec)reqSpecLists[i];
                        reqSpecObj.setReqId(reqObj.getReqId());
                        reqSpecObj.setImgPath("");
                    }
                    reqObj.setReqSpecs(reqSpecLists);

                    ArrayList reqQntyList=reqObj.getReqProdSrvQnty();
                    for(int i=0;i<reqQntyList.Count;i++)
                    {
                        BackEndObjects.RequirementProdServQnty reqQntyObj=(BackEndObjects.RequirementProdServQnty) reqQntyList[i];
                        reqQntyObj.setRequirementId(reqObj.getReqId());
                    }
                    reqObj.setReqProdSrvQnty(reqQntyList);

                    try
                    {
                        BackEndObjects.Requirement.insertRequirementDB(reqObj);
                        Label_Clone_Stat.Visible = true;
                        Label_Clone_Stat.ForeColor = System.Drawing.Color.Green;
                        Label_Clone_Stat.Text = "Cloning Successful. But no existing image or document will be copied.";
                    }
                    catch (Exception ex)
                    {
                        Label_Clone_Stat.Visible = true;
                        Label_Clone_Stat.ForeColor = System.Drawing.Color.Red;
                        Label_Clone_Stat.Text = "Cloning Failed";
                    }
                    break;

                case "rfq":
                    BackEndObjects.RFQDetails rfqObj = BackEndObjects.RFQDetails.getRFQDetailsbyIdDB(contextId);

                    ArrayList rfqSpecList = BackEndObjects.RFQProductServiceDetails.getAllProductServiceDetailsbyRFQIdDB(rfqObj.getRFQId());
                    ArrayList rfqQntyList = BackEndObjects.RFQProdServQnty.getRFQProductServiceQuantityforRFIdDB(rfqObj.getRFQId());

                    rfqObj.setRFQName(TextBox_Name.Text);
                    rfqObj.setSubmitDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    rfqObj.setRFQId(new Id().getNewId(Id.ID_TYPE_RFQ_STRING));
                    rfqObj.setReqId("");//Remove tagged requirements
                    rfqObj.setNDADocPath("");
                    rfqObj.setActiveStat(BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_ACTIVE); //Overwrite the active status to active

                    for (int i = 0; i < rfqSpecList.Count; i++)
                    {
                        BackEndObjects.RFQProductServiceDetails rfqSpecObj = (BackEndObjects.RFQProductServiceDetails)rfqSpecList[i];
                        rfqSpecObj.setRFQId(rfqObj.getRFQId());
                        rfqSpecObj.setImgPath("");
                    }
                    rfqObj.setRFQProdServList(rfqSpecList);
                    for (int i = 0; i < rfqQntyList.Count; i++)
                    {
                        BackEndObjects.RFQProdServQnty rfqQntyObj = (BackEndObjects.RFQProdServQnty)rfqQntyList[i];
                        rfqQntyObj.setRFQId(rfqObj.getRFQId());
                    }
                    rfqObj.setRFQProdServQntyList(rfqQntyList);

                    //Get the approval status
                                    int rfqLevel = BackEndObjects.MainBusinessEntity.
                                        getMainBusinessEntitybyIdwithLessDetailsDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()).getRfqApprovalLevel();
                if (rfqLevel > 0)
                {
                    String reportingToUser = BackEndObjects.userDetails.
                        getUserDetailsbyIdDB(User.Identity.Name, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()).getReportsTo();
                    rfqObj.setApprovalStat(reportingToUser);
                }
                else
                    rfqObj.setApprovalStat(RFQDetails.RFQ_APPROVAL_STAT_APPROVED);


                    try
                    {
                        BackEndObjects.RFQDetails.insertRFQDetailsDB(rfqObj);
                        Label_Clone_Stat.Visible = true;
                        Label_Clone_Stat.ForeColor = System.Drawing.Color.Green;
                        Label_Clone_Stat.Text = "Cloning Successful. But no existing image or document will be copied.Also approvals will be required as rule set in Administration->WorkFlow->RFQ Rules";
                    }
                    catch (Exception ex)
                    {
                        Label_Clone_Stat.Visible = true;
                        Label_Clone_Stat.ForeColor = System.Drawing.Color.Red;
                        Label_Clone_Stat.Text = "Cloning Failed";
                    }
                    break;

                case "lead": createLead(contextId,new Id().getNewId(BackEndObjects.Id.ID_TYPE_RFQ_STRING),false);
                    break;

                case "Potn":
                    createLead(contextId, new Id().getNewId(BackEndObjects.Id.ID_TYPE_RFQ_STRING), true);
                    
                    break;
            }
        }

    }
}