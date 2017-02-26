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
using System.IO;

namespace OnLine.Pages.Popups.Purchase
{
    public partial class PO_Details : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                clearSessionVariables();
                String rfId = Request.QueryString.GetValues("rfId")[0];
                 String context = Request.QueryString.GetValues("context")[0];
                 String poId = Request.QueryString.GetValues("poId")[0];

                bool allowEdit=Request.QueryString.GetValues("allowEdit") != null && Request.QueryString.GetValues("allowEdit")[0].Equals("true")?true:false;
                 if (allowEdit)
                 {
                     Button_Add_Prod_Srv.Visible = true;
                     LoadProductCat();
                     Panel_PO_Change_Warning.Visible = true;
                     Button_Update_PO_By_Client.Visible = true;
                 }

                String otherEntId="";

                RFQShortlisted rfqDealObj = BackEndObjects.RFQShortlisted.getRFQShortlistedEntryforFinalizedVendor(rfId);

                if (context.Equals("client") || context.Equals("clientTaggedRFQ"))
                {
                    otherEntId = rfqDealObj.getRespEntityId();

                    if (otherEntId == null || otherEntId.Equals("")) //This scenario is when the PO is manually created from the purchase screen and is viewed from the PO grid
                        otherEntId = BackEndObjects.PurchaseOrder.getPurchaseOrderforPoIdDB(poId).getRespEntId();
                    //Disable the edit options if the deal is closed
                    if (otherEntId != null && !otherEntId.Equals(""))
                    {
                        if (!allowEdit)
                        {
                            TextBox_Ship_Via.Enabled = false;
                            TextBox_tax.Enabled = false;
                            TextBox_TnC.Enabled = false;
                            //Button_Gen_PDF.Visible = false;
                        }
                        Label_Deal_Status.Text = "Closed";
                        //Button_Update_PO.Visible = false;
                    }
                }
                else
                {
                    Session["update_po_context"] = "vendor";

                    Panel_PO_Change_Warning.Visible = false;

                    otherEntId = RFQDetails.getRFQDetailsbyIdDB(rfId).getEntityId();
                    String createMode =Request.QueryString.GetValues("createMode")!=null? Request.QueryString.GetValues("createMode")[0]:"";
                    Label1.Text = "Sales Order#";
                    //For manually created potentail allow edit for the sales order
                    if (!createMode.Equals(BackEndObjects.RFQShortlisted.POTENTIAL_CREATION_MODE_MANUAL) && !allowEdit)
                    {
                        TextBox_Ship_Via.Enabled = false;
                        TextBox_tax.Enabled = false;
                        TextBox_TnC.Enabled = false;
                        //Button_Gen_PDF.Visible = false;
                        //Button_Update_PO.Visible = false;
                    }
                }
                PurchaseOrder POObj = PurchaseOrder.getPurchaseOrderforRFQIdDB(rfId);
                //Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SELECTED_PO] = POObj.getPo_id();
                
                loadClientAndVendorDetails(rfId, otherEntId);
                loadTnC(POObj.getPo_tnc());
                loadTax_POIDAndShipping(POObj);
                loadProductGrid(poId);
                populateLogo(otherEntId);
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

        protected void clearSessionVariables()
        {
            Session[SessionFactory.UPDATE_PO_MANUAL_PRODUCT_GRID] = null;
            Session[SessionFactory.UPDATE_PO_MANUAL_SELECTED_SPEC_MAP_DICT] = null;

        }

        protected void loadTax_POIDAndShipping(PurchaseOrder poObj)
        {
            TextBox_tax.Text = poObj.getTotal_tax_rate();
            TextBox_Ship_Via.Text = poObj.getPo_ship_via();
            Label_PO_No.Text = poObj.getPo_id();
        }

        protected void loadClientAndVendorDetails(String rfId, String otherEntId)
        {
            String entId = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();
            String context = Request.QueryString.GetValues("context")[0];    

            String localId = "";
            String vendDetails = "";
            String custDetails = "";
            String vendContact = "";
            String custContact = "";

            Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

            /*if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_POTENTIAL] &&
!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                Button_Update_PO.Enabled = false;*/

            //Populate the vendor details
            Dictionary<String, Object> vendObj = ActionLibrary.customerDetails.
                getContactDetails(otherEntId, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

            if (vendObj.ContainsKey(ActionLibrary.customerDetails.RETURN_OBJECT_TYPE_CONTACT_DETAILS))
            {
                BackEndObjects.Contacts cOBJ = (BackEndObjects.Contacts)vendObj[ActionLibrary.customerDetails.RETURN_OBJECT_TYPE_CONTACT_DETAILS];

                if (context.Equals("client") || context.Equals("clientTaggedRFQ"))
                {
                    Label_Vendor_Name.Text = cOBJ.getContactName();
                    vendContact = cOBJ.getMobNo();
                    vendDetails += cOBJ.getStreetName()+",";
                    localId = cOBJ.getLocalityId();
                }
                else
                {
                    Label_Client_Name.Text = cOBJ.getContactName();
                    custContact = cOBJ.getMobNo();
                    custDetails += cOBJ.getStreetName()+",";
                    localId = cOBJ.getLocalityId();
                }
            }
            else
            {
                BackEndObjects.MainBusinessEntity mBObj = (BackEndObjects.MainBusinessEntity)vendObj[ActionLibrary.customerDetails.RETURN_OBJECT_TYPE_MAIN_BUSINESS_ENTITY];
                BackEndObjects.AddressDetails addrObjEnt = BackEndObjects.AddressDetails.getAddressforMainBusinessEntitybyIdDB(mBObj.getEntityId());
                if (context.Equals("client") || context.Equals("clientTaggedRFQ"))
                {
                    Label_Vendor_Name.Text = mBObj.getEntityName();
                    vendContact = mBObj.getPhNo();
                    if (addrObjEnt != null)
                        vendDetails += addrObjEnt.getAddrLine1()+",";
                }
                else
                {
                    Label_Client_Name.Text = mBObj.getEntityName();
                    custContact = mBObj.getPhNo();
                    if (addrObjEnt != null)
                        custDetails += addrObjEnt.getAddrLine1()+",";
                }
                if (mBObj.getAddressDetails() != null)
                    localId = mBObj.getAddressDetails().getLocalityId();
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

                if (context.Equals("client") || context.Equals("clientTaggedRFQ"))
                vendDetails += lclObj.getLocalityName() + "<br/>" + ctObj.getCityName() + "<br/>" + stObj.getStateName() + "<br/>" + cntObj.getCountryName() + "<br/> Phone:" + vendContact;
                else
                    custDetails += lclObj.getLocalityName() + "<br/>" + ctObj.getCityName() + "<br/>" + stObj.getStateName() + "<br/>" + cntObj.getCountryName() + "<br/> Phone:" + custContact;
            }
            if (context.Equals("client") || context.Equals("clientTaggedRFQ"))
                Label_Vendor_Addr.Text = vendDetails;
            else
                Label_Client_Addr.Text = custDetails;

            //Get the customer details
            BackEndObjects.MainBusinessEntity custObj = BackEndObjects.MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(entId);

            if (context.Equals("client") || context.Equals("clientTaggedRFQ"))
            {
                Label_Client_Name.Text = custObj.getEntityName();
                custContact = custObj.getPhNo();
            }
            else
            {
                Label_Vendor_Name.Text = custObj.getEntityName();
                vendContact = custObj.getPhNo();
            }

            BackEndObjects.AddressDetails addrObj = BackEndObjects.AddressDetails.getAddressforMainBusinessEntitybyIdDB(entId);

            if (addrObj.getLocalityId() != null && !addrObj.getLocalityId().Equals(""))
            {
                if (context.Equals("client") || context.Equals("clientTaggedRFQ"))
                custDetails += addrObj.getAddrLine1() + ",";
                else
                    vendDetails += addrObj.getAddrLine1() + ",";
                
                localId = addrObj.getLocalityId();

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

                    if (context.Equals("client") || context.Equals("clientTaggedRFQ"))
                        custDetails += lclObj.getLocalityName() + "<br/>" + ctObj.getCityName() + "<br/>" + stObj.getStateName() + "<br/>" + cntObj.getCountryName() + "<br/> Phone:" + custContact;
                    else
                        vendDetails += lclObj.getLocalityName() + "<br/>" + ctObj.getCityName() + "<br/>" + stObj.getStateName() + "<br/>" + cntObj.getCountryName() + "<br/> Phone:" + vendContact;
                }
            }
            if (context.Equals("client") || context.Equals("clientTaggedRFQ"))
                Label_Client_Addr.Text = custDetails;
            else
                Label_Vendor_Addr.Text = vendDetails;
        }

