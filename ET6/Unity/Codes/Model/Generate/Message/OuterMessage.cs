using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
	[ResponseType(nameof(M2C_TestResponse))]
	[Message(OuterOpcode.C2M_TestRequest)]
	[ProtoContract]
	public partial class C2M_TestRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string request { get; set; }

	}

	[Message(OuterOpcode.M2C_TestResponse)]
	[ProtoContract]
	public partial class M2C_TestResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string response { get; set; }

	}

	[ResponseType(nameof(Actor_TransferResponse))]
	[Message(OuterOpcode.Actor_TransferRequest)]
	[ProtoContract]
	public partial class Actor_TransferRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int MapIndex { get; set; }

	}

	[Message(OuterOpcode.Actor_TransferResponse)]
	[ProtoContract]
	public partial class Actor_TransferResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(G2C_EnterMap))]
	[Message(OuterOpcode.C2G_EnterMap)]
	[ProtoContract]
	public partial class C2G_EnterMap: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.G2C_EnterMap)]
	[ProtoContract]
	public partial class G2C_EnterMap: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

// 自己unitId
		[ProtoMember(4)]
		public long MyId { get; set; }

	}

	[Message(OuterOpcode.MoveInfo)]
	[ProtoContract]
	public partial class MoveInfo: Object
	{
		[ProtoMember(1)]
		public List<float> X = new List<float>();

		[ProtoMember(2)]
		public List<float> Y = new List<float>();

		[ProtoMember(3)]
		public List<float> Z = new List<float>();

		[ProtoMember(4)]
		public float A { get; set; }

		[ProtoMember(5)]
		public float B { get; set; }

		[ProtoMember(6)]
		public float C { get; set; }

		[ProtoMember(7)]
		public float W { get; set; }

		[ProtoMember(8)]
		public int TurnSpeed { get; set; }

	}

	[Message(OuterOpcode.UnitInfo)]
	[ProtoContract]
	public partial class UnitInfo: Object
	{
		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public int ConfigId { get; set; }

		[ProtoMember(3)]
		public int Type { get; set; }

		[ProtoMember(4)]
		public float X { get; set; }

		[ProtoMember(5)]
		public float Y { get; set; }

		[ProtoMember(6)]
		public float Z { get; set; }

		[ProtoMember(7)]
		public float ForwardX { get; set; }

		[ProtoMember(8)]
		public float ForwardY { get; set; }

		[ProtoMember(9)]
		public float ForwardZ { get; set; }

		[ProtoMember(10)]
		public List<int> Ks = new List<int>();

		[ProtoMember(11)]
		public List<long> Vs = new List<long>();

		[ProtoMember(12)]
		public MoveInfo MoveInfo { get; set; }

	}

	[Message(OuterOpcode.M2C_CreateUnits)]
	[ProtoContract]
	public partial class M2C_CreateUnits: Object, IActorMessage
	{
		[ProtoMember(2)]
		public List<UnitInfo> Units = new List<UnitInfo>();

	}

	[Message(OuterOpcode.M2C_CreateMyUnit)]
	[ProtoContract]
	public partial class M2C_CreateMyUnit: Object, IActorMessage
	{
		[ProtoMember(1)]
		public UnitInfo Unit { get; set; }

	}

	[Message(OuterOpcode.M2C_StartSceneChange)]
	[ProtoContract]
	public partial class M2C_StartSceneChange: Object, IActorMessage
	{
		[ProtoMember(1)]
		public long SceneInstanceId { get; set; }

		[ProtoMember(2)]
		public string SceneName { get; set; }

	}

	[Message(OuterOpcode.M2C_RemoveUnits)]
	[ProtoContract]
	public partial class M2C_RemoveUnits: Object, IActorMessage
	{
		[ProtoMember(2)]
		public List<long> Units = new List<long>();

	}

	[Message(OuterOpcode.C2M_PathfindingResult)]
	[ProtoContract]
	public partial class C2M_PathfindingResult: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public float X { get; set; }

		[ProtoMember(2)]
		public float Y { get; set; }

		[ProtoMember(3)]
		public float Z { get; set; }

	}

	[Message(OuterOpcode.C2M_Stop)]
	[ProtoContract]
	public partial class C2M_Stop: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_PathfindingResult)]
	[ProtoContract]
	public partial class M2C_PathfindingResult: Object, IActorMessage
	{
		[ProtoMember(1)]
		public long Id { get; set; }

		[ProtoMember(2)]
		public float X { get; set; }

		[ProtoMember(3)]
		public float Y { get; set; }

		[ProtoMember(4)]
		public float Z { get; set; }

		[ProtoMember(5)]
		public List<float> Xs = new List<float>();

		[ProtoMember(6)]
		public List<float> Ys = new List<float>();

		[ProtoMember(7)]
		public List<float> Zs = new List<float>();

	}

	[Message(OuterOpcode.M2C_Stop)]
	[ProtoContract]
	public partial class M2C_Stop: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int Error { get; set; }

		[ProtoMember(2)]
		public long Id { get; set; }

		[ProtoMember(3)]
		public float X { get; set; }

		[ProtoMember(4)]
		public float Y { get; set; }

		[ProtoMember(5)]
		public float Z { get; set; }

		[ProtoMember(6)]
		public float A { get; set; }

		[ProtoMember(7)]
		public float B { get; set; }

		[ProtoMember(8)]
		public float C { get; set; }

		[ProtoMember(9)]
		public float W { get; set; }

	}

	[ResponseType(nameof(G2C_Ping))]
	[Message(OuterOpcode.C2G_Ping)]
	[ProtoContract]
	public partial class C2G_Ping: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.G2C_Ping)]
	[ProtoContract]
	public partial class G2C_Ping: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long Time { get; set; }

	}

	[Message(OuterOpcode.G2C_Test)]
	[ProtoContract]
	public partial class G2C_Test: Object, IMessage
	{
	}

	[ResponseType(nameof(M2C_Reload))]
	[Message(OuterOpcode.C2M_Reload)]
	[ProtoContract]
	public partial class C2M_Reload: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

	}

	[Message(OuterOpcode.M2C_Reload)]
	[ProtoContract]
	public partial class M2C_Reload: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.G2C_TestHotfixMessage)]
	[ProtoContract]
	public partial class G2C_TestHotfixMessage: Object, IMessage
	{
		[ProtoMember(1)]
		public string Info { get; set; }

	}

	[ResponseType(nameof(M2C_TestRobotCase))]
	[Message(OuterOpcode.C2M_TestRobotCase)]
	[ProtoContract]
	public partial class C2M_TestRobotCase: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int N { get; set; }

	}

	[Message(OuterOpcode.M2C_TestRobotCase)]
	[ProtoContract]
	public partial class M2C_TestRobotCase: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int N { get; set; }

	}

	[ResponseType(nameof(M2C_TransferMap))]
	[Message(OuterOpcode.C2M_TransferMap)]
	[ProtoContract]
	public partial class C2M_TransferMap: Object, IActorLocationRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_TransferMap)]
	[ProtoContract]
	public partial class M2C_TransferMap: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(Game2C_GmDataChange))]
	[Message(OuterOpcode.C2Game_GmDataChange)]
	[ProtoContract]
	public partial class C2Game_GmDataChange: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public List<PlayerDataProto> DataNodes = new List<PlayerDataProto>();

	}

	[Message(OuterOpcode.Game2C_GmDataChange)]
	[ProtoContract]
	public partial class Game2C_GmDataChange: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(Game2C_GmBagAdd))]
	[Message(OuterOpcode.C2Game_GmBagAdd)]
	[ProtoContract]
	public partial class C2Game_GmBagAdd: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int ConfigId { get; set; }

		[ProtoMember(2)]
		public int Num { get; set; }

	}

	[Message(OuterOpcode.Game2C_GmBagAdd)]
	[ProtoContract]
	public partial class Game2C_GmBagAdd: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

