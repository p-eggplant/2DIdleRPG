/*----------------------------------------------------------------
* 文件名:	UILoginServerListSystem
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/25 14:59:23
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Contexts;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    // 构造函数
    public class UILoginServerList_IAwake : AwakeSystem<UILoginServerList>
    {
        public override void Awake(UILoginServerList self)
        {
            self.m_window = null;
        }
    }

    // 析构函数
    public class UILoginServerList_IDestroy : DestroySystem<UILoginServerList>
    {
        public override void Destroy(UILoginServerList self)
        {
            self.m_window = null;
        }
    }



    [FriendClassAttribute(typeof(UILoginServerList))]


    public static class UILoginServerListSystem
    {
        public static void OnClickClose(this UILoginServerList self)
        {
            UIComponent.Instance.HideWindowDestroy("UILoginServerList");
        }

        /// <summary>
        /// 点击服务器列表按钮
        /// </summary>
        /// <param name="self"></param>
        public static void OnClickServerList(this UILoginServerList self)
        {
            Scene zoneScene = self.DomainScene();
            Account pAccount = zoneScene.GetComponent<Account>();

            UITools.FindChild(self.m_window, "EG_SeverList").SetActive(true);
            UITools.FindChild(self.m_window, "EG_PlayerList").SetActive(false);

            UITools.FindChild(self.m_window, "EImg_ServerSelected").SetActive(true);
            UITools.FindChild(self.m_window, "EImg_RoleSelected").SetActive(false);

            var pScrollView = UITools.FindChild(self.m_window, "E_SL_LoginServerList");
            var pContent = UITools.FindChild(pScrollView, "Content");
            UITools.DestroyChildren(pContent);


            GameObject pItem_LoginServerList = UIComponent.Instance.ItemGet( "Item_LoginServerList");
            foreach(var item in pAccount.m_listServer)
            {
                GameObject pItemObject = GameObject.Instantiate(pItem_LoginServerList, pContent.transform);
                UITools.FindChildComponent<Text>(pItemObject, "ELb_ServerName")?.SetText(item.szGameServerName.Trans());

                UITools.FindChildComponent<Button>(pItemObject, "EBt_Item")?.AddListenerWithId(self.OnClickServerListItem, item.nGameServerID);
                
            }

        }


        public static void OnClickServerListItem(this UILoginServerList self, int nGameServerID)
        {
            Scene zoneScene = self.DomainScene();
            Account pAccount = zoneScene.GetComponent<Account>();

            foreach (var item in pAccount.m_listServer)
            {
                if(item.nGameServerID == nGameServerID)
                {
                    pAccount.m_nDefaultGameServerID = item.nGameServerID;
                    pAccount.m_nDefaultGameServerName = item.szGameServerName;
                    UIComponent.Instance.GetWindow<UILoginMain>()?.OnChangeDefaultServer();
                    break;
                }
            }

            self.OnClickClose();

        }


        /// <summary>
        /// 点击玩家角色列表按钮
        /// </summary>
        /// <param name="self"></param>
        public static void OnClickPlayerList(this UILoginServerList self)
        {


            Scene zoneScene = self.DomainScene();
            Account pAccount = zoneScene.GetComponent<Account>();

            UITools.FindChild(self.m_window, "EG_SeverList").SetActive(false);
            UITools.FindChild(self.m_window, "EG_PlayerList").SetActive(true);

            UITools.FindChild(self.m_window, "EImg_ServerSelected").SetActive(false);
            UITools.FindChild(self.m_window, "EImg_RoleSelected").SetActive(true);

            var pPlayerScrollView = UITools.FindChild(self.m_window, "E_SL_LoginPlayerList");
            var pContent = UITools.FindChild(pPlayerScrollView, "Content");
            UITools.DestroyChildren(pContent);


            GameObject pItem_LoginPlayerList = UIComponent.Instance.ItemGet( "Item_LoginPlayerList");
            foreach (var item in pAccount.m_listPlayerHave)
            {
                GameObject pItemObject = GameObject.Instantiate(pItem_LoginPlayerList, pContent.transform);
                UITools.FindChildComponent<Text>(pItemObject, "ELb_PlayerName")?.SetText(item.szName.Trans());
                UITools.FindChildComponent<Text>(pItemObject, "ELb_FightingCapacityNumb")?.SetText(item.lFightingCapacity.ToString());
                UITools.FindChildComponent<Text>(pItemObject, "ELb_ServerName")?.SetText(item.szGameServerName.Trans());

                UITools.FindChildComponent<Button>(pItemObject, "EBt_Item")?.AddListenerWithId(self.OnClickPlayerListItem, item.nGameServerID);
            }
        }


        public static void OnClickPlayerListItem(this UILoginServerList self, int nGameServerID)
        {
            Scene zoneScene = self.DomainScene();
            Account pAccount = zoneScene.GetComponent<Account>();


            foreach (var item in pAccount.m_listPlayerHave)
            {
                if (item.nGameServerID == nGameServerID)
                {
                    pAccount.m_nDefaultGameServerID = item.nGameServerID;
                    pAccount.m_nDefaultGameServerName = item.szGameServerName;
                    UIComponent.Instance.GetWindow<UILoginMain>()?.OnChangeDefaultServer();
                    break;
                }
            }

            self.OnClickClose();
        }

       
    }

}
