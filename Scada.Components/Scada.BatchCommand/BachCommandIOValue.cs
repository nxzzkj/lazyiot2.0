

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
using Scada.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Scada.BatchCommand
{

    [Serializable]
    public class BachCommand_IOPara :  ISerializable
    {
         public IO_PARA IOParament { set; get; }
        public List<BachCommandBoolExpression> Expressions { set; get; } = new List<BachCommandBoolExpression>();

        /// <summary>
        /// 根据用户定义的表达式解析某个值
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="inputvalue"></param>
        /// <returns></returns>
        public float GetExpressionValue(float inputvalue)
        {
            float defaultValue = -9999;
            
            List<BachCommandBoolExpression> expressions1 = Expressions.FindAll(x => x.OpSymbol == "<=");
            if (expressions1 != null)
            {
                expressions1.Sort(delegate (BachCommandBoolExpression e1, BachCommandBoolExpression e2)
                {
                    //降序排列
                    return e2.Value.CompareTo(e1.Value);
                });
                expressions1.ForEach(delegate (BachCommandBoolExpression p)
                {
                    if (inputvalue <= p.Value)
                    {
                        defaultValue = p.DefaultValue;
                    }

                });
            }

            List<BachCommandBoolExpression> expressions2 = Expressions.FindAll(x => x.OpSymbol == "<");
            if (expressions2 != null)
            {
                expressions2.Sort(delegate (BachCommandBoolExpression e1, BachCommandBoolExpression e2)
                {
                    //降序排列
                    return e2.Value.CompareTo(e1.Value);
                });
                //进行分析获取该那个值
                expressions2.ForEach(delegate (BachCommandBoolExpression p)
                {
                    if (inputvalue < p.Value)
                    {
                        defaultValue= p.DefaultValue;
                    }

                });
            }
          
           
            List<BachCommandBoolExpression> expressions3 = Expressions.FindAll(x => x.OpSymbol == ">=");
            if (expressions3 != null)
            {
                expressions3.Sort(delegate (BachCommandBoolExpression e1, BachCommandBoolExpression e2)
                {
                    //升序排列
                    return e1.Value.CompareTo(e2.Value);


                });
                expressions3.ForEach(delegate (BachCommandBoolExpression p)
                {
                    if (inputvalue > p.Value)
                    {
                        defaultValue = p.DefaultValue;
                    }

                });
            }
            List<BachCommandBoolExpression> expressions4 = Expressions.FindAll(x => x.OpSymbol == ">");
            if (expressions4 != null)
            {
                expressions4.Sort(delegate (BachCommandBoolExpression e1, BachCommandBoolExpression e2)
                {
                    //升序排列
                    return e1.Value.CompareTo(e2.Value);


                });
                expressions4.ForEach(delegate (BachCommandBoolExpression p)
                {
                    if (inputvalue > p.Value)
                    {
                        defaultValue = p.DefaultValue;
                    }

                });
            }
            List<BachCommandBoolExpression> expressions5 = Expressions.FindAll(x => x.OpSymbol == "=");
            if (expressions5 != null)
            {
                expressions5.Sort(delegate (BachCommandBoolExpression e1, BachCommandBoolExpression e2)
                {
                    //升序排列
                    return e1.Value.CompareTo(e2.Value);

                });

                expressions4.ForEach(delegate (BachCommandBoolExpression p)
                { 
                    if (inputvalue == p.Value)
                    {
                        defaultValue = p.DefaultValue;
                    }

                });
            }
            return defaultValue;
        }
        public BachCommand_IOPara()
        {
           
        }

        public override string ToString()
        {
            if(IOParament!=null)
            {
                string str = IOParament.ToString();

                return str;
            }
            return "";
            
        }


        public   string GetDataString()
        {
            string str = "Expressions:";
            for (int i = 0; i < Expressions.Count; i++)
            {
                str += "" + Expressions[i].GetDataString()+";";
            }
            str = str.Substring(0, str.Length-1);
            if (IOParament != null)
                return "SERVER_ID:" + IOParament.IO_SERVER_ID + ",COMM_ID:" + IOParament.IO_COMM_ID + ",DEVICE_ID:" + IOParament.IO_DEVICE_ID + ",IO_ID:" + IOParament.IO_ID + "," + str;
            else
                return "";

        }
        public BachCommand_IOPara CreateFromDBString(string parastring)
        {

            if (!string.IsNullOrEmpty(parastring))
            {
                string[] arrays = parastring.Split(',');
                IOParament = new IO_PARA()
                {
                    IO_SERVER_ID = arrays[0],
                    IO_COMM_ID = arrays[1],
                    IO_DEVICE_ID = arrays[2],
                    IO_ID = arrays[3]


                };
                Expressions = new List<BachCommandBoolExpression>();
                if (arrays.Length>4)
                {
                    for(int i=4;i< arrays.Length;i++)
                    {
                        BachCommandBoolExpression boolExpression = new BachCommandBoolExpression();
                        boolExpression.CreateFromDBString(arrays[i]);
                        Expressions.Add(boolExpression);
                    }
                     
                }
              
               
            }
            return this;
           
        }

        #region  序列化和反序列化

        /// <summary>
        /// Deserialization constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected BachCommand_IOPara(SerializationInfo info, StreamingContext context)
        {

            #region 自定义属性
      

            this.IOParament = (IO_PARA)info.GetValue("IOParament", typeof(bool));

            this.Expressions = (List<BachCommandBoolExpression>)info.GetValue("Expressions", typeof(List<BachCommandBoolExpression>));
           
            
            #endregion

        }


        public   void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            

            info.AddValue("IOParament", this.IOParament);
            info.AddValue("Expressions", this.Expressions);
 
        }

         

        #endregion
    }

 
     
}
