using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour //部屋をグリッド管理
{
    public Room room;

    [System.Serializable]
    public struct Grid
    {
        public int columns, rows;
        public float verticalOffset, horizontalOffset;
    }

    public Grid grid;
    public GameObject gridTile;
    public List<Vector2> availablePoints = new List<Vector2>(); //生成したグリッドのうち、オブジェクトが存在していないもののリスト->ObjectRoomSpawnerで参照する

    void Awake()
    {
        room = GetComponentInParent<Room>(); //部屋情報を取得
        grid.columns = room.Width - 2; //部屋の横幅を定義?
        grid.rows = room.Height - 2;
        GenerateGrid(); //グリッドを生成
    }

    void GenerateGrid() //グリッドを生成する関数
    {
        grid.verticalOffset += room.transform.localPosition.y;
        grid.horizontalOffset += room.transform.localPosition.x;
        
        for(int y = 0; y < grid.rows; y++)
        {
            for(int x = 0; x < grid.columns; x++)
            {
                GameObject go = Instantiate(gridTile, transform); //Instantiate...オブジェクトをクローン生成する関数, transform...オブジェクトの座標を保存しているクラス, gridTileを生成
                go.GetComponent<Transform>().position = new Vector2(x - (grid.columns - grid.horizontalOffset), y - (grid.rows - grid.verticalOffset)); //grid座標と実際の座標を変換
                go.name = "X: " + x + ", Y: " + y;
                availablePoints.Add(go.transform.position); //未使用グリッドリストに追加
                go.SetActive(false); //gridを非表示化
            }
        }

        GetComponentInParent<ObjectRoomSpawner>().InitialiseObjectSpawning();
    }


}
