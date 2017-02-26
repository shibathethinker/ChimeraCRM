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
using System.IO;

namespace OnLine.Pages.Popups.AdminPref
{
    public partial class mainEntityManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                populateEntityDetails();
                loadCountry();
                loadAddrAndCurrDetails();
                populateLogo();
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

        protected void loadProdServList(Dictionary<String, ProductCategory> entityProdDict)
        {
            Dictionary<String,ProductCategory> prodCatList=BackEndObjects.ProductCategory.getAllParentCategory();

            foreach (KeyValuePair<String, ProductCategory> kvp in prodCatList)
            {
                ListItem ltProdCat = new ListItem();
                ltProdCat.Text = ((ProductCategory)kvp.Value).getProductCategoryName();
                ltProdCat.Value = ((ProductCategory)kvp.Value).getCategoryId();
                ListBoxProdServc.Items.Add(ltProdCat);
                if (entityProdDict != null && entityProdDict.ContainsKey(ltProdCat.Value))
                    ltProdCat.Selected = true;
            }
            
            //ListBoxProdServc.SelectedIndex = -1;
        }

        protected void populateEntityDetails()
        {
            MainBusinessEntity mBEObj=BackEndObjects.MainBusinessEntity.
                getMainBusinessEntitybyIdwithLessDetailsDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            loadProdServList(MainBusinessEntity.getProductDetailsforMainEntitybyIdDB(mBEObj.getEntityId()));

            TextBox_Name.Text = mBEObj.getEntityName();
            TextBox_Contact.Text = mBEObj.getPhNo();
            TextBox_Email.Text = mBEObj.getEmailId();
            TextBox_Owner_Name.Text = mBEObj.getOwnerName();
            TextBox_Site.Text = mBEObj.getWebSite();

            Dictionary<String,businessDescription> busDescDict=BackEndObjects.businessDescription.getAllBusinessDescriptionDB();

            foreach (KeyValuePair<String, businessDescription> kvp in busDescDict)
            {
                ListItem lt = new ListItem();
                lt.Text = ((businessDescription)kvp.Value).getDescName();
                lt.Value = ((businessDescription)kvp.Value).getDescId();
                
                DropDownList_Business_Desc.Items.Add(lt);
                if(((businessDescription)kvp.Value).getDescId().Equals(mBEObj.getDesc()))
                    DropDownList_Business_Desc.SelectedValue=lt.Value;
            }
        }

