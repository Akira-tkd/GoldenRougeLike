using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShowingMiniMap : MonoBehaviour
{
    [SerializeField] int _width;
    [SerializeField] int _height;
    [SerializeField] DangeonGenerator _dg;
    [SerializeField] RawImage _ri;
    // 機能テスト用にボタンクリックでテクスチャ適用するメソッド
    public void OnClicked()
    {
        var map = _dg.GetMap();

        Texture2D tex = new Texture2D(_width, _height);
        tex.filterMode = FilterMode.Point;
        tex.wrapMode = TextureWrapMode.Clamp;

        Color[] color = new Color[_width  * _height];

        for(int y = 0; y < _height; y++)
        {
            for(int  x = 0; x < _width; x++)
            {
                color[y * _width + x] = map[y][x].IsWall ? Color.black : Color.white;
            }
        }

        tex.SetPixels(color);
        tex.Apply();

        _ri.texture = tex;
    }
}
