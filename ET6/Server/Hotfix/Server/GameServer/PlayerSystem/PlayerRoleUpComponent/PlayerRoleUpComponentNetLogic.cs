using ET;
using System;
using System.Xml.Linq;

/// <summary>
/// 玩家升级业务逻辑
/// </summary>
[FriendClassAttribute(typeof(ET.PlayerRoleUpComponent))]
public class C2Game_PlayerRoleUpHandler : AMActorRpcHandler<Player, C2Game_PlayerRoleUp, Game2C_PlayerRoleUp>
{
    protected override async ETTask Run(Player player, C2Game_PlayerRoleUp request, Game2C_PlayerRoleUp response, Action reply)
    {
        try
        {
            string szRes = string.Empty;
            //获取玩家等级组件
            PlayerRoleUpComponent pPlayerRoleUpComponent = player.GetComponent<PlayerRoleUpComponent>();
            PlayerBagComponent pPlayerBagComponent = player.GetComponent<PlayerBagComponent>();
            PlayerDataComponent pPlayerDataComponent = player.GetComponent<PlayerDataComponent>();
            if (pPlayerRoleUpComponent == null)
            {
                response.Error = ErrorCode.ERR_SystemError;
                response.Message = "玩家等级组件异常";
                reply();
                return;
            }

            if (pPlayerBagComponent == null)
            {
                response.Error = ErrorCode.ERR_SystemError;
                response.Message = "玩家背包组件异常";
                reply();
                return;
            }

            if (pPlayerDataComponent == null)
            {
                response.Error = ErrorCode.ERR_SystemError;
                response.Message = "玩家货币组件异常";
                reply();
                return;
            }
            int nCurrentLevel = pPlayerRoleUpComponent.GetRoleCurrentLevel();
            if (nCurrentLevel >= RoleLevelUpConfigCategory.Instance.GetMaxLevel())
            {
                response.Error = ErrorCode.ERR_SystemError;
                response.Message = "该角色已经满级，无法继续升级";
                reply();
                return;
            }
            if (RoleLevelUpConfigCategory.Instance.Contain(nCurrentLevel + 1) == false)
            {
                response.Error = ErrorCode.ERR_SystemError;
                response.Message = "目标等级不存在";
                reply();
                return;
            }

            RoleLevelUpConfig roleLevelUpConfig = RoleLevelUpConfigCategory.Instance.Get(nCurrentLevel + 1);

            //Id安全判断
            if (false == MaterialConfigCategory.Instance.Contain(roleLevelUpConfig.MaterialId))
            {
                response.Error = ErrorCode.ERR_SystemError;
                response.Message = "升级所需的物资1不存在";
                reply();
                return;
            }
            if (false == PlayerDataConfigCategory.Instance.Contain(roleLevelUpConfig.CurrencyId))
            {
                response.Error = ErrorCode.ERR_SystemError;
                response.Message = "升级所需的物资2不存在";
                reply();
                return;
            }
            var dataConfig =  PlayerDataConfigCategory.Instance.Get(roleLevelUpConfig.CurrencyId);
            //是否可移除物资
            if (false == pPlayerBagComponent.isCanRemoveItem(roleLevelUpConfig.MaterialId, roleLevelUpConfig.MaterialNumb))
            {
                response.Error = ErrorCode.ERR_SystemError;
                response.Message = "物资数量不足，ID: " + roleLevelUpConfig.MaterialId;
                reply();
                return;
            }
            //是否可以移除货币
            if (false == pPlayerDataComponent.isCanSubData(dataConfig.EDataType, roleLevelUpConfig.CurrencyNumb))
            {
                response.Error = ErrorCode.ERR_SystemError;
                response.Message = "货币数量不足，ID: " + roleLevelUpConfig.CurrencyId;
                reply();
                return;
            }
            //是否可以升级
            if (pPlayerRoleUpComponent.IsCanLevelUp(out szRes) == false)
            {
                response.Error = ErrorCode.ERR_SystemError;
                response.Message = szRes;
                reply();
                return;
            }

            //升级
            response.CurLevel = pPlayerRoleUpComponent.LevelUp();
            //物资扣除
            pPlayerBagComponent.RemoveItem(roleLevelUpConfig.MaterialId, roleLevelUpConfig.MaterialNumb, EOssType.BagSystem, "升级");
            pPlayerDataComponent.SubData(dataConfig.EDataType, roleLevelUpConfig.CurrencyNumb, EOssType.BagSystem, "升级");
            //即时存盘
            pPlayerRoleUpComponent.SetOne();
            reply();
            await ETTask.CompletedTask;
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            response.Error = ErrorCode.ERR_SystemError;
            response.Message = ex.Message;
            reply();
        }

        await ETTask.CompletedTask;
    }
}