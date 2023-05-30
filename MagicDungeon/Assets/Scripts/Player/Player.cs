using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

#region Require Components
[RequireComponent(typeof(SortingGroup))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(StayEvent))]
[RequireComponent(typeof(Stay))]
[RequireComponent(typeof(AimWeaponEvent))]
[RequireComponent(typeof(AimWeapon))]
[RequireComponent(typeof(PlayerControl))]
[RequireComponent(typeof(AnimatePlayer))]
[RequireComponent(typeof(MovementByVelocityEvent))]
[RequireComponent(typeof(MovementByVelocity))]
[RequireComponent(typeof(MovementToPositionEvent))]
[RequireComponent(typeof(MovementToPosition))]
[DisallowMultipleComponent]
#endregion

public class Player : MonoBehaviour
{
    [HideInInspector] public PlayerDetailsSO playerDetails;
    [HideInInspector] public Health health;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Animator animator;

    [HideInInspector] public StayEvent stayEvent;
    [HideInInspector] public MovementByVelocityEvent movementByVelocityEvent;
    [HideInInspector] public AimWeaponEvent aimWeaponEvent;
    [HideInInspector] public MovementToPositionEvent movementToPositionEvent;

    private void Awake()
    {
        //�������� �����������
        health = GetComponent<Health>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        stayEvent = GetComponent<StayEvent>();
        movementByVelocityEvent = GetComponent<MovementByVelocityEvent>();
        aimWeaponEvent = GetComponent<AimWeaponEvent>();
        movementToPositionEvent = GetComponent<MovementToPositionEvent>();
    }

    /// <summary>
    /// ������������� ������
    /// </summary>
    public void Initialize(PlayerDetailsSO playerDetails)
    {
        this.playerDetails = playerDetails;
        SetPlayerHealth();
    }

    /// <summary>
    /// ���������� ��������� �������� �������� ������ �� playerDetails SO
    /// </summary>
    private void SetPlayerHealth()
    {
        health.SetStartingHealth(playerDetails.playerHealthAmount);
    }
}
