using Scada.FlowGraphEngine.GraphicsMap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
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
namespace ScadaFlowDesign.Core
{
    [Serializable]
    public class IOFlowViewGroup: ISerializable
    {
        public string Text { set; get; } = "";
        public string GroupID { set; get; }
        public List<string> Views { set; get; } = new List<string>();
        public int Index { set; get; } = 0;
        public IOFlowViewGroup()
        {
            Text = "未命名分组";
            GroupID = GUIDToNormalID.GuidToLongID().ToString();
            Views = new List<string>();
            Index = 0;
        }
        protected IOFlowViewGroup(SerializationInfo info, StreamingContext context)
        {
            
            this.Text = (string)info.GetValue("Text", typeof(string));
            this.GroupID = (string)info.GetValue("GroupID", typeof(string));
            this.Views = (List<string>)info.GetValue("Views", typeof(List<string>));
            this.Index = (int)info.GetValue("Index", typeof(int));
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Text", this.Text);
            info.AddValue("GroupID", this.GroupID);
            info.AddValue("Views", this.Views);
            info.AddValue("Index", this.Index);
        }

    }
    [Serializable]
    public class IOFlowProject : ISerializable
    {
        public List<IOFlowViewGroup> ViewGroups { set; get; } = new List<IOFlowViewGroup>();
        // Fields
        public List<ScadaFlowUser> FlowUsers;
        public List<ScadaConnectionBase> ScadaConnections;
        private string mTitle;
        private string mCreateDate;
        private string mProjectID;
        private List<FlowGraphAbstract> mGraphList;
        private string mPassword;
        public string FileFullName;

        public bool ExistViewInGroup(string gid)
        {
            for(int i=0;i< ViewGroups.Count;i++)
            {
                if(ViewGroups[i].Views.Exists(x=>x== gid))
                {
                    return true;
                }
            }
            return false;
        }
        public void AddViewGroup(string Title)
        {
            ViewGroups.Add(new IOFlowViewGroup() {  Text= Title });
        }
        public bool  AddViewGroup(IOFlowViewGroup group)
        {
            if(!ViewGroups.Exists(x=>x.GroupID== group.GroupID))
            {
                ViewGroups.Add(new IOFlowViewGroup() { Text = Title });
                return true;
            }
            return false;
         
        }
        public void DeleteViewGroup(IOFlowProject project,IOFlowViewGroup group)
        {
            DeleteViewGroup(project,group.GroupID);
        }
        public void DeleteViewGroup(IOFlowProject project,string groupid)
        {
            var group = ViewGroups.Find(x => x.GroupID == groupid);
  
            ViewGroups.Remove(group);
        }
        public List<IOFlowViewGroup> GetViewGroups()
        {
            ViewGroups.Sort(delegate (IOFlowViewGroup a, IOFlowViewGroup b) {
                return a.Index.CompareTo(b.Index);


            });
            return ViewGroups;
        }
        // Methods
        public IOFlowProject()
        {
            this.FlowUsers = new List<ScadaFlowUser>();
            this.ScadaConnections = new List<ScadaConnectionBase>();
            this.mTitle = "";
            this.mCreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.mProjectID = "";
            this.mGraphList = new List<FlowGraphAbstract>();
            this.mPassword = "123456";
            this.FileFullName = "";
            this.mProjectID = GUIDToNormalID.GuidToLongID().ToString();
            ScadaFlowUser item = new ScadaFlowUser
            {
                Nickname = "管理员",
                UserName = "admin",
                Password = "123456",
                Read = 1,
                Write = 1
            };
            this.FlowUsers.Add(item);
            ViewGroups = new List<IOFlowViewGroup>();
        }

        protected IOFlowProject(SerializationInfo info, StreamingContext context)
        {
            this.FlowUsers = new List<ScadaFlowUser>();
            this.ScadaConnections = new List<ScadaConnectionBase>();
            this.mTitle = "";
            this.mCreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            this.mProjectID = "";
            this.mGraphList = new List<FlowGraphAbstract>();
            this.mPassword = "123456";
            this.FileFullName = "";
            this.mProjectID = (string)info.GetValue("mProjectID", typeof(string));
            this.mTitle = (string)info.GetValue("mTitle", typeof(string));
            this.mCreateDate = (string)info.GetValue("mCreateDate", typeof(string));
            this.mGraphList = (List<FlowGraphAbstract>)info.GetValue("mGraphList", typeof(List<FlowGraphAbstract>));
            this.mPassword = (string)info.GetValue("mPassword", typeof(string));
            this.FileFullName = (string)info.GetValue("FileFullName", typeof(string));
            this.FlowUsers = (List<ScadaFlowUser>)info.GetValue("FlowUsers", typeof(List<ScadaFlowUser>));
            this.ScadaConnections = (List<ScadaConnectionBase>)info.GetValue("ScadaConnections", typeof(List<ScadaConnectionBase>));
            try
            {
                this.ViewGroups = (List<IOFlowViewGroup>)info.GetValue("ViewGroups", typeof(List<IOFlowViewGroup>));
            }
            catch
            {

            }
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("mProjectID", this.mProjectID);
            info.AddValue("mTitle", this.mTitle);
            info.AddValue("mCreateDate", this.mCreateDate);
            info.AddValue("mGraphList", this.mGraphList);
            info.AddValue("mPassword", this.mPassword);
            info.AddValue("FileFullName", this.FileFullName);
            info.AddValue("FlowUsers", this.FlowUsers);
            info.AddValue("ScadaConnections", this.ScadaConnections);
            info.AddValue("ViewGroups", this.ViewGroups);
        }

        public void LoadWork(FlowGraphAbstract site, FlowGraphControl graph)
        {
            for (int i = 0; i < this.mGraphList.Count; i++)
            {
                if (this.mGraphList[i] == site)
                {
                    graph.Abstract = site;
                }
            }
        }

        // Properties
        public string Title
        {
            get
            {
                return this.mTitle;
            }
            set
            {
                this.mTitle = value;
            }
        }

        public string CreateDate
        {
            get
            {
                return this.mTitle;
            }
            set
            {
                this.mTitle = value;
            }
        }

        public string ProjectID
        {
            get
            {
                return this.mProjectID;
            }
            set
            {
                this.mProjectID = value;
            }
        }

        public List<FlowGraphAbstract> GraphList
        {
            get
            {
                return this.mGraphList;
            }
            set
            {
                this.mGraphList = value;
            }
        }

        public string Password
        {
            get
            {
                return this.mPassword;
            }
            set
            {
                this.mPassword = value;
            }
        }
    }

}
