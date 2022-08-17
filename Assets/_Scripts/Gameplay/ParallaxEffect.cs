using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{

    #region Public Attributes

    public float endPos;
    public float speedIncrement;
    public List<Transform> parallexTransforms;

    #endregion

    #region Private Attributes

    private bool isInitialized;
    private Transform cachedPatch;
    private EnvironmentManager envManager;
    private List<Vector3> cachedPositions;

    #endregion

    #region Main Methods

    public void Init(Sprite _displaySprite, EnvironmentManager _envManager)
    {
        isInitialized = true;
        envManager = _envManager;
        cachedPositions = parallexTransforms.Select(n => n.position).ToList();

        foreach (Transform parallexTransform in parallexTransforms)
            parallexTransform.GetComponent<SpriteRenderer>().sprite = _displaySprite;
    }

    private void Update()
    {
        if (!isInitialized)
            return;

        if (parallexTransforms[0].position.x <= endPos)
        {
            // -- CIRCULATING THE LIST SO THE NEXT PATCH WILL COME TO ZERO INDEX
            cachedPatch = parallexTransforms[0];
            for (int i = 0; i < parallexTransforms.Count; i++)
            {
                if (i == parallexTransforms.Count - 1)
                    parallexTransforms[i] = cachedPatch;
                else
                    parallexTransforms[i] = parallexTransforms[i + 1];
            }

            // -- AFTER CIRCULATING, ASSIGNING ORIGINAL POSITIONS SO THAT ENVIRONMENT PATCHES KEEP LOOPING
            for (int i = 0; i < parallexTransforms.Count; i++)
            {
                parallexTransforms[i].position = cachedPositions[i];
            }
        }
        else
        {
            foreach (Transform pt in parallexTransforms)
            {
                pt.position -= new Vector3((envManager.patchMoveSpeed + (envManager.patchMoveSpeed * speedIncrement)) * Time.deltaTime, 0, 0);
            }
        }
    }


    #endregion


}