////////////////////////////////////GM属性///////////////////////////////
//玩家总属性消息体
	[Message(OuterOpcode.PlayerPropGmProto)]
	[ProtoContract]
	public partial class PlayerPropGmProto: Object
	{
		[ProtoMember(1)]
		public int nPropEnumId { get; set; }

		[ProtoMember(2)]
		public double lPropNum { get; set; }

	}

	[ResponseType(nameof(Game2C_GmTotalProp))]
	[Message(OuterOpcode.C2Game_GmTotalProp)]
	[ProtoContract]
	public partial class C2Game_GmTotalProp: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.Game2C_GmTotalProp)]
	[ProtoContract]
	public partial class Game2C_GmTotalProp: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<PlayerPropGmProto> pListTotalProp = new List<PlayerPropGmProto>();

	}

	[ResponseType(nameof(Game2C_GmSystemProp))]
	[Message(OuterOpcode.C2Game_GmSystemProp)]
	[ProtoContract]
	public partial class C2Game_GmSystemProp: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int nSystemEnumId { get; set; }

	}

	[Message(OuterOpcode.Game2C_GmSystemProp)]
	[ProtoContract]
	public partial class Game2C_GmSystemProp: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<PlayerPropGmProto> pListSystemProp = new List<PlayerPropGmProto>();

	}

