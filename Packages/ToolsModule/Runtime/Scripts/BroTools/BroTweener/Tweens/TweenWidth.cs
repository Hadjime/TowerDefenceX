using UnityEngine;

namespace GRV.ToolsModule.BroTools
{
    public class TweenWidth : ITween
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

            float w = Mathf.LerpUnclamped(data.fromFloat, data.toFloat, value);

            rectTransform.sizeDelta = new Vector2(w, rectTransform.sizeDelta.y);
        }
    }
}