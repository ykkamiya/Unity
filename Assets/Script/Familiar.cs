﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Familiar : MonoBehaviour //Familiarの素体ステータスを定義, playercontrollerからソースを流用
{
    private float lastFire;
    private GameObject player;
    public FamiliarData familiar;
    private float lastOffsetX;
    private float lastOffsetY;
    public bool currRoomChanged = false;

    FamiliarAnimationController anim;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<FamiliarAnimationController>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); //入力 GetAxisは操作設定に設定されているキーの入力を取得する
        float vertical = Input.GetAxis("Vertical");

        float shootHor = Input.GetAxis("ShootHorizontal");
        float shootVert = Input.GetAxis("ShootVertical");
        if((shootHor != 0 || shootVert != 0) && Time.time > lastFire + familiar.fireDelay)
        {
            Shoot(shootHor, shootVert);
            lastFire = Time.time; //このフレーム実行時の時刻(time.time)
        }  

        if(horizontal != 0 || vertical != 0)
        {
            float offsetX = (horizontal < 0) ? Mathf.Floor(horizontal) : Mathf.Ceil(horizontal);
            float offsetY = (vertical < 0) ? Mathf.Floor(vertical) : Mathf.Ceil(vertical);
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, familiar.speed * Time.deltaTime);
            lastOffsetX = offsetX;
            lastOffsetY = offsetY;
        }
        else
        {
            if(!(transform.position.x < lastOffsetX + 0.5f) || !(transform.position.y < lastOffsetY + 0.5f))
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x - lastOffsetX, player.transform.position.y - lastOffsetY), familiar.speed * Time.deltaTime);
            }
        }

        familiar.speed = player.GetComponent<PlayerController>().speed * 0.83f;

        if(currRoomChanged)
        {
            GetComponent<Transform>().position = player.GetComponent<Transform>().position; //プレイヤーの元にワープ
            currRoomChanged = false;
        }
    }

    void Shoot(float x, float y)
    {
        GameObject bullet = Instantiate(familiar.bulletPrefab, transform.position, Quaternion.identity) as GameObject; //弾オブジェクトを定義
        float posX = (x < 0) ? Mathf.Floor(x) * familiar.speed : Mathf.Ceil(x) * familiar.speed * 1.5f;
        float posY = (y < 0) ? Mathf.Floor(y) * familiar.speed : Mathf.Ceil(y) * familiar.speed * 1.5f;
        bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(posX, posY);
        bullet.GetComponent<BulletController>().isFamiliarBullet = true;
        anim.animShoot.SetTrigger("Shooting");
    }
}
