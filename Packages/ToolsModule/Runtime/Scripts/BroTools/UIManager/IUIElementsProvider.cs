using System;
using System.Collections;
using System.Collections.Generic;

namespace GRV.ToolsModule.BroTools
{
    public interface IUIElementsProvider
    {
        IEnumerable<UIElement> GetElements(UIKey key);
        IEnumerator Init(Action<float> progress = null);
    }
}
