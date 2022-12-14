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
	/// ScadaMachineTrainingCondition
	/// </summary>
	public partial class ScadaMachineTrainingCondition: IDisposable
	{
		private readonly Scada.Database.ScadaMachineTrainingCondition dal=new Scada.Database.ScadaMachineTrainingCondition();
		public ScadaMachineTrainingCondition()
		{}
		#region  BasicMethod
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string Id)
		{
			return dal.Exists(Id);
		}
		public bool Clear(string serverid)
		{
			return dal.Clear(serverid);
		}
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(Scada.Model.ScadaMachineTrainingCondition model)
		{
			return dal.Add(model);
		}
		public void Add(List<Scada.Model.ScadaMachineTrainingCondition> models)
		{
			if (models == null || models.Count <= 0)
				return;
			models.ForEach(delegate (Scada.Model.ScadaMachineTrainingCondition conditionModel) {


				dal.Add(conditionModel);
			});

		}
		public void Add(ConcurrentBag<Scada.Model.ScadaMachineTrainingCondition> models)
		{
			if (models == null || models.Count <= 0)
				return;
			while (models.Count > 0)
			{
				Scada.Model.ScadaMachineTrainingCondition conditionModel;
				models.TryTake(out conditionModel);
				if (conditionModel != null)
					dal.Add(conditionModel);
			};

		}
		
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(Scada.Model.ScadaMachineTrainingCondition model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(string Id)
		{
			
			return dal.Delete(Id);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string Idlist )
		{
			return dal.DeleteList(Idlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Scada.Model.ScadaMachineTrainingCondition GetModel(string Id)
		{
			
			return dal.GetModel(Id);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public Scada.Model.ScadaMachineTrainingCondition GetModelByCache(string Id)
		{
			
			string CacheKey = "ScadaMachineTrainingConditionModel-" + Id;
			object objModel = Scada.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(Id);
					if (objModel != null)
					{
						int ModelCache = Scada.Common.ConfigHelper.GetConfigInt("ModelCache");
                        Scada.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (Scada.Model.ScadaMachineTrainingCondition)objModel;
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
		public List<Scada.Model.ScadaMachineTrainingCondition> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<Scada.Model.ScadaMachineTrainingCondition> DataTableToList(DataTable dt)
		{
			List<Scada.Model.ScadaMachineTrainingCondition> modelList = new List<Scada.Model.ScadaMachineTrainingCondition>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
                Scada.Model.ScadaMachineTrainingCondition model;
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

