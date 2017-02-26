using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using BackEndObjects;
using ActionLibrary;

/*
THIS CLASS IS NOT BEING USED CURRENTLY AS THE INNER GRID IS WORKING FINE.
 * In current scenario this page will never be called - kept just for reference
*/

namespace OnLine.Pages.Popups.Purchase
{
    public partial class AllRequirement_Specification_Show_Feat : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                fillGrid();
            }
        }

        protected void fillGrid()
        {
            String selectedProdCat = Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_SHOW_FEAT_SELECTED_PRODCAT].ToString();
            ArrayList reqrSpecList = (ArrayList)Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_REQR_SPEC];

            DataTable dtSpec = new DataTable();
            dtSpec.Columns.Add("Hidden");
            dtSpec.Columns.Add("FeatName");
            dtSpec.Columns.Add("SpecText");
            dtSpec.Columns.Add("FromSpec");
            dtSpec.Columns.Add("ToSpec");

            int rowCount = 0;
            for (int j = 0; j < reqrSpecList.Count; j++)
            {                
                BackEndObjects.Requirement_Spec reqrSpecObj = (BackEndObjects.Requirement_Spec)reqrSpecList[j];
                
                if (reqrSpecObj.getProdCatId().Equals(selectedProdCat))
                {
                    dtSpec.Rows.Add();
                    String featName = BackEndObjects.Features.getFeaturebyIdwoSpecDB(reqrSpecObj.getFeatId()).getFeatureName();
                    dtSpec.Rows[rowCount]["Hidden"] = reqrSpecObj.getFeatId();
                    dtSpec.Rows[rowCount]["FeatName"] = (featName != null ? featName.Trim() : "");
                    dtSpec.Rows[rowCount]["SpecText"] = reqrSpecObj.getSpecText();
                    if (!reqrSpecObj.getFromSpecId().Equals(""))
                        dtSpec.Rows[rowCount]["FromSpec"] = BackEndObjects.Specifications.getSpecificationDetailbyIdDB(reqrSpecObj.getFromSpecId()).getSpecName();
                    if (!reqrSpecObj.getToSpecId().Equals(""))
                        dtSpec.Rows[rowCount]["ToSpec"] = BackEndObjects.Specifications.getSpecificationDetailbyIdDB(reqrSpecObj.getToSpecId()).getSpecName();

                    rowCount++;
                }

            }
            GridView_Spec.DataSource = dtSpec;
            GridView_Spec.DataBind();
            GridView_Spec.Columns[1].Visible = false;
            
        }
    }
}