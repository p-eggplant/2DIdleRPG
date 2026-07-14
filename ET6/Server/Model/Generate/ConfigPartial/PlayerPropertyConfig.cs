/*----------------------------------------------------------------
* 文件名:	PlayerPropertyConfig
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/7/2 9:38:20
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using System;

namespace ET
{
    public partial class PlayerPropertyConfig
    {
        public override void EndInit()
        {
            base.EndInit();
            // 配置数据 安全判断 
            if (InitPropNum < 0)
            {
                throw new Exception($"起始属性小于0 id = {Id} InitPropNum = {InitPropNum}");
            }
        }

        public EPlayerPropertyType GetEType()
        {
            EPlayerPropertyType eType = EPlayerPropertyType.None;
            EPlayerPropertyType.TryParse(PropType, out eType);
            return eType;
        }
    }

    public partial class PlayerPropertyConfigCategory
    {

        PlayerPropertyConfig[] m_ArrayConfig = new PlayerPropertyConfig[(int)EPlayerPropertyType.Max];
        public override void AfterEndInit()
        {
            for (int i = 0; i < (int)EPlayerPropertyType.Max; i++)
            {
                m_ArrayConfig[i] = null;
            }

            foreach (var pConfig in GetAll().Values)
            {
                EPlayerPropertyType eType;
                if (true == EPlayerPropertyType.TryParse(pConfig.PropType, out eType))
                {
                    m_ArrayConfig[(int)eType] = pConfig;
                }
            }

        }


        /// <summary>
        /// 通过属性访问配置数据
        /// </summary>
        /// <param name="eType"></param>
        /// <returns></returns>
        public PlayerPropertyConfig Get(EPlayerPropertyType eType)
        {
            return m_ArrayConfig[(int)eType];
        }

        public bool isHaveID(int nID)
        {
            PlayerPropertyConfig pNode = Get(nID);
            if (pNode == null)
            {
                return false;
            }
            return true;
        }

        public EPlayerPropertyType ID2eType(int nID)
        {
            EPlayerPropertyType eType = EPlayerPropertyType.None;
            PlayerPropertyConfig pNode = Get(nID);
            if (pNode != null)
            {
                eType = pNode.GetEType();
            }
            return eType;
        }


        public int eType2ID(EPlayerPropertyType eType)
        {
            int id = 0;
            PlayerPropertyConfig pNode = m_ArrayConfig[(int)eType];
            if (pNode != null)
            {
                id = pNode.Id;
            }
            return id;
        }
    }
}
