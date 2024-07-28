using Godot;
using System;

public partial class PlayerHotbar : Control
{
	private PlayerInventory _inv;

	private HotbarSlot[] hotbar = new HotbarSlot[9];

	private int heldSlot;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_inv = (PlayerInventory)GetParent();
		GetSetSlots();
	}

	public override void _EnterTree()
	{
		//_inv.onSlotSelected += Refresh;
		InventoryManager.onHeldItem += HeldItem;
	}

	private string HeldItem()
	{
		if (_inv.inventory[heldSlot].item == null) return null;
		return _inv.inventory[heldSlot].item.itemId;
	}

	private void GetSetSlots()
	{
		var slots = GetChild(0).GetChildren();

		for (int l = 0; l < slots.Count; l++)
		{
			hotbar[l] = (HotbarSlot) slots[l];
		}
		
		for(int i = 0; i < hotbar.Length; i++)
		{
			hotbar[i].referenceSlot = _inv.InventorySlots[i];
		}
	}

	public void Refresh(int n = 0)
	{
		for (int i = 0; i < hotbar.Length; i++)
		{
			hotbar[i].RefreshSlot(_inv.inventory[i]);
		}
	}
}
