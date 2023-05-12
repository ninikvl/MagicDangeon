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
    [Header("Тип узла комнаты. (Типом корридоа не может быть 'Corridor') ")]
    #endregion Tooltip
    public RoomNodeTypeSO roomNodeType;

    #region Tooltip
    [Header("Координаты нижнего левого угла в Tilemap (не World Coordinates)")]
    #endregion Tooltip
    public Vector2Int lowerBounds;

    #region Tooltip
    [Header("Координаты верхнего правого угла в Tilemap (не World Coordinates)")]
    #endregion Tooltip
    public Vector2Int upperBounds;

    #region Tooltip
    [Header("В комнате максимум 4 дверных прооёма, по одному на каждое направление. Положение средней плитки является координатоой дверного проёма (position).")]
    #endregion Tooltip
    [SerializeField] public List<Doorway> doorwaysList;

    #region Tooltip
    [Header("Массив позиций в которых могут спавниться сундуки и враги (кооординаты берутся из Tilemap)")]
    #endregion Tooltip
    public Vector2Int[] spawnPosArray;

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
