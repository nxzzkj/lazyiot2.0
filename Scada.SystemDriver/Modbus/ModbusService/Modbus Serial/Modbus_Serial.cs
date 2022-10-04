
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Scada.Model;
using Scada.Kernel;
using System.IO.Ports;
using System.Threading;
using Modbus.Device;
using Modbus.Data;
using Modbus.Globel;

namespace Modbus.ModbusService
{

    public enum SerialCheck
    {
        无 = 1,
        偶校验 = 2,
        奇校验 = 3,
        常1 = 4,
        常0 = 5



    }

    public class Modbus_Serial_PARA
    {
        /// <summary>
        /// 串口名称
        /// </summary>
        public string SerialPort = "";
 
        /// <summary>
        /// 波特率
        /// </summary>
        public int BaudRate = 19200;
        /// <summary>
        /// 校验
        /// </summary>
        public SerialCheck SerialCheck = SerialCheck.无;
        /// <summary>
        /// 数据位
        /// </summary>
        public int DataBits = 8;
        /// <summary>
        /// 停止位
        /// </summary>
        public StopBits StopBits = StopBits.One;
 
        /// <summary>
        /// 偏移间隔
        /// </summary>
        public int OffsetInterval = 10;
 
        public bool RTSEnable = false;
        public bool DTREnable = false;
        public Handshake Handshake = Handshake.None;
        public int ReadTimeout = 1000;
        public int WriteTimeout = 1000;
        public int ReadBuffSize = 1024;
        public int WriteBuffSize = 1024;
    }

    public class Modbus_Serial_Device_PARA
    {

        /// <summary>
        /// 是否连续采集3次失败
        /// </summary>
        public bool ContinueCollect = false;
        public int CollectFaultsNumber = 3;
        public int CollectFaultsInternal = 15;
        public string ModbusType = "ASCII";

    }
    public class ModbusBusSerialObject
    {

        public RealData RealData { set; get; }
        public IO_DEVICE Device { set; get; }

        public Modbus_Serial_Device_PARA Para { set; get; }
        public ModbusSerialMaster Master { set; get; }

    }
    public class Modbus_Serial : ScadaCommunicateKernel
    {
        private const string mGuid = "310AA8D3-DBCE-43F8-81A9-DFFD9C5D7D3A";
        /// <summary>
        /// 驱动唯一标识，采用系统GUID分配
        /// </summary>
        public override string GUID
        {
            get
            {
                return mGuid;
            }


        }
        private string mTitle = " Modbus Serial";
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
        public Modbus_Serial()
        {
            this.AssignDeviceProtocols = new List<string>();
            this.AssignDeviceProtocols.Add("945F7BEA-ED7E-4668-B145-C809FC5DA299");
        }
        public SerialPort SerialPort { set; get; }


