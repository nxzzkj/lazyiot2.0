using Scada.DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScadaFlowDesign.Control
{

 
    public class TreeCustumGroupNode : TreeNode
    {
     
        
        public TreeCustumGroupNode()
        {
            this.SelectedImageIndex = 0;
            this.ImageIndex = 0;
           

        }

 

        public string ID
        {
            set; get;
        }
        private string _Title = "";
        public string Title
        {
            set
            {
                _Title = value;
                this.Text = _Title;

               
            }
            get { return _Title; }
        }
    }
    public class TreeCustumElementNode : TreeNode
    {
        public Action<string, string> OpenElement;
       
        public TreeCustumElementNode()
        {
            this.SelectedImageIndex = 1;
            this.ImageIndex = 1;
         
           
        }
       
        public string GID
        {
            set; get;
        }
        public string ID
        {
            set; get;
        }
        private string _Title = "";
        public string Title
        {
            set
            {
                _Title = value;
                this.Text = _Title;
              
            }
            get { return _Title; }
        }
    }
}
