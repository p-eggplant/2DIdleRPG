using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [PlayerComponentEvent(nameof(PlayerRoleUpComponent))]
    [FriendClassAttribute(typeof(PlayerRoleUpComponent))]
    public class PlayerRoleUpComponentEvent : ComponentBase<PlayerRoleUpComponent>
    {
        public override void OnCreateComponent(ref Player pPlayer)
        {
            pPlayer.AddComponent<PlayerRoleUpComponent>(true);
        }

        public override void OnImprotLoginData(PlayerRoleUpComponent self, ref PlayerLoginInfo Data)
        {
            self.m_CurLevel = Data.PlayerRoleUpComponent.CurLevel;
        }
    }
}
