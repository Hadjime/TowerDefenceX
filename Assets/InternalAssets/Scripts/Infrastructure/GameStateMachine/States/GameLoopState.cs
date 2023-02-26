using Zenject;


namespace InternalAssets.Scripts.Infrastructure.GameStateMachine.States
{
    public class GameLoopState : IState
    {
        private readonly LazyInject<IGameStateMachine> _gameStateMachine;


		public GameLoopState(LazyInject<IGameStateMachine> gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

		public void Enter()
		{

		}

		public void Exit()
        {

        }
    }
}