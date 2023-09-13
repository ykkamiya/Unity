using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    }

    void UpdateRoomQueue()
    {
        if(isLoadingRoom) //生成済か？
        {
            return;
        }

        if(loadRoomQueue.Count == 0) //生成待ちのマップは無い？
        {
            return;
        }

        currentLoadRoomData = loadRoomQueue.Dequeue(); //リストから取り出す
        isLoadingRoom = true;

        StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
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
    
    public bool DoesRoomExist( int x, int y)
    {
        return loadedRooms.Find( item => item.X == x && item.Y == y) != null;
    }

    public void OnPlayerEnterRoom(Room room)
    {
        CameraController.instance.currRoom = room;
        currRoom = room;
    }
}
