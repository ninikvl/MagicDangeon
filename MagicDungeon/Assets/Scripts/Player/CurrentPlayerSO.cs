using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CurrentPlayer_", menuName = "Scriptable Objects/Игрок/Конфигурация выбранного персонажа игрока")]
public class CurrentPlayerSO : ScriptableObject 
{
    public PlayerDetailsSO playerDetails;
    public string playerName;
}
