using UnityEngine;

namespace GRV.ToolsModule.BroTools
{
    public class TweenScale: ITween
    {

        public void ApplyValue(Transform target, TweenData data, float value)
        {

            target.localScale = Vector3.LerpUnclamped(data.fromVector3, data.toVector3, value);
        }
    }
}