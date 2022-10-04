
using Scada.Controls;
using ScadaFlowDesign;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScadaFlowDesign.Control;
 
using Scada.FlowGraphEngine;
 
using Scada.FlowGraphEngine.GraphicsShape;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Controls;
using Scada.DBUtility;
using Scada.FlowGraphEngine.GraphicsMap;
using ScadaFlowDesign.Dialog;
using Scada.FlowGraphEngine.GraphicsCusControl;
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
    public partial class ShapeForm : DockContent, ICobaltTab
    {

        private Mediator mediator = null;

        public ShapeForm(Mediator m)
        {

            mediator = m;
            this.HideOnClose = true;
            InitializeComponent();
            this.Load += (s, e) =>
            {
                this.Width = 250;
                //加载符号标志
                InitSymbolCombox();

                SVGAnalysisIndustrySymbol.LoadedSymbol = (string path) =>
                {

                    return Task.Run(() =>
                    {
                        this.mediator.LogForm.AppendLogItem(path);
                    });
                };
                SVGAnalysisIndustrySymbol.EndLoadedSymbol = (string path) =>
                {
                    return Task.Run(() =>
                    {
                        EndLoadedSymbol(path);
                    });
                };
                CustumShapeManage.LoadedSymbol = (string path) =>
                {
                 

                    return Task.Run(() =>
                    {
                        this.mediator.LogForm.AppendLogItem(path);
                    });
                };

                CustumShapeManage.LoadedEndSymbol = () =>
                {
                    this.mediator.LogForm.AppendLogItem("加载自定义组件成功!");
                    return LoadCustumShape();

                };



            };



        }
        public Task LoadCustumShape()
        {
         
         return  Task.Run(() =>
            {
            
                this.treeViewCustum.Nodes.Clear();
                for (int i = 0; i <  CustumShapeManage.Custum.Group.Count; i++)
                {
                    TreeCustumGroupNode groupNode = new TreeCustumGroupNode();

                    groupNode.ID =  CustumShapeManage.Custum.Group[i].ID;
                    groupNode.Title =  CustumShapeManage.Custum.Group[i].Name;

                    for (int j = 0; j <  CustumShapeManage.Custum.Group[i].Elements.Count; j++)
                    {
                        TreeCustumElementNode elementNode = new TreeCustumElementNode();
                        elementNode.ID =  CustumShapeManage.Custum.Group[i].Elements[j].ID;
                        elementNode.Title =  CustumShapeManage.Custum.Group[i].Elements[j].Name;
                        elementNode.GID =  CustumShapeManage.Custum.Group[i].ID;


                        groupNode.Nodes.Add(elementNode);

                    }
                    groupNode.ExpandAll();
                    groupNode.Expand();


                    treeViewCustum.BeginInvoke(new Action(delegate
                    {
                        this.treeViewCustum.Nodes.Add(groupNode);


                    }));

                }

            });
        }
        private void InitSymbolCombox()
        {
            comboBoxSymbolStyle.SelectedIndexChanged += ComboBoxSymbolStyle_SelectedIndexChanged;
            comboBoxSymbolStyle.Items.Clear();
            comboBoxSymbolStyle.Items.Add(SVG_SymbolClassification.Arrow);
            comboBoxSymbolStyle.Items.Add(SVG_SymbolClassification.Basic);
            comboBoxSymbolStyle.Items.Add(SVG_SymbolClassification.Dialog_Balloon);
            comboBoxSymbolStyle.Items.Add(SVG_SymbolClassification.FlowChart);
            comboBoxSymbolStyle.Items.Add(SVG_SymbolClassification.Game);
            comboBoxSymbolStyle.Items.Add(SVG_SymbolClassification.Math);
            comboBoxSymbolStyle.Items.Add(SVG_SymbolClassification.Music);
            comboBoxSymbolStyle.Items.Add(SVG_SymbolClassification.Nature);
            comboBoxSymbolStyle.Items.Add(SVG_SymbolClassification.Object);
            comboBoxSymbolStyle.Items.Add(SVG_SymbolClassification.Social);
            comboBoxSymbolStyle.Items.Add(SVG_SymbolClassification.Symbol);
            comboBoxSymbolStyle.Items.Add(SVG_SymbolClassification.Ui);
            comboBoxSymbolStyle.Items.Add(SVG_SymbolClassification.Weather);




        }
        private void InitDrawSymbolIconButtons()
        {
            if (comboBoxSymbolStyle.SelectedItem != null)
            {
                this.flowLayoutPanelSymbol.Controls.Clear();
                SVG_SymbolClassification classification = (SVG_SymbolClassification)comboBoxSymbolStyle.SelectedItem;

                var eles = SVG_SymbolIconManager.SymbolIcons.FindAll(x => x.Classification == classification);
                foreach (SVG_SymbolIcon ele in eles)
                {
                    SCADAShapeButton button = new SCADAShapeButton();
                    button.Width = 60;
                    button.Height = 60;
                    button.Image = ele.Icon;

                    button.Click += ButtonSymbolIcon_Click;
                    button.ShapeElement = ShapeElement.SVG_SymbolShape;
                    button.Tag = ele;
                    this.flowLayoutPanelSymbol.Controls.Add(button);

                }


            }
        }
        private void ComboBoxSymbolStyle_SelectedIndexChanged(object sender, EventArgs e)
        {

            InitDrawSymbolIconButtons();
        }

        private void ButtonSymbolIcon_Click(object sender, EventArgs e)
        {
            if (this.mediator.ActiveWork == null)
                return;
            if (sender != null)
            {
                Clipboard.Clear();




                SCADAShapeButton selectShape = (SCADAShapeButton)sender;
                SVG_SymbolIcon icon = (SVG_SymbolIcon)selectShape.Tag;
                this.Cursor = new Cursor(selectShape.GetCursor());
                Clipboard.Clear();
                DataObject data = new DataObject("Scada.FlowGraphEngine.GraphicsMap.Shape.Draw", icon);
                Clipboard.SetDataObject(data);
                if (this.mediator.ActiveWork != null)
                {
                    ((WorkForm)this.mediator.ActiveWork).GraphControl.Cursor = this.Cursor;
                    SVG_SymbolShape shape = new SVG_SymbolShape();
                    shape.Rectangle = new RectangleF(0, 0, 36, 36);
                    IDataObject newdata = Clipboard.GetDataObject();
                    if (newdata.GetDataPresent("Scada.FlowGraphEngine.GraphicsMap.Shape.Draw"))
                    {
                        SVG_SymbolIcon bundle = newdata.GetData("Scada.FlowGraphEngine.GraphicsMap.Shape.Draw") as SVG_SymbolIcon;

                        shape.Icon = bundle;

                    }
                        ((WorkForm)this.mediator.ActiveWork).GraphControl.selector = new CommonSelector(((WorkForm)this.mediator.ActiveWork).GraphControl, shape);

                    Clipboard.Clear();
                }
            }

        }







        private void Button_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.mediator.ActiveWork == null)
                return;
            System.Windows.Forms.Label button = sender as System.Windows.Forms.Label;
            if (e.Clicks == 2)
            {
                button.BackColor = Color.White;
                if (this.mediator.ActiveWork != null && button.Tag != null)
                {
                    SVG_TemplateShape newShape = null;
                    FileStream fs = null;
                    try
                    {


                        fs = new FileStream(Application.StartupPath + "/ScadaTemplate/TemplateShapes/" + button.Tag.ToString(), FileMode.Open);
                        fs.Seek(0, SeekOrigin.Current);
                        IFormatter formatter = new BinaryFormatter();
                        while (fs.Position < fs.Length)
                        {
                            try
                            {
                                newShape = (SVG_TemplateShape)formatter.Deserialize(fs);
                            }
                            catch
                            {
                                continue;
                            }

                        }

                    }
                    catch (Exception emx)
                    {
#if DEBUG
                        throw emx;
#else
                               MessageBox.Show(this, emx.Message);
#endif
                    }
                    finally
                    {
                        if (fs != null)
                            fs.Close();
                    }

                    try
                    {


                        if (newShape == null)
                        {
                            return;
                        }
                        Point cp = new Point();

                        cp.X = ((WorkForm)this.mediator.ActiveWork).GraphControl.ClientRectangle.Width / 2 - Convert.ToInt32(newShape.TplShape.Width) / 2;
                        cp.Y = ((WorkForm)this.mediator.ActiveWork).GraphControl.ClientRectangle.Height / 2 - Convert.ToInt32(newShape.TplShape.Height) / 2;

                        ///重新获取GUID

                        GetTemplateShape(newShape.TplShape, newShape.shapes);
                        for (int i = 0; i < newShape.shapes.Count; i++)
                        {
                            //此处要做无限循环处理,读取容器内容
                            newShape.shapes[i].Site = ((WorkForm)this.mediator.ActiveWork).GraphControl;
                            newShape.shapes[i].Layer = ((WorkForm)this.mediator.ActiveWork).GraphControl.BasicLayer;
                            newShape.shapes[i].LayerName = ((WorkForm)this.mediator.ActiveWork).GraphControl.BasicLayer.Name;
                            newShape.shapes[i].GID = ((WorkForm)this.mediator.ActiveWork).GraphControl.Abstract.GID;

                        }
                        newShape.TplShape.GenerateNewUID();
                        newShape.TplShape.Site = ((WorkForm)this.mediator.ActiveWork).GraphControl;
                        newShape.TplShape.Layer = ((WorkForm)this.mediator.ActiveWork).GraphControl.BasicLayer;
                        newShape.TplShape.LayerName = ((WorkForm)this.mediator.ActiveWork).GraphControl.BasicLayer.Name;
                        newShape.TplShape.GID = ((WorkForm)this.mediator.ActiveWork).GraphControl.Abstract.GID;



                        for (int i = 0; i < newShape.shapes.Count; i++)
                        {
                            newShape.shapes[i].Locked = false;

                            ((WorkForm)this.mediator.ActiveWork).GraphControl.AddShape(newShape.shapes[i], AddShapeType.Create);

                        }
                         ((WorkForm)this.mediator.ActiveWork).Invalidate();
                        newShape.TplShape.MoveOffiset(cp.X - newShape.TplShape.X, cp.Y - newShape.TplShape.Y);


                    }
                    catch (Exception emx)
                    {

#if DEBUG
                        throw emx;
#else
                                     MessageBox.Show(this, emx.Message);
#endif

                    }
                }
            }
        }

        private void GetTemplateShape(FlowShape shape, List<FlowShape> shapes)
        {
            if (shape.GetType() == typeof(SVG_TabPanelShape))//tab页面
            {
                SVG_TabPanelShape container = (SVG_TabPanelShape)shape;
                for (int c = 0; c < container.TabPages.Count; c++)
                {
                    for (int s = container.TabPages[c].Shapes.Count - 1; s >= 0; s--)
                    {
                        FlowShape existShape = shapes.Find(x => x.UID == container.TabPages[c].Shapes[s].UID);
                        if (existShape != null)
                        {

                            container.TabPages[c].Shapes[s].Shape = existShape;
                            if (container.TabPages[c].Shapes[s].Shape != null)
                            {
                                container.TabPages[c].Shapes[s].ChangedUid();
                                container.TabPages[c].Shapes[s].Shape.Name = "Element" + container.TabPages[c].Shapes[s].UID;
                                GetTemplateShape(container.TabPages[c].Shapes[s].Shape, shapes);
                            }

                        }
                        else
                        {
                            container.TabPages[c].Shapes.RemoveAt(s);
                        }
                    }

                }

            }
            else if (shape.GetType() == typeof(Combination))//组合体
            {
                Combination container = (Combination)shape;

                for (int c = container.Shapes.Count - 1; c >= 0; c--)
                {
                    FlowShape existShape = shapes.Find(x => x.UID == container.Shapes[c]);
                    if (existShape != null)
                    {


                        string uid = existShape.GenerateNewUID();

                        existShape.Name = "Element" + uid;
                        container.Shapes[c] = uid;



                    }
                    else
                    {
                        container.Shapes.RemoveAt(c);
                    }

                }

            }


        }
        private void EndLoadedSymbol(string path)
        {
            if (this.IsHandleCreated)
            {
                this.BeginInvoke(new EventHandler(delegate
            {
                comboBoxSymbol.Items.Clear();
                for (int i = 0; i < SVGAnalysisIndustrySymbol.Groups.Count; i++)
                {
                    comboBoxSymbol.Items.Add(SVGAnalysisIndustrySymbol.Groups[i]);
                }
                comboBoxSymbol.SelectedIndex = 0;

            }));
            }
         
        }



        private string identifier;
        public TabTypes TabType
        {
            get
            {
                return TabTypes.Shape;
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
        private void ButtonShape_Click(object sender, EventArgs e)
        {
            if (this.mediator.ActiveWork == null)
                return;
            SCADAShapeButton button = sender as SCADAShapeButton;
            if (this.mediator.ActiveWork != null && (this.mediator.ActiveWork is WorkForm))
            {
                WorkForm mWorkForm = this.mediator.ActiveWork as WorkForm;
                mWorkForm.SetDrawShape(button.ShapeElement);
                ((WorkForm)this.mediator.ActiveWork).GraphControl.Cursor = DefaultCursor;
            }


        }

        private void comboBoxSymbol_SelectedIndexChanged(object sender, EventArgs e)
        {
          
            if (comboBoxSymbol.SelectedItem != null)
            {
                IndustrySymbolGroup FieldSymbol = (IndustrySymbolGroup)comboBoxSymbol.SelectedItem;
                flowLayoutPanelStatics.Controls.Clear();
                for (int i = 0; i < FieldSymbol.Shapes.Count; i++)
                {
                    SCADAShapeButton button = new SCADAShapeButton();
                    button.Width = 60;
                    button.Height = 60;
                    button.Image = FieldSymbol.Shapes[i].Bmp;
                    button.ShapeName = FieldSymbol.Shapes[i].Title;
                    button.Click += ButtonStaticIcon_Click;

                    button.Tag = FieldSymbol.Shapes[i];
                    this.flowLayoutPanelStatics.Controls.Add(button);

                }
            }
        }
        private void ButtonStaticIcon_Click(object sender, EventArgs e)
        {
            if (this.mediator.ActiveWork == null)
                return;
            SCADAShapeButton button = (SCADAShapeButton)sender;
            if (sender != null && button.Tag != null)
            {

                DataObject data = new DataObject("Scada.FlowGraphEngine.GraphicsMap.Shape.Draw", button.Tag);


                this.Cursor = Cursors.Default;
                if (this.mediator.ActiveWork != null)
                {
                    ((WorkForm)this.mediator.ActiveWork).GraphControl.Cursor = this.Cursor;
                    SVG_StaticShape shape = new SVG_StaticShape();
                    shape.SvgShape = data.GetData("Scada.FlowGraphEngine.GraphicsMap.Shape.Draw") as ScadaIndustrySymbol_Shape;
                    ((WorkForm)this.mediator.ActiveWork).GraphControl.selector = new CommonSelector(((WorkForm)this.mediator.ActiveWork).GraphControl, shape);
                }

            }

        }

        private void flowLayoutPanelNormal_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.mediator.ActiveWork == null)
                return;
            this.Cursor = Cursors.Default;
            ((WorkForm)this.mediator.ActiveWork).GraphControl.selector = null;
        }

        private void flowLayoutPanelNormal_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.mediator.ActiveWork == null)
                return;
            this.Cursor = Cursors.Default;
            ((WorkForm)this.mediator.ActiveWork).GraphControl.selector = null;
        }

        private void 自定义组件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Scada.IndustryDesigner.DesignerForm designerForm = new Scada.IndustryDesigner.DesignerForm();
            designerForm.ShowDialog(this);
            LoadCustumShape();
        }

        private void treeViewCustum_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (this.mediator.ActiveWork == null)
                return;
            if (e.Node is TreeCustumElementNode elementNode)
            {

                SVG_CustumShape cus = new SVG_CustumShape();
                cus.GroupID = elementNode.GID;
                cus.EID = elementNode.ID;
                ((WorkForm)this.mediator.ActiveWork).GraphControl.selector = new CommonSelector(((WorkForm)this.mediator.ActiveWork).GraphControl, cus);
                if (((WorkForm)this.mediator.ActiveWork).GraphControl.selector != null)
                    ((WorkForm)this.mediator.ActiveWork).GraphControl.selector.SelectorDrawEnd = (FlowShape sop) =>
                    {

                        ((WorkForm)this.mediator.ActiveWork).GraphSelectorDrawEnd(sop);


                    };
                ((WorkForm)this.mediator.ActiveWork).GraphControl.selector.SelectorUpdated = (FlowShape sop) =>
                {

                    ((WorkForm)this.mediator.ActiveWork).GraphSelectorUpdate(sop);


                };

                return;
            }
        }

        private void 重新加载库ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadCustumShape();
        }
    }
}
