using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Oracle.DataAccess.Client;
using System.Threading;
using System.Collections;
using System.Web.UI;
using System.Configuration;


namespace DBConn
{
    /*
     * This package is for all esssentials related to database connectivity 
     * All database related requests will through this packages
     *   
     * Most of the important requests coming through this object is logged to a DB table for
     * future references/audit purposes
     * 
     * It is advisable to invoke the disconnet method after the user is done with the object
     * 
     * @Author: Shibasis Sengupta
     */
    public class Connections
    {
        private String conString = "Data Source=ROOT-PC\\SQLEXPRESS_NEW;Initial Catalog=BEOBJ;Integrated Security=True;Min Pool Size=20;Max Pool Size=200;";
        public static String MAIN_BUSINESS_ENTITY_ID_STRING = "mainBusinessEntityId";
        public static String LOGGED_IN_USER_ID_STRING = "LOGGED_IN_USER_ID_STRING";

        private DataSet ds = null;
        private OracleCommand orclCmd = null;
        private OracleDataAdapter orclAdap = null;
        private SqlDataAdapter sqlAdap = null;
        private SqlCommand sqlCmd = null;

        public const String RETURN_DATASET_NAME="tempDataset";
        public const String DATABASE_LOG_TABLE = "dblog";
        public const String SUCCESS_STATUS = "success";
        public const String FAILURE_STATUS = "failure";
        public const String INSERT_DB_LOG = "insert into dblog values ";
        public const String OPERATION_UPDATE = "update";
        public const String OPERATION_DELETE = "delete";
        
        public const String STRING_TYPE = "varchar";
        public const String NUMBER_TYPE = "number";
        public const String BLOB_TYPE = "blob";
        public const String DATE_TIME_TYPE = "date_time";

        public OracleConnection orclConn = null;
        public OracleDataReader orclDr = null;
        public String command;
        public String operation;
        public  SqlConnection sqlConn = null; 
        
        public String strd;
            
        ~Connections()
        {
            //if (this.orclConn != null)
                //orclConn.Close();
          //  if (this.sqlConn != null)
               // sqlConn.Close();
            //if (this.ds != null)
                //ds.Clear();
            //if (this.orclAdap != null)
                //orclAdap.Dispose();
            //if (this.orclCmd != null)
                //orclCmd.Dispose();
        }

        /// <summary>
        /// it is advisable to invoke this method once the user is done with an object of this class
        /// </summary>
        public void disconnect()
        {
            //It is advisable to invoke this method explicitly after the user is done with an object of this class

            if (this.orclConn != null)
                orclConn.Close();
            if (this.sqlConn != null)
                sqlConn.Close();
            if (this.ds != null)
                ds.Clear();
            if (this.orclAdap != null)
                orclAdap.Dispose();
            if (this.orclCmd != null)
                orclCmd.Dispose();

        }

        /// <summary>
        /// this method initiates the sqlconnection object associated with this class,
        /// it is advisable to invoke the disconnect() method of this class to release resource once the user is done with the connection
        /// </summary>
        public Connections()
        {      
            //orclConn = new OracleConnection(conString);
            conString = ConfigurationManager.ConnectionStrings["OnlineMarketPlaceConnectionString"].ToString();
            sqlConn = (sqlConn == null ? new SqlConnection(conString) : sqlConn);
            sqlConn.Open();
            ds = new DataSet();
         }
        
        /// <summary>
        /// this method returns a dataset filled with records after the command is executed
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="userId"></param>
        /// <returns></returns>

