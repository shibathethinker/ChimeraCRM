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
using System.IO;
using System.Web.UI.HtmlControls;

namespace OnLine.Pages
{
    public partial class AllContacts : System.Web.UI.Page
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            Page.Theme = Session[SessionFactory.LOGGED_IN_USER_THEME].ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING] == null)
                    Response.Redirect("Login.aspx");
                else
                {
                    ((HtmlGenericControl)(Master.FindControl("Accounts"))).Attributes.Add("class", "active");

                    String entId = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();
                    Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];
                    Dictionary<String, Currency> allCurrList = (Dictionary<String, Currency>)Session[SessionFactory.CURRENCY_LIST];
                    String defaultCurr = Session[SessionFactory.MAIN_BUSINESS_ENTITY_DEFAULT_CURRENCY] != null ?
                       Session[SessionFactory.MAIN_BUSINESS_ENTITY_DEFAULT_CURRENCY].ToString() : "";
                    Dictionary<String, String> allExistingContacts = (Dictionary<String, String>)Session[SessionFactory.EXISTING_CONTACT_DICTIONARY];
                    String theme = Session[SessionFactory.LOGGED_IN_USER_THEME].ToString();

                    Session.RemoveAll();
                    Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING] = entId;
                    Session[SessionFactory.LOGGED_IN_USER_ID_STRING] = User.Identity.Name;
                    Session[SessionFactory.ACCESSLIST_FOR_USER] = accessList;
                    Session[SessionFactory.CURRENCY_LIST] = allCurrList;
                    Session[SessionFactory.MAIN_BUSINESS_ENTITY_DEFAULT_CURRENCY] = defaultCurr;
                    Session[SessionFactory.EXISTING_CONTACT_DICTIONARY] = allExistingContacts;
                    Session[SessionFactory.LOGGED_IN_USER_THEME] = theme;

                    //((Menu)Master.FindControl("Menu1")).Items[6].Selected = true;
                    populateLogo();
                    CheckAccessToActions();
                    if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                    { //Full Access - no need to do any restriction
                        loadProdServList();
                        fillContactGrid();                        
                    }
                    else if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_CONTACTS_SCREEN_VIEW])
                    {
                        loadProdServList();
                        fillContactGrid();
                    }
                    else
                    {
                        Label_Contact_Screen_Access.Visible = true;
                        Label_Contact_Screen_Access.Text = "You don't have access to view this page";
                    }
                    
                }
            }
        }

        
        protected void populateLogo()
        {
            ArrayList imgListObjs = BackEndObjects.Image.getImagebyTypeandEntId(BackEndObjects.Image.PICTURE_IMG_TYPE_LOGO, Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            if (imgListObjs.Count > 0)
            {
                //Only consider the first image object for logo
                BackEndObjects.Image imgObj = (BackEndObjects.Image)imgListObjs[0];
                ((System.Web.UI.WebControls.Image)Master.FindControl("Image_Logo")).ImageUrl = imageToURL(imgObj.getImgPath());
                ((System.Web.UI.WebControls.Image)Master.FindControl("Image_Logo")).Visible = true;

            }
        }
        public String generateImagePath(String folderName)
        {
            String imgStoreRoot = "~/Images/SessionImages";
            imgStoreRoot = Server.MapPath(imgStoreRoot);

            String returnPath = "";
            try
            {
                if (!Directory.Exists(imgStoreRoot + "\\" + folderName.ToString()))
                    Directory.CreateDirectory(imgStoreRoot + "\\" + folderName.ToString());
                returnPath = imgStoreRoot + "\\" + folderName.ToString();
            }
            catch (Exception ex)
            {
                returnPath = "";

            }
            return returnPath;
        }
        public String imageToURL(string sPath)
        {
            /*System.Net.WebClient client = new System.Net.WebClient();
            byte[] imageData = client.DownloadData(sPath);
            System.IO.MemoryStream stream = new System.IO.MemoryStream(imageData);
            System.Drawing.Image img = System.Drawing.Image.FromStream(stream);*/

            String[] imageNameParts = null;
            if (sPath != null && !sPath.Equals(""))
                imageNameParts = sPath.Split(new String[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);

            String finalImageUrl = "~/Images/SessionImages/" + Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString() + "/" + imageNameParts[imageNameParts.Length - 1];

            if (!File.Exists(Server.MapPath(finalImageUrl)))
            {
                try
                {
                    System.Drawing.Image img = System.Drawing.Image.FromFile(sPath);
                    System.Drawing.Bitmap newBitMap = new System.Drawing.Bitmap(img);

                    generateImagePath(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

                    newBitMap.Save(Server.MapPath(finalImageUrl), System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                catch (Exception ex)
                {
                    finalImageUrl = "";
                }
            }

            return finalImageUrl;
        }

        /// <summary>
        /// This method will check access to different buttons and enable/disable based on access
        /// </summary>
        protected void CheckAccessToActions()
        {
            Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

            if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_CREATE_CONTACT]&&
                !accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                Button_Create_Req.Enabled = false;
        }

        protected DataTable fillContactGrid()
        {
            ArrayList contactList=Contacts.getAllContactsbyEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());

            Dictionary<String, String> existingContactNames = new Dictionary<String, String>();
            Dictionary<String, String> existingContactShortNames = new Dictionary<String, String>();
            Dictionary<String, String> existingContactEmailIds = new Dictionary<String, String>();
            Dictionary<String, String> existingContactPhNos = new Dictionary<String, String>();

            Dictionary<String, Dictionary<String, String>> existingContactDict = new Dictionary<string, Dictionary<string, string>>();
            
            DataTable dt = new DataTable();

            dt.Columns.Add("ContactEntId");
            dt.Columns.Add("ContactName");
            dt.Columns.Add("ShortName");
            dt.Columns.Add("PhNo");
            dt.Columns.Add("EmailId");
            dt.Columns.Add("FromSite");

            for (int i = 0; i < contactList.Count; i++)
            {
                Contacts contactObj = (Contacts)contactList[i];
                dt.Rows.Add();

                dt.Rows[i]["ContactEntId"] = contactObj.getContactEntityId();
                dt.Rows[i]["ContactName"] = contactObj.getContactName();
                dt.Rows[i]["ShortName"] = contactObj.getContactShortName();
                dt.Rows[i]["PhNo"] = contactObj.getMobNo();
                dt.Rows[i]["EmailId"] = contactObj.getEmailId();
                dt.Rows[i]["FromSite"] = contactObj.getFromSite();

                if (!existingContactNames.ContainsKey(((BackEndObjects.Contacts)contactList[i]).getContactName()) &&
    !((BackEndObjects.Contacts)contactList[i]).getContactName().Equals(""))
                    existingContactNames.Add(((BackEndObjects.Contacts)contactList[i]).getContactName(), "");

                if (!existingContactShortNames.ContainsKey(((BackEndObjects.Contacts)contactList[i]).getContactShortName()) &&
                    !((BackEndObjects.Contacts)contactList[i]).getContactShortName().Equals(""))
                    existingContactShortNames.Add(((BackEndObjects.Contacts)contactList[i]).getContactShortName(), "");

                if (!existingContactEmailIds.ContainsKey(((BackEndObjects.Contacts)contactList[i]).getEmailId()) &&
                    !((BackEndObjects.Contacts)contactList[i]).getEmailId().Equals(""))
                    existingContactEmailIds.Add(((BackEndObjects.Contacts)contactList[i]).getEmailId(), "");

                if (!existingContactPhNos.ContainsKey(((BackEndObjects.Contacts)contactList[i]).getMobNo()) &&
                    !((BackEndObjects.Contacts)contactList[i]).getMobNo().Equals(""))
                    existingContactPhNos.Add(((BackEndObjects.Contacts)contactList[i]).getMobNo(), "");

            }

            dt.DefaultView.Sort = "ContactName" + " " + "ASC";
            GridView1.Visible = true;
            GridView1.DataSource = dt.DefaultView.ToTable();
            GridView1.DataBind();
            GridView1.Columns[2].Visible = false;

            Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

            if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_EDIT_CONTACT] &&
                !accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                GridView1.Columns[1].Visible = false;

            Session[SessionFactory.ALL_CONTACT_DATA_GRID] = dt.DefaultView.ToTable();
            existingContactDict.Add("names", existingContactNames);
            existingContactDict.Add("shortnames", existingContactShortNames);
            existingContactDict.Add("emailids", existingContactEmailIds);
            existingContactDict.Add("phonenos", existingContactPhNos);
            Session[SessionFactory.ALL_CONTACT_EXISTING_CONTACT_LIST] = existingContactDict;

            return dt;
        }

        protected void loadProdServList()
        {
            Dictionary<String, ProductCategory> prodCatList = BackEndObjects.ProductCategory.getAllParentCategory();

            ListItem lt = new ListItem();
            lt.Text = "_";
            lt.Value = "_";
            DropDownList_Prod.Items.Add(lt);

            foreach (KeyValuePair<String, ProductCategory> kvp in prodCatList)
            {
                ListItem ltProdCat = new ListItem();
                ltProdCat.Text = ((ProductCategory)kvp.Value).getProductCategoryName();
                ltProdCat.Value = ((ProductCategory)kvp.Value).getCategoryId();
                DropDownList_Prod.Items.Add(ltProdCat);
            }
            DropDownList_Prod.SelectedValue = "_";
        }

        protected DataTable fillContactGridFiltered(String contactName,String shortName,String prodCat)
        {
            ArrayList contactList = Contacts.getAllContactsbyEntityIdDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            
            DataTable dt = new DataTable();

            dt.Columns.Add("ContactEntId");
            dt.Columns.Add("ContactName");
            dt.Columns.Add("ShortName");
            dt.Columns.Add("PhNo");
            dt.Columns.Add("EmailId");
            dt.Columns.Add("FromSite");

            int rowCount = 0;

            for (int i = 0; i < contactList.Count; i++)
            {
                Contacts contactObj = (Contacts)contactList[i];
                Boolean considerRecord = true;

                if (!contactName.Equals("") && contactObj.getContactName().IndexOf(contactName,StringComparison.OrdinalIgnoreCase)<0)
                    considerRecord=false;

                    if(!shortName.Equals("") && contactObj.getContactShortName().IndexOf(shortName,StringComparison.OrdinalIgnoreCase)<0)
                        considerRecord=false;

                    if (!prodCat.Equals("") && !prodCat.Equals("_") && !contactObj.getProdList().Contains(prodCat))
                        considerRecord = false;

                    if (considerRecord)
                    {
                        dt.Rows.Add();

                        dt.Rows[rowCount]["ContactEntId"] = contactObj.getContactEntityId();
                        dt.Rows[rowCount]["ContactName"] = contactObj.getContactName();
                        dt.Rows[rowCount]["ShortName"] = contactObj.getContactShortName();
                        dt.Rows[rowCount]["PhNo"] = contactObj.getMobNo();
                        dt.Rows[rowCount]["EmailId"] = contactObj.getEmailId();
                        dt.Rows[rowCount]["FromSite"] = contactObj.getFromSite();

                        rowCount++;
                    }
            }

            GridView1.Visible = true;
            Button_Notes_Contact.Enabled = false;
            Button_Audit_Contact.Enabled = false;
            GridView1.SelectedIndex = -1;
            GridView1.DataSource = dt;
            GridView1.DataBind();
            GridView1.Columns[2].Visible = false;

            Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

            if (!accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_EDIT_CONTACT] &&
                !accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                GridView1.Columns[1].Visible = false;

            Session[SessionFactory.ALL_CONTACT_DATA_GRID] = dt;
            return dt;
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataSource = (DataTable)Session[SessionFactory.ALL_CONTACT_DATA_GRID];
            GridView1.DataBind();
            GridView1.Columns[2].Visible = false;
            GridView1.SelectedIndex = -1;
            Button_Audit_Contact.Enabled = false;
            Button_Notes_Contact.Enabled = false;
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GridView1.DataSource = (DataTable)Session[SessionFactory.ALL_CONTACT_DATA_GRID];
            GridView1.DataBind();
            GridView1.Columns[2].Visible = false;
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            /*Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

            if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_EDIT_CONTACT]||
                accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
            {*/
                Label_Contact_Grid_Access.Visible = false;
                GridView1.EditIndex = e.NewEditIndex;
                GridView1.DataSource = (DataTable)Session[SessionFactory.ALL_CONTACT_DATA_GRID];
                GridView1.DataBind();
                GridView1.Columns[2].Visible = false;

                Label_Contact_Note.Text = "If the account is from this site then you can only edit the short name";
                Label_Contact_Note.Visible = true;
                Label_Contact_Note.ForeColor = System.Drawing.Color.Red;
         /*   }
            else
            {
                Label_Contact_Grid_Access.Visible = true;
                Label_Contact_Grid_Access.Text = "You dont have edit access to Contact records";
                GridView1.EditIndex = -1;
                GridView1.DataSource = (DataTable)Session[SessionFactory.ALL_CONTACT_DATA_GRID];
                GridView1.DataBind();

            }*/
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow gVR = GridView1.Rows[e.RowIndex];

            int index = GridView1.Rows[e.RowIndex].DataItemIndex;

            String contantEntId=((Label)gVR.Cells[0].FindControl("Label_Hidden")).Text;
            String entId=Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();

            DataTable dt = (DataTable)Session[SessionFactory.ALL_CONTACT_DATA_GRID];

            if (((Label)gVR.Cells[0].FindControl("Label_From_Site")).Text.Equals("Y"))
            {
                if (!((Label)gVR.Cells[0].FindControl("Label_Contact_Short_Name_Edit")).Text.
                    Equals(((TextBox)gVR.Cells[0].FindControl("TextBox_Contact_Short_Name")).Text))
                {
                    Dictionary<String, String> whereCls = new Dictionary<string, string>();
                    whereCls.Add(BackEndObjects.Contacts.CONTACT_TABLE_COL_CONTACT_ENTITY_ID, contantEntId);
                    whereCls.Add(BackEndObjects.Contacts.CONTACT_TABLE_COL_ENTITY_ID, entId);

                    Dictionary<String, String> targetVals = new Dictionary<string, string>();
                    targetVals.Add(BackEndObjects.Contacts.CONTACT_TABLE_COL_CONTACT_SHORT_NAME,((TextBox)gVR.Cells[0].FindControl("TextBox_Contact_Short_Name")).Text);

                    BackEndObjects.Contacts.updateContactDetailsDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);

                    String val = ((TextBox)gVR.Cells[0].FindControl("TextBox_Contact_Short_Name")).Text;
                    dt.Rows[index]["ShortName"] = ((TextBox)gVR.Cells[0].FindControl("TextBox_Contact_Short_Name")).Text;

                    //Session[SessionFactory.ALL_CONTACT_DATA_GRID] = dt;
                }
            }
            else
            {
                Dictionary<String, String> whereCls = new Dictionary<String, String>();
                Dictionary<String, String> targetVals = new Dictionary<String, String>();
                whereCls.Add(BackEndObjects.Contacts.CONTACT_TABLE_COL_CONTACT_ENTITY_ID, contantEntId);
                whereCls.Add(BackEndObjects.Contacts.CONTACT_TABLE_COL_ENTITY_ID, entId);

                Dictionary<String, Dictionary<String, String>> existingContactDict = (Dictionary<String, Dictionary<String, String>>)Session[SessionFactory.ALL_CONTACT_EXISTING_CONTACT_LIST];
                                    Dictionary<String, String> existingContactNames = null;
                    Dictionary<String, String> existingContactShortNames = null;
                    Dictionary<String, String> existingContactEmailIds = null;
                    Dictionary<String, String> existingContactPhNos = null;

                if (existingContactDict != null && existingContactDict.Count > 0)
                {
                    existingContactNames = existingContactDict["names"];
                    existingContactShortNames = existingContactDict["shortnames"];
                    existingContactEmailIds = existingContactDict["emailids"];
                    existingContactPhNos = existingContactDict["phonenos"];
                }


                if (!((Label)gVR.Cells[0].FindControl("Label_Contact_Short_Name_Edit")).Text.
    Equals(((TextBox)gVR.Cells[0].FindControl("TextBox_Contact_Short_Name")).Text))
                {
                    String val = ((TextBox)gVR.Cells[0].FindControl("TextBox_Contact_Short_Name")).Text;
                    if (existingContactShortNames != null && 
                        existingContactShortNames.ContainsKey(((TextBox)gVR.Cells[0].FindControl("TextBox_Contact_Short_Name")).Text))
                    {
                        Label_Contact_Note.Text = "Short Name already exists in your list";
                    }
                    else
                    {
                        dt.Rows[index]["ShortName"] = ((TextBox)gVR.Cells[0].FindControl("TextBox_Contact_Short_Name")).Text;
                        targetVals.Add(BackEndObjects.Contacts.CONTACT_TABLE_COL_CONTACT_SHORT_NAME, ((TextBox)gVR.Cells[0].FindControl("TextBox_Contact_Short_Name")).Text);
                        
                        if (existingContactShortNames != null && 
                            existingContactShortNames.ContainsKey(((TextBox)gVR.Cells[0].FindControl("TextBox_Contact_Short_Name")).Text))
                        existingContactShortNames.Add(((TextBox)gVR.Cells[0].FindControl("TextBox_Contact_Short_Name")).Text, "");
                    }
                }

                if (!((Label)gVR.Cells[0].FindControl("Label_Contact_Name_Edit")).Text.
    Equals(((TextBox)gVR.Cells[0].FindControl("TextBox_Contact_Name")).Text))
                {
                    if (existingContactNames != null &&
    existingContactNames.ContainsKey(((TextBox)gVR.Cells[0].FindControl("TextBox_Contact_Name")).Text))
                    {
                        Label_Contact_Note.Text = "Account Name already exists in your list";
                    }
                    else
                    {
                        dt.Rows[index]["ContactName"] = ((TextBox)gVR.Cells[0].FindControl("TextBox_Contact_Name")).Text;
                        targetVals.Add(BackEndObjects.Contacts.CONTACT_TABLE_COL_CONTACT_NAME, ((TextBox)gVR.Cells[0].FindControl("TextBox_Contact_Name")).Text);

                        if (existingContactNames != null &&
    existingContactNames.ContainsKey(((TextBox)gVR.Cells[0].FindControl("TextBox_Contact_Name")).Text))
                        existingContactNames.Add(((TextBox)gVR.Cells[0].FindControl("TextBox_Contact_Name")).Text, "");
                    }
                }

                if (!((Label)gVR.Cells[0].FindControl("Label_Ph_No_Edit")).Text.
    Equals(((TextBox)gVR.Cells[0].FindControl("TextBox_Ph_No")).Text))
                {
                    if (existingContactPhNos != null &&
existingContactPhNos.ContainsKey(((TextBox)gVR.Cells[0].FindControl("TextBox_Ph_No")).Text))
                    {
                        Label_Contact_Note.Text = "Phone No already exists in your list";
                    }
                    else
                    {
                        dt.Rows[index]["PhNo"] = ((TextBox)gVR.Cells[0].FindControl("TextBox_Ph_No")).Text;
                        targetVals.Add(BackEndObjects.Contacts.CONTACT_TABLE_COL_MOB_NO, ((TextBox)gVR.Cells[0].FindControl("TextBox_Ph_No")).Text);

                        if (existingContactPhNos != null &&
existingContactPhNos.ContainsKey(((TextBox)gVR.Cells[0].FindControl("TextBox_Ph_No")).Text))
                            existingContactPhNos.Add(((TextBox)gVR.Cells[0].FindControl("TextBox_Ph_No")).Text, "");
                    }
                }

                if (!((Label)gVR.Cells[0].FindControl("Label_Email_Id_Edit")).Text.
    Equals(((TextBox)gVR.Cells[0].FindControl("TextBox_Email_Id")).Text))
                {
                    if (existingContactEmailIds != null &&
existingContactEmailIds.ContainsKey(((TextBox)gVR.Cells[0].FindControl("TextBox_Email_Id")).Text))
                    {
                        Label_Contact_Note.Text = "Email id already exists in your list";
                    }
                    else
                    {
                        dt.Rows[index]["EmailId"] = ((TextBox)gVR.Cells[0].FindControl("TextBox_Email_Id")).Text;
                        targetVals.Add(BackEndObjects.Contacts.CONTACT_TABLE_COL_EMAIL_ID, ((TextBox)gVR.Cells[0].FindControl("TextBox_Email_Id")).Text);

                        if (existingContactEmailIds != null &&
existingContactEmailIds.ContainsKey(((TextBox)gVR.Cells[0].FindControl("TextBox_Email_Id")).Text))
                        existingContactEmailIds.Add(((TextBox)gVR.Cells[0].FindControl("TextBox_Email_Id")).Text, "");
                    }
                }

                try
                {
                    BackEndObjects.Contacts.updateContactDetailsDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
                }
                catch (Exception ex)
                {
                }

                existingContactDict.Clear();
                existingContactDict.Add("names", existingContactNames);
                existingContactDict.Add("shortnames", existingContactShortNames);
                existingContactDict.Add("emailids", existingContactEmailIds);
                existingContactDict.Add("phonenos", existingContactPhNos);
                Session[SessionFactory.ALL_CONTACT_EXISTING_CONTACT_LIST] = existingContactDict;
            }
            //Added this call as the DataTable was not getting refreshed properly - unknown root cause
            /*if (TextBox_Contact_Name.Equals("") && TextBox_Contact_ShortName.Equals("") && DropDownList_Prod.SelectedValue.Equals("_"))
                dt = fillContactGrid();
            else
                dt = fillContactGridFiltered(TextBox_Contact_Name.Text, TextBox_Contact_ShortName.Text, DropDownList_Prod.SelectedValue);*/

            
            GridView1.EditIndex = -1;
            GridView1.DataSource = dt;
            GridView1.DataBind();
            Session[SessionFactory.ALL_CONTACT_DATA_GRID] = dt;
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Button_Audit_Contact.Enabled = true;
            Button_Notes_Contact.Enabled = true;
            ((RadioButton)GridView1.SelectedRow.Cells[0].FindControl("contact_radio")).Checked = true;
        }

        protected void GridView1_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            Session[SessionFactory.ALL_CONTACT_SELECTED_CONTACT_ID] = ((Label)GridView1.Rows[e.NewSelectedIndex].Cells[0].FindControl("Label_Hidden")).Text;
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                GridViewRow gVR = e.Row;

                if (((Label)gVR.Cells[0].FindControl("Label_From_Site")).Text.Equals("Y"))
                {
                    ((TextBox)gVR.Cells[0].FindControl("TextBox_Contact_Name")).Visible = false;
                    ((Label)gVR.Cells[0].FindControl("Label_Contact_Short_Name_Edit")).Visible = false;
                    ((TextBox)gVR.Cells[0].FindControl("TextBox_Ph_No")).Visible = false;
                    ((TextBox)gVR.Cells[0].FindControl("TextBox_Email_Id")).Visible = false;
                }
                else
                {
                    ((Label)gVR.Cells[0].FindControl("Label_Contact_Name_Edit")).Visible = false;
                    ((Label)gVR.Cells[0].FindControl("Label_Contact_Short_Name_Edit")).Visible = false;
                    ((Label)gVR.Cells[0].FindControl("Label_Ph_No_Edit")).Visible = false;
                    ((Label)gVR.Cells[0].FindControl("Label_Email_Id_Edit")).Visible = false;
                }
            }
        }

 

        protected void Button_Refresh_Click(object sender, EventArgs e)
        {
            //fillContactGrid();

            String entId = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();

            foreach(GridViewRow gVR in GridView1.Rows)
            {
                if (((Label)gVR.Cells[0].FindControl("Label_From_Site")).Text.Equals("Y"))
                {
                    //Refresh the contacts which are taken from the site
                    String contactEntId = ((Label)gVR.Cells[0].FindControl("Label_Hidden")).Text;
                    
                    Dictionary<String, String> whereCls = new Dictionary<string, string>();
                    whereCls.Add(BackEndObjects.Contacts.CONTACT_TABLE_COL_CONTACT_ENTITY_ID,contactEntId);
                    whereCls.Add(BackEndObjects.Contacts.CONTACT_TABLE_COL_ENTITY_ID, entId);

                    MainBusinessEntity mBEObj = BackEndObjects.MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(contactEntId);
                    AddressDetails addrObj=AddressDetails.getAddressforMainBusinessEntitybyIdDB(mBEObj.getEntityId());
                    Dictionary<String,ProductCategory> mainProdList=MainBusinessEntity.getProductDetailsforMainEntitybyIdDB(contactEntId);

                    String prodList="";

                    foreach (KeyValuePair<String, ProductCategory> kvp in mainProdList)
                    prodList += kvp.Value.getCategoryId() + ",";
            
                    if(prodList.Length>0)
                    prodList=prodList.TrimEnd(',');

                    Dictionary<String, String> targetVals = new Dictionary<string, string>();
                    targetVals.Add(BackEndObjects.Contacts.CONTACT_TABLE_COL_EMAIL_ID, mBEObj.getEmailId());
                    targetVals.Add(BackEndObjects.Contacts.CONTACT_TABLE_COL_MOB_NO, mBEObj.getPhNo());
                    targetVals.Add(BackEndObjects.Contacts.CONTACT_TABLE_COL_CONTACT_NAME, mBEObj.getEntityName());
                    targetVals.Add(BackEndObjects.Contacts.CONTACT_TABLE_COL_LOCALITY_ID, addrObj.getLocalityId());
                    targetVals.Add(BackEndObjects.Contacts.CONTACT_TABLE_COL_STREET_NAME, addrObj.getAddrLine1());
                    targetVals.Add(BackEndObjects.Contacts.CONTACT_TABLE_COL_PROD_LIST, prodList);

                    BackEndObjects.Contacts.updateContactDetailsDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);

                }
                fillContactGrid();
            }
            Button_Audit_Contact.Enabled = false;
            Button_Notes_Contact.Enabled = false;

            GridView1.SelectedIndex = -1;
        }

        protected void Button_Filter_Contacts_Click(object sender, EventArgs e)
        {
                        Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];

                        if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_CONTACTS_SCREEN_VIEW]||
                            accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS])
                        {
                            fillContactGridFiltered(TextBox_Contact_Name.Text, TextBox_Contact_ShortName.Text, DropDownList_Prod.SelectedValue);
                        }
        }

        protected void LinkButton_Deal_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView1.SelectedIndex)
                GridView1.SelectRow(row.RowIndex);

            ((RadioButton)GridView1.SelectedRow.Cells[0].FindControl("contact_radio")).Checked = true;

            String forwardString = "/Pages/Popups/Contacts/AllDealsWithContact.aspx";
            forwardString += "?contactId=" + ((Label)GridView1.SelectedRow.Cells[0].FindControl("Label_Hidden")).Text;

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispContactDeals", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=1000,Height=700');", true);
        }

        protected void LinkButton_Defect_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView1.SelectedIndex)
                GridView1.SelectRow(row.RowIndex);

            ((RadioButton)GridView1.SelectedRow.Cells[0].FindControl("contact_radio")).Checked = true;

            String forwardString = "/Pages/Popups/Contacts/AllDefectsWithContact.aspx";
            forwardString += "?contactId=" + ((Label)GridView1.SelectedRow.Cells[0].FindControl("Label_Hidden")).Text;

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispContactDefects", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=1000,Height=700');", true);
        }

        protected void Button_Audit_Contact_Click(object sender, EventArgs e)
        {
            String forwardString = "/Pages/DispAudit.aspx";
            forwardString += "?contextId1=" + Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();
            forwardString += "&contextId2=" + ((Label)GridView1.SelectedRow.Cells[0].FindControl("Label_Hidden")).Text;
            forwardString += "&contextId3=" + "";
            forwardString += "&contextIdTable=" + "All_Contacts";

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispContactAudit", "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=no,width=900,Height=700');", true);

        }

        protected void Button_Notes_Contact_Click(object sender, EventArgs e)
        {
            String forwardString = "NoteDetails.aspx";

            String rfqId = ((Label)GridView1.SelectedRow.Cells[0].FindControl("Label_Hidden")).Text;

            forwardString += "?contextId=" + rfqId;

            ScriptManager.RegisterStartupScript(this, typeof(string), "ContactNotes",
                "window.open('" + forwardString + "',null,'resizeable=yes,scrollbars=yes,addressbar=no,toolbar=yes,width=800,height=800,left=500,right=500');", true);

        }

        protected void Button_Contact_Refresh_Hidden_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session[SessionFactory.ALL_CONTACT_DATA_GRID];
            GridView1.DataSource = dt;
            GridView1.DataBind();
            GridView1.SelectedIndex = -1;
            GridView1.Columns[2].Visible = false;
        }

        protected void GridView_Contact_RadioSelect(object sender, EventArgs e)
        {
            RadioButton linkb = (RadioButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1)
                GridView1.SelectRow(row.RowIndex);
        }

        protected void LinkButton_Location_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView1.SelectedIndex)
                GridView1.SelectRow(row.RowIndex);

            ((RadioButton)GridView1.SelectedRow.Cells[0].FindControl("contact_radio")).Checked = true;

            String forwardString = "Popups/Contacts/AllContacts_Location.aspx";
            String fromSite = ((Label)GridView1.SelectedRow.Cells[0].FindControl("Label_From_Site")).Text;

            forwardString += "?fromSite=" + fromSite;
            ScriptManager.RegisterStartupScript(this, typeof(string), "DispLocationContacts",
                "window.open('" + forwardString + "',null,'status=1,width=600,height=400,left=500,right=500');", true);
        }

        protected void LinkButton_Prd_Command(object sender, CommandEventArgs e)
        {
            LinkButton linkb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)linkb.NamingContainer;
            if (row != null && row.RowIndex != -1 && row.RowIndex != GridView1.SelectedIndex)
                GridView1.SelectRow(row.RowIndex);

            ((RadioButton)GridView1.SelectedRow.Cells[0].FindControl("contact_radio")).Checked = true;

            String forwardString = "Popups/Contacts/AllContacts_ProdSrv.aspx";
            String fromSite = ((Label)GridView1.SelectedRow.Cells[0].FindControl("Label_From_Site")).Text;

            forwardString += "?fromSite=" + fromSite;

            ScriptManager.RegisterStartupScript(this, typeof(string), "DispProdSrvContacts",
                "window.open('" + forwardString + "',null,'status=1,width=600,height=400,left=500,right=500');", true);
        }
    }
}