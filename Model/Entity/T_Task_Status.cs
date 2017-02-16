/**  
* file: T_Task_Status.cs
* Type: ORM Entity Class 
* Genreated date: 2016/8/1 11:22:16
* Host: kspc
* 
*┌───────────────────────────┐
*│　Copyright (c) 2015 Ks_Studio. All rights reserved.	 │
*└───────────────────────────┘
*/

using System;
using System.Data;
using System.Collections.Generic;

namespace com.gdce_task.Model
{
	public partial class T_Task_Status : DBEntity
	{
		public T_Task_Status() { }

		public T_Task_Status(DataRow dr, String prefix)
		{

			if (dr.GetValue(prefix + "id") != null && !Convert.IsDBNull(dr[prefix + "id"]))
				this.id = Convert.ToInt32(dr.GetValue(prefix + "id"));

			if (dr.GetValue(prefix + "name") != null && !Convert.IsDBNull(dr[prefix + "name"]))
				this.name = Convert.ToString(dr.GetValue(prefix + "name"));

			if (dr.GetValue(prefix + "descr") != null && !Convert.IsDBNull(dr[prefix + "descr"]))
				this.descr = Convert.ToString(dr.GetValue(prefix + "descr"));
		}
 


		public Int32? id { get; set; }

		public String name { get; set; }

		public String descr { get; set; }

        /// <summary>
        /// 记录该数据表下面的外键结构
        /// </summary>
        private static Dictionary<String,DBColumn> _DBColumns = null;
        public static Dictionary<String, DBColumn> DBColumns
        {
            get
            {
                _DBColumns = new Dictionary<String, DBColumn>();
                DBColumn c = null;
                
                c = new DBColumn("id", DBType.Numeric);
                c.isPK = true;
                _DBColumns["id"] = c;
                
                _DBColumns["name"] = new DBColumn("name", DBType.String);
                
                _DBColumns["descr"] = new DBColumn("descr", DBType.String);
                return _DBColumns;
            }
        }


        public enum Columns
        {
                id,
                name,
                descr
        }

        /// <summary>
        /// 获取该数据表的所有列名
        /// </summary>
        public static String[] ColumnNames
        {
            get
            {
                return Enum.GetNames(typeof(T_Task_Status.Columns));
            }
        }
 
	}
}