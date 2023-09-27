using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControllerForDebug : MonoBehaviour
{
    public float lifeTime;
    public bool isEnemyBullet = false; //敵の弾かのフラグ
    public bool isFamiliarBullet = false;

    private Vector2 lastPos;
    private Vector2 curPos;
    private Vector2 playerPos;

    public GameObject ExpirePrefab;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DeathDelay()); //コルーチン, エフェクトを遅延させる
        if (!isEnemyBullet && !isFamiliarBullet) //敵の弾ではない
        {
            transform.localScale = new Vector2(GameController.BulletSize, GameController.BulletSize);
        }

        if(isFamiliarBullet)
        {
            transform.localScale = new Vector2(GameController.BulletSize * 0.8f, GameController.BulletSize * 0.8f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnemyBullet) //敵の弾である
        {
            //弾の発射処理
            curPos = transform.position; //敵弾の現在座標を保存
            transform.position = Vector2.MoveTowards(transform.position, playerPos, 5f * Time.deltaTime);
            if(curPos == lastPos)
            {
                Instantiate(ExpirePrefab, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            lastPos = curPos;
        }
    }

    public void GetPlayer(Transform player) //プレイヤーの現在位置を取得
    {
        playerPos = player.position;
    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(lifeTime);
        Instantiate(ExpirePrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name == "Duke Fly" && !isEnemyBullet)
        {
            col.gameObject.GetComponent<DukeflyController>().DealtDmg(1f); //dukeflyの2つの当たり判定両方にヒットして二回実行されてるっぽい...要修正
            Instantiate(ExpirePrefab, transform.position, transform.rotation);
            Destroy(gameObject);
            return;
        }

        if (col.tag == "Enemy" && !isEnemyBullet)
        {
            col.gameObject.GetComponent<EnemyControllerForDebug>().Death(); //getcomponent...unity editorにあるcomponentからデータを取得
            Instantiate(ExpirePrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        if(col.tag == "Player" && isEnemyBullet)
        {
            GameController.DamagePlayer(1);
            Instantiate(ExpirePrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
