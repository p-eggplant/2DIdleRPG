/*----------------------------------------------------------------
* 文件名:	PlayerBagComponentEvent
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/27 20:48:24
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
        /// <summary>
        /// 玩家第一次创建Player
        /// </summary>
        /// <param name="self"></param>
        public override void OnFirstCreate(PlayerBagComponent self)
        {
            foreach(var node in MaterialConfigCategory.Instance.m_listFirstValue)
            {
                self.m_dicBag.Add(node.Id, node.FirstValue);
            }
        }


        /// <summary>
        /// Player对象创建时，会调用这个函数，通过这个函数，将自己作为组件挂给Player
        /// </summary>
        /// <param name="pPlayer"></param>
        public override void OnCreateComponent(ref Player pPlayer)
        {
            pPlayer.AddComponent<PlayerBagComponent>(true);
        }



        /// <summary>
        /// 玩家登录时，从数据库加载上来玩家的数据，会调用这个函数，将数据用dBUnitComponent参数传入给你
        /// </summary>
        /// <param name="self"></param>
        /// <param name="dBUnitComponent"></param>
        /// <exception cref="Exception"></exception>
        public override void OnImprotDBData(PlayerBagComponent self, PlayerBagComponent dBUnitComponent)
        {
            self.m_dicBag.Clear();
            foreach (var item in dBUnitComponent.m_dicBag)
            {
                //检查背包配置Id是否存在，num值是否合规
                if (MaterialConfigCategory.Instance.Contain(item.Key) == false)
                {
                    throw new Exception($"数据库中加载上来了不存在的物资  物资ID = {item.Key}");
                }

                if(item.Value >0)
                {
                    self.m_dicBag.Add(item.Key, item.Value);
                }
            }
            return;
        }


        /// <summary>
        /// 玩家登录数据全部初始化完毕后，会调用这个函数，你需要将需要发给客户端的数据，填充进info
        /// </summary>
        /// <param name="self"></param>
        /// <param name="info"></param>
        public override void OnExportLoginData(PlayerBagComponent self, ref PlayerLoginInfo info)
        {
            foreach (var item in self.m_dicBag)
            {
                if(item.Value > 0)
                {
                    PlayerBagProto pPlayerBagProto = new PlayerBagProto();
                    pPlayerBagProto.ConfigId = item.Key;
                    pPlayerBagProto.Num = item.Value;
                    info.PlayerBagComponent.Add(pPlayerBagProto);
                }
            }
        }
    }
}