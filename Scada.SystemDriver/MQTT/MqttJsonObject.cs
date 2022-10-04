using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTTnet
{
    [Serializable]
    public class MqttJsonDevice:IDisposable
    {
        public string uid { set; get; }
        public string soft_version{set;get;}
        public string hard_version { set; get; }
        public string run_time { set; get; }
        public string status { set; get; }

        public void Dispose()
        {
            uid = "";
            soft_version = "";
            hard_version = "";
            run_time = "";
            status = "";
        }
    }

    [Serializable]
    public class MqttJsonPara : IDisposable
    {
        public string name { set; get; }
        public string datatype { set; get; }
        public string iotype { set; get; }
        public string version { set; get; }
        public List<Object> data { set; get; }
        public void Dispose()
        {
            if (data != null)
                data.Clear();
            data = null;
            name = "";
            datatype = "";
            iotype = "";
            version = "";
            if (data != null)
                data.Clear();
            data = null;
        }
    }


    [Serializable]
    public class MqttJsonObject : IDisposable
    {
        public MqttJsonDevice device { set; get; }
        public List<MqttJsonPara> paras
        {
            set; get;
        }
        public void Dispose()
        {
            if (device != null)
                device.Dispose();
            device = null;
            if (paras != null)
                paras.Clear();
            paras = null;
        }
    }
    ///定义西安亚泰的数据解析类
    [Serializable]
    public class CommonMqttJsonPara: IDisposable
    {
        public string name { set; get; }
        public List<Object> data { set; get; }
        public void Dispose()
        {
            name = "";
            if (data != null)
                data.Clear();
            data = null;
        }
        }
    [Serializable]
    public class CommonMqttJsonDevice: IDisposable
    {
        public string uid { set; get; }
        public void Dispose()
        {
            uid = "";
        }
        }

    [Serializable]
    public class CommonMqttJsonObject: IDisposable
    {
        public CommonMqttJsonDevice device { set; get; }
        public List<CommonMqttJsonPara> paras
        {
            set; get;
        }
        public void Dispose()
        {
            if (device != null)
                device.Dispose();
            device = null;
            if (paras != null)
                paras.Clear();
            paras = null;
        }
        }

}

