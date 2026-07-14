using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using xasset;
using Object = UnityEngine.Object;

//namespace xasset
//{
    public class CAsset : Loadable, IEnumerator
    {
        public static readonly Dictionary<string, CAsset> Cache = new Dictionary<string, CAsset>();

        public Action<CAsset> completed;

        public static Func<string, Type, CAsset> Creator { get; set; } = BundledAsset.Create;

        public Object asset { get; protected set; }

        public Object[] subAssets { get; protected set; }

        protected Type type { get; set; }

        protected bool isSubAssets { get; set; }

        public Task<CAsset> Task
        {
            get
            {
                var tcs = new TaskCompletionSource<CAsset>();
                completed += operation => { tcs.SetResult(this); };
                return tcs.Task;
            }
        }

        public bool MoveNext()
        {
            return !isDone;
        }

        public void Reset()
        {
        }

        public object Current => null;

        private static CAsset CreateInstance(string path, Type type)
        {
            //if (string.IsNullOrEmpty(path)) throw new ArgumentException(nameof(path));

            return Creator(path, type);
        }

        protected void OnLoaded(Object target)
        {
            asset = target;
            Finish(asset == null ? "asset == null" : null);
        }

        public T Get<T>() where T : Object
        {
            return asset as T;
        }

        protected override void OnComplete()
        {
            if (completed == null) return;

            var saved = completed;
            completed?.Invoke(this);

            completed -= saved;
        }

        protected override void OnUnused()
        {
            completed = null;
        }

        protected override void OnUnload()
        {
            Cache.Remove(pathOrURL);
        }

        public static CAsset LoadAsync(string path, Type type, Action<CAsset> completed = null)
        {
            return LoadInternal(path, type, completed);
        }

        public static CAsset Load(string path, Type type)
        {
            var asset = LoadInternal(path, type);
            if (asset == null)
                return null;
            asset.LoadImmediate();
            return asset;
        }

        public static CAsset LoadWithSubAssets(string path, Type type)
        {
            var asset = LoadInternal(path, type);
            if (asset == null)
                return null;
            asset.isSubAssets = true;
            asset.LoadImmediate();
            return asset;
        }

        public static CAsset LoadWithSubAssetsAsync(string path, Type type)
        {
            var asset = LoadInternal(path, type);
            if (asset == null)
                return null;
            asset.isSubAssets = true;
            return asset;
        }

        private static CAsset LoadInternal(string path, Type type,
            Action<CAsset> completed = null)
        {
            PathManager.GetActualPath(ref path);
            if (!Versions.Contains(path))
            {
                Logger.E("FileNotFoundException {0}", path);
                return CreateInstance(path, type);
            }

            if (!Cache.TryGetValue(path, out var item))
            {
                item = CreateInstance(path, type);
                Cache.Add(path, item);
            }

            if (completed != null) item.completed += completed;

            item.Load();
            return item;
        }
    }
//}