using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace Scada.DBUtility
{
    public class WebCacheIOData : ISerializable, IDisposable
    {
        public readonly int QualityStampValue = -9999;
        public WebCacheIOData()
        {
            ParaName = "";
            ParaValue = "-9999";
            Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            QualityStamp = "BAD";
          
        }
        #region  序列化和反序列化

        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected WebCacheIOData(SerializationInfo info, StreamingContext context)
        {

            #region 自定义属性
            this.ParaName = (string)info.GetValue("ParaName", typeof(string));
            this.ParaValue = (string)info.GetValue("ParaValue", typeof(string));
            this.Date = (string)info.GetValue("Date", typeof(string));
            this.ParaID = (string)info.GetValue("ParaID", typeof(string));
            this.QualityStamp = (string)info.GetValue("QualityStamp", typeof(string));
          
            #endregion

        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ParaName", this.ParaName);
            info.AddValue("ParaValue", this.ParaValue);
            info.AddValue("Date", this.Date);
            info.AddValue("ParaID", this.ParaID);
            info.AddValue("QualityStamp", this.QualityStamp);
          
           

        }

        public void Dispose()
        {
 
            ParaName = "";
            ParaValue = "";
            ParaID = "";
            Date = "";
            QualityStamp = "";
        
        }

        #endregion
        
        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParaName
        {
            set;
            get;
        }

        /// <summary>
        /// 数据所在的设备id
        /// </summary>
        public string ParaID
        {
            set;
            get;
        }
        
        /// <summary>
        /// 接收数据的值
        /// </summary>
        public string ParaValue
        {
            set;
            get;
        }
        /// <summary>
        /// 接收数据的日期
        /// </summary>
        public string  Date
        {
            set;
            get;
        }
        private string mQualityStamp ="BAD";
        /// <summary>
        /// 数据质量戳
        /// </summary>
        public string QualityStamp
        {
            set
            {
                mQualityStamp = value;
                if (mQualityStamp == "BAD")
                {
                    ParaValue = this.QualityStampValue.ToString();
                }
            }
            get { return mQualityStamp; }
        }

        

    }
    [Serializable]
    public class WebCacheDataItem : ISerializable
    {
        public WebCacheDataItem()
        {
            ServerId = "";
            CommunicationId = "";
            DeviceId = "";


        }
        public virtual string Key
        {
            get
            {
                return ServerId + "/" + CommunicationId + "/" + DeviceId + "";
            }
        }
        public string ServerId { set; get; }
        public string CommunicationId { set; get; }
        public string DeviceId { set; get; }



        public WebCacheDataItem(SerializationInfo info, StreamingContext context)
        {

            ServerId = (string)info.GetValue("ServerId", typeof(string));
            CommunicationId = (string)info.GetValue("CommunicationId", typeof(string));
            DeviceId = (string)info.GetValue("DeviceId", typeof(string));


        }
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ServerId", ServerId);
            info.AddValue("CommunicationId", CommunicationId);
            info.AddValue("DeviceId", DeviceId);


        }
    }
    [Serializable]
    public class RealWebCacheDataItem : WebCacheDataItem
    {
        public string Date
        { set; get; }
        public override string Key
        {
            get
            {
                return base.Key + "/Real";
            }
        }
        public List<WebCacheIOData> ParaReals { set; get; }
        public RealWebCacheDataItem()
        {
            ParaReals = new List<WebCacheIOData>();
            Date = "";
        }
       
        public RealWebCacheDataItem(SerializationInfo info, StreamingContext context) : base(info, context)
        {

            ParaReals = (List<WebCacheIOData>)info.GetValue("ParaReals", typeof(List<WebCacheIOData>));
            Date = (string)info.GetValue("Date", typeof(string));

        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("ParaReals", ParaReals);
            info.AddValue("Date", Date);
        }
    }
    /// <summary>
    /// 定义一个实时报警的缓存
    /// </summary>
    [Serializable]
    public class AlarmWebCacheDataItem : WebCacheDataItem
    {
        public override string Key
        {
            get
            {
                return base.Key +"/"+ ParaName+"/Alarm";
            }
        }
        public AlarmWebCacheDataItem()
        {
            AlarmValue = "";
            AlarmTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Disposalidea = "";
            Disposaluser = "";
            Alarmlevel = "";
            AlarmType = "";
            ParaName = "";
            DeviceName = "";
            ParaLabel = "";
            ParaId = "";
        }
        public string AlarmValue { set; get; }
        public string AlarmTime { set; get; }

        public string Disposalidea { set; get; }
        public string Disposaluser { set; get; }
        public string Alarmlevel { set; get; }
        public string AlarmType { set; get; }
        public string ParaName { set; get; }
        public string ParaId { set; get; }
        public string DeviceName { set; get; }
        public string ParaLabel { set; get; }

        public AlarmWebCacheDataItem(SerializationInfo info, StreamingContext context) : base(info, context)
        {

            AlarmValue = (string)info.GetValue("AlarmValue", typeof(string));
            AlarmTime = (string)info.GetValue("AlarmTime", typeof(string));
            Disposalidea = (string)info.GetValue("Disposalidea", typeof(string));
            Disposaluser = (string)info.GetValue("Disposaluser", typeof(string));
            Alarmlevel = (string)info.GetValue("Alarmlevel", typeof(string));
            AlarmType = (string)info.GetValue("AlarmType", typeof(string));
            ParaName = (string)info.GetValue("ParaName", typeof(string));
            DeviceName = (string)info.GetValue("DeviceName", typeof(string));
            ParaLabel = (string)info.GetValue("ParaLabel", typeof(string));
            ParaId = (string)info.GetValue("ParaId", typeof(string));
            
        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("AlarmValue", AlarmValue);
            info.AddValue("AlarmTime", AlarmTime);
            info.AddValue("Disposalidea", Disposalidea);
            info.AddValue("Disposaluser", Disposaluser);
            info.AddValue("Alarmlevel", Alarmlevel);
            info.AddValue("AlarmType", AlarmType);
            info.AddValue("ParaName", ParaName);
            info.AddValue("DeviceName", DeviceName);
            info.AddValue("ParaLabel", ParaLabel);
            info.AddValue("ParaId", ParaId);
            
        }
    }

    /// <summary>
    /// 定义一个设备状态的缓存类
    /// </summary>
    public class StatusWebCacheDataItem : WebCacheDataItem
    {
       
        /// <summary>
        /// 对应获取的实时值
        /// </summary>
        public string Status
        {
            set;
            get;
        }
        /// <summary>
        /// 对应获取的数据时间戳
        /// </summary>
        public string DateTime
        {
            set;
            get;
        }
        public StatusWebCacheDataItem()
        {


            Status = "";
            DateTime = "";

        }
        public StatusWebCacheDataItem(SerializationInfo info, StreamingContext context)
        {


            Status = (string)info.GetValue("Status", typeof(string));
            DateTime = (string)info.GetValue("DateTime", typeof(string));

        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            Status = (string)info.GetValue("Status", typeof(string));
            DateTime = (string)info.GetValue("DateTime", typeof(string));
        }

        public override string Key
        {
            get
            {
                return base.Key + "/Status";
            }
        }

    }

    /// <summary>
    /// 定义一个设备状态的缓存类
    /// </summary>
    public class MachineTrainForecastWebCacheDataItem : WebCacheDataItem
    {
        public override string Key
        {
            get
            {
                return base.Key +"/"+ TaskId + "/MachineTrainForecast";
            }
        }
        public MachineTrainForecastWebCacheDataItem()
        {
            SERVER_NAME = "";
            COMM_NAME = "";
            DEVICE_NAME = "";
            TaskName = "";
            Algorithm = "";
            AlgorithmType = "";
            FeaturesName = "";
            FeaturesValue = "";
            PredictedDate = DateTime.Now;
            PredictedLabel = "";
            Score = "";
            Remark = "";

        }
        #region Model

   
     
        private string _server_name;
   
        public string COMM_NAME
        {
            set; get;
        }
        public string DEVICE_NAME
        {
            set; get;
        }

      
        public string TaskName
        {
            set; get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string TaskId
        {
            set;get;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Algorithm
        {
            set; get;
        }
        public string AlgorithmType
        {
            set; get;
        }
 
        public string FeaturesName
        {
            set; get;
        }

    

        /// <summary>
        /// 
        /// </summary>
        public string FeaturesValue
        {
            set; get;
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime PredictedDate
        {
            set; get;
        }
        /// <summary>
        /// 
        /// </summary>
        public string PredictedLabel
        {
            set; get;
        }
        public string Score
        { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string Remark
        {
            set; get;
        }
 
        /// <summary>
        /// 
        /// </summary>
        public string SERVER_NAME
        {
            set { _server_name = value; }
            get { return _server_name; }
        }
        public void Dispose()
        {

        }
        public MachineTrainForecastWebCacheDataItem(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.Algorithm = (string)info.GetValue("Algorithm", typeof(string));
            this.AlgorithmType = (string)info.GetValue("AlgorithmType", typeof(string));
     
            this.COMM_NAME = (string)info.GetValue("COMM_NAME", typeof(string));
   
            this.DEVICE_NAME = (string)info.GetValue("DEVICE_NAME", typeof(string));
            this.FeaturesName = (string)info.GetValue("FeaturesName", typeof(string));
            this.FeaturesValue = (string)info.GetValue("FeaturesValue", typeof(string));
            this.PredictedDate = (DateTime)info.GetValue("PredictedDate", typeof(DateTime));
            this.Remark = (string)info.GetValue("Remark", typeof(string));
            this.Score = (string)info.GetValue("Score", typeof(string));
     
            this.SERVER_NAME = (string)info.GetValue("SERVER_NAME", typeof(string));
            this.TaskId = (string)info.GetValue("TaskId", typeof(string));
            this.TaskName = (string)info.GetValue("TaskName", typeof(string));


        }


        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Algorithm", Algorithm);
            info.AddValue("AlgorithmType", AlgorithmType);
 
            info.AddValue("COMM_NAME", COMM_NAME);
 
            info.AddValue("DEVICE_NAME", DEVICE_NAME);
            info.AddValue("FeaturesName", FeaturesName);
            info.AddValue("FeaturesValue", FeaturesValue);
            info.AddValue("PredictedDate", PredictedDate);
            info.AddValue("Remark", Remark);
            info.AddValue("Score", Score);
     
            info.AddValue("SERVER_NAME", SERVER_NAME);
            info.AddValue("TaskId", TaskId);
            info.AddValue("TaskName", TaskName);
        }
        #endregion Model

    }

    /// <summary>
    /// 定义一个web端实时数据的缓存类，通过该类保存所有实时数据到缓存，页面上的实时数据显示则直接从缓存中读取
    /// </summary>
    public   class WebRealCache
    {
        public WebRealCache()
        {

        }


        /// <summary>
        /// 删除一个缓存
        /// </summary>
        /// <param name="item"></param>
        public   void Remove(string key)
        {
        
            HttpRuntime.Cache.Remove(key);
        }
        #region 读取实时数据缓存
        /// <summary>
        /// 将实时数据插入缓存
        /// </summary>
        /// <param name="item"></param>
        public   void InsertReal(RealWebCacheDataItem item)
        {
            try
            {
                var obj = HttpRuntime.Cache.Add(item.Key, item, null, DateTime.MaxValue, Cache.NoSlidingExpiration, CacheItemPriority.High, null) ;
                if(obj!=null)
                {
                    HttpRuntime.Cache.Remove(item.Key);
                    HttpRuntime.Cache.Insert(item.Key, item, null, DateTime.MaxValue, Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                }
               
           

            }
            catch
            {

            }
        }
        /// <summary>
        /// 批量写入数据
        /// </summary>
        /// <param name="items"></param>
        public   void InsertReal(List<RealWebCacheDataItem> items)
        {
            try
            {
                for (int i = 0; i < items.Count; i++)
                {
                    InsertReal(items[i]);
                }
            }
            catch
            {

            }

        }
        /// <summary>
        /// 获取一个缓存数据
        /// </summary>
        /// <param name="serverid"></param>
        /// <param name="commid"></param>
        /// <param name="deviceid"></param>
        /// <param name="paraid"></param>
        /// <returns></returns>
        public   RealWebCacheDataItem GetReal(WebCacheDataItem inputItem)
        {
            string key = inputItem.Key;
            var dataItem = HttpRuntime.Cache[key + "/Real"];
            if (dataItem != null && dataItem.GetType() == typeof(RealWebCacheDataItem))
            {
                return (RealWebCacheDataItem)dataItem;
            }
            return null;
        }
        /// <summary>
        /// 获取指定的设备的所有实时数据
        /// </summary>
        /// <param name="inpouItems"></param>
        /// <returns></returns>
        public   List<RealWebCacheDataItem> GetReals(List<WebCacheDataItem> inpouItems)
        {
            List<RealWebCacheDataItem> result = new List<RealWebCacheDataItem>();
            inpouItems.ForEach(delegate (WebCacheDataItem item)
            {

                string key = item.Key + "/Real";
                var dataItem = HttpRuntime.Cache.Get(key);
                if (dataItem != null && dataItem.GetType() == typeof(RealWebCacheDataItem))
                {
                    result.Add((RealWebCacheDataItem)dataItem);
                }
            });
            return result;
        }
        #endregion
        #region 读取报警数据缓存
        /// <summary>
        /// 将实时数据插入缓存
        /// </summary>
        /// <param name="item"></param>
        public   void InsertAlarm(AlarmWebCacheDataItem item)
        {
            try
            {

                var obj = HttpRuntime.Cache.Add(item.Key, item, null, DateTime.Now.AddHours(6), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
                if (obj != null)
                    HttpRuntime.Cache.Insert(item.Key, item, null, DateTime.Now.AddHours(6), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            }
            catch
            {

            }
        }
        /// <summary>
        /// 批量写入数据
        /// </summary>
        /// <param name="items"></param>
        public   void InsertAlarm(List<AlarmWebCacheDataItem> items)
        {
            try
            {
                for (int i = 0; i < items.Count; i++)
                {
                    InsertAlarm(items[i]);
                }
            }
            catch
            {

            }

        }
        /// <summary>
        /// 获取一个缓存数据
        /// </summary>
        /// <param name="serverid"></param>
        /// <param name="commid"></param>
        /// <param name="deviceid"></param>
        /// <param name="paraid"></param>
        /// <returns></returns>
        public   AlarmWebCacheDataItem GetAlarm(AlarmWebCacheDataItem inputItem,string alarmType="",string alarmLevel="")
        {
            
                string key = inputItem.Key;
            var dataItem = HttpRuntime.Cache[key + "/" + inputItem.ParaName + "/Alarm"];
 
                if (dataItem != null && dataItem.GetType() == typeof(AlarmWebCacheDataItem))
                {
                    return (AlarmWebCacheDataItem)dataItem;
                }
          
            return null;
        }
        /// <summary>
        /// 获取指定的设备的所有实时数据
        /// </summary>
        /// <param name="inpouItems"></param>
        /// <returns></returns>
        public   List<AlarmWebCacheDataItem> GetAlarms(List<AlarmWebCacheDataItem> inpouItems, string alarmType = "", string alarmLevel = "")
        {
            List<AlarmWebCacheDataItem> result = new List<AlarmWebCacheDataItem>();
            for(int i=0;i< inpouItems.Count;i++)
            {
                AlarmWebCacheDataItem item = inpouItems[i];
                string key = item.Key + "/" + item.ParaName + "/Alarm";
                var dataItem = HttpRuntime.Cache.Get(key);
              
                if (dataItem != null && dataItem.GetType() == typeof(AlarmWebCacheDataItem))
                {
                    AlarmWebCacheDataItem getItem = (AlarmWebCacheDataItem)dataItem;
                    if (!string.IsNullOrEmpty(alarmType)&& getItem.AlarmType.Trim().ToLower()!=alarmType.Trim().ToLower())
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(alarmLevel) && getItem.Alarmlevel.Trim().ToLower() != alarmLevel.Trim().ToLower())
                    {
                        continue;
                    }
           
                    result.Add((AlarmWebCacheDataItem)dataItem);
                }
            }
            
            return result;
        }
        #endregion
        #region 读取设备状态数据缓存
        /// <summary>
        /// 将实时数据插入缓存
        /// </summary>
        /// <param name="item"></param>
        public   void InsertStatus(StatusWebCacheDataItem item)
        {
            try
            {

                var obj = HttpRuntime.Cache.Add(item.Key, item, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
                if (obj != null)
                    HttpRuntime.Cache.Insert(item.Key, item, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            }
            catch
            {

            }
        }
        /// <summary>
        /// 批量写入数据
        /// </summary>
        /// <param name="items"></param>
        public   void InsertStatus(List<StatusWebCacheDataItem> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                InsertStatus(items[i]);
            }

        }
        /// <summary>
        /// 获取一个缓存数据
        /// </summary>
        /// <param name="serverid"></param>
        /// <param name="commid"></param>
        /// <param name="deviceid"></param>
        /// <param name="paraid"></param>
        /// <returns></returns>
        public   StatusWebCacheDataItem GetStatus(StatusWebCacheDataItem inputItem)
        {
            string key = inputItem.Key;
            var dataItem = HttpRuntime.Cache.Get(key + "/Status");
            if (dataItem != null && dataItem.GetType() == typeof(StatusWebCacheDataItem))
            {
                return (StatusWebCacheDataItem)dataItem;
            }
            return null;
        }
        /// <summary>
        /// 获取指定的设备的所有实时数据
        /// </summary>
        /// <param name="inpouItems"></param>
        /// <returns></returns>
        public   List<StatusWebCacheDataItem> GetStatus(List<WebCacheDataItem> inpouItems)
        {
            List<StatusWebCacheDataItem> result = new List<StatusWebCacheDataItem>();
            inpouItems.ForEach(delegate (WebCacheDataItem item)
            {

                string key = item.Key;
                var dataItem = HttpRuntime.Cache[key + "/Status"];
 
                if (dataItem != null && dataItem.GetType() == typeof(StatusWebCacheDataItem))
                {
                    result.Add((StatusWebCacheDataItem)dataItem);
                }
            });
            return result;
        }
        #endregion


        #region 读取机器训练数据
        /// <summary>
        /// 将实时数据插入缓存
        /// </summary>
        /// <param name="item"></param>
        public void InsertTrainForecast(MachineTrainForecastWebCacheDataItem item)
        {
            try
            {

                var obj = HttpRuntime.Cache.Add(item.Key, item, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
                if (obj != null)
                    HttpRuntime.Cache.Insert(item.Key, item, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            }
            catch
            {

            }
        }
        /// <summary>
        /// 批量写入数据
        /// </summary>
        /// <param name="items"></param>
        public void InsertTrainForecast(List<MachineTrainForecastWebCacheDataItem> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                InsertTrainForecast(items[i]);
            }

        }
        /// <summary>
        /// 获取一个缓存数据
        /// </summary>
        /// <param name="serverid"></param>
        /// <param name="commid"></param>
        /// <param name="deviceid"></param>
        /// <param name="paraid"></param>
        /// <returns></returns>
        public MachineTrainForecastWebCacheDataItem GetTrainForecast(MachineTrainForecastWebCacheDataItem inputItem)
        {
            string key = inputItem.Key + "/" + inputItem.TaskId;
            var dataItem = HttpRuntime.Cache.Get(key + "/MachineTrainForecast");
            if (dataItem != null && dataItem.GetType() == typeof(MachineTrainForecastWebCacheDataItem))
            {
                return (MachineTrainForecastWebCacheDataItem)dataItem;
            }
            return null;
        }
        /// <summary>
        /// 获取指定的设备的所有实时数据
        /// </summary>
        /// <param name="inpouItems"></param>
        /// <returns></returns>
        public List<MachineTrainForecastWebCacheDataItem> GetTrainForecast(List<MachineTrainForecastWebCacheDataItem> inpouItems)
        {
            List<MachineTrainForecastWebCacheDataItem> result = new List<MachineTrainForecastWebCacheDataItem>();
            inpouItems.ForEach(delegate (MachineTrainForecastWebCacheDataItem item)
            {

                string key = item.Key+ "/"+ item.TaskId;
                var dataItem = HttpRuntime.Cache[key + "/MachineTrainForecast"];

                if (dataItem != null && dataItem.GetType() == typeof(MachineTrainForecastWebCacheDataItem))
                {
                    result.Add((MachineTrainForecastWebCacheDataItem)dataItem);
                }
            });
            return result;
        }
        #endregion


    }
}