        protected void loadTnC(String tnc)
        {
            TextBox_TnC.Text = tnc;
        }

        protected void loadProductGrid(String poId)
        {
            ArrayList rfqSpecList=new ArrayList();
            String context=Request.QueryString.GetValues("context")[0];

            if (context.Equals("client"))
                rfqSpecList = (ArrayList)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_RFQ_SPEC];
            else if (context.Equals("clientTaggedRFQ"))//From Tagged RFQ screen
                rfqSpecList = BackEndObjects.RFQProductServiceDetails.getAllProductServiceDetailsbyRFQIdDB(Request.QueryString.GetValues("rfId")[0]);
            else
            {
                String[] subContext = Request.QueryString.GetValues("subContext");
                if (subContext != null && subContext[0] != null && subContext[0].Equals("SOFromProduct"))
                    rfqSpecList = RFQProductServiceDetails.getAllProductServiceDetailsbyRFQIdDB(Request.QueryString.GetValues("rfId")[0]);
                else
                rfqSpecList = (ArrayList)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_POTN_SPEC];
            }

            ArrayList poQuoteList=PurchaseOrderQuote.getPurcahseOrderQuoteListbyPOIdDB(poId);

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
                String prodName = poQntyObj.getProduct_name();
                prodName = (prodName == null || prodName.Equals("") ? "product serial" + GridView1.Rows.Count + 1 : prodName);

                dt.Rows.Add();
                dt.Rows[i]["Hidden_Cat_Id"] = catId;
                dt.Rows[i]["Serial"] = i + 1;
                dt.Rows[i]["Qnty"] = qnty.ToString();
                dt.Rows[i]["Prod_Name"] = poQntyObj.getProduct_name();
                dt.Rows[i]["Cat_Name"] = BackEndObjects.ProductCategory.getProductCategorybyIdwoFeaturesDB(catId).getProductCategoryName();
                dt.Rows[i]["Unit_Price"] = poQntyObj.getQuote();
                dt.Rows[i]["Cat_Total"] = qnty * poQntyObj.getQuote();

                totalAmnt += poQntyObj.getQuote() * qnty;

                if (Button_Add_Prod_Srv.Visible)
                {
                    Dictionary<String, RFQProductServiceDetails> rfqProdSpecDictForCategory = BackEndObjects.RFQProductServiceDetails.
       getAllProductServiceDetailsbyRFQandProductIdDB(Request.QueryString.GetValues("rfId")[0], catId);
                    ArrayList rfqProdSrvList = new ArrayList();

                    foreach (KeyValuePair<String, RFQProductServiceDetails> kvp in rfqProdSpecDictForCategory)
                        rfqProdSrvList.Add(kvp.Value);

                    Dictionary<String, ArrayList> rfqProdSrvDict = (Dictionary<String, ArrayList>)Session[SessionFactory.UPDATE_PO_MANUAL_SELECTED_SPEC_MAP_DICT];
                    if (rfqProdSrvDict == null)
                        rfqProdSrvDict = new Dictionary<string, ArrayList>();

                    if (!rfqProdSrvDict.ContainsKey(prodName))
                        rfqProdSrvDict.Add(prodName, rfqProdSrvList);

                    Session[SessionFactory.UPDATE_PO_MANUAL_SELECTED_SPEC_MAP_DICT] = rfqProdSrvDict;
                    
                    Label_Product_Uniq_Name_List.Text += "," + "<start>"+prodName+"<end>";
                }
            }


            GridView1.Visible = true;
            GridView1.DataSource = dt;
            GridView1.DataBind();
            GridView1.Columns[2].Visible = false;
            if (Button_Add_Prod_Srv.Visible)
            {
                GridView1.Columns[0].Visible = true;
                GridView1.Columns[1].Visible = true;
            }
            Session[SessionFactory.UPDATE_PO_MANUAL_PRODUCT_GRID] = dt;

            String createMode="";
            if(context.Equals("vendor"))
                createMode = Request.QueryString.GetValues("createMode")!=null?Request.QueryString.GetValues("createMode")[0]:"";

            foreach (GridViewRow gVR in GridView1.Rows)
            {
                if (!context.Equals("client") && !createMode.Equals(BackEndObjects.RFQShortlisted.POTENTIAL_CREATION_MODE_MANUAL))//Allow edit only if from RFQ section of the purchase screen and not even from tagged RFQ list
                {
                    ((TextBox)gVR.Cells[2].FindControl("TextBox_Qnty")).Enabled = false;
                    ((TextBox)gVR.Cells[2].FindControl("TextBox_Unit_Price")).Enabled = false;
                }
                else if (Label_Deal_Status.Text.Trim().Equals("Closed"))
                {
                    ((TextBox)gVR.Cells[2].FindControl("TextBox_Qnty")).Enabled = false;
                    ((TextBox)gVR.Cells[2].FindControl("TextBox_Unit_Price")).Enabled = false;
                }

                DataTable dtSpec = new DataTable();
                dtSpec.Columns.Add("Hidden");
                dtSpec.Columns.Add("FeatName");
                dtSpec.Columns.Add("SpecText");
                dtSpec.Columns.Add("FromSpec");
                dtSpec.Columns.Add("ToSpec");
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
            Label_Total_Amount_Value.Visible = true;
            Label_Total_Amount_Value.Text = updateTotalPOAmount();

        }

        protected void populateLogo(String otherEntId)
        {
            String context = Request.QueryString.GetValues("context")[0];
            String entId="";
            if (context.Equals("client") || context.Equals("clientTaggedRFQ"))
                entId=Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();
            else
                entId=otherEntId;

            ArrayList imgListObjs = BackEndObjects.Image.
                getImagebyTypeandEntId(BackEndObjects.Image.PICTURE_IMG_TYPE_LOGO, entId);
            if (imgListObjs.Count > 0)
            {
                //Only consider the first image object for logo
                BackEndObjects.Image imgObj = (BackEndObjects.Image)imgListObjs[0];
                Image_Logo.ImageUrl = imageToURL(imgObj.getImgPath());
                Image_Logo.Visible = true;
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
                System.Drawing.Image img = System.Drawing.Image.FromFile(sPath);
                System.Drawing.Bitmap newBitMap = new System.Drawing.Bitmap(img);

                generateImagePath(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

                newBitMap.Save(Server.MapPath(finalImageUrl), System.Drawing.Imaging.ImageFormat.Jpeg);
            }

            return finalImageUrl;
        }

        protected void TextBox_tax_TextChanged(object sender, EventArgs e)
        {
            Label_Total_Amount_Value.Text = updateTotalPOAmount();
        }

        protected String updateTotalPOAmount()
        {
            //float tax = TextBox_tax.Text.Equals("") ? 0 : float.Parse(TextBox_tax.Text);
            float tax = float.TryParse(TextBox_tax.Text,out tax) ? float.Parse(TextBox_tax.Text):0;
            return ((float.Parse(Label_Sub_Total_Amount_Value.Text) * tax) / 100 + float.Parse(Label_Sub_Total_Amount_Value.Text)).ToString();
        }

        protected void TextBox_Qnty_TextChanged(object sender, EventArgs e)
        {
            try
            {
                float qnty = float.Parse(((TextBox)sender).Text);
                if (qnty < 0)
                    throw new FormatException(); 

                GridViewRow grdViewProdRow = (GridViewRow)((TextBox)sender).Parent.Parent.Parent.Parent;
                ((Label)grdViewProdRow.FindControl("Label_Qnty_Format")).Visible = false;

                float unitPrice = float.Parse(((TextBox)grdViewProdRow.FindControl("TextBox_Unit_Price")).Text);

                float prevVal = float.Parse(((Label)grdViewProdRow.FindControl("Label_Amount")).Text);

                ((Label)grdViewProdRow.FindControl("Label_Amount")).Text = (qnty * unitPrice).ToString();

                Label_Sub_Total_Amount_Value.Text = (float.Parse(Label_Sub_Total_Amount_Value.Text) +
                    float.Parse(((Label)grdViewProdRow.FindControl("Label_Amount")).Text) - prevVal).ToString();

                Label_Total_Amount_Value.Text = updateTotalPOAmount();
            }
            catch (FormatException fx)
            {
                GridViewRow grdViewProdRow = (GridViewRow)((TextBox)sender).Parent.Parent.Parent.Parent;
                ((Label)grdViewProdRow.FindControl("Label_Qnty_Format")).Visible = true;
            }
        }

        protected void TextBox_Unit_Price_TextChanged(object sender, EventArgs e)
        {
            try
            {
                float unitPrice = float.Parse(((TextBox)sender).Text);
                if (unitPrice < 0)
                    throw new FormatException();

                GridViewRow grdViewProdRow = (GridViewRow)((TextBox)sender).Parent.Parent.Parent.Parent;
                ((Label)grdViewProdRow.FindControl("Label_Unit_Format")).Visible = false;

                float qnty = float.Parse(((TextBox)grdViewProdRow.FindControl("TextBox_Qnty")).Text);

                float prevVal = float.Parse(((Label)grdViewProdRow.FindControl("Label_Amount")).Text);

                ((Label)grdViewProdRow.FindControl("Label_Amount")).Text = (qnty * unitPrice).ToString();

                Label_Sub_Total_Amount_Value.Text = (float.Parse(Label_Sub_Total_Amount_Value.Text) +
        float.Parse(((Label)grdViewProdRow.FindControl("Label_Amount")).Text) - prevVal).ToString();

                Label_Total_Amount_Value.Text = updateTotalPOAmount();
            }
            catch (FormatException fx)
            {
                GridViewRow grdViewProdRow = (GridViewRow)((TextBox)sender).Parent.Parent.Parent.Parent;
                ((Label)grdViewProdRow.FindControl("Label_Unit_Format")).Visible = true;
            }
        }

          protected void Button_Update_PO_Click(object sender, EventArgs e)
        {

            String poId = Request.QueryString.GetValues("poId")[0];
            String context = Request.QueryString.GetValues("context")[0];

            try
            {
            Dictionary<String, String> whereCls = new Dictionary<String, String>();
            whereCls.Add(BackEndObjects.PurchaseOrder.PURCHASE_ORDER_COL_PO_ID, poId);

            Dictionary<String, String> targetVals = new Dictionary<String, String>();
            targetVals.Add(BackEndObjects.PurchaseOrder.PURCHASE_ORDER_COL_PO_SHIP_VIA, TextBox_Ship_Via.Text);
            targetVals.Add(BackEndObjects.PurchaseOrder.PURCHASE_ORDER_COL_PO_TNC, TextBox_TnC.Text);
            targetVals.Add(BackEndObjects.PurchaseOrder.PURCHASE_ORDER_COL_TOTAL_TAX_RATE, TextBox_tax.Text);

            BackEndObjects.PurchaseOrder.updatePurchaseOrderDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);

            foreach (GridViewRow gVR in GridView1.Rows)
            {

                BackEndObjects.PurchaseOrderQuote POQuoteObj = new BackEndObjects.PurchaseOrderQuote();

                Dictionary<String, String> whereClsPoQuote = new Dictionary<String, String>();
                whereClsPoQuote.Add(BackEndObjects.PurchaseOrderQuote.PURCHASE_ORDER_QUOTE_DETAILS_COL_PO_ID, poId);
                whereClsPoQuote.Add(BackEndObjects.PurchaseOrderQuote.PURCHASE_ORDER_QUOTE_DETAILS_COL_PROD_SRV_CATEGORY, ((Label)gVR.Cells[0].FindControl("Label_Hidden")).Text);


                Dictionary<String, String> targetValsPoQuote = new Dictionary<String, String>();
                targetValsPoQuote.Add(BackEndObjects.PurchaseOrderQuote.PURCHASE_ORDER_QUOTE_DETAILS_COL_QUOTE, ((TextBox)gVR.Cells[0].FindControl("TextBox_Unit_Price")).Text);
                targetValsPoQuote.Add(BackEndObjects.PurchaseOrderQuote.PURCHASE_ORDER_QUOTE_DETAILS_COL_UNITS, ((TextBox)gVR.Cells[0].FindControl("TextBox_Qnty")).Text);

                BackEndObjects.PurchaseOrderQuote.updatePurchaseOrderQuoteDB(targetValsPoQuote, whereClsPoQuote, DBConn.Connections.OPERATION_UPDATE);
               
            }
                
                Label_PO_No.Visible = true;
                Label_PO_No.Text = poId;
                Label_PO_Creation_Stat.Visible = true;
                Label_PO_Creation_Stat.ForeColor = System.Drawing.Color.Green;
                if(context.Equals("client"))
                Label_PO_Creation_Stat.Text = "Purchase Order Updated Successfully";
                else
                    Label_PO_Creation_Stat.Text = "Sales Order Updated Successfully";

            }
            catch (Exception ex)
            {
                Label_PO_Creation_Stat.Visible = true;
                Label_PO_Creation_Stat.ForeColor = System.Drawing.Color.Red;
                if (context.Equals("client"))
                Label_PO_Creation_Stat.Text = "Purchase Order Update Failed";
                else
                    Label_PO_Creation_Stat.Text = "Sales Order Update Failed";
            }
        }

          protected void Button_Gen_PDF_Click(object sender, EventArgs e)
          {
              clearAllFields("form1");
              Label_PO_Creation_Stat.Visible = false;
              //Button_Update_PO.Visible = false;
              Button_Update_PO_By_Client.Visible = false;
              Button_Add_Prod_Srv.Visible = false;
              Panel_PO_Change_Warning.Visible = false;

              foreach (GridViewRow gVR in GridView1.Rows)
              {
                  ((TextBox)gVR.Cells[2].FindControl("TextBox_Qnty")).ReadOnly = true;
                  ((TextBox)gVR.Cells[2].FindControl("TextBox_Qnty")).BackColor = System.Drawing.Color.Transparent;
                  ((TextBox)gVR.Cells[2].FindControl("TextBox_Qnty")).BorderStyle = BorderStyle.None;

                  ((TextBox)gVR.Cells[2].FindControl("TextBox_Unit_Price")).ReadOnly = true;
                  ((TextBox)gVR.Cells[2].FindControl("TextBox_Unit_Price")).BackColor = System.Drawing.Color.Transparent;
                  ((TextBox)gVR.Cells[2].FindControl("TextBox_Unit_Price")).BorderStyle = BorderStyle.None;
              }
              Button_Gen_PDF.Visible = false;
              TextBox_tax.ReadOnly = true;
              TextBox_tax.BorderStyle = BorderStyle.None;
              TextBox_tax.BackColor = System.Drawing.Color.Transparent;

              ASPXToPDF1.RenderAsPDF("PO.pdf");
              //Response.Redirect("dispPOPDF.aspx");
              //Server.Transfer("dispPOPDF.aspx");
          }

          protected void clearAllFields(String id)
          {
              Control myForm = FindControl(id);

              foreach (Control ctl in myForm.Controls)
              {
                  if (ctl.GetType().ToString().Equals("System.Web.UI.WebControls.TextBox"))
                  {
                      ((TextBox)ctl).ReadOnly = true;
                      ((TextBox)ctl).BorderStyle = BorderStyle.None;
                      ((TextBox)ctl).BackColor = System.Drawing.Color.Transparent;
                  }
                  /*if (ctl.GetType().ToString().Equals("System.Web.UI.WebControls.DropDownList"))
                      ((DropDownList)ctl).SelectedIndex = -1;
                  if (ctl.GetType().ToString().Equals("System.Web.UI.WebControls.GridView"))
                  { ((GridView)ctl).DataSource = null; ((GridView)ctl).Visible = false; }*/
              }

          }

          protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
          {
              if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
              {
                  GridViewRow gVR = e.Row;

                  String context = Request.QueryString.GetValues("context")[0];
                  if (context.Equals("vendor"))
                  {
                      ((DropDownList)gVR.FindControl("DropDownList_Edit_ProdName")).Visible = true;
                      ((TextBox)gVR.FindControl("TextBox_Prod_Name_Edit")).Visible = false;

                      Dictionary<String, ShopChildProdsInventory> childDict = (Dictionary<String, ShopChildProdsInventory>)
                          Session[SessionFactory.UPDATE_PO_MANUAL_PRODUCT_GRID_PRODUCT_LIST];

                      if (childDict == null || childDict.Count == 0)
                      {
                          childDict = ShopChildProdsInventory.
          getAllShopChildProdObjsbyEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                          Session[SessionFactory.UPDATE_PO_MANUAL_PRODUCT_GRID_PRODUCT_LIST] = childDict;
                      }

                      String existingProdName = ((Label)gVR.FindControl("Label_Product_Name_Edit")).Text;
                      String selectedValue = "";
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

                          if (kvp.Key.Equals(existingProdName, StringComparison.OrdinalIgnoreCase))
                              selectedValue = kvp.Value.getUnitListPrice();

                      }

                      if (!selectedValue.Equals(""))
                      {
                          ((DropDownList)gVR.FindControl("DropDownList_Edit_ProdName")).SelectedItem.Text = existingProdName;
                          //((DropDownList)gVR.FindControl("DropDownList_Edit_ProdName")).SelectedValue = selectedValue;
                      }
                      else
                          ((DropDownList)gVR.FindControl("DropDownList_Edit_ProdName")).SelectedValue = "_";
                  }
                  else
                  {
                      ((DropDownList)gVR.FindControl("DropDownList_Edit_ProdName")).Visible = false;
                      ((TextBox)gVR.FindControl("TextBox_Prod_Name_Edit")).Visible = true;
                  }
              }
          }

          protected void Button_Add_Prod_Srv_Click(object sender, EventArgs e)
          {
              clearProdSelectionPanel();
              loadProdList();
              Panel_Prod_Service_Det.Visible = true;
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
      Session[SessionFactory.UPDATE_PO_MANUAL_PRODUCT_GRID_PRODUCT_LIST];

              if (childDict == null || childDict.Count == 0)
              {
                  childDict = ShopChildProdsInventory.
  getAllShopChildProdObjsbyEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                  Session[SessionFactory.UPDATE_PO_MANUAL_PRODUCT_GRID_PRODUCT_LIST] = childDict;
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
              DropDownList_Level3.Items.Clear();
              Session[SessionFactory.UPDATE_PO_MANUAL_SELECTED_PRODUCT_CAT] = DropDownList_Level1.SelectedValue;

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
              Session[SessionFactory.UPDATE_PO_MANUAL_SELECTED_PRODUCT_CAT] = DropDownList_Level2.SelectedValue;
          }

          protected void DropDownList_Level3_SelectedIndexChanged(object sender, EventArgs e)
          {
              Session[SessionFactory.UPDATE_PO_MANUAL_SELECTED_PRODUCT_CAT] = DropDownList_Level3.SelectedValue;
          }
          /// <summary>
          /// This method its to fill the product service selection grid
          /// </summary>
          protected void fillGrid()
          {
              String selectedProdCatId = Session[SessionFactory.UPDATE_PO_MANUAL_SELECTED_PRODUCT_CAT].ToString();
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

          protected void GridView_Prod_Srv_PageIndexChanging(object sender, GridViewPageEventArgs e)
          {
              GridView_Prod_Srv.PageIndex = e.NewPageIndex;
              GridView_Prod_Srv.SelectedIndex = -1;
              fillGrid();
          }

          [System.Web.Services.WebMethod]
          [System.Web.Script.Services.ScriptMethod]
          public static String[] GetCompletionListProd(string prefixText, int count)
          {
              String context = HttpContext.Current.Session["update_po_context"] != null ? HttpContext.Current.Session["update_po_context"].ToString() : "";

              if (context.Equals("vendor"))
              {
                  Dictionary<String, ShopChildProdsInventory> ProdNameDict = ((Dictionary<String, ShopChildProdsInventory>)
                      HttpContext.Current.Session[SessionFactory.UPDATE_PO_MANUAL_PRODUCT_GRID_PRODUCT_LIST]);

                  Dictionary<String, String> prodNameCostDict = new Dictionary<string, string>();

                  ArrayList temp = new ArrayList();
                  foreach (KeyValuePair<String, ShopChildProdsInventory> kvp in ProdNameDict)
                  {
                      if (kvp.Key.IndexOf(prefixText.Trim(), StringComparison.InvariantCultureIgnoreCase) >= 0)
                          temp.Add(kvp.Key);
                  }

                  return (String[])temp.ToArray(typeof(String));
              }
              else
                  return null;
          }

          protected void hdnValue_ValueChangedProd(object sender, EventArgs e)
          {
              String context = HttpContext.Current.Session["update_po_context"] != null ? HttpContext.Current.Session["update_po_context"].ToString() : "";

              if (context.Equals("vendor"))
              {
                  string selectedVal = ((HiddenField)sender).Value;
                  Dictionary<String, ShopChildProdsInventory> ProdNameDict = ((Dictionary<String, ShopChildProdsInventory>)
          HttpContext.Current.Session[SessionFactory.UPDATE_PO_MANUAL_PRODUCT_GRID_PRODUCT_LIST]);
                  if (ProdNameDict.ContainsKey(selectedVal))
                      TextBox_Prod_Unit_Price.Text = ProdNameDict[selectedVal].getUnitListPrice();
              }

              /*String[] rfqArray = (String[])Session[SessionFactory.ALL_DEFECT_CREATE_DEFECT_RFQ_AUTOCOMPLETE_LIST];

              for (int i = 0; i < rfqArray.Length; i++)
                  if (rfqArray[i].Equals(selectedVal))
                  {
                      TextBox_Rfq_No.Text = rfqArray[i].Substring(rfqArray[i].IndexOf("(") + 1, (rfqArray[i].Length - 2 - rfqArray[i].IndexOf("(")));
                      break;
                  }*/

          }

          protected void Buttin_Show_Spec_List1_Click(object sender, EventArgs e)
          {
              fillGrid();
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

          protected void GridView_Prod_Srv_SelectedIndexChanged(object sender, EventArgs e)
          {
              ArrayList rfqProdSrvList = (ArrayList)Session[SessionFactory.UPDATE_PO_MANUAL_SELECTED_SPEC_MAP];
              if (rfqProdSrvList == null)
                  rfqProdSrvList = new ArrayList();

              BackEndObjects.RFQProductServiceDetails rfqSpec = new RFQProductServiceDetails();
              rfqSpec.setPrdCatId(Session[SessionFactory.UPDATE_PO_MANUAL_SELECTED_PRODUCT_CAT].ToString());
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

              Session[SessionFactory.UPDATE_PO_MANUAL_SELECTED_SPEC_MAP] = rfqProdSrvList;
          }

          protected void getAddintionalProdSrvList()
          {
              ArrayList rfqProdSrvList = (ArrayList)Session[SessionFactory.UPDATE_PO_MANUAL_SELECTED_SPEC_MAP];
              if (rfqProdSrvList == null)
                  rfqProdSrvList = new ArrayList();

              BackEndObjects.RFQProductServiceDetails rfqSpec = new RFQProductServiceDetails();
              rfqSpec.setPrdCatId(Session[SessionFactory.UPDATE_PO_MANUAL_SELECTED_PRODUCT_CAT].ToString());
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

              Session[SessionFactory.UPDATE_PO_MANUAL_SELECTED_SPEC_MAP] = rfqProdSrvList;
          }

          protected void Button_Add_Prod_Srv_Det_Click(object sender, EventArgs e)
          {
              if (Label_Product_Uniq_Name_List.Text.IndexOf("<start>"+TextBox_Prod_Name.Text.Trim()+"<end>") > 0)
                  Label_Prod_Name.ForeColor = System.Drawing.Color.Red;
              else
              {
                  Label_Invalid_Prod_Cat.Visible = false;
                  //Label_Prod_Required.Visible = false;

                  String catId = null;
                  try
                  {
                      catId = Session[SessionFactory.UPDATE_PO_MANUAL_SELECTED_PRODUCT_CAT].ToString();
                  }
                  catch (Exception ex)
                  {
                  }

                  bool addProd = true;

                  if ((catId == null || catId.Equals("")))
                  {
                      Dictionary<String, ShopChildProdsInventory> ProdNameDict = ((Dictionary<String, ShopChildProdsInventory>)
      HttpContext.Current.Session[SessionFactory.UPDATE_PO_MANUAL_PRODUCT_GRID_PRODUCT_LIST]);
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
                      ArrayList rfqProdSrvList = (ArrayList)Session[SessionFactory.UPDATE_PO_MANUAL_SELECTED_SPEC_MAP];
                      Dictionary<String, ArrayList> rfqProdSrvDict = (Dictionary<String, ArrayList>)Session[SessionFactory.UPDATE_PO_MANUAL_SELECTED_SPEC_MAP_DICT];
                      if (rfqProdSrvDict == null)
                          rfqProdSrvDict = new Dictionary<string, ArrayList>();

                      rfqProdSrvDict.Add(TextBox_Prod_Name.Text, rfqProdSrvList);

                      //Put the cleared list and newly filled dictionary in the session
                      Session[SessionFactory.UPDATE_PO_MANUAL_SELECTED_SPEC_MAP] = new ArrayList();
                      Session[SessionFactory.UPDATE_PO_MANUAL_SELECTED_SPEC_MAP_DICT] = rfqProdSrvDict;

                      DataTable dt = (DataTable)Session[SessionFactory.UPDATE_PO_MANUAL_PRODUCT_GRID];

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

                      Session[SessionFactory.UPDATE_PO_MANUAL_PRODUCT_GRID] = dt;

                      Label_Selected_List.Text = "";
                      Label_Sub_Total_Amount_Value.Text = totalAmnt.ToString();
                      //TextBox_Sub_Total_Amount_Value.Text = Label_Sub_Total_Amount_Value.Text;
                      Label_Sub_Total_Amount_Value.Visible = true;
                      //if (TextBox_Taxable_Amount.Text.Equals("") || float.Parse(TextBox_Taxable_Amount.Text) == 0)
                     // TextBox_Taxable_Amount.Text = totalAmnt.ToString();
                      Label_Total_Amount_Value.Visible = true;
                      Label_Total_Amount_Value.Text = updateTotalPOAmount();

                      clearProdSelectionPanel();
                  }
              }
          }

          protected void loadInnerGridinProdGrid(GridView GridView1)
          {
              Dictionary<String, ArrayList> rfqProdSrvDict = (Dictionary<String, ArrayList>)Session[SessionFactory.UPDATE_PO_MANUAL_SELECTED_SPEC_MAP_DICT];
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

                  ArrayList rfqSpecList = rfqProdSrvDict.ContainsKey(prodName)?rfqProdSrvDict[prodName]:null;
                  //(ArrayList)Session[SessionFactory.CREATE_INVOICE_MANUAL_SELECTED_SPEC_MAP];
                  int rowCount = 0;

                  if (rfqSpecList != null)
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

          protected void Button_Add_Prod_Srv_Det_Hide_Click(object sender, EventArgs e)
          {
              //Clear any selected product details from the session variable because these are not going to be added to the product grid
              ArrayList rfqProdSrvList = (ArrayList)Session[SessionFactory.UPDATE_PO_MANUAL_SELECTED_SPEC_MAP];
              if (rfqProdSrvList != null && rfqProdSrvList.Count > 0)
                  rfqProdSrvList.Clear();

              Session[SessionFactory.UPDATE_PO_MANUAL_SELECTED_SPEC_MAP] = rfqProdSrvList;

              Label_Selected_List.Text = "";
              Panel_Prod_Service_Det.Visible = false;
          }

          protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
          {
              GridView1.EditIndex = e.NewEditIndex;
              GridView1.DataSource = (DataTable)Session[SessionFactory.UPDATE_PO_MANUAL_PRODUCT_GRID];
              GridView1.DataBind();
              //Button_Create_Inv.Enabled = false;          
              Button_Update_PO_By_Client.Enabled = false;
          }

          protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
          {
              GridViewRow gVR = GridView1.Rows[e.RowIndex];

              DataTable dt = (DataTable)Session[SessionFactory.UPDATE_PO_MANUAL_PRODUCT_GRID];
              String context = Request.QueryString.GetValues("context")[0];

              float unitPrice =float.Parse(((TextBox)gVR.FindControl("TextBox_Unit_Price_Edit")).Text);
              float prevVal = float.Parse(((Label)gVR.FindControl("Label_Amount")).Text);
              float qnty = float.Parse(((TextBox)gVR.FindControl("TextBox_Qnty_Edit")).Text);

              ((Label)gVR.FindControl("Label_Amount")).Text = (qnty * unitPrice).ToString();

              Label_Sub_Total_Amount_Value.Text = (float.Parse(Label_Sub_Total_Amount_Value.Text) +
      float.Parse(((Label)gVR.FindControl("Label_Amount")).Text) - prevVal).ToString();

              dt.Rows[gVR.RowIndex]["Prod_Name"] = context.Equals("vendor") ? ((DropDownList)gVR.FindControl("DropDownList_Edit_ProdName")).SelectedItem.Text :
                  ((TextBox)gVR.FindControl("TextBox_Prod_Name_Edit")).Text;
              dt.Rows[gVR.RowIndex]["Unit_Price"] = unitPrice;
              dt.Rows[gVR.RowIndex]["Cat_Total"] = (qnty * unitPrice).ToString();

              Dictionary<String, ArrayList> rfqProdSrvDict =
   (Dictionary<String, ArrayList>)Session[SessionFactory.UPDATE_PO_MANUAL_SELECTED_SPEC_MAP_DICT];

              String prevProdName = ((Label)gVR.FindControl("Label_Product_Name_Edit")).Text;
              //Remove the previous product name from the unique list
              String prodNameList = Label_Product_Uniq_Name_List.Text;
              if (prodNameList.IndexOf("<start>"+prevProdName + "<end>"+",") >= 0)
                  Label_Product_Uniq_Name_List.Text = prodNameList.Remove(prodNameList.IndexOf("<start>" + prevProdName+ "<end>"), ("<start>" + prevProdName +"<end>" + ",").Length);
              else if (prodNameList.IndexOf("<start>"+prevProdName+"<end>") >= 0)
                  Label_Product_Uniq_Name_List.Text = prodNameList.Remove(prodNameList.IndexOf("<start>" + prevProdName + "<end>"), ("<start>" + prevProdName + "<end>").Length);

              Label_Product_Uniq_Name_List.Text += "," + "<start>" + dt.Rows[gVR.RowIndex]["Prod_Name"].ToString()+ "<end>"; 

              ArrayList tempList = rfqProdSrvDict[prevProdName];
              rfqProdSrvDict.Remove(prevProdName);
              rfqProdSrvDict.Add(dt.Rows[gVR.RowIndex]["Prod_Name"].ToString(), tempList);

              ((Label)gVR.FindControl("Label_Product_Name_Edit")).Text = dt.Rows[gVR.RowIndex]["Prod_Name"].ToString();

              Session[SessionFactory.UPDATE_PO_MANUAL_SELECTED_SPEC_MAP_DICT] = rfqProdSrvDict;
              Session[SessionFactory.UPDATE_PO_MANUAL_PRODUCT_GRID] = dt;


              GridView1.EditIndex = -1;
              GridView1.DataSource = Session[SessionFactory.UPDATE_PO_MANUAL_PRODUCT_GRID];
              GridView1.DataBind();
              //Button_Create_Inv.Enabled = true;
              Button_Update_PO_By_Client.Enabled = true;
              loadInnerGridinProdGrid(GridView1);
              Label_Total_Amount_Value.Text = updateTotalPOAmount();
          }

          protected void TextBox_Qnty_TextChanged_Edit(object sender, EventArgs e)
          {
              try
              {
                  float qnty = float.Parse(((TextBox)sender).Text);
                  if (qnty < 0)
                      throw new FormatException();

                  GridViewRow grdViewProdRow = (GridViewRow)((TextBox)sender).Parent.Parent.Parent.Parent;
                  ((Label)grdViewProdRow.FindControl("Label_Qnty_Format_Edit")).Visible = false;

                  float unitPrice = float.Parse(((TextBox)grdViewProdRow.FindControl("TextBox_Unit_Price_Edit")).Text);

                  float prevVal = float.Parse(((Label)grdViewProdRow.FindControl("Label_Amount")).Text);

                  ((Label)grdViewProdRow.FindControl("Label_Amount")).Text = (qnty * unitPrice).ToString();

                  Label_Sub_Total_Amount_Value.Text = (float.Parse(Label_Sub_Total_Amount_Value.Text) +
                      float.Parse(((Label)grdViewProdRow.FindControl("Label_Amount")).Text) - prevVal).ToString();

                  Label_Total_Amount_Value.Text = updateTotalPOAmount();

                  DataTable dt = (DataTable)Session[SessionFactory.UPDATE_PO_MANUAL_PRODUCT_GRID];

                  dt.Rows[grdViewProdRow.RowIndex]["Qnty"] = qnty;
                  dt.Rows[grdViewProdRow.RowIndex]["Cat_Total"] = (qnty * unitPrice).ToString();

                  Session[SessionFactory.UPDATE_PO_MANUAL_PRODUCT_GRID] = dt;
              }
              catch (FormatException fx)
              {
                  GridViewRow grdViewProdRow = (GridViewRow)((TextBox)sender).Parent.Parent.Parent.Parent;
                  ((Label)grdViewProdRow.FindControl("Label_Qnty_Format_Edit")).Visible = true;
              }
          }

          protected void TextBox_Unit_Price_TextChanged_Edit(object sender, EventArgs e)
          {
              try
              {
                  float unitPrice = float.Parse(((TextBox)sender).Text);
                  if (unitPrice < 0)
                      throw new FormatException();

                  GridViewRow grdViewProdRow = (GridViewRow)((TextBox)sender).Parent.Parent.Parent.Parent;
                  ((Label)grdViewProdRow.FindControl("Label_Unit_Format_Edit")).Visible = false;

                  float qnty = float.Parse(((TextBox)grdViewProdRow.FindControl("TextBox_Qnty_Edit")).Text);

                  float prevVal = float.Parse(((Label)grdViewProdRow.FindControl("Label_Amount")).Text);

                  ((Label)grdViewProdRow.FindControl("Label_Amount")).Text = (qnty * unitPrice).ToString();

                  Label_Sub_Total_Amount_Value.Text = (float.Parse(Label_Sub_Total_Amount_Value.Text) +
          float.Parse(((Label)grdViewProdRow.FindControl("Label_Amount")).Text) - prevVal).ToString();

                  Label_Total_Amount_Value.Text = updateTotalPOAmount();

                  DataTable dt = (DataTable)Session[SessionFactory.UPDATE_PO_MANUAL_PRODUCT_GRID];

                  dt.Rows[grdViewProdRow.RowIndex]["Unit_Price"] = unitPrice;
                  dt.Rows[grdViewProdRow.RowIndex]["Cat_Total"] = (qnty * unitPrice).ToString();

                  Session[SessionFactory.UPDATE_PO_MANUAL_PRODUCT_GRID] = dt;
              }
              catch (FormatException fx)
              {
                  GridViewRow grdViewProdRow = (GridViewRow)((TextBox)sender).Parent.Parent.Parent.Parent;
                  ((Label)grdViewProdRow.FindControl("Label_Unit_Format_Edit")).Visible = true;
              }
          }

          protected void DropDownList_Edit_ProdName_SelectedIndexChanged(object sender, EventArgs e)
          {
              GridViewRow gVR = (GridViewRow)((DropDownList)sender).Parent.Parent.Parent.Parent;

              String selectedProdName = ((DropDownList)gVR.FindControl("DropDownList_Edit_ProdName")).SelectedItem.Text;

              if (Label_Product_Uniq_Name_List.Text.IndexOf("<start>"+selectedProdName+"<end>") > 0)
                  ((Label)gVR.FindControl("Label_Prod_Name_Warning")).Visible = true;
              else
              {
                  ((Label)gVR.FindControl("Label_Prod_Name_Warning")).Visible = false;
                  //Label_Product_Uniq_Name_List.Text += "," + selectedProdName; //Add the new name to the list


                  ((TextBox)gVR.FindControl("TextBox_Unit_Price_Edit")).Text = ((DropDownList)sender).SelectedValue;

                  //float unitPrice = float.Parse(!((DropDownList)sender).SelectedValue.Equals("_")?((DropDownList)sender).SelectedValue:"0");
                  float unitPrice = float.TryParse(((DropDownList)sender).SelectedValue, out unitPrice) ? float.Parse(((DropDownList)sender).SelectedValue) : 0;
                  float prevVal = float.Parse(((Label)gVR.FindControl("Label_Amount")).Text);
                  float qnty = float.Parse(((TextBox)gVR.FindControl("TextBox_Qnty_Edit")).Text);

                  ((Label)gVR.FindControl("Label_Amount")).Text = (qnty * unitPrice).ToString();

                  Label_Sub_Total_Amount_Value.Text = (float.Parse(Label_Sub_Total_Amount_Value.Text) +
          float.Parse(((Label)gVR.FindControl("Label_Amount")).Text) - prevVal).ToString();
                  //TextBox_Sub_Total_Amount_Value.Text = Label_Sub_Total_Amount_Value.Text;

                  //TextBox_Taxable_Amount.Text = Label_Sub_Total_Amount_Value.Text;
                  //Label_Total_Amount_Value.Text = updateTotalPOAmount();


                  /*DataTable dt = (DataTable)Session[SessionFactory.UPDATE_PO_MANUAL_PRODUCT_GRID];

                  dt.Rows[gVR.RowIndex]["Prod_Name"] = ((DropDownList)sender).SelectedItem.Text;
                  dt.Rows[gVR.RowIndex]["Unit_Price"] = unitPrice;
                  dt.Rows[gVR.RowIndex]["Cat_Total"] = (qnty * unitPrice).ToString();

                  Dictionary<String, ArrayList> rfqProdSrvDict =
       (Dictionary<String, ArrayList>)Session[SessionFactory.UPDATE_PO_MANUAL_SELECTED_SPEC_MAP_DICT];

                  String prevProdName = ((Label)gVR.FindControl("Label_Product_Name_Edit")).Text;

                  ArrayList tempList = rfqProdSrvDict[prevProdName];
                  rfqProdSrvDict.Remove(prevProdName);
                  rfqProdSrvDict.Add(((DropDownList)sender).SelectedItem.Text, tempList);

                  ((Label)gVR.FindControl("Label_Product_Name_Edit")).Text = dt.Rows[gVR.RowIndex]["Prod_Name"].ToString();

                  Session[SessionFactory.UPDATE_PO_MANUAL_SELECTED_SPEC_MAP_DICT] = rfqProdSrvDict;
                  Session[SessionFactory.UPDATE_PO_MANUAL_PRODUCT_GRID] = dt;*/
              }
          }

          protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
          {
              GridView1.EditIndex = -1;
              GridView1.DataSource = (DataTable)Session[SessionFactory.UPDATE_PO_MANUAL_PRODUCT_GRID];
              GridView1.DataBind();
              Button_Update_PO_By_Client.Enabled = true;
              //Button_Create_Inv.Enabled = true;
              loadInnerGridinProdGrid(GridView1);
          }

          protected void Button_Update_PO_By_Client_Click(object sender, EventArgs e)
          {
              String rfId = Request.QueryString.GetValues("rfId")[0];              
              String poId = Request.QueryString.GetValues("poId")[0];
              //First delete all quote details of the existing PO
              Dictionary<String, String> whereCls = new Dictionary<string, string>();
              Dictionary<String, String> targetVals = new Dictionary<string, string>();
              whereCls.Add(BackEndObjects.PurchaseOrderQuote.PURCHASE_ORDER_QUOTE_DETAILS_COL_PO_ID, poId);
              BackEndObjects.PurchaseOrderQuote.updatePurchaseOrderQuoteDB(targetVals, whereCls, DBConn.Connections.OPERATION_DELETE);

              ArrayList poQuoteList = new ArrayList();
              foreach (GridViewRow gVR in GridView1.Rows)
              {
                  BackEndObjects.PurchaseOrderQuote poQuoteObj = new PurchaseOrderQuote();
                  poQuoteObj.setPo_id(poId);
                  poQuoteObj.setProd_srv_category(((Label)gVR.Cells[0].FindControl("Label_Hidden")).Text);
                  poQuoteObj.setProduct_name(((Label)gVR.Cells[0].FindControl("Label_Product_Name")).Text);
                  poQuoteObj.setQuote(float.Parse(((TextBox)gVR.Cells[0].FindControl("TextBox_Unit_Price")).Text));
                  poQuoteObj.setUnits(float.Parse(((TextBox)gVR.Cells[0].FindControl("TextBox_Qnty")).Text));

                  poQuoteList.Add(poQuoteObj);
              }

              try
              {
                  BackEndObjects.PurchaseOrderQuote.insertPurchaseOrderQuoteListDB(poQuoteList);
                  targetVals.Clear();
                  targetVals.Add(BackEndObjects.PurchaseOrder.PURCHASE_ORDER_COL_PO_SHIP_VIA, TextBox_Ship_Via.Text);
                  targetVals.Add(BackEndObjects.PurchaseOrder.PURCHASE_ORDER_COL_PO_TNC, TextBox_TnC.Text);
                  targetVals.Add(BackEndObjects.PurchaseOrder.PURCHASE_ORDER_COL_TOTAL_TAX_RATE, TextBox_tax.Text);
                  targetVals.Add(BackEndObjects.PurchaseOrder.PURCHASE_ORDER_COL_AMOUNT, Label_Total_Amount_Value.Text);

                  BackEndObjects.PurchaseOrder.updatePurchaseOrderDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);

                  Label_PO_Creation_Stat.Visible = true;
                  Label_PO_Creation_Stat.ForeColor = System.Drawing.Color.Green;
                  Label_PO_Creation_Stat.Text = "PO updated successfully";

                  bool refreshParent = Request.QueryString.GetValues("refreshParent") != null ? Boolean.Parse(Request.QueryString.GetValues("refreshParent")[0]) : false;
                  if (refreshParent)
                  {
                      //Refresh the parent PO grid in the purchase screen
                      int i = Request.QueryString.GetValues("dataItemIndex") != null ?Int32.Parse( Request.QueryString.GetValues("dataItemIndex")[0]) : -1;

                      if (i >= 0)
                      {
                          String context = Request.QueryString.GetValues("context")[0];
                          DataTable dt =null;

                          if (context.Equals("vendor"))
                          {
                              dt = (DataTable)Session[SessionFactory.ALL_SALE_ALL_SO_GRID_DATA];
                              dt.Rows[i]["Amount"] = Label_Total_Amount_Value.Text;
                              dt.DefaultView.Sort = "SO_Date_Ticks" + " " + "DESC";
                              Session[SessionFactory.ALL_SALE_ALL_SO_GRID_DATA] = dt;
                          }
                          else
                          {
                              dt = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_PO_GRID_DATA];
                              dt.Rows[i]["Amount"] = Label_Total_Amount_Value.Text;
                              dt.DefaultView.Sort = "PO_Date_Ticks" + " " + "DESC";
                              Session[SessionFactory.ALL_PURCHASE_ALL_PO_GRID_DATA] = dt;
                          }
                          

                          ScriptManager.RegisterStartupScript(this, typeof(string), "RefreshPOGrid", "RefreshParent();", true);
                      }
                  }
              }
              catch (Exception ex)
              {
                  Label_PO_Creation_Stat.Visible = true;
                  Label_PO_Creation_Stat.ForeColor = System.Drawing.Color.Red;
                  Label_PO_Creation_Stat.Text = "PO update failed";
              }
          }

          protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
          {
              DataTable dt = (DataTable)Session[SessionFactory.UPDATE_PO_MANUAL_PRODUCT_GRID];

              float totalAmnt = 0;

              String totalForItem = dt.Rows[e.RowIndex]["Cat_Total"].ToString();
              String catId = dt.Rows[e.RowIndex]["Hidden_Cat_Id"].ToString();
              String prevTotal = Label_Sub_Total_Amount_Value.Text;

              totalAmnt = float.Parse(prevTotal) - float.Parse(totalForItem);

              Dictionary<String, ArrayList> rfqSpecDict = (Dictionary<String, ArrayList>)Session[SessionFactory.UPDATE_PO_MANUAL_SELECTED_SPEC_MAP_DICT];
              rfqSpecDict.Remove(dt.Rows[e.RowIndex]["Prod_Name"].ToString());
              Session[SessionFactory.UPDATE_PO_MANUAL_SELECTED_SPEC_MAP_DICT] = rfqSpecDict;

              //Remove the product name from the temporary list as well
              String oldProdName = dt.Rows[e.RowIndex]["Prod_Name"].ToString();
              String prodNameList = Label_Product_Uniq_Name_List.Text;
              if (prodNameList.IndexOf("<start>"+oldProdName + "<end>"+",") >= 0)
                  Label_Product_Uniq_Name_List.Text = prodNameList.Remove(prodNameList.IndexOf("<start>" + oldProdName + "<end>"), ("<start>" + oldProdName + "<end>" + ",").Length);
              else if (prodNameList.IndexOf(oldProdName) >= 0)
                  Label_Product_Uniq_Name_List.Text = prodNameList.Remove(prodNameList.IndexOf("<start>" + oldProdName + "<end>"), ("<start>" + oldProdName + "<end>").Length);

              dt.Rows[e.RowIndex].Delete();
              GridView1.DataSource = dt;
              GridView1.DataBind();

              loadInnerGridinProdGrid(GridView1);

              Label_Sub_Total_Amount_Value.Visible = true;
              Label_Sub_Total_Amount_Value.Text = totalAmnt.ToString();
              //TextBox_Sub_Total_Amount_Value.Text = Label_Sub_Total_Amount_Value.Text;
              //if (TextBox_Taxable_Amount.Text.Equals("") || float.Parse(TextBox_Taxable_Amount.Text) == 0)
              //TextBox_Taxable_Amount.Text = totalAmnt.ToString();
              Label_Total_Amount_Value.Visible = true;
              Label_Total_Amount_Value.Text = updateTotalPOAmount();

              Session[SessionFactory.UPDATE_PO_MANUAL_PRODUCT_GRID] = dt;
          }

          protected void TextBox_Prod_Name_Edit_TextChanged(object sender, EventArgs e)
          {
              GridViewRow grdViewProdRow = (GridViewRow)((TextBox)sender).Parent.Parent.Parent.Parent;
              String prodName = ((TextBox)grdViewProdRow.FindControl("TextBox_Prod_Name_Edit")).Text.Trim();

              if (Label_Product_Uniq_Name_List.Text.IndexOf("<start>"+prodName+"<end>") > 0)
                  ((Label)grdViewProdRow.FindControl("Label_Prod_Name_Warning")).Visible = true;
              else
              {
                  ((Label)grdViewProdRow.FindControl("Label_Prod_Name_Warning")).Visible = false;
                  Label_Product_Uniq_Name_List.Text += "," + "<start>"+prodName+"<end>"; //Add the new name to the list


              }
          }


    }
}