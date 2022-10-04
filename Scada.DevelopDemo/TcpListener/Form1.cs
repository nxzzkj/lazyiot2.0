using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MYTcpListener
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private TcpListener _listenerSocket = null;
        private void button1_Click(object sender, EventArgs e)
        {
            if(_listenerSocket==null)
            {
                _listenerSocket = new TcpListener(System.Net.IPAddress.Any, Convert.ToInt32(this.numericUpDown1.Value));
                _listenerSocket.Start();
                button1.Text = "断开监听";
                _running = true;
                DoListenAsThread();
            }
            else
            {
                _listenerSocket.Stop();
                _listenerSocket = null;
                button1.Text = "开始监听";
                _running = false;
            }
        
        }
        private bool _running = false;
        private Task DoListenAsThread()
        {
            return Task.Run(() => { 
            while (_running)
            {
                try
                {
                    //Wait and get connected socket
                    var clientSocket = _listenerSocket.AcceptSocket();
                        listBox1.Items.Add(clientSocket.RemoteEndPoint.ToString());


                }
                catch (Exception ex)
                {
                    if (!_running)
                    {
                        return;
                    }

                    
                }
            }
            });

        }

        private void button2_Click(object sender, EventArgs e)
        {
  
            TcpClient tcpClient = new TcpClient();
            tcpClient.Connect(IPAddress.Parse(this.textBox1.Text), Convert.ToInt32(this.numericUpDown2.Value));
           
            MessageBox.Show("链接服务器成功");
        }
    }
}
