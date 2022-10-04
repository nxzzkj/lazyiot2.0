using System;
using System.Collections.Generic;
using Scada.DBUtility;
using Scada.MDSCore.Client;
using System.Text;

namespace ScadaWeb.Web.Controllers
{
    public partial class MDSManager
    {
        static MDSConfig MDSServerConfig = null;

        static WebRealCache mWebRealCache = null;
        public static MDSClient WebClient
        {

            get
            {
                if (System.Web.HttpContext.Current.Application["ServiceConsumer"] == null)
                {
                    System.Web.HttpContext.Current.Application["ServiceConsumer"] = new MDSClient(MDSServerConfig.WebAppPrefix, MDSServerConfig.MDSServerIP, int.Parse(MDSServerConfig.MDSServerPort));
                    if (System.Web.HttpContext.Current.Application["ServiceConsumer"] != null)
                    {
                        MDSClient ServiceConsumer = (MDSClient)System.Web.HttpContext.Current.Application["ServiceConsumer"];
                        ServiceConsumer.MessageReceived += ServiceConsumer_MessageReceived;
                        ServiceConsumer.ReConnectServerOnError = true;
                        ServiceConsumer.Connect();
                    }

                }
                return (MDSClient)System.Web.HttpContext.Current.Application["ServiceConsumer"];

            }
        }
        public static MDSConfig WebCleintConfig
        {

            get {
                if(System.Web.HttpContext.Current.Application["MDSConfig"]==null)
                {
                    System.Web.HttpContext.Current.Application["MDSConfig"] = new MDSConfig();
                }
                return (MDSConfig)System.Web.HttpContext.Current.Application["MDSConfig"];
}
        }
       
        public static void CreateWebClient()
        {
            string remooted = null;

            System.Web.HttpContext.Current.Application.Lock();
            mWebRealCache = new WebRealCache();
            if (MDSServerConfig == null)
            {
                remooted = Environment.GetEnvironmentVariable("LAZY_IP", EnvironmentVariableTarget.Machine);
                if (!string.IsNullOrEmpty(remooted))
                {
                    MDSServerConfig = new MDSConfig();
                    MDSServerConfig.MDSServerIP = remooted.Split(':')[0];
                    MDSServerConfig.MDSServerPort = remooted.Split(':')[1];
                }

            }

            if (!string.IsNullOrEmpty(remooted))
            {

                if (System.Web.HttpContext.Current.Application["MDSConfig"] == null)
                {
                    System.Web.HttpContext.Current.Application["MDSConfig"] = MDSServerConfig;
                }
                if (System.Web.HttpContext.Current.Application["ServiceConsumer"] == null)
                {
                    System.Web.HttpContext.Current.Application["ServiceConsumer"] = new MDSClient(MDSServerConfig.WebAppPrefix, MDSServerConfig.MDSServerIP, int.Parse(MDSServerConfig.MDSServerPort));
                    if (System.Web.HttpContext.Current.Application["ServiceConsumer"] != null)
                    {
                        MDSClient ServiceConsumer = (MDSClient)System.Web.HttpContext.Current.Application["ServiceConsumer"];
                        ServiceConsumer.MessageReceived += ServiceConsumer_MessageReceived;
                        ServiceConsumer.ReConnectServerOnError = true;
                        ServiceConsumer.Connect();
                    }

                }
                

            }
            System.Web.HttpContext.Current.Application.UnLock();

        }

        private static void ServiceConsumer_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            e.Message.Acknowledge();
            if (e.Message.MessageData != null && e.Message.MessageData.Length > 0)
            {
                string str = Encoding.UTF8.GetString(e.Message.MessageData);
                if (str.Split('^')[0].Length > 1)
                {
                    string key = str.Split('^')[0];
                    string objStr = str.Split('^')[1];
                    if (!string.IsNullOrEmpty(objStr))
                    {
                        switch (key.Trim().ToLower())
                        {
                            case "status":
                                {
                                    List<StatusWebCacheDataItem> models = ScadaJsonConvertor.JsonToObject<List<StatusWebCacheDataItem>>(objStr);
                                    if (models == null || models.Count <= 0)
                                        return;
                                    mWebRealCache.InsertStatus(models);
                                }
                                break;
                            case "real":
                                {
                                    List<RealWebCacheDataItem> models = ScadaJsonConvertor.JsonToObject<List<RealWebCacheDataItem>>(objStr);
                                    if (models == null || models.Count <= 0)
                                        return;
                                    mWebRealCache.InsertReal(models);
                                }
                                break;
                            case "alarm":
                                {
                                    List<AlarmWebCacheDataItem> models = ScadaJsonConvertor.JsonToObject<List<AlarmWebCacheDataItem>>(objStr);
                                    if (models == null || models.Count <= 0)
                                        return;
                                    mWebRealCache.InsertAlarm(models);
                                }
                                break;
                            case "trainforecast":
                                {
                                    List<MachineTrainForecastWebCacheDataItem> models = ScadaJsonConvertor.JsonToObject<List<MachineTrainForecastWebCacheDataItem>>(objStr);
                                    if (models == null || models.Count <= 0)
                                        return;
                                    mWebRealCache.InsertTrainForecast(models);
                                }
                                break;
                                
                        }

                    }
                }


            }

        }
    }
}