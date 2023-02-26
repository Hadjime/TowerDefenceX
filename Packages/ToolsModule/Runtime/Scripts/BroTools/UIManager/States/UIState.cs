using System;

namespace GRV.ToolsModule.BroTools
{
    /// <summary>
    /// UI состояние описывает набор UI элементов
    /// </summary>
    public class UIState
    {
        /// <summary>
        /// Элементы в состоянии
        /// </summary>
        public UIKey[] Keys { get; private set; }

        /// <summary>
        /// Может ли игрок сам закрыть окно 
        /// (Например нажав назад, или фоновое затемнение)
        /// </summary>
        public bool IsEasyToClose { get; set; }

        /// <summary>
        /// Не добавлять состояние в стек состояний
        /// Для каких-то промежуточных состояний, к которым мы не будем возвращаться
        /// </summary>
        public bool DontAddToStack { get; set; }

        /// <summary>
        /// Действие для настройки отображаемых элементов, согласно текущему состоянию
        /// Например, если элемент имеет несколько вариантов, которые меняются в зависимости от состояния, можно задать их здесь
        /// Вызывается перед включением элементов
        /// </summary>
        public Action StateSpecificElementCustomization { get; set; }
        
        /// <summary>
        /// Инициализировано ли окно
        /// Флаг для вызова StateInitialization 1 раз
        /// </summary>
        public bool IsInitialized { get; set; }
        
        /// <summary>
        /// Действие для инициализации отображаемых элементов
        /// Например, если вы хотите вызвать логику при открытие окна как в OnEnable, можно задать ее здесь
        /// Вызывается перед первым включением элементов
        /// </summary>
        public Action StateInitialization { get; set; }

        /// <summary>
        /// Вызывается после того как состояние было открыто
        /// </summary>
        public Action AfterStateAdded { get; set; }

        /// <summary>
        /// Верхний элемент в состоянии
        /// </summary>
        public UIKey TopKey
        {
            get
            {
                return Keys[Keys.Length - 1];
            }
        }

        /// <summary>
        /// Количество элементов в состоянии
        /// </summary>
        public int ElementsCount
        {
            get => Keys.Length;
        }

        public UIState(params UIKey[] keys)
        {
            Keys = keys;
        }

        public UIState(params UIElement[] elements)
        {
            Keys = new UIKey[elements.Length];
            for (int i = 0; i < elements.Length; i++)
            {
                Keys[i] = elements[i].Key;
            }
        }

        /// <summary>
        /// Есть ли такой элемент в состоянии
        /// </summary>
        public bool HasType(UIKey key)
        {
            foreach (UIKey t in Keys)
            {
                if (t == key) return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"({string.Join(", ", Keys)})";
        }

        /// <summary>
        /// Добавить элементы из другого состояния.
        /// ToStart - добавить элементы в начало
        /// </summary>
        public void AddState(UIState other, bool toStart = false)
        {
            if (other == null) return;

            UIKey[] unionKeys = new UIKey[Keys.Length + other.ElementsCount];

            int oldKeysIndex = 0;
            int newKeysIndex = ElementsCount;

            if (toStart)
            {
                oldKeysIndex = other.ElementsCount;
                newKeysIndex = 0;
            }

            Keys.CopyTo(unionKeys, oldKeysIndex);
            other.Keys.CopyTo(unionKeys, newKeysIndex);

            Keys = unionKeys;
        }
    }
}