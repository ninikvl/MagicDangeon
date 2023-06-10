using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonMap : SingletonMonobehavior<DungeonMap>
{
    #region Header GameObject References
    [Space(10)]
    [Header("GameObject References")]
    #endregion
    #region Tooltip
    [Tooltip("Populate with the MinimapUI gameobject")]
    #endregion
    [SerializeField] private GameObject minimapUI;
    private Camera dungeonMapCamera;
    private Camera cameraMain;

    private void Start()
    {
        cameraMain = Camera.main;

        Transform playerTransform = GameManager.Instance.GetPlayer().transform;

        CinemachineVirtualCamera cinemachineVirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        cinemachineVirtualCamera.Follow = playerTransform;

        dungeonMapCamera = GetComponentInChildren<Camera>();
        dungeonMapCamera.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Если кнопка мыши нажата, а состояние игры - dungeonOverviewMap, то плучить комнату
        if (Input.GetMouseButtonDown(0) && GameManager.Instance.gameState == GameState.dungeonOverviewMap)
        {
            GetRoomClicked();
        }
    }

    /// <summary>
    /// Получить комнату на которую нажали на карте
    /// </summary>
    private void GetRoomClicked()
    {
        // Преобразовать положение экрана в мировое положение
        Vector3 worldPosition = dungeonMapCamera.ScreenToWorldPoint(Input.mousePosition);
        worldPosition = new Vector3(worldPosition.x, worldPosition.y, 0f);

        // Проверка, нет ли коллизий в положении курсора
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(new Vector2(worldPosition.x, worldPosition.y), 1f);

        // Проверка, является ли какой-либо из коллайдеров комнатой
        foreach (Collider2D collider2D in collider2DArray)
        {
            if (collider2D.GetComponent<InstantiatedRoom>() != null)
            {
                InstantiatedRoom instantiatedRoom = collider2D.GetComponent<InstantiatedRoom>();

                // Если выбранная комната свободна от врагов и ранее посещалась, переместите игрока в эту комнату
                if (instantiatedRoom.room.isClearedOfEnemies && instantiatedRoom.room.isPreviouslyVisited)
                {
                    // переместить игрока в комнату
                    StartCoroutine(MovePlayerToRoom(worldPosition, instantiatedRoom.room));
                }
            }
        }

    }

    /// <summary>
    /// Переместите игрока в выбранную комнату
    /// </summary>
    private IEnumerator MovePlayerToRoom(Vector3 worldPosition, Room room)
    {
        StaticEventHandler.CallRoomChangedEvent(room);

        yield return StartCoroutine(GameManager.Instance.Fade(0f, 1f, 0f, Color.black));

        ClearDungeonOverViewMap();

        GameManager.Instance.GetPlayer().playerControl.DisablePlayer();

        Vector3 spawnPosition = HelperUtilities.GetSpawnPositionToPlayer(worldPosition);

        GameManager.Instance.GetPlayer().transform.position = spawnPosition;

        yield return StartCoroutine(GameManager.Instance.Fade(1f, 0f, 1f, Color.black));

        GameManager.Instance.GetPlayer().playerControl.EnablePlayer();
    }

    /// <summary>
    /// Отображение пользовательского интерфейса обзорной карты подземелья
    /// </summary>
    public void DisplayDungeonOverViewMap()
    {
        GameManager.Instance.previousGameState = GameManager.Instance.gameState;
        GameManager.Instance.gameState = GameState.dungeonOverviewMap;

        GameManager.Instance.GetPlayer().playerControl.DisablePlayer();

        cameraMain.gameObject.SetActive(false);
        dungeonMapCamera.gameObject.SetActive(true);

        ActivateRoomsForDisplay();

        minimapUI.SetActive(false);
    }

    /// <summary>
    /// очистить пользовательский интерфейс обзорной карты подземелий
    /// </summary>
    public void ClearDungeonOverViewMap()
    {
        GameManager.Instance.gameState = GameManager.Instance.previousGameState;
        GameManager.Instance.previousGameState = GameState.dungeonOverviewMap;

        GameManager.Instance.GetPlayer().playerControl.EnablePlayer();

        cameraMain.gameObject.SetActive(true);
        dungeonMapCamera.gameObject.SetActive(false);

        minimapUI.SetActive(true);
    }

    /// <summary>
    /// Проверкка, что все комнаты активны, чтобы их можно было отобразить
    /// </summary>
    private void ActivateRoomsForDisplay()
    {
        foreach (KeyValuePair<string, Room> keyValuePair in DungeonBuilder.Instance.dungeonBuilderRoomDictionary)
        {
            Room room = keyValuePair.Value;

            room.instantiatedRoom.gameObject.SetActive(true);
        }
    }
}