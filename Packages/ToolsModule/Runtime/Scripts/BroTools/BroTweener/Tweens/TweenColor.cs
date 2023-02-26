using UnityEngine;
using UnityEngine.UI;

namespace GRV.ToolsModule.BroTools
{
    /// <summary>
    /// Анимирование цвета
    /// </summary>
    public class TweenColor : ITween
    {
        private Transform lastTransform;

        private MaskableGraphic graphic;
        private SpriteRenderer spriteRenderer;

        public void ApplyValue(Transform target, TweenData data, float value)
        {
            if (lastTransform != target)
            {
                lastTransform = target;
                graphic = target.GetComponent<MaskableGraphic>();
                if (!graphic)
                {
                    spriteRenderer = target.GetComponent<SpriteRenderer>();
                }
            }

            if (!graphic && !spriteRenderer)
            {
                Debug.Log("BroTweener.TweenColor: Gameobject must has Graphic or SpriteRenderer");
                return;
            }

            if (graphic)
            {
                graphic.color = Color32.Lerp(data.fromColor, data.toColor, value);
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(graphic);
#endif
            }
            else if (spriteRenderer)
            {
                spriteRenderer.color = Color32.Lerp(data.fromColor, data.toColor, value);
            }
        }
    }
}