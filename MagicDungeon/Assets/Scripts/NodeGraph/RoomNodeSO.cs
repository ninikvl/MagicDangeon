using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RoomNodeSO : ScriptableObject
{
    [HideInInspector] public string id;
    [HideInInspector] public List<string> parentRoomNodeIDList = new List<string>();
    [HideInInspector] public List<string> childRoomNodeIDList = new List<string>();
    [HideInInspector] public RoomNodeGraphSO roomNodeGraph;
    public RoomNodeTypeSO roomNodeType;
    [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList;

    #region Editor Code
#if UNITY_EDITOR

    [HideInInspector] public Rect rect;
    [HideInInspector] public bool isLeftClickDragging = false;
    [HideInInspector] public bool isSelected = false;

    //Инициалиизация узла
    public void Initialize(Rect rect, RoomNodeGraphSO nodeGraph, RoomNodeTypeSO roomNodeType)
    {
        this.rect = rect;
        this.id = Guid.NewGuid().ToString();
        this.name = "RoomNode";
        this.roomNodeGraph = nodeGraph;
        this.roomNodeType = roomNodeType;

        //Загрузка спика типов комнат
        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
    }

    //Отрисовка узла с определённым стилем
    public void Draw (GUIStyle nodeStyle)
    {
        //Отрисовка зоны узла
        GUILayout.BeginArea(rect, nodeStyle);

        //проверка на изменения
        EditorGUI.BeginChangeCheck();

        if (parentRoomNodeIDList.Count > 0 || roomNodeType.isEntrance)
        {
            // Lable который нельзя изменять
            EditorGUILayout.LabelField(roomNodeType.roomNodeTypeName);
        }
        else
        {
            int selected = roomNodeTypeList.list.FindIndex(x => x == roomNodeType);
            int selection = EditorGUILayout.Popup("", selected, GetRoomNodeTypesToDisplay());
            roomNodeType = roomNodeTypeList.list[selection];
        }

        if (EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(this);

        GUILayout.EndArea();
    }

    //массив с типами коинат которые отображаются в граф редактооре
    public string[] GetRoomNodeTypesToDisplay ()
    {
        string[] roomArray = new string[roomNodeTypeList.list.Count];

        for (int i = 0; i < roomNodeTypeList.list.Count; i++)
        {
            if (roomNodeTypeList.list[i].displayInNodeGraphEditor)
            {
                roomArray[i] = roomNodeTypeList.list[i].roomNodeTypeName;
            }
        }
        return roomArray;
    }

    public void ProcessEvent(Event currentEvent)
    {
        switch (currentEvent.type)
        {
            case EventType.MouseDown:
                ProcessMouseDownEvent(currentEvent);
                break;
            case EventType.MouseUp:
                ProcessMouseUpEvent(currentEvent);
                break;
            case EventType.MouseDrag:
                ProcessMouseDragEvent(currentEvent);
                break;
           
            default:
                break;
        }
    }


    private void ProcessMouseDownEvent(Event currentEvent)
    {
        if (currentEvent.button == 0)
        {
            ProcessLeftClickDownEvent();
        }
        else if (currentEvent.button == 1)
        {
            ProcessRightClickDownEvent(currentEvent);
        }
    }

    private void ProcessLeftClickDownEvent()
    {
        Selection.activeObject = this;

        isSelected = !isSelected;
    }
    
    private void ProcessRightClickDownEvent(Event currentEvent)
    {
        roomNodeGraph.SetNodeToDrawConnectionLineFrom(this, currentEvent.mousePosition);
    }


    private void ProcessMouseUpEvent(Event currentEvent)
    {
        if (currentEvent.button == 0)
        {
            ProcessLeftClickUpEvent();
        }
    }

    private void ProcessLeftClickUpEvent()
    {
        if (isLeftClickDragging)
        {
            isLeftClickDragging = false;
        }
    }


    private void ProcessMouseDragEvent(Event currentEvent)
    {
        if (currentEvent.button == 0)
        {
            ProcessLeftClickDragEvent(currentEvent);
        }
    }

    private void ProcessLeftClickDragEvent(Event currentEvent)
    {
        isLeftClickDragging = true;
        DragNode(currentEvent.delta);
        GUI.changed = true;
    }


    private void DragNode (Vector2 delta)
    {
        rect.position += delta;
        EditorUtility.SetDirty(this);
    }

    public bool AddChildRoomNodeIDToRoomNode(string childID)//?
    {
        if (IsChildRoomValid(childID))
        {
            childRoomNodeIDList.Add(childID);
            return true;//?
        }
        return false;
    }

    //проверка пподключаемой комнаты на правила
    public bool IsChildRoomValid(string childID)
    {
        bool isConnectedBossNodeAlready = false;
        
        //Есит ли уже подключённая комна с боссом в графе
        foreach (RoomNodeSO roomNode in roomNodeGraph.roomNodeList)
        {
            if (roomNode.roomNodeType.isBossRoom && roomNode.parentRoomNodeIDList.Count > 0)
                isConnectedBossNodeAlready = true;
        }

        //если новая комната босса является ещё одной комнатой босса то false
        if (roomNodeGraph.GetRoomNode(childID).roomNodeType.isBossRoom && isConnectedBossNodeAlready)
            return false;

        //Если подключаемая комната без указанного типа то false
        if (roomNodeGraph.GetRoomNode(childID).roomNodeType.isNone)
            return false;

        //Если подключаем новую комнату по существующему соединию то false
        if (childRoomNodeIDList.Contains(childID))
            return false;

        //Если подключаем комнату к себе же то false
        if (id == childID)
            return false;

        //Если подключаемся к родительксому узлу то false
        if (parentRoomNodeIDList.Contains(childID))
            return false;

        //Каждый узел имеет только один родительский узел то false
        if (roomNodeGraph.GetRoomNode(childID).parentRoomNodeIDList.Count > 0)
            return false;

        //Если поключается коридор к коридору то false
        if (roomNodeGraph.GetRoomNode(childID).roomNodeType.isCorridor && roomNodeType.isCorridor)
            return false;

        //омнут можно подключить только к коридору, а коридор только к комнате
        if (!roomNodeGraph.GetRoomNode(childID).roomNodeType.isCorridor && !roomNodeType.isCorridor)
            return false;

        //Если дочерних подключений больше 3 то false
        if (roomNodeGraph.GetRoomNode(childID).roomNodeType.isCorridor && childRoomNodeIDList.Count > Settings.maxChildCorridors)
            return false;

        //Стартовая комната не может быть дочерним узлом
        if (roomNodeGraph.GetRoomNode(childID).roomNodeType.isEntrance)
            return false;

        //Если у коридора уже есть дочерняя комната то false
        if (!roomNodeGraph.GetRoomNode(childID).roomNodeType.isCorridor && childRoomNodeIDList.Count > 0)
            return false;

        return true;
    }


    public bool AddParentRoomNodeIDToRoomNode(string parentID)//?
    {
        parentRoomNodeIDList.Add(parentID);
        return true;//?
    }

#endif
    #endregion
}
