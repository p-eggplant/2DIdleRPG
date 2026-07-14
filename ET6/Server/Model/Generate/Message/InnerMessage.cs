using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
	[ResponseType(nameof(ObjectQueryResponse))]
	[Message(InnerOpcode.ObjectQueryRequest)]
	[ProtoContract]
	public partial class ObjectQueryRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long Key { get; set; }

		[ProtoMember(2)]
		public long InstanceId { get; set; }

	}

	[ResponseType(nameof(A2M_Reload))]
	[Message(InnerOpcode.M2A_Reload)]
	[ProtoContract]
	public partial class M2A_Reload: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(InnerOpcode.A2M_Reload)]
	[ProtoContract]
	public partial class A2M_Reload: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(G2G_LockResponse))]
	[Message(InnerOpcode.G2G_LockRequest)]
	[ProtoContract]
	public partial class G2G_LockRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long Id { get; set; }

		[ProtoMember(2)]
		public string Address { get; set; }

	}

	[Message(InnerOpcode.G2G_LockResponse)]
	[ProtoContract]
	public partial class G2G_LockResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(G2G_LockReleaseResponse))]
	[Message(InnerOpcode.G2G_LockReleaseRequest)]
	[ProtoContract]
	public partial class G2G_LockReleaseRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long Id { get; set; }

		[ProtoMember(2)]
		public string Address { get; set; }

	}

	[Message(InnerOpcode.G2G_LockReleaseResponse)]
	[ProtoContract]
	public partial class G2G_LockReleaseResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(ObjectAddResponse))]
	[Message(InnerOpcode.ObjectAddRequest)]
	[ProtoContract]
	public partial class ObjectAddRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long Key { get; set; }

		[ProtoMember(2)]
		public long InstanceId { get; set; }

	}

	[Message(InnerOpcode.ObjectAddResponse)]
	[ProtoContract]
	public partial class ObjectAddResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(ObjectLockResponse))]
	[Message(InnerOpcode.ObjectLockRequest)]
	[ProtoContract]
	public partial class ObjectLockRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long Key { get; set; }

		[ProtoMember(2)]
		public long InstanceId { get; set; }

		[ProtoMember(3)]
		public int Time { get; set; }

	}

	[Message(InnerOpcode.ObjectLockResponse)]
	[ProtoContract]
	public partial class ObjectLockResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(ObjectUnLockResponse))]
	[Message(InnerOpcode.ObjectUnLockRequest)]
	[ProtoContract]
	public partial class ObjectUnLockRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long Key { get; set; }

		[ProtoMember(2)]
		public long OldInstanceId { get; set; }

		[ProtoMember(3)]
		public long InstanceId { get; set; }

	}

	[Message(InnerOpcode.ObjectUnLockResponse)]
	[ProtoContract]
	public partial class ObjectUnLockResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(ObjectRemoveResponse))]
	[Message(InnerOpcode.ObjectRemoveRequest)]
	[ProtoContract]
	public partial class ObjectRemoveRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long Key { get; set; }

	}

	[Message(InnerOpcode.ObjectRemoveResponse)]
	[ProtoContract]
	public partial class ObjectRemoveResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(ObjectGetResponse))]
	[Message(InnerOpcode.ObjectGetRequest)]
	[ProtoContract]
	public partial class ObjectGetRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long Key { get; set; }

	}

	[Message(InnerOpcode.ObjectGetResponse)]
	[ProtoContract]
	public partial class ObjectGetResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long InstanceId { get; set; }

	}

	[ResponseType(nameof(G2R_GetLoginKey))]
	[Message(InnerOpcode.R2G_GetLoginKey)]
	[ProtoContract]
	public partial class R2G_GetLoginKey: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long PlayerID { get; set; }

		[ProtoMember(2)]
		public int nGameServerID { get; set; }

	}

	[Message(InnerOpcode.G2R_GetLoginKey)]
	[ProtoContract]
	public partial class G2R_GetLoginKey: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long Key { get; set; }

	}

	[ResponseType(nameof(Game2G_PlayerLogin))]
	[Message(InnerOpcode.G2Game_PlayerLogin)]
	[ProtoContract]
	public partial class G2Game_PlayerLogin: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long PlayerID { get; set; }

		[ProtoMember(2)]
		public long SessionInstanceId { get; set; }

	}

	[Message(InnerOpcode.Game2G_PlayerLogin)]
	[ProtoContract]
	public partial class Game2G_PlayerLogin: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public long PlayerInstanceId { get; set; }

	}

	[ResponseType(nameof(Game2G_PlayerLogout))]
	[Message(InnerOpcode.G2Game_PlayerLogout)]
	[ProtoContract]
	public partial class G2Game_PlayerLogout: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long PlayerID { get; set; }

	}

	[Message(InnerOpcode.Game2G_PlayerLogout)]
	[ProtoContract]
	public partial class Game2G_PlayerLogout: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

	}

	[Message(InnerOpcode.M2M_UnitTransferResponse)]
	[ProtoContract]
	public partial class M2M_UnitTransferResponse: Object, IActorResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public long NewInstanceId { get; set; }

	}

	[Message(InnerOpcode.G2M_SessionDisconnect)]
	[ProtoContract]
	public partial class G2M_SessionDisconnect: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

