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
}
