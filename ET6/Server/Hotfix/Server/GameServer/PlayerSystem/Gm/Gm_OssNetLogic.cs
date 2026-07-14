/*----------------------------------------------------------------
* 文件名:	Gm_OssNetLogic
* 版  权:	(C) 深圳热区网络科技有限公司	
* 日  期:	2024/7/1 16:04:07
* 创建人:
* 描  述:	

*************************** 修改记录 ******************************
* 修改人: 
* 日  期: 
* 描  述: 
*******************************************************************/

using ET;
using System;
using System.Xml.Linq;

/// <summary>
/// 查询玩家数据日志
/// </summary>
[FriendClassAttribute(typeof(ET.PlayerOssComponent))]
public class C2Game_GmOssDataHandler : AMActorRpcHandler<Player, C2Game_GmOssData, Game2C_GmOssData>
{
    protected override async ETTask Run(Player player, C2Game_GmOssData request, Game2C_GmOssData response, Action reply)
    {
        try
        {
            //获取玩家日志组件
            PlayerOssComponent pPlayerOssComponent = player.GetComponent<PlayerOssComponent>();
            if (pPlayerOssComponent == null)
            {
                throw new Exception(" pPlayerOssComponent = null");
            }
            //设置查询起始和结束位置（一页20条日志）
            int nStartIndex = request.pageNum * 20;
            int nEndIndex = nStartIndex + 20;
            int count = pPlayerOssComponent.m_listDataOss.Count;
            if (nStartIndex >= count && nStartIndex != 0)
            {
                throw new Exception($"nStartIndex >= pPlayerOssComponent.m_listDataOss.Count " +
                    $"nStartIndex = {nStartIndex}, Count = {count}");
            }
            //若起始至结束不足20条
            if (nEndIndex >= count)
            {
                nEndIndex = count;
            }
            //设置循环头尾
            nStartIndex = count - nStartIndex - 1;
            nEndIndex = count - nEndIndex - 1;
            //设置日志数据
            for (int i = nStartIndex; i > nEndIndex; i--)
            {
                var node = pPlayerOssComponent.m_listDataOss[i];
                PlayerDataOss pDataNode = new PlayerDataOss();
                pDataNode.lTime = node.m_lTime;
                pDataNode.nConfigId = (int)node.m_nConfigID;
                pDataNode.lChangeNum = node.m_lChangeNum;
                pDataNode.lRealNum = node.m_lRealNum;
                pDataNode.szReason = node.m_szReason;
                response.pListDataOss.Add(pDataNode);
            }
            //设置日志总页数
            response.nTotalPage = pPlayerOssComponent.m_listDataOss.Count/20 + 1;
            reply();
            await ETTask.CompletedTask;
        }
        catch (Exception e)
        {
            Log.Error(e.ToString());
            response.Error = ErrorCode.ERR_SystemError;
            reply();
        }

    }
}

/// <summary>
/// 查询玩家背包日志
/// </summary>
[FriendClassAttribute(typeof(ET.PlayerOssComponent))]
public class C2Game_GmOssBagHandler : AMActorRpcHandler<Player, C2Game_GmOssBag, Game2C_GmOssBag>
{
    protected override async ETTask Run(Player player, C2Game_GmOssBag request, Game2C_GmOssBag response, Action reply)
    {
        try
        {
            //获取玩家日志组件
            PlayerOssComponent pPlayerOssComponent = player.GetComponent<PlayerOssComponent>();
            if (pPlayerOssComponent == null)
            {
                throw new Exception(" pPlayerOssComponent = null");
            }
            //设置查询起始和结束位置（一页20条日志）
            int nStartIndex = request.pageNum * 20;
            int nEndIndex = nStartIndex + 20;
            int count = pPlayerOssComponent.m_listMaterialOss.Count;
            if (nStartIndex >= count && nStartIndex != 0)
            {
                throw new Exception($"nStartIndex >= pPlayerOssComponent.m_listMaterialOss.Count " +
                    $"nStartIndex = {nStartIndex}, Count = {count}");
            }
            //若起始至结束不足20条
            if (nEndIndex >= count)
            {
                nEndIndex = count;
            }
            //设置循环头尾
            nStartIndex = count - nStartIndex - 1;
            nEndIndex = count - nEndIndex - 1;

            //设置日志数据
            for (int i = nStartIndex; i > nEndIndex; i--)
            {
                var node = pPlayerOssComponent.m_listMaterialOss[i];
                PlayerBagOss pDataNode = new PlayerBagOss();
                pDataNode.lTime = node.m_lTime;
                pDataNode.nConfigId = node.m_nConfigID;
                pDataNode.lChangeNum = node.m_nChangeNum;
                pDataNode.lRealNum = node.m_nRealNum;
                pDataNode.szReason = node.m_szReason;
                response.pListBagOss.Add(pDataNode);
            }
            //设置日志总页数
            response.nTotalPage = pPlayerOssComponent.m_listMaterialOss.Count / 20 + 1;
            reply();
            await ETTask.CompletedTask;
        }
        catch (Exception e)
        {
            Log.Error(e.ToString());
            response.Error = ErrorCode.ERR_SystemError;
            reply();
        }

    }
}

/// <summary>
/// 查询玩家背包日志
/// </summary>
[FriendClassAttribute(typeof(ET.PlayerOssComponent))]
public class C2Game_GmOssPropHandler : AMActorRpcHandler<Player, C2Game_GmOssProp, Game2C_GmOssProp>
{
    protected override async ETTask Run(Player player, C2Game_GmOssProp request, Game2C_GmOssProp response, Action reply)
    {
        try
        {
            //获取玩家日志组件
            PlayerOssComponent pPlayerOssComponent = player.GetComponent<PlayerOssComponent>();
            if (pPlayerOssComponent == null)
            {
                throw new Exception(" pPlayerOssComponent = null");
            }
            //设置查询起始和结束位置（一页20条日志）
            int nStartIndex = request.pageNum * 20;
            int nEndIndex = nStartIndex + 20;
            int count = pPlayerOssComponent.m_listPropertyOss.Count;
            if (nStartIndex >= count && nStartIndex != 0)
            {
                throw new Exception($"nStartIndex >= pPlayerOssComponent.m_listPropertyOss.Count " +
                    $"nStartIndex = {nStartIndex}, Count = {count}");
            }
            //若起始至结束不足20条
            if (nEndIndex >= count)
            {
                nEndIndex = count;
            }
            //设置循环头尾
            nStartIndex = count - nStartIndex - 1;
            nEndIndex = count - nEndIndex - 1;


            //设置日志数据
            for (int i = nStartIndex; i > nEndIndex; i--)
            {
                var node = pPlayerOssComponent.m_listPropertyOss[i];
                PlayerPropOss pDataNode = new PlayerPropOss();
                pDataNode.nPropEnumId           = (int)node.m_ePropType;
                pDataNode.nOssId                = (int)node.m_eOssType;
                pDataNode.nSystemId             = (int)node.m_eSystemType;
                pDataNode.dwChangeNum           = node.m_dwChangeNum;
                pDataNode.dwRealNum             = node.m_dwRealNum;
                pDataNode.szReason              = node.m_szReason;
                response.pListPropOss.Add(pDataNode);
            }
            //设置日志总页数
            response.nTotalPage = pPlayerOssComponent.m_listPropertyOss.Count / 20 + 1;
            reply();
            await ETTask.CompletedTask;
        }
        catch (Exception e)
        {
            Log.Error(e.ToString());
            response.Error = ErrorCode.ERR_SystemError;
            reply();
        }

    }
}