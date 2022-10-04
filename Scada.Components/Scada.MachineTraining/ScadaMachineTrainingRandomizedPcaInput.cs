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

namespace Scada.MachineTraining
{
    #region 异常监测
    public class ScadaMachineTrainingRandomizedPcaInput : ScadaMachineTrainingInput
    {
        public string DateStampTime { set; get; }
        public string MarkLabel { set; get; }

  


        public float Column1 { get; set; }


        public float Column2 { get; set; }

        public float Column3 { get; set; }

        public float Column4 { get; set; }

        public float Column5 { get; set; }

        public float Column6 { get; set; }

        public float Column7 { get; set; }

        public float Column8 { get; set; }

        public float Column9 { get; set; }

        public float Column10 { get; set; }
        public int ColumnNumber { set; get; }

        public string GetColumnValueString()
        {
            string str = "";
            for (int i = 1; i <= ColumnNumber; i++)
            {


                switch (i)
                {
                    case 1:
                        str += "" + Column1; break;

                    case 2:
                        str += "," + Column2; break;
                    case 3:
                        str += "," + Column3; break;
                    case 4:
                        str += "," + Column4; break;
                    case 5:
                        str += "," + Column5; break;
                    case 6:
                        str += "," + Column6; break;
                    case 7:
                        str += "," + Column7; break;
                    case 8:
                        str += "," + Column8; break;
                    case 9:
                        str += "," + Column9; break;
                    case 10:
                        str += "," + Column10; break;
                    default:
                        str = "";
                        break;

                }
            }
            return str;
        }


    }
    #endregion

}
