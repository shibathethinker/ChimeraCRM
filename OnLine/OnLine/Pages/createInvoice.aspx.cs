using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BackEndObjects;
using ActionLibrary;
using System.Collections;
using System.Data;

namespace OnLine.Pages
{
    public partial class createInvoice : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                clearSessionVariables();
                loadContacts();
                loadVendorDetails();
                LoadProductCat();
                loadInvTaxCompGrid(null);
                loadTnC(null);
                loadCurrency();

                String rfqId = Request.QueryString.GetValues("rfId")[0];

                Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];
                if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_CREATE_INVOICE_SALES] &&
!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                {
                    Label_INV_Creation_Stat.Visible = true;
                    Label_INV_Creation_Stat.Text = "You dont have create access to invouce records";
                    Label_INV_Creation_Stat.ForeColor = System.Drawing.Color.Red;
                    Button_Create_Inv.Enabled = false;
                }

                if (rfqId != null && !rfqId.Equals(""))
                {
                    Button_Add_Prod_Srv.Enabled = false;
                    DropDownList_Contacts.Enabled = false;
                    populateProdDetailsFromRFQ(rfqId);
                    populateCustomerDetailsForSelectedRFQ(rfqId);
                }

                bool emptySOWarning = Request.QueryString.GetValues("emptySO") != null ? true : false;
                PanelEmptySOWarning.Visible = emptySOWarning;
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

        [System.Web.Services.WebMethod]
        [System.Web.Script.Services.ScriptMethod]
        public static String[] GetCompletionListProd(string prefixText, int count)
        {
            Dictionary<String,ShopChildProdsInventory> ProdNameDict = ((Dictionary<String,ShopChildProdsInventory>)
                HttpContext.Current.Session[SessionFactory.CREATE_INVOICE_MANUAL_PRODUCT_GRID_PRODUCT_LIST]);

            Dictionary<String, String> prodNameCostDict = new Dictionary<string, string>();

            ArrayList temp = new ArrayList();
            foreach (KeyValuePair<String,ShopChildProdsInventory> kvp in ProdNameDict)
            {
                if (kvp.Key.IndexOf(prefixText.Trim(), StringComparison.InvariantCultureIgnoreCase) >= 0)
                    temp.Add(kvp.Key);
            }

            return (String[])temp.ToArray(typeof(String));
        }

        protected void hdnValue_ValueChangedProd(object sender, EventArgs e)
        {
            string selectedVal = ((HiddenField)sender).Value;
            Dictionary<String, ShopChildProdsInventory> ProdNameDict = ((Dictionary<String, ShopChildProdsInventory>)
    HttpContext.Current.Session[SessionFactory.CREATE_INVOICE_MANUAL_PRODUCT_GRID_PRODUCT_LIST]);
            if (ProdNameDict.ContainsKey(selectedVal))
                TextBox_Prod_Unit_Price.Text = ProdNameDict[selectedVal].getUnitListPrice();

            /*String[] rfqArray = (String[])Session[SessionFactory.ALL_DEFECT_CREATE_DEFECT_RFQ_AUTOCOMPLETE_LIST];

            for (int i = 0; i < rfqArray.Length; i++)
                if (rfqArray[i].Equals(selectedVal))
                {
                    TextBox_Rfq_No.Text = rfqArray[i].Substring(rfqArray[i].IndexOf("(") + 1, (rfqArray[i].Length - 2 - rfqArray[i].IndexOf("(")));
                    break;
                }*/

        }
        
        protected void clearSessionVariables()
        {
            Session[SessionFactory.CREATE_INVOICE_MANUAL_PRODUCT_GRID] = null;
            Session[SessionFactory.CREATE_INVOICE_MANUAL_SELECTED_SPEC_MAP_DICT] = null;

        }

        protected void loadVendorDetails()
        {
            String entId=Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();
            
                        BackEndObjects.MainBusinessEntity vendObj = BackEndObjects.MainBusinessEntity.
                            getMainBusinessEntitybyIdwithLessDetailsDB(entId);
                            
            Label_Vendor_Name.Text = vendObj.getEntityName();

                String vendContact = vendObj.getPhNo();
                String vendDetails = "";
                String localId = "";

            BackEndObjects.AddressDetails addrObj = BackEndObjects.AddressDetails.getAddressforMainBusinessEntitybyIdDB(entId);

              if (addrObj.getLocalityId() != null && !addrObj.getLocalityId().Equals(""))
            {

                    vendDetails += addrObj.getAddrLine1() + ",";
                localId = addrObj.getLocalityId();

                BackEndObjects.Localities lclObj = BackEndObjects.Localities.getLocalitybyIdDB(localId);
                BackEndObjects.City ctObj = BackEndObjects.Localities.getCityDetailsforLocalitywoOtherAsscLocalitiesDB(localId);
                BackEndObjects.State stObj = BackEndObjects.City.getStateDetailsforCitywoOtherAsscCitiesDB(ctObj.getCityId());
                BackEndObjects.Country cntObj = BackEndObjects.State.getCountryDetailsforStatewoOtherAsscStatesDB(stObj.getStateId());

                    vendDetails += lclObj.getLocalityName() + "<br/>" + ctObj.getCityName() + "<br/>" + stObj.getStateName() 
                        + "<br/>" + cntObj.getCountryName() + "<br/> Phone:" + vendContact;
            }

              Label_Vendor_Addr.Text = vendDetails;
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

        protected void clearProdSelectionPanel()
        {
            DropDownList_Level1.SelectedIndex = -1;
            DropDownList_Level2.Items.Clear();
            DropDownList_Level3.Items.Clear();
            TextBox_Prod_Name.Text = "";
            TextBox_Prod_Qnty.Text = "";
            TextBox_Prod_Unit_Price.Text = "";
            GridView_Prod_Srv.Visible = false;

        }

        protected void loadProdList()
        {
            Dictionary<String, ShopChildProdsInventory> childDict = (Dictionary<String, ShopChildProdsInventory>)
    Session[SessionFactory.CREATE_INVOICE_MANUAL_PRODUCT_GRID_PRODUCT_LIST];

            if (childDict == null || childDict.Count == 0)
            {
                childDict = ShopChildProdsInventory.
getAllShopChildProdObjsbyEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                Session[SessionFactory.CREATE_INVOICE_MANUAL_PRODUCT_GRID_PRODUCT_LIST] = childDict;
            }

        }
        protected void Button_Add_Prod_Srv_Click(object sender, EventArgs e)
        {
            clearProdSelectionPanel();
            loadProdList();
            Panel_Prod_Service_Det.Visible = true;
        }

        protected void Button_Add_Prod_Srv_Det_Hide_Click(object sender, EventArgs e)
        {
            //Clear any selected product details from the session variable because these are not going to be added to the product grid
            ArrayList rfqProdSrvList = (ArrayList)Session[SessionFactory.CREATE_INVOICE_MANUAL_SELECTED_SPEC_MAP];
            if(rfqProdSrvList!=null && rfqProdSrvList.Count>0)
            rfqProdSrvList.Clear();

            Session[SessionFactory.CREATE_INVOICE_MANUAL_SELECTED_SPEC_MAP] = rfqProdSrvList;

            Label_Selected_List.Text = "";
            Panel_Prod_Service_Det.Visible = false;
        }

        protected void GridView_Prod_Srv_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_Prod_Srv.PageIndex = e.NewPageIndex;
            GridView_Prod_Srv.SelectedIndex = -1;
            fillGrid();
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
            DropDownList_Level3.Items.Clear();
            Session[SessionFactory.CREATE_INVOICE_MANUAL_SELECTED_PRODUCT_CAT] = DropDownList_Level1.SelectedValue;

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
            Session[SessionFactory.CREATE_INVOICE_MANUAL_SELECTED_PRODUCT_CAT] = DropDownList_Level2.SelectedValue;
        }

        protected void DropDownList_Level3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session[SessionFactory.CREATE_INVOICE_MANUAL_SELECTED_PRODUCT_CAT] = DropDownList_Level3.SelectedValue;
        }

        protected void Buttin_Show_Spec_List1_Click(object sender, EventArgs e)
        {
            fillGrid();
        }

        protected String updateTotalINVAmount()
        {
            //float tax = TextBox_tax.Text.Equals("") ? 0 : float.Parse(TextBox_tax.Text);
            //return ((float.Parse(Label_Sub_Total_Amount_Value.Text) * tax) / 100 + float.Parse(Label_Sub_Total_Amount_Value.Text)).ToString();
            float totalAmount = (!Label_Sub_Total_Amount_Value.Text.Equals("")?float.Parse(Label_Sub_Total_Amount_Value.Text):0);

            if (Session[SessionFactory.CREATE_INVOICE_MANUAL_TOTAL_TAX_COMP_PERC] != null)
            {
                float totalTaxPerc = (float)Session[SessionFactory.CREATE_INVOICE_MANUAL_TOTAL_TAX_COMP_PERC];
                float totalTaxableAmount = (float.TryParse(TextBox_Taxable_Amount.Text,out totalTaxableAmount)?float.Parse(TextBox_Taxable_Amount.Text):0);
                TextBox_Taxable_Amount.Text = totalTaxableAmount.ToString();

                totalAmount = (totalTaxPerc * totalTaxableAmount) / 100 + totalAmount;

                Label_Total_Amount_Value.Text = totalAmount.ToString();
            }
            return totalAmount.ToString();
        }

        /// <summary>
        /// This method its to fill the product service selection grid
        /// </summary>
        protected void fillGrid()
        {
            String selectedProdCatId = Session[SessionFactory.CREATE_INVOICE_MANUAL_SELECTED_PRODUCT_CAT].ToString();
            Dictionary<String, Features> featDict = BackEndObjects.ProductCategory.getFeatureforCategoryDB(selectedProdCatId);

            if (featDict.Count > 0)
            {
                GridView_Prod_Srv.Visible = true;
                Label_Extra_Spec.Visible = true;
                TextBox_Spec.Visible = true;
                //Label_Extra_Spec_upload.Visible = true;
                //FileUpload_Extra_Spec.Visible = true;

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

                GridView_Prod_Srv.SelectedIndex = -1;
                GridView_Prod_Srv.DataSource = dt;
                GridView_Prod_Srv.DataBind();

                GridView_Prod_Srv.HeaderRow.Cells[1].Visible = false;
                foreach (GridViewRow gVR in GridView_Prod_Srv.Rows)
                {
                    gVR.Cells[1].Visible = false;

                    Features ft = featDict[((Label)gVR.Cells[1].FindControl("Label_Hidden_FeatId")).Text];
                    ArrayList specList = ft.getSpecifications();

                    ListItem ltEmpty = new ListItem();
                    ltEmpty.Value = "_";
                    ltEmpty.Text = "_";
                    ((DropDownList)gVR.Cells[2].FindControl("DropDownList_Gridview1_From")).Items.Add(ltEmpty);
                    ((DropDownList)gVR.Cells[3].FindControl("DropDownList_Gridview1_To")).Items.Add(ltEmpty);

                    for (int j = 0; j < specList.Count; j++)
                    {
                        BackEndObjects.Specifications specObj = (BackEndObjects.Specifications)specList[j];
                        ListItem ltSpec = new ListItem();
                        ltSpec.Text = specObj.getSpecName();
                        ltSpec.Value = specObj.getSpecId();
                        ((DropDownList)gVR.Cells[2].FindControl("DropDownList_Gridview1_From")).Items.Add(ltSpec);
                        ((DropDownList)gVR.Cells[3].FindControl("DropDownList_Gridview1_To")).Items.Add(ltSpec);
                    }

                    ((DropDownList)gVR.Cells[2].FindControl("DropDownList_Gridview1_From")).SelectedValue = "_";
                    ((DropDownList)gVR.Cells[3].FindControl("DropDownList_Gridview1_To")).SelectedValue = "_";
                }
            }
        }

        protected void loadProductGrid_DEPRECATED(String poId)
        {
            /*
            ArrayList rfqSpecList = new ArrayList();
            String context = Request.QueryString.GetValues("context")[0];

            if (context.Equals("client"))
            {
                rfqSpecList = (ArrayList)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_RFQ_SPEC];
            }
            else if (context.Equals("vendor"))
                rfqSpecList = (ArrayList)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_POTN_SPEC];
            else if (context.Equals("clientInvoiceGrid") || context.Equals("vendInvoiceGrid"))
            {//If this is sent from the Invoice grid in Purchase screen
                rfqSpecList = BackEndObjects.RFQProductServiceDetails.getAllProductServiceDetailsbyRFQIdDB(Request.QueryString.GetValues("rfId")[0]);
            }

            ArrayList poQuoteList = PurchaseOrderQuote.getPurcahseOrderQuoteListbyPOIdDB(poId);

            DataTable dt = new DataTable();
            dt.Columns.Add("Hidden_Cat_Id");
            dt.Columns.Add("Serial");
            dt.Columns.Add("Qnty");
            dt.Columns.Add("Prod_Name");
            dt.Columns.Add("Cat_Name");
            dt.Columns.Add("Unit_Price");
            dt.Columns.Add("Cat_Total");

            float totalAmnt = 0;

            for (int i = 0; i < poQuoteList.Count; i++)
            {
                BackEndObjects.PurchaseOrderQuote poQntyObj = (PurchaseOrderQuote)poQuoteList[i];
                //Preference to toQuantity
                float qnty = poQntyObj.getUnits();
                String catId = poQntyObj.getProd_srv_category();

                dt.Rows.Add();
                dt.Rows[i]["Hidden_Cat_Id"] = catId;
                dt.Rows[i]["Serial"] = i + 1;
                dt.Rows[i]["Qnty"] = qnty.ToString();
                dt.Rows[i]["Prod_Name"] = poQntyObj.getProduct_name();
                dt.Rows[i]["Cat_Name"] = BackEndObjects.ProductCategory.getProductCategorybyIdwoFeaturesDB(catId).getProductCategoryName();
                dt.Rows[i]["Unit_Price"] = poQntyObj.getQuote();
                dt.Rows[i]["Cat_Total"] = qnty * poQntyObj.getQuote();

                totalAmnt += poQntyObj.getQuote() * qnty;
            }


            GridView1.Visible = true;
            GridView1.DataSource = dt;
            GridView1.DataBind();
            GridView1.Columns[0].Visible = false;

            foreach (GridViewRow gVR in GridView1.Rows)
            {
                // if (!context.Equals("client"))
                //{
                ((TextBox)gVR.Cells[2].FindControl("TextBox_Qnty")).Enabled = false;
                ((TextBox)gVR.Cells[2].FindControl("TextBox_Unit_Price")).Enabled = false;
                //}
                DataTable dtSpec = new DataTable();
                dtSpec.Columns.Add("Hidden");
                dtSpec.Columns.Add("FeatName");
                dtSpec.Columns.Add("SpecText");
                dtSpec.Columns.Add("FromSpec");
                dtSpec.Columns.Add("ToSpec");
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

                        rowCount++;
                    }
                }
                ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).DataSource = dtSpec;
                ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).DataBind();
                ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).Visible = true;
                ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).Columns[0].Visible = false;
            }

            Label_Sub_Total_Amount_Value.Visible = true;
            Label_Sub_Total_Amount_Value.Text = totalAmnt.ToString();
            if (TextBox_Taxable_Amount.Text.Equals("") || float.Parse(TextBox_Taxable_Amount.Text) == 0)
                TextBox_Taxable_Amount.Text = totalAmnt.ToString();
            Label_Total_Amount_Value.Visible = true;
            Label_Total_Amount_Value.Text = updateTotalINVAmount();
            */
        }

        protected void getAddintionalProdSrvList()
        {
            ArrayList rfqProdSrvList = (ArrayList)Session[SessionFactory.CREATE_INVOICE_MANUAL_SELECTED_SPEC_MAP];
            if (rfqProdSrvList == null)
                rfqProdSrvList = new ArrayList();

            BackEndObjects.RFQProductServiceDetails rfqSpec = new RFQProductServiceDetails();
            rfqSpec.setPrdCatId(Session[SessionFactory.CREATE_INVOICE_MANUAL_SELECTED_PRODUCT_CAT].ToString());
            rfqSpec.setFeatId("ft_dummy");
            rfqSpec.setFromSpecId("");
            rfqSpec.setToSpecId("");
            rfqSpec.setSpecText(TextBox_Spec.Text);
            rfqSpec.setCreatedDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            if (User.Identity.Name != null)
                rfqSpec.setCreatedUsr(User.Identity.Name);
            //if (FileUpload_Extra_Spec.HasFile)
                //rfqSpec.setFileStream(FileUpload_Extra_Spec);


            rfqProdSrvList.Add(rfqSpec);

            Session[SessionFactory.CREATE_INVOICE_MANUAL_SELECTED_SPEC_MAP] = rfqProdSrvList;
        }

        protected void GridView_Prod_Srv_SelectedIndexChanged(object sender, EventArgs e)
        {
            ArrayList rfqProdSrvList = (ArrayList)Session[SessionFactory.CREATE_INVOICE_MANUAL_SELECTED_SPEC_MAP];
            if (rfqProdSrvList == null)
                rfqProdSrvList = new ArrayList();
            
            BackEndObjects.RFQProductServiceDetails rfqSpec = new RFQProductServiceDetails();
            rfqSpec.setPrdCatId(Session[SessionFactory.CREATE_INVOICE_MANUAL_SELECTED_PRODUCT_CAT].ToString());
            rfqSpec.setFeatId(((Label)GridView_Prod_Srv.SelectedRow.Cells[1].FindControl("Label_Hidden_FeatId")).Text);
            rfqSpec.setFromSpecId(((DropDownList)GridView_Prod_Srv.SelectedRow.Cells[2].FindControl("DropDownList_Gridview1_From")).SelectedValue);
            rfqSpec.setToSpecId(((DropDownList)GridView_Prod_Srv.SelectedRow.Cells[3].FindControl("DropDownList_Gridview1_To")).SelectedValue);
            //rfqSpec.setSpecText(TextBox_Spec.Text);
            rfqSpec.setCreatedDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            if (User.Identity.Name != null)
                rfqSpec.setCreatedUsr(User.Identity.Name);
            //if (((FileUpload)GridView_Prod_Srv.SelectedRow.Cells[3].FindControl("FileUpload_Spec")).HasFile)
                //rfqSpec.setFileStream((FileUpload)GridView_Prod_Srv.SelectedRow.Cells[3].FindControl("FileUpload_Spec"));
            

            rfqProdSrvList.Add(rfqSpec);

            GridView_Prod_Srv.SelectedRow.Cells[0].Enabled = false;
            GridView_Prod_Srv.SelectedRow.Cells[3].Enabled = false;
            GridView_Prod_Srv.SelectedRow.Cells[4].Enabled = false;
            //GridView_Prod_Srv.SelectedRow.Cells[5].Enabled = false;
            GridView_Prod_Srv.SelectedRow.Cells[0].FindControl("Image_Selected").Visible = true;
            Label_Selected_List.Text += "," + GridView_Prod_Srv.SelectedRow.DataItemIndex;

            Session[SessionFactory.CREATE_INVOICE_MANUAL_SELECTED_SPEC_MAP] = rfqProdSrvList;
        }
        /// <summary>
        /// This method will populate the product grid if there is a RFQ id supplied
        /// </summary>
        /// <param name="rfqId"></param>
        protected void populateProdDetailsFromRFQ(String rfqId)
        {
            //ArrayList rfqProdSrvList=BackEndObjects.RFQProductServiceDetails.getAllProductServiceDetailsbyRFQIdDB(rfqId);
            ArrayList rfqProdQntyList = BackEndObjects.RFQProdServQnty.getRFQProductServiceQuantityforRFIdDB(rfqId);
            Dictionary<String,RFQResponseQuotes> rfqRespQuoteDict=RFQResponseQuotes.
                getAllResponseQuotesforRFQandResponseEntityDB(rfqId, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

            float totalAmnt = 0;

           DataTable dt = new DataTable();
            dt.Columns.Add("Hidden_Cat_Id");
            dt.Columns.Add("Serial");
            dt.Columns.Add("Qnty");
            dt.Columns.Add("Prod_Name");
            dt.Columns.Add("Cat_Name");
            dt.Columns.Add("Unit_Price");
            dt.Columns.Add("Cat_Total");

            for (int i = 0; i < rfqProdQntyList.Count; i++)
            {
                BackEndObjects.RFQProdServQnty rfqProdQntyObj = (BackEndObjects.RFQProdServQnty)rfqProdQntyList[i];
                dt.Rows.Add();

                float qnty = (rfqProdQntyObj.getToQnty() != null && rfqProdQntyObj.getToQnty() != 0 ? rfqProdQntyObj.getToQnty() : rfqProdQntyObj.getFromQnty());
                float unitPrice = rfqRespQuoteDict.ContainsKey(rfqProdQntyObj.getProdCatId())?
                    float.Parse(rfqRespQuoteDict[rfqProdQntyObj.getProdCatId()].getQuote()):0;
                String prodName = rfqRespQuoteDict.ContainsKey(rfqProdQntyObj.getProdCatId())?
                    rfqRespQuoteDict[rfqProdQntyObj.getProdCatId()].getProductName():"";
                prodName = (prodName == null || prodName.Equals("") ? "product serial" + GridView1.Rows.Count + 1 : prodName);

                dt.Rows[i]["Hidden_Cat_Id"] = rfqProdQntyObj.getProdCatId();
                dt.Rows[i]["Serial"] = i+ 1;
                dt.Rows[i]["Qnty"] = qnty;
                dt.Rows[i]["Prod_Name"] = prodName;
                dt.Rows[i]["Cat_Name"] = BackEndObjects.ProductCategory.getProductCategorybyIdwoFeaturesDB(rfqProdQntyObj.getProdCatId()).getProductCategoryName();
                dt.Rows[i]["Unit_Price"] = unitPrice;
                dt.Rows[i]["Cat_Total"] = qnty * unitPrice;

                totalAmnt = Label_Sub_Total_Amount_Value.Text.Equals("") ? totalAmnt+qnty * unitPrice :
float.Parse(Label_Sub_Total_Amount_Value.Text) + qnty * unitPrice;

                //The following steps are required to populate the inner grid
                Dictionary<String,RFQProductServiceDetails> rfqProdSpecDictForCategory=BackEndObjects.RFQProductServiceDetails.
                    getAllProductServiceDetailsbyRFQandProductIdDB(rfqId, rfqProdQntyObj.getProdCatId());
                ArrayList rfqProdSrvList = new ArrayList();

                foreach (KeyValuePair<String, RFQProductServiceDetails> kvp in rfqProdSpecDictForCategory)
                                    rfqProdSrvList.Add(kvp.Value);
                
                Dictionary<String, ArrayList> rfqProdSrvDict = (Dictionary<String, ArrayList>)Session[SessionFactory.CREATE_INVOICE_MANUAL_SELECTED_SPEC_MAP_DICT];
                if (rfqProdSrvDict == null)
                    rfqProdSrvDict = new Dictionary<string, ArrayList>();

                if(!rfqProdSrvDict.ContainsKey(prodName))
                rfqProdSrvDict.Add(prodName, rfqProdSrvList);

                Session[SessionFactory.CREATE_INVOICE_MANUAL_SELECTED_SPEC_MAP_DICT] = rfqProdSrvDict;

            }
            

            GridView1.Visible = true;
            GridView1.DataSource = dt;
            GridView1.DataBind();
            GridView1.Columns[1].Visible = false;
            GridView1.Columns[2].Visible = false;

            loadInnerGridinProdGrid(GridView1);

            Session[SessionFactory.CREATE_INVOICE_MANUAL_PRODUCT_GRID] = dt;

            Label_Sub_Total_Amount_Value.Text = totalAmnt.ToString();
            //TextBox_Sub_Total_Amount_Value.Text = Label_Sub_Total_Amount_Value.Text;
            ////TextBox_Sub_Total_Amount_Value.Text = Label_Sub_Total_Amount_Value.Text;
            Label_Sub_Total_Amount_Value.Visible = true;
            //if (TextBox_Taxable_Amount.Text.Equals("") || float.Parse(TextBox_Taxable_Amount.Text) == 0)
            TextBox_Taxable_Amount.Text = totalAmnt.ToString();
            Label_Total_Amount_Value.Visible = true;
            Label_Total_Amount_Value.Text = updateTotalINVAmount();

        }
        /// <summary>
        /// Adds the product service details in the Main grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_Add_Prod_Srv_Det_Click(object sender, EventArgs e)
        {
            if (Label_Product_Uniq_Name_List.Text.IndexOf("<start>"+TextBox_Prod_Name.Text.Trim()+"<end>") > 0)
                Label_Prod_Name.ForeColor = System.Drawing.Color.Red;
            else
            {
                Label_Invalid_Prod_Cat.Visible = false;
                Label_Prod_Required.Visible = false;

                String catId = null;
                try
                {
                    catId = Session[SessionFactory.CREATE_INVOICE_MANUAL_SELECTED_PRODUCT_CAT].ToString();
                }
                catch (Exception ex)
                {
                }

                bool addProd = true;

                if ((catId == null || catId.Equals("")))
                {
                    Dictionary<String, ShopChildProdsInventory> ProdNameDict = ((Dictionary<String, ShopChildProdsInventory>)
    HttpContext.Current.Session[SessionFactory.CREATE_INVOICE_MANUAL_PRODUCT_GRID_PRODUCT_LIST]);
                    if (ProdNameDict.ContainsKey(TextBox_Prod_Name.Text.Trim()))
                        catId = ProdNameDict[TextBox_Prod_Name.Text.Trim()].getProdCatId();
                    //Try to retrive the product's category
                    else
                    { Label_Invalid_Prod_Cat.Visible = true; addProd = false; }
                }

                if (addProd)
                {
                    Label_Prod_Name.ForeColor = System.Drawing.Color.Black;
                    Label_Product_Uniq_Name_List.Text += "," + "<start>"+TextBox_Prod_Name.Text+"<end>";

                    if (!TextBox_Spec.Text.Equals(""))
                        getAddintionalProdSrvList();

                    //Get the latest prod/service list and put it in the prod/service dictionary
                    ArrayList rfqProdSrvList = (ArrayList)Session[SessionFactory.CREATE_INVOICE_MANUAL_SELECTED_SPEC_MAP];
                    Dictionary<String, ArrayList> rfqProdSrvDict = (Dictionary<String, ArrayList>)Session[SessionFactory.CREATE_INVOICE_MANUAL_SELECTED_SPEC_MAP_DICT];
                    if (rfqProdSrvDict == null)
                        rfqProdSrvDict = new Dictionary<string, ArrayList>();

                    rfqProdSrvDict.Add(TextBox_Prod_Name.Text, rfqProdSrvList);

                    //Put the cleared list and newly filled dictionary in the session
                    Session[SessionFactory.CREATE_INVOICE_MANUAL_SELECTED_SPEC_MAP] = new ArrayList();
                    Session[SessionFactory.CREATE_INVOICE_MANUAL_SELECTED_SPEC_MAP_DICT] = rfqProdSrvDict;

                    DataTable dt = (DataTable)Session[SessionFactory.CREATE_INVOICE_MANUAL_PRODUCT_GRID];

                    if (dt == null)
                    {
                        dt = new DataTable();
                        dt.Columns.Add("Hidden_Cat_Id");
                        dt.Columns.Add("Serial");
                        dt.Columns.Add("Qnty");
                        dt.Columns.Add("Prod_Name");
                        dt.Columns.Add("Cat_Name");
                        dt.Columns.Add("Unit_Price");
                        dt.Columns.Add("Cat_Total");
                    }

                    float totalAmnt = 0;

                    int i = dt.Rows.Count;
                    dt.Rows.Add();

                    dt.Rows[i]["Hidden_Cat_Id"] = catId;
                    dt.Rows[i]["Serial"] = GridView1.Rows.Count + 1;
                    dt.Rows[i]["Qnty"] = TextBox_Prod_Qnty.Text;
                    dt.Rows[i]["Prod_Name"] = TextBox_Prod_Name.Text;
                    dt.Rows[i]["Cat_Name"] = BackEndObjects.ProductCategory.getProductCategorybyIdwoFeaturesDB(catId).getProductCategoryName();
                    dt.Rows[i]["Unit_Price"] = TextBox_Prod_Unit_Price.Text;
                    dt.Rows[i]["Cat_Total"] = float.Parse(TextBox_Prod_Qnty.Text) * float.Parse(TextBox_Prod_Unit_Price.Text);

                    totalAmnt = Label_Sub_Total_Amount_Value.Text.Equals("") ? float.Parse(TextBox_Prod_Qnty.Text) * float.Parse(TextBox_Prod_Unit_Price.Text) :
                        float.Parse(Label_Sub_Total_Amount_Value.Text) + float.Parse(TextBox_Prod_Qnty.Text) * float.Parse(TextBox_Prod_Unit_Price.Text);


                    GridView1.Visible = true;
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    GridView1.Columns[2].Visible = false;

                    loadInnerGridinProdGrid(GridView1);

                    Session[SessionFactory.CREATE_INVOICE_MANUAL_PRODUCT_GRID] = dt;

                    Label_Selected_List.Text = "";
                    Label_Sub_Total_Amount_Value.Text = totalAmnt.ToString();
                    //TextBox_Sub_Total_Amount_Value.Text = Label_Sub_Total_Amount_Value.Text;
                    Label_Sub_Total_Amount_Value.Visible = true;
                    //if (TextBox_Taxable_Amount.Text.Equals("") || float.Parse(TextBox_Taxable_Amount.Text) == 0)
                    TextBox_Taxable_Amount.Text = totalAmnt.ToString();
                    Label_Total_Amount_Value.Visible = true;
                    Label_Total_Amount_Value.Text = updateTotalINVAmount();

                    clearProdSelectionPanel();
                }
            }
        }

        protected void loadInnerGridinProdGrid(GridView GridView1)
        {
            Dictionary<String, ArrayList> rfqProdSrvDict = (Dictionary<String, ArrayList>)Session[SessionFactory.CREATE_INVOICE_MANUAL_SELECTED_SPEC_MAP_DICT];
            foreach (GridViewRow gVR in GridView1.Rows)
            {
                // if (!context.Equals("client"))
                //{
                ((TextBox)gVR.Cells[2].FindControl("TextBox_Qnty")).Enabled = false;
                ((TextBox)gVR.Cells[2].FindControl("TextBox_Unit_Price")).Enabled = false;
                //}

                String prodName = ((Label)gVR.Cells[0].FindControl("Label_Product_Name")).Text;

                DataTable dtSpec = new DataTable();
                dtSpec.Columns.Add("Hidden");
                dtSpec.Columns.Add("FeatName");
                dtSpec.Columns.Add("SpecText");
                dtSpec.Columns.Add("FromSpec");
                dtSpec.Columns.Add("ToSpec");

                ArrayList rfqSpecList = rfqProdSrvDict[prodName];
                //(ArrayList)Session[SessionFactory.CREATE_INVOICE_MANUAL_SELECTED_SPEC_MAP];
                int rowCount = 0;

                if(rfqSpecList!=null)
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
                        if (!rfqSpecObj.getFromSpecId().Equals("") && !rfqSpecObj.getFromSpecId().Equals("_"))
                            dtSpec.Rows[rowCount]["FromSpec"] = BackEndObjects.Specifications.getSpecificationDetailbyIdDB(rfqSpecObj.getFromSpecId()).getSpecName();
                        if (!rfqSpecObj.getToSpecId().Equals("") && !rfqSpecObj.getToSpecId().Equals("_"))
                            dtSpec.Rows[rowCount]["ToSpec"] = BackEndObjects.Specifications.getSpecificationDetailbyIdDB(rfqSpecObj.getToSpecId()).getSpecName();

                        rowCount++;
                    }
                }
                ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).DataSource = dtSpec;
                ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).DataBind();
                ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).Visible = true;
                ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).Columns[0].Visible = false;
            }
        }

        protected void DropDownList_Contacts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!DropDownList_Contacts.SelectedValue.Equals("_"))
            {
                BackEndObjects.Contacts contactObj = BackEndObjects.Contacts.getContactDetailsforContactEntityDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(),
                    DropDownList_Contacts.SelectedValue);

                Label_Client_Name.Text = contactObj.getContactName();

                String localId = contactObj.getLocalityId();
                Label_Contact_Locality_Id_Hidden.Text = localId;

                String custDetails = contactObj.getStreetName();
                String custContact = contactObj.getMobNo();

                if (localId != null && !localId.Equals(""))
                {
                    BackEndObjects.Localities lclObj = BackEndObjects.Localities.getLocalitybyIdDB(localId);
                    BackEndObjects.City ctObj = BackEndObjects.Localities.getCityDetailsforLocalitywoOtherAsscLocalitiesDB(localId);
                    String cityId = (ctObj != null && ctObj.getCityId() != null && !ctObj.getCityId().Equals("") ? ctObj.getCityId() : localId);
                    BackEndObjects.State stObj = BackEndObjects.City.getStateDetailsforCitywoOtherAsscCitiesDB(cityId);
                    if (cityId.Equals(localId))
                        ctObj = BackEndObjects.City.getCitybyIdwoLocalitiesDB(localId);
                    String stateId = (stObj != null && stObj.getStateId() != null && !stObj.getStateId().Equals("") ? stObj.getStateId() : localId);
                    BackEndObjects.Country cntObj = BackEndObjects.State.getCountryDetailsforStatewoOtherAsscStatesDB(stateId);
                    if (stateId.Equals(localId))
                        stObj = BackEndObjects.State.getStatebyIdwoCitiesDB(stateId);

                    custDetails += "," + lclObj.getLocalityName() + "<br/>" + ctObj.getCityName() + "<br/>" + stObj.getStateName() + "<br/>" +
                        cntObj.getCountryName() + "<br/> Phone:" + custContact;
                }

                Label_Client_Addr.Text = custDetails;
                Label_Contact_Required.Visible = false;
            }
        }


        /// <summary>
        /// For cases where the RFQ is already selected the contact selection dropdown in disabled and the customer
        /// details are populated
        /// </summary>
        /// <param name="rfqId"></param>
        protected void populateCustomerDetailsForSelectedRFQ(String rfqId)
        {
            String custEntId = BackEndObjects.RFQDetails.getRFQDetailsbyIdDB(rfqId).getEntityId();
            String localId="", custDetails="", custContact="";

            BackEndObjects.Contacts contactObj = BackEndObjects.Contacts.getContactDetailsforContactEntityDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(),
    DropDownList_Contacts.SelectedValue);
                        

            if (contactObj != null && contactObj.getLocalityId() != null)
            {
                Label_Client_Name.Text = contactObj.getContactName();
                localId = contactObj.getLocalityId();
                Label_Contact_Locality_Id_Hidden.Text = localId;
                custDetails = contactObj.getStreetName();
                custContact = contactObj.getMobNo();
            }
            else
            {
                BackEndObjects.MainBusinessEntity mEntObj = MainBusinessEntity.getMainBusinessEntitybyIdDB(custEntId);
                BackEndObjects.AddressDetails addrObj = BackEndObjects.AddressDetails.getAddressforMainBusinessEntitybyIdDB(mEntObj.getEntityId());

                Label_Client_Name.Text = mEntObj.getEntityName();
                localId = addrObj.getLocalityId();
                Label_Contact_Locality_Id_Hidden.Text = localId;
                custDetails = addrObj.getAddrLine1();
                custContact = addrObj.getSubPhNo();
            }

            if (localId != null && !localId.Equals(""))
            {
                BackEndObjects.Localities lclObj = BackEndObjects.Localities.getLocalitybyIdDB(localId);
                BackEndObjects.City ctObj = BackEndObjects.Localities.getCityDetailsforLocalitywoOtherAsscLocalitiesDB(localId);
                String cityId = (ctObj != null && ctObj.getCityId() != null && !ctObj.getCityId().Equals("") ? ctObj.getCityId() : localId);
                BackEndObjects.State stObj = BackEndObjects.City.getStateDetailsforCitywoOtherAsscCitiesDB(cityId);
                if (cityId.Equals(localId))
                    ctObj = BackEndObjects.City.getCitybyIdwoLocalitiesDB(localId);
                String stateId = (stObj != null && stObj.getStateId() != null && !stObj.getStateId().Equals("") ? stObj.getStateId() : localId);
                BackEndObjects.Country cntObj = BackEndObjects.State.getCountryDetailsforStatewoOtherAsscStatesDB(stateId);
                if (stateId.Equals(localId))
                    stObj = BackEndObjects.State.getStatebyIdwoCitiesDB(stateId);

                custDetails += "," + lclObj.getLocalityName() + "<br/>" + ctObj.getCityName() + "<br/>" + stObj.getStateName() + "<br/>" +
                    cntObj.getCountryName() + "<br/> Phone:" + custContact;
            }

            Label_Client_Addr.Text = custDetails;

        }

        protected void loadContacts()
        {
            //ArrayList contactObjList = Contacts.getAllContactsbyEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

            Dictionary<String, String> existingContactDict = (Dictionary<String, String>)Session[SessionFactory.EXISTING_CONTACT_DICTIONARY];
            foreach (KeyValuePair<String, String> kvp in existingContactDict)
            {
                String contactName = kvp.Key;
                String contactEntId = kvp.Value;

                ListItem ltItem = new ListItem();
                ltItem.Text = contactName;
                ltItem.Value = contactEntId;
                DropDownList_Contacts.Items.Add(ltItem);

            }

            DropDownList_Contacts.SelectedValue = "_";
        }

        protected float getTotalTaxCompGridandTaxPerc(DataTable dt)
        {
            float totalTaxPerc = 0;

            ArrayList invList = BackEndObjects.DocFormat.
getDocFormatforEntityIdandDocTypeDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), BackEndObjects.DocFormat.DOCFORMAT_DOC_TYPE_INVOICE);


            int taxCompGridCounter = 0;

            for (int i = 0; i < invList.Count; i++)
            {
                DocFormat invObj = (DocFormat)invList[i];

                String sectionType = invObj.getSection_type();

                switch (sectionType)
                {
                    case BackEndObjects.DocFormat.DOCFORMAT_DOC_TYPE_INVOICE_SECTION_TYPE_TAX:
                        dt.Rows.Add();
                        dt.Rows[taxCompGridCounter]["Hidden"] = invObj.getDocformat_id();
                        //Component name is the section type name
                        dt.Rows[taxCompGridCounter]["Comp_Name"] = invObj.getSectionTypeName();
                        dt.Rows[taxCompGridCounter]["Comp_Value"] = invObj.getText();

                        totalTaxPerc += float.Parse(invObj.getText());

                        taxCompGridCounter++;
                        break;

                }
            }
            return totalTaxPerc;
        }

        protected void loadInvTaxCompGrid(String invId)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Hidden");
            dt.Columns.Add("Comp_Name");
            dt.Columns.Add("Comp_Value");

            DataTable dtComplete = new DataTable();
            dtComplete.Columns.Add("Hidden");
            dtComplete.Columns.Add("Comp_Name");
            dtComplete.Columns.Add("Comp_Value");

            float totalTaxPerc = 0;

            if (invId == null || invId.Equals(""))
            {
                totalTaxPerc = getTotalTaxCompGridandTaxPerc(dt);
                dtComplete = dt;
            }
            else
            {
                //If invoice was already created populate the tax component values from the invoice component table
                ArrayList invTaxCompList = BackEndObjects.InvoiceComponents.
                    getInvoiceComponentByInvIdandSecType(invId, BackEndObjects.InvoiceComponents.INVOICE_SECTION_TYPE_TAX);

                for (int i = 0; i < invTaxCompList.Count; i++)
                {
                    dt.Rows.Add();

                    BackEndObjects.InvoiceComponents invCompObj = (BackEndObjects.InvoiceComponents)invTaxCompList[i];

                    dt.Rows[i]["Hidden"] = invCompObj.getInvoice_Id();
                    //Component name is the section type name
                    dt.Rows[i]["Comp_Name"] = invCompObj.getSection_type_name();
                    dt.Rows[i]["Comp_Value"] = invCompObj.getSection_value();

                    totalTaxPerc += float.Parse(invCompObj.getSection_value());
                }

            }

            if (dtComplete.Rows.Count == 0)
                getTotalTaxCompGridandTaxPerc(dtComplete);

            GridView_Inv_Tax_Complete_List.DataSource = dtComplete;
            GridView_Inv_Tax_Complete_List.DataBind();
            GridView_Inv_Tax_Complete_List.Columns[1].Visible = false;



            Session[SessionFactory.CREATE_INVOICE_MANUAL_TOTAL_TAX_COMP_GRID] = dtComplete;

            if (dt.Rows.Count > 0)
            {
                GridView_Inv_Tax_Comp.DataSource = dt;
                GridView_Inv_Tax_Comp.DataBind();
                GridView_Inv_Tax_Comp.Visible = true;
                GridView_Inv_Tax_Comp.Columns[1].Visible = false;

                Session[SessionFactory.CREATE_INVOICE_MANUAL_TAX_COMP_DATA_GRID] = dt;
                Session[SessionFactory.CREATE_INVOICE_MANUAL_TOTAL_TAX_COMP_PERC] = totalTaxPerc;
            }
            else
            {
                Label_No_Tax_Comp_Warning.Visible = true;
                Label_No_Tax_Comp_Warning.Text = "Tax components for invoice not defined for your organization - set it up from Administration screen";
            }
        }

        protected void loadTnC(Invoice invObj)
        {

            String entId = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();

            if (invObj == null || invObj.getInvoiceId() == null || invObj.getInvoiceId().Equals(""))
            {
                ArrayList docFormatList = BackEndObjects.DocFormat.
                    getDocFormatforEntityIdandDocTypeDB(entId, BackEndObjects.DocFormat.DOCFORMAT_DOC_TYPE_INVOICE);

                for (int i = 0; i < docFormatList.Count; i++)
                {
                    BackEndObjects.DocFormat formatObj = (BackEndObjects.DocFormat)docFormatList[i];
                    if (formatObj.getSection_type().Equals(BackEndObjects.DocFormat.DOCFORMAT_DOC_TYPE_INVOICE_SECTION_TYPE_TNC))
                    {
                        TextBox_TnC.Text = formatObj.getText();
                        break;
                    }
                }
            }
            else
                TextBox_TnC.Text = invObj.getInvComments();

        }

        protected void TextBox_Taxable_Amount_TextChanged(object sender, EventArgs e)
        {
            float taxable = float.TryParse(TextBox_Taxable_Amount.Text, out taxable) ?
                float.Parse(TextBox_Taxable_Amount.Text) : 0;
            if (taxable < 0)
            {
                Label_No_Tax_Comp_Warning.Text = "Taxable Amount Should not be negative";
                TextBox_Taxable_Amount.Text = Label_Sub_Total_Amount_Value.Text;
                Label_No_Tax_Comp_Warning.Visible = true;
            }
            else  if (taxable > float.Parse(Label_Sub_Total_Amount_Value.Text))
            {
                Label_No_Tax_Comp_Warning.Text = "Taxable Amount Should be less than or eqaul to sub total";
                TextBox_Taxable_Amount.Text = Label_Sub_Total_Amount_Value.Text;
                Label_No_Tax_Comp_Warning.Visible = true;
            }
            else
            {
                Label_No_Tax_Comp_Warning.Visible = false;                
                updateTotalINVAmount();
            }
        }

        protected void GridView_Inv_Tax_Comp_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataTable dt = (DataTable)Session[SessionFactory.CREATE_INVOICE_MANUAL_TAX_COMP_DATA_GRID];
            //GridViewRow gVR = GridView_Inv_Tax_Comp.Rows[e.RowIndex];

            float taxDeductPerc = float.Parse(((TextBox)GridView_Inv_Tax_Comp.Rows[e.RowIndex].Cells[0].FindControl("TextBox_Value")).Text);
            float totalTaxPerc = (float)Session[SessionFactory.CREATE_INVOICE_MANUAL_TOTAL_TAX_COMP_PERC];

            Session[SessionFactory.CREATE_INVOICE_MANUAL_TOTAL_TAX_COMP_PERC] = totalTaxPerc - taxDeductPerc;
            updateTotalINVAmount();

            dt.Rows[e.RowIndex].Delete();

            Label_Inv_Tax_Comp_Changed.Text = "Y";


            GridView_Inv_Tax_Comp.DataSource = dt;
            GridView_Inv_Tax_Comp.DataBind();

            Session[SessionFactory.CREATE_INVOICE_MANUAL_TAX_COMP_DATA_GRID] = dt;
        }

                    //This event is fired if any text in any tax component value is changed
        protected void TextBox_Value_TextChanged(object sender, EventArgs e)
        {
            float totalTaxPerc=0,tempPerc=0;
            Label_Inv_Tax_Comp_Changed.Text = "Y";

            foreach(GridViewRow gVR in GridView_Inv_Tax_Comp.Rows)
            {
                tempPerc = float.TryParse(((TextBox)gVR.Cells[0].FindControl("TextBox_Value")).Text, out tempPerc) ?
                     float.Parse(((TextBox)gVR.Cells[0].FindControl("TextBox_Value")).Text) : 0;
                ((TextBox)gVR.Cells[0].FindControl("TextBox_Value")).Text = tempPerc.ToString();
                totalTaxPerc += tempPerc;
            }

            Session[SessionFactory.CREATE_INVOICE_MANUAL_TOTAL_TAX_COMP_PERC] = totalTaxPerc;
            updateTotalINVAmount();
        }

        protected void Button_Show_All_Tax_Comp_List_Click(object sender, EventArgs e)
        {
            Panel_Select_Tax_Comp.Visible = true;
            Label_Tax_Comp_Addn_Stat.Visible = false;
            GridView_Inv_Tax_Complete_List.Visible = true;
        }
        /// <summary>
        /// While creating invoice manually the following object details are passed to the backend DB -
        /// 1.RFQDetails (along with RFQProductServiceDetails, not RFQProdSrvQnty)
        /// 2. Invoice Details
        /// 3. Invoice Component Details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_Create_Inv_Click(object sender, EventArgs e)
        {

            bool duplInvno = false;

            if (!TextBox_Inv_No.Equals(""))
            {
                Dictionary<String, String> existingInvNoDict = (Dictionary<String,String>)Session[SessionFactory.ALL_SALE_ALL_INV_EXISTING_INV_NO];
                if (existingInvNoDict != null && existingInvNoDict.ContainsKey(TextBox_Inv_No.Text.Trim()))
                {
                    TextBox_Inv_No.Text = "Invoice No Exists =>" + TextBox_Inv_No.Text;
                    TextBox_Inv_No.ForeColor = System.Drawing.Color.Red;
                    TextBox_Inv_No.Focus();
                    duplInvno = true;
                }
                else
                {
                    TextBox_Inv_No.ForeColor = System.Drawing.Color.Black; duplInvno = false;
                }
            }
                if(!duplInvno)
                {
            if (DropDownList_Contacts.SelectedValue.Equals("_") && Label_Client_Name.Text.Equals(""))
            {
                Label_Contact_Required.Visible = true;
                Label_Contact_Required.Focus();
            }
            else if(GridView1.Rows.Count==0)
            {
                Label_Prod_Required.Visible = true;
                Label_Prod_Required.Focus();
            }
            else 
            {                
                BackEndObjects.Invoice invObj = new BackEndObjects.Invoice();
                invObj.setInvoiceId(new Id().getNewId(BackEndObjects.Id.ID_TYPE_INV_ID_STRING));

                String rfqId = Request.QueryString.GetValues("rfId")[0];
                Boolean rfqAlreadyExists = (rfqId == null || rfqId.Equals("") ? false : true);
                BackEndObjects.RFQDetails rfqObj = new RFQDetails();

                if (!rfqAlreadyExists)
                        rfqId = new Id().getNewId(BackEndObjects.Id.ID_TYPE_RFQ_STRING);

                
                ArrayList rfqSpecObjList = new ArrayList();
                Dictionary<String, ArrayList> rfqSpecObjDict = (Dictionary<String,ArrayList>)Session[SessionFactory.CREATE_INVOICE_MANUAL_SELECTED_SPEC_MAP_DICT];
                //Create the RFQ object only if does not exist already
                if (!rfqAlreadyExists)
                {
                    foreach (KeyValuePair<String, ArrayList> kvp in rfqSpecObjDict)
                    {
                        ArrayList specList = kvp.Value;
                        
                        if(specList!=null)
                        for (int i = 0; i < specList.Count; i++)
                            rfqSpecObjList.Add((RFQProductServiceDetails)specList[i]);
                    }

                    //Set the RFQ id for all the  spec objects
                    if (rfqSpecObjList != null)
                        for (int i = 0; i < rfqSpecObjList.Count; i++)
                            ((BackEndObjects.RFQProductServiceDetails)rfqSpecObjList[i]).setRFQId(rfqId);

                    //RFQDetails Object and associated details
                    rfqId = "rfq_dummy_inv_" + rfqId;
                    rfqObj.setRFQId(rfqId);
                    rfqObj.setRFQProdServList(rfqSpecObjList);
                    rfqObj.setCreatedUsr(User.Identity.Name);
                    rfqObj.setActiveStat(BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_ACTIVE);
                    rfqObj.setDueDate(TextBox_Inv_Date.Text);
                    rfqObj.setEntityId(DropDownList_Contacts.SelectedValue);
                    rfqObj.setCreatedEntity(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                    rfqObj.setLocalityId(Label_Contact_Locality_Id_Hidden.Text);
                    rfqObj.setSubmitDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    rfqObj.setTermsandConds(TextBox_TnC.Text);
                    rfqObj.setRFQName("");
                    //RFQ creation mode is manual while manually creating lead/potential/Invoice
                    rfqObj.setCreateMode(RFQDetails.CREATION_MODE_MANUAL);
                }

                //Invoice Details Object and Invoice Components            
                invObj.setRespEntityId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                invObj.setInvComments(TextBox_TnC.Text);
                invObj.setInvoiceDate(TextBox_Inv_Date.Text);
                invObj.setDeliveryStatus(BackEndObjects.DeliveryStat.DELIV_STAT_UNDELIVERED);
                invObj.setInvoiceType(BackEndObjects.InvoiceType.INVOICE_TYPE_TAX_INVOICE);
                invObj.setPaymentStatus(BackEndObjects.PaymentStat.PAYMENT_STAT_INCOMPLETE);
                invObj.setPolicyNo(TextBox_Policy_No.Text);
                invObj.setRFQId(rfqId);
                invObj.setShipVia(TextBox_Ship_Via.Text);
                invObj.setTaxableAmnt(float.Parse(TextBox_Taxable_Amount.Text));
                invObj.setTotalAmount(float.Parse(Label_Total_Amount_Value.Text));
                invObj.setCurrency(DropDownList_Curr.SelectedValue);
                if (TextBox_Inv_No.Text.Equals(""))
                    invObj.setInvoiceNo(invObj.getInvoiceId());
                else
                    invObj.setInvoiceNo(TextBox_Inv_No.Text);
                invObj.setCreationMode(BackEndObjects.Invoice.INVOICE_CREATION_MODE_MANUAL);

                ArrayList invTaxComp = new ArrayList();

                foreach (GridViewRow gVR in GridView_Inv_Tax_Comp.Rows)
                {
                    BackEndObjects.InvoiceComponents invCompObj = new BackEndObjects.InvoiceComponents();

                    invCompObj.setInvoice_Id(invObj.getInvoiceId());
                    invCompObj.setSection_type(BackEndObjects.InvoiceComponents.INVOICE_SECTION_TYPE_TAX);
                    invCompObj.setSection_type_name(((Label)gVR.Cells[0].FindControl("Label_Name")).Text);
                    invCompObj.setSection_value(((TextBox)gVR.Cells[0].FindControl("TextBox_Value")).Text);

                    invTaxComp.Add(invCompObj);
                }

                //Purchase Order Purchase Quote Object list
                //Also create the RFQProduct Service Quntity List
                BackEndObjects.PurchaseOrder POObj = new BackEndObjects.PurchaseOrder();

                String poId = new BackEndObjects.Id().getNewId(BackEndObjects.Id.ID_TYPE_PO_ID_STRING);

                POObj.setDate_created(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                POObj.setPo_id(poId);
                POObj.setPo_ship_via(TextBox_Ship_Via.Text);
                POObj.setPo_tnc(TextBox_TnC.Text);
                POObj.setRfq_id(rfqId);
                POObj.setTotal_tax_rate(Session[SessionFactory.CREATE_INVOICE_MANUAL_TOTAL_TAX_COMP_PERC].ToString());
                POObj.setRespEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

                ArrayList POQuoteObjList = new ArrayList();
                ArrayList prodSrvQntyList = new ArrayList();

                foreach (GridViewRow gVR in GridView1.Rows)
                {

                    BackEndObjects.PurchaseOrderQuote POQuoteObj = new BackEndObjects.PurchaseOrderQuote();
                    BackEndObjects.RFQProdServQnty temp = new RFQProdServQnty();

                    POQuoteObj.setPo_id(poId);
                    POQuoteObj.setProd_srv_category(((Label)gVR.Cells[0].FindControl("Label_Hidden")).Text);
                    POQuoteObj.setProduct_name(((Label)gVR.Cells[0].FindControl("Label_Product_Name")).Text);
                    POQuoteObj.setQuote(float.Parse(((TextBox)gVR.Cells[0].FindControl("TextBox_Unit_Price")).Text));
                    POQuoteObj.setUnits(float.Parse(((TextBox)gVR.Cells[0].FindControl("TextBox_Qnty")).Text));

                    POQuoteObjList.Add(POQuoteObj);

                    if (!rfqAlreadyExists)
                    {
                        temp.setRFQId(rfqId);
                        temp.setFromPrice(POQuoteObj.getQuote().ToString());
                        temp.setFromQnty(POQuoteObj.getUnits());
                        temp.setProdCatId(POQuoteObj.getProd_srv_category());

                        prodSrvQntyList.Add(temp);
                    }
                }


                try
                {
                    if (!rfqAlreadyExists)
                    {
                        rfqObj.setRFQProdServQntyList(prodSrvQntyList);
                        BackEndObjects.RFQDetails.insertRFQDetailsDB(rfqObj);
                                        }

                    //Now get the approval level rules
                    int invLevel = BackEndObjects.MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()).getInvApprovalLevel();
                    BackEndObjects.Workflow_Action actionObj = null;
                    if (invLevel > 0)
                    {
                        String reportingToUser = BackEndObjects.userDetails.
                            getUserDetailsbyIdDB(User.Identity.Name, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()).getReportsTo();
                        invObj.setApprovalStat(reportingToUser);

                        actionObj = new Workflow_Action();
                        actionObj.setEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                        actionObj.setUserId(User.Identity.Name);
                        actionObj.setActionTaken(BackEndObjects.Workflow_Action.WORKFLOW_ACTION_ACTION_TAKEN_SUBMITTED);
                        actionObj.setActionDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        actionObj.setContextId(invObj.getInvoiceId());
                        actionObj.setContextName(BackEndObjects.Workflow_Action.WORKFLOW_ACTION_CONTEXT_NAME_INV);
                        actionObj.setComment("");
                    }
                    else
                        invObj.setApprovalStat(Invoice.INVOICE_APPROVAL_STAT_APPROVED);

                    invObj.setApprovalLevel("0".ToString());

                    BackEndObjects.PurchaseOrder.insertPurchaseOrderDB(POObj);
                    BackEndObjects.PurchaseOrderQuote.insertPurchaseOrderQuoteListDB(POQuoteObjList);
                    
                    invObj.setRelatedPO(POObj.getPo_id());
                    BackEndObjects.Invoice.insertInvoiceDB(invObj);
                    BackEndObjects.InvoiceComponents.insertInvoiceComponentListDB(invTaxComp);
                    if (actionObj != null)
                        BackEndObjects.Workflow_Action.insertWorkflowActionObject(actionObj);
                    
                    

                    Label_INV_Creation_Stat.Visible = true;
                    Label_INV_Creation_Stat.ForeColor = System.Drawing.Color.Green;
                    Label_INV_Creation_Stat.Text = "Invoice Created Successfully." + (invLevel == 0 ? " Invoice will be auto approved as there is no approval rule set in Administration->WorkflowMgmt->Invoice" : "");
                    Label_INV_Id.Text = invObj.getInvoiceId();
                    TextBox_Inv_No.Text = invObj.getInvoiceNo();
                    Button_Create_Inv.Enabled = false;
                    Button_Add_Prod_Srv.Enabled = false;
                    disableOnSubmit();

                    Dictionary<String, String> existingInvNoDict = (Dictionary<String, String>)Session[SessionFactory.ALL_SALE_ALL_INV_EXISTING_INV_NO];
                    existingInvNoDict.Add(invObj.getInvoiceNo(), invObj.getInvoiceId());
                    Session[SessionFactory.ALL_SALE_ALL_INV_EXISTING_INV_NO] = existingInvNoDict;

                    DataTable dt = (DataTable)Session[SessionFactory.ALL_SALE_ALL_INV_GRID_DATA];
                    Dictionary<String, Currency> allCurrList = (Dictionary<String, Currency>)Session[SessionFactory.CURRENCY_LIST];

                    dt.Rows.Add();
                    int i = dt.Rows.Count - 1;
                    //DateUtility dU = new DateUtility();

                    dt.Rows[i]["RFQNo"] = invObj.getRFQId();
                    dt.Rows[i]["Inv_No"] = invObj.getInvoiceNo();
                    dt.Rows[i]["Inv_Id"] = invObj.getInvoiceId();
                    dt.Rows[i]["Inv_Date"] = invObj.getInvoiceDate().Replace("00", "").Replace(":", "");
                    dt.Rows[i]["Inv_Date_Ticks"] = Convert.ToDateTime(invObj.getInvoiceDate()).Ticks;
                    dt.Rows[i]["Deliv_Stat"] = invObj.getDeliveryStatus();
                    dt.Rows[i]["Pmnt_Stat"] = invObj.getPaymentStatus();
                    dt.Rows[i]["Amount"] = invObj.getTotalAmount();
                    dt.Rows[i]["approval"] = invObj.getApprovalStat();
                    dt.Rows[i]["curr"] = allCurrList.ContainsKey(invObj.getCurrency()) ?
                                        allCurrList[invObj.getCurrency()].getCurrencyName() : "";
                    dt.Rows[i]["Related_PO"] = invObj.getRelatedPO();
                    
                    dt.DefaultView.Sort = "Inv_Date_Ticks" + " " + "DESC";
                    Session[SessionFactory.ALL_SALE_ALL_INV_GRID_DATA] = dt.DefaultView.ToTable();

                    ScriptManager.RegisterStartupScript(this, typeof(string), "RefreshInvGrid", "RefreshParent();", true);
                    //string script = "this.window.opener.location=this.window.opener.location;this.window.close();";
                    //if (!ClientScript.IsClientScriptBlockRegistered("REFRESH_PARENT"))
                        //ClientScript.RegisterClientScriptBlock(typeof(string), "REFRESH_PARENT", script, true);  
                }
                catch (Exception ex)
                {
                    Label_INV_Creation_Stat.Visible = true;
                    Label_INV_Creation_Stat.ForeColor = System.Drawing.Color.Red;
                    Label_INV_Creation_Stat.Text = "Invoice Creation Failed";
                }
            }
                }
        }

        protected void GridView_Inv_Tax_Complete_List_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_Inv_Tax_Complete_List.PageIndex = e.NewPageIndex;
            GridView_Inv_Tax_Complete_List.DataSource = (DataTable)Session[SessionFactory.CREATE_INVOICE_MANUAL_TOTAL_TAX_COMP_GRID];
            GridView_Inv_Tax_Complete_List.DataBind();
        }

        protected void GridView_Inv_Tax_Complete_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session[SessionFactory.CREATE_INVOICE_MANUAL_TAX_COMP_DATA_GRID];
            GridViewRow gVR = GridView_Inv_Tax_Complete_List.Rows[GridView_Inv_Tax_Complete_List.SelectedIndex];
            Boolean alreadyExists = false;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["Comp_Name"].Equals(
                    ((Label)gVR.Cells[0].FindControl("Label_Name")).Text))
                {
                    alreadyExists = true;
                    break;
                }

            }

            if (!alreadyExists)
            {
                Label_Inv_Tax_Comp_Changed.Text = "Y";
                Label_Tax_Comp_Addn_Stat.Visible = false;

                dt.Rows.Add();

                dt.Rows[dt.Rows.Count - 1]["Hidden"] = ((Label)gVR.Cells[0].FindControl("Label_Hidden")).Text;
                dt.Rows[dt.Rows.Count - 1]["Comp_Name"] = ((Label)gVR.Cells[0].FindControl("Label_Name")).Text;
                dt.Rows[dt.Rows.Count - 1]["Comp_Value"] = ((Label)gVR.Cells[0].FindControl("Label_Value")).Text;

                GridView_Inv_Tax_Comp.DataSource = dt;
                GridView_Inv_Tax_Comp.DataBind();

                Session[SessionFactory.CREATE_INVOICE_MANUAL_TAX_COMP_DATA_GRID] = dt;

                float totalTaxPerc = (float)Session[SessionFactory.CREATE_INVOICE_MANUAL_TOTAL_TAX_COMP_PERC];
                totalTaxPerc += float.Parse(((Label)gVR.Cells[0].FindControl("Label_Value")).Text);
                Session[SessionFactory.CREATE_INVOICE_MANUAL_TOTAL_TAX_COMP_PERC] = totalTaxPerc;
                updateTotalINVAmount();

                Label_Tax_Comp_Addn_Stat.Visible = true;
                Label_Tax_Comp_Addn_Stat.ForeColor = System.Drawing.Color.Green;
                Label_Tax_Comp_Addn_Stat.Text = "Tax component added to the list";
            }
            else
            {
                Label_Tax_Comp_Addn_Stat.Visible = true;
                Label_Tax_Comp_Addn_Stat.ForeColor = System.Drawing.Color.Red;
                Label_Tax_Comp_Addn_Stat.Text = "Tax component already in the list";
            }
        }

        protected void Button_Hide_Complete_Tax_Comp_List_Click(object sender, EventArgs e)
        {
            Panel_Select_Tax_Comp.Visible = false;
            GridView_Inv_Tax_Complete_List.Visible = false;
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataTable dt =(DataTable)Session[SessionFactory.CREATE_INVOICE_MANUAL_PRODUCT_GRID];

            float totalAmnt = 0;
            
            String totalForItem = dt.Rows[e.RowIndex]["Cat_Total"].ToString();
            String catId = dt.Rows[e.RowIndex]["Hidden_Cat_Id"].ToString();
            String prevTotal = Label_Sub_Total_Amount_Value.Text;

            totalAmnt = float.Parse(prevTotal) - float.Parse(totalForItem);

            Dictionary<String, ArrayList> rfqSpecDict = (Dictionary<String, ArrayList>)Session[SessionFactory.CREATE_INVOICE_MANUAL_SELECTED_SPEC_MAP_DICT];
            rfqSpecDict.Remove(dt.Rows[e.RowIndex]["Prod_Name"].ToString());
            Session[SessionFactory.CREATE_INVOICE_MANUAL_SELECTED_SPEC_MAP_DICT] = rfqSpecDict;

            //Remove the product name from the temporary list as well
            String oldProdName = dt.Rows[e.RowIndex]["Prod_Name"].ToString();
            String prodNameList = Label_Product_Uniq_Name_List.Text;
            if (prodNameList.IndexOf("<start>"+oldProdName + "<end>"+",") >= 0)
                Label_Product_Uniq_Name_List.Text = prodNameList.Remove(prodNameList.IndexOf("<start>"+oldProdName+"<end>"), ("<start>"+oldProdName + "<end>"+",").Length);
            else if (prodNameList.IndexOf("<start>"+oldProdName+"<end>") >= 0)
                Label_Product_Uniq_Name_List.Text = prodNameList.Remove(prodNameList.IndexOf("<start>"+oldProdName+"<end>"), ("<start>"+oldProdName+"<end>").Length);

            dt.Rows[e.RowIndex].Delete();
            GridView1.DataSource = dt;
            GridView1.DataBind();

                              loadInnerGridinProdGrid(GridView1);

            Label_Sub_Total_Amount_Value.Visible = true;
            Label_Sub_Total_Amount_Value.Text = totalAmnt.ToString();
            //TextBox_Sub_Total_Amount_Value.Text = Label_Sub_Total_Amount_Value.Text;
            //if (TextBox_Taxable_Amount.Text.Equals("") || float.Parse(TextBox_Taxable_Amount.Text) == 0)
                TextBox_Taxable_Amount.Text = totalAmnt.ToString();
            Label_Total_Amount_Value.Visible = true;
            Label_Total_Amount_Value.Text = updateTotalINVAmount();

            Session[SessionFactory.CREATE_INVOICE_MANUAL_PRODUCT_GRID] = dt;
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GridView1.DataSource =(DataTable) Session[SessionFactory.CREATE_INVOICE_MANUAL_PRODUCT_GRID];
            GridView1.DataBind();
            Button_Create_Inv.Enabled = false;           
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                GridViewRow gVR = e.Row;

                Dictionary<String, ShopChildProdsInventory> childDict =(Dictionary<String,ShopChildProdsInventory>) 
                    Session[SessionFactory.CREATE_INVOICE_MANUAL_PRODUCT_GRID_PRODUCT_LIST];

                if (childDict == null || childDict.Count == 0)
                {
                    childDict = ShopChildProdsInventory.
    getAllShopChildProdObjsbyEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                    Session[SessionFactory.CREATE_INVOICE_MANUAL_PRODUCT_GRID_PRODUCT_LIST] = childDict;
                }

                String existingProdName=((Label)gVR.FindControl("Label_Product_Name_Edit")).Text;
                String selectedValue="";
                ListItem ltFirst = new ListItem();
                ltFirst.Text = "_";
                ltFirst.Value = "_";
                ((DropDownList)gVR.FindControl("DropDownList_Edit_ProdName")).Items.Add(ltFirst);

                foreach (KeyValuePair<String, ShopChildProdsInventory> kvp in childDict)
                {
                    ListItem lt = new ListItem();
                    lt.Text = kvp.Key;
                    lt.Value = kvp.Value.getUnitListPrice();
                    ((DropDownList)gVR.FindControl("DropDownList_Edit_ProdName")).Items.Add(lt);

                    if(kvp.Key.Equals(existingProdName,StringComparison.OrdinalIgnoreCase))
                        selectedValue=kvp.Value.getUnitListPrice();

                }

                if(!selectedValue.Equals(""))
                ((DropDownList)gVR.FindControl("DropDownList_Edit_ProdName")).SelectedValue = selectedValue;
                else
                    ((DropDownList)gVR.FindControl("DropDownList_Edit_ProdName")).SelectedValue = "_";
           
            }
        }

        protected void DropDownList_Edit_ProdName_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow gVR = (GridViewRow)((DropDownList)sender).Parent.Parent;
            ((TextBox)gVR.FindControl("TextBox_Unit_Price_Edit")).Text = ((DropDownList)sender).SelectedValue;

            //float unitPrice = float.Parse(!((DropDownList)sender).SelectedValue.Equals("_")?((DropDownList)sender).SelectedValue:"0");
            float unitPrice = float.TryParse(((DropDownList)sender).SelectedValue, out unitPrice) ? float.Parse(((DropDownList)sender).SelectedValue) : 0;
            float prevVal =float.Parse(((Label)gVR.FindControl("Label_Amount_Edit")).Text);
            float qnty = float.Parse(((TextBox)gVR.FindControl("TextBox_Qnty_Edit")).Text);
                        
            ((Label)gVR.FindControl("Label_Amount_Edit")).Text = (qnty * unitPrice).ToString();

            Label_Sub_Total_Amount_Value.Text = (float.Parse(Label_Sub_Total_Amount_Value.Text) +
    float.Parse(((Label)gVR.FindControl("Label_Amount_Edit")).Text) - prevVal).ToString();
            //TextBox_Sub_Total_Amount_Value.Text = Label_Sub_Total_Amount_Value.Text;

            TextBox_Taxable_Amount.Text = Label_Sub_Total_Amount_Value.Text;
            Label_Total_Amount_Value.Text = updateTotalINVAmount();
            

            DataTable dt = (DataTable)Session[SessionFactory.CREATE_INVOICE_MANUAL_PRODUCT_GRID];

            dt.Rows[gVR.RowIndex]["Prod_Name"] = ((DropDownList)sender).SelectedItem.Text;
            dt.Rows[gVR.RowIndex]["Unit_Price"]=unitPrice;
            dt.Rows[gVR.RowIndex]["Cat_Total"]=(qnty * unitPrice).ToString();
            
            Dictionary<String, ArrayList> rfqProdSrvDict =
 (Dictionary<String, ArrayList>)Session[SessionFactory.CREATE_INVOICE_MANUAL_SELECTED_SPEC_MAP_DICT];

            String prevProdName = ((Label)gVR.FindControl("Label_Product_Name_Edit")).Text;

            ArrayList tempList = rfqProdSrvDict[prevProdName];
            rfqProdSrvDict.Remove(prevProdName);
            rfqProdSrvDict.Add(((DropDownList)sender).SelectedItem.Text, tempList);

            ((Label)gVR.FindControl("Label_Product_Name_Edit")).Text = dt.Rows[gVR.RowIndex]["Prod_Name"].ToString();

            Session[SessionFactory.CREATE_INVOICE_MANUAL_SELECTED_SPEC_MAP_DICT] = rfqProdSrvDict;
            Session[SessionFactory.CREATE_INVOICE_MANUAL_PRODUCT_GRID]=dt;
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridView1.EditIndex = -1;
            GridView1.DataSource = Session[SessionFactory.CREATE_INVOICE_MANUAL_PRODUCT_GRID];
            GridView1.DataBind();
            Button_Create_Inv.Enabled = true;
            loadInnerGridinProdGrid(GridView1);
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GridView1.DataSource = (DataTable)Session[SessionFactory.CREATE_INVOICE_MANUAL_PRODUCT_GRID];
            GridView1.DataBind();
            Button_Create_Inv.Enabled = true;
            loadInnerGridinProdGrid(GridView1);
        }

        protected void TextBox_Unit_Price_Edit_TextChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gVR = (GridViewRow)((TextBox)sender).Parent.Parent.Parent.Parent;

                float unitPrice = float.Parse(((TextBox)gVR.FindControl("TextBox_Unit_Price_Edit")).Text);
                if (unitPrice < 0)
                    throw new FormatException();
                    //float.Parse(((TextBox)gVR.FindControl("TextBox_Unit_Price_Edit")).Text) : 0;
                float prevVal = float.Parse(((Label)gVR.FindControl("Label_Amount_Edit")).Text);
                float qnty =float.Parse(((TextBox)gVR.FindControl("TextBox_Qnty_Edit")).Text);
                    //float.Parse(((TextBox)gVR.FindControl("TextBox_Qnty_Edit")).Text) : 0;

                ((Label)gVR.FindControl("Label_Unit_Format")).Visible = false;

                ((Label)gVR.FindControl("Label_Amount_Edit")).Text = (qnty * unitPrice).ToString();

                ((TextBox)gVR.FindControl("TextBox_Qnty_Edit")).Text = qnty.ToString();
                ((TextBox)gVR.FindControl("TextBox_Unit_Price_Edit")).Text = unitPrice.ToString();

                Label_Sub_Total_Amount_Value.Text = (float.Parse(Label_Sub_Total_Amount_Value.Text) +
        float.Parse(((Label)gVR.FindControl("Label_Amount_Edit")).Text) - prevVal).ToString();
                //TextBox_Sub_Total_Amount_Value.Text = Label_Sub_Total_Amount_Value.Text;
                TextBox_Taxable_Amount.Text = Label_Sub_Total_Amount_Value.Text;
                Label_Total_Amount_Value.Text = updateTotalINVAmount();


                DataTable dt = (DataTable)Session[SessionFactory.CREATE_INVOICE_MANUAL_PRODUCT_GRID];

                dt.Rows[gVR.RowIndex]["Unit_Price"] = unitPrice;
                dt.Rows[gVR.RowIndex]["Cat_Total"] = (qnty * unitPrice).ToString();

                Session[SessionFactory.CREATE_INVOICE_MANUAL_PRODUCT_GRID] = dt;
            }
            catch (FormatException ex)
            {
                GridViewRow grdViewProdRow = (GridViewRow)((TextBox)sender).Parent.Parent.Parent.Parent;
                ((Label)grdViewProdRow.FindControl("Label_Unit_Format")).Visible = true;
            }
        }

        protected void TextBox_Qnty_Edit_TextChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gVR = (GridViewRow)((TextBox)sender).Parent.Parent.Parent.Parent;
                float qnty = float.Parse(((TextBox)gVR.FindControl("TextBox_Qnty_Edit")).Text);
                if (qnty < 0)
                    throw new FormatException();

                float unitPrice = float.Parse(((TextBox)gVR.FindControl("TextBox_Unit_Price_Edit")).Text);

                ((Label)gVR.FindControl("Label_Qnty_Format")).Visible = false;
                     
                float prevVal = float.Parse(((Label)gVR.FindControl("Label_Amount_Edit")).Text);
        
                   // float.Parse(((TextBox)gVR.FindControl("TextBox_Qnty_Edit")).Text) : 0;

                ((TextBox)gVR.FindControl("TextBox_Qnty_Edit")).Text = qnty.ToString();
                ((TextBox)gVR.FindControl("TextBox_Unit_Price_Edit")).Text = unitPrice.ToString();


                ((Label)gVR.FindControl("Label_Amount_Edit")).Text = (qnty * unitPrice).ToString();

                Label_Sub_Total_Amount_Value.Text = (float.Parse(Label_Sub_Total_Amount_Value.Text) +
        float.Parse(((Label)gVR.FindControl("Label_Amount_Edit")).Text) - prevVal).ToString();
                //TextBox_Sub_Total_Amount_Value.Text = Label_Sub_Total_Amount_Value.Text;

                TextBox_Taxable_Amount.Text = Label_Sub_Total_Amount_Value.Text;
                Label_Total_Amount_Value.Text = updateTotalINVAmount();


                DataTable dt = (DataTable)Session[SessionFactory.CREATE_INVOICE_MANUAL_PRODUCT_GRID];

                dt.Rows[gVR.RowIndex]["Qnty"] = qnty;
                dt.Rows[gVR.RowIndex]["Cat_Total"] = (qnty * unitPrice).ToString();

                Session[SessionFactory.CREATE_INVOICE_MANUAL_PRODUCT_GRID] = dt;
            }
            catch (FormatException ex)
            {
                GridViewRow grdViewProdRow = (GridViewRow)((TextBox)sender).Parent.Parent.Parent.Parent;
                ((Label)grdViewProdRow.FindControl("Label_Qnty_Format")).Visible = true;
            }
        }

        protected void disableOnSubmit()
        {
            //((TextBox)form1.FindControl("TextBox_TnC")).Enabled = false;
            Button_Create_Inv.Enabled = false;
            TextBox_TnC.Enabled = false;
            Button_Show_All_Tax_Comp_List.Enabled = false;
            DropDownList_Curr.Enabled = false;
            GridView_Inv_Tax_Comp.Enabled = false;
            TextBox_Taxable_Amount.Enabled = false;
            GridView1.Enabled = false;
            TextBox_Ship_Via.Enabled = false;
            TextBox_Policy_No.Enabled = false;
            DropDownList_Contacts.Enabled = false;
            TextBox_Inv_Date.Enabled = false;
            TextBox_Inv_No.Enabled = false;
            
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GridView_Prod_Srv_RowDataBound(object sender, GridViewRowEventArgs e)
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
                        //e.Row.Cells[5].Enabled = false;
                    }
                }
            }
        }

        protected void Button_Empty_SO_Click(object sender, EventArgs e)
        {
            PanelEmptySOWarning.Visible = false;
        }


    }
}