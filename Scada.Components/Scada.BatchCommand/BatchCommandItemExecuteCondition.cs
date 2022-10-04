

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
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Scada.BatchCommand
{
   
    [Serializable]
    /// <summary>
    /// 下置条件设置
    /// </summary>
    public class BatchCommandItemWriteValue: ISerializable
    {  
		 
		public BatchCommandItemWriteValue(float v)
        {
            FixedValue = v;
        }
        public float FixedValue = 0;
        public BatchCommandItemWriteValueType WriteValueType { set; get; } = BatchCommandItemWriteValueType.固定数值;
		//要进行变动输入值得IO
		public BachCommand_IOPara ChangeIOPara { set; get; }
        public override string ToString()
        {
            if(ChangeIOPara!=null)
            return "下置方式:"+WriteValueType.ToString()+",下置值:"+ FixedValue+","+ ChangeIOPara.GetDataString();

            return "";
        }
        public string GetDataString()
        {
            if(ChangeIOPara!=null)
            return "WriteValueType:" + WriteValueType.ToString() + ",FixedValue:" + FixedValue + "," + ChangeIOPara.GetDataString();
            return "";

        }
        public BatchCommandItemWriteValue CreateFromDBString(string parastring)
        {

            if (!string.IsNullOrEmpty(parastring))
            {
                string[] arrays = parastring.Split(',');
                string[] arraysIo=new string[arrays.Length-2];
                Array.Copy(arrays, 2, arraysIo, 0, arraysIo.Length);
                WriteValueType = (BatchCommandItemWriteValueType)Enum.Parse(typeof(BatchCommandItemWriteValueType), arrays[0].Split(',')[1]);
                FixedValue = Convert.ToSingle(arrays[1].Split(',')[1]);

                ChangeIOPara = new BachCommand_IOPara();
                ChangeIOPara.CreateFromDBString(string.Join(",", arraysIo));
            }
            return this;

        }
        protected BatchCommandItemWriteValue(SerializationInfo info, StreamingContext context)
		{

			#region 自定义属性
			this.FixedValue = (float)info.GetValue("FixedValue", typeof(float));
			this.WriteValueType = (BatchCommandItemWriteValueType)info.GetValue("WriteValueType", typeof(BatchCommandItemWriteValueType));
			this.ChangeIOPara = (BachCommand_IOPara)info.GetValue("ChangeIOPara", typeof(BachCommand_IOPara));
		    #endregion





		}
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("FixedValue", this.FixedValue);
			info.AddValue("WriteValueType", this.WriteValueType);
			info.AddValue("ChangeIOPara", this.ChangeIOPara);
			 

		}
	}
}
