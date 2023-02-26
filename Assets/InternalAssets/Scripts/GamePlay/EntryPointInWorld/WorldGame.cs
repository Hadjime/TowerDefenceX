using InternalAssets.Scripts.Infrastructure.GameStateMachine;
using InternalAssets.Scripts.Infrastructure.GameStateMachine.States;
using UnityEngine;
using Zenject;

namespace InternalAssets.Scripts.GamePlay.EntryPointInWorld
{
    public class WorldGame: IWorldGame, IInitializable
    {
        private IGameStateMachine _gameStateMachine;


        public WorldGame(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }
        
        public void Initialize()
        {
            Application.targetFrameRate = 30;
            
            _gameStateMachine.Enter<BootstrapState>();
        }
    }
}