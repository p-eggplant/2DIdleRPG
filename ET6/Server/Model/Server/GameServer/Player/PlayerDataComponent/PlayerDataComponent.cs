/*----------------------------------------------------------------
* 文件名:	PlayerDataComponent
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/27 16:53:24
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [ComponentOf(typeof(Player))]
    public class PlayerDataComponent : Entity, IAwake, IDestroy ,IUnitCacheNode
    {
        [BsonIgnore]
        /// <summary>
        /// 玩家内存数据
        /// </summary>
        public long[] m_arrData = new long[(int)EPlayerDataType.Max];

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        /// <summary>
        /// 玩家存盘数据
        /// </summary>
        public Dictionary<string, long> m_dicDBData = new Dictionary<string, long>();
    }
}
