using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnLine.Pages
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Page.Header.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}