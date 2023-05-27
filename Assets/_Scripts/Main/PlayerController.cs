using DG.Tweening;
using GameControllers;
using UnityEngine;
using Utilities.Audio;

public class PlayerController : GameState
{

    #region Public Attributes

    public float moveForce;
    public float gravityUp;
    public float gravityDown;
    public Vector2 minMaxY;
    public ParticleController jetpackSmoke;

    [Header("-- Player Colliders --")]
    public PlayerCollider bodyCollider;
    public PlayerCollider magnetCollider;
    public PlayerCollider shieldCollider;

    #endregion

    #region Private Attributes

    private bool canDie;
    private bool beginFlyUp;
    private bool controllsEnabled;
    private Animator m_animator;
    private Rigidbody2D m_rigidbody;
    private UIManager uiManager;
    private EnvironmentManager envManager;

    #endregion

    #region Public Methods

    public void Init(EnvironmentManager _environmentManager, RuntimeAnimatorController _playerAnimator, UIManager _uiManager)
    {
        // -- CACHING MAIN COMPONENTS
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator = GetComponentInChildren<Animator>();

        canDie = true;
        uiManager = _uiManager;
        envManager = _environmentManager;
        m_animator.runtimeAnimatorController = _playerAnimator;
    }

    public override void OnGameStart()
    {
        AudioController.Instance.PlayAudio(AudioName.JETPACK_SFX);

        // -- THE PLAYER IS MOVING TOWARDS THE INITIAL POSITION (IN THE CENTER OF SCREEN)
        transform.position = new Vector3(-10f, 0f, 0f);
        transform.DOMove(new Vector3(-1f, 0f, 0f), 1.5f).SetEase(Ease.Unset).OnComplete(() =>
        {
            // -- AFTER REACHING TO INITIAL POSITION, PLAYER CONTROLLS WILL BE ENABLED
            PlayerSmoke(false);
            controllsEnabled = true;
            m_rigidbody.velocity = Vector2.zero;
            m_rigidbody.bodyType = RigidbodyType2D.Dynamic;
            uiManager.DisplayGameplayUI();
            AudioController.Instance.StopAudio(AudioName.JETPACK_SFX);
        });

        bodyCollider.Init(this);
        shieldCollider.Init(this);
        magnetCollider.Init(this);
        ActivateBodyDetection();
    }

    #endregion

    #region Private Methods

    private void Update()
    {
        // -- IF CONTROLLS ARE NOT ENABLED WE DON'T NEED TO PROCEED
        if (!controllsEnabled)
            return;

        // -- IF PLAYER START PRESSING THE MOUSE CLICK, HE'LL START MOVING UP
        if (Input.GetMouseButtonDown(0) && !Helper.IsCursorOverUI())
        {
            beginFlyUp = true;
            PlayerSmoke(true);

            // -- RESETING PLAYER VELOCITY SO THAT HE CAN MOVE UP GRADUALLY
            m_rigidbody.velocity = Vector2.zero;
        }

        if (Input.GetMouseButtonUp(0))
        {
            // -- IF PLAYER IS NOT PRESSING MOUSE BUTTON HE CAN'T FLY SO HE'LL START FALLING
            beginFlyUp = false;
            PlayerSmoke(false);
        }

        // -- IF PLAYER TOCHES THE CEILING WE DON'T NEED TO ACCELERATE IT'S VELOCITY
        if (transform.position.y == minMaxY.y)
            m_rigidbody.velocity = Vector2.zero;

        // -- CLAMPING PLAYER POSITION WITH IN THE MIN AND MAX RANGE OF PLAYER 'Y' POSITION
        transform.position = new Vector2(transform.position.x, Mathf.Clamp(transform.position.y, minMaxY.x, minMaxY.y));
    }

