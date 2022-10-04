using Scada.License;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskTest
{
    public partial class Form1 : LicenseForm
    {
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {  
            Control.CheckForIllegalCrossThreadCalls = false;
            button1.Enabled = true;
            button2.Enabled = false;
        }

        ScadaTaskManager scadaTaskManager = null;
        private void button1_Click(object sender, EventArgs e)
        {
            scadaTaskManager=new ScadaTaskManager();
            scadaTaskManager.TaskCanceled = (string uid) => {
           

                //richTextBoxCancel.Invoke(new Action(() =>
                //{

                //    richTextBoxCancel.AppendText(uid);

                //}));
            };
            scadaTaskManager.TaskRanToCompletion = (string uid) =>
            {
                //richTextBoxEnd.Invoke(new Action(() =>
                //{

                //    richTextBoxEnd.AppendText(uid);

                //}));
          
            };
            scadaTaskManager.TaskRunException = (string uid) =>
            {
                //richTextBoxException.Invoke(new Action(() =>
                //{

                //    richTextBoxException.AppendText(uid);

                //}));
             
            };
            timer1.Start();
            button1.Enabled = false;
            button2.Enabled = true;


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                for (int i = 0; i < 1000; i++)
                {

                    if (i % 3 == 0)
                    {
                        var task = scadaTaskManager.Run(() =>
                        {

                            throw new Exception("异常错误任务测试");

                        });
                    }
                    else
                    {
                        var task = scadaTaskManager.Run(() =>
                        {

                            for (int s = 0; s < 10000; s++)
                            {
                                string str = "测试";
                            }


                        });
                    }
                    labelNumber.Invoke(new Action(() =>
                    {

                        labelNumber.Text = "当前已经还" + scadaTaskManager.Tasks.Count + "个正着执行的线程";

                    }));
                

                }

            });

        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            button1.Enabled = true;
            button2.Enabled = false;
        }
    }
}
