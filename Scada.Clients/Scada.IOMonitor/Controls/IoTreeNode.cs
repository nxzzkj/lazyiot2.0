using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scada.DBUtility;
using Scada.Model;


 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
namespace IOMonitor.Controls
{
    public interface INode
    {
        void InitNode();
        void ChangedStatus(bool status);
    }
    public class IoServerTreeNode : TreeNode, INode, IDisposable
    {
        public IO_SERVER Server = null;

        public void ChangedStatus(bool status)
        {
            Server.SERVER_STATUS = status ? 1 : 0;
        }

        public void Dispose()
        {
            Server = null;
        }

        public void InitNode()
        {
            if (Server != null)
            {
                this.Text = LocalIp.GetLocalIp();
                this.Name = Server.SERVER_ID;
                this.ImageIndex = 0;
                this.SelectedImageIndex = 0;
            }
        }
        public override string ToString()
        {
            return this.Text;
        }
    }
    public class IoCommunicationTreeNode : TreeNode, INode, IDisposable
    {
        public IO_COMMUNICATION Communication = null;
        public IO_SERVER Server = null;

        public void ChangedStatus(bool status)
        {
            Communication.IO_COMM_STATUS = status ? 1 : 0;
        }

        public void Dispose()
        {
            Communication = null;
            Server = null;
        }

        public void InitNode()
        {
            if (Communication != null)
            {
                this.Text = Communication.IO_COMM_LABEL + "[" + Communication.IO_COMM_NAME + "]";
                this.ToolTipText = "ID=" + Communication.IO_COMM_ID + " 备注=" + Communication.IO_COMM_REMARK;
                this.Name = Communication.IO_COMM_ID;
                this.ImageIndex = 1;
                this.SelectedImageIndex = 1;
            }
        }
        public override string ToString()
        {
            return Server.SERVER_IP + "[" + Server.SERVER_NAME + "]" + "//" + this.Text;
        }
    }
    public class IoDeviceTreeNode : TreeNode, INode, IDisposable
    {
        public IO_DEVICE Device = null;
        public IO_COMMUNICATION Communication = null;
        public IO_SERVER Server = null;
        public void Dispose()
        {
            Device = null;
            Communication = null;
            Server = null;
        }
        public void InitNode()
        {
            if (Device != null)
            {
                this.Text = Device.IO_DEVICE_LABLE + "[" + Device.IO_DEVICE_NAME + "]";
                this.ToolTipText = "ID=" + Device.IO_DEVICE_ID + " Address=" + Device.IO_DEVICE_ADDRESS;
                this.Name = Device.IO_DEVICE_ID;
                //默认显示是的非连接信号
                this.ImageIndex = 3;
                this.SelectedImageIndex = 3;
            }
        }
        public override string ToString()
        {
            return Server.SERVER_IP + "[" + Server.SERVER_NAME + "]" + "//" + Communication.IO_COMM_LABEL + "[" + Communication.IO_COMM_NAME + "]//" + this.Text;
        }

        public void ChangedStatus(bool status)
        {
            if (status)
            {
                this.ImageIndex = 2;
                this.SelectedImageIndex = 2;

            }
            else
            {
                this.ImageIndex = 3;
                this.SelectedImageIndex = 3;
            }
        }
        public void ChangedStatus()
        {
            if (this.Device.IO_DEVICE_STATUS==1)
            {
                this.ImageIndex = 2;
                this.SelectedImageIndex = 2;

            }
            else
            {
                this.ImageIndex = 3;
                this.SelectedImageIndex = 3;
            }
        }
        public void ChangedStatus(int status)
        {
            if (status == 1)
            {
                this.ImageIndex = 2;
                this.SelectedImageIndex = 2;

            }
            else if (status == 0)
            {
                this.ImageIndex = 3;
                this.SelectedImageIndex = 3;
            }
            //Device.IO_DEVICE_STATUS = status;
        }


    }
}
