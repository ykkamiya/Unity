using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class RoomInfo
{
    public string name;
    public int X;
    public int Y;
}

public class RoomController : MonoBehaviour
{

    public static RoomController instance;

    string currentWorldName = "Basement";

    RoomInfo currentLoadRoomData;

    Room currRoom;

    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>(); //Queue...FIFO型の構造体

    public List<Room> loadedRooms = new List<Room>();
    
    bool isLoadingRoom = false;
    bool spawnedBossRoom = false;
    bool updatedRooms = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // LoadRoom("Start", 0, 0); //初期部屋生成
        // LoadRoom("Empty", 1, 0);
        // LoadRoom("Empty", -1, 0);
        // LoadRoom("Empty", 0, 1);
        // LoadRoom("Empty", 0, -1);

    }

    void Update()
    {
        UpdateRoomQueue();
        Debug.Log("fuck");
    }

    void UpdateRoomQueue()
    {
        if(isLoadingRoom) //生成済か？
        {
            return;
        }

        if(loadRoomQueue.Count == 0) //生成待ちのマップは無い？
        {
            if(!spawnedBossRoom)
            {
                StartCoroutine(SpawnBossRoom());
            }
            else if(spawnedBossRoom && !updatedRooms)
            {
                foreach(Room room in loadedRooms)
                {
                    room.RemoveUnconnectedDoors();
                }
                UpdateRooms();
                updatedRooms = true;
            }
            return;
        }

        currentLoadRoomData = loadRoomQueue.Dequeue(); //リストから取り出す
        isLoadingRoom = true;

        StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
    }

    IEnumerator SpawnBossRoom()
    {
        spawnedBossRoom = true;
        yield return new WaitForSeconds(0.5f);
        if(loadRoomQueue.Count == 0)
        {
            Room bossRoom = loadedRooms[loadedRooms.Count - 1];
            Room tempRoom = new Room(bossRoom.X, bossRoom.Y);
            Destroy(bossRoom.gameObject);
            var roomToRemove = loadedRooms.Single(r => r.X == tempRoom.X && r.Y == tempRoom.Y);
            loadedRooms.Remove(roomToRemove);
            LoadRoom("End", tempRoom.X, tempRoom.Y);
        }
    }

    public void LoadRoom(string name, int x, int y) //部屋を生成する関数
    {
        if(DoesRoomExist(x, y)) //既にその座標に部屋がある->処理終了
        {
            return;
        }

        RoomInfo newRoomData = new RoomInfo(); //型を生成
        newRoomData.name = name;
        newRoomData.X = x;
        newRoomData.Y = y; //キューに追加

        loadRoomQueue.Enqueue(newRoomData); //この座標を部屋生成待ちリストに追加する
    }

    IEnumerator LoadRoomRoutine(RoomInfo info) //ラグなくオブジェクトを生成するためのルーチン
    {
        string roomName = currentWorldName + info.name; //部屋名生成

        AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);

        while(loadRoom.isDone == false)
        {
            yield return null;
        }
    }

    public void RegisterRoom( Room room) //部屋を生成する処理？
    {
        if(!DoesRoomExist(currentLoadRoomData.X, currentLoadRoomData.Y))
        {
            room.transform.position = new Vector3(
                currentLoadRoomData.X * room.Width,
                currentLoadRoomData.Y * room.Height,
                0
            );

            room.X = currentLoadRoomData.X;
            room.Y = currentLoadRoomData.Y;
            room.name = currentWorldName + "-" + currentLoadRoomData.name + " " + room.X + ", " + room.Y;
            room.transform.parent = transform; //tranform...インスペクターを取得する, parent...親から取得

            isLoadingRoom = false;

            if(loadedRooms.Count == 0)
            {
                CameraController.instance.currRoom = room; //生成マップが無い場合、最初に生成されたマップにカメラを合わせる->start
            }

            loadedRooms.Add(room); //生成済みリストに追加
        }
        else
        {
            Destroy(room.gameObject);
            isLoadingRoom = false;
        }
    }
    
    public bool DoesRoomExist( int x, int y)
    {
        return loadedRooms.Find( item => item.X == x && item.Y == y) != null;
    }

    public Room FindRoom( int x, int y)
    {
        return loadedRooms.Find( item => item.X == x && item.Y == y);
    }

    public string GetRandomRoomName() //部屋をランダムに選択する
    {
        string[] possibleRooms = new string[] {
            "Empty",
            "Basic1"
        };

        return possibleRooms[Random.Range(0, possibleRooms.Length)];
    }

    public void OnPlayerEnterRoom(Room room)
    {
        CameraController.instance.currRoom = room;
        GameObject.FindWithTag("Familiar").GetComponent<Familiar>().currRoomChanged = true;
        currRoom = room;

        StartCoroutine(RoomCoroutine());
    }


    public IEnumerator RoomCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        UpdateRooms();
    }

    public void UpdateRooms() //部屋更新
    {
        foreach(Room room in loadedRooms)
        {
            if(currRoom != room)
            {
                EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();
                if(enemies != null)
                { //非enemyの場合
                    foreach(EnemyController enemy in enemies)
                    {
                        enemy.notInRoom = true;
                        Debug.Log("Not in room");
                    }

                    foreach(Door door in room.GetComponentsInChildren<Door>())
                    {
                        door.doorCollider.SetActive(false); //ドアの当たり判定を無効化
                    }
                }
                else
                { //enemyの場合
                    foreach(Door door in room.GetComponentsInChildren<Door>())
                    {
                        door.doorCollider.SetActive(false); //ドアの当たり判定を無効化
                    }
                }
            }
            else
            {
                EnemyController[] enemies = room.GetComponentsInChildren<EnemyController>();
                if(enemies.Length > 0)
                {
                    foreach(EnemyController enemy in enemies)
                    {
                        enemy.notInRoom = false;
                        Debug.Log("Not in room");
                    }

                    foreach(Door door in room.GetComponentsInChildren<Door>())
                    {
                        //StartCoroutine(Shutdoors());
                        door.doorCollider.SetActive(true); //ドアの当たり判定を有効化
                    }
                }
                else
                {
                    foreach(Door door in room.GetComponentsInChildren<Door>())
                    {
                        door.doorCollider.SetActive(false); //ドアの当たり判定を無効化
                    }
                }
            }
        }
    }

    IEnumerator Shutdoors()
    {
        yield return new WaitForSeconds(0.5f);
    }
}
