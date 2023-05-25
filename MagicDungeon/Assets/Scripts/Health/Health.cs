using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
    private int startingHealth;
    private int currentHealth;
    
    /// <summary>
    /// ”становка начальных значений здоровь€
    /// </summary>
    public void SetStartingHealth(int startingHealth)
    {
        this.startingHealth = startingHealth;
        currentHealth = startingHealth;
    }

    /// <summary>
    /// ѕолучить стартовое значение здоровь€
    /// </summary>
    public int GetStartingHealth()
    {
        return startingHealth;
    }
}
