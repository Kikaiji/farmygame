using Godot;
using System;
using System.Collections.Generic;
using LitJson;

public partial class ItemData : Resource
{
	[Export] public string itemId;
	[Export] public string itemName;
	[Export] public Texture2D itemSprite;
	[Export] public int maxStack;
	[Export] public Godot.Collections.Array<ItemTags> tags;

	public ItemData()
	{
		itemId = "nan";
		itemName = "nan";
		itemSprite = new Texture2D();
		maxStack = 8;
	}

	public ItemData(string ItemId, string ItemName, Texture2D ItemSprite, int MaxStack, Godot.Collections.Array<ItemTags> Tags)
	{
		itemId = ItemId;
		itemName = ItemName;
		itemSprite = ItemSprite;
		maxStack = MaxStack;
		tags = Tags;
	}
}

public enum ItemTags
{
	Seed,
	Tool,
	Food
}
