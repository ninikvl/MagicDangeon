using UnityEngine;

[RequireComponent(typeof(Enemy))]
[DisallowMultipleComponent]
public class AnimateEnemy : MonoBehaviour
{
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    private void OnEnable()
    {
        enemy.movementToPositionEvent.OnMovementToPosition += MovementToPositionEvent_OnMovementToPosition;

        enemy.stayEvent.OnStay += StayEvent_OnStay;

        enemy.aimWeaponEvent.OnWeaponAim += AimWeaponEvent_OnWeaponAim;
    }

    private void OnDisable()
    {
        enemy.movementToPositionEvent.OnMovementToPosition -= MovementToPositionEvent_OnMovementToPosition;

        enemy.stayEvent.OnStay -= StayEvent_OnStay;

        enemy.aimWeaponEvent.OnWeaponAim -= AimWeaponEvent_OnWeaponAim;
    }

    /// <summary>
    /// Обработчик события OnWeaponAim
    /// </summary>
    private void AimWeaponEvent_OnWeaponAim(AimWeaponEvent aimWeaponEvent, AimWeaponEventArgs aimWeaponEventArgs)
    {
        InitialiseAimAnimationParameters();
        SetAimWeaponAnimationParameters(aimWeaponEventArgs.aimDirection);
    }

    /// <summary>
    /// Обработчик события OnMovementToPosition
    /// </summary>
    private void MovementToPositionEvent_OnMovementToPosition(MovementToPositionEvent movementToPositionEvent, MovementToPositionArgs movementToPositionArgs)
    {
        SetMovementAnimationParameters();
    }

    /// <summary>
    /// Обработчик события OnStay
    /// </summary>
    private void StayEvent_OnStay(StayEvent idleEvent)
    {
        SetIdleAnimationParameters();
    }

    /// <summary>
    /// Инициализация параметров анимации прицеливания
    /// </summary>
    private void InitialiseAimAnimationParameters()
    {
        enemy.animator.SetBool(Settings.aimUp, false);
        enemy.animator.SetBool(Settings.aimUpRight, false);
        enemy.animator.SetBool(Settings.aimUpLeft, false);
        enemy.animator.SetBool(Settings.aimRight, false);
        enemy.animator.SetBool(Settings.aimLeft, false);
        enemy.animator.SetBool(Settings.aimDown, false);
    }

    /// <summary>
    /// установить параметры анимации движения
    /// </summary>
    private void SetMovementAnimationParameters()
    {
        enemy.animator.SetBool(Settings.isStay, false);
        enemy.animator.SetBool(Settings.isMoving, true);
    }

    /// <summary>
    ///  установить параметр анимации стояниия
    /// </summary>
    private void SetIdleAnimationParameters()
    {
        enemy.animator.SetBool(Settings.isMoving, false);
        enemy.animator.SetBool(Settings.isStay, true);
    }

    /// <summary>
    /// установить параметры анимации прицеливания
    /// </summary>
    private void SetAimWeaponAnimationParameters(AimDirection aimDirection)
    {
        switch (aimDirection)
        {
            case AimDirection.Up:
                enemy.animator.SetBool(Settings.aimUp, true);
                break;

            case AimDirection.UpRight:
                enemy.animator.SetBool(Settings.aimUpRight, true);
                break;

            case AimDirection.UpLeft:
                enemy.animator.SetBool(Settings.aimUpLeft, true);
                break;

            case AimDirection.Right:
                enemy.animator.SetBool(Settings.aimRight, true);
                break;

            case AimDirection.Left:
                enemy.animator.SetBool(Settings.aimLeft, true);
                break;

            case AimDirection.Down:
                enemy.animator.SetBool(Settings.aimDown, true);
                break;
        }
    }
}