using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
 
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
 
using System.Windows.Forms.Design;


 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
namespace IOManager.Controls
{


 
    [Designer(typeof(OuterControlDesigner))]
    public partial class WizardTabControl : UserControl
    {
        public event EventHandler ButtonOK;
        public event EventHandler ButtonPrevious;
        public event EventHandler ButtonNext;
        public event EventHandler ButtonCancel;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TabControl TabControl
        {

            get { return mTabControl; }
            set { mTabControl = value; }
        }
       
        public List<TabPage> Pages = new List<TabPage>();
        private int mTabPageIndex = 0;
        public int TabPageIndex
        {

            set
            {
                mTabPageIndex = value;
                if (Pages.Count == 0)
                    mTabPageIndex= 0;
           
      
                try
                {
                    if(Pages.Count>0)
                    {
                        if (mTabPageIndex <= 0)
                            mTabPageIndex = 0;
                        if (mTabPageIndex > Pages.Count - 1)
                            mTabPageIndex = Pages.Count - 1;
                        this.btCancel.Visible = true;
                        if (Pages.Count <= 1)
                        {
                            this.btOK.Visible = true;
                        }
                        else
                        {
                            this.btOK.Visible = false;
                        }

                        for (int i = Pages.Count - 1; i >= 0; i--)
                        {
                            Pages[i].Parent = null;


                        }
                        this.btNext.Visible = false;
                        this.btPre.Visible = true;


                        if (mTabPageIndex == 0)
                        {
                            this.btOK.Visible = false;
                            this.btNext.Visible = true;
                            this.btPre.Visible = false;

                        }
                        else if (mTabPageIndex == Pages.Count - 1)
                        {
                            this.btOK.Visible = true;
                            this.btNext.Visible = false;
                            this.btPre.Visible = true;


                        }
                        else
                        {
                            this.btNext.Visible = true;
                            this.btPre.Visible = true;
                            this.btOK.Visible = false;
                        }

                        if (Pages.Count > 0)
                        {
                            Pages[mTabPageIndex].Parent = this.mTabControl;
                        }


                    }



                }
                catch
                {

                }
                   
           
            }
            get { return mTabPageIndex; }
        }
        public void InitWizard()
        {
            for(int i=0;i<this.mTabControl.TabPages.Count;i++)
            {
             
                Pages.Add(this.mTabControl.TabPages[i]);
               
            }
            for (int i = this.mTabControl.TabPages.Count-1; i >=0; i--)
            {
                this.mTabControl.TabPages[i].Parent = null;
            

            }
            if (Pages.Count > 0)
                Pages[0].Parent = this.mTabControl;
          
                this.btNext.Visible = true;
            this.btPre.Visible = false;
            this.btOK.Visible = false;
            this.btCancel.Visible = true;

        }


        public WizardTabControl()
        {
            InitializeComponent();
        
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            if (ButtonOK != null)
            {
                ButtonOK(sender,e);
            }
        }

        private void btPre_Click(object sender, EventArgs e)
        {
            TabPageIndex--;
            if (ButtonPrevious != null)
            {
             
                ButtonPrevious(sender, e);
            }
        }

        private void btNext_Click(object sender, EventArgs e)
        {
            TabPageIndex++;
            if (ButtonNext != null)
            {
             
                ButtonNext(sender, e);
            }
        }
        
        private void btCancel_Click(object sender, EventArgs e)
        {
            TabPageIndex = 0;
            if (ButtonCancel != null)
            {
              
                ButtonCancel(sender, e);
            }
        }
    }
}
