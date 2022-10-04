using Scada.DBUtility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScadaCenterServer.Core
{
    public class IOWebRealManager:IDisposable
    {
        public IOWebRealManager()
        {
          
        }
        public Func<string, Task> ExceptionOut;


        /// <summary>
        /// 向第三方推送实时数据
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public Task PostThirdPartyReal(RealWebCacheDataItem Model,string url)
        {
            if (string.IsNullOrEmpty(url))
                return null;
            if(!url.Contains("http://"))
            {
                return null;
            }
            var task = TaskHelper.Factory.StartNew(() =>
            {
                StringBuilder buffer = new StringBuilder();//这是要提交的数据
                string json = ScadaJsonConvertor.ObjectToJson(Model);

                PostWebRequest(url, json, Encoding.UTF8);

            });
            return task;
        }
        /// <summary>
        /// Post数据接口
        /// </summary>
        /// <param name="postUrl">接口地址</param>
        /// <param name="paramData">提交json数据</param>
        /// <param name="dataEncode">编码方式(Encoding.UTF8)</param>
        /// <returns></returns>
        private   string PostWebRequest(string postUrl, string paramData, Encoding dataEncode)
        {
            string responseContent = string.Empty;
            try
            {
                byte[] byteArray = dataEncode.GetBytes(paramData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/json;charset=utf-8";
                webReq.ContentLength = byteArray.Length;
                using (Stream reqStream = webReq.GetRequestStream())
                {
                    reqStream.Write(byteArray, 0, byteArray.Length);//写入参数
                                                                    //reqStream.Close();
                }
                using (HttpWebResponse response = (HttpWebResponse)webReq.GetResponse())
                {
                    //在这里对接收到的页面内容进行处理
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default))
                    {
                        responseContent = sr.ReadToEnd().ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return responseContent;
        }

        /// <summary>
        /// 向web批量上传实时数据
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public Task PostReal(List<RealWebCacheDataItem> models)
        {

            var task = TaskHelper.Factory.StartNew(() =>
            {
                StringBuilder buffer = new StringBuilder();//这是要提交的数据

                buffer.Append("real^" + ScadaJsonConvertor.ObjectToJson(models));
                byte[] bs = Encoding.UTF8.GetBytes(buffer.ToString());
                IOCenterManager.IOCenterClient.Send(bs, IOCenterManager.MDSConfig.WebAppPrefix); 
               
            });
            return task;
        }
        /// <summary>
        /// 向Web批量上传实时报警
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public Task PostAlarm(List<AlarmWebCacheDataItem> models)
        {
          
            var task = TaskHelper.Factory.StartNew(() =>
            {
                StringBuilder buffer = new StringBuilder();//这是要提交的数据

                buffer.Append("alarm^" + ScadaJsonConvertor.ObjectToJson(models));
                byte[] bs = Encoding.UTF8.GetBytes(buffer.ToString());
                IOCenterManager.IOCenterClient.Send(bs, IOCenterManager.MDSConfig.WebAppPrefix);
 
            });
            return task;
        }
        public Task PostTrainForeast(List<MachineTrainForecastWebCacheDataItem> models)
        {

            var task = TaskHelper.Factory.StartNew(() =>
            {
                StringBuilder buffer = new StringBuilder();//这是要提交的数据

                buffer.Append("trainforecast^" + ScadaJsonConvertor.ObjectToJson(models));
                byte[] bs = Encoding.UTF8.GetBytes(buffer.ToString());
                IOCenterManager.IOCenterClient.Send(bs, IOCenterManager.MDSConfig.WebAppPrefix);

            });
            return task;
        }
        /// <summary>
        /// 向Web端批量上传实时状态
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public Task PostStatus(List<StatusWebCacheDataItem> models)
        {

            var task = TaskHelper.Factory.StartNew(() =>
            {   
                   StringBuilder buffer = new StringBuilder();//这是要提交的数据

                  buffer.Append("status^"+ScadaJsonConvertor.ObjectToJson(models));
                  byte[] bs = Encoding.UTF8.GetBytes(buffer.ToString());
                  IOCenterManager.IOCenterClient.Send(bs, IOCenterManager.MDSConfig.WebAppPrefix);
                
            });
            return task;
        }
        public void Close()
        {
            Dispose();
        }

        public void Dispose()
        {
            

        }

          
    }
}
