using Zenject;


namespace InternalAssets.Scripts.Infrastructure.GameStateMachine.States
{
    public class LoadProgressState : IState
    {
        private const string MAIN_SCENE = "Start";
        private readonly LazyInject<IGameStateMachine> _gameStateMachine;


        public LoadProgressState(LazyInject<IGameStateMachine> gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
            //TODO: прокинуть сервисы необходимые для инициализации прогресса игрока
        }

        public void Enter()
        {
            //TODO: Здесь проинициализировать или подгрузить прогресс игрока
            _gameStateMachine.Value.Enter<LoadSceneState, string>(MAIN_SCENE);
        }

        public void Exit()
        {
            
        }
    }
}