/*----------------------------------------------------------------
* 文件名:	PlayerBagComponent
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/27 20:46:52
* 创建人:
* 描  述:	

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
    public class PlayerBagComponent : Entity, IAwake, IDestroy
    {
        /// <summary>
        /// 背包物资<id, 数量>
        /// </summary>
        public Dictionary<int, int> m_dicBag = new Dictionary<int, int>();
    }
}
