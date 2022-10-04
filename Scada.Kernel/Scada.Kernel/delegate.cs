using Scada.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Kernel
{
    /// </summary>
    /// <param name="msg"></param>
    public delegate void DeviceKernelError(IO_SERVER server, IO_COMMUNICATION comm, IO_DEVICE device, string msg);
    /// </summary>
    /// <param name="msg"></param>
    public delegate void CommunicationKernelError(IO_SERVER server, IO_COMMUNICATION comm, IO_DEVICE device, string msg);
    public delegate void KernelOutLog(string msg);
    public delegate void DataReceived(IO_SERVER server, IO_COMMUNICATION comm, IO_DEVICE device, byte[] receivedatas, string date, object sender = null);
    public delegate void KernelEvent(IO_SERVER server, IO_COMMUNICATION comm, List<IO_DEVICE> devices);
    public delegate void CommunicationEvent(IO_SERVER server, IO_COMMUNICATION comm, object tag);
    public delegate void DeviceSendedEvent(IO_SERVER server, IO_COMMUNICATION comm, IO_DEVICE device, IO_PARA para, string value, bool result);

}
