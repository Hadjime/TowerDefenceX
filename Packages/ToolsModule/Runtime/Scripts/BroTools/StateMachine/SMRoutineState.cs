using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GRV.ToolsModule.BroTools
{

    public abstract class SMRoutineState
    {
        public StateMachineRoutine Machine { get; set; }
        private List<Coroutine> _runningCoroutines = new List<Coroutine>();

        public virtual IEnumerator Enter() { yield break; }
        public virtual void Update() { }
        public virtual IEnumerator Exit() { yield break; }

        public Coroutine StartCoroutine(IEnumerator enumerator)
        {
            var newCoroutine = Machine.initiator.StartCoroutine(enumerator);
            _runningCoroutines.Add(newCoroutine);
            return newCoroutine;
        }
        public void StopCoroutine(IEnumerator enumerator)
        {
            Machine.initiator.StopCoroutine(enumerator);
        }
        public void StopCoroutine(Coroutine routine)
        {
            Machine.initiator.StopCoroutine(routine);
        }

        public void StopAllCoroutines()
        {
            foreach (var coroutine in _runningCoroutines) if (coroutine != null) Machine.initiator.StopCoroutine(coroutine);
            _runningCoroutines.Clear();
        }
    }
}