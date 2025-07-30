using Godot;
using System;

public partial class GroundItemRequest : Node3D
{
	[Export] private PackedScene groundItem;
	
	public static GroundItemRequest Instance;

	public override void _EnterTree()
	{
		if(Instance != null && Instance != this){
			QueueFree();
		} else 
		{
			Instance = this;
		}
	}

	
	public GroundItem NewGroundItem(ItemData item, int count){
		
		var NewItem = groundItem.Instantiate() as GroundItem;
		NewItem.Setup(item, count);
		
		return NewItem;
	}
}
