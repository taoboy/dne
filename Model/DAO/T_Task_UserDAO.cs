/**  
* file: T_Task_UserDAO.cs
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
    public partial class T_Task_UserDAO:DAO
    {

        //处理本类的所有属性，把String类型里面的单引号处理一下
        public void AntiSQLInjection(T_Task_User entity) {
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

        public List<T_Task_User> DtToList(DataTable dt)
        {
            List<T_Task_User> entities = new List<T_Task_User>();
            foreach (DataRow dr in dt.Rows)
            {
                T_Task_User entity = new T_Task_User(dr, "");
                try
                {
                    if(entity.taskId != null)
                        entity.taskIdEntity = new T_Task(dr, "taskId_");
                    if(entity.userId != null)
                        entity.userIdEntity = new T_User(dr, "userId_");
                }
                catch (Exception ex) { }
                entities.Add(entity);
            }
            return entities;
        }

        public int Add(T_Task_User entity)
        {
            AntiSQLInjection(entity);

            StringBuilder sql = new StringBuilder();

            sql.Append(" exec sp_executesql N'");
            sql.Append(" INSERT INTO [gdce_task].dbo.[T_Task_User] ( ");
            sql.Append(" [taskId],");
            sql.Append(" [userId],");
            sql.Append(" [descr],");
            sql.Remove(sql.Length - 1, 1);
            sql.Append(" ) values ( ");

            sql.Append(entity.taskId != null ? "@taskId," : "DEFAULT,");
            sql.Append(entity.userId != null ? "@userId," : "DEFAULT,");
            sql.Append(entity.descr != null ? "@descr," : "DEFAULT,");
            sql.Remove(sql.Length - 1, 1);
            sql.Append(" );select case when @@identity is not null then @@identity else 1 end as id;', N'");


            sql.Append(entity.taskId != null ? "@taskId nvarchar(max)," : "");
            sql.Append(entity.userId != null ? "@userId nvarchar(max)," : "");
            sql.Append(entity.descr != null ? "@descr nvarchar(max)," : "");
            sql.Remove(sql.Length - 1, 1);
            sql.Append("', ");

            sql.Append(entity.taskId != null ? "@taskId=N'" + entity.taskId + "'," : "");
            sql.Append(entity.userId != null ? "@userId=N'" + entity.userId + "'," : "");
            sql.Append(entity.descr != null ? "@descr=N'" + entity.descr + "'," : "");
            sql.Remove(sql.Length - 1, 1);

            return Convert.ToInt32(DBHelper.ExecuteDataTable(sql.ToString()).Rows[0][0]);
        }

        public T_Task_User GetById(Int32 id)
        {
            T_Task_User entity = new T_Task_User() { id = id };
            return GetByModel(entity, false);
        }

        public int Del(Int32 id)
        {
            String sql = " DELETE FROM [gdce_task].dbo.[T_Task_User] WHERE [id] = @id";
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("@id", id));
            return DBHelper.ExecuteNonQuery(sql, paras);
        }

        public int Update(T_Task_User entity)
        {
            AntiSQLInjection(entity);

            StringBuilder sql = new StringBuilder();

            sql.Append(" exec sp_executesql N'");
            sql.Append(" UPDATE [gdce_task].dbo.[T_Task_User] SET  ");
            sql.Append(" [taskId] = @taskId,");
            sql.Append(" [userId] = @userId,");
            sql.Append(" [descr] = @descr,");
            sql.Remove(sql.Length - 1, 1);
            sql.Append(" WHERE [id] = ''" + entity.id + "'' ', N'");

            sql.Append("@taskId nvarchar(max),");
            sql.Append("@userId nvarchar(max),");
            sql.Append("@descr nvarchar(max),");
            sql.Remove(sql.Length - 1, 1);
            sql.Append("', ");

            sql.Append("@taskId=" + (entity.taskId != null ? "N'" + entity.taskId + "'," : "NULL,"));
            sql.Append("@userId=" + (entity.userId != null ? "N'" + entity.userId + "'," : "NULL,"));
            sql.Append("@descr=" + (entity.descr != null ? "N'" + entity.descr + "'," : "NULL,"));
            sql.Remove(sql.Length - 1, 1);

            return DBHelper.ExecuteNonQuery(sql.ToString());
        }


        /// <summary>
        /// 以实体类作为查询条件获取符合条件的数据集，NULL值不作为查询条件
        /// </summary>
        public List<T_Task_User> Search(T_Task_User entity, int pageNum, int pageSize, String criteria, T_Task_User.Columns OrderBy, AscDesc AscDesc, out int totalRecords)
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
        public List<T_Task_User> GetAllByModel(T_Task_User entity) {
            String sql = String.Format("exec sp_executesql N'{0}'", PreparedTable(entity, false, false));
            return DtToList(DBHelper.ExecuteDataTable(sql));
        }

        public T_Task_User GetByModel(T_Task_User entity)
        {
            String sql = String.Format("exec sp_executesql N'{0}'", PreparedTable(entity, false, true));

            DataTable dt = DBHelper.ExecuteDataTable(sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                entity = new T_Task_User(dt.Rows[0], "");
                if(entity.taskId != null)
                    entity.taskIdEntity = new T_Task(dt.Rows[0], "taskId_");
                if(entity.userId != null)
                    entity.userIdEntity = new T_User(dt.Rows[0], "userId_");
                return entity;
            }
            else
                return null;
        }


        public T_Task_User GetByModel(T_Task_User entity, bool isLazy)
        {
            T_Task_User result = GetByModel(entity);

            if (result == null)//如果传入的实体为空（或者根据ID找不到）,则需要new一个空对象
                result = new T_Task_User();

            if (!isLazy) {
                using (T_TaskDAO dao = new T_TaskDAO())
                    result.taskIdEntitys = dao.GetAllByModel(new T_Task());
                using (T_UserDAO dao = new T_UserDAO())
                    result.userIdEntitys = dao.GetAllByModel(new T_User());
            }
            return result;   
        }

        /// <summary>
        /// 用拼接SQL的方式，
        /// 把当前表以及其使用到的所有外键表链接起来，
        /// 拼接成一条SQL子查询，供分页的存储过程使用
        /// </summary>
        private String PreparedTable(T_Task_User entity, bool isFuzzySearch, bool isSignleResult)
        {
            String sql = getFKColumnsSQL("[gdce_task].dbo.T_Task_User", T_Task_User.DBColumns, isFuzzySearch, isSignleResult);
            
            
            if (entity == null)
            	return sql;		

            if (entity.id != null)
                sql += " and [gdce_task].dbo.T_Task_User.id = " + entity.id;

            if (entity.taskId != null)
                sql += " and [gdce_task].dbo.T_Task_User.taskId = " + entity.taskId;

            if (entity.userId != null)
                sql += String.Format(" and [gdce_task].dbo.T_Task_User.userId = '{0}' ", AntiInjection(entity.userId));

            if (entity.descr != null)
                sql += String.Format(" and [gdce_task].dbo.T_Task_User.descr = '{0}' ", AntiInjection(entity.descr));

            return AntiInjection(sql);
        }


    }
}
