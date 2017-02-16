/**  
* file: T_TaskDAO.cs
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
using System.Data;
using System.Text;
using System.Data.SqlClient;


namespace com.gdce_task.Model
{
    /// <summary>
    /// 为了追求SQL的灵活性，没有使用参数式的SQL语句，有注入风险，在前端接收参数的时候记得要做防注入处理！！！
    /// </summary>
    public partial class T_TaskDAO:DAO
    {
        public int updateStatus() {
            String sql = "";

            sql = "update T_Task set statusId=6 where id in (select id  from T_Task where endTime is not null and ((CONVERT(int, endTime,120))- (convert(int,getdate(),120)))<0);";
            
            
            
            
            return DBHelper.ExecuteNonQuery(sql);

        }

    }
}
