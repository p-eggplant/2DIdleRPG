/*----------------------------------------------------------------
* 文件名:	UILoginMainSystem
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/22 16:00:27
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.UI;


namespace ET
{
    public class UILoginMain_IAwake : AwakeSystem<UILoginMain>
    {
        public override void Awake(UILoginMain self)
        {
            self.m_window = null;
        }
    }

    // 析构函数
    public class UILoginMain_IDestroy : DestroySystem<UILoginMain>
    {
        public override void Destroy(UILoginMain self)
        {
            self.m_window = null;
        }
    }



    [FriendClassAttribute(typeof(UILoginMain))]

    public static class UILoginMainSystem
    {
        
        /// <summary>
        /// 弹出账号输入界面
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static async ETTask OnRequestServerList(this UILoginMain self)
        {
            Scene zoneScene = self.DomainScene();
            Account pAccount = zoneScene.GetComponent<Account>();

            R2C_LoginServerPlayerList r2CResult = null;
            Session session = null;
            try
            {
                session = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(ConstValue.LoginAddress));
                {
                    C2R_LoginServerPlayerList pC2R_req = new C2R_LoginServerPlayerList();
                    pC2R_req.AccountID = pAccount.m_lAccountID;

                    r2CResult = (R2C_LoginServerPlayerList)await session.Call(pC2R_req);
                }
            }
            catch (Exception exception)
            {
                UITools.Tips(exception.Message);
            }
            finally
            {
                session?.Dispose();
            }


            if (r2CResult.Error != ErrorCode.ERR_Success)
            {
                UITools.Tips(r2CResult.Message);
                throw new Exception(r2CResult.Message);
            }


            pAccount.m_listPlayerHave = r2CResult.HavePlayerList;
            pAccount.m_listServer = r2CResult.ServerList;

            // 打开账户界面
            UIComponent.Instance.ShowWindow("UILoginServerList");
        }



        /// <summary>
        /// 账号界面输入完毕回调
        /// </summary>
        /// <param name="self"></param>
        /// <param name="szAccount">账户名</param>
        public static async ETTask OnRequestAccountLogin(this UILoginMain self)
        {
            Session session = null;
            try
            {
                 
                UIComponent.Instance.ShowWindow("UILoading","账户登录".Trans());
                Scene zoneScene = self.DomainScene();
                Account pAccount = zoneScene.GetComponent<Account>();
                if (pAccount.m_szAccount == string.Empty)
                {
                    throw new Exception("账户为空".Trans());
                }


                R2C_LoginAccount r2CLogin = null;
              
                session = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(ConstValue.LoginAddress));
                {
                    C2R_LoginAccount pC2R_Login = new C2R_LoginAccount();
                    pC2R_Login.Account = pAccount.m_szAccount;
                    r2CLogin = (R2C_LoginAccount)await session.Call(pC2R_Login);
                }

                if (r2CLogin.Error != ErrorCode.ERR_Success)
                {
                    UITools.Tips(r2CLogin.Message);
                    throw new Exception(r2CLogin.Message);
                }

                // 设置组件数据
                pAccount.m_nDefaultGameServerID = r2CLogin.DefaultGameServerId;
                pAccount.m_nDefaultGameServerName = r2CLogin.DefaultGameServerName;
                pAccount.m_lAccountID = r2CLogin.AccountID;

                // 设置显示隐藏
                UITools.FindChild(self.m_window, "Panel_Account")?.SetActive(false);
                UITools.FindChild(self.m_window, "Panel_EnterGame")?.SetActive(true);

                // 设置文本
                UITools.FindChildComponent<Text>(self.m_window, "Lb_ServerName").SetText(pAccount.m_nDefaultGameServerName.Trans());
                // 播放动画
                UITools.FindChildComponent<Animator>(self.m_window, "dlglogingmainanim")?.Play("dlgloging_mainanim_galaxy_execute");


            }
            catch (Exception exception)
            {
                UITools.Tips(exception.Message);
            }
            finally
            {
                UIComponent.Instance.HideWindowDestroy("UILoading");
                session?.Dispose();
            }

        }


        // 重新选择了服务器
        public static void OnChangeDefaultServer(this UILoginMain self)
        {
            Scene zoneScene = self.DomainScene();
            Account pAccount = zoneScene.GetComponent<Account>();
            // 设置文本
            UITools.FindChildComponent<Text>(self.m_window, "Lb_ServerName").SetText(pAccount.m_nDefaultGameServerName.Trans());
            // 播放动画
            UITools.FindChildComponent<Animator>(self.m_window, "dlglogingmainanim")?.Play("dlgloging_mainanim_galaxy_execute");
        }



        // 打开输入账号界面
        public static async ETTask OnClickInputAccount(this UILoginMain self)
        {
            Account pAccount = self.DomainScene().GetComponent<Account>();
            if (pAccount.m_szAccount != "")
            {
                await self.OnRequestAccountLogin();
            }
            else
            {
                UIComponent.Instance.ShowWindow("UILoginInputAccount");
            }

        }
        

        /// <summary>
        /// 点击进入游戏
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static async ETTask OnClickEnterGame(this UILoginMain self)
        {
            Scene zoneScene = self.DomainScene();
            Account pAccount = zoneScene.GetComponent<Account>();

            R2C_GetGateAddress pR2C_GetGateAddress;
            Session session = null;
            try
            {
                session = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(ConstValue.LoginAddress));
                {
                    C2R_GetGateAddress pC2R_GetGateAddress = new C2R_GetGateAddress();
                    pC2R_GetGateAddress.GameServerId = pAccount.m_nDefaultGameServerID;
                    pC2R_GetGateAddress.AccountID = pAccount.m_lAccountID;

                    pR2C_GetGateAddress = (R2C_GetGateAddress)await session.Call(pC2R_GetGateAddress);
                }
            }
            finally
            {
                session?.Dispose();
            }

            if (pR2C_GetGateAddress.Error != ErrorCode.ERR_Success)
            {
                UITools.Tips(pR2C_GetGateAddress.Message);
                throw new Exception(pR2C_GetGateAddress.Message);
            }


            // 创建一个gate Session,并且保存到SessionComponent中
            Session gateSession = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(pR2C_GetGateAddress.Address));
            gateSession.AddComponent<PingComponent>();
            zoneScene.AddComponent<SessionComponent>().Session = gateSession;

            G2C_LoginGate g2CLoginGate = (G2C_LoginGate)await gateSession.Call(
                new C2G_LoginGate() { Key = pR2C_GetGateAddress.Key });

            // 玩家数据全部准备完毕后退出登录界面
            Game.EventSystem.Publish(new EventType.ExitLogin() { ZoneScene = zoneScene });

        }

        public static void  OnClickDoTweenTest(this UILoginMain self)
        {
            UIComponent.Instance.ShowWindow("UIDoTweenTest");
        }

    }


}