using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.Collections.Generic;


namespace ET
{
    [ComponentOf(typeof(Player))]
    public class PlayerRoleUpComponent : Entity, IAwake, IDestroy, IUnitCacheNode
    {
        public int m_CurLevel = 0;
    }
}

