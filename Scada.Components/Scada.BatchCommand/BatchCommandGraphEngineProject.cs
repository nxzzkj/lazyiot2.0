using System.Collections.Generic;

namespace Scada.BatchCommand
{
    public abstract class BatchCommandGraphEngineProject
    {
        //当前服务器加载的采集站
        public static Scada.Model.IO_SERVER IOServer = null;
        //当前服务器加载的通道
        public static List<Scada.Model.IO_COMMUNICATION> IOCommunications = null;
     
      
    }
}
