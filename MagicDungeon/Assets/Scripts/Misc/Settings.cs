using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    #region Dungeon build settings

    public const int maxDungeonRebuildAttemptsForRoomGraph = 100;
    public const int maxDungeonRebuildAttempts = 10;

    #endregion


    #region Room settings

    //Максимальное количество дочерних узлов у комнаты
    public const int maxChildCorridors = 3;

    #endregion
}
