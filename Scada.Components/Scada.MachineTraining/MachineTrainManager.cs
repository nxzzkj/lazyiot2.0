

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
using Microsoft.ML;
using Scada.DBUtility;
using Scada.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Trainers;
using Microsoft.ML.Transforms;
using Microsoft.ML.Data;
using System.IO;
using System.Data;
using Scada.Model;

namespace Scada.MachineTraining
{

    /// <summary>
    /// 机器训练学习任务
    /// </summary>
    public class MachineTrainManager : IDisposable
    {
        public Func<string, Task> ExceptionThrow;
        public Func<string, Task> TrainManagerLog;
        public Func<ScadaMachineTrainingForecast, Task> ReceiveMachineTrainPredicte;//返回当前机器学习训练预测数据
        public Func<Scada.Model.ScadaMachineTrainingModel,bool, string,Task>  MachineTrainExecuteResult;//一个机器训练记录日志
        /// <summary>
        /// 读取时序数据库的实时采集数据并将数据加入训练集进行预测
        /// </summary>
        public Func<DateTime, List<Scada.Model.ScadaMachineTrainingModel>, List<ScadaMachineTrainingInput>> ReadToTxtTrainFromInfluxdb;
        private MLContext mlContext = new MLContext();
        private string DataBaseFileName = "";
        private Business.ScadaMachineTrainingModel mTrainingBll = new Business.ScadaMachineTrainingModel();
        private Business.ScadaMachineTrainingCondition mTrainingConditionBll = new Business.ScadaMachineTrainingCondition();
        private List<Scada.Model.ScadaMachineTrainingModel> mTrainingModels;
        private System.Threading.Timer trainTimer = null;
        public int interval = 10000;//秒的时间循环一次
        public bool RunStatus = false;
        public string TrainModelSavedPath = System.AppDomain.CurrentDomain.BaseDirectory + "\\MachineTrainingData\\";

        public void InitMachineTrainManager(string dbFileName)
        {
            try
            {
           
                //初始化加载所有的训练数据
                DataBaseFileName = dbFileName;
                //设置数据库数据源
                DbHelperSQLite.connectionString = "Data Source=" + DataBaseFileName;
                mTrainingModels = mTrainingBll.GetModelList("");
                List<Scada.Model.ScadaMachineTrainingCondition> mTrainingConditions = mTrainingConditionBll.GetModelList("");
                for (int i = mTrainingModels.Count - 1; i >= 0; i--)
                {
                    AddMessageToLog(mTrainingModels[i].SERVER_NAME + " 采集站下的" + mTrainingModels[i].TaskName + "任务开始加载......");
                    mTrainingModels[i].Conditions = mTrainingConditions.FindAll(x => x.TaskId == mTrainingModels[i].Id
                    && x.SERVER_ID == mTrainingModels[i].SERVER_ID);

                }

                RunStatus = true;

                trainTimer = new System.Threading.Timer(delegate
                {
                    //训练模型
                    if (RunStatus == false)
                        return;
                    BuildAndTrainModel();
                    //开始预测模型
                    ForeastTrainModel();
                }, null, 60000, interval);
            }
            catch (Exception emx)
            {
                RunStatus = false;
                if (trainTimer != null)
                    trainTimer.Dispose();
                AddException("" + emx.Message);

            }
        }
        public void ReInitMachineTrainManager(string ServerID)
        {

            Remove(ServerID);
            if (mTrainingModels != null)
            {
                lock (mTrainingModels)
                {
                    List<Scada.Model.ScadaMachineTrainingCondition> mTrainingConditions = mTrainingConditionBll.GetModelList("  SERVER_ID='" + ServerID + "' ");
                    List<Scada.Model.ScadaMachineTrainingModel> trainingModels = mTrainingBll.GetModelList(" SERVER_ID='" + ServerID + "'");
                    for (int i = trainingModels.Count; i >= 0; i++)
                    {
                        trainingModels[i].Conditions = mTrainingConditions.FindAll(x => x.TaskId == trainingModels[i].Id
                        && x.SERVER_ID == trainingModels[i].SERVER_ID);
                    }
                    mTrainingModels.AddRange(trainingModels);
                }

            }

        }
        public void Remove(string ServerID)
        {
            if (mTrainingModels != null)
            {
                lock (mTrainingModels)
                {
                    for (int i = mTrainingModels.Count; i >= 0; i++)
                    {
                        if (mTrainingModels[i].SERVER_ID == ServerID)
                        {
                            mTrainingModels.RemoveAt(i);
                        }
                    }

                }

            }
        }
        /// <summary>
        /// 日志的写入
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private Task AddMessageToLog(string msg)
        {
            if (TrainManagerLog != null)
            {
                return TrainManagerLog(msg);
            }
            return null;
        }
        private Task AddException(string msg)
        {
            if (ExceptionThrow != null)
            {
                return ExceptionThrow(msg);
            }
            return null;
        }
        //准备和了解数据
        //创建 Model Builder 配置文件
        //选择方案
        //加载数据
        //定型模型
        //评估模型
        //使用预测模型

       
        private DateTime currentTime = DateTime.Now;
        private void BuildAndTrainModel()
        {
            mTrainingModels.ForEach(delegate (Scada.Model.ScadaMachineTrainingModel model)
            {
                if (model.TrainingCycle.Value <= 10)
                {
                    model.TrainingCycle = 10;//要求至少是10分钟
                }
                if (model.ForecastPriod.Value <= 1)
                {
                    model.ForecastPriod = 1;//1分钟
                }

                if(!File.Exists(TrainModelSavedPath + "\\" + model.Id + "\\" + model.Id + ".zip"))
                {
                    model.IsTrain = 0;
                }

            });

            if (mTrainingModels == null)
                return;
            currentTime = DateTime.Now;
            mTrainingModels.ForEach(delegate (Scada.Model.ScadaMachineTrainingModel model)
            {
                
                    DateTime calcTime = model.LastTrainTime.AddMinutes(model.TrainingCycle.Value);
                if (currentTime > calcTime && model.IsTrain == 0)
                {

                    if (model.Conditions != null)
                    {
                        try
                        {
                            AddMessageToLog(model.SERVER_NAME + " 采集站下的" + model.TaskName + "任务开始训练......");
                            ScadaTrainingDataView scadaTrainingDataView = new ScadaTrainingDataView()
                            {
                                TrainingModel = model
                            };
                            scadaTrainingDataView.DataView = BuildModel(mlContext, model);
                            if (scadaTrainingDataView.DataView != null)
                            {
                                BuildAndTrainModel(scadaTrainingDataView);
                                if (MachineTrainExecuteResult != null)
                                {
                                    MachineTrainExecuteResult(model, true, "");

                                }

                                AddMessageToLog(model.SERVER_NAME + " 采集站下的" + model.TaskName + "任务训练已经完成!");

                            }
                        }
                        catch (Exception emx)
                        {
                            if (MachineTrainExecuteResult != null)
                            {
                                MachineTrainExecuteResult(model, false, emx.Message);

                            }
                            AddException(model.SERVER_NAME + " 采集站下的" + model.TaskName + "机器训练任务执行失败! 错误:" + emx.Message);
                        }

                    }
                }
              
            });


        }

