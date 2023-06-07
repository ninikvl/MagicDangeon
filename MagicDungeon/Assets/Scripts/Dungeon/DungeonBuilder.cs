using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[DisallowMultipleComponent]
public class DungeonBuilder : SingletonMonobehavior<DungeonBuilder>
{
    public Dictionary<string, Room> dungeonBuilderRoomDictionary = new Dictionary<string, Room>();
    private Dictionary<string, RoomTemplateSO> roomTemplateDictionary = new Dictionary<string, RoomTemplateSO>();
    private List<RoomTemplateSO> roomTemplateList = null;
    private RoomNodeTypeListSO roomNodeTypeList;
    private bool dungeonBuildSuccessful;

    protected override void Awake()
    {
        base.Awake();

        LoadRoomNodeTypeList();

        //GameResources.Instance.dimmedMaterial.SetFloat("Alpha_Slider", 1f);

    }

    private void OnEnable()
    {
        GameResources.Instance.dimmedMaterial.SetFloat("Alpha_Slider", 0f);
    }

    private void OnDisable()
    {
        GameResources.Instance.dimmedMaterial.SetFloat("Alpha_Slider", 1f);
    }

    /// <summary>
    /// Загрузить список типов узлов
    /// </summary>
    private void LoadRoomNodeTypeList()
    {
        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
    }

    /// <summary>
    /// Генерирует уровнь возвращает истину если построено, ложь если неудачно
    /// </summary>
    public bool GenerateDungeon(DungeonLevelSO currentDungeonLevel)
    {
        roomTemplateList = currentDungeonLevel.roomTemplateList;

        //запоолнение словаря
        LoadRoomTemplatesIntoDictionary();

        dungeonBuildSuccessful = false;
        int dungeonBuildAttemps = 0;

        while (!dungeonBuildSuccessful && dungeonBuildAttemps < Settings.maxDungeonRebuildAttempts)
        {
            dungeonBuildAttemps++;

            //выборка случайного графа
            RoomNodeGraphSO roomNodeGraph = SelectRandomNodeGraph(currentDungeonLevel.roomNodeGraphList);

            int dungeonRebuildAttemptsForNodeGraph = 0;
            dungeonBuildSuccessful = false;
            while (!dungeonBuildSuccessful && dungeonRebuildAttemptsForNodeGraph <= Settings.maxDungeonRebuildAttemptsForRoomGraph)
            {
                ClearDungeon();

                dungeonRebuildAttemptsForNodeGraph++;

                dungeonBuildSuccessful = AttemptToBuildRandomDungeon(roomNodeGraph);
            }
            if (dungeonBuildSuccessful)
            {
                InstiateRoomgameObjects();
            }
        }

        return dungeonBuildSuccessful;
    }

    /// <summary>
    /// заполнение словаря  списком
    /// </summary>
    private void LoadRoomTemplatesIntoDictionary()
    {
        roomTemplateDictionary.Clear();

        //запоолнение словаря из списка
        foreach (RoomTemplateSO roomTemplate in roomTemplateList)
        {
            if (!roomTemplateDictionary.ContainsKey(roomTemplate.guid))
            {
                roomTemplateDictionary.Add(roomTemplate.guid, roomTemplate);
            }
            else
            {
                Debug.Log("Дублирование id шаблона в словаре" + roomTemplateList);
            }
        }
    }

    /// <summary>
    /// Попытка построить уровень с помощью выбранного графа комнат
    /// Возвращает правду, если удалость сгенерировать макет
    /// Возвращает ложь, если попытка сгенерировать не удалась
    /// </summary>
    private bool AttemptToBuildRandomDungeon(RoomNodeGraphSO roomNodeGraph)
    {
        //Создание очереди узлов
        Queue<RoomNodeSO> openRoomNodeQueue = new Queue<RoomNodeSO>();

        //Добавление узла входа в очередь
        RoomNodeSO entranceNode = roomNodeGraph.GetRoomNode(roomNodeTypeList.list.Find(x => x.isEntrance));
        if (entranceNode != null)
        {
            openRoomNodeQueue.Enqueue(entranceNode);
        }
        else
        {
            Debug.Log("Нету узла входа");
            return false;
        }

        bool noRoomOverlaps = true;

        noRoomOverlaps = ProcessRoomsInOpenRoomNodeQueue(roomNodeGraph, openRoomNodeQueue, noRoomOverlaps);

        //Есди все узлы в очереди обрабоотаны и нет наложений комнат
        if (openRoomNodeQueue.Count == 0 && noRoomOverlaps)
            return true;
        else
            return false;
    }

