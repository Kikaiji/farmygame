using Godot;
using System;
using System.Collections.Generic;

public partial class Inventory : Control
{
	public override void _Ready()
	{
		
	}
	
	[Export] public int rows = 4;
	[Export] public int columns = 9;
	[Export] private PackedScene invSlot;
	
	public void Setup(){
		ItemDatabase.Instance.Setup();
		
		InventoryGrid = GetChild(0).GetChild(0);
		
		Inv = InventoryGrid as CanvasItem;
		var grid = InventoryGrid as GridContainer;
		
		grid.Columns = columns;
		for(int i = 0; i < (rows * columns); i++){
			var newSlot = invSlot.Instantiate() as InventorySlot;
			newSlot.id = i;
			InventoryGrid.AddChild(newSlot);
			newSlot.Pressed += () => SelectSlot(newSlot.id);
			InventorySlots[i] = newSlot;
		}
	}
	
	public override void _EnterTree(){
		InventoryManager.onAddItem += AddItem;
		InventoryManager.onRemoveItem += TryRemoveItem;
		InventoryManager.onGetItem += GetItemInSlot;
		InventoryManager.onToggleInv += ToggleVisibility;
	}
	
	public override void _ExitTree(){
		InventoryManager.onAddItem -= AddItem;
		InventoryManager.onRemoveItem -= TryRemoveItem;
		InventoryManager.onGetItem -= GetItemInSlot;
		InventoryManager.onToggleInv -= ToggleVisibility;
	}

	public delegate void SlotSelected(int slot);

	public event SlotSelected onSlotSelected;

	
	
	public InventorySlot[] InventorySlots { get; set; } = new InventorySlot[36];
	public InventoryItem[] inventory { get; set; } = new InventoryItem[36];

	private Node InventoryGrid;
	private CanvasItem Inv;
	private int first;
	
	private bool open = false;


	public int AddItem(string itemId, int amount)
	{
		if (GetFirstAvailableSlot() == -1 && CountItem(itemId) == 0)
		{
			GD.Print("Inventory Full");
			return -2;
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
		
		RefreshInventory();
	}

	private void SwapFirst(int id)
	{
		if(first != -1) 
		{
			SwapSecond(id);
			return;
		}
		first = id;
	}

	private void SwapSecond(int id)
	{
		SwapItem(first, id); 
		first = -1;
	}
	
	public InventoryItem ReplaceItem(InventoryItem item, int slot){
		var old = inventory[slot];
		inventory[slot] = item;
		RefreshInventory();
		
		return old;
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

	public virtual void SelectSlot(int id)
	{
		//GD.Print("slot " + id);
		if (id < 0 || id > (columns * rows) - 1) return;
		
		//if holding shift.... quick move item instead.
		
		//InventoryManager.Instance.SelectItem(this, id);
		//onSlotSelected?.Invoke(id);
	}

	public virtual void RefreshInventory()
	{
		for (int i = 0; i < inventory.Length; i++)
		{
			InventorySlots[i].RefreshSlot(inventory[i]);
		}
	}

	public string GetItemInSlot(int slot)
	{
		if (inventory[slot] == null) return null;
		return inventory[slot].item.itemId;
	}
	
	private void ToggleVisibility(){
		RefreshInventory();
		
		open = !open;
		Inv.SetVisible(open);
		
		first = -1;
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
