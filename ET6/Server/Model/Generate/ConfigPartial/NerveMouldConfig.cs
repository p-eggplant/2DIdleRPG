/*----------------------------------------------------------------
* 文件名:	NerveMouldConfig
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/7/8 9:45:14
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using MongoDB.Driver.Core.Servers;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public partial class NerveMouldConfig
    {
        public override void EndInit()
        {
            base.EndInit();

            if (null == MaterialConfigCategory.Instance.Get(MaterialUpId))
            {
                throw new Exception("不合法的物资ID = " + MaterialUpId + " NerveID = " + Id);
            }
            if (0 >= BaseNumber)
            {
                throw new Exception("物资数量为零 物资ID = " + MaterialUpId + " NerveID = " + Id);
            }
            if (null == MaterialConfigCategory.Instance.Get(MateirialBreakId))
            {
                throw new Exception("不合法的突破物资ID = " + MateirialBreakId + " NerveID = " + Id);
            }
            if (0 >= BreakNumber)
            {
                throw new Exception("突破物资数量为零 物资ID = " + MateirialBreakId + " NerveID = " + Id);
            }
        }
    }
    /// <summary>
    /// 功法层
    /// </summary>
    public class NerveMouldTier
    {
        #region //升级相关
        // 层数
        public int nTier = 0;
        //每个节点等级对应的属性值
        public int pMaxHp_0 = 0;
        public int pCriticalHit_1 = 0;
        public int pCriticalResist_2 = 0;
        public int pHitValue_3 = 0;
        public int pDodgeValue_4 = 0;
        public int pDefense_5 = 0;
        public int pCriticalHit_6 = 0;
        public int pCriticalResist_7 = 0;
        public int pHitValue_8 = 0;
        public int pDodgeValue_9 = 0;

        //每个节点等级对应的消耗材料类型
        public int pMaterialId = 0;
        //每个节点等级对应的消耗材料数值基础系数
        public int pMaterialBaseNum = 0;
        #endregion

        #region //突破相关
        //对应神经节点突破属性值
        public int pBreakNum = 0;
        //对应神经节点突破属性枚举类型
        public EPlayerPropertyType pBreakEnumType = 0;
        //对应神经节点突破消耗材料类型
        public int pBreakMaterialId = 0;
        //对应神经节点突破消耗材料数值
        public int pBreakMaterialNum = 0;
        #endregion

        /// <summary>
        /// 获取对应神经节点升级属性数值
        /// </summary>
        /// <param name="nLevel"></param>
        /// <returns></returns>
        public int GetLevelData(int nLevel)
        {
            switch (nLevel)
            {
                case 0:
                    return pMaxHp_0;
                case 1:
                    return pCriticalHit_1;
                case 2:
                    return pCriticalResist_2;
                case 3:
                    return pHitValue_3;
                case 4:
                    return pDodgeValue_4;
                case 5:
                    return pDefense_5;
                case 6:
                    return pCriticalHit_6;
                case 7:
                    return pCriticalResist_7;
                case 8:
                    return pHitValue_8;
                case 9:
                    return pDodgeValue_9;
                default:
                    Log.Error("nLevel =" + nLevel);
                    return 0;
            }
        }
        /// <summary>
        /// 获取对应神经节点属性枚举类型
        /// </summary>
        /// <param name="nLevel"></param>
        /// <returns></returns>
        public EPlayerPropertyType GetLevelType(int nLevel)
        {
            switch (nLevel)
            {
                case 0:
                    return EPlayerPropertyType.MaxHp;
                case 1:
                    return EPlayerPropertyType.CriticalHit;
                case 2:
                    return EPlayerPropertyType.CriticalResist;
                case 3:
                    return EPlayerPropertyType.HitValue;
                case 4:
                    return EPlayerPropertyType.DodgeValue;
                case 5:
                    return EPlayerPropertyType.Defense;
                case 6:
                    return EPlayerPropertyType.CriticalHit;
                case 7:
                    return EPlayerPropertyType.CriticalResist;
                case 8:
                    return EPlayerPropertyType.HitValue;
                case 9:
                    return EPlayerPropertyType.DodgeValue;
                default:
                    Log.Error("nLevel =" + nLevel);
                    return EPlayerPropertyType.None;
            }
        }

        /// <summary>
        /// 获取对应神经节点材料消耗数值
        /// </summary>
        /// <param name="nLevel"></param>
        /// <returns></returns>
        public int GetLevelMaterialNum(int nLevel)
        {
            return (nLevel / 2 + 1) * pMaterialBaseNum;
        }


        /// <summary>
        /// 得到升级所需材料ID
        /// </summary>
        /// <param name="nLevel">等级</param>
        /// <returns></returns>
        public int GetLevelMaterialID()
        {
            return pMaterialId;
        }



        /// <summary>
        /// 得到突破所需材料数量
        /// </summary>
        /// <returns></returns>
        public int GetBreakMaterialNum()
        {
            return pBreakMaterialNum;
        }

        /// <summary>
        /// 得到突破 所需材料ID
        /// </summary>
        /// <returns></returns>
        public int GetBreakMaterialID()
        {
            return pBreakMaterialId;
        }

        /// <summary>
        /// 获取神经每一层的最大等级
        /// </summary>
        /// <returns></returns>
        public int getMaxLevel()
        {
            return 10;
        }

    }

    /// <summary>
    /// 神经模板
    /// </summary>
    public class NerveMould
    {
        public int nNeverMouldID = 0;
        private Dictionary<int, NerveMouldTier> m_DicTier = new Dictionary<int, NerveMouldTier>();

        /// <summary>
        /// 得到最大层数
        /// </summary>
        /// <returns></returns>
        public int getMaxTier()
        {
            return m_DicTier.Count;
        }

        /// <summary>
        /// 通过层 得到 层数据
        /// </summary>
        /// <param name="nTier">层</param>
        /// <returns>层数据，可能为null </returns>
        public NerveMouldTier getTier(int nTier)
        {
            NerveMouldTier node = new NerveMouldTier();
            if (true == m_DicTier.TryGetValue(nTier, out node))
            {
                return node;
            }
            return null;
        }

        /// <summary>
        /// 内部使用，添加层数据
        /// </summary>
        /// <param name="nTier">层</param>
        /// <param name="TierConfig">层节点</param>
        /// <returns></returns>
        public bool addTier(int nTier, NerveMouldTier TierConfig)
        {
            if (true == m_DicTier.ContainsKey(nTier))
            {
                Log.Error("请专心配表：NerveTier重复  NeverMouldID =" + nNeverMouldID + " nTier = " + nTier);
                return false;
            }

            m_DicTier.Add(nTier, TierConfig);
            return true;
        }


        #region // 加速部分 按照层累加属性数值，因为初始化时大量遍历，这里用空间换时间
        // key1 层
        // key2 玩家属性
        // value 属性值
        private Dictionary<int, Dictionary<EPlayerPropertyType, int>> m_dicTotalNumericData = new Dictionary<int, Dictionary<EPlayerPropertyType, int>>();

        /// <summary>
        /// 统计每一层的所有属性数据
        /// </summary>
        public void culTotalNumericData()
        {
            // 按照层数 遍历
            for (int nTierCount = 0; nTierCount < m_DicTier.Count; nTierCount++)
            {
                // 每一层需要一个属性累加字典
                Dictionary<EPlayerPropertyType, int> dicTierNode = new Dictionary<EPlayerPropertyType, int>();

                // 循环便利所有层对象，找出小于当前层的层对象，进行属性累加
                foreach (var TierNode in m_DicTier)
                {
                    if (TierNode.Value.nTier <= nTierCount)
                    {
                        //进行 普通属性累加
                        for (int nLevel = 0; nLevel < TierNode.Value.getMaxLevel(); nLevel++)
                        {
                            EPlayerPropertyType eType = TierNode.Value.GetLevelType(nLevel);
                            int Data;
                            if (dicTierNode.TryGetValue(eType, out Data))
                            {
                                dicTierNode[eType] = Data + TierNode.Value.GetLevelData(nLevel);
                            }
                            else
                            {
                                dicTierNode.Add(eType, TierNode.Value.GetLevelData(nLevel));
                            }

                        }

                        // 进行突破属性累加
                        int breakData;
                        if (dicTierNode.TryGetValue(TierNode.Value.pBreakEnumType, out breakData))
                        {
                            dicTierNode[TierNode.Value.pBreakEnumType] = breakData + TierNode.Value.pBreakNum;
                        }
                        else
                        {
                            dicTierNode.Add(TierNode.Value.pBreakEnumType, TierNode.Value.pBreakNum);
                        }

                    }
                }

                /*
                 * 验证log
                Log.Warning("模板ID =" + this.nNeverMouldID  + " 层数 = " + nTierCount);
                foreach (var node in dicTierNode)
                {
                    Log.Warning(node.Key.ToString() +" = "+ node.Value);
                }
                */

                m_dicTotalNumericData.Add(nTierCount, dicTierNode);

            }
        }

        /// <summary>
        /// 根据层数，得到0 到当前层（包含当前层） 所有累计的属性数值
        /// </summary>
        /// <param name="nTier"></param>
        /// <returns></returns>
        public Dictionary<EPlayerPropertyType, int> getTotalNumericDatabyTier(int nTier)
        {
            Dictionary<EPlayerPropertyType, int> node = new Dictionary<EPlayerPropertyType, int>();
            if (true == m_dicTotalNumericData.TryGetValue(nTier, out node))
            {
                return node;
            }
            return null;
        }

        #endregion
    }





    public partial class NerveMouldConfigCategory
    {
        //key 模板Id
        //value 神经模板
        Dictionary<int, NerveMould> m_NerveMouldConfigCategory = new Dictionary<int, NerveMould>();

        /// <summary>
        /// 通过模板ID 和 层数 访问升级数据
        /// </summary>
        /// <param name="NerveMould">模板ID</param>
        /// <param name="NerveTier">层数</param>
        /// <returns>如果null 则非法</returns>
        public NerveMould getMouldConfig(int NerveMould)
        {
            NerveMould node = new NerveMould();
            if (true == m_NerveMouldConfigCategory.TryGetValue(NerveMould, out node))
            {
                return node;
            }
            return null;
        }

        public NerveMouldTier getNerveMouldTier(int NerveMould, int Tier)
        {
            NerveMould node = new NerveMould();
            if (false == m_NerveMouldConfigCategory.TryGetValue(NerveMould, out node))
            {
                throw new Exception($"false == m_NerveMouldConfigCategory.TryGetValue(NerveMould, out node) NerveMould = {NerveMould}, Tier = {Tier}");
            }
            NerveMouldTier tierNode = node.getTier(Tier);
            if (null == node.getTier(Tier))
            {
                throw new Exception($"null == node.getTier(Tier) NerveMould = {NerveMould}, Tier = {Tier}");
            }
            return tierNode;
        }




        public override void AfterEndInit()
        {
            base.AfterEndInit();


            foreach (var config in list)
            {
                //升级相关
                NerveMouldTier pMouldConfig = new NerveMouldTier();

                pMouldConfig.nTier = config.NerveTier;
                pMouldConfig.pMaxHp_0 = config.Numeric1;
                pMouldConfig.pCriticalHit_1 = config.Numeric2;
                pMouldConfig.pCriticalResist_2 = config.Numeric3;
                pMouldConfig.pHitValue_3 = config.Numeric4;
                pMouldConfig.pDodgeValue_4 = config.Numeric5;
                pMouldConfig.pDefense_5 = config.Numeric6;
                pMouldConfig.pCriticalHit_6 = config.Numeric7;
                pMouldConfig.pCriticalResist_7 = config.Numeric8;
                pMouldConfig.pHitValue_8 = config.Numeric9;
                pMouldConfig.pDodgeValue_9 = config.Numeric10;
                pMouldConfig.pMaterialId = config.MaterialUpId;
                //每个节点等级对应的消耗材料数值基础系数
                pMouldConfig.pMaterialBaseNum = config.BaseNumber;

                //突破相关
                //对应神经节点突破属性值
                pMouldConfig.pBreakNum = config.BreakAtk;
                //对应神经节点突破属性枚举类型
                pMouldConfig.pBreakEnumType = EPlayerPropertyType.Attack;
                //对应神经节点突破消耗材料类型
                pMouldConfig.pBreakMaterialId = config.MateirialBreakId;
                //对应神经节点突破消耗材料数值
                pMouldConfig.pBreakMaterialNum = config.BreakNumber;


                //安全判断
                if (0 == config.NerveMould)
                {
                    Log.Error("NerveMould  == 0  id =" + config.NerveMould);
                    break;
                }

                // 组件字典
                if (m_NerveMouldConfigCategory.TryGetValue(config.NerveMould, out NerveMould pNerveMouldNode))
                {
                    pNerveMouldNode.addTier(config.NerveTier, pMouldConfig);
                }
                else
                {
                    pNerveMouldNode = new NerveMould();
                    pNerveMouldNode.nNeverMouldID = config.NerveMould;
                    pNerveMouldNode.addTier(config.NerveTier, pMouldConfig);
                    m_NerveMouldConfigCategory.Add(config.NerveMould, pNerveMouldNode);
                }

            }


            // 计算累计数值
            foreach (var item in m_NerveMouldConfigCategory)
            {
                item.Value.culTotalNumericData();
            }
        }
    }
}
