using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Minimap : MonoBehaviour
{
    #region Tooltip
    [Tooltip("Populate with the child MinimapPlayer gameobject")]
    #endregion Tooltip

    [SerializeField] private GameObject miniMapPlayer;
    [SerializeField] private  GameObject miniMapBoss;

    private Transform playerTransform;

    private void OnEnable()
    {
        StaticEventHandler.OnRoomChanged += StaticEventHandler_OnRoomChanged;
    }

    private void OnDisable()
    {
        StaticEventHandler.OnRoomChanged -= StaticEventHandler_OnRoomChanged;
    }

    private void Start()
    {
        miniMapBoss.SetActive(false);

        playerTransform = GameManager.Instance.GetPlayer().transform;

        // Populate player as cinemachine camera target
        CinemachineVirtualCamera cinemachineVirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        cinemachineVirtualCamera.Follow = playerTransform;

        // Set minimap player icon
        SpriteRenderer spriteRenderer = miniMapPlayer.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = GameManager.Instance.GetPlayerMiniMapIcon();
        }
    }

    private void Update()
    {
        // Move the minimap player to follow the player
        if (playerTransform != null && miniMapPlayer != null)
        {
            miniMapPlayer.transform.position = playerTransform.position;
        }
    }

    private void StaticEventHandler_OnRoomChanged(RoomChangedEventArgs roomChangedEventArgs)
    {
        if (roomChangedEventArgs.room.isPreviouslyToBoss)
        {
            SetPositionBossMinimapIcon();
        }
    }

    public void SetPositionBossMinimapIcon()
    {
        foreach (KeyValuePair<string, Room> keyValuePair in DungeonBuilder.Instance.dungeonBuilderRoomDictionary)
        {
            Room room = keyValuePair.Value;

            if (room.roomNodeType.isBossRoom)
            {
                miniMapBoss.transform.position = new Vector3(room.lowerBounds.x + room.templateUpperBounds.x / 2, room.lowerBounds.y + room.templateUpperBounds.y / 2, 0f);
                miniMapBoss.SetActive(true);
            }
        }
    }

    #region Validation

#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(miniMapPlayer), miniMapPlayer);
    }

#endif

    #endregion Validation   

}