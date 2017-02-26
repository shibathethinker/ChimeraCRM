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
    public partial class AllRFQ_Specification : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Session.Remove(SessionFactory.RFQ_SPECIFICATION_EXISTING_PROD_LIST);
                fillGrid(null);                
                //CheckFeatAdditionAccess();
                LoadUnitsOfMsrmnt();
                LoadProductCat();
            }
        }

        protected void CheckFeatAdditionAccess()
        {
            /*Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

            if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_REQUIREMENT] &&
!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
            {
                Buttin_Show_Spec_List.Enabled = false;
                Button_Add_To_Req.Enabled = false;
            }*/
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
            ArrayList rfqProdList = (ArrayList)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_PROD_QNTY];
            ArrayList rfqSpecList = (ArrayList)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_RFQ_SPEC];

            Dictionary<String, String> existingProdList = new Dictionary<String, String>();

            String approvalStat = Request.QueryString.GetValues("approvalStat") != null ? Request.QueryString.GetValues("approvalStat")[0] : "";
            bool sentForApproval = approvalStat.Equals("") ? false :
                (approvalStat.Equals(RFQDetails.RFQ_APPROVAL_STAT_APPROVED) ? false : (approvalStat.Equals(User.Identity.Name,StringComparison.InvariantCultureIgnoreCase) ? false : true));

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

                }
                Session[SessionFactory.RFQ_SPECIFICATION_EXISTING_PROD_LIST] = existingProdList;
            }
                       

            GridView1.DataSource = dt;
            GridView1.DataBind();
            GridView1.Columns[2].Visible = false;

            Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

            //Disable edit button if the context is for approval

            bool approvalContext = false;
            if(Request.QueryString.GetValues("approvalContext")!=null)
                approvalContext=Request.QueryString.GetValues("approvalContext")[0].Equals("Y") ? true : false;
            if (approvalContext)
            {
                Label_Status.Visible = true;
                Label_Status.Text = "To Edit this specification, please check the RFQ from Purchase->RFQ screen";
                Label_Status.ForeColor = System.Drawing.Color.Red;
            }

            bool editAccess = true;
            if ((!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_RFQ] &&
!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS]) || approvalContext||sentForApproval)
            {
                GridView1.Columns[0].Visible = false;
                GridView1.Columns[1].Visible = false;
                editAccess = false;
                Button_Add_To_Req.Enabled = false;
                Buttin_Show_Spec_List.Enabled=false;
            }
            Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SPECIFICATION_GRID_DATA] = dt;
            

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

                        String[] imgPath = rfqSpecObj.getImgPath()!=null? rfqSpecObj.getImgPath().Split(new String[] { "\\" }, StringSplitOptions.RemoveEmptyEntries):null;
                        dtSpec.Rows[rowCount]["imgName"] = (imgPath != null && imgPath.Length > 0 ? imgPath[imgPath.Length - 1] : "N\\A");
                        rowCount++;
                    }
                }
                ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).DataSource = dtSpec;
                ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).DataBind();
                ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).Visible = true;
                ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).Columns[1].Visible = false;
                ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).Columns[0].Visible = editAccess && !sentForApproval;
            }
            GridView1.Visible = true;
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

            ArrayList rfqSpecList = (ArrayList)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_RFQ_SPEC];
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
            fillGrid((DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SPECIFICATION_GRID_DATA]);
        }

        protected void RFQ_Link_Feat_Img_Show_Command(object sender, CommandEventArgs e)
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
            icObj.setParentContextValue(Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SELECTED_RFQ_ID].ToString());

            Dictionary<String, String> childContextDict = new Dictionary<string, string>();
            childContextDict.Add(ActionLibrary.ImageContextFactory.CHILD_CONTEXT_PRODCAT_ID, prodCatId);
            childContextDict.Add(ActionLibrary.ImageContextFactory.CHILD_CONTEXT_FEAT_ID, featId);
            icObj.setChildContextObjects(childContextDict);
            icObj.setDestinationContextName(ActionLibrary.ImageContextFactory.DESTINATION_CONTEXT_FEAT_FOR_PARENT_RFQ);
            Session[SessionFactory.DISP_IMAGE_CONTEXT_FACTORY_OBJ] = icObj;
            //Server.Transfer("/Pages/DispImage.aspx", true);
            Response.Redirect("/Pages/DispImage.aspx");
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {            
            GridView1.EditIndex = e.NewEditIndex;
            fillGrid((DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SPECIFICATION_GRID_DATA]);
                       //fillGrid((DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SPECIFICATION_GRID_DATA]);            
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow gVR = GridView1.Rows[e.RowIndex];
            
            String prodCatId = ((Label)gVR.Cells[1].FindControl("Label_Hidden")).Text;
            String rfqId=Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SELECTED_RFQ_ID].ToString();
            

            BackEndObjects.RFQProdServQnty rfqProdQntyObj = BackEndObjects.RFQProdServQnty.
                getRFQProductServiceQuantityforRFIdandCatIdDB(rfqId, prodCatId);

            Dictionary<String, String> whereCls = new Dictionary<String, String>();
            Dictionary<String, String> targetVals = new Dictionary<String, String>();

            whereCls.Add(BackEndObjects.RFQProdServQnty.RFQ_PROD_SRV_COL_RFQ_ID, rfqId);
            whereCls.Add(BackEndObjects.RFQProdServQnty.RFQ_PROD_SRV_COL_PROD_SRV_ID, prodCatId);

            String fromPrice = ((TextBox)gVR.Cells[6].FindControl("TextBox_From_Price")).Text;
            String toPrice = ((TextBox)gVR.Cells[7].FindControl("TextBox_To_Price")).Text;
            String fromQnty = ((TextBox)gVR.Cells[4].FindControl("TextBox_From_Qnty")).Text;
            String toQnty = ((TextBox)gVR.Cells[5].FindControl("TextBox_To_Qnty")).Text;

            targetVals.Add(BackEndObjects.RFQProdServQnty.RFQ_PROD_SRV_COL_FROM_PRICE,fromPrice);
            targetVals.Add(BackEndObjects.RFQProdServQnty.RFQ_PROD_SRV_COL_TO_PRICE,toPrice);
            targetVals.Add(BackEndObjects.RFQProdServQnty.RFQ_PROD_SRV_COL_FROM_QNTY,fromQnty);
            targetVals.Add(BackEndObjects.RFQProdServQnty.RFQ_PROD_SRV_COL_TO_QNTY,toQnty);

            BackEndObjects.RFQProdServQnty.updateRFQProductServiceQuantityDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);

            DataTable dt = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SPECIFICATION_GRID_DATA];
            
            dt.Rows[e.RowIndex]["FromPrice"] = fromPrice;
            dt.Rows[e.RowIndex]["ToPrice"] = toPrice;
            dt.Rows[e.RowIndex]["FromQnty"] = fromQnty;
            dt.Rows[e.RowIndex]["ToQnty"] = toQnty;

            Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SPECIFICATION_GRID_DATA] = dt;
            GridView1.EditIndex = -1;
            fillGrid(dt);

            ArrayList rfqProdList = (ArrayList)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_PROD_QNTY];
            for (int i = 0; i < rfqProdList.Count; i++)
            {

                BackEndObjects.RFQProdServQnty rfqProdObj = (BackEndObjects.RFQProdServQnty)rfqProdList[i];
                if(rfqProdObj.getProdCatId().Equals(rfqProdObj.getProdCatId()))
                {
                    rfqProdObj.setFromPrice(fromPrice);
                    rfqProdObj.setFromQnty(float.Parse(fromQnty));
                    rfqProdObj.setToPrice(toPrice);
                    rfqProdObj.setToQnty(float.Parse(toQnty));

                    rfqProdList.RemoveAt(i);
                    rfqProdList.Add(rfqProdObj);
                    Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_PROD_QNTY] = rfqProdList;
                    break;
                }
            }
            
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            fillGrid((DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SPECIFICATION_GRID_DATA]);
            
        }

        protected void loadInnerGrid(GridView gridInner, GridViewRow parentGridRow)
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

            ArrayList rfqSpecList = (ArrayList)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_RFQ_SPEC];
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

                    String[] imgPath = rfqSpecObj.getImgPath().Split(new String[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                    dtSpec.Rows[rowCount]["imgName"] = (imgPath != null && imgPath.Length > 0 ? imgPath[imgPath.Length - 1] : "N\\A");
                    rowCount++;
                }
            }
            grdInner.DataSource = dtSpec;
            grdInner.DataBind();

            Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_RFQ_SPEC_EDIT_INNER_GRID] = dtSpec;
        }

        protected void GridView1_Inner_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView grdInner = ((GridView)sender);
            grdInner.EditIndex = e.NewEditIndex;

            GridViewRow gvRowParent = ((GridView)sender).Parent.Parent as GridViewRow;
            loadInnerGrid(grdInner, gvRowParent);
        }

        protected void GridView1_Inner_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DataTable dt = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_RFQ_SPEC_EDIT_INNER_GRID];

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
                Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SELECTED_RFQ_ID].ToString());

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
                specObj.setRFQId(Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SELECTED_RFQ_ID].ToString());
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

            ArrayList rfqSpecList = (ArrayList)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_RFQ_SPEC];
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

            Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_RFQ_SPEC_EDIT_INNER_GRID] = dt;
            Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_RFQ_SPEC] = rfqSpecList;

            grdInner.EditIndex = -1;
            grdInner.DataSource = dt;
            grdInner.DataBind();
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

        protected void GridView1_Inner_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView grdInner = ((GridView)sender);
            grdInner.EditIndex = -1;

            grdInner.DataSource = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_RFQ_SPEC_EDIT_INNER_GRID];
            grdInner.DataBind();
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
            if (Session[SessionFactory.UPDATE_RFQ_SELECTED_PRODUCT_CAT] != null && !Session[SessionFactory.UPDATE_RFQ_SELECTED_PRODUCT_CAT].Equals(""))
                selectedProdCatId = Session[SessionFactory.UPDATE_RFQ_SELECTED_PRODUCT_CAT].ToString();
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
                ArrayList RFQSpecList = (ArrayList)Session[SessionFactory.UPDATE_RFQ_SELECTED_RFQ_SPEC_MAP];
                String fileName = null;

                if (RFQSpecList == null)
                    RFQSpecList = new ArrayList();

                BackEndObjects.RFQProductServiceDetails RFQSpec = new RFQProductServiceDetails();
                RFQSpec.setPrdCatId(Session[SessionFactory.UPDATE_RFQ_SELECTED_PRODUCT_CAT].ToString());
                RFQSpec.setFeatId(((Label)GridView2.SelectedRow.Cells[1].FindControl("Label_Hidden")).Text);
                RFQSpec.setFromSpecId(((DropDownList)GridView2.SelectedRow.Cells[2].FindControl("DropDownList_GridView1_From")).SelectedValue);
                RFQSpec.setToSpecId(((DropDownList)GridView2.SelectedRow.Cells[3].FindControl("DropDownList_GridView1_To")).SelectedValue);
                //RFQSpec.setSpecText(TextBox_Spec.Text);
                RFQSpec.setCreatedDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                if (User.Identity.Name != null)
                    RFQSpec.setCreatedUsr(User.Identity.Name);
                if (((FileUpload)GridView2.SelectedRow.Cells[3].FindControl("FileUpload_Spec")).HasFile)
                {
                    RFQSpec.setFileStream((FileUpload)GridView2.SelectedRow.Cells[3].FindControl("FileUpload_Spec"));
                    fileName = ((FileUpload)GridView2.SelectedRow.Cells[3].FindControl("FileUpload_Spec")).FileName;
                    ((Label)GridView2.SelectedRow.Cells[0].FindControl("Label_File_Name")).Text = fileName;
                    ((Label)GridView2.SelectedRow.Cells[0].FindControl("Label_File_Name")).Visible = true;
                    ((FileUpload)GridView2.SelectedRow.Cells[0].FindControl("FileUpload_Spec")).Visible = false;

                }

                RFQSpecList.Add(RFQSpec);


                GridView2.SelectedRow.Cells[0].Enabled = false;
                GridView2.SelectedRow.Cells[3].Enabled = false;
                GridView2.SelectedRow.Cells[4].Enabled = false;
                GridView2.SelectedRow.Cells[5].Enabled = false;


                GridView2.SelectedRow.Cells[0].FindControl("Image_Selected").Visible = true;
                Label_Selected_List.Text += "," + GridView2.SelectedRow.DataItemIndex;
                Session[SessionFactory.UPDATE_RFQ_SELECTED_RFQ_SPEC_MAP] = RFQSpecList;
            
        }

        protected void GridView2_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }

        protected void getAddintionalProdSrvList()
        {
            ArrayList RFQProdSrvList = (ArrayList)Session[SessionFactory.UPDATE_RFQ_SELECTED_RFQ_SPEC_MAP];
            if (RFQProdSrvList == null)
                RFQProdSrvList = new ArrayList();

            BackEndObjects.RFQProductServiceDetails RFQSpec = new RFQProductServiceDetails();
            RFQSpec.setPrdCatId(Session[SessionFactory.UPDATE_RFQ_SELECTED_PRODUCT_CAT].ToString());
            RFQSpec.setFeatId("ft_dummy");
            RFQSpec.setFromSpecId("");
            RFQSpec.setToSpecId("");
            RFQSpec.setSpecText(TextBox_Spec.Text);
            RFQSpec.setCreatedDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            if (User.Identity.Name != null)
                RFQSpec.setCreatedUsr(User.Identity.Name);
            if (FileUpload_Extra_Spec.HasFile)
                RFQSpec.setFileStream(FileUpload_Extra_Spec);


            RFQProdSrvList.Add(RFQSpec);

            Session[SessionFactory.UPDATE_RFQ_SELECTED_RFQ_SPEC_MAP] = RFQProdSrvList;
        }

        protected void Button_Add_To_RFQ_Click(object sender, EventArgs e)
        {
            Dictionary<String, String> existingFeatList = (Dictionary<String, String>)Session[SessionFactory.RFQ_SPECIFICATION_EXISTING_PROD_LIST];
            if (DropDownList_Level1.SelectedValue.Equals("_"))
            {
                Label_Status.Text = "Please select one product category";
                Label_Status.ForeColor = System.Drawing.Color.Red;
                Label_Status.Visible = true;
            }
            else if (existingFeatList != null && existingFeatList.ContainsKey(DropDownList_Level3.SelectedValue))
            {
                Label_Feat_Exists.Visible = true;
                Label_Feat_Exists.Text = "This product category already added to this RFQ. You can delete from the above list and re-enter";
                Label_Feat_Exists.ForeColor = System.Drawing.Color.Red;
                Label_Feat_Exists.Focus();
            }
            else
            {
                Label_Feat_Exists.Visible = false;
                String RFQId = Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SELECTED_RFQ_ID].ToString();

                if (!TextBox_Spec.Text.Equals(""))
                {
                    getAddintionalProdSrvList();
                    TextBox_Spec.Text = "";
                }

                ArrayList RFQSpecObjList = (ArrayList)Session[SessionFactory.UPDATE_RFQ_SELECTED_RFQ_SPEC_MAP];

                Dictionary<String, String> RFQSpecUniqnessValidation = new Dictionary<string, string>();

                if (RFQSpecObjList != null)
                    for (int i = 0; i < RFQSpecObjList.Count; i++)
                    {
                        BackEndObjects.RFQProductServiceDetails RFQSpecObj = (BackEndObjects.RFQProductServiceDetails)RFQSpecObjList[i];
                        RFQSpecObj.setRFQId(RFQId);

                        if (RFQSpecUniqnessValidation.ContainsKey(RFQSpecObj.getPrdCatId() + ":" + RFQSpecObj.getFeatId()))
                            RFQSpecObjList.RemoveAt(i);//Remove the current RFQuirement spec object from the list - otherwise it will cause exception at DB layer while inserting
                        else
                        {
                            RFQSpecUniqnessValidation.Add(RFQSpecObj.getPrdCatId() + ":" + RFQSpecObj.getFeatId(), "");
                            if (RFQSpecObj.getFileStream() != null)
                                RFQSpecObj.setImgPathInFileStore(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                        }
                    }

                BackEndObjects.RFQProdServQnty RFQProdSrvObj = new BackEndObjects.RFQProdServQnty();
                RFQProdSrvObj.setRFQId(RFQId);
                RFQProdSrvObj.setProdCatId(Session[SessionFactory.UPDATE_RFQ_SELECTED_PRODUCT_CAT].ToString());
                RFQProdSrvObj.setFromPrice(TextBox_Price_Range_From.Text);
                RFQProdSrvObj.setFromQnty(float.Parse(TextBox_Prod_Qnty_From.Text));
                RFQProdSrvObj.setMsrmntUnit(DropDownList_Unit_Of_Msrmnt.SelectedValue);
                RFQProdSrvObj.setToPrice(TextBox_Price_Range_To.Text);
                RFQProdSrvObj.setToQnty(float.Parse(TextBoxrod_Qnty_To.Text));

                try
                {
                    BackEndObjects.RFQProdServQnty.insertRFQProductServiceQuantityDetailsDB(RFQProdSrvObj);
                    if (RFQSpecObjList != null && RFQSpecObjList.Count > 0)
                        BackEndObjects.RFQProductServiceDetails.insertRFQProductServiceDetails(RFQSpecObjList);

                    ArrayList RFQProdList = (ArrayList)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_PROD_QNTY];
                    ArrayList RFQSpecList = (ArrayList)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_RFQ_SPEC];
                    if (RFQProdList == null)
                        RFQProdList = new ArrayList();
                    if (RFQSpecList == null)
                        RFQSpecList = new ArrayList();

                    RFQProdList.Add(RFQProdSrvObj);

                    if (RFQSpecObjList != null)
                        for (int i = 0; i < RFQSpecObjList.Count; i++)
                        {
                            BackEndObjects.RFQProductServiceDetails specObj = (BackEndObjects.RFQProductServiceDetails)RFQSpecObjList[i];
                            RFQSpecList.Add(specObj);
                        }
                    Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_PROD_QNTY] = RFQProdList;
                    Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_RFQ_SPEC] = RFQSpecList;
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
                    Session.Remove(SessionFactory.UPDATE_RFQ_SELECTED_RFQ_SPEC_MAP);
                    Session.Remove(SessionFactory.UPDATE_RFQ_SELECTED_PRODUCT_CAT);
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
                        Dictionary<String, String> existingFeatList = (Dictionary<String, String>)Session[SessionFactory.RFQ_SPECIFICATION_EXISTING_PROD_LIST];
                        if (existingFeatList != null && existingFeatList.ContainsKey(DropDownList_Level3.SelectedValue))
                        {
                            Label_Feat_Exists.Visible = true;
                            Label_Feat_Exists.Text = "This product category already added to this RFQ. You can delete from the above list and re-enter";
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
            if (Session[SessionFactory.UPDATE_RFQ_SELECTED_PRODUCT_CAT] != null && !Session[SessionFactory.UPDATE_RFQ_SELECTED_PRODUCT_CAT].Equals(""))
                selectedProdCatId = Session[SessionFactory.UPDATE_RFQ_SELECTED_PRODUCT_CAT].ToString();
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
            Session[SessionFactory.UPDATE_RFQ_SELECTED_PRODUCT_CAT] = DropDownList_Level1.SelectedValue;

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
            Session[SessionFactory.UPDATE_RFQ_SELECTED_PRODUCT_CAT] = DropDownList_Level2.SelectedValue;
        }

        protected void DropDownList_Level3_SelectedIndexChanged(object sender, EventArgs e)
        {
                             Session[SessionFactory.UPDATE_RFQ_SELECTED_PRODUCT_CAT] = DropDownList_Level3.SelectedValue;           
        }

        protected void BindInnterGrid()
        {
            ArrayList RFQProdList = (ArrayList)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_PROD_QNTY];
            ArrayList RFQSpecList = (ArrayList)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_RFQ_SPEC];

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
                for (int j = 0; j < RFQSpecList.Count; j++)
                {

                    BackEndObjects.RFQProductServiceDetails RFQSpecObj = (BackEndObjects.RFQProductServiceDetails)RFQSpecList[j];

                    if (RFQSpecObj.getPrdCatId().Equals(((Label)gVR.Cells[1].FindControl("Label_Hidden")).Text))
                    {
                        dtSpec.Rows.Add();
                        String featName = BackEndObjects.Features.getFeaturebyIdwoSpecDB(RFQSpecObj.getFeatId()).getFeatureName();
                        dtSpec.Rows[rowCount]["Hidden"] = RFQSpecObj.getFeatId();
                        dtSpec.Rows[rowCount]["FeatName"] = (featName != null ? featName.Trim() : "");
                        dtSpec.Rows[rowCount]["SpecText"] = RFQSpecObj.getSpecText();
                        if (!RFQSpecObj.getFromSpecId().Equals(""))
                            dtSpec.Rows[rowCount]["FromSpec"] = BackEndObjects.Specifications.getSpecificationDetailbyIdDB(RFQSpecObj.getFromSpecId()).getSpecName();
                        if (!RFQSpecObj.getToSpecId().Equals(""))
                            dtSpec.Rows[rowCount]["ToSpec"] = BackEndObjects.Specifications.getSpecificationDetailbyIdDB(RFQSpecObj.getToSpecId()).getSpecName();

                        String[] imgPathList = RFQSpecObj.getImgPath().Split(new String[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
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
            String RFQId = Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SELECTED_RFQ_ID].ToString();

            Dictionary<String, String> existingProdList = (Dictionary<String, String>)Session[SessionFactory.RFQ_SPECIFICATION_EXISTING_PROD_LIST];
            
            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            Dictionary<String, String> targetVals = new Dictionary<string, string>();

            whereCls.Add(BackEndObjects.RFQProductServiceDetails.RFQ_PROD_COL_RFQ_ID, RFQId);
            whereCls.Add(BackEndObjects.RFQProductServiceDetails.RFQ_PROD_COL_PROD_ID, catId);
            try
            {
                BackEndObjects.RFQProductServiceDetails.updateRFQProductServiceDetails(targetVals, whereCls, DBConn.Connections.OPERATION_DELETE);
                BackEndObjects.RFQProdServQnty.updateRFQProductServiceQuantityDB(targetVals, whereCls, DBConn.Connections.OPERATION_DELETE);

                existingProdList.Remove(catId);

                DataTable dt = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SPECIFICATION_GRID_DATA];
                int index = GridView1.Rows[e.RowIndex].DataItemIndex;
                dt.Rows[index].Delete();
                GridView1.DataSource = dt;
                GridView1.DataBind();

                ArrayList RFQProdList = (ArrayList)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_PROD_QNTY];
                for (int i = 0; i < RFQProdList.Count; i++)
                {
                    BackEndObjects.RFQProdServQnty RFQProdQntyObj = (RFQProdServQnty)RFQProdList[i];
                    if (RFQProdQntyObj.getProdCatId().Equals(catId) && RFQProdQntyObj.getRFQId().Equals(RFQId))
                    {
                        RFQProdList.RemoveAt(i);
                        break;
                    }
                }

                ArrayList RFQSpecList = (ArrayList)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_RFQ_SPEC];
                ArrayList RFQSpecListDeletedRemoved = new ArrayList();
                for (int i = 0; i < RFQSpecList.Count; i++)
                {
                    BackEndObjects.RFQProductServiceDetails RFQSpecObj = (RFQProductServiceDetails)RFQSpecList[i];

                    if (!RFQSpecObj.getPrdCatId().Equals(catId))
                    {
                        RFQSpecListDeletedRemoved.Add(RFQSpecObj);
                    }
                }
                RFQSpecList = RFQSpecListDeletedRemoved;

                Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_RFQ_SPEC] = RFQSpecList;
                Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_PROD_QNTY] = RFQProdList;
                Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SPECIFICATION_GRID_DATA] = dt;
                Session[SessionFactory.RFQ_SPECIFICATION_EXISTING_PROD_LIST] = existingProdList;

                BindInnterGrid();
            }
            catch (Exception ex)
            {
            }
        }
    }
}