


/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ScadaCenterServer.Core
{
    
    public class MDSReceiveLargePackObject
    {
       
        public string ServerID { set; get; }
        public ConcurrentDictionary<int ,byte[]> TemporaryByteBuffer = new ConcurrentDictionary<int, byte[]>();
   
        public int TemporaryResultBytesCount = 0;
        public string TemporaryBytesKey = "";
        private bool mTemporaryByteRun = false;
        private DateTime currentTime = DateTime.Now;
        private int timeout = 10000;//超过10秒没有接收到数据就重新获取数据
        private int retry = 3;//默认重试次数3次，超过
        public Action ReadTimeout;
        public Action ReadTimeoutEnd;
        private int readCounting = 0;
        private Task mainTask = null;
        public bool TemporaryByteRun
        {
            set
            {
                if (mTemporaryByteRun = value)
                    return;

                mTemporaryByteRun = value;
                readCounting = 0;
                currentTime = DateTime.Now;
                if (mTemporaryByteRun)
                {
                    currentTime = DateTime.Now;
                    if(mainTask!=null)
                    {
                        mainTask.Wait();
                        mainTask.Dispose();
                        mainTask = null;
                    }
                    mainTask= Task.Run(() =>
                    {
                        while (mTemporaryByteRun)
                        {
                            if (!mTemporaryByteRun)
                            {

                                break;
                            }
                            if (currentTime.AddSeconds(timeout) < DateTime.Now)//小于设定的延迟时间的时候就重新发送一条获取数据的信息
                            {
                                currentTime = DateTime.Now;
                                if (ReadTimeout != null)
                                {
                                    ReadTimeout();
                                }
                                readCounting++;
                            }
                            if (readCounting > retry)
                            {
                                Clear();
                                if (ReadTimeoutEnd != null)
                                {
                                    ReadTimeoutEnd();
                                }
                            }
                            Thread.Sleep(200);
                        }

                    });

                }
                else
                {

                    if (mainTask != null)
                    {
                        mainTask.Wait();
                        mainTask.Dispose();
                        mainTask = null;
                    }
                }

            }
            get { return mTemporaryByteRun; }

        } 
         
        public List<int> ReceivePackIndexs = new List<int>();
        /// <summary>
        /// 计数器，接收某个数据达到指定次数认为失败
        /// </summary>
        public int TemporaryCounter = 0;
        /// <summary>
        /// 获取一个要发送包的索引
        /// </summary>
        /// <returns></returns>
        public int GetSendIndex()
        {
            if (ReceivePackIndexs.Count <= 0)
                return 0;
            if (ReceivePackIndexs.Count == TemporaryCounter)
                return -1;
            for (int i = 1; i <= TemporaryCounter; i++)
            {
                if (!ReceivePackIndexs.Contains(i))
                {
                    return i;
                }
            }
            return 0;
        }
        /// <summary>
        /// 从1开始,返回指定索引的字节
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public byte[] GetIndexBytes(int index)
        {
            byte[] datas = null; 

            if (index >= 0)
            {
                TemporaryByteBuffer.TryGetValue(index, out datas);

            }
            return datas;
        }
        public bool IsReceiveEnd()
        {
            if (ReceivePackIndexs.Count <= 0&& TemporaryCounter > 0)
                return false;
            if (ReceivePackIndexs.Count == TemporaryCounter && TemporaryCounter > 0)
                return true;
            return false;
        }

        public void Clear()
        {
            try
            {
                if (ReceivePackIndexs != null)
                    ReceivePackIndexs.Clear();
                if (TemporaryByteBuffer != null)
                    TemporaryByteBuffer.Clear();


                TemporaryResultBytesCount = 0;
                TemporaryBytesKey = "";
                TemporaryByteRun = false;
                TemporaryCounter = 0;
                /// <summary>
                /// 实际数据包个数
                /// </summary>
                TemporaryCounter = 0;

                if (mainTask != null)
                {
                    mainTask.Wait();
                    mainTask.Dispose();
                    mainTask = null;
                }
                readCounting = 0;
                currentTime = DateTime.Now;
            }
            catch
            {

            }

        }
        public   int ReceivePackNumber
        {
            get {return  ReceivePackIndexs.Count; }
        }
        public bool AddReceivePack(byte[] bytes,int index)
        {
            currentTime = DateTime.Now;
            if (bytes == null)
            {
                return false;
            }
            if (bytes.Length<=0)
            {
                return false;
            }
            ///保证数据不重复，不丢失,并且顺序也一致
            if(!ReceivePackIndexs.Contains(index))
            {//保存实际数据
                if (TemporaryByteBuffer.TryAdd(index, bytes))
                {
                    ReceivePackIndexs.Add(index);
                }
            
                return true;
            }
     
            return false;
        }
    }
}