////////////////////////////////////GM日志///////////////////////////////
//玩家数据日志消息体
	[Message(OuterOpcode.PlayerDataOss)]
	[ProtoContract]
	public partial class PlayerDataOss: Object
	{
		[ProtoMember(1)]
		public long lTime { get; set; }

		[ProtoMember(2)]
		public int nConfigId { get; set; }

		[ProtoMember(3)]
		public long lChangeNum { get; set; }

		[ProtoMember(4)]
		public long lRealNum { get; set; }

		[ProtoMember(5)]
		public string szReason { get; set; }

	}

	[ResponseType(nameof(Game2C_GmOssData))]
	[Message(OuterOpcode.C2Game_GmOssData)]
	[ProtoContract]
	public partial class C2Game_GmOssData: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int pageNum { get; set; }

	}

	[Message(OuterOpcode.Game2C_GmOssData)]
	[ProtoContract]
	public partial class Game2C_GmOssData: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int nTotalPage { get; set; }

		[ProtoMember(2)]
		public List<PlayerDataOss> pListDataOss = new List<PlayerDataOss>();

	}

//玩家数据日志消息体
	[Message(OuterOpcode.PlayerBagOss)]
	[ProtoContract]
	public partial class PlayerBagOss: Object
	{
		[ProtoMember(1)]
		public long lTime { get; set; }

		[ProtoMember(2)]
		public int nConfigId { get; set; }

		[ProtoMember(3)]
		public int lChangeNum { get; set; }

		[ProtoMember(4)]
		public int lRealNum { get; set; }

		[ProtoMember(5)]
		public string szReason { get; set; }

	}

	[ResponseType(nameof(Game2C_GmOssBag))]
	[Message(OuterOpcode.C2Game_GmOssBag)]
	[ProtoContract]
	public partial class C2Game_GmOssBag: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int pageNum { get; set; }

	}

	[Message(OuterOpcode.Game2C_GmOssBag)]
	[ProtoContract]
	public partial class Game2C_GmOssBag: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int nTotalPage { get; set; }

		[ProtoMember(2)]
		public List<PlayerBagOss> pListBagOss = new List<PlayerBagOss>();

	}

