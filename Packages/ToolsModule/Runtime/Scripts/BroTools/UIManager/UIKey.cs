using System;
using System.Collections.Generic;

namespace GRV.ToolsModule.BroTools
{

    /// <summary>
    /// Структура, описывающая UIElement.
    /// Элемент состоит из типа и ключа префаба, если существует несколько префабов одного типа
    /// </summary>
    [Serializable]
    public struct UIKey
    {
        public UIKey(Type type, string prefabKey = null)
        {
            this.type = type;
            this.prefabKey = string.IsNullOrEmpty(prefabKey) ? null : prefabKey;
        }

        public UIKey(string prefabKey)
        {
            this.type = default;
            this.prefabKey = string.IsNullOrEmpty(prefabKey) ? null : prefabKey;
        }

        public Type type;
        public string prefabKey;

        #region Overrides

        public override bool Equals(object obj)
        {
            if (!(obj is UIKey))
            {
                return false;
            }

            var key = (UIKey)obj;
            return EqualityComparer<Type>.Default.Equals(type, key.type) &&
                   prefabKey == key.prefabKey;
        }

        public override int GetHashCode()
        {
            var hashCode = -363334225;
            hashCode = hashCode * -1521134295 + EqualityComparer<Type>.Default.GetHashCode(type);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(prefabKey);
            return hashCode;
        }

        public override string ToString()
        {
            if (prefabKey == null)
            {
                return type.ToString();
            }

            return $"{type}+{prefabKey}";
        }

        public static bool operator ==(UIKey key1, UIKey key2)
        {
            return key1.Equals(key2);
        }

        public static bool operator !=(UIKey key1, UIKey key2)
        {
            return !(key1 == key2);
        }

        #endregion
    }


    /// <summary>
    /// Структура, описывающая UIElement.
    /// Элемент состоит из типа и ключа префаба, если существует несколько префабов одного типа
    /// </summary>
    public struct UIKey<T> where T : UIElement
    {
        private string prefabKey;

        public UIKey(string prefabKey = null)
        {
            this.prefabKey = prefabKey;
        }

        public static implicit operator UIKey(UIKey<T> key)
        {
            return new UIKey(typeof(T), key.prefabKey);
        }
    }
}