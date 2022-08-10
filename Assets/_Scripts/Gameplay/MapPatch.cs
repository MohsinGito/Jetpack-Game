using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPatch : MonoBehaviour
{

    #region Public Attributes

    [Header("-- Background Info --")]
    public SpriteRenderer layerOne;
    public SpriteRenderer layerTwo;
    public SpriteRenderer layerThree;

    [Header("-- Environment Assets Spawn Info --")]
    public Vector2 minMaxX;
    public Vector2 minMaxY;

    [Header("-- Electric Obstacle Spawn Info --")]
    public float floorPos;
    public float bottomPos;
    public List<Transform> electricObstaclePos;

    #endregion

    #region Private Attributes

    private bool initialized;
    private float deadEnd;
    private float moveSpeed;

    public Vector3 Position
    {
        set { transform.position = value; }
        get { return transform.position; }
    }

    #endregion

    #region Public Methods

    public void SetUpPatch(GameMap _mapData,Vector3 _initialPos, float _moveSpeed, float _deadEnd)
    {
        deadEnd = _deadEnd;
        moveSpeed = _moveSpeed;
        layerOne.sprite = _mapData.layerOne;
        layerTwo.sprite = _mapData.layerTwo;
        layerThree.sprite = _mapData.layerThree;
        transform.position = _initialPos;
        initialized= true;
    }

    public bool ReachedToDeadEnd()
    {
        if(transform.position.x <= deadEnd)
            return true;
        return false;
    }

    public void ResetPatch()
    {
        
    }

    #endregion

    #region Private Methods

    private void Update()
    {
        if (!initialized)
            return;

        transform.position -= new Vector3(moveSpeed * Time.deltaTime, 0, 0);
    }

    #endregion

}