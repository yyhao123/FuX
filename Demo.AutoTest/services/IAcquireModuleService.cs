using Demo.AutoTest.viewModel.Module;
using Demo.Communication.constant;
using Demo.Model.data;
using FuX.Core.services;
using FuX.Model.data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuX.Core.handler;
using Demo.Model.@enum;

namespace Demo.AutoTest.services
{
    public interface IAcquireModuleService
    {
        /// <summary>
        /// Add spectrum tree nodes
        /// </summary>
        /// <param name="model"></param>
        /// <param name="spectrum"></param>
        /// <returns></returns>
        int AddSpectrumTreeNode(AcquireModuleDataInfo model, SpectrumDto spectrum, Color spectrumColor, SpecInfo specInfo = null);

        /// <summary>
        /// Add root node, Id = 0
        /// </summary>
        /// <param name="model"></param>
        void AddRootSpectrumTreeNode(AcquireModuleDataInfo model);

        /// <summary>
        /// Remove from LineItemList (ZedGraph)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="spectrumId"></param>
        /// <returns></returns>
        bool RemoveLineItemToList(AcquireModuleDataInfo model, string spectrumId);

        /// <summary>
        /// Add to LineItemList (ZedGraph)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="spectrum"></param>
        /// <returns></returns>
        bool AddLineItemToList(AcquireModuleDataInfo model, SpectrumDto spectrum, Color spectrumColor, SpecInfo specInfo = null);

        /// <summary>
        /// Add to LineItemList (ZedGraph)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="spectrumNode"></param>
        /// <returns></returns>
        bool AddLineItemToList(AcquireModuleDataInfo model, SpectrumNode spectrumNode);

        /// <summary>
        /// Find SpectrumList item
        /// </summary>
        /// <param name="model"></param>
        /// <param name="spectrumId"></param>
        /// <returns></returns>
        SpectrumNode FindSpectrum(AcquireModuleDataInfo model, string spectrumId);

        /// <summary>
        /// 在graph上显示所有的光谱, Add checked treenode to LineItemList
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool ShowAllSpectrumLine(AcquireModuleDataInfo model);

        /// <summary>
        /// remove spectrum from 
        /// SpectrumList/LineItemList/SpectrumHistory/SpectrumTreeNodes
        /// </summary>
        /// <param name="model"></param>
        /// <param name="spectrumId"></param>
        void RemoveSpectrum(AcquireModuleDataInfo model, string spectrumId);

        void AddSpectrumHistory(AcquireModuleDataInfo model, SpectrumDto spectrum);
    }

    public class AcquireModuleService : IAcquireModuleService
    {
       

        private ILocalize _localize;

        public AcquireModuleService(
            ILocalize localize)
        {    
            _localize = localize;
        }


        public void AddRootSpectrumTreeNode(AcquireModuleDataInfo model)
        {
            if (model.SpectrumTreeNodes.Any(x => x.Id == 0 && x.ZedTypeBox == model.ZedTypeBox))
                return;


            var node = new SpectrumTreeNode()
            {
                // Id = ConstantFile.SPECTRUM_TREE_LIST_ROOT_ID,
                 Id = 0,

                ParentId = -1,
                NodeText = _localize.GetString(LocalizeConstant.SPECTRUM_TREE_ROOT_TITLE),
                NodeType = SpectrumTreeNodeType.Root,
                ZedTypeBox = model.ZedTypeBox,
            };

            model.SpectrumTreeNodes.Add(node);
        }

