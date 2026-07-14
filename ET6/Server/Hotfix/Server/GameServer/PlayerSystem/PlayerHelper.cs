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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public static class PlayerHelper
    {

        public static void CreateComponent(ref Player pPlayer)
        {
            List<Type> types = Game.EventSystem.GetTypes(typeof(PlayerComponentEventAttribute));
            foreach (Type type in types)
            {
                //实例化该组件的 初始化方法类，并强转成其接口对象
                IPlayerComponentEvent obj = (IPlayerComponentEvent)Activator.CreateInstance(type);
                obj.CreateComponent(ref pPlayer);
            }
        }

        /// <summary>
        /// 数据库不存在该玩家数据，根据各个组件的初始创建规则 进行初始化
        /// </summary>
        public static void FirstCreate(ref Player player)
        {
            List<Type> types = Game.EventSystem.GetTypes(typeof(PlayerComponentEventAttribute));
            foreach (Type type in types)
            {
                //实例化该组件的 初始化方法类，并强转成其接口对象
                IPlayerComponentEvent obj = (IPlayerComponentEvent)Activator.CreateInstance(type);
                obj.FirstCreate(player);
            }
        }


        public static PlayerLoginInfo ExportLoginData(ref Player pPlayer)
        {
            PlayerLoginInfo pLoginData = new PlayerLoginInfo();
            List<Type> types = Game.EventSystem.GetTypes(typeof(PlayerComponentEventAttribute));
            foreach (Type type in types)
            {
                //实例化该组件的 初始化方法类，并强转成其接口对象
                IPlayerComponentEvent obj = (IPlayerComponentEvent)Activator.CreateInstance(type);
                obj.ExportLoginData(pPlayer, ref pLoginData);
            }
            return pLoginData;
        }

        public static void Init(ref Player pPlayer)
        {
            // 调佣所有组件的Init方法
            List<Type> types = Game.EventSystem.GetTypes(typeof(PlayerComponentEventAttribute));
            foreach (Type type in types)
            {
                //实例化该组件的 初始化方法类，并强转成其接口对象
                IPlayerComponentEvent obj = (IPlayerComponentEvent)Activator.CreateInstance(type);
                obj.Init(pPlayer);
            }
        }

        public static void ExportDBData(ref Player player)
        {
            List<Type> types = Game.EventSystem.GetTypes(typeof(PlayerComponentEventAttribute));
            foreach (Type type in types)
            {
                //实例化该组件的 初始化方法类，并强转成其接口对象
                IPlayerComponentEvent obj = (IPlayerComponentEvent)Activator.CreateInstance(type);
                obj.ExportDBData(player);
            }
        }


        /// <summary>
        /// 数据库存在该玩家数据，遍历数据字典完成初始化，数据库不存在的组件根据 组件的初始创建规则 进行初始化
        /// </summary>
        public static void ImprotDBData(ref Player player, Dictionary<string, Entity> Entitydic)
        {


            List<Type> types = Game.EventSystem.GetTypes(typeof(PlayerComponentEventAttribute));
            foreach (Type type in types)
            {
                IPlayerComponentEvent obj = (IPlayerComponentEvent)Activator.CreateInstance(type);
                object[] attr = type.GetCustomAttributes(typeof(PlayerComponentEventAttribute), false);
                PlayerComponentEventAttribute unitCacheAttribute = (PlayerComponentEventAttribute)attr[0];
                string DBCompName = "ET." + unitCacheAttribute.TypeName;


                if (true == Entitydic.TryGetValue(DBCompName, out Entity ent))
                {
                    //实例化该组件的 初始化方法类，并强转成其接口对象

                    obj.ImprotDBData(player, ent);
                }
                else
                {
                    obj.FirstCreate(player);
                }
            }
        }



    }
}
