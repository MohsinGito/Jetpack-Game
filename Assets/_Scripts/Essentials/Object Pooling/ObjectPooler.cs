using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{

    #region Pool Lists

    private Dictionary<string , Queue<GameObject>> PoolList = null;
    private Dictionary<string, PoolRecord> poolRecords = null;
    private Queue<GameObject> objectQueue = null;

    #endregion
    
    #region Attributes Used In Methods

    private GameObject newInstantiatedObject = null;
    private GameObject getOject = null;
    private Transform newPoolParent = null;

    #endregion

    #region Main Methods

    protected bool Create(string tag, GameObject prefab)
    {
        {//  INITIALIZING LISITS
            if(PoolList == null)
            PoolList = new Dictionary<string , Queue<GameObject>>();

            if (poolRecords == null)
                poolRecords = new Dictionary<string, PoolRecord>();
        }
        
        {// CHECKING IF THE POOL ALREADY CREATED
            if(IsPoolCreated(tag))
                return false;
        }

        {// CREATING A RECORD FOR POOL
            newInstantiatedObject = CreateNewObject(tag, prefab);
            poolRecords[tag] = new PoolRecord(tag, newPoolParent, prefab);
        }

        {// CREATING A NEW POOL ITEM
            objectQueue = new Queue<GameObject>();
            objectQueue.Enqueue(newInstantiatedObject);
            PoolList[tag] = objectQueue;
        }
        
        return true; 
    }

    protected GameObject Get(string poolTag)
    {
        if(!IsPoolCreated(poolTag))
            return null;
        
        foreach(KeyValuePair<string, Queue<GameObject>> item in PoolList)
        {
            if(item.Key == poolTag)
            {
                if(item.Value.Count == 0)
                {
                    getOject = Instantiate(poolRecords[poolTag].Prefab, poolRecords[poolTag].PoolParent);
                }
                else
                {
                    getOject = item.Value.Dequeue();
                }
                break;
            }
        }

        getOject.SetActive(true);
        return getOject;
    }

    protected bool Put(string poolTag, GameObject objectToReturn)
    {
        if(!IsPoolCreated(poolTag))
            return false;

        objectToReturn.SetActive(false);
        objectToReturn.transform.parent = poolRecords[poolTag].PoolParent;
        PoolList[poolTag].Enqueue(objectToReturn);
        return true;
    }

    protected bool Delete(string poolTag)
    {
        if(!IsPoolCreated(poolTag))
            return false;
        
        foreach(GameObject obj in PoolList[poolTag])
        {
            Destroy(obj);
        }

        Destroy(poolRecords[poolTag].PoolParent.gameObject);
        PoolList.Remove(poolTag);
        return true;
    }

    #endregion

    #region  Utilities

    private bool IsPoolCreated(string tag)
    {
        foreach(KeyValuePair<string, Queue<GameObject>> item in PoolList)
        {
            if(item.Key == tag)
                return true;
        }
        return false;
    }

    private GameObject CreateNewObject(string poolTag, GameObject prefab)
    {
        newPoolParent = new GameObject().transform;
        newPoolParent.SetParent(this.gameObject.transform);
        newPoolParent.gameObject.name = poolTag;

        GameObject newCreatedObject = Instantiate(prefab, newPoolParent);
        newCreatedObject.SetActive(false);
        return newCreatedObject;
    }

    private Transform GetParentForPool(string tag)
    {
        foreach (KeyValuePair<string, PoolRecord> record in poolRecords)
        {
            if (record.Key.Equals(tag))
                return record.Value.PoolParent;
        }
        return null;
    }

    public class PoolRecord
    {
        public string PoolTag;
        public Transform PoolParent;
        public GameObject Prefab;

        public PoolRecord(string tag, Transform parent, GameObject prefab)
        {
            this.PoolTag = tag;
            this.PoolParent = parent;
            this.Prefab = prefab;
        }
    }

    #endregion

}