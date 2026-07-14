namespace ET
{
    [FriendClass(typeof(GateSessionKeyComponent))]
    public static class GateSessionKeyComponentSystem
    {
        public static void Add(this GateSessionKeyComponent self, long key, long lPlayerID, int nServerGameID)
        {
            SessionKeyNode node = new SessionKeyNode();
            node.PlayerID = lPlayerID;
            node.ServerGameID= nServerGameID;

            self.sessionKey.Add(key, node);
            self.TimeoutRemoveKey(key).Coroutine();
        }

        public static SessionKeyNode Get(this GateSessionKeyComponent self, long key)
        {
            SessionKeyNode node = null;
            self.sessionKey.TryGetValue(key, out node);
            return node;
        }

        public static void Remove(this GateSessionKeyComponent self, long key)
        {
            self.sessionKey.Remove(key);
        }

        private static async ETTask TimeoutRemoveKey(this GateSessionKeyComponent self, long key)
        {
            await TimerComponent.Instance.WaitAsync(20000);
            self.sessionKey.Remove(key);
        }
    }
}