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

namespace OnLine.Pages.Popups.Product
{
    public partial class AllProd_Specification : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                fillFirstGrid();
                Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];
                if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_PRODUCT] &&
                    !accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                {
                    GridView1.Columns[0].Visible = false;
                    GridView1.Columns[1].Visible = false;
                    Buttin_Show_Spec_List.Enabled = false;
                    Button_Add_To_Prod.Enabled = false;
                }

            }
        }

        protected void fillFirstGrid()
        {
            String entId = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();
            String prodName = Session[SessionFactory.ALL_PRODUCT_SELECTED_PRODUCT_NAME].ToString();

            Dictionary<String,ShopChildProdsSpecs> prodSpecDict=BackEndObjects.ShopChildProdsSpecs.
                getShopChildProdsSpecObjbyEntIdandProdNameDB(entId, prodName);
            Dictionary<String, String> existingFeatList = new Dictionary<string, string>();

            DataTable dt = new DataTable();

            dt.Columns.Add("FeatId");
            dt.Columns.Add("FeatName");
            dt.Columns.Add("SpecText");
            dt.Columns.Add("FromSpec");
            dt.Columns.Add("ToSpec");
            dt.Columns.Add("imgName");

            int counter = 0;
            foreach (KeyValuePair<String, ShopChildProdsSpecs> kvp in prodSpecDict)
            {
                ShopChildProdsSpecs specObj = (ShopChildProdsSpecs)kvp.Value;

                dt.Rows.Add();

                existingFeatList.Add(specObj.getFeatId(), specObj.getFeatId());

                dt.Rows[counter]["FeatId"] = specObj.getFeatId();
                dt.Rows[counter]["FeatName"] = Features.getFeaturebyIdwoSpecDB(specObj.getFeatId()).getFeatureName();
                dt.Rows[counter]["SpecText"] = specObj.getSpecText();
                if(!specObj.getFromSpecId().Equals(""))
                dt.Rows[counter]["FromSpec"] = Specifications.getSpecificationDetailbyIdDB(specObj.getFromSpecId()).getSpecName();
                if (!specObj.getToSpecId().Equals(""))
                dt.Rows[counter]["ToSpec"] = Specifications.getSpecificationDetailbyIdDB(specObj.getToSpecId()).getSpecName();

                String[] imgPath = specObj.getImgPath().Split(new String[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                dt.Rows[counter]["imgName"] = (imgPath != null && imgPath.Length > 0 ? imgPath[imgPath.Length - 1] : "N\\A");
                counter++;
            }

            GridView1.DataSource = dt;
            GridView1.DataBind();
            GridView1.Visible = true;           
            GridView1.Columns[2].Visible = false;

            Session[SessionFactory.PRODUCT_SPECIFICATION_EXISTING_FEAT_LIST] = existingFeatList;
            Session[SessionFactory.ALL_PROD_SPECIFICATION_DATAGRID] = dt;
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataSource = (DataTable)Session[SessionFactory.ALL_PROD_SPECIFICATION_DATAGRID];
            GridView1.DataBind();
            GridView1.Visible = true;
            GridView1.Columns[2].Visible = false;
        }

        protected void Link_Feat_Img_Show_Command(object sender, CommandEventArgs e)
        {
            ActionLibrary.ImageContextFactory icObj = new ActionLibrary.ImageContextFactory();
            icObj.setParentContextValue(Session[SessionFactory.ALL_PRODUCT_SELECTED_PRODUCT_NAME].ToString());
            icObj.setParentContextName(ActionLibrary.ImageContextFactory.PARENT_CONTEXT_PRODUCT);
            icObj.setDestinationContextName(ActionLibrary.ImageContextFactory.DESTINATION_CONTEXT_FEAT_FOR_PARENT_PRODUCT);

            Dictionary<String, String> childConObjs = new Dictionary<string, string>();
            childConObjs.Add(ActionLibrary.ImageContextFactory.CHILD_CONTEXT_PROD_ENT_ID, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            childConObjs.Add(ActionLibrary.ImageContextFactory.CHILD_CONTEXT_PROD_FEAT_ID,
                ((Label)GridView1.Rows[Convert.ToInt32(e.CommandArgument)].Cells[0].FindControl("Label_Hidden_Feat")).Text);

            icObj.setChildContextObjects(childConObjs);

            Session[SessionFactory.DISP_IMAGE_CONTEXT_FACTORY_OBJ] = icObj;            
            Response.Redirect("/Pages/DispImage.aspx");

        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            GridView1.DataSource = (DataTable)Session[SessionFactory.ALL_PROD_SPECIFICATION_DATAGRID];
            GridView1.DataBind();
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                GridViewRow gVR = e.Row;

                String featId = ((Label)gVR.Cells[0].FindControl("Label_Hidden_Feat")).Text;
                Dictionary<String,Specifications> specDict=BackEndObjects.Features.getSpecforFeatureDB(featId);

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

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DataTable dt = (DataTable)Session[SessionFactory.ALL_PROD_SPECIFICATION_DATAGRID];
            String prodName = Session[SessionFactory.ALL_PRODUCT_SELECTED_PRODUCT_NAME].ToString();

            String fromSpecText = ((DropDownList)GridView1.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_From_Spec_Edit")).SelectedItem != null ?
                ((DropDownList)GridView1.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_From_Spec_Edit")).SelectedItem.Text : "";
            String fromSpecId = ((DropDownList)GridView1.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_From_Spec_Edit")).SelectedValue != null ?
                ((DropDownList)GridView1.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_From_Spec_Edit")).SelectedValue:"";
            String ToSpecText = ((DropDownList)GridView1.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_To_Spec_Edit")).SelectedItem != null ?
                ((DropDownList)GridView1.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_To_Spec_Edit")).SelectedItem.Text:"";
            String ToSpecId = ((DropDownList)GridView1.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_To_Spec_Edit")).SelectedValue != null ?
                ((DropDownList)GridView1.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_To_Spec_Edit")).SelectedValue:"";

            String specText = ((TextBox)GridView1.Rows[e.RowIndex].Cells[0].FindControl("TextBox_SpecText_Edit")).Text;

            int index = GridView1.Rows[e.RowIndex].DataItemIndex;
            dt.Rows[index]["SpecText"] = specText;
            dt.Rows[index]["FromSpec"] = fromSpecText;
            dt.Rows[index]["ToSpec"] = ToSpecText;
            
            String updatedImgPath = "";

            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            Dictionary<String, String> targetVals = new Dictionary<string, string>();

            whereCls.Add(BackEndObjects.ShopChildProdsSpecs.SHOP_CHILD_PROD_SPEC_COL_PROD_NAME, prodName);
            whereCls.Add(BackEndObjects.ShopChildProdsSpecs.SHOP_CHILD_PROD_SPEC_COL_ENTITY_ID,Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            whereCls.Add(BackEndObjects.ShopChildProdsSpecs.SHOP_CHILD_PROD_SPEC_COL_FEAT_ID, ((Label)GridView1.Rows[e.RowIndex].Cells[0].FindControl("Label_Hidden_Feat")).Text);

            if (!((FileUpload)GridView1.Rows[e.RowIndex].Cells[0].FindControl("FileUpload_Image")).HasFile)
            {
                targetVals.Add(BackEndObjects.ShopChildProdsSpecs.SHOP_CHILD_PROD_SPEC_COL_FROM_SPEC_ID, fromSpecId);
                targetVals.Add(BackEndObjects.ShopChildProdsSpecs.SHOP_CHILD_PROD_SPEC_COL_TO_SPEC_ID, ToSpecId);
                targetVals.Add(BackEndObjects.ShopChildProdsSpecs.SHOP_CHILD_PROD_SPEC_COL_SPEC_TEXT, specText);

                BackEndObjects.ShopChildProdsSpecs.updateShopChildProdsSpecsDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);                               
            }
            else
            {                
                //Remove the entry and re-create
                BackEndObjects.ShopChildProdsSpecs.updateShopChildProdsSpecsDB(targetVals, whereCls, DBConn.Connections.OPERATION_DELETE);                               

                BackEndObjects.ShopChildProdsSpecs specObj = new ShopChildProdsSpecs();
                specObj.setEntityId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                specObj.setFeatId(((Label)GridView1.Rows[e.RowIndex].Cells[0].FindControl("Label_Hidden_Feat")).Text);
                specObj.setFileStream(((FileUpload)GridView1.Rows[e.RowIndex].Cells[0].FindControl("FileUpload_Image")));
                specObj.setFromSpecId(fromSpecId);
                specObj.setSpecText(specText);
                specObj.setToSpecId(ToSpecId);
                specObj.setProdName(prodName);
                specObj.setImgPathInFileStore(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

                BackEndObjects.ShopChildProdsSpecs.insertShopChildProdsSpecsDB(specObj);
                String[] imgPath = specObj.getImgPath().Split(new String[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                dt.Rows[index]["imgName"] = (imgPath != null && imgPath.Length > 0 ? imgPath[imgPath.Length - 1] : "N\\A");
                updatedImgPath = specObj.getImgPath();
            }
            Session[SessionFactory.ALL_PROD_SPECIFICATION_DATAGRID] = dt;
            GridView1.EditIndex = -1;
            GridView1.DataSource = dt;
            GridView1.DataBind();

        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GridView1.DataSource = (DataTable)Session[SessionFactory.ALL_PROD_SPECIFICATION_DATAGRID];
            GridView1.DataBind();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataTable dt = (DataTable)Session[SessionFactory.ALL_PROD_SPECIFICATION_DATAGRID];
            String prodName = Session[SessionFactory.ALL_PRODUCT_SELECTED_PRODUCT_NAME].ToString();

            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            Dictionary<String, String> targetVals = new Dictionary<string, string>();

            whereCls.Add(BackEndObjects.ShopChildProdsSpecs.SHOP_CHILD_PROD_SPEC_COL_PROD_NAME, prodName);
            whereCls.Add(BackEndObjects.ShopChildProdsSpecs.SHOP_CHILD_PROD_SPEC_COL_ENTITY_ID, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            whereCls.Add(BackEndObjects.ShopChildProdsSpecs.SHOP_CHILD_PROD_SPEC_COL_FEAT_ID, ((Label)GridView1.Rows[e.RowIndex].Cells[0].FindControl("Label_Hidden_Feat")).Text);

            try
            {
                BackEndObjects.ShopChildProdsSpecs.updateShopChildProdsSpecsDB(targetVals, whereCls, DBConn.Connections.OPERATION_DELETE);
                
                int index = GridView1.Rows[e.RowIndex].DataItemIndex;
                dt.Rows[index].Delete();
                Session[SessionFactory.ALL_PROD_SPECIFICATION_DATAGRID] = dt;
                Dictionary<String, String> existingFeatList = (Dictionary<String, String>)Session[SessionFactory.PRODUCT_SPECIFICATION_EXISTING_FEAT_LIST];
                if (existingFeatList != null && existingFeatList.ContainsKey(((Label)GridView1.Rows[e.RowIndex].Cells[0].FindControl("Label_Hidden_Feat")).Text))
                {
                    existingFeatList.Remove(((Label)GridView1.Rows[e.RowIndex].Cells[0].FindControl("Label_Hidden_Feat")).Text);
                    Session[SessionFactory.PRODUCT_SPECIFICATION_EXISTING_FEAT_LIST] = existingFeatList;
                }


                GridView1.EditIndex = -1;
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            catch (Exception ex)
            {
            }
        }

        protected void Buttin_Show_Spec_List_Click(object sender, EventArgs e)
        {
            fillFeatureGrid();
        }

        protected void fillFeatureGrid()
        {

            String selectedProdCatId = "";
            if (Request.QueryString.GetValues("prodCatId") != null)
                selectedProdCatId = Request.QueryString.GetValues("prodCatId")[0];

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

        protected void GridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView2.PageIndex = e.NewPageIndex;
            GridView2.SelectedIndex = -1;
            fillGrid();
        }

        protected void fillGrid()
        {
            String selectedProdCatId = "";
            if (Request.QueryString.GetValues("prodCatId") != null)
                selectedProdCatId = Request.QueryString.GetValues("prodCatId")[0];

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
            Dictionary<String, String> existingFeatList = (Dictionary<String,String>)Session[SessionFactory.PRODUCT_SPECIFICATION_EXISTING_FEAT_LIST];
            if (existingFeatList != null && existingFeatList.ContainsKey(((Label)GridView2.SelectedRow.Cells[1].FindControl("Label_Hidden")).Text))
            {
                Label_Feat_Exists.Visible = true;
                Label_Feat_Exists.Text = "Feature already added to this product. You can delete from the above list and re-enter";
                Label_Feat_Exists.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                Label_Feat_Exists.Visible = false;
                Button_Add_To_Prod.Enabled = true;

                ArrayList PRODSpecList = (ArrayList)Session[SessionFactory.UPDATE_PROD_SELECTED_PROD_SPEC_MAP];
                String fileName = null;
                String prodName = Session[SessionFactory.ALL_PRODUCT_SELECTED_PRODUCT_NAME].ToString();

                if (PRODSpecList == null)
                    PRODSpecList = new ArrayList();

                BackEndObjects.ShopChildProdsSpecs PRODSpec = new ShopChildProdsSpecs();
                PRODSpec.setEntityId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                PRODSpec.setFeatId(((Label)GridView2.SelectedRow.Cells[1].FindControl("Label_Hidden")).Text);
                PRODSpec.setFromSpecId(((DropDownList)GridView2.SelectedRow.Cells[2].FindControl("DropDownList_GridView1_From")).SelectedValue);
                PRODSpec.setToSpecId(((DropDownList)GridView2.SelectedRow.Cells[3].FindControl("DropDownList_GridView1_To")).SelectedValue);
                PRODSpec.setProdName(prodName);

                if (((FileUpload)GridView2.SelectedRow.Cells[3].FindControl("FileUpload_Spec")).HasFile)
                {
                    PRODSpec.setFileStream((FileUpload)GridView2.SelectedRow.Cells[3].FindControl("FileUpload_Spec"));
                    fileName = ((FileUpload)GridView2.SelectedRow.Cells[3].FindControl("FileUpload_Spec")).FileName;
                    ((Label)GridView2.SelectedRow.Cells[0].FindControl("Label_File_Name")).Text = fileName;
                    ((Label)GridView2.SelectedRow.Cells[0].FindControl("Label_File_Name")).Visible = true;
                    ((FileUpload)GridView2.SelectedRow.Cells[0].FindControl("FileUpload_Spec")).Visible = false;

                }

                PRODSpecList.Add(PRODSpec);


                GridView2.SelectedRow.Cells[0].Enabled = false;
                GridView2.SelectedRow.Cells[3].Enabled = false;
                GridView2.SelectedRow.Cells[4].Enabled = false;
                GridView2.SelectedRow.Cells[5].Enabled = false;


                GridView2.SelectedRow.Cells[0].FindControl("Image_Selected").Visible = true;
                Label_Selected_List.Text += "," + GridView2.SelectedRow.DataItemIndex;
                Session[SessionFactory.UPDATE_PROD_SELECTED_PROD_SPEC_MAP] = PRODSpecList;
            }
        }

        protected void getAddintionalProdSrvList()
        {
            ArrayList prodSpecList = (ArrayList)Session[SessionFactory.UPDATE_PROD_SELECTED_PROD_SPEC_MAP];
            if (prodSpecList == null)
                prodSpecList = new ArrayList();

            BackEndObjects.ShopChildProdsSpecs rfqSpec = new ShopChildProdsSpecs();
            //rfqSpec.set(Session[SessionFactory.CREATE_PRODUCT_SELECTED_PRODUCT_CAT].ToString());
            rfqSpec.setEntityId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            rfqSpec.setFeatId("ft_dummy");
            rfqSpec.setFromSpecId("");
            rfqSpec.setToSpecId("");
            rfqSpec.setSpecText(TextBox_Spec.Text);
            rfqSpec.setProdName(Session[SessionFactory.ALL_PRODUCT_SELECTED_PRODUCT_NAME].ToString());
            //rfqSpec.setCreatedDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            //if (User.Identity.Name != null)
            //rfqSpec.setCreatedUsr(User.Identity.Name);
            if (FileUpload_Extra_Spec.HasFile)
                rfqSpec.setFileStream(FileUpload_Extra_Spec);


            prodSpecList.Add(rfqSpec);

            Session[SessionFactory.UPDATE_PROD_SELECTED_PROD_SPEC_MAP] = prodSpecList;
        }

        protected void Button_Add_To_Prod_Click(object sender, EventArgs e)
        {
                if (!TextBox_Spec.Text.Equals(""))
                {
                    getAddintionalProdSrvList();
                    TextBox_Spec.Text = "";
                }

                ArrayList prodSpecList = (ArrayList)Session[SessionFactory.UPDATE_PROD_SELECTED_PROD_SPEC_MAP];
                         /*Dictionary<String, String> existingProdList = (Dictionary<String,String>)
                             Session[SessionFactory.PRODUCT_SPECIFICATION_EXISTING_FEAT_LIST];
            existingProdList=(existingProdList==null?new Dictionary<String,String>():existingProdList);*/

                Dictionary<String, String> rSpecUniqnessValidation = new Dictionary<string, string>();
                String mainEntId = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();

                if (prodSpecList != null)
                    for (int i = 0; i < prodSpecList.Count; i++)
                    {

                        ShopChildProdsSpecs prodSpecObj = (ShopChildProdsSpecs)prodSpecList[i];
                        //existingProdList.Add(prodSpecObj.getFeatId(),prodSpecObj.getFeatId());

                        if (rSpecUniqnessValidation.ContainsKey(prodSpecObj.getEntityId() + ":" + prodSpecObj.getProdName() + ":" + prodSpecObj.getFeatId()))
                            prodSpecList.RemoveAt(i);
                        else
                        {
                            rSpecUniqnessValidation.Add(prodSpecObj.getEntityId() + ":" + prodSpecObj.getProdName() + ":" + prodSpecObj.getFeatId(), prodSpecObj.getEntityId());
                            if (prodSpecObj != null && prodSpecObj.getFileStream() != null && prodSpecObj.getFileStream().HasFile)
                                prodSpecObj.setImgPathInFileStore(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                        }
                    }

                try
                {
                    if (prodSpecList != null)
                        BackEndObjects.ShopChildProdsSpecs.insertShopChildProdsSpecsListDB(prodSpecList);

                    Label_Status.Text = "Product/Service Details Updated Successfully";
                    Label_Status.Visible = true;
                    Label_Status.ForeColor = System.Drawing.Color.Green;
                    fillFirstGrid();       
                   
                }
                catch (Exception ex)
                {

                    Label_Status.Text = "Product/Service Details Addition Failed";
                    Label_Status.Visible = true;
                    Label_Status.ForeColor = System.Drawing.Color.Red;
                }
                finally
                {
                    Session.Remove(SessionFactory.UPDATE_PROD_SELECTED_PROD_SPEC_MAP);                   
                }
            
        }

        protected void TextBox_Spec_TextChanged(object sender, EventArgs e)
        {
            if (!TextBox_Spec.Text.Equals(""))
                Button_Add_To_Prod.Enabled = true;
        }


    }
}