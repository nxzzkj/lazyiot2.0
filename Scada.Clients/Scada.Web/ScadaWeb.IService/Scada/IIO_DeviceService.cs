﻿using ScadaWeb.Model;
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaWeb.IService
{
    public interface IIO_DeviceService : IBaseService<IODeviceModel>
    {
        IEnumerable<IODeviceModel> GetListObjectByFilter(IODeviceModel filter, PageInfo pageInfo, out long total);
    }
}
