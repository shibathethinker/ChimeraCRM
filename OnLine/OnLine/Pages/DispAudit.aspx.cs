using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BackEndObjects;
using ActionLibrary;
using System.Collections;
using System.Data;

namespace OnLine.Pages
{
    public partial class DispAudit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                String[] contextId1 = Request.QueryString.GetValues("contextId1");
                String[] contextId2 = Request.QueryString.GetValues("contextId2");
                String[] contextId3 = Request.QueryString.GetValues("contextId3");
                String[] contextIdTable = Request.QueryString.GetValues("contextIdTable");

                loadFiledMapping();
                fillGrid(contextId1[0], contextId2[0], contextId3[0],(contextIdTable==null||contextIdTable[0]==null?"":contextIdTable[0]));
            }
        }

        protected void loadFiledMapping()
        {
            Dictionary<String, Dictionary<String, AuditFieldMapping>> mappingDict=(Dictionary<String,Dictionary<String,AuditFieldMapping>>)Cache["AllAuditFieldMapping"];
            if (mappingDict == null || mappingDict.Count == 0)
            {
                mappingDict = BackEndObjects.AuditFieldMapping.getAllFieldMappingDetails();
                Cache.Insert("AllAuditFieldMapping", mappingDict, null, DateTime.UtcNow.AddHours(24), System.Web.Caching.Cache.NoSlidingExpiration);
            }

        }

        protected void fillGrid(String contextId1, String contextId2, String contextId3,String contextTable)
        {
            ArrayList contextIdList = new ArrayList();
            if (contextId1 != null && !contextId1.Equals(""))
                contextIdList.Add(contextId1);
            if (contextId2 != null && !contextId2.Equals(""))
                contextIdList.Add(contextId2);
            if (contextId3 != null && !contextId3.Equals(""))
                contextIdList.Add(contextId3);

            ArrayList auditDataList = BackEndObjects.AuditData.getAuditRecordsForContextIdsDB(contextIdList, contextTable);
            Dictionary<String,Dictionary<String,AuditFieldMapping>> mappingDict=(Dictionary<String,Dictionary<String,AuditFieldMapping>>)
                Cache["AllAuditFieldMapping"];
            
            DataTable dt = new DataTable();
            dt.Columns.Add("FieldName");
            dt.Columns.Add("FieldValue");
            dt.Columns.Add("ChangeTime");
            dt.Columns.Add("ChangedUser");
            dt.Columns.Add("ChangdOrg");

            String entId = Session[SessionFactory.MAIN_BUSINESS_ENTITY_ID_STRING].ToString();
            String orgName = MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(entId).getEntityName();
            
            for (int i = 0; i < auditDataList.Count; i++)
            {
                dt.Rows.Add();
                BackEndObjects.AuditData auditDataObj=(BackEndObjects.AuditData)auditDataList[i];

                Dictionary<String, AuditFieldMapping> tableSpecificMappingDict = null;
                
                if(mappingDict.ContainsKey(auditDataObj.getTable_name()))
                    tableSpecificMappingDict=mappingDict[auditDataObj.getTable_name()];

                if (tableSpecificMappingDict!=null && tableSpecificMappingDict.ContainsKey(auditDataObj.getField_name()))
                {
                    if (tableSpecificMappingDict[auditDataObj.getField_name()].getField_visible().Equals("Y"))
                        dt.Rows[i]["FieldName"] = tableSpecificMappingDict[auditDataObj.getField_name()].getField_display_name();
                    else
                        continue;
                }
                else
                    dt.Rows[i]["FieldName"]= auditDataObj.getField_name();;
              
                dt.Rows[i]["FieldValue"] = auditDataObj.getField_value();
                dt.Rows[i]["ChangeTime"] = auditDataObj.getChange_date_time();
                dt.Rows[i]["ChangedUser"] = auditDataObj.getChnaged_by_user();
                if (!entId.Equals(auditDataObj.getChanged_by_cmp()))
                    dt.Rows[i]["ChangdOrg"] = MainBusinessEntity.getMainBusinessEntitybyIdwithLessDetailsDB(auditDataObj.getChanged_by_cmp()).getEntityName();
                else
                    dt.Rows[i]["ChangdOrg"] = orgName;
            }
            GridView1.DataSource = dt;
            GridView1.DataBind();
            GridView1.Visible = true;
        }
    }
}