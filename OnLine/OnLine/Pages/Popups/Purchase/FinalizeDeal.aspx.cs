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

namespace OnLine.Pages.Popups.Purchase
{
    public partial class FinalizeDeal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                loadClientAndVendorDetails();
                loadTnC();
                loadProductGrid();
                populateLogo();
                loadCurrency();
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

        protected void loadClientAndVendorDetails()
        {
            String context = Request.QueryString.GetValues("context")[0];
            String ClientEntId="";
            String vendorId = "";
            Label_To.Text = "Vendor Details:";

            if (context.Equals("client"))
            {
                ClientEntId = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();
                vendorId = Request.QueryString.GetValues("respCompId")[0];                
            }
            else
            {
                Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

                if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_POTENTIAL] &&
    !accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                    Button_Create_PO.Enabled = false;

                ClientEntId = Request.QueryString.GetValues("EntId")[0];
                vendorId = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();
                Label1.Text = "Sales Order";
            }

            String localId = "";
            String vendDetails = "";
            String custDetails = "";
            String vendContact = "";
            String custContact = "";

            //Populate the vendor details
            Dictionary<String, Object> vendObj = ActionLibrary.customerDetails.getContactDetails(vendorId, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

            if (vendObj.ContainsKey(ActionLibrary.customerDetails.RETURN_OBJECT_TYPE_CONTACT_DETAILS))
            {
                BackEndObjects.Contacts cOBJ = (BackEndObjects.Contacts)vendObj[ActionLibrary.customerDetails.RETURN_OBJECT_TYPE_CONTACT_DETAILS];
                
                Label_Vendor_Name.Text = cOBJ.getContactName();                                
                vendContact = cOBJ.getMobNo();                
                localId = cOBJ.getLocalityId();
            }
            else
            {
                BackEndObjects.MainBusinessEntity mBObj = (BackEndObjects.MainBusinessEntity)vendObj[ActionLibrary.customerDetails.RETURN_OBJECT_TYPE_MAIN_BUSINESS_ENTITY];
                Label_Vendor_Name.Text = mBObj.getEntityName();                
                vendContact = mBObj.getPhNo();
                
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

                vendDetails += lclObj.getLocalityName() + "<br/>" + ctObj.getCityName() + "<br/>" + stObj.getStateName() + "<br/>" + cntObj.getCountryName() + "<br/> Phone:" + vendContact;
            }
            Label_Vendor_Addr.Text = vendDetails;

            //Get the customer details
            //BackEndObjects.MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(ClientEntId);
            Dictionary<String, Object> custObj = ActionLibrary.customerDetails.getContactDetails(ClientEntId, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            if (custObj.ContainsKey(ActionLibrary.customerDetails.RETURN_OBJECT_TYPE_CONTACT_DETAILS))
            {
                BackEndObjects.Contacts cOBJ = (BackEndObjects.Contacts)custObj[ActionLibrary.customerDetails.RETURN_OBJECT_TYPE_CONTACT_DETAILS];

                Label_Client_Name.Text = cOBJ.getContactName();
                custContact = cOBJ.getMobNo();
                localId = cOBJ.getLocalityId();
            }
            else
            {
                BackEndObjects.MainBusinessEntity mBObj = (BackEndObjects.MainBusinessEntity)custObj[ActionLibrary.customerDetails.RETURN_OBJECT_TYPE_MAIN_BUSINESS_ENTITY];
                Label_Client_Name.Text = mBObj.getEntityName();
                custContact = mBObj.getPhNo();

                if (mBObj.getAddressDetails() != null)
                    localId = mBObj.getAddressDetails().getLocalityId();
            }

            //BackEndObjects.AddressDetails addrObj=BackEndObjects.AddressDetails.getAddressforMainBusinessEntitybyIdDB(ClientEntId);

            /*if (addrObj.getLocalityId() != null && !addrObj.getLocalityId().Equals(""))
            {
                custDetails += addrObj.getAddrLine1() + ",";
                localId = addrObj.getLocalityId();

                BackEndObjects.Localities lclObj = BackEndObjects.Localities.getLocalitybyIdDB(localId);
                BackEndObjects.City ctObj = BackEndObjects.Localities.getCityDetailsforLocalitywoOtherAsscLocalitiesDB(localId);
                BackEndObjects.State stObj = BackEndObjects.City.getStateDetailsforCitywoOtherAsscCitiesDB(ctObj.getCityId());
                BackEndObjects.Country cntObj = BackEndObjects.State.getCountryDetailsforStatewoOtherAsscStatesDB(stObj.getStateId());

                custDetails += lclObj.getLocalityName() + "<br/>" + ctObj.getCityName() + "<br/>" + stObj.getStateName() + "<br/>" + cntObj.getCountryName() + "<br/> Phone:" + custContact;

            }*/
            if(localId!=null && !localId.Equals(""))
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

                custDetails += lclObj.getLocalityName() + "<br/>" + ctObj.getCityName() + "<br/>" + stObj.getStateName() + "<br/>" + cntObj.getCountryName() + "<br/> Phone:" + custContact;

            }
            Label_Client_Addr.Text = custDetails;


        }

