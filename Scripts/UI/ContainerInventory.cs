using Godot;
using System;

public partial class ContainerInventory : Inventory
{
	public ItemContainer container;
	
	public override void _Ready(){
		InventoryManager.Instance.containerInventory = this;
	}
	
	public override void SelectSlot(int id){
		if (id < 0 || id > (columns * rows) - 1) return;
		
		//if holding shift.... quick move item instead.
		
		var returnedItem = container.SelectItem(inventory[id]);
		
		if(returnedItem != null){
			inventory[id] = returnedItem;
		} else{
			inventory[id] = null;
		}
		
		RefreshInventory();
	}
	
	public InventoryItem[] ClearInventory(){
		var oldInventory = inventory;
		
		inventory = new InventoryItem[36];
		for(int i = 0; i < (rows * columns); i++){
			InventorySlots[i].QueueFree();
		}
		InventorySlots = new InventorySlot[36];
		
		
		
		return oldInventory;
		//Destroy old inventory stuff.
	}
}
