using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameResources : MonoBehaviour
{
    private static GameResources instance;

    public static GameResources Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<GameResources>("GameResources");
            }
            return instance;
        }
    }

    #region Header DUNGEON
    [Space(10)]
    [Header("DUNGEON")]
    #endregion
    #region Tooltip
    [Tooltip("�������� � ������� ������� RoomNodeTypeListSO")]
    #endregion
    public RoomNodeTypeListSO roomNodeTypeList;

    #region Header PLAYER
    [Space(10)]
    [Header("�����")]
    #endregion
    #region Tooltip
    [Tooltip("SO ������ ������������ �� �����")]
    #endregion
    public CurrentPlayerSO currentPlayer;

    #region Header Materials
    [Space(10)]
    [Header("���������")]
    #endregion
    #region Tooltip
    [Tooltip("���������� ��������")]
    #endregion
    public Material dimmedMaterial;
    public Material litMaterial;
    public Material variableLitShader;
    #region Tooltip
    [Tooltip("Populate with the Materialize Shader")]
    #endregion
    public Shader materializeShader;

    #region Header Door Boss Prefab
    [Space(10)]
    [Header("������� ������ �����")]
    #endregion
    public GameObject doorNSBossPrefab;
    public GameObject doorEWBossPrefab;

    #region Header CHESTS
    [Space(10)]
    [Header("CHESTS")]
    #endregion
    #region Tooltip
    [Tooltip("Chest item prefab")]
    #endregion
    public GameObject chestItemPrefab;
    #region Tooltip
    [Tooltip("Populate with heart icon sprite")]
    #endregion
    public Sprite heartIcon;
    #region Tooltip
    [Tooltip("Populate with bullet icon sprite")]
    #endregion
    public Sprite bulletIcon;

    #region Header UI
    [Space(10)]
    [Header("UI")]
    #endregion
    #region Tooltip
    [Tooltip("Populate with heart image prefab")]
    #endregion
    public GameObject heartPrefab;
    #region Tooltip
    [Tooltip("Populate with heart empty image prefab")]
    #endregion
    public GameObject heartEmptyPrefab;
    #region Tooltip
    [Tooltip("Populate with ammo icon prefab")]
    #endregion
    public GameObject ammoIconPrefab;
    //#region Tooltip
    //[Tooltip("The score prefab")]
    //#endregion
    //public GameObject scorePrefab;
    #region Tooltip
    [Tooltip("����������� �������� �� Minimap")]
    #endregion
    public GameObject bossMinimapIcon;

    #region Header SPECIAL TILEMAP TILES
    [Space(10)]
    [Header("SPECIAL TILEMAP TILES")]
    #endregion Header SPECIAL TILEMAP TILES
    #region Tooltip
    [Tooltip("Collision tiles that the enemies can navigate to")]
    #endregion Tooltip
    public TileBase[] enemyUnwalkableCollisionTilesArray;
    #region Tooltip
    [Tooltip("Preferred path tile for enemy navigation")]
    #endregion Tooltip
    public TileBase preferredEnemyPathTile;
    #region Tooltip
    [Tooltip("IgnoreAmmoCollision tiles that the enemies can navigate to")]
    #endregion Tooltip
    public TileBase[] enemyUnwalkableIgnoreAmmoTilesArray;

}
