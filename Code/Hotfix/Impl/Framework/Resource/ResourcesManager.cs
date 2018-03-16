using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using framework;

namespace Hotfix.framework
{
	public class ABInfo : ReferenceDisposer
    {
		private int refCount;
		public string Name { get; set; }

		public int RefCount
		{
			get
			{
				return this.refCount;
			}
			set
			{
				Logger.Log($"{this.Name} refcount: {value}");
				this.refCount = value;
			}
		}

		public AssetBundle AssetBundle { get; set; }

        public void Init(string name, AssetBundle ab)
        {
            this.Name = name;
            this.AssetBundle = ab;
            this.RefCount = 1;
            Logger.Log($"load assetbundle: {this.Name}");
        }

        public override void Dispose()
		{
            Logger.Log($"desdroy assetbundle: {this.Name}");
			this.AssetBundle?.Unload(true);

            base.Dispose();
        }
	}

    public class AssetsBundleLoaderAsync : ReferenceDisposer
    {
        private AssetBundleCreateRequest request;

        private TaskCompletionSource<AssetBundle> tcs;

        public bool Update()
        {
            if (!this.request.isDone)
            {
                return false;
            }

            TaskCompletionSource<AssetBundle> t = tcs;
            t.SetResult(this.request.assetBundle);

            return true;
        }

        public Task<AssetBundle> LoadAsync(string bundleName)
        {
            this.tcs = new TaskCompletionSource<AssetBundle>();
            this.request = AssetBundle.LoadFromFileAsync(Path.Combine(PathHelper.AppResPath, bundleName));
            return this.tcs.Task;
        }

        public override void Dispose()
        {
            this.tcs = null;
            this.request = null;

            base.Dispose();
        }
    }

    public class AssetsLoaderAsync : ReferenceDisposer
    {
        private AssetBundle assetBundle;

        private AssetBundleRequest request;

        private TaskCompletionSource<bool> tcs;

        public void SetAssetBundle(AssetBundle ab)
        {
            this.assetBundle = ab;
        }

        public bool Update()
        {
            if (!this.request.isDone)
            {
                return false;
            }

            TaskCompletionSource<bool> t = tcs;
            t.SetResult(true);

            return true;
        }

        public async Task<UnityEngine.Object[]> LoadAllAssetsAsync()
        {
            await InnerLoadAllAssetsAsync();
            return this.request.allAssets;
        }

        private Task<bool> InnerLoadAllAssetsAsync()
        {
            this.tcs = new TaskCompletionSource<bool>();
            this.request = assetBundle.LoadAllAssetsAsync();
            return this.tcs.Task;
        }

        public override void Dispose()
        {
            this.assetBundle = null;
            this.request = null;

            base.Dispose();
        }
    }

    public enum EABType
    {
        Invalid,
        Player,
        Monster,
        NPC,
        Scene,
        Effect,
        UI,
        Icon,
        Audio,
        Misc,
    }

    internal sealed class ResourcesManager : GameModuleBase, IResourcesManager
    {

		public static AssetBundleManifest AssetBundleManifestObject { get; set; }

		private readonly Dictionary<string, UnityEngine.Object> resourceCache = new Dictionary<string, UnityEngine.Object>();

		private readonly Dictionary<string, ABInfo> bundles = new Dictionary<string, ABInfo>();
		
		// lru缓存队列
		private readonly QueueDictionary<string, ABInfo> cacheDictionary = new QueueDictionary<string, ABInfo>();
        //
        private readonly List<AssetsBundleLoaderAsync> mAssetsBundleLoaderAsynList = new List<AssetsBundleLoaderAsync>();
        //
        private readonly List<AssetsLoaderAsync> mAssetsLoaderAsynList = new List<AssetsLoaderAsync>();

        internal override int Priority
        {
            get
            {
                return 0;
            }
        }

        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            for (int i = mAssetsBundleLoaderAsynList.Count - 1; i >= 0; --i)
            {
                if (mAssetsBundleLoaderAsynList[i].Update())
                {
                    mAssetsBundleLoaderAsynList.RemoveAt(i);
                }
            }

