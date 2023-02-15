
using System.Collections.Generic;
using UnityEngine;

using Utils;

[CreateAssetMenu(fileName = "LevelHolder", menuName = "ScriptableObjects/LevelHolder", order = 1)]
public class LevelHolder : ScriptableObject
{
    [SerializeField]
    private List<Level> levels = new List<Level>();

    public Level GetLevel(int levelId)
    {
        Level level = levels.Find((l) => l.id == levelId);
        if (level == null) throw new GameException($"No such level: {levelId}");
        return level;
    }

}
