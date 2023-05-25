using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonLevel_", menuName = "Scriptable Objects/Подземелье/Уровень подземелья ")]
public class DungeonLevelSO : ScriptableObject
{
    #region Header ROOM PREFAB
    [Space(20)]
    [Header("Детали уровня")]
    #endregion Header ROOM PREFAB
    #region Tooltip
    [Header("Имя уровня")]
    #endregion Tooltip
    public string levelName;

    #region Header ROOM PREFAB
    [Space(20)]
    [Header("Список шаблонов комнат")]
    #endregion Header ROOM PREFAB
    #region Tooltip
    [Tooltip("Заполните список шаблонов комнат")]
    #endregion Tooltip
    public List<RoomTemplateSO> roomTemplateList;

    #region Header ROOM PREFAB
    [Space(20)]
    [Header("Список графов уровня")]
    #endregion Header ROOM PREFAB
    #region Tooltip
    [Tooltip("Заполните список графов уровня")]
    #endregion Tooltip
    public List<RoomNodeGraphSO> roomNodeGraphList;

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        //проверкка заполнения имени уровня, списков графов и списков шаблонов
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(levelName), levelName);
        if (HelperUtilities.ValidateCheckEnumerableValues(this, nameof(roomNodeGraphList), roomNodeGraphList))
            return; 
        if (HelperUtilities.ValidateCheckEnumerableValues(this, nameof(roomTemplateList), roomTemplateList))
            return;

        bool isCorridorEW = false;
        bool isCorridorNs = false;
        bool isEntrance = false;
        bool isEndRoom = false;

        //Проверка на основные типы комнат в шаблонах
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

        //Ошибки при отсутсвии основных типов комнат
        if (!isCorridorEW)
            Debug.Log("В" + this.name.ToString() + "нету шаблона корридора EW");
        if (!isCorridorNs)
            Debug.Log("В" + this.name.ToString() + "нету шаблона корридора NS");
        if (!isEntrance)
            Debug.Log("В" + this.name.ToString() + "нету шаблона Entrance");
        if (!isEndRoom)
            Debug.Log("В" + this.name.ToString() + "нету шаблона ExitRoom");

        //Проверка, есть ли нужные типы шаблонов в графе
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
                    Debug.Log("В " + this.name.ToString() + " не хватает " + roomNode.name.ToString() + " в графе " + roomNodeGraph.name.ToString());
            }
        }
    }

#endif
    #endregion

}
