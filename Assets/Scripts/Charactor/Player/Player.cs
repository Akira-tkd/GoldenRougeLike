using UnityEngine;
using System.Collections.Generic;

public class Player : Charactor
{
    /*
     * プレイヤーに必要な情報を持っておくクラス
     * 数値に関する処理用のメソッドを持つ可能性はあるが、入力に関連する処理は他のスクリプトに書く
     */

    public List<List<DangeonTile>> Map;

    public Vector2Int Position;

    public List<Item> Items = new List<Item>();

    public void GetItem()
    {
        Item item = Map[Position.y][Position.x].OnItem;
        Items.Add(item);
        Map[Position.y][Position.x].OnItem = null;
        Debug.Log($"{item.Data.Name}をゲットしました!");
        Debug.Log($"ID:{item.Id}");
        DangeonManager.Instance.DeleteItem(item.Id);
    }
}
