using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.BatchCommand
{
   public  class IOParaTextBox:TextBox
    {
        public Scada.Model.IO_PARA IOParament = null;
     
        public string FullPath()
        {
            if (IOParament != null)
            {
                return   IOParament.IO_NAME + "[" + IOParament.IO_LABEL + "]";
            }
            else
            {
                return "";
            }
        }

    }
}
