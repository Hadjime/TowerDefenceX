using UnityEngine;
using UnityEngine.UI;

namespace GRV.ToolsModule.BroTools
{

    public class UIManager : MonoBehaviourSingleton<UIManager>
    {
        //public static Rect CanvasRect => ((RectTransform)Canvas.transform).rect;
        //public static Vector2 UIScaleFactor => CanvasRect.size / CanvasScaler.referenceResolution;
        //public static Vector2 UIScaleFactorInverted => CanvasScaler.referenceResolution / CanvasRect.size;

        [SerializeField] private UILayer sceneLayer;
        [SerializeField] private UILayer mainLayer;
        [SerializeField] private UILayer coverLayer;
        [SerializeField] private UILayer uiVfxLayer;
        [SerializeField] private UILayer notificationLayer;

        private Canvas canvas;

        public static UILayer SceneLayer => Instance.sceneLayer;
        public static UILayer MainLayer => Instance.mainLayer;
        public static UILayer CoverLayer => Instance.coverLayer;
        public static UILayer UIVFXLayer => Instance.uiVfxLayer;
        public static UILayer NotificationLayer => Instance.notificationLayer;

        public static Canvas Canvas => Instance.canvas ?? (Instance.canvas = Instance.GetComponent<Canvas>());
        public static Rect CanvasRect => ((RectTransform)Canvas.transform).rect;

        private CanvasScaler _canvasScaler;
        public CanvasScaler CanvasScaler
        {
            get => _canvasScaler ?? (_canvasScaler = Canvas.GetComponentInParent<CanvasScaler>());
        }

        private IUIElementsProvider elementsProvider;

        public IUIElementsProvider ElementsProvider => elementsProvider ??= new UIElementsAddressablesProvider();
    }
}