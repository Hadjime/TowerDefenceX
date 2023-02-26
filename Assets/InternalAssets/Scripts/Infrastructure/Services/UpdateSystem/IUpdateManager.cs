namespace InternalAssets.Scripts.Infrastructure.Services.UpdateSystem
{
    public interface IUpdateManager
    {
        bool Add(IBaseUpdatable value);
        bool Remove(IBaseUpdatable value);
    }
}