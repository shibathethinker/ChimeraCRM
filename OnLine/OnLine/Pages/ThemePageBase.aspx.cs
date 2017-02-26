using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnLine.Pages
{
    public partial class ThemePageBase : System.Web.UI.Page
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            Page.Theme = "ThemeRed";
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}