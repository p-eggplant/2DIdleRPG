using System.Collections.Generic;

namespace ET
{
	public class SessionKeyNode
	{
		public long PlayerID;
		public int ServerGameID;
	}


	[ComponentOf(typeof(Scene))]
	public class GateSessionKeyComponent : Entity, IAwake
	{
		public readonly Dictionary<long, SessionKeyNode> sessionKey = new Dictionary<long, SessionKeyNode>();
	}
}
