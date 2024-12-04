using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IPoolableObject
{
    UnityEvent<IPoolableObject> OnInactive { get; }
}

public class SimplePool<PoolObjectType> : MonoBehaviour where PoolObjectType : MonoBehaviour, IPoolableObject
{
    private Transform _poolObjectsParent;
    private PoolObjectType _poolObjectPrefab;
    private Queue<PoolObjectType> _poolObjects;
    private int _poolSize;
    private bool _isResizable;

    private UnityEvent OnPoolDisable;

    public void InitializePool(PoolObjectType poolObjectPrefab, Transform poolObjectsParent, int poolSize = 100, bool isResizable = false)
    {
        _poolObjectsParent = poolObjectsParent;
        _poolObjectPrefab = poolObjectPrefab;
        _poolSize = poolSize;
        _isResizable = isResizable; 

        _poolObjects = new Queue<PoolObjectType>();
        SpawnObjects(_poolSize);
    }

    private void SpawnObjects(int size)
    {
        for (int i = 0; i < size; ++i)
        {
            var poolableObj = Instantiate<PoolObjectType>(_poolObjectPrefab);
            poolableObj.transform.parent = _poolObjectsParent;
            _poolObjects.Enqueue(poolableObj);

            var iPoolableObj = poolableObj.GetComponent<IPoolableObject>();
            iPoolableObj.OnInactive.AddListener(Deactivate);
        }
    }
    
    public PoolObjectType GetObject()
    {
        if (_poolObjects.Count == 0)
        {
            if (_isResizable)
            {
                SpawnObjects(_poolSize);
                _poolSize *= 2;
            }
            else
            {
                return null;
            }
        }
        
        var poolObj = _poolObjects.Dequeue();
        poolObj.gameObject.SetActive(true);
        return poolObj;
    }

    private void Deactivate(IPoolableObject iPoolableObject)
    {
        var poolObj = iPoolableObject as PoolObjectType;
        _poolObjects.Enqueue(poolObj);
        poolObj.gameObject.SetActive(false);
    }
    
}
