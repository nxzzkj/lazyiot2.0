

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scada.DBUtility;
using Scada.Model;
using Scada.MDSCore;
using Scada.TriggerAlarm;


 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
namespace IOMonitor.Core
{
    /// <summary>
    /// 实际数据插入接口,目前采用TCP/IP的方式进行数据向服务器传输
    /// </summary>
    public class IOMonitorRealDataUploadDBUtility
    {
        /// <summary>
        /// 获取要上传的实时数据字符串组合
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
         public  static StringBuilder GetRealDataCacheString(IO_DEVICE device)
        {
          
   
            List<IO_PARA> paras = new List<IO_PARA>();
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("IO_COMM_ID:" + device.IO_COMM_ID + "#IO_DEVICE_ID:" + device.IO_DEVICE_ID + "#IO_SERVER_ID:" + device.IO_SERVER_ID + "#IO_DEVICE_NAME:" + device.IO_DEVICE_NAME + "#DATE:" + (device.GetedValueDate == null ? "" : UnixDateTimeConvert.ConvertDateTimeInt(device.GetedValueDate.Value).ToString()) + "");
            for (int i = 0; i < device.IOParas.Count; i++)
            {
                if (device.IOParas[i].IORealData == null)
                    continue;
                string QualityStampStr = "BAD";
                if (device.IOParas[i].RealQualityStamp == Scada.IOStructure.QualityStamp.NONE)
                {
                    device.IOParas[i].IORealData.QualityStamp = Scada.IOStructure.QualityStamp.NONE;
                    QualityStampStr = "NONE";
                }
                else if (device.IOParas[i].RealQualityStamp == Scada.IOStructure.QualityStamp.GOOD)
                {
                    device.IOParas[i].IORealData.QualityStamp = Scada.IOStructure.QualityStamp.GOOD;
                    QualityStampStr = "GOOD";
                }
                else
                {
                    device.IOParas[i].IORealData.QualityStamp = Scada.IOStructure.QualityStamp.BAD;
                    QualityStampStr = "BAD";
                }
                if (device.IOParas[i].RealValue == "")
                {
                    device.IOParas[i].IORealData.ParaValue = "-9999";
                    device.IOParas[i].IORealData.QualityStamp = Scada.IOStructure.QualityStamp.BAD;
                    QualityStampStr = "BAD";
                }

                stringBuilder.Append( "#" + device.IOParas[i].IO_NAME + ":" + device.IOParas[i].IO_ID + "|" + device.IOParas[i].RealValue + "|" + QualityStampStr);
            }
            return stringBuilder;
        }

        /// <summary>
        /// 上传实时数据
        /// </summary>
        /// <param name="server"></param>
        /// <param name="comm"></param>
        /// <param name="device"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public static bool UploadReal(List<ReceiveCacheObject> receiveCaches)
        {

            bool result = false;
            string sendString = "";
            receiveCaches.ForEach(delegate (ReceiveCacheObject p) {

                sendString += p.DataString+"^";
            });
          
            try
            {
                TcpData tcpData = new TcpData();
                byte[] simuBytes = tcpData.StringToTcpByte(sendString);
                result = IOMonitorManager.MDSClient.Send(simuBytes, MDSCommandType.实时值);
       
                IOMonitorUIManager.AppendLogItem("发送了 "+ simuBytes.Length+"字节实时数据");
                simuBytes = null;
                return result;
            }
            catch (Exception ex)
            {

                result = false;
                //写入错误日志，并将错误日志返回的日志窗体
                IOMonitorUIManager.DisplyException(ex);
            }
            for (int i = 0; i < receiveCaches.Count; i++)
            {
                receiveCaches[i].Dispose();
                receiveCaches[i] = null;
            }
            sendString = "";
            receiveCaches.Clear();
            receiveCaches = null;
            return result;

        }

        public static StringBuilder GetAlarmCacheString(IO_DEVICE device, out List<IO_PARAALARM> alarms)
        {
            alarms = new List<IO_PARAALARM>();

            IODeviceParaTriggerAlarm paraMaker = new IODeviceParaTriggerAlarm();
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < device.IOParas.Count; i++)
            {
                try
                {

                    IO_PARAALARM alarm = paraMaker.MakeAlarm(device.IOParas, device.IOParas[i], device.IOParas[i].IORealData, device.IO_DEVICE_LABLE);
                    if (alarm != null&&!string.IsNullOrEmpty(alarm.IO_ALARM_DATE))
                    {
                        stringBuilder.Append( "^" + alarm.GetCommandString());
                        alarms.Add(alarm);
                    }
                }
                catch  
                {
                    continue;
                }

            }
            return stringBuilder;
        }

