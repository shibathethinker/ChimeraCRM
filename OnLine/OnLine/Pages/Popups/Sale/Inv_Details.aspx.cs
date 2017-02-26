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

namespace OnLine.Pages.Popups.Sale
{
    public partial class Inv_Details : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                String rfId = Request.QueryString.GetValues("rfId")[0];
                String context = Request.QueryString.GetValues("context")[0];
                String poId = Request.QueryString.GetValues("poId")[0];
                String otherEntId = "";
                //This flag determines whether or no any tax component is changed in this screen
                Label_Inv_Tax_Comp_Changed.Text = "N";

                String invId = Request.QueryString.GetValues("invId")[0];
                BackEndObjects.Invoice invObj=new BackEndObjects.Invoice();

                if (invId != null && !invId.Equals(""))
                {
                    invObj = BackEndObjects.Invoice.getInvoicebyIdDB(invId);
                    loadAlreadyCreatedInvData(invObj);
                }
                else
                {
                    //If invoice data does not exist then forward to create invoice screen
                    String forwardString = "/Pages/createInvoice.aspx";
                    forwardString += "?rfId=" + rfId;
                    if (poId == null || poId.Equals(""))
                        forwardString += "&emptySO=" + "true";

                    Response.Redirect(forwardString);
                    //Server.Transfer(forwardString);
                    //ScriptManager.RegisterStartupScript(this, typeof(string), "createInv", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=900');", true);
                }
                 //   Button_Update_Inv.Visible = false;


                RFQShortlisted rfqDealObj = BackEndObjects.RFQShortlisted.getRFQShortlistedEntryforFinalizedVendor(rfId);

