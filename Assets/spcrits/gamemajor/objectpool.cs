using UnityEngine;
using System.Collections.Generic;

public static class LightObjectPool
{
    private static readonly Dictionary<string, Queue<GameObject>> _poolDict = new Dictionary<string, Queue<GameObject>>();
    private static GameObject _poolRoot;

    public static GameObject GetObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {

        string prefabName = prefab.name;
        if (_poolDict.ContainsKey(prefabName) && _poolDict[prefabName].Count > 0)
        {
            GameObject obj = _poolDict[prefabName].Dequeue();
            obj.SetActive(true);
            obj.transform.SetParent(null);
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            return obj;
        }
        GameObject newObj = Object.Instantiate(prefab, position, rotation);
        newObj.name = prefabName;
        return newObj;
    }

    public static void ReturnObject(GameObject obj)
    {
        if (obj == null) return;
        string prefabName = obj.name;
        if (!_poolDict.ContainsKey(prefabName))
        {
            _poolDict[prefabName] = new Queue<GameObject>();
        }

        obj.SetActive(false);
        obj.transform.SetParent(_poolRoot.transform);
        _poolDict[prefabName].Enqueue(obj);
    }
    public static void ClearPool(string prefabName)
    {
        if (_poolDict.ContainsKey(prefabName))
        {
            foreach (var obj in _poolDict[prefabName])
            {
                Object.Destroy(obj);
            }
            _poolDict[prefabName].Clear();
        }
    }
}