using System.Collections.Generic;
using UnityEngine;

namespace GRV.ToolsModule.BroTools
{
    /// <summary>
    /// Пул объектов
    /// </summary>
    public class ObjectPool<T> where T : Component
    {
        private GameObject _basePoolGO;

        private Transform _baseParent;
        private Transform BaseParent
        {
            get
            {
                if (_basePoolGO == null)
                {
                    _basePoolGO = GameObject.Find("Pools") ?? new GameObject("Pools");
                }

                if (_baseParent == null)
                {
                    _baseParent = new GameObject($"{typeof(T).Name}").transform;
                    _baseParent.SetParent(_basePoolGO.transform);
                }

                return _baseParent;
            }
        }

        /// <summary>
        /// Словарь пулов: префаб, пул его экземпляров
        /// </summary>
        private Dictionary<T, Stack<T>> pool = new Dictionary<T, Stack<T>>();

        /// <summary>
        /// Словарь экземпляров взятых из пула
        /// Key - экземпляр префаба, value - оригинальный префаб
        /// </summary>
        private Dictionary<T, T> usingObjects = new Dictionary<T, T>();

        /// <summary>
        /// Наполнить пул объектами до заданного количества
        /// </summary>
        public void FillPool(T prefab, int objectsCount, Transform parent = null, System.Action<T> SetObject = null)
        {
            if (!pool.ContainsKey(prefab))
            {
                pool[prefab] = new Stack<T>();
            }

            if (parent == null)
                parent = BaseParent;

            for (int i = pool[prefab].Count; i < objectsCount; i++)
            {
                var instance = Object.Instantiate(prefab, parent);
                SetObject?.Invoke(instance);
                pool[prefab].Push(instance);
            }
        }

        /// <summary>
        /// Получить из пула объект, если он есть,
        /// Иначе создать новый объект и получить его
        /// </summary>
        public T Get(T prefab, Transform parent = null)
        {
            if (!pool.ContainsKey(prefab))
            {
                pool[prefab] = new Stack<T>();
            }

            T instance = default;

            if (parent == null)
                parent = BaseParent;

            while (pool[prefab].Count > 0)
            {
                // Вынимаем нужный экземпляр из стека
                instance = pool[prefab].Pop();

                // Если сохранённый экземпляр существует, выходим из цикла
                if (instance)
                {
                    instance.transform.SetParent(parent, false);
                    break;
                }

                // Если экземпляр был потерян (например при переходе между сценами), удаляем его из используемых объектов 
                if (usingObjects.ContainsKey(instance)) usingObjects.Remove(instance);
            }

            if (!instance) instance = Object.Instantiate(prefab, parent);

            usingObjects.Add(instance, prefab);

            return instance;
        }

        /// <summary>
        /// Вернуть объект в пул
        /// </summary>
        public void Return(T obj)
        {
            if (usingObjects.TryGetValue(obj, out T originPrefab))
            {
                if (originPrefab)
                {
                    pool[originPrefab].Push(obj);
                }
                usingObjects.Remove(obj);
            }
        }

        /// <summary>
        /// Вернуть все объекты в пул
        /// </summary>
        public void ReturnAll(bool deactivate = false)
        {
            foreach (T obj in usingObjects.Keys)
            {
                if (!obj) continue;

                pool[usingObjects[obj]].Push(obj);

                if (deactivate && obj.gameObject.activeSelf)
                {
                    obj.gameObject.SetActive(false);
                }
            }
            usingObjects.Clear();
        }

        /// <summary>
        /// Очищаем пул
        /// </summary>
        public void Clear()
        {
            pool.Clear();
            usingObjects.Clear();
        }
    }
}