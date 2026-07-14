using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class MaterialConfigCategory : ProtoObject, IMerge
    {
        public static MaterialConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, MaterialConfig> dict = new Dictionary<int, MaterialConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<MaterialConfig> list = new List<MaterialConfig>();
		
        public MaterialConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            MaterialConfigCategory s = o as MaterialConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (MaterialConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public MaterialConfig Get(int id)
        {
            this.dict.TryGetValue(id, out MaterialConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (MaterialConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, MaterialConfig> GetAll()
        {
            return this.dict;
        }

        public MaterialConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class MaterialConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>物资名</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>是否在背包中隐藏(1_隐藏)</summary>
		[ProtoMember(3)]
		public int Hide { get; set; }
		/// <summary>初始数值</summary>
		[ProtoMember(4)]
		public int FirstValue { get; set; }
		/// <summary>物资类型1_特殊、2_丹药、3_材料、</summary>
		[ProtoMember(5)]
		public int MaterialType { get; set; }
		/// <summary>境界(0_无境界、1_一阶、2_二阶、3_三阶、4_四阶、5_五阶)</summary>
		[ProtoMember(6)]
		public int Stage { get; set; }
		/// <summary>品质 (1白、2绿、3蓝、4紫、5橙)</summary>
		[ProtoMember(7)]
		public int Quality { get; set; }
		/// <summary>品质底框</summary>
		[ProtoMember(8)]
		public string QualityIcon { get; set; }
		/// <summary>图标</summary>
		[ProtoMember(9)]
		public string Icon { get; set; }
		/// <summary>用途描述</summary>
		[ProtoMember(10)]
		public string Use { get; set; }
		/// <summary>背包使用调用函数（'|'分隔，函数名=参数1+参数2+……）</summary>
		[ProtoMember(11)]
		public string CallingFunction { get; set; }
		/// <summary>卖出单个价格（灵石）</summary>
		[ProtoMember(12)]
		public int SalePrice { get; set; }

	}
}
