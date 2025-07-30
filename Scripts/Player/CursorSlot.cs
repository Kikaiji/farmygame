using Godot;
using System;

public partial class CursorSlot : Control
{
	public InventoryItem HeldItem;
	[Export] TextureRect Sprite;

	public static CursorSlot Instance;

	public override void _EnterTree()
	{
		if(Instance != null && Instance != this){
			QueueFree();
		} else 
		{
			Instance = this;
		}
		
		InventoryManager.onToggleInv += DropItem;
	}
	
	public override void _Ready(){
		Sprite = GetChild(0) as TextureRect;
	}
	
	public override void _ExitTree(){
		
		InventoryManager.onToggleInv -= DropItem;
	}
	
	public InventoryItem TakeItem(InventoryItem item){
		if(HeldItem == null){
			HeldItem = item;
			Refresh();
			return null;
		}
		
		var oldItem = HeldItem;
		HeldItem = item;
		Refresh();
		return oldItem;
	}
	
	public void DropItem(){
		if(HeldItem != null){
			InventoryManager.Instance.DropItem(HeldItem);
			HeldItem = null;
			Refresh();
		}
	}

	private void Refresh(){
		if(HeldItem != null){
			Sprite.Texture = HeldItem.item.itemSprite;
			return;
		}
		
		Sprite.Texture = null;
	}
	
	public override void _Process(double delta){
		Position = GetViewport().GetMousePosition();
	}
}
