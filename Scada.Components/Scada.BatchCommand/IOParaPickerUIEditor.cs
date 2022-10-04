

 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Scada.BatchCommand
{
  

    public class IOParaPickerUIEditor : System.Drawing.Design.UITypeEditor
    {
        public IOParaPickerUIEditor() : base()
        {
          


        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return System.Drawing.Design.UITypeEditorEditStyle.DropDown;
        }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            System.Windows.Forms.Design.IWindowsFormsEditorService iws = (System.Windows.Forms.Design.IWindowsFormsEditorService)provider.GetService(typeof(System.Windows.Forms.Design.IWindowsFormsEditorService));
            if (iws != null)
            {
                IOParaPicker flowIopicker = new IOParaPicker(iws);
               
                flowIopicker.InitTree();
                flowIopicker.SelectItem = (BachCommand_IOPara)value;
                iws.DropDownControl(flowIopicker);
                return flowIopicker.SelectItem;


            }
            return value;
        }

        private void Dlc_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
      
        }
    }
    
    
}
