

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
using System;

namespace Scada.BatchCommand
{
    /// <summary>
    /// 定义一个绘制圆角的类型
    /// </summary>
    [Serializable]
    public enum RoundRectStyle
    {
        All,//全部为圆角
        Left_Top,//左上角为圆角
        Left_Bottom,//做下角为圆角
        Right_Top,//右上角为圆角
        Right_Bottom,//右下角为圆角

        Top,//上边为圆角
        Left,//左边为圆角
        Right,//右边为圆角
        Bottom,//下边为圆角

        Left_Top_NoCorner,//左上角不为圆角
        Left_Bottom_NoCorner,//做下角不为圆角
        Right_Top_NoCorner,//右上角不为圆角
        Right_Bottom_NoCorner,//右下角不为圆角

        LT_RB,//左上角和右下角为圆角
        LB_RT//左下角和右上角为圆角

    }
}
