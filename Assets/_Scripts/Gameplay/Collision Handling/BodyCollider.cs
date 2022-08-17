using UnityEngine;
using Utilities.Audio;

public class BodyCollider : PlayerCollider
{

    public GameObject bombPickupEffect;
    private bool canDectectCollision;
    private PlayerController player;

    public override void Init(PlayerController _player)
    {
        player = _player;
    }

    public override void CollisionDectection(bool _val)
    {
        canDectectCollision = _val;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canDectectCollision)
            return;

        if (collision.CompareTag(GameContants.COIN))
        {
            player.SimpleCoinCollision(collision.gameObject);
        }

        if (collision.CompareTag(GameContants.ENEMY))
        {
            player.EnemyCollision(collision.gameObject);
        }

        if (collision.CompareTag(GameContants.OBSTACLE))
        {
            player.SimpleObstacleCollision(collision.gameObject);
        }

        if (collision.CompareTag(GameContants.MAGNET))
        {
            player.ActivateMagnet();
            collision.gameObject.SetActive(false);
        }

        if (collision.CompareTag(GameContants.SHIELD))
        {
            player.ActivateShield();
            collision.gameObject.SetActive(false);
        }

        if (collision.CompareTag(GameContants.BOMB))
        {
            player.ActivateBomb();
            bombPickupEffect.SetActive(true);
            collision.gameObject.SetActive(false);
            AudioController.Instance.PlayAudio(AudioName.BOMB_DESTROY);
        }
    }
}