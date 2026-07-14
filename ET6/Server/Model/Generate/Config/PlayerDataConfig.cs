using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class PlayerDataConfigCategory : ProtoObject, IMerge
    {
        public static PlayerDataConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, PlayerDataConfig> dict = new Dictionary<int, PlayerDataConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<PlayerDataConfig> list = new List<PlayerDataConfig>();
		
        public PlayerDataConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            PlayerDataConfigCategory s = o as PlayerDataConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (PlayerDataConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public PlayerDataConfig Get(int id)
        {
            this.dict.TryGetValue(id, out PlayerDataConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (PlayerDataConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, PlayerDataConfig> GetAll()
        {
            return this.dict;
        }

        public PlayerDataConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class PlayerDataConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>属性枚举名</summary>
		[ProtoMember(2)]
		public string DataTypeString { get; set; }
		/// <summary>第一次创建玩家的数值</summary>
		[ProtoMember(4)]
		public int FirstCreateValue { get; set; }

	}
}
