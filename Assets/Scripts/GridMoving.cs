using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using DG.Tweening;

public class GridMoving : MonoBehaviour
{
    /*
     * 移動に関するクラス
     * プレイヤーも敵も共通の処理を使える部分が多いため共有できるように整理
     * 入力などは別のスクリプトで記述し、あくまで移動に関する処理をするクラスとして扱う
     */

    [SerializeField] float _walkTime;  // 移動にかかる時間

    private List<List<DangeonTile>> _map;  // 二次元リストで管理されているマップ(フロア)の情報
    private Tilemap _tilemap;  // 実際に描写するタイルマップオブジェクト

    private Vector2Int _gridPos;  // マスとしての現在位置を表す座標
    private Vector2Int _moveDirection;  // 移動する向き。入力によって値が変わる

    private bool _isCanMove;  // 今移動が可能かを表すbool

    // 今後複数のメソッドで扱う数値の初期化
    public void Init(List<List<DangeonTile>> map, Tilemap tilemap)
    {
        _map = map;
        _tilemap = tilemap;
        _isCanMove = true;
    }

    // 移動先が移動可能な場合、数値を更新する
    public Vector2Int TryMove(
        bool diag, Vector2Int startPos, Vector2Int moveDirection)
    {
        if (_isCanMove)
        {
            // 今後複数のメソッドで扱う数値の初期化
            _gridPos = startPos;
            _moveDirection = moveDirection;

            Vector2Int target = _gridPos + _moveDirection;

            if (IsWalkable(target, diag))
            {
                var returnPos = SetGridPosition(target, false);  // 実際のオブジェクト位置の更新
                return returnPos;
            }
        }
        return new Vector2Int(int.MaxValue, int.MaxValue);
    }

    // 座標数値上の移動を実際の場所移動に反映させるメソッド
    // 瞬間移動とDOTweenによる非同期的な移動の二つを使い分けられる
    public Vector2Int SetGridPosition(Vector2Int pos, bool instant = true)
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

        return _gridPos;
    }

    // 移動先の移動可否に関する判定
    bool IsWalkable(Vector2Int target, bool diag)
    {
        if (diag)
        {
            if (_map[_gridPos.y][target.x].IsWall || _map[target.y][_gridPos.x].IsWall)
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