        /// <summary>
        /// 添加历史记录tree node
        /// </summary>
        /// <param name="spectrum"></param>
        /// <returns></returns>
        public int AddSpectrumTreeNode(AcquireModuleDataInfo model, SpectrumDto spectrum, Color spectrumColor, SpecInfo specInfo = null)
        {
            if (spectrum == null) return -1;

            var node = new SpectrumTreeNode()
            {
                Id = model.NextNodeId(),
                ParentId = 0,
                SpectrumId = (specInfo != null ? specInfo.id : spectrum.Id),
                NodeText = spectrum.Name,
                NodeType = SpectrumTreeNodeType.Spectrum,
                Color = spectrumColor,
                ZedTypeBox = model.ZedTypeBox,
            };

            var pram = JsonConvert.DeserializeObject<AcquireParameter>(spectrum.PramInfo);

            if (pram.CCDInfo == null || pram.CCDInfo.CCDType == CCDType.Ponit)
            {
                //通用节点
                var childNodes = new List<SpectrumTreeNode>()
               {
                  new SpectrumTreeNode()
                  {
                    Id = model.NextNodeId(),
                    ParentId = node.Id,
                    SpectrumId = (specInfo != null ? specInfo.id : spectrum.Id),
                    NodeType= SpectrumTreeNodeType.ScanParameter,
                     ZedTypeBox = model.ZedTypeBox,
                    NodeText = $"{_localize.GetString(LocalizeConstant.TREELIST_COLLECTTYPES)} : { pram.CollectTypes}"
                   },
                 new SpectrumTreeNode()
                  {
                    Id = model.NextNodeId(),
                    ParentId = node.Id,
                    SpectrumId = (specInfo != null ? specInfo.id : spectrum.Id),
                    NodeType= SpectrumTreeNodeType.ScanParameter,
                     ZedTypeBox = model.ZedTypeBox,
                    NodeText = $"{_localize.GetString(LocalizeConstant.COLL_GAIN_MULTIPLE)} : {_localize.GetString( _localize.GetCfg<List<ComObj>>(_userCfg.Gain).FirstOrDefault(t=>t.val==pram.SignalGear.ToString())?.key)}"
                  },
                new SpectrumTreeNode()
                 {
                    Id = model.NextNodeId(),
                    ParentId = node.Id,
                    SpectrumId = (specInfo != null ? specInfo.id : spectrum.Id),
                    NodeType= SpectrumTreeNodeType.ScanParameter,
                     ZedTypeBox = model.ZedTypeBox,
                    NodeText = $"{_localize.GetString(LocalizeConstant.COLL_FILTER_WEIGHTS)} : {_localize.GetString( _localize.GetCfg<List<ComObj>>(_userCfg.FilterWeights).FirstOrDefault(t=>t.val==pram.FilterWeights.ToString())?.key)}"
                  },
            };



                if (pram.CollectTypes == CollectType.Point)
                {
                    childNodes.AddRange(
                        new List<SpectrumTreeNode>() {
                    new SpectrumTreeNode()
                    {
                        Id = model.NextNodeId(),
                        ParentId = node.Id,
                        SpectrumId =(specInfo != null ? specInfo.id : spectrum.Id),
                         ZedTypeBox = model.ZedTypeBox,
                        NodeType = SpectrumTreeNodeType.ScanParameter,
                        NodeText = $"{_localize.GetString(LocalizeConstant.TREELIST_RESPONSETIME)} : { pram.ResponseTime.ToString()}"
                    },
                    new SpectrumTreeNode()
                       {
                           Id = model.NextNodeId(),
                           ParentId = node.Id,
                           SpectrumId = (specInfo != null ? specInfo.id : spectrum.Id),
                            ZedTypeBox = model.ZedTypeBox,
                           NodeType = SpectrumTreeNodeType.ScanParameter,
                           NodeText = $"{_localize.GetString(LocalizeConstant.TREELIST_PONITBC)} : { pram.PonitBC.ToString()}"
                       },
                    new SpectrumTreeNode()
                          {
                              Id = model.NextNodeId(),
                              ParentId = node.Id,
                              SpectrumId = (specInfo != null ? specInfo.id : spectrum.Id),
                               ZedTypeBox = model.ZedTypeBox,
                              NodeType = SpectrumTreeNodeType.ScanParameter,
                              NodeText = $"{_localize.GetString(LocalizeConstant.COLLECTION_MODEL)} : { (pram.TimeType==0?_localize.GetString(LocalizeConstant.TREELIST_TIMETYPE_CONTINUOUS):_localize.GetFormatString(LocalizeConstant.TREELIST_TIMETYPE_TIME,new string[]{ pram.ScanningTime.ToString()}))}"
                       }
                     }
                    );
                }
                else
                {
                    if (pram.CCDInfo != null && pram.CCDInfo.CCDType == CCDType.Ponit)
                    {
                        childNodes.AddRange(
                                     new List<SpectrumTreeNode>() {

                    new SpectrumTreeNode()
                          {
                              Id = model.NextNodeId(),
                              ParentId = node.Id,
                              SpectrumId = (specInfo != null ? specInfo.id : spectrum.Id),
                               ZedTypeBox = model.ZedTypeBox,
                              NodeType = SpectrumTreeNodeType.ScanParameter,
                               NodeText = $"{_localize.GetString(LocalizeConstant.TREELIST_FREQUENCY)} : {_localize.GetString( _localize.GetCfg<List<ComObj>>(_userCfg.GatherRate).FirstOrDefault(t=>t.val==pram.GatherRate.ToString())?.key)}"

                       }
                                  }
                                 );
                    }


                    if (pram.CCDInfo != null && pram.CCDInfo.CCDType != CCDType.Area)
                    {
                        childNodes.AddRange(
                                        new List<SpectrumTreeNode>() {

                    new SpectrumTreeNode()
                          {
                              Id = model.NextNodeId(),
                              ParentId = node.Id,
                              SpectrumId = (specInfo != null ? specInfo.id : spectrum.Id),
                               ZedTypeBox = model.ZedTypeBox,
                              NodeType = SpectrumTreeNodeType.ScanParameter,
                              NodeText = $"{_localize.GetString(LocalizeConstant.COLLECTION_MODEL)} : { (pram.RangeType==0?_localize.GetString(LocalizeConstant.TREELIST_TIMETYPE_FULLSPECTRUM):_localize.GetFormatString(LocalizeConstant.TREELIST_TIMETYPE_RANGE,new string[]{ pram.RangeMin.ToString(),pram.RangeMax.ToString()}))}"
                       }
                                     }
                                    );
                    }
                    if (pram.CCDInfo != null && pram.CCDInfo.CCDType == CCDType.Area)
                    {
                        childNodes.AddRange(
                                      new List<SpectrumTreeNode>() {

                    new SpectrumTreeNode()
                          {
                              Id = model.NextNodeId(),
                              ParentId = node.Id,
                              SpectrumId = (specInfo != null ? specInfo.id : spectrum.Id),
                               ZedTypeBox = model.ZedTypeBox,
                              NodeType = SpectrumTreeNodeType.ScanParameter,
                              NodeText = $"{_localize.GetString(LocalizeConstant.TREELIST_SPECTRUM_ADDRESS)} : { (specInfo.LineNum.Count>1?(specInfo.LineNum.Min()+"-"+specInfo.LineNum.Max()):specInfo.LineNum.FirstOrDefault().ToString())}"
                       },
                     new SpectrumTreeNode()
                          {
                              Id = model.NextNodeId(),
                              ParentId = node.Id,
                              SpectrumId = (specInfo != null ? specInfo.id : spectrum.Id),
                               ZedTypeBox = model.ZedTypeBox,
                              NodeType = SpectrumTreeNodeType.ScanParameter,
                              NodeText = $"{_localize.GetString(LocalizeConstant.TREELIST_SPECTRUM_APPROACH)} : { specInfo.AlgorithmMode}"
                       }
                                   }
                                  );
                    }
                    if (pram.CCDInfo == null)
                    {
                        childNodes.AddRange(
                                     new List<SpectrumTreeNode>() {

                    new SpectrumTreeNode()
                          {
                              Id = model.NextNodeId(),
                              ParentId = node.Id,
                              SpectrumId = (specInfo != null ? specInfo.id : spectrum.Id),
                               ZedTypeBox = model.ZedTypeBox,
                              NodeType = SpectrumTreeNodeType.ScanParameter,
                               NodeText = $"{_localize.GetString(LocalizeConstant.TREELIST_FREQUENCY)} : {_localize.GetString( _localize.GetCfg<string>(_userCfg.Accuracy))}"

                       }
                                  }
                                 );
                    }

                }

                //时间
                childNodes.Add(new SpectrumTreeNode()
                {
                    Id = model.NextNodeId(),
                    ParentId = node.Id,
                    SpectrumId = (specInfo != null ? specInfo.id : spectrum.Id),
                    NodeType = SpectrumTreeNodeType.ScanParameter,
                    ZedTypeBox = model.ZedTypeBox,
                    NodeText = $"{_localize.GetString(LocalizeConstant.SPECTRUM_CREATED)} : {spectrum.Created.ToString("yyyy-MM-dd HH:mm:ss")}"
                });

                model.SpectrumTreeNodes.Add(node);

                childNodes.ForEach(x => model.SpectrumTreeNodes.Add(x));

                return node.Id;
            }
            if (pram.CCDInfo != null && pram.CCDInfo.CCDType == CCDType.Area)
            {
                var childNodes = new List<SpectrumTreeNode>()
                {
                  new SpectrumTreeNode()
                  {
                    Id = model.NextNodeId(),
                    ParentId = node.Id,
                    SpectrumId = spectrum.Id,
                    NodeType= SpectrumTreeNodeType.ScanParameter,
                    ZedTypeBox = model.ZedTypeBox,
                    NodeText = $"{_localize.GetString(LocalizeConstant.SPECTRUM_CREATED)} : {spectrum.Created.ToString("yyyy-MM-dd HH:mm:ss")}"
                   },

                  new SpectrumTreeNode()
                  {
                    Id = model.NextNodeId(),
                    ParentId = node.Id,
                    SpectrumId = spectrum.Id,
                    NodeType= SpectrumTreeNodeType.ScanParameter,
                    ZedTypeBox = model.ZedTypeBox,
                    NodeText = $"{_localize.GetString(LocalizeConstant.SPECTRUM_INTEGRATION_TIME)} : {spectrum.IntegrationTime.ToString()}"
                  }
                 };

                model.SpectrumTreeNodes.Add(node);

                childNodes.ForEach(x => model.SpectrumTreeNodes.Add(x));

                return node.Id;




            }
            if (pram.CCDInfo != null && pram.CCDInfo.CCDType == CCDType.Line)
            {
                //通用节点
                var childNodes = new List<SpectrumTreeNode>()
              {
                new SpectrumTreeNode()
                {
                    Id = model.NextNodeId(),
                    ParentId = node.Id,
                    SpectrumId = (specInfo != null ? specInfo.id : spectrum.Id),
                    NodeType= SpectrumTreeNodeType.ScanParameter,
                     ZedTypeBox = model.ZedTypeBox,
                    NodeText = $"{_localize.GetString(LocalizeConstant.TREE_GSNUM)} : {spectrum.Grating}"
                },
                new SpectrumTreeNode()
                {
                    Id = model.NextNodeId(),
                    ParentId = node.Id,
                    SpectrumId = (specInfo != null ? specInfo.id : spectrum.Id),
                    NodeType= SpectrumTreeNodeType.ScanParameter,
                     ZedTypeBox = model.ZedTypeBox,
                    NodeText = $"{_localize.GetString(LocalizeConstant.TREE_AVGNUM)} : {spectrum.Average}"
                },
                new SpectrumTreeNode()
                {
                    Id = model.NextNodeId(),
                    ParentId = node.Id,
                    SpectrumId = (specInfo != null ? specInfo.id : spectrum.Id),
                    NodeType= SpectrumTreeNodeType.ScanParameter,
                     ZedTypeBox = model.ZedTypeBox,
                    NodeText = $"{_localize.GetString(LocalizeConstant.TREE_INTEGRATIONTIME)} : {spectrum.IntegrationTime}"
                },
                new SpectrumTreeNode()
                {
                    Id = model.NextNodeId(),
                    ParentId = node.Id,
                    SpectrumId = (specInfo != null ? specInfo.id : spectrum.Id),
                    NodeType= SpectrumTreeNodeType.ScanParameter,
                    ZedTypeBox = model.ZedTypeBox,
                    NodeText = $"{_localize.GetString(LocalizeConstant.TREE_CENTERWAVE)} : {spectrum.CenterWave}"
                },
              };
                model.SpectrumTreeNodes.Add(node);

                childNodes.ForEach(x => model.SpectrumTreeNodes.Add(x));

                return node.Id;
            }
            else
                return -1;




        }

