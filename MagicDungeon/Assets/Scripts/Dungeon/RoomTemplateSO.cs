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
    [Header("��� ���� �������. (����� �������� �� ����� ���� 'Corridor') ")]
    #endregion Tooltip
    public RoomNodeTypeSO roomNodeType;

    #region Tooltip
    [Header("���������� ������� ������ ���� � Tilemap (�� World Coordinates)")]
    #endregion Tooltip
    public Vector2Int lowerBounds;

    #region Tooltip
    [Header("���������� �������� ������� ���� � Tilemap (�� World Coordinates)")]
    #endregion Tooltip
    public Vector2Int upperBounds;

    #region Tooltip
    [Header("� ������� �������� 4 ������� ������, �� ������ �� ������ �����������. ��������� ������� ������ �������� ������������ �������� ����� (position).")]
    #endregion Tooltip
    [SerializeField] public List<Doorway> doorwaysList;

    #region Tooltip
    [Header("������ ������� � ������� ����� ���������� ������� � ����� (����������� ������� �� Tilemap)")]
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
