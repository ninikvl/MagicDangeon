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

    [HideInInspector] public GameState gameState;

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
    /// 
    /// </summary>
    private void PlayDungeonLevel(int curLevelListIndex)
    {
        //Создание уровня
        bool isDungeonBuildSucessfuly = DungeonBuilder.Instance.GenerateDungeon(dungeonLevelsList[curLevelListIndex]);
        if (!isDungeonBuildSucessfuly)
        {
            Debug.LogError("Создание уровня не удалось");
        }
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
