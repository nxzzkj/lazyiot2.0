using Scada.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Kernel
{
    public partial class SimulatorDeviceEditControl : UserControl
    {
      
        /// <summary>
        /// 保存用户的界面参数
        /// </summary>
        public string ParameterString = "";
        public SimulatorDeviceEditControl()
        {
            InitializeComponent();
            ParameterString = "";
        }
        private IO_DEVICE Device = null;
        /// <summary>
        /// 设置界面参数
        /// </summary>
        /// <param name="para"></param>
        public   void SetUIParameter(IO_DEVICE para)
        {
            if(Device!=null)
            ParameterString = Device.IO_DEVICE_SIMULATOR_PARASTRING;
            InitUIParameter(para);
        }
        public virtual void InitUIParameter(IO_DEVICE para)
        {

        }
        //从界面返回用户设置的参数
        public virtual string GetUIParameter()
        {
            return ParameterString;
        }
        public virtual ScadaResult IsValidParameter()
        {
            return new ScadaResult(true, "参数有效");
        }

    }
}
