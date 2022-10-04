using Scada.DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ScadaWeb.Web.Controllers
{
    public class ReceiveController : Controller
    {
        WebRealCache mWebRealCache = new WebRealCache();
           // GET: 后期增加数据中心授权模式，方式用户任意增加数据
           [HttpPost]
        public void  Real(List<RealWebCacheDataItem> models)
        {
            if (models == null || models.Count <= 0)
                return;
            mWebRealCache.InsertReal(models);
           
        }
        // GET: Receive
        [HttpPost]
        public void Alarm(List<AlarmWebCacheDataItem> models)
        {
            if (models == null || models.Count <= 0)
                return;
            mWebRealCache.InsertAlarm(models);

        }
        [HttpPost]
        public void Status(List<StatusWebCacheDataItem> models)
        {
            if (models == null || models.Count <= 0)
                return;
            mWebRealCache.InsertStatus(models);

        }
    }
}