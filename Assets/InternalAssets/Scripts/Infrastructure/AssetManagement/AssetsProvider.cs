using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Zenject;


namespace InternalAssets.Scripts.Infrastructure.AssetManagement
{
    public class AssetsProvider : IAssets, IInitializable
    {
		private readonly Dictionary<string, AsyncOperationHandle> _competedCache = new Dictionary<string, AsyncOperationHandle>();
		private readonly Dictionary<string, List<AsyncOperationHandle>> _handles = new Dictionary<string, List<AsyncOperationHandle>>();
		private bool isActivateLog;
		private string loadingLog;


		[Inject]
		public void Construct()
		{

		}

		public void Initialize() =>
			Addressables.InitializeAsync();


		public Task<GameObject> InstantiateAsync(string path)
        {
			return Addressables.InstantiateAsync(path).Task;
		}

        public Task<GameObject> InstantiateAsync(string path, Vector3 at) =>
			Addressables.InstantiateAsync(path, at, Quaternion.identity).Task;


		public Task<GameObject> InstantiateAsync(string path, Vector3 at, Transform parent) =>
			Addressables.InstantiateAsync(path, at, Quaternion.identity, parent).Task;


		public async Task<T> LoadAsync<T>(AssetReference assetReference) where T : class
		{
			if (_competedCache.TryGetValue(assetReference.AssetGUID, out AsyncOperationHandle completedHandle))
				return completedHandle.Result as T;

			return await RunWithCacheOnComplete(
				Addressables.LoadAssetAsync<T>(assetReference),
				cacheKey: assetReference.AssetGUID);
		}




		public async Task<T> LoadAsync<T>(string address) where T : class
		{
			if (_competedCache.TryGetValue(address, out AsyncOperationHandle completedHandle))
				return completedHandle.Result as T;
			

			return await RunWithCacheOnComplete(
				Addressables.LoadAssetAsync<T>(address),
				cacheKey: address);
		}

		public async Task<List<T>> LoadAllAsyncByLabel<T>(string path)
		{
			var startTimeResourcesLoading = new Stopwatch();
			startTimeResourcesLoading.Start();
			List<string> keys = new List<string>();

			int prevInd = -1;
			for (int i = 0; i < path.Length; i++)
			{
				if (path[i].Equals('/'))
				{
					var key = path.Substring(prevInd + 1, i - prevInd - 1);
					keys.Add(key);
					prevInd = i;
				}
			}

			if (prevInd > 0)
			{
				var key = path.Substring(prevInd + 1, path.Length - prevInd - 1);
				keys.Add(key);
			}
			else
			{
				keys.Add(path);
			}

			List<T> list = new List<T>();
			var taskResult = await Addressables
				.LoadResourceLocationsAsync(keys, Addressables.MergeMode.Intersection, null).Task;
			int handledCount = 0;
			bool getComponent = typeof(T).IsSubclassOf(typeof(MonoBehaviour));

			foreach (IResourceLocation resourceLocation in taskResult)
			{
				if (resourceLocation.ResourceType == typeof(GameObject))
				{
					//Debug.LogError("[Resources] load all " + path + " " + resourceLocation.ResourceType + " " + resourceLocation.InternalId + " " + resourceLocation.PrimaryKey + " " + resourceLocation.ProviderId);
					var result = await Addressables.LoadAssetAsync<GameObject>(resourceLocation).Task;
					//Debug.LogError("[Resources] load all " + path + " " + handle.Result + " " + (typeof(T).IsSubclassOf(typeof(MonoBehaviour))), handle.Result);
					if (getComponent) list.Add(result.GetComponent<T>());
					handledCount++;
					if (handledCount != taskResult.Count)
						continue;

					if (isActivateLog)
						loadingLog +=
							$"[Resources] [{path}] - loading time = {startTimeResourcesLoading.Elapsed.Seconds.ToString()} \n";

					return list;
				}
				else
				{
					var result = await Addressables.LoadAssetAsync<T>(resourceLocation).Task;
					list.Add(result);
					handledCount++;
					if (handledCount != taskResult.Count)
						continue;

					if (isActivateLog)
						loadingLog +=
							$"[Resources] [{path}] - loading time = {startTimeResourcesLoading.Elapsed.Seconds.ToString()} \n";

					return list;
				}
			}

			return null;
		}


		public void CleanUp()
		{
			foreach (List<AsyncOperationHandle> resourceHandles in _handles.Values)
				foreach (AsyncOperationHandle handle in resourceHandles)
					Addressables.Release(handle);

			_competedCache.Clear();
			_handles.Clear();
		}


		private async Task<T> RunWithCacheOnComplete<T>(AsyncOperationHandle<T> handle, string cacheKey)
			where T : class
		{
			handle.Completed += completedHandle =>
			{
				_competedCache[cacheKey] = completedHandle;
			};

			AddHandle(cacheKey, handle);


			return await handle.Task;
		}


		private void AddHandle<T>(string key, AsyncOperationHandle<T> handle) where T : class
		{
			if (!_handles.TryGetValue(key,
				out List<AsyncOperationHandle> resourcesHandles))
			{
				resourcesHandles = new List<AsyncOperationHandle>();
				_handles[key] = resourcesHandles;
			}
			resourcesHandles.Add(handle);
		}
	}
}