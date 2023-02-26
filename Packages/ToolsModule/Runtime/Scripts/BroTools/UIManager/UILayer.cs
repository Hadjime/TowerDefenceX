using System.Collections.Generic;
using UnityEngine;


namespace GRV.ToolsModule.BroTools
{

    public class UILayer : MonoBehaviour
    {
        [SerializeField] private int sortingOrderBase = 10;
        [SerializeField] private int sortingOrderStep = 10;

        private Dictionary<UIKey, UIElement> elements = new Dictionary<UIKey, UIElement>();
        private Stack<UIState> stateStack = new Stack<UIState>();

        /// <summary>
        /// Базовое состояние, которое открывается, когда нет других состояний
        /// </summary>
        public UIState BaseState { get; set; }

        public int SortingOrderBase => sortingOrderBase;
        public int SortingOrderStep => sortingOrderStep;

        private void Awake()
        {
            var elems = GetComponentsInChildren<UIElement>(true);
            foreach (UIElement el in elems)
            {
                elements.Add(el.Key, el);
                if (el.gameObject.activeSelf) el.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GetTopState(out UIState topState) && topState.IsEasyToClose)
                {
                    CloseTopState();
                }
            }
        }

        /// <summary>
        /// Получить элемент из пула элементов
        /// </summary>
        public UIElement GetElement(UIKey key)
        {
            if (elements.ContainsKey(key) && elements[key] != null) return elements[key];
            
            UIElement element = null;

            var prefabs = UIManager.Instance.ElementsProvider.GetElements(key); //Resources.LoadAll("UIElements", key.type);

            foreach (var prefab in prefabs)
            {
                if (prefab is UIElement unboxed)
                {
                    if (unboxed.Key != key) continue;

                    bool activeState = unboxed.gameObject.activeSelf;
                    if (activeState) unboxed.gameObject.SetActive(false);
                    element = Instantiate(unboxed, transform);
                    if (activeState) unboxed.gameObject.SetActive(true);
                }
            }

            if (!element)
            {
                Debug.LogError($"UIManager не может найти элемент {key}");
                return default;
            }

            if (!elements.ContainsKey(key))
            {
                elements.Add(key, element);
            }
            else elements[key] = element;
                    
            return element;
        }

        public T GetElement<T>(string prefabKey = default) where T : UIElement
        {
            return (T)GetElement(new UIKey<T>(prefabKey));
        }

        /// <summary>
        /// Закрываем все другие состояния и ставим новое
        /// </summary>
        public void SetState(UIState state)
        {
            while (stateStack.Count > 0)
            {
                UIState topState = stateStack.Pop();
                HideElementsInState(topState, state);
            }
            AddState(state);
        }

        public void SetState(params UIKey[] keys) => SetState(new UIState(keys));

        public void SetState(params UIElement[] elements) => SetState(new UIState(elements));


        /// <summary>
        /// Открываем новое состояние поверх других
        /// </summary>
        public void AddState(UIState state)
        {
            if (state == null) return;
            if (state.Keys.Length == 0) return;

            if (!state.IsInitialized)
            {
                state.StateInitialization?.Invoke();
                state.IsInitialized = true;
            }
            
            // Перед показом элементов вызываем кастомизацию под новое состояние
            state.StateSpecificElementCustomization?.Invoke();

            // Стартовый порядок сортировки
            int order = GetTopSortingOrder();

            // Ставим элемент на нужный порядок и открываем
            foreach (UIKey key in state.Keys)
            {
                UIElement element = GetElement(key);
                order += sortingOrderStep;

                if (!element.IsActive)
                {
                    element.Show();
                }

                // Применение сортировки только после активации объекта
                element.ApplySorting(order);
            }

            // Добавляем состояние в стэк, если не указано обратное
            if (!state.DontAddToStack)
            {
                stateStack.Push(state);
            }

            state.AfterStateAdded?.Invoke();
        }

        public void AddState(params UIKey[] keys) => AddState(new UIState(keys));

        /// <summary>
        /// Открываем новое состояние, предыдущие скрываем, но оставляем в стеке.
        /// </summary>
        public void AddStateClean(UIState state)
        {
            foreach(var stateToHide in stateStack)
            {
                HideElementsInState(stateToHide, state);
            }
            AddState(state);
        }


        /// <summary>
        /// Верхнее открытое состояние
        /// </summary>
        public bool GetTopState(out UIState state)
        {
            if (stateStack.Count == 0)
            {
                state = null;
                return false;
            }
            state = stateStack.Peek();

            return true;
        }

        /// <summary>
        /// Закрыть верхнее состояние
        /// </summary>
        public void CloseTopState()
        {
            if (stateStack.Count == 0) return;

            UIState topState = stateStack.Pop();
            UIState nextState = BaseState;

            // Если есть предыдущее состояние, берём его сетаем заново
            if (stateStack.Count > 0)
            {
                nextState = stateStack.Pop();
            }

            HideElementsInState(topState, nextState);
            AddState(nextState);
        }

        /// <summary>
        /// Очиситить стек и закрыть все его элементы
        /// </summary>
        public void Clean()
        {
            while (stateStack.Count > 0)
            {
                UIState topState = stateStack.Pop();
                HideElementsInState(topState);
            }
        }

        private void HideElementsInState(UIState state, UIState nextState = null)
        {
            foreach (UIKey key in state.Keys)
            {
                // Если есть следующее состояние, не закрываем элементы входящие в него
                if (nextState != null && nextState.HasType(key)) continue;

                UIElement element = GetElement(key);
                if (element.IsActive) element.Hide();
            }
        }

        public int GetTopSortingOrder()
        {
            // Стартовый порядок сортировки
            int order = sortingOrderBase;

            // Если есть предыдущее состояние, берём порядок его верхнего элемента
            if (GetTopState(out UIState topState))
            {
                UIElement topElement = GetElement(topState.TopKey);
                order = topElement.SortingOrder;
            }

            return order;
        }
    }
}