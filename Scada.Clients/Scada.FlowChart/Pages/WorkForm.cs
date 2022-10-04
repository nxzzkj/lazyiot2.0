using Scada.FlowGraphEngine;
using Scada.Controls;
using Scada.FlowGraphEngine.GraphicsEngine;
using Scada.FlowGraphEngine.GraphicsMap;
using Scada.FlowGraphEngine.GraphicsShape;
using Scada.FlowGraphEngine.GraphicsShape.PipeLine;
using Scada.Model;
using ScadaFlowDesign.Core;
 
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Scada.DBUtility;
using Scada.IndustryDesigner;



/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
namespace ScadaFlowDesign
{
    public partial class WorkForm : DockContent, ICobaltTab
    {
        private Mediator mediator = null;
        public WorkForm(Mediator m, float mapwidth, float mapheight,string title)
        {
            InitializeComponent();
            this.HideOnClose = true;
            this.KeyPreview = true;//为了使OnKeyDown事件有效
            mediator = m;
          
            this.Load += (s, e) =>
            {

                this.graphControl.InitGraph(mapwidth, mapheight, title);
                this.graphControl.OnShowProperties += GraphControl_OnShowProperties;
                graphControl.OnGraphMouseInfo += GraphControl_OnGraphMouseInfo;
                graphControl.StateText = this.mediator.Parent.ToolStatusInfo;
                this.graphControl.MakeCombinationShape = new Action<SVG_TemplateShape, string>((template, filepath) =>
                {
                

                });

            };



        }

        private void GraphControl_OnGraphMouseInfo(object sender, FlowShape shape)
        {
            FlowServiceGlobel.Views = null;
            List<FlowGraphAbstract> list = new List<FlowGraphAbstract>();
            List<ScadaFlowUser> list2 = new List<ScadaFlowUser>();
            List<ScadaConnectionBase> list3 = new List<ScadaConnectionBase>();
            foreach (IOFlowProject project in IOFlowManager.Projects)
            {
                list.AddRange(project.GraphList);
                list2.AddRange(project.FlowUsers);
                list3.AddRange(project.ScadaConnections);
            }
            FlowServiceGlobel.Views = list;
            FlowServiceGlobel.Users = list2;
            FlowServiceGlobel.Connections = list3;
            if (sender != null)
            {
                this.mediator.Parent.SetHoverInformation(sender.ToString());
            }
            if (shape != null)
            {
                if (shape.GetType() == typeof(Combination))
                {
                    this.mediator.Parent.SetCombination();
                }
                else
                {
                    this.mediator.Parent.SetUnCombination();
                }
                this.mediator.Parent.SetUnLock(shape.Locked);
            }
            Task.Run(delegate {
                FlowIOServerGlobel.Server = IOFlowManager.FlowDataBaseManager.IOServer;
                FlowIOServerGlobel.Communications = IOFlowManager.FlowDataBaseManager.IOCommunications;
            });
        }

        public FlowGraphControl GraphControl
        {
            get { return graphControl; }
        }
        public void SetDrawShape(ShapeElement select)
        {
            switch (select)
            {


                case ShapeElement.SVG_GroupPanelHeadShape:
                    {
                        SVG_GroupPanelHeadShape shapeModel = new SVG_GroupPanelHeadShape
                        {
                           
                        };
                        this.graphControl.selector = new CommonSelector(this.graphControl, shapeModel);
                        return;
                    }
                case ShapeElement.SVG_GroupPanelTextShape:
                    {
                        SVG_GroupPanelTextShape shapeModel = new SVG_GroupPanelTextShape
                        {
                             
                        };
                        this.graphControl.selector = new CommonSelector(this.graphControl, shapeModel);
                        return;
                    }
                case ShapeElement.SVG_TabPanelShape:
                    {
                        SVG_TabPanelShape shapeModel = new SVG_TabPanelShape
                        {
                            
                        };
                        this.graphControl.selector = new CommonSelector(this.graphControl, shapeModel);
                        return;
                    }
                case ShapeElement.SVG_GeneralShape:
                    {
                        SVG_GeneralShape shapeModel = new SVG_GeneralShape
                        {
                          
                        };
                        this.graphControl.selector = new CommonSelector(this.graphControl, shapeModel);
                        return;
                    }
                


            }
            this.graphControl.SetDrawShape(select);
            if (this.graphControl.selector != null)
                this.graphControl.selector.SelectorDrawEnd = (FlowShape sop) => {

                    GraphSelectorDrawEnd(sop);


                };
            this.graphControl.selector.SelectorUpdated = (FlowShape sop) => {

                GraphSelectorUpdate(sop);


            };
            

        }
        public void GraphSelectorDrawEnd(FlowShape flowShape)
        {
            if (flowShape is SVG_CustumShape custumShape)
            {
                ElementShape elementShape = CustumShapeManage.Custum.GetElementShape(custumShape.GroupID, custumShape.EID);
                if (elementShape != null)
                {
                    custumShape.AddElement(elementShape);
                }
            }
            else if (flowShape is SVG_CloseCurve  _CloseCurve)
            {
                _CloseCurve.CalcCtrlPoint();
            }
            else if (flowShape is SVG_NoCloseCurve  _NoCloseCurve)
            {
                _NoCloseCurve.CalcCtrlPoint();
            }
        }
        public void GraphSelectorUpdate(FlowShape flowShape)
        {
            if (flowShape is SVG_CloseCurve _CloseCurve)
            {
                _CloseCurve.CalcCtrlPoint();
            }
            else if (flowShape is SVG_NoCloseCurve _NoCloseCurve)
            {
                _NoCloseCurve.CalcCtrlPoint();
            }
        }
        /// <summary>
        /// 保存页面
        /// </summary>
        public void SavePage()
        {

        }
        /// <summary>
        /// 加载页面
        /// </summary>
        public void LoadPage()
        {

        }


        private void GraphControl_OnShowProperties(object sender, object[] props)
        {
            this.mediator.PropertiesForm.ShowProperties(sender, props);
        }

        private string identifier;
        public TabTypes TabType
        {
            get
            {
                return TabTypes.WorkArea;
            }
        }
        public string TabIdentifier
        {
            get
            {
                return identifier;
            }
            set
            {
                identifier = value;
            }
        }
        //打开文件后返回的消息
        private void graphControl_OnDiagramOpened(object sender, System.IO.FileInfo info)
        {

        }
        //清空所有图键返回的消息
        private void graphControl_OnClear(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 保存图件返回的消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="info"></param>
        private void graphControl_OnSavingDiagram(object sender, System.IO.FileInfo info)
        {

        }
        /// <summary>
        /// 新增加一个图元返回的消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="shape"></param>
        private void graphControl_OnShapeAdded(object sender, FlowShape shape)
        {
            IOFlowManager.AddLogToMainLog("增加图元" + shape.Name);
        }
        /// <summary>
        /// 删除一个图元返回的消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="shape"></param>
        private void graphControl_OnShapeRemoved(object sender, FlowShape shape)
        {
            IOFlowManager.AddLogToMainLog("删除图元" + shape.Name);
        }
        /// <summary>
        /// 鼠标相关通知消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       

        private void graphControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.mediator.ShapeForm.Cursor = Cursors.Default;
        }
    }


}