        protected void loadTnC()
        {
                        String context = Request.QueryString.GetValues("context")[0];
                        if (context.Equals("client"))
                        {
                            String entId = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();

                            ArrayList docFormatList = BackEndObjects.DocFormat.
                                getDocFormatforEntityIdandDocTypeDB(entId, BackEndObjects.DocFormat.DOCFORMAT_DOC_TYPE_PURCHASE_ORDER);

                            for (int i = 0; i < docFormatList.Count; i++)
                            {
                                BackEndObjects.DocFormat formatObj = (BackEndObjects.DocFormat)docFormatList[i];
                                if (formatObj.getSection_type().Equals(BackEndObjects.DocFormat.DOCFORMAT_DOC_TYPE_PURCHASE_ORDER_SECTION_TYPE_TNC))
                                {
                                    TextBox_TnC.Text = formatObj.getText();
                                    break;
                                }
                            }
                        }
        }

        protected void loadProductGrid()
        {
            String context = Request.QueryString.GetValues("context")[0];
            String[] rfqId = Request.QueryString.GetValues("rfqId");
            String respEntId="";
            ArrayList rfqSpecList=null;

            if (context.Equals("client"))
            {
                respEntId = Request.QueryString.GetValues("respCompId")[0];
                rfqSpecList = (ArrayList)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_RFQ_SPEC];
            }
            else
            {
                respEntId = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();
                rfqSpecList = (ArrayList)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_POTN_SPEC];
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("Hidden_Cat_Id");
            dt.Columns.Add("Serial");
            dt.Columns.Add("Qnty");
            dt.Columns.Add("Prod_Name");
            dt.Columns.Add("Cat_Name");
            dt.Columns.Add("Unit_Price");
            dt.Columns.Add("Cat_Total");


            float totalAmnt = 0;

            ArrayList rfqQntyList=BackEndObjects.RFQProdServQnty.getRFQProductServiceQuantityforRFIdDB(rfqId[0]);
            Dictionary<String,RFQResponseQuotes> respQuoteDict=BackEndObjects.RFQResponseQuotes.getAllResponseQuotesforRFQandResponseEntityDB(rfqId[0],respEntId);

