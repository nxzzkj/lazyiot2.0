using System;
using System.Data;
using System.Text;
using System.Data.SQLite;
using Scada.DBUtility;


 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
namespace Scada.Database
{
	/// <summary>
	/// 数据访问类:ScadaMachineTrainingModel
	/// </summary>
	public partial class ScadaMachineTrainingModel: IDisposable
	{
		public ScadaMachineTrainingModel()
		{}
		#region  BasicMethod

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string Id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from ScadaMachineTrainingModel");
			strSql.Append(" where Id=@Id ");
			SQLiteParameter[] parameters = {
					new SQLiteParameter("@Id", DbType.Int64)			};
			parameters[0].Value = Id;

			return DbHelperSQLite.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(Scada.Model.ScadaMachineTrainingModel model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into ScadaMachineTrainingModel(");
			strSql.Append("Id,TaskName,Algorithm,TrainingCycle,ForecastPriod,Properties,Remark,SERVER_NAME,SERVER_ID,COMM_ID,DEVICE_ID,AlgorithmType,TrueText,FalseText,IsTrain,Detection)");
			strSql.Append(" values (");
			strSql.Append("@Id,@TaskName,@Algorithm,@TrainingCycle,@ForecastPriod,@Properties,@Remark,@SERVER_NAME,@SERVER_ID,@COMM_ID,@DEVICE_ID,@AlgorithmType,@TrueText,@FalseText,@IsTrain,@Detection)");
			SQLiteParameter[] parameters = {
					new SQLiteParameter("@Id", DbType.Int64),
					new SQLiteParameter("@TaskName", DbType.String),
					new SQLiteParameter("@Algorithm", DbType.String),
					new SQLiteParameter("@TrainingCycle", DbType.Int32,4),
					new SQLiteParameter("@ForecastPriod", DbType.Int32,4),
					new SQLiteParameter("@Properties", DbType.String),
					new SQLiteParameter("@Remark", DbType.String),
					new SQLiteParameter("@SERVER_NAME", DbType.String),
					new SQLiteParameter("@SERVER_ID", DbType.String),
			new SQLiteParameter("@COMM_ID", DbType.String),
			new SQLiteParameter("@DEVICE_ID", DbType.String),
			new SQLiteParameter("@AlgorithmType", DbType.String),
			new SQLiteParameter("@TrueText", DbType.String),
			new SQLiteParameter("@FalseText", DbType.String),
			new SQLiteParameter("@IsTrain", DbType.Int32),
				new SQLiteParameter("@Detection", DbType.String)
			
			};
			parameters[0].Value = model.Id;
			parameters[1].Value = model.TaskName;
			parameters[2].Value = model.Algorithm;
			parameters[3].Value = model.TrainingCycle;
			parameters[4].Value = model.ForecastPriod;
			parameters[5].Value = model.Properties;
			parameters[6].Value = model.Remark;
			parameters[7].Value = model.SERVER_NAME;
			parameters[8].Value = model.SERVER_ID;
			parameters[9].Value = model.COMM_ID;
			parameters[10].Value = model.DEVICE_ID;
			parameters[11].Value = model.AlgorithmType;
			parameters[12].Value = model.TrueText;
			parameters[13].Value = model.FalseText;
			parameters[14].Value = model.IsTrain;
			parameters[15].Value = model.Detection;
			

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
		public bool Update(Scada.Model.ScadaMachineTrainingModel model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update ScadaMachineTrainingModel set ");
			strSql.Append("TaskName=@TaskName,");
			strSql.Append("Algorithm=@Algorithm,");
			strSql.Append("TrainingCycle=@TrainingCycle,");
			strSql.Append("ForecastPriod=@ForecastPriod,");
			strSql.Append("Properties=@Properties,");
			strSql.Append("Remark=@Remark,");
			strSql.Append("SERVER_NAME=@SERVER_NAME,");
			strSql.Append("SERVER_ID=@SERVER_ID,");
			strSql.Append("COMM_ID=@COMM_ID,");
			strSql.Append("AlgorithmType=@AlgorithmType,");
			strSql.Append("TrueText=@TrueText,");
			strSql.Append("FalseText=@FalseText,");
			strSql.Append("IsTrain=@IsTrain,");
			strSql.Append("Detection=@Detection,");
			
			strSql.Append("DEVICE_ID=@DEVICE_ID ");
			strSql.Append(" where Id=@Id ");
			SQLiteParameter[] parameters = {
					new SQLiteParameter("@TaskName", DbType.String),
					new SQLiteParameter("@Algorithm", DbType.String),
					new SQLiteParameter("@TrainingCycle", DbType.Int32,4),
					new SQLiteParameter("@ForecastPriod", DbType.Int32,4),
					new SQLiteParameter("@Properties", DbType.String),
					new SQLiteParameter("@Remark", DbType.String),
					new SQLiteParameter("@SERVER_NAME", DbType.String),
					new SQLiteParameter("@SERVER_ID", DbType.String),
					new SQLiteParameter("@COMM_ID", DbType.String),
					new SQLiteParameter("@DEVICE_ID", DbType.String),
					new SQLiteParameter("@Id", DbType.Int64),
					new SQLiteParameter("@AlgorithmType", DbType.String),
					new SQLiteParameter("@TrueText", DbType.String),
			        new SQLiteParameter("@FalseText", DbType.String),
				    new SQLiteParameter("@IsTrain", DbType.Int32),
					new SQLiteParameter("@Detection", DbType.String)
			};
			parameters[0].Value = model.TaskName;
			parameters[1].Value = model.Algorithm;
			parameters[2].Value = model.TrainingCycle;
			parameters[3].Value = model.ForecastPriod;
			parameters[4].Value = model.Properties;
			parameters[5].Value = model.Remark;
			parameters[6].Value = model.SERVER_NAME;
			parameters[7].Value = model.SERVER_ID;
			parameters[8].Value = model.COMM_ID;
			parameters[9].Value = model.DEVICE_ID;
			parameters[10].Value = model.Id;
			parameters[11].Value = model.AlgorithmType;
			parameters[12].Value = model.TrueText;
			parameters[13].Value = model.FalseText;
			parameters[14].Value = model.IsTrain;
			parameters[15].Value = model.Detection;
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
			strSql.Append("delete from ScadaMachineTrainingModel ");
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
			strSql.Append("delete from ScadaMachineTrainingModel where SERVER_ID='"+ serverid + "' ");
		 
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
			strSql.Append("delete from ScadaMachineTrainingModel ");
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
		public Scada.Model.ScadaMachineTrainingModel GetModel(string Id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select Id,TaskName,Algorithm,AlgorithmType,TrainingCycle,ForecastPriod,Properties,Remark,SERVER_NAME,SERVER_ID,COMM_ID,DEVICE_ID,TrueText,FalseText,IsTrain,Detection from ScadaMachineTrainingModel ");
			strSql.Append(" where Id=@Id ");
			SQLiteParameter[] parameters = {
					new SQLiteParameter("@Id", DbType.Int64)			};
			parameters[0].Value = Id;

            Scada.Model.ScadaMachineTrainingModel model=new Scada.Model.ScadaMachineTrainingModel();
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
		public Scada.Model.ScadaMachineTrainingModel DataRowToModel(DataRow row)
		{
            Scada.Model.ScadaMachineTrainingModel model=new Scada.Model.ScadaMachineTrainingModel();
			if (row != null)
			{
				if(row["Id"]!=null)
				{
					model.Id= long.Parse(row["Id"].ToString());
				}
				if(row["TaskName"]!=null)
				{
					model.TaskName=row["TaskName"].ToString();
				}
				if(row["Algorithm"]!=null)
				{
					model.Algorithm=row["Algorithm"].ToString();
				}
				if(row["TrainingCycle"]!=null && row["TrainingCycle"].ToString()!="")
				{
					model.TrainingCycle=int.Parse(row["TrainingCycle"].ToString());
				}
				if(row["ForecastPriod"]!=null && row["ForecastPriod"].ToString()!="")
				{
					model.ForecastPriod=int.Parse(row["ForecastPriod"].ToString());
				}
				if(row["Properties"]!=null)
				{
					model.Properties=row["Properties"].ToString();
				}
				if(row["Remark"]!=null && row["Remark"].ToString()!="")
				{
					model.Remark=row["Remark"].ToString();
				}
				if(row["SERVER_NAME"]!=null)
				{
					model.SERVER_NAME=row["SERVER_NAME"].ToString();
				}
				if(row["SERVER_ID"]!=null)
				{
					model.SERVER_ID=row["SERVER_ID"].ToString();
				}
				if (row["COMM_ID"] != null)
				{
					model.COMM_ID = row["COMM_ID"].ToString();
				}
				if (row["DEVICE_ID"] != null)
				{
					model.DEVICE_ID = row["DEVICE_ID"].ToString();
				}
				if (row["AlgorithmType"] != null)
				{
					model.AlgorithmType = row["AlgorithmType"].ToString();
				}
				if (row["TrueText"] != null)
				{
					model.TrueText = row["TrueText"].ToString();
				}
				if (row["FalseText"] != null)
				{
					model.FalseText = row["FalseText"].ToString();
				}
				if (row["Detection"] != null)
				{
					model.Detection = row["Detection"].ToString();
				}
				

				if (row["IsTrain"] != null)
				{
					model.IsTrain = int.Parse(row["IsTrain"].ToString());
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
			strSql.Append("select Id,TaskName,Algorithm,AlgorithmType,TrainingCycle,ForecastPriod,Properties,Remark,SERVER_NAME,SERVER_ID ,COMM_ID,DEVICE_ID,TrueText,FalseText,IsTrain,Detection ");
			strSql.Append(" FROM ScadaMachineTrainingModel ");
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
			strSql.Append("select count(1) FROM ScadaMachineTrainingModel ");
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
			strSql.Append(")AS Row, T.*  from ScadaMachineTrainingModel T ");
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
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			SQLiteParameter[] parameters = {
					new SQLiteParameter("@tblName", DbType.VarChar, 255),
					new SQLiteParameter("@fldName", DbType.VarChar, 255),
					new SQLiteParameter("@PageSize", DbType.Int32),
					new SQLiteParameter("@PageIndex", DbType.Int32),
					new SQLiteParameter("@IsReCount", DbType.bit),
					new SQLiteParameter("@OrderType", DbType.bit),
					new SQLiteParameter("@strWhere", DbType.VarChar,1000),
					};
			parameters[0].Value = "ScadaMachineTrainingModel";
			parameters[1].Value = "Id";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperSQLite.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

        #endregion  BasicMethod

    }
}

