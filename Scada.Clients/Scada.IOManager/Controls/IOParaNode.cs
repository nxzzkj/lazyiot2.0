﻿using Scada.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scada.DBUtility;


 
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
   public class IOParaNode:TreeNode
    {
        public IO_PARA IOPara = new IO_PARA();
        public IOParaNode()
        {
            this.ContextMenu = new  ContextMenu();
            this.ContextMenu.MenuItems.Add(new MenuItem("删除") {  });
            this.ContextMenu.MenuItems.Add(new MenuItem("修改") { });
            this.ContextMenu.MenuItems[0].Click += IOParaNode_Click;
            this.ContextMenu.MenuItems[1].Click += IOParaNode_Click;
            IOPara.IO_ID = GUIDToNormalID.GuidToLongID().ToString();
            this.SelectedImageIndex = 3;
            this.StateImageIndex = 3;
            this.ImageIndex = 3;
        }

        private void IOParaNode_Click(object sender, EventArgs e)
        {
            
        }
        public override string ToString()
        {
            return Text.ToString();
        }
    }
}
