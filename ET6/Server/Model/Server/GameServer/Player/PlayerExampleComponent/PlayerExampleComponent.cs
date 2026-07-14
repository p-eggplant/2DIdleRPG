/*----------------------------------------------------------------
* 文件名:	PlayerExampleComponent
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/27 20:52:28
* 创建人:   王星莅
* 描  述:	实例组件，如果需要存盘，就继承IUnitCacheNode 接口

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System.Collections.Generic;


namespace ET
{
    [ComponentOf(typeof(Player))]
    public class PlayerExampleComponent : Entity, IAwake, IDestroy, IUnitCacheNode
    {
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        //背包物资<id, 数量>
        public Dictionary<int, int> m_dicExample = new Dictionary<int, int>();
    }

}
