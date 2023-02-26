using System;
using System.Collections;
using UnityEngine;

namespace GRV.ToolsModule.BroTools
{

    public class StateMachineRoutine
    {
        public event Action<SMRoutineState> OnStateUpdated;
        public MonoBehaviour initiator;

        private string name;
        private Coroutine routine;
        private SMRoutineState nextState;
        private bool isRun = true;

        public SMRoutineState CurrentState { get; private set; }

        public StateMachineRoutine(string name, MonoBehaviour initiator)
        {
            this.name = name;
            this.initiator = initiator;
            this.routine = initiator.StartCoroutine(Routine());
        }

        /// <summary>
        /// Остановить машину
        /// </summary>
        public IEnumerator Stop()
        {
            isRun = false;
            yield return routine;
        }


        /// <summary>
        /// Выйти из существующего состояния и запустить новое
        /// </summary>
        public SMRoutineState SetState(SMRoutineState state)
        {
            state.Machine = this;
            nextState = state;
            return nextState;
        }



        public IEnumerator Routine()
        {
            while (isRun)
            {
                if (nextState == null)
                {
                    yield return 0;
                    continue;
                }

                CurrentState = nextState;
                nextState = null;

                OnStateUpdated?.Invoke(CurrentState);
#if UNITY_EDITOR
                Debug.Log($"StateMachine <b>{name}</b> set to <color=white>{CurrentState}</color>");
#endif

                yield return CurrentState.Enter();

                while (isRun && nextState == null)
                {
                    CurrentState.Update();
                    yield return 0;
                }

                yield return CurrentState.Exit();
                CurrentState.StopAllCoroutines();

                CurrentState = null;
            }
        }
    }
}