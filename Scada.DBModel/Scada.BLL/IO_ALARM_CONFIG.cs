using System;
using System.Data;
using System.Collections.Generic;
using Scada.Common;
using Scada.Model;
using System.Collections.Concurrent;


 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
namespace Scada.Business
{
	/// <summary>
	/// IO_ALARM_CONFIG
	/// </summary>
	public partial class IO_ALARM_CONFIG: IDisposable
	{
		private readonly Scada.Database.IO_ALARM_CONFIG dal=new Scada.Database.IO_ALARM_CONFIG();
		public IO_ALARM_CONFIG()
		{}
		#region  BasicMethod

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(Scada.Model.IO_ALARM_CONFIG model)
		{
			return dal.Add(model);
		}
        public void  Add(List<Scada.Model.IO_ALARM_CONFIG> models)
        {
              dal.Add(models);
        }
		public void Add(ConcurrentStack<Scada.Model.IO_ALARM_CONFIG> models)
		{
			dal.Add(models);
		}
		
		public bool UpdateStatus(int status, string serverid)
        {
            return dal.UpdateStatus(status, serverid);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Scada.Model.IO_ALARM_CONFIG model)
		{
			return dal.Update(model);
		}
        public bool UserResultUpdate(Scada.Model.IO_ALARM_CONFIG model)
        {
            return dal.UserResultUpdate(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string IO_ID)
		{
			//该表无主键信息，请自定义主键/条件字段
			return dal.Delete(IO_ID);
		}
        public bool DeleteCommunication(string IO_SERVER_ID, string IO_COMM_ID)
        {
            return dal.DeleteCommunication(IO_SERVER_ID, IO_COMM_ID);
        }
        public bool DeleteDevice(string IO_SERVER_ID, string IO_DEVICE_ID)
        {
            return dal.DeleteDevice(IO_SERVER_ID, IO_DEVICE_ID);
        }
        public bool Clear(string SERVER_ID)
        {

            return dal.Clear(SERVER_ID);
        }
        public bool Clear()
        {

            return dal.Clear();
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Scada.Model.IO_ALARM_CONFIG GetModel(string IO_ID)
		{
			//该表无主键信息，请自定义主键/条件字段
			return dal.GetModel(IO_ID);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public Scada.Model.IO_ALARM_CONFIG GetModelByCache(string IO_ID)
		{
			//该表无主键信息，请自定义主键/条件字段
			string CacheKey = "IO_ALARM_CONFIGModel-" + IO_ID;
			object objModel = Scada.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(IO_ID);
					if (objModel != null)
					{
						int ModelCache = Scada.Common.ConfigHelper.GetConfigInt("ModelCache");
						Scada.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (Scada.Model.IO_ALARM_CONFIG)objModel;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public ConcurrentStack<Scada.Model.IO_ALARM_CONFIG> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public ConcurrentStack<Scada.Model.IO_ALARM_CONFIG> DataTableToList(DataTable dt)
		{
			ConcurrentStack<Scada.Model.IO_ALARM_CONFIG> modelList = new ConcurrentStack<Scada.Model.IO_ALARM_CONFIG>();
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				modelList.Push(dal.DataRowToModel(dt.Rows[i]));
			}
			return modelList;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}

		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			return dal.GetRecordCount(strWhere);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetListByPage(string strWhere,  int pageIndex, int pagesize)
		{
			return dal.GetListByPage( strWhere, pageIndex, pagesize);
		}

        public void Dispose()
        {
           
        }
 

        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}