        protected void populateLogo()
        {
            ArrayList imgListObjs = BackEndObjects.Image.getImagebyTypeandEntId(BackEndObjects.Image.PICTURE_IMG_TYPE_LOGO, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            if (imgListObjs.Count > 0)
            {
                //Only consider the first image object for logo
                BackEndObjects.Image imgObj = (BackEndObjects.Image)imgListObjs[0];
                Image_Logo.ImageUrl = imageToURL(imgObj.getImgPath());
                Image_Logo.Visible = true;

            }
        }
        public String generateImagePath(String folderName)
        {
            String imgStoreRoot = "~/Images/SessionImages";
            imgStoreRoot = Server.MapPath(imgStoreRoot);

            String returnPath = "";
            try
            {
                if (!Directory.Exists(imgStoreRoot + "\\" + folderName.ToString()))
                    Directory.CreateDirectory(imgStoreRoot + "\\" + folderName.ToString());
                returnPath = imgStoreRoot + "\\" + folderName.ToString();
            }
            catch (Exception ex)
            {
                returnPath = "";

            }
            return returnPath;
        }
        public String imageToURL(string sPath)
        {
            /*System.Net.WebClient client = new System.Net.WebClient();
            byte[] imageData = client.DownloadData(sPath);
            System.IO.MemoryStream stream = new System.IO.MemoryStream(imageData);
            System.Drawing.Image img = System.Drawing.Image.FromStream(stream);*/

            String[] imageNameParts = null;
            if (sPath != null && !sPath.Equals(""))
                imageNameParts = sPath.Split(new String[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);

            String finalImageUrl = "~/Images/SessionImages/" + Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString() + "/" + imageNameParts[imageNameParts.Length - 1];

            if (!File.Exists(Server.MapPath(finalImageUrl)))
            {
                try
                {
                    System.Drawing.Image img = System.Drawing.Image.FromFile(sPath);
                    System.Drawing.Bitmap newBitMap = new System.Drawing.Bitmap(img);

                    generateImagePath(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

                    newBitMap.Save(Server.MapPath(finalImageUrl), System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                catch (Exception ex)
                {
                    finalImageUrl = "";
                }
            }

            return finalImageUrl;
        }

        protected void Button_Change_Details_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> targetVals = new Dictionary<string, string>();
            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            Dictionary<String, String> whereClsProd = new Dictionary<string, string>();

            int[] prodServ=ListBoxProdServc.GetSelectedIndices();
            ArrayList catList= new ArrayList();
                       
            for (int i = 0; i < prodServ.Length; i++)
                      catList.Add(ListBoxProdServc.Items[prodServ[i]].Value);
            
            
            whereCls.Add(BackEndObjects.MainBusinessEntity.MAIN_BUSINESS_COL_BUSINESS_ID, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            whereClsProd.Add(BackEndObjects.MainBusinessEntity.MAIN_BUSINESS_RELATED_PRODUCTS_COL_BUSINESS_ID, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            

            targetVals.Add(BackEndObjects.MainBusinessEntity.MAIN_BUSINESS_COL_DESC, DropDownList_Business_Desc.SelectedValue);
            targetVals.Add(BackEndObjects.MainBusinessEntity.MAIN_BUSINESS_COL_EMAIL_ID, TextBox_Email.Text);
            targetVals.Add(BackEndObjects.MainBusinessEntity.MAIN_BUSINESS_COL_MOBILE_NO, TextBox_Contact.Text);
            targetVals.Add(BackEndObjects.MainBusinessEntity.MAIN_BUSINESS_COL_OWNER_NAME, TextBox_Owner_Name.Text);
            targetVals.Add(BackEndObjects.MainBusinessEntity.MAIN_BUSINESS_COL_SHOP_NAME, TextBox_Name.Text);
            targetVals.Add(BackEndObjects.MainBusinessEntity.MAIN_BUSINESS_COL_WEBSITE, TextBox_Site.Text);

            try
            {
                BackEndObjects.MainBusinessEntity.updateMainBusinessEntityWOimg_prd_user_subDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
                BackEndObjects.MainBusinessEntity.updateProductDetailsforEntityDB(null, whereClsProd, DBConn.Connections.OPERATION_DELETE);
                BackEndObjects.MainBusinessEntity.insertProductDetailsforEntityDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), catList);

                Label_Det_Change_Stat.Visible = true;
                Label_Det_Change_Stat.Text = "Details Changed Successfully";
                Label_Det_Change_Stat.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                Label_Det_Change_Stat.Visible = true;
                Label_Det_Change_Stat.Text = "Details Change Failed";
                Label_Det_Change_Stat.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void DropDownList_Country_SelectedIndexChanged(object sender, EventArgs e)
        {
            String itemId = DropDownList_Country.SelectedValue.ToString();
            Dictionary<String, BackEndObjects.State> stateList = new Dictionary<string, BackEndObjects.State>();
            DropDownList_State.Items.Clear();

            stateList = BackEndObjects.State.getStatesforCountrywoCitiesDB(itemId.Trim());


            foreach (KeyValuePair<String, BackEndObjects.State> kvp in stateList)
            {
                ListItem ltState = new ListItem();
                ltState.Value = ((BackEndObjects.State)kvp.Value).getStateId();
                ltState.Text = ((BackEndObjects.State)kvp.Value).getStateName();
                DropDownList_State.Items.Add(ltState);
            }
            if (DropDownList_State.Items.Count > 0)
                DropDownList_State.SelectedIndex = 0;
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
            if (DropDownList_City.Items.Count > 0)
                DropDownList_City.SelectedIndex = 0;
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
            if (DropDownList_Locality.Items.Count > 0)
                DropDownList_Locality.SelectedIndex = 0;


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

        protected void loadAddrAndCurrDetails()
        {
            AddressDetails addrObj=BackEndObjects.AddressDetails.
                getAddressforMainBusinessEntitybyIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

            Label_Addr1.Text = addrObj.getAddrLine1();
            loadLocalityDetails(addrObj.getLocalityId());

            Dictionary<String,Currency> currDict=BackEndObjects.Currency.getAllCurrencyDetailsDB();
            foreach (KeyValuePair<String, Currency> kvp in currDict)
            {
                ListItem lt = new ListItem();
                lt.Text = ((Currency)kvp.Value).getCurrencyName();
                lt.Value = ((Currency)kvp.Value).getCurrencyId();
                
                DropDownList_Base_Curr.Items.Add(lt);

                if (addrObj.getBaseCurrencyId()!=null && !addrObj.getBaseCurrencyId().Equals("") 
                    && addrObj.getBaseCurrencyId().Equals(((Currency)kvp.Value).getCurrencyId()))
                    DropDownList_Base_Curr.SelectedValue = lt.Value;
            }
        }

        protected void Button_Change_Addr_Click(object sender, EventArgs e)
        {
                        AddressDetails addrObj=BackEndObjects.AddressDetails.
                getAddressforMainBusinessEntitybyIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

                        Dictionary<String, String> whereCls = new Dictionary<string, string>();
                        Dictionary<String, String> targetVals = new Dictionary<string, string>();
                        ArrayList addrObjList = new ArrayList();

                        Boolean update = false;

                           //If address is already present then its a update request
                        if (addrObj!=null && addrObj.getMainBusinessId()!=null)
                        {
                            update = true;
                            whereCls.Add(BackEndObjects.AddressDetails.ADDR_DETAILS_COL_BUSINESS_ID, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                            whereCls.Add(BackEndObjects.AddressDetails.ADDR_DETAILS_COL_CHAIN_ID, "");
                            targetVals.Add(BackEndObjects.AddressDetails.ADDR_DETAILS_COL_ADDR_LINE1, TextBox_Street_Name.Text);
                            targetVals.Add(BackEndObjects.AddressDetails.ADDR_DETAILS_COL_LOCALITY_ID, DropDownList_Locality.SelectedValue);
                            targetVals.Add(BackEndObjects.AddressDetails.ADDR_DETAILS_COL_BASE_CURR, DropDownList_Base_Curr.SelectedValue);
                        }
                        else
                        {
                            BackEndObjects.AddressDetails addressObj = new AddressDetails();
                            
                            addressObj.setAddrLine1(TextBox_Street_Name.Text);
                            addressObj.setLocalityId(DropDownList_Locality.SelectedValue);
                            addressObj.setBaseCurrencyId(DropDownList_Base_Curr.SelectedValue);
                            addressObj.setMainBusinessId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                            addressObj.setSubEntityId(BackEndObjects.AddressDetails.DUMMY_CHAIN_ID);

                            addrObjList.Add(addressObj);
                        }
            try
            {
                if (update)
                    BackEndObjects.AddressDetails.updateAddressEntityDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
                else
                    BackEndObjects.AddressDetails.insertAddressEntityDB(addrObjList);
                
                Label_Addr_Change_Stat.Visible = true;
                Label_Addr_Change_Stat.Text = "Details updated successfully";
                Label_Addr_Change_Stat.ForeColor = System.Drawing.Color.Green;

                Label_Addr1.Text = TextBox_Street_Name.Text;
                            String localId = !DropDownList_Locality.SelectedValue.Equals("") ? DropDownList_Locality.SelectedValue :
    !DropDownList_City.SelectedValue.Equals("") ? DropDownList_City.SelectedValue :
    !DropDownList_State.SelectedValue.Equals("") ? DropDownList_State.SelectedValue : DropDownList_Country.SelectedValue;
                            loadLocalityDetails(localId);
            }
            catch (Exception ex)
            {
                Label_Addr_Change_Stat.Visible = true;
                Label_Addr_Change_Stat.Text = "Update Failed";
                Label_Addr_Change_Stat.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void Button_Insert_Image_Click(object sender, EventArgs e)
        {
            BackEndObjects.Image imgObj = new BackEndObjects.Image();
            imgObj.setEntityId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            imgObj.setFileStream(FileUpload1);
            imgObj.setImgId(new BackEndObjects.Id().getNewId(BackEndObjects.Id.ID_TYPE_IMAGE_ID_STRING));
            imgObj.setImgPath();
            imgObj.setImgType(BackEndObjects.Image.PICTURE_IMG_TYPE_LOGO);
            imgObj.setReferenceId("");

            ArrayList imgList=new ArrayList();
            imgList.Add(imgObj);

            try
            {
                ArrayList imgListObjs = BackEndObjects.Image.getImagebyTypeandEntId(BackEndObjects.Image.PICTURE_IMG_TYPE_LOGO, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                for(int i=0;i<imgListObjs.Count;i++)
                {
                    BackEndObjects.Image oldImgObj=(BackEndObjects.Image)imgListObjs[i];
                    BackEndObjects.Image.updateImageforEntityDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), oldImgObj.getImgId(), DBConn.Connections.OPERATION_DELETE);

                    String[] imageNameParts = null;
                    if (oldImgObj.getImgPath() != null && !oldImgObj.getImgPath().Equals(""))
                        imageNameParts = oldImgObj.getImgPath().Split(new String[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);

                    String finalImageUrl = "~/Images/SessionImages/" + Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString() +
                        "/" + imageNameParts[imageNameParts.Length - 1];

                    //Remove the existing logo copy from website storage
                    if(File.Exists(Server.MapPath(finalImageUrl)))
                        File.Delete(Server.MapPath(finalImageUrl));
                    
                }
                BackEndObjects.Image.insertImageforEntityDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), imgList);
                Label_Upload_Logo.Visible = true;
                Label_Upload_Logo.Text = "Image Uploaded Successfully";
                Label_Upload_Logo.ForeColor = System.Drawing.Color.Green;
                populateLogo();
            }
            catch (Exception ex)
            {
                Label_Upload_Logo.Visible = true;
                Label_Upload_Logo.Text = "Image Upload Failed";
                Label_Upload_Logo.ForeColor = System.Drawing.Color.Red;
            }
        }



    }
}