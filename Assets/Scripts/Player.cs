using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    /*
     * プレイヤーに必要な情報を持っておくクラス
     * 数値に関する処理用のメソッドを持つ可能性はあるが、入力に関連する処理は他のスクリプトに書く
     */
    public int MaxHP;
    public int HP;
    public float ActSpan;

    public List<List<DangeonTile>> Map;

    public int x;
    public int y;

    public List<int> Items;
}