                if (context.Equals("client") || context.Equals("clientInvoiceGrid"))
                {
                    otherEntId = rfqDealObj.getRespEntityId();
                    //If control is in client context, then the invoice is already created and the response entity details are available
                    if (otherEntId == null || otherEntId.Equals(""))
                        otherEntId = invObj.getRespEntityId();
                }
                else
                {
                               bool approvalContext = false;
            if (Request.QueryString.GetValues("approvalContext") != null)
                approvalContext = Request.QueryString.GetValues("approvalContext")[0].Equals("Y") ? true : false;

                        if (approvalContext)
                        {
                            Button_Create_Inv.Enabled = false;
                            Button_Update_Inv.Enabled = false;
                            Label_INV_Creation_Stat.Visible = true;
                            Label_INV_Creation_Stat.Text = "To edit anything for this invoice, please check the invoice from Sales->Invoice screen.";
                            Label_INV_Creation_Stat.ForeColor = System.Drawing.Color.Red;

                        }
                    else
                    {
                Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];
                if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_INVOICE_SALES] &&
!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                {
                    Label_INV_Creation_Stat.Visible = true;
                    Label_INV_Creation_Stat.Text = "You don't have edit access to invoice record";
                    Label_INV_Creation_Stat.ForeColor = System.Drawing.Color.Red;
                    Button_Create_Inv.Enabled = false;
                    Button_Update_Inv.Enabled = false;
                }
                     }
                    otherEntId = RFQDetails.getRFQDetailsbyIdDB(rfId).getEntityId();
                }

                loadClientAndVendorDetails(rfId, otherEntId);                
                loadProductGrid(poId);
                populateLogo(otherEntId,context);
                loadInvTaxCompGrid(invId);
                loadTnC(invObj);
                updateTotalINVAmount();

                if (context.Equals("client") || context.Equals("clientInvoiceGrid"))
                    disableControlsForClient();

            }
                   
        }

        protected void disableControlsForClient()
        {
            TextBox_Inv_Date.Enabled = false;
            TextBox_Policy_No.Enabled = false;
            TextBox_Ship_Via.Enabled = false;
            TextBox_Taxable_Amount.Enabled = false;
            TextBox_TnC.Enabled = false;
            Button_Update_Inv.Visible = false;
            Button_Show_All_Tax_Comp_List.Visible = false;
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

        protected void loadClientAndVendorDetails(String rfId, String otherEntId)
        {
            String entId = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();
            String context = Request.QueryString.GetValues("context")[0];
            context = context.Equals("client") || context.Equals("clientInvoiceGrid") ? "client" : context;

            String localId = "";
            String vendDetails = "";
            String custDetails = "";
            String vendContact = "";
            String custContact = "";

            //Populate the vendor details
            Dictionary<String, Object> vendObj = ActionLibrary.customerDetails.
                getContactDetails(otherEntId, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

            if (vendObj.ContainsKey(ActionLibrary.customerDetails.RETURN_OBJECT_TYPE_CONTACT_DETAILS))
            {
                BackEndObjects.Contacts cOBJ = (BackEndObjects.Contacts)vendObj[ActionLibrary.customerDetails.RETURN_OBJECT_TYPE_CONTACT_DETAILS];

                if (context.Equals("client"))
                {
                    Label_Vendor_Name.Text = cOBJ.getContactName();
                    vendContact = cOBJ.getMobNo();
                    vendDetails += cOBJ.getStreetName() + ",";
                    localId = cOBJ.getLocalityId();
                }
                else
                {
                    Label_Client_Name.Text = cOBJ.getContactName();
                    custContact = cOBJ.getMobNo();
                    custDetails += cOBJ.getStreetName() + ",";
                    localId = cOBJ.getLocalityId();
                }
            }
            else
            {
                BackEndObjects.MainBusinessEntity mBObj = (BackEndObjects.MainBusinessEntity)vendObj[ActionLibrary.customerDetails.RETURN_OBJECT_TYPE_MAIN_BUSINESS_ENTITY];
                BackEndObjects.AddressDetails addrObjEnt = BackEndObjects.AddressDetails.getAddressforMainBusinessEntitybyIdDB(mBObj.getEntityId());
                if (context.Equals("client"))
                {
                    Label_Vendor_Name.Text = mBObj.getEntityName();
                    vendContact = mBObj.getPhNo();
                    if (addrObjEnt != null)
                        vendDetails += addrObjEnt.getAddrLine1() + ",";
                }
                else
                {
                    Label_Client_Name.Text = mBObj.getEntityName();
                    custContact = mBObj.getPhNo();
                    if (addrObjEnt != null)
                        custDetails += addrObjEnt.getAddrLine1() + ",";
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

                if (context.Equals("client"))
                    vendDetails += lclObj.getLocalityName() + "<br/>" + ctObj.getCityName() + "<br/>" + stObj.getStateName() + "<br/>" + cntObj.getCountryName() + "<br/> Phone:" + vendContact;
                else
                    custDetails += lclObj.getLocalityName() + "<br/>" + ctObj.getCityName() + "<br/>" + stObj.getStateName() + "<br/>" + cntObj.getCountryName() + "<br/> Phone:" + custContact;
            }
            if (context.Equals("client"))
                Label_Vendor_Addr.Text = vendDetails;
            else
                Label_Client_Addr.Text = custDetails;

            //Get the customer details
            BackEndObjects.MainBusinessEntity custObj = BackEndObjects.MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(entId);

            if (context.Equals("client"))
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
                if (context.Equals("client"))
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

                    if (context.Equals("client"))
                        custDetails += lclObj.getLocalityName() + "<br/>" + ctObj.getCityName() + "<br/>" + stObj.getStateName() + "<br/>" + cntObj.getCountryName() + "<br/> Phone:" + custContact;
                    else
                        vendDetails += lclObj.getLocalityName() + "<br/>" + ctObj.getCityName() + "<br/>" + stObj.getStateName() + "<br/>" + cntObj.getCountryName() + "<br/> Phone:" + vendContact;
                }
            }
            if (context.Equals("client"))
                Label_Client_Addr.Text = custDetails;
            else
                Label_Vendor_Addr.Text = vendDetails;
        }

        protected void loadProductGrid(String poId)
        {
            ArrayList rfqSpecList = new ArrayList();
            String context = Request.QueryString.GetValues("context")[0];

            if (context.Equals("client"))
            {
                rfqSpecList = (ArrayList)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_RFQ_SPEC];                
            }
            else if(context.Equals("vendor"))
                rfqSpecList = (ArrayList)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_POTN_SPEC];
            else if (context.Equals("clientInvoiceGrid") || context.Equals("vendInvoiceGrid"))
            {//If this is sent from the Invoice grid in Purchase screen
                rfqSpecList=BackEndObjects.RFQProductServiceDetails.getAllProductServiceDetailsbyRFQIdDB(Request.QueryString.GetValues("rfId")[0]);
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
            if(TextBox_Taxable_Amount.Text.Equals("") ||float.Parse(TextBox_Taxable_Amount.Text)==0)
            TextBox_Taxable_Amount.Text = totalAmnt.ToString();
            Label_Total_Amount_Value.Visible = true;
            Label_Total_Amount_Value.Text = updateTotalINVAmount();

        }

        protected void populateLogo(String otherEntId,String context)
        {

                if (context.Equals("client") || context.Equals("clientInvoiceGrid"))
                {
                    ArrayList imgListObjs = BackEndObjects.Image.
    getImagebyTypeandEntId(BackEndObjects.Image.PICTURE_IMG_TYPE_LOGO, otherEntId);
                    if (imgListObjs.Count > 0)
                    {
                        //Only consider the first image object for logo
                        BackEndObjects.Image imgObj = (BackEndObjects.Image)imgListObjs[0];
                        Image_Logo_Vendor.ImageUrl = imageToURL(imgObj.getImgPath());
                        Image_Logo_Vendor.Visible = true;
                    }
                    imgListObjs.Clear();
                    imgListObjs = BackEndObjects.Image.
getImagebyTypeandEntId(BackEndObjects.Image.PICTURE_IMG_TYPE_LOGO, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                    if (imgListObjs.Count > 0)
                    {
                        //Only consider the first image object for logo
                        BackEndObjects.Image imgObj = (BackEndObjects.Image)imgListObjs[0];
                        Image_Logo_Client.ImageUrl = imageToURL(imgObj.getImgPath());
                        Image_Logo_Client.Visible = true;
                    }
                }
                else
                {
                    ArrayList imgListObjs = BackEndObjects.Image.
getImagebyTypeandEntId(BackEndObjects.Image.PICTURE_IMG_TYPE_LOGO, otherEntId);
                    if (imgListObjs.Count > 0)
                    {
                        //Only consider the first image object for logo
                        BackEndObjects.Image imgObj = (BackEndObjects.Image)imgListObjs[0];
                        Image_Logo_Client.ImageUrl = imageToURL(imgObj.getImgPath());
                        Image_Logo_Client.Visible = true;
                    }
                    imgListObjs.Clear();
                    imgListObjs = BackEndObjects.Image.
getImagebyTypeandEntId(BackEndObjects.Image.PICTURE_IMG_TYPE_LOGO, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                    if (imgListObjs.Count > 0)
                    {
                        //Only consider the first image object for logo
                        BackEndObjects.Image imgObj = (BackEndObjects.Image)imgListObjs[0];
                        Image_Logo_Vendor.ImageUrl = imageToURL(imgObj.getImgPath());
                        Image_Logo_Vendor.Visible = true;
                    }
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

        protected String updateTotalINVAmount()
        {
            //float tax = TextBox_tax.Text.Equals("") ? 0 : float.Parse(TextBox_tax.Text);
            //return ((float.Parse(Label_Sub_Total_Amount_Value.Text) * tax) / 100 + float.Parse(Label_Sub_Total_Amount_Value.Text)).ToString();
            float totalAmount = float.Parse(Label_Sub_Total_Amount_Value.Text);

            if (Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_CREATE_INV_TOTAL_TAX_COMP_PERC] != null)
            {
                float totalTaxPerc = (float)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_CREATE_INV_TOTAL_TAX_COMP_PERC];
                float totalTaxableAmount = (float.TryParse(TextBox_Taxable_Amount.Text, out totalTaxableAmount) ? float.Parse(TextBox_Taxable_Amount.Text) : 0);
                TextBox_Taxable_Amount.Text = totalTaxableAmount.ToString();

                totalAmount = (totalTaxPerc * totalTaxableAmount) / 100 + totalAmount;

                Label_Total_Amount_Value.Text = totalAmount.ToString();

            }
            return totalAmount.ToString();
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
            String context = Request.QueryString.GetValues("context")[0];

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
                ArrayList invTaxCompList=BackEndObjects.InvoiceComponents.getInvoiceComponentByInvIdandSecType(invId, BackEndObjects.InvoiceComponents.INVOICE_SECTION_TYPE_TAX);

                for (int i = 0; i < invTaxCompList.Count; i++)
                {
                    dt.Rows.Add();

                    BackEndObjects.InvoiceComponents invCompObj=(BackEndObjects.InvoiceComponents) invTaxCompList[i];

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
            

            
            Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_CREATE_INV_TOTAL_TAX_COMP_GRID] = dtComplete;

            if (dt.Rows.Count > 0)
            {
                GridView_Inv_Tax_Comp.DataSource = dt;
                GridView_Inv_Tax_Comp.DataBind();
                GridView_Inv_Tax_Comp.Visible = true;
                GridView_Inv_Tax_Comp.Columns[1].Visible = false;

                //Hide the delete link if the context is client
                if (context.Equals("client"))
                {
                    GridView_Inv_Tax_Comp.Columns[0].Visible = false;

                    foreach (GridViewRow gVR in GridView_Inv_Tax_Comp.Rows)
                        ((TextBox)gVR.Cells[0].FindControl("TextBox_Value")).Enabled = false;
                }

                Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_CREATE_INV_TAX_COMP_DATA_GRID] = dt;
                Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_CREATE_INV_TOTAL_TAX_COMP_PERC] = totalTaxPerc;
            }
            else
            {
                Label_No_Tax_Comp_Warning.Visible = true;
                Label_No_Tax_Comp_Warning.Text = "Tax components for invoice not defined for your organization - set it up from Administration screen";
            }
        }
        /// <summary>
        /// If the invoice is already created then load details like date,inv no,ship via,policy no etc.
        /// </summary>
        protected void loadAlreadyCreatedInvData(Invoice invObj)
        {
            Dictionary<String, Currency> allCurrList = (Dictionary<String, Currency>)Session[SessionFactory.CURRENCY_LIST];
            DateUtility dU = new DateUtility();

            Label_INV_No.Text = invObj.getInvoiceNo();
            TextBox_Inv_Date.Text = dU.getConvertedDateWoTime(invObj.getInvoiceDate());
            TextBox_Policy_No.Text = invObj.getPolicyNo();
            TextBox_Ship_Via.Text = invObj.getShipVia();
            TextBox_Taxable_Amount.Text = invObj.getTaxableAmnt().ToString();
            Label_Curr.Text = allCurrList.ContainsKey(invObj.getCurrency()) ?
                                        " "+allCurrList[invObj.getCurrency()].getCurrencyName()+" " : "";
 
            Button_Update_Inv.Visible = true;
            Button_Create_Inv.Visible = false;
        }

        protected void GridView_Inv_Tax_Comp_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataTable dt = (DataTable)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_CREATE_INV_TAX_COMP_DATA_GRID];
            //GridViewRow gVR = GridView_Inv_Tax_Comp.Rows[e.RowIndex];

            float taxDeductPerc = float.Parse(((TextBox)GridView_Inv_Tax_Comp.Rows[e.RowIndex].Cells[0].FindControl("TextBox_Value")).Text);
            float totalTaxPerc = (float)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_CREATE_INV_TOTAL_TAX_COMP_PERC];

            Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_CREATE_INV_TOTAL_TAX_COMP_PERC] = totalTaxPerc - taxDeductPerc;
            updateTotalINVAmount();

            dt.Rows[e.RowIndex].Delete();

            Label_Inv_Tax_Comp_Changed.Text = "Y";

           /* Dictionary<String, String> whereCls = new Dictionary<string, string>();
            whereCls.Add(BackEndObjects.DocFormat.DOCFORMAT_COL_CMP_ID, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            whereCls.Add(BackEndObjects.DocFormat.DOCFORMAT_COL_DOC_TYPE, BackEndObjects.DocFormat.DOCFORMAT_DOC_TYPE_INVOICE);
            whereCls.Add(BackEndObjects.DocFormat.DOCFORMAT_COL_SECTION_TYPE, BackEndObjects.DocFormat.DOCFORMAT_DOC_TYPE_INVOICE_SECTION_TYPE_TAX);
            whereCls.Add(BackEndObjects.DocFormat.DOCFORMAT_COL_SECTION_TYPE_NAME, ((Label)gVR.Cells[0].FindControl("Label_Name")).Text);

            Dictionary<String, String> targetVals = new Dictionary<string, string>();


            BackEndObjects.DocFormat.updateDocFormatObjsDB(whereCls, targetVals, DBConn.Connections.OPERATION_DELETE);*/

            GridView_Inv_Tax_Comp.DataSource = dt;
            GridView_Inv_Tax_Comp.DataBind();

            Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_CREATE_INV_TAX_COMP_DATA_GRID] = dt;
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void TextBox_Unit_Price_TextChanged(object sender, EventArgs e)
        {

        }

        protected void Button_Create_Inv_Click(object sender, EventArgs e)
        {
            BackEndObjects.Invoice invObj= new BackEndObjects.Invoice();
           
            invObj.setInvoiceId(new Id().getNewId(BackEndObjects.Id.ID_TYPE_INV_ID_STRING));
            invObj.setRespEntityId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            invObj.setInvComments(TextBox_TnC.Text);
            invObj.setInvoiceDate(TextBox_Inv_Date.Text);
            invObj.setDeliveryStatus(BackEndObjects.DeliveryStat.DELIV_STAT_UNDELIVERED);
            invObj.setInvoiceType(BackEndObjects.InvoiceType.INVOICE_TYPE_TAX_INVOICE);
            invObj.setPaymentStatus(BackEndObjects.PaymentStat.PAYMENT_STAT_INCOMPLETE);
            invObj.setPolicyNo(TextBox_Policy_No.Text);
            invObj.setRFQId(Request.QueryString.GetValues("rfId")[0]);
            invObj.setShipVia(TextBox_Ship_Via.Text);
            invObj.setTaxableAmnt(float.Parse(TextBox_Taxable_Amount.Text));
            invObj.setTotalAmount(float.Parse(Label_Total_Amount_Value.Text));
            //For invoice in auto-created mode, the invoice no and invoice id will be same
            invObj.setInvoiceNo(invObj.getInvoiceId());
            invObj.setCreationMode(BackEndObjects.Invoice.INVOICE_CREATION_MODE_AUTO);

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

            try
            {
                BackEndObjects.Invoice.insertInvoiceDB(invObj);
                BackEndObjects.InvoiceComponents.insertInvoiceComponentListDB(invTaxComp);
                Label_INV_Creation_Stat.Visible = true;
                Label_INV_Creation_Stat.ForeColor = System.Drawing.Color.Green;
                Label_INV_Creation_Stat.Text = "Invoice Created Successfully";
                Label_INV_No.Text = invObj.getInvoiceId();
                Button_Create_Inv.Enabled = false;
            }
            catch (Exception ex)
            {
                Label_INV_Creation_Stat.Visible = true;
                Label_INV_Creation_Stat.ForeColor = System.Drawing.Color.Red;
                Label_INV_Creation_Stat.Text = "Invoice Creation Failed";
            }
        }

        //This event is fired if any text in any tax component value is changed
        protected void TextBox_Value_TextChanged(object sender, EventArgs e)
        {
            float totalTaxPerc = 0, tempPerc = 0; 
            Label_Inv_Tax_Comp_Changed.Text = "Y";

            foreach(GridViewRow gVR in GridView_Inv_Tax_Comp.Rows)
            {
                tempPerc = float.TryParse(((TextBox)gVR.Cells[0].FindControl("TextBox_Value")).Text, out tempPerc) ?
     float.Parse(((TextBox)gVR.Cells[0].FindControl("TextBox_Value")).Text) : 0;
                ((TextBox)gVR.Cells[0].FindControl("TextBox_Value")).Text = tempPerc.ToString();
                totalTaxPerc += tempPerc;
                
            }

            Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_CREATE_INV_TOTAL_TAX_COMP_PERC] = totalTaxPerc;
            updateTotalINVAmount();
        }

        protected void Button_Update_Inv_Click(object sender, EventArgs e)
        {
            String invId = Request.QueryString.GetValues("invId")[0];

            Dictionary<String, String> whereCls = new Dictionary<String, String>();
            whereCls.Add(Invoice.INVOICE_COL_INVOICE_ID, invId);

            DateUtility dU = new DateUtility();
            
            Dictionary<String, String> targetVals = new Dictionary<String, String>();
            targetVals.Add(BackEndObjects.Invoice.INVOICE_COL_COMMENTS, TextBox_TnC.Text);
            targetVals.Add(BackEndObjects.Invoice.INVOICE_COL_INVOICE_DATE,dU.getDeConvertedDate(TextBox_Inv_Date.Text));
            targetVals.Add(BackEndObjects.Invoice.INVOICE_COL_POLICY_NO,TextBox_Policy_No.Text);
            targetVals.Add(BackEndObjects.Invoice.INVOICE_COL_SHIP_VIA,TextBox_Ship_Via.Text);
            targetVals.Add(BackEndObjects.Invoice.INVOICE_COL_TAXABLE_AMNT, TextBox_Taxable_Amount.Text);
            targetVals.Add(BackEndObjects.Invoice.INVOICE_COL_TOTAL_AMOUNT, Label_Total_Amount_Value.Text);

            try
            {
                BackEndObjects.Invoice.updateInvoiceDetailsDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);

                if (Label_Inv_Tax_Comp_Changed.Text.Equals("Y"))
                {
                    Label_Inv_Tax_Comp_Changed.Text = "N";
                    //Clear all invoice components 
                    whereCls.Clear();
                    targetVals.Clear();
                    whereCls.Add(BackEndObjects.InvoiceComponents.INVOICE_COMPONENTS_COL_INVOICE_ID, invId);
                    whereCls.Add(BackEndObjects.InvoiceComponents.INVOICE_COMPONENTS_COL_SECTION_TYPE, BackEndObjects.InvoiceComponents.INVOICE_SECTION_TYPE_TAX);
                    BackEndObjects.InvoiceComponents.updateInvoiceComponentDetailsDB(targetVals, whereCls, DBConn.Connections.OPERATION_DELETE);

                    ArrayList invCompList = new ArrayList();

                    foreach (GridViewRow gVR in GridView_Inv_Tax_Comp.Rows)
                    {
                        BackEndObjects.InvoiceComponents invCompObj = new InvoiceComponents();
                        invCompObj.setInvoice_Id(invId);
                        invCompObj.setSection_type(BackEndObjects.InvoiceComponents.INVOICE_SECTION_TYPE_TAX);
                        invCompObj.setSection_type_name(((Label)gVR.Cells[0].FindControl("Label_Name")).Text);
                        invCompObj.setSection_value(((TextBox)gVR.Cells[0].FindControl("TextBox_Value")).Text);

                        invCompList.Add(invCompObj);
                        /*whereCls.Clear();
                        targetVals.Clear();

                        whereCls.Add(BackEndObjects.InvoiceComponents.INVOICE_COMPONENTS_COL_INVOICE_ID, invId);
                        whereCls.Add(BackEndObjects.InvoiceComponents.INVOICE_COMPONENTS_COL_SECTION_TYPE_NAME,
                            ((Label)gVR.Cells[0].FindControl("Label_Name")).Text);

                        targetVals.Add(BackEndObjects.InvoiceComponents.INVOICE_COMPONENTS_COL_SECTION_VALUE,
                            ((TextBox)gVR.Cells[0].FindControl("TextBox_Value")).Text);

                        BackEndObjects.InvoiceComponents.updateInvoiceComponentDetailsDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);*/
                    }

                    BackEndObjects.InvoiceComponents.insertInvoiceComponentListDB(invCompList);

                    loadInvTaxCompGrid(Request.QueryString.GetValues("invId")[0]);
                }
                
                Label_INV_Creation_Stat.Visible = true;
                Label_INV_Creation_Stat.ForeColor = System.Drawing.Color.Green;
                Label_INV_Creation_Stat.Text = "Invoice Updated Successfully";
            }
            catch (Exception ex)
            {
                Label_INV_Creation_Stat.Visible = true;
                Label_INV_Creation_Stat.ForeColor = System.Drawing.Color.Red;
                Label_INV_Creation_Stat.Text = "Invoice Update Failed";
            }

        }

        protected void TextBox_Taxable_Amount_TextChanged(object sender, EventArgs e)
        {
            updateTotalINVAmount();
        }

        protected void GridView_Inv_Tax_Complete_List_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_Inv_Tax_Complete_List.PageIndex = e.NewPageIndex;
            GridView_Inv_Tax_Complete_List.DataSource = (DataTable)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_CREATE_INV_TOTAL_TAX_COMP_GRID];
            GridView_Inv_Tax_Complete_List.DataBind();
        }

        protected void Button_Show_All_Tax_Comp_List_Click(object sender, EventArgs e)
        {
            Panel_Select_Tax_Comp.Visible = true;
            GridView_Inv_Tax_Complete_List.Visible = true;
            Label_Tax_Comp_Addn_Stat.Visible = false;
        }

        protected void Button_Hide_Complete_Tax_Comp_List_Click(object sender, EventArgs e)
        {
            Panel_Select_Tax_Comp.Visible = false;
            GridView_Inv_Tax_Complete_List.Visible = false;
        }

        protected void GridView_Inv_Tax_Complete_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_CREATE_INV_TAX_COMP_DATA_GRID];
            GridViewRow gVR=GridView_Inv_Tax_Complete_List.Rows[GridView_Inv_Tax_Complete_List.SelectedIndex];
            Boolean alreadyExists=false;

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

                dt.Rows.Add();

                dt.Rows[dt.Rows.Count-1]["Hidden"] = ((Label)gVR.Cells[0].FindControl("Label_Hidden")).Text;
                dt.Rows[dt.Rows.Count-1]["Comp_Name"] = ((Label)gVR.Cells[0].FindControl("Label_Name")).Text;
                dt.Rows[dt.Rows.Count-1]["Comp_Value"] = ((Label)gVR.Cells[0].FindControl("Label_Value")).Text;

                GridView_Inv_Tax_Comp.DataSource = dt;
                GridView_Inv_Tax_Comp.DataBind();

                Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_CREATE_INV_TAX_COMP_DATA_GRID] = dt;

                float totalTaxPerc =(float) Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_CREATE_INV_TOTAL_TAX_COMP_PERC];
                totalTaxPerc +=float.Parse( ((Label)gVR.Cells[0].FindControl("Label_Value")).Text);
                Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_CREATE_INV_TOTAL_TAX_COMP_PERC] = totalTaxPerc;
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

        public override void VerifyRenderingInServerForm(Control control)
        {
            //base.VerifyRenderingInServerForm(control);
        }

        public override bool  EnableEventValidation
        {
            get { return false; }
            set { }
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

        protected void Button_Render_Click(object sender, EventArgs e)
        {
            clearAllFields("form1");
            Label_INV_Creation_Stat.Visible = false;

            TextBox_Taxable_Amount.ReadOnly = true;
            TextBox_Taxable_Amount.BorderStyle = BorderStyle.None;
            TextBox_Taxable_Amount.BackColor = System.Drawing.Color.Transparent;

            GridView_Inv_Tax_Comp.Columns[0].Visible = false;
            //TextBox_Value
            foreach (GridViewRow gVR in GridView1.Rows)
            {
                ((TextBox)gVR.Cells[2].FindControl("TextBox_Qnty")).ReadOnly = true;
                ((TextBox)gVR.Cells[2].FindControl("TextBox_Qnty")).BackColor = System.Drawing.Color.Transparent;
                ((TextBox)gVR.Cells[2].FindControl("TextBox_Qnty")).BorderStyle = BorderStyle.None;

                ((TextBox)gVR.Cells[2].FindControl("TextBox_Unit_Price")).ReadOnly = true;
                ((TextBox)gVR.Cells[2].FindControl("TextBox_Unit_Price")).BackColor = System.Drawing.Color.Transparent;
                ((TextBox)gVR.Cells[2].FindControl("TextBox_Unit_Price")).BorderStyle = BorderStyle.None;
            }
            foreach (GridViewRow gVR in GridView_Inv_Tax_Comp.Rows)
            {
                ((TextBox)gVR.Cells[3].FindControl("TextBox_Value")).ReadOnly = true;
                ((TextBox)gVR.Cells[3].FindControl("TextBox_Value")).BackColor = System.Drawing.Color.Transparent;
                ((TextBox)gVR.Cells[3].FindControl("TextBox_Value")).BorderStyle = BorderStyle.None;
            }

            Button_Render.Visible = false;
            Button_Update_Inv.Visible = false;
            Button_Show_All_Tax_Comp_List.Visible = false;

            ASPXToPDF1.RenderAsPDF("newFile.pdf");
           /* Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition",
              "attachment;filename=GridViewExport.doc");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-word ";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            TextBox_Inv_Date.DataBind();
            TextBox_Inv_Date.RenderControl(hw);
            Label_Client_Name.DataBind();
            Label_Client_Name.RenderControl(hw);
            /*GridView1.AllowPaging = false;
            GridView1.DataSource=
            GridView1.DataBind();
            GridView1.RenderControl(hw);*/
            /*Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();*/
        }


    }
}