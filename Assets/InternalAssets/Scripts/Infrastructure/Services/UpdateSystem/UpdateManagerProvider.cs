using System;
using System.Collections.Generic;

namespace InternalAssets.Scripts.Infrastructure.Services.UpdateSystem
{
    public class UpdateManagerProvider : IDisposable
    {
        private readonly IUpdateManager updateManager;
        private readonly HashSet<IBaseUpdatable> updatables;
        
        public UpdateManagerProvider(IUpdateManager updateManager, IEnumerable<IBaseUpdatable> updatables)
        {
            this.updateManager = updateManager;
            this.updatables = new HashSet<IBaseUpdatable>();
            RegisterUpdatables(updatables);
        }

        public void Dispose()
        {
            UnRegisterUpdatables();
        }

        private void RegisterUpdatables(IEnumerable<IBaseUpdatable> value)
        {
            foreach (var updatable in value)
            {
                if (updateManager.Add(updatable))
                {
                    updatables.Add(updatable);
                }
            }
        }

        private void UnRegisterUpdatables()
        {
            foreach (var updatable in updatables)
            {
                updateManager.Remove(updatable);
            }
        }
    }
}