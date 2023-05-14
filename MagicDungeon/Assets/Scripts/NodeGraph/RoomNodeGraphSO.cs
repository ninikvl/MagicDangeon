using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomNodeGraph", menuName = "Scriptable Objects/Редактор подземелья/Граф комнат")]
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
    /// Запполняет словарь
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
    /// Получить узел комнаты с помощью типа комнаты
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
    /// Возвращает узел по его id в словаре
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
    /// Возвращает дочерние узлы принимая родительскккий узел
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

    //Заполняет словарь пппри изменениии в инспекторе
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
