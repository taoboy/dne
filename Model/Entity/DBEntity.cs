/**  
* file: DBEntity.cs
* Type: ORM Entity Class 
* Genreated date: 2016/8/1 11:22:16
* Host: kspc
* 
*┌───────────────────────────┐
*│　Copyright (c) 2015 Ks_Studio. All rights reserved.	 │
*└───────────────────────────┘
*/


using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace com.gdce_task.Model
{
    [Serializable]
    public partial class DBEntity { 
    
    }

    public static class DataRowExtensions
    {
        public static object GetValue(this DataRow row, string column)
        {
            return row.Table.Columns.Contains(column) ? row[column] : null;
        }
    }

    public enum DBType
    {
        String,
        Numeric,
        DateTime,
        Bool
    }

    public enum AscDesc
    {
        ASC,
        DESC
    }

}
