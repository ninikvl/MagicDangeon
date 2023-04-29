using UnityEditor;
using UnityEngine;
using UnityEditor.Callbacks;

public class RoomNodeGraphEditor : EditorWindow
{
    private GUIStyle roomNodeStyle;
    private GUIStyle roomNodeSelectedStyle;
    private static RoomNodeGraphSO currentRoomNodeGraph;
    private RoomNodeTypeListSO roomNodeTypeList;
    private RoomNodeSO currentRoomNode = null;


    //�������� ��� ��������� ���� � ���������
    private const float nodeWidth = 200f;
    private const float nodeHeight = 75f;
    private const int nodePadding = 25;
    private const int nodeBorder = 12;

    //�������� ����������� �����
    private const float connectingLineWidth = 3f;


    [MenuItem("�������� ������ ������", menuItem = "Window/�������� ���������� /�������� ������ ������")]
    private static void OpenWindow ()
    {
        GetWindow<RoomNodeGraphEditor>("�������� ������ ������");
    }


    private void OnEnable()
    {
        // ����� ��������� ������ ���������� (������������ �� ����������)
        Selection.selectionChanged += InspectorSelectionChanged;

         //����� ���� � ��������
        roomNodeStyle = new GUIStyle();
        roomNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
        roomNodeStyle.normal.textColor = Color.white;
        roomNodeStyle.padding = new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding);
        roomNodeStyle.border = new RectOffset(nodeBorder, nodeBorder, nodeBorder, nodeBorder);

