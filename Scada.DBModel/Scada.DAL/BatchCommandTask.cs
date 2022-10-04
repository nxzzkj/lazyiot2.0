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
	/// 数据访问类:BatchCommandTask
	/// </summary>
	public partial class BatchCommandTaskModel
	{
		public BatchCommandTaskModel()
		{}
		#region  BasicMethod

		public bool Clear(string serverid)
		{

			StringBuilder strSql = new StringBuilder();
			strSql.Append("delete from BatchCommandTask  where SERVER_ID='" + serverid + "'");

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

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(Scada.Model.BatchCommandTaskModel model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into BatchCommandTask(");
			strSql.Append("Id,CommandTaskTitle,CommandTaskCreateTime,CommandTaskRemark,TaskStartRunType,ExecuteTaskTimingTime,SERVER_ID,IOStartConditionValue,MachineTrainingTaskId,ManualTask,StartCommandItemID)");
			strSql.Append(" values (");
			strSql.Append("@Id,@CommandTaskTitle,@CommandTaskCreateTime,@CommandTaskRemark,@TaskStartRunType,@ExecuteTaskTimingTime,@SERVER_ID,@IOStartConditionValue,@MachineTrainingTaskId,@ManualTask,@StartCommandItemID)");
			SQLiteParameter[] parameters = {
					new SQLiteParameter("@Id", DbType.String),
					new SQLiteParameter("@CommandTaskTitle", DbType.String),
					new SQLiteParameter("@CommandTaskCreateTime", DbType.String),
					new SQLiteParameter("@CommandTaskRemark", DbType.String),
					new SQLiteParameter("@TaskStartRunType", DbType.String),
					new SQLiteParameter("@ExecuteTaskTimingTime", DbType.String),
			 
					new SQLiteParameter("@SERVER_ID", DbType.String),
					new SQLiteParameter("@IOStartConditionValue", DbType.String),
					new SQLiteParameter("@MachineTrainingTaskId", DbType.String),
							new SQLiteParameter("@ManualTask", DbType.String),
	new SQLiteParameter("@StartCommandItemID", DbType.String)
							




			};
			parameters[0].Value = model.Id;
			parameters[1].Value = model.CommandTaskTitle;
			parameters[2].Value = model.CommandTaskCreateTime;
			parameters[3].Value = model.CommandTaskRemark;
			parameters[4].Value = model.TaskStartRunType;
			parameters[5].Value = model.ExecuteTaskTimingTime;
			parameters[6].Value = model.SERVER_ID;
			parameters[7].Value = model.IOStartConditionValue;
			parameters[8].Value = model.MachineTrainingTaskId;
			parameters[9].Value = model.ManualTask;
			parameters[10].Value = model.StartCommandItemID;
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
		public bool Update(Scada.Model.BatchCommandTaskModel model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update BatchCommandTask set ");
			strSql.Append("Id=@Id,");
			strSql.Append("CommandTaskTitle=@CommandTaskTitle,");
			strSql.Append("CommandTaskCreateTime=@CommandTaskCreateTime,");
			strSql.Append("CommandTaskRemark=@CommandTaskRemark,");
			strSql.Append("TaskStartRunType=@TaskStartRunType,");
			strSql.Append("ExecuteTaskTimingTime=@ExecuteTaskTimingTime,");
			 
			strSql.Append("SERVER_ID=@SERVER_ID,");
			strSql.Append("ManualTask=@ManualTask,");
			strSql.Append("StartCommandItemID=@StartCommandItemID,");
			
			strSql.Append("MachineTrainingTaskId=@MachineTrainingTaskId,");
			strSql.Append("IOStartConditionValue=@IOStartConditionValue");
			strSql.Append(" where ");
			SQLiteParameter[] parameters = {
					new SQLiteParameter("@Id", DbType.String),
					new SQLiteParameter("@CommandTaskTitle", DbType.String),
					new SQLiteParameter("@CommandTaskCreateTime", DbType.String),
					new SQLiteParameter("@CommandTaskRemark", DbType.String),
					new SQLiteParameter("@TaskStartRunType", DbType.String),
					new SQLiteParameter("@ExecuteTaskTimingTime", DbType.String),
					 
					new SQLiteParameter("@SERVER_ID", DbType.String),
					new SQLiteParameter("@IOStartConditionValue", DbType.String),
					new SQLiteParameter("@MachineTrainingTaskId", DbType.String),
					new SQLiteParameter("@ManualTask", DbType.String),
					new SQLiteParameter("@StartCommandItemID", DbType.String)


					
			};
			parameters[0].Value = model.Id;
			parameters[1].Value = model.CommandTaskTitle;
			parameters[2].Value = model.CommandTaskCreateTime;
			parameters[3].Value = model.CommandTaskRemark;
			parameters[4].Value = model.TaskStartRunType;
			parameters[5].Value = model.ExecuteTaskTimingTime;
	 
 
			parameters[6].Value = model.SERVER_ID;
			parameters[7].Value = model.IOStartConditionValue;
			parameters[8].Value = model.MachineTrainingTaskId;
			parameters[9].Value = model.ManualTask;
			parameters[10].Value = model.StartCommandItemID;
			


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
			strSql.Append("delete from BatchCommandTask ");
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
		public Scada.Model.BatchCommandTaskModel GetModel()
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select Id,CommandTaskTitle,CommandTaskCreateTime,CommandTaskRemark,TaskStartRunType,ExecuteTaskTimingTime,SERVER_ID,IOStartConditionValue,MachineTrainingTaskId,ManualTask,StartCommandItemID from BatchCommandTask ");
			strSql.Append(" where ");
			SQLiteParameter[] parameters = {
			};

			Scada.Model.BatchCommandTaskModel model =new Scada.Model.BatchCommandTaskModel();
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
		public Scada.Model.BatchCommandTaskModel DataRowToModel(DataRow row)
		{
			Scada.Model.BatchCommandTaskModel model =new Scada.Model.BatchCommandTaskModel();
			if (row != null)
			{
				if(row["Id"]!=null)
				{
					model.Id=row["Id"].ToString();
				}
				if(row["CommandTaskTitle"]!=null)
				{
					model.CommandTaskTitle=row["CommandTaskTitle"].ToString();
				}
				if(row["CommandTaskCreateTime"]!=null)
				{
					model.CommandTaskCreateTime=row["CommandTaskCreateTime"].ToString();
				}
				if(row["CommandTaskRemark"]!=null)
				{
					model.CommandTaskRemark=row["CommandTaskRemark"].ToString();
				}
				if(row["TaskStartRunType"]!=null)
				{
					model.TaskStartRunType=row["TaskStartRunType"].ToString();
				}
				if(row["ExecuteTaskTimingTime"]!=null)
				{
					model.ExecuteTaskTimingTime=row["ExecuteTaskTimingTime"].ToString();
				}
				 
				if(row["SERVER_ID"]!=null)
				{
					model.SERVER_ID=row["SERVER_ID"].ToString();
				}
				if(row["IOStartConditionValue"]!=null)
				{
					model.IOStartConditionValue=row["IOStartConditionValue"].ToString();
				}
				if (row["MachineTrainingTaskId"] != null)
				{
					model.MachineTrainingTaskId = row["MachineTrainingTaskId"].ToString();
				}
				if (row["ManualTask"] != null)
				{
					model.ManualTask = row["ManualTask"].ToString();
				}
				if (row["StartCommandItemID"] != null)
				{
					model.StartCommandItemID = row["StartCommandItemID"].ToString();
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
			strSql.Append("select Id,CommandTaskTitle,CommandTaskCreateTime,CommandTaskRemark,TaskStartRunType,ExecuteTaskTimingTime,SERVER_ID,IOStartConditionValue,MachineTrainingTaskId,ManualTask,StartCommandItemID ");
			strSql.Append(" FROM BatchCommandTask ");
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
			strSql.Append("select count(1) FROM BatchCommandTask ");
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
			strSql.Append(")AS Row, T.*  from BatchCommandTask T ");
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

