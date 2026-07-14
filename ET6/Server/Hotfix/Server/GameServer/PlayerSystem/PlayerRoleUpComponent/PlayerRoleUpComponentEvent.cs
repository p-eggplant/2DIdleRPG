using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [PlayerComponentEvent(nameof(PlayerRoleUpComponent))]
    [FriendClassAttribute(typeof(PlayerRoleUpComponent))]
    public class PlayerRoleUpComponentComponentEvent : ComponentBase<PlayerRoleUpComponent>
    {
        /// <summary>
        /// 玩家第一次创建Player
        /// </summary>
        /// <param name="self"></param>
        public override void OnFirstCreate(PlayerRoleUpComponent self)
        {
            //设置默认等级为0
            self.m_CurLevel = 1;
        }


        /// <summary>
        /// Player对象创建时，会调用这个函数，通过这个函数，将自己作为组件挂给Player
        /// </summary>
        /// <param name="pPlayer"></param>
        public override void OnCreateComponent(ref Player pPlayer)
        {
            pPlayer.AddComponent<PlayerRoleUpComponent>(true);
        }



        /// <summary>
        /// 玩家登录时，从数据库加载上来玩家的数据，会调用这个函数，将数据用dBUnitComponent参数传入给你
        /// </summary>
        /// <param name="self"></param>
        /// <param name="dBUnitComponent"></param>
        /// <exception cref="Exception"></exception>
        public override void OnImprotDBData(PlayerRoleUpComponent self, PlayerRoleUpComponent dBUnitComponent)
        {
            self.m_CurLevel = dBUnitComponent.m_CurLevel;
        }


        /// <summary>
        /// 玩家登录数据全部初始化完毕后，会调用这个函数，你需要将需要发给客户端的数据，填充进info
        /// </summary>
        /// <param name="self"></param>
        /// <param name="info"></param>
        public override void OnExportDBData(PlayerRoleUpComponent self)
        {
        }

        public override void OnExportLoginData(PlayerRoleUpComponent self, ref PlayerLoginInfo info)
        {
            PlayerRoleUpProto pPlayerRoleUpProto = new PlayerRoleUpProto();
            pPlayerRoleUpProto.CurLevel = self.m_CurLevel;
            info.PlayerRoleUpComponent = pPlayerRoleUpProto;
        }

        public override void OnInit(PlayerRoleUpComponent self)
        {
            PlayerPropertyComponent pPlayerPropertyComponent = self.Parent.GetComponent<PlayerPropertyComponent>();
            if (null == pPlayerPropertyComponent)
            {
                throw new Exception($"初始化计算等级属性时出错");
            }
            if (RoleLevelUpConfigCategory.Instance.Contain(self.m_CurLevel) == true)
            {
                RoleLevelUpConfig LevelConfig = RoleLevelUpConfigCategory.Instance.Get(self.m_CurLevel);
                EPlayerPropertyType PropType1 = PlayerPropertyConfigCategory.Instance.ID2eType(LevelConfig.PropTypeId1);
                EPlayerPropertyType PropType2 = PlayerPropertyConfigCategory.Instance.ID2eType(LevelConfig.PropTypeId2);
                Log.Warning($"初始化组件计算属性值，当前等级：{self.m_CurLevel}，属性枚举：{PropType1}，属性值{LevelConfig.PropTypeData1}");
                Log.Warning($"初始化组件计算属性值，当前等级：{self.m_CurLevel}，属性枚举：{PropType2}，属性值{LevelConfig.PropTypeData2}");
                //属性修改 
                pPlayerPropertyComponent.SetProperty(ESystemType.RoleupSystem, PropType1, LevelConfig.PropTypeData1);
                pPlayerPropertyComponent.SetProperty(ESystemType.RoleupSystem, PropType2, LevelConfig.PropTypeData2);
            }
        }
    }
}