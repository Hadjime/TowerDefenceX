namespace InternalAssets.Scripts.Infrastructure.Services.UpdateSystem
{
	public interface ILateUpdatable : IBaseUpdatable
	{
		void LateUpdateProcessing();
	}
}