        //����� ���������� ����
        roomNodeSelectedStyle = new GUIStyle();
        roomNodeSelectedStyle.normal.background = EditorGUIUtility.Load("node1 on") as Texture2D;
        roomNodeSelectedStyle.normal.textColor = Color.white;
        roomNodeSelectedStyle.padding = new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding);
        roomNodeSelectedStyle.border = new RectOffset(nodeBorder, nodeBorder, nodeBorder, nodeBorder);

        //��������� ������ ����� ������ �� ������
        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
    }

    private void OnDisable()
    {
        Selection.selectionChanged -= InspectorSelectionChanged;
    }


    //��������� �������� ������ ��� ������� ������� �� �����
    [OnOpenAsset(0)] // �������� ����� ��� �������� ������
    public static bool OnDoubleClickAsset (int instanceId, int line)
    {
        RoomNodeGraphSO roomNodeGraph = EditorUtility.InstanceIDToObject(instanceId) as RoomNodeGraphSO;

        if (roomNodeGraph != null)
        {
            OpenWindow();
            currentRoomNodeGraph = roomNodeGraph;
            return true;
        }
        return false;
    }
    

    private void OnGUI()
    {
        //GUILayout.BeginArea(new Rect(new Vector2(100f, 100f), new Vector2(nodeWidth, nodeHeight)), roomNodeStyle);
        //EditorGUILayout.LabelField("Node 1");
        //GUILayout.EndArea();
        
        if (currentRoomNodeGraph != null)
        {
            DrawDraggedLine();

            ProcessEvents(Event.current);

            DrawRoomConnections();

            DrawRoomNodes();
        }

        if (GUI.changed)
            Repaint();
    }

    private void DrawDraggedLine ()
    {
        if (currentRoomNodeGraph.linePos != Vector2.zero)
        {
            //��������� ����� �� ���� � ������� ����
            Handles.DrawBezier(currentRoomNodeGraph.roomNodeToDrawLineFrom.rect.center, currentRoomNodeGraph.linePos,
                currentRoomNodeGraph.roomNodeToDrawLineFrom.rect.center, currentRoomNodeGraph.linePos, Color.green, null, connectingLineWidth);
        }
    }

    //��������� �������
    private void ProcessEvents(Event currentEvent)
    {
        //�������� ���� ���� ��� �� �������� ��� �� ��� ������� ����������� ����
        if (currentRoomNode == null || currentRoomNode.isLeftClickDragging == false)
        {
            currentRoomNode = IsMouseOverRoomNode(currentEvent);
        }
        //���� �� ������ ���� ��� �������������� ��� ���� ����� ����������� ����
        if (currentRoomNode == null || currentRoomNodeGraph.roomNodeToDrawLineFrom != null)
        {
            ProcessRoomNodeGraphEvents(currentEvent);
        }
        else
        {
            currentRoomNode.ProcessEvent(currentEvent);
        }
    }

    //��������, ������ �� ���� � ������� ����
    private RoomNodeSO IsMouseOverRoomNode(Event currentEvent)
    {
        for (int i = currentRoomNodeGraph.roomNodeList.Count-1; i >= 0; i--)
        {
            if (currentRoomNodeGraph.roomNodeList[i].rect.Contains(currentEvent.mousePosition))
                return currentRoomNodeGraph.roomNodeList[i];
        }
        return null;
    }

    //��������� ������� � ��������� ������
    private void ProcessRoomNodeGraphEvents(Event currentEvent)
    {
        switch (currentEvent.type)
        {
            case EventType.MouseDown:
                ProcessMouseDownEvent(currentEvent);
                break;
            case EventType.MouseDrag:
                ProcessMouseDragEvent(currentEvent);
                break;
            case EventType.MouseUp:
                ProcessMouseUpEvent(currentEvent);
                break;

            default:
                break;
        }
    }

    //������� MouseDown ��� ������� �� ������� ��������� �����
    private void ProcessMouseDownEvent(Event currentEvent)
    {
        //����� ������������ ���� ��� ������� �� ���
        if (currentEvent.button == 1)
        {
            ShowContextMenu(currentEvent.mousePosition);
        }
        else if (currentEvent.button == 0)
        {
            ClearLineDrag();
            ClearAllSelectOnRoomNodes();
        }
    }
    
    private void ProcessMouseUpEvent(Event currentEvent)
    {
        if (currentEvent.button == 1 && currentRoomNodeGraph.roomNodeToDrawLineFrom != null)
        {
            RoomNodeSO roomNode = IsMouseOverRoomNode(currentEvent);

            if (roomNode != null)
            {
                if (currentRoomNodeGraph.roomNodeToDrawLineFrom.AddChildRoomNodeIDToRoomNode(roomNode.id)) //?
                {
                    roomNode.AddParentRoomNodeIDToRoomNode(currentRoomNodeGraph.roomNodeToDrawLineFrom.id); //?
                }
            }

            ClearLineDrag();
        }
            
    }

    //������ � ����������� ����������� ����
    private void ShowContextMenu(Vector2 mousePos)
    {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("������� ����"), false, CreateRoomNode, mousePos);
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("������� ��� ����"), false, SelectAllRoomNodes);
        menu.ShowAsContext();
    }

    //�������� ���� � ������� ����
    private void CreateRoomNode (object mousePosObject)
    {
        if (currentRoomNodeGraph.roomNodeList.Count == 0)
        {
            CreateRoomNode(new Vector2(200f, 200f), roomNodeTypeList.list.Find(x => x.isEntrance));
        }

        CreateRoomNode(mousePosObject, roomNodeTypeList.list.Find(x => x.isNone));
    }

    //�������� ����
    private void CreateRoomNode (object mousePosObject, RoomNodeTypeSO roomNodeType)
    {
        Vector2 mousePos = (Vector2)mousePosObject;

        //�������� �����
        RoomNodeSO roomNode = ScriptableObject.CreateInstance<RoomNodeSO>(); 

        // ���������� ����� � ������ ����� �����
        currentRoomNodeGraph.roomNodeList.Add(roomNode); 

        //��������� �������� ��������� ����
        roomNode.Initialize(new Rect(mousePos, new Vector2(nodeWidth, nodeHeight)), currentRoomNodeGraph, roomNodeType);

        //���������� ���� � ����� �����
        AssetDatabase.AddObjectToAsset(roomNode, currentRoomNodeGraph);
        AssetDatabase.SaveAssets();

        currentRoomNodeGraph.OnValidate();
    }

    //������� ��������� � ��������� �����
    private void ClearAllSelectOnRoomNodes()
    {
        foreach (RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
        {
            if (roomNode.isSelected)
            {
                roomNode.isSelected = false;

                GUI.changed = true;
            }
        }
    }

    //������� ��� ����
    private void SelectAllRoomNodes()
    {
        foreach (RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
        {
            roomNode.isSelected = true;
        }
        GUI.changed = true;
    }



    private void ProcessMouseDragEvent(Event currentEvent)
    {
        if (currentEvent.button == 1)
        {
            ProcessRightMouseDragEvent(currentEvent);
        }
    }
    
    private void ProcessRightMouseDragEvent(Event currentEvent)
    {
        if (currentRoomNodeGraph.roomNodeToDrawLineFrom != null)
        {
            DragConnectingLine(currentEvent.delta);
            GUI.changed = true;
        }
    }

    private void DragConnectingLine(Vector2 delta)
    {
        currentRoomNodeGraph.linePos += delta;
    }




    private void DrawRoomNodes ()
    {
        foreach(RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
        {
            if (roomNode.isSelected)
            {
                roomNode.Draw(roomNodeSelectedStyle);
            }
            else
            {
                roomNode.Draw(roomNodeStyle);
            }
        }

        GUI.changed = true;
    }
    
    private void ClearLineDrag()
    {
        currentRoomNodeGraph.roomNodeToDrawLineFrom = null;
        currentRoomNodeGraph.linePos = Vector2.zero;
        GUI.changed = true;
    }

    private void DrawRoomConnections()
    {
        foreach (RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
        {
            if (roomNode.childRoomNodeIDList.Count > 0)
            {
                foreach (string childRoomNodeID in roomNode.childRoomNodeIDList)
                {
                    if (currentRoomNodeGraph.roomNodeDictionary.ContainsKey(childRoomNodeID)) //?
                    {
                        DrawConnectionLine(roomNode, currentRoomNodeGraph.roomNodeDictionary[childRoomNodeID]); //?
                        GUI.changed = true;
                    }
                }
            }
        }
    }

    private void DrawConnectionLine(RoomNodeSO parentNode, RoomNodeSO childNode)
    {
        //��������� � �������� ����� �����
        Vector2 startPos = parentNode.rect.center;
        Vector2 endPos = childNode.rect.center;
        
        // ������ ������ 
        Vector2 midPosition = (endPos + startPos) / 2f;

        //�����������
        Vector2 direction = endPos - startPos;

        Vector2 arrowTailPoint1 = midPosition - new Vector2(-direction.y, direction.x).normalized * 5f;
        Vector2 arrowTailPoint2 = midPosition + new Vector2(-direction.y, direction.x).normalized * 5f;

        
        Vector2 arrowHeadPoint = midPosition + direction.normalized * 15f;
        Handles.DrawBezier(arrowHeadPoint, arrowTailPoint1, arrowHeadPoint, arrowTailPoint1, Color.green, null, connectingLineWidth);
        Handles.DrawBezier(arrowHeadPoint, arrowTailPoint2, arrowHeadPoint, arrowTailPoint2, Color.green, null, connectingLineWidth);

        // Handles.DrawBezier(startPos, modPosition, startPos, modPosition, Color.white, null, connectingLineWidth);
        // Handles.DrawBezier(modPosition, endPos, modPosition, endPos, Color.green, null, connectingLineWidth);
        Handles.DrawBezier(startPos, endPos, startPos, endPos, Color.green, null, connectingLineWidth);

        GUI.changed = true;
    }

    //Selection changd in the inspector
    private void InspectorSelectionChanged()
    {
        RoomNodeGraphSO roomNodeGraph = Selection.activeObject as RoomNodeGraphSO;

        if (roomNodeGraph != null)
        {
            currentRoomNodeGraph = roomNodeGraph;
            GUI.changed = true;
        }
    }

}
