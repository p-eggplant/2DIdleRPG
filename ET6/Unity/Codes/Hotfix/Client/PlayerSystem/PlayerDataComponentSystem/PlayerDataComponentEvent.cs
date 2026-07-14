/*----------------------------------------------------------------
* 文件名:	PlayerDataComponentEvent
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/28 15:43:27
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
    [PlayerComponentEvent(nameof(PlayerDataComponent))]
    [FriendClassAttribute(typeof(PlayerDataComponent))]
    public class PlayerDataComponentEvent : ComponentBase<PlayerDataComponent>
    {
        public override void OnCreateComponent(ref Player pPlayer)
        {
            pPlayer.AddComponent<PlayerDataComponent>(true);
        }


        public override void OnImprotLoginData(PlayerDataComponent self, ref PlayerLoginInfo Data)
        {
            foreach(var node in Data.PlayerDataComponent)
            {
                if( node.EnumId > (int)EPlayerDataType.Max)
                {
                    throw new Exception($"不存在的EPlayerDataType EnumId = {node.EnumId}");
                }
                self.m_arrData[node.EnumId] = node.Value;
                Log.Debug(node.Value.ToString());
            }
        }
    }
}
