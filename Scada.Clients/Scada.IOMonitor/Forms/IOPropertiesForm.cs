using Scada.Model;
using Scada.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


 
/*----------------------------------------------------------------
// Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
// 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
// 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
// 请大家尊重作者的劳动成果，共同促进行业健康发展。
// 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
// 创建者：马勇
//----------------------------------------------------------------*/
namespace IOMonitor.Forms
{
    public partial class IOPropertiesForm : DockContent, ICobaltTab
    {
        public IOPropertiesForm(Mediator m)
        {
            mediator = m;
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            this.HideOnClose = true;
            this.CloseButton = false;
            this.Load += IOPropertiesForm_Load;
        }
        public   void SetPara(IO_SERVER server, IO_COMMUNICATION comm, IO_DEVICE device, IO_PARA para)
        {
            this.lbID.Text = para.IO_ID;
            this.lbAddress.Text = para.IO_ADDRESS;
            this.lbDataType.Text = para.IO_DATATYPE;
            if(para.IO_ENABLERANGECONVERSION==1)
            this.lbEnableConvertionRange.Text = "是";
            else
            this.lbEnableConvertionRange.Text = "否";
            if (para.IO_HISTORY == 1)
                this.lbHistory.Text = "是";
            else
                this.lbHistory.Text = "否";
            this.lbDefault.Text = para.IO_INITALVALUE;
            this.lbDecimalPlace.Text = para.IO_OUTLIES;
            this.lbLabel.Text = para.IO_LABEL;
            this.lbName.Text = para.IO_NAME;
            this.lbMinValue.Text = para.IO_MINVALUE;
            this.lbMaxValue.Text = para.IO_MAXVALUE;
            this.lbOne.Text = para.IO_ONE;
            this.lbParaString.Text = para.IO_PARASTRING;
            this.lbIOType.Text = para.IO_POINTTYPE;
            this.lbRangeMax.Text = para.IO_RANGEMAX;
            this.lbRangeMin.Text = para.IO_RANGEMIN;
            this.lbUnit.Text = para.IO_UNIT;
            this.lbZero.Text = para.IO_ZERO;

            if (para.IO_ENABLEALARM == 1)
                this.lbAlarmEnable.Text = "是";
            else
                this.lbAlarmEnable.Text = "否";

            if (para.AlarmConfig.IO_ENABLE_MAX == 1)
                this.lbMaxEnable.Text = "是";
            else
                this.lbMaxEnable.Text = "否";
            if (para.AlarmConfig.IO_ENABLE_MAXMAX == 1)
                this.lbMaxMaxEnable.Text = "是";
            else
                this.lbMaxMaxEnable.Text = "否";
            if (para.AlarmConfig.IO_ENABLE_MINMIN == 1)
                this.lbMinMinEnable.Text = "是";
            else
                this.lbMinMinEnable.Text = "否";

            if (para.AlarmConfig.IO_ENABLE_MIN == 1)
                this.lbMinEnable.Text = "是";
            else
                this.lbMinEnable.Text = "否";


            this.lbMaxAlarmType.Text = para.AlarmConfig.IO_MAX_TYPE;
            this.lbMaxMaxAlarmType.Text = para.AlarmConfig.IO_MAXMAX_TYPE;
            this.lbMinAlarmType.Text = para.AlarmConfig.IO_MIN_TYPE;
            this.lbMinMinAlarmType.Text = para.AlarmConfig.IO_MINMIN_TYPE;

            this.lbMaxMaxAlarmValue.Text = para.AlarmConfig.IO_MAXMAX_VALUE.ToString();
            this.lbMaxAlarmValue.Text = para.AlarmConfig.IO_MAX_VALUE.ToString();
            this.lbMinAlarmValue.Text = para.AlarmConfig.IO_MIN_VALUE.ToString();
            this.lbMinMinAlarmValue.Text = para.AlarmConfig.IO_MINMIN_VALUE.ToString();

            lbAlarmCondition.Text = para.AlarmConfig.IO_CONDITION.ToString();
            //加载通道信息

            this.lbCommID.Text = comm.IO_COMM_ID;
            this.lbCommLabel.Text = comm.IO_COMM_LABEL;
            this.lbCommName.Text = comm.IO_COMM_NAME;
            this.lbCommDriver.Text = comm.IO_COMM_PARASTRING.ToString();


            this.lbDeviceIAddress.Text = device.IO_DEVICE_ADDRESS;
            this.lbDeviceID.Text = device.IO_DEVICE_ID;
            this.lbDeviceILabel.Text = device.IO_DEVICE_LABLE;
            this.lbDeviceIName.Text = device.IO_DEVICE_NAME;
            this.lbDeviceTimeout.Text = device.IO_DEVICE_OVERTIME.ToString();
            this.lbDeviceUpdateCirycle.Text = device.IO_DEVICE_UPDATECYCLE.ToString();
            this.lbDeviceDriver.Text = device.IO_DEVICE_PARASTRING;

        }

        private void IOPropertiesForm_Load(object sender, EventArgs e)
        {
            ControlHelper.FreezeControl(this, true);
        }

        private Mediator mediator = null;
        private string identifier;
        public TabTypes TabType
        {
            get
            {
                return TabTypes.Property;
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

        private void lbDeviceDriver_Click(object sender, EventArgs e)
        {

        }
    }
}
