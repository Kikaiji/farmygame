using Godot;
using System;

public partial class PlayerInventory : Inventory
{
	
	public override void _Ready(){
		
		InventoryManager.Instance.playerInventory = this as Inventory;
		
		Setup();
	}
	
	public override void SelectSlot(int id){
		if (id < 0 || id > (columns * rows) - 1) return;
		
		//if holding shift.... quick move item instead.
		
		var returnedItem = InventoryManager.Instance.SelectItem(inventory[id]);
		
		if(returnedItem != null){
			inventory[id] = returnedItem;
		} else{
			inventory[id] = null;
		}
		
		RefreshInventory();
	}
	
	public override void RefreshInventory(){
		InventoryManager.Instance.FullRefresh();
		
		for (int i = 0; i < inventory.Length; i++)
		{
			InventorySlots[i].RefreshSlot(inventory[i]);
		}
	}
}
