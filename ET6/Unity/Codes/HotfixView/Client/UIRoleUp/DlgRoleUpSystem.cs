using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class DlgRoleUp_IAwake : AwakeSystem<DlgRoleUp>
    {
        public override void Awake(DlgRoleUp self)
        {
            self.m_window = null;
        }
    }

    public class DlgRoleUp_IDestroy : DestroySystem<DlgRoleUp>
    {
        public override void Destroy(DlgRoleUp self)
        {
            self.m_window = null;
        }
    }

    [FriendClassAttribute(typeof(DlgRoleUp))]
    [FriendClassAttribute(typeof(PlayerRoleUpComponent))]
    public static class DlgRoleUpSystem
    {
        public static void OnRefresh(this DlgRoleUp self)
        {
            Player pPlayer = self.DomainScene().GetComponent<Player>();
            if (pPlayer == null)
                return;

            PlayerRoleUpComponent pRoleUp = pPlayer.GetComponent<PlayerRoleUpComponent>();
            PlayerBagComponent pBag = pPlayer.GetComponent<PlayerBagComponent>();
            PlayerDataComponent pData = pPlayer.GetComponent<PlayerDataComponent>();
            if (pRoleUp == null)
                return;

            int curLevel = pRoleUp.m_CurLevel;
            bool isMaxLevel = RoleLevelUpConfigCategory.Instance.IsMaxLevel(curLevel);

            // 等级数字
            UITools.FindChildComponent<Text>(self.m_window, "ELb_LevelNumb")
                ?.SetText(curLevel.ToString());

            // 当前等级配置
            RoleLevelUpConfig curConfig = RoleLevelUpConfigCategory.Instance.Get(curLevel);

            // 属性1
            int propId1 = curConfig.PropTypeId1;
            int propData1 = curConfig.PropTypeData1;
            PlayerPropertyConfig propConfig1 = PlayerPropertyConfigCategory.Instance.Get(propId1);
            UITools.FindChildComponent<Text>(self.m_window, "Lb_Atk")?.SetText(propConfig1.PropName);
            UITools.FindChildComponent<Text>(self.m_window, "ELb_NowAtk")?.SetText(propData1.ToString());

            // 属性2
            int propId2 = curConfig.PropTypeId2;
            int propData2 = curConfig.PropTypeData2;
            PlayerPropertyConfig propConfig2 = PlayerPropertyConfigCategory.Instance.Get(propId2);
            UITools.FindChildComponent<Text>(self.m_window, "Lb_AtkBlood")?.SetText(propConfig2.PropName);
            UITools.FindChildComponent<Text>(self.m_window, "ELb_NowBlood")?.SetText(propData2.ToString());

            // 下一级数据（非满级时显示）
            if (!isMaxLevel)
            {
                RoleLevelUpConfig nextConfig = RoleLevelUpConfigCategory.Instance.GetNextConfig(curLevel);

                int nextProp1 = nextConfig.PropTypeData1;
                int nextProp2 = nextConfig.PropTypeData2;

                UITools.FindChildComponent<Text>(self.m_window, "ELb_NextAtk")
                    ?.SetText($"+{nextProp1 - propData1}");
                UITools.FindChildComponent<Text>(self.m_window, "ELb_NextBlood")
                    ?.SetText($"+{nextProp2 - propData2}");

                // 材料（材料ID > 0 时显示）
                bool hasMaterial = nextConfig.MaterialId > 0 && MaterialConfigCategory.Instance.Contain(nextConfig.MaterialId);
                if (hasMaterial)
                {
                    MaterialConfig matConfig = MaterialConfigCategory.Instance.Get(nextConfig.MaterialId);
                    int haveNum = pBag != null ? pBag.GetItemNum(nextConfig.MaterialId) : 0;
                    UITools.FindChildComponent<Text>(self.m_window, "ELb_MaterialName1")
                        ?.SetText(matConfig.Name);
                    UITools.FindChildComponent<Text>(self.m_window, "ELb_MaterialNumb1")
                        ?.SetText($"{haveNum}/{nextConfig.MaterialNumb}");
                }
                else
                {
                    UITools.FindChildComponent<Text>(self.m_window, "ELb_MaterialName1")?.SetText("");
                    UITools.FindChildComponent<Text>(self.m_window, "ELb_MaterialNumb1")?.SetText("");
                }

                // 货币（货币ID > 0 时显示）
                if (nextConfig.CurrencyId > 0 && PlayerDataConfigCategory.Instance.Contain(nextConfig.CurrencyId))
                {
                    PlayerDataConfig dataConfig = PlayerDataConfigCategory.Instance.Get(nextConfig.CurrencyId);
                    long haveCurrency = pData != null ? pData.GetData(nextConfig.GetCurrencyEType()) : 0;
                    UITools.FindChildComponent<Text>(self.m_window, "ELb_MaterialName2")
                        ?.SetText(dataConfig.DataName);
                    UITools.FindChildComponent<Text>(self.m_window, "ELb_MaterialNumb2")
                        ?.SetText($"{haveCurrency}/{nextConfig.CurrencyNumb}");
                }
                else
                {
                    UITools.FindChildComponent<Text>(self.m_window, "ELb_MaterialName2")?.SetText("");
                    UITools.FindChildComponent<Text>(self.m_window, "ELb_MaterialNumb2")?.SetText("");
                }

                // 描述
                UITools.FindChildComponent<Text>(self.m_window, "Lb_Describe")
                    ?.SetText($"下一级: {propConfig1.PropName}+{nextProp1 - propData1}, {propConfig2.PropName}+{nextProp2 - propData2}");
            }
            else
            {
                // 满级：隐藏下一级相关控件
                UITools.FindChildComponent<Text>(self.m_window, "ELb_NextAtk")?.SetText("");
                UITools.FindChildComponent<Text>(self.m_window, "ELb_NextBlood")?.SetText("");
                UITools.FindChildComponent<Text>(self.m_window, "ELb_MaterialName1")?.SetText("");
                UITools.FindChildComponent<Text>(self.m_window, "ELb_MaterialNumb1")?.SetText("");
                UITools.FindChildComponent<Text>(self.m_window, "ELb_MaterialName2")?.SetText("");
                UITools.FindChildComponent<Text>(self.m_window, "ELb_MaterialNumb2")?.SetText("");
                UITools.FindChildComponent<Text>(self.m_window, "Lb_Describe")
                    ?.SetText("已达最大等级");
            }
        }

        public static void OnClickClose(this DlgRoleUp self)
        {
            UIComponent.Instance.HideWindow("DlgRoleUp");
        }

        public static async ETTask OnClickLevelUp(this DlgRoleUp self)
        {
            Scene zoneScene = self.DomainScene();
            Player pPlayer = zoneScene.GetComponent<Player>();
            if (pPlayer == null)
                return;

            PlayerRoleUpComponent pRoleUp = pPlayer.GetComponent<PlayerRoleUpComponent>();
            PlayerBagComponent pBag = pPlayer.GetComponent<PlayerBagComponent>();
            PlayerDataComponent pData = pPlayer.GetComponent<PlayerDataComponent>();

            if (pRoleUp == null)
                return;

            // 满级校验
            if (RoleLevelUpConfigCategory.Instance.IsMaxLevel(pRoleUp.m_CurLevel))
            {
                UITools.Tips("已达最大等级");
                return;
            }

            RoleLevelUpConfig config = RoleLevelUpConfigCategory.Instance.GetNextConfig(pRoleUp.m_CurLevel);

            // 材料校验
            if (pBag == null || pBag.GetItemNum(config.MaterialId) < config.MaterialNumb)
            {
                UITools.Tips("材料不足");
                return;
            }

            // 货币校验
            if (pData == null || pData.GetData(config.GetCurrencyEType()) < config.CurrencyNumb)
            {
                UITools.Tips("货币不足");
                return;
            }

            SessionComponent pSessionComponent = zoneScene.GetComponent<SessionComponent>();
            Game2C_PlayerRoleUp pGame2C_PlayerRoleUp = (Game2C_PlayerRoleUp)await pSessionComponent.Session.Call(
                new C2Game_PlayerRoleUp());

            if (pGame2C_PlayerRoleUp.Error != ErrorCode.ERR_Success)
            {
                UITools.Tips($"升级失败: {pGame2C_PlayerRoleUp.Message}");
                return;
            }

            pRoleUp.m_CurLevel = pGame2C_PlayerRoleUp.CurLevel;

            UITools.Tips($"升级成功！当前等级: {pGame2C_PlayerRoleUp.CurLevel}");
            self.OnRefresh();
        }
    }
}
