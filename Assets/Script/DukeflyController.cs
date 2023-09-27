using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DukeflyController : MonoBehaviour
{
    enum EnemyState //enum型...複数の定数(列挙子)を一つの型で統一して管理できるらしい
    {
        Idle, //非アクティブ
        Die, //死
        Active
    };

    /*public struct SummoningEnemy //構造体
    {
        public string name;
        public int num; //SpawnerData型の変数(SpawnerData.csでクラス定義している、参照できる)
    }*/

    GameObject player;
    public GameObject [] enemies; //インスペクターから召喚用モブを設定
    EnemyState currState = EnemyState.Idle;
    public float speed; //移動速度
    public float range; //視界の広さ
    public float health; //ボス体力
    public float coolDown; 
    public float summonTime;
    public int summonNum; //召喚数
    public float SummonRange;
    private bool dead = false;
    private bool coolDownAttack = false; //接触無敵時間
    private bool coolDownSummon = false; //召喚クールダウン
    private Vector3 summonPos;
    public bool notInRoom = true; //アクティブルームかどうか

    Animator anim = null;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); //見つけたPlayerをplayer変数に格納
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0f)
        {
            currState = EnemyState.Die;
        }

        switch (currState)
        {
            case (EnemyState.Idle):
                break;
            case (EnemyState.Die):
                Death();
                break;
            case (EnemyState.Active):
                Active();
                break;
        }

        if(IsPlayerInRange(range) && currState != EnemyState.Die) //プレイヤーと同じ部屋にいる
        {
            currState = EnemyState.Active;
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
    private IEnumerator CoolDown()
    {
        coolDownAttack = true;
        yield return new WaitForSeconds(coolDown); //coolDown秒待機
        coolDownAttack = false;
    }

    private IEnumerator CoolDownSummon()
    {
        coolDownSummon = true;
        yield return new WaitForSeconds(summonTime); //summonTime秒待機
        coolDownSummon = false;
    }

    void Active()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime); //移動
        Summon(); //ザコ召喚
    }
    void Summon()
    {
        int randomNumber = Random.Range(0, enemies.Length);

        if (!coolDownSummon)
        {
            anim.SetTrigger("isSummon");
            Summonmob(enemies[randomNumber], SummonRange, summonNum);
            StartCoroutine(CoolDownSummon()); //遅延してCoolDown処理->無敵時間の実装
        }
    }

    void Summonmob(GameObject summoningenemy, float SummonRange, int summonNum)
    {
        for(var i = 0; i < summonNum; i++)
        {
            float radius = 3f;
            float angle = Random.Range(0, 360);
            float rad = angle * Mathf.Deg2Rad;
            float px = Mathf.Cos(rad) * SummonRange;
            float py = Mathf.Sin(rad) * SummonRange;
            summonPos = new Vector3(px +  transform.position.x, py +  transform.position.y, 0);
            Instantiate(summoningenemy, summonPos, transform.rotation);
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            if (!coolDownAttack)
                {
                    GameController.DamagePlayer(1);
                    StartCoroutine(CoolDown()); //遅延してCoolDown処理->無敵時間の実装
                }
        }
    }

    public void DealtDmg(float dmg)
    {
        health -= dmg;  
        Debug.Log("dmg is: " + dmg + "hp is: " + health);
    }

    public void Death()
    {
        StartCoroutine(DeathDelay());
    }

    IEnumerator DeathDelay()
    {
        anim.SetTrigger("isDeath");
        anim.Update(0f);
        var state = anim.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(state.length);
        Destroy(gameObject);
    }

}
