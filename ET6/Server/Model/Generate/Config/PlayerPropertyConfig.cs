using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class PlayerPropertyConfigCategory : ProtoObject, IMerge
    {
        public static PlayerPropertyConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, PlayerPropertyConfig> dict = new Dictionary<int, PlayerPropertyConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<PlayerPropertyConfig> list = new List<PlayerPropertyConfig>();
		
        public PlayerPropertyConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            PlayerPropertyConfigCategory s = o as PlayerPropertyConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (PlayerPropertyConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public PlayerPropertyConfig Get(int id)
        {
            this.dict.TryGetValue(id, out PlayerPropertyConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (PlayerPropertyConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, PlayerPropertyConfig> GetAll()
        {
            return this.dict;
        }

        public PlayerPropertyConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class PlayerPropertyConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>属性枚举名</summary>
		[ProtoMember(2)]
		public string PropType { get; set; }
		/// <summary>初始值</summary>
		[ProtoMember(6)]
		public int InitPropNum { get; set; }

	}
}
