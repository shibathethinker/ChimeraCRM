using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ActionLibrary;
using BackEndObjects;
using System.Collections;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Web.UI.HtmlControls;

namespace OnLine.Pages
{
    public partial class UserPref : System.Web.UI.Page
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            Page.Theme = Session[SessionFactory.LOGGED_IN_USER_THEME].ToString();            
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                if (Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING] == null)
                    Response.Redirect("Login.aspx");
                else
                {
                    ((HtmlGenericControl)(Master.FindControl("UserPref"))).Attributes.Add("class", "active");

                    String entId = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();
                    Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];
                    Dictionary<String, Currency> allCurrList = (Dictionary<String, Currency>)Session[SessionFactory.CURRENCY_LIST];
                    String defaultCurr = Session[SessionFactory.MAIN_BUSINESS_ENTITY_DEFAULT_CURRENCY].ToString();
                    Dictionary<String, String> allExistingContacts = (Dictionary<String, String>)Session[SessionFactory.EXISTING_CONTACT_DICTIONARY];
                    String theme = Session[SessionFactory.LOGGED_IN_USER_THEME].ToString();

                    Session.RemoveAll();
                    Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING] = entId;
                    Session[SessionFactory.LOGGED_IN_USER_ID_STRING] = User.Identity.Name;
                    Session[SessionFactory.ACCESSLIST_FOR_USER] = accessList;
                    Session[SessionFactory.CURRENCY_LIST] = allCurrList;
                    Session[SessionFactory.MAIN_BUSINESS_ENTITY_DEFAULT_CURRENCY] = defaultCurr;
                    Session[SessionFactory.EXISTING_CONTACT_DICTIONARY] = allExistingContacts;
                    Session[SessionFactory.LOGGED_IN_USER_THEME] = theme;

                    //((Menu)Master.FindControl("Menu1")).Items[7].Selected = true;
                    TextBox_Pass1.Text = "";
                    TextBox_Pass2.Text = "";
                    BackEndObjects.userDetails uDObj = populatePersonalData();
                    loadThemes(uDObj);
                    populateChainData(uDObj);
                    populateLogo();
                }
            }
        }

        protected void loadThemes(userDetails uDObj)
        {
            ListItem first = new ListItem();
            first.Text = "";
            first.Value = "";

            ListItem blue = new ListItem();
            blue.Text = "Blue";
            blue.Value = "ThemeBlue";

            ListItem red = new ListItem();
            red.Text = "Red";
            red.Value = "ThemeRed";

            ListItem black = new ListItem();
            black.Text = "Black";
            black.Value = "ThemeBlack";

            DropDownList_Theme.Items.Add(first);
            DropDownList_Theme.Items.Add(blue);
            DropDownList_Theme.Items.Add(red);
            DropDownList_Theme.Items.Add(black);

            DropDownList_Theme.SelectedValue = "";


            Label_Curren_Theme_Name.Text = Session[SessionFactory.LOGGED_IN_USER_THEME].ToString();
            

        }
        protected void populateLogo()
        {
            ArrayList imgListObjs = BackEndObjects.Image.getImagebyTypeandEntId(BackEndObjects.Image.PICTURE_IMG_TYPE_LOGO, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            if (imgListObjs.Count > 0)
            {
                //Only consider the first image object for logo
                BackEndObjects.Image imgObj = (BackEndObjects.Image)imgListObjs[0];
                ((System.Web.UI.WebControls.Image)Master.FindControl("Image_Logo")).ImageUrl = imageToURL(imgObj.getImgPath());
                ((System.Web.UI.WebControls.Image)Master.FindControl("Image_Logo")).Visible = true;

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

        protected userDetails populatePersonalData()
        {
            userDetails uDObj=BackEndObjects.userDetails.
                getUserDetailsbyIdDB(User.Identity.Name, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            
            if (uDObj.getName() != null)
                TextBox_Name.Text = uDObj.getName();
            if (uDObj.getEmailId() != null)
                TextBox_Email.Text = uDObj.getEmailId();
            if (uDObj.getContactNo() != null)
                TextBox_Contact.Text = uDObj.getContactNo();
            /*if (uDObj.getDeptId() != null)
                TextBox_Dept.Text = uDObj.getDeptId();*/

            return uDObj;
        }

        protected void populateChainData(userDetails udObj)
        {
            if (udObj.getSubEntityId() != null && !udObj.getSubEntityId().Equals(""))
            {
                BackEndObjects.subBusinessEntity subObj=BackEndObjects.subBusinessEntity.getSubBusinessEntitybyIdDB(udObj.getSubEntityId());
                Label_Chain_Name.Text=subObj.getSubEntityName();
                Label_Chain_Email.Text = subObj.getSubEmailId();
                Label_Chain_Contact.Text = subObj.getSubPhNo();
                Label_Chain_Regstr.Text = subObj.getSubRegstrNo();
                Label_Addr1.Text = subObj.getAddrLine1();

                BackEndObjects.Localities lclObj = BackEndObjects.Localities.getLocalitybyIdDB(subObj.getLocalityId());
                BackEndObjects.City ctObj = BackEndObjects.Localities.getCityDetailsforLocalitywoOtherAsscLocalitiesDB(subObj.getLocalityId());
                BackEndObjects.State stObj = BackEndObjects.City.getStateDetailsforCitywoOtherAsscCitiesDB(ctObj.getCityId());
                BackEndObjects.Country cntObj = BackEndObjects.State.getCountryDetailsforStatewoOtherAsscStatesDB(stObj.getStateId());

                Label_Country.Text = cntObj.getCountryName();
                Label_State.Text = stObj.getStateName();
                Label_City.Text = ctObj.getCityName();
                Label_Locality.Text = lclObj.getLocalityName();
            }
        }

        protected void Button_Submit_Req_Click(object sender, EventArgs e)
        {
            BackEndObjects.userDetails userObj = BackEndObjects.userDetails.getUserDetailsbyIdDB(User.Identity.Name);
            //Combine salt and generate the password
            byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(TextBox_Pass2.Text + userObj.getSalt());
            HashAlgorithm hashConverter = new SHA256Managed();
            byte[] hashedByteStream = hashConverter.ComputeHash(plainTextBytes);
            String encryptedAndConvertedPassword = Convert.ToBase64String(hashedByteStream);

            Dictionary<String, String> targetVals = new Dictionary<string, string>();
            targetVals.Add(BackEndObjects.userDetails.USER_DETAILS_COL_PASSWORD, encryptedAndConvertedPassword);

            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            whereCls.Add(BackEndObjects.userDetails.USER_DETAILS_COL_BUSINESS_ID, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            whereCls.Add(BackEndObjects.userDetails.USER_DETAILS_COL_USERID, User.Identity.Name);
            try
            {
                BackEndObjects.userDetails.updateUserDetailsDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
                Label_Pass_Change_Stat.Visible = true;
                Label_Pass_Change_Stat.Text = "Password Changed Successfully";
                Label_Pass_Change_Stat.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                Label_Pass_Change_Stat.Visible = true;
                Label_Pass_Change_Stat.Text = "Password Change Failed";
                Label_Pass_Change_Stat.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void Button_Change_Details_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> targetVals = new Dictionary<string, string>();
            targetVals.Add(BackEndObjects.userDetails.USER_DETAILS_COL_CONTACT_NO,TextBox_Contact.Text);
            targetVals.Add(BackEndObjects.userDetails.USER_DETAILS_COL_EMAIL_ID, TextBox_Email.Text);
            targetVals.Add(BackEndObjects.userDetails.USER_DETAILS_COL_NAME, TextBox_Name.Text);
            
            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            whereCls.Add(BackEndObjects.userDetails.USER_DETAILS_COL_BUSINESS_ID, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            whereCls.Add(BackEndObjects.userDetails.USER_DETAILS_COL_USERID, User.Identity.Name);

            try
            {
                BackEndObjects.userDetails.updateUserDetailsDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
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

        protected void Button_Change_Theme_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> targetVals = new Dictionary<string, string>();
            targetVals.Add(BackEndObjects.userDetails.USER_DETAILS_COL_THEME, DropDownList_Theme.SelectedValue);

            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            whereCls.Add(BackEndObjects.userDetails.USER_DETAILS_COL_BUSINESS_ID, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            whereCls.Add(BackEndObjects.userDetails.USER_DETAILS_COL_USERID, User.Identity.Name);

            try
            {
                BackEndObjects.userDetails.updateUserDetailsDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
                Label_Theme_Change_Stat.Visible = true;
                Label_Theme_Change_Stat.Text = "Details Changed Successfully. You need to log out and log in again to see the change";
                Label_Theme_Change_Stat.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                Label_Theme_Change_Stat.Visible = true;
                Label_Theme_Change_Stat.Text = "Details Change Failed";
                Label_Theme_Change_Stat.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}