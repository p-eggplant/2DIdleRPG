/*----------------------------------------------------------------
* 文件名:	PlayerBagComponentEvent
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/28 16:13:47
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [PlayerComponentEvent(nameof(PlayerBagComponent))]
    [FriendClassAttribute(typeof(PlayerBagComponent))]
    public class PlayerBagComponentEvent : ComponentBase<PlayerBagComponent>
    {
        public override void OnCreateComponent(ref Player pPlayer)
        {
            pPlayer.AddComponent<PlayerBagComponent>(true);
        }


        public override void OnImprotLoginData(PlayerBagComponent self, ref PlayerLoginInfo Data)
        {
            foreach (var node in Data.PlayerBagComponent)
            {
                if (false == MaterialConfigCategory.Instance.Contain(node.ConfigId))
                {
                    throw new Exception($"不存在的背包材料ConfigId ConfigId = {node.ConfigId}");
                }
                self.m_dicBag[node.ConfigId] = node.Num;
            }
        }
    }
}

