 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScadaWeb.Model;
using ScadaWeb.IService;

namespace ScadaWeb.Service
{
  public  class IO_DeviceService : BaseService<IODeviceModel>,IIO_DeviceService
    {
        public IO_DeviceService()
        {

        }
     
        public dynamic GetAll()
        {
            return base.GetAll(null, null);
        }

        public dynamic GetListByFilter(IODeviceModel filter, PageInfo pageInfo)
        {
            return null;
        }

        public IEnumerable<IODeviceModel> GetListObjectByFilter(IODeviceModel filter, PageInfo pageInfo, out long total)
        {
            pageInfo.field = " IO_DEVICE_ID ";
            pageInfo.order = "   asc ";
            pageInfo.prefix = " ";
            string where = "  IO_DEVICE where IO_SERVER_ID='" + filter.IO_SERVER_ID + "' and IO_COMM_ID='" + filter.IO_COMM_ID + "'  ";
            if (!string.IsNullOrEmpty(filter.IO_DEVICE_NAME))
            {
                where += " and   (IO_DEVICE_LABLE like '%" + filter.IO_DEVICE_NAME + "%' or IO_DEVICE_NAME like '%" + filter.IO_DEVICE_NAME + "%' ) ";
            }
            pageInfo.returnFields = string.Format(" * ", pageInfo.prefix);
            return (IEnumerable<IODeviceModel>)base.GetPageOjectsUnite(filter, pageInfo, out total, where);
        }
     
    }
}
