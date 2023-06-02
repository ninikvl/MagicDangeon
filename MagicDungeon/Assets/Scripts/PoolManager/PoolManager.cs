using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PoolManager : SingletonMonobehavior<PoolManager>
{
    #region Tolltip
    [Tooltip("Заполните массив префабами, которые будут в пуле, укажите количество и тип")]
    #endregion
    [SerializeField] private Pool[] poolArray = null;
    private Transform objectPoolTransforrm;
    private Dictionary<int, Queue<Component>> poolDictionary = new Dictionary<int, Queue<Component>>();

    [System.Serializable]
    public struct Pool
    {
        public int poolSize;
        public GameObject prefab;
        public string componentType;
    }

    private void Start()
    {
        objectPoolTransforrm = this.gameObject.transform;

        for (int i = 0; i < poolArray.Length; i++)
        {
            CreatePool(poolArray[i].prefab, poolArray[i].poolSize, poolArray[i].componentType);
        }
    }

    /// <summary>
    /// Создание пула на основе и количестве префаба
    /// </summary>
    private void CreatePool(GameObject prefab, int poolSize, string componentType)
    {
        int poolKey = prefab.GetInstanceID();
        string prefabName = prefab.name;
        GameObject parentGameObject = new GameObject(prefabName + "Anchor");

        parentGameObject.transform.SetParent(objectPoolTransforrm);

        if (!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary.Add(poolKey, new Queue<Component>());

            for (int i = 0; i < poolSize; i++)
            {
                GameObject newObject = Instantiate(prefab, parentGameObject.transform) as GameObject;
                newObject.SetActive(false);

                poolDictionary[poolKey].Enqueue(newObject.GetComponent(Type.GetType(componentType)));
            }
        }
    }

    /// <summary>
    /// Повторное использование компонентов в пуле. prefab - префаб игрового объекта содержащего компонент, 
    /// position - мировые координаты где оюъект должен быть активирован,
    /// rotation - должно быть установлено если объект требует поворота
    /// </summary>
    public Component ReuseComponent(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        int poolKey = prefab.GetInstanceID();
        if (poolDictionary.ContainsKey(poolKey))
        {
            Component componentToReuse = GetComponentFromPool(poolKey);

            ResetObject(position, rotation, componentToReuse, prefab);

            return componentToReuse;
        }
        else
        {
            Debug.Log("Нету объекта в пуле " + prefab);
            return null;
        }
    }

    /// <summary>
    /// получить компонент объекта из пула по ключу
    /// </summary>
    private Component GetComponentFromPool(int poolKey)
    {
        Component componentToReuse = poolDictionary[poolKey].Dequeue();
        poolDictionary[poolKey].Enqueue(componentToReuse);

        if (componentToReuse.gameObject.activeSelf == true)
        {
            componentToReuse.gameObject.SetActive(false);
        }

        return componentToReuse;
    }

    /// <summary>
    /// Сброс игрового объекта
    /// </summary>
    private void ResetObject(Vector3 position, Quaternion rotation, Component componentToReuse, GameObject prefab)
    {
        componentToReuse.transform.position = position;
        componentToReuse.transform.rotation = rotation;
        componentToReuse.gameObject.transform.localScale = prefab.transform.localScale;
    }

    #region Validation
#if UNITY_EDITOR

    private void OnValidate()
    {
       //HelperUtilities.ValidateCheckEnumerableValues(this, nameof(poolArray), poolArray);
    }

#endif
    #endregion Validation
}
