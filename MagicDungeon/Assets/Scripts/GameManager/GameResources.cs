using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    #region Header Door Boss Prefab
    [Space(10)]
    [Header("������� ������ �����")]
    #endregion
    public GameObject doorNSBossPrefab;
    public GameObject doorEWBossPrefab;

    #region Header UI
    [Space(10)]
    [Header("UI")]
    #endregion
    //#region Tooltip
    //[Tooltip("Populate with heart image prefab")]
    //#endregion
    //public GameObject heartPrefab;
    #region Tooltip
    [Tooltip("Populate with ammo icon prefab")]
    #endregion
    public GameObject ammoIconPrefab;
    //#region Tooltip
    //[Tooltip("The score prefab")]
    //#endregion
    //public GameObject scorePrefab;


}
