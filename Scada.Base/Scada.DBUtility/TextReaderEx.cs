

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
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.DBUtility
{
    /// <summary>
    /// 定义一个读取和写入保存训练数据的类
    /// </summary>
    public class TextTrainReaderEx : IDisposable
    {
        public void Dispose()
        {
            
        }
   
        public static DataTable Read(string file,string[] columns)
        {
            DataTable  table= new DataTable();
            table.Columns.Add("DateStampTime");
            table.Columns.Add("MarkLabel");
            for(int i=0;i< columns.Length;i++)
            {
                table.Columns.Add(columns[i]);
            }
            string line = null;
            try
            {
                using (StreamReader sr = new StreamReader(file,Encoding.UTF8))
                {
                    sr.ReadLine();//取消读取第一行列头
                    while ((line = sr.ReadLineAsync().Result) != null)
                    {
                        string[] arrays = line.Split(',');
                        DataRow dataRow = table.NewRow();
                        dataRow.ItemArray = arrays.Take(columns.Length + 2).ToArray();
                        table.Rows.Add(dataRow);

                    }
                }
            }
            catch
            {

            }
            return table;
        }
        public static void Write(string file,DataTable dataTable)
        {
            if (dataTable == null)
            {
                return;
              
            }
            if(!Directory.Exists(Path.GetDirectoryName(file)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(file));
            }
            try
            {
                using (StreamWriter sw = new StreamWriter(file,false,Encoding.UTF8))
                {
                    List<string> cols = new List<string>();
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        cols.Add(dataTable.Columns[i].Caption);

                    }
                    string line = string.Join(",", cols);
                    sw.WriteLine(line);
                    foreach (DataRow dr in dataTable.Rows)
                    {

                        object[] arrays = dr.ItemArray;
                        line = string.Join(",", arrays);
                        sw.WriteLine(line);

                    }

                }
            }
            catch(Exception emx)
            {

            }
        }
    }
}
