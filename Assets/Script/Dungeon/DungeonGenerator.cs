using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public DungeonGenerationData dungeonGenerationData;
    private List<Vector2Int> dungeonRooms;

    private void Start()
    {
        dungeonRooms = DungeonCrawlerController.GenerateDungeon(dungeonGenerationData); //ダンジョン座標生成関数を実行
        SpawnRooms(dungeonRooms); //上で生成された座標に実際のマップを生成
    }

    private void SpawnRooms(IEnumerable<Vector2Int> rooms)
    {
        RoomController.instance.LoadRoom("Start", 0, 0); //[0,0]にstartを生成
        foreach (Vector2Int roomLocation in rooms) //foreach...与えられた配列データに対して各データでループ実行
        {
            RoomController.instance.LoadRoom("Empty", roomLocation.x, roomLocation.y); //roomsの座標にemptyルームを生成         
        }
    }
}
