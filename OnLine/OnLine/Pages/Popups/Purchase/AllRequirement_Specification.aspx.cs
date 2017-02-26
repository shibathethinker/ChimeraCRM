using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using BackEndObjects;
using ActionLibrary;
using System.Collections;

namespace OnLine.Pages.Popups.Purchase
{
    public partial class AllRequirement_Specification : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                Session.Remove(SessionFactory.REQ_SPECIFICATION_EXISTING_PROD_LIST);
                fillGrid(null);
                CheckFeatAdditionAccess();
                LoadUnitsOfMsrmnt();
                LoadProductCat();
            }
        }

        protected void CheckFeatAdditionAccess()
        {
            Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

            if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_REQUIREMENT] &&
!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
            {
                Buttin_Show_Spec_List.Enabled = false;
                Button_Add_To_Req.Enabled = false;
            }
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
            //DataTable dt = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_SPEC_DATA_GRID];
            ArrayList reqrProdList = (ArrayList)Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_REQR_PROD_SRV];
            ArrayList reqrSpecList = (ArrayList)Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_REQR_SPEC];

            Dictionary<String, String> existingProdList = new Dictionary<String, String>();

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
                dt.Columns.Add("HiddenCatId");

                for (int i = 0; i < reqrProdList.Count; i++)
                {

                    BackEndObjects.RequirementProdServQnty reqrProdObj = (BackEndObjects.RequirementProdServQnty)reqrProdList[i];

                    dt.Rows.Add();
                    dt.Rows[i]["Hidden"] = reqrProdObj.getProdCatId();
                    existingProdList.Add(reqrProdObj.getProdCatId(), reqrProdObj.getProdCatId());

                    dt.Rows[i]["CategoryName"] = BackEndObjects.ProductCategory.getProductCategorybyIdwoFeaturesDB(reqrProdObj.getProdCatId()).getProductCategoryName();
                    //dt.Rows[i]["featureDataTable"] = dtSpec;
                    dt.Rows[i]["FromQnty"] = reqrProdObj.getFromQnty();
                    dt.Rows[i]["ToQnty"] = reqrProdObj.getToQnty();
                    dt.Rows[i]["FromPrice"] = reqrProdObj.getFromPrice();
                    dt.Rows[i]["ToPrice"] = reqrProdObj.getToPrice();
                    dt.Rows[i]["msrmntUnit"] = reqrProdObj.getMsrmntUnit();

                }
                Session[SessionFactory.REQ_SPECIFICATION_EXISTING_PROD_LIST] = existingProdList;
                Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_SPEC_DATA_GRID] = dt;
            }
            GridView1.DataSource = dt;
            GridView1.DataBind();
            GridView1.Columns[2].Visible = false;

            Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];
            bool editAccess = true;

            if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_REQUIREMENT] &&
