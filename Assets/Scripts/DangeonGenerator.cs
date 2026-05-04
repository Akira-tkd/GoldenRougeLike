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

public class Room
{
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

    public List<GameObject> Players;
    [SerializeField] Tilemap _tilemap;

    [SerializeField] int _mapHeight;
    [SerializeField] int _mapWidth;

    private List<List<DangeonTile>> _map;

    private int _roomCount;
    private int _roomMinSize;
    private int _roomMaxSize;

    public float ExtraConnectionChance;

    const int WALL = 0;
    const int FLOOR = 1;

    int[,] _mapinfo;
    List<Room> rooms = new List<Room>();

    public void Generate(int seed)
    {
        System.Random rand = new System.Random(seed);
        _mapinfo = new int[_mapWidth, _mapHeight];
        _map = new List<List<DangeonTile>>(_mapHeight);

        Fill(WALL);
        GenerateRoom(rand);
        ConnectRooms(rand);
        MapSwap();
        PlayerSpwan(rand);
        // RoomDebug();
    }

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

    void GenerateRoom(System.Random rand)
    {
        float density = (float)(rand.NextDouble() * (0.50f - 0.25f) + 0.25f);

        int mapArea = _mapWidth * _mapHeight;
        int targetRoomArea = (int)(mapArea * density);

        int roomCountA = rand.Next(4, 15);
        int roomCountB = rand.Next(0, 3);
        _roomCount = roomCountA - roomCountB;
        int avgRoomArea = targetRoomArea / _roomCount;
        int baseSize = Mathf.RoundToInt(Mathf.Sqrt(avgRoomArea));

        _roomMinSize = Mathf.Clamp(baseSize - 2, 3, 5);
        _roomMaxSize = Mathf.Clamp(baseSize + 2, 4, 10);

        int trying = 0;
        while(rooms.Count < _roomCount && trying < _roomCount * 50)
        {
            trying++;

            int width = rand.Next(_roomMinSize, _roomMaxSize + 1);
            int height = rand.Next(_roomMinSize, _roomMaxSize + 1);

            int x = rand.Next(1, _mapWidth - width - 1);
            int y = rand.Next(1, _mapHeight - height - 1);

            Room newRoom = new Room(x, y, width, height);

            bool overlaps = false;
            foreach(var  room in rooms)
            {
                if (newRoom.isIntersect(room))
                {
                    overlaps = true;
                    break;
                }
            }

            if (overlaps) continue;

            rooms.Add(newRoom);
            SetRoom(newRoom);
        }
    }

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

    void ConnectRooms(System.Random rand)
    {
        List<Room> connected = new List<Room>();
        List<Room> unconnected = rooms;

        connected.Add(unconnected[0]);
        unconnected.RemoveAt(0);

        while(unconnected.Count > 0)
        {
            float bestDist = float.MaxValue;
            Room bestA = null;
            Room bestB = null;

            foreach(var a in connected)
            {
                foreach (var b in unconnected)
                {
                    float dist = Vector2Int.Distance(a.Center, b.Center);
                    if(dist < bestDist)
                    {
                        bestDist = dist;
                        bestA = a;
                        bestB = b;
                    }
                }
            }

            CreateRoad(bestA.Center, bestB.Center, rand);

            connected.Add(bestB);
            unconnected.Remove(bestB);
        }
    }

    void CreateRoad(Vector2Int a, Vector2Int b, System.Random rand)
    {
        Vector2Int pos = a;

        if((float)rand.NextDouble() > 0.5f)
        {
            while(pos.x != b.x)
            {
                _mapinfo[pos.x, pos.y] = FLOOR;
                pos.x += (b.x > pos.x) ? 1 : -1;
            }

            while(pos.y != b.y)
            {
                _mapinfo[pos.x, pos.y] = FLOOR;
                pos.y += (b.y > pos.y) ? 1 : -1;
            }
        }
        else
        {
            while(pos.y != b.y)
            {
                _mapinfo[pos.x, pos.y] = FLOOR;
                pos.y += (b.y > pos.y) ? 1 : -1;
            }

            while(pos.x != b.x)
            {
                _mapinfo[pos.x, pos.y] = FLOOR;
                pos.x += (b.x > pos.x) ? 1 : -1;
            }
        }
    }

    void AddExtraConnections(System.Random rand)
    {
        for(int i = 0; i < rooms.Count; i++)
        {
            for(int j = i + 1; j < rooms.Count; j++)
            {
                if((float)rand.NextDouble() < ExtraConnectionChance)
                {
                    CreateRoad(rooms[i].Center, rooms[j].Center, rand);
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
                }
            }
        }
    }

    void PlayerSpwan(System.Random rand)
    {
        foreach(var player in Players)
        {
            var p = Instantiate(player);
            int x, y;
            while (true)
            {
                x = rand.Next(0, _mapWidth);
                y = rand.Next(0, _mapHeight);
                if (_mapinfo[x, y] == FLOOR)
                {
                    break;
                }
            }

            p.GetComponent<PlayerMove>().Init(_map, _tilemap, x, y);
        }
    }

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

    public List<List<DangeonTile>> GetMap()
    {
        return _map;
    }
}