            for (int i = mAssetsLoaderAsynList.Count - 1; i >= 0; --i)
            {
                if (mAssetsLoaderAsynList[i].Update())
                {
                    mAssetsLoaderAsynList.RemoveAt(i);
                }
            }
        }
        
        internal override void Shutdown()
        {
            foreach (var abInfo in this.bundles)
            {
                abInfo.Value?.AssetBundle?.Unload(true);
            }

            this.bundles.Clear();
            this.cacheDictionary.Clear();
            this.resourceCache.Clear();
        }

        public void LoadManifest()
        {
            LoadOneBundle("StreamingAssets");
            AssetBundleManifestObject = GetAsset<AssetBundleManifest>("StreamingAssets", "AssetBundleManifest");
        }

        public K GetAssetByType<K>(EABType abType, string bundleName) where K : class
        {
            return GetAsset<K>(ResourcesHelper.GetBundleNameByType(abType, bundleName), bundleName);
        }

        public K GetAsset<K>(string bundleName, string prefab) where K : class
		{
			string path = $"{bundleName}/{prefab}".ToLower();

			UnityEngine.Object resource = null;
			if (!this.resourceCache.TryGetValue(path, out resource))
			{
				throw new Exception($"not found asset: {path}");
			}
			
			return resource as K;
		}

        public void UnLoadBundleByType(EABType type, string abName)
        {
            UnloadBundle(ResourcesHelper.GetBundleNameByType(type, abName));
        }

		public void UnloadBundle(string assetBundleName)
		{
			assetBundleName = assetBundleName.ToLower();
			
			this.UnloadOneBundle(assetBundleName);

			string[] dependencies = ResourcesHelper.GetSortedDependencies(assetBundleName);

			//Logger.Log($"-----------dep unload {assetBundleName} dep: {dependencies.ToList().ListToString()}");
			foreach (string dependency in dependencies)
			{
				this.UnloadOneBundle(dependency);
			}
		}

		private void UnloadOneBundle(string assetBundleName)
		{
			assetBundleName = assetBundleName.ToLower();
			
			//Logger.Log($"unload bundle {assetBundleName}");
			ABInfo abInfo;
			if (!this.bundles.TryGetValue(assetBundleName, out abInfo))
			{
				throw new Exception($"not found assetBundle: {assetBundleName}");
			}

			--abInfo.RefCount;
			if (abInfo.RefCount > 0)
			{
				return;
			}
			
			
			this.bundles.Remove(assetBundleName);
			
			// 缓存10个包
			this.cacheDictionary.Enqueue(assetBundleName, abInfo);
			if (this.cacheDictionary.Count > 10)
			{
				abInfo = this.cacheDictionary[this.cacheDictionary.FirstKey];
				this.cacheDictionary.Dequeue();
				abInfo.Dispose();
			}
			Logger.Log($"cache count: {this.cacheDictionary.Count}");
		}

        public void LoadBundleByType(EABType type, string abName)
        {
            LoadBundle(ResourcesHelper.GetBundleNameByType(type, abName));
        }
		
		/// <summary>
		/// 同步加载assetbundle
		/// </summary>
		/// <param name="assetBundleName"></param>
		/// <returns></returns>
		public void LoadBundle(string assetBundleName)
		{
			assetBundleName = assetBundleName.ToLower();
			this.LoadOneBundle(assetBundleName);

			string[] dependencies = ResourcesHelper.GetSortedDependencies(assetBundleName);

			Logger.Log($"-----------dep load {assetBundleName} dep: {dependencies.ToList().ListToString()}");
			foreach (string dependency in dependencies)
			{
				if (string.IsNullOrEmpty(dependency))
				{
					continue;
				}
				this.LoadOneBundle(dependency);
			}
		}
		
