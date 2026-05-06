using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
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

    private List<List<DangeonTile>> _map;  // 二次元リストで管理されているマップ(フロア)の情報
    private Tilemap _tilemap;  // 実際に描写するタイルマップオブジェクト
    private Player _player;  // 座標を他スクリプトで共有しやすくするためにPlayerに渡す

    private Vector2Int _gridPos;  // マスとしての現在位置を表す座標
    private Vector2Int _moveDirection;  // 移動する向き。入力によって値が変わる
    private bool _isCanMove;  // 今が移動可能かを表すbool

    // InputSystemのAction、Moveに割り当てるメソッド
    // 入力の方向を取得する
    public void OnMove(InputAction.CallbackContext context)
    {
        var inputVector = context.ReadValue<Vector2>();
        _moveDirection = new Vector2Int(Mathf.RoundToInt(inputVector.x), Mathf.RoundToInt(inputVector.y));
    }

    // 今後mapやtilemapが他のプレイヤー関係のスクリプトで使うことになった場合
    // 値の代入はPlayer側に全て託す可能性が高い
    public void Init(List<List<DangeonTile>> map, Tilemap tileMap, int x, int y)
    {
        _map = map;
        _tilemap = tileMap;

        _isCanMove = true;

        _player = GetComponent<Player>();
        _player.x = x;
        _player.y = y;

        SetGridPosition(new Vector2Int(x, y));  // 何があっても絶対にこれだけは必要
    }

    // 座標数値上の移動を実際の場所移動に反映させるメソッド
    // 瞬間移動とDOTweenによる非同期的な移動の二つを使い分けられる
    void SetGridPosition(Vector2Int pos, bool instant = true)
    {
        _gridPos = pos;  // 今の座標数値上の位置の更新

        Vector3 worldPos = _tilemap.GetCellCenterWorld(new Vector3Int(pos.x, pos.y, 0));  // 更新した座標のワールド空間の座標変換
        if (instant)  // 瞬間移動なら
        {
            transform.position = worldPos;
        }
        else  // そうでないなら(DOTweenを使うなら)
        {
            WalkMovie(worldPos);
        }
    }

    void Update()
    {
        if (!_isCanMove) return;

        var moveMagnitude = _moveDirection.sqrMagnitude;

        if (moveMagnitude > 1.4)  // 移動方向が斜めなら
        {
            TryMove(true);
        }
        else if(moveMagnitude > 0.1)  // 移動方向が十字なら
        {
            TryMove(false);
        }
    }

    // 移動先が移動可能な場合、数値を更新する
    void TryMove(bool diag)
    {
        Vector2Int target = _gridPos + _moveDirection;

        if (IsWalkable(target, diag))
        {
            _player.x += _moveDirection.x;  // 座標情報の更新
            _player.y += _moveDirection.y;

            SetGridPosition(target, false);  // 実際のオブジェクト位置の更新
        }
    }

    // 移動先の移動可否に関する判定
    bool IsWalkable(Vector2Int target, bool diag)
    {
        if (diag)
        {
            if (_map[_player.y][target.x].IsWall || _map[target.y][_player.x].IsWall)
            {
                return false;
            }
        }
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
