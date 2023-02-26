using System;
using System.Collections;
using GRV.ToolsModule;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace InternalAssets.Scripts.Infrastructure.Scene
{
	public class SceneLoader : ISceneLoaderProvider
	{
		private readonly ICoroutineRunner _coroutineRunner;


		public SceneLoader() { }


		public void Load(string name, Action onLoaded = null)
		{
			//TODO: надо прокинуть через конструктор сервис для запуска корутин
			Coroutines.StartRoutine(LoadScene(name, onLoaded));
		}
		
		
		private IEnumerator LoadScene(string nextScene, Action onLoaded = null)
		{
			if (SceneManager.GetActiveScene().name == nextScene)
			{
				onLoaded?.Invoke();
				yield break;
			}
			
			AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(nextScene);

			while (!loadSceneAsync.isDone)
				yield return null;
			
			onLoaded?.Invoke();
		}
	}
}
