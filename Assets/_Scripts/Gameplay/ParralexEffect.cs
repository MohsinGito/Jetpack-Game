using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParralexEffect : MonoBehaviour
{

    #region Main Attributes

    public float endPos;
    public float parallexSpeed;
    public List<Transform> parallexTransforms;

    private Transform cachedPatch;
    private List<Vector3> cachedPositions;

    #endregion

    #region Main Methods

    private void Awake()
    {
        cachedPositions = parallexTransforms.Select(n => n.position).ToList();
    }

    private void Update()
    {
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
            foreach(Transform pt in parallexTransforms)
            {
                pt.position -= new Vector3(parallexSpeed * Time.deltaTime, 0, 0);
            }
        }
    }

    #endregion

}