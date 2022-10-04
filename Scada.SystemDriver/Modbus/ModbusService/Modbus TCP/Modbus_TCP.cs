
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Scada.Model;
using System.Net.Sockets;
using Scada.Kernel;
using Modbus.Device;
using Modbus.Globel;
using System.Threading;
using Modbus.Data;

namespace Modbus.ModbusService
{

    public class Modbus_TCP_PARA
    {

        public int ReadTimeout = 1000;
        public int WriteTimeout = 1000;
        public int ReadBufferSize = 2048;
        public int WriteBufferSize = 2048;
        /// <summary>
        /// 本地端口
        /// </summary>
        public string LocalTCP_Port = "";
        /// <summary>
        /// 本地IP
        /// </summary>
        public string LocalTCP_IP = "";
        public ModbusTCP_CommunicateMode CommunicateMode = ModbusTCP_CommunicateMode.TCP;
        
   

    }
    public class Modbus_TCP_Device_PARA
    {


        
        /// <summary>
        /// 是否连续采集3次失败
        /// </summary>
        public bool ContinueCollect = false;
        public int CollectFaultsNumber = 3;
        public int CollectFaultsInternal = 15;
 
 

    }
    public enum ModbusTCP_CommunicateMode
    {
        TCP,
        UDP
    }

    public class ModbusBusTcpObject
    {
   
        public RealData RealData { set; get; }
        public IO_DEVICE Device { set; get; }

        public Modbus_TCP_Device_PARA Para { set; get; }
        public ModbusIpMaster Master { set; get; }

    }

    /// <summary>
    /// Modbus TCP 通讯
    /// </summary>
    public class Modbus_TCP : ScadaCommunicateKernel
    {
        private const string mGuid = "11880EA3-AC18-4A3B-B0F6-713C34B1CAFB";
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
        private string mTitle = " Modbus TCP";
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
        public Modbus_TCP()
        {
            this.AssignDeviceProtocols = new List<string>();
            this.AssignDeviceProtocols.Add("7D22423C-BC96-4771-AE32-7C00FD35B70E");
        }
        public TcpClient TcpClient { set; get; }
        public UdpClient UdpClient { set; get; }
       
