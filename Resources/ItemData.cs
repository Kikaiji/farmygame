using Godot;
using System;

public partial class ItemData : Resource
{
    [Export] public int itemId;
    [Export] public string itemName;
    [Export] public CompressedTexture2D itemSprite;

    public ItemData()
    {
        itemId = -1;
        itemName = "nan";
        itemSprite = new CompressedTexture2D();
    }

    public ItemData(int ItemId, string ItemName, CompressedTexture2D ItemSprite)
    {
        itemId = ItemId;
        itemName = ItemName;
        itemSprite = ItemSprite;
    }
}
