using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDetails_", menuName = "Scriptable Objects/�����/������������ ������")]
public class PlayerDetailsSO : ScriptableObject
{
    #region Header Player Base Details
    [Space(10)]
    [Header("������� �������� ������")]
    #endregion
    #region Tooltip
    [Tooltip("��� ���������")]
    #endregion
    public string playerCharacterName;

    #region Tooltip
    [Tooltip("������ ���������")]
    #endregion
    public GameObject playerPrefab;

    //��� ��������
    #region Tooltip
    [Tooltip("������ ���������")]
    #endregion
    public RuntimeAnimatorController runtimeAnimatorController;

    #region Header Player Health
    [Space(10)]
    [Header("�������� ������")]
    #endregion
    #region Tooltip
    [Tooltip("��������� �������� ������")]
    #endregion
    public int playerHealthAmount;
    #region Tooltip
    [Tooltip("Select if has immunity period immediately after being hit.  If so specify the immunity time in seconds in the other field")]
    #endregion
    public bool isImmuneAfterHit = false;
    #region Tooltip
    [Tooltip("Immunity time in seconds after being hit")]
    #endregion
    public float hitImmunityTime;

    #region Header Weapon
    [Space(10)]
    [Header("������ ���������")]
    #endregion
    #region Tooltip
    [Tooltip("��������� ������ ���������")]
    #endregion
    public WeaponDetailsSO startingWeapon;
    #region Tooltip
    [Tooltip("������ ���������� ������ ���������")]
    #endregion
    public List<WeaponDetailsSO> startingWeaponList ;

    #region Header Other
    [Space(10)]
    [Header("������")]
    #endregion
    #region Tooltip
    [Tooltip("������ ������ ��� ����������� �� ���������")]
    #endregion
    public Sprite playerMiniMapIcon;

    #region Tooltip
    [Tooltip("������ ���� ������")]
    #endregion
    public Sprite playerHandSprite;

    #region Vakidation
#if UNITY_EDITOR

    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(playerCharacterName), playerCharacterName);
        HelperUtilities.ValidateCheckNullValue(this, nameof(playerPrefab), playerPrefab);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(playerHealthAmount), playerHealthAmount, false);
        HelperUtilities.ValidateCheckNullValue(this, nameof(playerMiniMapIcon), playerMiniMapIcon);
        HelperUtilities.ValidateCheckNullValue(this, nameof(playerHandSprite), playerHandSprite);
        HelperUtilities.ValidateCheckNullValue(this, nameof(runtimeAnimatorController), runtimeAnimatorController);
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(startingWeaponList), startingWeaponList);
    }

#endif
    #endregion

}
