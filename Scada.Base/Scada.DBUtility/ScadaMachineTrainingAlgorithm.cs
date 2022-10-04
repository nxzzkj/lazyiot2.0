using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Scada.DBUtility
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
    /// 机器学习算法
    /// </summary>
    public enum ScadaMachineTrainingAlgorithm
    {
        //二元分类训练算法,支持重新训练

        [Category("二元分类")]
        [Description("二元分类训练-平均感知器训练的线性二元分类模型预测")]
        AveragedPerceptronTrainer,
        [Category("二元分类")]
        [Description("二元分类训练-使用随机双坐标提升方法训练二进制逻辑回归分类模型")]
        SdcaLogisticRegressionBinaryTrainer,//使用随机双坐标提升方法训练二进制逻辑回归分类模型
        [Category("二元分类")]
        [Description("二元分类训练-随机双重坐标上升方法为二元逻辑回归分类模型定型")]
        SdcaNonCalibratedBinaryTrainer,//随机双重坐标上升方法为二元逻辑回归分类模型定型
        [Category("二元分类")]
        [Description("二元分类训练-通过符号随机梯度下降定型的线性二元分类模型预测目标")]
        SymbolicSgdLogisticRegressionBinaryTrainer, //通过符号随机梯度下降定型的线性二元分类模型预测目标
        [Category("二元分类")]
        [Description("二元分类训练-L-BFGS 方法训练的线性逻辑回归模型预测目标")]
        LbfgsLogisticRegressionBinaryTrainer,// L-BFGS 方法训练的线性逻辑回归模型预测目标
        [Category("二元分类")]
        [Description("二元分类训练-定型使用LightGBM 的提升决策树二元分类模型")]
        LightGbmBinaryTrainer,//定型使用LightGBM 的提升决策树二元分类模型
        [Category("二元分类")]
        [Description("二元分类训练-FastTree 训练决策树二元分类模型")]
        FastTreeBinaryTrainer,//FastTree 训练决策树二元分类模型
        [Category("二元分类")]
        [Description("二元分类训练-训练使用 Fast 林的决策树二元分类模型")]
        FastForestBinaryTrainer,//训练使用 Fast 林的决策树二元分类模型
        [Category("二元分类")]
        [Description("二元分类训练-定型具有通用化加法模型 (GAM) 的二元分类模型")]
        GamBinaryTrainer,//定型具有通用化加法模型 (GAM) 的二元分类模型
        [Category("二元分类")]
        [Description("二元分类训练-随机渐变方法进行训练的、使用字段感知因子分解计算机模型预测目标")]
        FieldAwareFactorizationMachineTrainer,// 随机渐变方法进行训练的、使用字段感知因子分解计算机模型预测目标
        [Category("二元分类")]
        [Description("二元分类训练-预测使用二进制分类模型的目标")]
        PriorTrainer,//预测使用二进制分类模型的目标
        [Category("二元分类")]
        [Description("二元分类训练-线性 SVM 定型的线性二元分类模型预测目标")]
        LinearSvmTrainer,//线性 SVM 定型的线性二元分类模型预测目标
                         //多类分类训练
        [Category("多类分类")]
        [Description("多类分类训练-使用 LightGBM 训练提升决策树多类分类模型")]
        LightGbmMulticlassTrainer,//使用 LightGBM 训练提升决策树多类分类模型
        [Category("多类分类")]
        [Description("多类分类训练-最大平均信息量多类分类器预测目标")]
        SdcaMaximumEntropyMulticlassTrainer,//最大平均信息量多类分类器预测目标
        [Category("多类分类")]
        [Description("多类分类训练-线性多类分类器预测目标")]
        SdcaNonCalibratedMulticlassTrainer,// 线性多类分类器预测目标
        [Category("多类分类")]
        [Description("多类分类训练-使用 L-BFGS 方法训练的最大平均信息量多类分类器预测目标")]
        LbfgsMaximumEntropyMulticlassTrainer,//使用 L-BFGS 方法训练的最大平均信息量多类分类器预测目标
        [Description("多类分类训练-定型支持二进制功能值的多类 Naive Bayes 模型")]
        [Category("多类分类")]
        NaiveBayesMulticlassTrainer,//定型支持二进制功能值的多类 Naive Bayes 模型
        [Category("多类分类")]
        [Description("多类分类训练-定型使用指定的二元分类器的一对一多类分类")]
        OneVersusAllTrainer,//定型使用指定的二元分类器的一对一多类分类
        [Category("多类分类")]
        [Description("多类分类-定型使用指定的二元分类器的成对耦合多类分类")]
        PairwiseCouplingTrainer,//定型使用指定的二元分类器的成对耦合多类分类

        //异常情况检测训练
        [Category("异常检测")]
        [Description("异常情况检测训练-随机 SVD 算法训练近似 PCA")]
        RandomizedPcaTrainer,//随机 SVD 算法训练近似 PCA
     

    }
}
