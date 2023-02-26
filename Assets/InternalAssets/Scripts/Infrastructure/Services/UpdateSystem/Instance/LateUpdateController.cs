namespace InternalAssets.Scripts.Infrastructure.Services.UpdateSystem.Instance
{
	public sealed class LateUpdateController : UpdateController<ILateUpdatable>
	{
		protected override void UpdateProcessing(ILateUpdatable updatable) => updatable.LateUpdateProcessing();
	}
}