!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
            {
                GridView1.Columns[0].Visible = false;
                GridView1.Columns[1].Visible = false;
                editAccess = false;
            }

            Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_SPEC_DATA_GRID] = dt;

            
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
                for (int j = 0; j < reqrSpecList.Count; j++)
                {
                    
                    BackEndObjects.Requirement_Spec reqrSpecObj = (BackEndObjects.Requirement_Spec)reqrSpecList[j];

                    if (reqrSpecObj.getProdCatId().Equals(((Label)gVR.Cells[1].FindControl("Label_Hidden")).Text))
                    {
                        dtSpec.Rows.Add();
                        String featName = BackEndObjects.Features.getFeaturebyIdwoSpecDB(reqrSpecObj.getFeatId()).getFeatureName();
                        dtSpec.Rows[rowCount]["Hidden"] = reqrSpecObj.getFeatId();
                        dtSpec.Rows[rowCount]["FeatName"] = (featName != null ? featName.Trim() : "");
                        dtSpec.Rows[rowCount]["SpecText"] = reqrSpecObj.getSpecText();
                        if (!reqrSpecObj.getFromSpecId().Equals(""))
                            dtSpec.Rows[rowCount]["FromSpec"] = BackEndObjects.Specifications.getSpecificationDetailbyIdDB(reqrSpecObj.getFromSpecId()).getSpecName();
                        if (!reqrSpecObj.getToSpecId().Equals(""))
                            dtSpec.Rows[rowCount]["ToSpec"] = BackEndObjects.Specifications.getSpecificationDetailbyIdDB(reqrSpecObj.getToSpecId()).getSpecName();

                        String[] imgPathList = reqrSpecObj.getImgPath()!=null?reqrSpecObj.getImgPath().Split(new String[] { "\\" }, StringSplitOptions.RemoveEmptyEntries):null;
                        dtSpec.Rows[rowCount]["imgName"] =  (imgPathList != null && imgPathList.Length > 0) ? imgPathList[imgPathList.Length - 1] : "N\\A";
                        rowCount++;
                    }
                }
                ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).DataSource = dtSpec;
                ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).DataBind();
                ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).Visible = true;
                ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).Columns[1].Visible = false;//editAccess
                ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).Columns[0].Visible = editAccess;
            }
            GridView1.Visible = true;
        }
              
        protected void GridView1_Inner_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView grdInner = ((GridView)sender);
            grdInner.PageIndex = e.NewPageIndex;
            grdInner.SelectedIndex = -1;

            GridViewRow gvRowParent = ((GridView)sender).Parent.Parent as GridViewRow;

            DataTable dtSpec = new DataTable();
            dtSpec.Columns.Add("Hidden");
            dtSpec.Columns.Add("FeatName");
            dtSpec.Columns.Add("SpecText");
            dtSpec.Columns.Add("FromSpec");
            dtSpec.Columns.Add("ToSpec");
            dtSpec.Columns.Add("imgName");

            ArrayList reqrSpecList =(ArrayList) Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_REQR_SPEC];
            int rowCount = 0;
            for (int j = 0; j < reqrSpecList.Count; j++)
            {

                BackEndObjects.Requirement_Spec reqrSpecObj = (BackEndObjects.Requirement_Spec)reqrSpecList[j];

                if (reqrSpecObj.getProdCatId().Equals(((Label)gvRowParent.Cells[0].FindControl("Label_Hidden")).Text))
                {
                    dtSpec.Rows.Add();
                    String featName = BackEndObjects.Features.getFeaturebyIdwoSpecDB(reqrSpecObj.getFeatId()).getFeatureName();
                    dtSpec.Rows[rowCount]["Hidden"] = reqrSpecObj.getFeatId();
                    dtSpec.Rows[rowCount]["FeatName"] = (featName != null ? featName.Trim() : "");
                    dtSpec.Rows[rowCount]["SpecText"] = reqrSpecObj.getSpecText();
                    if (!reqrSpecObj.getFromSpecId().Equals(""))
                        dtSpec.Rows[rowCount]["FromSpec"] = BackEndObjects.Specifications.getSpecificationDetailbyIdDB(reqrSpecObj.getFromSpecId()).getSpecName();
                    if (!reqrSpecObj.getToSpecId().Equals(""))
                        dtSpec.Rows[rowCount]["ToSpec"] = BackEndObjects.Specifications.getSpecificationDetailbyIdDB(reqrSpecObj.getToSpecId()).getSpecName();

                    String[] imgPathList = reqrSpecObj.getImgPath()!=null?reqrSpecObj.getImgPath().Split(new String[]{"\\"},StringSplitOptions.RemoveEmptyEntries):null;

                    dtSpec.Rows[rowCount]["imgName"] = (imgPathList != null && imgPathList.Length>0)?imgPathList[imgPathList.Length - 1]:"N\\A";
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
            fillGrid((DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_SPEC_DATA_GRID]);
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

            icObj.setParentContextName(ActionLibrary.ImageContextFactory.PARENT_CONTEXT_REQUIREMENT);
            icObj.setParentContextValue(Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_SELECTED_REQR_ID].ToString());
            icObj.setDestinationContextName(ActionLibrary.ImageContextFactory.DESTINATION_CONTEXT_FEAT_FOR_PARENT_REQUIREMENT);

            Dictionary<String,String> childContextDict=new Dictionary<string,string>();
            childContextDict.Add(ActionLibrary.ImageContextFactory.CHILD_CONTEXT_PRODCAT_ID,prodCatId);
            childContextDict.Add(ActionLibrary.ImageContextFactory.CHILD_CONTEXT_FEAT_ID, featId);
            icObj.setChildContextObjects(childContextDict);

            Session[SessionFactory.DISP_IMAGE_CONTEXT_FACTORY_OBJ] = icObj;
            //Server.Transfer("/Pages/DispImage.aspx", true);
            Response.Redirect("/Pages/DispImage.aspx");
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            fillGrid((DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_SPEC_DATA_GRID]);
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            fillGrid((DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_SPEC_DATA_GRID]);
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow gVR = GridView1.Rows[e.RowIndex];            
            DataTable dt = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_SPEC_DATA_GRID];

            String fromQnty=((TextBox)gVR.Cells[0].FindControl("TextBox_From_Qnty_Edit")).Text;
            String toQnty=((TextBox)gVR.Cells[0].FindControl("TextBox_To_Qnty_Edit")).Text;
            String fromPrice=((TextBox)gVR.Cells[0].FindControl("TextBox_From_Price_Edit")).Text;
            String toPrice=((TextBox)gVR.Cells[0].FindControl("TextBox_To_Price_Edit")).Text;

            dt.Rows[e.RowIndex]["FromQnty"] =fromQnty ;
            dt.Rows[e.RowIndex]["ToQnty"] = toQnty;
            dt.Rows[e.RowIndex]["FromPrice"] = fromPrice;
            dt.Rows[e.RowIndex]["ToPrice"] =toPrice ;

            Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_SPEC_DATA_GRID] = dt;
            GridView1.EditIndex = -1;
            fillGrid((DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_SPEC_DATA_GRID]);

            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            Dictionary<String, String> targetCls = new Dictionary<string, string>();

            whereCls.Add(BackEndObjects.RequirementProdServQnty.REQ_PROD_SRV_COL_PROD_SRV_ID,((Label)gVR.Cells[0].FindControl("Label_Hidden")).Text);
            whereCls.Add(BackEndObjects.RequirementProdServQnty.REQ_PROD_SRV_COL_REQ_ID, (String)Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_SELECTED_REQR_ID]);

            targetCls.Add(BackEndObjects.RequirementProdServQnty.REQ_PROD_SRV_COL_FROM_QNTY, fromQnty);
            targetCls.Add(BackEndObjects.RequirementProdServQnty.REQ_PROD_SRV_COL_FROM_PRICE, fromPrice);
            targetCls.Add(BackEndObjects.RequirementProdServQnty.REQ_PROD_SRV_COL_TO_QNTY, toQnty);
            targetCls.Add(BackEndObjects.RequirementProdServQnty.REQ_PROD_SRV_COL_TO_PRICE, toPrice);

            try
            {
                BackEndObjects.RequirementProdServQnty.updateRequirementProductServiceQuantityDB(targetCls, whereCls, DBConn.Connections.OPERATION_UPDATE);
                ArrayList reqrProdList = (ArrayList)Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_REQR_PROD_SRV];
                for(int i =0;i<reqrProdList.Count;i++)
                {
                    BackEndObjects.RequirementProdServQnty reqrProdObj = (BackEndObjects.RequirementProdServQnty)reqrProdList[i];
                    
                    if (reqrProdObj.getRequirementId().Equals(Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_SELECTED_REQR_ID].ToString()) &&
                        reqrProdObj.getProdCatId().Equals(((Label)gVR.Cells[0].FindControl("Label_Hidden")).Text))
                    {
                        reqrProdObj.setFromPrice(fromPrice);
                        reqrProdObj.setToPrice(toPrice);
                        reqrProdObj.setFromQnty(float.Parse(fromQnty));
                        reqrProdObj.setToQnty(float.Parse(toQnty));
                        reqrProdList.RemoveAt(i);
                        reqrProdList.Add(reqrProdObj);
                        break;
                    }
                }
                Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_REQR_PROD_SRV] = reqrProdList;
            }
            catch (Exception ex)
            {
            }

        }

        protected void GridView1_Inner_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView grdInner = ((GridView)sender);
            grdInner.EditIndex = e.NewEditIndex;

            GridViewRow gvRowParent = ((GridView)sender).Parent.Parent as GridViewRow;
                        loadInnerGrid(grdInner, gvRowParent);

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

        protected void loadInnerGrid(GridView gridInner,GridViewRow parentGridRow)
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

            ArrayList reqrSpecList = (ArrayList)Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_REQR_SPEC];
            int rowCount = 0;
            for (int j = 0; j < reqrSpecList.Count; j++)
            {

                BackEndObjects.Requirement_Spec reqrSpecObj = (BackEndObjects.Requirement_Spec)reqrSpecList[j];

                if (reqrSpecObj.getProdCatId().Equals(((Label)gvRowParent.Cells[0].FindControl("Label_Hidden")).Text))
                {
                    dtSpec.Rows.Add();
                    String featName = BackEndObjects.Features.getFeaturebyIdwoSpecDB(reqrSpecObj.getFeatId()).getFeatureName();
                    dtSpec.Rows[rowCount]["Hidden"] = reqrSpecObj.getFeatId();
                    dtSpec.Rows[rowCount]["FeatName"] = (featName != null ? featName.Trim() : "");
                    dtSpec.Rows[rowCount]["SpecText"] = reqrSpecObj.getSpecText();
                    if (!reqrSpecObj.getFromSpecId().Equals(""))
                        dtSpec.Rows[rowCount]["FromSpec"] = BackEndObjects.Specifications.getSpecificationDetailbyIdDB(reqrSpecObj.getFromSpecId()).getSpecName();
                    if (!reqrSpecObj.getToSpecId().Equals(""))
                        dtSpec.Rows[rowCount]["ToSpec"] = BackEndObjects.Specifications.getSpecificationDetailbyIdDB(reqrSpecObj.getToSpecId()).getSpecName();

                    String[] imgPath = reqrSpecObj.getImgPath()!=null?reqrSpecObj.getImgPath().Split(new String[] { "\\" }, StringSplitOptions.RemoveEmptyEntries):null;
                    dtSpec.Rows[rowCount]["imgName"] = (imgPath != null && imgPath.Length > 0 ? imgPath[imgPath.Length - 1] : "N\\A");
                    rowCount++;
                }
            }
            grdInner.DataSource = dtSpec;
            grdInner.DataBind();

            Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_REQR_SPEC_EDIT_INNER_GRID] = dtSpec;
        }

        protected void GridView1_Inner_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView grdInner = ((GridView)sender);
            grdInner.EditIndex = -1;

            grdInner.DataSource = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_REQR_SPEC_EDIT_INNER_GRID];
            grdInner.DataBind();
            
        }

        protected void GridView1_Inner_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DataTable dt = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_REQR_SPEC_EDIT_INNER_GRID];
            
            GridView grdInner = ((GridView)sender);
            GridViewRow grdInnerRow=grdInner.Rows[e.RowIndex];
            GridViewRow gvRowParent = ((GridView)sender).Parent.Parent as GridViewRow;

            String featId = ((Label)grdInnerRow.Cells[0].FindControl("Label_Hidden")).Text;
            String prodCatId=((Label)gvRowParent.Cells[0].FindControl("Label_Hidden")).Text;

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

            whereCls.Add(BackEndObjects.Requirement_Spec.REQ_SPEC_COL_FEAT_ID, featId);
            whereCls.Add(BackEndObjects.Requirement_Spec.REQ_SPEC_COL_PROD_ID, prodCatId);
            whereCls.Add(BackEndObjects.Requirement_Spec.REQ_SPEC_COL_REQ_ID, Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_SELECTED_REQR_ID].ToString());

            if (!((FileUpload)grdInnerRow.Cells[0].FindControl("FileUpload_Image")).HasFile)
            {
                targetVals.Add(BackEndObjects.Requirement_Spec.REQ_SPEC_COL_FROM_SPEC_ID, fromSpecId);
                targetVals.Add(BackEndObjects.Requirement_Spec.REQ_SPEC_COL_TO_SPEC_ID, ToSpecId);
                targetVals.Add(BackEndObjects.Requirement_Spec.REQ_SPEC_COL_SPEC_TXT, specText);

                BackEndObjects.Requirement_Spec.updateRequirementSpecsDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
            }
            else
            {
                
                //Remove the entry and re-create
                BackEndObjects.Requirement_Spec.updateRequirementSpecsDB(targetVals, whereCls, DBConn.Connections.OPERATION_DELETE);

                BackEndObjects.Requirement_Spec specObj = new Requirement_Spec();
                specObj.setReqId(Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_SELECTED_REQR_ID].ToString());
                specObj.setFeatId(featId);
                specObj.setFileStream(((FileUpload)grdInner.Rows[e.RowIndex].Cells[0].FindControl("FileUpload_Image")));
                specObj.setFromSpecId(fromSpecId);
                specObj.setSpecText(specText);
                specObj.setToSpecId(ToSpecId);
                specObj.setProdCatId(prodCatId);
                specObj.setImgPathInFileStore(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

                BackEndObjects.Requirement_Spec.insertRequirementSpecsDB(specObj);

                String[] imgPath=specObj.getImgPath().Split(new String[]{"\\"},StringSplitOptions.RemoveEmptyEntries);
                dt.Rows[index]["imgName"] = (imgPath != null && imgPath.Length > 0 ? imgPath[imgPath.Length - 1] : "N\\A");
                updatedImgPath = specObj.getImgPath();
            }

            Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_REQR_SPEC_EDIT_INNER_GRID] = dt;
            ArrayList reqrSpecList = (ArrayList)Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_REQR_SPEC];
            for (int j = 0; j < reqrSpecList.Count; j++)
                {
                    
                    BackEndObjects.Requirement_Spec reqrSpecObj = (BackEndObjects.Requirement_Spec)reqrSpecList[j];

                    if (reqrSpecObj.getProdCatId().Equals(prodCatId) && reqrSpecObj.getFeatId().Equals(featId))
                    {
                        reqrSpecObj.setFromSpecId(fromSpecId);
                        reqrSpecObj.setToSpecId(ToSpecId);
                        reqrSpecObj.setSpecText(specText);
                        if (!updatedImgPath.Equals(""))
                            reqrSpecObj.setImgPath(updatedImgPath);
                        break;
                    }
                }

            Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_REQR_SPEC]=reqrSpecList;

            grdInner.EditIndex = -1;
            grdInner.DataSource = dt;
            grdInner.DataBind();

        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            String catId = ((Label)GridView1.Rows[e.RowIndex].Cells[0].FindControl("Label_Hidden")).Text;
            String reqrId = Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_SELECTED_REQR_ID].ToString();

            Dictionary<String, String> existingProdList = (Dictionary<String, String>)Session[SessionFactory.REQ_SPECIFICATION_EXISTING_PROD_LIST];

            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            Dictionary<String, String> targetVals = new Dictionary<string, string>();

            whereCls.Add(BackEndObjects.Requirement_Spec.REQ_SPEC_COL_REQ_ID, reqrId);
            whereCls.Add(BackEndObjects.Requirement_Spec.REQ_SPEC_COL_PROD_ID, catId);
            try
            {
                BackEndObjects.Requirement_Spec.updateRequirementSpecsDB(targetVals, whereCls, DBConn.Connections.OPERATION_DELETE);
                BackEndObjects.RequirementProdServQnty.updateRequirementProductServiceQuantityDB(targetVals, whereCls, DBConn.Connections.OPERATION_DELETE);
                existingProdList.Remove(catId);

                DataTable dt = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_SPEC_DATA_GRID];
                int index = GridView1.Rows[e.RowIndex].DataItemIndex;
                dt.Rows[index].Delete();
                GridView1.DataSource = dt;
                GridView1.DataBind();

                ArrayList reqrProdList = (ArrayList)Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_REQR_PROD_SRV];
                for (int i = 0; i < reqrProdList.Count; i++)
                {
                    BackEndObjects.RequirementProdServQnty reqrProdQntyObj = (RequirementProdServQnty)reqrProdList[i];
                    if (reqrProdQntyObj.getProdCatId().Equals(catId) && reqrProdQntyObj.getRequirementId().Equals(reqrId))
                    {
                        reqrProdList.RemoveAt(i);
                        break;
                    }
                }

                ArrayList reqrSpecList = (ArrayList)Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_REQR_SPEC];
                ArrayList reqrSpecListDeletedRemoved = new ArrayList();
                for (int i = 0; i < reqrSpecList.Count; i++)
                {
                    BackEndObjects.Requirement_Spec reqrSpecObj = (Requirement_Spec)reqrSpecList[i];
                    
                    if (!reqrSpecObj.getProdCatId().Equals(catId))
                    {
                        reqrSpecListDeletedRemoved.Add(reqrSpecObj);
                    }
                }
                reqrSpecList = reqrSpecListDeletedRemoved;

                Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_REQR_SPEC] = reqrSpecList;
                Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_REQR_PROD_SRV] = reqrProdList;
                Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_SPEC_DATA_GRID] = dt;
                Session[SessionFactory.REQ_SPECIFICATION_EXISTING_PROD_LIST] = existingProdList;
                BindInnterGrid();
            }
            catch (Exception ex)
            {
            }

        }

        protected void BindInnterGrid()
        {
            ArrayList reqrProdList = (ArrayList)Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_REQR_PROD_SRV];
            ArrayList reqrSpecList = (ArrayList)Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_REQR_SPEC];

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
                for (int j = 0; j < reqrSpecList.Count; j++)
                {

                    BackEndObjects.Requirement_Spec reqrSpecObj = (BackEndObjects.Requirement_Spec)reqrSpecList[j];

                    if (reqrSpecObj.getProdCatId().Equals(((Label)gVR.Cells[1].FindControl("Label_Hidden")).Text))
                    {
                        dtSpec.Rows.Add();
                        String featName = BackEndObjects.Features.getFeaturebyIdwoSpecDB(reqrSpecObj.getFeatId()).getFeatureName();
                        dtSpec.Rows[rowCount]["Hidden"] = reqrSpecObj.getFeatId();
                        dtSpec.Rows[rowCount]["FeatName"] = (featName != null ? featName.Trim() : "");
                        dtSpec.Rows[rowCount]["SpecText"] = reqrSpecObj.getSpecText();
                        if (!reqrSpecObj.getFromSpecId().Equals(""))
                            dtSpec.Rows[rowCount]["FromSpec"] = BackEndObjects.Specifications.getSpecificationDetailbyIdDB(reqrSpecObj.getFromSpecId()).getSpecName();
                        if (!reqrSpecObj.getToSpecId().Equals(""))
                            dtSpec.Rows[rowCount]["ToSpec"] = BackEndObjects.Specifications.getSpecificationDetailbyIdDB(reqrSpecObj.getToSpecId()).getSpecName();

                        String[] imgPathList = reqrSpecObj.getImgPath().Split(new String[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
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
            Session[SessionFactory.UPDATE_REQR_SELECTED_PRODUCT_CAT] = DropDownList_Level1.SelectedValue;

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
            Session[SessionFactory.UPDATE_REQR_SELECTED_PRODUCT_CAT] = DropDownList_Level2.SelectedValue;
        }

        protected void DropDownList_Level3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session[SessionFactory.UPDATE_REQR_SELECTED_PRODUCT_CAT] = DropDownList_Level3.SelectedValue;
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
            if (Session[SessionFactory.UPDATE_REQR_SELECTED_PRODUCT_CAT] != null && !Session[SessionFactory.UPDATE_REQR_SELECTED_PRODUCT_CAT].Equals(""))
                selectedProdCatId = Session[SessionFactory.UPDATE_REQR_SELECTED_PRODUCT_CAT].ToString();
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
            ArrayList reqrSpecList = (ArrayList)Session[SessionFactory.UPDATE_REQ_SELECTED_REQR_SPEC_MAP];
            String fileName = null;

            if (reqrSpecList == null)
                reqrSpecList = new ArrayList();

            BackEndObjects.Requirement_Spec reqrSpec = new Requirement_Spec();
            reqrSpec.setProdCatId(Session[SessionFactory.UPDATE_REQR_SELECTED_PRODUCT_CAT].ToString());
            reqrSpec.setFeatId(((Label)GridView2.SelectedRow.Cells[1].FindControl("Label_Hidden")).Text);
            reqrSpec.setFromSpecId(((DropDownList)GridView2.SelectedRow.Cells[2].FindControl("DropDownList_GridView1_From")).SelectedValue);
            reqrSpec.setToSpecId(((DropDownList)GridView2.SelectedRow.Cells[3].FindControl("DropDownList_GridView1_To")).SelectedValue);
            //reqrSpec.setSpecText(TextBox_Spec.Text);
            reqrSpec.setCreateDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            if (User.Identity.Name != null)
                reqrSpec.setCreatedUser(User.Identity.Name);
            if (((FileUpload)GridView2.SelectedRow.Cells[3].FindControl("FileUpload_Spec")).HasFile)
            {
                reqrSpec.setFileStream((FileUpload)GridView2.SelectedRow.Cells[3].FindControl("FileUpload_Spec"));
                fileName = ((FileUpload)GridView2.SelectedRow.Cells[3].FindControl("FileUpload_Spec")).FileName;
                ((Label)GridView2.SelectedRow.Cells[0].FindControl("Label_File_Name")).Text = fileName;
                ((Label)GridView2.SelectedRow.Cells[0].FindControl("Label_File_Name")).Visible = true;
                ((FileUpload)GridView2.SelectedRow.Cells[0].FindControl("FileUpload_Spec")).Visible = false;

            }

            reqrSpecList.Add(reqrSpec);


            GridView2.SelectedRow.Cells[0].Enabled = false;
            GridView2.SelectedRow.Cells[3].Enabled = false;
            GridView2.SelectedRow.Cells[4].Enabled = false;
            GridView2.SelectedRow.Cells[5].Enabled = false;          
            

            GridView2.SelectedRow.Cells[0].FindControl("Image_Selected").Visible = true;
            Label_Selected_List.Text += "," + GridView2.SelectedRow.DataItemIndex;
            Session[SessionFactory.UPDATE_REQ_SELECTED_REQR_SPEC_MAP] = reqrSpecList;
        }

        protected void GridView2_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }

        protected void getAddintionalProdSrvList()
        {
            ArrayList reqProdSrvList = (ArrayList)Session[SessionFactory.UPDATE_REQ_SELECTED_REQR_SPEC_MAP];
            if (reqProdSrvList == null)
                reqProdSrvList = new ArrayList();

            BackEndObjects.Requirement_Spec reqSpec = new Requirement_Spec();
            reqSpec.setProdCatId(Session[SessionFactory.UPDATE_REQR_SELECTED_PRODUCT_CAT].ToString());
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

            Session[SessionFactory.UPDATE_REQ_SELECTED_REQR_SPEC_MAP] = reqProdSrvList;
        }

        protected void Button_Add_To_Req_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> existingFeatList = (Dictionary<String, String>)Session[SessionFactory.REQ_SPECIFICATION_EXISTING_PROD_LIST];
            if (DropDownList_Level1.SelectedValue.Equals("_"))
            {
                Label_Status.Text = "Please select one product category";
                Label_Status.ForeColor = System.Drawing.Color.Red;
                Label_Status.Visible = true;
            }
            else if (existingFeatList != null && existingFeatList.ContainsKey(DropDownList_Level3.SelectedValue))
            {
                Label_Feat_Exists.Visible = true;
                Label_Feat_Exists.Text = "This product category already added to this requirement. You can delete from the above list and re-enter";
                Label_Feat_Exists.ForeColor = System.Drawing.Color.Red;
                Label_Feat_Exists.Focus();
            }
            else
            {
                Label_Feat_Exists.Visible = false;

                String reqId = Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_SELECTED_REQR_ID].ToString();

                if (!TextBox_Spec.Text.Equals(""))
                {
                    getAddintionalProdSrvList();
                    TextBox_Spec.Text = "";
                }

                ArrayList reqrSpecObjList = (ArrayList)Session[SessionFactory.UPDATE_REQ_SELECTED_REQR_SPEC_MAP];

                Dictionary<String, String> reqSpecUniqnessValidation = new Dictionary<string, string>();

                if (reqrSpecObjList != null)
                    for (int i = 0; i < reqrSpecObjList.Count; i++)
                    {
                        BackEndObjects.Requirement_Spec reqrSpecObj = (BackEndObjects.Requirement_Spec)reqrSpecObjList[i];
                        reqrSpecObj.setReqId(reqId);

                        if (reqSpecUniqnessValidation.ContainsKey(reqrSpecObj.getProdCatId() + ":" + reqrSpecObj.getFeatId()))
                            reqrSpecObjList.RemoveAt(i);//Remove the current requirement spec object from the list - otherwise it will cause exception at DB layer while inserting
                        else
                        {
                            reqSpecUniqnessValidation.Add(reqrSpecObj.getProdCatId() + ":" + reqrSpecObj.getFeatId(), "");
                            if (reqrSpecObj.getFileStream() != null)
                                reqrSpecObj.setImgPathInFileStore(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                        }
                    }

                BackEndObjects.RequirementProdServQnty reqProdSrvObj = new BackEndObjects.RequirementProdServQnty();
                reqProdSrvObj.setRequirementId(reqId);
                reqProdSrvObj.setProdCatId(Session[SessionFactory.UPDATE_REQR_SELECTED_PRODUCT_CAT].ToString());
                reqProdSrvObj.setFromPrice(TextBox_Price_Range_From.Text);
                reqProdSrvObj.setFromQnty(float.Parse(TextBox_Prod_Qnty_From.Text));
                reqProdSrvObj.setMsrmntUnit(DropDownList_Unit_Of_Msrmnt.SelectedValue);
                reqProdSrvObj.setToPrice(TextBox_Price_Range_To.Text);
                reqProdSrvObj.setToQnty(float.Parse(TextBoxrod_Qnty_To.Text));

                try
                {
                    BackEndObjects.RequirementProdServQnty.insertRequirementProductServiceQuantityDetailsDB(reqProdSrvObj);
                    if (reqrSpecObjList != null && reqrSpecObjList.Count > 0)
                        BackEndObjects.Requirement_Spec.insertRequirementSpecsDB(reqrSpecObjList);

                    ArrayList reqrProdList = (ArrayList)Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_REQR_PROD_SRV];
                    ArrayList reqrSpecList = (ArrayList)Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_REQR_SPEC];
                    if (reqrProdList == null)
                        reqrProdList = new ArrayList();
                    if (reqrSpecList == null)
                        reqrSpecList = new ArrayList();

                    reqrProdList.Add(reqProdSrvObj);

                    if (reqrSpecObjList != null)
                        for (int i = 0; i < reqrSpecObjList.Count; i++)
                        {
                            BackEndObjects.Requirement_Spec specObj = (BackEndObjects.Requirement_Spec)reqrSpecObjList[i];
                            reqrSpecList.Add(specObj);
                        }
                    Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_REQR_PROD_SRV] = reqrProdList;
                    Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_REQR_SPEC] = reqrSpecList;
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
                    Session.Remove(SessionFactory.UPDATE_REQ_SELECTED_REQR_SPEC_MAP);
                    Session.Remove(SessionFactory.UPDATE_REQR_SELECTED_PRODUCT_CAT);
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
                                    Dictionary<String, String> existingFeatList = (Dictionary<String, String>)Session[SessionFactory.REQ_SPECIFICATION_EXISTING_PROD_LIST];
                                    if (existingFeatList != null && existingFeatList.ContainsKey(DropDownList_Level3.SelectedValue))
                                    {
                                        Label_Feat_Exists.Visible = true;
                                        Label_Feat_Exists.Text = "This product category already added to this requirement. You can delete from the above list and re-enter";
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
            if (Session[SessionFactory.UPDATE_REQR_SELECTED_PRODUCT_CAT] != null && !Session[SessionFactory.UPDATE_REQR_SELECTED_PRODUCT_CAT].Equals(""))
                selectedProdCatId = Session[SessionFactory.UPDATE_REQR_SELECTED_PRODUCT_CAT].ToString();
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
    }
}