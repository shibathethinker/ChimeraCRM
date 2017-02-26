using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BackEndObjects;
using ActionLibrary;

namespace OnLine.Pages.Popups.Purchase
{
    public partial class AllRequirement_Location : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];
                if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS] ||
                    accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_REQUIREMENT])
                {
                    Button_Update.Enabled = true;
                    loadCountry();
                }
                else
                {
                    Button_Update.Enabled = false;
                    Label_Status.Visible = true;
                    Label_Status.Text = "You dont have edit access to Requirement records";
                    Label_Status.ForeColor = System.Drawing.Color.Red;
                }

                String localId=Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_LOCATION].ToString();
                loadLocalityDetails(localId);
            }
        }

        protected void loadLocalityDetails(String localId)
        {
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

        protected void loadCountry()
        {
            Dictionary<String, Country> countryDict = BackEndObjects.Country.getAllCountrywoStatesDB();
            ListItem ltCountry1 = new ListItem();
            ltCountry1.Text = " ";
            ltCountry1.Value = "none";
            DropDownList_Country.Items.Add(ltCountry1);

            foreach (KeyValuePair<String, Country> kvp in countryDict)
            {
                ListItem ltCountry = new ListItem();
                ltCountry.Text = ((Country)kvp.Value).getCountryName();
                ltCountry.Value = ((Country)kvp.Value).getCountryId();

                DropDownList_Country.Items.Add(ltCountry);
            }
            DropDownList_Country.SelectedIndex = -1;
        }

        protected void DropDownList_Country_SelectedIndexChanged(object sender, EventArgs e)
        {
            String itemId = DropDownList_Country.SelectedValue.ToString();
            Dictionary<String, BackEndObjects.State> stateList = new Dictionary<string, BackEndObjects.State>();
            DropDownList_State.Items.Clear();

            stateList = BackEndObjects.State.getStatesforCountrywoCitiesDB(itemId.Trim());

            ListItem ltEmpty = new ListItem();
            ltEmpty.Text = "";
            ltEmpty.Value = "";
            DropDownList_State.Items.Add(ltEmpty);

            foreach (KeyValuePair<String, BackEndObjects.State> kvp in stateList)
            {
                ListItem ltState = new ListItem();
                ltState.Value = ((BackEndObjects.State)kvp.Value).getStateId();
                ltState.Text = ((BackEndObjects.State)kvp.Value).getStateName();
                DropDownList_State.Items.Add(ltState);
            }
            if (DropDownList_State.Items.Count > 0)
                DropDownList_State.SelectedValue = "";
        }

        protected void DropDownList_State_SelectedIndexChanged(object sender, EventArgs e)
        {
            String itemId = DropDownList_State.SelectedValue.ToString();
            Dictionary<String, BackEndObjects.City> cityList = new Dictionary<string, BackEndObjects.City>();
            DropDownList_City.Items.Clear();

            cityList = BackEndObjects.City.getCitiesforStatewoLocalitiesDB(itemId.Trim());

            ListItem ltEmpty = new ListItem();
            ltEmpty.Text = "";
            ltEmpty.Value = "";
            DropDownList_City.Items.Add(ltEmpty);

            foreach (KeyValuePair<String, BackEndObjects.City> kvp in cityList)
            {
                ListItem ltCity = new ListItem();
                ltCity.Value = ((BackEndObjects.City)kvp.Value).getCityId();
                ltCity.Text = ((BackEndObjects.City)kvp.Value).getCityName();
                DropDownList_City.Items.Add(ltCity);
            }
            if (DropDownList_City.Items.Count > 0)
                DropDownList_City.SelectedValue = "";
        }

        protected void DropDownList_City_SelectedIndexChanged(object sender, EventArgs e)
        {
            String itemId = DropDownList_City.SelectedValue.ToString();
            Dictionary<String, BackEndObjects.Localities> localList = new Dictionary<string, BackEndObjects.Localities>();
            DropDownList_Locality.Items.Clear();

            localList = BackEndObjects.Localities.getLocalitiesforCityDB(itemId.Trim());
            ListItem ltEmpty = new ListItem();
            ltEmpty.Text = "";
            ltEmpty.Value = "";
            DropDownList_Locality.Items.Add(ltEmpty);


            foreach (KeyValuePair<String, BackEndObjects.Localities> kvp in localList)
            {
                ListItem ltLocal = new ListItem();
                ltLocal.Value = ((BackEndObjects.Localities)kvp.Value).getLocalityId();
                ltLocal.Text = ((BackEndObjects.Localities)kvp.Value).getLocalityName();
                DropDownList_Locality.Items.Add(ltLocal);
            }
            if (DropDownList_Locality.Items.Count > 0)
                DropDownList_Locality.SelectedValue = "";


        }

        protected void Button_Update_Click(object sender, EventArgs e)
        {
            String reqId = Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_SELECTED_REQR_ID].ToString();

            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            Dictionary<String, String> targetVals = new Dictionary<string, string>();

            whereCls.Add(BackEndObjects.Requirement.REQ_COL_REQ_ID, reqId);
            String localId = !DropDownList_Locality.SelectedValue.Equals("") ? DropDownList_Locality.SelectedValue :
    !DropDownList_City.SelectedValue.Equals("") ? DropDownList_City.SelectedValue :
    !DropDownList_State.SelectedValue.Equals("") ? DropDownList_State.SelectedValue : DropDownList_Country.SelectedValue;

            targetVals.Add(BackEndObjects.Requirement.REQ_COL_LOCAL_ID, localId);

            try
            {
                BackEndObjects.Requirement.updateRequirementDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
                Label_Status.Visible = true;
                Label_Status.Text = "Update Successful";
                Label_Status.ForeColor = System.Drawing.Color.Green;
                Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_LOCATION] = localId;
                loadLocalityDetails(localId);
            }
            catch (Exception ex)
            {
                Label_Status.Visible = true;
                Label_Status.Text = "Update Failed";
                Label_Status.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}