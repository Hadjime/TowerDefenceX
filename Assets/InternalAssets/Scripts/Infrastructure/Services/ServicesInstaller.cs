using InternalAssets.Scripts.Infrastructure.AssetManagement;
using InternalAssets.Scripts.Infrastructure.Scene;
using InternalAssets.Scripts.Infrastructure.Services.StaticData;
using UnityEngine;
using Zenject;

namespace InternalAssets.Scripts.Infrastructure.Services
{
	[CreateAssetMenu(fileName = "ServicesInstaller", menuName = "Installers/ServicesInstaller")]
	public class ServicesInstaller : ScriptableObjectInstaller<ServicesInstaller>
	{
		public override void InstallBindings()
		{
			RegisterServices();
		}
		
		private void RegisterServices()
		{
			Container.BindInterfacesAndSelfTo<SceneLoader>().AsSingle();
			Container.BindInterfacesAndSelfTo<AssetsProvider>().AsSingle();
			Container.BindInterfacesAndSelfTo<StaticDataService>().AsSingle();
		}
	}
}