/*----------------------------------------------------------------
* 文件名:	PlayerOssComponentEvent
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/30 17:14:08
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
    [PlayerComponentEvent(nameof(PlayerOssComponent))]
    [FriendClassAttribute(typeof(PlayerOssComponent))]
    public class PlayerOssComponentEvent : ComponentBase<PlayerOssComponent>
    {

        /// <summary>
        /// Player对象创建时，会调用这个函数，通过这个函数，将自己作为组件挂给Player
        /// </summary>
        /// <param name="pPlayer"></param>
        public override void OnCreateComponent(ref Player pPlayer)
        {
            pPlayer.AddComponent<PlayerOssComponent>(true);
        }


        /// <summary>
        /// 当玩家登录时，发现玩家没有存盘数据，会调用这个函数
        /// </summary>
        /// <param name="self"></param>
        public override void OnFirstCreate(PlayerOssComponent self)
        {

        }


        /// <summary>
        /// 玩家登录时，从数据库加载上来玩家的数据，会调用这个函数，将数据用dBUnitComponent参数传入给你
        /// </summary>
        /// <param name="self"></param>
        /// <param name="dBUnitComponent"></param>
        /// <exception cref="Exception"></exception>
        public override void OnImprotDBData(PlayerOssComponent self, PlayerOssComponent dBUnitComponent)
        {

        }


        /// <summary>
        /// 玩家数据全部创建或加载完毕后，会调用这个函数
        /// </summary>
        /// <param name="self"></param>
        public override void OnInit(PlayerOssComponent self)
        {

        }

        /// <summary>
        /// 玩家登录数据全部初始化完毕后，会调用这个函数，你需要将需要发给客户端的数据，填充进info
        /// </summary>
        /// <param name="self"></param>
        /// <param name="info"></param>
        public override void OnExportLoginData(PlayerOssComponent self, ref PlayerLoginInfo info)
        {


        }

        /// <summary>
        /// 当玩家需要存盘时（登出，或者定时存盘）会调用这个函数，如果需要处理就在这里处理
        /// </summary>
        /// <param name="self"></param>
        public override void OnExportDBData(PlayerOssComponent self)
        {

        }

    }
}


