using System;
using UnityEditor;
using UnityEngine;

namespace GRV.ToolsModule.BroTools
{

    [CustomEditor(typeof(BroTweener)), CanEditMultipleObjects]

    public class BroTweenerEditor : UnityEditor.Editor
    {
        BroTweener T;
        Event currentEvent;

        SerializedProperty animTypeProp;
        SerializedProperty curveProp;
        SerializedProperty durationProp;
        SerializedProperty delayProp;
        SerializedProperty playOnEnableProp;
        SerializedProperty ignoreTimescaleProp;
        SerializedProperty targetProp;
        SerializedProperty randomStartProp;

        SerializedProperty tweenList;

        SerializedProperty onFinishProp;


        private void OnEnable()
        {
            T = (BroTweener)target;

            animTypeProp = serializedObject.FindProperty("_animType");
            curveProp = serializedObject.FindProperty("_curve");
            durationProp = serializedObject.FindProperty("_duration");
            delayProp = serializedObject.FindProperty("_delay");
            playOnEnableProp = serializedObject.FindProperty("_playOnEnable");
            ignoreTimescaleProp = serializedObject.FindProperty("_ignoreTimescale");
            targetProp = serializedObject.FindProperty("_target");
            randomStartProp = serializedObject.FindProperty("_randomStart");

            tweenList = serializedObject.FindProperty("_tweenDataList");

            onFinishProp = serializedObject.FindProperty("_onFinish");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            currentEvent = Event.current;

            EditorGUILayout.Space();

            DrawMainLayout();

            EditorGUILayout.Space();

            DrawListLayout();

            serializedObject.ApplyModifiedProperties();
        }



        private bool isExtra;

