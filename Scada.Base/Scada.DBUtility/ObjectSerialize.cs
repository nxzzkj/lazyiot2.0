using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;


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
namespace Scada.DBUtility
{
    /// <summary>
    /// 将任何一个对象序列化和反序列化
    /// </summary>
    public class ObjectSerialize
    {
        public static byte[] ObjectToBytesBinaryFormatter(object obj)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();//定义BinaryFormatter以序列化object对象       
                MemoryStream ms = new MemoryStream();//创建内存流对象           
                formatter.Serialize(ms, obj);//把object对象序列化到内存流     
                byte[] buffer = ms.ToArray();//把内存流对象写入字节数组       
                ms.Close();//关闭内存流对象            
                ms.Dispose();//释放资源             
                MemoryStream msNew = new MemoryStream();
                GZipStream gzipStream = new GZipStream(msNew, CompressionMode.Compress, true);//创建压缩对象   
                gzipStream.Write(buffer, 0, buffer.Length);//把压缩后的数据写入文件       
                gzipStream.Close();//关闭压缩流,这里要注意：一定要关闭，要不然解压缩的时候会出现小于4K的文件读取不到数据，大于4K的文件读取不完整         
                gzipStream.Dispose();//释放对象         
                msNew.Close();
                msNew.Dispose();
                return msNew.ToArray();
            }
            catch
            {
                return new byte[0];
            }
        }

        public static object BytesToObjectBinaryFormatter(byte[] Bytes)
        {
            if (Bytes == null || Bytes.Length <= 0)
                return null;
            try
            {
                MemoryStream msNew = new MemoryStream(Bytes);
                msNew.Position = 0;
                GZipStream gzipStream = new GZipStream(msNew, CompressionMode.Decompress);//创建解压对象     
                byte[] buffer = new byte[10240];//定义数据缓冲         
                int offset = 0;//定义读取位置          
                MemoryStream ms = new MemoryStream();//定义内存流         
                while ((offset = gzipStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    ms.Write(buffer, 0, offset);//解压后的数据写入内存流          
                }
                BinaryFormatter sfFormatter = new BinaryFormatter();//定义BinaryFormatter以反序列化object对象  
                ms.Position = 0;//设置内存流的位置        
                object obj;
                try
                {
                    obj = (object)sfFormatter.Deserialize(ms);//反序列化  
                }
                catch
                {
                    throw;
                }
                finally
                {
                    ms.Close();//关闭内存流     
                    ms.Dispose();//释放资源     
                }
                gzipStream.Close();//关闭解压缩流    
                gzipStream.Dispose();//释放资源             
                msNew.Close();
                msNew.Dispose();
                return obj;
            }
            catch
            {
                return null;
            }
        }


    }
}
