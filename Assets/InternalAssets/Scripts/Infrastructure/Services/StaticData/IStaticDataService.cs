using System.Threading.Tasks;
using InternalAssets.Scripts.Enemies;
using InternalAssets.Scripts.StaticData;

namespace InternalAssets.Scripts.Infrastructure.Services.StaticData
{
	public interface IStaticDataService: IService
	{
		MonstersStaticData GetMonsterData(MonsterType monsterType);
		Task WarmUp();
		LevelStaticData GetLevelData(int numberLevel);
	}
}
