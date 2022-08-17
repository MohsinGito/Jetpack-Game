using System.Collections.Generic;
using UnityEngine;

public class MapAsset : MonoBehaviour
{

    #region Public Attributes

    public List<SpriteRenderer> assetSprites;
    public List<BoxCollider2D> assetBoxColliders;
    public List<CircleCollider2D> assetCircleColliders;
    public List<GameObject> assetsObjects;
    public Transform vfxPos;

    public bool IsActive
    {
        get
        {
            return assetSprites[0].enabled;
        }
    }
    #endregion

    #region Main Methods

    public void SetActive(bool _value)
    {
        foreach(SpriteRenderer sr in assetSprites)
            sr.enabled = _value;

        foreach (BoxCollider2D bx in assetBoxColliders)
            bx.enabled = _value;

        foreach (CircleCollider2D cc in assetCircleColliders)
            cc.enabled = _value;

        foreach (GameObject go in assetsObjects)
            go.SetActive(_value);
    }

    #endregion

}