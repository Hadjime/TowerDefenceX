using UnityEngine;

namespace GRV.ToolsModule.BroTools
{
    public abstract class UIElement : MonoBehaviour
    {
        [SerializeField]
        private UIKey key;
        private Canvas canvas;

        /// <summary>
        /// Определяющий ключ элемента
        /// </summary>
        public UIKey Key => new UIKey(GetType(), key.prefabKey);

        public int SortingOrder { get; protected set; }

        /// <summary>
        /// Элемент активен
        /// </summary>
        public bool IsActive { get; protected set; }

        public void SetActive(bool state)
        {
            gameObject.SetActive(state);
            IsActive = state;
        }

        public virtual void Show()
        {
            SetActive(true);
        }

        public virtual void Hide()
        {
            SetActive(false);
        }

        /// <summary>
        /// Применяет сортировку к канвасу. Вызывать только после включения объекта
        /// </summary>
        public void ApplySorting(int order)
        {
            SortingOrder = order;
            ApplySorting();
        }

        /// <summary>
        /// Применяет сортировку к канвасу. Вызывать только после включения объекта
        /// </summary>
        public void ApplySorting()
        {
            if (!canvas) canvas = gameObject.AddMissingComponent<Canvas>();

            // Этот флаг можно проставить только после активации канваса
            if (!canvas.overrideSorting) canvas.overrideSorting = true;
            canvas.sortingOrder = SortingOrder;
        }
    }
}