using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public enum TileInfo
{
    Room,
    Road,
    Door,
    UnWalkable
}

// 生成される部屋のクラス
public class Room
{
    /*
     * 部屋の左上の座標と横幅、縦幅、中心の座標をパラメータに持つ
     * 他の部屋との重なりを検知するメソッドを持つ
     * 上下左右で隣接する座標のリストも持つ
     */
    public int x;
    public int y;
    public int width;
    public int height;

    public Room(int x, int y, int width, int height)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }

    public Vector2Int Center
    {
        get
        {
            return new Vector2Int(
                x + width / 2,
                y + height / 2);
        }
    }

    public bool isIntersect(Room other)
    {
        return !(x + width + 1 < other.x ||
                other.x + other.width + 1 < x ||
                y + height + 1 < other.y ||
                other.y + other.height + 1 < y);
    }
}

public class DangeonGenerator : MonoBehaviour
{
    /*
     * 乱数からDungeonTileの二次元リストを生成するメソッドを持つ
     * 二次元リストを簡単に参照できるようにするメソッドも持つ
     */

    public List<GameObject> Players;  // マルチプレイ想定のためスポーンするプレイヤーのリストを持つ
    [SerializeField] GameObject _itemObject;
    [SerializeField] Tilemap _tilemap;  // プレイヤーがタイルマップを参照したいため、生成時に渡せるように持っておく

    [SerializeField] int _mapHeight;  // マップの縦幅の最大値
    [SerializeField] int _mapWidth;  // マップの横幅の最大値
    [SerializeField] List<ItemData> _popItems;  // そのフロアに出現する可能性のあるアイテムのリスト

    private List<List<DangeonTile>> _map;  // マップ情報を持つ二次元リスト

    // 下三つのintは乱数で決まる
    private int _roomCount;  // 部屋の数
    private int _roomMinSize;  // 部屋の最小サイズ
    private int _roomMaxSize;  // 部屋の最大サイズ

    public float ExtraConnectionChance;  // 一つの部屋に対して余分に接続路が生成される確率

    // 地形生成中は仮でintの二次元配列を使うため、それの意味定義
    const int WALL = 0;
    const int FLOOR = 1;
    const int ROAD = 2;

    int[,] _mapinfo;  // 仮で使う地形情報の配列
    List<Room> rooms = new List<Room>();  // 生成した部屋の数

    // 地形生成用のメソッド
    // 乱数固定用の引数seedが必要
    public void Generate(int seed)
    {
        // 初期化処理
        System.Random rand = new System.Random(seed);
        _mapinfo = new int[_mapWidth, _mapHeight];
        _map = new List<List<DangeonTile>>(_mapHeight);

        Fill(WALL);  // 仮地形を一旦壁で埋める

        GenerateRoom(rand);  // 部屋を生成する
        ConnectRooms(rand);  // 部屋を道で接続する
        AddExtraConnections(rand);  // ランダムで追加接続をする

        MapSwap();  // 仮情報を二次元リストに移す

        PlayerSpwan(rand);  // プレイヤーをスポーンさせる
        ItemSpawn(rand);  // アイテムをスポーンさせる

        // RoomDebug();  // 地形をテキスト表示するデバッグ用のメソッド
    }

    // 仮情報配列を全マス壁として初期化
    void Fill(int tile)
    {
        for(int x = 0; x < _mapWidth; x++)
        {
            for(int y = 0; y < _mapHeight; y++)
            {
                _mapinfo[x, y] = tile;
            }
        }
    }

