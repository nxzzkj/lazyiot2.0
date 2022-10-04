

 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
 
using Scada.DBUtility;
using Scada.MDSCore;
using Scada.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOMonitor.Core
{
    /// <summary>
    /// 实时数据接收的缓存,数据中心接收到的数据全部保存到该缓存中，该缓存每200秒会自动向influxdb插入一条数据,通过这种方式可以有效提高系统的通讯效率
    /// </summary>
    public class IOMonitorCacheManager : IDisposable
    {

        
        public IOMonitorCacheManager(int Interval, int maxNumber)
        {
            ReceiveCache = new ConcurrentQueue<ReceiveCacheObject>();
            AlarmCache = new ConcurrentQueue<AlarmCacheObject>();
            EventCache = new ConcurrentQueue<EventCacheObject>();
            ScadaStatusCache = new ConcurrentQueue<Scada.Model.ScadaStatusCacheObject>();
            TimeInterval = Interval;
            MaxReadNumber = maxNumber;
        }
        /// <summary>
        /// 准备要上传的实时数据
        /// </summary>
        public Func<List<ReceiveCacheObject>, Task> WillUpload;
        /// <summary>
        /// 准备要上传的报警数据
        /// </summary>
        public Func<List<AlarmCacheObject>, Task> WillUploadAlarm;
        /// <summary>
        /// 准备要上传的事件信息
        /// </summary>

        public Func<List<EventCacheObject>, Task> WillUploadEvent;
        /// <summary>
        /// 上传设备状态信息
        /// </summary>
        public Func<List<ScadaStatusCacheObject>, Task> WillUploadScadaStatus;
        public Func<string, Task> CacheInformation;
        public int TimeInterval = 10;//默认毫秒
        public int MaxReadNumber = 3000;//每次定时读取的最大数据量
        /// <summary>
        /// 接收数据缓存,采用线程安全的堆栈
        /// </summary>
        private static volatile ConcurrentQueue<ReceiveCacheObject> ReceiveCache;
        private static volatile ConcurrentQueue<AlarmCacheObject> AlarmCache;
        private static volatile ConcurrentQueue<EventCacheObject> EventCache;
        private static volatile ConcurrentQueue<ScadaStatusCacheObject> ScadaStatusCache;
      
        
        public void Push(ScadaStatusCacheObject cacheObject)
        {

            TaskHelper.Factory.StartNew(()=>{ 
            if (ScadaStatusCache != null)
                ScadaStatusCache.Enqueue(cacheObject);
            });
        }

        public void Push(ReceiveCacheObject cacheObject)
        {
            TaskHelper.Factory.StartNew(() => {
                if (ReceiveCache != null)
                ReceiveCache.Enqueue(cacheObject);
            });
        }
        public void Push(AlarmCacheObject cacheObject)
        {
            TaskHelper.Factory.StartNew(() => {
                if (AlarmCache != null)
                AlarmCache.Enqueue(cacheObject);
            });
        }
        public void Push(EventCacheObject cacheObject)
        {
            TaskHelper.Factory.StartNew(() => {
                if (EventCache != null)
                EventCache.Enqueue(cacheObject);
            });
        }
        private Timer _timer = null;
        /// <summary>
        /// 删除过期的数据
        /// </summary>
        public List<ReceiveCacheObject> Pop()
        {
            
          
                List<ReceiveCacheObject> pops = new List<ReceiveCacheObject>();
                if (ReceiveCache != null && ReceiveCache.Count > 0)
                {
                    for (int i = 0; i < MaxReadNumber; i++)
                    {
                        ReceiveCacheObject result = null;
                        if (ReceiveCache.TryDequeue(out result) && result != null)
                        {

                            pops.Add(result);
                        }
                    }
                }
                return pops.ToList();
            

        }
        public List<AlarmCacheObject> PopAlarms()
        {
           
                List<AlarmCacheObject> pops = new List<AlarmCacheObject>();
                if (AlarmCache != null && AlarmCache.Count > 0)
                {
                    for (int i = 0; i < MaxReadNumber; i++)
                    {
                        AlarmCacheObject result = null;
                        if (AlarmCache.TryDequeue(out result) && result != null)
                        {

                            pops.Add(result);
                        }
                    }
                }
                return pops.ToList();


       


        }

        public List<EventCacheObject> PopEvents()
        {
              List<EventCacheObject> pops = new List<EventCacheObject>();
              if(EventCache != null && EventCache.Count > 0)
              {
                  for (int i = 0; i < MaxReadNumber; i++)
                  {
                      EventCacheObject result = null;
                      if (EventCache.TryDequeue(out result) && result != null)
                      {

                          pops.Add(result);
                      }
                  }

              }
             
              return pops.ToList();
        }
        public List<ScadaStatusCacheObject> PopScadaStatus()
        {
   
                List<ScadaStatusCacheObject> pops = new List<ScadaStatusCacheObject>();
                if (ScadaStatusCache != null && ScadaStatusCache.Count > 0)
                {
                    for (int i = 0; i < MaxReadNumber; i++)
                    {
                        ScadaStatusCacheObject result = null;
                        if (ScadaStatusCache.TryDequeue(out result) && result != null)
                        {

                            pops.Add(result);
                        }
                    }

                }

                return pops.ToList();

        }
        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
            if (ReceiveCache != null)
                ReceiveCache=null;
            ReceiveCache = null;
            if (AlarmCache != null)
                AlarmCache = null;
            AlarmCache = null;
            if (EventCache != null)
                EventCache = null;
            EventCache = null;
            if (ScadaStatusCache != null)
                ScadaStatusCache = null;
            ScadaStatusCache = null;


        }

        public void Read()
        {
            _timer = new Timer(   delegate
            {
             
          
                      
                    List<ReceiveCacheObject> receiveCaches = Pop();
                    if (WillUpload != null && receiveCaches.Count > 0)
                         WillUpload(receiveCaches);

                        List<AlarmCacheObject> alarmCaches = PopAlarms();

                        if (WillUploadAlarm != null && alarmCaches.Count > 0)
                            WillUploadAlarm(alarmCaches);
                        List<EventCacheObject> eventCaches = PopEvents();
                        if (WillUploadEvent != null && eventCaches.Count > 0)
                            WillUploadEvent(eventCaches);

                        List<ScadaStatusCacheObject> statusCaches = PopScadaStatus();
                        if (WillUploadScadaStatus != null && statusCaches.Count > 0)
                            WillUploadScadaStatus(statusCaches);
                   
               
           
    
                if (CacheInformation != null)
                {
                    

                }
               
             
            }, null, 10000, TimeInterval);
        }
    }
    public class ReceiveCacheObject:IDisposable
    {

        public StringBuilder DataString = new StringBuilder();

        public void Dispose()
        {
            DataString.Clear();
            DataString = null;
        }
    }
    public class EventCacheObject: IDisposable
    {

        public StringBuilder DataString = new StringBuilder();

        public void Dispose()
        {
            DataString.Clear();
            DataString = null;
        }
    }
    public class AlarmCacheObject: IDisposable
    {

        public StringBuilder DataString = new StringBuilder();
        public IO_PARAALARM Alarm = null;

        public void Dispose()
        {
            DataString.Clear();
               DataString = null;
            if (Alarm != null)
                Alarm.Dispose();
               Alarm = null;
        }
    }
   
  


}
