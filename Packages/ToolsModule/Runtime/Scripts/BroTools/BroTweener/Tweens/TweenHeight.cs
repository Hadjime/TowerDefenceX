using UnityEngine;

namespace GRV.ToolsModule.BroTools
{
    public class TweenHeight : ITween
    {
        private Transform lastTransform;
        private RectTransform rectTransform;

        public void ApplyValue(Transform target, TweenData data, float value)
        {
            if (lastTransform != target)
            {
                lastTransform = target;
                rectTransform = target.GetComponent<RectTransform>();
            }

            float h = Mathf.LerpUnclamped(data.fromFloat, data.toFloat, value);

            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, h);
        }
    }
}