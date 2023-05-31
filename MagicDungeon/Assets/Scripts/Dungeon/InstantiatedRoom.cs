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


    private BoxCollider2D boxCollider2D;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();

        roomColliderBounds = boxCollider2D.bounds;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == Settings.playerTag && room != GameManager.Instance.GetCurrentRoom())
        {
            this.room.isPreviouslyVisited = true;
            StaticEventHandler.CallRoomChangedEvent(room);
        }
        
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
        
        foreach (string idChild in room.childRoomIdList)
        {
            
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
    /// Добавление двери, если комната не кориидор
    /// </summary>
    private void AddDoorsToRooms()
    {
        //if (room.roomNodeType.isCorridorEW || room.roomNodeType.isCorridorNs)
            //return;

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
    }

    /// <summary>
    /// Отключение визуализации уу слоя с коллизией
    /// </summary>
    private void DisableCollisionTilemapRenderer()
    {
        collisionTilemap.gameObject.GetComponent<TilemapRenderer>().enabled = false;
        ignoreAmmoTilemap.gameObject.GetComponent<TilemapRenderer>().enabled = false;
        //TEST
        //minimapTilemap.gameObject.GetComponent<TilemapRenderer>().enabled = false;
    }
}
 