using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomNodeGraph", menuName = "Scriptable Objects/�������� ����������/���� ������")]
public class RoomNodeGraphSO : ScriptableObject
{
    [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList;
    [HideInInspector] public List<RoomNodeSO> roomNodeList = new List<RoomNodeSO>();
    [HideInInspector] public Dictionary<string, RoomNodeSO> roomNodeDictionary = new Dictionary<string, RoomNodeSO>();

    private void Awake()
    {
        LoadRoomNodeDictionary();
    }

    /// <summary>
    /// ���������� �������
    /// </summary>
    private void LoadRoomNodeDictionary()
    {
        roomNodeDictionary.Clear();
        foreach (RoomNodeSO node in roomNodeList)
        {
            roomNodeDictionary[node.id] = node;
        }
    }

    /// <summary>
    /// �������� ���� ������� � ������� ���� �������
    /// </summary>
    public RoomNodeSO GetRoomNode (RoomNodeTypeSO roomNodeType)
    {
        foreach (RoomNodeSO node in roomNodeList)
        {
            if (node.roomNodeType == roomNodeType)
            {
                return node;
            }
        }
        return null;
    }

    /// <summary>
    /// ���������� ���� �� ��� id � �������
    /// </summary>
    public RoomNodeSO GetRoomNode(string roomNodeId)
    {
        if (roomNodeDictionary.TryGetValue(roomNodeId, out RoomNodeSO roomNode))
        {
            return roomNode;
        }
        return null;
    }

    /// <summary>
    /// ���������� �������� ���� �������� �������������� ����
    /// </summary>
    public IEnumerable<RoomNodeSO> GetChildRoomNodes(RoomNodeSO parentRoom)
    {
        foreach (string childNodeId in parentRoom.childRoomNodeIDList)
        {
            yield return GetRoomNode(childNodeId);
        }
    }



    #region Editor Code
#if UNITY_EDITOR

    [HideInInspector] public RoomNodeSO roomNodeToDrawLineFrom = null;
    [HideInInspector] public Vector2 linePos;

    //��������� ������� ����� ���������� � ����������
    public void OnValidate()
    {
        LoadRoomNodeDictionary();
    }


    public void SetNodeToDrawConnectionLineFrom (RoomNodeSO node, Vector2 pos)
    {
        roomNodeToDrawLineFrom = node;
        linePos = pos;
    }

#endif
    #endregion
}
