using UnityEngine;

namespace GRV.ToolsModule.BroTools
{
    /// <summary>
    /// Проигрывание другого твинера
    /// </summary>
    public class TweenLink : ITween
    {

        private BroTweener linkedTweener;

        public void ApplyValue(Transform target, TweenData data, float value)
        {
            if (linkedTweener != data.linkedTweener) linkedTweener = data.linkedTweener;

            if (linkedTweener)
            {
                linkedTweener.SetProgress(value);
            }
        }
    }
}