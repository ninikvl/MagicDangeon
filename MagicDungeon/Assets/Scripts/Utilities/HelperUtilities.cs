using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperUtilities 
{

    /// Empty string debug check
    public static bool ValidateCheckEmptyString(Object thisObject, string fieldName, string stringToCheck)
    {
        if (stringToCheck == "")
        {
            Debug.Log(fieldName + "is empty and must contain a value in object" + thisObject.name.ToString());
            return true;
        }
        return false;
    }

    /// <summary>
    /// �������� ���������� �� ������
    /// </summary>
    public static bool ValidateCheckNullValue(Object thisObject, string fieldName, Object objectToCheck)
    {
        if (objectToCheck == null)
        {
            Debug.Log(fieldName + " ������ ����� ������� �������� " + thisObject.name.ToString());
            return true;
        }
        return false;
    }

    //list empty or contains null valaue check - returns true if there is an error
    public static bool ValidateCheckEnumerableValues (Object thisObject, string fieldName, IEnumerable enumerableObjectToCheck)
    {
        bool error = false;
        int count = 0;

        if (enumerableObjectToCheck == null)
        {
            Debug.Log(fieldName + " ������ ����� ������� �������� " + thisObject.name.ToString());
            error = true;
        }

        foreach (var item in enumerableObjectToCheck)
        {
            if (item == null)
            {
                Debug.Log(fieldName + " ������ ������� �������� � ������� " + thisObject.name.ToString());
                error = true;
            }
            else
            {
                count++;
            }           
        }

        if (count == 0)
        {
            Debug.Log(fieldName + " ����  �������� � ������� " + thisObject.name.ToString());
            error = true;
        }
        return error;
    }

    /// <summary>
    /// �������� �� ������������� ��������, ������� ��� �� ������� ����
    /// </summary>
    public static bool ValidateCheckPositiveValue(Object thisObject, string fieldName, int valueToCheck, bool isZeroAllowed)
    {
        bool error = false;

        if (isZeroAllowed)
        {
            if (valueToCheck < 0)
            {
                Debug.Log(fieldName + " ������ �������� ������������� �������� " + thisObject.name.ToString());
                error = true;
            }
        }
        else
        {
            if (valueToCheck <= 0)
            {
                Debug.Log(fieldName + " ������ �������� ������������� ��� ������� �������� " + thisObject.name.ToString());
                error = true;
            }
        }
        return error;
    }
}
