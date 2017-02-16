/**  
* file: T_Task_History.cs
* Type: ORM Entity Class 
* Genreated date: 2016/8/17 17:39:13
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
	public partial class T_Task_History : DBEntity
	{
		public T_Task_History() { }

		public T_Task_History(DataRow dr, String prefix)
		{

			if (dr.GetValue(prefix + "id") != null && !Convert.IsDBNull(dr[prefix + "id"]))
				this.id = Convert.ToInt32(dr.GetValue(prefix + "id"));

			if (dr.GetValue(prefix + "taskId") != null && !Convert.IsDBNull(dr[prefix + "taskId"]))
				this.taskId = Convert.ToInt32(dr.GetValue(prefix + "taskId"));

			if (dr.GetValue(prefix + "userName") != null && !Convert.IsDBNull(dr[prefix + "userName"]))
				this.userName = Convert.ToString(dr.GetValue(prefix + "userName"));

			if (dr.GetValue(prefix + "content") != null && !Convert.IsDBNull(dr[prefix + "content"]))
				this.content = Convert.ToString(dr.GetValue(prefix + "content"));

			if (dr.GetValue(prefix + "descr") != null && !Convert.IsDBNull(dr[prefix + "descr"]))
				this.descr = Convert.ToString(dr.GetValue(prefix + "descr"));

			if (dr.GetValue(prefix + "createTime") != null && !Convert.IsDBNull(dr[prefix + "createTime"]))
				this.createTime = Convert.ToDateTime(dr.GetValue(prefix + "createTime"));

			if (dr.GetValue(prefix + "userId") != null && !Convert.IsDBNull(dr[prefix + "userId"]))
				this.userId = Convert.ToString(dr.GetValue(prefix + "userId"));
		}
 


		public Int32? id { get; set; }

		public Int32? taskId { get; set; }
		public T_Task taskIdEntity { get; set; } 
		public List<T_Task> taskIdEntitys { get; set; }

		public String userName { get; set; }

		public String content { get; set; }

		public String descr { get; set; }

		public DateTime? createTime { get; set; }

		public String userId { get; set; }

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
                
                _DBColumns["userName"] = new DBColumn("userName", DBType.String);
                
                _DBColumns["content"] = new DBColumn("content", DBType.String);
                
                _DBColumns["descr"] = new DBColumn("descr", DBType.String);
                
                _DBColumns["createTime"] = new DBColumn("createTime", DBType.DateTime);
                
                _DBColumns["userId"] = new DBColumn("userId", DBType.String);
                return _DBColumns;
            }
        }


        public enum Columns
        {
                id,
                taskId,
                userName,
                content,
                descr,
                createTime,
                userId
        }

        /// <summary>
        /// 获取该数据表的所有列名
        /// </summary>
        public static String[] ColumnNames
        {
            get
            {
                return Enum.GetNames(typeof(T_Task_History.Columns));
            }
        }
 
	}
}