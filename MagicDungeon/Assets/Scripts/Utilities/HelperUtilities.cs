using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperUtilities 
{
    public static Camera mainCamera;

    /// <summary>
    /// Возвращает глобальные координаты мыши
    /// </summary>
    public static Vector3 GetMouseWorldPosition()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        Vector3 mouseScreenPosition = Input.mousePosition;

        mouseScreenPosition.x = Mathf.Clamp(mouseScreenPosition.x, 0f, Screen.width);
        mouseScreenPosition.y = Mathf.Clamp(mouseScreenPosition.y, 0f, Screen.width);

        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        worldPosition.z = 0f;

        return worldPosition;
    }

    /// <summary>
    /// Получить угол в градусах относительно вектора
    /// </summary>
    public static float GetAngleFromVector(Vector3 vector)
    {
        float radians = Mathf.Atan2(vector.y, vector.x);

        float degrees = radians * Mathf.Rad2Deg;

        return degrees;
    }

    /// <summary>
    /// Получить вектор направления от угла в градусах
    /// </summary>
    public static Vector3 GetDirectionVectorFromAngle(float angle)
    {
        Vector3 directionVector = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0f);
        return directionVector;
    }

    /// <summary>
    /// Определить направление курсора мыши отностительно персонажа
    /// </summary>
    public static AimDirection GetAimDirection(float angleDegrees)
    {
        AimDirection aimDirection;
        
        if (angleDegrees >= 22f && angleDegrees <= 67f)
        {
            aimDirection = AimDirection.UpRight;
        }
        else if (angleDegrees > 67f && angleDegrees <= 122f)
        {
            aimDirection = AimDirection.Up;
        }
        else if(angleDegrees > 112f && angleDegrees <= 158f)
        {
            aimDirection = AimDirection.UpLeft;
        }
        else if ((angleDegrees <= 180f && angleDegrees > 158f) || (angleDegrees > -180 && angleDegrees <= -135f))
        {
            aimDirection = AimDirection.Left;
        }
        else if (angleDegrees > -135f && angleDegrees <= -45f)
        {
            aimDirection = AimDirection.Down;
        }
        else if ((angleDegrees > -45f && angleDegrees <= 0f) || (angleDegrees > 0 && angleDegrees < 22f))
        {
            aimDirection = AimDirection.Right;
        }
        else
        {
            aimDirection = AimDirection.Right;
        }

        return aimDirection;
    }

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
    /// Проверка сущесвутет ли объект
    /// </summary>
    public static bool ValidateCheckNullValue(Object thisObject, string fieldName, Object objectToCheck)
    {
        if (objectToCheck == null)
        {
            Debug.Log(fieldName + " Объект имеет нулевое значение " + thisObject.name.ToString());
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
            Debug.Log(fieldName + " Объект имеет нулевое значение " + thisObject.name.ToString());
            error = true;
            return error;
        }

        foreach (var item in enumerableObjectToCheck)
        {
            if (item == null)
            {
                Debug.Log(fieldName + " Имеент нулевые значения в объекте " + thisObject.name.ToString());
                error = true;
            }
            else
            {
                count++;
            }           
        }

        if (count == 0)
        {
            Debug.Log(fieldName + " Нету  значений в объекте " + thisObject.name.ToString());
            error = true;
        }
        return error;
    }

    /// <summary>
    /// Проверка на положительные значения, включая или не включая ноль
    /// </summary>
    public static bool ValidateCheckPositiveValue(Object thisObject, string fieldName, int valueToCheck, bool isZeroAllowed)
    {
        bool error = false;

        if (isZeroAllowed)
        {
            if (valueToCheck < 0)
            {
                Debug.Log(fieldName + " Объект содержит отрицательные значения " + thisObject.name.ToString());
                error = true;
            }
        }
        else
        {
            if (valueToCheck <= 0)
            {
                Debug.Log(fieldName + " Объект содержит отрицательные или нулевые значения " + thisObject.name.ToString());
                error = true;
            }
        }
        return error;
    }

    /// <summary>
    /// Проверка на положительные значения, включая или не включая ноль
    /// </summary>
    public static bool ValidateCheckPositiveValue(Object thisObject, string fieldName, float valueToCheck, bool isZeroAllowed)
    {
        bool error = false;

        if (isZeroAllowed)
        {
            if (valueToCheck < 0)
            {
                Debug.Log(fieldName + " Объект содержит отрицательные значения " + thisObject.name.ToString());
                error = true;
            }
        }
        else
        {
            if (valueToCheck <= 0)
            {
                Debug.Log(fieldName + " Объект содержит отрицательные или нулевые значения " + thisObject.name.ToString());
                error = true;
            }
        }
        return error;
    }

    /// <summary>
    /// Проверка на положительные числа в диапазоне, включая или не включая ноль
    /// </summary>
    public static bool vakidateCheckPositiveRange(Object thisObject, string fieldNameMinimum, float valueToCheckMinimum, string fieldNameMaximum,
        float valueToCheckMaximum, bool isZeroAllowed)
    {
        bool error = false;
        if (valueToCheckMinimum > valueToCheckMaximum)
        {
            Debug.Log(fieldNameMinimum + " должен быть меньше чем " + fieldNameMaximum + " в объекте " + thisObject.name.ToString());
            error = true;
        }

        if (ValidateCheckPositiveValue(thisObject, fieldNameMinimum, valueToCheckMinimum, isZeroAllowed))
            error = true;
        if (ValidateCheckPositiveValue(thisObject, fieldNameMaximum, valueToCheckMaximum, isZeroAllowed))
            error = true;

        return error;
    }

    /// <summary>
    /// Получить ближайшую позицую спавна
    /// </summary>
    public static Vector3 GetSpawnPositionToPlayer(Vector3 playerPosition)
    {
        Room currentRoom = GameManager.Instance.GetCurrentRoom();

        Grid grid = currentRoom.instantiatedRoom.grid;

        Vector3 nearestSpawnPosition = new Vector3(10000f, 10000f, 0);
        /* NEW CODE TEST
        Vector3 nearestSpawnPosition = grid.CellToWorld((Vector3Int)currentRoom.spawnPositionArray[0]);
        if (nearestSpawnPosition == null)
            return playerPosition;
        */

        foreach (Vector2Int spawnPositionGrid in currentRoom.spawnPositionArray)
        {
            Vector3 spawnPositionWorld = grid.CellToWorld((Vector3Int)spawnPositionGrid);

            if (Vector3.Distance(spawnPositionWorld, playerPosition) < Vector3.Distance(nearestSpawnPosition, playerPosition))
            {
                nearestSpawnPosition = spawnPositionWorld;
            }
        }

        return nearestSpawnPosition;
    }
}
