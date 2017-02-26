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
    public partial class Notification_Sales : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                String msgTxt = Request.QueryString.GetValues("msg")[0];
                Label1.Text = msgTxt;
            }
        }

    }
}