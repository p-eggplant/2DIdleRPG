using System;
using System.Net;


namespace ET
{
    [MessageHandler]
 
    [FriendClassAttribute(typeof(ET.Account))]

    /// 玩家账户登陆验证
    public class C2R_LoginAccountNetLogic : AMRpcHandler<C2R_LoginAccount, R2C_LoginAccount>
    {
        protected override async ETTask Run(Session session, C2R_LoginAccount request, R2C_LoginAccount response, Action reply)
        {
            
            DBComponent pBComponent = DBManagerComponent.Instance.GetZoneDB(session.DomainZone());
            var AccountInfolist = await pBComponent.Query<Account>(d => d.szAccount.Equals(request.Account));
            if(AccountInfolist == null)
            {
                throw new Exception("AccountInfolist == null");
            }

            if (request.Account == string.Empty)
            {
                throw new Exception("账号为空");
            }

            // 账号是否已经创建
            Account pAccount = session.AddChild<Account>();
            if (AccountInfolist.Count <= 0)
            {
                pAccount.szAccount = request.Account;
                pAccount.lCreateTime = TimeHelper.ServerNow();
                pAccount.lLastLoginTime = TimeHelper.ServerNow();
                pAccount.nAccountID = session.Domain.GetComponent<GenerateIdComponent>().GenerateIdByTimeAndServerid((uint)session.DomainScene().Id);
                pAccount.nLastGameServerID = 0;
                await pBComponent.Save<Account>(pAccount);
                //await pBComponent.UpdateProperty<Account>(pAccount, "nLastGameServerID", 100);
            }
            else
            {
                pAccount = AccountInfolist[0];
            }

            // 设置账户ID
            response.AccountID = pAccount.nAccountID;

            // 推荐区服信息
            if (pAccount.nLastGameServerID != 0)
            {
                response.DefaultGameServerId = pAccount.nLastGameServerID;
                response.DefaultGameServerName = GameServerInfoConfigCategory.Instance.Get(pAccount.nLastGameServerID)?.ServerName;
            }
            else
            {
                response.DefaultGameServerId = GameServerInfoConfigCategory.Instance.nDefaultGameServerID;
                response.DefaultGameServerName = GameServerInfoConfigCategory.Instance.szDefaultGameServerName;
            }

            reply();
            pAccount.Dispose();
            await ETTask.CompletedTask;
         
        }
    }



    /// <summary>
    /// 玩家请求获取服务器列表和角色列表
    /// </summary>
    [FriendClassAttribute(typeof(ET.AccountPlayer))]
    public class C2R_LoginServerPlayerListNetLogic : AMRpcHandler<C2R_LoginServerPlayerList, R2C_LoginServerPlayerList>
    {
        protected override async ETTask Run(Session session, C2R_LoginServerPlayerList request, R2C_LoginServerPlayerList response, Action reply)
        {

            // 玩家列表
            DBComponent pBComponent = DBManagerComponent.Instance.GetZoneDB(session.DomainZone());
            var pAccountPlayerlist = await pBComponent.Query<AccountPlayer>(d => d.lAccountID.Equals(request.AccountID));
            if (pAccountPlayerlist != null && pAccountPlayerlist.Count != 0)
            {
                for(int i= 0; i< pAccountPlayerlist.Count; i++)
                {
                    AccountPlayer pAccountPlayer = pAccountPlayerlist[i];
                    LoginPlayerInfoProto pLoginPlayerInfoProto = new LoginPlayerInfoProto();
                    pLoginPlayerInfoProto.nGameServerID = pAccountPlayer.nGameServerID;
                    pLoginPlayerInfoProto.szGameServerName = GameServerInfoConfigCategory.Instance.Get(pAccountPlayer.nGameServerID)?.ServerName;
                    pLoginPlayerInfoProto.lFightingCapacity = pAccountPlayer.lFightingCapacity;
                    pLoginPlayerInfoProto.szName = pAccountPlayer.szName;
                    response.HavePlayerList.Add(pLoginPlayerInfoProto);
                }
                
            }


            // 服务器列表
            var pGameServerDic = GameServerInfoConfigCategory.Instance.GetAll();
            if(pGameServerDic.Count >0)
            {
                foreach(var item in pGameServerDic)
                {
                    LoginServerInfoProto pLoginServerInfoProto = new LoginServerInfoProto();
                    pLoginServerInfoProto.nGameServerID = item.Value.Id;
                    pLoginServerInfoProto.szGameServerName = item.Value.ServerName;
                    response.ServerList.Add(pLoginServerInfoProto);
                }
            }

            reply();
            await ETTask.CompletedTask;
        }
    }




