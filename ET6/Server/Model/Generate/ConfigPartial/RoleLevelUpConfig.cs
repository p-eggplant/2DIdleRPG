/*----------------------------------------------------------------
* 文件名:	RoleLevelUpConfig
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/7/2 9:39:51
* 创建人:
* 描  述:	
*
*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;

namespace ET
{
    public partial class RoleLevelUpConfig
    {
        /// <summary>
        /// 获取属性id1对应的枚举，空值时抛异常
        /// </summary>
        public EPlayerPropertyType GetPropType1()
        {
            EPlayerPropertyType eType = PlayerPropertyConfigCategory.Instance.ID2eType(PropTypeId1);
            if (eType == EPlayerPropertyType.None)
            {
                throw new Exception($"RoleLevelUpConfig Id={Id} PropTypeId1={PropTypeId1} 不存在，请检查配置表");
            }
            return eType;
        }

        /// <summary>
        /// 获取属性id2对应的枚举，空值时抛异常
        /// </summary>
        public EPlayerPropertyType GetPropType2()
        {
            EPlayerPropertyType eType = PlayerPropertyConfigCategory.Instance.ID2eType(PropTypeId2);
            if (eType == EPlayerPropertyType.None)
            {
                throw new Exception($"RoleLevelUpConfig Id={Id} PropTypeId2={PropTypeId2} 不存在，请检查配置表");
            }
            return eType;
        }

        /// <summary>
        /// 获取货币id对应的枚举，空值时抛异常
        /// </summary>
        public EPlayerDataType GetCurrencyEType()
        {
            if (CurrencyId <= 0 || CurrencyId >= (int)EPlayerDataType.Max)
            {
                throw new Exception($"RoleLevelUpConfig Id={Id} CurrencyId={CurrencyId} 无效，请检查配置表");
            }
            return (EPlayerDataType)CurrencyId;
        }
    }

    public partial class RoleLevelUpConfigCategory
    {
        /// <summary>
        /// 按等级排序后的列表
        /// </summary>
        private List<int> m_listSortLevel = new List<int>();

        public override void AfterEndInit()
        {
            base.AfterEndInit();
            m_listSortLevel = dict.Keys.ToList();
            m_listSortLevel.Sort();
        }

        /// <summary>
        /// 获取配置中的最高等级
        /// </summary>
        public int GetMaxLevel()
        {
            if (m_listSortLevel.Count <= 0)
            {
                return 0;
            }
            return m_listSortLevel[m_listSortLevel.Count - 1];
        }

        /// <summary>
        /// 获取当前等级的下一级配置
        /// </summary>
        public RoleLevelUpConfig GetNextConfig(int curLevel)
        {
            int nextLevel = curLevel + 1;
            if (Contain(nextLevel) == false)
            {
                throw new Exception($"RoleLevelUpConfig 下一级配置不存在 curLevel={curLevel} nextLevel={nextLevel}");
            }
            return Get(nextLevel);
        }

        /// <summary>
        /// 当前等级是否已达最高级
        /// </summary>
        public bool IsMaxLevel(int curLevel)
        {
            return curLevel >= GetMaxLevel();
        }

        /// <summary>
        /// 获取指定等级升级所需的物资数量
        /// </summary>
        public int GetNeedMaterialNum(int level)
        {
            return Get(level).MaterialNumb;
        }

        /// <summary>
        /// 获取指定等级升级所需的货币数量
        /// </summary>
        public int GetNeedCurrencyNum(int level)
        {
            return Get(level).CurrencyNumb;
        }

        /// <summary>
        /// 获取指定等级升级获得的属性加成列表
        /// </summary>
        public List<(EPlayerPropertyType eType, int nValue)> GetLevelUpPropGains(int level)
        {
            List<(EPlayerPropertyType, int)> listGains = new List<(EPlayerPropertyType, int)>();
            RoleLevelUpConfig pConfig = Get(level);

            if (pConfig.PropTypeData1 > 0)
            {
                EPlayerPropertyType ePropType1 = pConfig.GetPropType1();
                listGains.Add((ePropType1, pConfig.PropTypeData1));
            }

            if (pConfig.PropTypeData2 > 0)
            {
                EPlayerPropertyType ePropType2 = pConfig.GetPropType2();
                listGains.Add((ePropType2, pConfig.PropTypeData2));
            }

            return listGains;
        }
    }
}
