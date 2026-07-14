/*----------------------------------------------------------------
* 文件名:	UIGmPlayerBagSystem
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/7/1 13:25:25
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/




using System;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

namespace ET
{
    // 构造函数
    public class UIGmPlayerBag_IAwake : AwakeSystem<UIGmPlayerBag>
    {
        public override void Awake(UIGmPlayerBag self)
        {
            self.m_window = null;
        }
    }

    // 析构函数
    public class UIGmPlayerBag_IDestroy : DestroySystem<UIGmPlayerBag>
    {
        public override void Destroy(UIGmPlayerBag self)
        {
            self.m_window = null;
        }
    }


    [FriendClassAttribute(typeof(UIGmPlayerBag))]
    public static class UIGmPlayerBagSystem
    {
        public static void OnClickClose(this UIGmPlayerBag self)
        {
            UIComponent.Instance.HideWindow("UIGmPlayerBag");
        }
        public static async ETTask OnClickAdd(this UIGmPlayerBag self)
        {

            Player pPlayer = self.DomainScene().GetComponent<Player>();
            //获取玩家背包组件
            PlayerBagComponent pPlayerBagComponent = pPlayer.GetComponent<PlayerBagComponent>();
            if (pPlayerBagComponent == null)
            {
                throw new Exception("pPlayerBagComponent == null");
            }


            var pPanel_Bottom = UITools.FindChild(self.m_window, "Panel_Bottom");

            //获取输入框的id和数量
            int nConfigId = int.Parse(UITools.FindChildComponent<InputField>(pPanel_Bottom, "EIF_InputId").text);
            int nConfigNum = int.Parse(UITools.FindChildComponent<InputField>(pPanel_Bottom, "EIF_InputNum").text);
            if(false == MaterialConfigCategory.Instance.Contain(nConfigId))
            {
                UITools.Tips("配置表没有该物品Id：" + nConfigId);
                return;
            }
            if (nConfigNum <= 0)
            {
                UITools.Tips("修改数量不合法：" + nConfigNum);
                return;
            }
            C2Game_GmBagAdd pC2Game_GmBagAdd = new C2Game_GmBagAdd();
            pC2Game_GmBagAdd.ConfigId = nConfigId;
            pC2Game_GmBagAdd.Num = nConfigNum;

            // 发送网络消息
            Scene zoneScene = self.DomainScene();
            SessionComponent pSessionComponent = zoneScene.GetComponent<SessionComponent>();

            Game2C_GmBagAdd pGame2C_GmBagAdd = (Game2C_GmBagAdd)await pSessionComponent.Session.Call(
                pC2Game_GmBagAdd);
            // 返回成功
            if (pGame2C_GmBagAdd.Error == ErrorCode.ERR_Success)
            {
                // 刷新列表
                self.OnShow();
            }
        }

        /// <summary>
        /// Gm背包Item显示
        /// </summary>
        /// <param name="self"></param>
        /// <exception cref="Exception"></exception>
        public static void OnShow(this UIGmPlayerBag self)
        {
            Player pPlayer = self.DomainScene().GetComponent<Player>();

            PlayerBagComponent pPlayerBagComponent = pPlayer.GetComponent<PlayerBagComponent>();
            if (pPlayerBagComponent == null)
            {
                throw new Exception("pPlayerBagComponent == null");
            }

            var pESV_List = UITools.FindChild(self.m_window, "ESV_List");
            var pContent = UITools.FindChild(pESV_List, "Content");
            UITools.DestroyChildren(pContent);

            GameObject objItem = UIComponent.Instance.ItemGet("Gm_PlayerBag_Item");

            List<MaterialConfig> dic = MaterialConfigCategory.Instance.m_listFirstValue;
            foreach (var item in dic)
            {
                GameObject pItem = GameObject.Instantiate(objItem, pContent.transform);
                
                UITools.FindChildComponent<Text>(pItem, "ELb_Name")?.SetText(item.Name);
                UITools.FindChildComponent<Text>(pItem, "ELb_Num")?.SetText(pPlayerBagComponent.GetItemNum(item.Id).ToString());

            }
        }
    }
}

