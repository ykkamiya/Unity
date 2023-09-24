using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRoomSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct RandomSpawner //構造体
    {
        public string name;
        public SpawnerData SpawnerData; //SpawnerData型の変数(SpawnerData.csでクラス定義している、参照できる)
    }

    public GridController grid; //参照元グリッドデータ
    public RandomSpawner[] spawnerData;

    void Start()
    {
        //grid = GetComponentInChildren<GridController>();
    }

    public void InitialiseObjectSpawning()
    {
        foreach(RandomSpawner rs in spawnerData)
        {
            SpawnObjects(rs);
        }
    }

    void SpawnObjects(RandomSpawner data) //randomspawner構造体を与えるとそこからオブジェクトをスポーンさせる
    {
        int randomIteration = Random.Range(data.SpawnerData.minSpawn, data.SpawnerData.maxSpawn) + 1; //+1

        for(int i = 0; i < randomIteration; i++)
        {
            int randomPos = Random.Range(0, grid.availablePoints.Count - 1);
            GameObject go = Instantiate(data.SpawnerData.itemToSpawn, grid.availablePoints[randomPos], Quaternion.identity, transform) as GameObject; //未使用グリッドからランダムに選んでオブジェクト生成
            grid.availablePoints.RemoveAt(randomPos); //使ったグリッドをリストから消す
            Debug.Log("Spawned Object");;
        }

    }
}
