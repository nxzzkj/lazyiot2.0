


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
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace ScadaCenterServer.Core
{

    
    /// <summary>
    /// 实时数据接收的缓存,数据中心接收到的数据全部保存到该缓存中，该缓存每200秒会自动向influxdb插入一条数据
    /// </summary>
    public class IOCenterCacheManager : IDisposable
    {
        public Func<List<ReceiveCacheObject>,bool> InsertInfluxdb;
        public Func<List<AlarmCacheObject>, bool> InsertAlarmInfluxdb;
        public Func<List<EventCacheObject>, bool> InsertEventInfluxdb;
        public Func<List<ScadaStatusCacheObject>, bool> InsertStatusInfluxdb;
        public Func<List<ScadaMachineTrainingForecastCacheObject>, bool> InsertForecastInfluxdb;
        public Func<List<MachineTrainCacheObject>, bool> InsertMachineTrainInfluxdb;
        public Func<List<BatchCommandCacheObject>, bool> InsertBatchCommandInfluxdb;



        public int TimeInterval = 20;//默认毫秒
        public int MaxInsertNumber = 999;
    
        /// <summary>
        /// 接收数据缓存,采用线程安全的堆栈
        /// </summary>
        private volatile ConcurrentQueue<ReceiveCacheObject> ReceiveCache;
        private volatile ConcurrentQueue<AlarmCacheObject> AlarmCache;
        private volatile ConcurrentQueue<EventCacheObject> EventCache;
        private volatile ConcurrentQueue<ScadaStatusCacheObject> StatusCache;
        private volatile ConcurrentQueue<ScadaMachineTrainingForecastCacheObject> ForecastCache;

        private volatile ConcurrentQueue<MachineTrainCacheObject> MachineTrainCache;//机器训练执行日志
        private volatile ConcurrentQueue<BatchCommandCacheObject> BatchCommandCache;//命令训练执行日志
      
        public Func<string, Task> CacheInformation;
     
        public void Push(MachineTrainCacheObject cacheObject)
        {

            if (IOCenterManager.PublishRestart)
            {

                return;
            }
            
                if (MachineTrainCache != null)
                    MachineTrainCache.Enqueue(cacheObject);
          


        }
        public void Push(BatchCommandCacheObject cacheObject)
        {

            if (IOCenterManager.PublishRestart)
            {

                return;
            }
          
                if (BatchCommandCache != null)
                    BatchCommandCache.Enqueue(cacheObject);
       


        }
        public void Push(ScadaStatusCacheObject cacheObject)
        {
             
                if (IOCenterManager.PublishRestart)
                {

                    return;
                }
             
                    if (StatusCache != null)
                        StatusCache.Enqueue(cacheObject);
          
            
           
        }
        public void Push(ReceiveCacheObject cacheObject)
        {
            if (IOCenterManager.PublishRestart)
            {

                return;
            }

         
            
                if (ReceiveCache != null)
                ReceiveCache.Enqueue(cacheObject);
           
        }
        public void Push(AlarmCacheObject cacheObject)
        {
            if (IOCenterManager.PublishRestart)
            {

                return;
            }
           
                if (AlarmCache != null)
                AlarmCache.Enqueue(cacheObject);
           
        }
        public void Push(EventCacheObject cacheObject)
        {
            if (IOCenterManager.PublishRestart)
            {

                return;
            }
           
                if (EventCache != null)
                EventCache.Enqueue(cacheObject);
          
        }
        public void Push(ScadaMachineTrainingForecastCacheObject cacheObject)
        {
            if (IOCenterManager.PublishRestart)
            {

                return;
            }
            
                if (ForecastCache != null)
                    ForecastCache.Enqueue(cacheObject);
         
        }

        private Timer _timer = null;
 
        /// <summary>
        /// 删除过期的数据
        /// </summary>
        private List<ReceiveCacheObject> PopRealData()
        {
           
                if (IOCenterManager.PublishRestart)
                {
                    return new List<ReceiveCacheObject>();
                }

                List<ReceiveCacheObject> pops = new List<ReceiveCacheObject>();
                if (ReceiveCache != null && ReceiveCache.Count > 0)
                {
                    for (int i = 0; i < MaxInsertNumber; i++)
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
        private List<AlarmCacheObject> PopAlarm()
        {
            
                if (IOCenterManager.PublishRestart)
                {
                    return new List<AlarmCacheObject>();
                }

              

                List<AlarmCacheObject> pops = new List<AlarmCacheObject>();
                if (AlarmCache != null && AlarmCache.Count > 0)
                {
                    for (int i = 0; i < MaxInsertNumber; i++)
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

        private List<ScadaStatusCacheObject> PopStatus()
        {
           
                if (IOCenterManager.PublishRestart)
                {
                    Thread.Sleep(5);//先释放资源
                    return new List<ScadaStatusCacheObject>();
                }

          

                List<ScadaStatusCacheObject> pops = new List<ScadaStatusCacheObject>();
                if (StatusCache != null && StatusCache.Count > 0)
                {
                    for (int i = 0; i < MaxInsertNumber; i++)
                    {
                        ScadaStatusCacheObject result = null;
                        if (StatusCache.TryDequeue(out result) && result != null)
                        {

                            pops.Add(result);
                        }
                    }
                }
                return pops.ToList();

          

        }
        private List<EventCacheObject> PopEvent()
        {
            
                if (IOCenterManager.PublishRestart)
                {
                    Thread.Sleep(5);//先释放资源
                    return new List<EventCacheObject>();
                }
        
                List<EventCacheObject> pops = new List<EventCacheObject>();
                if (EventCache != null && EventCache.Count > 0)
                {
                    for (int i = 0; i < MaxInsertNumber; i++)
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
        private List<ScadaMachineTrainingForecastCacheObject> PopForecast()
        {
            
                if (IOCenterManager.PublishRestart)
                {
                    Thread.Sleep(5);//先释放资源
                    return new List<ScadaMachineTrainingForecastCacheObject>();
                }
   

                List<ScadaMachineTrainingForecastCacheObject> pops = new List<ScadaMachineTrainingForecastCacheObject>();
                if (ForecastCache != null && ForecastCache.Count > 0)
                {
                    for (int i = 0; i < MaxInsertNumber; i++)
                    {
                        ScadaMachineTrainingForecastCacheObject result = null;
                        if (ForecastCache.TryDequeue(out result) && result != null)
                        {

                            pops.Add(result);
                        }
                    }
                }
                return pops.ToList();

 
        }
        private List<MachineTrainCacheObject> PopMachineTrain()
        {
           
                if (IOCenterManager.PublishRestart)
                {
                    return new List<MachineTrainCacheObject>();
                }
                List<MachineTrainCacheObject> pops = new List<MachineTrainCacheObject>();
                if (MachineTrainCache != null && MachineTrainCache.Count > 0)
                {
                    for (int i = 0; i < MaxInsertNumber; i++)
                    {
                        MachineTrainCacheObject result = null;
                        if (MachineTrainCache.TryDequeue(out result) && result != null)
                        {

                            pops.Add(result);
                        }
                    }
                }
                return pops.ToList();
 

        }
        private List<BatchCommandCacheObject> PopBatchCommand()
        {
           
                if (IOCenterManager.PublishRestart)
                {
                    return new List<BatchCommandCacheObject>();
                }

         

                List<BatchCommandCacheObject> pops = new List<BatchCommandCacheObject>();
                if (BatchCommandCache != null && BatchCommandCache.Count > 0)
                {
                    for (int i = 0; i < MaxInsertNumber; i++)
                    {
                        BatchCommandCacheObject result = null;
                        if (BatchCommandCache.TryDequeue(out result) && result != null)
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
                ReceiveCache = null;
            ReceiveCache = null;
            if (AlarmCache != null)
                AlarmCache = null;
            AlarmCache = null;
            if (EventCache != null)
                EventCache = null;
            EventCache = null;
            if (StatusCache != null)
                StatusCache = null;
            StatusCache = null;

            if (ForecastCache != null)
                ForecastCache = null;
            ForecastCache = null;

            if (MachineTrainCache != null)
                MachineTrainCache = null;
            MachineTrainCache = null;

            if (BatchCommandCache != null)
                BatchCommandCache = null;
            BatchCommandCache = null;

            if (WebRealManager != null)
                WebRealManager.Close();
            WebRealManager = null;




        }
        private IOWebRealManager WebRealManager = null;
        public IOCenterCacheManager()
        {
            ReceiveCache = new ConcurrentQueue<ReceiveCacheObject>();
            AlarmCache = new ConcurrentQueue<AlarmCacheObject>();
            EventCache = new ConcurrentQueue<EventCacheObject>();
            StatusCache = new ConcurrentQueue<ScadaStatusCacheObject>();
            ForecastCache = new ConcurrentQueue<ScadaMachineTrainingForecastCacheObject>();
            MachineTrainCache = new ConcurrentQueue<MachineTrainCacheObject>();
            BatchCommandCache = new ConcurrentQueue<BatchCommandCacheObject>();
            WebRealManager = new IOWebRealManager();
            WebRealManager.ExceptionOut = (string msg) => {

               return  TaskHelper.Factory.StartNew(() => {

                   if (CacheInformation != null)
                   {
                       CacheInformation(msg);
                   }

               });
            };


        }
        public void Read()
        {
            _timer = new Timer(delegate
          {
              if (IOCenterManager.PublishRestart)
              {
               
                    return;
              }


              TaskHelper.Factory.StartNew(() =>
                  {
                      try
                      {
                          List<ReceiveCacheObject> receiveCaches = PopRealData();
                          List<RealWebCacheDataItem> webModels = new List<RealWebCacheDataItem>();
                          for (int i = 0; i < receiveCaches.Count; i++)
                          {

                              RealWebCacheDataItem realCacheData = new RealWebCacheDataItem()
                              {
                                  CommunicationId = receiveCaches[i].device.IO_COMM_ID,
                                  DeviceId = receiveCaches[i].device.IO_DEVICE_ID,
                                  ServerId = receiveCaches[i].device.IO_SERVER_ID

                              };
                              realCacheData.ParaReals = new List<WebCacheIOData>();
                              realCacheData.Date = receiveCaches[i].RealDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                              receiveCaches[i].device.IOParas.ForEach(delegate (IO_PARA para)
                              {
                                  WebCacheIOData iOData = new WebCacheIOData()
                                  {
                                      Date = para.RealDate,
                                      ParaID = para.IO_ID,
                                      ParaName = para.IO_NAME,
                                      ParaValue = para.RealValue,
                                      QualityStamp = para.RealQualityStamp.ToString()

                                  };
                                  realCacheData.ParaReals.Add(iOData);
                                 
                              });
                              WebRealManager.PostThirdPartyReal(realCacheData, receiveCaches[i].device.DataPushUrl);
                              webModels.Add(realCacheData);
                          }
                          if (webModels.Count > 0)
                          {
                              WebRealManager.PostReal(webModels);
                          }
                         
                          if (InsertInfluxdb != null && receiveCaches.Count > 0)
                          {
                              InsertInfluxdb(receiveCaches);
                        
                          }
                      }
                      catch
                      {

                      }
                  });

              TaskHelper.Factory.StartNew(() =>
                  {


                      try
                      {
                          List<AlarmCacheObject> alarmCaches = PopAlarm();
                          List<AlarmWebCacheDataItem> webModels = new List<AlarmWebCacheDataItem>();
                          for (int i = 0; i < alarmCaches.Count; i++)
                          {
                              if (alarmCaches[i].Alarm == null)
                                  continue;
                              AlarmWebCacheDataItem alarmCacheData = new AlarmWebCacheDataItem()
                              {

                                  Alarmlevel = alarmCaches[i].Alarm.IO_ALARM_LEVEL,
                                  AlarmTime = alarmCaches[i].Alarm.IO_ALARM_DATE,
                                  AlarmType = alarmCaches[i].Alarm.IO_ALARM_TYPE,
                                  AlarmValue = alarmCaches[i].Alarm.IO_ALARM_VALUE,
                                  CommunicationId = alarmCaches[i].communication.IO_COMM_ID,
                                  DeviceId = alarmCaches[i].device.IO_DEVICE_ID,
                                  DeviceName = alarmCaches[i].device.IO_DEVICE_NAME,
                                  ParaLabel = alarmCaches[i].Alarm.IO_LABEL,
                                  ParaName = alarmCaches[i].Alarm.IO_NAME,
                                  ServerId = alarmCaches[i].server.SERVER_ID,
                                  ParaId = alarmCaches[i].Alarm.IO_ID

                              };
                              webModels.Add(alarmCacheData);
                          }
                          if (webModels.Count > 0)
                          {
                              WebRealManager.PostAlarm(webModels);
                          }

                          if (InsertAlarmInfluxdb != null && alarmCaches.Count > 0)
                          {
                              InsertAlarmInfluxdb(alarmCaches);

                          }

                      }
                      catch
                      {

                      }
                  });

              TaskHelper.Factory.StartNew(() =>
                  {
                      try
                      {
                          List<EventCacheObject> eventCaches = PopEvent();
                          if (InsertEventInfluxdb != null && eventCaches.Count > 0)
                          {
                              InsertEventInfluxdb(eventCaches);
                        
                          }
                      }
                      catch
                      {

                      }
                  });

              TaskHelper.Factory.StartNew(() =>
                  {
                      try
                      {
                          List<ScadaStatusCacheObject> statusCaches = PopStatus();
                          List<StatusWebCacheDataItem> webModels = new List<StatusWebCacheDataItem>();
                          for (int i = 0; i < statusCaches.Count; i++)
                          {
                              if (statusCaches[i].StatusElemnt.ToString() != "Device")
                                  continue;
                              StatusWebCacheDataItem statusCacheData = new StatusWebCacheDataItem()
                              {

                                  DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                  Status = statusCaches[i].ScadaStatus.ToString(),
                                  CommunicationId = statusCaches[i].COMM_ID,
                                  DeviceId = statusCaches[i].DEVICE_ID,
                                  ServerId = statusCaches[i].SERVER_ID

                              };
                              webModels.Add(statusCacheData);
                          }
                          if (webModels.Count > 0)
                          {
                              WebRealManager.PostStatus(webModels);
                          }
                          if (InsertStatusInfluxdb != null && statusCaches.Count > 0)
                          {
                              InsertStatusInfluxdb(statusCaches);
                      
                          }

                      }
                      catch
                      {

                      }
                  });
              TaskHelper.Factory.StartNew(() =>
                  {
                      try
                      {

                         


                          List<ScadaMachineTrainingForecastCacheObject> foreacastCaches = PopForecast();
                          List<MachineTrainForecastWebCacheDataItem> webModels = new List<MachineTrainForecastWebCacheDataItem>();
                          for (int i = 0; i < foreacastCaches.Count; i++)
                          {

                              MachineTrainForecastWebCacheDataItem realCacheData = new MachineTrainForecastWebCacheDataItem()
                              {
                                  CommunicationId = foreacastCaches[i].MachineTrainingForecast.COMM_ID,
                                  DeviceId = foreacastCaches[i].MachineTrainingForecast.DEVICE_ID,
                                  ServerId = foreacastCaches[i].MachineTrainingForecast.SERVER_ID,
                                  Algorithm = foreacastCaches[i].MachineTrainingForecast.Algorithm,
                                  AlgorithmType = foreacastCaches[i].MachineTrainingForecast.AlgorithmType,
                                  COMM_NAME = foreacastCaches[i].MachineTrainingForecast.COMM_NAME,
                                  DEVICE_NAME = foreacastCaches[i].MachineTrainingForecast.DEVICE_NAME,
                                  FeaturesName = foreacastCaches[i].MachineTrainingForecast.FeaturesName,
                                  FeaturesValue = foreacastCaches[i].MachineTrainingForecast.FeaturesValue,
                                  PredictedDate = foreacastCaches[i].MachineTrainingForecast.PredictedDate,
                                  PredictedLabel = foreacastCaches[i].MachineTrainingForecast.PredictedLabel,
                                  Remark = foreacastCaches[i].MachineTrainingForecast.Remark,
                                  Score = foreacastCaches[i].MachineTrainingForecast.Score,
                                  SERVER_NAME = foreacastCaches[i].MachineTrainingForecast.SERVER_NAME,
                                  TaskId = foreacastCaches[i].MachineTrainingForecast.TaskId.ToString(),
                                  TaskName = foreacastCaches[i].MachineTrainingForecast.TaskName


                              };
                              
                   
                              webModels.Add(realCacheData);
                          }
                          if (webModels.Count > 0)
                          {
                              WebRealManager.PostTrainForeast(webModels);
                          }
                          if (InsertForecastInfluxdb != null && foreacastCaches.Count > 0)
                          {
                              InsertForecastInfluxdb(foreacastCaches);

                          }

                      }
                      catch
                      {

                      }
                  });
              TaskHelper.Factory.StartNew(() =>
                  {
                      try
                      {
                          List<MachineTrainCacheObject> machineTrainCaches = PopMachineTrain();
                          if (InsertMachineTrainInfluxdb != null && machineTrainCaches.Count > 0)
                          {
                              InsertMachineTrainInfluxdb(machineTrainCaches);

                          }

                      }
                      catch
                      {

                      }
                  });

              TaskHelper.Factory.StartNew(() =>
                  {
                      try
                      {
                          List<BatchCommandCacheObject> batchCommandCaches = PopBatchCommand();
                          if (InsertBatchCommandInfluxdb != null && batchCommandCaches.Count > 0)
                          {
                              InsertBatchCommandInfluxdb(batchCommandCaches);

                          }

                      }
                      catch
                      {

                      }
                  });


                  if (CacheInformation != null)
                  {
                      try
                      {
                          //CacheInformation("实时数据缓存数量 " + ReceiveCache.Count + ",报警缓存数 " + AlarmCache.Count + ",事件缓存 " + EventCache.Count);
                      }
                      catch
                      {

                      }
                  }
           
              

          }, null, 10000, TimeInterval);
        }
    }
   
  
}
