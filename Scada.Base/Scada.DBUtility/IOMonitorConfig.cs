

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
using System.Xml;

namespace Scada.DBUtility
{
    /// <summary>
    /// 采集器端配置
    /// </summary>
    public class IOMonitorConfig: IDisposable
    {
        public string xmlname = System.Windows.Forms.Application.StartupPath + "/Config.xml";
        public IOMonitorConfig(string xml)
        {
            xmlname = xml;
            //读取xml文件
            ReadConfig(xml);
        }
        public IOMonitorConfig()
        {
            //读取xml文件
            ReadConfig(System.Windows.Forms.Application.StartupPath + "/Config.xml");
        }
        public string RemoteIP = "";
        public string Project = "";
        public string User = "admin";
        public string Password = "123456";
        //缓存定时读取的时间，毫秒 
        public int CacheInterval = 20;
        //缓存一次读取的最大数量
        public int CacheMaxNumber = 3000;
        //数据通信中阻塞时间毫秒
        public int SendBlockTime = 100;
        //数据发送超时时间，默认设置是10秒
        public int DataMessageTimeout = 10000;
        public int AutoLogin = 0;
        public int SaveLogger = 1;
        public int TaskMaxNumber = 10;
        public void ReadConfig(string filename)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            XmlNodeList list = doc.SelectNodes("/IOServer");
            foreach (XmlNode item in list)
            {
                RemoteIP = item["RemoteIP"].InnerText.Trim();
                Project = item["Project"].InnerText.Trim();
                User = item["User"].InnerText.Trim();
                Password = item["Password"].InnerText.Trim();
                if (item["CacheInterval"]!=null)
                CacheInterval = Convert.ToInt32(item["CacheInterval"].InnerText.Trim());
                if (item["SendBlockTime"] != null)
                    SendBlockTime = Convert.ToInt32(item["SendBlockTime"].InnerText.Trim());
                if (item["DataMessageTimeout"] != null)
                    DataMessageTimeout = Convert.ToInt32(item["DataMessageTimeout"].InnerText.Trim());
                
                if (item["CacheMaxNumber"]!=null)
                CacheMaxNumber = Convert.ToInt32(item["CacheMaxNumber"].InnerText.Trim());
                if (item["AutoLogin"] != null)
                    AutoLogin = Convert.ToInt32(item["AutoLogin"].InnerText.Trim());
                if (item["SaveLogger"] != null)
                    SaveLogger = Convert.ToInt32(item["SaveLogger"].InnerText.Trim());
                if (item["TaskMaxNumber"] != null)
                    TaskMaxNumber = Convert.ToInt32(item["TaskMaxNumber"].InnerText.Trim());


            }
            doc = null;
        }
        public void WriteConfig()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlname);
            XmlNodeList list = doc.SelectNodes("/IOServer");
            foreach (XmlNode item in list)
            {
                item["RemoteIP"].InnerText = RemoteIP;
                item["Project"].InnerText = Project;
                item["User"].InnerText = User;
                item["Password"].InnerText = Password;
                item["CacheInterval"].InnerText = CacheInterval.ToString();
                item["CacheMaxNumber"].InnerText = CacheMaxNumber.ToString();
                item["AutoLogin"].InnerText = AutoLogin.ToString();
                item["SaveLogger"].InnerText = SaveLogger.ToString();
                item["SendBlockTime"].InnerText = SendBlockTime.ToString();
                item["DataMessageTimeout"].InnerText = DataMessageTimeout.ToString();
                item["TaskMaxNumber"].InnerText = TaskMaxNumber.ToString();
                

            }

            doc.Save(xmlname);
           
            doc = null;
        }

        public void Dispose()
        {
            
        }
    }
}
