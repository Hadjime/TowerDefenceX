using System;
using System.Collections.Generic;
using InternalAssets.Scripts.Infrastructure.Services.UpdateSystem.Instance;
using UnityEngine;

namespace InternalAssets.Scripts.Infrastructure.Services.UpdateSystem
{
	public class UpdateManager : MonoBehaviour, IUpdateManager
	{
		private readonly HashSet<IUpdateController> updateControllers = new HashSet<IUpdateController>();
		private readonly IUpdateController defaultUpdateController = new DefaultUpdateController();
		private readonly IUpdateController fixUpdateController = new FixUpdateController();
		private readonly IUpdateController lateUpdateController = new LateUpdateController();

		public bool Add(IBaseUpdatable value)
		{
			Type updatableType = value.GetType();
			bool result = false;
			foreach (var controller in updateControllers)
			{
				if (controller.ServicedUpdatableType.IsAssignableFrom(updatableType))
				{
					result |= controller.Register(value);
				}
			}

			return result;
		}

		public bool Remove(IBaseUpdatable value)
		{
			bool result = false;
			foreach (var controller in updateControllers)
			{
				result |= controller.UnRegister(value);
			}

			return result;
		}

		private void FixedUpdate()
		{
			fixUpdateController.Processing();
		}

		private void Update()
		{
			defaultUpdateController.Processing();
		}

		private void LateUpdate()
		{
			lateUpdateController.Processing();
		}

		private void Awake()
		{
			updateControllers.Add(lateUpdateController);
			updateControllers.Add(fixUpdateController);
			updateControllers.Add(defaultUpdateController);
		}
	}
}