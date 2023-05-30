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
        //�������� �� ������
        player.stayEvent.OnStay += StayEvent_OnStay;
        player.aimWeaponEvent.OnWeaponAim += AimWeaponEvent_OnWeaponEvent;
        player.movementByVelocityEvent.OnMovementByVelocity += MovementByVelocityEvent_OnMovementByVelocity;
        player.movementToPositionEvent.OnMovementToPosition += MovementToPositionEvent_OnMovementToPosition;
    }

    private void OnDisable()
    {
        //�������� �� ������
        player.stayEvent.OnStay -= StayEvent_OnStay;
        player.aimWeaponEvent.OnWeaponAim -= AimWeaponEvent_OnWeaponEvent;
        player.movementByVelocityEvent.OnMovementByVelocity -= MovementByVelocityEvent_OnMovementByVelocity;
        player.movementToPositionEvent.OnMovementToPosition -= MovementToPositionEvent_OnMovementToPosition;
    }

    /// <summary>
    /// ��������� �������� �� ����� ������
    /// </summary>
    private void MovementToPositionEvent_OnMovementToPosition(MovementToPositionEvent movementToPositionEvent, 
        MovementToPositionArgs movementToPositionArgs)
    {
        InitializeAimAnimationParameters();
        InitializeBlinkAnimationParameters();
        SetMovementToPisitionAnimationParameters(movementToPositionArgs);
    }

    /// <summary>
    /// ��������� �������� �� ����� ��������
    /// </summary>
    private void MovementByVelocityEvent_OnMovementByVelocity(MovementByVelocityEvent movementByVelocityEvent,
        MovementByVelocityArgs movementByVelocityArgs)
    {
        SetMovementAnimationParameters();
        InitializeBlinkAnimationParameters();
    }

    /// <summary>
    /// ���������� �������� �� ����� ������������
    /// </summary>
    private void StayEvent_OnStay(StayEvent stayEvent)
    {
        SetStayAnimationParameters();
        InitializeBlinkAnimationParameters();
    }

    /// <summary>
    /// ���������� �������� �� ����� �������� ����
    /// </summary>
    private void AimWeaponEvent_OnWeaponEvent(AimWeaponEvent aimWeaponEvent, AimWeaponEventArgs aimWeaponEventArgs)
    {
        InitializeAimAnimationParameters();
        InitializeBlinkAnimationParameters();
        SetAimWeaponAnimationparameters(aimWeaponEventArgs.aimDirection);
    }

    /// <summary>
    /// ��������� ���������� �������� ������������
    /// </summary>
    private void SetStayAnimationParameters()
    {
        player.animator.SetBool(Settings.isMoving, false);
        player.animator.SetBool(Settings.isStay, true);
    }

    /// <summary>
    /// ������������� ���������� �������� ������������ ����������� �������
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
    /// ������������� ���������� �������� ������
    /// </summary>
    private void InitializeBlinkAnimationParameters()
    {
        player.animator.SetBool(Settings.BlinkDown, false);
        player.animator.SetBool(Settings.BlinkRight, false);
        player.animator.SetBool(Settings.BlinkLeft, false);
        player.animator.SetBool(Settings.BlinkUP, false);
    }

    /// <summary>
    /// ��������� ����������� ������������
    /// </summary>
    private void SetMovementAnimationParameters()
    {
        player.animator.SetBool(Settings.isMoving, true);
        player.animator.SetBool(Settings.isStay, false);
    }

    /// <summary>
    /// ��������� ���������� �������� ������
    /// </summary>
    private void SetMovementToPisitionAnimationParameters(MovementToPositionArgs movementToPositionArgs)
    {
        if (movementToPositionArgs.isBlinking)
        {
            if (movementToPositionArgs.moveDirection.x > 0f)
            {
                player.animator.SetBool(Settings.BlinkRight, true);
            }
            else if (movementToPositionArgs.moveDirection.x < 0f)
            {
                player.animator.SetBool(Settings.BlinkLeft, true);
            }
            else if (movementToPositionArgs.moveDirection.y > 0f)
            {
                player.animator.SetBool(Settings.BlinkUP, true);
            }
            else if (movementToPositionArgs.moveDirection.y < 0f)
            {
                player.animator.SetBool(Settings.BlinkDown, true);
            }
        }
    }

    /// <summary>
    /// ��������� ���������� �������� ������������ ����������� �������
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
