/**  
* file: T_Task_User.cs
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
	public partial class T_Task_User : DBEntity
	{
		public T_Task_User() { }

		public T_Task_User(DataRow dr, String prefix)
		{

			if (dr.GetValue(prefix + "id") != null && !Convert.IsDBNull(dr[prefix + "id"]))
				this.id = Convert.ToInt32(dr.GetValue(prefix + "id"));

			if (dr.GetValue(prefix + "taskId") != null && !Convert.IsDBNull(dr[prefix + "taskId"]))
				this.taskId = Convert.ToInt32(dr.GetValue(prefix + "taskId"));

			if (dr.GetValue(prefix + "userId") != null && !Convert.IsDBNull(dr[prefix + "userId"]))
				this.userId = Convert.ToString(dr.GetValue(prefix + "userId"));

			if (dr.GetValue(prefix + "descr") != null && !Convert.IsDBNull(dr[prefix + "descr"]))
				this.descr = Convert.ToString(dr.GetValue(prefix + "descr"));
		}
 


		public Int32? id { get; set; }

		public Int32? taskId { get; set; }
		public T_Task taskIdEntity { get; set; } 
		public List<T_Task> taskIdEntitys { get; set; }

		public String userId { get; set; }
		public T_User userIdEntity { get; set; } 
		public List<T_User> userIdEntitys { get; set; }

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

                c = new DBColumn("taskId", DBType.Numeric);
                c.isFK = true;
                c.FKTable = "T_Task";
                c.FKColumn = "id";
                c.FKText = "title";
                c.FKColumnNames = T_Task.ColumnNames;
                _DBColumns["taskId"] = c;

                c = new DBColumn("userId", DBType.Numeric);
                c.isFK = true;
                c.FKTable = "T_User";
                c.FKColumn = "id";
                c.FKText = "name";
                c.FKColumnNames = T_User.ColumnNames;
                _DBColumns["userId"] = c;
                
                _DBColumns["descr"] = new DBColumn("descr", DBType.String);
                return _DBColumns;
            }
        }


        public enum Columns
        {
                id,
                taskId,
                userId,
                descr
        }

        /// <summary>
        /// 获取该数据表的所有列名
        /// </summary>
        public static String[] ColumnNames
        {
            get
            {
                return Enum.GetNames(typeof(T_Task_User.Columns));
            }
        }
 
	}
}