using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Room_", menuName ="Scriptable Objects/Подземелье/Комната")]
public class RoomTemplateSO : ScriptableObject
{
    // id комнаты
    [HideInInspector] public string guid;

    #region Header ROOM PREFAB
    [Space(20)]
    [Header("Префаб комнаты")]
    #endregion Header ROOM PREFAB

    #region Tooltip
    [Header("Префаб комнаты который хранит в себе Tilemaps комнаты и объекты комнаты")]
    #endregion Tooltip
    public GameObject prefab;

    //используется для пересоздания id если SO objject скопирован и префаб поменялся
    [HideInInspector] public GameObject previousPrefab;

    #region Header ROOM CONFIGURATION
    [Space(10)]
    [Header("Конфигурация комнаты")]
    #endregion ROOM CONFIGURATION

    #region Tooltip
    [Tooltip("Тип узла комнаты. (Типом корридоа не может быть 'Corridor') ")]
    #endregion Tooltip
    public RoomNodeTypeSO roomNodeType;

    #region Tooltip
    [Tooltip("Координаты нижнего левого угла в Tilemap (не World Coordinates)")]
    #endregion Tooltip
    public Vector2Int lowerBounds;

    #region Tooltip
    [Tooltip("Координаты верхнего правого угла в Tilemap (не World Coordinates)")]
    #endregion Tooltip
    public Vector2Int upperBounds;

    #region Tooltip
    [Tooltip("В комнате максимум 4 дверных прооёма, по одному на каждое направление. Положение средней плитки является координатоой дверного проёма (position).")]
    #endregion Tooltip
    [SerializeField] public List<Doorway> doorwaysList;

    #region Tooltip
    [Tooltip("Массив позиций в которых могут спавниться сундуки и враги (кооординаты берутся из Tilemap)")]
    #endregion Tooltip
    public Vector2Int[] spawnPosArray;

    #region Header ENEMY DETAILS

    [Space(10)]
    [Header("ENEMY DETAILS")]

    #endregion Header ENEMY DETAILS
    #region Tooltip
    [Tooltip("Populate the list with all the enemies that can be spawned in this room by dungeon level, including the ratio (random) of this enemy type that will be spawned")]
    #endregion Tooltip
    public List<SpawnableObjectsByLevel<EnemyDetailsSO>> enemiesByLevelList;

    //#region Tooltip
    //[Tooltip("Populate the list with the spawn parameters for the enemies.")]
    //#endregion Tooltip
    //public List<RoomEnemySpawnParameters> roomEnemySpawnParametersList;

    public List<Doorway> GetDoorwayList ()
    {
        return doorwaysList;
    }

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        //Установка уникального id если нет префаба или он поменялся
        if(guid=="" || previousPrefab != prefab)
        {
            guid = GUID.Generate().ToString();
            previousPrefab = prefab;
            EditorUtility.SetDirty(this);
        }

        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(doorwaysList), doorwaysList);
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(spawnPosArray), spawnPosArray);
    }

#endif
    #endregion
}
