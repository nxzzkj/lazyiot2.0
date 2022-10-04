using ScadaWeb.DapperExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaWeb.Model
{
    [Table("ScadaEquipmentParaParameter")]
    public class ScadaEquipmentParaParameterModel: Entity
    {
        public int EquipmentId { set; get; }
        public string ParaId { set; get; }
        public string ParaName { set; get; }//参数原始名称
        public string ParaTitle { set; get; }//参数标题别名
        public string ParaUnit { set; get; }//参数单位
        public string SerieName { set; get; }//曲线代码
        public string SerieType { set; get; }//曲线代码
        public int CanWrite { set; get; }
    }
}
