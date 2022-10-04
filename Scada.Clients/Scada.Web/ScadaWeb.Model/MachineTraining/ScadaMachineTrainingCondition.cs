



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
    public enum AlgorithmClassify
    {
        二元分类,
        多类分类,
        异常检测
      
    }
    /// <summary>
    /// 训练工况
    /// </summary>
    [Table("ScadaMachineTrainingCondition")]
    public class ScadaMachineTrainingConditionModel
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
        public string Properties { set; get; }
        //是否二元分类
        [Computed]

        public string AlgorithmClassify { set; get; }
        [Computed]
        public string ServerID { set; get; }

        [Computed]
        public string CommunicateID { set; get; }

        [Computed]
        public string DeviceID { set; get; }
        [Computed]
        public string TrueText { set; get; }
        [Computed]
        public string FalseText { set; get; }

         

        public ScadaMachineTrainingConditionModel() : base()
        {
            StartDate = DateTime.Now.AddMonths(-1);
            EndDate = DateTime.Now;
        }
        public long TaskId
        {
            set; get;
        }
     
        public string ConditionTitle
        { set; get; }
        public string DataFile
        { set; get; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartDate
        {
            set; get;
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDate
        {
            set; get;
        }
        /// <summary>
        /// 标注时间
        /// </summary>
        public DateTime MarkDate
        {
            set; get;
        }
        public string MarkTitle
        {
            set; get;
        }
        /// <summary>
        /// 启用和禁用
        /// </summary>
        public int Enable
        { set; get; }
        /// <summary>
        /// 数据长度
        /// </summary>
        public int DataLength
        { set; get; }

        /// <summary>
        /// 相关参数条件
        /// </summary>

        public string Conditions
        { set; get; }

      
        public string Remark
        { set; get; }
        public int IsTrain
        { set; get; }


    }
    
}
