using UnityEngine;
using System.Collections.Generic;

namespace GRV.ToolsModule.BroTools
{
    [System.Serializable]
    public class TweenData
    {
        [SerializeField]
        private Tween type;
        public Tween Type => type;

        public string tag = "";

        public bool addMode = false;

        public Vector3 fromVector3;
        public Vector3 toVector3;

        public Color32 fromColor = Color.white;
        public Color32 toColor = Color.white;

        public float fromFloat;
        public float toFloat;

        public float leftEdge = 0;
        public float rightEdge = 1;

        public Transform target;
        public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
        public BroTweener linkedTweener;
        public ProcessEvent processEvent;

        public bool overrideTarget;
        public bool overrideScopes;
        public bool overrideCurve;

        public bool fillEmpty;

        [System.Serializable]
        public class ProcessEvent : UnityEngine.Events.UnityEvent<float> { }

        public TweenData(Tween type)
        {
            this.type = type;
        }

        public override int GetHashCode()
        {
            var hashCode = 880131741;
            hashCode = hashCode * -1521134295 + type.GetHashCode();
            if (overrideTarget && target != null) hashCode = hashCode * -1521134295 + EqualityComparer<Transform>.Default.GetHashCode(target);
            return hashCode;
        }
    }
}