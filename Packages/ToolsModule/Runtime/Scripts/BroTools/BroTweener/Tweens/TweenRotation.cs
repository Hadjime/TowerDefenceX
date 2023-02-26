using UnityEngine;

namespace GRV.ToolsModule.BroTools
{
    public class TweenRotation : ITween
    {

        public void ApplyValue(Transform target, TweenData data, float value)
        {
            target.localEulerAngles = Vector3.SlerpUnclamped(data.fromVector3, data.toVector3, value);
        }
    }
}