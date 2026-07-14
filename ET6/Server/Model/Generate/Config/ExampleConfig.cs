using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class ExampleConfigCategory : ProtoObject, IMerge
    {
        public static ExampleConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, ExampleConfig> dict = new Dictionary<int, ExampleConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<ExampleConfig> list = new List<ExampleConfig>();
		
        public ExampleConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            ExampleConfigCategory s = o as ExampleConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (ExampleConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public ExampleConfig Get(int id)
        {
            this.dict.TryGetValue(id, out ExampleConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (ExampleConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, ExampleConfig> GetAll()
        {
            return this.dict;
        }

        public ExampleConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class ExampleConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>参数1</summary>
		[ProtoMember(2)]
		public string param1 { get; set; }
		/// <summary>参数2</summary>
		[ProtoMember(3)]
		public int param2 { get; set; }
		/// <summary>参数3</summary>
		[ProtoMember(4)]
		public float param3 { get; set; }
		/// <summary>参数4</summary>
		[ProtoMember(5)]
		public string param4 { get; set; }

	}
}
