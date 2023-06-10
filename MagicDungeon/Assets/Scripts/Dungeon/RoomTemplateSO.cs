using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Room_", menuName ="Scriptable Objects/����������/�������")]
public class RoomTemplateSO : ScriptableObject
{
    // id �������
    [HideInInspector] public string guid;

    #region Header ROOM PREFAB
    [Space(20)]
    [Header("������ �������")]
    #endregion Header ROOM PREFAB

    #region Tooltip
    [Header("������ ������� ������� ������ � ���� Tilemaps ������� � ������� �������")]
    #endregion Tooltip
    public GameObject prefab;

    //������������ ��� ������������ id ���� SO objject ���������� � ������ ���������
    [HideInInspector] public GameObject previousPrefab;

    #region Header ROOM CONFIGURATION
    [Space(10)]
    [Header("������������ �������")]
    #endregion ROOM CONFIGURATION

    #region Tooltip
    [Tooltip("��� ���� �������. (����� �������� �� ����� ���� 'Corridor') ")]
    #endregion Tooltip
    public RoomNodeTypeSO roomNodeType;

    #region Tooltip
    [Tooltip("���������� ������� ������ ���� � Tilemap (�� World Coordinates)")]
    #endregion Tooltip
    public Vector2Int lowerBounds;

    #region Tooltip
    [Tooltip("���������� �������� ������� ���� � Tilemap (�� World Coordinates)")]
    #endregion Tooltip
    public Vector2Int upperBounds;

    #region Tooltip
    [Tooltip("� ������� �������� 4 ������� ������, �� ������ �� ������ �����������. ��������� ������� ������ �������� ������������ �������� ����� (position).")]
    #endregion Tooltip
    [SerializeField] public List<Doorway> doorwaysList;

    #region Tooltip
    [Tooltip("������ ������� � ������� ����� ���������� ������� � ����� (����������� ������� �� Tilemap)")]
    #endregion Tooltip
    public Vector2Int[] spawnPosArray;

    #region Header ENEMY DETAILS

    [Space(10)]
    [Header("������ ������")]

    #endregion Header ENEMY DETAILS
    #region Tooltip
    [Tooltip("��������� ������ ����� �������, ������� ����� ���� ��������� � ���� �������, � ����������� �� ������ ����������, ������� ����������� (���������) ����� ���� ������, ������� ����� ���������")]
    #endregion Tooltip
    public List<SpawnableObjectsByLevel<EnemyDetailsSO>> enemiesByLevelList;

    #region Tooltip
    [Tooltip("��������� ������ ����������� ��������� ������.")]
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
        //��������� ����������� id ���� ��� ������� ��� �� ���������
        if(guid=="" || previousPrefab != prefab)
        {
            guid = GUID.Generate().ToString();
            previousPrefab = prefab;
            EditorUtility.SetDirty(this);
        }

        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(doorwaysList), doorwaysList);
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(spawnPosArray), spawnPosArray);

        // �������� ���������� ��������� ������ � ������ ��� �������
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

                // �������� ������ ����� ������
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

        // �������� �� ����������� ������� ��� ���������
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(spawnPosArray), spawnPosArray);
    }


#endif
    #endregion
}