    private void FixedUpdate()
    {
        // -- IF CONTROLLS ARE NOT ENABLED WE DON'T NEED TO PROCEED
        if(!controllsEnabled)
            return;

        if(beginFlyUp)
        {
            // -- IF PLAYER KEEP PRESSING THE MOUSE CLICK HE'LL MOVE IN UPWARD DIRECTION
            m_rigidbody.AddForce(Vector2.up * moveForce, ForceMode2D.Force);
        }

        m_rigidbody.velocity = new Vector2(0, Mathf.Clamp(m_rigidbody.velocity.y, gravityDown, gravityUp));
        // -- AND IF HE DON'T CLICK MOUSE BUTTON, HE'LL START FALLING IN DOWNWARD DIRECTION THROUGH RIGIDBODY
    }

    private void PlayerDead()
    {
        if (!canDie)
            return;

        controllsEnabled = false;
        jetpackSmoke.gameObject.SetActive(false);
        bodyCollider.CollisionDectection(false);
        shieldCollider.CollisionDectection(false);
        magnetCollider.CollisionDectection(false);
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.black;
        m_rigidbody.AddForce(transform.forward * 100f, ForceMode2D.Force);
        GameSession.PlayerDied();

        var dieEffect = PoolManager.Instance.GetFromPool("Player Die Effect");
        dieEffect.transform.position = transform.position;
        AudioController.Instance.StopAudio(AudioName.JETPACK_SFX);
    }

    private void PlayerSmoke(bool val)
    {
        jetpackSmoke.Fade(val);

        if (val)
            AudioController.Instance.PlayAudio(AudioName.JETPACK_SFX);
        else
            AudioController.Instance.StopAudio(AudioName.JETPACK_SFX);
    }

    #endregion

    #region Player Collision Controlls

    public void ActivateBodyDetection()
    {
        bodyCollider.CollisionDectection(true);
        shieldCollider.CollisionDectection(false);
        magnetCollider.CollisionDectection(false);
    }

    public void ActivateMagnet()
    {
        bodyCollider.CollisionDectection(true);
        shieldCollider.CollisionDectection(false);
        magnetCollider.CollisionDectection(true);
    }

    public void ActivateShield()
    {
        bodyCollider.CollisionDectection(false);
        shieldCollider.CollisionDectection(true);
        magnetCollider.CollisionDectection(false);
    }

    public void ActivateBomb()
    {
        envManager.DestroyAllMapElement();
    }

    #endregion

    #region Collision Handling

    public void SimpleCoinCollision(GameObject collisionObj)
    {
        uiManager.IncrementSessionCoins();
        collisionObj.GetComponent<CircleCollider2D>().enabled = false;
        collisionObj.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        VFXManager.Instance.DisplayVFX("Coin Pickup Effect", collisionObj.transform.GetChild(1), true);
        AudioController.Instance.PlayAudio(AudioName.COIN_COLLECT);
    }

    public void MagnetCoinCollision(GameObject collisionObj)
    {
        uiManager.IncrementSessionCoins();
        collisionObj.transform.GetChild(0).GetComponent<SpriteRenderer>().DOFade(0, 0.35f);
        collisionObj.transform.DOMove(transform.position, 0.35f).OnComplete(() =>
        {
            collisionObj.SetActive(false);
            collisionObj.transform.GetChild(0).GetComponent<SpriteRenderer>().DOFade(1, 0.01f);
            AudioController.Instance.PlayAudio(AudioName.COIN_COLLECT);
        });
    }

    public void EnemyCollision(GameObject collisionObj)
    {
        envManager.DestroyMapElement(collisionObj, false);
        collisionObj.transform.GetChild(0).GetComponent<Animator>().CrossFade("Enemy Dead", 0);
    }

    public void SimpleObstacleCollision(GameObject collisionObj)
    {
        PlayerDead();
        envManager.DestroyMapElement(collisionObj);
    }

    public void ShieldObstacleCollision(GameObject collisionObj)
    {
        envManager.DestroyMapElement(collisionObj);
    }

    #endregion

}

public abstract class PlayerCollider : MonoBehaviour
{
    public abstract void Init(PlayerController player);
    public abstract void CollisionDectection(bool _val);
}