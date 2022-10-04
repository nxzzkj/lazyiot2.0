using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ScadaWeb.Common;
using ScadaWeb.IService;
using ScadaWeb.Model;
using ScadaWeb.Web.Controllers;
using Temporal.WebDbAPI;
using Temporal.Net.InfluxDb.Models.Responses;
using System.Collections;
using ScadaWeb.Web.Areas.Scada.Models;
using System.Reflection;
using ScadaWeb.Service;
using System.Dynamic;
using Newtonsoft.Json;
using Scada.DBUtility;
using ScadaWeb.Web.Models;

namespace ScadaWeb.Web.Areas.Scada.Controllers
{
    /// <summary>
    /// 通用SCADA系统系统的控制模块
    /// </summary>
    public class ScadaGeneralController: BaseController
    {
        public WebInfluxDbManager mWebInfluxDbManager = new WebInfluxDbManager();


  
        public IIO_ServerService IO_Server { get; set; }
        public IIO_CommunicateService IO_CommunicateServer { get; set; }
       
        public IIO_DeviceService IO_DeviceServer { get; set; }
  
    
        public IIO_ParaService ParaService { set; get; }

        #region 获取站点信息
        [HttpGet]
        public JsonResult GetStations()
        {
            var stations = IO_Server.GetAll();
            var result = new { code = 0, count = stations.Count(), data = stations };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetCommunications(string serverid)
        {
            var communications = IO_CommunicateServer.GetByWhere(" where IO_SERVER_ID='" + serverid + "'");
            var result = new { code = 0, count = communications.Count(), data = communications };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetDevices(string serverid, string communicationid)
        {
            var devices = IO_DeviceServer.GetByWhere(" where IO_SERVER_ID='" + serverid + "' and IO_COMM_ID='" + communicationid + "'");
            var result = new { code = 0, count = devices.Count(), data = devices };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetParas(string serverid, string communicationid, string deviceid)
        {
            var paras = ParaService.GetByWhere(" where IO_SERVER_ID='" + serverid + "' and IO_COMM_ID='" + communicationid + "' and IO_DEVICE_ID='" + deviceid + "'");
            List<ParaSelectItem> Items = new List<ParaSelectItem>();
            foreach (var item in paras)
            {
                Items.Add(new ParaSelectItem
                {
                    title =String.IsNullOrEmpty(item.IO_LABEL)? item.IO_NAME: item.IO_LABEL,
                    value = item.IO_NAME,
                    id = item.IO_ID

                });
            }
            var result = new { code = 0, count = Items.Count(), data = Items };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetParasXmSelect(string serverid, string communicationid, string deviceid)
        {

            var paras = ParaService.GetByWhere(" where IO_SERVER_ID='" + serverid + "' and IO_COMM_ID='" + communicationid + "' and IO_DEVICE_ID='" + deviceid + "'");
            List<SerieOption> _select = new List<SerieOption>();
            List<string> myios = new List<string>();

            if (paras != null && paras.Count() > 0)
            {
                int sindex = 0;
                foreach (var item in paras)
                {
                   
                    SerieOption _option = new SerieOption
                    {
                        id = item.IO_NAME.ToString(),
                        name = item.IO_NAME,
                        value = item.IO_NAME,
                        SerieConfig = new SerieConfigModel
                        {
                            SerieName = item.IO_NAME,
                            SerieClassify = item.IO_DEVICE_ID,
                            SerieTitle = String.IsNullOrEmpty(item.IO_LABEL) ? item.IO_NAME : item.IO_LABEL,
                             Id= sindex
                        }
                    };
                    _select.Add(_option);
                }
            }

            return Json(_select, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public override ActionResult Index(long? id)
        {
            base.Index(id);
            GeneralHistoryModel model = new GeneralHistoryModel();
            return View(model);
        }
        /// <summary>
        /// 通用历史数据查询模块
        /// </summary>
        /// <returns></returns>
        public ActionResult GeneralHistory()
        {
           
            GeneralHistoryModel model = new GeneralHistoryModel();
            return View(model);
        }
     
   
        [HttpGet]
        public JsonResult GeneralGridHistory(GeneralHistoryModel model, PageInfo pageInfo)
        {
            try
            {
         
                List<IOParaModel> iOParas = this.ParaService.GetByWhere(" where IO_SERVER_ID='" + model.ServerID + "' and IO_COMM_ID='" + model.CommunicateID + "' and IO_DEVICE_ID='" + model.DeviceID + "'").ToList();

                List<string> columns = new List<string>();
                foreach (IOParaModel para in iOParas)
                {
                    columns.Add(para.IO_NAME);
                }

                if (!string.IsNullOrWhiteSpace(model.DeviceID))
                {
                    string items = "[";
                    string sdate = model.StartDate;
                    string edate = model.EndDate;
                    InfluxDBHistoryResult realResult = mWebInfluxDbManager.DbQuery_History(model.ServerID, model.CommunicateID, model.DeviceID, Convert.ToDateTime(sdate), Convert.ToDateTime(edate), pageInfo.limit, pageInfo.page, " DESC ");
                    foreach (var s in realResult.Seres)
                    {

                        List<int> indexs = new List<int>();
                        for (int i = 0; i < s.Values.Count; i++)
                        {
                            string jsonrow = "";
                            int index = s.Columns.IndexOf("time");
                            object time = s.Values[i][index];
                            jsonrow += "{";
                            jsonrow += "\"DateStampTime\":\"" + (time != null ? time.ToString() : "") + "\"";

                            foreach (string str in columns)
                            {
                                try
                                {

                                    index = -1;
                                    index = s.Columns.IndexOf("field_" + str.Trim().ToLower().ToString() + "_value");
                                    if (index >= 0)
                                    {
                                        object v = s.Values[i][index];

                                        jsonrow += ",\"" + str + "\":\"" + (v != null ? v.ToString() : "") + "\"";
                                    }

                                }
                                catch
                                {
                                    continue;
                                }
                            }
                            jsonrow += "},";

                            items += jsonrow;

                        }
                    }
                    items += "]";
                    var result = Pager.Paging2(items, realResult.RecordCount);
                    //读取以下的实时数据，从influxDB中读取
                    return Json(result, "application/text", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = Pager.Paging2("[]", 0);
                    return Json(result, "application/text", JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                var result = Pager.Paging2("[]", 0);
                return Json(result, "application/text", JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 通用历史数据查询模块
        /// </summary>
        /// <returns></returns>
        public ActionResult GeneralHistorySummary()
        {

            GeneralHistorySummaryModel model = new GeneralHistorySummaryModel();
            return View(model);
        }
        /// <summary>
        /// 通用实时曲线页面
        /// </summary>
        /// <returns></returns>
        public ActionResult GeneralRealSeries()
        {
            GeneralRealModel model = new GeneralRealModel();


            return View(model);
        }
    
        [HttpGet]
        public JsonResult GeneralGridHistorySummary(GeneralHistorySummaryModel model, PageInfo pageInfo)
        {
            try
            {
              

                if (!string.IsNullOrWhiteSpace(model.DeviceID))
                {
                    string items = "[";
                    List<IOParaModel> iOParas = this.ParaService.GetByWhere(" where IO_SERVER_ID='" + model.ServerID + "' and IO_COMM_ID='" + model.CommunicateID + "' and IO_DEVICE_ID='" + model.DeviceID + "'").ToList();

                    List<string> columns = new List<string>();
                    foreach (IOParaModel para in iOParas)
                    {
                        columns.Add(para.IO_NAME);
                    }
                    string sdate = model.StartDate;
                    string edate = model.EndDate;

                    string returnFields = "  time";
                    #region
                    {
                        for (int i = 0; i < columns.Count; i++)
                        {
                            var para = iOParas.Find(x => x.IO_NAME.Trim().ToLower() == columns[i].Trim().ToLower());
                            if (para != null && IsNumberIOPoint(para))
                                returnFields += "," + model.Method + "(field_" + columns[i].Trim().ToLower().ToString() + "_value) as field_" + columns[i].Trim().ToLower().ToString() + "_value";
                            else
                                returnFields += ", MEAN(null)  as field_" + columns[i].Trim().ToLower().ToString() + "_value";
                        }


                    }
                    #endregion
                    InfluxDBHistoryResult realResult = mWebInfluxDbManager.DbQuery_HistoryStatics(model.ServerID, model.CommunicateID, model.DeviceID, Convert.ToDateTime(sdate), Convert.ToDateTime(edate), pageInfo.limit, pageInfo.page, " DESC ", model.Period, returnFields);
                    foreach (var s in realResult.Seres)
                    {

                        List<int> indexs = new List<int>();
                        for (int i = 0; i < s.Values.Count; i++)
                        {
                            string jsonrow = "";
                            int index = s.Columns.IndexOf("time");
                            object time = s.Values[i][index];
                            jsonrow += "{";
                            jsonrow += "\"DateStampTime\":\"" + (time != null ? time.ToString() : "") + "\"";

                            foreach (string str in columns)
                            {
                                try
                                {

                                    index = -1;
                                    index = s.Columns.IndexOf("field_" + str.Trim().ToLower().ToString() + "_value");
                                    if (index >= 0)
                                    {
                                        object v = s.Values[i][index];

                                        jsonrow += ",\"" + str + "\":\"" + (v != null ? v.ToString() : "") + "\"";
                                    }

                                }
                                catch
                                {
                                    continue;
                                }
                            }
                            jsonrow += "},";

                            items += jsonrow;

                        }
                    }
                    items += "]";
                    var result = Pager.Paging2(items, realResult.RecordCount);
                    //读取以下的实时数据，从influxDB中读取
                    return Json(result, "application/text", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = Pager.Paging2("[]", 0);
                    return Json(result, "application/text", JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                var result = Pager.Paging2("[]", 0);
                return Json(result, "application/text", JsonRequestBehavior.AllowGet);

            }
        }
        /// <summary>
        /// 通用历史曲线查询模块
        /// </summary>
        /// <returns></returns>
        public ActionResult GeneralHistorySeries()
        {
            GeneralHistoryModel model = new GeneralHistoryModel();
            return View(model);
        }
        public ActionResult GeneralHistorySummarySeries()
        {
            GeneralHistoryModel model = new GeneralHistoryModel();
            return View(model);
        }
        
        /// <summary>
        /// 用户自由选择任意井进行专门的历史曲线查询模块的实际查询数据的功能
        /// </summary>
        /// <param name="wellid"></param>
        /// <param name="series"></param>
        /// <param name="charttype"></param>
        /// <param name="sdate"></param>
        /// <param name="edate"></param>
        /// <param name="serieclassify"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult QueryHistorySeriesData(string serverid,string communicateid,string deviceid, string series = "", int charttype = 1, string sdate = "", string edate = "", int pagesize = 1000)
        {
            return QueryHistoryData(serverid, communicateid, deviceid, series, charttype, sdate, edate, pagesize);
        }
      
        private JsonResult QueryHistoryData(string serverid, string communicateid, string deviceid,  string series = "", int charttype = 1, string sdate = "", string edate = "",  int pagesize = 1000)
        {
            string[] myfields = series.Split(',');
            ///传递的曲线指标信息
            string serieindex = "'" + series.Trim().Replace(",", "','") + "'";
            //获取对应的曲线配置信息
            List<IOParaModel> iOParas = this.ParaService.GetByWhere(" where IO_SERVER_ID='" + serverid + "' and IO_COMM_ID='" + communicateid + "' and IO_DEVICE_ID='" + deviceid + "'").ToList();

            List<SerieConfigModel> Items = new List<SerieConfigModel>();
            int sindex = 0;
            foreach (var item in myfields)
            {
                sindex++;
                Items.Add(new SerieConfigModel()
                {
                    CreateTime = DateTime.Now,
                    CreateUserId = 1,
                    UpdateTime = DateTime.Now,
                    Id = sindex,
                    SerieClassify = deviceid,
                    SerieName = item,
                    SerieTitle = iOParas.Find(x => x.IO_NAME.Trim().ToLower() == item.Trim().ToLower()).IO_LABEL,
                    SerieType="line",
                      SerieColor= WebSerieColor.GetColor(sindex),
                       SymbolColor = WebSerieColor.GetColor(sindex)



                });
            }
          
            ///初始化曲线对象
            EChartOption chartOption = new EChartOption();
            chartOption.xAxis = new Axis[1];
            chartOption.xAxis[0] = new Axis();
            chartOption.xAxis[0].gridIndex = 1;
            chartOption.xAxis[0].type = "time";
            chartOption.xAxis[0].name = "时间";
            List<Axis> yaxis = new List<Axis>();
            int index = 0;
            string[] legend = new string[Items.Count()];

            foreach (var item in Items)
            {
                yaxis.Add(new Axis() { gridIndex = index, name = item.SerieTitle, type = "value" });
                legend[index] = item.SerieTitle;
                index++;
            }
            chartOption.legend.data = legend;
            chartOption.yAxis = yaxis.ToArray();
            //初始化对象结束
            if (sdate == null || sdate == "")
                sdate = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
            if (edate == null || edate == "")
                edate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            chartOption.series = new Series[Items.Count()];
            sindex = 0;
            foreach (var item in Items)
            {
                chartOption.series[sindex] = new Series();
                chartOption.series[sindex].id = item.SerieName;
                chartOption.series[sindex].name = item.SerieTitle;
                chartOption.series[sindex].lineStyle.color = item.SerieColor;
                chartOption.series[sindex].lineStyle.width = int.Parse(item.SerieWidth);
            
                chartOption.series[sindex].data = new double[0];
                chartOption.series[sindex].type = item.SerieType;
                chartOption.series[sindex].showSymbol = item.ShowSymbol == "1" ? true : false;
                chartOption.series[sindex].symbol = item.SymbolType;
                chartOption.series[sindex].symbolSize = int.Parse(item.SymbolSize);
             
                sindex++;
            }
            InfluxDBHistoryResult realResult = mWebInfluxDbManager.DbQuery_History(serverid, communicateid, deviceid, Convert.ToDateTime(sdate), Convert.ToDateTime(edate), 10000, 1, " ASC ");
            if (realResult != null)
            {

                var datas = realResult.Seres;
                if (datas != null && datas.Count() > 0)
                {
                    var onedata = datas.First();
                    sindex = 0;
                    foreach (var item in Items)
                    {
                        chartOption.series[sindex] = new Series();

                        chartOption.series[sindex].name = item.SerieTitle;
                        chartOption.series[sindex].lineStyle.color = item.SerieColor;
                        chartOption.series[sindex].lineStyle.width = int.Parse(item.SerieWidth);
              
                        chartOption.series[sindex].data = new double[onedata.Values.Count];
                        chartOption.series[sindex].type = item.SerieType;
                        chartOption.series[sindex].showSymbol = item.ShowSymbol == "1" ? true : false;
                        chartOption.series[sindex].symbol = item.SymbolType;
                        chartOption.series[sindex].symbolSize = int.Parse(item.SymbolSize);
                        sindex++;
                    }
                    string[] axisData = new string[onedata.Values.Count];
                    //获取的数据按照时间先后
                    int dataindex = onedata.Values.Count() - 1;

                    foreach (var value in onedata.Values)
                    {
                        //获取采集时间
                        object objx = onedata.Values[dataindex][onedata.Columns.IndexOf("time")];
                        axisData[dataindex] = objx != null ? objx.ToString() : "";
                        //////////////////////////
                        sindex = 0;
                        foreach (var item in Items)
                        {

                            try
                            {


                             
                                int recordindex = onedata.Columns.IndexOf("field_" + item.SerieName.Trim().ToLower() + "_value");
                                if (recordindex >= 0)
                                {
                                    object objy = onedata.Values[dataindex][recordindex];
                                    chartOption.series[sindex].data[dataindex] = Convert.ToDouble(objy);
                                    chartOption.series[sindex].id = item.SerieName;
                                }


                            }
                            catch
                            {

                            }

                            sindex++;
                        }
                        dataindex--;
                    }
                    chartOption.xAxis[0].data = axisData;//设置x轴数据，time格式的数据必须在Axis轴上进行设置


                }


            }

            //读取以下的实时数据，从influxDB中读取
            return Json(chartOption, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult QueryHistorySummarySeriesData( string serverid, string communicateid, string deviceid, string period, string method,string series = "", int charttype = 1, string sdate = "", string edate = "", int pagesize = 1000)
        {
 

            return QueryHistorySummaryData(serverid, communicateid, deviceid, period, method, series, charttype, sdate, edate, pagesize);
        }
        private JsonResult QueryHistorySummaryData(string serverid, string communicateid, string deviceid, string period, string method, string series = "", int charttype = 1, string sdate = "", string edate = "",int pagesize = 1000)
        {
            ///传递的曲线指标信息
            string serieindex = "'" + series.Trim().Replace(",", "','") + "'";
            string[] myfields = series.Split(',');
            //获取对应的曲线配置信息
            List<IOParaModel> iOParas = this.ParaService.GetByWhere(" where IO_SERVER_ID='" + serverid + "' and IO_COMM_ID='" + communicateid + "' and IO_DEVICE_ID='" + deviceid + "'").ToList();

            List<SerieConfigModel> Items = new List<SerieConfigModel>();
            int sindex = 0;
            foreach (var item in myfields)
            {
                var finder = iOParas.Find(x => x.IO_NAME.Trim().ToLower() == item.Trim().ToLower());
                sindex++;
                Items.Add(new SerieConfigModel()
                {
                    CreateTime = DateTime.Now,
                    CreateUserId = 1,
                    UpdateTime = DateTime.Now,
                    Id = sindex,
                    SerieClassify = deviceid,
                    SerieName = item,
                    SerieTitle = String.IsNullOrEmpty(finder.IO_LABEL)? finder.IO_NAME: finder.IO_LABEL,
                    SerieType = "line",
                    SerieColor = WebSerieColor.GetColor(sindex),
                    SymbolColor = WebSerieColor.GetColor(sindex)
               



                });
            }


            ///初始化曲线对象
            EChartOption chartOption = new EChartOption();
            chartOption.xAxis = new Axis[1];
            chartOption.xAxis[0] = new Axis();
            chartOption.xAxis[0].gridIndex = 1;
            chartOption.xAxis[0].type = "time";
            chartOption.xAxis[0].name = "时间";
            List<Axis> yaxis = new List<Axis>();
            int index = 0;
            string[] legend = new string[Items.Count()];

            foreach (var item in Items)
            {
                yaxis.Add(new Axis() { gridIndex = index, name = item.SerieTitle, type = "value" });
                legend[index] = item.SerieTitle;
                index++;
            }
            chartOption.legend.data = legend;
            chartOption.yAxis = yaxis.ToArray();
            //初始化对象结束
            if (sdate == null || sdate == "")
                sdate = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
            if (edate == null || edate == "")
                edate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            chartOption.series = new Series[Items.Count()];
            string returnFields = "  time";
            #region
            {
              
                foreach (var item in Items)
                {
                    var para = iOParas.Find(x => x.IO_NAME.Trim().ToLower() == item.SerieName.Trim().ToLower().ToString());
                    if(para!=null&&IsNumberIOPoint(para))
                    {
                        returnFields += "," + method + "(field_" + item.SerieName.Trim().ToLower().ToString() + "_value) as field_" + item.SerieName.Trim().ToLower().ToString() + "_value";
                    }
                    else
                    {
                        returnFields += ",mean(null) as field_" + item.SerieName.Trim().ToLower().ToString() + "_value";
                    }
                   
                }
             
                
            }
            #endregion
            InfluxDBHistoryResult realResult = mWebInfluxDbManager.DbQuery_HistoryStatics(serverid, communicateid, deviceid, Convert.ToDateTime(sdate), Convert.ToDateTime(edate), pagesize, 1, " DESC ", period, returnFields);
            if (realResult != null)
            {

                var datas = realResult.Seres;
                if (datas != null && datas.Count() > 0)
                {
                    var onedata = datas.First();
                     sindex = 0;
                    foreach (var item in Items)
                    {
                        chartOption.series[sindex] = new Series();
                        chartOption.series[sindex].id = item.SerieName;
                        chartOption.series[sindex].name = item.SerieTitle;
                        chartOption.series[sindex].lineStyle.color = item.SerieColor;
                        chartOption.series[sindex].lineStyle.width = int.Parse(item.SerieWidth);
                    
                        chartOption.series[sindex].data = new double[onedata.Values.Count];
                        chartOption.series[sindex].type = item.SerieType;
                        chartOption.series[sindex].showSymbol = item.ShowSymbol == "1" ? true : false;
                        chartOption.series[sindex].symbol = item.SymbolType;
                        chartOption.series[sindex].symbolSize = int.Parse(item.SymbolSize);
                        sindex++;
                    }
                    string[] axisData = new string[onedata.Values.Count];
                    //获取的数据按照时间先后
                    int dataindex = onedata.Values.Count() - 1;

                    foreach (var value in onedata.Values)
                    {
                        //获取采集时间
                        object objx = onedata.Values[dataindex][onedata.Columns.IndexOf("time")];
                        axisData[dataindex] = objx != null ? objx.ToString() : "";
                        //////////////////////////
                        sindex = 0;
                        foreach (var item in Items)
                        {

                            try
                            {


                         
                                string record = item.SerieName.ToString().ToLower();
                                int recordindex = onedata.Columns.IndexOf("field_" + record + "_value");
                                if (recordindex >= 0)
                                {
                                    object objy = onedata.Values[dataindex][recordindex];
                                    chartOption.series[sindex].data[dataindex] = Convert.ToDouble(objy);
                                    chartOption.series[sindex].id = item.SerieName;
                                }


                            }
                            catch 
                            {

                            }

                            sindex++;
                        }
                        dataindex--;
                    }
                    chartOption.xAxis[0].data = axisData;//设置x轴数据，time格式的数据必须在Axis轴上进行设置


                }


            }

            //读取以下的实时数据，从influxDB中读取
            return Json(chartOption, JsonRequestBehavior.AllowGet);
        }
      
      public sealed class PointData
        {
            public string Value = "";
            public string Name = "";
 
            public int Status = 0;
        } 
        public sealed class RealSerieData
        {
            public string Date = "";
   
            public int Status = 0;
            public List<PointData> Data = new List<PointData>();
        }
        public WebRealCache mWebRealCache = new WebRealCache();
        /// <summary>
        /// 获取曲线实时数据
        /// </summary>
        /// <param name="wellId"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult QueryRealSeriesData( string serverid, string communicateid, string deviceid,string series)
        {
            var realItem = mWebRealCache.GetReal(new WebCacheDataItem() { ServerId= serverid, DeviceId= deviceid, CommunicationId= communicateid });
 
            string[] fs = series.Split(',');
            RealSerieData resDatas =new  RealSerieData();
            List<IOParaModel> iOParas = this.ParaService.GetByWhere(" where IO_SERVER_ID='" + serverid + "' and IO_COMM_ID='" + communicateid + "' and IO_DEVICE_ID='" + deviceid + "'").ToList();

            if (realItem != null)
            { 
                resDatas.Date = realItem.Date;
                realItem.ParaReals.ForEach(delegate (WebCacheIOData iodata)
                {

                    PointData rdata = new PointData();

                    rdata.Status = 1;
                    rdata.Value = string.IsNullOrEmpty(iodata.ParaValue) ? "-9999" : iodata.ParaValue;
                    rdata.Name = iodata.ParaName;
                    if (rdata.Value == "-9999")
                    {
                        rdata.Status = 0;
                    }
                 if(fs.Contains(iodata.ParaName.Trim()))
                    {
                        resDatas.Data.Add(rdata);
                    }
                
                });


            }


            //读取以下的实时数据，从influxDB中读取
            return Json(resDatas, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GeneralEquipments(int Id)
        {
          
            return View();

        }
        [HttpGet]
        public JsonResult EquipmentList(string key, string serverid, string communicateid, int pageindex, int pagesize)
        {
        
            var paras = ParaService.GetAll();
            var communications = IO_CommunicateServer.GetAll();
            var stations = IO_Server.GetAll();
          
            IODeviceModel filter = new IODeviceModel();

            filter.IO_SERVER_ID = serverid;
            filter.IO_COMM_ID = communicateid;
            filter.IO_DEVICE_LABLE = key;
            filter.IO_DEVICE_NAME = key;
            PageInfo page = new PageInfo();
            page.prefix = "";
            page.limit = pagesize;
            page.page = pageindex;
            long total = 0;
            var devices = IO_DeviceServer.GetListObjectByFilter(filter, page,out total);
            List<ScadaEquipmentModel> equipments = new List<ScadaEquipmentModel>();
            foreach (var item in devices)
            {
                var subparas = paras.Where(x => x.IO_SERVER_ID == item.IO_SERVER_ID && x.IO_COMM_ID == item.IO_COMM_ID && x.IO_DEVICE_ID == item.IO_DEVICE_ID).ToList();
                var server = stations.First(x => x.SERVER_ID == item.IO_SERVER_ID);
                var communication = communications.First(x => x.IO_SERVER_ID == item.IO_SERVER_ID && x.IO_COMM_ID == item.IO_COMM_ID);
                var device = item;

                ScadaEquipmentModel scadaEquipment = new ScadaEquipmentModel
                {
                    ServerName = server != null ? server.SERVER_NAME + "[" + server.SERVER_ID + "]" : "采集站缺失",
                    CommunicationName = communication != null ? communication.IO_COMM_LABEL + "[" + communication.IO_COMM_NAME + "]" : "通道缺失",
                    DeviceName = device != null ? device.IO_DEVICE_LABLE + "[" + device.IO_DEVICE_NAME + "]" : "设备缺失",
                    ScadaGroup = new ScadaGroupModel(),
                    DeviceId = item.IO_DEVICE_ID,
                    CommunicationId = item.IO_COMM_ID,
                    Id = 0,
                    GroupId = 0,
                    IODevice = item,
                    ModelTitle = device != null ? device.IO_DEVICE_LABLE + "[" + device.IO_DEVICE_NAME + "]" : "设备缺失",

                    Remark = "",
                    ServerId = item.IO_SERVER_ID,
                    UpdateTime = DateTime.Now,
                    UpdateUserId = 0,
                    CreateTime = DateTime.Now,
                    CreateUserId = 0,
                    EndDate = "",
                    StartDate = "",
                    SortCode = 0,
                    Paras = new List<ScadaEquipmentParaParameterModel>()
                };
                foreach (var p in subparas)
                {
                    scadaEquipment.Paras.Add(new ScadaEquipmentParaParameterModel()
                    {
                        CanWrite = 1,
                        CreateTime = DateTime.Now,
                        CreateUserId = 0,
                        EndDate = "",
                        StartDate = "",
                        SortCode = 0,
                        EquipmentId = 0,
                        ParaId = p.IO_ID,
                        ParaName = p.IO_NAME,
                        ParaTitle = String.IsNullOrEmpty(p.IO_LABEL) ? p.IO_NAME : p.IO_LABEL,
                        ParaUnit = p.IO_ALIASNAME,
                        SerieName = p.IO_NAME,
                        SerieType = "随机",
                        Id = 0,
                        UpdateTime = DateTime.Now,
                        UpdateUserId = 0


                    });
                }
                equipments.Add(scadaEquipment);
            }
            var result = new { code = 0, count = total, data = equipments };
            JsonResult jsonResult = new JsonResult();
            jsonResult.MaxJsonLength = int.MaxValue;
            jsonResult.Data = result;
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            jsonResult.ContentType = "application/json";
            return jsonResult;
        }

        ///单个设备的实时数据显示
          #region 实时曲线
        public ActionResult GeneralEquipmentRealSerie(string Id)
        {
       
       
            IODeviceModel deviceModel= IO_DeviceServer.GetByWhere(" where IO_DEVICE_ID='" + Id + "'").FirstOrDefault();
            IOCommunicateModel communicateModel = IO_CommunicateServer.GetByWhere(" where IO_COMM_ID='" + deviceModel.IO_COMM_ID + "'").FirstOrDefault();
            IOServerModel serverModel = IO_Server.GetByWhere(" where SERVER_ID='" + deviceModel.IO_SERVER_ID + "'").FirstOrDefault();
            var paras = ParaService.GetByWhere(" where IO_DEVICE_ID='" + Id + "'");
            if (deviceModel != null)
            {
            
                ScadaEquipmentModel model = new ScadaEquipmentModel
                {
                    CommunicationId = deviceModel.IO_COMM_ID,
                    CommunicationName = communicateModel.IO_COMM_NAME,
               
                    DeviceId = deviceModel.IO_DEVICE_ID,
                    DeviceName = String.IsNullOrEmpty(deviceModel.IO_DEVICE_LABLE) ? deviceModel.IO_DEVICE_NAME : deviceModel.IO_DEVICE_LABLE,
                    GroupId = 0,
                    Id = 0,
                    IODevice = deviceModel,
                    ModelTitle = String.IsNullOrEmpty(deviceModel.IO_DEVICE_LABLE) ? deviceModel.IO_DEVICE_NAME : deviceModel.IO_DEVICE_LABLE,
                    Remark = "",
                    ScadaGroup = null,
                    ServerId = deviceModel.IO_SERVER_ID,
                    ServerName = deviceModel.IO_SERVER_ID,
              
                    EndDate = "",
                    StartDate = "",
                    CreateTime = DateTime.Now,
                    CreateUserId = 0,
                    UpdateTime = DateTime.Now,
                    SortCode = 0,
                    UpdateUserId = 0
                };
                model.Paras = new List<ScadaEquipmentParaParameterModel>();
                foreach(var p in paras)
                {
                    model.Paras.Add(new ScadaEquipmentParaParameterModel
                    {
                        CanWrite = 1,
                        CreateTime = DateTime.Now,
                        CreateUserId = 0,
                        UpdateTime = DateTime.Now,
                        SortCode = 0,
                        UpdateUserId = 0,
                        EndDate = "",
                        StartDate = "",
                        EquipmentId = 0,
                        Id = 0,
                        ParaId = p.IO_ID,
                        ParaName = p.IO_NAME,
                        ParaTitle = String.IsNullOrEmpty(p.IO_LABEL) ? p.IO_NAME : p.IO_LABEL,
                        ParaUnit = p.IO_ALIASNAME,
                        SerieName = p.IO_NAME,
                        SerieType = "随机样式"

                    }); ;
                }
                model.JsonParas = ScadaJsonConvertor.ObjectToJson(model.Paras);
                return View(model);
            }
            else
            {
                return View();
            }
        }
        /// <summary>
        /// 用户自由选择任意井进行专门的历史曲线查询模块的实际查询数据的功能
        /// </summary>
        /// <param name="wellid"></param>
        /// <param name="series"></param>
        /// <param name="charttype"></param>
        /// <param name="sdate"></param>
        /// <param name="edate"></param>
        /// <param name="serieclassify"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult EquipmentHistorySeriesData(SeriesPara para)
        {

            string serverid = para.serverid;
            string communicateid = para.communicateid;
            string deviceid = para.deviceid;
            int charttype = para.charttype;
            string sdate = para.sdate;
            string edate = para.edate;
            int pagesize = para.pagesize;
            int equipmentid = para.equipmentid;
            //曲线指标列表
            string[] serienames = para.selectserie.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            ApiSerieConfig serieConfig = new ApiSerieConfig();
            serieConfig.IO_DEVICE_ID = deviceid;
            serieConfig.Series = new List<ApiSerieConfigModel>();
   
            var ioparas = ParaService.GetByWhere(" where IO_DEVICE_ID='" + para.deviceid + "'");
            List<ScadaEquipmentParaParameterModel> paras = new List<ScadaEquipmentParaParameterModel>();
            foreach (var p in ioparas)
            {
                if(serienames.Contains(p.IO_NAME.Trim()))
                {
                    paras.Add(new ScadaEquipmentParaParameterModel
                    {
                        CanWrite = 1,
                        CreateTime = DateTime.Now,
                        CreateUserId = 0,
                        UpdateTime = DateTime.Now,
                        SortCode = 0,
                        UpdateUserId = 0,
                        EndDate = "",
                        StartDate = "",
                        EquipmentId = 0,
                        Id = 0,
                        ParaId = p.IO_NAME,
                        ParaName = p.IO_NAME,
                        ParaTitle = String.IsNullOrEmpty(p.IO_LABEL) ? p.IO_NAME : p.IO_LABEL,
                        ParaUnit = p.IO_ALIASNAME,
                        SerieName = p.IO_NAME,
                        SerieType = "随机样式"

                    }); 
                }
               
            }
         
            for (int i = 0; i < paras.Count; i++)
            {
                ApiSerieConfigModel apiSerie = new ApiSerieConfigModel();
                apiSerie.IO_ID = paras[i].ParaName;
                apiSerie.SerieColor = WebSerieColor.GetColor(i);
                apiSerie.SerieName = paras[i].ParaName;
                apiSerie.SymbolColor = WebSerieColor.GetColor(i);
                apiSerie.SerieTitle = paras[i].ParaTitle;
                apiSerie.IO_ID = paras[i].ParaId;

                serieConfig.Series.Add(apiSerie);

            }
            return GetEquipmentHistorySeriesData(serieConfig, serverid, communicateid, deviceid, charttype, sdate, edate, pagesize);
        }
        private JsonResult GetEquipmentHistorySeriesData(ApiSerieConfig serieConfig, string serverid, string communicateid, string deviceid, int charttype = 1, string sdate = "", string edate = "", int pagesize = 1000)
        {

            //获取对应的曲线配置信息

            List<ApiSerieConfigModel> Items = serieConfig.Series;
            ///初始化曲线对象
            EChartOption chartOption = new EChartOption();
            chartOption.xAxis = new Axis[1];
            chartOption.xAxis[0] = new Axis();
            chartOption.xAxis[0].gridIndex = 1;
            chartOption.xAxis[0].type = "time";
            chartOption.xAxis[0].name = "时间";
            List<Axis> yaxis = new List<Axis>();
            int index = 0;
            string[] legend = new string[Items.Count()];

            foreach (var item in Items)
            {
                yaxis.Add(new Axis() { gridIndex = index, name = item.SerieTitle, type = "value", data = new string[0] });
                legend[index] = item.SerieTitle;
                index++;
            }
            chartOption.legend.data = legend;
            chartOption.yAxis = yaxis.ToArray();
            //初始化对象结束
            if (sdate == null || sdate == "")
                sdate = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
            if (edate == null || edate == "")
                edate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            chartOption.series = new Series[Items.Count()];
            int snum = 0;
            foreach (var item in Items)
            {
                chartOption.series[snum] = new Series();
                chartOption.series[snum].id = item.IO_ID;
                chartOption.series[snum].name = item.SerieTitle;
                chartOption.series[snum].lineStyle.color = item.SerieColor;
                chartOption.series[snum].lineStyle.width = int.Parse(item.SerieWidth);

                chartOption.series[snum].data = new double[0];
                chartOption.series[snum].type = item.SerieType;
                chartOption.series[snum].showSymbol = item.ShowSymbol == "1" ? true : false;
                chartOption.series[snum].symbol = item.SymbolType;
                chartOption.series[snum].symbolSize = int.Parse(item.SymbolSize);
                snum++;
            }
            try
            {
                InfluxDBHistoryResult realResult = mWebInfluxDbManager.DbQuery_History(serverid, communicateid, deviceid, Convert.ToDateTime(sdate), Convert.ToDateTime(edate), 10000, 1, " ASC ");
                if (realResult != null)
                {

                    var datas = realResult.Seres;
                    if (datas != null && datas.Count() > 0)
                    {
                        var onedata = datas.First();
                        int sindex = 0;
                        foreach (var item in Items)
                        {
                            chartOption.series[sindex] = new Series();
                            chartOption.series[sindex].id = item.IO_ID;
                            chartOption.series[sindex].name = item.SerieTitle;
                            chartOption.series[sindex].lineStyle.color = item.SerieColor;
                            chartOption.series[sindex].lineStyle.width = int.Parse(item.SerieWidth);

                            chartOption.series[sindex].data = new double[onedata.Values.Count];
                            chartOption.series[sindex].type = item.SerieType;
                            chartOption.series[sindex].showSymbol = item.ShowSymbol == "1" ? true : false;
                            chartOption.series[sindex].symbol = item.SymbolType;
                            chartOption.series[sindex].symbolSize = int.Parse(item.SymbolSize);
                            sindex++;
                        }
                        string[] axisData = new string[onedata.Values.Count];
                        //获取的数据按照时间先后
                        int dataindex = onedata.Values.Count() - 1;

                        foreach (var value in onedata.Values)
                        {
                            //获取采集时间
                            object objx = onedata.Values[dataindex][onedata.Columns.IndexOf("time")];
                            axisData[dataindex] = objx != null ? objx.ToString() : "";
                            //////////////////////////
                            sindex = 0;
                            foreach (var item in Items)
                            {

                                try
                                {



                                    int recordindex = onedata.Columns.IndexOf("field_" + item.IO_ID.Trim().ToLower() + "_value");
                                    if (recordindex >= 0)
                                    {
                                        object objy = onedata.Values[dataindex][recordindex];
                                        chartOption.series[sindex].data[dataindex] = Convert.ToDouble(objy);
                                        chartOption.series[sindex].id = item.IO_ID;
                                    }


                                }
                                catch
                                {

                                }

                                sindex++;
                            }
                            dataindex--;
                        }
                        chartOption.xAxis[0].data = axisData;//设置x轴数据，time格式的数据必须在Axis轴上进行设置


                    }


                }


            }
            catch
            {


            }
            //读取以下的实时数据，从influxDB中读取
            return Json(chartOption, JsonRequestBehavior.AllowGet);
        }

        Random random = new Random();
        [HttpPost]
        public JsonResult EquipmentRealSeriesData(SeriesPara para)
        {
            //曲线指标列表
            string[] serienames = para.selectserie.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            ApiSerieConfig serieConfig = new ApiSerieConfig();
            serieConfig.IO_DEVICE_ID = para.deviceid;
            serieConfig.Series = new List<ApiSerieConfigModel>();
            var ioparas = ParaService.GetByWhere(" where IO_DEVICE_ID='" + para.deviceid + "'");
            List<ScadaEquipmentParaParameterModel> paras = new List<ScadaEquipmentParaParameterModel>();
            foreach (var p in ioparas)
            {
                if (serienames.Contains(p.IO_NAME.Trim()))
                {
                    paras.Add(new ScadaEquipmentParaParameterModel
                    {
                        CanWrite = 1,
                        CreateTime = DateTime.Now,
                        CreateUserId = 0,
                        UpdateTime = DateTime.Now,
                        SortCode = 0,
                        UpdateUserId = 0,
                        EndDate = "",
                        StartDate = "",
                        EquipmentId = 0,
                        Id = 0,
                        ParaId = p.IO_NAME,
                        ParaName = p.IO_NAME,
                        ParaTitle = String.IsNullOrEmpty(p.IO_LABEL) ? p.IO_NAME : p.IO_LABEL,
                        ParaUnit = p.IO_ALIASNAME,
                        SerieName = p.IO_NAME,
                        SerieType = "随机样式"

                    });
                }
            }
         
            para.serieConfig = serieConfig;
            RealSerieData resDatas = new RealSerieData();
            string serverid = para.serverid, communicateid = para.communicateid, deviceid = para.deviceid;
            int updatecycle = para.updatecycle;
            if (Configs.GetValue("SystemRunModel") == "Debug")
            {
                RealWebCacheDataItem realItem = new RealWebCacheDataItem() { CommunicationId = communicateid, DeviceId = deviceid, ServerId = serverid };
                realItem.ParaReals = new List<WebCacheIOData>();
                List<string> fs = new List<string>();
                foreach (var s in para.serieConfig.Series)
                {
                    fs.Add(s.SerieName);

                }

                for (int i = 0; i < serienames.Length; i++)
                {

                    realItem.ParaReals.Add(new WebCacheIOData
                    {

                        Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        ParaID = serienames[i],
                        ParaName = serienames[i],
                        ParaValue = random.Next(100).ToString(),
                        QualityStamp = "GOOD"
                    });
                }


                if (realItem != null)
                {

                    realItem.ParaReals.ForEach(delegate (WebCacheIOData iOData)
                    {   ///删除重复项

                        PointData rdata = new PointData() { Name = iOData.ParaName, Status = 1, Value = iOData.ParaValue.ToString() };
                        resDatas.Date = iOData.Date;
                        resDatas.Data.Add(rdata);

                    });


                }
            }
            else
            {

                RealWebCacheDataItem realItem = mWebRealCache.GetReal(new WebCacheDataItem() { CommunicationId = communicateid, DeviceId = deviceid, ServerId = serverid });

                List<string> fs = new List<string>();
                foreach (var s in para.serieConfig.Series)
                {
                    fs.Add(s.SerieName);
                }

                if (realItem != null)
                {

                    realItem.ParaReals.ForEach(delegate (WebCacheIOData iOData)
                    {   ///删除重复项

                        PointData rdata = new PointData() { Name = iOData.ParaName, Status = 1, Value = iOData.ParaValue.ToString() };
                        resDatas.Date = iOData.Date;
                        resDatas.Data.Add(rdata);

                    });


                }
            }

            //读取以下的实时数据，从influxDB中读取
            return Json(resDatas, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region 历史数据查询
        public ActionResult GeneralEquipmentHistoryQuery(string Id)
        {
          

            IODeviceModel deviceModel = IO_DeviceServer.GetByWhere(" where IO_DEVICE_ID='" + Id + "'").FirstOrDefault();
            IOCommunicateModel communicateModel = IO_CommunicateServer.GetByWhere(" where IO_COMM_ID='" + deviceModel.IO_COMM_ID + "'").FirstOrDefault();
            IOServerModel serverModel = IO_Server.GetByWhere(" where SERVER_ID='" + deviceModel.IO_SERVER_ID + "'").FirstOrDefault();
            var paras = ParaService.GetByWhere(" where IO_DEVICE_ID='" + Id + "'");
            if (deviceModel != null)
            {

                ScadaEquipmentModel model = new ScadaEquipmentModel
                {
                    CommunicationId = deviceModel.IO_COMM_ID,
                    CommunicationName = communicateModel.IO_COMM_NAME,

                    DeviceId = deviceModel.IO_DEVICE_ID,
                    DeviceName = String.IsNullOrEmpty(deviceModel.IO_DEVICE_LABLE) ? deviceModel.IO_DEVICE_NAME : deviceModel.IO_DEVICE_LABLE,
                    GroupId = 0,
                    Id = 0,
                    IODevice = deviceModel,
                    ModelTitle = String.IsNullOrEmpty(deviceModel.IO_DEVICE_LABLE) ? deviceModel.IO_DEVICE_NAME : deviceModel.IO_DEVICE_LABLE,
                    Remark = "",
                    ScadaGroup = null,
                    ServerId = deviceModel.IO_SERVER_ID,
                    ServerName = deviceModel.IO_SERVER_ID,

                    EndDate = "",
                    StartDate = "",
                    CreateTime = DateTime.Now,
                    CreateUserId = 0,
                    UpdateTime = DateTime.Now,
                    SortCode = 0,
                    UpdateUserId = 0
                };
                model.Paras = new List<ScadaEquipmentParaParameterModel>();
                foreach (var p in paras)
                {
                    model.Paras.Add(new ScadaEquipmentParaParameterModel
                    {
                        CanWrite = 1,
                        CreateTime = DateTime.Now,
                        CreateUserId = 0,
                        UpdateTime = DateTime.Now,
                        SortCode = 0,
                        UpdateUserId = 0,
                        EndDate = "",
                        StartDate = "",
                        EquipmentId = 0,
                        Id = 0,
                        ParaId = p.IO_ID,
                        ParaName = p.IO_NAME,
                        ParaTitle = String.IsNullOrEmpty(p.IO_LABEL) ? p.IO_NAME : p.IO_LABEL,
                        ParaUnit = p.IO_ALIASNAME,
                        SerieName = p.IO_NAME,
                        SerieType = "随机样式"

                    }); ;
                }
                model.JsonParas = ScadaJsonConvertor.ObjectToJson(model.Paras);
                return View(model);
            }
            else
            {
                return View();
            }
        }
        [HttpGet]
        public JsonResult EquipmentHistoryData(ScadaEquipmentModel model, PageInfo pageInfo)
        {
            if(model.Tag==null|| model.Tag=="")
                return Json("[]", "application/text", JsonRequestBehavior.AllowGet);
            List<string> columns = model.Tag.Split(',').ToList();
            ScadaEquipmentModel serachemodel = model;
  
            if (serachemodel == null)
                return Json(null, "application/text", JsonRequestBehavior.AllowGet);
            string items = "[";

           
            if (!string.IsNullOrWhiteSpace(serachemodel.DeviceId))
            {
                try
                {
                    string sdate = model.StartDate;
                    string edate = model.EndDate;
                    InfluxDBHistoryResult realResult = mWebInfluxDbManager.DbQuery_History(model.ServerId, model.CommunicationId, model.DeviceId, Convert.ToDateTime(sdate), Convert.ToDateTime(edate), pageInfo.limit, pageInfo.page, " DESC ");
                    foreach (var s in realResult.Seres)
                    {

                        List<int> indexs = new List<int>();
                        for (int i = 0; i < s.Values.Count; i++)
                        {
                            string jsonrow = "";
                            int index = s.Columns.IndexOf("time");
                            object time = s.Values[i][index];
                            jsonrow += "{";
                            jsonrow += "\"DateStampTime\":\"" + (time != null ? time.ToString() : "") + "\"";

                            foreach (string str in columns)
                            {
                                try
                                {

                                    index = -1;
                                    index = s.Columns.IndexOf("field_" + str.Trim().ToLower().ToString() + "_value");
                                    if (index >= 0)
                                    {
                                        object v = s.Values[i][index];

                                        jsonrow += ",\"" + str + "\":\"" + (v != null ? v.ToString() : "") + "\"";
                                    }

                                }
                                catch
                                {
                                    continue;
                                }
                            }
                            jsonrow += "},";

                            items += jsonrow;

                        }
                    }
                    items += "]";
                    var result = Pager.Paging2(items, realResult.RecordCount);
                    //读取以下的实时数据，从influxDB中读取
          
                    JsonResult jsonResult = new JsonResult();
                    jsonResult.MaxJsonLength = int.MaxValue;
                    jsonResult.ContentType = "application/text";
                    jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                    jsonResult.Data = result;
                    return jsonResult;
                }
                catch
                {
                    return Json(null, "application/text", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var result = Pager.Paging2("", 0);
                JsonResult jsonResult = new JsonResult();
                jsonResult.MaxJsonLength = int.MaxValue;
                jsonResult.ContentType = "application/text";
                jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                jsonResult.Data = result;
                return jsonResult;
            }
        }
        #endregion
        #region 历史曲线
        public ActionResult GeneralEquipmentHistorySerie(string Id)
        {
            IODeviceModel deviceModel = IO_DeviceServer.GetByWhere(" where IO_DEVICE_ID='" + Id + "'").FirstOrDefault();
            IOCommunicateModel communicateModel = IO_CommunicateServer.GetByWhere(" where IO_COMM_ID='" + deviceModel.IO_COMM_ID + "'").FirstOrDefault();
            IOServerModel serverModel = IO_Server.GetByWhere(" where SERVER_ID='" + deviceModel.IO_SERVER_ID + "'").FirstOrDefault();
            var paras = ParaService.GetByWhere(" where IO_DEVICE_ID='" + Id + "'");
            if (deviceModel != null)
            {

                ScadaEquipmentModel model = new ScadaEquipmentModel
                {
                    CommunicationId = deviceModel.IO_COMM_ID,
                    CommunicationName = communicateModel.IO_COMM_NAME,

                    DeviceId = deviceModel.IO_DEVICE_ID,
                    DeviceName = String.IsNullOrEmpty(deviceModel.IO_DEVICE_LABLE) ? deviceModel.IO_DEVICE_NAME : deviceModel.IO_DEVICE_LABLE,
                    GroupId = 0,
                    Id = 0,
                    IODevice = deviceModel,
                    ModelTitle = String.IsNullOrEmpty(deviceModel.IO_DEVICE_LABLE) ? deviceModel.IO_DEVICE_NAME : deviceModel.IO_DEVICE_LABLE,
                    Remark = "",
                    ScadaGroup = null,
                    ServerId = deviceModel.IO_SERVER_ID,
                    ServerName = deviceModel.IO_SERVER_ID,

                    EndDate = "",
                    StartDate = "",
                    CreateTime = DateTime.Now,
                    CreateUserId = 0,
                    UpdateTime = DateTime.Now,
                    SortCode = 0,
                    UpdateUserId = 0
                };
                model.Paras = new List<ScadaEquipmentParaParameterModel>();
                foreach (var p in paras)
                {
                    model.Paras.Add(new ScadaEquipmentParaParameterModel
                    {
                        CanWrite = 1,
                        CreateTime = DateTime.Now,
                        CreateUserId = 0,
                        UpdateTime = DateTime.Now,
                        SortCode = 0,
                        UpdateUserId = 0,
                        EndDate = "",
                        StartDate = "",
                        EquipmentId = 0,
                        Id = 0,
                        ParaId = p.IO_ID,
                        ParaName = p.IO_NAME,
                        ParaTitle = String.IsNullOrEmpty(p.IO_LABEL) ? p.IO_NAME : p.IO_LABEL,
                        ParaUnit = p.IO_ALIASNAME,
                        SerieName = p.IO_NAME,
                        SerieType = "随机样式"

                    }); ;
                }
                model.JsonParas = ScadaJsonConvertor.ObjectToJson(model.Paras);
                return View(model);
            }
            else
            {
                return View();
            }
        }

        #endregion
        #region 历史统计
        public ActionResult GeneralEquipmentHistorySummaryQuery(string Id)
        {
            IODeviceModel deviceModel = IO_DeviceServer.GetByWhere(" where IO_DEVICE_ID='" + Id + "'").FirstOrDefault();
            IOCommunicateModel communicateModel = IO_CommunicateServer.GetByWhere(" where IO_COMM_ID='" + deviceModel.IO_COMM_ID + "'").FirstOrDefault();
            IOServerModel serverModel = IO_Server.GetByWhere(" where SERVER_ID='" + deviceModel.IO_SERVER_ID + "'").FirstOrDefault();
            var paras = ParaService.GetByWhere(" where IO_DEVICE_ID='" + Id + "'");
            if (deviceModel != null)
            {

                ScadaEquipmentModel model = new ScadaEquipmentModel
                {
                    CommunicationId = deviceModel.IO_COMM_ID,
                    CommunicationName = communicateModel.IO_COMM_NAME,

                    DeviceId = deviceModel.IO_DEVICE_ID,
                    DeviceName = String.IsNullOrEmpty(deviceModel.IO_DEVICE_LABLE) ? deviceModel.IO_DEVICE_NAME : deviceModel.IO_DEVICE_LABLE,
                    GroupId = 0,
                    Id = 0,
                    IODevice = deviceModel,
                    ModelTitle = String.IsNullOrEmpty(deviceModel.IO_DEVICE_LABLE) ? deviceModel.IO_DEVICE_NAME : deviceModel.IO_DEVICE_LABLE,
                    Remark = "",
                    ScadaGroup = null,
                    ServerId = deviceModel.IO_SERVER_ID,
                    ServerName = deviceModel.IO_SERVER_ID,

                    EndDate = "",
                    StartDate = "",
                    CreateTime = DateTime.Now,
                    CreateUserId = 0,
                    UpdateTime = DateTime.Now,
                    SortCode = 0,
                    UpdateUserId = 0
                };
                model.Paras = new List<ScadaEquipmentParaParameterModel>();
                foreach (var p in paras)
                {
                    model.Paras.Add(new ScadaEquipmentParaParameterModel
                    {
                        CanWrite = 1,
                        CreateTime = DateTime.Now,
                        CreateUserId = 0,
                        UpdateTime = DateTime.Now,
                        SortCode = 0,
                        UpdateUserId = 0,
                        EndDate = "",
                        StartDate = "",
                        EquipmentId = 0,
                        Id = 0,
                        ParaId = p.IO_ID,
                        ParaName = p.IO_NAME,
                        ParaTitle = String.IsNullOrEmpty(p.IO_LABEL) ? p.IO_NAME : p.IO_LABEL,
                        ParaUnit = p.IO_ALIASNAME,
                        SerieName = p.IO_NAME,
                        SerieType = "随机样式"

                    }); ;
                }
                model.JsonParas = ScadaJsonConvertor.ObjectToJson(model.Paras);
                return View(model);
            }
            else
            {
                return View();
            }
        }
        private bool IsNumberIOPoint(IOParaModel para)
        {
            //"模拟量",
            //"开关量",
            //"字符串量",
            //"计算值",
            //"关系数据库值",
            //"常量值"

            if (para.IO_POINTTYPE == "模拟量" ||
            para.IO_POINTTYPE == "开关量" ||
            para.IO_POINTTYPE == "计算值" ||
             para.IO_POINTTYPE == "常量值")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        [HttpGet]
        public JsonResult EquipmentHistorySummaryData(ScadaEquipmentSummaryPageModel model, PageInfo pageInfo)
        {
            if (model.Tag == null || model.Tag == "")
                return Json("[]", "application/text", JsonRequestBehavior.AllowGet);
            List<string> columns = model.Tag.Split(',').ToList();
            ScadaEquipmentModel serachemodel = model;

            if (serachemodel == null)
                return Json(null, "application/text", JsonRequestBehavior.AllowGet);
            List<IOParaModel> iOParas = this.ParaService.GetByWhere(" where IO_SERVER_ID='" + serachemodel.ServerId + "' and IO_COMM_ID='" + serachemodel.CommunicationId + "' and IO_DEVICE_ID='" + serachemodel.DeviceId + "'").ToList();

            if (serachemodel != null)
            {
           
                serachemodel.Paras = new List<ScadaEquipmentParaParameterModel>();
                foreach (var p in iOParas)
                {
                    if (!columns.Contains(p.IO_NAME))
                        continue;
                    serachemodel.Paras.Add(new ScadaEquipmentParaParameterModel
                    {
                        CanWrite = 1,
                        CreateTime = DateTime.Now,
                        CreateUserId = 0,
                        UpdateTime = DateTime.Now,
                        SortCode = 0,
                        UpdateUserId = 0,
                        EndDate = "",
                        StartDate = "",
                        EquipmentId = 0,
                        Id = 0,
                        ParaId = p.IO_ID,
                        ParaName = p.IO_NAME,
                        ParaTitle = String.IsNullOrEmpty(p.IO_LABEL) ? p.IO_NAME : p.IO_LABEL,
                        ParaUnit = p.IO_ALIASNAME,
                        SerieName = p.IO_NAME,
                        SerieType = "随机样式"

                    }); ;
                }
            }
            if (serachemodel == null)
                return Json(null, "application/text", JsonRequestBehavior.AllowGet);
            
            string items = "[";
 


            if (!string.IsNullOrWhiteSpace(serachemodel.DeviceId))
            {
                try
                {
                    
                    string returnFields = "  time";
                    #region
                    {

                        foreach (var item in serachemodel.Paras)
                        {
                            var para = iOParas.Find(x => x.IO_NAME.Trim().ToLower() == item.ParaName.Trim().ToLower().ToString());
                            if (para != null && IsNumberIOPoint(para))
                            {
                                returnFields += "," + model.Method + "(field_" + item.ParaName.Trim().ToLower().ToString() + "_value) as field_" + item.ParaName.Trim().ToLower().ToString() + "_value";
                            }
                            else
                            {
                                returnFields += ",mean(null) as field_" + item.ParaName.Trim().ToLower().ToString() + "_value";
                            }

                        }


                    }
                    #endregion
                    string sdate = model.StartDate;
                    string edate = model.EndDate;
                    InfluxDBHistoryResult realResult = mWebInfluxDbManager.DbQuery_HistoryStatics(model.ServerId, model.CommunicationId, model.DeviceId, Convert.ToDateTime(sdate), Convert.ToDateTime(edate), pageInfo.limit, pageInfo.page, " DESC ", model.Period, returnFields);
                    foreach (var s in realResult.Seres)
                    {

                        List<int> indexs = new List<int>();
                        for (int i = 0; i < s.Values.Count; i++)
                        {
                            string jsonrow = "";
                            int index = s.Columns.IndexOf("time");
                            object time = s.Values[i][index];
                            jsonrow += "{";
                            jsonrow += "\"DateStampTime\":\"" + (time != null ? time.ToString() : "") + "\"";

                            foreach (string str in columns)
                            {
                                try
                                {

                                    index = -1;
                                    index = s.Columns.IndexOf("field_" + str.Trim().ToLower().ToString() + "_value");
                                    if (index >= 0)
                                    {
                                        object v = s.Values[i][index];

                                        jsonrow += ",\"" + str + "\":\"" + (v != null ? v.ToString() : "") + "\"";
                                    }

                                }
                                catch
                                {
                                    continue;
                                }
                            }
                            jsonrow += "},";

                            items += jsonrow;

                        }
                    }
                    items += "]";
                    var result = Pager.Paging2(items, realResult.RecordCount);
                    //读取以下的实时数据，从influxDB中读取
                    return Json(result, "application/text", JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return Json(null, "application/text", JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var result = Pager.Paging2("", 0);
                return Json(result, "application/text", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
        #region 历史统计曲线
        public ActionResult GeneralEquipmentHistorySummarySerie(string Id)
        {
            IODeviceModel deviceModel = IO_DeviceServer.GetByWhere(" where IO_DEVICE_ID='" + Id + "'").FirstOrDefault();
            IOCommunicateModel communicateModel = IO_CommunicateServer.GetByWhere(" where IO_COMM_ID='" + deviceModel.IO_COMM_ID + "'").FirstOrDefault();
            IOServerModel serverModel = IO_Server.GetByWhere(" where SERVER_ID='" + deviceModel.IO_SERVER_ID + "'").FirstOrDefault();
            var paras = ParaService.GetByWhere(" where IO_DEVICE_ID='" + Id + "'");
            if (deviceModel != null)
            {

                ScadaEquipmentModel model = new ScadaEquipmentModel
                {
                    CommunicationId = deviceModel.IO_COMM_ID,
                    CommunicationName = communicateModel.IO_COMM_NAME,

                    DeviceId = deviceModel.IO_DEVICE_ID,
                    DeviceName = String.IsNullOrEmpty(deviceModel.IO_DEVICE_LABLE) ? deviceModel.IO_DEVICE_NAME : deviceModel.IO_DEVICE_LABLE,
                    GroupId = 0,
                    Id = 0,
                    IODevice = deviceModel,
                    ModelTitle = String.IsNullOrEmpty(deviceModel.IO_DEVICE_LABLE) ? deviceModel.IO_DEVICE_NAME : deviceModel.IO_DEVICE_LABLE,
                    Remark = "",
                    ScadaGroup = null,
                    ServerId = deviceModel.IO_SERVER_ID,
                    ServerName = deviceModel.IO_SERVER_ID,

                    EndDate = "",
                    StartDate = "",
                    CreateTime = DateTime.Now,
                    CreateUserId = 0,
                    UpdateTime = DateTime.Now,
                    SortCode = 0,
                    UpdateUserId = 0
                };
                model.Paras = new List<ScadaEquipmentParaParameterModel>();
                foreach (var p in paras)
                {
                    model.Paras.Add(new ScadaEquipmentParaParameterModel
                    {
                        CanWrite = 1,
                        CreateTime = DateTime.Now,
                        CreateUserId = 0,
                        UpdateTime = DateTime.Now,
                        SortCode = 0,
                        UpdateUserId = 0,
                        EndDate = "",
                        StartDate = "",
                        EquipmentId = 0,
                        Id = 0,
                        ParaId = p.IO_NAME,
                        ParaName = p.IO_NAME,
                        ParaTitle = String.IsNullOrEmpty(p.IO_LABEL) ? p.IO_NAME : p.IO_LABEL,
                        ParaUnit = p.IO_ALIASNAME,
                        SerieName = p.IO_NAME,
                        SerieType = "随机样式"

                    }); ;
                }
                model.JsonParas = ScadaJsonConvertor.ObjectToJson(model.Paras);
                return View(model);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public JsonResult EquipmentHistorySummarySeriesData(SeriesPara para)
        {


            string serverid = para.serverid;
            string communicateid = para.communicateid;
            string deviceid = para.deviceid;
            int charttype = para.charttype;
            string sdate = para.sdate;
            string edate = para.edate;
            int pagesize = para.pagesize;
            int equipmentid = para.equipmentid;
            //曲线指标列表
            string[] serienames = para.selectserie.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            ApiSerieConfig serieConfig = new ApiSerieConfig();
            serieConfig.IO_DEVICE_ID = deviceid;
            serieConfig.Series = new List<ApiSerieConfigModel>();

            var ioparas = ParaService.GetByWhere(" where IO_DEVICE_ID='" + para.deviceid + "'");
            List<ScadaEquipmentParaParameterModel> paras = new List<ScadaEquipmentParaParameterModel>();
            foreach (var p in ioparas)
            {
                if (serienames.Contains(p.IO_NAME.Trim()))
                {
                    paras.Add(new ScadaEquipmentParaParameterModel
                    {
                        CanWrite = 1,
                        CreateTime = DateTime.Now,
                        CreateUserId = 0,
                        UpdateTime = DateTime.Now,
                        SortCode = 0,
                        UpdateUserId = 0,
                        EndDate = "",
                        StartDate = "",
                        EquipmentId = 0,
                        Id = 0,
                        ParaId = p.IO_NAME,
                        ParaName = p.IO_NAME,
                        ParaTitle = String.IsNullOrEmpty(p.IO_LABEL) ? p.IO_NAME : p.IO_LABEL,
                        ParaUnit = p.IO_ALIASNAME,
                        SerieName = p.IO_NAME,
                        SerieType = "随机样式"

                    });
                }

            }

            for (int i = 0; i < paras.Count; i++)
            {
                ApiSerieConfigModel apiSerie = new ApiSerieConfigModel();
                apiSerie.IO_ID = paras[i].ParaName;
                apiSerie.SerieColor = WebSerieColor.GetColor(i);
                apiSerie.SerieName = paras[i].ParaName;
                apiSerie.SymbolColor = WebSerieColor.GetColor(i);
                apiSerie.SerieTitle = paras[i].ParaTitle;
                apiSerie.IO_ID = paras[i].ParaId;

                serieConfig.Series.Add(apiSerie);

            }
            return GetEquipmentHistorySummarySeriesData(serieConfig, serverid, communicateid, deviceid, charttype, sdate, edate, pagesize, para.period, para.method);
        }
        private JsonResult GetEquipmentHistorySummarySeriesData(ApiSerieConfig serieConfig, string serverid, string communicateid, string deviceid, int charttype = 1, string sdate = "", string edate = "", int pagesize = 1000, string timespan = "1m", string method = "MEAN")
        {

            //获取对应的曲线配置信息

            List<ApiSerieConfigModel> Items = serieConfig.Series;
            ///初始化曲线对象
            EChartOption chartOption = new EChartOption();
            chartOption.xAxis = new Axis[1];
            chartOption.xAxis[0] = new Axis();
            chartOption.xAxis[0].gridIndex = 1;
            chartOption.xAxis[0].type = "time";
            chartOption.xAxis[0].name = "时间";
            List<Axis> yaxis = new List<Axis>();
            int index = 0;
            string[] legend = new string[Items.Count()];

            foreach (var item in Items)
            {
                yaxis.Add(new Axis() { gridIndex = index, name = item.SerieTitle, type = "value", data = new string[0] });
                legend[index] = item.SerieTitle;
                index++;
            }
            chartOption.legend.data = legend;
            chartOption.yAxis = yaxis.ToArray();
            //初始化对象结束
            if (sdate == null || sdate == "")
                sdate = DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm:ss");
            if (edate == null || edate == "")
                edate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            chartOption.series = new Series[Items.Count()];
            int snum = 0;
            foreach (var item in Items)
            {
                chartOption.series[snum] = new Series();
                chartOption.series[snum].id = item.IO_ID;
                chartOption.series[snum].name = item.SerieTitle;

                chartOption.series[snum].lineStyle.color = item.SerieColor;
                chartOption.series[snum].lineStyle.width = int.Parse(item.SerieWidth);

                chartOption.series[snum].data = new double[0];
                chartOption.series[snum].type = item.SerieType;
                chartOption.series[snum].showSymbol = item.ShowSymbol == "1" ? true : false;
                chartOption.series[snum].symbol = item.SymbolType;
                chartOption.series[snum].symbolSize = int.Parse(item.SymbolSize);

                snum++;
            }
            try
            {
                List<IOParaModel> iOParas = this.ParaService.GetByWhere(" where IO_SERVER_ID='" + serverid + "' and IO_COMM_ID='" + communicateid + "' and IO_DEVICE_ID='" + deviceid + "'").ToList();

                string returnFields = "  time";
                #region
                {

                    foreach (var item in Items)
                    {
                        var para = iOParas.Find(x => x.IO_NAME.Trim().ToLower() == item.IO_ID.Trim().ToLower().ToString());
                        if (para != null && IsNumberIOPoint(para))
                        {
                            returnFields += "," + method + "(field_" + item.IO_ID.Trim().ToLower().ToString() + "_value) as field_" + item.IO_ID.Trim().ToLower().ToString() + "_value";
                        }
                        else
                        {
                            returnFields += ",mean(null) as field_" + item.IO_ID.Trim().ToLower().ToString() + "_value";
                        }

                    }


                }
                #endregion
                InfluxDBHistoryResult realResult = mWebInfluxDbManager.DbQuery_HistoryStatics(serverid, communicateid, deviceid, Convert.ToDateTime(sdate), Convert.ToDateTime(edate), 10000, 1, " ASC ", timespan, returnFields);
                if (realResult != null)
                {

                    var datas = realResult.Seres;
                    if (datas != null && datas.Count() > 0)
                    {
                        var onedata = datas.First();
                        int sindex = 0;
                        foreach (var item in Items)
                        {
                            chartOption.series[sindex] = new Series();
                            chartOption.series[sindex].id = item.IO_ID;
                            chartOption.series[sindex].name = item.SerieTitle;
                            chartOption.series[sindex].lineStyle.color = item.SerieColor;
                            chartOption.series[sindex].lineStyle.width = int.Parse(item.SerieWidth);

                            chartOption.series[sindex].data = new double[onedata.Values.Count];
                            chartOption.series[sindex].type = item.SerieType;
                            chartOption.series[sindex].showSymbol = item.ShowSymbol == "1" ? true : false;
                            chartOption.series[sindex].symbol = item.SymbolType;
                            chartOption.series[sindex].symbolSize = int.Parse(item.SymbolSize);
                            sindex++;
                        }
                        string[] axisData = new string[onedata.Values.Count];
                        //获取的数据按照时间先后
                        int dataindex = onedata.Values.Count() - 1;

                        foreach (var value in onedata.Values)
                        {
                            //获取采集时间
                            object objx = onedata.Values[dataindex][onedata.Columns.IndexOf("time")];
                            axisData[dataindex] = objx != null ? objx.ToString() : "";
                            //////////////////////////
                            sindex = 0;
                            foreach (var item in Items)
                            {

                                try
                                {



                                    int recordindex = onedata.Columns.IndexOf("field_" + item.IO_ID.Trim().ToLower() + "_value");
                                    if (recordindex >= 0)
                                    {
                                        object objy = onedata.Values[dataindex][recordindex];
                                        chartOption.series[sindex].data[dataindex] = Convert.ToDouble(objy);
                                        chartOption.series[sindex].id = item.SerieName;
                                    }


                                }
                                catch
                                {

                                }

                                sindex++;
                            }
                            dataindex--;
                        }
                        chartOption.xAxis[0].data = axisData;//设置x轴数据，time格式的数据必须在Axis轴上进行设置


                    }


                }


            }
            catch
            {


            }
            //读取以下的实时数据，从influxDB中读取
            return Json(chartOption, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}