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

    //Запполняет словарь
    private void LoadRoomNodeDictionary()
    {
        roomNodeDictionary.Clear();
        foreach (RoomNodeSO node in roomNodeList)
        {
            roomNodeDictionary[node.id] = node;
        }
    }

    public RoomNodeSO GetRoomNode(string roomNodeId)
    {
        if (roomNodeDictionary.TryGetValue(roomNodeId, out RoomNodeSO roomNode))
        {
            return roomNode;
        }
        return null;
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
