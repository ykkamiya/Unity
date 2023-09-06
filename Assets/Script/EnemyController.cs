using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState //enum型...複数の定数(列挙子)を一つの型で統一して管理できるらしい
{
    Wander, //未発見
    Follow, //発見
    Die //死
};

public class EnemyController : MonoBehaviour
{
    GameObject player;
    public EnemyState currState = EnemyState.Wander;

    public float range; //視界の広さ
    public float speed; //移動速度
    private bool chooseDir = false;
    private bool dead = false;
    private Vector3 randomDir;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); //見つけたPlayerをplayer変数に格納
    }

    // Update is called once per frame
    void Update()
    {
        switch (currState)
        {
            case (EnemyState.Wander):
                Wander();
                break;
            case (EnemyState.Follow):
                Follow();
                break;
            case (EnemyState.Die):

                break;
        }

        if(IsPlayerInRange(range) && currState != EnemyState.Die) //playerが視界内かつenemyがdieではない
        {
            currState = EnemyState.Follow; //currstateをfollowにする
        }
        else if(!IsPlayerInRange(range) && currState != EnemyState.Die)
        {
            currState = EnemyState.Wander;
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
        Quaternion nextRotation = Quaternion.Euler(randomDir); //Quaternion...unityでは回転を扱う際に用いる数
        transform.rotation = Quaternion.Lerp(transform.rotation, nextRotation, Random.Range(0.5f, 2.5f));
        chooseDir = false;
    }
    void Wander()
    {
        if(!chooseDir)
        {
            StartCoroutine(ChooseDirection()); //並列処理の開始
        }

        transform.position += -transform.right * speed * Time.deltaTime; //deltatime...最後のフレームを完了するのに要した時間
        if(IsPlayerInRange(range))
        {
            currState = EnemyState.Follow;
        }
    }

    void Follow()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    public void Death()
    {
        Destroy(gameObject);
    }
}
