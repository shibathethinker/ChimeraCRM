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
    public partial class CreateReq : System.Web.UI.Page
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
                if (defaultCurr.Equals(lt.Value.Trim()))
                    DropDownList_Curr.SelectedValue = lt.Value;
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
            if (Session[SessionFactory.CREATE_REQR_SELECTED_PRODUCT_CAT] != null && !Session[SessionFactory.CREATE_REQR_SELECTED_PRODUCT_CAT].Equals(""))
            selectedProdCatId=Session[SessionFactory.CREATE_REQR_SELECTED_PRODUCT_CAT].ToString();
            if (!selectedProdCatId.Equals(""))
            {
                Dictionary<String, Features> featDict = BackEndObjects.ProductCategory.getFeatureforCategoryDB(selectedProdCatId);

                if (featDict.Count > 0)
                {
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
                    String[] selectedList = Label_Selected_List.Text.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries);
                    Dictionary<String, String> selectedIndices = new Dictionary<String, String>();
                    for(int i=0;i<selectedList.Length;i++)
                        selectedIndices.Add(selectedList[i],selectedList[i]);

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
            GridView1.SelectedIndex = -1;
            fillGrid();
        }
        /// <summary>
        /// create the requirement details 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Button_Submit_Req_Click(object sender, EventArgs e)
        {
            Button_Submit_Req.Enabled = false;
            Button_Submit_Next.Enabled = false;
            Button_Submit_Extra_Prd_Srv.Enabled = false;
            createReq();
        }

        protected void createReq()
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
                BackEndObjects.Requirement reqObj = new BackEndObjects.Requirement();
                BackEndObjects.Id idGen = new BackEndObjects.Id();
                // String reqId = idGen.getNewId(BackEndObjects.Id.ID_TYPE_REQR_STRING);

                String reqId = "";
                if (Session[SessionFactory.CREATE_REQ_REQR_ID] == null)
                {
                    reqId = idGen.getNewId(BackEndObjects.Id.ID_TYPE_REQR_STRING);
                    Session[SessionFactory.CREATE_REQ_REQR_ID] = reqId; //store the newly created RFQ id in the session
                }
                if (!TextBox_Spec.Text.Equals(""))
                {
                    getAddintionalProdSrvList();
                    TextBox_Spec.Text = "";
                }

                ArrayList reqrSpecObjList = (ArrayList)Session[SessionFactory.CREATE_REQ_SELECTED_REQR_SPEC_MAP];

                //Set the requirement id for all the requirement spec objects
                //if(reqrSpecObjList!=null)
                //for(int i=0;i<reqrSpecObjList.Count;i++)
                //((BackEndObjects.Requirement_Spec)reqrSpecObjList[i]).setReqId(reqId);
                reqObj.setReqSpecs(reqrSpecObjList);
                //This setter method makes sure to add req id to all associated requirement spec objects if missing.
                reqObj.setReqId(reqId);
                reqObj.setCreatedUsr(User.Identity.Name);
                reqObj.setActiveStat(BackEndObjects.Requirement.REQ_ACTIVE_STAT_ACTIVE);
                reqObj.setDueDate(TextBox_Within_Date.Text);

                //Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()
                //reqObj.setEntityId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                //Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString()
                reqObj.setEntityId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

                String localId = (!DropDownList_Locality.SelectedValue.Equals("_") && !DropDownList_Locality.SelectedValue.Equals("") ?
DropDownList_Locality.SelectedValue : (!DropDownList_City.SelectedValue.Equals("_") && !DropDownList_City.SelectedValue.Equals("") ?
DropDownList_City.SelectedValue : (!DropDownList_State.SelectedValue.Equals("_") && !DropDownList_State.SelectedValue.Equals("") ?
DropDownList_State.SelectedValue : (!DropDownList_Country.SelectedValue.Equals("_") && !DropDownList_Country.SelectedValue.Equals("") ?
DropDownList_Country.SelectedValue : ""))));

                reqObj.setLocalId(localId);
                reqObj.setReqName(TextBox_Reqr_Name.Text);

                reqObj.setSubmitDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                reqObj.setReqName(TextBox_Reqr_Name.Text);
                reqObj.setCurrency(DropDownList_Curr.SelectedValue);

                Dictionary<String, String> reqSpecUniqnessValidation = new Dictionary<string, string>();

                if (reqrSpecObjList != null)
                    for (int i = 0; i < reqrSpecObjList.Count; i++)
                    {
                        BackEndObjects.Requirement_Spec reqrSpecObj = (BackEndObjects.Requirement_Spec)reqrSpecObjList[i];
                        if (reqSpecUniqnessValidation.ContainsKey(reqrSpecObj.getProdCatId() + ":" + reqrSpecObj.getFeatId()))
                            reqrSpecObjList.RemoveAt(i);//Remove the current requirement spec object from the list - otherwise it will cause exception at DB layer while inserting
                        else
                        {
                            reqSpecUniqnessValidation.Add(reqrSpecObj.getProdCatId() + ":" + reqrSpecObj.getFeatId(), "");
                            if (reqrSpecObj.getFileStream() != null)
                                reqrSpecObj.setImgPathInFileStore(reqObj.getEntityId());
                        }
                    }

                BackEndObjects.RequirementProdServQnty reqProdSrvObj = new BackEndObjects.RequirementProdServQnty();
                reqProdSrvObj.setRequirementId(reqObj.getReqId());
                reqProdSrvObj.setProdCatId(Session[SessionFactory.CREATE_REQR_SELECTED_PRODUCT_CAT].ToString());
                reqProdSrvObj.setFromPrice(TextBox_Price_Range_From.Text);
                reqProdSrvObj.setFromQnty(float.Parse(TextBox_Prod_Qnty_From.Text));
                reqProdSrvObj.setMsrmntUnit(DropDownList_Unit_Of_Msrmnt.SelectedValue);
                reqProdSrvObj.setToPrice(TextBox_Price_Range_To.Text);
                reqProdSrvObj.setToQnty(float.Parse(TextBoxrod_Qnty_To.Text));

                ArrayList prodSrvQntyList = (ArrayList)Session[SessionFactory.CREATE_REQ_PROD_SRV_QNTY_LIST];

                if (prodSrvQntyList == null)
                    prodSrvQntyList = new ArrayList();

                //Ensure that all objects in the arraylist has an associated rfqid
                for (int i = 0; i < prodSrvQntyList.Count; i++)
                {
                    BackEndObjects.RequirementProdServQnty temp = (RequirementProdServQnty)prodSrvQntyList[i];
                    if (temp.getRequirementId() == null || temp.getRequirementId().Equals(""))
                        temp.setRequirementId(reqObj.getReqId());
                }

                prodSrvQntyList.Add(reqProdSrvObj);
                reqObj.setReqProdSrvQnty(prodSrvQntyList);

                ActionLibrary.PurchaseActions._createRequirements cR = new ActionLibrary.PurchaseActions._createRequirements();
                try
                {
                    cR.createNewRequirement(reqObj);
                    Label_Status.Text = "Requirement created successfully";
                    Label_Status.ForeColor = System.Drawing.Color.Green;
                    Label_Status.Visible = true;
                    //Refresh the parent grid
                    DataTable dt = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_GRID_DATA];
                    Dictionary<String, Currency> allCurrList = (Dictionary<String, Currency>)Session[SessionFactory.CURRENCY_LIST];

                        dt.Rows.Add();
                        int i = dt.Rows.Count - 1;
                        DateUtility dU = new DateUtility();

                        dt.Rows[i]["Hidden"] = reqObj.getReqId();
                        dt.Rows[i]["Requirement Name"] = reqObj.getReqName();
                        dt.Rows[i]["curr"] = allCurrList.ContainsKey(reqObj.getCurrency()) ?
                                    allCurrList[reqObj.getCurrency()].getCurrencyName() : "";
                        dt.Rows[i]["Submit Date"] = reqObj.getSubmitDate();
                        dt.Rows[i]["Submit Date Ticks"] = Convert.ToDateTime(reqObj.getSubmitDate()).Ticks;
                        dt.Rows[i]["Due Date"] = dU.getConvertedDateWoTime(reqObj.getDueDate());
                        dt.Rows[i]["Due Date Ticks"] = reqObj.getDueDate()!=null && !reqObj.getDueDate().Equals("")?
                            Convert.ToDateTime(dt.Rows[i]["Due Date"]).Ticks:0;
                        dt.Rows[i]["Created By"] = reqObj.getCratedUsr();
                        dt.Rows[i]["Active?"] = reqObj.getActiveStat();

                        dt.DefaultView.Sort = "Submit Date Ticks" + " " + "DESC";
                        Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_GRID_DATA] = dt.DefaultView.ToTable();

                    ScriptManager.RegisterStartupScript(this, typeof(string), "RefreshReqGrid", "RefreshParent();", true);
                }
                catch (Exception ex)
                {
                    Label_Status.Text = "Requirement creation failed";
                    Label_Status.ForeColor = System.Drawing.Color.Red;
                    Label_Status.Visible = true;
                }
                finally
                {
                    Session.Remove(SessionFactory.CREATE_REQ_SELECTED_REQR_SPEC_MAP);
                    Session.Remove(SessionFactory.CREATE_REQ_REQR_ID);
                    Session.Remove(SessionFactory.CREATE_RFQ_PROD_SRV_QNTY_LIST);

                }
            }
        }
        protected void Button_Submit_Req_Next(object sender, EventArgs e)
        {
            createReq();
            Session.Remove(SessionFactory.CREATE_REQ_SELECTED_REQR_SPEC_MAP);
            
            Label_Selected_List.Text = "";
            Buttin_Show_Spec_List1.Enabled = true;
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
            Session[SessionFactory.CREATE_REQR_SELECTED_PRODUCT_CAT] = DropDownList_Level1.SelectedValue;
   
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
            Session[SessionFactory.CREATE_REQR_SELECTED_PRODUCT_CAT] = DropDownList_Level2.SelectedValue;
        }

        protected void DropDownList_Level3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session[SessionFactory.CREATE_REQR_SELECTED_PRODUCT_CAT] = DropDownList_Level3.SelectedValue;
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

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ArrayList reqrSpecList = (ArrayList)Session[SessionFactory.CREATE_REQ_SELECTED_REQR_SPEC_MAP];
            if(reqrSpecList==null)
                reqrSpecList=new ArrayList();

            BackEndObjects.Requirement_Spec reqrSpec = new Requirement_Spec();
            reqrSpec.setProdCatId(Session[SessionFactory.CREATE_REQR_SELECTED_PRODUCT_CAT].ToString());
            reqrSpec.setFeatId(((Label)GridView1.SelectedRow.Cells[1].FindControl("Label_Hidden")).Text);
            reqrSpec.setFromSpecId(((DropDownList)GridView1.SelectedRow.Cells[2].FindControl("DropDownList_Gridview1_From")).SelectedValue);
            reqrSpec.setToSpecId(((DropDownList)GridView1.SelectedRow.Cells[3].FindControl("DropDownList_Gridview1_To")).SelectedValue);
            //reqrSpec.setSpecText(TextBox_Spec.Text);
            reqrSpec.setCreateDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            if(User.Identity.Name!=null)
            reqrSpec.setCreatedUser(User.Identity.Name);
            if(((FileUpload)GridView1.SelectedRow.Cells[3].FindControl("FileUpload_Spec")).HasFile)
            reqrSpec.setFileStream((FileUpload)GridView1.SelectedRow.Cells[3].FindControl("FileUpload_Spec"));
            
            
            reqrSpecList.Add(reqrSpec);

            GridView1.SelectedRow.Cells[0].Enabled = false;
            GridView1.SelectedRow.Cells[3].Enabled = false;
            GridView1.SelectedRow.Cells[4].Enabled = false;
            GridView1.SelectedRow.Cells[5].Enabled = false;
            GridView1.SelectedRow.Cells[0].FindControl("Image_Selected").Visible = true;
            Label_Selected_List.Text += ","+GridView1.SelectedRow.DataItemIndex;
            Session[SessionFactory.CREATE_REQ_SELECTED_REQR_SPEC_MAP]=reqrSpecList;
            Buttin_Show_Spec_List1.Enabled = false;
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
            ArrayList reqProdSrvList = (ArrayList)Session[SessionFactory.CREATE_REQ_SELECTED_REQR_SPEC_MAP];
            if (reqProdSrvList == null)
                reqProdSrvList = new ArrayList();

            BackEndObjects.Requirement_Spec reqSpec = new Requirement_Spec();
            reqSpec.setProdCatId(Session[SessionFactory.CREATE_REQR_SELECTED_PRODUCT_CAT].ToString());
            reqSpec.setFeatId("ft_dummy");
            reqSpec.setFromSpecId("");
            reqSpec.setToSpecId("");
            reqSpec.setSpecText(TextBox_Spec.Text);
            reqSpec.setCreateDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            if (User.Identity.Name != null)
                reqSpec.setCreatedUser(User.Identity.Name);
            if (FileUpload_Extra_Spec.HasFile)
                reqSpec.setFileStream(FileUpload_Extra_Spec);


            reqProdSrvList.Add(reqSpec);

            Session[SessionFactory.CREATE_REQ_SELECTED_REQR_SPEC_MAP] = reqProdSrvList;
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
                Label_Selected_List.Text = "";
                DropDownList_Locality.Enabled = false;
                DropDownList_State.Enabled = false;
                DropDownList_Country.Enabled = false;
                DropDownList_City.Enabled = false;
                TextBox_Within_Date.Enabled = false;
                TextBox_Reqr_Name.Enabled = false;
                TextBox_Street_Name.Enabled = false;

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

                //Session[SessionFactory.CREATE_RFQ_NDA_FILE] = FileUpload1;

                ArrayList prodSrvQntyList = (ArrayList)Session[SessionFactory.CREATE_REQ_PROD_SRV_QNTY_LIST];

                if (prodSrvQntyList == null)
                    prodSrvQntyList = new ArrayList();

                BackEndObjects.RequirementProdServQnty reqpPrdSrvQntyObj = new RequirementProdServQnty();
                reqpPrdSrvQntyObj.setFromPrice(TextBox_Price_Range_From.Text);
                reqpPrdSrvQntyObj.setToPrice(TextBox_Price_Range_To.Text);
                reqpPrdSrvQntyObj.setMsrmntUnit(DropDownList_Unit_Of_Msrmnt.SelectedValue);
                reqpPrdSrvQntyObj.setProdCatId(Session[SessionFactory.CREATE_REQR_SELECTED_PRODUCT_CAT].ToString());
                reqpPrdSrvQntyObj.setFromQnty(float.Parse(TextBox_Prod_Qnty_From.Text));
                reqpPrdSrvQntyObj.setToQnty(float.Parse(TextBoxrod_Qnty_To.Text));

                prodSrvQntyList.Add(reqpPrdSrvQntyObj);

                Session[SessionFactory.CREATE_REQ_PROD_SRV_QNTY_LIST] = prodSrvQntyList;
            }
        }
    
    }
}