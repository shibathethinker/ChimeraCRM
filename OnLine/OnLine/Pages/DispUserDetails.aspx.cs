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
    public partial class DispUserDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                loadUserData();
        }

        protected void loadUserData()
        {
            String userId = Request.QueryString.GetValues("userId")[0];
            userDetails uDObj=BackEndObjects.userDetails.getUserDetailsbyIdDB(userId, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

            if (uDObj != null && uDObj.getUserId() != null && !uDObj.getUserId().Equals(""))
            {
                Label_Name.Text = uDObj.getName();
                Label_Email.Text = uDObj.getEmailId();
                Label_Contact.Text = uDObj.getContactNo();
            }
        }
    }
}