        Modbus_Serial_PARA Tcp_PARA = new Modbus_Serial_PARA();
        List<ModbusBusSerialObject> busTcpObjects = null;
        protected override bool InitCommunicateKernel(IO_SERVER server, IO_COMMUNICATION communication, List<IO_DEVICE> ioDevices, SCADA_DRIVER driver)
        {
            this.AssignDeviceProtocols = new List<string>();
            this.AssignDeviceProtocols.Add("945F7BEA-ED7E-4668-B145-C809FC5DA299");
            if (IsCreateControl)
            {
                CommunicationControl = new Modbus_Serial_Ctrl();
                if (communication != null)
                    CommunicationControl.SetUIParameter(communication.IO_COMM_PARASTRING);

            }


            busTcpObjects = new List<ModbusBusSerialObject>();

            if (communication != null)
            {
                Tcp_PARA = new Modbus_Serial_PARA();
                ParaPack TcpParaPack = new ParaPack(communication.IO_COMM_PARASTRING);

                Tcp_PARA.SerialPort = TcpParaPack.GetValue("串口");
                Tcp_PARA.SerialCheck =(SerialCheck)Enum.Parse(typeof(SerialCheck), TcpParaPack.GetValue("校验"));
                Tcp_PARA.WriteTimeout = int.Parse(TcpParaPack.GetValue("写超时时间"));
                Tcp_PARA.ReadTimeout = int.Parse(TcpParaPack.GetValue("读超时时间"));
                Tcp_PARA.BaudRate = int.Parse(TcpParaPack.GetValue("波特率"));
                Tcp_PARA.DataBits = int.Parse(TcpParaPack.GetValue("数据位"));
                Tcp_PARA.StopBits = (StopBits)Enum.Parse(typeof(StopBits), TcpParaPack.GetValue("停止位"));
                Tcp_PARA.OffsetInterval = int.Parse(TcpParaPack.GetValue("偏移间隔"));
                Tcp_PARA.Handshake = (Handshake)Enum.Parse(typeof(Handshake), TcpParaPack.GetValue("握手协议"));
                Tcp_PARA.ReadTimeout = int.Parse(TcpParaPack.GetValue("写超时时间"));
                Tcp_PARA.WriteTimeout = int.Parse(TcpParaPack.GetValue("读超时时间"));
     
                Tcp_PARA.RTSEnable = bool.Parse(TcpParaPack.GetValue("RTS"));
                Tcp_PARA.DTREnable = bool.Parse(TcpParaPack.GetValue("DTR"));
                Tcp_PARA.WriteBuffSize = int.Parse(TcpParaPack.GetValue("写缓存"));
                Tcp_PARA.ReadBuffSize = int.Parse(TcpParaPack.GetValue("读缓存"));
                //构造获取数据命令的字节数组,Modbus
                for (int i = 0; i < this.IODevices.Count; i++)
                {
                    ParaPack devicePack = new ParaPack(this.IODevices[i].IO_DEVICE_PARASTRING);
                    ModbusBusSerialObject modbusBusTcp = new ModbusBusSerialObject();
                    modbusBusTcp.Para = new Modbus_Serial_Device_PARA();
                    modbusBusTcp.Para.CollectFaultsInternal = Convert.ToInt32(devicePack.GetValue("重试间隔"));
                    modbusBusTcp.Para.CollectFaultsNumber = Convert.ToInt32(devicePack.GetValue("重试次数"));
                    modbusBusTcp.Para.ContinueCollect = devicePack.GetValue("重试") == "1" ? true : false;
                    modbusBusTcp.Para.ModbusType = devicePack.GetValue("通讯协议");
                    object fragment = new ModbusFragmentStore();
                    RealData mRealData = new RealData();
                    mRealData.Device = this.IODevices[i];
                    ScadaDeviceKernel driverDll = DeviceDrives.Find(x => x.DeviceDriverID == this.IODevices[i].DEVICE_DRIVER_ID);
                    if (driverDll != null)
                    {
                        driverDll.InitKernel(IOServer, IOCommunication, this.IODevices[i], null, this.IODevices[i].DriverInfo);
                        //IO_DEVICE_ADDRESS中存储的是DTU编号
                        mRealData.SlaveId = this.IODevices[i].IO_DEVICE_ADDRESS;
                        //数据库中系统编号
                        mRealData.DEVICEID = this.IODevices[i].IO_DEVICE_ID;
                        ////获取下发命令的参数,注意此次要进心分段存储，因为modbus一次不能超过123个寄存器地址
                        mRealData.ReadSendByte = GetDataCommandBytes(this.IOServer, this.IOCommunication, this.IODevices[i], this.IODevices[i].IOParas, null, ref fragment);
                    }
                    mRealData.Fragment = (ModbusFragmentStore)fragment;
                    modbusBusTcp.RealData = mRealData;
                    modbusBusTcp.Device = this.IODevices[i];
                    busTcpObjects.Add(modbusBusTcp);

                }
            }

            return true;


        }
        private List<byte[]> GetDataCommandBytes(IO_SERVER server, IO_COMMUNICATION Communication, IO_DEVICE device, List<IO_PARA> paras, IO_PARA currentpara, ref object sender)
        {
            List<byte[]> cmmdBytes = new List<byte[]>();
            //必须Read的IO参数,一个设备节点可能有多种命令，要对命令进行归类整理
            List<ModbusFragmentStore> modbusCodes = new List<ModbusFragmentStore>();
            for (int i = 0; i < paras.Count; i++)
            {
                ParaPack paraPack = new ParaPack(paras[i].IO_PARASTRING);
                if (!modbusCodes.Exists(x => x.StoredCode == paraPack.GetValue("内存区")))
                {
                    ModbusFragmentStore stored = new ModbusFragmentStore();
                    stored.StoredCode = paraPack.GetValue("内存区");
                    stored.Fragments = new List<ModbusFragment>();
                    stored.Units = new List<ushort>();
                    modbusCodes.Add(stored);
                }
                paraPack.Dispose();
                paraPack = null;
            }
            //将对应的内存器的地址加入到对应分段的集合中
            for (int i = 0; i < paras.Count; i++)
            {
                ParaPack paraPack = new ParaPack(paras[i].IO_PARASTRING);
                if (paraPack.GetValue("内存区") != "")
                {
                    if (modbusCodes.Exists(x => x.StoredCode == paraPack.GetValue("内存区")))
                    {
                        ModbusFragmentStore stored = modbusCodes.Find(x => x.StoredCode == paraPack.GetValue("内存区"));
                        if (paraPack.GetValue("偏置") != "")
                        {
                            ushort offset = 0;
                            if (ushort.TryParse(paraPack.GetValue("偏置"), out offset))
                            {
                                if (!stored.Units.Contains(offset))
                                    stored.Units.Add(offset);
                            }

                        }

                    }


                }

            }
            ModbusFragmentStore mainStored = new ModbusFragmentStore();
            //由于modbus获取寄存器最大数量是124个，所以要进行分段，最大线圈数量是1999个
            foreach (ModbusFragmentStore stored in modbusCodes)
            {
                stored.MakeFragment();
                mainStored.Fragments.AddRange(stored.Fragments);
            }
            //获取要解析的命令
            sender = mainStored;
            return cmmdBytes;
        }
        private bool RequestData(IO_DEVICE device, ModbusBusSerialObject rData, out string error, ModbusFragmentStore fragmentstore)
        {
            ModbusSerialMaster master = rData.Master;
            RealData realData = rData.RealData;
            error = "";
            bool res = true;
            try
            {


                //分段读取数据，如果是读取整个寄存器的话，一次只能最多读取123个，
                //如果是读取线圈的话最大只能读取1999个，因此要分段进行数据的读取
                List<byte> allbytes = new List<byte>();
                try
                {
                    for (int i = 0; i < fragmentstore.Fragments.Count; i++)
                    {
                        ModbusFragment fragment = fragmentstore.Fragments[i];
                        switch (fragment.Code)
                        {
                            case "01":// 01和05是一个码 可写可读
                                {
                                    //返回的线圈状态,由于线圈是按位操作，转换也是按位转换
                                    bool[] result = master.ReadCoils(byte.Parse(device.IO_DEVICE_ADDRESS), fragment.StartRegister, fragment.RegisterNum);

                                    byte[] bytes = ModbusConvert.BoolToByte(result);
                                    fragment.StartIndex = allbytes.Count;
                                    fragment.Length = bytes.Length;
                                    allbytes.AddRange(bytes);

                                }
                                break;
                            case "02"://只读属性
                                {
                                    //返回的线圈状态
                                    bool[] result = master.ReadInputs(byte.Parse(device.IO_DEVICE_ADDRESS), fragment.StartRegister, fragment.RegisterNum);
                                    byte[] bytes = ModbusConvert.BoolToByte(result);
                                    fragment.StartIndex = allbytes.Count;
                                    fragment.Length = bytes.Length;
                                    allbytes.AddRange(bytes);
                                }
                                break;
                            case "03"://HR保持寄存器，可写可读
                                {
                                    //返回的数据全部是ushort 需要将ushort 转换为byte在进行传递
                                    ushort[] result = master.ReadHoldingRegisters(byte.Parse(device.IO_DEVICE_ADDRESS), fragment.StartRegister, fragment.RegisterNum);

                                    byte[] bytes = ModbusConvert.Ushorts2Bytes(result);

                                    fragment.StartIndex = allbytes.Count;
                                    fragment.Length = bytes.Length;
                                    allbytes.AddRange(bytes);

                                }
                                break;
                            case "04"://只读属性
                                {

                                    //返回的数据全部是ushort 需要将ushort 转换为byte在进行传递
                                    ushort[] result = master.ReadInputRegisters(byte.Parse(device.IO_DEVICE_ADDRESS), fragment.StartRegister, fragment.RegisterNum);
                                    byte[] bytes = ModbusConvert.Ushorts2Bytes(result);
                                    fragment.StartIndex = allbytes.Count;
                                    fragment.Length = bytes.Length;
                                    allbytes.AddRange(bytes);
                                }
                                break;
                        }

                    }

                }
                catch (Exception emx)
                {
                    error = emx.Message;
                    //读取异常处理
                    this.DeviceOffline(this.IOServer, this.IOCommunication, device, null);//tag为1表示上线，如果为0表示下线

                }
                //将数据返回到采集客户端
                if (allbytes.Count > 0)
                {
                    device.IO_DEVICE_STATUS = 1;
                    ReceiveData(this.IOServer, this.IOCommunication, device, allbytes.ToArray(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), fragmentstore);
                    //设置设备状态
                    this.DeviceOnline(this.IOServer, this.IOCommunication, device, null);//tag为1表示上线，如果为0表示下线
                    res = true;
                }
                else
                {
                    device.IO_DEVICE_STATUS = 0;
                    //设置设备状态
                    this.DeviceOffline(this.IOServer, this.IOCommunication, device, null);//tag为1表示上线，如果为0表示下线
                    res = false;
                }




            }
            catch
            {
                res = false;
            }
            return res;
        }
        private bool ResponseData(IO_SERVER server, IO_COMMUNICATION comm, IO_DEVICE device, IO_PARA para, string value, ModbusBusSerialObject rData)
        {
            ModbusSerialMaster master = rData.Master;
            if (para == null)
                return false;
            if (para.IO_POINTTYPE == "计算值" || para.IO_POINTTYPE == "关系数据库值")
            {
                return false;
            }
            //设备地址不能为空
            if (device.IO_DEVICE_ADDRESS == "")
                return false;

            try
            {
                //通过设备驱动进行数据解析，并生成下置的数据bytes
                if (device.DeviceDrive != null)
                {
                    ScadaDeviceKernel Driver = (ScadaDeviceKernel)device.DeviceDrive;
                    ParaPack paraPack = new ParaPack(para.IO_PARASTRING);
                    if (paraPack.Count > 0)
                    {
                        ushort offset = ushort.Parse(paraPack.GetValue("偏置"));
                        string code = paraPack.GetValue("内存区");
                        switch (code)
                        {
                            case "01":
                                {
                                    if (ushort.Parse(value) > 0)
                                    {
                                        master.WriteSingleCoil(byte.Parse(device.IO_DEVICE_ADDRESS), offset, true);
                                    }
                                    else
                                    {
                                        master.WriteSingleCoil(byte.Parse(device.IO_DEVICE_ADDRESS), offset, false);
                                    }

                                }
                                break;
                            case "03":
                                {
                                    Modbus_Type datatype = (Modbus_Type)Enum.Parse(typeof(Modbus_Type), paraPack.GetValue("数据类型"));
                                    bool ishigh = paraPack.GetValue("存储位置") == "高八位" ? true : false;
                                    int charsize = int.Parse(paraPack.GetValue("字节长度"));
                                    bool isposition = paraPack.GetValue("按位存取") == "1" ? true : false;
                                    ushort dataposition = ushort.Parse(paraPack.GetValue("数据位"));
                                    switch (datatype)
                                    {
                                        case Modbus_Type.单精度浮点数32位:
                                            {
                                                ushort[] buff = new ushort[2];
                                                float WriteValue = float.Parse(value);
                                                ModbusConvert.SetReal(buff, 0, WriteValue);
                                                master.WriteMultipleRegisters(byte.Parse(device.IO_DEVICE_ADDRESS), offset, buff);
                                            }
                                            break;
                                        case Modbus_Type.双精度浮点数64位:
                                            {
                                                ushort[] buff = new ushort[4];
                                                double WriteValue = double.Parse(value);
                                                ModbusConvert.SetDouble(buff, 0, WriteValue);
                                                master.WriteMultipleRegisters(byte.Parse(device.IO_DEVICE_ADDRESS), offset, buff);
                                            }
                                            break;
                                        case Modbus_Type.字符型:
                                            {
                                                ushort[] buff = new ushort[charsize];
                                                string WriteValue = value;
                                                if (value.Length > charsize)
                                                    WriteValue = value.Substring(0, charsize);
                                                if (value.Length < charsize)
                                                    WriteValue = value.PadRight(charsize, ' ');
                                                ModbusConvert.SetString(buff, 0, WriteValue);
                                                master.WriteMultipleRegisters(byte.Parse(device.IO_DEVICE_ADDRESS), offset, buff);

                                            }
                                            break;
                                        case Modbus_Type.无符号整数8位:
                                            {
                                                ushort[] buff = new ushort[1];
                                                byte WriteValue = byte.Parse(value);
                                                ModbusConvert.SetByte(buff, 0, WriteValue, ishigh);
                                                master.WriteSingleRegister(byte.Parse(device.IO_DEVICE_ADDRESS), offset, buff[0]);
                                            }
                                            break;
                                        case Modbus_Type.有符号整数8位:
                                            {
                                                ushort[] buff = new ushort[1];
                                                sbyte WriteValue = sbyte.Parse(value);
                                                ModbusConvert.SetSByte(buff, 0, WriteValue, ishigh);
                                                master.WriteSingleRegister(byte.Parse(device.IO_DEVICE_ADDRESS), offset, buff[0]);
                                            }
                                            break;
                                        case Modbus_Type.无符号整数16位:
                                            {
                                                UInt16 WriteValue = UInt16.Parse(value);
                                                if (isposition)//如果参数是按位存取，则需要再次读取指定的设备数据，将读取的数据的值转化成二进制，然后重新设置二进制指定位置的值后，在将该二进制转化成对应整数并写入对应的寄存器
                                                {
                                                    //获取当前寄存器的值
                                                    ushort[] datas = master.ReadHoldingRegisters(byte.Parse(device.IO_DEVICE_ADDRESS), offset, 1);
                                                    byte[] sourcebyte = ModbusConvert.Ushorts2Bytes(datas);
                                                    byte dataValueUp = sourcebyte[0];
                                                    byte dataValueDown = sourcebyte[1];

                                                    if (ishigh)
                                                    {
                                                        dataValueUp = Convert.ToByte(ModbusConvert.Set_Int_bit(dataValueUp, dataposition - 1, WriteValue > 0 ? true : false));
                                                    }
                                                    else
                                                    {
                                                        dataValueDown = Convert.ToByte(ModbusConvert.Set_Int_bit(dataValueDown, dataposition - 1, WriteValue > 0 ? true : false));
                                                    }


                                                    sourcebyte[0] = dataValueUp;
                                                    sourcebyte[1] = dataValueDown;
                                                    datas = ModbusConvert.Bytes2Ushorts(sourcebyte);
                                                    ushort newValue = ModbusConvert.GetUShort(datas, 0);

                                                    master.WriteSingleRegister(byte.Parse(device.IO_DEVICE_ADDRESS), offset, newValue);

                                                }
                                                else
                                                {

                                                    ushort[] buff = new ushort[1];
                                                    ModbusConvert.SetUShort(buff, 0, WriteValue);
                                                    master.WriteSingleRegister(byte.Parse(device.IO_DEVICE_ADDRESS), offset, buff[0]);
                                                }
                                            }
                                            break;
                                        case Modbus_Type.有符号整数16位:
                                            {
                                                Int16 WriteValue = Int16.Parse(value);
                                                if (isposition)
                                                {
                                                    //获取当前寄存器的值
                                                    ushort[] datas = master.ReadHoldingRegisters(byte.Parse(device.IO_DEVICE_ADDRESS), offset, 1);
                                                    byte[] sourcebyte = ModbusConvert.Ushorts2Bytes(datas);
                                                    byte dataValueUp = sourcebyte[0];
                                                    byte dataValueDown = sourcebyte[1];

                                                    if (ishigh)
                                                    {
                                                        dataValueUp = Convert.ToByte(ModbusConvert.Set_Int_bit(dataValueUp, dataposition - 1, WriteValue > 0 ? true : false));
                                                    }
                                                    else
                                                    {
                                                        dataValueDown = Convert.ToByte(ModbusConvert.Set_Int_bit(dataValueDown, dataposition - 1, WriteValue > 0 ? true : false));
                                                    }


                                                    sourcebyte[0] = dataValueUp;
                                                    sourcebyte[1] = dataValueDown;
                                                    datas = ModbusConvert.Bytes2Ushorts(sourcebyte);
                                                    ushort newValue = ModbusConvert.GetUShort(datas, 0);

                                                    master.WriteSingleRegister(byte.Parse(device.IO_DEVICE_ADDRESS), offset, newValue);
                                                }
                                                else
                                                {

                                                    ushort[] buff = new ushort[1];
                                                    ModbusConvert.SetShort(buff, 0, WriteValue);
                                                    master.WriteSingleRegister(byte.Parse(device.IO_DEVICE_ADDRESS), offset, buff[0]);
                                                }
                                            }
                                            break;
                                        case Modbus_Type.无符号整数32位:
                                            {
                                                uint WriteValue = uint.Parse(value);
                                                ushort[] buff = new ushort[2];
                                                ModbusConvert.SetUInt(buff, 0, WriteValue);
                                                master.WriteMultipleRegisters(byte.Parse(device.IO_DEVICE_ADDRESS), offset, buff);
                                            }
                                            break;
                                        case Modbus_Type.有符号整数32位:
                                            {
                                                int WriteValue = int.Parse(value);
                                                ushort[] buff = new ushort[2];
                                                ModbusConvert.SetInt(buff, 0, WriteValue);
                                                master.WriteMultipleRegisters(byte.Parse(device.IO_DEVICE_ADDRESS), offset, buff);
                                            }
                                            break;
                                    }

                                }
                                break;
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }


                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        protected override ScadaResult IOSendCommand(IO_SERVER server, IO_COMMUNICATION comm, IO_DEVICE device, IO_PARA para, string value)
        {
            ScadaResult res = new ScadaResult(false, "");
            if (device == null)
                return res;
            ModbusBusSerialObject modbusTcp = busTcpObjects.Find(x => x.Device.IO_DEVICE_ID == device.IO_DEVICE_ID);

            //用户发送一条命令后要及时获取最新的数据，主要是因为有些命令需要及时判断命令是否成功，
            if (modbusTcp != null && ResponseData(server, comm, device, para, value, modbusTcp))
            {

                res = new ScadaResult(true, "数据下置成功");

            }
            else
            {
                res = new ScadaResult(true, "数据下置失败");
            }
            DataSended(server, comm, device, para, value, res.Result);
            return res;
        }

        protected override Task Start()
        {
            return Task.Run(() =>
            {
                Parity parity = Parity.None;
                switch (Tcp_PARA.SerialCheck)
                {
                    case SerialCheck.无:
                        parity = Parity.None;
                        break;
                    case SerialCheck.偶校验:
                        parity = Parity.Even;
                        break;
                    case SerialCheck.奇校验:
                        parity = Parity.Odd;
                        break;
                    case SerialCheck.常0:
                        parity = Parity.Space;
                        break;
                    case SerialCheck.常1:
                        parity = Parity.Mark;
                        break;
                }

                SerialPort serialPort = new SerialPort(Tcp_PARA.SerialPort, Tcp_PARA.BaudRate, parity, Tcp_PARA.DataBits, Tcp_PARA.StopBits);
                serialPort.WriteBufferSize = Tcp_PARA.WriteBuffSize;
                serialPort.ReadBufferSize = Tcp_PARA.ReadBuffSize;
                serialPort.ReadTimeout = Tcp_PARA.ReadTimeout;
                serialPort.WriteTimeout = Tcp_PARA.WriteTimeout;
                serialPort.RtsEnable = Tcp_PARA.RTSEnable;
                serialPort.DtrEnable = Tcp_PARA.DTREnable;
                serialPort.Handshake = Tcp_PARA.Handshake;
                SerialPort = serialPort;
                //此处采用多线程技术创建一个实时读取数据的任务
                for (int i = 0; i < this.busTcpObjects.Count; i++)
                {

                    RealData mRealData = this.busTcpObjects[i].RealData;
                    if (string.IsNullOrWhiteSpace(mRealData.Device.IO_DEVICE_PARASTRING))
                        continue;
                    ModbusBusSerialObject modbusBusTcp = this.busTcpObjects[i];

                    try
                    {



                        if (modbusBusTcp.Para.ModbusType.Trim() == "RTU")
                        {
                            ModbusSerialMaster master = ModbusSerialMaster.CreateRtu(SerialPort);
                            master.Transport.Retries = modbusBusTcp.Para.CollectFaultsNumber;//重试次数
                            master.Transport.WaitToRetryMilliseconds = modbusBusTcp.Para.CollectFaultsInternal;//重试间隔     
                            modbusBusTcp.Master = master;
                        }
                        else
                        {
                            ModbusSerialMaster master = ModbusSerialMaster.CreateAscii(SerialPort);
                            master.Transport.Retries = modbusBusTcp.Para.CollectFaultsNumber;//重试次数
                            master.Transport.WaitToRetryMilliseconds = modbusBusTcp.Para.CollectFaultsInternal;//重试间隔     
                            modbusBusTcp.Master = master;
                        }



                    }
                    catch
                    {

                    }





                    //创建一个子任务
                    Task.Run(() =>
                    {
                        while (true && this.ServerIsRun)
                        {
                            try
                            {
                                if (!serialPort.IsOpen)
                                {
                                    serialPort.Open();
                                }
                                //发送获取数据的命令
                                string error = "";
                                if (!this.RequestData(mRealData.Device, modbusBusTcp, out error, mRealData.Fragment))
                                {
                                    this.CommunicationException("ERROR=Modbus_Serial_10001," + error);
                                }
                            }
                            catch (Exception e)
                            {
                                this.CommunicationException("ERROR=Modbus_Serial_10002," + e.Message);
                            }
                            Thread.Sleep(mRealData.Device.IO_DEVICE_UPDATECYCLE * 1000);
                            //更新周期

                        }
                    });
                }
                this.CommunctionStartChanged(this.IOServer, this.IOServer.SERVER_IP + " " + this.IOServer.SERVER_NAME + "启动服务");
            });
            }
        protected override Task Close()
        {
            return Task.Run(() =>
            {

                try
                {
                    for (int i = 0; i < this.busTcpObjects.Count; i++)
                    {
                        if (busTcpObjects[i].Master != null)
                        {

                            busTcpObjects[i].Master.Dispose();
                            busTcpObjects[i].Master = null;
                        }
                    }

                    if (SerialPort != null)
                    {
                        SerialPort.Close();
                        SerialPort.Dispose();
                        SerialPort = null;
                    }


                    this.CommunctionCloseChanged(this.IOServer, this.IOServer.SERVER_IP + " " + this.IOServer.SERVER_NAME + "关闭网络服务");
                }
                catch (Exception emx)
                {
                    this.CommunicationException("ERROR=Modbus_Tcp_10006," + emx.Message);
                    this.CommunctionStopChanged(this.IOServer, this.IOServer.SERVER_IP + " " + this.IOServer.SERVER_NAME + "关闭网络服务失败");
                }
            });
        }
        protected override void Continue()
        {
            try
            {

                this.CommunctionContinueChanged(this.IOServer, this.IOServer.SERVER_IP + " " + this.IOServer.SERVER_NAME + "继续网络服务");
            }
            catch (Exception emx)
            {
                this.CommunicationException("ERROR=Modbus_Tcp_10003," + emx.Message);
                this.CommunctionStopChanged(this.IOServer, this.IOServer.SERVER_IP + " " + this.IOServer.SERVER_NAME + "继续网络服务失败");
            }
        }
        protected override void Pause()
        {
            try
            {

                this.CommunctionPauseChanged(this.IOServer, this.IOServer.SERVER_IP + " " + this.IOServer.SERVER_NAME + "暂停网络服务");
            }
            catch (Exception emx)
            {
                this.CommunicationException("ERROR=Modbus_Tcp_10005," + emx.Message);
                this.CommunctionStopChanged(this.IOServer, this.IOServer.SERVER_IP + " " + this.IOServer.SERVER_NAME + "暂停网络服务失败");
            }
        }
        protected override Task Stop()
        {
            return Task.Run(() =>
            {
                try
                {


                    this.CommunctionStopChanged(this.IOServer, this.IOServer.SERVER_IP + " " + this.IOServer.SERVER_NAME + "停止网络服务");


                }
                catch (Exception emx)
                {
                    this.CommunicationException("ERROR=Modbus_Tcp_10003," + emx.Message);
                    this.CommunctionStopChanged(this.IOServer, this.IOServer.SERVER_IP + " " + this.IOServer.SERVER_NAME + "停止网络服务失败");
                }
            });

        }

        #region 模拟器模拟下位机
        List<ModbusSlave> slaves = new List<ModbusSlave>();
 
        private int SimulatorUpdateCycle = 5;//默认是秒

        //模拟器初始化
        public override void Simulator(IO_COMMUNICATION communication)
        {
            int times = 10;
            SimulatorClose();

            SimulatorUpdateCycle = times;
            Parity parity = Parity.None;
            switch (Tcp_PARA.SerialCheck)
            {
                case SerialCheck.无:
                    parity = Parity.None;
                    break;
                case SerialCheck.偶校验:
                    parity = Parity.Even;
                    break;
                case SerialCheck.奇校验:
                    parity = Parity.Odd;
                    break;
                case SerialCheck.常0:
                    parity = Parity.Space;
                    break;
                case SerialCheck.常1:
                    parity = Parity.Mark;
                    break;
            }

            SerialPort serialPort = new SerialPort(Tcp_PARA.SerialPort, Tcp_PARA.BaudRate, parity, Tcp_PARA.DataBits, Tcp_PARA.StopBits);
            serialPort.WriteBufferSize = Tcp_PARA.WriteBuffSize;
            serialPort.ReadBufferSize = Tcp_PARA.ReadBuffSize;
            serialPort.ReadTimeout = Tcp_PARA.ReadTimeout;
            serialPort.WriteTimeout = Tcp_PARA.WriteTimeout;
            serialPort.RtsEnable = Tcp_PARA.RTSEnable;
            serialPort.DtrEnable = Tcp_PARA.DTREnable;
            serialPort.Handshake = Tcp_PARA.Handshake;


            try
            {


                for (int i = 0; i < this.busTcpObjects.Count; i++)
                {

                    if (busTcpObjects[i].Para.ModbusType == "RTU")
                    {
                        try
                        {

                            ModbusSerialSlave DeviceSlave = ModbusSerialSlave.CreateRtu(byte.Parse(this.busTcpObjects[i].Device.IO_DEVICE_ADDRESS), serialPort);
                            DeviceSlave.Tag = this.IODevices[i];
                            slaves.Add(DeviceSlave);


                        }
                        catch (Exception emx)
                        {
                            this.SimulatorAppendLog("创建设备模拟器失败 " + this.IODevices[i].IO_DEVICE_NAME + " " + emx.Message);

                        }

                    }
                    else
                    {
                        try
                        {

                            ModbusSerialSlave DeviceSlave = ModbusSerialSlave.CreateAscii(byte.Parse(this.busTcpObjects[i].Device.IO_DEVICE_ADDRESS), serialPort);
                            DeviceSlave.Tag = this.IODevices[i];
                            slaves.Add(DeviceSlave);


                        }
                        catch (Exception emx)
                        {
                            this.SimulatorAppendLog("创建设备模拟器失败 " + this.IODevices[i].IO_DEVICE_NAME + " " + emx.Message);

                        }

                    }
                }

            }
            catch (Exception emx)
            {
                this.SimulatorAppendLog("创建设备模拟器失败   " + emx.Message);

            }

        }
        protected override Task SimulatorClose()
        {
            return Task.Run(() =>
            {
                try
                {

                    for (int i = 0; i < slaves.Count; i++)
                    {

                        slaves[i].Dispose();
                        slaves[i] = null;

                    }
                    slaves.Clear();



                }
                catch (Exception emx)
                {
                    this.SimulatorAppendLog("关闭模拟器失败 " + emx.Message);
                }
            });

        }
        protected override Task SimulatorStart()
        {
            return Task.Run(() =>
            {
                ParaPack paraPack = new ParaPack(this.IOCommunication.IO_COMM_SIMULATOR_PARASTRING);
                SimulatorUpdateCycle = Convert.ToInt32(paraPack.GetValue("数据更新周期"));
                int maxRegisterNum = Convert.ToInt32(paraPack.GetValue("寄存器数量"));
                for (int i = 0; i < slaves.Count; i++)
                {
                    try
                    {
                        slaves[i].ListenAsync();

                        slaves[i].ModbusSlaveRequestReceived += Modbus_Serial_ModbusSlaveRequestReceived;
                        slaves[i].WriteComplete += Modbus_Serial_WriteComplete;


                    }
                    catch (Exception emx)
                    {
                        this.SimulatorAppendLog("启动模拟器失败 " + emx.Message);
                    }
                }

                ///初始化从机

                Random rd = new Random();
                //初始化数据
                while (SimulatorStatus)
                {
                    //每10秒初始化数据一次
                    for (int i = 0; i < slaves.Count; i++)
                    {
                        IO_DEVICE device = slaves[i].Tag as IO_DEVICE;
                        ParaPack devPack = new ParaPack(device.IO_DEVICE_SIMULATOR_PARASTRING);
                        slaves[i].DataStore = DataStoreFactory.CreateTestDataStore(maxRegisterNum, Convert.ToUInt16(devPack.GetValue("模拟最大值")), Convert.ToUInt16(devPack.GetValue("模拟最小值")));
                        this.SimulatorAppendLog("设备" + device.IO_DEVICE_ID + "[" + device.IO_DEVICE_NAME + "]" + maxRegisterNum + "个寄存器地址初始化数据一次");
                    }

                    Thread.Sleep(SimulatorUpdateCycle * 1000);
                }


            });



        }

        /// <summary>
        /// 当从机收到写入命令的时候发生 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Modbus_Serial_WriteComplete(object sender, ModbusSlaveRequestEventArgs e)
        {
            this.SimulatorAppendLog("服务器请求从机要求写入数据");
        }

        /// <summary>
        /// 当从机收到数据请求时候发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Modbus_Serial_ModbusSlaveRequestReceived(object sender, ModbusSlaveRequestEventArgs e)
        {
            this.SimulatorAppendLog("服务器请求从机获取数据 ");
        }

        #endregion

        public override CommunicationKernelControl CreateCommunicationKernelControl()
        {
            return new Modbus_Serial_Ctrl();
        }
        public override SimulatorDeviceEditControl CreateSimulatorDeviceEdit()
        {
            return new Modbus_Serial_Simulator_Device();
        }
        public override SimulatorKernelControl CreateSimulatorKernelControl()
        {
            return new Modbus_Serial_Simulator();
        }
    }
}