        Modbus_TCP_PARA Tcp_PARA = new Modbus_TCP_PARA();
        List<ModbusBusTcpObject> busTcpObjects = null;
        protected override bool InitCommunicateKernel(IO_SERVER server, IO_COMMUNICATION communication, List<IO_DEVICE> ioDevices, SCADA_DRIVER driver)
        {
            this.AssignDeviceProtocols = new List<string>();
            this.AssignDeviceProtocols.Add("7D22423C-BC96-4771-AE32-7C00FD35B70E");
            if (IsCreateControl)
            {
                CommunicationControl = new Modbus_TCP_Ctrl();
                if (communication != null)
                    CommunicationControl.SetUIParameter(communication.IO_COMM_PARASTRING);

            }


            busTcpObjects = new List<ModbusBusTcpObject>();

            if (communication != null)
            {
                Tcp_PARA = new Modbus_TCP_PARA();
                ParaPack TcpParaPack = new ParaPack(communication.IO_COMM_PARASTRING);
             
                Tcp_PARA.LocalTCP_Port = TcpParaPack.GetValue("设备端口");
                Tcp_PARA.LocalTCP_IP = TcpParaPack.GetValue("设备IP");
                Tcp_PARA.WriteTimeout = int.Parse(TcpParaPack.GetValue("写超时时间"));
                Tcp_PARA.ReadTimeout = int.Parse(TcpParaPack.GetValue("读超时时间"));
                Tcp_PARA.WriteBufferSize = int.Parse(TcpParaPack.GetValue("写缓存"));
                Tcp_PARA.ReadBufferSize = int.Parse(TcpParaPack.GetValue("读缓存"));
                Tcp_PARA.CommunicateMode = TcpParaPack.GetValue("通讯协议") == "TCP/IP" ? ModbusTCP_CommunicateMode.TCP : ModbusTCP_CommunicateMode.UDP;


                //构造获取数据命令的字节数组,Modbus
                for (int i = 0; i < this.IODevices.Count; i++)
                {
                    ParaPack devicePack = new ParaPack(this.IODevices[i].IO_DEVICE_PARASTRING);
                    ModbusBusTcpObject modbusBusTcp = new ModbusBusTcpObject();
                    modbusBusTcp.Para = new Modbus_TCP_Device_PARA();
                    modbusBusTcp.Para.CollectFaultsInternal = Convert.ToInt32(devicePack.GetValue("重试间隔"));
                    modbusBusTcp.Para.CollectFaultsNumber = Convert.ToInt32(devicePack.GetValue("重试次数"));
                    modbusBusTcp.Para.ContinueCollect = devicePack.GetValue("重试") == "1" ? true : false;
      
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
        private bool RequestData(IO_DEVICE device, ModbusBusTcpObject rData, out string error, ModbusFragmentStore fragmentstore)
        {
            ModbusIpMaster master = rData.Master;
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
        private bool ResponseData(IO_SERVER server, IO_COMMUNICATION comm, IO_DEVICE device, IO_PARA para, string value, ModbusBusTcpObject rData)
        {
            ModbusIpMaster master = rData.Master;
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
            ModbusBusTcpObject modbusTcp = busTcpObjects.Find(x => x.Device.IO_DEVICE_ID == device.IO_DEVICE_ID);

            //用户发送一条命令后要及时获取最新的数据，主要是因为有些命令需要及时判断命令是否成功，
            if (modbusTcp!=null&&ResponseData(server, comm, device, para, value, modbusTcp))
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
          return   Task.Run(() => {
            if (Tcp_PARA.CommunicateMode == ModbusTCP_CommunicateMode.TCP)
            {
                TcpClient tcpClient = new TcpClient(AddressFamily.InterNetwork);
                tcpClient.ReceiveTimeout = Tcp_PARA.ReadTimeout;
                tcpClient.SendTimeout = Tcp_PARA.WriteTimeout;
                tcpClient.SendBufferSize = Tcp_PARA.WriteBufferSize;
                tcpClient.ReceiveBufferSize = Tcp_PARA.ReadBufferSize;
                TcpClient = tcpClient;
         

            }
            else
            {
                UdpClient tcpClient = new UdpClient(AddressFamily.InterNetwork);
                tcpClient.Connect(new IPEndPoint(IPAddress.Parse(Tcp_PARA.LocalTCP_IP), int.Parse(Tcp_PARA.LocalTCP_Port)));
                tcpClient.Client.ReceiveTimeout = Tcp_PARA.ReadTimeout;
                tcpClient.Client.SendTimeout = Tcp_PARA.WriteTimeout;
                tcpClient.Client.SendBufferSize = Tcp_PARA.WriteBufferSize;
                tcpClient.Client.ReceiveBufferSize = Tcp_PARA.ReadBufferSize;
                UdpClient = tcpClient;
              
            }

      

            //此处采用多线程技术创建一个实时读取数据的任务
            for (int i = 0; i < this.busTcpObjects.Count; i++)
            {
            
                RealData mRealData = this.busTcpObjects[i].RealData;
                if (string.IsNullOrWhiteSpace(mRealData.Device.IO_DEVICE_PARASTRING))
                    continue;
                ModbusBusTcpObject modbusBusTcp = this.busTcpObjects[i];
                if (Tcp_PARA.CommunicateMode == ModbusTCP_CommunicateMode.TCP)
                {
                    try
                    {

                        TcpClient tcpClient = new TcpClient(AddressFamily.InterNetwork);
                        tcpClient.ReceiveTimeout = Tcp_PARA.ReadTimeout;
                        tcpClient.SendTimeout = Tcp_PARA.WriteTimeout;
                        tcpClient.SendBufferSize = Tcp_PARA.WriteBufferSize;
                        tcpClient.ReceiveBufferSize = Tcp_PARA.ReadBufferSize;
                        TcpClient = tcpClient;

                        ModbusIpMaster master = ModbusIpMaster.CreateIp(tcpClient);
                        master.Transport.Retries = modbusBusTcp.Para.CollectFaultsNumber;//重试次数
                        master.Transport.WaitToRetryMilliseconds = modbusBusTcp.Para.CollectFaultsInternal;//重试间隔     
                        modbusBusTcp.Master = master;

                        tcpClient.Connect(IPAddress.Parse(Tcp_PARA.LocalTCP_IP), int.Parse(Tcp_PARA.LocalTCP_Port));
                    }
                    catch
                    {

                    }


                }
                else
                {
                    try
                    {
                        UdpClient udplient = new UdpClient(AddressFamily.InterNetwork);
                        udplient.Client.ReceiveTimeout = Tcp_PARA.ReadTimeout;
                        udplient.Client.SendTimeout = Tcp_PARA.WriteTimeout;
                        udplient.Client.SendBufferSize = Tcp_PARA.WriteBufferSize;
                        udplient.Client.ReceiveBufferSize = Tcp_PARA.ReadBufferSize;
                        UdpClient = udplient;
                        ModbusIpMaster master = ModbusIpMaster.CreateIp(udplient);
                        master.Transport.Retries = modbusBusTcp.Para.CollectFaultsNumber;//重试次数
                        master.Transport.WaitToRetryMilliseconds = modbusBusTcp.Para.CollectFaultsInternal;//重试间隔     
                        modbusBusTcp.Master = master;
                        UdpClient.Connect(IPAddress.Parse(Tcp_PARA.LocalTCP_IP), int.Parse(Tcp_PARA.LocalTCP_Port));
                    }
                    catch
                    {

                    }
                }



                //创建一个子任务
                Task.Run(() =>
                {
                    while (true && this.ServerIsRun)
                    {
                        try
                        {

                            if (Tcp_PARA.CommunicateMode == ModbusTCP_CommunicateMode.TCP)
                            {
                                if (!TcpClient.Connected)
                                {
                                    lock (TcpClient)
                                    {
                                        TcpClient.Close();
                                        TcpClient.Dispose();
                                        TcpClient = null;
                                        TcpClient tcpClient = new TcpClient(AddressFamily.InterNetwork);
                                        tcpClient.ReceiveTimeout = Tcp_PARA.ReadTimeout;
                                        tcpClient.SendTimeout = Tcp_PARA.WriteTimeout;
                                        tcpClient.SendBufferSize = Tcp_PARA.WriteBufferSize;
                                        tcpClient.ReceiveBufferSize = Tcp_PARA.ReadBufferSize;
                                        TcpClient = tcpClient;
                                        ModbusIpMaster master = ModbusIpMaster.CreateIp(tcpClient);
                                        master.Transport.Retries = modbusBusTcp.Para.CollectFaultsNumber;//重试次数
                                        master.Transport.WaitToRetryMilliseconds = modbusBusTcp.Para.CollectFaultsInternal;//重试间隔     
                                        modbusBusTcp.Master = master;
                                        tcpClient.Connect(IPAddress.Parse(Tcp_PARA.LocalTCP_IP), int.Parse(Tcp_PARA.LocalTCP_Port));
                                    }

                                }
                            }


                            //发送获取数据的命令
                            string error = "";
                            if (!this.RequestData(mRealData.Device, modbusBusTcp, out error, mRealData.Fragment))
                            {
                                this.CommunicationException("ERROR=Modbus_Tcp_10001," + error);
                            }
                        }
                        catch (Exception e)
                        {
                            this.CommunicationException("ERROR=Modbus_Tcp_10002," + e.Message);
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
            return Task.Run(() => {
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

                if (TcpClient != null)
                {
                    TcpClient.Close();
                    TcpClient.Dispose();
                    TcpClient = null;
                }

                if (UdpClient != null)
                {
                    UdpClient.Close();
                    UdpClient.Dispose();
                    UdpClient = null;
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
        TcpListener tcpListener = null;
        UdpClient udpServer = null;
        private int SimulatorUpdateCycle = 5;//默认是秒

        //模拟器初始化
        public override void Simulator(IO_COMMUNICATION communication)
        {
            int times = 10;
            SimulatorClose();

            SimulatorUpdateCycle = times;


            if (Tcp_PARA.CommunicateMode == ModbusTCP_CommunicateMode.TCP)
            {
                try
                {
                    tcpListener = new TcpListener(new IPEndPoint(IPAddress.Parse(Tcp_PARA.LocalTCP_IP), int.Parse(Tcp_PARA.LocalTCP_Port)));



                    for (int i = 0; i < this.busTcpObjects.Count; i++)
                    {

                        try
                        {

                            ModbusSlave DeviceSlave = ModbusTcpSlave.CreateTcp(byte.Parse(this.busTcpObjects[i].Device.IO_DEVICE_ADDRESS), tcpListener);
                            DeviceSlave.Tag = this.IODevices[i];
                            slaves.Add(DeviceSlave);


                        }
                        catch (Exception emx)
                        {
                            this.SimulatorAppendLog("创建设备模拟器失败 " + this.IODevices[i].IO_DEVICE_NAME + " " + emx.Message);

                        }
                    }

                }
                catch (Exception emx)
                {
                    this.SimulatorAppendLog("创建设备模拟器失败   " + emx.Message);

                }
            }
            else
            {
                udpServer = new UdpClient(new IPEndPoint(IPAddress.Parse(Tcp_PARA.LocalTCP_IP), int.Parse(Tcp_PARA.LocalTCP_Port)));
                for (int i = 0; i < this.busTcpObjects.Count; i++)
                {

                    try
                    {
                        udpServer = new UdpClient(new IPEndPoint(IPAddress.Parse(Tcp_PARA.LocalTCP_IP), int.Parse(Tcp_PARA.LocalTCP_Port)));
                        ModbusSlave DeviceSlave = ModbusUdpSlave.CreateUdp(byte.Parse(this.busTcpObjects[i].Device.IO_DEVICE_ADDRESS), udpServer);
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
                    if (tcpListener != null)
                    {
                        tcpListener.Stop();
                        tcpListener = null;
                    }

                    if (udpServer != null)
                    {
                        udpServer.Close();
                        udpServer.Dispose();
                        udpServer = null;
                    }


                }
                catch (Exception emx)
                {
                    this.SimulatorAppendLog("关闭模拟器失败 " + emx.Message);
                }
            });

        }
        protected override   Task SimulatorStart()
        { return Task.Run(() =>
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
                        slaves[i].DataStore = DataStoreFactory.CreateTestDataStore(maxRegisterNum,Convert.ToUInt16(devPack.GetValue("模拟最大值")),Convert.ToUInt16(devPack.GetValue("模拟最小值")));
                        this.SimulatorAppendLog("设备"+ device .IO_DEVICE_ID+"["+ device .IO_DEVICE_NAME+ "]"+ maxRegisterNum + "个寄存器地址初始化数据一次");
                    }
                  
                    Thread.Sleep(SimulatorUpdateCycle *1000);
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
            return new Modbus_TCP_Ctrl();
        }
        public override SimulatorDeviceEditControl CreateSimulatorDeviceEdit()
        {
            return new Modbus_TCP_Simulator_Device();
        }
        public override SimulatorKernelControl CreateSimulatorKernelControl()
        {
            return new Modbus_TCP_Simulator();
        }
    }
}
