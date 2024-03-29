using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Orientation
{
    north,
    south,
    east,
    west,
    none
}

public enum GameState
{
    gameStarted,
    playingLevel,
    engaginEnemies,
    bossStage,
    engaginBoss,
    levelComplited,
    gameWon,
    gameLost,
    gamePaused,
    dungeonOverviewMap,
    restartGame
}

public enum AimDirection
{
    Up,
    UpRight,
    UpLeft,
    Right,
    Left,
    Down
}

public enum ChestSpawnEvent
{
    onRoomEntry,
    onEnemiesDefeated
}

public enum ChestSpawnPosition
{
    atSpawnerPosition,
    atPlayerPosition
}

public enum ChestState
{
    closed,
    healthItem,
    ammoItem,
    weaponItem,
    empty
}

