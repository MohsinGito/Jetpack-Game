using System.Collections;
using System.Collections.Generic;
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

    public List<PoolObj> ObjectsForPooling;
    public bool canDebug;

    #endregion

    #region Main Methods

    private void Init()
    {
        foreach(PoolObj item in ObjectsForPooling)
        {
            if(Create(item.Tag, item.Prefab))
            {
                Log("Pool Created With Tag ('" + item.Tag + "')");
            }
            else
            {
                LogError("Failed To Create Pool With Tag ('" + item.Tag + "') || POOL ALREADY CREATED!");
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

#endregion