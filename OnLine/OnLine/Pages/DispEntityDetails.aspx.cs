using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BackEndObjects;
using ActionLibrary;

namespace OnLine.Pages
{
    public partial class DispEntityDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                fillCustData();
        }

        protected void fillCustData()
        {
            String[] entId = Request.QueryString.GetValues("entityId");

            Panel_Customer.GroupingText = "Entity Details";

            Dictionary<String, Object> custObj=ActionLibrary.customerDetails.getContactDetails(entId[0], Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                         
            String localId = "";


            if (custObj.ContainsKey(ActionLibrary.customerDetails.RETURN_OBJECT_TYPE_CONTACT_DETAILS))
            {
                BackEndObjects.Contacts cOBJ = (BackEndObjects.Contacts)custObj[ActionLibrary.customerDetails.RETURN_OBJECT_TYPE_CONTACT_DETAILS];
                Label_Cust_Name.Text = cOBJ.getContactName();
                Label_Email.Text = cOBJ.getEmailId();
                Label_Contact_No.Text = cOBJ.getMobNo();
                Label_From_Site.Text = cOBJ.getFromSite();
                localId = cOBJ.getLocalityId();

                //If the contact object is created from the site then get the Main proudct details 
                if (cOBJ.getFromSite().Equals("Y"))
                {
                    Dictionary<String, BackEndObjects.ProductCategory> mainProds = BackEndObjects.MainBusinessEntity.getProductDetailsforMainEntitybyIdDB(cOBJ.getContactEntityId());
                    String mainProdList = "";

                    foreach (KeyValuePair<String, BackEndObjects.ProductCategory> kvp in mainProds)
                        mainProdList = (mainProdList.Equals("") ? ((BackEndObjects.ProductCategory)kvp.Value).getProductCategoryName() : mainProdList +
                            "," + ((BackEndObjects.ProductCategory)kvp.Value).getProductCategoryName());

                    Label_Main_Business.Text = (!mainProdList.Equals("") ? mainProdList : Label_Main_Business.Text);
                }
            }
            else
            {
                BackEndObjects.MainBusinessEntity mBObj = (BackEndObjects.MainBusinessEntity)custObj[ActionLibrary.customerDetails.RETURN_OBJECT_TYPE_MAIN_BUSINESS_ENTITY];
                Label_Cust_Name.Text = mBObj.getEntityName();
                Label_Email.Text = mBObj.getEmailId();
                Label_Contact_No.Text = mBObj.getPhNo();
                Label_From_Site.Text = "Y";

                Dictionary<String, BackEndObjects.ProductCategory> mainProds = mBObj.getMainProductServices();
                //BackEndObjects.MainBusinessEntity.getProductDetailsforMainEntitybyIdDB(mBObj.getEntityId());
                String mainProdList = "";

                if (mainProds != null)
                    foreach (KeyValuePair<String, BackEndObjects.ProductCategory> kvp in mainProds)
                        mainProdList = (mainProdList.Equals("") ? ((BackEndObjects.ProductCategory)kvp.Value).getProductCategoryName() : mainProdList + "," + ((BackEndObjects.ProductCategory)kvp.Value).getProductCategoryName());

                Label_Main_Business.Text = (!mainProdList.Equals("") ? mainProdList : Label_Main_Business.Text);
                if (mBObj.getAddressDetails() != null)
                    localId = mBObj.getAddressDetails().getLocalityId();
            }

            if (localId != null && !localId.Equals(""))
            {
                BackEndObjects.Localities lclObj = BackEndObjects.Localities.getLocalitybyIdDB(localId);
                BackEndObjects.City ctObj = BackEndObjects.Localities.getCityDetailsforLocalitywoOtherAsscLocalitiesDB(localId);
                BackEndObjects.State stObj = BackEndObjects.City.getStateDetailsforCitywoOtherAsscCitiesDB(ctObj.getCityId());
                BackEndObjects.Country cntObj = BackEndObjects.State.getCountryDetailsforStatewoOtherAsscStatesDB(stObj.getStateId());

                Label_Country.Text = cntObj.getCountryName();
                Label_State.Text = stObj.getStateName();
                Label_City.Text = ctObj.getCityName();
                Label_Locality.Text = lclObj.getLocalityName();
            }

        }
    }
}