        private void DrawMainLayout()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUILayout.VerticalScope())
                {
                    EditorGUILayout.PropertyField(animTypeProp);
                    EditorGUILayout.PropertyField(durationProp);
                    EditorGUILayout.PropertyField(delayProp);
                    EditorGUILayout.PropertyField(playOnEnableProp);
                }
                using (new EditorGUILayout.VerticalScope())
                {
                    EditorGUILayout.PropertyField(curveProp, GUIContent.none, GUILayout.ExpandHeight(true));
                }
            }

            DrawPreview();

            if (isExtra = EditorGUILayout.Foldout(isExtra, "Extra Options", true))
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(targetProp);
                EditorGUILayout.PropertyField(ignoreTimescaleProp);
                EditorGUILayout.PropertyField(randomStartProp);
                EditorGUI.indentLevel--;
            }
        }




        float previewValue = 0;
        /// <summary>
        /// Preview slider
        /// </summary>
        private void DrawPreview()
        {
            if (tweenList.arraySize == 0) return;
            if (targets.Length > 1) return;

            using (new EditorGUILayout.HorizontalScope())
            {
                using (var check = new EditorGUI.ChangeCheckScope())
                {
                    previewValue = EditorGUILayout.Slider(new GUIContent("Progress"), previewValue, 0, 1);

                    if (check.changed)
                    {
                        T.SetProgress(previewValue);
                    }
                }
            }
        }




        private void DrawListLayout()
        {

            if (tweenList.hasMultipleDifferentValues)
            {
                EditorGUILayout.LabelField("Tweens are different in multiply objects", EditorStyles.helpBox);
                return;
            }

            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {

                Color baseBGColor = GUI.backgroundColor;

                for (int i = 0; i < tweenList.arraySize; i++)
                {
                    // Color element background
                    GUI.backgroundColor = Defaults.ElementBGColors[i % 2];
                    using (var verticalScope = new EditorGUILayout.VerticalScope(Defaults.ElementBackgroundWhite))
                    {
                        GUI.backgroundColor = baseBGColor;

                        SerializedProperty item = tweenList.GetArrayElementAtIndex(i);
                        SerializedProperty typeProp = item.FindPropertyRelative("type");
                        Tween type = (Tween)typeProp.enumValueIndex;

                        DrawElementHeader(i, type, item);

                        using (new BroUtilites.FixWideScope())
                        {
                            DrawElementOverride(type, item);

                            // Draw context button only if we are not editing tag. Because it in one place
                            if (elementTagEditing != i)
                            {
                                GUIStyle paneOptions = "PaneOptions";
                                Rect buttonRect = new Rect(verticalScope.rect.width, verticalScope.rect.y + 2, paneOptions.fixedWidth, paneOptions.fixedHeight);
                                if (GUI.Button(buttonRect, GUIContent.none, paneOptions))
                                {
                                    RightClickOnItem(i);
                                }
                            }

                            GUIContent gcFrom = new GUIContent("From");
                            GUIContent gcTo = new GUIContent("To");

                            switch (type)
                            {
                                case Tween.Position:
                                case Tween.Rotation:
                                case Tween.Scale:

                                    EditorGUILayout.PropertyField(item.FindPropertyRelative("fromVector3"), gcFrom);
                                    EditorGUILayout.PropertyField(item.FindPropertyRelative("toVector3"), gcTo);
                                    break;

                                case Tween.Width:
                                case Tween.Height:

                                    using (new EditorGUILayout.HorizontalScope())
                                    {
                                        EditorGUILayout.PropertyField(item.FindPropertyRelative("fromFloat"), gcFrom);
                                        EditorGUIUtility.labelWidth = 40f;
                                        EditorGUILayout.PropertyField(item.FindPropertyRelative("toFloat"), gcTo);
                                        EditorGUIUtility.labelWidth = 0f;
                                    }
                                    break;

                                case Tween.Color:

                                    using (new EditorGUILayout.HorizontalScope())
                                    {
                                        EditorGUILayout.PropertyField(item.FindPropertyRelative("fromColor"), gcFrom);
                                        EditorGUIUtility.labelWidth = 40f;
                                        EditorGUILayout.PropertyField(item.FindPropertyRelative("toColor"), gcTo);
                                        EditorGUIUtility.labelWidth = 0f;
                                    }
                                    break;

                                case Tween.Alpha:

                                    EditorGUILayout.Slider(item.FindPropertyRelative("fromFloat"), 0f, 1f, gcFrom);
                                    EditorGUILayout.Slider(item.FindPropertyRelative("toFloat"), 0f, 1f, gcTo);
                                    break;

                                case Tween.Link:

                                    EditorGUILayout.PropertyField(item.FindPropertyRelative("linkedTweener"), new GUIContent("Tweener"));
                                    break;

                                case Tween.Action:

                                    EditorGUILayout.PropertyField(item.FindPropertyRelative("processEvent"));
                                    break;
                            }

                            // If tween overrided, draw blue override mark at left
                            if (BroUtilites.IsPropertyOverrided(item))
                            {
                                BroUtilites.DrawBlueOverrideMark(verticalScope.rect);
                            }

                        }

                    }
                }

                // Empty list label
                if (tweenList.arraySize == 0)
                {
                    EditorGUILayout.LabelField("There is no tweens yet...");
                }

            }


            // Add tween button with context menu for choose tween type
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                if (EditorGUILayout.DropdownButton(new GUIContent("Add Tween"), FocusType.Keyboard, EditorStyles.miniButton, GUILayout.Width(100)))
                {
                    GenericMenu menu = new GenericMenu();
                    foreach (Tween type in Enum.GetValues(typeof(Tween)))
                    {
                        menu.AddItem(new GUIContent(type.ToString()), false, AddTween, type);
                    }
                    menu.ShowAsContext();
                }
            }
        }




        bool tagEditorInFocus = false;
        int elementTagEditing = -1;

        /// <summary>
        /// Header of the element. Include override tween data
        /// </summary>
        private void DrawElementHeader(int index, Tween type, SerializedProperty item)
        {
            using (var scopes = new EditorGUILayout.HorizontalScope(GUILayout.Height(EditorGUIUtility.singleLineHeight), GUILayout.ExpandWidth(true)))
            {
                GUILayout.FlexibleSpace();
                Rect rect = scopes.rect;

                GUIContent title = new GUIContent($"Tween {type}");
                float titleWidth = EditorStyles.boldLabel.CalcSize(title).x;
                EditorGUI.indentLevel--;
                EditorGUI.LabelField(new Rect(rect.x, rect.y, titleWidth, rect.height), title, EditorStyles.boldLabel);
                // Tag redactor in right of title;
                SerializedProperty tagProp = item.FindPropertyRelative("tag");
                Rect tagRect = new Rect(rect.x + titleWidth + 4, rect.y, rect.width - titleWidth - 4, rect.height);
                bool isEditing = index == elementTagEditing;
                if (isEditing)
                {
                    // name control for setting focus
                    GUI.SetNextControlName("tagFieldControl");
                    EditorGUI.PropertyField(tagRect, tagProp, GUIContent.none);
                    if (!tagEditorInFocus)
                    {
                        GUI.FocusControl("tagFieldControl");
                        tagEditorInFocus = true;
                    }
                    else
                    {
                        // if we stop editing, cancel input field
                        bool notInFocus = GUI.GetNameOfFocusedControl() != "tagFieldControl";
                        if (notInFocus || BroUtilites.GetKeyUp(KeyCode.Return))
                        {
                            elementTagEditing = -1;
                            tagEditorInFocus = false;
                            Repaint();
                        }
                    }
                }
                else
                {
                    // Draw tag label if it exists
                    GUIContent tagContent = new GUIContent();
                    bool isOverrided = BroUtilites.IsPropertyOverrided(tagProp);
                    if (!string.IsNullOrEmpty(tagProp.stringValue))
                    {
                        tagContent.text = $"[{tagProp.stringValue}]";
                        tagContent.tooltip = "Use tag for controlling tween trough script";
                    }

                    EditorGUI.LabelField(tagRect, tagContent, isOverrided ? EditorStyles.boldLabel : EditorStyles.label);
                }


                // Right click on element header
                if (rect.Contains(currentEvent.mousePosition) && currentEvent.type == EventType.ContextClick)
                {
                    RightClickOnItem(index);
                    currentEvent.Use();
                }
            }

            if (item.FindPropertyRelative("addMode").boolValue)
            {
                EditorGUILayout.LabelField("Works in Addition Mode!", Defaults.AdditionModeWarning);
            }

            EditorGUI.indentLevel++;


        }

        /// <summary>
        /// Extend layout in tween window. if choosed toggle Extend tween
        /// </summary>
        private void DrawElementOverride(Tween type, SerializedProperty item)
        {
            using (var fixScopes = new BroUtilites.FixWideScope())
            {
                if (item.FindPropertyRelative("overrideTarget").boolValue)
                {
                    GUIContent targetCont = new GUIContent("Target", "If chosen, tween will control this gameobject");
                    EditorGUILayout.PropertyField(item.FindPropertyRelative("target"), targetCont);
                }
                if (item.FindPropertyRelative("overrideScopes").boolValue)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        SerializedProperty leftProp = item.FindPropertyRelative("leftEdge");
                        SerializedProperty rightProp = item.FindPropertyRelative("rightEdge");

                        float minValue = leftProp.floatValue;
                        float maxValue = rightProp.floatValue;
                        EditorGUI.BeginChangeCheck();
                        EditorGUILayout.MinMaxSlider(new GUIContent("Scopes"), ref minValue, ref maxValue, 0f, 1f);
                        if (EditorGUI.EndChangeCheck())
                        {
                            leftProp.floatValue = minValue;
                            rightProp.floatValue = maxValue;
                        }

                        EditorGUI.BeginChangeCheck();
                        EditorGUILayout.PropertyField(leftProp, GUIContent.none, GUILayout.Width(40));
                        EditorGUILayout.PropertyField(rightProp, GUIContent.none, GUILayout.Width(40));
                        if (EditorGUI.EndChangeCheck())
                        {
                            leftProp.floatValue = Mathf.Clamp01(leftProp.floatValue);
                            rightProp.floatValue = Mathf.Clamp01(rightProp.floatValue);
                        }


                    }


                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(item.FindPropertyRelative("fillEmpty"), new GUIContent("Apply outside scopes"));
                    EditorGUI.indentLevel--;
                }
                if (item.FindPropertyRelative("overrideCurve").boolValue)
                {
                    EditorGUILayout.PropertyField(item.FindPropertyRelative("curve"), new GUIContent("Curve"));
                }
            }
        }

        /// <summary>
        /// Event layout in bottom
        /// </summary>
        private void DrawEventLayout()
        {
            if (tweenList.arraySize == 0) return;

            if (animTypeProp.enumValueIndex == 0)
            {
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(onFinishProp);
            }
            else
            {
                EditorGUILayout.LabelField(new GUIContent("Finish event is not available on loop animations", "Cause it's endless, you know?"), EditorStyles.helpBox);
            }
        }

        /// <summary>
        /// Adding tween
        /// </summary>
        private void AddTween(object type)
        {
            tweenList.arraySize++;
            SerializedProperty newTweenProp = tweenList.GetArrayElementAtIndex(tweenList.arraySize - 1);

            newTweenProp.FindPropertyRelative("type").enumValueIndex = (int)type;
            newTweenProp.FindPropertyRelative("tag").stringValue = "";
            newTweenProp.FindPropertyRelative("overrideTarget").boolValue = false;
            newTweenProp.FindPropertyRelative("overrideCurve").boolValue = false;
            newTweenProp.FindPropertyRelative("overrideScopes").boolValue = false;
            newTweenProp.FindPropertyRelative("addMode").boolValue = false;

            newTweenProp.FindPropertyRelative("target").objectReferenceValue = null;
            newTweenProp.FindPropertyRelative("curve").animationCurveValue = AnimationCurve.Linear(0, 0, 1, 1);
            newTweenProp.FindPropertyRelative("leftEdge").floatValue = 0;
            newTweenProp.FindPropertyRelative("rightEdge").floatValue = 1f;
            newTweenProp.FindPropertyRelative("fillEmpty").boolValue = false;

            tweenList.serializedObject.ApplyModifiedProperties();
            SetTweenValues(newTweenProp);
        }


        /// <summary>
        /// Set values to tween based on target object
        /// </summary>
        private void SetTweenValues(SerializedProperty element, bool setFrom = true, bool setTo = true)
        {
            SerializedProperty typeProp = element.FindPropertyRelative("type");

            SerializedProperty fromVector3 = element.FindPropertyRelative("fromVector3");
            SerializedProperty toVector3 = element.FindPropertyRelative("toVector3");
            SerializedProperty fromFloat = element.FindPropertyRelative("fromFloat");
            SerializedProperty toFloat = element.FindPropertyRelative("toFloat");
            SerializedProperty fromColor = element.FindPropertyRelative("fromColor");
            SerializedProperty toColor = element.FindPropertyRelative("toColor");

            Tween type = (Tween)typeProp.enumValueIndex;
            Transform target = T.Target != null ? T.Target : T.transform;
            RectTransform rectTransform = null;

            switch (type)
            {
                case Tween.Position:
                    if (setFrom) fromVector3.vector3Value = target.localPosition;
                    if (setTo) toVector3.vector3Value = target.localPosition;
                    break;
                case Tween.Rotation:
                    if (setFrom) fromVector3.vector3Value = target.localEulerAngles;
                    if (setTo) toVector3.vector3Value = target.localEulerAngles;
                    break;
                case Tween.Scale:
                    if (setFrom) fromVector3.vector3Value = target.localScale;
                    if (setTo) toVector3.vector3Value = target.localScale;
                    break;
                case Tween.Width:
                    rectTransform = target.GetComponent<RectTransform>();
                    if (rectTransform)
                    {
                        if (setFrom) fromFloat.floatValue = rectTransform.rect.width;
                        if (setTo) toFloat.floatValue = rectTransform.rect.width;
                    }
                    break;
                case Tween.Height:
                    rectTransform = target.GetComponent<RectTransform>();
                    if (rectTransform)
                    {
                        if (setFrom) fromFloat.floatValue = rectTransform.rect.height;
                        if (setTo) toFloat.floatValue = rectTransform.rect.height;
                    }
                    break;
                case Tween.Color:
                    fromColor.colorValue = Color.white;
                    toColor.colorValue = Color.white;
                    break;
                case Tween.Alpha:
                    fromFloat.floatValue = 0;
                    toFloat.floatValue = 1;
                    break;
            }


            element.serializedObject.ApplyModifiedProperties();
        }


        /// <summary>
        /// Context menu when right click on list element
        /// </summary>
        private void RightClickOnItem(int index)
        {
            SerializedProperty element = tweenList.GetArrayElementAtIndex(index);
            SerializedProperty overrideTargetProp = element.FindPropertyRelative("overrideTarget");
            SerializedProperty overrideScopesProp = element.FindPropertyRelative("overrideScopes");
            SerializedProperty overrideCurveProp = element.FindPropertyRelative("overrideCurve");
            SerializedProperty addModeProp = element.FindPropertyRelative("addMode");

            Tween type = GetElementType(element);

            GenericMenu menu = new GenericMenu();

            // Удаление твина
            menu.AddItem(new GUIContent("Remove Tween"), false, () =>
            {
                tweenList.DeleteArrayElementAtIndex(index);
                tweenList.serializedObject.ApplyModifiedProperties();
            });

            menu.AddSeparator("");

            // Кнопки перемещения твина вверх и вниз
            bool isFirst = index == 0;
            bool isLast = index == tweenList.arraySize - 1;
            GUIContent moveUpCont = new GUIContent("Move Up");
            GUIContent moveDownCont = new GUIContent("Move Down");
            // Перемещение вверх
            if (!isFirst) menu.AddItem(moveUpCont, false, () =>
            {
                tweenList.MoveArrayElement(index, index - 1);
                tweenList.serializedObject.ApplyModifiedProperties();
            });
            else menu.AddDisabledItem(moveUpCont);
            // Перемещение вниз
            if (!isLast) menu.AddItem(moveDownCont, false, () =>
            {
                tweenList.MoveArrayElement(index, index + 1);
                tweenList.serializedObject.ApplyModifiedProperties();
            });
            else menu.AddDisabledItem(moveDownCont);

            menu.AddSeparator("");


            // Добавление или Редактирование тега
            GUIContent editCont = new GUIContent("Add Tag");
            if (!string.IsNullOrEmpty(element.FindPropertyRelative("tag").stringValue))
            {
                editCont.text = "Edit Tag";
            }
            menu.AddItem(new GUIContent("Override Target"), overrideTargetProp.boolValue, () =>
            {
                overrideTargetProp.boolValue = !overrideTargetProp.boolValue;
                element.serializedObject.ApplyModifiedProperties();
            });


            // Кнопки расширения полей
            menu.AddItem(new GUIContent("Override Scopes"), overrideScopesProp.boolValue, () =>
            {
                overrideScopesProp.boolValue = !overrideScopesProp.boolValue;
                element.serializedObject.ApplyModifiedProperties();
            });

            menu.AddItem(new GUIContent("Override Curve"), overrideCurveProp.boolValue, () =>
            {
                overrideCurveProp.boolValue = !overrideCurveProp.boolValue;
                element.serializedObject.ApplyModifiedProperties();
            });

            menu.AddItem(new GUIContent("Override Curve"), overrideCurveProp.boolValue, () =>
            {
                overrideCurveProp.boolValue = !overrideCurveProp.boolValue;
                element.serializedObject.ApplyModifiedProperties();
            });

            if (TweensConfig.tweensTable[type].hasAdditionMode)
            {
                menu.AddItem(new GUIContent("Addition Mode", "Тултип"), addModeProp.boolValue, () =>
                {
                    addModeProp.boolValue = !addModeProp.boolValue;
                    element.serializedObject.ApplyModifiedProperties();
                });
            }

            menu.AddSeparator("");

            menu.AddItem(editCont, false, () => elementTagEditing = index);
            // Засетать значения твина из контекстного меню
            GUIContent setFromCont = new GUIContent("Set \"From\"");
            GUIContent setToCont = new GUIContent("Set \"To\"");

            bool couldBeSet = TweensConfig.tweensTable[type].valuesCouldBeSetFromContext;

            if (couldBeSet) menu.AddItem(setFromCont, false, () => SetTweenValues(element, true, false));

            if (couldBeSet) menu.AddItem(setToCont, false, () => SetTweenValues(element, false, true));

            menu.ShowAsContext();
        }


        private Tween GetElementType(SerializedProperty element)
        {
            SerializedProperty typeProp = element.FindPropertyRelative("type");
            Tween type = (Tween)typeProp.enumValueIndex;
            return type;
        }


        public static class BroUtilites
        {
            public static bool IsPropertyOverrided(SerializedProperty prop)
            {
                return prop.isInstantiatedPrefab && prop.prefabOverride;
            }

            public static bool GetKeyUp(KeyCode code)
            {
                return Event.current.type == EventType.KeyUp && Event.current.keyCode == code;
            }

            public static void DrawBlueOverrideMark(Rect rect)
            {
                Color baseColor = GUI.color;
                GUI.color = new Color(0.05f, 0.61f, 0.91f);
                Rect boxRect = new Rect(0, rect.y, 2, rect.height);
                GUI.Box(boxRect, GUIContent.none, Defaults.SolidWhite);
                GUI.color = baseColor;
            }


            public class FixWideScope : IDisposable
            {
                bool currentWideMode;
                public float labelWidth;

                public FixWideScope()
                {
                    labelWidth = EditorGUIUtility.labelWidth;
                    currentWideMode = EditorGUIUtility.wideMode;

                    if (currentWideMode) return;
                    EditorGUIUtility.wideMode = true;
                    labelWidth = EditorGUIUtility.currentViewWidth - 220;
                    EditorGUIUtility.labelWidth = labelWidth;
                }

                public void Dispose()
                {
                    if (currentWideMode) return;
                    EditorGUIUtility.wideMode = false;
                    EditorGUIUtility.labelWidth = 0;
                }
            }
        }



        public static class Defaults
        {
            public static GUIStyle SolidWhite = new GUIStyle
            {
                normal = { background = Texture2D.whiteTexture }
            };
            public static GUIStyle ElementBackgroundWhite = new GUIStyle(SolidWhite)
            {
                padding = new RectOffset(18, 4, 4, 4),
            };
            public static GUIStyle AdditionModeWarning = new GUIStyle
            {
                normal = { textColor = new Color(1f, 0.5f, 0) }
            };

            public static Color[] ElementBGColors = new Color[]
            {
                EditorGUIUtility.isProSkin? new Color(1f, 1f, 1f, 0.06f) : new Color(0.9f, 0.9f, 0.9f, 0.8f),
                Color.clear
            };
        }
    }
}