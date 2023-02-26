using System;

namespace InternalAssets.Scripts.Infrastructure.Scene
{
    public interface ISceneLoaderProvider
    {
        void Load(string name, Action onLoaded = null);
    }
}