using System.Collections.Generic;
using InternalAssets.Scripts.Enemies;

namespace InternalAssets.Scripts.GamePlay.MonstersSpawner
{
    public interface IMonstersSpawner
    {
        IEnumerable<Monster> GetActiveMonsters();
        void ReturnMonster(Monster monster);
    }
}