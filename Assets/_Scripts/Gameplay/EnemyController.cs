using System.Collections;
using UnityEngine;
using Utilities.Audio;

public class EnemyController : MonoBehaviour
{

    #region Public Attributes

    [Header("-- Enemy Controlling --")]
    public float enemyMoveSpeed;
    public float bulletMoveSpeed;
    public float bulletMaxDistance;
    public float waitTimeInitial;
    public Vector2 minMaxWaitTime;

    [Header("-- Enemy Info --")]
    public Transform startPos;
    public Transform endPos;
    public Transform attackPos;
    public Transform bulletInitialPos;

    #endregion

    #region Private Attributes

    private float tempCooldown;
    private Transform enemy;
    private Transform bullet;
    private Transform player;
    private EnvironmentManager envManager;
    private Animator enemyAnimator;

    #endregion

    #region Public Methods

    public void Init(Transform _player, EnvironmentManager _envManager)
    {
        player = _player;
        envManager = _envManager;
        tempCooldown = minMaxWaitTime.y;

        enemy = PoolManager.Instance.GetFromPool("Enemy").transform;
        bullet = PoolManager.Instance.GetFromPool("Bullet").transform;
        enemyAnimator = enemy.GetChild(0).GetComponent<Animator>();

        enemy.gameObject.SetActive(false);
        bullet.gameObject.SetActive(false);

        StartCoroutine(StartSpawningEnemies());
    }

    public void DestroyEnemies()
    {

    }

    public void GameEnded()
    {
        StopAllCoroutines();
        PoolManager.Instance.ReturnToPool("Enemy", enemy.gameObject);
        PoolManager.Instance.ReturnToPool("Bullet", bullet.gameObject);
    }

    private void OnDisable()
    {
        StopAllCoroutines();

        if (enemy != null)
        {
            PoolManager.Instance.ReturnToPool("Enemy", enemy.gameObject);
            PoolManager.Instance.ReturnToPool("Bullet", bullet.gameObject);
        }
    }

    #endregion

    #region Private Methods

    private IEnumerator StartSpawningEnemies()
    {
        yield return new WaitForSeconds(waitTimeInitial);

        while (true)
        {
            yield return Helper.WaitFor(tempCooldown);
            tempCooldown = Mathf.Clamp(--tempCooldown, minMaxWaitTime.x, minMaxWaitTime.y);
            StartCoroutine(SetEnemyForMoving());
        }
    }

    private IEnumerator SetEnemyForMoving()
    {
        enemyAnimator.CrossFade("Idle", 0);
        enemy.gameObject.SetActive(true);
        enemy.position = startPos.position;

        float timer = 0;
        float duration = enemyMoveSpeed / 3;
        while (timer < duration)
        {
            enemy.position = Vector3.Lerp(startPos.position, attackPos.position, timer / duration);

            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        enemyAnimator.CrossFade("Enemy Attack", 0);
        yield return new WaitForSeconds(0.25f);
        AudioController.Instance.PlayAudio(AudioName.BULLET_FIRE);
        StartCoroutine(MoveBullet());

        timer = 0;
        duration = enemyMoveSpeed - (enemyMoveSpeed / 3);
        while (timer < duration)
        {
            enemy.position = Vector3.Lerp(attackPos.position, endPos.position, timer / duration);

            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator MoveBullet()
    {
        bullet.gameObject.SetActive(true);
        bullet.position = bulletInitialPos.position;
        Vector2 direction = (player.position - bullet.position).normalized;

        float timer = 0;
        float duration = bulletMoveSpeed;
        while (timer < duration)
        {
            bullet.position = Vector3.Lerp(bulletInitialPos.position, direction * bulletMaxDistance, timer / duration);

            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }


    #endregion

}