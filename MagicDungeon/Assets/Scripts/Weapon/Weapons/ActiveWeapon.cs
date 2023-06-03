using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SetAtiveWeaponEvent))]
[DisallowMultipleComponent]
public class ActiveWeapon : MonoBehaviour
{
    #region Tooltip
    [Tooltip("Заполняется от дочернего объекта Weapon")]
    #endregion
    [SerializeField] private SpriteRenderer weaponSpriteRenderer;
    #region Tooltip
    [Tooltip("Заполняется от дочернего объекта Weapon")]
    #endregion
    [SerializeField] private PolygonCollider2D weaponPolygonCollider2D;
    #region Tooltip
    [Tooltip("Заполняется от дочернего объекта Weapon")]
    #endregion
    [SerializeField] private Transform weaponShootPositionTransform;
    #region Tooltip
    [Tooltip("Заполняется от дочернего объекта Weapon")]
    #endregion
    [SerializeField] private Transform weaponEffectPositionTransform;

    private SetAtiveWeaponEvent setAtiveWeaponEvent;
    private Weapon currentWeapon;

    private void Awake()
    {
        setAtiveWeaponEvent = GetComponent<SetAtiveWeaponEvent>();
    }

    private void OnEnable()
    {
        setAtiveWeaponEvent.OnSetActiveWeapon += SetAtiveWeaponEvent_OnSetActiveWeapon;
    }

    private void OnDisable()
    {
        setAtiveWeaponEvent.OnSetActiveWeapon -= SetAtiveWeaponEvent_OnSetActiveWeapon;
    }

    private void SetAtiveWeaponEvent_OnSetActiveWeapon(SetAtiveWeaponEvent setAtiveWeaponEvent, SetAtiveWeaponArgs setAtiveWeaponArgs)
    {
        SetAtiveWeapon(setAtiveWeaponArgs.weapon);
    }

    private void SetAtiveWeapon(Weapon weapon)
    {
        currentWeapon = weapon;
        weaponSpriteRenderer.sprite = currentWeapon.weaponDetails.weaponSprite;
        if (weaponPolygonCollider2D != null && weaponSpriteRenderer.sprite != null)
        {
            List<Vector2> spritePhysicsPointsList = new List<Vector2>();
            weaponSpriteRenderer.sprite.GetPhysicsShape(0, spritePhysicsPointsList);
            weaponPolygonCollider2D.points = spritePhysicsPointsList.ToArray();
        }

        weaponShootPositionTransform.localPosition = currentWeapon.weaponDetails.weaponShootPosition;
    }

    public AmmoDetailsSO GetCurrentAmmo()
    {
        return currentWeapon.weaponDetails.weaponCurrentAmmo;
    }

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public Vector3 GetShootPosition()
    {
        return weaponShootPositionTransform.position;
    }

    public Vector3 GetShootEffectPosition()
    {
        return weaponEffectPositionTransform.position;
    }

    public void RemoveCurrentWeapon()
    {
        currentWeapon = null;
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(weaponSpriteRenderer), weaponSpriteRenderer);
        HelperUtilities.ValidateCheckNullValue(this, nameof(weaponPolygonCollider2D), weaponPolygonCollider2D);
        HelperUtilities.ValidateCheckNullValue(this, nameof(weaponShootPositionTransform), weaponShootPositionTransform);
        HelperUtilities.ValidateCheckNullValue(this, nameof(weaponEffectPositionTransform), weaponEffectPositionTransform);
    }
#endif
    #endregion

}