        /// <summary>
        /// 从采集站端上传报警值
        /// </summary>
        /// <param name="server"></param>
        /// <param name="comm"></param>
        /// <param name="device"></param>
        /// <returns></returns>
        public static bool UploadAlarm(List<AlarmCacheObject> receiveCaches)
        {


            bool result = false;
            StringBuilder sendString = new StringBuilder();
            receiveCaches.ForEach(delegate (AlarmCacheObject p) {

                sendString.Append( p.DataString + "^");
            });

            try
            {
                TcpData tcpData = new TcpData();
                byte[] simuBytes = tcpData.StringToTcpByte(sendString);
                result = IOMonitorManager.MDSClient.Send(simuBytes, MDSCommandType.报警值);
                IOMonitorUIManager.AppendLogItem("发送了 " + simuBytes.Length + "字节报警数据");
                simuBytes = null;

                return result;
            }
            catch (Exception ex)
            {

                result = false;
                //写入错误日志，并将错误日志返回的日志窗体
                IOMonitorUIManager.DisplyException(ex);
            }
            for (int i = 0; i < receiveCaches.Count; i++)
            {
                receiveCaches[i].Dispose();
                receiveCaches[i] = null;
            }
            receiveCaches.Clear();
            receiveCaches = null;
            sendString.Clear();
            sendString = null;
            return result;

        }

        /// <summary>
        /// 上传采集站各种事件
        /// </summary>
        /// <param name="scadaEvent"></param>
        /// <param name="eventModel"></param>
        public static StringBuilder GetScadaEventCacheStriung(ScadaEvent scadaEvent, ScadaEventModel eventModel)
        {

            eventModel.Id = GUIDToNormalID.GuidToLongID().ToString();
            eventModel.Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            eventModel.Event = scadaEvent.ToString();
            return new StringBuilder().Append(eventModel.GetCommandString());

 
        }

        public static bool UploadEvent(List<EventCacheObject> receiveCaches)
        {


            bool result = false;
            StringBuilder sendString = new StringBuilder();
            receiveCaches.ForEach(delegate (EventCacheObject p) {

                sendString.Append(p.DataString + "^");
            });

            try
            {
                TcpData tcpData = new TcpData();
                byte[] simuBytes = tcpData.StringToTcpByte(sendString);
                result = IOMonitorManager.MDSClient.Send(simuBytes, MDSCommandType.系统事件);
                IOMonitorUIManager.AppendLogItem("发送了 " + simuBytes.Length + "字节事件数据");
                simuBytes = null;
                return result;
            }
            catch (Exception ex)
            {

                result = false;
                //写入错误日志，并将错误日志返回的日志窗体
                IOMonitorUIManager.DisplyException(ex);
            }
            for(int i=0;i< receiveCaches.Count;i++)
            {
                receiveCaches[i].Dispose();
                receiveCaches[i] = null;
            }
            receiveCaches.Clear();
            receiveCaches = null;
            sendString.Clear() ;
            sendString = null;
            return result;

        }

        public static bool UploadScadaStatus(List<ScadaStatusCacheObject> receiveCaches)
        {


            
            bool result = false;
            StringBuilder sendString = new StringBuilder();
            receiveCaches.ForEach(delegate (ScadaStatusCacheObject p) {

                sendString.Append(p.GetCommandString() + "^");
            });

            try
            {
                TcpData tcpData = new TcpData();
                byte[] simuBytes = tcpData.StringToTcpByte(sendString);
                result = IOMonitorManager.MDSClient.Send(simuBytes, MDSCommandType.通道设备状态);
                IOMonitorUIManager.AppendLogItem("发送了 " + simuBytes.Length + "字节设备状态数据");
                simuBytes = null;
                return result;
            }
            catch (Exception ex)
            {

                result = false;
                //写入错误日志，并将错误日志返回的日志窗体
                IOMonitorUIManager.DisplyException(ex);
            }
            for (int i = 0; i < receiveCaches.Count; i++)
            {
                receiveCaches[i].Dispose();
                receiveCaches[i] = null;
            }
            receiveCaches.Clear();
            receiveCaches = null;
            sendString.Clear();
            sendString = null;
            return result;

        }
    }


}
    
