using Godot;
using System;

public partial class ItemContainer : Area3D
{
	private ContainerInventory containerInventory;
	private InventoryItem[] containerItems;
	[Export] private int rows = 5;
	[Export] private int columns = 5;
	private bool open = false;
	
	public override void _Ready(){
		containerItems = new InventoryItem[rows * columns];
	}
	
	private void InteractBehaviour()
	{
		GD.Print("Interacted with " + Name);
		if(!open)
		{
			containerInventory = InventoryManager.Instance.OpenContainer(this);
			containerInventory.rows = rows;
			containerInventory.columns = columns;
			containerInventory.container = this;
			containerInventory.Setup();
			
			containerInventory.inventory = containerItems;
			containerInventory.RefreshInventory();
			open = true;
		} else{
			CloseInventory();
		}
	}

	public void OnBodyEnter(Node3D body)
	{
		if (body is not PlayerMovement other) return;

		other.onPlayerInteract += InteractBehaviour;
		other.prompt.SetPrompt(Name);
	}

	public void OnBodyExit(Node3D body)
	{
		if (body is not PlayerMovement other) return;

		if(open) 
		{
			CloseInventory();
		}
		other.onPlayerInteract -= InteractBehaviour;
		other.prompt.SetPrompt("");
	}
	
	public int TryAddItem(string itemId, int count = 1)
	{
		if (containerInventory != null)
		{
			var result = containerInventory.AddItem(itemId, count);
			//var result = onAddItem.Invoke(itemId, count);
			containerInventory.RefreshInventory();
			return result;
		}
		return -2;
	}
	
	public bool TryRemoveItem(string itemId, int count = 1, int slot = -1)
	{
		if (containerInventory != null)
		{
			var result = containerInventory.TryRemoveItem(itemId, count, slot);
			//var result = onRemoveItem.Invoke(itemId, count, slot);
			containerInventory.RefreshInventory();
			return result;
		}
		return false;
	}
	
	public string GetInventoryItem(int id)
	{
		if (containerInventory != null)
		{
			return containerInventory.GetItemInSlot(id);
			//return onGetItem.Invoke(id);
		}
		return null;
	}
	
	public InventoryItem SelectItem(InventoryItem item)
	{
		return CursorSlot.Instance.TakeItem(item);
	}
	
	private void CloseInventory(){
		containerItems = InventoryManager.Instance.CloseContainer();
		open = false;
	}
}
