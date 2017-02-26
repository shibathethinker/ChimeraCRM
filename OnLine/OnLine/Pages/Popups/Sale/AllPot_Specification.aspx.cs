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

namespace OnLine.Pages.Popups.Sale
{
    public partial class AllPot_Specification : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Session.Remove(SessionFactory.POTN_SPECIFICATION_EXISTING_PROD_LIST);
                fillGrid(null);
                String createMode = Label_Create_Mode.Text;
                if (!createMode.Equals(RFQDetails.CREATION_MODE_AUTO))
                {
                    LoadUnitsOfMsrmnt();
                    LoadProductCat();
                    loadProductList();
                }
            }
        }

        protected void loadProductList()
        {
            Dictionary<String, ShopChildProdsInventory> childDict = (Dictionary<String, ShopChildProdsInventory>)
    Session[SessionFactory.CREATE_INVOICE_MANUAL_PRODUCT_GRID_PRODUCT_LIST];

            if (childDict == null || childDict.Count == 0)
                childDict = ShopChildProdsInventory.
    getAllShopChildProdObjsbyEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());


            foreach (KeyValuePair<String, ShopChildProdsInventory> kvp in childDict)
            {
                ListItem lt = new ListItem();
                lt.Text = kvp.Key;
                lt.Value = kvp.Value.getUnitListPrice();
                DropDownList_Prod_List.Items.Add(lt);
            }
            ListItem firstItem = new ListItem();
            firstItem.Text = "_";
            firstItem.Value = "";
            DropDownList_Prod_List.Items.Add(firstItem);

            DropDownList_Prod_List.SelectedValue = "";
        }

        protected void LoadProductCat()
        {
            Dictionary<String, ProductCategory> prodDict = BackEndObjects.ProductCategory.getAllParentCategory();
            ListItem firstItem = new ListItem();
            firstItem.Text = "_";
            firstItem.Value = "_";
            DropDownList_Level1.Items.Add(firstItem);

            foreach (KeyValuePair<String, ProductCategory> kvp in prodDict)
            {
                ListItem ltProd = new ListItem();
                ltProd.Text = ((BackEndObjects.ProductCategory)kvp.Value).getProductCategoryName();
                ltProd.Value = kvp.Key.ToString();
                DropDownList_Level1.Items.Add(ltProd);
            }

            DropDownList_Level1.SelectedValue = "_";
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
            DropDownList_Unit_Of_Msrmnt.SelectedValue = "Numbers";
        }

        protected void fillGrid(DataTable dt)
        {
            ArrayList rfqProdList = (ArrayList)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_POTN_PROD_SRV];
            ArrayList rfqSpecList = (ArrayList)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_POTN_SPEC];

            Dictionary<String, String> existingProdList = new Dictionary<String, String>();

            float totalAmountFrom = 0, totalAmountTo = 0;

            String rfqId = Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_SELECTED_RFQ_ID].ToString();
            Label_Create_Mode.Text = BackEndObjects.RFQDetails.getRFQDetailsbyIdDB(rfqId).getCreateMode();

            if (dt == null || dt.Rows.Count == 0)
            {
                dt = new DataTable();

                dt.Columns.Add("Hidden");
                dt.Columns.Add("CategoryName");
                dt.Columns.Add("featureDataTable");
                dt.Columns.Add("FromQnty");
                dt.Columns.Add("ToQnty");
                dt.Columns.Add("FromPrice");
                dt.Columns.Add("ToPrice");
                dt.Columns.Add("msrmntUnit");
                dt.Columns.Add("prodName");
                //dt.Columns.Add("HiddenCatId");
                dt.Columns.Add("Quote");
                // dt.Columns.Add("QuoteUnit");
                dt.Columns.Add("Total");
                dt.Columns.Add("Audit");

                Dictionary<String, RFQResponseQuotes> rfqRespQuoteDict = BackEndObjects.RFQResponseQuotes.getAllResponseQuotesforRFQandResponseEntityDB(rfqId, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

                for (int i = 0; i < rfqProdList.Count; i++)
                {

                    BackEndObjects.RFQProdServQnty rfqProdObj = (BackEndObjects.RFQProdServQnty)rfqProdList[i];

                    dt.Rows.Add();
                    dt.Rows[i]["Hidden"] = rfqProdObj.getProdCatId();
                    existingProdList.Add(rfqProdObj.getProdCatId(), rfqProdObj.getProdCatId());
                    dt.Rows[i]["CategoryName"] = BackEndObjects.ProductCategory.getProductCategorybyIdwoFeaturesDB(rfqProdObj.getProdCatId()).getProductCategoryName();
                    //dt.Rows[i]["featureDataTable"] = dtSpec;
                    dt.Rows[i]["FromQnty"] = rfqProdObj.getFromQnty();
                    dt.Rows[i]["ToQnty"] = rfqProdObj.getToQnty();
                    dt.Rows[i]["FromPrice"] = rfqProdObj.getFromPrice();
                    dt.Rows[i]["ToPrice"] = rfqProdObj.getToPrice();
                    dt.Rows[i]["msrmntUnit"] = rfqProdObj.getMsrmntUnit();

                    RFQResponseQuotes rfqRespObj = new RFQResponseQuotes();

                    try
                    {
                        rfqRespObj = rfqRespQuoteDict[rfqProdObj.getProdCatId()];

                        dt.Rows[i]["prodName"] = rfqRespObj.getProductName();
                        String respQuote = rfqRespObj.getQuote().Equals("") ? "0" : rfqRespObj.getQuote(); ;
                        dt.Rows[i]["Quote"] = respQuote;
                        //dt.Rows[i]["QuoteUnit"] = rfqRespObj.getUnitName();
                        totalAmountFrom = totalAmountFrom + (rfqProdObj.getFromQnty() * float.Parse(respQuote));
                        totalAmountTo = totalAmountTo + (rfqProdObj.getToQnty() * float.Parse(respQuote));
                        dt.Rows[i]["Total"] = (rfqProdObj.getFromQnty() * float.Parse(respQuote)) + " to " + (rfqProdObj.getToQnty() * float.Parse(respQuote));
                    }
                    catch (KeyNotFoundException e)
                    {
                        //If no response given to this RFQ by this entity so far
                        dt.Rows[i]["Quote"] = "0";
                        dt.Rows[i]["Total"] = "0";
                    }

                }
                Session[SessionFactory.POTN_SPECIFICATION_EXISTING_PROD_LIST] = existingProdList;
            }

            GridView1.DataSource = dt;
            GridView1.DataBind();
            GridView1.Columns[2].Visible = false;
            bool editAccess = true;

            Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

            if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_POTENTIAL] &&
