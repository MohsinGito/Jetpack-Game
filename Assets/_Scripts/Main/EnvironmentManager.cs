using GameControllers;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : GameState
{

    #region Public Attributes

    [Header("-- Patch Prefab --")]
    public MapPatch mapPatch;

    [Header("-- Map Speed Factor --")]
    public float patchMoveSpeed;

    [Header("-- Map Spawning Info's --")]
    public int startingMapPatches;
    public float patchDistance;
    public float startingPosition;
    public float deadEndPosition;

    #endregion

    #region Private Attributes

    private bool isGameStarted;
    private GameData gameData;
    private MapPatch cachedPatch;
    private List<MapPatch> environmentPatches;
    private List<Vector3> patchesInitialPositions;

    #endregion

    #region Public Methods

    public void Init(GameData _gameData)
    {
        gameData = _gameData;

        // -- INITIALIZING LISTS FOR REUSING REFERENCES
        environmentPatches = new List<MapPatch>();
        patchesInitialPositions = new List<Vector3>();

        // -- CREATING INITITAL GAME MAP PATCHES AND STORING THEM IN LIST
        for (int i = 0; i < startingMapPatches; i++)
        {
            environmentPatches.Add(Instantiate(mapPatch, transform));

            if (i == 0) 
            {
                // -- CHECKING IF ITS THE FIRST PATCH THEN ASSIGN IT INITIAL POSITION
                environmentPatches[i].Init(this, new Vector3(startingPosition, 0, 0), patchMoveSpeed, deadEndPosition);
            }
            else 
            {
                // -- IF ITS NOT FIRST PATCH THEN PREVIOUS PATCH MINUE PATCH DISTANCE WILL BE NEXT POSITION (DOING MINUS BECAUSE WE ARE MOVING IN REVERSE DIRECTION)
                environmentPatches[i].Init(this, environmentPatches[i - 1].Position - new Vector3(patchDistance, 0, 0), patchMoveSpeed, deadEndPosition);
            }
        }

        // -- REVERSING THE PATCH LIST AND CACHING THEIR INITIAL POSITIONS IN A VECTOR3 LIST (THIS WILL HELP US TO REPOSITION PATCHES TO IT'S ORIGINAL POSITIONS)
        List<MapPatch> tempList = new List<MapPatch>(environmentPatches);
        for (int i = tempList.Count - 1, reversePatchIndex = 0; i >= 0; i--)
        {
            environmentPatches[reversePatchIndex++] = tempList[i];
            patchesInitialPositions.Add(tempList[i].Position);
        }
    }

    public override void OnGameStart()
    {
        // -- SETTING MAP SPRITES (PLAYER SELECTED MAP)
        foreach (MapPatch patch in environmentPatches)
            patch.SetUpPatch(gameData.selectedMap);

        isGameStarted = true;
    }

    #endregion

    #region Private Methods

    private void Update()
    {
        // -- IF GAME IS NOT STARTED, THERE IS NO NEED TO PROCEED FURTHER
        if (!isGameStarted)
            return;

        // -- CHECKING IF THE FIRST ELEMENT REACHES TO FINAL POSITION, IF IT DOES THEN WE'LL RE-ARRANGE THE PATCHES TO INITIAL POSITIONS
        if (environmentPatches[0].ReachedToDeadEnd())
            RearrangePatches();
    }

    private void RearrangePatches()
    {
        // -- CIRCULATING THE LIST SO THE NEXT PATCH WILL COME TO ZERO INDEX
        cachedPatch = environmentPatches[0];
        for (int i = 0; i < environmentPatches.Count; i++)
        {
            if (i == environmentPatches.Count - 1)
                environmentPatches[i] = cachedPatch;
            else
                environmentPatches[i] = environmentPatches[i + 1];
        }

        // -- AFTER CIRCULATING, ASSIGNING ORIGINAL POSITIONS SO THAT ENVIRONMENT PATCHES KEEP LOOPING
        for (int i = 0; i < environmentPatches.Count; i++)
        {
            environmentPatches[i].Position = patchesInitialPositions[i];
        }

        // -- RESETTING THE PATCH THAT JUST COMPLETED IT'S LOOP
        cachedPatch.ResetPatch();

    }

    #endregion

}