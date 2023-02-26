using System;

namespace InternalAssets.Scripts.Infrastructure.Services.UpdateSystem
{
	public interface IUpdateController
	{
		Type ServicedUpdatableType { get; }

		bool Register(IBaseUpdatable value);
		bool UnRegister(IBaseUpdatable value);

		void Processing();
	}
}