        public SpectrumNode FindSpectrum(AcquireModuleDataInfo model, string spectrumId)
        {
            if (model == null
                || model.SpectrumList == null) return null;

            return model.SpectrumList.FirstOrDefault(x => x.SpectrumId == spectrumId && model.ZedTypeBox == x.ZedTypeBox);
        }

        /// <summary>
        /// 移除Zedgraph曲线，来源treelist unchecked
        /// </summary>
        /// <param name="model"></param>
        /// <param name="spectrumId"></param>
        /// <returns></returns>
        public bool RemoveLineItemToList(AcquireModuleDataInfo model, string spectrumId)
        {
            if (model.LineItemList == null || model.LineItemList.Count < 1)
                return false;

            var item = model.LineItemList.FirstOrDefault(x => x.SpectrumId == spectrumId && x.ZedTypeBox == model.ZedTypeBox);

            if (item == null) return false;

            model.LineItemList.Remove(item);

            return true;
        }

        /// <summary>
        /// 添加谱图曲线到graph
        /// </summary>
        /// <param name="spectrum"></param>
        /// <returns></returns>
        public bool AddLineItemToList(AcquireModuleDataInfo model, SpectrumDto spectrum, Color spectrumColor, SpecInfo specInfo = null)
        {
            if (spectrum == null) return false;

            if (model.LineItemList.Any(x => x.SpectrumId == spectrum.Id && x.ZedTypeBox == model.ZedTypeBox))
            {
                return false;
            }
            if (model.ZedTypeBox == ZedTypeBox.D)
            {
                // 判断曲线数量是否超过最大和设置的曲线数目
                if ((model.LineItemList.Count >= _localize.GetCfg<int>(_userCfg.MaxCurveCount)
                        && model.LineItemList.Count > 0)
                    || model.LineItemList.Count >= _localize.GetCfg<int>(_userCfg.CurveCount)) // 移除最先添加的曲线
                {
                    // 不移除pined光谱
                    var itemToRemove = model.LineItemList.Where(x => !model.PinSpectrumList.Any(p => p.SpectrumId == x.SpectrumId)).FirstOrDefault();

                    if (itemToRemove != null)
                        model.LineItemList.Remove(itemToRemove);
                    else
                        return false;
                }
            }
            var line = new SpectrumNode(spectrum, spectrumColor, model.ZedTypeBox, specInfo);
            line.ZedTypeBox = model.ZedTypeBox;
            model.LineItemList.Add(line);

            return true;
        }