//增加或者更新Unit缓存
	[Message(InnerOpcode.Other2UnitCache_AddOrUpdateUnit)]
	[ProtoContract]
	public partial class Other2UnitCache_AddOrUpdateUnit: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long playerId { get; set; }

		[ProtoMember(2)]
		public int ZoneId { get; set; }

		[ProtoMember(3)]
		public List<string> ComponentTypeList = new List<string>();

		[ProtoMember(4)]
		public List<byte[]> ComponentBytes = new List<byte[]>();

	}

//获取Unit缓存
	[ResponseType(nameof(UnitCache2Other_GetUnit))]
	[Message(InnerOpcode.Other2UnitCache_GetUnit)]
	[ProtoContract]
	public partial class Other2UnitCache_GetUnit: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long playerId { get; set; }

		[ProtoMember(2)]
		public int ZoneId { get; set; }

		[ProtoMember(3)]
		public List<string> ComponentTypeList = new List<string>();

	}

	[Message(InnerOpcode.UnitCache2Other_GetUnit)]
	[ProtoContract]
	public partial class UnitCache2Other_GetUnit: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<byte[]> ComponentBytes = new List<byte[]>();

		[ProtoMember(2)]
		public List<string> ComponentTypeList = new List<string>();

	}

//删除Unit缓存
	[ResponseType(nameof(UnitCache2Other_DeleteUnit))]
	[Message(InnerOpcode.Other2UnitCache_DeleteUnit)]
	[ProtoContract]
	public partial class Other2UnitCache_DeleteUnit: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long playerId { get; set; }

		[ProtoMember(2)]
		public int ZoneId { get; set; }

		[ProtoMember(3)]
		public string ComponentName { get; set; }

	}

	[Message(InnerOpcode.UnitCache2Other_DeleteUnit)]
	[ProtoContract]
	public partial class UnitCache2Other_DeleteUnit: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//增加或者更新class缓存
	[Message(InnerOpcode.Other2UnitCache_AddOrUpdateClass)]
	[ProtoContract]
	public partial class Other2UnitCache_AddOrUpdateClass: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long playerId { get; set; }

		[ProtoMember(2)]
		public string ClassType { get; set; }

		[ProtoMember(3)]
		public List<byte[]> ClassBytes = new List<byte[]>();

	}

//获取Unit缓存
	[ResponseType(nameof(UnitCache2Other_GetClass))]
	[Message(InnerOpcode.Other2UnitCache_GetClass)]
	[ProtoContract]
	public partial class Other2UnitCache_GetClass: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long playerId { get; set; }

		[ProtoMember(2)]
		public string classType { get; set; }

	}

	[Message(InnerOpcode.UnitCache2Other_GetClass)]
	[ProtoContract]
	public partial class UnitCache2Other_GetClass: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<byte[]> ClassBytes = new List<byte[]>();

	}

//广播单条聊天数据（世界）
	[Message(InnerOpcode.Game2Game_ChatInfoAddGlobal)]
	[ProtoContract]
	public partial class Game2Game_ChatInfoAddGlobal: Object, IActorMessage
	{
//ChatInfoNodeProto ChatInfos = 1;				//聊天数据节点
	}

//获取玩家个人信息数据
	[ResponseType(nameof(Game2Game_CenterDataPlayerInfoRes))]
	[Message(InnerOpcode.Game2Game_CenterDataPlayerInfoReq)]
	[ProtoContract]
	public partial class Game2Game_CenterDataPlayerInfoReq: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long PlayerId { get; set; }

	}

	[Message(InnerOpcode.Game2Game_CenterDataPlayerInfoRes)]
	[ProtoContract]
	public partial class Game2Game_CenterDataPlayerInfoRes: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

//CenterDataPlayerInfoProto PlayerInfo = 1;	//玩家个人信息数据
	}

//获取玩家战力对比数据
	[ResponseType(nameof(Game2Game_CenterDataPlayerCompareRes))]
	[Message(InnerOpcode.Game2Game_CenterDataPlayerCompareReq)]
	[ProtoContract]
	public partial class Game2Game_CenterDataPlayerCompareReq: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long PlayerId { get; set; }

	}

	[Message(InnerOpcode.Game2Game_CenterDataPlayerCompareRes)]
	[ProtoContract]
	public partial class Game2Game_CenterDataPlayerCompareRes: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<long> Numeric = new List<long>();

		[ProtoMember(2)]
		public List<long> OtherNumeric = new List<long>();

//repeated PlayerCompareSystemProto SystemDatas = 3;	//玩家所有系统影响的属性
	}

}
