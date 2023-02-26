using UnityEngine;
using UnityEngine.UI;

namespace GRV.ToolsModule.BroTools
{
    /// <summary>
    /// Анимирование альфы
    /// </summary>
    public class TweenAlpha : ITween
    {
        private CanvasGroup canvasGroup;
        private MaskableGraphic maskableGraphic;

        private Transform lastTarget;

        private void Init(Transform target)
        {
            if (target == lastTarget) return;

            lastTarget = target;

            canvasGroup = target.GetComponent<CanvasGroup>();

            if (!canvasGroup)
            {
                maskableGraphic = target.GetComponent<MaskableGraphic>();
            }

            if (!maskableGraphic)
            {
                canvasGroup = target.gameObject.AddMissingComponent<CanvasGroup>();
            }
        }


        public void ApplyValue(Transform target, TweenData data, float value)
        {
            Init(target);

            float alpha = Mathf.Lerp(data.fromFloat, data.toFloat, value);

            if (canvasGroup)
            {
                canvasGroup.alpha = alpha;
            }
            else if (maskableGraphic)
            {
                Color color = maskableGraphic.color;
                color.a = alpha;
                maskableGraphic.color = color;

#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(maskableGraphic);
#endif
            }
            else
            {
                Debug.Log($"BroTweener.TweenAlpha: Gameobject ({target.name}) must have CanvasGroup or any Graphic for changing Alpha");
            }
        }

    }
}