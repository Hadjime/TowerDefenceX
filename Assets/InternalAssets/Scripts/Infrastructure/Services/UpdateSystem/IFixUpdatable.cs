namespace InternalAssets.Scripts.Infrastructure.Services.UpdateSystem
{
	public interface IFixUpdatable : IBaseUpdatable
	{
		void FixUpdateProcessing();
	}
}