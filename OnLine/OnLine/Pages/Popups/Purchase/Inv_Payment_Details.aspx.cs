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
    public partial class Inv_Payment_Details : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                String rfId = Request.QueryString.GetValues("rfId")[0];
                String invId = Request.QueryString.GetValues("invId")[0];
                String invNo = Request.QueryString.GetValues("invNo")[0];
                String context = Request.QueryString.GetValues("context")[0];

                Label_Inv_No.Text = invNo;
                Label_RFQ_No.Text = rfId;
                loadPaymentTypeAndClearingStat();
                Dictionary<String,String> tranNoDict=fillPmntGrid();

                Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

                if (((context.Equals("client")&&!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_INVOICE_PURCHASE])
                    || (context.Equals("vendor") && !accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_INVOICE_SALES]) )
                    &&
    !accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                {
                    Label_Access.Visible = true;
                    Label_Access.Text = "You dont have edit access to Invoice records";
                    TextBox_Pmnt_Date.Enabled = false;
                    TextBox_Tran_Amnt.Enabled = false;
                    TextBox_Tran_No.Enabled = false;
                    Button_Create_Pmnt.Enabled = false;
                    GridView_Invoice_Pmnt.Columns[2].Visible = false;
                }

                Session[SessionFactory.ALL_INVOICE_PAYMENT_GRID_EXISTING_TRAN_NO] = tranNoDict;
            }
        }

        /// <summary>
        /// Returns the existing transaction number list
        /// </summary>
        /// <returns></returns>
        protected Dictionary<String,String> fillPmntGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Pmnt_Id");
            dt.Columns.Add("Tran_No");
            dt.Columns.Add("Pmnt_Type");
            dt.Columns.Add("Amount");
            dt.Columns.Add("Clearing_Stat");
            dt.Columns.Add("Clearing_Stat_Note");
            dt.Columns.Add("Pmnt_Date");
            dt.Columns.Add("updatedBy");

            String invId = Request.QueryString.GetValues("invId")[0];

            Dictionary<String,Payment> pmntDict=BackEndObjects.Payment.getPaymentDetailsforInvoiceDB(invId);
            Dictionary<String, String> tranDict = new Dictionary<String, String>();
            DateUtility dU = new DateUtility();

            int counter = 0;

            float totalPmnt = 0;
            float totalCleared = 0;
            float totalUnCleared = 0;

            foreach (KeyValuePair<String, Payment> kvp in pmntDict)
            {
                dt.Rows.Add();
                Payment pmntObj=kvp.Value;
                
                dt.Rows[counter]["Pmnt_Id"] = pmntObj.getPaymentId();
                dt.Rows[counter]["Tran_No"] = pmntObj.getTranNo();
                dt.Rows[counter]["Pmnt_Type"] = pmntObj.getPaymentType();
                dt.Rows[counter]["Amount"] = pmntObj.getAmount();
                dt.Rows[counter]["Clearing_Stat"] = pmntObj.getClearingStat();
                dt.Rows[counter]["Clearing_Stat_Note"] = pmntObj.getClearingStatNote();
                dt.Rows[counter]["Pmnt_Date"] = dU.getConvertedDate(pmntObj.getPmntDate());
                
                String enttId = pmntObj.getEntityid();
                String entName = "";
                entName = BackEndObjects.Contacts.getContactDetailsforContactEntityDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), enttId).getContactName();
                if (entName == null || entName.Equals(""))
                    entName = MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(enttId).getEntityName();

                dt.Rows[counter]["updatedBy"] = entName;

                totalPmnt += pmntObj.getAmount();
                totalCleared = pmntObj.getClearingStat().Equals(BackEndObjects.Payment.PAYMENT_CLEARING_STAT_CLEAR) ?
                    totalCleared + pmntObj.getAmount() : totalCleared;
                totalUnCleared = pmntObj.getClearingStat().Equals(BackEndObjects.Payment.PAYMENT_CLEARING_STAT_PENDING) ?
                    totalUnCleared + pmntObj.getAmount() : totalUnCleared;

                if(!tranDict.ContainsKey(pmntObj.getTranNo()))
                    tranDict.Add(pmntObj.getTranNo(),pmntObj.getTranNo());

                counter++;
            }

            dt.DefaultView.Sort = "Pmnt_Date" + " " + "DESC";
            GridView_Invoice_Pmnt.Visible = true;
            GridView_Invoice_Pmnt.DataSource = dt.DefaultView.ToTable();
            GridView_Invoice_Pmnt.DataBind();
            GridView_Invoice_Pmnt.Columns[2].Visible = false;

            Session[SessionFactory.ALL_INVOICE_PAYMENT_GRID] = dt;

            Label_Total_Pmnt_Made.Text = totalPmnt.ToString();
            
            Label_Total_Cleared.Text = totalCleared.ToString();
            Label_Total_Cleared.ForeColor = System.Drawing.Color.Green;

            Label_Total_Uncleared.Text = totalUnCleared.ToString();
            Label_Total_Uncleared.ForeColor = System.Drawing.Color.Red;

            return tranDict;
        }

        protected void loadPaymentTypeAndClearingStat()
        {
            ListItem lt = new ListItem();

            lt.Text = "_";
            lt.Value = "_";

            ListItem lt1 = new ListItem();
            lt1.Text = BackEndObjects.PaymentType.PAYMENT_TYPE_CASH;
            lt1.Value = BackEndObjects.PaymentType.PAYMENT_TYPE_CASH;

            ListItem lt2 = new ListItem();
            lt2.Text = BackEndObjects.PaymentType.PAYMENT_TYPE_CHEQUE;
            lt2.Value = BackEndObjects.PaymentType.PAYMENT_TYPE_CHEQUE;

            ListItem lt3 = new ListItem();
            lt3.Text = BackEndObjects.PaymentType.PAYMENT_TYPE_CREDIT_CARD;
            lt3.Text = BackEndObjects.PaymentType.PAYMENT_TYPE_CREDIT_CARD;

            ListItem lt4 = new ListItem();
            lt4.Text = BackEndObjects.PaymentType.PAYMENT_TYPE_DEBIT_CARD;
            lt4.Value = BackEndObjects.PaymentType.PAYMENT_TYPE_DEBIT_CARD;

            ListItem lt5 = new ListItem();
            lt5.Text = BackEndObjects.PaymentType.PAYMENT_TYPE_NET_BANKING;
            lt5.Value = BackEndObjects.PaymentType.PAYMENT_TYPE_NET_BANKING;

            DropDownList_Pmnt_Type.Items.Add(lt1);
            DropDownList_Pmnt_Type.Items.Add(lt2);
            DropDownList_Pmnt_Type.Items.Add(lt3);
            DropDownList_Pmnt_Type.Items.Add(lt4);
            DropDownList_Pmnt_Type.Items.Add(lt5);

            DropDownList_Pmnt_Type.SelectedValue = "_";

            ListItem ltC1 = new ListItem();
            ltC1.Text = BackEndObjects.Payment.PAYMENT_CLEARING_STAT_CLEAR;
            ltC1.Value = BackEndObjects.Payment.PAYMENT_CLEARING_STAT_CLEAR;

            ListItem ltC2 = new ListItem();
            ltC2.Text = BackEndObjects.Payment.PAYMENT_CLEARING_STAT_PENDING;
            ltC2.Value = BackEndObjects.Payment.PAYMENT_CLEARING_STAT_PENDING;

            DropDownList_Clearing_Stat.Items.Add(ltC1);
            DropDownList_Clearing_Stat.Items.Add(ltC2);

            DropDownList_Clearing_Stat.SelectedValue = BackEndObjects.Payment.PAYMENT_CLEARING_STAT_CLEAR;
        }

        protected void Button_Create_Pmnt_Click(object sender, EventArgs e)
        {
            Dictionary<String,String> existingTranDict=(Dictionary<String,String>) Session[SessionFactory.ALL_INVOICE_PAYMENT_GRID_EXISTING_TRAN_NO];
            if(existingTranDict.ContainsKey(TextBox_Tran_No.Text.Trim()))
            {
                Label_Pmnt_Create_Stat.Visible=true;
                Label_Pmnt_Create_Stat.Text="Transaction Number already exists.";
                Label_Pmnt_Create_Stat.ForeColor=System.Drawing.Color.Red;
            }
            else
            {
            BackEndObjects.Payment pmntObj = new BackEndObjects.Payment();

            String rfId = Request.QueryString.GetValues("rfId")[0];
            String invId = Request.QueryString.GetValues("invId")[0];

            pmntObj.setPaymentId(new BackEndObjects.Id().getNewId(BackEndObjects.Id.ID_TYPE_PMNT_ID_STRING));
            pmntObj.setClearingStat(DropDownList_Clearing_Stat.SelectedValue);
            pmntObj.setPmntDate(TextBox_Pmnt_Date.Text);
            pmntObj.setEntityId(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            pmntObj.setInvoiceId(invId);
            pmntObj.setPaymentType(DropDownList_Pmnt_Type.SelectedValue);
            pmntObj.setRFQId(rfId);
            pmntObj.setTranNo(TextBox_Tran_No.Text);
            pmntObj.setUsrId(User.Identity.Name);
            pmntObj.setAmount(float.Parse(TextBox_Tran_Amnt.Text));
            pmntObj.setClearingStatNote(TextBox_Clearing_Stat_Note.Text);

            try
            {
                BackEndObjects.Payment.insertPaymentDetailsDB(pmntObj);
                Label_Pmnt_Create_Stat.Visible = true;
                Label_Pmnt_Create_Stat.ForeColor = System.Drawing.Color.Green;
                Label_Pmnt_Create_Stat.Text = "Payment Details Inserted Successfully";

                DataTable dt = (DataTable)Session[SessionFactory.ALL_INVOICE_PAYMENT_GRID];
                dt.Rows.Add();
                int rowIndex = dt.Rows.Count-1;
                dt.Rows[rowIndex]["Pmnt_Id"] = pmntObj.getPaymentId();
                dt.Rows[rowIndex]["Tran_No"] = pmntObj.getTranNo();
                dt.Rows[rowIndex]["Pmnt_Type"] = pmntObj.getPaymentType();
                dt.Rows[rowIndex]["Amount"] = pmntObj.getAmount();
                dt.Rows[rowIndex]["Clearing_Stat"] = pmntObj.getClearingStat();
                dt.Rows[rowIndex]["Clearing_Stat_Note"] = pmntObj.getClearingStatNote();
                dt.Rows[rowIndex]["Pmnt_Date"] = pmntObj.getPmntDate();

                GridView_Invoice_Pmnt.DataSource = dt;
                GridView_Invoice_Pmnt.DataBind();
                Session[SessionFactory.ALL_INVOICE_PAYMENT_GRID] = dt;

                if (pmntObj.getClearingStat().Equals(BackEndObjects.Payment.PAYMENT_CLEARING_STAT_CLEAR))
                    Label_Total_Cleared.Text = (float.Parse(Label_Total_Cleared.Text) + pmntObj.getAmount()).ToString();
                if (pmntObj.getClearingStat().Equals(BackEndObjects.Payment.PAYMENT_CLEARING_STAT_PENDING))
                    Label_Total_Uncleared.Text = (float.Parse(Label_Total_Uncleared.Text) + pmntObj.getAmount()).ToString();

                Label_Total_Pmnt_Made.Text = (float.Parse(Label_Total_Pmnt_Made.Text) + pmntObj.getAmount()).ToString();
                //fillPmntGrid();
                
                existingTranDict.Add(pmntObj.getTranNo(),pmntObj.getTranNo());
                Session[SessionFactory.ALL_INVOICE_PAYMENT_GRID_EXISTING_TRAN_NO]=existingTranDict;
                
            }
            catch (Exception ex)
            {
                Label_Pmnt_Create_Stat.Visible = true;
                Label_Pmnt_Create_Stat.ForeColor = System.Drawing.Color.Red;
                Label_Pmnt_Create_Stat.Text = "Payment Details Creation Failed";
            }
            }
        }

        protected void GridView_Invoice_Pmnt_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView_Invoice_Pmnt.PageIndex = e.NewPageIndex;
            GridView_Invoice_Pmnt.DataSource = (DataTable)Session[SessionFactory.ALL_INVOICE_PAYMENT_GRID];
            GridView_Invoice_Pmnt.DataBind();

        }

        protected void GridView_Invoice_Pmnt_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView_Invoice_Pmnt.EditIndex = e.NewEditIndex;
            GridView_Invoice_Pmnt.DataSource = (DataTable)Session[SessionFactory.ALL_INVOICE_PAYMENT_GRID];
            GridView_Invoice_Pmnt.DataBind();
        }

        protected void GridView_Invoice_Pmnt_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                GridViewRow gVR = e.Row;
                ListItem lt1 = new ListItem();
                lt1.Text = BackEndObjects.PaymentType.PAYMENT_TYPE_CASH;
                lt1.Value = BackEndObjects.PaymentType.PAYMENT_TYPE_CASH;

                ListItem lt2 = new ListItem();
                lt2.Text = BackEndObjects.PaymentType.PAYMENT_TYPE_CHEQUE;
                lt2.Value = BackEndObjects.PaymentType.PAYMENT_TYPE_CHEQUE;

                ListItem lt3 = new ListItem();
                lt3.Text = BackEndObjects.PaymentType.PAYMENT_TYPE_CREDIT_CARD;
                lt3.Text = BackEndObjects.PaymentType.PAYMENT_TYPE_CREDIT_CARD;

                ListItem lt4 = new ListItem();
                lt4.Text = BackEndObjects.PaymentType.PAYMENT_TYPE_DEBIT_CARD;
                lt4.Value = BackEndObjects.PaymentType.PAYMENT_TYPE_DEBIT_CARD;

                ListItem lt5 = new ListItem();
                lt5.Text = BackEndObjects.PaymentType.PAYMENT_TYPE_NET_BANKING;
                lt5.Value = BackEndObjects.PaymentType.PAYMENT_TYPE_NET_BANKING;

                ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Pmnt_Type_Edit")).Items.Add(lt1);
                ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Pmnt_Type_Edit")).Items.Add(lt2);
                ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Pmnt_Type_Edit")).Items.Add(lt3);
                ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Pmnt_Type_Edit")).Items.Add(lt4);
                ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Pmnt_Type_Edit")).Items.Add(lt5);

                ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Pmnt_Type_Edit")).SelectedValue = ((Label)gVR.
                    Cells[0].FindControl("Label_Pmnt_Type_Edit")).Text;


                ListItem ltC1 = new ListItem();
                ltC1.Text = BackEndObjects.Payment.PAYMENT_CLEARING_STAT_CLEAR;
                ltC1.Value = BackEndObjects.Payment.PAYMENT_CLEARING_STAT_CLEAR;

                ListItem ltC2 = new ListItem();
                ltC2.Text = BackEndObjects.Payment.PAYMENT_CLEARING_STAT_PENDING;
                ltC2.Value = BackEndObjects.Payment.PAYMENT_CLEARING_STAT_PENDING;

                ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Clearing_Stat_Edit")).Items.Add(ltC1);
                ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Clearing_Stat_Edit")).Items.Add(ltC2);

                ((DropDownList)gVR.Cells[0].FindControl("DropDownList_Clearing_Stat_Edit")).SelectedValue = ((Label)gVR.
                   Cells[0].FindControl("Label_Clearing_Stat_Edit")).Text;

            }
        }

        protected void GridView_Invoice_Pmnt_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView_Invoice_Pmnt.EditIndex = -1;
            GridView_Invoice_Pmnt.DataSource = (DataTable)Session[SessionFactory.ALL_INVOICE_PAYMENT_GRID];
            GridView_Invoice_Pmnt.DataBind();
        }

        protected void GridView_Invoice_Pmnt_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int dataItemIndex = GridView_Invoice_Pmnt.Rows[e.RowIndex].DataItemIndex;
            DataTable dt = (DataTable)Session[SessionFactory.ALL_INVOICE_PAYMENT_GRID];

            String invId = Request.QueryString.GetValues("invId")[0];
            String pmntId=((Label)GridView_Invoice_Pmnt.Rows[e.RowIndex].Cells[0].FindControl("Label_Pmnt_Id")).Text;

            String tranNo = ((TextBox)GridView_Invoice_Pmnt.Rows[e.RowIndex].Cells[0].FindControl("TextBox_Tran_No_Edit")).Text;
           
            float amnt =float.TryParse(((TextBox)GridView_Invoice_Pmnt.Rows[e.RowIndex].Cells[0].FindControl("TextBox_Amnt_Edit")).Text,out amnt)?
                float.Parse(((TextBox)GridView_Invoice_Pmnt.Rows[e.RowIndex].Cells[0].FindControl("TextBox_Amnt_Edit")).Text):0;
            float previousAmnt = float.Parse(dt.Rows[dataItemIndex]["Amount"].ToString());

            String pmntType = ((DropDownList)GridView_Invoice_Pmnt.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Pmnt_Type_Edit")).SelectedValue;
            String clearingStat = ((DropDownList)GridView_Invoice_Pmnt.Rows[e.RowIndex].Cells[0].FindControl("DropDownList_Clearing_Stat_Edit")).SelectedValue;
            String clearingStatNote = ((TextBox)GridView_Invoice_Pmnt.Rows[e.RowIndex].Cells[0].FindControl("TextBox_Clearing_Stat_Note_Edit")).Text;
            String pmntDate = ((TextBox)GridView_Invoice_Pmnt.Rows[e.RowIndex].Cells[0].FindControl("TextBox_Pmnt_Date")).Text;


            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            whereCls.Add(BackEndObjects.Payment.PAYMENT_COL_PAYMENT_ID, pmntId);

            Dictionary<String, String> targetVals = new Dictionary<string, string>();
            targetVals.Add(BackEndObjects.Payment.PAYMENT_COL_AMOUNT, amnt.ToString());
            targetVals.Add(BackEndObjects.Payment.PAYMENT_COL_CLEARING_STAT, clearingStat);
            targetVals.Add(BackEndObjects.Payment.PAYMENT_COL_CLEARING_STAT_NOTE, clearingStatNote);
            targetVals.Add(BackEndObjects.Payment.PAYMENT_COL_PAYMENT_TYPE, pmntType);
            targetVals.Add(BackEndObjects.Payment.PAYMENT_COL_PMNT_DATE,new DateUtility().getDeConvertedDate(pmntDate));
            targetVals.Add(BackEndObjects.Payment.PAYMENT_COL_TRAN_NO, tranNo);

            BackEndObjects.Payment.updatePaymentDetailsDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
            GridView_Invoice_Pmnt.EditIndex = -1;
            
            dt.Rows[dataItemIndex]["Tran_No"] = tranNo;
            dt.Rows[dataItemIndex]["Pmnt_Type"] = pmntType;
            dt.Rows[dataItemIndex]["Amount"] = amnt;
            dt.Rows[dataItemIndex]["Clearing_Stat"] = clearingStat;
            dt.Rows[dataItemIndex]["Clearing_Stat_Note"] = clearingStatNote;
            dt.Rows[dataItemIndex]["Pmnt_Date"] = pmntDate;

            GridView_Invoice_Pmnt.DataSource = dt;
            GridView_Invoice_Pmnt.DataBind();
            Session[SessionFactory.ALL_INVOICE_PAYMENT_GRID] = dt;
            if (clearingStat.Equals(BackEndObjects.Payment.PAYMENT_CLEARING_STAT_CLEAR))
                Label_Total_Cleared.Text = (float.Parse(Label_Total_Cleared.Text) + (amnt-previousAmnt)).ToString();
            if (clearingStat.Equals(BackEndObjects.Payment.PAYMENT_CLEARING_STAT_PENDING))
                Label_Total_Uncleared.Text = (float.Parse(Label_Total_Uncleared.Text) + (amnt - previousAmnt)).ToString();

            Label_Total_Pmnt_Made.Text = (float.Parse(Label_Total_Pmnt_Made.Text) + (amnt - previousAmnt)).ToString();
            //fillPmntGrid();
        }

        protected void LinkButton_All_Audit_Rec_Command(object sender, CommandEventArgs e)
        {
            String forwardString = "/Pages/DispAudit.aspx";
            forwardString += "?contextId1=" + ((Label)GridView_Invoice_Pmnt.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("Label_Pmnt_Id")).Text;
            forwardString += "&contextId2=" + "";
            forwardString += "&contextId3=" + "";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispInvPmntAudit", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=700');", true);

        }

        protected void GridView_Invoice_Pmnt_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            String pmntId = ((Label)GridView_Invoice_Pmnt.Rows[e.RowIndex].Cells[0].FindControl("Label_Pmnt_Id")).Text;
            String amount = ((Label)GridView_Invoice_Pmnt.Rows[e.RowIndex].Cells[0].FindControl("Label_Amnt")).Text;
            String clearingStat = ((Label)GridView_Invoice_Pmnt.Rows[e.RowIndex].Cells[0].FindControl("Label_Clearing_Stat")).Text;
            String tranNo = ((Label)GridView_Invoice_Pmnt.Rows[e.RowIndex].Cells[0].FindControl("Label_Tran_No")).Text;

            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            Dictionary<String, String> targetVals = new Dictionary<string, string>();

            whereCls.Add(BackEndObjects.Payment.PAYMENT_COL_PAYMENT_ID, pmntId);
            BackEndObjects.Payment.updatePaymentDetailsDB(targetVals, whereCls, DBConn.Connections.OPERATION_DELETE);

            DataTable dt = (DataTable)Session[SessionFactory.ALL_INVOICE_PAYMENT_GRID];
            int index = GridView_Invoice_Pmnt.Rows[e.RowIndex].DataItemIndex;
            dt.Rows[index].Delete();

            GridView_Invoice_Pmnt.DataSource = dt;
            GridView_Invoice_Pmnt.DataBind();
            GridView_Invoice_Pmnt.SelectedIndex = -1;

            if (clearingStat.Equals(BackEndObjects.Payment.PAYMENT_CLEARING_STAT_CLEAR))
                Label_Total_Cleared.Text = (float.Parse(Label_Total_Cleared.Text) - float.Parse(amount)).ToString();
            if (clearingStat.Equals(BackEndObjects.Payment.PAYMENT_CLEARING_STAT_PENDING))
                Label_Total_Uncleared.Text = (float.Parse(Label_Total_Uncleared.Text) - float.Parse(amount)).ToString();

            Label_Total_Pmnt_Made.Text = (float.Parse(Label_Total_Pmnt_Made.Text) - float.Parse(amount)).ToString();

            Dictionary<String,String> existingTranDict= (Dictionary<string,string>)Session[SessionFactory.ALL_INVOICE_PAYMENT_GRID_EXISTING_TRAN_NO];
            existingTranDict.Remove(tranNo);
            Session[SessionFactory.ALL_INVOICE_PAYMENT_GRID_EXISTING_TRAN_NO] = existingTranDict;
        }


    }
}