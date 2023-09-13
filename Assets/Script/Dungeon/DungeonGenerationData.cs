using UnityEngine;

[CreateAssetMenu(fileName = "DungeonGenerationData.asset", menuName = "DungeonGenerationData/Dungeon Data")] //Assetに作成できるタブ項目を登録
public class DungeonGenerationData : ScriptableObject
{
    public int numberOfCrawlers; //探索するヘッドの数
    public int iterationMin; //そのヘッドの最短距離
    public int iterationMax; //そのヘッドの最長距離
}
