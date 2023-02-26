using UnityEngine;


namespace GRV.ToolsModule
{
    public enum TypeHighlighting
    {
        All,
        OnlyField
    }
    public class ColorHtmlPropertyAttribute : PropertyAttribute
    {
        public Color Color;
        public TypeHighlighting TypeHighlighting;
        
        
        // public ColorHtmlPropertyAttribute(KnownColor _color) // System.Drawing.KnownColor структура не хочет работать в Unity
        // {
        //     System.Drawing.Color knownColor = System.Drawing.Color.FromKnownColor(_color);
        //     this.color = new UnityEngine.Color(knownColor.R, knownColor.G, knownColor.B, knownColor.A);
        // }

        /// <summary>
        /// Строки, начинающиеся с '#', будут анализироваться как шестнадцатеричные. Если альфа не указана, по умолчанию используется FF.
        /// Строки, которые не начинаются с символа '#', будут анализироваться как буквальные цвета,
        /// при этом поддерживаются следующие цвета : красный, голубой, синий, темно-синий, светло-голубой, фиолетовый, желтый, салатовый,
        /// фуксия, белый, серебристый, серый, черный, оранжевый, коричневый. , бордовый, зеленый, оливковый, темно-синий, бирюзовый, голубой, пурпурный ..
        /// </summary>
        /// <param name="color"></param>
        /// <param name="typeHighlighting">Тип подсветки: All - Окрасить все, OnlyField - окрасить только область поля</param>
        public ColorHtmlPropertyAttribute(string color, TypeHighlighting typeHighlighting)
        {
            Color MyColour = UnityEngine.Color.clear;
            ColorUtility.TryParseHtmlString (color, out MyColour);
            this.Color = MyColour;
            this.TypeHighlighting = typeHighlighting;
        }

        public ColorHtmlPropertyAttribute(float r, float g, float b, float a, TypeHighlighting typeHighlighting)
        {
            this.Color = new UnityEngine.Color(r, g, b, a);
            this.TypeHighlighting = typeHighlighting;
        }
    }
}