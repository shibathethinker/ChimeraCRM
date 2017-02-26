using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BackEndObjects;
using System.Collections;
using ActionLibrary;
using System.Data;

namespace OnLine.Pages
{
    public partial class createContact : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                loadCountry();
                loadProdServList();
                loadExistingContacts();
            }
        }

        protected void loadExistingContacts()
        {

            ArrayList contactList = BackEndObjects.Contacts.getAllContactsbyEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

            Dictionary<String, String> existingContactNames = new Dictionary<String, String>();
            Dictionary<String, String> existingContactShortNames = new Dictionary<String, String>();
            Dictionary<String, String> existingContactEmailIds = new Dictionary<String, String>();
            Dictionary<String, String> existingContactPhNos = new Dictionary<String, String>();

            for (int i = 0; i < contactList.Count; i++)
            {
                //Short Name - MobNo - Email Id
                if (!existingContactNames.ContainsKey(((BackEndObjects.Contacts)contactList[i]).getContactName()) && 
                    !((BackEndObjects.Contacts)contactList[i]).getContactName().Equals(""))
                    existingContactNames.Add(((BackEndObjects.Contacts)contactList[i]).getContactName(), "");

                if (!existingContactShortNames.ContainsKey(((BackEndObjects.Contacts)contactList[i]).getContactShortName()) &&
                    !((BackEndObjects.Contacts)contactList[i]).getContactShortName().Equals(""))
                    existingContactShortNames.Add(((BackEndObjects.Contacts)contactList[i]).getContactShortName(), "");

                if (!existingContactEmailIds.ContainsKey(((BackEndObjects.Contacts)contactList[i]).getEmailId()) &&
                    !((BackEndObjects.Contacts)contactList[i]).getEmailId().Equals(""))
                    existingContactEmailIds.Add(((BackEndObjects.Contacts)contactList[i]).getEmailId(), "");

                if (!existingContactPhNos.ContainsKey(((BackEndObjects.Contacts)contactList[i]).getMobNo()) &&
                    !((BackEndObjects.Contacts)contactList[i]).getMobNo().Equals(""))
                    existingContactPhNos.Add(((BackEndObjects.Contacts)contactList[i]).getMobNo(), "");
            }

            Dictionary<String, Dictionary<String, String>> existingContactDict = new Dictionary<string, Dictionary<string, string>>();
            existingContactDict.Add("names", existingContactNames);
            existingContactDict.Add("shortnames", existingContactShortNames);
            existingContactDict.Add("emailids", existingContactEmailIds);
            existingContactDict.Add("phonenos", existingContactPhNos);

            Session[SessionFactory.ALL_CONTACT_EXISTING_CONTACT_LIST] = existingContactDict;

        }
        protected void loadCountry()
        {
            Dictionary<String, Country> countryDict = BackEndObjects.Country.getAllCountrywoStatesDB();
            ListItem ltCountry1 = new ListItem();
            ltCountry1.Text = "";
            ltCountry1.Value = "";
            DropDownList_Country.Items.Add(ltCountry1);

            foreach (KeyValuePair<String, Country> kvp in countryDict)
            {
                ListItem ltCountry = new ListItem();
                ltCountry.Text = ((Country)kvp.Value).getCountryName();
                ltCountry.Value = ((Country)kvp.Value).getCountryId();

                DropDownList_Country.Items.Add(ltCountry);
            }
            DropDownList_Country.SelectedValue = "";
        }

        protected void loadProdServList()
        {
            Dictionary<String, ProductCategory> prodCatList = BackEndObjects.ProductCategory.getAllParentCategory();

            foreach (KeyValuePair<String, ProductCategory> kvp in prodCatList)
            {
                ListItem ltProdCat = new ListItem();
                ltProdCat.Text = ((ProductCategory)kvp.Value).getProductCategoryName();
                ltProdCat.Value = ((ProductCategory)kvp.Value).getCategoryId();
                ListBoxProdServc.Items.Add(ltProdCat);
            }
            ListBoxProdServc.SelectedIndex = -1;
        }
        protected void Buttin_Show_Spec_List_Click(object sender, EventArgs e)
        {
            fillGrid();
            Button_Unselect.Visible = true;
        }

        protected void fillGrid()
        {

                ArrayList mBEList = BackEndObjects.MainBusinessEntity.getMainBusinessEntityListbyNameWithAddrDetailsDB(TextBox_Search_Contact.Text,
                   Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

                if (mBEList == null || mBEList.Count == 0)
                {
                    Label_Status_Search.Visible = true;
                    Label_Status_Search.ForeColor = System.Drawing.Color.Red;
                    Label_Status_Search.Text = "No company found for the given id";
                    GridView1.Visible = false;
                }
                    
                else
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Name");
                    dt.Columns.Add("Country");
                    dt.Columns.Add("State");
                    dt.Columns.Add("City");
                    dt.Columns.Add("Locality");
                    dt.Columns.Add("Street Name");
                    dt.Columns.Add("Mob");
                    dt.Columns.Add("Email");
                    dt.Columns.Add("Contact Exists?");
                    dt.Columns.Add("localId");
                    dt.Columns.Add("ContactEntId");
                    
                    for (int i = 0; i < mBEList.Count; i++)
                    {

                        //MainBusinessEntity mBEObj = BackEndObjects.MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(TextBox_Search_Contact.Text);
                        MainBusinessEntity mBEObj = (MainBusinessEntity)mBEList[i];
                        AddressDetails mBEAddr = mBEObj.getAddressDetails();

                        Contacts contactObj = Contacts.
                            getContactDetailsforContactEntityDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), mBEObj.getEntityId());
                        
                        Label_Status_Search.Visible = false;

                        BackEndObjects.Localities localObj = null;
                        BackEndObjects.City cityObj = null;
                        BackEndObjects.State stateObj = null;
                        BackEndObjects.Country countryObj = null;

                        if (mBEAddr.getLocalityId() != null && !mBEAddr.getLocalityId().Equals(""))
                        {
                            localObj = Localities.getLocalitybyIdDB(mBEAddr.getLocalityId());
                            cityObj = BackEndObjects.Localities.getCityDetailsforLocalitywoOtherAsscLocalitiesDB(mBEAddr.getLocalityId());
                            stateObj = BackEndObjects.City.getStateDetailsforCitywoOtherAsscCitiesDB(cityObj.getCityId());
                            countryObj = BackEndObjects.State.getCountryDetailsforStatewoOtherAsscStatesDB(stateObj.getStateId());
                            //Session[SessionFactory.CREATE_CONTACT_LOCALITY_ID] = localObj.getLocalityId();
                            //Session[SessionFactory.CREATE_CONTACT_ADDRESS_LINE1] = mBEAddr.getAddrLine1();
                        }
                                   

                        dt.Rows.Add();

                        dt.Rows[i]["Name"] = mBEObj.getEntityName();
                        if (mBEAddr.getLocalityId() != null && !mBEAddr.getLocalityId().Equals(""))
                        {
                            dt.Rows[i]["Country"] = countryObj.getCountryName();
                            dt.Rows[i]["State"] = stateObj.getStateName();
                            dt.Rows[i]["City"] = cityObj.getCityName();
                            dt.Rows[i]["Locality"] = localObj.getLocalityName();
                            dt.Rows[i]["Street Name"] = mBEAddr.getAddrLine1();
                            dt.Rows[i]["localId"] = mBEAddr.getLocalityId();
                            dt.Rows[i]["ContactEntId"] = mBEAddr.getMainBusinessId();
                        }
                        else
                        {
                            dt.Rows[i]["Country"] = "N/A";
                            dt.Rows[i]["State"] = "N/A";
                            dt.Rows[i]["City"] = "N/A";
                            dt.Rows[i]["Locality"] = "N/A";
                            dt.Rows[i]["Street Name"] = "N/A";
                            dt.Rows[i]["localId"] = "";
                            dt.Rows[i]["ContactEntId"] = mBEObj.getEntityId();
                        }
                        dt.Rows[i]["Mob"] = mBEObj.getPhNo();
                        dt.Rows[i]["Email"] = mBEObj.getEmailId();
                        dt.Rows[i]["Contact Exists?"] = ((contactObj.getContactEntityId() != null && !contactObj.getContactEntityId().Equals("")) ? "Y" : "N");
                    }
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    GridView1.Visible = true;
                    GridView1.Columns[10].Visible = false;
                    GridView1.Columns[11].Visible = false;


                    Session[SessionFactory.CREATE_CONTACT_DATA_GRID] = dt;
                }
            

        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            BackEndObjects.Contacts contactObj = new BackEndObjects.Contacts();
            Panel_Create_Contact.Enabled = false;
            Label_Disable.Visible = true;

            contactObj.setContactEntityId(((Label)GridView1.SelectedRow.Cells[0].FindControl("Label_Contact_Ent_Id")).Text);
            //contactObj.setContactName(((Label)GridView1.SelectedRow.Cells[1].FindControl("Label_Name")).Text);
            //contactObj.setEmailID(((Label)GridView1.SelectedRow.Cells[1].FindControl("Label_Email")).Text);
            contactObj.setContactName(((Label)GridView1.SelectedRow.Cells[0].FindControl("Label_Name")).Text);
            contactObj.setEmailID(((Label)GridView1.SelectedRow.Cells[0].FindControl("Label_Email_Id")).Text);
            contactObj.setEntityId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            contactObj.setFromSite("Y");

            //if (Session[SessionFactory.CREATE_CONTACT_LOCALITY_ID]!=null)
            if (((Label)GridView1.SelectedRow.Cells[0].FindControl("Label_Local_Id")).Text != null)
                contactObj.setLocalityId(((Label)GridView1.SelectedRow.Cells[0].FindControl("Label_Local_Id")).Text);

            contactObj.setMobNo(((Label)GridView1.SelectedRow.Cells[0].FindControl("Label_Contact_No")).Text);

            if (((Label)GridView1.SelectedRow.Cells[0].FindControl("Label_Street_Name")).Text != null)
                contactObj.setStreetName(((Label)GridView1.SelectedRow.Cells[0].FindControl("Label_Street_Name")).Text);

            String prodList = "";
            //Get the product/service details
            Dictionary<String,ProductCategory> prodDict= MainBusinessEntity.getProductDetailsforMainEntitybyIdDB(contactObj.getContactEntityId());
            foreach (KeyValuePair<String, ProductCategory> kvp in prodDict)
                prodList += kvp.Value.getCategoryId() + ",";
            
            if(prodList.Length>0)
                prodList=prodList.TrimEnd(',');

            contactObj.setProdList(prodList);

            Session[SessionFactory.CREATE_CONTACT_CONTACT_OBJ] = contactObj;
                
        }

        protected void DropDownList_Country_SelectedIndexChanged(object sender, EventArgs e)
        {
            String itemId = DropDownList_Country.SelectedValue.ToString();
            Dictionary<String, BackEndObjects.State> stateList = new Dictionary<string, BackEndObjects.State>();
            DropDownList_State.Items.Clear();

            stateList = BackEndObjects.State.getStatesforCountrywoCitiesDB(itemId.Trim());
            ListItem ltFirst = new ListItem();
            ltFirst.Text = "_";
            ltFirst.Value = "_";
            DropDownList_State.Items.Add(ltFirst);

            foreach (KeyValuePair<String, BackEndObjects.State> kvp in stateList)
            {
                ListItem ltState = new ListItem();
                ltState.Value = ((BackEndObjects.State)kvp.Value).getStateId();
                ltState.Text = ((BackEndObjects.State)kvp.Value).getStateName();
                DropDownList_State.Items.Add(ltState);
            }
            if (DropDownList_State.Items.Count > 0)
                DropDownList_State.SelectedValue = "_";
        }

        /// <summary>
        /// A change in State selection should populate the City list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DropDownList_State_SelectedIndexChanged(object sender, EventArgs e)
        {
            String itemId = DropDownList_State.SelectedValue.ToString();
            Dictionary<String, BackEndObjects.City> cityList = new Dictionary<string, BackEndObjects.City>();
            DropDownList_City.Items.Clear();

            cityList = BackEndObjects.City.getCitiesforStatewoLocalitiesDB(itemId.Trim());

            foreach (KeyValuePair<String, BackEndObjects.City> kvp in cityList)
            {
                ListItem ltCity = new ListItem();
                ltCity.Value = ((BackEndObjects.City)kvp.Value).getCityId();
                ltCity.Text = ((BackEndObjects.City)kvp.Value).getCityName();
                DropDownList_City.Items.Add(ltCity);
            }
            ListItem ltFirst = new ListItem();
            ltFirst.Text = "";
            ltFirst.Value = "";
            DropDownList_City.Items.Add(ltFirst);

            if (DropDownList_City.Items.Count > 0)
                DropDownList_City.SelectedValue = "";
        }
        /// <summary>
        /// A change in City selection should populate the locality list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DropDownList_City_SelectedIndexChanged(object sender, EventArgs e)
        {
            String itemId = DropDownList_City.SelectedValue.ToString();
            Dictionary<String, BackEndObjects.Localities> localList = new Dictionary<string, BackEndObjects.Localities>();
            DropDownList_Locality.Items.Clear();

            localList = BackEndObjects.Localities.getLocalitiesforCityDB(itemId.Trim());

            foreach (KeyValuePair<String, BackEndObjects.Localities> kvp in localList)
            {
                ListItem ltLocal = new ListItem();
                ltLocal.Value = ((BackEndObjects.Localities)kvp.Value).getLocalityId();
                ltLocal.Text = ((BackEndObjects.Localities)kvp.Value).getLocalityName();
                DropDownList_Locality.Items.Add(ltLocal);
            }
            ListItem ltFirst = new ListItem();
            ltFirst.Text = "";
            ltFirst.Value = "";
            DropDownList_Locality.Items.Add(ltFirst);

            if (DropDownList_Locality.Items.Count > 0)
                DropDownList_Locality.SelectedValue = "";

        }

        protected void Buttin_Submit_Click(object sender, EventArgs e)
        {
            BackEndObjects.Contacts contactObj = (Contacts)Session[SessionFactory.CREATE_CONTACT_CONTACT_OBJ];
            if (contactObj == null && TextBox_Contact_Name.Text.Equals(""))
            {
                Label_Status.Text = "Please select contact from the site or enter the details";
                Label_Status.ForeColor = System.Drawing.Color.Red;
                Label_Status.Visible = true;
                Buttin_Submit.Focus();
            }
            else
            {
                if (contactObj == null)
                {
                    contactObj = new BackEndObjects.Contacts();
                    BackEndObjects.Id IdGen = new Id();

                    int[] prodServ = ListBoxProdServc.GetSelectedIndices();
                    String prodList = "";

                    for (int i = 0; i < prodServ.Length; i++)
                    {
                        if (i < (prodServ.Length - 1))
                            prodList += ListBoxProdServc.Items[prodServ[i]].Value + ",";
                        else
                            prodList += ListBoxProdServc.Items[prodServ[i]].Value;
                    }

                    contactObj.setContactEntityId(IdGen.getNewId(BackEndObjects.Id.ID_TYPE_CMP_USR_STRING));
                    contactObj.setContactName(TextBox_Contact_Name.Text);
                    contactObj.setContactShortName(TextBox_Contact_ShortName.Text);
                    contactObj.setEmailID(TextBox_EmailId.Text);
                    contactObj.setEntityId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                    contactObj.setFromSite("N");
                    String localId = (!DropDownList_Locality.SelectedValue.Equals("_") && !DropDownList_Locality.SelectedValue.Equals("") ?
    DropDownList_Locality.SelectedValue : (!DropDownList_City.SelectedValue.Equals("_") && !DropDownList_City.SelectedValue.Equals("") ?
    DropDownList_City.SelectedValue : (!DropDownList_State.SelectedValue.Equals("_") && !DropDownList_State.SelectedValue.Equals("") ?
    DropDownList_State.SelectedValue : (!DropDownList_Country.SelectedValue.Equals("_") && !DropDownList_Country.SelectedValue.Equals("") ?
    DropDownList_Country.SelectedValue : ""))));

                    contactObj.setLocalityId(localId);
                    contactObj.setMobNo(TextBox_Contact_No.Text);
                    contactObj.setStreetName(TextBox_Street_Name.Text);
                    contactObj.setProdList(prodList);
                }
                else
                {
                    contactObj = (BackEndObjects.Contacts)Session[SessionFactory.CREATE_CONTACT_CONTACT_OBJ];
                    contactObj.setContactShortName(TextBox_Contact_ShortName.Text);
                    Session.Remove(SessionFactory.CREATE_CONTACT_ADDRESS_LINE1);
                    Session.Remove(SessionFactory.CREATE_CONTACT_CONTACT_OBJ);
                    Session.Remove(SessionFactory.CREATE_CONTACT_LOCALITY_ID);

                }

                try
                {
                    bool found = false;

                    Dictionary<String, Dictionary<String, String>> existingContactDict = (Dictionary<String, Dictionary<String, String>>)Session[SessionFactory.ALL_CONTACT_EXISTING_CONTACT_LIST];
                    if (existingContactDict != null && existingContactDict.Count > 0)
                    {
                        Dictionary<String, String> existingContactNames = existingContactDict["names"];
                        Dictionary<String, String> existingContactShortNames = existingContactDict["shortnames"];
                        Dictionary<String, String> existingContactEmailIds = existingContactDict["emailids"];
                        Dictionary<String, String> existingContactPhNos = existingContactDict["phonenos"];

                        if (existingContactShortNames != null && existingContactShortNames.ContainsKey(contactObj.getContactShortName()))
                            found = true;
                        else if (existingContactNames != null && existingContactNames.ContainsKey(contactObj.getContactName()))
                            found = true;
                        else if (existingContactEmailIds != null && existingContactEmailIds.ContainsKey(contactObj.getEmailId()))
                            found = true;
                        else if (existingContactPhNos != null && existingContactPhNos.ContainsKey(contactObj.getMobNo()))
                            found = true;
                    }

                    if (found)
                    {
                        Label_Status.Text = "Contact Exists with similar details. Please recheck";
                        Label_Status.ForeColor = System.Drawing.Color.Red;
                        Label_Status.Visible = true;
                        Buttin_Submit.Focus();
                    }
                    else
                    {
                        BackEndObjects.Contacts.insertContactDetailsDB(contactObj);
                        Label_Status.Text = "Contact created successfully";
                        Label_Status.ForeColor = System.Drawing.Color.Green;
                        Label_Status.Visible = true;
                        Buttin_Submit.Focus();
                        Dictionary<String, String> allExistingContacts = (Dictionary<String, String>)Session[SessionFactory.EXISTING_CONTACT_DICTIONARY];
                        if(!allExistingContacts.ContainsKey(contactObj.getContactName()))
                            allExistingContacts.Add(contactObj.getContactName(),contactObj.getContactEntityId());
                        Session[SessionFactory.EXISTING_CONTACT_DICTIONARY] = allExistingContacts;

                        if (existingContactDict != null && existingContactDict.Count > 0)
                        {
                            Dictionary<String, String> existingContactNames = existingContactDict["names"];
                            Dictionary<String, String> existingContactShortNames = existingContactDict["shortnames"];
                            Dictionary<String, String> existingContactEmailIds = existingContactDict["emailids"];
                            Dictionary<String, String> existingContactPhNos = existingContactDict["phonenos"];

                            if (existingContactShortNames != null && !existingContactShortNames.ContainsKey(contactObj.getContactShortName()) && !contactObj.getContactShortName().Equals(""))
                                existingContactShortNames.Add(contactObj.getContactShortName(), "");
                            if (existingContactNames != null && !existingContactNames.ContainsKey(contactObj.getContactName()) && !contactObj.getContactName().Equals(""))
                                existingContactNames.Add(contactObj.getContactName(), "");
                            if (existingContactEmailIds != null && !existingContactEmailIds.ContainsKey(contactObj.getEmailId()) && !contactObj.getEmailId().Equals(""))
                                existingContactEmailIds.Add(contactObj.getEmailId(), "");
                            if (existingContactPhNos != null && !existingContactPhNos.ContainsKey(contactObj.getMobNo()) && !contactObj.getMobNo().Equals(""))
                                existingContactPhNos.Add(contactObj.getMobNo(), "");

                            existingContactDict.Clear();
                            existingContactDict.Add("names", existingContactNames);
                            existingContactDict.Add("shortnames", existingContactShortNames);
                            existingContactDict.Add("emailids", existingContactEmailIds);
                            existingContactDict.Add("phonenos", existingContactPhNos);
                            Session[SessionFactory.ALL_CONTACT_EXISTING_CONTACT_LIST] = existingContactDict;
                        }

                      
                        if (Request.QueryString.GetValues("parentContext") != null)
                        {
                            String parentContext = Request.QueryString.GetValues("parentContext")[0];
                            if (parentContext.Equals("lead") || parentContext.Equals("potn") || parentContext.Equals("rfq") || parentContext.Equals("broadcast") || parentContext.Equals("defect") || parentContext.Equals("sr"))
                                ScriptManager.RegisterStartupScript(this, typeof(string), "DispContactLead", "RefreshParent();", true);
                        }
                        else
                        {
                            DataTable dt = (DataTable)Session[SessionFactory.ALL_CONTACT_DATA_GRID];
                            dt.Rows.Add();

                            int i = dt.Rows.Count - 1;

                            dt.Rows[i]["ContactEntId"] = contactObj.getContactEntityId();
                            dt.Rows[i]["ContactName"] = contactObj.getContactName();
                            dt.Rows[i]["ShortName"] = contactObj.getContactShortName();
                            dt.Rows[i]["PhNo"] = contactObj.getMobNo();
                            dt.Rows[i]["EmailId"] = contactObj.getEmailId();
                            dt.Rows[i]["FromSite"] = contactObj.getFromSite();

                            dt.DefaultView.Sort = "ContactName" + " " + "ASC";
                            Session[SessionFactory.ALL_CONTACT_DATA_GRID] = dt.DefaultView.ToTable();

                            ScriptManager.RegisterStartupScript(this, typeof(string), "updateContactGrid", "RefreshParentPostCreation();", true);
                        }
                    }
                    /*ListItem lt = new ListItem();
                    lt.Text = (contactObj.getContactShortName() != null && !contactObj.getContactShortName().Equals("") ? contactObj.getContactShortName() :
                        contactObj.getContactName());
                    lt.Value = contactObj.getContactEntityId();

                    DropDownList prevDropList = PreviousPage.FindControl("DropDownList_Contacts") as DropDownList;
                    ((DropDownList)Page.PreviousPage.FindControl("DropDownList_Contacts")).Items.Add(lt);*/
                }
                catch (Exception ex)
                {
                    Label_Status.Text = "Contact creation failed";
                    Label_Status.ForeColor = System.Drawing.Color.Red;
                    Label_Status.Visible = true;
                    Buttin_Submit.Focus();
                }
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataSource = (DataTable)Session[SessionFactory.CREATE_CONTACT_DATA_GRID];
            GridView1.DataBind();
            
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void GridView1_RowCreated(object sender, GridViewRowEventArgs e)
        {

        }

        protected void Button_Unselect_Click(object sender, EventArgs e)
        {
            GridView1.SelectedIndex = -1;
            Panel_Create_Contact.Enabled = true;
            Label_Disable.Visible = false;
        }


    }
}