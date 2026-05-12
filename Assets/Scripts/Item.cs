using UnityEngine;

public class Item
{
    public Item(ItemData data)
    {
        Data = data;
    }

    public ItemData Data { get; private set; }
    public int Id;
}
