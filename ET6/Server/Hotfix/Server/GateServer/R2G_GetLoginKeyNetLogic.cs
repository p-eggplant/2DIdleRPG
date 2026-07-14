using System;


namespace ET
{
	[ActorMessageHandler]
	public class R2G_GetLoginKeyNetLogic : AMActorRpcHandler<Scene, R2G_GetLoginKey, G2R_GetLoginKey>
	{
		protected override async ETTask Run(Scene scene, R2G_GetLoginKey request, G2R_GetLoginKey response, Action reply)
		{
			long key = RandomHelper.RandInt64();
			scene.GetComponent<GateSessionKeyComponent>().Add(key, request.PlayerID, request.nGameServerID);
			response.Key = key;
			reply();
			await ETTask.CompletedTask;
		}
	}

}