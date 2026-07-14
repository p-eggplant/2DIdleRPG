namespace ET
{


	public sealed class EPlayer : Entity, IAwake<string>
	{
		public string Account { get; set; }
		
		public long UnitId { get; set; }
	}
}