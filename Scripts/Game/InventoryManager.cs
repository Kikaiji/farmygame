using Godot;
using System;

public partial class InventoryManager : Node
{
	private static InventoryManager _instance;

	public static InventoryManager Instance
	{
		get { return _instance ??= new InventoryManager(); }
		set => _instance = value;
	}

	public delegate int ItemAdd(string itemId, int count = 1);
	public static event ItemAdd onAddItem;

	public delegate bool ItemRemove(string itemId, int count = 1);
	public static event ItemRemove onRemoveItem;
	
	public delegate string HeldItem();
	public static event HeldItem onHeldItem;

	public int TryAddItem(string itemId, int count = 1)
	{
		if(onAddItem != null) return onAddItem.Invoke(itemId, count);
		return -2;
	}
	
	public bool TryRemoveItem(string itemId, int count = 1)
	{
		if (onRemoveItem != null) return onRemoveItem.Invoke(itemId, count);
		return false;
	}

	public string GetHeldItem()
	{
		if(onHeldItem != null) return onHeldItem.Invoke();

		return null;
	}
}
