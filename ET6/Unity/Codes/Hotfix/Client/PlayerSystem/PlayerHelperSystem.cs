/*----------------------------------------------------------------
* 文件名:	PlayerHelper
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/26 18:50:44
* 创建人:   王星莅
* 描  述:	用于创建Player流程的辅助类，发玩家组件创建的各个事件

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using System;
using System.Collections.Generic;
using UnityEditor.UI;
using static System.Collections.Specialized.BitVector32;



namespace ET
{
    [ObjectSystem]
    public class PlayerHelperAwakeSystem : AwakeSystem<PlayerHelper>
    {
        public override void Awake(PlayerHelper self)
        {
            PlayerHelper.Instance = self;

            self.m_dicPlayerEvent.Clear();
            List<Type> types = Game.EventSystem.GetTypes(typeof(PlayerComponentEventAttribute));
            foreach (Type type in types)
            {
                IPlayerComponentEvent obj = (IPlayerComponentEvent)Activator.CreateInstance(type);
                self.m_dicPlayerEvent.Add(obj);
            }
        }
    }


    public class PlayerHelperDestroySystem : DestroySystem<PlayerHelper>
    {
        public override void Destroy(PlayerHelper self)
        {
            self.m_dicPlayerEvent.Clear();
        }
    }

    [FriendClass(typeof(PlayerHelper))]
    public static class PlayerHelperSystem
    {

        public static Player GetPlayer(this PlayerHelper self)
        {
            Scene zoneScene = (Scene)self.DomainScene();
            Player pPlayer = null;
            pPlayer = zoneScene.GetComponent<Player>();
            if (pPlayer != null)
            {
                throw new Exception("pPlayer 不存在");
            }
            return pPlayer;
        }
        
        public static void CreateComponent(this PlayerHelper self, ref Player pPlayer)
        {
            foreach(IPlayerComponentEvent intface in self.m_dicPlayerEvent)
            {
                intface.CreateComponent(ref pPlayer);
            }
           
        }


        public static void Init(this PlayerHelper self, ref Player pPlayer)
        {

            foreach (IPlayerComponentEvent intface in self.m_dicPlayerEvent)
            {
                intface.Init(pPlayer);
            }
        }



        /// <summary>
        /// 数据库存在该玩家数据，遍历数据字典完成初始化，数据库不存在的组件根据 组件的初始创建规则 进行初始化
        /// </summary>
        public static void ImprotLoginData(this PlayerHelper self, ref Player pPlayer, ref PlayerLoginInfo Data)
        {
            foreach (IPlayerComponentEvent intface in self.m_dicPlayerEvent)
            {
                intface.ImprotLoginData(pPlayer, ref Data);
            }
        }



    }
}
