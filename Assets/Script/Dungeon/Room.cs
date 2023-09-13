﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{

    public int Width;
    public int Height;
    public int X;
    public int Y;
    
    // Start is called before the first frame update
    void Start()
    {
        if(RoomController.instance == null)
        {
            Debug.Log("You pressed play in the wrong scene!");
            return;
        }

        RoomController.instance.RegisterRoom(this);
    }

    void OnDrawGizmos() //部屋の範囲に赤い枠を描画
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(Width, Height, 0));
    }

    public Vector3 GetRoomCentre()
    {
        return new Vector3( X * Width, Y * Height);
    }

    void OnTriggerEnter2D(Collider2D other) //triggerした時, となりの部屋のtriggerを作動させた時
    {
        if(other.tag == "Player")
        {
            RoomController.instance.OnPlayerEnterRoom(this); //triggerが作動したthis=隣の部屋にOnPlayerEnterRoomする
        }
    }
}
