/**  
* file: DAO.cs
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
    public partial class DAO : IDisposable
    {
        public void Dispose() { }
 
        /// <summary>
        /// 组装前端传递过来的查询条件，做了简单的单引号处理以防SQL注入
        /// </summary>
        protected String getSearchCriterias(String criteria)
        {
            if (String.IsNullOrWhiteSpace(criteria))
                return "";

            String result = " ";
            String[] terms = criteria.Trim().Split(' ');
            foreach (String term in terms)
            {
                if(!String.IsNullOrWhiteSpace(term))
                    result += " and search_criteria like '%" + AntiInjection(term) + "%' \r\n ";
            }
            return AntiInjection(result);
        }


        /// <summary>
        /// 组装获取某个表的所有外键表的列SQL的方法
        /// </summary>
        /// <param name="DBColumns">某个表的所有列信息，比如XXX.DBColumns</param>
        /// <param name="isFuzzySearch">模糊查询开关，仅在需要模糊查询的时候组装临时查询字段，供前端跨字段查询使用</param>
        protected String getFKColumnsSQL(String tableName, Dictionary<String,DBColumn> DBColumns, bool isFuzzySearch, bool isSignleResult)
        {
            //要查询的列集合
            String columnSql = "select " + (isSignleResult ? " top 1 " : " ") + tableName + ".* \n";
            //动态组装的条件列
            String criteriaSql = " , (";
            //外键表
            String leftJoinSql = "";

            int Count = 0;
            foreach (DBColumn c in DBColumns.Values)
            {
                Count++;
                if (c.isFK)
                {
                    leftJoinSql += String.Format(" left join {0} as {0}{3} on {0}{3}.{1} = {4}.{2} \r\n", c.FKTable, c.FKColumn, c.Name, Count, tableName);
                    foreach (String col in c.FKColumnNames)
                    {
                        //外键列加上前缀以跟主表数据区分开
                        columnSql += String.Format(", {0}{3}.{1} as {2}_{1} \r\n", c.FKTable, col, c.Name, Count);
                    }
                }

                switch (c.DBType)
                {
                    case DBType.String:
                        criteriaSql += String.Format("\r\n case when {0}.{1} is null then ' ' else {0}.{1} end +", tableName, c.Name);
                        break;
                    case DBType.DateTime:
                        criteriaSql += String.Format("\r\n case when {0}.{1} is null then ' ' else convert(varchar(50),{0}.{1},20) end +", tableName, c.Name);
                        break;
                    case DBType.Numeric:
                        if (c.isFK)//如果是外键则组装外键列
                            criteriaSql += String.Format("\r\n case when {0}{2}.{1} is null then ' ' else {0}{2}.{1} end +", c.FKTable, c.FKText, Count);
                        else if(!c.isPK)//如果是非主键的数字列则转换成varchar
                            criteriaSql += String.Format("\r\n case when {0}.{1} is null then ' ' else cast({0}.{1} as varchar(20)) end +", tableName, c.Name);
                        break;
                }
                
            }

            criteriaSql = criteriaSql.TrimEnd('+') + ")\r\n as search_criteria \r\n";

            String sql = "";
            sql += columnSql;

            //仅在需要模糊查询的时候组装临时查询字段，以供前端跨字段查询使用
            if(isFuzzySearch)
                sql += criteriaSql;
            sql += "from "+tableName+" \r\n";
            sql += leftJoinSql;
            sql += " where 1=1 \r\n";

            return sql;
        }
        
        public String AntiInjection(String str)
        {
            //把将要加入到SQL里面的单引号替换成两个单引号，把注释替换成空格
            return str
                .Replace("'", "''");
        }

        /// <summary>
        /// MD5 32位加密
        /// </summary>
        public string GetMD5(string str)
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
        protected string DataTableToJson(DataTable dt)
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
        public string ListToJson<T>(IList<T> IL)
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

        /*
 //旧的组装查询条件的方法，实现多关键字跨字段模糊查询，缺点是：
 //比如这里查“案例”可以查出外键“文章类别”是“案例”的那些数据
 //但是但是再加一个“系统”作为条件的话，就没有返回数据了，不能实现跨字段的多关键字
 //仅仅实现了“多个单字段”的多关键字查询，不知道我说得是否清楚
 protected String getSearchCriterias(String criteria, Dictionary<String, DBColumn> columns)
 {
     if (String.IsNullOrWhiteSpace(criteria) || columns == null || columns.Count == 0) {
         return "";
     }

     String result = " and \r\n (";
     String[] terms = criteria.Trim().Split(' ');
     foreach (String key in columns.Keys) {
         DBColumn column = columns[key];
         if(column.DBType == DBType.String)
             result += getSearchCriteria(column.Name, terms);

         if (!String.IsNullOrEmpty(column.FKTable))
             result += getSearchCriteria(column.Name + "_" + column.FKText, terms);
     }
     result = result.Substring(0, result.Length - 2);
     result += " )\r\n ";
     return AntiInjection(result);
 }

 private String getSearchCriteria(String columnName, String[] terms) {
     String result = " (";
     foreach (String term in terms)
     {
         result += String.Format(" {0} like '%{1}%' and", columnName, term);
     }
     result = result.Substring(0, result.Length - 3);//去掉最后的and
     result += ")\r\n or";

     return result;
 }
  * 
  * 
 /// <summary>
 /// 组装需要用来进行模糊查询的SQL
 /// </summary>
 protected String getJoinedStringColumn(String tableName, Dictionary<String, DBColumn> columns) {
     String result = " , (";

     foreach (String key in columns.Keys) {
         DBColumn column = columns[key];
         switch (column.DBType)
         {
             case DBType.String:
                 result += String.Format(" case when {0}.{1} is null then ' ' else {0}.{1} end +", tableName, column.Name);
                 break;
             case DBType.DateTime:
                 result += String.Format(" case when {0}.{1} is null then ' ' else convert(varchar(50),{0}.{1},20) end +", tableName, column.Name);
                 break;
             case DBType.Numeric:
                 if (column.isFK)
                     result += String.Format(" case when {0}.{1} is null then ' ' else {0}.{1} end +", column.FKTable, column.FKText);
                 else
                     result += String.Format(" case when {0}.{1} is null then ' ' else cast({0}.{1} as varchar(20)) end +", tableName, column.Name);
                 break;
         }
     }
     result = result.TrimEnd('+') + ") as search_criteria ";

     return result;
 }
 */

    }
}