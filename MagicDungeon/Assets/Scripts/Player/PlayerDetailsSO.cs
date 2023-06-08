using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDetails_", menuName = "Scriptable Objects/Игрок/Конфигурация игрока")]
public class PlayerDetailsSO : ScriptableObject
{
    #region Header Player Base Details
    [Space(10)]
    [Header("Базовые значения игрока")]
    #endregion
    #region Tooltip
    [Tooltip("Имя персонажа")]
    #endregion
    public string playerCharacterName;

    #region Tooltip
    [Tooltip("Префаб персонажа")]
    #endregion
    public GameObject playerPrefab;

    //Под Удаление
    #region Tooltip
    [Tooltip("Префаб персонажа")]
    #endregion
    public RuntimeAnimatorController runtimeAnimatorController;

    #region Header Player Health
    [Space(10)]
    [Header("Здоровье игрока")]
    #endregion
    #region Tooltip
    [Tooltip("Стартовое здоровье игрока")]
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
    [Header("Оружие персонажа")]
    #endregion
    #region Tooltip
    [Tooltip("Стартовое оружие персонажа")]
    #endregion
    public WeaponDetailsSO startingWeapon;
    #region Tooltip
    [Tooltip("Список стартового оружия персонажа")]
    #endregion
    public List<WeaponDetailsSO> startingWeaponList ;

    #region Header Other
    [Space(10)]
    [Header("Разное")]
    #endregion
    #region Tooltip
    [Tooltip("Иконка игрока для отображения на миникарте")]
    #endregion
    public Sprite playerMiniMapIcon;

    #region Tooltip
    [Tooltip("Спрайт руки игрока")]
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
