using UnityEngine;
using System.Collections.Generic;

public class Player : Charactor
{
    /*
     * プレイヤーに必要な情報を持っておくクラス
     * 数値に関する処理用のメソッドを持つ可能性はあるが、入力に関連する処理は他のスクリプトに書く
     */

    public List<List<DangeonTile>> Map;

    public int x;
    public int y;

    public List<Item> Items;

    public void GetItem()
    {
        Item item = Map[y][x].OnItem;
        Items.Add(item);
        Map[y][x].OnItem = null;
        Debug.Log($"{item.Name}をゲットしました!");
        DangeonManager.Instance.Changed();
    }
}
