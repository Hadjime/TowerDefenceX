using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GRV.ToolsModule.BroTools
{
    public class UIElementsAddressablesProvider : IUIElementsProvider
    {
        public const string UIELEMENT_KEY = "UIElement";

        private readonly Dictionary<UIKey, UIElement> elements = new Dictionary<UIKey, UIElement>();

        private AsyncOperationHandle handle;

        public IEnumerable<UIElement> GetElements(UIKey key) => elements.Where(x => x.Key.Equals(key)).Select(x => x.Value);

        public IEnumerator Init(Action<float> progress = null)
        {
            handle = Addressables.LoadAssetsAsync<GameObject>(UIELEMENT_KEY, SetElement);
            while (!handle.IsDone)
            {
                progress?.Invoke(handle.PercentComplete);
                yield return null;
            }
            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                throw new Exception("UIElements provider initialization is fail");
            }            
        }

        private void SetElement(GameObject go)
        {
            var element = go.GetComponent<UIElement>();
            if (element != null)
            {                
                elements[element.Key] = element;
            }
            else
            {
                Debug.LogWarning($"GameObject {go.name} doesn't contain component UIElement. You shouldn't mark game objects without component UIElement in addressables by key {UIELEMENT_KEY}");
            }
        }
    }
}
