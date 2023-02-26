using UnityEditor;
using UnityEngine;

namespace GRV.ToolsModule.BroTools
{
    [CustomPropertyDrawer(typeof(UIElementAnimated.AnimationSettings))]
    public class UIAnimationSettingsDrawer : PropertyDrawer
    {
        private bool foldout;

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            //property.serializedObject.Update();

            Rect rect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

            foldout = EditorGUI.BeginFoldoutHeaderGroup(rect, foldout, "UI Element Animation");
            if (foldout)
            {
                EditorGUI.indentLevel++;

                var settingsType = property.FindPropertyRelative("source");
                var currentTypeIndex = settingsType.enumValueIndex;

                rect.y += EditorGUIUtility.singleLineHeight;

                settingsType.enumValueIndex = EditorGUI.Popup(rect, "Animation Source", settingsType.enumValueIndex, settingsType.enumDisplayNames);

                switch ((UIElementAnimated.AnimationSettings.SourceType)settingsType.enumValueIndex)
                {
                    case UIElementAnimated.AnimationSettings.SourceType.Preset:

                        rect.y += EditorGUIUtility.singleLineHeight;
                        EditorGUI.PropertyField(rect, property.FindPropertyRelative("presetName"), new GUIContent("Preset Name"), true);
                        break;

                    case UIElementAnimated.AnimationSettings.SourceType.Tweener:

                        rect.y += EditorGUIUtility.singleLineHeight;
                        var split = property.FindPropertyRelative("splitTweeners").boolValue;

                        EditorGUI.PropertyField(rect, property.FindPropertyRelative("splitTweeners"), new GUIContent("Split"), true);

                        rect.y += EditorGUIUtility.singleLineHeight;
                        EditorGUI.PropertyField(rect, property.FindPropertyRelative("tweenerShow"), new GUIContent(split ? "Tweener Show" : "Tweener Show & Hide"), true);

                        if (split)
                        {
                            rect.y += EditorGUIUtility.singleLineHeight;
                            EditorGUI.PropertyField(rect, property.FindPropertyRelative("tweenerHide"), new GUIContent("Tweener Hide"), true);
                        }
                        break;
                }

                EditorGUI.indentLevel--;
            }
            EditorGUI.EndFoldoutHeaderGroup();

            property.serializedObject.ApplyModifiedProperties();
            EditorGUI.EndProperty();
        }


        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!foldout) return EditorGUIUtility.singleLineHeight;
            switch ((UIElementAnimated.AnimationSettings.SourceType)property.FindPropertyRelative("source").enumValueIndex)
            {
                case UIElementAnimated.AnimationSettings.SourceType.Preset: return EditorGUIUtility.singleLineHeight * 3;
                case UIElementAnimated.AnimationSettings.SourceType.Tweener:
                    var split = property.FindPropertyRelative("splitTweeners").boolValue;
                    return EditorGUIUtility.singleLineHeight * (split ? 5 : 4);
                default:
                    return EditorGUIUtility.singleLineHeight * 2;
            }
        }
    }
}