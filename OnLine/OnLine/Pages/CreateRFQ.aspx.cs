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
    public partial class CreateRFQ : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                LoadProductCat();
                LoadUnitsOfMsrmnt();
                //LoadQuoteUnits();
                loadCountry();
                loadCurrency();
            }
        }
        /// <summary>
        /// Get the parent cateogry list
        /// </summary>
        protected void LoadProductCat()
        {
            Dictionary<String,ProductCategory> prodDict=BackEndObjects.ProductCategory.getAllParentCategory();
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
            ArrayList allUnits=BackEndObjects.UnitOfMsrmnt.getAllMsrmntUnitsDB();
            for (int i = 0; i < allUnits.Count; i++)
            {
                ListItem ltUnit = new ListItem();
                ltUnit.Value = ((BackEndObjects.UnitOfMsrmnt)allUnits[i]).getUnitName();
                ltUnit.Text = ((BackEndObjects.UnitOfMsrmnt)allUnits[i]).getUnitName();
                DropDownList_Unit_Of_Msrmnt.Items.Add(ltUnit);
            }
            DropDownList_Unit_Of_Msrmnt.SelectedValue = "Numbers";
        }
        protected void LoadQuoteUnits()
        {
           ArrayList listQuoteUnits= BackEndObjects.QuoteUnits.getAllQuoteUnitsDB();
           for (int i = 0; i < listQuoteUnits.Count; i++)
           {               
               Dictionary<String, float> unitAndDiv = ((BackEndObjects.QuoteUnits)listQuoteUnits[i]).getUnitAndDivisor();

               foreach (KeyValuePair<String, float> kvp in unitAndDiv)
               {
                   ListItem ltQU = new ListItem();
                   ltQU.Text = kvp.Key.ToString();
                   ltQU.Value = kvp.Key.ToString();

                   //DropDownList_Quote_Unit.Items.Add(ltQU);
               }
           }
        }
        protected void loadCountry()
        {
            Dictionary<String,Country> countryDict=BackEndObjects.Country.getAllCountrywoStatesDB();
            ListItem ltCountry1 = new ListItem();
            ltCountry1.Text = " ";
            ltCountry1.Value = "none";
            DropDownList_Country.Items.Add(ltCountry1);

            foreach (KeyValuePair<String, Country> kvp in countryDict)
            {
                ListItem ltCountry = new ListItem();
                ltCountry.Text = ((Country)kvp.Value).getCountryName();
                ltCountry.Value = ((Country)kvp.Value).getCountryId();

                DropDownList_Country.Items.Add(ltCountry);
            }
            DropDownList_Country.SelectedIndex = -1;
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
                if(defaultCurr.Equals(lt.Value.Trim()))
                    DropDownList_Curr.SelectedValue=lt.Value;
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
            if(Session[SessionFactory.CREATE_RFQ_SELECTED_PRODUCT_CAT]!=null && !Session[SessionFactory.CREATE_RFQ_SELECTED_PRODUCT_CAT].Equals(""))
            selectedProdCatId=Session[SessionFactory.CREATE_RFQ_SELECTED_PRODUCT_CAT].ToString();
            if (!selectedProdCatId.Equals(""))
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
        /// <summary>
        /// create the requirement details 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_Submit_Req_Click(object sender, EventArgs e)
        {
            Label_Status.Focus();
            Button_Submit_Req.Enabled = false;
            Button_Submit_Next.Enabled = false;
            Button_Submit_Extra_Prd_Srv.Enabled = false;

            createRfq();
        }

        protected void getAddintionalProdSrvList()
        {
            ArrayList rfqProdSrvList = (ArrayList)Session[SessionFactory.CREATE_RFQ_SELECTED_RFQ_SPEC_MAP];
            if (rfqProdSrvList == null)
                rfqProdSrvList = new ArrayList();

            BackEndObjects.RFQProductServiceDetails rfqSpec = new RFQProductServiceDetails();
            rfqSpec.setPrdCatId(Session[SessionFactory.CREATE_RFQ_SELECTED_PRODUCT_CAT].ToString());
            rfqSpec.setFeatId("ft_dummy");
            rfqSpec.setFromSpecId("");
            rfqSpec.setToSpecId("");
            rfqSpec.setSpecText(TextBox_Spec.Text);
            rfqSpec.setCreatedDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            if (User.Identity.Name != null)
                rfqSpec.setCreatedUsr(User.Identity.Name);
            if (FileUpload2.HasFile)
                rfqSpec.setFileStream(FileUpload2);


            rfqProdSrvList.Add(rfqSpec);

            Session[SessionFactory.CREATE_RFQ_SELECTED_RFQ_SPEC_MAP] = rfqProdSrvList;
        }

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

                Buttin_Show_Spec_List1.Enabled = true;
                DropDownList_Locality.Enabled = false;
                DropDownList_State.Enabled = false;
                DropDownList_Country.Enabled = false;
                DropDownList_City.Enabled = false;
                TextBox_Within_Date.Enabled = false;
                TextBox_Reqr_Name.Enabled = false;
                TextBox_TnC.Enabled = false;
                TextBox_Street_Name.Enabled = false;
                FileUpload1.Enabled = false;

                Label_Extra_Spec.Visible = false;
                Label_Selected_List.Text = "";

                if (!TextBox_Spec.Text.Equals(""))
                {
                    getAddintionalProdSrvList();

                    TextBox_Spec.Text = "";
                }
                TextBox_Spec.Visible = false;
                Label_Extra_Spec_upload.Visible = false;
                FileUpload2.Visible = false;

                GridView1.Visible = false;
                DropDownList_Level1.SelectedValue ="_" ;
                DropDownList_Level2.SelectedIndex = -1;
                DropDownList_Level3.SelectedIndex = -1;

                if (FileUpload1 != null && FileUpload1.HasFile)
                    Session[SessionFactory.CREATE_RFQ_NDA_FILE] = FileUpload1;

                ArrayList prodSrvQntyList = (ArrayList)Session[SessionFactory.CREATE_RFQ_PROD_SRV_QNTY_LIST];

                if (prodSrvQntyList == null)
                    prodSrvQntyList = new ArrayList();

                BackEndObjects.RFQProdServQnty rfqpPrdSrvQntyObj = new RFQProdServQnty();
                rfqpPrdSrvQntyObj.setFromPrice(TextBox_Price_Range_From.Text);
                rfqpPrdSrvQntyObj.setToPrice(TextBox_Price_Range_To.Text);
                rfqpPrdSrvQntyObj.setMsrmntUnit(DropDownList_Unit_Of_Msrmnt.SelectedValue);
                rfqpPrdSrvQntyObj.setProdCatId(Session[SessionFactory.CREATE_RFQ_SELECTED_PRODUCT_CAT].ToString());
                rfqpPrdSrvQntyObj.setFromQnty(float.Parse(TextBox_Prod_Qnty_From.Text));
                rfqpPrdSrvQntyObj.setToQnty(float.Parse(TextBoxrod_Qnty_To.Text));

                prodSrvQntyList.Add(rfqpPrdSrvQntyObj);

                Session[SessionFactory.CREATE_RFQ_PROD_SRV_QNTY_LIST] = prodSrvQntyList;
            }
        }

        protected void createRfq()
        {
            if (DropDownList_Level1.SelectedValue.Equals("_"))
            {
                Label_Status.Text = "Please select one product category";
                Label_Status.ForeColor = System.Drawing.Color.Red;
                Label_Status.Visible = true;

                Button_Submit_Req.Enabled = true;
                Button_Submit_Next.Enabled = true;
                Button_Submit_Extra_Prd_Srv.Enabled = true;
            }
            else
            {                

                BackEndObjects.RFQDetails rfqObj = new BackEndObjects.RFQDetails();
                BackEndObjects.Id idGen = new BackEndObjects.Id();
                String rfqId = "";
                if (Session[SessionFactory.CREATE_RFQ_RFQ_ID] == null)
                {
                    rfqId = idGen.getNewId(BackEndObjects.Id.ID_TYPE_RFQ_STRING);
                    Session[SessionFactory.CREATE_RFQ_RFQ_ID] = rfqId; //store the newly created RFQ id in the session
                }
                if (!TextBox_Spec.Text.Equals(""))
                {
                    getAddintionalProdSrvList();
                    TextBox_Spec.Text = "";
                }

                ArrayList rfqSpecObjList = (ArrayList)Session[SessionFactory.CREATE_RFQ_SELECTED_RFQ_SPEC_MAP];

                //Set the RFQ id for all the  spec objects
                if (rfqSpecObjList != null)
                    for (int i = 0; i < rfqSpecObjList.Count; i++)
                        ((BackEndObjects.RFQProductServiceDetails)rfqSpecObjList[i]).setRFQId(rfqId);

                rfqObj.setRFQProdServList(rfqSpecObjList);
                rfqObj.setRFQId(rfqId);
                rfqObj.setCreatedUsr(User.Identity.Name);
                rfqObj.setActiveStat(BackEndObjects.RFQDetails.RFQ_ACTIVE_STAT_ACTIVE);
                rfqObj.setDueDate(TextBox_Within_Date.Text);
                rfqObj.setEntityId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                rfqObj.setCreatedEntity(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                rfqObj.setCurrency(DropDownList_Curr.SelectedValue);
                //     FileUpload1=(FileUpload1 == null ? (FileUpload)Session[SessionFactory.CREATE_RFQ_NDA_FILE] : FileUpload1);
                if (FileUpload1 != null && FileUpload1.HasFile)
                {
                    rfqObj.setFileStream(FileUpload1);
                    rfqObj.setNDADocPathInFileStore(rfqObj.getEntityId());

                }
                else if ((FileUpload)Session[SessionFactory.CREATE_RFQ_NDA_FILE] != null && ((FileUpload)Session[SessionFactory.CREATE_RFQ_NDA_FILE]).HasFile)
                {
                    rfqObj.setFileStream((FileUpload)Session[SessionFactory.CREATE_RFQ_NDA_FILE]);
                    rfqObj.setNDADocPathInFileStore(rfqObj.getEntityId());
                }

                String localId = (!DropDownList_Locality.SelectedValue.Equals("_") && !DropDownList_Locality.SelectedValue.Equals("") ?
    DropDownList_Locality.SelectedValue : (!DropDownList_City.SelectedValue.Equals("_") && !DropDownList_City.SelectedValue.Equals("") ?
    DropDownList_City.SelectedValue : (!DropDownList_State.SelectedValue.Equals("_") && !DropDownList_State.SelectedValue.Equals("") ?
    DropDownList_State.SelectedValue : (!DropDownList_Country.SelectedValue.Equals("_") && !DropDownList_Country.SelectedValue.Equals("") ?
    DropDownList_Country.SelectedValue : ""))));

                rfqObj.setLocalityId(localId);
                rfqObj.setSubmitDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                rfqObj.setTermsandConds(TextBox_TnC.Text);
                rfqObj.setRFQName(TextBox_Reqr_Name.Text);
                rfqObj.setCreateMode(RFQDetails.CREATION_MODE_AUTO);

                //Set the approval Status
                int rfqLevel = BackEndObjects.MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()).getRfqApprovalLevel();
                BackEndObjects.Workflow_Action actionObj = null;
                if (rfqLevel > 0)
                {
                    String reportingToUser = BackEndObjects.userDetails.
                        getUserDetailsbyIdDB(User.Identity.Name, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()).getReportsTo();
                    rfqObj.setApprovalStat(reportingToUser);

                    actionObj = new Workflow_Action();
                    actionObj.setEntId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                    actionObj.setUserId(User.Identity.Name);
                    actionObj.setActionTaken(BackEndObjects.Workflow_Action.WORKFLOW_ACTION_ACTION_TAKEN_SUBMITTED);
                    actionObj.setActionDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    actionObj.setContextId(rfqObj.getRFQId());
                    actionObj.setContextName(BackEndObjects.Workflow_Action.WORKFLOW_ACTION_CONTEXT_NAME_RFQ);
                    actionObj.setComment("");
                }
                else
                    rfqObj.setApprovalStat(RFQDetails.RFQ_APPROVAL_STAT_APPROVED);
                rfqObj.setApprovalLevel("0".ToString());

                BackEndObjects.RFQProdServQnty rfqPrdQnty = new BackEndObjects.RFQProdServQnty();
                rfqPrdQnty.setRFQId(rfqObj.getRFQId());
                if (!TextBox_Price_Range_From.Text.Equals(""))
                    rfqPrdQnty.setFromPrice(TextBox_Price_Range_From.Text);
                if (!TextBox_Price_Range_To.Text.Equals(""))
                    rfqPrdQnty.setToPrice(TextBox_Price_Range_To.Text);
                if (!TextBox_Prod_Qnty_From.Text.Equals(""))
                    rfqPrdQnty.setFromQnty(float.Parse(TextBox_Prod_Qnty_From.Text));
                if (!TextBoxrod_Qnty_To.Text.Equals(""))
                    rfqPrdQnty.setToQnty(float.Parse(TextBoxrod_Qnty_To.Text));
                rfqPrdQnty.setMsrmntUnit(DropDownList_Unit_Of_Msrmnt.SelectedValue);
                rfqPrdQnty.setProdCatId(Session[SessionFactory.CREATE_RFQ_SELECTED_PRODUCT_CAT].ToString());
                //ArrayList rfqPrdQntyList = new ArrayList();
                //rfqPrdQntyList.Add(rfqPrdQnty);

                ArrayList prodSrvQntyList = (ArrayList)Session[SessionFactory.CREATE_RFQ_PROD_SRV_QNTY_LIST];

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

                if (rfqSpecObjList != null)
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

                ActionLibrary.PurchaseActions._createRFQ cR = new ActionLibrary.PurchaseActions._createRFQ();
                try
                {
                    cR.createNewRFQ(rfqObj);
                    if (actionObj != null)
                        BackEndObjects.Workflow_Action.insertWorkflowActionObject(actionObj);

                    Label_Status.Text = "RFQ created successfully....You MUST broadcast an RFQ for vendors to see!" + (rfqLevel == 0 ? " RFQ will be auto approved as there is no approval rule set in Administration->WorkflowMgmt->RFQ" : "");
                    Label_Status.ForeColor = System.Drawing.Color.Green;
                    Label_Status.Visible = true;
                    
                    //Refresh the parent grid
                    DataTable dt = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_GRID_DATA];
                    Dictionary<String, Currency> allCurrList = (Dictionary<String, Currency>)Session[SessionFactory.CURRENCY_LIST];

                    dt.Rows.Add();
                    int i = dt.Rows.Count - 1;

                                        String docName = "";
                    if (rfqObj.getNDADocPath() != null)
                    {
                        String[] docPathList = rfqObj.getNDADocPath().
                            Split(new String[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                        if (docPathList.Length > 0)
                            docName = docPathList[docPathList.Length - 1];
                    }

                    DateUtility dU = new DateUtility();
                    
                                        dt.Rows[i]["RFQNo"] = rfqObj.getRFQId();
                    dt.Rows[i]["RFQName"] = rfqObj.getRFQName();
                    dt.Rows[i]["Submit Date"] = rfqObj.getSubmitDate();
                    dt.Rows[i]["curr"] = allCurrList.ContainsKey(rfqObj.getCurrency()) ?
                                    allCurrList[rfqObj.getCurrency()].getCurrencyName() : "";
                    dt.Rows[i]["Submit Date Ticks"] = Convert.ToDateTime(rfqObj.getSubmitDate()).Ticks;
                    dt.Rows[i]["Due Date"] = dU.getConvertedDateWoTime(rfqObj.getDueDate());
                    dt.Rows[i]["Due Date Ticks"] =!dt.Rows[i]["Due Date"].Equals("")?Convert.ToDateTime(dt.Rows[i]["Due Date"]).Ticks:0;
                    dt.Rows[i]["ApprovalStat"] = rfqObj.getApprovalStat();
                    dt.Rows[i]["Po_No"] = "N/A";                    
                    dt.Rows[i]["Hidden_Doc_Name"] = (docName == null || docName.Equals("") ? "" : docName);
                    dt.Rows[i]["ActiveStatus"] = rfqObj.getActiveStat();
                    dt.Rows[i]["Inv_No"] = "N/A";

                                        dt.DefaultView.Sort = "Submit Date Ticks" + " " + "DESC";
                    Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_GRID_DATA] = dt.DefaultView.ToTable();
                                        ScriptManager.RegisterStartupScript(this, typeof(string), "RefreshRefqGrid", "RefreshParent();", true);

                }
                            
                catch (Exception ex)
                {
                    Label_Status.Text = "RFQ creation failed";
                    Label_Status.ForeColor = System.Drawing.Color.Red;
                    Label_Status.Visible = true;
                }
                finally
                {
                    Session.Remove(SessionFactory.CREATE_RFQ_SELECTED_RFQ_SPEC_MAP);
                    Session.Remove(SessionFactory.CREATE_RFQ_RFQ_ID);
                    Session.Remove(SessionFactory.CREATE_RFQ_PROD_SRV_QNTY_LIST);
                    Session.Remove(SessionFactory.CREATE_RFQ_NDA_FILE);
                }
            }
        }
        protected void Button_Submit_Req_Next(object sender, EventArgs e)
        {
            createRfq();
            Label_Status.Focus();

            Buttin_Show_Spec_List1.Enabled = true;
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
            FileUpload2.Visible = false;

            Label_Selected_List.Text = "";
            clearAllFields("Panel2");
            clearAllFields("Panel3");
            clearAllFields("Panel_Prod_Service_Det");
            clearAllFields("Panel_Prod_Srv_Qnty");
            clearAllFields("Panel_Price_Range");
            clearAllFields("Panel_Location");
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
            Dictionary<String,ProductCategory> prodDict=BackEndObjects.ProductCategory.getAllChildCategoryDB(DropDownList_Level1.SelectedValue);
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
            Session[SessionFactory.CREATE_RFQ_SELECTED_PRODUCT_CAT] = DropDownList_Level1.SelectedValue;
   
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
            Session[SessionFactory.CREATE_RFQ_SELECTED_PRODUCT_CAT] = DropDownList_Level2.SelectedValue;
        }

        protected void DropDownList_Level3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session[SessionFactory.CREATE_RFQ_SELECTED_PRODUCT_CAT] = DropDownList_Level3.SelectedValue;
        }

        protected void DropDownList2_TextChanged(object sender, EventArgs e)
        {

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

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ArrayList rfqProdSrvList = (ArrayList)Session[SessionFactory.CREATE_RFQ_SELECTED_RFQ_SPEC_MAP];
            if(rfqProdSrvList==null)
                rfqProdSrvList=new ArrayList();

            BackEndObjects.RFQProductServiceDetails rfqSpec = new RFQProductServiceDetails();
            rfqSpec.setPrdCatId(Session[SessionFactory.CREATE_RFQ_SELECTED_PRODUCT_CAT].ToString());
            rfqSpec.setFeatId(((Label)GridView1.SelectedRow.Cells[1].FindControl("Label_Hidden")).Text);
            rfqSpec.setFromSpecId(((DropDownList)GridView1.SelectedRow.Cells[2].FindControl("DropDownList_Gridview1_From")).SelectedValue);
            rfqSpec.setToSpecId(((DropDownList)GridView1.SelectedRow.Cells[3].FindControl("DropDownList_Gridview1_To")).SelectedValue);
            //rfqSpec.setSpecText(TextBox_Spec.Text);
            rfqSpec.setCreatedDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            if(User.Identity.Name!=null)
            rfqSpec.setCreatedUsr(User.Identity.Name);
            if(((FileUpload)GridView1.SelectedRow.Cells[3].FindControl("FileUpload_Spec")).HasFile)
            rfqSpec.setFileStream((FileUpload)GridView1.SelectedRow.Cells[3].FindControl("FileUpload_Spec"));
            
            
            rfqProdSrvList.Add(rfqSpec);

            GridView1.SelectedRow.Cells[0].Enabled = false;
            GridView1.SelectedRow.Cells[3].Enabled = false;
            GridView1.SelectedRow.Cells[4].Enabled = false;
            GridView1.SelectedRow.Cells[5].Enabled = false;
            GridView1.SelectedRow.Cells[0].FindControl("Image_Selected").Visible = true;
            Label_Selected_List.Text += "," + GridView1.SelectedRow.DataItemIndex;

            Session[SessionFactory.CREATE_RFQ_SELECTED_RFQ_SPEC_MAP]=rfqProdSrvList;
            Buttin_Show_Spec_List1.Enabled = false;
        }

        protected void GridView1_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
   
        }

        protected void Calendar_Within_Date_SelectionChanged(object sender, EventArgs e)
        {
            //TextBox_Within_Date.Text = Calendar_Within_Date.SelectedDate.ToString("yyyy-MM-dd");
        }
    
    }
}