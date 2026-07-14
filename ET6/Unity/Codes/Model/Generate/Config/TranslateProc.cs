using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class TranslateProcCategory : ProtoObject, IMerge
    {
        public static TranslateProcCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, TranslateProc> dict = new Dictionary<int, TranslateProc>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<TranslateProc> list = new List<TranslateProc>();
		
        public TranslateProcCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            TranslateProcCategory s = o as TranslateProcCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (TranslateProc config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public TranslateProc Get(int id)
        {
            this.dict.TryGetValue(id, out TranslateProc item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (TranslateProc)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, TranslateProc> GetAll()
        {
            return this.dict;
        }

        public TranslateProc GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class TranslateProc: ProtoObject, IConfig
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
