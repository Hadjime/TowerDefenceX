using System;
using System.Collections.Generic;
using System.Linq;
using GRV.ToolsModule;
using UnityEngine;


namespace InternalAssets.Scripts.Infrastructure.GameStateMachine
{
    public class GameStateMachine : IGameStateMachine
	{
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        
        public GameStateMachine(IEnumerable<IExitableState> states)
        {
            _states = states.ToDictionary(state => state.GetType());
        }
        
        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>
        {
            TState  state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
			CustomDebug.Log($"Exit {_activeState?.GetType().Name} state", Color.green);
			_activeState?.Exit();
            
            TState state = GetState<TState>();
            _activeState = state;
			CustomDebug.Log($"Enter {_activeState?.GetType().Name} state", Color.green);
			
            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState => 
            _states[typeof(TState)] as TState;
    }
}