using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//возможность серилизации системой серилизации Unity
[System.Serializable] 
public class Doorway
{
    #region Header
    [Header("Основные параметры дверного проёма")]
    #endregion
    public Vector2Int position;
    public Orientation orientation;
    public GameObject doorPrefab;

    #region Header
    [Header("Верхняя левая позиция откуда копируется стена для закрытия прохода")]
    #endregion
    public Vector2Int doorwayStartCopyPos;
    #region Header
    [Header("Ширина тайлов дверного проёма для копирования")]
    #endregion
    public int doorwayCopyTileWidth;
    #region Header
    [Header("Высота тайлов дверного проёма для копирования")]
    #endregion
    public int doorwayCopyTileHeight;

    [HideInInspector]
    public bool isConnected = false;
    [HideInInspector]
    public bool isUnavailable = false;

}
