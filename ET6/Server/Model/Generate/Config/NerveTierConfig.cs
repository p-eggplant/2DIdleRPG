using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class NerveTierConfigCategory : ProtoObject, IMerge
    {
        public static NerveTierConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, NerveTierConfig> dict = new Dictionary<int, NerveTierConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<NerveTierConfig> list = new List<NerveTierConfig>();
		
        public NerveTierConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            NerveTierConfigCategory s = o as NerveTierConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (NerveTierConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public NerveTierConfig Get(int id)
        {
            this.dict.TryGetValue(id, out NerveTierConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (NerveTierConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, NerveTierConfig> GetAll()
        {
            return this.dict;
        }

        public NerveTierConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class NerveTierConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>天赋id</summary>
		[ProtoMember(2)]
		public int NerveId { get; set; }
		/// <summary>天赋层数</summary>
		[ProtoMember(3)]
		public int NerveTier { get; set; }
		/// <summary>升层属性ID，百分数放大10000倍</summary>
		[ProtoMember(4)]
		public int TierNumericId { get; set; }
		/// <summary>升层属性值</summary>
		[ProtoMember(5)]
		public int TierNumericData { get; set; }

	}
}
