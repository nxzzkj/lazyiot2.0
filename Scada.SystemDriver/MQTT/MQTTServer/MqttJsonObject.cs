

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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MQTTnet
{



    [Serializable]
    public class SimulatorMqttJsonPara : ISerializable,IDisposable
    {
        public SimulatorMqttJsonPara()
        {
            name = "";
            SimulatorMax = 100;
            SimulatorMin = -100;
            data = new List<string>();
        }
        public void Dispose()
        {
            data.Clear();
            data = null;
        }
        public string name { set; get; }
        public List<string> data { set; get; }

        public int SimulatorMax { set; get; }

        public int SimulatorMin { set; get; }
        public SimulatorMqttJsonPara(SerializationInfo info, StreamingContext context)
        {
            name = (string)info.GetValue("name", typeof(string));
            data = (List<string>)info.GetValue("data", typeof(List<string>));
            SimulatorMax = (int)info.GetValue("SimulatorMax", typeof(int));
            SimulatorMin = (int)info.GetValue("SimulatorMin", typeof(int));
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("name", name);
            info.AddValue("data", data);
            info.AddValue("SimulatorMax", SimulatorMax);
            info.AddValue("SimulatorMin", SimulatorMin);
        }

      
    }
    [Serializable]
    public class SimulatorMqttJsonDevice : ISerializable, IDisposable
    {
        public SimulatorMqttJsonDevice()
        {
            name = "";
            uid = "";
            ClientID = "";
            UpdateCycle = 1000;
            DataTopic = "";
            CommandTopic = "";
            UpdateCycleTopic = "";
        

        }
        public string uid { set; get; }
        public string name { set; get; }
        /// <summary>
        /// MQTT客户端ID号
        /// </summary>
        public string ClientID { set; get; }
        /// <summary>
        /// 保存的是毫秒数据
        /// </summary>
        public int UpdateCycle { set; get; }

        public string DataTopic { set; get; }
        public string CommandTopic { set; get; }
        public string UpdateCycleTopic { set; get; }



        public SimulatorMqttJsonDevice(SerializationInfo info, StreamingContext context)
        {
            name = (string)info.GetValue("name", typeof(string));
            uid = (string)info.GetValue("uid", typeof(string));
            DataTopic = (string)info.GetValue("DataTopic", typeof(string));
            CommandTopic = (string)info.GetValue("CommandTopic", typeof(string));
            UpdateCycleTopic = (string)info.GetValue("UpdateCycleTopic", typeof(string));
            ClientID = (string)info.GetValue("ClientID", typeof(string));
            UpdateCycle = (int)info.GetValue("UpdateCycle", typeof(int));
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("name", name);
            info.AddValue("uid", uid);
            info.AddValue("ClientID", ClientID);
            info.AddValue("UpdateCycle", UpdateCycle);
            info.AddValue("CommandTopic", CommandTopic);
            info.AddValue("UpdateCycleTopic", UpdateCycleTopic);
            info.AddValue("DataTopic", DataTopic);
        }

        public void Dispose()
        {
            
        }
    }

    [Serializable]
    public class SimulatorMqttJsonObject : ISerializable,IDisposable
    {
        public SimulatorMqttJsonObject()
        {
            paras = new List<SimulatorMqttJsonPara>();
            device = new SimulatorMqttJsonDevice();
        }
        public SimulatorMqttJsonDevice device { set; get; }
        public List<SimulatorMqttJsonPara> paras
        {
            set; get;
        }
        public void Dispose()
        {
            paras.Clear();
            paras = null;
            device.Dispose();
            device = null;
        }
        public SimulatorMqttJsonObject(SerializationInfo info, StreamingContext context)
        {
            paras = (List<SimulatorMqttJsonPara>)info.GetValue("paras", typeof(List<SimulatorMqttJsonPara>));
            device = (SimulatorMqttJsonDevice)info.GetValue("device", typeof(SimulatorMqttJsonDevice));


        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("paras", paras);
            info.AddValue("device", device);


        }

       

        public string UID
        {
            get { return device.uid; }
        }

        public string Name
        {
            get { return device.name; }
        }
        public int UpdateCycle
        {
            get { return device.UpdateCycle; }
        }
        public string ClientID
        {
            get { return device.ClientID; }
        }
        [NonSerialized]
        public DateTime LastTime = DateTime.Now;

    }
    [Serializable]
    public class SimulatorMqttJson : ISerializable, IDisposable
    {
        public SimulatorMqttJson()
        {
            ServerIP = "";

            Port = "";
            HeartPeried = "";
            User = "";
            Password = "";

            PassiveActive = false;

            Devices = new List<SimulatorMqttJsonObject>();
        }
        public void Dispose()
        {
            Devices.Clear();
            Devices = null;

        }
        public List<SimulatorMqttJsonObject> Devices { set; get; }
        public string ServerIP { set; get; }
        public string Port { set; get; }
        public string HeartPeried { set; get; }
        public string User { set; get; }
        public string Password { set; get; }
     
        public bool PassiveActive { set; get; }

        public SimulatorMqttJson(SerializationInfo info, StreamingContext context)
        {
            Devices = (List<SimulatorMqttJsonObject>)info.GetValue("Devices", typeof(List<SimulatorMqttJsonObject>));
            ServerIP = (string)info.GetValue("ServerIP", typeof(string));

            Port = (string)info.GetValue("Port", typeof(string));
            HeartPeried = (string)info.GetValue("HeartPeried", typeof(string));
            User = (string)info.GetValue("User", typeof(string));
            Password = (string)info.GetValue("Password", typeof(string));
        
            PassiveActive = (bool)info.GetValue("PassiveActive", typeof(bool));

        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Devices", Devices);
            info.AddValue("ServerIP", ServerIP);

            info.AddValue("Port", Port);
            info.AddValue("HeartPeried", HeartPeried);
            info.AddValue("User", User);
            info.AddValue("Password", Password);
       
            info.AddValue("PassiveActive", PassiveActive);

        }

     
    }

    ///主动请求主题的Object
    [Serializable]
    public class MQTTPassiveSubTopicObject: IDisposable
    {
        /// <summary>
        /// 设备唯一标识,一般是工控机端唯一标识
        /// </summary>
        public string uid { set; get; }
        /// <summary>
        /// 服务器端刷新数据的时间
        /// </summary>
        public int updatecycle { set; get; }
        /// <summary>
        /// 服务器端数据订阅主题,用来接收用户的数据
        /// </summary>
        public string topic { set; get; }

        public void Dispose()
        {
           
        }
    }
}