        public DataSet execQuery(String cmd,String userId) 
        {
            String status = FAILURE_STATUS ;
            try
            {
                //orclCmd = new OracleCommand(cmd, orclConn);
                //orclAdap = new OracleDataAdapter(orclCmd);
                //orclAdap.Fill(ds, RETURN_DATASET_NAME);
                //status =SUCCESS_STATUS  ;
                ds.Clear();
                sqlCmd = new SqlCommand(cmd,sqlConn);
                sqlAdap = new SqlDataAdapter(sqlCmd);
                sqlAdap.Fill(ds, RETURN_DATASET_NAME);
                status = SUCCESS_STATUS;
                
              }
            catch (Exception e)
            {
                throw e;
            }
         //   finally
            //{
                //It can be logged only if there is no problem in creating the connection
                //cmd = INSERT_DB_LOG + "(" + cmd + "," + (DateTime.Now) +","+ (userId != null ? userId : "")+")";
                //orclCmd = new OracleCommand(cmd, orclConn);
                //orclCmd.ExecuteNonQuery();
            //}
            return ds;
        }
        /// <summary>
        /// The third parameter is added for audit - framework
        /// It will hold the entity id. If this id/user id is passed as empty no audit entry will be made
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="userId"></param>
        /// <param name="contextId"></param>
        /// <returns></returns>
        public int execInsertUpdate(String cmd,String userId)
        {
            int count = 0;
            try
            {
                
         //       orclCmd = new OracleCommand(cmd, orclConn);
           //     count = orclCmd.ExecuteNonQuery();
                sqlCmd=new SqlCommand(cmd,sqlConn);
                count=sqlCmd.ExecuteNonQuery();                
                    AuditData.createAuditRecordForCommand(cmd,
                        System.Web.HttpContext.Current.Session[Connections.LOGGED_IN_USER_ID_STRING].ToString(), 
                        System.Web.HttpContext.Current.Session[Connections.MAIN_BUSINESS_ENTITY_ID_STRING].ToString());
                
                
            }
            catch (Exception e)
            {
                //throw e;
            }
            finally
            {
                //It can be logged only if there is no problem in creating the connection
                //cmd = INSERT_DB_LOG + "(" + cmd + "," + (DateTime.Now) + "," + (userId != null ? userId : "") + ")";
                //orclCmd = new OracleCommand(cmd, orclConn);
                //orclCmd.ExecuteNonQuery();

            }
            return count;

        }
        /// <summary>        
        /// This method is a non-audit version of the 'execInsertUpdate' method. Mainly used by Audit framework's internal method
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="userId"></param>
        /// <param name="contextId"></param>
        /// <returns></returns>
        public int execInsertUpdateWithoutAuditEntry(String cmd)
        {
            int count = 0;
            try
            {

                //       orclCmd = new OracleCommand(cmd, orclConn);
                //     count = orclCmd.ExecuteNonQuery();
                sqlCmd = new SqlCommand(cmd, sqlConn);
                count = sqlCmd.ExecuteNonQuery();
                
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                //It can be logged only if there is no problem in creating the connection
                //cmd = INSERT_DB_LOG + "(" + cmd + "," + (DateTime.Now) + "," + (userId != null ? userId : "") + ")";
                //orclCmd = new OracleCommand(cmd, orclConn);
                //orclCmd.ExecuteNonQuery();

            }
            return count;

        }
    }

    public class AuditData
    {

        private String context_id1;
        private String context_id2;
        private String context_id3;
        private String context_field_name1;
        private String context_field_name2;
        private String context_field_name3;
        private String table_name;
        private String field_name;
        private String field_value;
        private String change_date_time;
        private String chnaged_by_user;
        private String changed_by_cmp;

        private static String sessionUser = "";

        public const String AUDIT_CONTEXT_MAPPING_COL_TABLE_NAME = "table_name";
        public const String AUDIT_CONTEXT_MAPPING_COL_CONTEXT_FIELD1 = "context_field1";
        public const String AUDIT_CONTEXT_MAPPING_COL_CONTEXT_FIELD2 = "context_field2";
        public const String AUDIT_CONTEXT_MAPPING_COL_CONTEXT_FIELD3 = "context_field3";

        public const String AUDIT_CONTEXT_MAPPING_TABLE = "Audit_Table_Context_Field_Mapping";

