using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace GRV.ToolsModule.BroTools
{

    public class SimplePool<T> where T : Component
    {
        /// <summary>
        /// Словарь пулов: префаб, пул его экземпляров
        /// </summary>
        private Dictionary<T, List<T>> pool = new Dictionary<T, List<T>>();

        public int ActiveCount(T prefab) => pool.ContainsKey(prefab) ? pool[prefab].Count(_ => _.gameObject.activeSelf) : 0;

        /// <summary>
        /// Получить из пула объект, если он есть,
        /// Иначе создать новый объект и получить его
        /// </summary>
        public T Get(T prefab, Transform parent = null)
        {
            if (!pool.ContainsKey(prefab))
            {
                pool[prefab] = new List<T>();
            }

            T instance = default;

            // Ищем первый выключенный объект
            for (int i = 0; i < pool[prefab].Count; i++)
            {
                if (!pool[prefab][i].gameObject.activeSelf)
                {
                    instance = pool[prefab][i];
                    if (parent) instance.transform.SetParent(parent);
                }
            }

            if (instance == default)
            {
                instance = Object.Instantiate(prefab, parent);
                pool[prefab].Add(instance);
            }

            return instance;
        }


        /// <summary>
        /// Отключить все объекты
        /// </summary>
        public void DeactivateAll()
        {
            foreach (var poolRow in pool)
            {
                foreach (var obj in poolRow.Value)
                {
                    if (obj.gameObject.activeSelf)
                    {
                        obj.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}