/*----------------------------------------------------------------
* 文件名:	UIGmPlayerOssSystem
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/7/1 14:58:14
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace ET
{
    // 构造函数
    public class UIGmPlayerOss_IAwake : AwakeSystem<UIGmPlayerOss>
    {
        public override void Awake(UIGmPlayerOss self)
        {
            self.m_window = null;
        }
    }

    // 析构函数
    public class UIGmPlayerOss_IDestroy : DestroySystem<UIGmPlayerOss>
    {
        public override void Destroy(UIGmPlayerOss self)
        {
            self.m_window = null;
        }
    }


    [FriendClassAttribute(typeof(UIGmPlayerOss))]
    public static class UIGmPlayerOssSystem
    {
        public static void OnClickClose(this UIGmPlayerOss self)
        {
            UIComponent.Instance.HideWindow("UIGmPlayerOss");
        }


        /// <summary>
        /// 点击上一页按钮
        /// </summary>
        /// <param name="self"></param>
        public static void OnClickLastPage(this UIGmPlayerOss self)
        {
            if (self.m_nPageNum <= 0)
            {
                UITools.Tips("已经是第一页！！没有上一页了！！");
                return;
            }
            self.m_nPageNum--;
            self.OnShow();
        }

        /// <summary>
        /// 点击下一页按钮
        /// </summary>
        /// <param name="self"></param>
        public static void OnClickNextPage(this UIGmPlayerOss self)
        {
            self.m_nPageNum++;
            self.OnShow();
        }
        public static void OnShow(this UIGmPlayerOss self)
        {
            //根据类型执行点击
            switch (self.m_eType)
            {
                case EOssPageType.DataSystem:
                    {
                        self.OnClickData().Coroutine();
                        break;
                    }
                case EOssPageType.BagSystem:
                    {
                        self.OnClickBag().Coroutine();
                        break;
                    }
                case EOssPageType.PropSystem:
                    {
                        self.OnClickProp().Coroutine();
                        break;
                    }
                default:
                    throw new Exception("未找到的日志枚举类型");
            }
            
        }

        /// <summary>
        /// 根据枚举类型显示对应Oss界面
        /// </summary>
        /// <param name="self"></param>
        /// <param name="mType"></param>
        /// <exception cref="Exception"></exception>
        public static void OnShow(this UIGmPlayerOss self, int mType)
        {
            self.m_eType = (EOssPageType)mType;
            //根据类型执行点击
            switch (self.m_eType)
            {
                case EOssPageType.DataSystem:
                    {
                        self.OnClickData().Coroutine();
                        break;
                    }
                case EOssPageType.BagSystem:
                    {
                        self.OnClickBag().Coroutine();
                        break;
                    }
                case EOssPageType.PropSystem:
                    {
                        self.OnClickProp().Coroutine();
                        break;
                    }
                default:
                    throw new Exception("未找到的日志枚举类型");
            }
        }

        /// <summary>
        /// 点击数值日志系统
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static async ETTask OnClickData(this UIGmPlayerOss self)
        {
            //发送网络消息
            C2Game_GmOssData pC2Game_GmOssData = new C2Game_GmOssData();
            pC2Game_GmOssData.pageNum = self.m_nPageNum;

            Scene zoneScene = self.DomainScene();
            SessionComponent pSessionComponent = zoneScene.GetComponent<SessionComponent>();

            Game2C_GmOssData pGame2C_GmOssData = (Game2C_GmOssData)await pSessionComponent.Session.Call(
                pC2Game_GmOssData);
            //列表初始化
            var pESV_List = UITools.FindChild(self.m_window, "E_SL_GMOssList");
            var pContent = UITools.FindChild(pESV_List, "Content");

            UITools.DestroyChildren(pContent);
            GameObject objItem = UIComponent.Instance.ItemGet("Item_GMOssList");
            // 返回成功
            if (pGame2C_GmOssData.Error == ErrorCode.ERR_Success)
            {
                // 下一页按钮显隐
                if (self.m_nPageNum + 1 >= pGame2C_GmOssData.nTotalPage)
                {
                    UITools.FindChild(self.m_window, "EBt_NextPage")?.SetActive(false);
                }
                else
                {
                    UITools.FindChild(self.m_window, "EBt_NextPage")?.SetActive(true);
                }
                foreach (var node in pGame2C_GmOssData.pListDataOss)
                {
                    //刷新列表
                    GameObject pItem = GameObject.Instantiate(objItem, pContent.transform);
                    EPlayerDataType eType = (EPlayerDataType)node.nConfigId;
                    UITools.FindChildComponent<Text>(pItem, "ELb_EnumName")?.SetText(eType.ToString().Trans());
                    //设置改变值正负号
                    string szChangeNum = "";
                    if(node.lChangeNum > 0)
                    {
                        szChangeNum += "+";
                    }
                    szChangeNum += node.lChangeNum;
                    UITools.FindChildComponent<Text>(pItem, "ELb_ChangeNum")?.SetText(szChangeNum);
                    UITools.FindChildComponent<Text>(pItem, "ELb_FinalNum")?.SetText("" + node.lRealNum.ToString());
                    UITools.FindChildComponent<Text>(pItem, "ELb_Reason")?.SetText(node.szReason);
                    //设置时间
                    DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                    long lTime = long.Parse(node.lTime + "0000");
                    TimeSpan toNow = new TimeSpan(lTime);
                    DateTime dtResult = dtStart.Add(toNow);

                    UITools.FindChildComponent<Text>(pItem, "ELb_Time")?.SetText(dtResult.ToString());
                }
            }
            UITools.FindChildComponent<Text>(self.m_window, "ELb_Page")?.SetText(self.m_nPageNum + 1 + "/" + pGame2C_GmOssData.nTotalPage);
        }

        /// <summary>
        /// 点击背包日志系统
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static async ETTask OnClickBag(this UIGmPlayerOss self)
        {
            //发送网络消息
            C2Game_GmOssBag pC2Game_GmOssBag = new C2Game_GmOssBag();
            pC2Game_GmOssBag.pageNum = self.m_nPageNum;

            Scene zoneScene = self.DomainScene();
            SessionComponent pSessionComponent = zoneScene.GetComponent<SessionComponent>();

            Game2C_GmOssBag pGame2C_GmOssBag = (Game2C_GmOssBag)await pSessionComponent.Session.Call(
                pC2Game_GmOssBag);
            var pESV_List = UITools.FindChild(self.m_window, "E_SL_GMOssList");
            var pContent = UITools.FindChild(pESV_List, "Content");


            UITools.DestroyChildren(pContent);
            GameObject objItem = UIComponent.Instance.ItemGet("Item_GMOssList");
            // 返回成功
            if (pGame2C_GmOssBag.Error == ErrorCode.ERR_Success)
            {
                // 下一页按钮显隐
                if (self.m_nPageNum + 1 >= pGame2C_GmOssBag.nTotalPage)
                {
                    UITools.FindChild(self.m_window, "EBt_NextPage")?.SetActive(false);
                }
                else
                {
                    UITools.FindChild(self.m_window, "EBt_NextPage")?.SetActive(true);
                }
                foreach (var node in pGame2C_GmOssBag.pListBagOss)
                {
                    //刷新列表
                    GameObject pItem = GameObject.Instantiate(objItem, pContent.transform);
                    string szName = MaterialConfigCategory.Instance.Get(node.nConfigId).Name;
                    UITools.FindChildComponent<Text>(pItem, "ELb_EnumName")?.SetText(szName);
                    //设置改变值正负号
                    string szChangeNum = "";
                    if (node.lChangeNum > 0)
                    {
                        szChangeNum += "+";
                    }
                    szChangeNum += node.lChangeNum;
                    UITools.FindChildComponent<Text>(pItem, "ELb_ChangeNum")?.SetText(szChangeNum);
                    UITools.FindChildComponent<Text>(pItem, "ELb_FinalNum")?.SetText("" + node.lRealNum.ToString());
                    UITools.FindChildComponent<Text>(pItem, "ELb_Reason")?.SetText(node.szReason);
                    //获取DateTime时间
                    DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                    long lTime = long.Parse(node.lTime + "0000");
                    TimeSpan toNow = new TimeSpan(lTime);
                    DateTime dtResult = dtStart.Add(toNow);
                    UITools.FindChildComponent<Text>(pItem, "ELb_Time")?.SetText(dtResult.ToString());
                }
            }
            UITools.FindChildComponent<Text>(self.m_window, "ELb_Page")?.SetText(self.m_nPageNum + 1 + "/" + pGame2C_GmOssBag.nTotalPage);
        }

        /// <summary>
        /// 点击属性日志系统
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static async ETTask OnClickProp(this UIGmPlayerOss self)
        {
            //发送网络消息
            C2Game_GmOssProp pC2Game_GmOssProp = new C2Game_GmOssProp();
            pC2Game_GmOssProp.pageNum = self.m_nPageNum;

            Scene zoneScene = self.DomainScene();
            SessionComponent pSessionComponent = zoneScene.GetComponent<SessionComponent>();

            Game2C_GmOssProp pGame2C_GmOssProp = (Game2C_GmOssProp)await pSessionComponent.Session.Call(
                pC2Game_GmOssProp);
            var pESV_List = UITools.FindChild(self.m_window, "E_SL_GMOssList");
            var pContent = UITools.FindChild(pESV_List, "Content");


            UITools.DestroyChildren(pContent);
            GameObject objItem = UIComponent.Instance.ItemGet("Item_GMOssList");
            // 返回成功
            if (pGame2C_GmOssProp.Error == ErrorCode.ERR_Success)
            {
                // 下一页按钮显隐
                if (self.m_nPageNum + 1 >= pGame2C_GmOssProp.nTotalPage)
                {
                    UITools.FindChild(self.m_window, "EBt_NextPage")?.SetActive(false);
                }
                else
                {
                    UITools.FindChild(self.m_window, "EBt_NextPage")?.SetActive(true);
                }
                foreach (var node in pGame2C_GmOssProp.pListPropOss)
                {
                    //刷新列表
                    GameObject pItem = GameObject.Instantiate(objItem, pContent.transform);
                    //配置表查询属性名
                    int nConfigId = PlayerPropertyConfigCategory.Instance.eType2ID((EPlayerPropertyType)node.nPropEnumId);
                    string szName = PlayerPropertyConfigCategory.Instance.Get(nConfigId).PropName;
                    UITools.FindChildComponent<Text>(pItem, "ELb_EnumName")?.SetText(szName);
                    //设置改变值正负号
                    string szChangeNum = "";
                    if (node.dwChangeNum > 0)
                    {
                        szChangeNum += "+";
                    }
                    szChangeNum += node.dwChangeNum;

                    UITools.FindChildComponent<Text>(pItem, "ELb_ChangeNum")?.SetText(szChangeNum);
                    UITools.FindChildComponent<Text>(pItem, "ELb_FinalNum")?.SetText("" + node.dwRealNum.ToString());
                    UITools.FindChildComponent<Text>(pItem, "ELb_Reason")?.SetText(node.szReason);
                    EOssType eOssType = (EOssType)node.nOssId;
                    ESystemType eSystemType = (ESystemType)node.nSystemId;
                    UITools.FindChildComponent<Text>(pItem, "ELb_Time")?.SetText($"{eOssType.ToString().Trans()}>>{eSystemType.ToString().Trans()}");
                }
            }
            UITools.FindChildComponent<Text>(self.m_window, "ELb_Page")?.SetText(self.m_nPageNum + 1 + "/" + pGame2C_GmOssProp.nTotalPage);
        }
    }
}
