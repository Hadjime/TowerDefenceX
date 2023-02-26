using System.Collections;
using UnityEngine;


namespace InternalAssets.Scripts.Infrastructure.Scene
{
	public interface ICoroutineRunner
	{
		Coroutine StartCoroutine(IEnumerator coroutine);
	}
}
