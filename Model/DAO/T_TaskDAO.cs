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

        //处理本类的所有属性，把String类型里面的单引号处理一下
        public void AntiSQLInjection(T_Task entity) {
            Type t = entity.GetType();
            foreach (System.Reflection.PropertyInfo p in t.GetProperties())
            {
                if (p.GetValue(entity, null) is String)
                {
                    String s = AntiInjection(p.GetValue(entity, null).ToString());
                    p.SetValue(entity, s, null);
                }
            }
        }

        public List<T_Task> DtToList(DataTable dt)
        {
            List<T_Task> entities = new List<T_Task>();
            foreach (DataRow dr in dt.Rows)
            {
                T_Task entity = new T_Task(dr, "");
                try
                {
                    if(entity.userId != null)
                        entity.userIdEntity = new T_User(dr, "userId_");
                    if(entity.statusId != null)
                        entity.statusIdEntity = new T_Task_Status(dr, "statusId_");
                }
                catch (Exception ex) { }
                entities.Add(entity);
            }
            return entities;
        }

        public int Add(T_Task entity)
        {
            AntiSQLInjection(entity);

            StringBuilder sql = new StringBuilder();

            sql.Append(" exec sp_executesql N'");
            sql.Append(" INSERT INTO [gdce_task].dbo.[T_Task] ( ");
            sql.Append(" [userId],");
            sql.Append(" [title],");
            sql.Append(" [content],");
            sql.Append(" [createTime],");
            sql.Append(" [beginTime],");
            sql.Append(" [endTime],");
            sql.Append(" [statusId],");
            sql.Remove(sql.Length - 1, 1);
            sql.Append(" ) values ( ");

            sql.Append(entity.userId != null ? "@userId," : "DEFAULT,");
            sql.Append(entity.title != null ? "@title," : "DEFAULT,");
            sql.Append(entity.content != null ? "@content," : "DEFAULT,");
            sql.Append(entity.createTime != null ? "@createTime," : "DEFAULT,");
            sql.Append(entity.beginTime != null ? "@beginTime," : "DEFAULT,");
            sql.Append(entity.endTime != null ? "@endTime," : "DEFAULT,");
            sql.Append(entity.statusId != null ? "@statusId," : "DEFAULT,");
            sql.Remove(sql.Length - 1, 1);
            sql.Append(" );select case when @@identity is not null then @@identity else 1 end as id;', N'");


            sql.Append(entity.userId != null ? "@userId nvarchar(max)," : "");
            sql.Append(entity.title != null ? "@title nvarchar(max)," : "");
            sql.Append(entity.content != null ? "@content nvarchar(max)," : "");
            sql.Append(entity.createTime != null ? "@createTime nvarchar(max)," : "");
            sql.Append(entity.beginTime != null ? "@beginTime nvarchar(max)," : "");
            sql.Append(entity.endTime != null ? "@endTime nvarchar(max)," : "");
            sql.Append(entity.statusId != null ? "@statusId nvarchar(max)," : "");
            sql.Remove(sql.Length - 1, 1);
            sql.Append("', ");

            sql.Append(entity.userId != null ? "@userId=N'" + entity.userId + "'," : "");
            sql.Append(entity.title != null ? "@title=N'" + entity.title + "'," : "");
            sql.Append(entity.content != null ? "@content=N'" + entity.content + "'," : "");
            sql.Append(entity.createTime != null ? "@createTime=N'" + entity.createTime + "'," : "");
            sql.Append(entity.beginTime != null ? "@beginTime=N'" + entity.beginTime + "'," : "");
            sql.Append(entity.endTime != null ? "@endTime=N'" + entity.endTime + "'," : "");
            sql.Append(entity.statusId != null ? "@statusId=N'" + entity.statusId + "'," : "");
            sql.Remove(sql.Length - 1, 1);

            return Convert.ToInt32(DBHelper.ExecuteDataTable(sql.ToString()).Rows[0][0]);
        }

        public T_Task GetById(Int32 id)
        {
            T_Task entity = new T_Task() { id = id };
            return GetByModel(entity, false);
        }

        public int Del(Int32 id)
        {
            String sql = " DELETE FROM [gdce_task].dbo.[T_Task] WHERE [id] = @id";
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("@id", id));
            return DBHelper.ExecuteNonQuery(sql, paras);
        }

        public int Update(T_Task entity)
        {
            AntiSQLInjection(entity);

            StringBuilder sql = new StringBuilder();

            sql.Append(" exec sp_executesql N'");
            sql.Append(" UPDATE [gdce_task].dbo.[T_Task] SET  ");
            sql.Append(" [userId] = @userId,");
            sql.Append(" [title] = @title,");
            sql.Append(" [content] = @content,");
            sql.Append(" [createTime] = @createTime,");
            sql.Append(" [beginTime] = @beginTime,");
            sql.Append(" [endTime] = @endTime,");
            sql.Append(" [statusId] = @statusId,");
            sql.Remove(sql.Length - 1, 1);
            sql.Append(" WHERE [id] = ''" + entity.id + "'' ', N'");

            sql.Append("@userId nvarchar(max),");
            sql.Append("@title nvarchar(max),");
            sql.Append("@content nvarchar(max),");
            sql.Append("@createTime nvarchar(max),");
            sql.Append("@beginTime nvarchar(max),");
            sql.Append("@endTime nvarchar(max),");
            sql.Append("@statusId nvarchar(max),");
            sql.Remove(sql.Length - 1, 1);
            sql.Append("', ");

            sql.Append("@userId=" + (entity.userId != null ? "N'" + entity.userId + "'," : "NULL,"));
            sql.Append("@title=" + (entity.title != null ? "N'" + entity.title + "'," : "NULL,"));
            sql.Append("@content=" + (entity.content != null ? "N'" + entity.content + "'," : "NULL,"));
            sql.Append("@createTime=" + (entity.createTime != null ? "N'" + entity.createTime + "'," : "NULL,"));
            sql.Append("@beginTime=" + (entity.beginTime != null ? "N'" + entity.beginTime + "'," : "NULL,"));
            sql.Append("@endTime=" + (entity.endTime != null ? "N'" + entity.endTime + "'," : "NULL,"));
            sql.Append("@statusId=" + (entity.statusId != null ? "N'" + entity.statusId + "'," : "NULL,"));
            sql.Remove(sql.Length - 1, 1);

            return DBHelper.ExecuteNonQuery(sql.ToString());
        }


        /// <summary>
        /// 以实体类作为查询条件获取符合条件的数据集，NULL值不作为查询条件
        /// </summary>
        public List<T_Task> Search(T_Task entity, int pageNum, int pageSize, String criteria, T_Task.Columns OrderBy, AscDesc AscDesc, out int totalRecords)
        {
            String sql = "";
            //如果没有任何查询条件，则不开启模糊查询列以减少SQL消耗
            if (String.IsNullOrWhiteSpace(criteria))
                sql = this.PreparedTable(entity, false, false);
            else
                sql = this.PreparedTable(entity, true, false);

            String tbl = "'(" + sql + ") as t'";
            List<SqlParameter> pars = new List<SqlParameter>();

            String crit = getSearchCriterias(criteria);
            String order = OrderBy.ToString() + " " + AscDesc.ToString();            

            //有待改进，没危险的int数据用了参数插入，有风险的字符串反而直接组装SQL，虽然做了简单的去单引号处理
            sql = String.Format("exec sp_paging {0},@pageNum,@pageSize,'{1}','{2}'", tbl, crit, order);

            pars.Add(new SqlParameter("@pageNum", pageNum));
            pars.Add(new SqlParameter("@pageSize", pageSize));
            DataTable dt = DBHelper.ExecuteDataTable(sql, pars);
            totalRecords = 0;
            if (dt.Rows.Count > 0)
                totalRecords = (int)dt.Rows[0]["TotalRecord"];
            return DtToList(dt);
        }


        /// <summary>
        /// 以实体类作为查询条件获取符合条件的数据集，NULL值不作为查询条件
        /// </summary>
        public List<T_Task> GetAllByModel(T_Task entity) {
            String sql = String.Format("exec sp_executesql N'{0}'", PreparedTable(entity, false, false));
            return DtToList(DBHelper.ExecuteDataTable(sql));
        }

        public T_Task GetByModel(T_Task entity)
        {
            String sql = String.Format("exec sp_executesql N'{0}'", PreparedTable(entity, false, true));

            DataTable dt = DBHelper.ExecuteDataTable(sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                entity = new T_Task(dt.Rows[0], "");
                if(entity.userId != null)
                    entity.userIdEntity = new T_User(dt.Rows[0], "userId_");
                if(entity.statusId != null)
                    entity.statusIdEntity = new T_Task_Status(dt.Rows[0], "statusId_");
                return entity;
            }
            else
                return null;
        }


        public T_Task GetByModel(T_Task entity, bool isLazy)
        {
            T_Task result = GetByModel(entity);

            if (result == null)//如果传入的实体为空（或者根据ID找不到）,则需要new一个空对象
                result = new T_Task();

            if (!isLazy) {
                using (T_UserDAO dao = new T_UserDAO())
                    result.userIdEntitys = dao.GetAllByModel(new T_User());
                using (T_Task_StatusDAO dao = new T_Task_StatusDAO())
                    result.statusIdEntitys = dao.GetAllByModel(new T_Task_Status());
            }
            return result;   
        }

        /// <summary>
        /// 用拼接SQL的方式，
        /// 把当前表以及其使用到的所有外键表链接起来，
        /// 拼接成一条SQL子查询，供分页的存储过程使用
        /// </summary>
        private String PreparedTable(T_Task entity, bool isFuzzySearch, bool isSignleResult)
        {
            String sql = getFKColumnsSQL("[gdce_task].dbo.T_Task", T_Task.DBColumns, isFuzzySearch, isSignleResult);
            
            
            if (entity == null)
            	return sql;		

            if (entity.id != null)
                sql += " and [gdce_task].dbo.T_Task.id = " + entity.id;

            if (entity.userId != null)
                sql += String.Format(" and [gdce_task].dbo.T_Task.userId = '{0}' ", AntiInjection(entity.userId));

            if (entity.title != null)
                sql += String.Format(" and [gdce_task].dbo.T_Task.title = '{0}' ", AntiInjection(entity.title));

            if (entity.content != null)
                sql += String.Format(" and [gdce_task].dbo.T_Task.content = '{0}' ", AntiInjection(entity.content));

            if (entity.createTime != null)
                sql += String.Format(" and [gdce_task].dbo.T_Task.createTime = '{0}' ", entity.createTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));            

            if (entity.beginTime != null)
                sql += String.Format(" and [gdce_task].dbo.T_Task.beginTime = '{0}' ", entity.beginTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));            

            if (entity.endTime != null)
                sql += String.Format(" and [gdce_task].dbo.T_Task.endTime = '{0}' ", entity.endTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));            

            if (entity.statusId != null)
                sql += " and [gdce_task].dbo.T_Task.statusId = " + entity.statusId;

            return AntiInjection(sql);
        }


    }
}
