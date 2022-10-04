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
	/// 数据访问类:ScadaMachineTrainingCondition
	/// </summary>
	public partial class ScadaMachineTrainingCondition: IDisposable
	{
		public ScadaMachineTrainingCondition()
		{}
		#region  BasicMethod

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string Id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from ScadaMachineTrainingCondition");
			strSql.Append(" where Id=@Id ");
			SQLiteParameter[] parameters = {
					new SQLiteParameter("@Id", DbType.Int64)			};
			parameters[0].Value = Id;

			return DbHelperSQLite.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(Scada.Model.ScadaMachineTrainingCondition model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into ScadaMachineTrainingCondition(");
			strSql.Append("Id,TaskId,ConditionTitle,StartDate,EndDate,MarkDate,MarkTitle,DataLength,Conditions,Remark,SERVER_ID,SERVER_NAME,ConditionName,DataFile,IsTrain)");
			strSql.Append(" values (");
			strSql.Append("@Id,@TaskId,@ConditionTitle,@StartDate,@EndDate,@MarkDate,@MarkTitle,@DataLength,@Conditions,@Remark,@SERVER_ID,@SERVER_NAME,@ConditionName,@DataFile,@IsTrain)");
			SQLiteParameter[] parameters = {
					new SQLiteParameter("@Id", DbType.Int64),
					new SQLiteParameter("@TaskId", DbType.Int64),
					new SQLiteParameter("@ConditionTitle", DbType.String),
					new SQLiteParameter("@StartDate", DbType.String),
					new SQLiteParameter("@EndDate", DbType.String),
					new SQLiteParameter("@MarkDate", DbType.String),
					new SQLiteParameter("@MarkTitle", DbType.String),
					new SQLiteParameter("@DataLength", DbType.Int32,4),
					new SQLiteParameter("@Conditions", DbType.String),
					new SQLiteParameter("@Remark", DbType.String),
					new SQLiteParameter("@SERVER_ID", DbType.String),
					new SQLiteParameter("@SERVER_NAME", DbType.String),
            new SQLiteParameter("@ConditionName", DbType.String),
			new SQLiteParameter("@DataFile", DbType.String),
			new SQLiteParameter("@IsTrain", DbType.Int32),
			
			};
			

			parameters[0].Value = model.Id;
			parameters[1].Value = model.TaskId;
			parameters[2].Value = model.ConditionTitle;
			parameters[3].Value = model.StartDate;
			parameters[4].Value = model.EndDate;
			parameters[5].Value = model.MarkDate;
			parameters[6].Value = model.MarkTitle;
			parameters[7].Value = model.DataLength;
			parameters[8].Value = model.Conditions;
			parameters[9].Value = model.Remark;
			parameters[10].Value = model.SERVER_ID;
			parameters[11].Value = model.SERVER_NAME;
            parameters[12].Value = model.ConditionName;
			parameters[13].Value = model.DataFile;
			parameters[14].Value = model.IsTrain;
			


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
		public bool Update(Scada.Model.ScadaMachineTrainingCondition model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update ScadaMachineTrainingCondition set ");
			strSql.Append("TaskId=@TaskId,");
			strSql.Append("ConditionTitle=@ConditionTitle,");
			strSql.Append("StartDate=@StartDate,");
			strSql.Append("EndDate=@EndDate,");
			strSql.Append("MarkDate=@MarkDate,");
			strSql.Append("MarkTitle=@MarkTitle,");
			strSql.Append("DataLength=@DataLength,");
			strSql.Append("Conditions=@Conditions,");
			strSql.Append("Remark=@Remark,");
			strSql.Append("SERVER_ID=@SERVER_ID,");
            strSql.Append("ConditionName=@ConditionName,");
			strSql.Append("DataFile=@DataFile,");
			strSql.Append("IsTrain=@IsTrain,");
			
			strSql.Append("SERVER_NAME=@SERVER_NAME");
			strSql.Append(" where Id=@Id ");
			SQLiteParameter[] parameters = {
					new SQLiteParameter("@TaskId", DbType.Int64),
					new SQLiteParameter("@ConditionTitle", DbType.String),
					new SQLiteParameter("@StartDate", DbType.String),
					new SQLiteParameter("@EndDate", DbType.String),
					new SQLiteParameter("@MarkDate", DbType.String),
					new SQLiteParameter("@MarkTitle", DbType.String),
					new SQLiteParameter("@DataLength", DbType.Int32,4),
					new SQLiteParameter("@Conditions", DbType.String),
					new SQLiteParameter("@Remark", DbType.String),
					new SQLiteParameter("@SERVER_ID", DbType.String),
					new SQLiteParameter("@SERVER_NAME", DbType.String),
					new SQLiteParameter("@Id", DbType.Int64),
			new SQLiteParameter("@ConditionName", DbType.String),
			new SQLiteParameter("@DataFile", DbType.String),
				new SQLiteParameter("@IsTrain", DbType.Int32),
			};
			
			parameters[0].Value = model.TaskId;
			parameters[1].Value = model.ConditionTitle;
			parameters[2].Value = model.StartDate;
			parameters[3].Value = model.EndDate;
			parameters[4].Value = model.MarkDate;
			parameters[5].Value = model.MarkTitle;
			parameters[6].Value = model.DataLength;
			parameters[7].Value = model.Conditions;
			parameters[8].Value = model.Remark;
			parameters[9].Value = model.SERVER_ID;
			parameters[10].Value = model.SERVER_NAME;
			parameters[11].Value = model.Id;
            parameters[12].Value = model.ConditionName;
			parameters[13].Value = model.DataFile;
			parameters[14].Value = model.IsTrain;
			
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
		public bool Delete(string Id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from ScadaMachineTrainingCondition ");
			strSql.Append(" where Id=@Id ");
			SQLiteParameter[] parameters = {
					new SQLiteParameter("@Id", DbType.Int64)			};
			parameters[0].Value = Id;

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

		public bool Clear(string serverid)
		{

			StringBuilder strSql = new StringBuilder();
			strSql.Append("delete from ScadaMachineTrainingCondition  where SERVER_ID='"+ serverid + "'");

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
		/// 批量删除数据
		/// </summary>
		public bool DeleteList(string Idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from ScadaMachineTrainingCondition ");
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
		public Scada.Model.ScadaMachineTrainingCondition GetModel(string Id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select Id,TaskId,ConditionTitle,IsTrain,StartDate,EndDate,MarkDate,MarkTitle,DataLength,Conditions,Remark,SERVER_ID,SERVER_NAME,ConditionName,DataFile from ScadaMachineTrainingCondition ");
			strSql.Append(" where Id=@Id ");
			SQLiteParameter[] parameters = {
					new SQLiteParameter("@Id", DbType.Int64)			};
			parameters[0].Value = Id;

            Scada.Model.ScadaMachineTrainingCondition model=new Scada.Model.ScadaMachineTrainingCondition();
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
		public Scada.Model.ScadaMachineTrainingCondition DataRowToModel(DataRow row)
		{
            Scada.Model.ScadaMachineTrainingCondition model=new Scada.Model.ScadaMachineTrainingCondition();
			if (row != null)
			{
				if(row["Id"]!=null)
				{
					model.Id= long.Parse(row["Id"].ToString());
				}
				if (row["IsTrain"] != null)
				{
					model.IsTrain = int.Parse(row["IsTrain"].ToString());
				}
				
				if (row["TaskId"]!=null)
				{
					model.TaskId= long.Parse(row["TaskId"].ToString());
				}
				if(row["ConditionTitle"]!=null)
				{
					model.ConditionTitle=row["ConditionTitle"].ToString();
				}
				if(row["StartDate"]!=null)
				{
					model.StartDate=row["StartDate"].ToString();
				}
				if(row["EndDate"]!=null)
				{
					model.EndDate=row["EndDate"].ToString();
				}
				if(row["MarkDate"]!=null)
				{
					model.MarkDate=row["MarkDate"].ToString();
				}
				if(row["MarkTitle"]!=null)
				{
					model.MarkTitle=row["MarkTitle"].ToString();
				}
				if(row["DataLength"]!=null && row["DataLength"].ToString()!="")
				{
					model.DataLength=int.Parse(row["DataLength"].ToString());
				}
				if(row["Conditions"]!=null)
				{
					model.Conditions=row["Conditions"].ToString();
				}
				if(row["Remark"]!=null)
				{
					model.Remark=row["Remark"].ToString();
				}
				if(row["SERVER_ID"]!=null)
				{
					model.SERVER_ID=row["SERVER_ID"].ToString();
				}
				if(row["SERVER_NAME"]!=null)
				{
					model.SERVER_NAME=row["SERVER_NAME"].ToString();
				}
                if (row["ConditionName"] != null)
                {
                    model.ConditionName = row["ConditionName"].ToString();
                }
				if (row["DataFile"] != null)
				{
					model.DataFile = row["DataFile"].ToString();
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
			strSql.Append("select Id,TaskId,IsTrain,ConditionTitle,StartDate,EndDate,MarkDate,MarkTitle,DataLength,Conditions,Remark,SERVER_ID,SERVER_NAME,ConditionName,DataFile ");
			strSql.Append(" FROM ScadaMachineTrainingCondition ");
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
			strSql.Append("select count(1) FROM ScadaMachineTrainingCondition ");
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
			strSql.Append(")AS Row, T.*  from ScadaMachineTrainingCondition T ");
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

        #endregion  BasicMethod

    }
}

