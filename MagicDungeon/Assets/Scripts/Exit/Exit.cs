using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour, IUseable
{
    public void UseItem()
    {
        GameManager.Instance.UseExit();
        gameObject.SetActive(false);
    }
}
