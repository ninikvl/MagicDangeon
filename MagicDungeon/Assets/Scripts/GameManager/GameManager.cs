using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : SingletonMonobehavior<GameManager>
{
    #region Header Dungeon Levels
    [Space(20)]
    [Header("Уровни подземелья")]
    #endregion Header Dungeon Levels
    #region Tooltip
    [Tooltip("Список уровней подземелья")]
    #endregion Tooltip
    [SerializeField] private List<DungeonLevelSO> dungeonLevelsList;

    #region Tooltip
    [Tooltip("Индекс играемого уровня")]
    #endregion Tooltip
    [SerializeField] private  int currentDungeonLevelListIndex = 0;
    private Room currentRoom;
    private Room previosRoom;
    private PlayerDetailsSO playerDetails;
    private Player player;

    [HideInInspector] public GameState gameState;

    protected override void Awake()
    {
        base.Awake();

        playerDetails = GameResources.Instance.currentPlayer.playerDetails;

        InstatiatePlayer();
    }

    /// <summary>
    /// Создание персонажа на сцене
    /// </summary>
    private void InstatiatePlayer()
    {
        GameObject playerGameObject = Instantiate(playerDetails.playerPrefab);

        player = playerGameObject.GetComponent<Player>();

        player.Initialize(playerDetails);
    }

    private void Start()
    {
        gameState = GameState.gameStarted;
    }

    private void Update()
    {
        HandleGameState();

        // ДЛЯ ТЕСТА
        if (Input.GetKeyDown(KeyCode.R))
        {
            gameState = GameState.gameStarted;
        }
    }

    private void OnEnable()
    {
        StaticEventHandler.OnRoomChanged += StaticEventHandler_OnRoomChanged;
    }

    private void OnDisable()
    {
        StaticEventHandler.OnRoomChanged -= StaticEventHandler_OnRoomChanged;
    }

    //TEST
    private void StaticEventHandler_OnRoomChanged(RoomChangedEventArgs roomChangedEventArgs)
    {
        //SetCurrentRoom(roomChangedEventArgs.room);
    }

    /// <summary>
    /// Переключатель игровых состояний
    /// </summary>
    private void HandleGameState ()
    {
        switch (gameState)
        {
            case GameState.gameStarted:
                PlayDungeonLevel(currentDungeonLevelListIndex);
                gameState = GameState.playingLevel;
                break;
            case GameState.playingLevel:
                break;
            case GameState.engaginEnemies:
                break;
            case GameState.bossStage:
                break;
            case GameState.engaginBoss:
                break;
            case GameState.levelComplited:
                break;
            case GameState.gameWon:
                break;
            case GameState.gameLost:
                break;
            case GameState.gamePaused:
                break;
            case GameState.dungeonOverviewMap:
                break;
            case GameState.restartGame:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Установить комнату в которой находится игрок
    /// </summary>
    public void SetCurrentRoom(Room room)
    {
        previosRoom = currentRoom;
        currentRoom = room;
    }

    /// <summary>
    /// Создание уровня
    /// </summary>
    private void PlayDungeonLevel(int curLevelListIndex)
    {
        //Создание уровня
        bool isDungeonBuildSucessfuly = DungeonBuilder.Instance.GenerateDungeon(dungeonLevelsList[curLevelListIndex]);
        if (!isDungeonBuildSucessfuly)
        {
            Debug.LogError("Создание уровня не удалось");
        }

        StaticEventHandler.CallRoomChangedEvent(currentRoom);

        player.gameObject.transform.position = new Vector3((currentRoom.lowerBounds.x + currentRoom.upperBounds.x) / 2f,
            (currentRoom.lowerBounds.y + currentRoom.upperBounds.y) / 2f, 0f);

        player.gameObject.transform.position = HelperUtilities.GetSpawnPositionToPlayer(player.gameObject.transform.position);
    }

    public Room GetCurrentRoom()
    {
        return currentRoom;
    }

    public Player GetPlayer()
    {
        return player;
    }



    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(dungeonLevelsList), dungeonLevelsList);
    }

#endif
    #endregion Validation
}
