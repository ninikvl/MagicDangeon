using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomNodeType_", menuName = "Scriptable Objects/Редактор подземелья/Тип комнаты")]
public class RoomNodeTypeSO : ScriptableObject
{
    public string roomNodeTypeName;

    #region Header
    [Header("Оторбражение в редакторе графа")]
    #endregion Header
    public bool displayInNodeGraphEditor = true;
    #region Header
    [Header("Коридор")]
    #endregion Header
    public bool isCorridor;
    #region Header
    [Header("Коридор Север Юг")]
    #endregion Header
    public bool isCorridorNs;
    #region Header
    [Header("Коридор Запад Восток")]
    #endregion Header
    public bool isCorridorEW;
    #region Header
    [Header("Стартовая комната")]
    #endregion Header
    public bool isEntrance;
    #region Header
    [Header("Комната с боссом")]
    #endregion Header
    public bool isBossRoom;
    #region Header
    [Header("Без типа")]
    #endregion Header
    public bool isNone;
    #region Header
    [Header("Комната с выходом из уровня")]
    #endregion Header
    public bool isEndRoom;
    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(roomNodeTypeName), roomNodeTypeName);
    }
#endif
    #endregion
}