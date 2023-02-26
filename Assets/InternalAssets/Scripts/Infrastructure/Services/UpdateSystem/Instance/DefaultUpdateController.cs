namespace InternalAssets.Scripts.Infrastructure.Services.UpdateSystem.Instance
{
	public sealed class DefaultUpdateController : UpdateController<IUpdatable>
	{
		protected override void UpdateProcessing(IUpdatable updatable) => updatable.UpdateProcessing();
	}
}