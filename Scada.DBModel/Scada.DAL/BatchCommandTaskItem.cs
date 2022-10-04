using System;
using System.Data;
using System.Text;
using System.Data.SQLite;
using Scada.DBUtility;
using System.Collections;
using System.Collections.Generic;


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
	/// 数据访问类:BatchCommandTaskItem
	/// </summary>
	public partial class BatchCommandTaskItemModel
	{
		public BatchCommandTaskItemModel()
		{}
		#region  BasicMethod

		public bool Clear(string serverid)
		{

			StringBuilder strSql = new StringBuilder();
			strSql.Append("delete from BatchCommandTaskItem  where SERVER_ID='" + serverid + "'");

			int rows = DbHelperSQLite.ExecuteSql(strSql.ToString());
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		public void Add(List<Scada.Model.BatchCommandTaskItemModel> models)
		{
			ArrayList sqlArray = new ArrayList();
			for (int i = 0; i < models.Count; i++)
			{
				Scada.Model.BatchCommandTaskItemModel model = models[i];
			 
				StringBuilder strSql = new StringBuilder();
				StringBuilder strSql1 = new StringBuilder();
				StringBuilder strSql2 = new StringBuilder();
				if (model.Id != null)
				{
					strSql1.Append("Id,");
					strSql2.Append("'" + model.Id + "',");
				}
				if (model.CommandTaskID != null)
				{
					strSql1.Append("CommandTaskID,");
					strSql2.Append("'" + model.CommandTaskID + "',");
				}
				if (model.CommandCreateTime != null)
				{
					strSql1.Append("CommandCreateTime,");
					strSql2.Append("'" + model.CommandCreateTime + "',");
				}
				if (model.Remark != null)
				{
					strSql1.Append("Remark,");
					strSql2.Append("'" + model.Remark + "',");
				}
				if (model.CommandExecuteType != null)
				{
					strSql1.Append("CommandExecuteType,");
					strSql2.Append("'" + model.CommandExecuteType + "',");
				}
				if (model.CommandExecuteTime != null)
				{
					strSql1.Append("CommandExecuteTime,");
					strSql2.Append("'" + model.CommandExecuteTime + "',");
				}
				if (model.PreCommandItemID != null)
				{
					strSql1.Append("PreCommandItemID,");
					strSql2.Append("'" + model.PreCommandItemID + "',");
				}

				strSql1.Append("SERVER_ID,");
				strSql2.Append("'" + model.SERVER_ID + "',");


				strSql1.Append("NextCommandItemIDList,");
				strSql2.Append("'" + model.NextCommandItemIDList + "',");
				if (model.X != null)
				{
					strSql1.Append("X,");
					strSql2.Append("" + model.X + ",");
				}

				if (model.Y != null)
				{
					strSql1.Append("Y,");
					strSql2.Append("" + model.Y + ",");
				}
				if (model.Width != null)
				{
					strSql1.Append("Width,");
					strSql2.Append("" + model.Width + ",");
				}

				if (model.Height != null)
				{
					strSql1.Append("Height,");
					strSql2.Append("" + model.Height + ",");
				}

				if (model.Expand != null)
				{
					strSql1.Append("Expand,");
					strSql2.Append("" + model.Expand + ",");
				}
				if (model.IOParaCommand != null)
				{
					strSql1.Append("IOParaCommand,");
					strSql2.Append("'" + model.IOParaCommand + "',");
				}
				if (model.CommandItemTitle != null)
				{
					strSql1.Append("CommandItemTitle,");
					strSql2.Append("'" + model.CommandItemTitle + "',");
				}
				if (model.StartIOParaValue != null)
				{
					strSql1.Append("StartIOParaValue,");
					strSql2.Append("'" + model.StartIOParaValue + "',");
				}
				if (model.Delayed != null)
				{
					strSql1.Append("Delayed,");
					strSql2.Append("'" + model.Delayed + "',");
				}
				if (model.FixedValue != null)
				{
					strSql1.Append("FixedValue,");
					strSql2.Append("'" + model.FixedValue + "',");
				}
				if (model.CommandItemType != null)
				{
					strSql1.Append("CommandItemType,");
					strSql2.Append("'" + model.CommandItemType + "',");
				}

			 
				if (model.CommandItemExecuteResult != null)
				{
					strSql1.Append("CommandItemExecuteResult,");
					strSql2.Append("'" + model.CommandItemExecuteResult + "',");
				}
				strSql.Append("insert into BatchCommandTaskItem(");
				strSql.Append(strSql1.ToString().Remove(strSql1.Length - 1));
				strSql.Append(")");
				strSql.Append(" values (");
				strSql.Append(strSql2.ToString().Remove(strSql2.Length - 1));
				strSql.Append(")");
				sqlArray.Add(strSql);
			}

			DbHelperSQLite.ExecuteSqlTran(sqlArray);
			sqlArray.Clear();
			sqlArray = null;
		}
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(Scada.Model.BatchCommandTaskItemModel model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into BatchCommandTaskItem(");
			strSql.Append("Id,CommandTaskID,Remark,CommandCreateTime,CommandExecuteType,CommandExecuteTime,PreCommandItemID,SERVER_ID,NextCommandItemIDList,X,Y,IOParaCommand,CommandItemTitle,StartIOParaValue,Delayed,FixedValue,CommandItemExecuteResult,CommandItemType,Width,Height,Expand)");
			strSql.Append(" values (");
			strSql.Append("@Id,@CommandTaskID,@Remark,@CommandCreateTime,@CommandExecuteType,@CommandExecuteTime,@PreCommandItemID,@SERVER_ID,@NextCommandItemIDList,@X,@Y,@IOParaCommand,@CommandItemTitle,@StartIOParaValue,@Delayed,@FixedValue,@CommandItemExecuteResult,@CommandItemType,@Width,@Height,@Expand)");
			SQLiteParameter[] parameters = {
					new SQLiteParameter("@Id", DbType.String),
					new SQLiteParameter("@CommandTaskID", DbType.String),
					new SQLiteParameter("@Remark", DbType.String),
					new SQLiteParameter("@CommandCreateTime", DbType.String),
					new SQLiteParameter("@CommandExecuteType", DbType.String),
					new SQLiteParameter("@CommandExecuteTime", DbType.String),
					new SQLiteParameter("@PreCommandItemID", DbType.String),
					new SQLiteParameter("@SERVER_ID", DbType.String),
					new SQLiteParameter("@NextCommandItemIDList", DbType.String),
					new SQLiteParameter("@X", DbType.Single),
					new SQLiteParameter("@Y", DbType.Single),
					new SQLiteParameter("@IOParaCommand", DbType.String),
					new SQLiteParameter("@CommandItemTitle", DbType.String),
					new SQLiteParameter("@StartIOParaValue", DbType.String),
					new SQLiteParameter("@Delayed", DbType.String),
					new SQLiteParameter("@FixedValue", DbType.String),
					new SQLiteParameter("@CommandItemExecuteResult", DbType.String),
						new SQLiteParameter("@CommandItemType", DbType.String),
						new SQLiteParameter("@Width", DbType.Single),
					new SQLiteParameter("@Height", DbType.Single),
			new SQLiteParameter("@Expand", DbType.Int32,4)


			}; 
			 parameters[0].Value = model.Id;
			parameters[1].Value = model.CommandTaskID;
			parameters[2].Value = model.Remark;
			parameters[3].Value = model.CommandCreateTime;
			parameters[4].Value = model.CommandExecuteType;
			parameters[5].Value = model.CommandExecuteTime;
			parameters[6].Value = model.PreCommandItemID;
			parameters[7].Value = model.SERVER_ID;
			parameters[8].Value = model.NextCommandItemIDList;
			parameters[9].Value = model.X;
			parameters[10].Value = model.Y;
			parameters[11].Value = model.IOParaCommand;
			parameters[12].Value = model.CommandItemTitle;
			parameters[13].Value = model.StartIOParaValue;
			parameters[14].Value = model.Delayed;
			parameters[15].Value = model.FixedValue;
			parameters[16].Value = model.CommandItemExecuteResult;
			parameters[17].Value = model.CommandItemType;
			parameters[18].Value = model.Width;
			parameters[19].Value = model.Height;
			parameters[20].Value = model.Expand;
			int rows=DbHelperSQLite.ExecuteSql(strSql.ToString(),parameters);
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
		/// 更新一条数据
		/// </summary>
		public bool Update(Scada.Model.BatchCommandTaskItemModel model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update BatchCommandTaskItem set ");
			strSql.Append("Id=@Id,");
			strSql.Append("CommandTaskID=@CommandTaskID,");
			strSql.Append("Remark=@Remark,");
			strSql.Append("CommandCreateTime=@CommandCreateTime,");
			strSql.Append("CommandExecuteType=@CommandExecuteType,");
			strSql.Append("CommandExecuteTime=@CommandExecuteTime,");
			strSql.Append("PreCommandItemID=@PreCommandItemID,");
			strSql.Append("SERVER_ID=@SERVER_ID,");
			strSql.Append("NextCommandItemIDList=@NextCommandItemIDList,");
			strSql.Append("X=@X,");
			strSql.Append("Y=@Y,");
			strSql.Append("Width=@Width,");
			strSql.Append("Height=@Height,");
			strSql.Append("IOParaCommand=@IOParaCommand,");
			strSql.Append("CommandItemTitle=@CommandItemTitle,");
			strSql.Append("StartIOParaValue=@StartIOParaValue,");
			strSql.Append("Delayed=@Delayed,");
			strSql.Append("CommandItemExecuteResult=@CommandItemExecuteResult,");
			strSql.Append("CommandItemType=@CommandItemType,");
			strSql.Append("Expand=@Expand,");
			
			strSql.Append("FixedValue=@FixedValue");
			strSql.Append(" where ");
			SQLiteParameter[] parameters = {
					new SQLiteParameter("@Id", DbType.String),
					new SQLiteParameter("@CommandTaskID", DbType.String),
					new SQLiteParameter("@Remark", DbType.String),
					new SQLiteParameter("@CommandCreateTime", DbType.String),
					new SQLiteParameter("@CommandExecuteType", DbType.String),
					new SQLiteParameter("@CommandExecuteTime", DbType.String),
					new SQLiteParameter("@PreCommandItemID", DbType.String),
					new SQLiteParameter("@SERVER_ID", DbType.String),
					new SQLiteParameter("@NextCommandItemIDList", DbType.String),
					new SQLiteParameter("@X", DbType.Single),
					new SQLiteParameter("@Y", DbType.Single),
				
					new SQLiteParameter("@IOParaCommand", DbType.String),
					new SQLiteParameter("@CommandItemTitle", DbType.String),
					new SQLiteParameter("@StartIOParaValue", DbType.String),
					new SQLiteParameter("@Delayed", DbType.String),
					new SQLiteParameter("@FixedValue", DbType.String),
					new SQLiteParameter("@CommandItemExecuteResult", DbType.String),
					new SQLiteParameter("@CommandItemType", DbType.String),
						new SQLiteParameter("@Width", DbType.Single),
					new SQLiteParameter("@Height", DbType.Single),
	        new SQLiteParameter("@Expand", DbType.Int32,4)

					


			};
			parameters[0].Value = model.Id;
			parameters[1].Value = model.CommandTaskID;
			parameters[2].Value = model.Remark;
			parameters[3].Value = model.CommandCreateTime;
			parameters[4].Value = model.CommandExecuteType;
			parameters[5].Value = model.CommandExecuteTime;
			parameters[6].Value = model.PreCommandItemID;
			parameters[7].Value = model.SERVER_ID;
			parameters[8].Value = model.NextCommandItemIDList;
			parameters[9].Value = model.X;
			parameters[10].Value = model.Y;
			parameters[11].Value = model.IOParaCommand;
			parameters[12].Value = model.CommandItemTitle;
			parameters[13].Value = model.StartIOParaValue;
			parameters[14].Value = model.Delayed;
			parameters[15].Value = model.FixedValue;
			parameters[16].Value = model.CommandItemExecuteResult;
			parameters[17].Value = model.CommandItemType;
			parameters[18].Value = model.Width;
			parameters[19].Value = model.Height;
			parameters[20].Value = model.Expand;


			int rows=DbHelperSQLite.ExecuteSql(strSql.ToString(),parameters);
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
		/// 删除一条数据
		/// </summary>
		public bool Delete()
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from BatchCommandTaskItem ");
			strSql.Append(" where ");
			SQLiteParameter[] parameters = {
			};

			int rows=DbHelperSQLite.ExecuteSql(strSql.ToString(),parameters);
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
		public Scada.Model.BatchCommandTaskItemModel GetModel()
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select Id,CommandTaskID,Remark,CommandCreateTime,CommandExecuteType,CommandExecuteTime,PreCommandItemID,SERVER_ID,NextCommandItemIDList,X,Y,IOParaCommand,CommandItemTitle,StartIOParaValue,Delayed,FixedValue,CommandItemExecuteResult,CommandItemType,Width,Height,Expand from BatchCommandTaskItem ");
			strSql.Append(" where ");
			SQLiteParameter[] parameters = {
			};

			Scada.Model.BatchCommandTaskItemModel model =new Scada.Model.BatchCommandTaskItemModel();
			DataSet ds=DbHelperSQLite.Query(strSql.ToString(),parameters);
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
		public Scada.Model.BatchCommandTaskItemModel DataRowToModel(DataRow row)
		{
			Scada.Model.BatchCommandTaskItemModel model =new Scada.Model.BatchCommandTaskItemModel();
			if (row != null)
			{
				if(row["Id"]!=null)
				{
					model.Id=row["Id"].ToString();
				}
				if(row["CommandTaskID"]!=null)
				{
					model.CommandTaskID=row["CommandTaskID"].ToString();
				}
					//model.Remark=row["Remark"].ToString();
				if(row["CommandCreateTime"]!=null)
				{
					model.CommandCreateTime=row["CommandCreateTime"].ToString();
				}
				if(row["CommandExecuteType"]!=null)
				{
					model.CommandExecuteType=row["CommandExecuteType"].ToString();
				}
				if(row["CommandExecuteTime"]!=null)
				{
					model.CommandExecuteTime=row["CommandExecuteTime"].ToString();
				}
				if(row["PreCommandItemID"]!=null)
				{
					model.PreCommandItemID=row["PreCommandItemID"].ToString();
				}
				if(row["SERVER_ID"]!=null)
				{
					model.SERVER_ID=row["SERVER_ID"].ToString();
				}
				if(row["NextCommandItemIDList"]!=null)
				{
					model.NextCommandItemIDList=row["NextCommandItemIDList"].ToString();
				}
				if(row["X"]!=null && row["X"].ToString()!="")
				{
					model.X= float.Parse(row["X"].ToString());
				}
				if (row["Expand"] != null && row["Expand"].ToString() != "")
				{
					model.Expand = int.Parse(row["Expand"].ToString());
				}
				if (row["Y"]!=null && row["Y"].ToString()!="")
				{
					model.Y= float.Parse(row["Y"].ToString());
				}
				if (row["Width"] != null && row["Width"].ToString() != "")
				{
					model.Width = float.Parse(row["Width"].ToString());
				}
				if (row["Height"] != null && row["Height"].ToString() != "")
				{
					model.Height = float.Parse(row["Height"].ToString());
				}
				if (row["IOParaCommand"]!=null)
				{
					model.IOParaCommand=row["IOParaCommand"].ToString();
				}
				if(row["CommandItemTitle"]!=null)
				{
					model.CommandItemTitle=row["CommandItemTitle"].ToString();
				}
				if(row["StartIOParaValue"]!=null)
				{
					model.StartIOParaValue=row["StartIOParaValue"].ToString();
				}
				if(row["Delayed"]!=null)
				{
					model.Delayed=row["Delayed"].ToString();
				}
				if(row["FixedValue"]!=null)
				{
					model.FixedValue=row["FixedValue"].ToString();
				}
				if (row["CommandItemExecuteResult"] != null)
				{
					model.CommandItemExecuteResult = row["CommandItemExecuteResult"].ToString();
				}

				if (row["CommandItemType"] != null)
				{
					model.CommandItemType = row["CommandItemType"].ToString();
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
			strSql.Append("select Id,CommandTaskID,Remark,CommandCreateTime,CommandExecuteType,CommandExecuteTime,PreCommandItemID,SERVER_ID,NextCommandItemIDList,X,Y,IOParaCommand,CommandItemTitle,StartIOParaValue,Delayed,FixedValue,CommandItemExecuteResult,CommandItemType,Width,Height,Expand");
			strSql.Append(" FROM BatchCommandTaskItem ");
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
			strSql.Append("select count(1) FROM BatchCommandTaskItem ");
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
				strSql.Append("order by T. desc");
			}
			strSql.Append(")AS Row, T.*  from BatchCommandTaskItem T ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" ) TT");
			strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
			return DbHelperSQLite.Query(strSql.ToString());
		}
		#endregion  BasicMethod
 
	}
}