        /// <summary>
        /// 开始预测训练模型
        /// </summary>
        private void ForeastTrainModel()
        {


            try
            {
                currentTime = DateTime.Now;
                if (mTrainingModels == null)
                    return;
                List<Scada.Model.ScadaMachineTrainingModel> trainmodels = new List<Model.ScadaMachineTrainingModel>();
                mTrainingModels.ForEach(delegate (Scada.Model.ScadaMachineTrainingModel model)
                {

                    DateTime lastTime = model.LastTrainTime;
                    DateTime calcTime = lastTime.AddMinutes(model.ForecastPriod.Value);
                    if (currentTime > calcTime&&File.Exists(TrainModelSavedPath + "\\" + model.Id + "\\" + model.Id + ".zip"))
                    {
                        model.LastTrainTime = currentTime;
                        trainmodels.Add(model);
                    }
                });
                if (trainmodels.Count <= 0)
                    return;
                //读取要进行预测的数据
                List<ScadaMachineTrainingInput> dataInputs = new List<ScadaMachineTrainingInput>();
                if (ReadToTxtTrainFromInfluxdb != null)
                {
                    //读取所有的数据
                    dataInputs = ReadToTxtTrainFromInfluxdb(currentTime, trainmodels);
                    if (dataInputs != null && dataInputs.Count > 0)
                    {
                        trainmodels.ForEach(delegate (Scada.Model.ScadaMachineTrainingModel model)
                        {

#if DEBUG 
                                AddMessageToLog(model.SERVER_NAME + " 采集站下的" + model.TaskName + "任务预测行为......");
#endif
                            ScadaTrainingDataView scadaTrainingDataView = new ScadaTrainingDataView()
                                {
                                    DataView = null,

                                    TrainingModel = model

                                };
                                ScadaMachineTrainingInput input = dataInputs.Find(x => x.TaskId == model.Id
                                );
                                if (input != null)
                                {
                                    switch (model.AlgorithmType)
                                    {

                                        case "多类分类":
                                            this.MultiClassicTrainModel(scadaTrainingDataView, (ScadaMachineTrainingMultiClassicInput)input, currentTime);
                                            break;
                                        case "二元分类":
                                            this.BinaryTrainModel(scadaTrainingDataView, (ScadaMachineTrainingBinaryInput)input, currentTime);
                                            break;
                                        case "异常检测":
                                            this.RandomizedPcaTrainModel(scadaTrainingDataView, (ScadaMachineTrainingRandomizedPcaInput)input, currentTime);
                                            break;

                                    }
                                }
#if DEBUG
                                AddMessageToLog(model.SERVER_NAME + " 采集站下的" + model.TaskName + "任务预测已经完成!");
#endif

                          
                        });
                    }
                }
                else
                {
                    return;
                }
             
            }
            catch (Exception emx)
            {
                AddException("机器学习预测失败!" + emx.Message);
            }

        }
        /// <summary>
        /// 建立模型
        /// </summary>
        /// <param name="mlContext"></param>
        /// <param name="splitTrainSet"></param>
        /// <returns></returns>

