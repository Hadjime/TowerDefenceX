using UnityEngine;

namespace GRV.ToolsModule.BroTools
{

    public class TweenAction : ITween
    {
        private TweenData.ProcessEvent action;

        public void ApplyValue(Transform target, TweenData data, float value)
        {
            data.processEvent?.Invoke(value);
        }
    }
}