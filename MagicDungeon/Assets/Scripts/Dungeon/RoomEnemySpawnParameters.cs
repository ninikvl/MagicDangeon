using UnityEngine;

[System.Serializable]
public class RoomEnemySpawnParameters
{
    #region Tooltip
    [Tooltip("Определяет уровень подземелья для этой комнаты в зависимости от того, сколько всего врагов должно быть порождено")]
    #endregion Tooltip
    public DungeonLevelSO dungeonLevel;
    #region Tooltip
    [Tooltip("Минимальное количество врагов, которые могут появиться в этой комнате для данного уровня подземелья.  Фактическое число будет случайным значением между минимальным и максимальным значениями")]
    #endregion Tooltip
    public int minTotalEnemiesToSpawn;
    #region Tooltip
    [Tooltip("Максимальное количество врагов, которые могут появиться в этой комнате для данного уровня подземелья.  Фактическое число будет случайным значением между минимальным и максимальным значениями.")]
    #endregion Tooltip
    public int maxTotalEnemiesToSpawn;
    #region Tooltip
    [Tooltip("Минимальное количество одновременно появляющихся врагов в этой комнате для данного уровня подземелья.  Фактическое число будет случайным значением между минимальным и максимальным значениями.")]
    #endregion Tooltip
    public int minConcurrentEnemies;
    #region Tooltip
    [Tooltip("Максимальное количество одновременно появляющихся врагов в этой комнате для данного уровня подземелья.  Фактическое число будет случайным значением между минимальным и максимальным значениями.")]
    #endregion Tooltip
    public int maxConcurrentEnemies;
    #region Tooltip
    [Tooltip("Минимальный интервал появления врагов в этой комнате для данного уровня подземелья в секундах.  Фактическое число будет случайным значением между минимальным и максимальным значениями.")]
    #endregion Tooltip
    public int minSpawnInterval;
    #region Tooltip
    [Tooltip("Максимальный интервал появления врагов в этой комнате для данного уровня подземелья в секундах.  Фактическое число будет случайным значением между минимальным и максимальным значениями.")]
    #endregion Tooltip
    public int maxSpawnInterval;
}