namespace InternalAssets.Scripts.Infrastructure.GameStateMachine
{
    public interface IExitableState
    {
        void Exit();
    }
    public interface IState : IExitableState
    {
        void Enter();
    }
    
    public interface IPayloadState<TPayload> : IExitableState
    {
        void Enter(TPayload payload);
    }
}