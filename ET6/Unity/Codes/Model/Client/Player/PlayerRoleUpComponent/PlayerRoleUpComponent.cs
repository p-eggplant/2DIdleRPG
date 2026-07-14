using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [ComponentOf(typeof(Player))]
    public class PlayerRoleUpComponent : Entity, IAwake, IDestroy
    {
        //当前等级
        public int m_CurLevel = 0;
    }
}
