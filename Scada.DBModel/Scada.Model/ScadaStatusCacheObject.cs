

#region << 版 本 注 释 >>
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Scada.Model
{
    [Serializable]
    public class ScadaStatusCacheObject : IDisposable
    {
        public string AppName { set; get; }
        public ScadaStatusCacheObject()
        {
            SERVER_ID = "";
            COMM_ID = "";
            DEVICE_ID = "";
            AppName = "";
        }
        public ScadaStatusElemnt StatusElemnt = ScadaStatusElemnt.None;
        public ScadaStatus ScadaStatus = ScadaStatus.False;
        public string SERVER_ID
        { set; get; }
        public string COMM_ID
        { set; get; }
        public string DEVICE_ID
        { set; get; }
        public void Dispose()
        {
            SERVER_ID = "";
            COMM_ID = "";
            DEVICE_ID = "";
        }
        public string GetCommandString()
        {
            try
            {
                string str = "TABLE:ScadaStatusCacheObject#SERVER_ID:" + SERVER_ID;
                str += "#COMM_ID:" + COMM_ID;
                str += "#DEVICE_ID:" + DEVICE_ID;
                str += "#COMM_ID:" + COMM_ID;
                str += "#StatusElemnt:" + StatusElemnt;
                str += "#ScadaStatus:" + ScadaStatus;


                return str;
            }
            catch
            {
                return "";
            }
        }
    }
    [Serializable]
    public class ScadaMachineTrainingForecastCacheObject : IDisposable
    {
        public string AppName { set; get; }
        public ScadaMachineTrainingForecast MachineTrainingForecast { set; get; }
        public ScadaMachineTrainingForecastCacheObject()
        {
            
        }
    
     
        public void Dispose()
        {
            if(MachineTrainingForecast!=null)
            MachineTrainingForecast.Dispose();


        }
       
    }

}
