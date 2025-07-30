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
	
	public delegate int HeldItem();
	public static event HeldItem onHeldItem;
	
	public delegate void ToggleInv();
	public static event ToggleInv onToggleInv;
	
	public delegate void ItemSelect();
	public static event ItemSelect onItemSelected;
	
	public delegate void ItemDrop(GroundItem item);
	public static event ItemDrop onItemDropped;
	
	public Inventory playerInventory;
	public ItemContainer otherInventory;
	public ContainerInventory containerInventory;
	
	private Inventory selectOrigin;
	private int selectSlot;
	
	
	public int MoveItem(Inventory origin, int id){
		return -2;
	}
	
	public void SwapItem(Inventory firstInv, int firstSlot, Inventory secondInv, int secondSlot){
		
		var first = firstInv.inventory[firstSlot];
		var second = secondInv.inventory[secondSlot];
		
		firstInv.ReplaceItem(second, firstSlot);
		secondInv.ReplaceItem(first, secondSlot);
	}
/*
	public int TryAddItem(string itemId, int count = 1)
	{
		if (otherInventory != null)
		{
			var result = otherInventory.AddItem(itemId, count);
			//var result = onAddItem.Invoke(itemId, count);
			onInventoryModified?.Invoke();
			return result;
		}
		return -2;
	}
*/
	
	public int TryAddItemPlayer(string itemId, int count = 1){
		if (playerInventory != null)
		{
			var result = playerInventory.AddItem(itemId, count);
			//var result = onAddItem.Invoke(itemId, count);
			onInventoryModified?.Invoke();
			return result;
		}
		return -2;
	}
	/*
	public bool TryRemoveItem(string itemId, int count = 1, int slot = -1)
	{
		if (otherInventory != null)
		{
			var result = otherInventory.TryRemoveItem(itemId, count, slot);
			//var result = onRemoveItem.Invoke(itemId, count, slot);
			onInventoryModified?.Invoke();
			return result;
		}
		return false;
	}
	*/
	public bool TryRemoveItemPlayer(string itemId, int count = 1, int slot = -1)
	{
		if (playerInventory != null)
		{
			var result = playerInventory.TryRemoveItem(itemId, count, slot);
			//var result = onRemoveItem.Invoke(itemId, count, slot);
			onInventoryModified?.Invoke();
			return result;
		}
		return false;
	}

/*
	public string GetInventoryItem(int id)
	{
		if (otherInventory != null)
		{
			return otherInventory.GetItemInSlot(id);
			//return onGetItem.Invoke(id);
		}
		return null;
	}
*/
	
	public string GetInventoryItemPlayer(int id)
	{
		if (playerInventory != null)
		{
			return playerInventory.GetItemInSlot(id);
			//return onGetItem.Invoke(id);
		}
		return null;
	}

	public int GetHeldItem()
	{
		if(onHeldItem != null) return onHeldItem.Invoke();

		return -1;
	}
	
	public void FullRefresh()
	{
		onInventoryModified?.Invoke();
	}
	
	public void ToggleInventory()
	{
		onToggleInv?.Invoke();
	}
	
	public ContainerInventory OpenContainer(ItemContainer container)
	{
		otherInventory = container;
		return containerInventory;
	}
	
	public void CloseContainer(){
		otherInventory = null;
		containerInventory.ClearInventory();
	}
	
	public InventoryItem SelectItem(InventoryItem item)
	{
		return CursorSlot.Instance.TakeItem(item);
	}
	
	public void DropItem(InventoryItem item){
		var newGroundItem = GroundItemRequest.Instance.NewGroundItem(item.item, item.Count);
		if(newGroundItem != null && onItemDropped != null){
			onItemDropped.Invoke(newGroundItem as GroundItem);
		}
	}
}
