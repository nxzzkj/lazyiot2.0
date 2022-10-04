using MQTTnet.Protocol;
using System;

namespace MQTTnet
{
    public class MqttApplicationMessage: IDisposable
    {
        public string Topic { get; set; }

        public byte[] Payload { get; set; }

        public MqttQualityOfServiceLevel QualityOfServiceLevel { get; set; }

        public bool Retain { get; set; }

        public void Dispose()
        {
            Payload = null;
        }
    }
}
