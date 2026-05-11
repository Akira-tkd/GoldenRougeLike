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

public class Item : ScriptableObject
{
    [Header("全アイテム共通要素")]
    public int ID;
    public string Name;
    public ItemKind Kind;
    public int Price;
}
