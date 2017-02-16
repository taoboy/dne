/**  
* file: T_UserDAO.cs
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
    public partial class T_UserDAO:DAO
    {

        //处理本类的所有属性，把String类型里面的单引号处理一下
        public void AntiSQLInjection(T_User entity) {
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

        public List<T_User> DtToList(DataTable dt)
        {
            List<T_User> entities = new List<T_User>();
            foreach (DataRow dr in dt.Rows)
            {
                T_User entity = new T_User(dr, "");
                try
                {
                    if(entity.roleId != null)
                        entity.roleIdEntity = new T_Role(dr, "roleId_");
                }
                catch (Exception ex) { }
                entities.Add(entity);
            }
            return entities;
        }

        public int Add(T_User entity)
        {
            AntiSQLInjection(entity);

            StringBuilder sql = new StringBuilder();

            sql.Append(" exec sp_executesql N'");
            sql.Append(" INSERT INTO [gdce_task].dbo.[T_User] ( ");
            sql.Append(" [account],");
            sql.Append(" [roleId],");
            sql.Append(" [name],");
            sql.Append(" [password],");
            sql.Append(" [createTime],");
            sql.Append(" [phone],");
            sql.Append(" [dept],");
            sql.Append(" [descr],");
            sql.Append(" [status],");
            sql.Remove(sql.Length - 1, 1);
            sql.Append(" ) values ( ");

            sql.Append(entity.account != null ? "@account," : "DEFAULT,");
            sql.Append(entity.roleId != null ? "@roleId," : "DEFAULT,");
            sql.Append(entity.name != null ? "@name," : "DEFAULT,");
            sql.Append(entity.password != null ? "@password," : "DEFAULT,");
            sql.Append(entity.createTime != null ? "@createTime," : "DEFAULT,");
            sql.Append(entity.phone != null ? "@phone," : "DEFAULT,");
            sql.Append(entity.dept != null ? "@dept," : "DEFAULT,");
            sql.Append(entity.descr != null ? "@descr," : "DEFAULT,");
            sql.Append(entity.status != null ? "@status," : "DEFAULT,");
            sql.Remove(sql.Length - 1, 1);
            sql.Append(" );select case when @@identity is not null then @@identity else 1 end as id;', N'");


            sql.Append(entity.account != null ? "@account nvarchar(max)," : "");
            sql.Append(entity.roleId != null ? "@roleId nvarchar(max)," : "");
            sql.Append(entity.name != null ? "@name nvarchar(max)," : "");
            sql.Append(entity.password != null ? "@password nvarchar(max)," : "");
            sql.Append(entity.createTime != null ? "@createTime nvarchar(max)," : "");
            sql.Append(entity.phone != null ? "@phone nvarchar(max)," : "");
            sql.Append(entity.dept != null ? "@dept nvarchar(max)," : "");
            sql.Append(entity.descr != null ? "@descr nvarchar(max)," : "");
            sql.Append(entity.status != null ? "@status nvarchar(max)," : "");
            sql.Remove(sql.Length - 1, 1);
            sql.Append("', ");

            sql.Append(entity.account != null ? "@account=N'" + entity.account + "'," : "");
            sql.Append(entity.roleId != null ? "@roleId=N'" + entity.roleId + "'," : "");
            sql.Append(entity.name != null ? "@name=N'" + entity.name + "'," : "");
            sql.Append(entity.password != null ? "@password=N'" + entity.password + "'," : "");
            sql.Append(entity.createTime != null ? "@createTime=N'" + entity.createTime + "'," : "");
            sql.Append(entity.phone != null ? "@phone=N'" + entity.phone + "'," : "");
            sql.Append(entity.dept != null ? "@dept=N'" + entity.dept + "'," : "");
            sql.Append(entity.descr != null ? "@descr=N'" + entity.descr + "'," : "");
            sql.Append(entity.status != null ? "@status=N'" + entity.status + "'," : "");
            sql.Remove(sql.Length - 1, 1);

            return Convert.ToInt32(DBHelper.ExecuteDataTable(sql.ToString()).Rows[0][0]);
        }

        public T_User GetById(String id)
        {
            T_User entity = new T_User() { id = id };
            return GetByModel(entity, false);
        }

        public int Del(String id)
        {
            String sql = " DELETE FROM [gdce_task].dbo.[T_User] WHERE [id] = @id";
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("@id", id));
            return DBHelper.ExecuteNonQuery(sql, paras);
        }

        public int Update(T_User entity)
        {
            AntiSQLInjection(entity);

            StringBuilder sql = new StringBuilder();

            sql.Append(" exec sp_executesql N'");
            sql.Append(" UPDATE [gdce_task].dbo.[T_User] SET  ");
            sql.Append(" [account] = @account,");
            sql.Append(" [roleId] = @roleId,");
            sql.Append(" [name] = @name,");
            sql.Append(" [password] = @password,");
            sql.Append(" [createTime] = @createTime,");
            sql.Append(" [phone] = @phone,");
            sql.Append(" [dept] = @dept,");
            sql.Append(" [descr] = @descr,");
            sql.Append(" [status] = @status,");
            sql.Remove(sql.Length - 1, 1);
            sql.Append(" WHERE [id] = ''" + entity.id + "'' ', N'");

            sql.Append("@account nvarchar(max),");
            sql.Append("@roleId nvarchar(max),");
            sql.Append("@name nvarchar(max),");
            sql.Append("@password nvarchar(max),");
            sql.Append("@createTime nvarchar(max),");
            sql.Append("@phone nvarchar(max),");
            sql.Append("@dept nvarchar(max),");
            sql.Append("@descr nvarchar(max),");
            sql.Append("@status nvarchar(max),");
            sql.Remove(sql.Length - 1, 1);
            sql.Append("', ");

            sql.Append("@account=" + (entity.account != null ? "N'" + entity.account + "'," : "NULL,"));
            sql.Append("@roleId=" + (entity.roleId != null ? "N'" + entity.roleId + "'," : "NULL,"));
            sql.Append("@name=" + (entity.name != null ? "N'" + entity.name + "'," : "NULL,"));
            sql.Append("@password=" + (entity.password != null ? "N'" + entity.password + "'," : "NULL,"));
            sql.Append("@createTime=" + (entity.createTime != null ? "N'" + entity.createTime + "'," : "NULL,"));
            sql.Append("@phone=" + (entity.phone != null ? "N'" + entity.phone + "'," : "NULL,"));
            sql.Append("@dept=" + (entity.dept != null ? "N'" + entity.dept + "'," : "NULL,"));
            sql.Append("@descr=" + (entity.descr != null ? "N'" + entity.descr + "'," : "NULL,"));
            sql.Append("@status=" + (entity.status != null ? "N'" + entity.status + "'," : "NULL,"));
            sql.Remove(sql.Length - 1, 1);

            return DBHelper.ExecuteNonQuery(sql.ToString());
        }


        /// <summary>
        /// 以实体类作为查询条件获取符合条件的数据集，NULL值不作为查询条件
        /// </summary>
        public List<T_User> Search(T_User entity, int pageNum, int pageSize, String criteria, T_User.Columns OrderBy, AscDesc AscDesc, out int totalRecords)
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
        public List<T_User> GetAllByModel(T_User entity) {
            String sql = String.Format("exec sp_executesql N'{0}'", PreparedTable(entity, false, false));
            return DtToList(DBHelper.ExecuteDataTable(sql));
        }

        public T_User GetByModel(T_User entity)
        {
            String sql = String.Format("exec sp_executesql N'{0}'", PreparedTable(entity, false, true));

            DataTable dt = DBHelper.ExecuteDataTable(sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                entity = new T_User(dt.Rows[0], "");
                if(entity.roleId != null)
                    entity.roleIdEntity = new T_Role(dt.Rows[0], "roleId_");
                return entity;
            }
            else
                return null;
        }


        public T_User GetByModel(T_User entity, bool isLazy)
        {
            T_User result = GetByModel(entity);

            if (result == null)//如果传入的实体为空（或者根据ID找不到）,则需要new一个空对象
                result = new T_User();

            if (!isLazy) {
                using (T_RoleDAO dao = new T_RoleDAO())
                    result.roleIdEntitys = dao.GetAllByModel(new T_Role());
            }
            return result;   
        }

        /// <summary>
        /// 用拼接SQL的方式，
        /// 把当前表以及其使用到的所有外键表链接起来，
        /// 拼接成一条SQL子查询，供分页的存储过程使用
        /// </summary>
        private String PreparedTable(T_User entity, bool isFuzzySearch, bool isSignleResult)
        {
            String sql = getFKColumnsSQL("[gdce_task].dbo.T_User", T_User.DBColumns, isFuzzySearch, isSignleResult);
            
            
            if (entity == null)
            	return sql;		

            if (entity.id != null)
                sql += String.Format(" and [gdce_task].dbo.T_User.id = '{0}' ", AntiInjection(entity.id));

            if (entity.account != null)
                sql += String.Format(" and [gdce_task].dbo.T_User.account = '{0}' ", AntiInjection(entity.account));

            if (entity.roleId != null)
                sql += " and [gdce_task].dbo.T_User.roleId = " + entity.roleId;

            if (entity.name != null)
                sql += String.Format(" and [gdce_task].dbo.T_User.name = '{0}' ", AntiInjection(entity.name));

            if (entity.password != null)
                sql += String.Format(" and [gdce_task].dbo.T_User.password = '{0}' ", AntiInjection(entity.password));

            if (entity.createTime != null)
                sql += String.Format(" and [gdce_task].dbo.T_User.createTime = '{0}' ", entity.createTime.Value.ToString("yyyy-MM-dd HH:mm:ss"));            

            if (entity.phone != null)
                sql += String.Format(" and [gdce_task].dbo.T_User.phone = '{0}' ", AntiInjection(entity.phone));

            if (entity.dept != null)
                sql += String.Format(" and [gdce_task].dbo.T_User.dept = '{0}' ", AntiInjection(entity.dept));

            if (entity.descr != null)
                sql += String.Format(" and [gdce_task].dbo.T_User.descr = '{0}' ", AntiInjection(entity.descr));

            if (entity.status != null)
                sql += " and [gdce_task].dbo.T_User.status =  " + (entity.status.Value ? "1" : "0");

            return AntiInjection(sql);
        }


    }
}
