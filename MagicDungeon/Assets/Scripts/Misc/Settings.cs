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

    #region Animator settings

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

    #endregion
}