    /// <summary>
    /// Обрабатывает комны в очереди узлов комнат
    /// Возвращает истинну, если нету наложений
    /// </summary>
    private bool ProcessRoomsInOpenRoomNodeQueue(RoomNodeGraphSO roomNodeGraph, Queue<RoomNodeSO> openRoomNodeQueue, bool noRoomOverlaps)
    {
        while (openRoomNodeQueue.Count > 0 && noRoomOverlaps == true)
        {
            RoomNodeSO roomNode = openRoomNodeQueue.Dequeue();

            foreach (RoomNodeSO childRoomNode in roomNodeGraph.GetChildRoomNodes(roomNode))
            {
                openRoomNodeQueue.Enqueue(childRoomNode);
            }

            //Если узел входа, то  добавляет в словарь и позиционирует
            if (roomNode.roomNodeType.isEntrance)
            {
                RoomTemplateSO roomTemplateSO = GetRandomRoomTemplate(roomNode.roomNodeType);
                Room room = CreateRroomFromRoomTemplate(roomTemplateSO, roomNode);
                room.isPositioned = true;
                //Добавление комнаты в словарь
                dungeonBuilderRoomDictionary.Add(room.id, room);
            }
            else
            {
                //Получение родительского узла
                Room parentRoom = dungeonBuilderRoomDictionary[roomNode.parentRoomNodeIDList[0]];

                noRoomOverlaps = CanPlaceRoomWithNoOverlaps(roomNode, parentRoom);
            }
        }

        return noRoomOverlaps;
    }

    /// <summary>
    /// Попытка поместить узел комнаты на уровень
    /// Возвращает истину, если комната может быть размещена, в противном случае null
    /// </summary>
    private bool CanPlaceRoomWithNoOverlaps(RoomNodeSO roomNode, Room parentRoom)
    {
        //Предполагает перекрытие, пока не будет доказано обратоное
        bool roomOverlaps = true;
        while (roomOverlaps)
        {
            List<Doorway> unconnectedAvailableParentDoorways = GetUnconnectedAvailableDoorways(parentRoom.doorWayList).ToList();

            //Если нету доступных дверных проёмов
            if (unconnectedAvailableParentDoorways.Count == 0)
            {
                return false;
            }

            Doorway doorwayParent = unconnectedAvailableParentDoorways[Random.Range(0, unconnectedAvailableParentDoorways.Count)];

            //Получение случайного шаблона с учём ориентации дверного проёма
            RoomTemplateSO roomTemplate = GetRandomRoomTemplateForConsistentWithParent(roomNode, doorwayParent);

            //Создание комнаты
            Room room = CreateRroomFromRoomTemplate(roomTemplate, roomNode);

            if (PlaceRoom(parentRoom, doorwayParent, room))
            {
                roomOverlaps = false;
                room.isPositioned = true;
                dungeonBuilderRoomDictionary.Add(room.id, room);
            }
            else
            {
                roomOverlaps = true;
            }
        }

        return true;
    }

    /// <summary>
    /// Получить шаблон комнаты с учётом ориентации дверного проёма родителя
    /// </summary>
    private RoomTemplateSO GetRandomRoomTemplateForConsistentWithParent(RoomNodeSO roomNode, Doorway doorwayParent)
    {
        RoomTemplateSO roomTemplate = null;

        if (roomNode.roomNodeType.isCorridor)
        {
            switch (doorwayParent.orientation)
            {
                case Orientation.north:
                case Orientation.south:
                    roomTemplate = GetRandomRoomTemplate(roomNodeTypeList.list.Find(x => x.isCorridorNs));
                    break;

                case Orientation.east:
                case Orientation.west:
                    roomTemplate = GetRandomRoomTemplate(roomNodeTypeList.list.Find(x => x.isCorridorEW));
                    break;

                case Orientation.none:
                    break;
                default:
                    break;
            }
        }
        else
        {
            roomTemplate = GetRandomRoomTemplate(roomNode.roomNodeType);
        }

        return roomTemplate;
    }

