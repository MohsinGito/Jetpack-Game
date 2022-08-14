using DG.Tweening;
using System.Collections;
using UnityEngine;
using Utilities.Audio;

public class MissileController : MonoBehaviour
{

    #region Public Attributes

    [Header("-- Missilse Controlling --")]
    public float moveDuration;
    public float indicationTimer;
    public float startPositionX;
    public float endPositionX;
    public Vector2 minMaxCooldown;

    #endregion

    #region Private Attributes

    private bool canSpawnDouble;
    private float cooldownTime;
    private PoolObj missileInfo;
    private UIManager uiManager;
    private EnvironmentManager environmentManager;
    private Transform playerTransform;

    #endregion

    #region Public Methods

    public void Init(EnvironmentManager _environmentManager, UIManager _uiManager, Transform _player)
    {
        uiManager = _uiManager;
        playerTransform = _player;
        environmentManager = _environmentManager;

        cooldownTime = minMaxCooldown.y;
        StartCoroutine(StartLaunchingMissile());
    }

    public void GameEnded()
    {
        ResetElements(true);
        StopAllCoroutines();
    }

    public void DestroyAllMissiles()
    {
        ResetElements(true);
        StopAllCoroutines();
        StartCoroutine(StartLaunchingMissile());
    }

    #endregion

    #region Private Methods

    private IEnumerator StartLaunchingMissile()
    {
        uiManager.HideIndication();
        cooldownTime = Mathf.Clamp(cooldownTime, minMaxCooldown.x, minMaxCooldown.y);

        while (true) 
        {
            yield return Helper.WaitFor(cooldownTime);
            cooldownTime = Mathf.Clamp(--cooldownTime, minMaxCooldown.x, minMaxCooldown.y);
            InitiateNewMissile();
        }
    }

    private void InitiateNewMissile()
    {
        AudioController.Instance.PlayAudio(AudioName.MISSILE_INDICATION);
        uiManager.DisplayMissileIndication(indicationTimer, playerTransform, (r) => LaunchMissle(r));
        if (cooldownTime == minMaxCooldown.x) { canSpawnDouble = Random.Range(0, 2) == 1; }
    }

    private void LaunchMissle(float _yPos)
    {
        AudioController.Instance.PlayAudio(AudioName.MISSILE_LAUNCH);
        missileInfo = new PoolObj("Missile", PoolManager.Instance.GetFromPool("Missile"));
        missileInfo.Prefab.transform.position = new Vector2(startPositionX, _yPos);
        missileInfo.Prefab.transform.DOMove(new Vector2(endPositionX, _yPos), moveDuration)
            .OnComplete(() => { PoolManager.Instance.ReturnToPool(missileInfo); });
        missileInfo.Prefab.transform.parent = transform;

        if (canSpawnDouble) { InitiateNewMissile(); }
    }

    private void ResetElements(bool displayVfx = false)
    {
        if (missileInfo.Prefab != null)
        {
            if(displayVfx)
                environmentManager.DestroyMapElement(missileInfo.Prefab);

            PoolManager.Instance.ReturnToPool(missileInfo);
            missileInfo.Prefab = null;
        }
    }

    #endregion

}