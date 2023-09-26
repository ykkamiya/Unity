using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameController : MonoBehaviour
{

    public static GameController instance;

    //外部から参照できない、このスクリプトのみで用いる変数
    private static float health = 6;
    private static int maxHealth = 6;
    private static float moveSpeed = 3f;
    private static float fireRate = 0.5f;
    private static float bulletSize = 0.5f;

    private bool bootCollected = false;
    private bool screwCollected = false;

    public List<string> collectedNames = new List<string>();

    //上の変数を外部から参照できるようにするらしい(なんで？)
    public static float Health { get => health; set => health = value; } //=>っていうのはラムダ式ってやつらしい
    public static int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public static float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public static float FireRate { get => fireRate; set => fireRate = value; }
    public static float BulletSize { get => bulletSize; set => bulletSize = value; }

    public Text healthText;

    // Start is called before the first frame update
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "Health: " + health;
    }

    //ステータス変更関数(他スクリプトから呼び出せる)
    public static void DamagePlayer(int damage)
    {
        health -= damage;

        if(Health <= 0)
        {
            KillPlayer();
        }
    }

    public static void HealPlayer(float healAmount)
    {
        Health = Mathf.Min(maxHealth, health + healAmount);
    }
    public static void MoveSpeedChange(float speed)
    {
        moveSpeed += speed;
    }

        public static void FireRateChange(float rate)
    {
        fireRate -= rate; //rate減らす->速くなる
    }

    public static void BulletSizeChange(float size)
    {
        bulletSize += size;
    }

    public void UpdateCollectedItems(CollectionController item)
    {
        collectedNames.Add(item.item.name);

        foreach(string i in collectedNames)
        {
            switch(i)
            {
                case "Boot":
                    bootCollected = true;
                break;
                case "Screw":
                    screwCollected = true;
                break;
            }
        }

        if(bootCollected && screwCollected) //シナジー発生
        {
            FireRateChange(0.0625f);
        }
    }

    private static void KillPlayer()
    {

    }
}
