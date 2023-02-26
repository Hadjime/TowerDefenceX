using System.Threading.Tasks;
using InternalAssets.Scripts.Infrastructure.Scene;
using InternalAssets.Scripts.Infrastructure.Services.StaticData;
using Zenject;


namespace InternalAssets.Scripts.Infrastructure.GameStateMachine.States
{
	public class BootstrapState : IState
	{
		private const string InitialSceneName = "Initial";


		private readonly LazyInject<IGameStateMachine> _gameStateMachine;
		private readonly SceneLoader _sceneLoader;
		private IStaticDataService _staticDataService;


		public BootstrapState(LazyInject<IGameStateMachine> gameStateMachine, SceneLoader sceneLoader, IStaticDataService staticDataService)
		{
			_gameStateMachine = gameStateMachine;
			_sceneLoader = sceneLoader;
			_staticDataService = staticDataService;
		}

		public void Enter()
		{
			Work();
		}


		public void Exit()
		{

		}

		private async Task Work()
		{
			await _staticDataService.WarmUp();
			_sceneLoader.Load(InitialSceneName, onLoaded: EnterInLoadLevel);
		}

		private void EnterInLoadLevel() =>
			_gameStateMachine.Value.Enter<LoadProgressState>();
	}
}