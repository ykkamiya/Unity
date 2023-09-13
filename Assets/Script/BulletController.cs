﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float lifeTime;
    public bool isEnemyBullet = false; //敵の弾かのフラグ

    private Vector2 lastPos;
    private Vector2 curPos;
    private Vector2 playerPos;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DeathDelay()); //コルーチン, エフェクトを遅延させる
        if (!isEnemyBullet) //敵の弾ではない
        {
            transform.localScale = new Vector2(GameController.BulletSize, GameController.BulletSize);
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
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy" && !isEnemyBullet)
        {
            col.gameObject.GetComponent<EnemyController>().Death(); //getcomponent...unity editorにあるcomponentからデータを取得
            Destroy(gameObject);
        }

        if(col.tag == "Player" && isEnemyBullet)
        {
            GameController.DamagePlayer(1);
            Destroy(gameObject);
        }
    }
}