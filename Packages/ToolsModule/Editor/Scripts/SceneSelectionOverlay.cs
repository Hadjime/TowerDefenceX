#if UNITY_2021_1_OR_NEWER
using System.IO;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.SceneManagement;
using UnityEditor.Toolbars;
using UnityEngine;
using Scene = UnityEngine.SceneManagement.Scene;

namespace GRV.ToolsModule
{
    /// <summary>
    /// Панель инструментов для быстрого переключения между сценами в проекте
    /// </summary>
    [Overlay(typeof(SceneView), "Scene selection")]
    // [Icon(k_icon)]
    public class SceneSelectionOverlay: ToolbarOverlay
    {
        // public const string k_icon = "Assets/Editor/Icons/UnityIcon.png";

        SceneSelectionOverlay() : base(SceneDropdownToggle.k_id) { }
        
        [EditorToolbarElement(k_id, typeof(SceneView))]
        class SceneDropdownToggle : EditorToolbarDropdownToggle, IAccessContainerWindow
        {
            public const string k_id = "SceneSelectionOverlay/SceneDropdownToggle";
            public EditorWindow containerWindow { get; set; }

            SceneDropdownToggle()
            {
                text = "Scenes";
                tooltip = "Select a scene to load";
                // icon = AssetDatabase.LoadAssetAtPath<Texture2D>(k_icon);
                
                dropdownClicked += ShowSceneOnlyBuildSettings;
            }

            /// <summary>
            /// Показать в выпадающем списке все сцены в проекте
            /// </summary>
            private void ShowAllSceneInProject()
            {
                GenericMenu menu = new GenericMenu();

                Scene currentScene = EditorSceneManager.GetActiveScene();

                string[] sceneGuids = AssetDatabase.FindAssets("t:scene", null);

                for (int i = 0; i < sceneGuids.Length; i++)
                {
                    string path = AssetDatabase.GUIDToAssetPath(sceneGuids[i]);
                    string name = Path.GetFileNameWithoutExtension(path);
                    menu.AddItem(new GUIContent(name), string.Compare(currentScene.name, name) == 0, () => OpenScene(currentScene, path));
                }
                
                menu.ShowAsContext();
            }
            
            /// <summary>
            /// Показать в выпадающем списке сцены попадающие в сборку
            /// </summary>
            private void ShowSceneOnlyBuildSettings()
            {
                GenericMenu menu = new GenericMenu();

                Scene currentScene = EditorSceneManager.GetActiveScene();

                EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;

                foreach (EditorBuildSettingsScene scene in scenes)
                {
                    string path = scene.path;
                    string name = Path.GetFileNameWithoutExtension(path);
                    menu.AddItem(new GUIContent(name), string.Compare(currentScene.name, name) == 0, () => OpenScene(currentScene, path));  
                }

                menu.ShowAsContext();
            }

            private void OpenScene(Scene currentScene, string path)
            {
                if (currentScene.isDirty)
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                        EditorSceneManager.OpenScene(path);
                }
                else
                {
                    EditorSceneManager.OpenScene(path);
                }
            }
        }
    }
}
#endif