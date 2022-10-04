using System;
using System.Data;
using System.Text;
using System.Data.SQLite;
using Scada.DBUtility;


#region << 版 本 注 释 >>
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
#endregion
namespace Scada.Database
{
	/// <summary>
	/// 数据访问类:SystemGroup
	/// </summary>
	public partial class SystemGroup: IDisposable
	{
		public SystemGroup()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQLite.GetMaxID("Id", "SystemGroup"); 
		}


		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from SystemGroup");
			strSql.Append(" where Id="+Id+" ");
			return DbHelperSQLite.Exists(strSql.ToString());
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(Scada.Model.SystemGroup model)
		{
			StringBuilder strSql=new StringBuilder();
			StringBuilder strSql1=new StringBuilder();
			StringBuilder strSql2=new StringBuilder();
			if (model.GroupTitle != null)
			{
				strSql1.Append("GroupTitle,");
				strSql2.Append(""+model.GroupTitle+",");
			}
			if (model.CreateTime != null)
			{
				strSql1.Append("CreateTime,");
				strSql2.Append("'"+model.CreateTime+"',");
			}
			if (model.CreateUserId != null)
			{
				strSql1.Append("CreateUserId,");
				strSql2.Append(""+model.CreateUserId+",");
			}
			if (model.UpdateTime != null)
			{
				strSql1.Append("UpdateTime,");
				strSql2.Append("'"+model.UpdateTime+"',");
			}
			if (model.UpdateUserId != null)
			{
				strSql1.Append("UpdateUserId,");
				strSql2.Append(""+model.UpdateUserId+",");
			}
			if (model.ParentId != null)
			{
				strSql1.Append("ParentId,");
				strSql2.Append(""+model.ParentId+",");
			}
			strSql.Append("insert into SystemGroup(");
			strSql.Append(strSql1.ToString().Remove(strSql1.Length - 1));
			strSql.Append(")");
			strSql.Append(" values (");
			strSql.Append(strSql2.ToString().Remove(strSql2.Length - 1));
			strSql.Append(")");
			strSql.Append(";select LAST_INSERT_ROWID()");
			object obj = DbHelperSQLite.GetSingle(strSql.ToString());
			if (obj == null)
			{
				return 0;
			}
			else
			{
				return Convert.ToInt32(obj);
			}
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(Scada.Model.SystemGroup model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update SystemGroup set ");
			if (model.GroupTitle != null)
			{
				strSql.Append("GroupTitle="+model.GroupTitle+",");
			}
			else
			{
				strSql.Append("GroupTitle= null ,");
			}
			if (model.CreateTime != null)
			{
				strSql.Append("CreateTime='"+model.CreateTime+"',");
			}
			else
			{
				strSql.Append("CreateTime= null ,");
			}
			if (model.CreateUserId != null)
			{
				strSql.Append("CreateUserId="+model.CreateUserId+",");
			}
			else
			{
				strSql.Append("CreateUserId= null ,");
			}
			if (model.UpdateTime != null)
			{
				strSql.Append("UpdateTime='"+model.UpdateTime+"',");
			}
			else
			{
				strSql.Append("UpdateTime= null ,");
			}
			if (model.UpdateUserId != null)
			{
				strSql.Append("UpdateUserId="+model.UpdateUserId+",");
			}
			else
			{
				strSql.Append("UpdateUserId= null ,");
			}
			if (model.ParentId != null)
			{
				strSql.Append("ParentId="+model.ParentId+",");
			}
			else
			{
				strSql.Append("ParentId= null ,");
			}
			int n = strSql.ToString().LastIndexOf(",");
			strSql.Remove(n, 1);
			strSql.Append(" where Id="+ model.Id+"");
			int rowsAffected=DbHelperSQLite.ExecuteSql(strSql.ToString());
			if (rowsAffected > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int Id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from SystemGroup ");
			strSql.Append(" where Id="+Id+"" );
			int rowsAffected=DbHelperSQLite.ExecuteSql(strSql.ToString());
			if (rowsAffected > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}		/// <summary>
		/// 批量删除数据
		/// </summary>
		public bool DeleteList(string Idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from SystemGroup ");
			strSql.Append(" where Id in ("+Idlist + ")  ");
			int rows=DbHelperSQLite.ExecuteSql(strSql.ToString());
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Scada.Model.SystemGroup GetModel(int Id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  ");
			strSql.Append(" Id,GroupTitle,CreateTime,CreateUserId,UpdateTime,UpdateUserId,ParentId ");
			strSql.Append(" from SystemGroup ");
			strSql.Append(" where Id="+Id+"" );
			Scada.Model.SystemGroup model=new Scada.Model.SystemGroup();
			DataSet ds=DbHelperSQLite.Query(strSql.ToString());
			if(ds.Tables[0].Rows.Count>0)
			{
				return DataRowToModel(ds.Tables[0].Rows[0]);
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Scada.Model.SystemGroup DataRowToModel(DataRow row)
		{
			Scada.Model.SystemGroup model=new Scada.Model.SystemGroup();
			if (row != null)
			{
				if(row["Id"]!=null && row["Id"].ToString()!="")
				{
					model.Id=int.Parse(row["Id"].ToString());
				}
				if(row["GroupTitle"]!=null && row["GroupTitle"].ToString()!="")
				{
					model.GroupTitle=int.Parse(row["GroupTitle"].ToString());
				}
				if(row["CreateTime"]!=null)
				{
					model.CreateTime=row["CreateTime"].ToString();
				}
				if(row["CreateUserId"]!=null && row["CreateUserId"].ToString()!="")
				{
					model.CreateUserId=int.Parse(row["CreateUserId"].ToString());
				}
				if(row["UpdateTime"]!=null)
				{
					model.UpdateTime=row["UpdateTime"].ToString();
				}
				if(row["UpdateUserId"]!=null && row["UpdateUserId"].ToString()!="")
				{
					model.UpdateUserId=int.Parse(row["UpdateUserId"].ToString());
				}
				if(row["ParentId"]!=null && row["ParentId"].ToString()!="")
				{
					model.ParentId=int.Parse(row["ParentId"].ToString());
				}
			}
			return model;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select Id,GroupTitle,CreateTime,CreateUserId,UpdateTime,UpdateUserId,ParentId ");
			strSql.Append(" FROM SystemGroup ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSQLite.Query(strSql.ToString());
		}

		/// <summary>
		/// 获取记录总数
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) FROM SystemGroup ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			object obj = DbHelperSQLite.GetSingle(strSql.ToString());
			if (obj == null)
			{
				return 0;
			}
			else
			{
				return Convert.ToInt32(obj);
			}
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("SELECT * FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrEmpty(orderby.Trim()))
			{
				strSql.Append("order by T." + orderby );
			}
			else
			{
				strSql.Append("order by T.Id desc");
			}
			strSql.Append(")AS Row, T.*  from SystemGroup T ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" ) TT");
			strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
			return DbHelperSQLite.Query(strSql.ToString());
		}

        public void Dispose()
        {
           
        }

        /*
		*/

        #endregion  Method
        #region  MethodEx

        #endregion  MethodEx
    }
}

