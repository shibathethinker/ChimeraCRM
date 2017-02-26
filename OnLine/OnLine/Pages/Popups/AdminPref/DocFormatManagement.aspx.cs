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

namespace OnLine.Pages.Popups.AdminPref
{
    public partial class DocFormatManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                loadPODetails();
                loadSODetails();
                loadInvDetails();                
            }
        }

        protected void loadPODetails()
        {
            ArrayList poList=BackEndObjects.DocFormat.
                getDocFormatforEntityIdandDocTypeDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), BackEndObjects.DocFormat.DOCFORMAT_DOC_TYPE_PURCHASE_ORDER);

            for(int i=0;i<poList.Count;i++)
            {
                DocFormat poObj=(DocFormat)poList[i];

                String sectionType=poObj.getSection_type();

                if(TextBox_PO_format_name.Text.Equals(""))
                TextBox_PO_format_name.Text = poObj.getDocformat_name();

                switch(sectionType)
                {
                    case BackEndObjects.DocFormat.DOCFORMAT_DOC_TYPE_PURCHASE_ORDER_SECTION_TYPE_TNC:
                        TextBox_PO_TnC.Text = poObj.getText();
                        Label_TNC_flag_PO.Text = TextBox_PO_TnC.Text.Equals("") ? "Y" : "N";
                        break;
                }
            }            
            
        }

        protected void loadSODetails()
        {
            ArrayList soList = BackEndObjects.DocFormat.
                getDocFormatforEntityIdandDocTypeDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), BackEndObjects.DocFormat.DOCFORMAT_DOC_TYPE_SALES_ORDER);

            for (int i = 0; i < soList.Count; i++)
            {
                DocFormat soObj = (DocFormat)soList[i];

                String sectionType = soObj.getSection_type();

                if (TextBox_SO_format_name.Text.Equals(""))
                    TextBox_SO_format_name.Text = soObj.getDocformat_name();

                switch (sectionType)
                {
                    case BackEndObjects.DocFormat.DOCFORMAT_DOC_TYPE_SALES_ORDER_SECTION_TYPE_TNC:
                        TextBox_SO_TnC.Text = soObj.getText();
                        Label_TNC_flag_SO.Text = TextBox_SO_TnC.Text.Equals("") ? "Y" : "N";
                        break;
                }
            }          
        }

        protected void loadInvDetails()
        {
            ArrayList invList = BackEndObjects.DocFormat.
    getDocFormatforEntityIdandDocTypeDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), BackEndObjects.DocFormat.DOCFORMAT_DOC_TYPE_INVOICE);

            DataTable dt = new DataTable();
            dt.Columns.Add("Hidden");
            dt.Columns.Add("Comp_Name");
            dt.Columns.Add("Comp_Value");

            int taxCompGridCounter = 0;


            for (int i = 0; i < invList.Count; i++)
            {
                DocFormat invObj = (DocFormat)invList[i];

                String sectionType = invObj.getSection_type();

                if (TextBox_INV_format_name.Text.Equals(""))
                    TextBox_INV_format_name.Text = invObj.getDocformat_name();

                switch (sectionType)
                {
                    case BackEndObjects.DocFormat.DOCFORMAT_DOC_TYPE_INVOICE_SECTION_TYPE_TNC:
                        TextBox_INV_TnC.Text = invObj.getText();
                        Label_TNC_flag_Inv.Text = TextBox_INV_TnC.Text.Equals("") ? "Y" : "N";
                        break;

                    case BackEndObjects.DocFormat.DOCFORMAT_DOC_TYPE_INVOICE_SECTION_TYPE_TAX:
                        dt.Rows.Add();
                        dt.Rows[taxCompGridCounter]["Hidden"] = invObj.getDocformat_id();
                        //Component name is the section type name
                        dt.Rows[taxCompGridCounter]["Comp_Name"] = invObj.getSectionTypeName();
                        dt.Rows[taxCompGridCounter]["Comp_Value"] = invObj.getText();
                        taxCompGridCounter++;
                        break;

                }
            }
            
            if (dt.Rows.Count > 0)
            {
                GridView_Inv_Tax_Comp.DataSource = dt;
                GridView_Inv_Tax_Comp.DataBind();
                GridView_Inv_Tax_Comp.Visible = true;
                GridView_Inv_Tax_Comp.Columns[2].Visible = false;
                Session[SessionFactory.ADMIN_PREF_DOCFORMAT_MGMT_TAX_COMP_GRID] = dt;
            }
        }

        protected void loadInvTaxCompGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Hidden");
            dt.Columns.Add("Comp_Name");
            dt.Columns.Add("Comp_Value");

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
                        taxCompGridCounter++;
                        break;

                }
            }

            if (dt.Rows.Count > 0)
            {
                GridView_Inv_Tax_Comp.DataSource = dt;
                GridView_Inv_Tax_Comp.DataBind();
                GridView_Inv_Tax_Comp.Visible = true;
                GridView_Inv_Tax_Comp.Columns[2].Visible = false;
                Session[SessionFactory.ADMIN_PREF_DOCFORMAT_MGMT_TAX_COMP_GRID] = dt;
            }
        }

        protected void Button_Update_PO_Click(object sender, EventArgs e)
        {

            String sectionTnC = Label_TNC_flag_PO.Text;
            Boolean insertFlag = sectionTnC.Equals("Y");
            Boolean updateStat = true;

            ArrayList docFormatList = new ArrayList();
            DocFormat sectionTnCObj = new DocFormat();
            String docFormatId = "";

            if (sectionTnC.Equals("Y"))
            {
                if (!TextBox_PO_format_name.Text.Equals("") && !TextBox_PO_TnC.Text.Equals(""))
                {
                    docFormatId = new BackEndObjects.Id().getNewId(BackEndObjects.Id.ID_TYPE_DOCFORMAT_ID_STRING);


                    sectionTnCObj.setCmp_id(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                    sectionTnCObj.setDoc_type(BackEndObjects.DocFormat.DOCFORMAT_DOC_TYPE_PURCHASE_ORDER);
                    sectionTnCObj.setDocformat_id(docFormatId);
                    sectionTnCObj.setDocformat_name(TextBox_PO_format_name.Text);
                    sectionTnCObj.setSection_type(BackEndObjects.DocFormat.DOCFORMAT_DOC_TYPE_PURCHASE_ORDER_SECTION_TYPE_TNC);
                    sectionTnCObj.setText(TextBox_PO_TnC.Text);

                    docFormatList.Add(sectionTnCObj);
                }
            }
            else
            {
                            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            whereCls.Add(BackEndObjects.DocFormat.DOCFORMAT_COL_CMP_ID, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            whereCls.Add(BackEndObjects.DocFormat.DOCFORMAT_COL_DOC_TYPE, BackEndObjects.DocFormat.DOCFORMAT_DOC_TYPE_PURCHASE_ORDER);
            whereCls.Add(BackEndObjects.DocFormat.DOCFORMAT_COL_SECTION_TYPE, BackEndObjects.DocFormat.DOCFORMAT_DOC_TYPE_PURCHASE_ORDER_SECTION_TYPE_TNC);
                
                Dictionary<String, String> targetVals = new Dictionary<string, string>();
                targetVals.Add(BackEndObjects.DocFormat.DOCFORMAT_COL_TEXT, TextBox_PO_TnC.Text);
                targetVals.Add(BackEndObjects.DocFormat.DOCFORMAT_COL_DOCFORMAT_NAME, TextBox_PO_format_name.Text);
              
                BackEndObjects.DocFormat.updateDocFormatObjsDB(whereCls, targetVals, DBConn.Connections.OPERATION_UPDATE);
                updateStat = true;
            }

            try
            {
                if (insertFlag)
                {
                    BackEndObjects.DocFormat.insertDocFormatDB(docFormatList);
                }
                    Label_PO_update_stat.Visible = true;
                    Label_PO_update_stat.ForeColor = System.Drawing.Color.Green;
                    Label_PO_update_stat.Text = "Updated Successfully";
                
            }
            catch (Exception ex)
            {
                Label_PO_update_stat.Visible = true;
                Label_PO_update_stat.ForeColor = System.Drawing.Color.Red;
                Label_PO_update_stat.Text = "Update Failed";
            }
        }

        protected void Button_SO_Update_Click(object sender, EventArgs e)
        {
            String sectionTnC = Label_TNC_flag_SO.Text;
            Boolean insertFlag = sectionTnC.Equals("Y");
            Boolean updateStat = true;

            ArrayList docFormatList = new ArrayList();
            DocFormat sectionTnCObj = new DocFormat();
            String docFormatId = "";

            if (sectionTnC.Equals("Y"))
            {
                if (!TextBox_SO_format_name.Text.Equals("") && !TextBox_SO_TnC.Text.Equals(""))
                {
                    docFormatId = new BackEndObjects.Id().getNewId(BackEndObjects.Id.ID_TYPE_DOCFORMAT_ID_STRING);


                    sectionTnCObj.setCmp_id(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                    sectionTnCObj.setDoc_type(BackEndObjects.DocFormat.DOCFORMAT_DOC_TYPE_SALES_ORDER);
                    sectionTnCObj.setDocformat_id(docFormatId);
                    sectionTnCObj.setDocformat_name(TextBox_SO_format_name.Text);
                    sectionTnCObj.setSection_type(BackEndObjects.DocFormat.DOCFORMAT_DOC_TYPE_SALES_ORDER_SECTION_TYPE_TNC);
                    sectionTnCObj.setText(TextBox_SO_TnC.Text);

                    docFormatList.Add(sectionTnCObj);
                }
            }
            else
            {
                Dictionary<String, String> whereCls = new Dictionary<string, string>();
                whereCls.Add(BackEndObjects.DocFormat.DOCFORMAT_COL_CMP_ID, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                whereCls.Add(BackEndObjects.DocFormat.DOCFORMAT_COL_DOC_TYPE, BackEndObjects.DocFormat.DOCFORMAT_DOC_TYPE_SALES_ORDER);
                whereCls.Add(BackEndObjects.DocFormat.DOCFORMAT_COL_SECTION_TYPE, BackEndObjects.DocFormat.DOCFORMAT_DOC_TYPE_SALES_ORDER_SECTION_TYPE_TNC);

                Dictionary<String, String> targetVals = new Dictionary<string, string>();
                targetVals.Add(BackEndObjects.DocFormat.DOCFORMAT_COL_TEXT, TextBox_SO_TnC.Text);
                targetVals.Add(BackEndObjects.DocFormat.DOCFORMAT_COL_DOCFORMAT_NAME, TextBox_SO_format_name.Text);

                BackEndObjects.DocFormat.updateDocFormatObjsDB(whereCls, targetVals, DBConn.Connections.OPERATION_UPDATE);
                updateStat = true;
            }

            try
            {
                if (insertFlag)
                {
                    BackEndObjects.DocFormat.insertDocFormatDB(docFormatList);
                }
                    Label_SO_update_stat.Visible = true;
                    Label_SO_update_stat.ForeColor = System.Drawing.Color.Green;
                    Label_SO_update_stat.Text = "Updated Successfully";
                
            }
            catch (Exception ex)
            {
                Label_SO_update_stat.Visible = true;
                Label_SO_update_stat.ForeColor = System.Drawing.Color.Red;
                Label_SO_update_stat.Text = "Update Failed";
            }
        }

        protected void Button_Inv_Update_Click(object sender, EventArgs e)
        {
            String sectionTnC = Label_TNC_flag_Inv.Text;
            //The sectionTnC label helps to determinte whethere the sections need an insert or update
            Boolean insertFlag = sectionTnC.Equals("Y");
            Boolean updateStat = true;

            ArrayList docFormatList = new ArrayList();
            DocFormat sectionTnCObj = new DocFormat();
            DocFormat sectionTaxCompObj = new DocFormat();

            String docFormatId = "";

            if (sectionTnC.Equals("Y"))
            {
                if (!TextBox_INV_format_name.Text.Equals("") && !TextBox_INV_TnC.Text.Equals(""))
                {
                    docFormatId = new BackEndObjects.Id().getNewId(BackEndObjects.Id.ID_TYPE_DOCFORMAT_ID_STRING);


                    sectionTnCObj.setCmp_id(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                    sectionTnCObj.setDoc_type(BackEndObjects.DocFormat.DOCFORMAT_DOC_TYPE_INVOICE);
                    sectionTnCObj.setDocformat_id(docFormatId);
                    sectionTnCObj.setDocformat_name(TextBox_INV_format_name.Text);
                    sectionTnCObj.setSection_type(BackEndObjects.DocFormat.DOCFORMAT_DOC_TYPE_INVOICE_SECTION_TYPE_TNC);
                    sectionTnCObj.setText(TextBox_INV_TnC.Text);

                    docFormatList.Add(sectionTnCObj);
                }
            }
            else
            {
                Dictionary<String, String> whereCls = new Dictionary<string, string>();
                whereCls.Add(BackEndObjects.DocFormat.DOCFORMAT_COL_CMP_ID, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                whereCls.Add(BackEndObjects.DocFormat.DOCFORMAT_COL_DOC_TYPE, BackEndObjects.DocFormat.DOCFORMAT_DOC_TYPE_INVOICE);
                whereCls.Add(BackEndObjects.DocFormat.DOCFORMAT_COL_SECTION_TYPE, BackEndObjects.DocFormat.DOCFORMAT_DOC_TYPE_INVOICE_SECTION_TYPE_TNC);

                Dictionary<String, String> targetVals = new Dictionary<string, string>();
                targetVals.Add(BackEndObjects.DocFormat.DOCFORMAT_COL_TEXT, TextBox_INV_TnC.Text);
                targetVals.Add(BackEndObjects.DocFormat.DOCFORMAT_COL_DOCFORMAT_NAME, TextBox_INV_format_name.Text);

                BackEndObjects.DocFormat.updateDocFormatObjsDB(whereCls, targetVals, DBConn.Connections.OPERATION_UPDATE);
                updateStat = true;
            }

            try
            {
                Boolean refreshTaxCompGrid = false;

                if (!TextBox_Inv_Tax_Comp_Name.Text.Equals("") && !TextBox_Inv_Tax_Comp_Value.Text.Equals(""))
                {
                    sectionTaxCompObj.setCmp_id(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                    sectionTaxCompObj.setDoc_type(BackEndObjects.DocFormat.DOCFORMAT_DOC_TYPE_INVOICE);

                    docFormatId = docFormatId.Equals("") ? new BackEndObjects.Id().getNewId(BackEndObjects.Id.ID_TYPE_DOCFORMAT_ID_STRING) : docFormatId;

                    sectionTaxCompObj.setDocformat_id(docFormatId);
                    sectionTaxCompObj.setDocformat_name(TextBox_INV_format_name.Text);
                    sectionTaxCompObj.setSection_type(BackEndObjects.DocFormat.DOCFORMAT_DOC_TYPE_INVOICE_SECTION_TYPE_TAX);
                    sectionTaxCompObj.setSectionTypeName(TextBox_Inv_Tax_Comp_Name.Text);
                    sectionTaxCompObj.setText(TextBox_Inv_Tax_Comp_Value.Text);

                    docFormatList.Add(sectionTaxCompObj);
                    refreshTaxCompGrid = true;
                }
                if (docFormatList.Count>0)
                {
                    BackEndObjects.DocFormat.insertDocFormatDB(docFormatList);
                }
                if (refreshTaxCompGrid)
                {
                    loadInvTaxCompGrid();
                }

                    Label_Inv_Update_stat.Visible = true;
                    Label_Inv_Update_stat.ForeColor = System.Drawing.Color.Green;
                    Label_Inv_Update_stat.Text = "Updated Successfully";
                
            }
            catch (Exception ex)
            {
                Label_Inv_Update_stat.Visible = true;
                Label_Inv_Update_stat.ForeColor = System.Drawing.Color.Red;
                Label_Inv_Update_stat.Text = "Update Failed";
            }
        }

        protected void GridView_Inv_Tax_Comp_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_Inv_Tax_Comp.PageIndex = e.NewPageIndex;
            GridView_Inv_Tax_Comp.DataSource = (DataTable)Session[SessionFactory.ADMIN_PREF_DOCFORMAT_MGMT_TAX_COMP_GRID];
            GridView_Inv_Tax_Comp.DataBind();

        }

        protected void GridView_Inv_Tax_Comp_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView_Inv_Tax_Comp.EditIndex = e.NewEditIndex;
            GridView_Inv_Tax_Comp.DataSource = (DataTable)Session[SessionFactory.ADMIN_PREF_DOCFORMAT_MGMT_TAX_COMP_GRID];
            GridView_Inv_Tax_Comp.DataBind();
        }

        protected void GridView_Inv_Tax_Comp_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void GridView_Inv_Tax_Comp_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataTable dt = (DataTable)Session[SessionFactory.ADMIN_PREF_DOCFORMAT_MGMT_TAX_COMP_GRID];
            GridViewRow gVR = GridView_Inv_Tax_Comp.Rows[e.RowIndex];

            dt.Rows[e.RowIndex].Delete();

            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            whereCls.Add(BackEndObjects.DocFormat.DOCFORMAT_COL_CMP_ID, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            whereCls.Add(BackEndObjects.DocFormat.DOCFORMAT_COL_DOC_TYPE, BackEndObjects.DocFormat.DOCFORMAT_DOC_TYPE_INVOICE);
            whereCls.Add(BackEndObjects.DocFormat.DOCFORMAT_COL_SECTION_TYPE, BackEndObjects.DocFormat.DOCFORMAT_DOC_TYPE_INVOICE_SECTION_TYPE_TAX);
            whereCls.Add(BackEndObjects.DocFormat.DOCFORMAT_COL_SECTION_TYPE_NAME, ((Label)gVR.Cells[0].FindControl("Label_Name")).Text);

            Dictionary<String, String> targetVals = new Dictionary<string, string>();


            BackEndObjects.DocFormat.updateDocFormatObjsDB(whereCls, targetVals, DBConn.Connections.OPERATION_DELETE);

            GridView_Inv_Tax_Comp.DataSource = dt;
            GridView_Inv_Tax_Comp.DataBind();

            Session[SessionFactory.ADMIN_PREF_DOCFORMAT_MGMT_TAX_COMP_GRID] = dt;
        }

        protected void GridView_Inv_Tax_Comp_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView_Inv_Tax_Comp.EditIndex = -1;
            GridView_Inv_Tax_Comp.DataSource = (DataTable)Session[SessionFactory.ADMIN_PREF_DOCFORMAT_MGMT_TAX_COMP_GRID];
            GridView_Inv_Tax_Comp.DataBind();
        }

        protected void GridView_Inv_Tax_Comp_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            DataTable dt = (DataTable)Session[SessionFactory.ADMIN_PREF_DOCFORMAT_MGMT_TAX_COMP_GRID];            
            GridViewRow gVR=GridView_Inv_Tax_Comp.Rows[e.RowIndex];

            //dt.Rows[e.RowIndex]["Comp_Name"] = ((TextBox)gVR.Cells[0].FindControl("TextBox_Name_Edit")).Text;
            dt.Rows[e.RowIndex]["Comp_Value"] = ((TextBox)gVR.Cells[0].FindControl("TextBox_Value_Edit")).Text;
            
            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            whereCls.Add(BackEndObjects.DocFormat.DOCFORMAT_COL_CMP_ID, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            whereCls.Add(BackEndObjects.DocFormat.DOCFORMAT_COL_DOC_TYPE, BackEndObjects.DocFormat.DOCFORMAT_DOC_TYPE_INVOICE);
            whereCls.Add(BackEndObjects.DocFormat.DOCFORMAT_COL_SECTION_TYPE, BackEndObjects.DocFormat.DOCFORMAT_DOC_TYPE_INVOICE_SECTION_TYPE_TAX);
            whereCls.Add(BackEndObjects.DocFormat.DOCFORMAT_COL_SECTION_TYPE_NAME, ((Label)gVR.Cells[0].FindControl("Label_Name")).Text);

            Dictionary<String, String> targetVals = new Dictionary<string, string>();
            targetVals.Add(BackEndObjects.DocFormat.DOCFORMAT_COL_TEXT, ((TextBox)gVR.Cells[0].FindControl("TextBox_Value_Edit")).Text);

            BackEndObjects.DocFormat.updateDocFormatObjsDB(whereCls, targetVals, DBConn.Connections.OPERATION_UPDATE);
            
            Session[SessionFactory.ADMIN_PREF_DOCFORMAT_MGMT_TAX_COMP_GRID] = dt;
            GridView_Inv_Tax_Comp.EditIndex = -1;
            loadInvTaxCompGrid();
                        
        }


        }
    
}