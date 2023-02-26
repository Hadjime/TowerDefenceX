using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GRV.ToolsModule.BroTools
{
    /// <summary>
    /// Пул объектов, с которым можно работать как с листом
    /// </summary>
    public class ListPoolable<T> : ObjectPool<T>, IEnumerable<T> where T : Component
    {
        public ListPoolable() : base()
        {
            List = new List<T>();
        }

        public List<T> List { get; }

        public T this[int index]
        {
            get => List[index];
            set => List[index] = value;
        }

        public int Count => List.Count;

        public T Add(T prefab, Transform parent = null)
        {
            T obj = Get(prefab, parent);
            List.Add(obj);
            return obj;
        }

        public void Clear()
        {
            ReturnAll(true);
            List.Clear();
        }

        public bool Contains(T item)
        {
            return List.Contains(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return List.IndexOf(item);
        }

        public bool Remove(T item)
        {
            Return(item);
            return List.Remove(item);
        }

        public void RemoveAt(int index)
        {
            Return(List[index]);
            List.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return List.GetEnumerator();
        }

        //public void ClearFrom(int startIndex)
        //{
        //    for (int i = Count - 1; i >= startIndex; i--)
        //    {
        //        RemoveAt(i);
        //    }
        //}
    }
}