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
using System.Security.Cryptography;

namespace OnLine.Pages.Popups.AdminPref
{
    public partial class ChainMgmt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                populateGrid();
                loadCountryNames();
                loadCurrency();
                loadSubBusinessEntity();
            }
            else if (!TextBox2.Text.Equals(""))
            {
                TextBox2.Attributes["password"] = TextBox2.Text;
            }


        }

        protected void populateGrid()
        {
            Dictionary<String, subBusinessEntity> subEntDict = BackEndObjects.MainBusinessEntity.
                getSubEntitiesforMainEntitybyIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

            DataTable dt = new DataTable();
            dt.Columns.Add("ChainName");
            dt.Columns.Add("ContactNo");
            dt.Columns.Add("ContactName");
            dt.Columns.Add("EmailId");
            dt.Columns.Add("WebSite");
            dt.Columns.Add("RegstrNo");
            dt.Columns.Add("BaseCurr");
            dt.Columns.Add("ChainId");

            int counter = 0;

            foreach (KeyValuePair<String, subBusinessEntity> kvp in subEntDict)
            {
                subBusinessEntity subEntObj = (subBusinessEntity)kvp.Value;
                dt.Rows.Add();

                dt.Rows[counter]["ChainName"] = subEntObj.getSubEntityName();
                dt.Rows[counter]["ContactNo"] = subEntObj.getSubPhNo();
                dt.Rows[counter]["ContactName"] = subEntObj.getContactName();
                dt.Rows[counter]["EmailId"] = subEntObj.getSubEmailId();
                dt.Rows[counter]["WebSite"] = subEntObj.getSubWebSite();
                dt.Rows[counter]["RegstrNo"] = subEntObj.getSubRegstrNo();
                dt.Rows[counter]["BaseCurr"] = BackEndObjects.Currency.getCurrencybyIdDB(subEntObj.getBaseCurrencyId()).getCurrencyName();
                dt.Rows[counter]["ChainId"] = subEntObj.getSubEntityId();
                //dt.Rows[counter]["dlList"] = dl;
                counter++;
            }

            GridView1.DataSource = dt;
            GridView1.DataBind();
            GridView1.Columns[10].Visible = false;
        

            Session[SessionFactory.ADMIN_PREF_CHAIN_MGMT_DATA_GRID] = dt;
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GridView1.DataSource = (DataTable)Session[SessionFactory.ADMIN_PREF_CHAIN_MGMT_DATA_GRID];
            GridView1.DataBind();
            GridView1.Columns[10].Visible = false;

            Dictionary<String, Currency> currDict = BackEndObjects.Currency.getAllCurrencyDetailsDB();
            //DropDownList dl = new DropDownList();
            
            //dl.SelectedIndex = -1;

            foreach (GridViewRow gVR in GridView1.Rows)
            {
                DropDownList currList = (DropDownList)gVR.Cells[9].FindControl("DropDownList_BaseCurr");
                foreach (KeyValuePair<String, Currency> kvp in currDict)
                {
                    ListItem lt = new ListItem();
                    lt.Text = ((Currency)kvp.Value).getCurrencyName();
                    lt.Value = ((Currency)kvp.Value).getCurrencyId();

                    currList.Items.Add(lt);
                }
                currList.SelectedIndex = -1;
                //currList = dl;
            }
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DataTable dt = (DataTable)Session[SessionFactory.ADMIN_PREF_CHAIN_MGMT_DATA_GRID];
            GridViewRow gVR = (GridViewRow)GridView1.Rows[e.RowIndex];

            String chainName = ((TextBox)gVR.Cells[2].FindControl("TextBox_ChainName")).Text;
            String ContactNo = ((TextBox)gVR.Cells[4].FindControl("TextBox_ContactNo")).Text;
            String ContactName = ((TextBox)gVR.Cells[5].FindControl("TextBox_ContactName")).Text;
            String EmailId = ((TextBox)gVR.Cells[6].FindControl("TextBox_EmailId")).Text;
            String WebSite = ((TextBox)gVR.Cells[7].FindControl("TextBox_WebSite")).Text;
            String RegstrNo = ((TextBox)gVR.Cells[8].FindControl("TextBox_RegstrNo")).Text;
            String BaseCurr = ((DropDownList)gVR.Cells[9].FindControl("DropDownList_BaseCurr")).SelectedValue;

            Dictionary<String, String> targetVals = new Dictionary<string, string>();
            Dictionary<String, String> whereCls = new Dictionary<string, string>();

            whereCls.Add(BackEndObjects.subBusinessEntity.SUB_BUSINESS_COL_BUSINESS_ID, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            whereCls.Add(BackEndObjects.subBusinessEntity.SUB_BUSINESS_COL_CHAIN_ID, ((Label)gVR.Cells[10].FindControl("Label_Hidden")).Text);

            targetVals.Add(BackEndObjects.subBusinessEntity.SUB_BUSINESS_COL_CHAIN_NAME, chainName);
            targetVals.Add(BackEndObjects.subBusinessEntity.SUB_BUSINESS_COL_MOBILE_NO, ContactNo);
            targetVals.Add(BackEndObjects.subBusinessEntity.SUB_BUSINESS_COL_CONTACT_NAME, ContactName);
            targetVals.Add(BackEndObjects.subBusinessEntity.SUB_BUSINESS_COL_EMAIL_ID, EmailId);
            targetVals.Add(BackEndObjects.subBusinessEntity.SUB_BUSINESS_COL_WEBSITE, WebSite);
            targetVals.Add(BackEndObjects.subBusinessEntity.SUB_BUSINESS_COL_REGSTR_NO, RegstrNo);
            targetVals.Add(BackEndObjects.subBusinessEntity.SUB_BUSINESS_COL_BASE_CURR, BaseCurr);

            try
            {
                BackEndObjects.subBusinessEntity.updateSubBusinessEntityDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
                Label_Update_Stat.Visible = true;
                Label_Update_Stat.Text = "update Successfull";
                Label_Update_Stat.ForeColor = System.Drawing.Color.Green;

                dt.Rows[e.RowIndex]["ChainName"] = chainName;
                dt.Rows[e.RowIndex]["ContactNo"] = ContactNo;
                dt.Rows[e.RowIndex]["ContactName"] = ContactName;
                dt.Rows[e.RowIndex]["EmailId"] = EmailId;
                dt.Rows[e.RowIndex]["WebSite"] = WebSite;
                dt.Rows[e.RowIndex]["RegstrNo"] = RegstrNo;
                dt.Rows[e.RowIndex]["BaseCurr"] = BaseCurr;

                GridView1.DataSource = dt;
                GridView1.DataBind();
                GridView1.Columns[10].Visible = false;
                Session[SessionFactory.ADMIN_PREF_CHAIN_MGMT_DATA_GRID] = dt;
            }
            catch (Exception ex)
            {
                Label_Update_Stat.Visible = true;
                Label_Update_Stat.Text = "update failed";
                Label_Update_Stat.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void loadCurrency()
        {
            Dictionary<String, BackEndObjects.Currency> currObjs = Currency.getAllCurrencyDetailsDB();

            foreach (KeyValuePair<String, Currency> kvp in currObjs)
            {
                ListItem ltCurr = new ListItem();
                ltCurr.Text = ((Currency)kvp.Value).getCurrencyName();
                ltCurr.Value = ((Currency)kvp.Value).getCurrencyId();
                DropDownListBaseCurr.Items.Add(ltCurr);

            }
            DropDownListBaseCurr.SelectedIndex = 0;
        }

        protected void loadCountryNames()
        {
            Country cn = new Country();
            Dictionary<String, BackEndObjects.Country> allCountries = new Dictionary<string, BackEndObjects.Country>();
            allCountries = Country.getAllCountrywoStatesDB();

            ListItem ltCountry = new ListItem();
            ltCountry.Text = " ";
            ltCountry.Value = "none";
            ltCountry.Selected = false;

            DropDownList2.Items.Add(ltCountry);
            DropDownList2.ClearSelection();


            foreach (KeyValuePair<String, BackEndObjects.Country> kvp in allCountries)
            {
                ListItem ltCountry1 = new ListItem();
                ltCountry1.Text = ((BackEndObjects.Country)kvp.Value).getCountryName();
                ltCountry1.Value = ((BackEndObjects.Country)kvp.Value).getCountryId();
                ltCountry.Selected = false;

                DropDownList2.Items.Add(ltCountry1);
            }
            DropDownList2.SelectedIndex = 0;
            //DropDownList2.ClearSelection();

        }
        /// <summary>
        /// Populate the sub business entity drop down list
        /// </summary>
        protected void loadSubBusinessEntity()
        {
            String mbBEId = null;
            try
            {
                mbBEId = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();
            }
            catch (Exception ex)
            {
            }
            if (mbBEId != null)
            {
                Dictionary<String, subBusinessEntity> subList = MainBusinessEntity.getSubEntitiesforMainEntitybyIdDB(mbBEId);
                foreach (KeyValuePair<String, subBusinessEntity> kvp in subList)
                {
                    ListItem ltSub = new ListItem();
                    ltSub.Text = ((subBusinessEntity)kvp.Value).getSubEntityName();
                    ltSub.Value = kvp.Key.ToString();
                    DropDownList1.Items.Add(ltSub);
                }
                DropDownList1.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// this is a overloaded method of 'loadSubBusinessEntity' to refresh the drop downl list
        /// </summary>
        /// <param name="sub"></param>
        protected void loadSubBusinessEntity(subBusinessEntity sub)
        {
            ListItem ltSub = new ListItem();
            ltSub.Text = sub.getSubEntityName();
            ltSub.Value = sub.getSubEntityId();
            DropDownList1.Items.Add(ltSub);

        }
        /// <summary>
        /// A change in Country selection should populate the State list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            String itemId = DropDownList2.SelectedValue.ToString();
            Dictionary<String, BackEndObjects.State> stateList = new Dictionary<string, BackEndObjects.State>();
            DropDownList3.Items.Clear();

            stateList = BackEndObjects.State.getStatesforCountrywoCitiesDB(itemId.Trim());


            foreach (KeyValuePair<String, BackEndObjects.State> kvp in stateList)
            {
                ListItem ltState = new ListItem();
                ltState.Value = ((BackEndObjects.State)kvp.Value).getStateId();
                ltState.Text = ((BackEndObjects.State)kvp.Value).getStateName();
                DropDownList3.Items.Add(ltState);
            }
            if (DropDownList3.Items.Count > 0)
                DropDownList3.SelectedIndex = 0;
        }

        protected void DropDownList2_TextChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// A change in State selection should populate the City list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DropDownList3_SelectedIndexChanged(object sender, EventArgs e)
        {
            String itemId = DropDownList3.SelectedValue.ToString();
            Dictionary<String, BackEndObjects.City> cityList = new Dictionary<string, BackEndObjects.City>();
            DropDownList4.Items.Clear();

            cityList = BackEndObjects.City.getCitiesforStatewoLocalitiesDB(itemId.Trim());


            foreach (KeyValuePair<String, BackEndObjects.City> kvp in cityList)
            {
                ListItem ltCity = new ListItem();
                ltCity.Value = ((BackEndObjects.City)kvp.Value).getCityId();
                ltCity.Text = ((BackEndObjects.City)kvp.Value).getCityName();
                DropDownList4.Items.Add(ltCity);
            }
            if (DropDownList4.Items.Count > 0)
                DropDownList4.SelectedIndex = 0;
        }
        /// <summary>
        /// A change in City selection should populate the locality list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DropDownList4_SelectedIndexChanged(object sender, EventArgs e)
        {
            String itemId = DropDownList4.SelectedValue.ToString();
            Dictionary<String, BackEndObjects.Localities> localList = new Dictionary<string, BackEndObjects.Localities>();
            DropDownList5.Items.Clear();

            localList = BackEndObjects.Localities.getLocalitiesforCityDB(itemId.Trim());


            foreach (KeyValuePair<String, BackEndObjects.Localities> kvp in localList)
            {
                ListItem ltLocal = new ListItem();
                ltLocal.Value = ((BackEndObjects.Localities)kvp.Value).getLocalityId();
                ltLocal.Text = ((BackEndObjects.Localities)kvp.Value).getLocalityName();
                DropDownList5.Items.Add(ltLocal);
            }
            if (DropDownList5.Items.Count > 0)
                DropDownList5.SelectedIndex = 0;


        }
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Completes the registration for the sub business entity 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button1_Click(object sender, EventArgs e)
        {
            subBusinessEntity subBE = createSubBusinessEntity();
            RegistrationActions regStr = new RegistrationActions();

            ArrayList regList = new ArrayList();
            regList.Add(subBE);
            try
            {
                regStr.completeRegr(regList);
                Label1.Visible = true;
                Label1.ForeColor = System.Drawing.Color.Green;
                Label1.Text = "Data created successfully";
                loadSubBusinessEntity(subBE);

            }
            catch (Exception ex)
            {
                Label1.Visible = true;
                Label1.ForeColor = System.Drawing.Color.Red;
                Label1.Text = "Error in creating data";
            }
        }

        protected void ButtonNext_Click(object sender, EventArgs e)
        {
            subBusinessEntity subBE = createSubBusinessEntity();
            RegistrationActions regStr = new RegistrationActions();

            ArrayList regList = new ArrayList();
            regList.Add(subBE);
            regStr.completeRegr(regList);
            loadSubBusinessEntity(subBE);
            clearAllFields("Panel2");
        }

        protected void clearAllFields(String id)
        {
            Control myForm = FindControl(id);

            foreach (Control ctl in myForm.Controls)
            {
                if (ctl.GetType().ToString().Equals("System.Web.UI.WebControls.TextBox"))
                    ((TextBox)ctl).Text = "";
                if (ctl.GetType().ToString().Equals("System.Web.UI.WebControls.DropDownList"))
                    ((DropDownList)ctl).SelectedIndex = 0;
            }

        }

        protected subBusinessEntity createSubBusinessEntity()
        {
            subBusinessEntity subBE = new subBusinessEntity();
            subBE.setMainBusinessId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            subBE.setSubEntityName(TextBox6.Text);
            subBE.setSubRegstrNo(TextBox7.Text);
            subBE.setContactName(TextBox10.Text);
            subBE.setSubPhNo(TextBox11.Text);
            subBE.setSubEmailId(TextBox12.Text);
            subBE.setSubWebSite(TextBox13.Text);
            subBE.setBaseCurrencyId(DropDownListBaseCurr.SelectedValue);
            subBE.setLocalityId(DropDownList5.SelectedValue);
            subBE.setAddrLine1(TextBox8.Text);

            Id idGen = new Id();
            String subBEId = idGen.getNewId(Id.ID_TYPE_SUB_BUS_STRING);

            subBE.setSubEntityId(subBEId);

            return subBE;
        }

        protected void Create_Chain_User_Click(object sender, EventArgs e)
        {
            userDetails udTest = BackEndObjects.userDetails.getUserDetailsbyIdDB(TextBox1.Text);
            if (udTest.getUserId() == null || udTest.getUserId().Equals("")) //New user id
            {
                userDetails uD = new userDetails();
                uD.setMainEntityId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

                Random ranGen = new Random();
                int saltInt = ranGen.Next(1, 16);
                byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes((TextBox2.Text.Equals("") ? TextBox2.Attributes["password"] : TextBox2.Text)
                    + saltInt);
                HashAlgorithm hashConverter = new SHA256Managed();
                byte[] hashedByteStream = hashConverter.ComputeHash(plainTextBytes);
                String encryptedAndConvertedPassword = Convert.ToBase64String(hashedByteStream);

                uD.setSubEntityId(DropDownList1.SelectedValue);
                uD.setUserId(TextBox1.Text);
                uD.setPassword(encryptedAndConvertedPassword);
                uD.setSalt(saltInt.ToString());
                uD.setName(TextBox_User_Name_NewAccount.Text);

                Dictionary<String, userDetails> userList = MainBusinessEntity.getUserDetailsforMainEntitybyIdDB(uD.getMainEntityId());
                if (userList.ContainsKey(uD.getUserId()))
                {
                    Label2.Visible = true;
                    Label2.ForeColor = System.Drawing.Color.Red;
                    Label2.Text = "This user account is already created for your organization";
                }
                else
                {
                    ArrayList uDChains = new ArrayList();
                    uDChains.Add(uD);
                    ActionLibrary.RegistrationActions regstr = new RegistrationActions();
                    try
                    {
                        regstr.completeRegr(uDChains);
                        Label2.Visible = true;
                        Label2.ForeColor = System.Drawing.Color.Green;
                        Label2.Text = "Account created successfully";
                    }
                    catch (Exception ex)
                    {
                        Label2.Visible = true;
                        Label2.ForeColor = System.Drawing.Color.Red;
                        Label2.Text = "Account creation failed";
                    }
                }
            }
            else
            {
                Label2.Visible = true;
                Label2.ForeColor = System.Drawing.Color.Red;
                Label2.Text = "User Id is not available..please choose another one";
            }
        }

        protected void Button_Register_Short_Click(object sender, EventArgs e)
        {

        }
    }
}