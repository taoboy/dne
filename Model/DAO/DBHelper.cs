/**  
* file: DBHelper.cs
* Type: ORM DAO Class 
* Genreated date: 2016/8/1 11:22:16
* Host: kspc
* 
*┌───────────────────────────┐
*│　Copyright (c) 2015 Ks_Studio. All rights reserved.	 │
*└───────────────────────────┘
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Security.Cryptography;
using System.Reflection;


namespace com.gdce_task.Model
{
    public static class DBHelper
    {      //连接字符串，从web.config中读取
        public static String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["connStr"].ToString();
        //@"data source=.\sql08;uid=sa;pwd=jsjx502Admin;database=GUPT_HRMS";

        /// <summary>
        /// 执行非查询型的sql语句，返回受影响的行数，用SQL组装方式
        /// </summary>
        public static int ExecuteNonQuery(String sql)
        {
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();

            sql = "exec sp_executesql N'" + sql.Replace("'", "''") + "'";

            SqlCommand comm = new SqlCommand(sql, conn);
            int result = comm.ExecuteNonQuery();

            conn.Close();
            return result;
        }

        /// <summary>
        /// 执行查询型的sql，返回一个DataTable，用SQL组装方式
        /// </summary>
        public static DataTable ExecuteDataTable(String sql)
        {
            DataTable dt = new DataTable();

            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();

            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            da.Fill(dt);

            conn.Close();
            return dt;
        }
        

        /// <summary>
        /// 执行非查询型的sql语句，返回受影响的行数，用SQL Parameters方式
        /// </summary>
        public static int ExecuteNonQuery(string SQLString, List<SqlParameter> cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        int result = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();

                        return result;
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        throw e;
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询型的sql，返回一个DataTable，用SQL Parameters方式
        /// </summary>
        public static DataTable ExecuteDataTable(string SQLString, List<SqlParameter> cmdParms)
        {
            using (SqlConnection connection = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    try
                    {
                        da.Fill(dt);
                        cmd.Parameters.Clear();
                    }
                    catch (System.Data.SqlClient.SqlException ex)
                    {
                        //实际生产系统记得注释掉这里，一般不显示SQL错误
                        throw new Exception(ex.Message);
                    }
                    return dt;
                }
            }
        }

        /// <summary>
        /// 组装SQL Parameters的方法
        /// </summary>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, List<SqlParameter> cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (SqlParameter parameter in cmdParms)
                {
                    cmd.Parameters.Add(parameter);
                }
            }
        }

        /// <summary>
        /// MD5 32位加密
        /// </summary>
        static public string GetMD5(string str)
        {
            string cl = str;
            string pwd = "";
            MD5 md5 = MD5.Create();//实例化一个md5对像
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                String c = s[i].ToString("x");
                pwd = pwd + (c.Length == 1 ? "0" + c : c);
            }
            return pwd;
        }

        /// <summary>
        /// DataTable转成Json
        /// </summary>
        public static string DataTableToJson(DataTable dt)
        {
            StringBuilder Json = new StringBuilder();
            //Json.Append("{\"" + jsonName + "\":[");
            Json.Append("[");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Json.Append("{");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        Json.Append("\"" + dt.Columns[j].ColumnName.ToString() + "\":\"" + dt.Rows[i][j].ToString() + "\"");
                        if (j < dt.Columns.Count - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < dt.Rows.Count - 1)
                    {
                        Json.Append(",\r\n");
                    }
                }
            }
            Json.Append("]");
            return Json.ToString();
        }

        //List转成json
        public static string ListToJson<T>(IList<T> IL)
        {
            StringBuilder Json = new StringBuilder();
            //Json.Append("{\"" + jsonName + "\":[");
            Json.Append("[");
            if (IL.Count > 0)
            {
                for (int i = 0; i < IL.Count; i++)
                {
                    T obj = Activator.CreateInstance<T>();
                    Type type = obj.GetType();
                    PropertyInfo[] pis = type.GetProperties();
                    Json.Append("{");
                    for (int j = 0; j < pis.Length; j++)
                    {
                        Json.Append("\"" + pis[j].Name.ToString() + "\":\"" + pis[j].GetValue(IL[i], null) + "\"");
                        if (j < pis.Length - 1)
                        {
                            Json.Append(",");
                        }
                    }
                    Json.Append("}");
                    if (i < IL.Count - 1)
                    {
                        Json.Append(",\r\n");
                    }
                }
            }
            Json.Append("]");
            return Json.ToString();
        }

        /// <summary>
        /// 获取json节点的值
        /// 10/20/显康新增
        /// </summary>
        /// <param name="jsonStr">json串</param>
        /// <param name="key">节点的值</param>
        /// <returns>节点的值</returns>
        public static string GetJsonValue(string jsonStr, string key)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(jsonStr))
            {

                key = "\"" + key.Trim('"') + "\"";
                int index = jsonStr.IndexOf(key) + key.Length + 1;
                if (index > key.Length + 1)
                {
                    //先截逗号，若是最后一个，截“｝”号，取最小值
                    int end = jsonStr.IndexOf(',', index);
                    if (end == -1)
                    {
                        end = jsonStr.IndexOf('}', index);
                    }

                    result = jsonStr.Substring(index, end - index);
                    result = result.Trim(new char[] { '"', ' ', '\'' }); //过滤引号或空格
                }
            }
            return result;
        }

        /*
/// <summary>
/// 组装获取某个表的所有外键表的列SQL的方法
/// </summary>
/// <param name="DBColumns">某个表的所有列信息，比如XXX.DBColumns</param>
public static String getFKColumnsSQL(Dictionary<String,DBColumn> DBColumns)
{
    String sql = "";
    int Count = 0;
    foreach (DBColumn c in DBColumns.Values)
    {
        Count++;
        if (c.isFK)
            foreach (String col in c.FKColumnNames)
            {
                //外键列加上前缀以跟主表数据区分开
                sql += String.Format(", {0}{3}.{1} as {2}_{1} \n", c.FKTable, col, c.Name, Count);
            }
    }
    return sql;
}

public static String getFKColumnsSQL1(Dictionary<String, DBColumn> DBColumns)
{
    String sql = getFKColumnsSQL(DBColumns);
    return sql == "" ? "," : sql;
}

/// <summary>
/// 组装获取某个表的所有外键表的列SQL的方法
/// </summary>
/// <param name="DBColumns">某个表的所有列信息，比如XXX.DBColumns</param>
public static String getFKColumnsSQL2(Dictionary<String, DBColumn> DBColumns)
{
    String sql = ",";
    int Count = 0;
    foreach (DBColumn c in DBColumns.Values)
    {
        Count++;
        if (c.isFK)
            foreach (String col in c.FKColumnNames)
            {
                //外键列加上前缀以跟主表数据区分开
                String temp = c.FKTable + Count.ToString() + "." + col;
                //sql += String.Format(", {0}{3}.{1} as {2}_{0}_{1} \n", c.FKTable, col, c.Name, DBColumns.IndexOf(c));
                sql += String.Format("\n ' ' + case when {0} is not null then cast({0} as varchar(800)) else ' ' end +", temp);
            }
    }
    return sql.TrimEnd(',').TrimEnd('+');
}


/// <summary>
/// 组装Left Join某个表的所有外键表的SQL的方法
/// </summary>
/// <param name="DBColumns">某个表的所有列信息，比如XXX.DBColumns</param>
public static String getFKLeftJoinSQL(String tableName, Dictionary<String, DBColumn> DBColumns)
{
    String sql = "";
    int Count = 0; 
    foreach (DBColumn c in DBColumns.Values)
    {
        Count++;
        if (c.isFK)
            sql += String.Format(" left join {0} as {0}{3} on {0}{3}.{1} = {4}.{2} \n", c.FKTable, c.FKColumn, c.Name, Count, tableName);
    }
    return sql;
}


/// <summary>
/// 提供给web前端用的search方法
/// </summary>
public static DataTable WebSearch(String tbl_name, Dictionary<String, DBColumn> DBColumns, PropertyInfo[] ClassProperties, int pageNum, int pageSize, String search_criteria, String orderBy, out int totalRecords)
{
    search_criteria = search_criteria.Trim();
    String criteria = "";//构建查询条件
    String[] terms = search_criteria.Split(new char[] { ' ' });
    for (int i = 0; i < terms.Length; i++)
    {
        terms[i] = terms[i].Trim();
        if (!String.IsNullOrEmpty(terms[i]))
            criteria += String.Format(" and search_creteria like '%{0}%' ", terms[i]);
    }

    String tbl = "";//用子查询代替存储过程里面的表名
    tbl += " (select " + tbl_name + ".* \n";
    tbl += getFKColumnsSQL1(DBColumns);
    tbl += getFKColumnsSQL2(DBColumns);
    //tbl += "\n,' ' ";
    foreach (System.Reflection.PropertyInfo p in ClassProperties)
    {
        //遍历类的属性，根据属性的类型判断如何进行类型转换，把所有字段拼接成一个文本字段，用于全文查询
        String t = p.PropertyType.ToString();
        if (t.Equals("System.String"))
        {
            tbl += String.Format("+' '+case when {1}.{0} is null then ' ' else {1}.{0} end ", p.Name, tbl_name);
        }
        else if (t.Contains("Int"))
        {
            tbl += String.Format("+' '+case when {1}.{0} is null then ' ' else cast({1}.{0} as varchar(50)) end ", p.Name, tbl_name);
        }
        else if (t.Contains("DateTime"))
        {
            tbl += String.Format("+' '+case when {1}.{0} is null then ' ' else convert(varchar(50),{1}.{0},20) end ", p.Name, tbl_name);
        }
    }
    tbl += " as search_creteria from " + tbl_name + DBHelper.getFKLeftJoinSQL(tbl_name, DBColumns) + ") as t ";

    if (String.IsNullOrEmpty(orderBy.Trim()))
    {
        return DBHelper.GetDataTableByPage(tbl, pageNum, pageSize, criteria, out totalRecords);
    }
    else
    {
        return DBHelper.GetDataTableByPage(tbl, pageNum, pageSize, criteria, orderBy, out totalRecords);
    }
}
                
public static DataTable GetDataTableByPage(String viewName)
{
    String sql = "exec sp_paging @viewName";
    List<SqlParameter> paras = new List<SqlParameter>();
    paras.Add(new SqlParameter("@viewName", viewName));
    return ExecuteDataTable(sql, paras);
}


public static DataTable GetDataTableByPage(String viewName, int pageNum, out int totalRecords)
{
    String sql = "exec sp_paging @viewName,@pageNum";
    List<SqlParameter> paras = new List<SqlParameter>();
    paras.Add(new SqlParameter("@viewName", viewName));
    paras.Add(new SqlParameter("@pageNum", pageNum));
    DataTable dt = ExecuteDataTable(sql, paras);
    totalRecords = 0;
    if (dt.Rows.Count > 0)
        totalRecords = (int)dt.Rows[0]["TotalRecord"];
    return dt;
}


public static DataTable GetDataTableByPage(String viewName, int pageNum, int pageSize, out int totalRecords)
{
    String sql = "exec sp_paging @viewName,@pageNum,@pageSize";
    List<SqlParameter> paras = new List<SqlParameter>();
    paras.Add(new SqlParameter("@viewName", viewName));
    paras.Add(new SqlParameter("@pageNum", pageNum));
    paras.Add(new SqlParameter("@pageSize", pageSize));
    DataTable dt = ExecuteDataTable(sql, paras);
    totalRecords = 0;
    if (dt.Rows.Count > 0)
        totalRecords = (int)dt.Rows[0]["TotalRecord"];
    return dt;
}


public static DataTable GetDataTableByPage(String viewName, int pageNum, int pageSize, String strWhere, out int totalRecords)
{
    String sql = "exec sp_paging @viewName,@pageNum,@pageSize,@strWhere";
    List<SqlParameter> paras = new List<SqlParameter>();
    paras.Add(new SqlParameter("@viewName", viewName));
    paras.Add(new SqlParameter("@pageNum", pageNum));
    paras.Add(new SqlParameter("@pageSize", pageSize));
    paras.Add(new SqlParameter("@strWhere", strWhere));
    DataTable dt = ExecuteDataTable(sql, paras);
    totalRecords = 0;
    if (dt.Rows.Count > 0)
        totalRecords = (int)dt.Rows[0]["TotalRecord"];
    return dt;
}

public static DataTable GetDataTableByPage(String viewName, int pageNum, int pageSize, String strWhere, String orderBy, out int totalRecords)
{
    String sql = "exec sp_paging @viewName,@pageNum,@pageSize,@strWhere,@orderBy";
    List<SqlParameter> paras = new List<SqlParameter>();
    paras.Add(new SqlParameter("@viewName", viewName));
    paras.Add(new SqlParameter("@pageNum", pageNum));
    paras.Add(new SqlParameter("@pageSize", pageSize));
    paras.Add(new SqlParameter("@strWhere", strWhere));
    paras.Add(new SqlParameter("@orderBy", orderBy));
    DataTable dt = ExecuteDataTable(sql, paras);
    totalRecords = 0;
    if (dt.Rows.Count > 0)
        totalRecords = (int)dt.Rows[0]["TotalRecord"];
    return dt;
}


public static String GetSearchCriteria(String column, String criteria, bool isAccurate)
{
    if (String.IsNullOrEmpty(criteria))
        return "";

    //criteria = SQLInjectionFilter(criteria);

    if (isAccurate)
        return String.Format(" and {0} = '{1}' ", column, criteria);

    criteria = criteria.Trim();
    String result = "";
    char[] spliter = { ' ' };
    String[] terms = criteria.Split(spliter);
    for (int i = 0; i < terms.Length; i++)
    {
        terms[i] = terms[i].Trim();
        if (!String.IsNullOrEmpty(terms[i]))
            result += String.Format(" and {0} like '%{1}%' ", column, terms[i]);
    }
    return result;
}

public static String GetSearchCriteria(String column, Int32? criteria, bool isAccurate)
{
    String result = "";
    if (criteria == null)
        return "";
    result += String.Format(" and {0} = {1} ", column, criteria);
    return result;
}

public static String GetSearchCriteria(String column, DateTime? criteria, bool isAccurate)
{
    String result = "";
    if (criteria == null)
        return "";
    result += String.Format(" and CONVERT(varchar(50), {0}, 23) = '{1}' ", column, criteria.Value.ToString("yyyy-MM-dd"));
    return result;
}

public static String GetSearchCriteria(String column, Double? criteria, bool isAccurate)
{
    String result = "";
    if (criteria == null)
        return "";
    result += String.Format(" and {0} = {1} ", column, criteria);
    return result;
}

public static String GetSearchCriteria(String column, Boolean? criteria, bool isAccurate)
{
    String result = "";
    if (criteria == null)
        return "";
    result += String.Format(" and {0} = {1} ", column, Convert.ToInt16(criteria));
    return result;
}
*/


    }
}