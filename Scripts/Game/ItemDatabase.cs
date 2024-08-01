using Godot;
using System;
using System.Collections.Generic;

public class ItemDatabase
{
	private static ItemDatabase _instance;

	public static ItemDatabase Instance
	{
		get { return _instance ??= new ItemDatabase(); }
		private set => _instance = value;
	}
	
	public Dictionary<string, ItemData> ItemList { get; private set; }

	public void Setup()
	{
		ItemList = ResourceFileLoader.Instance.LoadFolderAsDict<ItemData>("res://Resources/ItemDatas/");
	}

	public ItemData GetItem(string itemId)
	{
		ItemData result;
		if (ItemList.TryGetValue(itemId, out result)) return result;
		return null;
	}
}
