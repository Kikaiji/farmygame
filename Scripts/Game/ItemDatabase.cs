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
	
	public List<ItemData> ItemList { get; private set; }

	public void Setup()
	{
		ItemList = ResourceFileLoader.Instance.LoadFolder<ItemData>("res://Resources/ItemDatas/");
	}
}
