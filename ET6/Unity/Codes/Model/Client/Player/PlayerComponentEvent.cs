/*----------------------------------------------------------------
* 文件名:	PlayerComponentEvent
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/26 18:08:23
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using System;

namespace ET
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PlayerComponentEventAttribute : BaseAttribute
    {
        public string TypeName { get; }

        public PlayerComponentEventAttribute(string szName)
        {
            this.TypeName = szName;
        }
    }



    public interface IPlayerComponentEvent
    {

        public void CreateComponent(ref Player pPlayer);

       
        /// <summary>
        /// 导入登录数据
        /// </summary>
        /// <param name="player"></param>
        /// <param name="dBUnitComponent"></param>
        /// <returns></returns>
        public void ImprotLoginData(Player player, ref PlayerLoginInfo Data);



        /// <summary>
        /// 玩家数据准备完毕后，执行初始化
        /// </summary>
        /// <param name="player"></param>
        public void Init(Player player);



        /// <summary>
        /// 响应玩家登录
        /// </summary>
        /// <param name="player"></param>
        public void Login(Player player);


        /// <summary>
        /// 响应玩家登出
        /// </summary>
        /// <param name="player"></param>
        public void Logout(Player player);


        /// <summary>
        /// 响应玩家断线
        /// </summary>
        /// <param name="player"></param>
        public void DisConnect(Player player);


        /// <summary>
        /// 响应玩家重连
        /// </summary>
        /// <param name="player"></param>
        public void ReConnect(Player player);
    }


    public abstract class ComponentBase<T> : IPlayerComponentEvent where T : Entity
    {

        /// <summary>
        /// 新玩家第一次创建
        /// </summary>
        public void CreateComponent(ref Player pPlayer)
        {
            if (pPlayer == null || pPlayer.IsDisposed)
                return;
            OnCreateComponent(ref pPlayer);
        }

        public abstract void OnCreateComponent(ref Player pPlayer);




        /// <summary>
        /// 导入数据库数据
        /// </summary>
        /// <param name="player"></param>
        /// <param name="dBUnitComponent"></param>
        /// <returns></returns>
        public void ImprotLoginData(Player player, ref PlayerLoginInfo Data)
        {
            if (player == null || player.IsDisposed)
                return;

            T unitComponent = player.GetComponent<T>();

            if (unitComponent == null || unitComponent.IsDisposed)
                return;

            OnImprotLoginData(unitComponent, ref Data);
        }
        public virtual void OnImprotLoginData(T self, ref PlayerLoginInfo Data) { }


      

        /// <summary>
        /// 玩家数据准备完毕后，执行初始化
        /// </summary>
        /// <param name="player"></param>
        public void Init(Player player)
        {
            if (player == null || player.IsDisposed)
                return;

            T unitComponent = player.GetComponent<T>();

            if (unitComponent == null || unitComponent.IsDisposed)
                return;

            OnInit(unitComponent);
        }
        public virtual void OnInit(T self) { }



        /// <summary>
        /// 响应玩家登录
        /// </summary>
        /// <param name="player"></param>
        public void Login(Player player)
        {
            if (player == null || player.IsDisposed)
                return;

            T unitComponent = player.GetComponent<T>();

            if (unitComponent == null || unitComponent.IsDisposed)
                return;

            OnLogin(unitComponent);
        }
        public virtual void OnLogin(T self) { }

        /// <summary>
        /// 响应玩家登出
        /// </summary>
        /// <param name="player"></param>
        public void Logout(Player player)
        {
            if (player == null || player.IsDisposed)
                return;

            T unitComponent = player.GetComponent<T>();

            if (unitComponent == null || unitComponent.IsDisposed)
                return;

            OnLogout(unitComponent);
        }
        public virtual void OnLogout(T self) { }

        /// <summary>
        /// 响应玩家断线
        /// </summary>
        /// <param name="player"></param>
        public void DisConnect(Player player)
        {
            if (player == null || player.IsDisposed)
                return;

            T unitComponent = player.GetComponent<T>();

            if (unitComponent == null || unitComponent.IsDisposed)
                return;

            OnDisConnect(unitComponent);
        }

        public virtual void OnDisConnect(T self) { }


        /// <summary>
        /// 响应玩家重连
        /// </summary>
        /// <param name="player"></param>
        public void ReConnect(Player player)
        {
            if (player == null || player.IsDisposed)
                return;

            T unitComponent = player.GetComponent<T>();

            if (unitComponent == null || unitComponent.IsDisposed)
                return;

            OnReConnect(unitComponent);
        }
        public virtual void OnReConnect(T self) { }


    }

}