    /// <summary>
    /// 玩家请求进入服务器
    /// </summary>
    [FriendClassAttribute(typeof(ET.AccountPlayer))]
    [FriendClassAttribute(typeof(ET.Account))]
    public class C2R_GetGateAddressNetLogic : AMRpcHandler<C2R_GetGateAddress, R2C_GetGateAddress>
    {
        protected override async ETTask Run(Session session, C2R_GetGateAddress request, R2C_GetGateAddress response, Action reply)
        {

            // 先判断参数是否正确
            if(false == StartSceneConfigCategory.Instance.Contain(request.GameServerId))
            {
                response.Error = ErrorCode.ERR_SystemError;
                response.Message = $"不存在的服务器 id ={request.GameServerId}";
                reply();
                return;
            }


            // 获得数据库组件
            DBComponent pBComponent = DBManagerComponent.Instance.GetZoneDB(session.DomainZone());
            var AccountInfolist = await pBComponent.Query<Account>(d => d.nAccountID.Equals(request.AccountID));
            if (AccountInfolist == null || AccountInfolist.Count == 0)
            {
                response.Error = ErrorCode.ERR_SystemError;
                response.Message = $"不存在的账户 id ={request.AccountID}";
                reply();
                return;
            }

            Account pAccount = session.AddChild<Account>();
            pAccount = AccountInfolist[0];


            // 获取是否存在这样的账号，如果不存在则创建
            var pAccountPlayerlist = await pBComponent.Query<AccountPlayer>(d => d.lAccountID.Equals(request.AccountID) && d.nGameServerID.Equals(request.GameServerId));
            if (pAccountPlayerlist == null)
            {
                throw new Exception("pAccountPlayerlist == null");
            }

            AccountPlayer pAccountPlayer = session.AddChild<AccountPlayer>();
            if (pAccountPlayerlist.Count <= 0)
            {
                pAccountPlayer.lAccountID = request.AccountID;
                pAccountPlayer.nGameServerID = request.GameServerId;
                pAccountPlayer.lPlayerID = session.Domain.GetComponent<GenerateIdComponent>().GenerateIdByTimeAndServerid((uint)session.DomainScene().Id);
                pAccountPlayer.lCreateTime = TimeHelper.ServerNow();
                pAccountPlayer.szName = pAccountPlayer.lPlayerID.ToString();
                pAccountPlayer.lFightingCapacity = 0;
                await pBComponent.Save<AccountPlayer>(pAccountPlayer);
            }
            else
            {
                pAccountPlayer = pAccountPlayerlist[0];
            }



            StartSceneConfig pServerConfig = StartSceneConfigCategory.Instance.Get(request.GameServerId);
            // 随机分配一个Gate
            StartSceneConfig config = RealmGateAddressHelper.GetGate(pServerConfig.Zone);


            // 将PlayerID传给网关，并且获得网关的令牌
            G2R_GetLoginKey g2RGetLoginKey = (G2R_GetLoginKey)await ActorMessageSenderComponent.Instance.Call(
                config.InstanceId, new R2G_GetLoginKey() { PlayerID = pAccountPlayer.lPlayerID , nGameServerID = pAccountPlayer.nGameServerID});


            // 修改账户上次存储的ServerID;
            pAccount.nLastGameServerID = request.GameServerId;
            await pBComponent.Save<Account>(pAccount);


            response.Address = config.OuterIPPort.ToString();
            response.Key = g2RGetLoginKey.Key;

            pAccountPlayer.Dispose();
            reply();

            


            await ETTask.CompletedTask;
        }
    }
}