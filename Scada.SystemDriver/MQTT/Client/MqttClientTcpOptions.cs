namespace MQTTnet.Client
{
    public class MqttClientTcpOptions : IMqttClientChannelOptions
    {
        public string Server { get; set; }

        public int? Port { get; set; }

        public int BufferSize { get; set; } = 8192;

        public MqttClientTlsOptions TlsOptions { get; set; } = new MqttClientTlsOptions();

        public override string ToString()
        {
            return Server + ":" + this.GetPort();
        }
    }
}
