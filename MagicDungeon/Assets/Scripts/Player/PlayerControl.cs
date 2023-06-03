using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[DisallowMultipleComponent]
public class PlayerControl : MonoBehaviour
{
    #region Tooltip
    [Tooltip("Детали передвижения Sriptable Object")]
    #endregion
    [SerializeField] private MovementDetailsSO movementDetails;

    private Player player;
    private int currentWeaponIndex = 1;
    private float moveSpeed;
    private Coroutine playerBlinkCoroutine;
    private WaitForFixedUpdate waitForFixedUpdate;
    private bool isPlayerIsBlinking;
    private float playerBlinkCooldownTimer = 0f;

    private void Awake()
    {
        player = GetComponent<Player>();
        moveSpeed = movementDetails.GetMoveSpeed();
    }

    private void Start()
    {
        waitForFixedUpdate = new WaitForFixedUpdate();

        SetPlayerAnimationSpeed();

        SetStartingWeapon();
    }

    private void Update()
    {
        if (isPlayerIsBlinking)
            return;
        
        MovementInput();

        WeaponInput();

        PlayerBlinkCooldownTimer();
    }

    /// <summary>
    /// Установка стартового оружия
    /// </summary>
    private void SetStartingWeapon()
    {
        int index = 1;
        foreach (Weapon weapon in player.weaponList)
        {
            if (weapon.weaponDetails == player.playerDetails.startingWeapon)
            {
                SetWeaponByIndex(index);
                break;
            }
            index++;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void SetWeaponByIndex(int weaponIndex)
    {
        if (weaponIndex - 1 < player.weaponList.Count)
        {
            currentWeaponIndex = weaponIndex;
            player.setAtiveWeaponEvent.CallSetActiveWeaponEvent(player.weaponList[weaponIndex-1]);
        }
    }

    /// <summary>
    /// Установка скорости анимции от сккорости движения персонажа
    /// </summary>
    private void SetPlayerAnimationSpeed()
    {
        player.animator.speed = moveSpeed / Settings.baseSpeedForPlayerAnimations;
    }

    /// <summary>
    /// 
    /// </summary>
    private void MovementInput()
    {
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticlalMovement = Input.GetAxisRaw("Vertical");
        bool rightMouseButtonDown = Input.GetMouseButtonDown(1);
        Vector2 direction = new Vector2(horizontalMovement, verticlalMovement);

        if (horizontalMovement != 0f && verticlalMovement != 0f)
        {
            direction *= 0.7f;
        }

        if (direction != Vector2.zero)
        {
            if (!rightMouseButtonDown)
            {
                player.movementByVelocityEvent.CallMovementByVelocityEvent(direction, moveSpeed);
            }
            else if (playerBlinkCooldownTimer <= 0f)
            {
                PlayerRoll((Vector3)direction);
            }
        }
        else
        {
            player.stayEvent.CallStayEvent();
        }
    }

    /// <summary>
    /// Телепорт персонажа
    /// </summary>
    private void PlayerRoll(Vector3 direction)
    {
        playerBlinkCoroutine = StartCoroutine(PlayerBlinkCoroutine(direction));
    }

    private IEnumerator PlayerBlinkCoroutine(Vector3 direction)
    {
        float minDistance = 0.2f;
        isPlayerIsBlinking = true;

        Vector3 targetPosition = player.transform.position + (Vector3)direction * movementDetails.blinkDistance;

        while (Vector3.Distance(player.transform.position, targetPosition) > minDistance)
        {
            player.movementToPositionEvent.CallMovementToPositionEvent(targetPosition, player.transform.position, movementDetails.blinkSpeed,
                direction, isPlayerIsBlinking);

            yield return waitForFixedUpdate;
        }

        isPlayerIsBlinking = false;
        playerBlinkCooldownTimer = movementDetails.blinkColldownTime;

        player.transform.position = targetPosition;
    }

    /// <summary>
    /// 
    /// </summary>
    private void PlayerBlinkCooldownTimer()
    {
        if (playerBlinkCooldownTimer >= 0f)
        {
            playerBlinkCooldownTimer -= Time.deltaTime;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void WeaponInput()
    {
        Vector3 weaponDirection;
        float weaponAngleDegrees, playerAngleDegrees;
        AimDirection playerAimDirection;

        AimWeaponInput(out weaponDirection, out weaponAngleDegrees, out playerAngleDegrees, out playerAimDirection);

        FireWeaponInput(weaponDirection, weaponAngleDegrees, playerAngleDegrees, playerAimDirection);
    }    

    private void AimWeaponInput(out Vector3 weaponDirection, out float weaponAngleDegrees, out float playerAngleDegrees, 
        out AimDirection playerAimDirection)
    {
        Vector3 mouseWorldPosition = HelperUtilities.GetMouseWorldPosition();

        weaponDirection = (mouseWorldPosition - player.ativeWeapon.GetShootPosition());

        Vector3 playerDirection = (mouseWorldPosition - transform.position);

        weaponAngleDegrees = HelperUtilities.GetAngleFromVector(weaponDirection);

        playerAngleDegrees = HelperUtilities.GetAngleFromVector(playerDirection);

        playerAimDirection = HelperUtilities.GetAimDirection(playerAngleDegrees);

        player.aimWeaponEvent.CallAimWeaponEvent(playerAimDirection, playerAngleDegrees, weaponAngleDegrees, weaponDirection);

    }

    private void FireWeaponInput(Vector3 weaponDirection, float weaponAngleDegrees, float playerAngleDegrees, AimDirection playerAimDirection)
    {
        if (Input.GetMouseButton(0))
        {
            player.fireWeaponEvent.CallFireWeaponEvent(true, playerAimDirection, playerAngleDegrees, weaponAngleDegrees, weaponDirection);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StopPlayerRollCoroutine();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        StopPlayerRollCoroutine();
    }

    private void StopPlayerRollCoroutine()
    {
        if (playerBlinkCoroutine != null)
        {
            StopCoroutine(playerBlinkCoroutine);
            isPlayerIsBlinking = false;
        }
    }

    #region Vakidation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(movementDetails), movementDetails);
    }

#endif
    #endregion
}
