using System;


namespace ET
{
	[MessageHandler]
	[FriendClass(typeof(SessionPlayerComponent))]
	public class C2G_LoginGateHandler : AMRpcHandler<C2G_LoginGate, G2C_LoginGate>
	{
		protected override async ETTask Run(Session session, C2G_LoginGate request, G2C_LoginGate response, Action reply)
		{
			Scene scene = session.DomainScene();
			SessionKeyNode pSessionKeyNode = scene.GetComponent<GateSessionKeyComponent>().Get(request.Key);
			if (pSessionKeyNode == null)
			{
				response.Error = ErrorCore.ERR_ConnectGateKeyError;
				response.Message = "Gate key验证失败!";
				reply();
				return;
			}

			// 移除令牌
            scene.GetComponent<GateSessionKeyComponent>().Remove(request.Key);
			// 移除
            session.RemoveComponent<SessionAcceptTimeoutComponent>();


            // 向Game服务器请求玩家的 InstanceId
            StartSceneConfig gameStartSceneConfig = StartSceneConfigCategory.Instance.Get(pSessionKeyNode.ServerGameID);
            if (gameStartSceneConfig == null)
            {
				throw new Exception($"gameStartSceneConfig == null  ServerGameID = {pSessionKeyNode.ServerGameID}");
            }

            G2Game_PlayerLogin pG2Game_PlayerLogin = new G2Game_PlayerLogin();
            pG2Game_PlayerLogin.PlayerID = pSessionKeyNode.PlayerID;
            pG2Game_PlayerLogin.SessionInstanceId = session.InstanceId;
            Game2G_PlayerLogin pGame2G_PlayerLogin = (Game2G_PlayerLogin)await EMessageHelper.CallActor(gameStartSceneConfig.InstanceId, pG2Game_PlayerLogin);

            //
            SessionPlayerComponent pSessionPlayerComponent = session.AddComponent<SessionPlayerComponent>();
            pSessionPlayerComponent.PlayerInstanceId = pGame2G_PlayerLogin.PlayerInstanceId;
            pSessionPlayerComponent.PlayerId = pSessionKeyNode.PlayerID;
            pSessionPlayerComponent.ServerID = pSessionKeyNode.ServerGameID;

            session.AddComponent<MailBoxComponent, MailboxType>(MailboxType.GateSession);

            reply();
			await ETTask.CompletedTask;
		}
	}

    [FriendClass(typeof(SessionPlayerComponent))]
    public class C2G_LogoutGateHandler : AMRpcHandler<C2G_LogoutGate, G2C_LogoutGate>
	{
		protected override async ETTask Run(Session session, C2G_LogoutGate request, G2C_LogoutGate response, Action reply)
		{
            SessionPlayerComponent pSessionPlayerComponent = session.GetComponent<SessionPlayerComponent>();
            StartSceneConfig gameStartSceneConfig = StartSceneConfigCategory.Instance.Get(pSessionPlayerComponent.ServerID);
            if (gameStartSceneConfig == null)
            {
                throw new Exception($"gameStartSceneConfig == null  ServerGameID = {pSessionPlayerComponent.ServerID}");
            }

            G2Game_PlayerLogout pG2Game_PlayerLogout = new G2Game_PlayerLogout();
            pG2Game_PlayerLogout.PlayerID = pSessionPlayerComponent.PlayerId;
            Game2G_PlayerLogout pGame2G_PlayerLogout = (Game2G_PlayerLogout)await EMessageHelper.CallActor(gameStartSceneConfig.InstanceId, pG2Game_PlayerLogout);

            reply();
            await ETTask.CompletedTask;
        }
	}
}