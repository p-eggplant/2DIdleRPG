namespace ET
{
    [FriendClass(typeof(EPlayer))]
    public static class EPlayerSystem
    {
        [ObjectSystem]
        public class PlayerAwakeSystem : AwakeSystem<EPlayer, string>
        {
            public override void Awake(EPlayer self, string a)
            {
                self.Account = a;
            }
        }
    }
}