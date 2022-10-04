using System;
using System.Data;
using System.Collections.Generic;
using Scada.Common;
using Scada.Model;
using System.IO;
using System.Windows.Forms;


 
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
	/// IO_SERVER
	/// </summary>
	public partial class IO_SERVER: IDisposable
	{
		private readonly Scada.Database.IO_SERVER dal=new Scada.Database.IO_SERVER();
		public IO_SERVER()
		{}
		#region  BasicMethod
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string SERVER_ID)
		{
			return dal.Exists(SERVER_ID);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(Scada.Model.IO_SERVER model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(Scada.Model.IO_SERVER model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(string SERVER_ID)
		{
			
			return dal.Delete(SERVER_ID);
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
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string SERVER_IDlist )
		{
			return dal.DeleteList(SERVER_IDlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Scada.Model.IO_SERVER GetModel(string SERVER_ID)
		{
			
			return dal.GetModel(SERVER_ID);
		}
        public Scada.Model.IO_SERVER GetModel()
        {
            return dal.GetModel();
        }
        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public Scada.Model.IO_SERVER GetModelByCache(string SERVER_ID)
		{
			
			string CacheKey = "IO_SERVERModel-" + SERVER_ID;
			object objModel = Scada.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(SERVER_ID);
					if (objModel != null)
					{
						int ModelCache = Scada.Common.ConfigHelper.GetConfigInt("ModelCache");
						Scada.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (Scada.Model.IO_SERVER)objModel;
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
		public List<Scada.Model.IO_SERVER> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<Scada.Model.IO_SERVER> DataTableToList(DataTable dt)
		{
			List<Scada.Model.IO_SERVER> modelList = new List<Scada.Model.IO_SERVER>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				Scada.Model.IO_SERVER model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = dal.DataRowToModel(dt.Rows[n]);
					if (model != null)
					{
						modelList.Add(model);
					}
				}
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
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			return dal.GetListByPage( strWhere,  orderby,  startIndex,  endIndex);
		}
        public bool UpdateStatus(int status, string serverid)
        {
            return dal.UpdateStatus(status, serverid);
        }

        public void Dispose()
        {
             
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        //public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        //{
        //return dal.GetList(PageSize,PageIndex,strWhere);
        //}

        #endregion  BasicMethod

    }
}

