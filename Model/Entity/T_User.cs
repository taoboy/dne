/**  
* file: T_User.cs
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
	public partial class T_User : DBEntity
	{
		public T_User() { }

		public T_User(DataRow dr, String prefix)
		{

			if (dr.GetValue(prefix + "id") != null && !Convert.IsDBNull(dr[prefix + "id"]))
				this.id = Convert.ToString(dr.GetValue(prefix + "id"));

			if (dr.GetValue(prefix + "account") != null && !Convert.IsDBNull(dr[prefix + "account"]))
				this.account = Convert.ToString(dr.GetValue(prefix + "account"));

			if (dr.GetValue(prefix + "roleId") != null && !Convert.IsDBNull(dr[prefix + "roleId"]))
				this.roleId = Convert.ToInt32(dr.GetValue(prefix + "roleId"));

			if (dr.GetValue(prefix + "name") != null && !Convert.IsDBNull(dr[prefix + "name"]))
				this.name = Convert.ToString(dr.GetValue(prefix + "name"));

			if (dr.GetValue(prefix + "password") != null && !Convert.IsDBNull(dr[prefix + "password"]))
				this.password = Convert.ToString(dr.GetValue(prefix + "password"));

			if (dr.GetValue(prefix + "createTime") != null && !Convert.IsDBNull(dr[prefix + "createTime"]))
				this.createTime = Convert.ToDateTime(dr.GetValue(prefix + "createTime"));

			if (dr.GetValue(prefix + "phone") != null && !Convert.IsDBNull(dr[prefix + "phone"]))
				this.phone = Convert.ToString(dr.GetValue(prefix + "phone"));

			if (dr.GetValue(prefix + "dept") != null && !Convert.IsDBNull(dr[prefix + "dept"]))
				this.dept = Convert.ToString(dr.GetValue(prefix + "dept"));

			if (dr.GetValue(prefix + "descr") != null && !Convert.IsDBNull(dr[prefix + "descr"]))
				this.descr = Convert.ToString(dr.GetValue(prefix + "descr"));

			if (dr.GetValue(prefix + "status") != null && !Convert.IsDBNull(dr[prefix + "status"]))
				this.status = Convert.ToBoolean(dr.GetValue(prefix + "status"));
		}
 


		public String id { get; set; }

		public String account { get; set; }

		public Int32? roleId { get; set; }
		public T_Role roleIdEntity { get; set; } 
		public List<T_Role> roleIdEntitys { get; set; }

		public String name { get; set; }

		public String password { get; set; }

		public DateTime? createTime { get; set; }

		public String phone { get; set; }

		public String dept { get; set; }

		public String descr { get; set; }

		public Boolean? status { get; set; }

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
                
                _DBColumns["account"] = new DBColumn("account", DBType.String);

                c = new DBColumn("roleId", DBType.Numeric);
                c.isFK = true;
                c.FKTable = "T_Role";
                c.FKColumn = "id";
                c.FKText = "name";
                c.FKColumnNames = T_Role.ColumnNames;
                _DBColumns["roleId"] = c;
                
                _DBColumns["name"] = new DBColumn("name", DBType.String);
                
                _DBColumns["password"] = new DBColumn("password", DBType.String);
                
                _DBColumns["createTime"] = new DBColumn("createTime", DBType.DateTime);
                
                _DBColumns["phone"] = new DBColumn("phone", DBType.String);
                
                _DBColumns["dept"] = new DBColumn("dept", DBType.String);
                
                _DBColumns["descr"] = new DBColumn("descr", DBType.String);
                
                _DBColumns["status"] = new DBColumn("status", DBType.Bool);
                return _DBColumns;
            }
        }


        public enum Columns
        {
                id,
                account,
                roleId,
                name,
                password,
                createTime,
                phone,
                dept,
                descr,
                status
        }

        /// <summary>
        /// 获取该数据表的所有列名
        /// </summary>
        public static String[] ColumnNames
        {
            get
            {
                return Enum.GetNames(typeof(T_User.Columns));
            }
        }
 
	}
}