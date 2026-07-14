using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class RoleLevelUpConfigCategory : ProtoObject, IMerge
    {
        public static RoleLevelUpConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, RoleLevelUpConfig> dict = new Dictionary<int, RoleLevelUpConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<RoleLevelUpConfig> list = new List<RoleLevelUpConfig>();
		
        public RoleLevelUpConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            RoleLevelUpConfigCategory s = o as RoleLevelUpConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (RoleLevelUpConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public RoleLevelUpConfig Get(int id)
        {
            this.dict.TryGetValue(id, out RoleLevelUpConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (RoleLevelUpConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, RoleLevelUpConfig> GetAll()
        {
            return this.dict;
        }

        public RoleLevelUpConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class RoleLevelUpConfig: ProtoObject, IConfig
	{
		/// <summary>id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>属性id1</summary>
		[ProtoMember(2)]
		public int PropTypeId1 { get; set; }
		/// <summary>属性值1</summary>
		[ProtoMember(3)]
		public int PropTypeData1 { get; set; }
		/// <summary>属性id2</summary>
		[ProtoMember(4)]
		public int PropTypeId2 { get; set; }
		/// <summary>属性值2</summary>
		[ProtoMember(5)]
		public int PropTypeData2 { get; set; }
		/// <summary>物资id</summary>
		[ProtoMember(6)]
		public int MaterialId { get; set; }
		/// <summary>物资数</summary>
		[ProtoMember(7)]
		public int MaterialNumb { get; set; }
		/// <summary>货币id</summary>
		[ProtoMember(8)]
		public int CurrencyId { get; set; }
		/// <summary>货币数</summary>
		[ProtoMember(9)]
		public int CurrencyNumb { get; set; }

	}
}
