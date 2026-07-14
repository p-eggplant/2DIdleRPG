using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class NerveConfigCategory : ProtoObject, IMerge
    {
        public static NerveConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, NerveConfig> dict = new Dictionary<int, NerveConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<NerveConfig> list = new List<NerveConfig>();
		
        public NerveConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            NerveConfigCategory s = o as NerveConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (NerveConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public NerveConfig Get(int id)
        {
            this.dict.TryGetValue(id, out NerveConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (NerveConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, NerveConfig> GetAll()
        {
            return this.dict;
        }

        public NerveConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class NerveConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>神经名</summary>
		[ProtoMember(2)]
		public string NerveName { get; set; }
		/// <summary>神经显示类型（1_普通神经，2_限定神经）</summary>
		[ProtoMember(3)]
		public int NerveType { get; set; }
		/// <summary>神经图片预制</summary>
		[ProtoMember(4)]
		public string NerveEffectTypePrefab { get; set; }
		/// <summary>神经线路A</summary>
		[ProtoMember(5)]
		public string NervePathPrefabA { get; set; }
		/// <summary>神经线路B</summary>
		[ProtoMember(6)]
		public string NervePathPrefabB { get; set; }
		/// <summary>阶位(特殊功法用0表示)</summary>
		[ProtoMember(7)]
		public int NerveRank { get; set; }
		/// <summary>神经描述</summary>
		[ProtoMember(8)]
		public string NerveDesc { get; set; }
		/// <summary>模板ID</summary>
		[ProtoMember(9)]
		public int NerveMould { get; set; }
		/// <summary>圆满倍率（乘法）</summary>
		[ProtoMember(10)]
		public int Mutiply { get; set; }
		/// <summary>圆满描述</summary>
		[ProtoMember(11)]
		public string EffectText { get; set; }
		/// <summary>解锁消耗材料ID</summary>
		[ProtoMember(12)]
		public int NerveUnlockId { get; set; }
		/// <summary>解锁材料数量</summary>
		[ProtoMember(13)]
		public int NerveUnlockNum { get; set; }
		/// <summary>通用解锁条件Id（Id+Id）</summary>
		[ProtoMember(14)]
		public string Condition { get; set; }
		/// <summary>显示时期Id（不填写，默认显示）</summary>
		[ProtoMember(15)]
		public int ShowRank { get; set; }

	}
}
