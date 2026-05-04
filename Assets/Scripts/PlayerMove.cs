using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using DG.Tweening;

public class PlayerMove : MonoBehaviour
{
    /*
     * プレイヤーの入力による移動を管理する
     * 攻撃についてもここにする予定
     * プレイヤーの位置はステータス扱いで、Playerクラスが値を保持する
     */

    [SerializeField] float _walkTime;

    private List<List<DangeonTile>> _map;
    private Tilemap _tilemap;
    private Player _player;

    private Vector2Int _gridPos;
    private bool _isCanMove;

    public void Init(List<List<DangeonTile>> map, Tilemap tileMap, int x, int y)
    {
        _map = map;
        _tilemap = tileMap;

        _isCanMove = true;

        _player = GetComponent<Player>();
        _player.x = x;
        _player.y = y;

        SetGridPosition(new Vector2Int(x, y));
    }

    void SetGridPosition(Vector2Int pos, bool instant = true)
    {
        _gridPos = pos;

        Vector3 worldPos = _tilemap.GetCellCenterWorld(new Vector3Int(pos.x, pos.y, 0));
        if (instant)
        {
            transform.position = worldPos;
        }
        else
        {
            WalkMovie(worldPos);
        }
    }

    void Update()
    {
        if (!_isCanMove) return;

        Vector2Int dir = Vector2Int.zero;

        // InputSystemに変更する
        if (Input.GetKeyDown(KeyCode.W)) dir = Vector2Int.up;
        if (Input.GetKeyDown(KeyCode.S)) dir = Vector2Int.down;
        if (Input.GetKeyDown(KeyCode.A)) dir = Vector2Int.left;
        if (Input.GetKeyDown(KeyCode.D)) dir = Vector2Int.right;

        if(dir != Vector2Int.zero)
        {
            TryMove(dir);
        }
    }

    void TryMove(Vector2Int dir)
    {
        Vector2Int target = _gridPos + dir;

        if (IsWalkable(target))
        {
            _player.x += dir.x;
            _player.y += dir.y;
            SetGridPosition(target, false);
        }
    }

    bool IsWalkable(Vector2Int target)
    {
        if (_map[target.y][target.x].IsWall || _map[target.y][target.x].IsWater)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void WalkMovie(Vector3 worldPos)
    {
        _isCanMove = false;
        transform.DOMove(worldPos, _walkTime)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                _isCanMove = true;
            });
    }
}
