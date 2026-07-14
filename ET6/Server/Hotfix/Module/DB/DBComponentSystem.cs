using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace ET
{
	[FriendClass(typeof(DBComponent))]
    public static class DBComponentSystem
    {
	    public class DBComponentAwakeSystem : AwakeSystem<DBComponent, string, string, int>
	    {
		    public override void Awake(DBComponent self, string dbConnection, string dbName, int zone)
		    {
			    self.mongoClient = new MongoClient(dbConnection);
			    self.database = self.mongoClient.GetDatabase(dbName);
		    }
	    }

	    private static IMongoCollection<T> GetCollection<T>(this DBComponent self, string collection = null)
	    {
		    return self.database.GetCollection<T>(collection ?? typeof (T).Name);
	    }

	    private static IMongoCollection<Entity> GetCollection(this DBComponent self, string name)
	    {
		    return self.database.GetCollection<Entity>(name);
	    }
	    
	    #region Query

	    public static async ETTask<T> Query<T>(this DBComponent self, long id, string collection = null) where T : Entity
	    {
		    using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DB, id % DBComponent.TaskCount))
		    {
			    IAsyncCursor<T> cursor = await self.GetCollection<T>(collection).FindAsync(d => d.Id == id);

			    return await cursor.FirstOrDefaultAsync();
		    }
	    }
	    
	    public static async ETTask<List<T>> Query<T>(this DBComponent self, Expression<Func<T, bool>> filter, string collection = null)
			    where T : Entity
	    {
		    using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DB, RandomHelper.RandInt64() % DBComponent.TaskCount))
		    {
			    IAsyncCursor<T> cursor = await self.GetCollection<T>(collection).FindAsync(filter);

			    return await cursor.ToListAsync();
		    }
	    }

	    public static async ETTask<List<T>> Query<T>(this DBComponent self, long taskId, Expression<Func<T, bool>> filter, string collection = null)
			    where T : Entity
	    {
		    using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DB, taskId % DBComponent.TaskCount))
		    {
			    IAsyncCursor<T> cursor = await self.GetCollection<T>(collection).FindAsync(filter);

			    return await cursor.ToListAsync();
		    }
	    }
	    
	    public static async ETTask Query(this DBComponent self, long id, List<string> collectionNames, List<Entity> result)
	    {
		    if (collectionNames == null || collectionNames.Count == 0)
		    {
			    return;
		    }

		    using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DB, id % DBComponent.TaskCount))
		    {
			    foreach (string collectionName in collectionNames)
			    {
				    IAsyncCursor<Entity> cursor = await self.GetCollection(collectionName).FindAsync(d => d.Id == id);

				    Entity e = await cursor.FirstOrDefaultAsync();

				    if (e == null)
				    {
					    continue;
				    }

				    result.Add(e);
			    }
		    }
	    }

	    public static async ETTask<List<T>> QueryJson<T>(this DBComponent self, string json, string collection = null) where T : Entity
	    {
		    using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DB, RandomHelper.RandInt64() % DBComponent.TaskCount))
		    {
			    FilterDefinition<T> filterDefinition = new JsonFilterDefinition<T>(json);
			    IAsyncCursor<T> cursor = await self.GetCollection<T>(collection).FindAsync(filterDefinition);
			    return await cursor.ToListAsync();
		    }
	    }

	    public static async ETTask<List<T>> QueryJson<T>(this DBComponent self, long taskId, string json, string collection = null) where T : Entity
	    {
		    using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DB, RandomHelper.RandInt64() % DBComponent.TaskCount))
		    {
			    FilterDefinition<T> filterDefinition = new JsonFilterDefinition<T>(json);
			    IAsyncCursor<T> cursor = await self.GetCollection<T>(collection).FindAsync(filterDefinition);
			    return await cursor.ToListAsync();
		    }
	    }

	    #endregion

	    #region Insert

	    public static async ETTask InsertBatch<T>(this DBComponent self, IEnumerable<T> list, string collection = null) where T: Entity
	    {
		    if (collection == null)
		    {
			    collection = typeof (T).Name;
		    }
		    
		    using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DB, RandomHelper.RandInt64() % DBComponent.TaskCount))
		    {
			    await self.GetCollection(collection).InsertManyAsync(list);
		    }
	    }

	    #endregion

	    #region Save

	    public static async ETTask Save<T>(this DBComponent self, T entity, string collection = null) where T : Entity
	    {
		    if (entity == null)
		    {
			    Log.Error($"save entity is null: {typeof (T).Name}");

			    return;
		    }
		    
		    if (collection == null)
		    {
			    collection = entity.GetType().Name;
		    }

		    using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DB, entity.Id % DBComponent.TaskCount))
		    {
			    await self.GetCollection(collection).ReplaceOneAsync(d => d.Id == entity.Id, entity, new ReplaceOptions { IsUpsert = true });
		    }
	    }




        public static async ETTask Save<T>(this DBComponent self, long taskId, T entity, string collection = null) where T : Entity
	    {
		    if (entity == null)
		    {
			    Log.Error($"save entity is null: {typeof (T).Name}");

			    return;
		    }

		    if (collection == null)
		    {
			    collection = entity.GetType().Name;
		    }

		    using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DB, taskId % DBComponent.TaskCount))
		    {
			    await self.GetCollection(collection).ReplaceOneAsync(d => d.Id == entity.Id, entity, new ReplaceOptions { IsUpsert = true });
		    }
	    }

	    public static async ETTask Save(this DBComponent self, long id, List<Entity> entities)
	    {
		    if (entities == null)
		    {
			    Log.Error($"save entity is null");
			    return;
		    }

		    using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DB, id % DBComponent.TaskCount))
		    {
			    foreach (Entity entity in entities)
			    {
				    if (entity == null)
				    {
					    continue;
				    }

				    await self.GetCollection(entity.GetType().Name)
						    .ReplaceOneAsync(d => d.Id == entity.Id, entity, new ReplaceOptions { IsUpsert = true });
			    }
		    }
	    }

	    public static async ETTask SaveNotWait<T>(this DBComponent self, T entity, long taskId = 0, string collection = null) where T : Entity
	    {
		    if (taskId == 0)
		    {
			    await self.Save(entity, collection);

			    return;
		    }

		    await self.Save(taskId, entity, collection);
	    }

	    #endregion

	    #region Remove
	    
	    public static async ETTask<long> Remove<T>(this DBComponent self, long id, string collection = null) where T : Entity
	    {
		    using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DB, id % DBComponent.TaskCount))
		    {
			    DeleteResult result = await self.GetCollection<T>(collection).DeleteOneAsync(d => d.Id == id);

			    return result.DeletedCount;
		    }
	    }

	    public static async ETTask<long> Remove<T>(this DBComponent self, long taskId, long id, string collection = null) where T : Entity
	    {
		    using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DB, taskId % DBComponent.TaskCount))
		    {
			    DeleteResult result = await self.GetCollection<T>(collection).DeleteOneAsync(d => d.Id == id);

			    return result.DeletedCount;
		    }
	    }

	    public static async ETTask<long> Remove<T>(this DBComponent self, Expression<Func<T, bool>> filter, string collection = null) where T : Entity
	    {
		    using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DB, RandomHelper.RandInt64() % DBComponent.TaskCount))
		    {
			    DeleteResult result = await self.GetCollection<T>(collection).DeleteManyAsync(filter);

			    return result.DeletedCount;
		    }
	    }

	    public static async ETTask<long> Remove<T>(this DBComponent self, long taskId, Expression<Func<T, bool>> filter, string collection = null)
			    where T : Entity
	    {
		    using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DB, taskId % DBComponent.TaskCount))
		    {
			    DeleteResult result = await self.GetCollection<T>(collection).DeleteManyAsync(filter);

			    return result.DeletedCount;
		    }
	    }

        #endregion

        #region//测试,更改数据库文档中某一属性
        public static async ETTask UpdateProperty<T>(this DBComponent self, T entity, string propertyName, object newValue, string collection = null) where T : Entity
        {
            if (entity == null)
            {
                Log.Error($"update entity is null: {typeof(T).Name}");
                return;
            }

            if (string.IsNullOrEmpty(propertyName))
            {
                Log.Error($"property name is null or empty for type: {typeof(T).Name}");
                return;
            }

            if (collection == null)
            {
                collection = entity.GetType().Name;
            }

            // 使用Builders<T>.Filter来创建过滤条件
            var filter = Builders<T>.Filter.Eq(e => e.Id, entity.Id);

            // 使用Builders<T>.Update来设置要更新的字段，这里使用字符串propertyName来动态指定属性  
            var updateDefinition = Builders<T>.Update.Set(propertyName, newValue);

            // 使用UpdateOneAsync来更新单个匹配的文档  
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DB, entity.Id % DBComponent.TaskCount))
            {
                await self.GetCollection<T>(collection).UpdateOneAsync(filter, updateDefinition);
            }
        }
        #endregion
    }
}