//玩家属性日志消息体
	[Message(OuterOpcode.PlayerPropOss)]
	[ProtoContract]
	public partial class PlayerPropOss: Object
	{
		[ProtoMember(1)]
		public int nPropEnumId { get; set; }

		[ProtoMember(2)]
		public int nOssId { get; set; }

		[ProtoMember(3)]
		public int nSystemId { get; set; }

		[ProtoMember(4)]
		public double dwChangeNum { get; set; }

		[ProtoMember(5)]
		public double dwRealNum { get; set; }

		[ProtoMember(6)]
		public string szReason { get; set; }

	}

	[ResponseType(nameof(Game2C_GmOssProp))]
	[Message(OuterOpcode.C2Game_GmOssProp)]
	[ProtoContract]
	public partial class C2Game_GmOssProp: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int pageNum { get; set; }

	}

	[Message(OuterOpcode.Game2C_GmOssProp)]
	[ProtoContract]
	public partial class Game2C_GmOssProp: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int nTotalPage { get; set; }

		[ProtoMember(2)]
		public List<PlayerPropOss> pListPropOss = new List<PlayerPropOss>();

	}

	[ResponseType(nameof(R2C_LoginAccount))]
	[Message(OuterOpcode.C2R_LoginAccount)]
	[ProtoContract]
	public partial class C2R_LoginAccount: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

	}

	[Message(OuterOpcode.R2C_LoginAccount)]
	[ProtoContract]
	public partial class R2C_LoginAccount: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long AccountID { get; set; }

		[ProtoMember(2)]
		public int DefaultGameServerId { get; set; }

		[ProtoMember(3)]
		public string DefaultGameServerName { get; set; }

	}

	[ResponseType(nameof(R2C_LoginServerPlayerList))]
	[Message(OuterOpcode.C2R_LoginServerPlayerList)]
	[ProtoContract]
	public partial class C2R_LoginServerPlayerList: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long AccountID { get; set; }

	}

	[Message(OuterOpcode.R2C_LoginServerPlayerList)]
	[ProtoContract]
	public partial class R2C_LoginServerPlayerList: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<LoginPlayerInfoProto> HavePlayerList = new List<LoginPlayerInfoProto>();

		[ProtoMember(2)]
		public List<LoginServerInfoProto> ServerList = new List<LoginServerInfoProto>();

	}

//玩家角色列表数据
	[Message(OuterOpcode.LoginPlayerInfoProto)]
	[ProtoContract]
	public partial class LoginPlayerInfoProto: Object
	{
		[ProtoMember(1)]
		public int nGameServerID { get; set; }

		[ProtoMember(2)]
		public string szGameServerName { get; set; }

		[ProtoMember(3)]
		public string szName { get; set; }

		[ProtoMember(4)]
		public long lFightingCapacity { get; set; }

		[ProtoMember(5)]
		public int nHeadIcon { get; set; }

	}

// 服务器列表
	[Message(OuterOpcode.LoginServerInfoProto)]
	[ProtoContract]
	public partial class LoginServerInfoProto: Object
	{
		[ProtoMember(1)]
		public int nGameServerID { get; set; }

		[ProtoMember(2)]
		public string szGameServerName { get; set; }

		[ProtoMember(3)]
		public int nHeadIcon { get; set; }

	}

	[ResponseType(nameof(R2C_GetGateAddress))]
	[Message(OuterOpcode.C2R_GetGateAddress)]
	[ProtoContract]
	public partial class C2R_GetGateAddress: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long AccountID { get; set; }

		[ProtoMember(2)]
		public int GameServerId { get; set; }

	}

	[Message(OuterOpcode.R2C_GetGateAddress)]
	[ProtoContract]
	public partial class R2C_GetGateAddress: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long Key { get; set; }

		[ProtoMember(3)]
		public string Address { get; set; }

	}

	[ResponseType(nameof(G2C_LoginGate))]
	[Message(OuterOpcode.C2G_LoginGate)]
	[ProtoContract]
	public partial class C2G_LoginGate: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long Key { get; set; }

	}

	[Message(OuterOpcode.G2C_LoginGate)]
	[ProtoContract]
	public partial class G2C_LoginGate: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(G2C_LogoutGate))]
	[Message(OuterOpcode.C2G_LogoutGate)]
	[ProtoContract]
	public partial class C2G_LogoutGate: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.G2C_LogoutGate)]
	[ProtoContract]
	public partial class G2C_LogoutGate: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//--------------天赋组件--------------
