using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Audio;

public class MagnetCollider : PlayerCollider
{

    public GameData gameData;
    public SpriteRenderer magnet;
    public GameObject magnetUI;
    public Image magnetFillBar;
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
        magnet.enabled = _val;
        magnetUI.SetActive(_val);

        if (_val)
        {
            magnetFillBar.fillAmount = 1;
            pickUpEffect.SetActive(false);
            pickUpEffect.SetActive(true);
            AudioController.Instance.PlayAudio(AudioName.PICKUP_COLLECT);
            magnetFillBar.DOFillAmount(0, gameData.magnetDuration)
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
    }
}