using Godot;
using System;
using System.Diagnostics;

public partial class PlayerHotbar : Control
{
	private Inventory _inv;

	private HotbarSlot[] hotbar = new HotbarSlot[9];
	
	[Export] private StyleBox heldTexture;
	[Export] private StyleBox defTexture;

	private int heldSlot = -1;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_inv = (Inventory)GetParent();
		GetSetSlots();
	}

	public override void _EnterTree()
	{
		InventoryManager.onInventoryModified += Refresh;
		InventoryManager.onHeldItem += HeldItem;
	}

	private int HeldItem()
	{
		return heldSlot;
	}

	private void GetSetSlots()
	{
		var slots = GetChild(0).GetChildren();

		for (int l = 0; l < slots.Count; l++)
		{
			hotbar[l] = (HotbarSlot) slots[l];
			GD.Print(hotbar[l]);
		}
		
		for(int i = 0; i < hotbar.Length; i++)
		{
			hotbar[i].referenceSlot = _inv.InventorySlots[i];
		}
	}

	public void Refresh()
	{
		for (int i = 0; i < hotbar.Length; i++)
		{
			hotbar[i].RefreshSlot(_inv.inventory[i]);
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Pressed)
		{
			int prev = heldSlot;
			switch (keyEvent.Keycode)
			{
				case Key.Key1:
					heldSlot = 0;
					break;
				case Key.Key2:
					heldSlot = 1;
					break;
				case Key.Key3:
					heldSlot = 2;
					break;
				case Key.Key4:
					heldSlot = 3;
					break;
				case Key.Key5:
					heldSlot = 4;
					break;
				case Key.Key6:
					heldSlot = 5;
					break;
				case Key.Key7:
					heldSlot = 6;
					break;
				case Key.Key8:
					heldSlot = 7;
					break;
				case Key.Key9:
					heldSlot = 8;
					break;
			}

			//GD.Print("Slot " + heldSlot);
			if(prev != -1) hotbar[prev].RemoveThemeStyleboxOverride("normal");
			if(heldSlot >= 0) hotbar[heldSlot].AddThemeStyleboxOverride("normal", heldTexture);
		}
	}
}
