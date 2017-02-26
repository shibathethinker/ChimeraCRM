using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BackEndObjects;
using ActionLibrary;

namespace OnLine.Pages.Popups.Sale
{
    public partial class AllLead_Customer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                fillCustData();
        }

        protected void fillCustData()
        {
            Dictionary<String, Object> custObj = (Dictionary<String,Object>)Session[SessionFactory.ALL_SALE_ALL_LEAD_SELECTED_CUSTOMER_OBJ];
             String localId = "";


            if(custObj.ContainsKey(ActionLibrary.customerDetails.RETURN_OBJECT_TYPE_CONTACT_DETAILS))
            {
                BackEndObjects.Contacts cOBJ=(BackEndObjects.Contacts)custObj[ActionLibrary.customerDetails.RETURN_OBJECT_TYPE_CONTACT_DETAILS];
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
                else
                {
                    String[] prodCatArray = cOBJ.getProdList().Split(new String[]{","}, StringSplitOptions.RemoveEmptyEntries);
                    String mainProdList = "";

                    for (int i = 0; i < prodCatArray.Length; i++)
                        mainProdList = (mainProdList.Equals("") ? BackEndObjects.ProductCategory.getProductCategorybyIdwoFeaturesDB(prodCatArray[i]).getProductCategoryName() : mainProdList +
    "," + BackEndObjects.ProductCategory.getProductCategorybyIdwoFeaturesDB(prodCatArray[i]).getProductCategoryName());
                    
                    Label_Main_Business.Text = (!mainProdList.Equals("") ? mainProdList : Label_Main_Business.Text);
                }
            }
            else
            {
                BackEndObjects.MainBusinessEntity mBObj=(BackEndObjects.MainBusinessEntity)custObj[ActionLibrary.customerDetails.RETURN_OBJECT_TYPE_MAIN_BUSINESS_ENTITY];
                Label_Cust_Name.Text = mBObj.getEntityName();
                Label_Email.Text = mBObj.getEmailId();
                Label_Contact_No.Text = mBObj.getPhNo();
                Label_From_Site.Text = "Y";

                Dictionary<String, BackEndObjects.ProductCategory> mainProds = mBObj.getMainProductServices();
                    //BackEndObjects.MainBusinessEntity.getProductDetailsforMainEntitybyIdDB(mBObj.getEntityId());
                String mainProdList = "";

                if (mainProds != null)
                foreach (KeyValuePair<String, BackEndObjects.ProductCategory> kvp in mainProds)
                    mainProdList = (mainProdList.Equals("") ? ((BackEndObjects.ProductCategory)kvp.Value).getProductCategoryName() : mainProdList +    "," + ((BackEndObjects.ProductCategory)kvp.Value).getProductCategoryName());

                Label_Main_Business.Text = (!mainProdList.Equals("") ? mainProdList : Label_Main_Business.Text);
                if (mBObj.getAddressDetails() != null)
                localId = mBObj.getAddressDetails().getLocalityId();
            }

            if (localId != null && !localId.Equals(""))
            {
                BackEndObjects.Localities lclObj = BackEndObjects.Localities.getLocalitybyIdDB(localId);

                BackEndObjects.City ctObj = BackEndObjects.Localities.getCityDetailsforLocalitywoOtherAsscLocalitiesDB(localId);

                String cityId = (ctObj != null && ctObj.getCityId() != null && !ctObj.getCityId().Equals("") ? ctObj.getCityId() : localId);
                BackEndObjects.State stObj = BackEndObjects.City.getStateDetailsforCitywoOtherAsscCitiesDB(cityId);
                if (cityId.Equals(localId))
                    ctObj = BackEndObjects.City.getCitybyIdwoLocalitiesDB(localId);

                String stateId = (stObj != null && stObj.getStateId() != null && !stObj.getStateId().Equals("") ? stObj.getStateId() : localId);
                BackEndObjects.Country cntObj = BackEndObjects.State.getCountryDetailsforStatewoOtherAsscStatesDB(stateId);
                if (stateId.Equals(localId))
                    stObj = BackEndObjects.State.getStatebyIdwoCitiesDB(stateId);

                if (cntObj != null && cntObj.getCountryName() != null && !cntObj.getCountryName().Equals(""))
                    Label_Country.Text = cntObj.getCountryName();

                if (stObj != null && stObj.getStateName() != null && !stObj.getStateName().Equals(""))
                    Label_State.Text = stObj.getStateName();

                if (ctObj != null && ctObj.getCityName() != null && !ctObj.getCityName().Equals(""))
                    Label_City.Text = ctObj.getCityName();

                if (lclObj != null && lclObj.getLocalityName() != null && !lclObj.getLocalityName().Equals(""))
                    Label_Locality.Text = lclObj.getLocalityName();
            }

        }
    }
}