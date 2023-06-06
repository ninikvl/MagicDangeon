using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    #region Units
    public const float pixelPerUnit = 16f;
    public const float tileSizePixel = 16f;
    #endregion

    #region Dungeon build settings
    public const int maxDungeonRebuildAttemptsForRoomGraph = 100;
    public const int maxDungeonRebuildAttempts = 10;
    #endregion

    #region Room settings
    //Максимальное количество дочерних узлов у комнаты
    public const int maxChildCorridors = 3;

    public const float fadeInTime = 0.5f;
    #endregion

    #region Animator settings
    //Параметры игрока
    public static int aimUp = Animator.StringToHash("aimUP");
    public static int aimDown = Animator.StringToHash("aimDown");
    public static int aimUpRight = Animator.StringToHash("aimUPRight");
    public static int aimUpLeft = Animator.StringToHash("aimUPLeft");
    public static int aimRight = Animator.StringToHash("aimRight");
    public static int aimLeft = Animator.StringToHash("aimLeft");
    public static int isStay = Animator.StringToHash("isStay");
    public static int isMoving = Animator.StringToHash("isMoving");
    public static int BlinkUP = Animator.StringToHash("BlinkUP");
    public static int BlinkDown = Animator.StringToHash("BlinkDown");
    public static int BlinkLeft = Animator.StringToHash("BlinkLeft");
    public static int BlinkRight = Animator.StringToHash("BlinkRight");

    //Параметры двери
    public static int open = Animator.StringToHash("open");

    //Базовая скорость анимации 
    public static float baseSpeedForPlayerAnimations = 8f;
    #endregion

    #region GameObects Tags
    public const string playerTag = "Player";
    public const string playerWeapon = "playerWeapon";
    #endregion

    #region Firing Control
    public const float useAimAngleDistance = 3.5f;
    #endregion

    #region UI PARAMETERS
    public const float uiHeartSpacing = 16f;
    public const float uiAmmoIconSpacing = 5f;
    #endregion

    #region ASTAR PATHFINDING PARAMETERS
    public const int defaultAStarMovementPenalty = 40;
    public const int preferredPathAStarMovementPenalty = 1;
    //public const int targetFrameRateToSpreadPathfindingOver = 60;
    public const float playerMoveDistanceToRebuildPath = 3f;
    public const float enemyPathRebuildCooldown = 2f;

    #endregion
}
