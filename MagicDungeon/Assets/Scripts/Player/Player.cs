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
[DisallowMultipleComponent]
#endregion
public class Player : MonoBehaviour
{
    [HideInInspector] public PlayerDetailsSO playerDetails;
    [HideInInspector] public Health health;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Animator animator;

    private void Awake()
    {
        //загрузка компонентов
        health = GetComponent<Health>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Инициализация игрока
    /// </summary>
    public void Initialize(PlayerDetailsSO playerDetails)
    {
        this.playerDetails = playerDetails;
        SetPlayerHealth();
    }

    /// <summary>
    /// Установить стартовое значение здоровья игрока из playerDetails SO
    /// </summary>
    private void SetPlayerHealth()
    {
        health.SetStartingHealth(playerDetails.playerHealthAmount);
    }
}
