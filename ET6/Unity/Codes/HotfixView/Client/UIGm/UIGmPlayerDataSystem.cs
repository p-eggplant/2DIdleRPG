/*----------------------------------------------------------------
* 文件名:	UIGMPlayerData
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/6/28 16:56:35
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

    public class UIGmPlayerData_IAwake : AwakeSystem<UIGmPlayerData>
    {
        public override void Awake(UIGmPlayerData self)
        {
            self.m_window = null;
        }
    }

    // 析构函数
    public class UIGmPlayerData_IDestroy : DestroySystem<UIGmPlayerData>
    {
        public override void Destroy(UIGmPlayerData self)
        {
            self.m_window = null;
        }
    }



    [FriendClassAttribute(typeof(UIGmPlayerData))]


    public static class UIGmPlayerDataSystem
    {
        public static void OnClickClose(this UIGmPlayerData self)
        {
            UIComponent.Instance.HideWindow("UIGmPlayerData");
        }

        /// <summary>
        /// Gm数值修改按钮
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async ETTask OnClickSet(this UIGmPlayerData self)
        {

            Player pPlayer = self.DomainScene().GetComponent<Player>();

            PlayerDataComponent pPlayerDataComponent = pPlayer.GetComponent<PlayerDataComponent>();
            if (pPlayerDataComponent == null)
            {
                throw new Exception("pPlayerDataComponent == null");
            }


            var pE_SL_GMDataList = UITools.FindChild(self.m_window, "E_SL_GMDataList");
            var pContent = UITools.FindChild(pE_SL_GMDataList, "Content");

            C2Game_GmDataChange pC2Game_GmDataChange = new C2Game_GmDataChange();



            for (int i = 0; i < pContent.transform.childCount;i++)
            {
                GameObject pChlid = pContent.transform.GetChild(i).gameObject;
                EPlayerDataType eType = EPlayerDataType.None;
                if(true == EPlayerDataType.TryParse(pChlid.name, out eType))
                {
                    string text = UITools.FindChildComponent<InputField>(pChlid, "EIf_Input").text;
                    long Num = long.Parse(text);
                    if(Num !=  pPlayerDataComponent.GetData(eType))
                    {
                        PlayerDataProto pData = new PlayerDataProto();
                        pData.EnumId = (int)eType;
                        pData.Value = Num;
                        pC2Game_GmDataChange.DataNodes.Add(pData);
                    }
                }   
            }

            // 发送网络消息
            Scene zoneScene = self.DomainScene();
            SessionComponent pSessionComponent = zoneScene.GetComponent<SessionComponent>();

            Game2C_GmDataChange pGame2C_GmDataChange = (Game2C_GmDataChange)await pSessionComponent.Session.Call(
                pC2Game_GmDataChange);
            // 返回成功
            if (pGame2C_GmDataChange.Error == ErrorCode.ERR_Success)
            {
                foreach (var node in pC2Game_GmDataChange.DataNodes)
                {
                    pPlayerDataComponent.SetData((EPlayerDataType)node.EnumId, node.Value);
                    UITools.Tips("数值修改成功");
                }
            }
            // 刷新列表
            self.OnShow();
        }

        /// <summary>
        /// GM数值界面刷新方法
        /// </summary>
        /// <param name="self"></param>
        /// <exception cref="Exception"></exception>
        public static void OnShow(this UIGmPlayerData self)
        {
            Player pPlayer = self.DomainScene().GetComponent<Player>();

            PlayerDataComponent pPlayerDataComponent = pPlayer.GetComponent<PlayerDataComponent>();
            if(pPlayerDataComponent == null) 
            {
                throw new Exception("pPlayerDataComponent == null");
            }

            var pE_SL_GMDataList = UITools.FindChild(self.m_window, "E_SL_GMDataList");
            var pContent = UITools.FindChild(pE_SL_GMDataList, "Content");
            UITools.DestroyChildren(pContent);

            GameObject objItem = UIComponent.Instance.ItemGet("Item_GMDataList");

            var dic =  pPlayerDataComponent.GetAllData();
            foreach( var item in dic )
            {
                GameObject pItem = GameObject.Instantiate(objItem, pContent.transform);
                pItem.name = item.Key.ToString();
                UITools.FindChildComponent<Text>(pItem, "ELb_DataNme")?.SetText(item.Key.ToString().Trans());
                UITools.FindChildComponent<InputField>(pItem, "EIf_Input")?.SetTextWithoutNotify(item.Value.ToString());

            }
        }
    }


}
