namespace InternalAssets.Scripts.Infrastructure.Services.UpdateSystem.Instance
{
	public sealed class FixUpdateController : UpdateController<IFixUpdatable>
	{
		protected override void UpdateProcessing(IFixUpdatable updatable) => updatable.FixUpdateProcessing();
	}
}