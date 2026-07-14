using System.Linq;

namespace ET
{
    [FriendClass(typeof(PlayerComponent))]
    public static class PlayerComponentSystem
    {
        public class AwakeSystem : AwakeSystem<PlayerComponent>
        {
            public override void Awake(PlayerComponent self)
            {
            }
        }

        [ObjectSystem]
        public class PlayerComponentDestroySystem: DestroySystem<PlayerComponent>
        {
            public override void Destroy(PlayerComponent self)
            {
            }
        }
        
        public static void Add(this PlayerComponent self, EPlayer player)
        {
            self.idPlayers.Add(player.Id, player);
        }

        public static EPlayer Get(this PlayerComponent self,long id)
        {
            self.idPlayers.TryGetValue(id, out EPlayer gamer);
            return gamer;
        }

        public static void Remove(this PlayerComponent self,long id)
        {
            self.idPlayers.Remove(id);
        }

        public static EPlayer[] GetAll(this PlayerComponent self)
        {
            return self.idPlayers.Values.ToArray();
        }
    }
}