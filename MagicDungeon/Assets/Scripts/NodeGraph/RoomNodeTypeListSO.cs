using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomNodeTypeListSO", menuName = "Scriptable Objects/Редактор подземелья/Список типов комнат")]
public class RoomNodeTypeListSO : ScriptableObject
{
    #region Header
    [Space(10)]
    [Header("Список типов комнат")]
    #endregion
    #region Tooltip
    [Tooltip("Этот список должен быть заполнен всеми типами комнат для игры")]
    #endregion
    public List<RoomNodeTypeSO> list;

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEnumeravbleValues(this, nameof(list), list);
    }
#endif
    #endregion
}