        public const String AUDIT_DATA_COL_CONTEXT_ID1 = "context_id1";
        public const String AUDIT_DATA_COL_CONTEXT_ID2 = "context_id2";
        public const String AUDIT_DATA_COL_CONTEXT_ID3 = "context_id3";
        public const String AUDIT_DATA_COL_CONTEXT_FIELD_NAME1 = "context_field_name1";
        public const String AUDIT_DATA_COL_CONTEXT_FIELD_NAME2 = "context_field_name2";
        public const String AUDIT_DATA_COL_CONTEXT_FIELD_NAME3 = "context_field_name3";
        public const String AUDIT_DATA_COL_TABLE_NAME = "table_name";
        public const String AUDIT_DATA_COL_FIELD_NAME = "field_name";
        public const String AUDIT_DATA_COL_FIELD_VALUE = "field_value";
        public const String AUDIT_DATA_COL_CHANGE_DATE_TIME = "change_date_time";
        public const String AUDIT_DATA_COL_CHNAGED_BY_USER = "changed_by_user";
        public const String AUDIT_DATA_COL_CHANGED_BY_CMP = "changed_by_cmp";

        public const String AUDIT_DATA_TABLE = "Audit_Data";

        public const String AUDIT_DATA_COL_CONTEXT_ID1_TYPE = Connections.STRING_TYPE;
        public const String AUDIT_DATA_COL_CONTEXT_ID2_TYPE = Connections.STRING_TYPE;
        public const String AUDIT_DATA_COL_CONTEXT_ID3_TYPE = Connections.STRING_TYPE;
        public const String AUDIT_DATA_COL_CONTEXT_FIELD_NAME1_TYPE = Connections.STRING_TYPE;
        public const String AUDIT_DATA_COL_CONTEXT_FIELD_NAME2_TYPE = Connections.STRING_TYPE;
        public const String AUDIT_DATA_COL_CONTEXT_FIELD_NAME3_TYPE = Connections.STRING_TYPE;
        public const String AUDIT_DATA_COL_TABLE_NAME_TYPE = Connections.STRING_TYPE;
        public const String AUDIT_DATA_COL_FIELD_NAME_TYPE = Connections.STRING_TYPE;
        public const String AUDIT_DATA_COL_FIELD_VALUE_TYPE = Connections.STRING_TYPE;
        public const String AUDIT_DATA_COL_CHANGE_DATE_TIME_TYPE = Connections.STRING_TYPE;
        public const String AUDIT_DATA_COL_CHNAGED_BY_USER_TYPE = Connections.STRING_TYPE;
        public const String AUDIT_DATA_COL_CHANGED_BY_CMP_TYPE = Connections.STRING_TYPE;

        /// <summary>
        /// Returns all the three context ids associated with this object
        /// </summary>
        /// <returns></returns>
        public Dictionary<String, String> getAllContextIds()
        {
            Dictionary<String, String> returnDict = new Dictionary<String, String>();
            returnDict.Add("1", this.getContext_id1());
            returnDict.Add("2", this.getContext_id2());
            returnDict.Add("3", this.getContext_id3());

            return returnDict;
        }
        public Dictionary<String, String> getAllContextFieldNames()
        {
            Dictionary<String, String> returnDict = new Dictionary<String, String>();
            returnDict.Add("1", this.getContext_field_name1());
            returnDict.Add("2", this.getContext_field_name2());
            returnDict.Add("3", this.getContext_field_name3());

            return returnDict;
        }
        public String getContext_id1()
        {
            return context_id1;
        }
        public void setContext_id1(String context_id1)
        {
            this.context_id1 = context_id1;
        }
        public String getContext_id2()
        {
            return context_id2;
        }
        public void setContext_id2(String context_id2)
        {
            this.context_id2 = context_id2;
        }
        public String getContext_id3()
        {
            return context_id3;
        }
        public void setContext_id3(String context_id3)
        {
            this.context_id3 = context_id3;
        }
        public String getContext_field_name1()
        {
            return context_field_name1;
        }
        public void setContext_field_name1(String context_field_name1)
        {
            this.context_field_name1 = context_field_name1;
        }
        public String getContext_field_name2()
        {
            return context_field_name2;
        }
        public void setContext_field_name2(String context_field_name2)
        {
            this.context_field_name2 = context_field_name2;
        }
        public String getContext_field_name3()
        {
            return context_field_name3;
        }
        public void setContext_field_name3(String context_field_name3)
        {
            this.context_field_name3 = context_field_name3;
        }
        public String getTable_name()
        {
            return table_name;
        }
        public void setTable_name(String table_name)
        {
            this.table_name = table_name;
        }
        public String getField_name()
        {
            return field_name;
        }
        public void setField_name(String field_name)
        {
            this.field_name = field_name;
        }
        public String getField_value()
        {
            return field_value;
        }
        public void setField_value(String field_value)
        {
            this.field_value = field_value;
        }
        public String getChange_date_time()
        {
            return change_date_time;
        }
        public void setChange_date_time(String change_date_time)
        {
            this.change_date_time = change_date_time;
        }
        public String getChnaged_by_user()
        {
            return chnaged_by_user;
        }
        public void setChnaged_by_user(String chnaged_by_user)
        {
            this.chnaged_by_user = chnaged_by_user;
        }
        public String getChanged_by_cmp()
        {
            return changed_by_cmp;
        }
        public void setChanged_by_cmp(String changed_by_cmp)
        {
            this.changed_by_cmp = changed_by_cmp;
        }