            int k = 0;
            for (int i = 0; i < rfqQntyList.Count; i++)
            {
                BackEndObjects.RFQProdServQnty rfqQntyObj = (RFQProdServQnty)rfqQntyList[i];
                //Preference to toQuantity
                float qnty = rfqQntyObj.getToQnty() > 0 ? rfqQntyObj.getToQnty() : rfqQntyObj.getFromQnty();
                String catId = rfqQntyObj.getProdCatId();

                RFQResponseQuotes respQuoteObj = respQuoteDict != null && respQuoteDict.ContainsKey(catId)?respQuoteDict[catId]:null;
                if (respQuoteObj != null)
                {
                    String quote = respQuoteObj.getQuote();
                    String prodName = respQuoteObj.getProductName();

                    dt.Rows.Add();
                    dt.Rows[k]["Hidden_Cat_Id"] = catId;
                    dt.Rows[k]["Serial"] = k + 1;
                    dt.Rows[k]["Qnty"] = qnty.ToString();
                    dt.Rows[k]["Prod_Name"] = prodName;
                    dt.Rows[k]["Cat_Name"] = BackEndObjects.ProductCategory.getProductCategorybyIdwoFeaturesDB(catId).getProductCategoryName();
                    dt.Rows[k]["Unit_Price"] = quote;
                    dt.Rows[k]["Cat_Total"] = float.Parse(quote) * qnty;

                    totalAmnt += float.Parse(quote) * qnty;
                    k++;
                }
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                GridView1.Visible = true;
                GridView1.DataSource = dt;
                GridView1.DataBind();
                GridView1.Columns[0].Visible = false;

                foreach (GridViewRow gVR in GridView1.Rows)
                {
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
                Label_Total_Amount_Value.Visible = true;
                Label_Total_Amount_Value.Text = updateTotalPOAmount();
            }
            else
            {
                //Warning --- no prod specification found

                GridView1.Visible = false;
                Label_GridView_Status.Visible = true;
                if(context.Equals("client"))
                Label_GridView_Status.Text = "* No product details added in the RFQ. Only product details added in RFQ will populate in Purchase Order";
                else
                    Label_GridView_Status.Text = "* No product details added in the Potential. Only product details added in Potential will populate in Sales Order";

                Label_GridView_Status.ForeColor = System.Drawing.Color.Red;
            }
            
            Session[SessionFactory.ALL_PURCHASE_FINALIZE_DEAL_PO_PROD_GRID] = dt;
        }

