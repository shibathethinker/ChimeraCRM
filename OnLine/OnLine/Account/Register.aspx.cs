using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActionLibrary;
using BackEndObjects;
using System.Collections;
using BackEndObjects;
using System.Security.Cryptography;



namespace OnLine.Pages
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                loadCountryNames();
                loadDescriptions();
                loadCurrency();
                loadProdServList();
              }
            else if (!TextBox2.Text.Equals(""))
            {
                TextBox2.Attributes["password"] = TextBox2.Text;
            }
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
        /// Load drop down box containing business description.
        /// </summary>
        protected void loadDescriptions()
        {
            Dictionary<String, BackEndObjects.businessDescription> busDescAll = BackEndObjects.businessDescription.getAllBusinessDescriptionDB();

            foreach (KeyValuePair<String, BackEndObjects.businessDescription> kvp in busDescAll)
            {
                ListItem ltDesc = new ListItem();
                ltDesc.Text = ((businessDescription)kvp.Value).getDescName();
                ltDesc.Value = ((businessDescription)kvp.Value).getDescId();
                DropDownListDescr.Items.Add(ltDesc);
            }
            DropDownListDescr.SelectedIndex = 0;
        }

        protected void loadCurrency()
        {
            Dictionary<String, BackEndObjects.Currency> currObjs = Currency.getAllCurrencyDetailsDB();

            foreach (KeyValuePair<String, Currency> kvp in currObjs)
            {
                ListItem ltCurr = new ListItem();
                ltCurr.Text=((Currency)kvp.Value).getCurrencyName();
                ltCurr.Value=((Currency)kvp.Value).getCurrencyId();
                DropDownListBaseCurr.Items.Add(ltCurr);

            }
            DropDownListBaseCurr.SelectedIndex = 0;
        }
        /// <summary>
        /// Populate the listbox with list of product category/services
        /// </summary>
        protected void loadProdServList()
        {
            Dictionary<String,ProductCategory> prodCatList=BackEndObjects.ProductCategory.getAllParentCategory();

            foreach (KeyValuePair<String, ProductCategory> kvp in prodCatList)
            {
                ListItem ltProdCat = new ListItem();
                ltProdCat.Text = ((ProductCategory)kvp.Value).getProductCategoryName();
                ltProdCat.Value = ((ProductCategory)kvp.Value).getCategoryId();
                ListBoxProdServc.Items.Add(ltProdCat);
            }
            ListBoxProdServc.SelectedIndex = -1;
        }
        /// <summary>
        /// Pass required objects to ActionLibray registration method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_Register_Short_Click(object sender, EventArgs e)
        {
            ActionLibrary.RegistrationActions regstr = new ActionLibrary.RegistrationActions();

            BackEndObjects.MainBusinessEntity mBE = new MainBusinessEntity();

            userDetails udTest = BackEndObjects.userDetails.getUserDetailsbyIdDB(TextBox1.Text);
            
            if (udTest.getUserId() == null||udTest.getUserId().Equals("")) //New user id
            {
                
                mBE.setEntityName(TextBox5.Text);
                mBE.setEmailId(TextBox4.Text);
                mBE.setIndChain("I"); //This should later be allowed to be changed
                
                BackEndObjects.Id IdGen = new BackEndObjects.Id();
                String mBEId = IdGen.getNewId(Id.ID_TYPE_CMP_USR_STRING);
                mBE.setEntityId(mBEId);

                Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING] = mBEId;


                userDetails uD = new userDetails();
                uD.setMainEntityId(mBEId);
                
                Random ranGen = new Random();
                int saltInt=ranGen.Next(1, 16);
                byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes((TextBox2.Text.Equals("") ? TextBox2.Attributes["password"] : TextBox2.Text)
                    + saltInt);
                HashAlgorithm hashConverter = new SHA256Managed();
                byte[] hashedByteStream = hashConverter.ComputeHash(plainTextBytes);
                String encryptedAndConvertedPassword = Convert.ToBase64String(hashedByteStream);

                uD.setPassword(encryptedAndConvertedPassword);
                uD.setUserId(TextBox1.Text);
                //Set owner privilege for the first user by default
                uD.setPrivilege(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS);
                uD.setSalt(saltInt.ToString());

                ArrayList regstObjs = new ArrayList();
                regstObjs.Add(mBE);
                regstObjs.Add(uD);

                regstr.completeRegr(regstObjs);

                Session[SessionFactory.SHORT_REGISTR_COMPLETE] = "true";

                Label_UserId_Exists.Visible = true;
                Label_UserId_Exists.ForeColor = System.Drawing.Color.Green;
                Label_UserId_Exists.Text = "Registration Successful";

                Button_Register_Short.Enabled = false;
            }
            else
            {
                Label_UserId_Exists.Visible = true;
                Label_UserId_Exists.Text = "User id is not available";
            }
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

            stateList=BackEndObjects.State.getStatesforCountrywoCitiesDB(itemId.Trim());

            
            foreach(KeyValuePair<String,BackEndObjects.State> kvp in stateList)
            {
                ListItem ltState = new ListItem();
                ltState.Value = ((BackEndObjects.State)kvp.Value).getStateId();
                ltState.Text = ((BackEndObjects.State)kvp.Value).getStateName();
                DropDownList3.Items.Add(ltState);
            }
            if(DropDownList3.Items.Count>0)
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
            if(DropDownList4.Items.Count>0)
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
            if(DropDownList5.Items.Count>0)
            DropDownList5.SelectedIndex = 0;


        }
        /// <summary>
        /// Complete registration for the business entity
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button1_Click(object sender, EventArgs e)
        {
            String shortRegstr = (Session[SessionFactory.SHORT_REGISTR_COMPLETE] != null ? Session[SessionFactory.SHORT_REGISTR_COMPLETE].ToString() : "");

           bool shortRegstrCompl = ((shortRegstr != null && shortRegstr.Equals("true"))? true : false);

            ActionLibrary.RegistrationActions regstr = new ActionLibrary.RegistrationActions();


            BackEndObjects.MainBusinessEntity mBE = new MainBusinessEntity();
                mBE.setEntityName(TextBox5.Text);
                mBE.setEmailId(TextBox4.Text);
                mBE.setWebSite(TextBox7.Text);
                mBE.setIndChain(DropDownList1.SelectedValue.Trim());
                mBE.setOwnerName(TextBox6.Text);
                mBE.setDesc(DropDownListDescr.SelectedValue);
                mBE.setPhNo(TextBox10.Text);

                String mBEId = "";
                BackEndObjects.Id IdGen = new BackEndObjects.Id();

                if (!shortRegstrCompl)
                    mBEId = IdGen.getNewId(Id.ID_TYPE_CMP_USR_STRING);
                else
                    mBEId = (Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING] != null ? Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString() : "");

                mBE.setEntityId(mBEId);


            int[] prodServ=ListBoxProdServc.GetSelectedIndices();
            Dictionary<String, ProductCategory> prdDict = new Dictionary<string, BackEndObjects.ProductCategory>();

            Dictionary<String, ProductCategory> prodCatMBE = MainBusinessEntity.getProductDetailsforMainEntitybyIdDB(mBE.getEntityId());

            for (int i = 0; i < prodServ.Length; i++)
            {
                ProductCategory ePd = new ProductCategory();

                //Add products/service which are not already added for the Main business entity - this is to avoid error situation
                //when the user by mistake clicks twice in the submit button in the registration page
                if (!prodCatMBE.ContainsKey(ListBoxProdServc.Items[prodServ[i]].Value))
                {
                    ePd.setCategoryId(ListBoxProdServc.Items[prodServ[i]].Value);
                    ePd.setProductCategoryName(ListBoxProdServc.Items[prodServ[i]].Text);
                    prdDict.Add(ePd.getCategoryId(), ePd);
                }
                
            }
            mBE.setMainProductServices(prdDict);
                                             
            userDetails uD = new userDetails();
            Boolean userIdAlreadyExistis = false;
            if (!shortRegstrCompl)
            {
                userDetails udTest = BackEndObjects.userDetails.getUserDetailsbyIdDB(TextBox1.Text);
                if (udTest.getUserId() == null || udTest.getUserId().Equals("")) //New user id
                {
                    uD.setMainEntityId(mBEId);

                    Random ranGen = new Random();
                    int saltInt = ranGen.Next(1, 16);
                    byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes((TextBox2.Text.Equals("") ? TextBox2.Attributes["password"] : TextBox2.Text)
                        + saltInt);
                    HashAlgorithm hashConverter = new SHA256Managed();
                    byte[] hashedByteStream = hashConverter.ComputeHash(plainTextBytes);
                    String encryptedAndConvertedPassword = Convert.ToBase64String(hashedByteStream);

                    uD.setPassword(encryptedAndConvertedPassword);
                    uD.setUserId(TextBox1.Text);
                    uD.setMainEntityId(mBE.getEntityId());
                    uD.setSalt(saltInt.ToString());
                    uD.setPrivilege(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS);
                }
                else
                {
                    Label_UserId_Exists.Visible = true;
                    Label_UserId_Exists.Text = "User Id not available.. please enter a different one";
                    userIdAlreadyExistis = true;
                }
            }

            AddressDetails aD = new AddressDetails();

            AddressDetails aDTest = AddressDetails.getAddressforMainBusinessEntitybyIdDB(mBE.getEntityId());
            //If Address detais for this main business entity is not already set up - this is to handle error situation.
            //when the user by mistake clicks twice in the submit button in the registration page
            //Because as of now, the backend only accepts one address detail for the main business entity
            if (aDTest.getLocalityId() == null || aDTest.getLocalityId().Equals("")) 
            {
                aD.setAddrLine1(TextBox8.Text);
                aD.setLocalityId(DropDownList5.SelectedValue);
                aD.setBaseCurrencyId(DropDownListBaseCurr.SelectedValue);
                aD.setMainBusinessId(mBEId);
                aD.setSubEntityId(AddressDetails.DUMMY_CHAIN_ID);
            }           
            //FileUpload fU = FileUpload1;
            //Removing the option of image upload in registration page
            BackEndObjects.Image imgObj = new BackEndObjects.Image();
            /*if (fU != null && fU.HasFile)
            {
                imgObj.setImgId(IdGen.getNewId(Id.ID_TYPE_IMAGE_ID_STRING));
                imgObj.setEntityId(mBE.getEntityId());
                imgObj.setFileStream(fU);
                imgObj.setImgPath();
            }*/

            Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING] = mBEId;
            ArrayList regstObjs = new ArrayList();

            if(mBE.getEntityId()!=null && !mBE.getEntityId().Equals(""))
            regstObjs.Add(mBE);
            if(uD.getUserId()!=null && !uD.getUserId().Equals(""))
            regstObjs.Add(uD);
            if(aD.getLocalityId()!=null && !aD.getLocalityId().Equals(""))
            regstObjs.Add(aD);
            if(imgObj.getImgId()!=null && !imgObj.getImgId().Equals(""))
            regstObjs.Add(imgObj);

            try
            {
                if (!userIdAlreadyExistis)
                {
                    regstr.completeRegr(regstObjs);
                    Label_Status.Visible = true;
                    Label_Status.ForeColor = System.Drawing.Color.Green;
                    Label_Status.Text = "Data inserted successfully";
                    Button_Register_Business.Enabled = false;
                    Button_Register_Short.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Label_Status.Visible = true;
                Label_Status.ForeColor = System.Drawing.Color.Red;
                Label_Status.Text = "Error entering details";
            }
            HyperLink1.Visible = true;
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Panel2.Visible = true;
        }

            protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

 

 
    }
}