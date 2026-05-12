using UnityEngine;

public enum ItemKind
{
    Weapon,
    Shield,
    Arrow,
    Potion,
    MagicBook,
    MagicStone,
    Box,
    Food,
    Other
}

public class ItemData : ScriptableObject
{
    [Header("全アイテム共通要素")]
    public string Name;
    public ItemKind Kind;
    public int Price;
}
