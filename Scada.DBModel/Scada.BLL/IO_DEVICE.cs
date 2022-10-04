using System;
using System.Data;
using System.Collections.Generic;
using Scada.Common;
using Scada.Model;
using System.Threading.Tasks;
using System.Linq;
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
	/// IO_DEVICE
	/// </summary>
	public partial class IO_DEVICE: IDisposable
    {
		private readonly Scada.Database.IO_DEVICE dal=new Scada.Database.IO_DEVICE();
		public IO_DEVICE()
		{}
		#region  BasicMethod

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(Scada.Model.IO_DEVICE model)
		{
			return dal.Add(model);
		}
        public void Add(List<Scada.Model.IO_DEVICE> models)
        {
            if (models == null || models.Count <= 0)
                return;
              dal.Add(models);
        }
        public void Add(ConcurrentBag<Scada.Model.IO_DEVICE> models)
        {
            if (models == null || models.Count <= 0)
                return;
            dal.Add(models);
        }

        
        public bool UpdateStatus(int status, string serverid)
        {
            return dal.UpdateStatus(status, serverid);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Scada.Model.IO_DEVICE model)
		{
			return dal.Update(model);
		}
        IO_PARA paraDal = new IO_PARA();
        IO_ALARM_CONFIG configDal = new IO_ALARM_CONFIG();
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string IO_SERVER_ID, string IO_DEVICE_ID)
		{
			//该表无主键信息，请自定义主键/条件字段
			bool res= dal.Delete(IO_DEVICE_ID);
            if(res)
            {
              
                paraDal.DeleteDevice(IO_SERVER_ID, IO_DEVICE_ID);
                configDal.DeleteDevice(IO_SERVER_ID, IO_DEVICE_ID);
            }
            return res;

        }
        public bool DeleteCommunication(string serverid, string comm_id)
        {
            //该表无主键信息，请自定义主键/条件字段
            bool res = dal.DeleteCommunication(serverid, comm_id);
            
            return res;


        }

        public bool Clear(string SERVER_ID)
        {

            return dal.Clear(SERVER_ID.Trim());
        }
        public bool Clear()
        {

            return dal.Clear();
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Scada.Model.IO_DEVICE GetModel(string IO_DEVICE_ID)
		{
			//该表无主键信息，请自定义主键/条件字段
			return dal.GetModel(IO_DEVICE_ID);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public Scada.Model.IO_DEVICE GetModelByCache(string IO_DEVICE_ID)
		{
			//该表无主键信息，请自定义主键/条件字段
			string CacheKey = "IO_DEVICEModel-"+ IO_DEVICE_ID;
			object objModel = Scada.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(IO_DEVICE_ID);
					if (objModel != null)
					{
						int ModelCache = Scada.Common.ConfigHelper.GetConfigInt("ModelCache");
						Scada.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (Scada.Model.IO_DEVICE)objModel;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
        IO_PARA paraBll = new IO_PARA();
       
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Scada.Model.IO_DEVICE> GetModelList(string strWhere)
        {

          
            DataSet ds = dal.GetList(strWhere);
            List<Scada.Model.IO_DEVICE>  res=DataTableToList(ds.Tables[0], strWhere);
          
            return res;
        }
        SCADA_DEVICE_DRIVER driverBLL = new SCADA_DEVICE_DRIVER();
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<Scada.Model.IO_DEVICE> DataTableToList(DataTable dt, string strWhere)
        {
            
            List<Scada.Model.SCADA_DEVICE_DRIVER> drivers = driverBLL.GetModelList("");
      
            List<List<Scada.Model.IO_PARA>> allParas = paraBll.GetModelList(strWhere);
       
            ConcurrentStack<Scada.Model.IO_DEVICE> modelList = new ConcurrentStack<Scada.Model.IO_DEVICE>();

          
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow devicedr = dt.Rows[i];
                Scada.Model.IO_DEVICE device = dal.DataRowToModel(devicedr, drivers);
                if (device != null)
                {
                    
                        device.IOParas = new List<Model.IO_PARA>();
                        allParas.ForEach(delegate (List<Scada.Model.IO_PARA> paras)
                        {
                            if (paras != null)
                            {
                                List<Scada.Model.IO_PARA> paraFind = paras.FindAll(x => x != null
                         && x.IO_DEVICE_ID == device.IO_DEVICE_ID
                         && x.IO_COMM_ID == device.IO_COMM_ID
                         && x.IO_SERVER_ID == device.IO_SERVER_ID);
                                if (paraFind.Count > 0)
                                    device.IOParas.AddRange(paraFind);
                            }

                        });
                        modelList.Push(device);

                   
                   
                }


            }
          
          
            allParas = null;
            drivers.Clear();
            drivers = null;
            List<Scada.Model.IO_DEVICE> devices = modelList.ToList();
            modelList.Clear();
            modelList = null;
            return devices;

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
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}

