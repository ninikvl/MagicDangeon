using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonLevel_", menuName = "Scriptable Objects/����������/������� ���������� ")]
public class DungeonLevelSO : ScriptableObject
{
    #region Header ROOM PREFAB
    [Space(20)]
    [Header("������ ������")]
    #endregion Header ROOM PREFAB
    #region Tooltip
    [Header("��� ������")]
    #endregion Tooltip
    public string levelName;

    #region Header ROOM PREFAB
    [Space(20)]
    [Header("������ �������� ������")]
    #endregion Header ROOM PREFAB
    #region Tooltip
    [Tooltip("��������� ������ �������� ������")]
    #endregion Tooltip
    public List<RoomTemplateSO> roomTemplateList;

    #region Header ROOM PREFAB
    [Space(20)]
    [Header("������ ������ ������")]
    #endregion Header ROOM PREFAB
    #region Tooltip
    [Tooltip("��������� ������ ������ ������")]
    #endregion Tooltip
    public List<RoomNodeGraphSO> roomNodeGraphList;

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        //��������� ���������� ����� ������, ������� ������ � ������� ��������
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(levelName), levelName);
        if (HelperUtilities.ValidateCheckEnumerableValues(this, nameof(roomNodeGraphList), roomNodeGraphList))
            return; 
        if (HelperUtilities.ValidateCheckEnumerableValues(this, nameof(roomTemplateList), roomTemplateList))
            return;

        bool isCorridorEW = false;
        bool isCorridorNs = false;
        bool isEntrance = false;
        bool isEndRoom = false;

        //�������� �� �������� ���� ������ � ��������
        foreach (RoomTemplateSO roomTemplateSO in roomTemplateList)
        {
            if (roomTemplateSO == null)
                return;

            if (roomTemplateSO.roomNodeType.isCorridorEW)
                isCorridorEW = true;
            if (roomTemplateSO.roomNodeType.isCorridorNs)
                isCorridorNs = true;

            if (roomTemplateSO.roomNodeType.isEntrance)
                isEntrance = true;

            if (roomTemplateSO.roomNodeType.isEndRoom)
                isEndRoom = true;
        }

        //������ ��� ��������� �������� ����� ������
        if (!isCorridorEW)
            Debug.Log("�" + this.name.ToString() + "���� ������� ��������� EW");
        if (!isCorridorNs)
            Debug.Log("�" + this.name.ToString() + "���� ������� ��������� NS");
        if (!isEntrance)
            Debug.Log("�" + this.name.ToString() + "���� ������� Entrance");
        if (!isEndRoom)
            Debug.Log("�" + this.name.ToString() + "���� ������� ExitRoom");

        //��������, ���� �� ������ ���� �������� � �����
        foreach (RoomNodeGraphSO roomNodeGraph in roomNodeGraphList)
        {
            if (roomNodeGraph == null)
                return;

            foreach (RoomNodeSO roomNode in roomNodeGraph.roomNodeList)
            {
                if (roomNode == null)
                    continue;

                if (roomNode.roomNodeType.isCorridor || roomNode.roomNodeType.isCorridorEW || roomNode.roomNodeType.isCorridorNs ||
                    roomNode.roomNodeType.isEntrance || roomNode.roomNodeType.isEndRoom || roomNode.roomNodeType.isNone)
                    continue;

                bool isRoomNodeTypeNotFound = true;
                foreach (RoomTemplateSO roomTemplate in roomTemplateList)
                {
                    if (roomTemplateList == null)
                        continue;

                    if (roomTemplate.roomNodeType == roomNode.roomNodeType)
                    {
                        isRoomNodeTypeNotFound = false;
                        break;
                    }
                }
                if (isRoomNodeTypeNotFound)
                    Debug.Log("� " + this.name.ToString() + " �� ������� " + roomNode.name.ToString() + " � ����� " + roomNodeGraph.name.ToString());
            }
        }
    }

#endif
    #endregion

}