    /// <summary>
    /// Размещение комнаты, если комната не перекрывает, то возвращает истинну
    /// </summary>
    private bool PlaceRoom(Room parentRoom, Doorway doorwayParent, Room room)
    {
        Doorway doorway = GetOppositeDoorway(doorwayParent, room.doorWayList);

        //Если нету проёма противополжной ориентации
        if (doorway == null)
        {
            doorwayParent.isUnavailable = true;
            return false;
        }

        //Расчёт мировых координат родительского прохода
        Vector2Int parentDoorwayPosition = parentRoom.lowerBounds + doorwayParent.position - parentRoom.templateLowerBounds;
        Vector2Int adjjusment = Vector2Int.zero;

        switch (doorway.orientation)
        {
            case Orientation.north:
                adjjusment = new Vector2Int(0, -1);
                break;
            case Orientation.south:
                adjjusment = new Vector2Int(0, 1);
                break;
            case Orientation.east:
                adjjusment = new Vector2Int(-1, 0);
                break;
            case Orientation.west:
                adjjusment = new Vector2Int(1, 0);
                break;
            case Orientation.none:
                break;
            default:
                break;
        }

        //Нахождение мировых координат комнаты нижнего левого угла и верхнего правого
        room.lowerBounds = parentDoorwayPosition + adjjusment + room.templateLowerBounds - doorway.position;
        room.upperBounds = room.lowerBounds + room.templateUpperBounds - room.templateLowerBounds;

        //проверка на перекрытие комнат
        Room overLappingRroom = CheckForRoomOverlap(room);

        if (overLappingRroom == null)
        {
            //Двери подключены и не более не доступны для размещения новых комнат
            doorwayParent.isConnected = true;
            doorwayParent.isUnavailable = true;

            doorway.isConnected = true;
            doorway.isUnavailable = true;

            return true;
        }
        else
        {
            doorwayParent.isUnavailable = true;
            return false;
        }
    }

    /// <summary>
    /// Возвращает дверной проём имеющий противоположную ориентацию родительского проёма
    /// </summary>
    private Doorway GetOppositeDoorway(Doorway parentDoorway, List<Doorway> doorwayList)
    {
        foreach (Doorway doorwayToCheck in doorwayList)
        {
            if (parentDoorway.orientation == Orientation.east && doorwayToCheck.orientation == Orientation.west)
                return doorwayToCheck;
            else if (parentDoorway.orientation == Orientation.west && doorwayToCheck.orientation == Orientation.east)
                return doorwayToCheck;
            else if(parentDoorway.orientation == Orientation.north && doorwayToCheck.orientation == Orientation.south)
                return doorwayToCheck;
            else if(parentDoorway.orientation == Orientation.south && doorwayToCheck.orientation == Orientation.north)
                return doorwayToCheck;
        }
        return null;
    }

    /// <summary>
    /// Проверяет не перкрывает ли размещаемая комната размещённые комнаты
    /// </summary>
    private Room CheckForRoomOverlap(Room roomToTest)
    {
        foreach (KeyValuePair<string, Room> keyValuePair in dungeonBuilderRoomDictionary)
        {
            Room room = keyValuePair.Value;

            if (room.id == roomToTest.id || !room.isPositioned)
                continue;
            if (IsOverLappingRoom(roomToTest, room))
            {
                return room;
            }
        }

        return null;
    }

