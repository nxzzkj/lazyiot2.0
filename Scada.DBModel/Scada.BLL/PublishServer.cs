using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scada.DBUtility;


 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
namespace Scada.Business
{
    public class PublishServer: IDisposable
    {
        #region  ExtensionMethod
        public string tempFile = "";
        /// <summary>
        /// 备份工程
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public bool Backups(string project)
        {
            try
            {
                tempFile = Application.StartupPath + "\\temp\\" + Guid.NewGuid();
                //路径合法性判断
                //构造读取文件流对象
                using (FileStream fsRead = new FileStream(project, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) //打开文件，不能创建新的
                {
                    //构建写文件流对象
                    using (FileStream fsWrite = new FileStream(tempFile, FileMode.Create)) //没有找到就创建
                    {
                        //开辟临时缓存内存
                        byte[] byteArrayRead = new byte[1024 * 1024]; // 1字节*1024 = 1k 1k*1024 = 1M内存

                        //通过死缓存去读文本中的内容
                        while (true)
                        {
                            //readCount 这个是保存真正读取到的字节数
                            int readCount = fsRead.Read(byteArrayRead, 0, byteArrayRead.Length);

                            //开始写入读取到缓存内存中的数据到目标文本文件中
                            fsWrite.Write(byteArrayRead, 0, readCount);


                            //既然是死循环 那么什么时候我们停止读取文本内容 我们知道文本最后一行的大小肯定是小于缓存内存大小的
                            if (readCount < byteArrayRead.Length)
                            {
                                break; //结束循环
                            }
                        }

                    }
                    return true;
                }
            }
            catch
            {
                return false;
            }
        
           

       }
        public    bool ClearServers(string serverid)
        {
            try
            {
                SCADA_DRIVER DriverBll = new SCADA_DRIVER();
                ScadaMachineTrainingCondition conditionBll = new ScadaMachineTrainingCondition();
                BatchCommandTaskModel batchCommandBll = new BatchCommandTaskModel();
                ScadaMachineTrainingModel machineTrainingModelBll = new ScadaMachineTrainingModel();
                        IO_ALARM_CONFIG alarmBll = new IO_ALARM_CONFIG();
                IO_COMMUNICATION commBll = new IO_COMMUNICATION();
                IO_DEVICE deviceBll = new IO_DEVICE();
                IO_PARA paraBll = new IO_PARA();
                IO_SERVER serverBll = new IO_SERVER();
                serverBll.Clear(serverid.Trim());
                alarmBll.Clear(serverid.Trim());
                commBll.Clear(serverid.Trim());
                deviceBll.Clear(serverid.Trim());
                paraBll.Clear(serverid.Trim());
                conditionBll.Clear(serverid.Trim());
                batchCommandBll.Clear(serverid.Trim());
                machineTrainingModelBll.Clear(serverid.Trim());
                return true;
            }
            catch
            {
                return false;
            }


        }
        public   bool Recovery( string sourceFile)
        {
            bool exist = File.Exists(tempFile);
            //路径合法性判断
            if (exist)
            {
                //构造读取文件流对象
                using (FileStream fsRead = new FileStream(tempFile, FileMode.Open)) //打开文件，不能创建新的
                {
                    //构建写文件流对象
                    using (FileStream fsWrite = new FileStream(sourceFile, FileMode.Create)) //没有找到就创建
                    {
                        //开辟临时缓存内存
                        byte[] byteArrayRead = new byte[1024 * 1024]; // 1字节*1024 = 1k 1k*1024 = 1M内存

                        //通过死缓存去读文本中的内容
                        while (true)
                        {
                            //readCount 这个是保存真正读取到的字节数
                            int readCount = fsRead.Read(byteArrayRead, 0, byteArrayRead.Length);

                            //开始写入读取到缓存内存中的数据到目标文本文件中
                            fsWrite.Write(byteArrayRead, 0, readCount);


                            //既然是死循环 那么什么时候我们停止读取文本内容 我们知道文本最后一行的大小肯定是小于缓存内存大小的
                            if (readCount < byteArrayRead.Length)
                            {
                                break; //结束循环
                            }
                        }

                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 发布工程
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public bool Publish(string project)
        {

            //路径合法性判断
            try { 
                //构造读取文件流对象
                using (FileStream fsRead = new FileStream(project, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) //打开文件，不能创建新的
                {
                    //构建写文件流对象
                    using (FileStream fsWrite = new FileStream(tempFile, FileMode.Create)) //没有找到就创建
                    {
                        //开辟临时缓存内存
                        byte[] byteArrayRead = new byte[1024 * 1024]; // 1字节*1024 = 1k 1k*1024 = 1M内存

                        //通过死缓存去读文本中的内容
                        while (true)
                        {
                            //readCount 这个是保存真正读取到的字节数
                            int readCount = fsRead.Read(byteArrayRead, 0, byteArrayRead.Length);

                            //开始写入读取到缓存内存中的数据到目标文本文件中
                            fsWrite.Write(byteArrayRead, 0, readCount);


                            //既然是死循环 那么什么时候我们停止读取文本内容 我们知道文本最后一行的大小肯定是小于缓存内存大小的
                            if (readCount < byteArrayRead.Length)
                            {
                                break; //结束循环
                            }
                        }

                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
          
        }
        #endregion  ExtensionMethod
    }
}
