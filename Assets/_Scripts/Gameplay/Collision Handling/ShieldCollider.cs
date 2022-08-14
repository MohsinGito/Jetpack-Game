using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Audio;

public class ShieldCollider : PlayerCollider
{

    public GameData gameData;
    public SpriteRenderer shield;
    public GameObject ShieldUI;
    public Image shieldFillBar;
    public GameObject pickUpEffect;

    private bool canDectectCollision;
    private PlayerController player;

    public override void Init(PlayerController _player)
    {
        player = _player;
    }

    public override void CollisionDectection(bool _val)
    {
        canDectectCollision = _val;
        shield.enabled = _val;
        ShieldUI.SetActive(_val);

        if (_val)
        {
            shieldFillBar.fillAmount = 1;
            pickUpEffect.SetActive(false);
            pickUpEffect.SetActive(true);
            AudioController.Instance.PlayAudio(AudioName.PICKUP_COLLECT);
            shieldFillBar.DOFillAmount(0, gameData.shieldDuration)
                .OnComplete(() => { player.ActivateBodyDetection(); });
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canDectectCollision)
            return;

        if (collision.CompareTag(GameContants.COIN))
        {
            player.MagnetCoinCollision(collision.gameObject);
        }

        if (collision.CompareTag(GameContants.ENEMY))
        {
            player.EnemyCollision(collision.gameObject);
        }

        if (collision.CompareTag(GameContants.OBSTACLE))
        {
            player.ShieldObstacleCollision(collision.gameObject);
        }
    }
}