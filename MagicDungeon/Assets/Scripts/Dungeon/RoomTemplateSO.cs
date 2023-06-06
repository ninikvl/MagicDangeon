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
        //��������� ����������� id ���� ��� ������� ��� �� ���������
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