        protected void populateLogo()
        {
            String context = Request.QueryString.GetValues("context")[0];
            
            ArrayList imgListObjs = BackEndObjects.Image.getImagebyTypeandEntId(BackEndObjects.Image.PICTURE_IMG_TYPE_LOGO, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            if (imgListObjs.Count > 0)
            {
                //Only consider the first image object for logo
                BackEndObjects.Image imgObj = (BackEndObjects.Image)imgListObjs[0];
                if (context.Equals("client"))
                {
                    Image_Logo.ImageUrl = imgObj.getImgPath();
                    Image_Logo.Visible = true;
                }
                else
                {
                    Image_Logo1.ImageUrl = imgObj.getImgPath();
                    Image_Logo1.Visible = true;
                }
            }
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GridView1.DataSource = (DataTable)Session[SessionFactory.ALL_PURCHASE_FINALIZE_DEAL_PO_PROD_GRID];
            GridView1.DataBind();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GridView1.DataSource =(DataTable) Session[SessionFactory.ALL_PURCHASE_FINALIZE_DEAL_PO_PROD_GRID];
            GridView1.DataBind();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }
        
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void GridView1_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {

        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void TextBox_tax_TextChanged(object sender, EventArgs e)
        {
            Label_Total_Amount_Value.Text = updateTotalPOAmount();
        }

        protected String updateTotalPOAmount()
        {
            float tax = TextBox_tax.Text.Equals("") ? 0 : float.Parse(TextBox_tax.Text);
            return ((float.Parse(Label_Sub_Total_Amount_Value.Text) * tax)/100+ float.Parse(Label_Sub_Total_Amount_Value.Text)).ToString();
        }

        protected void TextBox_Qnty_TextChanged(object sender, EventArgs e)
        {
            float qnty=float.Parse(((TextBox)sender).Text);
            GridViewRow grdViewProdRow = (GridViewRow)((TextBox)sender).Parent.Parent.Parent.Parent;

            float unitPrice = float.Parse(((TextBox)grdViewProdRow.FindControl("TextBox_Unit_Price")).Text);

            float prevVal = float.Parse(((Label)grdViewProdRow.FindControl("Label_Amount")).Text);

            ((Label)grdViewProdRow.FindControl("Label_Amount")).Text = (qnty * unitPrice).ToString();

            Label_Sub_Total_Amount_Value.Text = (float.Parse(Label_Sub_Total_Amount_Value.Text) +
                float.Parse(((Label)grdViewProdRow.FindControl("Label_Amount")).Text)-prevVal).ToString();

            Label_Total_Amount_Value.Text = updateTotalPOAmount();
        }

        protected void TextBox_Unit_Price_TextChanged(object sender, EventArgs e)
        {
            float unitPrice = float.Parse(((TextBox)sender).Text);
            GridViewRow grdViewProdRow = (GridViewRow)((TextBox)sender).Parent.Parent.Parent.Parent;

            float qnty = float.Parse(((TextBox)grdViewProdRow.FindControl("TextBox_Qnty")).Text);

            float prevVal = float.Parse(((Label)grdViewProdRow.FindControl("Label_Amount")).Text);

            ((Label)grdViewProdRow.FindControl("Label_Amount")).Text = (qnty * unitPrice).ToString();

            Label_Sub_Total_Amount_Value.Text = (float.Parse(Label_Sub_Total_Amount_Value.Text) + 
    float.Parse(((Label)grdViewProdRow.FindControl("Label_Amount")).Text)-prevVal).ToString();
            
            Label_Total_Amount_Value.Text = updateTotalPOAmount();
        }

        protected void Button_Create_Req_Click(object sender, EventArgs e)
        {
            BackEndObjects.PurchaseOrder POObj = new BackEndObjects.PurchaseOrder();
            String context = Request.QueryString.GetValues("context")[0];

            String poId=new BackEndObjects.Id().getNewId(BackEndObjects.Id.ID_TYPE_PO_ID_STRING);

            POObj.setDate_created(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            POObj.setPo_id(poId);
            POObj.setPo_ship_via(TextBox_Ship_Via.Text);
            POObj.setPo_tnc(TextBox_TnC.Text);
            POObj.setRfq_id(Request.QueryString.GetValues("rfqId")[0]);
            POObj.setTotal_tax_rate((!TextBox_tax.Text.Equals("")?TextBox_tax.Text:"0"));
            POObj.setCurrency(DropDownList_Curr.SelectedValue);
            if (context.Equals("client"))
                POObj.setRespEntId(Request.QueryString.GetValues("respCompId")[0]);
            else
                POObj.setRespEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                        

            ArrayList POQuoteObjList = new ArrayList();

            foreach (GridViewRow gVR in GridView1.Rows)
            {

                BackEndObjects.PurchaseOrderQuote POQuoteObj = new BackEndObjects.PurchaseOrderQuote();

                POQuoteObj.setPo_id(poId);
                POQuoteObj.setProd_srv_category(((Label)gVR.Cells[0].FindControl("Label_Hidden")).Text);
                POQuoteObj.setProduct_name(((Label)gVR.Cells[0].FindControl("Label_Product_Name")).Text);
                POQuoteObj.setQuote(float.Parse(((TextBox)gVR.Cells[0].FindControl("TextBox_Unit_Price")).Text));
                POQuoteObj.setUnits(float.Parse(((TextBox)gVR.Cells[0].FindControl("TextBox_Qnty")).Text));

                POQuoteObjList.Add(POQuoteObj);
            }

            try
            {
                BackEndObjects.PurchaseOrder.insertPurchaseOrderDB(POObj);
                BackEndObjects.PurchaseOrderQuote.insertPurchaseOrderQuoteListDB(POQuoteObjList);
                Label_PO_No.Visible = true;
                Label_PO_No.Text = poId;
                Label_PO_Creation_Stat.Visible = true;
                Label_PO_Creation_Stat.ForeColor = System.Drawing.Color.Green;
                if (context.Equals("client"))
                    Label_PO_Creation_Stat.Text = "Purchase Order Created Successfully";
                else
                    Label_PO_Creation_Stat.Text = "Sales Order Created Successfully";

                String dataItemIndex = Request.QueryString.GetValues("dataItemIndex") != null?  Request.QueryString.GetValues("dataItemIndex")[0]:"";
                if (!dataItemIndex.Equals(""))
                {
                    DataTable dt = (DataTable)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_GRID_DATA];
                    dt.Rows[Int32.Parse(dataItemIndex)]["PO_Id"] = POObj.getPo_id();
                    Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_GRID_DATA] = dt;
                    ScriptManager.RegisterStartupScript(this, typeof(string), "RefreshPotnGrid", "RefreshParent();", true);
                }
                Button_Create_PO.Enabled = false;
            }
            catch (Exception ex)
            {
                Label_PO_Creation_Stat.Visible = true;
                Label_PO_Creation_Stat.ForeColor = System.Drawing.Color.Red;
                if (context.Equals("client"))
                    Label_PO_Creation_Stat.Text = "Purchase Order Creation Failed";
                else
                    Label_PO_Creation_Stat.Text = "Sales Order Creation Failed";
            }

        }
        

    }
}