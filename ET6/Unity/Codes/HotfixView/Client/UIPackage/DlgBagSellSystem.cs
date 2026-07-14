using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace ET
{
    // 构造函数
    public class DlgBagSell_IAwake : AwakeSystem<DlgBagSell>
    {
        public override void Awake(DlgBagSell self)
        {
            self.m_window = null;
        }
    }

    // 析构函数
    public class DlgBagSell_IDestroy : DestroySystem<DlgBagSell>
    {
        public override void Destroy(DlgBagSell self)
        {
            self.m_window = null;
        }
    }


    [FriendClassAttribute(typeof(DlgBagSell))]
    public static class DlgBagSellSystem
    {
        public static void OnClickClose(this DlgBagSell self)
        {
            UIComponent.Instance.HideWindow("DlgBagSell");
        }

        /// <summary>
        /// 售卖物品
        /// </summary>
        /// <param name="self"></param>
        /// <param name="self.m_nMaterialId">物资Id</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async ETTask OnClickSell(this DlgBagSell self)
        {
            Player pPlayer = self.DomainScene().GetComponent<Player>();
            //获取玩家组件
            PlayerBagComponent pPlayerBagComponent = pPlayer.GetComponent<PlayerBagComponent>();
            if (pPlayerBagComponent == null)
            {
                throw new Exception("pPlayerBagComponent == null");
            }
            ////是否可移除物资
            //pPlayerBagComponent.isCanRemoveItem(request.self.m_nMaterialId, 1);

            ////是否可添加物资
            //pPlayerBagComponent.isCanAddItem(103, 5);

            if (false == MaterialConfigCategory.Instance.Contain(self.m_nMaterialId))
            {
                throw new Exception("材料配置未找到 self.m_nMaterialId = " + self.m_nMaterialId);
            }

            Scene zoneScene = self.DomainScene();
            C2Game_BagSell pC2Game_BagSell = new C2Game_BagSell();
            pC2Game_BagSell.nMaterialId = self.m_nMaterialId;

            SessionComponent pSessionComponent = zoneScene.GetComponent<SessionComponent>();
            Game2C_BagSell pGame2C_BagSell = (Game2C_BagSell)await pSessionComponent.Session.Call(
                pC2Game_BagSell);
            // 返回成功
            if (pGame2C_BagSell.Error != ErrorCode.ERR_Success)
            {
                UITools.Tips("售卖失败，错误码：" + pGame2C_BagSell.Error.ToString());
                return;
            }
            UITools.Tips("售卖成功");

            //调用背包界面刷新方法
            UIComponent.Instance.GetWindow<UIPackage>()?.OnRefresh();

            self.OnClickClose();
        }
    }
}