        //开始建模型
        private void BuildAndTrainModel(ScadaTrainingDataView dataView)
        {
            IDataView inputdata = dataView.DataView;

            var testData = mlContext.Data.TrainTestSplit(inputdata, testFraction: 0.2).TestSet;


            ITransformer trainedModel = null;
            ScadaMachineTrainingAlgorithm mAlgorithm = ScadaMachineTrainingAlgorithm.AveragedPerceptronTrainer;
            if (Enum.TryParse<ScadaMachineTrainingAlgorithm>(dataView.TrainingModel.Algorithm, out mAlgorithm))
            {

                var lookupData = new LookupBinaryMap[2];

                lookupData[0] = new LookupBinaryMap { Value = true, Category = dataView.TrainingModel.TrueText };
                lookupData[1] = new LookupBinaryMap { Value = false, Category = dataView.TrainingModel.FalseText };
                var lookupIdvMap = mlContext.Data.LoadFromEnumerable(lookupData);
                switch (mAlgorithm)
                {


                    case ScadaMachineTrainingAlgorithm.AveragedPerceptronTrainer://二元分类
                        {
                            #region
                            //开始模型训练


                            var dataProcessPipeline = BuildBinaryClissicsTrainingPipeline(mlContext, dataView.TrainingModel.Properties.Split(','));
                            var trainer = mlContext.BinaryClassification.Trainers.AveragedPerceptron(labelColumnName: "Label", featureColumnName: "Features");
                            var trainingPipeline = dataProcessPipeline.Append(trainer)
                                               .Append(mlContext.Transforms.Conversion.MapValue("PredictedLabel",
                                             lookupIdvMap, lookupIdvMap.Schema["Value"], lookupIdvMap.Schema[
                                                 "Category"], "PredictedLabel"));
                            trainedModel = trainingPipeline.Fit(inputdata);

                            #endregion
                        }
                        break;
                    case ScadaMachineTrainingAlgorithm.FastForestBinaryTrainer://二元分类
                        {
                            #region
                            //开始模型训练


                            //转换IDataView
                            var dataProcessPipeline = BuildBinaryClissicsTrainingPipeline(mlContext, dataView.TrainingModel.Properties.Split(','));
                            var trainer = mlContext.BinaryClassification.Trainers.FastForest(labelColumnName: "Label", featureColumnName: "Features");
                            var trainingPipeline = dataProcessPipeline.Append(trainer)
                                               .Append(mlContext.Transforms.Conversion.MapValue("PredictedLabel",
                                             lookupIdvMap, lookupIdvMap.Schema["Value"], lookupIdvMap.Schema[
                                                 "Category"], "PredictedLabel"));
                            trainedModel = trainingPipeline.Fit(inputdata);

                            #endregion
                        }
                        break;
                    case ScadaMachineTrainingAlgorithm.FastTreeBinaryTrainer://二元分类
                        {

                            #region
                            //开始模型训练


                            //转换IDataView
                            var dataProcessPipeline = BuildBinaryClissicsTrainingPipeline(mlContext, dataView.TrainingModel.Properties.Split(','));
                            var trainer = mlContext.BinaryClassification.Trainers.FastTree(labelColumnName: "Label", featureColumnName: "Features");
                            var trainingPipeline = dataProcessPipeline.Append(trainer)
                                               .Append(mlContext.Transforms.Conversion.MapValue("PredictedLabel",
                                             lookupIdvMap, lookupIdvMap.Schema["Value"], lookupIdvMap.Schema[
                                                 "Category"], "PredictedLabel"));
                            trainedModel = trainingPipeline.Fit(inputdata);

                            #endregion

                        }
                        break;
                    case ScadaMachineTrainingAlgorithm.FieldAwareFactorizationMachineTrainer://二元分类
                        {

                            #region
                            //开始模型训练


                            //转换IDataView
                            var dataProcessPipeline = BuildBinaryClissicsTrainingPipeline(mlContext, dataView.TrainingModel.Properties.Split(','));
                            var trainer = mlContext.BinaryClassification.Trainers.FieldAwareFactorizationMachine(labelColumnName: "MarkLabel", featureColumnName: "Features");
                            var trainingPipeline = dataProcessPipeline.Append(trainer)
                                               .Append(mlContext.Transforms.Conversion.MapValue("PredictedLabel",
                                             lookupIdvMap, lookupIdvMap.Schema["Value"], lookupIdvMap.Schema[
                                                 "Category"], "PredictedLabel"));
                            trainedModel = trainingPipeline.Fit(inputdata);

                            #endregion

                        }
                        break;
                    case ScadaMachineTrainingAlgorithm.GamBinaryTrainer://二元分类
                        {

                            #region
                            //开始模型训练


                            //转换IDataView
                            var dataProcessPipeline = BuildBinaryClissicsTrainingPipeline(mlContext, dataView.TrainingModel.Properties.Split(','));
                            var trainer = mlContext.BinaryClassification.Trainers.Gam(labelColumnName: "Label", featureColumnName: "Features");
                            var trainingPipeline = dataProcessPipeline.Append(trainer)
                                               .Append(mlContext.Transforms.Conversion.MapValue("PredictedLabel",
                                             lookupIdvMap, lookupIdvMap.Schema["Value"], lookupIdvMap.Schema[
                                                 "Category"], "PredictedLabel"));
                            trainedModel = trainingPipeline.Fit(inputdata);

                            #endregion

                        }

                        break;
                    case ScadaMachineTrainingAlgorithm.LbfgsLogisticRegressionBinaryTrainer://二元分类
                        {

                            #region
                            //开始模型训练


                            //转换IDataView
                            var dataProcessPipeline = BuildBinaryClissicsTrainingPipeline(mlContext, dataView.TrainingModel.Properties.Split(','));
                            var trainer = mlContext.BinaryClassification.Trainers.LbfgsLogisticRegression(labelColumnName: "Label", featureColumnName: "Features");
                            var trainingPipeline = dataProcessPipeline.Append(trainer)
                                                .Append(mlContext.Transforms.Conversion.MapValue("PredictedLabel",
                                              lookupIdvMap, lookupIdvMap.Schema["Value"], lookupIdvMap.Schema[
                                                  "Category"], "PredictedLabel"));
                            trainedModel = trainingPipeline.Fit(inputdata);

                            #endregion

                        }
                        break;

                    case ScadaMachineTrainingAlgorithm.LightGbmBinaryTrainer://二元分类
                        {

                            #region
                            //开始模型训练


                            //转换IDataView
                            var dataProcessPipeline = BuildBinaryClissicsTrainingPipeline(mlContext, dataView.TrainingModel.Properties.Split(','));
                            var trainer = mlContext.BinaryClassification.Trainers.LightGbm(labelColumnName: "Label", featureColumnName: "Features");
                            var trainingPipeline = dataProcessPipeline.Append(trainer)
                                                 .Append(mlContext.Transforms.Conversion.MapValue("PredictedLabel",
                                               lookupIdvMap, lookupIdvMap.Schema["Value"], lookupIdvMap.Schema[
                                                   "Category"], "PredictedLabel"));
                            trainedModel = trainingPipeline.Fit(inputdata);

                            #endregion

                        }
                        break;
                    case ScadaMachineTrainingAlgorithm.SdcaLogisticRegressionBinaryTrainer://二元分类
                        {
                            #region
                            //开始模型训练


                            //转换IDataView
                            var dataProcessPipeline = BuildBinaryClissicsTrainingPipeline(mlContext, dataView.TrainingModel.Properties.Split(','));
                            var trainer = mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features");
                            var trainingPipeline = dataProcessPipeline.Append(trainer)
                                               .Append(mlContext.Transforms.Conversion.MapValue("PredictedLabel",
                                             lookupIdvMap, lookupIdvMap.Schema["Value"], lookupIdvMap.Schema[
                                                 "Category"], "PredictedLabel"));
                            trainedModel = trainingPipeline.Fit(inputdata);

                            #endregion
                        }
                        break;
                    case ScadaMachineTrainingAlgorithm.LinearSvmTrainer://二元分类
                        {

                            #region

                            //转换IDataView
                            var dataProcessPipeline = BuildBinaryClissicsTrainingPipeline(mlContext, dataView.TrainingModel.Properties.Split(','));
                            var trainer = mlContext.BinaryClassification.Trainers.LinearSvm(labelColumnName: "Label", featureColumnName: "Features");
                            var trainingPipeline = dataProcessPipeline.Append(trainer)
                                               .Append(mlContext.Transforms.Conversion.MapValue("PredictedLabel",
                                             lookupIdvMap, lookupIdvMap.Schema["Value"], lookupIdvMap.Schema[
                                                 "Category"], "PredictedLabel"));
                            trainedModel = trainingPipeline.Fit(inputdata);

                            #endregion

                        }
                        break;
                    case ScadaMachineTrainingAlgorithm.PriorTrainer://二元分类
                        {


                            #region
                            //开始模型训练


                            //转换IDataView
                            var dataProcessPipeline = BuildBinaryClissicsTrainingPipeline(mlContext, dataView.TrainingModel.Properties.Split(','));
                            var trainer = mlContext.BinaryClassification.Trainers.Prior(labelColumnName: "Label");
                            var trainingPipeline = dataProcessPipeline.Append(trainer)
                                                .Append(mlContext.Transforms.Conversion.MapValue("PredictedLabel",
                                              lookupIdvMap, lookupIdvMap.Schema["Value"], lookupIdvMap.Schema[
                                                  "Category"], "PredictedLabel"));
                            trainedModel = trainingPipeline.Fit(inputdata);

                            #endregion


                        }
                        break;
                    case ScadaMachineTrainingAlgorithm.SdcaNonCalibratedBinaryTrainer://二元分类
                        {
                            #region
                            //转换IDataView
                            var dataProcessPipeline = BuildBinaryClissicsTrainingPipeline(mlContext, dataView.TrainingModel.Properties.Split(','));
                            var trainer = mlContext.BinaryClassification.Trainers.SdcaNonCalibrated(labelColumnName: "Label", featureColumnName: "Features");
                            var trainingPipeline = dataProcessPipeline.Append(trainer)
                                               .Append(mlContext.Transforms.Conversion.MapValue("PredictedLabel",
                                             lookupIdvMap, lookupIdvMap.Schema["Value"], lookupIdvMap.Schema[
                                                 "Category"], "PredictedLabel"));
                            trainedModel = trainingPipeline.Fit(inputdata);

                            #endregion
                        }
                        break;
                    case ScadaMachineTrainingAlgorithm.SymbolicSgdLogisticRegressionBinaryTrainer://二元分类
                        {
                            #region
                            //转换IDataView
                            var dataProcessPipeline = BuildBinaryClissicsTrainingPipeline(mlContext, dataView.TrainingModel.Properties.Split(','));
                            var trainer = mlContext.BinaryClassification.Trainers.SymbolicSgdLogisticRegression(labelColumnName: "MarkLabel", featureColumnName: "Features");
                            var trainingPipeline = dataProcessPipeline.Append(trainer)
                                                .Append(mlContext.Transforms.Conversion.MapValue("PredictedLabel",
                                              lookupIdvMap, lookupIdvMap.Schema["Value"], lookupIdvMap.Schema[
                                                  "Category"], "PredictedLabel"));
                            trainedModel = trainingPipeline.Fit(inputdata);

                            #endregion
                        }

                        break;
                    case ScadaMachineTrainingAlgorithm.LightGbmMulticlassTrainer://多类分类
                        {
                            #region
                            //开始模型训练


                            var dataProcessPipeline = BuildMultiClissicsTrainingPipeline(mlContext, dataView.TrainingModel.Properties.Split(','));
                            var trainer = mlContext.MulticlassClassification.Trainers.LightGbm(labelColumnName: "Label", featureColumnName: "Features");
                            var trainingPipeline = dataProcessPipeline.Append(trainer)
                             .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel"));
                            trainedModel = trainingPipeline.Fit(inputdata);

                            #endregion


                        }
                        break;

                    case ScadaMachineTrainingAlgorithm.NaiveBayesMulticlassTrainer://多类分类
                        {
                            #region

                            //转换IDataView

                            var dataProcessPipeline = BuildMultiClissicsTrainingPipeline(mlContext, dataView.TrainingModel.Properties.Split(','));
                            var trainer = mlContext.MulticlassClassification.Trainers.NaiveBayes(labelColumnName: "Label", featureColumnName: "Features");
                            var trainingPipeline = dataProcessPipeline.Append(trainer)
                             .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel"));
                            trainedModel = trainingPipeline.Fit(inputdata);

                            #endregion


                        }

                        break;
                    case ScadaMachineTrainingAlgorithm.LbfgsMaximumEntropyMulticlassTrainer://多元分类
                        {
                            #region

                            var dataProcessPipeline = BuildMultiClissicsTrainingPipeline(mlContext, dataView.TrainingModel.Properties.Split(','));
                            var trainer = mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy(labelColumnName: "Label", featureColumnName: "Features");
                            var trainingPipeline = dataProcessPipeline.Append(trainer)
                             .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel"));
                            trainedModel = trainingPipeline.Fit(inputdata);

                            #endregion


                        }
                        break;
                    case ScadaMachineTrainingAlgorithm.OneVersusAllTrainer://多类分类
                        {

                            #region
                            var dataProcessPipeline = BuildMultiClissicsTrainingPipeline(mlContext, dataView.TrainingModel.Properties.Split(','));
                            var trainer = mlContext.MulticlassClassification.Trainers.OneVersusAll(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features"), labelColumnName: "Label");
                            var trainingPipeline = dataProcessPipeline.Append(trainer)
                             .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel"));
                            trainedModel = trainingPipeline.Fit(inputdata);

                            #endregion
                        }

                        break;
                    case ScadaMachineTrainingAlgorithm.PairwiseCouplingTrainer://多类分类
                        {

                            #region
                            var dataProcessPipeline = BuildMultiClissicsTrainingPipeline(mlContext, dataView.TrainingModel.Properties.Split(','));
                            var trainer = mlContext.MulticlassClassification.Trainers.PairwiseCoupling(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features"), labelColumnName: "Label");
                            var trainingPipeline = dataProcessPipeline.Append(trainer)
                             .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel"));
                            trainedModel = trainingPipeline.Fit(inputdata);

                            #endregion

                        }
                        break;

                    case ScadaMachineTrainingAlgorithm.SdcaMaximumEntropyMulticlassTrainer://多类分类
                        {

                            #region
                            var dataProcessPipeline = BuildMultiClissicsTrainingPipeline(mlContext, dataView.TrainingModel.Properties.Split(','));
                            var trainer = mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy(labelColumnName: "Label", featureColumnName: "Features");
                            var trainingPipeline = dataProcessPipeline.Append(trainer)
                             .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel"));
                            trainedModel = trainingPipeline.Fit(inputdata);

                            #endregion
                        }
                        break;

                    case ScadaMachineTrainingAlgorithm.SdcaNonCalibratedMulticlassTrainer://多元分类
                        {

                            #region
                            var dataProcessPipeline = BuildMultiClissicsTrainingPipeline(mlContext, dataView.TrainingModel.Properties.Split(','));
                            var trainer = mlContext.MulticlassClassification.Trainers.SdcaNonCalibrated(labelColumnName: "Label", featureColumnName: "Features");
                            var trainingPipeline = dataProcessPipeline.Append(trainer)
                             .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel", "PredictedLabel"));
                            trainedModel = trainingPipeline.Fit(inputdata);


                            #endregion
                        }

                        break;

                    case ScadaMachineTrainingAlgorithm.RandomizedPcaTrainer://异常检测
                        {

                            #region
                            var dataProcessPipeline = BuildRandomizedPcaPipeline(mlContext, dataView.TrainingModel.Properties.Split(','));
                            var trainer = mlContext.AnomalyDetection.Trainers.RandomizedPca(featureColumnName: "Features", rank: 1, ensureZeroMean: false);
                            var trainingPipeline = dataProcessPipeline.Append(trainer);
                            trainedModel = trainingPipeline.Fit(inputdata);


                            #endregion
                        }

                        break;


                }
                if (dataView.TrainingModel != null && trainedModel != null)
                {
                   
                        mTrainingBll.Update(dataView.TrainingModel);
                        if (!Directory.Exists(TrainModelSavedPath + dataView.TrainingModel.Id))
                        {
                            Directory.CreateDirectory(TrainModelSavedPath + dataView.TrainingModel.Id);
                        }
                        mlContext.Model.Save(trainedModel, dataView.DataView.Schema, TrainModelSavedPath + dataView.TrainingModel.Id + "\\" + dataView.TrainingModel.Id.ToString() + ".zip");
                  
                }

            }
        }
        /// <summary>
        /// 创建异常检测管道
        /// </summary>
        /// <param name="mlContext"></param>
        /// <returns></returns>
        private static IEstimator<ITransformer> BuildRandomizedPcaPipeline(MLContext mlContext, string[] columns)
        {
            string[] cols = new string[columns.Length];
            for (int i = 0; i < columns.Length; i++)
            {
                cols[i] = "Column" + (i + 1);
            }


            var dataProcessPipeline = mlContext.Transforms.Concatenate("Features", cols)
                                      .Append(mlContext.Transforms.NormalizeMinMax("Features", "Features"))
                                      .AppendCacheCheckpoint(mlContext);

            return dataProcessPipeline;
        }
        /// <summary>
        /// 多类分析
        /// </summary>
        /// <param name="dataView"></param>
        /// <param name="input"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private Task RandomizedPcaTrainModel(ScadaTrainingDataView dataView, ScadaMachineTrainingRandomizedPcaInput input, DateTime dateTime)
        {
            
            return MachineTrainTaskHelper.Factory.StartNew(() =>
            {
                try
                {
                    DateTime sdate = dateTime.AddMinutes(dataView.TrainingModel.ForecastPriod.Value);
                    DateTime edate = dateTime;


                    try
                    {
                        #region  
                        DataViewSchema modelSchema;
                        ITransformer predictionPipeline = mlContext.Model.Load(TrainModelSavedPath + "\\" + dataView.TrainingModel.Id + "\\" + dataView.TrainingModel.Id + ".zip", out modelSchema);
                        if (modelSchema == null)
                            return;
                        input.ColumnNumber = input.Properties.Count();
                        PredictionEngine<ScadaMachineTrainingRandomizedPcaInput, ScadaMachineTrainingRandomizedPcaOutput> predictionEngine = mlContext.Model.CreatePredictionEngine<ScadaMachineTrainingRandomizedPcaInput, ScadaMachineTrainingRandomizedPcaOutput>(predictionPipeline);
                        ScadaMachineTrainingRandomizedPcaOutput prediction = predictionEngine.Predict(input);
                        if (prediction != null && prediction.PredictedLabel==false)
                        {
                            string decs = dataView.TrainingModel.GetDetection(prediction.Score);
                            ScadaMachineTrainingForecast forecast = new ScadaMachineTrainingForecast()
                            {

                                Algorithm = input.Algorithm,
                                SERVER_ID = input.SERVER_ID,
                                COMM_ID = input.COMM_ID,
                                DEVICE_ID = input.DEVICE_ID,
                                TaskId = input.TaskId,
                                FeaturesName = string.Join(",", input.Properties),//特征点
                                PredictedDate = DateTime.Now,//预测点的时间
                                PredictedLabel = decs,//当前的预测数据值
                                FeaturesValue = input.GetColumnValueString(),//当前的特征数据
                                Remark = "",
                                AlgorithmType = dataView.TrainingModel.AlgorithmType,
                                Score = prediction.Score.ToString(),
                                TaskName = dataView.TrainingModel.TaskName



                            };
                            if (!string.IsNullOrEmpty(forecast.PredictedLabel))
                            {
                                if (ReceiveMachineTrainPredicte != null)
                                {
                                    ReceiveMachineTrainPredicte(forecast);
                                }
                                 
                            }
                        }

                        #endregion
                    }
                    catch (Exception emx)
                    {
                        AddException(emx.Message);
                    }


                }
                catch (Exception emx)
                {
                    AddException(dataView.TrainingModel.TaskName + "任务下样本模型训练错误");
                }

            });

        }
        /// <summary>
        /// 创建多类分类管道
        /// </summary>
        /// <param name="mlContext"></param>
        /// <returns></returns>
        private static IEstimator<ITransformer> BuildMultiClissicsTrainingPipeline(MLContext mlContext, string[] columns)
        {
            string[] cols = new string[columns.Length];
            for (int i = 0; i < columns.Length; i++)
            {
                cols[i] = "Column" + (i + 1);
            }


            var dataProcessPipeline = mlContext.Transforms.Conversion.MapValueToKey("Label", "MarkLabel")
                                      .Append(mlContext.Transforms.Concatenate("Features", cols))
                                      .Append(mlContext.Transforms.NormalizeMinMax("Features", "Features"))
                                      .AppendCacheCheckpoint(mlContext);

            return dataProcessPipeline;
        }
        /// <summary>
        /// 创建二元分类管道BinaryClassification
        /// </summary>
        /// <param name="mlContext"></param>
        /// <returns></returns>
        private static IEstimator<ITransformer> BuildBinaryClissicsTrainingPipeline(MLContext mlContext, string[] columns)
        {
            string[] cols = new string[columns.Length];
            for (int i = 0; i < columns.Length; i++)
            {
                cols[i] = "Column" + (i + 1);
            }
            // Data process configuration with pipeline data transformations 
            var dataProcessPipeline = mlContext.Transforms.Conversion.ConvertType("Label", "MarkLabel", DataKind.Boolean)
                .Append(mlContext.Transforms.Concatenate("Features", cols))
                                      .Append(mlContext.Transforms.NormalizeMinMax("Features", "Features"))
                                      .AppendCacheCheckpoint(mlContext);

            return dataProcessPipeline;
        }
        /// <summary>
        /// 多类分析
        /// </summary>
        /// <param name="dataView"></param>
        /// <param name="input"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private Task MultiClassicTrainModel(ScadaTrainingDataView dataView, ScadaMachineTrainingMultiClassicInput input, DateTime dateTime)
        {
            return MachineTrainTaskHelper.Factory.StartNew(() =>
            {
                try
                {
                    DateTime sdate = dateTime.AddMinutes(dataView.TrainingModel.ForecastPriod.Value);
                    DateTime edate = dateTime;


                    try
                    {
                        #region  
                        DataViewSchema modelSchema;
                        ITransformer predictionPipeline = mlContext.Model.Load(TrainModelSavedPath + "\\" + dataView.TrainingModel.Id + "\\" + dataView.TrainingModel.Id + ".zip", out modelSchema);
                        if (modelSchema == null)
                            return;
                        input.ColumnNumber = input.Properties.Count();
                        PredictionEngine<ScadaMachineTrainingMultiClassicInput, ScadaMachineTrainingMultiClassicOutput> predictionEngine = mlContext.Model.CreatePredictionEngine<ScadaMachineTrainingMultiClassicInput, ScadaMachineTrainingMultiClassicOutput>(predictionPipeline);
                        ScadaMachineTrainingMultiClassicOutput prediction = predictionEngine.Predict(input);
                        if (prediction != null && prediction.PredictedLabel.ToString() != "")
                        {
                            ScadaMachineTrainingForecast forecast = new ScadaMachineTrainingForecast()
                            {

                                Algorithm = input.Algorithm,
                                SERVER_ID = input.SERVER_ID,
                                COMM_ID = input.COMM_ID,
                                DEVICE_ID = input.DEVICE_ID,
                                TaskId = input.TaskId,
                                FeaturesName = string.Join(",", input.Properties),//特征点
                                PredictedDate = DateTime.Now,//预测点的时间
                                PredictedLabel = prediction.PredictedLabel,//当前的预测数据值
                                FeaturesValue = input.GetColumnValueString(),//当前的特征数据
                                Remark = "",
                                AlgorithmType = dataView.TrainingModel.AlgorithmType,
                                Score = prediction.GetScoreString().ToString(),
                                TaskName = dataView.TrainingModel.TaskName
                                 


                            };
                            if (!string.IsNullOrEmpty(forecast.PredictedLabel))
                            {
                                if (ReceiveMachineTrainPredicte != null)
                                {
                                    ReceiveMachineTrainPredicte(forecast);
                                }
                               
                            }
                        }

                        #endregion
                    }
                    catch (Exception emx)
                    {
                        AddException(emx.Message);
                    }


                }
                catch (Exception emx)
                {
                    AddException(dataView.TrainingModel.TaskName + "任务下样本模型训练错误");
                }

            });

        }
        /// <summary>
        /// 异常检测
        /// </summary>
        /// <param name="dataView"></param>
        /// <param name="input"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>

        /// <summary>
        /// 二元类计算
        /// </summary>
        /// <param name="dataView"></param>
        /// <param name="input"></param>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        private Task BinaryTrainModel(ScadaTrainingDataView dataView, ScadaMachineTrainingBinaryInput input, DateTime dateTime)
        {
            return MachineTrainTaskHelper.Factory.StartNew(() =>
            {
                try
                {
                    DateTime sdate = dateTime.AddMinutes(dataView.TrainingModel.ForecastPriod.Value);
                    DateTime edate = dateTime;


                    try
                    {
                        #region  
                        DataViewSchema modelSchema;
                        ITransformer predictionPipeline = mlContext.Model.Load(TrainModelSavedPath + "\\" + dataView.TrainingModel.Id + "\\" + dataView.TrainingModel.Id + ".zip", out modelSchema);
                        if (modelSchema == null)
                            return;
                        PredictionEngine<ScadaMachineTrainingBinaryInput, ScadaMachineTrainingBinaryOutput> predictionEngine = mlContext.Model.CreatePredictionEngine<ScadaMachineTrainingBinaryInput, ScadaMachineTrainingBinaryOutput>(predictionPipeline);
                        ScadaMachineTrainingBinaryOutput prediction = predictionEngine.Predict(input);
                        ScadaMachineTrainingForecast forecast = new ScadaMachineTrainingForecast()
                        {


                            Algorithm = input.Algorithm,
                            SERVER_ID = input.SERVER_ID,
                            COMM_ID = input.COMM_ID,
                            DEVICE_ID = input.DEVICE_ID,
                            TaskId = input.TaskId,
                            FeaturesName = string.Join(",", input.Properties),//特征点
                            PredictedDate = DateTime.Now,//预测点的时间
                            PredictedLabel = prediction.PredictedLabel,//当前的预测数据值
                            FeaturesValue = input.GetColumnValueString(),//当前的特征数据
                            Remark = "",
                            AlgorithmType = dataView.TrainingModel.AlgorithmType,
                            Score = prediction.Score.ToString(),
                            TaskName = dataView.TrainingModel.TaskName



                        };
                        if (!string.IsNullOrEmpty(forecast.PredictedLabel))
                        {
                            if (ReceiveMachineTrainPredicte != null)
                            {
                                ReceiveMachineTrainPredicte(forecast);
                            }
                          
                        }

                        #endregion
                    }
                    catch (Exception emx)
                    {
                        AddException(emx.Message);
                    }


                }
                catch (Exception emx)
                {
                    AddException(dataView.TrainingModel.TaskName + "任务下样本模型训练错误");
                }

            });

        }
        private List<ScadaTrainingDataView> GetTrainModels(string serverid, string communicationid, string deviceid)
        {
            List<ScadaTrainingDataView> views = new List<ScadaTrainingDataView>();
            List<Scada.Model.ScadaMachineTrainingModel> finders = mTrainingModels.FindAll(x => x.SERVER_ID == serverid
           && x.COMM_ID == communicationid && x.DEVICE_ID == deviceid);
            finders.ForEach(delegate (Scada.Model.ScadaMachineTrainingModel finder)
            {
                views.Add(new ScadaTrainingDataView()
                {
                    TrainingModel = finder,
                    DataView = null,

                });
            });
            return views;
        }
        public void Dispose()
        {
            RunStatus = false;
            if (trainTimer != null)
                trainTimer.Dispose();
            trainTimer = null;
            if (mTrainingModels != null)
                mTrainingModels.Clear();
            mTrainingModels = null;
            mlContext = null;
            DataBaseFileName = "";
        }
        /// <summary>
        /// 替换缺失值
        /// </summary>
        /// <param name="inputColumn"></param>
        /// <param name="outputColumn"></param>
        /// <param name="mLContext"></param>
        /// <param name="inputdata"></param>
        /// <returns></returns>
        private IDataView ReplaceMissingValues(string inputColumn, string outputColumn, MLContext mLContext, IDataView inputdata)
        {
            var replacementEstimatorA = mLContext.Transforms.ReplaceMissingValues(inputColumn, outputColumn, replacementMode: MissingValueReplacingEstimator.ReplacementMode.Mean);
            ITransformer replacementTransformer = replacementEstimatorA.Fit(inputdata);

            // Transform data
            IDataView transformedData = replacementTransformer.Transform(inputdata);
            return transformedData;
        }
        /// <summary>
        /// 标准化数据
        /// </summary>
        /// <param name="inputColumn"></param>
        /// <param name="outputColumn"></param>
        /// <param name="mLContext"></param>
        /// <param name="inputdata"></param>
        /// <returns></returns>

        private IDataView NormalizeMinMax(string inputColumn, string outputColumn, MLContext mLContext, IDataView inputdata)
        {
            // Define min-max estimator
            var minMaxEstimator = mLContext.Transforms.NormalizeMinMax(inputColumn, outputColumn);

            // Fit data to estimator
            // Fitting generates a transformer that applies the operations of defined by estimator
            ITransformer minMaxTransformer = minMaxEstimator.Fit(inputdata);

            // Transform data
            IDataView transformedData = minMaxTransformer.Transform(inputdata);
            return transformedData;
        }

        /// <summary>
        /// 合并列
        /// </summary>
        /// <param name="Columns"></param>
        /// <param name="mLContext"></param>
        /// <param name="inputdata"></param>
        /// <param name="outColumn"></param>
        /// <returns></returns>
        private IDataView MergeColumn(string[] Columns, MLContext mLContext, IDataView inputdata, string outColumn = "Features")
        {
            // Define Data Prep Estimator
            // 1. Concatenate Size and Historical into a single feature vector output to a new column called Features
            // 2. Normalize Features vector
            IEstimator<ITransformer> dataPrepEstimator =
                mlContext.Transforms.Concatenate(outColumn, Columns)
                    .Append(mlContext.Transforms.NormalizeMinMax(outColumn));

            // Create data prep transformer
            ITransformer dataPrepTransformer = dataPrepEstimator.Fit(inputdata);

            // Apply transforms to training data
            IDataView transformedTrainingData = dataPrepTransformer.Transform(inputdata);
            return transformedTrainingData;
        }
        //将文本列格式化
        private IDataView FeaturizeText(MLContext mLContext, string inputColumn, string outputColumn, IDataView inputdata)
        {
            // Define text transform estimator
            var textEstimator = mlContext.Transforms.Text.FeaturizeText(outputColumn, inputColumn);
            ITransformer textTransformer = textEstimator.Fit(inputdata);

            // Transform data
            IDataView transformedData = textTransformer.Transform(inputdata);
            return transformedData;
        }

        /// <summary>
        /// 建立模型
        /// </summary>
        /// <param name="mLContext"></param>
        /// <param name="inputdata"></param>
        /// <param name="trainingModel"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        private IDataView BuildModel(MLContext mLContext, Scada.Model.ScadaMachineTrainingModel trainingModel)
        {
            string kind = trainingModel.AlgorithmType;
            DataKind mDataKind = DataKind.String;
            if ("二元分类" == kind)
            {
                mDataKind = DataKind.Boolean;
            }
            else if ("多类分类" == kind)
            {
                mDataKind = DataKind.String;
            }
            else if ("异常检测" == kind)
            {
                mDataKind = DataKind.String;
            }
            //创建架构
            var columns = new TextLoader.Column[trainingModel.Properties.Split(',').Length + 2];
            columns[0] = new TextLoader.Column("DateStampTime", DataKind.String, 0);
            columns[1] = new TextLoader.Column("MarkLabel", mDataKind, 1);
            for (int i = 0; i < trainingModel.Properties.Split(',').Length; i++)
            {
                columns[2 + i] = new TextLoader.Column("Column" + (i + 1), DataKind.Single, 2 + i);
            }
            // STEP 1: 准备数据,加载多个文件的数据


            List<string> files = new List<string>();
            trainingModel.Conditions.ForEach(delegate (Scada.Model.ScadaMachineTrainingCondition condition)
            {
                files.Add(System.AppDomain.CurrentDomain.BaseDirectory + "\\MachineTrainingData\\" + trainingModel.Id + "\\" + condition.DataFile);
            });
            if (!trainingModel.Conditions.Exists(x => x.IsTrain == 0))
            {
                return null;
            }

            TextLoader textLoader = mlContext.Data.CreateTextLoader(columns, separatorChar: ',', hasHeader: true);
            IDataView sourcedata = textLoader.Load(files.ToArray());
  
                //对原始数据进行处理
                for (int i = 2; i < columns.Length; i++)
                {
                    //缺失值处理
                    sourcedata = ReplaceMissingValues(columns[i].Name, columns[i].Name, mlContext, sourcedata);
                    //规范化数据

                }

          
            return sourcedata;


        }

    }
}
