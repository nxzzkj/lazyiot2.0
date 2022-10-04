using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScadaWeb.Web.Models
{
    public class PointData
    {
        public string Value = "";
        public string Name = "";

        public int Status = 0;
    }
    public class RealSerieData
    {
        public string Date = "";

        public int Status = 0;
        public List<PointData> Data = new List<PointData>();
    }
    public class ApiSerieConfigModel
    {
        public string IO_ID { set; get; }
        public string SerieName { set; get; }
        public string SerieTitle { get; set; }
        public string SerieWidth { get; set; }
        //"line","bar"
        public string SerieType { get; set; }
        public string SerieColor { get; set; }
        public string SymbolSize { get; set; }

        //'circle', 'rect', 'roundRect', 'triangle', 'diamond', 'pin', 'arrow', 'none'
        public string SymbolType { get; set; }
        public string SymbolColor { get; set; }
        public string SymbolStep { get; set; }
        public string ShowSymbol { get; set; }
        public string ShowLegend { get; set; }

        public ApiSerieConfigModel()
        {
            SerieWidth = "2";
            SerieColor = "#FF0000";
            SymbolColor = "#0000FF";
            SymbolType = "circle";
            SymbolSize = "8";
            SymbolStep = "8";
            ShowSymbol = "true";
            ShowLegend = "true";

            IO_ID = "";
            SerieName = "";
            SerieType = "line";
        }


    }
    public sealed class ApiSerieConfig
    {
        public string IO_DEVICE_ID
        {

            set; get;
        }
        public List<ApiSerieConfigModel> Series { set; get; }
    }
}