		public void LoadOneBundle(string assetBundleName)
		{
			ABInfo abInfo;
			if (this.bundles.TryGetValue(assetBundleName, out abInfo))
			{
				++abInfo.RefCount;
				return;
			}


			if (this.cacheDictionary.ContainsKey(assetBundleName))
			{
				abInfo = this.cacheDictionary[assetBundleName];
				++abInfo.RefCount;
				this.bundles[assetBundleName] = abInfo;
				this.cacheDictionary.Remove(assetBundleName);
				return;
			}

			AssetBundle assetBundle = AssetBundle.LoadFromFile(Path.Combine(PathHelper.AppResPath, assetBundleName));

			if (!assetBundle.isStreamedSceneAssetBundle)
			{
				// 异步load资源到内存cache住
				UnityEngine.Object[] assets = assetBundle.LoadAllAssets();
				foreach (UnityEngine.Object asset in assets)
				{
					string path = $"{assetBundleName}/{asset.name}".ToLower();
					this.resourceCache[path] = asset;
				}
			}
			
            ABInfo tmpABInfo = ReferencePool.Fetch<ABInfo>();
            tmpABInfo.Init(assetBundleName, assetBundle);
            this.bundles[assetBundleName] = tmpABInfo;
        }

        public async Task LoadBundleByTypeAsync(EABType abType, string abName)
        {
            await LoadBundleAsync(ResourcesHelper.GetBundleNameByType(abType, abName));
        }

        /// <summary>
        /// 异步加载assetbundle
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <returns></returns>
        public async Task LoadBundleAsync(string assetBundleName)
		{
			assetBundleName = assetBundleName.ToLower();
			await this.LoadOneBundleAsync(assetBundleName);

			string[] dependencies = ResourcesHelper.GetSortedDependencies(assetBundleName);

			//Logger.Log($"-----------dep load {assetBundleName} dep: {dependencies.ToList().ListToString()}");
			foreach (string dependency in dependencies)
			{
				if (string.IsNullOrEmpty(dependency))
				{
					continue;
				}
				await this.LoadOneBundleAsync(dependency);
			}
		}

		public async Task LoadOneBundleAsync(string assetBundleName)
		{
			ABInfo abInfo;
			if (this.bundles.TryGetValue(assetBundleName, out abInfo))
			{
				++abInfo.RefCount;
				return;
			}


			if (this.cacheDictionary.ContainsKey(assetBundleName))
			{
				abInfo = this.cacheDictionary[assetBundleName];
				++abInfo.RefCount;
				this.bundles[assetBundleName] = abInfo;
				this.cacheDictionary.Remove(assetBundleName);
				return;
			}

			AssetBundle assetBundle;
            AssetsBundleLoaderAsync assetsBundleLoaderAsync = ReferencePool.Fetch<AssetsBundleLoaderAsync>();
            mAssetsBundleLoaderAsynList.Add(assetsBundleLoaderAsync);
            assetBundle = await assetsBundleLoaderAsync.LoadAsync(assetBundleName);
            assetsBundleLoaderAsync.Dispose();

            if (!assetBundle.isStreamedSceneAssetBundle)
			{
				// 异步load资源到内存cache住
				UnityEngine.Object[] assets;
                AssetsLoaderAsync assetsLoaderAsync = ReferencePool.Fetch<AssetsLoaderAsync>();
                assetsLoaderAsync.SetAssetBundle(assetBundle);
                mAssetsLoaderAsynList.Add(assetsLoaderAsync);
                assets = await assetsLoaderAsync.LoadAllAssetsAsync();
                assetsLoaderAsync.Dispose();

                foreach (UnityEngine.Object asset in assets)
				{
					string path = $"{assetBundleName}/{asset.name}".ToLower();
					this.resourceCache[path] = asset;
				}
			}

            ABInfo tmpABInfo = ReferencePool.Fetch<ABInfo>();
            tmpABInfo.Init(assetBundleName, assetBundle);
            this.bundles[assetBundleName] = tmpABInfo;

        }
	}
}