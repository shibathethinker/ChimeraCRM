using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using BackEndObjects;
using ActionLibrary;
using System.Collections;


namespace OnLine.Pages
{
    public partial class Login : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //RegisterUser.ContinueDestinationPageUrl = Request.QueryString["ReturnUrl"];
            }
        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {            
           // BackEndObjects.userDetails uDObj = new BackEndObjects.userDetails();

           BackEndObjects.userDetails userObj = BackEndObjects.userDetails.getUserDetailsbyIdDB(UserName.Text);
            //Combine salt and generate the password
           byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(Password.Text + userObj.getSalt());
           HashAlgorithm hashConverter = new SHA256Managed();
           byte[] hashedByteStream = hashConverter.ComputeHash(plainTextBytes);
           String encryptedAndConvertedPassword = Convert.ToBase64String(hashedByteStream);


           if (userObj.authenticateUserDB(UserName.Text, encryptedAndConvertedPassword))
            {

                Session[SessionFactory.LOGGED_IN_USER_ID_STRING] = UserName.Text.Trim();
                Session[SessionFactory.LOGGED_IN_USER_THEME] = userObj.getTheme() == null || userObj.getTheme().Equals("") ? "ThemeBlue" : userObj.getTheme();
                //Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING] = BackEndObjects.userDetails.getUserDetailsbyIdDB(UserName.Text).getMainEntityId();
                Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING] = userObj.getMainEntityId();
                Session[SessionFactory.ACCESSLIST_FOR_USER] = new ActionLibrary.LoginActions().
                    retrieveAccessList(UserName.Text.Trim(), Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                Session[SessionFactory.CURRENCY_LIST]=BackEndObjects.Currency.getAllCurrencyDetailsDB();
                Session[SessionFactory.MAIN_BUSINESS_ENTITY_DEFAULT_CURRENCY] = AddressDetails.
                    getAddressforMainBusinessEntitybyIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()).getBaseCurrencyId();
                
               ArrayList contactObjList = Contacts.
getAllContactsbyEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
               Dictionary<String, String> existingContactDict = new Dictionary<string, string>();
                for (int i = 0; i < contactObjList.Count; i++)
                {
                    String contactName = ((Contacts)contactObjList[i]).getContactName();
                    String contactEntId = ((Contacts)contactObjList[i]).getContactEntityId();

                    if (!existingContactDict.ContainsKey(contactName))
                    existingContactDict.Add(contactName, contactEntId);
                }
                Session[SessionFactory.EXISTING_CONTACT_DICTIONARY] = existingContactDict;

                FormsAuthentication.RedirectFromLoginPage(UserName.Text, RememberMe.Checked);
            }
            else
            {
                FailureText.Visible= true;
            }

     
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Register.aspx");
        }

        

    }
}