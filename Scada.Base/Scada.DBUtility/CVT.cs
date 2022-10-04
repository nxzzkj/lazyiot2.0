namespace Scada.DBUtility
{

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

    public class CVT: IDisposable
    {
        public static string BinDotBin(string bin)
        {
            string str = "";
            for (int i = 0; i < bin.Length; i++)
            {
                str = str + bin.Substring(i, 1) + ",";
            }
            return str.TrimEnd(new char[] { ',' });
        }

        public static string BitDivision(int begin, int end, byte byt)
        {
            string str = "1";
            int num = Convert.ToInt32(str.PadRight((end - begin) + 1, '1').PadLeft(end, '0').PadRight(8, '0'), 2);
            int num2 = byt;
            int num3 = num & num2;
            num3 = num3 >> (8 - end);
            return Convert.ToString(num3, 2).PadLeft((end - begin) + 1, '0');
        }

        public static byte[] ByteAdd(byte[] by1, byte[] by2)
        {
            int length = by1.Length;
            int num2 = by2.Length;
            byte[] buffer = new byte[length + num2];
            for (int i = 0; i < length; i++)
            {
                buffer[i] = by1[i];
            }
            for (int j = 0; j < num2; j++)
            {
                buffer[j + length] = by2[j];
            }
            return buffer;
        }
        /// <summary>
        /// 带空格的16进制字符串
        /// </summary>
        /// <param name="da"></param>
        /// <returns></returns>
        public static string ByteToHexStr(byte[] da)
        {
            string str = "";
            for (int i = 0; i < da.Length; i++)
            {
                str = str + Convert.ToString(da[i], 0x10).PadLeft(2, '0') + " ";
            }
            return str;
        }
        /// <summary>
        /// 不带空格的16进制字符串
        /// </summary>
        /// <param name="da"></param>
        /// <returns></returns>
        public static string ByteToHexStr2(byte[] da)
        {
            string str = "";
            for (int i = 0; i < da.Length; i++)
            {
                str = str + Convert.ToString(da[i], 0x10);
            }
            return str;
        }

        public static string HexStrToBinStr(string hex)
        {
            int totalWidth = hex.Length * 4;
            return Convert.ToString(Convert.ToInt32(hex, 0x10), 2).PadLeft(totalWidth, '0');
        }

        public static bool IsDigit(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if ((str[i] < '0') || (str[i] > '9'))
                {
                    return false;
                }
            }
            return true;
        }

        public static byte[] StrToHexByte(string da)
        {
            string str = da;
            str = str.Replace(" ", "").Replace("\n", "").Replace("\r", "");
            int num = str.Length / 2;
            byte[] buffer = new byte[num];
            for (int i = 0; i < num; i++)
            {
                buffer[i] = Convert.ToByte(str.Substring(i * 2, 2), 0x10);
            }
            return buffer;
        }

        public static string StrTOStrV(string str)
        {
            string str2 = "";
            int length = str.Length;
            for (int i = 0; i <= (length - 2); i += 2)
            {
                str2 = str.Substring(i, 2) + str2;
            }
            return str2;
        }

        public static byte[] SubByteArray(byte[] by, int index)
        {
            return SubByteArray(by, index, by.Length - index);
        }

        public static byte[] SubByteArray(byte[] by, int index, int length)
        {
            byte[] buffer = new byte[length];
            for (int i = index; i < (index + length); i++)
            {
                buffer[i - index] = by[i];
            }
            return buffer;
        }

        public static string ToSix(string str)
        {
            long num = Convert.ToInt64(str, 2);
            return ("0x" + Convert.ToString(num, 0x10).PadLeft(2, '0'));
        }

        public static string ToTen(string str)
        {
            return Convert.ToString(Convert.ToInt64(str, 2), 10);
        }

        public void Dispose()
        {
         
        }
    }
}

