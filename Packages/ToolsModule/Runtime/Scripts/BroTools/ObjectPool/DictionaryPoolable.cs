using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GRV.ToolsModule.BroTools
{

    public class DictionaryPoolable<TKey, TValue> : ObjectPool<TValue>, IEnumerable<KeyValuePair<TKey, TValue>> where TValue : Component
    {
        public DictionaryPoolable() : base()
        {
            Dictionary = new Dictionary<TKey, TValue>();
        }

        public Dictionary<TKey, TValue> Dictionary { get; }

        public Dictionary<TKey, TValue>.KeyCollection Keys => Dictionary.Keys;
        public Dictionary<TKey, TValue>.ValueCollection Values => Dictionary.Values;

        public TValue this[TKey key]
        {
            get => Dictionary[key];
            set => Dictionary[key] = value;
        }

        public TValue Add(TKey key, TValue prefab, Transform parent = null)
        {
            if (!ContainsKey(key))
            {
                TValue obj = Get(prefab, parent);
                Dictionary.Add(key, obj);
            }
            return Dictionary[key];
        }

        public bool Remove(TKey key)
        {
            if (ContainsKey(key))
            {
                Return(Dictionary[key]);
            }
            return Dictionary.Remove(key);
        }

        public bool ContainsKey(TKey key)
        {
            return Dictionary.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            //value = default;
            return Dictionary.TryGetValue(key, out value);
        }

        public void Clear()
        {
            ReturnAll(true);
            Dictionary.Clear();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return Dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Dictionary.GetEnumerator();
        }
    }
}