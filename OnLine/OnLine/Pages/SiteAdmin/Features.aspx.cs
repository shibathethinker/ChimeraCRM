using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using BackEndObjects;
using System.Data;
using DBConn;

namespace OnLine.Pages.SiteAdmin
{
    public partial class Features : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Dictionary<String, BackEndObjects.Features> ftList = BackEndObjects.Features.getAllFeatureswoSpecDB();
                foreach (KeyValuePair<String, BackEndObjects.Features> kvp in ftList)
                {
                    ListItem ltFeat = new ListItem();
                    ltFeat.Text = ((BackEndObjects.Features)kvp.Value).getFeatureName();
                    ltFeat.Value = kvp.Key.ToString();

                    DropDownList1.Items.Add(ltFeat);
                    DropDownList2.Items.Add(ltFeat);
                }
            }
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
        }

        protected void Button_Show_Feat_Click(object sender, EventArgs e)
        {
            FillGrid();
        }
        protected void FillGrid()
        {
            Dictionary<String, Specifications> specDict = BackEndObjects.Features.getSpecforFeatureDB(DropDownList1.SelectedValue);

            if (specDict.Count > 0)
            {
                GridView1.Visible = true;
                Button_Feat_Spec.Visible = true;

                DataTable dt = new DataTable();
                dt.Columns.Add("Hidden_Spec_Id");
                dt.Columns.Add("Specification Name");
                dt.Columns.Add("Dim1");
                dt.Columns.Add("Dim2");
                dt.Columns.Add("Dim3");
                
                int i = 0;
                foreach (KeyValuePair<String, Specifications> kvp in specDict)
                {
                    dt.Rows.Add();
                    Specifications specObj = kvp.Value;
                    dt.Rows[i]["Hidden_Spec_Id"] = specObj.getSpecId();
                    dt.Rows[i]["Specification Name"] = specObj.getSpecName();

                    ArrayList dimList = specObj.getDimensions();

                    dt.Rows[i]["Dim1"] = dimList[0].ToString();
                    dt.Rows[i]["Dim2"] = dimList[1].ToString();
                    dt.Rows[i]["Dim3"] = dimList[2].ToString();
                    i++;
                }

                GridView1.DataSource = dt;
                GridView1.DataBind();
                //int count = GridView1.Columns.Count;
                GridView1.HeaderRow.Cells[2].Visible = false;
                foreach (GridViewRow gVR in GridView1.Rows)
                    gVR.Cells[2].Visible = false;
               
            }
            else
            {
                GridView1.Visible = false;
                Button_Feat_Spec.Visible = false;
            }
        }

        protected void Button_Add_Feat_Click(object sender, EventArgs e)
        {
            BackEndObjects.Id IdGen = new BackEndObjects.Id();
            String ftId = IdGen.getNewId(BackEndObjects.Id.ID_TYPE_FEAT_STRING);

            BackEndObjects.Features ft = new BackEndObjects.Features();
            ft.setFeatureId(ftId);
            ft.setFeatureName(TextBox8.Text);
            ft.setWeightage(TextBox9.Text);

            try
            {
                BackEndObjects.Features.insertFeatureDB(ft);
                Label1.Visible = true;
                Label1.ForeColor = System.Drawing.Color.Green;
                Label1.Text = "Data Inserted Successfully";

                ListItem ltFeat = new ListItem();
                ltFeat.Value = ft.getFeatureId();
                ltFeat.Text = ft.getFeatureName();

                DropDownList1.Items.Add(ltFeat);
                DropDownList2.Items.Add(ltFeat);
                DropDownList1.SelectedIndex = 0;
                DropDownList2.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Label1.Visible = true;
                Label1.ForeColor = System.Drawing.Color.Red;
                Label1.Text = "Data Insertion Failed";
            }

        }

        protected void Button_Add_Spec_Click(object sender, EventArgs e)
        {
            BackEndObjects.Id IdGen = new BackEndObjects.Id();
            String specId = IdGen.getNewId(BackEndObjects.Id.ID_TYPE_SPEC_SRING);
            ArrayList dimn = new ArrayList();
            dimn.Add(TextBox11.Text);
            dimn.Add(TextBox12.Text);
            dimn.Add(TextBox13.Text);

            BackEndObjects.Specifications spec = new BackEndObjects.Specifications();
            spec.setSpecName(TextBox10.Text);
            spec.setDimensions(dimn);
            spec.setSpecId(specId);

            ArrayList specList=new ArrayList();
            specList.Add(spec);
            try
            {
                BackEndObjects.Features.insertSpecforFeatureDB(DropDownList2.SelectedValue, specList);
                Label2.Visible = true;
                Label2.Text = "Data Inserted Successfully";
                Label2.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                Label2.Visible = true;
                Label2.Text = "Data Insertion Failed";
                Label2.ForeColor = System.Drawing.Color.Green;
            }

        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            FillGrid();
            
            
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {

            GridView1.EditIndex = e.NewEditIndex;
            FillGrid();
        }

        protected void GridView1_RowUpdated(object sender, GridViewUpdatedEventArgs e)
        {
        }

        protected void GridView1_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {

        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            String ftId = DropDownList1.SelectedValue;

            GridViewRow gVR = GridView1.Rows[e.RowIndex];

            String specId = ((TextBox)gVR.Cells[2].Controls[0]).Text;
            String specName = ((TextBox)gVR.Cells[3].Controls[0]).Text;
            float dim1 = float.Parse(((TextBox)gVR.Cells[4].Controls[0]).Text);
            float dim2 = float.Parse(((TextBox)gVR.Cells[5].Controls[0]).Text);
            float dim3 = float.Parse(((TextBox)gVR.Cells[6].Controls[0]).Text);

            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            Dictionary<String, String> targetVals = new Dictionary<string, string>();

            whereCls.Add(Specifications.FEATURE_SPEC_COL_FEATURE_ID, ftId);
            whereCls.Add(Specifications.FEATURE_SPEC_COL_SPEC_ID, specId);

            targetVals.Add(Specifications.FEATURE_SPEC_COL_SPEC_NAME, specName);
            targetVals.Add(Specifications.FEATURE_SPEC_COL_DIM1, dim1.ToString());
            targetVals.Add(Specifications.FEATURE_SPEC_COL_DIM2, dim2.ToString());
            targetVals.Add(Specifications.FEATURE_SPEC_COL_DIM3, dim3.ToString());

            try
            {
                BackEndObjects.Features.updateSpecforFeatureDB(targetVals, whereCls, Connections.OPERATION_UPDATE);
                Label3.Visible = true;
                Label3.Text = "Data Modified Successfully";
                Label3.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                Label3.Visible = true;
                Label3.Text = "Data Modification Failed";
                Label3.ForeColor = System.Drawing.Color.Red;
            }
            
            GridView1.EditIndex = -1;
            FillGrid();
            
        }
    }
}