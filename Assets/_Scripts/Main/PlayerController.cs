using DG.Tweening;
using GameControllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : GameState
{

    #region Public Attributes

    public float moveForce;
    public Vector2 minMaxY;

    #endregion

    #region Private Attributes

    private bool beginFlyUp;
    private bool controllsEnabled;
    private Animator m_animator;
    private Rigidbody2D m_rigidbody;

    #endregion

    #region Public Methods

    public void Init(RuntimeAnimatorController _playerAnimator)
    {
        // -- CACHING MAIN COMPONENTS
        m_animator = GetComponentInChildren<Animator>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_animator.runtimeAnimatorController = _playerAnimator;
    }

    public override void OnGameStart()
    {
        // -- THE PLAYER IS MOVING TOWARDS THE INITIAL POSITION (IN THE CENTER OF SCREEN)
        transform.position = new Vector3(-10f, 0f, 0f);
        transform.DOMove(new Vector3(-1f, 0f, 0f), 1.5f).OnComplete(() =>
        {
            // -- AFTER REACHING TO INITIAL POSITION, PLAYER CONTROLLS WILL BE ENABLED
            controllsEnabled = true;
            m_rigidbody.velocity = Vector2.zero;
            m_rigidbody.bodyType = RigidbodyType2D.Dynamic;
        });
    }

    #endregion

    #region Private Methods

    private void Update()
    {
        // -- IF CONTROLLS ARE NOT ENABLED WE DON'T NEED TO PROCEED
        if (!controllsEnabled)
            return;

        // -- IF PLAYER START PRESSING THE MOUSE CLICK, HE'LL START MOVING UP
        if (Input.GetMouseButtonDown(0))
        {
            beginFlyUp = true;
            
            // -- RESETING PLAYER VELOCITY SO THAT HE CAN MOVE UP GRADUALLY
            m_rigidbody.velocity = Vector2.zero;
        }

        if (Input.GetMouseButtonUp(0))
        {
            // -- IF PLAYER IS NOT PRESSING MOUSE BUTTON HE CAN'T FLY SO HE'LL START FALLING
            beginFlyUp = false;
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

        // -- AND IF HE DON'T CLICK MOUSE BUTTON, HE'LL START FALLING IN DOWNWARD DIRECTION THROUGH RIGIDBODY
    }

    #endregion

    #region Collision Handling

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameContants.PLAYER))
            return;

        if (collision.CompareTag(GameContants.OBSTACLE))
        {
            // -- DEATH
        }
    }

    #endregion

}