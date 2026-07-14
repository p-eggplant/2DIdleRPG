using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class NerveMouldConfigCategory : ProtoObject, IMerge
    {
        public static NerveMouldConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, NerveMouldConfig> dict = new Dictionary<int, NerveMouldConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<NerveMouldConfig> list = new List<NerveMouldConfig>();
		
        public NerveMouldConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            NerveMouldConfigCategory s = o as NerveMouldConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (NerveMouldConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public NerveMouldConfig Get(int id)
        {
            this.dict.TryGetValue(id, out NerveMouldConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (NerveMouldConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, NerveMouldConfig> GetAll()
        {
            return this.dict;
        }

        public NerveMouldConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class NerveMouldConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>模板ID</summary>
		[ProtoMember(2)]
		public int NerveMould { get; set; }
		/// <summary>神经层数</summary>
		[ProtoMember(3)]
		public int NerveTier { get; set; }
		/// <summary>升级、加成路线预制名</summary>
		[ProtoMember(4)]
		public string NervePrefab { get; set; }
		/// <summary>最大血量</summary>
		[ProtoMember(5)]
		public int Numeric1 { get; set; }
		/// <summary>暴击值</summary>
		[ProtoMember(6)]
		public int Numeric2 { get; set; }
		/// <summary>暴击抗性值</summary>
		[ProtoMember(7)]
		public int Numeric3 { get; set; }
		/// <summary>命中值</summary>
		[ProtoMember(8)]
		public int Numeric4 { get; set; }
		/// <summary>闪避值</summary>
		[ProtoMember(9)]
		public int Numeric5 { get; set; }
		/// <summary>防御值</summary>
		[ProtoMember(10)]
		public int Numeric6 { get; set; }
		/// <summary>暴击值</summary>
		[ProtoMember(11)]
		public int Numeric7 { get; set; }
		/// <summary>暴击抗性值</summary>
		[ProtoMember(12)]
		public int Numeric8 { get; set; }
		/// <summary>命中值</summary>
		[ProtoMember(13)]
		public int Numeric9 { get; set; }
		/// <summary>闪避值</summary>
		[ProtoMember(14)]
		public int Numeric10 { get; set; }
		/// <summary>升级材料Id</summary>
		[ProtoMember(15)]
		public int MaterialUpId { get; set; }
		/// <summary>基础消耗（实际消耗=（等级/2）向上取整*基础消耗）</summary>
		[ProtoMember(16)]
		public int BaseNumber { get; set; }
		/// <summary>突破节点攻击值</summary>
		[ProtoMember(17)]
		public int BreakAtk { get; set; }
		/// <summary>升层材料</summary>
		[ProtoMember(18)]
		public int MateirialBreakId { get; set; }
		/// <summary>数量</summary>
		[ProtoMember(19)]
		public int BreakNumber { get; set; }

	}
}
