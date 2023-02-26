using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace GRV.ToolsModule
{
    public static class ExtensionMethods
    {
        public static Vector3 Inverse(this Vector3 self)
        {
            self.x = Mathf.Approximately(self.x, 0) ? 1 : 1 / self.x;
            self.y = Mathf.Approximately(self.y, 0) ? 1 : 1 / self.y;
            self.z = Mathf.Approximately(self.z, 0) ? 1 : 1 / self.z;
            return self;
        }
    
        /// <summary>
        /// Add component to GameObject or return if exists;
        /// </summary>
        public static T AddMissingComponent<T>(this GameObject go) where T : Component
        {
            return AddMissingComponent(go, typeof(T)) as T;
        }

        /// <summary>
        /// Add component to GameObject or return if exists;
        /// </summary>
        public static Component AddMissingComponent(this GameObject go, Type type)
        {
            Component existComponent = go.GetComponent(type);
            if (existComponent == null)
            {
                existComponent = go.AddComponent(type);
            }
            return existComponent;
        }

        /// <summary>
        /// Create child GameObjects with components;
        /// </summary>
        public static GameObject CreateChild(this GameObject go, string name, params Type[] components)
        {
            GameObject child = new GameObject(name, components);
            child.transform.SetParent(go.transform);
            return child;
        }

        /// <summary>
        /// Get child transforms
        /// </summary>
        public static Transform[] GetChildren(this Transform transform)
        {
            Transform[] array = new Transform[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                array[i] = transform.GetChild(i);
            }
            return array;
        }

        /// <summary>
        /// Returns last element of the list
        /// </summary>
        public static T GetLast<T>(this List<T> list)
        {
            return list[list.Count - 1];
        }

        /// <summary>
        /// Convert array to this list
        /// </summary>
        public static List<T> ConvertFrom<T, TSource>(this List<T> list, IEnumerable<TSource> sourceArray, Converter<TSource, T> converter)
        {
            var convertedList = new List<T>();
            foreach (var sourceItem in sourceArray)
            {
                convertedList.Add(converter(sourceItem));
            }
            return convertedList;
        }

        /// <summary>
        /// Waiting coroutines in async methods
        /// </summary>
        public static IEnumerator AsIEnumerator(this Task task)
        {
            while (!task.IsCompleted)
            {
                yield return null;
            }

            if (task.IsFaulted)
            {
                throw task.Exception;
            }
        }

        /// <summary>
        /// Destroys all children of transform
        /// </summary>
        public static void DestroyChildren(this Transform transform)
        {
            foreach (Transform child in transform) GameObject.Destroy(child.gameObject);
        }

        /// <summary>
        /// Calls SetActive(true) and returns T back (convenient for chaining with Instantiate(prefab))
        /// </summary>
        public static T ActivateGameObject<T>(this T monoBehaviour) where T : MonoBehaviour
        {
            monoBehaviour.gameObject.SetActive(true);
            return monoBehaviour;
        }

        /// <summary>
        /// Calls SetActive(true) and returns GameObject back (convenient for chaining with Instantiate(prefab))
        /// </summary>
        public static GameObject ActivateGameObject(this GameObject gameObject)
        {
            gameObject.SetActive(true);
            return gameObject;
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source) action(item);
        }

        /// <summary>
        /// Iterate over collection with access to current element index
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> source, Action<int, T> action)
        {
            int n = 0;
            foreach (var item in source) action(n++, item);
        }

        public static string ToBBCodeString(this Color color) => $"#{ColorUtility.ToHtmlStringRGBA(color)}";

        public static void SetNativePivot(this Image image)
        {
            image.rectTransform.pivot = image.sprite.pivot / image.sprite.rect.size;
        }


        public static void Shuffle<T>(this IList<T> list)
        {
            System.Random rng = new System.Random();

            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static Color ChangeA(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }
    }
}