!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
            {
                GridView1.Columns[0].Visible = false;
                GridView1.Columns[1].Visible = false;
                editAccess = false;
                Button_Add_To_Req.Enabled = false;
                Buttin_Show_Spec_List.Enabled = false;
            }

            String createMode = Label_Create_Mode.Text;
            foreach (GridViewRow gVR in GridView1.Rows)
            {
                DataTable dtSpec = new DataTable();
                dtSpec.Columns.Add("Hidden");
                dtSpec.Columns.Add("FeatName");
                dtSpec.Columns.Add("SpecText");
                dtSpec.Columns.Add("FromSpec");
                dtSpec.Columns.Add("ToSpec");
                dtSpec.Columns.Add("imgName");

                int rowCount = 0;
                for (int j = 0; j < rfqSpecList.Count; j++)
                {

                    BackEndObjects.RFQProductServiceDetails rfqSpecObj = (BackEndObjects.RFQProductServiceDetails)rfqSpecList[j];

                    if (rfqSpecObj.getPrdCatId().Equals(((Label)gVR.Cells[1].FindControl("Label_Hidden")).Text))
                    {
                        dtSpec.Rows.Add();
                        String featName = BackEndObjects.Features.getFeaturebyIdwoSpecDB(rfqSpecObj.getFeatId()).getFeatureName();
                        dtSpec.Rows[rowCount]["Hidden"] = rfqSpecObj.getFeatId();
                        dtSpec.Rows[rowCount]["FeatName"] = (featName != null ? featName.Trim() : "");
                        dtSpec.Rows[rowCount]["SpecText"] = rfqSpecObj.getSpecText();
                        if (!rfqSpecObj.getFromSpecId().Equals(""))
                            dtSpec.Rows[rowCount]["FromSpec"] = BackEndObjects.Specifications.getSpecificationDetailbyIdDB(rfqSpecObj.getFromSpecId()).getSpecName();
                        if (!rfqSpecObj.getToSpecId().Equals(""))
                            dtSpec.Rows[rowCount]["ToSpec"] = BackEndObjects.Specifications.getSpecificationDetailbyIdDB(rfqSpecObj.getToSpecId()).getSpecName();

                        String[] imgPath =  rfqSpecObj.getImgPath()!=null?rfqSpecObj.getImgPath().Split(new String[] { "\\" }, StringSplitOptions.RemoveEmptyEntries):null;
                        dtSpec.Rows[rowCount]["imgName"] = (imgPath != null && imgPath.Length > 0 ? imgPath[imgPath.Length - 1] : "N\\A");

                        rowCount++;
                    }
                }
                ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).DataSource = dtSpec;
                ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).DataBind();
                ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).Visible = true;
                ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).Columns[1].Visible = false;
                ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).Columns[0].Visible = editAccess;
                //Dont show the edit button if the Potential is Auto created
                
                if (createMode.Equals(BackEndObjects.RFQDetails.CREATION_MODE_AUTO))
                    ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).Columns[0].Visible = false;
            }

            if (createMode.Equals(RFQDetails.CREATION_MODE_AUTO))
            {
                Panel2.Visible = false;
                GridView1.Columns[1].Visible = false;
            }

            GridView1.Visible = true;
            Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_POTN_PROD_SRV_DATATABLE] = dt;

            Label_Total.Text = totalAmountFrom + " to " + totalAmountTo;
        }

        protected void GridView1_Inner_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView grdInner = ((GridView)sender);
            grdInner.SelectedIndex = -1;
            grdInner.PageIndex = e.NewPageIndex;

            GridViewRow gvRowParent = ((GridView)sender).Parent.Parent as GridViewRow;

            DataTable dtSpec = new DataTable();
            dtSpec.Columns.Add("Hidden");
            dtSpec.Columns.Add("FeatName");
            dtSpec.Columns.Add("SpecText");
            dtSpec.Columns.Add("FromSpec");
            dtSpec.Columns.Add("ToSpec");
            dtSpec.Columns.Add("imgName");

            ArrayList rfqSpecList = (ArrayList)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_POTN_SPEC];
            int rowCount = 0;
            for (int j = 0; j < rfqSpecList.Count; j++)
            {

                BackEndObjects.RFQProductServiceDetails rfqSpecObj = (BackEndObjects.RFQProductServiceDetails)rfqSpecList[j];

                if (rfqSpecObj.getPrdCatId().Equals(((Label)gvRowParent.Cells[0].FindControl("Label_Hidden")).Text))
                {
                    dtSpec.Rows.Add();
                    String featName = BackEndObjects.Features.getFeaturebyIdwoSpecDB(rfqSpecObj.getFeatId()).getFeatureName();
                    dtSpec.Rows[rowCount]["Hidden"] = rfqSpecObj.getFeatId();
                    dtSpec.Rows[rowCount]["FeatName"] = (featName != null ? featName.Trim() : "");
                    dtSpec.Rows[rowCount]["SpecText"] = rfqSpecObj.getSpecText();
                    if (!rfqSpecObj.getFromSpecId().Equals(""))
                        dtSpec.Rows[rowCount]["FromSpec"] = BackEndObjects.Specifications.getSpecificationDetailbyIdDB(rfqSpecObj.getFromSpecId()).getSpecName();
                    if (!rfqSpecObj.getToSpecId().Equals(""))
                        dtSpec.Rows[rowCount]["ToSpec"] = BackEndObjects.Specifications.getSpecificationDetailbyIdDB(rfqSpecObj.getToSpecId()).getSpecName();

                    String[] imgPath = rfqSpecObj.getImgPath()!=null?rfqSpecObj.getImgPath().Split(new String[] { "\\" }, StringSplitOptions.RemoveEmptyEntries):null;
                    dtSpec.Rows[rowCount]["imgName"] = (imgPath != null && imgPath.Length > 0 ? imgPath[imgPath.Length - 1] : "N\\A");

                    rowCount++;
                }
            }
            grdInner.DataSource = dtSpec;
            grdInner.DataBind();
            grdInner.Visible = true;
            //Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_SHOW_SPEC_INNER_GRID_DATA] = dtSpec;

        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            fillGrid();
        }

        protected void Link_Feat_Img_Show_Command(object sender, CommandEventArgs e)
        {
            GridView grdInner = (GridView)((LinkButton)sender).Parent.Parent.Parent.Parent;
            Int32 SelectedRowIndex = Convert.ToInt32(e.CommandArgument) % grdInner.PageSize;
            //SelectedRowIndex = ((GridViewRow)grdInner.Rows).RowIndex;

            int selectedIndexParent = ((GridViewRow)grdInner.Parent.Parent).RowIndex;
            String prodCatId = ((Label)((GridView)grdInner.Parent.Parent.Parent.Parent).Rows[selectedIndexParent].Cells[0].FindControl("Label_Hidden")).Text;
            String featId = ((Label)((GridView)grdInner).Rows[SelectedRowIndex].Cells[1].FindControl("Label_Hidden")).Text;
            //String prodCatId = row.Cells[1].Text;

            ActionLibrary.ImageContextFactory icObj = new ImageContextFactory();

            icObj.setParentContextName(ActionLibrary.ImageContextFactory.PARENT_CONTEXT_RFQ);
            icObj.setParentContextValue(Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_SELECTED_RFQ_ID].ToString());
            icObj.setDestinationContextName(ActionLibrary.ImageContextFactory.DESTINATION_CONTEXT_FEAT_FOR_PARENT_RFQ);

            Dictionary<String, String> childContextDict = new Dictionary<string, string>();
            childContextDict.Add(ActionLibrary.ImageContextFactory.CHILD_CONTEXT_PRODCAT_ID, prodCatId);
            childContextDict.Add(ActionLibrary.ImageContextFactory.CHILD_CONTEXT_FEAT_ID, featId);
            icObj.setChildContextObjects(childContextDict);

            Session[SessionFactory.DISP_IMAGE_CONTEXT_FACTORY_OBJ] = icObj;
            //Server.Transfer("/Pages/DispImage.aspx", true);
            Response.Redirect("/Pages/DispImage.aspx");
        }


        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GridView1.DataSource = (DataTable)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_POTN_PROD_SRV_DATATABLE];
            GridView1.DataBind();

            //Now generate the inner gridview
            generateInnerGrid();
        }

        protected void generateInnerGrid()
        {
            ArrayList rfqSpecList = (ArrayList)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_POTN_SPEC];
            String createMode = Label_Create_Mode.Text;

            foreach (GridViewRow gVR in GridView1.Rows)
            {
                DataTable dtSpec = new DataTable();
                dtSpec.Columns.Add("Hidden");
                dtSpec.Columns.Add("FeatName");
                dtSpec.Columns.Add("SpecText");
                dtSpec.Columns.Add("FromSpec");
                dtSpec.Columns.Add("ToSpec");
                dtSpec.Columns.Add("imgName");

                int rowCount = 0;
                for (int j = 0; j < rfqSpecList.Count; j++)
                {

                    BackEndObjects.RFQProductServiceDetails rfqSpecObj = (BackEndObjects.RFQProductServiceDetails)rfqSpecList[j];

                    if (rfqSpecObj.getPrdCatId().Equals(((Label)gVR.Cells[1].FindControl("Label_Hidden")).Text))
                    {
                        dtSpec.Rows.Add();
                        String featName = BackEndObjects.Features.getFeaturebyIdwoSpecDB(rfqSpecObj.getFeatId()).getFeatureName();
                        dtSpec.Rows[rowCount]["Hidden"] = rfqSpecObj.getFeatId();
                        dtSpec.Rows[rowCount]["FeatName"] = (featName != null ? featName.Trim() : "");
                        dtSpec.Rows[rowCount]["SpecText"] = rfqSpecObj.getSpecText();
                        if (!rfqSpecObj.getFromSpecId().Equals(""))
                            dtSpec.Rows[rowCount]["FromSpec"] = BackEndObjects.Specifications.getSpecificationDetailbyIdDB(rfqSpecObj.getFromSpecId()).getSpecName();
                        if (!rfqSpecObj.getToSpecId().Equals(""))
                            dtSpec.Rows[rowCount]["ToSpec"] = BackEndObjects.Specifications.getSpecificationDetailbyIdDB(rfqSpecObj.getToSpecId()).getSpecName();

                        String[] imgPath = rfqSpecObj.getImgPath().Split(new String[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                        dtSpec.Rows[rowCount]["imgName"] = (imgPath != null && imgPath.Length > 0 ? imgPath[imgPath.Length - 1] : "N\\A");

                        rowCount++;
                    }
                }
                ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).DataSource = dtSpec;
                ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).DataBind();
                ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).Visible = true;
                ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).Columns[1].Visible = false;

                if (createMode.Equals(BackEndObjects.RFQDetails.CREATION_MODE_AUTO))
                    ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).Columns[0].Visible = false;
            }
                      

           
        }

        /// <summary>
        /// Creates and stores the datatable for only the inner grid which is being edited
        /// </summary>
        /// <param name="gridInner"></param>
        /// <param name="parentGridRow"></param>
        protected void loadInnerGridForEdit(GridView gridInner, GridViewRow parentGridRow)
        {
            GridViewRow gvRowParent = parentGridRow;
            GridView grdInner = gridInner;

            DataTable dtSpec = new DataTable();
            dtSpec.Columns.Add("Hidden");
            dtSpec.Columns.Add("FeatName");
            dtSpec.Columns.Add("SpecText");
            dtSpec.Columns.Add("FromSpec");
            dtSpec.Columns.Add("ToSpec");
            dtSpec.Columns.Add("imgName");
            
            ArrayList potnSpecList = (ArrayList)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_POTN_SPEC];
            int rowCount = 0;
            for (int j = 0; j < potnSpecList.Count; j++)
            {

                BackEndObjects.RFQProductServiceDetails rfqSpecObj = (BackEndObjects.RFQProductServiceDetails)potnSpecList[j];

                if (rfqSpecObj.getPrdCatId().Equals(((Label)gvRowParent.Cells[0].FindControl("Label_Hidden")).Text))
                {
                    dtSpec.Rows.Add();
                    String featName = BackEndObjects.Features.getFeaturebyIdwoSpecDB(rfqSpecObj.getFeatId()).getFeatureName();
                    dtSpec.Rows[rowCount]["Hidden"] = rfqSpecObj.getFeatId();
                    dtSpec.Rows[rowCount]["FeatName"] = (featName != null ? featName.Trim() : "");
                    dtSpec.Rows[rowCount]["SpecText"] = rfqSpecObj.getSpecText();
                    if (!rfqSpecObj.getFromSpecId().Equals(""))
                        dtSpec.Rows[rowCount]["FromSpec"] = BackEndObjects.Specifications.getSpecificationDetailbyIdDB(rfqSpecObj.getFromSpecId()).getSpecName();
                    if (!rfqSpecObj.getToSpecId().Equals(""))
                        dtSpec.Rows[rowCount]["ToSpec"] = BackEndObjects.Specifications.getSpecificationDetailbyIdDB(rfqSpecObj.getToSpecId()).getSpecName();

                    String[] imgPath = rfqSpecObj.getImgPath().Split(new String[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                    dtSpec.Rows[rowCount]["imgName"] = (imgPath != null && imgPath.Length > 0 ? imgPath[imgPath.Length - 1] : "N\\A");

                    rowCount++;
                }
            }
            grdInner.DataSource = dtSpec;
            grdInner.DataBind();

            Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_POTN_SPEC_EDIT_INNER_GRID] = dtSpec;
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DataTable dt = (DataTable)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_POTN_PROD_SRV_DATATABLE];
            GridViewRow gVR = GridView1.Rows[e.RowIndex];

            String newQuote = ((TextBox)gVR.Cells[10].FindControl("TextBox_Quote")).Text;
            String oldQuote = ((Label)gVR.Cells[10].FindControl("Label_Quote_Prev")).Text;

            String fromQnty = ((TextBox)gVR.Cells[4].FindControl("TextBox_From_Qnty_Edit")).Text;
            String fromQntyPrev = ((Label)gVR.Cells[4].FindControl("Label_From_Qnty_Prev")).Text;

            String toQnty = ((TextBox)gVR.Cells[5].FindControl("TextBox_To_Qnty_Edit")).Text;
            String toQntyPrev = ((Label)gVR.Cells[5].FindControl("Label_To_Qnty_Prev")).Text;

            String msrmntUnit = ((Label)gVR.Cells[9].FindControl("Label_Msrmnt")).Text;

            String fromPrice = ((TextBox)gVR.Cells[6].FindControl("TextBox_From_Price_Edit")).Text;
            String toPrice = ((TextBox)gVR.Cells[7].FindControl("TextBox_To_Price_Edit")).Text;
            
            dt.Rows[e.RowIndex]["FromQnty"] = fromQnty;
            dt.Rows[e.RowIndex]["ToQnty"] = toQnty;
            dt.Rows[e.RowIndex]["FromPrice"] = fromPrice;
            dt.Rows[e.RowIndex]["ToPrice"] = toPrice;
            dt.Rows[e.RowIndex]["prodName"] = ((DropDownList)gVR.Cells[9].FindControl("DropDownList_Prod_Name_List")).SelectedItem.Text;

            dt.Rows[e.RowIndex]["Quote"] = newQuote;
            //Adjust the total of this row
            dt.Rows[e.RowIndex]["Total"] = (float.Parse(newQuote) * float.Parse(fromQnty)).ToString() + " to " + (float.Parse(newQuote) * float.Parse(toQnty)).ToString();
            //Adjust the grand total
            String[] splitDelim = { "to" };
            String[] splitData = Label_Total.Text.Split(splitDelim, StringSplitOptions.RemoveEmptyEntries);

            String grandTotalFrom = (float.Parse(splitData[0]) - (float.Parse(oldQuote) * float.Parse(fromQntyPrev)) + (float.Parse(newQuote) * float.Parse(fromQnty))).ToString();
            String grandTotalTo = (float.Parse(splitData[1]) - (float.Parse(oldQuote) * float.Parse(toQntyPrev)) + (float.Parse(newQuote) * float.Parse(toQnty))).ToString(); Label_Total.Text = grandTotalFrom + " to " + grandTotalTo;

            float potnAmnt = (float.Parse(grandTotalFrom) +float.Parse(grandTotalTo)) / 2;

            //reset the previous label to hold the latest set value
            //((Label)gVR.Cells[10].FindControl("Label_Quote_Prev")).Text = newQuote;

            GridView1.EditIndex = -1;
            GridView1.DataSource = dt;
            GridView1.DataBind();
            Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_POTN_PROD_SRV_DATATABLE] = dt;

            RFQResponse tempResp = RFQResponse.getRFQResponseforRFQIdandResponseEntityIdDB(Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_SELECTED_RFQ_ID].ToString(),
                Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            if (tempResp != null && tempResp.getRFQId() != null && !tempResp.getRFQId().Equals("")) //Update if a response already exists
            {
                bool update = true;

                if (oldQuote == null || oldQuote.Equals("") || oldQuote.Equals("0"))
                {
                    Dictionary<String, RFQResponseQuotes> respQuoteDict = RFQResponseQuotes.getAllResponseQuotesforRFQandResponseEntityandProdCatIdDB(Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_SELECTED_RFQ_ID].ToString(),
                Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), ((Label)gVR.Cells[1].FindControl("Label_Hidden")).Text);

                    if (respQuoteDict == null || respQuoteDict.Count == 0)//Insert is required - there is no existing response quote entry for this product category
                    {
                        update = false;

                        BackEndObjects.RFQResponseQuotes respQuoteObj = new RFQResponseQuotes();
                        respQuoteObj.setRFQId(Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_SELECTED_RFQ_ID].ToString());
                        respQuoteObj.setPrdCatId(((Label)gVR.Cells[1].FindControl("Label_Hidden")).Text);
                        respQuoteObj.setQuote(newQuote);
                        respQuoteObj.setResponseEntityId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                        respQuoteObj.setResponseUsrId(User.Identity.Name);
                        respQuoteObj.setUnitName(msrmntUnit);
                        respQuoteObj.setProductName(((DropDownList)gVR.Cells[9].FindControl("DropDownList_Prod_Name_List")).SelectedItem.Text);

                        RFQResponseQuotes.insertRFQResponseQuotesDB(respQuoteObj);
                    }
                    else
                        update = true;
                }

                Dictionary<String, String> whereCls = new Dictionary<string, string>();
                Dictionary<String, String> targetVals = new Dictionary<string, string>();

                if (update)
                {
                    whereCls.Add(BackEndObjects.RFQResponseQuotes.RFQ_RES_QUOTE_COL_RFQ_ID, Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_SELECTED_RFQ_ID].ToString());
                    whereCls.Add(BackEndObjects.RFQResponseQuotes.RFQ_RES_QUOTE_COL_RESP_COMP_ID, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                    whereCls.Add(BackEndObjects.RFQResponseQuotes.RFQ_RES_QUOTE_COL_PROD_CAT, ((Label)gVR.Cells[1].FindControl("Label_Hidden")).Text);

                    targetVals.Add(BackEndObjects.RFQResponseQuotes.RFQ_RES_QUOTE_COL_QUOTE, ((TextBox)gVR.Cells[10].FindControl("TextBox_Quote")).Text);
                    targetVals.Add(BackEndObjects.RFQResponseQuotes.RFQ_RES_QUOTE_COL_PRODUCT_NAME, ((DropDownList)gVR.Cells[9].FindControl("DropDownList_Prod_Name_List")).SelectedItem.Text);

                    BackEndObjects.RFQResponseQuotes.updateRFQResponseQuotesDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
                }
                //Now update the potential amount
                whereCls = new Dictionary<string, string>();
                targetVals = new Dictionary<string, string>();
                whereCls.Add(BackEndObjects.RFQShortlisted.RFQ_SHORTLIST_COL_RFQ_ID, Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_SELECTED_RFQ_ID].ToString());
                whereCls.Add(BackEndObjects.RFQShortlisted.RFQ_SHORTLIST_COL_RESP_ENT_ID, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

                targetVals.Add(BackEndObjects.RFQShortlisted.RFQ_SHORTLIST_COL_POTENTIAL_AMNT, potnAmnt.ToString());
                BackEndObjects.RFQShortlisted.updateRFQShortListedEntryDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);

                if (Label_Create_Mode.Text.Equals(BackEndObjects.RFQDetails.CREATION_MODE_MANUAL))
                {
                    whereCls.Clear();
                    targetVals.Clear();

                    targetVals.Add(BackEndObjects.RFQProdServQnty.RFQ_PROD_SRV_COL_FROM_PRICE, fromPrice);
                    targetVals.Add(BackEndObjects.RFQProdServQnty.RFQ_PROD_SRV_COL_TO_PRICE, toPrice);
                    targetVals.Add(BackEndObjects.RFQProdServQnty.RFQ_PROD_SRV_COL_FROM_QNTY, fromQnty);
                    targetVals.Add(BackEndObjects.RFQProdServQnty.RFQ_PROD_SRV_COL_TO_QNTY, toQnty);

                    whereCls.Add(BackEndObjects.RFQProdServQnty.RFQ_PROD_SRV_COL_RFQ_ID, Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_SELECTED_RFQ_ID].ToString());
                    whereCls.Add(BackEndObjects.RFQProdServQnty.RFQ_PROD_SRV_COL_PROD_SRV_ID, ((Label)gVR.Cells[1].FindControl("Label_Hidden")).Text);

                    BackEndObjects.RFQProdServQnty.updateRFQProductServiceQuantityDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);

                    ArrayList rfqProdList = (ArrayList)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_POTN_PROD_SRV];
                    //Update the session variable with the latest list
                    for (int i = 0; i < rfqProdList.Count; i++)
                    {
                        BackEndObjects.RFQProdServQnty prodSrvQntyObj = (BackEndObjects.RFQProdServQnty)rfqProdList[i];
                        if (prodSrvQntyObj.getProdCatId().Equals(((Label)gVR.Cells[1].FindControl("Label_Hidden")).Text))
                        {
                            prodSrvQntyObj.setFromPrice(fromPrice);
                            prodSrvQntyObj.setFromQnty(float.Parse(fromQnty));
                            prodSrvQntyObj.setToPrice(toPrice);
                            prodSrvQntyObj.setToQnty(float.Parse(toQnty));

                            rfqProdList.RemoveAt(i);
                            rfqProdList.Add(prodSrvQntyObj);
                            Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_POTN_PROD_SRV] = rfqProdList;
                            break;
                        }
                    }
                }
            }
            else
            {
                BackEndObjects.RFQResponse leadRespObj = new RFQResponse();
                leadRespObj.setRespEntityId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                leadRespObj.setRFQId(Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_SELECTED_RFQ_ID].ToString());
                leadRespObj.setRespDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                BackEndObjects.RFQResponseQuotes respQuoteObj = new RFQResponseQuotes();
                respQuoteObj.setRFQId(Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_SELECTED_RFQ_ID].ToString());
                respQuoteObj.setPrdCatId(((Label)gVR.Cells[1].FindControl("Label_Hidden")).Text);
                respQuoteObj.setQuote(newQuote);
                respQuoteObj.setResponseEntityId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                respQuoteObj.setResponseUsrId(User.Identity.Name);
                //respQuoteObj.setUnitName(Session[SessionFactory.CREATE_LEAD_QUOTE_UNIT].ToString());
                respQuoteObj.setUnitName(msrmntUnit);
                respQuoteObj.setProductName(((DropDownList)gVR.Cells[9].FindControl("DropDownList_Prod_Name_List")).SelectedItem.Text);

                RFQResponse.insertRFQResponseDB(leadRespObj);
                RFQResponseQuotes.insertRFQResponseQuotesDB(respQuoteObj);

            }//This else block will never be required for potential entries as the reponse entry would always be there already
            generateInnerGrid();
        }

        protected void LinkButton_All_Comm_Click(object sender, EventArgs e)
        {
            String sourceEnt = BackEndObjects.RFQDetails.getRFQDetailsbyIdDB(Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_SELECTED_RFQ_ID].ToString()).getEntityId();
            String destEnt = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();

            String forwardString = "/Pages/DispComm.aspx";
            forwardString += "?contextId=" + Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_SELECTED_RFQ_ID].ToString() +
                "&source=" + sourceEnt +
                "&destination=" + destEnt;

            Server.Transfer(forwardString);

        }

        protected void LinkButton_Show_Audit_Command(object sender, CommandEventArgs e)
        {
            String rfqId = Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_SELECTED_RFQ_ID].ToString();
            String forwardString = "/Pages/DispAudit.aspx";
            forwardString += "?contextId1=" + rfqId;
            forwardString += "&contextId2=" + Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();
            forwardString += "&contextId3=" + "";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispAuditPotnSpec", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=700');", true);

        }

        protected void DropDownList_Prod_Name_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow gVR = (GridViewRow)((DropDownList)sender).Parent.Parent;
            ((TextBox)gVR.FindControl("TextBox_Quote")).Text = ((DropDownList)sender).SelectedValue;

        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                GridViewRow gVR = e.Row;
                String rfqId = Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_SELECTED_RFQ_ID].ToString();
                String createMode = Label_Create_Mode.Text;

                if (createMode.Equals(RFQDetails.CREATION_MODE_MANUAL))
                {
                    ((TextBox)gVR.FindControl("TextBox_From_Qnty_Edit")).Enabled = true;
                    ((TextBox)gVR.FindControl("TextBox_To_Qnty_Edit")).Enabled = true;
                    ((TextBox)gVR.FindControl("TextBox_From_Price_Edit")).Enabled = true;
                    ((TextBox)gVR.FindControl("TextBox_To_Price_Edit")).Enabled = true;
                }

                Dictionary<String, ShopChildProdsInventory> childDict = (Dictionary<String, ShopChildProdsInventory>)
                    Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_SPECIFICATION_PRODUCT_LIST];
                
                if (childDict == null || childDict.Count == 0)
                {
                    childDict = ShopChildProdsInventory.
        getAllShopChildProdObjsbyEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                    Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_SPECIFICATION_PRODUCT_LIST] = childDict;
                }

                String existingProdName = ((Label)gVR.FindControl("Label_Prod_Name_Edit")).Text;
                String selectedValue = "";

                ListItem ltStart = new ListItem();
                ltStart.Text = "_";
                ltStart.Value = "_";
                ((DropDownList)gVR.FindControl("DropDownList_Prod_Name_List")).Items.Add(ltStart);

                foreach (KeyValuePair<String, ShopChildProdsInventory> kvp in childDict)
                {
                    ListItem lt = new ListItem();
                    lt.Text = kvp.Key;
                    lt.Value = kvp.Value.getUnitListPrice();
                    ((DropDownList)gVR.FindControl("DropDownList_Prod_Name_List")).Items.Add(lt);

                    if (kvp.Key.Equals(existingProdName, StringComparison.OrdinalIgnoreCase))
                        selectedValue = kvp.Value.getUnitListPrice();

                }

                if (!selectedValue.Equals(""))
                    ((DropDownList)gVR.FindControl("DropDownList_Prod_Name_List")).SelectedValue = selectedValue;
                else
                    ((DropDownList)gVR.FindControl("DropDownList_Prod_Name_List")).SelectedValue = "_";


            }
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GridView1.DataSource = (DataTable)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_POTN_PROD_SRV_DATATABLE];
            GridView1.DataBind();
            generateInnerGrid();
        }

        protected void GridView1_Inner_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView grdInner = ((GridView)sender);
            grdInner.EditIndex = -1;

            grdInner.DataSource = (DataTable)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_POTN_SPEC_EDIT_INNER_GRID];
            grdInner.DataBind();
        }

        protected void GridView1_Inner_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DataTable dt = (DataTable)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_POTN_SPEC_EDIT_INNER_GRID];

            GridView grdInner = ((GridView)sender);
            GridViewRow grdInnerRow = grdInner.Rows[e.RowIndex];
            GridViewRow gvRowParent = ((GridView)sender).Parent.Parent as GridViewRow;

            String featId = ((Label)grdInnerRow.Cells[0].FindControl("Label_Hidden")).Text;
            String prodCatId = ((Label)gvRowParent.Cells[0].FindControl("Label_Hidden")).Text;

            String fromSpecText = ((DropDownList)grdInner.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_From_Spec_Edit")).SelectedItem.Text;
            String fromSpecId = ((DropDownList)grdInner.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_From_Spec_Edit")).SelectedValue;
            String ToSpecText = ((DropDownList)grdInner.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_To_Spec_Edit")).SelectedItem.Text;
            String ToSpecId = ((DropDownList)grdInner.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_To_Spec_Edit")).SelectedValue;

            String specText = ((TextBox)grdInner.Rows[e.RowIndex].Cells[0].FindControl("TextBox_SpecText_Edit")).Text;

            int index = grdInner.Rows[e.RowIndex].DataItemIndex;
            dt.Rows[index]["SpecText"] = specText;
            dt.Rows[index]["FromSpec"] = fromSpecText;
            dt.Rows[index]["ToSpec"] = ToSpecText;

            String updatedImgPath = "";

            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            Dictionary<String, String> targetVals = new Dictionary<string, string>();

            whereCls.Add(BackEndObjects.RFQProductServiceDetails.RFQ_PROD_COL_FEAT_ID, featId);
            whereCls.Add(BackEndObjects.RFQProductServiceDetails.RFQ_PROD_COL_PROD_ID, prodCatId);
            whereCls.Add(BackEndObjects.RFQProductServiceDetails.RFQ_PROD_COL_RFQ_ID,
                Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_SELECTED_RFQ_ID].ToString());

            if (!((FileUpload)grdInnerRow.Cells[0].FindControl("FileUpload_Image")).HasFile)
            {
                targetVals.Add(BackEndObjects.RFQProductServiceDetails.RFQ_PROD_COL_FROM_SPEC_ID, fromSpecId);
                targetVals.Add(BackEndObjects.RFQProductServiceDetails.RFQ_PROD_COL_TO_SPEC_ID, ToSpecId);
                targetVals.Add(BackEndObjects.RFQProductServiceDetails.RFQ_PROD_COL_SPEC_TEXT, specText);

                BackEndObjects.RFQProductServiceDetails.updateRFQProductServiceDetails(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
            }
            else
            {
                //Remove the entry and re-create
                BackEndObjects.RFQProductServiceDetails.updateRFQProductServiceDetails(targetVals, whereCls, DBConn.Connections.OPERATION_DELETE);

                BackEndObjects.RFQProductServiceDetails specObj = new RFQProductServiceDetails();
                specObj.setRFQId(Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_SELECTED_RFQ_ID].ToString());
                specObj.setFeatId(featId);
                specObj.setFileStream(((FileUpload)grdInner.Rows[e.RowIndex].Cells[0].FindControl("FileUpload_Image")));
                specObj.setFromSpecId(fromSpecId);
                specObj.setSpecText(specText);
                specObj.setToSpecId(ToSpecId);
                specObj.setPrdCatId(prodCatId);
                specObj.setImgPathInFileStore(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

                BackEndObjects.RFQProductServiceDetails.insertRFQProductServiceDetails(specObj);
                String[] imgPath = specObj.getImgPath().Split(new String[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                dt.Rows[index]["imgName"] = (imgPath != null && imgPath.Length > 0 ? imgPath[imgPath.Length - 1] : "N\\A");
                updatedImgPath = specObj.getImgPath();
            }

            ArrayList rfqSpecList = (ArrayList)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_POTN_SPEC];
            for (int j = 0; j < rfqSpecList.Count; j++)
            {

                BackEndObjects.RFQProductServiceDetails rfqSpecObj = (BackEndObjects.RFQProductServiceDetails)rfqSpecList[j];

                if (rfqSpecObj.getPrdCatId().Equals(prodCatId) && rfqSpecObj.getFeatId().Equals(featId))
                {
                    rfqSpecObj.setFromSpecId(fromSpecId);
                    rfqSpecObj.setToSpecId(ToSpecId);
                    rfqSpecObj.setSpecText(specText);
                    if (!updatedImgPath.Equals(""))
                        rfqSpecObj.setImgPath(updatedImgPath);
                    break;
                }
            }

            Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_POTN_SPEC_EDIT_INNER_GRID] = dt;
            Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_POTN_SPEC] = rfqSpecList;

            grdInner.EditIndex = -1;
            grdInner.DataSource = dt;
            grdInner.DataBind();
        }

        protected void GridView1_Inner_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView grdInner = ((GridView)sender);
            grdInner.EditIndex = e.NewEditIndex;

            GridViewRow gvRowParent = ((GridView)sender).Parent.Parent as GridViewRow;

            loadInnerGridForEdit(grdInner, gvRowParent);
        }

        protected void GridView1_Inner_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                GridViewRow gVR = e.Row;

                String featId = ((Label)gVR.Cells[0].FindControl("Label_Hidden")).Text;
                Dictionary<String, Specifications> specDict = BackEndObjects.Features.getSpecforFeatureDB(featId);

                String fromSpecName = ((Label)gVR.Cells[0].FindControl("Label_From_Spec")).Text;
                String toSpecName = ((Label)gVR.Cells[0].FindControl("Label_ToSpec")).Text;

                foreach (KeyValuePair<String, Specifications> kvp in specDict)
                {
                    Specifications specObj = kvp.Value;

                    ListItem ltSpec = new ListItem();
                    ltSpec.Text = specObj.getSpecName();
                    ltSpec.Value = specObj.getSpecId();

                    ((DropDownList)gVR.Cells[0].FindControl("DropDownList_From_Spec_Edit")).Items.Add(ltSpec);
                    ((DropDownList)gVR.Cells[0].FindControl("DropDownList_To_Spec_Edit")).Items.Add(ltSpec);

                    if (specObj.getSpecName().Equals(fromSpecName))
                        ((DropDownList)gVR.Cells[0].FindControl("DropDownList_From_Spec_Edit")).SelectedValue = specObj.getSpecId();
                    if (specObj.getSpecName().Equals(toSpecName))
                        ((DropDownList)gVR.Cells[0].FindControl("DropDownList_To_Spec_Edit")).SelectedValue = specObj.getSpecId();
                }

            }
        }
        
        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView2.PageIndex = e.NewPageIndex;
            GridView2.SelectedIndex = -1;
            fillGrid();
        }

        protected void fillGrid()
        {
            String selectedProdCatId = "";
            if (Session[SessionFactory.UPDATE_POTN_SELECTED_PRODUCT_CAT] != null && !Session[SessionFactory.UPDATE_POTN_SELECTED_PRODUCT_CAT].Equals(""))
                selectedProdCatId = Session[SessionFactory.UPDATE_POTN_SELECTED_PRODUCT_CAT].ToString();
            if (!selectedProdCatId.Equals(""))
            {
                Dictionary<String, Features> featDict = BackEndObjects.ProductCategory.getFeatureforCategoryDB(selectedProdCatId);

                if (featDict.Count > 0)
                {
                    GridView2.Visible = true;
                    Label_Extra_Spec.Visible = true;
                    TextBox_Spec.Visible = true;
                    Label_Extra_Spec_upload.Visible = true;
                    FileUpload_Extra_Spec.Visible = true;

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

                    GridView2.DataSource = dt;
                    GridView2.DataBind();

                    GridView2.HeaderRow.Cells[1].Visible = false;
                    foreach (GridViewRow gVR in GridView2.Rows)
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
                            ((DropDownList)gVR.Cells[2].FindControl("DropDownList_GridView1_From")).Items.Add(ltSpec);
                            ((DropDownList)gVR.Cells[3].FindControl("DropDownList_GridView1_To")).Items.Add(ltSpec);
                        }

                    }
                }
            }
        }

        protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ArrayList POTNSpecList = (ArrayList)Session[SessionFactory.UPDATE_POTN_SELECTED_POTN_SPEC_MAP];
            String fileName = null;

            if (POTNSpecList == null)
                POTNSpecList = new ArrayList();

            BackEndObjects.RFQProductServiceDetails POTNSpec = new RFQProductServiceDetails();
            POTNSpec.setPrdCatId(Session[SessionFactory.UPDATE_POTN_SELECTED_PRODUCT_CAT].ToString());
            POTNSpec.setFeatId(((Label)GridView2.SelectedRow.Cells[1].FindControl("Label_Hidden")).Text);
            POTNSpec.setFromSpecId(((DropDownList)GridView2.SelectedRow.Cells[2].FindControl("DropDownList_GridView1_From")).SelectedValue);
            POTNSpec.setToSpecId(((DropDownList)GridView2.SelectedRow.Cells[3].FindControl("DropDownList_GridView1_To")).SelectedValue);
            //POTNSpec.setSpecText(TextBox_Spec.Text);
            POTNSpec.setCreatedDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            if (User.Identity.Name != null)
                POTNSpec.setCreatedUsr(User.Identity.Name);
            if (((FileUpload)GridView2.SelectedRow.Cells[3].FindControl("FileUpload_Spec")).HasFile)
            {
                POTNSpec.setFileStream((FileUpload)GridView2.SelectedRow.Cells[3].FindControl("FileUpload_Spec"));
                fileName = ((FileUpload)GridView2.SelectedRow.Cells[3].FindControl("FileUpload_Spec")).FileName;
                ((Label)GridView2.SelectedRow.Cells[0].FindControl("Label_File_Name")).Text = fileName;
                ((Label)GridView2.SelectedRow.Cells[0].FindControl("Label_File_Name")).Visible = true;
                ((FileUpload)GridView2.SelectedRow.Cells[0].FindControl("FileUpload_Spec")).Visible = false;

            }

            POTNSpecList.Add(POTNSpec);


            GridView2.SelectedRow.Cells[0].Enabled = false;
            GridView2.SelectedRow.Cells[3].Enabled = false;
            GridView2.SelectedRow.Cells[4].Enabled = false;
            GridView2.SelectedRow.Cells[5].Enabled = false;


            GridView2.SelectedRow.Cells[0].FindControl("Image_Selected").Visible = true;
            Label_Selected_List.Text += "," + GridView2.SelectedRow.DataItemIndex;
            Session[SessionFactory.UPDATE_POTN_SELECTED_POTN_SPEC_MAP] = POTNSpecList;
        }

        protected void GridView2_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }

        protected void getAddintionalProdSrvList()
        {
            ArrayList POTNProdSrvList = (ArrayList)Session[SessionFactory.UPDATE_POTN_SELECTED_POTN_SPEC_MAP];
            if (POTNProdSrvList == null)
                POTNProdSrvList = new ArrayList();

            BackEndObjects.RFQProductServiceDetails POTNSpec = new RFQProductServiceDetails();
            POTNSpec.setPrdCatId(Session[SessionFactory.UPDATE_POTN_SELECTED_PRODUCT_CAT].ToString());
            POTNSpec.setFeatId("ft_dummy");
            POTNSpec.setFromSpecId("");
            POTNSpec.setToSpecId("");
            POTNSpec.setSpecText(TextBox_Spec.Text);
            POTNSpec.setCreatedDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            if (User.Identity.Name != null)
                POTNSpec.setCreatedUsr(User.Identity.Name);
            if (FileUpload_Extra_Spec.HasFile)
                POTNSpec.setFileStream(FileUpload_Extra_Spec);


            POTNProdSrvList.Add(POTNSpec);

            Session[SessionFactory.UPDATE_POTN_SELECTED_POTN_SPEC_MAP] = POTNProdSrvList;
        }

        protected void Button_Add_To_RFQ_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> existingFeatList = (Dictionary<String, String>)Session[SessionFactory.POTN_SPECIFICATION_EXISTING_PROD_LIST];
            if (DropDownList_Level1.SelectedValue.Equals("_"))
            {
                Label_Status.Text = "Please select one product category";
                Label_Status.ForeColor = System.Drawing.Color.Red;
                Label_Status.Visible = true;
            }
            else if (existingFeatList != null && existingFeatList.ContainsKey(DropDownList_Level3.SelectedValue))
            {
                Label_Feat_Exists.Visible = true;
                Label_Feat_Exists.Text = "This product category already added to this Potential. You can delete from the above list and re-enter";
                Label_Feat_Exists.ForeColor = System.Drawing.Color.Red;
                Label_Feat_Exists.Focus();
            }
            else
            {
                Label_Feat_Exists.Visible = false;
                String POTNId = Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_SELECTED_RFQ_ID].ToString();

                if (!TextBox_Spec.Text.Equals(""))
                {
                    getAddintionalProdSrvList();
                    TextBox_Spec.Text = "";
                }

                ArrayList POTNSpecObjList = (ArrayList)Session[SessionFactory.UPDATE_POTN_SELECTED_POTN_SPEC_MAP];

                Dictionary<String, String> POTNSpecUniqnessValidation = new Dictionary<string, string>();

                if (POTNSpecObjList != null)
                    for (int i = 0; i < POTNSpecObjList.Count; i++)
                    {
                        BackEndObjects.RFQProductServiceDetails POTNSpecObj = (BackEndObjects.RFQProductServiceDetails)POTNSpecObjList[i];
                        POTNSpecObj.setRFQId(POTNId);

                        if (POTNSpecUniqnessValidation.ContainsKey(POTNSpecObj.getPrdCatId() + ":" + POTNSpecObj.getFeatId()))
                            POTNSpecObjList.RemoveAt(i);//Remove the current POTNuirement spec object from the list - otherwise it will cause exception at DB layer while inserting
                        else
                        {
                            POTNSpecUniqnessValidation.Add(POTNSpecObj.getPrdCatId() + ":" + POTNSpecObj.getFeatId(), "");
                            if (POTNSpecObj.getFileStream() != null)
                                POTNSpecObj.setImgPathInFileStore(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                        }
                    }

                BackEndObjects.RFQProdServQnty POTNProdSrvObj = new BackEndObjects.RFQProdServQnty();
                POTNProdSrvObj.setRFQId(POTNId);
                POTNProdSrvObj.setProdCatId(Session[SessionFactory.UPDATE_POTN_SELECTED_PRODUCT_CAT].ToString());
                POTNProdSrvObj.setFromPrice(TextBox_Price_Range_From.Text);
                POTNProdSrvObj.setFromQnty(float.Parse(TextBox_Prod_Qnty_From.Text));
                POTNProdSrvObj.setMsrmntUnit(DropDownList_Unit_Of_Msrmnt.SelectedValue);
                POTNProdSrvObj.setToPrice(TextBox_Price_Range_To.Text);
                POTNProdSrvObj.setToQnty(float.Parse(TextBoxrod_Qnty_To.Text));

                BackEndObjects.RFQResponseQuotes leadRespQuoteObjs = new RFQResponseQuotes();
                leadRespQuoteObjs.setPrdCatId(Session[SessionFactory.UPDATE_POTN_SELECTED_PRODUCT_CAT].ToString());
                leadRespQuoteObjs.setQuote(TextBox_Quote_Amnt.Text);
                leadRespQuoteObjs.setResponseEntityId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                leadRespQuoteObjs.setResponseUsrId(User.Identity.Name);
                //leadRespQuoteObjs.setUnitName(Session[SessionFactory.CREATE_POTN_QUOTE_UNIT].ToString());
                leadRespQuoteObjs.setUnitName(DropDownList_Unit_Of_Msrmnt.SelectedValue);
                leadRespQuoteObjs.setProductName((!TextBox_Prod_Name.Text.Equals("") ? TextBox_Prod_Name.Text.Trim() : DropDownList_Prod_List.SelectedItem.Text));
                leadRespQuoteObjs.setRFQId(POTNId);

                try
                {
                    BackEndObjects.RFQProdServQnty.insertRFQProductServiceQuantityDetailsDB(POTNProdSrvObj);
                    BackEndObjects.RFQResponseQuotes.insertRFQResponseQuotesDB(leadRespQuoteObjs);
                    if (POTNSpecObjList != null && POTNSpecObjList.Count > 0)
                        BackEndObjects.RFQProductServiceDetails.insertRFQProductServiceDetails(POTNSpecObjList);

                    ArrayList POTNProdList = (ArrayList)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_POTN_PROD_SRV];
                    ArrayList POTNSpecList = (ArrayList)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_POTN_SPEC];
                    if (POTNProdList == null)
                        POTNProdList = new ArrayList();
                    if (POTNSpecList == null)
                        POTNSpecList = new ArrayList();

                    POTNProdList.Add(POTNProdSrvObj);

                    if (POTNSpecObjList != null)
                        for (int i = 0; i < POTNSpecObjList.Count; i++)
                        {
                            BackEndObjects.RFQProductServiceDetails specObj = (BackEndObjects.RFQProductServiceDetails)POTNSpecObjList[i];
                            POTNSpecList.Add(specObj);
                        }
                    Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_POTN_PROD_SRV] = POTNProdList;
                    Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_POTN_SPEC] = POTNSpecList;
                    fillGrid(null);

                    Label_Status.Text = "Details Added Succeddfully";
                    Label_Status.ForeColor = System.Drawing.Color.Green;
                    Label_Status.Visible = true;
                    //Refresh the parent grid
                }
                catch (Exception ex)
                {
                    Label_Status.Text = "Addition Failed";
                    Label_Status.ForeColor = System.Drawing.Color.Red;
                    Label_Status.Visible = true;
                }
                finally
                {
                    Session.Remove(SessionFactory.UPDATE_POTN_SELECTED_POTN_SPEC_MAP);
                    Session.Remove(SessionFactory.UPDATE_POTN_SELECTED_PRODUCT_CAT);
                    clearAllFields("Panel_Prod_Srv_Qnty");
                    clearAllFields("Panel_Price_Range");
                    clearAllFields("Panel_Prod_Service_Det");
                    DropDownList_Level1.SelectedValue = "_";
                    DropDownList_Level2.SelectedIndex = -1;
                    DropDownList_Level3.SelectedIndex = -1;
                    Label_Selected_List.Text = "";
                }
            }
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

        protected void Buttin_Show_Spec_List_Click(object sender, EventArgs e)
        {
                                                            Dictionary<String, String> existingFeatList = (Dictionary<String, String>)Session[SessionFactory.POTN_SPECIFICATION_EXISTING_PROD_LIST];
                                                            if (existingFeatList != null && existingFeatList.ContainsKey(DropDownList_Level3.SelectedValue))
                                                            {
                                                                Label_Feat_Exists.Visible = true;
                                                                Label_Feat_Exists.Text = "This product category already added to this Potential. You can delete from the above list and re-enter";
                                                                Label_Feat_Exists.ForeColor = System.Drawing.Color.Red;
                                                            }
                                                            else
                                                            {
                                                                Label_Feat_Exists.Visible = false;
                                                                fillFeatureGrid();
                                                            }
        }

        protected void fillFeatureGrid()
        {
            String selectedProdCatId = "";
            if (Session[SessionFactory.UPDATE_POTN_SELECTED_PRODUCT_CAT] != null && !Session[SessionFactory.UPDATE_POTN_SELECTED_PRODUCT_CAT].Equals(""))
                selectedProdCatId = Session[SessionFactory.UPDATE_POTN_SELECTED_PRODUCT_CAT].ToString();
            if (!selectedProdCatId.Equals(""))
            {
                Dictionary<String, Features> featDict = BackEndObjects.ProductCategory.getFeatureforCategoryDB(selectedProdCatId);

                if (featDict.Count > 0)
                {
                    GridView2.Visible = true;
                    Label_Extra_Spec.Visible = true;
                    TextBox_Spec.Visible = true;
                    Label_Extra_Spec_upload.Visible = true;
                    FileUpload_Extra_Spec.Visible = true;

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

                    GridView2.DataSource = dt;
                    GridView2.DataBind();

                    GridView2.HeaderRow.Cells[1].Visible = false;
                    foreach (GridViewRow gVR in GridView2.Rows)
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
                            ((DropDownList)gVR.Cells[2].FindControl("DropDownList_GridView1_From")).Items.Add(ltSpec);
                            ((DropDownList)gVR.Cells[3].FindControl("DropDownList_GridView1_To")).Items.Add(ltSpec);
                        }

                    }
                }
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
            Session[SessionFactory.UPDATE_POTN_SELECTED_PRODUCT_CAT] = DropDownList_Level1.SelectedValue;

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
            Session[SessionFactory.UPDATE_POTN_SELECTED_PRODUCT_CAT] = DropDownList_Level2.SelectedValue;
        }

        protected void DropDownList_Level3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session[SessionFactory.UPDATE_POTN_SELECTED_PRODUCT_CAT] = DropDownList_Level3.SelectedValue;
        }

        protected void BindInnterGrid()
        {
            ArrayList POTNProdList = (ArrayList)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_POTN_PROD_SRV];
            ArrayList POTNSpecList = (ArrayList)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_POTN_SPEC];

            foreach (GridViewRow gVR in GridView1.Rows)
            {
                DataTable dtSpec = new DataTable();
                dtSpec.Columns.Add("Hidden");
                dtSpec.Columns.Add("FeatName");
                dtSpec.Columns.Add("SpecText");
                dtSpec.Columns.Add("FromSpec");
                dtSpec.Columns.Add("ToSpec");
                dtSpec.Columns.Add("imgName");
                int rowCount = 0;
                for (int j = 0; j < POTNSpecList.Count; j++)
                {

                    BackEndObjects.RFQProductServiceDetails POTNSpecObj = (BackEndObjects.RFQProductServiceDetails)POTNSpecList[j];

                    if (POTNSpecObj.getPrdCatId().Equals(((Label)gVR.Cells[1].FindControl("Label_Hidden")).Text))
                    {
                        dtSpec.Rows.Add();
                        String featName = BackEndObjects.Features.getFeaturebyIdwoSpecDB(POTNSpecObj.getFeatId()).getFeatureName();
                        dtSpec.Rows[rowCount]["Hidden"] = POTNSpecObj.getFeatId();
                        dtSpec.Rows[rowCount]["FeatName"] = (featName != null ? featName.Trim() : "");
                        dtSpec.Rows[rowCount]["SpecText"] = POTNSpecObj.getSpecText();
                        if (!POTNSpecObj.getFromSpecId().Equals(""))
                            dtSpec.Rows[rowCount]["FromSpec"] = BackEndObjects.Specifications.getSpecificationDetailbyIdDB(POTNSpecObj.getFromSpecId()).getSpecName();
                        if (!POTNSpecObj.getToSpecId().Equals(""))
                            dtSpec.Rows[rowCount]["ToSpec"] = BackEndObjects.Specifications.getSpecificationDetailbyIdDB(POTNSpecObj.getToSpecId()).getSpecName();

                        String[] imgPathList = POTNSpecObj.getImgPath().Split(new String[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                        dtSpec.Rows[rowCount]["imgName"] = (imgPathList != null && imgPathList.Length > 0) ? imgPathList[imgPathList.Length - 1] : "N\\A";
                        rowCount++;
                    }
                }
                ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).DataSource = dtSpec;
                ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).DataBind();
                ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).Visible = true;
                ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).Columns[1].Visible = false;
            }
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            String catId = ((Label)GridView1.Rows[e.RowIndex].Cells[0].FindControl("Label_Hidden")).Text;
            String POTNId = Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_SELECTED_RFQ_ID].ToString();

            Dictionary<String, String> existingProdList = (Dictionary<String, String>)Session[SessionFactory.POTN_SPECIFICATION_EXISTING_PROD_LIST];

            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            Dictionary<String, String> targetVals = new Dictionary<string, string>();

            whereCls.Add(BackEndObjects.RFQProductServiceDetails.RFQ_PROD_COL_RFQ_ID, POTNId);
            whereCls.Add(BackEndObjects.RFQProductServiceDetails.RFQ_PROD_COL_PROD_ID, catId);
            try
            {
                BackEndObjects.RFQProductServiceDetails.updateRFQProductServiceDetails(targetVals, whereCls, DBConn.Connections.OPERATION_DELETE);
                
                whereCls.Clear();
                whereCls.Add(BackEndObjects.RFQProdServQnty.RFQ_PROD_SRV_COL_RFQ_ID, POTNId);
                whereCls.Add(BackEndObjects.RFQProdServQnty.RFQ_PROD_SRV_COL_PROD_SRV_ID, catId);
                BackEndObjects.RFQProdServQnty.updateRFQProductServiceQuantityDB(targetVals, whereCls, DBConn.Connections.OPERATION_DELETE);

                whereCls.Clear();
                whereCls.Add(BackEndObjects.RFQResponseQuotes.RFQ_RES_QUOTE_COL_RFQ_ID, POTNId);
                whereCls.Add(BackEndObjects.RFQResponseQuotes.RFQ_RES_QUOTE_COL_PROD_CAT, catId);
                whereCls.Add(BackEndObjects.RFQResponseQuotes.RFQ_RES_QUOTE_COL_RESP_COMP_ID, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                BackEndObjects.RFQResponseQuotes.updateRFQResponseQuotesDB(targetVals, whereCls, DBConn.Connections.OPERATION_DELETE);

                existingProdList.Remove(catId);

                DataTable dt = (DataTable)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_POTN_PROD_SRV_DATATABLE];
                int index = GridView1.Rows[e.RowIndex].DataItemIndex;
                dt.Rows[index].Delete();
                GridView1.DataSource = dt;
                GridView1.DataBind();

                ArrayList POTNProdList = (ArrayList)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_POTN_PROD_SRV];
                for (int i = 0; i < POTNProdList.Count; i++)
                {
                    BackEndObjects.RFQProdServQnty POTNProdQntyObj = (RFQProdServQnty)POTNProdList[i];
                    if (POTNProdQntyObj.getProdCatId().Equals(catId) && POTNProdQntyObj.getRFQId().Equals(POTNId))
                    {
                        POTNProdList.RemoveAt(i);
                        break;
                    }
                }

                ArrayList POTNSpecList = (ArrayList)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_POTN_SPEC];
                ArrayList POTNSpecListDeletedRemoved = new ArrayList();
                for (int i = 0; i < POTNSpecList.Count; i++)
                {
                    BackEndObjects.RFQProductServiceDetails POTNSpecObj = (RFQProductServiceDetails)POTNSpecList[i];

                    if (!POTNSpecObj.getPrdCatId().Equals(catId))
                    {
                        POTNSpecListDeletedRemoved.Add(POTNSpecObj);
                    }
                }
                POTNSpecList = POTNSpecListDeletedRemoved;

                Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_POTN_SPEC] = POTNSpecList;
                Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_POTN_PROD_SRV] = POTNProdList;
                Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_POTN_PROD_SRV_DATATABLE] = dt;
                Session[SessionFactory.POTN_SPECIFICATION_EXISTING_PROD_LIST] = existingProdList;
                BindInnterGrid();
            }
            catch (Exception ex)
            {
            }
        }

        protected void DropDownList_Prod_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBox_Quote_Amnt.Text = DropDownList_Prod_List.SelectedValue;
        }

            }
}