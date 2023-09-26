using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour //Doorクラスの定義
{
    public enum DoorType
    {
        left, right, top, bottom
    }

    public DoorType doorType;

    public GameObject doorCollider;
    private GameObject player;
    private float widthOffset = 11f;
    private float HeightOffset = 8f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            switch(doorType)
            {
                case DoorType.bottom:
                    player.transform.position = new Vector2(transform.position.x, transform.position.y - HeightOffset + 1.5f);
                    break;
                case DoorType.left:
                    player.transform.position = new Vector2(transform.position.x - widthOffset, transform.position.y);
                    break;
                case DoorType.right:
                    player.transform.position = new Vector2(transform.position.x + widthOffset, transform.position.y);
                    break;
                case DoorType.top:
                    player.transform.position = new Vector2(transform.position.x, transform.position.y + HeightOffset + 0.9f);
                    break;
            }
        }
    }
}
