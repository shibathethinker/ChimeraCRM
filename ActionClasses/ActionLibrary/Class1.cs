using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBConn;
using BackEndObjects;
using System.Collections;
using ActionLibrary;
using System.Net;
using System.Net.Mail;

namespace ActionLibrary
{
    public class Emails
    {
        public static string smtpAddress = "smtp.gmail.com";
        public static int portNumber = 587;
        public static bool enableSSL = true;

        public static void sendEmail(String from, String fromPass, String to, String subject, String body)
        {
            MailMessage newEmail=new MailMessage();
            newEmail.From = new MailAddress(from.Trim());            
            newEmail.To.Add(to.Trim());
            newEmail.Body = body;
            newEmail.Subject = subject;
            newEmail.IsBodyHtml = true;

            SmtpClient smptClientObj = new SmtpClient(smtpAddress, portNumber);
            smptClientObj.Credentials = new NetworkCredential(from, fromPass);
            smptClientObj.EnableSsl = enableSSL;
            smptClientObj.Send(newEmail);
        }
    }
    public class LoginActions
    {
        private  static Dictionary<String, bool> getAllAccessListEntries()
        {
            Dictionary<String,bool> AccessList = new Dictionary<String,bool>();

            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_PURCHASE_SCREEN_VIEW, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_REQUIREMENT, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_CREATE_REQUIREMENT, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_REQUIREMENT, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_CONVERT_REQUIREMENT, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_RFQ, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_CREATE_RFQ, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_RFQ, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_INVOICE_PURCHASE, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_CREATE_INVOICE_PURCHASE, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_INVOICE_PURCHASE, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_PO_PURCHASE, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_CREATE_PO_PURCHASE, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_PO_PURCHASE, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_INVOICE_SALES, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_CREATE_INVOICE_SALES, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_INVOICE_SALES, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_SO_SALES, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_CREATE_SO_SALES, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_SO_SALES, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_SALES_SCREEN_VIEW, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_LEAD, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_CREATE_LEAD, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_LEAD, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_CONVERT_LEAD, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_POTENTIAL, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_CREATE_POTENTIAL, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_POTENTIAL, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_PRODUCTS_SCREEN_VIEW, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_CREATE_PRODUCT, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_EDIT_PRODUCT, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_DEFECTS_SCREEN_VIEW, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_CREATE_INCOMING_DEFECT, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_EDIT_INCOMING_DEFECT, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_CREATE_OUTGOING_DEFECT, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_EDIT_OUTGOING_DEFECT, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_SR_SCREEN_VIEW, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_CREATE_INCOMING_SR, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_EDIT_INCOMING_SR, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_CREATE_OUTGOING_SR, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_EDIT_OUTGOING_SR, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_DASHBOARD_SCREEN_VIEW, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_LEAD_REPORT, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_DOWNLOAD_LEAD_REPORT, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_POTENTIAL_REPORT, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_DOWNLOAD_POTENTIAL_REPORT, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_SALES_TRANSAC_REPORT, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_DOWNLOAD_SALES_TRANSAC_REPORT, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_PURCHASE_TRANSAC_REPORT, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_DOWNLOAD_PURCHASE_TRANSAC_REPORT, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_INCOMING_DEFECT_REPORT, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_DOWNLOAD_INCOMING_DEFECT_REPORT, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_OUTGOING_DEFECT_REPORT, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_DOWNLOAD_OUTGOING_DEFECT_REPORT, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_VIEW_CUSTOM_REPORT, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_DOWNLOAD_CUSTOM_REPORT, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_ADMIN_PREF_PAGE_VIEW, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_ADMIN_PREF_ENT_MGMT_WRITE_ACCESS, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_ADMIN_PREF_CHAIN_MGMT_WRITE_ACCESS, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_ADMIN_PREF_SEC_MGMT_WRITE_ACCESS, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_ADMIN_PREF_USR_MGMT_WRITE_ACCESS, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_ADMIN_PREF_WORKFLOW_MGMT_WRITE_ACCESS, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_CONTACTS_SCREEN_VIEW, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_CREATE_CONTACT, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCES_EDIT_CONTACT, false);
            AccessList.Add(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS, false);

            return AccessList;
        }
        /// <summary>
        /// This method retrieves the access details of the logged in user in a Dictionary
        /// The key defines the access context and the value is true/false.
        /// For any particular access context the resultant value is derived from a OR condition if the same access context is defined in multiple groups where the user 
        /// belongs to
        /// The first parameter is the user id and the second parameter is the main business entity id
        /// If the user has owner access this method does not do any other access check 
        /// </summary>
        /// <returns></returns>
        public Dictionary<String, bool> retrieveAccessList(String userId,String entId)
        {
            Dictionary<String, bool> AccessList = getAllAccessListEntries();

            String privString=BackEndObjects.userDetails.getUserDetailsbyIdDB(userId, entId).getPrivilege();
            if (privString.IndexOf(BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS) >= 0)
                AccessList[BackEndObjects.EntityAccessListRecord.ENTITY_ACCESS_LIST_RECORD_ACCESS_OWNER_ACCESS] =true;

            else
            {
                String[] privilegeStringArray = privString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < privilegeStringArray.Length; i++)
                {
                    ArrayList allowedContextListFortheGroup = BackEndObjects.EntityAccessListRecord.getAccessDetailsForGroupOrUserbyEntIdandGroupDB(entId, privilegeStringArray[i]);

                    for (int j = 0; j < allowedContextListFortheGroup.Count; j++)
                    {
                        if (AccessList.ContainsKey(allowedContextListFortheGroup[j].ToString()))
                            AccessList[allowedContextListFortheGroup[j].ToString()] = AccessList[allowedContextListFortheGroup[j].ToString()] || true;
                    }
                }
            }
            return AccessList;
        }
    }
    /// <summary>
    /// this action class can be used to process all reagistration related actions,
    /// this class stores all the business logics required to perform the registration related actions;
    /// it interacts with the backend objects namespace and the dbcon.
    /// </summary>
    public class RegistrationActions
    {
        Connections conn1 = new Connections();
        
        /// <summary>
        /// Only pass objects related to a single main business entity;
        ///Though for a given main business entity there can be multple related objects (e.g. subbusinessentity,image,user details,AddressDetails etc.) 
        ///which can be passed on to this method AND these should be passed as separate objects in the arraylist and not attached to the main business entity.
        ///ALSO, if other types of objects (subbusinessentity,image,user details etc.) are being sent along with the associated MainBusinessEntity in the arraylist,
        ///make sure to first get the associated MainBusinessEntity id.
        ///   ***********************************************************************************************************
        /// <para>the following types of objects can be passd in the arraylist as parameters (All OPTIONAL) </para>
        /// <para>a. MainBusinessEntity (only one can be processed) - NOTE THAT, if the 'entityId' property of the passed object is NULL/empty/not currently present in database
        /// then this method cosiders this as an INSERT request; otherwise it will be processed as an UPDATE.An update request to the associate 'product/service' category
        /// should be made only through this object (Because unlike the other possible parameters 'subBusiness'/'userDetails'/'Image') product or service
        /// details object does not have associated main business entity object.And, an update request for an associate 'ProductCategory' object will always result in a INSERT into table
        /// 'Shop_Main_Prdcts'.
        /// DO not attach the userDetails property to the main business entity. Rather send it as a separate object.
        /// </para>
        /// <para>b.subBusinessEntity (multiple objects - which mainly holds address details of the sub-chains under the main business entity)- NOTE THAT, 
        /// if the 'subEntityId' property of the passed object is NULL/empty/not currently present in database, then this method considers this as an INSERT request, otherwise it will be processed as an UPDATE -
        /// ** must have the 'mainBusinessId' property set for each of the subbusiness entity object passed;otherwise invalidParamException will be thrown**</para>
        /// <para>c.userDetails (multiple objects - which mainly holds the userid-password details for multiple users for all the sub business entities) - NOTE THAT,
        /// if the 'userId' property of the passed object is NULL/empty/not currently present in database, then this method considers this as an INSERT request, otherwise it will be processed as an UPDATE</para>
        /// <para> d.Image  (Multiple objects - which mainly holds the all the images associated with the MAIN BUSINESS entities - NOTE THAT 
        /// if the 'imgId' property of the passed object is  NULL/empty/not currently present in database, 
        /// then this method considers this as an INSERT request, otherwise it will be processed as an UPDATE</para>
        /// <para>If sending 'AddressDetails' object the sub entity id value must be set to AddressDetails.DUMMY_CHAIN_ID.This ensures that this address
        /// is considered for the associated Mainbusinessentity.Note that according to current design only one address object can be sent for a main business entity</para>
        /// ***********************************************************************************************************
        /// </summary>
        /// <param name="a"></param>
        public void completeRegr(ArrayList allObj)
        {
            ArrayList MainBusinessEntityObjs=new ArrayList();
            ArrayList subBusinessObjs=new ArrayList();
            ArrayList usrIdPassMapObjs=new ArrayList();
            ArrayList BusinessEntityImageObjs=new ArrayList();
            //This is introdcued to mainly address the need of associated address objects for main business entity
            ArrayList BusinessEntityAddrObjs = new ArrayList();

            Id IdGenerator = new Id();
            
            Dictionary<String, String> updateTargetVals = new Dictionary<string, string>();
            Dictionary<String, String> updateWhereVals = new Dictionary<string, string>();
            
            int i=0;
            while(i<allObj.Count)
            {
                if (allObj[i] is userDetails)
                    usrIdPassMapObjs.Add(allObj[i]);
                if (allObj[i] is subBusinessEntity)
                    subBusinessObjs.Add(allObj[i]);
                if (allObj[i] is MainBusinessEntity)
                    MainBusinessEntityObjs.Add(allObj[i]);
                if (allObj[i] is Image)
                    BusinessEntityImageObjs.Add(allObj[i]);
                if (allObj[i] is AddressDetails)
                    BusinessEntityAddrObjs.Add(allObj[i]);
                i++;                
            }
            
            if (MainBusinessEntityObjs.Count > 0)
            {
                MainBusinessEntity mBE = (MainBusinessEntity)MainBusinessEntityObjs[0];
                if (mBE.getEntityId() != null && MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(mBE.getEntityId()).getEntityId() != null) //This is an update request
                {
                    if (mBE.getDesc() != null)
                        updateTargetVals.Add(MainBusinessEntity.MAIN_BUSINESS_COL_DESC, mBE.getDesc());
                    if (mBE.getEmailId() != null)
                        updateTargetVals.Add(MainBusinessEntity.MAIN_BUSINESS_COL_EMAIL_ID, mBE.getEmailId());
                    if (mBE.getIndChain() != null)
                        updateTargetVals.Add(MainBusinessEntity.MAIN_BUSINESS_COL_IND_CHAIN, mBE.getIndChain());
                    if (mBE.getPhNo() != null)
                        updateTargetVals.Add(MainBusinessEntity.MAIN_BUSINESS_COL_MOBILE_NO, mBE.getPhNo());
                    if (mBE.getOwnerName() != null)
                        updateTargetVals.Add(MainBusinessEntity.MAIN_BUSINESS_COL_OWNER_NAME, mBE.getOwnerName());
                    if (mBE.getEntityName() != null)
                        updateTargetVals.Add(MainBusinessEntity.MAIN_BUSINESS_COL_SHOP_NAME, mBE.getEntityName());
                    if (mBE.getWebSite() != null)
                        updateTargetVals.Add(MainBusinessEntity.MAIN_BUSINESS_COL_WEBSITE, mBE.getWebSite());

                    if (mBE.getEntityId() != null)
                        updateWhereVals.Add(MainBusinessEntity.MAIN_BUSINESS_COL_BUSINESS_ID, mBE.getEntityId());
                   
                    MainBusinessEntity.updateMainBusinessEntityWOimg_prd_user_subDB(updateTargetVals, updateWhereVals, DBConn.Connections.OPERATION_UPDATE);

                    //insert product services
                    Dictionary<String, ProductCategory> tempData = mBE.getMainProductServices();
                    ProductCategory tempProd = new ProductCategory();
                    ArrayList tempList = new ArrayList();

                    if (tempData != null)
                        foreach (KeyValuePair<String, ProductCategory> kvp in tempData)
                        {
                            if (tempData.TryGetValue(kvp.Key, out tempProd))
                                tempList.Add(tempProd.getCategoryId());
                        }

                   if (tempList.Count > 0)
                        MainBusinessEntity.insertProductDetailsforEntityDB(mBE.getEntityId(), tempList);
                    
                    //Dictionary<String,ProductCategory> tempProdSrv=mBE.getMainProductServices();
                    //int prdSvcCount=0;
                    
                    //if(tempProdSrv!=null && tempProdSrv.Count>0) //update request for the associated product category mapping - it will always be serverd as DELETE
                        /*foreach (KeyValuePair<String, ProductCategory> kvp in tempProdSrv)
                        {
                            ProductCategory prodCatObj = new ProductCategory();

                            if (tempProdSrv.TryGetValue(kvp.Key, out prodCatObj))
                            {
                                if (prodCatObj.getCategoryId() != null && !prodCatObj.getCategoryId().Equals("") )
                                {
                                    updateWhereVals.Add(MainBusinessEntity.MAIN_BUSINESS_RELATED_PRODUCTS_COL_CATEGORY_ID, prodCatObj.getCategoryId());
                                    updateWhereVals.Add(MainBusinessEntity.MAIN_BUSINESS_RELATED_PRODUCTS_COL_BUSINESS_ID, mBE.getEntityId());

                                    MainBusinessEntity.updateProductDetailsforEntityDB(new Dictionary<string, string>(), updateWhereVals, DBConn.Connections.OPERATION_DELETE);
                                }

                            }

                        }*/


                }
                else  //This is an insert request need to get the new id generated before inserting into databaseE.
                {
                    if(mBE.getEntityId()==null||mBE.getEntityId().Equals("")) //Generate the id if the caller program did not generate it
                    mBE.setEntityId(IdGenerator.getNewId(Id.ID_TYPE_CMP_USR_STRING));
                    MainBusinessEntity.insertMainBusinessEntityWOimg_prd_user_subDB(mBE);  
                    
                    //Now Insert the related product/services details
                    Dictionary<String,ProductCategory> tempData=mBE.getMainProductServices();
                   ProductCategory tempProd=new ProductCategory();
                    ArrayList tempList=new ArrayList();

                    if(tempData!=null)
                    foreach(KeyValuePair <String,ProductCategory> kvp in tempData)
                    {
                        if(tempData.TryGetValue(kvp.Key,out tempProd))
                            tempList.Add(tempProd.getCategoryId());
                    }
                    
                    if(tempList.Count>0)
                    MainBusinessEntity.insertProductDetailsforEntityDB(mBE.getEntityId(),tempList);
                    //End of inserting related product/service details

                    //Start of inserting images for a main business entity
                    Dictionary<String, Image> tempImg = mBE.getImages();
                    Image tempImgObj = new Image();
                    ArrayList tempImgList = new ArrayList();
                    
                    if(tempImg!=null)
                    foreach (KeyValuePair<String, Image> kvp in tempImg)
                    {
                        if(tempImg.TryGetValue(kvp.Key,out tempImgObj))
                        {
                            tempImgList.Add(tempImgObj);
                        }
                    }

                    if (tempImgList.Count > 0)
                        Image.insertImageforEntityDB(mBE.getEntityId(), tempImgList);
                    //End of inserting images of a main business entity
                }
            }

            if (subBusinessObjs.Count > 0)
            {
                int j = 0;
                
                while (j < subBusinessObjs.Count)  //for each of the subbusiness entity either insert/update records
                {
                    ArrayList tempSubEnt = new ArrayList();
                    subBusinessEntity sBE = (subBusinessEntity)subBusinessObjs[j];
                    if (sBE.getMainBusinessId() == null)
                        throw new CustomExceptions.invalidParamException("Main business entity id missing in the sub business entity object");
                    
                    if (sBE.getSubEntityId() != null && subBusinessEntity.getSubBusinessEntitybyIdDB(sBE.getSubEntityId()).getSubEntityId() != null) //This is an update request
                    {
                        updateTargetVals = new Dictionary<string, string>();
                        updateWhereVals = new Dictionary<string, string>();  //Resetting the dictionaries before using again

                        if (sBE.getAddrLine1()!=null)
                        updateTargetVals.Add(subBusinessEntity.SUB_BUSINESS_COL_ADDR_LINE1, sBE.getAddrLine1());
                        if(sBE.getBaseCurrencyId()!=null)
                        updateTargetVals.Add(subBusinessEntity.SUB_BUSINESS_COL_BASE_CURR, sBE.getBaseCurrencyId());
                        if(sBE.getMainBusinessId()!=null)
                        updateTargetVals.Add(subBusinessEntity.SUB_BUSINESS_COL_BUSINESS_ID, sBE.getMainBusinessId());
                        //if(sBE.getSubEntityId()!=null)
                        //updateTargetVals.Add(subBusinessEntity.SUB_BUSINESS_COL_CHAIN_ID, sBE.getSubEntityId());
                        if(sBE.getSubEntityName()!=null)
                        updateTargetVals.Add(subBusinessEntity.SUB_BUSINESS_COL_CHAIN_NAME, sBE.getSubEntityName());
                        if(sBE.getContactName()!=null)
                        updateTargetVals.Add(subBusinessEntity.SUB_BUSINESS_COL_CONTACT_NAME, sBE.getContactName());
                        if(sBE.getSubEmailId()!=null)
                        updateTargetVals.Add(subBusinessEntity.SUB_BUSINESS_COL_EMAIL_ID, sBE.getSubEmailId());
                        if(sBE.getLocalityId()!=null)
                        updateTargetVals.Add(subBusinessEntity.SUB_BUSINESS_COL_LOCALITY_ID, sBE.getLocalityId());
                        if(sBE.getSubPhNo()!=null)
                        updateTargetVals.Add(subBusinessEntity.SUB_BUSINESS_COL_MOBILE_NO, sBE.getSubPhNo());
                        if(sBE.getSubRegstrNo()!=null)
                        updateTargetVals.Add(subBusinessEntity.SUB_BUSINESS_COL_REGSTR_NO, sBE.getSubRegstrNo());
                        if (sBE.getSubWebSite()!=null)
                        updateTargetVals.Add(subBusinessEntity.SUB_BUSINESS_COL_WEBSITE, sBE.getSubWebSite());

                        if (sBE.getSubEntityId() != null)
                            updateWhereVals.Add(subBusinessEntity.SUB_BUSINESS_COL_CHAIN_ID, sBE.getSubEntityId());

                        subBusinessEntity.updateSubBusinessEntityDB(updateTargetVals, updateWhereVals, DBConn.Connections.OPERATION_UPDATE);
                    }
                    else //This is an insert request, need to get the new id generated before inserting into database
                    {
                        if(sBE.getSubEntityId()==null ||sBE.getSubEntityId().Equals("")) //Generate the id if the caller program did not generate it
                        sBE.setSubEntityId(IdGenerator.getNewId(Id.ID_TYPE_SUB_BUS_STRING));
                        tempSubEnt.Add(sBE);
                        subBusinessEntity.insertSubBusinessEntityDB(tempSubEnt);
                    }
                    j++;
                }
            }

            if (usrIdPassMapObjs.Count > 0)
            {
                int k = 0;

                while (k < usrIdPassMapObjs.Count)
                {
                    userDetails uDE = (userDetails)usrIdPassMapObjs[k];
                    if (uDE.getMainEntityId() == null)
                        throw new CustomExceptions.invalidParamException("Main business entity id missing in the userDetails object");

                    if (uDE.getUserId() != null && userDetails.getUserDetailsbyIdDB(uDE.getUserId(), uDE.getMainEntityId()).getUserId() != null) //This is an update request
                    {
                        updateTargetVals = new Dictionary<string, string>();
                        updateWhereVals = new Dictionary<string, string>(); //Resetting the objects before reuse
                        
                        if(uDE.getMainEntityId()!=null)
                        updateTargetVals.Add(userDetails.USER_DETAILS_COL_BUSINESS_ID, uDE.getMainEntityId());
                        if(uDE.getSubEntityId()!=null)
                        updateTargetVals.Add(userDetails.USER_DETAILS_COL_CHAIN_ID, uDE.getSubEntityId());
                        if(uDE.getPassword()!=null)
                        updateTargetVals.Add(userDetails.USER_DETAILS_COL_PASSWORD, uDE.getPassword());
                        if(uDE.getPrivilege()!=null)
                        updateTargetVals.Add(userDetails.USER_DETAILS_COL_PRIVILEGE, uDE.getPrivilege());
                        //if(uDE.getUserId()!=null)
                        //updateTargetVals.Add(userDetails.USER_DETAILS_COL_USERID, uDE.getUserId());

                        updateWhereVals.Add(userDetails.USER_DETAILS_COL_USERID, uDE.getUserId());

                        userDetails.updateUserDetailsDB(updateTargetVals, updateWhereVals, DBConn.Connections.OPERATION_UPDATE);
                    }
                    else  //This is an insert request
                    {
                        userDetails.insertUserDetailsDB(uDE);
                    }
                    k++;
                }
                              
            }

            if (BusinessEntityImageObjs.Count > 0)
            {
                int l = 0;

                while(l<BusinessEntityImageObjs.Count)
                {
                    Image iDE = (Image)BusinessEntityImageObjs[l];

                    if (iDE.getEntityId() == null)
                        throw new CustomExceptions.invalidParamException("Main business entity id missing in Image object");

                    if (iDE.getImgId() != null && Image.getImagebyidDB(iDE.getImgId()).getImgId() != null) //This is an update request
                    {
                        Image.updateImageforEntityDB(iDE.getEntityId(), iDE.getImgId(), DBConn.Connections.OPERATION_UPDATE);
                    }
                    else //this is an insert request
                    {
                        ArrayList tempImgList = new ArrayList();
                        if (iDE.getImgId() == null || iDE.getImgId().Equals("")) //Generate the id if the caller program did not generate the id
                            iDE.setImgId(IdGenerator.getNewId(Id.ID_TYPE_IMAGE_ID_STRING));

                        tempImgList.Add(iDE);
                        Image.insertImageforEntityDB(iDE.getEntityId(),tempImgList);
                    }
                    l++;
                }

            }

            if (BusinessEntityAddrObjs.Count > 0)
            {
                //Sending multple address objects for a business entity may result in error as currently only one address object/main business entity is allowed in DB
                for(int j=0;j<BusinessEntityAddrObjs.Count;j++)
                {
                    AddressDetails addrObj=(AddressDetails)BusinessEntityAddrObjs[j];
                    if(addrObj.getMainBusinessId()==null ||addrObj.getMainBusinessId().Equals(""))
                        throw new CustomExceptions.invalidParamException("Empty Main Busines Entity Id sent with address Details object");
                    if(!addrObj.getSubEntityId().Equals(AddressDetails.DUMMY_CHAIN_ID))
                        throw new CustomExceptions.invalidParamException("Invalid chain id set for the main business entity");

                }
                ArrayList addrList=new ArrayList();
                addrList.Add(BusinessEntityAddrObjs[0]);
                  AddressDetails.insertAddressEntityDB(addrList);
                 }
            
        }


    }
    /// <summary>
    /// This class can be used to process all purchase related actions.
    /// This class stores all the business logics required to perform the purchase related actions;
    /// it interacts with backend object namespace and dbcon.
    /// </summary>
    public class PurchaseActions
    {
       // public static String RFQ_ACTIVE_STATUS="active";
        /// <summary>
        /// This inner class defines all the operations required to complete a new requirement.
        /// </summary>
        public class _createRequirements
        {

            /// <summary>
            /// This method returns all product/service category details for which there is no parent category;
            ///  i.e, it returns a list of all top level product/service category.This method is particularly useful for scenarios where it is required to
            ///  list the main catogories online.
            /// </summary>
            /// <returns></returns>
            public Dictionary<String, ProductCategory> getParentProductDetails()
            {
                return ProductCategory.getAllParentCategory();
            }
            /// <summary>
            /// for any given parent categoryid  returns all the associated child cateogories
            /// </summary>
            /// <returns></returns>
            public Dictionary<String, ProductCategory> getChildProductDetails(String catId)
            {
                return ProductCategory.getAllChildCategoryDB(catId);
            }
            /// <summary>
            /// for any given product category id, returns all the associated Feature objects.
            /// Each of the elements of the returned dictionary has a Feature object which in turn contains all the specifications attached to it.
            /// </summary>
            /// <param name="catId"></param>
            /// <returns></returns>
            public Dictionary<String, Features> getAllFeaturesForCategory(String catId)
            {
                Dictionary<String, Features> prodFeat = ProductCategory.getFeatureforCategoryDB(catId);

                foreach (KeyValuePair<String, Features> kvp in prodFeat)
                {
                    //Get all specifications related to each of the feature object
                    Dictionary<String, Specifications> tempSpec = Features.getSpecforFeatureDB(kvp.Key);

                    //Put all the specification objects in an arrayList , because feature object's specification property is an arraylist
                    ArrayList allSpecListforFeat = new ArrayList();
                    foreach (KeyValuePair<String, Specifications> kvps in tempSpec)
                        allSpecListforFeat.Add(kvps.Value);
                    //Add the specification ArrayList into the Feature object
                    kvp.Value.setSpecifications(allSpecListforFeat);
                }

                return prodFeat;
            }
            /// <summary>
            /// Returns all units of measurement from database. Each element of the returned ArrayList is an object of 'UnitOfMsrmnt'.
            /// Use 'getUnitName()' method on each of the containing objects to get the name of the units.
            /// </summary>
            /// <returns></returns>
            public ArrayList getAllUnitsOfMsrmnt()
            {
                return UnitOfMsrmnt.getAllMsrmntUnitsDB();
            }
            /// <summary>
            /// Returns an ArrayList which contains only the names of all the available currecnies in the database
            /// </summary>
            /// <returns></returns>
            public ArrayList getAllCurrencies()
            {
                return Currency.getAllCurrencyNamesDB();
            }
            /// <summary>
            /// Returns all entries of the database table 'Quote_Units'returns all the quote units objects from database.
            /// Each element of the returned entity has the property 'unitAndDivisor' which is a Dictionary<String,float> of a single record.
            /// use the 'getUnitAndDivisor'/'setUnitAndDivisor' as getter/setter for the property.
            /// The 'key' of the Dictionary<String,float> is 'unit name' and the 'value' is the respective divisor of the quote unit.
            /// </summary>
            /// <returns></returns>
            public ArrayList getAllQuoteUnits()
            {
                return QuoteUnits.getAllQuoteUnitsDB();
            }
            /// <summary>
            /// This method returns a dictionary; each element of the dictionary is the id of the country (the 'key') and the
            /// name of the country (the 'value').
            /// </summary>
            /// <returns></returns>
            public Dictionary<String, String> getAllCountryNames()
            {
                Dictionary<String, Country> countryDict = Country.getAllCountrywoStatesDB();
                Dictionary<String, String> countryNamesDict = new Dictionary<string, string>();

                ArrayList allCountryNames = new ArrayList();
                foreach (KeyValuePair<String, Country> kvp in countryDict)
                    countryNamesDict.Add(kvp.Key, kvp.Value.getCountryName());

                return countryNamesDict;
            }
            /// <summary>
            /// For a given country id,this method returns a dictionary; each element of the dictionary is the id of the State (the 'key') and the
            /// name of the State (the 'value').
            /// </summary>
            /// <param name="cId"></param>
            /// <returns></returns>
            public Dictionary<String, String> getAllStateNamesForCountry(String cId)
            {
                Dictionary<String, State> tempStateData = State.getStatesforCountryDB(cId);
                Dictionary<String, String> allStateNames = new Dictionary<string, string>();

                foreach (KeyValuePair<String, State> kvp in tempStateData)
                    allStateNames.Add(kvp.Key, kvp.Value.getStateName());

                return allStateNames;
            }
            /// <summary>
            /// For a given state id,this method returns a dictionary; each element of the dictionary is the id of the City (the 'key') and the
            /// name of the City (the 'value').
            /// </summary>
            /// <param name="stId"></param>
            /// <returns></returns>
            public Dictionary<String, String> getAllCityNamesForState(String stId)
            {
                Dictionary<String, City> tempCityData = City.getCitiesforStateDB(stId);
                Dictionary<String, String> allCityNames = new Dictionary<string, string>();

                foreach (KeyValuePair<String, City> kvp in tempCityData)
                    allCityNames.Add(kvp.Key, kvp.Value.getCityName());

                return allCityNames;

            }
            /// <summary>
            /// For a given City id,this method returns a dictionary; each element of the dictionary is the id of the Locality (the 'key') and the
            /// name of the Locality (the 'value').
            /// </summary>
            /// <param name="ctId"></param>
            /// <returns></returns>
            public Dictionary<String, String> getAllLocalityNamesForCity(String ctId)
            {
                Dictionary<String, Localities> tempLocalityData = Localities.getLocalitiesforCityDB(ctId);
                Dictionary<String, String> allLocalityNames = new Dictionary<string, string>();

                foreach (KeyValuePair<String, Localities> kvp in tempLocalityData)
                    allLocalityNames.Add(kvp.Key, kvp.Value.getLocalityName());

                return allLocalityNames;

            }
            /// <summary>
            /// This method inserts the requirement object into the database. If the passed object has specifications attached to it,
            /// those details are also inserted into the database.
            /// For a given requirement object there can be multiple 'Requirement_Spec' objects attached to it.
            /// This method returns the number of records created in the database. In case it returns 0, that means the insert operation failed.
            /// </summary>
            /// <param name="reqObj"></param>
            public int createNewRequirement(Requirement reqObj)
            {
                //Id IdGenerator = new Id();
                //reqObj.setReqId(IdGenerator.getNewId(Id.ID_TYPE_REQR_STRING));
                return Requirement.insertRequirementDB(reqObj);
            }
        }
        /// <summary>
        /// This class contains operations to display the requirement details 
        /// </summary>
        public class _dispRequirements
        {
            public String FILTER_BY_PROD_CAT = "FILTER_BY_PROD_CAT";
            public String FILTER_BY_STATUS = "FILTER_BY_STATUS";
            public String FILTER_BY_SUBMIT_DATE_FROM = "FILTER_BY_SUBMIT_DATE_FROM";
            public String FILTER_BY_SUBMIT_DATE_TO = "FILTER_BY_SUBMIT_DATE_TO";
            public String FILTER_BY_DUE_DATE_FROM = "FILTER_BY_DUE_DATE_FROM";
            public String FILTER_BY_DUE_DATE_TO = "FILTER_BY_DUE_DATE_TO";
            public String FILTER_BY_REQUIREMENT_NAME = "FILTER_BY_REQUIREMENT_NAME";

            /// <summary>
            /// For a given user id and company id, it returns an ArrayList of 'RequirementRecords' objects.
            /// Each of this returned object contains  Requirement object (with associated Requirement specifications) and associated RFQs.
            /// </summary>
            /// <param name="cmpUsrId"></param>
            /// <param name="userId"></param>
            /// <returns></returns>
            public ArrayList getAllRequirements(String cmpUsrId, String userId)
            {
                //The second parameter is not used as of now, but will be used once the role based access control logic is introduced

                if (cmpUsrId == null || cmpUsrId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid entity id value sent to method _dispRequirements.getAllRequirements"));

                if(userId==null || userId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid userId id value sent to method _dispRequirements.getAllRequirements"));

                ArrayList allReqrmnts = new ArrayList();
                ArrayList allReqrmntsUpdated = new ArrayList();

                allReqrmnts=Requirement.getAllRequirementsbyEntityIdDB(cmpUsrId);
                int i = 0;

                for (i = 0; i < allReqrmnts.Count; i++)
                {
                    Requirement tempReqr =(Requirement) allReqrmnts[i];
                    tempReqr.setReqSpecs(Requirement_Spec.getRequirementSpecsforReqbyIdDB(tempReqr.getReqId()));


                    RequirementRecords tempReqrRec = new RequirementRecords();
                    tempReqrRec.setActiveStat(tempReqr.getActiveStat());
                    tempReqrRec.setCreatedUsr(tempReqr.getCratedUsr());
                    tempReqrRec.setDueDate(tempReqr.getDueDate());
                    tempReqrRec.setEntityId(tempReqr.getEntityId());
                    tempReqrRec.setLocalId(tempReqr.getLocalId());
                    tempReqrRec.setReqId(tempReqr.getReqId());
                    tempReqrRec.setReqName(tempReqr.getReqName());
                    tempReqrRec.setReqSpecs(tempReqr.getReqSpecs());
                    tempReqrRec.setSubmitDate(tempReqr.getSubmitDate());
                    tempReqrRec.setTaggedRFQList(RFQDetails.getAllRFQbyRequirementIdDB(tempReqr.getReqId()));

                    allReqrmntsUpdated.Add(tempReqrRec);
                }

                return allReqrmntsUpdated;
            }
            /// <summary>
            /// This is same as 'getAllRequirements(String,String)' except, here there is a third parameter which can be used to specify the filter conditions.
            /// The third parameter is a dictionary which contains FILTER parameter name as 'key' and the respective filter value as the 'value'.
            /// Note that, all the filter parameters name must be chosen from the 'FILTER_BY_' properties of this class.
            /// Also, all the date values passed as filter parameter must be passed in 'yyyy-MM-dd' format.
            /// The tagged RFQs are not sent with the requirement list
            /// </summary>
            /// <param name="cmUsrId"></param>
            /// <param name="userId"></param>
            /// <param name="filterParam"></param>
            /// <returns></returns>
            public ArrayList getAllRequirementsFiltered(String cmpUsrId, String userId, Dictionary<String, String> filterParam)
            {
                if (cmpUsrId == null || cmpUsrId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid entity id value sent to method _dispRequirements.getAllRequirements"));

                if (userId == null || userId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid userId id value sent to method _dispRequirements.getAllRequirements"));


                ArrayList allReqrmnts = new ArrayList();
                ArrayList allReqrmntsUpdated = new ArrayList();

                allReqrmnts = Requirement.getAllRequirementsbyEntityIdDB(cmpUsrId);
                int i = 0;

                for (i = 0; i < allReqrmnts.Count; i++)
                {
                    Requirement tempReqr = (Requirement)allReqrmnts[i];
                    Boolean filterConditionSatisfied = true;
                   

                    if (filterConditionSatisfied)
                    {
                        RequirementRecords tempReqrRec = new RequirementRecords();

                        tempReqrRec.setActiveStat(tempReqr.getActiveStat());
                        tempReqrRec.setCreatedUsr(tempReqr.getCratedUsr());
                        tempReqrRec.setDueDate(tempReqr.getDueDate());
                        tempReqrRec.setEntityId(tempReqr.getEntityId());
                        tempReqrRec.setLocalId(tempReqr.getLocalId());
                        tempReqrRec.setReqId(tempReqr.getReqId());
                        tempReqrRec.setReqName(tempReqr.getReqName());
                        tempReqrRec.setReqSpecs(tempReqr.getReqSpecs());
                        tempReqrRec.setSubmitDate(tempReqr.getSubmitDate());
                        tempReqrRec.setCurrency(tempReqr.getCurrency());

                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_REQUIREMENT_NAME))
                            filterConditionSatisfied = tempReqrRec.getReqName().IndexOf(filterParam[this.FILTER_BY_REQUIREMENT_NAME], StringComparison.InvariantCultureIgnoreCase) >= 0 ? filterConditionSatisfied && true : false; 
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_STATUS))
                            filterConditionSatisfied = tempReqrRec.getActiveStat().Equals(filterParam[this.FILTER_BY_STATUS]) ? filterConditionSatisfied&&true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterConditionSatisfied&& filterParam.ContainsKey(this.FILTER_BY_DUE_DATE_FROM) && filterParam.ContainsKey(this.FILTER_BY_DUE_DATE_TO))
                        {
                            //Parse the due date as stored in the object
                            String dueDate = tempReqrRec.getDueDate().Replace(" ", "  ").Substring(0,9);
                            DateTime dueDateVal;
                            bool stat=DateTime.TryParseExact(dueDate.Trim(), "%M/%d/yyyy", null, DateTimeStyles.AllowWhiteSpaces, out dueDateVal);
                            
                                //parse the from due date as passed in the filter parameter
                                DateTime dueDateFromVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_DUE_DATE_FROM], "yyyy-MM-dd", null, DateTimeStyles.None, out dueDateFromVal);
                                DateTime dueDateToVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_DUE_DATE_TO], "yyyy-MM-dd", null, DateTimeStyles.None, out dueDateToVal);


                            if (DateTime.Compare(dueDateVal, dueDateFromVal) >= 0 && DateTime.Compare(dueDateVal, dueDateToVal) <= 0)
                                filterConditionSatisfied = filterConditionSatisfied && true;
                            else
                                filterConditionSatisfied = false;
                        }
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_SUBMIT_DATE_FROM) && filterParam.ContainsKey(this.FILTER_BY_SUBMIT_DATE_TO))
                        {
                            //Parse the due date as stored in the object
                            String subDate = tempReqrRec.getSubmitDate().Replace(" ", "  ").Substring(0, 9);
                            DateTime subDateVal;
                            DateTime.TryParseExact(subDate, "%M/%d/yyyy", null, DateTimeStyles.AllowWhiteSpaces, out subDateVal);

                            //parse the from due date as passed in the filter parameter
                            DateTime subDateFromVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_SUBMIT_DATE_FROM], "yyyy-MM-dd", null, DateTimeStyles.None, out subDateFromVal);
                            DateTime subDateToVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_SUBMIT_DATE_TO], "yyyy-MM-dd", null, DateTimeStyles.None, out subDateToVal);


                            if (DateTime.Compare(subDateVal, subDateFromVal) >= 0 && DateTime.Compare(subDateVal, subDateToVal) <= 0)
                                filterConditionSatisfied = filterConditionSatisfied && true;
                            else
                                filterConditionSatisfied = false;
                        }
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_PROD_CAT))
                    {
                        //ArrayList relatedSpecs = Requirement_Spec.getRequirementSpecsforReqbyIdDB(tempReqr.getReqId());
                        ArrayList relatedQntyList=RequirementProdServQnty.getRequirementProductServiceQuantityforReqIdDB(tempReqr.getReqId());
                        int j = 0;
                        filterConditionSatisfied = false;
                        //Even if one of the specificaion object matches the filter criteria, then allow this selection
                        for (j = 0; j < relatedQntyList.Count; j++)
                        {
                            RequirementProdServQnty tempSpec = (RequirementProdServQnty)relatedQntyList[j];
                            if (tempSpec.getProdCatId().Equals(filterParam[this.FILTER_BY_PROD_CAT]) || BackEndObjects.ProductCategory.getRootLevelParentCategoryDB(tempSpec.getProdCatId()).ContainsKey(filterParam[this.FILTER_BY_PROD_CAT]))
                            { filterConditionSatisfied = true; break; }
                        }

                        //if (filterConditionSatisfied)
                            //tempReqr.setReqSpecs(relatedQntyList);
                    }
                        if (filterConditionSatisfied)
                        {
                            //tempReqrRec.setTaggedRFQList(RFQDetails.getAllRFQbyRequirementIdDB(tempReqr.getReqId()));
                            allReqrmntsUpdated.Add(tempReqrRec);
                        }
                    }
                }

                return allReqrmntsUpdated;
            }
        }
        /// <summary>
        /// This class contains method to update requirement details.
        /// </summary>
        public class _updateRequirements
        {
            /// <summary>
            /// It is mandatory the pass the requirement object id attached to this method.Otherwise, this method will throw exception.
            /// The second parameter is the operation name ('update'/'delete').
            /// Attach only those property values to the requirement object which needs to be updated. Other than the requirement id,
            /// any other property value will be used for updating in case the second parameter value is 'DBConn.Connections.OPERATION_UPDATE'.
            /// Also, if the property 'requriementSpecList' is sent, those specification objects will also be modified.
            /// NOTE that, not all properties can be modified through this method - mainly  the create date/create user properties.
            /// A delete operation will have a cascading effect on the dependent tables and will remove the entire record from the requirement table.
            /// </summary>
            /// <param name="reqObj"></param>
            /// <returns></returns>
            public int updateRequirement(Requirement reqObj, String operation)
            {
               // Requirement tempReq=null ;
                //if (reqObj!=null)
                //tempReq = Requirement.getRequirementbyIdwoSpecsDB(reqObj.getReqId());

                if (reqObj==null||reqObj.getReqId() == null || reqObj.getReqId().Equals(""))
                    throw (new CustomExceptions.invalidParamException("Requirement Id not passed to method _updateRequirements.updateRequirement()"));
                else if (!operation.Equals(DBConn.Connections.OPERATION_UPDATE) && !operation.Equals(DBConn.Connections.OPERATION_DELETE))
                    throw (new CustomExceptions.invalidParamException("Invalid operation Name passed to method _updateRequirements.updateRequirement()"));

                Dictionary<String, String> updateTargetVals = new Dictionary<string, string>();
                Dictionary<String, String> updateWhereVals = new Dictionary<string, string>();

                int totalRecAffected = 0;

                if (operation.Equals(DBConn.Connections.OPERATION_UPDATE))
                {
                    ArrayList requriementSpecList = reqObj.getReqSpecs();

                    if (reqObj.getDueDate() != null)
                        updateTargetVals.Add(Requirement.REQ_COL_DUE_DATE, reqObj.getDueDate());
                    if (reqObj.getActiveStat() != null)
                        updateTargetVals.Add(Requirement.REQ_COL_ACTIVE_STAT, reqObj.getActiveStat());
                   // if (reqObj.getCratedUsr() != null)
                        //updateTargetVals.Add(Requirement.REQ_COL_CREATED_USR, reqObj.getCratedUsr());
                    if (reqObj.getEntityId() != null)
                        updateTargetVals.Add(Requirement.REQ_COL_ENTITY_ID, reqObj.getEntityId());
                    if (reqObj.getLocalId() != null)
                        updateTargetVals.Add(Requirement.REQ_COL_LOCAL_ID, reqObj.getLocalId());
                    if (reqObj.getReqName() != null)
                        updateTargetVals.Add(Requirement.REQ_COL_REQ_NAME, reqObj.getReqName());
                    //if (reqObj.getSubmitDate() != null)
                        //updateTargetVals.Add(Requirement.REQ_COL_SUBMIT_DATE, reqObj.getSubmitDate());

                    updateWhereVals.Add(Requirement.REQ_COL_REQ_ID, reqObj.getReqId());

                    totalRecAffected = Requirement.updateRequirementDB(updateTargetVals, updateWhereVals, operation);

                    if (requriementSpecList != null && requriementSpecList.Count > 0)
                    {
                        int count = 0;
                        while (count < requriementSpecList.Count)
                        {
                            Requirement_Spec reqSpecObj = (Requirement_Spec)requriementSpecList[count];

                            if (reqSpecObj.getProdCatId() == null || reqSpecObj.getProdCatId().Equals("") || reqSpecObj.getFeatId() == null || reqSpecObj.getFeatId().Equals(""))
                                throw (new CustomExceptions.invalidParamException("Product Category Id and Feature id necessary to update Requirement specification - method ActionLibrary.updateRequirement()"));

                            updateTargetVals = new Dictionary<string, string>();
                            updateWhereVals = new Dictionary<string, string>();

                           // if (reqSpecObj.getCreateDate() != null)
                                //updateTargetVals.Add(Requirement_Spec.REQ_SPEC_COL_CREATED_DATE, reqSpecObj.getCreateDate());
                            //if (reqSpecObj.getCreatedUser() != null)
                                //updateTargetVals.Add(Requirement_Spec.REQ_SPEC_COL_CREATED_USR, reqSpecObj.getCreatedUser());
                            //if (reqSpecObj.getFeatId() != null)
                            //updateTargetVals.Add(Requirement_Spec.REQ_SPEC_COL_FEAT_ID, reqSpecObj.getFeatId());
                            if (reqSpecObj.getFromSpecId() != null)
                                updateTargetVals.Add(Requirement_Spec.REQ_SPEC_COL_FROM_SPEC_ID, reqSpecObj.getFromSpecId());
                            if (reqSpecObj.getImgPath() != null) 
                                updateTargetVals.Add(Requirement_Spec.REQ_SPEC_COL_PIC_PATH, reqSpecObj.getImgPath().ToString());
                            if (reqSpecObj.getSpecText() != null)
                                updateTargetVals.Add(Requirement_Spec.REQ_SPEC_COL_SPEC_TXT, reqSpecObj.getSpecText());
                            if (reqSpecObj.getToSpecId() != null)
                                updateTargetVals.Add(Requirement_Spec.REQ_SPEC_COL_TO_SPEC_ID, reqSpecObj.getToSpecId());

                            updateWhereVals.Add(Requirement_Spec.REQ_SPEC_COL_REQ_ID, reqSpecObj.getReqId());
                            updateWhereVals.Add(Requirement_Spec.REQ_SPEC_COL_PROD_ID, reqSpecObj.getProdCatId());
                            updateWhereVals.Add(Requirement_Spec.REQ_SPEC_COL_FEAT_ID, reqSpecObj.getFeatId());

                            totalRecAffected += Requirement_Spec.updateRequirementSpecsDB(updateTargetVals, updateWhereVals, operation);

                            count++;
                        }
                    }
                }

                else if (operation.Equals(DBConn.Connections.OPERATION_DELETE))
                {
                    updateWhereVals.Add(Requirement.REQ_COL_REQ_ID, reqObj.getReqId());
                    totalRecAffected = Requirement.updateRequirementDB(updateTargetVals, updateWhereVals, operation);
                }

                return totalRecAffected;
            }
        }
        /// <summary>
        /// This class contains all necessary operations to create a new RFQ.
        /// Many of its methods are direct copy of respective methods from '_createRequirements' class.
        /// </summary>
        public class _createRFQ
        {
            /// <summary>
            /// This method returns all product/service category details for which there is no parent category;
            ///  i.e, it returns a list of all top level product/service category.This method is particularly useful for scenarios where it is required to
            ///  list the main catogories online.
            /// </summary>
            /// <returns></returns>
            public Dictionary<String, ProductCategory> getParentProductDetails()
            {
                return ProductCategory.getAllParentCategory();
            }
            /// <summary>
            /// for any given parent categoryid  returns all the associated child cateogories
            /// </summary>
            /// <returns></returns>
            public Dictionary<String, ProductCategory> getChildProductDetails(String catId)
            {
                return ProductCategory.getAllChildCategoryDB(catId);
            }
            /// <summary>
            /// for any given product category id, returns all the associated Feature objects.
            /// Each of the elements of the returned dictionary has a Feature object which in turn contains all the specifications attached to it.
            /// </summary>
            /// <param name="catId"></param>
            /// <returns></returns>
            public Dictionary<String, Features> getAllFeaturesForCategory(String catId)
            {
                Dictionary<String, Features> prodFeat = ProductCategory.getFeatureforCategoryDB(catId);

                foreach (KeyValuePair<String, Features> kvp in prodFeat)
                {
                    //Get all specifications related to each of the feature object
                    Dictionary<String, Specifications> tempSpec = Features.getSpecforFeatureDB(kvp.Key);

                    //Put all the specification objects in an arrayList , because feature object's specification property is an arraylist
                    ArrayList allSpecListforFeat = new ArrayList();
                    foreach (KeyValuePair<String, Specifications> kvps in tempSpec)
                        allSpecListforFeat.Add(kvps.Value);
                    //Add the specification ArrayList into the Feature object
                    kvp.Value.setSpecifications(allSpecListforFeat);
                }

                return prodFeat;
            }
            /// <summary>
            /// Returns all units of measurement from database. Each element of the returned ArrayList is an object of 'UnitOfMsrmnt'.
            /// Use 'getUnitName()' method on each of the containing objects to get the name of the units.
            /// </summary>
            /// <returns></returns>
            public ArrayList getAllUnitsOfMsrmnt()
            {
                return UnitOfMsrmnt.getAllMsrmntUnitsDB();
            }
            /// <summary>
            /// Returns an ArrayList which contains only the names of all the available currecnies in the database
            /// </summary>
            /// <returns></returns>
            public ArrayList getAllCurrencies()
            {
                return Currency.getAllCurrencyNamesDB();
            }
            /// <summary>
            /// Returns all entries of the database table 'Quote_Units'returns all the quote units objects from database.
            /// Each element of the returned entity has the property 'unitAndDivisor' which is a Dictionary<String,float> of a single record.
            /// use the 'getUnitAndDivisor'/'setUnitAndDivisor' as getter/setter for the property.
            /// The 'key' of the Dictionary<String,float> is 'unit name' and the 'value' is the respective divisor of the quote unit.
            /// </summary>
            /// <returns></returns>
            public ArrayList getAllQuoteUnits()
            {
                return QuoteUnits.getAllQuoteUnitsDB();
            }
            /// <summary>
            /// This method returns a dictionary; each element of the dictionary is the id of the country (the 'key') and the
            /// name of the country (the 'value').
            /// </summary>
            /// <returns></returns>
            public Dictionary<String, String> getAllCountryNames()
            {
                Dictionary<String, Country> countryDict = Country.getAllCountrywoStatesDB();
                Dictionary<String, String> countryNamesDict = new Dictionary<string, string>();

                ArrayList allCountryNames = new ArrayList();
                foreach (KeyValuePair<String, Country> kvp in countryDict)
                    countryNamesDict.Add(kvp.Key, kvp.Value.getCountryName());

                return countryNamesDict;
            }
            /// <summary>
            /// For a given country id,this method returns a dictionary; each element of the dictionary is the id of the State (the 'key') and the
            /// name of the State (the 'value').
            /// </summary>
            /// <param name="cId"></param>
            /// <returns></returns>
            public Dictionary<String, String> getAllStateNamesForCountry(String cId)
            {
                Dictionary<String, State> tempStateData = State.getStatesforCountryDB(cId);
                Dictionary<String, String> allStateNames = new Dictionary<string, string>();

                foreach (KeyValuePair<String, State> kvp in tempStateData)
                    allStateNames.Add(kvp.Key, kvp.Value.getStateName());

                return allStateNames;
            }
            /// <summary>
            /// For a given state id,this method returns a dictionary; each element of the dictionary is the id of the City (the 'key') and the
            /// name of the City (the 'value').
            /// </summary>
            /// <param name="stId"></param>
            /// <returns></returns>
            public Dictionary<String, String> getAllCityNamesForState(String stId)
            {
                Dictionary<String, City> tempCityData = City.getCitiesforStateDB(stId);
                Dictionary<String, String> allCityNames = new Dictionary<string, string>();

                foreach (KeyValuePair<String, City> kvp in tempCityData)
                    allCityNames.Add(kvp.Key, kvp.Value.getCityName());

                return allCityNames;

            }
            /// <summary>
            /// For a given City id,this method returns a dictionary; each element of the dictionary is the id of the Locality (the 'key') and the
            /// name of the Locality (the 'value').
            /// </summary>
            /// <param name="ctId"></param>
            /// <returns></returns>
            public Dictionary<String, String> getAllLocalityNamesForCity(String ctId)
            {
                Dictionary<String, Localities> tempLocalityData = Localities.getLocalitiesforCityDB(ctId);
                Dictionary<String, String> allLocalityNames = new Dictionary<string, string>();

                foreach (KeyValuePair<String, Localities> kvp in tempLocalityData)
                    allLocalityNames.Add(kvp.Key, kvp.Value.getLocalityName());

                return allLocalityNames;

            }
            /// <summary>
            /// This method inserts the RFQ object into the database. If the passed object has specifications attached to it,
            /// those details are also inserted into the database.
            /// For a given RFQ object there can be multiple 'Requirement_Spec' objects attached to it.
            /// This method returns the number of records created in the database. In case it returns 0, that means the insert operation failed.
            /// For a new RFQ, the creation Mode should always be 'AUTO' (RFQDetails.CREATION_MODE_AUTO) - this method enforces this rule
            /// irrespective of the value present for the passed creation mode.
            /// Also, this method enforces the rule that for Lead/RFQ entry created in AUTO mode, the createdEntity id is same as the entity id.
            /// </summary>
            /// <param name="rfqObj"></param>
            public int createNewRFQ(RFQDetails rfqObj)
            {
                if (rfqObj == null)
                    throw (new CustomExceptions.invalidParamException("Invalid 'RFQDetails' object sent to method _createRFQ.createNewRFQ"));
                //if (rfqObj.getCreateMode() == null || !rfqObj.getCreateMode().Equals(RFQDetails.CREATION_MODE_AUTO))
                    //throw (new CustomExceptions.businessRuleViolationException("An RFQ creation mode should be always be auto - from method _createRFQ.createNewRFQ"));

                rfqObj.setCreatedEntity(rfqObj.getEntityId());
                rfqObj.setCreateMode(RFQDetails.CREATION_MODE_AUTO);
                //Id IdGenerator = new Id();
                //rfqObj.setReqId(IdGenerator.getNewId(Id.ID_TYPE_RFQ_STRING));
                return RFQDetails.insertRFQDetailsDB(rfqObj);
            }
            /// <summary>
            /// Updates the list which specifies which user/users should be able to view the RFQ after it is broadcasted.
            /// </summary>
            /// <param name="rBL"></param>
            /// <returns></returns>
            public int insertRFQBroadcastDetails(RFQBroadcastList rBL)
            {
                return RFQBroadcastList.insertRFQBroadcastListDB(rBL);
            }
        }
        /// <summary>
        /// This class contains operations related to display of RFQ records on the purchase screen
        /// </summary>
        public class _dispRFQDetails
        {
            public String FILTER_BY_PROD_CAT = "FILTER_BY_PROD_CAT";
            public String FILTER_BY_APPROVAL_STATUS = "FILTER_BY_APPROVAL_STATUS";
            public String FILTER_BY_ACTIVE_STATUS = "FILTER_BY_ACTIVE_STATUS";
            public String FILTER_BY_SUBMIT_DATE_FROM = "FILTER_BY_SUBMIT_DATE_FROM";
            public String FILTER_BY_SUBMIT_DATE_TO = "FILTER_BY_SUBMIT_DATE_TO";
            public String FILTER_BY_DUE_DATE_FROM = "FILTER_BY_DUE_DATE_FROM";
            public String FILTER_BY_DUE_DATE_TO = "FILTER_BY_DUE_DATE_TO";
            public String FILTER_BY_RFQ_NO = "FILTER_BY_RFQ_NO";

            /// <summary>
            /// Given an entity id and the user id (who is trying to view the RFQ list), this method returns all the RFQ records available in the DB along with the Product/service 
            /// specifications.
            /// Each object of the returned ArrayList is instance of the class 'RFQDetails' with attached 'RFQProductServiceDetails' objects.
            /// </summary>
            /// <param name="cmpUsrId"></param>
            /// <param name="userId"></param>
            /// <returns></returns>
            public ArrayList getAllRFQDetails(String cmpUsrId, String userId)
            {
                if (cmpUsrId == null || cmpUsrId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid entity id value sent to method _dispRFQDetails.getAllRFQDetails"));

                if (userId == null || userId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid userId id value sent to method _dispRFQDetails.getAllRFQDetails"));

                ArrayList allRFQList = new ArrayList();
                //ArrayList allRFQListUpdated = new ArrayList();

                allRFQList = RFQDetails.getAllRFQbyEntityIdDB(cmpUsrId,true);

                for(int count=0;count<allRFQList.Count;count++)
                {
                    RFQDetails temp=(RFQDetails)allRFQList[count];

                    ArrayList rfqSpecList = RFQProductServiceDetails.getAllProductServiceDetailsbyRFQIdDB(temp.getRFQId());
                    //ArrayList rfqSpecList=new ArrayList();

                    //foreach(KeyValuePair<String,RFQProductServiceDetails> kvp in rfqSpecDict)
                        //rfqSpecList.Add(kvp.Value);

                    temp.setRFQProdServList(rfqSpecList);

                }
                

                return allRFQList;
           
            }
            /// <summary>
            /// This is same as 'getAllRFQDetails(String,String)' except, here there is a third parameter which can be used to specify the filter conditions.
            /// The third parameter is a dictionary which contains FILTER parameter name as 'key' and the respective filter value as the 'value'.
            /// Note that, all the filter parameters name must be chosen from the 'FILTER_BY_' properties of this class.
            /// Also, all the date values passed as filter parameter must be passed in 'yyyy-MM-dd' format.
            /// Each object of the returned ArrayList is instance of the class 'RFQDetails' with attached 'RFQProductServiceDetails' objects.
            /// </summary>
            /// <param name="cmpUsrId"></param>
            /// <param name="userId"></param>
            /// <param name="filterParam"></param>
            /// <returns></returns>
            public ArrayList getAllRFQDetailsFiltered(String cmpUsrId, String userId, Dictionary<String, String> filterParam)
            {
                if (cmpUsrId == null || cmpUsrId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid entity id value sent to method _dispRFQDetails.getAllRFQDetailsFiltered"));

                if (userId == null || userId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid userId id value sent to method _dispRFQDetails.getAllRFQDetailsFiltered"));


                ArrayList allRFQList = new ArrayList();
                ArrayList allRFQFiltered = new ArrayList();

                allRFQList = RFQDetails.getAllRFQbyEntityIdDB(cmpUsrId,true);
                int i = 0;

                for (i = 0; i < allRFQList.Count; i++)
                {
                    RFQDetails tempRFQ = (RFQDetails)allRFQList[i];
                    Boolean filterConditionSatisfied = true;


                    if (filterConditionSatisfied)
                    {
                        if (filterParam.ContainsKey(this.FILTER_BY_RFQ_NO))
                            filterConditionSatisfied = tempRFQ.getRFQId().IndexOf(filterParam[this.FILTER_BY_RFQ_NO],StringComparison.InvariantCultureIgnoreCase)>=0 ? filterConditionSatisfied && true : false;
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_APPROVAL_STATUS))
                            filterConditionSatisfied = tempRFQ.getApprovalStat().Equals(filterParam[this.FILTER_BY_APPROVAL_STATUS]) ? filterConditionSatisfied && true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_ACTIVE_STATUS))
                            filterConditionSatisfied = tempRFQ.getActiveStat().Equals(filterParam[this.FILTER_BY_ACTIVE_STATUS]) ? filterConditionSatisfied && true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_DUE_DATE_FROM) && filterParam.ContainsKey(this.FILTER_BY_DUE_DATE_TO))
                        {
                            //Parse the due date as stored in the object
                            String dueDate = tempRFQ.getDueDate().Substring(0, tempRFQ.getDueDate().IndexOf(" "));
                            DateTime dueDateVal;
                            DateTime.TryParseExact(dueDate, "%M/%d/yyyy", null, DateTimeStyles.AllowWhiteSpaces, out dueDateVal);

                            //parse the from due date as passed in the filter parameter
                            DateTime dueDateFromVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_DUE_DATE_FROM], "yyyy-MM-dd", null, DateTimeStyles.None, out dueDateFromVal);
                            DateTime dueDateToVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_DUE_DATE_TO], "yyyy-MM-dd", null, DateTimeStyles.None, out dueDateToVal);


                            if (DateTime.Compare(dueDateVal, dueDateFromVal) >= 0 && DateTime.Compare(dueDateVal, dueDateToVal) <= 0)
                                filterConditionSatisfied = filterConditionSatisfied && true;
                            else
                                filterConditionSatisfied = false;
                        }
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_SUBMIT_DATE_FROM) && filterParam.ContainsKey(this.FILTER_BY_SUBMIT_DATE_TO))
                        {
                            //Parse the due date as stored in the object
                            String subDate = tempRFQ.getSubmitDate().Substring(0, tempRFQ.getSubmitDate().IndexOf(" "));
                            DateTime subDateVal;
                            DateTime.TryParseExact(subDate, "%M/%d/yyyy", null, DateTimeStyles.AllowWhiteSpaces, out subDateVal);

                            //parse the from due date as passed in the filter parameter
                            DateTime subDateFromVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_SUBMIT_DATE_FROM], "yyyy-MM-dd", null, DateTimeStyles.None, out subDateFromVal);
                            DateTime subDateToVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_SUBMIT_DATE_TO], "yyyy-MM-dd", null, DateTimeStyles.None, out subDateToVal);


                            if (DateTime.Compare(subDateVal, subDateFromVal) >= 0 && DateTime.Compare(subDateVal, subDateToVal) <= 0)
                                filterConditionSatisfied = filterConditionSatisfied && true;
                            else
                                filterConditionSatisfied = false;
                        }
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_PROD_CAT))
                    {
                        ArrayList rfqpProdList = RFQProdServQnty.getRFQProductServiceQuantityforRFIdDB(tempRFQ.getRFQId());
                        int j = 0;
                        filterConditionSatisfied=false;
                        //Even if one of the specificaion object matches the filter criteria, then allow this selection
                        for (j = 0; j < rfqpProdList.Count; j++)
                        {
                            RFQProdServQnty tempSpec = (RFQProdServQnty)rfqpProdList[j];
                            if (tempSpec.getProdCatId().Equals(filterParam[this.FILTER_BY_PROD_CAT]) || BackEndObjects.ProductCategory.getRootLevelParentCategoryDB(tempSpec.getProdCatId()).ContainsKey(filterParam[this.FILTER_BY_PROD_CAT]))
                            { filterConditionSatisfied = true; break; }
                        }

                        //if (filterConditionSatisfied)
                            //tempRFQ.setRFQProdServList(rfqpProdList);
                    }

                        if (filterConditionSatisfied)
                            allRFQFiltered.Add(tempRFQ);
                    }
                }

                return allRFQFiltered;
            }
            /// <summary>
            /// For a given entity id and approver id this method filters out all the RFQ objects in an ArrayList which meets the filter criteria as mentioned in the
            /// filter parameter list
            /// </summary>
            /// <param name="cmpUsrId"></param>
            /// <param name="userId"></param>
            /// <param name="filterParam"></param>
            /// <returns></returns>
            public ArrayList getAllRFQDetailsFilteredForEntityandApproverId(String cmpUsrId, String userId, Dictionary<String, String> filterParam)
            {
                if (cmpUsrId == null || cmpUsrId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid entity id value sent to method _dispRFQDetails.getAllRFQDetailsFilteredForEntityandApproverId"));

                if (userId == null || userId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid userId id value sent to method _dispRFQDetails.getAllRFQDetailsFilteredForEntityandApproverId"));


                ArrayList allRFQList = new ArrayList();
                ArrayList allRFQFiltered = new ArrayList();

                allRFQList = RFQDetails.getAllRFQbyApproverIdDB(userId, cmpUsrId);
                int i = 0;

                for (i = 0; i < allRFQList.Count; i++)
                {
                    RFQDetails tempRFQ = (RFQDetails)allRFQList[i];
                    Boolean filterConditionSatisfied = true;


                    if (filterConditionSatisfied)
                    {
                        if (filterParam.ContainsKey(this.FILTER_BY_RFQ_NO))
                            filterConditionSatisfied = tempRFQ.getRFQId().IndexOf(filterParam[this.FILTER_BY_RFQ_NO], StringComparison.InvariantCultureIgnoreCase) >= 0 ? filterConditionSatisfied && true : false;
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_APPROVAL_STATUS))
                            filterConditionSatisfied = tempRFQ.getApprovalStat().Equals(filterParam[this.FILTER_BY_APPROVAL_STATUS]) ? filterConditionSatisfied && true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_ACTIVE_STATUS))
                            filterConditionSatisfied = tempRFQ.getActiveStat().Equals(filterParam[this.FILTER_BY_ACTIVE_STATUS]) ? filterConditionSatisfied && true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_DUE_DATE_FROM) && filterParam.ContainsKey(this.FILTER_BY_DUE_DATE_TO))
                        {
                            //Parse the due date as stored in the object
                            String dueDate = tempRFQ.getDueDate().Substring(0, tempRFQ.getDueDate().IndexOf(" "));
                            DateTime dueDateVal;
                            DateTime.TryParseExact(dueDate, "%M/%d/yyyy", null, DateTimeStyles.AllowWhiteSpaces, out dueDateVal);

                            //parse the from due date as passed in the filter parameter
                            DateTime dueDateFromVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_DUE_DATE_FROM], "yyyy-MM-dd", null, DateTimeStyles.None, out dueDateFromVal);
                            DateTime dueDateToVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_DUE_DATE_TO], "yyyy-MM-dd", null, DateTimeStyles.None, out dueDateToVal);


                            if (DateTime.Compare(dueDateVal, dueDateFromVal) >= 0 && DateTime.Compare(dueDateVal, dueDateToVal) <= 0)
                                filterConditionSatisfied = filterConditionSatisfied && true;
                            else
                                filterConditionSatisfied = false;
                        }
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_SUBMIT_DATE_FROM) && filterParam.ContainsKey(this.FILTER_BY_SUBMIT_DATE_TO))
                        {
                            //Parse the due date as stored in the object
                            String subDate = tempRFQ.getSubmitDate().Substring(0, tempRFQ.getSubmitDate().IndexOf(" "));
                            DateTime subDateVal;
                            DateTime.TryParseExact(subDate, "%M/%d/yyyy", null, DateTimeStyles.AllowWhiteSpaces, out subDateVal);

                            //parse the from due date as passed in the filter parameter
                            DateTime subDateFromVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_SUBMIT_DATE_FROM], "yyyy-MM-dd", null, DateTimeStyles.None, out subDateFromVal);
                            DateTime subDateToVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_SUBMIT_DATE_TO], "yyyy-MM-dd", null, DateTimeStyles.None, out subDateToVal);


                            if (DateTime.Compare(subDateVal, subDateFromVal) >= 0 && DateTime.Compare(subDateVal, subDateToVal) <= 0)
                                filterConditionSatisfied = filterConditionSatisfied && true;
                            else
                                filterConditionSatisfied = false;
                        }
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_PROD_CAT))
                        {
                            ArrayList rfqpProdList = RFQProdServQnty.getRFQProductServiceQuantityforRFIdDB(tempRFQ.getRFQId());
                            int j = 0;
                            filterConditionSatisfied = false;
                            //Even if one of the specificaion object matches the filter criteria, then allow this selection
                            for (j = 0; j < rfqpProdList.Count; j++)
                            {
                                RFQProdServQnty tempSpec = (RFQProdServQnty)rfqpProdList[j];
                                if (tempSpec.getProdCatId().Equals(filterParam[this.FILTER_BY_PROD_CAT]) || BackEndObjects.ProductCategory.getRootLevelParentCategoryDB(tempSpec.getProdCatId()).ContainsKey(filterParam[this.FILTER_BY_PROD_CAT]))
                                { filterConditionSatisfied = true; break; }
                            }

                            //if (filterConditionSatisfied)
                            //tempRFQ.setRFQProdServList(rfqpProdList);
                        }

                        if (filterConditionSatisfied)
                            allRFQFiltered.Add(tempRFQ);
                    }
                }

                return allRFQFiltered;
            }
            /// <summary>
            /// This methid return all response for an RFQ.
            /// This method returns a Dictionary of 'String' and 'RFQReponseRecord' objects. The 'key' is the response entity id.
            /// </summary>
            /// <param name="cmpUsrId"></param>
            /// <param name="userId"></param>
            /// <param name="rfId"></param>
            /// <returns></returns>
            public Dictionary<String, RFQResponseRecord> viewRFQResponses(String cmpUsrId, String userId, String rfId)
            {
                //The first two parameters are not used as of now, but they are still expected to contain future enchancements
                if (cmpUsrId == null || cmpUsrId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid entity id value sent to method _dispRFQDetails.viewRFQResponses"));

                if (userId == null || userId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid userId id value sent to method _dispRFQDetails.viewRFQResponses"));

                if (rfId == null || rfId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid rfqId id value sent to method _dispRFQDetails.viewRFQResponses"));

                Dictionary<String,RFQResponse> rfqResp=RFQResponse.getAllRFQResponseforRFQIdDB(rfId);
                Dictionary<String, RFQResponseRecord> retObj = new Dictionary<string, RFQResponseRecord>();
                foreach(KeyValuePair<String,RFQResponse> kvp in rfqResp)
                {
                    RFQResponse tempRec = kvp.Value;
                    RFQResponseRecord respRecObj = new RFQResponseRecord();

                    respRecObj.setRFQId(tempRec.getRFQId());
                    respRecObj.setRespEntityId(tempRec.getRespEntityId());
                    respRecObj.setRespDate(tempRec.getRespDate());
                    respRecObj.setNdaPath(tempRec.getNdaPath());
                    respRecObj.findAndSetEntityName();

                    retObj.Add(tempRec.getRespEntityId(), respRecObj);
                }
                return retObj;
            }
            /// <summary>
            /// This method returns an object of 'RFQResponseQuoteTotal'.
            /// For a particular RFQ, this method returns the respective response quote details given a particular response entity id. 
            /// </summary>
            /// <param name="cmpUsrId"></param>
            /// <param name="userId"></param>
            /// <param name="responseEntId"></param>
            /// <param name="rfId"></param>
            /// <returns></returns>
            public RFQResponseQuoteTotal viewRFQResponseQuoteDetail(String cmpUsrId,String userId,String responseEntId,String rfId)
            {
                                //The first two parameters are not used as of now, but they are still expected to contain future enchancements
                if (cmpUsrId == null || cmpUsrId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid entity id value sent to method _dispRFQDetails.viewRFQResponseQuoteDetail"));

                if (userId == null || userId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid userId id value sent to method _dispRFQDetails.viewRFQResponseQuoteDetail"));

                if (rfId == null || rfId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid rfqId id value sent to method _dispRFQDetails.viewRFQResponseQuoteDetail"));

                if (responseEntId == null || responseEntId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid response entity id value sent to method _dispRFQDetails.viewRFQResponseQuoteDetail"));

                RFQResponseQuoteTotal completeQuoteResp=new RFQResponseQuoteTotal();
                ArrayList tempList=new ArrayList();

                Dictionary<String,RFQResponseQuotes> allRespQut=RFQResponseQuotes.getAllResponseQuotesforRFQandResponseEntityDB(rfId,responseEntId);

                foreach(KeyValuePair <String,RFQResponseQuotes> kvp in allRespQut)
                {
                    RFQResponseQuotes tempRec=kvp.Value;

                    RFQResponseQuoteRecord tempQuoteRec=new RFQResponseQuoteRecord();
                    tempQuoteRec.setUnitName(tempRec.getUnitName());
                    tempQuoteRec.setRFQId(tempRec.getRFQId());
                    tempQuoteRec.setResponseUsrId(tempRec.getResponseUsrId());
                    tempQuoteRec.setResponseEntityId(tempRec.getResponseEntityId());
                    tempQuoteRec.setQuote(tempRec.getQuote());
                    tempQuoteRec.setPrdCatId(tempRec.getPrdCatId());

                    tempQuoteRec.setAmntQuntyandProdSpec();
                    tempList.Add(tempQuoteRec);

                }
                completeQuoteResp.setRFQResponseQuoteRecordList(tempList);
                completeQuoteResp.calculateTotalAmnt();

                return completeQuoteResp;
            }
            /// <summary>
            /// Returns a dictionary containing the 'entity id' and 'entity name'.
            /// </summary>
            /// <param name="cmpUsrId"></param>
            /// <param name="userId"></param>
            /// <param name="rfId"></param>
            /// <returns></returns>
            public Dictionary<String,String> getRFQBroadCastList(String cmpUsrId, String userId,String rfId)
            {
                if (cmpUsrId == null || cmpUsrId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid entity id value sent to method _dispRFQDetails.getRFQBroadCastList"));

                if (userId == null || userId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid userId id value sent to method _dispRFQDetails.getRFQBroadCastList"));

                if (rfId == null || rfId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid rfqId id value sent to method _dispRFQDetails.getRFQBroadCastList"));

                RFQBroadcastList tempList = RFQBroadcastList.getRFQBroadcastListbyIdDB(rfId);
                ArrayList broadCastidList=tempList.getBroadcastList();
                Dictionary<String, String> broadCastList = new Dictionary<string, string>();
               
                for (int count = 0; count < broadCastidList.Count; count++)
                {
                    String brdId=broadCastidList[count].ToString();
                    if (!brdId.Equals(RFQBroadcastList.RFQ_BROADCAST_TO_ALL) && !brdId.Equals(RFQBroadcastList.RFQ_BROADCAST_TO_ALL_INTERESTED))
                        broadCastList.Add(brdId, Contacts.getContactDetailsforContactEntityDB(cmpUsrId, brdId).getContactName());
                    else
                        broadCastList.Add(brdId, brdId);
                }
                return broadCastList;
            }
            /// <summary>
            /// This method returns all Shortlisted entries for a particular RFQ.
            /// This method returns a Dictionary of 'String' and 'RFQReponseRecord' objects. The 'key' is the response entity id.
            /// </summary>
            /// <param name="cmpUsrId"></param>
            /// <param name="userId"></param>
            /// <param name="rfId"></param>
            /// <returns></returns>
            public Dictionary<String, RFQResponseRecord> viewRFQResponsesForShortListedEntries(String cmpUsrId, String userId, String rfId)
            {
                //The first two parameters are not used as of now, but they are still expected to contain future enchancements
                if (cmpUsrId == null || cmpUsrId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid entity id value sent to method _dispRFQDetails.viewRFQResponsesForShortListedEntries"));

                if (userId == null || userId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid userId id value sent to method _dispRFQDetails.viewRFQResponsesForShortListedEntries"));

                if (rfId == null || rfId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid rfqId id value sent to method _dispRFQDetails.viewRFQResponsesForShortListedEntries"));

                ArrayList rfqResp = RFQShortlisted.getAllShortListedEntriesbyRFQId(rfId); ;
                
                Dictionary<String, RFQResponseRecord> retObj = new Dictionary<string, RFQResponseRecord>();

                for (int count = 0; count < rfqResp.Count; count++)
                {
                    RFQShortlisted tempShortListedRec = (RFQShortlisted)rfqResp[count];
                    RFQResponse tempRec = RFQResponse.getRFQResponseforRFQIdandResponseEntityIdDB(tempShortListedRec.getRFQId(), tempShortListedRec.getRespEntityId());
                    RFQResponseRecord respRecObj = new RFQResponseRecord();

                    respRecObj.setRFQId(tempRec.getRFQId());
                    respRecObj.setRespEntityId(tempRec.getRespEntityId());
                    respRecObj.setRespDate(tempRec.getRespDate());
                    respRecObj.setNdaPath(tempRec.getNdaPath());

                    respRecObj.findAndSetEntityName();

                    retObj.Add(tempRec.getRespEntityId(), respRecObj);
                }

                return retObj;
            }
            /// <summary>
            /// For a given rfq id, this method returns the Potential details  of the finalized vendor
            /// </summary>
            /// <param name="cmpUsrId"></param>
            /// <param name="userId"></param>
            /// <param name="rfId"></param>
            /// <returns></returns>
            public RFQShortlisted getFinalizedVendorIdforRFQid(String cmpUsrId, String userId, String rfId)
            {
                if (cmpUsrId == null || cmpUsrId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid entity id value sent to method _dispRFQDetails.getFinalizedVendorIdforRFQid"));

                if (userId == null || userId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid userId id value sent to method _dispRFQDetails.getFinalizedVendorIdforRFQid"));

                return RFQShortlisted.getRFQShortlistedEntryforFinalizedVendor(rfId);
                
            }
        }
        /// <summary>
        /// This class contains method to update RFQ details
        /// </summary>
        public class _updateRFQDetails
        {
            /// <summary>
            /// This method needs to be called when a user shortlists a Lead entry - that lead entry is automatically,
            /// shown as a potential for the response entity id.
            ///  Given a response entity id, user id and the RFQ id this method will create a potential entry for the respective response entity id and RFQ id
            /// </summary>
            /// <param name="respEntId"></param>
            /// <param name="userId"></param>
            /// <param name="rfId"></param>
            /// <returns></returns>
            public int shortListResponseEntry(String respEntId, String userId, String rfId)
            {
                SalesActions._createPotential cpE = new SalesActions._createPotential();
                return cpE.convertLeadToPotential(respEntId, userId, rfId);

            }
            /// <summary>
            /// This method can be used after the customer clicks the 'Finalize Deal' button from purchase screen for a shortlisted entry.
            /// The first parameter must contain the RFQ id and the response entity id.
            /// </summary>
            /// <param name="rfS"></param>
            /// <param name="cmpUsrId"></param>
            /// <param name="userId"></param>
            /// <returns></returns>
            public int finalizeDeal(RFQShortlisted rfS, String cmpUsrId, String userId)
            {
                if (cmpUsrId == null || cmpUsrId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid entity id value sent to method _updateRFQDetails.finalizeDeal"));

                if (userId == null || userId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid userId id value sent to method _updateRFQDetails.finalizeDeal"));

                if (rfS == null || rfS.getRFQId() == null || rfS.getRFQId().Equals("") || rfS.getRespEntityId() == null || rfS.getRespEntityId().Equals("") || rfS.getPotentialId() == null || rfS.getPotentialId().Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid Potential details value sent to method _updateRFQDetails.finalizeDeal"));

                Dictionary<String, String> updateTargetVals = new Dictionary<string, string>();
                Dictionary<String, String> updateWhereVals = new Dictionary<string, string>();

                int totalRecAffected = 0;
                //if (rfS.getFinlCustFlag() != null)
                    updateTargetVals.Add(RFQShortlisted.RFQ_SHORTLIST_COL_FINL_BY_CUST, "Y");
                //if (rfS.getCreateMode().Equals(RFQShortlisted.POTENTIAL_CREATION_MODE_MANUAL))
                    //updateTargetVals.Add(RFQShortlisted.RFQ_SHORTLIST_COL_FINL_BY_SUPL, "Y");
                
                if (RFQShortlisted.getAllRFQShortListedbyPotentialIdDB(rfS.getPotentialId()).getFinlSupFlag().Equals("Y"))
                    updateTargetVals.Add(RFQShortlisted.RFQ_SHORTLIST_COL_POTENTIAL_ACTIVE_STAT, RFQShortlisted.POTENTIAL_ACTIVE_STAT_CLOSED_WON);

                updateWhereVals.Add(RFQShortlisted.RFQ_SHORTLIST_COL_RFQ_ID, rfS.getRFQId());
                updateWhereVals.Add(RFQShortlisted.RFQ_SHORTLIST_COL_RESP_ENT_ID, rfS.getRespEntityId());

                totalRecAffected = RFQShortlisted.updateRFQShortListedEntryDB(updateTargetVals, updateWhereVals, DBConn.Connections.OPERATION_UPDATE);

                return totalRecAffected;
            }
            /// <summary>
            ///  It is mandatory the pass the rfq  id attached to this method.Otherwise, this method will throw exception.
            /// The second parameter is the operation name ('update'/'delete').
            /// Attach only those property values to the requirement object which needs to be updated. Other than the requirement id,
            /// any other property value will be used for updating in case the second parameter value is 'DBConn.Connections.OPERATION_UPDATE'.
            /// Along with the 'RFQDetails' object if the associated 'RFQProductServiceDetails' are also sent with this object (Property 'RFQProdServList'), those
            /// will also be updated. For the product service list to be udpated the associated prod/cat id and Feature id must be sent.
            /// In case of a delete operation it will delete the entire RFQ record from the main RFQ table and also it will have a cascading effect on all the dependant 
            /// tables.
            /// NOTE THAT not all the properties of an RFQ can be changed -
            /// 1. Submit date,
            /// 2. Created entity,
            /// 3. Created User,
            /// 4. Create Mode.
            /// Similarly some properties of the attached 'RFQProdServList' property can not be modified as well.
            /// </summary>
            /// <param name="rfqObj"></param>
            /// <param name="operation"></param>
            /// <returns></returns>
            public int updateRFQ(RFQDetails rfqObj,String operation)
            {
                if (rfqObj == null || rfqObj.getRFQId() == null || rfqObj.getRFQId().Equals(""))
                    throw (new CustomExceptions.invalidParamException("RFQ Id not passed to method _updateRFQDetails.updateRFQ()"));
                else if (!operation.Equals(DBConn.Connections.OPERATION_UPDATE) && !operation.Equals(DBConn.Connections.OPERATION_DELETE))
                    throw (new CustomExceptions.invalidParamException("Invalid operation Name passed to method _updateRFQDetails.updateRFQ()"));

                Dictionary<String, String> updateTargetVals = new Dictionary<string, string>();
                Dictionary<String, String> updateWhereVals = new Dictionary<string, string>();
                
                int totalRecAffected = 0;

                if (operation.Equals(DBConn.Connections.OPERATION_UPDATE))
                {
                    ArrayList RFQProdSpecList = rfqObj.getRFQProdServList();

                    if (rfqObj.getDueDate() != null)
                        updateTargetVals.Add(RFQDetails.RFQ_COL_DUE_DATE, rfqObj.getDueDate());
                    if (rfqObj.getActiveStat() != null)
                        updateTargetVals.Add(RFQDetails.RFQ_COL_ACTIVE_STAT, rfqObj.getActiveStat());
                    if (rfqObj.getCreatedUsr() != null)
                        updateTargetVals.Add(RFQDetails.RFQ_COL_CREATED_USR,rfqObj.getCreatedUsr());
                    if (rfqObj.getEntityId() != null)
                        updateTargetVals.Add(RFQDetails.RFQ_COL_ENTITY_ID, rfqObj.getEntityId());
                    if (rfqObj.getLocalityId() != null)
                        updateTargetVals.Add(RFQDetails.RFQ_COL_LOCAL_ID, rfqObj.getLocalityId());
                    if (rfqObj.getApprovalStat() != null)
                        updateTargetVals.Add(RFQDetails.RFQ_COL_APPROVAL_STAT, rfqObj.getApprovalStat());
                    if (rfqObj.getReqId() != null)
                        updateTargetVals.Add(RFQDetails.RFQ_COL_RELATED_REQ, rfqObj.getReqId());
                    if (rfqObj.getTermsandConds() != null)
                        updateTargetVals.Add(RFQDetails.RFQ_COL_T_AND_C, rfqObj.getTermsandConds());
                    if (rfqObj.getNDADocPath() != null)
                        updateTargetVals.Add(RFQDetails.RFQ_COL_T_AND_C, rfqObj.getNDADocPath().ToString());
                    
                    updateWhereVals.Add(RFQDetails.RFQ_COL_RFQ_ID, rfqObj.getRFQId());

                    totalRecAffected = RFQDetails.updateRFQDetailsDB(updateTargetVals, updateWhereVals, operation);

                    if (RFQProdSpecList != null && RFQProdSpecList.Count > 0)
                    {
                        int count = 0;
                        while (count < RFQProdSpecList.Count)
                        {
                            RFQProductServiceDetails RFQProdSpecObj = (RFQProductServiceDetails)RFQProdSpecList[count];

                            if (RFQProdSpecObj.getPrdCatId() == null || RFQProdSpecObj.getPrdCatId().Equals("") || RFQProdSpecObj.getFeatId() == null || RFQProdSpecObj.getFeatId().Equals(""))
                                throw (new CustomExceptions.invalidParamException("Product Category Id and Feature id necessary to update Requirement specification - method   _updateRFQDetails.updateRFQ()"));

                            updateTargetVals = new Dictionary<string, string>();
                            updateWhereVals = new Dictionary<string, string>();

                            if (RFQProdSpecObj.getFromSpecId() != null)
                                updateTargetVals.Add(RFQProductServiceDetails.RFQ_PROD_COL_FROM_SPEC_ID, RFQProdSpecObj.getFromSpecId());
                            if (RFQProdSpecObj.getToSpecId() != null)
                                updateTargetVals.Add(RFQProductServiceDetails.RFQ_PROD_COL_TO_SPEC_ID, RFQProdSpecObj.getToSpecId());
                            if (RFQProdSpecObj.getMsrmntUnit() != null)
                                updateTargetVals.Add(RFQProductServiceDetails.RFQ_PROD_COL_MSRMNT_UNIT, RFQProdSpecObj.getMsrmntUnit());
                            if (RFQProdSpecObj.getImgPath() != null)
                                updateTargetVals.Add(RFQProductServiceDetails.RFQ_PROD_COL_IMAGE_PATH, RFQProdSpecObj.getImgPath().ToString());
                            if (RFQProdSpecObj.getSpecText() != null)
                                updateTargetVals.Add(RFQProductServiceDetails.RFQ_PROD_COL_SPEC_TEXT, RFQProdSpecObj.getSpecText());
                            if (RFQProdSpecObj.getQuantity() != 0)
                                updateTargetVals.Add(RFQProductServiceDetails.RFQ_PROD_COL_QUANTITY, RFQProdSpecObj.getQuantity().ToString());

                            updateWhereVals.Add(RFQProductServiceDetails.RFQ_PROD_COL_RFQ_ID, RFQProdSpecObj.getRFQId());
                            updateWhereVals.Add(RFQProductServiceDetails.RFQ_PROD_COL_PROD_ID, RFQProdSpecObj.getPrdCatId());
                            updateWhereVals.Add(RFQProductServiceDetails.RFQ_PROD_COL_FEAT_ID, RFQProdSpecObj.getFeatId());

                            totalRecAffected += RFQProductServiceDetails.updateRFQProductServiceDetails(updateTargetVals, updateWhereVals, operation);

                            count++;
                        }
                    }
                }

                else if (operation.Equals(DBConn.Connections.OPERATION_DELETE))
                {
                    updateWhereVals.Add(RFQDetails.RFQ_COL_RFQ_ID, rfqObj.getRFQId());
                    totalRecAffected = RFQDetails.updateRFQDetailsDB(updateTargetVals, updateWhereVals, operation);
                }

                return totalRecAffected;
            }
            /// <summary>
            /// This method can be used to update/delete entries from the RFQ broadcast list for a particular RFQ.
            /// Whichever be the current broadcast list of an RFQ, this method will remove the exsting one and set this new list.
            /// </summary>
            /// <param name="rfqBl"></param>
            /// <param name="operation"></param>
            /// <returns></returns>
            public int updateRFQBroadCastList(RFQBroadcastList rfqBl)
            {
                if (rfqBl == null || rfqBl.getRFQId() == null || rfqBl.getRFQId().Equals(""))
                    throw (new CustomExceptions.invalidParamException("RFQ Id not passed to method _updateRFQDetails.updateRFQBroadCastList()"));

                return RFQBroadcastList.insertRFQBroadcastListDB(rfqBl);
            }
        }
        /// <summary>
        /// This class contains operations related to display of PO records on the purchase screen
        /// </summary>
        public class _dispPODetails
        {
            public String FILTER_BY_RFQ_NO = "FILTER_BY_RFQ_NO";
            public String FILTER_BY_SUBMIT_DATE_FROM = "FILTER_BY_SUBMIT_DATE_FROM";
            public String FILTER_BY_SUBMIT_DATE_TO = "FILTER_BY_SUBMIT_DATE_TO";
            public String FILTER_BY_PO_NO = "FILTER_BY_PO_NO";
            public String FILTER_BY_VENDOR = "FILTER_BY_VENDOR";
            public String FILTER_BY_CLIENT = "FILTER_BY_CLIENT";

            /// <summary>
            /// The third parameter is a dictionary which contains FILTER parameter name as 'key' and the respective filter value as the 'value'.
            /// Note that, all the filter parameters name must be chosen from the 'FILTER_BY_' properties of this class.
            /// Also, all the date values passed as filter parameter must be passed in 'yyyy-MM-dd' format.
            /// </summary>
            /// <param name="cmpUsrId"></param>
            /// <param name="userId"></param>
            /// <param name="filterParam"></param>
            /// <returns></returns>
            public ArrayList getAllPODetailsFiltered(String cmpUsrId, String userId, Dictionary<String, String> filterParam)
            {
                ArrayList rfqNoList = new ArrayList();
                ArrayList poNoList = new ArrayList();
                ArrayList allPOFiltered = new ArrayList();

                rfqNoList = BackEndObjects.RFQDetails.
                    getAllRFQIncludingDummybyEntityIdDB(cmpUsrId, true);

                Dictionary<String, String> poDict = rfqNoList != null && rfqNoList.Count > 0 ?
                    PurchaseOrder.getPurchaseOrdersforRFQIdListDB(rfqNoList) : new Dictionary<String, String>();

                String[] poArray = poDict.Values.ToArray<String>();
                for (int i = 0; i < poArray.Length; i++)
                {
                    poNoList.Add(poArray[i]);
                }
                Dictionary<String, PurchaseOrder> poObjDict = PurchaseOrder.getPurchaseOrdersforPOIdListDB(poNoList);

                foreach (KeyValuePair<String, PurchaseOrder> kvp in poObjDict)
                {
                    Boolean filterConditionSatisfied = true;
                    PurchaseOrder poObj = kvp.Value;

                    if (filterConditionSatisfied)
                    {
                        if (filterParam.ContainsKey(this.FILTER_BY_RFQ_NO))
                            filterConditionSatisfied = poObj.getRfq_id().IndexOf(filterParam[this.FILTER_BY_RFQ_NO], StringComparison.InvariantCultureIgnoreCase) >= 0 ? filterConditionSatisfied && true : false;
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_SUBMIT_DATE_FROM) && filterParam.ContainsKey(this.FILTER_BY_SUBMIT_DATE_TO))
                        {
                            //Parse the due date as stored in the object
                            String subDate = poObj.getDate_created().Substring(0, poObj.getDate_created().IndexOf(" "));
                            DateTime subDateVal;
                            DateTime.TryParseExact(subDate, "%M/%d/yyyy", null, DateTimeStyles.AllowWhiteSpaces, out subDateVal);

                            //parse the from due date as passed in the filter parameter
                            DateTime subDateFromVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_SUBMIT_DATE_FROM], "yyyy-MM-dd", null, DateTimeStyles.None, out subDateFromVal);
                            DateTime subDateToVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_SUBMIT_DATE_TO], "yyyy-MM-dd", null, DateTimeStyles.None, out subDateToVal);


                            if (DateTime.Compare(subDateVal, subDateFromVal) >= 0 && DateTime.Compare(subDateVal, subDateToVal) <= 0)
                                filterConditionSatisfied = filterConditionSatisfied && true;
                            else
                                filterConditionSatisfied = false;
                        }
                        if (filterParam.ContainsKey(this.FILTER_BY_PO_NO))
                            filterConditionSatisfied = poObj.getPo_id().IndexOf(filterParam[this.FILTER_BY_PO_NO], StringComparison.InvariantCultureIgnoreCase) >= 0 ? filterConditionSatisfied && true : false;
                        if (filterParam.ContainsKey(this.FILTER_BY_VENDOR))
                            filterConditionSatisfied = poObj.getRespEntId().IndexOf(filterParam[this.FILTER_BY_VENDOR], StringComparison.InvariantCultureIgnoreCase) >= 0 ? filterConditionSatisfied && true : false;

                        if (filterConditionSatisfied)
                            allPOFiltered.Add(poObj);

                    }
                }
              
                  
                return allPOFiltered;
            }
            /// <summary>
            /// The third parameter is a dictionary which contains FILTER parameter name as 'key' and the respective filter value as the 'value'.
            /// Note that, all the filter parameters name must be chosen from the 'FILTER_BY_' properties of this class.
            /// Also, all the date values passed as filter parameter must be passed in 'yyyy-MM-dd' format.
            /// Each object of the returned ArrayList is instance of the class 'PurchaseOrder'.The RFQ id is appended with a semicolon as a delim followed by the creation mode
            /// </summary>
            /// <param name="cmpUsrId"></param>
            /// <param name="userId"></param>
            /// <param name="filterParam"></param>
            /// <returns></returns>
            public ArrayList getAllSODetailsFiltered(String cmpUsrId, String userId, Dictionary<String, String> filterParam)
            {
                ArrayList rfqNoList = new ArrayList();
                ArrayList poNoList = new ArrayList();
                ArrayList allPOFiltered = new ArrayList();

                Dictionary<String, RFQShortlisted> rfqListSO = BackEndObjects.RFQShortlisted.
                    getAllRFQShortlistedEntriesforFinalizedVendor(cmpUsrId);

                foreach (KeyValuePair<String, RFQShortlisted> kvp in rfqListSO)
                    rfqNoList.Add(kvp.Key);

                Dictionary<String, String> poDict = rfqNoList != null && rfqNoList.Count > 0 ?
                    PurchaseOrder.getPurchaseOrdersforRFQIdListDB(rfqNoList) : new Dictionary<String, String>();

                String[] poArray = poDict.Values.ToArray<String>();
                for (int i = 0; i < poArray.Length; i++)
                {
                    poNoList.Add(poArray[i]);
                }
                Dictionary<String, PurchaseOrder> poObjDict = PurchaseOrder.getPurchaseOrdersforPOIdListDB(poNoList);

                foreach (KeyValuePair<String, PurchaseOrder> kvp in poObjDict)
                {
                    Boolean filterConditionSatisfied = true;
                    PurchaseOrder poObj = kvp.Value;

                    if (filterConditionSatisfied)
                    {
                        if (filterParam.ContainsKey(this.FILTER_BY_RFQ_NO))
                            filterConditionSatisfied = poObj.getRfq_id().IndexOf(filterParam[this.FILTER_BY_RFQ_NO], StringComparison.InvariantCultureIgnoreCase) >= 0 ? filterConditionSatisfied && true : false;
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_SUBMIT_DATE_FROM) && filterParam.ContainsKey(this.FILTER_BY_SUBMIT_DATE_TO))
                        {
                            //Parse the due date as stored in the object
                            String subDate = poObj.getDate_created().Substring(0, poObj.getDate_created().IndexOf(" "));
                            DateTime subDateVal;
                            DateTime.TryParseExact(subDate, "%M/%d/yyyy", null, DateTimeStyles.AllowWhiteSpaces, out subDateVal);

                            //parse the from due date as passed in the filter parameter
                            DateTime subDateFromVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_SUBMIT_DATE_FROM], "yyyy-MM-dd", null, DateTimeStyles.None, out subDateFromVal);
                            DateTime subDateToVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_SUBMIT_DATE_TO], "yyyy-MM-dd", null, DateTimeStyles.None, out subDateToVal);


                            if (DateTime.Compare(subDateVal, subDateFromVal) >= 0 && DateTime.Compare(subDateVal, subDateToVal) <= 0)
                                filterConditionSatisfied = filterConditionSatisfied && true;
                            else
                                filterConditionSatisfied = false;
                        }
                        if (filterParam.ContainsKey(this.FILTER_BY_PO_NO))
                            filterConditionSatisfied = poObj.getPo_id().IndexOf(filterParam[this.FILTER_BY_PO_NO], StringComparison.InvariantCultureIgnoreCase) >= 0 ? filterConditionSatisfied && true : false;
                        if (filterParam.ContainsKey(this.FILTER_BY_CLIENT))
                            filterConditionSatisfied = RFQDetails.getRFQDetailsbyIdDB(poObj.getRfq_id()).getEntityId().Equals(filterParam[this.FILTER_BY_CLIENT]) ? filterConditionSatisfied && true : false;

                        if (filterConditionSatisfied)
                        {
                            //Sending the creation mode as well
                            poObj.setRfq_id(poObj.getRfq_id() + ";" + rfqListSO[poObj.getRfq_id()].getCreateMode());
                            allPOFiltered.Add(poObj);
                        }

                    }
                }


                return allPOFiltered;
            }
        }
        /// <summary>
        /// This class contains operations related to the display of invoice records
        /// </summary>
        public class _dispInvoiceDetails
        {
            public String FILTER_BY_INVOICE_NO = "FILTER_BY_INVOICE_NO";
            public String FILTER_BY_PRODUCT_CAT = "FILTER_BY_PRODUCT_CAT";
            public String FILTER_BY_DELIV_STAT = "FILTER_BY_DELIV_STAT";
            public String FILTER_BY_PMNT_STAT = "FILTER_BY_PMNT_STAT";
            public String FILTER_BY_FROM_DATE = "FILTER_BY_FROM_DATE";
            public String FILTER_BY_TO_DATE = "FILTER_BY_TO_DATE";
            public String FILTER_BY_RFQ_NO = "FILTER_BY_RFQ_NO";
            public String FILTER_BY_TRAN_NO = "FILTER_BY_TRAN_NO";
            public String FILTER_BY_CUSTOMER = "FILTER_BY_CUSTOMER";
            public String FILTER_BY_SUPPLIER = "FILTER_BY_SUPPLIER";
           
            /// <summary>
            /// The second parameter is the complete invoice object arraylist which needs to be filtered
            /// The third parameter is a dictionary which contains FILTER parameter name as 'key' and the respective filter value as the 'value'.
            /// Note that, all the filter parameters name must be chosen from the 'FILTER_BY_' properties of this class.
            /// Also, all the date values passed as filter parameter must be passed in 'yyyy-MM-dd' format.
            /// Each object of the returned ArrayList is instance of the class 'Invoice' 
            /// </summary>
            /// <param name="cmpUsrId"></param>
            /// <param name="userId"></param>
            /// <param name="filterParam"></param>
            /// <returns></returns>
            public ArrayList getAllInvDetailsFiltered(String cmpUsrId, ArrayList allInvList, Dictionary<String, String> filterParam)
            {
                if (cmpUsrId == null || cmpUsrId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid entity id value sent to method _dispInvoiceDetails.getAllInvDetailsFiltered"));


                //ArrayList allInvList = new ArrayList();
                ArrayList allInvFiltered = new ArrayList();

                //allInvList = RFQDetails.getAllRFQbyEntityIdDB(cmpUsrId);
                int i = 0;

                for (i = 0; i < allInvList.Count; i++)
                {
                    Invoice tempInv = (Invoice)allInvList[i];
                    Boolean filterConditionSatisfied = true;
                    
                    //ArrayList rfqSpecList = RFQProductServiceDetails.getAllProductServiceDetailsbyRFQIdDB(tempInv.getRFQId());
                     
                    //foreach (KeyValuePair<String, RFQProductServiceDetails> kvp in rfqSpecDict)
                    //rfqSpecList.Add(kvp.Value);
                    if (filterParam.ContainsKey(this.FILTER_BY_INVOICE_NO))
                    {
                        if (tempInv.getInvoiceNo().IndexOf(filterParam[this.FILTER_BY_INVOICE_NO],StringComparison.OrdinalIgnoreCase) < 0)
                            filterConditionSatisfied = false;
                    }   

                    

                    if (filterConditionSatisfied)
                    {
                        if (filterParam.ContainsKey(this.FILTER_BY_RFQ_NO))
                            filterConditionSatisfied = tempInv.getRFQId().IndexOf(filterParam[this.FILTER_BY_RFQ_NO], StringComparison.OrdinalIgnoreCase)>=0? filterConditionSatisfied && true : false;
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_DELIV_STAT))
                            filterConditionSatisfied = tempInv.getDeliveryStatus().Equals(filterParam[this.FILTER_BY_DELIV_STAT]) ? filterConditionSatisfied && true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_PMNT_STAT))
                            filterConditionSatisfied = tempInv.getPaymentStatus().Equals(filterParam[this.FILTER_BY_PMNT_STAT]) ? filterConditionSatisfied && true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_FROM_DATE) && filterParam.ContainsKey(this.FILTER_BY_TO_DATE))
                        {
                            //Parse the due date as stored in the object
                            String dueDate = tempInv.getInvoiceDate().Substring(0, tempInv.getInvoiceDate().IndexOf(" "));
                            DateTime dueDateVal;
                            DateTime.TryParseExact(dueDate, "%M/%d/yyyy", null, DateTimeStyles.AllowWhiteSpaces, out dueDateVal);

                            //parse the from due date as passed in the filter parameter
                            DateTime dueDateFromVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_FROM_DATE], "yyyy-MM-dd", null, DateTimeStyles.None, out dueDateFromVal);
                            DateTime dueDateToVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_TO_DATE], "yyyy-MM-dd", null, DateTimeStyles.None, out dueDateToVal);


                            if (DateTime.Compare(dueDateVal, dueDateFromVal) >= 0 && DateTime.Compare(dueDateVal, dueDateToVal) <= 0)
                                filterConditionSatisfied = filterConditionSatisfied && true;
                            else
                                filterConditionSatisfied = false;
                        }
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_TRAN_NO))
                            filterConditionSatisfied=BackEndObjects.Payment.
                                getPaymentDetailsforInvoiceIdandTranIdDB(tempInv.getInvoiceId(), filterParam[this.FILTER_BY_TRAN_NO]).Count > 0 ? filterConditionSatisfied && true : false;
                        if (filterParam.ContainsKey(this.FILTER_BY_PRODUCT_CAT) && filterConditionSatisfied)
                        {
                            ArrayList rfqProdList = RFQProdServQnty.getRFQProductServiceQuantityforRFIdDB(tempInv.getRFQId());
                            int j = 0; 
                            filterConditionSatisfied = false;
                            //Even if one of the specificaion object matches the filter criteria, then allow this selection
                            for (j = 0; j < rfqProdList.Count; j++)
                            {
                                RFQProdServQnty tempQnty = (RFQProdServQnty)rfqProdList[j];
                                if (!tempQnty.getProdCatId().Equals(filterParam[this.FILTER_BY_PRODUCT_CAT]) &&
                                    !BackEndObjects.ProductCategory.getRootLevelParentCategoryDB(tempQnty.getProdCatId()).ContainsKey(filterParam[this.FILTER_BY_PRODUCT_CAT]))
                                { filterConditionSatisfied = false; }
                                else
                                { filterConditionSatisfied = true; break; }
                            }

                            //if (filterConditionSatisfied)
                            //tempInv.setRFQProdServList(rfqSpecList);
                        }
                        if(filterParam.ContainsKey(this.FILTER_BY_CUSTOMER) && filterConditionSatisfied)
                            filterConditionSatisfied = BackEndObjects.RFQDetails.getRFQDetailsbyIdDB(tempInv.getRFQId()).getEntityId().Equals(filterParam[this.FILTER_BY_CUSTOMER]) ? filterConditionSatisfied && true : false;
                        if (filterParam.ContainsKey(this.FILTER_BY_SUPPLIER) && filterConditionSatisfied)
                            filterConditionSatisfied = tempInv.getRespEntityId().Equals(filterParam[this.FILTER_BY_SUPPLIER]) ? filterConditionSatisfied && true : false;
                        if (filterConditionSatisfied)
                            allInvFiltered.Add(tempInv);
                    }
                }

                return allInvFiltered;
            }
        }
    }
    /// <summary>
    /// This class holds all the column names to be displayed on reports
    /// </summary>
    public class DisplayNamesForDashBoard
    {
        
        public static Dictionary<String,String> leadRecords=new Dictionary<String,String>();

        public static Dictionary<String,String> getLeadColumns()
        {
            leadRecords.Add("Lead Name",DBConn.Connections.STRING_TYPE);
            leadRecords.Add("Account Name", DBConn.Connections.STRING_TYPE);

            return leadRecords;
        }
    }
    public class DefectActions
    {
        public class _dispDefectDetails
        {
            public String FILTER_BY_DEFECT_NO = "FILTER_BY_DEFECT_NO";
            public String FILTER_BY_RFQ_NO = "FILTER_BY_RFQ_NO";
            public String FILTER_BY_INVOICE_NO = "FILTER_BY_INVOICE_NO";
            public String FILTER_BY_DEFECT_STAT = "FILTER_BY_DEFECT_STAT";
            public String FILTER_BY_DEFECT_RESOL_STAT = "FILTER_BY_DEFECT_RESOL_STAT";
            public String FILTER_BY_SUBMIT_DATE_FROM = "FILTER_BY_SUBMIT_DATE_FROM";
            public String FILTER_BY_SUBMIT_DATE_TO = "FILTER_BY_SUBMIT_DATE_TO";
            public String FILTER_BY_ASSIGNED_TO = "FILTER_BY_ASSIGNED_TO";
            public String FILTER_BY_DEFECT_SEV = "FILTER_BY_DEFECT_SEV";
            
            /// <summary>
            /// The context tells whether it is a filter for a incoming or an ourgoing defect.
            /// The passed values should be "incoming"/"outgoing".
            /// If the context value is "incoming" the second paramter should be the supplier id.
            /// If the context value is "outgoing" the second parameter should be the customer id
            /// </summary>
            /// <param name="context"></param>
            /// <param name="searchForId"></param>
            /// <param name="filterParams"></param>
            /// <returns></returns>
            public Dictionary<String,DefectDetails> getAllDefectsFiltered(String context, String searchForId, Dictionary<String, String> filterParams)
            {
                if (context == null || context.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid context value sent to method _dispDefectDetails.getAllDefectsFiltered"));

                if (searchForId == null || searchForId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid search for id value sent to method _dispDefectDetails.getAllDefectsFiltered"));


                ArrayList allDefectList = new ArrayList();
                Dictionary<String, DefectDetails> allDefectFilteredDict = new Dictionary<String, DefectDetails>();

                if (context.Equals("incoming"))
                {
                    Dictionary<String, DefectDetails> defectDict = BackEndObjects.DefectDetails.getAllDefectDetailsforSupplierIdDB(searchForId);
                    foreach (KeyValuePair<String, DefectDetails> kvp in defectDict)
                        allDefectList.Add(kvp.Value);
                }
                else
                {
                    Dictionary<String, DefectDetails> defectDict = BackEndObjects.DefectDetails.getAllDefectDetailsforCustomerIdDB(searchForId);
                    foreach (KeyValuePair<String, DefectDetails> kvp in defectDict)
                        allDefectList.Add(kvp.Value);
                }
                int i = 0;

                for (i = 0; i < allDefectList.Count; i++)
                {
                    DefectDetails tempDefectObj = (DefectDetails)allDefectList[i];
                    Boolean filterConditionSatisfied = false;


                    if (filterParams.ContainsKey(this.FILTER_BY_DEFECT_NO))
                    {

                        if (tempDefectObj.getDefectId().IndexOf(filterParams[this.FILTER_BY_DEFECT_NO],StringComparison.OrdinalIgnoreCase)>=0)
                            filterConditionSatisfied = true;
                    }

                    else
                    {
                        filterConditionSatisfied = true;
                    }

                    if (filterConditionSatisfied)
                    {
                        if (filterParams.ContainsKey(this.FILTER_BY_DEFECT_STAT))
                            filterConditionSatisfied = tempDefectObj.getDefectStat().Equals(filterParams[this.FILTER_BY_DEFECT_STAT]) ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterParams.ContainsKey(this.FILTER_BY_DEFECT_RESOL_STAT))
                            filterConditionSatisfied = tempDefectObj.getResolStat().Equals(filterParams[this.FILTER_BY_DEFECT_RESOL_STAT]) ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterParams.ContainsKey(this.FILTER_BY_INVOICE_NO))
                            filterConditionSatisfied = tempDefectObj.getInvoiceId().IndexOf(filterParams[this.FILTER_BY_INVOICE_NO],StringComparison.OrdinalIgnoreCase)>=0 ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterParams.ContainsKey(this.FILTER_BY_RFQ_NO))
                            filterConditionSatisfied = tempDefectObj.getRFQId().IndexOf(filterParams[this.FILTER_BY_RFQ_NO],StringComparison.OrdinalIgnoreCase)>=0 ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied

                        if (filterParams.ContainsKey(this.FILTER_BY_SUBMIT_DATE_FROM) && filterParams.ContainsKey(this.FILTER_BY_SUBMIT_DATE_TO))
                        {
                            //Parse the create date as stored in the object
                            String createDate = tempDefectObj.getDateCreated().Substring(0, tempDefectObj.getDateCreated().IndexOf(" "));
                            DateTime createDateVal;
                            DateTime.TryParseExact(createDate, "%M/%d/yyyy", null, DateTimeStyles.AllowWhiteSpaces, out createDateVal);

                            //parse the from create date as passed in the filter parameter
                            DateTime createDateFromVal;
                            DateTime.TryParseExact(filterParams[this.FILTER_BY_SUBMIT_DATE_FROM], "yyyy-MM-dd", null, DateTimeStyles.None, out createDateFromVal);
                            DateTime createDateToVal;
                            DateTime.TryParseExact(filterParams[this.FILTER_BY_SUBMIT_DATE_TO], "yyyy-MM-dd", null, DateTimeStyles.None, out createDateToVal);


                            if (DateTime.Compare(createDateVal, createDateFromVal) >= 0 && DateTime.Compare(createDateVal, createDateToVal) <= 0)
                                filterConditionSatisfied = filterConditionSatisfied & true;
                            else
                                filterConditionSatisfied = false;
                        }
                        if (filterParams.ContainsKey(this.FILTER_BY_DEFECT_SEV))
                            filterConditionSatisfied = tempDefectObj.getSeverity().Equals(filterParams[FILTER_BY_DEFECT_SEV]) ? filterConditionSatisfied & true : false;
                        if (filterParams.ContainsKey(FILTER_BY_ASSIGNED_TO))
                            filterConditionSatisfied = tempDefectObj.getAssignedToUser().Equals(filterParams[FILTER_BY_ASSIGNED_TO]) ? filterConditionSatisfied & true : false;
                        if (filterConditionSatisfied)
                        {
                            allDefectFilteredDict.Add(tempDefectObj.getDefectId(), tempDefectObj);
                        }
                    }
                }

                return allDefectFilteredDict;
            }
            /// <summary>
            /// this will filter out all open defects satisfying the filter criteria which are assigned to the given user id and belonging to the given entity id
            /// </summary>
            /// <param name="context"></param>
            /// <param name="searchForId"></param>
            /// <param name="userId"></param>
            /// <param name="filterParams"></param>
            /// <returns></returns>
            public Dictionary<String, DefectDetails> getAllOpenDefectFilteredForSupplierAndAssignedToUser(String context, String searchForId, String userId, Dictionary<String, String> filterParams)
            {
                if (context == null || context.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid context value sent to method _dispDefectDetails.getAllOpenDefectFilteredForSupplierAndAssignedToUser"));

                if (searchForId == null || searchForId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid search for id value sent to method _dispDefectDetails.getAllOpenDefectFilteredForSupplierAndAssignedToUser"));


                ArrayList allDefectList = new ArrayList();
                Dictionary<String, DefectDetails> allDefectFilteredDict = new Dictionary<String, DefectDetails>();

                if (context.Equals("incoming"))
                {
                    Dictionary<String, DefectDetails> defectDict = BackEndObjects.DefectDetails.
                    getAllOpenDefectDetailsforSupplierIdAndAssignedToUserDB(searchForId,userId);

                    foreach (KeyValuePair<String, DefectDetails> kvp in defectDict)
                        allDefectList.Add(kvp.Value);
                }
                else
                {
                    Dictionary<String, DefectDetails> defectDict = BackEndObjects.DefectDetails.
getAllOpenDefectDetailsforSupplierIdAndAssignedToUserDB(searchForId, userId);

                    foreach (KeyValuePair<String, DefectDetails> kvp in defectDict)
                        allDefectList.Add(kvp.Value);
                }
                int i = 0;

                for (i = 0; i < allDefectList.Count; i++)
                {
                    DefectDetails tempDefectObj = (DefectDetails)allDefectList[i];
                    Boolean filterConditionSatisfied = false;


                    if (filterParams.ContainsKey(this.FILTER_BY_DEFECT_NO))
                    {

                        if (tempDefectObj.getDefectId().IndexOf(filterParams[this.FILTER_BY_DEFECT_NO], StringComparison.OrdinalIgnoreCase) >= 0)
                            filterConditionSatisfied = true;
                    }

                    else
                    {
                        filterConditionSatisfied = true;
                    }

                    if (filterConditionSatisfied)
                    {
                        if (filterParams.ContainsKey(this.FILTER_BY_DEFECT_STAT))
                            filterConditionSatisfied = tempDefectObj.getDefectStat().Equals(filterParams[this.FILTER_BY_DEFECT_STAT]) ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterParams.ContainsKey(this.FILTER_BY_DEFECT_RESOL_STAT))
                            filterConditionSatisfied = tempDefectObj.getResolStat().Equals(filterParams[this.FILTER_BY_DEFECT_RESOL_STAT]) ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterParams.ContainsKey(this.FILTER_BY_INVOICE_NO))
                            filterConditionSatisfied = tempDefectObj.getInvoiceId().IndexOf(filterParams[this.FILTER_BY_INVOICE_NO], StringComparison.OrdinalIgnoreCase) >= 0 ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterParams.ContainsKey(this.FILTER_BY_RFQ_NO))
                            filterConditionSatisfied = tempDefectObj.getRFQId().IndexOf(filterParams[this.FILTER_BY_RFQ_NO], StringComparison.OrdinalIgnoreCase) >= 0 ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied

                        if (filterParams.ContainsKey(this.FILTER_BY_SUBMIT_DATE_FROM) && filterParams.ContainsKey(this.FILTER_BY_SUBMIT_DATE_TO))
                        {
                            //Parse the create date as stored in the object
                            String createDate = tempDefectObj.getDateCreated().Substring(0, tempDefectObj.getDateCreated().IndexOf(" "));
                            DateTime createDateVal;
                            DateTime.TryParseExact(createDate, "%M/%d/yyyy", null, DateTimeStyles.AllowWhiteSpaces, out createDateVal);

                            //parse the from create date as passed in the filter parameter
                            DateTime createDateFromVal;
                            DateTime.TryParseExact(filterParams[this.FILTER_BY_SUBMIT_DATE_FROM], "yyyy-MM-dd", null, DateTimeStyles.None, out createDateFromVal);
                            DateTime createDateToVal;
                            DateTime.TryParseExact(filterParams[this.FILTER_BY_SUBMIT_DATE_TO], "yyyy-MM-dd", null, DateTimeStyles.None, out createDateToVal);


                            if (DateTime.Compare(createDateVal, createDateFromVal) >= 0 && DateTime.Compare(createDateVal, createDateToVal) <= 0)
                                filterConditionSatisfied = filterConditionSatisfied & true;
                            else
                                filterConditionSatisfied = false;
                        }
                        if (filterParams.ContainsKey(this.FILTER_BY_DEFECT_SEV))
                            filterConditionSatisfied = tempDefectObj.getSeverity().Equals(filterParams[FILTER_BY_DEFECT_SEV]) ? filterConditionSatisfied & true : false;
                        if (filterParams.ContainsKey(FILTER_BY_ASSIGNED_TO))
                            filterConditionSatisfied = tempDefectObj.getAssignedToUser().Equals(filterParams[FILTER_BY_ASSIGNED_TO]) ? filterConditionSatisfied & true : false;
                        if (filterConditionSatisfied)
                        {
                            allDefectFilteredDict.Add(tempDefectObj.getDefectId(), tempDefectObj);
                        }
                    }
                }

                return allDefectFilteredDict;
            }
            /// <summary>
            /// The context tells whether it is a filter for a incoming or an ourgoing defect.
            /// The passed values should be "incoming"/"outgoing".
            /// If the context value is "incoming" the second paramter should be the supplier id.
            /// If the context value is "outgoing" the second parameter should be the customer id
            /// </summary>
            /// <param name="context"></param>
            /// <param name="searchForId"></param>
            /// <param name="filterParams"></param>
            /// <returns></returns>
            public ArrayList getAllDefectsFilteredORDERBYCreateDate(String context, String searchForId, Dictionary<String, String> filterParams)
            {
                if (context == null || context.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid context value sent to method _dispDefectDetails.getAllDefectsFiltered"));

                if (searchForId == null || searchForId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid search for id value sent to method _dispDefectDetails.getAllDefectsFiltered"));


                ArrayList allDefectList = new ArrayList();
                ArrayList allDefectFilteredList = new ArrayList();

                if (context.Equals("incoming"))
                {
                    allDefectList = BackEndObjects.DefectDetails.getAllDefectDetailsforSupplierIdDBORDERBYCreateDate(searchForId);
                    /*foreach (KeyValuePair<String, DefectDetails> kvp in defectDict)
                        allDefectList.Add(kvp.Value);*/
                }
                else
                {
                    allDefectList = BackEndObjects.DefectDetails.getAllDefectDetailsforCustomerIdDBORDERBYCreateDate(searchForId);
                    /*foreach (KeyValuePair<String, DefectDetails> kvp in defectDict)
                        allDefectList.Add(kvp.Value);*/
                }
                int i = 0;

                for (i = 0; i < allDefectList.Count; i++)
                {
                    DefectDetails tempDefectObj = (DefectDetails)allDefectList[i];
                    Boolean filterConditionSatisfied = false;


                    if (filterParams.ContainsKey(this.FILTER_BY_DEFECT_NO))
                    {

                        if (tempDefectObj.getDefectId().IndexOf(filterParams[this.FILTER_BY_DEFECT_NO], StringComparison.OrdinalIgnoreCase) >= 0)
                            filterConditionSatisfied = true;
                    }

                    else
                    {
                        filterConditionSatisfied = true;
                    }

                    if (filterConditionSatisfied)
                    {
                        if (filterParams.ContainsKey(this.FILTER_BY_DEFECT_STAT))
                            filterConditionSatisfied = tempDefectObj.getDefectStat().Equals(filterParams[this.FILTER_BY_DEFECT_STAT]) ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterParams.ContainsKey(this.FILTER_BY_DEFECT_RESOL_STAT))
                            filterConditionSatisfied = tempDefectObj.getResolStat().Equals(filterParams[this.FILTER_BY_DEFECT_RESOL_STAT]) ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterParams.ContainsKey(this.FILTER_BY_INVOICE_NO))
                            filterConditionSatisfied = tempDefectObj.getInvoiceId().IndexOf(filterParams[this.FILTER_BY_INVOICE_NO], StringComparison.OrdinalIgnoreCase) >= 0 ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterParams.ContainsKey(this.FILTER_BY_RFQ_NO))
                            filterConditionSatisfied = tempDefectObj.getRFQId().IndexOf(filterParams[this.FILTER_BY_RFQ_NO], StringComparison.OrdinalIgnoreCase) >= 0 ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied

                        if (filterParams.ContainsKey(this.FILTER_BY_SUBMIT_DATE_FROM) && filterParams.ContainsKey(this.FILTER_BY_SUBMIT_DATE_TO))
                        {
                            //Parse the create date as stored in the object
                            String createDate = tempDefectObj.getDateCreated().Substring(0, tempDefectObj.getDateCreated().IndexOf(" "));
                            DateTime createDateVal;
                            DateTime.TryParseExact(createDate, "%M/%d/yyyy", null, DateTimeStyles.AllowWhiteSpaces, out createDateVal);

                            //parse the from create date as passed in the filter parameter
                            DateTime createDateFromVal;
                            DateTime.TryParseExact(filterParams[this.FILTER_BY_SUBMIT_DATE_FROM], "yyyy-MM-dd", null, DateTimeStyles.None, out createDateFromVal);
                            DateTime createDateToVal;
                            DateTime.TryParseExact(filterParams[this.FILTER_BY_SUBMIT_DATE_TO], "yyyy-MM-dd", null, DateTimeStyles.None, out createDateToVal);


                            if (DateTime.Compare(createDateVal, createDateFromVal) >= 0 && DateTime.Compare(createDateVal, createDateToVal) <= 0)
                                filterConditionSatisfied = filterConditionSatisfied & true;
                            else
                                filterConditionSatisfied = false;
                        }
                        if (filterConditionSatisfied)
                        {
                            allDefectFilteredList.Add(tempDefectObj);
                        }
                    }
                }

                return allDefectFilteredList;
            }

            public ArrayList getAllDefectsFilteredORDERBYCloseDate(String context, String searchForId, Dictionary<String, String> filterParams)
            {
                if (context == null || context.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid context value sent to method _dispDefectDetails.getAllDefectsFiltered"));

                if (searchForId == null || searchForId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid search for id value sent to method _dispDefectDetails.getAllDefectsFiltered"));


                ArrayList allDefectList = new ArrayList();
                ArrayList allDefectFilteredList = new ArrayList();

                if (context.Equals("incoming"))
                {
                    allDefectList = BackEndObjects.DefectDetails.getAllDefectDetailsforSupplierIdDBORDERBYCloseDate(searchForId);
                    /*foreach (KeyValuePair<String, DefectDetails> kvp in defectDict)
                        allDefectList.Add(kvp.Value);*/
                }
                else
                {
                    allDefectList = BackEndObjects.DefectDetails.getAllDefectDetailsforCustomerIdDBORDERBYCloseDate(searchForId);
                    /*foreach (KeyValuePair<String, DefectDetails> kvp in defectDict)
                        allDefectList.Add(kvp.Value);*/
                }
                int i = 0;

                for (i = 0; i < allDefectList.Count; i++)
                {
                    DefectDetails tempDefectObj = (DefectDetails)allDefectList[i];
                    Boolean filterConditionSatisfied = false;


                    if (filterParams.ContainsKey(this.FILTER_BY_DEFECT_NO))
                    {

                        if (tempDefectObj.getDefectId().IndexOf(filterParams[this.FILTER_BY_DEFECT_NO], StringComparison.OrdinalIgnoreCase) >= 0)
                            filterConditionSatisfied = true;
                    }

                    else
                    {
                        filterConditionSatisfied = true;
                    }

                    if (filterConditionSatisfied)
                    {
                        if (filterParams.ContainsKey(this.FILTER_BY_DEFECT_STAT))
                            filterConditionSatisfied = tempDefectObj.getDefectStat().Equals(filterParams[this.FILTER_BY_DEFECT_STAT]) ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterParams.ContainsKey(this.FILTER_BY_DEFECT_RESOL_STAT))
                            filterConditionSatisfied = tempDefectObj.getResolStat().Equals(filterParams[this.FILTER_BY_DEFECT_RESOL_STAT]) ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterParams.ContainsKey(this.FILTER_BY_INVOICE_NO))
                            filterConditionSatisfied = tempDefectObj.getInvoiceId().IndexOf(filterParams[this.FILTER_BY_INVOICE_NO], StringComparison.OrdinalIgnoreCase) >= 0 ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterParams.ContainsKey(this.FILTER_BY_RFQ_NO))
                            filterConditionSatisfied = tempDefectObj.getRFQId().IndexOf(filterParams[this.FILTER_BY_RFQ_NO], StringComparison.OrdinalIgnoreCase) >= 0 ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied

                        if (filterParams.ContainsKey(this.FILTER_BY_SUBMIT_DATE_FROM) && filterParams.ContainsKey(this.FILTER_BY_SUBMIT_DATE_TO))
                        {
                            //Parse the create date as stored in the object
                            String createDate = tempDefectObj.getDateCreated().Substring(0, tempDefectObj.getDateCreated().IndexOf(" "));
                            DateTime createDateVal;
                            DateTime.TryParseExact(createDate, "%M/%d/yyyy", null, DateTimeStyles.AllowWhiteSpaces, out createDateVal);

                            //parse the from create date as passed in the filter parameter
                            DateTime createDateFromVal;
                            DateTime.TryParseExact(filterParams[this.FILTER_BY_SUBMIT_DATE_FROM], "yyyy-MM-dd", null, DateTimeStyles.None, out createDateFromVal);
                            DateTime createDateToVal;
                            DateTime.TryParseExact(filterParams[this.FILTER_BY_SUBMIT_DATE_TO], "yyyy-MM-dd", null, DateTimeStyles.None, out createDateToVal);


                            if (DateTime.Compare(createDateVal, createDateFromVal) >= 0 && DateTime.Compare(createDateVal, createDateToVal) <= 0)
                                filterConditionSatisfied = filterConditionSatisfied & true;
                            else
                                filterConditionSatisfied = false;
                        }
                        if (filterConditionSatisfied)
                        {
                            allDefectFilteredList.Add(tempDefectObj);
                        }
                    }
                }

                return allDefectFilteredList;
            }


            public Dictionary<String, DefectDetails> getAllSRsFiltered(String context, String searchForId, Dictionary<String, String> filterParams)
            {
                if (context == null || context.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid context value sent to method _dispDefectDetails.getAllDefectsFiltered"));

                if (searchForId == null || searchForId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid search for id value sent to method _dispDefectDetails.getAllDefectsFiltered"));


                ArrayList allDefectList = new ArrayList();
                Dictionary<String, DefectDetails> allDefectFilteredDict = new Dictionary<String, DefectDetails>();

                if (context.Equals("incoming"))
                {
                    Dictionary<String, DefectDetails> defectDict = BackEndObjects.DefectDetails.getAllSRDetailsforSupplierIdDB(searchForId);
                    foreach (KeyValuePair<String, DefectDetails> kvp in defectDict)
                        allDefectList.Add(kvp.Value);
                }
                else
                {
                    Dictionary<String, DefectDetails> defectDict = BackEndObjects.DefectDetails.getAllSRDetailsforCustomerIdDB(searchForId);
                    foreach (KeyValuePair<String, DefectDetails> kvp in defectDict)
                        allDefectList.Add(kvp.Value);
                }
                int i = 0;

                for (i = 0; i < allDefectList.Count; i++)
                {
                    DefectDetails tempDefectObj = (DefectDetails)allDefectList[i];
                    Boolean filterConditionSatisfied = false;


                    if (filterParams.ContainsKey(this.FILTER_BY_DEFECT_NO))
                    {

                        if (tempDefectObj.getDefectId().IndexOf(filterParams[this.FILTER_BY_DEFECT_NO], StringComparison.OrdinalIgnoreCase) >= 0)
                            filterConditionSatisfied = true;
                    }

                    else
                    {
                        filterConditionSatisfied = true;
                    }

                    if (filterConditionSatisfied)
                    {
                        if (filterParams.ContainsKey(this.FILTER_BY_DEFECT_STAT))
                            filterConditionSatisfied = tempDefectObj.getDefectStat().Equals(filterParams[this.FILTER_BY_DEFECT_STAT]) ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterParams.ContainsKey(this.FILTER_BY_DEFECT_RESOL_STAT))
                            filterConditionSatisfied = tempDefectObj.getResolStat().Equals(filterParams[this.FILTER_BY_DEFECT_RESOL_STAT]) ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterParams.ContainsKey(this.FILTER_BY_INVOICE_NO))
                            filterConditionSatisfied = tempDefectObj.getInvoiceId().IndexOf(filterParams[this.FILTER_BY_INVOICE_NO], StringComparison.OrdinalIgnoreCase) >= 0 ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterParams.ContainsKey(this.FILTER_BY_RFQ_NO))
                            filterConditionSatisfied = tempDefectObj.getRFQId().IndexOf(filterParams[this.FILTER_BY_RFQ_NO], StringComparison.OrdinalIgnoreCase) >= 0 ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied

                        if (filterParams.ContainsKey(this.FILTER_BY_SUBMIT_DATE_FROM) && filterParams.ContainsKey(this.FILTER_BY_SUBMIT_DATE_TO))
                        {
                            //Parse the create date as stored in the object
                            String createDate = tempDefectObj.getDateCreated().Substring(0, tempDefectObj.getDateCreated().IndexOf(" "));
                            DateTime createDateVal;
                            DateTime.TryParseExact(createDate, "%M/%d/yyyy", null, DateTimeStyles.AllowWhiteSpaces, out createDateVal);

                            //parse the from create date as passed in the filter parameter
                            DateTime createDateFromVal;
                            DateTime.TryParseExact(filterParams[this.FILTER_BY_SUBMIT_DATE_FROM], "yyyy-MM-dd", null, DateTimeStyles.None, out createDateFromVal);
                            DateTime createDateToVal;
                            DateTime.TryParseExact(filterParams[this.FILTER_BY_SUBMIT_DATE_TO], "yyyy-MM-dd", null, DateTimeStyles.None, out createDateToVal);


                            if (DateTime.Compare(createDateVal, createDateFromVal) >= 0 && DateTime.Compare(createDateVal, createDateToVal) <= 0)
                                filterConditionSatisfied = filterConditionSatisfied & true;
                            else
                                filterConditionSatisfied = false;
                        }
                        if (filterParams.ContainsKey(this.FILTER_BY_DEFECT_SEV))
                            filterConditionSatisfied = tempDefectObj.getSeverity().Equals(filterParams[FILTER_BY_DEFECT_SEV]) ? filterConditionSatisfied & true : false;
                        if (filterParams.ContainsKey(FILTER_BY_ASSIGNED_TO))
                            filterConditionSatisfied = tempDefectObj.getAssignedToUser().Equals(filterParams[FILTER_BY_ASSIGNED_TO]) ? filterConditionSatisfied & true : false;
                        if (filterConditionSatisfied)
                        {
                            allDefectFilteredDict.Add(tempDefectObj.getDefectId(), tempDefectObj);
                        }
                    }
                }

                return allDefectFilteredDict;
            }
            /// <summary>
            /// this will filter out all open defects satisfying the filter criteria which are assigned to the given user id and belonging to the given entity id
            /// </summary>
            /// <param name="context"></param>
            /// <param name="searchForId"></param>
            /// <param name="userId"></param>
            /// <param name="filterParams"></param>
            /// <returns></returns>
            public Dictionary<String, DefectDetails> getAllOpenSRFilteredForSupplierAndAssignedToUser(String context, String searchForId, String userId, Dictionary<String, String> filterParams)
            {
                if (context == null || context.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid context value sent to method _dispDefectDetails.getAllOpenDefectFilteredForSupplierAndAssignedToUser"));

                if (searchForId == null || searchForId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid search for id value sent to method _dispDefectDetails.getAllOpenDefectFilteredForSupplierAndAssignedToUser"));


                ArrayList allDefectList = new ArrayList();
                Dictionary<String, DefectDetails> allDefectFilteredDict = new Dictionary<String, DefectDetails>();

                if (context.Equals("incoming"))
                {
                    Dictionary<String, DefectDetails> defectDict = BackEndObjects.DefectDetails.
                    getAllOpenSRDetailsforSupplierIdAndAssignedToUserDB(searchForId, userId);

                    foreach (KeyValuePair<String, DefectDetails> kvp in defectDict)
                        allDefectList.Add(kvp.Value);
                }
                else
                {
                    Dictionary<String, DefectDetails> defectDict = BackEndObjects.DefectDetails.
getAllOpenSRDetailsforSupplierIdAndAssignedToUserDB(searchForId, userId);

                    foreach (KeyValuePair<String, DefectDetails> kvp in defectDict)
                        allDefectList.Add(kvp.Value);
                }
                int i = 0;

                for (i = 0; i < allDefectList.Count; i++)
                {
                    DefectDetails tempDefectObj = (DefectDetails)allDefectList[i];
                    Boolean filterConditionSatisfied = false;


                    if (filterParams.ContainsKey(this.FILTER_BY_DEFECT_NO))
                    {

                        if (tempDefectObj.getDefectId().IndexOf(filterParams[this.FILTER_BY_DEFECT_NO], StringComparison.OrdinalIgnoreCase) >= 0)
                            filterConditionSatisfied = true;
                    }

                    else
                    {
                        filterConditionSatisfied = true;
                    }

                    if (filterConditionSatisfied)
                    {
                        if (filterParams.ContainsKey(this.FILTER_BY_DEFECT_STAT))
                            filterConditionSatisfied = tempDefectObj.getDefectStat().Equals(filterParams[this.FILTER_BY_DEFECT_STAT]) ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterParams.ContainsKey(this.FILTER_BY_DEFECT_RESOL_STAT))
                            filterConditionSatisfied = tempDefectObj.getResolStat().Equals(filterParams[this.FILTER_BY_DEFECT_RESOL_STAT]) ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterParams.ContainsKey(this.FILTER_BY_INVOICE_NO))
                            filterConditionSatisfied = tempDefectObj.getInvoiceId().IndexOf(filterParams[this.FILTER_BY_INVOICE_NO], StringComparison.OrdinalIgnoreCase) >= 0 ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterParams.ContainsKey(this.FILTER_BY_RFQ_NO))
                            filterConditionSatisfied = tempDefectObj.getRFQId().IndexOf(filterParams[this.FILTER_BY_RFQ_NO], StringComparison.OrdinalIgnoreCase) >= 0 ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied

                        if (filterParams.ContainsKey(this.FILTER_BY_SUBMIT_DATE_FROM) && filterParams.ContainsKey(this.FILTER_BY_SUBMIT_DATE_TO))
                        {
                            //Parse the create date as stored in the object
                            String createDate = tempDefectObj.getDateCreated().Substring(0, tempDefectObj.getDateCreated().IndexOf(" "));
                            DateTime createDateVal;
                            DateTime.TryParseExact(createDate, "%M/%d/yyyy", null, DateTimeStyles.AllowWhiteSpaces, out createDateVal);

                            //parse the from create date as passed in the filter parameter
                            DateTime createDateFromVal;
                            DateTime.TryParseExact(filterParams[this.FILTER_BY_SUBMIT_DATE_FROM], "yyyy-MM-dd", null, DateTimeStyles.None, out createDateFromVal);
                            DateTime createDateToVal;
                            DateTime.TryParseExact(filterParams[this.FILTER_BY_SUBMIT_DATE_TO], "yyyy-MM-dd", null, DateTimeStyles.None, out createDateToVal);


                            if (DateTime.Compare(createDateVal, createDateFromVal) >= 0 && DateTime.Compare(createDateVal, createDateToVal) <= 0)
                                filterConditionSatisfied = filterConditionSatisfied & true;
                            else
                                filterConditionSatisfied = false;
                        }
                        if (filterParams.ContainsKey(this.FILTER_BY_DEFECT_SEV))
                            filterConditionSatisfied = tempDefectObj.getSeverity().Equals(filterParams[FILTER_BY_DEFECT_SEV]) ? filterConditionSatisfied & true : false;
                        if (filterParams.ContainsKey(FILTER_BY_ASSIGNED_TO))
                            filterConditionSatisfied = tempDefectObj.getAssignedToUser().Equals(filterParams[FILTER_BY_ASSIGNED_TO]) ? filterConditionSatisfied & true : false;
                        if (filterConditionSatisfied)
                        {
                            allDefectFilteredDict.Add(tempDefectObj.getDefectId(), tempDefectObj);
                        }
                    }
                }

                return allDefectFilteredDict;
            }
            /// <summary>
            /// The context tells whether it is a filter for a incoming or an ourgoing defect.
            /// The passed values should be "incoming"/"outgoing".
            /// If the context value is "incoming" the second paramter should be the supplier id.
            /// If the context value is "outgoing" the second parameter should be the customer id
            /// </summary>
            /// <param name="context"></param>
            /// <param name="searchForId"></param>
            /// <param name="filterParams"></param>
            /// <returns></returns>
            public ArrayList getAllSRsFilteredORDERBYCreateDate(String context, String searchForId, Dictionary<String, String> filterParams)
            {
                if (context == null || context.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid context value sent to method _dispDefectDetails.getAllDefectsFiltered"));

                if (searchForId == null || searchForId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid search for id value sent to method _dispDefectDetails.getAllDefectsFiltered"));


                ArrayList allDefectList = new ArrayList();
                ArrayList allDefectFilteredList = new ArrayList();

                if (context.Equals("incoming"))
                {
                    allDefectList = BackEndObjects.DefectDetails.getAllSRDetailsforSupplierIdDBORDERBYCreateDate(searchForId);
                    /*foreach (KeyValuePair<String, DefectDetails> kvp in defectDict)
                        allDefectList.Add(kvp.Value);*/
                }
                else
                {
                    allDefectList = BackEndObjects.DefectDetails.getAllSRDetailsforCustomerIdDBORDERBYCreateDate(searchForId);
                    /*foreach (KeyValuePair<String, DefectDetails> kvp in defectDict)
                        allDefectList.Add(kvp.Value);*/
                }
                int i = 0;

                for (i = 0; i < allDefectList.Count; i++)
                {
                    DefectDetails tempDefectObj = (DefectDetails)allDefectList[i];
                    Boolean filterConditionSatisfied = false;


                    if (filterParams.ContainsKey(this.FILTER_BY_DEFECT_NO))
                    {

                        if (tempDefectObj.getDefectId().IndexOf(filterParams[this.FILTER_BY_DEFECT_NO], StringComparison.OrdinalIgnoreCase) >= 0)
                            filterConditionSatisfied = true;
                    }

                    else
                    {
                        filterConditionSatisfied = true;
                    }

                    if (filterConditionSatisfied)
                    {
                        if (filterParams.ContainsKey(this.FILTER_BY_DEFECT_STAT))
                            filterConditionSatisfied = tempDefectObj.getDefectStat().Equals(filterParams[this.FILTER_BY_DEFECT_STAT]) ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterParams.ContainsKey(this.FILTER_BY_DEFECT_RESOL_STAT))
                            filterConditionSatisfied = tempDefectObj.getResolStat().Equals(filterParams[this.FILTER_BY_DEFECT_RESOL_STAT]) ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterParams.ContainsKey(this.FILTER_BY_INVOICE_NO))
                            filterConditionSatisfied = tempDefectObj.getInvoiceId().IndexOf(filterParams[this.FILTER_BY_INVOICE_NO], StringComparison.OrdinalIgnoreCase) >= 0 ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterParams.ContainsKey(this.FILTER_BY_RFQ_NO))
                            filterConditionSatisfied = tempDefectObj.getRFQId().IndexOf(filterParams[this.FILTER_BY_RFQ_NO], StringComparison.OrdinalIgnoreCase) >= 0 ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied

                        if (filterParams.ContainsKey(this.FILTER_BY_SUBMIT_DATE_FROM) && filterParams.ContainsKey(this.FILTER_BY_SUBMIT_DATE_TO))
                        {
                            //Parse the create date as stored in the object
                            String createDate = tempDefectObj.getDateCreated().Substring(0, tempDefectObj.getDateCreated().IndexOf(" "));
                            DateTime createDateVal;
                            DateTime.TryParseExact(createDate, "%M/%d/yyyy", null, DateTimeStyles.AllowWhiteSpaces, out createDateVal);

                            //parse the from create date as passed in the filter parameter
                            DateTime createDateFromVal;
                            DateTime.TryParseExact(filterParams[this.FILTER_BY_SUBMIT_DATE_FROM], "yyyy-MM-dd", null, DateTimeStyles.None, out createDateFromVal);
                            DateTime createDateToVal;
                            DateTime.TryParseExact(filterParams[this.FILTER_BY_SUBMIT_DATE_TO], "yyyy-MM-dd", null, DateTimeStyles.None, out createDateToVal);


                            if (DateTime.Compare(createDateVal, createDateFromVal) >= 0 && DateTime.Compare(createDateVal, createDateToVal) <= 0)
                                filterConditionSatisfied = filterConditionSatisfied & true;
                            else
                                filterConditionSatisfied = false;
                        }
                        if (filterConditionSatisfied)
                        {
                            allDefectFilteredList.Add(tempDefectObj);
                        }
                    }
                }

                return allDefectFilteredList;
            }

            public ArrayList getAllSRsFilteredORDERBYCloseDate(String context, String searchForId, Dictionary<String, String> filterParams)
            {
                if (context == null || context.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid context value sent to method _dispDefectDetails.getAllDefectsFiltered"));

                if (searchForId == null || searchForId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid search for id value sent to method _dispDefectDetails.getAllDefectsFiltered"));


                ArrayList allDefectList = new ArrayList();
                ArrayList allDefectFilteredList = new ArrayList();

                if (context.Equals("incoming"))
                {
                    allDefectList = BackEndObjects.DefectDetails.getAllSRDetailsforSupplierIdDBORDERBYCloseDate(searchForId);
                    /*foreach (KeyValuePair<String, DefectDetails> kvp in defectDict)
                        allDefectList.Add(kvp.Value);*/
                }
                else
                {
                    allDefectList = BackEndObjects.DefectDetails.getAllSRDetailsforCustomerIdDBORDERBYCloseDate(searchForId);
                    /*foreach (KeyValuePair<String, DefectDetails> kvp in defectDict)
                        allDefectList.Add(kvp.Value);*/
                }
                int i = 0;

                for (i = 0; i < allDefectList.Count; i++)
                {
                    DefectDetails tempDefectObj = (DefectDetails)allDefectList[i];
                    Boolean filterConditionSatisfied = false;


                    if (filterParams.ContainsKey(this.FILTER_BY_DEFECT_NO))
                    {

                        if (tempDefectObj.getDefectId().IndexOf(filterParams[this.FILTER_BY_DEFECT_NO], StringComparison.OrdinalIgnoreCase) >= 0)
                            filterConditionSatisfied = true;
                    }

                    else
                    {
                        filterConditionSatisfied = true;
                    }

                    if (filterConditionSatisfied)
                    {
                        if (filterParams.ContainsKey(this.FILTER_BY_DEFECT_STAT))
                            filterConditionSatisfied = tempDefectObj.getDefectStat().Equals(filterParams[this.FILTER_BY_DEFECT_STAT]) ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterParams.ContainsKey(this.FILTER_BY_DEFECT_RESOL_STAT))
                            filterConditionSatisfied = tempDefectObj.getResolStat().Equals(filterParams[this.FILTER_BY_DEFECT_RESOL_STAT]) ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterParams.ContainsKey(this.FILTER_BY_INVOICE_NO))
                            filterConditionSatisfied = tempDefectObj.getInvoiceId().IndexOf(filterParams[this.FILTER_BY_INVOICE_NO], StringComparison.OrdinalIgnoreCase) >= 0 ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterParams.ContainsKey(this.FILTER_BY_RFQ_NO))
                            filterConditionSatisfied = tempDefectObj.getRFQId().IndexOf(filterParams[this.FILTER_BY_RFQ_NO], StringComparison.OrdinalIgnoreCase) >= 0 ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied

                        if (filterParams.ContainsKey(this.FILTER_BY_SUBMIT_DATE_FROM) && filterParams.ContainsKey(this.FILTER_BY_SUBMIT_DATE_TO))
                        {
                            //Parse the create date as stored in the object
                            String createDate = tempDefectObj.getDateCreated().Substring(0, tempDefectObj.getDateCreated().IndexOf(" "));
                            DateTime createDateVal;
                            DateTime.TryParseExact(createDate, "%M/%d/yyyy", null, DateTimeStyles.AllowWhiteSpaces, out createDateVal);

                            //parse the from create date as passed in the filter parameter
                            DateTime createDateFromVal;
                            DateTime.TryParseExact(filterParams[this.FILTER_BY_SUBMIT_DATE_FROM], "yyyy-MM-dd", null, DateTimeStyles.None, out createDateFromVal);
                            DateTime createDateToVal;
                            DateTime.TryParseExact(filterParams[this.FILTER_BY_SUBMIT_DATE_TO], "yyyy-MM-dd", null, DateTimeStyles.None, out createDateToVal);


                            if (DateTime.Compare(createDateVal, createDateFromVal) >= 0 && DateTime.Compare(createDateVal, createDateToVal) <= 0)
                                filterConditionSatisfied = filterConditionSatisfied & true;
                            else
                                filterConditionSatisfied = false;
                        }
                        if (filterConditionSatisfied)
                        {
                            allDefectFilteredList.Add(tempDefectObj);
                        }
                    }
                }

                return allDefectFilteredList;
            }
        }
    }
    /// <summary>
    /// This class can be used to process all purchase related actions
    /// This class stores all the business logics required to perform the purchase related actions;
    /// it interacts with backend object namespace and dbcon.
    /// </summary>
    public class SalesActions
    {
        /// <summary>
        /// This class contains methods to create a potential.
        /// There are multple ways of creating a potential -
        /// 1. Convert a Manually created lead to a potential
        /// 2. Create a Manully created Potential without a lead entry
        /// 3. An automatic potential entry created once a Lead entry is shortlisted by the customer.
        /// </summary>
        public class _createPotential
        {
            /// <summary>
            /// This method returns all product/service category details for which there is no parent category;
            ///  i.e, it returns a list of all top level product/service category.This method is particularly useful for scenarios where it is required to
            ///  list the main catogories online.
            /// </summary>
            /// <returns></returns>
            public Dictionary<String, ProductCategory> getParentProductDetails()
            {
                return ProductCategory.getAllParentCategory();
            }
            /// <summary>
            /// for any given parent categoryid  returns all the associated child cateogories
            /// </summary>
            /// <returns></returns>
            public Dictionary<String, ProductCategory> getChildProductDetails(String catId)
            {
                return ProductCategory.getAllChildCategoryDB(catId);
            }
            /// <summary>
            /// for any given product category id, returns all the associated Feature objects.
            /// Each of the elements of the returned dictionary has a Feature object which in turn contains all the specifications attached to it.
            /// </summary>
            /// <param name="catId"></param>
            /// <returns></returns>
            public Dictionary<String, Features> getAllFeaturesForCategory(String catId)
            {
                Dictionary<String, Features> prodFeat = ProductCategory.getFeatureforCategoryDB(catId);

                foreach (KeyValuePair<String, Features> kvp in prodFeat)
                {
                    //Get all specifications related to each of the feature object
                    Dictionary<String, Specifications> tempSpec = Features.getSpecforFeatureDB(kvp.Key);

                    //Put all the specification objects in an arrayList , because feature object's specification property is an arraylist
                    ArrayList allSpecListforFeat = new ArrayList();
                    foreach (KeyValuePair<String, Specifications> kvps in tempSpec)
                        allSpecListforFeat.Add(kvps.Value);
                    //Add the specification ArrayList into the Feature object
                    kvp.Value.setSpecifications(allSpecListforFeat);
                }

                return prodFeat;
            }
            /// <summary>
            /// Returns all units of measurement from database. Each element of the returned ArrayList is an object of 'UnitOfMsrmnt'.
            /// Use 'getUnitName()' method on each of the containing objects to get the name of the units.
            /// </summary>
            /// <returns></returns>
            public ArrayList getAllUnitsOfMsrmnt()
            {
                return UnitOfMsrmnt.getAllMsrmntUnitsDB();
            }
            /// <summary>
            /// Returns an ArrayList which contains only the names of all the available currecnies in the database
            /// </summary>
            /// <returns></returns>
            public ArrayList getAllCurrencies()
            {
                return Currency.getAllCurrencyNamesDB();
            }
            /// <summary>
            /// Returns all entries of the database table 'Quote_Units'returns all the quote units objects from database.
            /// Each element of the returned entity has the property 'unitAndDivisor' which is a Dictionary<String,float> of a single record.
            /// use the 'getUnitAndDivisor'/'setUnitAndDivisor' as getter/setter for the property.
            /// The 'key' of the Dictionary<String,float> is 'unit name' and the 'value' is the respective divisor of the quote unit.
            /// </summary>
            /// <returns></returns>
            public ArrayList getAllQuoteUnits()
            {
                return QuoteUnits.getAllQuoteUnitsDB();
            }
            /// <summary>
            /// This method returns a dictionary; each element of the dictionary is the id of the country (the 'key') and the
            /// name of the country (the 'value').
            /// </summary>
            /// <returns></returns>
            public Dictionary<String, String> getAllCountryNames()
            {
                Dictionary<String, Country> countryDict = Country.getAllCountrywoStatesDB();
                Dictionary<String, String> countryNamesDict = new Dictionary<string, string>();

                ArrayList allCountryNames = new ArrayList();
                foreach (KeyValuePair<String, Country> kvp in countryDict)
                    countryNamesDict.Add(kvp.Key, kvp.Value.getCountryName());

                return countryNamesDict;
            }
            /// <summary>
            /// For a given country id,this method returns a dictionary; each element of the dictionary is the id of the State (the 'key') and the
            /// name of the State (the 'value').
            /// </summary>
            /// <param name="cId"></param>
            /// <returns></returns>
            public Dictionary<String, String> getAllStateNamesForCountry(String cId)
            {
                Dictionary<String, State> tempStateData = State.getStatesforCountryDB(cId);
                Dictionary<String, String> allStateNames = new Dictionary<string, string>();

                foreach (KeyValuePair<String, State> kvp in tempStateData)
                    allStateNames.Add(kvp.Key, kvp.Value.getStateName());

                return allStateNames;
            }
            /// <summary>
            /// For a given state id,this method returns a dictionary; each element of the dictionary is the id of the City (the 'key') and the
            /// name of the City (the 'value').
            /// </summary>
            /// <param name="stId"></param>
            /// <returns></returns>
            public Dictionary<String, String> getAllCityNamesForState(String stId)
            {
                Dictionary<String, City> tempCityData = City.getCitiesforStateDB(stId);
                Dictionary<String, String> allCityNames = new Dictionary<string, string>();

                foreach (KeyValuePair<String, City> kvp in tempCityData)
                    allCityNames.Add(kvp.Key, kvp.Value.getCityName());

                return allCityNames;

            }
            /// <summary>
            /// For a given City id,this method returns a dictionary; each element of the dictionary is the id of the Locality (the 'key') and the
            /// name of the Locality (the 'value').
            /// </summary>
            /// <param name="ctId"></param>
            /// <returns></returns>
            public Dictionary<String, String> getAllLocalityNamesForCity(String ctId)
            {
                Dictionary<String, Localities> tempLocalityData = Localities.getLocalitiesforCityDB(ctId);
                Dictionary<String, String> allLocalityNames = new Dictionary<string, string>();

                foreach (KeyValuePair<String, Localities> kvp in tempLocalityData)
                    allLocalityNames.Add(kvp.Key, kvp.Value.getLocalityName());

                return allLocalityNames;

            }
            /// <summary>
            /// For a given entity id, it returns an ArrayList containing 'Contacts' object from database.
            /// This list is the contact list of this particular organization.
            /// </summary>
            /// <param name="entityId"></param>
            /// <param name="userId"></param>
            /// <returns></returns>
            public ArrayList getAllContactsForEntity(String entityId, String userId)
            {
                if (entityId == null || entityId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid entity id value sent to method _createPotential.getAllContactsForEntity"));

                if (userId == null || userId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid userId id value sent to method _createPotential.getAllContactsForEntity"));

                return Contacts.getAllContactsbyEntityIdDB(entityId);
            }
            /// <summary>
            /// Given a response entity id, user id and the RFQ id this method will
            /// create a potential entry for the respective response entity id and RFQ id.
            /// </summary>
            /// <param name="respEntId"></param>
            /// <param name="userId"></param>
            /// <param name="rfId"></param>
            /// <returns></returns>
            public int convertManulLeadToPotential(String respEntId, String userId, String rfId)
            {
                if (respEntId == null || respEntId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid entity id value sent to method _createPotential._convertManulLeadToPotential"));

                if (userId == null || userId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid userId id value sent to method _createPotential._convertManulLeadToPotential"));

                if (!RFQDetails.getRFQDetailsbyIdDB(rfId.Trim()).getCreateMode().Equals(RFQDetails.CREATION_MODE_MANUAL))
                  throw (new CustomExceptions.businessRuleViolationException("Only a Manually Created lead can be converted to Potential Manually - method _createPotential.convertManulLeadToPotential"));


                RFQShortlisted potObj = new RFQShortlisted();
                RFQResponseQuoteTotal respTotal = new RFQResponseQuoteTotal();
                ArrayList respToalQuoteList = new ArrayList();

                Dictionary<String, RFQResponseQuotes> respQuoteObjs = RFQResponseQuotes.getAllResponseQuotesforRFQandResponseEntityDB(rfId, respEntId);
                foreach (KeyValuePair<String, RFQResponseQuotes> kvp in respQuoteObjs)
                {
                    RFQResponseQuotes tempObj = kvp.Value;
                    RFQResponseQuoteRecord qRec = new RFQResponseQuoteRecord();
                    qRec.setRFQId(rfId);
                    qRec.setUnitName(tempObj.getUnitName());
                    qRec.setResponseUsrId(tempObj.getResponseUsrId());
                    qRec.setResponseEntityId(tempObj.getResponseEntityId());
                    qRec.setQuote(tempObj.getQuote());
                    qRec.setPrdCatId(tempObj.getPrdCatId());

                    qRec.setAmntQuntyandProdSpec();
                    respToalQuoteList.Add(respToalQuoteList);
                }
                respTotal.setRFQResponseQuoteRecordList(respToalQuoteList);
                respTotal.calculateTotalAmnt(); //Now calculate the toal amount of the potential

                Id idGenerator = new Id();
                potObj.setPotentialId(idGenerator.getNewId(Id.ID_TYPE_POTENTIAL_ID_STRING));
                potObj.setRFQId(rfId.Trim());
                potObj.setRespEntityId(respEntId.Trim());
                potObj.setPotStat(PotentialStatus.POTENTIAL_STAT_PRELIM);
                potObj.setPotenAmnt(respTotal.getTotalAmnt());
                potObj.setPotActStat(RFQShortlisted.POTENTIAL_ACTIVE_STAT_ACTIVE);
                potObj.setFinlSupFlag("N");
                potObj.setFinlCustFlag("N");
                potObj.setCreateMode(RFQShortlisted.POTENTIAL_CREATION_MODE_MANUAL);
                DateTime dt = DateTime.Now;
                String currDateTimeVal = dt.ToString("yyyy-MM-dd HH:mm:ss");
                potObj.setCreatedDate(currDateTimeVal);

                return RFQShortlisted.insertRFQShorListedEntryDB(potObj);
            }
            /// <summary>
            /// This method creates a Potential entry when created manually.
            /// Along with creating a new potential records in the backend, this method creates the respetive Lead entry and the
            /// response quote records (if any).
            /// The second parameter is an ArrayList of 'RFQResponseQuoteRecord' objects. To populate the backend potential record with the proper
            /// amount, amount value for each of this record should be passed.
            /// The potential creation mode will automatically be set to 'Manual'.
            /// </summary>
            /// <returns></returns>
            public int createPotentialManually(RFQDetails reqObj, ArrayList respQuoteRecs, String respEntId, String userId)
            {
                if (reqObj == null)
                    throw (new CustomExceptions.invalidParamException("Invalid 'RFQDetails' object sent to method _createPotential.createPotentialManually"));
                if (reqObj.getCreateMode() == null || !reqObj.getCreateMode().Equals(RFQDetails.CREATION_MODE_MANUAL))
                    throw (new CustomExceptions.businessRuleViolationException("For a Manually created Potential entry creation mode should be always be Manual - from method _createPotential.createPotentialManually"));
                if (respEntId == null || respEntId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid entity id value sent to method _createPotential.createPotentialManually"));
                if (userId == null || userId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid userId id value sent to method _createPotential.createPotentialManually"));
                if (!reqObj.getCreatedEntity().Equals(respEntId.Trim()))
                    throw (new CustomExceptions.businessRuleViolationException("For Manually created Potential entry the createdEntity value of RFQDetails object must match the response entity id value"));

                int rowsAffected = 0;

                float totalAmnt = 0;
                DateTime dt = DateTime.Now;
                String currDateTimeVal = dt.ToString("yyyy-MM-dd HH:mm:ss");

                Id IdGenerator = new Id();
                reqObj.setRFQId(IdGenerator.getNewId(Id.ID_TYPE_RFQ_STRING));
                rowsAffected += RFQDetails.insertRFQDetailsDB(reqObj);

                //If the reponse quote details are sent insert those as well
                if (respQuoteRecs != null && respQuoteRecs.Count > 0)
                {
                    RFQResponseQuoteRecord tempRec = (RFQResponseQuoteRecord)respQuoteRecs[0];
                    RFQResponse rfqResp = new RFQResponse();

                    rfqResp.setRFQId(reqObj.getRFQId());
                    rfqResp.setRespEntityId(respEntId.Trim());
                    rfqResp.setRespDate(currDateTimeVal);

                    rowsAffected += RFQResponse.insertRFQResponseDB(rfqResp);

                    for (int count = 0; count > respQuoteRecs.Count; count++)
                    {
                        RFQResponseQuoteRecord tempRespRec = (RFQResponseQuoteRecord)respQuoteRecs[count];
                        totalAmnt += tempRespRec.getAmount();
                        RFQResponseQuotes tempRfqResp = new RFQResponseQuotes();
                        tempRfqResp.setRFQId(reqObj.getRFQId());
                        tempRfqResp.setUnitName(tempRespRec.getUnitName());
                        tempRfqResp.setResponseUsrId(userId.Trim());
                        tempRfqResp.setResponseEntityId(respEntId.Trim());
                        tempRfqResp.setQuote(tempRespRec.getQuote());
                        tempRfqResp.setPrdCatId(tempRespRec.getPrdCatId());

                        rowsAffected += RFQResponseQuotes.insertRFQResponseQuotesDB(tempRfqResp);

                    }
                }


                RFQShortlisted potObj = new RFQShortlisted();
                Id idGenerator = new Id();
                potObj.setPotentialId(idGenerator.getNewId(Id.ID_TYPE_POTENTIAL_ID_STRING));
                potObj.setRFQId(reqObj.getRFQId());
                potObj.setRespEntityId(respEntId.Trim());
                potObj.setPotStat(PotentialStatus.POTENTIAL_STAT_PRELIM);
                potObj.setPotenAmnt(totalAmnt);
                potObj.setPotActStat(RFQShortlisted.POTENTIAL_ACTIVE_STAT_ACTIVE);
                potObj.setFinlSupFlag("N");
                potObj.setFinlCustFlag("N");
                potObj.setCreateMode(RFQShortlisted.POTENTIAL_CREATION_MODE_MANUAL);
                potObj.setCreatedDate(currDateTimeVal);

                rowsAffected += RFQShortlisted.insertRFQShorListedEntryDB(potObj);

                return rowsAffected;
            }
            /// <summary>
            /// This method needs to be called when a user shortlists a Lead entry - that lead entry is automatically,
            /// shown as a potential for the response entity id.
            ///  Given a response entity id, user id and the RFQ id this method will create a potential entry for the respective response entity id and RFQ id
            /// </summary>
            /// <param name="respEntId"></param>
            /// <param name="userId"></param>
            /// <param name="rfId"></param>
            /// <returns></returns>
            public int convertLeadToPotential(String respEntId, String userId, String rfId)
            {
                if (respEntId == null || respEntId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid entity id value sent to method _createPotential.convertLeadToPotential"));

                if (userId == null || userId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid userId id value sent to method _createPotential.convertLeadToPotential"));

                if (!RFQDetails.getRFQDetailsbyIdDB(rfId.Trim()).getCreateMode().Equals(RFQDetails.CREATION_MODE_AUTO))
                    throw (new CustomExceptions.businessRuleViolationException("Only a Auto Created lead can be converted to Potential Automatically - method _createPotential.convertLeadToPotential"));

                RFQShortlisted potObj = new RFQShortlisted();
                RFQResponseQuoteTotal respTotal = new RFQResponseQuoteTotal();
                ArrayList respToalQuoteList = new ArrayList();

                Dictionary<String, RFQResponseQuotes> respQuoteObjs = RFQResponseQuotes.getAllResponseQuotesforRFQandResponseEntityDB(rfId, respEntId);
                foreach (KeyValuePair<String, RFQResponseQuotes> kvp in respQuoteObjs)
                {
                    RFQResponseQuotes tempObj = kvp.Value;
                    RFQResponseQuoteRecord qRec = new RFQResponseQuoteRecord();
                    qRec.setRFQId(rfId);
                    qRec.setUnitName(tempObj.getUnitName());
                    qRec.setResponseUsrId(tempObj.getResponseUsrId());
                    qRec.setResponseEntityId(tempObj.getResponseEntityId());
                    qRec.setQuote(tempObj.getQuote());
                    qRec.setPrdCatId(tempObj.getPrdCatId());

                    qRec.setAmntQuntyandProdSpec();
                    respToalQuoteList.Add(respToalQuoteList);
                }
                respTotal.setRFQResponseQuoteRecordList(respToalQuoteList);
                respTotal.calculateTotalAmnt(); //Now calculate the toal amount of the potential

                Id idGenerator = new Id();
                potObj.setPotentialId(idGenerator.getNewId(Id.ID_TYPE_POTENTIAL_ID_STRING));
                potObj.setRFQId(rfId.Trim());
                potObj.setRespEntityId(respEntId.Trim());
                potObj.setPotStat(PotentialStatus.POTENTIAL_STAT_PRELIM);
                potObj.setPotenAmnt(respTotal.getTotalAmnt());
                potObj.setPotActStat(RFQShortlisted.POTENTIAL_ACTIVE_STAT_ACTIVE);
                potObj.setFinlSupFlag("N");
                potObj.setFinlCustFlag("N");
                potObj.setCreateMode(RFQShortlisted.POTENTIAL_CREATION_MODE_AUTO);
                DateTime dt = DateTime.Now;
                String currDateTimeVal = dt.ToString("yyyy-MM-dd HH:mm:ss");
                potObj.setCreatedDate(currDateTimeVal);

                return RFQShortlisted.insertRFQShorListedEntryDB(potObj);
            }
        }
        /// <summary>
        /// Each instance of this class represents a potential.
        /// </summary>
        public class _dispPotentials
        {
            public String FILTER_BY_PROD_CAT = "FILTER_BY_PROD_CAT";
            public String FILTER_BY_ACTIVE_STATUS = "FILTER_BY_ACTIVE_STATUS";
            public String FILTER_BY_STAGE = "FILTER_BY_STAGE";
            /// <summary>
            /// This filter parameter is not implemented as of now
            /// </summary>
            public String FILTER_BY_REQUEST_FOR_FINALIZATION = "FILTER_BY_REQUEST_FOR_FINALIZATION";
            public String FILTER_BY_CREATE_DATE_FROM = "FILTER_BY_CREATE_DATE_FROM";
            public String FILTER_BY_CREATE_DATE_TO = "FILTER_BY_CREATE_DATE_TO";
            public String FILTER_BY_DUE_DATE_FROM = "FILTER_BY_DUE_DATE_FROM";
            public String FILTER_BY_DUE_DATE_TO = "FILTER_BY_DUE_DATE_TO";
            public String FILTER_BY_CUST_ID = "FILTER_BY_CUST_ID";
            public String FILTER_BY_RFQ_NO = "FILTER_BY_RFQ_NO";
            public String FILTER_BY_ASSGND_TO = "FILTER_BY_ASSGND_TO";

            /// <summary>
            /// For a given response entity id, this methods returns all potentials in an ArrayList.
            /// Each object of this ArrayList is an object of type 'PotentialRecords' class (which is a subclass of 'RFQShortlisted' class).
            /// This method does not provide the option to filter the list.
            /// Note that this method only returns the arraylist of potential objects, associated specification details and NDA document are note sent.
            /// </summary>
            /// <param name="respEntityId"></param>
            /// <param name="userId"></param>
            /// <returns></returns>
            public ArrayList getAllPotentials(String respEntityId, String userId)
            {
                //The second parameter is not used as of now, but will be used once the role based access control logic is introduced

                if (respEntityId == null || respEntityId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid entity id value sent to method _dispPotentials.getAllPotentials"));

                if (userId == null || userId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid userId id value sent to method _dispPotentials.getAllPotentials"));

                ArrayList allPotentials = new ArrayList();
                ArrayList allPotentialsRecords = new ArrayList();
                allPotentials = RFQShortlisted.getAllRFQShortListedbyEntityIdDB(respEntityId);

                for (int count = 0; count < allPotentials.Count; count++)
                {
                    RFQShortlisted tempPotn = (RFQShortlisted)allPotentials[count];
                    RFQDetails rfqMainObj= RFQDetails.getRFQDetailsbyIdDB(tempPotn.getRFQId());
                    PotentialRecords tempPotnRec = new PotentialRecords();
                    tempPotnRec.setRFQId(tempPotn.getRFQId());
                    tempPotnRec.setRespEntityId(tempPotn.getRespEntityId());
                    tempPotnRec.setPotStat(tempPotn.getPotStat());
                    tempPotnRec.setPotentialId(tempPotn.getPotentialId());
                    tempPotnRec.setPotenAmnt(tempPotn.getPotenAmnt());
                    tempPotnRec.setPotActStat(tempPotn.getPotActStat());
                    tempPotnRec.setFinlSupFlag(tempPotn.getFinlSupFlag());
                    tempPotnRec.setFinlCustFlag(tempPotn.getFinlCustFlag());
                    tempPotnRec.setDueDate(rfqMainObj.getDueDate());
                    tempPotnRec.setCreateMode(tempPotn.getCreateMode());
                    tempPotnRec.setCreatedDate(tempPotn.getCreatedDate());
                    tempPotnRec.setConfMatPath(tempPotn.getConfMatPath());
                    tempPotnRec.setEntityId(rfqMainObj.getEntityId());
                    tempPotnRec.setCreatedBy(rfqMainObj.getCreatedUsr());
                    tempPotnRec.setCreatedByEntity(rfqMainObj.getCreatedEntity());
                    tempPotnRec.setRFQName(rfqMainObj.getRFQName());
                    tempPotnRec.setCurrency(rfqMainObj.getCurrency());

                    tempPotnRec.findAndSetEntityName();
                    allPotentialsRecords.Add(tempPotnRec);
                }

                return allPotentialsRecords;
            }
            /// <summary>
            /// Returns all Active potentials which are assigned to the given user along belongs to the given entity id
            /// </summary>
            /// <param name="respEntityId"></param>
            /// <param name="userId"></param>
            /// <returns></returns>
            public ArrayList getAllActiveAndAssignedToPotentials(String respEntityId, String userId)
            {
                //The second parameter is not used as of now, but will be used once the role based access control logic is introduced

                if (respEntityId == null || respEntityId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid entity id value sent to method _dispPotentials.getAllAssignedToPotentials"));

                if (userId == null || userId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid userId id value sent to method _dispPotentials.getAllAssignedToPotentials"));

                ArrayList allPotentials = new ArrayList();
                ArrayList allPotentialsRecords = new ArrayList();
                allPotentials = RFQShortlisted.getAllRFQShortListedbyEntityIdAndAssignedToUser(respEntityId,userId);

                for (int count = 0; count < allPotentials.Count; count++)
                {
                    RFQShortlisted tempPotn = (RFQShortlisted)allPotentials[count];
                    RFQDetails rfqMainObj = RFQDetails.getRFQDetailsbyIdDB(tempPotn.getRFQId());
                    PotentialRecords tempPotnRec = new PotentialRecords();
                    tempPotnRec.setRFQId(tempPotn.getRFQId());
                    tempPotnRec.setRespEntityId(tempPotn.getRespEntityId());
                    tempPotnRec.setPotStat(tempPotn.getPotStat());
                    tempPotnRec.setPotentialId(tempPotn.getPotentialId());
                    tempPotnRec.setPotenAmnt(tempPotn.getPotenAmnt());
                    tempPotnRec.setPotActStat(tempPotn.getPotActStat());
                    tempPotnRec.setFinlSupFlag(tempPotn.getFinlSupFlag());
                    tempPotnRec.setFinlCustFlag(tempPotn.getFinlCustFlag());
                    tempPotnRec.setDueDate(rfqMainObj.getDueDate());
                    tempPotnRec.setCreateMode(tempPotn.getCreateMode());
                    tempPotnRec.setCreatedDate(tempPotn.getCreatedDate());
                    tempPotnRec.setConfMatPath(tempPotn.getConfMatPath());
                    tempPotnRec.setEntityId(rfqMainObj.getEntityId());
                    tempPotnRec.setCreatedBy(rfqMainObj.getCreatedUsr());
                    tempPotnRec.setCreatedByEntity(rfqMainObj.getCreatedEntity());
                    tempPotnRec.setRFQName(rfqMainObj.getRFQName());

                    tempPotnRec.findAndSetEntityName();
                    allPotentialsRecords.Add(tempPotnRec);
                }

                return allPotentialsRecords;
            }          
            /// <summary>
            /// For a given entity id,user id and RFQ id this method returns a Dictionary of 'RFQResponseQuotes'.
            ///  Each record of the returned dictionary contains the 'Product/Service Category Id' as the 'key' and the respective 'RFQResponseQuotes' object as 'value'.
            ///  A more versatile method is 'getPotentialQuoteandSpecDetails'.
            /// </summary>
            /// <param name="cmpUsrId"></param>
            /// <param name="userId"></param>
            /// <param name="rfqId"></param>
            /// <returns></returns>
            public Dictionary<String, RFQResponseQuotes> getPotentialQuoteDetails(String cmpUsrId, String userId, String potId)
            {
                if (cmpUsrId == null || cmpUsrId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid entity id value sent to method _dispPotentials.getPotentialQuoteDetails"));

                if (userId == null || userId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid userId id value sent to method _dispPotentials.getPotentialQuoteDetails"));

                if (potId == null || potId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid potential id value sent to method _dispPotentials.getPotentialQuoteDetails"));

                ArrayList finalList = new ArrayList();

                RFQShortlisted potentialRecord = RFQShortlisted.getAllRFQShortListedbyPotentialIdDB(potId);
                if (!potentialRecord.Equals(potentialRecord.getRespEntityId()))
                    throw (new CustomExceptions.invalidParamException("Unauthorized access to potential record from  _dispPotentials.getPotentialQuoteDetails"));

                return RFQResponseQuotes.getAllResponseQuotesforRFQandResponseEntityDB
                    (potentialRecord.getRFQId(), potentialRecord.getRespEntityId());

            }
            /// <summary>
            /// For a given entity id, user id and potential id, it returns a dictionary of 'RFQProductServiceDetails'.
            /// Each record of the returned dictionary contains the 'Product/Service Category Id' as the 'key' and the respective 'RFQProductServiceDetails' object as 'value'.
            /// A more versatile method is 'getPotentialQuoteandSpecDetails'.
            /// </summary>
            /// <param name="cmpUsrId"></param>
            /// <param name="userId"></param>
            /// <param name="potId"></param>
            /// <returns></returns>
            public Dictionary<String, RFQProductServiceDetails> getPotentialSpecificationDetails(String cmpUsrId, String userId, String potId)
            {
                if (cmpUsrId == null || cmpUsrId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid entity id value sent to method _dispPotentials.getPotentialSpecificationDetails"));

                if (userId == null || userId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid userId id value sent to method _dispPotentials.getPotentialSpecificationDetails"));

                if (potId == null || potId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid potential id value sent to method _dispPotentials.getPotentialSpecificationDetails"));

                //return RFQProductServiceDetails.getAllProductServiceDetailsbyRFQIdDB(RFQShortlisted.getAllRFQShortListedbyPotentialIdDB(potId).getRFQId());
                return new Dictionary<string, RFQProductServiceDetails>();
            }
            /// <summary>
            /// This is same as 'getAllPotentials(String,String)' except, here there is a third parameter which can be used to specify the filter conditions.
            /// The third parameter is a dictionary which contains FILTER parameter name as 'key' and the respective filter value as the 'value'.
            /// Note that, all the filter parameters name must be chosen from the 'FILTER_BY_' properties of this class.
            /// Also, all the date values passed as filter parameter must be passed in 'yyyy-MM-dd' format.
            /// The return value is an Arraylist of 'PotentialRecords' objects
            /// </summary>
            /// <param name="respEntityId"></param>
            /// <param name="userId"></param>
            /// <param name="filterParam"></param>
            /// <returns></returns>
            public ArrayList getAllPotentialsFiltered(String respEntityId, String userId, Dictionary<String, String> filterParam)
            {
                if (respEntityId == null || respEntityId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid entity id value sent to method _dispPotentials.getAllPotentialsFiltered"));

                if (userId == null || userId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid userId id value sent to method _dispPotentials.getAllPotentialsFiltered"));


                ArrayList allPotnList = new ArrayList();
                ArrayList allPotnListFiltered = new ArrayList();

                allPotnList = RFQShortlisted.getAllRFQShortListedbyEntityIdDB(respEntityId);
                int i = 0;

                for (i = 0; i < allPotnList.Count; i++)
                {
                    RFQShortlisted tempPotn = (RFQShortlisted)allPotnList[i];
                    RFQDetails rfqMainObj = RFQDetails.getRFQDetailsbyIdDB(tempPotn.getRFQId());
                    Boolean filterConditionSatisfied = true;

                    //ArrayList potSpecList = RFQProductServiceDetails.getAllProductServiceDetailsbyRFQIdDB(tempPotn.getRFQId());
                    
                    //ArrayList potSpecList = new ArrayList();

                    //foreach (KeyValuePair<String, RFQProductServiceDetails> kvp in potSpecDict)
                        //potSpecList.Add(kvp.Value);
                    if (filterParam.ContainsKey(this.FILTER_BY_RFQ_NO))
                        filterConditionSatisfied = rfqMainObj.getRFQId().IndexOf(filterParam[this.FILTER_BY_RFQ_NO], StringComparison.InvariantCultureIgnoreCase) >= 0 ? filterConditionSatisfied & true : false; 
         
                    if (filterConditionSatisfied)
                    {
                        if (filterParam.ContainsKey(this.FILTER_BY_ACTIVE_STATUS))
                            filterConditionSatisfied = tempPotn.getPotActStat().Equals(filterParam[this.FILTER_BY_ACTIVE_STATUS]) ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_CUST_ID))
                            filterConditionSatisfied = rfqMainObj.getEntityId().Equals(filterParam[this.FILTER_BY_CUST_ID]) ? filterConditionSatisfied & true : false;
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_STAGE))
                            filterConditionSatisfied = tempPotn.getPotStat().Equals(filterParam[this.FILTER_BY_STAGE]) ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_CREATE_DATE_FROM) && filterParam.ContainsKey(this.FILTER_BY_CREATE_DATE_TO))
                        {
                            //Parse the create date as stored in the object                            
                            String createDate = tempPotn.getCreatedDate().Substring(0, tempPotn.getCreatedDate().IndexOf(" "));
                            DateTime createDateVal;
                            DateTime.TryParseExact(createDate, "%M/%d/yyyy", null, DateTimeStyles.AllowWhiteSpaces, out createDateVal);

                            //parse the from create date as passed in the filter parameter
                            DateTime createDateFromVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_CREATE_DATE_FROM], "yyyy-MM-dd", null, DateTimeStyles.None, out createDateFromVal);
                            DateTime createDateToVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_CREATE_DATE_TO], "yyyy-MM-dd", null, DateTimeStyles.None, out createDateToVal);


                            if (DateTime.Compare(createDateVal, createDateFromVal) >= 0 && DateTime.Compare(createDateVal, createDateToVal) <= 0)
                                filterConditionSatisfied = filterConditionSatisfied & true;
                            else
                                filterConditionSatisfied = false;
                        }
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_DUE_DATE_FROM) && filterParam.ContainsKey(this.FILTER_BY_DUE_DATE_TO))
                        {
                            //Parse the create date as stored in the object
                            String dueDate = rfqMainObj.getDueDate().Replace(" ","  ").Substring(0, 10);
                            DateTime dueDateVal;
                            DateTime.TryParseExact(dueDate, "%M/%d/yyyy", null, DateTimeStyles.AllowWhiteSpaces, out dueDateVal);

                            //parse the from create date as passed in the filter parameter
                            DateTime dueDateFromVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_DUE_DATE_FROM], "yyyy-MM-dd", null, DateTimeStyles.None, out dueDateFromVal);
                            DateTime dueDateToVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_DUE_DATE_TO], "yyyy-MM-dd", null, DateTimeStyles.None, out dueDateToVal);


                            if (DateTime.Compare(dueDateVal, dueDateFromVal) >= 0 && DateTime.Compare(dueDateVal, dueDateToVal) <= 0)
                                filterConditionSatisfied = filterConditionSatisfied & true;
                            else
                                filterConditionSatisfied = false;
                        }
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_PROD_CAT))
                    {
                        int j = 0;
                            ArrayList potSpecList = RFQProdServQnty.getRFQProductServiceQuantityforRFIdDB(tempPotn.getRFQId());
                            filterConditionSatisfied=false;
                        //Even if one of the specificaion object matches the filter criteria, then allow this selection
                        for (j = 0; j < potSpecList.Count; j++)
                        {
                            RFQProdServQnty tempSpec = (RFQProdServQnty)potSpecList[j];
                            if (tempSpec.getProdCatId().Equals(filterParam[this.FILTER_BY_PROD_CAT]) || BackEndObjects.ProductCategory.
                                getRootLevelParentCategoryDB(tempSpec.getProdCatId()).ContainsKey(filterParam[this.FILTER_BY_PROD_CAT]))
                            { filterConditionSatisfied = true; break; }
                        }

                    }
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_ASSGND_TO))
                        {
                            String assgnTo = RFQResponse.getRFQResponseWOQuotesForRFQIdandResponseEntityIdDB(tempPotn.getRFQId(), respEntityId).getAssignedTo();
                            filterConditionSatisfied = (assgnTo != null && assgnTo.Equals(filterParam[FILTER_BY_ASSGND_TO])) ? filterConditionSatisfied & true : false;
                        }
                        if (filterConditionSatisfied)
                        {
                            PotentialRecords tempPotnRec = new PotentialRecords();
                            
                            tempPotnRec.setRFQId(tempPotn.getRFQId());
                            tempPotnRec.setRFQName(rfqMainObj.getRFQName());
                            tempPotnRec.setRespEntityId(tempPotn.getRespEntityId());
                            tempPotnRec.setPotStat(tempPotn.getPotStat());
                            tempPotnRec.setPotentialId(tempPotn.getPotentialId());
                            tempPotnRec.setPotenAmnt(tempPotn.getPotenAmnt());
                            tempPotnRec.setPotActStat(tempPotn.getPotActStat());
                            tempPotnRec.setFinlSupFlag(tempPotn.getFinlSupFlag());
                            tempPotnRec.setFinlCustFlag(tempPotn.getFinlCustFlag());
                            tempPotnRec.setDueDate(rfqMainObj.getDueDate());
                            tempPotnRec.setCreateMode(tempPotn.getCreateMode());
                            tempPotnRec.setCreatedDate(tempPotn.getCreatedDate());
                            tempPotnRec.setConfMatPath(tempPotn.getConfMatPath());
                            tempPotnRec.setEntityId(rfqMainObj.getEntityId());
                            tempPotnRec.setCreatedBy(rfqMainObj.getCreatedUsr());
                            tempPotnRec.setCreatedByEntity(rfqMainObj.getCreatedEntity());
                            tempPotnRec.setCurrency(rfqMainObj.getCurrency());

                            tempPotnRec.findAndSetEntityName();

                            allPotnListFiltered.Add(tempPotnRec);
                        }
                    }
                }

                return allPotnListFiltered;
            }
            /// <summary>
            /// Returns all active potentials for the reponse entity id which are assigned to the given user id and satisfy the filter condition list
            /// </summary>
            /// <param name="respEntityId"></param>
            /// <param name="userId"></param>
            /// <param name="filterParam"></param>
            /// <returns></returns>
            public ArrayList getAllActiveAndAssignedPotentialsFiltered(String respEntityId, String userId, Dictionary<String, String> filterParam)
            {
                if (respEntityId == null || respEntityId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid entity id value sent to method _dispPotentials.getAllActiveAndAssignedPotentialsFiltered"));

                if (userId == null || userId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid userId id value sent to method _dispPotentials.getAllActiveAndAssignedPotentialsFiltered"));


                ArrayList allPotnList = new ArrayList();
                ArrayList allPotnListFiltered = new ArrayList();

                allPotnList = RFQShortlisted.getAllRFQShortListedbyEntityIdAndAssignedToUser(respEntityId, userId);
                int i = 0;

                for (i = 0; i < allPotnList.Count; i++)
                {
                    RFQShortlisted tempPotn = (RFQShortlisted)allPotnList[i];
                    RFQDetails rfqMainObj = RFQDetails.getRFQDetailsbyIdDB(tempPotn.getRFQId());
                    Boolean filterConditionSatisfied = true;

                    //ArrayList potSpecList = RFQProductServiceDetails.getAllProductServiceDetailsbyRFQIdDB(tempPotn.getRFQId());

                    //ArrayList potSpecList = new ArrayList();

                    //foreach (KeyValuePair<String, RFQProductServiceDetails> kvp in potSpecDict)
                    //potSpecList.Add(kvp.Value);
                    if (filterParam.ContainsKey(this.FILTER_BY_RFQ_NO))
                        filterConditionSatisfied = rfqMainObj.getRFQId().IndexOf(filterParam[this.FILTER_BY_RFQ_NO], StringComparison.InvariantCultureIgnoreCase) >= 0 ? filterConditionSatisfied & true : false;

                    if (filterConditionSatisfied)
                    {
                        if (filterParam.ContainsKey(this.FILTER_BY_ACTIVE_STATUS))
                            filterConditionSatisfied = tempPotn.getPotActStat().Equals(filterParam[this.FILTER_BY_ACTIVE_STATUS]) ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_CUST_ID))
                            filterConditionSatisfied = rfqMainObj.getEntityId().Equals(filterParam[this.FILTER_BY_CUST_ID]) ? filterConditionSatisfied & true : false;
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_STAGE))
                            filterConditionSatisfied = tempPotn.getPotStat().Equals(filterParam[this.FILTER_BY_STAGE]) ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_CREATE_DATE_FROM) && filterParam.ContainsKey(this.FILTER_BY_CREATE_DATE_TO))
                        {
                            //Parse the create date as stored in the object                            
                            String createDate = tempPotn.getCreatedDate().Substring(0, tempPotn.getCreatedDate().IndexOf(" "));
                            DateTime createDateVal;
                            DateTime.TryParseExact(createDate, "%M/%d/yyyy", null, DateTimeStyles.AllowWhiteSpaces, out createDateVal);

                            //parse the from create date as passed in the filter parameter
                            DateTime createDateFromVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_CREATE_DATE_FROM], "yyyy-MM-dd", null, DateTimeStyles.None, out createDateFromVal);
                            DateTime createDateToVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_CREATE_DATE_TO], "yyyy-MM-dd", null, DateTimeStyles.None, out createDateToVal);


                            if (DateTime.Compare(createDateVal, createDateFromVal) >= 0 && DateTime.Compare(createDateVal, createDateToVal) <= 0)
                                filterConditionSatisfied = filterConditionSatisfied & true;
                            else
                                filterConditionSatisfied = false;
                        }
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_DUE_DATE_FROM) && filterParam.ContainsKey(this.FILTER_BY_DUE_DATE_TO))
                        {
                            //Parse the create date as stored in the object
                            String dueDate = rfqMainObj.getDueDate().Substring(0, rfqMainObj.getDueDate().IndexOf(" "));
                            DateTime dueDateVal;
                            DateTime.TryParseExact(dueDate, "%M/%d/yyyy", null, DateTimeStyles.AllowWhiteSpaces, out dueDateVal);

                            //parse the from create date as passed in the filter parameter
                            DateTime dueDateFromVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_DUE_DATE_FROM], "yyyy-MM-dd", null, DateTimeStyles.None, out dueDateFromVal);
                            DateTime dueDateToVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_DUE_DATE_TO], "yyyy-MM-dd", null, DateTimeStyles.None, out dueDateToVal);


                            if (DateTime.Compare(dueDateVal, dueDateFromVal) >= 0 && DateTime.Compare(dueDateVal, dueDateToVal) <= 0)
                                filterConditionSatisfied = filterConditionSatisfied & true;
                            else
                                filterConditionSatisfied = false;
                        }
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_PROD_CAT))
                        {
                            int j = 0;
                            ArrayList potSpecList = RFQProdServQnty.getRFQProductServiceQuantityforRFIdDB(tempPotn.getRFQId());
                            filterConditionSatisfied = false;
                            //Even if one of the specificaion object matches the filter criteria, then allow this selection
                            for (j = 0; j < potSpecList.Count; j++)
                            {
                                RFQProdServQnty tempSpec = (RFQProdServQnty)potSpecList[j];
                                if (tempSpec.getProdCatId().Equals(filterParam[this.FILTER_BY_PROD_CAT]) || BackEndObjects.ProductCategory.
                                    getRootLevelParentCategoryDB(tempSpec.getProdCatId()).ContainsKey(filterParam[this.FILTER_BY_PROD_CAT]))
                                { filterConditionSatisfied = true; break; }
                            }

                        }
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_ASSGND_TO))
                        {
                            String assgnTo = RFQResponse.getRFQResponseWOQuotesForRFQIdandResponseEntityIdDB(tempPotn.getRFQId(), respEntityId).getAssignedTo();
                            filterConditionSatisfied = (assgnTo != null && assgnTo.Equals(filterParam[FILTER_BY_ASSGND_TO])) ? filterConditionSatisfied & true : false;
                        }
                        if (filterConditionSatisfied)
                        {
                            PotentialRecords tempPotnRec = new PotentialRecords();

                            tempPotnRec.setRFQId(tempPotn.getRFQId());
                            tempPotnRec.setRFQName(rfqMainObj.getRFQName());
                            tempPotnRec.setRespEntityId(tempPotn.getRespEntityId());
                            tempPotnRec.setPotStat(tempPotn.getPotStat());
                            tempPotnRec.setPotentialId(tempPotn.getPotentialId());
                            tempPotnRec.setPotenAmnt(tempPotn.getPotenAmnt());
                            tempPotnRec.setPotActStat(tempPotn.getPotActStat());
                            tempPotnRec.setFinlSupFlag(tempPotn.getFinlSupFlag());
                            tempPotnRec.setFinlCustFlag(tempPotn.getFinlCustFlag());
                            tempPotnRec.setDueDate(rfqMainObj.getDueDate());
                            tempPotnRec.setCreateMode(tempPotn.getCreateMode());
                            tempPotnRec.setCreatedDate(tempPotn.getCreatedDate());
                            tempPotnRec.setConfMatPath(tempPotn.getConfMatPath());
                            tempPotnRec.setEntityId(rfqMainObj.getEntityId());
                            tempPotnRec.setCreatedBy(rfqMainObj.getCreatedUsr());
                            tempPotnRec.setCreatedByEntity(rfqMainObj.getCreatedEntity());

                            tempPotnRec.findAndSetEntityName();

                            allPotnListFiltered.Add(tempPotnRec);
                        }
                    }
                }

                return allPotnListFiltered;
            }
            /// <summary>
            /// This method is a replicate of the method _dispRFQDetails.viewRFQResponseQuoteDetail.
            ///  For a particular RFQ, this method returns the respective response quote details given a particular response entity id.
            ///  This method does the work of both the other methods 'getPotentialQuoteDetails' and 'getPotentialSpecificationDetails' together and returns the combined entity
            /// </summary>
            /// <param name="cmpUsrId"></param>
            /// <param name="userId"></param>
            /// <param name="responseEntId"></param>
            /// <param name="rfId"></param>
            /// <returns></returns>
            public RFQResponseQuoteTotal getPotentialQuoteandSpecDetails(String cmpUsrId, String userId, String responseEntId, String rfId)
            {
                //The first two parameters are not used as of now, but they are still expected to contain future enchancements
                if (cmpUsrId == null || cmpUsrId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid entity id value sent to method _dispPotentials.getPotentialQuoteandSpecDetails"));

                if (userId == null || userId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid userId id value sent to method _dispPotentials.getPotentialQuoteandSpecDetails"));

                if (rfId == null || rfId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid rfqId id value sent to method _dispPotentials.getPotentialQuoteandSpecDetails"));

                if (responseEntId == null || responseEntId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid response entity id value sent to method _dispPotentials.getPotentialQuoteandSpecDetails"));

                RFQResponseQuoteTotal completeQuoteResp = new RFQResponseQuoteTotal();
                ArrayList tempList = new ArrayList();

                Dictionary<String, RFQResponseQuotes> allRespQut = RFQResponseQuotes.getAllResponseQuotesforRFQandResponseEntityDB(rfId, responseEntId);

                foreach (KeyValuePair<String, RFQResponseQuotes> kvp in allRespQut)
                {
                    RFQResponseQuotes tempRec = kvp.Value;

                    RFQResponseQuoteRecord tempQuoteRec = new RFQResponseQuoteRecord();
                    tempQuoteRec.setUnitName(tempRec.getUnitName());
                    tempQuoteRec.setRFQId(tempRec.getRFQId());
                    tempQuoteRec.setResponseUsrId(tempRec.getResponseUsrId());
                    tempQuoteRec.setResponseEntityId(tempRec.getResponseEntityId());
                    tempQuoteRec.setQuote(tempRec.getQuote());
                    tempQuoteRec.setPrdCatId(tempRec.getPrdCatId());

                    tempQuoteRec.setAmntQuntyandProdSpec();
                    tempList.Add(tempQuoteRec);

                }
                completeQuoteResp.setRFQResponseQuoteRecordList(tempList);
                completeQuoteResp.calculateTotalAmnt();

                return completeQuoteResp;
            }
            /// <summary>
            /// For a given response entity id, user id and the rfq id this method will return the respective NDA document path.
            /// </summary>
            /// <param name="responseEntId"></param>
            /// <param name="userId"></param>
            /// <param name="rfId"></param>
            /// <returns></returns>
            public String getPotentialNDA(String responseEntId, String userId, String rfId)
            {
                 if (responseEntId == null || responseEntId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid entity id value sent to method _dispPotentials.getPotentialNDA"));

                if (userId == null || userId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid userId id value sent to method _dispPotentials.getPotentialNDA"));

                if (rfId == null || rfId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid potential id value sent to method _dispPotentials.getPotentialNDA"));

                return RFQResponse.getRFQResponseforRFQIdandResponseEntityIdDB(rfId, responseEntId).getNdaPath();
            }
        }
        /// <summary>
        /// This class is only for updating the  Potential details of manually created potentials.
        /// </summary>
        public class _updatePotentials
        {
            /// <summary>
            /// This method is for updating a Potential details if the potential was created manually.
            /// If this is a delete operation, records from RFQDetails,RFQShortlisted and other prod/service details will be deleted.
            /// To update related product/service details for a manually created potential, use the method 'updatePotentialProdServDetails'.
            /// </summary>
            /// <returns></returns>
            public int updatePotential(PotentialRecords rfS, String cmpUsrId, String userId,String operation)
            {
                if (cmpUsrId == null || cmpUsrId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid entity id value sent to method _updatePotentials.updatePotential"));

                if (userId == null || userId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid userId id value sent to method _updatePotentials.updatePotential"));

                if(rfS==null || rfS.getRFQId()==null||rfS.getRFQId().Equals("")||rfS.getRespEntityId()==null ||rfS.getRespEntityId().Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid Potential details value sent to method _updatePotentials.updatePotential"));

                if (!rfS.getCreateMode().Equals(RFQShortlisted.POTENTIAL_CREATION_MODE_MANUAL))
                    throw (new CustomExceptions.businessRuleViolationException("Only Manually Created Potential Entries Can be Modified - _updatePotentials.updatePotential"));

                if (!operation.Equals(DBConn.Connections.OPERATION_UPDATE) && !operation.Equals(DBConn.Connections.OPERATION_DELETE))
                    throw (new CustomExceptions.invalidParamException("Invalid operation Name passed to method _updatePotentials.updatePotential"));

                //RFQDetails rfqObj = RFQDetails.getRFQDetailsbyIdDB(rfS.getRFQId());

                                Dictionary<String, String> updateTargetVals = new Dictionary<string, string>();
                Dictionary<String, String> updateWhereVals = new Dictionary<string, string>();
                
                int totalRecAffected = 0;

                if (operation.Equals(DBConn.Connections.OPERATION_UPDATE))
                {
                      if (rfS.getConfMatPath()!=null)
                        updateTargetVals.Add(RFQShortlisted.RFQ_SHORTLIST_COL_CONFRM_MATERIAL, rfS.getConfMatPath().ToString());
                    if (rfS.getFinlCustFlag() != null)
                        updateTargetVals.Add(RFQShortlisted.RFQ_SHORTLIST_COL_FINL_BY_CUST, rfS.getFinlCustFlag());
                    if (rfS.getFinlSupFlag() != null)
                        updateTargetVals.Add(RFQShortlisted.RFQ_SHORTLIST_COL_FINL_BY_SUPL, rfS.getFinlSupFlag());
                    if (rfS.getPotActStat() != null)
                        updateTargetVals.Add(RFQShortlisted.RFQ_SHORTLIST_COL_POTENTIAL_ACTIVE_STAT, rfS.getPotActStat());
                    if (rfS.getPotenAmnt() != null)
                        updateTargetVals.Add(RFQShortlisted.RFQ_SHORTLIST_COL_POTENTIAL_AMNT, rfS.getPotenAmnt().ToString());
                    if (rfS.getPotStat() != null)
                        updateTargetVals.Add(RFQShortlisted.RFQ_SHORTLIST_COL_POTENTIAL_ACTIVE_STAT, rfS.getPotStat());

                    updateWhereVals.Add(RFQShortlisted.RFQ_SHORTLIST_COL_RFQ_ID, rfS.getRFQId());
                    updateWhereVals.Add(RFQShortlisted.RFQ_SHORTLIST_COL_RESP_ENT_ID, rfS.getRespEntityId());

                    totalRecAffected = RFQShortlisted.updateRFQShortListedEntryDB(updateTargetVals, updateWhereVals, operation);

                   updateTargetVals = new Dictionary<string, string>();
                updateWhereVals = new Dictionary<string, string>();
                    
                    if(rfS.getDueDate()!=null)
                        updateTargetVals.Add(RFQDetails.RFQ_COL_DUE_DATE, rfS.getDueDate());
                    if (rfS.getEntityId() != null)
                        updateTargetVals.Add(RFQDetails.RFQ_COL_ENTITY_ID, rfS.getEntityId());

                    updateWhereVals.Add(RFQDetails.RFQ_COL_RFQ_ID, rfS.getRFQId());

                    totalRecAffected += RFQDetails.updateRFQDetailsDB(updateTargetVals, updateWhereVals, operation);
                }
                
                else if (operation.Equals(DBConn.Connections.OPERATION_DELETE))
                {
                    //In case of a delete .. delete the main record from the RFQDetails table which will have a cascading effect on RFQShortlisted table and other related
                    //tables automatically
                    updateWhereVals.Add(RFQDetails.RFQ_COL_RFQ_ID, rfS.getRFQId());
                    totalRecAffected = RFQDetails.updateRFQDetailsDB(updateTargetVals, updateWhereVals, operation);
                }

                return totalRecAffected;
            }
            /// <summary>
            /// The fourth parameter is an ArrayList of 'RFQProductServiceDetails' associated with a particular potential record.
            /// In case a delete was performed a check if there is no record available in the table 'RFQ_Product_Service_Map' after the deletion for the given
            ///RFQid and prodcatid.
            ///If Yes, this method will delete all entries from the dependent table (this can not be done through foreign key rule) 'RFQ_Response_Quote_Details' for the respective 
            ///RFQid and prodcatid
            /// </summary>
            /// <param name="cmpUsrId"></param>
            /// <param name="userId"></param>
            /// <param name="prodServList"></param>
            /// <param name="operation"></param>
            /// <returns></returns>
            public int updatePotentialProdServDetails(PotentialRecords potRec, String cmpUsrId, String userId, ArrayList prodServList,String operation)
            {
                if (cmpUsrId == null || cmpUsrId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid entity id value sent to method _updatePotentials.updatePotentialProdServDetails"));

                if (userId == null || userId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid userId id value sent to method _updatePotentials.updatePotentialProdServDetails"));

                if (!potRec.getCreateMode().Equals(RFQShortlisted.POTENTIAL_CREATION_MODE_MANUAL))
                    throw (new CustomExceptions.businessRuleViolationException("Only Manually Created Potential Entries Can be Modified - _updatePotentials.updatePotentialProdServDetails"));

                 //if(operation.Equals(DBConn.Connections.OPERATION_DELETE))
                     //throw (new CustomExceptions.invalidParamException("For delete operation use 'updatePotential' method which will delete all dependent table records _updatePotentials.updatePotentialProdServDetails"));

                int totalRecAffected = 0;
                for (int count = 0; count < prodServList.Count; count++)
                {
                    RFQProductServiceDetails RFQProdSpecObj =(RFQProductServiceDetails) prodServList[count];

                    if (RFQProdSpecObj.getPrdCatId() == null || RFQProdSpecObj.getPrdCatId().Equals("") || RFQProdSpecObj.getFeatId() == null || RFQProdSpecObj.getFeatId().Equals(""))
                        throw (new CustomExceptions.invalidParamException("Product Category Id and Feature id necessary to update Requirement specification - method   _updatePotentials.updatePotentialProdServDetails()"));

                    Dictionary<String,String> updateTargetVals = new Dictionary<string, string>();
                    Dictionary<String,String>  updateWhereVals = new Dictionary<string, string>();

                    if (RFQProdSpecObj.getFromSpecId() != null)
                        updateTargetVals.Add(RFQProductServiceDetails.RFQ_PROD_COL_FROM_SPEC_ID, RFQProdSpecObj.getFromSpecId());
                    if (RFQProdSpecObj.getToSpecId() != null)
                        updateTargetVals.Add(RFQProductServiceDetails.RFQ_PROD_COL_TO_SPEC_ID, RFQProdSpecObj.getToSpecId());
                    if (RFQProdSpecObj.getMsrmntUnit() != null)
                        updateTargetVals.Add(RFQProductServiceDetails.RFQ_PROD_COL_MSRMNT_UNIT, RFQProdSpecObj.getMsrmntUnit());
                    if (RFQProdSpecObj.getImgPath() != null)
                        updateTargetVals.Add(RFQProductServiceDetails.RFQ_PROD_COL_IMAGE_PATH, RFQProdSpecObj.getImgPath().ToString());
                    if (RFQProdSpecObj.getSpecText() != null)
                        updateTargetVals.Add(RFQProductServiceDetails.RFQ_PROD_COL_SPEC_TEXT, RFQProdSpecObj.getSpecText());
                    if (RFQProdSpecObj.getQuantity() != 0)
                        updateTargetVals.Add(RFQProductServiceDetails.RFQ_PROD_COL_QUANTITY, RFQProdSpecObj.getQuantity().ToString());

                    updateWhereVals.Add(RFQProductServiceDetails.RFQ_PROD_COL_RFQ_ID, RFQProdSpecObj.getRFQId());
                    updateWhereVals.Add(RFQProductServiceDetails.RFQ_PROD_COL_PROD_ID, RFQProdSpecObj.getPrdCatId());
                    updateWhereVals.Add(RFQProductServiceDetails.RFQ_PROD_COL_FEAT_ID, RFQProdSpecObj.getFeatId());
                                        
                    totalRecAffected += RFQProductServiceDetails.updateRFQProductServiceDetails(updateTargetVals, updateWhereVals, operation);

                    if (operation.Equals(DBConn.Connections.OPERATION_DELETE))
                    {
                        //In case a delete was performed check if there is no record available in the table 'RFQ_Product_Service_Map' after the deletion for the given
                        //RFQid and prodcatid.
                        //If Yes, delete all entries from the dependent table (this can not be done through foreign key rule) 'RFQ_Response_Quote_Details' for the respective 
                        //RFQid and prodcatid
                        Dictionary<String, RFQProductServiceDetails> checkAvail =
                        RFQProductServiceDetails.getAllProductServiceDetailsbyRFQandProductIdDB(RFQProdSpecObj.getRFQId(), RFQProdSpecObj.getPrdCatId());
                        if (checkAvail == null || checkAvail.Count == 0)
                        {
                            updateTargetVals = new Dictionary<string, string>();
                            updateWhereVals = new Dictionary<string, string>();

                            updateWhereVals.Add(RFQResponseQuotes.RFQ_RES_QUOTE_COL_RFQ_ID, RFQProdSpecObj.getRFQId());
                            updateWhereVals.Add(RFQResponseQuotes.RFQ_RES_QUOTE_COL_PROD_CAT, RFQProdSpecObj.getPrdCatId());

                            //Removing all response entries for that particualr RFQid and product cateogry id
                            RFQResponseQuotes.updateRFQResponseQuotesDB(updateTargetVals, updateWhereVals, DBConn.Connections.OPERATION_DELETE);
                        }
                    }
                }

                return totalRecAffected;
            }
            /// <summary>
            /// This method can be used after the vendor clicks the 'Finalize Deal' button from sales screen for a Potential entry.
            /// The first parameter must contain the RFQ id and the response entity id.
            /// If the potential entry was created manually thi method will mark some other necessary flags as complete as well.
            /// </summary>
            /// <param name="rfS"></param>
            /// <param name="cmpUsrId"></param>
            /// <param name="userId"></param>
            /// <returns></returns>
            public int finalizeDeal(RFQShortlisted rfS, String cmpUsrId, String userId)
            {
                if (cmpUsrId == null || cmpUsrId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid entity id value sent to method _updateRFQDetails.finalizeDeal"));

                if (userId == null || userId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid userId id value sent to method _updateRFQDetails.finalizeDeal"));

                if (rfS == null || rfS.getRFQId() == null || rfS.getRFQId().Equals("") || rfS.getRespEntityId() == null || rfS.getRespEntityId().Equals("") || rfS.getPotentialId() == null || rfS.getPotentialId().Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid Potential details value sent to method _updateRFQDetails.finalizeDeal"));

                Dictionary<String, String> updateTargetVals = new Dictionary<string, string>();
                Dictionary<String, String> updateWhereVals = new Dictionary<string, string>();

                int totalRecAffected = 0;
                //if (rfS.getFinlCustFlag() != null)
                updateTargetVals.Add(RFQShortlisted.RFQ_SHORTLIST_COL_FINL_BY_SUPL, "Y");
                if (rfS.getCreateMode().Equals(RFQShortlisted.POTENTIAL_CREATION_MODE_MANUAL))
                    updateTargetVals.Add(RFQShortlisted.RFQ_SHORTLIST_COL_FINL_BY_CUST, "Y");
                
                if (RFQShortlisted.getAllRFQShortListedbyPotentialIdDB(rfS.getPotentialId()).getFinlCustFlag().Equals("Y"))
                    updateTargetVals.Add(RFQShortlisted.RFQ_SHORTLIST_COL_POTENTIAL_ACTIVE_STAT, RFQShortlisted.POTENTIAL_ACTIVE_STAT_CLOSED_WON);

                updateWhereVals.Add(RFQShortlisted.RFQ_SHORTLIST_COL_RFQ_ID, rfS.getRFQId());
                updateWhereVals.Add(RFQShortlisted.RFQ_SHORTLIST_COL_RESP_ENT_ID, rfS.getRespEntityId());

                totalRecAffected = RFQShortlisted.updateRFQShortListedEntryDB(updateTargetVals, updateWhereVals, DBConn.Connections.OPERATION_UPDATE);

                return totalRecAffected;
            }
        }
        /// <summary>
        /// This class contains methods to creat/update reponse for a particular Potential entry.
        ///  This updated response will be viewed by the customer in his RFQ response section.
        /// </summary>
        public class _respondToPotentials
        {
            /// <summary>
            /// This method can be used to both create /update the response quote details for a given RFQ.
            /// The first parameter is RFQResponse object (if this object is found already existing in database then this will be considered as an Update request).
            /// The second parameter is an ArrayList of 'RFQResponseQuotes' objects (for update, send only those response quote records which needs to be updated - 
            /// ALSO , for an update the prod category value must be sent for each of item in this list).
            /// The third parameter is useful only for update request.. this parameter says whether or not to update the NDA/documents passed through the first parameter.
            /// For an insert request it is advisable NOT TO provide any rfqid in the 'RFQResponse' object as this will be ignored and system will create new id.
            /// </summary>
            /// <param name="rfS"></param>
            /// <param name="rfQuotes"></param>
            /// <param name="updateNDA"></param>
            /// <returns></returns>
            public int updateOrCreatePotentialQuoteDetails(RFQResponse rfS, ArrayList rfQuotes, Boolean updateNDA)
            {
                if (rfS == null || rfS.getRFQId() == null || rfS.getRFQId().Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid 'RFQResponse' object value sent to method _respondToPotentials.updateOrCreatePotentialQuoteDetails"));

                if (rfQuotes == null || rfQuotes.Count == 0)
                    throw (new CustomExceptions.invalidParamException("Invalid Quote List  sent to method _respondToPotentials.updateOrCreatePotentialQuoteDetails"));

                int rowsAffected = 0;

                Boolean insertRequest = (rfS.getRFQId() == null ? true : (rfS.getRFQId().Equals("") == true ? true : false));
                if (!insertRequest)
                {
                    RFQResponse rfsObj = RFQResponse.getRFQResponseforRFQIdandResponseEntityIdDB(rfS.getRFQId(), rfS.getRespEntityId());
                    if (rfsObj == null || !rfsObj.getRFQId().Equals(rfS.getRFQId()))
                        insertRequest = true;
                }

                if (insertRequest)
                {

                    Id idGenerator = new Id();
                    rfS.setRFQId(idGenerator.getNewId(Id.ID_TYPE_RFQ_STRING));
                    rowsAffected += RFQResponse.insertRFQResponseDB(rfS);

                    for (int count = 0; count < rfQuotes.Count; count++)
                    {
                        RFQResponseQuotes respQuoteTemp = (RFQResponseQuotes)rfQuotes[count];
                        respQuoteTemp.setRFQId(rfS.getRFQId());
                        rowsAffected += RFQResponseQuotes.insertRFQResponseQuotesDB(respQuoteTemp);
                    }

                }
                else
                {
                    if (updateNDA)
                        rowsAffected += RFQResponse.updateorInsertRFQResponseNDADB(rfS);

                    for (int count = 0; count < rfQuotes.Count; count++)
                    {
                        RFQResponseQuotes respQuoteTemp = (RFQResponseQuotes)rfQuotes[count];
                        if (respQuoteTemp.getPrdCatId() == null || respQuoteTemp.getPrdCatId().Equals(""))
                            throw (new CustomExceptions.invalidParamException("Invalid prod category id found in Quote - _respondToPotentials.updateOrCreatePotentialQuoteDetails"));

                        Dictionary<String, String> targetVal = new Dictionary<string, string>();
                        Dictionary<String, String> whereCls = new Dictionary<string, string>();

                        whereCls.Add(RFQResponseQuotes.RFQ_RES_QUOTE_COL_RFQ_ID, rfS.getRFQId());
                        whereCls.Add(RFQResponseQuotes.RFQ_RES_QUOTE_COL_RESP_COMP_ID, rfS.getRespEntityId());
                        whereCls.Add(RFQResponseQuotes.RFQ_RES_QUOTE_COL_PROD_CAT, respQuoteTemp.getPrdCatId());
                        if (respQuoteTemp.getQuote() != null)
                            targetVal.Add(RFQResponseQuotes.RFQ_RES_QUOTE_COL_QUOTE, respQuoteTemp.getQuote());
                        if (respQuoteTemp.getUnitName() != null)
                            targetVal.Add(RFQResponseQuotes.RFQ_RES_QUOTE_COL_UNIT_NAME, respQuoteTemp.getUnitName());
                        if (respQuoteTemp.getResponseUsrId() != null)
                            targetVal.Add(RFQResponseQuotes.RFQ_RES_QUOTE_COL_RESP_COMP_USR, respQuoteTemp.getResponseUsrId());

                        rowsAffected += RFQResponseQuotes.updateRFQResponseQuotesDB(targetVal, whereCls, Connections.OPERATION_UPDATE);
                    }
                }
                return rowsAffected;
            }
        }
        /// <summary>
        /// This class contain methods which lets the user to update/create quote details in response to an RFQ.
        /// These methods can be used from Lead/Potential sections to update quote details.
        /// </summary>
        public class _quoteDetails
        {
            /// <summary>
            /// This method inserts the RFQ response into backend database.
            /// The first parameter should be an object of type 'RFQResponse' class.
            /// The second parameter is an ArrayList of 'RFQResponseQuotes' class. Each object in that arraylist is response quote to one of the product/service category of the 
            /// RFQ.
            /// </summary>
            /// <param name="rfR"></param>
            /// <param name="RFQResponseQuoteList"></param>
            /// <returns></returns>
            public int insertRFQResponseQuote(RFQResponse rfR, ArrayList RFQResponseQuoteList)
            {
                if (rfR == null || rfR.getRFQId() == null || rfR.getRespEntityId() == null || rfR.getRFQId().Equals("") || rfR.getRespEntityId().Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid 'RFQResponse' object value sent to method _quoteDetails.insertRFQResponseQuote"));
                if (RFQResponseQuoteList == null || RFQResponseQuoteList.Count == 0)
                    throw (new CustomExceptions.invalidParamException("Invalid list of RFQ response objects sent to method _quoteDetails.insertRFQResponseQuote"));
                if (!RFQDetails.getRFQDetailsbyIdDB(rfR.getRFQId()).getActiveStat().Equals(RFQActiveStat.RFQ_ACTIVE_STAT_ACTIVE)) //RFQ not open for new quotes
                    throw (new CustomExceptions.businessRuleViolationException("RFQ not active to receive new quotes  - exception from method _quoteDetails.insertRFQResponseQuote"));

                int recordsInserted = 0;

                recordsInserted = RFQResponse.insertRFQResponseDB(rfR);

                for (int count = 0; count < RFQResponseQuoteList.Count; count++)
                {
                    RFQResponseQuotes tempQuote = (RFQResponseQuotes)RFQResponseQuoteList[count];
                    recordsInserted += RFQResponseQuotes.insertRFQResponseQuotesDB(tempQuote);
                }

                return recordsInserted;
            }


        }
        /// <summary>
        /// This class contains method to display the lead records in the sales screen.
        /// </summary>
        public class _dispLeads
        {
            public String FILTER_BY_PROD_CAT = "FILTER_BY_PROD_CAT";
            public String FILTER_BY_ACTIVE_STATUS = "FILTER_BY_ACTIVE_STATUS";
            public String FILTER_BY_SUBMIT_DATE_FROM = "FILTER_BY_SUBMIT_DATE_FROM";
            public String FILTER_BY_SUBMIT_DATE_TO = "FILTER_BY_SUBMIT_DATE_TO";
            public String FILTER_BY_DUE_DATE_FROM = "FILTER_BY_DUR_DATE_FROM";
            public String FILTER_BY_DUE_DATE_TO = "FILTER_BY_DUE_DATE_TO";
            public String FILTER_BY_CUST_ID = "FILTER_BY_CUST_ID";
            public String FILTER_BY_RFQ_NO = "FILTER_BY_RFQ_NO";
            public String FILTER_BY_ASSGND_TO = "FILTER_BY_ASSGND_TO";

            /// <summary>
            /// For a given entity id and a user id, this method returns all Lead objects with associated product specifications.
            /// Each element of the returned arraylist is an object of type 'LeadRecord' (which is a subclass of 'RFQDetails') with the property 'RFQProdServList' set in it.
            /// Note that the returned list does not contain LeadResponse object. If required the caller method needs to retrieve this data.
            /// </summary>
            /// <param name="cmpUsrId"></param>
            /// <param name="userId"></param>
            /// <returns></returns>
            public ArrayList getAllLeadDetails(String cmpUsrId, String userId)
            {
                if (cmpUsrId == null || cmpUsrId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid entity id value sent to method _dispRFQDetails.getAllRFQDetails"));

                if (userId == null || userId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid userId id value sent to method _dispRFQDetails.getAllRFQDetails"));

                ArrayList allRFQList = new ArrayList();
                ArrayList allLeadList = new ArrayList();
                //ArrayList allRFQListUpdated = new ArrayList();

                allRFQList = RFQDetails.getAllLeadsforEntityIdDB(cmpUsrId);

                for (int count = 0; count < allRFQList.Count; count++)
                {
                    RFQDetails temp = (RFQDetails)allRFQList[count];

                    LeadRecord tempLeadRec = new LeadRecord();
                    tempLeadRec.setTermsandConds(temp.getTermsandConds());
                    tempLeadRec.setSubmitDate(temp.getSubmitDate());
                    tempLeadRec.setRFQId(temp.getRFQId());
                    tempLeadRec.setReqId(temp.getReqId());
                    tempLeadRec.setRFQName(temp.getRFQName());
                    tempLeadRec.setNDADocPath(temp.getNDADocPath());
                    tempLeadRec.setLocalityId(temp.getLocalityId());
                    tempLeadRec.setEntityId(temp.getEntityId());
                    tempLeadRec.setDueDate(temp.getDueDate());
                    tempLeadRec.setCreateMode(temp.getCreateMode());
                    tempLeadRec.setCreatedUsr(temp.getCreatedUsr());
                    tempLeadRec.setCreatedEntity(temp.getCreatedEntity());
                    tempLeadRec.setApprovalStat(temp.getApprovalStat());
                    tempLeadRec.setActiveStat(temp.getActiveStat());

                    ArrayList rfqSpecList = RFQProductServiceDetails.getAllProductServiceDetailsbyRFQIdDB(temp.getRFQId());
                    //ArrayList rfqSpecList = new ArrayList();

                    //foreach (KeyValuePair<String, RFQProductServiceDetails> kvp in rfqSpecDict)
                        //rfqSpecList.Add(kvp.Value);

                    tempLeadRec.setRFQProdServList(rfqSpecList);
                    tempLeadRec.setRFQProdServQntyList(RFQProdServQnty.getRFQProductServiceQuantityforRFIdDB(temp.getRFQId()));
                    tempLeadRec.findAndSetEntityName();

                    allLeadList.Add(tempLeadRec);
                }


                return allLeadList;
            }
            /// <summary>
            /// This is a faster version of 'getAllLeadDetails' but it does not return the associated product specs and quantity details
            /// </summary>
            /// <param name="cmpUsrId"></param>
            /// <param name="userId"></param>
            /// <returns></returns>
            public ArrayList getAllLeadDetailsWOProdQntyAndSpec(String cmpUsrId, String userId)
            {
                if (cmpUsrId == null || cmpUsrId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid entity id value sent to method _dispRFQDetails.getAllRFQDetails"));

                if (userId == null || userId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid userId id value sent to method _dispRFQDetails.getAllRFQDetails"));

                ArrayList allRFQList = new ArrayList();
                ArrayList allLeadList = new ArrayList();
                //ArrayList allRFQListUpdated = new ArrayList();

                allRFQList = RFQDetails.getAllLeadsforEntityIdDB(cmpUsrId);

                for (int count = 0; count < allRFQList.Count; count++)
                {
                    RFQDetails temp = (RFQDetails)allRFQList[count];

                    LeadRecord tempLeadRec = new LeadRecord();
                    tempLeadRec.setTermsandConds(temp.getTermsandConds());
                    tempLeadRec.setSubmitDate(temp.getSubmitDate());
                    tempLeadRec.setRFQId(temp.getRFQId());
                    tempLeadRec.setReqId(temp.getReqId());
                    tempLeadRec.setRFQName(temp.getRFQName());
                    tempLeadRec.setNDADocPath(temp.getNDADocPath());
                    tempLeadRec.setLocalityId(temp.getLocalityId());
                    tempLeadRec.setEntityId(temp.getEntityId());
                    tempLeadRec.setDueDate(temp.getDueDate());
                    tempLeadRec.setCreateMode(temp.getCreateMode());
                    tempLeadRec.setCreatedUsr(temp.getCreatedUsr());
                    tempLeadRec.setCreatedEntity(temp.getCreatedEntity());
                    tempLeadRec.setApprovalStat(temp.getApprovalStat());
                    tempLeadRec.setActiveStat(temp.getActiveStat());
                    tempLeadRec.setCurrency(temp.getCurrency());
                    //ArrayList rfqSpecList = RFQProductServiceDetails.getAllProductServiceDetailsbyRFQIdDB(temp.getRFQId());
                    //ArrayList rfqSpecList = new ArrayList();

                    //foreach (KeyValuePair<String, RFQProductServiceDetails> kvp in rfqSpecDict)
                    //rfqSpecList.Add(kvp.Value);

                    tempLeadRec.setRFQProdServList(null);
                    tempLeadRec.setRFQProdServQntyList(null);
                    tempLeadRec.findAndSetEntityName();

                    allLeadList.Add(tempLeadRec);
                }


                return allLeadList;
            }
            /// <summary>
            /// All Leads which are active and assigned to the passed user id
            /// </summary>
            /// <param name="cmpUsrId"></param>
            /// <param name="userId"></param>
            /// <returns></returns>
            public ArrayList getAllActiveAndAssignedToLeadDetailsWOProdQntyAndSpec(String cmpUsrId, String userId)
            {
                if (cmpUsrId == null || cmpUsrId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid entity id value sent to method _dispRFQDetails.getAllActiveAndAssignedToLeadDetailsWOProdQntyAndSpec"));

                if (userId == null || userId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid userId id value sent to method _dispRFQDetails.getAllActiveAndAssignedToLeadDetailsWOProdQntyAndSpec"));

                ArrayList allRFQList = new ArrayList();
                ArrayList allLeadList = new ArrayList();
                //ArrayList allRFQListUpdated = new ArrayList();

                allRFQList = RFQDetails.getAllActiveAndAssignedToUserLeadsforEntityIdDB(cmpUsrId,userId);

                for (int count = 0; count < allRFQList.Count; count++)
                {
                    RFQDetails temp = (RFQDetails)allRFQList[count];

                    LeadRecord tempLeadRec = new LeadRecord();
                    tempLeadRec.setTermsandConds(temp.getTermsandConds());
                    tempLeadRec.setSubmitDate(temp.getSubmitDate());
                    tempLeadRec.setRFQId(temp.getRFQId());
                    tempLeadRec.setReqId(temp.getReqId());
                    tempLeadRec.setRFQName(temp.getRFQName());
                    tempLeadRec.setNDADocPath(temp.getNDADocPath());
                    tempLeadRec.setLocalityId(temp.getLocalityId());
                    tempLeadRec.setEntityId(temp.getEntityId());
                    tempLeadRec.setDueDate(temp.getDueDate());
                    tempLeadRec.setCreateMode(temp.getCreateMode());
                    tempLeadRec.setCreatedUsr(temp.getCreatedUsr());
                    tempLeadRec.setCreatedEntity(temp.getCreatedEntity());
                    tempLeadRec.setApprovalStat(temp.getApprovalStat());
                    tempLeadRec.setActiveStat(temp.getActiveStat());
                    
                    //ArrayList rfqSpecList = RFQProductServiceDetails.getAllProductServiceDetailsbyRFQIdDB(temp.getRFQId());
                    //ArrayList rfqSpecList = new ArrayList();

                    //foreach (KeyValuePair<String, RFQProductServiceDetails> kvp in rfqSpecDict)
                    //rfqSpecList.Add(kvp.Value);

                    tempLeadRec.setRFQProdServList(null);
                    tempLeadRec.setRFQProdServQntyList(null);
                    tempLeadRec.findAndSetEntityName();

                    allLeadList.Add(tempLeadRec);
                }


                return allLeadList;
            }
            /// <summary>
            /// This method functions the same way as 'getAllLeadDetails' except that this method can send the List filtered.
            /// Each element of the returned arraylist is an object of type 'LeadRecord' (which is a subclass of 'RFQDetails') with the property 'RFQProdServList' set in it.
            /// </summary>
            /// <param name="cmpUsrId"></param>
            /// <param name="userId"></param>
            /// <param name="filterParam"></param>
            /// <returns></returns>
            public ArrayList getAllLeadDetailsFiltered(String cmpUsrId, String userId, Dictionary<String, String> filterParam)
            {
                ArrayList allRFQList = new ArrayList();
                ArrayList allRFQFiltered = new ArrayList();

                allRFQList = RFQDetails.getAllLeadsforEntityIdDB(cmpUsrId);
                int i = 0;

                for (i = 0; i < allRFQList.Count; i++)
                {
                    RFQDetails tempRFQ = (RFQDetails)allRFQList[i];
                    

                    Boolean filterConditionSatisfied = true;

                    //ArrayList rfqSpecList = RFQProductServiceDetails.getAllProductServiceDetailsbyRFQIdDB(tempRFQ.getRFQId());
                    
                    //ArrayList rfqSpecList = new ArrayList();

                    //foreach (KeyValuePair<String, RFQProductServiceDetails> kvp in rfqSpecDict)
                        //rfqSpecList.Add(kvp.Value);
                    if (filterParam.ContainsKey(this.FILTER_BY_RFQ_NO))
                        filterConditionSatisfied = tempRFQ.getRFQId().IndexOf(filterParam[this.FILTER_BY_RFQ_NO], StringComparison.InvariantCultureIgnoreCase) >= 0 ? true : false;
                    
                    if (filterConditionSatisfied)
                    {
                        if (filterParam.ContainsKey(this.FILTER_BY_ACTIVE_STATUS))
                            filterConditionSatisfied = tempRFQ.getActiveStat().Equals(filterParam[this.FILTER_BY_ACTIVE_STATUS]) ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterConditionSatisfied&& filterParam.ContainsKey(this.FILTER_BY_CUST_ID))
                            filterConditionSatisfied = tempRFQ.getEntityId().Equals(filterParam[this.FILTER_BY_CUST_ID]) ? filterConditionSatisfied & true : false;
                        if (filterConditionSatisfied&& filterParam.ContainsKey(this.FILTER_BY_DUE_DATE_FROM) && filterParam.ContainsKey(this.FILTER_BY_DUE_DATE_TO))
                        {
                            //Parse the due date as stored in the object
                            String dueDate = tempRFQ.getDueDate().Substring(0, tempRFQ.getDueDate().IndexOf(" "));
                            DateTime dueDateVal;
                            DateTime.TryParseExact(dueDate, "%M/%d/yyyy", null, DateTimeStyles.AllowWhiteSpaces, out dueDateVal);

                            //parse the from due date as passed in the filter parameter
                            DateTime dueDateFromVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_DUE_DATE_FROM], "yyyy-MM-dd", null, DateTimeStyles.None, out dueDateFromVal);
                            DateTime dueDateToVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_DUE_DATE_TO], "yyyy-MM-dd", null, DateTimeStyles.None, out dueDateToVal);


                            if (DateTime.Compare(dueDateVal, dueDateFromVal) >= 0 && DateTime.Compare(dueDateVal, dueDateToVal) <= 0)
                                filterConditionSatisfied = filterConditionSatisfied & true;
                            else
                                filterConditionSatisfied = false;
                        }
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_SUBMIT_DATE_FROM) && filterParam.ContainsKey(this.FILTER_BY_SUBMIT_DATE_TO))
                        {
                            //Parse the due date as stored in the object
                            String subDate = tempRFQ.getSubmitDate().Substring(0, tempRFQ.getSubmitDate().IndexOf(" "));
                            DateTime subDateVal;
                            DateTime.TryParseExact(subDate, "%M/%d/yyyy", null, DateTimeStyles.AllowWhiteSpaces, out subDateVal);

                            //parse the from due date as passed in the filter parameter
                            DateTime subDateFromVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_SUBMIT_DATE_FROM], "yyyy-MM-dd", null, DateTimeStyles.None, out subDateFromVal);
                            DateTime subDateToVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_SUBMIT_DATE_TO], "yyyy-MM-dd", null, DateTimeStyles.None, out subDateToVal);


                            if (DateTime.Compare(subDateVal, subDateFromVal) >= 0 && DateTime.Compare(subDateVal, subDateToVal) <= 0)
                                filterConditionSatisfied = filterConditionSatisfied & true;
                            else
                                filterConditionSatisfied = false;
                        }
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_PROD_CAT))
                        {
                            ArrayList rfqSpecList = RFQProdServQnty.getRFQProductServiceQuantityforRFIdDB(tempRFQ.getRFQId());
                            int j = 0;
                            filterConditionSatisfied = false;
                            //Even if one of the specificaion object matches the filter criteria, then allow this selection
                            for (j = 0; j < rfqSpecList.Count; j++)
                            {
                                RFQProdServQnty tempSpec = (RFQProdServQnty)rfqSpecList[j];
                                if (tempSpec.getProdCatId().Equals(filterParam[this.FILTER_BY_PROD_CAT]) ||
                                    BackEndObjects.ProductCategory.getRootLevelParentCategoryDB(tempSpec.getProdCatId()).ContainsKey(filterParam[this.FILTER_BY_PROD_CAT]))
                                { filterConditionSatisfied = true; break; }
                            }

                            //if (filterConditionSatisfied)
                            //tempRFQ.setRFQProdServList(rfqSpecList);
                        }
                        if (filterConditionSatisfied && filterParam.ContainsKey(FILTER_BY_ASSGND_TO))
                        {
                            String assgndTo = RFQResponse.getRFQResponseWOQuotesForRFQIdandResponseEntityIdDB(tempRFQ.getRFQId(), cmpUsrId).getAssignedTo();
                            filterConditionSatisfied = (assgndTo != null && assgndTo.Equals(filterParam[FILTER_BY_ASSGND_TO])) ? filterConditionSatisfied = filterConditionSatisfied & true : false;
                                
                        }
                        if (filterConditionSatisfied)
                        {
                            LeadRecord tempLeadRec = new LeadRecord();
                            tempLeadRec.setTermsandConds(tempRFQ.getTermsandConds());
                            tempLeadRec.setSubmitDate(tempRFQ.getSubmitDate());
                            tempLeadRec.setRFQId(tempRFQ.getRFQId());
                            tempLeadRec.setRFQName(tempRFQ.getRFQName());
                            tempLeadRec.setReqId(tempRFQ.getReqId());
                            tempLeadRec.setNDADocPath(tempRFQ.getNDADocPath());
                            tempLeadRec.setLocalityId(tempRFQ.getLocalityId());
                            tempLeadRec.setEntityId(tempRFQ.getEntityId());
                            tempLeadRec.setDueDate(tempRFQ.getDueDate());
                            tempLeadRec.setCreateMode(tempRFQ.getCreateMode());
                            tempLeadRec.setCreatedUsr(tempRFQ.getCreatedUsr());
                            tempLeadRec.setCreatedEntity(tempRFQ.getCreatedEntity());
                            tempLeadRec.setApprovalStat(tempRFQ.getApprovalStat());
                            tempLeadRec.setActiveStat(tempRFQ.getActiveStat());
                            tempLeadRec.setRFQProdServList(tempRFQ.getRFQProdServList());
                            tempLeadRec.setCurrency(tempRFQ.getCurrency());
                            //Set the entity name 
                            tempLeadRec.findAndSetEntityName();

                            allRFQFiltered.Add(tempLeadRec);
                        }
                        
                    }

                }
                return allRFQFiltered;
            }
            /// <summary>
            /// Returns all active leads which are meeting the filter criteria and are assigned to the given userid
            /// </summary>
            /// <param name="cmpUsrId"></param>
            /// <param name="userId"></param>
            /// <param name="filterParam"></param>
            /// <returns></returns>
            public ArrayList getAllActiveAndAssignedToLeadDetailsFilteredWOProdQntyAndSpec(String cmpUsrId, String userId, Dictionary<String, String> filterParam)
            {
                ArrayList allRFQList = new ArrayList();
                ArrayList allRFQFiltered = new ArrayList();

                allRFQList = RFQDetails.getAllActiveAndAssignedToUserLeadsforEntityIdDB(cmpUsrId, userId);
                int i = 0;

                for (i = 0; i < allRFQList.Count; i++)
                {
                    RFQDetails tempRFQ = (RFQDetails)allRFQList[i];


                    Boolean filterConditionSatisfied = true;

                    //ArrayList rfqSpecList = RFQProductServiceDetails.getAllProductServiceDetailsbyRFQIdDB(tempRFQ.getRFQId());

                    //ArrayList rfqSpecList = new ArrayList();

                    //foreach (KeyValuePair<String, RFQProductServiceDetails> kvp in rfqSpecDict)
                    //rfqSpecList.Add(kvp.Value);
                    if (filterParam.ContainsKey(this.FILTER_BY_RFQ_NO))
                        filterConditionSatisfied = tempRFQ.getRFQId().IndexOf(filterParam[this.FILTER_BY_RFQ_NO], StringComparison.InvariantCultureIgnoreCase) >= 0 ? true : false;

                    if (filterConditionSatisfied)
                    {
                        if (filterParam.ContainsKey(this.FILTER_BY_ACTIVE_STATUS))
                            filterConditionSatisfied = tempRFQ.getActiveStat().Equals(filterParam[this.FILTER_BY_ACTIVE_STATUS]) ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_CUST_ID))
                            filterConditionSatisfied = tempRFQ.getEntityId().Equals(filterParam[this.FILTER_BY_CUST_ID]) ? filterConditionSatisfied & true : false;
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_DUE_DATE_FROM) && filterParam.ContainsKey(this.FILTER_BY_DUE_DATE_TO))
                        {
                            //Parse the due date as stored in the object
                            String dueDate = tempRFQ.getDueDate().Substring(0, tempRFQ.getDueDate().IndexOf(" "));
                            DateTime dueDateVal;
                            DateTime.TryParseExact(dueDate, "%M/%d/yyyy", null, DateTimeStyles.AllowWhiteSpaces, out dueDateVal);

                            //parse the from due date as passed in the filter parameter
                            DateTime dueDateFromVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_DUE_DATE_FROM], "yyyy-MM-dd", null, DateTimeStyles.None, out dueDateFromVal);
                            DateTime dueDateToVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_DUE_DATE_TO], "yyyy-MM-dd", null, DateTimeStyles.None, out dueDateToVal);


                            if (DateTime.Compare(dueDateVal, dueDateFromVal) >= 0 && DateTime.Compare(dueDateVal, dueDateToVal) <= 0)
                                filterConditionSatisfied = filterConditionSatisfied & true;
                            else
                                filterConditionSatisfied = false;
                        }
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_SUBMIT_DATE_FROM) && filterParam.ContainsKey(this.FILTER_BY_SUBMIT_DATE_TO))
                        {
                            //Parse the due date as stored in the object
                            String subDate = tempRFQ.getSubmitDate().Substring(0, tempRFQ.getSubmitDate().IndexOf(" "));
                            DateTime subDateVal;
                            DateTime.TryParseExact(subDate, "%M/%d/yyyy", null, DateTimeStyles.AllowWhiteSpaces, out subDateVal);

                            //parse the from due date as passed in the filter parameter
                            DateTime subDateFromVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_SUBMIT_DATE_FROM], "yyyy-MM-dd", null, DateTimeStyles.None, out subDateFromVal);
                            DateTime subDateToVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_SUBMIT_DATE_TO], "yyyy-MM-dd", null, DateTimeStyles.None, out subDateToVal);


                            if (DateTime.Compare(subDateVal, subDateFromVal) >= 0 && DateTime.Compare(subDateVal, subDateToVal) <= 0)
                                filterConditionSatisfied = filterConditionSatisfied & true;
                            else
                                filterConditionSatisfied = false;
                        }
                        if (filterConditionSatisfied && filterParam.ContainsKey(this.FILTER_BY_PROD_CAT))
                        {
                            ArrayList rfqSpecList = RFQProdServQnty.getRFQProductServiceQuantityforRFIdDB(tempRFQ.getRFQId());
                            int j = 0;
                            filterConditionSatisfied = false;
                            //Even if one of the specificaion object matches the filter criteria, then allow this selection
                            for (j = 0; j < rfqSpecList.Count; j++)
                            {
                                RFQProdServQnty tempSpec = (RFQProdServQnty)rfqSpecList[j];
                                if (tempSpec.getProdCatId().Equals(filterParam[this.FILTER_BY_PROD_CAT]) ||
                                    BackEndObjects.ProductCategory.getRootLevelParentCategoryDB(tempSpec.getProdCatId()).ContainsKey(filterParam[this.FILTER_BY_PROD_CAT]))
                                { filterConditionSatisfied = true; break; }
                            }

                            //if (filterConditionSatisfied)
                            //tempRFQ.setRFQProdServList(rfqSpecList);
                        }
                        if (filterConditionSatisfied && filterParam.ContainsKey(FILTER_BY_ASSGND_TO))
                        {
                            String assgndTo = RFQResponse.getRFQResponseWOQuotesForRFQIdandResponseEntityIdDB(tempRFQ.getRFQId(), cmpUsrId).getAssignedTo();
                            filterConditionSatisfied = (assgndTo != null && assgndTo.Equals(filterParam[FILTER_BY_ASSGND_TO])) ? filterConditionSatisfied = filterConditionSatisfied & true : false;

                        }
                        if (filterConditionSatisfied)
                        {
                            LeadRecord tempLeadRec = new LeadRecord();
                            tempLeadRec.setTermsandConds(tempRFQ.getTermsandConds());
                            tempLeadRec.setSubmitDate(tempRFQ.getSubmitDate());
                            tempLeadRec.setRFQId(tempRFQ.getRFQId());
                            tempLeadRec.setRFQName(tempRFQ.getRFQName());
                            tempLeadRec.setReqId(tempRFQ.getReqId());
                            tempLeadRec.setNDADocPath(tempRFQ.getNDADocPath());
                            tempLeadRec.setLocalityId(tempRFQ.getLocalityId());
                            tempLeadRec.setEntityId(tempRFQ.getEntityId());
                            tempLeadRec.setDueDate(tempRFQ.getDueDate());
                            tempLeadRec.setCreateMode(tempRFQ.getCreateMode());
                            tempLeadRec.setCreatedUsr(tempRFQ.getCreatedUsr());
                            tempLeadRec.setCreatedEntity(tempRFQ.getCreatedEntity());
                            tempLeadRec.setApprovalStat(tempRFQ.getApprovalStat());
                            tempLeadRec.setActiveStat(tempRFQ.getActiveStat());
                            tempLeadRec.setRFQProdServList(tempRFQ.getRFQProdServList());
                            //Set the entity name 
                            tempLeadRec.findAndSetEntityName();

                            allRFQFiltered.Add(tempLeadRec);
                        }

                    }

                }
                return allRFQFiltered;
            }
            /// <summary>
            /// This method is similar to 'getAllLeadDetailsFiltered' except that it contains all leads which have already been converted to potential
            /// </summary>
            /// <param name="cmpUsrId"></param>
            /// <param name="userId"></param>
            /// <param name="filterParam"></param>
            /// <returns></returns>
            public ArrayList getAllLeadDetailsFilteredIncludingConvertedtoPotential(String cmpUsrId, String userId, Dictionary<String, String> filterParam)
            {
                ArrayList allRFQList = new ArrayList();
                ArrayList allRFQFiltered = new ArrayList();

                allRFQList = RFQDetails.getAllLeadsIncludingConvertedtoPotentialforEntityIdDB(cmpUsrId);
                int i = 0;

                for (i = 0; i < allRFQList.Count; i++)
                {
                    RFQDetails tempRFQ = (RFQDetails)allRFQList[i];


                    Boolean filterConditionSatisfied = false;

                    ArrayList rfqSpecList = RFQProductServiceDetails.getAllProductServiceDetailsbyRFQIdDB(tempRFQ.getRFQId());
                    //ArrayList rfqSpecList = new ArrayList();

                    //foreach (KeyValuePair<String, RFQProductServiceDetails> kvp in rfqSpecDict)
                    //rfqSpecList.Add(kvp.Value);

                    if (filterParam.ContainsKey(this.FILTER_BY_PROD_CAT))
                    {
                        int j = 0;
                        //Even if one of the specificaion object matches the filter criteria, then allow this selection
                        for (j = 0; j < rfqSpecList.Count; j++)
                        {
                            RFQProductServiceDetails tempSpec = (RFQProductServiceDetails)rfqSpecList[j];
                            if (tempSpec.getPrdCatId().Equals(filterParam[this.FILTER_BY_PROD_CAT]) ||
                                BackEndObjects.ProductCategory.getRootLevelParentCategoryDB(tempSpec.getPrdCatId()).ContainsKey(filterParam[this.FILTER_BY_PROD_CAT]))
                            { filterConditionSatisfied = true; break; }
                        }

                        if (filterConditionSatisfied)
                            tempRFQ.setRFQProdServList(rfqSpecList);
                    }

                    else
                    {
                        tempRFQ.setRFQProdServList(rfqSpecList);
                        filterConditionSatisfied = true;
                    }

                    if (filterConditionSatisfied)
                    {
                        if (filterParam.ContainsKey(this.FILTER_BY_ACTIVE_STATUS))
                            filterConditionSatisfied = tempRFQ.getActiveStat().Equals(filterParam[this.FILTER_BY_ACTIVE_STATUS]) ? filterConditionSatisfied & true : false; //overall filter condition value will be false if any of the filter condition is not satisfied
                        if (filterParam.ContainsKey(this.FILTER_BY_DUE_DATE_FROM) && filterParam.ContainsKey(this.FILTER_BY_DUE_DATE_TO))
                        {
                            //Parse the due date as stored in the object
                            String dueDate = tempRFQ.getDueDate().Substring(0, tempRFQ.getDueDate().IndexOf(" "));
                            DateTime dueDateVal;
                            DateTime.TryParseExact(dueDate, "%M/%d/yyyy", null, DateTimeStyles.AllowWhiteSpaces, out dueDateVal);

                            //parse the from due date as passed in the filter parameter
                            DateTime dueDateFromVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_DUE_DATE_FROM], "yyyy-MM-dd", null, DateTimeStyles.None, out dueDateFromVal);
                            DateTime dueDateToVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_DUE_DATE_TO], "yyyy-MM-dd", null, DateTimeStyles.None, out dueDateToVal);


                            if (DateTime.Compare(dueDateVal, dueDateFromVal) >= 0 && DateTime.Compare(dueDateVal, dueDateToVal) <= 0)
                                filterConditionSatisfied = filterConditionSatisfied & true;
                            else
                                filterConditionSatisfied = false;
                        }
                        if (filterParam.ContainsKey(this.FILTER_BY_SUBMIT_DATE_FROM) && filterParam.ContainsKey(this.FILTER_BY_SUBMIT_DATE_TO))
                        {
                            //Parse the due date as stored in the object
                            String subDate = tempRFQ.getSubmitDate().Substring(0, tempRFQ.getSubmitDate().IndexOf(" "));
                            DateTime subDateVal;
                            DateTime.TryParseExact(subDate, "%M/%d/yyyy", null, DateTimeStyles.AllowWhiteSpaces, out subDateVal);

                            //parse the from due date as passed in the filter parameter
                            DateTime subDateFromVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_SUBMIT_DATE_FROM], "yyyy-MM-dd", null, DateTimeStyles.None, out subDateFromVal);
                            DateTime subDateToVal;
                            DateTime.TryParseExact(filterParam[this.FILTER_BY_SUBMIT_DATE_TO], "yyyy-MM-dd", null, DateTimeStyles.None, out subDateToVal);


                            if (DateTime.Compare(subDateVal, subDateFromVal) >= 0 && DateTime.Compare(subDateVal, subDateToVal) <= 0)
                                filterConditionSatisfied = filterConditionSatisfied & true;
                            else
                                filterConditionSatisfied = false;
                        }

                        if (filterConditionSatisfied)
                        {
                            LeadRecord tempLeadRec = new LeadRecord();
                            tempLeadRec.setTermsandConds(tempRFQ.getTermsandConds());
                            tempLeadRec.setSubmitDate(tempRFQ.getSubmitDate());
                            tempLeadRec.setRFQId(tempRFQ.getRFQId());
                            tempLeadRec.setRFQName(tempRFQ.getRFQName());
                            tempLeadRec.setReqId(tempRFQ.getReqId());
                            tempLeadRec.setNDADocPath(tempRFQ.getNDADocPath());
                            tempLeadRec.setLocalityId(tempRFQ.getLocalityId());
                            tempLeadRec.setEntityId(tempRFQ.getEntityId());
                            tempLeadRec.setDueDate(tempRFQ.getDueDate());
                            tempLeadRec.setCreateMode(tempRFQ.getCreateMode());
                            tempLeadRec.setCreatedUsr(tempRFQ.getCreatedUsr());
                            tempLeadRec.setCreatedEntity(tempRFQ.getCreatedEntity());
                            tempLeadRec.setApprovalStat(tempRFQ.getApprovalStat());
                            tempLeadRec.setActiveStat(tempRFQ.getActiveStat());
                            tempLeadRec.setRFQProdServList(tempRFQ.getRFQProdServList());
                            //Set the entity name 
                            tempLeadRec.findAndSetEntityName();

                            allRFQFiltered.Add(tempLeadRec);
                        }

                    }

                }
                return allRFQFiltered;
            }
            /// <summary>
            /// For a particular lead response, this method provides the specfication objects and respective response quote from a response entity.
            /// The response object is 'RFQResponseQuoteTotal' which contains a property (another property is the total amount of a complete response quote)
            /// 'RFQResponseQuoteRecordList' which actually an ArrayList 
            /// of 'RFQResponseQuoteRecord' objects.
            /// 'RFQResponseQuoteRecord' is a subclass of 'RFQResponseQuotes' - each object of which represents one record of response for an RFQ/Lead for one particular.
            /// </summary>
            /// <param name="cmpUsrId"></param>
            /// <param name="userId"></param>
            /// <param name="responseEntId"></param>
            /// <param name="rfId"></param>
            /// <returns></returns>
            public RFQResponseQuoteTotal getLeadQuoteandSpecDetails(String responseEntId, String rfId)
            {
                if (rfId == null || rfId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid rfqId id value sent to method _dispLeads.getLeadQuoteandSpecDetails"));

                if (responseEntId == null || responseEntId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid response entity id value sent to method _dispLeads.getLeadQuoteandSpecDetails"));

                RFQResponseQuoteTotal completeQuoteResp = new RFQResponseQuoteTotal();
                ArrayList tempList = new ArrayList();
                float totalAmount = 0;

                Dictionary<String, RFQResponseQuotes> allRespQut = RFQResponseQuotes.getAllResponseQuotesforRFQandResponseEntityDB(rfId, responseEntId);

                foreach (KeyValuePair<String, RFQResponseQuotes> kvp in allRespQut)
                {
                    RFQResponseQuotes tempRec = kvp.Value;

                    RFQResponseQuoteRecord tempQuoteRec = new RFQResponseQuoteRecord();

                    RFQProdServQnty tempProdQnty = RFQProdServQnty.getRFQProductServiceQuantityforRFIdandCatIdDB(tempRec.getRFQId(), tempRec.getPrdCatId());

                    tempQuoteRec.setUnitName(tempRec.getUnitName());
                    tempQuoteRec.setRFQId(tempRec.getRFQId());
                    tempQuoteRec.setResponseUsrId(tempRec.getResponseUsrId());
                    tempQuoteRec.setResponseEntityId(tempRec.getResponseEntityId());
                    tempQuoteRec.setQuote(tempRec.getQuote());
                    tempQuoteRec.setPrdCatId(tempRec.getPrdCatId());

                    float qntyVal=(tempProdQnty.getFromQnty()!=0&&tempProdQnty.getToQnty()!=0?
                        (tempProdQnty.getFromQnty()+tempProdQnty.getToQnty())/2:0);
                    qntyVal = (qntyVal == 0 ? (tempProdQnty.getFromQnty() == 0 ? tempProdQnty.getToQnty() : tempProdQnty.getFromQnty()) : qntyVal);

                    tempQuoteRec.setQuantity(qntyVal);
                    tempQuoteRec.setAmount(qntyVal * float.Parse(tempQuoteRec.getQuote()));

                    totalAmount += tempQuoteRec.getAmount();
                    //tempQuoteRec.setAmntQuntyandProdSpec();
                    tempList.Add(tempQuoteRec);

                }
                completeQuoteResp.setRFQResponseQuoteRecordList(tempList);
                completeQuoteResp.setTotalAmnt(totalAmount);

                return completeQuoteResp;
            }
        }

        /// <summary>
        /// This class contains methods for updating details of manually created Leads.
        /// </summary>
        public class _updateLeads
        {
            /// <summary>
            /// This method is for updating a Lead details if the Lead was created manually.
            /// If this is a delete operation, records from RFQDetails, and other prod/service details will be deleted.
            /// In the object of 'LeadRecord' pass only those properties (other than the RFQ id) which need to be updated.
            /// No need to send the entityName with this parameter as the enttity name will not be modified by this method (only the entity id will be modified).
            /// To update related product/service details for a manually created Lead, use the method 'updateLeadProdServDetails'.
            /// </summary>
            /// <param name="rfS"></param>
            /// <param name="cmpUsrId"></param>
            /// <param name="userId"></param>
            /// <param name="operation"></param>
            /// <returns></returns>
            public int updateLead(LeadRecord rfS, String cmpUsrId, String userId, String operation)
            {
                if (cmpUsrId == null || cmpUsrId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid entity id value sent to method _updateLeads.updateLead"));

                if (userId == null || userId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid userId id value sent to method _updateLeads.updateLead"));

                if (rfS == null || rfS.getRFQId() == null || rfS.getRFQId().Equals("") )
                    throw (new CustomExceptions.invalidParamException("Invalid Lead details value sent to method _updateLeads.updateLead"));

                if (!rfS.getCreateMode().Equals(RFQDetails.CREATION_MODE_MANUAL))
                    throw (new CustomExceptions.businessRuleViolationException("Only Manually Created Lead Entries Can be Modified - _updateLeads.updateLead"));

                if (!operation.Equals(DBConn.Connections.OPERATION_UPDATE) && !operation.Equals(DBConn.Connections.OPERATION_DELETE))
                    throw (new CustomExceptions.invalidParamException("Invalid operation Name passed to method _updateLeads.updateLead"));

                Dictionary<String, String> updateTargetVals = new Dictionary<string, string>();
                Dictionary<String, String> updateWhereVals = new Dictionary<string, string>();

                int totalRecAffected = 0;

                if (operation.Equals(DBConn.Connections.OPERATION_UPDATE))
                {
                                      
                    if(rfS.getActiveStat()!=null)
                        updateTargetVals.Add(RFQDetails.RFQ_COL_ACTIVE_STAT, rfS.getActiveStat());
                    if (rfS.getApprovalStat() != null)
                        updateTargetVals.Add(RFQDetails.RFQ_COL_APPROVAL_STAT, rfS.getApprovalStat());
                    if (rfS.getDueDate() != null)
                        updateTargetVals.Add(RFQDetails.RFQ_COL_DUE_DATE, rfS.getDueDate());
                    if (rfS.getEntityId() != null)
                        updateTargetVals.Add(RFQDetails.RFQ_COL_ENTITY_ID, rfS.getEntityId());
                    if (rfS.getLocalityId() != null)
                        updateTargetVals.Add(RFQDetails.RFQ_COL_LOCAL_ID, rfS.getLocalityId());
                    if (rfS.getNDADocPath() != null)
                        updateTargetVals.Add(RFQDetails.RFQ_COL_NDA_PATH, rfS.getNDADocPath().ToString());
                    if (rfS.getReqId() != null)
                        updateTargetVals.Add(RFQDetails.RFQ_COL_RELATED_REQ,rfS.getReqId());
                    //if (rfS.getRFQId() != null)
                        //updateTargetVals.Add(RFQDetails.RFQ_COL_RFQ_ID, rfS.getRFQId());
                    if (rfS.getTermsandConds() != null)
                        updateTargetVals.Add(RFQDetails.RFQ_COL_T_AND_C, rfS.getTermsandConds());

                    updateWhereVals.Add(RFQDetails.RFQ_COL_RFQ_ID, rfS.getRFQId());
                    
                    totalRecAffected += RFQDetails.updateRFQDetailsDB(updateTargetVals, updateWhereVals, operation);
                }
                else if (operation.Equals(DBConn.Connections.OPERATION_DELETE))
                {
                    //In case of a delete .. delete the main record from the RFQDetails table which will have a cascading effect on RFQShortlisted table and other related
                    //tables automatically
                    updateWhereVals.Add(RFQDetails.RFQ_COL_RFQ_ID, rfS.getRFQId());
                    totalRecAffected = RFQDetails.updateRFQDetailsDB(updateTargetVals, updateWhereVals, operation);
                }

                return totalRecAffected;
            }
            /// <summary>
            /// The first parameter is mainly checked to confirm the creation mode. No need to send the entiy name with it.
            ///The fourth parameter is an ArrayList of 'RFQProductServiceDetails' associated with a particular Lead record.
            /// In case a delete was performed a check if there is no record available in the table 'RFQ_Product_Service_Map' after the deletion for the given
            ///RFQid and prodcatid.
            ///If Yes, this method will delete all entries from the dependent table (this can not be done through foreign key rule) 'RFQ_Response_Quote_Details' for the respective 
            ///RFQid and prodcatid
            /// </summary>
            /// <param name="leadRec"></param>
            /// <param name="cmpUsrId"></param>
            /// <param name="userId"></param>
            /// <param name="prodServList"></param>
            /// <param name="operation"></param>
            /// <returns></returns>
            public int updateLeadProdServDetails(LeadRecord leadRec, String cmpUsrId, String userId, ArrayList prodServList, String operation)
            {
                if (cmpUsrId == null || cmpUsrId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid entity id value sent to method _updateLeads.updateLeadProdServDetails"));

                if (userId == null || userId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid userId id value sent to method _updateLeads.updateLeadProdServDetails"));

                if (!leadRec.getCreateMode().Equals(RFQShortlisted.POTENTIAL_CREATION_MODE_MANUAL))
                    throw (new CustomExceptions.businessRuleViolationException("Only Manually Created Lead Entries Can be Modified - _updateLeads.updateLeadProdServDetails"));

                //if(operation.Equals(DBConn.Connections.OPERATION_DELETE))
                //throw (new CustomExceptions.invalidParamException("For delete operation use 'updatePotential' method which will delete all dependent table records _updatePotentials.updatePotentialProdServDetails"));

                int totalRecAffected = 0;
                for (int count = 0; count < prodServList.Count; count++)
                {
                    RFQProductServiceDetails RFQProdSpecObj = (RFQProductServiceDetails)prodServList[count];

                    if (RFQProdSpecObj.getPrdCatId() == null || RFQProdSpecObj.getPrdCatId().Equals("") || RFQProdSpecObj.getFeatId() == null || RFQProdSpecObj.getFeatId().Equals(""))
                        throw (new CustomExceptions.invalidParamException("Product Category Id and Feature id necessary to update Requirement specification - method  _updateLeads.updateLeadProdServDetails"));

                    Dictionary<String, String> updateTargetVals = new Dictionary<string, string>();
                    Dictionary<String, String> updateWhereVals = new Dictionary<string, string>();

                    if (RFQProdSpecObj.getFromSpecId() != null)
                        updateTargetVals.Add(RFQProductServiceDetails.RFQ_PROD_COL_FROM_SPEC_ID, RFQProdSpecObj.getFromSpecId());
                    if (RFQProdSpecObj.getToSpecId() != null)
                        updateTargetVals.Add(RFQProductServiceDetails.RFQ_PROD_COL_TO_SPEC_ID, RFQProdSpecObj.getToSpecId());
                    if (RFQProdSpecObj.getMsrmntUnit() != null)
                        updateTargetVals.Add(RFQProductServiceDetails.RFQ_PROD_COL_MSRMNT_UNIT, RFQProdSpecObj.getMsrmntUnit());
                    if (RFQProdSpecObj.getImgPath() != null) 
                        updateTargetVals.Add(RFQProductServiceDetails.RFQ_PROD_COL_IMAGE_PATH, RFQProdSpecObj.getImgPath().ToString());
                    if (RFQProdSpecObj.getSpecText() != null)
                        updateTargetVals.Add(RFQProductServiceDetails.RFQ_PROD_COL_SPEC_TEXT, RFQProdSpecObj.getSpecText());
                    if (RFQProdSpecObj.getQuantity() != 0)
                        updateTargetVals.Add(RFQProductServiceDetails.RFQ_PROD_COL_QUANTITY, RFQProdSpecObj.getQuantity().ToString());

                    updateWhereVals.Add(RFQProductServiceDetails.RFQ_PROD_COL_RFQ_ID, RFQProdSpecObj.getRFQId());
                    updateWhereVals.Add(RFQProductServiceDetails.RFQ_PROD_COL_PROD_ID, RFQProdSpecObj.getPrdCatId());
                    updateWhereVals.Add(RFQProductServiceDetails.RFQ_PROD_COL_FEAT_ID, RFQProdSpecObj.getFeatId());

                    totalRecAffected += RFQProductServiceDetails.updateRFQProductServiceDetails(updateTargetVals, updateWhereVals, operation);

                    if (operation.Equals(DBConn.Connections.OPERATION_DELETE))
                    {
                        //In case a delete was performed check if there is no record available in the table 'RFQ_Product_Service_Map' after the deletion for the given
                        //RFQid and prodcatid.
                        //If Yes, delete all entries from the dependent table (this can not be done through foreign key rule) 'RFQ_Response_Quote_Details' for the respective 
                        //RFQid and prodcatid
                        Dictionary<String, RFQProductServiceDetails> checkAvail =
                        RFQProductServiceDetails.getAllProductServiceDetailsbyRFQandProductIdDB(RFQProdSpecObj.getRFQId(), RFQProdSpecObj.getPrdCatId());
                        if (checkAvail == null || checkAvail.Count == 0)
                        {
                            updateTargetVals = new Dictionary<string, string>();
                            updateWhereVals = new Dictionary<string, string>();

                            updateWhereVals.Add(RFQResponseQuotes.RFQ_RES_QUOTE_COL_RFQ_ID, RFQProdSpecObj.getRFQId());
                            updateWhereVals.Add(RFQResponseQuotes.RFQ_RES_QUOTE_COL_PROD_CAT, RFQProdSpecObj.getPrdCatId());

                            //Removing all response entries for that particualr RFQid and product cateogry id
                            RFQResponseQuotes.updateRFQResponseQuotesDB(updateTargetVals, updateWhereVals, DBConn.Connections.OPERATION_DELETE);
                        }
                    }
                }

                return totalRecAffected;
            }
        }
        /// <summary>
        /// This class contains methods to creat/update reponse for a particular lead entry.
        /// This updated response will be viewed by the customer in his RFQ response section.
        /// </summary>
        public class _respondToLeads
        {
            /// <summary>
            /// For a given rfq id, this method checks whether or not one response has already been provided by the response entity id.
            /// If yes, this method returns a true.
            /// The first parameter is the reponse entity id;
            /// The second parameter is the user id of the reponse entity id;
            /// The third parameter is the RFQ id.
            /// </summary>
            /// <param name="respEntityId"></param>
            /// <param name="userId"></param>
            /// <param name="rfId"></param>
            /// <returns></returns>
            public Boolean isFirstResponse(String respEntityId,String userId,String rfId)
            {
                if (respEntityId == null || respEntityId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid entity id value sent to method _respondToLeads.isFirstResponse"));

                if (userId == null || userId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid userId id value sent to method _respondToLeads.isFirstResponse"));

                if (rfId == null || rfId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid rfq id value sent to method _respondToLeads.isFirstResponse"));

                RFQResponse rfsObj=RFQResponse.getRFQResponseforRFQIdandResponseEntityIdDB(rfId, respEntityId);
                if (rfsObj == null || !rfsObj.getRFQId().Equals(rfId.Trim()))
                    return true;
                else
                    return false;
            }
            /// <summary>
            /// This method can be used to both create /update the response quote details for a given RFQ.
            /// The first parameter is RFQResponse object (if this object is found already existing in database then this will be considered as an Update request).
            /// The second parameter is an ArrayList of 'RFQResponseQuotes' objects (for update, send only those response quote records which needs to be updated - 
            /// ALSO , for an update the prod category value must be sent for each of item in this list).
            /// The third parameter is useful only for update request.. this parameter says whether or not to update the NDA/documents passed through the first parameter.
            /// For an insert request it is advisable NOT TO provide any rfqid in the 'RFQResponse' object as this will be ignored and system will create new id.
            /// </summary>
            /// <param name="rfS"></param>
            /// <param name="rfQuotes"></param>
            /// <param name="updateNDA"></param>
            /// <returns></returns>
            public int updateOrCreateLeadQuoteDetails(RFQResponse rfS,ArrayList rfQuotes,Boolean updateNDA )
            {
                if (rfS == null || rfS.getRFQId() == null || rfS.getRFQId().Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid 'RFQResponse' object value sent to method _respondToLeads.updateOrCreateLeadQuoteDetails"));

                if (rfQuotes == null || rfQuotes.Count==0)
                    throw (new CustomExceptions.invalidParamException("Invalid Quote List  sent to method _respondToLeads.updateOrCreateLeadQuoteDetails"));
                
                int rowsAffected = 0;

                Boolean insertRequest =(rfS.getRFQId()==null?true:(rfS.getRFQId().Equals("")==true?true:false));
                if (!insertRequest)
                {
                    RFQResponse rfsObj = RFQResponse.getRFQResponseforRFQIdandResponseEntityIdDB(rfS.getRFQId(), rfS.getRespEntityId());
                    if (rfsObj == null || !rfsObj.getRFQId().Equals(rfS.getRFQId()))
                        insertRequest = true;
                }

                if (insertRequest)
                {

                    Id idGenerator = new Id();
                    rfS.setRFQId(idGenerator.getNewId(Id.ID_TYPE_RFQ_STRING));
                    rowsAffected+=RFQResponse.insertRFQResponseDB(rfS);

                    for (int count = 0; count < rfQuotes.Count; count++)
                    {
                        RFQResponseQuotes respQuoteTemp = (RFQResponseQuotes)rfQuotes[count];
                        respQuoteTemp.setRFQId(rfS.getRFQId());
                        rowsAffected+=RFQResponseQuotes.insertRFQResponseQuotesDB(respQuoteTemp);
                    }

                }
                else
                {
                    if (updateNDA)
                        rowsAffected += RFQResponse.updateorInsertRFQResponseNDADB(rfS);

                    for (int count = 0; count < rfQuotes.Count; count++)
                    {
                        RFQResponseQuotes respQuoteTemp = (RFQResponseQuotes)rfQuotes[count];
                        if(respQuoteTemp.getPrdCatId()==null ||respQuoteTemp.getPrdCatId().Equals(""))
                            throw (new CustomExceptions.invalidParamException("Invalid prod category id found in Quote - _respondToLeads.updateOrCreateLeadQuoteDetails"));

                        Dictionary<String, String> targetVal = new Dictionary<string, string>();
                        Dictionary<String, String> whereCls = new Dictionary<string, string>();

                        whereCls.Add(RFQResponseQuotes.RFQ_RES_QUOTE_COL_RFQ_ID, rfS.getRFQId());
                        whereCls.Add(RFQResponseQuotes.RFQ_RES_QUOTE_COL_RESP_COMP_ID, rfS.getRespEntityId());
                        whereCls.Add(RFQResponseQuotes.RFQ_RES_QUOTE_COL_PROD_CAT, respQuoteTemp.getPrdCatId());
                        if (respQuoteTemp.getQuote() != null)
                            targetVal.Add(RFQResponseQuotes.RFQ_RES_QUOTE_COL_QUOTE, respQuoteTemp.getQuote());
                        if(respQuoteTemp.getUnitName()!=null)
                            targetVal.Add(RFQResponseQuotes.RFQ_RES_QUOTE_COL_UNIT_NAME, respQuoteTemp.getUnitName());
                        if(respQuoteTemp.getResponseUsrId()!=null)
                            targetVal.Add(RFQResponseQuotes.RFQ_RES_QUOTE_COL_RESP_COMP_USR, respQuoteTemp.getResponseUsrId());

                        rowsAffected+=RFQResponseQuotes.updateRFQResponseQuotesDB(targetVal, whereCls, Connections.OPERATION_UPDATE);
                    }
                }
                return rowsAffected;
            }
        }
        /// <summary>
        /// This class contains all methods to create a new Lead entry Manually from the Sales screen
        /// </summary>
        public class _createLeads
        {
            /// <summary>
            /// This method returns all product/service category details for which there is no parent category;
            ///  i.e, it returns a list of all top level product/service category.This method is particularly useful for scenarios where it is required to
            ///  list the main catogories online.
            /// </summary>
            /// <returns></returns>
            public Dictionary<String, ProductCategory> getParentProductDetails()
            {
                return ProductCategory.getAllParentCategory();
            }
            /// <summary>
            /// for any given parent categoryid  returns all the associated child cateogories
            /// </summary>
            /// <returns></returns>
            public Dictionary<String, ProductCategory> getChildProductDetails(String catId)
            {
                return ProductCategory.getAllChildCategoryDB(catId);
            }
            /// <summary>
            /// for any given product category id, returns all the associated Feature objects.
            /// Each of the elements of the returned dictionary has a Feature object which in turn contains all the specifications attached to it.
            /// </summary>
            /// <param name="catId"></param>
            /// <returns></returns>
            public Dictionary<String, Features> getAllFeaturesForCategory(String catId)
            {
                Dictionary<String, Features> prodFeat = ProductCategory.getFeatureforCategoryDB(catId);

                foreach (KeyValuePair<String, Features> kvp in prodFeat)
                {
                    //Get all specifications related to each of the feature object
                    Dictionary<String, Specifications> tempSpec = Features.getSpecforFeatureDB(kvp.Key);

                    //Put all the specification objects in an arrayList , because feature object's specification property is an arraylist
                    ArrayList allSpecListforFeat = new ArrayList();
                    foreach (KeyValuePair<String, Specifications> kvps in tempSpec)
                        allSpecListforFeat.Add(kvps.Value);
                    //Add the specification ArrayList into the Feature object
                    kvp.Value.setSpecifications(allSpecListforFeat);
                }

                return prodFeat;
            }
            /// <summary>
            /// Returns all units of measurement from database. Each element of the returned ArrayList is an object of 'UnitOfMsrmnt'.
            /// Use 'getUnitName()' method on each of the containing objects to get the name of the units.
            /// </summary>
            /// <returns></returns>
            public ArrayList getAllUnitsOfMsrmnt()
            {
                return UnitOfMsrmnt.getAllMsrmntUnitsDB();
            }
            /// <summary>
            /// Returns an ArrayList which contains only the names of all the available currecnies in the database
            /// </summary>
            /// <returns></returns>
            public ArrayList getAllCurrencies()
            {
                return Currency.getAllCurrencyNamesDB();
            }
            /// <summary>
            /// Returns all entries of the database table 'Quote_Units'returns all the quote units objects from database.
            /// Each element of the returned entity has the property 'unitAndDivisor' which is a Dictionary<String,float> of a single record.
            /// use the 'getUnitAndDivisor'/'setUnitAndDivisor' as getter/setter for the property.
            /// The 'key' of the Dictionary<String,float> is 'unit name' and the 'value' is the respective divisor of the quote unit.
            /// </summary>
            /// <returns></returns>
            public ArrayList getAllQuoteUnits()
            {
                return QuoteUnits.getAllQuoteUnitsDB();
            }
            /// <summary>
            /// This method returns a dictionary; each element of the dictionary is the id of the country (the 'key') and the
            /// name of the country (the 'value').
            /// </summary>
            /// <returns></returns>
            public Dictionary<String, String> getAllCountryNames()
            {
                Dictionary<String, Country> countryDict = Country.getAllCountrywoStatesDB();
                Dictionary<String, String> countryNamesDict = new Dictionary<string, string>();

                ArrayList allCountryNames = new ArrayList();
                foreach (KeyValuePair<String, Country> kvp in countryDict)
                    countryNamesDict.Add(kvp.Key, kvp.Value.getCountryName());

                return countryNamesDict;
            }
            /// <summary>
            /// For a given country id,this method returns a dictionary; each element of the dictionary is the id of the State (the 'key') and the
            /// name of the State (the 'value').
            /// </summary>
            /// <param name="cId"></param>
            /// <returns></returns>
            public Dictionary<String, String> getAllStateNamesForCountry(String cId)
            {
                Dictionary<String, State> tempStateData = State.getStatesforCountryDB(cId);
                Dictionary<String, String> allStateNames = new Dictionary<string, string>();

                foreach (KeyValuePair<String, State> kvp in tempStateData)
                    allStateNames.Add(kvp.Key, kvp.Value.getStateName());

                return allStateNames;
            }
            /// <summary>
            /// For a given state id,this method returns a dictionary; each element of the dictionary is the id of the City (the 'key') and the
            /// name of the City (the 'value').
            /// </summary>
            /// <param name="stId"></param>
            /// <returns></returns>
            public Dictionary<String, String> getAllCityNamesForState(String stId)
            {
                Dictionary<String, City> tempCityData = City.getCitiesforStateDB(stId);
                Dictionary<String, String> allCityNames = new Dictionary<string, string>();

                foreach (KeyValuePair<String, City> kvp in tempCityData)
                    allCityNames.Add(kvp.Key, kvp.Value.getCityName());

                return allCityNames;

            }
            /// <summary>
            /// For a given City id,this method returns a dictionary; each element of the dictionary is the id of the Locality (the 'key') and the
            /// name of the Locality (the 'value').
            /// </summary>
            /// <param name="ctId"></param>
            /// <returns></returns>
            public Dictionary<String, String> getAllLocalityNamesForCity(String ctId)
            {
                Dictionary<String, Localities> tempLocalityData = Localities.getLocalitiesforCityDB(ctId);
                Dictionary<String, String> allLocalityNames = new Dictionary<string, string>();

                foreach (KeyValuePair<String, Localities> kvp in tempLocalityData)
                    allLocalityNames.Add(kvp.Key, kvp.Value.getLocalityName());

                return allLocalityNames;

            }
            /// <summary>
            /// For a given entity id, it returns an ArrayList containing 'Contacts' object from database.
            /// This list is the contact list of this particular organization.
            /// </summary>
            /// <param name="entityId"></param>
            /// <param name="userId"></param>
            /// <returns></returns>
            public ArrayList getAllContactsForEntity(String entityId, String userId)
            {
                if (entityId == null || entityId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid entity id value sent to method _createLeads.getAllContactsForEntity"));

                if (userId == null || userId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid userId id value sent to method _createLeads.getAllContactsForEntity"));

                return Contacts.getAllContactsbyEntityIdDB(entityId);
            }
            /// <summary>
            /// This method should only be used to CREATE A LEAD ENTRY MANUALLY.
            /// This method creates a new Lead entry into database.
            /// In the backend Lead entries are maintained as RFQDetails objects
            /// This method inserts the Lead Record object into the database (along with all the associated objects)
            /// those details are also inserted into the database.
            /// This method returns the number of records created in the database. In case it returns 0, that means the insert operation failed.
            /// For a new Manually created Lead, the creation Mode should always be 'Manusl' (RFQDetails.RFQDetails.CREATION_MODE_MANUAL). 
            /// </summary>
            /// <param name="leadObj"></param>
            public int createNewLead(LeadRecord  leadObj,String respEntId, String userId)
            {
                if (leadObj == null)
                    throw (new CustomExceptions.invalidParamException("Invalid 'RFQDetails' object sent to method _createLeads.createNewLead"));
                if (leadObj.getCreateMode() == null || !leadObj.getCreateMode().Equals(RFQDetails.CREATION_MODE_MANUAL))
                    throw (new CustomExceptions.businessRuleViolationException("For a Manually created Lead entry creation mode should be always be Manual - from method _createLeads.createNewLead"));
                if (respEntId == null || respEntId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid entity id value sent to method _createLeads.createNewLead"));
                if (userId == null || userId.Equals(""))
                    throw (new CustomExceptions.invalidParamException("Invalid userId id value sent to method _createLeads.createNewLead"));
                if(!leadObj.getCreatedEntity().Equals(respEntId.Trim()))
                    throw (new CustomExceptions.businessRuleViolationException("For Manually created Lead entry the createdEntity value of RFQDetails object must match the response entity id value"));

                Id IdGenerator = new Id();
                if(leadObj.getRFQId()==null || leadObj.getRFQId().Equals(""))
                leadObj.setRFQId(IdGenerator.getNewId(Id.ID_TYPE_RFQ_STRING));

                RFQDetails rfObj = new RFQDetails();
                rfObj.setRFQId(leadObj.getRFQId());
                rfObj.setActiveStat(leadObj.getActiveStat());
                rfObj.setApprovalStat(leadObj.getApprovalStat());
                rfObj.setCreatedEntity(leadObj.getCreatedEntity());
                rfObj.setCreatedUsr(leadObj.getCreatedUsr());
                rfObj.setCreateMode(leadObj.getCreateMode());
                rfObj.setDueDate(leadObj.getDueDate());
                rfObj.setEntityId(leadObj.getEntityId());
                rfObj.setFileStream(leadObj.getFileStream());
                rfObj.setLocalityId(leadObj.getLocalityId());
                rfObj.setNDADocPath(leadObj.getNDADocPath());
                rfObj.setNDADocPath(leadObj.getNDADocPath());
                rfObj.setReqId(leadObj.getReqId());
                rfObj.setRFQName(leadObj.getRFQName());
                rfObj.setRFQProdServList(leadObj.getRFQProdServList());
                rfObj.setRFQProdServQntyList(leadObj.getRFQProdServQntyList());
                rfObj.setSubmitDate(leadObj.getSubmitDate());
                rfObj.setTermsandConds(leadObj.getTermsandConds());
                rfObj.setCurrency(leadObj.getCurrency());

                int rowsAffected = RFQDetails.insertRFQDetailsDB(rfObj);

                RFQResponse leadRespObj = leadObj.getLeadResp();
                rowsAffected+=RFQResponse.insertRFQResponseDB(leadRespObj);

                return rowsAffected;
            }

        }

    }
    /// <summary>
    /// This class is represents the customer details data. Method of this class can be used in multple contexts to get the customer details to be
    /// displaed on lead/potential screens etc.
    /// </summary>
    public class customerDetails
    {
        public static String RETURN_OBJECT_TYPE_CONTACT_DETAILS = "contactDetails";
        public static String RETURN_OBJECT_TYPE_MAIN_BUSINESS_ENTITY = "mainEntity";
        /// <summary>
        /// For a given customer id and business entity id, this method searches in Contact list and Main business entity backend objects and returns
        /// the matching customer entity detail object.
        /// The returned value is a dictionary where the key defines the type of object being returned (RETURN_OBJECT_TYPE_CONTACT_DETAILS/RETURN_OBJECT_TYPE_MAIN_BUSINESS_ENTITY)
        /// and the value is an object of respective type.
        /// </summary>
        /// <param name="contactEntId"></param>
        /// <param name="cmpUsrUsrId"></param>
        /// <returns></returns>
        public static Dictionary<String, Object> getContactDetails(String contactEntId, String cmpUsrUsrId)
        {
            Dictionary<String, Object> returnObj = new Dictionary<string, object>();
            
            BackEndObjects.Contacts contactObj = BackEndObjects.Contacts.getContactDetailsforContactEntityDB(cmpUsrUsrId, contactEntId);

            if (contactObj != null && contactObj.getContactEntityId() != null && !contactObj.getContactEntityId().Equals(""))
                returnObj.Add(customerDetails.RETURN_OBJECT_TYPE_CONTACT_DETAILS, contactObj);
            else
            {
                MainBusinessEntity mBE=BackEndObjects.MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(contactEntId);
                AddressDetails adObj = BackEndObjects.AddressDetails.getAddressforMainBusinessEntitybyIdDB(contactEntId);
                if (adObj != null && adObj.getLocalityId() != null && !adObj.getLocalityId().Equals(""))
                    mBE.setAddressDetails(adObj);
                Dictionary<String, ProductCategory> prodDict = BackEndObjects.MainBusinessEntity.getProductDetailsforMainEntitybyIdDB(contactEntId);
                if(prodDict.Count>0)
                    mBE.setMainProductServices(prodDict);
                returnObj.Add(customerDetails.RETURN_OBJECT_TYPE_MAIN_BUSINESS_ENTITY, mBE);
            }

            return returnObj;
        }
    } 
    /// <summary>
    /// Each instance of this class represents one record of the 'All Requirements' grid in the purchase screen.
    /// This class is available only to this namespace
    /// </summary>
    public class RequirementRecords: Requirement
    {
        private ArrayList taggedRFQList;

        public ArrayList getTaggedRFQList()
        {
            return taggedRFQList;
        }
        public void setTaggedRFQList(ArrayList lst)
        {
            taggedRFQList = lst;
        }
         }
    /// <summary>
    /// This class adds properties to the bases class 'RFQShortlisted' where some properties are not directly available and needs to be 
    /// extracted from the associated RFQDetails object.
    /// e.g - duedate,EntityId,createdBy,entityName
    /// </summary>
    public class PotentialRecords : RFQShortlisted
    {
        public String dueDate;
        public String EntityId;
        public String createdBy;
        public String createdByEntity;
        public String entityName;
        public String RFQName;
        public String currency;

        public String getCurrency()
        {
            return this.currency;
        }
        public void setCurrency(String curr)
        {
            this.currency = curr;
        }
        public String getRFQName()
        {
            return this.RFQName;
        }
        public void setRFQName(String rfN)
        {
            this.RFQName=rfN;
        }
        public String getDueDate()
        {
            return this.dueDate;
        }
        public void setDueDate(String dt)
        {
            this.dueDate = dt;
        }
        public String getEntityId()
        {
            return EntityId;
        }
        public void setEntityId(String entId)
        {
            EntityId = entId;
        }
        public String getCreatedBy()
        {
            return createdBy;
        }
        public void setCreatedBy(String crtUsr)
        {
            createdBy = crtUsr;
        }
        public String getCreatedByEntity()
        {
            return this.createdByEntity;
        }
        public void setCreatedByEntity(String entId)
        {
            this.createdByEntity = entId;
        }
        public String getEntityName()
        {
            return this.entityName;
        }
        public void setEntityName(String eName)
        {
            this.entityName = eName;
        }

        /// <summary>
        /// This method finds the entity name by certain business rule and set the property 'entityName'.
        /// Invoke this method only after setting all other properties of the 'PotentialRecords' object.
        /// </summary>
        public Boolean findAndSetEntityName()
        {
            if (this.getRFQId() == null || this.getRFQId().Equals(""))
                throw (new CustomExceptions.invalidParamException("RFQId not set  -PotentialRecords.findAndSetEntityName()"));
            if (this.getEntityId() == null || this.getEntityId().Equals(""))
                throw (new CustomExceptions.invalidParamException("Entity id not set  -PotentialRecords.findAndSetEntityName()"));
            if (this.getCreateMode() == null || this.getCreateMode().Equals(""))
                throw (new CustomExceptions.invalidParamException("Creation mode not set  -PotentialRecords.findAndSetEntityName()"));

            if (this.getCreateMode().Equals(RFQDetails.CREATION_MODE_MANUAL))
            {
                String tempName = Contacts.getContactDetailsforContactEntityDB(this.getCreatedByEntity(), this.getEntityId()).getContactName();

                if (tempName == null || tempName.Equals(""))
                    tempName = MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(this.getEntityId()).getEntityName();

                this.setEntityName(tempName);
            }
            else
            {
                this.setEntityName(MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(this.getEntityId()).getEntityName());
            }

            return true;
        }

    }
    /// <summary>
    /// Each instance of this class represents one record of the RFQ Response Quote.
    /// Each record contains the product/service details, total amount, quanity.
    /// A complete response from a vendor for a RFQ may contain multiple  records like this.
    /// </summary>
    public class RFQResponseQuoteRecord:RFQResponseQuotes
    {
        private float amount;
        private float quantity=0;
        private RFQProductServiceDetails prodServiceDetails;
        //private Dictionary<String,float> quoteUnit;

        public void setAmount(float amnt)
        {
            amount=amnt;
        }
        public float getAmount()
        {
            return amount;
        }
        public void setQuantity(float qnt)
        {
            quantity=qnt;
        }
        public float getQuantity()
        {
            return quantity;
        }
        public void setProdServiceDetails(RFQProductServiceDetails prdSrv)
        {
            prodServiceDetails=prdSrv;
        }
        public RFQProductServiceDetails getProdServiceDetails()
        {
            return prodServiceDetails;
        }

        /// <summary>
        /// THIS METHOD NEEDS TO BE RE-WRITTEN - AS THIS CURRENTLY IS SET TO A DUMMY METHOD.
        /// By default a 'RFQResponseQuoteRecord' object does not have the three properties set/calculated - 'amount','quantity' and 'prodServiceDetails'.
        /// Calling this method will set these properties properly on that object.
        /// </summary>
        public void setAmntQuntyandProdSpec()
        {
                /*RFQProductServiceDetails prodServDet=new RFQProductServiceDetails();
                RFQProductServiceDetails.getAllProductServiceDetailsbyRFQIdDB(this.getRFQId()).TryGetValue(this.getPrdCatId(),out prodServDet);

            if(prodServDet!=null)
                this.setProdServiceDetails(prodServDet);

            if(this.getQuantity()==0)
                this.setQuantity(prodServDet.getQuantity());

            if(this.getQuantity()!=0)
            {
                Dictionary<String,float> quoteUnitDefn=new Dictionary<string,float>();
                quoteUnitDefn.Add("numbers", 1);
                quoteUnitDefn.Add("per unit",1);
                quoteUnitDefn.Add("per 10",1/10);
                quoteUnitDefn.Add("per 100",1/100);
                quoteUnitDefn.Add("per Dozen",1/12);
                                              
                float unit;
                quoteUnitDefn.TryGetValue(this.getUnitName(),out unit);
                
                this.setAmount(float.Parse(this.getQuote())*this.getQuantity()*unit);
            }*/
        }
    }
    /// <summary>
    /// An object of this class represents a complete response from a vendor for a RFQ.
    /// This contains a property 'RFQResponseQuoteRecordList' which actually an ArrayList of 'RFQResponseQuoteRecord' objects.
    /// 'RFQResponseQuoteRecord' is a subclass of 'RFQResponseQuotes' - each object of which represents one record of response for an RFQ/Lead for one particular
    /// product specification.
    /// Also, it contains the total amount for a particular response.
    /// </summary>
    public class RFQResponseQuoteTotal
    {
        private float totalAmnt;
        private ArrayList RFQResponseQuoteRecordList;

        public float getTotalAmnt()
        {
            return totalAmnt;
        }
        public void setTotalAmnt(float tAmnt)
        {
            totalAmnt=tAmnt;
        }
        public ArrayList getRFQResponseQuoteRecordList()
        {
            return RFQResponseQuoteRecordList;
        }
        public void setRFQResponseQuoteRecordList(ArrayList rfList)
        {
            RFQResponseQuoteRecordList=rfList;
        }
        /// <summary>
        /// Invoke this method to calculate the total amount of the respone.
        /// Invoke this method only after the property 'RFQResponseQuoteRecordList' is set.
        /// </summary>
        public void calculateTotalAmnt()
        {
            float total=0;
            for(int count=0;count<RFQResponseQuoteRecordList.Count;count++)
            {
                RFQResponseQuoteRecord tempRec=(RFQResponseQuoteRecord)RFQResponseQuoteRecordList[count];
                total+=tempRec.getAmount();
            }
            this.setTotalAmnt(total);
        }
    }
    /// <summary>
    /// This is a wrapper class which encapsulates the all underlying classes required to represent a lead.
    /// </summary>
    public class LeadRecord:RFQDetails
    {
        private String entityName;
        private RFQResponse LeadResp;

        public String getEntityName()
        {
            return this.entityName;
        }
        public void setEntityName(String eName)
        {
            this.entityName = eName;
        }
        //Get the respone object associated with the particular lead
        public RFQResponse getLeadResp()
        {
            return this.LeadResp;
        }
        public void setLeadResp(RFQResponse lResp)
        {
            this.LeadResp = lResp;
        }
        /// <summary>
        /// This method finds the entity name by certain business rule and set the property 'entityName'.
        /// Invoke this method only after setting all other properties of the 'LeadRecord' object.
        /// </summary>
        public Boolean findAndSetEntityName()
        {
            if (this.getRFQId() == null || this.getRFQId().Equals(""))
                throw (new CustomExceptions.invalidParamException("RFQId not set  -LeadRecord.findAndSetEntityName()"));
            if(this.getEntityId()==null ||this.getEntityId().Equals(""))
                throw (new CustomExceptions.invalidParamException("Entity id not set  -LeadRecord.findAndSetEntityName()"));
            if (this.getCreateMode() == null || this.getCreateMode().Equals(""))
                throw (new CustomExceptions.invalidParamException("Creation mode not set  -LeadRecord.findAndSetEntityName()"));

            if (this.getCreateMode().Equals(RFQDetails.CREATION_MODE_MANUAL))
            {
                this.setEntityName(Contacts.getContactDetailsforContactEntityDB(this.getCreatedEntity(), this.getEntityId()).getContactName());
            }
            else
            {
                this.setEntityName(MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(this.getEntityId()).getEntityName());
            }

            return true;
        }
    }
    /// <summary>
    /// This class is a subclass of 'RFQResponse' and contains the entity name property.
    /// A record of this class is generated when the response entity's name needs to be displayed along with other details.
    /// This is mainly useful in purchase screens to know the name details (along with other) of the reposne entities.
    /// </summary>
    public class RFQResponseRecord : RFQResponse
    {
        private String entityName;
        public String getEntityName()
        {
            return this.entityName;
        }
        public void setEntityName(String eName)
        {
            this.entityName = eName;
        }
        /// <summary>
        /// This method finds the entity name by certain business rule and set the property 'entityName'.
        /// Invoke this method only after setting all other properties of the 'RFQReponseRecord' object.
        /// </summary>
        public Boolean findAndSetEntityName()
        {
            if (this.getRFQId() == null || this.getRFQId().Equals(""))
                throw (new CustomExceptions.invalidParamException("RFQId not set  -LeadRecord.findAndSetEntityName()"));
            if (this.getRespEntityId() == null || this.getRespEntityId().Equals(""))
                throw (new CustomExceptions.invalidParamException("Entity id not set  -LeadRecord.findAndSetEntityName()"));
            if (!RFQDetails.getRFQDetailsbyIdDB(this.getRFQId()).getCreateMode().Equals(RFQDetails.CREATION_MODE_AUTO))
                throw (new CustomExceptions.businessRuleViolationException("RFQ Creation Mode must be Auto"));

            this.setEntityName(MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(this.getRespEntityId()).getEntityName());

            return true;
        }
    }
    public class ImageContextFactory
    {
        // There can be only one parent context
        private String parentContextName;
        private String parentContextValue;
        private Dictionary<String, String> childContextObjects;
        private String destinationContextName;
        
        public const String PARENT_CONTEXT_REQUIREMENT="requirement";
        public const String PARENT_CONTEXT_RFQ="rfq";
        public const String PARENT_CONTEXT_RFQ_RESPONSE = "rfqResponse";
        public const String PARENT_CONTEXT_PRODUCT = "product";
        public const String PARENET_CONTEXT_DEFECT = "defect";
        public const String PARENT_CONTEXT_NOTES = "notes";

        public const String CHILD_CONTEXT_FEAT_ID = "featid";
        public const String CHILD_CONTEXT_PRODCAT_ID = "prodcatid";
        public const String CHILD_CONTEXT_RFQ_RESPONSE_RESPONSE_ENTITY_ID = "rfqRespEntId";
        public const String CHILD_CONTEXT_PROD_ENT_ID = "CHILD_CONTEXT_PROD_ENT_ID";
        public const String CHILD_CONTEXT_PROD_FEAT_ID = "CHILD_CONTEXT_PROD_FEAT_ID";


        public const String DESTINATION_CONTEXT_NDA_FOR_PARENT_REQUIREMENT = "destinationNDARequirement";
        public const String DESTINATION_CONTEXT_NDA_FOR_PARENT_RFQ = "destinationNDARFQ";
        public const String DESTINATION_CONTEXT_FEAT_FOR_PARENT_REQUIREMENT = "destinationFeatRequirement";
        public const String DESTINATION_CONTEXT_FEAT_FOR_PARENT_RFQ = "destinationFeatRFQ";
        public const String DESTINATION_CONTEXT_NDA_FOR_PARENT_RFQ_RESPONSE = "rfqResponseNDA";
        public const String DESTINATION_CONTEXT_FEAT_FOR_PARENT_PRODUCT = "DESTINATION_CONTEXT_FEAT_FOR_PARENT_PRODUCT";
        public const String DESTINATION_CONTEXT_DOC_FOR_PARENT_DEFECT = "DESTINATION_CONTEXT_DOC_FOR_PARENT_DEFECT";
        public const String DESTINATION_CONTEXT_DOC_FOR_PARENT_NOTE = "DESTINATION_CONTEXT_DOC_FOR_PARENT_NOTE";
        public String getParentContextName()
        {
            return this.parentContextName;
        }
        public void setParentContextName(String cN)
        {
            this.parentContextName = cN;
        }
        public String getDestinationContextName()
        {
            return this.destinationContextName;
        }
        public void setDestinationContextName(String dC)
        {
            this.destinationContextName = dC;
        }
        public String getParentContextValue()
        {
            return this.parentContextValue;
        }
        public void setParentContextValue(String cV)
        {
            this.parentContextValue = cV;
        }
        public Dictionary<String, String> getChildContextObjects()
        {
            return this.childContextObjects;
        }
        public void setChildContextObjects(Dictionary<String, String> cO)
        {
            this.childContextObjects = cO;
        }
    }

    public class DateUtility
    {
        public Dictionary<int, String> monthList;

        public DateUtility()
        {
            monthList = new Dictionary<int, string>();

            monthList.Add(1, "Jan");
            monthList.Add(2, "Feb");
            monthList.Add(3, "Mar");
            monthList.Add(4, "Apr");
            monthList.Add(5, "May");
            monthList.Add(6, "Jun");
            monthList.Add(7, "Jul");
            monthList.Add(8, "Aug");
            monthList.Add(9, "Sep");
            monthList.Add(10, "Oct");
            monthList.Add(11, "Nov");
            monthList.Add(12, "Dec");
        }

        public String getConvertedDate(String inputDate)
        {
            String outputDate = "";
            if (inputDate != null && !inputDate.Equals("") && !(inputDate.IndexOf("1/1/1900") >= 0))
            {
                if (inputDate.Contains("/"))
                {
                    if (monthList.ContainsKey(Int32.Parse(inputDate.Substring(0, inputDate.IndexOf("/")))))
                    {
                        outputDate = monthList[Int32.Parse(inputDate.Substring(0, inputDate.IndexOf("/")))]; //Got the month            
                        outputDate += "-" + inputDate.Substring(inputDate.IndexOf("/") + 1, inputDate.LastIndexOf("/") - inputDate.IndexOf("/") - 1);//Got the day
                        outputDate += "-" + inputDate.Substring(inputDate.LastIndexOf("/") + 1);//Got the year
                    }
                }
                else//Contains "-"
                {
                    outputDate = inputDate;
                    /*outputDate =monthList[Int32.Parse(inputDate.Substring(inputDate.IndexOf("-") + 1, inputDate.LastIndexOf("-") - inputDate.IndexOf("-") - 1))]; //Got the month
                    outputDate += "-" + inputDate.Substring(inputDate.LastIndexOf("-") + 1,
                        inputDate.Substring(inputDate.LastIndexOf("-") + 1).IndexOf(" ")?inputDate.LastIndexOf(" ")-inputDate.LastIndexOf("-")-1:inputDate.Length);  //Got the day
                    outputDate+="-"+inputDate.Substring(0,4);//Got the year*/
                }
            }
            else if (inputDate.IndexOf("1/1/1900") >= 0)
                outputDate = "";
            return outputDate;
        }

        public String getConvertedDateWoTime(String inputDate)
        {
            String outputDate = "";
            if (inputDate != null && !inputDate.Equals("") && !(inputDate.IndexOf("1/1/1900") >= 0))
            {
                if (inputDate.Contains("/"))
                {
                    outputDate = monthList[Int32.Parse(inputDate.Substring(0, inputDate.IndexOf("/")))]; //Got the month            
                    outputDate += "-" + inputDate.Substring(inputDate.IndexOf("/") + 1, inputDate.LastIndexOf("/") - inputDate.IndexOf("/") - 1);//Got the day
                    outputDate += "-" + inputDate.Substring(inputDate.LastIndexOf("/") + 1);//Got the year
                }
                else//Contains "-"
                {
                    outputDate = inputDate;
                    /*outputDate =monthList[Int32.Parse(inputDate.Substring(inputDate.IndexOf("-") + 1, inputDate.LastIndexOf("-") - inputDate.IndexOf("-") - 1))]; //Got the month
                    outputDate += "-" + inputDate.Substring(inputDate.LastIndexOf("-") + 1,
                        inputDate.Substring(inputDate.LastIndexOf("-") + 1).IndexOf(" ")?inputDate.LastIndexOf(" ")-inputDate.LastIndexOf("-")-1:inputDate.Length);  //Got the day
                    outputDate+="-"+inputDate.Substring(0,4);//Got the year*/
                }
            }
            else if (inputDate != null && inputDate.IndexOf("1/1/1900") >= 0)
                outputDate = "";
            return (outputDate.IndexOf(" ")>0?outputDate.Substring(0,outputDate.IndexOf(" ")):outputDate);
        }

        /// <summary>
        /// For date values like 'Mar-18-2014' this method returns '2014-03-18'
        /// Expects the date format in this format as shown in the example
        /// </summary>
        /// <param name="inputDate"></param>
        /// <returns></returns>
        public String getDeConvertedDate(String inputDate)
        {
            String outputDate = "";
            Dictionary<String, String> monthNumberList = new Dictionary<string, string>();

            monthNumberList.Add("Jan", "01");
            monthNumberList.Add("Feb", "02");
            monthNumberList.Add("Mar", "03");
            monthNumberList.Add("Apr","04");
            monthNumberList.Add("May","05");
            monthNumberList.Add("Jun","06");
            monthNumberList.Add("Jul","07");
            monthNumberList.Add("Aug","08");
            monthNumberList.Add("Sep","09");
            monthNumberList.Add("Oct","10");
            monthNumberList.Add("Nov","11");
            monthNumberList.Add("Dec","12");
            if (!monthNumberList.ContainsKey(inputDate.Substring(0, 3)))
            {
                return inputDate;
            }
            else
            {
                if (inputDate != null && !inputDate.Equals("") && !(inputDate.IndexOf("1/1/1900") >= 0) && !(inputDate.IndexOf("Jan-01-1900") >= 0))
                {
                    String month = monthNumberList[inputDate.Substring(0, 3)];
                    String year = inputDate.Substring(inputDate.LastIndexOf("-") + 1);
                    String day = inputDate.Substring(inputDate.IndexOf("-") + 1, inputDate.LastIndexOf("-") - inputDate.IndexOf("-") - 1);
                    outputDate = year + "-" + month + "-" + day;
                }
                else
                    outputDate = "";

                return outputDate;
            }
           
        }
    }
    /// <summary>
    /// Use this class to set/get session variables names and follow the types mentioned to set/get objects/Strings accordingly.
    /// </summary>
    public class SessionFactory
    {
        /// <summary>
        /// This variable holds the access list dictionary details of the logged in user
        /// </summary>
        public static String ACCESSLIST_FOR_USER = "ACCESSLIST_FOR_USER";
        /// <summary>
        /// This variable holds the currency names along with the ids in a dictionary
        /// </summary>
        public static String CURRENCY_LIST = "CURRENCY_LIST";
        /// <summary>
        /// The defult currency of the main business entity
        /// </summary>
        public static String MAIN_BUSINESS_ENTITY_DEFAULT_CURRENCY = "MAIN_BUSINESS_ENTITY_DEFAULT_CURRENCY";
        /// <summary>
        /// main business entity id string
        /// </summary>
        public static String MAIN_BUSINESS_ENTITY_ID_STRING = "mainBusinessEntityId";
        /// <summary>
        /// This variable holds the logged in user id string
        /// </summary>
        public static String LOGGED_IN_USER_ID_STRING = "LOGGED_IN_USER_ID_STRING";
        /// <summary>
        /// This variable holds the theme name of the logged in user
        /// </summary>
        public static String LOGGED_IN_USER_THEME = "LOGGED_IN_USER_THEME";
        /// <summary>
        /// main business entity object
        /// </summary>
        public static String MAIN_BUSINESS_ENTITY_OBJECT = "mainBusinessEntityObject";
        /// <summary>
        /// Sub business entity id string
        /// </summary>
        public static String SUB_BUSINESS_ENTITY_ID_STRING = "subBusinessEntityId";
        /// <summary>
        /// Indicates that the user has completed the short registration process
        /// </summary>
        public static String SHORT_REGISTR_COMPLETE = "shortRegistrationComplete";
        /// <summary>
        /// This session variable for holding the selected product category from Create Requirement screen
        /// </summary>
        public static String CREATE_REQR_SELECTED_PRODUCT_CAT = "createReqrSelectedProdCat";
        /// <summary>
        /// This variable is used to hold the selected product category in requirement specification screen's addition section
        /// </summary>
        public static String UPDATE_REQR_SELECTED_PRODUCT_CAT = "UPDATE_REQR_SELECTED_PRODUCT_CAT";

        public static String UPDATE_RFQ_SELECTED_PRODUCT_CAT = "UPDATE_RFQ_SELECTED_PRODUCT_CAT";

        public static String UPDATE_POTN_SELECTED_PRODUCT_CAT = "UPDATE_POTN_SELECTED_PRODUCT_CAT";

        public static String UPDATE_LEAD_SELECTED_PRODUCT_CAT = "UPDATE_LEAD_SELECTED_PRODUCT_CAT";
        /// <summary>
        /// This variable is to hold the Requirement specification map object's arrayList as selected in the create requirement screen
        /// </summary>
        public static String CREATE_REQ_SELECTED_REQR_SPEC_MAP = "createReqSelectedReqrSpecMap";
        /// <summary>
        /// holds the specification arraylist in requirement specification screen's addition section
        /// </summary>
        public static String UPDATE_REQ_SELECTED_REQR_SPEC_MAP = "UPDATE_REQ_SELECTED_REQR_SPEC_MAP";

        public static String UPDATE_RFQ_SELECTED_RFQ_SPEC_MAP = "UPDATE_RFQ_SELECTED_RFQ_SPEC_MAP";

        public static String UPDATE_LEAD_SELECTED_LEAD_SPEC_MAP = "UPDATE_LEAD_SELECTED_LEAD_SPEC_MAP";

        public static String UPDATE_POTN_SELECTED_POTN_SPEC_MAP = "UPDATE_POTN_SELECTED_POTN_SPEC_MAP";
        /// <summary>
        /// This session variable for holding the selected product category from Create RFQ screen
        /// </summary>
        public static String CREATE_RFQ_SELECTED_PRODUCT_CAT = "createRfqrSelectedProdCat";
        /// <summary>
        /// This variable is to hold the rfq specification map object's arrayList as selected in the create RFQ screen
        /// </summary>
        public static String CREATE_RFQ_SELECTED_RFQ_SPEC_MAP = "createRfqSelectedRfqSpecMap";
        /// <summary>
        /// This variable is to hold the rfq id which is created from the create RFQ screen
        /// </summary>
        public static String CREATE_RFQ_RFQ_ID = "createRFQ_RFQid";
        /// <summary>
        /// This variable is to hold the arraylist of objects of type 'RFQProdServQnty' during the process of RFQ creation
        /// </summary>
        public static String CREATE_RFQ_PROD_SRV_QNTY_LIST = "createRFQProdSrvQntyList";
        /// <summary>
        /// Holds the arraylist of product service quantity details in RFQ specification screens addition section
        /// </summary>
        public static String UPDATE_RFQ_PROD_SRV_QNTY_LIST = "UPDATE_RFQ_PROD_SRV_QNTY_LIST";
        /// <summary>
        /// Holds the arraylist of product service quantity details in requirement specification screens addition section
        /// </summary>
        public static String UPDATE_REQ_PROD_SRV_QNTY_LIST = "UPDATE_REQ_PROD_SRV_QNTY_LIST";
        /// <summary>
        /// This variable is to hold the requirement id which is created from the create Requirement screen
        /// </summary>
        public static String CREATE_REQ_REQR_ID = "createREQ_ReqId";
        /// <summary>
        /// This variable is to hold the uploaded NDA document while creating RFQ.
        /// </summary>
        public static String CREATE_RFQ_NDA_FILE = "createRFQNDAFile";
        /// <summary>
        /// This variable is to hold the arraylist of objects of type 'RequirementProdServQnty' during the process of Requirement creation.
        /// </summary>
        public static String CREATE_REQ_PROD_SRV_QNTY_LIST = "createREQProdSrvQntyList";
        /// <summary>
        /// This session variable for holding the selected product category from Create Lead screen
        /// </summary>
        public static String CREATE_LEAD_SELECTED_PRODUCT_CAT = "createLeadSelectedProdCat";
        /// <summary>
        /// This variable is to hold Tthe rfq specification map object's arrayList as selected in the create Lead screen
        /// </summary>
        public static String CREATE_LEAD_SELECTED_RFQ_SPEC_MAP = "createLeadSelectedRfqSpecMap";
        /// <summary>
        /// This variable is to hold the rfq id which is created from the create Lead screen
        /// </summary>
        public static String CREATE_LEAD_RFQ_ID = "createLead_RFQid";
        /// <summary>
        /// This variable is to hold the arraylist of objects of type 'RFQProdServQnty' during the process of Lead creation
        /// </summary>
        public static String CREATE_LEAD_PROD_SRV_QNTY_LIST = "createLeadProdSrvQntyList";
        /// <summary>
        /// This variable is to hold the uploaded NDA document while creating Lead.
        /// </summary>
        public static String CREATE_LEAD_NDA_FILE = "createLeadNDAFile";
        /// <summary>
        /// This stores the response quote unit value in the create lead page
        /// </summary>
        public static String CREATE_LEAD_QUOTE_UNIT = "createLeadQuoteUnit";
        /// <summary>
        /// This variable stores multiple RFQResponseQuote objects while creating lead from the create lead screen
        /// </summary>
        public static String CREATE_LEAD_RESP_QUOTE_LIST = "createLeadRespQuoteList";
        /// <summary>
        /// This variable holds the locality id of the in-site business entity which was searched
        /// </summary>
        public static String CREATE_CONTACT_LOCALITY_ID="createContactLocalityId";
        /// <summary>
        /// This variable holds the address line1 value of the in-site business entity which was searched
        /// </summary>
        public static String CREATE_CONTACT_ADDRESS_LINE1 = "createContactAddressLine1";
        /// <summary>
        /// This variable holds the contact object created from the create contact screen
        /// </summary>
        public static String CREATE_CONTACT_CONTACT_OBJ = "createContactObj";
        /// <summary>
        /// This variable holds the datatable showing all contacts from the site in the create contact screen
        /// </summary>
        public static String CREATE_CONTACT_DATA_GRID = "CREATE_CONTACT_DATA_GRID";
        /// <summary>
        /// This variable stores the selected requirement id in the all requirement section of the purchase screen
        /// </summary>
        public static String ALL_PURCHASE_ALL_REQUIREMENT_SELECTED_REQR_ID = "selectedreqrdid";
        /// <summary>
        /// This variable holds the location id of the selected' rows location in all requirement panel of the purchase screen
        /// </summary>
        public static String ALL_PURCHASE_ALL_REQUIREMENT_LOCATION = "allprchallreqrlocl";
        /// <summary>
        /// This variable stores the arraylist of selected requirement's requirement sepcification objects in all requirement panel of the purchase screen.
        /// Each element of this arraylist of the type 'Requirement_Spec'.
        /// </summary>
        public static String ALL_PURCHASE_ALL_REQUIREMENT_REQR_SPEC = "allprchallreqrspec";
        /// <summary>
        /// While editing the requirement specification from purchase screen this variable holds the currently edited inner grids data
        /// </summary>
        public static String ALL_PURCHASE_ALL_REQUIREMENT_REQR_SPEC_EDIT_INNER_GRID = "ALL_PURCHASE_ALL_REQUIREMENT_REQR_SPEC_EDIT_INNER_GRID";
        /// <summary>
        /// This variable stores the arraylist of selected requirement's product service quantity objects in all requirement panel of the purchase screen.
        /// Each element of this arraylist of the type 'RequirementProdServQnty'.
        /// </summary>
        public static String ALL_PURCHASE_ALL_REQUIREMENT_REQR_PROD_SRV = "allprchallreqrprodqnty";
        /// <summary>
        /// This variable stores the datatable to be bound to the all requiment gridview. This reduces the gridview page index change time.
        /// </summary>
        public static String ALL_PURCHASE_ALL_REQUIREMENT_GRID_DATA = "allprchallreqrgriddata";
        /// <summary>
        /// Stores the tagged RFQ grid for a particular selected requirement when displayed in the tag RFQ screen
        /// </summary>
        public static String ALL_PURCHASE_ALL_REQUIREMENT_TAGGED_RFQ_GRID = "ALL_PURCHASE_ALL_REQUIREMENT_TAGGED_RFQ_GRID";
        /// <summary>
        /// This variable hods the datasource objet for the inner grid available in the show specificiation grid
        /// </summary>
        public static String ALL_PURCHASE_ALL_REQUIREMENT_SHOW_SPEC_INNER_GRID_DATA = "showspecinnergridData";
        /// <summary>
        /// This variable holds the datagrid for the specification of the of the requirement
        /// </summary>
        public static String ALL_PURCHASE_ALL_REQUIREMENT_SPEC_DATA_GRID = "ALL_PURCHASE_ALL_REQUIREMENT_SPEC_DATA_GRID";
        /// <summary>
        /// This variable holds the selected product category id in 'AllRequirement_Specification' screen
        /// </summary>
        public static String ALL_PURCHASE_ALL_REQUIREMENT_SHOW_FEAT_SELECTED_PRODCAT = "selectProdCat";
        /// <summary>
        /// This variable stores the selected RFQ id from the RFQ details grid of the purchase screen
        /// </summary>
        public static String ALL_PURCHASE_ALL_RFQ_SELECTED_RFQ_ID = "selectedrfq";

        public static String ALL_PURCHASE_ALL_RFQ_ALL_QUOTES_GRID = "ALL_PURCHASE_ALL_RFQ_ALL_QUOTES_GRID";
        /// <summary>
        /// This variable holds the location id of the selected row in the all RFQ gridview in the all purchase screen
        /// </summary>
        public static String ALL_PURCHASE_ALL_RFQ_LOCATION = "allpurchaserfqlocation";
        /// <summary>
        /// This variable holds the arraylist containing specificaion details for a RFQ.
        /// Each element of this arraylist is an object of type 'RFQProductServiceDetails'
        /// </summary>
        public static String ALL_PURCHASE_ALL_RFQ_RFQ_SPEC = "allpurchaserfqspec";
        /// <summary>
        /// In RFQ specification screen this variable holds the inner grid which is currently being edited
        /// </summary>
        public static String ALL_PURCHASE_ALL_RFQ_RFQ_SPEC_EDIT_INNER_GRID = "ALL_PURCHASE_ALL_RFQ_RFQ_SPEC_EDIT_INNER_GRID";
        /// <summary>
        /// This variable is used to hold an arrylist.
        /// Each element of this arraylist is an object of type 'RFQProdServQnty'.
        /// </summary>
        public static String ALL_PURCHASE_ALL_RFQ_PROD_QNTY = "allpurchaserfqprdqnty";
        /// <summary>
        /// This variable holds the contact object for the selected contact in the broadcast list
        /// </summary>
        public static String ALL_PURCHASE_ALL_RFQ_BROADCASTLIST_SELECTED_CONTACT = "selectedContactRFQBrd";
        /// <summary>
        /// In the allRFQShortlisted screen this variable holds the finalized potential id
        /// </summary>
        public static String ALL_PURCHASE_ALL_RFQ_SHORLISTED_FINALIZED_POTEN = "ALL_PURCHASE_ALL_RFQ_SHORLISTED_FINALIZED_POTEN";
        /// <summary>
        /// This variable is used to hold the RFQ grid data in the purchase screen
        /// </summary>
        public static String ALL_PURCHASE_ALL_RFQ_GRID_DATA = "allPurchaseAllRFQ";

        public static String ALL_PURCHASE_ALL_PO_GRID_DATA = "ALL_PURCHASE_ALL_PO_GRID_DATA";
        /// <summary>
        /// Holds all the rfqlist of potential records in the sales screen
        /// </summary>
        public static String ALL_SALE_ALL_POTN_RFQ_LIST = "ALL_SALE_ALL_POTN_RFQ_LIST";

        public static String ALL_SALE_ALL_SO_GRID_DATA = "ALL_SALE_ALL_SO_GRID_DATA";
        /// <summary>
        /// This variable holds the response entity id of the selected row from the quote details pop up from RFQ details subsection in purchase screen
        /// </summary>
        public static String ALL_PURCHASE_ALL_RFQ_SHOW_QUOTE_SELECTED_RESP_ENTITY_ID = "selectedRespEntId";
        /// <summary>
        /// This variable holds the datatable containing the details of a particular reponse quotes from a particular response entity id
        /// </summary>
        public static String ALL_PURCHASE_ALL_RFQ_RESP_QUOTE_GRID_DATA = "respQuoteGrid";
        /// <summary>
        /// This variable holds the datagrid from the all rfq speicifcation gridview
        /// </summary>
        public static String ALL_PURCHASE_ALL_RFQ_SPECIFICATION_GRID_DATA = "ALL_PURCHASE_ALL_RFQ_SPECIFICATION_GRID_DATA";
        /// <summary>
        /// This variable holds the dataTable containting the product grid details in the PO creation screen in Finalize Deal section of the purchase screen
        /// </summary>
        public static String ALL_PURCHASE_FINALIZE_DEAL_PO_PROD_GRID = "ALL_PURCHASE_FINALIZE_DEAL_PO_PROD_GRID";
        /// <summary>
        /// This variable holds the specification details for each prod category in the purchase order grid in Finalize Deal section of the purchase screen
        /// </summary>
        public static String ALL_PURCHASE_FINALIZE_DEAL_PO_PROD_GRID_SPEC = "ALL_PURCHASE_FINALIZE_DEAL_PO_PROD_GRID_SPEC";
        /// <summary>
        /// This variable holds the selected purchase order id in the purchase screen all_RFQ section
        /// </summary>
        public static String ALL_PURCHASE_ALL_RFQ_SELECTED_PO = "ALL_PURCHASE_ALL_RFQ_SELECTED_PO";
        /// <summary>
        /// This variable holds the complete invoice data grid in all purchase screen
        /// </summary>
        public static String ALL_PURCHASE_ALL_INV_GRID_DATA = "ALL_PURCHASE_ALL_INV_GRID_DATA";
        /// <summary>
        /// This variable holds the arraylist containing the specificaion details of an invoice. Each element of this arraylist is an object of type 'RFQProductServiceDetails'.
        /// </summary>
        public static String ALL_PURCHASE_ALL_INV_SELECTED_RFQ_SPEC = "ALL_PURCHASE_ALL_INV_SELECTED_RFQ_SPEC";
        /// <summary>
        /// This variable holds the payment grid from all invoice section from within Purchase/Sales Screen
        /// </summary>
        public static String ALL_INVOICE_PAYMENT_GRID = "ALL_INVOICE_PAYMENT_GRID";
        /// <summary>
        /// This variable holds the existing transaction number dictionary in the payment screen of invoice grid
        /// </summary>
        public static String ALL_INVOICE_PAYMENT_GRID_EXISTING_TRAN_NO = "ALL_INVOICE_PAYMENT_GRID_EXISTING_TRAN_NO";
        /// <summary>
        /// While creating an invoice manually this variable holds the selected product specification details
        /// </summary>
        public static String CREATE_INVOICE_MANUAL_SELECTED_SPEC_MAP = "CREATE_INVOICE_MANUAL_SELECTED_SPEC_MAP";
        /// <summary>
        /// This dictionary holds the 'product_Name','CREATE_INVOICE_MANUAL_SELECTED_SPEC_MAP' - arrayList
        /// </summary>
        public static String CREATE_INVOICE_MANUAL_SELECTED_SPEC_MAP_DICT = "CREATE_INVOICE_MANUAL_SELECTED_SPEC_MAP_DICT";
        /// <summary>
        /// While creating an invoice manually this variable holds the selected product/service category
        /// </summary>
        public static String CREATE_INVOICE_MANUAL_SELECTED_PRODUCT_CAT = "CREATE_INVOICE_MANUAL_SELECTED_PRODUCT_CAT";
        /// <summary>
        /// While creating an invoice this variable holds the RFQ id
        /// </summary>
        public static String CREATE_INVOICE_MANUAL_RFQ_ID = "CREATE_INVOICE_MANUAL_RFQ_ID";
        /// <summary>
        /// While creating invoice manually this variable holds the invoice specific tax component details
        /// </summary>
        public static String CREATE_INVOICE_MANUAL_TAX_COMP_DATA_GRID = "CREATE_INVOICE_MANUAL_TAX_COMP_DATA_GRID";
        /// <summary>
        /// While creating invoice manually this variable holds the total tax percentage selected
        /// </summary>
        public static String CREATE_INVOICE_MANUAL_TOTAL_TAX_COMP_PERC = "CREATE_INVOICE_MANUAL_TOTAL_TAX_COMP_PERC";
        /// <summary>
        /// In the manully create invoice screen this variable holds the total tax component grid of the org
        /// </summary>
        public static String CREATE_INVOICE_MANUAL_TOTAL_TAX_COMP_GRID = "CREATE_INVOICE_MANUAL_TOTAL_TAX_COMP_GRID";
        /// <summary>
        /// This variable holds the product grid details in the create invoice page while creating invoice manually
        /// </summary>
        public static String CREATE_INVOICE_MANUAL_PRODUCT_GRID = "CREATE_INVOICE_MANUAL_PRODUCT_GRID";
        /// <summary>
        /// This variable holds the product/service list of the organization while creating invoice manually
        /// </summary>
        public static String CREATE_INVOICE_MANUAL_PRODUCT_GRID_PRODUCT_LIST = "CREATE_INVOICE_MANUAL_PRODUCT_GRID_PRODUCT_LIST";

        public static String CREATE_PO_MANUAL_PRODUCT_GRID_PRODUCT_LIST = "CREATE_PO_MANUAL_PRODUCT_GRID_PRODUCT_LIST";

        public static String CREATE_PO_MANUAL_SELECTED_PRODUCT_CAT = "CREATE_PO_MANUAL_SELECTED_PRODUCT_CAT";

        public static String CREATE_PO_MANUAL_SELECTED_SPEC_MAP = "CREATE_PO_MANUAL_SELECTED_SPEC_MAP";

        public static String CREATE_PO_MANUAL_PRODUCT_GRID = "CREATE_PO_MANUAL_PRODUCT_GRID";

        public static String CREATE_PO_MANUAL_SELECTED_SPEC_MAP_DICT = "CREATE_PO_MANUAL_SELECTED_SPEC_MAP_DICT";
        /// <summary>
        /// This variable holds the product/service list of the organization while updating PO manually
        /// </summary>
        public static String UPDATE_PO_MANUAL_PRODUCT_GRID_PRODUCT_LIST = "UPDATE_PO_MANUAL_PRODUCT_GRID_PRODUCT_LIST";

        public static String UPDATE_PO_MANUAL_SELECTED_PRODUCT_CAT = "UPDATE_PO_MANUAL_SELECTED_PRODUCT_CAT";

        public static String UPDATE_PO_MANUAL_SELECTED_SPEC_MAP = "UPDATE_PO_MANUAL_SELECTED_SPEC_MAP";

        public static String UPDATE_PO_MANUAL_PRODUCT_GRID = "UPDATE_PO_MANUAL_PRODUCT_GRID";

        public static String UPDATE_PO_MANUAL_SELECTED_SPEC_MAP_DICT = "UPDATE_PO_MANUAL_SELECTED_SPEC_MAP_DICT";
        /// <summary>
        /// This variable stores the 'ImageContextFactory' object.
        /// </summary>
        public static String DISP_IMAGE_CONTEXT_FACTORY_OBJ = "imagecontext";
        /// <summary>
        /// This variable stores the selected lead id in the all lead section of the sales screen
        /// </summary>
        public static String ALL_SALE_ALL_LEAD_SELECTED_LEAD_ID = "selectedLeadId";
        /// <summary>
        /// This variable holds a session variable containing dictionary of dictionary containing invoice details for potential where there are multiple invoices attached
        /// </summary>
        public static String ALL_SALE_ALL_POTN_MUTLTIPLE_INV_DATA_FOR_RFQ = "ALL_SALE_ALL_POTN_MUTLTIPLE_INV_DATA_FOR_RFQ";
        /// <summary>
        /// This variable stores the location id of the selected lead in the sales screen
        /// </summary>
        public static String ALL_SALE_ALL_LEAD_LOCATION = "selectedlocationId";
        /// <summary>
        /// This variable holds the arraylist containing the lead specification objects.
        /// </summary>
        public static String ALL_SALE_ALL_LEAD_LD_SPEC = "leadreqrSpec";
        /// <summary>
        /// This variable holds the arraylist containing the lead product service details obje cts
        /// </summary>
        public static String ALL_SALE_ALL_LEAD_LD_PROD_SRV = "leadProdSrv";
        /// <summary>
        /// Once generated this variable holds the datatable containing the product service details of the selected lead in sales screen's specification window
        /// </summary>
        public static String ALL_SALE_ALL_LEAD_LD_PROD_SRV_DATATABLE = "leadProdSrvDataTable";
        /// <summary>
        /// This variable holds the Dictionary containing the selected the customer details object from the all lead section of the sales screen
        /// </summary>
        public static String ALL_SALE_ALL_LEAD_SELECTED_CUSTOMER_OBJ = "leadSelectedCustomer";
        /// <summary>
        /// This variable holds the lead grid dataTable for the lead fram in the sales screen
        /// </summary>
        public static String ALL_SALE_ALL_LEAD_GRID_DATA = "ALL_SALE_ALL_LEAD_GRID_DATA";
/// <summary>
/// Holds the specification inner grid from lead specification screen of the inner grid is being edited
/// </summary>
        public static String ALL_SALE_ALL_LEAD_LEAD_SPEC_EDIT_INNER_GRID = "ALL_SALE_ALL_LEAD_LEAD_SPEC_EDIT_INNER_GRID";
/// <summary>
/// This variable holds the product list to be displayed in the edit mode from specification screen of a lead
/// </summary>
        public static String ALL_SALE_ALL_LEAD_SPECIFICATION_PRODUCT_LIST = "ALL_SALE_ALL_LEAD_SPECIFICATION_PRODUCT_LIST";
        /// <summary>
        /// This variable holds the product list to be displayed in the edit mode from specification screen of a potential
        /// </summary>
        public static String ALL_SALE_ALL_POTENTIAL_SPECIFICATION_PRODUCT_LIST = "ALL_SALE_ALL_POTENTIAL_SPECIFICATION_PRODUCT_LIST";
        /// <summary>
        /// This variables stores the selected RFQ from the all sale page's all potential  grid's selected row
        /// </summary>
        public static String ALL_SALE_ALL_POTENTIAL_SELECTED_RFQ_ID = "allPotSelectedRFQ";
        /// <summary>
        /// This variable holds the selected potential id from the all potential gridview of the all sale screen
        /// </summary>
        public static String ALL_SALE_ALL_POTENTIAL_SELECTED_POTN_ID = "allPotSelectedPotId";
        /// <summary>
        /// This variable holds the Dictionary containing the customer data from the selected row of the all potential grid of the all sale screen
        /// </summary>
        public static String ALL_SALE_ALL_POTENTIAL_SELECTED_CUSTOMER_OBJ = "allPotnSelectCustObj";
        /// <summary>
        /// This variable holds the location id of the selected record in the all potential section in the all sale page
        /// </summary>
        public static String ALL_SALE_ALL_POTENTIAL_LOCATION = "allPotnLocation";
        /// <summary>
        /// This variable holds the arrayList containing the specification object of the selected potential from the all potential section in the all sale screen
        /// </summary>
        public static String ALL_SALE_ALL_POTENTIAL_POTN_SPEC = "allPotnSpec";
        /// <summary>
        /// This variable holds teh arrayList containing the product service details of the selected potential from the all potential section in the all sale screen
        /// </summary>
        public static String ALL_SALE_ALL_POTENTIAL_POTN_PROD_SRV = "allPotnProdSrv";
        /// <summary>
        /// Holds the specification inner grid from potential specification screen of the inner grid is being edited
        /// </summary>
        public static String ALL_SALE_ALL_POTENTIAL_POTN_SPEC_EDIT_INNER_GRID = "ALL_SALE_ALL_POTENTIAL_POTN_SPEC_EDIT_INNER_GRID";
        /// <summary>
        /// This variable holds the potential grid data in the all sale screen
        /// </summary>
        public static String ALL_SALE_ALL_POTENTIAL_GRID_DATA = "allPotenGridData";
        /// <summary>
        /// Holds the datatable containing the specification details of a selected potential from the all sale screen's potential section
        /// </summary>
        public static String ALL_SALE_ALL_POTENTIAL_POTN_PROD_SRV_DATATABLE = "potPrdSrvDataTable";
        /// <summary>
        /// While creating invoice from all potential section of all sale screen, this variable holds the data grid for tax components
        /// </summary>
        public static String ALL_SALE_ALL_POTENTIAL_CREATE_INV_TAX_COMP_DATA_GRID = "ALL_SALE_ALL_POTENTIAL_CREATE_INV_TAX_COMP_DATA_GRID";
        /// <summary>
        /// This variable holds the total % value of all tax components combined for the invoice
        /// </summary>
        public static String ALL_SALE_ALL_POTENTIAL_CREATE_INV_TOTAL_TAX_COMP_PERC = "ALL_SALE_ALL_POTENTIAL_CREATE_INV_TOTAL_TAX_COMP_PERC";
        /// <summary>
        /// This variable holds the complete tax component grid in the invoice creation/update screen which is shown from all sales page
        /// </summary>
        public static String ALL_SALE_ALL_POTENTIAL_CREATE_INV_TOTAL_TAX_COMP_GRID = "ALL_SALE_ALL_POTENTIAL_CREATE_INV_TOTAL_TAX_COMP_GRID";
        /// <summary>
        /// This variable holds the complete invoice data grid in all sales screen
        /// </summary>
        public static String ALL_SALE_ALL_INV_GRID_DATA = "ALL_SALE_ALL_INV_GRID_DATA";
        /// <summary>
        /// Stores a dictionary containing the existing inv-no,inv-id as key value pair
        /// </summary>
        public static String ALL_SALE_ALL_INV_EXISTING_INV_NO = "ALL_SALE_ALL_INV_EXISTING_INV_NO";
        /// <summary>
        /// This variable holds the arraylist containing the specificaion details of an invoice (from Invoice details section in sales page). 
        /// Each element of this arraylist is an object of type 'RFQProductServiceDetails'.
        /// </summary>
        public static String ALL_SALE_ALL_INV_SELECTED_RFQ_SPEC="ALL_SALE_ALL_INV_SELECTED_RFQ_SPEC";
        /// <summary>
        /// This variable holds the selected product category in create potential screen
        /// </summary>
        public static String CREATE_POTENTIAL_SELECTED_PRODUCT_CAT = "createPotSelcProdCat";
        /// <summary>
        /// This variable holds the selected prod spec ArrayList as selected in the create potential specification gridview
        /// </summary>
        public static String CREATE_POTENTIAL_SELECTED_POTN_SPEC_MAP = "createPotSelcPotnSpecMap";
        /// <summary>
        /// This variable holds the arraylist containing the 'RFQProdServQnty' objects.
        /// </summary>
        public static String CREATE_POTENTIAL_PROD_SRV_QNTY_LIST = "createPotentialProdSrvQntyList";
        /// <summary>
        /// This variable stores the file upload control holding the NDA file while creating the potential
        /// </summary>
        public static String CREATE_POTENTIAL_NDA_FILE = "createPotentialNDA";
        /// <summary>
        /// This variable holds the RFQ id in create potential screen
        /// </summary>
        public static String CREATE_POTENTIAL_RFQ_ID = "createPotentialRFQId";
        /// <summary>
        /// This variable holds the potential id of the newly created potential in the create potential screen
        /// </summary>
        public static String CREATE_POTENTIAL_POT_ID = "createPotentialPotId";
        /// <summary>
        /// This variable holds the ArrayList containing the response quote details while creating a potential
        /// </summary>
        public static String CREATE_POTENTIAL_RESP_QUOTE_LIST = "createPotentialRespQuoteList";
        /// <summary>
        /// While manually creating a potential this variable holds the average potential amount 
        /// </summary>
        public static String CREATE_POTENTIAL_POTN_AMNT = "createPotentialAmnt";
        /// <summary>
        /// This variable holds the confirmation of Material document associated with the potential
        /// </summary>
        public static String CREATE_POTENTIAL_CONF_MAT = "createPotConfMat";
        /// <summary>
        /// This variable holds the selected product category in the create product page
        /// </summary>
        public static String CREATE_PRODUCT_SELECTED_PRODUCT_CAT = "createProdselectedProdCat";

        public static String CREATE_PRODUCT_EXISTING_PROD_NAME_LIST = "CREATE_PRODUCT_EXISTING_PROD_NAME_LIST";

        public static String UPDATE_PRODUCT_SELECTED_PRODUCT_CAT = "UPDATE_PRODUCT_SELECTED_PRODUCT_CAT";

        public static String UPDATE_PROD_SELECTED_PROD_SPEC_MAP = "UPDATE_PROD_SELECTED_PROD_SPEC_MAP";

        public static String PRODUCT_SPECIFICATION_EXISTING_FEAT_LIST = "PRODUCT_SPECIFICATION_EXISTING_FEAT_LIST";

        public static String RFQ_SPECIFICATION_EXISTING_PROD_LIST = "RFQ_SPECIFICATION_EXISTING_PROD_LIST";

        public static String REQ_SPECIFICATION_EXISTING_PROD_LIST = "REQ_SPECIFICATION_EXISTING_PROD_LIST";

        public static String LEAD_SPECIFICATION_EXISTING_PROD_LIST = "LEAD_SPECIFICATION_EXISTING_PROD_LIST";

        public static String POTN_SPECIFICATION_EXISTING_PROD_LIST = "POTN_SPECIFICATION_EXISTING_PROD_LIST";
        /// <summary>
        /// This variable holds the 'ShopChildProdsSpecs' objects arraylist while creating a new product from create product page
        /// </summary>
        public static String CREATE_PRODUCT_CHILD_PROD_SPEC_MAP = "CREATE_PRODUCT_CHILD_PROD_SPEC_MAP";
        /// <summary>
        /// This variable holds the data grid containing the specification object details of any selected product in the from all Products page.
        /// </summary>
        public static String ALL_PROD_SPECIFICATION_DATAGRID = "ALL_PROD_SPECIFICATION_DATAGRID";
        /// <summary>
        /// This variable holds the selected product name from the all products page data grid
        /// </summary>
        public static String ALL_PRODUCT_SELECTED_PRODUCT_NAME = "ALL_PRODUCT_SELECTED_PRODUCT_NAME";
        /// <summary>
        /// This variable holds the datatable contanining the values for the all products grid.
        /// </summary>
        public static String ALL_PRODUCT_PROD_DATA_GRID = "ALL_PRODUCT_PROD_DATA_GRID";
        /// <summary>
        /// This variable holds the selected product cateogry to filter in the all products page.
        /// </summary>
        public static String ALL_PRODUCT_SELECTED_PROD_CAT_FILTER = "ALL_PRODUCT_SELECTED_PROD_CAT_FILTER";
        /// <summary>
        /// This variable holds a dictionary of existing product names to check for duplicates while creating new products
        /// </summary>
        public static String ALL_PRODUCT_CREATE_PRODUCT_EXISTING_NAMES = "ALL_PRODUCT_CREATE_PRODUCT_EXISTING_NAMES";
        /// <summary>
        /// This variable holds the data grid generated for the chain management screen from the admin preference screen
        /// </summary>
        public static String ADMIN_PREF_CHAIN_MGMT_DATA_GRID = "ADMIN_PREF_CHAIN_MGMT_DATA_GRID";
        /// <summary>
        /// Holds the tax component data grid in the admin pref->doc format mgmt->invoice section.
        /// </summary>
        public static String ADMIN_PREF_DOCFORMAT_MGMT_TAX_COMP_GRID = "ADMIN_PREF_DOCFORMAT_MGMT_TAX_COMP_GRID";
        /// <summary>
        /// Holds the basic user details data grid in the admin pref->user mgmt
        /// </summary>
        public static String ADMIN_PREF_USER_MGMT_BASIC_USER_DET_GRID = "ADMIN_PREF_USER_MGMT_BASIC_USER_DET_GRID";
        /// <summary>
        /// This variable holds the data grid in the all contacts screen
        /// </summary>
        public static String ALL_CONTACT_DATA_GRID = "ALL_CONTACT_DATA_GRID";
        /// <summary>
        /// Stores the contact Entity id of the selcted contact from the contact data grid
        /// </summary>
        public static String ALL_CONTACT_SELECTED_CONTACT_ID = "ALL_CONTACT_SELECTED_CONTACT_ID";
        /// <summary>
        /// For a given contact it hods the datagrid for all incoming invoice
        /// </summary>
        public static String ALL_CONTACT_ALL_DEAL_INCOMING_INV_GRID = "ALL_CONTACT_ALL_DEAL_INCOMING_INV_GRID";
        /// <summary>
        /// Holds the arraylist of invoice objects for incoming invoices from contact's deal screen
        /// </summary>
        public static String ALL_CONTACT_ALL_DEAL_INCOMING_INV_ARRAYLIST = "ALL_CONTACT_ALL_DEAL_INCOMING_INV_ARRAYLIST";
        /// <summary>
        /// For a given contact it hods the datagrid for all outgoing invoice
        /// </summary>
        public static String ALL_CONTACT_ALL_DEAL_OUTGOING_INV_GRID = "ALL_CONTACT_ALL_DEAL_OUTGOING_INV_GRID";
        /// <summary>
        /// Holds the arraylist of invoice objects for outgoing invoices from contact's deal screen
        /// </summary>
        public static String ALL_CONTACT_ALL_DEAL_OUTGOING_INV_ARRAYLIST = "ALL_CONTACT_ALL_DEAL_OUTGOING_INV_ARRAYLIST";
        /// <summary>
        ///  For a given contact it hods the datagrid for all incoming defects
        /// </summary>
        public static String ALL_CONTACT_ALL_DEFECT_INCOMING_DEF_GRID = "ALL_CONTACT_ALL_DEFECT_INCOMING_DEF_GRID";
        /// <summary>
        /// Holds the dictionary of defect objects for incoming defects from contact's defect screen
        /// </summary>
        public static String ALL_CONTACT_ALL_DEFECT_INCOMING_DEF_DICTIONARY = "ALL_CONTACT_ALL_DEFECT_INCOMING_DEF_DICTIONARY";
        /// <summary>
        ///  For a given contact it hods the datagrid for all outgoing defects
        /// </summary>
        public static String ALL_CONTACT_ALL_DEFECT_OUTGOING_DEF_GRID = "ALL_CONTACT_ALL_DEFECT_OUTGOING_DEF_GRID";
        /// <summary>
        /// Holds the dictionary of defect objects for outgoing defects from contact's defect screen
        /// </summary>
        public static String ALL_CONTACT_ALL_DEFECT_OUTGOING_DEF_DICTIONARY = "ALL_CONTACT_ALL_DEFECT_OUTGOING_DEF_DICTIONARY";
        /// <summary>
        /// This variable holds the dictionary containing the existing contact details while creating new contact
        /// </summary>
        public static String ALL_CONTACT_EXISTING_CONTACT_LIST = "ALL_CONTACT_EXISTING_CONTACT_LIST";
        /// <summary>
        /// Holds all existing contacts names and contact entity id
        /// </summary>
        public static String EXISTING_CONTACT_DICTIONARY = "EXISTING_CONTACT_DICTIONARY";
        /// <summary>
        /// This variable holds the already connected pop3 client object
        /// </summary>
        public static String POP3_CLIENT_OBJECT = "POP3_CLIENT_OBJECT";
        /// <summary>
        /// This variable name is used to form the session variable name which holds a selected downloadable message in all communication screen
        /// </summary>
        public static String ALL_COMM_DOWNLOADABLE_MSG = "ALL_COMM_DOWNLOADABLE_MSG";
        /// <summary>
        /// holds the arraylist of DefectSLA objects in defect page
        /// </summary>
        public static String ALL_DEFECT_SLA_LIST = "ALL_DEFECT_SLA_LIST";
        /// <summary>
        /// In the all defects screen this variable holds the data grid containing all incoming defect details
        /// </summary>
        public static String ALL_DEFECT_ALL_INCOMING_DEFECT_GRID = "ALL_DEFECT_ALL_INCOMING_DEFECT_GRID";        
        /// <summary>
        /// This variable holds the auto complete list for the RFQ No textbox while creating defect
        /// </summary>
        public static String ALL_DEFECT_CREATE_DEFECT_RFQ_AUTOCOMPLETE_LIST="ALL_DEFECT_CREATE_DEFECT_RFQ_AUTOCOMPLETE_LIST";
        /// <summary>
        /// This variable holds the auto complete list for the Invoice No textbox while creating defect
        /// </summary>
        public static String ALL_DEFECT_CREATE_DEFECT_INV_AUTOCOMPLETE_LIST = "ALL_DEFECT_CREATE_DEFECT_INV_AUTOCOMPLETE_LIST";
        /// <summary>
        /// This variable stores the dictionary holding the invoice id and respective amount
        /// </summary>
        public static String ALL_DEFECT_CREATE_DEFECT_INV_AMNT_DICT = "ALL_DEFECT_CREATE_DEFECT_INV_AMNT_DICT";
        /// <summary>
        /// In the all defects screen this variable holds the data grid containing all outgoing defect details
        /// </summary>
        public static String ALL_DEFECT_ALL_OUTGOING_DEFECT_GRID = "ALL_DEFECT_ALL_OUTGOING_DEFECT_GRID";
        /// <summary>
        /// This variable holds the customer object for the selected incoming defect object
        /// </summary>
        public static String ALL_DEFECT_INCM_SELECTED_CUSTOMER_OBJ="ALL_DEFECT_INCM_SELECTED_CUSTOMER_OBJ";
        /// <summary>
        /// This variable holds the supplier object for the selected outgoing defect object
        /// </summary>
        public static String ALL_DEFECT_OUTG_SELECTED_SUPPLIER_OBJ = "ALL_DEFECT_OUTG_SELECTED_SUPPLIER_OBJ";

        public static String ALL_SR_CREATE_SR_RFQ_AUTOCOMPLETE_LIST = "ALL_SR_CREATE_SR_RFQ_AUTOCOMPLETE_LIST";
        public static String ALL_SR_CREATE_SR_INV_AUTOCOMPLETE_LIST = "ALL_SR_CREATE_SR_INV_AUTOCOMPLETE_LIST";
        public static String ALL_SR_CREATE_SR_INV_AMNT_DICT = "ALL_SR_CREATE_SR_INV_AMNT_DICT";
        public static String ALL_SR_ALL_INCOMING_SR_GRID = "ALL_SR_ALL_INCOMING_SR_GRID";
        public static String ALL_SR_ALL_OUTGOING_SR_GRID = "ALL_SR_ALL_OUTGOING_SR_GRID";
        public static String ALL_SR_INCM_SELECTED_CUSTOMER_OBJ = "ALL_SR_INCM_SELECTED_CUSTOMER_OBJ";
        public static String ALL_SR_OUTG_SELECTED_SUPPLIER_OBJ = "ALL_SR_OUTG_SELECTED_SUPPLIER_OBJ";
        public static String ALL_SR_SLA_LIST = "ALL_SR_SLA_LIST";

        /// <summary>
        /// This variable holds a dictionary containing the 'from-date,to-date'(key) and LeadandPotential(value) generated by the method generateLeadCharts
        /// </summary>
        public static String ALL_DASHBOARD_LEAD_AND_POTENTIAL_DICTIONARY = "ALL_DASHBOARD_LEAD_AND_POTENTIAL_DICTIONARY";
        /// <summary>
        /// This variable holds the dictionary containing the latest success-failure value for the lead conv % by val chart
        /// </summary>
        public static String ALL_DASHBOARD_LEAD_BY_VAL_SUCCESS_FAILURE = "ALL_DASHBOARD_LEAD_BY_VAL_SUCCESS_FAILURE";
        /// <summary>
        /// This variable holds the dictionary containing the latest success-failure value for the lead conv % by number chart
        /// </summary>
        public static String ALL_DASHBOARD_LEAD_BY_NUMBER_SUCCESS_FAILURE = "ALL_DASHBOARD_LEAD_BY_NUMBER_SUCCESS_FAILURE";
        /// <summary>
        /// This variable holds the dictionary containing the latest success-failure value for the potential conv % by val chart
        /// </summary>
        public static String ALL_DASHBOARD_POTN_BY_VAL_SUCCESS_FAILURE = "ALL_DASHBOARD_POTN_BY_VAL_SUCCESS_FAILURE";
        /// <summary>
        /// This variable holds the dictionary containing the latest potential stage details for the potential stages chart
        /// </summary>
        public static String ALL_DASHBOARD_POTN_BY_STAGES = "ALL_DASHBOARD_POTN_BY_STAGES";
        /// <summary>
        /// The variable holds the dictionary containing latest potential category chart details
        /// </summary>
        public static String ALL_DASHBOARD_POTN_BY_CATEGORY = "ALL_DASHBOARD_POTN_BY_CATEGORY";
        /// <summary>
        /// Stores the product cateogry names to be displayed on potential charts/reports.
        /// </summary>
        public static String ALL_DASHBOARD_POTN_BY_CATEGORY_CATG_NAMES = "ALL_DASHBOARD_POTN_BY_CATEGORY_CATG_NAMES";
        /// <summary>
        /// This variable holds the dictionary containing the latest success-failure value for the potential no % by val chart
        /// </summary>
        public static String ALL_DASHBOARD_POTN_BY_NO_SUCCESS_FAILURE = "ALL_DASHBOARD_POTN_BY_NO_SUCCESS_FAILURE";
        /// <summary>
        /// This variable holds the contact dictionary for the chart 'total sales by account'
        /// </summary>
        public static String ALL_DASHBOARD_TRAN_SALES_TOTAL_BUSINESS_BY_ACCNT_CONTACTS = "ALL_DASHBOARD_TRAN_SALES_TOTAL_BUSINESS_BY_ACCNT_CONTACTS";
        /// <summary>
        /// This variable holds the total business dictionary for the chart 'total sales by account'
        /// </summary>
        public static String ALL_DASHBOARD_TRAN_SALES_TOTAL_BUSINESS_BY_ACCNT_TOTAL_BUSINESS = "ALL_DASHBOARD_TRAN_SALES_TOTAL_BUSINESS_BY_ACCNT_TOTAL_BUSINESS";
        /// <summary>
        /// This variable holds the total pending dictionary for the chart 'total sales by account'
        /// </summary>
        public static String ALL_DASHBOARD_TRAN_SALES_TOTAL_BUSINESS_BY_ACCNT_TOTAL_PENDING = "ALL_DASHBOARD_TRAN_SALES_TOTAL_BUSINESS_BY_ACCNT_TOTAL_PENDING";
        /// <summary>
        /// This variable holds the 'total business during period' dictionary for the chart 'total sales by account'
        /// </summary>
        public static String ALL_DASHBOARD_TRAN_SALES_TOTAL_BUSINESS_BY_ACCNT_TOTAL_BUSINESS_DURING_PERIOD = "ALL_DASHBOARD_TRAN_SALES_TOTAL_BUSINESS_BY_ACCNT_TOTAL_BUSINESS_DURING_PERIOD";
        /// <summary>
        /// This variable holds the contact dicionary for 'pending clear payment' chart in transaction sales chart section
        /// </summary>
        public static String ALL_DASHBOARD_TRAN_SALES_PENDINGCLEARPMNT_CONTACT = "ALL_DASHBOARD_TRAN_SALES_PENDINGCLEARPMNT_CONTACT";
        /// <summary>
        /// This variable holds the total cleared dicionary for 'pending clear payment' chart in transaction sales chart section
        /// </summary>
        public static String ALL_DASHBOARD_TRAN_SALES_PENDINGCLEARPMNT_TOTAL_CLEARED = "ALL_DASHBOARD_TRAN_SALES_PENDINGCLEARPMNT_TOTAL_CLEARED";
        /// <summary>
        /// This variable holds the total not cleared dictionary for 'pending clear payment' chart in transaction sales chart section
        /// </summary>
        public static String ALL_DASHBOARD_TRAN_SALES_PENDINGCLEARPMNT_TOTAL_NOT_CLEARED = "ALL_DASHBOARD_TRAN_SALES_PENDINGCLEARPMNT_TOTAL_NOT_CLEARED";
        /// <summary>
        /// This variable holds the product dictionary details for the ProductWiseSalesQnty chart in transaction sales chart section
        /// </summary>
        public static String ALL_DASHBOARD_TRAN_SALES_PRODUCTWISESALES_QNTY_PROD_DICT = "ALL_DASHBOARD_TRAN_SALES_PRODUCTWISESALES_QNTY_PROD_DICT";
        /// <summary>
        /// This variable holds the sales quantity dictionary details for the ProductWiseSalesQnty chart in transaction sales chart section
        /// </summary>
        public static String ALL_DASHBOARD_TRAN_SALES_PRODUCTWISESALES_QNTY_SALES_QNTY = "ALL_DASHBOARD_TRAN_SALES_PRODUCTWISESALES_QNTY_SALES_QNTY";
        /// <summary>
        /// This variable holds the product dictionary details for the ProductWiseSalesAmnt chart in transaction sales chart section
        /// </summary>
        public static String ALL_DASHBOARD_TRAN_SALES_PRODUCTWISESALES_AMNT_PROD_DICT = "ALL_DASHBOARD_TRAN_SALES_PRODUCTWISESALES_AMNT_PROD_DICT";
        /// <summary>
        /// This variable holds the sales amount dictionary details for the ProductWiseSalesAmount chart in transaction sales chart section
        /// </summary>
        public static String ALL_DASHBOARD_TRAN_SALES_PRODUCTWISESALES_AMNT_SALES_AMNT = "ALL_DASHBOARD_TRAN_SALES_PRODUCTWISESALES_AMNT_SALES_AMNT";
        /// <summary>
        /// This variable holds the contact dictionary for the chart 'total PURCHASE by account'
        /// </summary>
        public static String ALL_DASHBOARD_TRAN_PURCHASE_TOTAL_BUSINESS_BY_ACCNT_CONTACTS = "ALL_DASHBOARD_TRAN_PURCHASE_TOTAL_BUSINESS_BY_ACCNT_CONTACTS";
        /// <summary>
        /// This variable holds the total business dictionary for the chart 'total PURCHASE by account'
        /// </summary>
        public static String ALL_DASHBOARD_TRAN_PURCHASE_TOTAL_BUSINESS_BY_ACCNT_TOTAL_BUSINESS = "ALL_DASHBOARD_TRAN_PURCHASE_TOTAL_BUSINESS_BY_ACCNT_TOTAL_BUSINESS";
        /// <summary>
        /// This variable holds the total pending dictionary for the chart 'total PURCHASE by account'
        /// </summary>
        public static String ALL_DASHBOARD_TRAN_PURCHASE_TOTAL_BUSINESS_BY_ACCNT_TOTAL_PENDING = "ALL_DASHBOARD_TRAN_PURCHASE_TOTAL_BUSINESS_BY_ACCNT_TOTAL_PENDING";
        /// <summary>
        /// This variable holds the 'total business during period' dictionary for the chart 'total PURCHASE by account'
        /// </summary>
        public static String ALL_DASHBOARD_TRAN_PURCHASE_TOTAL_BUSINESS_BY_ACCNT_TOTAL_BUSINESS_DURING_PERIOD = "ALL_DASHBOARD_TRAN_PURCHASE_TOTAL_BUSINESS_BY_ACCNT_TOTAL_BUSINESS_DURING_PERIOD";
        /// <summary>
        /// This variable holds the contact dicionary for 'pending clear payment' chart in transaction PURCHASE chart section
        /// </summary>
        public static String ALL_DASHBOARD_TRAN_PURCHASE_PENDINGCLEARPMNT_CONTACT = "ALL_DASHBOARD_TRAN_PURCHASE_PENDINGCLEARPMNT_CONTACT";
        /// <summary>
        /// This variable holds the total cleared dicionary for 'pending clear payment' chart in transaction PURCHASE chart section
        /// </summary>
        public static String ALL_DASHBOARD_TRAN_PURCHASE_PENDINGCLEARPMNT_TOTAL_CLEARED = "ALL_DASHBOARD_TRAN_PURCHASE_PENDINGCLEARPMNT_TOTAL_CLEARED";
        /// <summary>
        /// This variable holds the total not cleared dictionary for 'pending clear payment' chart in transaction PURCHASE chart section
        /// </summary>
        public static String ALL_DASHBOARD_TRAN_PURCHASE_PENDINGCLEARPMNT_TOTAL_NOT_CLEARED = "ALL_DASHBOARD_TRAN_PURCHASE_PENDINGCLEARPMNT_TOTAL_NOT_CLEARED";
        /// <summary>
        /// This variable holds the product dictionary details for the ProductWisePURCHASEQnty chart in transaction PURCHASE chart section
        /// </summary>
        public static String ALL_DASHBOARD_TRAN_PURCHASE_PRODUCTWISEPURCHASE_QNTY_PROD_DICT = "ALL_DASHBOARD_TRAN_PURCHASE_PRODUCTWISEPURCHASE_QNTY_PROD_DICT";
        /// <summary>
        /// This variable holds the PURCHASE quantity dictionary details for the ProductWisePURCHASEQnty chart in transaction PURCHASE chart section
        /// </summary>
        public static String ALL_DASHBOARD_TRAN_PURCHASE_PRODUCTWISEPURCHASE_QNTY_PURCHASE_QNTY = "ALL_DASHBOARD_TRAN_PURCHASE_PRODUCTWISEPURCHASE_QNTY_PURCHASE_QNTY";
        /// <summary>
        /// This variable holds the product dictionary details for the ProductWisePURCHASEAmnt chart in transaction PURCHASE chart section
        /// </summary>
        public static String ALL_DASHBOARD_TRAN_PURCHASE_PRODUCTWISEPURCHASE_AMNT_PROD_DICT = "ALL_DASHBOARD_TRAN_PURCHASE_PRODUCTWISEPURCHASE_AMNT_PROD_DICT";
        /// <summary>
        /// This variable holds the PURCHASE amount dictionary details for the ProductWisePURCHASEAmount chart in transaction PURCHASE chart section
        /// </summary>
        public static String ALL_DASHBOARD_TRAN_PURCHASE_PRODUCTWISEPURCHASE_AMNT_PURCHASE_AMNT = "ALL_DASHBOARD_TRAN_PURCHASE_PRODUCTWISEPURCHASE_AMNT_PURCHASE_AMNT";
        /// <summary>
        /// This variable holds the contact dictionary for 'defect value by accnt' chart in incoming defect
        /// </summary>
        public static String ALL_DASHBOARD_INCM_DEFECT_DEFECTVALBYACCNT_CONTACT_DICT = "ALL_DASHBOARD_INCM_DEFECT_DEFECTVALBYACCNT_CONTACT_DICT";
        /// <summary>
        /// This variable holds the defect amount dictionary for 'defect value by accnt' chart in incoming defect
        /// </summary>
        public static String ALL_DASHBOARD_INCM_DEFECT_DEFECTVALBYACCNT_DEFECT_AMNT = "ALL_DASHBOARD_INCM_DEFECT_DEFECTVALBYACCNT_DEFECT_AMNT";
        /// <summary>
        /// This variable holds the contact dictionary for 'defect no by accnt' chart in incoming defect
        /// </summary>
        public static String ALL_DASHBOARD_INCM_DEFECT_DEFECTNOBYACCNT_CONTACT_DICT = "ALL_DASHBOARD_OUTGOING_DEFECT_DEFECTNOBYACCNT_CONTACT_DICT";
        /// <summary>
        /// This variable holds the last selected defect type for 'defect no by accnt' chart in incoming defect
        /// </summary>
        public static String ALL_DASHBOARD_INCM_DEFECT_DEFECTNOBYACCNT_LAST_DEFECT_TYPE = "ALL_DASHBOARD_INCM_DEFECT_DEFECTNOBYACCNT_LAST_DEFECT_TYPE";
        /// <summary>
        /// This variable holds the total defects dictionary for 'defect no by accnt' chart in incoming defect
        /// </summary>
        public static String ALL_DASHBOARD_INCM_DEFECT_DEFECTNOBYACCNT_TOTAL_DEFECT = "ALL_DASHBOARD_OUTGOING_DEFECT_DEFECTNOBYACCNT_TOTAL_DEFECT";
        /// <summary>
        /// This variable holds the high sev defect dictionary for 'defect no by accnt' chart in incoming defect
        /// </summary>
        public static String ALL_DASHBOARD_INCM_DEFECT_DEFECTNOBYACCNT_HIGH_DEFECT = "ALL_DASHBOARD_OUTGOING_DEFECT_DEFECTNOBYACCNT_HIGH_DEFECT";
        /// <summary>
        /// This variable holds the medium sev dictionary for 'defect no by accnt' chart in incoming defect
        /// </summary>
        public static String ALL_DASHBOARD_INCM_DEFECT_DEFECTNOBYACCNT_MEDM_DEFECT = "ALL_DASHBOARD_OUTGOING_DEFECT_DEFECTNOBYACCNT_MEDM_DEFECT";
        /// <summary>
        /// This variable holds the low sev dictionary for 'defect no by accnt' chart in incoming defect
        /// </summary>
        public static String ALL_DASHBOARD_INCM_DEFECT_DEFECTNOBYACCNT_LOW_DEFECT = "ALL_DASHBOARD_OUTGOING_DEFECT_DEFECTNOBYACCNT_LOW_DEFECT";
        /// <summary>
        /// This variable holds the contact dictionary for 'defect value by accnt' chart in outgoing defect
        /// </summary>
        public static String ALL_DASHBOARD_OUTG_DEFECT_DEFECTVALBYACCNT_CONTACT_DICT = "ALL_DASHBOARD_OUTG_DEFECT_DEFECTVALBYACCNT_CONTACT_DICT";
        /// <summary>
        /// This variable holds the defect amount dictionary for 'defect value by accnt' chart in outgoing defect
        /// </summary>
        public static String ALL_DASHBOARD_OUTG_DEFECT_DEFECTVALBYACCNT_DEFECT_AMNT = "ALL_DASHBOARD_OUTG_DEFECT_DEFECTVALBYACCNT_DEFECT_AMNT";
        /// <summary>
        /// This variable holds the contact dictionary for 'defect no by accnt' chart in outgoing defect
        /// </summary>
        public static String ALL_DASHBOARD_OUTG_DEFECT_DEFECTNOBYACCNT_CONTACT_DICT = "ALL_DASHBOARD_OUTGOING_DEFECT_DEFECTNOBYACCNT_CONTACT_DICT";
        /// <summary>
        /// This variable holds the total defects dictionary for 'defect no by accnt' chart in outgoing defect
        /// </summary>
        public static String ALL_DASHBOARD_OUTG_DEFECT_DEFECTNOBYACCNT_TOTAL_DEFECT = "ALL_DASHBOARD_OUTGOING_DEFECT_DEFECTNOBYACCNT_TOTAL_DEFECT";
        /// <summary>
        /// This variable holds the high sev defect dictionary for 'defect no by accnt' chart in outgoing defect
        /// </summary>
        public static String ALL_DASHBOARD_OUTG_DEFECT_DEFECTNOBYACCNT_HIGH_DEFECT = "ALL_DASHBOARD_OUTGOING_DEFECT_DEFECTNOBYACCNT_HIGH_DEFECT";
        /// <summary>
        /// This variable holds the medium sev dictionary for 'defect no by accnt' chart in outgoing defect
        /// </summary>
        public static String ALL_DASHBOARD_OUTG_DEFECT_DEFECTNOBYACCNT_MEDM_DEFECT = "ALL_DASHBOARD_OUTGOING_DEFECT_DEFECTNOBYACCNT_MEDM_DEFECT";
        /// <summary>
        /// This variable holds the low sev dictionary for 'defect no by accnt' chart in outgoing defect
        /// </summary>
        public static String ALL_DASHBOARD_OUTG_DEFECT_DEFECTNOBYACCNT_LOW_DEFECT = "ALL_DASHBOARD_OUTGOING_DEFECT_DEFECTNOBYACCNT_LOW_DEFECT";
        /// <summary>
        /// In the 'arrival-closure' chart for incoming dictionary it stores the last created date range dict
        /// </summary>
        public static String ALL_DASHBOARD_INCM_DEFECT_ARRVLCLOS_DATE_RANGE = "ALL_DASHBOARD_INCM_DEFECT_ARRVLCLOS_DATE_RANGE";
        /// <summary>
        /// In the 'arrival-closure' chart for incoming dictionary it stores the last created frequency
        /// </summary>
        public static String ALL_DASHBOARD_INCM_DEFECT_ARRVLCLOS_LAST_FREQ = "ALL_DASHBOARD_INCM_DEFECT_ARRVLCLOS_LAST_FREQ";
        /// <summary>
        /// In the 'arrival-closure' chart for incoming dictionary it stores the last created all dictionaries
        /// </summary>
        public static String ALL_DASHBOARD_INCM_DEFECT_ARRVLCLOS_GENERATED_DICTS = "ALL_DASHBOARD_INCM_DEFECT_ARRVLCLOS_GENERATED_DICTS";
        /// <summary>
        /// In the 'arrival-closure' chart for incoming dictionary it stores the last created date range dict
        /// </summary>
        public static String ALL_DASHBOARD_INCM_DEFECT_AVERAGECLOSTIME_DATE_RANGE = "ALL_DASHBOARD_INCM_DEFECT_AVERAGECLOSTIME_DATE_RANGE";
        /// <summary>
        /// In the 'average-closure time' chart for incoming dictionary it stores the last created frequency
        /// </summary>
        public static String ALL_DASHBOARD_INCM_DEFECT_AVERAGECLOSTIME_LAST_FREQ = "ALL_DASHBOARD_INCM_DEFECT_AVERAGECLOSTIME_LAST_FREQ";
        /// <summary>
        ///  In the 'average-closure time' chart for incoming dictionary it stores the last selected agent
        /// </summary>
        public static String ALL_DASHBOARD_INCM_DEFECT_AVERAGECLOSTIME_LAST_SELECTED_AGENT = "ALL_DASHBOARD_INCM_DEFECT_AVERAGECLOSTIME_LAST_SELECTED_AGENT";
        /// <summary>
        /// In the 'average-closure time' chart for incoming dictionary it stores the last created all dictionaries
        /// </summary>
        public static String ALL_DASHBOARD_INCM_DEFECT_AVERAGECLOSTIME_GENERATED_DICTS = "ALL_DASHBOARD_INCM_DEFECT_AVERAGECLOSTIME_GENERATED_DICTS";

        /// <summary>
        /// This variable holds the contact dictionary for 'SR value by accnt' chart in incoming SR
        /// </summary>
        public static String ALL_DASHBOARD_INCM_SR_SRVALBYACCNT_CONTACT_DICT = "ALL_DASHBOARD_INCM_SR_SRVALBYACCNT_CONTACT_DICT";
        /// <summary>
        /// This variable holds the SR amount dictionary for 'SR value by accnt' chart in incoming SR
        /// </summary>
        public static String ALL_DASHBOARD_INCM_SR_SRVALBYACCNT_SR_AMNT = "ALL_DASHBOARD_INCM_SR_SRVALBYACCNT_SR_AMNT";
        /// <summary>
        /// This variable holds the contact dictionary for 'SR no by accnt' chart in incoming SR
        /// </summary>
        public static String ALL_DASHBOARD_INCM_SR_SRNOBYACCNT_CONTACT_DICT = "ALL_DASHBOARD_OUTGOING_SR_SRNOBYACCNT_CONTACT_DICT";
        /// <summary>
        /// This variable holds the last selected SR type for 'SR no by accnt' chart in incoming SR
        /// </summary>
        public static String ALL_DASHBOARD_INCM_SR_SRNOBYACCNT_LAST_SR_TYPE = "ALL_DASHBOARD_INCM_SR_SRNOBYACCNT_LAST_SR_TYPE";
        /// <summary>
        /// This variable holds the total SRs dictionary for 'SR no by accnt' chart in incoming SR
        /// </summary>
        public static String ALL_DASHBOARD_INCM_SR_SRNOBYACCNT_TOTAL_SR = "ALL_DASHBOARD_OUTGOING_SR_SRNOBYACCNT_TOTAL_SR";
        /// <summary>
        /// This variable holds the high sev SR dictionary for 'SR no by accnt' chart in incoming SR
        /// </summary>
        public static String ALL_DASHBOARD_INCM_SR_SRNOBYACCNT_HIGH_SR = "ALL_DASHBOARD_OUTGOING_SR_SRNOBYACCNT_HIGH_SR";
        /// <summary>
        /// This variable holds the medium sev dictionary for 'SR no by accnt' chart in incoming SR
        /// </summary>
        public static String ALL_DASHBOARD_INCM_SR_SRNOBYACCNT_MEDM_SR = "ALL_DASHBOARD_OUTGOING_SR_SRNOBYACCNT_MEDM_SR";
        /// <summary>
        /// This variable holds the low sev dictionary for 'SR no by accnt' chart in incoming SR
        /// </summary>
        public static String ALL_DASHBOARD_INCM_SR_SRNOBYACCNT_LOW_SR = "ALL_DASHBOARD_OUTGOING_SR_SRNOBYACCNT_LOW_SR";
        /// <summary>
        /// This variable holds the contact dictionary for 'SR value by accnt' chart in outgoing SR
        /// </summary>
        public static String ALL_DASHBOARD_OUTG_SR_SRVALBYACCNT_CONTACT_DICT = "ALL_DASHBOARD_OUTG_SR_SRVALBYACCNT_CONTACT_DICT";
        /// <summary>
        /// This variable holds the SR amount dictionary for 'SR value by accnt' chart in outgoing SR
        /// </summary>
        public static String ALL_DASHBOARD_OUTG_SR_SRVALBYACCNT_SR_AMNT = "ALL_DASHBOARD_OUTG_SR_SRVALBYACCNT_SR_AMNT";
        /// <summary>
        /// This variable holds the contact dictionary for 'SR no by accnt' chart in outgoing SR
        /// </summary>
        public static String ALL_DASHBOARD_OUTG_SR_SRNOBYACCNT_CONTACT_DICT = "ALL_DASHBOARD_OUTGOING_SR_SRNOBYACCNT_CONTACT_DICT";
        /// <summary>
        /// This variable holds the total SRs dictionary for 'SR no by accnt' chart in outgoing SR
        /// </summary>
        public static String ALL_DASHBOARD_OUTG_SR_SRNOBYACCNT_TOTAL_SR = "ALL_DASHBOARD_OUTGOING_SR_SRNOBYACCNT_TOTAL_SR";
        /// <summary>
        /// This variable holds the high sev SR dictionary for 'SR no by accnt' chart in outgoing SR
        /// </summary>
        public static String ALL_DASHBOARD_OUTG_SR_SRNOBYACCNT_HIGH_SR = "ALL_DASHBOARD_OUTGOING_SR_SRNOBYACCNT_HIGH_SR";
        /// <summary>
        /// This variable holds the medium sev dictionary for 'SR no by accnt' chart in outgoing SR
        /// </summary>
        public static String ALL_DASHBOARD_OUTG_SR_SRNOBYACCNT_MEDM_SR = "ALL_DASHBOARD_OUTGOING_SR_SRNOBYACCNT_MEDM_SR";
        /// <summary>
        /// This variable holds the low sev dictionary for 'SR no by accnt' chart in outgoing SR
        /// </summary>
        public static String ALL_DASHBOARD_OUTG_SR_SRNOBYACCNT_LOW_SR = "ALL_DASHBOARD_OUTGOING_SR_SRNOBYACCNT_LOW_SR";
        /// <summary>
        /// In the 'arrival-closure' chart for incoming dictionary it stores the last created date range dict
        /// </summary>
        public static String ALL_DASHBOARD_INCM_SR_ARRVLCLOS_DATE_RANGE = "ALL_DASHBOARD_INCM_SR_ARRVLCLOS_DATE_RANGE";
        /// <summary>
        /// In the 'arrival-closure' chart for incoming dictionary it stores the last created frequency
        /// </summary>
        public static String ALL_DASHBOARD_INCM_SR_ARRVLCLOS_LAST_FREQ = "ALL_DASHBOARD_INCM_SR_ARRVLCLOS_LAST_FREQ";


        /// <summary>
        /// In the 'arrival-closure' chart for outgoing dictionary it stores the last created date range dict
        /// </summary>
        public static String ALL_DASHBOARD_OUTG_SR_ARRVLCLOS_DATE_RANGE = "ALL_DASHBOARD_OUTG_SR_ARRVLCLOS_DATE_RANGE";
        /// <summary>
        /// In the 'arrival-closure' chart for outgoing dictionary it stores the last created frequency
        /// </summary>
        public static String ALL_DASHBOARD_OUTG_SR_ARRVLCLOS_LAST_FREQ = "ALL_DASHBOARD_OUTG_SR_ARRVLCLOS_LAST_FREQ";
        public static String ALL_DASHBOARD_OUTG_SR_ARRVLCLOS_GENERATED_DICTS = "ALL_DASHBOARD_OUTG_SR_ARRVLCLOS_GENERATED_DICTS";
        /// <summary>
        /// In the 'arrival-closure' chart for outgoing dictionary it stores the last created date range dict
        /// </summary>
        public static String ALL_DASHBOARD_OUTG_SR_AVERAGECLOSTIME_DATE_RANGE = "ALL_DASHBOARD_OUTG_SR_AVERAGECLOSTIME_DATE_RANGE";
        /// <summary>
        /// In the 'average-closure time' chart for outgoing dictionary it stores the last created frequency
        /// </summary>
        public static String ALL_DASHBOARD_OUTG_SR_AVERAGECLOSTIME_LAST_FREQ = "ALL_DASHBOARD_OUTG_SR_AVERAGECLOSTIME_LAST_FREQ";
        


        /// <summary>
        /// In the 'arrival-closure' chart for incoming dictionary it stores the last created all dictionaries
        /// </summary>
        public static String ALL_DASHBOARD_INCM_SR_ARRVLCLOS_GENERATED_DICTS = "ALL_DASHBOARD_INCM_SR_ARRVLCLOS_GENERATED_DICTS";
        /// <summary>
        /// In the 'arrival-closure' chart for incoming dictionary it stores the last created date range dict
        /// </summary>
        public static String ALL_DASHBOARD_INCM_SR_AVERAGECLOSTIME_DATE_RANGE = "ALL_DASHBOARD_INCM_SR_AVERAGECLOSTIME_DATE_RANGE";
        /// <summary>
        /// In the 'average-closure time' chart for incoming dictionary it stores the last created frequency
        /// </summary>
        public static String ALL_DASHBOARD_INCM_SR_AVERAGECLOSTIME_LAST_FREQ = "ALL_DASHBOARD_INCM_SR_AVERAGECLOSTIME_LAST_FREQ";
        /// <summary>
        ///  In the 'average-closure time' chart for incoming dictionary it stores the last selected agent
        /// </summary>
        public static String ALL_DASHBOARD_INCM_SR_AVERAGECLOSTIME_LAST_SELECTED_AGENT = "ALL_DASHBOARD_INCM_SR_AVERAGECLOSTIME_LAST_SELECTED_AGENT";
        public static String ALL_DASHBOARD_OUTG_SR_AVERAGECLOSTIME_LAST_SELECTED_AGENT = "ALL_DASHBOARD_OUTG_SR_AVERAGECLOSTIME_LAST_SELECTED_AGENT";
        /// <summary>
        /// In the 'average-closure time' chart for incoming dictionary it stores the last created all dictionaries
        /// </summary>
        public static String ALL_DASHBOARD_INCM_SR_AVERAGECLOSTIME_GENERATED_DICTS = "ALL_DASHBOARD_INCM_SR_AVERAGECLOSTIME_GENERATED_DICTS";
        public static String ALL_DASHBOARD_OUTG_SR_AVERAGECLOSTIME_GENERATED_DICTS = "ALL_DASHBOARD_OUTG_SR_AVERAGECLOSTIME_GENERATED_DICTS";
        /// <summary>
        /// Holds all the data table of dictionaries currently generated for reports. The key of each of these items identify the unique name of the 
        /// report whose value is stored alongside. The key is passed to the report generator module which extracts the datatable from the session
        /// </summary>
        public static String ALL_REPORTS_SESSION_DICTIONARY_OF_DATATABLES = "ALL_REPORTS_SESSION_DICTIONARY";
        /// <summary>
        /// In the 'arrival-closure' chart for outgoing dictionary it stores the last created date range dict
        /// </summary>
        public static String ALL_DASHBOARD_OUTG_DEFECT_ARRVLCLOS_DATE_RANGE = "ALL_DASHBOARD_OUTG_DEFECT_ARRVLCLOS_DATE_RANGE";
        /// <summary>
        /// In the 'arrival-closure' chart for outgoing dictionary it stores the last created frequency
        /// </summary>
        public static String ALL_DASHBOARD_OUTG_DEFECT_ARRVLCLOS_LAST_FREQ = "ALL_DASHBOARD_OUTG_DEFECT_ARRVLCLOS_LAST_FREQ";
        /// <summary>
        /// In the 'arrival-closure' chart for outgoing dictionary it stores the last created all dictionaries
        /// </summary>
        public static String ALL_DASHBOARD_OUTG_DEFECT_ARRVLCLOS_GENERATED_DICTS = "ALL_DASHBOARD_OUTG_DEFECT_ARRVLCLOS_GENERATED_DICTS";
        /// <summary>
        /// In the 'arrival-closure' chart for outgoing dictionary it stores the last created date range dict
        /// </summary>
        public static String ALL_DASHBOARD_OUTG_DEFECT_AVERAGECLOSTIME_DATE_RANGE = "ALL_DASHBOARD_OUTG_DEFECT_AVERAGECLOSTIME_DATE_RANGE";
        /// <summary>
        /// In the 'average-closure time' chart for outgoing dictionary it stores the last created frequency
        /// </summary>
        public static String ALL_DASHBOARD_OUTG_DEFECT_AVERAGECLOSTIME_LAST_FREQ = "ALL_DASHBOARD_OUTG_DEFECT_AVERAGECLOSTIME_LAST_FREQ";
        /// <summary>
        ///  In the 'average-closure time' chart for outgoing dictionary it stores the last selected vendor
        /// </summary>
        public static String ALL_DASHBOARD_OUTG_DEFECT_AVERAGECLOSTIME_LAST_SELECTED_AGENT = "ALL_DASHBOARD_OUTG_DEFECT_AVERAGECLOSTIME_LAST_SELECTED_AGENT";
        /// <summary>
        /// In the 'average-closure time' chart for outgoing dictionary it stores the last created all dictionaries
        /// </summary>
        public static String ALL_DASHBOARD_OUTG_DEFECT_AVERAGECLOSTIME_GENERATED_DICTS = "ALL_DASHBOARD_OUTG_DEFECT_AVERAGECLOSTIME_GENERATED_DICTS";
        /// <summary>
        /// This variable contains a dictionary of DataTable where the key is the respective context id for the notes.
        /// </summary>
        public static String ALL_NOTES_DATAGRID = "ALL_NOTES_DATAGRID";
        /// <summary>
        /// This variable contains a dictionary of DataTable where the key is the respective context id for the messages.
        /// </summary>
        public static String ALL_MSGS_DATAGRID = "ALL_MSGS_DATAGRID";
        /// <summary>
        /// Stores the data grid for the department data grid from admin pref-org mgmt screen
        /// </summary>
        public static String ADMIN_PREF_DEPT_MGMT_DEPT_GRID = "ADMIN_PREF_DEPT_MGMT_DEPT_GRID";

        public static String ADMIN_PREF_ACCESS_MGMT_EXISTING_GROUP_DATAGRID = "ADMIN_PREF_ACCESS_MGMT_EXISTING_GROUP_DATAGRID";

        public static String ADMIN_PREF_ACCESS_MGMT_EXISTING_GROUP_LIST = "ADMIN_PREF_ACCESS_MGMT_EXISTING_GROUP_LIST";
        /// <summary>
        /// This variable holds the RFQ datagrid for the list of RFQs which needs the user's approval
        /// </summary>
        public static String HOME_PAGE_RFQ_DATAGRID = "HOME_PAGE_RFQ_DATAGRID";
        /// <summary>
        /// This variable holds the Invoice datagrid for the list of Invoices which needs the user's approval
        /// </summary>
        public static String HOME_PAGE_INV_DATAGRID = "HOME_PAGE_INV_DATAGRID";
        /// <summary>
        /// This variable holds the Lead datagrid for the list of Lead which are assigned to the user and are active
        /// </summary>
        public static String HOME_PAGE_ASSGND_LEAD_DATAGRID = "HOME_PAGE_ASSGND_LEAD_DATAGRID";
        /// <summary>
        /// This variable holds the Potential datagrid for the list of Potential which are assigned to the user and are active
        /// </summary>
        public static String HOME_PAGE_ASSGND_POTN_DATAGRID = "HOME_PAGE_ASSGND_POTN_DATAGRID";
        /// <summary>
        /// This variable holds the Defects datagrid for the list of Defects which are assigned to the user and are not resolved
        /// </summary>
        public static String HOME_PAGE_ASSGND_OPEN_DEFECTS_DATAGRID = "HOME_PAGE_ASSGND_OPEN_DEFECTS_DATAGRID";

}
}
