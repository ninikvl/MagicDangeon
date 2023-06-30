using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[DisallowMultipleComponent]
[RequireComponent(typeof(BoxCollider2D))]
public class InstantiatedRoom : MonoBehaviour
{
    [HideInInspector] public Room room;
    [HideInInspector] public Grid grid;
    [HideInInspector] public Tilemap groundTilemap;
    [HideInInspector] public Tilemap decoration1Tilemap;
    [HideInInspector] public Tilemap decoration2Tilemap;
    [HideInInspector] public Tilemap frontTilemap;
    [HideInInspector] public Tilemap collisionTilemap;
    [HideInInspector] public Tilemap minimapTilemap;
    [HideInInspector] public Tilemap ignoreAmmoTilemap;
    [HideInInspector] public Bounds roomColliderBounds;
    [HideInInspector] public int[,] aStarMovementPenalty;  //  массив для хранения штрафов за перемещение из tilemaps, которые будут использоваться при поиске пути AStar
    [HideInInspector] public int[,] aStarItemObstacles; // используется для сохранения положения подвижных предметов, которые являются препятствиями
    [HideInInspector] public List<MoveItem> moveableItemsList = new List<MoveItem>();


    private BoxCollider2D boxCollider2D;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();

        roomColliderBounds = boxCollider2D.bounds;
    }

    private void Start()
    {
        UpdateMoveableObstacles();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == Settings.playerTag && room != GameManager.Instance.GetCurrentRoom())
        {
            this.room.isPreviouslyVisited = true;
            StaticEventHandler.CallRoomChangedEvent(room);
            //Minimap minimap = new Minimap();
            //minimap.SetPositionBossMinimapIcon();

        }

        //Minimap.SetPositionBossMinimapIcon();
        //Если previousToBoss то на крате иконка босса
    }

    /// <summary>
    /// Инициализация созданной комнаты
    /// </summary>
    public void Initialise(GameObject roomGameObject)
    {
        PopulateTilemapMemberVariables(roomGameObject);
        BlockOffUnusedDoorWays();
        AddDoorsToRooms();
        DisableCollisionTilemapRenderer();
        AddObstaclesAndPreferredPaths();
        CreateItemObstaclesArray();

       
    }

    /// <summary>
    /// Заролняет переменныe tilemap и grid
    /// </summary>
    private void PopulateTilemapMemberVariables(GameObject roomGameObject)
    {
        grid = roomGameObject.GetComponentInChildren<Grid>();

        Tilemap[] tilemapArray = roomGameObject.GetComponentsInChildren<Tilemap>();

        foreach (Tilemap tilemap in tilemapArray)
        {
            if (tilemap.gameObject.tag == "groundTilemap")
            {
                groundTilemap = tilemap;
            }
            if (tilemap.gameObject.tag == "decoration1Tilemap")
            {
                decoration1Tilemap = tilemap;
            }
            if (tilemap.gameObject.tag == "decoration2Tilemap")
            {
                decoration2Tilemap = tilemap;
            }
            if (tilemap.gameObject.tag == "frontTilemap")
            {
                frontTilemap = tilemap;
            }
            if (tilemap.gameObject.tag == "collisionTilemap")
            {
                collisionTilemap = tilemap;
            }
            if (tilemap.gameObject.tag == "minimapTilemap")
            {
                minimapTilemap = tilemap;
            }
            if (tilemap.gameObject.tag == "ignoreAmmoTilemap")
            {
                ignoreAmmoTilemap = tilemap;
            }
        }
    }

    /// <summary>
    /// Блокировка всех не испольщуемых проёмов на всех Tilemap
    /// </summary>
    private void BlockOffUnusedDoorWays()
    {
        foreach (Doorway doorway in room.doorWayList)
        {
            if (doorway.isConnected)
                continue;

            if (collisionTilemap != null)
            {
                BlockDoorwayOnTilemapLayer(collisionTilemap, doorway);
            }
            if (minimapTilemap != null)
            {
                BlockDoorwayOnTilemapLayer(minimapTilemap, doorway);
            }
            if (groundTilemap != null)
            {
                BlockDoorwayOnTilemapLayer(groundTilemap, doorway);
            }
            if (decoration1Tilemap != null)
            {
                BlockDoorwayOnTilemapLayer(decoration1Tilemap, doorway);
            }
            if (decoration2Tilemap != null)
            {
                BlockDoorwayOnTilemapLayer(decoration2Tilemap, doorway);
            }
            if (frontTilemap != null)
            {
                BlockDoorwayOnTilemapLayer(frontTilemap, doorway);
            }
            if (ignoreAmmoTilemap != null)
            {
                BlockDoorwayOnTilemapLayer(ignoreAmmoTilemap, doorway);
            }
        }
    }

    /// <summary>
    /// Блокирует дверной проём на Tilemap
    /// </summary>
    private void BlockDoorwayOnTilemapLayer (Tilemap tilemap, Doorway doorway)
    {
        switch (doorway.orientation)
        {
            case Orientation.north:
            case Orientation.south:
                BlockDoorwayVertically(tilemap, doorway);
                break;
            case Orientation.east:
            case Orientation.west:
                BlockDoorwayHorizontally(tilemap, doorway);
                break;

            case Orientation.none:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Блокирование вертикальных проёмов
    /// </summary>
    private void BlockDoorwayVertically(Tilemap tilemap, Doorway doorway)
    {
        Vector2Int startPos = doorway.doorwayStartCopyPos;

        for (int xPos = 0; xPos < doorway.doorwayCopyTileWidth; xPos++)
        {
            for (int yPos = 0; yPos < doorway.doorwayCopyTileHeight; yPos++)
            {
                Matrix4x4 transformMatrix = tilemap.GetTransformMatrix(new Vector3Int(startPos.x + xPos, startPos.y - yPos, 0));
                tilemap.SetTile(new Vector3Int(startPos.x + 1 + xPos, startPos.y - yPos, 0),
                    tilemap.GetTile(new Vector3Int(startPos.x + xPos, startPos.y - yPos, 0)));
                tilemap.SetTransformMatrix(new Vector3Int(startPos.x + 1 + xPos, startPos.y - yPos, 0), transformMatrix);
            }
        }
    }

    /// <summary>
    /// Блокирование горизонтальных проёмов
    /// </summary>
    private void BlockDoorwayHorizontally(Tilemap tilemap, Doorway doorway)
    {
        Vector2Int startPos = doorway.doorwayStartCopyPos;
        for (int yPos = 0; yPos < doorway.doorwayCopyTileHeight; yPos++)
        {
            for (int xPos = 0; xPos < doorway.doorwayCopyTileWidth; xPos++)
            {
                Matrix4x4 transformMatrix = tilemap.GetTransformMatrix(new Vector3Int(startPos.x + xPos, startPos.y - yPos, 0));

                tilemap.SetTile(new Vector3Int(startPos.x + xPos, startPos.y - 1 - yPos, 0),
                    tilemap.GetTile(new Vector3Int(startPos.x + xPos, startPos.y - yPos, 0)));
                tilemap.SetTransformMatrix(new Vector3Int(startPos.x + xPos, startPos.y - 1 - yPos, 0), transformMatrix);
            }
        }

    }

    /// <summary>
    /// Обновить препятствия, используемые AStar при поиске пути.
    /// </summary>
    private void AddObstaclesAndPreferredPaths()
    {
        // этот массив будет заполнен стеновыми препятствиями
        aStarMovementPenalty = new int[room.templateUpperBounds.x - room.templateLowerBounds.x + 1,
            room.templateUpperBounds.y - room.templateLowerBounds.y + 1];

        for (int x = 0; x < (room.templateUpperBounds.x - room.templateLowerBounds.x + 1); x++)
        {
            for (int y = 0; y < (room.templateUpperBounds.y - room.templateLowerBounds.y + 1); y++)
            {
                // установить штраф за перемещение по умолчанию для квадратов сетки
                aStarMovementPenalty[x, y] = Settings.defaultAStarMovementPenalty;

                // Добавьте препятствия для столкновения плиток, по которым враг не сможет пройти
                TileBase tile = collisionTilemap.GetTile(new Vector3Int(x + room.templateLowerBounds.x, y + room.templateLowerBounds.y, 0));

                foreach (TileBase collisionTile in GameResources.Instance.enemyUnwalkableCollisionTilesArray)
                {
                    if (tile == collisionTile)
                    {
                        aStarMovementPenalty[x, y] = 0;
                        break;
                    }
                }

                TileBase tileIgnoreAmmo = ignoreAmmoTilemap.GetTile(new Vector3Int(x + room.templateLowerBounds.x, y + room.templateLowerBounds.y, 0));
                foreach (TileBase ignoreAmmoTile in GameResources.Instance.enemyUnwalkableIgnoreAmmoTilesArray)
                {
                    if (tileIgnoreAmmo == ignoreAmmoTile)
                    {
                        aStarMovementPenalty[x, y] = 0;
                        break;
                    }
                }

                // добавить предпочтительный путь для врагов (1 - предпочтительное значение пути, значение по умолчанию для
                // местоположение в сетке указано в настройках).
                if (tile == GameResources.Instance.preferredEnemyPathTile)
                {
                    aStarMovementPenalty[x, y] = Settings.preferredPathAStarMovementPenalty;
                }

            }
        }

    }

    /// <summary>
    /// Добавление двери, если комната не кориидор
    /// </summary>
    private void AddDoorsToRooms()
    {
        //if (room.roomNodeType.isCorridorEW || room.roomNodeType.isCorridorNs)
        //return;
        if (room.isPreviouslyCorridorToBoss)
            ReplaceDoorsPrefabsToBossDoors();

        foreach (Doorway doorway in room.doorWayList)
        {
            if (doorway.doorPrefab != null && doorway.isConnected)
            {
                float tileDistance = Settings.tileSizePixel / Settings.pixelPerUnit;
                GameObject door = null;

                switch (doorway.orientation)
                {
                    case Orientation.north:
                        door = Instantiate(doorway.doorPrefab, gameObject.transform);
                        door.transform.localPosition = new Vector3(doorway.position.x + tileDistance / 2f, doorway.position.y + tileDistance, 0f);
                        break;

                    case Orientation.south:
                        door = Instantiate(doorway.doorPrefab, gameObject.transform);
                        door.transform.localPosition = new Vector3(doorway.position.x + tileDistance / 2f, doorway.position.y, 0f);
                        break;

                    case Orientation.east:
                        door = Instantiate(doorway.doorPrefab, gameObject.transform);
                        door.transform.localPosition = new Vector3(doorway.position.x + tileDistance, doorway.position.y + tileDistance * 1.5f, 0f);
                        break;

                    case Orientation.west:
                        door = Instantiate(doorway.doorPrefab, gameObject.transform);
                        door.transform.localPosition = new Vector3(doorway.position.x, doorway.position.y + tileDistance * 1.5f, 0f);
                        break;
                }
            }
        }

        room.doorArray = GetComponentsInChildren<Door>();
    }

    /// <summary>
    /// Отключение визуализации у слоя с коллизией
    /// </summary>
    private void DisableCollisionTilemapRenderer()
    {
        collisionTilemap.gameObject.GetComponent<TilemapRenderer>().enabled = false;
        ignoreAmmoTilemap.gameObject.GetComponent<TilemapRenderer>().enabled = false;
        //TEST
        //minimapTilemap.gameObject.GetComponent<TilemapRenderer>().enabled = false;
    }

    /// <summary>
    /// Закрытие дверей
    /// </summary>
    public void LockDoors()
    {
        Door[] doorArray = GetComponentsInChildren<Door>();

        // триггер закрытия дверей
        foreach (Door door in doorArray)
        {
            door.LockDoor();
        }

        // Отключить триггер коллайдер
        DisableRoomCollider();
    }

    /// <summary>
    /// Открыть двери комнаты
    /// </summary>
    public void UnlockDoors(float doorUnlockDelay)
    {
        StartCoroutine(UnlockDoorsRoutine(doorUnlockDelay));
    }

    /// <summary>
    /// Открытие дверей Coroutine
    /// </summary>
    private IEnumerator UnlockDoorsRoutine(float doorUnlockDelay)
    {
        if (doorUnlockDelay > 0f)
            yield return new WaitForSeconds(doorUnlockDelay);

        Door[] doorArray = GetComponentsInChildren<Door>();

        // активировать открытие дверей
        foreach (Door door in doorArray)
        {
            door.UnlockDoor();
        }

        // Включить триггерный коллайдер
        EnableRoomCollider();
    }

    /// <summary>
    /// отключить коллайдер room , который используется для срабатывания, когда игрок входит в комнату
    /// </summary>
    public void DisableRoomCollider()
    {
        boxCollider2D.enabled = false;
    }

    /// <summary>
    /// включить коллайдер room , который используется для срабатывания, когда игрок входит в комнату
    /// </summary>
    public void EnableRoomCollider()
    {
        boxCollider2D.enabled = true;
    }


    /// <summary>
    /// Замена префабов дверей на префабы дверей босса
    /// </summary>
    private void ReplaceDoorsPrefabsToBossDoors()
    {
        foreach (Doorway doorway in room.doorWayList)
        {
            switch (doorway.orientation)
            {
                case Orientation.north:
                    doorway.doorPrefab = GameResources.Instance.doorNSBossPrefab;
                    break;
                case Orientation.south:
                    doorway.doorPrefab = GameResources.Instance.doorNSBossPrefab;
                    break;
                case Orientation.east:
                    doorway.doorPrefab = GameResources.Instance.doorEWBossPrefab;
                    break;
                case Orientation.west:
                    doorway.doorPrefab = GameResources.Instance.doorEWBossPrefab;
                    break;
            }
        }
    }

    /// <summary>
    /// обновить набор подвижных препятствий
    /// </summary>
    public void UpdateMoveableObstacles()
    {
        InitializeItemObstaclesArray();

        foreach (MoveItem moveItem in moveableItemsList)
        {
            Vector3Int colliderBoundsMin = grid.WorldToCell(moveItem.boxCollider2D.bounds.min);
            Vector3Int colliderBoundsMax = grid.WorldToCell(moveItem.boxCollider2D.bounds.max);

            //  добавьть границы коллайдера подвижных элементов к массиву препятствий
            for (int i = colliderBoundsMin.x; i <= colliderBoundsMax.x; i++)
            {
                for (int j = colliderBoundsMin.y; j <= colliderBoundsMax.y; j++)
                {
                    aStarItemObstacles[i - room.templateLowerBounds.x, j - room.templateLowerBounds.y] = 0;
                }
            }
        }
    }

    /// <summary>
    /// Создать массив препятствий для элементов
    /// </summary>
    private void CreateItemObstaclesArray()
    {
        // этот массив будет заполнен во время игрового процесса подвижными препятствиями
        aStarItemObstacles = new int[room.templateUpperBounds.x - room.templateLowerBounds.x + 1, room.templateUpperBounds.y - room.templateLowerBounds.y + 1];
    }

    /// <summary>
    /// Инициализировать массив препятствий элемента со значениями штрафа для перемещение AStar по умолчанию
    /// </summary>
    private void InitializeItemObstaclesArray()
    {
        for (int x = 0; x < (room.templateUpperBounds.x - room.templateLowerBounds.x + 1); x++)
        {
            for (int y = 0; y < (room.templateUpperBounds.y - room.templateLowerBounds.y + 1); y++)
            {
                // установить штраф за перемещение по умолчанию для квадратов сетки
                aStarItemObstacles[x, y] = Settings.defaultAStarMovementPenalty;
            }
        }
    }
}
