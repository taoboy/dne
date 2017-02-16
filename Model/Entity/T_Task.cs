/**  
* file: T_Task.cs
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
	public partial class T_Task : DBEntity
	{
		public T_Task() { }

		public T_Task(DataRow dr, String prefix)
		{

			if (dr.GetValue(prefix + "id") != null && !Convert.IsDBNull(dr[prefix + "id"]))
				this.id = Convert.ToInt32(dr.GetValue(prefix + "id"));

			if (dr.GetValue(prefix + "userId") != null && !Convert.IsDBNull(dr[prefix + "userId"]))
				this.userId = Convert.ToString(dr.GetValue(prefix + "userId"));

			if (dr.GetValue(prefix + "title") != null && !Convert.IsDBNull(dr[prefix + "title"]))
				this.title = Convert.ToString(dr.GetValue(prefix + "title"));

			if (dr.GetValue(prefix + "content") != null && !Convert.IsDBNull(dr[prefix + "content"]))
				this.content = Convert.ToString(dr.GetValue(prefix + "content"));

			if (dr.GetValue(prefix + "createTime") != null && !Convert.IsDBNull(dr[prefix + "createTime"]))
				this.createTime = Convert.ToDateTime(dr.GetValue(prefix + "createTime"));

			if (dr.GetValue(prefix + "beginTime") != null && !Convert.IsDBNull(dr[prefix + "beginTime"]))
				this.beginTime = Convert.ToDateTime(dr.GetValue(prefix + "beginTime"));

			if (dr.GetValue(prefix + "endTime") != null && !Convert.IsDBNull(dr[prefix + "endTime"]))
				this.endTime = Convert.ToDateTime(dr.GetValue(prefix + "endTime"));

			if (dr.GetValue(prefix + "statusId") != null && !Convert.IsDBNull(dr[prefix + "statusId"]))
				this.statusId = Convert.ToInt32(dr.GetValue(prefix + "statusId"));
		}
 


		public Int32? id { get; set; }

		public String userId { get; set; }
		public T_User userIdEntity { get; set; } 
		public List<T_User> userIdEntitys { get; set; }

		public String title { get; set; }

		public String content { get; set; }

		public DateTime? createTime { get; set; }

		public DateTime? beginTime { get; set; }

		public DateTime? endTime { get; set; }

		public Int32? statusId { get; set; }
		public T_Task_Status statusIdEntity { get; set; } 
		public List<T_Task_Status> statusIdEntitys { get; set; }

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

                c = new DBColumn("userId", DBType.Numeric);
                c.isFK = true;
                c.FKTable = "T_User";
                c.FKColumn = "id";
                c.FKText = "name";
                c.FKColumnNames = T_User.ColumnNames;
                _DBColumns["userId"] = c;
                
                _DBColumns["title"] = new DBColumn("title", DBType.String);
                
                _DBColumns["content"] = new DBColumn("content", DBType.String);
                
                _DBColumns["createTime"] = new DBColumn("createTime", DBType.DateTime);
                
                _DBColumns["beginTime"] = new DBColumn("beginTime", DBType.DateTime);
                
                _DBColumns["endTime"] = new DBColumn("endTime", DBType.DateTime);

                c = new DBColumn("statusId", DBType.Numeric);
                c.isFK = true;
                c.FKTable = "T_Task_Status";
                c.FKColumn = "id";
                c.FKText = "name";
                c.FKColumnNames = T_Task_Status.ColumnNames;
                _DBColumns["statusId"] = c;
                return _DBColumns;
            }
        }


        public enum Columns
        {
                id,
                userId,
                title,
                content,
                createTime,
                beginTime,
                endTime,
                statusId
        }

        /// <summary>
        /// 获取该数据表的所有列名
        /// </summary>
        public static String[] ColumnNames
        {
            get
            {
                return Enum.GetNames(typeof(T_Task.Columns));
            }
        }
 
	}
}