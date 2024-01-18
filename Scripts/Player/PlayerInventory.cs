using Godot;
using System;

public partial class PlayerInventory : Control
{
	public override void _Ready()
	{
		ItemDatabase.Instance.Setup();
		
		InventoryGrid = GetChild(0).GetChild(0);
		var children = InventoryGrid.GetChildren();

		for (int i = 0; i < children.Count; i++)
		{
			InventorySlots[i] = (InventorySlot) children[i];
			InventorySlots[i].id = i;
		}

		onSlotSelected += SwapFirst;
	}

	public delegate void SlotSelected(int slot);

	public event SlotSelected onSlotSelected;

	
	
	public InventorySlot[] InventorySlots { get; private set; } = new InventorySlot[36];
	public InventoryItem[] inventory { get; private set; } = new InventoryItem[36];

	private Node InventoryGrid;

	private int first, second;


	public bool AddItem(InventoryItem item, int slot, int amount)
	{
		if (slot > inventory.Length) return false;
		
		inventory[slot] = item;
		return true;
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
	}

	public void RemoveItem(int slot)
	{
		if (slot > inventory.Length) return;
		inventory[slot] = null;
	}

	public bool LookForItem(ItemData item, int count)
	{
		int amount = 0;
		for (int i = 0; i < inventory.Length; i++)
		{
			if (inventory[i] != null && inventory[i].id == item.itemId) amount += inventory[i].count;
		}

		if (amount >= count) return true;
		
		return false;
	}

	public void SelectSlot(int id)
	{
		if (id < 0 || id > 35) return;
		
		onSlotSelected?.Invoke(id);
	}
	
	


}

public class InventoryItem
{
	public int id;
	public string itemName;
	public CompressedTexture2D sprite;
	public int count;

	public InventoryItem()
	{
		
	}

	public InventoryItem(ItemData data)
	{
		id = data.itemId;
		itemName = data.itemName;
		sprite = (CompressedTexture2D) GD.Load("res://Art/Sprites/Plants/turnip test.png");
	}
}
