using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Health : MonoBehaviour
{
    private int startingHealth;
    private int currentHealth;
    
    /// <summary>
    /// ��������� ��������� �������� ��������
    /// </summary>
    public void SetStartingHealth(int startingHealth)
    {
        this.startingHealth = startingHealth;
        currentHealth = startingHealth;
    }

    /// <summary>
    /// �������� ��������� �������� ��������
    /// </summary>
    public int GetStartingHealth()
    {
        return startingHealth;
    }
}