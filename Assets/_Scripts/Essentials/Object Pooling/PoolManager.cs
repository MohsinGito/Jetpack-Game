using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolManager : ObjectPooler
{
    
    #region Singleton

    public static PoolManager Instance;
    
    private void Awake() 
    {
        if(!Instance)
            Instance = this;
        else
            Destroy(Instance);

        DontDestroyOnLoad(Instance);
        Init();
    }

    #endregion

    #region Public Attributes

    public List<PoolInfo> poolInfos;
    public bool canDebug;

    #endregion

    #region Main Methods

    private void Init()
    {
        foreach(PoolInfo poolItem in poolInfos)
        {
            foreach (PoolObj item in poolItem.ObjectsForPooling)
            {
                if (Create(item.Tag, item.Prefab))
                {
                    Log("Pool Created With Tag ('" + item.Tag + "')");
                }
                else
                {
                    LogError("Failed To Create Pool With Tag ('" + item.Tag + "') || POOL ALREADY CREATED!");
                }
            }
        }
    }
    
    public GameObject GetFromPool(string tag)
    {
        GameObject newObj = Get(tag);
        if(newObj == null)
        {
            LogError("Failed To Get Object With Tag ('" + tag + "')");
        }
        else
        {
            Log("Oject Fetched With Tag ('" + tag + "') || POOL NOT FOUND!");
        }
        return newObj;
    }

    public void ReturnToPool(string tag, GameObject objToReturn)
    {
        if(Put(tag, objToReturn))
        {
            Log("Item Added To Pool With Tag ('" + tag + "')");
        }
        else
        {
            LogError("Failed To Put Object To Pool With Tag ('" + tag + "') || POOL NOT FOUND!");
        }
    }


    public void ReturnToPool(PoolObj objInfo)
    {
        if (string.IsNullOrEmpty(objInfo.Tag) || objInfo.Prefab == null)
            return;

        if (Put(objInfo.Tag, objInfo.Prefab))
        {
            Log("Item Added To Pool With Tag ('" + tag + "')");
        }
        else
        {
            LogError("Failed To Put Object To Pool With Tag ('" + tag + "') || POOL NOT FOUND!");
        }
    }

    public void DeletePool(string tag)
    {
        if(Delete(tag))
        {
            Log("Pool Deleted With Tag ('" + tag + "')");
        }
        else
        {
            LogError("Failed To Delete Pool With Tag ('" + tag + "') || POOL NOT FOUND!");
        }
    }

    public List<string> GetPoolTags(string _poolName)
    {
        PoolInfo poolInfo = poolInfos.Find(n => n.poolName.Equals(_poolName));
        return poolInfo.ObjectsForPooling.Select(n => n.Tag).ToList();
    }

    #endregion

    #region Debugging Methods

    private void Log(string msg)
    {
        if(!canDebug)
            return;

        Debug.Log("[Script : " + this.name + "] || " + msg);
    }

    private void LogError(string msg)
    {
        if(!canDebug)
            return;

        Debug.LogError("[Script : " + this.name + "] || " + msg);
    }

    #endregion

}

#region Utilities

[System.Serializable]
public struct PoolObj
{
    public string Tag;
    public GameObject Prefab;

    public PoolObj(string _tag, GameObject _obj)
    {
        Tag = _tag;
        Prefab = _obj;
    }
}

[System.Serializable]
public struct PoolInfo
{
    public string poolName;
    public List<PoolObj> ObjectsForPooling;
}

#endregion