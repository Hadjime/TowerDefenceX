using InternalAssets.Scripts.Infrastructure.GameStateMachine.States;
using UnityEngine;
using Zenject;

namespace InternalAssets.Scripts.Infrastructure.GameStateMachine
{
    [CreateAssetMenu(fileName = "GameStateMachineInstaller", menuName = "Installers/GameStateMachineInstaller")]
    public class GameStateMachineInstaller : ScriptableObjectInstaller<GameStateMachineInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<GameStateMachine>().AsSingle();

            Container.BindInterfacesTo<BootstrapState>().AsSingle();
            Container.BindInterfacesTo<LoadSceneState>().AsSingle();
            Container.BindInterfacesTo<LoadProgressState>().AsSingle();
            Container.BindInterfacesTo<GameLoopState>().AsSingle();
        }
    }
}