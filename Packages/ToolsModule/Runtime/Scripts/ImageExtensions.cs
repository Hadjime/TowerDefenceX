using UnityEngine;
using UnityEngine.UI;

namespace GRV.ToolsModule
{
    public static class ImageExtensions
    {
        public static T ChangeAlpha<T>(this T graphic, float newAlpha) where T : Graphic
        {
            Color color = graphic.color;
            color.a = newAlpha;
            graphic.color = color;
            return graphic;
        }
    }
}