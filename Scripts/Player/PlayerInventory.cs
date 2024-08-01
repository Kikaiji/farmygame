using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerInventory : Control
{
	public override void _Ready()
	{
		ItemDatabase.Instance.Setup();
		
		InventoryGrid = GetChild(0).GetChild(0);
		var children = InventoryGrid.GetChildren();

		for (int i = 0; i < children.Count; i++)
		{
			InventorySlots[i] = (InventorySlot)children[i];
			InventorySlots[i].id = i;
		}

		onSlotSelected += SwapFirst;

		InventoryManager.onAddItem += AddItem;
		InventoryManager.onRemoveItem += TryRemoveItem;
	}

	public delegate void SlotSelected(int slot);

	public event SlotSelected onSlotSelected;

	
	
	public InventorySlot[] InventorySlots { get; private set; } = new InventorySlot[36];
	public InventoryItem[] inventory { get; private set; } = new InventoryItem[36];

	private Node InventoryGrid;

	private int first, second;


	public int AddItem(string itemId, int amount)
	{
		if (GetFirstAvailableSlot() == -1 && CountItem(itemId) == 0)
		{
			GD.Print("Inventory Full");
			return -1;
		}

		Dictionary<int, int> slots = LookForItem(itemId);
		int remaining = amount;
		int max = ItemDatabase.Instance.GetItem(itemId).maxStack;
		
		foreach (var slot in slots)
		{
			if ((remaining = inventory[slot.Key].AddCount(remaining)) < 0)
			{
				RefreshInventory();
				return -1;
			}
		}

		while (remaining > 0)
		{
			int slot = 0;
			if ((slot = GetFirstAvailableSlot()) == -1)
			{
				RefreshInventory();
				GD.Print("No Room for remaining items.");
				return remaining;
			}
			inventory[slot] = new InventoryItem(ItemDatabase.Instance.GetItem(itemId));
			remaining = inventory[slot].AddCount(remaining);
		}
		RefreshInventory();
		return -1;

	}

	public int GetFirstAvailableSlot()
	{
		for (int i = 0; i < inventory.Length; i++)
		{
			if (inventory[i] == null) return i;
		}

		return -1;
	}

	public void SwapItem(int start, int end)
	{
		(inventory[end], inventory[start]) = (inventory[start], inventory[end]);
		
		InventorySlots[start].RefreshSlot(inventory[start]);
		InventorySlots[end].RefreshSlot(inventory[end]);
	}

	private void SwapFirst(int id)
	{
		first = id;
		onSlotSelected -= SwapFirst;
		onSlotSelected += SwapSecond;

	}

	private void SwapSecond(int id)
	{
		SwapItem(first, id); 
		first = 0;
		onSlotSelected += SwapFirst;
		onSlotSelected -= SwapSecond;
	}

	public bool TryRemoveItem(string itemId, int count = 1, int slot = -1)
	{
		if (slot == -1)
		{
			return RemoveItemByType(itemId, count);
		}
		else
		{
			return RemoveItemBySlot(itemId, slot, count);
		}
	}

	public bool RemoveItemBySlot(string itemId, int slot, int count = 1)
	{
		if (slot > inventory.Length || slot < 0)
		{
			GD.PushWarning("Slot number " + slot + " is invalid.");
			return false;
		}

		if (inventory[slot].Count < count)
		{
			GD.PushWarning("Amount to remove (" + count + ") is greater than which currently exists in the specified slot");
			return false;
		}

		if (inventory[slot].item.itemId != itemId)
		{
			GD.PushWarning("Specified item is not in that slot");
			return false;
		}
		
		if ((inventory[slot].RemoveCount(count)) >= 0) inventory[slot] = null;
		RefreshInventory();
		return true;
	}

	public bool RemoveItemByType(string itemId, int count = 1)
	{
		if (CountItem(itemId) < count)
		{
			GD.PushWarning("Number of item specified in inventory (" + count + ") is greater than which currently exists.");
			return false;
		}

		Dictionary<int, int> itemCounts = LookForItem(itemId);

		int remaining = count;
		foreach (var slot in itemCounts)
		{
			if (slot.Value <= remaining)
			{
				remaining -= slot.Value;
				RemoveItemBySlot(itemId, slot.Key, slot.Value);
			}
			else
			{
				RemoveItemBySlot(itemId, slot.Key, remaining);
				break;
			}
		}

		return true;
	}
	
	public Dictionary<int, int> LookForItem(string itemId)
	{
		Dictionary<int, int> result = new Dictionary<int, int>();
		for (int i = 0; i < inventory.Length; i++)
		{
			if (inventory[i] != null && inventory[i].item.itemId == itemId) result.Add(i, inventory[i].Count);
		}

		PrintDict(result);
		return result;
	}

	private void PrintDict(Dictionary<int, int> dict)
	{
		GD.Print("Dictionary");
		foreach (var VARIABLE in dict)
		{
			GD.Print("Slot " + VARIABLE.Key + " Count " + VARIABLE.Value);
		}
	}

	public int CountItem(string itemID)
	{
		int amount = 0;

		for (int i = 0; i < inventory.Length; i++)
		{
			if (inventory[i] != null && inventory[i].item.itemId == itemID) amount += inventory[i].Count;
		}
		
		return amount;
	}

	public void SelectSlot(int id)
	{
		if (id < 0 || id > 35) return;
		
		onSlotSelected?.Invoke(id);
	}

	public void RefreshInventory()
	{
		for (int i = 0; i < inventory.Length; i++)
		{
			InventorySlots[i].RefreshSlot(inventory[i]);
		}
	}

	private string GetItemInSlot(int slot)
	{
		return inventory[slot].item.itemId;
	}

}

public class InventoryItem
{
	public ItemData item;
	public int Count { get; private set; }

	
	public InventoryItem()
	{
		
	}

	public InventoryItem(ItemData data)
	{
		item = data;
	}

	public int AddCount(int add)
	{
		Count += add;

		if (Count > item.maxStack)
		{
			int remaining = Count - item.maxStack;
			Count -= remaining;
			return remaining;
		}

		return -1;
	}

	public int RemoveCount(int minus)
	{
		Count -= minus;

		if (Count <= 0)
		{
			int remaining = -Count;
			return remaining;
		}

		return -1;
	}
}
