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

namespace OnLine.Pages.Popups.Contacts
{
    public partial class AllContacts_ProdSrv : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                String contactEntId = Session[SessionFactory.ALL_CONTACT_SELECTED_CONTACT_ID].ToString();
                String[] fromSite = Request.QueryString.GetValues("fromSite");

                fillProdSrvDetails(contactEntId);

                                Dictionary<String, bool> accessList = (Dictionary<String, bool>)Session[SessionFactory.ACCESSLIST_FOR_USER];
                                if (accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS] ||
                                    accessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_EDIT_CONTACT])
                                {
                                    if (fromSite[0].Equals("N", StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        Buttin_Update.Enabled = true;
                                        loadProdServList();
                                    }
                                    else
                                    {
                                        Buttin_Update.Enabled = false;
                                        Label_Status.Visible = true;
                                        Label_Status.Text = "You can not edit contact details which are taken from the site";
                                        Label_Status.ForeColor = System.Drawing.Color.Red;
                                    }
                                }
                                else
                                {
                                    Buttin_Update.Enabled = false;
                                    Label_Status.Visible = true;
                                    Label_Status.Text = "You don't have edit access to contact records";
                                    Label_Status.ForeColor = System.Drawing.Color.Red;
                                }

            }
        }

        protected void fillProdSrvDetails(String contactEntId)
        {
            String prodList = BackEndObjects.Contacts.
                getContactDetailsforContactEntityDB(Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString(), contactEntId).getProdList();

            String[] prodArray = prodList.Split(new Char[] { ',' });
            String prodServiceText = "";

            for (int i = 0; i < prodArray.Length; i++)
            {
                if (prodArray[i] != null && !prodArray[i].Equals(""))
                    prodServiceText += BackEndObjects.ProductCategory.getProductCategorybyIdwoFeaturesDB(prodArray[i]).getProductCategoryName() + ",";
            }
            prodServiceText = prodServiceText.TrimEnd(',');

            if (!prodServiceText.Equals(""))
                Label_Main_Prd_Srv.Text = prodServiceText;

        }

        protected void loadProdServList()
        {
            Dictionary<String, ProductCategory> prodCatList = BackEndObjects.ProductCategory.getAllParentCategory();

            foreach (KeyValuePair<String, ProductCategory> kvp in prodCatList)
            {
                ListItem ltProdCat = new ListItem();
                ltProdCat.Text = ((ProductCategory)kvp.Value).getProductCategoryName();
                ltProdCat.Value = ((ProductCategory)kvp.Value).getCategoryId();
                ListBoxProdServc.Items.Add(ltProdCat);
            }
            ListBoxProdServc.SelectedIndex = -1;
        }

        protected void Buttin_Update_Click(object sender, EventArgs e)
        {
            int[] prodServ = ListBoxProdServc.GetSelectedIndices();
            String prodList = "";

            for (int i = 0; i < prodServ.Length; i++)
            {
                if (i < (prodServ.Length - 1))
                    prodList += ListBoxProdServc.Items[prodServ[i]].Value + ",";
                else
                    prodList += ListBoxProdServc.Items[prodServ[i]].Value;
            }

            Dictionary<String, String> whereCls = new Dictionary<string, string>();
            Dictionary<String, String> targetVals = new Dictionary<string, string>();

            whereCls.Add(BackEndObjects.Contacts.CONTACT_TABLE_COL_CONTACT_ENTITY_ID,
                Session[SessionFactory.ALL_CONTACT_SELECTED_CONTACT_ID].ToString());
            whereCls.Add(BackEndObjects.Contacts.CONTACT_TABLE_COL_ENTITY_ID,
                Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
            targetVals.Add(BackEndObjects.Contacts.CONTACT_TABLE_COL_PROD_LIST, prodList);
            try
            {
                BackEndObjects.Contacts.updateContactDetailsDB(targetVals, whereCls, DBConn.Connections.OPERATION_UPDATE);
                Label_Status.Visible = true;
                Label_Status.Text = "Update Successful";
                Label_Status.ForeColor = System.Drawing.Color.Green;
                String contactEntId = Session[SessionFactory.ALL_CONTACT_SELECTED_CONTACT_ID].ToString();
                fillProdSrvDetails(contactEntId);
            }
            catch (Exception ex)
            {
                Label_Status.Visible = true;
                Label_Status.Text = "Update Failed";
                Label_Status.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}