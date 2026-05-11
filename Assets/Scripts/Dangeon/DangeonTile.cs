using UnityEngine;

public class DangeonTile
{
    /*
     * ダンジョンの1マスごとの情報を格納するためのクラス
     * マップ情報として二次元リストに詰め込まれる
     */

    public DangeonTile()
    {
        IsWall = false;
        IsWater = false;
        IsRoad = false;
        OnChara = null;
        OnItem = null;
    }

    // これらのboolはそのうち対応する型のインスタンスを入れる方向に切り替える予定
    public Charactor OnChara;
    public Item OnItem;
    public bool IsTrap;

    // これらのboolはboolのままの予定
    public bool IsWall;
    public bool IsWater;
    public bool IsRoad;
}