    // 部屋をランダムで生成する
    void GenerateRoom(System.Random rand)
    {
        float density = (float)(rand.NextDouble() * (0.50f - 0.25f) + 0.25f);  // フロア全体の内、部屋が締める面積の割合

        int mapArea = _mapWidth * _mapHeight;  // フロア全体の面積
        int targetRoomArea = (int)(mapArea * density);  // 全部屋の面積(目安)

        int roomCountA = rand.Next(4, 15);
        int roomCountB = rand.Next(0, 3);
        _roomCount = roomCountA - roomCountB;  // 部屋の数
        int avgRoomArea = targetRoomArea / _roomCount;  // 平均的な部屋の大きさ
        int baseSize = Mathf.RoundToInt(Mathf.Sqrt(avgRoomArea));  // 部屋の面積の基礎値

        _roomMinSize = Mathf.Clamp(baseSize - 2, 3, 5);  // 部屋の幅の最小値
        _roomMaxSize = Mathf.Clamp(baseSize + 2, 4, 10);  // 部屋の幅の最大値

        int trying = 0;  // 無限ループ回避用の試行回数カウント
        while(rooms.Count < _roomCount && trying < _roomCount * 50)
        {
            trying++;

            int width = rand.Next(_roomMinSize, _roomMaxSize + 1);  // 生成する部屋の横幅
            int height = rand.Next(_roomMinSize, _roomMaxSize + 1);  // 生成する部屋の縦幅

            int x = rand.Next(1, _mapWidth - width - 1);  // 生成する部屋の左上の点のx座標
            int y = rand.Next(1, _mapHeight - height - 1);  // 生成する部屋の左上の点のy座標

            Room newRoom = new Room(x, y, width, height);  // 部屋としてのインスタンス

            bool overlaps = false;  // 新たに生成する部屋が既存の物と重なっているかの検知
            foreach(var  room in rooms)  // 現時点で生成している部屋との重なりを検証する
            {
                if (newRoom.isIntersect(room))
                {
                    overlaps = true;
                    break;
                }
            }

            if (overlaps) continue;  // 一つでも重なりがあればやり直し

            rooms.Add(newRoom);  // 生成済みの部屋としてリストに追加
            SetRoom(newRoom);  // 部屋の位置を仮情報配列に反映
        }
    }
    
    // 部屋となる部分を仮情報配列に反映する
    void SetRoom(Room room)
    {
        for(int x = room.x; x < room.x + room.width; x++)
        {
            for(int y = room.y; y < room.y + room.height; y++)
            {
                _mapinfo[x, y] = FLOOR;
            }
        }
    }

    // 部屋同士を通路で繋げる
    void ConnectRooms(System.Random rand)
    {
        // 全部屋がつながっている必要があるため、接続済みか否かを二つのリストで管理する
        List<Room> connected = new List<Room>();
        List<Room> unconnected = rooms;

        // 最初の一つ目を接続済みに追加して、未接続から削除
        connected.Add(unconnected[0]);
        unconnected.RemoveAt(0);

        while(unconnected.Count > 0)
        {
            float bestDist = float.MaxValue;  // 距離探索をするための初期化
            Room bestA = null;
            Room bestB = null;

            // 接続済みの部屋と未接続の部屋で最も距離が近いものを探す
            // Aが接続済み、Bが未接続
            foreach(var a in connected)
            {
                foreach (var b in unconnected)
                {
                    float dist = Vector2Int.Distance(a.Center, b.Center);  // 任意の二部屋の距離
                    if(dist < bestDist)  // 最短を更新するような長さの場合
                    {
                        bestDist = dist;
                        bestA = a;
                        bestB = b;
                    }
                }
            }

            CreateRoad(bestA, bestB, rand);  // 二つの部屋を接続する通路を生成する

            connected.Add(bestB);  // Bを接続済みのリストに追加 
            unconnected.Remove(bestB);  // Bを未接続から削除
        }
    }

    // 二部屋間の通路を生成する
    void CreateRoad(Room aRoom, Room bRoom, System.Random rand)
    {
        Vector2Int a = aRoom.Center;
        Vector2Int b = bRoom.Center;
        Vector2Int pos = a;  // 通路を伸ばす初期地点
        int xDirection = (b.x > pos.x) ? 1 : -1;
        int yDirection = (b.y > pos.y) ? 1 : -1;

        // 目標地点に向けて横から延ばすか縦から延ばすかを決める
        if ((float)rand.NextDouble() > 0.5f)
        {
            // 横から延ばす
            while(pos.x != b.x)
            {
                _mapinfo[pos.x, pos.y] = ROAD;
                pos.x += xDirection;
            }

            while(pos.y != b.y)
            {
                _mapinfo[pos.x, pos.y] = ROAD;
                pos.y += yDirection;
            }
        }
        else
        {
            // 縦から延ばす
            while(pos.y != b.y)
            {
                _mapinfo[pos.x, pos.y] = ROAD;
                pos.y += yDirection;
            }

            while(pos.x != b.x)
            {
                _mapinfo[pos.x, pos.y] = ROAD;
                pos.x += xDirection;
            }
        }
    }

