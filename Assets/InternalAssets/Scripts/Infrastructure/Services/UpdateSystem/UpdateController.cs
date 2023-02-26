using System;
using System.Collections.Generic;

namespace InternalAssets.Scripts.Infrastructure.Services.UpdateSystem
{
	public abstract class UpdateController<T> : IUpdateController where T : IBaseUpdatable
	{
		private class OrderComparer : IComparer<T>
		{
			public int Compare(T x, T y)
			{
				return y.UpdateOrder.CompareTo(x.UpdateOrder);
			}
		}
		
		private readonly Type servicedUpdateableType;
		private readonly List<T> updatables;
		private readonly Queue<T> toAdd;
		private readonly Queue<T> toRemove;
		private readonly IComparer<T> orderComparer;

		private int updatebleCount;
		
		public Type ServicedUpdatableType => servicedUpdateableType;

		public UpdateController()
		{
			servicedUpdateableType = typeof(T);
			updatables = new List<T>();
			toAdd = new Queue<T>();
			toRemove = new Queue<T>();
			orderComparer = new OrderComparer();
			updatebleCount = 0;
		}
		
		public bool Register(IBaseUpdatable value)
		{
			if (!(value is T generic) ||
				updatables.Contains(generic))
			{
				return false;
			}
			
			toAdd.Enqueue(generic);
			return true;
		}

		public bool UnRegister(IBaseUpdatable value)
		{
			if (!(value is T generic) ||
				!updatables.Contains(generic))
			{
				return false;
			}
			
			toRemove.Enqueue(generic);
			return true;
		}

		public void Processing()
		{
			QueueProcessing();
			for (int i = 0; i < updatebleCount; i++)
			{
				T updatable = updatables[i];
				#if UNITY_EDITOR
				UnityEngine.Profiling.Profiler.BeginSample($"{updatable.GetType()}.UpdateProcessing()");
				#endif
				UpdateProcessing(updatable);
				#if UNITY_EDITOR
				UnityEngine.Profiling.Profiler.EndSample();
				#endif
			}

		}

		protected abstract void UpdateProcessing(T updatable);

		private void QueueProcessing()
		{
			while (toAdd.Count > 0)
			{
				T updatable = toAdd.Dequeue();
				updatables.Add(updatable);
			}

			while (toRemove.Count > 0)
			{
				T updatable = toRemove.Dequeue();
				updatables.Remove(updatable);
			}
			
			updatables.Sort(orderComparer);
			updatebleCount = updatables.Count;
		}
	}
}