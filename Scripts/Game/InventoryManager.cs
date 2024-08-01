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

	public delegate void InventoryModify();

	public static event InventoryModify onInventoryModified;

	public delegate int ItemAdd(string itemId, int count = 1);
	public static event ItemAdd onAddItem;

	public delegate bool ItemRemove(string itemId, int count = 1, int slot = -1);
	public static event ItemRemove onRemoveItem;

	public delegate string GetItem(int id);
	public static event GetItem onGetItem;
	
	public delegate HotbarSlot HeldItem();
	public static event HeldItem onHeldItem;

	public int TryAddItem(string itemId, int count = 1)
	{
		if (onAddItem != null)
		{
			onInventoryModified?.Invoke();
			return onAddItem.Invoke(itemId, count);
		}
		return -2;
	}
	
	public bool TryRemoveItem(string itemId, int count = 1, int slot = -1)
	{
		if (onRemoveItem != null)
		{
			onInventoryModified?.Invoke();
			return onRemoveItem.Invoke(itemId, count, slot);
		}
		return false;
	}

	public string GetInventoryItem(int id)
	{
		if (onGetItem != null)
		{
			return onGetItem.Invoke(id);
		}
		return null;
	}

	public HotbarSlot GetHeldItem()
	{
		if(onHeldItem != null) return onHeldItem.Invoke();

		return null;
	}
}
