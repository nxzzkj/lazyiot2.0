using Modbus.Data;
using Modbus.Device;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModbusClientTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Random rd = new Random();
        private  DataStore CreateDefaultDataStore(int max)
        {
       
            DataStore dataStore = new DataStore();
           
            for (int i = 1; i < 3000; i++)
            {
                bool value = i % 2 > 0;
                dataStore.CoilDiscretes.Add(value);//线圈
                dataStore.InputDiscretes.Add(!value);
                dataStore.HoldingRegisters.Add((ushort)rd.Next(max));
                dataStore.InputRegisters.Add((ushort)rd.Next(max));
            
            }

            return dataStore;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            UdpClient _simularorListener = new UdpClient(new IPEndPoint(IPAddress.Parse("192.168.1.2"), 5000));
            try
            {
                
                        ModbusSlave DeviceSlave = ModbusUdpSlave.CreateUdp(1, _simularorListener);
                DeviceSlave.ListenAsync();
                DataStore dataStore= CreateDefaultDataStore(1000);
                DeviceSlave.DataStore = dataStore;
            }
            catch (Exception emx)
            {
                 

            }
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
          
            using (UdpClient tcpClient = new UdpClient(AddressFamily.InterNetwork))
            {
                tcpClient.Connect(new IPEndPoint(IPAddress.Parse("192.168.1.2"), 5000));
                ModbusSerialMaster master = ModbusSerialMaster.CreateAscii(tcpClient);
                ushort[] result = master.ReadHoldingRegisters(1, 1, 122);
              

            }
        }
    }
}
