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
    public partial class createPotential : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadProductCat();
                LoadUnitsOfMsrmnt();
                LoadPotStageandState();
                loadCountry();
                loadContacts();
                loadProductList();
                loadUserList();
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

        protected void loadUserList()
        {
            Dictionary<String, userDetails> allUserDetails = BackEndObjects.MainBusinessEntity.getUserDetailsforMainEntitybyIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());


            foreach (KeyValuePair<String, userDetails> kvp in allUserDetails)
            {
                BackEndObjects.userDetails userDetObj = kvp.Value;

                ListItem lt1 = new ListItem();
                lt1.Text = userDetObj.getUserId();
                lt1.Value = userDetObj.getUserId();

                DropDownList_Users.Items.Add(lt1);
            }

            ListItem emptyItem = new ListItem();
            emptyItem.Text = "";
            emptyItem.Value = "";

            DropDownList_Users.Items.Add(emptyItem);
            DropDownList_Users.SelectedValue = "";
        }

                /// <summary>
        /// Get the parent cateogry list
        /// </summary>
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
            Label_Per_Unit.Text = "Numbers";
            Label_Per_Unit.Visible = true;
        }

        protected void LoadPotStageandState()
        {
            ListItem ltStg1 = new ListItem();
            ltStg1.Text = BackEndObjects.PotentialStatus.POTENTIAL_STAT_PRELIM;
            ltStg1.Value = BackEndObjects.PotentialStatus.POTENTIAL_STAT_PRELIM;

            ListItem ltStg2 = new ListItem();
            ltStg2.Text = BackEndObjects.PotentialStatus.POTENTIAL_STAT_MEDIUM;
            ltStg2.Value = BackEndObjects.PotentialStatus.POTENTIAL_STAT_MEDIUM;

            ListItem ltStg3 = new ListItem();
            ltStg3.Text = BackEndObjects.PotentialStatus.POTENTIAL_STAT_ADVNCD;
            ltStg3.Value = BackEndObjects.PotentialStatus.POTENTIAL_STAT_ADVNCD;

            ListItem ltAct1 = new ListItem();
            ltAct1.Text = BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_ACTIVE;
            ltAct1.Value = BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_ACTIVE;

            ListItem ltAct2 = new ListItem();
            ltAct2.Text = BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_NONE;
            ltAct2.Value = BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_NONE;

            ListItem ltAct3 = new ListItem();
            ltAct3.Text = BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_NOT_OPEN;
            ltAct3.Value = BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_NOT_OPEN;

            DropDownList_Pot_Stage.Items.Add(ltStg1);
            DropDownList_Pot_Stage.Items.Add(ltStg2);
            DropDownList_Pot_Stage.Items.Add(ltStg3);

            DropDownList_Pot_State.Items.Add(ltAct1);
            DropDownList_Pot_State.Items.Add(ltAct2);
            DropDownList_Pot_State.Items.Add(ltAct3);
        }

        protected void loadCountry()
        {
            Dictionary<String, Country> countryDict = BackEndObjects.Country.getAllCountrywoStatesDB();
            ListItem ltCountry1 = new ListItem();
            ltCountry1.Text = " ";
            ltCountry1.Value = "_";
            DropDownList_Country.Items.Add(ltCountry1);

            foreach (KeyValuePair<String, Country> kvp in countryDict)
            {
                ListItem ltCountry = new ListItem();
                ltCountry.Text = ((Country)kvp.Value).getCountryName();
                ltCountry.Value = ((Country)kvp.Value).getCountryId();

                DropDownList_Country.Items.Add(ltCountry);
            }
            DropDownList_Country.SelectedValue = "_";
        }

        protected void loadContacts()
        {
            //ArrayList contactObjList = Contacts.getAllContactsbyEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            if (DropDownList_Contacts.Items != null && DropDownList_Contacts.Items.Count > 0)
                DropDownList_Contacts.Items.Clear();
            Dictionary<String, String> existingContactDict = (Dictionary<String, String>)Session[SessionFactory.EXISTING_CONTACT_DICTIONARY];
            if(existingContactDict!=null)
            foreach (KeyValuePair<String, String> kvp in existingContactDict)
            {
                String contactName = kvp.Key;
                String contactEntId = kvp.Value;

                ListItem ltItem = new ListItem();
                ltItem.Text = contactName;
                ltItem.Value = contactEntId;
                DropDownList_Contacts.Items.Add(ltItem);

            }

        }

        protected void DropDownList_Locality_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void DropDownList_Locality_TextChanged(object sender, EventArgs e)
        {

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
            String selectedProdCatId = "";
            if(Session[SessionFactory.CREATE_POTENTIAL_SELECTED_PRODUCT_CAT]!=null && !Session[SessionFactory.CREATE_POTENTIAL_SELECTED_PRODUCT_CAT].Equals(""))
            selectedProdCatId=Session[SessionFactory.CREATE_POTENTIAL_SELECTED_PRODUCT_CAT].ToString();

            if (!selectedProdCatId.Equals(""))
            {
                Dictionary<String, Features> featDict = BackEndObjects.ProductCategory.getFeatureforCategoryDB(selectedProdCatId);

                if (featDict.Count > 0)
                {
                    GridView1.SelectedIndex = -1;
                    GridView1.Visible = true;
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
        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

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

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            fillGrid();
        }

        protected void createPotentialManul()
        {
            if (DropDownList_Level1.SelectedValue.Equals("_"))
            {
                Label_Status.Text = "Please select one product category";
                Label_Status.ForeColor = System.Drawing.Color.Red;
                Label_Status.Visible = true;

                Button_Submit_Pot.Enabled = true;
                Button_Submit_Next.Enabled = true;
                Button_Submit_Extra_Prd_Srv.Enabled = true;
            }
            else
            {
                BackEndObjects.RFQDetails rfqObj = new BackEndObjects.RFQDetails();
                BackEndObjects.Id idGen = new BackEndObjects.Id();
                String rfqId = "";
                if (Session[SessionFactory.CREATE_POTENTIAL_RFQ_ID] == null)
                {
                    rfqId = idGen.getNewId(BackEndObjects.Id.ID_TYPE_RFQ_STRING);
                    Session[SessionFactory.CREATE_POTENTIAL_RFQ_ID] = rfqId; //store the newly created RFQ id in the session
                }
                if (!TextBox_Spec.Text.Equals(""))
                {
                    getAddintionalProdSrvList();
                    TextBox_Spec.Text = "";
                }

                ArrayList rfqSpecObjList = (ArrayList)Session[SessionFactory.CREATE_POTENTIAL_SELECTED_POTN_SPEC_MAP];

                //Set the RFQ id for all the  spec objects
                if (rfqSpecObjList != null)
                    for (int i = 0; i < rfqSpecObjList.Count; i++)
                        ((BackEndObjects.RFQProductServiceDetails)rfqSpecObjList[i]).setRFQId(rfqId);

                rfqObj.setRFQProdServList(rfqSpecObjList);
                rfqObj.setRFQId(rfqId);
                rfqObj.setCreatedUsr(User.Identity.Name);
                rfqObj.setActiveStat(BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_ACTIVE);
                rfqObj.setDueDate(TextBox_Within_Date.Text);
                rfqObj.setEntityId(DropDownList_Contacts.SelectedValue);
                rfqObj.setCreatedEntity(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                rfqObj.setCurrency(DropDownList_Curr.SelectedValue);
                //     FileUpload1=(FileUpload1 == null ? (FileUpload)Session[SessionFactory.CREATE_RFQ_NDA_FILE] : FileUpload1);
               /* if (FileUpload1 != null && FileUpload1.HasFile)
                {
                    rfqObj.setFileStream(FileUpload1);
                    rfqObj.setNDADocPathInFileStore(rfqObj.getEntityId());

                }
                else if ((FileUpload)Session[SessionFactory.CREATE_POTENTIAL_NDA_FILE] != null && ((FileUpload)Session[SessionFactory.CREATE_POTENTIAL_NDA_FILE]).HasFile)
                {
                    rfqObj.setFileStream((FileUpload)Session[SessionFactory.CREATE_POTENTIAL_NDA_FILE]);
                    rfqObj.setNDADocPathInFileStore(rfqObj.getEntityId());
                }*/
                String localId = (!DropDownList_Locality.SelectedValue.Equals("_") && !DropDownList_Locality.SelectedValue.Equals("") ?
    DropDownList_Locality.SelectedValue : (!DropDownList_City.SelectedValue.Equals("_") && !DropDownList_City.SelectedValue.Equals("") ?
    DropDownList_City.SelectedValue : (!DropDownList_State.SelectedValue.Equals("_") && !DropDownList_State.SelectedValue.Equals("") ?
    DropDownList_State.SelectedValue : (!DropDownList_Country.SelectedValue.Equals("_") && !DropDownList_Country.SelectedValue.Equals("") ?
    DropDownList_Country.SelectedValue : ""))));

                rfqObj.setLocalityId(localId);
                rfqObj.setSubmitDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                rfqObj.setTermsandConds(TextBox_TnC.Text);
                rfqObj.setRFQName(TextBox_Reqr_Name.Text);
                //RFQ creation mode is manual while manually creating lead/potential
                rfqObj.setCreateMode(RFQDetails.CREATION_MODE_MANUAL);

                BackEndObjects.RFQProdServQnty rfqPrdQnty = new BackEndObjects.RFQProdServQnty();
                rfqPrdQnty.setRFQId(rfqObj.getRFQId());
                rfqPrdQnty.setFromPrice(TextBox_Price_Range_From.Text);
                rfqPrdQnty.setToPrice(TextBox_Price_Range_To.Text);
                rfqPrdQnty.setFromQnty(float.Parse(TextBox_Prod_Qnty_From.Text));
                rfqPrdQnty.setToQnty(float.Parse(TextBox_Prod_Qnty_To.Text));
                rfqPrdQnty.setMsrmntUnit(DropDownList_Unit_Of_Msrmnt.SelectedValue);
                rfqPrdQnty.setProdCatId(Session[SessionFactory.CREATE_POTENTIAL_SELECTED_PRODUCT_CAT].ToString());
                //ArrayList rfqPrdQntyList = new ArrayList();
                //rfqPrdQntyList.Add(rfqPrdQnty);

                ArrayList prodSrvQntyList = (ArrayList)Session[SessionFactory.CREATE_POTENTIAL_PROD_SRV_QNTY_LIST];

                if (prodSrvQntyList == null)
                    prodSrvQntyList = new ArrayList();

                prodSrvQntyList.Add(rfqPrdQnty);

                //Ensure that all objects in the arraylist has an associated rfqid
                for (int i = 0; i < prodSrvQntyList.Count; i++)
                {
                    BackEndObjects.RFQProdServQnty temp = (RFQProdServQnty)prodSrvQntyList[i];
                    if (temp.getRFQId() == null || temp.getRFQId().Equals(""))
                        temp.setRFQId(rfqObj.getRFQId());
                }

                rfqObj.setRFQProdServQntyList(prodSrvQntyList);

                Dictionary<String, String> reqSpecUniqnessValidation = new Dictionary<string, string>();

                if(rfqSpecObjList!=null)
                for (int i = 0; i < rfqSpecObjList.Count; i++)
                {
                    BackEndObjects.RFQProductServiceDetails rfqSpecObj = (BackEndObjects.RFQProductServiceDetails)rfqSpecObjList[i];
                    if (reqSpecUniqnessValidation.ContainsKey(rfqSpecObj.getPrdCatId() + ":" + rfqSpecObj.getFeatId()))
                        rfqSpecObjList.RemoveAt(i);//Remove the current RFQ spec object from the list - otherwise it will cause exception at DB layer while inserting
                    else
                    {
                        reqSpecUniqnessValidation.Add(rfqSpecObj.getPrdCatId() + ":" + rfqSpecObj.getFeatId(), "");
                        if (rfqSpecObj.getFileStream() != null)
                            rfqSpecObj.setImgPathInFileStore(rfqObj.getEntityId());
                    }
                }

                BackEndObjects.RFQResponse leadRespObj = new RFQResponse();
                leadRespObj.setRespEntityId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                leadRespObj.setRFQId(Session[SessionFactory.CREATE_POTENTIAL_RFQ_ID].ToString());
                leadRespObj.setRespDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                leadRespObj.setAssignedTo(DropDownList_Users.SelectedValue);
                leadRespObj.setNextFollowupDate(TextBox_Fwp_Date.Text);

                //Load the document for the lead response 
                if (FileUpload1 != null && FileUpload1.HasFile)
                {
                    leadRespObj.setFileStream(FileUpload1);
                    leadRespObj.setNDADocPathInFileStore(leadRespObj.getRespEntityId());

                }
                else if ((FileUpload)Session[SessionFactory.CREATE_POTENTIAL_NDA_FILE] != null && ((FileUpload)Session[SessionFactory.CREATE_POTENTIAL_NDA_FILE]).HasFile)
                {
                    leadRespObj.setFileStream((FileUpload)Session[SessionFactory.CREATE_POTENTIAL_NDA_FILE]);
                    leadRespObj.setNDADocPathInFileStore(leadRespObj.getRespEntityId());
                }

                ArrayList leadRespQuoteList = (ArrayList)Session[SessionFactory.CREATE_POTENTIAL_RESP_QUOTE_LIST];
                if (leadRespQuoteList == null)
                    leadRespQuoteList = new ArrayList();

                BackEndObjects.RFQResponseQuotes respQuoteObj = new RFQResponseQuotes();
                    respQuoteObj.setPrdCatId(Session[SessionFactory.CREATE_POTENTIAL_SELECTED_PRODUCT_CAT].ToString());
                    respQuoteObj.setQuote((TextBox_Quote_Amnt.Text.Equals("") ? "0" : TextBox_Quote_Amnt.Text));
                    respQuoteObj.setResponseEntityId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                    respQuoteObj.setResponseUsrId(User.Identity.Name);
                    //respQuoteObj.setUnitName(Session[SessionFactory.CREATE_LEAD_QUOTE_UNIT].ToString());
                    respQuoteObj.setUnitName(Label_Per_Unit.Text);
                    respQuoteObj.setProductName((!TextBox_Prod_Name.Text.Equals("") ? TextBox_Prod_Name.Text.Trim() : DropDownList_Prod_List.SelectedItem.Text));

                leadRespQuoteList.Add(respQuoteObj);
                //Ensure that all objects in the arraylist has an associated rfqid
                for (int i = 0; i < leadRespQuoteList.Count; i++)
                {
                    BackEndObjects.RFQResponseQuotes temp = (RFQResponseQuotes)leadRespQuoteList[i];
                    if (temp.getRFQId() == null || temp.getRFQId().Equals(""))
                        temp.setRFQId(rfqObj.getRFQId());
                }

                leadRespObj.setResponseQuoteList(leadRespQuoteList);

                ActionLibrary.SalesActions._createLeads cL = new ActionLibrary.SalesActions._createLeads();

                LeadRecord leadObj = new LeadRecord();
                leadObj.setRFQProdServList(rfqObj.getRFQProdServList());
                leadObj.setRFQId(rfqObj.getRFQId());
                leadObj.setActiveStat(rfqObj.getActiveStat());
                leadObj.setApprovalStat(rfqObj.getApprovalStat());
                leadObj.setCreatedEntity(rfqObj.getCreatedEntity());
                leadObj.setCreatedUsr(rfqObj.getCreatedUsr());
                leadObj.setCreateMode(rfqObj.getCreateMode());
                leadObj.setDueDate(rfqObj.getDueDate());
                leadObj.setEntityId(rfqObj.getEntityId());
                leadObj.setFileStream(rfqObj.getFileStream());
                leadObj.setLocalityId(rfqObj.getLocalityId());
                leadObj.setNDADocPath(rfqObj.getNDADocPath());
                leadObj.setReqId(rfqObj.getReqId());
                leadObj.setRFQName(rfqObj.getRFQName());
                leadObj.setRFQProdServQntyList(rfqObj.getRFQProdServQntyList());
                leadObj.setSubmitDate(rfqObj.getSubmitDate());
                leadObj.setTermsandConds(rfqObj.getTermsandConds());
                leadObj.setCurrency(rfqObj.getCurrency());

                leadObj.setLeadResp(leadRespObj);

                BackEndObjects.RFQShortlisted potObj = new RFQShortlisted();

                potObj.setRFQId(rfqObj.getRFQId());
                potObj.setRespEntityId(rfqObj.getCreatedEntity());
                potObj.setPotStat(DropDownList_Pot_Stage.SelectedValue);
                potObj.setPotentialId(new Id().getNewId(BackEndObjects.Id.ID_TYPE_POTENTIAL_ID_STRING));
                potObj.setPotenAmnt(calculatePotAmnt(leadRespQuoteList, prodSrvQntyList));
                potObj.setPotActStat(DropDownList_Pot_State.SelectedValue);
                potObj.setFinlSupFlag("N");
                potObj.setFinlCustFlag("N");
                potObj.setCreateMode(rfqObj.getCreateMode());
                potObj.setCreatedDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                //potObj.setConfMatPathInFileStore(potObj.getRespEntityId());

               /* if (FileUpload2 != null && FileUpload2.HasFile)
                {
                    potObj.setFileStream(FileUpload2);
                    potObj.setConfMatPathInFileStore(potObj.getRespEntityId());

                }*/
                /*else if ((FileUpload)Session[SessionFactory.CREATE_POTENTIAL_CONF_MAT] != null &&
                    ((FileUpload)Session[SessionFactory.CREATE_POTENTIAL_CONF_MAT]).HasFile)
                {
                    potObj.setFileStream((FileUpload)Session[SessionFactory.CREATE_POTENTIAL_CONF_MAT]);
                    potObj.setConfMatPathInFileStore(potObj.getRespEntityId());
                }*/


                try
                {
                    cL.createNewLead(leadObj, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), User.Identity.Name);
                    BackEndObjects.RFQShortlisted.insertRFQShorListedEntryDB(potObj);
                    Label_Status.Text = "Potential created successfully";
                    Label_Status.ForeColor = System.Drawing.Color.Green;
                    Label_Status.Visible = true;

                    DataTable dt = (DataTable)Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_GRID_DATA];
                    Dictionary<String, Currency> allCurrList = (Dictionary<String, Currency>)Session[SessionFactory.CURRENCY_LIST];
                    //DateUtility dU = new DateUtility();
                                       

                    dt.Rows.Add();
                    int count = dt.Rows.Count - 1;

                    dt.Rows[count]["RFQNo"] = leadObj.getRFQId();
                    dt.Rows[count]["RFQName"] = leadObj.getRFQName();
                    dt.Rows[count]["curr"] = allCurrList.ContainsKey(leadObj.getCurrency()) ?
                                        allCurrList[leadObj.getCurrency()].getCurrencyName() : "";

                    String alertRequired = "true";
                    for (int j = 0; j < leadRespQuoteList.Count; j++)
                    {
                        RFQResponseQuotes QuoteObj = (RFQResponseQuotes)leadRespQuoteList[j];
                        if (!QuoteObj.getQuote().Equals("0"))
                        { alertRequired = "false"; break; }
                    }

                    DateUtility dU = new DateUtility();
                    
                    dt.Rows[count]["PotId"] = potObj.getPotentialId();
                    dt.Rows[count]["Potn_Alert_Required"] = alertRequired;
                    dt.Rows[count]["DateCreated"] = potObj.getCreatedDate();
                    dt.Rows[count]["DateCreatedTicks"] = Convert.ToDateTime(potObj.getCreatedDate()).Ticks;
                    dt.Rows[count]["CustName"] = DropDownList_Contacts.SelectedItem.Text;
                    dt.Rows[count]["CustId"] = leadObj.getEntityId();
                    dt.Rows[count]["PotAmnt"] = potObj.getPotenAmnt();
                    dt.Rows[count]["Due Date"] = dU.getConvertedDateWoTime(leadObj.getDueDate());
                    dt.Rows[count]["Due Date Ticks"] = !leadObj.getDueDate().Equals("")?Convert.ToDateTime(leadObj.getDueDate()).Ticks:0;
                    dt.Rows[count]["Next Date"] = dU.getConvertedDateWoTime(TextBox_Fwp_Date.Text.Trim());
                    dt.Rows[count]["Next Date Ticks"] = (!TextBox_Fwp_Date.Text.Equals("") ? Convert.ToDateTime(TextBox_Fwp_Date.Text).Ticks : 0);
                    dt.Rows[count]["Assgn To"] = (!DropDownList_Users.SelectedValue.Equals("") ? DropDownList_Users.SelectedValue : "");
                    dt.Rows[count]["PotStage"] = potObj.getPotStat();
                    dt.Rows[count]["ActiveStat"] = potObj.getPotActStat();
                    dt.Rows[count]["Mode"] = potObj.getCreateMode();
                    dt.Rows[count]["PO_Id"] = "N/A";
                    

                    dt.DefaultView.Sort = "DateCreatedTicks" + " " + "DESC";
                    Session[SessionFactory.ALL_SALE_ALL_POTENTIAL_GRID_DATA] = dt.DefaultView.ToTable();

                    ScriptManager.RegisterStartupScript(this, typeof(string), "RefreshPotnGrid", "RefreshParent();", true);


                }
                catch (Exception ex)
                {
                    Label_Status.Text = "Potential creation failed";
                    Label_Status.ForeColor = System.Drawing.Color.Red;
                    Label_Status.Visible = true;
                }
                finally
                {
                    Session.Remove(SessionFactory.CREATE_POTENTIAL_SELECTED_POTN_SPEC_MAP);
                    Session.Remove(SessionFactory.CREATE_POTENTIAL_RFQ_ID);
                    Session.Remove(SessionFactory.CREATE_POTENTIAL_PROD_SRV_QNTY_LIST);
                    Session.Remove(SessionFactory.CREATE_POTENTIAL_NDA_FILE);
                    Session.Remove(SessionFactory.CREATE_POTENTIAL_RESP_QUOTE_LIST);

                }
            }
        }

        /// <summary>
        /// Provided the reponse quote list and the product qunatity list this method will calculate the average potential amount
        /// </summary>
        /// <param name="respQuoteList"></param>
        /// <param name="prodQntyList"></param>
        /// <returns></returns>
        protected float calculatePotAmnt(ArrayList respQuoteList,ArrayList prodQntyList)
        {
            float totalAmnt = 0;
            Dictionary<String,BackEndObjects.RFQProdServQnty> respQuoteDict=new Dictionary<string,RFQProdServQnty>();

            for (int j = 0; j < prodQntyList.Count; j++)
            {
                respQuoteDict.Add(((RFQProdServQnty)prodQntyList[j]).getProdCatId(),(RFQProdServQnty)prodQntyList[j]);
            }

            for (int i = 0; i < respQuoteList.Count; i++)
            {
                BackEndObjects.RFQResponseQuotes respQuoteObj = (RFQResponseQuotes)respQuoteList[i];
                float fromQnty = (respQuoteDict[respQuoteObj.getPrdCatId()]).getFromQnty();
                float toQnty = (respQuoteDict[respQuoteObj.getPrdCatId()]).getToQnty();
                float quoteAmnt = float.Parse(respQuoteObj.getQuote());

                //Now calculate the average
                totalAmnt += (fromQnty * quoteAmnt + toQnty * quoteAmnt) / 2;
            }
            
            return totalAmnt;
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
            Session[SessionFactory.CREATE_POTENTIAL_SELECTED_PRODUCT_CAT] = DropDownList_Level1.SelectedValue;

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
            Session[SessionFactory.CREATE_POTENTIAL_SELECTED_PRODUCT_CAT] = DropDownList_Level2.SelectedValue;
        }

        protected void DropDownList_Level3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session[SessionFactory.CREATE_POTENTIAL_SELECTED_PRODUCT_CAT] = DropDownList_Level3.SelectedValue;
        }

        protected void DropDownList_Country_SelectedIndexChanged(object sender, EventArgs e)
        {
            String itemId = DropDownList_Country.SelectedValue.ToString();
            Dictionary<String, BackEndObjects.State> stateList = new Dictionary<string, BackEndObjects.State>();
            DropDownList_State.Items.Clear();

            stateList = BackEndObjects.State.getStatesforCountrywoCitiesDB(itemId.Trim());
            ListItem ltFirst = new ListItem();
            ltFirst.Text = "_";
            ltFirst.Value = "_";
            DropDownList_State.Items.Add(ltFirst);

            foreach (KeyValuePair<String, BackEndObjects.State> kvp in stateList)
            {
                ListItem ltState = new ListItem();
                ltState.Value = ((BackEndObjects.State)kvp.Value).getStateId();
                ltState.Text = ((BackEndObjects.State)kvp.Value).getStateName();
                DropDownList_State.Items.Add(ltState);
            }
            if (DropDownList_State.Items.Count > 0)
                DropDownList_State.SelectedValue = "_";
        }

        /// <summary>
        /// A change in State selection should populate the City list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DropDownList_State_SelectedIndexChanged(object sender, EventArgs e)
        {
            String itemId = DropDownList_State.SelectedValue.ToString();
            Dictionary<String, BackEndObjects.City> cityList = new Dictionary<string, BackEndObjects.City>();
            DropDownList_City.Items.Clear();

            cityList = BackEndObjects.City.getCitiesforStatewoLocalitiesDB(itemId.Trim());

            foreach (KeyValuePair<String, BackEndObjects.City> kvp in cityList)
            {
                ListItem ltCity = new ListItem();
                ltCity.Value = ((BackEndObjects.City)kvp.Value).getCityId();
                ltCity.Text = ((BackEndObjects.City)kvp.Value).getCityName();
                DropDownList_City.Items.Add(ltCity);
            }
            ListItem ltFirst = new ListItem();
            ltFirst.Text = "_";
            ltFirst.Value = "_";
            DropDownList_City.Items.Add(ltFirst);

            if (DropDownList_City.Items.Count > 0)
                DropDownList_City.SelectedValue = "_";
        }
        /// <summary>
        /// A change in City selection should populate the locality list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DropDownList_City_SelectedIndexChanged(object sender, EventArgs e)
        {
            String itemId = DropDownList_City.SelectedValue.ToString();
            Dictionary<String, BackEndObjects.Localities> localList = new Dictionary<string, BackEndObjects.Localities>();
            DropDownList_Locality.Items.Clear();

            localList = BackEndObjects.Localities.getLocalitiesforCityDB(itemId.Trim());

            foreach (KeyValuePair<String, BackEndObjects.Localities> kvp in localList)
            {
                ListItem ltLocal = new ListItem();
                ltLocal.Value = ((BackEndObjects.Localities)kvp.Value).getLocalityId();
                ltLocal.Text = ((BackEndObjects.Localities)kvp.Value).getLocalityName();
                DropDownList_Locality.Items.Add(ltLocal);
            }
            ListItem ltFirst = new ListItem();
            ltFirst.Text = "_";
            ltFirst.Value = "_";
            DropDownList_Locality.Items.Add(ltFirst);

            if (DropDownList_Locality.Items.Count > 0)
                DropDownList_Locality.SelectedValue = "_";

        }
        protected void DropDownList2_TextChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// A change in State selection should populate the City list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ArrayList rfqProdSrvList = (ArrayList)Session[SessionFactory.CREATE_POTENTIAL_SELECTED_POTN_SPEC_MAP];
            if (rfqProdSrvList == null)
                rfqProdSrvList = new ArrayList();

            BackEndObjects.RFQProductServiceDetails rfqSpec = new RFQProductServiceDetails();
            rfqSpec.setPrdCatId(Session[SessionFactory.CREATE_POTENTIAL_SELECTED_PRODUCT_CAT].ToString());
            rfqSpec.setFeatId(((Label)GridView1.SelectedRow.Cells[1].FindControl("Label_Hidden")).Text);
            rfqSpec.setFromSpecId(((DropDownList)GridView1.SelectedRow.Cells[2].FindControl("DropDownList_Gridview1_From")).SelectedValue);
            rfqSpec.setToSpecId(((DropDownList)GridView1.SelectedRow.Cells[3].FindControl("DropDownList_Gridview1_To")).SelectedValue);
            //rfqSpec.setSpecText(TextBox_Spec.Text);
            rfqSpec.setCreatedDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            if (User.Identity.Name != null)
                rfqSpec.setCreatedUsr(User.Identity.Name);
            if (((FileUpload)GridView1.SelectedRow.Cells[3].FindControl("FileUpload_Spec")).HasFile)
                rfqSpec.setFileStream((FileUpload)GridView1.SelectedRow.Cells[3].FindControl("FileUpload_Spec"));


            rfqProdSrvList.Add(rfqSpec);

            GridView1.SelectedRow.Cells[0].Enabled = false;
            GridView1.SelectedRow.Cells[3].Enabled = false;
            GridView1.SelectedRow.Cells[4].Enabled = false;
            GridView1.SelectedRow.Cells[5].Enabled = false;
            GridView1.SelectedRow.Cells[0].FindControl("Image_Selected").Visible = true;
            Label_Selected_List.Text += "," + GridView1.SelectedRow.DataItemIndex;

            Session[SessionFactory.CREATE_POTENTIAL_SELECTED_POTN_SPEC_MAP] = rfqProdSrvList;
            Buttin_Show_Spec_List.Enabled = false;
        }

        protected void GridView1_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {

        }

        protected void Calendar_Within_Date_SelectionChanged(object sender, EventArgs e)
        {
            //TextBox_Within_Date.Text = Calendar_Within_Date.SelectedDate.ToString("yyyy-MM-dd");
        }

        protected void getAddintionalProdSrvList()
        {
            ArrayList rfqProdSrvList = (ArrayList)Session[SessionFactory.CREATE_POTENTIAL_SELECTED_POTN_SPEC_MAP];
            if (rfqProdSrvList == null)
                rfqProdSrvList = new ArrayList();

            BackEndObjects.RFQProductServiceDetails rfqSpec = new RFQProductServiceDetails();
            rfqSpec.setPrdCatId(Session[SessionFactory.CREATE_POTENTIAL_SELECTED_PRODUCT_CAT].ToString());
            rfqSpec.setFeatId("ft_dummy");
            rfqSpec.setFromSpecId("");
            rfqSpec.setToSpecId("");
            rfqSpec.setSpecText(TextBox_Spec.Text);
            rfqSpec.setCreatedDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            if (User.Identity.Name != null)
                rfqSpec.setCreatedUsr(User.Identity.Name);
            if (FileUpload_Extra_Spec.HasFile)
                rfqSpec.setFileStream(FileUpload_Extra_Spec);
            

            rfqProdSrvList.Add(rfqSpec);

            Session[SessionFactory.CREATE_POTENTIAL_SELECTED_POTN_SPEC_MAP] = rfqProdSrvList;
        }
        /// <summary>
        /// Adding more prod service details to the potential
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_Submit_Extra_Prd_Srv_Click(object sender, EventArgs e)
        {
            if (DropDownList_Level1.SelectedValue.Equals("_"))
            {
                Label_Status.Text = "Please select one product category";
                Label_Status.ForeColor = System.Drawing.Color.Red;
                Label_Status.Visible = true;
                Label_Status.Focus();
            }
            else
            {
                Label_Status.Visible = false;

                Buttin_Show_Spec_List.Enabled = true;
                DropDownList_Locality.Enabled = false;
                DropDownList_State.Enabled = false;
                DropDownList_Country.Enabled = false;
                DropDownList_City.Enabled = false;
                TextBox_Within_Date.Enabled = false;
                TextBox_Reqr_Name.Enabled = false;
                TextBox_TnC.Enabled = false;
                TextBox_Street_Name.Enabled = false;
                FileUpload1.Enabled = false;

                Label_Selected_List.Text = "";
                Label_Extra_Spec.Visible = false;
                if (!TextBox_Spec.Text.Equals(""))
                {
                    getAddintionalProdSrvList();

                    TextBox_Spec.Text = "";
                }
                TextBox_Spec.Visible = false;
                Label_Extra_Spec_upload.Visible = false;
                FileUpload_Extra_Spec.Visible = false;

                GridView1.Visible = false;
                DropDownList_Level1.SelectedValue = "_";
                DropDownList_Level2.SelectedIndex = -1;
                DropDownList_Level3.SelectedIndex = -1;

                if (FileUpload1 != null && FileUpload1.HasFile)
                    Session[SessionFactory.CREATE_POTENTIAL_NDA_FILE] = FileUpload1;

                //if (FileUpload2 != null && FileUpload2.HasFile)
                    //Session[SessionFactory.CREATE_POTENTIAL_CONF_MAT] = FileUpload2;


                ArrayList prodSrvQntyList = (ArrayList)Session[SessionFactory.CREATE_POTENTIAL_PROD_SRV_QNTY_LIST];

                if (prodSrvQntyList == null)
                    prodSrvQntyList = new ArrayList();

                BackEndObjects.RFQProdServQnty rfqpPrdSrvQntyObj = new RFQProdServQnty();
                rfqpPrdSrvQntyObj.setFromPrice(TextBox_Price_Range_From.Text);
                rfqpPrdSrvQntyObj.setToPrice(TextBox_Price_Range_To.Text);
                rfqpPrdSrvQntyObj.setMsrmntUnit(DropDownList_Unit_Of_Msrmnt.SelectedValue);
                rfqpPrdSrvQntyObj.setProdCatId(Session[SessionFactory.CREATE_POTENTIAL_SELECTED_PRODUCT_CAT].ToString());
                rfqpPrdSrvQntyObj.setFromQnty(float.Parse(TextBox_Prod_Qnty_From.Text));
                rfqpPrdSrvQntyObj.setToQnty(float.Parse(TextBox_Prod_Qnty_To.Text));

                prodSrvQntyList.Add(rfqpPrdSrvQntyObj);

                Session[SessionFactory.CREATE_POTENTIAL_PROD_SRV_QNTY_LIST] = prodSrvQntyList;

                //Now add the response quote details for this product
                ArrayList leadRespQuoteList = (ArrayList)Session[SessionFactory.CREATE_POTENTIAL_RESP_QUOTE_LIST];
                if (leadRespQuoteList == null)
                    leadRespQuoteList = new ArrayList();

                BackEndObjects.RFQResponseQuotes respQuoteObj = new RFQResponseQuotes();
                respQuoteObj.setPrdCatId(Session[SessionFactory.CREATE_POTENTIAL_SELECTED_PRODUCT_CAT].ToString());
                respQuoteObj.setQuote(TextBox_Quote_Amnt.Text);
                respQuoteObj.setResponseEntityId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                respQuoteObj.setResponseUsrId(User.Identity.Name);
                //respQuoteObj.setUnitName(Session[SessionFactory.CREATE_LEAD_QUOTE_UNIT].ToString());
                respQuoteObj.setUnitName(Label_Per_Unit.Text);
                respQuoteObj.setProductName((!TextBox_Prod_Name.Text.Equals("") ? TextBox_Prod_Name.Text.Trim() : DropDownList_Prod_List.SelectedItem.Text));

                leadRespQuoteList.Add(respQuoteObj);
                Session[SessionFactory.CREATE_POTENTIAL_RESP_QUOTE_LIST] = leadRespQuoteList;
            }
        }

        protected void Button_Submit_Pot_Click(object sender, EventArgs e)
        {
            Button_Submit_Pot.Enabled = false;
            Button_Submit_Next.Enabled = false;
            Button_Submit_Extra_Prd_Srv.Enabled = false;

            createPotentialManul();
            Session.Remove(SessionFactory.CREATE_POTENTIAL_SELECTED_POTN_SPEC_MAP);

            /*clearAllFields("Panel2");
            clearAllFields("Panel3");
            clearAllFields("Panel_Prod_Service_Det");
            clearAllFields("Panel_Prod_Srv_Qnty");
            clearAllFields("Panel_Price_Range");
            clearAllFields("Panel_Location");*/
        }

        protected void Button_Submit_Next_Click(object sender, EventArgs e)
        {
            createPotentialManul();

            Buttin_Show_Spec_List.Enabled = true;
            DropDownList_Locality.Enabled = true;
            DropDownList_State.Enabled = true;
            DropDownList_Country.Enabled = true;
            DropDownList_City.Enabled = true;
            TextBox_Within_Date.Enabled = true;
            TextBox_Reqr_Name.Enabled = true;
            TextBox_TnC.Enabled = true;
            TextBox_Street_Name.Enabled = true;
            FileUpload1.Enabled = true;
            Label_Extra_Spec.Visible = true;
            TextBox_Spec.Visible = true;
            GridView1.Visible = true;
            Label_Extra_Spec.Visible = false;
            Label_Extra_Spec_upload.Visible = false;
            TextBox_Spec.Visible = false;
            FileUpload_Extra_Spec.Visible = false;
            Label_Selected_List.Text = "";

            clearAllFields("Panel2");
            clearAllFields("Panel3");
            clearAllFields("Panel_Prod_Service_Det");
            clearAllFields("Panel_Prod_Srv_Qnty");
            clearAllFields("Panel_Price_Range");
            clearAllFields("Panel_Location");
            clearAllFields("Panel_Terms_Conds");
            clearAllFields("Panel_Pot_State");
            clearAllFields("Panel_Quote");
        }

        protected void DropDownList_Unit_Of_Msrmnt_SelectedIndexChanged(object sender, EventArgs e)
        {
            Label_Per_Unit.Text = DropDownList_Unit_Of_Msrmnt.SelectedValue;
            Label_Per_Unit.Visible = true;
        }

        protected void DropDownList_Prod_List_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBox_Quote_Amnt.Text = DropDownList_Prod_List.SelectedValue;
        }


        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            String forwardString = "createContact.aspx";
            forwardString += "?parentContext=" + "potn";
            //Server.Transfer("createContact.aspx",true);

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispContactPotn", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=900');", true);

        }

        protected void Button_Refresh_Click(object sender, EventArgs e)
        {
            loadContacts();
            Button_Refresh.Focus();
        }

    }
    }
