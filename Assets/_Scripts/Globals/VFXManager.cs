using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{

    #region Main Attributes

    [SerializeField] List<GameVFX> gameVfxes;
     Dictionary<string, List<GameObject>> gameVFXs;

    #endregion

    #region Singleton

    public static VFXManager Instance;
    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);
    }

    #endregion

    #region Main Methods

    private void Start()
    {
        gameVFXs = new Dictionary<string, List<GameObject>>();
        foreach (GameVFX gameVFX in gameVfxes)
        {
            gameVFXs[gameVFX.Name] = gameVFX.vfxs;
        }
    }

    public void DisplayVFX(string _vfxName, Vector3 _vfxPos, bool _usePoolManager = false)
    {
        GameObject vfx;

        if (_usePoolManager)
            vfx = GetVFX(_vfxName);
        else
            vfx = gameVFXs[_vfxName][0];

        vfx.SetActive(false);
        vfx.SetActive(true);
        vfx.transform.position = _vfxPos;
    }

    public void DisplayVFX(string _vfxName)
    {
        GameObject vfx = GetVFX(_vfxName);
        vfx.SetActive(false);
        vfx.SetActive(true);
    }
    
    private GameObject GetVFX(string _vfxName)
    {
        if (!gameVFXs.ContainsKey(_vfxName))
            AddNewVFX(_vfxName);

        foreach (GameObject vfx in  gameVFXs[_vfxName])
        {
            if(!vfx.activeSelf)
                return vfx;
        }

        gameVFXs[_vfxName].Add(PoolManager.Instance.GetFromPool(_vfxName));
        return gameVFXs[_vfxName][gameVFXs[_vfxName].Count - 1];
    }
    private void AddNewVFX(string _vfxName)
    {
        List<GameObject> vfxs = new List<GameObject>();
        vfxs.Add(PoolManager.Instance.GetFromPool(_vfxName));
        gameVFXs[_vfxName] = vfxs;

        vfxs[vfxs.Count - 1].SetActive(false);
        gameVfxes.Add(new GameVFX(_vfxName, vfxs));

        if (vfxs[vfxs.Count - 1] == null)
            Debug.Log("-- VFX Not Exist --");
    }

    #endregion

}

[System.Serializable]
public struct GameVFX
{
    public string Name;
    public List<GameObject> vfxs;

    public GameVFX(string _n, List<GameObject> _l)
    {
        Name = _n;
        vfxs = _l;
    }
}