using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET.Codes.Hotfix.Client.PlayerSystem.PlayerRoleUpComponentSystem
{


    [FriendClassAttribute(typeof(PlayerRoleUpComponent))]
    public static  class PlayerRoleUpComponentSystem 
    {
        public static int GetCurLevel(this PlayerRoleUpComponent self)
        {
            return self.m_CurLevel;
        }
        public static void SetLevel(this PlayerRoleUpComponent self, int level)
        {
            self.m_CurLevel = level;
        }
    }
}
