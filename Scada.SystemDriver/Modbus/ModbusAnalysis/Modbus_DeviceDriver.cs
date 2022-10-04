

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
using Scada.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scada.Model;
using Scada.IOStructure;
using System.ComponentModel;
using Modbus.Globel;
using Modbus.Device;

namespace Modbus.ModbusAnalysis
{
  public  class Modbus_DeviceDriver: ScadaDeviceKernel
    {
        private   string mGuid = "";
        /// <summary>
        /// 驱动唯一标识，采用系统GUID分配
        /// </summary>
        public override string GUID
        {
            get
            {
                return mGuid;
            }
            set
            {
                mGuid = value;

            }



        }
        private string mTitle = " Modbus协议解析";
        public override string Title
        {
            get
            {
                return mTitle;
            }

            set
            {
                mTitle = value;
            }
        }
        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="server"></param>
        /// <param name="Communication"></param>
        /// <param name="device"></param>
        /// <param name="para"></param>
        /// <param name="datas"></param>
        /// <param name="datatime"></param>
        /// <param name="sender"></param>
        /// <returns></returns>
        protected override IOData Analysis(IO_SERVER server, IO_COMMUNICATION Communication, IO_DEVICE device, IO_PARA para, byte[] datas, DateTime datatime, object sender)
        {
            if (datas.Length > 0 && sender != null && para != null)
            {
                ParaPack paraPack = new ParaPack(para.IO_PARASTRING);
                ModbusFragmentStore modbusStore = (ModbusFragmentStore)sender;
                IOData data = new IOData();
                data.CommunicationID = Communication.IO_COMM_ID;
                data.ID = device.IO_DEVICE_ADDRESS;
                data.ParaName = para.IO_NAME;
                data.ServerID = server.SERVER_ID;
                data.Date = DateTime.Now;
                int numRegister = 1;
                Modbus_Type dataType = (Modbus_Type)Enum.Parse(typeof(Modbus_Type), paraPack.GetValue("数据类型"));
                int charsize = int.Parse(paraPack.GetValue("字节长度"));
                switch (dataType)
                {
                    case Modbus_Type.单精度浮点数32位:
                        data.DataType = typeof(float);
                        numRegister = 2;
                        break;
                    case Modbus_Type.双精度浮点数64位:
                        data.DataType = typeof(double);
                        numRegister = 4;
                        break;
                    case Modbus_Type.字符型:
                        data.DataType = typeof(string);
                        
                        break;
                    case Modbus_Type.无符号整数16位:
                        data.DataType = typeof(UInt16);
                        numRegister = 1;
                        break;
                    case Modbus_Type.无符号整数32位:
                        data.DataType = typeof(UInt32);
                        numRegister = 2;
                        break;
                    case Modbus_Type.无符号整数8位:
                        data.DataType = typeof(byte);
                        numRegister = 1;
                        break;
                    case Modbus_Type.有符号整数16位:
                        data.DataType = typeof(Int16);
                        numRegister = 1;
                        break;
                    case Modbus_Type.有符号整数8位:
                        data.DataType = typeof(sbyte);
                        numRegister = 1;
                        break;
                    case Modbus_Type.有符号整数32位:
                        data.DataType = typeof(int);
                        numRegister = 2;
                        break;
                }
                data.datas = datas;
                data.ParaString = para.IO_PARASTRING;
                //获取数据值高八位,低八位
                int startaddr = int.Parse(paraPack.GetValue("偏置"));
            
                int bitDataIndex=  int.Parse(paraPack.GetValue("数据位"));
                string code = paraPack.GetValue("内存区");
                bool bitSave = paraPack.GetValue("按位存取") == "1" ? true : false;
                bool ishigh = paraPack.GetValue("存储位置") == "高八位" ? true : false;
                ModbusFragment fragment = new ModbusFragment()
                {
                    Code = code,
                    DevAddress = Convert.ToByte(device.IO_DEVICE_ADDRESS),
                    StartRegister = Convert.ToUInt16(startaddr),
                    RegisterNum = Convert.ToUInt16(numRegister),
                    Length = Convert.ToUInt16(numRegister * 2),
                    StartIndex = Convert.ToUInt16(startaddr * 2)
                };


                switch (fragment.Code)
                {
                    case "01"://线圈
                        {
                            byte[] vdatas = new byte[fragment.Length];
                            Array.Copy(datas, fragment.StartIndex, vdatas, 0, fragment.Length);
                            data.ParaValue = vdatas[startaddr].ToString();
                        }
                        break;
                    case "02"://线圈只读
                        {
                            byte[] vdatas = new byte[fragment.Length];
                            Array.Copy(datas, fragment.StartIndex, vdatas, 0, fragment.Length);
                            data.ParaValue = vdatas[startaddr].ToString();
                        }
                        break;
                    case "03"://寄存器可读可写
                        {
                            //注意是否按位读取，
                            byte[] vdatas = new byte[fragment.Length];
                            Array.Copy(datas, fragment.StartIndex, vdatas, 0, fragment.Length);
                            ushort[] vals = ModbusConvert.Bytes2Ushorts(vdatas);//将字节数组转为ushort                     
                            switch (dataType)
                            {
                                case Modbus_Type.单精度浮点数32位:
                                    {
                                        data.ParaValue = ModbusConvert.GetReal(vals, 0).ToString();
                                    }
                                    break;
                                case Modbus_Type.双精度浮点数64位:
                                    {
                                        data.ParaValue = ModbusConvert.GetDouble(vals, 0).ToString();
                                    }
                                    break;
                                case Modbus_Type.字符型:
                                    {
                                        data.ParaValue = ModbusConvert.GetString(vals, 0, charsize).ToString();
                                    }
                                    break;
                                case Modbus_Type.无符号整数16位:
                                    {
                                    
                                        if(bitSave)//是否按位读取，如果是按位读取，则1个寄存器存储16个二进制，全部存储的是开关量
                                        {
                                            int v = 0;
                                            if (ishigh)//表示在高8位
                                            {
                                                v = ModbusConvert.Get_Int_bit(vdatas[0], bitDataIndex);
                                            }
                                            else//表示在低8位
                                            {
                                                v = ModbusConvert.Get_Int_bit(vdatas[1], bitDataIndex);
                                            }


                                            data.ParaValue = v.ToString() ;
                                        }
                                        else
                                        {
                                            data.ParaValue = ModbusConvert.GetUShort(vals, 0).ToString();
                                        }

                                    }
                                    break;
                                case Modbus_Type.有符号整数16位:
                                    {
                                        if (bitSave)//是否按位读取，如果是按位读取，则1个寄存器存储16个二进制，全部存储的是开关量
                                        {
                                            int v = 0;
                                            if (ishigh)//表示在高8位
                                            {
                                                v = ModbusConvert.Get_Int_bit(vdatas[0], bitDataIndex);
                                            }
                                            else//表示在低8位
                                            {
                                                v = ModbusConvert.Get_Int_bit(vdatas[1], bitDataIndex);
                                            }


                                            data.ParaValue = v.ToString();
                                        }
                                        else
                                        {
                                            data.ParaValue = ModbusConvert.GetShort(vals, 0).ToString();
                                        }
                                   
                                    }
                                    break;
                                case Modbus_Type.无符号整数32位:
                                    {
                                        data.ParaValue = ModbusConvert.GetUInt(vals, 0).ToString();
                                    }
                                    break;
                                case Modbus_Type.无符号整数8位:
                                    {
                                        data.ParaValue = ModbusConvert.GetSByte(vals, 0, ishigh).ToString();
                                    }
                                    break;
                              
                                case Modbus_Type.有符号整数8位:
                                    {
                                        data.ParaValue = ModbusConvert.GetByte(vals, 0, ishigh).ToString();
                                    }
                                    break;
                                case Modbus_Type.有符号整数32位:
                                    {
                                        data.ParaValue = ModbusConvert.GetInt(vals, 0).ToString();
                                    }
                                    break;
                            }

                        }
                        break;
                    case "04"://寄存器只读
                        {
                            //注意是否按位读取，
                            byte[] vdatas = new byte[fragment.Length];
                            Array.Copy(datas, fragment.StartIndex, vdatas, 0, fragment.Length);
                            ushort[] vals = ModbusConvert.Bytes2Ushorts(vdatas);//将字节数组转为ushort                     
                            switch (dataType)
                            {
                                case Modbus_Type.单精度浮点数32位:
                                    {
                                        data.ParaValue = ModbusConvert.GetReal(vals, 0).ToString();
                                    }
                                    break;
                                case Modbus_Type.双精度浮点数64位:
                                    {
                                        data.ParaValue = ModbusConvert.GetDouble(vals, 0).ToString();
                                    }
                                    break;
                                case Modbus_Type.字符型:
                                    {
                                        data.ParaValue = ModbusConvert.GetString(vals, 0, charsize).ToString();
                                    }
                                    break;
                                case Modbus_Type.无符号整数16位:
                                    {

                                        if (bitSave)//是否按位读取，如果是按位读取，则1个寄存器存储16个二进制，全部存储的是开关量
                                        {
                                            int v = 0;
                                            if (ishigh)//表示在高8位
                                            {
                                                v = ModbusConvert.Get_Int_bit(vdatas[0], bitDataIndex);
                                            }
                                            else//表示在低8位
                                            {
                                                v = ModbusConvert.Get_Int_bit(vdatas[1], bitDataIndex);
                                            }


                                            data.ParaValue = v.ToString();
                                        }
                                        else
                                        {
                                            data.ParaValue = ModbusConvert.GetUShort(vals, 0).ToString();
                                        }

                                    }
                                    break;
                                case Modbus_Type.有符号整数16位:
                                    {
                                        if (bitSave)//是否按位读取，如果是按位读取，则1个寄存器存储16个二进制，全部存储的是开关量
                                        {
                                            int v = 0;
                                            if (ishigh)//表示在高8位
                                            {
                                                v = ModbusConvert.Get_Int_bit(vdatas[0], bitDataIndex);
                                            }
                                            else//表示在低8位
                                            {
                                                v = ModbusConvert.Get_Int_bit(vdatas[1], bitDataIndex);
                                            }


                                            data.ParaValue = v.ToString();
                                        }
                                        else
                                        {
                                            data.ParaValue = ModbusConvert.GetShort(vals, 0).ToString();
                                        }

                                    }
                                    break;
                                case Modbus_Type.无符号整数32位:
                                    {
                                        data.ParaValue = ModbusConvert.GetUInt(vals, 0).ToString();
                                    }
                                    break;
                                case Modbus_Type.无符号整数8位:
                                    {
                                        data.ParaValue = ModbusConvert.GetSByte(vals, 0, ishigh).ToString();
                                    }
                                    break;
                               
                                case Modbus_Type.有符号整数8位:
                                    {
                                        data.ParaValue = ModbusConvert.GetByte(vals, 0, ishigh).ToString();
                                    }
                                    break;
                                case Modbus_Type.有符号整数32位:
                                    {
                                        data.ParaValue = ModbusConvert.GetInt(vals, 0).ToString();
                                    }
                                    break;
                            }

                        }
                        break;
                }
                data.QualityStamp = QualityStamp.GOOD;
                return string.IsNullOrEmpty(data.ParaValue) ? null : data;
            }
            return null;
        }
        protected override bool InitDeviceKernel(IO_SERVER server, IO_COMMUNICATION communication, IO_DEVICE device, IO_PARA para, SCADA_DEVICE_DRIVER driver)
        {
            if (IsCreateControl)
            {
                if (para != null)
                {
                    this.ParaCtrl = new IOParaCtrl();
                    this.ParaCtrl.SetUIParameter(server, device, para);


                }
                this.DeviceCtrl = new Modbus_TCP_Device();
                this.DeviceCtrl.SetUIParameter(server, device);
            }
            if (para != null)
            {
                this.ParaString = para.IO_PARASTRING;
            }
            this.DeviceParaString = device.IO_DEVICE_PARASTRING;
            return true;
        }

    
    }
}
