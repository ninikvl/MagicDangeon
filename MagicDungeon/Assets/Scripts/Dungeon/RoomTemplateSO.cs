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
    [Header("Детали врагов")]

    #endregion Header ENEMY DETAILS
    #region Tooltip
    [Tooltip("Заполните список всеми врагами, которые могут быть порождены в этой комнате, в зависимости от уровня подземелья, включая соотношение (случайное) этого типа врагов, которые будут порождены")]
    #endregion Tooltip
    public List<SpawnableObjectsByLevel<EnemyDetailsSO>> enemiesByLevelList;

    #region Tooltip
    [Tooltip("Заполните список параметрами появления врагов.")]
    #endregion Tooltip
    public List<RoomEnemySpawnParameters> roomEnemySpawnParametersList;

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

        // проверка параметров появления врагов и комнат для уровней
        if (enemiesByLevelList.Count > 0 || roomEnemySpawnParametersList.Count > 0)
        {
            HelperUtilities.ValidateCheckEnumerableValues(this, nameof(enemiesByLevelList), enemiesByLevelList);
            HelperUtilities.ValidateCheckEnumerableValues(this, nameof(roomEnemySpawnParametersList), roomEnemySpawnParametersList);

            foreach (RoomEnemySpawnParameters roomEnemySpawnParameters in roomEnemySpawnParametersList)
            {
                HelperUtilities.ValidateCheckNullValue(this, nameof(roomEnemySpawnParameters.dungeonLevel), roomEnemySpawnParameters.dungeonLevel);
                HelperUtilities.vakidateCheckPositiveRange(this, nameof(roomEnemySpawnParameters.minTotalEnemiesToSpawn), roomEnemySpawnParameters.minTotalEnemiesToSpawn, nameof(roomEnemySpawnParameters.maxTotalEnemiesToSpawn), roomEnemySpawnParameters.maxTotalEnemiesToSpawn, true);
                HelperUtilities.vakidateCheckPositiveRange(this, nameof(roomEnemySpawnParameters.minSpawnInterval), roomEnemySpawnParameters.minSpawnInterval, nameof(roomEnemySpawnParameters.maxSpawnInterval), roomEnemySpawnParameters.maxSpawnInterval, true);
                HelperUtilities.vakidateCheckPositiveRange(this, nameof(roomEnemySpawnParameters.minConcurrentEnemies), roomEnemySpawnParameters.minConcurrentEnemies, nameof(roomEnemySpawnParameters.maxConcurrentEnemies), roomEnemySpawnParameters.maxConcurrentEnemies, false);

                bool isEnemyTypesListForDungeonLevel = false;

                // Проверка списка типов врагов
                foreach (SpawnableObjectsByLevel<EnemyDetailsSO> dungeonObjectsByLevel in enemiesByLevelList)
                {
                    if (dungeonObjectsByLevel.dungeonLevel == roomEnemySpawnParameters.dungeonLevel && dungeonObjectsByLevel.spawnableObjectRatioList.Count > 0)
                        isEnemyTypesListForDungeonLevel = true;

                    HelperUtilities.ValidateCheckNullValue(this, nameof(dungeonObjectsByLevel.dungeonLevel), dungeonObjectsByLevel.dungeonLevel);

                    foreach (SpawnableObjectRatio<EnemyDetailsSO> dungeonObjectRatio in dungeonObjectsByLevel.spawnableObjectRatioList)
                    {
                        HelperUtilities.ValidateCheckNullValue(this, nameof(dungeonObjectRatio.dungeonObject), dungeonObjectRatio.dungeonObject);

                        HelperUtilities.ValidateCheckPositiveValue(this, nameof(dungeonObjectRatio.ratio), dungeonObjectRatio.ratio, false);
                    }

                }

                if (isEnemyTypesListForDungeonLevel == false && roomEnemySpawnParameters.dungeonLevel != null)
                {
                    Debug.Log("No enemy types specified in for dungeon level " + roomEnemySpawnParameters.dungeonLevel.levelName + " in gameobject " + this.name.ToString());
                }
            }
        }

        // проверка на заполненные позиции для появления
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(spawnPosArray), spawnPosArray);
    }


#endif
    #endregion
}
