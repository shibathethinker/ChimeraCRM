using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BackEndObjects;
using System.Collections;
using ActionLibrary;
using System.Data;

namespace OnLine.Pages
{
    public partial class createProduct : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadProductCat();
                LoadUnitsOfMsrmnt();
                loadCurrency();
            }
        }

        protected void LoadProductCat()
        {
            Dictionary<String, ProductCategory> prodDict = BackEndObjects.ProductCategory.getAllParentCategory();
            ListItem firstItem = new ListItem();
            firstItem.Text = " ";
            firstItem.Value = "none";
            foreach (KeyValuePair<String, ProductCategory> kvp in prodDict)
            {
                ListItem ltProd = new ListItem();
                ltProd.Text = ((BackEndObjects.ProductCategory)kvp.Value).getProductCategoryName();
                ltProd.Value = kvp.Key.ToString();
                DropDownList_Level1.Items.Add(ltProd);
            }

            DropDownList_Level1.SelectedIndex = -1;
        }
        protected void LoadUnitsOfMsrmnt()
        {
            ArrayList allUnits = BackEndObjects.UnitOfMsrmnt.getAllMsrmntUnitsDB();
            for (int i = 0; i < allUnits.Count; i++)
            {
                ListItem ltUnit = new ListItem();
                ltUnit.Value = ((BackEndObjects.UnitOfMsrmnt)allUnits[i]).getUnitName();
                ltUnit.Text = ((BackEndObjects.UnitOfMsrmnt)allUnits[i]).getUnitName();
                DropDownList_Unit_Of_Msrmnt.Items.Add(ltUnit);
            }
        }
        protected void loadCurrency()
        {
            Dictionary<String, Currency> allCurrList = (Dictionary<String, Currency>)Session[SessionFactory.CURRENCY_LIST];
            String defaultCurr = Session[SessionFactory.MAIN_BUSINESS_ENTITY_DEFAULT_CURRENCY].ToString();

            foreach (KeyValuePair<String, Currency> kvp in allCurrList)
            {
                ListItem lt = new ListItem();
                lt.Text = kvp.Value.getCurrencyName();
                lt.Value = kvp.Key;

                DropDownList_Curr.Items.Add(lt);
                if (defaultCurr.Equals(lt.Value.Trim()))
                    DropDownList_Curr.SelectedValue = lt.Value;
            }
        }

        protected void DropDownList_Level1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Dictionary<String, ProductCategory> prodDict = BackEndObjects.ProductCategory.getAllChildCategoryDB(DropDownList_Level1.SelectedValue);
            DropDownList_Level2.Items.Clear();
            ListItem first = new ListItem();
            first.Text = " ";
            first.Value = "none";
            DropDownList_Level2.Items.Add(first);
            foreach (KeyValuePair<String, ProductCategory> kvp in prodDict)
            {
                ListItem lt = new ListItem();
                lt.Text = ((ProductCategory)kvp.Value).getProductCategoryName();
                lt.Value = ((ProductCategory)kvp.Value).getCategoryId();
                DropDownList_Level2.Items.Add(lt);
            }
            DropDownList_Level2.SelectedIndex = -1;
            Session[SessionFactory.CREATE_PRODUCT_SELECTED_PRODUCT_CAT] = DropDownList_Level1.SelectedValue;

        }

        protected void DropDownList_Level2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Dictionary<String, ProductCategory> prodDict = BackEndObjects.ProductCategory.getAllChildCategoryDB(DropDownList_Level2.SelectedValue);
            DropDownList_Level3.Items.Clear();
            ListItem first = new ListItem();
            first.Text = " ";
            first.Value = "none";
            DropDownList_Level3.Items.Add(first);
            foreach (KeyValuePair<String, ProductCategory> kvp in prodDict)
            {
                ListItem lt = new ListItem();
                lt.Text = ((ProductCategory)kvp.Value).getProductCategoryName();
                lt.Value = ((ProductCategory)kvp.Value).getCategoryId();
                DropDownList_Level3.Items.Add(lt);
            }
            DropDownList_Level3.SelectedIndex = -1;
            Session[SessionFactory.CREATE_PRODUCT_SELECTED_PRODUCT_CAT] = DropDownList_Level2.SelectedValue;
        }

        protected void DropDownList_Level3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session[SessionFactory.CREATE_PRODUCT_SELECTED_PRODUCT_CAT] = DropDownList_Level3.SelectedValue;
        }

        /// <summary>
        /// Populate feature and specification list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Buttin_Show_Spec_List_Click(object sender, EventArgs e)
        {
            fillGrid();

        }

        protected void fillGrid()
        {
            String selectedProdCatId = Session[SessionFactory.CREATE_PRODUCT_SELECTED_PRODUCT_CAT]!=null?
                Session[SessionFactory.CREATE_PRODUCT_SELECTED_PRODUCT_CAT].ToString():"";

            if (selectedProdCatId != null && !selectedProdCatId.Equals(""))
            {
                Dictionary<String, Features> featDict = BackEndObjects.ProductCategory.getFeatureforCategoryDB(selectedProdCatId);

                if (featDict.Count > 0)
                {
                    GridView1.Visible = true;
                    Label_Extra_Spec.Visible = true;
                    TextBox_Spec.Visible = true;
                    Label_Extra_Spec_upload.Visible = true;
                    FileUpload2.Visible = true;

                    DataTable dt = new DataTable();
                    dt.Columns.Add("Hidden_Feat_Id");
                    dt.Columns.Add("Feature");
                    dt.Columns.Add("From");
                    dt.Columns.Add("To");

                    int i = 0;
                    foreach (KeyValuePair<String, Features> kvp in featDict)
                    {
                        dt.Rows.Add();
                        Features ft = kvp.Value;
                        dt.Rows[i]["Hidden_Feat_Id"] = ft.getFeatureId();
                        dt.Rows[i]["Feature"] = ft.getFeatureName();

                        i++;
                    }

                    GridView1.DataSource = dt;
                    GridView1.DataBind();

                    GridView1.HeaderRow.Cells[1].Visible = false;
                    foreach (GridViewRow gVR in GridView1.Rows)
                    {
                        gVR.Cells[1].Visible = false;

                        Features ft = featDict[((Label)gVR.Cells[1].FindControl("Label_Hidden")).Text];
                        ArrayList specList = ft.getSpecifications();

                        for (int j = 0; j < specList.Count; j++)
                        {
                            BackEndObjects.Specifications specObj = (BackEndObjects.Specifications)specList[j];
                            ListItem ltSpec = new ListItem();
                            ltSpec.Text = specObj.getSpecName();
                            ltSpec.Value = specObj.getSpecId();
                            ((DropDownList)gVR.Cells[2].FindControl("DropDownList_Gridview1_From")).Items.Add(ltSpec);
                            ((DropDownList)gVR.Cells[3].FindControl("DropDownList_Gridview1_To")).Items.Add(ltSpec);
                        }

                    }
                }
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ArrayList rfqProdSrvList = (ArrayList)Session[SessionFactory.CREATE_PRODUCT_CHILD_PROD_SPEC_MAP];
            if (rfqProdSrvList == null)
                rfqProdSrvList = new ArrayList();

            BackEndObjects.ShopChildProdsSpecs prodSpec = new ShopChildProdsSpecs();

            prodSpec.setEntityId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            prodSpec.setFeatId(((Label)GridView1.SelectedRow.Cells[1].FindControl("Label_Hidden")).Text);
            prodSpec.setFromSpecId(((DropDownList)GridView1.SelectedRow.Cells[2].FindControl("DropDownList_Gridview1_From")).SelectedValue);
            prodSpec.setToSpecId(((DropDownList)GridView1.SelectedRow.Cells[3].FindControl("DropDownList_Gridview1_To")).SelectedValue);
            //rfqSpec.setSpecText(TextBox_Spec.Text);
            //prodSpec.setSpecText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            if (((FileUpload)GridView1.SelectedRow.Cells[4].FindControl("FileUpload_Spec")).HasFile)
                prodSpec.setFileStream((FileUpload)GridView1.SelectedRow.Cells[4].FindControl("FileUpload_Spec"));


            rfqProdSrvList.Add(prodSpec);

            GridView1.SelectedRow.Cells[0].Enabled = false;
            GridView1.SelectedRow.Cells[3].Enabled = false;
            GridView1.SelectedRow.Cells[4].Enabled = false;
            GridView1.SelectedRow.Cells[5].Enabled = false;
            GridView1.SelectedRow.Cells[0].FindControl("Image_Selected").Visible = true;
            Label_Selected_List.Text += "," + GridView1.SelectedRow.DataItemIndex;

            Session[SessionFactory.CREATE_PRODUCT_CHILD_PROD_SPEC_MAP] = rfqProdSrvList;
        }

        protected void getAddintionalProdSrvList()
        {
            ArrayList prodSpecList = (ArrayList)Session[SessionFactory.CREATE_PRODUCT_CHILD_PROD_SPEC_MAP];
            if (prodSpecList == null)
                prodSpecList = new ArrayList();

            BackEndObjects.ShopChildProdsSpecs rfqSpec = new ShopChildProdsSpecs();
            //rfqSpec.set(Session[SessionFactory.CREATE_PRODUCT_SELECTED_PRODUCT_CAT].ToString());
            rfqSpec.setEntityId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            rfqSpec.setFeatId("ft_dummy");
            rfqSpec.setFromSpecId("");
            rfqSpec.setToSpecId("");
            rfqSpec.setSpecText(TextBox_Spec.Text);
            //rfqSpec.setCreatedDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //if (User.Identity.Name != null)
                //rfqSpec.setCreatedUsr(User.Identity.Name);
            if (FileUpload2.HasFile)
                rfqSpec.setFileStream(FileUpload2);
            

            prodSpecList.Add(rfqSpec);

            Session[SessionFactory.CREATE_PRODUCT_CHILD_PROD_SPEC_MAP] = prodSpecList;
        }

        protected void createNewProduct()
        {
            Dictionary<String, String> allExistingProdDict = (Dictionary<String, String>)Session[SessionFactory.ALL_PRODUCT_CREATE_PRODUCT_EXISTING_NAMES];
            if (allExistingProdDict.ContainsKey(TextBox_Prod_Name.Text.Trim()))
            {
                Label_Status.Text = "Product Name Already Exists";
                Label_Status.Visible = true;
                Label_Status.ForeColor = System.Drawing.Color.Red;
            }
            else if (Session[SessionFactory.CREATE_PRODUCT_SELECTED_PRODUCT_CAT] == null || Session[SessionFactory.CREATE_PRODUCT_SELECTED_PRODUCT_CAT].ToString().Equals(""))
            {
                Label_Status.Text = "Must select one product category";
                Label_Status.Visible = true;
                Label_Status.ForeColor = System.Drawing.Color.Red;
            }
            else
            {

                if (!TextBox_Spec.Text.Equals(""))
                {
                    getAddintionalProdSrvList();
                    TextBox_Spec.Text = "";
                }

                ArrayList prodSpecList = (ArrayList)Session[SessionFactory.CREATE_PRODUCT_CHILD_PROD_SPEC_MAP];

                Dictionary<String, String> rSpecUniqnessValidation = new Dictionary<string, string>();
                String mainEntId = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();

                if (prodSpecList != null)
                    for (int i = 0; i < prodSpecList.Count; i++)
                    {

                        ShopChildProdsSpecs prodSpecObj = (ShopChildProdsSpecs)prodSpecList[i];

                        prodSpecObj.setProdName(TextBox_Prod_Name.Text);

                        if (rSpecUniqnessValidation.ContainsKey(prodSpecObj.getEntityId() + ":" + prodSpecObj.getProdName() + ":" + prodSpecObj.getFeatId()))
                            prodSpecList.RemoveAt(i);
                        else
                        {
                            rSpecUniqnessValidation.Add(prodSpecObj.getEntityId() + ":" + prodSpecObj.getProdName() + ":" + prodSpecObj.getFeatId(), prodSpecObj.getEntityId());
                            if (prodSpecObj != null && prodSpecObj.getFileStream() != null && prodSpecObj.getFileStream().HasFile)
                                prodSpecObj.setImgPathInFileStore(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                        }
                    }

                ShopChildProdsInventory childProdObj = new ShopChildProdsInventory();
                childProdObj.setCreatedBy(User.Identity.Name);
                childProdObj.setDateCreated(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                childProdObj.setEntityId(mainEntId);
                childProdObj.setMsrmntUnit(DropDownList_Unit_Of_Msrmnt.SelectedValue);
                childProdObj.setProdCatId(Session[SessionFactory.CREATE_PRODUCT_SELECTED_PRODUCT_CAT].ToString());
                childProdObj.setProdName(TextBox_Prod_Name.Text);
                childProdObj.setQnty((!TextBox_Stock.Text.Equals("")?float.Parse(TextBox_Stock.Text):0));
                childProdObj.setUnitListPrice(TextBox_List_Price.Text);
                childProdObj.setUnitSrcPrice(TextBox_Src_Price.Text);
                childProdObj.setCurrency(DropDownList_Curr.SelectedValue);

                try
                {
                    BackEndObjects.ShopChildProdsInventory.insertShopChildProdsInventoryDB(childProdObj);
                    if(prodSpecList!=null)
                    BackEndObjects.ShopChildProdsSpecs.insertShopChildProdsSpecsListDB(prodSpecList);

                    //Refresh the session variable with the newly added product name
                    allExistingProdDict.Add(childProdObj.getProdName(), childProdObj.getProdName());
                    Session[SessionFactory.ALL_PRODUCT_CREATE_PRODUCT_EXISTING_NAMES] = allExistingProdDict;

                    Dictionary<String, ProductCategory> existingProdDict = MainBusinessEntity.
                        getProductDetailsforMainEntitybyIdDB(mainEntId);

                    Label_Status.Text = "Product/Service Details Created Successfully";
                    Label_Status.Visible = true;
                    Label_Status.ForeColor = System.Drawing.Color.Green;
                    DropDownList_Level1.SelectedIndex = -1;
                    DropDownList_Level2.SelectedIndex = -1;
                    DropDownList_Level3.SelectedIndex = -1;


                    if (!existingProdDict.ContainsKey(DropDownList_Level1.SelectedValue))
                    {
                        ArrayList newMainProdCat = new ArrayList();
                        newMainProdCat.Add(DropDownList_Level1.SelectedValue);

                        MainBusinessEntity.insertProductDetailsforEntityDB(mainEntId, newMainProdCat);
                        Label_Status.Text += "...New Product category added to your product list";
                    }

                    DataTable dt = (DataTable)Session[SessionFactory.ALL_PRODUCT_PROD_DATA_GRID];
                    
                    dt.Rows.Add();
                    int count = dt.Rows.Count - 1;

                    dt.Rows[count]["ProdCatId"] = childProdObj.getProdCatId();
                    dt.Rows[count]["ProdName"] = childProdObj.getProdName();
                    dt.Rows[count]["Stock"] = childProdObj.getQnty();
                    dt.Rows[count]["msrmnt"] = childProdObj.getMsrmntUnit();
                    dt.Rows[count]["srcPrice"] = childProdObj.getUnitSrcPrice();
                    dt.Rows[count]["listPrice"] = childProdObj.getUnitListPrice();
                    dt.Rows[count]["curr"] = DropDownList_Curr.SelectedItem.Text;

                    Session[SessionFactory.ALL_PRODUCT_PROD_DATA_GRID] = dt;
                    ScriptManager.RegisterStartupScript(this, typeof(string), "RefreshProdGrid", "RefreshParent();", true);
                }
                catch (Exception ex)
                {

                    Label_Status.Text = "Product/Service Details Creation Failed";
                    Label_Status.Visible = true;
                    Label_Status.ForeColor = System.Drawing.Color.Red;
                }
                finally
                {
                    Session.Remove(SessionFactory.CREATE_PRODUCT_CHILD_PROD_SPEC_MAP);
                    Session.Remove(SessionFactory.CREATE_PRODUCT_SELECTED_PRODUCT_CAT);
                }
            }
        }

        protected void Button_Submit_Prod_Click(object sender, EventArgs e)
        {
            createNewProduct();
            Label_Selected_List.Text = "";
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            fillGrid();
        }

        protected void Button_Submit_Next_Click(object sender, EventArgs e)
        {
            createNewProduct();

            clearAllFields("Panel_Prod_Service_Det");
            clearAllFields("Panel_Prod_Srv_Qnty");
            clearAllFields("Panel_Price_Range");
            TextBox_Prod_Name.Text = "";
            Label_Extra_Spec.Visible = false;
            Label_Extra_Spec_upload.Visible = false;
            Label_Selected_List.Text = "";
            FileUpload2.Visible = false;
            TextBox_Spec.Visible = false;
        }

        protected void clearAllFields(String id)
        {
            Control myForm = FindControl(id);

            foreach (Control ctl in myForm.Controls)
            {
                if (ctl.GetType().ToString().Equals("System.Web.UI.WebControls.TextBox"))
                    ((TextBox)ctl).Text = "";
                if (ctl.GetType().ToString().Equals("System.Web.UI.WebControls.DropDownList"))
                    ((DropDownList)ctl).SelectedIndex = -1;
                if (ctl.GetType().ToString().Equals("System.Web.UI.WebControls.GridView"))
                { ((GridView)ctl).DataSource = null; ((GridView)ctl).Visible = false; }
            }

        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Normal) == DataControlRowState.Normal)
            {
                if (Label_Selected_List.Text.Length > 0)
                {
                    String[] selectedList = Label_Selected_List.Text.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    Dictionary<String, String> selectedIndices = new Dictionary<String, String>();
                    for (int i = 0; i < selectedList.Length; i++)
                        selectedIndices.Add(selectedList[i], selectedList[i]);

                    if (selectedIndices.ContainsKey(e.Row.DataItemIndex.ToString()))
                    {
                        e.Row.FindControl("Image_Selected").Visible = true;
                        e.Row.Cells[0].Enabled = false;
                        e.Row.Cells[3].Enabled = false;
                        e.Row.Cells[4].Enabled = false;
                        e.Row.Cells[5].Enabled = false;
                    }
                }
            }
        }
        

    }
}