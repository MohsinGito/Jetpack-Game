using UnityEngine;

public class MapBackground : MonoBehaviour
{

    #region Main Attributes

    [Header("-- Background Layers --")]
    public ParallaxEffect backLayer;
    public ParallaxEffect middleLayer;
    public ParallaxEffect frontLayer;

    #endregion

    #region Main Methods

    public void Init(EnvironmentManager _envManager, GameMap mapData)
    {
        backLayer.Init(mapData.layerOne, _envManager);
        middleLayer.Init(mapData.layerTwo, _envManager);
        frontLayer.Init(mapData.layerThree, _envManager);
    }

    #endregion

}