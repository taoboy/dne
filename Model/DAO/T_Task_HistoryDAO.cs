/**  
* file: T_Task_HistoryDAO.cs
* Type: ORM DAO Class 
* Genreated date: 2016/8/17 17:39:13
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
    public partial class T_Task_HistoryDAO:DAO
    {

        //处理本类的所有属性，把String类型里面的单引号处理一下
        public void AntiSQLInjection(T_Task_History entity) {
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

        public List<T_Task_History> DtToList(DataTable dt)
        {
            List<T_Task_History> entities = new List<T_Task_History>();
            foreach (DataRow dr in dt.Rows)
            {
                T_Task_History entity = new T_Task_History(dr, "");
                try
                {
                    if(entity.taskId != null)
                        entity.taskIdEntity = new T_Task(dr, "taskId_");
                }
                catch (Exception ex) { }
                entities.Add(entity);
            }
            return entities;
        }

        /// <summary>
        /// 新增一条数据，空字段将使用数据库默认值
        /// </summary>
        public int Add(T_Task_History entity)
        {
            List<SqlParameter> pars =  new List<SqlParameter>();
            StringBuilder sql = new StringBuilder();
            String sql2 = " ";
             
            sql.Append(" INSERT INTO [gdce_task].dbo.[T_Task_History] ( ");
            

            if (entity.id != null){
                sql.Append(" [id],");
                pars.Add(new SqlParameter("@id",entity.id));
                sql2 += " @id,";
            }

            if (entity.taskId != null){
                sql.Append(" [taskId],");
                pars.Add(new SqlParameter("@taskId",entity.taskId));
                sql2 += " @taskId,";
            }

            if (entity.userName != null){
                sql.Append(" [userName],");
                pars.Add(new SqlParameter("@userName",entity.userName));
                sql2 += " @userName,";
            }

            if (entity.content != null){
                sql.Append(" [content],");
                pars.Add(new SqlParameter("@content",entity.content));
                sql2 += " @content,";
            }

            if (entity.descr != null){
                sql.Append(" [descr],");
                pars.Add(new SqlParameter("@descr",entity.descr));
                sql2 += " @descr,";
            }

            if (entity.createTime != null){
                sql.Append(" [createTime],");
                pars.Add(new SqlParameter("@createTime",entity.createTime));
                sql2 += " @createTime,";
            }

            if (entity.userId != null){
                sql.Append(" [userId],");
                pars.Add(new SqlParameter("@userId",entity.userId));
                sql2 += " @userId,";
            }

            sql.Remove(sql.Length - 1, 1);//去掉末尾的逗号
            sql2 = sql2.Remove(sql2.Length - 1, 1);
            sql.Append(" ) values ( " + sql2 + ");select case when @@identity is not null then @@identity else 1 end as id;");
 
            return Convert.ToInt32(DBHelper.ExecuteDataTable(sql.ToString(), pars).Rows[0][0]);
        }

        public T_Task_History GetById(Int32 id)
        {
            T_Task_History entity = new T_Task_History() { id = id };
            return GetByModel(entity, false);
        }

        public int Del(Int32 id)
        {
            String sql = " DELETE FROM [gdce_task].dbo.[T_Task_History] WHERE [id] = @id";
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("@id", id));
            return DBHelper.ExecuteNonQuery(sql, paras);
        }

        /// <summary>
        /// 更新一条数据，空字段将在数据库里面写入NULL值，请注意。
        /// </summary>
        public int Update(T_Task_History entity)
        {
            AntiSQLInjection(entity);

            StringBuilder sql = new StringBuilder();

            sql.Append(" exec sp_executesql N'");
            sql.Append(" UPDATE [gdce_task].dbo.[T_Task_History] SET  ");
            sql.Append(" [taskId] = @taskId,");
            sql.Append(" [userName] = @userName,");
            sql.Append(" [content] = @content,");
            sql.Append(" [descr] = @descr,");
            sql.Append(" [createTime] = @createTime,");
            sql.Append(" [userId] = @userId,");
            sql.Remove(sql.Length - 1, 1);
            sql.Append(" WHERE [id] = ''" + entity.id + "'' ', N'");

            sql.Append("@taskId nvarchar(max),");
            sql.Append("@userName nvarchar(max),");
            sql.Append("@content nvarchar(max),");
            sql.Append("@descr nvarchar(max),");
            sql.Append("@createTime nvarchar(max),");
            sql.Append("@userId nvarchar(max),");
            sql.Remove(sql.Length - 1, 1);
            sql.Append("', ");

            sql.Append("@taskId=" + (entity.taskId != null ? "N'" + entity.taskId + "'," : "NULL,"));
            sql.Append("@userName=" + (entity.userName != null ? "N'" + entity.userName + "'," : "NULL,"));
            sql.Append("@content=" + (entity.content != null ? "N'" + entity.content + "'," : "NULL,"));
            sql.Append("@descr=" + (entity.descr != null ? "N'" + entity.descr + "'," : "NULL,"));
            sql.Append("@createTime=" + (entity.createTime != null ? "N'" + entity.createTime + "'," : "NULL,"));
            sql.Append("@userId=" + (entity.userId != null ? "N'" + entity.userId + "'," : "NULL,"));
            sql.Remove(sql.Length - 1, 1);

            return DBHelper.ExecuteNonQuery(sql.ToString());
        }


        /// <summary>
        /// 更新一条数据，当isNullIgnored为false时，空字段将在数据库里面写入NULL值；
        /// 当isNullIgnored为true时，空字段将不进行更新，此时用户可以少用一次getById来获取原对象进行更新
        /// </summary>
        public int Update(T_Task_History entity, bool isNullIgnored)
        {
            if (!isNullIgnored)
                return Update(entity);

            List<SqlParameter> pars = new List<SqlParameter>();
            StringBuilder sql = new StringBuilder();
             
            sql.Append(" UPDATE [gdce_task].dbo.[T_Task_History] SET  ");


            if (entity.taskId != null)
            {
                sql.Append(" [taskId] = @taskId,");
                pars.Add(new SqlParameter("@taskId", entity.taskId));
            }

            if (entity.userName != null)
            {
                sql.Append(" [userName] = @userName,");
                pars.Add(new SqlParameter("@userName", entity.userName));
            }

            if (entity.content != null)
            {
                sql.Append(" [content] = @content,");
                pars.Add(new SqlParameter("@content", entity.content));
            }

            if (entity.descr != null)
            {
                sql.Append(" [descr] = @descr,");
                pars.Add(new SqlParameter("@descr", entity.descr));
            }

            if (entity.createTime != null)
            {
                sql.Append(" [createTime] = @createTime,");
                pars.Add(new SqlParameter("@createTime", entity.createTime));
            }

            if (entity.userId != null)
            {
                sql.Append(" [userId] = @userId,");
                pars.Add(new SqlParameter("@userId", entity.userId));
            }               
    
            sql.Remove(sql.Length - 1, 1);
            sql.Append(" where id = @id ;");
            pars.Add(new SqlParameter("@id", entity.id));
              
            return DBHelper.ExecuteNonQuery(sql.ToString(), pars);
        }


        /// <summary>
        /// 以实体类作为查询条件获取符合条件的数据集，NULL值不作为查询条件
        /// </summary>
        public List<T_Task_History> Search(T_Task_History entity, int pageNum, int pageSize, String criteria, T_Task_History.Columns OrderBy, AscDesc AscDesc, out int totalRecords)
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
        public List<T_Task_History> GetAllByModel(T_Task_History entity) {
            String sql = String.Format("exec sp_executesql N'{0}'", PreparedTable(entity, false, false));
            return DtToList(DBHelper.ExecuteDataTable(sql));
        }

        public T_Task_History GetByModel(T_Task_History entity)
        {
            String sql = String.Format("exec sp_executesql N'{0}'", PreparedTable(entity, false, true));

            DataTable dt = DBHelper.ExecuteDataTable(sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                entity = new T_Task_History(dt.Rows[0], "");
                if(entity.taskId != null)
                    entity.taskIdEntity = new T_Task(dt.Rows[0], "taskId_");
                return entity;
            }
            else
                return null;
        }


        public T_Task_History GetByModel(T_Task_History entity, bool isLazy)
        {
            T_Task_History result = GetByModel(entity);

            if (result == null)//如果传入的实体为空（或者根据ID找不到）,则需要new一个空对象
                result = new T_Task_History();

            if (!isLazy) {
                using (T_TaskDAO dao = new T_TaskDAO())
                    result.taskIdEntitys = dao.GetAllByModel(new T_Task());
            }
            return result;   
        }

        /// <summary>
        /// 用拼接SQL的方式，
        /// 把当前表以及其使用到的所有外键表链接起来，
        /// 拼接成一条SQL子查询，供分页的存储过程使用
        /// </summary>
        private String PreparedTable(T_Task_History entity, bool isFuzzySearch, bool isSignleResult)
        {
            String sql = getFKColumnsSQL("[gdce_task].dbo.T_Task_History", T_Task_History.DBColumns, isFuzzySearch, isSignleResult);
            
            
            if (entity == null)
            	return sql;		

            if (entity.id != null)
                sql += " and [gdce_task].dbo.T_Task_History.id = " + entity.id;

            if (entity.taskId != null)
                sql += " and [gdce_task].dbo.T_Task_History.taskId = " + entity.taskId;

            if (entity.userName != null)
                sql += String.Format(" and [gdce_task].dbo.T_Task_History.userName = '{0}' ", AntiInjection(entity.userName));

            if (entity.content != null)
                sql += String.Format(" and [gdce_task].dbo.T_Task_History.content = '{0}' ", AntiInjection(entity.content));

            if (entity.descr != null)
                sql += String.Format(" and [gdce_task].dbo.T_Task_History.descr = '{0}' ", AntiInjection(entity.descr));

            if (entity.createTime != null)
                sql += String.Format(" and [gdce_task].dbo.T_Task_History.createTime = '{0}' ", entity.createTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));            

            if (entity.userId != null)
                sql += String.Format(" and [gdce_task].dbo.T_Task_History.userId = '{0}' ", AntiInjection(entity.userId));

            return AntiInjection(sql);
        }


    }
}
