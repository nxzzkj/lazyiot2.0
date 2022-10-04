using ScadaWeb.DapperExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaWeb.Model
{
    public class TransferItem
    {
        public string value { set; get; }
        public string title { set; get; }
    }
    [Table("ScadaEquipment")]
    public class ScadaEquipmentModel: Entity
    {
        public int GroupId { set; get; }
        public string DeviceId { set; get; }
        public string ServerId { set; get; }
        public string CommunicationId { set; get; }
        public string ModelTitle { set; get; }
        public string Remark { set; get; }

        [Computed]
        public List<ScadaEquipmentParaParameterModel> Paras { set; get; } = new List<ScadaEquipmentParaParameterModel>();
        [Computed]
        public string JsonParas
        { set; get; }
        [Computed]
        public string TransferParaValues
        {
            get
            {
                string str = "";
                if (Paras != null)
                {
                    for (int i = 0; i < Paras.Count; i++)
                    {
                        str += "," + Paras[i].ParaName;
                    }
                }
                if (!string.IsNullOrEmpty(str))
                {
                    str = str.Substring(1, str.Length - 1);
                }
                return str;
            }
        }

        [Computed]
        public ScadaGroupModel ScadaGroup { set; get; }
        [Computed]
        public IODeviceModel IODevice { set; get; }
        [Computed]
        public string ServerName { set; get; }
        [Computed]
        public string CommunicationName { set; get; }
        [Computed]
        public string DeviceName { set; get; }

        [Computed]
        public string Tag { set; get; }

    }
   
    public class ScadaEquipmentSummaryPageModel : ScadaEquipmentModel
    {

      
        [Computed]
        public string Period { set; get; }


        [Computed]
        public string Method { set; get; }
    }
}
