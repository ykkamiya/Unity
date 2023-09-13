using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCrawler : MonoBehaviour
{
    public Vector2Int Position { get; set; }
    public DungeonCrawler(Vector2Int startPos)
    {
        Position = startPos;
    }

    public Vector2Int Move(Dictionary<Direction, Vector2Int> directionMovementMap) //部屋生成で距離を一つ動かす
    {
        Direction toMove = (Direction)Random.Range(0, directionMovementMap.Count); //辞書内からランダムで方向選択
        Position += directionMovementMap[toMove]; //部屋距離を増加させる
        return Position;
    }
}
