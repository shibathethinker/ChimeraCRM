using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BackEndObjects;
using System.Data;
using DBConn;
using System.Collections;

namespace OnLine.Pages.SiteAdmin
{
    public partial class ProdCatHier : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Dictionary<String, ProductCategory> prodDict = ProductCategory.getAllParentCategory();

                foreach (KeyValuePair<String, ProductCategory> kvp in prodDict)
                {
                    Dictionary<String, ProductCategory> allcatDict = ProductCategory.getAllChildCategoryDB(((ProductCategory)kvp.Value).getCategoryId());

                    ListItem ltCat1 = new ListItem();
                    ltCat1.Text = ((ProductCategory)kvp.Value).getProductCategoryName();
                    ltCat1.Value = kvp.Key.ToString();
                    DropDownList1.Items.Add(ltCat1);
                    DropDownList2.Items.Add(ltCat1);

                    foreach (KeyValuePair<String, ProductCategory> kvp1 in allcatDict)
                    {
                        ListItem ltCat = new ListItem();
                        ltCat.Text = ((ProductCategory)kvp1.Value).getProductCategoryName();
                        ltCat.Value = kvp1.Key.ToString();
                        DropDownList1.Items.Add(ltCat);
                        DropDownList2.Items.Add(ltCat);
                    }

                }

                Dictionary<String, BackEndObjects.Features> featDict = BackEndObjects.Features.getAllFeatureswoSpecDB();
                foreach (KeyValuePair<String, BackEndObjects.Features> kvp in featDict)
                {
                    ListItem ltFeat = new ListItem();
                    ltFeat.Value = ((BackEndObjects.Features)kvp.Value).getFeatureId();
                    ltFeat.Text = ((BackEndObjects.Features)kvp.Value).getFeatureName();

                    ListBoxFeat.Items.Add(ltFeat);
                }
            }

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            fillGrid();
        }

        protected void fillGrid()
        {
            Dictionary<String, ProductCategory> childDict = ProductCategory.getAllChildCategoryDB(DropDownList1.SelectedValue);

            if (childDict.Count > 0)
            {
                GridView1.Visible = true;
                Button_Change_Parent_Cat.Visible = true;

                DataTable dt = new DataTable();
                dt.Columns.Add("Hidden_Cat_Id");
                dt.Columns.Add("Category Name");
                dt.Columns.Add("Visible");

                int i = 0;
                foreach (KeyValuePair<String, ProductCategory> kvp in childDict)
                {
                    dt.Rows.Add();
                    ProductCategory pH = kvp.Value;
                    dt.Rows[i]["Hidden_Cat_Id"] = pH.getCategoryId();
                    dt.Rows[i]["Category Name"] = pH.getProductCategoryName();
                    dt.Rows[i]["Visible"] = pH.getVisible();
                    i++;
                }

                GridView1.DataSource = dt;
                GridView1.DataBind();
                GridView1.HeaderRow.Cells[2].Visible = false;
                foreach (GridViewRow gVR in GridView1.Rows)
                    gVR.Cells[2].Visible = false;
            }
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            fillGrid();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            
            fillGrid();


            //((TextBox)gVR.Cells[4].Controls[0]).Text;
           
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            String parentCatId = DropDownList1.SelectedValue;

            GridViewRow gVR = GridView1.Rows[e.RowIndex];

            //((TextBox)gVR.Cells[2].Controls[0]).Text;
            String catId = ((Label)gVR.Cells[2].FindControl("Label3")).Text;

            String catName = ((Label)gVR.Cells[3].FindControl("Label4")).Text;
            String vis = ((DropDownList)gVR.Cells[4].FindControl("DropDownListVisible")).SelectedValue;
            
            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            Dictionary<String, String> targetVals = new Dictionary<string, string>();

            whereCls.Add(BackEndObjects.ProductCategory.PROD_CAT_COL_CAT_ID, catId);
            
            targetVals.Add(BackEndObjects.ProductCategory.PROD_CAT_COL_CATEGORY_NAME, catName);
            targetVals.Add(BackEndObjects.ProductCategory.PROD_CAT_COL_VIS, vis);
            
            try
            {
                BackEndObjects.ProductCategory.updateProductCategoryDB(targetVals, whereCls, Connections.OPERATION_UPDATE);
                Label1.Visible = true;
                Label1.Text = "Data Modified Successfully";
                Label1.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                Label1.Visible = true;
                Label1.Text = "Data Modification Failed";
                Label1.ForeColor = System.Drawing.Color.Red;
            }

            GridView1.EditIndex = -1;
            fillGrid();
        }

        protected void DropDownListVisible_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                GridViewRow gVR = e.Row;
                ListItem lt = new ListItem();
                lt.Text = "T"; lt.Value = "T";

                //String valTest = ((Label)gVR.FindControl("Label2")).Text;
                ((DropDownList)gVR.FindControl("DropDownListVisible")).Items.Add(lt);
                lt = new ListItem();
                lt.Text = "F"; lt.Value = "F";
                ((DropDownList)gVR.Cells[4].FindControl("DropDownListVisible")).Items.Add(lt);
                ((DropDownList)gVR.Cells[4].FindControl("DropDownListVisible")).SelectedIndex = -1;
            }
        }

        protected void Button_Add_Category_Click(object sender, EventArgs e)
        {
            String catName = TextBox1.Text;
            String parentCatId = DropDownList2.SelectedValue;
            BackEndObjects.ProductCategory catObj = new BackEndObjects.ProductCategory();
            catObj.setProductCategoryName(catName);
            catObj.setParentCategoryId(parentCatId);
            catObj.setVisible("T");

            String catId=new BackEndObjects.Id().getNewId(BackEndObjects.Id.ID_TYPE_PROD_CAT_STRING);
            catObj.setCategoryId(catId);

            ArrayList catList = new ArrayList();
            catList.Add(catObj);

            try
            {
                BackEndObjects.ProductCategory.insertProductCategoryDB(catList);

                int[] FeatIdList = ListBoxFeat.GetSelectedIndices();
                ArrayList catFeatObjList = new ArrayList();

                for (int i = 0; i < FeatIdList.Length; i++)
                {
                    String ftId = ListBoxFeat.Items[FeatIdList[i]].Value;
                    BackEndObjects.CategoryFeatureMap catFeatMapObj = new BackEndObjects.CategoryFeatureMap();
                    catFeatMapObj.setFeatId(ftId);
                    catFeatMapObj.setProdCatId(catId);

                    catFeatObjList.Add(catFeatMapObj);
                }
                BackEndObjects.ProductCategory.insertFeaturesforCategory(catFeatObjList);

                Label5.Visible = true;
                Label5.Text = "Data Inserted Successfully";
                Label5.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                Label5.Visible = true;
                Label5.Text = "Data Insertion Failed";
                Label5.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            String catId = ((Label)GridView1.Rows[e.RowIndex].FindControl("Label3")).Text;
            Dictionary<String,String> whereClause=new Dictionary<string,string>();
            whereClause.Add(ProductCategory.PROD_CAT_COL_CAT_ID,catId);

            ProductCategory.updateProductCategoryDB(new Dictionary<string, string>(), whereClause, Connections.OPERATION_DELETE);
            fillGrid();
        }
    }
}