        /// <summary>
        /// 在graph上显示所有的光谱, Add checked treenode to LineItemList
        /// </summary>
        /// <param name="model"></param>
        /// <param name="spectrumNode"></param>
        /// <returns></returns>
        public bool AddLineItemToList(AcquireModuleDataInfo model, SpectrumNode spectrumNode)
        {
            if (spectrumNode == null) return false;

            if (model.LineItemList.Any(x => x.SpectrumId == spectrumNode.SpectrumId))
            {
                return false;
            }

            if (model.ZedTypeBox == ZedTypeBox.D)
            {
                // 判断曲线数量是否超过最大和设置的曲线数目
                if ((model.LineItemList.Count >= _localize.GetCfg<int>(_userCfg.MaxCurveCount)
                        && model.LineItemList.Count > 0)
                    || model.LineItemList.Count >= _localize.GetCfg<int>(_userCfg.CurveCount)) // 移除最先添加的曲线
                {
                    // 不移除pined光谱
                    var itemToRemove = model.LineItemList.Where(x => !model.PinSpectrumList.Any(p => p.SpectrumId == x.SpectrumId)).FirstOrDefault();

                    if (itemToRemove != null)
                        model.LineItemList.Remove(itemToRemove);
                    else
                        return false;
                }
            }

            model.LineItemList.Add(spectrumNode);

            return true;
        }

