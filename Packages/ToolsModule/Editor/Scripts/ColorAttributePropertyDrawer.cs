using UnityEditor;
using UnityEngine;


namespace GRV.ToolsModule
{
    [CustomPropertyDrawer(typeof(ColorHtmlPropertyAttribute))]
    public class ColorAttributePropertyDrawer : PropertyDrawer
    {
        protected virtual SerializedProperty GetProperty(SerializedProperty rootProperty) => rootProperty;


        protected virtual Color RequiredColor =>
            ((ColorHtmlPropertyAttribute) this.attribute).Color;
        protected virtual TypeHighlighting TypeHighlighting =>
            ((ColorHtmlPropertyAttribute) this.attribute).TypeHighlighting;
        
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property = GetProperty(property);

            HandleObjectReference(position, property, label);
        }

        
        private void HandleObjectReference(Rect position, SerializedProperty property, GUIContent label)
        {
            Color oldColor = GUI.color;
            
            if (TypeHighlighting == TypeHighlighting.All)
            {
                GUI.color = RequiredColor;
                EditorGUI.PropertyField(position, property, label);
                GUI.color = oldColor;
            }
            else if (TypeHighlighting == TypeHighlighting.OnlyField)
            {
                GUI.backgroundColor = RequiredColor;
                EditorGUI.PropertyField(position, property, label);
                GUI.backgroundColor = oldColor;
            }
        }
    }
}
