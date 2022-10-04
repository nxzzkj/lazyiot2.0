

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
using System.Windows;
using System.Xml;

namespace Scada.DBUtility
{
    public class MDSConfig: IDisposable
    {

        private string xmlname = System.Windows.Forms.Application.StartupPath + "/MDSConfig.xml";

        public MDSConfig()
        {
            try
            {
                //读取xml文件
                ReadConfig(System.Windows.Forms.Application.StartupPath + "/MDSConfig.xml");
            }
            catch
            {

            }
        }
        public MDSConfig(string path)
        {
            try
            {
                //读取xml文件
                ReadConfig(path + "/MDSConfig.xml");
            }
            catch
            {

            }
        }
        public   string MDSServerIP = "127.0.0.1";
        public   string MDSServerPort = "9993";
   

        public void ReadConfig(string filename)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filename);
                XmlNodeList list = doc.SelectNodes("/IOServer");
                if (list == null)
                {
                    return;
                }
                foreach (XmlNode item in list)
                {
             
                    MDSServerPort = item["MDSServerPort"].InnerText.Trim();
                    MDSServerIP = item["MDSServerIP"].InnerText.Trim();
                }
                doc = null;
            }
            catch (Exception emx)
            {
                throw emx;
            }
        }
        public void WriteConfig()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlname);
            XmlNodeList list = doc.SelectNodes("/IOServer");
            foreach (XmlNode item in list)
            {
              
                item["MDSServerPort"].InnerText = MDSServerPort;
                item["MDSServerIP"].InnerText = MDSServerIP;

            }

            doc.Save(xmlname);
            doc = null;
        }

        public void Dispose()
        {
         
        }

        public   string CenterServerName = "CenterServer";
        public string CenterAppName = "CenterApp";
        public   string IOStationPrefix = "Station";
        public   string ManagerAppPrefix = "ManagerApp";
        public   string MonitorAppPrefix = "MonitorApp";
        public   string FlowAppPrefix = "FlowApp";
        public string WebAppPrefix = "WebApp";
    }
}