//天赋组件数据消息体
	[Message(OuterOpcode.NerveDataProto)]
	[ProtoContract]
	public partial class NerveDataProto: Object
	{
		[ProtoMember(1)]
		public List<NerveNode> listLevelData = new List<NerveNode>();

		[ProtoMember(2)]
		public List<NerveTierNode> listReachTierData = new List<NerveTierNode>();

	}

//天赋数据节点
	[Message(OuterOpcode.NerveNode)]
	[ProtoContract]
	public partial class NerveNode: Object
	{
		[ProtoMember(1)]
		public int Id { get; set; }

		[ProtoMember(2)]
		public short Level { get; set; }

	}

//天赋层级数据节点
	[Message(OuterOpcode.NerveTierNode)]
	[ProtoContract]
	public partial class NerveTierNode: Object
	{
		[ProtoMember(1)]
		public int Id { get; set; }

		[ProtoMember(2)]
		public short Tier { get; set; }

	}

//解锁
	[ResponseType(nameof(Game2C_NerveUnlock))]
	[Message(OuterOpcode.C2Game_NerveUnlock)]
	[ProtoContract]
	public partial class C2Game_NerveUnlock: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int NerveId { get; set; }

	}

	[Message(OuterOpcode.Game2C_NerveUnlock)]
	[ProtoContract]
	public partial class Game2C_NerveUnlock: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int NerveId { get; set; }

		[ProtoMember(2)]
		public short NewTier { get; set; }

		[ProtoMember(3)]
		public short NewLevel { get; set; }

	}

//升级
	[ResponseType(nameof(Game2C_NerveLevelUp))]
	[Message(OuterOpcode.C2Game_NerveLevelUp)]
	[ProtoContract]
	public partial class C2Game_NerveLevelUp: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int NerveId { get; set; }

	}

	[Message(OuterOpcode.Game2C_NerveLevelUp)]
	[ProtoContract]
	public partial class Game2C_NerveLevelUp: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int NerveId { get; set; }

		[ProtoMember(2)]
		public short NewTier { get; set; }

		[ProtoMember(3)]
		public short NewLevel { get; set; }

	}

//突破层
	[ResponseType(nameof(Game2C_NerveTierUp))]
	[Message(OuterOpcode.C2Game_NerveTierUp)]
	[ProtoContract]
	public partial class C2Game_NerveTierUp: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int NerveId { get; set; }

	}

	[Message(OuterOpcode.Game2C_NerveTierUp)]
	[ProtoContract]
	public partial class Game2C_NerveTierUp: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int NerveId { get; set; }

		[ProtoMember(2)]
		public short NewTier { get; set; }

		[ProtoMember(3)]
		public short NewLevel { get; set; }

	}

//背包消息体
	[Message(OuterOpcode.PlayerBagProto)]
	[ProtoContract]
	public partial class PlayerBagProto: Object
	{
		[ProtoMember(1)]
		public int ConfigId { get; set; }

		[ProtoMember(2)]
		public int Num { get; set; }

	}

//添加/移除物资
	[Message(OuterOpcode.Game2C_BagChange)]
	[ProtoContract]
	public partial class Game2C_BagChange: Object, IActorMessage
	{
		[ProtoMember(1)]
		public List<PlayerBagProto> BagNodes = new List<PlayerBagProto>();

	}

//添加/移除物资
	[Message(OuterOpcode.Game2C_BagChangeDebug)]
	[ProtoContract]
	public partial class Game2C_BagChangeDebug: Object, IActorMessage
	{
		[ProtoMember(1)]
		public List<PlayerBagProto> BagNodes = new List<PlayerBagProto>();

		[ProtoMember(2)]
		public int Reason { get; set; }

	}

