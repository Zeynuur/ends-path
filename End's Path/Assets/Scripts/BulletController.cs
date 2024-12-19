using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public float lifeTime;
    public bool isEnemyBullet = false;
    private Vector2 lastPosition;
    private Vector2 currentPosition;
    private Vector2 playerPosition;

    void Start()
    {
        StartCoroutine(DeathDelay());
        if (!isEnemyBullet)
        {
            transform.localScale = new Vector2(GameController.BulletSize, GameController.BulletSize);
        }
    }

    void Update()
    {
        if (isEnemyBullet)
        {
            currentPosition = transform.position;
            transform.position = Vector2.MoveTowards(transform.position, playerPosition, 5f * Time.deltaTime );
            if(currentPosition == lastPosition)
            {
                Destroy(gameObject);
            }
            lastPosition = currentPosition;
        }
    }

    public void GetPlayer(Transform player)
    {
        playerPosition = player.position;
    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Enemy" && !isEnemyBullet)
        {
            col.gameObject.GetComponent<EnemyController>().Death();
            Destroy(gameObject);
        }

        if(col.tag == "Player" && isEnemyBullet)
        {
            GameController.DamagePlayer(1);
            Destroy(gameObject);
        }
    }
}
