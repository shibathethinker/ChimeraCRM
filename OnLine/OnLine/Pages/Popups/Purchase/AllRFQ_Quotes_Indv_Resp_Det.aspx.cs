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

namespace OnLine.Pages.Popups.Purchase
{
    public partial class AllRFQ_Quotes_Indv_Resp_Det : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                fillGrid();
                Button_ShortList.Enabled = checkIfAlreadyShortListed();
            }
        }
        /// <summary>
        /// This method check if this respectiv entry was already shortlisted.
        /// Based on this result the shortlist button will be enabled/disabled
        /// </summary>
        /// <returns></returns>
        protected Boolean checkIfAlreadyShortListed()
        {
            BackEndObjects.RFQShortlisted shortObj = BackEndObjects.RFQShortlisted.
                getRFQShortlistedbyRespEntandRFQId(Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SHOW_QUOTE_SELECTED_RESP_ENTITY_ID].ToString(),
                Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SELECTED_RFQ_ID].ToString());

            Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

            if (shortObj != null && shortObj.getRFQId() != null && !shortObj.getRFQId().Equals(""))
                return false;
            else if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_RFQ] ||
                accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                return true;
            else
                return false;
        }
        protected void fillGrid()
        {
            ArrayList rfqProdList = (ArrayList)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_PROD_QNTY];
            ArrayList rfqSpecList = (ArrayList)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_RFQ_SPEC];
                       

            float totalAmountFrom = 0, totalAmountTo = 0;

            String rfqId = Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SELECTED_RFQ_ID].ToString();

            DataTable dt = new DataTable();
            dt.Columns.Add("Hidden");
            dt.Columns.Add("CategoryName");
            dt.Columns.Add("featureDataTable");
            dt.Columns.Add("FromQnty");
            dt.Columns.Add("ToQnty");
            dt.Columns.Add("FromPrice");
            dt.Columns.Add("ToPrice");
            dt.Columns.Add("msrmntUnit");
            //dt.Columns.Add("HiddenCatId");
            dt.Columns.Add("Quote");
            // dt.Columns.Add("QuoteUnit");
            dt.Columns.Add("Total");
            //dt.Columns.Add("Audit");

            Dictionary<String, RFQResponseQuotes> rfqRespQuoteDict = BackEndObjects.RFQResponseQuotes.
                getAllResponseQuotesforRFQandResponseEntityDB(rfqId, Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SHOW_QUOTE_SELECTED_RESP_ENTITY_ID].ToString());

            String contactEntId = "";

            for (int i = 0; i < rfqProdList.Count; i++)
            {

                BackEndObjects.RFQProdServQnty rfqProdObj = (BackEndObjects.RFQProdServQnty)rfqProdList[i];

                dt.Rows.Add();
                dt.Rows[i]["Hidden"] = rfqProdObj.getProdCatId();
                dt.Rows[i]["CategoryName"] = BackEndObjects.ProductCategory.
                    getProductCategorybyIdwoFeaturesDB(rfqProdObj.getProdCatId()).getProductCategoryName();
                //dt.Rows[i]["featureDataTable"] = dtSpec;
                dt.Rows[i]["FromQnty"] = rfqProdObj.getFromQnty();
                dt.Rows[i]["ToQnty"] = rfqProdObj.getToQnty();
                dt.Rows[i]["FromPrice"] = rfqProdObj.getFromPrice();
                dt.Rows[i]["ToPrice"] = rfqProdObj.getToPrice();
                dt.Rows[i]["msrmntUnit"] = rfqProdObj.getMsrmntUnit();

                RFQResponseQuotes rfqRespObj = new RFQResponseQuotes();
                
                
                try
                {
                    rfqRespObj = rfqRespQuoteDict[rfqProdObj.getProdCatId()];
                    contactEntId = rfqRespObj.getResponseEntityId();
                    String respQuote = rfqRespObj.getQuote().Equals("") ? "0" : rfqRespObj.getQuote(); ;
                    dt.Rows[i]["Quote"] = respQuote;
                    //dt.Rows[i]["QuoteUnit"] = rfqRespObj.getUnitName();
                    totalAmountFrom = totalAmountFrom + (rfqProdObj.getFromQnty() * float.Parse(respQuote));
                    totalAmountTo = totalAmountTo + (rfqProdObj.getToQnty() * float.Parse(respQuote));
                    dt.Rows[i]["Total"] = (rfqProdObj.getFromQnty() * float.Parse(respQuote)) + " to " + (rfqProdObj.getToQnty() * float.Parse(respQuote));
                }
                catch (KeyNotFoundException e)
                {
                    //If no response given to this RFQ by this entity so far
                    dt.Rows[i]["Quote"] = "0";
                    dt.Rows[i]["Total"] = "0";
                }

            }

            Label_RFQ_Name.Text = rfqId;
            //Label_Vendor.Text = 
            Dictionary<String,Object> contactDict=ActionLibrary.customerDetails.getContactDetails(contactEntId,
                Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            if (contactDict.ContainsKey(ActionLibrary.customerDetails.RETURN_OBJECT_TYPE_CONTACT_DETAILS))
                Label_Vendor.Text = ((BackEndObjects.Contacts)contactDict[ActionLibrary.customerDetails.RETURN_OBJECT_TYPE_CONTACT_DETAILS]).getContactName();
            else
                Label_Vendor.Text = ((BackEndObjects.MainBusinessEntity)contactDict[ActionLibrary.customerDetails.RETURN_OBJECT_TYPE_MAIN_BUSINESS_ENTITY]).getEntityName();


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
                            dtSpec.Rows[rowCount]["FromSpec"] = BackEndObjects.Specifications.
                                getSpecificationDetailbyIdDB(rfqSpecObj.getFromSpecId()).getSpecName();
                        if (!rfqSpecObj.getToSpecId().Equals(""))
                            dtSpec.Rows[rowCount]["ToSpec"] = BackEndObjects.Specifications.
                                getSpecificationDetailbyIdDB(rfqSpecObj.getToSpecId()).getSpecName();

                        rowCount++;
                    }
                }
                ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).DataSource = dtSpec;
                ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).DataBind();
                ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).Visible = true;
                ((GridView)gVR.Cells[2].FindControl("GridView1_Inner")).Columns[1].Visible = false;
            }

            GridView1.Visible = true;
            Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_RESP_QUOTE_GRID_DATA] = dt;

            Label_Total.Text = totalAmountFrom + " to " + totalAmountTo;
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataSource =(DataTable) Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_RESP_QUOTE_GRID_DATA];
            GridView1.DataBind();
            GridView1.Columns[0].Visible = false;

        }

        protected void GridView1_Inner_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView grdInner = ((GridView)sender);
            grdInner.PageIndex = e.NewPageIndex;

            GridViewRow gvRowParent = ((GridView)sender).Parent.Parent as GridViewRow;

            DataTable dtSpec = new DataTable();
            dtSpec.Columns.Add("Hidden");
            dtSpec.Columns.Add("FeatName");
            dtSpec.Columns.Add("SpecText");
            dtSpec.Columns.Add("FromSpec");
            dtSpec.Columns.Add("ToSpec");

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

                    rowCount++;
                }
            }
            grdInner.DataSource = dtSpec;
            grdInner.DataBind();
            grdInner.Visible = true;
            //Session[SessionFactory.ALL_PURCHASE_ALL_REQUIREMENT_SHOW_SPEC_INNER_GRID_DATA] = dtSpec;

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

            icObj.setParentContextName(ActionLibrary.ImageContextFactory.PARENT_CONTEXT_RFQ);
            icObj.setParentContextValue(Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SELECTED_RFQ_ID].ToString());
            icObj.setDestinationContextName(ActionLibrary.ImageContextFactory.DESTINATION_CONTEXT_FEAT_FOR_PARENT_RFQ);

            Dictionary<String, String> childContextDict = new Dictionary<string, string>();
            childContextDict.Add(ActionLibrary.ImageContextFactory.CHILD_CONTEXT_PRODCAT_ID, prodCatId);
            childContextDict.Add(ActionLibrary.ImageContextFactory.CHILD_CONTEXT_FEAT_ID, featId);
            icObj.setChildContextObjects(childContextDict);

            Session[SessionFactory.DISP_IMAGE_CONTEXT_FACTORY_OBJ] = icObj;
            //Server.Transfer("/Pages/DispImage.aspx", true);
            Response.Redirect("/Pages/DispImage.aspx");
        }

        protected void Button_ShortList_Click(object sender, EventArgs e)
        {
            BackEndObjects.RFQShortlisted potRec = new BackEndObjects.RFQShortlisted();

            potRec.setRFQId(Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SELECTED_RFQ_ID].ToString());
            potRec.setRespEntityId(Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_SHOW_QUOTE_SELECTED_RESP_ENTITY_ID].ToString());
            potRec.setPotStat(BackEndObjects.PotentialStatus.POTENTIAL_STAT_PRELIM);
            potRec.setPotentialId(new Id().getNewId(BackEndObjects.Id.ID_TYPE_POTENTIAL_ID_STRING));
            
            //Now calculate the potential amount
            String[] splitDelim = { "to" };
            String[] splitData = Label_Total.Text.Split(splitDelim, StringSplitOptions.RemoveEmptyEntries);
            float potAmnt = 0;

            if (splitData.Length > 1)
                potAmnt = (float.Parse(splitData[0]) + float.Parse(splitData[1])) / 2;
            else
                potAmnt = float.Parse(splitData[0]);

            potRec.setPotenAmnt(potAmnt);
            potRec.setPotActStat(BackEndObjects.RFQShortlisted.POTENTIAL_ACTIVE_STAT_ACTIVE);
            potRec.setFinlSupFlag("N");
            potRec.setFinlCustFlag("N");
            potRec.setCreateMode(BackEndObjects.RFQShortlisted.POTENTIAL_CREATION_MODE_AUTO);
            potRec.setCreatedDate(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            try
            {
                BackEndObjects.RFQShortlisted.insertRFQShorListedEntryDB(potRec);
                Label_ShortList_Stat.Text = "Entry shortlisted successfully";
                Label_ShortList_Stat.ForeColor = System.Drawing.Color.Green;
                Label_ShortList_Stat.Visible = true;

                DataTable dt = (DataTable)Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_ALL_QUOTES_GRID];
                int dataItemIndex = Int32.Parse(Request.QueryString.GetValues("dataItemIndex")[0]);
                dt.Rows[dataItemIndex]["ShortListed"] = "Y";
                //dt.DefaultView.Sort = "Submit Date Ticks" + " " + "DESC";
                Session[SessionFactory.ALL_PURCHASE_ALL_RFQ_GRID_DATA] = dt;
                ScriptManager.RegisterStartupScript(this, typeof(string), "RefreshRefqGrid", "RefreshParent();", true);
            }
            catch(Exception ex)
            {
                Label_ShortList_Stat.Text = "Shortlisting process failed";
                Label_ShortList_Stat.ForeColor = System.Drawing.Color.Red;
                Label_ShortList_Stat.Visible = true;
            }
        }
    }
}