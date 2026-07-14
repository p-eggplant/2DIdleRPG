/*----------------------------------------------------------------
* 文件名:	NerveConfig
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/7/8 9:44:54
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public partial class NerveConfig
    {
        public List<int> ConditionIds = new List<int>();
        public override void EndInit()
        {
            base.EndInit();

            if (null == MaterialConfigCategory.Instance.Get(NerveUnlockId))
            {
                throw new Exception("不合法的物资ID = " + NerveUnlockId + " NerveID = " + Id);
            }
            if (0 >= NerveUnlockNum)
            {
                throw new Exception("物资数量为零 物资ID = " + NerveUnlockId + " NerveID = " + Id);
            }
            if (1 != NerveType && 2 != NerveType)
            {
                throw new Exception("不合法的神经显示类型ID = " + NerveType + " NerveID = " + Id);
            }
            //通用条件
            ConditionIds.Clear();
            if (string.IsNullOrEmpty(Condition) == false)
            {
                string[] conditions = this.Condition.Split('+');
                if (conditions != null)
                {
                    foreach (var conditionIdStr in conditions)
                    {
                        if (int.TryParse(conditionIdStr, out int conditionId))
                        {
                            ConditionIds.Add(conditionId);
                        }
                    }
                }
            }
        }
    }

    public partial class NerveConfigCategory
    {
        //根据阶组织天赋配置（普通）
        List<NerveConfig> m_dicNerveRankNormal = new List<NerveConfig>();
        //根据阶组织天赋配置（限定）
        List<NerveConfig> m_listNerveRankLimit = new List<NerveConfig>();
        public override void AfterEndInit()
        {
            m_listNerveRankLimit.Clear();
            //天赋层级列表字典
            foreach (var pConfig in GetAll().Values)
            {
                if (pConfig.NerveType == 2)
                {
                    m_listNerveRankLimit.Add(pConfig);
                }
                else if (pConfig.NerveType == 1)
                {
                    m_dicNerveRankNormal.Add(pConfig);
                }
            }
        }

        public List<NerveConfig> GetNerveRankNormalConfig()
        {
            return m_dicNerveRankNormal;
        }

        public List<NerveConfig> GetNerveRankLimitConfig()
        {
            return m_listNerveRankLimit;
        }
    }
}
