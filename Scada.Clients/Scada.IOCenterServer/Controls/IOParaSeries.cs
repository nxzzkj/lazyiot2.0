using Scada.Model;
using ScadaCenterServer.Core;
using System.Windows.Forms.DataVisualization.Charting;


 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
namespace ScadaCenterServer.Controls
{
    public class IOSeriesDataPoint : DataPoint
    {
        public IOSeriesDataPoint(string date)
        {
            DateTimeString = date;
        }
        public string DateTimeString = "";
    }
    /// <summary>
    /// 定义实时曲线类
    /// </summary>
    public class IOParaSeries : Series
    {

        public IOParaSeries(IO_SERVER server, IO_COMMUNICATION communication, IO_DEVICE device, IO_PARA para)
        {
            this.Name = para.IO_NAME;
            this.LegendText = para.IO_LABEL;
            this.ChartType = SeriesChartType.Line;
            this.BorderWidth = 4;
            this.MarkerSize = 4;
            this.MarkerStep = 1;
            this.MarkerStyle = MarkerStyle.Circle;
            this.MarkerColor = System.Drawing.Color.Black;
            this.ChartArea = "IOChartArea";
            this.Para = para;
            this.Server = server;
            this.Communication = communication;
            this.Device = device;
            this.XValueType = ChartValueType.Time;
            this.YValueType = ChartValueType.Double;
            this.Color = System.Drawing.Color.FromArgb(155 + IOCenterManager.QueryFormManager.MainRandom.Next(100), 155 + IOCenterManager.QueryFormManager.MainRandom.Next(100), IOCenterManager.QueryFormManager.MainRandom.Next(255));


        }
        public IO_DEVICE Device = null;
        public IO_COMMUNICATION Communication = null;
        public IO_SERVER Server = null;
        public IO_PARA Para = null;
        /// <summary>
        /// 刷新实时数据,只绘制模拟量曲线
        /// </summary>
        public void RefreshRealData()
        {
            ///只绘制模拟量曲线
            if (Para != null && Para.IO_POINTTYPE.Trim() == "模拟量")
            {
                if (this.Points.Count > 100)
                {
                    this.Points.RemoveAt(0);

                }
                IOSeriesDataPoint dp = new IOSeriesDataPoint(Para.RealDate);
                dp.SetValueXY(Para.RealDate, new object[1] { Para.RealValue });
                if (this.Points.Count > 0)
                {
                    if (((IOSeriesDataPoint)this.Points[this.Points.Count - 1]).DateTimeString.Trim() == Para.RealDate.Trim())
                    {
                        return;
                    }

                }

                this.Points.Add(dp);
                if (Para.RealQualityStamp == Scada.IOStructure.QualityStamp.GOOD)
                {
                    this.Points[this.Points.Count - 1].IsEmpty = false;
                }
                else
                {
                    this.Points[this.Points.Count - 1].IsEmpty = true;
                }
                this.Points[this.Points.Count - 1].ToolTip = "时间:" + Para.RealDate + ",值:" + Para.RealValue;


            }
        }

    }
}
