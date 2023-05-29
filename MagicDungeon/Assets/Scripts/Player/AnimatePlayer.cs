using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[DisallowMultipleComponent]
public class AnimatePlayer : MonoBehaviour
{
    private Player player;
    

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void OnEnable()
    {
        //Подписки на эвенты
        player.stayEvent.OnStay += StayEvent_OnStay;
        player.aimWeaponEvent.OnWeaponAim += AimWeaponEvent_OnWeaponEvent;
        player.movementByVelocityEvent.OnMovementByVelocity += MovementByVelocityEvent_OnMovementByVelocity;
    }

    private void OnDisable()
    {
        //Подписки на эвенты
        player.stayEvent.OnStay -= StayEvent_OnStay;
        player.aimWeaponEvent.OnWeaponAim -= AimWeaponEvent_OnWeaponEvent;
        player.movementByVelocityEvent.OnMovementByVelocity -= MovementByVelocityEvent_OnMovementByVelocity;
    }

    private void MovementByVelocityEvent_OnMovementByVelocity(MovementByVelocityEvent movementByVelocityEvent,
        MovementByVelocityArgs movementByVelocityArgs)
    {
        SetMovementAnimationParameters();
    }

    private void StayEvent_OnStay(StayEvent stayEvent)
    {
        SetStayAnimationParameters();
    }

    private void AimWeaponEvent_OnWeaponEvent(AimWeaponEvent aimWeaponEvent, AimWeaponEventArgs aimWeaponEventArgs)
    {
        InitializeAimAnimationParameters();
        SetAimWeaponAnimationparameters(aimWeaponEventArgs.aimDirection);
    }

    /// <summary>
    /// Установка параметров анимации передвижения
    /// </summary>
    private void SetStayAnimationParameters()
    {
        player.animator.SetBool(Settings.isMoving, false);
        player.animator.SetBool(Settings.isStay, true);
    }

    /// <summary>
    /// инициализация параметров анимации относительно направления курсора
    /// </summary>
    private void InitializeAimAnimationParameters()
    {
        player.animator.SetBool(Settings.aimUp, false);
        player.animator.SetBool(Settings.aimDown, false);
        player.animator.SetBool(Settings.aimLeft, false);
        player.animator.SetBool(Settings.aimRight, false);
        player.animator.SetBool(Settings.aimUpLeft, false);
        player.animator.SetBool(Settings.aimUpRight, false);
    }

    /// <summary>
    /// Установка параметроов передвижения
    /// </summary>
    private void SetMovementAnimationParameters()
    {
        player.animator.SetBool(Settings.isMoving, true);
        player.animator.SetBool(Settings.isStay, false);
    }

    /// <summary>
    /// Установка параметров анимации относительно направления курсора
    /// </summary>
    private void SetAimWeaponAnimationparameters(AimDirection aimDirection)
    {
        switch (aimDirection)
        {
            case AimDirection.Up:
                player.animator.SetBool(Settings.aimUp, true);
                break;
            case AimDirection.UpRight:
                player.animator.SetBool(Settings.aimUpRight, true);
                break;
            case AimDirection.UpLeft:
                player.animator.SetBool(Settings.aimUpLeft, true);
                break;
            case AimDirection.Right:
                player.animator.SetBool(Settings.aimRight, true);
                break;
            case AimDirection.Left:
                player.animator.SetBool(Settings.aimLeft, true);
                break;
            case AimDirection.Down:
                player.animator.SetBool(Settings.aimDown, true);
                break;
            default:
                break;
        }
    }
}
