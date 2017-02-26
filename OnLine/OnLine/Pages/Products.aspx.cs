using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BackEndObjects;
using ActionLibrary;
using System.Data;
using System.Collections;
using System.IO;
using System.Web.UI.HtmlControls;

namespace OnLine.Pages
{
    public partial class Products : System.Web.UI.Page
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            Page.Theme = Session[SessionFactory.LOGGED_IN_USER_THEME].ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING] == null)
                    Response.Redirect("Login.aspx");
                else
                {
                    ((HtmlGenericControl)(Master.FindControl("Products"))).Attributes.Add("class", "active");

                    String entId = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();
                    Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];
                    Dictionary<String, Currency> allCurrList = (Dictionary<String, Currency>)Session[SessionFactory.CURRENCY_LIST];
                    String defaultCurr = Session[SessionFactory.MAIN_BUSINESS_ENTITY_DEFAULT_CURRENCY] != null ?
                       Session[SessionFactory.MAIN_BUSINESS_ENTITY_DEFAULT_CURRENCY].ToString() : "";
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

                    //((Menu)Master.FindControl("Menu1")).Items[3].Selected = true;
                    populateLogo();
                    CheckAccessToActions();
                    if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                    { //Full Access - no need to do any restriction
                        populateCategoryDropDown();
                        fillGrid();
                    }
                    else if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_PRODUCTS_SCREEN_VIEW])
                    {
                        populateCategoryDropDown();
                        fillGrid();
                    }
                    else
                    {
                        Label_Product_Screen_Access.Visible = true;
                        Label_Product_Screen_Access.Text = "You don't have access to view this page";
                    }
                }
            }
        }

        protected void loadCurrency(DropDownList DropDownList_Curr, String selectedVal)
        {
            Dictionary<String, Currency> allCurrList = (Dictionary<String, Currency>)Session[SessionFactory.CURRENCY_LIST];
            String defaultCurr = Session[SessionFactory.MAIN_BUSINESS_ENTITY_DEFAULT_CURRENCY].ToString();

            foreach (KeyValuePair<String, Currency> kvp in allCurrList)
            {
                ListItem lt = new ListItem();
                lt.Text = kvp.Value.getCurrencyName();
                lt.Value = kvp.Key;

                DropDownList_Curr.Items.Add(lt);

                if (selectedVal != null && !selectedVal.Equals("") && selectedVal.Equals(lt.Text)) //Currency name sent instead of id 
                    DropDownList_Curr.SelectedValue = lt.Value;
            }

            if (selectedVal == null || selectedVal.Equals(""))
                DropDownList_Curr.SelectedValue = defaultCurr;
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

        protected void populateCategoryDropDown()
        {
            String entId = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();

            Dictionary<String,ProductCategory> catDict=MainBusinessEntity.getProductDetailsforMainEntitybyIdDB(entId);

            ListItem firstVal = new ListItem();
            firstVal.Text = "_";
            firstVal.Value = "_";
            DropDownList_Category.Items.Add(firstVal);
            foreach (KeyValuePair<String, ProductCategory> kvp in catDict)
            {
                ListItem lt = new ListItem();
                lt.Text = ((ProductCategory)kvp.Value).getProductCategoryName();
                lt.Value = kvp.Key;
                
                DropDownList_Category.Items.Add(lt);                               
            }
            DropDownList_Category.SelectedValue = "_";
        }

        protected void populateChildCategoryDropDown(String catId)
        {
            Dictionary<String,Dictionary<String,String>> prodChildDict=(Dictionary<String,Dictionary<String,String>>)Session["ProductChildCategories"];
            
            DropDownList_Child_Category.Items.Clear();

            if (prodChildDict == null || prodChildDict.Count == 0 || !prodChildDict.ContainsKey(catId))
            {
                              

                Dictionary<String, ShopChildProdsInventory> childProdDict = ShopChildProdsInventory.
                    getAllShopChildProdObjsbyEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

                Dictionary<String, String> prodDictForCatId = new Dictionary<string, string>();
                Dictionary<String, String> dupcCheck = new Dictionary<string, string>();

                ListItem ltFirst = new ListItem();
                ltFirst.Text = "_";
                ltFirst.Value = "_";
                DropDownList_Child_Category.Items.Add(ltFirst);
                foreach (KeyValuePair<String, ShopChildProdsInventory> kvp in childProdDict)
                {
                    ShopChildProdsInventory prodObj = (ShopChildProdsInventory)kvp.Value;
                    if (prodObj.getProdCatId().Equals(catId.Trim()) ||
                        BackEndObjects.ProductCategory.getRootLevelParentCategoryDB(prodObj.getProdCatId()).ContainsKey(catId.Trim()))
                    {
                        ListItem lt = new ListItem();
                        lt.Text = ProductCategory.getProductCategorybyIdwoFeaturesDB(prodObj.getProdCatId()).getProductCategoryName();
                        lt.Value = prodObj.getProdCatId();                        

                        if (!dupcCheck.ContainsKey(lt.Value))
                        {
                            DropDownList_Child_Category.Items.Add(lt);
                            dupcCheck.Add(lt.Value, lt.Text);
                            prodDictForCatId.Add(lt.Text, lt.Value);
                        }
                    }
                }

                if (prodChildDict == null)
                    prodChildDict = new Dictionary<string, Dictionary<string, string>>();

                prodChildDict.Add(catId, prodDictForCatId);
                Session["ProductChildCategories"] = prodChildDict;
            }
            else
            {
                ListItem ltFirst = new ListItem();
                ltFirst.Text = "_";
                ltFirst.Value = "_";
                DropDownList_Child_Category.Items.Add(ltFirst);

                Dictionary<String, String> childDict = prodChildDict[catId];
                foreach (KeyValuePair<String, String> kvp in childDict)
                {
                    ListItem lt = new ListItem();
                    lt.Text = kvp.Key;
                    lt.Value = kvp.Value;
                    DropDownList_Child_Category.Items.Add(lt);
                }
            }
            DropDownList_Child_Category.SelectedValue = "_";
            
        }

        protected void DropDownList_Category_SelectedIndexChanged(object sender, EventArgs e)
        {
            populateChildCategoryDropDown(DropDownList_Category.SelectedValue);
            Session[SessionFactory.ALL_PRODUCT_SELECTED_PROD_CAT_FILTER] = DropDownList_Category.SelectedValue;
        }

        protected void fillGrid()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("ProdCatId");
            dt.Columns.Add("ProdName");
            dt.Columns.Add("Stock");
            dt.Columns.Add("msrmnt");
            dt.Columns.Add("srcPrice");
            dt.Columns.Add("listPrice");
            dt.Columns.Add("curr");

            Dictionary<String,ShopChildProdsInventory> chilDict=ShopChildProdsInventory.
                getAllShopChildProdObjsbyEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            Dictionary<String, Currency> allCurrList = (Dictionary<String, Currency>)Session[SessionFactory.CURRENCY_LIST];

            //This dictionary will be used by created product screen for duplicacy check
            Dictionary<String, String> allExistingProdDict = new Dictionary<String, String>();

            int counter=0;

            foreach (KeyValuePair<String, ShopChildProdsInventory> kvp in chilDict)
            {
                dt.Rows.Add();
                ShopChildProdsInventory childObj = (ShopChildProdsInventory)kvp.Value;

                dt.Rows[counter]["ProdCatId"] = childObj.getProdCatId();
                dt.Rows[counter]["ProdName"] = childObj.getProdName();
                dt.Rows[counter]["Stock"] = childObj.getQnty();
                dt.Rows[counter]["msrmnt"] = childObj.getMsrmntUnit();
                dt.Rows[counter]["srcPrice"] = childObj.getUnitSrcPrice();
                dt.Rows[counter]["listPrice"] = childObj.getUnitListPrice();
                dt.Rows[counter]["curr"] = childObj.getCurrency() != null &&
                        allCurrList.ContainsKey(childObj.getCurrency()) ?
                                   allCurrList[childObj.getCurrency()].getCurrencyName() : "";

                if (!allExistingProdDict.ContainsKey(childObj.getProdName()) && !childObj.getProdName().Equals(""))
                    allExistingProdDict.Add(childObj.getProdName(), childObj.getProdName());

                counter++;
            }
            dt.DefaultView.Sort = "ProdName" + " " + "ASC";
            Button_Audit_Prod.Enabled = false;
            Button_Notes_Prod.Enabled = false;

            GridView1.DataSource = dt.DefaultView.ToTable();
            GridView1.DataBind();            
            GridView1.Visible = true;
            GridView1.Columns[2].Visible = false;
            GridView1.SelectedIndex = -1;

            Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];
            if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_PRODUCT] &&
                !accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                GridView1.Columns[1].Visible = false;

            Session[SessionFactory.ALL_PRODUCT_CREATE_PRODUCT_EXISTING_NAMES] = allExistingProdDict;
            Session[SessionFactory.ALL_PRODUCT_PROD_DATA_GRID] = dt.DefaultView.ToTable();
        }

        protected void fillGrid(String catId)
        {
            if (catId != null && !catId.Equals("") && !catId.Equals("_"))
            {
                DataTable dt = new DataTable();

                dt.Columns.Add("ProdCatId");
                dt.Columns.Add("ProdName");
                dt.Columns.Add("Stock");
                dt.Columns.Add("msrmnt");
                dt.Columns.Add("srcPrice");
                dt.Columns.Add("listPrice");
                dt.Columns.Add("curr");

                Dictionary<String, ShopChildProdsInventory> chilDict = ShopChildProdsInventory.
                    getAllShopChildProdObjsbyEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                Dictionary<String, Currency> allCurrList = (Dictionary<String, Currency>)Session[SessionFactory.CURRENCY_LIST];

                int counter = 0;

                foreach (KeyValuePair<String, ShopChildProdsInventory> kvp in chilDict)
                {
                    ShopChildProdsInventory childObj = (ShopChildProdsInventory)kvp.Value;

                    if (childObj.getProdCatId().Equals(catId.Trim()) ||
                        BackEndObjects.ProductCategory.getRootLevelParentCategoryDB(childObj.getProdCatId()).ContainsKey(catId.Trim()))
                    {

                            dt.Rows.Add();

                            dt.Rows[counter]["ProdCatId"] = childObj.getProdCatId();
                            dt.Rows[counter]["ProdName"] = childObj.getProdName();
                            dt.Rows[counter]["Stock"] = childObj.getQnty();
                            dt.Rows[counter]["msrmnt"] = childObj.getMsrmntUnit();
                            dt.Rows[counter]["srcPrice"] = childObj.getUnitSrcPrice();
                            dt.Rows[counter]["listPrice"] = childObj.getUnitListPrice();
                            dt.Rows[counter]["curr"] = dt.Rows[counter]["curr"] = childObj.getCurrency() != null &&
                            allCurrList.ContainsKey(childObj.getCurrency()) ?
                                       allCurrList[childObj.getCurrency()].getCurrencyName() : "";

                            counter++;
                                                
                    }
                }

                Button_Audit_Prod.Enabled = false;
                Button_Notes_Prod.Enabled = false;
                GridView1.DataSource = dt;
                GridView1.DataBind();
                GridView1.Visible = true;
                GridView1.Columns[2].Visible = false;
                GridView1.SelectedIndex = -1;

                Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];
                if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_PRODUCT] &&
                    !accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                    GridView1.Columns[1].Visible = false;

                Session[SessionFactory.ALL_PRODUCT_PROD_DATA_GRID] = dt;
            }
            else
                fillGrid();
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session[SessionFactory.ALL_PRODUCT_SELECTED_PRODUCT_NAME] = ((Label)GridView1.SelectedRow.Cells[3].FindControl("Label_Name")).Text;
            ((RadioButton)GridView1.SelectedRow.Cells[0].FindControl("prod_radio")).Checked = true;
            Button_Audit_Prod.Enabled = true;
            Button_Notes_Prod.Enabled = true;
        }

        protected void GridView1_PageIndexChanged(object sender, EventArgs e)
        {
           
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.SelectedIndex = -1;
            GridView1.DataSource =(DataTable) Session[SessionFactory.ALL_PRODUCT_PROD_DATA_GRID];
            GridView1.DataBind();
            GridView1.Visible = true;
            GridView1.Columns[2].Visible = false;
            
            Button_Audit_Prod.Enabled = false;
            Button_Notes_Prod.Enabled = false;
        }

        protected void DropDownList_Child_Category_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session[SessionFactory.ALL_PRODUCT_SELECTED_PROD_CAT_FILTER] = DropDownList_Child_Category.SelectedValue;
        }

        protected void Button_Filter_All_Prod_Click(object sender, EventArgs e)
        {
            if (Session[SessionFactory.ALL_PRODUCT_SELECTED_PROD_CAT_FILTER] != null)
            {
                Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];
                if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_PRODUCTS_SCREEN_VIEW] ||
                    accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])                  
                fillGrid(Session[SessionFactory.ALL_PRODUCT_SELECTED_PROD_CAT_FILTER].ToString());
            }
        }



        protected void Button_Prod_Refresh_Click(object sender, EventArgs e)
        {
            fillGrid();
            GridView1.SelectedIndex = -1;
            Button_Audit_Prod.Enabled = false;
            Button_Notes_Prod.Enabled = false;
            Button_Prod_Refresh.Focus();
        }

        /// <summary>
        /// This method will check access to different buttons and enable/disable based on access
        /// </summary>
        protected void CheckAccessToActions()
        {
            Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

            if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_CREATE_PRODUCT] &&
                                !accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                Button_Create_Product.Enabled = false;

        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            /*Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];
            if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_PRODUCT]||
                accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
            {*/
                GridView1.EditIndex = e.NewEditIndex;
                GridView1.DataSource = (DataTable)Session[SessionFactory.ALL_PRODUCT_PROD_DATA_GRID];
                GridView1.DataBind();
            /*}
            else
            {
                GridView1.EditIndex = -1;
                GridView1.DataSource = (DataTable)Session[SessionFactory.ALL_PRODUCT_PROD_DATA_GRID];
                GridView1.DataBind();
                Label_Product_Grid_Access.Visible = true;
                Label_Product_Grid_Access.Text = "You dont have edit access to product records";
            }*/
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow gVR = GridView1.Rows[e.RowIndex];
             int index = GridView1.Rows[e.RowIndex].DataItemIndex;

            String catId=((Label)gVR.FindControl("Label_Hidden")).Text;
            String prevProdName=((Label)gVR.FindControl("Label_Name_Hidden")).Text;
            String prodName = ((TextBox)gVR.FindControl("TextBox_Prod_Name_Edit")).Text;
            String stock = ((TextBox)gVR.FindControl("TextBox_Invt_Stock_Edit")).Text;
            String listPrice = ((TextBox)gVR.FindControl("TextBox_Unit_Lst_Price_Edit")).Text;
            String srcPrice = ((TextBox)gVR.FindControl("TextBox_Unit_Src_Prc_Edit")).Text;
         ;

            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            whereCls.Add(BackEndObjects.ShopChildProdsInventory.SHOP_CHILD_PROD_INVNTRY_COL_PROD_CAT_ID, catId);
            whereCls.Add(BackEndObjects.ShopChildProdsInventory.SHOP_CHILD_PROD_INVNTRY_COL_PROD_NAME, prevProdName);

            Dictionary<String, String> targetVals = new Dictionary<string, string>();
            targetVals.Add(BackEndObjects.ShopChildProdsInventory.SHOP_CHILD_PROD_INVNTRY_COL_QNTY, stock);
            targetVals.Add(BackEndObjects.ShopChildProdsInventory.SHOP_CHILD_PROD_INVNTRY_COL_UNIT_LST_PRC, listPrice);
            targetVals.Add(BackEndObjects.ShopChildProdsInventory.SHOP_CHILD_PROD_INVNTRY_COL_UNIT_SRC_PRC, srcPrice);
            targetVals.Add(BackEndObjects.ShopChildProdsInventory.SHOP_CHILD_PROD_INVNTRY_COL_PROD_NAME, prodName);
            targetVals.Add(BackEndObjects.ShopChildProdsInventory.SHOP_CHILD_PROD_INVNTRY_COL_CURRENCY,
                ((DropDownList)gVR.FindControl("DropDownList_Curr")).SelectedValue);

            try
            {
                GridView1.EditIndex = -1;
                BackEndObjects.ShopChildProdsInventory.updateShopChildProdsInventoryDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
                DataTable dt = (DataTable)Session[SessionFactory.ALL_PRODUCT_PROD_DATA_GRID];

                 dt.Rows[index]["ProdName"] = prodName;
                dt.Rows[index]["srcPrice"] = srcPrice;
                dt.Rows[index]["listPrice"] = listPrice;
                dt.Rows[index]["Stock"] = stock;
                dt.Rows[index]["curr"] = ((DropDownList)gVR.FindControl("DropDownList_Curr")).SelectedItem.Text;

                Session[SessionFactory.ALL_PRODUCT_PROD_DATA_GRID] = dt;
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            catch (Exception ex)
            {

            }
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GridView1.DataSource = (DataTable)Session[SessionFactory.ALL_PRODUCT_PROD_DATA_GRID];
            GridView1.DataBind();
        }

        protected void Button_Audit_Prod_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/DispAudit.aspx";
            forwardString += "?contextId1=" + Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();
            forwardString += "&contextId2=" + ((Label)GridView1.SelectedRow.Cells[0].FindControl("Label_Hidden")).Text;
            forwardString += "&contextId3=" + "";
            forwardString += "&contextIdTable=" + "Shop_Child_Prdcts_Inventory";
            
            ScriptManager.RegisterStartupScript(this, typeof(string), "DispProdAudit", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=700');", true);

        }

        protected void Button_Notes_Prod_Click(object sender, EventArgs e)
        {
            String forwardString = "NoteDetails.aspx";

            String rfqId = ((Label)GridView1.SelectedRow.Cells[0].FindControl("Label_Name")).Text;

            forwardString += "?contextId=" + rfqId;

            ScriptManager.RegisterStartupScript(this, typeof(string), "ProdNotes",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=800,height=800,left=500,right=500');", true);

        }

 
        protected void LinkButton_Show_SO_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView1.SelectedIndex)
                GridView1.SelectRow(row.RowIndex);

            ((RadioButton)GridView1.SelectedRow.Cells[0].FindControl("prod_radio")).Checked = true;

            String forwardString = "/Pages/Popups/Product/AllProd_SO.aspx";

            String name = ((Label)GridView1.SelectedRow.Cells[0].FindControl("Label_Name")).Text;

            forwardString += "?prodName=" + name;

            ScriptManager.RegisterStartupScript(this, typeof(string), "ProdNotes",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=800,height=800,left=500,right=500');", true);

        }

        protected void Button_Prod_Refresh_Hidden_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session[SessionFactory.ALL_PRODUCT_PROD_DATA_GRID];
            GridView1.DataSource = dt;
            GridView1.DataBind();
            GridView1.SelectedIndex = -1;
        }

        protected void GridView_Prod_RadioSelect(object sender, EventArgs e)
        {
            RadioButton linkb = (RadioButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1)
                GridView1.SelectRow(row.RowIndex);
        }

        protected void LinkButton_Show_Spec_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView1.SelectedIndex)
                GridView1.SelectRow(row.RowIndex);

            ((RadioButton)GridView1.SelectedRow.Cells[0].FindControl("prod_radio")).Checked = true;

            String forwardString = "Popups/Product/AllProd_Specification.aspx";
            forwardString+="?prodCatId="+ ((Label)GridView1.SelectedRow.Cells[0].FindControl("Label_Hidden")).Text;
            ScriptManager.RegisterStartupScript(this, typeof(string), "DispSpecProd",
               "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=700,height=600,left=100,right=500,scrollbars=1');", true);
        }
        
        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                GridViewRow gVR = e.Row;
                loadCurrency(((DropDownList)gVR.FindControl("DropDownList_Curr")), ((Label)gVR.FindControl("Label_Curr_Edit")).Text);
            }
        }

    }
}