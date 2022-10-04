using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

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
    [Serializable]
    /// <summary>
    /// 流程图管理的用户
    /// </summary>
    public class ScadaFlowUser: ISerializable, IDisposable
    {
        public override string ToString()
        {
            return _Nickname+"["+ _UserName + "]".ToString();
        }
        public ScadaFlowUser()
        { }
        protected ScadaFlowUser(SerializationInfo info, StreamingContext context)

        {
            #region 自定义属性
            this._UserName = (string)info.GetValue("_UserName", typeof(string));
            this._Nickname = (string)info.GetValue("_Nickname", typeof(string));
            this._Password = (string)info.GetValue("_Password", typeof(string));
            this._Write = (int)info.GetValue("_Write", typeof(int));
            this._Read = (int)info.GetValue("_Read", typeof(int));
          

            #endregion
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("_UserName", this._UserName);
            info.AddValue("_Nickname", this._Nickname);
            info.AddValue("_Password", this._Password);
            info.AddValue("_Write", this._Write);
            info.AddValue("_Read", this._Read);
        }

        public void Dispose()
        {
         
        }
        #region Model

        private string _UserName;
        private string _Nickname;
        private string _Password;
        private int _Write;
        private int _Read;
       
        /// <summary>
        /// 
        /// </summary>
        public string UserName
        {
            set { _UserName = value; }
            get { return _UserName; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Nickname
        {
            set { _Nickname = value; }
            get { return _Nickname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Password
        {
            set { _Password = value; }
            get { return _Password; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Write
        {
            set { _Write = value; }
            get { return _Write; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Read
        {
            set { _Read = value; }
            get { return _Read; }
        }

      
        #endregion Model

    }
}
