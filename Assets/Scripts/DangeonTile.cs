using UnityEngine;

public class DangeonTile
{
    /*
     * ダンジョンの1マスごとの情報を格納するためのクラス
     * マップ情報として二次元リストに詰め込まれる
     */

    public DangeonTile()
    {
        IsWall = true;
        IsWater = true;
    }

    // これらのboolはそのうち対応する型のインスタンスを入れる方向に切り替える予定
    public bool IsChara;
    public bool IsItem;
    public bool IsTrap;

    // これらのboolはboolのままの予定
    public bool IsWall;
    public bool IsWater;
}
