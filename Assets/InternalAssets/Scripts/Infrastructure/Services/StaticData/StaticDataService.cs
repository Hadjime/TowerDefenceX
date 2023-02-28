using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternalAssets.Scripts.Enemies;
using InternalAssets.Scripts.Infrastructure.AssetManagement;
using InternalAssets.Scripts.StaticData;

namespace InternalAssets.Scripts.Infrastructure.Services.StaticData
{
	public class StaticDataService : IStaticDataService
	{
		private const string MonstersLabel = "Monsters";
		private const string LevelsLabel = "Levels";

		private readonly IAssets _assets;
		private Dictionary<MonsterType, MonstersStaticData> _monsters = new Dictionary<MonsterType, MonstersStaticData>();
		private Dictionary<int, LevelStaticData> _levels = new Dictionary<int, LevelStaticData>();


		public StaticDataService(IAssets assets) 
		{
			_assets = assets;
		}

		public async Task WarmUp()
		{
			await LoadMonstersData();
			await LoadLevelsData();
		}

		public MonstersStaticData GetMonsterData(MonsterType monsterType) =>
			_monsters.TryGetValue(monsterType, out MonstersStaticData monstersStaticData) 
				? monstersStaticData 
				: null;

		public LevelStaticData GetLevelData(int numberLevel) => 
			_levels.TryGetValue(numberLevel, out LevelStaticData levelStaticData) 
				? levelStaticData 
				: null;

		private async Task LoadMonstersData()
		{
			List<MonstersStaticData> loadAllAsyncByLabel = await _assets.LoadAllAsyncByLabel<MonstersStaticData>(MonstersLabel);
			loadAllAsyncByLabel.ToDictionary(data => _monsters[data.MonsterType] = data);
			int monstersCount = _monsters.Count;
		}
		private async Task LoadLevelsData()
		{
			List<LevelStaticData> loadAllAsyncByLabel = await _assets.LoadAllAsyncByLabel<LevelStaticData>(LevelsLabel);
			loadAllAsyncByLabel.ToDictionary(data => _levels[data.NumberLevel] = data);
			int levelsCount = _levels.Count;
		}
	}
}
