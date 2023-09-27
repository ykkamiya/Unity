using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState //enum型...複数の定数(列挙子)を一つの型で統一して管理できるらしい
{
    Idle, //非アクティブ
    Wander, //未発見
    Follow, //発見
    Die, //死
    Attack
};

public enum EnemyType
{
    Melee, //近接
    Ranged //遠距離持ち
}

public class EnemyController : MonoBehaviour
{

    GameObject player;
    public EnemyState currState = EnemyState.Idle;
    public EnemyType enemyType;
    public float range; //視界の広さ
    public float speed; //移動速度
    public float attackRange;
    public float bulletSpeed;
    public float coolDown;
    private bool chooseDir = false;
    private bool dead = false;
    private bool coolDownAttack = false;
    public bool notInRoom = false; //アクティブルームかどうか
    private Vector3 randomDir;
    public GameObject bulletPrefab;
    GameObject rootObj;

    Animator anim = null;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); //見つけたPlayerをplayer変数に格納
        anim = GetComponent<Animator>();
        rootObj = transform.root.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.rotation = 
        switch (currState)
        {
            case (EnemyState.Idle):
                //Idle();
                break;
            case (EnemyState.Wander):
                Wander();
                break;
            case (EnemyState.Follow):
                Follow();
                break;
            case (EnemyState.Die):
                break;
            case (EnemyState.Attack):
                Attack();
                break;
        }

        if(!notInRoom) //プレイヤーと同じ部屋にいる
        {
            if (IsPlayerInRange(range) && currState != EnemyState.Die) //playerが視界内かつenemyがdieではない
            {
                currState = EnemyState.Follow; //currstateをfollowにする
            }
            else if (!IsPlayerInRange(range) && currState != EnemyState.Die)
            {
                currState = EnemyState.Wander;
            }
            if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
            {
                currState = EnemyState.Attack;
            }
        }
        else
        {
            currState = EnemyState.Idle;
        }

    }

    private bool IsPlayerInRange(float range)
    {
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    private IEnumerator ChooseDirection() //IEnumerator...非同期処理を行うための型
    {
        chooseDir = true;
        yield return new WaitForSeconds(Random.Range(2f, 8f)); //yield return...ここで作業を中断し,一定時間メインの処理に戻す
        randomDir = new Vector3(0, 0, Random.Range(0, 360));
        //Quaternion nextRotation = Quaternion.Euler(randomDir); //Quaternion...unityでは回転を扱う際に用いる数
        //transform.rotation = Quaternion.Lerp(transform.rotation, nextRotation, Random.Range(0.5f, 2.5f));
        chooseDir = false;
    }
    void Wander()
    {
        if (!chooseDir)
        {
            StartCoroutine(ChooseDirection()); //並列処理の開始
        }

        transform.position += -transform.right * speed * Time.deltaTime; //deltatime...最後のフレームを完了するのに要した時間
        if (IsPlayerInRange(range))
        {
            currState = EnemyState.Follow;
        }
    }

    void Follow()
    {
        Vector3 scale = transform.localScale;

        float x1 = transform.position.x;
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        float x2 = transform.position.x;
        float movechangeX = x2 - x1; //x方向に変化した差分

        if(movechangeX > 0)
        {
            if(transform.localScale.x > 0)
            {
                scale.x *= -1f;
            }
        }
        else
        {
            if(transform.localScale.x < 0)
            {
                scale.x *= -1f;
            }
        }

        transform.localScale = scale;
    }

    void Attack()
    {
        if (!coolDownAttack)
        {
            switch (enemyType)
            {
                case (EnemyType.Melee):
                    GameController.DamagePlayer(1);
                    StartCoroutine(CoolDown()); //遅延してCoolDown処理->無敵時間の実装
                    break;
                case (EnemyType.Ranged):
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
        yield return new WaitForSeconds(coolDown); //coolDown秒待機
        coolDownAttack = false;
    }
    public void Death()
    {
        if(enemyType == EnemyType.Ranged)
        {
            Destroy(gameObject);
            return;
        }
        StartCoroutine(DeathDelay());
    }

    IEnumerator DeathDelay()
    {
        anim.SetTrigger("Died");
        anim.Update(0f);
        var state = anim.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(state.length);
        RoomController.instance.StartCoroutine(RoomController.instance.RoomCoroutine()); //ここで部屋の更新処理を呼び出す
        Destroy(gameObject);
    }
}
