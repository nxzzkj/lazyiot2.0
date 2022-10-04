using System;
using System.Data;
using System.Collections.Generic;
using Scada.Common;
using Scada.Model;
using System.Threading.Tasks;
using System.Threading;
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
    /// IO_PARA
    /// </summary>
    public partial class IO_PARA : IDisposable
    {
        private   Scada.Database.IO_PARA dal = new Scada.Database.IO_PARA();
        public IO_PARA()
        { }
        #region  BasicMethod
        IO_ALARM_CONFIG alarmBll = new IO_ALARM_CONFIG();
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(Scada.Model.IO_PARA model)
        {
            if (dal.Add(model))
            {
                alarmBll.Add(model.AlarmConfig);
                return true;
            }
            else
                return false;


        }
        public void Add(List<Scada.Model.IO_PARA> models, List<Scada.Model.IO_ALARM_CONFIG> configs)
        {
            if (models != null && models.Count > 0)
                dal.Add(models);
            if (configs != null && configs.Count > 0)
                alarmBll.Add(configs);
        }
        public void Add(ConcurrentStack<Scada.Model.IO_PARA> models, ConcurrentStack<Scada.Model.IO_ALARM_CONFIG> configs)
        {
            if (models != null && models.Count > 0)
                dal.Add(models);
            if (configs != null && configs.Count > 0)
                alarmBll.Add(configs);
        }
        public void Add(ConcurrentStack<Scada.Model.IO_PARA> models)
        {
            if (models != null && models.Count > 0)
                dal.Add(models);
            
        }
        public void Add(ConcurrentStack<Scada.Model.IO_ALARM_CONFIG> configs)
        {
           
            if (configs != null && configs.Count > 0)
                alarmBll.Add(configs);
        }
        public bool UpdateStatus(int status, string serverid)
        {
            return dal.UpdateStatus(status, serverid);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Scada.Model.IO_PARA model)
        {
            if (dal.Update(model))
            {
                alarmBll.Update(model.AlarmConfig);
                return true;
            }

            return false;

        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string IO_ID)
        {
            //该表无主键信息，请自定义主键/条件字段
            if (dal.Delete(IO_ID))
            {
                alarmBll.Delete(IO_ID);
            }


            return dal.Delete(IO_ID);
        }
        /// <summary>
        /// 删除通道下的
        /// </summary>
        /// <param name="serverid"></param>
        /// <param name="comm_id"></param>
        /// <returns></returns>
        public bool DeleteCommunication(string serverid, string comm_id)
        {
            //该表无主键信息，请自定义主键/条件字段
            bool res = dal.DeleteCommunication(serverid, comm_id);
            if (res)
            {
                alarmBll.DeleteCommunication(serverid, comm_id);
            }
            return res;


        }
        /// <summary>
        /// 删除设备下的
        /// </summary>
        /// <param name="serverid"></param>
        /// <param name="deviceid"></param>
        /// <returns></returns>
        public bool DeleteDevice(string serverid, string deviceid)
        {
            //该表无主键信息，请自定义主键/条件字段
            bool res = dal.DeleteDevice(serverid, deviceid);
            if (res)
            {
                alarmBll.DeleteDevice(serverid, deviceid);
            }
            return res;


        }

        public bool Clear(string SERVER_ID)
        {

            dal.Clear(SERVER_ID.Trim());
            alarmBll.Clear(SERVER_ID.Trim());
            return true;

        }
        public bool Clear()
        {

            dal.Clear();
            alarmBll.Clear();
            return true;

        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Scada.Model.IO_PARA GetModel(string IO_ID)
        {
            //该表无主键信息，请自定义主键/条件字段
            return dal.GetModel(IO_ID);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public Scada.Model.IO_PARA GetModelByCache(string IO_ID)
        {
            //该表无主键信息，请自定义主键/条件字段
            string CacheKey = "IO_PARAModel-" + IO_ID;
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
                catch { }
            }
            return (Scada.Model.IO_PARA)objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }
        IO_ALARM_CONFIG alarmDal = new IO_ALARM_CONFIG();
        /// <summary>
        /// 获得数据列表,由于list 集合不能太大，所以要分区域读取，每个区域存储2wio点
        /// </summary>
        public List<List<Scada.Model.IO_PARA>> GetModelList(string strWhere)
        {
            DateTime dt1 = DateTime.Now;
            List<List<Scada.Model.IO_PARA>> result = new List<List<Scada.Model.IO_PARA>>();
            List<List<Scada.Model.IO_ALARM_CONFIG>> resultConfig = new List<List<Scada.Model.IO_ALARM_CONFIG>>();
            List<Task> tasks = new List<Task>();
            #region
            {
              
                int num = this.alarmDal.GetRecordCount(strWhere);
                int page = num / 20000;
                if (num % 20000 != 0)
                {
                    page++;
                }
             
                for (int i = 0; i < page; i++)
                {
                    int index = i;
                    var task = Task.Factory.StartNew(  () =>
                    {
                        List<Scada.Model.IO_ALARM_CONFIG> subRes = new List<Model.IO_ALARM_CONFIG>();
                        DataSet ds = alarmDal.GetListByPage(strWhere, index, 20000);
                        ConcurrentStack<Scada.Model.IO_ALARM_CONFIG> res = alarmDal.DataTableToList(ds.Tables[0]);
                        Scada.Model.IO_ALARM_CONFIG[] paras = res.ToArray();
                        if(paras.Length>0)
                        {
                            subRes.AddRange(paras);
                            resultConfig.Add(subRes);

                        }
                        res.Clear();
                        res = null;

                    });
                    tasks.Add(task);
                }
             
            }
            #endregion
            #region
            {
                int num = dal.GetRecordCount(strWhere);
                int page = num / 20000;
                if (num % 20000 != 0)
                {
                    page++;
                }
              
                for (int i = 0; i < page; i++)
                {
                    int index = i;
                    var task = Task.Factory.StartNew(  () =>
                    {
                        List<Scada.Model.IO_PARA> subRes = new List<Model.IO_PARA>();
                        DataSet ds = dal.GetListByPage(strWhere, index, 20000);
                        ConcurrentStack<Scada.Model.IO_PARA> res = DataTableToList(ds.Tables[0]);
                        Scada.Model.IO_PARA[] paras = res.ToArray();
                        if(paras.Length>0)
                        {
                            subRes.AddRange(paras);
                            result.Add(subRes);
                        }
                      
                        res.Clear();
                        res = null;

                    });
                    tasks.Add(task);
                }
               
            }
            #endregion
            #region 
            {
               
                for (int i=0;i< result.Count;i++)
                {
                    List<Scada.Model.IO_PARA> items = result[i];
                    var task = Task.Factory.StartNew(  () => {
                        for(int j=0;j< items.Count;j++)
                        {
                          
                            resultConfig.ForEach(delegate (List<Scada.Model.IO_ALARM_CONFIG> congs) {
                                items[j].AlarmConfig = congs.Find(x=>x.IO_SERVER_ID== items[j].IO_SERVER_ID
                                &&x.IO_COMM_ID== items[j].IO_COMM_ID
                                &&x.IO_DEVICE_ID== items[j].IO_DEVICE_ID
                                &&x.IO_ID== items[j].IO_ID);
                            });
                            
                           
                            if (items[j].AlarmConfig == null)
                                items[j].AlarmConfig = new Model.IO_ALARM_CONFIG();
                        }
                    });
                    tasks.Add(task);
                }
             

            }
            #endregion
           Task.WaitAll(tasks.ToArray());
            DateTime dt2 = DateTime.Now;
            TimeSpan timeSpan = dt2 - dt1;
            for (int i = 0; i < resultConfig.Count; i++)
            {
                resultConfig[i].Clear();
                resultConfig[i] = null;
            }
                resultConfig.Clear();
            resultConfig = null;
            return result;

        }


        public ConcurrentStack<Scada.Model.IO_PARA> DataTableToList(DataTable dt)
        {
            ConcurrentStack<Scada.Model.IO_PARA> modelList = new ConcurrentStack<Scada.Model.IO_PARA>();
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
		public DataSet GetListByPage(string strWhere, int pageIndex, int pageSize)
		{
			return dal.GetListByPage( strWhere, pageIndex, pageSize);
		}

        public void Dispose()
        {
            dal.Dispose();
        }
        

        #endregion  BasicMethod

    }
}

