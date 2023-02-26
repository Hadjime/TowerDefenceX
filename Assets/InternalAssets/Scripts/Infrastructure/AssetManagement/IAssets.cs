using System.Collections.Generic;
using System.Threading.Tasks;
using InternalAssets.Scripts.Infrastructure.Services;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace InternalAssets.Scripts.Infrastructure.AssetManagement
{
    public interface IAssets : IService
    {
	    Task<GameObject> InstantiateAsync(string path);
        Task<GameObject> InstantiateAsync(string path, Vector3 at);
		void LoadAllAsyncByLabel<T>(string path, System.Action<List<T>> onFinish);
		Task<GameObject> InstantiateAsync(string path, Vector3 at, Transform parent);
		Task<T> LoadAsync<T>(AssetReference assetReference) where T : class;
		Task<T> LoadAsync<T>(string address) where T : class;
		void CleanUp();
	}
}