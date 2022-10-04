using System;
namespace Scada.Model
{
    #region << 版 本 注 释 >>
    /*----------------------------------------------------------------
    // Copyright (C) 2017 宁夏众智科技有限公司 版权所有。 
    // 开源版本代码仅限个人技术研究使用，未经作者允许严禁商用。宁夏众智科技有限公司是一家油田自动化行业经营多年的软件开发公司，公司承接OA、工控、组态、微信小程序等开发。
    // 对于本系统的相关版权归属宁夏众智科技所有，如果本系统使用第三方开源模块，该模块版权归属原作者所有。
    // 请大家尊重作者的劳动成果，共同促进行业健康发展。
    // 相关技术交流群89226196 ,作者QQ:249250126 作者微信18695221159 邮箱:my820403@126.com
    // 创建者：马勇
    //----------------------------------------------------------------*/
    #endregion

    /// <summary>
    /// IO_COMMANDS:下置命令类
    /// </summary>
    [Serializable]
    public partial class IO_COMMANDS: IDisposable
    {
        public IO_COMMANDS()
        { }
        #region Model
        private string _command_id = "";
        private string _command_value = "";
        private string _command_date = "";
        private string _IO_SERVER_id = "";
        private string _io_comm_id = "";
        private string _io_device_id = "";
        private string _io_id = "";
        private string _io_name = "";
        private string _io_label = "";
        private string _command_result = "false";
        public string IO_LABEL
        {
            set { _io_label = value; }
            get { return _io_label; }
        }
        public string IO_NAME
        {
            set { _io_name = value; }
            get { return _io_name; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string COMMAND_ID
        {
            set { _command_id = value; }
            get { return _command_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string COMMAND_VALUE
        {
            set { _command_value = value; }
            get { return _command_value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string COMMAND_DATE
        {
            set { _command_date = value; }
            get { return _command_date; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IO_SERVER_ID
        {
            set { _IO_SERVER_id = value; }
            get { return _IO_SERVER_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IO_COMM_ID
        {
            set { _io_comm_id = value; }
            get { return _io_comm_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IO_DEVICE_ID
        {
            set { _io_device_id = value; }
            get { return _io_device_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IO_ID
        {
            set { _io_id = value; }
            get { return _io_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string COMMAND_RESULT
        {
            set { _command_result = value; }
            get { return _command_result; }
        }
        private string _command_user = "";
        public string COMMAND_USER
        {
            set { _command_user = value; }
            get { return _command_user; }
        }
        public string IO_DEVICE_NAME
        {
            set; get;
        }
        public string IO_COMM_NAME
        {
            set; get;
        }
        public string GetCommandString()
        {

            string str = "IO_ID:" + IO_ID + "#COMMAND_ID:" + COMMAND_ID + "#COMMAND_VALUE:" + COMMAND_VALUE + "#COMMAND_DATE:" + COMMAND_DATE.Replace("#", "//").Replace(":", "\\") + "#IO_SERVER_ID:" + IO_SERVER_ID + "#IO_COMM_ID:" + IO_COMM_ID + "#IO_DEVICE_ID:" + IO_DEVICE_ID + "#COMMAND_RESULT:" + COMMAND_RESULT + "#COMMAND_USER:" + COMMAND_USER + "#IO_LABEL:" + IO_LABEL + "#IO_NAME:" + IO_NAME;
            return str;
        }

        public void Dispose()
        {
      
        }

        #endregion Model

    }
}

