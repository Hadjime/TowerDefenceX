using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GRV.ToolsModule
{
	public sealed class Coroutines : MonoBehaviour
	{
		private static readonly Dictionary<float, WaitForSeconds> WaitForSecondsDictionary = new Dictionary<float, WaitForSeconds>();
		private static readonly Dictionary<float, WaitForSecondsRealtime> WaitForSecondsRealTime = new Dictionary<float, WaitForSecondsRealtime>();
		private static readonly List<WaitForSecondsRealtime> WaitForSecondsRealTimeList = new List<WaitForSecondsRealtime>();
		
		private static Coroutines instance
		{
			get
			{
				if (_instance == null)
				{
					var go = new GameObject("===[Coroutine Handler]===");
					_instance = go.AddComponent<Coroutines>();
					DontDestroyOnLoad(go);
				}

				return _instance;
			}
		}

		private static Coroutines _instance;


		public static Coroutine StartRoutine(IEnumerator enumerator)
		{
			return instance.StartCoroutine(enumerator);
		}


		public static void StopRoutine(IEnumerator enumerator)
		{
			if (enumerator != null)
				instance.StopCoroutine(enumerator);
		}


		public static void StopRoutine(Coroutine coroutine)
		{
			if (coroutine != null)
				instance.StopCoroutine(coroutine);
		}


		public static WaitForSeconds GetWait(float time)
		{
			if (WaitForSecondsDictionary.TryGetValue(time, out var wait)) return wait;

			WaitForSecondsDictionary[time] = new WaitForSeconds(time);
			return WaitForSecondsDictionary[time];
		}

		[Obsolete("Eсли вызывать с одинаковым временем в нескольких местах то не будет обрабатываться в каком-то месте")]
		public static WaitForSecondsRealtime GetWaitRealTime(float time)
		{
			if (WaitForSecondsRealTime.TryGetValue(time, out var wait)) return wait;

			WaitForSecondsRealTime[time] = new WaitForSecondsRealtime(time);
			return WaitForSecondsRealTime[time];
		}

		public static WaitForSecondsRealtime GetWaitRealtime(float time)
		{
			var wait = WaitForSecondsRealTimeList.Find(w => !w.keepWaiting);

			if (wait != null)
			{
				wait.waitTime = time;
				return wait;
			}

			wait = new WaitForSecondsRealtime(time);
			WaitForSecondsRealTimeList.Add(wait);
			Debug.LogWarning($"Extend Coroutines.WaitForSecondsRealtimeList. It's size now {WaitForSecondsRealTimeList.Count}");
			return wait;
		}
	}
}