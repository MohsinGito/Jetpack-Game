using UnityEngine;
using System.Collections.Generic;

public class ParallaxEffect : MonoBehaviour
{
    public float differenceFactor = 1;
    public List<GameObject> parrallaxTransforms;

    private bool isInit;
    private EnvironmentManager envManager;
    private List<Vector3> initialPositions;

    public void Init(Sprite _displaySp, EnvironmentManager _envManager)
    {
        envManager = _envManager;
        initialPositions = new List<Vector3>();

        for (int i = 0; i < parrallaxTransforms.Count; i++)
        {
            Vector3 initialPosition;
            if (i == 0)
            {
                initialPosition = Vector3.zero;
            }
            else
            {
                initialPosition = parrallaxTransforms[i - 1].transform.position + new Vector3(envManager.patchDistance, 0, 0);
            }
            parrallaxTransforms[i].transform.position = initialPosition;
            initialPositions.Add(initialPosition);
            parrallaxTransforms[i].GetComponent<SpriteRenderer>().sprite = _displaySp;
        }

        isInit = true;
    }

    void Update()
    {
        if (!isInit)
            return;

        for (int i = 0; i < parrallaxTransforms.Count; i++)
        {
            float moveAmount = envManager.patchMoveSpeed * Time.deltaTime * differenceFactor;
            parrallaxTransforms[i].transform.position -= new Vector3(moveAmount, 0, 0);

            if (parrallaxTransforms[i].transform.position.x <= envManager.deadEndPosition)
            {
                float rightmostX = GetRightmostX() + envManager.patchDistance;
                parrallaxTransforms[i].transform.position = new Vector3(rightmostX, parrallaxTransforms[i].transform.position.y, parrallaxTransforms[i].transform.position.z);
            }
        }
    }

    float GetRightmostX()
    {
        float rightmostX = parrallaxTransforms[0].transform.position.x;
        for (int i = 1; i < parrallaxTransforms.Count; i++)
        {
            if (parrallaxTransforms[i].transform.position.x > rightmostX)
            {
                rightmostX = parrallaxTransforms[i].transform.position.x;
            }
        }

        return rightmostX;
    }
}
