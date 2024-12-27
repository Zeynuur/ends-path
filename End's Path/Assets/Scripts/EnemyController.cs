using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;

public enum EnemyState
{
    Idle,
    Wander,
    Follow,
    Die,
    Attack
};

public enum EnemyType
{
    Melee,
    Ranged
};

public class EnemyController : MonoBehaviour
{

    GameObject player;
    public EnemyState currentState = EnemyState.Idle;
    public EnemyType enemyType;


    public float range;
    public float speed;
    public float attackRange;
    public float bulletSpeed;
    public float coolDown;


    private bool chooseDirection = false;
    private bool dead = false;
    private bool coolDownAttack = false;
    public bool notInRoom = false;


    private Vector3 randomDirection;
    public GameObject bulletPrefab; 


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        switch(currentState)
        {
            // case(EnemyState.Idle):
            //     Idle();
            //     break;
            case(EnemyState.Wander):
                Wander();
                break;
            case(EnemyState.Follow):
                Follow();
                break;
            case(EnemyState.Die):
                Death();
                break;
            case(EnemyState.Attack):
                Attack();
                break;
        }

        if(!notInRoom)
        { 
            if (IsPlayerInRange(range)&& currentState != EnemyState.Die)
            {
                currentState = EnemyState.Follow;
            }
            else if (!IsPlayerInRange(range) && currentState != EnemyState.Die)
            {
                currentState = EnemyState.Wander;
            }

            if(Vector3.Distance(transform.position, player.transform.position) <= attackRange)
            {
                currentState = EnemyState.Attack;
            }
        }
        else
        {
            currentState = EnemyState.Idle;
        }
    }

    private bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    private IEnumerator ChooseDirection()
    {
        chooseDirection = true;
        yield return new WaitForSeconds(Random.Range(2f,8f));
        randomDirection = new Vector3(0, 0, Random.Range(0,360));
        Quaternion nextRotation = Quaternion.Euler(randomDirection);
        transform.rotation = Quaternion.Lerp(transform.rotation, nextRotation, Random.Range(0.5f, 2.5f));
        chooseDirection = false;
    }
    
    void Wander()
    {
        if (!chooseDirection)
        {
            StartCoroutine(ChooseDirection());
        }

        transform.position += -transform.right * speed * Time.deltaTime;

        if (IsPlayerInRange(range))
        {
            currentState = EnemyState.Follow;
        }
    }

    void Follow()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    void Attack()
    {
        if(!coolDownAttack)
        {
            switch(enemyType)
            {
                case(EnemyType.Melee):
                    GameController.DamagePlayer(1);
                    StartCoroutine(CoolDown());
                    break;
                case(EnemyType.Ranged):
                    GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
                    bullet.GetComponent<BulletController>().GetPlayer(player.transform);
                    bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
                    bullet.GetComponent<BulletController>().isEnemyBullet = true;
                    StartCoroutine(CoolDown());
                    break;
            }
        }            
    }

    private IEnumerator CoolDown()
    {
        coolDownAttack = true;
        yield return new WaitForSeconds(coolDown);
        coolDownAttack = false;
    }

    public void Death()
    {
        Destroy(gameObject);
    }

}
