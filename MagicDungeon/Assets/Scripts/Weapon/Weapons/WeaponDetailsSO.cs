using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDetails_", menuName = "Scriptable Objects/������/������ ������")]
public class WeaponDetailsSO : ScriptableObject
{
    #region Header Weapon Base Details
    [Space(10)]
    [Header("������� ������ ������")]
    #endregion Header
    #region Tooltip
    [Tooltip("�������� ������")]
    #endregion
    public string weaponName;
    #region Tooltip
    [Tooltip("������ ������")]
    #endregion
    public Sprite weaponSprite;

    #region Header Weapon Configuration
    [Space(10)]
    [Header("��������� ������")]
    #endregion Header
    #region Tooltip
    [Tooltip("����� ������ ���� ������")]
    #endregion
    public Vector3 weaponShootPosition;
    #region Tooltip
    [Tooltip("��� �����������")]
    #endregion
    public AmmoDetailsSO weaponCurrentAmmo;

    #region Header Weapon Configuration
    [Space(10)]
    [Header("��������� ������ ������")]
    #endregion Header
    #region Tooltip
    [Tooltip("����� �� ������ ����������� ��������")]
    #endregion
    public bool hasInfiniteAmmo = false;
    #region Tooltip
    [Tooltip("����� �� ������ ����������� �������")]
    #endregion
    public bool hasInfiniteClipCapacity = false;
    #region Tooltip
    [Tooltip("������� ��������")]
    #endregion
    public int weaponClipAmmoCapacity = 6;
    #region Tooltip
    [Tooltip("����� ��������")]
    #endregion
    public int weaponAmmoCapacity = 100;
    #region Tooltip
    [Tooltip("���������������� (0.1 - ��� 10 ��������� � �������)")]
    #endregion
    public float weaponFireRate = 0.2f;
    #region Tooltip
    [Tooltip("�������� ����� ���������")]
    #endregion
    public float weaponPrechargeTime = 0f;
    #region Tooltip
    [Tooltip("����� �����������")]
    #endregion
    public float weaponReloadTime = 0f;

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(weaponName), weaponName);
        HelperUtilities.ValidateCheckNullValue(this, nameof(weaponCurrentAmmo), weaponCurrentAmmo);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponFireRate), weaponFireRate, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponPrechargeTime), weaponPrechargeTime, true);

        if (!hasInfiniteAmmo)
        {
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponAmmoCapacity), weaponAmmoCapacity, false);
        }
        if (!hasInfiniteClipCapacity)
        {
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponClipAmmoCapacity), weaponClipAmmoCapacity, false);
        }
    }
#endif
    #endregion
}


