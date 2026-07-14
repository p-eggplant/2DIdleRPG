/*----------------------------------------------------------------
* 文件名:	UIGmPlayerPropertySystem
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/7/2 11:13:05
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using ET;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace ET
{
    // 构造函数
    public class UIGmPlayerProperty_IAwake : AwakeSystem<UIGmPlayerProperty>
    {
        public override void Awake(UIGmPlayerProperty self)
        {
            self.m_window = null;
            self.m_eType = ESystemType.RankSystem;
        }
    }

    // 析构函数
    public class UIGmPlayerProperty_IDestroy : DestroySystem<UIGmPlayerProperty>
    {
        public override void Destroy(UIGmPlayerProperty self)
        {
            self.m_window = null;
            self.m_eType = ESystemType.RankSystem;
        }
    }


    [FriendClassAttribute(typeof(UIGmPlayerProperty))]
    public static class UIGmPlayerPropertySystem
    {
        /// <summary>
        /// 刷新界面
        /// </summary>
        /// <param name="self"></param>
        public static void Refresh(this UIGmPlayerProperty self)
        {
            var pE_SL_GMPropertySwitch = UITools.FindChild(self.m_window, "E_SL_GMPropertySwitch");
            var pContent = UITools.FindChild(pE_SL_GMPropertySwitch, "Content");

            UITools.DestroyChildren(pContent);
            GameObject objItem = UIComponent.Instance.ItemGet("Item_GMPropertySwitch");
            //循环系统列表
            for (int i = 0; i < (int)ESystemType.Max; i++)
            {
                //刷新列表
                GameObject pItem = GameObject.Instantiate(objItem, pContent.transform);
                UITools.FindChildComponent<Text>(pItem, "ELb_System")?.SetText(self.GetSystemName(i));
                //带有系统枚举id的绑定回调
                UITools.FindChildComponent<Button>(pItem, "EBt_System")?.AddListenerAsyncWithId(self.OnClickSystemProp, i);
            }
        }

        public static string GetSystemName(this UIGmPlayerProperty self, int id)
        {
            string Name = "";
            switch (id)
            {
                case 0:
                    {
                        Name = "境界";
                        break;
                    }
                case 1:
                    {
                        Name = "神经";
                        break;
                    }
                case 2:
                    {
                        Name = "基因";
                        break;
                    }
                case 3:
                    {
                        Name = "装备";
                        break;
                    }
                case 4:
                    {
                        Name = "密药";
                        break;
                    }
            }
            return Name;
        }

        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="self"></param>
        public static void OnClickClose(this UIGmPlayerProperty self)
        {
            UIComponent.Instance.HideWindow("UIGmPlayerProperty");
        }

        /// <summary>
        /// 点击查询对应系统属性
        /// </summary>
        /// <param name="self"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async ETTask OnClickSystemProp(this UIGmPlayerProperty self, int id)
        {
            //发送网络消息
            C2Game_GmSystemProp pC2Game_GmSystemProp = new C2Game_GmSystemProp();
            pC2Game_GmSystemProp.nSystemEnumId = id;

            Scene zoneScene = self.DomainScene();
            SessionComponent pSessionComponent = zoneScene.GetComponent<SessionComponent>();

            Game2C_GmSystemProp pGame2C_GmSystemProp = (Game2C_GmSystemProp)await pSessionComponent.Session.Call(
                pC2Game_GmSystemProp);
            var pESV_List = UITools.FindChild(self.m_window, "E_SL_GMPropertyList");
            var pContent = UITools.FindChild(pESV_List, "Content");

            UITools.DestroyChildren(pContent);
            GameObject objItem = UIComponent.Instance.ItemGet("Item_GMPropertyList");
            // 返回成功
            if (pGame2C_GmSystemProp.Error == ErrorCode.ERR_Success)
            {
                foreach (var node in pGame2C_GmSystemProp.pListSystemProp)
                {
                    //刷新列表
                    GameObject pItem = GameObject.Instantiate(objItem, pContent.transform);
                    //配置表查询属性名
                    int nConfigId = PlayerPropertyConfigCategory.Instance.eType2ID((EPlayerPropertyType)node.nPropEnumId);
                    string szName = PlayerPropertyConfigCategory.Instance.Get(nConfigId).PropName;
                    UITools.FindChildComponent<Text>(pItem, "ELb_PropertyName")?.SetText(szName);
                    UITools.FindChildComponent<DoTweenObj>(pItem, "ELb_PropertyNumb")?.NumJump(0 , node.lPropNum);
                }
            }
        }

        /// <summary>
        /// 点击查询总属性
        /// </summary>
        /// <param name="self"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async ETTask OnClickTotalProp(this UIGmPlayerProperty self)
        {
            //发送网络消息
            C2Game_GmTotalProp pC2Game_GmTotalProp = new C2Game_GmTotalProp();

            Scene zoneScene = self.DomainScene();
            SessionComponent pSessionComponent = zoneScene.GetComponent<SessionComponent>();

            Game2C_GmTotalProp pGame2C_GmTotalProp = (Game2C_GmTotalProp)await pSessionComponent.Session.Call(
                pC2Game_GmTotalProp);
            var pESV_List = UITools.FindChild(self.m_window, "E_SL_GMPropertyList");
            var pContent = UITools.FindChild(pESV_List, "Content");

            UITools.DestroyChildren(pContent);
            GameObject objItem = UIComponent.Instance.ItemGet("Item_GMPropertyList");
            // 返回成功
            if (pGame2C_GmTotalProp.Error == ErrorCode.ERR_Success)
            {
                foreach (var node in pGame2C_GmTotalProp.pListTotalProp)
                {
                    //刷新列表
                    GameObject pItem = GameObject.Instantiate(objItem, pContent.transform);
                    //配置表查询属性名
                    int nConfigId = PlayerPropertyConfigCategory.Instance.eType2ID((EPlayerPropertyType)node.nPropEnumId);
                    string szName = PlayerPropertyConfigCategory.Instance.Get(nConfigId).PropName;
                    UITools.FindChildComponent<Text>(pItem, "ELb_PropertyName")?.SetText(szName);
                    UITools.FindChildComponent<DoTweenObj>(pItem, "ELb_PropertyNumb")?.NumJump(0, node.lPropNum);
                }
            }
        }

        /// <summary>
        /// 关闭所有选中框
        /// </summary>
        /// <param name="self"></param>
        public static void CloseAllSelectOn(this UIGmPlayerProperty self)
        {

        }
    }
}