        public bool ShowAllSpectrumLine(AcquireModuleDataInfo model)
        {
            foreach (var item in model.SpectrumList)
            {
                AddLineItemToList(model, item);
            }

            return true;
        }

        /// <summary>
        /// 删除光谱数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="spectrumId"></param>
        public void RemoveSpectrum(AcquireModuleDataInfo model, string spectrumId)
        {
            var history = model.SpectrumHistory.FirstOrDefault(x => x.SpectrumId == spectrumId);
            if (history != null)
                model.SpectrumHistory.Remove(history);

            var spectrumItem = model.SpectrumList.FirstOrDefault(x => x.SpectrumId == spectrumId);
            if (spectrumItem != null)
                model.SpectrumList.Remove(spectrumItem);

            var lineItem = model.LineItemList.FirstOrDefault(x => x.SpectrumId == spectrumId);
            if (lineItem != null)
                model.LineItemList.Remove(lineItem);

            var treeNodes = model.SpectrumTreeNodes.Where(x => x.SpectrumId == spectrumId).ToList();

            foreach (var node in treeNodes ?? Enumerable.Empty<SpectrumTreeNode>())
            {
                model.SpectrumTreeNodes.Remove(node);
            }
        }

        public void AddSpectrumHistory(AcquireModuleDataInfo model, SpectrumDto spectrumDto)
        {
            var history = model.SpectrumHistory.FirstOrDefault(x => x.SpectrumId == spectrumDto.Id);
            if (history != null)
                return;

            history = new SpectrumHistoryDto(spectrumDto);
            model.SpectrumHistory.Insert(0, history);
        }

    }
}