//售卖物资
	[ResponseType(nameof(Game2C_BagSell))]
	[Message(OuterOpcode.C2Game_BagSell)]
	[ProtoContract]
	public partial class C2Game_BagSell: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int nMaterialId { get; set; }

	}

	[Message(OuterOpcode.Game2C_BagSell)]
	[ProtoContract]
	public partial class Game2C_BagSell: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//玩家数据消息体
	[Message(OuterOpcode.PlayerDataProto)]
	[ProtoContract]
	public partial class PlayerDataProto: Object
	{
		[ProtoMember(1)]
		public int EnumId { get; set; }

		[ProtoMember(2)]
		public long Value { get; set; }

	}

//改变玩家属性值
	[Message(OuterOpcode.Game2C_PlayerDataChange)]
	[ProtoContract]
	public partial class Game2C_PlayerDataChange: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int EnumId { get; set; }

		[ProtoMember(2)]
		public long Value { get; set; }

	}

//改变玩家属性值
	[Message(OuterOpcode.Game2C_PlayerDataChangeDebug)]
	[ProtoContract]
	public partial class Game2C_PlayerDataChangeDebug: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int EnumId { get; set; }

		[ProtoMember(2)]
		public long OldValue { get; set; }

		[ProtoMember(3)]
		public long NewValue { get; set; }

		[ProtoMember(4)]
		public int Reason { get; set; }

	}

//--------------技能组件--------------
//技能总消息体
	[Message(OuterOpcode.PlayerLoginInfo)]
	[ProtoContract]
	public partial class PlayerLoginInfo: Object, IActorMessage
	{
		[ProtoMember(1)]
		public long PlayerID { get; set; }

// PlayerDataComponent
		[ProtoMember(2)]
		public List<PlayerDataProto> PlayerDataComponent = new List<PlayerDataProto>();

// PlayerBagComponent
		[ProtoMember(3)]
		public List<PlayerBagProto> PlayerBagComponent = new List<PlayerBagProto>();

// PlayerROleUpComponent
		[ProtoMember(4)]
		public PlayerRoleUpProto PlayerRoleUpComponent { get; set; }

	}

//玩家等级消息体
	[Message(OuterOpcode.PlayerRoleUpProto)]
	[ProtoContract]
	public partial class PlayerRoleUpProto: Object
	{
		[ProtoMember(1)]
		public int CurLevel { get; set; }

	}

//升级请求
	[ResponseType(nameof(Game2C_PlayerRoleUp))]
	[Message(OuterOpcode.C2Game_PlayerRoleUp)]
	[ProtoContract]
	public partial class C2Game_PlayerRoleUp: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

//升级回复
	[Message(OuterOpcode.Game2C_PlayerRoleUp)]
	[ProtoContract]
	public partial class Game2C_PlayerRoleUp: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int CurLevel { get; set; }

	}

//--------------技能组件--------------
//技能总消息体
	[Message(OuterOpcode.SkillsDataProto)]
	[ProtoContract]
	public partial class SkillsDataProto: Object
	{
		[ProtoMember(1)]
		public List<SkillsProto> listSkillsData = new List<SkillsProto>();

		[ProtoMember(2)]
		public List<SkillsSubNodeProto> listSubSkillsData = new List<SkillsSubNodeProto>();

		[ProtoMember(3)]
		public SkillsEquipProto EquipSkills { get; set; }

	}

//技能数据消息体
	[Message(OuterOpcode.SkillsProto)]
	[ProtoContract]
	public partial class SkillsProto: Object
	{
		[ProtoMember(1)]
		public int SkillsId { get; set; }

		[ProtoMember(2)]
		public int Level { get; set; }

		[ProtoMember(3)]
		public List<int> Prama = new List<int>();

	}

//技能节点数据消息体
	[Message(OuterOpcode.SkillsSubNodeProto)]
	[ProtoContract]
	public partial class SkillsSubNodeProto: Object
	{
		[ProtoMember(1)]
		public int SubNodeId { get; set; }

		[ProtoMember(2)]
		public int Level { get; set; }

	}

