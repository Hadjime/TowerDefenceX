using UnityEngine;

namespace GRV.ToolsModule.BroTools
{

    public interface ITween
    {
        void ApplyValue(Transform target, TweenData data, float value);
    }
}