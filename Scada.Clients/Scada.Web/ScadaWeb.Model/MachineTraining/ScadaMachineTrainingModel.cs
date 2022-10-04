

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
using ScadaWeb.DapperExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScadaWeb.Model
{
    /// <summary>
    /// 机器学习模型
    /// </summary>
    [Table("ScadaMachineTrainingModel")]
    public class ScadaMachineTrainingModel 
    {
        /// <summary>
     /// 主键
     /// </summary>
        [DapperExtensions.Key(true)]
        public long Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        [Display(Name = "创建时间")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 创建用户主键
        /// </summary>
        public long CreateUserId { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        [Display(Name = "修改时间")]
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 修改用户主键
        /// </summary>
        public long UpdateUserId { get; set; }
        /// <summary>
        /// 查询时间范围
        /// </summary>
        [Computed]
        public string StartEndDate { get; set; }
        [Computed]
        public string StartDate
        {
            set;
            get;
        }
        [Computed]
        public string EndDate
        {
            set;
            get;
        }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName
        {
            set; get;
        }
        /// <summary>
        /// 算法
        /// </summary>
        public string Algorithm
        {
            set; get;
        }
        /// <summary>
        /// 训练周期
        /// </summary>
        public int TrainingCycle
        { set; get; }
        /// <summary>
        /// 预测周期
        /// </summary>
        public int ForecastPriod
        {
            set; get;
        }
        /// <summary>
        /// 属性,多个IO字段的属性，采用逗号分隔
        /// </summary>
        public string Properties
        {
            set; get;

        }

     public string AlgorithmType { set; get; }
        public string Remark
        { set; get; }

        private string _comm_id = "";
        public string COMM_ID
        {
            set { _comm_id = value; }
            get { return _comm_id; }

        }
        private string _device_id = "";
        public string DEVICE_ID
        {
            set { _device_id = value; }
            get { return _device_id; }
        }

        private string _server_id = "";
        public string SERVER_ID
        {
            set { _server_id = value; }
            get { return _server_id; }

        }

        private string _SERVER_NAME = "";
        public string SERVER_NAME
        {
            set { _SERVER_NAME = value; }
            get { return _SERVER_NAME; }

        }
        public string TrueText { set; get; } = "是";
        public string FalseText { set; get; } = "否";

        public int _isTrain = 0;//是否已经训练
        public int IsTrain
        {
            set { _isTrain = value; }
            get { return _isTrain; }
        }
        public string _detection;

        public string Detection
        {
            set { _detection = value; }
            get { return _detection; }
        }
        [Computed]
        public string Detection5
        {
            set;
            get;
        }
        [Computed]
        public string Detection6
        {
            set;
            get;
        }
        [Computed]
        public string Detection7
        {
            set;
            get;
        }
        [Computed]
        public string Detection8
        {
            set;
            get;
        }
        [Computed]
        public string Detection9
        {
            set;
            get;
        }
        [Computed]
        public string Detection10
        {
            set;
            get;
        }
    }
 
}
