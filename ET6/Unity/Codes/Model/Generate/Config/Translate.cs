using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class TranslateCategory : ProtoObject, IMerge
    {
        public static TranslateCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, Translate> dict = new Dictionary<int, Translate>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<Translate> list = new List<Translate>();
		
        public TranslateCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            TranslateCategory s = o as TranslateCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (Translate config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public Translate Get(int id)
        {
            this.dict.TryGetValue(id, out Translate item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (Translate)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, Translate> GetAll()
        {
            return this.dict;
        }

        public Translate GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class Translate: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>key</summary>
		[ProtoMember(2)]
		public string key { get; set; }
		/// <summary>中文</summary>
		[ProtoMember(3)]
		public string cn { get; set; }
		/// <summary>英文</summary>
		[ProtoMember(4)]
		public string en { get; set; }
		/// <summary>繁体中文</summary>
		[ProtoMember(5)]
		public string hk { get; set; }

	}
}
