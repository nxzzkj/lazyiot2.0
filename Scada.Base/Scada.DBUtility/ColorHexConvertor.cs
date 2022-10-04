

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
using System.Text;
using System.Threading.Tasks;

namespace Scada.DBUtility
{
public   abstract  class ColorHexConvertor: IDisposable
    {
        #region [颜色：16进制转成RGB]
        /// <summary>
        /// [颜色：16进制转成RGB]
        /// </summary>
        /// <param name="strColor">设置16进制颜色 [返回RGB]</param>
        /// <returns></returns>
        public static  System.Drawing.Color colorHx16toRGB(string strHxColor)
        {
            try
            {
                if (strHxColor.Length == 0)
                {//如果为空
                    return System.Drawing.Color.FromArgb(0, 0, 0);//设为黑色
                }
                else
                {//转换颜色
                    return System.Drawing.Color.FromArgb(System.Int32.Parse(strHxColor.Substring(1, 2), System.Globalization.NumberStyles.AllowHexSpecifier), System.Int32.Parse(strHxColor.Substring(3, 2), System.Globalization.NumberStyles.AllowHexSpecifier), System.Int32.Parse(strHxColor.Substring(5, 2), System.Globalization.NumberStyles.AllowHexSpecifier));
                }
            }
            catch
            {//设为黑色
                return System.Drawing.Color.FromArgb(0, 0, 0);
            }
        }
        #endregion

        #region [颜色：RGB转成16进制]
        /// <summary>
        /// [颜色：RGB转成16进制]
        /// </summary>
        /// <param name="R">红 int</param>
        /// <param name="G">绿 int</param>
        /// <param name="B">蓝 int</param>
        /// <returns></returns>
        public static string colorRGBtoHx16(System.Drawing.Color color)
        {
            if (color.IsEmpty)
                return "#000000";
            if (color.IsEmpty)
                return "#000000";
            string R = Convert.ToString(color.R, 16).PadLeft(2,'0');
           
            string G = Convert.ToString(color.G, 16).PadLeft(2, '0'); ;
          
            string B = Convert.ToString(color.B, 16).PadLeft(2, '0'); ;
         
            string HexColor = "#" + R + G + B;
            return HexColor.ToUpper();
        }
        #endregion

        #region
        /// <summary>
        /// [颜色：RGB转成16进制]
        /// </summary>
        /// <param name="color">颜色</param>
        /// <returns>十六进制值，如果参数为空，默认返回#000000</returns>
        public static string ToHexColor(System.Drawing.Color color)
        {
            if (color.IsEmpty)
                return "#000000";
            string R = Convert.ToString(color.R, 16);
            if (R == "0")
                R = "00";
            string G = Convert.ToString(color.G, 16);
            if (G == "0")
                G = "00";
            string B = Convert.ToString(color.B, 16);
            if (B == "0")
                B = "00";
            string HexColor = "#" + R + G + B;
            return HexColor.ToUpper();
        }

        public void Dispose()
        {
        
        }
        #endregion
    }
}