    // 任意の二部屋間に余分に通路を生成する
    void AddExtraConnections(System.Random rand)
    {
        for(int i = 0; i < rooms.Count; i++)
        {
            for(int j = i + 1; j < rooms.Count; j++)
            {
                if((float)rand.NextDouble() < ExtraConnectionChance)
                {
                    CreateRoad(rooms[i], rooms[j], rand);
                }
            }
        }
    }

    // int型の配列で作った地形情報をDangeonTileの二次元配列に変えるためのメソッド
    void MapSwap()
    {
        for(int y = 0; y < _mapHeight; y++)
        {
            _map.Add(new List<DangeonTile>());
            for(int x = 0; x < _mapWidth; x++)
            {
                _map[y].Add(new DangeonTile());
                if (_mapinfo[x, y] == WALL)
                {
                    _map[y][x].IsWall = true;
                }
                else
                {
                    _map[y][x].IsWall = false;
                    if (_mapinfo[x, y] == ROAD)
                    {
                        _map[y][x].IsRoad = true;
                    }
                }
            }
        }
        DangeonManager.Instance.MapSetter(_map);
    }

    // プレイヤーを任意の部屋にスポーンさせる
    void PlayerSpwan(System.Random rand)
    {
        foreach(var player in Players)  // マルチプレイ想定のためプレイヤーはリスト
        {
            var p = Instantiate(player);  // ゲームオブジェクトとして生成
            int x, y;
            while (true)  // スポーン地点が部屋内になるまで無限ループ
            {
                x = rand.Next(0, _mapWidth);
                y = rand.Next(0, _mapHeight);
                if (_mapinfo[x, y] == FLOOR)
                {
                    break;
                }
            }

            var pPlayer = p.GetComponent<Player>();

            // 座標などの初期情報を与える
            pPlayer.Position = new Vector2Int(x, y);
            pPlayer.Map = _map;

            var gm = p.GetComponent<GridMoving>();
            gm.Init(_map, _tilemap);
            gm.SetGridPosition(pPlayer.Position);
        }
    }

    // アイテムをランダムに生成する
    void ItemSpawn(System.Random rand)
    {
        int itemAmount = rand.Next(4, 16);
        for (int i = 0; i < itemAmount; i++)
        {
            var itemObj = Instantiate(_itemObject);  // ゲームオブジェクトとして生成
            int x, y;
            while (true)  // スポーン地点が部屋内になるまで無限ループ
            {
                x = rand.Next(0, _mapWidth);
                y = rand.Next(0, _mapHeight);

                if (_mapinfo[x, y] != WALL && _mapinfo[x,y] != ROAD && _map[y][x].OnItem == null)
                {
                    break;
                }
            }

            var itemData = _popItems[rand.Next(0, _popItems.Count)];
            Item item = new Item(itemData);

            var obj = itemObj.GetComponent<ItemObject>();
            obj.Init(new Vector2Int(x, y), _tilemap, item.Data.Kind);

            _map[y][x].OnItem = item;
            DangeonManager.Instance.AddItem(obj, item);
        }
    }

    // 地形生成デバッグ用のテキスト形式での出力
    void RoomDebug()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        for(int y = _mapHeight - 1; y >= 0; y--)
        {
            for(int x = 0; x < _mapWidth; x++)
            {
                sb.Append(_mapinfo[x, y] == WALL ? "#" : " ");
            }
            sb.AppendLine();
        }

        Debug.Log(sb.ToString());
    }

    // 他スクリプトが地図情報を使うためのパブリックメソッド
    public List<List<DangeonTile>> GetMap()
    {
        return _map;
    }
}
