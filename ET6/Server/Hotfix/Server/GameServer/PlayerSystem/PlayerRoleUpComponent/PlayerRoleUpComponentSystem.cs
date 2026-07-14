using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [FriendClassAttribute(typeof(PlayerRoleUpComponent))]

    // 构造函数
    public class PlayerRoleUpComponent_IAwake : AwakeSystem<PlayerRoleUpComponent>
    {
        public override void Awake(PlayerRoleUpComponent self)
        {
            self.m_CurLevel = 0;
        }
    }

    // 析构函数
    public class PlayerRoleUpComponent_IDestroy : DestroySystem<PlayerRoleUpComponent>
    {
        public override void Destroy(PlayerRoleUpComponent self)
        {
            self.m_CurLevel = 0;
        }
    }



    [FriendClassAttribute(typeof(PlayerRoleUpComponent))]
    public static class PlayerRoleUpComponentSystem
    {

        /// <summary>
        /// 得到玩家等级
        /// </summary>
        /// <param name="self"></param>
        /// <param name="eType">类型</param>
        /// <returns>数值</returns>
        public static int GetRoleCurrentLevel(this PlayerRoleUpComponent self)
        {
            return self.m_CurLevel;
        }

        /// <summary>
        /// 升级成功
        /// 1、升级
        /// 2、计算属性值
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int LevelUp(this PlayerRoleUpComponent self)
        {
            int nMaxLevel = RoleLevelUpConfigCategory.Instance.GetMaxLevel();
            self.m_CurLevel = self.m_CurLevel + 1 >= nMaxLevel ? nMaxLevel : self.m_CurLevel + 1;
            PlayerPropertyComponent pPlayerPropertyComponent = self.Parent.GetComponent<PlayerPropertyComponent>();
            if (null == pPlayerPropertyComponent)
            {
                throw new Exception($"升级成功计算属性时出错");
            }

            RoleLevelUpConfig NewLevel = RoleLevelUpConfigCategory.Instance.Get(self.m_CurLevel);
            EPlayerPropertyType PropType1 = PlayerPropertyConfigCategory.Instance.ID2eType(NewLevel.PropTypeId1);
            EPlayerPropertyType PropType2 = PlayerPropertyConfigCategory.Instance.ID2eType(NewLevel.PropTypeId2);

            Log.Warning($"升级成功计算属性值，当前等级：{self.m_CurLevel}，属性枚举：{PropType1}，属性值{NewLevel.PropTypeData1}");
            Log.Warning($"升级成功计算属性值，当前等级：{self.m_CurLevel}，属性枚举：{PropType2}，属性值{NewLevel.PropTypeData2}");

            //属性修改 
            pPlayerPropertyComponent.SetProperty(ESystemType.RoleupSystem, PropType1, NewLevel.PropTypeData1);
            pPlayerPropertyComponent.SetProperty(ESystemType.RoleupSystem, PropType2, NewLevel.PropTypeData2);

            return self.m_CurLevel;
        }

        /// <summary>
        /// 判断是否可以升级
        /// </summary>
        /// <param name="self"></param>
        /// <param name="szRes"></param>
        /// <returns></returns>
        public static bool IsCanLevelUp(this PlayerRoleUpComponent self, out string szRes)
        {
            szRes = string.Empty;
            if (self.m_CurLevel >= RoleLevelUpConfigCategory.Instance.GetMaxLevel())
            {
                szRes = "当前已经达到最大等级，无法再提升";
                return false;
            }
            return true;
        }
    }
}
