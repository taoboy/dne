/**  
* file: DBColumn.cs
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

	[Serializable]
	public class DBColumn
	{

        public DBColumn(String columnName, DBType type)
        {
            this.Name = columnName;
            this.DBType = type;
        }

        public String Name { get; set; }
        public bool isFK { get; set; }
        public bool isPK { get; set; }
        public bool isCal { get; set; }
        public String FKTable { get; set; }
        public String FKColumn { get; set; }
        public String FKText { get; set; }
        public DBType DBType { get; set; }
        public String[] FKColumnNames { get; set; }
	}
}