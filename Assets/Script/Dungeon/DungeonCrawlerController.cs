using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    top = 0,
    left = 1,
    down = 2,
    right = 3
};

public class DungeonCrawlerController : MonoBehaviour //よく分からん,,,マップ自動生成につかう
{
    public static List<Vector2Int> positionVisited = new List<Vector2Int>(); //部屋生成リスト(ここに格納された座標に部屋を生成する)
    private static readonly Dictionary<Direction, Vector2Int> directionMovementMap = new Dictionary<Direction, Vector2Int>
    {
        {Direction.top, Vector2Int.up},
        {Direction.left, Vector2Int.left},
        {Direction.down, Vector2Int.down},
        {Direction.right, Vector2Int.right}
    };

    public static List<Vector2Int> GenerateDungeon(DungeonGenerationData dungeonData) //ダンジョン"座標"生成
    {
        List<DungeonCrawler> dungeonCrawlers = new List<DungeonCrawler>(); //ダンジョン生成ヘッド格納用リスト

        for(int i = 0; i < dungeonData.numberOfCrawlers; i++)
        {
            dungeonCrawlers.Add(new DungeonCrawler(Vector2Int.zero));
        }

        int iterations = Random.Range(dungeonData.iterationMin, dungeonData.iterationMax); //どの距離の長さまで部屋を生成するか指定範囲内でランダム決定

        for(int i = 0; i< iterations; i++)
        {
            foreach(DungeonCrawler dungeonCrawler in dungeonCrawlers)
            {
                Vector2Int newPos = dungeonCrawler.Move(directionMovementMap); //上の辞書をMove関数の引数に与える
                positionVisited.Add(newPos); //部屋生成リストに座標追加
            }
        }

        return positionVisited;
    }
}