//技能组件数据消息体
	[Message(OuterOpcode.SkillsEquipProto)]
	[ProtoContract]
	public partial class SkillsEquipProto: Object
	{
		[ProtoMember(1)]
		public List<int> EquipSkillsIds = new List<int>();

//repeated SkillsNodeProto SkillsNodes = 2;	//已有技能列表
// int32 UnlockNum = 3;						//允许装备技能槽位数
	}

//请求领悟技能
	[ResponseType(nameof(Game2C_SkillsLearn))]
	[Message(OuterOpcode.C2Game_SkillsLearn)]
	[ProtoContract]
	public partial class C2Game_SkillsLearn: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int SkillsId { get; set; }

	}

	[Message(OuterOpcode.Game2C_SkillsLearn)]
	[ProtoContract]
	public partial class Game2C_SkillsLearn: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int SkillsId { get; set; }

		[ProtoMember(2)]
		public int Level { get; set; }

		[ProtoMember(3)]
		public List<int> Prama = new List<int>();

	}

//请求研习（升级）技能
	[ResponseType(nameof(Game2C_SkillsLevelUp))]
	[Message(OuterOpcode.C2Game_SkillsLevelUp)]
	[ProtoContract]
	public partial class C2Game_SkillsLevelUp: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int SkillsId { get; set; }

	}

	[Message(OuterOpcode.Game2C_SkillsLevelUp)]
	[ProtoContract]
	public partial class Game2C_SkillsLevelUp: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int SkillsId { get; set; }

		[ProtoMember(2)]
		public int Level { get; set; }

		[ProtoMember(3)]
		public List<int> Prama = new List<int>();

	}

//请求装备技能
	[ResponseType(nameof(Game2C_SkillsEquip))]
	[Message(OuterOpcode.C2Game_SkillsEquip)]
	[ProtoContract]
	public partial class C2Game_SkillsEquip: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int SkillsId { get; set; }

	}

	[Message(OuterOpcode.Game2C_SkillsEquip)]
	[ProtoContract]
	public partial class Game2C_SkillsEquip: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int SkillsId { get; set; }

	}

//请求卸下技能
	[ResponseType(nameof(Game2C_SkillsUnequip))]
	[Message(OuterOpcode.C2Game_SkillsUnequip)]
	[ProtoContract]
	public partial class C2Game_SkillsUnequip: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int SkillsId { get; set; }

	}

	[Message(OuterOpcode.Game2C_SkillsUnequip)]
	[ProtoContract]
	public partial class Game2C_SkillsUnequip: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int SkillsId { get; set; }

	}

//--------------技能节点--------------
//请求激活技能节点
	[ResponseType(nameof(Game2C_SkillsSubActive))]
	[Message(OuterOpcode.C2Game_SkillsSubActive)]
	[ProtoContract]
	public partial class C2Game_SkillsSubActive: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int SubNodeId { get; set; }

	}

	[Message(OuterOpcode.Game2C_SkillsSubActive)]
	[ProtoContract]
	public partial class Game2C_SkillsSubActive: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int SubNodeId { get; set; }

		[ProtoMember(2)]
		public int SubLevel { get; set; }

	}

//请求升级技能节点
	[ResponseType(nameof(Game2C_SkillsSubLevelUp))]
	[Message(OuterOpcode.C2Game_SkillsSubLevelUp)]
	[ProtoContract]
	public partial class C2Game_SkillsSubLevelUp: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int SubNodeId { get; set; }

	}

	[Message(OuterOpcode.Game2C_SkillsSubLevelUp)]
	[ProtoContract]
	public partial class Game2C_SkillsSubLevelUp: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int SubNodeId { get; set; }

		[ProtoMember(2)]
		public int SubLevel { get; set; }

		[ProtoMember(3)]
		public List<int> Prama = new List<int>();

	}

}
