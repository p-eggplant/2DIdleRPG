using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class GameServerInfoConfigCategory : ProtoObject, IMerge
    {
        public static GameServerInfoConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, GameServerInfoConfig> dict = new Dictionary<int, GameServerInfoConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<GameServerInfoConfig> list = new List<GameServerInfoConfig>();
		
        public GameServerInfoConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            GameServerInfoConfigCategory s = o as GameServerInfoConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (GameServerInfoConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public GameServerInfoConfig Get(int id)
        {
            this.dict.TryGetValue(id, out GameServerInfoConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (GameServerInfoConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, GameServerInfoConfig> GetAll()
        {
            return this.dict;
        }

        public GameServerInfoConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class GameServerInfoConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>服务器名字</summary>
		[ProtoMember(2)]
		public string ServerName { get; set; }
		/// <summary>服务器人数上限</summary>
		[ProtoMember(3)]
		public int PeopleMax { get; set; }
		/// <summary>所属区</summary>
		[ProtoMember(4)]
		public int Zoneid { get; set; }
		/// <summary>是否推荐</summary>
		[ProtoMember(5)]
		public int isDefault { get; set; }

	}
}