    /// <summary>
    /// Проверяет пересекаются ли две комнаты между друг другом на мировых координатах
    /// Возвращает правду если перекрываются
    /// </summary>
    private bool IsOverLappingRoom(Room room1, Room room2)
    {
        bool overlappingX = IsOverLappingInterval(room1.lowerBounds.x, room1.upperBounds.x, room2.lowerBounds.x, room2.upperBounds.x);
        bool overlappingY = IsOverLappingInterval(room1.lowerBounds.y, room1.upperBounds.y, room2.lowerBounds.y, room2.upperBounds.y);

        if (overlappingX && overlappingY)
        {
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// Проверяет перекрывает ли интервал 1 интервал 2 на одной оси
    /// Возвращает правду если перекрывает
    /// </summary>
    private bool IsOverLappingInterval(int min1, int max1, int min2, int max2)
    {
        if (Mathf.Max(min1, min2) <= Mathf.Min(max1, max2))
        {
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// полученте случайного шаблона комнаты с помощью типа узла
    /// </summary>
    private RoomTemplateSO GetRandomRoomTemplate(RoomNodeTypeSO roomNodeType)
    {
        List<RoomTemplateSO> matchingRoomTeplateList = new List<RoomTemplateSO>();

        foreach (RoomTemplateSO roomTemplate in roomTemplateList)
        {
            if (roomTemplate.roomNodeType == roomNodeType)
            {
                matchingRoomTeplateList.Add(roomTemplate);
            }
        }

        if (matchingRoomTeplateList.Count == 0)
        {
            return null;
        }
        else
        {
            return matchingRoomTeplateList[Random.Range(0, matchingRoomTeplateList.Count)];
        }
    }

    /// <summary>
    /// Возвращает неиспользуемые дверные проёмы
    /// </summary>
    private IEnumerable<Doorway> GetUnconnectedAvailableDoorways(List<Doorway> roomDoorwayList)
    {
        foreach (Doorway doorway in roomDoorwayList)
        {
            if (!doorway.isConnected && !doorway.isUnavailable)
            {
                yield return doorway;
            }
        }
    }

    /// <summary>
    /// Создание комнаты на основе шаблона
    /// Возвращает комнату
    /// </summary>
    private Room CreateRroomFromRoomTemplate(RoomTemplateSO roomTemplate, RoomNodeSO roomNode)
    {
        Room room = new Room();

        room.templateId = roomTemplate.guid;
        room.id = roomNode.id;
        room.prefab = roomTemplate.prefab;
        room.roomNodeType = roomTemplate.roomNodeType;
        room.lowerBounds = roomTemplate.lowerBounds;
        room.upperBounds = roomTemplate.upperBounds;
        room.spawnPositionArray = roomTemplate.spawnPosArray;
        room.templateLowerBounds = roomTemplate.lowerBounds;
        room.templateUpperBounds = roomTemplate.upperBounds;

        room.childRoomIdList = CopyStringList(roomNode.childRoomNodeIDList);
        room.doorWayList = CopyDoorwayList(roomTemplate.doorwaysList);

        room.enemiesByLevelList = roomTemplate.enemiesByLevelList;
        room.roomLevelEnemySpawnParametersList = roomTemplate.roomEnemySpawnParametersList;

        if (roomNode.parentRoomNodeIDList.Count == 0)
        {
            room.parentRoomId = "";
            room.isPreviouslyVisited = true;

            GameManager.Instance.SetCurrentRoom(room);
        }
        else
        {
            room.parentRoomId = roomNode.parentRoomNodeIDList[0];
        }

        if (room.roomNodeType.isBossRoom)
        {
            Room corridorPreviousToBoss = dungeonBuilderRoomDictionary[room.parentRoomId];
            Room roomPreviousToBoss = dungeonBuilderRoomDictionary[dungeonBuilderRoomDictionary[room.parentRoomId].parentRoomId];
            roomPreviousToBoss.isPreviouslyToBoss = true;
            corridorPreviousToBoss.isPreviouslyCorridorToBoss = true;
        }
        // If there are no enemies to spawn then default the room to be clear of enemies
        if (room.GetNumberOfEnemiesToSpawn(GameManager.Instance.GetCurrentDungeonLevel()) == 0)
        {
            room.isClearedOfEnemies = true;
        }

        return room;
    }

    /// <summary>
    /// Выбирает случайный граф комнат из списка графов
    /// </summary>
    private RoomNodeGraphSO SelectRandomNodeGraph(List<RoomNodeGraphSO> roomNodeGraphList)
    {
        if (roomNodeGraphList.Count > 0)
        {
            return roomNodeGraphList[Random.Range(0, roomNodeGraphList.Count)];
        }
        else
        {
            Debug.Log("Список графов пуст");
            return null;
        }
    }

    /// <summary>
    /// Копирование списка
    /// Возвращает список
    /// </summary>
    private List<string> CopyStringList(List<string> oldStringList)
    {
        List<string> newStringList = new List<string>();

        foreach (string value in oldStringList)
        {
            newStringList.Add(value);
        }

        return newStringList;
    }

    /// <summary>
    /// Создаёт копию списка Dorway
    /// </summary>
    private List<Doorway> CopyDoorwayList(List<Doorway> oldDoorwayList)
    {
        List<Doorway> newDoorwayList = new List<Doorway>();

        foreach (Doorway doorway in oldDoorwayList)
        {
            Doorway newDoorway = new Doorway();

            newDoorway.position = doorway.position;
            newDoorway.orientation = doorway.orientation;
            newDoorway.doorPrefab = doorway.doorPrefab;
            newDoorway.isConnected = doorway.isConnected;
            newDoorway.isUnavailable = doorway.isUnavailable;
            newDoorway.doorwayStartCopyPos = doorway.doorwayStartCopyPos;
            newDoorway.doorwayCopyTileWidth = doorway.doorwayCopyTileWidth;
            newDoorway.doorwayCopyTileHeight = doorway.doorwayCopyTileHeight;

            newDoorwayList.Add(newDoorway);
        }

        return newDoorwayList;
    }

    /// <summary>
    /// Создание комнаты в сцене на основе префаба
    /// </summary>
    private void InstiateRoomgameObjects()
    {
        foreach (KeyValuePair<string, Room> keyValuePair in dungeonBuilderRoomDictionary)
        {
            Room room = keyValuePair.Value;

            Vector3 roomPosition = new Vector3(room.lowerBounds.x - room.templateLowerBounds.x, 
                room.lowerBounds.y - room.templateLowerBounds.y, 0f);

            //Создание комнаты на сцене
            GameObject roomGameObject = Instantiate(room.prefab, roomPosition, Quaternion.identity, transform);

            InstantiatedRoom instatiatedRoom = roomGameObject.GetComponentInChildren<InstantiatedRoom>();

            instatiatedRoom.room = room;
            instatiatedRoom.Initialise(roomGameObject);

            room.instantiatedRoom = instatiatedRoom;
        }
    }

    /// <summary>
    /// Получить шаблон комнаты из словаря по id шаблона
    /// </summary>
    public RoomTemplateSO GetRoomTeplate(string roomTemplateID)
    {
        if (roomTemplateDictionary.TryGetValue(roomTemplateID, out RoomTemplateSO roomTemplate))
            return roomTemplate;
        else
            return null;
    }

    /// <summary>
    /// Получить комнату по id из словаря
    /// </summary>
    public Room GetRoomByRoomID(string roomId)
    {
        if (dungeonBuilderRoomDictionary.TryGetValue(roomId, out Room room))
            return room;
        else
            return null;
    }

    /// <summary>
    /// Удаляет все gameobject комнты и очищает словарь dungeonBuilderRoomDictionary
    /// </summary>
    private void ClearDungeon()
    {
        if (dungeonBuilderRoomDictionary.Count > 0)
        {
            foreach (KeyValuePair<string, Room> keyValuePair in dungeonBuilderRoomDictionary)
            {
                Room room = keyValuePair.Value;
                if (room.instantiatedRoom != null)
                {
                    Destroy(room.instantiatedRoom.gameObject);
                }
            }

            dungeonBuilderRoomDictionary.Clear();
        }
    }
}
