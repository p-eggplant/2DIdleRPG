using System.Collections.Generic;

namespace ET
{
	[ComponentOf(typeof(Scene))]
	[ChildType(typeof(EPlayer))]
	public class PlayerComponent : Entity, IAwake, IDestroy
	{
		public readonly Dictionary<long, EPlayer> idPlayers = new Dictionary<long, EPlayer>();
	}
}