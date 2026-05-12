using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class ItemObject : MonoBehaviour
{
    [SerializeField] SpriteRenderer _sr;
    [SerializeField] List<Sprite> _sprites;

    private Vector2Int _position;

    public void Init(Vector2Int pos, Tilemap tilemap, ItemKind kind)
    {
        _position = pos;
        Vector3 worldPos = tilemap.GetCellCenterWorld(new Vector3Int(pos.x, pos.y, 0));
        transform.position = worldPos;
        _sr.sprite = _sprites[(int)kind];

        DangeonManager.Instance.ItemObjectDict.Add(pos, this.gameObject);
    }
}