        /// <summary>
        /// Inserts the passed AuditData object into database
        /// </summary>
        /// <param name="auditObj"></param>
        /// <returns></returns>
        public static int insertContextObject(AuditData auditObj)
        {
            int rowsAffected = 0;
            String insertCmd = "insert into " + AuditData.AUDIT_DATA_TABLE
    + " ("
    + AuditData.AUDIT_DATA_COL_CHANGE_DATE_TIME + ","
    + AuditData.AUDIT_DATA_COL_CHANGED_BY_CMP + ","
    + AuditData.AUDIT_DATA_COL_CHNAGED_BY_USER + ","
    + AuditData.AUDIT_DATA_COL_CONTEXT_ID1 + ","
    + AuditData.AUDIT_DATA_COL_CONTEXT_ID2 + ","
    + AuditData.AUDIT_DATA_COL_CONTEXT_ID3 + ","
    + AuditData.AUDIT_DATA_COL_CONTEXT_FIELD_NAME1 + ","
    + AuditData.AUDIT_DATA_COL_CONTEXT_FIELD_NAME2 + ","
    + AuditData.AUDIT_DATA_COL_CONTEXT_FIELD_NAME3 + ","
    + AuditData.AUDIT_DATA_COL_FIELD_NAME + ","
    + AuditData.AUDIT_DATA_COL_FIELD_VALUE + ","
    + AuditData.AUDIT_DATA_COL_TABLE_NAME + ")"
    + "values (";

            Connections cn = new Connections();

            insertCmd = insertCmd + " '" + auditObj.getChange_date_time() + "'," + "'" +
                auditObj.getChanged_by_cmp() + "'," + "'" +
                auditObj.getChnaged_by_user() + "'," + "'" +
                auditObj.getContext_id1() + "'," + "'" +
                auditObj.getContext_id2() + "'," + "'" +
                auditObj.getContext_id3() + "'," + "'" +
                auditObj.getContext_field_name1() + "'," + "'" +
                auditObj.getContext_field_name2() + "'," + "'" +
                auditObj.getContext_field_name3() + "'," + "'" +
                auditObj.getField_name() + "'," + "'" +
                auditObj.getField_value() + "'," + "'" +
                auditObj.getTable_name() + "'";

            insertCmd += ")";

            rowsAffected += cn.execInsertUpdateWithoutAuditEntry(insertCmd);

            cn.disconnect();
            return rowsAffected;
        }
        /// <summary>
        /// returns all context field names in the dictionary for the mentioned table
        /// </summary>
        /// <param name="tabName"></param>
        /// <returns></returns>
        public static Dictionary<String, String> getContextFieldNamesforTable(String tabName)
        {
            Dictionary<String, String> contextFieldsNameList = new Dictionary<string, string>();
            Connections cn = new Connections();

            String selectCmd = " select " + AuditData.AUDIT_CONTEXT_MAPPING_COL_TABLE_NAME +
                                "," + AuditData.AUDIT_CONTEXT_MAPPING_COL_CONTEXT_FIELD1 +
                                "," + AuditData.AUDIT_CONTEXT_MAPPING_COL_CONTEXT_FIELD2 +
                                "," + AuditData.AUDIT_CONTEXT_MAPPING_COL_CONTEXT_FIELD3 +
                                " from " + AuditData.AUDIT_CONTEXT_MAPPING_TABLE +
                                " where " + AuditData.AUDIT_CONTEXT_MAPPING_COL_TABLE_NAME + "='" + tabName.Trim() + "'";

            DataSet ds = cn.execQuery(selectCmd, sessionUser);
            DataTable dt = ds.Tables[0];

            foreach (DataRow dr in dt.Rows)
            {
                if (!dr[1].ToString().Equals(""))
                    contextFieldsNameList.Add(dr[1].ToString(), dr[1].ToString());
                if (!dr[2].ToString().Equals(""))
                    contextFieldsNameList.Add(dr[2].ToString(), dr[2].ToString());
                if (!dr[3].ToString().Equals(""))
                    contextFieldsNameList.Add(dr[3].ToString(), dr[3].ToString());
            }
            cn.disconnect();

            return contextFieldsNameList;
        }
        /// <summary>
        /// This method works in multiple steps -
        /// 1. Invoke getAuditRecord to generate a dictionary of field names and values as passed in the command
        /// 2. Invoke getContextFieldValues to get the dictionary of context fields and their respective values
        /// 3. No audit entry will be created for that command where there is not context field value
        /// 4. one entry of a field and its value along with the respective (upto 3) context field/s will be made into audit data table
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="user"></param>
        /// <param name="cmpId"></param>
        /// <returns></returns>
        public static int createAuditRecordForCommand(String cmd, String user, String cmpId)
        {
            Dictionary<String, String> fieldValueDict = getAuditRecord(cmd);
            String tableName = "";

            foreach (KeyValuePair<String, String> kvp in fieldValueDict)
            {
                tableName = kvp.Key.Substring(kvp.Key.IndexOf("-") + 1).Trim();
                break;
            }
            String[] tableNameComps = tableName.Split('.');
            if (tableNameComps!=null && tableNameComps.Length > 1)
                tableName = tableNameComps[tableNameComps.Length-1].Replace("[", "").Replace("]", "").Trim();

            Dictionary<String, String> contextFieldsNameList = getContextFieldNamesforTable(tableName);
            Dictionary<String, String> contextFieldsValues = getContextFieldValues(contextFieldsNameList, cmd);

            int rowsAffected = 0;

            //No need to make audit entry if there is no context field values
            if (contextFieldsValues != null && contextFieldsValues.Count > 0)
                foreach (KeyValuePair<String, String> kvp in fieldValueDict)
                {
                    AuditData auditObj = new AuditData();

                    auditObj.setField_value(kvp.Value);
                    auditObj.setField_name(kvp.Key.Substring(0, kvp.Key.IndexOf("-")));
                    //Insert the context field values and names for each of the audit entries
                    int counter = 1;
                    foreach (KeyValuePair<String, String> kvpVals in contextFieldsValues)
                    {
                        if (counter == 1)
                        {
                            auditObj.setContext_id1(kvpVals.Value);
                            auditObj.setContext_field_name1(kvpVals.Key);
                        }
                        if (counter == 2)
                        {
                            auditObj.setContext_id2(kvpVals.Value);
                            auditObj.setContext_field_name2(kvpVals.Key);
                        }
                        if (counter == 3)
                        {
                            auditObj.setContext_id3(kvpVals.Value);
                            auditObj.setContext_field_name3(kvpVals.Key);
                        }
                        counter++;
                    }

                    auditObj.setChnaged_by_user(user);
                    auditObj.setChanged_by_cmp(cmpId);
                    auditObj.setChange_date_time(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    auditObj.setTable_name(tableName);

                    rowsAffected += insertContextObject(auditObj);

                }

            return rowsAffected;
        }
        public static Dictionary<String, String> getContextFieldValues(Dictionary<String, String> contextFieldNamesCaseSensitive, String cmd)
        {
            Dictionary<String, String> returnDict = new Dictionary<String, String>();
            Dictionary<String, String> contextFieldNames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (KeyValuePair<String, String> kvp in contextFieldNamesCaseSensitive)
                contextFieldNames.Add(kvp.Key, kvp.Value);


            cmd = cmd.ToLower();
            bool commandInsert = (cmd.IndexOf("insert") >= 0 ? true : false);

            if (commandInsert)
            {
                //Get the field values list
                String[] fieldsArray = cmd.Substring(cmd.IndexOf("(") + 1, cmd.IndexOf(")") - cmd.IndexOf("(") - 1).Replace("[", "").Replace("]", "").Split(',');
                //Remove all single quotes from the query
                String valuesSubstring = cmd.Substring(cmd.IndexOf("values"));
                String[] fieldsValueArray = valuesSubstring.Substring(valuesSubstring.IndexOf("(") + 1, valuesSubstring.IndexOf(")") - valuesSubstring.IndexOf("(") - 1).Replace("'", "").Split(',');

                for (int i = 0; i < fieldsArray.Length; i++)
                {
                    if (fieldsArray[i].IndexOf(".") > 0)// remove the aliases
                        fieldsArray[i] = fieldsArray[i].Substring(fieldsArray[i].IndexOf(".") + 1).Trim();

                    if (contextFieldNames != null && contextFieldNames.ContainsKey(fieldsArray[i].Trim()))
                        returnDict.Add(fieldsArray[i].Trim(), fieldsValueArray[i].Trim());

                }
            }
            else
            {
                String[] fieldAndValueArray = cmd.Substring(cmd.IndexOf("set") + "set".Length, cmd.IndexOf("where") - cmd.IndexOf("set") - "set".Length - 1).Replace("(", "").
                    Replace(")", "").Replace("[", "").Replace("]", "").Replace("'", "").Split(',');

                for (int i = 0; i < fieldAndValueArray.Length; i++)
                {
                    String fieldName="";
                    String fieldValue="";

                    if (fieldAndValueArray[i].IndexOf("=") >= 0)
                    {
                        fieldName = fieldAndValueArray[i].Substring(0, fieldAndValueArray[i].IndexOf("=")).Trim();
                        fieldValue = fieldAndValueArray[i].Substring(fieldAndValueArray[i].IndexOf("=") + 1).Trim();
                        if (fieldName.IndexOf(".") > 0)
                            fieldName = fieldName.Substring(fieldName.IndexOf(".") + 1);

                        if (contextFieldNames != null && contextFieldNames.ContainsKey(fieldName))
                            returnDict.Add(fieldName, fieldValue);
                    }
                }
            }
            
            //Now check the where clause for context field and if the field value was already addedd in the dictionary then ignore it
            //The new value gets precedence
            if (cmd.IndexOf("where") >= 0)
            {
                String[] fieldAndValueArrayWhere = cmd.Substring(cmd.IndexOf("where") + "where".Length).Replace("(", "").Replace(")", "").Replace("[", "").Replace("]", "").Replace("'", "").Split(new String[] { "and" }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < fieldAndValueArrayWhere.Length; i++)
                {
                    String fieldName = fieldAndValueArrayWhere[i].Substring(0, fieldAndValueArrayWhere[i].IndexOf("=")).Trim();
                    String fieldValue = fieldAndValueArrayWhere[i].Substring(fieldAndValueArrayWhere[i].IndexOf("=") + 1).Trim();
                    if (fieldName.IndexOf(".") > 0)
                        fieldName = fieldName.Substring(fieldName.IndexOf(".") + 1);

                    if (contextFieldNames != null && contextFieldNames.ContainsKey(fieldName) && !returnDict.ContainsKey(fieldName))
                        returnDict.Add(fieldName, fieldValue);

                }
            }
            return returnDict;
        }
        /// <summary>
        /// This method parses the query string and returns the field name and value in a dictionary.
        /// All the values are returned as a string without enclosing quotes (if any).
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static Dictionary<String, String> getAuditRecord(String cmd)
        {
            Dictionary<String, String> returnDict = new Dictionary<String, String>();
            cmd = cmd.ToLower();
            bool commandInsert = (cmd.IndexOf("insert") >= 0 ? true : false);

            if (commandInsert)
            {
                //Get the field values list
                String[] fieldsArray = cmd.Substring(cmd.IndexOf("(") + 1, cmd.IndexOf(")") - cmd.IndexOf("(") - 1).Replace("[", "").Replace("]", "").Split(',');
                //Remove all single quotes from the query
                String valuesSubstring = cmd.Substring(cmd.IndexOf("values"));
                String[] fieldsValueArray = valuesSubstring.Substring(valuesSubstring.IndexOf("(") + 1, valuesSubstring.IndexOf(")") - valuesSubstring.IndexOf("(") - 1).Replace("'", "").Split(',');
                
                String tableName = cmd.Substring(cmd.IndexOf("insert into") + "insert into".Length, cmd.IndexOf("(") - cmd.IndexOf("insert into") - "insert into".Length).Replace("[", "").Replace("]", "");

                for (int i = 0; i < fieldsArray.Length; i++)
                {
                    if (fieldsArray[i].IndexOf(".") > 0)// remove the aliases
                        fieldsArray[i] = fieldsArray[i].Substring(fieldsArray[i].IndexOf(".") + 1).Trim();
                    returnDict.Add(fieldsArray[i] + "-" + tableName, fieldsValueArray[i].Trim());
                }
            }
            else
            {
                String[] fieldAndValueArray = cmd.Substring(cmd.IndexOf("set") + "set".Length, cmd.IndexOf("where") - cmd.IndexOf("set") - "set".Length - 1).Replace("(", "").
                    Replace(")", "").Replace("[", "").Replace("]", "").Replace("'", "").Split(',');
                String tableName = cmd.Substring(cmd.IndexOf("update") + "update".Length, cmd.IndexOf("set") - cmd.IndexOf("update") - "update".Length).Replace("[", "").
                    Replace("]", "");
                if(fieldAndValueArray!=null)
                for (int i = 0; i < fieldAndValueArray.Length; i++)
                {
                    if (fieldAndValueArray[i].IndexOf("=")>=0)
                    {
                        String fieldName = fieldAndValueArray[i].Substring(0, fieldAndValueArray[i].IndexOf("=")).Trim();
                        String fieldValue = fieldAndValueArray[i].Substring(fieldAndValueArray[i].IndexOf("=") + 1).Trim();
                        if (fieldName.IndexOf(".") > 0)
                            fieldName = fieldName.Substring(fieldName.IndexOf(".") + 1);

                        returnDict.Add(fieldName + "-" + tableName, fieldValue);
                    }
                }
            }

            return returnDict;
        }
        /// <summary>
        /// Returns an arraylist of AuditData objects sorted in descending order by change time stamp
        /// The 0 th indexed element of the arraylist contains the object with latest change time stamp and goes descedning thereafter.
        /// The second parameter is optional - should be used in situations where only the context ids might not be sufficient to point out the audit entries
        /// </summary>
        /// <param name="contextFieldValues"></param>
        /// <returns></returns>
        public static ArrayList getAuditRecordsForContextIdsDB(ArrayList contextFieldValues,String contextTable)
        {
            ArrayList returnList = new ArrayList();

            String selectCmd = " select " + AuditData.AUDIT_DATA_COL_CHANGE_DATE_TIME +
                                "," + AuditData.AUDIT_DATA_COL_CHANGED_BY_CMP +
                                "," + AuditData.AUDIT_DATA_COL_CHNAGED_BY_USER +
                                "," + AuditData.AUDIT_DATA_COL_FIELD_NAME +
                                "," + AuditData.AUDIT_DATA_COL_FIELD_VALUE +
                                "," + AuditData.AUDIT_DATA_COL_CONTEXT_ID1 +
                                "," + AuditData.AUDIT_DATA_COL_CONTEXT_ID2 +
                                "," + AuditData.AUDIT_DATA_COL_CONTEXT_ID3 +
                                "," + AuditData.AUDIT_DATA_COL_CONTEXT_FIELD_NAME1 +
                                "," + AuditData.AUDIT_DATA_COL_CONTEXT_FIELD_NAME2 +
                                "," + AuditData.AUDIT_DATA_COL_CONTEXT_FIELD_NAME3 +
                                "," + AuditData.AUDIT_DATA_COL_TABLE_NAME +
                                " from " + AuditData.AUDIT_DATA_TABLE + " where ";

                                for(int i=0;i<contextFieldValues.Count;i++)
                                {
                                    selectCmd+="("+"'"+contextFieldValues[i]+"'"+"="+AuditData.AUDIT_DATA_COL_CONTEXT_ID1+" or "+
                                        "'"+contextFieldValues[i]+"'"+"="+AuditData.AUDIT_DATA_COL_CONTEXT_ID2+" or "+
                                        "'"+contextFieldValues[i]+"'"+"="+AuditData.AUDIT_DATA_COL_CONTEXT_ID3+")";
                                    if (i < contextFieldValues.Count - 1)
                                        selectCmd += " and ";
                                }

                                if (contextTable != null && !contextTable.Equals(""))
                                    selectCmd += " and " + AuditData.AUDIT_DATA_COL_TABLE_NAME + "=" + "'" + contextTable + "'";

                                selectCmd += "order by change_date_time DESC";

                                Connections cn = new Connections();
            DataSet ds = cn.execQuery(selectCmd, sessionUser);
            DataTable dt = ds.Tables[0];
            int counter = 0;

            foreach (DataRow dr in dt.Rows)
            {
                AuditData auditDataObj = new AuditData();
                auditDataObj.setChange_date_time(dr[0].ToString());
                auditDataObj.setChanged_by_cmp(dr[1].ToString());
                auditDataObj.setChnaged_by_user(dr[2].ToString());
                auditDataObj.setField_name(dr[3].ToString());
                auditDataObj.setField_value(dr[4].ToString());
                auditDataObj.setContext_id1(dr[5].ToString());
                auditDataObj.setContext_id2(dr[6].ToString());
                auditDataObj.setContext_id3(dr[7].ToString());
                auditDataObj.setContext_field_name1(dr[8].ToString());
                auditDataObj.setContext_field_name2(dr[9].ToString());
                auditDataObj.setContext_field_name3(dr[10].ToString());
                auditDataObj.setTable_name(dr[11].ToString());

                returnList.Insert(counter, auditDataObj);
                counter++;
            }
            cn.disconnect();

            return returnList;
        }

        /// <summary>
        /// Returns a splitted string array based on the supplied delimiter.
        /// The exclusion start and end string are used to skip substrings even if that substring contains the delim.
        /// </summary>
        /// <param name="delim"></param>
        /// <param name="exclusionStartString"></param>
        /// <param name="exclusionEndString"></param>
        /// <returns></returns>
        public String[] splitWithExclusion(String targetString,String delim,String exclusionStartString, String exclusionEndString)
        {
            ArrayList listOfString = new ArrayList();
            int startIndex = -1;
            String bufferString="";

            for (int i = 0; i < targetString.Length; i++)
            {
                if (targetString[i].Equals(exclusionStartString))
                    startIndex = i;
                else if (targetString[i].Equals(exclusionEndString))
                {                    
                    listOfString.Add(targetString.Substring(startIndex + 1, i - startIndex - 1));
                    startIndex = -1;
                }
                else if (targetString[i].Equals(delim) && startIndex == -1)
                {
                    listOfString.Add(bufferString);
                    bufferString = "";
                }
                else
                    bufferString += targetString[i];
            }

            return (listOfString.ToArray(typeof(String)) as String[]);
        }
               
    }
        
}
