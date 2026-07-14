/*----------------------------------------------------------------
* 文件名:	UIPackageSystem
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/7/5 13:45:34
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using System;

using UnityEngine;
using UnityEngine.UI;


namespace ET
{
    // 构造函数
    public class UIPackage_IAwake : AwakeSystem<UIPackage>
    {
        public override void Awake(UIPackage self)
        {
            self.m_window = null;
        }
    }

    // 析构函数
    public class UIPackage_IDestroy : DestroySystem<UIPackage>
    {
        public override void Destroy(UIPackage self)
        {
            self.m_window = null;
        }
    }


    [FriendClassAttribute(typeof(UIPackage))]
    [FriendClassAttribute(typeof(PlayerBagComponent))]
    public static class UIPackageSystem
    {


        public static void OnRefresh(this UIPackage self)
        {

            Player pPlayer = self.DomainScene().GetComponent<Player>();

            //获取玩家背包组件
            PlayerBagComponent pPlayerBagComponent = pPlayer.GetComponent<PlayerBagComponent>();
            if (pPlayerBagComponent == null)
            {
                throw new Exception("pPlayerBagComponent == null");
            }
            //初始化列表
            var pE_SL_MaterialList = UITools.FindChild(self.m_window, "E_SL_MaterialList");
            var pContent = UITools.FindChild(pE_SL_MaterialList, "Content");

            UITools.DestroyChildren(pContent);
            GameObject objItem = UIComponent.Instance.ItemGet("Item_dlgpackagemain_MaterialList");
            foreach (var item in pPlayerBagComponent.m_dicBag)
            {
                //生成Item
                GameObject pItem = GameObject.Instantiate(objItem, pContent.transform);
                //Get方法会抛异常
                MaterialConfig pMaterialConfig = MaterialConfigCategory.Instance.Get(item.Key);

                UITools.FindChildComponent<Text>(pItem, "ELb_MaterialName")?.SetText(pMaterialConfig.Name);
                UITools.FindChildComponent<Text>(pItem, "ELb_MaterialNumb")?.SetText(item.Value.ToString());
                UITools.FindChildComponent<Button>(pItem, "EBt_Material")?.AddListenerWithId(self.OnClickItem, item.Key);
            }
        }

        public static void OnClickClose(this UIPackage self)
        {
            UIComponent.Instance.HideWindow("UIPackage");
        }
        public static async ETTask OnClickLogout(this UIPackage self)
        {
            Scene zoneScene = self.DomainScene();
            SessionComponent pSessionComponent = zoneScene.GetComponent<SessionComponent>();

            G2C_LogoutGate g2CLoginGate = (G2C_LogoutGate)await pSessionComponent.Session.Call(
                new C2G_LogoutGate());

            if (g2CLoginGate.Error == ErrorCode.ERR_Success)
            {
                Game.EventSystem.Publish(new EventType.ExitGame() { ZoneScene = zoneScene });
            }
        }
        public static void OnClickGm(this UIPackage self)
        {
            UIComponent.Instance.ShowWindow("UIGmMain");
        }

        public static void OnClickLevelUp(this UIPackage self)
        {
            UIComponent.Instance.ShowWindow("DlgRoleUp");
        }


        /// <summary>
        /// 售卖物品
        /// </summary>
        /// <param name="self"></param>
        /// <param name="nMaterialId">物资Id</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static void OnClickItem(this UIPackage self, int nMaterialId)
        {
            UIComponent.Instance.ShowWindow("DlgBagSell", nMaterialId);
        }
    }
}

