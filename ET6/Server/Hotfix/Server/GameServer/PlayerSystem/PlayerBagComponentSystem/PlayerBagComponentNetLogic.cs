using ET;
using System;
using System.Xml.Linq;

/// <summary>
/// 查询玩家数据日志
/// </summary>
[FriendClassAttribute(typeof(ET.PlayerBagComponent))]
public class C2Game_BagSellHandler : AMActorRpcHandler<Player, C2Game_BagSell, Game2C_BagSell>
{
    protected override async ETTask Run(Player player, C2Game_BagSell request, Game2C_BagSell response, Action reply)
    {
        //获取玩家背包组件
        PlayerBagComponent pPlayerBagComponent = player.GetComponent<PlayerBagComponent>();
        if (pPlayerBagComponent == null)
        {
            throw new Exception(message: " pPlayerBagComponent = null");
        }

        //Id安全判断
        if (false == MaterialConfigCategory.Instance.Contain(request.nMaterialId))
        {
            throw new Exception($"false == MaterialConfigCategory.Instance.Contain(request.nMaterialId)  nMaterialId = {request.nMaterialId}");
        }
        //是否可移除物资
        if(false == pPlayerBagComponent.isCanRemoveItem(request.nMaterialId, 1))
        {
            response.Error = ErrorCode.ERR_SystemError; 
            response.Message = "物资数量不足，ID: " + request.nMaterialId;
            reply();
            return;
        }

        //是否可添加物资
        if(false == pPlayerBagComponent.isCanAddItem(103, 5))
        {
            response.Error = ErrorCode.ERR_SystemError;
            response.Message = "不可添加，ID: " + 103;
            reply();
            return;
        }

        //执行移除
        pPlayerBagComponent.RemoveItem(request.nMaterialId, 1, EOssType.BagSystem, "售卖物品");

        //执行添加
        pPlayerBagComponent.AddItem(103, 5, EOssType.BagSystem, "售卖物品");

            
        reply();
        await ETTask.CompletedTask;
    }
}