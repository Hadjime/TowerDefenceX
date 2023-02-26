using UnityEngine;

namespace GRV.ToolsModule.BroTools
{
    /// <summary>
    /// Анимирование позиции
    /// </summary>
    [System.Serializable]
    public class TweenPosition : ITween
    {
        private Transform lastTransform;
        private RectTransform rectTransform;

        private void Init(Transform target)
        {
            if (lastTransform == target) return;
            lastTransform = target;
            rectTransform = target.GetComponent<RectTransform>();
        }

        public void ApplyValue(Transform target, TweenData data, float value)
        {
            Init(target);

            Vector3 newPosition = Vector3.LerpUnclamped(data.fromVector3, data.toVector3, value);

            if (rectTransform)
            {
                if (data.addMode) newPosition += (Vector3)rectTransform.anchoredPosition;
                rectTransform.anchoredPosition = newPosition;
            }
            else
            {
                if (data.addMode) newPosition += target.localPosition;
                target.localPosition = newPosition;
            }
        }
    }
}