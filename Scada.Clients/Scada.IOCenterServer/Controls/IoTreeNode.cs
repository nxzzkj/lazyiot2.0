using Scada.Model;
using System;
using System.Net;
using System.Windows.Forms;


 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
namespace ScadaCenterServer.Controls
{
    public class IoServerTreeNode : TreeNode, INode
    {
        public IO_SERVER Server = null;
        public string MAC = "";
        public bool status = false;
        /// <summary>
        /// 保存客户端的IP
        /// </summary>
        public EndPoint ClientEndPoint = null;
        public void InitNode()
        {
            if (Server != null)
            {
                this.Text = Server.SERVER_NAME + "[未上线]";
                this.Name = Server.SERVER_ID;
                this.ImageIndex = 1;
                this.SelectedImageIndex = 1;
                this.Tag = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        public override string ToString()
        {
            return this.Text;
        }
    }
    public class IoCommunicationTreeNode : TreeNode, INode
    {
        public bool status = false;
        public IO_COMMUNICATION Communication = null;
        public IO_SERVER Server = null;
        public void InitNode()
        {
            if (Communication != null)
            {
                this.Text = Communication.IO_COMM_LABEL + "[" + Communication.IO_COMM_NAME + "]";
                this.ToolTipText = "ID=" + Communication.IO_COMM_ID + " 备注=" + Communication.IO_COMM_REMARK;
                this.Name = Communication.IO_COMM_ID;
                this.ImageIndex = 3;
                this.SelectedImageIndex = 3;
                this.Tag = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        public override string ToString()
        {
            return Server.SERVER_IP + "[" + Server.SERVER_NAME + "]" + "//" + this.Text;
        }
    }
    public class IoDeviceTreeNode : TreeNode, INode
    {
        public bool status = false;
        public IO_DEVICE Device = null;
        public IO_COMMUNICATION Communication = null;
        public IO_SERVER Server = null;
        public void InitNode()
        {
            if (Device != null)
            {
                this.Text = Device.IO_DEVICE_LABLE + "[" + Device.IO_DEVICE_NAME + "]";
                this.ToolTipText = "ID=" + Device.IO_DEVICE_ID + " Address=" + Device.IO_DEVICE_ADDRESS;
                this.Name = Device.IO_DEVICE_ID;
                this.ImageIndex = 4;
                this.SelectedImageIndex = 4;
                this.Tag = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        public override string ToString()
        {
            return Server.SERVER_IP + "[" + Server.SERVER_NAME + "]" + "//" + Communication.IO_COMM_LABEL + "[" + Communication.IO_COMM_NAME + "]//" + this.Text;
        }
    }
}
