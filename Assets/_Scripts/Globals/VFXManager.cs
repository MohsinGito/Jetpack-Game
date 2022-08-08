using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{

    #region Main Attributes

    [SerializeField] List<GameVFX> gameVfxes;
    static Dictionary<string, GameVFX> gameVFXs;

    #endregion

    #region Main Methods

    private void Start()
    {
        gameVFXs = new Dictionary<string, GameVFX>();
        foreach (GameVFX gameVFX in gameVfxes)
        {
            gameVFXs[gameVFX.Name] = gameVFX;
        }
    }

    public static void DisplayVFX(string _vfxName, Vector3 vfxPos)
    {
        gameVFXs[_vfxName].vfx.SetActive(false);
        gameVFXs[_vfxName].vfx.SetActive(true);
        gameVFXs[_vfxName].vfx.transform.position = vfxPos;
    }

    public static void DisplayVFX(string _vfxName)
    {
        gameVFXs[_vfxName].vfx.SetActive(false);
        gameVFXs[_vfxName].vfx.SetActive(true);
    }

    #endregion

}

[System.Serializable]
public struct GameVFX
{
    public string Name;
    public GameObject vfx;
}