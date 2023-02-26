using System.Threading.Tasks;
using InternalAssets.Scripts.Data;
using InternalAssets.Scripts.GamePlay.Towers;
using InternalAssets.Scripts.GamePlay.Weapons;
using InternalAssets.Scripts.Infrastructure.Scene;
using InternalAssets.Scripts.Infrastructure.Services.StaticData;
using Zenject;


namespace InternalAssets.Scripts.Infrastructure.GameStateMachine.States
{
    public class LoadSceneState : IPayloadState<string>
    {
	    private const int NUMBER_LEVEL = 1;
	    
	    
	    private readonly LazyInject<IGameStateMachine> _gameStateMachine;
	    private readonly SceneLoader _sceneLoader;
	    private GroundTower.Factory _groundTowerFactory;
        private Weapon.Factory _weaponFactory;
        private IStaticDataService _staticDataService;


        public LoadSceneState(
				LazyInject<IGameStateMachine> gameStateMachine,
				SceneLoader sceneLoader,
				GroundTower.Factory groundTowerFactory,
				Weapon.Factory weaponFactory,
				IStaticDataService staticDataService
		)
        {
	        _gameStateMachine = gameStateMachine;
	        _sceneLoader = sceneLoader;
	        _groundTowerFactory = groundTowerFactory;
	        _weaponFactory = weaponFactory;
	        _staticDataService = staticDataService;
        }

        public void Enter(string sceneName)
        {
            //TODO: возможно что-то в асинхронном режиме начать загружать(прогревать) здесь
            _sceneLoader.Load(sceneName, OnLoaded);
        }

        public void Exit()
        {
            
        }

        private async void OnLoaded()
		{
			await InitGameWorld();
		}
        


		private async Task InitGameWorld()
		{
			foreach (TowerSpawnerData towerSpawnerData in _staticDataService.GetLevelData(NUMBER_LEVEL).TowerSpawners)
			{
				GroundTower tower = _groundTowerFactory.Create();
				tower.gameObject.transform.position = towerSpawnerData.Position;
				
				Weapon weapon = _weaponFactory.Create();
				tower.SetWeaponInPlace(weapon);
			}
		}
    }
}